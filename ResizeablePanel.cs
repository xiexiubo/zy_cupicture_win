using System.Windows.Forms;
using System.Drawing;
using System;

public class ResizablePanel : Panel
{
    private const int WM_NCHITTEST = 0x0084;
    private const int RESIZE_BORDER = 10;

    public ResizablePanel()
    {
        this.SetStyle(ControlStyles.ResizeRedraw, true);
    }

    protected override void WndProc(ref Message m)
    {
        if (m.Msg == WM_NCHITTEST)
        {
            Point cursor = this.PointToClient(Cursor.Position);

            bool left = cursor.X <= RESIZE_BORDER;
            bool right = cursor.X >= this.ClientSize.Width - RESIZE_BORDER;
            bool top = cursor.Y <= RESIZE_BORDER;
            bool bottom = cursor.Y >= this.ClientSize.Height - RESIZE_BORDER;

            if (top && left) m.Result = (IntPtr)13; // HTTOPLEFT
            else if (top && right) m.Result = (IntPtr)14; // HTTOPRIGHT
            else if (bottom && left) m.Result = (IntPtr)16; // HTBOTTOMLEFT
            else if (bottom && right) m.Result = (IntPtr)17; // HTBOTTOMRIGHT
            else if (left) m.Result = (IntPtr)10; // HTLEFT
            else if (right) m.Result = (IntPtr)11; // HTRIGHT
            else if (top) m.Result = (IntPtr)12; // HTTOP
            else if (bottom) m.Result = (IntPtr)15; // HTBOTTOM
            else base.WndProc(ref m);
        }
        else
        {
            base.WndProc(ref m);
        }
    }
}