using System;
using System.Collections;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200062F RID: 1583
	internal class SystemIPGlobalProperties : IPGlobalProperties
	{
		// Token: 0x060030C4 RID: 12484 RVA: 0x000D237D File Offset: 0x000D137D
		internal SystemIPGlobalProperties()
		{
		}

		// Token: 0x060030C5 RID: 12485 RVA: 0x000D2388 File Offset: 0x000D1388
		internal static FixedInfo GetFixedInfo()
		{
			uint num = 0U;
			SafeLocalFree safeLocalFree = null;
			FixedInfo fixedInfo = default(FixedInfo);
			uint num2 = UnsafeNetInfoNativeMethods.GetNetworkParams(SafeLocalFree.Zero, ref num);
			while (num2 == 111U)
			{
				try
				{
					safeLocalFree = SafeLocalFree.LocalAlloc((int)num);
					num2 = UnsafeNetInfoNativeMethods.GetNetworkParams(safeLocalFree, ref num);
					if (num2 == 0U)
					{
						fixedInfo = new FixedInfo((FIXED_INFO)Marshal.PtrToStructure(safeLocalFree.DangerousGetHandle(), typeof(FIXED_INFO)));
					}
				}
				finally
				{
					if (safeLocalFree != null)
					{
						safeLocalFree.Close();
					}
				}
			}
			if (num2 != 0U)
			{
				throw new NetworkInformationException((int)num2);
			}
			return fixedInfo;
		}

		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x060030C6 RID: 12486 RVA: 0x000D2410 File Offset: 0x000D1410
		internal FixedInfo FixedInfo
		{
			get
			{
				if (!this.fixedInfoInitialized)
				{
					lock (this)
					{
						if (!this.fixedInfoInitialized)
						{
							this.fixedInfo = SystemIPGlobalProperties.GetFixedInfo();
							this.fixedInfoInitialized = true;
						}
					}
				}
				return this.fixedInfo;
			}
		}

		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x060030C7 RID: 12487 RVA: 0x000D2468 File Offset: 0x000D1468
		public override string HostName
		{
			get
			{
				if (SystemIPGlobalProperties.hostName == null)
				{
					lock (SystemIPGlobalProperties.syncObject)
					{
						if (SystemIPGlobalProperties.hostName == null)
						{
							SystemIPGlobalProperties.hostName = this.FixedInfo.HostName;
							SystemIPGlobalProperties.domainName = this.FixedInfo.DomainName;
						}
					}
				}
				return SystemIPGlobalProperties.hostName;
			}
		}

		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x060030C8 RID: 12488 RVA: 0x000D24D4 File Offset: 0x000D14D4
		public override string DomainName
		{
			get
			{
				if (SystemIPGlobalProperties.domainName == null)
				{
					lock (SystemIPGlobalProperties.syncObject)
					{
						if (SystemIPGlobalProperties.domainName == null)
						{
							SystemIPGlobalProperties.hostName = this.FixedInfo.HostName;
							SystemIPGlobalProperties.domainName = this.FixedInfo.DomainName;
						}
					}
				}
				return SystemIPGlobalProperties.domainName;
			}
		}

		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x060030C9 RID: 12489 RVA: 0x000D2540 File Offset: 0x000D1540
		public override NetBiosNodeType NodeType
		{
			get
			{
				return this.FixedInfo.NodeType;
			}
		}

		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x060030CA RID: 12490 RVA: 0x000D255C File Offset: 0x000D155C
		public override string DhcpScopeName
		{
			get
			{
				return this.FixedInfo.ScopeId;
			}
		}

		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x060030CB RID: 12491 RVA: 0x000D2578 File Offset: 0x000D1578
		public override bool IsWinsProxy
		{
			get
			{
				return this.FixedInfo.EnableProxy;
			}
		}

		// Token: 0x060030CC RID: 12492 RVA: 0x000D2594 File Offset: 0x000D1594
		public override TcpConnectionInformation[] GetActiveTcpConnections()
		{
			ArrayList arrayList = new ArrayList();
			TcpConnectionInformation[] array = this.GetAllTcpConnections();
			foreach (TcpConnectionInformation tcpConnectionInformation in array)
			{
				if (tcpConnectionInformation.State != TcpState.Listen)
				{
					arrayList.Add(tcpConnectionInformation);
				}
			}
			array = new TcpConnectionInformation[arrayList.Count];
			for (int j = 0; j < arrayList.Count; j++)
			{
				array[j] = (TcpConnectionInformation)arrayList[j];
			}
			return array;
		}

		// Token: 0x060030CD RID: 12493 RVA: 0x000D2608 File Offset: 0x000D1608
		public override IPEndPoint[] GetActiveTcpListeners()
		{
			ArrayList arrayList = new ArrayList();
			TcpConnectionInformation[] allTcpConnections = this.GetAllTcpConnections();
			foreach (TcpConnectionInformation tcpConnectionInformation in allTcpConnections)
			{
				if (tcpConnectionInformation.State == TcpState.Listen)
				{
					arrayList.Add(tcpConnectionInformation.LocalEndPoint);
				}
			}
			IPEndPoint[] array2 = new IPEndPoint[arrayList.Count];
			for (int j = 0; j < arrayList.Count; j++)
			{
				array2[j] = (IPEndPoint)arrayList[j];
			}
			return array2;
		}

		// Token: 0x060030CE RID: 12494 RVA: 0x000D2688 File Offset: 0x000D1688
		private TcpConnectionInformation[] GetAllTcpConnections()
		{
			uint num = 0U;
			SafeLocalFree safeLocalFree = null;
			SystemTcpConnectionInformation[] array = null;
			uint num2 = UnsafeNetInfoNativeMethods.GetTcpTable(SafeLocalFree.Zero, ref num, true);
			while (num2 == 122U)
			{
				try
				{
					safeLocalFree = SafeLocalFree.LocalAlloc((int)num);
					num2 = UnsafeNetInfoNativeMethods.GetTcpTable(safeLocalFree, ref num, true);
					if (num2 == 0U)
					{
						IntPtr intPtr = safeLocalFree.DangerousGetHandle();
						MibTcpTable mibTcpTable = (MibTcpTable)Marshal.PtrToStructure(intPtr, typeof(MibTcpTable));
						if (mibTcpTable.numberOfEntries > 0U)
						{
							array = new SystemTcpConnectionInformation[mibTcpTable.numberOfEntries];
							intPtr = (IntPtr)((long)intPtr + (long)Marshal.SizeOf(mibTcpTable.numberOfEntries));
							int num3 = 0;
							while ((long)num3 < (long)((ulong)mibTcpTable.numberOfEntries))
							{
								MibTcpRow mibTcpRow = (MibTcpRow)Marshal.PtrToStructure(intPtr, typeof(MibTcpRow));
								array[num3] = new SystemTcpConnectionInformation(mibTcpRow);
								intPtr = (IntPtr)((long)intPtr + (long)Marshal.SizeOf(mibTcpRow));
								num3++;
							}
						}
					}
				}
				finally
				{
					if (safeLocalFree != null)
					{
						safeLocalFree.Close();
					}
				}
			}
			if (num2 != 0U && num2 != 232U)
			{
				throw new NetworkInformationException((int)num2);
			}
			if (array == null)
			{
				return new SystemTcpConnectionInformation[0];
			}
			return array;
		}

		// Token: 0x060030CF RID: 12495 RVA: 0x000D27BC File Offset: 0x000D17BC
		public override IPEndPoint[] GetActiveUdpListeners()
		{
			uint num = 0U;
			SafeLocalFree safeLocalFree = null;
			IPEndPoint[] array = null;
			uint num2 = UnsafeNetInfoNativeMethods.GetUdpTable(SafeLocalFree.Zero, ref num, true);
			while (num2 == 122U)
			{
				try
				{
					safeLocalFree = SafeLocalFree.LocalAlloc((int)num);
					num2 = UnsafeNetInfoNativeMethods.GetUdpTable(safeLocalFree, ref num, true);
					if (num2 == 0U)
					{
						IntPtr intPtr = safeLocalFree.DangerousGetHandle();
						MibUdpTable mibUdpTable = (MibUdpTable)Marshal.PtrToStructure(intPtr, typeof(MibUdpTable));
						if (mibUdpTable.numberOfEntries > 0U)
						{
							array = new IPEndPoint[mibUdpTable.numberOfEntries];
							intPtr = (IntPtr)((long)intPtr + (long)Marshal.SizeOf(mibUdpTable.numberOfEntries));
							int num3 = 0;
							while ((long)num3 < (long)((ulong)mibUdpTable.numberOfEntries))
							{
								MibUdpRow mibUdpRow = (MibUdpRow)Marshal.PtrToStructure(intPtr, typeof(MibUdpRow));
								int num4 = ((int)mibUdpRow.localPort3 << 24) | ((int)mibUdpRow.localPort4 << 16) | ((int)mibUdpRow.localPort1 << 8) | (int)mibUdpRow.localPort2;
								array[num3] = new IPEndPoint((long)((ulong)mibUdpRow.localAddr), num4);
								intPtr = (IntPtr)((long)intPtr + (long)Marshal.SizeOf(mibUdpRow));
								num3++;
							}
						}
					}
				}
				finally
				{
					if (safeLocalFree != null)
					{
						safeLocalFree.Close();
					}
				}
			}
			if (num2 != 0U && num2 != 232U)
			{
				throw new NetworkInformationException((int)num2);
			}
			if (array == null)
			{
				return new IPEndPoint[0];
			}
			return array;
		}

		// Token: 0x060030D0 RID: 12496 RVA: 0x000D2930 File Offset: 0x000D1930
		public override IPGlobalStatistics GetIPv4GlobalStatistics()
		{
			return new SystemIPGlobalStatistics(AddressFamily.InterNetwork);
		}

		// Token: 0x060030D1 RID: 12497 RVA: 0x000D2938 File Offset: 0x000D1938
		public override IPGlobalStatistics GetIPv6GlobalStatistics()
		{
			return new SystemIPGlobalStatistics(AddressFamily.InterNetworkV6);
		}

		// Token: 0x060030D2 RID: 12498 RVA: 0x000D2941 File Offset: 0x000D1941
		public override TcpStatistics GetTcpIPv4Statistics()
		{
			return new SystemTcpStatistics(AddressFamily.InterNetwork);
		}

		// Token: 0x060030D3 RID: 12499 RVA: 0x000D2949 File Offset: 0x000D1949
		public override TcpStatistics GetTcpIPv6Statistics()
		{
			return new SystemTcpStatistics(AddressFamily.InterNetworkV6);
		}

		// Token: 0x060030D4 RID: 12500 RVA: 0x000D2952 File Offset: 0x000D1952
		public override UdpStatistics GetUdpIPv4Statistics()
		{
			return new SystemUdpStatistics(AddressFamily.InterNetwork);
		}

		// Token: 0x060030D5 RID: 12501 RVA: 0x000D295A File Offset: 0x000D195A
		public override UdpStatistics GetUdpIPv6Statistics()
		{
			return new SystemUdpStatistics(AddressFamily.InterNetworkV6);
		}

		// Token: 0x060030D6 RID: 12502 RVA: 0x000D2963 File Offset: 0x000D1963
		public override IcmpV4Statistics GetIcmpV4Statistics()
		{
			return new SystemIcmpV4Statistics();
		}

		// Token: 0x060030D7 RID: 12503 RVA: 0x000D296A File Offset: 0x000D196A
		public override IcmpV6Statistics GetIcmpV6Statistics()
		{
			return new SystemIcmpV6Statistics();
		}

		// Token: 0x04002E44 RID: 11844
		private FixedInfo fixedInfo;

		// Token: 0x04002E45 RID: 11845
		private bool fixedInfoInitialized;

		// Token: 0x04002E46 RID: 11846
		private static string hostName = null;

		// Token: 0x04002E47 RID: 11847
		private static string domainName = null;

		// Token: 0x04002E48 RID: 11848
		private static object syncObject = new object();
	}
}
