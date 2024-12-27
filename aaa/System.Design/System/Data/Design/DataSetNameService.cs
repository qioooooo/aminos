using System;

namespace System.Data.Design
{
	// Token: 0x020000B9 RID: 185
	internal class DataSetNameService : SimpleNameService
	{
		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000843 RID: 2115 RVA: 0x0001524A File Offset: 0x0001424A
		internal new static DataSetNameService DefaultInstance
		{
			get
			{
				if (DataSetNameService.defaultInstance == null)
				{
					DataSetNameService.defaultInstance = new DataSetNameService();
				}
				return DataSetNameService.defaultInstance;
			}
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x00015262 File Offset: 0x00014262
		public override void ValidateName(string name)
		{
		}

		// Token: 0x04000C12 RID: 3090
		private static DataSetNameService defaultInstance;
	}
}
