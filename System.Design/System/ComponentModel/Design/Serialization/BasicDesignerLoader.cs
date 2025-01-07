using System;
using System.Collections;
using System.Collections.Specialized;
using System.Design;
using System.Reflection;
using System.Security.Permissions;
using System.Windows.Forms;

namespace System.ComponentModel.Design.Serialization
{
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public abstract class BasicDesignerLoader : DesignerLoader, IDesignerLoaderService
	{
		protected BasicDesignerLoader()
		{
			this._state[BasicDesignerLoader.StateFlushInProgress] = false;
			this._state[BasicDesignerLoader.StateReloadSupported] = true;
			this._state[BasicDesignerLoader.StateEnableComponentEvents] = false;
			this._hostInitialized = false;
			this._loading = false;
		}

		protected virtual bool Modified
		{
			get
			{
				return this._state[BasicDesignerLoader.StateModified];
			}
			set
			{
				this._state[BasicDesignerLoader.StateModified] = value;
			}
		}

		protected IDesignerLoaderHost LoaderHost
		{
			get
			{
				if (this._host != null)
				{
					return this._host;
				}
				if (this._hostInitialized)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				throw new InvalidOperationException(SR.GetString("BasicDesignerLoaderNotInitialized"));
			}
		}

		public override bool Loading
		{
			get
			{
				return this._loadDependencyCount > 0 || this._loading;
			}
		}

		protected object PropertyProvider
		{
			get
			{
				if (this._serializationManager == null)
				{
					throw new InvalidOperationException(SR.GetString("BasicDesignerLoaderNotInitialized"));
				}
				return this._serializationManager.PropertyProvider;
			}
			set
			{
				if (this._serializationManager == null)
				{
					throw new InvalidOperationException(SR.GetString("BasicDesignerLoaderNotInitialized"));
				}
				this._serializationManager.PropertyProvider = value;
			}
		}

		protected bool ReloadPending
		{
			get
			{
				return this._state[BasicDesignerLoader.StateReloadAtIdle];
			}
		}

		public override void BeginLoad(IDesignerLoaderHost host)
		{
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			if (this._state[BasicDesignerLoader.StateLoaded])
			{
				throw new InvalidOperationException(SR.GetString("BasicDesignerLoaderAlreadyLoaded"))
				{
					HelpLink = "BasicDesignerLoaderAlreadyLoaded"
				};
			}
			if (this._host != null && this._host != host)
			{
				throw new InvalidOperationException(SR.GetString("BasicDesignerLoaderDifferentHost"))
				{
					HelpLink = "BasicDesignerLoaderDifferentHost"
				};
			}
			this._state[BasicDesignerLoader.StateLoaded | BasicDesignerLoader.StateLoadFailed] = false;
			this._loadDependencyCount = 0;
			if (this._host == null)
			{
				this._host = host;
				this._hostInitialized = true;
				this._serializationManager = new DesignerSerializationManager(this._host);
				DesignSurfaceServiceContainer designSurfaceServiceContainer = this.GetService(typeof(DesignSurfaceServiceContainer)) as DesignSurfaceServiceContainer;
				if (designSurfaceServiceContainer != null)
				{
					designSurfaceServiceContainer.AddFixedService(typeof(IDesignerSerializationManager), this._serializationManager);
				}
				else
				{
					IServiceContainer serviceContainer = this.GetService(typeof(IServiceContainer)) as IServiceContainer;
					if (serviceContainer == null)
					{
						this.ThrowMissingService(typeof(IServiceContainer));
					}
					serviceContainer.AddService(typeof(IDesignerSerializationManager), this._serializationManager);
				}
				this.Initialize();
				host.Activated += this.OnDesignerActivate;
				host.Deactivated += this.OnDesignerDeactivate;
			}
			bool flag = true;
			ArrayList arrayList = null;
			IDesignerLoaderService designerLoaderService = this.GetService(typeof(IDesignerLoaderService)) as IDesignerLoaderService;
			try
			{
				if (designerLoaderService != null)
				{
					designerLoaderService.AddLoadDependency();
				}
				else
				{
					this._loading = true;
					this.OnBeginLoad();
				}
				this.PerformLoad(this._serializationManager);
			}
			catch (Exception innerException)
			{
				while (innerException is TargetInvocationException)
				{
					innerException = innerException.InnerException;
				}
				arrayList = new ArrayList();
				arrayList.Add(innerException);
				flag = false;
			}
			if (designerLoaderService != null)
			{
				designerLoaderService.DependentLoadComplete(flag, arrayList);
				return;
			}
			this.OnEndLoad(flag, arrayList);
			this._loading = false;
		}

		public override void Dispose()
		{
			if (this._state[BasicDesignerLoader.StateReloadAtIdle])
			{
				Application.Idle -= this.OnIdle;
			}
			this.UnloadDocument();
			IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentAdded -= this.OnComponentAdded;
				componentChangeService.ComponentAdding -= this.OnComponentAdding;
				componentChangeService.ComponentRemoving -= this.OnComponentRemoving;
				componentChangeService.ComponentRemoved -= this.OnComponentRemoved;
				componentChangeService.ComponentChanged -= this.OnComponentChanged;
				componentChangeService.ComponentChanging -= this.OnComponentChanging;
				componentChangeService.ComponentRename -= this.OnComponentRename;
			}
			if (this._host != null)
			{
				this._host.RemoveService(typeof(IDesignerLoaderService));
				this._host.Activated -= this.OnDesignerActivate;
				this._host.Deactivated -= this.OnDesignerDeactivate;
				this._host = null;
			}
		}

		public override void Flush()
		{
			if (this._state[BasicDesignerLoader.StateFlushInProgress] || !this._state[BasicDesignerLoader.StateLoaded] || !this.Modified)
			{
				return;
			}
			this._state[BasicDesignerLoader.StateFlushInProgress] = true;
			Cursor cursor = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				IDesignerLoaderHost host = this._host;
				bool flag = true;
				if (host != null && host.RootComponent != null)
				{
					using (this._serializationManager.CreateSession())
					{
						try
						{
							this.PerformFlush(this._serializationManager);
						}
						catch (CheckoutException)
						{
							flag = false;
							throw;
						}
						catch (Exception ex)
						{
							this._serializationManager.Errors.Add(ex);
						}
						ICollection errors = this._serializationManager.Errors;
						if (errors != null && errors.Count > 0)
						{
							this.ReportFlushErrors(errors);
						}
					}
				}
				if (flag)
				{
					this.Modified = false;
				}
			}
			finally
			{
				this._state[BasicDesignerLoader.StateFlushInProgress] = false;
				Cursor.Current = cursor;
			}
		}

		protected object GetService(Type serviceType)
		{
			object obj = null;
			if (this._host != null)
			{
				obj = this._host.GetService(serviceType);
			}
			return obj;
		}

		protected virtual void Initialize()
		{
			this.LoaderHost.AddService(typeof(IDesignerLoaderService), this);
		}

		protected virtual bool IsReloadNeeded()
		{
			return true;
		}

		protected virtual void OnBeginLoad()
		{
			this._serializationSession = this._serializationManager.CreateSession();
			this._state[BasicDesignerLoader.StateLoaded] = false;
			this.EnableComponentNotification(false);
			IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentAdded -= this.OnComponentAdded;
				componentChangeService.ComponentAdding -= this.OnComponentAdding;
				componentChangeService.ComponentRemoving -= this.OnComponentRemoving;
				componentChangeService.ComponentRemoved -= this.OnComponentRemoved;
				componentChangeService.ComponentChanged -= this.OnComponentChanged;
				componentChangeService.ComponentChanging -= this.OnComponentChanging;
				componentChangeService.ComponentRename -= this.OnComponentRename;
			}
		}

		protected virtual bool EnableComponentNotification(bool enable)
		{
			bool flag = this._state[BasicDesignerLoader.StateEnableComponentEvents];
			if (!flag && enable)
			{
				this._state[BasicDesignerLoader.StateEnableComponentEvents] = true;
			}
			else if (flag && !enable)
			{
				this._state[BasicDesignerLoader.StateEnableComponentEvents] = false;
			}
			return flag;
		}

		protected virtual void OnBeginUnload()
		{
		}

		private void OnComponentAdded(object sender, ComponentEventArgs e)
		{
			if (this._state[BasicDesignerLoader.StateEnableComponentEvents] && !this.LoaderHost.Loading)
			{
				this.Modified = true;
			}
		}

		private void OnComponentAdding(object sender, ComponentEventArgs e)
		{
			if (this._state[BasicDesignerLoader.StateEnableComponentEvents] && !this.LoaderHost.Loading)
			{
				this.OnModifying();
			}
		}

		private void OnComponentChanged(object sender, ComponentChangedEventArgs e)
		{
			if (this._state[BasicDesignerLoader.StateEnableComponentEvents] && !this.LoaderHost.Loading)
			{
				this.Modified = true;
			}
		}

		private void OnComponentChanging(object sender, ComponentChangingEventArgs e)
		{
			if (this._state[BasicDesignerLoader.StateEnableComponentEvents] && !this.LoaderHost.Loading)
			{
				this.OnModifying();
			}
		}

		private void OnComponentRemoved(object sender, ComponentEventArgs e)
		{
			if (this._state[BasicDesignerLoader.StateEnableComponentEvents] && !this.LoaderHost.Loading)
			{
				this.Modified = true;
			}
		}

		private void OnComponentRemoving(object sender, ComponentEventArgs e)
		{
			if (this._state[BasicDesignerLoader.StateEnableComponentEvents] && !this.LoaderHost.Loading)
			{
				this.OnModifying();
			}
		}

		private void OnComponentRename(object sender, ComponentRenameEventArgs e)
		{
			if (this._state[BasicDesignerLoader.StateEnableComponentEvents] && !this.LoaderHost.Loading)
			{
				this.OnModifying();
				this.Modified = true;
			}
		}

		private void OnDesignerActivate(object sender, EventArgs e)
		{
			this._state[BasicDesignerLoader.StateActiveDocument] = true;
			if (this._state[BasicDesignerLoader.StateDeferredReload] && this._host != null)
			{
				this._state[BasicDesignerLoader.StateDeferredReload] = false;
				BasicDesignerLoader.ReloadOptions reloadOptions = BasicDesignerLoader.ReloadOptions.Default;
				if (this._state[BasicDesignerLoader.StateForceReload])
				{
					reloadOptions |= BasicDesignerLoader.ReloadOptions.Force;
				}
				if (!this._state[BasicDesignerLoader.StateFlushReload])
				{
					reloadOptions |= BasicDesignerLoader.ReloadOptions.NoFlush;
				}
				if (this._state[BasicDesignerLoader.StateModifyIfErrors])
				{
					reloadOptions |= BasicDesignerLoader.ReloadOptions.ModifyOnError;
				}
				this.Reload(reloadOptions);
			}
		}

		private void OnDesignerDeactivate(object sender, EventArgs e)
		{
			this._state[BasicDesignerLoader.StateActiveDocument] = false;
		}

		protected virtual void OnEndLoad(bool successful, ICollection errors)
		{
			successful = successful && (errors == null || errors.Count == 0) && (this._serializationManager.Errors == null || this._serializationManager.Errors.Count == 0);
			try
			{
				this._state[BasicDesignerLoader.StateLoaded] = true;
				IDesignerLoaderHost2 designerLoaderHost = this.GetService(typeof(IDesignerLoaderHost2)) as IDesignerLoaderHost2;
				if (!successful && (designerLoaderHost == null || !designerLoaderHost.IgnoreErrorsDuringReload))
				{
					if (designerLoaderHost != null)
					{
						designerLoaderHost.CanReloadWithErrors = this.LoaderHost.RootComponent != null;
					}
					this.UnloadDocument();
				}
				else
				{
					successful = true;
				}
				if (errors != null)
				{
					foreach (object obj in errors)
					{
						this._serializationManager.Errors.Add(obj);
					}
				}
				errors = this._serializationManager.Errors;
			}
			finally
			{
				this._serializationSession.Dispose();
				this._serializationSession = null;
			}
			if (successful)
			{
				IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
				if (componentChangeService != null)
				{
					componentChangeService.ComponentAdded += this.OnComponentAdded;
					componentChangeService.ComponentAdding += this.OnComponentAdding;
					componentChangeService.ComponentRemoving += this.OnComponentRemoving;
					componentChangeService.ComponentRemoved += this.OnComponentRemoved;
					componentChangeService.ComponentChanged += this.OnComponentChanged;
					componentChangeService.ComponentChanging += this.OnComponentChanging;
					componentChangeService.ComponentRename += this.OnComponentRename;
				}
				this.EnableComponentNotification(true);
			}
			this.LoaderHost.EndLoad(this._baseComponentClassName, successful, errors);
			if (this._state[BasicDesignerLoader.StateModifyIfErrors] && errors != null && errors.Count > 0)
			{
				try
				{
					this.OnModifying();
					this.Modified = true;
				}
				catch (CheckoutException ex)
				{
					if (ex != CheckoutException.Canceled)
					{
						throw;
					}
				}
			}
		}

		protected virtual void OnModifying()
		{
		}

		private void OnIdle(object sender, EventArgs e)
		{
			Application.Idle -= this.OnIdle;
			if (this._state[BasicDesignerLoader.StateReloadAtIdle])
			{
				this._state[BasicDesignerLoader.StateReloadAtIdle] = false;
				DesignSurfaceManager designSurfaceManager = (DesignSurfaceManager)this.GetService(typeof(DesignSurfaceManager));
				DesignSurface designSurface = (DesignSurface)this.GetService(typeof(DesignSurface));
				if (designSurfaceManager != null && designSurface != null && !object.ReferenceEquals(designSurfaceManager.ActiveDesignSurface, designSurface))
				{
					this._state[BasicDesignerLoader.StateActiveDocument] = false;
					this._state[BasicDesignerLoader.StateDeferredReload] = true;
					return;
				}
				IDesignerLoaderHost loaderHost = this.LoaderHost;
				if (loaderHost != null)
				{
					if (!this._state[BasicDesignerLoader.StateForceReload])
					{
						if (!this.IsReloadNeeded())
						{
							return;
						}
					}
					try
					{
						if (this._state[BasicDesignerLoader.StateFlushReload])
						{
							this.Flush();
						}
						this.UnloadDocument();
						loaderHost.Reload();
					}
					finally
					{
						this._state[BasicDesignerLoader.StateForceReload | BasicDesignerLoader.StateModifyIfErrors | BasicDesignerLoader.StateFlushReload] = false;
					}
				}
			}
		}

		protected abstract void PerformFlush(IDesignerSerializationManager serializationManager);

		protected abstract void PerformLoad(IDesignerSerializationManager serializationManager);

		protected void Reload(BasicDesignerLoader.ReloadOptions flags)
		{
			this._state[BasicDesignerLoader.StateForceReload] = (flags & BasicDesignerLoader.ReloadOptions.Force) != BasicDesignerLoader.ReloadOptions.Default;
			this._state[BasicDesignerLoader.StateFlushReload] = (flags & BasicDesignerLoader.ReloadOptions.NoFlush) == BasicDesignerLoader.ReloadOptions.Default;
			this._state[BasicDesignerLoader.StateModifyIfErrors] = (flags & BasicDesignerLoader.ReloadOptions.ModifyOnError) != BasicDesignerLoader.ReloadOptions.Default;
			if (!this._state[BasicDesignerLoader.StateFlushInProgress])
			{
				if (this._state[BasicDesignerLoader.StateActiveDocument])
				{
					if (!this._state[BasicDesignerLoader.StateReloadAtIdle])
					{
						Application.Idle += this.OnIdle;
						this._state[BasicDesignerLoader.StateReloadAtIdle] = true;
						return;
					}
				}
				else
				{
					this._state[BasicDesignerLoader.StateDeferredReload] = true;
				}
			}
		}

		protected virtual void ReportFlushErrors(ICollection errors)
		{
			object obj = null;
			foreach (object obj2 in errors)
			{
				obj = obj2;
			}
			if (obj != null)
			{
				Exception ex = obj as Exception;
				if (ex == null)
				{
					ex = new InvalidOperationException(obj.ToString());
				}
				throw ex;
			}
		}

		protected void SetBaseComponentClassName(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this._baseComponentClassName = name;
		}

		private void ThrowMissingService(Type serviceType)
		{
			throw new InvalidOperationException(SR.GetString("BasicDesignerLoaderMissingService", new object[] { serviceType.Name }))
			{
				HelpLink = "BasicDesignerLoaderMissingService"
			};
		}

		private void UnloadDocument()
		{
			this.OnBeginUnload();
			this._state[BasicDesignerLoader.StateLoaded] = false;
			this._baseComponentClassName = null;
		}

		void IDesignerLoaderService.AddLoadDependency()
		{
			if (this._serializationManager == null)
			{
				throw new InvalidOperationException();
			}
			if (this._loadDependencyCount++ == 0)
			{
				this.OnBeginLoad();
			}
		}

		void IDesignerLoaderService.DependentLoadComplete(bool successful, ICollection errorCollection)
		{
			if (this._loadDependencyCount == 0)
			{
				throw new InvalidOperationException();
			}
			if (!successful)
			{
				this._state[BasicDesignerLoader.StateLoadFailed] = true;
			}
			if (--this._loadDependencyCount == 0)
			{
				this.OnEndLoad(!this._state[BasicDesignerLoader.StateLoadFailed], errorCollection);
				return;
			}
			if (errorCollection != null)
			{
				foreach (object obj in errorCollection)
				{
					this._serializationManager.Errors.Add(obj);
				}
			}
		}

		bool IDesignerLoaderService.Reload()
		{
			if (this._state[BasicDesignerLoader.StateReloadSupported] && this._loadDependencyCount == 0)
			{
				this.Reload(BasicDesignerLoader.ReloadOptions.Force);
				return true;
			}
			return false;
		}

		private static readonly int StateLoaded = BitVector32.CreateMask();

		private static readonly int StateLoadFailed = BitVector32.CreateMask(BasicDesignerLoader.StateLoaded);

		private static readonly int StateFlushInProgress = BitVector32.CreateMask(BasicDesignerLoader.StateLoadFailed);

		private static readonly int StateModified = BitVector32.CreateMask(BasicDesignerLoader.StateFlushInProgress);

		private static readonly int StateReloadSupported = BitVector32.CreateMask(BasicDesignerLoader.StateModified);

		private static readonly int StateActiveDocument = BitVector32.CreateMask(BasicDesignerLoader.StateReloadSupported);

		private static readonly int StateDeferredReload = BitVector32.CreateMask(BasicDesignerLoader.StateActiveDocument);

		private static readonly int StateReloadAtIdle = BitVector32.CreateMask(BasicDesignerLoader.StateDeferredReload);

		private static readonly int StateForceReload = BitVector32.CreateMask(BasicDesignerLoader.StateReloadAtIdle);

		private static readonly int StateFlushReload = BitVector32.CreateMask(BasicDesignerLoader.StateForceReload);

		private static readonly int StateModifyIfErrors = BitVector32.CreateMask(BasicDesignerLoader.StateFlushReload);

		private static readonly int StateEnableComponentEvents = BitVector32.CreateMask(BasicDesignerLoader.StateModifyIfErrors);

		private BitVector32 _state = default(BitVector32);

		private IDesignerLoaderHost _host;

		private int _loadDependencyCount;

		private string _baseComponentClassName;

		private bool _hostInitialized;

		private bool _loading;

		private DesignerSerializationManager _serializationManager;

		private IDisposable _serializationSession;

		[Flags]
		protected enum ReloadOptions
		{
			Default = 0,
			ModifyOnError = 1,
			Force = 2,
			NoFlush = 4
		}
	}
}
