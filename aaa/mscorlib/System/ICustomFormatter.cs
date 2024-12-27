using System;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x020000BF RID: 191
	[ComVisible(true)]
	public interface ICustomFormatter
	{
		// Token: 0x06000AFD RID: 2813
		string Format(string format, object arg, IFormatProvider formatProvider);
	}
}
