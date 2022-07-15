using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace AstroGrep.Windows.Forms
{
   public partial class frmPrint
   {
      #region Windows Form Designer generated code
      private System.Windows.Forms.Button cmdPrint;
      private System.Windows.Forms.Button cmdPreview;
      private System.Windows.Forms.Button cmdPageSetup;
      private System.Windows.Forms.Button cmdCancel;
      private System.ComponentModel.Container components = null;

      private void InitializeComponent()
      {
			this.cmdPrint = new System.Windows.Forms.Button();
			this.cmdPreview = new System.Windows.Forms.Button();
			this.cmdPageSetup = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.pnlPrintTypes = new System.Windows.Forms.Panel();
			this.lstPrintTypes = new System.Windows.Forms.ListBox();
			this.lblSelect = new System.Windows.Forms.Label();
			this.rdbNone = new System.Windows.Forms.RadioButton();
			this.rdbBold = new System.Windows.Forms.RadioButton();
			this.rdbColor = new System.Windows.Forms.RadioButton();
			this.lblResultsMatch = new System.Windows.Forms.Label();
			this.pnlPrintTypes.SuspendLayout();
			this.SuspendLayout();
			// 
			// cmdPrint
			// 
			this.cmdPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdPrint.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cmdPrint.Location = new System.Drawing.Point(8, 211);
			this.cmdPrint.Name = "cmdPrint";
			this.cmdPrint.Size = new System.Drawing.Size(105, 25);
			this.cmdPrint.TabIndex = 1;
			this.cmdPrint.Text = "&Print";
			this.cmdPrint.Click += new System.EventHandler(this.cmdPrint_Click);
			// 
			// cmdPreview
			// 
			this.cmdPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdPreview.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cmdPreview.Location = new System.Drawing.Point(120, 211);
			this.cmdPreview.Name = "cmdPreview";
			this.cmdPreview.Size = new System.Drawing.Size(105, 25);
			this.cmdPreview.TabIndex = 2;
			this.cmdPreview.Text = "Pre&view";
			this.cmdPreview.Click += new System.EventHandler(this.cmdPreview_Click);
			// 
			// cmdPageSetup
			// 
			this.cmdPageSetup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdPageSetup.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cmdPageSetup.Location = new System.Drawing.Point(256, 211);
			this.cmdPageSetup.Name = "cmdPageSetup";
			this.cmdPageSetup.Size = new System.Drawing.Size(105, 25);
			this.cmdPageSetup.TabIndex = 3;
			this.cmdPageSetup.Text = "Page &Setup";
			this.cmdPageSetup.Click += new System.EventHandler(this.cmdPageSetup_Click);
			// 
			// cmdCancel
			// 
			this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cmdCancel.Location = new System.Drawing.Point(367, 211);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(105, 25);
			this.cmdCancel.TabIndex = 4;
			this.cmdCancel.Text = "&Cancel";
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// pnlPrintTypes
			// 
			this.pnlPrintTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlPrintTypes.Controls.Add(this.lstPrintTypes);
			this.pnlPrintTypes.Controls.Add(this.lblSelect);
			this.pnlPrintTypes.Location = new System.Drawing.Point(8, 8);
			this.pnlPrintTypes.Name = "pnlPrintTypes";
			this.pnlPrintTypes.Size = new System.Drawing.Size(466, 133);
			this.pnlPrintTypes.TabIndex = 5;
			// 
			// lstPrintTypes
			// 
			this.lstPrintTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lstPrintTypes.ItemHeight = 15;
			this.lstPrintTypes.Location = new System.Drawing.Point(0, 24);
			this.lstPrintTypes.Name = "lstPrintTypes";
			this.lstPrintTypes.Size = new System.Drawing.Size(466, 109);
			this.lstPrintTypes.TabIndex = 5;
			// 
			// lblSelect
			// 
			this.lblSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblSelect.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lblSelect.Location = new System.Drawing.Point(0, 0);
			this.lblSelect.Name = "lblSelect";
			this.lblSelect.Size = new System.Drawing.Size(466, 16);
			this.lblSelect.TabIndex = 6;
			this.lblSelect.Text = "Please select the output type:";
			// 
			// rdbNone
			// 
			this.rdbNone.AutoSize = true;
			this.rdbNone.Location = new System.Drawing.Point(8, 173);
			this.rdbNone.Name = "rdbNone";
			this.rdbNone.Size = new System.Drawing.Size(55, 19);
			this.rdbNone.TabIndex = 6;
			this.rdbNone.TabStop = true;
			this.rdbNone.Text = "&None";
			this.rdbNone.UseVisualStyleBackColor = true;
			// 
			// rdbBold
			// 
			this.rdbBold.AutoSize = true;
			this.rdbBold.Location = new System.Drawing.Point(120, 173);
			this.rdbBold.Name = "rdbBold";
			this.rdbBold.Size = new System.Drawing.Size(50, 19);
			this.rdbBold.TabIndex = 7;
			this.rdbBold.TabStop = true;
			this.rdbBold.Text = "&Bold";
			this.rdbBold.UseVisualStyleBackColor = true;
			// 
			// rdbColor
			// 
			this.rdbColor.AutoSize = true;
			this.rdbColor.Location = new System.Drawing.Point(256, 173);
			this.rdbColor.Name = "rdbColor";
			this.rdbColor.Size = new System.Drawing.Size(54, 19);
			this.rdbColor.TabIndex = 8;
			this.rdbColor.TabStop = true;
			this.rdbColor.Text = "C&olor";
			this.rdbColor.UseVisualStyleBackColor = true;
			// 
			// lblResultsMatch
			// 
			this.lblResultsMatch.AutoSize = true;
			this.lblResultsMatch.Location = new System.Drawing.Point(5, 149);
			this.lblResultsMatch.Name = "lblResultsMatch";
			this.lblResultsMatch.Size = new System.Drawing.Size(85, 15);
			this.lblResultsMatch.TabIndex = 9;
			this.lblResultsMatch.Text = "Results Match";
			// 
			// frmPrint
			// 
			this.AcceptButton = this.cmdPrint;
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(484, 248);
			this.Controls.Add(this.lblResultsMatch);
			this.Controls.Add(this.rdbColor);
			this.Controls.Add(this.rdbBold);
			this.Controls.Add(this.rdbNone);
			this.Controls.Add(this.pnlPrintTypes);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdPageSetup);
			this.Controls.Add(this.cmdPreview);
			this.Controls.Add(this.cmdPrint);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmPrint";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Print";
			this.Load += new System.EventHandler(this.frmPrint_Load);
			this.pnlPrintTypes.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

      }
      #endregion

      /// <summary>
      /// Dispose form.
      /// </summary>
      /// <param name="disposing">system parameter</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing)
         {
            if (components != null)
            {
               components.Dispose();
            }
         }
         base.Dispose(disposing);
      }

      private Panel pnlPrintTypes;
      private ListBox lstPrintTypes;
      private Label lblSelect;
		private RadioButton rdbNone;
		private RadioButton rdbBold;
		private RadioButton rdbColor;
		private Label lblResultsMatch;
	}
}
