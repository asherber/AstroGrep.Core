using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AstroGrep.Windows.Forms
{
	/// <summary>
	/// Unhandled exception dialog.  Used to show an unhandled expection message along with the option to continue or quit the application.
	/// </summary>
	public partial class UnhandledExceptionDialog : BaseForm
	{
		private const int RESIZE = 250;

		/// <summary>
		/// Constructor for the <see cref="UnhandledExceptionDialog"/> dialog.
		/// </summary>
		/// <param name="ex"><see cref="Exception"/> raised</param>
		/// <param name="isFatal">True if the only option is to quit (continue will be hidden)</param>
		public UnhandledExceptionDialog(Exception ex, bool isFatal) : this()
		{
			if (isFatal)
			{
				MessageLabel.Text = "An unhandled exception has occurred in the application. The program has logged the message. The application will close immediately and all work will be lost.";
				ContinueButton.Visible = false;
			}
			else
			{
				MessageLabel.Text = "An unhandled exception has occurred in the application.  The program has logged the message. You may continue, however the last action may not have processed correctly. If you Quit, the application will close immediately and all work will be lost.";
			}

			StringBuilder sb = new StringBuilder();
			Exception e = ex;
			while (e != null)
			{
				sb.Append(e.Source);
				sb.Append(": ");
				sb.Append(e.Message);
				sb.AppendLine();

				e = e.InnerException;
			}

			sb.AppendLine();

			System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(@"");
			var stackTrace = r.Replace(ex.StackTrace, "").Trim();
			sb.Append(stackTrace);

			DetailText.Text = sb.ToString();
			ExceptionLabel.Text = ex.Message;
			DetailText.Visible = false;
		}

		/// <summary>
		/// Private, standard, constructor.
		/// </summary>
		private UnhandledExceptionDialog()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Close the dialog but keep running.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ContinueButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		/// <summary>
		/// Show/hide the details text.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DetailsToggleButton_CheckedChanged(object sender, EventArgs e)
		{
			DetailText.Visible = DetailsToggleButton.Checked;
			if (DetailsToggleButton.Checked)
			{
				Top -= RESIZE / 2;
				Height += RESIZE;
			}
			else
			{
				Top += RESIZE / 2;
				Height -= RESIZE;
			}
		}

		/// <summary>
		/// Close the dialog and terminate.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void QuitButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Abort;
			Close();
		}

		/// <summary>
		/// Handle making sure to set to <see cref="DialogResult.Abort"/> if dialog is canceled.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void UnhandledExceptionDialog_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (DialogResult == DialogResult.Cancel)
				DialogResult = DialogResult.Abort;
		}
	}
}