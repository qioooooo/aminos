using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x020003E2 RID: 994
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class DataSourceView
	{
		// Token: 0x06003039 RID: 12345 RVA: 0x000D5118 File Offset: 0x000D4118
		protected DataSourceView(IDataSource owner, string viewName)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			if (viewName == null)
			{
				throw new ArgumentNullException("viewName");
			}
			this._name = viewName;
			DataSourceControl dataSourceControl = owner as DataSourceControl;
			if (dataSourceControl != null)
			{
				dataSourceControl.DataSourceChangedInternal += this.OnDataSourceChangedInternal;
				return;
			}
			owner.DataSourceChanged += this.OnDataSourceChangedInternal;
		}

		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x0600303A RID: 12346 RVA: 0x000D517D File Offset: 0x000D417D
		public virtual bool CanDelete
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x0600303B RID: 12347 RVA: 0x000D5180 File Offset: 0x000D4180
		public virtual bool CanInsert
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x0600303C RID: 12348 RVA: 0x000D5183 File Offset: 0x000D4183
		public virtual bool CanPage
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x0600303D RID: 12349 RVA: 0x000D5186 File Offset: 0x000D4186
		public virtual bool CanRetrieveTotalRowCount
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A7A RID: 2682
		// (get) Token: 0x0600303E RID: 12350 RVA: 0x000D5189 File Offset: 0x000D4189
		public virtual bool CanSort
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A7B RID: 2683
		// (get) Token: 0x0600303F RID: 12351 RVA: 0x000D518C File Offset: 0x000D418C
		public virtual bool CanUpdate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A7C RID: 2684
		// (get) Token: 0x06003040 RID: 12352 RVA: 0x000D518F File Offset: 0x000D418F
		protected EventHandlerList Events
		{
			get
			{
				if (this._events == null)
				{
					this._events = new EventHandlerList();
				}
				return this._events;
			}
		}

		// Token: 0x17000A7D RID: 2685
		// (get) Token: 0x06003041 RID: 12353 RVA: 0x000D51AA File Offset: 0x000D41AA
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x14000037 RID: 55
		// (add) Token: 0x06003042 RID: 12354 RVA: 0x000D51B2 File Offset: 0x000D41B2
		// (remove) Token: 0x06003043 RID: 12355 RVA: 0x000D51C5 File Offset: 0x000D41C5
		public event EventHandler DataSourceViewChanged
		{
			add
			{
				this.Events.AddHandler(DataSourceView.EventDataSourceViewChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(DataSourceView.EventDataSourceViewChanged, value);
			}
		}

		// Token: 0x06003044 RID: 12356 RVA: 0x000D51D8 File Offset: 0x000D41D8
		public virtual void Delete(IDictionary keys, IDictionary oldValues, DataSourceViewOperationCallback callback)
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			int num = 0;
			bool flag = false;
			try
			{
				num = this.ExecuteDelete(keys, oldValues);
			}
			catch (Exception ex)
			{
				flag = true;
				if (!callback(num, ex))
				{
					throw;
				}
			}
			finally
			{
				if (!flag)
				{
					callback(num, null);
				}
			}
		}

		// Token: 0x06003045 RID: 12357 RVA: 0x000D5240 File Offset: 0x000D4240
		protected virtual int ExecuteDelete(IDictionary keys, IDictionary oldValues)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06003046 RID: 12358 RVA: 0x000D5247 File Offset: 0x000D4247
		protected virtual int ExecuteInsert(IDictionary values)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06003047 RID: 12359
		protected internal abstract IEnumerable ExecuteSelect(DataSourceSelectArguments arguments);

		// Token: 0x06003048 RID: 12360 RVA: 0x000D524E File Offset: 0x000D424E
		protected virtual int ExecuteUpdate(IDictionary keys, IDictionary values, IDictionary oldValues)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06003049 RID: 12361 RVA: 0x000D5255 File Offset: 0x000D4255
		private void OnDataSourceChangedInternal(object sender, EventArgs e)
		{
			this.OnDataSourceViewChanged(e);
		}

		// Token: 0x0600304A RID: 12362 RVA: 0x000D5260 File Offset: 0x000D4260
		protected virtual void OnDataSourceViewChanged(EventArgs e)
		{
			EventHandler eventHandler = this.Events[DataSourceView.EventDataSourceViewChanged] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600304B RID: 12363 RVA: 0x000D5290 File Offset: 0x000D4290
		public virtual void Insert(IDictionary values, DataSourceViewOperationCallback callback)
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			int num = 0;
			bool flag = false;
			try
			{
				num = this.ExecuteInsert(values);
			}
			catch (Exception ex)
			{
				flag = true;
				if (!callback(num, ex))
				{
					throw;
				}
			}
			finally
			{
				if (!flag)
				{
					callback(num, null);
				}
			}
		}

		// Token: 0x0600304C RID: 12364 RVA: 0x000D52F8 File Offset: 0x000D42F8
		protected internal virtual void RaiseUnsupportedCapabilityError(DataSourceCapabilities capability)
		{
			if (!this.CanPage && (capability & DataSourceCapabilities.Page) != DataSourceCapabilities.None)
			{
				throw new NotSupportedException(SR.GetString("DataSourceView_NoPaging"));
			}
			if (!this.CanSort && (capability & DataSourceCapabilities.Sort) != DataSourceCapabilities.None)
			{
				throw new NotSupportedException(SR.GetString("DataSourceView_NoSorting"));
			}
			if (!this.CanRetrieveTotalRowCount && (capability & DataSourceCapabilities.RetrieveTotalRowCount) != DataSourceCapabilities.None)
			{
				throw new NotSupportedException(SR.GetString("DataSourceView_NoRowCount"));
			}
		}

		// Token: 0x0600304D RID: 12365 RVA: 0x000D535C File Offset: 0x000D435C
		public virtual void Select(DataSourceSelectArguments arguments, DataSourceViewSelectCallback callback)
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			callback(this.ExecuteSelect(arguments));
		}

		// Token: 0x0600304E RID: 12366 RVA: 0x000D537C File Offset: 0x000D437C
		public virtual void Update(IDictionary keys, IDictionary values, IDictionary oldValues, DataSourceViewOperationCallback callback)
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			int num = 0;
			bool flag = false;
			try
			{
				num = this.ExecuteUpdate(keys, values, oldValues);
			}
			catch (Exception ex)
			{
				flag = true;
				if (!callback(num, ex))
				{
					throw;
				}
			}
			finally
			{
				if (!flag)
				{
					callback(num, null);
				}
			}
		}

		// Token: 0x04002213 RID: 8723
		private static readonly object EventDataSourceViewChanged = new object();

		// Token: 0x04002214 RID: 8724
		private EventHandlerList _events;

		// Token: 0x04002215 RID: 8725
		private string _name;
	}
}
