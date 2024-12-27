using System;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200055F RID: 1375
	[Guid("AFBF15E5-C37C-11d2-B88E-00A0C9B471B8")]
	internal interface IReflect
	{
		// Token: 0x06003395 RID: 13205
		MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x06003396 RID: 13206
		MethodInfo GetMethod(string name, BindingFlags bindingAttr);

		// Token: 0x06003397 RID: 13207
		MethodInfo[] GetMethods(BindingFlags bindingAttr);

		// Token: 0x06003398 RID: 13208
		FieldInfo GetField(string name, BindingFlags bindingAttr);

		// Token: 0x06003399 RID: 13209
		FieldInfo[] GetFields(BindingFlags bindingAttr);

		// Token: 0x0600339A RID: 13210
		PropertyInfo GetProperty(string name, BindingFlags bindingAttr);

		// Token: 0x0600339B RID: 13211
		PropertyInfo GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x0600339C RID: 13212
		PropertyInfo[] GetProperties(BindingFlags bindingAttr);

		// Token: 0x0600339D RID: 13213
		MemberInfo[] GetMember(string name, BindingFlags bindingAttr);

		// Token: 0x0600339E RID: 13214
		MemberInfo[] GetMembers(BindingFlags bindingAttr);

		// Token: 0x0600339F RID: 13215
		object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters);

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x060033A0 RID: 13216
		Type UnderlyingSystemType { get; }
	}
}
