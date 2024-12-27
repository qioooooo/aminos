using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020002E7 RID: 743
	[ComVisible(true)]
	[Guid("9DE59C64-D889-35A1-B897-587D74469E5B")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[CLSCompliant(false)]
	[TypeLibImportClass(typeof(EventInfo))]
	public interface _EventInfo
	{
		// Token: 0x06001DB5 RID: 7605
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06001DB6 RID: 7606
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06001DB7 RID: 7607
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06001DB8 RID: 7608
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);

		// Token: 0x06001DB9 RID: 7609
		string ToString();

		// Token: 0x06001DBA RID: 7610
		bool Equals(object other);

		// Token: 0x06001DBB RID: 7611
		int GetHashCode();

		// Token: 0x06001DBC RID: 7612
		Type GetType();

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06001DBD RID: 7613
		MemberTypes MemberType { get; }

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06001DBE RID: 7614
		string Name { get; }

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06001DBF RID: 7615
		Type DeclaringType { get; }

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06001DC0 RID: 7616
		Type ReflectedType { get; }

		// Token: 0x06001DC1 RID: 7617
		object[] GetCustomAttributes(Type attributeType, bool inherit);

		// Token: 0x06001DC2 RID: 7618
		object[] GetCustomAttributes(bool inherit);

		// Token: 0x06001DC3 RID: 7619
		bool IsDefined(Type attributeType, bool inherit);

		// Token: 0x06001DC4 RID: 7620
		MethodInfo GetAddMethod(bool nonPublic);

		// Token: 0x06001DC5 RID: 7621
		MethodInfo GetRemoveMethod(bool nonPublic);

		// Token: 0x06001DC6 RID: 7622
		MethodInfo GetRaiseMethod(bool nonPublic);

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06001DC7 RID: 7623
		EventAttributes Attributes { get; }

		// Token: 0x06001DC8 RID: 7624
		MethodInfo GetAddMethod();

		// Token: 0x06001DC9 RID: 7625
		MethodInfo GetRemoveMethod();

		// Token: 0x06001DCA RID: 7626
		MethodInfo GetRaiseMethod();

		// Token: 0x06001DCB RID: 7627
		void AddEventHandler(object target, Delegate handler);

		// Token: 0x06001DCC RID: 7628
		void RemoveEventHandler(object target, Delegate handler);

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06001DCD RID: 7629
		Type EventHandlerType { get; }

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06001DCE RID: 7630
		bool IsSpecialName { get; }

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06001DCF RID: 7631
		bool IsMulticast { get; }
	}
}
