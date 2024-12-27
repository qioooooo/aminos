using System;
using System.Collections;
using System.Data.Common;

namespace System.Data.SqlClient
{
	// Token: 0x0200030D RID: 781
	internal sealed class SqlStatistics
	{
		// Token: 0x060028D2 RID: 10450 RVA: 0x00290C38 File Offset: 0x00290038
		internal static SqlStatistics StartTimer(SqlStatistics statistics)
		{
			if (statistics != null && !statistics.RequestExecutionTimer())
			{
				statistics = null;
			}
			return statistics;
		}

		// Token: 0x060028D3 RID: 10451 RVA: 0x00290C54 File Offset: 0x00290054
		internal static void StopTimer(SqlStatistics statistics)
		{
			if (statistics != null)
			{
				statistics.ReleaseAndUpdateExecutionTimer();
			}
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x060028D4 RID: 10452 RVA: 0x00290C6C File Offset: 0x0029006C
		// (set) Token: 0x060028D5 RID: 10453 RVA: 0x00290C80 File Offset: 0x00290080
		internal bool WaitForDoneAfterRow
		{
			get
			{
				return this._waitForDoneAfterRow;
			}
			set
			{
				this._waitForDoneAfterRow = value;
			}
		}

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x060028D6 RID: 10454 RVA: 0x00290C94 File Offset: 0x00290094
		internal bool WaitForReply
		{
			get
			{
				return this._waitForReply;
			}
		}

		// Token: 0x060028D7 RID: 10455 RVA: 0x00290CA8 File Offset: 0x002900A8
		internal SqlStatistics()
		{
		}

		// Token: 0x060028D8 RID: 10456 RVA: 0x00290CBC File Offset: 0x002900BC
		internal void ContinueOnNewConnection()
		{
			this._startExecutionTimestamp = 0L;
			this._startFetchTimestamp = 0L;
			this._waitForDoneAfterRow = false;
			this._waitForReply = false;
		}

		// Token: 0x060028D9 RID: 10457 RVA: 0x00290CE8 File Offset: 0x002900E8
		internal IDictionary GetHashtable()
		{
			return new Hashtable
			{
				{ "BuffersReceived", this._buffersReceived },
				{ "BuffersSent", this._buffersSent },
				{ "BytesReceived", this._bytesReceived },
				{ "BytesSent", this._bytesSent },
				{ "CursorOpens", this._cursorOpens },
				{ "IduCount", this._iduCount },
				{ "IduRows", this._iduRows },
				{ "PreparedExecs", this._preparedExecs },
				{ "Prepares", this._prepares },
				{ "SelectCount", this._selectCount },
				{ "SelectRows", this._selectRows },
				{ "ServerRoundtrips", this._serverRoundtrips },
				{ "SumResultSets", this._sumResultSets },
				{ "Transactions", this._transactions },
				{ "UnpreparedExecs", this._unpreparedExecs },
				{
					"ConnectionTime",
					ADP.TimerToMilliseconds(this._connectionTime)
				},
				{
					"ExecutionTime",
					ADP.TimerToMilliseconds(this._executionTime)
				},
				{
					"NetworkServerTime",
					ADP.TimerToMilliseconds(this._networkServerTime)
				}
			};
		}

		// Token: 0x060028DA RID: 10458 RVA: 0x00290E98 File Offset: 0x00290298
		internal bool RequestExecutionTimer()
		{
			if (this._startExecutionTimestamp == 0L)
			{
				ADP.TimerCurrent(out this._startExecutionTimestamp);
				return true;
			}
			return false;
		}

		// Token: 0x060028DB RID: 10459 RVA: 0x00290EC0 File Offset: 0x002902C0
		internal void RequestNetworkServerTimer()
		{
			if (this._startNetworkServerTimestamp == 0L)
			{
				ADP.TimerCurrent(out this._startNetworkServerTimestamp);
			}
			this._waitForReply = true;
		}

		// Token: 0x060028DC RID: 10460 RVA: 0x00290EEC File Offset: 0x002902EC
		internal void ReleaseAndUpdateExecutionTimer()
		{
			if (this._startExecutionTimestamp > 0L)
			{
				long num;
				ADP.TimerCurrent(out num);
				this._executionTime += num - this._startExecutionTimestamp;
				this._startExecutionTimestamp = 0L;
			}
		}

		// Token: 0x060028DD RID: 10461 RVA: 0x00290F28 File Offset: 0x00290328
		internal void ReleaseAndUpdateNetworkServerTimer()
		{
			if (this._waitForReply && this._startNetworkServerTimestamp > 0L)
			{
				long num;
				ADP.TimerCurrent(out num);
				this._networkServerTime += num - this._startNetworkServerTimestamp;
				this._startNetworkServerTimestamp = 0L;
			}
			this._waitForReply = false;
		}

		// Token: 0x060028DE RID: 10462 RVA: 0x00290F74 File Offset: 0x00290374
		internal void Reset()
		{
			this._buffersReceived = 0L;
			this._buffersSent = 0L;
			this._bytesReceived = 0L;
			this._bytesSent = 0L;
			this._connectionTime = 0L;
			this._cursorOpens = 0L;
			this._executionTime = 0L;
			this._iduCount = 0L;
			this._iduRows = 0L;
			this._networkServerTime = 0L;
			this._preparedExecs = 0L;
			this._prepares = 0L;
			this._selectCount = 0L;
			this._selectRows = 0L;
			this._serverRoundtrips = 0L;
			this._sumResultSets = 0L;
			this._transactions = 0L;
			this._unpreparedExecs = 0L;
			this._waitForDoneAfterRow = false;
			this._waitForReply = false;
			this._startExecutionTimestamp = 0L;
			this._startNetworkServerTimestamp = 0L;
		}

		// Token: 0x060028DF RID: 10463 RVA: 0x00291030 File Offset: 0x00290430
		internal void SafeAdd(ref long value, long summand)
		{
			if (9223372036854775807L - value > summand)
			{
				value += summand;
				return;
			}
			value = long.MaxValue;
		}

		// Token: 0x060028E0 RID: 10464 RVA: 0x00291060 File Offset: 0x00290460
		internal long SafeIncrement(ref long value)
		{
			if (value < 9223372036854775807L)
			{
				value += 1L;
			}
			return value;
		}

		// Token: 0x060028E1 RID: 10465 RVA: 0x00291084 File Offset: 0x00290484
		internal void UpdateStatistics()
		{
			if (this._closeTimestamp >= this._openTimestamp)
			{
				this.SafeAdd(ref this._connectionTime, this._closeTimestamp - this._openTimestamp);
				return;
			}
			this._connectionTime = long.MaxValue;
		}

		// Token: 0x0400196F RID: 6511
		internal long _closeTimestamp;

		// Token: 0x04001970 RID: 6512
		internal long _openTimestamp;

		// Token: 0x04001971 RID: 6513
		internal long _startExecutionTimestamp;

		// Token: 0x04001972 RID: 6514
		internal long _startFetchTimestamp;

		// Token: 0x04001973 RID: 6515
		internal long _startNetworkServerTimestamp;

		// Token: 0x04001974 RID: 6516
		internal long _buffersReceived;

		// Token: 0x04001975 RID: 6517
		internal long _buffersSent;

		// Token: 0x04001976 RID: 6518
		internal long _bytesReceived;

		// Token: 0x04001977 RID: 6519
		internal long _bytesSent;

		// Token: 0x04001978 RID: 6520
		internal long _connectionTime;

		// Token: 0x04001979 RID: 6521
		internal long _cursorOpens;

		// Token: 0x0400197A RID: 6522
		internal long _executionTime;

		// Token: 0x0400197B RID: 6523
		internal long _iduCount;

		// Token: 0x0400197C RID: 6524
		internal long _iduRows;

		// Token: 0x0400197D RID: 6525
		internal long _networkServerTime;

		// Token: 0x0400197E RID: 6526
		internal long _preparedExecs;

		// Token: 0x0400197F RID: 6527
		internal long _prepares;

		// Token: 0x04001980 RID: 6528
		internal long _selectCount;

		// Token: 0x04001981 RID: 6529
		internal long _selectRows;

		// Token: 0x04001982 RID: 6530
		internal long _serverRoundtrips;

		// Token: 0x04001983 RID: 6531
		internal long _sumResultSets;

		// Token: 0x04001984 RID: 6532
		internal long _transactions;

		// Token: 0x04001985 RID: 6533
		internal long _unpreparedExecs;

		// Token: 0x04001986 RID: 6534
		private bool _waitForDoneAfterRow;

		// Token: 0x04001987 RID: 6535
		private bool _waitForReply;
	}
}
