using System;
using System.ComponentModel.Design;
using System.Design;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x0200044C RID: 1100
	internal class FormViewActionList : DesignerActionList
	{
		// Token: 0x060027FA RID: 10234 RVA: 0x000DB218 File Offset: 0x000DA218
		public FormViewActionList(FormViewDesigner formViewDesigner)
			: base(formViewDesigner.Component)
		{
			this._formViewDesigner = formViewDesigner;
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x060027FB RID: 10235 RVA: 0x000DB22D File Offset: 0x000DA22D
		// (set) Token: 0x060027FC RID: 10236 RVA: 0x000DB235 File Offset: 0x000DA235
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

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x060027FD RID: 10237 RVA: 0x000DB23E File Offset: 0x000DA23E
		// (set) Token: 0x060027FE RID: 10238 RVA: 0x000DB246 File Offset: 0x000DA246
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

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x060027FF RID: 10239 RVA: 0x000DB24F File Offset: 0x000DA24F
		// (set) Token: 0x06002800 RID: 10240 RVA: 0x000DB252 File Offset: 0x000DA252
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

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06002801 RID: 10241 RVA: 0x000DB254 File Offset: 0x000DA254
		// (set) Token: 0x06002802 RID: 10242 RVA: 0x000DB261 File Offset: 0x000DA261
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

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06002803 RID: 10243 RVA: 0x000DB26F File Offset: 0x000DA26F
		// (set) Token: 0x06002804 RID: 10244 RVA: 0x000DB27C File Offset: 0x000DA27C
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

		// Token: 0x06002805 RID: 10245 RVA: 0x000DB28C File Offset: 0x000DA28C
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

		// Token: 0x04001BA6 RID: 7078
		private FormViewDesigner _formViewDesigner;

		// Token: 0x04001BA7 RID: 7079
		private bool _allowDynamicData;

		// Token: 0x04001BA8 RID: 7080
		private bool _allowPaging;
	}
}
