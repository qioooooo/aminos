using System;
using System.Security;
using System.Security.Permissions;

namespace System.Web
{
	// Token: 0x0200073B RID: 1851
	[Serializable]
	public sealed class AspNetHostingPermission : CodeAccessPermission, IUnrestrictedPermission
	{
		// Token: 0x0600385B RID: 14427 RVA: 0x000EDA74 File Offset: 0x000ECA74
		internal static void VerifyAspNetHostingPermissionLevel(AspNetHostingPermissionLevel level, string arg)
		{
			if (level <= AspNetHostingPermissionLevel.Low)
			{
				if (level == AspNetHostingPermissionLevel.None || level == AspNetHostingPermissionLevel.Minimal || level == AspNetHostingPermissionLevel.Low)
				{
					return;
				}
			}
			else if (level == AspNetHostingPermissionLevel.Medium || level == AspNetHostingPermissionLevel.High || level == AspNetHostingPermissionLevel.Unrestricted)
			{
				return;
			}
			throw new ArgumentException(arg);
		}

		// Token: 0x0600385C RID: 14428 RVA: 0x000EDAC4 File Offset: 0x000ECAC4
		public AspNetHostingPermission(PermissionState state)
		{
			switch (state)
			{
			case PermissionState.None:
				this._level = AspNetHostingPermissionLevel.None;
				return;
			case PermissionState.Unrestricted:
				this._level = AspNetHostingPermissionLevel.Unrestricted;
				return;
			default:
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					state.ToString(),
					"state"
				}));
			}
		}

		// Token: 0x0600385D RID: 14429 RVA: 0x000EDB2B File Offset: 0x000ECB2B
		public AspNetHostingPermission(AspNetHostingPermissionLevel level)
		{
			AspNetHostingPermission.VerifyAspNetHostingPermissionLevel(level, "level");
			this._level = level;
		}

		// Token: 0x17000D12 RID: 3346
		// (get) Token: 0x0600385E RID: 14430 RVA: 0x000EDB45 File Offset: 0x000ECB45
		// (set) Token: 0x0600385F RID: 14431 RVA: 0x000EDB4D File Offset: 0x000ECB4D
		public AspNetHostingPermissionLevel Level
		{
			get
			{
				return this._level;
			}
			set
			{
				AspNetHostingPermission.VerifyAspNetHostingPermissionLevel(value, "Level");
				this._level = value;
			}
		}

		// Token: 0x06003860 RID: 14432 RVA: 0x000EDB61 File Offset: 0x000ECB61
		public bool IsUnrestricted()
		{
			return this._level == AspNetHostingPermissionLevel.Unrestricted;
		}

		// Token: 0x06003861 RID: 14433 RVA: 0x000EDB70 File Offset: 0x000ECB70
		public override IPermission Copy()
		{
			return new AspNetHostingPermission(this._level);
		}

		// Token: 0x06003862 RID: 14434 RVA: 0x000EDB80 File Offset: 0x000ECB80
		public override IPermission Union(IPermission target)
		{
			if (target == null)
			{
				return this.Copy();
			}
			if (target.GetType() != typeof(AspNetHostingPermission))
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					(target == null) ? "null" : target.ToString(),
					"target"
				}));
			}
			AspNetHostingPermission aspNetHostingPermission = (AspNetHostingPermission)target;
			if (this.Level >= aspNetHostingPermission.Level)
			{
				return new AspNetHostingPermission(this.Level);
			}
			return new AspNetHostingPermission(aspNetHostingPermission.Level);
		}

		// Token: 0x06003863 RID: 14435 RVA: 0x000EDC08 File Offset: 0x000ECC08
		public override IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			if (target.GetType() != typeof(AspNetHostingPermission))
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					(target == null) ? "null" : target.ToString(),
					"target"
				}));
			}
			AspNetHostingPermission aspNetHostingPermission = (AspNetHostingPermission)target;
			if (this.Level <= aspNetHostingPermission.Level)
			{
				return new AspNetHostingPermission(this.Level);
			}
			return new AspNetHostingPermission(aspNetHostingPermission.Level);
		}

		// Token: 0x06003864 RID: 14436 RVA: 0x000EDC8C File Offset: 0x000ECC8C
		public override bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return this._level == AspNetHostingPermissionLevel.None;
			}
			if (target.GetType() != typeof(AspNetHostingPermission))
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					(target == null) ? "null" : target.ToString(),
					"target"
				}));
			}
			AspNetHostingPermission aspNetHostingPermission = (AspNetHostingPermission)target;
			return this.Level <= aspNetHostingPermission.Level;
		}

		// Token: 0x06003865 RID: 14437 RVA: 0x000EDD04 File Offset: 0x000ECD04
		public override void FromXml(SecurityElement securityElement)
		{
			if (securityElement == null)
			{
				throw new ArgumentNullException(SR.GetString("AspNetHostingPermissionBadXml", new object[] { "securityElement" }));
			}
			if (!securityElement.Tag.Equals("IPermission"))
			{
				throw new ArgumentException(SR.GetString("AspNetHostingPermissionBadXml", new object[] { "securityElement" }));
			}
			string text = securityElement.Attribute("class");
			if (text == null)
			{
				throw new ArgumentException(SR.GetString("AspNetHostingPermissionBadXml", new object[] { "securityElement" }));
			}
			if (text.IndexOf(base.GetType().FullName, StringComparison.Ordinal) < 0)
			{
				throw new ArgumentException(SR.GetString("AspNetHostingPermissionBadXml", new object[] { "securityElement" }));
			}
			string text2 = securityElement.Attribute("version");
			if (string.Compare(text2, "1", StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw new ArgumentException(SR.GetString("AspNetHostingPermissionBadXml", new object[] { "version" }));
			}
			string text3 = securityElement.Attribute("Level");
			if (text3 == null)
			{
				this._level = AspNetHostingPermissionLevel.None;
				return;
			}
			this._level = (AspNetHostingPermissionLevel)Enum.Parse(typeof(AspNetHostingPermissionLevel), text3);
		}

		// Token: 0x06003866 RID: 14438 RVA: 0x000EDE44 File Offset: 0x000ECE44
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("IPermission");
			securityElement.AddAttribute("class", base.GetType().FullName + ", " + base.GetType().Module.Assembly.FullName.Replace('"', '\''));
			securityElement.AddAttribute("version", "1");
			securityElement.AddAttribute("Level", Enum.GetName(typeof(AspNetHostingPermissionLevel), this._level));
			if (this.IsUnrestricted())
			{
				securityElement.AddAttribute("Unrestricted", "true");
			}
			return securityElement;
		}

		// Token: 0x04003241 RID: 12865
		private AspNetHostingPermissionLevel _level;
	}
}
