using System;

namespace System.Xml.Serialization
{
	// Token: 0x02000316 RID: 790
	internal class ImportStructWorkItem
	{
		// Token: 0x0600257B RID: 9595 RVA: 0x000B3420 File Offset: 0x000B2420
		internal ImportStructWorkItem(StructModel model, StructMapping mapping)
		{
			this.model = model;
			this.mapping = mapping;
		}

		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x0600257C RID: 9596 RVA: 0x000B3436 File Offset: 0x000B2436
		internal StructModel Model
		{
			get
			{
				return this.model;
			}
		}

		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x0600257D RID: 9597 RVA: 0x000B343E File Offset: 0x000B243E
		internal StructMapping Mapping
		{
			get
			{
				return this.mapping;
			}
		}

		// Token: 0x040015A2 RID: 5538
		private StructModel model;

		// Token: 0x040015A3 RID: 5539
		private StructMapping mapping;
	}
}
