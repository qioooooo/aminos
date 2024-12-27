using System;
using System.EnterpriseServices.Thunk;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Services;

namespace System.EnterpriseServices
{
	// Token: 0x02000037 RID: 55
	internal sealed class ComponentServices
	{
		// Token: 0x06000108 RID: 264 RVA: 0x00004DCC File Offset: 0x00003DCC
		public static byte[] GetDCOMBuffer(object o)
		{
			int marshalSize = Proxy.GetMarshalSize(o);
			if (marshalSize == -1)
			{
				throw new RemotingException(Resource.FormatString("Remoting_InteropError"));
			}
			byte[] array = new byte[marshalSize];
			if (!Proxy.MarshalObject(o, array, marshalSize))
			{
				throw new RemotingException(Resource.FormatString("Remoting_InteropError"));
			}
			return array;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00004E16 File Offset: 0x00003E16
		internal static void InitializeRemotingChannels()
		{
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00004E18 File Offset: 0x00003E18
		public static void DeactivateObject(object otp, bool disposing)
		{
			RealProxy realProxy = RemotingServices.GetRealProxy(otp);
			ServicedComponentProxy servicedComponentProxy = realProxy as ServicedComponentProxy;
			if (!servicedComponentProxy.IsProxyDeactivated)
			{
				if (servicedComponentProxy.IsObjectPooled)
				{
					ComponentServices.ReconnectForPooling(servicedComponentProxy);
				}
				servicedComponentProxy.DeactivateProxy(disposing);
			}
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00004E50 File Offset: 0x00003E50
		private static void ReconnectForPooling(ServicedComponentProxy scp)
		{
			Type proxiedType = scp.GetProxiedType();
			bool isJitActivated = scp.IsJitActivated;
			bool isObjectPooled = scp.IsObjectPooled;
			bool areMethodsSecure = scp.AreMethodsSecure;
			ProxyTearoff proxyTearoff = null;
			ServicedComponent servicedComponent = scp.DisconnectForPooling(ref proxyTearoff);
			ServicedComponentProxy servicedComponentProxy = new ServicedComponentProxy(proxiedType, isJitActivated, isObjectPooled, areMethodsSecure, false);
			servicedComponentProxy.ConnectForPooling(scp, servicedComponent, proxyTearoff, false);
			EnterpriseServicesHelper.SwitchWrappers(scp, servicedComponentProxy);
			if (proxyTearoff != null)
			{
				Marshal.ChangeWrapperHandleStrength(proxyTearoff, false);
			}
			Marshal.ChangeWrapperHandleStrength(servicedComponentProxy.GetTransparentProxy(), false);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00004EC0 File Offset: 0x00003EC0
		private ComponentServices()
		{
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00004EC8 File Offset: 0x00003EC8
		internal static string ConvertToString(IMessage reqMsg)
		{
			ComponentSerializer componentSerializer = ComponentSerializer.Get();
			long num;
			byte[] array = componentSerializer.MarshalToBuffer(reqMsg, out num);
			string @string = ComponentServices.GetString(array, (int)num);
			componentSerializer.Release();
			return @string;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00004EF8 File Offset: 0x00003EF8
		internal static IMessage ConvertToMessage(string s, object tp)
		{
			ComponentSerializer componentSerializer = ComponentSerializer.Get();
			byte[] bytes = ComponentServices.GetBytes(s);
			IMessage message = (IMessage)componentSerializer.UnmarshalFromBuffer(bytes, tp);
			componentSerializer.Release();
			return message;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00004F28 File Offset: 0x00003F28
		internal static IMessage ConvertToReturnMessage(string s, IMessage mcMsg)
		{
			ComponentSerializer componentSerializer = ComponentSerializer.Get();
			byte[] bytes = ComponentServices.GetBytes(s);
			IMessage message = (IMessage)componentSerializer.UnmarshalReturnMessageFromBuffer(bytes, (IMethodCallMessage)mcMsg);
			componentSerializer.Release();
			return message;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00004F5C File Offset: 0x00003F5C
		internal unsafe static string GetString(byte[] bytes, int count)
		{
			fixed (byte* ptr = bytes)
			{
				return Marshal.PtrToStringUni((IntPtr)((void*)ptr), count / 2);
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00004F94 File Offset: 0x00003F94
		internal unsafe static byte[] GetBytes(string s)
		{
			int num = s.Length * 2;
			IntPtr zero = IntPtr.Zero;
			fixed (char* ptr = s.ToCharArray())
			{
				byte[] array = new byte[num];
				Marshal.Copy((IntPtr)((void*)ptr), array, 0, num);
				return array;
			}
		}
	}
}
