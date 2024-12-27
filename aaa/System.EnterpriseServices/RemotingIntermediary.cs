using System;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Security.Permissions;

namespace System.EnterpriseServices
{
	// Token: 0x0200002F RID: 47
	internal class RemotingIntermediary : RealProxy
	{
		// Token: 0x060000E4 RID: 228 RVA: 0x00004578 File Offset: 0x00003578
		internal RemotingIntermediary(RealProxy pxy)
			: base(pxy.GetProxiedType())
		{
			this._pxy = pxy;
			this._blind = new BlindMBRO((MarshalByRefObject)this.GetTransparentProxy());
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x000045A3 File Offset: 0x000035A3
		public override IntPtr GetCOMIUnknown(bool fIsMarshalled)
		{
			return this._pxy.GetCOMIUnknown(fIsMarshalled);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x000045B1 File Offset: 0x000035B1
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public override void SetCOMIUnknown(IntPtr pUnk)
		{
			this._pxy.SetCOMIUnknown(pUnk);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000045BF File Offset: 0x000035BF
		public override ObjRef CreateObjRef(Type requestedType)
		{
			return new IntermediaryObjRef((MarshalByRefObject)this.GetTransparentProxy(), requestedType, this._pxy);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x000045D8 File Offset: 0x000035D8
		private IMessage HandleSpecialMessages(IMessage reqmsg)
		{
			IMethodCallMessage methodCallMessage = reqmsg as IMethodCallMessage;
			MethodBase methodBase = methodCallMessage.MethodBase;
			if (methodBase == RemotingIntermediary._initializeLifetimeServiceMethod)
			{
				return new ReturnMessage(this._blind.InitializeLifetimeService(), null, 0, methodCallMessage.LogicalCallContext, methodCallMessage);
			}
			if (methodBase == RemotingIntermediary._getLifetimeServiceMethod)
			{
				return new ReturnMessage(this._blind.GetLifetimeService(), null, 0, methodCallMessage.LogicalCallContext, methodCallMessage);
			}
			return null;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00004638 File Offset: 0x00003638
		public override IMessage Invoke(IMessage reqmsg)
		{
			IMessage message = this.HandleSpecialMessages(reqmsg);
			if (message != null)
			{
				return message;
			}
			return this._pxy.Invoke(reqmsg);
		}

		// Token: 0x04000064 RID: 100
		private static MethodInfo _initializeLifetimeServiceMethod = typeof(MarshalByRefObject).GetMethod("InitializeLifetimeService", new Type[0]);

		// Token: 0x04000065 RID: 101
		private static MethodInfo _getLifetimeServiceMethod = typeof(MarshalByRefObject).GetMethod("GetLifetimeService", new Type[0]);

		// Token: 0x04000066 RID: 102
		private static MethodInfo _getCOMIUnknownMethod = typeof(MarshalByRefObject).GetMethod("GetComIUnknown", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(bool) }, null);

		// Token: 0x04000067 RID: 103
		private static MethodInfo _setCOMIUnknownMethod = typeof(ServicedComponent).GetMethod("DoSetCOMIUnknown", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04000068 RID: 104
		private RealProxy _pxy;

		// Token: 0x04000069 RID: 105
		private BlindMBRO _blind;
	}
}
