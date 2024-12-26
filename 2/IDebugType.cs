using System;
using System.Runtime.InteropServices;

namespace Microsoft.JScript
{
	// Token: 0x02000065 RID: 101
	[ComVisible(true)]
	[Guid("613CC05D-05F4-4969-B369-5AEEF56E32D0")]
	public interface IDebugType
	{
		// Token: 0x06000504 RID: 1284
		bool HasInstance([MarshalAs(UnmanagedType.Interface)] object o);
	}
}
