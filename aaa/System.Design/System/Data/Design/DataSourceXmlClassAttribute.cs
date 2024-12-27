using System;

namespace System.Data.Design
{
	// Token: 0x0200007D RID: 125
	[AttributeUsage(AttributeTargets.Class)]
	internal sealed class DataSourceXmlClassAttribute : Attribute
	{
		// Token: 0x06000537 RID: 1335 RVA: 0x00009343 File Offset: 0x00008343
		internal DataSourceXmlClassAttribute(string elementName)
		{
			this.name = elementName;
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x00009352 File Offset: 0x00008352
		// (set) Token: 0x06000539 RID: 1337 RVA: 0x0000935A File Offset: 0x0000835A
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

		// Token: 0x04000ACA RID: 2762
		private string name;
	}
}
