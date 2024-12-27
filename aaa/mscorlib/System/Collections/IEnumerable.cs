using System;
using System.Runtime.InteropServices;

namespace System.Collections
{
	// Token: 0x02000008 RID: 8
	[ComVisible(true)]
	[Guid("496B0ABE-CDEE-11d3-88E8-00902754C43A")]
	public interface IEnumerable
	{
		// Token: 0x06000011 RID: 17
		[DispId(-4)]
		IEnumerator GetEnumerator();
	}
}
