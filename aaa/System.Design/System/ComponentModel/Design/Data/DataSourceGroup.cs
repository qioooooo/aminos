using System;
using System.Drawing;

namespace System.ComponentModel.Design.Data
{
	// Token: 0x02000140 RID: 320
	public abstract class DataSourceGroup
	{
		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000C65 RID: 3173
		public abstract string Name { get; }

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000C66 RID: 3174
		public abstract Bitmap Image { get; }

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000C67 RID: 3175
		public abstract DataSourceDescriptorCollection DataSources { get; }

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000C68 RID: 3176
		public abstract bool IsDefault { get; }
	}
}
