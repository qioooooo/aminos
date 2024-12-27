using System;

namespace System.Windows.Forms
{
	// Token: 0x02000434 RID: 1076
	public sealed class HtmlElementErrorEventArgs : EventArgs
	{
		// Token: 0x060040B5 RID: 16565 RVA: 0x000E8ED3 File Offset: 0x000E7ED3
		internal HtmlElementErrorEventArgs(string description, string urlString, int lineNumber)
		{
			this.description = description;
			this.urlString = urlString;
			this.lineNumber = lineNumber;
		}

		// Token: 0x17000C7A RID: 3194
		// (get) Token: 0x060040B6 RID: 16566 RVA: 0x000E8EF0 File Offset: 0x000E7EF0
		public string Description
		{
			get
			{
				return this.description;
			}
		}

		// Token: 0x17000C7B RID: 3195
		// (get) Token: 0x060040B7 RID: 16567 RVA: 0x000E8EF8 File Offset: 0x000E7EF8
		// (set) Token: 0x060040B8 RID: 16568 RVA: 0x000E8F00 File Offset: 0x000E7F00
		public bool Handled
		{
			get
			{
				return this.handled;
			}
			set
			{
				this.handled = value;
			}
		}

		// Token: 0x17000C7C RID: 3196
		// (get) Token: 0x060040B9 RID: 16569 RVA: 0x000E8F09 File Offset: 0x000E7F09
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x17000C7D RID: 3197
		// (get) Token: 0x060040BA RID: 16570 RVA: 0x000E8F11 File Offset: 0x000E7F11
		public Uri Url
		{
			get
			{
				if (this.url == null)
				{
					this.url = new Uri(this.urlString);
				}
				return this.url;
			}
		}

		// Token: 0x04001F5D RID: 8029
		private string description;

		// Token: 0x04001F5E RID: 8030
		private string urlString;

		// Token: 0x04001F5F RID: 8031
		private Uri url;

		// Token: 0x04001F60 RID: 8032
		private int lineNumber;

		// Token: 0x04001F61 RID: 8033
		private bool handled;
	}
}
