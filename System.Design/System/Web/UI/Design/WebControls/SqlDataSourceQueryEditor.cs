using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing.Design;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	internal sealed class SqlDataSourceQueryEditor : UITypeEditor
	{
		private bool EditQueryChangeCallback(object context)
		{
			SqlDataSource sqlDataSource = (SqlDataSource)((Pair)context).First;
			DataSourceOperation dataSourceOperation = (DataSourceOperation)((Pair)context).Second;
			IServiceProvider site = sqlDataSource.Site;
			IDesignerHost designerHost = (IDesignerHost)site.GetService(typeof(IDesignerHost));
			SqlDataSourceDesigner sqlDataSourceDesigner = (SqlDataSourceDesigner)designerHost.GetDesigner(sqlDataSource);
			ParameterCollection parameterCollection = null;
			string text = string.Empty;
			SqlDataSourceCommandType sqlDataSourceCommandType = SqlDataSourceCommandType.Text;
			switch (dataSourceOperation)
			{
			case DataSourceOperation.Delete:
				parameterCollection = sqlDataSource.DeleteParameters;
				text = sqlDataSource.DeleteCommand;
				sqlDataSourceCommandType = sqlDataSource.DeleteCommandType;
				break;
			case DataSourceOperation.Insert:
				parameterCollection = sqlDataSource.InsertParameters;
				text = sqlDataSource.InsertCommand;
				sqlDataSourceCommandType = sqlDataSource.InsertCommandType;
				break;
			case DataSourceOperation.Select:
				parameterCollection = sqlDataSource.SelectParameters;
				text = sqlDataSource.SelectCommand;
				sqlDataSourceCommandType = sqlDataSource.SelectCommandType;
				break;
			case DataSourceOperation.Update:
				parameterCollection = sqlDataSource.UpdateParameters;
				text = sqlDataSource.UpdateCommand;
				sqlDataSourceCommandType = sqlDataSource.UpdateCommandType;
				break;
			}
			SqlDataSourceQueryEditorForm sqlDataSourceQueryEditorForm = new SqlDataSourceQueryEditorForm(site, sqlDataSourceDesigner, sqlDataSource.ProviderName, sqlDataSourceDesigner.ConnectionString, dataSourceOperation, sqlDataSourceCommandType, text, parameterCollection);
			DialogResult dialogResult = UIServiceHelper.ShowDialog(site, sqlDataSourceQueryEditorForm);
			if (dialogResult == DialogResult.OK)
			{
				PropertyDescriptor propertyDescriptor = null;
				switch (dataSourceOperation)
				{
				case DataSourceOperation.Delete:
					propertyDescriptor = TypeDescriptor.GetProperties(sqlDataSource)["DeleteCommand"];
					break;
				case DataSourceOperation.Insert:
					propertyDescriptor = TypeDescriptor.GetProperties(sqlDataSource)["InsertCommand"];
					break;
				case DataSourceOperation.Select:
					propertyDescriptor = TypeDescriptor.GetProperties(sqlDataSource)["SelectCommand"];
					break;
				case DataSourceOperation.Update:
					propertyDescriptor = TypeDescriptor.GetProperties(sqlDataSource)["UpdateCommand"];
					break;
				}
				if (propertyDescriptor != null)
				{
					propertyDescriptor.ResetValue(sqlDataSource);
					propertyDescriptor.SetValue(sqlDataSource, sqlDataSourceQueryEditorForm.Command);
				}
				return true;
			}
			return false;
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			ControlDesigner.InvokeTransactedChange((IComponent)context.Instance, new TransactedChangeCallback(this.EditQueryChangeCallback), new Pair(context.Instance, value), SR.GetString("SqlDataSourceDesigner_EditQueryTransactionDescription"));
			return value;
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}
	}
}
