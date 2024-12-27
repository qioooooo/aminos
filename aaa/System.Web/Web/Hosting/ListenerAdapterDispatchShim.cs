using System;
using System.Runtime.InteropServices;

namespace System.Web.Hosting
{
	// Token: 0x020002BB RID: 699
	internal sealed class ListenerAdapterDispatchShim : MarshalByRefObject, IRegisteredObject
	{
		// Token: 0x0600242B RID: 9259 RVA: 0x0009B150 File Offset: 0x0009A150
		void IRegisteredObject.Stop(bool immediate)
		{
			HostingEnvironment.UnregisterObject(this);
		}

		// Token: 0x0600242C RID: 9260 RVA: 0x0009B158 File Offset: 0x0009A158
		internal void StartListenerChannel(AppDomainProtocolHandler handler, IListenerChannelCallback listenerCallback)
		{
			IListenerChannelCallback listenerChannelCallback = this.MarshalComProxy(listenerCallback);
			if (listenerChannelCallback != null && handler != null)
			{
				handler.StartListenerChannel(listenerChannelCallback);
			}
		}

		// Token: 0x0600242D RID: 9261 RVA: 0x0009B17C File Offset: 0x0009A17C
		internal IListenerChannelCallback MarshalComProxy(IListenerChannelCallback defaultDomainCallback)
		{
			IListenerChannelCallback listenerChannelCallback = null;
			IntPtr iunknownForObject = Marshal.GetIUnknownForObject(defaultDomainCallback);
			if (IntPtr.Zero == iunknownForObject)
			{
				return null;
			}
			IntPtr zero = IntPtr.Zero;
			try
			{
				Guid guid = typeof(IListenerChannelCallback).GUID;
				int num = Marshal.QueryInterface(iunknownForObject, ref guid, out zero);
				if (num < 0)
				{
					Marshal.ThrowExceptionForHR(num);
				}
				listenerChannelCallback = (IListenerChannelCallback)Marshal.GetObjectForIUnknown(zero);
			}
			finally
			{
				if (IntPtr.Zero != zero)
				{
					Marshal.Release(zero);
				}
				if (IntPtr.Zero != iunknownForObject)
				{
					Marshal.Release(iunknownForObject);
				}
			}
			return listenerChannelCallback;
		}
	}
}
