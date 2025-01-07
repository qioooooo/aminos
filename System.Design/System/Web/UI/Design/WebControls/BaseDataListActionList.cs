using System;
using System.ComponentModel.Design;
using System.Design;

namespace System.Web.UI.Design.WebControls
{
	internal class BaseDataListActionList : DataBoundControlActionList
	{
		public BaseDataListActionList(ControlDesigner controlDesigner, IDataSourceDesigner dataSourceDesigner)
			: base(controlDesigner, dataSourceDesigner)
		{
			this._controlDesigner = controlDesigner;
			this._dataSourceDesigner = dataSourceDesigner;
		}

		public void InvokePropertyBuilder()
		{
			((BaseDataListDesigner)this._controlDesigner).InvokePropertyBuilder(0);
		}

		public override DesignerActionItemCollection GetSortedActionItems()
		{
			DesignerActionItemCollection designerActionItemCollection = base.GetSortedActionItems();
			if (designerActionItemCollection == null)
			{
				designerActionItemCollection = new DesignerActionItemCollection();
			}
			designerActionItemCollection.Add(new DesignerActionMethodItem(this, "InvokePropertyBuilder", SR.GetString("BDL_PropertyBuilderVerb"), SR.GetString("BDL_BehaviorGroup"), SR.GetString("BDL_PropertyBuilderDesc")));
			return designerActionItemCollection;
		}

		private IDataSourceDesigner _dataSourceDesigner;

		private ControlDesigner _controlDesigner;
	}
}
