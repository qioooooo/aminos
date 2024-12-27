using System;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020002E4 RID: 740
	[ComVisible(true)]
	[CLSCompliant(false)]
	[TypeLibImportClass(typeof(ConstructorInfo))]
	[Guid("E9A19478-9646-3679-9B10-8411AE1FD57D")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface _ConstructorInfo
	{
		// Token: 0x06001D4E RID: 7502
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06001D4F RID: 7503
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06001D50 RID: 7504
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06001D51 RID: 7505
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);

		// Token: 0x06001D52 RID: 7506
		string ToString();

		// Token: 0x06001D53 RID: 7507
		bool Equals(object other);

		// Token: 0x06001D54 RID: 7508
		int GetHashCode();

		// Token: 0x06001D55 RID: 7509
		Type GetType();

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06001D56 RID: 7510
		MemberTypes MemberType { get; }

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06001D57 RID: 7511
		string Name { get; }

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06001D58 RID: 7512
		Type DeclaringType { get; }

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06001D59 RID: 7513
		Type ReflectedType { get; }

		// Token: 0x06001D5A RID: 7514
		object[] GetCustomAttributes(Type attributeType, bool inherit);

		// Token: 0x06001D5B RID: 7515
		object[] GetCustomAttributes(bool inherit);

		// Token: 0x06001D5C RID: 7516
		bool IsDefined(Type attributeType, bool inherit);

		// Token: 0x06001D5D RID: 7517
		ParameterInfo[] GetParameters();

		// Token: 0x06001D5E RID: 7518
		MethodImplAttributes GetMethodImplementationFlags();

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06001D5F RID: 7519
		RuntimeMethodHandle MethodHandle { get; }

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06001D60 RID: 7520
		MethodAttributes Attributes { get; }

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06001D61 RID: 7521
		CallingConventions CallingConvention { get; }

		// Token: 0x06001D62 RID: 7522
		object Invoke_2(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture);

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06001D63 RID: 7523
		bool IsPublic { get; }

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06001D64 RID: 7524
		bool IsPrivate { get; }

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06001D65 RID: 7525
		bool IsFamily { get; }

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x06001D66 RID: 7526
		bool IsAssembly { get; }

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x06001D67 RID: 7527
		bool IsFamilyAndAssembly { get; }

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06001D68 RID: 7528
		bool IsFamilyOrAssembly { get; }

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x06001D69 RID: 7529
		bool IsStatic { get; }

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06001D6A RID: 7530
		bool IsFinal { get; }

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06001D6B RID: 7531
		bool IsVirtual { get; }

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06001D6C RID: 7532
		bool IsHideBySig { get; }

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06001D6D RID: 7533
		bool IsAbstract { get; }

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06001D6E RID: 7534
		bool IsSpecialName { get; }

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06001D6F RID: 7535
		bool IsConstructor { get; }

		// Token: 0x06001D70 RID: 7536
		object Invoke_3(object obj, object[] parameters);

		// Token: 0x06001D71 RID: 7537
		object Invoke_4(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture);

		// Token: 0x06001D72 RID: 7538
		object Invoke_5(object[] parameters);
	}
}
