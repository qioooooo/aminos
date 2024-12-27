using System;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200052C RID: 1324
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IReflect instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Guid("AFBF15E5-C37C-11d2-B88E-00A0C9B471B8")]
	internal interface UCOMIReflect
	{
		// Token: 0x0600330D RID: 13069
		MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x0600330E RID: 13070
		MethodInfo GetMethod(string name, BindingFlags bindingAttr);

		// Token: 0x0600330F RID: 13071
		MethodInfo[] GetMethods(BindingFlags bindingAttr);

		// Token: 0x06003310 RID: 13072
		FieldInfo GetField(string name, BindingFlags bindingAttr);

		// Token: 0x06003311 RID: 13073
		FieldInfo[] GetFields(BindingFlags bindingAttr);

		// Token: 0x06003312 RID: 13074
		PropertyInfo GetProperty(string name, BindingFlags bindingAttr);

		// Token: 0x06003313 RID: 13075
		PropertyInfo GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x06003314 RID: 13076
		PropertyInfo[] GetProperties(BindingFlags bindingAttr);

		// Token: 0x06003315 RID: 13077
		MemberInfo[] GetMember(string name, BindingFlags bindingAttr);

		// Token: 0x06003316 RID: 13078
		MemberInfo[] GetMembers(BindingFlags bindingAttr);

		// Token: 0x06003317 RID: 13079
		object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters);

		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x06003318 RID: 13080
		Type UnderlyingSystemType { get; }
	}
}
