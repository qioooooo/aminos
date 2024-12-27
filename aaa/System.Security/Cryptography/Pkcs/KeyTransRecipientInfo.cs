using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Cryptography.Pkcs
{
	// Token: 0x0200006C RID: 108
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class KeyTransRecipientInfo : RecipientInfo
	{
		// Token: 0x0600013E RID: 318 RVA: 0x00006BCE File Offset: 0x00005BCE
		private KeyTransRecipientInfo()
		{
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00006BD8 File Offset: 0x00005BD8
		internal unsafe KeyTransRecipientInfo(SafeLocalAllocHandle pRecipientInfo, CAPIBase.CERT_INFO certInfo, uint index)
			: base(RecipientInfoType.KeyTransport, RecipientSubType.Pkcs7KeyTransport, pRecipientInfo, certInfo, index)
		{
			int num = 2;
			byte* ptr = (byte*)(void*)certInfo.SerialNumber.pbData;
			int num2 = 0;
			while ((long)num2 < (long)((ulong)certInfo.SerialNumber.cbData))
			{
				if (*(ptr++) != 0)
				{
					num = 0;
					break;
				}
				num2++;
			}
			this.Reset(num);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00006C36 File Offset: 0x00005C36
		internal KeyTransRecipientInfo(SafeLocalAllocHandle pRecipientInfo, CAPIBase.CMSG_KEY_TRANS_RECIPIENT_INFO keyTrans, uint index)
			: base(RecipientInfoType.KeyTransport, RecipientSubType.CmsKeyTransport, pRecipientInfo, keyTrans, index)
		{
			this.Reset((int)keyTrans.dwVersion);
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000141 RID: 321 RVA: 0x00006C55 File Offset: 0x00005C55
		public override int Version
		{
			get
			{
				return this.m_version;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000142 RID: 322 RVA: 0x00006C60 File Offset: 0x00005C60
		public override SubjectIdentifier RecipientIdentifier
		{
			get
			{
				if (this.m_recipientIdentifier == null)
				{
					if (base.SubType == RecipientSubType.CmsKeyTransport)
					{
						this.m_recipientIdentifier = new SubjectIdentifier(((CAPIBase.CMSG_KEY_TRANS_RECIPIENT_INFO)base.CmsgRecipientInfo).RecipientId);
					}
					else
					{
						CAPIBase.CERT_INFO cert_INFO = (CAPIBase.CERT_INFO)base.CmsgRecipientInfo;
						this.m_recipientIdentifier = new SubjectIdentifier(cert_INFO);
					}
				}
				return this.m_recipientIdentifier;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00006CBC File Offset: 0x00005CBC
		public override AlgorithmIdentifier KeyEncryptionAlgorithm
		{
			get
			{
				if (this.m_encryptionAlgorithm == null)
				{
					if (base.SubType == RecipientSubType.CmsKeyTransport)
					{
						this.m_encryptionAlgorithm = new AlgorithmIdentifier(((CAPIBase.CMSG_KEY_TRANS_RECIPIENT_INFO)base.CmsgRecipientInfo).KeyEncryptionAlgorithm);
					}
					else
					{
						this.m_encryptionAlgorithm = new AlgorithmIdentifier(((CAPIBase.CERT_INFO)base.CmsgRecipientInfo).SignatureAlgorithm);
					}
				}
				return this.m_encryptionAlgorithm;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00006D20 File Offset: 0x00005D20
		public override byte[] EncryptedKey
		{
			get
			{
				if (this.m_encryptedKey.Length == 0 && base.SubType == RecipientSubType.CmsKeyTransport)
				{
					CAPIBase.CMSG_KEY_TRANS_RECIPIENT_INFO cmsg_KEY_TRANS_RECIPIENT_INFO = (CAPIBase.CMSG_KEY_TRANS_RECIPIENT_INFO)base.CmsgRecipientInfo;
					if (cmsg_KEY_TRANS_RECIPIENT_INFO.EncryptedKey.cbData > 0U)
					{
						this.m_encryptedKey = new byte[cmsg_KEY_TRANS_RECIPIENT_INFO.EncryptedKey.cbData];
						Marshal.Copy(cmsg_KEY_TRANS_RECIPIENT_INFO.EncryptedKey.pbData, this.m_encryptedKey, 0, this.m_encryptedKey.Length);
					}
				}
				return this.m_encryptedKey;
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00006D99 File Offset: 0x00005D99
		private void Reset(int version)
		{
			this.m_version = version;
			this.m_recipientIdentifier = null;
			this.m_encryptionAlgorithm = null;
			this.m_encryptedKey = new byte[0];
		}

		// Token: 0x04000478 RID: 1144
		private int m_version;

		// Token: 0x04000479 RID: 1145
		private SubjectIdentifier m_recipientIdentifier;

		// Token: 0x0400047A RID: 1146
		private AlgorithmIdentifier m_encryptionAlgorithm;

		// Token: 0x0400047B RID: 1147
		private byte[] m_encryptedKey;
	}
}
