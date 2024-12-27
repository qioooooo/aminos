using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Drawing.Design
{
	// Token: 0x0200001B RID: 27
	internal class NativeMethods
	{
		// Token: 0x060000B0 RID: 176 RVA: 0x00005B15 File Offset: 0x00004B15
		private NativeMethods()
		{
		}

		// Token: 0x060000B1 RID: 177
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendDlgItemMessage(IntPtr hDlg, int nIDDlgItem, int Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x060000B2 RID: 178
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetDlgItem(IntPtr hWnd, int nIDDlgItem);

		// Token: 0x060000B3 RID: 179
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool EnableWindow(IntPtr hWnd, bool enable);

		// Token: 0x060000B4 RID: 180
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int flags);

		// Token: 0x060000B5 RID: 181
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetDlgItemInt(IntPtr hWnd, int nIDDlgItem, bool[] err, bool signed);

		// Token: 0x060000B6 RID: 182
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

		// Token: 0x04000087 RID: 135
		public const int EM_GETSEL = 176;

		// Token: 0x04000088 RID: 136
		public const int EM_SETSEL = 177;

		// Token: 0x04000089 RID: 137
		public const int EM_GETRECT = 178;

		// Token: 0x0400008A RID: 138
		public const int EM_SETRECT = 179;

		// Token: 0x0400008B RID: 139
		public const int EM_SETRECTNP = 180;

		// Token: 0x0400008C RID: 140
		public const int EM_SCROLL = 181;

		// Token: 0x0400008D RID: 141
		public const int EM_LINESCROLL = 182;

		// Token: 0x0400008E RID: 142
		public const int EM_SCROLLCARET = 183;

		// Token: 0x0400008F RID: 143
		public const int EM_GETMODIFY = 184;

		// Token: 0x04000090 RID: 144
		public const int EM_SETMODIFY = 185;

		// Token: 0x04000091 RID: 145
		public const int EM_GETLINECOUNT = 186;

		// Token: 0x04000092 RID: 146
		public const int EM_LINEINDEX = 187;

		// Token: 0x04000093 RID: 147
		public const int EM_SETHANDLE = 188;

		// Token: 0x04000094 RID: 148
		public const int EM_GETHANDLE = 189;

		// Token: 0x04000095 RID: 149
		public const int EM_GETTHUMB = 190;

		// Token: 0x04000096 RID: 150
		public const int EM_LINELENGTH = 193;

		// Token: 0x04000097 RID: 151
		public const int EM_REPLACESEL = 194;

		// Token: 0x04000098 RID: 152
		public const int EM_GETLINE = 196;

		// Token: 0x04000099 RID: 153
		public const int EM_LIMITTEXT = 197;

		// Token: 0x0400009A RID: 154
		public const int EM_CANUNDO = 198;

		// Token: 0x0400009B RID: 155
		public const int EM_UNDO = 199;

		// Token: 0x0400009C RID: 156
		public const int EM_FMTLINES = 200;

		// Token: 0x0400009D RID: 157
		public const int EM_LINEFROMCHAR = 201;

		// Token: 0x0400009E RID: 158
		public const int EM_SETTABSTOPS = 203;

		// Token: 0x0400009F RID: 159
		public const int EM_SETPASSWORDCHAR = 204;

		// Token: 0x040000A0 RID: 160
		public const int EM_EMPTYUNDOBUFFER = 205;

		// Token: 0x040000A1 RID: 161
		public const int EM_GETFIRSTVISIBLELINE = 206;

		// Token: 0x040000A2 RID: 162
		public const int EM_SETREADONLY = 207;

		// Token: 0x040000A3 RID: 163
		public const int EM_SETWORDBREAKPROC = 208;

		// Token: 0x040000A4 RID: 164
		public const int EM_GETWORDBREAKPROC = 209;

		// Token: 0x040000A5 RID: 165
		public const int EM_GETPASSWORDCHAR = 210;

		// Token: 0x040000A6 RID: 166
		public const int EM_SETMARGINS = 211;

		// Token: 0x040000A7 RID: 167
		public const int EM_GETMARGINS = 212;

		// Token: 0x040000A8 RID: 168
		public const int EM_SETLIMITTEXT = 197;

		// Token: 0x040000A9 RID: 169
		public const int EM_GETLIMITTEXT = 213;

		// Token: 0x040000AA RID: 170
		public const int EM_POSFROMCHAR = 214;

		// Token: 0x040000AB RID: 171
		public const int EM_CHARFROMPOS = 215;

		// Token: 0x040000AC RID: 172
		public const int EC_LEFTMARGIN = 1;

		// Token: 0x040000AD RID: 173
		public const int EC_RIGHTMARGIN = 2;

		// Token: 0x040000AE RID: 174
		public const int EC_USEFONTINFO = 65535;

		// Token: 0x040000AF RID: 175
		public const int IDOK = 1;

		// Token: 0x040000B0 RID: 176
		public const int IDCANCEL = 2;

		// Token: 0x040000B1 RID: 177
		public const int IDABORT = 3;

		// Token: 0x040000B2 RID: 178
		public const int IDRETRY = 4;

		// Token: 0x040000B3 RID: 179
		public const int IDIGNORE = 5;

		// Token: 0x040000B4 RID: 180
		public const int IDYES = 6;

		// Token: 0x040000B5 RID: 181
		public const int IDNO = 7;

		// Token: 0x040000B6 RID: 182
		public const int IDCLOSE = 8;

		// Token: 0x040000B7 RID: 183
		public const int IDHELP = 9;

		// Token: 0x040000B8 RID: 184
		public const int WM_INITDIALOG = 272;

		// Token: 0x040000B9 RID: 185
		public const int SWP_NOSIZE = 1;

		// Token: 0x040000BA RID: 186
		public const int SWP_NOMOVE = 2;

		// Token: 0x040000BB RID: 187
		public const int SWP_NOZORDER = 4;

		// Token: 0x040000BC RID: 188
		public const int SWP_NOREDRAW = 8;

		// Token: 0x040000BD RID: 189
		public const int SWP_NOACTIVATE = 16;

		// Token: 0x040000BE RID: 190
		public const int SWP_FRAMECHANGED = 32;

		// Token: 0x040000BF RID: 191
		public const int SWP_SHOWWINDOW = 64;

		// Token: 0x040000C0 RID: 192
		public const int SWP_HIDEWINDOW = 128;

		// Token: 0x040000C1 RID: 193
		public const int SWP_NOCOPYBITS = 256;

		// Token: 0x040000C2 RID: 194
		public const int SWP_NOOWNERZORDER = 512;

		// Token: 0x040000C3 RID: 195
		public const int SWP_NOSENDCHANGING = 1024;

		// Token: 0x040000C4 RID: 196
		public const int SWP_DRAWFRAME = 32;

		// Token: 0x040000C5 RID: 197
		public const int SWP_NOREPOSITION = 512;

		// Token: 0x040000C6 RID: 198
		public const int SWP_DEFERERASE = 8192;

		// Token: 0x040000C7 RID: 199
		public const int SWP_ASYNCWINDOWPOS = 16384;

		// Token: 0x040000C8 RID: 200
		public const int WM_COMMAND = 273;

		// Token: 0x040000C9 RID: 201
		public const int CC_FULLOPEN = 2;

		// Token: 0x040000CA RID: 202
		public const int CC_PREVENTFULLOPEN = 4;

		// Token: 0x040000CB RID: 203
		public const int CC_SHOWHELP = 8;

		// Token: 0x040000CC RID: 204
		public const int CC_ENABLEHOOK = 16;

		// Token: 0x040000CD RID: 205
		public const int CC_ENABLETEMPLATE = 32;

		// Token: 0x040000CE RID: 206
		public const int CC_ENABLETEMPLATEHANDLE = 64;

		// Token: 0x040000CF RID: 207
		public const int CC_SOLIDCOLOR = 128;

		// Token: 0x040000D0 RID: 208
		public const int CC_ANYCOLOR = 256;

		// Token: 0x040000D1 RID: 209
		public static IntPtr InvalidIntPtr = (IntPtr)(-1);

		// Token: 0x0200001C RID: 28
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public class Util
		{
			// Token: 0x060000B8 RID: 184 RVA: 0x00005B2A File Offset: 0x00004B2A
			private Util()
			{
			}

			// Token: 0x060000B9 RID: 185 RVA: 0x00005B32 File Offset: 0x00004B32
			public static int MAKELONG(int low, int high)
			{
				return (high << 16) | (low & 65535);
			}

			// Token: 0x060000BA RID: 186 RVA: 0x00005B40 File Offset: 0x00004B40
			public static int MAKELPARAM(int low, int high)
			{
				return (high << 16) | (low & 65535);
			}

			// Token: 0x060000BB RID: 187 RVA: 0x00005B4E File Offset: 0x00004B4E
			public static int HIWORD(int n)
			{
				return (n >> 16) & 65535;
			}

			// Token: 0x060000BC RID: 188 RVA: 0x00005B5A File Offset: 0x00004B5A
			public static int LOWORD(int n)
			{
				return n & 65535;
			}

			// Token: 0x060000BD RID: 189 RVA: 0x00005B64 File Offset: 0x00004B64
			public static int SignedHIWORD(int n)
			{
				int num = (int)((short)((n >> 16) & 65535));
				num <<= 16;
				return num >> 16;
			}

			// Token: 0x060000BE RID: 190 RVA: 0x00005B88 File Offset: 0x00004B88
			public static int SignedLOWORD(int n)
			{
				int num = (int)((short)(n & 65535));
				num <<= 16;
				return num >> 16;
			}

			// Token: 0x060000BF RID: 191
			[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
			private static extern int lstrlen(string s);

			// Token: 0x060000C0 RID: 192
			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			internal static extern int RegisterWindowMessage(string msg);
		}

		// Token: 0x0200001D RID: 29
		[StructLayout(LayoutKind.Sequential)]
		public class POINT
		{
			// Token: 0x060000C1 RID: 193 RVA: 0x00005BA9 File Offset: 0x00004BA9
			public POINT()
			{
			}

			// Token: 0x060000C2 RID: 194 RVA: 0x00005BB1 File Offset: 0x00004BB1
			public POINT(int x, int y)
			{
				this.x = x;
				this.y = y;
			}

			// Token: 0x040000D2 RID: 210
			public int x;

			// Token: 0x040000D3 RID: 211
			public int y;
		}
	}
}
