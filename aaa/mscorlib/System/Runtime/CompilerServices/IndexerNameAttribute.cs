using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005D2 RID: 1490
	[AttributeUsage(AttributeTargets.Property, Inherited = true)]
	[ComVisible(true)]
	[Serializable]
	public sealed class IndexerNameAttribute : Attribute
	{
		// Token: 0x06003798 RID: 14232 RVA: 0x000BB895 File Offset: 0x000BA895
		public IndexerNameAttribute(string indexerName)
		{
		}
	}
}
