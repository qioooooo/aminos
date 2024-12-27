using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000851 RID: 2129
	[ComVisible(true)]
	public sealed class RNGCryptoServiceProvider : RandomNumberGenerator
	{
		// Token: 0x06004E2E RID: 20014 RVA: 0x0010FC45 File Offset: 0x0010EC45
		public RNGCryptoServiceProvider()
			: this(null)
		{
		}

		// Token: 0x06004E2F RID: 20015 RVA: 0x0010FC4E File Offset: 0x0010EC4E
		public RNGCryptoServiceProvider(string str)
			: this(null)
		{
		}

		// Token: 0x06004E30 RID: 20016 RVA: 0x0010FC57 File Offset: 0x0010EC57
		public RNGCryptoServiceProvider(byte[] rgb)
			: this(null)
		{
		}

		// Token: 0x06004E31 RID: 20017 RVA: 0x0010FC60 File Offset: 0x0010EC60
		public RNGCryptoServiceProvider(CspParameters cspParams)
		{
			if (cspParams != null)
			{
				this.m_safeProvHandle = Utils.AcquireProvHandle(cspParams);
				return;
			}
			this.m_safeProvHandle = Utils.StaticProvHandle;
		}

		// Token: 0x06004E32 RID: 20018 RVA: 0x0010FC83 File Offset: 0x0010EC83
		public override void GetBytes(byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			Utils._GetBytes(this.m_safeProvHandle, data);
		}

		// Token: 0x06004E33 RID: 20019 RVA: 0x0010FC9F File Offset: 0x0010EC9F
		public override void GetNonZeroBytes(byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			Utils._GetNonZeroBytes(this.m_safeProvHandle, data);
		}

		// Token: 0x04002845 RID: 10309
		private SafeProvHandle m_safeProvHandle;
	}
}
