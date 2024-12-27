using System;
using System.Windows.Forms;

namespace System.Drawing.Design
{
	// Token: 0x02000021 RID: 33
	public sealed class ToolboxItemCreator
	{
		// Token: 0x060000FE RID: 254 RVA: 0x00006FAC File Offset: 0x00005FAC
		internal ToolboxItemCreator(ToolboxItemCreatorCallback callback, string format)
		{
			this._callback = callback;
			this._format = format;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00006FC2 File Offset: 0x00005FC2
		public ToolboxItem Create(IDataObject data)
		{
			return this._callback(data, this._format);
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00006FD6 File Offset: 0x00005FD6
		public string Format
		{
			get
			{
				return this._format;
			}
		}

		// Token: 0x040000E1 RID: 225
		private ToolboxItemCreatorCallback _callback;

		// Token: 0x040000E2 RID: 226
		private string _format;
	}
}
