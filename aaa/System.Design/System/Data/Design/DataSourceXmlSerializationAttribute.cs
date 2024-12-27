using System;

namespace System.Data.Design
{
	// Token: 0x0200007B RID: 123
	internal abstract class DataSourceXmlSerializationAttribute : Attribute
	{
		// Token: 0x0600052E RID: 1326 RVA: 0x000092E9 File Offset: 0x000082E9
		internal DataSourceXmlSerializationAttribute()
		{
			this.specialWay = false;
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600052F RID: 1327 RVA: 0x000092F8 File Offset: 0x000082F8
		// (set) Token: 0x06000530 RID: 1328 RVA: 0x00009300 File Offset: 0x00008300
		public Type ItemType
		{
			get
			{
				return this.itemType;
			}
			set
			{
				this.itemType = value;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000531 RID: 1329 RVA: 0x00009309 File Offset: 0x00008309
		// (set) Token: 0x06000532 RID: 1330 RVA: 0x00009311 File Offset: 0x00008311
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000533 RID: 1331 RVA: 0x0000931A File Offset: 0x0000831A
		// (set) Token: 0x06000534 RID: 1332 RVA: 0x00009322 File Offset: 0x00008322
		public bool SpecialWay
		{
			get
			{
				return this.specialWay;
			}
			set
			{
				this.specialWay = value;
			}
		}

		// Token: 0x04000AC7 RID: 2759
		private bool specialWay;

		// Token: 0x04000AC8 RID: 2760
		private Type itemType;

		// Token: 0x04000AC9 RID: 2761
		private string name;
	}
}
