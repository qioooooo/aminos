using System;

namespace System.ComponentModel.Design
{
	// Token: 0x0200012B RID: 299
	internal class DesignerActionVerbList : DesignerActionList
	{
		// Token: 0x06000BC4 RID: 3012 RVA: 0x0002E067 File Offset: 0x0002D067
		public DesignerActionVerbList(DesignerVerb[] verbs)
			: base(null)
		{
			this._verbs = verbs;
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000BC5 RID: 3013 RVA: 0x0002E077 File Offset: 0x0002D077
		public override bool AutoShow
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x0002E07C File Offset: 0x0002D07C
		public override DesignerActionItemCollection GetSortedActionItems()
		{
			DesignerActionItemCollection designerActionItemCollection = new DesignerActionItemCollection();
			for (int i = 0; i < this._verbs.Length; i++)
			{
				if (this._verbs[i].Visible && this._verbs[i].Enabled && this._verbs[i].Supported)
				{
					designerActionItemCollection.Add(new DesignerActionVerbItem(this._verbs[i]));
				}
			}
			return designerActionItemCollection;
		}

		// Token: 0x04000E5B RID: 3675
		private DesignerVerb[] _verbs;
	}
}
