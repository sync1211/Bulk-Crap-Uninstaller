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

        public static void OverrideColors(Form form)
        {
            //TODO: Declare the colors as constants
            form.ForeColor = ForeColor;
            form.BackColor = BackColor;

            //Apply Color to all Elements
            OverrideControlColors(form);

            //this.Refresh();
        }

        private static void OverrideControlColors(Control control)
        {
            foreach (object item in control.Controls)
            {
                //Any Control
                if (item is Control controlItem)
                {
                    controlItem.BackColor = BackColor;
                    controlItem.ForeColor = ForeColor;
                    OverrideControlColors(controlItem);
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

                //MenuStrip
                if (item is MenuStrip menuStrip)
                {
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
            }
        }

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
    }
}
