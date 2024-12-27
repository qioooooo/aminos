using System;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x0200041D RID: 1053
	public abstract class DataControlFieldDesigner
	{
		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06002693 RID: 9875
		public abstract string DefaultNodeText { get; }

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x06002694 RID: 9876 RVA: 0x000D14C1 File Offset: 0x000D04C1
		// (set) Token: 0x06002695 RID: 9877 RVA: 0x000D14C9 File Offset: 0x000D04C9
		internal DesignerForm DesignerForm
		{
			get
			{
				return this._designerForm;
			}
			set
			{
				this._designerForm = value;
			}
		}

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x06002696 RID: 9878 RVA: 0x000D14D2 File Offset: 0x000D04D2
		protected IServiceProvider ServiceProvider
		{
			get
			{
				return this._designerForm.ServiceProvider;
			}
		}

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x06002697 RID: 9879
		public abstract bool UsesSchema { get; }

		// Token: 0x06002698 RID: 9880
		public abstract DataControlField CreateField();

		// Token: 0x06002699 RID: 9881
		public abstract DataControlField CreateField(IDataSourceFieldSchema fieldSchema);

		// Token: 0x0600269A RID: 9882
		public abstract TemplateField CreateTemplateField(DataControlField dataControlField, DataBoundControl dataBoundControl);

		// Token: 0x0600269B RID: 9883 RVA: 0x000D14E0 File Offset: 0x000D04E0
		protected string GetNewDataSourceName(Type controlType, DataBoundControlMode mode)
		{
			DataControlFieldsEditor dataControlFieldsEditor = this.DesignerForm as DataControlFieldsEditor;
			if (dataControlFieldsEditor != null)
			{
				return dataControlFieldsEditor.GetNewDataSourceName(controlType, mode);
			}
			return string.Empty;
		}

		// Token: 0x0600269C RID: 9884
		public abstract string GetNodeText(DataControlField dataControlField);

		// Token: 0x0600269D RID: 9885 RVA: 0x000D150A File Offset: 0x000D050A
		protected object GetService(Type serviceType)
		{
			if (this.ServiceProvider != null)
			{
				return this.ServiceProvider.GetService(serviceType);
			}
			return null;
		}

		// Token: 0x0600269E RID: 9886 RVA: 0x000D1522 File Offset: 0x000D0522
		protected ITemplate GetTemplate(DataBoundControl control, string templateContent)
		{
			return DataControlFieldHelper.GetTemplate(control, templateContent);
		}

		// Token: 0x0600269F RID: 9887 RVA: 0x000D152B File Offset: 0x000D052B
		protected TemplateField GetTemplateField(DataControlField dataControlField, DataBoundControl dataBoundControl)
		{
			return DataControlFieldHelper.GetTemplateField(dataControlField, dataBoundControl);
		}

		// Token: 0x060026A0 RID: 9888
		public abstract bool IsEnabled(DataBoundControl parent);

		// Token: 0x04001A8C RID: 6796
		private DesignerForm _designerForm;
	}
}
