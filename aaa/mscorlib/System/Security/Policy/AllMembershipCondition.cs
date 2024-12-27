using System;
using System.Runtime.InteropServices;
using System.Security.Util;

namespace System.Security.Policy
{
	// Token: 0x0200047D RID: 1149
	[ComVisible(true)]
	[Serializable]
	public sealed class AllMembershipCondition : IConstantMembershipCondition, IReportMatchMembershipCondition, IMembershipCondition, ISecurityEncodable, ISecurityPolicyEncodable
	{
		// Token: 0x06002E17 RID: 11799 RVA: 0x0009C6E4 File Offset: 0x0009B6E4
		public bool Check(Evidence evidence)
		{
			object obj = null;
			return ((IReportMatchMembershipCondition)this).Check(evidence, out obj);
		}

		// Token: 0x06002E18 RID: 11800 RVA: 0x0009C6FC File Offset: 0x0009B6FC
		bool IReportMatchMembershipCondition.Check(Evidence evidence, out object usedEvidence)
		{
			usedEvidence = null;
			return true;
		}

		// Token: 0x06002E19 RID: 11801 RVA: 0x0009C702 File Offset: 0x0009B702
		public IMembershipCondition Copy()
		{
			return new AllMembershipCondition();
		}

		// Token: 0x06002E1A RID: 11802 RVA: 0x0009C709 File Offset: 0x0009B709
		public override string ToString()
		{
			return Environment.GetResourceString("All_ToString");
		}

		// Token: 0x06002E1B RID: 11803 RVA: 0x0009C715 File Offset: 0x0009B715
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		// Token: 0x06002E1C RID: 11804 RVA: 0x0009C71E File Offset: 0x0009B71E
		public void FromXml(SecurityElement e)
		{
			this.FromXml(e, null);
		}

		// Token: 0x06002E1D RID: 11805 RVA: 0x0009C728 File Offset: 0x0009B728
		public SecurityElement ToXml(PolicyLevel level)
		{
			SecurityElement securityElement = new SecurityElement("IMembershipCondition");
			XMLUtil.AddClassAttribute(securityElement, base.GetType(), "System.Security.Policy.AllMembershipCondition");
			securityElement.AddAttribute("version", "1");
			return securityElement;
		}

		// Token: 0x06002E1E RID: 11806 RVA: 0x0009C762 File Offset: 0x0009B762
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

		// Token: 0x06002E1F RID: 11807 RVA: 0x0009C794 File Offset: 0x0009B794
		public override bool Equals(object o)
		{
			return o is AllMembershipCondition;
		}

		// Token: 0x06002E20 RID: 11808 RVA: 0x0009C79F File Offset: 0x0009B79F
		public override int GetHashCode()
		{
			return typeof(AllMembershipCondition).GetHashCode();
		}
	}
}
