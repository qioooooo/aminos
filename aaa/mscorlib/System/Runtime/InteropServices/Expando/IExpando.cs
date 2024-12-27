using System;
using System.Reflection;

namespace System.Runtime.InteropServices.Expando
{
	// Token: 0x02000587 RID: 1415
	[ComVisible(true)]
	[Guid("AFBF15E6-C37C-11d2-B88E-00A0C9B471B8")]
	public interface IExpando : IReflect
	{
		// Token: 0x06003420 RID: 13344
		FieldInfo AddField(string name);

		// Token: 0x06003421 RID: 13345
		PropertyInfo AddProperty(string name);

		// Token: 0x06003422 RID: 13346
		MethodInfo AddMethod(string name, Delegate method);

		// Token: 0x06003423 RID: 13347
		void RemoveMember(MemberInfo m);
	}
}
