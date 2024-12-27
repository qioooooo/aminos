using System;

namespace System.Web.UI.Design
{
	// Token: 0x02000332 RID: 818
	public class ContentDefinition
	{
		// Token: 0x06001EF9 RID: 7929 RVA: 0x000AEEFF File Offset: 0x000ADEFF
		public ContentDefinition(string id, string content, string designTimeHtml)
		{
			this._contentPlaceHolderID = id;
			this._defaultContent = content;
			this._defaultDesignTimeHTML = designTimeHtml;
		}

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06001EFA RID: 7930 RVA: 0x000AEF1C File Offset: 0x000ADF1C
		public string ContentPlaceHolderID
		{
			get
			{
				return this._contentPlaceHolderID;
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06001EFB RID: 7931 RVA: 0x000AEF24 File Offset: 0x000ADF24
		public string DefaultContent
		{
			get
			{
				return this._defaultContent;
			}
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06001EFC RID: 7932 RVA: 0x000AEF2C File Offset: 0x000ADF2C
		public string DefaultDesignTimeHtml
		{
			get
			{
				return this._defaultDesignTimeHTML;
			}
		}

		// Token: 0x0400175D RID: 5981
		private string _contentPlaceHolderID;

		// Token: 0x0400175E RID: 5982
		private string _defaultContent;

		// Token: 0x0400175F RID: 5983
		private string _defaultDesignTimeHTML;
	}
}
