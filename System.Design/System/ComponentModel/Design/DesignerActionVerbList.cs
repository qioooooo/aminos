using System;

namespace System.ComponentModel.Design
{
	internal class DesignerActionVerbList : DesignerActionList
	{
		public DesignerActionVerbList(DesignerVerb[] verbs)
			: base(null)
		{
			this._verbs = verbs;
		}

		public override bool AutoShow
		{
			get
			{
				return false;
			}
		}

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

		private DesignerVerb[] _verbs;
	}
}
