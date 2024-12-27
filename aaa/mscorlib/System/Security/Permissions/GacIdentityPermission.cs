using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000646 RID: 1606
	[ComVisible(true)]
	[Serializable]
	public sealed class GacIdentityPermission : CodeAccessPermission, IBuiltInPermission
	{
		// Token: 0x06003A65 RID: 14949 RVA: 0x000C60A7 File Offset: 0x000C50A7
		public GacIdentityPermission(PermissionState state)
		{
			if (state == PermissionState.Unrestricted)
			{
				if (!CodeAccessSecurityEngine.DoesFullTrustMeanFullTrust())
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_UnrestrictedIdentityPermission"));
				}
				return;
			}
			else
			{
				if (state == PermissionState.None)
				{
					return;
				}
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidPermissionState"));
			}
		}

		// Token: 0x06003A66 RID: 14950 RVA: 0x000C60DE File Offset: 0x000C50DE
		public GacIdentityPermission()
		{
		}

		// Token: 0x06003A67 RID: 14951 RVA: 0x000C60E6 File Offset: 0x000C50E6
		public override IPermission Copy()
		{
			return new GacIdentityPermission();
		}

		// Token: 0x06003A68 RID: 14952 RVA: 0x000C60F0 File Offset: 0x000C50F0
		public override bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return false;
			}
			if (!(target is GacIdentityPermission))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_WrongType"), new object[] { base.GetType().FullName }));
			}
			return true;
		}

		// Token: 0x06003A69 RID: 14953 RVA: 0x000C613C File Offset: 0x000C513C
		public override IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			if (!(target is GacIdentityPermission))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_WrongType"), new object[] { base.GetType().FullName }));
			}
			return this.Copy();
		}

		// Token: 0x06003A6A RID: 14954 RVA: 0x000C618C File Offset: 0x000C518C
		public override IPermission Union(IPermission target)
		{
			if (target == null)
			{
				return this.Copy();
			}
			if (!(target is GacIdentityPermission))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_WrongType"), new object[] { base.GetType().FullName }));
			}
			return this.Copy();
		}

		// Token: 0x06003A6B RID: 14955 RVA: 0x000C61E4 File Offset: 0x000C51E4
		public override SecurityElement ToXml()
		{
			return CodeAccessPermission.CreatePermissionElement(this, "System.Security.Permissions.GacIdentityPermission");
		}

		// Token: 0x06003A6C RID: 14956 RVA: 0x000C61FE File Offset: 0x000C51FE
		public override void FromXml(SecurityElement securityElement)
		{
			CodeAccessPermission.ValidateElement(securityElement, this);
		}

		// Token: 0x06003A6D RID: 14957 RVA: 0x000C6207 File Offset: 0x000C5207
		int IBuiltInPermission.GetTokenIndex()
		{
			return GacIdentityPermission.GetTokenIndex();
		}

		// Token: 0x06003A6E RID: 14958 RVA: 0x000C620E File Offset: 0x000C520E
		internal static int GetTokenIndex()
		{
			return 15;
		}
	}
}
