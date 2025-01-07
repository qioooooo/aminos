using System;
using System.ComponentModel.Design.Data;
using System.Design;
using System.Drawing;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	internal partial class SqlDataSourceWizardForm : WizardForm
	{
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

		internal DesignerDataConnection DesignerDataConnection
		{
			get
			{
				return this._designerDataConnection;
			}
		}

		internal IDataEnvironment DataEnvironment
		{
			get
			{
				return this._dataEnvironment;
			}
		}

		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.SqlDataSource.ConfigureDataSource";
			}
		}

		internal SqlDataSourceDesigner SqlDataSourceDesigner
		{
			get
			{
				return this._sqlDataSourceDesigner;
			}
		}

		protected virtual SqlDataSourceConnectionPanel CreateConnectionPanel()
		{
			return new SqlDataSourceDataConnectionChooserPanel(this.SqlDataSourceDesigner, this.DataEnvironment);
		}

		internal SqlDataSourceConfigureParametersPanel GetConfigureParametersPanel()
		{
			this._configureParametersPanel.ResetUI();
			return this._configureParametersPanel;
		}

		internal SqlDataSourceConfigureSelectPanel GetConfigureSelectPanel()
		{
			this._configureSelectPanel.ResetUI();
			return this._configureSelectPanel;
		}

		internal SqlDataSourceCustomCommandPanel GetCustomCommandPanel()
		{
			this._customCommandPanel.ResetUI();
			return this._customCommandPanel;
		}

		internal SqlDataSourceSaveConfiguredConnectionPanel GetSaveConfiguredConnectionPanel()
		{
			this._saveConfiguredConnectionPanel.ResetUI();
			return this._saveConfiguredConnectionPanel;
		}

		internal SqlDataSourceSummaryPanel GetSummaryPanel()
		{
			this._summaryPanel.ResetUI();
			return this._summaryPanel;
		}

		protected override void OnPanelChanging(WizardPanelChangingEventArgs e)
		{
			base.OnPanelChanging(e);
			if (e.CurrentPanel == this._connectionPanel)
			{
				this._designerDataConnection = this._connectionPanel.DataConnection;
			}
		}

		private SqlDataSourceConnectionPanel _connectionPanel;

		private SqlDataSource _sqlDataSource;

		private SqlDataSourceDesigner _sqlDataSourceDesigner;

		private IDataEnvironment _dataEnvironment;

		private DesignerDataConnection _designerDataConnection;

		private SqlDataSourceSaveConfiguredConnectionPanel _saveConfiguredConnectionPanel;

		private SqlDataSourceConfigureParametersPanel _configureParametersPanel;

		private SqlDataSourceConfigureSelectPanel _configureSelectPanel;

		private SqlDataSourceCustomCommandPanel _customCommandPanel;

		private SqlDataSourceSummaryPanel _summaryPanel;
	}
}
