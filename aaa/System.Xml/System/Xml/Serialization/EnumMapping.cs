using System;

namespace System.Xml.Serialization
{
	// Token: 0x020002C9 RID: 713
	internal class EnumMapping : PrimitiveMapping
	{
		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x060021C5 RID: 8645 RVA: 0x0009F33B File Offset: 0x0009E33B
		// (set) Token: 0x060021C6 RID: 8646 RVA: 0x0009F343 File Offset: 0x0009E343
		internal bool IsFlags
		{
			get
			{
				return this.isFlags;
			}
			set
			{
				this.isFlags = value;
			}
		}

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x060021C7 RID: 8647 RVA: 0x0009F34C File Offset: 0x0009E34C
		// (set) Token: 0x060021C8 RID: 8648 RVA: 0x0009F354 File Offset: 0x0009E354
		internal ConstantMapping[] Constants
		{
			get
			{
				return this.constants;
			}
			set
			{
				this.constants = value;
			}
		}

		// Token: 0x0400147B RID: 5243
		private ConstantMapping[] constants;

		// Token: 0x0400147C RID: 5244
		private bool isFlags;
	}
}
