using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.Media
{
	// Token: 0x02000221 RID: 545
	[HostProtection(SecurityAction.LinkDemand, UI = true)]
	public class SystemSound
	{
		// Token: 0x06001262 RID: 4706 RVA: 0x0003E2B3 File Offset: 0x0003D2B3
		internal SystemSound(int soundType)
		{
			this.soundType = soundType;
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x0003E2C4 File Offset: 0x0003D2C4
		public void Play()
		{
			IntSecurity.UnmanagedCode.Assert();
			try
			{
				SystemSound.SafeNativeMethods.MessageBeep(this.soundType);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x040010BB RID: 4283
		private int soundType;

		// Token: 0x02000222 RID: 546
		private class SafeNativeMethods
		{
			// Token: 0x06001264 RID: 4708 RVA: 0x0003E300 File Offset: 0x0003D300
			private SafeNativeMethods()
			{
			}

			// Token: 0x06001265 RID: 4709
			[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
			internal static extern bool MessageBeep(int type);
		}
	}
}
