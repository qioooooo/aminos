using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel.Design.Serialization;
using System.Design;
using System.Globalization;
using System.Reflection;

namespace System.ComponentModel.Design
{
	internal sealed class DesignerHost : Container, IDesignerLoaderHost2, IDesignerLoaderHost, IDesignerHost, IServiceContainer, IServiceProvider, IDesignerHostTransactionState, IComponentChangeService, IReflect
	{
		public DesignerHost(DesignSurface surface)
		{
			this._surface = surface;
			this._state = default(BitVector32);
			this._designers = new Hashtable();
			this._events = new EventHandlerList();
			DesignSurfaceServiceContainer designSurfaceServiceContainer = this.GetService(typeof(DesignSurfaceServiceContainer)) as DesignSurfaceServiceContainer;
			if (designSurfaceServiceContainer != null)
			{
				foreach (Type type in DesignerHost.DefaultServices)
				{
					designSurfaceServiceContainer.AddFixedService(type, this);
				}
				return;
			}
			IServiceContainer serviceContainer = this.GetService(typeof(IServiceContainer)) as IServiceContainer;
			if (serviceContainer != null)
			{
				foreach (Type type2 in DesignerHost.DefaultServices)
				{
					serviceContainer.AddService(type2, this);
				}
			}
		}

		internal HostDesigntimeLicenseContext LicenseContext
		{
			get
			{
				if (this._licenseCtx == null)
				{
					this._licenseCtx = new HostDesigntimeLicenseContext(this);
				}
				return this._licenseCtx;
			}
		}

		internal bool IsClosingTransaction
		{
			get
			{
				return this._state[DesignerHost.StateIsClosingTransaction];
			}
			set
			{
				this._state[DesignerHost.StateIsClosingTransaction] = value;
			}
		}

		bool IDesignerHostTransactionState.IsClosingTransaction
		{
			get
			{
				return this.IsClosingTransaction;
			}
		}

		public override void Add(IComponent component, string name)
		{
			if (this.AddToContainerPreProcess(component, name, this))
			{
				base.Add(component, name);
				try
				{
					this.AddToContainerPostProcess(component, name, this);
				}
				catch (Exception ex)
				{
					if (ex != CheckoutException.Canceled)
					{
						this.Remove(component);
					}
					throw;
				}
				catch
				{
					this.Remove(component);
					throw;
				}
			}
		}

		internal bool AddToContainerPreProcess(IComponent component, string name, IContainer containerToAddTo)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			if (this._state[DesignerHost.StateUnloading])
			{
				throw new Exception(SR.GetString("DesignerHostUnloading"))
				{
					HelpLink = "DesignerHostUnloading"
				};
			}
			if (this._rootComponent != null && string.Equals(component.GetType().FullName, this._rootComponentClassName, StringComparison.OrdinalIgnoreCase))
			{
				throw new Exception(SR.GetString("DesignerHostCyclicAdd", new object[]
				{
					component.GetType().FullName,
					this._rootComponentClassName
				}))
				{
					HelpLink = "DesignerHostCyclicAdd"
				};
			}
			ISite site = component.Site;
			if (site != null && site.Container == this)
			{
				if (name != null)
				{
					site.Name = name;
				}
				return false;
			}
			ComponentEventArgs componentEventArgs = new ComponentEventArgs(component);
			ComponentEventHandler componentEventHandler = this._events[DesignerHost.EventComponentAdding] as ComponentEventHandler;
			if (componentEventHandler != null)
			{
				componentEventHandler(containerToAddTo, componentEventArgs);
			}
			return true;
		}

		internal void AddToContainerPostProcess(IComponent component, string name, IContainer containerToAddTo)
		{
			if (component is IExtenderProvider && !TypeDescriptor.GetAttributes(component).Contains(InheritanceAttribute.InheritedReadOnly))
			{
				IExtenderProviderService extenderProviderService = this.GetService(typeof(IExtenderProviderService)) as IExtenderProviderService;
				if (extenderProviderService != null)
				{
					extenderProviderService.AddExtenderProvider((IExtenderProvider)component);
				}
			}
			IDesigner designer = null;
			if (this._rootComponent == null)
			{
				designer = this._surface.CreateDesigner(component, true) as IRootDesigner;
				if (designer == null)
				{
					throw new Exception(SR.GetString("DesignerHostNoTopLevelDesigner", new object[] { component.GetType().FullName }))
					{
						HelpLink = "DesignerHostNoTopLevelDesigner"
					};
				}
				this._rootComponent = component;
				if (this._rootComponentClassName == null)
				{
					this._rootComponentClassName = component.Site.Name;
				}
			}
			else
			{
				designer = this._surface.CreateDesigner(component, false);
			}
			if (designer != null)
			{
				this._designers[component] = designer;
				try
				{
					designer.Initialize(component);
					if (designer.Component == null)
					{
						throw new InvalidOperationException(SR.GetString("DesignerHostDesignerNeedsComponent"));
					}
				}
				catch
				{
					this._designers.Remove(component);
					throw;
				}
				if (designer is IExtenderProvider)
				{
					IExtenderProviderService extenderProviderService2 = this.GetService(typeof(IExtenderProviderService)) as IExtenderProviderService;
					if (extenderProviderService2 != null)
					{
						extenderProviderService2.AddExtenderProvider((IExtenderProvider)designer);
					}
				}
			}
			ComponentEventArgs componentEventArgs = new ComponentEventArgs(component);
			ComponentEventHandler componentEventHandler = this._events[DesignerHost.EventComponentAdded] as ComponentEventHandler;
			if (componentEventHandler != null)
			{
				componentEventHandler(containerToAddTo, componentEventArgs);
			}
		}

		internal void BeginLoad(DesignerLoader loader)
		{
			if (this._loader != null && this._loader != loader)
			{
				throw new InvalidOperationException(SR.GetString("DesignerHostLoaderSpecified"))
				{
					HelpLink = "DesignerHostLoaderSpecified"
				};
			}
			bool flag = this._loader != null;
			this._loader = loader;
			if (!flag)
			{
				if (loader is IExtenderProvider)
				{
					IExtenderProviderService extenderProviderService = this.GetService(typeof(IExtenderProviderService)) as IExtenderProviderService;
					if (extenderProviderService != null)
					{
						extenderProviderService.AddExtenderProvider((IExtenderProvider)loader);
					}
				}
				IDesignerEventService designerEventService = this.GetService(typeof(IDesignerEventService)) as IDesignerEventService;
				if (designerEventService != null)
				{
					designerEventService.ActiveDesignerChanged += this.OnActiveDesignerChanged;
					this._designerEventService = designerEventService;
				}
			}
			this._state[DesignerHost.StateLoading] = true;
			this._surface.OnLoading();
			try
			{
				this._loader.BeginLoad(this);
			}
			catch (Exception ex)
			{
				if (ex is TargetInvocationException)
				{
					ex = ex.InnerException;
				}
				string message = ex.Message;
				if (message == null || message.Length == 0)
				{
					ex = new Exception(SR.GetString("DesignSurfaceFatalError", new object[] { ex.ToString() }), ex);
				}
				((IDesignerLoaderHost)this).EndLoad(null, false, new object[] { ex });
			}
			if (this._designerEventService == null)
			{
				this.OnActiveDesignerChanged(null, new ActiveDesignerEventArgs(null, this));
			}
		}

		protected override ISite CreateSite(IComponent component, string name)
		{
			if (this._newComponentName != null)
			{
				name = this._newComponentName;
				this._newComponentName = null;
			}
			INameCreationService nameCreationService = this.GetService(typeof(INameCreationService)) as INameCreationService;
			if (name == null)
			{
				if (nameCreationService != null)
				{
					name = nameCreationService.CreateName(this, TypeDescriptor.GetReflectionType(component));
				}
				else
				{
					name = string.Empty;
				}
			}
			else if (nameCreationService != null)
			{
				nameCreationService.ValidateName(name);
			}
			return new DesignerHost.Site(component, this, name, this);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				throw new InvalidOperationException(SR.GetString("DesignSurfaceContainerDispose"));
			}
			base.Dispose(disposing);
		}

		internal void DisposeHost()
		{
			try
			{
				if (this._loader != null)
				{
					this._loader.Dispose();
					this.Unload();
				}
				if (this._surface != null)
				{
					if (this._designerEventService != null)
					{
						this._designerEventService.ActiveDesignerChanged -= this.OnActiveDesignerChanged;
					}
					DesignSurfaceServiceContainer designSurfaceServiceContainer = this.GetService(typeof(DesignSurfaceServiceContainer)) as DesignSurfaceServiceContainer;
					if (designSurfaceServiceContainer != null)
					{
						foreach (Type type in DesignerHost.DefaultServices)
						{
							designSurfaceServiceContainer.RemoveFixedService(type);
						}
					}
					else
					{
						IServiceContainer serviceContainer = this.GetService(typeof(IServiceContainer)) as IServiceContainer;
						if (serviceContainer != null)
						{
							foreach (Type type2 in DesignerHost.DefaultServices)
							{
								serviceContainer.RemoveService(type2);
							}
						}
					}
				}
			}
			finally
			{
				this._loader = null;
				this._surface = null;
				this._events.Dispose();
			}
			base.Dispose(true);
		}

		internal void Flush()
		{
			if (this._loader != null)
			{
				this._loader.Flush();
			}
		}

		protected override object GetService(Type service)
		{
			if (service == null)
			{
				throw new ArgumentNullException("service");
			}
			object obj = base.GetService(service);
			if (obj == null && this._surface != null)
			{
				obj = this._surface.GetService(service);
			}
			return obj;
		}

		private void OnActiveDesignerChanged(object sender, ActiveDesignerEventArgs e)
		{
			object obj = null;
			if (e.OldDesigner == this)
			{
				obj = DesignerHost.EventDeactivated;
			}
			else if (e.NewDesigner == this)
			{
				obj = DesignerHost.EventActivated;
			}
			if (obj == null)
			{
				return;
			}
			if (e.OldDesigner == this && this._surface != null)
			{
				this._surface.Flush();
			}
			EventHandler eventHandler = this._events[obj] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, EventArgs.Empty);
			}
		}

		private void OnComponentRename(IComponent component, string oldName, string newName)
		{
			if (component == this._rootComponent)
			{
				string rootComponentClassName = this._rootComponentClassName;
				int num = rootComponentClassName.LastIndexOf(oldName);
				if (num + oldName.Length == rootComponentClassName.Length && num - 1 >= 0 && rootComponentClassName[num - 1] == '.')
				{
					this._rootComponentClassName = rootComponentClassName.Substring(0, num) + newName;
				}
				else
				{
					this._rootComponentClassName = newName;
				}
			}
			ComponentRenameEventHandler componentRenameEventHandler = this._events[DesignerHost.EventComponentRename] as ComponentRenameEventHandler;
			if (componentRenameEventHandler != null)
			{
				componentRenameEventHandler(this, new ComponentRenameEventArgs(component, oldName, newName));
			}
		}

		private void OnLoadComplete(EventArgs e)
		{
			EventHandler eventHandler = this._events[DesignerHost.EventLoadComplete] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		private void OnTransactionClosed(DesignerTransactionCloseEventArgs e)
		{
			DesignerTransactionCloseEventHandler designerTransactionCloseEventHandler = this._events[DesignerHost.EventTransactionClosed] as DesignerTransactionCloseEventHandler;
			if (designerTransactionCloseEventHandler != null)
			{
				designerTransactionCloseEventHandler(this, e);
			}
		}

		private void OnTransactionClosing(DesignerTransactionCloseEventArgs e)
		{
			DesignerTransactionCloseEventHandler designerTransactionCloseEventHandler = this._events[DesignerHost.EventTransactionClosing] as DesignerTransactionCloseEventHandler;
			if (designerTransactionCloseEventHandler != null)
			{
				designerTransactionCloseEventHandler(this, e);
			}
		}

		private void OnTransactionOpened(EventArgs e)
		{
			EventHandler eventHandler = this._events[DesignerHost.EventTransactionOpened] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		private void OnTransactionOpening(EventArgs e)
		{
			EventHandler eventHandler = this._events[DesignerHost.EventTransactionOpening] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		public override void Remove(IComponent component)
		{
			if (this.RemoveFromContainerPreProcess(component, this))
			{
				DesignerHost.Site site = component.Site as DesignerHost.Site;
				base.RemoveWithoutUnsiting(component);
				site.Disposed = true;
				this.RemoveFromContainerPostProcess(component, this);
			}
		}

		internal bool RemoveFromContainerPreProcess(IComponent component, IContainer container)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			ISite site = component.Site;
			if (site == null || site.Container != container)
			{
				return false;
			}
			ComponentEventArgs componentEventArgs = new ComponentEventArgs(component);
			ComponentEventHandler componentEventHandler = this._events[DesignerHost.EventComponentRemoving] as ComponentEventHandler;
			if (componentEventHandler != null)
			{
				componentEventHandler(this, componentEventArgs);
			}
			if (component is IExtenderProvider)
			{
				IExtenderProviderService extenderProviderService = this.GetService(typeof(IExtenderProviderService)) as IExtenderProviderService;
				if (extenderProviderService != null)
				{
					extenderProviderService.RemoveExtenderProvider((IExtenderProvider)component);
				}
			}
			IDesigner designer = this._designers[component] as IDesigner;
			if (designer is IExtenderProvider)
			{
				IExtenderProviderService extenderProviderService2 = this.GetService(typeof(IExtenderProviderService)) as IExtenderProviderService;
				if (extenderProviderService2 != null)
				{
					extenderProviderService2.RemoveExtenderProvider((IExtenderProvider)designer);
				}
			}
			if (designer != null)
			{
				designer.Dispose();
				this._designers.Remove(component);
			}
			if (component == this._rootComponent)
			{
				this._rootComponent = null;
				this._rootComponentClassName = null;
			}
			return true;
		}

		internal void RemoveFromContainerPostProcess(IComponent component, IContainer container)
		{
			try
			{
				ComponentEventHandler componentEventHandler = this._events[DesignerHost.EventComponentRemoved] as ComponentEventHandler;
				ComponentEventArgs componentEventArgs = new ComponentEventArgs(component);
				if (componentEventHandler != null)
				{
					componentEventHandler(this, componentEventArgs);
				}
			}
			finally
			{
				component.Site = null;
			}
		}

		private void Unload()
		{
			this._surface.OnUnloading();
			IHelpService helpService = this.GetService(typeof(IHelpService)) as IHelpService;
			if (helpService != null && this._rootComponent != null && this._designers[this._rootComponent] != null)
			{
				helpService.RemoveContextAttribute("Keyword", "Designer_" + this._designers[this._rootComponent].GetType().FullName);
			}
			ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
			if (selectionService != null)
			{
				selectionService.SetSelectedComponents(null, SelectionTypes.Replace);
			}
			this._state[DesignerHost.StateUnloading] = true;
			DesignerTransaction designerTransaction = ((IDesignerHost)this).CreateTransaction();
			ArrayList arrayList = new ArrayList();
			try
			{
				IComponent[] array = new IComponent[this.Components.Count];
				this.Components.CopyTo(array, 0);
				foreach (IComponent component in array)
				{
					if (!object.ReferenceEquals(component, this._rootComponent))
					{
						IDesigner designer = this._designers[component] as IDesigner;
						if (designer != null)
						{
							this._designers.Remove(component);
							try
							{
								designer.Dispose();
							}
							catch (Exception ex)
							{
								if (designer == null)
								{
									string empty = string.Empty;
								}
								else
								{
									string name = designer.GetType().Name;
								}
								arrayList.Add(ex);
							}
						}
						try
						{
							component.Dispose();
						}
						catch (Exception ex2)
						{
							if (component == null)
							{
								string empty2 = string.Empty;
							}
							else
							{
								string name2 = component.GetType().Name;
							}
							arrayList.Add(ex2);
						}
					}
				}
				if (this._rootComponent != null)
				{
					IDesigner designer2 = this._designers[this._rootComponent] as IDesigner;
					if (designer2 != null)
					{
						this._designers.Remove(this._rootComponent);
						try
						{
							designer2.Dispose();
						}
						catch (Exception ex3)
						{
							if (designer2 == null)
							{
								string empty3 = string.Empty;
							}
							else
							{
								string name3 = designer2.GetType().Name;
							}
							arrayList.Add(ex3);
						}
					}
					try
					{
						this._rootComponent.Dispose();
					}
					catch (Exception ex4)
					{
						if (this._rootComponent == null)
						{
							string empty4 = string.Empty;
						}
						else
						{
							string name4 = this._rootComponent.GetType().Name;
						}
						arrayList.Add(ex4);
					}
				}
				this._designers.Clear();
				while (this.Components.Count > 0)
				{
					this.Remove(this.Components[0]);
				}
			}
			finally
			{
				designerTransaction.Commit();
				this._state[DesignerHost.StateUnloading] = false;
			}
			if (this._transactions != null && this._transactions.Count > 0)
			{
				while (this._transactions.Count > 0)
				{
					DesignerTransaction designerTransaction2 = (DesignerTransaction)this._transactions.Peek();
					designerTransaction2.Commit();
				}
			}
			this._surface.OnUnloaded();
			if (arrayList.Count > 0)
			{
				throw new ExceptionCollection(arrayList);
			}
		}

		event ComponentEventHandler IComponentChangeService.ComponentAdded
		{
			add
			{
				this._events.AddHandler(DesignerHost.EventComponentAdded, value);
			}
			remove
			{
				this._events.RemoveHandler(DesignerHost.EventComponentAdded, value);
			}
		}

		event ComponentEventHandler IComponentChangeService.ComponentAdding
		{
			add
			{
				this._events.AddHandler(DesignerHost.EventComponentAdding, value);
			}
			remove
			{
				this._events.RemoveHandler(DesignerHost.EventComponentAdding, value);
			}
		}

		event ComponentChangedEventHandler IComponentChangeService.ComponentChanged
		{
			add
			{
				this._events.AddHandler(DesignerHost.EventComponentChanged, value);
			}
			remove
			{
				this._events.RemoveHandler(DesignerHost.EventComponentChanged, value);
			}
		}

		event ComponentChangingEventHandler IComponentChangeService.ComponentChanging
		{
			add
			{
				this._events.AddHandler(DesignerHost.EventComponentChanging, value);
			}
			remove
			{
				this._events.RemoveHandler(DesignerHost.EventComponentChanging, value);
			}
		}

		event ComponentEventHandler IComponentChangeService.ComponentRemoved
		{
			add
			{
				this._events.AddHandler(DesignerHost.EventComponentRemoved, value);
			}
			remove
			{
				this._events.RemoveHandler(DesignerHost.EventComponentRemoved, value);
			}
		}

		event ComponentEventHandler IComponentChangeService.ComponentRemoving
		{
			add
			{
				this._events.AddHandler(DesignerHost.EventComponentRemoving, value);
			}
			remove
			{
				this._events.RemoveHandler(DesignerHost.EventComponentRemoving, value);
			}
		}

		event ComponentRenameEventHandler IComponentChangeService.ComponentRename
		{
			add
			{
				this._events.AddHandler(DesignerHost.EventComponentRename, value);
			}
			remove
			{
				this._events.RemoveHandler(DesignerHost.EventComponentRename, value);
			}
		}

		void IComponentChangeService.OnComponentChanged(object component, MemberDescriptor member, object oldValue, object newValue)
		{
			if (!((IDesignerHost)this).Loading)
			{
				ComponentChangedEventHandler componentChangedEventHandler = this._events[DesignerHost.EventComponentChanged] as ComponentChangedEventHandler;
				if (componentChangedEventHandler != null)
				{
					componentChangedEventHandler(this, new ComponentChangedEventArgs(component, member, oldValue, newValue));
				}
			}
		}

		void IComponentChangeService.OnComponentChanging(object component, MemberDescriptor member)
		{
			if (!((IDesignerHost)this).Loading)
			{
				ComponentChangingEventHandler componentChangingEventHandler = this._events[DesignerHost.EventComponentChanging] as ComponentChangingEventHandler;
				if (componentChangingEventHandler != null)
				{
					componentChangingEventHandler(this, new ComponentChangingEventArgs(component, member));
				}
			}
		}

		bool IDesignerHost.Loading
		{
			get
			{
				return this._state[DesignerHost.StateLoading] || this._state[DesignerHost.StateUnloading] || (this._loader != null && this._loader.Loading);
			}
		}

		bool IDesignerHost.InTransaction
		{
			get
			{
				return (this._transactions != null && this._transactions.Count > 0) || this.IsClosingTransaction;
			}
		}

		IContainer IDesignerHost.Container
		{
			get
			{
				return this;
			}
		}

		IComponent IDesignerHost.RootComponent
		{
			get
			{
				return this._rootComponent;
			}
		}

		string IDesignerHost.RootComponentClassName
		{
			get
			{
				return this._rootComponentClassName;
			}
		}

		string IDesignerHost.TransactionDescription
		{
			get
			{
				if (this._transactions != null && this._transactions.Count > 0)
				{
					return ((DesignerTransaction)this._transactions.Peek()).Description;
				}
				return null;
			}
		}

		event EventHandler IDesignerHost.Activated
		{
			add
			{
				this._events.AddHandler(DesignerHost.EventActivated, value);
			}
			remove
			{
				this._events.RemoveHandler(DesignerHost.EventActivated, value);
			}
		}

		event EventHandler IDesignerHost.Deactivated
		{
			add
			{
				this._events.AddHandler(DesignerHost.EventDeactivated, value);
			}
			remove
			{
				this._events.RemoveHandler(DesignerHost.EventDeactivated, value);
			}
		}

		event EventHandler IDesignerHost.LoadComplete
		{
			add
			{
				this._events.AddHandler(DesignerHost.EventLoadComplete, value);
			}
			remove
			{
				this._events.RemoveHandler(DesignerHost.EventLoadComplete, value);
			}
		}

		event DesignerTransactionCloseEventHandler IDesignerHost.TransactionClosed
		{
			add
			{
				this._events.AddHandler(DesignerHost.EventTransactionClosed, value);
			}
			remove
			{
				this._events.RemoveHandler(DesignerHost.EventTransactionClosed, value);
			}
		}

		event DesignerTransactionCloseEventHandler IDesignerHost.TransactionClosing
		{
			add
			{
				this._events.AddHandler(DesignerHost.EventTransactionClosing, value);
			}
			remove
			{
				this._events.RemoveHandler(DesignerHost.EventTransactionClosing, value);
			}
		}

		event EventHandler IDesignerHost.TransactionOpened
		{
			add
			{
				this._events.AddHandler(DesignerHost.EventTransactionOpened, value);
			}
			remove
			{
				this._events.RemoveHandler(DesignerHost.EventTransactionOpened, value);
			}
		}

		event EventHandler IDesignerHost.TransactionOpening
		{
			add
			{
				this._events.AddHandler(DesignerHost.EventTransactionOpening, value);
			}
			remove
			{
				this._events.RemoveHandler(DesignerHost.EventTransactionOpening, value);
			}
		}

		void IDesignerHost.Activate()
		{
			this._surface.OnViewActivate();
		}

		IComponent IDesignerHost.CreateComponent(Type componentType)
		{
			return ((IDesignerHost)this).CreateComponent(componentType, null);
		}

		IComponent IDesignerHost.CreateComponent(Type componentType, string name)
		{
			if (componentType == null)
			{
				throw new ArgumentNullException("componentType");
			}
			LicenseContext currentContext = LicenseManager.CurrentContext;
			bool flag = false;
			if (currentContext != this.LicenseContext)
			{
				LicenseManager.CurrentContext = this.LicenseContext;
				LicenseManager.LockContext(DesignerHost._selfLock);
				flag = true;
			}
			IComponent component;
			try
			{
				try
				{
					this._newComponentName = name;
					component = this._surface.CreateInstance(componentType) as IComponent;
				}
				finally
				{
					this._newComponentName = null;
				}
				if (component == null)
				{
					throw new InvalidOperationException(SR.GetString("DesignerHostFailedComponentCreate", new object[] { componentType.Name }))
					{
						HelpLink = "DesignerHostFailedComponentCreate"
					};
				}
				if (component.Site == null || component.Site.Container != this)
				{
					this.Add(component, name);
				}
			}
			finally
			{
				if (flag)
				{
					LicenseManager.UnlockContext(DesignerHost._selfLock);
					LicenseManager.CurrentContext = currentContext;
				}
			}
			return component;
		}

		DesignerTransaction IDesignerHost.CreateTransaction()
		{
			return ((IDesignerHost)this).CreateTransaction(null);
		}

		DesignerTransaction IDesignerHost.CreateTransaction(string description)
		{
			if (description == null)
			{
				description = SR.GetString("DesignerHostGenericTransactionName");
			}
			return new DesignerHost.DesignerHostTransaction(this, description);
		}

		void IDesignerHost.DestroyComponent(IComponent component)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			string text;
			if (component.Site != null && component.Site.Name != null)
			{
				text = component.Site.Name;
			}
			else
			{
				text = component.GetType().Name;
			}
			InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(component)[typeof(InheritanceAttribute)];
			if (inheritanceAttribute != null && inheritanceAttribute.InheritanceLevel != InheritanceLevel.NotInherited)
			{
				throw new InvalidOperationException(SR.GetString("DesignerHostCantDestroyInheritedComponent", new object[] { text }))
				{
					HelpLink = "DesignerHostCantDestroyInheritedComponent"
				};
			}
			if (((IDesignerHost)this).InTransaction)
			{
				this.Remove(component);
				component.Dispose();
				return;
			}
			DesignerTransaction designerTransaction2;
			DesignerTransaction designerTransaction = (designerTransaction2 = ((IDesignerHost)this).CreateTransaction(SR.GetString("DesignerHostDestroyComponentTransaction", new object[] { text })));
			try
			{
				this.Remove(component);
				component.Dispose();
				designerTransaction.Commit();
			}
			finally
			{
				if (designerTransaction2 != null)
				{
					((IDisposable)designerTransaction2).Dispose();
				}
			}
		}

		IDesigner IDesignerHost.GetDesigner(IComponent component)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			return this._designers[component] as IDesigner;
		}

		Type IDesignerHost.GetType(string typeName)
		{
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			ITypeResolutionService typeResolutionService = this.GetService(typeof(ITypeResolutionService)) as ITypeResolutionService;
			if (typeResolutionService != null)
			{
				return typeResolutionService.GetType(typeName);
			}
			return Type.GetType(typeName);
		}

		void IDesignerLoaderHost.EndLoad(string rootClassName, bool successful, ICollection errorCollection)
		{
			bool flag = this._state[DesignerHost.StateLoading];
			this._state[DesignerHost.StateLoading] = false;
			if (rootClassName != null)
			{
				this._rootComponentClassName = rootClassName;
			}
			else if (this._rootComponent != null && this._rootComponent.Site != null)
			{
				this._rootComponentClassName = this._rootComponent.Site.Name;
			}
			if (successful && this._rootComponent == null)
			{
				errorCollection = new ArrayList
				{
					new InvalidOperationException(SR.GetString("DesignerHostNoBaseClass"))
					{
						HelpLink = "DesignerHostNoBaseClass"
					}
				};
				successful = false;
			}
			if (!successful)
			{
				this.Unload();
			}
			if (flag && this._surface != null)
			{
				this._surface.OnLoaded(successful, errorCollection);
			}
			if (successful && flag)
			{
				IRootDesigner rootDesigner = ((IDesignerHost)this).GetDesigner(this._rootComponent) as IRootDesigner;
				IHelpService helpService = this.GetService(typeof(IHelpService)) as IHelpService;
				if (helpService != null)
				{
					helpService.AddContextAttribute("Keyword", "Designer_" + rootDesigner.GetType().FullName, HelpKeywordType.F1Keyword);
				}
				try
				{
					this.OnLoadComplete(EventArgs.Empty);
				}
				catch (Exception ex)
				{
					this._state[DesignerHost.StateLoading] = true;
					this.Unload();
					ArrayList arrayList = new ArrayList();
					arrayList.Add(ex);
					if (errorCollection != null)
					{
						arrayList.AddRange(errorCollection);
					}
					errorCollection = arrayList;
					successful = false;
					if (this._surface != null)
					{
						this._surface.OnLoaded(successful, errorCollection);
					}
					throw;
				}
				if (successful && this._savedSelection != null)
				{
					ISelectionService selectionService = this.GetService(typeof(ISelectionService)) as ISelectionService;
					if (selectionService != null)
					{
						ArrayList arrayList2 = new ArrayList(this._savedSelection.Count);
						foreach (object obj in this._savedSelection)
						{
							string text = (string)obj;
							IComponent component = this.Components[text];
							if (component != null)
							{
								arrayList2.Add(component);
							}
						}
						this._savedSelection = null;
						selectionService.SetSelectedComponents(arrayList2, SelectionTypes.Replace);
					}
				}
			}
		}

		void IDesignerLoaderHost.Reload()
		{
			if (this._loader != null)
			{
				this._surface.Flush();
				ISelectionService selectionService = this.GetService(typeof(ISelectionService)) as ISelectionService;
				if (selectionService != null)
				{
					ArrayList arrayList = new ArrayList(selectionService.SelectionCount);
					foreach (object obj in selectionService.GetSelectedComponents())
					{
						IComponent component = obj as IComponent;
						if (component != null && component.Site != null && component.Site.Name != null)
						{
							arrayList.Add(component.Site.Name);
						}
					}
					this._savedSelection = arrayList;
				}
				this.Unload();
				this.BeginLoad(this._loader);
			}
		}

		bool IDesignerLoaderHost2.IgnoreErrorsDuringReload
		{
			get
			{
				return this._ignoreErrorsDuringReload;
			}
			set
			{
				if (!value || ((IDesignerLoaderHost2)this).CanReloadWithErrors)
				{
					this._ignoreErrorsDuringReload = value;
				}
			}
		}

		bool IDesignerLoaderHost2.CanReloadWithErrors
		{
			get
			{
				return this._canReloadWithErrors;
			}
			set
			{
				this._canReloadWithErrors = value;
			}
		}

		MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
		{
			return typeof(IDesignerHost).GetMethod(name, bindingAttr, binder, types, modifiers);
		}

		MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr)
		{
			return typeof(IDesignerHost).GetMethod(name, bindingAttr);
		}

		MethodInfo[] IReflect.GetMethods(BindingFlags bindingAttr)
		{
			return typeof(IDesignerHost).GetMethods(bindingAttr);
		}

		FieldInfo IReflect.GetField(string name, BindingFlags bindingAttr)
		{
			return typeof(IDesignerHost).GetField(name, bindingAttr);
		}

		FieldInfo[] IReflect.GetFields(BindingFlags bindingAttr)
		{
			return typeof(IDesignerHost).GetFields(bindingAttr);
		}

		PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr)
		{
			return typeof(IDesignerHost).GetProperty(name, bindingAttr);
		}

		PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			return typeof(IDesignerHost).GetProperty(name, bindingAttr, binder, returnType, types, modifiers);
		}

		PropertyInfo[] IReflect.GetProperties(BindingFlags bindingAttr)
		{
			return typeof(IDesignerHost).GetProperties(bindingAttr);
		}

		MemberInfo[] IReflect.GetMember(string name, BindingFlags bindingAttr)
		{
			return typeof(IDesignerHost).GetMember(name, bindingAttr);
		}

		MemberInfo[] IReflect.GetMembers(BindingFlags bindingAttr)
		{
			return typeof(IDesignerHost).GetMembers(bindingAttr);
		}

		object IReflect.InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			return typeof(IDesignerHost).InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
		}

		Type IReflect.UnderlyingSystemType
		{
			get
			{
				return typeof(IDesignerHost).UnderlyingSystemType;
			}
		}

		void IServiceContainer.AddService(Type serviceType, object serviceInstance)
		{
			IServiceContainer serviceContainer = this.GetService(typeof(IServiceContainer)) as IServiceContainer;
			if (serviceContainer == null)
			{
				throw new ObjectDisposedException("IServiceContainer");
			}
			serviceContainer.AddService(serviceType, serviceInstance);
		}

		void IServiceContainer.AddService(Type serviceType, object serviceInstance, bool promote)
		{
			IServiceContainer serviceContainer = this.GetService(typeof(IServiceContainer)) as IServiceContainer;
			if (serviceContainer == null)
			{
				throw new ObjectDisposedException("IServiceContainer");
			}
			serviceContainer.AddService(serviceType, serviceInstance, promote);
		}

		void IServiceContainer.AddService(Type serviceType, ServiceCreatorCallback callback)
		{
			IServiceContainer serviceContainer = this.GetService(typeof(IServiceContainer)) as IServiceContainer;
			if (serviceContainer == null)
			{
				throw new ObjectDisposedException("IServiceContainer");
			}
			serviceContainer.AddService(serviceType, callback);
		}

		void IServiceContainer.AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
		{
			IServiceContainer serviceContainer = this.GetService(typeof(IServiceContainer)) as IServiceContainer;
			if (serviceContainer == null)
			{
				throw new ObjectDisposedException("IServiceContainer");
			}
			serviceContainer.AddService(serviceType, callback, promote);
		}

		void IServiceContainer.RemoveService(Type serviceType)
		{
			IServiceContainer serviceContainer = this.GetService(typeof(IServiceContainer)) as IServiceContainer;
			if (serviceContainer == null)
			{
				throw new ObjectDisposedException("IServiceContainer");
			}
			serviceContainer.RemoveService(serviceType);
		}

		void IServiceContainer.RemoveService(Type serviceType, bool promote)
		{
			IServiceContainer serviceContainer = this.GetService(typeof(IServiceContainer)) as IServiceContainer;
			if (serviceContainer == null)
			{
				throw new ObjectDisposedException("IServiceContainer");
			}
			serviceContainer.RemoveService(serviceType, promote);
		}

		object IServiceProvider.GetService(Type serviceType)
		{
			return this.GetService(serviceType);
		}

		private static readonly int StateLoading = BitVector32.CreateMask();

		private static readonly int StateUnloading = BitVector32.CreateMask(DesignerHost.StateLoading);

		private static readonly int StateIsClosingTransaction = BitVector32.CreateMask(DesignerHost.StateUnloading);

		private static Type[] DefaultServices = new Type[]
		{
			typeof(IDesignerHost),
			typeof(IContainer),
			typeof(IComponentChangeService),
			typeof(IDesignerLoaderHost2)
		};

		private static readonly object EventActivated = new object();

		private static readonly object EventDeactivated = new object();

		private static readonly object EventLoadComplete = new object();

		private static readonly object EventTransactionClosed = new object();

		private static readonly object EventTransactionClosing = new object();

		private static readonly object EventTransactionOpened = new object();

		private static readonly object EventTransactionOpening = new object();

		private static readonly object EventComponentAdding = new object();

		private static readonly object EventComponentAdded = new object();

		private static readonly object EventComponentChanging = new object();

		private static readonly object EventComponentChanged = new object();

		private static readonly object EventComponentRemoving = new object();

		private static readonly object EventComponentRemoved = new object();

		private static readonly object EventComponentRename = new object();

		private BitVector32 _state;

		private DesignSurface _surface;

		private string _newComponentName;

		private Stack _transactions;

		private IComponent _rootComponent;

		private string _rootComponentClassName;

		private Hashtable _designers;

		private EventHandlerList _events;

		private DesignerLoader _loader;

		private ICollection _savedSelection;

		private HostDesigntimeLicenseContext _licenseCtx;

		private IDesignerEventService _designerEventService;

		private static readonly object _selfLock = new object();

		private bool _ignoreErrorsDuringReload;

		private bool _canReloadWithErrors;

		private sealed class DesignerHostTransaction : DesignerTransaction
		{
			public DesignerHostTransaction(DesignerHost host, string description)
				: base(description)
			{
				this._host = host;
				if (this._host._transactions == null)
				{
					this._host._transactions = new Stack();
				}
				this._host._transactions.Push(this);
				this._host.OnTransactionOpening(EventArgs.Empty);
				this._host.OnTransactionOpened(EventArgs.Empty);
			}

			protected override void OnCancel()
			{
				if (this._host != null)
				{
					if (this._host._transactions.Peek() != this)
					{
						string description = ((DesignerTransaction)this._host._transactions.Peek()).Description;
						throw new InvalidOperationException(SR.GetString("DesignerHostNestedTransaction", new object[] { base.Description, description }));
					}
					this._host.IsClosingTransaction = true;
					try
					{
						this._host._transactions.Pop();
						DesignerTransactionCloseEventArgs designerTransactionCloseEventArgs = new DesignerTransactionCloseEventArgs(false, this._host._transactions.Count == 0);
						this._host.OnTransactionClosing(designerTransactionCloseEventArgs);
						this._host.OnTransactionClosed(designerTransactionCloseEventArgs);
					}
					finally
					{
						this._host.IsClosingTransaction = false;
						this._host = null;
					}
				}
			}

			protected override void OnCommit()
			{
				if (this._host != null)
				{
					if (this._host._transactions.Peek() != this)
					{
						string description = ((DesignerTransaction)this._host._transactions.Peek()).Description;
						throw new InvalidOperationException(SR.GetString("DesignerHostNestedTransaction", new object[] { base.Description, description }));
					}
					this._host.IsClosingTransaction = true;
					try
					{
						this._host._transactions.Pop();
						DesignerTransactionCloseEventArgs designerTransactionCloseEventArgs = new DesignerTransactionCloseEventArgs(true, this._host._transactions.Count == 0);
						this._host.OnTransactionClosing(designerTransactionCloseEventArgs);
						this._host.OnTransactionClosed(designerTransactionCloseEventArgs);
					}
					finally
					{
						this._host.IsClosingTransaction = false;
						this._host = null;
					}
				}
			}

			private DesignerHost _host;
		}

		internal class Site : ISite, IServiceContainer, IServiceProvider, IDictionaryService
		{
			internal Site(IComponent component, DesignerHost host, string name, Container container)
			{
				this._component = component;
				this._host = host;
				this._name = name;
				this._container = container;
			}

			private IServiceContainer SiteServiceContainer
			{
				get
				{
					SiteNestedContainer siteNestedContainer = ((IServiceProvider)this).GetService(typeof(INestedContainer)) as SiteNestedContainer;
					return siteNestedContainer.GetServiceInternal(typeof(IServiceContainer)) as IServiceContainer;
				}
			}

			object IDictionaryService.GetKey(object value)
			{
				if (this._dictionary != null)
				{
					foreach (object obj in this._dictionary)
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
						object value2 = dictionaryEntry.Value;
						if (value != null && value.Equals(value2))
						{
							return dictionaryEntry.Key;
						}
					}
				}
				return null;
			}

			object IDictionaryService.GetValue(object key)
			{
				if (this._dictionary != null)
				{
					return this._dictionary[key];
				}
				return null;
			}

			void IDictionaryService.SetValue(object key, object value)
			{
				if (this._dictionary == null)
				{
					this._dictionary = new Hashtable();
				}
				if (value == null)
				{
					this._dictionary.Remove(key);
					return;
				}
				this._dictionary[key] = value;
			}

			void IServiceContainer.AddService(Type serviceType, object serviceInstance)
			{
				this.SiteServiceContainer.AddService(serviceType, serviceInstance);
			}

			void IServiceContainer.AddService(Type serviceType, object serviceInstance, bool promote)
			{
				this.SiteServiceContainer.AddService(serviceType, serviceInstance, promote);
			}

			void IServiceContainer.AddService(Type serviceType, ServiceCreatorCallback callback)
			{
				this.SiteServiceContainer.AddService(serviceType, callback);
			}

			void IServiceContainer.AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
			{
				this.SiteServiceContainer.AddService(serviceType, callback, promote);
			}

			void IServiceContainer.RemoveService(Type serviceType)
			{
				this.SiteServiceContainer.RemoveService(serviceType);
			}

			void IServiceContainer.RemoveService(Type serviceType, bool promote)
			{
				this.SiteServiceContainer.RemoveService(serviceType, promote);
			}

			object IServiceProvider.GetService(Type service)
			{
				if (service == null)
				{
					throw new ArgumentNullException("service");
				}
				if (service == typeof(IDictionaryService))
				{
					return this;
				}
				if (service == typeof(INestedContainer))
				{
					if (this._nestedContainer == null)
					{
						this._nestedContainer = new SiteNestedContainer(this._component, null, this._host);
					}
					return this._nestedContainer;
				}
				if (service != typeof(IServiceContainer) && service != typeof(IContainer) && this._nestedContainer != null)
				{
					return this._nestedContainer.GetServiceInternal(service);
				}
				return this._host.GetService(service);
			}

			IComponent ISite.Component
			{
				get
				{
					return this._component;
				}
			}

			IContainer ISite.Container
			{
				get
				{
					return this._container;
				}
			}

			bool ISite.DesignMode
			{
				get
				{
					return true;
				}
			}

			internal bool Disposed
			{
				get
				{
					return this._disposed;
				}
				set
				{
					this._disposed = value;
				}
			}

			string ISite.Name
			{
				get
				{
					return this._name;
				}
				set
				{
					if (value == null)
					{
						value = string.Empty;
					}
					if (this._name != value)
					{
						bool flag = true;
						if (value.Length > 0)
						{
							IComponent component = this._container.Components[value];
							flag = this._component != component;
							if (component != null && flag)
							{
								throw new Exception(SR.GetString("DesignerHostDuplicateName", new object[] { value }))
								{
									HelpLink = "DesignerHostDuplicateName"
								};
							}
						}
						if (flag)
						{
							INameCreationService nameCreationService = (INameCreationService)((IServiceProvider)this).GetService(typeof(INameCreationService));
							if (nameCreationService != null)
							{
								nameCreationService.ValidateName(value);
							}
						}
						string name = this._name;
						this._name = value;
						this._host.OnComponentRename(this._component, name, this._name);
					}
				}
			}

			private IComponent _component;

			private Hashtable _dictionary;

			private DesignerHost _host;

			private string _name;

			private bool _disposed;

			private SiteNestedContainer _nestedContainer;

			private Container _container;
		}
	}
}
