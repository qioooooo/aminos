using System;
using System.Collections;

namespace System.Web.Util
{
	// Token: 0x02000775 RID: 1909
	internal class AssemblySet : ObjectSet
	{
		// Token: 0x06005C68 RID: 23656 RVA: 0x00172616 File Offset: 0x00171616
		internal AssemblySet()
		{
		}

		// Token: 0x06005C69 RID: 23657 RVA: 0x00172620 File Offset: 0x00171620
		internal static AssemblySet Create(ICollection c)
		{
			AssemblySet assemblySet = new AssemblySet();
			assemblySet.AddCollection(c);
			return assemblySet;
		}
	}
}
