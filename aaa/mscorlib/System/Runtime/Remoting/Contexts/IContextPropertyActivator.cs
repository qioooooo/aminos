using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x020006B2 RID: 1714
	[ComVisible(true)]
	public interface IContextPropertyActivator
	{
		// Token: 0x06003E5E RID: 15966
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		bool IsOKToActivate(IConstructionCallMessage msg);

		// Token: 0x06003E5F RID: 15967
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void CollectFromClientContext(IConstructionCallMessage msg);

		// Token: 0x06003E60 RID: 15968
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		bool DeliverClientContextToServerContext(IConstructionCallMessage msg);

		// Token: 0x06003E61 RID: 15969
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void CollectFromServerContext(IConstructionReturnMessage msg);

		// Token: 0x06003E62 RID: 15970
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		bool DeliverServerContextToClientContext(IConstructionReturnMessage msg);
	}
}
