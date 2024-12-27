using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Cryptography.Pkcs
{
	// Token: 0x0200006D RID: 109
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class KeyAgreeRecipientInfo : RecipientInfo
	{
		// Token: 0x06000146 RID: 326 RVA: 0x00006DBC File Offset: 0x00005DBC
		private KeyAgreeRecipientInfo()
		{
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00006DC4 File Offset: 0x00005DC4
		internal KeyAgreeRecipientInfo(SafeLocalAllocHandle pRecipientInfo, CAPIBase.CMSG_KEY_AGREE_CERT_ID_RECIPIENT_INFO certIdRecipient, uint index, uint subIndex)
			: base(RecipientInfoType.KeyAgreement, RecipientSubType.CertIdKeyAgreement, pRecipientInfo, certIdRecipient, index)
		{
			IntPtr intPtr = Marshal.ReadIntPtr(new IntPtr((long)certIdRecipient.rgpRecipientEncryptedKeys + (long)((ulong)subIndex * (ulong)((long)Marshal.SizeOf(typeof(IntPtr))))));
			CAPIBase.CMSG_RECIPIENT_ENCRYPTED_KEY_INFO cmsg_RECIPIENT_ENCRYPTED_KEY_INFO = (CAPIBase.CMSG_RECIPIENT_ENCRYPTED_KEY_INFO)Marshal.PtrToStructure(intPtr, typeof(CAPIBase.CMSG_RECIPIENT_ENCRYPTED_KEY_INFO));
			this.Reset(1U, certIdRecipient.dwVersion, cmsg_RECIPIENT_ENCRYPTED_KEY_INFO, subIndex);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00006E34 File Offset: 0x00005E34
		internal KeyAgreeRecipientInfo(SafeLocalAllocHandle pRecipientInfo, CAPIBase.CMSG_KEY_AGREE_PUBLIC_KEY_RECIPIENT_INFO publicKeyRecipient, uint index, uint subIndex)
			: base(RecipientInfoType.KeyAgreement, RecipientSubType.PublicKeyAgreement, pRecipientInfo, publicKeyRecipient, index)
		{
			IntPtr intPtr = Marshal.ReadIntPtr(new IntPtr((long)publicKeyRecipient.rgpRecipientEncryptedKeys + (long)((ulong)subIndex * (ulong)((long)Marshal.SizeOf(typeof(IntPtr))))));
			CAPIBase.CMSG_RECIPIENT_ENCRYPTED_KEY_INFO cmsg_RECIPIENT_ENCRYPTED_KEY_INFO = (CAPIBase.CMSG_RECIPIENT_ENCRYPTED_KEY_INFO)Marshal.PtrToStructure(intPtr, typeof(CAPIBase.CMSG_RECIPIENT_ENCRYPTED_KEY_INFO));
			this.Reset(2U, publicKeyRecipient.dwVersion, cmsg_RECIPIENT_ENCRYPTED_KEY_INFO, subIndex);
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000149 RID: 329 RVA: 0x00006EA4 File Offset: 0x00005EA4
		public override int Version
		{
			get
			{
				return this.m_version;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600014A RID: 330 RVA: 0x00006EAC File Offset: 0x00005EAC
		public SubjectIdentifierOrKey OriginatorIdentifierOrKey
		{
			get
			{
				if (this.m_originatorIdentifier == null)
				{
					if (this.m_originatorChoice == 1U)
					{
						this.m_originatorIdentifier = new SubjectIdentifierOrKey(((CAPIBase.CMSG_KEY_AGREE_CERT_ID_RECIPIENT_INFO)base.CmsgRecipientInfo).OriginatorCertId);
					}
					else
					{
						this.m_originatorIdentifier = new SubjectIdentifierOrKey(((CAPIBase.CMSG_KEY_AGREE_PUBLIC_KEY_RECIPIENT_INFO)base.CmsgRecipientInfo).OriginatorPublicKeyInfo);
					}
				}
				return this.m_originatorIdentifier;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600014B RID: 331 RVA: 0x00006F0E File Offset: 0x00005F0E
		public override SubjectIdentifier RecipientIdentifier
		{
			get
			{
				if (this.m_recipientIdentifier == null)
				{
					this.m_recipientIdentifier = new SubjectIdentifier(this.m_encryptedKeyInfo.RecipientId);
				}
				return this.m_recipientIdentifier;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00006F34 File Offset: 0x00005F34
		public DateTime Date
		{
			get
			{
				if (this.m_date == DateTime.MinValue)
				{
					if (this.RecipientIdentifier.Type != SubjectIdentifierType.SubjectKeyIdentifier)
					{
						throw new InvalidOperationException(SecurityResources.GetResourceString("Cryptography_Cms_Key_Agree_Date_Not_Available"));
					}
					long num = (long)(((ulong)this.m_encryptedKeyInfo.Date.dwHighDateTime << 32) | (ulong)this.m_encryptedKeyInfo.Date.dwLowDateTime);
					this.m_date = DateTime.FromFileTimeUtc(num);
				}
				return this.m_date;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00006FAC File Offset: 0x00005FAC
		public CryptographicAttributeObject OtherKeyAttribute
		{
			get
			{
				if (this.m_otherKeyAttribute == null)
				{
					if (this.RecipientIdentifier.Type != SubjectIdentifierType.SubjectKeyIdentifier)
					{
						throw new InvalidOperationException(SecurityResources.GetResourceString("Cryptography_Cms_Key_Agree_Other_Key_Attribute_Not_Available"));
					}
					if (this.m_encryptedKeyInfo.pOtherAttr != IntPtr.Zero)
					{
						CAPIBase.CRYPT_ATTRIBUTE_TYPE_VALUE crypt_ATTRIBUTE_TYPE_VALUE = (CAPIBase.CRYPT_ATTRIBUTE_TYPE_VALUE)Marshal.PtrToStructure(this.m_encryptedKeyInfo.pOtherAttr, typeof(CAPIBase.CRYPT_ATTRIBUTE_TYPE_VALUE));
						this.m_otherKeyAttribute = new CryptographicAttributeObject(crypt_ATTRIBUTE_TYPE_VALUE);
					}
				}
				return this.m_otherKeyAttribute;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00007028 File Offset: 0x00006028
		public override AlgorithmIdentifier KeyEncryptionAlgorithm
		{
			get
			{
				if (this.m_encryptionAlgorithm == null)
				{
					if (this.m_originatorChoice == 1U)
					{
						this.m_encryptionAlgorithm = new AlgorithmIdentifier(((CAPIBase.CMSG_KEY_AGREE_CERT_ID_RECIPIENT_INFO)base.CmsgRecipientInfo).KeyEncryptionAlgorithm);
					}
					else
					{
						this.m_encryptionAlgorithm = new AlgorithmIdentifier(((CAPIBase.CMSG_KEY_AGREE_PUBLIC_KEY_RECIPIENT_INFO)base.CmsgRecipientInfo).KeyEncryptionAlgorithm);
					}
				}
				return this.m_encryptionAlgorithm;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600014F RID: 335 RVA: 0x0000708C File Offset: 0x0000608C
		public override byte[] EncryptedKey
		{
			get
			{
				if (this.m_encryptedKey.Length == 0 && this.m_encryptedKeyInfo.EncryptedKey.cbData > 0U)
				{
					this.m_encryptedKey = new byte[this.m_encryptedKeyInfo.EncryptedKey.cbData];
					Marshal.Copy(this.m_encryptedKeyInfo.EncryptedKey.pbData, this.m_encryptedKey, 0, this.m_encryptedKey.Length);
				}
				return this.m_encryptedKey;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000150 RID: 336 RVA: 0x000070FC File Offset: 0x000060FC
		internal CAPIBase.CERT_ID RecipientId
		{
			get
			{
				return this.m_encryptedKeyInfo.RecipientId;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00007109 File Offset: 0x00006109
		internal uint SubIndex
		{
			get
			{
				return this.m_subIndex;
			}
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00007114 File Offset: 0x00006114
		private void Reset(uint originatorChoice, uint version, CAPIBase.CMSG_RECIPIENT_ENCRYPTED_KEY_INFO encryptedKeyInfo, uint subIndex)
		{
			this.m_encryptedKeyInfo = encryptedKeyInfo;
			this.m_originatorChoice = originatorChoice;
			this.m_version = (int)version;
			this.m_originatorIdentifier = null;
			this.m_userKeyMaterial = new byte[0];
			this.m_encryptionAlgorithm = null;
			this.m_recipientIdentifier = null;
			this.m_encryptedKey = new byte[0];
			this.m_date = DateTime.MinValue;
			this.m_otherKeyAttribute = null;
			this.m_subIndex = subIndex;
		}

		// Token: 0x0400047C RID: 1148
		private CAPIBase.CMSG_RECIPIENT_ENCRYPTED_KEY_INFO m_encryptedKeyInfo;

		// Token: 0x0400047D RID: 1149
		private uint m_originatorChoice;

		// Token: 0x0400047E RID: 1150
		private int m_version;

		// Token: 0x0400047F RID: 1151
		private SubjectIdentifierOrKey m_originatorIdentifier;

		// Token: 0x04000480 RID: 1152
		private byte[] m_userKeyMaterial;

		// Token: 0x04000481 RID: 1153
		private AlgorithmIdentifier m_encryptionAlgorithm;

		// Token: 0x04000482 RID: 1154
		private SubjectIdentifier m_recipientIdentifier;

		// Token: 0x04000483 RID: 1155
		private byte[] m_encryptedKey;

		// Token: 0x04000484 RID: 1156
		private DateTime m_date;

		// Token: 0x04000485 RID: 1157
		private CryptographicAttributeObject m_otherKeyAttribute;

		// Token: 0x04000486 RID: 1158
		private uint m_subIndex;
	}
}
