using System;

namespace System.ComponentModel.Design
{
	// Token: 0x02000103 RID: 259
	public class DesignerActionListsChangedEventArgs : EventArgs
	{
		// Token: 0x06000AA5 RID: 2725 RVA: 0x00029416 File Offset: 0x00028416
		public DesignerActionListsChangedEventArgs(object relatedObject, DesignerActionListsChangedType changeType, DesignerActionListCollection actionLists)
		{
			this.relatedObject = relatedObject;
			this.changeType = changeType;
			this.actionLists = actionLists;
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000AA6 RID: 2726 RVA: 0x00029433 File Offset: 0x00028433
		public DesignerActionListsChangedType ChangeType
		{
			get
			{
				return this.changeType;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000AA7 RID: 2727 RVA: 0x0002943B File Offset: 0x0002843B
		public object RelatedObject
		{
			get
			{
				return this.relatedObject;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000AA8 RID: 2728 RVA: 0x00029443 File Offset: 0x00028443
		public DesignerActionListCollection ActionLists
		{
			get
			{
				return this.actionLists;
			}
		}

		// Token: 0x04000D97 RID: 3479
		private object relatedObject;

		// Token: 0x04000D98 RID: 3480
		private DesignerActionListCollection actionLists;

		// Token: 0x04000D99 RID: 3481
		private DesignerActionListsChangedType changeType;
	}
}
