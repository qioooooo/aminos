using System;
using System.EnterpriseServices.Thunk;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.EnterpriseServices
{
	// Token: 0x02000031 RID: 49
	[AttributeUsage(AttributeTargets.Class)]
	internal class ServicedComponentProxyAttribute : ProxyAttribute, ICustomFactory
	{
		// Token: 0x060000EE RID: 238 RVA: 0x00004758 File Offset: 0x00003758
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public override MarshalByRefObject CreateInstance(Type serverType)
		{
			RealProxy realProxy = null;
			MarshalByRefObject marshalByRefObject = null;
			ServicedComponentProxy.CleanupQueues(false);
			if (RemotingConfiguration.IsWellKnownClientType(serverType) != null || RemotingConfiguration.IsRemotelyActivatedClientType(serverType) != null)
			{
				marshalByRefObject = base.CreateInstance(serverType);
				realProxy = RemotingServices.GetRealProxy(marshalByRefObject);
			}
			else
			{
				bool flag = false;
				string text = "";
				bool flag2 = ServicedComponentInfo.IsTypeEventSource(serverType);
				IntPtr intPtr = Proxy.CoCreateObject(serverType, !flag2, ref flag, ref text);
				if (intPtr != IntPtr.Zero)
				{
					try
					{
						if (flag2)
						{
							realProxy = new RemoteServicedComponentProxy(serverType, intPtr, true);
							marshalByRefObject = (MarshalByRefObject)realProxy.GetTransparentProxy();
						}
						else
						{
							bool flag3 = RemotingConfiguration.IsWellKnownClientType(serverType) != null || null != RemotingConfiguration.IsRemotelyActivatedClientType(serverType);
							if (flag && !flag3)
							{
								FastRSCPObjRef fastRSCPObjRef = new FastRSCPObjRef(intPtr, serverType, text);
								marshalByRefObject = (MarshalByRefObject)RemotingServices.Unmarshal(fastRSCPObjRef);
							}
							else
							{
								marshalByRefObject = (MarshalByRefObject)Marshal.GetObjectForIUnknown(intPtr);
								if (!serverType.IsInstanceOfType(marshalByRefObject))
								{
									throw new InvalidCastException(Resource.FormatString("ServicedComponentException_UnexpectedType", serverType, marshalByRefObject.GetType()));
								}
								realProxy = RemotingServices.GetRealProxy(marshalByRefObject);
								if (!flag && !(realProxy is ServicedComponentProxy) && !(realProxy is RemoteServicedComponentProxy))
								{
									ServicedComponent servicedComponent = (ServicedComponent)marshalByRefObject;
									servicedComponent.DoSetCOMIUnknown(intPtr);
								}
							}
						}
					}
					finally
					{
						Marshal.Release(intPtr);
					}
				}
			}
			if (realProxy is ServicedComponentProxy)
			{
				ServicedComponentProxy servicedComponentProxy = (ServicedComponentProxy)realProxy;
				if (servicedComponentProxy.HomeToken == Proxy.GetCurrentContextToken())
				{
					servicedComponentProxy.FilterConstructors();
				}
			}
			return marshalByRefObject;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x000048C0 File Offset: 0x000038C0
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		MarshalByRefObject ICustomFactory.CreateInstance(Type serverType)
		{
			IdentityManager.NoticeApartment();
			RealProxy realProxy = null;
			ServicedComponentProxy.CleanupQueues(false);
			int num = ServicedComponentInfo.SCICachedLookup(serverType);
			bool flag = (num & 8) != 0;
			bool flag2 = (num & 16) != 0;
			bool flag3 = (num & 32) != 0;
			if (flag)
			{
				IntPtr currentContextToken = Proxy.GetCurrentContextToken();
				object obj = IdentityTable.FindObject(currentContextToken);
				if (obj != null)
				{
					realProxy = RemotingServices.GetRealProxy(obj);
				}
			}
			if (realProxy == null)
			{
				realProxy = new ServicedComponentProxy(serverType, flag, flag2, flag3, true);
			}
			else if (realProxy is ServicedComponentProxy)
			{
				ServicedComponentProxy servicedComponentProxy = (ServicedComponentProxy)realProxy;
				servicedComponentProxy.ConstructServer();
			}
			return (MarshalByRefObject)realProxy.GetTransparentProxy();
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x0000495C File Offset: 0x0000395C
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public override RealProxy CreateProxy(ObjRef objRef, Type serverType, object serverObject, Context serverContext)
		{
			if (objRef == null)
			{
				return base.CreateProxy(objRef, serverType, serverObject, serverContext);
			}
			if (objRef is FastRSCPObjRef || (objRef is ServicedComponentMarshaler && (!objRef.IsFromThisProcess() || ServicedComponentInfo.IsTypeEventSource(serverType))))
			{
				object realObject = objRef.GetRealObject(new StreamingContext(StreamingContextStates.Remoting));
				return RemotingServices.GetRealProxy(realObject);
			}
			return base.CreateProxy(objRef, serverType, serverObject, serverContext);
		}
	}
}
