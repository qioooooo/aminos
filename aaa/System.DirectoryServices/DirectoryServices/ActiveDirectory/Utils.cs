using System;
using System.Collections;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000135 RID: 309
	[DirectoryServicesPermission(SecurityAction.Assert, Unrestricted = true)]
	internal sealed class Utils
	{
		// Token: 0x060007C7 RID: 1991 RVA: 0x000277EF File Offset: 0x000267EF
		private Utils()
		{
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x000277F8 File Offset: 0x000267F8
		internal static string GetDnsNameFromDN(string distinguishedName)
		{
			string text = null;
			IntPtr zero = IntPtr.Zero;
			IntPtr intPtr = UnsafeNativeMethods.GetProcAddress(DirectoryContext.ADHandle, "DsCrackNamesW");
			if (intPtr == (IntPtr)0)
			{
				throw ExceptionHelper.GetExceptionFromErrorCode(Marshal.GetLastWin32Error());
			}
			NativeMethods.DsCrackNames dsCrackNames = (NativeMethods.DsCrackNames)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(NativeMethods.DsCrackNames));
			IntPtr intPtr2 = Marshal.StringToHGlobalUni(distinguishedName);
			IntPtr intPtr3 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
			Marshal.WriteIntPtr(intPtr3, intPtr2);
			int num = dsCrackNames(IntPtr.Zero, 1, 1, 7, 1, intPtr3, out zero);
			if (num == 0)
			{
				try
				{
					DsNameResult dsNameResult = new DsNameResult();
					Marshal.PtrToStructure(zero, dsNameResult);
					if (dsNameResult.itemCount >= 1 && dsNameResult.items != IntPtr.Zero)
					{
						DsNameResultItem dsNameResultItem = new DsNameResultItem();
						Marshal.PtrToStructure(dsNameResult.items, dsNameResultItem);
						if (dsNameResultItem.status == 6 || dsNameResultItem.name == null)
						{
							throw new ArgumentException(Res.GetString("InvalidDNFormat"), "distinguishedName");
						}
						if (dsNameResultItem.status != 0)
						{
							throw ExceptionHelper.GetExceptionFromErrorCode(num);
						}
						if (dsNameResultItem.name.Length - 1 == dsNameResultItem.name.IndexOf('/'))
						{
							text = dsNameResultItem.name.Substring(0, dsNameResultItem.name.Length - 1);
						}
						else
						{
							text = dsNameResultItem.name;
						}
					}
					return text;
				}
				finally
				{
					if (intPtr3 != (IntPtr)0)
					{
						Marshal.FreeHGlobal(intPtr3);
					}
					if (intPtr2 != (IntPtr)0)
					{
						Marshal.FreeHGlobal(intPtr2);
					}
					if (zero != IntPtr.Zero)
					{
						intPtr = UnsafeNativeMethods.GetProcAddress(DirectoryContext.ADHandle, "DsFreeNameResultW");
						if (intPtr == (IntPtr)0)
						{
							throw ExceptionHelper.GetExceptionFromErrorCode(Marshal.GetLastWin32Error());
						}
						UnsafeNativeMethods.DsFreeNameResultW dsFreeNameResultW = (UnsafeNativeMethods.DsFreeNameResultW)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(UnsafeNativeMethods.DsFreeNameResultW));
						dsFreeNameResultW(zero);
					}
				}
			}
			if (num == 6)
			{
				throw new ArgumentException(Res.GetString("InvalidDNFormat"), "distinguishedName");
			}
			throw ExceptionHelper.GetExceptionFromErrorCode(num);
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x00027A0C File Offset: 0x00026A0C
		internal static string GetDNFromDnsName(string dnsName)
		{
			string text = null;
			IntPtr zero = IntPtr.Zero;
			IntPtr intPtr = UnsafeNativeMethods.GetProcAddress(DirectoryContext.ADHandle, "DsCrackNamesW");
			if (intPtr == (IntPtr)0)
			{
				throw ExceptionHelper.GetExceptionFromErrorCode(Marshal.GetLastWin32Error());
			}
			NativeMethods.DsCrackNames dsCrackNames = (NativeMethods.DsCrackNames)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(NativeMethods.DsCrackNames));
			IntPtr intPtr2 = Marshal.StringToHGlobalUni(dnsName + "/");
			IntPtr intPtr3 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
			Marshal.WriteIntPtr(intPtr3, intPtr2);
			int num = dsCrackNames(IntPtr.Zero, 1, 7, 1, 1, intPtr3, out zero);
			if (num == 0)
			{
				try
				{
					DsNameResult dsNameResult = new DsNameResult();
					Marshal.PtrToStructure(zero, dsNameResult);
					if (dsNameResult.itemCount >= 1 && dsNameResult.items != IntPtr.Zero)
					{
						DsNameResultItem dsNameResultItem = new DsNameResultItem();
						Marshal.PtrToStructure(dsNameResult.items, dsNameResultItem);
						text = dsNameResultItem.name;
					}
					return text;
				}
				finally
				{
					if (intPtr3 != (IntPtr)0)
					{
						Marshal.FreeHGlobal(intPtr3);
					}
					if (intPtr2 != (IntPtr)0)
					{
						Marshal.FreeHGlobal(intPtr2);
					}
					if (zero != IntPtr.Zero)
					{
						intPtr = UnsafeNativeMethods.GetProcAddress(DirectoryContext.ADHandle, "DsFreeNameResultW");
						if (intPtr == (IntPtr)0)
						{
							throw ExceptionHelper.GetExceptionFromErrorCode(Marshal.GetLastWin32Error());
						}
						UnsafeNativeMethods.DsFreeNameResultW dsFreeNameResultW = (UnsafeNativeMethods.DsFreeNameResultW)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(UnsafeNativeMethods.DsFreeNameResultW));
						dsFreeNameResultW(zero);
					}
				}
			}
			if (num == 6)
			{
				throw new ArgumentException(Res.GetString("InvalidDNFormat"));
			}
			throw ExceptionHelper.GetExceptionFromErrorCode(num);
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x00027BAC File Offset: 0x00026BAC
		internal static string GetDnsHostNameFromNTDSA(DirectoryContext context, string dn)
		{
			string text = null;
			int num = dn.IndexOf(',');
			if (num == -1)
			{
				throw new ArgumentException(Res.GetString("InvalidDNFormat"), "dn");
			}
			string text2 = dn.Substring(num + 1);
			DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(context, text2);
			try
			{
				text = (string)PropertyManager.GetPropertyValue(context, directoryEntry, PropertyManager.DnsHostName);
			}
			finally
			{
				directoryEntry.Dispose();
			}
			return text;
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x00027C1C File Offset: 0x00026C1C
		internal static string GetAdamDnsHostNameFromNTDSA(DirectoryContext context, string dn)
		{
			string text = null;
			int num = -1;
			string partialDN = Utils.GetPartialDN(dn, 1);
			string partialDN2 = Utils.GetPartialDN(dn, 2);
			string text2 = "CN=NTDS-DSA";
			DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(context, partialDN2);
			string text3 = string.Concat(new string[]
			{
				"(|(&(",
				PropertyManager.ObjectCategory,
				"=server)(",
				PropertyManager.DistinguishedName,
				"=",
				Utils.GetEscapedFilterValue(partialDN),
				"))(&(",
				PropertyManager.ObjectCategory,
				"=nTDSDSA)(",
				PropertyManager.DistinguishedName,
				"=",
				Utils.GetEscapedFilterValue(dn),
				")))"
			});
			ADSearcher adsearcher = new ADSearcher(directoryEntry, text3, new string[]
			{
				PropertyManager.DnsHostName,
				PropertyManager.MsDSPortLDAP,
				PropertyManager.ObjectCategory
			}, SearchScope.Subtree, true, true);
			SearchResultCollection searchResultCollection = adsearcher.FindAll();
			try
			{
				if (searchResultCollection.Count != 2)
				{
					throw new ActiveDirectoryOperationException(Res.GetString("NoHostNameOrPortNumber", new object[] { dn }));
				}
				foreach (object obj in searchResultCollection)
				{
					SearchResult searchResult = (SearchResult)obj;
					string text4 = (string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.ObjectCategory);
					if (text4.Length >= text2.Length && Utils.Compare(text4, 0, text2.Length, text2, 0, text2.Length) == 0)
					{
						num = (int)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.MsDSPortLDAP);
					}
					else
					{
						text = (string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.DnsHostName);
					}
				}
			}
			finally
			{
				searchResultCollection.Dispose();
				directoryEntry.Dispose();
			}
			if (num == -1 || text == null)
			{
				throw new ActiveDirectoryOperationException(Res.GetString("NoHostNameOrPortNumber", new object[] { dn }));
			}
			return text + ":" + num;
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x00027E44 File Offset: 0x00026E44
		internal static string GetAdamHostNameAndPortsFromNTDSA(DirectoryContext context, string dn)
		{
			string text = null;
			int num = -1;
			int num2 = -1;
			string partialDN = Utils.GetPartialDN(dn, 1);
			string partialDN2 = Utils.GetPartialDN(dn, 2);
			string text2 = "CN=NTDS-DSA";
			DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(context, partialDN2);
			string text3 = string.Concat(new string[]
			{
				"(|(&(",
				PropertyManager.ObjectCategory,
				"=server)(",
				PropertyManager.DistinguishedName,
				"=",
				Utils.GetEscapedFilterValue(partialDN),
				"))(&(",
				PropertyManager.ObjectCategory,
				"=nTDSDSA)(",
				PropertyManager.DistinguishedName,
				"=",
				Utils.GetEscapedFilterValue(dn),
				")))"
			});
			ADSearcher adsearcher = new ADSearcher(directoryEntry, text3, new string[]
			{
				PropertyManager.DnsHostName,
				PropertyManager.MsDSPortLDAP,
				PropertyManager.MsDSPortSSL,
				PropertyManager.ObjectCategory
			}, SearchScope.Subtree, true, true);
			SearchResultCollection searchResultCollection = adsearcher.FindAll();
			try
			{
				if (searchResultCollection.Count != 2)
				{
					throw new ActiveDirectoryOperationException(Res.GetString("NoHostNameOrPortNumber", new object[] { dn }));
				}
				foreach (object obj in searchResultCollection)
				{
					SearchResult searchResult = (SearchResult)obj;
					string text4 = (string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.ObjectCategory);
					if (text4.Length >= text2.Length && Utils.Compare(text4, 0, text2.Length, text2, 0, text2.Length) == 0)
					{
						num = (int)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.MsDSPortLDAP);
						num2 = (int)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.MsDSPortSSL);
					}
					else
					{
						text = (string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.DnsHostName);
					}
				}
			}
			finally
			{
				searchResultCollection.Dispose();
				directoryEntry.Dispose();
			}
			if (num == -1 || num2 == -1 || text == null)
			{
				throw new ActiveDirectoryOperationException(Res.GetString("NoHostNameOrPortNumber", new object[] { dn }));
			}
			return string.Concat(new object[] { text, ":", num, ":", num2 });
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x000280C0 File Offset: 0x000270C0
		internal static string GetRdnFromDN(string distinguishedName)
		{
			Component[] dncomponents = Utils.GetDNComponents(distinguishedName);
			return dncomponents[0].Name + "=" + dncomponents[0].Value;
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x000280F8 File Offset: 0x000270F8
		internal static string GetPartialDN(string distinguishedName, int startingIndex)
		{
			string text = "";
			Component[] dncomponents = Utils.GetDNComponents(distinguishedName);
			bool flag = true;
			for (int i = startingIndex; i < dncomponents.GetLength(0); i++)
			{
				if (flag)
				{
					text = dncomponents[i].Name + "=" + dncomponents[i].Value;
					flag = false;
				}
				else
				{
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						",",
						dncomponents[i].Name,
						"=",
						dncomponents[i].Value
					});
				}
			}
			return text;
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x000281A0 File Offset: 0x000271A0
		internal static Component[] GetDNComponents(string distinguishedName)
		{
			string[] array = Utils.Split(distinguishedName, ',');
			Component[] array2 = new Component[array.GetLength(0)];
			for (int i = 0; i < array.GetLength(0); i++)
			{
				string[] array3 = Utils.Split(array[i], '=');
				if (array3.GetLength(0) != 2)
				{
					throw new ArgumentException(Res.GetString("InvalidDNFormat"), "distinguishedName");
				}
				array2[i].Name = array3[0].Trim();
				if (array2[i].Name.Length == 0)
				{
					throw new ArgumentException(Res.GetString("InvalidDNFormat"), "distinguishedName");
				}
				array2[i].Value = array3[1].Trim();
				if (array2[i].Value.Length == 0)
				{
					throw new ArgumentException(Res.GetString("InvalidDNFormat"), "distinguishedName");
				}
			}
			return array2;
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x00028280 File Offset: 0x00027280
		internal static bool IsValidDNFormat(string distinguishedName)
		{
			string[] array = Utils.Split(distinguishedName, ',');
			Component[] array2 = new Component[array.GetLength(0)];
			for (int i = 0; i < array.GetLength(0); i++)
			{
				string[] array3 = Utils.Split(array[i], '=');
				if (array3.GetLength(0) != 2)
				{
					return false;
				}
				array2[i].Name = array3[0].Trim();
				if (array2[i].Name.Length == 0)
				{
					return false;
				}
				array2[i].Value = array3[1].Trim();
				if (array2[i].Value.Length == 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x00028320 File Offset: 0x00027320
		public static string[] Split(string distinguishedName, char delim)
		{
			bool flag = false;
			char c = '"';
			char c2 = '\\';
			int num = 0;
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < distinguishedName.Length; i++)
			{
				char c3 = distinguishedName[i];
				if (c3 == c)
				{
					flag = !flag;
				}
				else if (c3 == c2)
				{
					if (i < distinguishedName.Length - 1)
					{
						i++;
					}
				}
				else if (!flag && c3 == delim)
				{
					arrayList.Add(distinguishedName.Substring(num, i - num));
					num = i + 1;
				}
				if (i == distinguishedName.Length - 1)
				{
					if (flag)
					{
						throw new ArgumentException(Res.GetString("InvalidDNFormat"), "distinguishedName");
					}
					arrayList.Add(distinguishedName.Substring(num, i - num + 1));
				}
			}
			string[] array = new string[arrayList.Count];
			for (int j = 0; j < arrayList.Count; j++)
			{
				array[j] = (string)arrayList[j];
			}
			return array;
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x0002841C File Offset: 0x0002741C
		internal static DirectoryContext GetNewDirectoryContext(string name, DirectoryContextType contextType, DirectoryContext context)
		{
			return new DirectoryContext(contextType, name, context);
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x00028428 File Offset: 0x00027428
		internal static void GetDomainAndUsername(DirectoryContext context, out string username, out string domain)
		{
			if (context.UserName == null || context.UserName.Length <= 0)
			{
				username = context.UserName;
				domain = null;
				return;
			}
			string userName = context.UserName;
			int num;
			if ((num = userName.IndexOf('\\')) != -1)
			{
				domain = userName.Substring(0, num);
				username = userName.Substring(num + 1, userName.Length - num - 1);
				return;
			}
			username = userName;
			domain = null;
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x00028494 File Offset: 0x00027494
		internal static IntPtr GetAuthIdentity(DirectoryContext context, LoadLibrarySafeHandle libHandle)
		{
			string text;
			string text2;
			Utils.GetDomainAndUsername(context, out text, out text2);
			IntPtr procAddress = UnsafeNativeMethods.GetProcAddress(libHandle, "DsMakePasswordCredentialsW");
			if (procAddress == (IntPtr)0)
			{
				throw ExceptionHelper.GetExceptionFromErrorCode(Marshal.GetLastWin32Error());
			}
			NativeMethods.DsMakePasswordCredentials dsMakePasswordCredentials = (NativeMethods.DsMakePasswordCredentials)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(NativeMethods.DsMakePasswordCredentials));
			IntPtr intPtr;
			int num = dsMakePasswordCredentials(text, text2, context.Password, out intPtr);
			if (num != 0)
			{
				throw ExceptionHelper.GetExceptionFromErrorCode(num);
			}
			return intPtr;
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x0002850C File Offset: 0x0002750C
		internal static void FreeAuthIdentity(IntPtr authIdentity, LoadLibrarySafeHandle libHandle)
		{
			if (authIdentity != IntPtr.Zero)
			{
				IntPtr procAddress = UnsafeNativeMethods.GetProcAddress(libHandle, "DsFreePasswordCredentials");
				if (procAddress == (IntPtr)0)
				{
					throw ExceptionHelper.GetExceptionFromErrorCode(Marshal.GetLastWin32Error());
				}
				NativeMethods.DsFreePasswordCredentials dsFreePasswordCredentials = (NativeMethods.DsFreePasswordCredentials)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(NativeMethods.DsFreePasswordCredentials));
				dsFreePasswordCredentials(authIdentity);
			}
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x00028568 File Offset: 0x00027568
		internal static IntPtr GetDSHandle(string domainControllerName, string domainName, IntPtr authIdentity, LoadLibrarySafeHandle libHandle)
		{
			IntPtr procAddress = UnsafeNativeMethods.GetProcAddress(libHandle, "DsBindWithCredW");
			if (procAddress == (IntPtr)0)
			{
				throw ExceptionHelper.GetExceptionFromErrorCode(Marshal.GetLastWin32Error());
			}
			NativeMethods.DsBindWithCred dsBindWithCred = (NativeMethods.DsBindWithCred)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(NativeMethods.DsBindWithCred));
			IntPtr intPtr;
			int num = dsBindWithCred(domainControllerName, domainName, authIdentity, out intPtr);
			if (num != 0)
			{
				throw ExceptionHelper.GetExceptionFromErrorCode(num, (domainControllerName != null) ? domainControllerName : domainName);
			}
			return intPtr;
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x000285D0 File Offset: 0x000275D0
		internal static void FreeDSHandle(IntPtr dsHandle, LoadLibrarySafeHandle libHandle)
		{
			if (dsHandle != IntPtr.Zero)
			{
				IntPtr procAddress = UnsafeNativeMethods.GetProcAddress(libHandle, "DsUnBindW");
				if (procAddress == (IntPtr)0)
				{
					throw ExceptionHelper.GetExceptionFromErrorCode(Marshal.GetLastWin32Error());
				}
				NativeMethods.DsUnBind dsUnBind = (NativeMethods.DsUnBind)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(NativeMethods.DsUnBind));
				dsUnBind(ref dsHandle);
			}
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x00028630 File Offset: 0x00027630
		internal static bool CheckCapability(DirectoryEntry rootDSE, Capability capability)
		{
			bool flag = false;
			if (rootDSE != null)
			{
				if (capability == Capability.ActiveDirectory)
				{
					using (IEnumerator enumerator = rootDSE.Properties[PropertyManager.SupportedCapabilities].GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							string text = (string)obj;
							if (string.Compare(text, SupportedCapability.ADOid, StringComparison.OrdinalIgnoreCase) == 0)
							{
								flag = true;
								break;
							}
						}
						return flag;
					}
				}
				if (capability == Capability.ActiveDirectoryApplicationMode)
				{
					using (IEnumerator enumerator2 = rootDSE.Properties[PropertyManager.SupportedCapabilities].GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							string text2 = (string)obj2;
							if (string.Compare(text2, SupportedCapability.ADAMOid, StringComparison.OrdinalIgnoreCase) == 0)
							{
								flag = true;
								break;
							}
						}
						return flag;
					}
				}
				if (capability == Capability.ActiveDirectoryOrADAM)
				{
					foreach (object obj3 in rootDSE.Properties[PropertyManager.SupportedCapabilities])
					{
						string text3 = (string)obj3;
						if (string.Compare(text3, SupportedCapability.ADAMOid, StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(text3, SupportedCapability.ADOid, StringComparison.OrdinalIgnoreCase) == 0)
						{
							flag = true;
							break;
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x00028794 File Offset: 0x00027794
		internal static DirectoryEntry GetCrossRefEntry(DirectoryContext context, DirectoryEntry partitionsEntry, string partitionName)
		{
			StringBuilder stringBuilder = new StringBuilder(15);
			stringBuilder.Append("(&(");
			stringBuilder.Append(PropertyManager.ObjectCategory);
			stringBuilder.Append("=crossRef)(");
			stringBuilder.Append(PropertyManager.SystemFlags);
			stringBuilder.Append(":1.2.840.113556.1.4.804:=");
			stringBuilder.Append(1);
			stringBuilder.Append(")(!(");
			stringBuilder.Append(PropertyManager.SystemFlags);
			stringBuilder.Append(":1.2.840.113556.1.4.803:=");
			stringBuilder.Append(2);
			stringBuilder.Append("))(");
			stringBuilder.Append(PropertyManager.NCName);
			stringBuilder.Append("=");
			stringBuilder.Append(Utils.GetEscapedFilterValue(partitionName));
			stringBuilder.Append("))");
			string text = stringBuilder.ToString();
			ADSearcher adsearcher = new ADSearcher(partitionsEntry, text, new string[] { PropertyManager.DistinguishedName }, SearchScope.OneLevel, false, false);
			SearchResult searchResult = null;
			try
			{
				searchResult = adsearcher.FindOne();
				if (searchResult == null)
				{
					throw new ActiveDirectoryObjectNotFoundException(Res.GetString("AppNCNotFound"), typeof(ActiveDirectoryPartition), partitionName);
				}
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(context, ex);
			}
			string text2 = (string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.DistinguishedName);
			return searchResult.GetDirectoryEntry();
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x000288D8 File Offset: 0x000278D8
		internal static ActiveDirectoryTransportType GetTransportTypeFromDN(string DN)
		{
			string rdnFromDN = Utils.GetRdnFromDN(DN);
			Component[] dncomponents = Utils.GetDNComponents(rdnFromDN);
			string value = dncomponents[0].Value;
			if (string.Compare(value, "IP", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return ActiveDirectoryTransportType.Rpc;
			}
			if (string.Compare(value, "SMTP", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return ActiveDirectoryTransportType.Smtp;
			}
			string @string = Res.GetString("UnknownTransport", new object[] { value });
			throw new ActiveDirectoryOperationException(@string);
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x00028940 File Offset: 0x00027940
		internal static string GetDNFromTransportType(ActiveDirectoryTransportType transport, DirectoryContext context)
		{
			string text = DirectoryEntryManager.ExpandWellKnownDN(context, WellKnownDN.SitesContainer);
			string text2 = "CN=Inter-Site Transports," + text;
			if (transport == ActiveDirectoryTransportType.Rpc)
			{
				return "CN=IP," + text2;
			}
			return "CN=SMTP," + text2;
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x0002897C File Offset: 0x0002797C
		internal static string GetServerNameFromInvocationID(string serverObjectDN, Guid invocationID, DirectoryServer server)
		{
			string text = null;
			if (serverObjectDN == null)
			{
				string text2 = ((server is DomainController) ? ((DomainController)server).SiteObjectName : ((AdamInstance)server).SiteObjectName);
				DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(server.Context, text2);
				byte[] array = invocationID.ToByteArray();
				IntPtr intPtr = (IntPtr)0;
				string text3 = null;
				int num = UnsafeNativeMethods.ADsEncodeBinaryData(array, array.Length, ref intPtr);
				if (num == 0)
				{
					try
					{
						text3 = Marshal.PtrToStringUni(intPtr);
						goto IL_0092;
					}
					finally
					{
						if (intPtr != (IntPtr)0)
						{
							UnsafeNativeMethods.FreeADsMem(intPtr);
						}
					}
					goto IL_007D;
					IL_0092:
					ADSearcher adsearcher = new ADSearcher(directoryEntry, "(&(objectClass=nTDSDSA)(invocationID=" + text3 + "))", new string[] { "distinguishedName" }, SearchScope.Subtree, false, false);
					try
					{
						SearchResult searchResult = adsearcher.FindOne();
						if (searchResult != null)
						{
							DirectoryEntry parent = searchResult.GetDirectoryEntry().Parent;
							text = (string)PropertyManager.GetPropertyValue(server.Context, parent, PropertyManager.DnsHostName);
						}
						return text;
					}
					catch (COMException ex)
					{
						throw ExceptionHelper.GetExceptionFromCOMException(server.Context, ex);
					}
					goto IL_010C;
				}
				IL_007D:
				throw ExceptionHelper.GetExceptionFromCOMException(new COMException(ExceptionHelper.GetErrorMessage(num, true), num));
			}
			IL_010C:
			DirectoryEntry directoryEntry2 = DirectoryEntryManager.GetDirectoryEntry(server.Context, serverObjectDN);
			try
			{
				text = (string)PropertyManager.GetPropertyValue(directoryEntry2.Parent, PropertyManager.DnsHostName);
			}
			catch (COMException ex2)
			{
				if (ex2.ErrorCode == -2147016656)
				{
					return null;
				}
				throw ExceptionHelper.GetExceptionFromCOMException(server.Context, ex2);
			}
			if (!(server is AdamInstance))
			{
				return text;
			}
			int num2 = (int)PropertyManager.GetPropertyValue(server.Context, directoryEntry2, PropertyManager.MsDSPortLDAP);
			if (num2 != 389)
			{
				return text + ":" + num2;
			}
			return text;
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x00028B48 File Offset: 0x00027B48
		internal static int GetRandomIndex(int count)
		{
			Random random = new Random();
			int num = random.Next();
			return num % count;
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x00028B68 File Offset: 0x00027B68
		internal static bool Impersonate(DirectoryContext context)
		{
			IntPtr intPtr = (IntPtr)0;
			if (context.UserName == null && context.Password == null)
			{
				return false;
			}
			string text;
			string text2;
			Utils.GetDomainAndUsername(context, out text, out text2);
			if (UnsafeNativeMethods.LogonUserW(text, text2, context.Password, Utils.LOGON32_LOGON_NEW_CREDENTIALS, Utils.LOGON32_PROVIDER_WINNT50, ref intPtr) == 0)
			{
				throw ExceptionHelper.GetExceptionFromErrorCode(Marshal.GetLastWin32Error());
			}
			try
			{
				if (UnsafeNativeMethods.ImpersonateLoggedOnUser(intPtr) == 0)
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					throw ExceptionHelper.GetExceptionFromErrorCode(lastWin32Error);
				}
			}
			finally
			{
				if (intPtr != (IntPtr)0)
				{
					UnsafeNativeMethods.CloseHandle(intPtr);
				}
			}
			return true;
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x00028C00 File Offset: 0x00027C00
		internal static void ImpersonateAnonymous()
		{
			IntPtr intPtr = (IntPtr)0;
			intPtr = UnsafeNativeMethods.OpenThread(Utils.THREAD_ALL_ACCESS, false, UnsafeNativeMethods.GetCurrentThreadId());
			if (intPtr == (IntPtr)0)
			{
				throw ExceptionHelper.GetExceptionFromErrorCode(Marshal.GetLastWin32Error());
			}
			try
			{
				if (UnsafeNativeMethods.ImpersonateAnonymousToken(intPtr) == 0)
				{
					throw ExceptionHelper.GetExceptionFromErrorCode(Marshal.GetLastWin32Error());
				}
			}
			finally
			{
				if (intPtr != (IntPtr)0)
				{
					UnsafeNativeMethods.CloseHandle(intPtr);
				}
			}
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x00028C7C File Offset: 0x00027C7C
		internal static void Revert()
		{
			if (UnsafeNativeMethods.RevertToSelf() == 0)
			{
				throw ExceptionHelper.GetExceptionFromErrorCode(Marshal.GetLastWin32Error());
			}
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x00028CA0 File Offset: 0x00027CA0
		internal static string GetPolicyServerName(DirectoryContext context, bool isForest, bool needPdc, string source)
		{
			PrivateLocatorFlags privateLocatorFlags = PrivateLocatorFlags.DirectoryServicesRequired;
			string text;
			if (context.isDomain())
			{
				if (needPdc)
				{
					privateLocatorFlags |= PrivateLocatorFlags.PdcRequired;
				}
				text = Locator.GetDomainControllerInfo(null, source, null, (long)privateLocatorFlags).DomainControllerName.Substring(2);
			}
			else if (isForest)
			{
				if (needPdc)
				{
					privateLocatorFlags |= PrivateLocatorFlags.PdcRequired;
					text = Locator.GetDomainControllerInfo(null, source, null, (long)privateLocatorFlags).DomainControllerName.Substring(2);
				}
				else if (context.ContextType == DirectoryContextType.DirectoryServer)
				{
					DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(context, WellKnownDN.RootDSE);
					string text2 = (string)PropertyManager.GetPropertyValue(context, directoryEntry, PropertyManager.DefaultNamingContext);
					string text3 = (string)PropertyManager.GetPropertyValue(context, directoryEntry, PropertyManager.RootDomainNamingContext);
					if (Utils.Compare(text2, text3) == 0)
					{
						text = context.Name;
					}
					else
					{
						text = Locator.GetDomainControllerInfo(null, source, null, (long)privateLocatorFlags).DomainControllerName.Substring(2);
					}
				}
				else
				{
					text = Locator.GetDomainControllerInfo(null, source, null, (long)privateLocatorFlags).DomainControllerName.Substring(2);
				}
			}
			else
			{
				text = context.Name;
			}
			return text;
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x00028D8C File Offset: 0x00027D8C
		internal static IntPtr GetPolicyHandle(string serverName)
		{
			IntPtr intPtr = (IntPtr)0;
			LSA_OBJECT_ATTRIBUTES lsa_OBJECT_ATTRIBUTES = new LSA_OBJECT_ATTRIBUTES();
			IntPtr intPtr2 = (IntPtr)0;
			int policy_VIEW_LOCAL_INFORMATION = Utils.POLICY_VIEW_LOCAL_INFORMATION;
			LSA_UNICODE_STRING lsa_UNICODE_STRING = new LSA_UNICODE_STRING();
			intPtr2 = Marshal.StringToHGlobalUni(serverName);
			UnsafeNativeMethods.RtlInitUnicodeString(lsa_UNICODE_STRING, intPtr2);
			IntPtr intPtr3;
			try
			{
				int num = UnsafeNativeMethods.LsaOpenPolicy(lsa_UNICODE_STRING, lsa_OBJECT_ATTRIBUTES, policy_VIEW_LOCAL_INFORMATION, out intPtr);
				if (num != 0)
				{
					throw ExceptionHelper.GetExceptionFromErrorCode(UnsafeNativeMethods.LsaNtStatusToWinError(num), serverName);
				}
				intPtr3 = intPtr;
			}
			finally
			{
				if (intPtr2 != (IntPtr)0)
				{
					Marshal.FreeHGlobal(intPtr2);
				}
			}
			return intPtr3;
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x00028E14 File Offset: 0x00027E14
		internal static Hashtable GetValuesWithRangeRetrieval(DirectoryEntry searchRootEntry, string filter, ArrayList propertiesToLoad, SearchScope searchScope)
		{
			return Utils.GetValuesWithRangeRetrieval(searchRootEntry, filter, propertiesToLoad, new ArrayList(), searchScope);
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x00028E24 File Offset: 0x00027E24
		internal static Hashtable GetValuesWithRangeRetrieval(DirectoryEntry searchRootEntry, string filter, ArrayList propertiesWithRangeRetrieval, ArrayList propertiesWithoutRangeRetrieval, SearchScope searchScope)
		{
			ADSearcher adsearcher = new ADSearcher(searchRootEntry, filter, new string[0], searchScope, false, false);
			int num = 0;
			Hashtable hashtable = new Hashtable();
			Hashtable hashtable2 = new Hashtable();
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			foreach (object obj in propertiesWithoutRangeRetrieval)
			{
				string text = (string)obj;
				string text2 = text.ToLower(CultureInfo.InvariantCulture);
				arrayList.Add(text2);
				hashtable.Add(text2, new ArrayList());
				adsearcher.PropertiesToLoad.Add(text);
			}
			foreach (object obj2 in propertiesWithRangeRetrieval)
			{
				string text3 = (string)obj2;
				string text4 = text3.ToLower(CultureInfo.InvariantCulture);
				arrayList2.Add(text4);
				hashtable.Add(text4, new ArrayList());
			}
			for (;;)
			{
				foreach (object obj3 in arrayList2)
				{
					string text5 = (string)obj3;
					string text6 = string.Concat(new object[] { text5, ";range=", num, "-*" });
					adsearcher.PropertiesToLoad.Add(text6);
					hashtable2.Add(text5.ToLower(CultureInfo.InvariantCulture), text6);
				}
				arrayList2.Clear();
				SearchResult searchResult = adsearcher.FindOne();
				if (searchResult == null)
				{
					break;
				}
				using (IEnumerator enumerator4 = searchResult.Properties.PropertyNames.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						object obj4 = enumerator4.Current;
						string text7 = (string)obj4;
						int num2 = text7.IndexOf(';');
						string text8;
						if (num2 != -1)
						{
							text8 = text7.Substring(0, num2);
						}
						else
						{
							text8 = text7;
						}
						if (hashtable2.Contains(text8) || arrayList.Contains(text8))
						{
							ArrayList arrayList3 = (ArrayList)hashtable[text8];
							arrayList3.AddRange(searchResult.Properties[text7]);
							if (hashtable2.Contains(text8))
							{
								string text9 = (string)hashtable2[text8];
								if (text7.Length >= text9.Length && Utils.Compare(text9, 0, text9.Length, text7, 0, text9.Length) != 0)
								{
									arrayList2.Add(text8);
									num += searchResult.Properties[text7].Count;
								}
							}
						}
					}
					goto IL_02BA;
				}
				break;
				IL_02BA:
				adsearcher.PropertiesToLoad.Clear();
				hashtable2.Clear();
				if (arrayList2.Count <= 0)
				{
					return hashtable;
				}
			}
			throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DSNotFound"));
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x00029140 File Offset: 0x00028140
		internal static ArrayList GetReplicaList(DirectoryContext context, string partitionName, string siteName, bool isDefaultNC, bool isADAM, bool isGC)
		{
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			Hashtable hashtable = new Hashtable();
			Hashtable hashtable2 = new Hashtable();
			StringBuilder stringBuilder = new StringBuilder(10);
			StringBuilder stringBuilder2 = new StringBuilder(10);
			StringBuilder stringBuilder3 = new StringBuilder(10);
			StringBuilder stringBuilder4 = new StringBuilder(10);
			bool flag = false;
			string text = null;
			try
			{
				text = DirectoryEntryManager.ExpandWellKnownDN(context, WellKnownDN.ConfigurationNamingContext);
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(context, ex);
			}
			if (partitionName != null && !isDefaultNC)
			{
				DistinguishedName distinguishedName = new DistinguishedName(partitionName);
				DistinguishedName distinguishedName2 = new DistinguishedName(text);
				DistinguishedName distinguishedName3 = new DistinguishedName("CN=Schema," + text);
				if (!distinguishedName2.Equals(distinguishedName) && !distinguishedName3.Equals(distinguishedName))
				{
					flag = true;
				}
			}
			if (flag)
			{
				DirectoryEntry directoryEntry = null;
				DirectoryEntry directoryEntry2 = null;
				try
				{
					directoryEntry = DirectoryEntryManager.GetDirectoryEntry(context, "CN=Partitions," + text);
					string text2;
					if (isADAM)
					{
						text2 = Utils.GetAdamDnsHostNameFromNTDSA(context, (string)PropertyManager.GetPropertyValue(context, directoryEntry, PropertyManager.FsmoRoleOwner));
					}
					else
					{
						text2 = Utils.GetDnsHostNameFromNTDSA(context, (string)PropertyManager.GetPropertyValue(context, directoryEntry, PropertyManager.FsmoRoleOwner));
					}
					DirectoryContext newDirectoryContext = Utils.GetNewDirectoryContext(text2, DirectoryContextType.DirectoryServer, context);
					directoryEntry2 = DirectoryEntryManager.GetDirectoryEntry(newDirectoryContext, "CN=Partitions," + text);
					string text3 = string.Concat(new string[]
					{
						"(&(",
						PropertyManager.ObjectCategory,
						"=crossRef)(",
						PropertyManager.NCName,
						"=",
						Utils.GetEscapedFilterValue(partitionName),
						"))"
					});
					ArrayList arrayList3 = new ArrayList();
					arrayList3.Add(PropertyManager.MsDSNCReplicaLocations);
					arrayList3.Add(PropertyManager.MsDSNCROReplicaLocations);
					Hashtable hashtable3 = null;
					try
					{
						hashtable3 = Utils.GetValuesWithRangeRetrieval(directoryEntry2, text3, arrayList3, SearchScope.OneLevel);
					}
					catch (COMException ex2)
					{
						throw ExceptionHelper.GetExceptionFromCOMException(newDirectoryContext, ex2);
					}
					catch (ActiveDirectoryObjectNotFoundException)
					{
						return arrayList2;
					}
					ArrayList arrayList4 = (ArrayList)hashtable3[PropertyManager.MsDSNCReplicaLocations.ToLower(CultureInfo.InvariantCulture)];
					ArrayList arrayList5 = (ArrayList)hashtable3[PropertyManager.MsDSNCROReplicaLocations.ToLower(CultureInfo.InvariantCulture)];
					if (arrayList4.Count == 0)
					{
						return arrayList2;
					}
					foreach (object obj in arrayList4)
					{
						string text4 = (string)obj;
						stringBuilder.Append("(");
						stringBuilder.Append(PropertyManager.DistinguishedName);
						stringBuilder.Append("=");
						stringBuilder.Append(Utils.GetEscapedFilterValue(text4));
						stringBuilder.Append(")");
						stringBuilder2.Append("(");
						stringBuilder2.Append(PropertyManager.DistinguishedName);
						stringBuilder2.Append("=");
						stringBuilder2.Append(Utils.GetEscapedFilterValue(Utils.GetPartialDN(text4, 1)));
						stringBuilder2.Append(")");
					}
					foreach (object obj2 in arrayList5)
					{
						string text5 = (string)obj2;
						stringBuilder3.Append("(");
						stringBuilder3.Append(PropertyManager.DistinguishedName);
						stringBuilder3.Append("=");
						stringBuilder3.Append(Utils.GetEscapedFilterValue(text5));
						stringBuilder3.Append(")");
						stringBuilder4.Append("(");
						stringBuilder4.Append(PropertyManager.DistinguishedName);
						stringBuilder4.Append("=");
						stringBuilder4.Append(Utils.GetEscapedFilterValue(Utils.GetPartialDN(text5, 1)));
						stringBuilder4.Append(")");
					}
				}
				catch (COMException ex3)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(context, ex3);
				}
				finally
				{
					if (directoryEntry != null)
					{
						directoryEntry.Dispose();
					}
					if (directoryEntry2 != null)
					{
						directoryEntry2.Dispose();
					}
				}
			}
			DirectoryEntry directoryEntry3 = null;
			try
			{
				string text6;
				if (siteName != null)
				{
					text6 = "CN=Servers,CN=" + siteName + ",CN=Sites," + text;
				}
				else
				{
					text6 = "CN=Sites," + text;
				}
				directoryEntry3 = DirectoryEntryManager.GetDirectoryEntry(context, text6);
				string text7;
				if (stringBuilder.ToString().Length == 0)
				{
					if (isDefaultNC)
					{
						text7 = string.Concat(new string[]
						{
							"(|(&(",
							PropertyManager.ObjectCategory,
							"=nTDSDSA)(",
							PropertyManager.HasMasterNCs,
							"=",
							Utils.GetEscapedFilterValue(partitionName),
							"))(&(",
							PropertyManager.ObjectCategory,
							"=nTDSDSARO)(",
							PropertyManager.MsDSHasFullReplicaNCs,
							"=",
							Utils.GetEscapedFilterValue(partitionName),
							"))(",
							PropertyManager.ObjectCategory,
							"=server))"
						});
					}
					else if (isGC)
					{
						text7 = string.Concat(new string[]
						{
							"(|(&(",
							PropertyManager.ObjectCategory,
							"=nTDSDSA)(",
							PropertyManager.Options,
							":1.2.840.113556.1.4.804:=1))(&(",
							PropertyManager.ObjectCategory,
							"=nTDSDSARO)(",
							PropertyManager.Options,
							":1.2.840.113556.1.4.804:=1))(",
							PropertyManager.ObjectCategory,
							"=server))"
						});
					}
					else
					{
						text7 = string.Concat(new string[]
						{
							"(|(",
							PropertyManager.ObjectCategory,
							"=nTDSDSA)(",
							PropertyManager.ObjectCategory,
							"=nTDSDSARO)(",
							PropertyManager.ObjectCategory,
							"=server))"
						});
					}
				}
				else if (isGC)
				{
					if (stringBuilder3.Length > 0)
					{
						text7 = string.Concat(new string[]
						{
							"(|(&(",
							PropertyManager.ObjectCategory,
							"=nTDSDSA)(",
							PropertyManager.Options,
							":1.2.840.113556.1.4.804:=1)(",
							PropertyManager.MsDSHasMasterNCs,
							"=",
							Utils.GetEscapedFilterValue(partitionName),
							")(|",
							stringBuilder.ToString(),
							"))(&(",
							PropertyManager.ObjectCategory,
							"=nTDSDSARO)(",
							PropertyManager.Options,
							":1.2.840.113556.1.4.804:=1)(|",
							stringBuilder3.ToString(),
							"))(&(",
							PropertyManager.ObjectCategory,
							"=server)(|",
							stringBuilder2.ToString(),
							"))(&(",
							PropertyManager.ObjectCategory,
							"=server)(|",
							stringBuilder4.ToString(),
							")))"
						});
					}
					else
					{
						text7 = string.Concat(new string[]
						{
							"(|(&(",
							PropertyManager.ObjectCategory,
							"=nTDSDSA)(",
							PropertyManager.Options,
							":1.2.840.113556.1.4.804:=1)(",
							PropertyManager.MsDSHasMasterNCs,
							"=",
							Utils.GetEscapedFilterValue(partitionName),
							")(|",
							stringBuilder.ToString(),
							"))(&(",
							PropertyManager.ObjectCategory,
							"=server)(|",
							stringBuilder2.ToString(),
							")))"
						});
					}
				}
				else if (stringBuilder3.Length > 0)
				{
					text7 = string.Concat(new string[]
					{
						"(|(&(",
						PropertyManager.ObjectCategory,
						"=nTDSDSA)(",
						PropertyManager.MsDSHasMasterNCs,
						"=",
						Utils.GetEscapedFilterValue(partitionName),
						")(|",
						stringBuilder.ToString(),
						"))(&(",
						PropertyManager.ObjectCategory,
						"=nTDSDSARO)(|",
						stringBuilder3.ToString(),
						"))(&(",
						PropertyManager.ObjectCategory,
						"=server)(|",
						stringBuilder2.ToString(),
						"))(&(",
						PropertyManager.ObjectCategory,
						"=server)(|",
						stringBuilder4.ToString(),
						")))"
					});
				}
				else
				{
					text7 = string.Concat(new string[]
					{
						"(|(&(",
						PropertyManager.ObjectCategory,
						"=nTDSDSA)(",
						PropertyManager.MsDSHasMasterNCs,
						"=",
						Utils.GetEscapedFilterValue(partitionName),
						")(|",
						stringBuilder.ToString(),
						"))(&(",
						PropertyManager.ObjectCategory,
						"=server)(|",
						stringBuilder2.ToString(),
						")))"
					});
				}
				ADSearcher adsearcher = new ADSearcher(directoryEntry3, text7, new string[0], SearchScope.Subtree);
				SearchResultCollection searchResultCollection = null;
				bool flag2 = false;
				ArrayList arrayList6 = new ArrayList();
				int num = 0;
				string text8 = PropertyManager.MsDSHasInstantiatedNCs + ";range=0-*";
				adsearcher.PropertiesToLoad.Add(PropertyManager.DistinguishedName);
				adsearcher.PropertiesToLoad.Add(PropertyManager.DnsHostName);
				adsearcher.PropertiesToLoad.Add(text8);
				adsearcher.PropertiesToLoad.Add(PropertyManager.ObjectCategory);
				if (isADAM)
				{
					adsearcher.PropertiesToLoad.Add(PropertyManager.MsDSPortLDAP);
				}
				try
				{
					string text9 = "CN=NTDS-DSA";
					string text10 = "CN=NTDS-DSA-RO";
					using (SearchResultCollection searchResultCollection = adsearcher.FindAll())
					{
						foreach (object obj3 in searchResultCollection)
						{
							SearchResult searchResult = (SearchResult)obj3;
							string text11 = (string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.ObjectCategory);
							if (text11.Length >= text9.Length && Utils.Compare(text11, 0, text9.Length, text9, 0, text9.Length) == 0)
							{
								string text12 = (string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.DistinguishedName);
								if (flag)
								{
									if (text11.Length < text10.Length || Utils.Compare(text11, 0, text10.Length, text10, 0, text10.Length) != 0)
									{
										string text13 = null;
										if (!searchResult.Properties.Contains(text8))
										{
											using (IEnumerator enumerator2 = searchResult.Properties.PropertyNames.GetEnumerator())
											{
												while (enumerator2.MoveNext())
												{
													object obj4 = enumerator2.Current;
													string text14 = (string)obj4;
													if (text14.Length >= PropertyManager.MsDSHasInstantiatedNCs.Length && Utils.Compare(text14, 0, PropertyManager.MsDSHasInstantiatedNCs.Length, PropertyManager.MsDSHasInstantiatedNCs, 0, PropertyManager.MsDSHasInstantiatedNCs.Length) == 0)
													{
														text13 = text14;
														break;
													}
												}
												goto IL_0AF1;
											}
											goto IL_0AED;
										}
										goto IL_0AED;
										IL_0AF1:
										if (text13 == null)
										{
											continue;
										}
										bool flag3 = false;
										int num2 = 0;
										foreach (object obj5 in searchResult.Properties[text13])
										{
											string text15 = (string)obj5;
											if (text15.Length - 13 >= partitionName.Length && Utils.Compare(text15, 13, partitionName.Length, partitionName, 0, partitionName.Length) == 0)
											{
												flag3 = true;
												if (string.Compare(text15, 10, "0", 0, 1, StringComparison.OrdinalIgnoreCase) == 0)
												{
													arrayList.Add(text12);
													if (isADAM)
													{
														hashtable2.Add(text12, (int)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.MsDSPortLDAP));
														break;
													}
													break;
												}
											}
											num2++;
										}
										if (!flag3 && text13.Length >= text8.Length && Utils.Compare(text13, 0, text8.Length, text8, 0, text8.Length) != 0)
										{
											flag2 = true;
											arrayList6.Add(text12);
											num = num2;
											continue;
										}
										continue;
										IL_0AED:
										text13 = text8;
										goto IL_0AF1;
									}
									arrayList.Add(text12);
									if (isADAM)
									{
										hashtable2.Add(text12, (int)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.MsDSPortLDAP));
									}
								}
								else
								{
									arrayList.Add(text12);
									if (isADAM)
									{
										hashtable2.Add(text12, (int)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.MsDSPortLDAP));
									}
								}
							}
							else if (searchResult.Properties.Contains(PropertyManager.DnsHostName))
							{
								hashtable.Add("CN=NTDS Settings," + (string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.DistinguishedName), (string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.DnsHostName));
							}
						}
					}
					if (flag2)
					{
						do
						{
							StringBuilder stringBuilder5 = new StringBuilder(20);
							if (arrayList6.Count > 1)
							{
								stringBuilder5.Append("(|");
							}
							foreach (object obj6 in arrayList6)
							{
								string text16 = (string)obj6;
								stringBuilder5.Append("(");
								stringBuilder5.Append(PropertyManager.NCName);
								stringBuilder5.Append("=");
								stringBuilder5.Append(Utils.GetEscapedFilterValue(text16));
								stringBuilder5.Append(")");
							}
							if (arrayList6.Count > 1)
							{
								stringBuilder5.Append(")");
							}
							arrayList6.Clear();
							flag2 = false;
							adsearcher.Filter = string.Concat(new string[]
							{
								"(&(",
								PropertyManager.ObjectCategory,
								"=nTDSDSA)",
								stringBuilder5.ToString(),
								")"
							});
							string text17 = string.Concat(new object[]
							{
								PropertyManager.MsDSHasInstantiatedNCs,
								";range=",
								num,
								"-*"
							});
							adsearcher.PropertiesToLoad.Clear();
							adsearcher.PropertiesToLoad.Add(text17);
							adsearcher.PropertiesToLoad.Add(PropertyManager.DistinguishedName);
							SearchResultCollection searchResultCollection2 = adsearcher.FindAll();
							try
							{
								foreach (object obj7 in searchResultCollection2)
								{
									SearchResult searchResult2 = (SearchResult)obj7;
									string text18 = (string)PropertyManager.GetSearchResultPropertyValue(searchResult2, PropertyManager.DistinguishedName);
									string text19 = null;
									if (!searchResult2.Properties.Contains(text17))
									{
										using (IEnumerator enumerator2 = searchResult2.Properties.PropertyNames.GetEnumerator())
										{
											while (enumerator2.MoveNext())
											{
												object obj8 = enumerator2.Current;
												string text20 = (string)obj8;
												if (string.Compare(text20, 0, PropertyManager.MsDSHasInstantiatedNCs, 0, PropertyManager.MsDSHasInstantiatedNCs.Length, StringComparison.OrdinalIgnoreCase) == 0)
												{
													text19 = text20;
													break;
												}
											}
											goto IL_0ECB;
										}
										goto IL_0EC7;
									}
									goto IL_0EC7;
									IL_0ECB:
									if (text19 == null)
									{
										continue;
									}
									bool flag4 = false;
									int num3 = 0;
									foreach (object obj9 in searchResult2.Properties[text19])
									{
										string text21 = (string)obj9;
										if (text21.Length - 13 >= partitionName.Length && Utils.Compare(text21, 13, partitionName.Length, partitionName, 0, partitionName.Length) == 0)
										{
											flag4 = true;
											if (string.Compare(text21, 10, "0", 0, 1, StringComparison.OrdinalIgnoreCase) == 0)
											{
												arrayList.Add(text18);
												if (isADAM)
												{
													hashtable2.Add(text18, (int)PropertyManager.GetSearchResultPropertyValue(searchResult2, PropertyManager.MsDSPortLDAP));
													break;
												}
												break;
											}
										}
										num3++;
									}
									if (!flag4 && text19.Length >= text17.Length && Utils.Compare(text19, 0, text17.Length, text17, 0, text17.Length) != 0)
									{
										flag2 = true;
										arrayList6.Add(text18);
										num += num3;
										continue;
									}
									continue;
									IL_0EC7:
									text19 = text17;
									goto IL_0ECB;
								}
							}
							finally
							{
								searchResultCollection2.Dispose();
							}
						}
						while (flag2);
					}
				}
				catch (COMException ex4)
				{
					if (ex4.ErrorCode == -2147016656 && siteName != null)
					{
						return arrayList2;
					}
					throw ExceptionHelper.GetExceptionFromCOMException(context, ex4);
				}
			}
			finally
			{
				if (directoryEntry3 != null)
				{
					directoryEntry3.Dispose();
				}
			}
			foreach (object obj10 in arrayList)
			{
				string text22 = (string)obj10;
				string text23 = (string)hashtable[text22];
				if (text23 == null)
				{
					if (isADAM)
					{
						throw new ActiveDirectoryOperationException(Res.GetString("NoHostNameOrPortNumber", new object[] { text22 }));
					}
					throw new ActiveDirectoryOperationException(Res.GetString("NoHostName", new object[] { text22 }));
				}
				else
				{
					if (isADAM && hashtable2[text22] == null)
					{
						throw new ActiveDirectoryOperationException(Res.GetString("NoHostNameOrPortNumber", new object[] { text22 }));
					}
					if (isADAM)
					{
						arrayList2.Add(text23 + ":" + (int)hashtable2[text22]);
					}
					else
					{
						arrayList2.Add(text23);
					}
				}
			}
			return arrayList2;
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x0002A460 File Offset: 0x00029460
		internal static string GetEscapedFilterValue(string filterValue)
		{
			char[] array = new char[] { '(', ')', '*', '\\' };
			int num = filterValue.IndexOfAny(array);
			if (num != -1)
			{
				StringBuilder stringBuilder = new StringBuilder(2 * filterValue.Length);
				stringBuilder.Append(filterValue.Substring(0, num));
				for (int i = num; i < filterValue.Length; i++)
				{
					char c = filterValue[i];
					switch (c)
					{
					case '(':
						stringBuilder.Append("\\28");
						break;
					case ')':
						stringBuilder.Append("\\29");
						break;
					case '*':
						stringBuilder.Append("\\2A");
						break;
					default:
						if (c != '\\')
						{
							stringBuilder.Append(filterValue[i]);
						}
						else
						{
							stringBuilder.Append("\\5C");
						}
						break;
					}
				}
				return stringBuilder.ToString();
			}
			return filterValue;
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x0002A534 File Offset: 0x00029534
		internal static string GetEscapedPath(string originalPath)
		{
			NativeComInterfaces.IAdsPathname adsPathname = (NativeComInterfaces.IAdsPathname)new NativeComInterfaces.Pathname();
			return adsPathname.GetEscapedElement(0, originalPath);
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x0002A554 File Offset: 0x00029554
		internal static int Compare(string s1, string s2, uint compareFlags)
		{
			int num = 0;
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			try
			{
				intPtr = Marshal.StringToHGlobalUni(s1);
				int length = s1.Length;
				intPtr2 = Marshal.StringToHGlobalUni(s2);
				int length2 = s2.Length;
				num = NativeMethods.CompareString(Utils.LCID, compareFlags, intPtr, length, intPtr2, length2);
				if (num == 0)
				{
					throw ExceptionHelper.GetExceptionFromErrorCode(Marshal.GetLastWin32Error());
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
				if (intPtr2 != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr2);
				}
			}
			return num - 2;
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x0002A5EC File Offset: 0x000295EC
		internal static int Compare(string s1, string s2)
		{
			return Utils.Compare(s1, s2, Utils.DEFAULT_CMP_FLAGS);
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x0002A5FA File Offset: 0x000295FA
		internal static int Compare(string s1, int offset1, int length1, string s2, int offset2, int length2)
		{
			return Utils.Compare(s1.Substring(offset1, length1), s2.Substring(offset2, length2));
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0002A613 File Offset: 0x00029613
		internal static int Compare(string s1, int offset1, int length1, string s2, int offset2, int length2, uint compareFlags)
		{
			return Utils.Compare(s1.Substring(offset1, length1), s2.Substring(offset2, length2), compareFlags);
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x0002A630 File Offset: 0x00029630
		internal static string SplitServerNameAndPortNumber(string serverName, out string portNumber)
		{
			portNumber = null;
			int num = serverName.LastIndexOf(':');
			if (num == -1)
			{
				return serverName;
			}
			bool flag = serverName.StartsWith("[");
			if (!flag)
			{
				try
				{
					IPAddress ipaddress = IPAddress.Parse(serverName);
					if (ipaddress.AddressFamily == AddressFamily.InterNetworkV6)
					{
						return serverName;
					}
				}
				catch (FormatException)
				{
				}
				portNumber = serverName.Substring(num + 1);
				serverName = serverName.Substring(0, num);
				return serverName;
			}
			if (serverName.EndsWith("]"))
			{
				serverName = serverName.Substring(1, serverName.Length - 2);
				return serverName;
			}
			int num2 = serverName.LastIndexOf("]:");
			if (num2 == -1 || num2 + 1 != num)
			{
				return serverName;
			}
			portNumber = serverName.Substring(num + 1);
			serverName = serverName.Substring(1, num2 - 1);
			return serverName;
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x0002A6F4 File Offset: 0x000296F4
		internal static string GetNtAuthorityString()
		{
			if (Utils.NTAuthorityString == null)
			{
				SecurityIdentifier securityIdentifier = new SecurityIdentifier("S-1-5-18");
				NTAccount ntaccount = (NTAccount)securityIdentifier.Translate(typeof(NTAccount));
				int num = ntaccount.Value.IndexOf('\\');
				Utils.NTAuthorityString = ntaccount.Value.Substring(0, num);
			}
			return Utils.NTAuthorityString;
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x0002A750 File Offset: 0x00029750
		internal static IntPtr AddToIntPtr(IntPtr intptr, int offset)
		{
			checked
			{
				if (IntPtr.Size == 4)
				{
					uint num = intptr.ToPointer() + (uint)offset;
					return new IntPtr(num);
				}
				return (IntPtr)((long)intptr + unchecked((long)offset));
			}
		}

		// Token: 0x040006F2 RID: 1778
		private static int LOGON32_LOGON_NEW_CREDENTIALS = 9;

		// Token: 0x040006F3 RID: 1779
		private static int LOGON32_PROVIDER_WINNT50 = 3;

		// Token: 0x040006F4 RID: 1780
		private static int POLICY_VIEW_LOCAL_INFORMATION = 1;

		// Token: 0x040006F5 RID: 1781
		private static uint STANDARD_RIGHTS_REQUIRED = 983040U;

		// Token: 0x040006F6 RID: 1782
		private static uint SYNCHRONIZE = 1048576U;

		// Token: 0x040006F7 RID: 1783
		private static uint THREAD_ALL_ACCESS = Utils.STANDARD_RIGHTS_REQUIRED | Utils.SYNCHRONIZE | 1023U;

		// Token: 0x040006F8 RID: 1784
		internal static AuthenticationTypes DefaultAuthType = AuthenticationTypes.Secure | AuthenticationTypes.Signing | AuthenticationTypes.Sealing;

		// Token: 0x040006F9 RID: 1785
		private static uint LANG_ENGLISH = 9U;

		// Token: 0x040006FA RID: 1786
		private static uint SUBLANG_ENGLISH_US = 1U;

		// Token: 0x040006FB RID: 1787
		private static uint SORT_DEFAULT = 0U;

		// Token: 0x040006FC RID: 1788
		private static uint LANGID = (uint)(((int)((ushort)Utils.SUBLANG_ENGLISH_US) << 10) | (int)((ushort)Utils.LANG_ENGLISH));

		// Token: 0x040006FD RID: 1789
		private static uint LCID = (uint)(((int)((ushort)Utils.SORT_DEFAULT) << 16) | (int)((ushort)Utils.LANGID));

		// Token: 0x040006FE RID: 1790
		internal static uint NORM_IGNORECASE = 1U;

		// Token: 0x040006FF RID: 1791
		internal static uint NORM_IGNORENONSPACE = 2U;

		// Token: 0x04000700 RID: 1792
		internal static uint NORM_IGNOREKANATYPE = 65536U;

		// Token: 0x04000701 RID: 1793
		internal static uint NORM_IGNOREWIDTH = 131072U;

		// Token: 0x04000702 RID: 1794
		internal static uint SORT_STRINGSORT = 4096U;

		// Token: 0x04000703 RID: 1795
		internal static uint DEFAULT_CMP_FLAGS = Utils.NORM_IGNORECASE | Utils.NORM_IGNOREKANATYPE | Utils.NORM_IGNORENONSPACE | Utils.NORM_IGNOREWIDTH | Utils.SORT_STRINGSORT;

		// Token: 0x04000704 RID: 1796
		private static string NTAuthorityString = null;
	}
}
