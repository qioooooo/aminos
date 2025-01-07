using System;
using System.ComponentModel.Design;
using System.Design;

namespace System.Web.UI.Design.WebControls
{
	internal class GridViewActionList : DesignerActionList
	{
		public GridViewActionList(GridViewDesigner gridViewDesigner)
			: base(gridViewDesigner.Component)
		{
			this._gridViewDesigner = gridViewDesigner;
		}

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

		public void AddNewField()
		{
			this._gridViewDesigner.AddNewField();
		}

		public void EditFields()
		{
			this._gridViewDesigner.EditFields();
		}

		public void MoveFieldLeft()
		{
			this._gridViewDesigner.MoveLeft();
		}

		public void MoveFieldRight()
		{
			this._gridViewDesigner.MoveRight();
		}

		public void RemoveField()
		{
			this._gridViewDesigner.RemoveField();
		}

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

		private GridViewDesigner _gridViewDesigner;

		private bool _allowDeleting;

		private bool _allowEditing;

		private bool _allowSorting;

		private bool _allowPaging;

		private bool _allowSelection;

		private bool _allowRemoveField;

		private bool _allowMoveLeft;

		private bool _allowMoveRight;
	}
}
