using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000B1 RID: 177
	internal sealed class Locator
	{
		// Token: 0x060005E2 RID: 1506 RVA: 0x0002211C File Offset: 0x0002111C
		private Locator()
		{
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00022124 File Offset: 0x00021124
		internal static DomainControllerInfo GetDomainControllerInfo(string computerName, string domainName, string siteName, long flags)
		{
			DomainControllerInfo domainControllerInfo;
			int num = Locator.DsGetDcNameWrapper(computerName, domainName, siteName, flags, out domainControllerInfo);
			if (num != 0)
			{
				throw ExceptionHelper.GetExceptionFromErrorCode(num, domainName);
			}
			return domainControllerInfo;
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0002214C File Offset: 0x0002114C
		internal static int DsGetDcNameWrapper(string computerName, string domainName, string siteName, long flags, out DomainControllerInfo domainControllerInfo)
		{
			IntPtr zero = IntPtr.Zero;
			int num = 0;
			if (computerName != null && computerName.Length == 0)
			{
				computerName = null;
			}
			if (siteName != null && siteName.Length == 0)
			{
				siteName = null;
			}
			num = NativeMethods.DsGetDcName(computerName, domainName, IntPtr.Zero, siteName, (int)(flags | 1073741824L), out zero);
			if (num == 0)
			{
				try
				{
					domainControllerInfo = new DomainControllerInfo();
					Marshal.PtrToStructure(zero, domainControllerInfo);
					return num;
				}
				finally
				{
					if (zero != IntPtr.Zero)
					{
						num = NativeMethods.NetApiBufferFree(zero);
					}
				}
			}
			domainControllerInfo = new DomainControllerInfo();
			return num;
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x000221DC File Offset: 0x000211DC
		internal static ArrayList EnumerateDomainControllers(DirectoryContext context, string domainName, string siteName, long dcFlags)
		{
			Hashtable hashtable = null;
			ArrayList arrayList = new ArrayList();
			if (siteName == null)
			{
				DomainControllerInfo domainControllerInfo;
				int num = Locator.DsGetDcNameWrapper(null, domainName, null, dcFlags & 36928L, out domainControllerInfo);
				if (num == 0)
				{
					siteName = domainControllerInfo.ClientSiteName;
				}
				else
				{
					if (num == 1355)
					{
						return arrayList;
					}
					throw ExceptionHelper.GetExceptionFromErrorCode(num);
				}
			}
			if (DirectoryContext.DnsgetdcSupported)
			{
				hashtable = Locator.DnsGetDcWrapper(domainName, siteName, dcFlags);
			}
			else
			{
				hashtable = Locator.DnsQueryWrapper(domainName, null, dcFlags);
				if (siteName != null)
				{
					foreach (object obj in Locator.DnsQueryWrapper(domainName, siteName, dcFlags).Keys)
					{
						string text = (string)obj;
						if (!hashtable.Contains(text))
						{
							hashtable.Add(text, null);
						}
					}
				}
			}
			foreach (object obj2 in hashtable.Keys)
			{
				string text2 = (string)obj2;
				DirectoryContext newDirectoryContext = Utils.GetNewDirectoryContext(text2, DirectoryContextType.DirectoryServer, context);
				if ((dcFlags & 64L) != 0L)
				{
					arrayList.Add(new GlobalCatalog(newDirectoryContext, text2));
				}
				else
				{
					arrayList.Add(new DomainController(newDirectoryContext, text2));
				}
			}
			return arrayList;
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x0002232C File Offset: 0x0002132C
		private static Hashtable DnsGetDcWrapper(string domainName, string siteName, long dcFlags)
		{
			Hashtable hashtable = new Hashtable();
			int num = 0;
			IntPtr zero = IntPtr.Zero;
			IntPtr zero2 = IntPtr.Zero;
			int num2 = 0;
			IntPtr intPtr = new IntPtr(num2);
			IntPtr zero3 = IntPtr.Zero;
			int num3 = NativeMethods.DsGetDcOpen(domainName, num, siteName, IntPtr.Zero, null, (int)dcFlags, out zero);
			if (num3 == 0)
			{
				try
				{
					num3 = NativeMethods.DsGetDcNext(zero, ref intPtr, out zero3, out zero2);
					if (num3 != 0 && num3 != 1101 && num3 != 9003 && num3 != 259)
					{
						throw ExceptionHelper.GetExceptionFromErrorCode(num3);
					}
					while (num3 != 259)
					{
						if (num3 != 1101 && num3 != 9003)
						{
							try
							{
								string text = Marshal.PtrToStringUni(zero2);
								string text2 = text.ToLower(CultureInfo.InvariantCulture);
								if (!hashtable.Contains(text2))
								{
									hashtable.Add(text2, null);
								}
							}
							finally
							{
								if (zero2 != IntPtr.Zero)
								{
									num3 = NativeMethods.NetApiBufferFree(zero2);
								}
							}
						}
						num3 = NativeMethods.DsGetDcNext(zero, ref intPtr, out zero3, out zero2);
						if (num3 != 0 && num3 != 1101 && num3 != 9003 && num3 != 259)
						{
							throw ExceptionHelper.GetExceptionFromErrorCode(num3);
						}
					}
					return hashtable;
				}
				finally
				{
					NativeMethods.DsGetDcClose(zero);
				}
			}
			if (num3 != 0)
			{
				throw ExceptionHelper.GetExceptionFromErrorCode(num3);
			}
			return hashtable;
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00022490 File Offset: 0x00021490
		private static Hashtable DnsQueryWrapper(string domainName, string siteName, long dcFlags)
		{
			Hashtable hashtable = new Hashtable();
			string text = "_ldap._tcp.";
			int num = 0;
			IntPtr zero = IntPtr.Zero;
			if (siteName != null && siteName.Length != 0)
			{
				text = text + siteName + "._sites.";
			}
			if ((dcFlags & 64L) != 0L)
			{
				text += "gc._msdcs.";
			}
			else if ((dcFlags & 4096L) != 0L)
			{
				text += "dc._msdcs.";
			}
			text += domainName;
			if ((dcFlags & 1L) != 0L)
			{
				num |= 8;
			}
			int num2 = NativeMethods.DnsQuery(text, 33, num, IntPtr.Zero, out zero, IntPtr.Zero);
			if (num2 == 0)
			{
				try
				{
					IntPtr intPtr = zero;
					while (intPtr != IntPtr.Zero)
					{
						PartialDnsRecord partialDnsRecord = new PartialDnsRecord();
						Marshal.PtrToStructure(intPtr, partialDnsRecord);
						if (partialDnsRecord.type == 33)
						{
							DnsRecord dnsRecord = new DnsRecord();
							Marshal.PtrToStructure(intPtr, dnsRecord);
							string targetName = dnsRecord.data.targetName;
							string text2 = targetName.ToLower(CultureInfo.InvariantCulture);
							if (!hashtable.Contains(text2))
							{
								hashtable.Add(text2, null);
							}
						}
						intPtr = partialDnsRecord.next;
					}
					return hashtable;
				}
				finally
				{
					if (zero != IntPtr.Zero)
					{
						NativeMethods.DnsRecordListFree(zero, true);
					}
				}
			}
			if (num2 != 0)
			{
				throw ExceptionHelper.GetExceptionFromErrorCode(num2);
			}
			return hashtable;
		}
	}
}
