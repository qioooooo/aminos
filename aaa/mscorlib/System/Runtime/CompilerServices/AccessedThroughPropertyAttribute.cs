using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005BC RID: 1468
	[AttributeUsage(AttributeTargets.Field)]
	[ComVisible(true)]
	public sealed class AccessedThroughPropertyAttribute : Attribute
	{
		// Token: 0x0600375E RID: 14174 RVA: 0x000BB633 File Offset: 0x000BA633
		public AccessedThroughPropertyAttribute(string propertyName)
		{
			this.propertyName = propertyName;
		}

		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x0600375F RID: 14175 RVA: 0x000BB642 File Offset: 0x000BA642
		public string PropertyName
		{
			get
			{
				return this.propertyName;
			}
		}

		// Token: 0x04001C8F RID: 7311
		private readonly string propertyName;
	}
}
