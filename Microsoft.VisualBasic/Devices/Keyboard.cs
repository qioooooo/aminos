using System;
using System.Security.Permissions;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic.Devices
{
	[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
	public class Keyboard
	{
		public void SendKeys(string keys)
		{
			this.SendKeys(keys, false);
		}

		public void SendKeys(string keys, bool wait)
		{
			if (wait)
			{
				global::System.Windows.Forms.SendKeys.SendWait(keys);
			}
			else
			{
				global::System.Windows.Forms.SendKeys.Send(keys);
			}
		}

		public bool ShiftKeyDown
		{
			get
			{
				Keys modifierKeys = Control.ModifierKeys;
				return (modifierKeys & Keys.Shift) > Keys.None;
			}
		}

		public bool AltKeyDown
		{
			get
			{
				Keys modifierKeys = Control.ModifierKeys;
				return (modifierKeys & Keys.Alt) > Keys.None;
			}
		}

		public bool CtrlKeyDown
		{
			get
			{
				Keys modifierKeys = Control.ModifierKeys;
				return (modifierKeys & Keys.Control) > Keys.None;
			}
		}

		public bool CapsLock
		{
			get
			{
				return (UnsafeNativeMethods.GetKeyState(20) & 1) != 0;
			}
		}

		public bool NumLock
		{
			get
			{
				return (UnsafeNativeMethods.GetKeyState(144) & 1) != 0;
			}
		}

		public bool ScrollLock
		{
			get
			{
				return (UnsafeNativeMethods.GetKeyState(145) & 1) != 0;
			}
		}
	}
}
