using System;
using System.ComponentModel.Design;
using System.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	// Token: 0x0200036F RID: 879
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class HierarchicalDataSourceDesigner : ControlDesigner, IHierarchicalDataSourceDesigner
	{
		// Token: 0x1400002D RID: 45
		// (add) Token: 0x060020E9 RID: 8425 RVA: 0x000B8B49 File Offset: 0x000B7B49
		// (remove) Token: 0x060020EA RID: 8426 RVA: 0x000B8B62 File Offset: 0x000B7B62
		private event EventHandler _dataSourceChangedEvent;

		// Token: 0x1400002E RID: 46
		// (add) Token: 0x060020EB RID: 8427 RVA: 0x000B8B7B File Offset: 0x000B7B7B
		// (remove) Token: 0x060020EC RID: 8428 RVA: 0x000B8B94 File Offset: 0x000B7B94
		private event EventHandler _schemaRefreshedEvent;

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x060020ED RID: 8429 RVA: 0x000B8BB0 File Offset: 0x000B7BB0
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection designerActionListCollection = new DesignerActionListCollection();
				designerActionListCollection.AddRange(base.ActionLists);
				designerActionListCollection.Add(new HierarchicalDataSourceDesigner.HierarchicalDataSourceDesignerActionList(this));
				return designerActionListCollection;
			}
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x060020EE RID: 8430 RVA: 0x000B8BDD File Offset: 0x000B7BDD
		public virtual bool CanConfigure
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x060020EF RID: 8431 RVA: 0x000B8BE0 File Offset: 0x000B7BE0
		public virtual bool CanRefreshSchema
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1400002F RID: 47
		// (add) Token: 0x060020F0 RID: 8432 RVA: 0x000B8BE3 File Offset: 0x000B7BE3
		// (remove) Token: 0x060020F1 RID: 8433 RVA: 0x000B8BFC File Offset: 0x000B7BFC
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

		// Token: 0x14000030 RID: 48
		// (add) Token: 0x060020F2 RID: 8434 RVA: 0x000B8C15 File Offset: 0x000B7C15
		// (remove) Token: 0x060020F3 RID: 8435 RVA: 0x000B8C2E File Offset: 0x000B7C2E
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

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x060020F4 RID: 8436 RVA: 0x000B8C47 File Offset: 0x000B7C47
		protected bool SuppressingDataSourceEvents
		{
			get
			{
				return this._suppressEventsCount > 0;
			}
		}

		// Token: 0x060020F5 RID: 8437 RVA: 0x000B8C52 File Offset: 0x000B7C52
		public virtual void Configure()
		{
			throw new NotSupportedException();
		}

		// Token: 0x060020F6 RID: 8438 RVA: 0x000B8C59 File Offset: 0x000B7C59
		public override string GetDesignTimeHtml()
		{
			return base.CreatePlaceHolderDesignTimeHtml();
		}

		// Token: 0x060020F7 RID: 8439 RVA: 0x000B8C61 File Offset: 0x000B7C61
		public virtual DesignerHierarchicalDataSourceView GetView(string viewPath)
		{
			return null;
		}

		// Token: 0x060020F8 RID: 8440 RVA: 0x000B8C64 File Offset: 0x000B7C64
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

		// Token: 0x060020F9 RID: 8441 RVA: 0x000B8C92 File Offset: 0x000B7C92
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

		// Token: 0x060020FA RID: 8442 RVA: 0x000B8CC0 File Offset: 0x000B7CC0
		public virtual void RefreshSchema(bool preferSilent)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060020FB RID: 8443 RVA: 0x000B8CC8 File Offset: 0x000B7CC8
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

		// Token: 0x060020FC RID: 8444 RVA: 0x000B8D29 File Offset: 0x000B7D29
		public virtual void SuppressDataSourceEvents()
		{
			this._suppressEventsCount++;
		}

		// Token: 0x0400180D RID: 6157
		private int _suppressEventsCount;

		// Token: 0x0400180E RID: 6158
		private bool _raiseDataSourceChangedEvent;

		// Token: 0x0400180F RID: 6159
		private bool _raiseSchemaRefreshedEvent;

		// Token: 0x02000370 RID: 880
		private class HierarchicalDataSourceDesignerActionList : DesignerActionList
		{
			// Token: 0x060020FE RID: 8446 RVA: 0x000B8D41 File Offset: 0x000B7D41
			public HierarchicalDataSourceDesignerActionList(HierarchicalDataSourceDesigner parent)
				: base(parent.Component)
			{
				this._parent = parent;
			}

			// Token: 0x170005ED RID: 1517
			// (get) Token: 0x060020FF RID: 8447 RVA: 0x000B8D56 File Offset: 0x000B7D56
			// (set) Token: 0x06002100 RID: 8448 RVA: 0x000B8D59 File Offset: 0x000B7D59
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

			// Token: 0x06002101 RID: 8449 RVA: 0x000B8D5B File Offset: 0x000B7D5B
			public void Configure()
			{
				this._parent.Configure();
			}

			// Token: 0x06002102 RID: 8450 RVA: 0x000B8D68 File Offset: 0x000B7D68
			public void RefreshSchema()
			{
				this._parent.RefreshSchema(false);
			}

			// Token: 0x06002103 RID: 8451 RVA: 0x000B8D78 File Offset: 0x000B7D78
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

			// Token: 0x04001810 RID: 6160
			private HierarchicalDataSourceDesigner _parent;
		}
	}
}
