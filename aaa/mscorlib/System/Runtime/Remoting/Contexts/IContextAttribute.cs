using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x02000684 RID: 1668
	[ComVisible(true)]
	public interface IContextAttribute
	{
		// Token: 0x06003CE7 RID: 15591
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		bool IsContextOK(Context ctx, IConstructionCallMessage msg);

		// Token: 0x06003CE8 RID: 15592
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void GetPropertiesForNewContext(IConstructionCallMessage msg);
	}
}
