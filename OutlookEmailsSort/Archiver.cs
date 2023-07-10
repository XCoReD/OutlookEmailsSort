using Microsoft.Office.Interop.Outlook;
using Microsoft.Office.Tools.Ribbon;
using Newtonsoft.Json;
using SettingsUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;


namespace OutlookEmailsSort
{
    public partial class Archiver
    {
        static Config _settings;
        static string _settingsFileName;
        static Dictionary<string, FolderInfo> _folders;
        static List<string> _foldersPlain;

        static Dictionary<string, string> _senderHistory;
        static string _senderHistoryFileName;

        static HashSet<string> _exactMailFolders;
        static char[] _extraCharsInFolderName = new char[] { '(', '[' };
        static char[] _addressesSeparators = new char[] { ';',',' };
        static StringBuilder _error = new StringBuilder();

        static List<string> _domainsToSkip = new List<string> { "scnsoft", "scnvision", "gmail", "yahoo", "hotmail", "msn", "outlook", "mail", "yandex" };

        const string PR_SMTP_ADDRESS =
    "http://schemas.microsoft.com/mapi/proptag/0x39FE001E";


        private void Archiver_Load(object sender, RibbonUIEventArgs e)
        {
            if (_settings == null)
            {
                LoadSettings();
            }
        }

        static void LoadSettings()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            _settingsFileName = path + "\\.outlookemailsort.settings.json";
            try
            {
                var s = File.ReadAllText(_settingsFileName);
                _settings = JsonConvert.DeserializeObject<Config>(s);

                if(_settings.OutlookTargetFolder != "Inbox")
                {
                    _folders = new Dictionary<string, FolderInfo>();
                    _exactMailFolders = new HashSet<string>();
                    InitFolders(_settings.OutlookTargetFolder);

                    _foldersPlain = new List<string>(_folders.Count);
                    foreach (var v in _folders)
                        _foldersPlain.Add(v.Key);
                }
            }
            catch (System.Exception)
            {
                _settings = GetDefaultSettings();
            }

            _senderHistoryFileName = path + "\\.outlookemailsort.senderhistory.json";
            try
            {
                var s = File.ReadAllText(_senderHistoryFileName);
                _senderHistory = JsonConvert.DeserializeObject<Dictionary<string, string>>(s);
            }
            catch (System.Exception)
            {
                _senderHistory = new Dictionary<string, string>();
            }

        }

        static Config GetDefaultSettings()
        {
            return new Config { MarkAsReadWhenMove = true, OutlookTargetFolder = "Inbox" };
        }
        void SaveSettings()
        {
            string json = JsonConvert.SerializeObject(_settings, Formatting.Indented);
            File.WriteAllText(_settingsFileName, json);
        }

        void SaveHistory()
        {
            string json = JsonConvert.SerializeObject(_senderHistory, Formatting.Indented);
            File.WriteAllText(_senderHistoryFileName, json);
        }

        static void InitFolders(string startPath)
        {
            var inbox = Globals.ThisAddIn.Application.ActiveExplorer().Session.
                GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox);

            PopulateFoldersDictionary(startPath, null, inbox, false);

            if(_error.Length != 0)
            {
                MessageBox.Show("Please resolve the issues below" + Environment.NewLine + Environment.NewLine + _error.ToString(), "Duplicated names found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


        static void PopulateFoldersDictionary(string startPath, string parent, MAPIFolder folder, bool includes)
        {
            var folderName = folder.Name;
            var fullPath = string.IsNullOrEmpty(parent) ? folderName : parent + "\\" + folderName;
            folderName = folderName.ToLowerInvariant().Trim();
            if (!includes)
                includes = fullPath.IndexOf(startPath) == 0;

            if (includes)
            {
                int ic = folderName.IndexOfAny(_extraCharsInFolderName);
                if (ic != -1)
                    folderName = folderName.Substring(0, ic).TrimEnd();

                string[] items = folderName.Split('/');
                foreach(var v in items)
                {
                    var name = v.RemoveWhitespaces();
                    if(!string.IsNullOrEmpty(name))
                    {

                        bool isMail = name.IndexOf('@') != -1 && IsValidEmail(name);
                        if (!isMail)
                        {
                            name = name.RemoveDomain1stLevel();
                        }
                        else
                        {
                            try
                            {
                                _exactMailFolders.Add(name);
                            }
                            catch (ArgumentException ex)
                            {
                                //will fire on next step
                            }
                        }

                        var fi = new FolderInfo { Path = fullPath, EntryID = folder.EntryID };
                        try
                        {

                            _folders.Add(name, fi);
                        }
                        catch (ArgumentException ex)
                        {
                            var existingFi = _folders[name];
                            Debug.WriteLine($"PopulateFoldersDictionary: {name} already exists" + ex.Message);
                            _error.AppendLine($"Ambiguous folder name found: [{name}] found in {fullPath} and {existingFi.Path}");
                        }
                    }

                }

            }

            if (folder.Folders != null && folder.Folders.Count != 0)
            {
                foreach (MAPIFolder subfolder in folder.Folders)
                {
                    PopulateFoldersDictionary(startPath, fullPath, subfolder, includes);
                }
            }
        }

        delegate bool ArchiveProc(MailItem mail, bool enableMovingUnread);
        private void buttonArchive_Click(object sender, RibbonControlEventArgs e)
        {
            DoArchive(Archive);
        }

        void DoArchive(ArchiveProc p)
        {
            var aw = Globals.ThisAddIn.Application.ActiveWindow();
            if (aw is Microsoft.Office.Interop.Outlook.Explorer)
            {
                Microsoft.Office.Interop.Outlook.Explorer ex = aw;
                if (ex.Selection != null)
                {
                    int total = ex.Selection.Count;
                    foreach (var mail in ex.Selection)
                    {
                        p(mail as MailItem, total == 1);
                    }
                }
                else
                {
                    var folder = ex.CurrentFolder;
                    foreach (var mail in folder.Items)
                    {
                        p(mail as MailItem, false);
                    }
                }
            }
            else if (aw is Microsoft.Office.Interop.Outlook.Inspector)
            {
                Microsoft.Office.Interop.Outlook.Inspector ex = aw;
                var ci = ex.CurrentItem;
                if (ci is Outlook.MailItem)
                {
                    p(ci as MailItem, true);
                }
            }
        }

        private string GetSenderEmailAddress(Outlook.MailItem mail)
        {
            Outlook.AddressEntry sender = mail.Sender;
            string SenderEmailAddress = "";

            if (sender.AddressEntryUserType == Outlook.OlAddressEntryUserType.olExchangeUserAddressEntry || sender.AddressEntryUserType == Outlook.OlAddressEntryUserType.olExchangeRemoteUserAddressEntry)
            {
                Outlook.ExchangeUser exchUser = sender.GetExchangeUser();
                if (exchUser != null)
                {
                    SenderEmailAddress = exchUser.PrimarySmtpAddress;
                }
            }
            else
            {
                SenderEmailAddress = mail.SenderEmailAddress;
            }

            return SenderEmailAddress;
        }

        string SelectFolder()
        {
            using(ConfigUI form = new ConfigUI(_settings, null, _folders))
            {
                if (form.ShowDialog() != DialogResult.OK)
                    return null;

                return form.SelectedFolder;
            }
        }
        bool ArchiveToSelectedFolder(MailItem mail, bool enableMovingUnread)
        {
            if (!CanBeArchived(mail, enableMovingUnread))
                return false;

            var s = SelectFolder();
            if (string.IsNullOrEmpty(s))
                return false;

            FolderInfo identifiedFolder = null;
            if (!_folders.TryGetValue(s, out identifiedFolder))
            {
                Debug.Assert(false);
                return false;
            }

            if (mail.UnRead)
                mail.UnRead = false;

            if (MoveToFolder(mail, identifiedFolder, true))
                return false;

            return true;
        }

        bool CanBeArchived(MailItem mail, bool enableMovingUnread)
        {
            bool unread = mail.UnRead;
            if (unread && !enableMovingUnread)
                return false;

            Outlook.MAPIFolder parent = mail.Parent;
            string s = parent.FullFolderPath;

            if (s.IndexOf(_settings.OutlookTargetFolder) == 0)
                return false;   //already archived

            return true;

        }

        enum KeywordProcessingMode
        {
            AsIs,
            RemoveNonAlpha
        }

        enum FindFolderResult
        {
            FoundOne,
            FoundMultiple,
            NotFound
        }
        FindFolderResult FindFolder(HashSet<string> keywords, KeywordProcessingMode mode, ref FolderInfo identifiedFolder)
        {
            Debug.Assert(identifiedFolder == null);

            foreach (var k in keywords)
            {
                FolderInfo fi;
                string key = mode == KeywordProcessingMode.AsIs ? k : k.RemoveNonAlpha();
                if (_folders.TryGetValue(k, out fi))
                {
                    if (identifiedFolder != null)
                    {
                        if (identifiedFolder.Path != fi.Path)
                        {
                            Debug.WriteLine($"Archive: multiple keys found: ({identifiedFolder.Path}) and ({fi.Path}) - no move possible");
                            return FindFolderResult.FoundMultiple;
                        }
                    }
                    identifiedFolder = fi;
                }
            }
            return identifiedFolder != null ? FindFolderResult.FoundOne : FindFolderResult.NotFound;
        }

        FindFolderResult FindFolder(HashSet<string> keywords, ref FolderInfo identifiedFolder)
        {
            var r = FindFolder(keywords, KeywordProcessingMode.AsIs, ref identifiedFolder);
            if (r == FindFolderResult.FoundMultiple)
                return r;
            if (r == FindFolderResult.NotFound)
            {
                //second try, with removing dashes
                r = FindFolder(keywords, KeywordProcessingMode.RemoveNonAlpha, ref identifiedFolder);
            }
            return r;
        }

        FindFolderResult GetFolderBySenderOrRecipientsOrSubject(MailItem mail, ref FolderInfo identifiedFolder)
        {
            HashSet<string> keywords = new HashSet<string>();

            var sender = GetSenderEmailAddress(mail);
            FillFromAddress(keywords, sender);
            foreach (Recipient rc in mail.Recipients)
            {
                Outlook.PropertyAccessor pa = rc.PropertyAccessor;
                string smtpAddress = pa.GetProperty(PR_SMTP_ADDRESS).ToString();
                FillFromAddress(keywords, smtpAddress);
            }

            FillFromSubject(keywords, mail.Subject);

            var r = FindFolder(keywords, KeywordProcessingMode.AsIs, ref identifiedFolder);
            if (r == FindFolderResult.FoundMultiple)
                return r;
            if (r == FindFolderResult.NotFound)
            {
                //second try, with removing dashes
                r = FindFolder(keywords, KeywordProcessingMode.RemoveNonAlpha, ref identifiedFolder);
            }

            return r;
        }

        bool Archive(MailItem mail, bool enableMovingUnread)
        {
            if (!CanBeArchived(mail, enableMovingUnread))
                return false;

            FolderInfo identifiedFolder = null;
            var r = GetFolderBySenderOrRecipientsOrSubject(mail, ref identifiedFolder);
            if(r == FindFolderResult.FoundMultiple)
                return false;
            if (r == FindFolderResult.NotFound)
            {
                //next try, now with checking senders and recipients in email chain
                r = CheckInEmailChain(mail, ref identifiedFolder);
                if(r == FindFolderResult.FoundMultiple)
                    return false;
            }

            if (r == FindFolderResult.NotFound)
            {
                //try to find in history
                var sender = GetSenderEmailAddress(mail);

                string historyFolder = null;
                if (_senderHistory.TryGetValue(sender, out historyFolder))
                {
                    _folders.TryGetValue(historyFolder.ToLowerInvariant(), out identifiedFolder);
                }
            }

            if (identifiedFolder == null)
                return false;

            if (mail.UnRead)
                mail.UnRead = false;

            if (!MoveToFolder(mail, identifiedFolder, false))
                return false;

            return true;
        }

        FindFolderResult CheckInEmailChain(MailItem mail, ref FolderInfo identifiedFolder)
        {
            //first - try conversation approach
            //https://docs.microsoft.com/en-us/office/client-developer/outlook/pia/how-to-get-and-display-items-in-a-conversation

            FindFolderResult r = FindFolderResult.NotFound;
            Outlook.Folder folder = mail.Parent as Outlook.Folder;
            Outlook.Store store = folder.Store;
            if (store.IsConversationEnabled == true)
            {
                // Obtain a Conversation object.
                Outlook.Conversation conv = mail.GetConversation();
                if (conv != null)
                {
                    Outlook.SimpleItems simpleItems = conv.GetRootItems();
                    foreach (object item in simpleItems)
                    {
                        // enumerate only MailItem type.
                        if (item is Outlook.MailItem)
                        {
                            Outlook.MailItem mailInChain = item as Outlook.MailItem;
                            r = FindInConversation(mail, mailInChain, conv, ref identifiedFolder);
                            if (r != FindFolderResult.NotFound)
                                break;
                        }
                    }
                }
            }
            if(r == FindFolderResult.NotFound)
            {
                r = WalkThroughMessageBody(mail, ref identifiedFolder);
            }
            return r;
        }

        FindFolderResult WalkThroughMessageBody(Outlook.MailItem mail, ref FolderInfo identifiedFolder)
        {
            //walk through html message
            HashSet<string> keywords = new HashSet<string>();

            string[] seps = new string[] { Environment.NewLine };
            string[] lines = mail.Body.Split(seps, StringSplitOptions.RemoveEmptyEntries);
            string subj = "Subject:";
            string[] titles = new string[] { "From:", "To:", "Cc:", subj };
            foreach (var line in lines)
            {
                foreach (var t in titles)
                {
                    var foundIndex = line.IndexOf(t, StringComparison.InvariantCultureIgnoreCase);
                    if (foundIndex != -1)
                    {
                        var value = line.Substring(foundIndex + t.Length);
                        if (t == subj)
                            FillFromSubject(keywords, value);
                        else
                            FillFromAddress(keywords, value);
                        break;
                    }
                }
            }
            return FindFolder(keywords, ref identifiedFolder);
        }
        FindFolderResult FindInConversation(Outlook.MailItem startMail, Outlook.MailItem conversationMail, Outlook.Conversation conv, ref FolderInfo identifiedFolder)
        {
            FindFolderResult r = FindFolderResult.NotFound;
            string entryId = conversationMail.EntryID;
            string currentIntryId = startMail.EntryID;
            if (entryId != currentIntryId)
            {
                r = GetFolderBySenderOrRecipientsOrSubject(conversationMail, ref identifiedFolder);
                if (r != FindFolderResult.NotFound)
                    return r;
            }

            Outlook.SimpleItems items = conv.GetChildren(conversationMail);
            if (items.Count > 0)
            {
                foreach (object myItem in items)
                {
                    if (myItem is Outlook.MailItem)
                    {
                        Outlook.MailItem mailInChain = myItem as Outlook.MailItem;
                        r = FindInConversation(startMail, mailInChain, conv, ref identifiedFolder);
                        if (r != FindFolderResult.NotFound)
                            return r;
                    }
                }
            }
            return r;
        }

        bool MoveToFolder(MailItem mail, FolderInfo folder, bool saveSenderToHistory)
        {
            Outlook.MAPIFolder inbox = Globals.ThisAddIn.Application.ActiveExplorer().Session.
                GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox);
            string folderStoreID = inbox.StoreID;

            MAPIFolder target = null;
            try
            {
                Outlook.NameSpace mapiNameSpace = Globals.ThisAddIn.Application.GetNamespace("MAPI");
                target = (Outlook.MAPIFolder)mapiNameSpace.GetFolderFromID(folder.EntryID, folderStoreID);
            }
            catch
            {
                MessageBox.Show("There is no folder named " + folder.Path);
                return false;
            }

            mail.Move(target);

            if(saveSenderToHistory)
            {
                var sender = GetSenderEmailAddress(mail);
                try
                {
                    _senderHistory[sender] = folder.Name;
                    SaveHistory();
                }
                catch (System.Exception ex)
                {
                }
            }

            return true;
        }
        void FillFromSubject(HashSet<string> keywords, string subject)
        {
            int i = subject.IndexOf('[');
            if (i == -1)
                return;

            int j = subject.IndexOf(']', i + 1);
            if (j == -1)
                return;

            var key = subject.Substring(i + 1, j - i - 1).ToLowerInvariant().RemoveDomain1stLevel().RemoveWhitespaces();
            if (keywords.Contains(key))
                return;
            keywords.Add(key);
        }

        void FillFromAddress(HashSet<string> keywords, string addressLine)
        {
            string[] addresses = addressLine.Split(_addressesSeparators);
            const string mailto = "mailto:";
            char[] mailtoEnding = { '>'};
            foreach (var addressItem in addresses)
            {
                var address = addressItem.Trim();

                bool mailtoCut = false;
                int mt = address.IndexOf(mailto);
                if(mt != -1)
                {
                    int mtend = address.IndexOfAny(mailtoEnding);
                    if(mtend != -1)
                    {
                        address = address.Substring(mt + mailto.Length, mtend - mt - mailto.Length);
                        mailtoCut = true;
                    }
                }

                if(!mailtoCut)
                {
                    int lq = address.IndexOf('<');
                    if (lq != -1)
                    {
                        int rq = address.IndexOf('>', lq + 1);
                        if (lq != -1 && rq != -1)
                            address = address.Substring(lq + 1, rq - lq - 1);
                    }
                }

                int a = address.IndexOf('@');
                if (a == -1)
                    return;

                int b = address.IndexOf('.', a + 1);
                if (b == -1)
                    return;
                string domain = address.Substring(a + 1, b - a - 1).ToLowerInvariant();
                if (_domainsToSkip.Contains(domain))
                    return;

                string key = null;
                if (!_exactMailFolders.Contains(address))
                {
                    key = address.Substring(a + 1).RemoveDomain1stLevel();
                }
                else
                    key = address;

                if (!keywords.Contains(key))
                    keywords.Add(key);
            }

        }

        private void buttonSettings_Click(object sender, RibbonControlEventArgs e)
        {
            LoadSettings();

            OutlookTreeNode outlookTree = ReadFolders();
            ConfigUI form = new ConfigUI(_settings, outlookTree, null);
            if(form.ShowDialog() == DialogResult.OK)
            {
                SaveSettings();
            }
        }

        OutlookTreeNode ReadFolders()
        {
            var inbox = Globals.ThisAddIn.Application.ActiveExplorer().Session.
                GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox);

            OutlookTreeNode result = new OutlookTreeNode();
            Populate(result, inbox);

            return result;
        }

        void Populate(OutlookTreeNode result, MAPIFolder folder)
        {
            result.Name = folder.Name;

            if (folder.Folders != null && folder.Folders.Count != 0)
            {
                result.Children = new List<OutlookTreeNode>();

                foreach (MAPIFolder subfolder in folder.Folders)
                {
                    OutlookTreeNode child = new OutlookTreeNode();
                    Populate(child, subfolder);
                    result.Children.Add(child);
                }
            }
        }

        private void buttonArchiveSelect_Click(object sender, RibbonControlEventArgs e)
        {
            DoArchive(ArchiveToSelectedFolder);
        }

        private void buttonLocate_Click(object sender, RibbonControlEventArgs e)
        {
            var aw = Globals.ThisAddIn.Application.ActiveWindow();
            if (aw is Microsoft.Office.Interop.Outlook.Explorer)
            {
                Microsoft.Office.Interop.Outlook.Explorer ex = aw;

                var s = SelectFolder();
                if (string.IsNullOrEmpty(s))
                    return;

                FolderInfo identifiedFolder = null;
                if (!_folders.TryGetValue(s, out identifiedFolder))
                {
                    Debug.Assert(false);
                    return;
                }

                Outlook.MAPIFolder inbox = Globals.ThisAddIn.Application.ActiveExplorer().Session.
                    GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox);
                string folderStoreID = inbox.StoreID;



                Outlook.NameSpace mapiNameSpace = Globals.ThisAddIn.Application.GetNamespace("MAPI");
                var target = (Outlook.MAPIFolder)mapiNameSpace.GetFolderFromID(identifiedFolder.EntryID, folderStoreID);

                ex.CurrentFolder = target;
            }
        }

        private void buttonRescan_Click(object sender, RibbonControlEventArgs e)
        {
            LoadSettings();
        }
    }
}
