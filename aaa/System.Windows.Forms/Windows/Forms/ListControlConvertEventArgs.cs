using System;

namespace System.Windows.Forms
{
	// Token: 0x0200047B RID: 1147
	public class ListControlConvertEventArgs : ConvertEventArgs
	{
		// Token: 0x06004366 RID: 17254 RVA: 0x000F1820 File Offset: 0x000F0820
		public ListControlConvertEventArgs(object value, Type desiredType, object listItem)
			: base(value, desiredType)
		{
			this.listItem = listItem;
		}

		// Token: 0x17000D37 RID: 3383
		// (get) Token: 0x06004367 RID: 17255 RVA: 0x000F1831 File Offset: 0x000F0831
		public object ListItem
		{
			get
			{
				return this.listItem;
			}
		}

		// Token: 0x040020DB RID: 8411
		private object listItem;
	}
}
