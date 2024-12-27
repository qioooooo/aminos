using System;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020002E2 RID: 738
	[Guid("6240837A-707F-3181-8E98-A36AE086766B")]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[CLSCompliant(false)]
	[TypeLibImportClass(typeof(MethodBase))]
	public interface _MethodBase
	{
		// Token: 0x06001D05 RID: 7429
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06001D06 RID: 7430
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06001D07 RID: 7431
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06001D08 RID: 7432
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);

		// Token: 0x06001D09 RID: 7433
		string ToString();

		// Token: 0x06001D0A RID: 7434
		bool Equals(object other);

		// Token: 0x06001D0B RID: 7435
		int GetHashCode();

		// Token: 0x06001D0C RID: 7436
		Type GetType();

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x06001D0D RID: 7437
		MemberTypes MemberType { get; }

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06001D0E RID: 7438
		string Name { get; }

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06001D0F RID: 7439
		Type DeclaringType { get; }

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06001D10 RID: 7440
		Type ReflectedType { get; }

		// Token: 0x06001D11 RID: 7441
		object[] GetCustomAttributes(Type attributeType, bool inherit);

		// Token: 0x06001D12 RID: 7442
		object[] GetCustomAttributes(bool inherit);

		// Token: 0x06001D13 RID: 7443
		bool IsDefined(Type attributeType, bool inherit);

		// Token: 0x06001D14 RID: 7444
		ParameterInfo[] GetParameters();

		// Token: 0x06001D15 RID: 7445
		MethodImplAttributes GetMethodImplementationFlags();

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x06001D16 RID: 7446
		RuntimeMethodHandle MethodHandle { get; }

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x06001D17 RID: 7447
		MethodAttributes Attributes { get; }

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x06001D18 RID: 7448
		CallingConventions CallingConvention { get; }

		// Token: 0x06001D19 RID: 7449
		object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture);

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06001D1A RID: 7450
		bool IsPublic { get; }

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06001D1B RID: 7451
		bool IsPrivate { get; }

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06001D1C RID: 7452
		bool IsFamily { get; }

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x06001D1D RID: 7453
		bool IsAssembly { get; }

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06001D1E RID: 7454
		bool IsFamilyAndAssembly { get; }

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x06001D1F RID: 7455
		bool IsFamilyOrAssembly { get; }

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x06001D20 RID: 7456
		bool IsStatic { get; }

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x06001D21 RID: 7457
		bool IsFinal { get; }

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x06001D22 RID: 7458
		bool IsVirtual { get; }

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x06001D23 RID: 7459
		bool IsHideBySig { get; }

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x06001D24 RID: 7460
		bool IsAbstract { get; }

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x06001D25 RID: 7461
		bool IsSpecialName { get; }

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x06001D26 RID: 7462
		bool IsConstructor { get; }

		// Token: 0x06001D27 RID: 7463
		object Invoke(object obj, object[] parameters);
	}
}
