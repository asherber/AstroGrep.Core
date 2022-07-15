namespace AstroGrep.Windows.Forms
{
	partial class UnhandledExceptionDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UnhandledExceptionDialog));
			this.ContinueButton = new System.Windows.Forms.Button();
			this.QuitButton = new System.Windows.Forms.Button();
			this.MessageLabel = new System.Windows.Forms.Label();
			this.ExceptionLabel = new System.Windows.Forms.Label();
			this.DetailText = new System.Windows.Forms.TextBox();
			this.DetailsToggleButton = new System.Windows.Forms.CheckBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// ContinueButton
			// 
			this.ContinueButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ContinueButton.Location = new System.Drawing.Point(414, 124);
			this.ContinueButton.Name = "ContinueButton";
			this.ContinueButton.Size = new System.Drawing.Size(75, 23);
			this.ContinueButton.TabIndex = 0;
			this.ContinueButton.Text = "&Continue";
			this.ContinueButton.UseVisualStyleBackColor = true;
			this.ContinueButton.Click += new System.EventHandler(this.ContinueButton_Click);
			// 
			// QuitButton
			// 
			this.QuitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.QuitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.QuitButton.Location = new System.Drawing.Point(495, 124);
			this.QuitButton.Name = "QuitButton";
			this.QuitButton.Size = new System.Drawing.Size(75, 23);
			this.QuitButton.TabIndex = 1;
			this.QuitButton.Text = "&Quit";
			this.QuitButton.UseVisualStyleBackColor = true;
			this.QuitButton.Click += new System.EventHandler(this.QuitButton_Click);
			// 
			// MessageLabel
			// 
			this.MessageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.MessageLabel.Location = new System.Drawing.Point(58, 14);
			this.MessageLabel.Name = "MessageLabel";
			this.MessageLabel.Size = new System.Drawing.Size(513, 57);
			this.MessageLabel.TabIndex = 3;
			this.MessageLabel.Text = "Message Text";
			// 
			// ExceptionLabel
			// 
			this.ExceptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ExceptionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ExceptionLabel.Location = new System.Drawing.Point(12, 71);
			this.ExceptionLabel.Name = "ExceptionLabel";
			this.ExceptionLabel.Size = new System.Drawing.Size(559, 40);
			this.ExceptionLabel.TabIndex = 4;
			this.ExceptionLabel.Text = "Exception Message";
			this.ExceptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// DetailText
			// 
			this.DetailText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DetailText.Location = new System.Drawing.Point(15, 115);
			this.DetailText.Multiline = true;
			this.DetailText.Name = "DetailText";
			this.DetailText.ReadOnly = true;
			this.DetailText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.DetailText.Size = new System.Drawing.Size(556, 3);
			this.DetailText.TabIndex = 5;
			this.DetailText.WordWrap = false;
			// 
			// DetailsToggleButton
			// 
			this.DetailsToggleButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.DetailsToggleButton.Appearance = System.Windows.Forms.Appearance.Button;
			this.DetailsToggleButton.AutoSize = true;
			this.DetailsToggleButton.Location = new System.Drawing.Point(12, 124);
			this.DetailsToggleButton.Name = "DetailsToggleButton";
			this.DetailsToggleButton.Size = new System.Drawing.Size(49, 23);
			this.DetailsToggleButton.TabIndex = 6;
			this.DetailsToggleButton.Text = "Details";
			this.DetailsToggleButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.DetailsToggleButton.UseVisualStyleBackColor = true;
			this.DetailsToggleButton.CheckedChanged += new System.EventHandler(this.DetailsToggleButton_CheckedChanged);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(14, 14);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(32, 32);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 7;
			this.pictureBox1.TabStop = false;
			// 
			// frmUnhandledException
			// 
			this.AcceptButton = this.QuitButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.QuitButton;
			this.ClientSize = new System.Drawing.Size(583, 159);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.DetailsToggleButton);
			this.Controls.Add(this.DetailText);
			this.Controls.Add(this.ExceptionLabel);
			this.Controls.Add(this.MessageLabel);
			this.Controls.Add(this.QuitButton);
			this.Controls.Add(this.ContinueButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmUnhandledException";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Unhandled Exception in Application";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UnhandledExceptionDialog_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button ContinueButton;
		private System.Windows.Forms.Button QuitButton;
		private System.Windows.Forms.Label MessageLabel;
		private System.Windows.Forms.Label ExceptionLabel;
		private System.Windows.Forms.TextBox DetailText;
		private System.Windows.Forms.CheckBox DetailsToggleButton;
		private System.Windows.Forms.PictureBox pictureBox1;
	}
}