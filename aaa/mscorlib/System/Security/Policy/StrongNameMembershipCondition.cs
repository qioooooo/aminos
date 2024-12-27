using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Util;

namespace System.Security.Policy
{
	// Token: 0x020004A2 RID: 1186
	[ComVisible(true)]
	[Serializable]
	public sealed class StrongNameMembershipCondition : IConstantMembershipCondition, IReportMatchMembershipCondition, IMembershipCondition, ISecurityEncodable, ISecurityPolicyEncodable
	{
		// Token: 0x06002FD5 RID: 12245 RVA: 0x000A4EA9 File Offset: 0x000A3EA9
		internal StrongNameMembershipCondition()
		{
		}

		// Token: 0x06002FD6 RID: 12246 RVA: 0x000A4EB4 File Offset: 0x000A3EB4
		public StrongNameMembershipCondition(StrongNamePublicKeyBlob blob, string name, Version version)
		{
			if (blob == null)
			{
				throw new ArgumentNullException("blob");
			}
			if (name != null && name.Equals(""))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyStrongName"));
			}
			this.m_publicKeyBlob = blob;
			this.m_name = name;
			this.m_version = version;
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x06002FD8 RID: 12248 RVA: 0x000A4F21 File Offset: 0x000A3F21
		// (set) Token: 0x06002FD7 RID: 12247 RVA: 0x000A4F0A File Offset: 0x000A3F0A
		public StrongNamePublicKeyBlob PublicKey
		{
			get
			{
				if (this.m_publicKeyBlob == null && this.m_element != null)
				{
					this.ParseKeyBlob();
				}
				return this.m_publicKeyBlob;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("PublicKey");
				}
				this.m_publicKeyBlob = value;
			}
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x06002FDA RID: 12250 RVA: 0x000A4FA4 File Offset: 0x000A3FA4
		// (set) Token: 0x06002FD9 RID: 12249 RVA: 0x000A4F40 File Offset: 0x000A3F40
		public string Name
		{
			get
			{
				if (this.m_name == null && this.m_element != null)
				{
					this.ParseName();
				}
				return this.m_name;
			}
			set
			{
				if (value == null)
				{
					if (this.m_publicKeyBlob == null && this.m_element != null)
					{
						this.ParseKeyBlob();
					}
					if (this.m_version == null && this.m_element != null)
					{
						this.ParseVersion();
					}
					this.m_element = null;
				}
				else if (value.Length == 0)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"));
				}
				this.m_name = value;
			}
		}

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x06002FDC RID: 12252 RVA: 0x000A5014 File Offset: 0x000A4014
		// (set) Token: 0x06002FDB RID: 12251 RVA: 0x000A4FC4 File Offset: 0x000A3FC4
		public Version Version
		{
			get
			{
				if (this.m_version == null && this.m_element != null)
				{
					this.ParseVersion();
				}
				return this.m_version;
			}
			set
			{
				if (value == null)
				{
					if (this.m_name == null && this.m_element != null)
					{
						this.ParseName();
					}
					if (this.m_publicKeyBlob == null && this.m_element != null)
					{
						this.ParseKeyBlob();
					}
					this.m_element = null;
				}
				this.m_version = value;
			}
		}

		// Token: 0x06002FDD RID: 12253 RVA: 0x000A5034 File Offset: 0x000A4034
		public bool Check(Evidence evidence)
		{
			object obj = null;
			return ((IReportMatchMembershipCondition)this).Check(evidence, out obj);
		}

		// Token: 0x06002FDE RID: 12254 RVA: 0x000A504C File Offset: 0x000A404C
		bool IReportMatchMembershipCondition.Check(Evidence evidence, out object usedEvidence)
		{
			usedEvidence = null;
			if (evidence == null)
			{
				return false;
			}
			IEnumerator hostEnumerator = evidence.GetHostEnumerator();
			while (hostEnumerator.MoveNext())
			{
				if (hostEnumerator.Current is StrongName)
				{
					StrongName strongName = (StrongName)hostEnumerator.Current;
					if (this.PublicKey != null && this.PublicKey.Equals(strongName.PublicKey) && (this.Name == null || (strongName.Name != null && StrongName.CompareNames(strongName.Name, this.Name))) && (this.Version == null || (strongName.Version != null && strongName.Version.CompareTo(this.Version) == 0)))
					{
						usedEvidence = strongName;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002FDF RID: 12255 RVA: 0x000A50F5 File Offset: 0x000A40F5
		public IMembershipCondition Copy()
		{
			return new StrongNameMembershipCondition(this.PublicKey, this.Name, this.Version);
		}

		// Token: 0x06002FE0 RID: 12256 RVA: 0x000A510E File Offset: 0x000A410E
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		// Token: 0x06002FE1 RID: 12257 RVA: 0x000A5117 File Offset: 0x000A4117
		public void FromXml(SecurityElement e)
		{
			this.FromXml(e, null);
		}

		// Token: 0x06002FE2 RID: 12258 RVA: 0x000A5124 File Offset: 0x000A4124
		public SecurityElement ToXml(PolicyLevel level)
		{
			SecurityElement securityElement = new SecurityElement("IMembershipCondition");
			XMLUtil.AddClassAttribute(securityElement, base.GetType(), "System.Security.Policy.StrongNameMembershipCondition");
			securityElement.AddAttribute("version", "1");
			if (this.PublicKey != null)
			{
				securityElement.AddAttribute("PublicKeyBlob", Hex.EncodeHexString(this.PublicKey.PublicKey));
			}
			if (this.Name != null)
			{
				securityElement.AddAttribute("Name", this.Name);
			}
			if (this.Version != null)
			{
				securityElement.AddAttribute("AssemblyVersion", this.Version.ToString());
			}
			return securityElement;
		}

		// Token: 0x06002FE3 RID: 12259 RVA: 0x000A51B8 File Offset: 0x000A41B8
		public void FromXml(SecurityElement e, PolicyLevel level)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (!e.Tag.Equals("IMembershipCondition"))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MembershipConditionElement"));
			}
			lock (this)
			{
				this.m_name = null;
				this.m_publicKeyBlob = null;
				this.m_version = null;
				this.m_element = e;
			}
		}

		// Token: 0x06002FE4 RID: 12260 RVA: 0x000A5234 File Offset: 0x000A4234
		private void ParseName()
		{
			lock (this)
			{
				if (this.m_element != null)
				{
					string text = this.m_element.Attribute("Name");
					this.m_name = ((text == null) ? null : text);
					if (this.m_version != null && this.m_name != null && this.m_publicKeyBlob != null)
					{
						this.m_element = null;
					}
				}
			}
		}

		// Token: 0x06002FE5 RID: 12261 RVA: 0x000A52AC File Offset: 0x000A42AC
		private void ParseKeyBlob()
		{
			lock (this)
			{
				if (this.m_element != null)
				{
					string text = this.m_element.Attribute("PublicKeyBlob");
					StrongNamePublicKeyBlob strongNamePublicKeyBlob = new StrongNamePublicKeyBlob();
					if (text == null)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_BlobCannotBeNull"));
					}
					strongNamePublicKeyBlob.PublicKey = Hex.DecodeHexString(text);
					this.m_publicKeyBlob = strongNamePublicKeyBlob;
					if (this.m_version != null && this.m_name != null && this.m_publicKeyBlob != null)
					{
						this.m_element = null;
					}
				}
			}
		}

		// Token: 0x06002FE6 RID: 12262 RVA: 0x000A5344 File Offset: 0x000A4344
		private void ParseVersion()
		{
			lock (this)
			{
				if (this.m_element != null)
				{
					string text = this.m_element.Attribute("AssemblyVersion");
					this.m_version = ((text == null) ? null : new Version(text));
					if (this.m_version != null && this.m_name != null && this.m_publicKeyBlob != null)
					{
						this.m_element = null;
					}
				}
			}
		}

		// Token: 0x06002FE7 RID: 12263 RVA: 0x000A53C0 File Offset: 0x000A43C0
		public override string ToString()
		{
			string text = "";
			string text2 = "";
			if (this.Name != null)
			{
				text = " " + string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("StrongName_Name"), new object[] { this.Name });
			}
			if (this.Version != null)
			{
				text2 = " " + string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("StrongName_Version"), new object[] { this.Version });
			}
			return string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("StrongName_ToString"), new object[]
			{
				Hex.EncodeHexString(this.PublicKey.PublicKey),
				text,
				text2
			});
		}

		// Token: 0x06002FE8 RID: 12264 RVA: 0x000A5488 File Offset: 0x000A4488
		public override bool Equals(object o)
		{
			StrongNameMembershipCondition strongNameMembershipCondition = o as StrongNameMembershipCondition;
			if (strongNameMembershipCondition != null)
			{
				if (this.m_publicKeyBlob == null && this.m_element != null)
				{
					this.ParseKeyBlob();
				}
				if (strongNameMembershipCondition.m_publicKeyBlob == null && strongNameMembershipCondition.m_element != null)
				{
					strongNameMembershipCondition.ParseKeyBlob();
				}
				if (object.Equals(this.m_publicKeyBlob, strongNameMembershipCondition.m_publicKeyBlob))
				{
					if (this.m_name == null && this.m_element != null)
					{
						this.ParseName();
					}
					if (strongNameMembershipCondition.m_name == null && strongNameMembershipCondition.m_element != null)
					{
						strongNameMembershipCondition.ParseName();
					}
					if (object.Equals(this.m_name, strongNameMembershipCondition.m_name))
					{
						if (this.m_version == null && this.m_element != null)
						{
							this.ParseVersion();
						}
						if (strongNameMembershipCondition.m_version == null && strongNameMembershipCondition.m_element != null)
						{
							strongNameMembershipCondition.ParseVersion();
						}
						if (object.Equals(this.m_version, strongNameMembershipCondition.m_version))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06002FE9 RID: 12265 RVA: 0x000A5574 File Offset: 0x000A4574
		public override int GetHashCode()
		{
			if (this.m_publicKeyBlob == null && this.m_element != null)
			{
				this.ParseKeyBlob();
			}
			if (this.m_publicKeyBlob != null)
			{
				return this.m_publicKeyBlob.GetHashCode();
			}
			if (this.m_name == null && this.m_element != null)
			{
				this.ParseName();
			}
			if (this.m_version == null && this.m_element != null)
			{
				this.ParseVersion();
			}
			if (this.m_name != null || this.m_version != null)
			{
				return ((this.m_name == null) ? 0 : this.m_name.GetHashCode()) + ((this.m_version == null) ? 0 : this.m_version.GetHashCode());
			}
			return typeof(StrongNameMembershipCondition).GetHashCode();
		}

		// Token: 0x0400180F RID: 6159
		private const string s_tagName = "Name";

		// Token: 0x04001810 RID: 6160
		private const string s_tagVersion = "AssemblyVersion";

		// Token: 0x04001811 RID: 6161
		private const string s_tagPublicKeyBlob = "PublicKeyBlob";

		// Token: 0x04001812 RID: 6162
		private StrongNamePublicKeyBlob m_publicKeyBlob;

		// Token: 0x04001813 RID: 6163
		private string m_name;

		// Token: 0x04001814 RID: 6164
		private Version m_version;

		// Token: 0x04001815 RID: 6165
		private SecurityElement m_element;
	}
}
