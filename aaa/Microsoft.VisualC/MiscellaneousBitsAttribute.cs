using System;

namespace Microsoft.VisualC
{
	// Token: 0x0200000A RID: 10
	[Obsolete("Microsoft.VisualC.dll is an obsolete assembly and exists only for backwards compatibility.")]
	[AttributeUsage(AttributeTargets.All)]
	public sealed class MiscellaneousBitsAttribute : Attribute
	{
		// Token: 0x0600000A RID: 10 RVA: 0x000010B4 File Offset: 0x000004B4
		public MiscellaneousBitsAttribute(int miscellaneousBits)
		{
		}

		// Token: 0x04000001 RID: 1
		public int m_dwAttrs = miscellaneousBits;
	}
}
