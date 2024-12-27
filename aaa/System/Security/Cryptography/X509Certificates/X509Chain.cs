using System;
using System.Globalization;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000331 RID: 817
	public class X509Chain
	{
		// Token: 0x060019D2 RID: 6610 RVA: 0x00059865 File Offset: 0x00058865
		public static X509Chain Create()
		{
			return (X509Chain)CryptoConfig.CreateFromName("X509Chain");
		}

		// Token: 0x060019D3 RID: 6611 RVA: 0x00059876 File Offset: 0x00058876
		public X509Chain()
			: this(false)
		{
		}

		// Token: 0x060019D4 RID: 6612 RVA: 0x00059880 File Offset: 0x00058880
		public X509Chain(bool useMachineContext)
		{
			this.m_syncRoot = new object();
			base..ctor();
			this.m_status = 0U;
			this.m_chainPolicy = null;
			this.m_chainStatus = null;
			this.m_chainElementCollection = new X509ChainElementCollection();
			this.m_safeCertChainHandle = SafeCertChainHandle.InvalidHandle;
			this.m_useMachineContext = useMachineContext;
		}

		// Token: 0x060019D5 RID: 6613 RVA: 0x000598D0 File Offset: 0x000588D0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public X509Chain(IntPtr chainContext)
		{
			this.m_syncRoot = new object();
			base..ctor();
			if (chainContext == IntPtr.Zero)
			{
				throw new ArgumentNullException("chainContext");
			}
			this.m_safeCertChainHandle = CAPISafe.CertDuplicateCertificateChain(chainContext);
			if (this.m_safeCertChainHandle == null || this.m_safeCertChainHandle == SafeCertChainHandle.InvalidHandle)
			{
				throw new CryptographicException(SR.GetString("Cryptography_InvalidContextHandle"), "chainContext");
			}
			this.Init();
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x060019D6 RID: 6614 RVA: 0x00059942 File Offset: 0x00058942
		public IntPtr ChainContext
		{
			[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				return this.m_safeCertChainHandle.DangerousGetHandle();
			}
		}

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x060019D7 RID: 6615 RVA: 0x0005994F File Offset: 0x0005894F
		// (set) Token: 0x060019D8 RID: 6616 RVA: 0x0005996A File Offset: 0x0005896A
		public X509ChainPolicy ChainPolicy
		{
			get
			{
				if (this.m_chainPolicy == null)
				{
					this.m_chainPolicy = new X509ChainPolicy();
				}
				return this.m_chainPolicy;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_chainPolicy = value;
			}
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x060019D9 RID: 6617 RVA: 0x00059981 File Offset: 0x00058981
		public X509ChainStatus[] ChainStatus
		{
			get
			{
				if (this.m_chainStatus == null)
				{
					if (this.m_status == 0U)
					{
						this.m_chainStatus = new X509ChainStatus[0];
					}
					else
					{
						this.m_chainStatus = X509Chain.GetChainStatusInformation(this.m_status);
					}
				}
				return this.m_chainStatus;
			}
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x060019DA RID: 6618 RVA: 0x000599B8 File Offset: 0x000589B8
		public X509ChainElementCollection ChainElements
		{
			get
			{
				return this.m_chainElementCollection;
			}
		}

		// Token: 0x060019DB RID: 6619 RVA: 0x000599C0 File Offset: 0x000589C0
		[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
		public bool Build(X509Certificate2 certificate)
		{
			bool flag;
			lock (this.m_syncRoot)
			{
				if (certificate == null || certificate.CertContext.IsInvalid)
				{
					throw new ArgumentException(SR.GetString("Cryptography_InvalidContextHandle"), "certificate");
				}
				StorePermission storePermission = new StorePermission(StorePermissionFlags.OpenStore | StorePermissionFlags.EnumerateCertificates);
				storePermission.Demand();
				X509ChainPolicy chainPolicy = this.ChainPolicy;
				if (chainPolicy.RevocationMode == X509RevocationMode.Online && (certificate.Extensions["2.5.29.31"] != null || certificate.Extensions["1.3.6.1.5.5.7.1.1"] != null))
				{
					PermissionSet permissionSet = new PermissionSet(PermissionState.None);
					permissionSet.AddPermission(new WebPermission(PermissionState.Unrestricted));
					permissionSet.AddPermission(new StorePermission(StorePermissionFlags.AddToStore));
					permissionSet.Demand();
				}
				this.Reset();
				int num = X509Chain.BuildChain(this.m_useMachineContext ? new IntPtr(1L) : new IntPtr(0L), certificate.CertContext, chainPolicy.ExtraStore, chainPolicy.ApplicationPolicy, chainPolicy.CertificatePolicy, chainPolicy.RevocationMode, chainPolicy.RevocationFlag, chainPolicy.VerificationTime, chainPolicy.UrlRetrievalTimeout, ref this.m_safeCertChainHandle);
				if (num != 0)
				{
					if (X509Chain.CompatSwitches.ShouldThrowOnChainBuildingFailure)
					{
						throw new CryptographicException(num);
					}
					flag = false;
				}
				else
				{
					this.Init();
					CAPIBase.CERT_CHAIN_POLICY_PARA cert_CHAIN_POLICY_PARA = new CAPIBase.CERT_CHAIN_POLICY_PARA(Marshal.SizeOf(typeof(CAPIBase.CERT_CHAIN_POLICY_PARA)));
					CAPIBase.CERT_CHAIN_POLICY_STATUS cert_CHAIN_POLICY_STATUS = new CAPIBase.CERT_CHAIN_POLICY_STATUS(Marshal.SizeOf(typeof(CAPIBase.CERT_CHAIN_POLICY_STATUS)));
					cert_CHAIN_POLICY_PARA.dwFlags = (uint)chainPolicy.VerificationFlags;
					if (!CAPISafe.CertVerifyCertificateChainPolicy(new IntPtr(1L), this.m_safeCertChainHandle, ref cert_CHAIN_POLICY_PARA, ref cert_CHAIN_POLICY_STATUS))
					{
						throw new CryptographicException(Marshal.GetLastWin32Error());
					}
					CAPISafe.SetLastError(cert_CHAIN_POLICY_STATUS.dwError);
					flag = cert_CHAIN_POLICY_STATUS.dwError == 0U;
				}
			}
			return flag;
		}

		// Token: 0x060019DC RID: 6620 RVA: 0x00059B8C File Offset: 0x00058B8C
		[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
		public void Reset()
		{
			this.m_status = 0U;
			this.m_chainStatus = null;
			this.m_chainElementCollection = new X509ChainElementCollection();
			if (!this.m_safeCertChainHandle.IsInvalid)
			{
				this.m_safeCertChainHandle.Dispose();
				this.m_safeCertChainHandle = SafeCertChainHandle.InvalidHandle;
			}
		}

		// Token: 0x060019DD RID: 6621 RVA: 0x00059BCC File Offset: 0x00058BCC
		private unsafe void Init()
		{
			using (SafeCertChainHandle safeCertChainHandle = CAPISafe.CertDuplicateCertificateChain(this.m_safeCertChainHandle))
			{
				CAPIBase.CERT_CHAIN_CONTEXT cert_CHAIN_CONTEXT = new CAPIBase.CERT_CHAIN_CONTEXT(Marshal.SizeOf(typeof(CAPIBase.CERT_CHAIN_CONTEXT)));
				uint num = (uint)Marshal.ReadInt32(safeCertChainHandle.DangerousGetHandle());
				if ((ulong)num > (ulong)((long)Marshal.SizeOf(cert_CHAIN_CONTEXT)))
				{
					num = (uint)Marshal.SizeOf(cert_CHAIN_CONTEXT);
				}
				X509Utils.memcpy(this.m_safeCertChainHandle.DangerousGetHandle(), new IntPtr((void*)(&cert_CHAIN_CONTEXT)), num);
				this.m_status = cert_CHAIN_CONTEXT.dwErrorStatus;
				this.m_chainElementCollection = new X509ChainElementCollection(Marshal.ReadIntPtr(cert_CHAIN_CONTEXT.rgpChain));
			}
		}

		// Token: 0x060019DE RID: 6622 RVA: 0x00059C84 File Offset: 0x00058C84
		internal static X509ChainStatus[] GetChainStatusInformation(uint dwStatus)
		{
			if (dwStatus == 0U)
			{
				return new X509ChainStatus[0];
			}
			int num = 0;
			for (uint num2 = dwStatus; num2 != 0U; num2 >>= 1)
			{
				if ((num2 & 1U) != 0U)
				{
					num++;
				}
			}
			X509ChainStatus[] array = new X509ChainStatus[num];
			int num3 = 0;
			if ((dwStatus & 8U) != 0U)
			{
				array[num3].StatusInformation = X509Utils.GetSystemErrorString(-2146869244);
				array[num3].Status = X509ChainStatusFlags.NotSignatureValid;
				num3++;
				dwStatus &= 4294967287U;
			}
			if ((dwStatus & 262144U) != 0U)
			{
				array[num3].StatusInformation = X509Utils.GetSystemErrorString(-2146869244);
				array[num3].Status = X509ChainStatusFlags.CtlNotSignatureValid;
				num3++;
				dwStatus &= 4294705151U;
			}
			if ((dwStatus & 32U) != 0U)
			{
				array[num3].StatusInformation = X509Utils.GetSystemErrorString(-2146762487);
				array[num3].Status = X509ChainStatusFlags.UntrustedRoot;
				num3++;
				dwStatus &= 4294967263U;
			}
			if ((dwStatus & 65536U) != 0U)
			{
				array[num3].StatusInformation = X509Utils.GetSystemErrorString(-2146762486);
				array[num3].Status = X509ChainStatusFlags.PartialChain;
				num3++;
				dwStatus &= 4294901759U;
			}
			if ((dwStatus & 4U) != 0U)
			{
				array[num3].StatusInformation = X509Utils.GetSystemErrorString(-2146885616);
				array[num3].Status = X509ChainStatusFlags.Revoked;
				num3++;
				dwStatus &= 4294967291U;
			}
			if ((dwStatus & 16U) != 0U)
			{
				array[num3].StatusInformation = X509Utils.GetSystemErrorString(-2146762480);
				array[num3].Status = X509ChainStatusFlags.NotValidForUsage;
				num3++;
				dwStatus &= 4294967279U;
			}
			if ((dwStatus & 524288U) != 0U)
			{
				array[num3].StatusInformation = X509Utils.GetSystemErrorString(-2146762480);
				array[num3].Status = X509ChainStatusFlags.CtlNotValidForUsage;
				num3++;
				dwStatus &= 4294443007U;
			}
			if ((dwStatus & 1U) != 0U)
			{
				array[num3].StatusInformation = X509Utils.GetSystemErrorString(-2146762495);
				array[num3].Status = X509ChainStatusFlags.NotTimeValid;
				num3++;
				dwStatus &= 4294967294U;
			}
			if ((dwStatus & 131072U) != 0U)
			{
				array[num3].StatusInformation = X509Utils.GetSystemErrorString(-2146762495);
				array[num3].Status = X509ChainStatusFlags.CtlNotTimeValid;
				num3++;
				dwStatus &= 4294836223U;
			}
			if ((dwStatus & 2048U) != 0U)
			{
				array[num3].StatusInformation = X509Utils.GetSystemErrorString(-2146762476);
				array[num3].Status = X509ChainStatusFlags.InvalidNameConstraints;
				num3++;
				dwStatus &= 4294965247U;
			}
			if ((dwStatus & 4096U) != 0U)
			{
				array[num3].StatusInformation = X509Utils.GetSystemErrorString(-2146762476);
				array[num3].Status = X509ChainStatusFlags.HasNotSupportedNameConstraint;
				num3++;
				dwStatus &= 4294963199U;
			}
			if ((dwStatus & 8192U) != 0U)
			{
				array[num3].StatusInformation = X509Utils.GetSystemErrorString(-2146762476);
				array[num3].Status = X509ChainStatusFlags.HasNotDefinedNameConstraint;
				num3++;
				dwStatus &= 4294959103U;
			}
			if ((dwStatus & 16384U) != 0U)
			{
				array[num3].StatusInformation = X509Utils.GetSystemErrorString(-2146762476);
				array[num3].Status = X509ChainStatusFlags.HasNotPermittedNameConstraint;
				num3++;
				dwStatus &= 4294950911U;
			}
			if ((dwStatus & 32768U) != 0U)
			{
				array[num3].StatusInformation = X509Utils.GetSystemErrorString(-2146762476);
				array[num3].Status = X509ChainStatusFlags.HasExcludedNameConstraint;
				num3++;
				dwStatus &= 4294934527U;
			}
			if ((dwStatus & 512U) != 0U)
			{
				array[num3].StatusInformation = X509Utils.GetSystemErrorString(-2146762477);
				array[num3].Status = X509ChainStatusFlags.InvalidPolicyConstraints;
				num3++;
				dwStatus &= 4294966783U;
			}
			if ((dwStatus & 33554432U) != 0U)
			{
				array[num3].StatusInformation = X509Utils.GetSystemErrorString(-2146762477);
				array[num3].Status = X509ChainStatusFlags.NoIssuanceChainPolicy;
				num3++;
				dwStatus &= 4261412863U;
			}
			if ((dwStatus & 1024U) != 0U)
			{
				array[num3].StatusInformation = X509Utils.GetSystemErrorString(-2146869223);
				array[num3].Status = X509ChainStatusFlags.InvalidBasicConstraints;
				num3++;
				dwStatus &= 4294966271U;
			}
			if ((dwStatus & 2U) != 0U)
			{
				array[num3].StatusInformation = X509Utils.GetSystemErrorString(-2146762494);
				array[num3].Status = X509ChainStatusFlags.NotTimeNested;
				num3++;
				dwStatus &= 4294967293U;
			}
			if ((dwStatus & 64U) != 0U)
			{
				array[num3].StatusInformation = X509Utils.GetSystemErrorString(-2146885614);
				array[num3].Status = X509ChainStatusFlags.RevocationStatusUnknown;
				num3++;
				dwStatus &= 4294967231U;
			}
			if ((dwStatus & 16777216U) != 0U)
			{
				array[num3].StatusInformation = X509Utils.GetSystemErrorString(-2146885613);
				array[num3].Status = X509ChainStatusFlags.OfflineRevocation;
				num3++;
				dwStatus &= 4278190079U;
			}
			int num4 = 0;
			for (uint num5 = dwStatus; num5 != 0U; num5 >>= 1)
			{
				if ((num5 & 1U) != 0U)
				{
					array[num3].Status = (X509ChainStatusFlags)(1 << num4);
					array[num3].StatusInformation = SR.GetString("Unknown_Error");
					num3++;
				}
				num4++;
			}
			return array;
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x0005A184 File Offset: 0x00059184
		internal unsafe static int BuildChain(IntPtr hChainEngine, SafeCertContextHandle pCertContext, X509Certificate2Collection extraStore, OidCollection applicationPolicy, OidCollection certificatePolicy, X509RevocationMode revocationMode, X509RevocationFlag revocationFlag, DateTime verificationTime, TimeSpan timeout, ref SafeCertChainHandle ppChainContext)
		{
			if (pCertContext == null || pCertContext.IsInvalid)
			{
				throw new ArgumentException(SR.GetString("Cryptography_InvalidContextHandle"), "pCertContext");
			}
			SafeCertStoreHandle safeCertStoreHandle = SafeCertStoreHandle.InvalidHandle;
			if (extraStore != null && extraStore.Count > 0)
			{
				safeCertStoreHandle = X509Utils.ExportToMemoryStore(extraStore);
			}
			CAPIBase.CERT_CHAIN_PARA cert_CHAIN_PARA = default(CAPIBase.CERT_CHAIN_PARA);
			cert_CHAIN_PARA.cbSize = (uint)Marshal.SizeOf(cert_CHAIN_PARA);
			SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
			if (applicationPolicy != null && applicationPolicy.Count > 0)
			{
				cert_CHAIN_PARA.RequestedUsage.dwType = 0U;
				cert_CHAIN_PARA.RequestedUsage.Usage.cUsageIdentifier = (uint)applicationPolicy.Count;
				safeLocalAllocHandle = X509Utils.CopyOidsToUnmanagedMemory(applicationPolicy);
				cert_CHAIN_PARA.RequestedUsage.Usage.rgpszUsageIdentifier = safeLocalAllocHandle.DangerousGetHandle();
			}
			SafeLocalAllocHandle safeLocalAllocHandle2 = SafeLocalAllocHandle.InvalidHandle;
			if (certificatePolicy != null && certificatePolicy.Count > 0)
			{
				cert_CHAIN_PARA.RequestedIssuancePolicy.dwType = 0U;
				cert_CHAIN_PARA.RequestedIssuancePolicy.Usage.cUsageIdentifier = (uint)certificatePolicy.Count;
				safeLocalAllocHandle2 = X509Utils.CopyOidsToUnmanagedMemory(certificatePolicy);
				cert_CHAIN_PARA.RequestedIssuancePolicy.Usage.rgpszUsageIdentifier = safeLocalAllocHandle2.DangerousGetHandle();
			}
			cert_CHAIN_PARA.dwUrlRetrievalTimeout = (uint)timeout.Milliseconds;
			global::System.Runtime.InteropServices.ComTypes.FILETIME filetime = default(global::System.Runtime.InteropServices.ComTypes.FILETIME);
			*(long*)(&filetime) = verificationTime.ToFileTime();
			uint num = X509Utils.MapRevocationFlags(revocationMode, revocationFlag);
			if (!CAPISafe.CertGetCertificateChain(hChainEngine, pCertContext, ref filetime, safeCertStoreHandle, ref cert_CHAIN_PARA, num, IntPtr.Zero, ref ppChainContext))
			{
				return Marshal.GetHRForLastWin32Error();
			}
			safeLocalAllocHandle.Dispose();
			safeLocalAllocHandle2.Dispose();
			return 0;
		}

		// Token: 0x04001ADA RID: 6874
		private uint m_status;

		// Token: 0x04001ADB RID: 6875
		private X509ChainPolicy m_chainPolicy;

		// Token: 0x04001ADC RID: 6876
		private X509ChainStatus[] m_chainStatus;

		// Token: 0x04001ADD RID: 6877
		private X509ChainElementCollection m_chainElementCollection;

		// Token: 0x04001ADE RID: 6878
		private SafeCertChainHandle m_safeCertChainHandle;

		// Token: 0x04001ADF RID: 6879
		private bool m_useMachineContext;

		// Token: 0x04001AE0 RID: 6880
		private readonly object m_syncRoot;

		// Token: 0x02000332 RID: 818
		private static class CompatSwitches
		{
			// Token: 0x060019E0 RID: 6624 RVA: 0x0005A2E8 File Offset: 0x000592E8
			[SecuritySafeCritical]
			[RegistryPermission(SecurityAction.Assert, Unrestricted = true)]
			[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
			[EnvironmentPermission(SecurityAction.Assert, Unrestricted = true)]
			private static int ReadInt32CompatSwitch(string switchName, int defaultValue)
			{
				string environmentVariable = Environment.GetEnvironmentVariable("COMPlus_" + switchName);
				int num;
				if (environmentVariable != null && int.TryParse(environmentVariable, NumberStyles.Integer, CultureInfo.InvariantCulture, out num))
				{
					return num;
				}
				int? num2 = X509Chain.CompatSwitches.ReadInt32CompatSwitchFromRegistry(Registry.CurrentUser, switchName);
				if (num2 != null)
				{
					return num2.GetValueOrDefault();
				}
				int? num3 = X509Chain.CompatSwitches.ReadInt32CompatSwitchFromRegistry(Registry.LocalMachine, switchName);
				if (num3 == null)
				{
					return defaultValue;
				}
				return num3.GetValueOrDefault();
			}

			// Token: 0x060019E1 RID: 6625 RVA: 0x0005A358 File Offset: 0x00059358
			[SecuritySafeCritical]
			[RegistryPermission(SecurityAction.Assert, Unrestricted = true)]
			[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
			private static int? ReadInt32CompatSwitchFromRegistry(RegistryKey baseKey, string switchName)
			{
				try
				{
					using (RegistryKey registryKey = baseKey.OpenSubKey("SOFTWARE\\Microsoft\\.NETFramework", false))
					{
						if (registryKey != null)
						{
							object value = registryKey.GetValue(switchName);
							if (value is int)
							{
								return new int?((int)value);
							}
						}
					}
				}
				catch
				{
				}
				return null;
			}

			// Token: 0x04001AE1 RID: 6881
			internal static readonly bool ShouldThrowOnChainBuildingFailure = X509Chain.CompatSwitches.ReadInt32CompatSwitch("X509Chain_ThrowOnBuildFailure", 1) != 0;
		}
	}
}
