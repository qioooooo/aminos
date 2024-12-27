using System;
using System.ComponentModel.Design;
using System.Design;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x0200044F RID: 1103
	internal class GridViewActionList : DesignerActionList
	{
		// Token: 0x06002825 RID: 10277 RVA: 0x000DCA88 File Offset: 0x000DBA88
		public GridViewActionList(GridViewDesigner gridViewDesigner)
			: base(gridViewDesigner.Component)
		{
			this._gridViewDesigner = gridViewDesigner;
		}

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x06002826 RID: 10278 RVA: 0x000DCA9D File Offset: 0x000DBA9D
		// (set) Token: 0x06002827 RID: 10279 RVA: 0x000DCAA5 File Offset: 0x000DBAA5
		internal bool AllowDeleting
		{
			get
			{
				return this._allowDeleting;
			}
			set
			{
				this._allowDeleting = value;
			}
		}

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x06002828 RID: 10280 RVA: 0x000DCAAE File Offset: 0x000DBAAE
		// (set) Token: 0x06002829 RID: 10281 RVA: 0x000DCAB6 File Offset: 0x000DBAB6
		internal bool AllowEditing
		{
			get
			{
				return this._allowEditing;
			}
			set
			{
				this._allowEditing = value;
			}
		}

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x0600282A RID: 10282 RVA: 0x000DCABF File Offset: 0x000DBABF
		// (set) Token: 0x0600282B RID: 10283 RVA: 0x000DCAC7 File Offset: 0x000DBAC7
		internal bool AllowMoveLeft
		{
			get
			{
				return this._allowMoveLeft;
			}
			set
			{
				this._allowMoveLeft = value;
			}
		}

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x0600282C RID: 10284 RVA: 0x000DCAD0 File Offset: 0x000DBAD0
		// (set) Token: 0x0600282D RID: 10285 RVA: 0x000DCAD8 File Offset: 0x000DBAD8
		internal bool AllowMoveRight
		{
			get
			{
				return this._allowMoveRight;
			}
			set
			{
				this._allowMoveRight = value;
			}
		}

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x0600282E RID: 10286 RVA: 0x000DCAE1 File Offset: 0x000DBAE1
		// (set) Token: 0x0600282F RID: 10287 RVA: 0x000DCAE9 File Offset: 0x000DBAE9
		internal bool AllowPaging
		{
			get
			{
				return this._allowPaging;
			}
			set
			{
				this._allowPaging = value;
			}
		}

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x06002830 RID: 10288 RVA: 0x000DCAF2 File Offset: 0x000DBAF2
		// (set) Token: 0x06002831 RID: 10289 RVA: 0x000DCAFA File Offset: 0x000DBAFA
		internal bool AllowRemoveField
		{
			get
			{
				return this._allowRemoveField;
			}
			set
			{
				this._allowRemoveField = value;
			}
		}

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x06002832 RID: 10290 RVA: 0x000DCB03 File Offset: 0x000DBB03
		// (set) Token: 0x06002833 RID: 10291 RVA: 0x000DCB0B File Offset: 0x000DBB0B
		internal bool AllowSelection
		{
			get
			{
				return this._allowSelection;
			}
			set
			{
				this._allowSelection = value;
			}
		}

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x06002834 RID: 10292 RVA: 0x000DCB14 File Offset: 0x000DBB14
		// (set) Token: 0x06002835 RID: 10293 RVA: 0x000DCB1C File Offset: 0x000DBB1C
		internal bool AllowSorting
		{
			get
			{
				return this._allowSorting;
			}
			set
			{
				this._allowSorting = value;
			}
		}

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x06002836 RID: 10294 RVA: 0x000DCB25 File Offset: 0x000DBB25
		// (set) Token: 0x06002837 RID: 10295 RVA: 0x000DCB28 File Offset: 0x000DBB28
		public override bool AutoShow
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06002838 RID: 10296 RVA: 0x000DCB2A File Offset: 0x000DBB2A
		// (set) Token: 0x06002839 RID: 10297 RVA: 0x000DCB37 File Offset: 0x000DBB37
		public bool EnableDeleting
		{
			get
			{
				return this._gridViewDesigner.EnableDeleting;
			}
			set
			{
				this._gridViewDesigner.EnableDeleting = value;
			}
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x0600283A RID: 10298 RVA: 0x000DCB45 File Offset: 0x000DBB45
		// (set) Token: 0x0600283B RID: 10299 RVA: 0x000DCB52 File Offset: 0x000DBB52
		public bool EnableEditing
		{
			get
			{
				return this._gridViewDesigner.EnableEditing;
			}
			set
			{
				this._gridViewDesigner.EnableEditing = value;
			}
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x0600283C RID: 10300 RVA: 0x000DCB60 File Offset: 0x000DBB60
		// (set) Token: 0x0600283D RID: 10301 RVA: 0x000DCB6D File Offset: 0x000DBB6D
		public bool EnablePaging
		{
			get
			{
				return this._gridViewDesigner.EnablePaging;
			}
			set
			{
				this._gridViewDesigner.EnablePaging = value;
			}
		}

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x0600283E RID: 10302 RVA: 0x000DCB7B File Offset: 0x000DBB7B
		// (set) Token: 0x0600283F RID: 10303 RVA: 0x000DCB88 File Offset: 0x000DBB88
		public bool EnableSelection
		{
			get
			{
				return this._gridViewDesigner.EnableSelection;
			}
			set
			{
				this._gridViewDesigner.EnableSelection = value;
			}
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06002840 RID: 10304 RVA: 0x000DCB96 File Offset: 0x000DBB96
		// (set) Token: 0x06002841 RID: 10305 RVA: 0x000DCBA3 File Offset: 0x000DBBA3
		public bool EnableSorting
		{
			get
			{
				return this._gridViewDesigner.EnableSorting;
			}
			set
			{
				this._gridViewDesigner.EnableSorting = value;
			}
		}

		// Token: 0x06002842 RID: 10306 RVA: 0x000DCBB1 File Offset: 0x000DBBB1
		public void AddNewField()
		{
			this._gridViewDesigner.AddNewField();
		}

		// Token: 0x06002843 RID: 10307 RVA: 0x000DCBBE File Offset: 0x000DBBBE
		public void EditFields()
		{
			this._gridViewDesigner.EditFields();
		}

		// Token: 0x06002844 RID: 10308 RVA: 0x000DCBCB File Offset: 0x000DBBCB
		public void MoveFieldLeft()
		{
			this._gridViewDesigner.MoveLeft();
		}

		// Token: 0x06002845 RID: 10309 RVA: 0x000DCBD8 File Offset: 0x000DBBD8
		public void MoveFieldRight()
		{
			this._gridViewDesigner.MoveRight();
		}

		// Token: 0x06002846 RID: 10310 RVA: 0x000DCBE5 File Offset: 0x000DBBE5
		public void RemoveField()
		{
			this._gridViewDesigner.RemoveField();
		}

		// Token: 0x06002847 RID: 10311 RVA: 0x000DCBF4 File Offset: 0x000DBBF4
		public override DesignerActionItemCollection GetSortedActionItems()
		{
			DesignerActionItemCollection designerActionItemCollection = new DesignerActionItemCollection();
			designerActionItemCollection.Add(new DesignerActionMethodItem(this, "EditFields", SR.GetString("GridView_EditFieldsVerb"), "Action", SR.GetString("GridView_EditFieldsDesc")));
			designerActionItemCollection.Add(new DesignerActionMethodItem(this, "AddNewField", SR.GetString("GridView_AddNewFieldVerb"), "Action", SR.GetString("GridView_AddNewFieldDesc")));
			if (this.AllowMoveLeft)
			{
				designerActionItemCollection.Add(new DesignerActionMethodItem(this, "MoveFieldLeft", SR.GetString("GridView_MoveFieldLeftVerb"), "Action", SR.GetString("GridView_MoveFieldLeftDesc")));
			}
			if (this.AllowMoveRight)
			{
				designerActionItemCollection.Add(new DesignerActionMethodItem(this, "MoveFieldRight", SR.GetString("GridView_MoveFieldRightVerb"), "Action", SR.GetString("GridView_MoveFieldRightDesc")));
			}
			if (this.AllowRemoveField)
			{
				designerActionItemCollection.Add(new DesignerActionMethodItem(this, "RemoveField", SR.GetString("GridView_RemoveFieldVerb"), "Action", SR.GetString("GridView_RemoveFieldDesc")));
			}
			if (this.AllowPaging)
			{
				designerActionItemCollection.Add(new DesignerActionPropertyItem("EnablePaging", SR.GetString("GridView_EnablePaging"), "Behavior", SR.GetString("GridView_EnablePagingDesc")));
			}
			if (this.AllowSorting)
			{
				designerActionItemCollection.Add(new DesignerActionPropertyItem("EnableSorting", SR.GetString("GridView_EnableSorting"), "Behavior", SR.GetString("GridView_EnableSortingDesc")));
			}
			if (this.AllowEditing)
			{
				designerActionItemCollection.Add(new DesignerActionPropertyItem("EnableEditing", SR.GetString("GridView_EnableEditing"), "Behavior", SR.GetString("GridView_EnableEditingDesc")));
			}
			if (this.AllowDeleting)
			{
				designerActionItemCollection.Add(new DesignerActionPropertyItem("EnableDeleting", SR.GetString("GridView_EnableDeleting"), "Behavior", SR.GetString("GridView_EnableDeletingDesc")));
			}
			if (this.AllowSelection)
			{
				designerActionItemCollection.Add(new DesignerActionPropertyItem("EnableSelection", SR.GetString("GridView_EnableSelection"), "Behavior", SR.GetString("GridView_EnableSelectionDesc")));
			}
			return designerActionItemCollection;
		}

		// Token: 0x04001BDE RID: 7134
		private GridViewDesigner _gridViewDesigner;

		// Token: 0x04001BDF RID: 7135
		private bool _allowDeleting;

		// Token: 0x04001BE0 RID: 7136
		private bool _allowEditing;

		// Token: 0x04001BE1 RID: 7137
		private bool _allowSorting;

		// Token: 0x04001BE2 RID: 7138
		private bool _allowPaging;

		// Token: 0x04001BE3 RID: 7139
		private bool _allowSelection;

		// Token: 0x04001BE4 RID: 7140
		private bool _allowRemoveField;

		// Token: 0x04001BE5 RID: 7141
		private bool _allowMoveLeft;

		// Token: 0x04001BE6 RID: 7142
		private bool _allowMoveRight;
	}
}
