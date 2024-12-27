using System;
using System.Reflection;

namespace System.Web.Compilation
{
	// Token: 0x02000135 RID: 309
	internal class AssemblyReferenceInfo
	{
		// Token: 0x06000EA1 RID: 3745 RVA: 0x00042885 File Offset: 0x00041885
		internal AssemblyReferenceInfo(int referenceIndex)
		{
			this.ReferenceIndex = referenceIndex;
		}

		// Token: 0x0400159D RID: 5533
		internal Assembly Assembly;

		// Token: 0x0400159E RID: 5534
		internal int ReferenceIndex;
	}
}
