using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000323 RID: 803
	public sealed class X500DistinguishedName : AsnEncodedData
	{
		// Token: 0x06001931 RID: 6449 RVA: 0x00055B17 File Offset: 0x00054B17
		internal X500DistinguishedName(CAPIBase.CRYPTOAPI_BLOB encodedDistinguishedNameBlob)
			: base(new Oid(), encodedDistinguishedNameBlob)
		{
		}

		// Token: 0x06001932 RID: 6450 RVA: 0x00055B25 File Offset: 0x00054B25
		public X500DistinguishedName(byte[] encodedDistinguishedName)
			: base(new Oid(), encodedDistinguishedName)
		{
		}

		// Token: 0x06001933 RID: 6451 RVA: 0x00055B33 File Offset: 0x00054B33
		public X500DistinguishedName(AsnEncodedData encodedDistinguishedName)
			: base(encodedDistinguishedName)
		{
		}

		// Token: 0x06001934 RID: 6452 RVA: 0x00055B3C File Offset: 0x00054B3C
		public X500DistinguishedName(X500DistinguishedName distinguishedName)
			: base(distinguishedName)
		{
			this.m_distinguishedName = distinguishedName.Name;
		}

		// Token: 0x06001935 RID: 6453 RVA: 0x00055B51 File Offset: 0x00054B51
		public X500DistinguishedName(string distinguishedName)
			: this(distinguishedName, X500DistinguishedNameFlags.Reversed)
		{
		}

		// Token: 0x06001936 RID: 6454 RVA: 0x00055B5B File Offset: 0x00054B5B
		public X500DistinguishedName(string distinguishedName, X500DistinguishedNameFlags flag)
			: base(new Oid(), X500DistinguishedName.Encode(distinguishedName, flag))
		{
			this.m_distinguishedName = distinguishedName;
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06001937 RID: 6455 RVA: 0x00055B76 File Offset: 0x00054B76
		public string Name
		{
			get
			{
				if (this.m_distinguishedName == null)
				{
					this.m_distinguishedName = this.Decode(X500DistinguishedNameFlags.Reversed);
				}
				return this.m_distinguishedName;
			}
		}

		// Token: 0x06001938 RID: 6456 RVA: 0x00055B94 File Offset: 0x00054B94
		public unsafe string Decode(X500DistinguishedNameFlags flag)
		{
			uint num = 3U | X500DistinguishedName.MapNameToStrFlag(flag);
			byte[] rawData = this.m_rawData;
			fixed (byte* ptr = rawData)
			{
				CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB;
				IntPtr intPtr = new IntPtr((void*)(&cryptoapi_BLOB));
				cryptoapi_BLOB.cbData = (uint)rawData.Length;
				cryptoapi_BLOB.pbData = new IntPtr((void*)ptr);
				uint num2 = CAPISafe.CertNameToStrW(65537U, intPtr, num, SafeLocalAllocHandle.InvalidHandle, 0U);
				if (num2 == 0U)
				{
					throw new CryptographicException(-2146762476);
				}
				string text;
				using (SafeLocalAllocHandle safeLocalAllocHandle = CAPI.LocalAlloc(64U, new IntPtr((long)((ulong)(2U * num2)))))
				{
					if (CAPISafe.CertNameToStrW(65537U, intPtr, num, safeLocalAllocHandle, num2) == 0U)
					{
						throw new CryptographicException(-2146762476);
					}
					text = Marshal.PtrToStringUni(safeLocalAllocHandle.DangerousGetHandle());
				}
				return text;
			}
		}

		// Token: 0x06001939 RID: 6457 RVA: 0x00055C70 File Offset: 0x00054C70
		public override string Format(bool multiLine)
		{
			if (this.m_rawData == null || this.m_rawData.Length == 0)
			{
				return string.Empty;
			}
			return CAPI.CryptFormatObject(1U, multiLine ? 1U : 0U, new IntPtr(7L), this.m_rawData);
		}

		// Token: 0x0600193A RID: 6458 RVA: 0x00055CA4 File Offset: 0x00054CA4
		private unsafe static byte[] Encode(string distinguishedName, X500DistinguishedNameFlags flag)
		{
			if (distinguishedName == null)
			{
				throw new ArgumentNullException("distinguishedName");
			}
			uint num = 0U;
			uint num2 = 3U | X500DistinguishedName.MapNameToStrFlag(flag);
			if (!CAPISafe.CertStrToNameW(65537U, distinguishedName, num2, IntPtr.Zero, IntPtr.Zero, ref num, IntPtr.Zero))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			byte[] array = new byte[num];
			fixed (byte* ptr = array)
			{
				if (!CAPISafe.CertStrToNameW(65537U, distinguishedName, num2, IntPtr.Zero, new IntPtr((void*)ptr), ref num, IntPtr.Zero))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
			}
			return array;
		}

		// Token: 0x0600193B RID: 6459 RVA: 0x00055D48 File Offset: 0x00054D48
		private static uint MapNameToStrFlag(X500DistinguishedNameFlags flag)
		{
			uint num = 29169U;
			if ((flag & (X500DistinguishedNameFlags)(~(X500DistinguishedNameFlags)num)) != X500DistinguishedNameFlags.None)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Arg_EnumIllegalVal"), new object[] { "flag" }));
			}
			uint num2 = 0U;
			if (flag != X500DistinguishedNameFlags.None)
			{
				if ((flag & X500DistinguishedNameFlags.Reversed) == X500DistinguishedNameFlags.Reversed)
				{
					num2 |= 33554432U;
				}
				if ((flag & X500DistinguishedNameFlags.UseSemicolons) == X500DistinguishedNameFlags.UseSemicolons)
				{
					num2 |= 1073741824U;
				}
				else if ((flag & X500DistinguishedNameFlags.UseCommas) == X500DistinguishedNameFlags.UseCommas)
				{
					num2 |= 67108864U;
				}
				else if ((flag & X500DistinguishedNameFlags.UseNewLines) == X500DistinguishedNameFlags.UseNewLines)
				{
					num2 |= 134217728U;
				}
				if ((flag & X500DistinguishedNameFlags.DoNotUsePlusSign) == X500DistinguishedNameFlags.DoNotUsePlusSign)
				{
					num2 |= 536870912U;
				}
				if ((flag & X500DistinguishedNameFlags.DoNotUseQuotes) == X500DistinguishedNameFlags.DoNotUseQuotes)
				{
					num2 |= 268435456U;
				}
				if ((flag & X500DistinguishedNameFlags.ForceUTF8Encoding) == X500DistinguishedNameFlags.ForceUTF8Encoding)
				{
					num2 |= 524288U;
				}
				if ((flag & X500DistinguishedNameFlags.UseUTF8Encoding) == X500DistinguishedNameFlags.UseUTF8Encoding)
				{
					num2 |= 262144U;
				}
				else if ((flag & X500DistinguishedNameFlags.UseT61Encoding) == X500DistinguishedNameFlags.UseT61Encoding)
				{
					num2 |= 131072U;
				}
			}
			return num2;
		}

		// Token: 0x04001A8A RID: 6794
		private string m_distinguishedName;
	}
}
