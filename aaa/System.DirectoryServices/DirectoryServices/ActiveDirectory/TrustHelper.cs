using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000F6 RID: 246
	internal class TrustHelper
	{
		// Token: 0x06000748 RID: 1864 RVA: 0x00025F65 File Offset: 0x00024F65
		private TrustHelper()
		{
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x00025F70 File Offset: 0x00024F70
		internal static bool GetTrustedDomainInfoStatus(DirectoryContext context, string sourceName, string targetName, TRUST_ATTRIBUTE attribute, bool isForest)
		{
			IntPtr intPtr = (IntPtr)0;
			bool flag = false;
			IntPtr intPtr2 = (IntPtr)0;
			string policyServerName = Utils.GetPolicyServerName(context, isForest, false, sourceName);
			flag = Utils.Impersonate(context);
			bool flag2;
			try
			{
				try
				{
					PolicySafeHandle policySafeHandle = new PolicySafeHandle(Utils.GetPolicyHandle(policyServerName));
					LSA_UNICODE_STRING lsa_UNICODE_STRING = new LSA_UNICODE_STRING();
					intPtr2 = Marshal.StringToHGlobalUni(targetName);
					UnsafeNativeMethods.RtlInitUnicodeString(lsa_UNICODE_STRING, intPtr2);
					int num = UnsafeNativeMethods.LsaQueryTrustedDomainInfoByName(policySafeHandle, lsa_UNICODE_STRING, TRUSTED_INFORMATION_CLASS.TrustedDomainInformationEx, ref intPtr);
					if (num != 0)
					{
						int num2 = UnsafeNativeMethods.LsaNtStatusToWinError(num);
						if (num2 != TrustHelper.STATUS_OBJECT_NAME_NOT_FOUND)
						{
							throw ExceptionHelper.GetExceptionFromErrorCode(num2, policyServerName);
						}
						if (isForest)
						{
							throw new ActiveDirectoryObjectNotFoundException(Res.GetString("ForestTrustDoesNotExist", new object[] { sourceName, targetName }), typeof(ForestTrustRelationshipInformation), null);
						}
						throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DomainTrustDoesNotExist", new object[] { sourceName, targetName }), typeof(TrustRelationshipInformation), null);
					}
					else
					{
						TRUSTED_DOMAIN_INFORMATION_EX trusted_DOMAIN_INFORMATION_EX = new TRUSTED_DOMAIN_INFORMATION_EX();
						Marshal.PtrToStructure(intPtr, trusted_DOMAIN_INFORMATION_EX);
						TrustHelper.ValidateTrustAttribute(trusted_DOMAIN_INFORMATION_EX, isForest, sourceName, targetName);
						if (attribute == TRUST_ATTRIBUTE.TRUST_ATTRIBUTE_CROSS_ORGANIZATION)
						{
							if ((trusted_DOMAIN_INFORMATION_EX.TrustAttributes & TRUST_ATTRIBUTE.TRUST_ATTRIBUTE_CROSS_ORGANIZATION) == (TRUST_ATTRIBUTE)0)
							{
								flag2 = false;
							}
							else
							{
								flag2 = true;
							}
						}
						else if (attribute == TRUST_ATTRIBUTE.TRUST_ATTRIBUTE_TREAT_AS_EXTERNAL)
						{
							if ((trusted_DOMAIN_INFORMATION_EX.TrustAttributes & TRUST_ATTRIBUTE.TRUST_ATTRIBUTE_TREAT_AS_EXTERNAL) == (TRUST_ATTRIBUTE)0)
							{
								flag2 = true;
							}
							else
							{
								flag2 = false;
							}
						}
						else
						{
							if (attribute != TRUST_ATTRIBUTE.TRUST_ATTRIBUTE_QUARANTINED_DOMAIN)
							{
								throw new ArgumentException("attribute");
							}
							if ((trusted_DOMAIN_INFORMATION_EX.TrustAttributes & TRUST_ATTRIBUTE.TRUST_ATTRIBUTE_QUARANTINED_DOMAIN) == (TRUST_ATTRIBUTE)0)
							{
								flag2 = false;
							}
							else
							{
								flag2 = true;
							}
						}
					}
				}
				finally
				{
					if (flag)
					{
						Utils.Revert();
					}
					if (intPtr2 != (IntPtr)0)
					{
						Marshal.FreeHGlobal(intPtr2);
					}
					if (intPtr != (IntPtr)0)
					{
						UnsafeNativeMethods.LsaFreeMemory(intPtr);
					}
				}
			}
			catch
			{
				throw;
			}
			return flag2;
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0002613C File Offset: 0x0002513C
		internal static void SetTrustedDomainInfoStatus(DirectoryContext context, string sourceName, string targetName, TRUST_ATTRIBUTE attribute, bool status, bool isForest)
		{
			IntPtr intPtr = (IntPtr)0;
			IntPtr intPtr2 = (IntPtr)0;
			bool flag = false;
			IntPtr intPtr3 = (IntPtr)0;
			string policyServerName = Utils.GetPolicyServerName(context, isForest, false, sourceName);
			flag = Utils.Impersonate(context);
			try
			{
				try
				{
					PolicySafeHandle policySafeHandle = new PolicySafeHandle(Utils.GetPolicyHandle(policyServerName));
					LSA_UNICODE_STRING lsa_UNICODE_STRING = new LSA_UNICODE_STRING();
					intPtr3 = Marshal.StringToHGlobalUni(targetName);
					UnsafeNativeMethods.RtlInitUnicodeString(lsa_UNICODE_STRING, intPtr3);
					int num = UnsafeNativeMethods.LsaQueryTrustedDomainInfoByName(policySafeHandle, lsa_UNICODE_STRING, TRUSTED_INFORMATION_CLASS.TrustedDomainInformationEx, ref intPtr);
					if (num != 0)
					{
						int num2 = UnsafeNativeMethods.LsaNtStatusToWinError(num);
						if (num2 != TrustHelper.STATUS_OBJECT_NAME_NOT_FOUND)
						{
							throw ExceptionHelper.GetExceptionFromErrorCode(num2, policyServerName);
						}
						if (isForest)
						{
							throw new ActiveDirectoryObjectNotFoundException(Res.GetString("ForestTrustDoesNotExist", new object[] { sourceName, targetName }), typeof(ForestTrustRelationshipInformation), null);
						}
						throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DomainTrustDoesNotExist", new object[] { sourceName, targetName }), typeof(TrustRelationshipInformation), null);
					}
					else
					{
						TRUSTED_DOMAIN_INFORMATION_EX trusted_DOMAIN_INFORMATION_EX = new TRUSTED_DOMAIN_INFORMATION_EX();
						Marshal.PtrToStructure(intPtr, trusted_DOMAIN_INFORMATION_EX);
						TrustHelper.ValidateTrustAttribute(trusted_DOMAIN_INFORMATION_EX, isForest, sourceName, targetName);
						if (attribute == TRUST_ATTRIBUTE.TRUST_ATTRIBUTE_CROSS_ORGANIZATION)
						{
							if (status)
							{
								trusted_DOMAIN_INFORMATION_EX.TrustAttributes |= TRUST_ATTRIBUTE.TRUST_ATTRIBUTE_CROSS_ORGANIZATION;
							}
							else
							{
								trusted_DOMAIN_INFORMATION_EX.TrustAttributes &= ~TRUST_ATTRIBUTE.TRUST_ATTRIBUTE_CROSS_ORGANIZATION;
							}
						}
						else if (attribute == TRUST_ATTRIBUTE.TRUST_ATTRIBUTE_TREAT_AS_EXTERNAL)
						{
							if (status)
							{
								trusted_DOMAIN_INFORMATION_EX.TrustAttributes &= ~TRUST_ATTRIBUTE.TRUST_ATTRIBUTE_TREAT_AS_EXTERNAL;
							}
							else
							{
								trusted_DOMAIN_INFORMATION_EX.TrustAttributes |= TRUST_ATTRIBUTE.TRUST_ATTRIBUTE_TREAT_AS_EXTERNAL;
							}
						}
						else
						{
							if (attribute != TRUST_ATTRIBUTE.TRUST_ATTRIBUTE_QUARANTINED_DOMAIN)
							{
								throw new ArgumentException("attribute");
							}
							if (status)
							{
								trusted_DOMAIN_INFORMATION_EX.TrustAttributes |= TRUST_ATTRIBUTE.TRUST_ATTRIBUTE_QUARANTINED_DOMAIN;
							}
							else
							{
								trusted_DOMAIN_INFORMATION_EX.TrustAttributes &= ~TRUST_ATTRIBUTE.TRUST_ATTRIBUTE_QUARANTINED_DOMAIN;
							}
						}
						intPtr2 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(TRUSTED_DOMAIN_INFORMATION_EX)));
						Marshal.StructureToPtr(trusted_DOMAIN_INFORMATION_EX, intPtr2, false);
						num = UnsafeNativeMethods.LsaSetTrustedDomainInfoByName(policySafeHandle, lsa_UNICODE_STRING, TRUSTED_INFORMATION_CLASS.TrustedDomainInformationEx, intPtr2);
						if (num != 0)
						{
							throw ExceptionHelper.GetExceptionFromErrorCode(UnsafeNativeMethods.LsaNtStatusToWinError(num), policyServerName);
						}
					}
				}
				finally
				{
					if (flag)
					{
						Utils.Revert();
					}
					if (intPtr3 != (IntPtr)0)
					{
						Marshal.FreeHGlobal(intPtr3);
					}
					if (intPtr != (IntPtr)0)
					{
						UnsafeNativeMethods.LsaFreeMemory(intPtr);
					}
					if (intPtr2 != (IntPtr)0)
					{
						Marshal.FreeHGlobal(intPtr2);
					}
				}
			}
			catch
			{
				throw;
			}
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x00026398 File Offset: 0x00025398
		internal static void DeleteTrust(DirectoryContext sourceContext, string sourceName, string targetName, bool isForest)
		{
			bool flag = false;
			IntPtr intPtr = (IntPtr)0;
			IntPtr intPtr2 = (IntPtr)0;
			string policyServerName = Utils.GetPolicyServerName(sourceContext, isForest, false, sourceName);
			flag = Utils.Impersonate(sourceContext);
			try
			{
				try
				{
					PolicySafeHandle policySafeHandle = new PolicySafeHandle(Utils.GetPolicyHandle(policyServerName));
					LSA_UNICODE_STRING lsa_UNICODE_STRING = new LSA_UNICODE_STRING();
					intPtr = Marshal.StringToHGlobalUni(targetName);
					UnsafeNativeMethods.RtlInitUnicodeString(lsa_UNICODE_STRING, intPtr);
					int num = UnsafeNativeMethods.LsaQueryTrustedDomainInfoByName(policySafeHandle, lsa_UNICODE_STRING, TRUSTED_INFORMATION_CLASS.TrustedDomainInformationEx, ref intPtr2);
					if (num != 0)
					{
						int num2 = UnsafeNativeMethods.LsaNtStatusToWinError(num);
						if (num2 != TrustHelper.STATUS_OBJECT_NAME_NOT_FOUND)
						{
							throw ExceptionHelper.GetExceptionFromErrorCode(num2, policyServerName);
						}
						if (isForest)
						{
							throw new ActiveDirectoryObjectNotFoundException(Res.GetString("ForestTrustDoesNotExist", new object[] { sourceName, targetName }), typeof(ForestTrustRelationshipInformation), null);
						}
						throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DomainTrustDoesNotExist", new object[] { sourceName, targetName }), typeof(TrustRelationshipInformation), null);
					}
					else
					{
						try
						{
							TRUSTED_DOMAIN_INFORMATION_EX trusted_DOMAIN_INFORMATION_EX = new TRUSTED_DOMAIN_INFORMATION_EX();
							Marshal.PtrToStructure(intPtr2, trusted_DOMAIN_INFORMATION_EX);
							TrustHelper.ValidateTrustAttribute(trusted_DOMAIN_INFORMATION_EX, isForest, sourceName, targetName);
							num = UnsafeNativeMethods.LsaDeleteTrustedDomain(policySafeHandle, trusted_DOMAIN_INFORMATION_EX.Sid);
							if (num != 0)
							{
								int num2 = UnsafeNativeMethods.LsaNtStatusToWinError(num);
								throw ExceptionHelper.GetExceptionFromErrorCode(num2, policyServerName);
							}
						}
						finally
						{
							if (intPtr2 != (IntPtr)0)
							{
								UnsafeNativeMethods.LsaFreeMemory(intPtr2);
							}
						}
					}
				}
				finally
				{
					if (flag)
					{
						Utils.Revert();
					}
					if (intPtr != (IntPtr)0)
					{
						Marshal.FreeHGlobal(intPtr);
					}
				}
			}
			catch
			{
				throw;
			}
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x0002654C File Offset: 0x0002554C
		internal static void VerifyTrust(DirectoryContext context, string sourceName, string targetName, bool isForest, TrustDirection direction, bool forceSecureChannelReset, string preferredTargetServer)
		{
			IntPtr intPtr = (IntPtr)0;
			IntPtr intPtr2 = (IntPtr)0;
			IntPtr intPtr3 = (IntPtr)0;
			IntPtr intPtr4 = (IntPtr)0;
			bool flag = true;
			IntPtr intPtr5 = (IntPtr)0;
			string policyServerName = Utils.GetPolicyServerName(context, isForest, false, sourceName);
			flag = Utils.Impersonate(context);
			try
			{
				try
				{
					PolicySafeHandle policySafeHandle = new PolicySafeHandle(Utils.GetPolicyHandle(policyServerName));
					LSA_UNICODE_STRING lsa_UNICODE_STRING = new LSA_UNICODE_STRING();
					intPtr5 = Marshal.StringToHGlobalUni(targetName);
					UnsafeNativeMethods.RtlInitUnicodeString(lsa_UNICODE_STRING, intPtr5);
					TrustHelper.ValidateTrust(policySafeHandle, lsa_UNICODE_STRING, sourceName, targetName, isForest, (int)direction, policyServerName);
					if (preferredTargetServer == null)
					{
						intPtr = Marshal.StringToHGlobalUni(targetName);
					}
					else
					{
						intPtr = Marshal.StringToHGlobalUni(targetName + "\\" + preferredTargetServer);
					}
					intPtr2 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
					Marshal.WriteIntPtr(intPtr2, intPtr);
					if (!forceSecureChannelReset)
					{
						int num = UnsafeNativeMethods.I_NetLogonControl2(policyServerName, TrustHelper.NETLOGON_CONTROL_TC_VERIFY, TrustHelper.NETLOGON_QUERY_LEVEL, intPtr2, out intPtr3);
						if (num == 0)
						{
							NETLOGON_INFO_2 netlogon_INFO_ = new NETLOGON_INFO_2();
							Marshal.PtrToStructure(intPtr3, netlogon_INFO_);
							if ((netlogon_INFO_.netlog2_flags & TrustHelper.NETLOGON_VERIFY_STATUS_RETURNED) == 0)
							{
								int netlog2_tc_connection_status = netlogon_INFO_.netlog2_tc_connection_status;
								throw ExceptionHelper.GetExceptionFromErrorCode(netlog2_tc_connection_status);
							}
							int netlog2_pdc_connection_status = netlogon_INFO_.netlog2_pdc_connection_status;
							if (netlog2_pdc_connection_status != 0)
							{
								throw ExceptionHelper.GetExceptionFromErrorCode(netlog2_pdc_connection_status);
							}
						}
						else
						{
							if (num == TrustHelper.ERROR_INVALID_LEVEL)
							{
								throw new NotSupportedException(Res.GetString("TrustVerificationNotSupport"));
							}
							throw ExceptionHelper.GetExceptionFromErrorCode(num);
						}
					}
					else
					{
						int num = UnsafeNativeMethods.I_NetLogonControl2(policyServerName, TrustHelper.NETLOGON_CONTROL_REDISCOVER, TrustHelper.NETLOGON_QUERY_LEVEL, intPtr2, out intPtr4);
						if (num != 0)
						{
							throw ExceptionHelper.GetExceptionFromErrorCode(num);
						}
					}
				}
				finally
				{
					if (flag)
					{
						Utils.Revert();
					}
					if (intPtr5 != (IntPtr)0)
					{
						Marshal.FreeHGlobal(intPtr5);
					}
					if (intPtr2 != (IntPtr)0)
					{
						Marshal.FreeHGlobal(intPtr2);
					}
					if (intPtr != (IntPtr)0)
					{
						Marshal.FreeHGlobal(intPtr);
					}
					if (intPtr3 != (IntPtr)0)
					{
						UnsafeNativeMethods.NetApiBufferFree(intPtr3);
					}
					if (intPtr4 != (IntPtr)0)
					{
						UnsafeNativeMethods.NetApiBufferFree(intPtr4);
					}
				}
			}
			catch
			{
				throw;
			}
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x0002676C File Offset: 0x0002576C
		internal static void CreateTrust(DirectoryContext sourceContext, string sourceName, DirectoryContext targetContext, string targetName, bool isForest, TrustDirection direction, string password)
		{
			IntPtr intPtr = (IntPtr)0;
			IntPtr intPtr2 = (IntPtr)0;
			IntPtr intPtr3 = (IntPtr)0;
			IntPtr intPtr4 = (IntPtr)0;
			IntPtr intPtr5 = (IntPtr)0;
			bool flag = false;
			intPtr3 = TrustHelper.GetTrustedDomainInfo(targetContext, targetName, isForest);
			try
			{
				try
				{
					POLICY_DNS_DOMAIN_INFO policy_DNS_DOMAIN_INFO = new POLICY_DNS_DOMAIN_INFO();
					Marshal.PtrToStructure(intPtr3, policy_DNS_DOMAIN_INFO);
					LSA_AUTH_INFORMATION lsa_AUTH_INFORMATION = new LSA_AUTH_INFORMATION();
					intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(FileTime)));
					UnsafeNativeMethods.GetSystemTimeAsFileTime(intPtr);
					FileTime fileTime = new FileTime();
					Marshal.PtrToStructure(intPtr, fileTime);
					lsa_AUTH_INFORMATION.LastUpdateTime = new LARGE_INTEGER();
					lsa_AUTH_INFORMATION.LastUpdateTime.lowPart = fileTime.lower;
					lsa_AUTH_INFORMATION.LastUpdateTime.highPart = fileTime.higher;
					lsa_AUTH_INFORMATION.AuthType = TrustHelper.TRUST_AUTH_TYPE_CLEAR;
					intPtr2 = Marshal.StringToHGlobalUni(password);
					lsa_AUTH_INFORMATION.AuthInfo = intPtr2;
					lsa_AUTH_INFORMATION.AuthInfoLength = password.Length * 2;
					intPtr5 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(LSA_AUTH_INFORMATION)));
					Marshal.StructureToPtr(lsa_AUTH_INFORMATION, intPtr5, false);
					TRUSTED_DOMAIN_AUTH_INFORMATION trusted_DOMAIN_AUTH_INFORMATION = new TRUSTED_DOMAIN_AUTH_INFORMATION();
					if ((direction & TrustDirection.Inbound) != (TrustDirection)0)
					{
						trusted_DOMAIN_AUTH_INFORMATION.IncomingAuthInfos = 1;
						trusted_DOMAIN_AUTH_INFORMATION.IncomingAuthenticationInformation = intPtr5;
						trusted_DOMAIN_AUTH_INFORMATION.IncomingPreviousAuthenticationInformation = (IntPtr)0;
					}
					if ((direction & TrustDirection.Outbound) != (TrustDirection)0)
					{
						trusted_DOMAIN_AUTH_INFORMATION.OutgoingAuthInfos = 1;
						trusted_DOMAIN_AUTH_INFORMATION.OutgoingAuthenticationInformation = intPtr5;
						trusted_DOMAIN_AUTH_INFORMATION.OutgoingPreviousAuthenticationInformation = (IntPtr)0;
					}
					TRUSTED_DOMAIN_INFORMATION_EX trusted_DOMAIN_INFORMATION_EX = new TRUSTED_DOMAIN_INFORMATION_EX();
					trusted_DOMAIN_INFORMATION_EX.FlatName = policy_DNS_DOMAIN_INFO.Name;
					trusted_DOMAIN_INFORMATION_EX.Name = policy_DNS_DOMAIN_INFO.DnsDomainName;
					trusted_DOMAIN_INFORMATION_EX.Sid = policy_DNS_DOMAIN_INFO.Sid;
					trusted_DOMAIN_INFORMATION_EX.TrustType = TrustHelper.TRUST_TYPE_UPLEVEL;
					trusted_DOMAIN_INFORMATION_EX.TrustDirection = (int)direction;
					if (isForest)
					{
						trusted_DOMAIN_INFORMATION_EX.TrustAttributes = TRUST_ATTRIBUTE.TRUST_ATTRIBUTE_FOREST_TRANSITIVE;
					}
					else
					{
						trusted_DOMAIN_INFORMATION_EX.TrustAttributes = TRUST_ATTRIBUTE.TRUST_ATTRIBUTE_QUARANTINED_DOMAIN;
					}
					string policyServerName = Utils.GetPolicyServerName(sourceContext, isForest, false, sourceName);
					flag = Utils.Impersonate(sourceContext);
					PolicySafeHandle policySafeHandle = new PolicySafeHandle(Utils.GetPolicyHandle(policyServerName));
					int num = UnsafeNativeMethods.LsaCreateTrustedDomainEx(policySafeHandle, trusted_DOMAIN_INFORMATION_EX, trusted_DOMAIN_AUTH_INFORMATION, TrustHelper.TRUSTED_SET_POSIX | TrustHelper.TRUSTED_SET_AUTH, out intPtr4);
					if (num != 0)
					{
						num = UnsafeNativeMethods.LsaNtStatusToWinError(num);
						if (num != TrustHelper.ERROR_ALREADY_EXISTS)
						{
							throw ExceptionHelper.GetExceptionFromErrorCode(num, policyServerName);
						}
						if (isForest)
						{
							throw new ActiveDirectoryObjectExistsException(Res.GetString("AlreadyExistingForestTrust", new object[] { sourceName, targetName }));
						}
						throw new ActiveDirectoryObjectExistsException(Res.GetString("AlreadyExistingDomainTrust", new object[] { sourceName, targetName }));
					}
				}
				finally
				{
					if (flag)
					{
						Utils.Revert();
					}
					if (intPtr != (IntPtr)0)
					{
						Marshal.FreeHGlobal(intPtr);
					}
					if (intPtr4 != (IntPtr)0)
					{
						UnsafeNativeMethods.LsaClose(intPtr4);
					}
					if (intPtr3 != (IntPtr)0)
					{
						UnsafeNativeMethods.LsaFreeMemory(intPtr3);
					}
					if (intPtr2 != (IntPtr)0)
					{
						Marshal.FreeHGlobal(intPtr2);
					}
					if (intPtr5 != (IntPtr)0)
					{
						Marshal.FreeHGlobal(intPtr5);
					}
				}
			}
			catch
			{
				throw;
			}
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x00026A68 File Offset: 0x00025A68
		internal static string UpdateTrust(DirectoryContext context, string sourceName, string targetName, string password, bool isForest)
		{
			IntPtr intPtr = (IntPtr)0;
			IntPtr intPtr2 = (IntPtr)0;
			bool flag = false;
			IntPtr intPtr3 = (IntPtr)0;
			IntPtr intPtr4 = (IntPtr)0;
			IntPtr intPtr5 = (IntPtr)0;
			IntPtr intPtr6 = (IntPtr)0;
			string policyServerName = Utils.GetPolicyServerName(context, isForest, false, sourceName);
			flag = Utils.Impersonate(context);
			string text;
			try
			{
				try
				{
					PolicySafeHandle policySafeHandle = new PolicySafeHandle(Utils.GetPolicyHandle(policyServerName));
					LSA_UNICODE_STRING lsa_UNICODE_STRING = new LSA_UNICODE_STRING();
					intPtr6 = Marshal.StringToHGlobalUni(targetName);
					UnsafeNativeMethods.RtlInitUnicodeString(lsa_UNICODE_STRING, intPtr6);
					int num = UnsafeNativeMethods.LsaQueryTrustedDomainInfoByName(policySafeHandle, lsa_UNICODE_STRING, TRUSTED_INFORMATION_CLASS.TrustedDomainFullInformation, ref intPtr);
					if (num != 0)
					{
						int num2 = UnsafeNativeMethods.LsaNtStatusToWinError(num);
						if (num2 != TrustHelper.STATUS_OBJECT_NAME_NOT_FOUND)
						{
							throw ExceptionHelper.GetExceptionFromErrorCode(num2, policyServerName);
						}
						if (isForest)
						{
							throw new ActiveDirectoryObjectNotFoundException(Res.GetString("ForestTrustDoesNotExist", new object[] { sourceName, targetName }), typeof(ForestTrustRelationshipInformation), null);
						}
						throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DomainTrustDoesNotExist", new object[] { sourceName, targetName }), typeof(TrustRelationshipInformation), null);
					}
					else
					{
						TRUSTED_DOMAIN_FULL_INFORMATION trusted_DOMAIN_FULL_INFORMATION = new TRUSTED_DOMAIN_FULL_INFORMATION();
						Marshal.PtrToStructure(intPtr, trusted_DOMAIN_FULL_INFORMATION);
						TrustHelper.ValidateTrustAttribute(trusted_DOMAIN_FULL_INFORMATION.Information, isForest, sourceName, targetName);
						TrustDirection trustDirection = (TrustDirection)trusted_DOMAIN_FULL_INFORMATION.Information.TrustDirection;
						LSA_AUTH_INFORMATION lsa_AUTH_INFORMATION = new LSA_AUTH_INFORMATION();
						intPtr3 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(FileTime)));
						UnsafeNativeMethods.GetSystemTimeAsFileTime(intPtr3);
						FileTime fileTime = new FileTime();
						Marshal.PtrToStructure(intPtr3, fileTime);
						lsa_AUTH_INFORMATION.LastUpdateTime = new LARGE_INTEGER();
						lsa_AUTH_INFORMATION.LastUpdateTime.lowPart = fileTime.lower;
						lsa_AUTH_INFORMATION.LastUpdateTime.highPart = fileTime.higher;
						lsa_AUTH_INFORMATION.AuthType = TrustHelper.TRUST_AUTH_TYPE_CLEAR;
						intPtr4 = Marshal.StringToHGlobalUni(password);
						lsa_AUTH_INFORMATION.AuthInfo = intPtr4;
						lsa_AUTH_INFORMATION.AuthInfoLength = password.Length * 2;
						intPtr5 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(LSA_AUTH_INFORMATION)));
						Marshal.StructureToPtr(lsa_AUTH_INFORMATION, intPtr5, false);
						TRUSTED_DOMAIN_AUTH_INFORMATION trusted_DOMAIN_AUTH_INFORMATION = new TRUSTED_DOMAIN_AUTH_INFORMATION();
						if ((trustDirection & TrustDirection.Inbound) != (TrustDirection)0)
						{
							trusted_DOMAIN_AUTH_INFORMATION.IncomingAuthInfos = 1;
							trusted_DOMAIN_AUTH_INFORMATION.IncomingAuthenticationInformation = intPtr5;
							trusted_DOMAIN_AUTH_INFORMATION.IncomingPreviousAuthenticationInformation = (IntPtr)0;
						}
						if ((trustDirection & TrustDirection.Outbound) != (TrustDirection)0)
						{
							trusted_DOMAIN_AUTH_INFORMATION.OutgoingAuthInfos = 1;
							trusted_DOMAIN_AUTH_INFORMATION.OutgoingAuthenticationInformation = intPtr5;
							trusted_DOMAIN_AUTH_INFORMATION.OutgoingPreviousAuthenticationInformation = (IntPtr)0;
						}
						trusted_DOMAIN_FULL_INFORMATION.AuthInformation = trusted_DOMAIN_AUTH_INFORMATION;
						intPtr2 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(TRUSTED_DOMAIN_FULL_INFORMATION)));
						Marshal.StructureToPtr(trusted_DOMAIN_FULL_INFORMATION, intPtr2, false);
						num = UnsafeNativeMethods.LsaSetTrustedDomainInfoByName(policySafeHandle, lsa_UNICODE_STRING, TRUSTED_INFORMATION_CLASS.TrustedDomainFullInformation, intPtr2);
						if (num != 0)
						{
							throw ExceptionHelper.GetExceptionFromErrorCode(UnsafeNativeMethods.LsaNtStatusToWinError(num), policyServerName);
						}
						text = policyServerName;
					}
				}
				finally
				{
					if (flag)
					{
						Utils.Revert();
					}
					if (intPtr6 != (IntPtr)0)
					{
						Marshal.FreeHGlobal(intPtr6);
					}
					if (intPtr != (IntPtr)0)
					{
						UnsafeNativeMethods.LsaFreeMemory(intPtr);
					}
					if (intPtr2 != (IntPtr)0)
					{
						Marshal.FreeHGlobal(intPtr2);
					}
					if (intPtr3 != (IntPtr)0)
					{
						Marshal.FreeHGlobal(intPtr3);
					}
					if (intPtr4 != (IntPtr)0)
					{
						Marshal.FreeHGlobal(intPtr4);
					}
					if (intPtr5 != (IntPtr)0)
					{
						Marshal.FreeHGlobal(intPtr5);
					}
				}
			}
			catch
			{
				throw;
			}
			return text;
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x00026DBC File Offset: 0x00025DBC
		internal static void UpdateTrustDirection(DirectoryContext context, string sourceName, string targetName, string password, bool isForest, TrustDirection newTrustDirection)
		{
			IntPtr intPtr = (IntPtr)0;
			IntPtr intPtr2 = (IntPtr)0;
			bool flag = false;
			IntPtr intPtr3 = (IntPtr)0;
			IntPtr intPtr4 = (IntPtr)0;
			IntPtr intPtr5 = (IntPtr)0;
			IntPtr intPtr6 = (IntPtr)0;
			string policyServerName = Utils.GetPolicyServerName(context, isForest, false, sourceName);
			flag = Utils.Impersonate(context);
			try
			{
				try
				{
					PolicySafeHandle policySafeHandle = new PolicySafeHandle(Utils.GetPolicyHandle(policyServerName));
					LSA_UNICODE_STRING lsa_UNICODE_STRING = new LSA_UNICODE_STRING();
					intPtr6 = Marshal.StringToHGlobalUni(targetName);
					UnsafeNativeMethods.RtlInitUnicodeString(lsa_UNICODE_STRING, intPtr6);
					int num = UnsafeNativeMethods.LsaQueryTrustedDomainInfoByName(policySafeHandle, lsa_UNICODE_STRING, TRUSTED_INFORMATION_CLASS.TrustedDomainFullInformation, ref intPtr);
					if (num != 0)
					{
						int num2 = UnsafeNativeMethods.LsaNtStatusToWinError(num);
						if (num2 != TrustHelper.STATUS_OBJECT_NAME_NOT_FOUND)
						{
							throw ExceptionHelper.GetExceptionFromErrorCode(num2, policyServerName);
						}
						if (isForest)
						{
							throw new ActiveDirectoryObjectNotFoundException(Res.GetString("ForestTrustDoesNotExist", new object[] { sourceName, targetName }), typeof(ForestTrustRelationshipInformation), null);
						}
						throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DomainTrustDoesNotExist", new object[] { sourceName, targetName }), typeof(TrustRelationshipInformation), null);
					}
					else
					{
						TRUSTED_DOMAIN_FULL_INFORMATION trusted_DOMAIN_FULL_INFORMATION = new TRUSTED_DOMAIN_FULL_INFORMATION();
						Marshal.PtrToStructure(intPtr, trusted_DOMAIN_FULL_INFORMATION);
						TrustHelper.ValidateTrustAttribute(trusted_DOMAIN_FULL_INFORMATION.Information, isForest, sourceName, targetName);
						LSA_AUTH_INFORMATION lsa_AUTH_INFORMATION = new LSA_AUTH_INFORMATION();
						intPtr3 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(FileTime)));
						UnsafeNativeMethods.GetSystemTimeAsFileTime(intPtr3);
						FileTime fileTime = new FileTime();
						Marshal.PtrToStructure(intPtr3, fileTime);
						lsa_AUTH_INFORMATION.LastUpdateTime = new LARGE_INTEGER();
						lsa_AUTH_INFORMATION.LastUpdateTime.lowPart = fileTime.lower;
						lsa_AUTH_INFORMATION.LastUpdateTime.highPart = fileTime.higher;
						lsa_AUTH_INFORMATION.AuthType = TrustHelper.TRUST_AUTH_TYPE_CLEAR;
						intPtr4 = Marshal.StringToHGlobalUni(password);
						lsa_AUTH_INFORMATION.AuthInfo = intPtr4;
						lsa_AUTH_INFORMATION.AuthInfoLength = password.Length * 2;
						intPtr5 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(LSA_AUTH_INFORMATION)));
						Marshal.StructureToPtr(lsa_AUTH_INFORMATION, intPtr5, false);
						TRUSTED_DOMAIN_AUTH_INFORMATION trusted_DOMAIN_AUTH_INFORMATION = new TRUSTED_DOMAIN_AUTH_INFORMATION();
						if ((newTrustDirection & TrustDirection.Inbound) != (TrustDirection)0)
						{
							trusted_DOMAIN_AUTH_INFORMATION.IncomingAuthInfos = 1;
							trusted_DOMAIN_AUTH_INFORMATION.IncomingAuthenticationInformation = intPtr5;
							trusted_DOMAIN_AUTH_INFORMATION.IncomingPreviousAuthenticationInformation = (IntPtr)0;
						}
						else
						{
							trusted_DOMAIN_AUTH_INFORMATION.IncomingAuthInfos = 0;
							trusted_DOMAIN_AUTH_INFORMATION.IncomingAuthenticationInformation = (IntPtr)0;
							trusted_DOMAIN_AUTH_INFORMATION.IncomingPreviousAuthenticationInformation = (IntPtr)0;
						}
						if ((newTrustDirection & TrustDirection.Outbound) != (TrustDirection)0)
						{
							trusted_DOMAIN_AUTH_INFORMATION.OutgoingAuthInfos = 1;
							trusted_DOMAIN_AUTH_INFORMATION.OutgoingAuthenticationInformation = intPtr5;
							trusted_DOMAIN_AUTH_INFORMATION.OutgoingPreviousAuthenticationInformation = (IntPtr)0;
						}
						else
						{
							trusted_DOMAIN_AUTH_INFORMATION.OutgoingAuthInfos = 0;
							trusted_DOMAIN_AUTH_INFORMATION.OutgoingAuthenticationInformation = (IntPtr)0;
							trusted_DOMAIN_AUTH_INFORMATION.OutgoingPreviousAuthenticationInformation = (IntPtr)0;
						}
						trusted_DOMAIN_FULL_INFORMATION.AuthInformation = trusted_DOMAIN_AUTH_INFORMATION;
						trusted_DOMAIN_FULL_INFORMATION.Information.TrustDirection = (int)newTrustDirection;
						intPtr2 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(TRUSTED_DOMAIN_FULL_INFORMATION)));
						Marshal.StructureToPtr(trusted_DOMAIN_FULL_INFORMATION, intPtr2, false);
						num = UnsafeNativeMethods.LsaSetTrustedDomainInfoByName(policySafeHandle, lsa_UNICODE_STRING, TRUSTED_INFORMATION_CLASS.TrustedDomainFullInformation, intPtr2);
						if (num != 0)
						{
							throw ExceptionHelper.GetExceptionFromErrorCode(UnsafeNativeMethods.LsaNtStatusToWinError(num), policyServerName);
						}
					}
				}
				finally
				{
					if (flag)
					{
						Utils.Revert();
					}
					if (intPtr6 != (IntPtr)0)
					{
						Marshal.FreeHGlobal(intPtr6);
					}
					if (intPtr != (IntPtr)0)
					{
						UnsafeNativeMethods.LsaFreeMemory(intPtr);
					}
					if (intPtr2 != (IntPtr)0)
					{
						Marshal.FreeHGlobal(intPtr2);
					}
					if (intPtr3 != (IntPtr)0)
					{
						Marshal.FreeHGlobal(intPtr3);
					}
					if (intPtr4 != (IntPtr)0)
					{
						Marshal.FreeHGlobal(intPtr4);
					}
					if (intPtr5 != (IntPtr)0)
					{
						Marshal.FreeHGlobal(intPtr5);
					}
				}
			}
			catch
			{
				throw;
			}
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x00027150 File Offset: 0x00026150
		private static void ValidateTrust(PolicySafeHandle handle, LSA_UNICODE_STRING trustedDomainName, string sourceName, string targetName, bool isForest, int direction, string serverName)
		{
			IntPtr intPtr = (IntPtr)0;
			int num = UnsafeNativeMethods.LsaQueryTrustedDomainInfoByName(handle, trustedDomainName, TRUSTED_INFORMATION_CLASS.TrustedDomainInformationEx, ref intPtr);
			if (num == 0)
			{
				try
				{
					TRUSTED_DOMAIN_INFORMATION_EX trusted_DOMAIN_INFORMATION_EX = new TRUSTED_DOMAIN_INFORMATION_EX();
					Marshal.PtrToStructure(intPtr, trusted_DOMAIN_INFORMATION_EX);
					TrustHelper.ValidateTrustAttribute(trusted_DOMAIN_INFORMATION_EX, isForest, sourceName, targetName);
					if (direction != 0 && (direction & trusted_DOMAIN_INFORMATION_EX.TrustDirection) == 0)
					{
						if (isForest)
						{
							throw new ActiveDirectoryObjectNotFoundException(Res.GetString("WrongTrustDirection", new object[]
							{
								sourceName,
								targetName,
								(TrustDirection)direction
							}), typeof(ForestTrustRelationshipInformation), null);
						}
						throw new ActiveDirectoryObjectNotFoundException(Res.GetString("WrongTrustDirection", new object[]
						{
							sourceName,
							targetName,
							(TrustDirection)direction
						}), typeof(TrustRelationshipInformation), null);
					}
				}
				finally
				{
					if (intPtr != (IntPtr)0)
					{
						UnsafeNativeMethods.LsaFreeMemory(intPtr);
					}
				}
				return;
			}
			int num2 = UnsafeNativeMethods.LsaNtStatusToWinError(num);
			if (num2 != TrustHelper.STATUS_OBJECT_NAME_NOT_FOUND)
			{
				throw ExceptionHelper.GetExceptionFromErrorCode(num2, serverName);
			}
			if (isForest)
			{
				throw new ActiveDirectoryObjectNotFoundException(Res.GetString("ForestTrustDoesNotExist", new object[] { sourceName, targetName }), typeof(ForestTrustRelationshipInformation), null);
			}
			throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DomainTrustDoesNotExist", new object[] { sourceName, targetName }), typeof(TrustRelationshipInformation), null);
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x000272B8 File Offset: 0x000262B8
		private static void ValidateTrustAttribute(TRUSTED_DOMAIN_INFORMATION_EX domainInfo, bool isForest, string sourceName, string targetName)
		{
			if (isForest)
			{
				if ((domainInfo.TrustAttributes & TRUST_ATTRIBUTE.TRUST_ATTRIBUTE_FOREST_TRANSITIVE) == (TRUST_ATTRIBUTE)0)
				{
					throw new ActiveDirectoryObjectNotFoundException(Res.GetString("ForestTrustDoesNotExist", new object[] { sourceName, targetName }), typeof(ForestTrustRelationshipInformation), null);
				}
			}
			else
			{
				if ((domainInfo.TrustAttributes & TRUST_ATTRIBUTE.TRUST_ATTRIBUTE_FOREST_TRANSITIVE) != (TRUST_ATTRIBUTE)0)
				{
					throw new ActiveDirectoryObjectNotFoundException(Res.GetString("WrongForestTrust", new object[] { sourceName, targetName }), typeof(TrustRelationshipInformation), null);
				}
				if (domainInfo.TrustType == TrustHelper.TRUST_TYPE_DOWNLEVEL)
				{
					throw new InvalidOperationException(Res.GetString("NT4NotSupported"));
				}
				if (domainInfo.TrustType == TrustHelper.TRUST_TYPE_MIT)
				{
					throw new InvalidOperationException(Res.GetString("KerberosNotSupported"));
				}
			}
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x00027370 File Offset: 0x00026370
		internal static string CreateTrustPassword()
		{
			byte[] array = new byte[TrustHelper.PASSWORD_LENGTH];
			char[] array2 = new char[TrustHelper.PASSWORD_LENGTH];
			new RNGCryptoServiceProvider().GetBytes(array);
			for (int i = 0; i < TrustHelper.PASSWORD_LENGTH; i++)
			{
				int num = (int)(array[i] % 87);
				if (num < 10)
				{
					array2[i] = (char)(48 + num);
				}
				else if (num < 36)
				{
					array2[i] = (char)(65 + num - 10);
				}
				else if (num < 62)
				{
					array2[i] = (char)(97 + num - 36);
				}
				else
				{
					array2[i] = TrustHelper.punctuations[num - 62];
				}
			}
			return new string(array2);
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x00027408 File Offset: 0x00026408
		private static IntPtr GetTrustedDomainInfo(DirectoryContext targetContext, string targetName, bool isForest)
		{
			/*
An exception occurred when decompiling this method (06000753)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.IntPtr System.DirectoryServices.ActiveDirectory.TrustHelper::GetTrustedDomainInfo(System.DirectoryServices.ActiveDirectory.DirectoryContext,System.String,System.Boolean)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.AsyncDecompiler.RunStep1(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, String& compilerName, List`1 listExpr, List`1 listBlock, Dictionary`2 labelRefCount) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\AsyncDecompiler.cs:line 95
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 249
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x040005E8 RID: 1512
		private static int STATUS_OBJECT_NAME_NOT_FOUND = 2;

		// Token: 0x040005E9 RID: 1513
		internal static int ERROR_NOT_FOUND = 1168;

		// Token: 0x040005EA RID: 1514
		internal static int NETLOGON_QUERY_LEVEL = 2;

		// Token: 0x040005EB RID: 1515
		internal static int NETLOGON_CONTROL_REDISCOVER = 5;

		// Token: 0x040005EC RID: 1516
		private static int NETLOGON_CONTROL_TC_VERIFY = 10;

		// Token: 0x040005ED RID: 1517
		private static int NETLOGON_VERIFY_STATUS_RETURNED = 128;

		// Token: 0x040005EE RID: 1518
		private static int PASSWORD_LENGTH = 15;

		// Token: 0x040005EF RID: 1519
		private static int TRUST_AUTH_TYPE_CLEAR = 2;

		// Token: 0x040005F0 RID: 1520
		private static int PolicyDnsDomainInformation = 12;

		// Token: 0x040005F1 RID: 1521
		private static int TRUSTED_SET_POSIX = 16;

		// Token: 0x040005F2 RID: 1522
		private static int TRUSTED_SET_AUTH = 32;

		// Token: 0x040005F3 RID: 1523
		internal static int TRUST_TYPE_DOWNLEVEL = 1;

		// Token: 0x040005F4 RID: 1524
		internal static int TRUST_TYPE_UPLEVEL = 2;

		// Token: 0x040005F5 RID: 1525
		internal static int TRUST_TYPE_MIT = 3;

		// Token: 0x040005F6 RID: 1526
		private static int ERROR_ALREADY_EXISTS = 183;

		// Token: 0x040005F7 RID: 1527
		private static int ERROR_INVALID_LEVEL = 124;

		// Token: 0x040005F8 RID: 1528
		private static char[] punctuations = "!@#$%^&*()_-+=[{]};:>|./?".ToCharArray();
	}
}
