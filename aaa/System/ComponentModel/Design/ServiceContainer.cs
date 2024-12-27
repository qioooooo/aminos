using System;
using System.Collections;
using System.Diagnostics;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x0200019B RID: 411
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ServiceContainer : IServiceContainer, IServiceProvider, IDisposable
	{
		// Token: 0x06000CD3 RID: 3283 RVA: 0x000299C3 File Offset: 0x000289C3
		public ServiceContainer()
		{
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x000299CB File Offset: 0x000289CB
		public ServiceContainer(IServiceProvider parentProvider)
		{
			this.parentProvider = parentProvider;
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000CD5 RID: 3285 RVA: 0x000299DC File Offset: 0x000289DC
		private IServiceContainer Container
		{
			get
			{
				IServiceContainer serviceContainer = null;
				if (this.parentProvider != null)
				{
					serviceContainer = (IServiceContainer)this.parentProvider.GetService(typeof(IServiceContainer));
				}
				return serviceContainer;
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000CD6 RID: 3286 RVA: 0x00029A0F File Offset: 0x00028A0F
		protected virtual Type[] DefaultServices
		{
			get
			{
				return ServiceContainer._defaultServices;
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000CD7 RID: 3287 RVA: 0x00029A16 File Offset: 0x00028A16
		private Hashtable Services
		{
			get
			{
				if (this.services == null)
				{
					this.services = new Hashtable();
				}
				return this.services;
			}
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x00029A31 File Offset: 0x00028A31
		public void AddService(Type serviceType, object serviceInstance)
		{
			this.AddService(serviceType, serviceInstance, false);
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x00029A3C File Offset: 0x00028A3C
		public virtual void AddService(Type serviceType, object serviceInstance, bool promote)
		{
			if (promote)
			{
				IServiceContainer container = this.Container;
				if (container != null)
				{
					container.AddService(serviceType, serviceInstance, promote);
					return;
				}
			}
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			if (serviceInstance == null)
			{
				throw new ArgumentNullException("serviceInstance");
			}
			if (!(serviceInstance is ServiceCreatorCallback) && !serviceInstance.GetType().IsCOMObject && !serviceType.IsAssignableFrom(serviceInstance.GetType()))
			{
				throw new ArgumentException(SR.GetString("ErrorInvalidServiceInstance", new object[] { serviceType.FullName }));
			}
			if (this.Services.ContainsKey(serviceType))
			{
				throw new ArgumentException(SR.GetString("ErrorServiceExists", new object[] { serviceType.FullName }), "serviceType");
			}
			this.Services[serviceType] = serviceInstance;
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x00029B01 File Offset: 0x00028B01
		public void AddService(Type serviceType, ServiceCreatorCallback callback)
		{
			this.AddService(serviceType, callback, false);
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x00029B0C File Offset: 0x00028B0C
		public virtual void AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
		{
			if (promote)
			{
				IServiceContainer container = this.Container;
				if (container != null)
				{
					container.AddService(serviceType, callback, promote);
					return;
				}
			}
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			if (this.Services.ContainsKey(serviceType))
			{
				throw new ArgumentException(SR.GetString("ErrorServiceExists", new object[] { serviceType.FullName }), "serviceType");
			}
			this.Services[serviceType] = callback;
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x00029B8D File Offset: 0x00028B8D
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x00029B98 File Offset: 0x00028B98
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				Hashtable hashtable = this.services;
				this.services = null;
				if (hashtable != null)
				{
					foreach (object obj in hashtable.Values)
					{
						if (obj is IDisposable)
						{
							((IDisposable)obj).Dispose();
						}
					}
				}
			}
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x00029C0C File Offset: 0x00028C0C
		public virtual object GetService(Type serviceType)
		{
			object obj = null;
			Type[] defaultServices = this.DefaultServices;
			for (int i = 0; i < defaultServices.Length; i++)
			{
				if (serviceType == defaultServices[i])
				{
					obj = this;
					break;
				}
			}
			if (obj == null)
			{
				obj = this.Services[serviceType];
			}
			if (obj is ServiceCreatorCallback)
			{
				obj = ((ServiceCreatorCallback)obj)(this, serviceType);
				if (obj != null && !obj.GetType().IsCOMObject && !serviceType.IsAssignableFrom(obj.GetType()))
				{
					obj = null;
				}
				this.Services[serviceType] = obj;
			}
			if (obj == null && this.parentProvider != null)
			{
				obj = this.parentProvider.GetService(serviceType);
			}
			return obj;
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x00029CA6 File Offset: 0x00028CA6
		public void RemoveService(Type serviceType)
		{
			this.RemoveService(serviceType, false);
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x00029CB0 File Offset: 0x00028CB0
		public virtual void RemoveService(Type serviceType, bool promote)
		{
			if (promote)
			{
				IServiceContainer container = this.Container;
				if (container != null)
				{
					container.RemoveService(serviceType, promote);
					return;
				}
			}
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			this.Services.Remove(serviceType);
		}

		// Token: 0x04000B03 RID: 2819
		private Hashtable services;

		// Token: 0x04000B04 RID: 2820
		private IServiceProvider parentProvider;

		// Token: 0x04000B05 RID: 2821
		private static Type[] _defaultServices = new Type[]
		{
			typeof(IServiceContainer),
			typeof(ServiceContainer)
		};

		// Token: 0x04000B06 RID: 2822
		private static TraceSwitch TRACESERVICE = new TraceSwitch("TRACESERVICE", "ServiceProvider: Trace service provider requests.");
	}
}
