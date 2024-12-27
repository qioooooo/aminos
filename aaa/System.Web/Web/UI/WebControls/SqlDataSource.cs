using System;
using System.Collections;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Security.Permissions;
using System.Text;
using System.Web.Caching;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004B3 RID: 1203
	[ToolboxBitmap(typeof(SqlDataSource))]
	[ParseChildren(true)]
	[PersistChildren(false)]
	[DefaultEvent("Selecting")]
	[WebSysDescription("SqlDataSource_Description")]
	[WebSysDisplayName("SqlDataSource_DisplayName")]
	[DefaultProperty("SelectQuery")]
	[Designer("System.Web.UI.Design.WebControls.SqlDataSourceDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class SqlDataSource : DataSourceControl
	{
		// Token: 0x0600385B RID: 14427 RVA: 0x000F029C File Offset: 0x000EF29C
		public SqlDataSource()
		{
		}

		// Token: 0x0600385C RID: 14428 RVA: 0x000F02AB File Offset: 0x000EF2AB
		public SqlDataSource(string connectionString, string selectCommand)
		{
			this._connectionString = connectionString;
			this._cachedSelectCommand = selectCommand;
		}

		// Token: 0x0600385D RID: 14429 RVA: 0x000F02C8 File Offset: 0x000EF2C8
		public SqlDataSource(string providerName, string connectionString, string selectCommand)
			: this(connectionString, selectCommand)
		{
			this._providerName = providerName;
		}

		// Token: 0x17000CA4 RID: 3236
		// (get) Token: 0x0600385E RID: 14430 RVA: 0x000F02D9 File Offset: 0x000EF2D9
		internal virtual DataSourceCache Cache
		{
			get
			{
				if (this._cache == null)
				{
					this._cache = new SqlDataSourceCache();
				}
				return this._cache;
			}
		}

		// Token: 0x17000CA5 RID: 3237
		// (get) Token: 0x0600385F RID: 14431 RVA: 0x000F02F4 File Offset: 0x000EF2F4
		// (set) Token: 0x06003860 RID: 14432 RVA: 0x000F0301 File Offset: 0x000EF301
		[WebSysDescription("DataSourceCache_Duration")]
		[DefaultValue(0)]
		[TypeConverter(typeof(DataSourceCacheDurationConverter))]
		[WebCategory("Cache")]
		public virtual int CacheDuration
		{
			get
			{
				return this.Cache.Duration;
			}
			set
			{
				this.Cache.Duration = value;
			}
		}

		// Token: 0x17000CA6 RID: 3238
		// (get) Token: 0x06003861 RID: 14433 RVA: 0x000F030F File Offset: 0x000EF30F
		// (set) Token: 0x06003862 RID: 14434 RVA: 0x000F031C File Offset: 0x000EF31C
		[WebSysDescription("DataSourceCache_ExpirationPolicy")]
		[DefaultValue(DataSourceCacheExpiry.Absolute)]
		[WebCategory("Cache")]
		public virtual DataSourceCacheExpiry CacheExpirationPolicy
		{
			get
			{
				return this.Cache.ExpirationPolicy;
			}
			set
			{
				this.Cache.ExpirationPolicy = value;
			}
		}

		// Token: 0x17000CA7 RID: 3239
		// (get) Token: 0x06003863 RID: 14435 RVA: 0x000F032A File Offset: 0x000EF32A
		// (set) Token: 0x06003864 RID: 14436 RVA: 0x000F0337 File Offset: 0x000EF337
		[WebSysDescription("DataSourceCache_KeyDependency")]
		[WebCategory("Cache")]
		[DefaultValue("")]
		public virtual string CacheKeyDependency
		{
			get
			{
				return this.Cache.KeyDependency;
			}
			set
			{
				this.Cache.KeyDependency = value;
			}
		}

		// Token: 0x17000CA8 RID: 3240
		// (get) Token: 0x06003865 RID: 14437 RVA: 0x000F0345 File Offset: 0x000EF345
		// (set) Token: 0x06003866 RID: 14438 RVA: 0x000F0352 File Offset: 0x000EF352
		[WebSysDescription("SqlDataSource_CancelSelectOnNullParameter")]
		[WebCategory("Data")]
		[DefaultValue(true)]
		public virtual bool CancelSelectOnNullParameter
		{
			get
			{
				return this.GetView().CancelSelectOnNullParameter;
			}
			set
			{
				this.GetView().CancelSelectOnNullParameter = value;
			}
		}

		// Token: 0x17000CA9 RID: 3241
		// (get) Token: 0x06003867 RID: 14439 RVA: 0x000F0360 File Offset: 0x000EF360
		// (set) Token: 0x06003868 RID: 14440 RVA: 0x000F036D File Offset: 0x000EF36D
		[WebCategory("Data")]
		[WebSysDescription("SqlDataSource_ConflictDetection")]
		[DefaultValue(ConflictOptions.OverwriteChanges)]
		public ConflictOptions ConflictDetection
		{
			get
			{
				return this.GetView().ConflictDetection;
			}
			set
			{
				this.GetView().ConflictDetection = value;
			}
		}

		// Token: 0x17000CAA RID: 3242
		// (get) Token: 0x06003869 RID: 14441 RVA: 0x000F037B File Offset: 0x000EF37B
		// (set) Token: 0x0600386A RID: 14442 RVA: 0x000F0391 File Offset: 0x000EF391
		[WebSysDescription("SqlDataSource_ConnectionString")]
		[WebCategory("Data")]
		[DefaultValue("")]
		[Editor("System.Web.UI.Design.WebControls.SqlDataSourceConnectionStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public virtual string ConnectionString
		{
			get
			{
				if (this._connectionString != null)
				{
					return this._connectionString;
				}
				return string.Empty;
			}
			set
			{
				if (this.ConnectionString != value)
				{
					this._connectionString = value;
					this.RaiseDataSourceChangedEvent(EventArgs.Empty);
				}
			}
		}

		// Token: 0x17000CAB RID: 3243
		// (get) Token: 0x0600386B RID: 14443 RVA: 0x000F03B3 File Offset: 0x000EF3B3
		// (set) Token: 0x0600386C RID: 14444 RVA: 0x000F03BC File Offset: 0x000EF3BC
		[DefaultValue(SqlDataSourceMode.DataSet)]
		[WebSysDescription("SqlDataSource_DataSourceMode")]
		[WebCategory("Behavior")]
		public SqlDataSourceMode DataSourceMode
		{
			get
			{
				return this._dataSourceMode;
			}
			set
			{
				if (value < SqlDataSourceMode.DataReader || value > SqlDataSourceMode.DataSet)
				{
					throw new ArgumentOutOfRangeException(SR.GetString("SqlDataSource_InvalidMode", new object[] { this.ID }));
				}
				if (this.DataSourceMode != value)
				{
					this._dataSourceMode = value;
					this.RaiseDataSourceChangedEvent(EventArgs.Empty);
				}
			}
		}

		// Token: 0x17000CAC RID: 3244
		// (get) Token: 0x0600386D RID: 14445 RVA: 0x000F040D File Offset: 0x000EF40D
		// (set) Token: 0x0600386E RID: 14446 RVA: 0x000F041A File Offset: 0x000EF41A
		[DefaultValue("")]
		[WebCategory("Data")]
		[WebSysDescription("SqlDataSource_DeleteCommand")]
		public string DeleteCommand
		{
			get
			{
				return this.GetView().DeleteCommand;
			}
			set
			{
				this.GetView().DeleteCommand = value;
			}
		}

		// Token: 0x17000CAD RID: 3245
		// (get) Token: 0x0600386F RID: 14447 RVA: 0x000F0428 File Offset: 0x000EF428
		// (set) Token: 0x06003870 RID: 14448 RVA: 0x000F0435 File Offset: 0x000EF435
		[WebSysDescription("SqlDataSource_DeleteCommandType")]
		[DefaultValue(SqlDataSourceCommandType.Text)]
		[WebCategory("Data")]
		public SqlDataSourceCommandType DeleteCommandType
		{
			get
			{
				return this.GetView().DeleteCommandType;
			}
			set
			{
				this.GetView().DeleteCommandType = value;
			}
		}

		// Token: 0x17000CAE RID: 3246
		// (get) Token: 0x06003871 RID: 14449 RVA: 0x000F0443 File Offset: 0x000EF443
		[DefaultValue(null)]
		[WebSysDescription("SqlDataSource_DeleteParameters")]
		[Editor("System.Web.UI.Design.WebControls.ParameterCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[MergableProperty(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Data")]
		public ParameterCollection DeleteParameters
		{
			get
			{
				return this.GetView().DeleteParameters;
			}
		}

		// Token: 0x17000CAF RID: 3247
		// (get) Token: 0x06003872 RID: 14450 RVA: 0x000F0450 File Offset: 0x000EF450
		// (set) Token: 0x06003873 RID: 14451 RVA: 0x000F045D File Offset: 0x000EF45D
		[DefaultValue(false)]
		[WebSysDescription("DataSourceCache_Enabled")]
		[WebCategory("Cache")]
		public virtual bool EnableCaching
		{
			get
			{
				return this.Cache.Enabled;
			}
			set
			{
				this.Cache.Enabled = value;
			}
		}

		// Token: 0x17000CB0 RID: 3248
		// (get) Token: 0x06003874 RID: 14452 RVA: 0x000F046B File Offset: 0x000EF46B
		// (set) Token: 0x06003875 RID: 14453 RVA: 0x000F0478 File Offset: 0x000EF478
		[WebSysDescription("SqlDataSource_FilterExpression")]
		[WebCategory("Data")]
		[DefaultValue("")]
		public string FilterExpression
		{
			get
			{
				return this.GetView().FilterExpression;
			}
			set
			{
				this.GetView().FilterExpression = value;
			}
		}

		// Token: 0x17000CB1 RID: 3249
		// (get) Token: 0x06003876 RID: 14454 RVA: 0x000F0486 File Offset: 0x000EF486
		[Editor("System.Web.UI.Design.WebControls.ParameterCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[MergableProperty(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Data")]
		[WebSysDescription("SqlDataSource_FilterParameters")]
		[DefaultValue(null)]
		public ParameterCollection FilterParameters
		{
			get
			{
				return this.GetView().FilterParameters;
			}
		}

		// Token: 0x17000CB2 RID: 3250
		// (get) Token: 0x06003877 RID: 14455 RVA: 0x000F0493 File Offset: 0x000EF493
		// (set) Token: 0x06003878 RID: 14456 RVA: 0x000F04A0 File Offset: 0x000EF4A0
		[WebCategory("Data")]
		[DefaultValue("")]
		[WebSysDescription("SqlDataSource_InsertCommand")]
		public string InsertCommand
		{
			get
			{
				return this.GetView().InsertCommand;
			}
			set
			{
				this.GetView().InsertCommand = value;
			}
		}

		// Token: 0x17000CB3 RID: 3251
		// (get) Token: 0x06003879 RID: 14457 RVA: 0x000F04AE File Offset: 0x000EF4AE
		// (set) Token: 0x0600387A RID: 14458 RVA: 0x000F04BB File Offset: 0x000EF4BB
		[WebSysDescription("SqlDataSource_InsertCommandType")]
		[DefaultValue(SqlDataSourceCommandType.Text)]
		[WebCategory("Data")]
		public SqlDataSourceCommandType InsertCommandType
		{
			get
			{
				return this.GetView().InsertCommandType;
			}
			set
			{
				this.GetView().InsertCommandType = value;
			}
		}

		// Token: 0x17000CB4 RID: 3252
		// (get) Token: 0x0600387B RID: 14459 RVA: 0x000F04C9 File Offset: 0x000EF4C9
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("SqlDataSource_InsertParameters")]
		[MergableProperty(false)]
		[Editor("System.Web.UI.Design.WebControls.ParameterCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[WebCategory("Data")]
		[DefaultValue(null)]
		public ParameterCollection InsertParameters
		{
			get
			{
				return this.GetView().InsertParameters;
			}
		}

		// Token: 0x17000CB5 RID: 3253
		// (get) Token: 0x0600387C RID: 14460 RVA: 0x000F04D6 File Offset: 0x000EF4D6
		// (set) Token: 0x0600387D RID: 14461 RVA: 0x000F04E3 File Offset: 0x000EF4E3
		[DefaultValue("{0}")]
		[WebCategory("Data")]
		[WebSysDescription("DataSource_OldValuesParameterFormatString")]
		public string OldValuesParameterFormatString
		{
			get
			{
				return this.GetView().OldValuesParameterFormatString;
			}
			set
			{
				this.GetView().OldValuesParameterFormatString = value;
			}
		}

		// Token: 0x17000CB6 RID: 3254
		// (get) Token: 0x0600387E RID: 14462 RVA: 0x000F04F1 File Offset: 0x000EF4F1
		// (set) Token: 0x0600387F RID: 14463 RVA: 0x000F0507 File Offset: 0x000EF507
		[WebSysDescription("SqlDataSource_ProviderName")]
		[DefaultValue("")]
		[TypeConverter("System.Web.UI.Design.WebControls.DataProviderNameConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[WebCategory("Data")]
		public virtual string ProviderName
		{
			get
			{
				if (this._providerName != null)
				{
					return this._providerName;
				}
				return string.Empty;
			}
			set
			{
				if (this.ProviderName != value)
				{
					this._providerFactory = null;
					this._providerName = value;
					this.RaiseDataSourceChangedEvent(EventArgs.Empty);
				}
			}
		}

		// Token: 0x17000CB7 RID: 3255
		// (get) Token: 0x06003880 RID: 14464 RVA: 0x000F0530 File Offset: 0x000EF530
		// (set) Token: 0x06003881 RID: 14465 RVA: 0x000F053D File Offset: 0x000EF53D
		[WebSysDescription("SqlDataSource_SelectCommand")]
		[DefaultValue("")]
		[WebCategory("Data")]
		public string SelectCommand
		{
			get
			{
				return this.GetView().SelectCommand;
			}
			set
			{
				this.GetView().SelectCommand = value;
			}
		}

		// Token: 0x17000CB8 RID: 3256
		// (get) Token: 0x06003882 RID: 14466 RVA: 0x000F054B File Offset: 0x000EF54B
		// (set) Token: 0x06003883 RID: 14467 RVA: 0x000F0558 File Offset: 0x000EF558
		[DefaultValue(SqlDataSourceCommandType.Text)]
		[WebCategory("Data")]
		[WebSysDescription("SqlDataSource_SelectCommandType")]
		public SqlDataSourceCommandType SelectCommandType
		{
			get
			{
				return this.GetView().SelectCommandType;
			}
			set
			{
				this.GetView().SelectCommandType = value;
			}
		}

		// Token: 0x17000CB9 RID: 3257
		// (get) Token: 0x06003884 RID: 14468 RVA: 0x000F0566 File Offset: 0x000EF566
		[WebCategory("Data")]
		[WebSysDescription("SqlDataSource_SelectParameters")]
		[DefaultValue(null)]
		[Editor("System.Web.UI.Design.WebControls.ParameterCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[MergableProperty(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public ParameterCollection SelectParameters
		{
			get
			{
				return this.GetView().SelectParameters;
			}
		}

		// Token: 0x17000CBA RID: 3258
		// (get) Token: 0x06003885 RID: 14469 RVA: 0x000F0573 File Offset: 0x000EF573
		// (set) Token: 0x06003886 RID: 14470 RVA: 0x000F0580 File Offset: 0x000EF580
		[DefaultValue("")]
		[WebSysDescription("SqlDataSource_SortParameterName")]
		[WebCategory("Data")]
		public string SortParameterName
		{
			get
			{
				return this.GetView().SortParameterName;
			}
			set
			{
				this.GetView().SortParameterName = value;
			}
		}

		// Token: 0x17000CBB RID: 3259
		// (get) Token: 0x06003887 RID: 14471 RVA: 0x000F0590 File Offset: 0x000EF590
		private SqlDataSourceCache SqlDataSourceCache
		{
			get
			{
				SqlDataSourceCache sqlDataSourceCache = this.Cache as SqlDataSourceCache;
				if (sqlDataSourceCache == null)
				{
					throw new NotSupportedException(SR.GetString("SqlDataSource_SqlCacheDependencyNotSupported", new object[] { this.ID }));
				}
				return sqlDataSourceCache;
			}
		}

		// Token: 0x17000CBC RID: 3260
		// (get) Token: 0x06003888 RID: 14472 RVA: 0x000F05CE File Offset: 0x000EF5CE
		// (set) Token: 0x06003889 RID: 14473 RVA: 0x000F05DB File Offset: 0x000EF5DB
		[DefaultValue("")]
		[WebCategory("Cache")]
		[WebSysDescription("SqlDataSourceCache_SqlCacheDependency")]
		public virtual string SqlCacheDependency
		{
			get
			{
				return this.SqlDataSourceCache.SqlCacheDependency;
			}
			set
			{
				this.SqlDataSourceCache.SqlCacheDependency = value;
			}
		}

		// Token: 0x17000CBD RID: 3261
		// (get) Token: 0x0600388A RID: 14474 RVA: 0x000F05E9 File Offset: 0x000EF5E9
		// (set) Token: 0x0600388B RID: 14475 RVA: 0x000F05F6 File Offset: 0x000EF5F6
		[WebCategory("Data")]
		[WebSysDescription("SqlDataSource_UpdateCommand")]
		[DefaultValue("")]
		public string UpdateCommand
		{
			get
			{
				return this.GetView().UpdateCommand;
			}
			set
			{
				this.GetView().UpdateCommand = value;
			}
		}

		// Token: 0x17000CBE RID: 3262
		// (get) Token: 0x0600388C RID: 14476 RVA: 0x000F0604 File Offset: 0x000EF604
		// (set) Token: 0x0600388D RID: 14477 RVA: 0x000F0611 File Offset: 0x000EF611
		[WebSysDescription("SqlDataSource_UpdateCommandType")]
		[WebCategory("Data")]
		[DefaultValue(SqlDataSourceCommandType.Text)]
		public SqlDataSourceCommandType UpdateCommandType
		{
			get
			{
				return this.GetView().UpdateCommandType;
			}
			set
			{
				this.GetView().UpdateCommandType = value;
			}
		}

		// Token: 0x17000CBF RID: 3263
		// (get) Token: 0x0600388E RID: 14478 RVA: 0x000F061F File Offset: 0x000EF61F
		[DefaultValue(null)]
		[Editor("System.Web.UI.Design.WebControls.ParameterCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[MergableProperty(false)]
		[WebSysDescription("SqlDataSource_UpdateParameters")]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Data")]
		public ParameterCollection UpdateParameters
		{
			get
			{
				return this.GetView().UpdateParameters;
			}
		}

		// Token: 0x14000050 RID: 80
		// (add) Token: 0x0600388F RID: 14479 RVA: 0x000F062C File Offset: 0x000EF62C
		// (remove) Token: 0x06003890 RID: 14480 RVA: 0x000F063A File Offset: 0x000EF63A
		[WebCategory("Data")]
		[WebSysDescription("DataSource_Deleted")]
		public event SqlDataSourceStatusEventHandler Deleted
		{
			add
			{
				this.GetView().Deleted += value;
			}
			remove
			{
				this.GetView().Deleted -= value;
			}
		}

		// Token: 0x14000051 RID: 81
		// (add) Token: 0x06003891 RID: 14481 RVA: 0x000F0648 File Offset: 0x000EF648
		// (remove) Token: 0x06003892 RID: 14482 RVA: 0x000F0656 File Offset: 0x000EF656
		[WebSysDescription("DataSource_Deleting")]
		[WebCategory("Data")]
		public event SqlDataSourceCommandEventHandler Deleting
		{
			add
			{
				this.GetView().Deleting += value;
			}
			remove
			{
				this.GetView().Deleting -= value;
			}
		}

		// Token: 0x14000052 RID: 82
		// (add) Token: 0x06003893 RID: 14483 RVA: 0x000F0664 File Offset: 0x000EF664
		// (remove) Token: 0x06003894 RID: 14484 RVA: 0x000F0672 File Offset: 0x000EF672
		[WebCategory("Data")]
		[WebSysDescription("DataSource_Filtering")]
		public event SqlDataSourceFilteringEventHandler Filtering
		{
			add
			{
				this.GetView().Filtering += value;
			}
			remove
			{
				this.GetView().Filtering -= value;
			}
		}

		// Token: 0x14000053 RID: 83
		// (add) Token: 0x06003895 RID: 14485 RVA: 0x000F0680 File Offset: 0x000EF680
		// (remove) Token: 0x06003896 RID: 14486 RVA: 0x000F068E File Offset: 0x000EF68E
		[WebCategory("Data")]
		[WebSysDescription("DataSource_Inserted")]
		public event SqlDataSourceStatusEventHandler Inserted
		{
			add
			{
				this.GetView().Inserted += value;
			}
			remove
			{
				this.GetView().Inserted -= value;
			}
		}

		// Token: 0x14000054 RID: 84
		// (add) Token: 0x06003897 RID: 14487 RVA: 0x000F069C File Offset: 0x000EF69C
		// (remove) Token: 0x06003898 RID: 14488 RVA: 0x000F06AA File Offset: 0x000EF6AA
		[WebSysDescription("DataSource_Inserting")]
		[WebCategory("Data")]
		public event SqlDataSourceCommandEventHandler Inserting
		{
			add
			{
				this.GetView().Inserting += value;
			}
			remove
			{
				this.GetView().Inserting -= value;
			}
		}

		// Token: 0x14000055 RID: 85
		// (add) Token: 0x06003899 RID: 14489 RVA: 0x000F06B8 File Offset: 0x000EF6B8
		// (remove) Token: 0x0600389A RID: 14490 RVA: 0x000F06C6 File Offset: 0x000EF6C6
		[WebCategory("Data")]
		[WebSysDescription("SqlDataSource_Selected")]
		public event SqlDataSourceStatusEventHandler Selected
		{
			add
			{
				this.GetView().Selected += value;
			}
			remove
			{
				this.GetView().Selected -= value;
			}
		}

		// Token: 0x14000056 RID: 86
		// (add) Token: 0x0600389B RID: 14491 RVA: 0x000F06D4 File Offset: 0x000EF6D4
		// (remove) Token: 0x0600389C RID: 14492 RVA: 0x000F06E2 File Offset: 0x000EF6E2
		[WebCategory("Data")]
		[WebSysDescription("SqlDataSource_Selecting")]
		public event SqlDataSourceSelectingEventHandler Selecting
		{
			add
			{
				this.GetView().Selecting += value;
			}
			remove
			{
				this.GetView().Selecting -= value;
			}
		}

		// Token: 0x14000057 RID: 87
		// (add) Token: 0x0600389D RID: 14493 RVA: 0x000F06F0 File Offset: 0x000EF6F0
		// (remove) Token: 0x0600389E RID: 14494 RVA: 0x000F06FE File Offset: 0x000EF6FE
		[WebSysDescription("DataSource_Updated")]
		[WebCategory("Data")]
		public event SqlDataSourceStatusEventHandler Updated
		{
			add
			{
				this.GetView().Updated += value;
			}
			remove
			{
				this.GetView().Updated -= value;
			}
		}

		// Token: 0x14000058 RID: 88
		// (add) Token: 0x0600389F RID: 14495 RVA: 0x000F070C File Offset: 0x000EF70C
		// (remove) Token: 0x060038A0 RID: 14496 RVA: 0x000F071A File Offset: 0x000EF71A
		[WebCategory("Data")]
		[WebSysDescription("DataSource_Updating")]
		public event SqlDataSourceCommandEventHandler Updating
		{
			add
			{
				this.GetView().Updating += value;
			}
			remove
			{
				this.GetView().Updating -= value;
			}
		}

		// Token: 0x060038A1 RID: 14497 RVA: 0x000F0728 File Offset: 0x000EF728
		internal string CreateCacheKey(int startRowIndex, int maximumRows)
		{
			StringBuilder stringBuilder = this.CreateRawCacheKey();
			stringBuilder.Append(startRowIndex.ToString(CultureInfo.InvariantCulture));
			stringBuilder.Append(':');
			stringBuilder.Append(maximumRows.ToString(CultureInfo.InvariantCulture));
			return stringBuilder.ToString();
		}

		// Token: 0x060038A2 RID: 14498 RVA: 0x000F0774 File Offset: 0x000EF774
		internal DbConnection CreateConnection(string connectionString)
		{
			DbProviderFactory dbProviderFactorySecure = this.GetDbProviderFactorySecure();
			DbConnection dbConnection = dbProviderFactorySecure.CreateConnection();
			dbConnection.ConnectionString = connectionString;
			return dbConnection;
		}

		// Token: 0x060038A3 RID: 14499 RVA: 0x000F0798 File Offset: 0x000EF798
		internal DbCommand CreateCommand(string commandText, DbConnection connection)
		{
			DbProviderFactory dbProviderFactorySecure = this.GetDbProviderFactorySecure();
			DbCommand dbCommand = dbProviderFactorySecure.CreateCommand();
			dbCommand.CommandText = commandText;
			dbCommand.Connection = connection;
			return dbCommand;
		}

		// Token: 0x060038A4 RID: 14500 RVA: 0x000F07C4 File Offset: 0x000EF7C4
		internal DbDataAdapter CreateDataAdapter(DbCommand command)
		{
			DbProviderFactory dbProviderFactorySecure = this.GetDbProviderFactorySecure();
			DbDataAdapter dbDataAdapter = dbProviderFactorySecure.CreateDataAdapter();
			dbDataAdapter.SelectCommand = command;
			return dbDataAdapter;
		}

		// Token: 0x060038A5 RID: 14501 RVA: 0x000F07E7 File Offset: 0x000EF7E7
		protected virtual SqlDataSourceView CreateDataSourceView(string viewName)
		{
			return new SqlDataSourceView(this, viewName, this.Context);
		}

		// Token: 0x060038A6 RID: 14502 RVA: 0x000F07F6 File Offset: 0x000EF7F6
		internal string CreateMasterCacheKey()
		{
			return this.CreateRawCacheKey().ToString();
		}

		// Token: 0x060038A7 RID: 14503 RVA: 0x000F0804 File Offset: 0x000EF804
		internal DbParameter CreateParameter(string parameterName, object parameterValue)
		{
			DbProviderFactory dbProviderFactorySecure = this.GetDbProviderFactorySecure();
			DbParameter dbParameter = dbProviderFactorySecure.CreateParameter();
			dbParameter.ParameterName = parameterName;
			dbParameter.Value = parameterValue;
			return dbParameter;
		}

		// Token: 0x060038A8 RID: 14504 RVA: 0x000F0830 File Offset: 0x000EF830
		private StringBuilder CreateRawCacheKey()
		{
			StringBuilder stringBuilder = new StringBuilder("u", 1024);
			stringBuilder.Append(base.GetType().GetHashCode().ToString(CultureInfo.InvariantCulture));
			stringBuilder.Append(this.CacheDuration.ToString(CultureInfo.InvariantCulture));
			stringBuilder.Append(':');
			stringBuilder.Append(((int)this.CacheExpirationPolicy).ToString(CultureInfo.InvariantCulture));
			SqlDataSourceCache sqlDataSourceCache = this.Cache as SqlDataSourceCache;
			if (sqlDataSourceCache != null)
			{
				stringBuilder.Append(":");
				stringBuilder.Append(sqlDataSourceCache.SqlCacheDependency);
			}
			stringBuilder.Append(":");
			stringBuilder.Append(this.ConnectionString);
			stringBuilder.Append(":");
			stringBuilder.Append(this.SelectCommand);
			if (this.SelectParameters.Count > 0)
			{
				stringBuilder.Append("?");
				IDictionary values = this.SelectParameters.GetValues(this.Context, this);
				foreach (object obj in values)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					stringBuilder.Append(dictionaryEntry.Key.ToString());
					if (dictionaryEntry.Value != null && dictionaryEntry.Value != DBNull.Value)
					{
						stringBuilder.Append("=");
						stringBuilder.Append(dictionaryEntry.Value.ToString());
					}
					else if (dictionaryEntry.Value == DBNull.Value)
					{
						stringBuilder.Append("(dbnull)");
					}
					else
					{
						stringBuilder.Append("(null)");
					}
					stringBuilder.Append("&");
				}
			}
			return stringBuilder;
		}

		// Token: 0x060038A9 RID: 14505 RVA: 0x000F0A04 File Offset: 0x000EFA04
		public int Delete()
		{
			return this.GetView().Delete(null, null);
		}

		// Token: 0x060038AA RID: 14506 RVA: 0x000F0A14 File Offset: 0x000EFA14
		protected virtual DbProviderFactory GetDbProviderFactory()
		{
			string providerName = this.ProviderName;
			if (string.IsNullOrEmpty(providerName))
			{
				return SqlClientFactory.Instance;
			}
			return DbProviderFactories.GetFactory(providerName);
		}

		// Token: 0x060038AB RID: 14507 RVA: 0x000F0A3C File Offset: 0x000EFA3C
		private DbProviderFactory GetDbProviderFactorySecure()
		{
			if (this._providerFactory == null)
			{
				this._providerFactory = this.GetDbProviderFactory();
				if (!HttpRuntime.ProcessRequestInApplicationTrust && !HttpRuntime.HasDbPermission(this._providerFactory))
				{
					throw new HttpException(SR.GetString("SqlDataSource_NoDbPermission", new object[]
					{
						this._providerFactory.GetType().Name,
						this.ID
					}));
				}
			}
			return this._providerFactory;
		}

		// Token: 0x060038AC RID: 14508 RVA: 0x000F0AAC File Offset: 0x000EFAAC
		private SqlDataSourceView GetView()
		{
			if (this._view == null)
			{
				this._view = this.CreateDataSourceView("DefaultView");
				if (this._cachedSelectCommand != null)
				{
					this._view.SelectCommand = this._cachedSelectCommand;
				}
				if (base.IsTrackingViewState)
				{
					((IStateManager)this._view).TrackViewState();
				}
			}
			return this._view;
		}

		// Token: 0x060038AD RID: 14509 RVA: 0x000F0B04 File Offset: 0x000EFB04
		protected override DataSourceView GetView(string viewName)
		{
			if (viewName == null || (viewName.Length != 0 && !string.Equals(viewName, "DefaultView", StringComparison.OrdinalIgnoreCase)))
			{
				throw new ArgumentException(SR.GetString("DataSource_InvalidViewName", new object[] { this.ID, "DefaultView" }), "viewName");
			}
			return this.GetView();
		}

		// Token: 0x060038AE RID: 14510 RVA: 0x000F0B60 File Offset: 0x000EFB60
		protected override ICollection GetViewNames()
		{
			if (this._viewNames == null)
			{
				this._viewNames = new string[] { "DefaultView" };
			}
			return this._viewNames;
		}

		// Token: 0x060038AF RID: 14511 RVA: 0x000F0B91 File Offset: 0x000EFB91
		public int Insert()
		{
			return this.GetView().Insert(null);
		}

		// Token: 0x060038B0 RID: 14512 RVA: 0x000F0BA0 File Offset: 0x000EFBA0
		internal void InvalidateCacheEntry()
		{
			string text = this.CreateMasterCacheKey();
			DataSourceCache cache = this.Cache;
			cache.Invalidate(text);
		}

		// Token: 0x060038B1 RID: 14513 RVA: 0x000F0BC2 File Offset: 0x000EFBC2
		private void LoadCompleteEventHandler(object sender, EventArgs e)
		{
			this.SelectParameters.UpdateValues(this.Context, this);
			this.FilterParameters.UpdateValues(this.Context, this);
		}

		// Token: 0x060038B2 RID: 14514 RVA: 0x000F0BE8 File Offset: 0x000EFBE8
		internal object LoadDataFromCache(int startRowIndex, int maximumRows)
		{
			string text = this.CreateCacheKey(startRowIndex, maximumRows);
			return this.Cache.LoadDataFromCache(text);
		}

		// Token: 0x060038B3 RID: 14515 RVA: 0x000F0C0C File Offset: 0x000EFC0C
		internal int LoadTotalRowCountFromCache()
		{
			string text = this.CreateMasterCacheKey();
			object obj = this.Cache.LoadDataFromCache(text);
			if (obj is int)
			{
				return (int)obj;
			}
			return -1;
		}

		// Token: 0x060038B4 RID: 14516 RVA: 0x000F0C40 File Offset: 0x000EFC40
		protected override void LoadViewState(object savedState)
		{
			Pair pair = (Pair)savedState;
			if (savedState == null)
			{
				base.LoadViewState(null);
				return;
			}
			base.LoadViewState(pair.First);
			if (pair.Second != null)
			{
				((IStateManager)this.GetView()).LoadViewState(pair.Second);
			}
		}

		// Token: 0x060038B5 RID: 14517 RVA: 0x000F0C84 File Offset: 0x000EFC84
		protected internal override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			if (this.Page != null)
			{
				this.Page.LoadComplete += this.LoadCompleteEventHandler;
			}
		}

		// Token: 0x060038B6 RID: 14518 RVA: 0x000F0CAC File Offset: 0x000EFCAC
		internal virtual void SaveDataToCache(int startRowIndex, int maximumRows, object data, CacheDependency dependency)
		{
			string text = this.CreateCacheKey(startRowIndex, maximumRows);
			string text2 = this.CreateMasterCacheKey();
			if (this.Cache.LoadDataFromCache(text2) == null)
			{
				this.Cache.SaveDataToCache(text2, -1, dependency);
			}
			CacheDependency cacheDependency = new CacheDependency(0, new string[0], new string[] { text2 });
			this.Cache.SaveDataToCache(text, data, cacheDependency);
		}

		// Token: 0x060038B7 RID: 14519 RVA: 0x000F0D14 File Offset: 0x000EFD14
		protected override object SaveViewState()
		{
			Pair pair = new Pair();
			pair.First = base.SaveViewState();
			if (this._view != null)
			{
				pair.Second = ((IStateManager)this._view).SaveViewState();
			}
			if (pair.First == null && pair.Second == null)
			{
				return null;
			}
			return pair;
		}

		// Token: 0x060038B8 RID: 14520 RVA: 0x000F0D5F File Offset: 0x000EFD5F
		public IEnumerable Select(DataSourceSelectArguments arguments)
		{
			return this.GetView().Select(arguments);
		}

		// Token: 0x060038B9 RID: 14521 RVA: 0x000F0D6D File Offset: 0x000EFD6D
		protected override void TrackViewState()
		{
			base.TrackViewState();
			if (this._view != null)
			{
				((IStateManager)this._view).TrackViewState();
			}
		}

		// Token: 0x060038BA RID: 14522 RVA: 0x000F0D88 File Offset: 0x000EFD88
		public int Update()
		{
			return this.GetView().Update(null, null, null);
		}

		// Token: 0x040025DE RID: 9694
		private const string DefaultProviderName = "System.Data.SqlClient";

		// Token: 0x040025DF RID: 9695
		private const string DefaultViewName = "DefaultView";

		// Token: 0x040025E0 RID: 9696
		private DataSourceCache _cache;

		// Token: 0x040025E1 RID: 9697
		private string _cachedSelectCommand;

		// Token: 0x040025E2 RID: 9698
		private string _connectionString;

		// Token: 0x040025E3 RID: 9699
		private SqlDataSourceMode _dataSourceMode = SqlDataSourceMode.DataSet;

		// Token: 0x040025E4 RID: 9700
		private string _providerName;

		// Token: 0x040025E5 RID: 9701
		private DbProviderFactory _providerFactory;

		// Token: 0x040025E6 RID: 9702
		private SqlDataSourceView _view;

		// Token: 0x040025E7 RID: 9703
		private ICollection _viewNames;
	}
}
