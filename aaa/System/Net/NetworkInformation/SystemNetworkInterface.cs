using System;
using System.Collections;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000639 RID: 1593
	internal class SystemNetworkInterface : NetworkInterface
	{
		// Token: 0x06003147 RID: 12615 RVA: 0x000D3A5A File Offset: 0x000D2A5A
		private SystemNetworkInterface()
		{
		}

		// Token: 0x06003148 RID: 12616 RVA: 0x000D3A62 File Offset: 0x000D2A62
		internal static NetworkInterface[] GetNetworkInterfaces()
		{
			return SystemNetworkInterface.GetNetworkInterfaces(AddressFamily.Unspecified);
		}

		// Token: 0x17000B1C RID: 2844
		// (get) Token: 0x06003149 RID: 12617 RVA: 0x000D3A6C File Offset: 0x000D2A6C
		internal static int InternalLoopbackInterfaceIndex
		{
			get
			{
				int num;
				int bestInterface = (int)UnsafeNetInfoNativeMethods.GetBestInterface(16777343, out num);
				if (bestInterface != 0)
				{
					throw new NetworkInformationException(bestInterface);
				}
				return num;
			}
		}

		// Token: 0x0600314A RID: 12618 RVA: 0x000D3A94 File Offset: 0x000D2A94
		internal static bool InternalGetIsNetworkAvailable()
		{
			if (ComNetOS.IsWinNt)
			{
				NetworkInterface[] networkInterfaces = SystemNetworkInterface.GetNetworkInterfaces();
				foreach (NetworkInterface networkInterface in networkInterfaces)
				{
					if (networkInterface.OperationalStatus == OperationalStatus.Up && networkInterface.NetworkInterfaceType != NetworkInterfaceType.Tunnel && networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback)
					{
						return true;
					}
				}
				return false;
			}
			uint num = 0U;
			return UnsafeWinINetNativeMethods.InternetGetConnectedState(ref num, 0U);
		}

		// Token: 0x0600314B RID: 12619 RVA: 0x000D3B00 File Offset: 0x000D2B00
		private static NetworkInterface[] GetNetworkInterfaces(AddressFamily family)
		{
			IpHelperErrors.CheckFamilyUnspecified(family);
			if (ComNetOS.IsPostWin2K)
			{
				return SystemNetworkInterface.PostWin2KGetNetworkInterfaces(family);
			}
			FixedInfo fixedInfo = SystemIPGlobalProperties.GetFixedInfo();
			if (family != AddressFamily.Unspecified && family != AddressFamily.InterNetwork)
			{
				throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
			}
			SafeLocalFree safeLocalFree = null;
			uint num = 0U;
			ArrayList arrayList = new ArrayList();
			uint num2 = UnsafeNetInfoNativeMethods.GetAdaptersInfo(SafeLocalFree.Zero, ref num);
			while (num2 == 111U)
			{
				try
				{
					safeLocalFree = SafeLocalFree.LocalAlloc((int)num);
					num2 = UnsafeNetInfoNativeMethods.GetAdaptersInfo(safeLocalFree, ref num);
					if (num2 == 0U)
					{
						IpAdapterInfo ipAdapterInfo = (IpAdapterInfo)Marshal.PtrToStructure(safeLocalFree.DangerousGetHandle(), typeof(IpAdapterInfo));
						arrayList.Add(new SystemNetworkInterface(fixedInfo, ipAdapterInfo));
						while (ipAdapterInfo.Next != IntPtr.Zero)
						{
							ipAdapterInfo = (IpAdapterInfo)Marshal.PtrToStructure(ipAdapterInfo.Next, typeof(IpAdapterInfo));
							arrayList.Add(new SystemNetworkInterface(fixedInfo, ipAdapterInfo));
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
			if (num2 == 232U)
			{
				return new SystemNetworkInterface[0];
			}
			if (num2 != 0U)
			{
				throw new NetworkInformationException((int)num2);
			}
			SystemNetworkInterface[] array = new SystemNetworkInterface[arrayList.Count];
			for (int i = 0; i < arrayList.Count; i++)
			{
				array[i] = (SystemNetworkInterface)arrayList[i];
			}
			return array;
		}

		// Token: 0x0600314C RID: 12620 RVA: 0x000D3C58 File Offset: 0x000D2C58
		private static SystemNetworkInterface[] GetAdaptersAddresses(AddressFamily family, FixedInfo fixedInfo)
		{
			uint num = 0U;
			SafeLocalFree safeLocalFree = null;
			ArrayList arrayList = new ArrayList();
			uint num2 = UnsafeNetInfoNativeMethods.GetAdaptersAddresses(family, 0U, IntPtr.Zero, SafeLocalFree.Zero, ref num);
			while (num2 == 111U)
			{
				try
				{
					safeLocalFree = SafeLocalFree.LocalAlloc((int)num);
					num2 = UnsafeNetInfoNativeMethods.GetAdaptersAddresses(family, 0U, IntPtr.Zero, safeLocalFree, ref num);
					if (num2 == 0U)
					{
						IpAdapterAddresses ipAdapterAddresses = (IpAdapterAddresses)Marshal.PtrToStructure(safeLocalFree.DangerousGetHandle(), typeof(IpAdapterAddresses));
						arrayList.Add(new SystemNetworkInterface(fixedInfo, ipAdapterAddresses));
						while (ipAdapterAddresses.next != IntPtr.Zero)
						{
							ipAdapterAddresses = (IpAdapterAddresses)Marshal.PtrToStructure(ipAdapterAddresses.next, typeof(IpAdapterAddresses));
							arrayList.Add(new SystemNetworkInterface(fixedInfo, ipAdapterAddresses));
						}
					}
				}
				finally
				{
					if (safeLocalFree != null)
					{
						safeLocalFree.Close();
					}
					safeLocalFree = null;
				}
			}
			if (num2 == 232U || num2 == 87U)
			{
				return new SystemNetworkInterface[0];
			}
			if (num2 != 0U)
			{
				throw new NetworkInformationException((int)num2);
			}
			SystemNetworkInterface[] array = new SystemNetworkInterface[arrayList.Count];
			for (int i = 0; i < arrayList.Count; i++)
			{
				array[i] = (SystemNetworkInterface)arrayList[i];
			}
			return array;
		}

		// Token: 0x0600314D RID: 12621 RVA: 0x000D3D90 File Offset: 0x000D2D90
		private static SystemNetworkInterface[] PostWin2KGetNetworkInterfaces(AddressFamily family)
		{
			FixedInfo fixedInfo = SystemIPGlobalProperties.GetFixedInfo();
			SystemNetworkInterface[] array = null;
			try
			{
				IL_0008:
				array = SystemNetworkInterface.GetAdaptersAddresses(family, fixedInfo);
			}
			catch (NetworkInformationException ex)
			{
				if ((long)ex.ErrorCode != 1L)
				{
					throw;
				}
				goto IL_0008;
			}
			if (!Socket.SupportsIPv4)
			{
				return array;
			}
			uint num = 0U;
			uint num2 = 0U;
			SafeLocalFree safeLocalFree = null;
			if (family == AddressFamily.Unspecified || family == AddressFamily.InterNetwork)
			{
				num2 = UnsafeNetInfoNativeMethods.GetAdaptersInfo(SafeLocalFree.Zero, ref num);
				int num3 = 0;
				while (num2 == 111U)
				{
					try
					{
						safeLocalFree = SafeLocalFree.LocalAlloc((int)num);
						num2 = UnsafeNetInfoNativeMethods.GetAdaptersInfo(safeLocalFree, ref num);
						if (num2 == 0U)
						{
							IntPtr intPtr = safeLocalFree.DangerousGetHandle();
							while (intPtr != IntPtr.Zero)
							{
								IpAdapterInfo ipAdapterInfo = (IpAdapterInfo)Marshal.PtrToStructure(intPtr, typeof(IpAdapterInfo));
								int i = 0;
								while (i < array.Length)
								{
									if (array[i] != null && ipAdapterInfo.index == array[i].index)
									{
										if (!array[i].interfaceProperties.Update(fixedInfo, ipAdapterInfo))
										{
											array[i] = null;
											num3++;
											break;
										}
										break;
									}
									else
									{
										i++;
									}
								}
								intPtr = ipAdapterInfo.Next;
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
				if (num3 != 0)
				{
					SystemNetworkInterface[] array2 = new SystemNetworkInterface[array.Length - num3];
					int num4 = 0;
					for (int j = 0; j < array.Length; j++)
					{
						if (array[j] != null)
						{
							array2[num4++] = array[j];
						}
					}
					array = array2;
				}
			}
			if (num2 != 0U && num2 != 232U)
			{
				throw new NetworkInformationException((int)num2);
			}
			return array;
		}

		// Token: 0x0600314E RID: 12622 RVA: 0x000D3F1C File Offset: 0x000D2F1C
		internal SystemNetworkInterface(FixedInfo fixedInfo, IpAdapterAddresses ipAdapterAddresses)
		{
			this.id = ipAdapterAddresses.AdapterName;
			this.name = ipAdapterAddresses.friendlyName;
			this.description = ipAdapterAddresses.description;
			this.index = ipAdapterAddresses.index;
			this.physicalAddress = ipAdapterAddresses.address;
			this.addressLength = ipAdapterAddresses.addressLength;
			this.type = ipAdapterAddresses.type;
			this.operStatus = ipAdapterAddresses.operStatus;
			this.ipv6Index = ipAdapterAddresses.ipv6Index;
			this.adapterFlags = ipAdapterAddresses.flags;
			this.interfaceProperties = new SystemIPInterfaceProperties(fixedInfo, ipAdapterAddresses);
		}

		// Token: 0x0600314F RID: 12623 RVA: 0x000D3FC0 File Offset: 0x000D2FC0
		internal SystemNetworkInterface(FixedInfo fixedInfo, IpAdapterInfo ipAdapterInfo)
		{
			this.id = ipAdapterInfo.adapterName;
			this.name = string.Empty;
			this.description = ipAdapterInfo.description;
			this.index = ipAdapterInfo.index;
			this.physicalAddress = ipAdapterInfo.address;
			this.addressLength = ipAdapterInfo.addressLength;
			if (ComNetOS.IsWin2K && !ComNetOS.IsPostWin2K)
			{
				this.name = this.ReadAdapterName(this.id);
			}
			if (this.name.Length == 0)
			{
				this.name = this.description;
			}
			SystemIPv4InterfaceStatistics systemIPv4InterfaceStatistics = new SystemIPv4InterfaceStatistics((long)((ulong)this.index));
			this.operStatus = systemIPv4InterfaceStatistics.OperationalStatus;
			OldInterfaceType oldInterfaceType = ipAdapterInfo.type;
			if (oldInterfaceType <= OldInterfaceType.TokenRing)
			{
				if (oldInterfaceType == OldInterfaceType.Ethernet)
				{
					this.type = NetworkInterfaceType.Ethernet;
					goto IL_011B;
				}
				if (oldInterfaceType == OldInterfaceType.TokenRing)
				{
					this.type = NetworkInterfaceType.TokenRing;
					goto IL_011B;
				}
			}
			else
			{
				if (oldInterfaceType == OldInterfaceType.Fddi)
				{
					this.type = NetworkInterfaceType.Fddi;
					goto IL_011B;
				}
				switch (oldInterfaceType)
				{
				case OldInterfaceType.Ppp:
					this.type = NetworkInterfaceType.Ppp;
					goto IL_011B;
				case OldInterfaceType.Loopback:
					this.type = NetworkInterfaceType.Loopback;
					goto IL_011B;
				default:
					if (oldInterfaceType == OldInterfaceType.Slip)
					{
						this.type = NetworkInterfaceType.Slip;
						goto IL_011B;
					}
					break;
				}
			}
			this.type = NetworkInterfaceType.Unknown;
			IL_011B:
			this.interfaceProperties = new SystemIPInterfaceProperties(fixedInfo, ipAdapterInfo);
		}

		// Token: 0x17000B1D RID: 2845
		// (get) Token: 0x06003150 RID: 12624 RVA: 0x000D40F5 File Offset: 0x000D30F5
		public override string Id
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x17000B1E RID: 2846
		// (get) Token: 0x06003151 RID: 12625 RVA: 0x000D40FD File Offset: 0x000D30FD
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000B1F RID: 2847
		// (get) Token: 0x06003152 RID: 12626 RVA: 0x000D4105 File Offset: 0x000D3105
		public override string Description
		{
			get
			{
				return this.description;
			}
		}

		// Token: 0x06003153 RID: 12627 RVA: 0x000D4110 File Offset: 0x000D3110
		public override PhysicalAddress GetPhysicalAddress()
		{
			byte[] array = new byte[this.addressLength];
			Array.Copy(this.physicalAddress, array, (long)((ulong)this.addressLength));
			return new PhysicalAddress(array);
		}

		// Token: 0x17000B20 RID: 2848
		// (get) Token: 0x06003154 RID: 12628 RVA: 0x000D4143 File Offset: 0x000D3143
		public override NetworkInterfaceType NetworkInterfaceType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x06003155 RID: 12629 RVA: 0x000D414B File Offset: 0x000D314B
		public override IPInterfaceProperties GetIPProperties()
		{
			return this.interfaceProperties;
		}

		// Token: 0x06003156 RID: 12630 RVA: 0x000D4153 File Offset: 0x000D3153
		public override IPv4InterfaceStatistics GetIPv4Statistics()
		{
			return new SystemIPv4InterfaceStatistics((long)((ulong)this.index));
		}

		// Token: 0x06003157 RID: 12631 RVA: 0x000D4161 File Offset: 0x000D3161
		public override bool Supports(NetworkInterfaceComponent networkInterfaceComponent)
		{
			return (networkInterfaceComponent == NetworkInterfaceComponent.IPv6 && this.ipv6Index > 0U) || (networkInterfaceComponent == NetworkInterfaceComponent.IPv4 && this.index > 0U);
		}

		// Token: 0x17000B21 RID: 2849
		// (get) Token: 0x06003158 RID: 12632 RVA: 0x000D4181 File Offset: 0x000D3181
		public override OperationalStatus OperationalStatus
		{
			get
			{
				return this.operStatus;
			}
		}

		// Token: 0x17000B22 RID: 2850
		// (get) Token: 0x06003159 RID: 12633 RVA: 0x000D418C File Offset: 0x000D318C
		public override long Speed
		{
			get
			{
				if (this.speed == 0L)
				{
					SystemIPv4InterfaceStatistics systemIPv4InterfaceStatistics = new SystemIPv4InterfaceStatistics((long)((ulong)this.index));
					this.speed = systemIPv4InterfaceStatistics.Speed;
				}
				return this.speed;
			}
		}

		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x0600315A RID: 12634 RVA: 0x000D41C2 File Offset: 0x000D31C2
		public override bool IsReceiveOnly
		{
			get
			{
				if (!ComNetOS.IsPostWin2K)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
				}
				return (this.adapterFlags & AdapterFlags.ReceiveOnly) > (AdapterFlags)0;
			}
		}

		// Token: 0x17000B24 RID: 2852
		// (get) Token: 0x0600315B RID: 12635 RVA: 0x000D41E6 File Offset: 0x000D31E6
		public override bool SupportsMulticast
		{
			get
			{
				if (!ComNetOS.IsPostWin2K)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
				}
				return (this.adapterFlags & AdapterFlags.NoMulticast) == (AdapterFlags)0;
			}
		}

		// Token: 0x0600315C RID: 12636 RVA: 0x000D420C File Offset: 0x000D320C
		[RegistryPermission(SecurityAction.Assert, Read = "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Network\\{4D36E972-E325-11CE-BFC1-08002BE10318}")]
		private string ReadAdapterName(string id)
		{
			RegistryKey registryKey = null;
			string text = string.Empty;
			try
			{
				string text2 = "SYSTEM\\CurrentControlSet\\Control\\Network\\{4D36E972-E325-11CE-BFC1-08002BE10318}\\" + id + "\\Connection";
				registryKey = Registry.LocalMachine.OpenSubKey(text2);
				if (registryKey != null)
				{
					text = (string)registryKey.GetValue("Name");
					if (text == null)
					{
						text = string.Empty;
					}
				}
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
			}
			return text;
		}

		// Token: 0x04002E73 RID: 11891
		private string name;

		// Token: 0x04002E74 RID: 11892
		private string id;

		// Token: 0x04002E75 RID: 11893
		private string description;

		// Token: 0x04002E76 RID: 11894
		private byte[] physicalAddress;

		// Token: 0x04002E77 RID: 11895
		private uint addressLength;

		// Token: 0x04002E78 RID: 11896
		private NetworkInterfaceType type;

		// Token: 0x04002E79 RID: 11897
		private OperationalStatus operStatus;

		// Token: 0x04002E7A RID: 11898
		private long speed;

		// Token: 0x04002E7B RID: 11899
		internal uint index;

		// Token: 0x04002E7C RID: 11900
		internal uint ipv6Index;

		// Token: 0x04002E7D RID: 11901
		private AdapterFlags adapterFlags;

		// Token: 0x04002E7E RID: 11902
		private SystemIPInterfaceProperties interfaceProperties;
	}
}
