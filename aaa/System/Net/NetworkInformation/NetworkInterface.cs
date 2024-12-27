using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200061C RID: 1564
	public abstract class NetworkInterface
	{
		// Token: 0x06003023 RID: 12323 RVA: 0x000CFF0F File Offset: 0x000CEF0F
		public static NetworkInterface[] GetAllNetworkInterfaces()
		{
			new NetworkInformationPermission(NetworkInformationAccess.Read).Demand();
			return SystemNetworkInterface.GetNetworkInterfaces();
		}

		// Token: 0x06003024 RID: 12324 RVA: 0x000CFF21 File Offset: 0x000CEF21
		public static bool GetIsNetworkAvailable()
		{
			return SystemNetworkInterface.InternalGetIsNetworkAvailable();
		}

		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x06003025 RID: 12325 RVA: 0x000CFF28 File Offset: 0x000CEF28
		public static int LoopbackInterfaceIndex
		{
			get
			{
				return SystemNetworkInterface.InternalLoopbackInterfaceIndex;
			}
		}

		// Token: 0x17000A72 RID: 2674
		// (get) Token: 0x06003026 RID: 12326
		public abstract string Id { get; }

		// Token: 0x17000A73 RID: 2675
		// (get) Token: 0x06003027 RID: 12327
		public abstract string Name { get; }

		// Token: 0x17000A74 RID: 2676
		// (get) Token: 0x06003028 RID: 12328
		public abstract string Description { get; }

		// Token: 0x06003029 RID: 12329
		public abstract IPInterfaceProperties GetIPProperties();

		// Token: 0x0600302A RID: 12330
		public abstract IPv4InterfaceStatistics GetIPv4Statistics();

		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x0600302B RID: 12331
		public abstract OperationalStatus OperationalStatus { get; }

		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x0600302C RID: 12332
		public abstract long Speed { get; }

		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x0600302D RID: 12333
		public abstract bool IsReceiveOnly { get; }

		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x0600302E RID: 12334
		public abstract bool SupportsMulticast { get; }

		// Token: 0x0600302F RID: 12335
		public abstract PhysicalAddress GetPhysicalAddress();

		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x06003030 RID: 12336
		public abstract NetworkInterfaceType NetworkInterfaceType { get; }

		// Token: 0x06003031 RID: 12337
		public abstract bool Supports(NetworkInterfaceComponent networkInterfaceComponent);
	}
}
