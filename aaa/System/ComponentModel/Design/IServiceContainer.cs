using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	// Token: 0x0200017F RID: 383
	[ComVisible(true)]
	public interface IServiceContainer : IServiceProvider
	{
		// Token: 0x06000C44 RID: 3140
		void AddService(Type serviceType, object serviceInstance);

		// Token: 0x06000C45 RID: 3141
		void AddService(Type serviceType, object serviceInstance, bool promote);

		// Token: 0x06000C46 RID: 3142
		void AddService(Type serviceType, ServiceCreatorCallback callback);

		// Token: 0x06000C47 RID: 3143
		void AddService(Type serviceType, ServiceCreatorCallback callback, bool promote);

		// Token: 0x06000C48 RID: 3144
		void RemoveService(Type serviceType);

		// Token: 0x06000C49 RID: 3145
		void RemoveService(Type serviceType, bool promote);
	}
}
