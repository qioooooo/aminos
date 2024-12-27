using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Cryptography.Pkcs
{
	// Token: 0x02000074 RID: 116
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class PublicKeyInfo
	{
		// Token: 0x0600016D RID: 365 RVA: 0x000079BD File Offset: 0x000069BD
		private PublicKeyInfo()
		{
		}

		// Token: 0x0600016E RID: 366 RVA: 0x000079C8 File Offset: 0x000069C8
		internal PublicKeyInfo(CAPIBase.CERT_PUBLIC_KEY_INFO keyInfo)
		{
			this.m_algorithm = new AlgorithmIdentifier(keyInfo);
			this.m_keyValue = new byte[keyInfo.PublicKey.cbData];
			if (this.m_keyValue.Length > 0)
			{
				Marshal.Copy(keyInfo.PublicKey.pbData, this.m_keyValue, 0, this.m_keyValue.Length);
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00007A2A File Offset: 0x00006A2A
		public AlgorithmIdentifier Algorithm
		{
			get
			{
				return this.m_algorithm;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000170 RID: 368 RVA: 0x00007A32 File Offset: 0x00006A32
		public byte[] KeyValue
		{
			get
			{
				return this.m_keyValue;
			}
		}

		// Token: 0x0400049B RID: 1179
		private AlgorithmIdentifier m_algorithm;

		// Token: 0x0400049C RID: 1180
		private byte[] m_keyValue;
	}
}
