using System;
using System.ComponentModel;
using System.ComponentModel.Design.Data;
using System.Security.Permissions;
using System.Web.Compilation;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class SqlDataSourceConnectionStringEditor : ConnectionStringEditor
	{
		protected override string GetProviderName(object instance)
		{
			SqlDataSource sqlDataSource = instance as SqlDataSource;
			if (sqlDataSource != null)
			{
				return sqlDataSource.ProviderName;
			}
			return string.Empty;
		}

		protected override void SetProviderName(object instance, DesignerDataConnection connection)
		{
			SqlDataSource sqlDataSource = instance as SqlDataSource;
			if (sqlDataSource != null)
			{
				if (connection.IsConfigured)
				{
					ExpressionEditor expressionEditor = ExpressionEditor.GetExpressionEditor(typeof(ConnectionStringsExpressionBuilder), sqlDataSource.Site);
					if (expressionEditor != null)
					{
						string expressionPrefix = expressionEditor.ExpressionPrefix;
						ExpressionBindingCollection expressions = ((IExpressionsAccessor)sqlDataSource).Expressions;
						expressions.Add(new ExpressionBinding("ProviderName", typeof(string), expressionPrefix, connection.Name + ".ProviderName"));
						return;
					}
				}
				else
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(sqlDataSource)["ProviderName"];
					propertyDescriptor.SetValue(sqlDataSource, connection.ProviderName);
				}
			}
		}
	}
}
