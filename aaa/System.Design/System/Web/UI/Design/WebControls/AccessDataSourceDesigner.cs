using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Data;
using System.Security.Permissions;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020003D8 RID: 984
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class AccessDataSourceDesigner : SqlDataSourceDesigner
	{
		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x0600245F RID: 9311 RVA: 0x000C2BE2 File Offset: 0x000C1BE2
		private AccessDataSource AccessDataSource
		{
			get
			{
				return (AccessDataSource)base.Component;
			}
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06002460 RID: 9312 RVA: 0x000C2BEF File Offset: 0x000C1BEF
		// (set) Token: 0x06002461 RID: 9313 RVA: 0x000C2BFC File Offset: 0x000C1BFC
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

		// Token: 0x06002462 RID: 9314 RVA: 0x000C2C29 File Offset: 0x000C1C29
		internal override SqlDataSourceWizardForm CreateConfigureDataSourceWizardForm(IServiceProvider serviceProvider, IDataEnvironment dataEnvironment)
		{
			return new AccessDataSourceWizardForm(serviceProvider, this, dataEnvironment);
		}

		// Token: 0x06002463 RID: 9315 RVA: 0x000C2C33 File Offset: 0x000C1C33
		protected override string GetConnectionString()
		{
			return AccessDataSourceDesigner.GetConnectionString(base.Component.Site, this.AccessDataSource);
		}

		// Token: 0x06002464 RID: 9316 RVA: 0x000C2C4C File Offset: 0x000C1C4C
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

		// Token: 0x06002465 RID: 9317 RVA: 0x000C2CA0 File Offset: 0x000C1CA0
		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties["DataFile"];
			properties["DataFile"] = TypeDescriptor.CreateProperty(base.GetType(), propertyDescriptor, new Attribute[0]);
		}
	}
}
