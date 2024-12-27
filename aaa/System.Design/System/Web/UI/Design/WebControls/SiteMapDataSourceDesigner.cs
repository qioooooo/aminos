using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020004A9 RID: 1193
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class SiteMapDataSourceDesigner : HierarchicalDataSourceDesigner, IDataSourceDesigner
	{
		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x06002B39 RID: 11065 RVA: 0x000EF48C File Offset: 0x000EE48C
		public override bool CanRefreshSchema
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x06002B3A RID: 11066 RVA: 0x000EF490 File Offset: 0x000EE490
		internal SiteMapProvider DesignTimeSiteMapProvider
		{
			get
			{
				if (this._siteMapProvider == null)
				{
					IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
					this._siteMapProvider = new DesignTimeSiteMapProvider(designerHost);
				}
				return this._siteMapProvider;
			}
		}

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x06002B3B RID: 11067 RVA: 0x000EF4CD File Offset: 0x000EE4CD
		internal SiteMapDataSource SiteMapDataSource
		{
			get
			{
				return this._siteMapDataSource;
			}
		}

		// Token: 0x06002B3C RID: 11068 RVA: 0x000EF4D5 File Offset: 0x000EE4D5
		public override DesignerHierarchicalDataSourceView GetView(string viewPath)
		{
			return new SiteMapDesignerHierarchicalDataSourceView(this, viewPath);
		}

		// Token: 0x06002B3D RID: 11069 RVA: 0x000EF4DE File Offset: 0x000EE4DE
		public virtual string[] GetViewNames()
		{
			return new string[0];
		}

		// Token: 0x06002B3E RID: 11070 RVA: 0x000EF4E6 File Offset: 0x000EE4E6
		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(SiteMapDataSource));
			base.Initialize(component);
			this._siteMapDataSource = (SiteMapDataSource)component;
		}

		// Token: 0x06002B3F RID: 11071 RVA: 0x000EF50B File Offset: 0x000EE50B
		public override void OnComponentChanged(object sender, ComponentChangedEventArgs e)
		{
			base.OnComponentChanged(sender, e);
			this.OnDataSourceChanged(EventArgs.Empty);
		}

		// Token: 0x06002B40 RID: 11072 RVA: 0x000EF520 File Offset: 0x000EE520
		public override void RefreshSchema(bool preferSilent)
		{
			try
			{
				this.SuppressDataSourceEvents();
				this._siteMapProvider = null;
				this.OnDataSourceChanged(EventArgs.Empty);
			}
			finally
			{
				this.ResumeDataSourceEvents();
			}
		}

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x06002B41 RID: 11073 RVA: 0x000EF560 File Offset: 0x000EE560
		bool IDataSourceDesigner.CanConfigure
		{
			get
			{
				return this.CanConfigure;
			}
		}

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x06002B42 RID: 11074 RVA: 0x000EF568 File Offset: 0x000EE568
		bool IDataSourceDesigner.CanRefreshSchema
		{
			get
			{
				return this.CanRefreshSchema;
			}
		}

		// Token: 0x14000040 RID: 64
		// (add) Token: 0x06002B43 RID: 11075 RVA: 0x000EF570 File Offset: 0x000EE570
		// (remove) Token: 0x06002B44 RID: 11076 RVA: 0x000EF579 File Offset: 0x000EE579
		event EventHandler IDataSourceDesigner.DataSourceChanged
		{
			add
			{
				base.DataSourceChanged += value;
			}
			remove
			{
				base.DataSourceChanged -= value;
			}
		}

		// Token: 0x14000041 RID: 65
		// (add) Token: 0x06002B45 RID: 11077 RVA: 0x000EF582 File Offset: 0x000EE582
		// (remove) Token: 0x06002B46 RID: 11078 RVA: 0x000EF58B File Offset: 0x000EE58B
		event EventHandler IDataSourceDesigner.SchemaRefreshed
		{
			add
			{
				base.SchemaRefreshed += value;
			}
			remove
			{
				base.SchemaRefreshed -= value;
			}
		}

		// Token: 0x06002B47 RID: 11079 RVA: 0x000EF594 File Offset: 0x000EE594
		void IDataSourceDesigner.Configure()
		{
			this.Configure();
		}

		// Token: 0x06002B48 RID: 11080 RVA: 0x000EF59C File Offset: 0x000EE59C
		DesignerDataSourceView IDataSourceDesigner.GetView(string viewName)
		{
			return new SiteMapDesignerDataSourceView(this, viewName);
		}

		// Token: 0x06002B49 RID: 11081 RVA: 0x000EF5A5 File Offset: 0x000EE5A5
		string[] IDataSourceDesigner.GetViewNames()
		{
			return this.GetViewNames();
		}

		// Token: 0x06002B4A RID: 11082 RVA: 0x000EF5AD File Offset: 0x000EE5AD
		void IDataSourceDesigner.RefreshSchema(bool preferSilent)
		{
			this.RefreshSchema(preferSilent);
		}

		// Token: 0x06002B4B RID: 11083 RVA: 0x000EF5B6 File Offset: 0x000EE5B6
		void IDataSourceDesigner.ResumeDataSourceEvents()
		{
			this.ResumeDataSourceEvents();
		}

		// Token: 0x06002B4C RID: 11084 RVA: 0x000EF5BE File Offset: 0x000EE5BE
		void IDataSourceDesigner.SuppressDataSourceEvents()
		{
			this.SuppressDataSourceEvents();
		}

		// Token: 0x04001D72 RID: 7538
		internal static readonly SiteMapDataSourceDesigner.SiteMapSchema SiteMapHierarchicalSchema = new SiteMapDataSourceDesigner.SiteMapSchema();

		// Token: 0x04001D73 RID: 7539
		private SiteMapDataSource _siteMapDataSource;

		// Token: 0x04001D74 RID: 7540
		private SiteMapProvider _siteMapProvider;

		// Token: 0x04001D75 RID: 7541
		private static readonly string _siteMapNodeType = typeof(SiteMapNode).Name;

		// Token: 0x020004AA RID: 1194
		internal class SiteMapSchema : IDataSourceSchema
		{
			// Token: 0x06002B4F RID: 11087 RVA: 0x000EF5F0 File Offset: 0x000EE5F0
			IDataSourceViewSchema[] IDataSourceSchema.GetViews()
			{
				return new SiteMapDataSourceDesigner.SiteMapDataSourceViewSchema[]
				{
					new SiteMapDataSourceDesigner.SiteMapDataSourceViewSchema()
				};
			}
		}

		// Token: 0x020004AB RID: 1195
		internal class SiteMapDataSourceViewSchema : IDataSourceViewSchema
		{
			// Token: 0x17000812 RID: 2066
			// (get) Token: 0x06002B51 RID: 11089 RVA: 0x000EF615 File Offset: 0x000EE615
			string IDataSourceViewSchema.Name
			{
				get
				{
					return SiteMapDataSourceDesigner._siteMapNodeType;
				}
			}

			// Token: 0x06002B52 RID: 11090 RVA: 0x000EF61C File Offset: 0x000EE61C
			IDataSourceViewSchema[] IDataSourceViewSchema.GetChildren()
			{
				return null;
			}

			// Token: 0x06002B53 RID: 11091 RVA: 0x000EF620 File Offset: 0x000EE620
			IDataSourceFieldSchema[] IDataSourceViewSchema.GetFields()
			{
				return new SiteMapDataSourceDesigner.SiteMapDataSourceTextField[]
				{
					SiteMapDataSourceDesigner.SiteMapDataSourceTextField.DescriptionField,
					SiteMapDataSourceDesigner.SiteMapDataSourceTextField.TitleField,
					SiteMapDataSourceDesigner.SiteMapDataSourceTextField.UrlField
				};
			}
		}

		// Token: 0x020004AC RID: 1196
		private class SiteMapDataSourceTextField : IDataSourceFieldSchema
		{
			// Token: 0x06002B55 RID: 11093 RVA: 0x000EF655 File Offset: 0x000EE655
			internal SiteMapDataSourceTextField(string fieldName)
			{
				this._fieldName = fieldName;
			}

			// Token: 0x17000813 RID: 2067
			// (get) Token: 0x06002B56 RID: 11094 RVA: 0x000EF664 File Offset: 0x000EE664
			Type IDataSourceFieldSchema.DataType
			{
				get
				{
					return typeof(string);
				}
			}

			// Token: 0x17000814 RID: 2068
			// (get) Token: 0x06002B57 RID: 11095 RVA: 0x000EF670 File Offset: 0x000EE670
			bool IDataSourceFieldSchema.Identity
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000815 RID: 2069
			// (get) Token: 0x06002B58 RID: 11096 RVA: 0x000EF673 File Offset: 0x000EE673
			bool IDataSourceFieldSchema.IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000816 RID: 2070
			// (get) Token: 0x06002B59 RID: 11097 RVA: 0x000EF676 File Offset: 0x000EE676
			bool IDataSourceFieldSchema.IsUnique
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000817 RID: 2071
			// (get) Token: 0x06002B5A RID: 11098 RVA: 0x000EF679 File Offset: 0x000EE679
			int IDataSourceFieldSchema.Length
			{
				get
				{
					return -1;
				}
			}

			// Token: 0x17000818 RID: 2072
			// (get) Token: 0x06002B5B RID: 11099 RVA: 0x000EF67C File Offset: 0x000EE67C
			string IDataSourceFieldSchema.Name
			{
				get
				{
					return this._fieldName;
				}
			}

			// Token: 0x17000819 RID: 2073
			// (get) Token: 0x06002B5C RID: 11100 RVA: 0x000EF684 File Offset: 0x000EE684
			bool IDataSourceFieldSchema.Nullable
			{
				get
				{
					return true;
				}
			}

			// Token: 0x1700081A RID: 2074
			// (get) Token: 0x06002B5D RID: 11101 RVA: 0x000EF687 File Offset: 0x000EE687
			int IDataSourceFieldSchema.Precision
			{
				get
				{
					return -1;
				}
			}

			// Token: 0x1700081B RID: 2075
			// (get) Token: 0x06002B5E RID: 11102 RVA: 0x000EF68A File Offset: 0x000EE68A
			bool IDataSourceFieldSchema.PrimaryKey
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700081C RID: 2076
			// (get) Token: 0x06002B5F RID: 11103 RVA: 0x000EF68D File Offset: 0x000EE68D
			int IDataSourceFieldSchema.Scale
			{
				get
				{
					return -1;
				}
			}

			// Token: 0x04001D76 RID: 7542
			internal static readonly SiteMapDataSourceDesigner.SiteMapDataSourceTextField DescriptionField = new SiteMapDataSourceDesigner.SiteMapDataSourceTextField("Description");

			// Token: 0x04001D77 RID: 7543
			internal static readonly SiteMapDataSourceDesigner.SiteMapDataSourceTextField TitleField = new SiteMapDataSourceDesigner.SiteMapDataSourceTextField("Title");

			// Token: 0x04001D78 RID: 7544
			internal static readonly SiteMapDataSourceDesigner.SiteMapDataSourceTextField UrlField = new SiteMapDataSourceDesigner.SiteMapDataSourceTextField("Url");

			// Token: 0x04001D79 RID: 7545
			private string _fieldName;
		}
	}
}
