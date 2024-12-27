using System;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020002E6 RID: 742
	[TypeLibImportClass(typeof(PropertyInfo))]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[CLSCompliant(false)]
	[Guid("F59ED4E4-E68F-3218-BD77-061AA82824BF")]
	public interface _PropertyInfo
	{
		// Token: 0x06001D96 RID: 7574
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06001D97 RID: 7575
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06001D98 RID: 7576
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06001D99 RID: 7577
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);

		// Token: 0x06001D9A RID: 7578
		string ToString();

		// Token: 0x06001D9B RID: 7579
		bool Equals(object other);

		// Token: 0x06001D9C RID: 7580
		int GetHashCode();

		// Token: 0x06001D9D RID: 7581
		Type GetType();

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06001D9E RID: 7582
		MemberTypes MemberType { get; }

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06001D9F RID: 7583
		string Name { get; }

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06001DA0 RID: 7584
		Type DeclaringType { get; }

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06001DA1 RID: 7585
		Type ReflectedType { get; }

		// Token: 0x06001DA2 RID: 7586
		object[] GetCustomAttributes(Type attributeType, bool inherit);

		// Token: 0x06001DA3 RID: 7587
		object[] GetCustomAttributes(bool inherit);

		// Token: 0x06001DA4 RID: 7588
		bool IsDefined(Type attributeType, bool inherit);

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06001DA5 RID: 7589
		Type PropertyType { get; }

		// Token: 0x06001DA6 RID: 7590
		object GetValue(object obj, object[] index);

		// Token: 0x06001DA7 RID: 7591
		object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture);

		// Token: 0x06001DA8 RID: 7592
		void SetValue(object obj, object value, object[] index);

		// Token: 0x06001DA9 RID: 7593
		void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture);

		// Token: 0x06001DAA RID: 7594
		MethodInfo[] GetAccessors(bool nonPublic);

		// Token: 0x06001DAB RID: 7595
		MethodInfo GetGetMethod(bool nonPublic);

		// Token: 0x06001DAC RID: 7596
		MethodInfo GetSetMethod(bool nonPublic);

		// Token: 0x06001DAD RID: 7597
		ParameterInfo[] GetIndexParameters();

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06001DAE RID: 7598
		PropertyAttributes Attributes { get; }

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06001DAF RID: 7599
		bool CanRead { get; }

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06001DB0 RID: 7600
		bool CanWrite { get; }

		// Token: 0x06001DB1 RID: 7601
		MethodInfo[] GetAccessors();

		// Token: 0x06001DB2 RID: 7602
		MethodInfo GetGetMethod();

		// Token: 0x06001DB3 RID: 7603
		MethodInfo GetSetMethod();

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06001DB4 RID: 7604
		bool IsSpecialName { get; }
	}
}
