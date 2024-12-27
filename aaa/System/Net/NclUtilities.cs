using System;
using System.Collections;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace System.Net
{
	// Token: 0x020003EC RID: 1004
	internal static class NclUtilities
	{
		// Token: 0x06002078 RID: 8312 RVA: 0x0007FD84 File Offset: 0x0007ED84
		internal static bool IsThreadPoolLow()
		{
			if (ComNetOS.IsAspNetServer)
			{
				return false;
			}
			int num;
			int num2;
			ThreadPool.GetAvailableThreads(out num, out num2);
			return num < 2 || (ComNetOS.IsWinNt && num2 < 2);
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06002079 RID: 8313 RVA: 0x0007FDB6 File Offset: 0x0007EDB6
		internal static bool HasShutdownStarted
		{
			get
			{
				return Environment.HasShutdownStarted || AppDomain.CurrentDomain.IsFinalizingForUnload();
			}
		}

		// Token: 0x0600207A RID: 8314 RVA: 0x0007FDCC File Offset: 0x0007EDCC
		internal static bool IsCredentialFailure(SecurityStatus error)
		{
			return error == SecurityStatus.LogonDenied || error == SecurityStatus.UnknownCredentials || error == SecurityStatus.NoImpersonation || error == SecurityStatus.NoAuthenticatingAuthority || error == SecurityStatus.UntrustedRoot || error == SecurityStatus.CertExpired || error == SecurityStatus.SmartcardLogonRequired || error == SecurityStatus.BadBinding;
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x0007FE1C File Offset: 0x0007EE1C
		internal static bool IsClientFault(SecurityStatus error)
		{
			return error == SecurityStatus.InvalidToken || error == SecurityStatus.CannotPack || error == SecurityStatus.QopNotSupported || error == SecurityStatus.NoCredentials || error == SecurityStatus.MessageAltered || error == SecurityStatus.OutOfSequence || error == SecurityStatus.IncompleteMessage || error == SecurityStatus.IncompleteCredentials || error == SecurityStatus.WrongPrincipal || error == SecurityStatus.TimeSkew || error == SecurityStatus.IllegalMessage || error == SecurityStatus.CertUnknown || error == SecurityStatus.AlgorithmMismatch || error == SecurityStatus.SecurityQosFailed || error == SecurityStatus.UnsupportedPreauth;
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x0600207C RID: 8316 RVA: 0x0007FEA3 File Offset: 0x0007EEA3
		internal static ContextCallback ContextRelativeDemandCallback
		{
			get
			{
				if (NclUtilities.s_ContextRelativeDemandCallback == null)
				{
					NclUtilities.s_ContextRelativeDemandCallback = new ContextCallback(NclUtilities.DemandCallback);
				}
				return NclUtilities.s_ContextRelativeDemandCallback;
			}
		}

		// Token: 0x0600207D RID: 8317 RVA: 0x0007FEC2 File Offset: 0x0007EEC2
		private static void DemandCallback(object state)
		{
			((CodeAccessPermission)state).Demand();
		}

		// Token: 0x0600207E RID: 8318 RVA: 0x0007FED0 File Offset: 0x0007EED0
		internal static bool GuessWhetherHostIsLoopback(string host)
		{
			string text = host.ToLowerInvariant();
			if (text == "localhost" || text == "loopback")
			{
				return true;
			}
			IPGlobalProperties ipglobalProperties = IPGlobalProperties.InternalGetIPGlobalProperties();
			string text2 = ipglobalProperties.HostName.ToLowerInvariant();
			return text == text2 || text == text2 + "." + ipglobalProperties.DomainName.ToLowerInvariant();
		}

		// Token: 0x0600207F RID: 8319 RVA: 0x0007FF39 File Offset: 0x0007EF39
		internal static bool IsFatal(Exception exception)
		{
			return exception != null && (exception is OutOfMemoryException || exception is StackOverflowException || exception is ThreadAbortException);
		}

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06002080 RID: 8320 RVA: 0x0007FF5C File Offset: 0x0007EF5C
		internal static IPAddress[] LocalAddresses
		{
			get
			{
				if (NclUtilities.s_AddressChange != null && NclUtilities.s_AddressChange.CheckAndReset())
				{
					return NclUtilities._LocalAddresses = NclUtilities.GetLocalAddresses();
				}
				if (NclUtilities._LocalAddresses != null)
				{
					return NclUtilities._LocalAddresses;
				}
				IPAddress[] array;
				lock (NclUtilities.LocalAddressesLock)
				{
					if (NclUtilities._LocalAddresses != null)
					{
						array = NclUtilities._LocalAddresses;
					}
					else
					{
						NclUtilities.s_AddressChange = new NetworkAddressChangePolled();
						array = (NclUtilities._LocalAddresses = NclUtilities.GetLocalAddresses());
					}
				}
				return array;
			}
		}

		// Token: 0x06002081 RID: 8321 RVA: 0x0007FFE0 File Offset: 0x0007EFE0
		private static IPAddress[] GetLocalAddresses()
		{
			IPAddress[] array;
			if (ComNetOS.IsPostWin2K)
			{
				ArrayList arrayList = new ArrayList(16);
				int num = 0;
				SafeLocalFree safeLocalFree = null;
				GetAdaptersAddressesFlags getAdaptersAddressesFlags = GetAdaptersAddressesFlags.SkipAnycast | GetAdaptersAddressesFlags.SkipMulticast | GetAdaptersAddressesFlags.SkipDnsServer | GetAdaptersAddressesFlags.SkipFriendlyName;
				uint num2 = 0U;
				uint num3 = UnsafeNetInfoNativeMethods.GetAdaptersAddresses(AddressFamily.Unspecified, (uint)getAdaptersAddressesFlags, IntPtr.Zero, SafeLocalFree.Zero, ref num2);
				while (num3 == 111U)
				{
					try
					{
						safeLocalFree = SafeLocalFree.LocalAlloc((int)num2);
						num3 = UnsafeNetInfoNativeMethods.GetAdaptersAddresses(AddressFamily.Unspecified, (uint)getAdaptersAddressesFlags, IntPtr.Zero, safeLocalFree, ref num2);
						if (num3 == 0U)
						{
							IpAdapterAddresses ipAdapterAddresses = (IpAdapterAddresses)Marshal.PtrToStructure(safeLocalFree.DangerousGetHandle(), typeof(IpAdapterAddresses));
							for (;;)
							{
								if (ipAdapterAddresses.FirstUnicastAddress != IntPtr.Zero)
								{
									UnicastIPAddressInformationCollection unicastIPAddressInformationCollection = SystemUnicastIPAddressInformation.ToAddressInformationCollection(ipAdapterAddresses.FirstUnicastAddress);
									num += unicastIPAddressInformationCollection.Count;
									arrayList.Add(unicastIPAddressInformationCollection);
								}
								if (ipAdapterAddresses.next == IntPtr.Zero)
								{
									break;
								}
								ipAdapterAddresses = (IpAdapterAddresses)Marshal.PtrToStructure(ipAdapterAddresses.next, typeof(IpAdapterAddresses));
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
				if (num3 != 0U && num3 != 232U)
				{
					throw new NetworkInformationException((int)num3);
				}
				array = new IPAddress[num];
				uint num4 = 0U;
				using (IEnumerator enumerator = arrayList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						UnicastIPAddressInformationCollection unicastIPAddressInformationCollection2 = (UnicastIPAddressInformationCollection)obj;
						foreach (IPAddressInformation ipaddressInformation in unicastIPAddressInformationCollection2)
						{
							array[(int)((UIntPtr)(num4++))] = ipaddressInformation.Address;
						}
					}
					return array;
				}
			}
			ArrayList arrayList2 = new ArrayList(16);
			int num5 = 0;
			SafeLocalFree safeLocalFree2 = null;
			uint num6 = 0U;
			uint num7 = UnsafeNetInfoNativeMethods.GetAdaptersInfo(SafeLocalFree.Zero, ref num6);
			while (num7 == 111U)
			{
				try
				{
					safeLocalFree2 = SafeLocalFree.LocalAlloc((int)num6);
					num7 = UnsafeNetInfoNativeMethods.GetAdaptersInfo(safeLocalFree2, ref num6);
					if (num7 == 0U)
					{
						IpAdapterInfo ipAdapterInfo = (IpAdapterInfo)Marshal.PtrToStructure(safeLocalFree2.DangerousGetHandle(), typeof(IpAdapterInfo));
						for (;;)
						{
							IPAddressCollection ipaddressCollection = ipAdapterInfo.ipAddressList.ToIPAddressCollection();
							num5 += ipaddressCollection.Count;
							arrayList2.Add(ipaddressCollection);
							if (ipAdapterInfo.Next == IntPtr.Zero)
							{
								break;
							}
							ipAdapterInfo = (IpAdapterInfo)Marshal.PtrToStructure(ipAdapterInfo.Next, typeof(IpAdapterInfo));
						}
					}
				}
				finally
				{
					if (safeLocalFree2 != null)
					{
						safeLocalFree2.Close();
					}
				}
			}
			if (num7 != 0U && num7 != 232U)
			{
				throw new NetworkInformationException((int)num7);
			}
			array = new IPAddress[num5];
			uint num8 = 0U;
			foreach (object obj2 in arrayList2)
			{
				IPAddressCollection ipaddressCollection2 = (IPAddressCollection)obj2;
				foreach (IPAddress ipaddress in ipaddressCollection2)
				{
					array[(int)((UIntPtr)(num8++))] = ipaddress;
				}
			}
			return array;
		}

		// Token: 0x06002082 RID: 8322 RVA: 0x0008031C File Offset: 0x0007F31C
		internal static bool IsAddressLocal(IPAddress ipAddress)
		{
			IPAddress[] localAddresses = NclUtilities.LocalAddresses;
			for (int i = 0; i < localAddresses.Length; i++)
			{
				if (ipAddress.Equals(localAddresses[i], false))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06002083 RID: 8323 RVA: 0x0008034C File Offset: 0x0007F34C
		private static object LocalAddressesLock
		{
			get
			{
				if (NclUtilities._LocalAddressesLock == null)
				{
					Interlocked.CompareExchange(ref NclUtilities._LocalAddressesLock, new object(), null);
				}
				return NclUtilities._LocalAddressesLock;
			}
		}

		// Token: 0x04001FAF RID: 8111
		private static ContextCallback s_ContextRelativeDemandCallback;

		// Token: 0x04001FB0 RID: 8112
		private static IPAddress[] _LocalAddresses;

		// Token: 0x04001FB1 RID: 8113
		private static object _LocalAddressesLock;

		// Token: 0x04001FB2 RID: 8114
		private static NetworkAddressChangePolled s_AddressChange;
	}
}
