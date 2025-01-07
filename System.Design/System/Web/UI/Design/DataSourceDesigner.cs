using System;
using System.ComponentModel.Design;
using System.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class DataSourceDesigner : ControlDesigner, IDataSourceDesigner
	{
		private event EventHandler _dataSourceChangedEvent;

		private event EventHandler _schemaRefreshedEvent;

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

		public virtual bool CanConfigure
		{
			get
			{
				return false;
			}
		}

		public virtual bool CanRefreshSchema
		{
			get
			{
				return false;
			}
		}

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

		protected bool SuppressingDataSourceEvents
		{
			get
			{
				return this._suppressEventsCount > 0;
			}
		}

		public virtual void Configure()
		{
			throw new NotSupportedException();
		}

		public override string GetDesignTimeHtml()
		{
			return base.CreatePlaceHolderDesignTimeHtml();
		}

		public virtual DesignerDataSourceView GetView(string viewName)
		{
			return null;
		}

		public virtual string[] GetViewNames()
		{
			return new string[0];
		}

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

		public virtual void RefreshSchema(bool preferSilent)
		{
			throw new NotSupportedException();
		}

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

		public virtual void SuppressDataSourceEvents()
		{
			this._suppressEventsCount++;
		}

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

		private int _suppressEventsCount;

		private bool _raiseDataSourceChangedEvent;

		private bool _raiseSchemaRefreshedEvent;

		private class DataSourceDesignerActionList : DesignerActionList
		{
			public DataSourceDesignerActionList(DataSourceDesigner parent)
				: base(parent.Component)
			{
				this._parent = parent;
			}

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

			public void Configure()
			{
				this._parent.Configure();
			}

			public void RefreshSchema()
			{
				this._parent.RefreshSchema(false);
			}

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

			private DataSourceDesigner _parent;
		}
	}
}
