using System;
using System.ComponentModel.Design;
using System.Design;

namespace System.Web.UI.Design.WebControls
{
	internal class FormViewActionList : DesignerActionList
	{
		public FormViewActionList(FormViewDesigner formViewDesigner)
			: base(formViewDesigner.Component)
		{
			this._formViewDesigner = formViewDesigner;
		}

		internal bool AllowDynamicData
		{
			get
			{
				return this._allowDynamicData;
			}
			set
			{
				this._allowDynamicData = value;
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

		public bool EnableDynamicData
		{
			get
			{
				return this._formViewDesigner.EnableDynamicData;
			}
			set
			{
				this._formViewDesigner.EnableDynamicData = value;
			}
		}

		public bool EnablePaging
		{
			get
			{
				return this._formViewDesigner.EnablePaging;
			}
			set
			{
				this._formViewDesigner.EnablePaging = value;
			}
		}

		public override DesignerActionItemCollection GetSortedActionItems()
		{
			DesignerActionItemCollection designerActionItemCollection = new DesignerActionItemCollection();
			if (this.AllowDynamicData)
			{
				designerActionItemCollection.Add(new DesignerActionPropertyItem("EnableDynamicData", SR.GetString("FormView_EnableDynamicData"), "Behavior", SR.GetString("FormView_EnableDynamicDataDesc")));
			}
			if (this.AllowPaging)
			{
				designerActionItemCollection.Add(new DesignerActionPropertyItem("EnablePaging", SR.GetString("FormView_EnablePaging"), "Behavior", SR.GetString("FormView_EnablePagingDesc")));
			}
			return designerActionItemCollection;
		}

		private FormViewDesigner _formViewDesigner;

		private bool _allowDynamicData;

		private bool _allowPaging;
	}
}
