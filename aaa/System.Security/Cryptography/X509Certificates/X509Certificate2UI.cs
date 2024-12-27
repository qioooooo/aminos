using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000080 RID: 128
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class X509Certificate2UI
	{
		// Token: 0x06000209 RID: 521 RVA: 0x0000BE09 File Offset: 0x0000AE09
		private X509Certificate2UI()
		{
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000BE11 File Offset: 0x0000AE11
		public static void DisplayCertificate(X509Certificate2 certificate)
		{
			if (certificate == null)
			{
				throw new ArgumentNullException("certificate");
			}
			X509Certificate2UI.DisplayX509Certificate(X509Utils.GetCertContext(certificate), IntPtr.Zero);
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000BE31 File Offset: 0x0000AE31
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void DisplayCertificate(X509Certificate2 certificate, IntPtr hwndParent)
		{
			if (certificate == null)
			{
				throw new ArgumentNullException("certificate");
			}
			X509Certificate2UI.DisplayX509Certificate(X509Utils.GetCertContext(certificate), hwndParent);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000BE4D File Offset: 0x0000AE4D
		public static X509Certificate2Collection SelectFromCollection(X509Certificate2Collection certificates, string title, string message, X509SelectionFlag selectionFlag)
		{
			return X509Certificate2UI.SelectFromCollectionHelper(certificates, title, message, selectionFlag, IntPtr.Zero);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000BE5D File Offset: 0x0000AE5D
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static X509Certificate2Collection SelectFromCollection(X509Certificate2Collection certificates, string title, string message, X509SelectionFlag selectionFlag, IntPtr hwndParent)
		{
			return X509Certificate2UI.SelectFromCollectionHelper(certificates, title, message, selectionFlag, hwndParent);
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000BE6C File Offset: 0x0000AE6C
		private static void DisplayX509Certificate(SafeCertContextHandle safeCertContext, IntPtr hwndParent)
		{
			if (safeCertContext.IsInvalid)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_InvalidHandle"), "safeCertContext");
			}
			int num = 0;
			CAPIBase.CRYPTUI_VIEWCERTIFICATE_STRUCTW cryptui_VIEWCERTIFICATE_STRUCTW = new CAPIBase.CRYPTUI_VIEWCERTIFICATE_STRUCTW();
			cryptui_VIEWCERTIFICATE_STRUCTW.dwSize = (uint)Marshal.SizeOf(cryptui_VIEWCERTIFICATE_STRUCTW);
			cryptui_VIEWCERTIFICATE_STRUCTW.hwndParent = hwndParent;
			cryptui_VIEWCERTIFICATE_STRUCTW.dwFlags = 0U;
			cryptui_VIEWCERTIFICATE_STRUCTW.szTitle = null;
			cryptui_VIEWCERTIFICATE_STRUCTW.pCertContext = safeCertContext.DangerousGetHandle();
			cryptui_VIEWCERTIFICATE_STRUCTW.rgszPurposes = IntPtr.Zero;
			cryptui_VIEWCERTIFICATE_STRUCTW.cPurposes = 0U;
			cryptui_VIEWCERTIFICATE_STRUCTW.pCryptProviderData = IntPtr.Zero;
			cryptui_VIEWCERTIFICATE_STRUCTW.fpCryptProviderDataTrustedUsage = false;
			cryptui_VIEWCERTIFICATE_STRUCTW.idxSigner = 0U;
			cryptui_VIEWCERTIFICATE_STRUCTW.idxCert = 0U;
			cryptui_VIEWCERTIFICATE_STRUCTW.fCounterSigner = false;
			cryptui_VIEWCERTIFICATE_STRUCTW.idxCounterSigner = 0U;
			cryptui_VIEWCERTIFICATE_STRUCTW.cStores = 0U;
			cryptui_VIEWCERTIFICATE_STRUCTW.rghStores = IntPtr.Zero;
			cryptui_VIEWCERTIFICATE_STRUCTW.cPropSheetPages = 0U;
			cryptui_VIEWCERTIFICATE_STRUCTW.rgPropSheetPages = IntPtr.Zero;
			cryptui_VIEWCERTIFICATE_STRUCTW.nStartPage = 0U;
			if (!CAPI.CryptUIDlgViewCertificateW(cryptui_VIEWCERTIFICATE_STRUCTW, IntPtr.Zero))
			{
				num = Marshal.GetLastWin32Error();
			}
			if (num != 0 && num != 1223)
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000BF60 File Offset: 0x0000AF60
		private static X509Certificate2Collection SelectFromCollectionHelper(X509Certificate2Collection certificates, string title, string message, X509SelectionFlag selectionFlag, IntPtr hwndParent)
		{
			if (certificates == null)
			{
				throw new ArgumentNullException("certificates");
			}
			if (selectionFlag < X509SelectionFlag.SingleSelection || selectionFlag > X509SelectionFlag.MultiSelection)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SecurityResources.GetResourceString("Arg_EnumIllegalVal"), new object[] { "selectionFlag" }));
			}
			StorePermission storePermission = new StorePermission(StorePermissionFlags.AllFlags);
			storePermission.Assert();
			SafeCertStoreHandle safeCertStoreHandle = X509Utils.ExportToMemoryStore(certificates);
			SafeCertStoreHandle safeCertStoreHandle2 = SafeCertStoreHandle.InvalidHandle;
			safeCertStoreHandle2 = X509Certificate2UI.SelectFromStore(safeCertStoreHandle, title, message, selectionFlag, hwndParent);
			X509Certificate2Collection certificates2 = X509Utils.GetCertificates(safeCertStoreHandle2);
			safeCertStoreHandle2.Dispose();
			safeCertStoreHandle.Dispose();
			return certificates2;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000BFF0 File Offset: 0x0000AFF0
		private unsafe static SafeCertStoreHandle SelectFromStore(SafeCertStoreHandle safeSourceStoreHandle, string title, string message, X509SelectionFlag selectionFlags, IntPtr hwndParent)
		{
			int num = 0;
			SafeCertStoreHandle safeCertStoreHandle = CAPI.CertOpenStore((IntPtr)2L, 65537U, IntPtr.Zero, 0U, null);
			if (safeCertStoreHandle == null || safeCertStoreHandle.IsInvalid)
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			CAPIBase.CRYPTUI_SELECTCERTIFICATE_STRUCTW cryptui_SELECTCERTIFICATE_STRUCTW = new CAPIBase.CRYPTUI_SELECTCERTIFICATE_STRUCTW();
			cryptui_SELECTCERTIFICATE_STRUCTW.dwSize = (uint)(int)Marshal.OffsetOf(typeof(CAPIBase.CRYPTUI_SELECTCERTIFICATE_STRUCTW), "hSelectedCertStore");
			cryptui_SELECTCERTIFICATE_STRUCTW.hwndParent = hwndParent;
			cryptui_SELECTCERTIFICATE_STRUCTW.dwFlags = (uint)selectionFlags;
			cryptui_SELECTCERTIFICATE_STRUCTW.szTitle = title;
			cryptui_SELECTCERTIFICATE_STRUCTW.dwDontUseColumn = 0U;
			cryptui_SELECTCERTIFICATE_STRUCTW.szDisplayString = message;
			cryptui_SELECTCERTIFICATE_STRUCTW.pFilterCallback = IntPtr.Zero;
			cryptui_SELECTCERTIFICATE_STRUCTW.pDisplayCallback = IntPtr.Zero;
			cryptui_SELECTCERTIFICATE_STRUCTW.pvCallbackData = IntPtr.Zero;
			cryptui_SELECTCERTIFICATE_STRUCTW.cDisplayStores = 1U;
			IntPtr intPtr = safeSourceStoreHandle.DangerousGetHandle();
			cryptui_SELECTCERTIFICATE_STRUCTW.rghDisplayStores = new IntPtr((void*)(&intPtr));
			cryptui_SELECTCERTIFICATE_STRUCTW.cStores = 0U;
			cryptui_SELECTCERTIFICATE_STRUCTW.rghStores = IntPtr.Zero;
			cryptui_SELECTCERTIFICATE_STRUCTW.cPropSheetPages = 0U;
			cryptui_SELECTCERTIFICATE_STRUCTW.rgPropSheetPages = IntPtr.Zero;
			cryptui_SELECTCERTIFICATE_STRUCTW.hSelectedCertStore = safeCertStoreHandle.DangerousGetHandle();
			SafeCertContextHandle safeCertContextHandle = CAPI.CryptUIDlgSelectCertificateW(cryptui_SELECTCERTIFICATE_STRUCTW);
			if (safeCertContextHandle != null && !safeCertContextHandle.IsInvalid)
			{
				SafeCertContextHandle invalidHandle = SafeCertContextHandle.InvalidHandle;
				if (!CAPI.CertAddCertificateContextToStore(safeCertStoreHandle, safeCertContextHandle, 7U, invalidHandle))
				{
					num = Marshal.GetLastWin32Error();
				}
			}
			if (num != 0)
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			return safeCertStoreHandle;
		}
	}
}
