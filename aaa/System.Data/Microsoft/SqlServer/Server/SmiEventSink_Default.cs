using System;
using System.Data.SqlClient;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000036 RID: 54
	internal class SmiEventSink_Default : SmiEventSink
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x001CB028 File Offset: 0x001CA428
		private SqlErrorCollection Errors
		{
			get
			{
				if (this._errors == null)
				{
					this._errors = new SqlErrorCollection();
				}
				return this._errors;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x001CB050 File Offset: 0x001CA450
		internal bool HasMessages
		{
			get
			{
				SmiEventSink_Default smiEventSink_Default = (SmiEventSink_Default)this._parent;
				if (smiEventSink_Default != null)
				{
					return smiEventSink_Default.HasMessages;
				}
				return this._errors != null || null != this._warnings;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x001CB08C File Offset: 0x001CA48C
		internal virtual string ServerVersion
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060001FA RID: 506 RVA: 0x001CB09C File Offset: 0x001CA49C
		// (set) Token: 0x060001FB RID: 507 RVA: 0x001CB0B0 File Offset: 0x001CA4B0
		internal SmiEventSink Parent
		{
			get
			{
				return this._parent;
			}
			set
			{
				this._parent = value;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060001FC RID: 508 RVA: 0x001CB0C4 File Offset: 0x001CA4C4
		private SqlErrorCollection Warnings
		{
			get
			{
				if (this._warnings == null)
				{
					this._warnings = new SqlErrorCollection();
				}
				return this._warnings;
			}
		}

		// Token: 0x060001FD RID: 509 RVA: 0x001CB0EC File Offset: 0x001CA4EC
		protected virtual void DispatchMessages(bool ignoreNonFatalMessages)
		{
			SmiEventSink_Default smiEventSink_Default = (SmiEventSink_Default)this._parent;
			if (smiEventSink_Default != null)
			{
				smiEventSink_Default.DispatchMessages(ignoreNonFatalMessages);
				return;
			}
			SqlException ex = this.ProcessMessages(true, ignoreNonFatalMessages);
			if (ex != null)
			{
				throw ex;
			}
		}

		// Token: 0x060001FE RID: 510 RVA: 0x001CB120 File Offset: 0x001CA520
		protected SqlException ProcessMessages(bool ignoreWarnings, bool ignoreNonFatalMessages)
		{
			SqlException ex = null;
			SqlErrorCollection sqlErrorCollection = null;
			if (this._errors != null)
			{
				if (ignoreNonFatalMessages)
				{
					sqlErrorCollection = new SqlErrorCollection();
					foreach (object obj in this._errors)
					{
						SqlError sqlError = (SqlError)obj;
						if (sqlError.Class >= 20)
						{
							sqlErrorCollection.Add(sqlError);
						}
					}
					if (sqlErrorCollection.Count <= 0)
					{
						sqlErrorCollection = null;
					}
				}
				else
				{
					if (this._warnings != null)
					{
						foreach (object obj2 in this._warnings)
						{
							SqlError sqlError2 = (SqlError)obj2;
							this._errors.Add(sqlError2);
						}
					}
					sqlErrorCollection = this._errors;
				}
				this._errors = null;
				this._warnings = null;
			}
			else
			{
				if (!ignoreWarnings)
				{
					sqlErrorCollection = this._warnings;
				}
				this._warnings = null;
			}
			if (sqlErrorCollection != null)
			{
				ex = SqlException.CreateException(sqlErrorCollection, this.ServerVersion);
			}
			return ex;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x001CB25C File Offset: 0x001CA65C
		internal void ProcessMessagesAndThrow()
		{
			this.ProcessMessagesAndThrow(false);
		}

		// Token: 0x06000200 RID: 512 RVA: 0x001CB270 File Offset: 0x001CA670
		internal void ProcessMessagesAndThrow(bool ignoreNonFatalMessages)
		{
			if (this.HasMessages)
			{
				this.DispatchMessages(ignoreNonFatalMessages);
			}
		}

		// Token: 0x06000201 RID: 513 RVA: 0x001CB28C File Offset: 0x001CA68C
		internal SmiEventSink_Default()
		{
		}

		// Token: 0x06000202 RID: 514 RVA: 0x001CB2A0 File Offset: 0x001CA6A0
		internal SmiEventSink_Default(SmiEventSink parent)
		{
			this._parent = parent;
		}

		// Token: 0x06000203 RID: 515 RVA: 0x001CB2BC File Offset: 0x001CA6BC
		internal override void BatchCompleted()
		{
			if (this._parent == null)
			{
				throw SQL.UnexpectedSmiEvent(SmiEventSink_Default.UnexpectedEventType.BatchCompleted);
			}
			this._parent.BatchCompleted();
		}

		// Token: 0x06000204 RID: 516 RVA: 0x001CB2E4 File Offset: 0x001CA6E4
		internal override void ParametersAvailable(SmiParameterMetaData[] metaData, ITypedGettersV3 paramValues)
		{
			if (this._parent == null)
			{
				throw SQL.UnexpectedSmiEvent(SmiEventSink_Default.UnexpectedEventType.ParametersAvailable);
			}
			this._parent.ParametersAvailable(metaData, paramValues);
		}

		// Token: 0x06000205 RID: 517 RVA: 0x001CB310 File Offset: 0x001CA710
		internal override void ParameterAvailable(SmiParameterMetaData metaData, SmiTypedGetterSetter paramValue, int ordinal)
		{
			if (this._parent == null)
			{
				throw SQL.UnexpectedSmiEvent(SmiEventSink_Default.UnexpectedEventType.ParameterAvailable);
			}
			this._parent.ParameterAvailable(metaData, paramValue, ordinal);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x001CB33C File Offset: 0x001CA73C
		internal override void DefaultDatabaseChanged(string databaseName)
		{
			if (this._parent == null)
			{
				throw SQL.UnexpectedSmiEvent(SmiEventSink_Default.UnexpectedEventType.DefaultDatabaseChanged);
			}
			this._parent.DefaultDatabaseChanged(databaseName);
		}

		// Token: 0x06000207 RID: 519 RVA: 0x001CB364 File Offset: 0x001CA764
		internal override void MessagePosted(int number, byte state, byte errorClass, string server, string message, string procedure, int lineNumber)
		{
			if (this._parent != null)
			{
				this._parent.MessagePosted(number, state, errorClass, server, message, procedure, lineNumber);
				return;
			}
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<sc.SmiEventSink_Default.MessagePosted|ADV> %d#, number=%d state=%d errorClass=%d server='%ls' message='%ls' procedure='%ls' linenumber=%d.\n", 0, number, (int)state, (int)errorClass, (server != null) ? server : "<null>", (message != null) ? message : "<null>", (procedure != null) ? procedure : "<null>", lineNumber);
			}
			SqlError sqlError = new SqlError(number, state, errorClass, server, message, procedure, lineNumber);
			if (sqlError.Class < 11)
			{
				this.Warnings.Add(sqlError);
				return;
			}
			this.Errors.Add(sqlError);
		}

		// Token: 0x06000208 RID: 520 RVA: 0x001CB404 File Offset: 0x001CA804
		internal override void MetaDataAvailable(SmiQueryMetaData[] metaData, bool nextEventIsRow)
		{
			if (this._parent == null)
			{
				throw SQL.UnexpectedSmiEvent(SmiEventSink_Default.UnexpectedEventType.MetaDataAvailable);
			}
			this._parent.MetaDataAvailable(metaData, nextEventIsRow);
		}

		// Token: 0x06000209 RID: 521 RVA: 0x001CB430 File Offset: 0x001CA830
		internal override void RowAvailable(ITypedGetters rowData)
		{
			if (this._parent == null)
			{
				throw SQL.UnexpectedSmiEvent(SmiEventSink_Default.UnexpectedEventType.RowAvailable);
			}
			this._parent.RowAvailable(rowData);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x001CB458 File Offset: 0x001CA858
		internal override void RowAvailable(ITypedGettersV3 rowData)
		{
			if (this._parent == null)
			{
				throw SQL.UnexpectedSmiEvent(SmiEventSink_Default.UnexpectedEventType.RowAvailable);
			}
			this._parent.RowAvailable(rowData);
		}

		// Token: 0x0600020B RID: 523 RVA: 0x001CB480 File Offset: 0x001CA880
		internal override void StatementCompleted(int rowsAffected)
		{
			if (this._parent == null)
			{
				throw SQL.UnexpectedSmiEvent(SmiEventSink_Default.UnexpectedEventType.StatementCompleted);
			}
			this._parent.StatementCompleted(rowsAffected);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x001CB4A8 File Offset: 0x001CA8A8
		internal override void TransactionCommitted(long transactionId)
		{
			if (this._parent == null)
			{
				throw SQL.UnexpectedSmiEvent(SmiEventSink_Default.UnexpectedEventType.TransactionCommitted);
			}
			this._parent.TransactionCommitted(transactionId);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x001CB4D4 File Offset: 0x001CA8D4
		internal override void TransactionDefected(long transactionId)
		{
			if (this._parent == null)
			{
				throw SQL.UnexpectedSmiEvent(SmiEventSink_Default.UnexpectedEventType.TransactionDefected);
			}
			this._parent.TransactionDefected(transactionId);
		}

		// Token: 0x0600020E RID: 526 RVA: 0x001CB500 File Offset: 0x001CA900
		internal override void TransactionEnlisted(long transactionId)
		{
			if (this._parent == null)
			{
				throw SQL.UnexpectedSmiEvent(SmiEventSink_Default.UnexpectedEventType.TransactionEnlisted);
			}
			this._parent.TransactionEnlisted(transactionId);
		}

		// Token: 0x0600020F RID: 527 RVA: 0x001CB52C File Offset: 0x001CA92C
		internal override void TransactionEnded(long transactionId)
		{
			if (this._parent == null)
			{
				throw SQL.UnexpectedSmiEvent(SmiEventSink_Default.UnexpectedEventType.TransactionEnded);
			}
			this._parent.TransactionEnded(transactionId);
		}

		// Token: 0x06000210 RID: 528 RVA: 0x001CB558 File Offset: 0x001CA958
		internal override void TransactionRolledBack(long transactionId)
		{
			if (this._parent == null)
			{
				throw SQL.UnexpectedSmiEvent(SmiEventSink_Default.UnexpectedEventType.TransactionRolledBack);
			}
			this._parent.TransactionRolledBack(transactionId);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x001CB584 File Offset: 0x001CA984
		internal override void TransactionStarted(long transactionId)
		{
			if (this._parent == null)
			{
				throw SQL.UnexpectedSmiEvent(SmiEventSink_Default.UnexpectedEventType.TransactionStarted);
			}
			this._parent.TransactionStarted(transactionId);
		}

		// Token: 0x04000572 RID: 1394
		private SmiEventSink _parent;

		// Token: 0x04000573 RID: 1395
		private SqlErrorCollection _errors;

		// Token: 0x04000574 RID: 1396
		private SqlErrorCollection _warnings;

		// Token: 0x02000037 RID: 55
		internal enum UnexpectedEventType
		{
			// Token: 0x04000576 RID: 1398
			BatchCompleted,
			// Token: 0x04000577 RID: 1399
			ColumnInfoAvailable,
			// Token: 0x04000578 RID: 1400
			DefaultDatabaseChanged,
			// Token: 0x04000579 RID: 1401
			MessagePosted,
			// Token: 0x0400057A RID: 1402
			MetaDataAvailable,
			// Token: 0x0400057B RID: 1403
			ParameterAvailable,
			// Token: 0x0400057C RID: 1404
			ParametersAvailable,
			// Token: 0x0400057D RID: 1405
			RowAvailable,
			// Token: 0x0400057E RID: 1406
			StatementCompleted,
			// Token: 0x0400057F RID: 1407
			TableNameAvailable,
			// Token: 0x04000580 RID: 1408
			TransactionCommitted,
			// Token: 0x04000581 RID: 1409
			TransactionDefected,
			// Token: 0x04000582 RID: 1410
			TransactionEnlisted,
			// Token: 0x04000583 RID: 1411
			TransactionEnded,
			// Token: 0x04000584 RID: 1412
			TransactionRolledBack,
			// Token: 0x04000585 RID: 1413
			TransactionStarted
		}
	}
}
