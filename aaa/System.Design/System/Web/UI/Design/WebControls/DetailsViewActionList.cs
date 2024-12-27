using System;
using System.ComponentModel.Design;
using System.Design;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000447 RID: 1095
	internal class DetailsViewActionList : DesignerActionList
	{
		// Token: 0x060027A2 RID: 10146 RVA: 0x000D8EBC File Offset: 0x000D7EBC
		public DetailsViewActionList(DetailsViewDesigner detailsViewDesigner)
			: base(detailsViewDesigner.Component)
		{
			this._detailsViewDesigner = detailsViewDesigner;
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x060027A3 RID: 10147 RVA: 0x000D8ED1 File Offset: 0x000D7ED1
		// (set) Token: 0x060027A4 RID: 10148 RVA: 0x000D8ED9 File Offset: 0x000D7ED9
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

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x060027A5 RID: 10149 RVA: 0x000D8EE2 File Offset: 0x000D7EE2
		// (set) Token: 0x060027A6 RID: 10150 RVA: 0x000D8EEA File Offset: 0x000D7EEA
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

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x060027A7 RID: 10151 RVA: 0x000D8EF3 File Offset: 0x000D7EF3
		// (set) Token: 0x060027A8 RID: 10152 RVA: 0x000D8EFB File Offset: 0x000D7EFB
		internal bool AllowInserting
		{
			get
			{
				return this._allowInserting;
			}
			set
			{
				this._allowInserting = value;
			}
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x060027A9 RID: 10153 RVA: 0x000D8F04 File Offset: 0x000D7F04
		// (set) Token: 0x060027AA RID: 10154 RVA: 0x000D8F0C File Offset: 0x000D7F0C
		internal bool AllowMoveDown
		{
			get
			{
				return this._allowMoveDown;
			}
			set
			{
				this._allowMoveDown = value;
			}
		}

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x060027AB RID: 10155 RVA: 0x000D8F15 File Offset: 0x000D7F15
		// (set) Token: 0x060027AC RID: 10156 RVA: 0x000D8F1D File Offset: 0x000D7F1D
		internal bool AllowMoveUp
		{
			get
			{
				return this._allowMoveUp;
			}
			set
			{
				this._allowMoveUp = value;
			}
		}

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x060027AD RID: 10157 RVA: 0x000D8F26 File Offset: 0x000D7F26
		// (set) Token: 0x060027AE RID: 10158 RVA: 0x000D8F2E File Offset: 0x000D7F2E
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

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x060027AF RID: 10159 RVA: 0x000D8F37 File Offset: 0x000D7F37
		// (set) Token: 0x060027B0 RID: 10160 RVA: 0x000D8F3F File Offset: 0x000D7F3F
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

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x060027B1 RID: 10161 RVA: 0x000D8F48 File Offset: 0x000D7F48
		// (set) Token: 0x060027B2 RID: 10162 RVA: 0x000D8F4B File Offset: 0x000D7F4B
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

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x060027B3 RID: 10163 RVA: 0x000D8F4D File Offset: 0x000D7F4D
		// (set) Token: 0x060027B4 RID: 10164 RVA: 0x000D8F5A File Offset: 0x000D7F5A
		public bool EnableDeleting
		{
			get
			{
				return this._detailsViewDesigner.EnableDeleting;
			}
			set
			{
				this._detailsViewDesigner.EnableDeleting = value;
			}
		}

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x060027B5 RID: 10165 RVA: 0x000D8F68 File Offset: 0x000D7F68
		// (set) Token: 0x060027B6 RID: 10166 RVA: 0x000D8F75 File Offset: 0x000D7F75
		public bool EnableEditing
		{
			get
			{
				return this._detailsViewDesigner.EnableEditing;
			}
			set
			{
				this._detailsViewDesigner.EnableEditing = value;
			}
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x060027B7 RID: 10167 RVA: 0x000D8F83 File Offset: 0x000D7F83
		// (set) Token: 0x060027B8 RID: 10168 RVA: 0x000D8F90 File Offset: 0x000D7F90
		public bool EnableInserting
		{
			get
			{
				return this._detailsViewDesigner.EnableInserting;
			}
			set
			{
				this._detailsViewDesigner.EnableInserting = value;
			}
		}

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x060027B9 RID: 10169 RVA: 0x000D8F9E File Offset: 0x000D7F9E
		// (set) Token: 0x060027BA RID: 10170 RVA: 0x000D8FAB File Offset: 0x000D7FAB
		public bool EnablePaging
		{
			get
			{
				return this._detailsViewDesigner.EnablePaging;
			}
			set
			{
				this._detailsViewDesigner.EnablePaging = value;
			}
		}

		// Token: 0x060027BB RID: 10171 RVA: 0x000D8FB9 File Offset: 0x000D7FB9
		public void AddNewField()
		{
			this._detailsViewDesigner.AddNewField();
		}

		// Token: 0x060027BC RID: 10172 RVA: 0x000D8FC6 File Offset: 0x000D7FC6
		public void EditFields()
		{
			this._detailsViewDesigner.EditFields();
		}

		// Token: 0x060027BD RID: 10173 RVA: 0x000D8FD3 File Offset: 0x000D7FD3
		public void MoveFieldUp()
		{
			this._detailsViewDesigner.MoveUp();
		}

		// Token: 0x060027BE RID: 10174 RVA: 0x000D8FE0 File Offset: 0x000D7FE0
		public void MoveFieldDown()
		{
			this._detailsViewDesigner.MoveDown();
		}

		// Token: 0x060027BF RID: 10175 RVA: 0x000D8FED File Offset: 0x000D7FED
		public void RemoveField()
		{
			this._detailsViewDesigner.RemoveField();
		}

		// Token: 0x060027C0 RID: 10176 RVA: 0x000D8FFC File Offset: 0x000D7FFC
		public override DesignerActionItemCollection GetSortedActionItems()
		{
			DesignerActionItemCollection designerActionItemCollection = new DesignerActionItemCollection();
			designerActionItemCollection.Add(new DesignerActionMethodItem(this, "EditFields", SR.GetString("DetailsView_EditFieldsVerb"), "Action", SR.GetString("DetailsView_EditFieldsDesc")));
			designerActionItemCollection.Add(new DesignerActionMethodItem(this, "AddNewField", SR.GetString("DetailsView_AddNewFieldVerb"), "Action", SR.GetString("DetailsView_AddNewFieldDesc")));
			if (this.AllowMoveUp)
			{
				designerActionItemCollection.Add(new DesignerActionMethodItem(this, "MoveFieldUp", SR.GetString("DetailsView_MoveFieldUpVerb"), "Action", SR.GetString("DetailsView_MoveFieldUpDesc")));
			}
			if (this.AllowMoveDown)
			{
				designerActionItemCollection.Add(new DesignerActionMethodItem(this, "MoveFieldDown", SR.GetString("DetailsView_MoveFieldDownVerb"), "Action", SR.GetString("DetailsView_MoveFieldDownDesc")));
			}
			if (this.AllowRemoveField)
			{
				designerActionItemCollection.Add(new DesignerActionMethodItem(this, "RemoveField", SR.GetString("DetailsView_RemoveFieldVerb"), "Action", SR.GetString("DetailsView_RemoveFieldDesc")));
			}
			if (this.AllowPaging)
			{
				designerActionItemCollection.Add(new DesignerActionPropertyItem("EnablePaging", SR.GetString("DetailsView_EnablePaging"), "Behavior", SR.GetString("DetailsView_EnablePagingDesc")));
			}
			if (this.AllowInserting)
			{
				designerActionItemCollection.Add(new DesignerActionPropertyItem("EnableInserting", SR.GetString("DetailsView_EnableInserting"), "Behavior", SR.GetString("DetailsView_EnableInsertingDesc")));
			}
			if (this.AllowEditing)
			{
				designerActionItemCollection.Add(new DesignerActionPropertyItem("EnableEditing", SR.GetString("DetailsView_EnableEditing"), "Behavior", SR.GetString("DetailsView_EnableEditingDesc")));
			}
			if (this.AllowDeleting)
			{
				designerActionItemCollection.Add(new DesignerActionPropertyItem("EnableDeleting", SR.GetString("DetailsView_EnableDeleting"), "Behavior", SR.GetString("DetailsView_EnableDeletingDesc")));
			}
			return designerActionItemCollection;
		}

		// Token: 0x04001B61 RID: 7009
		private DetailsViewDesigner _detailsViewDesigner;

		// Token: 0x04001B62 RID: 7010
		private bool _allowDeleting;

		// Token: 0x04001B63 RID: 7011
		private bool _allowEditing;

		// Token: 0x04001B64 RID: 7012
		private bool _allowInserting;

		// Token: 0x04001B65 RID: 7013
		private bool _allowPaging;

		// Token: 0x04001B66 RID: 7014
		private bool _allowRemoveField;

		// Token: 0x04001B67 RID: 7015
		private bool _allowMoveUp;

		// Token: 0x04001B68 RID: 7016
		private bool _allowMoveDown;
	}
}
