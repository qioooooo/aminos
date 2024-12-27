using System;

namespace System.Windows.Forms
{
	// Token: 0x02000347 RID: 839
	public class DataGridViewDataErrorEventArgs : DataGridViewCellCancelEventArgs
	{
		// Token: 0x06003583 RID: 13699 RVA: 0x000C0AED File Offset: 0x000BFAED
		public DataGridViewDataErrorEventArgs(Exception exception, int columnIndex, int rowIndex, DataGridViewDataErrorContexts context)
			: base(columnIndex, rowIndex)
		{
			this.exception = exception;
			this.context = context;
		}

		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x06003584 RID: 13700 RVA: 0x000C0B06 File Offset: 0x000BFB06
		public DataGridViewDataErrorContexts Context
		{
			get
			{
				return this.context;
			}
		}

		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x06003585 RID: 13701 RVA: 0x000C0B0E File Offset: 0x000BFB0E
		public Exception Exception
		{
			get
			{
				return this.exception;
			}
		}

		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x06003586 RID: 13702 RVA: 0x000C0B16 File Offset: 0x000BFB16
		// (set) Token: 0x06003587 RID: 13703 RVA: 0x000C0B1E File Offset: 0x000BFB1E
		public bool ThrowException
		{
			get
			{
				return this.throwException;
			}
			set
			{
				if (value && this.exception == null)
				{
					throw new ArgumentException(SR.GetString("DataGridView_CannotThrowNullException"));
				}
				this.throwException = value;
			}
		}

		// Token: 0x04001B92 RID: 7058
		private Exception exception;

		// Token: 0x04001B93 RID: 7059
		private bool throwException;

		// Token: 0x04001B94 RID: 7060
		private DataGridViewDataErrorContexts context;
	}
}
