using System;

namespace System.Web.UI.Design
{
	// Token: 0x020003B6 RID: 950
	public sealed class ViewEvent
	{
		// Token: 0x0600231F RID: 8991 RVA: 0x000BE3BE File Offset: 0x000BD3BE
		private ViewEvent()
		{
		}

		// Token: 0x04001884 RID: 6276
		public static readonly ViewEvent Click = new ViewEvent();

		// Token: 0x04001885 RID: 6277
		public static readonly ViewEvent Paint = new ViewEvent();

		// Token: 0x04001886 RID: 6278
		public static readonly ViewEvent TemplateModeChanged = new ViewEvent();
	}
}
