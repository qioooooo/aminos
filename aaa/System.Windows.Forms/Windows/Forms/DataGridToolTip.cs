using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x020002E8 RID: 744
	internal class DataGridToolTip : MarshalByRefObject
	{
		// Token: 0x06002C77 RID: 11383 RVA: 0x00078805 File Offset: 0x00077805
		public DataGridToolTip(DataGrid dataGrid)
		{
			this.dataGrid = dataGrid;
		}

		// Token: 0x06002C78 RID: 11384 RVA: 0x00078814 File Offset: 0x00077814
		public void CreateToolTipHandle()
		{
			if (this.tipWindow == null || this.tipWindow.Handle == IntPtr.Zero)
			{
				NativeMethods.INITCOMMONCONTROLSEX initcommoncontrolsex = new NativeMethods.INITCOMMONCONTROLSEX();
				initcommoncontrolsex.dwICC = 8;
				initcommoncontrolsex.dwSize = Marshal.SizeOf(initcommoncontrolsex);
				SafeNativeMethods.InitCommonControlsEx(initcommoncontrolsex);
				CreateParams createParams = new CreateParams();
				createParams.Parent = this.dataGrid.Handle;
				createParams.ClassName = "tooltips_class32";
				createParams.Style = 1;
				this.tipWindow = new NativeWindow();
				this.tipWindow.CreateHandle(createParams);
				UnsafeNativeMethods.SendMessage(new HandleRef(this.tipWindow, this.tipWindow.Handle), 1048, 0, SystemInformation.MaxWindowTrackSize.Width);
				SafeNativeMethods.SetWindowPos(new HandleRef(this.tipWindow, this.tipWindow.Handle), NativeMethods.HWND_NOTOPMOST, 0, 0, 0, 0, 19);
				UnsafeNativeMethods.SendMessage(new HandleRef(this.tipWindow, this.tipWindow.Handle), 1027, 3, 0);
			}
		}

		// Token: 0x06002C79 RID: 11385 RVA: 0x0007891C File Offset: 0x0007791C
		public void AddToolTip(string toolTipString, IntPtr toolTipId, Rectangle iconBounds)
		{
			if (toolTipString == null)
			{
				throw new ArgumentNullException("toolTipString");
			}
			if (iconBounds.IsEmpty)
			{
				throw new ArgumentNullException("iconBounds", SR.GetString("DataGridToolTipEmptyIcon"));
			}
			NativeMethods.TOOLINFO_T toolinfo_T = new NativeMethods.TOOLINFO_T();
			toolinfo_T.cbSize = Marshal.SizeOf(toolinfo_T);
			toolinfo_T.hwnd = this.dataGrid.Handle;
			toolinfo_T.uId = toolTipId;
			toolinfo_T.lpszText = toolTipString;
			toolinfo_T.rect = NativeMethods.RECT.FromXYWH(iconBounds.X, iconBounds.Y, iconBounds.Width, iconBounds.Height);
			toolinfo_T.uFlags = 16;
			UnsafeNativeMethods.SendMessage(new HandleRef(this.tipWindow, this.tipWindow.Handle), NativeMethods.TTM_ADDTOOL, 0, toolinfo_T);
		}

		// Token: 0x06002C7A RID: 11386 RVA: 0x000789D8 File Offset: 0x000779D8
		public void RemoveToolTip(IntPtr toolTipId)
		{
			NativeMethods.TOOLINFO_T toolinfo_T = new NativeMethods.TOOLINFO_T();
			toolinfo_T.cbSize = Marshal.SizeOf(toolinfo_T);
			toolinfo_T.hwnd = this.dataGrid.Handle;
			toolinfo_T.uId = toolTipId;
			UnsafeNativeMethods.SendMessage(new HandleRef(this.tipWindow, this.tipWindow.Handle), NativeMethods.TTM_DELTOOL, 0, toolinfo_T);
		}

		// Token: 0x06002C7B RID: 11387 RVA: 0x00078A32 File Offset: 0x00077A32
		public void Destroy()
		{
			this.tipWindow.DestroyHandle();
			this.tipWindow = null;
		}

		// Token: 0x0400183D RID: 6205
		private NativeWindow tipWindow;

		// Token: 0x0400183E RID: 6206
		private DataGrid dataGrid;
	}
}
