using System;

namespace System.Transactions
{
	// Token: 0x0200006E RID: 110
	public struct TransactionOptions
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600030F RID: 783 RVA: 0x000325F0 File Offset: 0x000319F0
		// (set) Token: 0x06000310 RID: 784 RVA: 0x00032604 File Offset: 0x00031A04
		public TimeSpan Timeout
		{
			get
			{
				return this.timeout;
			}
			set
			{
				this.timeout = value;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000311 RID: 785 RVA: 0x00032618 File Offset: 0x00031A18
		// (set) Token: 0x06000312 RID: 786 RVA: 0x0003262C File Offset: 0x00031A2C
		public IsolationLevel IsolationLevel
		{
			get
			{
				return this.isolationLevel;
			}
			set
			{
				this.isolationLevel = value;
			}
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00032640 File Offset: 0x00031A40
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000314 RID: 788 RVA: 0x00032660 File Offset: 0x00031A60
		public override bool Equals(object obj)
		{
			if (!(obj is TransactionOptions))
			{
				return false;
			}
			TransactionOptions transactionOptions = (TransactionOptions)obj;
			return transactionOptions.timeout == this.timeout && transactionOptions.isolationLevel == this.isolationLevel;
		}

		// Token: 0x06000315 RID: 789 RVA: 0x000326A4 File Offset: 0x00031AA4
		public static bool operator ==(TransactionOptions x, TransactionOptions y)
		{
			return x.Equals(y);
		}

		// Token: 0x06000316 RID: 790 RVA: 0x000326C4 File Offset: 0x00031AC4
		public static bool operator !=(TransactionOptions x, TransactionOptions y)
		{
			return !x.Equals(y);
		}

		// Token: 0x04000143 RID: 323
		private TimeSpan timeout;

		// Token: 0x04000144 RID: 324
		private IsolationLevel isolationLevel;
	}
}
