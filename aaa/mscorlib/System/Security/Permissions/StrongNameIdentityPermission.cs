using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Util;

namespace System.Security.Permissions
{
	// Token: 0x0200063E RID: 1598
	[ComVisible(true)]
	[Serializable]
	public sealed class StrongNameIdentityPermission : CodeAccessPermission, IBuiltInPermission
	{
		// Token: 0x06003A14 RID: 14868 RVA: 0x000C44A8 File Offset: 0x000C34A8
		public StrongNameIdentityPermission(PermissionState state)
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

		// Token: 0x06003A15 RID: 14869 RVA: 0x000C44F8 File Offset: 0x000C34F8
		public StrongNameIdentityPermission(StrongNamePublicKeyBlob blob, string name, Version version)
		{
			if (blob == null)
			{
				throw new ArgumentNullException("blob");
			}
			if (name != null && name.Equals(""))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyStrongName"));
			}
			this.m_unrestricted = false;
			this.m_strongNames = new StrongName2[1];
			this.m_strongNames[0] = new StrongName2(blob, name, version);
		}

		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x06003A17 RID: 14871 RVA: 0x000C45C4 File Offset: 0x000C35C4
		// (set) Token: 0x06003A16 RID: 14870 RVA: 0x000C455C File Offset: 0x000C355C
		public StrongNamePublicKeyBlob PublicKey
		{
			get
			{
				if (this.m_strongNames == null || this.m_strongNames.Length == 0)
				{
					return null;
				}
				if (this.m_strongNames.Length > 1)
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_AmbiguousIdentity"));
				}
				return this.m_strongNames[0].m_publicKeyBlob;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("PublicKey");
				}
				this.m_unrestricted = false;
				if (this.m_strongNames != null && this.m_strongNames.Length == 1)
				{
					this.m_strongNames[0].m_publicKeyBlob = value;
					return;
				}
				this.m_strongNames = new StrongName2[1];
				this.m_strongNames[0] = new StrongName2(value, "", new Version());
			}
		}

		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x06003A19 RID: 14873 RVA: 0x000C4678 File Offset: 0x000C3678
		// (set) Token: 0x06003A18 RID: 14872 RVA: 0x000C4604 File Offset: 0x000C3604
		public string Name
		{
			get
			{
				if (this.m_strongNames == null || this.m_strongNames.Length == 0)
				{
					return "";
				}
				if (this.m_strongNames.Length > 1)
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_AmbiguousIdentity"));
				}
				return this.m_strongNames[0].m_name;
			}
			set
			{
				if (value != null && value.Length == 0)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"));
				}
				this.m_unrestricted = false;
				if (this.m_strongNames != null && this.m_strongNames.Length == 1)
				{
					this.m_strongNames[0].m_name = value;
					return;
				}
				this.m_strongNames = new StrongName2[1];
				this.m_strongNames[0] = new StrongName2(null, value, new Version());
			}
		}

		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x06003A1B RID: 14875 RVA: 0x000C4720 File Offset: 0x000C3720
		// (set) Token: 0x06003A1A RID: 14874 RVA: 0x000C46C8 File Offset: 0x000C36C8
		public Version Version
		{
			get
			{
				if (this.m_strongNames == null || this.m_strongNames.Length == 0)
				{
					return new Version();
				}
				if (this.m_strongNames.Length > 1)
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_AmbiguousIdentity"));
				}
				return this.m_strongNames[0].m_version;
			}
			set
			{
				this.m_unrestricted = false;
				if (this.m_strongNames != null && this.m_strongNames.Length == 1)
				{
					this.m_strongNames[0].m_version = value;
					return;
				}
				this.m_strongNames = new StrongName2[1];
				this.m_strongNames[0] = new StrongName2(null, "", value);
			}
		}

		// Token: 0x06003A1C RID: 14876 RVA: 0x000C4770 File Offset: 0x000C3770
		public override IPermission Copy()
		{
			StrongNameIdentityPermission strongNameIdentityPermission = new StrongNameIdentityPermission(PermissionState.None);
			strongNameIdentityPermission.m_unrestricted = this.m_unrestricted;
			if (this.m_strongNames != null)
			{
				strongNameIdentityPermission.m_strongNames = new StrongName2[this.m_strongNames.Length];
				for (int i = 0; i < this.m_strongNames.Length; i++)
				{
					strongNameIdentityPermission.m_strongNames[i] = this.m_strongNames[i].Copy();
				}
			}
			return strongNameIdentityPermission;
		}

		// Token: 0x06003A1D RID: 14877 RVA: 0x000C47D4 File Offset: 0x000C37D4
		public override bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return !this.m_unrestricted && (this.m_strongNames == null || this.m_strongNames.Length == 0);
			}
			StrongNameIdentityPermission strongNameIdentityPermission = target as StrongNameIdentityPermission;
			if (strongNameIdentityPermission == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_WrongType"), new object[] { base.GetType().FullName }));
			}
			if (strongNameIdentityPermission.m_unrestricted)
			{
				return true;
			}
			if (this.m_unrestricted)
			{
				return false;
			}
			if (this.m_strongNames != null)
			{
				foreach (StrongName2 strongName in this.m_strongNames)
				{
					bool flag = false;
					if (strongNameIdentityPermission.m_strongNames != null)
					{
						foreach (StrongName2 strongName2 in strongNameIdentityPermission.m_strongNames)
						{
							if (strongName.IsSubsetOf(strongName2))
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

		// Token: 0x06003A1E RID: 14878 RVA: 0x000C48C4 File Offset: 0x000C38C4
		public override IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			StrongNameIdentityPermission strongNameIdentityPermission = target as StrongNameIdentityPermission;
			if (strongNameIdentityPermission == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_WrongType"), new object[] { base.GetType().FullName }));
			}
			if (this.m_unrestricted && strongNameIdentityPermission.m_unrestricted)
			{
				return new StrongNameIdentityPermission(PermissionState.None)
				{
					m_unrestricted = true
				};
			}
			if (this.m_unrestricted)
			{
				return strongNameIdentityPermission.Copy();
			}
			if (strongNameIdentityPermission.m_unrestricted)
			{
				return this.Copy();
			}
			if (this.m_strongNames == null || strongNameIdentityPermission.m_strongNames == null || this.m_strongNames.Length == 0 || strongNameIdentityPermission.m_strongNames.Length == 0)
			{
				return null;
			}
			ArrayList arrayList = new ArrayList();
			foreach (StrongName2 strongName in this.m_strongNames)
			{
				foreach (StrongName2 strongName2 in strongNameIdentityPermission.m_strongNames)
				{
					StrongName2 strongName3 = strongName.Intersect(strongName2);
					if (strongName3 != null)
					{
						arrayList.Add(strongName3);
					}
				}
			}
			if (arrayList.Count == 0)
			{
				return null;
			}
			return new StrongNameIdentityPermission(PermissionState.None)
			{
				m_strongNames = (StrongName2[])arrayList.ToArray(typeof(StrongName2))
			};
		}

		// Token: 0x06003A1F RID: 14879 RVA: 0x000C4A08 File Offset: 0x000C3A08
		public override IPermission Union(IPermission target)
		{
			if (target == null)
			{
				if ((this.m_strongNames == null || this.m_strongNames.Length == 0) && !this.m_unrestricted)
				{
					return null;
				}
				return this.Copy();
			}
			else
			{
				StrongNameIdentityPermission strongNameIdentityPermission = target as StrongNameIdentityPermission;
				if (strongNameIdentityPermission == null)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_WrongType"), new object[] { base.GetType().FullName }));
				}
				if (this.m_unrestricted || strongNameIdentityPermission.m_unrestricted)
				{
					return new StrongNameIdentityPermission(PermissionState.None)
					{
						m_unrestricted = true
					};
				}
				if (this.m_strongNames == null || this.m_strongNames.Length == 0)
				{
					if (strongNameIdentityPermission.m_strongNames == null || strongNameIdentityPermission.m_strongNames.Length == 0)
					{
						return null;
					}
					return strongNameIdentityPermission.Copy();
				}
				else
				{
					if (strongNameIdentityPermission.m_strongNames == null || strongNameIdentityPermission.m_strongNames.Length == 0)
					{
						return this.Copy();
					}
					ArrayList arrayList = new ArrayList();
					foreach (StrongName2 strongName in this.m_strongNames)
					{
						arrayList.Add(strongName);
					}
					foreach (StrongName2 strongName2 in strongNameIdentityPermission.m_strongNames)
					{
						bool flag = false;
						foreach (object obj in arrayList)
						{
							StrongName2 strongName3 = (StrongName2)obj;
							if (strongName2.Equals(strongName3))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							arrayList.Add(strongName2);
						}
					}
					return new StrongNameIdentityPermission(PermissionState.None)
					{
						m_strongNames = (StrongName2[])arrayList.ToArray(typeof(StrongName2))
					};
				}
			}
		}

		// Token: 0x06003A20 RID: 14880 RVA: 0x000C4BC0 File Offset: 0x000C3BC0
		public override void FromXml(SecurityElement e)
		{
			this.m_unrestricted = false;
			this.m_strongNames = null;
			CodeAccessPermission.ValidateElement(e, this);
			string text = e.Attribute("Unrestricted");
			if (text != null && string.Compare(text, "true", StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.m_unrestricted = true;
				return;
			}
			string text2 = e.Attribute("PublicKeyBlob");
			string text3 = e.Attribute("Name");
			string text4 = e.Attribute("AssemblyVersion");
			ArrayList arrayList = new ArrayList();
			if (text2 != null || text3 != null || text4 != null)
			{
				StrongName2 strongName = new StrongName2((text2 == null) ? null : new StrongNamePublicKeyBlob(text2), text3, (text4 == null) ? null : new Version(text4));
				arrayList.Add(strongName);
			}
			ArrayList children = e.Children;
			if (children != null)
			{
				foreach (object obj in children)
				{
					SecurityElement securityElement = (SecurityElement)obj;
					text2 = securityElement.Attribute("PublicKeyBlob");
					text3 = securityElement.Attribute("Name");
					text4 = securityElement.Attribute("AssemblyVersion");
					if (text2 != null || text3 != null || text4 != null)
					{
						StrongName2 strongName = new StrongName2((text2 == null) ? null : new StrongNamePublicKeyBlob(text2), text3, (text4 == null) ? null : new Version(text4));
						arrayList.Add(strongName);
					}
				}
			}
			if (arrayList.Count != 0)
			{
				this.m_strongNames = (StrongName2[])arrayList.ToArray(typeof(StrongName2));
			}
		}

		// Token: 0x06003A21 RID: 14881 RVA: 0x000C4D3C File Offset: 0x000C3D3C
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = CodeAccessPermission.CreatePermissionElement(this, "System.Security.Permissions.StrongNameIdentityPermission");
			if (this.m_unrestricted)
			{
				securityElement.AddAttribute("Unrestricted", "true");
			}
			else if (this.m_strongNames != null)
			{
				if (this.m_strongNames.Length == 1)
				{
					if (this.m_strongNames[0].m_publicKeyBlob != null)
					{
						securityElement.AddAttribute("PublicKeyBlob", Hex.EncodeHexString(this.m_strongNames[0].m_publicKeyBlob.PublicKey));
					}
					if (this.m_strongNames[0].m_name != null)
					{
						securityElement.AddAttribute("Name", this.m_strongNames[0].m_name);
					}
					if (this.m_strongNames[0].m_version != null)
					{
						securityElement.AddAttribute("AssemblyVersion", this.m_strongNames[0].m_version.ToString());
					}
				}
				else
				{
					for (int i = 0; i < this.m_strongNames.Length; i++)
					{
						SecurityElement securityElement2 = new SecurityElement("StrongName");
						if (this.m_strongNames[i].m_publicKeyBlob != null)
						{
							securityElement2.AddAttribute("PublicKeyBlob", Hex.EncodeHexString(this.m_strongNames[i].m_publicKeyBlob.PublicKey));
						}
						if (this.m_strongNames[i].m_name != null)
						{
							securityElement2.AddAttribute("Name", this.m_strongNames[i].m_name);
						}
						if (this.m_strongNames[i].m_version != null)
						{
							securityElement2.AddAttribute("AssemblyVersion", this.m_strongNames[i].m_version.ToString());
						}
						securityElement.AddChild(securityElement2);
					}
				}
			}
			return securityElement;
		}

		// Token: 0x06003A22 RID: 14882 RVA: 0x000C4EC7 File Offset: 0x000C3EC7
		int IBuiltInPermission.GetTokenIndex()
		{
			return StrongNameIdentityPermission.GetTokenIndex();
		}

		// Token: 0x06003A23 RID: 14883 RVA: 0x000C4ECE File Offset: 0x000C3ECE
		internal static int GetTokenIndex()
		{
			return 12;
		}

		// Token: 0x04001E07 RID: 7687
		private bool m_unrestricted;

		// Token: 0x04001E08 RID: 7688
		private StrongName2[] m_strongNames;
	}
}
