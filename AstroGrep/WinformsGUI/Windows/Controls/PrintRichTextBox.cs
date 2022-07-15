using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace AstroGrep.Windows.Controls
{
   /// <summary>
   /// Enhanced <see cref="RichTextBox"/> that allows printing using the rich text formats displayed in the <see cref="RichTextBox"/>.
   /// </summary>
   /// <remarks>
   /// AstroGrep File Searching Utility. Written by Theodore L. Ward
   /// Copyright (C) 2002 AstroComma Incorporated.
   /// 
   /// This program is free software; you can redistribute it and/or
   /// modify it under the terms of the GNU General Public License
   /// as published by the Free Software Foundation; either version 2
   /// of the License, or (at your option) any later version.
   /// 
   /// This program is distributed in the hope that it will be useful,
   /// but WITHOUT ANY WARRANTY; without even the implied warranty of
   /// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   /// GNU General Public License for more details.
   /// 
   /// You should have received a copy of the GNU General Public License
   /// along with this program; if not, write to the Free Software
   /// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
   /// 
   /// The author may be contacted at:
   /// ted@astrocomma.com or curtismbeard@gmail.com or mandrolic@sourceforge.net
   /// </remarks>
   /// <history>
   /// [Curtis_Beard]      09/12/2019	CHG: 117, emphasize match text
   /// </history>
   public class PrintRichTextBox : RichTextBox
   {
      //Convert the unit used by the .NET framework (1/100 inch) 
      //and the unit used by Win32 API calls (twips 1/1440 inch)
      private const double anInch = 14.4;

      [StructLayout(LayoutKind.Sequential)]
      private struct RECT
      {
         public int Left;
         public int Top;
         public int Right;
         public int Bottom;
      }

      [StructLayout(LayoutKind.Sequential)]
      private struct CHARRANGE
      {
         public int cpMin;         //First character of range (0 for start of doc)
         public int cpMax;           //Last character of range (-1 for end of doc)
      }

      [StructLayout(LayoutKind.Sequential)]
      private struct FORMATRANGE
      {
         public IntPtr hdc;             //Actual DC to draw on
         public IntPtr hdcTarget;       //Target DC for determining text formatting
         public RECT rc;                //Region of the DC to draw to (in twips)
         public RECT rcPage;            //Region of the whole DC (page size) (in twips)
         public CHARRANGE chrg;         //Range of text to draw (see earlier declaration)
      }

      private const int WM_USER = 0x0400;
      private const int EM_FORMATRANGE = WM_USER + 57;

      [DllImport("USER32.dll")]
      private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

      // Render the contents of the RichTextBox for printing
      //  Return the last character printed + 1 (printing start from this point for next page)
      /// <summary>
      /// 
      /// </summary>
      /// <param name="charFrom"></param>
      /// <param name="charTo"></param>
      /// <param name="e"></param>
      /// <returns></returns>
      public int Print(int charFrom, int charTo, PrintPageEventArgs e)
      {
         //Calculate the area to render and print
         RECT rectToPrint;
         rectToPrint.Top = (int)(e.MarginBounds.Top * anInch);
         rectToPrint.Bottom = (int)(e.MarginBounds.Bottom * anInch);
         rectToPrint.Left = (int)(e.MarginBounds.Left * anInch);
         rectToPrint.Right = (int)(e.MarginBounds.Right * anInch);

         //Calculate the size of the page
         RECT rectPage;
         rectPage.Top = (int)(e.PageBounds.Top * anInch);
         rectPage.Bottom = (int)(e.PageBounds.Bottom * anInch);
         rectPage.Left = (int)(e.PageBounds.Left * anInch);
         rectPage.Right = (int)(e.PageBounds.Right * anInch);

         IntPtr hdc = e.Graphics.GetHdc();

         FORMATRANGE fmtRange;
         fmtRange.chrg.cpMax = charTo;               //Indicate character from to character to 
         fmtRange.chrg.cpMin = charFrom;
         fmtRange.hdc = hdc;                    //Use the same DC for measuring and rendering
         fmtRange.hdcTarget = hdc;              //Point at printer hDC
         fmtRange.rc = rectToPrint;             //Indicate the area on page to print
         fmtRange.rcPage = rectPage;            //Indicate size of page

         IntPtr res = IntPtr.Zero;

         IntPtr wparam = IntPtr.Zero;
         wparam = new IntPtr(1);

         //Get the pointer to the FORMATRANGE structure in memory
         IntPtr lparam = IntPtr.Zero;
         lparam = Marshal.AllocCoTaskMem(Marshal.SizeOf(fmtRange));
         Marshal.StructureToPtr(fmtRange, lparam, false);

         //Send the rendered data for printing 
         res = SendMessage(Handle, EM_FORMATRANGE, wparam, lparam);

         //Free the block of memory allocated
         Marshal.FreeCoTaskMem(lparam);

         //Release the device context handle obtained by a previous call
         e.Graphics.ReleaseHdc(hdc);

         //Return last + 1 character printer
         return res.ToInt32();
      }
   }
}
