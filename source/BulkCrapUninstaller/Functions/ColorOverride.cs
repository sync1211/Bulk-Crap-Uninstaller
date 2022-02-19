using System;
using System.Drawing;
using System.Windows.Forms;

namespace BulkCrapUninstaller.Functions
{

    internal class ColorOverride
    {
        public static Color ForeColor = Color.White;
        public static Color ForeColorDisabled = Color.Gray;
        public static Color BackColor = Color.FromArgb(12, 13, 14);
        public static Color BackColorHover = Color.FromArgb(unchecked((int) 0xff07090d));
        public static Color BackColorActive = Color.FromArgb(unchecked((int) 0xff07090d));

        public static void OverrideColors(Form form)
        {
            //TODO: Declare colors as constants
            form.ForeColor = ForeColor;
            form.BackColor = BackColor;

            //Apply Color to all Elements
            foreach (Control control in form.Controls)
            {
                OverrideControlColors(control);
            }

            //this.Refresh();
        }

        public static void OverrideControlColors(Object item)
        {
            //Any Control
            if (item is Control controlItem)
            {
                controlItem.BackColor = BackColor;
                controlItem.ForeColor = ForeColor;
                foreach (Control control in controlItem.Controls)
                {
                    OverrideControlColors(control);
                }
            }
                
            //Label
            if (item is Label label)
            {
                label.ForeColor = ForeColor;
                label.BackColor = Color.Transparent;
            }
                
            //Button
            if (item is Button button)
            {
                button.Paint += new PaintEventHandler(PaintButton);
            }

            if (item is ContextMenuStrip contextMenuStrip)
            {
                foreach (ToolStripItem toolStripMenu in contextMenuStrip.Items)
                {
                    toolStripMenu.ForeColor = ForeColor;
                    toolStripMenu.BackColor = BackColor;

                    if (toolStripMenu is ToolStripMenuItem menuItem)
                    {
                        foreach (ToolStripItem dropDownItem in menuItem.DropDownItems)
                        {
                            dropDownItem.ForeColor = ForeColor;
                            dropDownItem.BackColor = BackColor;
                        }
                    }
                }
            }

            //MenuStrip
            if (item is MenuStrip menuStrip) { 
                foreach (ToolStripItem toolStripMenu in menuStrip.Items)
                {
                    toolStripMenu.ForeColor = ForeColor;
                    toolStripMenu.BackColor = BackColor;

                    if (toolStripMenu is ToolStripMenuItem menuItem)
                    {
                        foreach (ToolStripItem dropDownItem in menuItem.DropDownItems)
                        {
                            dropDownItem.ForeColor = ForeColor;
                            dropDownItem.BackColor = BackColor;
                        }
                    }
                }
            }

            //DataGridView
            if (item is DataGridView dataGridView)
            {
                dataGridView.DefaultCellStyle.ForeColor = ForeColor;
                dataGridView.DefaultCellStyle.BackColor = BackColor;

                dataGridView.EnableHeadersVisualStyles = false;
                dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = ForeColor;
                dataGridView.ColumnHeadersDefaultCellStyle.BackColor = BackColor;
            }

            //ObjectListView
            if (item is BrightIdeasSoftware.ObjectListView objectListView)
            {
                if (objectListView.HeaderFormatStyle != null)
                {
                    objectListView.HeaderFormatStyle.SetForeColor(ForeColor);
                    objectListView.HeaderFormatStyle.Normal.BackColor = BackColor;
                    objectListView.HeaderFormatStyle.Hot.BackColor = BackColorHover;
                    objectListView.HeaderFormatStyle.Pressed.BackColor = BackColorActive;
                } else
                {
                    BrightIdeasSoftware.HeaderFormatStyle headerStyle = new BrightIdeasSoftware.HeaderFormatStyle();

                    headerStyle.SetForeColor(ForeColor);
                    headerStyle.Normal.BackColor = BackColor;
                    
                    //TODO: These colors do not show up
                    //headerStyle.Hot.BackColor = BackColorHover;
                    //headerStyle.Pressed.BackColor = BackColorActive;

                    objectListView.HeaderFormatStyle = headerStyle;
                }
                objectListView.HeaderUsesThemes = false;
            }

        }

        //Functions for overriding the rendering methods of controls
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
                switch(Math.Floor(Math.Log((int) btn.TextAlign, 16)))
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
