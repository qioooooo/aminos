using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Security.Util;

namespace System.Security.Policy
{
	// Token: 0x02000484 RID: 1156
	[ComVisible(true)]
	[Serializable]
	public sealed class ApplicationTrust : ISecurityEncodable
	{
		// Token: 0x06002E4D RID: 11853 RVA: 0x0009D1E1 File Offset: 0x0009C1E1
		public ApplicationTrust(ApplicationIdentity applicationIdentity)
			: this()
		{
			this.ApplicationIdentity = applicationIdentity;
		}

		// Token: 0x06002E4E RID: 11854 RVA: 0x0009D1F0 File Offset: 0x0009C1F0
		public ApplicationTrust()
			: this(new PermissionSet(PermissionState.None))
		{
		}

		// Token: 0x06002E4F RID: 11855 RVA: 0x0009D1FE File Offset: 0x0009C1FE
		internal ApplicationTrust(PermissionSet defaultGrantSet)
			: this(defaultGrantSet, null)
		{
		}

		// Token: 0x06002E50 RID: 11856 RVA: 0x0009D208 File Offset: 0x0009C208
		internal ApplicationTrust(PermissionSet defaultGrantSet, StrongName[] fullTrustAssemblies)
		{
			this.DefaultGrantSet = new PolicyStatement(defaultGrantSet);
			this.FullTrustAssemblies = fullTrustAssemblies;
		}

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x06002E51 RID: 11857 RVA: 0x0009D223 File Offset: 0x0009C223
		// (set) Token: 0x06002E52 RID: 11858 RVA: 0x0009D22B File Offset: 0x0009C22B
		public ApplicationIdentity ApplicationIdentity
		{
			get
			{
				return this.m_appId;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(Environment.GetResourceString("Argument_InvalidAppId"));
				}
				this.m_appId = value;
			}
		}

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x06002E53 RID: 11859 RVA: 0x0009D247 File Offset: 0x0009C247
		// (set) Token: 0x06002E54 RID: 11860 RVA: 0x0009D263 File Offset: 0x0009C263
		public PolicyStatement DefaultGrantSet
		{
			get
			{
				if (this.m_psDefaultGrant == null)
				{
					return new PolicyStatement(new PermissionSet(PermissionState.None));
				}
				return this.m_psDefaultGrant;
			}
			set
			{
				if (value == null)
				{
					this.m_psDefaultGrant = null;
					this.m_grantSetSpecialFlags = 0;
					return;
				}
				this.m_psDefaultGrant = value;
				this.m_grantSetSpecialFlags = SecurityManager.GetSpecialFlags(this.m_psDefaultGrant.PermissionSet, null);
			}
		}

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x06002E55 RID: 11861 RVA: 0x0009D295 File Offset: 0x0009C295
		// (set) Token: 0x06002E56 RID: 11862 RVA: 0x0009D29D File Offset: 0x0009C29D
		internal StrongName[] FullTrustAssemblies
		{
			get
			{
				return this.m_fullTrustAssemblies;
			}
			set
			{
				this.m_fullTrustAssemblies = value;
			}
		}

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x06002E57 RID: 11863 RVA: 0x0009D2A6 File Offset: 0x0009C2A6
		// (set) Token: 0x06002E58 RID: 11864 RVA: 0x0009D2AE File Offset: 0x0009C2AE
		public bool IsApplicationTrustedToRun
		{
			get
			{
				return this.m_appTrustedToRun;
			}
			set
			{
				this.m_appTrustedToRun = value;
			}
		}

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x06002E59 RID: 11865 RVA: 0x0009D2B7 File Offset: 0x0009C2B7
		// (set) Token: 0x06002E5A RID: 11866 RVA: 0x0009D2BF File Offset: 0x0009C2BF
		public bool Persist
		{
			get
			{
				return this.m_persist;
			}
			set
			{
				this.m_persist = value;
			}
		}

		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x06002E5B RID: 11867 RVA: 0x0009D2C8 File Offset: 0x0009C2C8
		// (set) Token: 0x06002E5C RID: 11868 RVA: 0x0009D2F0 File Offset: 0x0009C2F0
		public object ExtraInfo
		{
			get
			{
				if (this.m_elExtraInfo != null)
				{
					this.m_extraInfo = ApplicationTrust.ObjectFromXml(this.m_elExtraInfo);
					this.m_elExtraInfo = null;
				}
				return this.m_extraInfo;
			}
			set
			{
				this.m_elExtraInfo = null;
				this.m_extraInfo = value;
			}
		}

		// Token: 0x06002E5D RID: 11869 RVA: 0x0009D300 File Offset: 0x0009C300
		public SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("ApplicationTrust");
			securityElement.AddAttribute("version", "1");
			if (this.m_appId != null)
			{
				securityElement.AddAttribute("FullName", SecurityElement.Escape(this.m_appId.FullName));
			}
			if (this.m_appTrustedToRun)
			{
				securityElement.AddAttribute("TrustedToRun", "true");
			}
			if (this.m_persist)
			{
				securityElement.AddAttribute("Persist", "true");
			}
			if (this.m_psDefaultGrant != null)
			{
				SecurityElement securityElement2 = new SecurityElement("DefaultGrant");
				securityElement2.AddChild(this.m_psDefaultGrant.ToXml());
				securityElement.AddChild(securityElement2);
			}
			if (this.m_fullTrustAssemblies != null)
			{
				SecurityElement securityElement3 = new SecurityElement("FullTrustAssemblies");
				for (int i = 0; i < this.m_fullTrustAssemblies.Length; i++)
				{
					if (this.m_fullTrustAssemblies[i] != null)
					{
						securityElement3.AddChild(this.m_fullTrustAssemblies[i].ToXml());
					}
				}
				securityElement.AddChild(securityElement3);
			}
			if (this.ExtraInfo != null)
			{
				securityElement.AddChild(ApplicationTrust.ObjectToXml("ExtraInfo", this.ExtraInfo));
			}
			return securityElement;
		}

		// Token: 0x06002E5E RID: 11870 RVA: 0x0009D410 File Offset: 0x0009C410
		public void FromXml(SecurityElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (string.Compare(element.Tag, "ApplicationTrust", StringComparison.Ordinal) != 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidXML"));
			}
			this.m_psDefaultGrant = null;
			this.m_grantSetSpecialFlags = 0;
			this.m_fullTrustAssemblies = null;
			this.m_appTrustedToRun = false;
			string text = element.Attribute("TrustedToRun");
			if (text != null && string.Compare(text, "true", StringComparison.Ordinal) == 0)
			{
				this.m_appTrustedToRun = true;
			}
			string text2 = element.Attribute("Persist");
			if (text2 != null && string.Compare(text2, "true", StringComparison.Ordinal) == 0)
			{
				this.m_persist = true;
			}
			string text3 = element.Attribute("FullName");
			if (text3 != null && text3.Length > 0)
			{
				this.m_appId = new ApplicationIdentity(text3);
			}
			SecurityElement securityElement = element.SearchForChildByTag("DefaultGrant");
			if (securityElement != null)
			{
				SecurityElement securityElement2 = securityElement.SearchForChildByTag("PolicyStatement");
				if (securityElement2 != null)
				{
					PolicyStatement policyStatement = new PolicyStatement(null);
					policyStatement.FromXml(securityElement2);
					this.m_psDefaultGrant = policyStatement;
					this.m_grantSetSpecialFlags = SecurityManager.GetSpecialFlags(policyStatement.PermissionSet, null);
				}
			}
			SecurityElement securityElement3 = element.SearchForChildByTag("FullTrustAssemblies");
			if (securityElement3 != null && securityElement3.InternalChildren != null)
			{
				this.m_fullTrustAssemblies = new StrongName[securityElement3.Children.Count];
				IEnumerator enumerator = securityElement3.Children.GetEnumerator();
				int num = 0;
				while (enumerator.MoveNext())
				{
					this.m_fullTrustAssemblies[num] = new StrongName();
					this.m_fullTrustAssemblies[num].FromXml(enumerator.Current as SecurityElement);
					num++;
				}
			}
			this.m_elExtraInfo = element.SearchForChildByTag("ExtraInfo");
		}

		// Token: 0x06002E5F RID: 11871 RVA: 0x0009D5B0 File Offset: 0x0009C5B0
		private static SecurityElement ObjectToXml(string tag, object obj)
		{
			ISecurityEncodable securityEncodable = obj as ISecurityEncodable;
			SecurityElement securityElement;
			if (securityEncodable != null)
			{
				securityElement = securityEncodable.ToXml();
				if (!securityElement.Tag.Equals(tag))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidXML"));
				}
			}
			MemoryStream memoryStream = new MemoryStream();
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(memoryStream, obj);
			byte[] array = memoryStream.ToArray();
			securityElement = new SecurityElement(tag);
			securityElement.AddAttribute("Data", Hex.EncodeHexString(array));
			return securityElement;
		}

		// Token: 0x06002E60 RID: 11872 RVA: 0x0009D624 File Offset: 0x0009C624
		private static object ObjectFromXml(SecurityElement elObject)
		{
			if (elObject.Attribute("class") != null)
			{
				ISecurityEncodable securityEncodable = XMLUtil.CreateCodeGroup(elObject) as ISecurityEncodable;
				if (securityEncodable != null)
				{
					securityEncodable.FromXml(elObject);
					return securityEncodable;
				}
			}
			string text = elObject.Attribute("Data");
			MemoryStream memoryStream = new MemoryStream(Hex.DecodeHexString(text));
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			return binaryFormatter.Deserialize(memoryStream);
		}

		// Token: 0x04001788 RID: 6024
		private ApplicationIdentity m_appId;

		// Token: 0x04001789 RID: 6025
		private bool m_appTrustedToRun;

		// Token: 0x0400178A RID: 6026
		private bool m_persist;

		// Token: 0x0400178B RID: 6027
		private object m_extraInfo;

		// Token: 0x0400178C RID: 6028
		private SecurityElement m_elExtraInfo;

		// Token: 0x0400178D RID: 6029
		private PolicyStatement m_psDefaultGrant;

		// Token: 0x0400178E RID: 6030
		private StrongName[] m_fullTrustAssemblies;

		// Token: 0x0400178F RID: 6031
		[NonSerialized]
		private int m_grantSetSpecialFlags;
	}
}
