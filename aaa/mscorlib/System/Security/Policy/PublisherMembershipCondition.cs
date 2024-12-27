using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Util;

namespace System.Security.Policy
{
	// Token: 0x020004AD RID: 1197
	[ComVisible(true)]
	[Serializable]
	public sealed class PublisherMembershipCondition : IConstantMembershipCondition, IReportMatchMembershipCondition, IMembershipCondition, ISecurityEncodable, ISecurityPolicyEncodable
	{
		// Token: 0x06003082 RID: 12418 RVA: 0x000A7377 File Offset: 0x000A6377
		internal PublisherMembershipCondition()
		{
			this.m_element = null;
			this.m_certificate = null;
		}

		// Token: 0x06003083 RID: 12419 RVA: 0x000A738D File Offset: 0x000A638D
		public PublisherMembershipCondition(X509Certificate certificate)
		{
			PublisherMembershipCondition.CheckCertificate(certificate);
			this.m_certificate = new X509Certificate(certificate);
		}

		// Token: 0x06003084 RID: 12420 RVA: 0x000A73A7 File Offset: 0x000A63A7
		private static void CheckCertificate(X509Certificate certificate)
		{
			if (certificate == null)
			{
				throw new ArgumentNullException("certificate");
			}
		}

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x06003086 RID: 12422 RVA: 0x000A73CB File Offset: 0x000A63CB
		// (set) Token: 0x06003085 RID: 12421 RVA: 0x000A73B7 File Offset: 0x000A63B7
		public X509Certificate Certificate
		{
			get
			{
				if (this.m_certificate == null && this.m_element != null)
				{
					this.ParseCertificate();
				}
				if (this.m_certificate != null)
				{
					return new X509Certificate(this.m_certificate);
				}
				return null;
			}
			set
			{
				PublisherMembershipCondition.CheckCertificate(value);
				this.m_certificate = new X509Certificate(value);
			}
		}

		// Token: 0x06003087 RID: 12423 RVA: 0x000A73F8 File Offset: 0x000A63F8
		public override string ToString()
		{
			if (this.m_certificate == null && this.m_element != null)
			{
				this.ParseCertificate();
			}
			if (this.m_certificate == null)
			{
				return Environment.GetResourceString("Publisher_ToString");
			}
			string subject = this.m_certificate.Subject;
			if (subject != null)
			{
				return string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Publisher_ToStringArg"), new object[] { Hex.EncodeHexString(this.m_certificate.GetPublicKey()) });
			}
			return Environment.GetResourceString("Publisher_ToString");
		}

		// Token: 0x06003088 RID: 12424 RVA: 0x000A7478 File Offset: 0x000A6478
		public bool Check(Evidence evidence)
		{
			object obj = null;
			return ((IReportMatchMembershipCondition)this).Check(evidence, out obj);
		}

		// Token: 0x06003089 RID: 12425 RVA: 0x000A7490 File Offset: 0x000A6490
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
				object obj = hostEnumerator.Current;
				Publisher publisher = obj as Publisher;
				if (publisher != null)
				{
					if (this.m_certificate == null && this.m_element != null)
					{
						this.ParseCertificate();
					}
					if (publisher.Equals(new Publisher(this.m_certificate)))
					{
						usedEvidence = publisher;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600308A RID: 12426 RVA: 0x000A74F4 File Offset: 0x000A64F4
		public IMembershipCondition Copy()
		{
			if (this.m_certificate == null && this.m_element != null)
			{
				this.ParseCertificate();
			}
			return new PublisherMembershipCondition(this.m_certificate);
		}

		// Token: 0x0600308B RID: 12427 RVA: 0x000A7517 File Offset: 0x000A6517
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		// Token: 0x0600308C RID: 12428 RVA: 0x000A7520 File Offset: 0x000A6520
		public void FromXml(SecurityElement e)
		{
			this.FromXml(e, null);
		}

		// Token: 0x0600308D RID: 12429 RVA: 0x000A752C File Offset: 0x000A652C
		public SecurityElement ToXml(PolicyLevel level)
		{
			if (this.m_certificate == null && this.m_element != null)
			{
				this.ParseCertificate();
			}
			SecurityElement securityElement = new SecurityElement("IMembershipCondition");
			XMLUtil.AddClassAttribute(securityElement, base.GetType(), "System.Security.Policy.PublisherMembershipCondition");
			securityElement.AddAttribute("version", "1");
			if (this.m_certificate != null)
			{
				securityElement.AddAttribute("X509Certificate", this.m_certificate.GetRawCertDataString());
			}
			return securityElement;
		}

		// Token: 0x0600308E RID: 12430 RVA: 0x000A759C File Offset: 0x000A659C
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
				this.m_element = e;
				this.m_certificate = null;
			}
		}

		// Token: 0x0600308F RID: 12431 RVA: 0x000A7608 File Offset: 0x000A6608
		private void ParseCertificate()
		{
			lock (this)
			{
				if (this.m_element != null)
				{
					string text = this.m_element.Attribute("X509Certificate");
					this.m_certificate = ((text == null) ? null : new X509Certificate(Hex.DecodeHexString(text)));
					PublisherMembershipCondition.CheckCertificate(this.m_certificate);
					this.m_element = null;
				}
			}
		}

		// Token: 0x06003090 RID: 12432 RVA: 0x000A767C File Offset: 0x000A667C
		public override bool Equals(object o)
		{
			PublisherMembershipCondition publisherMembershipCondition = o as PublisherMembershipCondition;
			if (publisherMembershipCondition != null)
			{
				if (this.m_certificate == null && this.m_element != null)
				{
					this.ParseCertificate();
				}
				if (publisherMembershipCondition.m_certificate == null && publisherMembershipCondition.m_element != null)
				{
					publisherMembershipCondition.ParseCertificate();
				}
				if (Publisher.PublicKeyEquals(this.m_certificate, publisherMembershipCondition.m_certificate))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003091 RID: 12433 RVA: 0x000A76D5 File Offset: 0x000A66D5
		public override int GetHashCode()
		{
			if (this.m_certificate == null && this.m_element != null)
			{
				this.ParseCertificate();
			}
			if (this.m_certificate != null)
			{
				return this.m_certificate.GetHashCode();
			}
			return typeof(PublisherMembershipCondition).GetHashCode();
		}

		// Token: 0x0400182A RID: 6186
		private X509Certificate m_certificate;

		// Token: 0x0400182B RID: 6187
		private SecurityElement m_element;
	}
}
