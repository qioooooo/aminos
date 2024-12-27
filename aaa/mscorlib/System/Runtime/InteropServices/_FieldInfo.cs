using System;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020002E5 RID: 741
	[TypeLibImportClass(typeof(FieldInfo))]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[CLSCompliant(false)]
	[Guid("8A7C1442-A9FB-366B-80D8-4939FFA6DBE0")]
	[ComVisible(true)]
	public interface _FieldInfo
	{
		// Token: 0x06001D73 RID: 7539
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06001D74 RID: 7540
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06001D75 RID: 7541
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06001D76 RID: 7542
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);

		// Token: 0x06001D77 RID: 7543
		string ToString();

		// Token: 0x06001D78 RID: 7544
		bool Equals(object other);

		// Token: 0x06001D79 RID: 7545
		int GetHashCode();

		// Token: 0x06001D7A RID: 7546
		Type GetType();

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06001D7B RID: 7547
		MemberTypes MemberType { get; }

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06001D7C RID: 7548
		string Name { get; }

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06001D7D RID: 7549
		Type DeclaringType { get; }

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06001D7E RID: 7550
		Type ReflectedType { get; }

		// Token: 0x06001D7F RID: 7551
		object[] GetCustomAttributes(Type attributeType, bool inherit);

		// Token: 0x06001D80 RID: 7552
		object[] GetCustomAttributes(bool inherit);

		// Token: 0x06001D81 RID: 7553
		bool IsDefined(Type attributeType, bool inherit);

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06001D82 RID: 7554
		Type FieldType { get; }

		// Token: 0x06001D83 RID: 7555
		object GetValue(object obj);

		// Token: 0x06001D84 RID: 7556
		object GetValueDirect(TypedReference obj);

		// Token: 0x06001D85 RID: 7557
		void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture);

		// Token: 0x06001D86 RID: 7558
		void SetValueDirect(TypedReference obj, object value);

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06001D87 RID: 7559
		RuntimeFieldHandle FieldHandle { get; }

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06001D88 RID: 7560
		FieldAttributes Attributes { get; }

		// Token: 0x06001D89 RID: 7561
		void SetValue(object obj, object value);

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06001D8A RID: 7562
		bool IsPublic { get; }

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06001D8B RID: 7563
		bool IsPrivate { get; }

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06001D8C RID: 7564
		bool IsFamily { get; }

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06001D8D RID: 7565
		bool IsAssembly { get; }

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06001D8E RID: 7566
		bool IsFamilyAndAssembly { get; }

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06001D8F RID: 7567
		bool IsFamilyOrAssembly { get; }

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06001D90 RID: 7568
		bool IsStatic { get; }

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06001D91 RID: 7569
		bool IsInitOnly { get; }

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06001D92 RID: 7570
		bool IsLiteral { get; }

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06001D93 RID: 7571
		bool IsNotSerialized { get; }

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06001D94 RID: 7572
		bool IsSpecialName { get; }

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06001D95 RID: 7573
		bool IsPinvokeImpl { get; }
	}
}
