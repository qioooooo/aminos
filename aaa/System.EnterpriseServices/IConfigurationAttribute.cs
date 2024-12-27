using System;
using System.Collections;

namespace System.EnterpriseServices
{
	// Token: 0x02000065 RID: 101
	internal interface IConfigurationAttribute
	{
		// Token: 0x06000221 RID: 545
		bool IsValidTarget(string s);

		// Token: 0x06000222 RID: 546
		bool Apply(Hashtable info);

		// Token: 0x06000223 RID: 547
		bool AfterSaveChanges(Hashtable info);
	}
}
