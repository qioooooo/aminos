using System;

namespace System.ComponentModel.Design
{
	// Token: 0x02000127 RID: 295
	public class DesignerActionUIStateChangeEventArgs : EventArgs
	{
		// Token: 0x06000BB6 RID: 2998 RVA: 0x0002DFF5 File Offset: 0x0002CFF5
		public DesignerActionUIStateChangeEventArgs(object relatedObject, DesignerActionUIStateChangeType changeType)
		{
			this.relatedObject = relatedObject;
			this.changeType = changeType;
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000BB7 RID: 2999 RVA: 0x0002E00B File Offset: 0x0002D00B
		public DesignerActionUIStateChangeType ChangeType
		{
			get
			{
				return this.changeType;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000BB8 RID: 3000 RVA: 0x0002E013 File Offset: 0x0002D013
		public object RelatedObject
		{
			get
			{
				return this.relatedObject;
			}
		}

		// Token: 0x04000E54 RID: 3668
		private object relatedObject;

		// Token: 0x04000E55 RID: 3669
		private DesignerActionUIStateChangeType changeType;
	}
}
