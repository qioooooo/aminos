using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Data.SqlTypes
{
	// Token: 0x0200034C RID: 844
	internal class UnicodeString : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002D53 RID: 11603 RVA: 0x002AA3D4 File Offset: 0x002A97D4
		public UnicodeString(string path)
			: base(true)
		{
			this.Initialize(path);
		}

		// Token: 0x06002D54 RID: 11604 RVA: 0x002AA3F0 File Offset: 0x002A97F0
		protected override bool ReleaseHandle()
		{
			if (this.handle == IntPtr.Zero)
			{
				return true;
			}
			Marshal.FreeHGlobal(this.handle);
			this.handle = IntPtr.Zero;
			return true;
		}

		// Token: 0x06002D55 RID: 11605 RVA: 0x002AA428 File Offset: 0x002A9828
		private void Initialize(string path)
		{
			UnsafeNativeMethods.UNICODE_STRING unicode_STRING;
			unicode_STRING.length = (ushort)(path.Length * 2);
			unicode_STRING.maximumLength = (ushort)(path.Length * 2);
			unicode_STRING.buffer = path;
			IntPtr intPtr = IntPtr.Zero;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(unicode_STRING));
				if (intPtr != IntPtr.Zero)
				{
					base.SetHandle(intPtr);
				}
			}
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr2 = base.DangerousGetHandle();
				Marshal.StructureToPtr(unicode_STRING, intPtr2, false);
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
		}
	}
}
