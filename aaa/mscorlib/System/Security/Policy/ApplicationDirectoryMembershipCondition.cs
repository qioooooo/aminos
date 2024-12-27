using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Util;

namespace System.Security.Policy
{
	// Token: 0x02000480 RID: 1152
	[ComVisible(true)]
	[Serializable]
	public sealed class ApplicationDirectoryMembershipCondition : IConstantMembershipCondition, IReportMatchMembershipCondition, IMembershipCondition, ISecurityEncodable, ISecurityPolicyEncodable
	{
		// Token: 0x06002E30 RID: 11824 RVA: 0x0009C97C File Offset: 0x0009B97C
		public bool Check(Evidence evidence)
		{
			object obj = null;
			return ((IReportMatchMembershipCondition)this).Check(evidence, out obj);
		}

		// Token: 0x06002E31 RID: 11825 RVA: 0x0009C994 File Offset: 0x0009B994
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
				ApplicationDirectory applicationDirectory = obj as ApplicationDirectory;
				if (applicationDirectory != null)
				{
					IEnumerator hostEnumerator2 = evidence.GetHostEnumerator();
					while (hostEnumerator2.MoveNext())
					{
						object obj2 = hostEnumerator2.Current;
						Url url = obj2 as Url;
						if (url != null)
						{
							string text = applicationDirectory.Directory;
							if (text != null && text.Length > 1)
							{
								if (text[text.Length - 1] == '/')
								{
									text += "*";
								}
								else
								{
									text += "/*";
								}
								URLString urlstring = new URLString(text);
								if (url.GetURLString().IsSubsetOf(urlstring))
								{
									usedEvidence = applicationDirectory;
									return true;
								}
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06002E32 RID: 11826 RVA: 0x0009CA58 File Offset: 0x0009BA58
		public IMembershipCondition Copy()
		{
			return new ApplicationDirectoryMembershipCondition();
		}

		// Token: 0x06002E33 RID: 11827 RVA: 0x0009CA5F File Offset: 0x0009BA5F
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		// Token: 0x06002E34 RID: 11828 RVA: 0x0009CA68 File Offset: 0x0009BA68
		public void FromXml(SecurityElement e)
		{
			this.FromXml(e, null);
		}

		// Token: 0x06002E35 RID: 11829 RVA: 0x0009CA74 File Offset: 0x0009BA74
		public SecurityElement ToXml(PolicyLevel level)
		{
			SecurityElement securityElement = new SecurityElement("IMembershipCondition");
			XMLUtil.AddClassAttribute(securityElement, base.GetType(), "System.Security.Policy.ApplicationDirectoryMembershipCondition");
			securityElement.AddAttribute("version", "1");
			return securityElement;
		}

		// Token: 0x06002E36 RID: 11830 RVA: 0x0009CAAE File Offset: 0x0009BAAE
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
		}

		// Token: 0x06002E37 RID: 11831 RVA: 0x0009CAE0 File Offset: 0x0009BAE0
		public override bool Equals(object o)
		{
			return o is ApplicationDirectoryMembershipCondition;
		}

		// Token: 0x06002E38 RID: 11832 RVA: 0x0009CAEB File Offset: 0x0009BAEB
		public override int GetHashCode()
		{
			return typeof(ApplicationDirectoryMembershipCondition).GetHashCode();
		}

		// Token: 0x06002E39 RID: 11833 RVA: 0x0009CAFC File Offset: 0x0009BAFC
		public override string ToString()
		{
			return Environment.GetResourceString("ApplicationDirectory_ToString");
		}
	}
}
