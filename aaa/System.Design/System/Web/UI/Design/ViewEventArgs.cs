using System;

namespace System.Web.UI.Design
{
	// Token: 0x020003B7 RID: 951
	public class ViewEventArgs : EventArgs
	{
		// Token: 0x06002321 RID: 8993 RVA: 0x000BE3E6 File Offset: 0x000BD3E6
		public ViewEventArgs(ViewEvent eventType, DesignerRegion region, EventArgs eventArgs)
		{
			this._eventType = eventType;
			this._region = region;
			this._eventArgs = eventArgs;
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06002322 RID: 8994 RVA: 0x000BE403 File Offset: 0x000BD403
		public EventArgs EventArgs
		{
			get
			{
				return this._eventArgs;
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06002323 RID: 8995 RVA: 0x000BE40B File Offset: 0x000BD40B
		public ViewEvent EventType
		{
			get
			{
				return this._eventType;
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06002324 RID: 8996 RVA: 0x000BE413 File Offset: 0x000BD413
		public DesignerRegion Region
		{
			get
			{
				return this._region;
			}
		}

		// Token: 0x04001887 RID: 6279
		private DesignerRegion _region;

		// Token: 0x04001888 RID: 6280
		private EventArgs _eventArgs;

		// Token: 0x04001889 RID: 6281
		private ViewEvent _eventType;
	}
}
