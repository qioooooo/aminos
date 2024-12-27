using System;
using System.Globalization;

namespace System.EnterpriseServices.CompensatingResourceManager
{
	// Token: 0x020000AE RID: 174
	public class Compensator : ServicedComponent, _ICompensator, _IFormatLogRecords
	{
		// Token: 0x0600040F RID: 1039 RVA: 0x0000D100 File Offset: 0x0000C100
		void _ICompensator._SetLogControl(IntPtr logControl)
		{
			this._clerk = new Clerk(new CrmLogControl(logControl));
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0000D114 File Offset: 0x0000C114
		bool _ICompensator._PrepareRecord(_LogRecord record)
		{
			LogRecord logRecord = new LogRecord(record);
			return this.PrepareRecord(logRecord);
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0000D130 File Offset: 0x0000C130
		bool _ICompensator._CommitRecord(_LogRecord record)
		{
			LogRecord logRecord = new LogRecord(record);
			return this.CommitRecord(logRecord);
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0000D14C File Offset: 0x0000C14C
		bool _ICompensator._AbortRecord(_LogRecord record)
		{
			LogRecord logRecord = new LogRecord(record);
			return this.AbortRecord(logRecord);
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0000D167 File Offset: 0x0000C167
		void _ICompensator._BeginPrepare()
		{
			this.BeginPrepare();
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0000D16F File Offset: 0x0000C16F
		bool _ICompensator._EndPrepare()
		{
			return this.EndPrepare();
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0000D177 File Offset: 0x0000C177
		void _ICompensator._BeginCommit(bool fRecovery)
		{
			this.BeginCommit(fRecovery);
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0000D180 File Offset: 0x0000C180
		void _ICompensator._EndCommit()
		{
			this.EndCommit();
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0000D188 File Offset: 0x0000C188
		void _ICompensator._BeginAbort(bool fRecovery)
		{
			this.BeginAbort(fRecovery);
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0000D191 File Offset: 0x0000C191
		void _ICompensator._EndAbort()
		{
			this.EndAbort();
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0000D199 File Offset: 0x0000C199
		public Compensator()
		{
			this._clerk = null;
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0000D1A8 File Offset: 0x0000C1A8
		public virtual void BeginPrepare()
		{
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0000D1AA File Offset: 0x0000C1AA
		public virtual bool PrepareRecord(LogRecord rec)
		{
			return false;
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0000D1AD File Offset: 0x0000C1AD
		public virtual bool EndPrepare()
		{
			return true;
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0000D1B0 File Offset: 0x0000C1B0
		public virtual void BeginCommit(bool fRecovery)
		{
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0000D1B2 File Offset: 0x0000C1B2
		public virtual bool CommitRecord(LogRecord rec)
		{
			return false;
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0000D1B5 File Offset: 0x0000C1B5
		public virtual void EndCommit()
		{
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0000D1B7 File Offset: 0x0000C1B7
		public virtual void BeginAbort(bool fRecovery)
		{
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0000D1B9 File Offset: 0x0000C1B9
		public virtual bool AbortRecord(LogRecord rec)
		{
			return false;
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0000D1BC File Offset: 0x0000C1BC
		public virtual void EndAbort()
		{
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x0000D1BE File Offset: 0x0000C1BE
		public Clerk Clerk
		{
			get
			{
				return this._clerk;
			}
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0000D1C6 File Offset: 0x0000C1C6
		int _IFormatLogRecords.GetColumnCount()
		{
			if (this is IFormatLogRecords)
			{
				return ((IFormatLogRecords)this).ColumnCount;
			}
			return 3;
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0000D1E0 File Offset: 0x0000C1E0
		object _IFormatLogRecords.GetColumnHeaders()
		{
			if (this is IFormatLogRecords)
			{
				return ((IFormatLogRecords)this).ColumnHeaders;
			}
			return new string[]
			{
				Resource.FormatString("CRM_HeaderFlags"),
				Resource.FormatString("CRM_HeaderRecord"),
				Resource.FormatString("CRM_HeaderString")
			};
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0000D230 File Offset: 0x0000C230
		object _IFormatLogRecords.GetColumn(_LogRecord r)
		{
			LogRecord logRecord = new LogRecord(r);
			if (this is IFormatLogRecords)
			{
				return ((IFormatLogRecords)this).Format(logRecord);
			}
			return new string[]
			{
				logRecord.Flags.ToString(),
				logRecord.Sequence.ToString(CultureInfo.CurrentUICulture),
				logRecord.Record.ToString()
			};
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0000D298 File Offset: 0x0000C298
		object _IFormatLogRecords.GetColumnVariants(object logRecord)
		{
			throw new NotSupportedException();
		}

		// Token: 0x040001E1 RID: 481
		private Clerk _clerk;
	}
}
