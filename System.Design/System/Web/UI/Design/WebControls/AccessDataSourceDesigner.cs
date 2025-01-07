using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Data;
using System.Security.Permissions;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class AccessDataSourceDesigner : SqlDataSourceDesigner
	{
		private AccessDataSource AccessDataSource
		{
			get
			{
				return (AccessDataSource)base.Component;
			}
		}

		public string DataFile
		{
			get
			{
				return this.AccessDataSource.DataFile;
			}
			set
			{
				if (value != this.DataFile)
				{
					this.AccessDataSource.DataFile = value;
					this.UpdateDesignTimeHtml();
					this.OnDataSourceChanged(EventArgs.Empty);
				}
			}
		}

		internal override SqlDataSourceWizardForm CreateConfigureDataSourceWizardForm(IServiceProvider serviceProvider, IDataEnvironment dataEnvironment)
		{
			return new AccessDataSourceWizardForm(serviceProvider, this, dataEnvironment);
		}

		protected override string GetConnectionString()
		{
			return AccessDataSourceDesigner.GetConnectionString(base.Component.Site, this.AccessDataSource);
		}

		internal static string GetConnectionString(IServiceProvider serviceProvider, AccessDataSource dataSource)
		{
			string dataFile = dataSource.DataFile;
			string connectionString;
			try
			{
				if (dataFile.Length == 0)
				{
					return null;
				}
				dataSource.DataFile = UrlPath.MapPath(serviceProvider, dataFile);
				connectionString = dataSource.ConnectionString;
			}
			finally
			{
				dataSource.DataFile = dataFile;
			}
			return connectionString;
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties["DataFile"];
			properties["DataFile"] = TypeDescriptor.CreateProperty(base.GetType(), propertyDescriptor, new Attribute[0]);
		}
	}
}
