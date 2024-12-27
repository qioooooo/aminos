using System;

namespace System.Drawing.Printing
{
	// Token: 0x0200012A RID: 298
	public class QueryPageSettingsEventArgs : PrintEventArgs
	{
		// Token: 0x06000F5E RID: 3934 RVA: 0x0002DDFC File Offset: 0x0002CDFC
		public QueryPageSettingsEventArgs(PageSettings pageSettings)
		{
			this.pageSettings = pageSettings;
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06000F5F RID: 3935 RVA: 0x0002DE0B File Offset: 0x0002CE0B
		// (set) Token: 0x06000F60 RID: 3936 RVA: 0x0002DE13 File Offset: 0x0002CE13
		public PageSettings PageSettings
		{
			get
			{
				return this.pageSettings;
			}
			set
			{
				if (value == null)
				{
					value = new PageSettings();
				}
				this.pageSettings = value;
			}
		}

		// Token: 0x04000C7B RID: 3195
		private PageSettings pageSettings;
	}
}
