using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Contexts;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Proxies
{
	// Token: 0x02000723 RID: 1827
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	public class ProxyAttribute : Attribute, IContextAttribute
	{
		// Token: 0x060041EA RID: 16874 RVA: 0x000E109F File Offset: 0x000E009F
		public virtual MarshalByRefObject CreateInstance(Type serverType)
		{
			if (!serverType.IsContextful)
			{
				throw new RemotingException(Environment.GetResourceString("Remoting_Activation_MBR_ProxyAttribute"));
			}
			if (serverType.IsAbstract)
			{
				throw new RemotingException(Environment.GetResourceString("Acc_CreateAbst"));
			}
			return this.CreateInstanceInternal(serverType);
		}

		// Token: 0x060041EB RID: 16875 RVA: 0x000E10D8 File Offset: 0x000E00D8
		internal MarshalByRefObject CreateInstanceInternal(Type serverType)
		{
			return ActivationServices.CreateInstance(serverType);
		}

		// Token: 0x060041EC RID: 16876 RVA: 0x000E10E0 File Offset: 0x000E00E0
		public virtual RealProxy CreateProxy(ObjRef objRef, Type serverType, object serverObject, Context serverContext)
		{
			RemotingProxy remotingProxy = new RemotingProxy(serverType);
			if (serverContext != null)
			{
				RealProxy.SetStubData(remotingProxy, serverContext.InternalContextID);
			}
			if (objRef != null && objRef.GetServerIdentity().IsAllocated)
			{
				remotingProxy.SetSrvInfo(objRef.GetServerIdentity(), objRef.GetDomainID());
			}
			remotingProxy.Initialized = true;
			if (!serverType.IsContextful && !serverType.IsMarshalByRef && serverContext != null)
			{
				throw new RemotingException(Environment.GetResourceString("Remoting_Activation_MBR_ProxyAttribute"));
			}
			return remotingProxy;
		}

		// Token: 0x060041ED RID: 16877 RVA: 0x000E115D File Offset: 0x000E015D
		[ComVisible(true)]
		public bool IsContextOK(Context ctx, IConstructionCallMessage msg)
		{
			return true;
		}

		// Token: 0x060041EE RID: 16878 RVA: 0x000E1160 File Offset: 0x000E0160
		[ComVisible(true)]
		public void GetPropertiesForNewContext(IConstructionCallMessage msg)
		{
		}
	}
}
