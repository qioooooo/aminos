using System;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000AE RID: 174
	[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	public class ForestTrustRelationshipInformation : TrustRelationshipInformation
	{
		// Token: 0x060005C3 RID: 1475 RVA: 0x00020AB0 File Offset: 0x0001FAB0
		internal ForestTrustRelationshipInformation(DirectoryContext context, string source, DS_DOMAIN_TRUSTS unmanagedTrust, TrustType type)
		{
			string text = null;
			string text2 = null;
			this.context = context;
			this.source = source;
			if (unmanagedTrust.DnsDomainName != (IntPtr)0)
			{
				text = Marshal.PtrToStringUni(unmanagedTrust.DnsDomainName);
			}
			if (unmanagedTrust.NetbiosDomainName != (IntPtr)0)
			{
				text2 = Marshal.PtrToStringUni(unmanagedTrust.NetbiosDomainName);
			}
			this.target = ((text == null) ? text2 : text);
			if ((unmanagedTrust.Flags & 2) != 0 && (unmanagedTrust.Flags & 32) != 0)
			{
				this.direction = TrustDirection.Bidirectional;
			}
			else if ((unmanagedTrust.Flags & 2) != 0)
			{
				this.direction = TrustDirection.Outbound;
			}
			else if ((unmanagedTrust.Flags & 32) != 0)
			{
				this.direction = TrustDirection.Inbound;
			}
			this.type = type;
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060005C4 RID: 1476 RVA: 0x00020BAD File Offset: 0x0001FBAD
		public TopLevelNameCollection TopLevelNames
		{
			get
			{
				if (!this.retrieved)
				{
					this.GetForestTrustInfoHelper();
				}
				return this.topLevelNames;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060005C5 RID: 1477 RVA: 0x00020BC3 File Offset: 0x0001FBC3
		public StringCollection ExcludedTopLevelNames
		{
			get
			{
				if (!this.retrieved)
				{
					this.GetForestTrustInfoHelper();
				}
				return this.excludedNames;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060005C6 RID: 1478 RVA: 0x00020BD9 File Offset: 0x0001FBD9
		public ForestTrustDomainInfoCollection TrustedDomainInformation
		{
			get
			{
				if (!this.retrieved)
				{
					this.GetForestTrustInfoHelper();
				}
				return this.domainInfo;
			}
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x00020BF0 File Offset: 0x0001FBF0
		public void Save()
		{
			int num = 0;
			IntPtr intPtr = (IntPtr)0;
			int num2 = 0;
			IntPtr intPtr2 = (IntPtr)0;
			IntPtr intPtr3 = (IntPtr)0;
			IntPtr intPtr4 = (IntPtr)0;
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			bool flag = false;
			IntPtr intPtr5 = (IntPtr)0;
			IntPtr intPtr6 = (IntPtr)0;
			int count = this.TopLevelNames.Count;
			int count2 = this.ExcludedTopLevelNames.Count;
			int count3 = this.TrustedDomainInformation.Count;
			int num3 = 0;
			checked
			{
				num += count;
				num += count2;
				num += count3;
				if (this.binaryData.Count != 0)
				{
					num3 = this.binaryData.Count;
					num++;
					num += num3;
				}
				intPtr = Marshal.AllocHGlobal(num * Marshal.SizeOf(typeof(IntPtr)));
			}
			try
			{
				try
				{
					IntPtr intPtr7 = (IntPtr)0;
					intPtr6 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(FileTime)));
					UnsafeNativeMethods.GetSystemTimeAsFileTime(intPtr6);
					FileTime fileTime = new FileTime();
					Marshal.PtrToStructure(intPtr6, fileTime);
					for (int i = 0; i < count; i++)
					{
						LSA_FOREST_TRUST_RECORD lsa_FOREST_TRUST_RECORD = new LSA_FOREST_TRUST_RECORD();
						lsa_FOREST_TRUST_RECORD.Flags = (int)this.topLevelNames[i].Status;
						lsa_FOREST_TRUST_RECORD.ForestTrustType = LSA_FOREST_TRUST_RECORD_TYPE.ForestTrustTopLevelName;
						TopLevelName topLevelName = this.topLevelNames[i];
						lsa_FOREST_TRUST_RECORD.Time = topLevelName.time;
						lsa_FOREST_TRUST_RECORD.TopLevelName = new LSA_UNICODE_STRING();
						intPtr7 = Marshal.StringToHGlobalUni(topLevelName.Name);
						arrayList.Add(intPtr7);
						UnsafeNativeMethods.RtlInitUnicodeString(lsa_FOREST_TRUST_RECORD.TopLevelName, intPtr7);
						intPtr2 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(LSA_FOREST_TRUST_RECORD)));
						arrayList.Add(intPtr2);
						Marshal.StructureToPtr(lsa_FOREST_TRUST_RECORD, intPtr2, false);
						Marshal.WriteIntPtr(intPtr, Marshal.SizeOf(typeof(IntPtr)) * num2, intPtr2);
						num2++;
					}
					for (int j = 0; j < count2; j++)
					{
						LSA_FOREST_TRUST_RECORD lsa_FOREST_TRUST_RECORD2 = new LSA_FOREST_TRUST_RECORD();
						lsa_FOREST_TRUST_RECORD2.Flags = 0;
						lsa_FOREST_TRUST_RECORD2.ForestTrustType = LSA_FOREST_TRUST_RECORD_TYPE.ForestTrustTopLevelNameEx;
						if (this.excludedNameTime.Contains(this.excludedNames[j]))
						{
							lsa_FOREST_TRUST_RECORD2.Time = (LARGE_INTEGER)this.excludedNameTime[j];
						}
						else
						{
							lsa_FOREST_TRUST_RECORD2.Time = new LARGE_INTEGER();
							lsa_FOREST_TRUST_RECORD2.Time.lowPart = fileTime.lower;
							lsa_FOREST_TRUST_RECORD2.Time.highPart = fileTime.higher;
						}
						lsa_FOREST_TRUST_RECORD2.TopLevelName = new LSA_UNICODE_STRING();
						intPtr7 = Marshal.StringToHGlobalUni(this.excludedNames[j]);
						arrayList.Add(intPtr7);
						UnsafeNativeMethods.RtlInitUnicodeString(lsa_FOREST_TRUST_RECORD2.TopLevelName, intPtr7);
						intPtr2 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(LSA_FOREST_TRUST_RECORD)));
						arrayList.Add(intPtr2);
						Marshal.StructureToPtr(lsa_FOREST_TRUST_RECORD2, intPtr2, false);
						Marshal.WriteIntPtr(intPtr, Marshal.SizeOf(typeof(IntPtr)) * num2, intPtr2);
						num2++;
					}
					for (int k = 0; k < count3; k++)
					{
						LSA_FOREST_TRUST_RECORD lsa_FOREST_TRUST_RECORD3 = new LSA_FOREST_TRUST_RECORD();
						lsa_FOREST_TRUST_RECORD3.Flags = (int)this.domainInfo[k].Status;
						lsa_FOREST_TRUST_RECORD3.ForestTrustType = LSA_FOREST_TRUST_RECORD_TYPE.ForestTrustDomainInfo;
						ForestTrustDomainInformation forestTrustDomainInformation = this.domainInfo[k];
						lsa_FOREST_TRUST_RECORD3.Time = forestTrustDomainInformation.time;
						IntPtr intPtr8 = (IntPtr)0;
						IntPtr intPtr9 = (IntPtr)0;
						intPtr9 = Marshal.StringToHGlobalUni(forestTrustDomainInformation.DomainSid);
						arrayList.Add(intPtr9);
						if (UnsafeNativeMethods.ConvertStringSidToSidW(intPtr9, ref intPtr8) == 0)
						{
							throw ExceptionHelper.GetExceptionFromErrorCode(Marshal.GetLastWin32Error());
						}
						lsa_FOREST_TRUST_RECORD3.DomainInfo = new LSA_FOREST_TRUST_DOMAIN_INFO();
						lsa_FOREST_TRUST_RECORD3.DomainInfo.sid = intPtr8;
						arrayList2.Add(intPtr8);
						lsa_FOREST_TRUST_RECORD3.DomainInfo.DNSNameBuffer = Marshal.StringToHGlobalUni(forestTrustDomainInformation.DnsName);
						arrayList.Add(lsa_FOREST_TRUST_RECORD3.DomainInfo.DNSNameBuffer);
						lsa_FOREST_TRUST_RECORD3.DomainInfo.DNSNameLength = (short)((forestTrustDomainInformation.DnsName == null) ? 0 : (forestTrustDomainInformation.DnsName.Length * 2));
						lsa_FOREST_TRUST_RECORD3.DomainInfo.DNSNameMaximumLength = (short)((forestTrustDomainInformation.DnsName == null) ? 0 : (forestTrustDomainInformation.DnsName.Length * 2));
						lsa_FOREST_TRUST_RECORD3.DomainInfo.NetBIOSNameBuffer = Marshal.StringToHGlobalUni(forestTrustDomainInformation.NetBiosName);
						arrayList.Add(lsa_FOREST_TRUST_RECORD3.DomainInfo.NetBIOSNameBuffer);
						lsa_FOREST_TRUST_RECORD3.DomainInfo.NetBIOSNameLength = (short)((forestTrustDomainInformation.NetBiosName == null) ? 0 : (forestTrustDomainInformation.NetBiosName.Length * 2));
						lsa_FOREST_TRUST_RECORD3.DomainInfo.NetBIOSNameMaximumLength = (short)((forestTrustDomainInformation.NetBiosName == null) ? 0 : (forestTrustDomainInformation.NetBiosName.Length * 2));
						intPtr2 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(LSA_FOREST_TRUST_RECORD)));
						arrayList.Add(intPtr2);
						Marshal.StructureToPtr(lsa_FOREST_TRUST_RECORD3, intPtr2, false);
						Marshal.WriteIntPtr(intPtr, Marshal.SizeOf(typeof(IntPtr)) * num2, intPtr2);
						num2++;
					}
					if (num3 > 0)
					{
						LSA_FOREST_TRUST_RECORD lsa_FOREST_TRUST_RECORD4 = new LSA_FOREST_TRUST_RECORD();
						lsa_FOREST_TRUST_RECORD4.Flags = 0;
						lsa_FOREST_TRUST_RECORD4.ForestTrustType = LSA_FOREST_TRUST_RECORD_TYPE.ForestTrustRecordTypeLast;
						intPtr2 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(LSA_FOREST_TRUST_RECORD)));
						arrayList.Add(intPtr2);
						Marshal.StructureToPtr(lsa_FOREST_TRUST_RECORD4, intPtr2, false);
						Marshal.WriteIntPtr(intPtr, Marshal.SizeOf(typeof(IntPtr)) * num2, intPtr2);
						num2++;
						for (int l = 0; l < num3; l++)
						{
							LSA_FOREST_TRUST_RECORD lsa_FOREST_TRUST_RECORD5 = new LSA_FOREST_TRUST_RECORD();
							lsa_FOREST_TRUST_RECORD5.Flags = 0;
							lsa_FOREST_TRUST_RECORD5.Time = (LARGE_INTEGER)this.binaryDataTime[l];
							lsa_FOREST_TRUST_RECORD5.Data.Length = ((byte[])this.binaryData[l]).Length;
							if (lsa_FOREST_TRUST_RECORD5.Data.Length == 0)
							{
								lsa_FOREST_TRUST_RECORD5.Data.Buffer = (IntPtr)0;
							}
							else
							{
								lsa_FOREST_TRUST_RECORD5.Data.Buffer = Marshal.AllocHGlobal(lsa_FOREST_TRUST_RECORD5.Data.Length);
								arrayList.Add(lsa_FOREST_TRUST_RECORD5.Data.Buffer);
								Marshal.Copy((byte[])this.binaryData[l], 0, lsa_FOREST_TRUST_RECORD5.Data.Buffer, lsa_FOREST_TRUST_RECORD5.Data.Length);
							}
							intPtr2 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(LSA_FOREST_TRUST_RECORD)));
							arrayList.Add(intPtr2);
							Marshal.StructureToPtr(lsa_FOREST_TRUST_RECORD5, intPtr2, false);
							Marshal.WriteIntPtr(intPtr, Marshal.SizeOf(typeof(IntPtr)) * num2, intPtr2);
							num2++;
						}
					}
					LSA_FOREST_TRUST_INFORMATION lsa_FOREST_TRUST_INFORMATION = new LSA_FOREST_TRUST_INFORMATION();
					lsa_FOREST_TRUST_INFORMATION.RecordCount = num;
					lsa_FOREST_TRUST_INFORMATION.Entries = intPtr;
					intPtr3 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(LSA_FOREST_TRUST_INFORMATION)));
					Marshal.StructureToPtr(lsa_FOREST_TRUST_INFORMATION, intPtr3, false);
					string policyServerName = Utils.GetPolicyServerName(this.context, true, true, base.SourceName);
					flag = Utils.Impersonate(this.context);
					PolicySafeHandle policySafeHandle = new PolicySafeHandle(Utils.GetPolicyHandle(policyServerName));
					LSA_UNICODE_STRING lsa_UNICODE_STRING = new LSA_UNICODE_STRING();
					intPtr5 = Marshal.StringToHGlobalUni(base.TargetName);
					UnsafeNativeMethods.RtlInitUnicodeString(lsa_UNICODE_STRING, intPtr5);
					int num4 = UnsafeNativeMethods.LsaSetForestTrustInformation(policySafeHandle, lsa_UNICODE_STRING, intPtr3, 1, out intPtr4);
					if (num4 != 0)
					{
						throw ExceptionHelper.GetExceptionFromErrorCode(UnsafeNativeMethods.LsaNtStatusToWinError(num4), policyServerName);
					}
					if (intPtr4 != (IntPtr)0)
					{
						throw ExceptionHelper.CreateForestTrustCollisionException(intPtr4);
					}
					num4 = UnsafeNativeMethods.LsaSetForestTrustInformation(policySafeHandle, lsa_UNICODE_STRING, intPtr3, 0, out intPtr4);
					if (num4 != 0)
					{
						throw ExceptionHelper.GetExceptionFromErrorCode(num4, policyServerName);
					}
					this.retrieved = false;
				}
				finally
				{
					if (flag)
					{
						Utils.Revert();
					}
					for (int m = 0; m < arrayList.Count; m++)
					{
						Marshal.FreeHGlobal((IntPtr)arrayList[m]);
					}
					for (int n = 0; n < arrayList2.Count; n++)
					{
						UnsafeNativeMethods.LocalFree((IntPtr)arrayList2[n]);
					}
					if (intPtr != (IntPtr)0)
					{
						Marshal.FreeHGlobal(intPtr);
					}
					if (intPtr3 != (IntPtr)0)
					{
						Marshal.FreeHGlobal(intPtr3);
					}
					if (intPtr4 != (IntPtr)0)
					{
						UnsafeNativeMethods.LsaFreeMemory(intPtr4);
					}
					if (intPtr5 != (IntPtr)0)
					{
						Marshal.FreeHGlobal(intPtr5);
					}
					if (intPtr6 != (IntPtr)0)
					{
						Marshal.FreeHGlobal(intPtr6);
					}
				}
			}
			catch
			{
				throw;
			}
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x00021480 File Offset: 0x00020480
		private void GetForestTrustInfoHelper()
		{
			IntPtr intPtr = (IntPtr)0;
			bool flag = false;
			IntPtr intPtr2 = (IntPtr)0;
			TopLevelNameCollection topLevelNameCollection = new TopLevelNameCollection();
			StringCollection stringCollection = new StringCollection();
			ForestTrustDomainInfoCollection forestTrustDomainInfoCollection = new ForestTrustDomainInfoCollection();
			ArrayList arrayList = new ArrayList();
			Hashtable hashtable = new Hashtable();
			ArrayList arrayList2 = new ArrayList();
			try
			{
				try
				{
					LSA_UNICODE_STRING lsa_UNICODE_STRING = new LSA_UNICODE_STRING();
					intPtr2 = Marshal.StringToHGlobalUni(base.TargetName);
					UnsafeNativeMethods.RtlInitUnicodeString(lsa_UNICODE_STRING, intPtr2);
					string policyServerName = Utils.GetPolicyServerName(this.context, true, false, this.source);
					flag = Utils.Impersonate(this.context);
					PolicySafeHandle policySafeHandle = new PolicySafeHandle(Utils.GetPolicyHandle(policyServerName));
					int num = UnsafeNativeMethods.LsaQueryForestTrustInformation(policySafeHandle, lsa_UNICODE_STRING, ref intPtr);
					if (num != 0)
					{
						int num2 = UnsafeNativeMethods.LsaNtStatusToWinError(num);
						if (num2 != 0)
						{
							throw ExceptionHelper.GetExceptionFromErrorCode(num2, policyServerName);
						}
					}
					try
					{
						if (intPtr != (IntPtr)0)
						{
							LSA_FOREST_TRUST_INFORMATION lsa_FOREST_TRUST_INFORMATION = new LSA_FOREST_TRUST_INFORMATION();
							Marshal.PtrToStructure(intPtr, lsa_FOREST_TRUST_INFORMATION);
							int recordCount = lsa_FOREST_TRUST_INFORMATION.RecordCount;
							IntPtr intPtr3 = (IntPtr)0;
							for (int i = 0; i < recordCount; i++)
							{
								intPtr3 = Marshal.ReadIntPtr(lsa_FOREST_TRUST_INFORMATION.Entries, i * Marshal.SizeOf(typeof(IntPtr)));
								LSA_FOREST_TRUST_RECORD lsa_FOREST_TRUST_RECORD = new LSA_FOREST_TRUST_RECORD();
								Marshal.PtrToStructure(intPtr3, lsa_FOREST_TRUST_RECORD);
								if (lsa_FOREST_TRUST_RECORD.ForestTrustType == LSA_FOREST_TRUST_RECORD_TYPE.ForestTrustTopLevelName)
								{
									IntPtr intPtr4 = Utils.AddToIntPtr(intPtr3, 16);
									Marshal.PtrToStructure(intPtr4, lsa_FOREST_TRUST_RECORD.TopLevelName);
									TopLevelName topLevelName = new TopLevelName(lsa_FOREST_TRUST_RECORD.Flags, lsa_FOREST_TRUST_RECORD.TopLevelName, lsa_FOREST_TRUST_RECORD.Time);
									topLevelNameCollection.Add(topLevelName);
								}
								else if (lsa_FOREST_TRUST_RECORD.ForestTrustType == LSA_FOREST_TRUST_RECORD_TYPE.ForestTrustTopLevelNameEx)
								{
									IntPtr intPtr5 = Utils.AddToIntPtr(intPtr3, 16);
									Marshal.PtrToStructure(intPtr5, lsa_FOREST_TRUST_RECORD.TopLevelName);
									string text = Marshal.PtrToStringUni(lsa_FOREST_TRUST_RECORD.TopLevelName.Buffer, (int)(lsa_FOREST_TRUST_RECORD.TopLevelName.Length / 2));
									stringCollection.Add(text);
									hashtable.Add(text, lsa_FOREST_TRUST_RECORD.Time);
								}
								else if (lsa_FOREST_TRUST_RECORD.ForestTrustType == LSA_FOREST_TRUST_RECORD_TYPE.ForestTrustDomainInfo)
								{
									ForestTrustDomainInformation forestTrustDomainInformation = new ForestTrustDomainInformation(lsa_FOREST_TRUST_RECORD.Flags, lsa_FOREST_TRUST_RECORD.DomainInfo, lsa_FOREST_TRUST_RECORD.Time);
									forestTrustDomainInfoCollection.Add(forestTrustDomainInformation);
								}
								else if (lsa_FOREST_TRUST_RECORD.ForestTrustType != LSA_FOREST_TRUST_RECORD_TYPE.ForestTrustRecordTypeLast)
								{
									int length = lsa_FOREST_TRUST_RECORD.Data.Length;
									byte[] array = new byte[length];
									if (lsa_FOREST_TRUST_RECORD.Data.Buffer != (IntPtr)0 && length != 0)
									{
										Marshal.Copy(lsa_FOREST_TRUST_RECORD.Data.Buffer, array, 0, length);
									}
									arrayList.Add(array);
									arrayList2.Add(lsa_FOREST_TRUST_RECORD.Time);
								}
							}
						}
					}
					finally
					{
						UnsafeNativeMethods.LsaFreeMemory(intPtr);
					}
					this.topLevelNames = topLevelNameCollection;
					this.excludedNames = stringCollection;
					this.domainInfo = forestTrustDomainInfoCollection;
					this.binaryData = arrayList;
					this.excludedNameTime = hashtable;
					this.binaryDataTime = arrayList2;
					this.retrieved = true;
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
				}
			}
			catch
			{
				throw;
			}
		}

		// Token: 0x04000474 RID: 1140
		private TopLevelNameCollection topLevelNames = new TopLevelNameCollection();

		// Token: 0x04000475 RID: 1141
		private StringCollection excludedNames = new StringCollection();

		// Token: 0x04000476 RID: 1142
		private ForestTrustDomainInfoCollection domainInfo = new ForestTrustDomainInfoCollection();

		// Token: 0x04000477 RID: 1143
		private ArrayList binaryData = new ArrayList();

		// Token: 0x04000478 RID: 1144
		private Hashtable excludedNameTime = new Hashtable();

		// Token: 0x04000479 RID: 1145
		private ArrayList binaryDataTime = new ArrayList();

		// Token: 0x0400047A RID: 1146
		internal bool retrieved;
	}
}
