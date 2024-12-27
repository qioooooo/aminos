using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x0200046C RID: 1132
	[ComVisible(true)]
	public class LinkClickedEventArgs : EventArgs
	{
		// Token: 0x17000CFC RID: 3324
		// (get) Token: 0x06004283 RID: 17027 RVA: 0x000ED833 File Offset: 0x000EC833
		public string LinkText
		{
			get
			{
				return this.linkText;
			}
		}

		// Token: 0x06004284 RID: 17028 RVA: 0x000ED83B File Offset: 0x000EC83B
		public LinkClickedEventArgs(string linkText)
		{
			this.linkText = linkText;
		}

		// Token: 0x040020A7 RID: 8359
		private string linkText;
	}
}
