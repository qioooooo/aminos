using System;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	public abstract class DataControlFieldDesigner
	{
		public abstract string DefaultNodeText { get; }

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

		protected IServiceProvider ServiceProvider
		{
			get
			{
				return this._designerForm.ServiceProvider;
			}
		}

		public abstract bool UsesSchema { get; }

		public abstract DataControlField CreateField();

		public abstract DataControlField CreateField(IDataSourceFieldSchema fieldSchema);

		public abstract TemplateField CreateTemplateField(DataControlField dataControlField, DataBoundControl dataBoundControl);

		protected string GetNewDataSourceName(Type controlType, DataBoundControlMode mode)
		{
			DataControlFieldsEditor dataControlFieldsEditor = this.DesignerForm as DataControlFieldsEditor;
			if (dataControlFieldsEditor != null)
			{
				return dataControlFieldsEditor.GetNewDataSourceName(controlType, mode);
			}
			return string.Empty;
		}

		public abstract string GetNodeText(DataControlField dataControlField);

		protected object GetService(Type serviceType)
		{
			if (this.ServiceProvider != null)
			{
				return this.ServiceProvider.GetService(serviceType);
			}
			return null;
		}

		protected ITemplate GetTemplate(DataBoundControl control, string templateContent)
		{
			return DataControlFieldHelper.GetTemplate(control, templateContent);
		}

		protected TemplateField GetTemplateField(DataControlField dataControlField, DataBoundControl dataBoundControl)
		{
			return DataControlFieldHelper.GetTemplateField(dataControlField, dataBoundControl);
		}

		public abstract bool IsEnabled(DataBoundControl parent);

		private DesignerForm _designerForm;
	}
}
