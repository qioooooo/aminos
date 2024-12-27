using System;
using System.Collections;
using System.EnterpriseServices.Thunk;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace System.EnterpriseServices
{
	// Token: 0x02000024 RID: 36
	[ServicedComponentProxy]
	[Serializable]
	public abstract class ServicedComponent : ContextBoundObject, IRemoteDispatch, IDisposable, IManagedObject, IServicedComponentInfo
	{
		// Token: 0x0600006D RID: 109 RVA: 0x000021E0 File Offset: 0x000011E0
		public ServicedComponent()
		{
			ServicedComponentProxy servicedComponentProxy = RemotingServices.GetRealProxy(this) as ServicedComponentProxy;
			servicedComponentProxy.SuppressFinalizeServer();
			Type type = base.GetType();
			this._denyRemoteDispatch = ServicedComponentInfo.AreMethodsSecure(type);
			bool flag = false;
			this._finalize = ServicedComponent._finalizeCache.Get(type, out flag) as MethodInfo;
			if (!flag)
			{
				this._finalize = ServicedComponent.GetDeclaredFinalizer(type);
				ServicedComponent._finalizeCache.Put(type, this._finalize);
			}
			this._calledDispose = false;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x0000225C File Offset: 0x0000125C
		void IServicedComponentInfo.GetComponentInfo(ref int infoMask, out string[] infoArray)
		{
			int num = 0;
			ArrayList arrayList = new ArrayList();
			if ((infoMask & Proxy.INFO_PROCESSID) != 0)
			{
				arrayList.Add(RemotingConfiguration.ProcessId);
				num |= Proxy.INFO_PROCESSID;
			}
			if ((infoMask & Proxy.INFO_APPDOMAINID) != 0)
			{
				arrayList.Add(RemotingConfiguration.ApplicationId);
				num |= Proxy.INFO_APPDOMAINID;
			}
			if ((infoMask & Proxy.INFO_URI) != 0)
			{
				string text = RemotingServices.GetObjectUri(this);
				if (text == null)
				{
					RemotingServices.Marshal(this);
					text = RemotingServices.GetObjectUri(this);
				}
				arrayList.Add(text);
				num |= Proxy.INFO_URI;
			}
			infoArray = (string[])arrayList.ToArray(typeof(string));
			infoMask = num;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000022FC File Offset: 0x000012FC
		private static MethodInfo GetDeclaredFinalizer(Type t)
		{
			MethodInfo methodInfo = null;
			while (t != ServicedComponent._typeofSC)
			{
				methodInfo = t.GetMethod("Finalize", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic);
				if (methodInfo != null)
				{
					break;
				}
				t = t.BaseType;
			}
			return methodInfo;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00002334 File Offset: 0x00001334
		public static void DisposeObject(ServicedComponent sc)
		{
			RealProxy realProxy = RemotingServices.GetRealProxy(sc);
			if (realProxy is ServicedComponentProxy)
			{
				ServicedComponentProxy servicedComponentProxy = (ServicedComponentProxy)realProxy;
				RemotingServices.Disconnect(sc);
				servicedComponentProxy.Dispose(true);
				return;
			}
			if (realProxy is RemoteServicedComponentProxy)
			{
				RemoteServicedComponentProxy remoteServicedComponentProxy = (RemoteServicedComponentProxy)realProxy;
				sc.Dispose();
				remoteServicedComponentProxy.Dispose(true);
				return;
			}
			sc.Dispose();
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00002389 File Offset: 0x00001389
		protected internal virtual void Activate()
		{
		}

		// Token: 0x06000072 RID: 114 RVA: 0x0000238B File Offset: 0x0000138B
		protected internal virtual void Deactivate()
		{
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000238D File Offset: 0x0000138D
		protected internal virtual bool CanBePooled()
		{
			return false;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00002390 File Offset: 0x00001390
		protected internal virtual void Construct(string s)
		{
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00002394 File Offset: 0x00001394
		[AutoComplete(true)]
		string IRemoteDispatch.RemoteDispatchAutoDone(string s)
		{
			bool flag = false;
			string text = this.RemoteDispatchHelper(s, out flag);
			if (flag)
			{
				ContextUtil.SetAbort();
			}
			return text;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000023B8 File Offset: 0x000013B8
		[AutoComplete(false)]
		string IRemoteDispatch.RemoteDispatchNotAutoDone(string s)
		{
			bool flag = false;
			return this.RemoteDispatchHelper(s, out flag);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000023D0 File Offset: 0x000013D0
		private void CheckMethodAccess(IMessage request)
		{
			IMethodMessage methodMessage = request as IMethodMessage;
			if (methodMessage == null)
			{
				throw new UnauthorizedAccessException();
			}
			MethodBase methodBase = methodMessage.MethodBase;
			MethodBase methodBase2 = ReflectionCache.ConvertToClassMI(base.GetType(), methodBase) as MethodBase;
			if (methodBase2 == null)
			{
				throw new UnauthorizedAccessException();
			}
			if (ServicedComponentInfo.HasSpecialMethodAttributes(methodBase2))
			{
				throw new UnauthorizedAccessException(Resource.FormatString("ServicedComponentException_SecurityMapping"));
			}
			if ((!methodBase.IsPublic || methodBase.IsStatic) && !ServicedComponent.IsMethodAllowedRemotely(methodBase))
			{
				throw new UnauthorizedAccessException(Resource.FormatString("ServicedComponentException_SecurityNoPrivateAccess"));
			}
			Type type = methodBase.DeclaringType;
			if (!type.IsPublic && !type.IsNestedPublic)
			{
				throw new UnauthorizedAccessException(Resource.FormatString("ServicedComponentException_SecurityNoPrivateAccess"));
			}
			for (type = methodBase.DeclaringType.DeclaringType; type != null; type = type.DeclaringType)
			{
				if (!type.IsPublic && !type.IsNestedPublic)
				{
					throw new UnauthorizedAccessException(Resource.FormatString("ServicedComponentException_SecurityNoPrivateAccess"));
				}
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000024B4 File Offset: 0x000014B4
		internal static bool IsMethodAllowedRemotely(MethodBase method)
		{
			if (ServicedComponent.s_mbFieldGetter == null)
			{
				ServicedComponent.s_mbFieldGetter = typeof(object).GetMethod("FieldGetter", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			if (ServicedComponent.s_mbFieldSetter == null)
			{
				ServicedComponent.s_mbFieldSetter = typeof(object).GetMethod("FieldSetter", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			if (ServicedComponent.s_mbIsInstanceOfType == null)
			{
				ServicedComponent.s_mbIsInstanceOfType = typeof(MarshalByRefObject).GetMethod("IsInstanceOfType", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return method == ServicedComponent.s_mbFieldGetter || method == ServicedComponent.s_mbFieldSetter || method == ServicedComponent.s_mbIsInstanceOfType;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00002544 File Offset: 0x00001544
		private string RemoteDispatchHelper(string s, out bool failed)
		{
			if (this._denyRemoteDispatch)
			{
				throw new UnauthorizedAccessException(Resource.FormatString("ServicedComponentException_SecurityMapping"));
			}
			IMessage message = ComponentServices.ConvertToMessage(s, this);
			this.CheckMethodAccess(message);
			RealProxy realProxy = RemotingServices.GetRealProxy(this);
			IMessage message2 = realProxy.Invoke(message);
			IMethodReturnMessage methodReturnMessage = message2 as IMethodReturnMessage;
			if (methodReturnMessage != null && methodReturnMessage.Exception != null)
			{
				failed = true;
			}
			else
			{
				failed = false;
			}
			return ComponentServices.ConvertToString(message2);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000025AB File Offset: 0x000015AB
		public void Dispose()
		{
			ServicedComponent.DisposeObject(this);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000025B3 File Offset: 0x000015B3
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000025B5 File Offset: 0x000015B5
		internal void _callFinalize(bool disposing)
		{
			if (!this._calledDispose)
			{
				this._calledDispose = true;
				this.Dispose(disposing);
			}
			if (this._finalize != null)
			{
				this._finalize.Invoke(this, new object[0]);
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000025E8 File Offset: 0x000015E8
		internal void _internalDeactivate(bool disposing)
		{
			ComponentServices.DeactivateObject(this, disposing);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000025F4 File Offset: 0x000015F4
		internal void DoSetCOMIUnknown(IntPtr pUnk)
		{
			RealProxy realProxy = RemotingServices.GetRealProxy(this);
			realProxy.SetCOMIUnknown(pUnk);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0000260F File Offset: 0x0000160F
		void IManagedObject.GetSerializedBuffer(ref string s)
		{
			throw new NotSupportedException(Resource.GetString("Err_IManagedObjectGetSerializedBuffer"));
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00002620 File Offset: 0x00001620
		void IManagedObject.GetObjectIdentity(ref string s, ref int AppDomainID, ref int ccw)
		{
			throw new NotSupportedException(Resource.GetString("Err_IManagedObjectGetObjectIdentity"));
		}

		// Token: 0x0400001F RID: 31
		private const string c_strFieldGetterName = "FieldGetter";

		// Token: 0x04000020 RID: 32
		private const string c_strFieldSetterName = "FieldSetter";

		// Token: 0x04000021 RID: 33
		private const string c_strIsInstanceOfTypeName = "IsInstanceOfType";

		// Token: 0x04000022 RID: 34
		private const BindingFlags bfLookupAll = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

		// Token: 0x04000023 RID: 35
		private bool _denyRemoteDispatch;

		// Token: 0x04000024 RID: 36
		private MethodInfo _finalize;

		// Token: 0x04000025 RID: 37
		private bool _calledDispose;

		// Token: 0x04000026 RID: 38
		private static RWHashTableEx _finalizeCache = new RWHashTableEx();

		// Token: 0x04000027 RID: 39
		private static Type _typeofSC = typeof(ServicedComponent);

		// Token: 0x04000028 RID: 40
		private static MethodBase s_mbFieldGetter;

		// Token: 0x04000029 RID: 41
		private static MethodBase s_mbFieldSetter;

		// Token: 0x0400002A RID: 42
		private static MethodBase s_mbIsInstanceOfType;
	}
}
