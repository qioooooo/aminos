using System;
using System.Reflection;

namespace System.Runtime.Serialization.Formatters.Soap
{
	// Token: 0x0200002E RID: 46
	internal sealed class SerObjectInfoCache
	{
		// Token: 0x04000213 RID: 531
		internal string fullTypeName;

		// Token: 0x04000214 RID: 532
		internal string assemblyString;

		// Token: 0x04000215 RID: 533
		internal MemberInfo[] memberInfos;

		// Token: 0x04000216 RID: 534
		internal string[] memberNames;

		// Token: 0x04000217 RID: 535
		internal Type[] memberTypes;

		// Token: 0x04000218 RID: 536
		internal SoapAttributeInfo[] memberAttributeInfos;
	}
}
