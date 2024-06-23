using BulkCrapUninstaller.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BulkCrapUninstaller.Functions
{
    internal static class ColorOverride
    {
        public static Color ForeColor = Color.White;
        public static Color ForeColorDisabled = Color.Gray;
        public static Color BackColor = Color.FromArgb(12, 13, 14);
        public static Color BackColorHover = Color.FromArgb(unchecked((int)0xff07090d));
        public static Color BackColorActive = Color.FromArgb(unchecked((int)0xff07090d));

        public static void OverrideColors(Form form)
        {
            //TODO: Declare colors as constants
            form.ForeColor = ForeColor;
            form.BackColor = BackColor;

            bool isStrict = form is ListLegendWindow;

            //Apply Color to all Elements
            foreach (Control control in form.Controls)
            {
                OverrideControlColors(control, isStrict);
            }

            //this.Refresh();
        }

        private static Bitmap InvertImageColors(Image origImage)
        {
            Bitmap bitmap = new Bitmap(origImage);
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color orig = bitmap.GetPixel(y, x);
                    Color inv = Color.FromArgb(orig.A, 255 - orig.R, 255 - orig.G, 255 - orig.B);
                    bitmap.SetPixel(y, x, inv);
                }
            }
            return bitmap;
        }

        public static void OverrideControlColors(Object item, bool strict = false)
        {

            // Any Control
            if (item is Control controlItem)
            {
                // Strict mode; Only override if the control's color is the default
                if (!strict || controlItem.BackColor == SystemColors.Control)
                {
                    controlItem.BackColor = BackColor;
                    controlItem.ForeColor = ForeColor;
                }

                foreach (Control control in controlItem.Controls)
                {
                    OverrideControlColors(control, strict);
                }
            }

            // Label
            if (item is Label label && !(strict && label.BackColor != SystemColors.Control))
            {
                label.ForeColor = ForeColor;
                label.BackColor = Color.Transparent;
            }

            // Button
            if (item is Button button && !(strict && button.BackColor != SystemColors.Control))
            {
                button.Paint += PaintButton;

                if (button.Image is Image buttonImage)
                {
                    button.Image = InvertImageColors(buttonImage);
                }
            }

            // ToolStripItem
            if (item is ToolStripItem toolStripItem)
            {
                toolStripItem.ForeColor = ForeColor;
                toolStripItem.BackColor = BackColor;

                if (toolStripItem.Image is Image itemImage)
                {
                    toolStripItem.Image = InvertImageColors(itemImage);
                }
            }

            // ToolStrip
            if (item is ToolStrip toolStrip)
            {
                foreach (ToolStripItem tsItem in toolStrip.Items)
                {
                    OverrideControlColors(tsItem, strict);
                }
                return;
            }

            // ToolStripDropDownItem
            if (item is ToolStripDropDownItem toolStripDropDownItem)
            {
                toolStripDropDownItem.ForeColor = ForeColor;
                toolStripDropDownItem.BackColor = BackColor;
            }

            // ToolStripMenuItem
            if (item is ToolStripMenuItem toolStripMenuItem)
            {
                foreach (ToolStripItem dropDownItem in toolStripMenuItem.DropDownItems)
                {
                    OverrideControlColors(dropDownItem, strict);
                }
            }

            // ContextMenuStrip
            if (item is ContextMenuStrip contextMenuStrip)
            {
                foreach (ToolStripItem contextMenuStripItem in contextMenuStrip.Items)
                {
                    OverrideControlColors(contextMenuStripItem, strict);
                }
            }

            // MenuStrip
            if (item is MenuStrip menuStrip)
            {
                foreach (ToolStripItem toolStripMenu in menuStrip.Items)
                {
                    OverrideControlColors(toolStripMenu, strict);
                }
            }

            // DataGridView
            if (item is DataGridView dataGridView)
            {
                dataGridView.DefaultCellStyle.ForeColor = ForeColor;
                dataGridView.DefaultCellStyle.BackColor = BackColor;

                dataGridView.EnableHeadersVisualStyles = false;
                dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = ForeColor;
                dataGridView.ColumnHeadersDefaultCellStyle.BackColor = BackColor;
            }

            // ObjectListView
            if (item is BrightIdeasSoftware.ObjectListView objectListView)
            {
                if (objectListView.HeaderFormatStyle != null)
                {
                    objectListView.HeaderFormatStyle.SetForeColor(ForeColor);
                    objectListView.HeaderFormatStyle.Normal.BackColor = BackColor;
                    objectListView.HeaderFormatStyle.Hot.BackColor = BackColorHover;
                    objectListView.HeaderFormatStyle.Pressed.BackColor = BackColorActive;
                }
                else
                {
                    BrightIdeasSoftware.HeaderFormatStyle headerStyle = new BrightIdeasSoftware.HeaderFormatStyle();

                    headerStyle.SetForeColor(ForeColor);
                    headerStyle.Normal.BackColor = BackColor;

                    //TODO: These colors do not show up
                    // headerStyle.Hot.BackColor = BackColorHover;
                    // headerStyle.Pressed.BackColor = BackColorActive;

                    objectListView.HeaderFormatStyle = headerStyle;
                }
                objectListView.HeaderUsesThemes = false;
            }
        }

        // Functions for overriding the rendering methods of controls
        private static void PaintButton(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (sender is Button btn)
            {
                if (btn.Enabled)
                {
                    return;
                }

                //Redraw the text, as the default text is not visible in dark mode
                SolidBrush drawBrush = new SolidBrush(ForeColorDisabled);

                StringAlignment textAlign;
                switch (Math.Log(((int)btn.TextAlign) % 5, 2))
                {
                    case 0:
                        textAlign = StringAlignment.Near;
                        break;
                    case 1:
                        textAlign = StringAlignment.Center;
                        break;
                    case 2:
                        textAlign = StringAlignment.Far;
                        break;
                    default:
                        textAlign = StringAlignment.Center;
                        break;
                }

                StringAlignment lineAlign;
                switch (Math.Floor(Math.Log((int)btn.TextAlign, 16)))
                {
                    case 0:
                        lineAlign = StringAlignment.Near;
                        break;
                    case 1:
                        lineAlign = StringAlignment.Center;
                        break;
                    case 2:
                        lineAlign = StringAlignment.Far;
                        break;
                    default:
                        lineAlign = StringAlignment.Center;
                        break;
                }


                StringFormat sf = new StringFormat
                {
                    FormatFlags = StringFormatFlags.NoWrap,
                    Alignment = textAlign,
                    LineAlignment = lineAlign
                };

                e.Graphics.DrawString(btn.Text, btn.Font, drawBrush, btn.DisplayRectangle, sf);
                drawBrush.Dispose();
                sf.Dispose();
            }
        }

        public static void TabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (sender is TabControl tabCtrl)
            {
                //Draw box
                SizeF tabSize = e.Graphics.MeasureString(tabCtrl.TabPages[e.Index].Text, e.Font);
                using (Brush backBrush = new SolidBrush(ColorOverride.BackColor))
                {
                    e.Graphics.FillRectangle(backBrush, e.Bounds);

                    Rectangle rect = e.Bounds;
                    rect.Offset(0, 1);
                    rect.Inflate(0, 1);
                    e.Graphics.FillRectangle(new SolidBrush(ColorOverride.BackColor), rect);
                }

                //Draw text
                using (Brush textBrush = new SolidBrush(tabCtrl.ForeColor))
                {
                    e.Graphics.DrawString(
                        tabCtrl.TabPages[e.Index].Text,
                        e.Font,
                        textBrush,
                        e.Bounds.Left + (e.Bounds.Width - tabSize.Width) / 2,
                        e.Bounds.Top + (e.Bounds.Height - tabSize.Height) / 2 + 1
                    );
                }
                e.DrawFocusRectangle();
            }
        }
    }


}
