using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x0200029C RID: 668
	internal sealed class ComponentManagerBroker : MarshalByRefObject
	{
		// Token: 0x060023F8 RID: 9208 RVA: 0x00052708 File Offset: 0x00051708
		static ComponentManagerBroker()
		{
			int currentProcessId = SafeNativeMethods.GetCurrentProcessId();
			ComponentManagerBroker._syncObject = new object();
			ComponentManagerBroker._remoteObjectName = string.Format(CultureInfo.CurrentCulture, "ComponentManagerBroker.{0}.{1:X}", new object[]
			{
				Application.WindowsFormsVersion,
				currentProcessId
			});
		}

		// Token: 0x060023F9 RID: 9209 RVA: 0x00052752 File Offset: 0x00051752
		public ComponentManagerBroker()
		{
			if (ComponentManagerBroker._broker == null)
			{
				ComponentManagerBroker._broker = this;
			}
		}

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x060023FA RID: 9210 RVA: 0x00052767 File Offset: 0x00051767
		internal ComponentManagerBroker Singleton
		{
			get
			{
				return ComponentManagerBroker._broker;
			}
		}

		// Token: 0x060023FB RID: 9211 RVA: 0x0005276E File Offset: 0x0005176E
		internal void ClearComponentManager()
		{
			this._proxy = null;
		}

		// Token: 0x060023FC RID: 9212 RVA: 0x00052777 File Offset: 0x00051777
		public override object InitializeLifetimeService()
		{
			return null;
		}

		// Token: 0x060023FD RID: 9213 RVA: 0x0005277C File Offset: 0x0005177C
		public UnsafeNativeMethods.IMsoComponentManager GetProxy(long pCM)
		{
			if (this._proxy == null)
			{
				UnsafeNativeMethods.IMsoComponentManager msoComponentManager = (UnsafeNativeMethods.IMsoComponentManager)Marshal.GetObjectForIUnknown((IntPtr)pCM);
				this._proxy = new ComponentManagerProxy(this, msoComponentManager);
			}
			return this._proxy;
		}

		// Token: 0x060023FE RID: 9214 RVA: 0x000527B8 File Offset: 0x000517B8
		internal static UnsafeNativeMethods.IMsoComponentManager GetComponentManager(IntPtr pOriginal)
		{
			lock (ComponentManagerBroker._syncObject)
			{
				if (ComponentManagerBroker._broker == null)
				{
					UnsafeNativeMethods.ICorRuntimeHost corRuntimeHost = (UnsafeNativeMethods.ICorRuntimeHost)new UnsafeNativeMethods.CorRuntimeHost();
					object obj;
					corRuntimeHost.GetDefaultDomain(out obj);
					AppDomain appDomain = obj as AppDomain;
					if (appDomain == null)
					{
						appDomain = AppDomain.CurrentDomain;
					}
					if (appDomain == AppDomain.CurrentDomain)
					{
						ComponentManagerBroker._broker = new ComponentManagerBroker();
					}
					else
					{
						ComponentManagerBroker._broker = ComponentManagerBroker.GetRemotedComponentManagerBroker(appDomain);
					}
				}
			}
			return ComponentManagerBroker._broker.GetProxy((long)pOriginal);
		}

		// Token: 0x060023FF RID: 9215 RVA: 0x00052844 File Offset: 0x00051844
		private static ComponentManagerBroker GetRemotedComponentManagerBroker(AppDomain domain)
		{
			Type typeFromHandle = typeof(ComponentManagerBroker);
			ComponentManagerBroker componentManagerBroker = (ComponentManagerBroker)domain.CreateInstanceAndUnwrap(typeFromHandle.Assembly.FullName, typeFromHandle.FullName);
			return componentManagerBroker.Singleton;
		}

		// Token: 0x04001594 RID: 5524
		private static object _syncObject;

		// Token: 0x04001595 RID: 5525
		private static string _remoteObjectName;

		// Token: 0x04001596 RID: 5526
		private static ComponentManagerBroker _broker;

		// Token: 0x04001597 RID: 5527
		[ThreadStatic]
		private ComponentManagerProxy _proxy;
	}
}
