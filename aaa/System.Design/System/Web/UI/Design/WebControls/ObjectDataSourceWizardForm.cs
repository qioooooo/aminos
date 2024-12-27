using System;
using System.Design;
using System.Drawing;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x0200048A RID: 1162
	internal sealed partial class ObjectDataSourceWizardForm : WizardForm
	{
		// Token: 0x06002A3A RID: 10810 RVA: 0x000E8610 File Offset: 0x000E7610
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

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x06002A3B RID: 10811 RVA: 0x000E86CC File Offset: 0x000E76CC
		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.ObjectDataSource.ConfigureDataSource";
			}
		}

		// Token: 0x06002A3C RID: 10812 RVA: 0x000E86D3 File Offset: 0x000E76D3
		internal ObjectDataSourceConfigureParametersPanel GetParametersPanel()
		{
			this._parametersPanel.ResetUI();
			return this._parametersPanel;
		}

		// Token: 0x04001CD8 RID: 7384
		private ObjectDataSourceDesigner _objectDataSourceDesigner;

		// Token: 0x04001CD9 RID: 7385
		private ObjectDataSource _objectDataSource;

		// Token: 0x04001CDA RID: 7386
		private ObjectDataSourceConfigureParametersPanel _parametersPanel;
	}
}
