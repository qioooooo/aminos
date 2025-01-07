using System;
using System.ComponentModel.Design;
using System.Design;

namespace System.Web.UI.Design.WebControls
{
	internal class DetailsViewActionList : DesignerActionList
	{
		public DetailsViewActionList(DetailsViewDesigner detailsViewDesigner)
			: base(detailsViewDesigner.Component)
		{
			this._detailsViewDesigner = detailsViewDesigner;
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
				return this._detailsViewDesigner.EnableDeleting;
			}
			set
			{
				this._detailsViewDesigner.EnableDeleting = value;
			}
		}

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

		public void AddNewField()
		{
			this._detailsViewDesigner.AddNewField();
		}

		public void EditFields()
		{
			this._detailsViewDesigner.EditFields();
		}

		public void MoveFieldUp()
		{
			this._detailsViewDesigner.MoveUp();
		}

		public void MoveFieldDown()
		{
			this._detailsViewDesigner.MoveDown();
		}

		public void RemoveField()
		{
			this._detailsViewDesigner.RemoveField();
		}

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

		private DetailsViewDesigner _detailsViewDesigner;

		private bool _allowDeleting;

		private bool _allowEditing;

		private bool _allowInserting;

		private bool _allowPaging;

		private bool _allowRemoveField;

		private bool _allowMoveUp;

		private bool _allowMoveDown;
	}
}
