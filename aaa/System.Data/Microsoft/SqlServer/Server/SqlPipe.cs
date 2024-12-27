using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000051 RID: 81
	public sealed class SqlPipe
	{
		// Token: 0x06000374 RID: 884 RVA: 0x001CEFD4 File Offset: 0x001CE3D4
		internal SqlPipe(SmiContext smiContext)
		{
			this._smiContext = smiContext;
			this._eventSink = new SmiEventSink_Default();
		}

		// Token: 0x06000375 RID: 885 RVA: 0x001CEFFC File Offset: 0x001CE3FC
		public void ExecuteAndSend(SqlCommand command)
		{
			this.SetPipeBusy();
			try
			{
				this.EnsureNormalSendValid("ExecuteAndSend");
				if (command == null)
				{
					throw ADP.ArgumentNull("command");
				}
				SqlConnection connection = command.Connection;
				if (connection == null)
				{
					using (SqlConnection sqlConnection = new SqlConnection("Context Connection=true"))
					{
						sqlConnection.Open();
						try
						{
							command.Connection = sqlConnection;
							command.ExecuteToPipe(this._smiContext);
						}
						finally
						{
							command.Connection = null;
						}
						goto IL_0093;
					}
				}
				if (ConnectionState.Open != connection.State)
				{
					throw ADP.ClosedConnectionError();
				}
				if (!(connection.InnerConnection is SqlInternalConnectionSmi))
				{
					throw SQL.SqlPipeCommandHookedUpToNonContextConnection();
				}
				command.ExecuteToPipe(this._smiContext);
				IL_0093:;
			}
			finally
			{
				this.ClearPipeBusy();
			}
		}

		// Token: 0x06000376 RID: 886 RVA: 0x001CF0F4 File Offset: 0x001CE4F4
		public void Send(string message)
		{
			ADP.CheckArgumentNull(message, "message");
			if (4000L < (long)message.Length)
			{
				throw SQL.SqlPipeMessageTooLong(message.Length);
			}
			this.SetPipeBusy();
			try
			{
				this.EnsureNormalSendValid("Send");
				this._smiContext.SendMessageToPipe(message, this._eventSink);
				this._eventSink.ProcessMessagesAndThrow();
			}
			finally
			{
				this.ClearPipeBusy();
			}
		}

		// Token: 0x06000377 RID: 887 RVA: 0x001CF17C File Offset: 0x001CE57C
		public void Send(SqlDataReader reader)
		{
			ADP.CheckArgumentNull(reader, "reader");
			this.SetPipeBusy();
			try
			{
				this.EnsureNormalSendValid("Send");
				do
				{
					SmiExtendedMetaData[] internalSmiMetaData = reader.GetInternalSmiMetaData();
					if (internalSmiMetaData != null && internalSmiMetaData.Length != 0)
					{
						using (SmiRecordBuffer smiRecordBuffer = this._smiContext.CreateRecordBuffer(internalSmiMetaData, this._eventSink))
						{
							this._eventSink.ProcessMessagesAndThrow();
							this._smiContext.SendResultsStartToPipe(smiRecordBuffer, this._eventSink);
							this._eventSink.ProcessMessagesAndThrow();
							try
							{
								while (reader.Read())
								{
									if (SmiContextFactory.Instance.NegotiatedSmiVersion >= 210UL)
									{
										ValueUtilsSmi.FillCompatibleSettersFromReader(this._eventSink, smiRecordBuffer, new List<SmiExtendedMetaData>(internalSmiMetaData), reader);
									}
									else
									{
										ValueUtilsSmi.FillCompatibleITypedSettersFromReader(this._eventSink, smiRecordBuffer, internalSmiMetaData, reader);
									}
									this._smiContext.SendResultsRowToPipe(smiRecordBuffer, this._eventSink);
									this._eventSink.ProcessMessagesAndThrow();
								}
							}
							finally
							{
								this._smiContext.SendResultsEndToPipe(smiRecordBuffer, this._eventSink);
								this._eventSink.ProcessMessagesAndThrow();
							}
						}
					}
				}
				while (reader.NextResult());
			}
			finally
			{
				this.ClearPipeBusy();
			}
		}

		// Token: 0x06000378 RID: 888 RVA: 0x001CF2E0 File Offset: 0x001CE6E0
		public void Send(SqlDataRecord record)
		{
			ADP.CheckArgumentNull(record, "record");
			this.SetPipeBusy();
			try
			{
				this.EnsureNormalSendValid("Send");
				if (record.FieldCount != 0)
				{
					SmiRecordBuffer smiRecordBuffer;
					if (record.RecordContext == this._smiContext)
					{
						smiRecordBuffer = record.RecordBuffer;
					}
					else
					{
						SmiExtendedMetaData[] array = record.InternalGetSmiMetaData();
						smiRecordBuffer = this._smiContext.CreateRecordBuffer(array, this._eventSink);
						if (SmiContextFactory.Instance.NegotiatedSmiVersion >= 210UL)
						{
							ValueUtilsSmi.FillCompatibleSettersFromRecord(this._eventSink, smiRecordBuffer, array, record, null);
						}
						else
						{
							ValueUtilsSmi.FillCompatibleITypedSettersFromRecord(this._eventSink, smiRecordBuffer, array, record);
						}
					}
					this._smiContext.SendResultsStartToPipe(smiRecordBuffer, this._eventSink);
					this._eventSink.ProcessMessagesAndThrow();
					try
					{
						this._smiContext.SendResultsRowToPipe(smiRecordBuffer, this._eventSink);
						this._eventSink.ProcessMessagesAndThrow();
					}
					finally
					{
						this._smiContext.SendResultsEndToPipe(smiRecordBuffer, this._eventSink);
						this._eventSink.ProcessMessagesAndThrow();
					}
				}
			}
			finally
			{
				this.ClearPipeBusy();
			}
		}

		// Token: 0x06000379 RID: 889 RVA: 0x001CF410 File Offset: 0x001CE810
		public void SendResultsStart(SqlDataRecord record)
		{
			ADP.CheckArgumentNull(record, "record");
			this.SetPipeBusy();
			try
			{
				this.EnsureNormalSendValid("SendResultsStart");
				SmiRecordBuffer smiRecordBuffer = record.RecordBuffer;
				if (record.RecordContext == this._smiContext)
				{
					smiRecordBuffer = record.RecordBuffer;
				}
				else
				{
					smiRecordBuffer = this._smiContext.CreateRecordBuffer(record.InternalGetSmiMetaData(), this._eventSink);
				}
				this._smiContext.SendResultsStartToPipe(smiRecordBuffer, this._eventSink);
				this._eventSink.ProcessMessagesAndThrow();
				this._recordBufferSent = smiRecordBuffer;
				this._metaDataSent = record.InternalGetMetaData();
			}
			finally
			{
				this.ClearPipeBusy();
			}
		}

		// Token: 0x0600037A RID: 890 RVA: 0x001CF4C4 File Offset: 0x001CE8C4
		public void SendResultsRow(SqlDataRecord record)
		{
			ADP.CheckArgumentNull(record, "record");
			this.SetPipeBusy();
			try
			{
				this.EnsureResultStarted("SendResultsRow");
				if (this._hadErrorInResultSet)
				{
					throw SQL.SqlPipeErrorRequiresSendEnd();
				}
				this._hadErrorInResultSet = true;
				SmiRecordBuffer smiRecordBuffer;
				if (record.RecordContext == this._smiContext)
				{
					smiRecordBuffer = record.RecordBuffer;
				}
				else
				{
					SmiExtendedMetaData[] array = record.InternalGetSmiMetaData();
					smiRecordBuffer = this._smiContext.CreateRecordBuffer(array, this._eventSink);
					if (SmiContextFactory.Instance.NegotiatedSmiVersion >= 210UL)
					{
						ValueUtilsSmi.FillCompatibleSettersFromRecord(this._eventSink, smiRecordBuffer, array, record, null);
					}
					else
					{
						ValueUtilsSmi.FillCompatibleITypedSettersFromRecord(this._eventSink, smiRecordBuffer, array, record);
					}
				}
				this._smiContext.SendResultsRowToPipe(smiRecordBuffer, this._eventSink);
				this._eventSink.ProcessMessagesAndThrow();
				this._hadErrorInResultSet = false;
			}
			finally
			{
				this.ClearPipeBusy();
			}
		}

		// Token: 0x0600037B RID: 891 RVA: 0x001CF5B0 File Offset: 0x001CE9B0
		public void SendResultsEnd()
		{
			this.SetPipeBusy();
			try
			{
				this.EnsureResultStarted("SendResultsEnd");
				this._smiContext.SendResultsEndToPipe(this._recordBufferSent, this._eventSink);
				this._metaDataSent = null;
				this._recordBufferSent = null;
				this._hadErrorInResultSet = false;
				this._eventSink.ProcessMessagesAndThrow();
			}
			finally
			{
				this.ClearPipeBusy();
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600037C RID: 892 RVA: 0x001CF62C File Offset: 0x001CEA2C
		public bool IsSendingResults
		{
			get
			{
				return null != this._metaDataSent;
			}
		}

		// Token: 0x0600037D RID: 893 RVA: 0x001CF648 File Offset: 0x001CEA48
		internal void OnOutOfScope()
		{
			this._metaDataSent = null;
			this._recordBufferSent = null;
			this._hadErrorInResultSet = false;
			this._isBusy = false;
		}

		// Token: 0x0600037E RID: 894 RVA: 0x001CF674 File Offset: 0x001CEA74
		private void SetPipeBusy()
		{
			if (this._isBusy)
			{
				throw SQL.SqlPipeIsBusy();
			}
			this._isBusy = true;
		}

		// Token: 0x0600037F RID: 895 RVA: 0x001CF698 File Offset: 0x001CEA98
		private void ClearPipeBusy()
		{
			this._isBusy = false;
		}

		// Token: 0x06000380 RID: 896 RVA: 0x001CF6AC File Offset: 0x001CEAAC
		private void EnsureNormalSendValid(string methodName)
		{
			if (this.IsSendingResults)
			{
				throw SQL.SqlPipeAlreadyHasAnOpenResultSet(methodName);
			}
		}

		// Token: 0x06000381 RID: 897 RVA: 0x001CF6C8 File Offset: 0x001CEAC8
		private void EnsureResultStarted(string methodName)
		{
			if (!this.IsSendingResults)
			{
				throw SQL.SqlPipeDoesNotHaveAnOpenResultSet(methodName);
			}
		}

		// Token: 0x04000615 RID: 1557
		private SmiContext _smiContext;

		// Token: 0x04000616 RID: 1558
		private SmiRecordBuffer _recordBufferSent;

		// Token: 0x04000617 RID: 1559
		private SqlMetaData[] _metaDataSent;

		// Token: 0x04000618 RID: 1560
		private SmiEventSink_Default _eventSink;

		// Token: 0x04000619 RID: 1561
		private bool _isBusy;

		// Token: 0x0400061A RID: 1562
		private bool _hadErrorInResultSet;
	}
}
