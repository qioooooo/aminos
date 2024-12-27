using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;

namespace System.Security.Cryptography.Pkcs
{
	// Token: 0x02000060 RID: 96
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class CmsRecipientCollection : ICollection, IEnumerable
	{
		// Token: 0x060000EA RID: 234 RVA: 0x000062AE File Offset: 0x000052AE
		public CmsRecipientCollection()
		{
			this.m_recipients = new ArrayList();
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000062C1 File Offset: 0x000052C1
		public CmsRecipientCollection(CmsRecipient recipient)
		{
			this.m_recipients = new ArrayList(1);
			this.m_recipients.Add(recipient);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000062E4 File Offset: 0x000052E4
		public CmsRecipientCollection(SubjectIdentifierType recipientIdentifierType, X509Certificate2Collection certificates)
		{
			this.m_recipients = new ArrayList(certificates.Count);
			for (int i = 0; i < certificates.Count; i++)
			{
				this.m_recipients.Add(new CmsRecipient(recipientIdentifierType, certificates[i]));
			}
		}

		// Token: 0x17000018 RID: 24
		public CmsRecipient this[int index]
		{
			get
			{
				if (index < 0 || index >= this.m_recipients.Count)
				{
					throw new ArgumentOutOfRangeException("index", SecurityResources.GetResourceString("ArgumentOutOfRange_Index"));
				}
				return (CmsRecipient)this.m_recipients[index];
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000EE RID: 238 RVA: 0x0000636C File Offset: 0x0000536C
		public int Count
		{
			get
			{
				return this.m_recipients.Count;
			}
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00006379 File Offset: 0x00005379
		public int Add(CmsRecipient recipient)
		{
			if (recipient == null)
			{
				throw new ArgumentNullException("recipient");
			}
			return this.m_recipients.Add(recipient);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00006395 File Offset: 0x00005395
		public void Remove(CmsRecipient recipient)
		{
			if (recipient == null)
			{
				throw new ArgumentNullException("recipient");
			}
			this.m_recipients.Remove(recipient);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x000063B1 File Offset: 0x000053B1
		public CmsRecipientEnumerator GetEnumerator()
		{
			return new CmsRecipientEnumerator(this);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000063B9 File Offset: 0x000053B9
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new CmsRecipientEnumerator(this);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x000063C4 File Offset: 0x000053C4
		public void CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(SecurityResources.GetResourceString("Arg_RankMultiDimNotSupported"));
			}
			if (index < 0 || index >= array.Length)
			{
				throw new ArgumentOutOfRangeException("index", SecurityResources.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (index + this.Count > array.Length)
			{
				throw new ArgumentException(SecurityResources.GetResourceString("Argument_InvalidOffLen"));
			}
			for (int i = 0; i < this.Count; i++)
			{
				array.SetValue(this[i], index);
				index++;
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000645E File Offset: 0x0000545E
		public void CopyTo(CmsRecipient[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00006468 File Offset: 0x00005468
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x0000646B File Offset: 0x0000546B
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x04000454 RID: 1108
		private ArrayList m_recipients;
	}
}
