using System;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200008E RID: 142
	[SuppressUnmanagedCodeSecurity]
	internal sealed class ConnectionHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06000319 RID: 793 RVA: 0x0000F47C File Offset: 0x0000E47C
		internal ConnectionHandle()
			: base(true)
		{
			base.SetHandle(Wldap32.ldap_init(null, 389));
			if (!(this.handle == (IntPtr)0))
			{
				return;
			}
			int num = Wldap32.LdapGetLastError();
			if (Utility.IsLdapError((LdapError)num))
			{
				string text = LdapErrorMappings.MapResultCode(num);
				throw new LdapException(num, text);
			}
			throw new LdapException(num);
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000F4D8 File Offset: 0x0000E4D8
		protected override bool ReleaseHandle()
		{
			Wldap32.ldap_unbind(this.handle);
			return true;
		}
	}
}
