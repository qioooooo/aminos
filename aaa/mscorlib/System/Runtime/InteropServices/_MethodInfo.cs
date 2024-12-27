using System;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020002E3 RID: 739
	[CLSCompliant(false)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[TypeLibImportClass(typeof(MethodInfo))]
	[ComVisible(true)]
	[Guid("FFCC1B5D-ECB8-38DD-9B01-3DC8ABC2AA5F")]
	public interface _MethodInfo
	{
		// Token: 0x06001D28 RID: 7464
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06001D29 RID: 7465
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06001D2A RID: 7466
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06001D2B RID: 7467
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);

		// Token: 0x06001D2C RID: 7468
		string ToString();

		// Token: 0x06001D2D RID: 7469
		bool Equals(object other);

		// Token: 0x06001D2E RID: 7470
		int GetHashCode();

		// Token: 0x06001D2F RID: 7471
		Type GetType();

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x06001D30 RID: 7472
		MemberTypes MemberType { get; }

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06001D31 RID: 7473
		string Name { get; }

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06001D32 RID: 7474
		Type DeclaringType { get; }

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06001D33 RID: 7475
		Type ReflectedType { get; }

		// Token: 0x06001D34 RID: 7476
		object[] GetCustomAttributes(Type attributeType, bool inherit);

		// Token: 0x06001D35 RID: 7477
		object[] GetCustomAttributes(bool inherit);

		// Token: 0x06001D36 RID: 7478
		bool IsDefined(Type attributeType, bool inherit);

		// Token: 0x06001D37 RID: 7479
		ParameterInfo[] GetParameters();

		// Token: 0x06001D38 RID: 7480
		MethodImplAttributes GetMethodImplementationFlags();

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x06001D39 RID: 7481
		RuntimeMethodHandle MethodHandle { get; }

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x06001D3A RID: 7482
		MethodAttributes Attributes { get; }

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06001D3B RID: 7483
		CallingConventions CallingConvention { get; }

		// Token: 0x06001D3C RID: 7484
		object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture);

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06001D3D RID: 7485
		bool IsPublic { get; }

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x06001D3E RID: 7486
		bool IsPrivate { get; }

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06001D3F RID: 7487
		bool IsFamily { get; }

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06001D40 RID: 7488
		bool IsAssembly { get; }

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06001D41 RID: 7489
		bool IsFamilyAndAssembly { get; }

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06001D42 RID: 7490
		bool IsFamilyOrAssembly { get; }

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06001D43 RID: 7491
		bool IsStatic { get; }

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06001D44 RID: 7492
		bool IsFinal { get; }

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06001D45 RID: 7493
		bool IsVirtual { get; }

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06001D46 RID: 7494
		bool IsHideBySig { get; }

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06001D47 RID: 7495
		bool IsAbstract { get; }

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06001D48 RID: 7496
		bool IsSpecialName { get; }

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06001D49 RID: 7497
		bool IsConstructor { get; }

		// Token: 0x06001D4A RID: 7498
		object Invoke(object obj, object[] parameters);

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06001D4B RID: 7499
		Type ReturnType { get; }

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06001D4C RID: 7500
		ICustomAttributeProvider ReturnTypeCustomAttributes { get; }

		// Token: 0x06001D4D RID: 7501
		MethodInfo GetBaseDefinition();
	}
}
