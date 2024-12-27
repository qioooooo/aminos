using System;
using System.Collections;
using System.ComponentModel.Design.Data;
using System.Design;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020003D5 RID: 981
	internal abstract class SqlDataSourceConnectionPanel : WizardPanel
	{
		// Token: 0x06002415 RID: 9237 RVA: 0x000C12D5 File Offset: 0x000C02D5
		protected SqlDataSourceConnectionPanel(SqlDataSourceDesigner sqlDataSourceDesigner)
		{
			this._sqlDataSourceDesigner = sqlDataSourceDesigner;
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06002416 RID: 9238
		public abstract DesignerDataConnection DataConnection { get; }

		// Token: 0x06002417 RID: 9239 RVA: 0x000C12E4 File Offset: 0x000C02E4
		protected bool CheckValidProvider()
		{
			DesignerDataConnection dataConnection = this.DataConnection;
			bool flag;
			try
			{
				SqlDataSourceDesigner.GetDbProviderFactory(dataConnection.ProviderName);
				flag = true;
			}
			catch (Exception ex)
			{
				UIServiceHelper.ShowError(base.ServiceProvider, ex, SR.GetString("SqlDataSourceConnectionPanel_ProviderNotFound", new object[] { dataConnection.ProviderName }));
				flag = false;
			}
			return flag;
		}

		// Token: 0x06002418 RID: 9240 RVA: 0x000C1348 File Offset: 0x000C0348
		internal static WizardPanel CreateCommandPanel(SqlDataSourceWizardForm wizard, DesignerDataConnection dataConnection, WizardPanel nextPanel)
		{
			IDataEnvironment dataEnvironment = null;
			IServiceProvider site = wizard.SqlDataSourceDesigner.Component.Site;
			if (site != null)
			{
				dataEnvironment = (IDataEnvironment)site.GetService(typeof(IDataEnvironment));
			}
			bool flag = false;
			if (dataEnvironment != null)
			{
				try
				{
					IDesignerDataSchema connectionSchema = dataEnvironment.GetConnectionSchema(dataConnection);
					if (connectionSchema != null)
					{
						flag = connectionSchema.SupportsSchemaClass(DesignerDataSchemaClass.Tables);
						if (flag)
						{
							connectionSchema.GetSchemaItems(DesignerDataSchemaClass.Tables);
						}
						else
						{
							flag = connectionSchema.SupportsSchemaClass(DesignerDataSchemaClass.Views);
							connectionSchema.GetSchemaItems(DesignerDataSchemaClass.Views);
						}
					}
				}
				catch (Exception ex)
				{
					UIServiceHelper.ShowError(site, ex, SR.GetString("SqlDataSourceConnectionPanel_CouldNotGetConnectionSchema"));
					return null;
				}
			}
			if (nextPanel != null)
			{
				if (flag)
				{
					if (!(nextPanel is SqlDataSourceConfigureSelectPanel))
					{
						return wizard.GetConfigureSelectPanel();
					}
				}
				else if (!(nextPanel is SqlDataSourceCustomCommandPanel))
				{
					return SqlDataSourceConnectionPanel.CreateCustomCommandPanel(wizard, dataConnection);
				}
				return nextPanel;
			}
			if (flag)
			{
				return wizard.GetConfigureSelectPanel();
			}
			return SqlDataSourceConnectionPanel.CreateCustomCommandPanel(wizard, dataConnection);
		}

		// Token: 0x06002419 RID: 9241 RVA: 0x000C1434 File Offset: 0x000C0434
		private static WizardPanel CreateCustomCommandPanel(SqlDataSourceWizardForm wizard, DesignerDataConnection dataConnection)
		{
			SqlDataSource sqlDataSource = (SqlDataSource)wizard.SqlDataSourceDesigner.Component;
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			ArrayList arrayList3 = new ArrayList();
			ArrayList arrayList4 = new ArrayList();
			wizard.SqlDataSourceDesigner.CopyList(sqlDataSource.SelectParameters, arrayList);
			wizard.SqlDataSourceDesigner.CopyList(sqlDataSource.InsertParameters, arrayList2);
			wizard.SqlDataSourceDesigner.CopyList(sqlDataSource.UpdateParameters, arrayList3);
			wizard.SqlDataSourceDesigner.CopyList(sqlDataSource.DeleteParameters, arrayList4);
			SqlDataSourceCustomCommandPanel customCommandPanel = wizard.GetCustomCommandPanel();
			customCommandPanel.SetQueries(dataConnection, new SqlDataSourceQuery(sqlDataSource.SelectCommand, sqlDataSource.SelectCommandType, arrayList), new SqlDataSourceQuery(sqlDataSource.InsertCommand, sqlDataSource.InsertCommandType, arrayList2), new SqlDataSourceQuery(sqlDataSource.UpdateCommand, sqlDataSource.UpdateCommandType, arrayList3), new SqlDataSourceQuery(sqlDataSource.DeleteCommand, sqlDataSource.DeleteCommandType, arrayList4));
			return customCommandPanel;
		}

		// Token: 0x0600241A RID: 9242 RVA: 0x000C1510 File Offset: 0x000C0510
		public override bool OnNext()
		{
			if (!this.CheckValidProvider())
			{
				return false;
			}
			WizardPanel wizardPanel = SqlDataSourceConnectionPanel.CreateCommandPanel((SqlDataSourceWizardForm)base.ParentWizard, this.DataConnection, base.NextPanel);
			if (wizardPanel == null)
			{
				return false;
			}
			base.NextPanel = wizardPanel;
			return true;
		}

		// Token: 0x040018DE RID: 6366
		private SqlDataSourceDesigner _sqlDataSourceDesigner;
	}
}
