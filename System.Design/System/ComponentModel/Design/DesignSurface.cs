using System;
using System.Collections;
using System.ComponentModel.Design.Serialization;
using System.Design;
using System.Reflection;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class DesignSurface : IDisposable, IServiceProvider
	{
		public DesignSurface()
			: this(null)
		{
		}

		public DesignSurface(IServiceProvider parentProvider)
		{
			this._parentProvider = parentProvider;
			this._serviceContainer = new DesignSurfaceServiceContainer(this._parentProvider);
			ServiceCreatorCallback serviceCreatorCallback = new ServiceCreatorCallback(this.OnCreateService);
			this.ServiceContainer.AddService(typeof(ISelectionService), serviceCreatorCallback);
			this.ServiceContainer.AddService(typeof(IExtenderProviderService), serviceCreatorCallback);
			this.ServiceContainer.AddService(typeof(IExtenderListService), serviceCreatorCallback);
			this.ServiceContainer.AddService(typeof(ITypeDescriptorFilterService), serviceCreatorCallback);
			this.ServiceContainer.AddService(typeof(IReferenceService), serviceCreatorCallback);
			this.ServiceContainer.AddService(typeof(DesignSurface), this);
			this._host = new DesignerHost(this);
		}

		public DesignSurface(Type rootComponentType)
			: this(null, rootComponentType)
		{
		}

		public DesignSurface(IServiceProvider parentProvider, Type rootComponentType)
			: this(parentProvider)
		{
			if (rootComponentType == null)
			{
				throw new ArgumentNullException("rootComponentType");
			}
			this.BeginLoad(rootComponentType);
		}

		public IContainer ComponentContainer
		{
			get
			{
				if (this._host == null)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
				return ((IDesignerHost)this._host).Container;
			}
		}

		public bool IsLoaded
		{
			get
			{
				return this._loaded;
			}
		}

		public ICollection LoadErrors
		{
			get
			{
				if (this._loadErrors != null)
				{
					return this._loadErrors;
				}
				return new object[0];
			}
		}

		protected ServiceContainer ServiceContainer
		{
			get
			{
				if (this._serviceContainer == null)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
				return this._serviceContainer;
			}
		}

		public object View
		{
			get
			{
				if (this._host == null)
				{
					throw new ObjectDisposedException(this.ToString());
				}
				IComponent rootComponent = ((IDesignerHost)this._host).RootComponent;
				if (rootComponent == null)
				{
					if (this._loadErrors != null)
					{
						using (IEnumerator enumerator = this._loadErrors.GetEnumerator())
						{
							if (enumerator.MoveNext())
							{
								object obj = enumerator.Current;
								Exception ex = obj as Exception;
								if (ex != null)
								{
									throw new InvalidOperationException(ex.Message, ex);
								}
								throw new InvalidOperationException(obj.ToString());
							}
						}
					}
					throw new InvalidOperationException(SR.GetString("DesignSurfaceNoRootComponent"))
					{
						HelpLink = "DesignSurfaceNoRootComponent"
					};
				}
				IRootDesigner rootDesigner = ((IDesignerHost)this._host).GetDesigner(rootComponent) as IRootDesigner;
				if (rootDesigner == null)
				{
					throw new InvalidOperationException(SR.GetString("DesignSurfaceDesignerNotLoaded"))
					{
						HelpLink = "DesignSurfaceDesignerNotLoaded"
					};
				}
				ViewTechnology[] supportedTechnologies = rootDesigner.SupportedTechnologies;
				ViewTechnology[] array = supportedTechnologies;
				int num = 0;
				if (num >= array.Length)
				{
					throw new NotSupportedException(SR.GetString("DesignSurfaceNoSupportedTechnology"))
					{
						HelpLink = "DesignSurfaceNoSupportedTechnology"
					};
				}
				ViewTechnology viewTechnology = array[num];
				return rootDesigner.GetView(viewTechnology);
			}
		}

		public event EventHandler Disposed;

		public event EventHandler Flushed;

		public event LoadedEventHandler Loaded;

		public event EventHandler Loading;

		public event EventHandler Unloaded;

		public event EventHandler Unloading;

		public event EventHandler ViewActivated;

		public void BeginLoad(DesignerLoader loader)
		{
			if (loader == null)
			{
				throw new ArgumentNullException("loader");
			}
			if (this._host == null)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			this._loadErrors = null;
			this._host.BeginLoad(loader);
		}

		public void BeginLoad(Type rootComponentType)
		{
			if (rootComponentType == null)
			{
				throw new ArgumentNullException("rootComponentType");
			}
			if (this._host == null)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			this.BeginLoad(new DesignSurface.DefaultDesignerLoader(rootComponentType));
		}

		[Obsolete("CreateComponent has been replaced by CreateInstance and will be removed after Beta2")]
		protected internal virtual IComponent CreateComponent(Type componentType)
		{
			return this.CreateInstance(componentType) as IComponent;
		}

		protected internal virtual IDesigner CreateDesigner(IComponent component, bool rootDesigner)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			if (this._host == null)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			IDesigner designer;
			if (rootDesigner)
			{
				designer = TypeDescriptor.CreateDesigner(component, typeof(IRootDesigner)) as IRootDesigner;
			}
			else
			{
				designer = TypeDescriptor.CreateDesigner(component, typeof(IDesigner));
			}
			return designer;
		}

		protected internal virtual object CreateInstance(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			object obj = null;
			ConstructorInfo constructorInfo = TypeDescriptor.GetReflectionType(type).GetConstructor(new Type[0]);
			if (constructorInfo != null)
			{
				obj = TypeDescriptor.CreateInstance(this, type, new Type[0], new object[0]);
			}
			else
			{
				if (typeof(IComponent).IsAssignableFrom(type))
				{
					constructorInfo = TypeDescriptor.GetReflectionType(type).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.ExactBinding, null, new Type[] { typeof(IContainer) }, null);
				}
				if (constructorInfo != null)
				{
					obj = TypeDescriptor.CreateInstance(this, type, new Type[] { typeof(IContainer) }, new object[] { this.ComponentContainer });
				}
			}
			if (obj == null)
			{
				obj = Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, null, null);
			}
			return obj;
		}

		public INestedContainer CreateNestedContainer(IComponent owningComponent)
		{
			return this.CreateNestedContainer(owningComponent, null);
		}

		public INestedContainer CreateNestedContainer(IComponent owningComponent, string containerName)
		{
			if (this._host == null)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (owningComponent == null)
			{
				throw new ArgumentNullException("owningComponent");
			}
			return new SiteNestedContainer(owningComponent, containerName, this._host);
		}

		public void Dispose()
		{
			this.Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.Disposed != null)
				{
					this.Disposed(this, EventArgs.Empty);
				}
				try
				{
					try
					{
						if (this._host != null)
						{
							this._host.DisposeHost();
						}
					}
					finally
					{
						if (this._serviceContainer != null)
						{
							this._serviceContainer.RemoveService(typeof(DesignSurface));
							this._serviceContainer.Dispose();
						}
					}
				}
				finally
				{
					this._host = null;
					this._serviceContainer = null;
				}
			}
		}

		public void Flush()
		{
			if (this._host != null)
			{
				this._host.Flush();
			}
			if (this.Flushed != null)
			{
				this.Flushed(this, EventArgs.Empty);
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

		internal void OnViewActivate()
		{
			this.OnViewActivate(EventArgs.Empty);
		}

		private object OnCreateService(IServiceContainer container, Type serviceType)
		{
			if (serviceType == typeof(ISelectionService))
			{
				return new SelectionService(container);
			}
			if (serviceType == typeof(IExtenderProviderService))
			{
				return new ExtenderProviderService();
			}
			if (serviceType == typeof(IExtenderListService))
			{
				return this.GetService(typeof(IExtenderProviderService));
			}
			if (serviceType == typeof(ITypeDescriptorFilterService))
			{
				return new TypeDescriptorFilterService();
			}
			if (serviceType == typeof(IReferenceService))
			{
				return new ReferenceService(container);
			}
			return null;
		}

		internal void OnLoaded(bool successful, ICollection errors)
		{
			this._loaded = successful;
			this._loadErrors = errors;
			if (successful && ((IDesignerHost)this._host).RootComponent == null)
			{
				ArrayList arrayList = new ArrayList();
				arrayList.Add(new InvalidOperationException(SR.GetString("DesignSurfaceNoRootComponent"))
				{
					HelpLink = "DesignSurfaceNoRootComponent"
				});
				if (errors != null)
				{
					arrayList.AddRange(errors);
				}
				errors = arrayList;
				successful = false;
			}
			this.OnLoaded(new LoadedEventArgs(successful, errors));
		}

		protected virtual void OnLoaded(LoadedEventArgs e)
		{
			if (this.Loaded != null)
			{
				this.Loaded(this, e);
			}
		}

		internal void OnLoading()
		{
			this.OnLoading(EventArgs.Empty);
		}

		protected virtual void OnLoading(EventArgs e)
		{
			if (this.Loading != null)
			{
				this.Loading(this, e);
			}
		}

		internal void OnUnloaded()
		{
			this.OnUnloaded(EventArgs.Empty);
		}

		protected virtual void OnUnloaded(EventArgs e)
		{
			if (this.Unloaded != null)
			{
				this.Unloaded(this, e);
			}
		}

		internal void OnUnloading()
		{
			this.OnUnloading(EventArgs.Empty);
			this._loaded = false;
		}

		protected virtual void OnUnloading(EventArgs e)
		{
			if (this.Unloading != null)
			{
				this.Unloading(this, e);
			}
		}

		protected virtual void OnViewActivate(EventArgs e)
		{
			if (this.ViewActivated != null)
			{
				this.ViewActivated(this, e);
			}
		}

		private IServiceProvider _parentProvider;

		private ServiceContainer _serviceContainer;

		private DesignerHost _host;

		private ICollection _loadErrors;

		private bool _loaded;

		private class DefaultDesignerLoader : DesignerLoader
		{
			public DefaultDesignerLoader(Type type)
			{
				this._type = type;
			}

			public DefaultDesignerLoader(ICollection components)
			{
				this._components = components;
			}

			public override void BeginLoad(IDesignerLoaderHost loaderHost)
			{
				string text = null;
				if (this._type != null)
				{
					loaderHost.CreateComponent(this._type);
					text = this._type.FullName;
				}
				else
				{
					foreach (object obj in this._components)
					{
						IComponent component = (IComponent)obj;
						loaderHost.Container.Add(component);
					}
				}
				loaderHost.EndLoad(text, true, null);
			}

			public override void Dispose()
			{
			}

			private Type _type;

			private ICollection _components;
		}
	}
}
