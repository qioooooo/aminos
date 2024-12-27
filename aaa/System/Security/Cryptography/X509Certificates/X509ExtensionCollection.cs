using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000341 RID: 833
	public sealed class X509ExtensionCollection : ICollection, IEnumerable
	{
		// Token: 0x06001A34 RID: 6708 RVA: 0x0005B4B0 File Offset: 0x0005A4B0
		public X509ExtensionCollection()
		{
		}

		// Token: 0x06001A35 RID: 6709 RVA: 0x0005B4C4 File Offset: 0x0005A4C4
		internal unsafe X509ExtensionCollection(SafeCertContextHandle safeCertContextHandle)
		{
			using (SafeCertContextHandle safeCertContextHandle2 = CAPI.CertDuplicateCertificateContext(safeCertContextHandle))
			{
				CAPIBase.CERT_CONTEXT cert_CONTEXT = *(CAPIBase.CERT_CONTEXT*)(void*)safeCertContextHandle2.DangerousGetHandle();
				CAPIBase.CERT_INFO cert_INFO = (CAPIBase.CERT_INFO)Marshal.PtrToStructure(cert_CONTEXT.pCertInfo, typeof(CAPIBase.CERT_INFO));
				uint cExtension = cert_INFO.cExtension;
				IntPtr rgExtension = cert_INFO.rgExtension;
				for (uint num = 0U; num < cExtension; num += 1U)
				{
					X509Extension x509Extension = new X509Extension(new IntPtr((long)rgExtension + (long)((ulong)num * (ulong)((long)Marshal.SizeOf(typeof(CAPIBase.CERT_EXTENSION))))));
					X509Extension x509Extension2 = CryptoConfig.CreateFromName(x509Extension.Oid.Value) as X509Extension;
					if (x509Extension2 != null)
					{
						x509Extension2.CopyFrom(x509Extension);
						x509Extension = x509Extension2;
					}
					this.Add(x509Extension);
				}
			}
		}

		// Token: 0x17000507 RID: 1287
		public X509Extension this[int index]
		{
			get
			{
				if (index < 0)
				{
					throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumNotStarted"));
				}
				if (index >= this.m_list.Count)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("ArgumentOutOfRange_Index"));
				}
				return (X509Extension)this.m_list[index];
			}
		}

		// Token: 0x17000508 RID: 1288
		public X509Extension this[string oid]
		{
			get
			{
				string text = X509Utils.FindOidInfo(2U, oid, OidGroup.ExtensionOrAttribute);
				if (text == null)
				{
					text = oid;
				}
				foreach (object obj in this.m_list)
				{
					X509Extension x509Extension = (X509Extension)obj;
					if (string.Compare(x509Extension.Oid.Value, text, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return x509Extension;
					}
				}
				return null;
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06001A38 RID: 6712 RVA: 0x0005B688 File Offset: 0x0005A688
		public int Count
		{
			get
			{
				return this.m_list.Count;
			}
		}

		// Token: 0x06001A39 RID: 6713 RVA: 0x0005B695 File Offset: 0x0005A695
		public int Add(X509Extension extension)
		{
			if (extension == null)
			{
				throw new ArgumentNullException("extension");
			}
			return this.m_list.Add(extension);
		}

		// Token: 0x06001A3A RID: 6714 RVA: 0x0005B6B1 File Offset: 0x0005A6B1
		public X509ExtensionEnumerator GetEnumerator()
		{
			return new X509ExtensionEnumerator(this);
		}

		// Token: 0x06001A3B RID: 6715 RVA: 0x0005B6B9 File Offset: 0x0005A6B9
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new X509ExtensionEnumerator(this);
		}

		// Token: 0x06001A3C RID: 6716 RVA: 0x0005B6C4 File Offset: 0x0005A6C4
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(SR.GetString("Arg_RankMultiDimNotSupported"));
			}
			if (index < 0 || index >= array.Length)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("ArgumentOutOfRange_Index"));
			}
			if (index + this.Count > array.Length)
			{
				throw new ArgumentException(SR.GetString("Argument_InvalidOffLen"));
			}
			for (int i = 0; i < this.Count; i++)
			{
				array.SetValue(this[i], index);
				index++;
			}
		}

		// Token: 0x06001A3D RID: 6717 RVA: 0x0005B75E File Offset: 0x0005A75E
		public void CopyTo(X509Extension[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06001A3E RID: 6718 RVA: 0x0005B768 File Offset: 0x0005A768
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06001A3F RID: 6719 RVA: 0x0005B76B File Offset: 0x0005A76B
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x04001B21 RID: 6945
		private ArrayList m_list = new ArrayList();
	}
}
