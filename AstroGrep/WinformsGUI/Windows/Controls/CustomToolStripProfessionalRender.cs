using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AstroGrep.Windows.Controls
{
   /// <summary>
   /// Handle better showing button selected state.
   /// </summary>
   /// <history>
   /// [Curtis_Beard]	   11/11/2014	Initial
   /// [Curtis_Beard]	   08/20/2019	Move to own class
   /// </history>
   public class CustomToolStripProfessionalRender : ToolStripProfessionalRenderer
   {
      /// <summary>
      /// Adjust button background display to better show selected item.
      /// </summary>
      /// <param name="e">render event argument</param>
      /// <history>
      /// [Curtis_Beard]	   11/11/2014	Initial
      /// </history>
      protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
      {
         if (e.Item is ToolStripButton btn && btn.CheckOnClick && btn.Checked)
         {
            Rectangle bounds = new Rectangle(Point.Empty, e.Item.Size);

            // fill button background
            e.Graphics.FillRectangle(new SolidBrush(ProfessionalColors.ButtonCheckedHighlight), bounds);

            // draw border around button
            bounds.Inflate(-1, -1);
            e.Graphics.DrawRectangle(new Pen(ProfessionalColors.ButtonCheckedHighlightBorder), bounds);
         }
         else
         {
            base.OnRenderButtonBackground(e);
         }
      }
   }
}
