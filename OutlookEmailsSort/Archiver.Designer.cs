
namespace OutlookEmailsSort
{
    partial class Archiver : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public Archiver()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabOutlookHappiness = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.buttonArchive = this.Factory.CreateRibbonButton();
            this.buttonArchiveSelect = this.Factory.CreateRibbonButton();
            this.separator1 = this.Factory.CreateRibbonSeparator();
            this.buttonLocate = this.Factory.CreateRibbonButton();
            this.separator2 = this.Factory.CreateRibbonSeparator();
            this.buttonRescan = this.Factory.CreateRibbonButton();
            this.separator3 = this.Factory.CreateRibbonSeparator();
            this.buttonSettings = this.Factory.CreateRibbonButton();
            this.tabOutlookHappiness.SuspendLayout();
            this.group1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabOutlookHappiness
            // 
            this.tabOutlookHappiness.Groups.Add(this.group1);
            this.tabOutlookHappiness.Label = "Outlook Happiness";
            this.tabOutlookHappiness.Name = "tabOutlookHappiness";
            this.tabOutlookHappiness.Position = this.Factory.RibbonPosition.AfterOfficeId("TabHome");
            // 
            // group1
            // 
            this.group1.Items.Add(this.buttonArchive);
            this.group1.Items.Add(this.buttonArchiveSelect);
            this.group1.Items.Add(this.separator1);
            this.group1.Items.Add(this.buttonLocate);
            this.group1.Items.Add(this.separator2);
            this.group1.Items.Add(this.buttonRescan);
            this.group1.Items.Add(this.separator3);
            this.group1.Items.Add(this.buttonSettings);
            this.group1.Label = "Happiness :)";
            this.group1.Name = "group1";
            // 
            // buttonArchive
            // 
            this.buttonArchive.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.buttonArchive.Image = global::OutlookEmailsSort.Properties.Resources.archive;
            this.buttonArchive.KeyTip = "A";
            this.buttonArchive.Label = "Archive!";
            this.buttonArchive.Name = "buttonArchive";
            this.buttonArchive.ScreenTip = "Archive the email according to found match";
            this.buttonArchive.ShowImage = true;
            this.buttonArchive.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonArchive_Click);
            // 
            // buttonArchiveSelect
            // 
            this.buttonArchiveSelect.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.buttonArchiveSelect.Image = global::OutlookEmailsSort.Properties.Resources.clipart2307205;
            this.buttonArchiveSelect.KeyTip = "B";
            this.buttonArchiveSelect.Label = "Archive to..";
            this.buttonArchiveSelect.Name = "buttonArchiveSelect";
            this.buttonArchiveSelect.ScreenTip = "Archive the email to the given folder";
            this.buttonArchiveSelect.ShowImage = true;
            this.buttonArchiveSelect.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonArchiveSelect_Click);
            // 
            // separator1
            // 
            this.separator1.Name = "separator1";
            // 
            // buttonLocate
            // 
            this.buttonLocate.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.buttonLocate.Image = global::OutlookEmailsSort.Properties.Resources.deadpool;
            this.buttonLocate.KeyTip = "B";
            this.buttonLocate.Label = "Locate..";
            this.buttonLocate.Name = "buttonLocate";
            this.buttonLocate.ScreenTip = "Locate the given folder in tree";
            this.buttonLocate.ShowImage = true;
            this.buttonLocate.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonLocate_Click);
            // 
            // separator2
            // 
            this.separator2.Name = "separator2";
            // 
            // buttonRescan
            // 
            this.buttonRescan.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.buttonRescan.Image = global::OutlookEmailsSort.Properties.Resources.Refresh_PNG_Image_Background;
            this.buttonRescan.KeyTip = "S";
            this.buttonRescan.Label = "Refresh";
            this.buttonRescan.Name = "buttonRescan";
            this.buttonRescan.ScreenTip = "Refresh the list of Outlook folders";
            this.buttonRescan.ShowImage = true;
            this.buttonRescan.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonRescan_Click);
            // 
            // separator3
            // 
            this.separator3.Name = "separator3";
            // 
            // buttonSettings
            // 
            this.buttonSettings.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.buttonSettings.Image = global::OutlookEmailsSort.Properties.Resources.red_matreshka_inside_icon_icon;
            this.buttonSettings.KeyTip = "S";
            this.buttonSettings.Label = "Settings..";
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.ScreenTip = "Open Add-in Settings..";
            this.buttonSettings.ShowImage = true;
            this.buttonSettings.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonSettings_Click);
            // 
            // Archiver
            // 
            this.Name = "Archiver";
            this.RibbonType = "Microsoft.Outlook.Appointment, Microsoft.Outlook.Explorer, Microsoft.Outlook.Mail" +
    ".Read, Microsoft.Outlook.Response.Read";
            this.Tabs.Add(this.tabOutlookHappiness);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Archiver_Load);
            this.tabOutlookHappiness.ResumeLayout(false);
            this.tabOutlookHappiness.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonArchive;
        internal Microsoft.Office.Tools.Ribbon.RibbonSeparator separator1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonSettings;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonArchiveSelect;
        public Microsoft.Office.Tools.Ribbon.RibbonTab tabOutlookHappiness;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonLocate;
        internal Microsoft.Office.Tools.Ribbon.RibbonSeparator separator2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonRescan;
        internal Microsoft.Office.Tools.Ribbon.RibbonSeparator separator3;
    }

    partial class ThisRibbonCollection
    {
        internal Archiver Archiver
        {
            get { return this.GetRibbon<Archiver>(); }
        }
    }
}
