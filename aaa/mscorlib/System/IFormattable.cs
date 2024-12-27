using System;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x0200001D RID: 29
	[ComVisible(true)]
	public interface IFormattable
	{
		// Token: 0x060000EB RID: 235
		string ToString(string format, IFormatProvider formatProvider);
	}
}
