using System;

namespace Microsoft.VisualC
{
	// Token: 0x02000009 RID: 9
	[AttributeUsage(AttributeTargets.All)]
	[Obsolete("Microsoft.VisualC.dll is an obsolete assembly and exists only for backwards compatibility.")]
	public sealed class DecoratedNameAttribute : Attribute
	{
		// Token: 0x06000008 RID: 8 RVA: 0x000010A0 File Offset: 0x000004A0
		public DecoratedNameAttribute(string decoratedName)
		{
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000108C File Offset: 0x0000048C
		public DecoratedNameAttribute()
		{
		}
	}
}
