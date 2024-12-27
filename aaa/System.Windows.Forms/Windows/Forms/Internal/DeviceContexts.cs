using System;

namespace System.Windows.Forms.Internal
{
	// Token: 0x0200000C RID: 12
	internal static class DeviceContexts
	{
		// Token: 0x06000047 RID: 71 RVA: 0x00002E2C File Offset: 0x00001E2C
		internal static void AddDeviceContext(DeviceContext dc)
		{
			if (DeviceContexts.activeDeviceContexts == null)
			{
				DeviceContexts.activeDeviceContexts = new ClientUtils.WeakRefCollection();
				DeviceContexts.activeDeviceContexts.RefCheckThreshold = 20;
			}
			if (!DeviceContexts.activeDeviceContexts.Contains(dc))
			{
				dc.Disposing += DeviceContexts.OnDcDisposing;
				DeviceContexts.activeDeviceContexts.Add(dc);
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002E84 File Offset: 0x00001E84
		private static void OnDcDisposing(object sender, EventArgs e)
		{
			DeviceContext deviceContext = sender as DeviceContext;
			if (deviceContext != null)
			{
				deviceContext.Disposing -= DeviceContexts.OnDcDisposing;
				DeviceContexts.RemoveDeviceContext(deviceContext);
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002EB3 File Offset: 0x00001EB3
		internal static void RemoveDeviceContext(DeviceContext dc)
		{
			if (DeviceContexts.activeDeviceContexts == null)
			{
				return;
			}
			DeviceContexts.activeDeviceContexts.RemoveByHashCode(dc);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002EC8 File Offset: 0x00001EC8
		internal static bool IsFontInUse(WindowsFont wf)
		{
			if (wf == null)
			{
				return false;
			}
			for (int i = 0; i < DeviceContexts.activeDeviceContexts.Count; i++)
			{
				DeviceContext deviceContext = DeviceContexts.activeDeviceContexts[i] as DeviceContext;
				if (deviceContext != null && (deviceContext.ActiveFont == wf || deviceContext.IsFontOnContextStack(wf)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040009A9 RID: 2473
		[ThreadStatic]
		private static ClientUtils.WeakRefCollection activeDeviceContexts;
	}
}
