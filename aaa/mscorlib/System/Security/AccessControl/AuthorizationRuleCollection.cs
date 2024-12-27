using System;
using System.Collections;

namespace System.Security.AccessControl
{
	// Token: 0x02000923 RID: 2339
	public sealed class AuthorizationRuleCollection : ReadOnlyCollectionBase
	{
		// Token: 0x060054C9 RID: 21705 RVA: 0x0013443C File Offset: 0x0013343C
		internal AuthorizationRuleCollection()
		{
		}

		// Token: 0x060054CA RID: 21706 RVA: 0x00134444 File Offset: 0x00133444
		internal void AddRule(AuthorizationRule rule)
		{
			base.InnerList.Add(rule);
		}

		// Token: 0x060054CB RID: 21707 RVA: 0x00134453 File Offset: 0x00133453
		public void CopyTo(AuthorizationRule[] rules, int index)
		{
			((ICollection)this).CopyTo(rules, index);
		}

		// Token: 0x17000EB2 RID: 3762
		public AuthorizationRule this[int index]
		{
			get
			{
				return base.InnerList[index] as AuthorizationRule;
			}
		}
	}
}
