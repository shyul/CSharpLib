using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Shyu
{
    public static class FormControl
    {
        private const int WM_SETREDRAW = 11;
        private const int WM_GETTABRECT = 0x130a;
        private const int WS_EX_TRANSPARENT = 0x20;
        private const int WM_SETFONT = 0x30;
        private const int WM_FONTCHANGE = 0x1d;
        private const int WM_HSCROLL = 0x114;
        private const int TCM_HITTEST = 0x130D;
        private const int WM_PAINT = 0xf;
        private const int WS_EX_LAYOUTRTL = 0x400000;
        private const int WS_EX_NOINHERITLAYOUT = 0x100000;

        private const int EM_GET_RECT = 0xB2;
        private const int EM_SET_RECT = 0xB3;

        /*
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
        */
        public static IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            Control control = Control.FromHandle(hWnd);
            if (control == null){return IntPtr.Zero;}
            Message message = new Message();
            message.HWnd = hWnd;
            message.LParam = lParam;
            message.WParam = wParam;
            message.Msg = msg;

            MethodInfo wproc = control.GetType().GetMethod(
                "WndProc",
                BindingFlags.NonPublic | 
                BindingFlags.InvokeMethod | 
                BindingFlags.FlattenHierarchy | 
                BindingFlags.IgnoreCase | 
                BindingFlags.Instance);

            object[] args = new object[] { message };
            wproc.Invoke(control, args);

            return ((Message)args[0]).Result;
        }
        public static IntPtr ToIntPtr(object structure)
        {
            IntPtr lparam = IntPtr.Zero;
            lparam = Marshal.AllocCoTaskMem(Marshal.SizeOf(structure));
            Marshal.StructureToPtr(structure, lparam, false);
            return lparam;
        }

        public static void Suspend(Control parent)
        {
            if (parent != null && !parent.IsDisposed)
            {
                //SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
                SendMessage(parent.Handle, WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
            }
        }
        public static void Resume(Control parent)
        {
            if (parent != null && !parent.IsDisposed)
            {
                //SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
                SendMessage(parent.Handle, WM_SETREDRAW, ToIntPtr(true), IntPtr.Zero);
                parent.Refresh();
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TCHITTESTINFO
        {
            public TCHITTESTINFO(Point location)
            {
                pt = location;
                flags = TCHITTESTFLAGS.TCHT_ONITEM;
            }
            public Point pt;
            public TCHITTESTFLAGS flags;
        }
        [Flags()]
        public enum TCHITTESTFLAGS
        {
            TCHT_NOWHERE = 1,
            TCHT_ONITEMICON = 2,
            TCHT_ONITEMLABEL = 4,
            TCHT_ONITEM = TCHT_ONITEMICON | TCHT_ONITEMLABEL
        }

        public static readonly ContentAlignment AnyRightAlign = ContentAlignment.BottomRight | ContentAlignment.MiddleRight | ContentAlignment.TopRight;
        public static readonly ContentAlignment AnyLeftAlign = ContentAlignment.BottomLeft | ContentAlignment.MiddleLeft | ContentAlignment.TopLeft;
        public static readonly ContentAlignment AnyTopAlign = ContentAlignment.TopRight | ContentAlignment.TopCenter | ContentAlignment.TopLeft;
        public static readonly ContentAlignment AnyBottomAlign = ContentAlignment.BottomRight | ContentAlignment.BottomCenter | ContentAlignment.BottomLeft;
        public static readonly ContentAlignment AnyMiddleAlign = ContentAlignment.MiddleRight | ContentAlignment.MiddleCenter | ContentAlignment.MiddleLeft;
        public static readonly ContentAlignment AnyCenterAlign = ContentAlignment.BottomCenter | ContentAlignment.MiddleCenter | ContentAlignment.TopCenter;

    }

}
