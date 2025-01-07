using System;
using System.Design;
using System.Drawing;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	internal sealed partial class ObjectDataSourceWizardForm : WizardForm
	{
		public ObjectDataSourceWizardForm(IServiceProvider serviceProvider, ObjectDataSourceDesigner objectDataSourceDesigner)
			: base(serviceProvider)
		{
			base.Glyph = new Bitmap(typeof(SqlDataSourceWizardForm), "datasourcewizard.bmp");
			this._objectDataSourceDesigner = objectDataSourceDesigner;
			this._objectDataSource = (ObjectDataSource)this._objectDataSourceDesigner.Component;
			this.Text = SR.GetString("ConfigureDataSource_Title", new object[] { this._objectDataSource.ID });
			ObjectDataSourceChooseTypePanel objectDataSourceChooseTypePanel = new ObjectDataSourceChooseTypePanel(this._objectDataSourceDesigner);
			ObjectDataSourceChooseMethodsPanel objectDataSourceChooseMethodsPanel = new ObjectDataSourceChooseMethodsPanel(this._objectDataSourceDesigner);
			base.SetPanels(new WizardPanel[] { objectDataSourceChooseTypePanel, objectDataSourceChooseMethodsPanel });
			this._parametersPanel = new ObjectDataSourceConfigureParametersPanel(this._objectDataSourceDesigner);
			base.RegisterPanel(this._parametersPanel);
		}

		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.ObjectDataSource.ConfigureDataSource";
			}
		}

		internal ObjectDataSourceConfigureParametersPanel GetParametersPanel()
		{
			this._parametersPanel.ResetUI();
			return this._parametersPanel;
		}

		private ObjectDataSourceDesigner _objectDataSourceDesigner;

		private ObjectDataSource _objectDataSource;

		private ObjectDataSourceConfigureParametersPanel _parametersPanel;
	}
}
