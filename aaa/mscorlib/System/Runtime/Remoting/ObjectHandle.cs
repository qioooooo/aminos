using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Lifetime;
using System.Security.Permissions;

namespace System.Runtime.Remoting
{
	// Token: 0x02000719 RID: 1817
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDual)]
	public class ObjectHandle : MarshalByRefObject, IObjectHandle
	{
		// Token: 0x0600419E RID: 16798 RVA: 0x000E02CF File Offset: 0x000DF2CF
		private ObjectHandle()
		{
		}

		// Token: 0x0600419F RID: 16799 RVA: 0x000E02D7 File Offset: 0x000DF2D7
		public ObjectHandle(object o)
		{
			this.WrappedObject = o;
		}

		// Token: 0x060041A0 RID: 16800 RVA: 0x000E02E6 File Offset: 0x000DF2E6
		public object Unwrap()
		{
			return this.WrappedObject;
		}

		// Token: 0x060041A1 RID: 16801 RVA: 0x000E02F0 File Offset: 0x000DF2F0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public override object InitializeLifetimeService()
		{
			MarshalByRefObject marshalByRefObject = this.WrappedObject as MarshalByRefObject;
			if (marshalByRefObject != null && marshalByRefObject.InitializeLifetimeService() == null)
			{
				return null;
			}
			return (ILease)base.InitializeLifetimeService();
		}

		// Token: 0x040020C5 RID: 8389
		private object WrappedObject;
	}
}
