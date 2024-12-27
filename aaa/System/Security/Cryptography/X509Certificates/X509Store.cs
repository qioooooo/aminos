using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000346 RID: 838
	public sealed class X509Store
	{
		// Token: 0x06001A46 RID: 6726 RVA: 0x0005B7E3 File Offset: 0x0005A7E3
		public X509Store()
			: this("MY", StoreLocation.CurrentUser)
		{
		}

		// Token: 0x06001A47 RID: 6727 RVA: 0x0005B7F1 File Offset: 0x0005A7F1
		public X509Store(string storeName)
			: this(storeName, StoreLocation.CurrentUser)
		{
		}

		// Token: 0x06001A48 RID: 6728 RVA: 0x0005B7FB File Offset: 0x0005A7FB
		public X509Store(StoreName storeName)
			: this(storeName, StoreLocation.CurrentUser)
		{
		}

		// Token: 0x06001A49 RID: 6729 RVA: 0x0005B805 File Offset: 0x0005A805
		public X509Store(StoreLocation storeLocation)
			: this("MY", storeLocation)
		{
		}

		// Token: 0x06001A4A RID: 6730 RVA: 0x0005B814 File Offset: 0x0005A814
		public X509Store(StoreName storeName, StoreLocation storeLocation)
		{
			this.m_safeCertStoreHandle = SafeCertStoreHandle.InvalidHandle;
			base..ctor();
			if (storeLocation != StoreLocation.CurrentUser && storeLocation != StoreLocation.LocalMachine)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Arg_EnumIllegalVal"), new object[] { "storeLocation" }));
			}
			switch (storeName)
			{
			case StoreName.AddressBook:
				this.m_storeName = "AddressBook";
				break;
			case StoreName.AuthRoot:
				this.m_storeName = "AuthRoot";
				break;
			case StoreName.CertificateAuthority:
				this.m_storeName = "CA";
				break;
			case StoreName.Disallowed:
				this.m_storeName = "Disallowed";
				break;
			case StoreName.My:
				this.m_storeName = "My";
				break;
			case StoreName.Root:
				this.m_storeName = "Root";
				break;
			case StoreName.TrustedPeople:
				this.m_storeName = "TrustedPeople";
				break;
			case StoreName.TrustedPublisher:
				this.m_storeName = "TrustedPublisher";
				break;
			default:
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Arg_EnumIllegalVal"), new object[] { "storeName" }));
			}
			this.m_location = storeLocation;
		}

		// Token: 0x06001A4B RID: 6731 RVA: 0x0005B92C File Offset: 0x0005A92C
		public X509Store(string storeName, StoreLocation storeLocation)
		{
			this.m_safeCertStoreHandle = SafeCertStoreHandle.InvalidHandle;
			base..ctor();
			if (storeLocation != StoreLocation.CurrentUser && storeLocation != StoreLocation.LocalMachine)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Arg_EnumIllegalVal"), new object[] { "storeLocation" }));
			}
			this.m_storeName = storeName;
			this.m_location = storeLocation;
		}

		// Token: 0x06001A4C RID: 6732 RVA: 0x0005B98C File Offset: 0x0005A98C
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public X509Store(IntPtr storeHandle)
		{
			this.m_safeCertStoreHandle = SafeCertStoreHandle.InvalidHandle;
			base..ctor();
			if (storeHandle == IntPtr.Zero)
			{
				throw new ArgumentNullException("storeHandle");
			}
			this.m_safeCertStoreHandle = CAPISafe.CertDuplicateStore(storeHandle);
			if (this.m_safeCertStoreHandle == null || this.m_safeCertStoreHandle.IsInvalid)
			{
				throw new CryptographicException(SR.GetString("Cryptography_InvalidStoreHandle"), "storeHandle");
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06001A4D RID: 6733 RVA: 0x0005B9F8 File Offset: 0x0005A9F8
		public IntPtr StoreHandle
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				return this.m_safeCertStoreHandle.DangerousGetHandle();
			}
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06001A4E RID: 6734 RVA: 0x0005BA05 File Offset: 0x0005AA05
		public StoreLocation Location
		{
			get
			{
				return this.m_location;
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06001A4F RID: 6735 RVA: 0x0005BA0D File Offset: 0x0005AA0D
		public string Name
		{
			get
			{
				return this.m_storeName;
			}
		}

		// Token: 0x06001A50 RID: 6736 RVA: 0x0005BA18 File Offset: 0x0005AA18
		public void Open(OpenFlags flags)
		{
			if (this.m_location != StoreLocation.CurrentUser && this.m_location != StoreLocation.LocalMachine)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Arg_EnumIllegalVal"), new object[] { "m_location" }));
			}
			uint num = X509Utils.MapX509StoreFlags(this.m_location, flags);
			if (!this.m_safeCertStoreHandle.IsInvalid)
			{
				this.m_safeCertStoreHandle.Dispose();
			}
			this.m_safeCertStoreHandle = CAPI.CertOpenStore(new IntPtr(10L), 65537U, IntPtr.Zero, num, this.m_storeName);
			if (this.m_safeCertStoreHandle == null || this.m_safeCertStoreHandle.IsInvalid)
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			CAPISafe.CertControlStore(this.m_safeCertStoreHandle, 0U, 4U, IntPtr.Zero);
		}

		// Token: 0x06001A51 RID: 6737 RVA: 0x0005BADD File Offset: 0x0005AADD
		public void Close()
		{
			if (this.m_safeCertStoreHandle != null && !this.m_safeCertStoreHandle.IsClosed)
			{
				this.m_safeCertStoreHandle.Dispose();
			}
		}

		// Token: 0x06001A52 RID: 6738 RVA: 0x0005BAFF File Offset: 0x0005AAFF
		public void Add(X509Certificate2 certificate)
		{
			if (certificate == null)
			{
				throw new ArgumentNullException("certificate");
			}
			if (!CAPI.CertAddCertificateContextToStore(this.m_safeCertStoreHandle, certificate.CertContext, 5U, SafeCertContextHandle.InvalidHandle))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
		}

		// Token: 0x06001A53 RID: 6739 RVA: 0x0005BB34 File Offset: 0x0005AB34
		public void AddRange(X509Certificate2Collection certificates)
		{
			if (certificates == null)
			{
				throw new ArgumentNullException("certificates");
			}
			int num = 0;
			try
			{
				foreach (X509Certificate2 x509Certificate in certificates)
				{
					this.Add(x509Certificate);
					num++;
				}
			}
			catch
			{
				for (int i = 0; i < num; i++)
				{
					this.Remove(certificates[i]);
				}
				throw;
			}
		}

		// Token: 0x06001A54 RID: 6740 RVA: 0x0005BBA4 File Offset: 0x0005ABA4
		public void Remove(X509Certificate2 certificate)
		{
			if (certificate == null)
			{
				throw new ArgumentNullException("certificate");
			}
			X509Store.RemoveCertificateFromStore(this.m_safeCertStoreHandle, certificate.CertContext);
		}

		// Token: 0x06001A55 RID: 6741 RVA: 0x0005BBC8 File Offset: 0x0005ABC8
		public void RemoveRange(X509Certificate2Collection certificates)
		{
			if (certificates == null)
			{
				throw new ArgumentNullException("certificates");
			}
			int num = 0;
			try
			{
				foreach (X509Certificate2 x509Certificate in certificates)
				{
					this.Remove(x509Certificate);
					num++;
				}
			}
			catch
			{
				for (int i = 0; i < num; i++)
				{
					this.Add(certificates[i]);
				}
				throw;
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06001A56 RID: 6742 RVA: 0x0005BC38 File Offset: 0x0005AC38
		public X509Certificate2Collection Certificates
		{
			get
			{
				if (this.m_safeCertStoreHandle.IsInvalid || this.m_safeCertStoreHandle.IsClosed)
				{
					return new X509Certificate2Collection();
				}
				return X509Utils.GetCertificates(this.m_safeCertStoreHandle);
			}
		}

		// Token: 0x06001A57 RID: 6743 RVA: 0x0005BC68 File Offset: 0x0005AC68
		private static void RemoveCertificateFromStore(SafeCertStoreHandle safeCertStoreHandle, SafeCertContextHandle safeCertContext)
		{
			if (safeCertContext == null || safeCertContext.IsInvalid)
			{
				return;
			}
			SafeCertContextHandle safeCertContextHandle = CAPI.CertFindCertificateInStore(safeCertStoreHandle, 65537U, 0U, 851968U, safeCertContext.DangerousGetHandle(), SafeCertContextHandle.InvalidHandle);
			if (safeCertContextHandle == null || safeCertContextHandle.IsInvalid)
			{
				return;
			}
			GC.SuppressFinalize(safeCertContextHandle);
			if (!CAPI.CertDeleteCertificateFromStore(safeCertContextHandle))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
		}

		// Token: 0x04001B36 RID: 6966
		private string m_storeName;

		// Token: 0x04001B37 RID: 6967
		private StoreLocation m_location;

		// Token: 0x04001B38 RID: 6968
		private SafeCertStoreHandle m_safeCertStoreHandle;
	}
}
