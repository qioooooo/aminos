using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020000F2 RID: 242
	[Guid("AFBF15E5-C37C-11d2-B88E-00A0C9B471B8")]
	[ComVisible(true)]
	public interface IReflect
	{
		// Token: 0x06000D30 RID: 3376
		MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x06000D31 RID: 3377
		MethodInfo GetMethod(string name, BindingFlags bindingAttr);

		// Token: 0x06000D32 RID: 3378
		MethodInfo[] GetMethods(BindingFlags bindingAttr);

		// Token: 0x06000D33 RID: 3379
		FieldInfo GetField(string name, BindingFlags bindingAttr);

		// Token: 0x06000D34 RID: 3380
		FieldInfo[] GetFields(BindingFlags bindingAttr);

		// Token: 0x06000D35 RID: 3381
		PropertyInfo GetProperty(string name, BindingFlags bindingAttr);

		// Token: 0x06000D36 RID: 3382
		PropertyInfo GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x06000D37 RID: 3383
		PropertyInfo[] GetProperties(BindingFlags bindingAttr);

		// Token: 0x06000D38 RID: 3384
		MemberInfo[] GetMember(string name, BindingFlags bindingAttr);

		// Token: 0x06000D39 RID: 3385
		MemberInfo[] GetMembers(BindingFlags bindingAttr);

		// Token: 0x06000D3A RID: 3386
		object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters);

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000D3B RID: 3387
		Type UnderlyingSystemType { get; }
	}
}
