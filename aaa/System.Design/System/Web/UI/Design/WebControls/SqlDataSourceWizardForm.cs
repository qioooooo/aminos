using System;
using System.ComponentModel.Design.Data;
using System.Design;
using System.Drawing;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020003D9 RID: 985
	internal partial class SqlDataSourceWizardForm : WizardForm
	{
		// Token: 0x06002467 RID: 9319 RVA: 0x000C2CEC File Offset: 0x000C1CEC
		public SqlDataSourceWizardForm(IServiceProvider serviceProvider, SqlDataSourceDesigner sqlDataSourceDesigner, IDataEnvironment dataEnvironment)
			: base(serviceProvider)
		{
			base.Glyph = new Bitmap(typeof(SqlDataSourceWizardForm), "datasourcewizard.bmp");
			this._dataEnvironment = dataEnvironment;
			this._sqlDataSource = (SqlDataSource)sqlDataSourceDesigner.Component;
			this._sqlDataSourceDesigner = sqlDataSourceDesigner;
			this.Text = SR.GetString("ConfigureDataSource_Title", new object[] { this._sqlDataSource.ID });
			this._connectionPanel = this.CreateConnectionPanel();
			base.SetPanels(new WizardPanel[] { this._connectionPanel });
			this._saveConfiguredConnectionPanel = new SqlDataSourceSaveConfiguredConnectionPanel(this._sqlDataSourceDesigner, this._dataEnvironment);
			base.RegisterPanel(this._saveConfiguredConnectionPanel);
			this._configureParametersPanel = new SqlDataSourceConfigureParametersPanel(this._sqlDataSourceDesigner);
			base.RegisterPanel(this._configureParametersPanel);
			this._configureSelectPanel = new SqlDataSourceConfigureSelectPanel(this._sqlDataSourceDesigner);
			base.RegisterPanel(this._configureSelectPanel);
			this._customCommandPanel = new SqlDataSourceCustomCommandPanel(this._sqlDataSourceDesigner);
			base.RegisterPanel(this._customCommandPanel);
			this._summaryPanel = new SqlDataSourceSummaryPanel(this._sqlDataSourceDesigner);
			base.RegisterPanel(this._summaryPanel);
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06002468 RID: 9320 RVA: 0x000C2E19 File Offset: 0x000C1E19
		internal DesignerDataConnection DesignerDataConnection
		{
			get
			{
				return this._designerDataConnection;
			}
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06002469 RID: 9321 RVA: 0x000C2E21 File Offset: 0x000C1E21
		internal IDataEnvironment DataEnvironment
		{
			get
			{
				return this._dataEnvironment;
			}
		}

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x0600246A RID: 9322 RVA: 0x000C2E29 File Offset: 0x000C1E29
		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.SqlDataSource.ConfigureDataSource";
			}
		}

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x0600246B RID: 9323 RVA: 0x000C2E30 File Offset: 0x000C1E30
		internal SqlDataSourceDesigner SqlDataSourceDesigner
		{
			get
			{
				return this._sqlDataSourceDesigner;
			}
		}

		// Token: 0x0600246C RID: 9324 RVA: 0x000C2E38 File Offset: 0x000C1E38
		protected virtual SqlDataSourceConnectionPanel CreateConnectionPanel()
		{
			return new SqlDataSourceDataConnectionChooserPanel(this.SqlDataSourceDesigner, this.DataEnvironment);
		}

		// Token: 0x0600246D RID: 9325 RVA: 0x000C2E4B File Offset: 0x000C1E4B
		internal SqlDataSourceConfigureParametersPanel GetConfigureParametersPanel()
		{
			this._configureParametersPanel.ResetUI();
			return this._configureParametersPanel;
		}

		// Token: 0x0600246E RID: 9326 RVA: 0x000C2E5E File Offset: 0x000C1E5E
		internal SqlDataSourceConfigureSelectPanel GetConfigureSelectPanel()
		{
			this._configureSelectPanel.ResetUI();
			return this._configureSelectPanel;
		}

		// Token: 0x0600246F RID: 9327 RVA: 0x000C2E71 File Offset: 0x000C1E71
		internal SqlDataSourceCustomCommandPanel GetCustomCommandPanel()
		{
			this._customCommandPanel.ResetUI();
			return this._customCommandPanel;
		}

		// Token: 0x06002470 RID: 9328 RVA: 0x000C2E84 File Offset: 0x000C1E84
		internal SqlDataSourceSaveConfiguredConnectionPanel GetSaveConfiguredConnectionPanel()
		{
			this._saveConfiguredConnectionPanel.ResetUI();
			return this._saveConfiguredConnectionPanel;
		}

		// Token: 0x06002471 RID: 9329 RVA: 0x000C2E97 File Offset: 0x000C1E97
		internal SqlDataSourceSummaryPanel GetSummaryPanel()
		{
			this._summaryPanel.ResetUI();
			return this._summaryPanel;
		}

		// Token: 0x06002472 RID: 9330 RVA: 0x000C2EAA File Offset: 0x000C1EAA
		protected override void OnPanelChanging(WizardPanelChangingEventArgs e)
		{
			base.OnPanelChanging(e);
			if (e.CurrentPanel == this._connectionPanel)
			{
				this._designerDataConnection = this._connectionPanel.DataConnection;
			}
		}

		// Token: 0x040018F1 RID: 6385
		private SqlDataSourceConnectionPanel _connectionPanel;

		// Token: 0x040018F2 RID: 6386
		private SqlDataSource _sqlDataSource;

		// Token: 0x040018F3 RID: 6387
		private SqlDataSourceDesigner _sqlDataSourceDesigner;

		// Token: 0x040018F4 RID: 6388
		private IDataEnvironment _dataEnvironment;

		// Token: 0x040018F5 RID: 6389
		private DesignerDataConnection _designerDataConnection;

		// Token: 0x040018F6 RID: 6390
		private SqlDataSourceSaveConfiguredConnectionPanel _saveConfiguredConnectionPanel;

		// Token: 0x040018F7 RID: 6391
		private SqlDataSourceConfigureParametersPanel _configureParametersPanel;

		// Token: 0x040018F8 RID: 6392
		private SqlDataSourceConfigureSelectPanel _configureSelectPanel;

		// Token: 0x040018F9 RID: 6393
		private SqlDataSourceCustomCommandPanel _customCommandPanel;

		// Token: 0x040018FA RID: 6394
		private SqlDataSourceSummaryPanel _summaryPanel;
	}
}
