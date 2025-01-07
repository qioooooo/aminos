using System;
using System.Collections;

namespace System.ComponentModel.Design
{
	internal sealed class DesignerEventService : IDesignerEventService
	{
		internal DesignerEventService()
		{
		}

		internal void OnActivateDesigner(DesignSurface surface)
		{
			IDesignerHost designerHost = null;
			if (surface != null)
			{
				designerHost = surface.GetService(typeof(IDesignerHost)) as IDesignerHost;
			}
			if (designerHost != null && (this._designerList == null || !this._designerList.Contains(designerHost)))
			{
				this.OnCreateDesigner(surface);
			}
			if (this._activeDesigner != designerHost)
			{
				IDesignerHost activeDesigner = this._activeDesigner;
				this._activeDesigner = designerHost;
				if (activeDesigner != null)
				{
					this.SinkChangeEvents(activeDesigner, false);
				}
				if (this._activeDesigner != null)
				{
					this.SinkChangeEvents(this._activeDesigner, true);
				}
				if (this._events != null)
				{
					ActiveDesignerEventHandler activeDesignerEventHandler = this._events[DesignerEventService.EventActiveDesignerChanged] as ActiveDesignerEventHandler;
					if (activeDesignerEventHandler != null)
					{
						activeDesignerEventHandler(this, new ActiveDesignerEventArgs(activeDesigner, designerHost));
					}
				}
				this.OnSelectionChanged(this, EventArgs.Empty);
			}
		}

		private void OnComponentAddedRemoved(object sender, ComponentEventArgs ce)
		{
			IComponent component = ce.Component;
			if (component != null)
			{
				ISite site = component.Site;
				if (site != null)
				{
					IDesignerHost designerHost = site.Container as IDesignerHost;
					if (designerHost != null && designerHost.Loading)
					{
						this._deferredSelChange = true;
						return;
					}
				}
			}
			this.OnSelectionChanged(this, EventArgs.Empty);
		}

		private void OnComponentChanged(object sender, ComponentChangedEventArgs ce)
		{
			IComponent component = ce.Component as IComponent;
			if (component != null)
			{
				ISite site = component.Site;
				if (site != null)
				{
					ISelectionService selectionService = site.GetService(typeof(ISelectionService)) as ISelectionService;
					if (selectionService != null && selectionService.GetComponentSelected(component))
					{
						this.OnSelectionChanged(this, EventArgs.Empty);
					}
				}
			}
		}

		internal void OnCreateDesigner(DesignSurface surface)
		{
			IDesignerHost designerHost = surface.GetService(typeof(IDesignerHost)) as IDesignerHost;
			if (this._designerList == null)
			{
				this._designerList = new ArrayList();
			}
			this._designerList.Add(designerHost);
			surface.Disposed += this.OnDesignerDisposed;
			if (this._events != null)
			{
				DesignerEventHandler designerEventHandler = this._events[DesignerEventService.EventDesignerCreated] as DesignerEventHandler;
				if (designerEventHandler != null)
				{
					designerEventHandler(this, new DesignerEventArgs(designerHost));
				}
			}
		}

		private void OnDesignerDisposed(object sender, EventArgs e)
		{
			DesignSurface designSurface = (DesignSurface)sender;
			designSurface.Disposed -= this.OnDesignerDisposed;
			this.SinkChangeEvents(designSurface, false);
			IDesignerHost designerHost = designSurface.GetService(typeof(IDesignerHost)) as IDesignerHost;
			if (designerHost != null)
			{
				if (this._events != null)
				{
					DesignerEventHandler designerEventHandler = this._events[DesignerEventService.EventDesignerDisposed] as DesignerEventHandler;
					if (designerEventHandler != null)
					{
						designerEventHandler(this, new DesignerEventArgs(designerHost));
					}
				}
				if (this._designerList != null)
				{
					this._designerList.Remove(designerHost);
				}
			}
		}

		private void OnSelectionChanged(object sender, EventArgs e)
		{
			if (this._inTransaction)
			{
				this._deferredSelChange = true;
				return;
			}
			if (this._events != null)
			{
				EventHandler eventHandler = this._events[DesignerEventService.EventSelectionChanged] as EventHandler;
				if (eventHandler != null)
				{
					eventHandler(this, e);
				}
			}
		}

		private void OnLoadComplete(object sender, EventArgs e)
		{
			if (this._deferredSelChange)
			{
				this._deferredSelChange = false;
				this.OnSelectionChanged(this, EventArgs.Empty);
			}
		}

		private void OnTransactionClosed(object sender, DesignerTransactionCloseEventArgs e)
		{
			if (e.LastTransaction)
			{
				this._inTransaction = false;
				if (this._deferredSelChange)
				{
					this._deferredSelChange = false;
					this.OnSelectionChanged(this, EventArgs.Empty);
				}
			}
		}

		private void OnTransactionOpened(object sender, EventArgs e)
		{
			this._inTransaction = true;
		}

		private void SinkChangeEvents(IServiceProvider provider, bool sink)
		{
			ISelectionService selectionService = provider.GetService(typeof(ISelectionService)) as ISelectionService;
			IComponentChangeService componentChangeService = provider.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			IDesignerHost designerHost = provider.GetService(typeof(IDesignerHost)) as IDesignerHost;
			if (sink)
			{
				if (selectionService != null)
				{
					selectionService.SelectionChanged += this.OnSelectionChanged;
				}
				if (componentChangeService != null)
				{
					ComponentEventHandler componentEventHandler = new ComponentEventHandler(this.OnComponentAddedRemoved);
					componentChangeService.ComponentAdded += componentEventHandler;
					componentChangeService.ComponentRemoved += componentEventHandler;
					componentChangeService.ComponentChanged += this.OnComponentChanged;
				}
				if (designerHost != null)
				{
					designerHost.TransactionOpened += this.OnTransactionOpened;
					designerHost.TransactionClosed += this.OnTransactionClosed;
					designerHost.LoadComplete += this.OnLoadComplete;
					if (designerHost.InTransaction)
					{
						this.OnTransactionOpened(designerHost, EventArgs.Empty);
						return;
					}
				}
			}
			else
			{
				if (selectionService != null)
				{
					selectionService.SelectionChanged -= this.OnSelectionChanged;
				}
				if (componentChangeService != null)
				{
					ComponentEventHandler componentEventHandler2 = new ComponentEventHandler(this.OnComponentAddedRemoved);
					componentChangeService.ComponentAdded -= componentEventHandler2;
					componentChangeService.ComponentRemoved -= componentEventHandler2;
					componentChangeService.ComponentChanged -= this.OnComponentChanged;
				}
				if (designerHost != null)
				{
					designerHost.TransactionOpened -= this.OnTransactionOpened;
					designerHost.TransactionClosed -= this.OnTransactionClosed;
					designerHost.LoadComplete -= this.OnLoadComplete;
					if (designerHost.InTransaction)
					{
						this.OnTransactionClosed(designerHost, new DesignerTransactionCloseEventArgs(false, true));
					}
				}
			}
		}

		IDesignerHost IDesignerEventService.ActiveDesigner
		{
			get
			{
				return this._activeDesigner;
			}
		}

		DesignerCollection IDesignerEventService.Designers
		{
			get
			{
				if (this._designerList == null)
				{
					this._designerList = new ArrayList();
				}
				if (this._designerCollection == null)
				{
					this._designerCollection = new DesignerCollection(this._designerList);
				}
				return this._designerCollection;
			}
		}

		event ActiveDesignerEventHandler IDesignerEventService.ActiveDesignerChanged
		{
			add
			{
				if (this._events == null)
				{
					this._events = new EventHandlerList();
				}
				this._events[DesignerEventService.EventActiveDesignerChanged] = Delegate.Combine(this._events[DesignerEventService.EventActiveDesignerChanged], value);
			}
			remove
			{
				if (this._events != null)
				{
					this._events[DesignerEventService.EventActiveDesignerChanged] = Delegate.Remove(this._events[DesignerEventService.EventActiveDesignerChanged], value);
				}
			}
		}

		event DesignerEventHandler IDesignerEventService.DesignerCreated
		{
			add
			{
				if (this._events == null)
				{
					this._events = new EventHandlerList();
				}
				this._events[DesignerEventService.EventDesignerCreated] = Delegate.Combine(this._events[DesignerEventService.EventDesignerCreated], value);
			}
			remove
			{
				if (this._events != null)
				{
					this._events[DesignerEventService.EventDesignerCreated] = Delegate.Remove(this._events[DesignerEventService.EventDesignerCreated], value);
				}
			}
		}

		event DesignerEventHandler IDesignerEventService.DesignerDisposed
		{
			add
			{
				if (this._events == null)
				{
					this._events = new EventHandlerList();
				}
				this._events[DesignerEventService.EventDesignerDisposed] = Delegate.Combine(this._events[DesignerEventService.EventDesignerDisposed], value);
			}
			remove
			{
				if (this._events != null)
				{
					this._events[DesignerEventService.EventDesignerDisposed] = Delegate.Remove(this._events[DesignerEventService.EventDesignerDisposed], value);
				}
			}
		}

		event EventHandler IDesignerEventService.SelectionChanged
		{
			add
			{
				if (this._events == null)
				{
					this._events = new EventHandlerList();
				}
				this._events[DesignerEventService.EventSelectionChanged] = Delegate.Combine(this._events[DesignerEventService.EventSelectionChanged], value);
			}
			remove
			{
				if (this._events != null)
				{
					this._events[DesignerEventService.EventSelectionChanged] = Delegate.Remove(this._events[DesignerEventService.EventSelectionChanged], value);
				}
			}
		}

		private static readonly object EventActiveDesignerChanged = new object();

		private static readonly object EventDesignerCreated = new object();

		private static readonly object EventDesignerDisposed = new object();

		private static readonly object EventSelectionChanged = new object();

		private ArrayList _designerList;

		private DesignerCollection _designerCollection;

		private IDesignerHost _activeDesigner;

		private EventHandlerList _events;

		private bool _inTransaction;

		private bool _deferredSelChange;
	}
}
