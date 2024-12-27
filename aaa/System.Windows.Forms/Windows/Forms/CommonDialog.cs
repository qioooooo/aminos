using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x0200027A RID: 634
	[ToolboxItemFilter("System.Windows.Forms")]
	public abstract class CommonDialog : Component
	{
		// Token: 0x0600221B RID: 8731 RVA: 0x0004A517 File Offset: 0x00049517
		public CommonDialog()
		{
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x0600221C RID: 8732 RVA: 0x0004A51F File Offset: 0x0004951F
		// (set) Token: 0x0600221D RID: 8733 RVA: 0x0004A527 File Offset: 0x00049527
		[Localizable(false)]
		[TypeConverter(typeof(StringConverter))]
		[SRCategory("CatData")]
		[Bindable(true)]
		[SRDescription("ControlTagDescr")]
		[DefaultValue(null)]
		public object Tag
		{
			get
			{
				return this.userData;
			}
			set
			{
				this.userData = value;
			}
		}

		// Token: 0x140000DB RID: 219
		// (add) Token: 0x0600221E RID: 8734 RVA: 0x0004A530 File Offset: 0x00049530
		// (remove) Token: 0x0600221F RID: 8735 RVA: 0x0004A543 File Offset: 0x00049543
		[SRDescription("CommonDialogHelpRequested")]
		public event EventHandler HelpRequest
		{
			add
			{
				base.Events.AddHandler(CommonDialog.EventHelpRequest, value);
			}
			remove
			{
				base.Events.RemoveHandler(CommonDialog.EventHelpRequest, value);
			}
		}

		// Token: 0x06002220 RID: 8736 RVA: 0x0004A558 File Offset: 0x00049558
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected virtual IntPtr HookProc(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam)
		{
			if (msg == 272)
			{
				CommonDialog.MoveToScreenCenter(hWnd);
				this.defaultControlHwnd = wparam;
				UnsafeNativeMethods.SetFocus(new HandleRef(null, wparam));
			}
			else if (msg == 7)
			{
				UnsafeNativeMethods.PostMessage(new HandleRef(null, hWnd), 1105, 0, 0);
			}
			else if (msg == 1105)
			{
				UnsafeNativeMethods.SetFocus(new HandleRef(this, this.defaultControlHwnd));
			}
			return IntPtr.Zero;
		}

		// Token: 0x06002221 RID: 8737 RVA: 0x0004A5C4 File Offset: 0x000495C4
		internal static void MoveToScreenCenter(IntPtr hWnd)
		{
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			UnsafeNativeMethods.GetWindowRect(new HandleRef(null, hWnd), ref rect);
			Rectangle workingArea = Screen.GetWorkingArea(Control.MousePosition);
			int num = workingArea.X + (workingArea.Width - rect.right + rect.left) / 2;
			int num2 = workingArea.Y + (workingArea.Height - rect.bottom + rect.top) / 3;
			SafeNativeMethods.SetWindowPos(new HandleRef(null, hWnd), NativeMethods.NullHandleRef, num, num2, 0, 0, 21);
		}

		// Token: 0x06002222 RID: 8738 RVA: 0x0004A650 File Offset: 0x00049650
		protected virtual void OnHelpRequest(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[CommonDialog.EventHelpRequest];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002223 RID: 8739 RVA: 0x0004A680 File Offset: 0x00049680
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected virtual IntPtr OwnerWndProc(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam)
		{
			if (msg == CommonDialog.helpMsg)
			{
				if (NativeWindow.WndProcShouldBeDebuggable)
				{
					this.OnHelpRequest(EventArgs.Empty);
				}
				else
				{
					try
					{
						this.OnHelpRequest(EventArgs.Empty);
					}
					catch (Exception ex)
					{
						Application.OnThreadException(ex);
					}
				}
				return IntPtr.Zero;
			}
			return UnsafeNativeMethods.CallWindowProc(this.defOwnerWndProc, hWnd, msg, wparam, lparam);
		}

		// Token: 0x06002224 RID: 8740
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public abstract void Reset();

		// Token: 0x06002225 RID: 8741
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected abstract bool RunDialog(IntPtr hwndOwner);

		// Token: 0x06002226 RID: 8742 RVA: 0x0004A6E8 File Offset: 0x000496E8
		public DialogResult ShowDialog()
		{
			return this.ShowDialog(null);
		}

		// Token: 0x06002227 RID: 8743 RVA: 0x0004A6F4 File Offset: 0x000496F4
		public DialogResult ShowDialog(IWin32Window owner)
		{
			IntSecurity.SafeSubWindows.Demand();
			if (!SystemInformation.UserInteractive)
			{
				throw new InvalidOperationException(SR.GetString("CantShowModalOnNonInteractive"));
			}
			NativeWindow nativeWindow = null;
			IntPtr intPtr = IntPtr.Zero;
			DialogResult dialogResult = DialogResult.Cancel;
			try
			{
				if (owner != null)
				{
					intPtr = Control.GetSafeHandle(owner);
				}
				if (intPtr == IntPtr.Zero)
				{
					intPtr = UnsafeNativeMethods.GetActiveWindow();
				}
				if (intPtr == IntPtr.Zero)
				{
					nativeWindow = new NativeWindow();
					nativeWindow.CreateHandle(new CreateParams());
					intPtr = nativeWindow.Handle;
				}
				if (CommonDialog.helpMsg == 0)
				{
					CommonDialog.helpMsg = SafeNativeMethods.RegisterWindowMessage("commdlg_help");
				}
				NativeMethods.WndProc wndProc = new NativeMethods.WndProc(this.OwnerWndProc);
				this.hookedWndProc = Marshal.GetFunctionPointerForDelegate(wndProc);
				IntPtr intPtr2 = IntPtr.Zero;
				try
				{
					this.defOwnerWndProc = UnsafeNativeMethods.SetWindowLong(new HandleRef(this, intPtr), -4, wndProc);
					if (Application.UseVisualStyles)
					{
						intPtr2 = UnsafeNativeMethods.ThemingScope.Activate();
					}
					Application.BeginModalMessageLoop();
					try
					{
						dialogResult = (this.RunDialog(intPtr) ? DialogResult.OK : DialogResult.Cancel);
					}
					finally
					{
						Application.EndModalMessageLoop();
					}
				}
				finally
				{
					IntPtr windowLong = UnsafeNativeMethods.GetWindowLong(new HandleRef(this, intPtr), -4);
					if (IntPtr.Zero != this.defOwnerWndProc || windowLong != this.hookedWndProc)
					{
						UnsafeNativeMethods.SetWindowLong(new HandleRef(this, intPtr), -4, new HandleRef(this, this.defOwnerWndProc));
					}
					UnsafeNativeMethods.ThemingScope.Deactivate(intPtr2);
					this.defOwnerWndProc = IntPtr.Zero;
					this.hookedWndProc = IntPtr.Zero;
					GC.KeepAlive(wndProc);
				}
			}
			finally
			{
				if (nativeWindow != null)
				{
					nativeWindow.DestroyHandle();
				}
			}
			return dialogResult;
		}

		// Token: 0x040014F9 RID: 5369
		private const int CDM_SETDEFAULTFOCUS = 1105;

		// Token: 0x040014FA RID: 5370
		private static readonly object EventHelpRequest = new object();

		// Token: 0x040014FB RID: 5371
		private static int helpMsg;

		// Token: 0x040014FC RID: 5372
		private IntPtr defOwnerWndProc;

		// Token: 0x040014FD RID: 5373
		private IntPtr hookedWndProc;

		// Token: 0x040014FE RID: 5374
		private IntPtr defaultControlHwnd;

		// Token: 0x040014FF RID: 5375
		private object userData;
	}
}
