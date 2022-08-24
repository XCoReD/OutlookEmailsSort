namespace SettingsUI
{
    partial class ConfigUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigUI));
            this.treeFolders = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.targetFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxEnableMoveUnreadMails = new System.Windows.Forms.CheckBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.listFolders = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.selectedTextItem = new System.Windows.Forms.TextBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // treeFolders
            // 
            this.treeFolders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeFolders.Location = new System.Drawing.Point(22, 52);
            this.treeFolders.Name = "treeFolders";
            this.treeFolders.Size = new System.Drawing.Size(286, 325);
            this.treeFolders.TabIndex = 0;
            this.treeFolders.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeFolders_AfterSelect);
            this.treeFolders.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeFolders_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Target folder:";
            // 
            // targetFolder
            // 
            this.targetFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.targetFolder.Location = new System.Drawing.Point(22, 26);
            this.targetFolder.Name = "targetFolder";
            this.targetFolder.ReadOnly = true;
            this.targetFolder.Size = new System.Drawing.Size(286, 20);
            this.targetFolder.TabIndex = 2;
            this.targetFolder.TextChanged += new System.EventHandler(this.targetFolder_TextChanged);
            this.targetFolder.Enter += new System.EventHandler(this.targetFolder_Enter);
            this.targetFolder.KeyDown += new System.Windows.Forms.KeyEventHandler(this.targetFolder_KeyDown);
            this.targetFolder.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.targetFolder_KeyPress);
            this.targetFolder.Leave += new System.EventHandler(this.targetFolder_Leave);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(147, 380);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "(select existing folder as a target)";
            // 
            // checkBoxEnableMoveUnreadMails
            // 
            this.checkBoxEnableMoveUnreadMails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxEnableMoveUnreadMails.AutoSize = true;
            this.checkBoxEnableMoveUnreadMails.Location = new System.Drawing.Point(22, 409);
            this.checkBoxEnableMoveUnreadMails.Name = "checkBoxEnableMoveUnreadMails";
            this.checkBoxEnableMoveUnreadMails.Size = new System.Drawing.Size(266, 17);
            this.checkBoxEnableMoveUnreadMails.TabIndex = 4;
            this.checkBoxEnableMoveUnreadMails.Text = "reset unread flag when move (single selection only)";
            this.checkBoxEnableMoveUnreadMails.UseVisualStyleBackColor = true;
            this.checkBoxEnableMoveUnreadMails.Visible = false;
            this.checkBoxEnableMoveUnreadMails.CheckedChanged += new System.EventHandler(this.checkBoxEnableMoveUnreadMails_CheckedChanged);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonOK.Location = new System.Drawing.Point(336, 22);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(336, 52);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // listFolders
            // 
            this.listFolders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listFolders.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listFolders.HideSelection = false;
            this.listFolders.Location = new System.Drawing.Point(22, 52);
            this.listFolders.Name = "listFolders";
            this.listFolders.Size = new System.Drawing.Size(286, 325);
            this.listFolders.TabIndex = 7;
            this.listFolders.UseCompatibleStateImageBehavior = false;
            this.listFolders.Visible = false;
            this.listFolders.SelectedIndexChanged += new System.EventHandler(this.listFolders_SelectedIndexChanged);
            this.listFolders.DoubleClick += new System.EventHandler(this.listFolders_DoubleClick);
            this.listFolders.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listFolders_KeyDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            // 
            // selectedTextItem
            // 
            this.selectedTextItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectedTextItem.Enabled = false;
            this.selectedTextItem.Location = new System.Drawing.Point(22, 407);
            this.selectedTextItem.Name = "selectedTextItem";
            this.selectedTextItem.ReadOnly = true;
            this.selectedTextItem.Size = new System.Drawing.Size(389, 20);
            this.selectedTextItem.TabIndex = 8;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(336, 363);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(70, 13);
            this.linkLabel1.TabIndex = 9;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Contact me :)";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // ConfigUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 439);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.selectedTextItem);
            this.Controls.Add(this.listFolders);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.checkBoxEnableMoveUnreadMails);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.targetFolder);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeFolders);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(454, 478);
            this.Name = "ConfigUI";
            this.Text = "Outlook Happiness";
            this.Load += new System.EventHandler(this.ConfigUI_Load);
            this.Click += new System.EventHandler(this.buttonOK_Click);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ConfigUI_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeFolders;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox targetFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBoxEnableMoveUnreadMails;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ListView listFolders;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.TextBox selectedTextItem;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}