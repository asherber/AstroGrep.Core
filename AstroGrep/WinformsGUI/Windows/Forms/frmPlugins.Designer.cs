namespace AstroGrep.Windows.Forms
{
   partial class frmPlugins
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
         this.btnCancel = new System.Windows.Forms.Button();
         this.btnOK = new System.Windows.Forms.Button();
         this.btnDown = new System.Windows.Forms.Button();
         this.PluginDetailsGroup = new System.Windows.Forms.GroupBox();
         this.lblPluginDescription = new System.Windows.Forms.Label();
         this.lblPluginAuthor = new System.Windows.Forms.Label();
         this.lblPluginVersion = new System.Windows.Forms.Label();
         this.lblPluginName = new System.Windows.Forms.Label();
         this.lblPluginDetailAuthor = new System.Windows.Forms.Label();
         this.lblPluginDetailVersion = new System.Windows.Forms.Label();
         this.lblPluginDetailName = new System.Windows.Forms.Label();
         this.PluginsList = new System.Windows.Forms.ListView();
         this.PluginsColumnEnabled = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.PluginsColumnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.PluginsColumnExt = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.btnUp = new System.Windows.Forms.Button();
         this.PluginDetailsGroup.SuspendLayout();
         this.SuspendLayout();
         // 
         // btnCancel
         // 
         this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.btnCancel.Location = new System.Drawing.Point(437, 374);
         this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
         this.btnCancel.Name = "btnCancel";
         this.btnCancel.Size = new System.Drawing.Size(100, 25);
         this.btnCancel.TabIndex = 3;
         this.btnCancel.Text = "&Cancel";
         this.btnCancel.UseVisualStyleBackColor = true;
         this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
         // 
         // btnOK
         // 
         this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.btnOK.Location = new System.Drawing.Point(329, 374);
         this.btnOK.Margin = new System.Windows.Forms.Padding(4);
         this.btnOK.Name = "btnOK";
         this.btnOK.Size = new System.Drawing.Size(100, 25);
         this.btnOK.TabIndex = 2;
         this.btnOK.Text = "&OK";
         this.btnOK.UseVisualStyleBackColor = true;
         this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
         // 
         // btnDown
         // 
         this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.btnDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.btnDown.Image = global::AstroGrep.Properties.Resources.bullet_arrow_down;
         this.btnDown.Location = new System.Drawing.Point(504, 46);
         this.btnDown.Name = "btnDown";
         this.btnDown.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
         this.btnDown.Size = new System.Drawing.Size(34, 28);
         this.btnDown.TabIndex = 6;
         this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
         // 
         // PluginDetailsGroup
         // 
         this.PluginDetailsGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.PluginDetailsGroup.Controls.Add(this.lblPluginDescription);
         this.PluginDetailsGroup.Controls.Add(this.lblPluginAuthor);
         this.PluginDetailsGroup.Controls.Add(this.lblPluginVersion);
         this.PluginDetailsGroup.Controls.Add(this.lblPluginName);
         this.PluginDetailsGroup.Controls.Add(this.lblPluginDetailAuthor);
         this.PluginDetailsGroup.Controls.Add(this.lblPluginDetailVersion);
         this.PluginDetailsGroup.Controls.Add(this.lblPluginDetailName);
         this.PluginDetailsGroup.Location = new System.Drawing.Point(12, 236);
         this.PluginDetailsGroup.Name = "PluginDetailsGroup";
         this.PluginDetailsGroup.Size = new System.Drawing.Size(525, 121);
         this.PluginDetailsGroup.TabIndex = 7;
         this.PluginDetailsGroup.TabStop = false;
         this.PluginDetailsGroup.Text = "Plugin Details";
         // 
         // lblPluginDescription
         // 
         this.lblPluginDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.lblPluginDescription.Location = new System.Drawing.Point(272, 24);
         this.lblPluginDescription.Name = "lblPluginDescription";
         this.lblPluginDescription.Size = new System.Drawing.Size(245, 88);
         this.lblPluginDescription.TabIndex = 3;
         // 
         // lblPluginAuthor
         // 
         this.lblPluginAuthor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.lblPluginAuthor.Location = new System.Drawing.Point(96, 88);
         this.lblPluginAuthor.Name = "lblPluginAuthor";
         this.lblPluginAuthor.Size = new System.Drawing.Size(156, 23);
         this.lblPluginAuthor.TabIndex = 2;
         // 
         // lblPluginVersion
         // 
         this.lblPluginVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.lblPluginVersion.Location = new System.Drawing.Point(96, 56);
         this.lblPluginVersion.Name = "lblPluginVersion";
         this.lblPluginVersion.Size = new System.Drawing.Size(156, 23);
         this.lblPluginVersion.TabIndex = 6;
         // 
         // lblPluginName
         // 
         this.lblPluginName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.lblPluginName.Location = new System.Drawing.Point(96, 24);
         this.lblPluginName.Name = "lblPluginName";
         this.lblPluginName.Size = new System.Drawing.Size(156, 23);
         this.lblPluginName.TabIndex = 5;
         // 
         // lblPluginDetailAuthor
         // 
         this.lblPluginDetailAuthor.Location = new System.Drawing.Point(16, 88);
         this.lblPluginDetailAuthor.Name = "lblPluginDetailAuthor";
         this.lblPluginDetailAuthor.Size = new System.Drawing.Size(80, 23);
         this.lblPluginDetailAuthor.TabIndex = 7;
         this.lblPluginDetailAuthor.Text = "Author:";
         // 
         // lblPluginDetailVersion
         // 
         this.lblPluginDetailVersion.Location = new System.Drawing.Point(16, 56);
         this.lblPluginDetailVersion.Name = "lblPluginDetailVersion";
         this.lblPluginDetailVersion.Size = new System.Drawing.Size(80, 23);
         this.lblPluginDetailVersion.TabIndex = 1;
         this.lblPluginDetailVersion.Text = "Version:";
         // 
         // lblPluginDetailName
         // 
         this.lblPluginDetailName.Location = new System.Drawing.Point(16, 24);
         this.lblPluginDetailName.Name = "lblPluginDetailName";
         this.lblPluginDetailName.Size = new System.Drawing.Size(80, 23);
         this.lblPluginDetailName.TabIndex = 0;
         this.lblPluginDetailName.Text = "Name:";
         // 
         // PluginsList
         // 
         this.PluginsList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.PluginsList.CheckBoxes = true;
         this.PluginsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PluginsColumnEnabled,
            this.PluginsColumnName,
            this.PluginsColumnExt});
         this.PluginsList.FullRowSelect = true;
         this.PluginsList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
         this.PluginsList.HideSelection = false;
         this.PluginsList.Location = new System.Drawing.Point(12, 12);
         this.PluginsList.MultiSelect = false;
         this.PluginsList.Name = "PluginsList";
         this.PluginsList.Size = new System.Drawing.Size(485, 206);
         this.PluginsList.TabIndex = 4;
         this.PluginsList.UseCompatibleStateImageBehavior = false;
         this.PluginsList.View = System.Windows.Forms.View.Details;
         this.PluginsList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.PluginsList_ItemCheck);
         this.PluginsList.SelectedIndexChanged += new System.EventHandler(this.PluginsList_SelectedIndexChanged);
         // 
         // PluginsColumnEnabled
         // 
         this.PluginsColumnEnabled.Text = "Enabled";
         this.PluginsColumnEnabled.Width = 72;
         // 
         // PluginsColumnName
         // 
         this.PluginsColumnName.Text = "Name";
         this.PluginsColumnName.Width = 246;
         // 
         // PluginsColumnExt
         // 
         this.PluginsColumnExt.Text = "Extensions";
         this.PluginsColumnExt.Width = 134;
         // 
         // btnUp
         // 
         this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.btnUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.btnUp.Image = global::AstroGrep.Properties.Resources.bullet_arrow_up;
         this.btnUp.Location = new System.Drawing.Point(504, 12);
         this.btnUp.Name = "btnUp";
         this.btnUp.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
         this.btnUp.Size = new System.Drawing.Size(34, 28);
         this.btnUp.TabIndex = 5;
         this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
         // 
         // frmPlugins
         // 
         this.AcceptButton = this.btnOK;
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.CancelButton = this.btnCancel;
         this.ClientSize = new System.Drawing.Size(550, 412);
         this.Controls.Add(this.btnDown);
         this.Controls.Add(this.btnUp);
         this.Controls.Add(this.PluginDetailsGroup);
         this.Controls.Add(this.PluginsList);
         this.Controls.Add(this.btnCancel);
         this.Controls.Add(this.btnOK);
         this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.MinimumSize = new System.Drawing.Size(566, 451);
         this.Name = "frmPlugins";
         this.ShowIcon = false;
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "Plugins";
         this.Load += new System.EventHandler(this.frmPlugins_Load);
         this.PluginDetailsGroup.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Button btnCancel;
      private System.Windows.Forms.Button btnOK;
      private System.Windows.Forms.Button btnDown;
      private System.Windows.Forms.Button btnUp;
      private System.Windows.Forms.GroupBox PluginDetailsGroup;
      private System.Windows.Forms.Label lblPluginDescription;
      private System.Windows.Forms.Label lblPluginAuthor;
      private System.Windows.Forms.Label lblPluginVersion;
      private System.Windows.Forms.Label lblPluginName;
      private System.Windows.Forms.Label lblPluginDetailAuthor;
      private System.Windows.Forms.Label lblPluginDetailVersion;
      private System.Windows.Forms.Label lblPluginDetailName;
      private System.Windows.Forms.ListView PluginsList;
      private System.Windows.Forms.ColumnHeader PluginsColumnEnabled;
      private System.Windows.Forms.ColumnHeader PluginsColumnName;
      private System.Windows.Forms.ColumnHeader PluginsColumnExt;
   }
}