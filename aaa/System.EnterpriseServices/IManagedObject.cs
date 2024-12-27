using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000023 RID: 35
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("C3FCC19E-A970-11d2-8B5A-00A0C9B7C9C4")]
	[ComImport]
	internal interface IManagedObject
	{
		// Token: 0x0600006A RID: 106
		void GetSerializedBuffer(ref string s);

		// Token: 0x0600006B RID: 107
		void GetObjectIdentity(ref string s, ref int AppDomainID, ref int ccw);
	}
}
