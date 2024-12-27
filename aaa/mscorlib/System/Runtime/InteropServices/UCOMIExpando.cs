using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200052D RID: 1325
	[Guid("AFBF15E6-C37C-11d2-B88E-00A0C9B471B8")]
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IExpando instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	internal interface UCOMIExpando : UCOMIReflect
	{
		// Token: 0x06003319 RID: 13081
		FieldInfo AddField(string name);

		// Token: 0x0600331A RID: 13082
		PropertyInfo AddProperty(string name);

		// Token: 0x0600331B RID: 13083
		MethodInfo AddMethod(string name, Delegate method);

		// Token: 0x0600331C RID: 13084
		void RemoveMember(MemberInfo m);
	}
}
