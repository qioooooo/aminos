using System;
using System.Globalization;

namespace System.Security.Permissions
{
	// Token: 0x020000CE RID: 206
	[Serializable]
	public sealed class DataProtectionPermission : CodeAccessPermission, IUnrestrictedPermission
	{
		// Token: 0x0600050B RID: 1291 RVA: 0x0001956C File Offset: 0x0001856C
		public DataProtectionPermission(PermissionState state)
		{
			if (state == PermissionState.Unrestricted)
			{
				this.m_flags = DataProtectionPermissionFlags.AllFlags;
				return;
			}
			if (state == PermissionState.None)
			{
				this.m_flags = DataProtectionPermissionFlags.NoFlags;
				return;
			}
			throw new ArgumentException(SecurityResources.GetResourceString("Argument_InvalidPermissionState"));
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0001959B File Offset: 0x0001859B
		public DataProtectionPermission(DataProtectionPermissionFlags flag)
		{
			this.Flags = flag;
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600050E RID: 1294 RVA: 0x000195B9 File Offset: 0x000185B9
		// (set) Token: 0x0600050D RID: 1293 RVA: 0x000195AA File Offset: 0x000185AA
		public DataProtectionPermissionFlags Flags
		{
			get
			{
				return this.m_flags;
			}
			set
			{
				DataProtectionPermission.VerifyFlags(value);
				this.m_flags = value;
			}
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x000195C1 File Offset: 0x000185C1
		public bool IsUnrestricted()
		{
			return this.m_flags == DataProtectionPermissionFlags.AllFlags;
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x000195D0 File Offset: 0x000185D0
		public override IPermission Union(IPermission target)
		{
			if (target == null)
			{
				return this.Copy();
			}
			IPermission permission;
			try
			{
				DataProtectionPermission dataProtectionPermission = (DataProtectionPermission)target;
				DataProtectionPermissionFlags dataProtectionPermissionFlags = this.m_flags | dataProtectionPermission.m_flags;
				if (dataProtectionPermissionFlags == DataProtectionPermissionFlags.NoFlags)
				{
					permission = null;
				}
				else
				{
					permission = new DataProtectionPermission(dataProtectionPermissionFlags);
				}
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SecurityResources.GetResourceString("Argument_WrongType"), new object[] { base.GetType().FullName }));
			}
			return permission;
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00019650 File Offset: 0x00018650
		public override bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return this.m_flags == DataProtectionPermissionFlags.NoFlags;
			}
			bool flag;
			try
			{
				DataProtectionPermission dataProtectionPermission = (DataProtectionPermission)target;
				DataProtectionPermissionFlags flags = this.m_flags;
				DataProtectionPermissionFlags flags2 = dataProtectionPermission.m_flags;
				flag = (flags & flags2) == flags;
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SecurityResources.GetResourceString("Argument_WrongType"), new object[] { base.GetType().FullName }));
			}
			return flag;
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x000196D0 File Offset: 0x000186D0
		public override IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			IPermission permission;
			try
			{
				DataProtectionPermission dataProtectionPermission = (DataProtectionPermission)target;
				DataProtectionPermissionFlags dataProtectionPermissionFlags = dataProtectionPermission.m_flags & this.m_flags;
				if (dataProtectionPermissionFlags == DataProtectionPermissionFlags.NoFlags)
				{
					permission = null;
				}
				else
				{
					permission = new DataProtectionPermission(dataProtectionPermissionFlags);
				}
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SecurityResources.GetResourceString("Argument_WrongType"), new object[] { base.GetType().FullName }));
			}
			return permission;
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x0001974C File Offset: 0x0001874C
		public override IPermission Copy()
		{
			if (this.Flags == DataProtectionPermissionFlags.NoFlags)
			{
				return null;
			}
			return new DataProtectionPermission(this.m_flags);
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00019764 File Offset: 0x00018764
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("IPermission");
			securityElement.AddAttribute("class", base.GetType().FullName + ", " + base.GetType().Module.Assembly.FullName.Replace('"', '\''));
			securityElement.AddAttribute("version", "1");
			if (!this.IsUnrestricted())
			{
				securityElement.AddAttribute("Flags", this.m_flags.ToString());
			}
			else
			{
				securityElement.AddAttribute("Unrestricted", "true");
			}
			return securityElement;
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00019800 File Offset: 0x00018800
		public override void FromXml(SecurityElement securityElement)
		{
			if (securityElement == null)
			{
				throw new ArgumentNullException("securityElement");
			}
			string text = securityElement.Attribute("class");
			if (text == null || text.IndexOf(base.GetType().FullName, StringComparison.Ordinal) == -1)
			{
				throw new ArgumentException(SecurityResources.GetResourceString("Argument_InvalidClassAttribute"), "securityElement");
			}
			string text2 = securityElement.Attribute("Unrestricted");
			if (text2 != null && string.Compare(text2, "true", StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.m_flags = DataProtectionPermissionFlags.AllFlags;
				return;
			}
			this.m_flags = DataProtectionPermissionFlags.NoFlags;
			string text3 = securityElement.Attribute("Flags");
			if (text3 != null)
			{
				DataProtectionPermissionFlags dataProtectionPermissionFlags = (DataProtectionPermissionFlags)Enum.Parse(typeof(DataProtectionPermissionFlags), text3);
				DataProtectionPermission.VerifyFlags(dataProtectionPermissionFlags);
				this.m_flags = dataProtectionPermissionFlags;
			}
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x000198B4 File Offset: 0x000188B4
		internal static void VerifyFlags(DataProtectionPermissionFlags flags)
		{
			if ((flags & ~(DataProtectionPermissionFlags.ProtectData | DataProtectionPermissionFlags.UnprotectData | DataProtectionPermissionFlags.ProtectMemory | DataProtectionPermissionFlags.UnprotectMemory)) != DataProtectionPermissionFlags.NoFlags)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SecurityResources.GetResourceString("Arg_EnumIllegalVal"), new object[] { (int)flags }));
			}
		}

		// Token: 0x040005CE RID: 1486
		private DataProtectionPermissionFlags m_flags;
	}
}
