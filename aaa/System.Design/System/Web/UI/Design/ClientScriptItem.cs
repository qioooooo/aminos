using System;

namespace System.Web.UI.Design
{
	// Token: 0x02000321 RID: 801
	public sealed class ClientScriptItem
	{
		// Token: 0x06001E27 RID: 7719 RVA: 0x000ABACB File Offset: 0x000AAACB
		public ClientScriptItem(string text, string source, string language, string type, string id)
		{
			this._text = text;
			this._source = source;
			this._language = language;
			this._type = type;
			this._id = id;
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06001E28 RID: 7720 RVA: 0x000ABAF8 File Offset: 0x000AAAF8
		public string Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06001E29 RID: 7721 RVA: 0x000ABB00 File Offset: 0x000AAB00
		public string Language
		{
			get
			{
				return this._language;
			}
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06001E2A RID: 7722 RVA: 0x000ABB08 File Offset: 0x000AAB08
		public string Source
		{
			get
			{
				return this._source;
			}
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06001E2B RID: 7723 RVA: 0x000ABB10 File Offset: 0x000AAB10
		public string Text
		{
			get
			{
				return this._text;
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06001E2C RID: 7724 RVA: 0x000ABB18 File Offset: 0x000AAB18
		public string Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x0400172C RID: 5932
		private string _text;

		// Token: 0x0400172D RID: 5933
		private string _source;

		// Token: 0x0400172E RID: 5934
		private string _language;

		// Token: 0x0400172F RID: 5935
		private string _type;

		// Token: 0x04001730 RID: 5936
		private string _id;
	}
}
