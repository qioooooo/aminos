using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Util;

namespace System.Security.Policy
{
	// Token: 0x020004A9 RID: 1193
	[ComVisible(true)]
	[Serializable]
	public sealed class GacMembershipCondition : IConstantMembershipCondition, IReportMatchMembershipCondition, IMembershipCondition, ISecurityEncodable, ISecurityPolicyEncodable
	{
		// Token: 0x0600303F RID: 12351 RVA: 0x000A6400 File Offset: 0x000A5400
		public bool Check(Evidence evidence)
		{
			object obj = null;
			return ((IReportMatchMembershipCondition)this).Check(evidence, out obj);
		}

		// Token: 0x06003040 RID: 12352 RVA: 0x000A6418 File Offset: 0x000A5418
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
				if (obj is GacInstalled)
				{
					usedEvidence = obj;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003041 RID: 12353 RVA: 0x000A6453 File Offset: 0x000A5453
		public IMembershipCondition Copy()
		{
			return new GacMembershipCondition();
		}

		// Token: 0x06003042 RID: 12354 RVA: 0x000A645A File Offset: 0x000A545A
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		// Token: 0x06003043 RID: 12355 RVA: 0x000A6463 File Offset: 0x000A5463
		public void FromXml(SecurityElement e)
		{
			this.FromXml(e, null);
		}

		// Token: 0x06003044 RID: 12356 RVA: 0x000A6470 File Offset: 0x000A5470
		public SecurityElement ToXml(PolicyLevel level)
		{
			SecurityElement securityElement = new SecurityElement("IMembershipCondition");
			XMLUtil.AddClassAttribute(securityElement, base.GetType(), base.GetType().FullName);
			securityElement.AddAttribute("version", "1");
			return securityElement;
		}

		// Token: 0x06003045 RID: 12357 RVA: 0x000A64B0 File Offset: 0x000A54B0
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

		// Token: 0x06003046 RID: 12358 RVA: 0x000A64E4 File Offset: 0x000A54E4
		public override bool Equals(object o)
		{
			return o is GacMembershipCondition;
		}

		// Token: 0x06003047 RID: 12359 RVA: 0x000A64FE File Offset: 0x000A54FE
		public override int GetHashCode()
		{
			return 0;
		}

		// Token: 0x06003048 RID: 12360 RVA: 0x000A6501 File Offset: 0x000A5501
		public override string ToString()
		{
			return Environment.GetResourceString("GAC_ToString");
		}
	}
}
