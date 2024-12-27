using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x0200033D RID: 829
	public sealed class X509BasicConstraintsExtension : X509Extension
	{
		// Token: 0x06001A17 RID: 6679 RVA: 0x0005ABAC File Offset: 0x00059BAC
		public X509BasicConstraintsExtension()
			: base("2.5.29.19")
		{
			this.m_decoded = true;
		}

		// Token: 0x06001A18 RID: 6680 RVA: 0x0005ABC0 File Offset: 0x00059BC0
		public X509BasicConstraintsExtension(bool certificateAuthority, bool hasPathLengthConstraint, int pathLengthConstraint, bool critical)
			: base("2.5.29.19", X509BasicConstraintsExtension.EncodeExtension(certificateAuthority, hasPathLengthConstraint, pathLengthConstraint), critical)
		{
		}

		// Token: 0x06001A19 RID: 6681 RVA: 0x0005ABD7 File Offset: 0x00059BD7
		public X509BasicConstraintsExtension(AsnEncodedData encodedBasicConstraints, bool critical)
			: base("2.5.29.19", encodedBasicConstraints.RawData, critical)
		{
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06001A1A RID: 6682 RVA: 0x0005ABEB File Offset: 0x00059BEB
		public bool CertificateAuthority
		{
			get
			{
				if (!this.m_decoded)
				{
					this.DecodeExtension();
				}
				return this.m_isCA;
			}
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06001A1B RID: 6683 RVA: 0x0005AC01 File Offset: 0x00059C01
		public bool HasPathLengthConstraint
		{
			get
			{
				if (!this.m_decoded)
				{
					this.DecodeExtension();
				}
				return this.m_hasPathLenConstraint;
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06001A1C RID: 6684 RVA: 0x0005AC17 File Offset: 0x00059C17
		public int PathLengthConstraint
		{
			get
			{
				if (!this.m_decoded)
				{
					this.DecodeExtension();
				}
				return this.m_pathLenConstraint;
			}
		}

		// Token: 0x06001A1D RID: 6685 RVA: 0x0005AC2D File Offset: 0x00059C2D
		public override void CopyFrom(AsnEncodedData asnEncodedData)
		{
			base.CopyFrom(asnEncodedData);
			this.m_decoded = false;
		}

		// Token: 0x06001A1E RID: 6686 RVA: 0x0005AC40 File Offset: 0x00059C40
		private void DecodeExtension()
		{
			uint num = 0U;
			SafeLocalAllocHandle safeLocalAllocHandle = null;
			if (base.Oid.Value == "2.5.29.10")
			{
				if (!CAPI.DecodeObject(new IntPtr(13L), this.m_rawData, out safeLocalAllocHandle, out num))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
				CAPIBase.CERT_BASIC_CONSTRAINTS_INFO cert_BASIC_CONSTRAINTS_INFO = (CAPIBase.CERT_BASIC_CONSTRAINTS_INFO)Marshal.PtrToStructure(safeLocalAllocHandle.DangerousGetHandle(), typeof(CAPIBase.CERT_BASIC_CONSTRAINTS_INFO));
				byte[] array = new byte[1];
				Marshal.Copy(cert_BASIC_CONSTRAINTS_INFO.SubjectType.pbData, array, 0, 1);
				this.m_isCA = (array[0] & 128) != 0;
				this.m_hasPathLenConstraint = cert_BASIC_CONSTRAINTS_INFO.fPathLenConstraint;
				this.m_pathLenConstraint = (int)cert_BASIC_CONSTRAINTS_INFO.dwPathLenConstraint;
			}
			else
			{
				if (!CAPI.DecodeObject(new IntPtr(15L), this.m_rawData, out safeLocalAllocHandle, out num))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
				CAPIBase.CERT_BASIC_CONSTRAINTS2_INFO cert_BASIC_CONSTRAINTS2_INFO = (CAPIBase.CERT_BASIC_CONSTRAINTS2_INFO)Marshal.PtrToStructure(safeLocalAllocHandle.DangerousGetHandle(), typeof(CAPIBase.CERT_BASIC_CONSTRAINTS2_INFO));
				this.m_isCA = cert_BASIC_CONSTRAINTS2_INFO.fCA != 0;
				this.m_hasPathLenConstraint = cert_BASIC_CONSTRAINTS2_INFO.fPathLenConstraint != 0;
				this.m_pathLenConstraint = (int)cert_BASIC_CONSTRAINTS2_INFO.dwPathLenConstraint;
			}
			this.m_decoded = true;
			safeLocalAllocHandle.Dispose();
		}

		// Token: 0x06001A1F RID: 6687 RVA: 0x0005AD80 File Offset: 0x00059D80
		private unsafe static byte[] EncodeExtension(bool certificateAuthority, bool hasPathLengthConstraint, int pathLengthConstraint)
		{
			CAPIBase.CERT_BASIC_CONSTRAINTS2_INFO cert_BASIC_CONSTRAINTS2_INFO = default(CAPIBase.CERT_BASIC_CONSTRAINTS2_INFO);
			cert_BASIC_CONSTRAINTS2_INFO.fCA = (certificateAuthority ? 1 : 0);
			cert_BASIC_CONSTRAINTS2_INFO.fPathLenConstraint = (hasPathLengthConstraint ? 1 : 0);
			if (hasPathLengthConstraint)
			{
				if (pathLengthConstraint < 0)
				{
					throw new ArgumentOutOfRangeException("pathLengthConstraint", SR.GetString("Arg_OutOfRange_NeedNonNegNum"));
				}
				cert_BASIC_CONSTRAINTS2_INFO.dwPathLenConstraint = (uint)pathLengthConstraint;
			}
			byte[] array = null;
			if (!CAPI.EncodeObject("2.5.29.19", new IntPtr((void*)(&cert_BASIC_CONSTRAINTS2_INFO)), out array))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			return array;
		}

		// Token: 0x04001B15 RID: 6933
		private bool m_isCA;

		// Token: 0x04001B16 RID: 6934
		private bool m_hasPathLenConstraint;

		// Token: 0x04001B17 RID: 6935
		private int m_pathLenConstraint;

		// Token: 0x04001B18 RID: 6936
		private bool m_decoded;
	}
}
