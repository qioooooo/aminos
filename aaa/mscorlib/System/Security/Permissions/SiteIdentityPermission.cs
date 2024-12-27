using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Util;

namespace System.Security.Permissions
{
	// Token: 0x0200063C RID: 1596
	[ComVisible(true)]
	[Serializable]
	public sealed class SiteIdentityPermission : CodeAccessPermission, IBuiltInPermission
	{
		// Token: 0x06003A00 RID: 14848 RVA: 0x000C3C3C File Offset: 0x000C2C3C
		[OnDeserialized]
		private void OnDeserialized(StreamingContext ctx)
		{
			if (this.m_serializedPermission != null)
			{
				this.FromXml(SecurityElement.FromString(this.m_serializedPermission));
				this.m_serializedPermission = null;
				return;
			}
			if (this.m_site != null)
			{
				this.m_unrestricted = false;
				this.m_sites = new SiteString[1];
				this.m_sites[0] = this.m_site;
				this.m_site = null;
			}
		}

		// Token: 0x06003A01 RID: 14849 RVA: 0x000C3C9C File Offset: 0x000C2C9C
		[OnSerializing]
		private void OnSerializing(StreamingContext ctx)
		{
			if ((ctx.State & ~(StreamingContextStates.Clone | StreamingContextStates.CrossAppDomain)) != (StreamingContextStates)0)
			{
				this.m_serializedPermission = this.ToXml().ToString();
				if (this.m_sites != null && this.m_sites.Length == 1)
				{
					this.m_site = this.m_sites[0];
				}
			}
		}

		// Token: 0x06003A02 RID: 14850 RVA: 0x000C3CEA File Offset: 0x000C2CEA
		[OnSerialized]
		private void OnSerialized(StreamingContext ctx)
		{
			if ((ctx.State & ~(StreamingContextStates.Clone | StreamingContextStates.CrossAppDomain)) != (StreamingContextStates)0)
			{
				this.m_serializedPermission = null;
				this.m_site = null;
			}
		}

		// Token: 0x06003A03 RID: 14851 RVA: 0x000C3D0C File Offset: 0x000C2D0C
		public SiteIdentityPermission(PermissionState state)
		{
			if (state == PermissionState.Unrestricted)
			{
				if (CodeAccessSecurityEngine.DoesFullTrustMeanFullTrust())
				{
					this.m_unrestricted = true;
					return;
				}
				throw new ArgumentException(Environment.GetResourceString("Argument_UnrestrictedIdentityPermission"));
			}
			else
			{
				if (state == PermissionState.None)
				{
					this.m_unrestricted = false;
					return;
				}
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidPermissionState"));
			}
		}

		// Token: 0x06003A04 RID: 14852 RVA: 0x000C3D5C File Offset: 0x000C2D5C
		public SiteIdentityPermission(string site)
		{
			this.Site = site;
		}

		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x06003A06 RID: 14854 RVA: 0x000C3D8E File Offset: 0x000C2D8E
		// (set) Token: 0x06003A05 RID: 14853 RVA: 0x000C3D6B File Offset: 0x000C2D6B
		public string Site
		{
			get
			{
				if (this.m_sites == null)
				{
					return "";
				}
				if (this.m_sites.Length == 1)
				{
					return this.m_sites[0].ToString();
				}
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_AmbiguousIdentity"));
			}
			set
			{
				this.m_unrestricted = false;
				this.m_sites = new SiteString[1];
				this.m_sites[0] = new SiteString(value);
			}
		}

		// Token: 0x06003A07 RID: 14855 RVA: 0x000C3DC8 File Offset: 0x000C2DC8
		public override IPermission Copy()
		{
			SiteIdentityPermission siteIdentityPermission = new SiteIdentityPermission(PermissionState.None);
			siteIdentityPermission.m_unrestricted = this.m_unrestricted;
			if (this.m_sites != null)
			{
				siteIdentityPermission.m_sites = new SiteString[this.m_sites.Length];
				for (int i = 0; i < this.m_sites.Length; i++)
				{
					siteIdentityPermission.m_sites[i] = this.m_sites[i].Copy();
				}
			}
			return siteIdentityPermission;
		}

		// Token: 0x06003A08 RID: 14856 RVA: 0x000C3E2C File Offset: 0x000C2E2C
		public override bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return !this.m_unrestricted && (this.m_sites == null || this.m_sites.Length == 0);
			}
			SiteIdentityPermission siteIdentityPermission = target as SiteIdentityPermission;
			if (siteIdentityPermission == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_WrongType"), new object[] { base.GetType().FullName }));
			}
			if (siteIdentityPermission.m_unrestricted)
			{
				return true;
			}
			if (this.m_unrestricted)
			{
				return false;
			}
			if (this.m_sites != null)
			{
				foreach (SiteString siteString in this.m_sites)
				{
					bool flag = false;
					if (siteIdentityPermission.m_sites != null)
					{
						foreach (SiteString siteString2 in siteIdentityPermission.m_sites)
						{
							if (siteString.IsSubsetOf(siteString2))
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06003A09 RID: 14857 RVA: 0x000C3F1C File Offset: 0x000C2F1C
		public override IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			SiteIdentityPermission siteIdentityPermission = target as SiteIdentityPermission;
			if (siteIdentityPermission == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_WrongType"), new object[] { base.GetType().FullName }));
			}
			if (this.m_unrestricted && siteIdentityPermission.m_unrestricted)
			{
				return new SiteIdentityPermission(PermissionState.None)
				{
					m_unrestricted = true
				};
			}
			if (this.m_unrestricted)
			{
				return siteIdentityPermission.Copy();
			}
			if (siteIdentityPermission.m_unrestricted)
			{
				return this.Copy();
			}
			if (this.m_sites == null || siteIdentityPermission.m_sites == null || this.m_sites.Length == 0 || siteIdentityPermission.m_sites.Length == 0)
			{
				return null;
			}
			ArrayList arrayList = new ArrayList();
			foreach (SiteString siteString in this.m_sites)
			{
				foreach (SiteString siteString2 in siteIdentityPermission.m_sites)
				{
					SiteString siteString3 = siteString.Intersect(siteString2);
					if (siteString3 != null)
					{
						arrayList.Add(siteString3);
					}
				}
			}
			if (arrayList.Count == 0)
			{
				return null;
			}
			return new SiteIdentityPermission(PermissionState.None)
			{
				m_sites = (SiteString[])arrayList.ToArray(typeof(SiteString))
			};
		}

		// Token: 0x06003A0A RID: 14858 RVA: 0x000C4060 File Offset: 0x000C3060
		public override IPermission Union(IPermission target)
		{
			if (target == null)
			{
				if ((this.m_sites == null || this.m_sites.Length == 0) && !this.m_unrestricted)
				{
					return null;
				}
				return this.Copy();
			}
			else
			{
				SiteIdentityPermission siteIdentityPermission = target as SiteIdentityPermission;
				if (siteIdentityPermission == null)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_WrongType"), new object[] { base.GetType().FullName }));
				}
				if (this.m_unrestricted || siteIdentityPermission.m_unrestricted)
				{
					return new SiteIdentityPermission(PermissionState.None)
					{
						m_unrestricted = true
					};
				}
				if (this.m_sites == null || this.m_sites.Length == 0)
				{
					if (siteIdentityPermission.m_sites == null || siteIdentityPermission.m_sites.Length == 0)
					{
						return null;
					}
					return siteIdentityPermission.Copy();
				}
				else
				{
					if (siteIdentityPermission.m_sites == null || siteIdentityPermission.m_sites.Length == 0)
					{
						return this.Copy();
					}
					ArrayList arrayList = new ArrayList();
					foreach (SiteString siteString in this.m_sites)
					{
						arrayList.Add(siteString);
					}
					foreach (SiteString siteString2 in siteIdentityPermission.m_sites)
					{
						bool flag = false;
						foreach (object obj in arrayList)
						{
							SiteString siteString3 = (SiteString)obj;
							if (siteString2.Equals(siteString3))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							arrayList.Add(siteString2);
						}
					}
					return new SiteIdentityPermission(PermissionState.None)
					{
						m_sites = (SiteString[])arrayList.ToArray(typeof(SiteString))
					};
				}
			}
		}

		// Token: 0x06003A0B RID: 14859 RVA: 0x000C4218 File Offset: 0x000C3218
		public override void FromXml(SecurityElement esd)
		{
			this.m_unrestricted = false;
			this.m_sites = null;
			CodeAccessPermission.ValidateElement(esd, this);
			string text = esd.Attribute("Unrestricted");
			if (text != null && string.Compare(text, "true", StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.m_unrestricted = true;
				return;
			}
			string text2 = esd.Attribute("Site");
			ArrayList arrayList = new ArrayList();
			if (text2 != null)
			{
				arrayList.Add(new SiteString(text2));
			}
			ArrayList children = esd.Children;
			if (children != null)
			{
				foreach (object obj in children)
				{
					SecurityElement securityElement = (SecurityElement)obj;
					text2 = securityElement.Attribute("Site");
					if (text2 != null)
					{
						arrayList.Add(new SiteString(text2));
					}
				}
			}
			if (arrayList.Count != 0)
			{
				this.m_sites = (SiteString[])arrayList.ToArray(typeof(SiteString));
			}
		}

		// Token: 0x06003A0C RID: 14860 RVA: 0x000C4314 File Offset: 0x000C3314
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = CodeAccessPermission.CreatePermissionElement(this, "System.Security.Permissions.SiteIdentityPermission");
			if (this.m_unrestricted)
			{
				securityElement.AddAttribute("Unrestricted", "true");
			}
			else if (this.m_sites != null)
			{
				if (this.m_sites.Length == 1)
				{
					securityElement.AddAttribute("Site", this.m_sites[0].ToString());
				}
				else
				{
					for (int i = 0; i < this.m_sites.Length; i++)
					{
						SecurityElement securityElement2 = new SecurityElement("Site");
						securityElement2.AddAttribute("Site", this.m_sites[i].ToString());
						securityElement.AddChild(securityElement2);
					}
				}
			}
			return securityElement;
		}

		// Token: 0x06003A0D RID: 14861 RVA: 0x000C43B2 File Offset: 0x000C33B2
		int IBuiltInPermission.GetTokenIndex()
		{
			return SiteIdentityPermission.GetTokenIndex();
		}

		// Token: 0x06003A0E RID: 14862 RVA: 0x000C43B9 File Offset: 0x000C33B9
		internal static int GetTokenIndex()
		{
			return 11;
		}

		// Token: 0x04001E00 RID: 7680
		[OptionalField(VersionAdded = 2)]
		private bool m_unrestricted;

		// Token: 0x04001E01 RID: 7681
		[OptionalField(VersionAdded = 2)]
		private SiteString[] m_sites;

		// Token: 0x04001E02 RID: 7682
		[OptionalField(VersionAdded = 2)]
		private string m_serializedPermission;

		// Token: 0x04001E03 RID: 7683
		private SiteString m_site;
	}
}
