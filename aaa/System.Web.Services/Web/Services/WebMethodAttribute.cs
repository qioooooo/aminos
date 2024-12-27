using System;
using System.EnterpriseServices;

namespace System.Web.Services
{
	// Token: 0x02000011 RID: 17
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class WebMethodAttribute : Attribute
	{
		// Token: 0x06000024 RID: 36 RVA: 0x000023C2 File Offset: 0x000013C2
		public WebMethodAttribute()
		{
			this.enableSession = false;
			this.transactionOption = 0;
			this.cacheDuration = 0;
			this.bufferResponse = true;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000023E6 File Offset: 0x000013E6
		public WebMethodAttribute(bool enableSession)
			: this()
		{
			this.EnableSession = enableSession;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000023F5 File Offset: 0x000013F5
		public WebMethodAttribute(bool enableSession, TransactionOption transactionOption)
			: this()
		{
			this.EnableSession = enableSession;
			this.transactionOption = (int)transactionOption;
			this.transactionOptionSpecified = true;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002412 File Offset: 0x00001412
		public WebMethodAttribute(bool enableSession, TransactionOption transactionOption, int cacheDuration)
		{
			this.EnableSession = enableSession;
			this.transactionOption = (int)transactionOption;
			this.transactionOptionSpecified = true;
			this.CacheDuration = cacheDuration;
			this.BufferResponse = true;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000243D File Offset: 0x0000143D
		public WebMethodAttribute(bool enableSession, TransactionOption transactionOption, int cacheDuration, bool bufferResponse)
		{
			this.EnableSession = enableSession;
			this.transactionOption = (int)transactionOption;
			this.transactionOptionSpecified = true;
			this.CacheDuration = cacheDuration;
			this.BufferResponse = bufferResponse;
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000029 RID: 41 RVA: 0x00002469 File Offset: 0x00001469
		// (set) Token: 0x0600002A RID: 42 RVA: 0x0000247F File Offset: 0x0000147F
		public string Description
		{
			get
			{
				if (this.description != null)
				{
					return this.description;
				}
				return string.Empty;
			}
			set
			{
				this.description = value;
				this.descriptionSpecified = true;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600002B RID: 43 RVA: 0x0000248F File Offset: 0x0000148F
		internal bool DescriptionSpecified
		{
			get
			{
				return this.descriptionSpecified;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002497 File Offset: 0x00001497
		// (set) Token: 0x0600002D RID: 45 RVA: 0x0000249F File Offset: 0x0000149F
		public bool EnableSession
		{
			get
			{
				return this.enableSession;
			}
			set
			{
				this.enableSession = value;
				this.enableSessionSpecified = true;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002E RID: 46 RVA: 0x000024AF File Offset: 0x000014AF
		internal bool EnableSessionSpecified
		{
			get
			{
				return this.enableSessionSpecified;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002F RID: 47 RVA: 0x000024B7 File Offset: 0x000014B7
		// (set) Token: 0x06000030 RID: 48 RVA: 0x000024BF File Offset: 0x000014BF
		public int CacheDuration
		{
			get
			{
				return this.cacheDuration;
			}
			set
			{
				this.cacheDuration = value;
				this.cacheDurationSpecified = true;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000031 RID: 49 RVA: 0x000024CF File Offset: 0x000014CF
		internal bool CacheDurationSpecified
		{
			get
			{
				return this.cacheDurationSpecified;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000032 RID: 50 RVA: 0x000024D7 File Offset: 0x000014D7
		// (set) Token: 0x06000033 RID: 51 RVA: 0x000024DF File Offset: 0x000014DF
		public bool BufferResponse
		{
			get
			{
				return this.bufferResponse;
			}
			set
			{
				this.bufferResponse = value;
				this.bufferResponseSpecified = true;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000034 RID: 52 RVA: 0x000024EF File Offset: 0x000014EF
		internal bool BufferResponseSpecified
		{
			get
			{
				return this.bufferResponseSpecified;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000035 RID: 53 RVA: 0x000024F7 File Offset: 0x000014F7
		// (set) Token: 0x06000036 RID: 54 RVA: 0x000024FF File Offset: 0x000014FF
		public TransactionOption TransactionOption
		{
			get
			{
				return (TransactionOption)this.transactionOption;
			}
			set
			{
				this.transactionOption = (int)value;
				this.transactionOptionSpecified = true;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000037 RID: 55 RVA: 0x0000250F File Offset: 0x0000150F
		internal bool TransactionOptionSpecified
		{
			get
			{
				return this.transactionOptionSpecified;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002517 File Offset: 0x00001517
		internal bool TransactionEnabled
		{
			get
			{
				return this.transactionOption != 0;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00002525 File Offset: 0x00001525
		// (set) Token: 0x0600003A RID: 58 RVA: 0x0000253B File Offset: 0x0000153B
		public string MessageName
		{
			get
			{
				if (this.messageName != null)
				{
					return this.messageName;
				}
				return string.Empty;
			}
			set
			{
				this.messageName = value;
				this.messageNameSpecified = true;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600003B RID: 59 RVA: 0x0000254B File Offset: 0x0000154B
		internal bool MessageNameSpecified
		{
			get
			{
				return this.messageNameSpecified;
			}
		}

		// Token: 0x04000208 RID: 520
		private int transactionOption;

		// Token: 0x04000209 RID: 521
		private bool enableSession;

		// Token: 0x0400020A RID: 522
		private int cacheDuration;

		// Token: 0x0400020B RID: 523
		private bool bufferResponse;

		// Token: 0x0400020C RID: 524
		private string description;

		// Token: 0x0400020D RID: 525
		private string messageName;

		// Token: 0x0400020E RID: 526
		private bool transactionOptionSpecified;

		// Token: 0x0400020F RID: 527
		private bool enableSessionSpecified;

		// Token: 0x04000210 RID: 528
		private bool cacheDurationSpecified;

		// Token: 0x04000211 RID: 529
		private bool bufferResponseSpecified;

		// Token: 0x04000212 RID: 530
		private bool descriptionSpecified;

		// Token: 0x04000213 RID: 531
		private bool messageNameSpecified;
	}
}
