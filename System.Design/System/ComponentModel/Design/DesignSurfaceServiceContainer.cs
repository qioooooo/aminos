﻿using System;
using System.Collections;
using System.Design;

namespace System.ComponentModel.Design
{
	internal sealed class DesignSurfaceServiceContainer : ServiceContainer
	{
		internal DesignSurfaceServiceContainer(IServiceProvider parentProvider)
			: base(parentProvider)
		{
			this.AddFixedService(typeof(DesignSurfaceServiceContainer), this);
		}

		internal void AddFixedService(Type serviceType, object serviceInstance)
		{
			base.AddService(serviceType, serviceInstance);
			if (this._fixedServices == null)
			{
				this._fixedServices = new Hashtable();
			}
			this._fixedServices[serviceType] = serviceType;
		}

		internal void RemoveFixedService(Type serviceType)
		{
			if (this._fixedServices != null)
			{
				this._fixedServices.Remove(serviceType);
			}
			base.RemoveService(serviceType);
		}

		public override void RemoveService(Type serviceType, bool promote)
		{
			if (serviceType != null && this._fixedServices != null && this._fixedServices.ContainsKey(serviceType))
			{
				throw new InvalidOperationException(SR.GetString("DesignSurfaceServiceIsFixed", new object[] { serviceType.Name }));
			}
			base.RemoveService(serviceType, promote);
		}

		private Hashtable _fixedServices;
	}
}
