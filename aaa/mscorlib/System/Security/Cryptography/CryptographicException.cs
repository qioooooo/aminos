using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Microsoft.Win32;

namespace System.Security.Cryptography
{
	// Token: 0x0200084D RID: 2125
	[ComVisible(true)]
	[Serializable]
	public class CryptographicException : SystemException
	{
		// Token: 0x06004E17 RID: 19991 RVA: 0x0010FAE4 File Offset: 0x0010EAE4
		public CryptographicException()
			: base(Environment.GetResourceString("Arg_CryptographyException"))
		{
			base.SetErrorCode(-2146233296);
		}

		// Token: 0x06004E18 RID: 19992 RVA: 0x0010FB01 File Offset: 0x0010EB01
		public CryptographicException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233296);
		}

		// Token: 0x06004E19 RID: 19993 RVA: 0x0010FB18 File Offset: 0x0010EB18
		public CryptographicException(string format, string insert)
			: base(string.Format(CultureInfo.CurrentCulture, format, new object[] { insert }))
		{
			base.SetErrorCode(-2146233296);
		}

		// Token: 0x06004E1A RID: 19994 RVA: 0x0010FB4D File Offset: 0x0010EB4D
		public CryptographicException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2146233296);
		}

		// Token: 0x06004E1B RID: 19995 RVA: 0x0010FB62 File Offset: 0x0010EB62
		public CryptographicException(int hr)
			: this(Win32Native.GetMessage(hr))
		{
			if (((long)hr & (long)((ulong)(-2147483648))) != (long)((ulong)(-2147483648)))
			{
				hr = (hr & 65535) | -2147024896;
			}
			base.SetErrorCode(hr);
		}

		// Token: 0x06004E1C RID: 19996 RVA: 0x0010FB97 File Offset: 0x0010EB97
		protected CryptographicException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x06004E1D RID: 19997 RVA: 0x0010FBA1 File Offset: 0x0010EBA1
		private static void ThrowCryptogaphicException(int hr)
		{
			throw new CryptographicException(hr);
		}

		// Token: 0x04002842 RID: 10306
		private const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;

		// Token: 0x04002843 RID: 10307
		private const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;

		// Token: 0x04002844 RID: 10308
		private const int FORMAT_MESSAGE_ARGUMENT_ARRAY = 8192;
	}
}
