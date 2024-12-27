using System;
using System.Drawing;

namespace System.ComponentModel.Design.Data
{
	// Token: 0x0200013E RID: 318
	public abstract class DataSourceDescriptor
	{
		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000C57 RID: 3159
		public abstract string Name { get; }

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000C58 RID: 3160
		public abstract Bitmap Image { get; }

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000C59 RID: 3161
		public abstract string TypeName { get; }

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000C5A RID: 3162
		public abstract bool IsDesignable { get; }
	}
}
