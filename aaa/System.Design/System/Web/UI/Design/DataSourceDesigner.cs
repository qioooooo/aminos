using System;
using System.ComponentModel.Design;
using System.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	// Token: 0x02000356 RID: 854
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class DataSourceDesigner : ControlDesigner, IDataSourceDesigner
	{
		// Token: 0x14000027 RID: 39
		// (add) Token: 0x06001FFD RID: 8189 RVA: 0x000B5EF7 File Offset: 0x000B4EF7
		// (remove) Token: 0x06001FFE RID: 8190 RVA: 0x000B5F10 File Offset: 0x000B4F10
		private event EventHandler _dataSourceChangedEvent;

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x06001FFF RID: 8191 RVA: 0x000B5F29 File Offset: 0x000B4F29
		// (remove) Token: 0x06002000 RID: 8192 RVA: 0x000B5F42 File Offset: 0x000B4F42
		private event EventHandler _schemaRefreshedEvent;

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06002001 RID: 8193 RVA: 0x000B5F5C File Offset: 0x000B4F5C
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection designerActionListCollection = new DesignerActionListCollection();
				designerActionListCollection.AddRange(base.ActionLists);
				designerActionListCollection.Add(new DataSourceDesigner.DataSourceDesignerActionList(this));
				return designerActionListCollection;
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06002002 RID: 8194 RVA: 0x000B5F89 File Offset: 0x000B4F89
		public virtual bool CanConfigure
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06002003 RID: 8195 RVA: 0x000B5F8C File Offset: 0x000B4F8C
		public virtual bool CanRefreshSchema
		{
			get
			{
				return false;
			}
		}

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x06002004 RID: 8196 RVA: 0x000B5F8F File Offset: 0x000B4F8F
		// (remove) Token: 0x06002005 RID: 8197 RVA: 0x000B5FA8 File Offset: 0x000B4FA8
		public event EventHandler DataSourceChanged
		{
			add
			{
				this._dataSourceChangedEvent = (EventHandler)Delegate.Combine(this._dataSourceChangedEvent, value);
			}
			remove
			{
				this._dataSourceChangedEvent = (EventHandler)Delegate.Remove(this._dataSourceChangedEvent, value);
			}
		}

		// Token: 0x1400002A RID: 42
		// (add) Token: 0x06002006 RID: 8198 RVA: 0x000B5FC1 File Offset: 0x000B4FC1
		// (remove) Token: 0x06002007 RID: 8199 RVA: 0x000B5FDA File Offset: 0x000B4FDA
		public event EventHandler SchemaRefreshed
		{
			add
			{
				this._schemaRefreshedEvent = (EventHandler)Delegate.Combine(this._schemaRefreshedEvent, value);
			}
			remove
			{
				this._schemaRefreshedEvent = (EventHandler)Delegate.Remove(this._schemaRefreshedEvent, value);
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06002008 RID: 8200 RVA: 0x000B5FF3 File Offset: 0x000B4FF3
		protected bool SuppressingDataSourceEvents
		{
			get
			{
				return this._suppressEventsCount > 0;
			}
		}

		// Token: 0x06002009 RID: 8201 RVA: 0x000B5FFE File Offset: 0x000B4FFE
		public virtual void Configure()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600200A RID: 8202 RVA: 0x000B6005 File Offset: 0x000B5005
		public override string GetDesignTimeHtml()
		{
			return base.CreatePlaceHolderDesignTimeHtml();
		}

		// Token: 0x0600200B RID: 8203 RVA: 0x000B600D File Offset: 0x000B500D
		public virtual DesignerDataSourceView GetView(string viewName)
		{
			return null;
		}

		// Token: 0x0600200C RID: 8204 RVA: 0x000B6010 File Offset: 0x000B5010
		public virtual string[] GetViewNames()
		{
			return new string[0];
		}

		// Token: 0x0600200D RID: 8205 RVA: 0x000B6018 File Offset: 0x000B5018
		protected virtual void OnDataSourceChanged(EventArgs e)
		{
			if (this.SuppressingDataSourceEvents)
			{
				this._raiseDataSourceChangedEvent = true;
				return;
			}
			if (this._dataSourceChangedEvent != null)
			{
				this._dataSourceChangedEvent(this, e);
			}
			this._raiseDataSourceChangedEvent = false;
		}

		// Token: 0x0600200E RID: 8206 RVA: 0x000B6046 File Offset: 0x000B5046
		protected virtual void OnSchemaRefreshed(EventArgs e)
		{
			if (this.SuppressingDataSourceEvents)
			{
				this._raiseSchemaRefreshedEvent = true;
				return;
			}
			if (this._schemaRefreshedEvent != null)
			{
				this._schemaRefreshedEvent(this, e);
			}
			this._raiseSchemaRefreshedEvent = false;
		}

		// Token: 0x0600200F RID: 8207 RVA: 0x000B6074 File Offset: 0x000B5074
		public virtual void RefreshSchema(bool preferSilent)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06002010 RID: 8208 RVA: 0x000B607C File Offset: 0x000B507C
		public virtual void ResumeDataSourceEvents()
		{
			if (this._suppressEventsCount == 0)
			{
				throw new InvalidOperationException(SR.GetString("DataSource_CannotResumeEvents"));
			}
			this._suppressEventsCount--;
			if (this._suppressEventsCount == 0)
			{
				if (this._raiseDataSourceChangedEvent)
				{
					this.OnDataSourceChanged(EventArgs.Empty);
				}
				if (this._raiseSchemaRefreshedEvent)
				{
					this.OnSchemaRefreshed(EventArgs.Empty);
				}
			}
		}

		// Token: 0x06002011 RID: 8209 RVA: 0x000B60DD File Offset: 0x000B50DD
		public virtual void SuppressDataSourceEvents()
		{
			this._suppressEventsCount++;
		}

		// Token: 0x06002012 RID: 8210 RVA: 0x000B60F0 File Offset: 0x000B50F0
		public static bool SchemasEquivalent(IDataSourceSchema schema1, IDataSourceSchema schema2)
		{
			if ((schema1 == null) ^ (schema2 == null))
			{
				return false;
			}
			if (schema1 == null && schema2 == null)
			{
				return true;
			}
			IDataSourceViewSchema[] views = schema1.GetViews();
			IDataSourceViewSchema[] views2 = schema2.GetViews();
			if ((views == null) ^ (views2 == null))
			{
				return false;
			}
			if (views == null && views2 == null)
			{
				return true;
			}
			int num = views.Length;
			int num2 = views2.Length;
			if (num != num2)
			{
				return false;
			}
			foreach (IDataSourceViewSchema dataSourceViewSchema in views)
			{
				bool flag = false;
				string name = dataSourceViewSchema.Name;
				foreach (IDataSourceViewSchema dataSourceViewSchema2 in views2)
				{
					if (name == dataSourceViewSchema2.Name && DataSourceDesigner.ViewSchemasEquivalent(dataSourceViewSchema, dataSourceViewSchema2))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002013 RID: 8211 RVA: 0x000B61B8 File Offset: 0x000B51B8
		public static bool ViewSchemasEquivalent(IDataSourceViewSchema viewSchema1, IDataSourceViewSchema viewSchema2)
		{
			if ((viewSchema1 == null) ^ (viewSchema2 == null))
			{
				return false;
			}
			if (viewSchema1 == null && viewSchema2 == null)
			{
				return true;
			}
			IDataSourceFieldSchema[] fields = viewSchema1.GetFields();
			IDataSourceFieldSchema[] fields2 = viewSchema2.GetFields();
			if ((fields == null) ^ (fields2 == null))
			{
				return false;
			}
			if (fields == null && fields2 == null)
			{
				return true;
			}
			int num = fields.Length;
			int num2 = fields2.Length;
			if (num != num2)
			{
				return false;
			}
			foreach (IDataSourceFieldSchema dataSourceFieldSchema in fields)
			{
				bool flag = false;
				string name = dataSourceFieldSchema.Name;
				Type dataType = dataSourceFieldSchema.DataType;
				foreach (IDataSourceFieldSchema dataSourceFieldSchema2 in fields2)
				{
					if (name == dataSourceFieldSchema2.Name && dataType == dataSourceFieldSchema2.DataType)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040017C5 RID: 6085
		private int _suppressEventsCount;

		// Token: 0x040017C6 RID: 6086
		private bool _raiseDataSourceChangedEvent;

		// Token: 0x040017C7 RID: 6087
		private bool _raiseSchemaRefreshedEvent;

		// Token: 0x02000357 RID: 855
		private class DataSourceDesignerActionList : DesignerActionList
		{
			// Token: 0x06002015 RID: 8213 RVA: 0x000B628F File Offset: 0x000B528F
			public DataSourceDesignerActionList(DataSourceDesigner parent)
				: base(parent.Component)
			{
				this._parent = parent;
			}

			// Token: 0x170005A2 RID: 1442
			// (get) Token: 0x06002016 RID: 8214 RVA: 0x000B62A4 File Offset: 0x000B52A4
			// (set) Token: 0x06002017 RID: 8215 RVA: 0x000B62A7 File Offset: 0x000B52A7
			public override bool AutoShow
			{
				get
				{
					return true;
				}
				set
				{
				}
			}

			// Token: 0x06002018 RID: 8216 RVA: 0x000B62A9 File Offset: 0x000B52A9
			public void Configure()
			{
				this._parent.Configure();
			}

			// Token: 0x06002019 RID: 8217 RVA: 0x000B62B6 File Offset: 0x000B52B6
			public void RefreshSchema()
			{
				this._parent.RefreshSchema(false);
			}

			// Token: 0x0600201A RID: 8218 RVA: 0x000B62C4 File Offset: 0x000B52C4
			public override DesignerActionItemCollection GetSortedActionItems()
			{
				DesignerActionItemCollection designerActionItemCollection = new DesignerActionItemCollection();
				if (this._parent.CanConfigure)
				{
					designerActionItemCollection.Add(new DesignerActionMethodItem(this, "Configure", SR.GetString("DataSourceDesigner_ConfigureDataSourceVerb"), SR.GetString("DataSourceDesigner_DataActionGroup"), SR.GetString("DataSourceDesigner_ConfigureDataSourceVerbDesc"), true)
					{
						AllowAssociate = true
					});
				}
				if (this._parent.CanRefreshSchema)
				{
					designerActionItemCollection.Add(new DesignerActionMethodItem(this, "RefreshSchema", SR.GetString("DataSourceDesigner_RefreshSchemaVerb"), SR.GetString("DataSourceDesigner_DataActionGroup"), SR.GetString("DataSourceDesigner_RefreshSchemaVerbDesc"), false)
					{
						AllowAssociate = true
					});
				}
				return designerActionItemCollection;
			}

			// Token: 0x040017C8 RID: 6088
			private DataSourceDesigner _parent;
		}
	}
}
