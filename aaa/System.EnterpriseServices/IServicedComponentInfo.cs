using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000022 RID: 34
	[Guid("8165B19E-8D3A-4d0b-80C8-97DE310DB583")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IServicedComponentInfo
	{
		// Token: 0x06000069 RID: 105
		void GetComponentInfo(ref int infoMask, out string[] infoArray);
	}
}
