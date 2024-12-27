using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Principal
{
	// Token: 0x020004BE RID: 1214
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, SecurityInfrastructure = true)]
	[Serializable]
	public class WindowsPrincipal : IPrincipal
	{
		// Token: 0x060030D7 RID: 12503 RVA: 0x000A892C File Offset: 0x000A792C
		private WindowsPrincipal()
		{
		}

		// Token: 0x060030D8 RID: 12504 RVA: 0x000A8934 File Offset: 0x000A7934
		public WindowsPrincipal(WindowsIdentity ntIdentity)
		{
			if (ntIdentity == null)
			{
				throw new ArgumentNullException("ntIdentity");
			}
			this.m_identity = ntIdentity;
		}

		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x060030D9 RID: 12505 RVA: 0x000A8951 File Offset: 0x000A7951
		public virtual IIdentity Identity
		{
			get
			{
				return this.m_identity;
			}
		}

		// Token: 0x060030DA RID: 12506 RVA: 0x000A895C File Offset: 0x000A795C
		public virtual bool IsInRole(string role)
		{
			if (role == null || role.Length == 0)
			{
				return false;
			}
			NTAccount ntaccount = new NTAccount(role);
			IdentityReferenceCollection identityReferenceCollection = NTAccount.Translate(new IdentityReferenceCollection(1) { ntaccount }, typeof(SecurityIdentifier), false);
			SecurityIdentifier securityIdentifier = identityReferenceCollection[0] as SecurityIdentifier;
			return !(securityIdentifier == null) && this.IsInRole(securityIdentifier);
		}

		// Token: 0x060030DB RID: 12507 RVA: 0x000A89BC File Offset: 0x000A79BC
		public virtual bool IsInRole(WindowsBuiltInRole role)
		{
			if (role < WindowsBuiltInRole.Administrator || role > WindowsBuiltInRole.Replicator)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EnumIllegalVal", new object[] { (int)role }), "role");
			}
			return this.IsInRole((int)role);
		}

		// Token: 0x060030DC RID: 12508 RVA: 0x000A8A08 File Offset: 0x000A7A08
		public virtual bool IsInRole(int rid)
		{
			SecurityIdentifier securityIdentifier = new SecurityIdentifier(IdentifierAuthority.NTAuthority, new int[] { 32, rid });
			return this.IsInRole(securityIdentifier);
		}

		// Token: 0x060030DD RID: 12509 RVA: 0x000A8A38 File Offset: 0x000A7A38
		[ComVisible(false)]
		public virtual bool IsInRole(SecurityIdentifier sid)
		{
			if (sid == null)
			{
				throw new ArgumentNullException("sid");
			}
			if (this.m_identity.TokenHandle.IsInvalid)
			{
				return false;
			}
			SafeTokenHandle invalidHandle = SafeTokenHandle.InvalidHandle;
			if (this.m_identity.ImpersonationLevel == TokenImpersonationLevel.None && !Win32Native.DuplicateTokenEx(this.m_identity.TokenHandle, 8U, IntPtr.Zero, 2U, 2U, ref invalidHandle))
			{
				throw new SecurityException(Win32Native.GetMessage(Marshal.GetLastWin32Error()));
			}
			bool flag = false;
			if (!Win32Native.CheckTokenMembership((this.m_identity.ImpersonationLevel != TokenImpersonationLevel.None) ? this.m_identity.TokenHandle : invalidHandle, sid.BinaryForm, ref flag))
			{
				throw new SecurityException(Win32Native.GetMessage(Marshal.GetLastWin32Error()));
			}
			invalidHandle.Dispose();
			return flag;
		}

		// Token: 0x04001889 RID: 6281
		private WindowsIdentity m_identity;

		// Token: 0x0400188A RID: 6282
		private string[] m_roles;

		// Token: 0x0400188B RID: 6283
		private Hashtable m_rolesTable;

		// Token: 0x0400188C RID: 6284
		private bool m_rolesLoaded;
	}
}
