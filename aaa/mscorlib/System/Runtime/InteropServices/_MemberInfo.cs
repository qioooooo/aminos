using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020000EF RID: 239
	[Guid("f7102fa9-cabb-3a74-a6da-b4567ef1b079")]
	[TypeLibImportClass(typeof(MemberInfo))]
	[CLSCompliant(false)]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface _MemberInfo
	{
		// Token: 0x06000C9E RID: 3230
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06000C9F RID: 3231
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06000CA0 RID: 3232
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06000CA1 RID: 3233
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);

		// Token: 0x06000CA2 RID: 3234
		string ToString();

		// Token: 0x06000CA3 RID: 3235
		bool Equals(object other);

		// Token: 0x06000CA4 RID: 3236
		int GetHashCode();

		// Token: 0x06000CA5 RID: 3237
		Type GetType();

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000CA6 RID: 3238
		MemberTypes MemberType { get; }

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000CA7 RID: 3239
		string Name { get; }

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000CA8 RID: 3240
		Type DeclaringType { get; }

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000CA9 RID: 3241
		Type ReflectedType { get; }

		// Token: 0x06000CAA RID: 3242
		object[] GetCustomAttributes(Type attributeType, bool inherit);

		// Token: 0x06000CAB RID: 3243
		object[] GetCustomAttributes(bool inherit);

		// Token: 0x06000CAC RID: 3244
		bool IsDefined(Type attributeType, bool inherit);
	}
}
