using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SettingsUI
{
    public partial class ConfigUI : Form
    {
        public Config _settings;
        OutlookTreeNode _root;
        Dictionary<string, FolderInfo> _folders;
        List<string> _foldersPlain;
        TreeNode _selectedNode;
        bool _initInProgress = true;
        bool _disableListFiltering = false;
        InputLanguage _original;
        public ConfigUI(Config settings, OutlookTreeNode root, Dictionary<string, FolderInfo> folders)
        {
            _settings = settings;
            _root = root;
            _folders = folders;
            InitializeComponent();
        }

        private void ConfigUI_Load(object sender, EventArgs e)
        {
            if (_root != null)
            {
                treeFolders.Visible = true;
                listFolders.Visible = false;
                targetFolder.ReadOnly = true;

                targetFolder.Text = _settings.OutlookTargetFolder;
                checkBoxEnableMoveUnreadMails.Checked = _settings.MarkAsReadWhenMove;

                FillTree(treeFolders.Nodes, _root, "");
                treeFolders.ExpandAll();
                if (_selectedNode != null)
                    treeFolders.SelectedNode = _selectedNode;

                checkBoxEnableMoveUnreadMails.Visible = true;
                selectedTextItem.Visible = false;
            }
            else
            {
                listFolders.Visible = true;
                treeFolders.Visible = false;
                targetFolder.ReadOnly = false;

                _foldersPlain = new List<string>(_folders.Count);
                foreach (var v in _folders.Keys)
                    _foldersPlain.Add(v);
                _foldersPlain.Sort();

                FillFolders(null);
                targetFolder.Focus();


                checkBoxEnableMoveUnreadMails.Visible = false;
                selectedTextItem.Visible = true;

            }
        }

        public string SelectedFolder
        {
            get
            {
                return targetFolder.Text;
            }
        }

        void FillFolders(string beginning)
        {
            listFolders.Clear();
            bool anyPlace = !string.IsNullOrEmpty(beginning) && beginning.First() == '*';
            if (anyPlace)
                beginning = beginning.TrimStart('*');
            foreach (var s in _foldersPlain)
            {
                if (!string.IsNullOrEmpty(beginning))
                {
                    if(anyPlace)
                    {
                        if (s.IndexOf(beginning) == -1)
                            continue;
                    }
                    else
                    {
                        if (!s.StartsWith(beginning))
                            continue;
                    }
                }
                listFolders.Items.Add(s);
            }
        }
        void FillTree(TreeNodeCollection nodes, OutlookTreeNode _source, string parentPath)
        {
            TreeNode root = nodes.Add(_source.Name);
            parentPath = string.IsNullOrEmpty(parentPath) ? _source.Name : parentPath + "\\" + _source.Name;

            if (parentPath == targetFolder.Text)
            {
                _selectedNode = root;
            }

            if (_source.Children != null)
            {
                foreach (var sc in _source.Children)
                {
                    FillTree(root.Nodes, sc, parentPath);
                }
            }
        }

        bool FolderSelectMode
        {
            get
            {
                return _root == null;
            }
        }
        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (!FolderSelectMode)
            {
                _settings.OutlookTargetFolder = targetFolder.Text;
                _settings.MarkAsReadWhenMove = checkBoxEnableMoveUnreadMails.Checked;
            }
        }

        private void treeFolders_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (_initInProgress)
            {
                _initInProgress = false;
                return;
            }
            var s = treeFolders.SelectedNode;
            if (s != null)
            {
                string text = s.Text;
                while (s.Parent != null)
                {
                    text = s.Parent.Text + "\\" + text;
                    s = s.Parent;
                }
                targetFolder.Text = text;
            }
        }

        private void listFolders_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectListItem();
        }

        bool SelectListItem()
        {
            if (this.listFolders.SelectedItems.Count != 1)
                return false;

            string name = this.listFolders.SelectedItems[0].Text;
            _disableListFiltering = true;
            targetFolder.Text = name;
            selectedTextItem.Text = _folders[name].Path;
            _disableListFiltering = false;

            return true;
        }

        private void targetFolder_TextChanged(object sender, EventArgs e)
        {
            if (_disableListFiltering)
                return;

            if (FolderSelectMode)
                FillFolders(targetFolder.Text.ToLowerInvariant().RemoveWhitespaces());
        }

        private void listFolders_DoubleClick(object sender, EventArgs e)
        {
            if (SelectListItem())
            {
                Close();
                DialogResult = DialogResult.OK;
            }
        }

        private void targetFolder_Enter(object sender, EventArgs e)
        {
            _original = InputLanguage.CurrentInputLanguage;
            var culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            var language = InputLanguage.FromCulture(culture);
            if (InputLanguage.InstalledInputLanguages.IndexOf(language) >= 0)
                InputLanguage.CurrentInputLanguage = language;
            else
                InputLanguage.CurrentInputLanguage = InputLanguage.DefaultInputLanguage;
        }

        private void targetFolder_Leave(object sender, EventArgs e)
        {
            InputLanguage.CurrentInputLanguage = _original;
        }

        private void targetFolder_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void targetFolder_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter)
            {
                if (FolderSelectMode)
                {
                    if (listFolders.Items.Count == 1)
                    {
                        var s = listFolders.Items[0].Text;
                        _disableListFiltering = true;
                        targetFolder.Text = s;

                        Close();
                        DialogResult = DialogResult.OK;
                    }
                    else if (listFolders.Items.Count > 1)
                    {
                        listFolders.Items[0].Selected = true;
                        listFolders.Select();
                    }
                }
            }
            else if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void listFolders_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (SelectListItem())
                {
                    Close();
                    DialogResult = DialogResult.OK;
                }
            }
        }

        private void treeFolders_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if(!string.IsNullOrEmpty(targetFolder.Text))
                {
                    _settings.OutlookTargetFolder = targetFolder.Text;
                    Close();
                    DialogResult = DialogResult.OK;
                }
            }
        }

        private void ConfigUI_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void checkBoxEnableMoveUnreadMails_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.linkedin.com/in/dtrus/");
        }
    }
}
