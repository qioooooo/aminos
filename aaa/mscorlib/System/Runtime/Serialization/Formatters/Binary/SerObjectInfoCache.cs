using System;
using System.Reflection;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007E5 RID: 2021
	internal sealed class SerObjectInfoCache
	{
		// Token: 0x040024EE RID: 9454
		internal string fullTypeName;

		// Token: 0x040024EF RID: 9455
		internal string assemblyString;

		// Token: 0x040024F0 RID: 9456
		internal MemberInfo[] memberInfos;

		// Token: 0x040024F1 RID: 9457
		internal string[] memberNames;

		// Token: 0x040024F2 RID: 9458
		internal Type[] memberTypes;
	}
}
