using System;
using System.ComponentModel.Design;
using System.Design;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020003ED RID: 1005
	internal class BaseDataListActionList : DataBoundControlActionList
	{
		// Token: 0x06002513 RID: 9491 RVA: 0x000C73DB File Offset: 0x000C63DB
		public BaseDataListActionList(ControlDesigner controlDesigner, IDataSourceDesigner dataSourceDesigner)
			: base(controlDesigner, dataSourceDesigner)
		{
			this._controlDesigner = controlDesigner;
			this._dataSourceDesigner = dataSourceDesigner;
		}

		// Token: 0x06002514 RID: 9492 RVA: 0x000C73F3 File Offset: 0x000C63F3
		public void InvokePropertyBuilder()
		{
			((BaseDataListDesigner)this._controlDesigner).InvokePropertyBuilder(0);
		}

		// Token: 0x06002515 RID: 9493 RVA: 0x000C7408 File Offset: 0x000C6408
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

		// Token: 0x04001976 RID: 6518
		private IDataSourceDesigner _dataSourceDesigner;

		// Token: 0x04001977 RID: 6519
		private ControlDesigner _controlDesigner;
	}
}
