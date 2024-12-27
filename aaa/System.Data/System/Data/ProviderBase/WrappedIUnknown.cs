using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Services;

namespace System.Data.ProviderBase
{
	// Token: 0x0200024F RID: 591
	internal class WrappedIUnknown : SafeHandle
	{
		// Token: 0x06002087 RID: 8327 RVA: 0x002630F4 File Offset: 0x002624F4
		internal WrappedIUnknown()
			: base(IntPtr.Zero, true)
		{
		}

		// Token: 0x06002088 RID: 8328 RVA: 0x00263110 File Offset: 0x00262510
		internal WrappedIUnknown(object unknown)
			: this()
		{
			if (unknown != null)
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					this.handle = Marshal.GetIUnknownForObject(unknown);
				}
			}
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06002089 RID: 8329 RVA: 0x00263158 File Offset: 0x00262558
		public override bool IsInvalid
		{
			get
			{
				return IntPtr.Zero == this.handle;
			}
		}

		// Token: 0x0600208A RID: 8330 RVA: 0x00263178 File Offset: 0x00262578
		internal object ComWrapper()
		{
			object obj = null;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr = base.DangerousGetHandle();
				obj = EnterpriseServicesHelper.WrapIUnknownWithComObject(intPtr);
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
			return obj;
		}

		// Token: 0x0600208B RID: 8331 RVA: 0x002631D0 File Offset: 0x002625D0
		protected override bool ReleaseHandle()
		{
			IntPtr handle = this.handle;
			this.handle = IntPtr.Zero;
			if (IntPtr.Zero != handle)
			{
				Marshal.Release(handle);
			}
			return true;
		}
	}
}
