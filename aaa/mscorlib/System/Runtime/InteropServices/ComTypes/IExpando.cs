using System;
using System.Reflection;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000560 RID: 1376
	[Guid("AFBF15E6-C37C-11d2-B88E-00A0C9B471B8")]
	internal interface IExpando : IReflect
	{
		// Token: 0x060033A1 RID: 13217
		FieldInfo AddField(string name);

		// Token: 0x060033A2 RID: 13218
		PropertyInfo AddProperty(string name);

		// Token: 0x060033A3 RID: 13219
		MethodInfo AddMethod(string name, Delegate method);

		// Token: 0x060033A4 RID: 13220
		void RemoveMember(MemberInfo m);
	}
}
