using System;
using System.Security.Permissions;

namespace System.Security.Cryptography.Pkcs
{
	// Token: 0x0200006B RID: 107
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public abstract class RecipientInfo
	{
		// Token: 0x06000133 RID: 307 RVA: 0x00006B4E File Offset: 0x00005B4E
		internal RecipientInfo()
		{
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00006B58 File Offset: 0x00005B58
		internal RecipientInfo(RecipientInfoType recipientInfoType, RecipientSubType recipientSubType, SafeLocalAllocHandle pCmsgRecipientInfo, object cmsgRecipientInfo, uint index)
		{
			if (recipientInfoType < RecipientInfoType.Unknown || recipientInfoType > RecipientInfoType.KeyAgreement)
			{
				recipientInfoType = RecipientInfoType.Unknown;
			}
			if (recipientSubType < RecipientSubType.Unknown || recipientSubType > RecipientSubType.PublicKeyAgreement)
			{
				recipientSubType = RecipientSubType.Unknown;
			}
			this.m_recipentInfoType = recipientInfoType;
			this.m_recipientSubType = recipientSubType;
			this.m_pCmsgRecipientInfo = pCmsgRecipientInfo;
			this.m_cmsgRecipientInfo = cmsgRecipientInfo;
			this.m_index = index;
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00006BA6 File Offset: 0x00005BA6
		public RecipientInfoType Type
		{
			get
			{
				return this.m_recipentInfoType;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000136 RID: 310
		public abstract int Version { get; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000137 RID: 311
		public abstract SubjectIdentifier RecipientIdentifier { get; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000138 RID: 312
		public abstract AlgorithmIdentifier KeyEncryptionAlgorithm { get; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000139 RID: 313
		public abstract byte[] EncryptedKey { get; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00006BAE File Offset: 0x00005BAE
		internal RecipientSubType SubType
		{
			get
			{
				return this.m_recipientSubType;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600013B RID: 315 RVA: 0x00006BB6 File Offset: 0x00005BB6
		internal SafeLocalAllocHandle pCmsgRecipientInfo
		{
			get
			{
				return this.m_pCmsgRecipientInfo;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600013C RID: 316 RVA: 0x00006BBE File Offset: 0x00005BBE
		internal object CmsgRecipientInfo
		{
			get
			{
				return this.m_cmsgRecipientInfo;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600013D RID: 317 RVA: 0x00006BC6 File Offset: 0x00005BC6
		internal uint Index
		{
			get
			{
				return this.m_index;
			}
		}

		// Token: 0x04000473 RID: 1139
		private RecipientInfoType m_recipentInfoType;

		// Token: 0x04000474 RID: 1140
		private RecipientSubType m_recipientSubType;

		// Token: 0x04000475 RID: 1141
		private SafeLocalAllocHandle m_pCmsgRecipientInfo;

		// Token: 0x04000476 RID: 1142
		private object m_cmsgRecipientInfo;

		// Token: 0x04000477 RID: 1143
		private uint m_index;
	}
}
