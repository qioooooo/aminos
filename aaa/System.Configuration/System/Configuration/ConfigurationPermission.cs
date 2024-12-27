using System;
using System.Security;
using System.Security.Permissions;

namespace System.Configuration
{
	// Token: 0x02000036 RID: 54
	[Serializable]
	public sealed class ConfigurationPermission : CodeAccessPermission, IUnrestrictedPermission
	{
		// Token: 0x0600028B RID: 651 RVA: 0x0001035C File Offset: 0x0000F35C
		public ConfigurationPermission(PermissionState state)
		{
			switch (state)
			{
			case PermissionState.None:
			case PermissionState.Unrestricted:
				this._permissionState = state;
				return;
			default:
				throw ExceptionUtil.ParameterInvalid("state");
			}
		}

		// Token: 0x0600028C RID: 652 RVA: 0x00010393 File Offset: 0x0000F393
		public bool IsUnrestricted()
		{
			return this._permissionState == PermissionState.Unrestricted;
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0001039E File Offset: 0x0000F39E
		public override IPermission Copy()
		{
			return new ConfigurationPermission(this._permissionState);
		}

		// Token: 0x0600028E RID: 654 RVA: 0x000103AC File Offset: 0x0000F3AC
		public override IPermission Union(IPermission target)
		{
			if (target == null)
			{
				return this.Copy();
			}
			if (target.GetType() != typeof(ConfigurationPermission))
			{
				throw ExceptionUtil.ParameterInvalid("target");
			}
			if (this._permissionState == PermissionState.Unrestricted)
			{
				return new ConfigurationPermission(PermissionState.Unrestricted);
			}
			ConfigurationPermission configurationPermission = (ConfigurationPermission)target;
			return new ConfigurationPermission(configurationPermission._permissionState);
		}

		// Token: 0x0600028F RID: 655 RVA: 0x00010404 File Offset: 0x0000F404
		public override IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			if (target.GetType() != typeof(ConfigurationPermission))
			{
				throw ExceptionUtil.ParameterInvalid("target");
			}
			if (this._permissionState == PermissionState.None)
			{
				return new ConfigurationPermission(PermissionState.None);
			}
			ConfigurationPermission configurationPermission = (ConfigurationPermission)target;
			return new ConfigurationPermission(configurationPermission._permissionState);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x00010454 File Offset: 0x0000F454
		public override bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return this._permissionState == PermissionState.None;
			}
			if (target.GetType() != typeof(ConfigurationPermission))
			{
				throw ExceptionUtil.ParameterInvalid("target");
			}
			ConfigurationPermission configurationPermission = (ConfigurationPermission)target;
			return this._permissionState == PermissionState.None || configurationPermission._permissionState == PermissionState.Unrestricted;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x000104A8 File Offset: 0x0000F4A8
		public override void FromXml(SecurityElement securityElement)
		{
			if (securityElement == null)
			{
				throw new ArgumentNullException(SR.GetString("ConfigurationPermissionBadXml", new object[] { "securityElement" }));
			}
			if (!securityElement.Tag.Equals("IPermission"))
			{
				throw new ArgumentException(SR.GetString("ConfigurationPermissionBadXml", new object[] { "securityElement" }));
			}
			string text = securityElement.Attribute("class");
			if (text == null)
			{
				throw new ArgumentException(SR.GetString("ConfigurationPermissionBadXml", new object[] { "securityElement" }));
			}
			if (text.IndexOf(base.GetType().FullName, StringComparison.Ordinal) < 0)
			{
				throw new ArgumentException(SR.GetString("ConfigurationPermissionBadXml", new object[] { "securityElement" }));
			}
			string text2 = securityElement.Attribute("version");
			if (text2 != "1")
			{
				throw new ArgumentException(SR.GetString("ConfigurationPermissionBadXml", new object[] { "version" }));
			}
			string text3 = securityElement.Attribute("Unrestricted");
			if (text3 == null)
			{
				this._permissionState = PermissionState.None;
				return;
			}
			string text4;
			if ((text4 = text3) != null)
			{
				if (text4 == "true")
				{
					this._permissionState = PermissionState.Unrestricted;
					return;
				}
				if (text4 == "false")
				{
					this._permissionState = PermissionState.None;
					return;
				}
			}
			throw new ArgumentException(SR.GetString("ConfigurationPermissionBadXml", new object[] { "Unrestricted" }));
		}

		// Token: 0x06000292 RID: 658 RVA: 0x00010620 File Offset: 0x0000F620
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("IPermission");
			securityElement.AddAttribute("class", base.GetType().FullName + ", " + base.GetType().Module.Assembly.FullName.Replace('"', '\''));
			securityElement.AddAttribute("version", "1");
			if (this.IsUnrestricted())
			{
				securityElement.AddAttribute("Unrestricted", "true");
			}
			return securityElement;
		}

		// Token: 0x04000276 RID: 630
		private PermissionState _permissionState;
	}
}
