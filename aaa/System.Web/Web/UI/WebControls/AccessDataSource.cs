using System;
using System.ComponentModel;
using System.Data.Common;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Web.Caching;
using System.Web.Util;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004B4 RID: 1204
	[WebSysDescription("AccessDataSource_Description")]
	[ToolboxBitmap(typeof(AccessDataSource))]
	[Designer("System.Web.UI.Design.WebControls.AccessDataSourceDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[WebSysDisplayName("AccessDataSource_DisplayName")]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class AccessDataSource : SqlDataSource
	{
		// Token: 0x060038BB RID: 14523 RVA: 0x000F0D98 File Offset: 0x000EFD98
		public AccessDataSource()
		{
		}

		// Token: 0x060038BC RID: 14524 RVA: 0x000F0DA0 File Offset: 0x000EFDA0
		public AccessDataSource(string dataFile, string selectCommand)
		{
			if (string.IsNullOrEmpty(dataFile))
			{
				throw new ArgumentNullException("dataFile");
			}
			this.DataFile = dataFile;
			base.SelectCommand = selectCommand;
		}

		// Token: 0x17000CC0 RID: 3264
		// (get) Token: 0x060038BD RID: 14525 RVA: 0x000F0DC9 File Offset: 0x000EFDC9
		internal override DataSourceCache Cache
		{
			get
			{
				if (this._cache == null)
				{
					this._cache = new FileDataSourceCache();
				}
				return this._cache;
			}
		}

		// Token: 0x17000CC1 RID: 3265
		// (get) Token: 0x060038BE RID: 14526 RVA: 0x000F0DE4 File Offset: 0x000EFDE4
		// (set) Token: 0x060038BF RID: 14527 RVA: 0x000F0E00 File Offset: 0x000EFE00
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public override string ConnectionString
		{
			get
			{
				if (this._connectionString == null)
				{
					this._connectionString = this.CreateConnectionString();
				}
				return this._connectionString;
			}
			set
			{
				throw new InvalidOperationException(SR.GetString("AccessDataSource_CannotSetConnectionString"));
			}
		}

		// Token: 0x17000CC2 RID: 3266
		// (get) Token: 0x060038C0 RID: 14528 RVA: 0x000F0E11 File Offset: 0x000EFE11
		// (set) Token: 0x060038C1 RID: 14529 RVA: 0x000F0E27 File Offset: 0x000EFE27
		[WebSysDescription("AccessDataSource_DataFile")]
		[WebCategory("Data")]
		[DefaultValue("")]
		[Editor("System.Web.UI.Design.MdbDataFileEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[UrlProperty]
		public string DataFile
		{
			get
			{
				if (this._dataFile != null)
				{
					return this._dataFile;
				}
				return string.Empty;
			}
			set
			{
				if (this.DataFile != value)
				{
					this._dataFile = value;
					this._connectionString = null;
					this._physicalDataFile = null;
					this.RaiseDataSourceChangedEvent(EventArgs.Empty);
				}
			}
		}

		// Token: 0x17000CC3 RID: 3267
		// (get) Token: 0x060038C2 RID: 14530 RVA: 0x000F0E58 File Offset: 0x000EFE58
		private FileDataSourceCache FileDataSourceCache
		{
			get
			{
				return this.Cache as FileDataSourceCache;
			}
		}

		// Token: 0x17000CC4 RID: 3268
		// (get) Token: 0x060038C3 RID: 14531 RVA: 0x000F0E72 File Offset: 0x000EFE72
		private string PhysicalDataFile
		{
			get
			{
				if (this._physicalDataFile == null)
				{
					this._physicalDataFile = this.GetPhysicalDataFilePath();
				}
				return this._physicalDataFile;
			}
		}

		// Token: 0x17000CC5 RID: 3269
		// (get) Token: 0x060038C4 RID: 14532 RVA: 0x000F0E8E File Offset: 0x000EFE8E
		// (set) Token: 0x060038C5 RID: 14533 RVA: 0x000F0E98 File Offset: 0x000EFE98
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override string ProviderName
		{
			get
			{
				return "System.Data.OleDb";
			}
			set
			{
				throw new InvalidOperationException(SR.GetString("AccessDataSource_CannotSetProvider", new object[] { this.ID }));
			}
		}

		// Token: 0x17000CC6 RID: 3270
		// (get) Token: 0x060038C6 RID: 14534 RVA: 0x000F0EC8 File Offset: 0x000EFEC8
		// (set) Token: 0x060038C7 RID: 14535 RVA: 0x000F0EF8 File Offset: 0x000EFEF8
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public override string SqlCacheDependency
		{
			get
			{
				throw new NotSupportedException(SR.GetString("AccessDataSource_SqlCacheDependencyNotSupported", new object[] { this.ID }));
			}
			set
			{
				throw new NotSupportedException(SR.GetString("AccessDataSource_SqlCacheDependencyNotSupported", new object[] { this.ID }));
			}
		}

		// Token: 0x060038C8 RID: 14536 RVA: 0x000F0F28 File Offset: 0x000EFF28
		private void AddCacheFileDependency()
		{
			this.FileDataSourceCache.FileDependencies.Clear();
			string physicalDataFile = this.PhysicalDataFile;
			if (physicalDataFile.Length > 0)
			{
				this.FileDataSourceCache.FileDependencies.Add(physicalDataFile);
			}
		}

		// Token: 0x060038C9 RID: 14537 RVA: 0x000F0F67 File Offset: 0x000EFF67
		private string CreateConnectionString()
		{
			return "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + this.PhysicalDataFile;
		}

		// Token: 0x060038CA RID: 14538 RVA: 0x000F0F79 File Offset: 0x000EFF79
		protected override SqlDataSourceView CreateDataSourceView(string viewName)
		{
			return new AccessDataSourceView(this, viewName, this.Context);
		}

		// Token: 0x060038CB RID: 14539 RVA: 0x000F0F88 File Offset: 0x000EFF88
		protected override DbProviderFactory GetDbProviderFactory()
		{
			return OleDbFactory.Instance;
		}

		// Token: 0x060038CC RID: 14540 RVA: 0x000F0F90 File Offset: 0x000EFF90
		private string GetPhysicalDataFilePath()
		{
			string text = this.DataFile;
			if (text.Length == 0)
			{
				return null;
			}
			if (!UrlPath.IsAbsolutePhysicalPath(text))
			{
				if (base.DesignMode)
				{
					throw new NotSupportedException(SR.GetString("AccessDataSource_DesignTimeRelativePathsNotSupported", new object[] { this.ID }));
				}
				text = this.Context.Request.MapPath(text, base.AppRelativeTemplateSourceDirectory, true);
			}
			HttpRuntime.CheckFilePermission(text, true);
			if (!HttpRuntime.HasPathDiscoveryPermission(text))
			{
				throw new HttpException(SR.GetString("AccessDataSource_NoPathDiscoveryPermission", new object[]
				{
					HttpRuntime.GetSafePath(text),
					this.ID
				}));
			}
			return text;
		}

		// Token: 0x060038CD RID: 14541 RVA: 0x000F1032 File Offset: 0x000F0032
		internal override void SaveDataToCache(int startRowIndex, int maximumRows, object data, CacheDependency dependency)
		{
			this.AddCacheFileDependency();
			base.SaveDataToCache(startRowIndex, maximumRows, data, dependency);
		}

		// Token: 0x040025E8 RID: 9704
		private const string OleDbProviderName = "System.Data.OleDb";

		// Token: 0x040025E9 RID: 9705
		private FileDataSourceCache _cache;

		// Token: 0x040025EA RID: 9706
		private string _connectionString;

		// Token: 0x040025EB RID: 9707
		private string _dataFile;

		// Token: 0x040025EC RID: 9708
		private string _physicalDataFile;
	}
}
