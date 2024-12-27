using System;
using System.Globalization;
using System.Security.Util;

namespace System.Security.Permissions
{
	// Token: 0x0200061D RID: 1565
	[Serializable]
	internal sealed class HostProtectionPermission : CodeAccessPermission, IUnrestrictedPermission, IBuiltInPermission
	{
		// Token: 0x060038F5 RID: 14581 RVA: 0x000C11B7 File Offset: 0x000C01B7
		public HostProtectionPermission(PermissionState state)
		{
			if (state == PermissionState.Unrestricted)
			{
				this.Resources = HostProtectionResource.All;
				return;
			}
			if (state == PermissionState.None)
			{
				this.Resources = HostProtectionResource.None;
				return;
			}
			throw new ArgumentException(Environment.GetResourceString("Argument_InvalidPermissionState"));
		}

		// Token: 0x060038F6 RID: 14582 RVA: 0x000C11E9 File Offset: 0x000C01E9
		public HostProtectionPermission(HostProtectionResource resources)
		{
			this.Resources = resources;
		}

		// Token: 0x060038F7 RID: 14583 RVA: 0x000C11F8 File Offset: 0x000C01F8
		public bool IsUnrestricted()
		{
			return this.Resources == HostProtectionResource.All;
		}

		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x060038F9 RID: 14585 RVA: 0x000C1253 File Offset: 0x000C0253
		// (set) Token: 0x060038F8 RID: 14584 RVA: 0x000C1208 File Offset: 0x000C0208
		public HostProtectionResource Resources
		{
			get
			{
				return this.m_resources;
			}
			set
			{
				if (value < HostProtectionResource.None || value > HostProtectionResource.All)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_EnumIllegalVal"), new object[] { (int)value }));
				}
				this.m_resources = value;
			}
		}

		// Token: 0x060038FA RID: 14586 RVA: 0x000C125C File Offset: 0x000C025C
		public override bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return this.m_resources == HostProtectionResource.None;
			}
			if (base.GetType() != target.GetType())
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_WrongType"), new object[] { base.GetType().FullName }));
			}
			return (this.m_resources & ((HostProtectionPermission)target).m_resources) == this.m_resources;
		}

		// Token: 0x060038FB RID: 14587 RVA: 0x000C12D0 File Offset: 0x000C02D0
		public override IPermission Union(IPermission target)
		{
			if (target == null)
			{
				return this.Copy();
			}
			if (base.GetType() != target.GetType())
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_WrongType"), new object[] { base.GetType().FullName }));
			}
			HostProtectionResource hostProtectionResource = this.m_resources | ((HostProtectionPermission)target).m_resources;
			return new HostProtectionPermission(hostProtectionResource);
		}

		// Token: 0x060038FC RID: 14588 RVA: 0x000C1340 File Offset: 0x000C0340
		public override IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			if (base.GetType() != target.GetType())
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_WrongType"), new object[] { base.GetType().FullName }));
			}
			HostProtectionResource hostProtectionResource = this.m_resources & ((HostProtectionPermission)target).m_resources;
			if (hostProtectionResource == HostProtectionResource.None)
			{
				return null;
			}
			return new HostProtectionPermission(hostProtectionResource);
		}

		// Token: 0x060038FD RID: 14589 RVA: 0x000C13AE File Offset: 0x000C03AE
		public override IPermission Copy()
		{
			return new HostProtectionPermission(this.m_resources);
		}

		// Token: 0x060038FE RID: 14590 RVA: 0x000C13BC File Offset: 0x000C03BC
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = CodeAccessPermission.CreatePermissionElement(this, base.GetType().FullName);
			if (this.IsUnrestricted())
			{
				securityElement.AddAttribute("Unrestricted", "true");
			}
			else
			{
				securityElement.AddAttribute("Resources", XMLUtil.BitFieldEnumToString(typeof(HostProtectionResource), this.Resources));
			}
			return securityElement;
		}

		// Token: 0x060038FF RID: 14591 RVA: 0x000C141C File Offset: 0x000C041C
		public override void FromXml(SecurityElement esd)
		{
			CodeAccessPermission.ValidateElement(esd, this);
			if (XMLUtil.IsUnrestricted(esd))
			{
				this.Resources = HostProtectionResource.All;
				return;
			}
			string text = esd.Attribute("Resources");
			if (text == null)
			{
				this.Resources = HostProtectionResource.None;
				return;
			}
			this.Resources = (HostProtectionResource)Enum.Parse(typeof(HostProtectionResource), text);
		}

		// Token: 0x06003900 RID: 14592 RVA: 0x000C1476 File Offset: 0x000C0476
		int IBuiltInPermission.GetTokenIndex()
		{
			return HostProtectionPermission.GetTokenIndex();
		}

		// Token: 0x06003901 RID: 14593 RVA: 0x000C147D File Offset: 0x000C047D
		internal static int GetTokenIndex()
		{
			return 9;
		}

		// Token: 0x04001D60 RID: 7520
		internal static HostProtectionResource protectedResources;

		// Token: 0x04001D61 RID: 7521
		private HostProtectionResource m_resources;
	}
}
