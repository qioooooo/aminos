using System;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x020000C0 RID: 192
	[ComVisible(true)]
	public interface IFormatProvider
	{
		// Token: 0x06000AFE RID: 2814
		object GetFormat(Type formatType);
	}
}
