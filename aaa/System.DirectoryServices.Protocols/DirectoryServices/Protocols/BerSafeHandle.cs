using System;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200008C RID: 140
	[SuppressUnmanagedCodeSecurity]
	internal sealed class BerSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06000314 RID: 788 RVA: 0x0000F3F0 File Offset: 0x0000E3F0
		internal BerSafeHandle()
			: base(true)
		{
			base.SetHandle(Wldap32.ber_alloc(1));
			if (this.handle == (IntPtr)0)
			{
				throw new OutOfMemoryException();
			}
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000F41E File Offset: 0x0000E41E
		internal BerSafeHandle(berval value)
			: base(true)
		{
			base.SetHandle(Wldap32.ber_init(value));
			if (this.handle == (IntPtr)0)
			{
				throw new BerConversionException();
			}
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000F44C File Offset: 0x0000E44C
		protected override bool ReleaseHandle()
		{
			Wldap32.ber_free(this.handle, 1);
			return true;
		}
	}
}
