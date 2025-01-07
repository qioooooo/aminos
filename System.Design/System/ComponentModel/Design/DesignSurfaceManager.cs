using System;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class DesignSurfaceManager : IServiceProvider, IDisposable
	{
		public DesignSurfaceManager()
			: this(null)
		{
		}

		public DesignSurfaceManager(IServiceProvider parentProvider)
		{
			this._parentProvider = parentProvider;
			ServiceCreatorCallback serviceCreatorCallback = new ServiceCreatorCallback(this.OnCreateService);
			this.ServiceContainer.AddService(typeof(IDesignerEventService), serviceCreatorCallback);
		}

		public virtual DesignSurface ActiveDesignSurface
		{
			get
			{
				IDesignerEventService eventService = this.EventService;
				if (eventService != null)
				{
					IDesignerHost activeDesigner = eventService.ActiveDesigner;
					if (activeDesigner != null)
					{
						return activeDesigner.GetService(typeof(DesignSurface)) as DesignSurface;
					}
				}
				return null;
			}
			set
			{
				DesignerEventService designerEventService = this.EventService as DesignerEventService;
				if (designerEventService != null)
				{
					designerEventService.OnActivateDesigner(value);
				}
			}
		}

		public DesignSurfaceCollection DesignSurfaces
		{
			get
			{
				IDesignerEventService eventService = this.EventService;
				if (eventService != null)
				{
					return new DesignSurfaceCollection(eventService.Designers);
				}
				return new DesignSurfaceCollection(null);
			}
		}

		private IDesignerEventService EventService
		{
			get
			{
				return this.GetService(typeof(IDesignerEventService)) as IDesignerEventService;
			}
		}

		protected ServiceContainer ServiceContainer
		{
			get
			{
				if (this._serviceContainer == null)
				{
					this._serviceContainer = new ServiceContainer(this._parentProvider);
				}
				return this._serviceContainer;
			}
		}

		public event ActiveDesignSurfaceChangedEventHandler ActiveDesignSurfaceChanged
		{
			add
			{
				if (this._activeDesignSurfaceChanged == null)
				{
					IDesignerEventService eventService = this.EventService;
					if (eventService != null)
					{
						eventService.ActiveDesignerChanged += this.OnActiveDesignerChanged;
					}
				}
				this._activeDesignSurfaceChanged = (ActiveDesignSurfaceChangedEventHandler)Delegate.Combine(this._activeDesignSurfaceChanged, value);
			}
			remove
			{
				this._activeDesignSurfaceChanged = (ActiveDesignSurfaceChangedEventHandler)Delegate.Remove(this._activeDesignSurfaceChanged, value);
				if (this._activeDesignSurfaceChanged == null)
				{
					IDesignerEventService eventService = this.EventService;
					if (eventService != null)
					{
						eventService.ActiveDesignerChanged -= this.OnActiveDesignerChanged;
					}
				}
			}
		}

		public event DesignSurfaceEventHandler DesignSurfaceCreated
		{
			add
			{
				if (this._designSurfaceCreated == null)
				{
					IDesignerEventService eventService = this.EventService;
					if (eventService != null)
					{
						eventService.DesignerCreated += this.OnDesignerCreated;
					}
				}
				this._designSurfaceCreated = (DesignSurfaceEventHandler)Delegate.Combine(this._designSurfaceCreated, value);
			}
			remove
			{
				this._designSurfaceCreated = (DesignSurfaceEventHandler)Delegate.Remove(this._designSurfaceCreated, value);
				if (this._designSurfaceCreated == null)
				{
					IDesignerEventService eventService = this.EventService;
					if (eventService != null)
					{
						eventService.DesignerCreated -= this.OnDesignerCreated;
					}
				}
			}
		}

		public event DesignSurfaceEventHandler DesignSurfaceDisposed
		{
			add
			{
				if (this._designSurfaceDisposed == null)
				{
					IDesignerEventService eventService = this.EventService;
					if (eventService != null)
					{
						eventService.DesignerDisposed += this.OnDesignerDisposed;
					}
				}
				this._designSurfaceDisposed = (DesignSurfaceEventHandler)Delegate.Combine(this._designSurfaceDisposed, value);
			}
			remove
			{
				this._designSurfaceDisposed = (DesignSurfaceEventHandler)Delegate.Remove(this._designSurfaceDisposed, value);
				if (this._designSurfaceDisposed == null)
				{
					IDesignerEventService eventService = this.EventService;
					if (eventService != null)
					{
						eventService.DesignerDisposed -= this.OnDesignerDisposed;
					}
				}
			}
		}

		public event EventHandler SelectionChanged
		{
			add
			{
				if (this._selectionChanged == null)
				{
					IDesignerEventService eventService = this.EventService;
					if (eventService != null)
					{
						eventService.SelectionChanged += this.OnSelectionChanged;
					}
				}
				this._selectionChanged = (EventHandler)Delegate.Combine(this._selectionChanged, value);
			}
			remove
			{
				this._selectionChanged = (EventHandler)Delegate.Remove(this._selectionChanged, value);
				if (this._selectionChanged == null)
				{
					IDesignerEventService eventService = this.EventService;
					if (eventService != null)
					{
						eventService.SelectionChanged -= this.OnSelectionChanged;
					}
				}
			}
		}

		public DesignSurface CreateDesignSurface()
		{
			DesignSurface designSurface = this.CreateDesignSurfaceCore(this);
			DesignerEventService designerEventService = this.GetService(typeof(IDesignerEventService)) as DesignerEventService;
			if (designerEventService != null)
			{
				designerEventService.OnCreateDesigner(designSurface);
			}
			return designSurface;
		}

		public DesignSurface CreateDesignSurface(IServiceProvider parentProvider)
		{
			if (parentProvider == null)
			{
				throw new ArgumentNullException("parentProvider");
			}
			IServiceProvider serviceProvider = new DesignSurfaceManager.MergedServiceProvider(parentProvider, this);
			DesignSurface designSurface = this.CreateDesignSurfaceCore(serviceProvider);
			DesignerEventService designerEventService = this.GetService(typeof(IDesignerEventService)) as DesignerEventService;
			if (designerEventService != null)
			{
				designerEventService.OnCreateDesigner(designSurface);
			}
			return designSurface;
		}

		protected virtual DesignSurface CreateDesignSurfaceCore(IServiceProvider parentProvider)
		{
			return new DesignSurface(parentProvider);
		}

		public void Dispose()
		{
			this.Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this._serviceContainer != null)
			{
				this._serviceContainer.Dispose();
				this._serviceContainer = null;
			}
		}

		public object GetService(Type serviceType)
		{
			if (this._serviceContainer != null)
			{
				return this._serviceContainer.GetService(serviceType);
			}
			return null;
		}

		private object OnCreateService(IServiceContainer container, Type serviceType)
		{
			if (serviceType == typeof(IDesignerEventService))
			{
				return new DesignerEventService();
			}
			return null;
		}

		private void OnActiveDesignerChanged(object sender, ActiveDesignerEventArgs e)
		{
			if (this._activeDesignSurfaceChanged != null)
			{
				DesignSurface designSurface = null;
				DesignSurface designSurface2 = null;
				if (e.OldDesigner != null)
				{
					designSurface2 = e.OldDesigner.GetService(typeof(DesignSurface)) as DesignSurface;
				}
				if (e.NewDesigner != null)
				{
					designSurface = e.NewDesigner.GetService(typeof(DesignSurface)) as DesignSurface;
				}
				this._activeDesignSurfaceChanged(this, new ActiveDesignSurfaceChangedEventArgs(designSurface2, designSurface));
			}
		}

		private void OnDesignerCreated(object sender, DesignerEventArgs e)
		{
			if (this._designSurfaceCreated != null)
			{
				DesignSurface designSurface = e.Designer.GetService(typeof(DesignSurface)) as DesignSurface;
				if (designSurface != null)
				{
					this._designSurfaceCreated(this, new DesignSurfaceEventArgs(designSurface));
				}
			}
		}

		private void OnDesignerDisposed(object sender, DesignerEventArgs e)
		{
			if (this._designSurfaceDisposed != null)
			{
				DesignSurface designSurface = e.Designer.GetService(typeof(DesignSurface)) as DesignSurface;
				if (designSurface != null)
				{
					this._designSurfaceDisposed(this, new DesignSurfaceEventArgs(designSurface));
				}
			}
		}

		private void OnSelectionChanged(object sender, EventArgs e)
		{
			if (this._selectionChanged != null)
			{
				this._selectionChanged(this, e);
			}
		}

		private IServiceProvider _parentProvider;

		private ServiceContainer _serviceContainer;

		private ActiveDesignSurfaceChangedEventHandler _activeDesignSurfaceChanged;

		private DesignSurfaceEventHandler _designSurfaceCreated;

		private DesignSurfaceEventHandler _designSurfaceDisposed;

		private EventHandler _selectionChanged;

		private sealed class MergedServiceProvider : IServiceProvider
		{
			internal MergedServiceProvider(IServiceProvider primaryProvider, IServiceProvider secondaryProvider)
			{
				this._primaryProvider = primaryProvider;
				this._secondaryProvider = secondaryProvider;
			}

			object IServiceProvider.GetService(Type serviceType)
			{
				if (serviceType == null)
				{
					throw new ArgumentNullException("serviceType");
				}
				object obj = this._primaryProvider.GetService(serviceType);
				if (obj == null)
				{
					obj = this._secondaryProvider.GetService(serviceType);
				}
				return obj;
			}

			private IServiceProvider _primaryProvider;

			private IServiceProvider _secondaryProvider;
		}
	}
}
