using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Drawing.Drawing2D
{
	// Token: 0x020000E1 RID: 225
	[SecurityCritical]
	internal class SafeCustomLineCapHandle : SafeHandle
	{
		// Token: 0x06000D00 RID: 3328 RVA: 0x00026C7D File Offset: 0x00025C7D
		internal SafeCustomLineCapHandle(IntPtr h)
			: base(IntPtr.Zero, true)
		{
			base.SetHandle(h);
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x00026C94 File Offset: 0x00025C94
		[SecurityCritical]
		protected override bool ReleaseHandle()
		{
			int num = 0;
			if (!this.IsInvalid)
			{
				try
				{
					num = SafeNativeMethods.Gdip.GdipDeleteCustomLineCap(new HandleRef(this, this.handle));
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
				}
				finally
				{
					this.handle = IntPtr.Zero;
				}
			}
			return num != 0;
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000D02 RID: 3330 RVA: 0x00026D00 File Offset: 0x00025D00
		public override bool IsInvalid
		{
			get
			{
				return this.handle == IntPtr.Zero;
			}
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x00026D12 File Offset: 0x00025D12
		public static implicit operator IntPtr(SafeCustomLineCapHandle handle)
		{
			if (handle != null)
			{
				return handle.handle;
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x00026D23 File Offset: 0x00025D23
		public static explicit operator SafeCustomLineCapHandle(IntPtr handle)
		{
			return new SafeCustomLineCapHandle(handle);
		}
	}
}
