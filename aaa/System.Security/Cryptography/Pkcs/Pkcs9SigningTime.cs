using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Cryptography.Pkcs
{
	// Token: 0x02000064 RID: 100
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class Pkcs9SigningTime : Pkcs9AttributeObject
	{
		// Token: 0x06000114 RID: 276 RVA: 0x000067C4 File Offset: 0x000057C4
		public Pkcs9SigningTime()
			: this(DateTime.Now)
		{
		}

		// Token: 0x06000115 RID: 277 RVA: 0x000067D1 File Offset: 0x000057D1
		public Pkcs9SigningTime(DateTime signingTime)
			: base("1.2.840.113549.1.9.5", Pkcs9SigningTime.Encode(signingTime))
		{
			this.m_signingTime = signingTime;
			this.m_decoded = true;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x000067F2 File Offset: 0x000057F2
		public Pkcs9SigningTime(byte[] encodedSigningTime)
			: base("1.2.840.113549.1.9.5", encodedSigningTime)
		{
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00006800 File Offset: 0x00005800
		public DateTime SigningTime
		{
			get
			{
				if (!this.m_decoded && base.RawData != null)
				{
					this.Decode();
				}
				return this.m_signingTime;
			}
		}

		// Token: 0x06000118 RID: 280 RVA: 0x0000681E File Offset: 0x0000581E
		public override void CopyFrom(AsnEncodedData asnEncodedData)
		{
			base.CopyFrom(asnEncodedData);
			this.m_decoded = false;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00006830 File Offset: 0x00005830
		private void Decode()
		{
			uint num = 0U;
			SafeLocalAllocHandle safeLocalAllocHandle = null;
			if (!CAPI.DecodeObject(new IntPtr(17L), base.RawData, out safeLocalAllocHandle, out num))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			long num2 = Marshal.ReadInt64(safeLocalAllocHandle.DangerousGetHandle());
			safeLocalAllocHandle.Dispose();
			this.m_signingTime = DateTime.FromFileTimeUtc(num2);
			this.m_decoded = true;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0000688C File Offset: 0x0000588C
		private static byte[] Encode(DateTime signingTime)
		{
			long num = signingTime.ToFileTimeUtc();
			SafeLocalAllocHandle safeLocalAllocHandle = CAPI.LocalAlloc(64U, new IntPtr(Marshal.SizeOf(typeof(long))));
			Marshal.WriteInt64(safeLocalAllocHandle.DangerousGetHandle(), num);
			byte[] array = new byte[0];
			if (!CAPI.EncodeObject("1.2.840.113549.1.9.5", safeLocalAllocHandle.DangerousGetHandle(), out array))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			safeLocalAllocHandle.Dispose();
			return array;
		}

		// Token: 0x0400045F RID: 1119
		private DateTime m_signingTime;

		// Token: 0x04000460 RID: 1120
		private bool m_decoded;
	}
}
