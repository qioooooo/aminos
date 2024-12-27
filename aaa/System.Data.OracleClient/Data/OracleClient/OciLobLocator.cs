using System;
using System.Data.Common;
using System.Runtime.ConstrainedExecution;
using System.Threading;

namespace System.Data.OracleClient
{
	// Token: 0x02000049 RID: 73
	internal sealed class OciLobLocator
	{
		// Token: 0x06000214 RID: 532 RVA: 0x0005B5B4 File Offset: 0x0005A9B4
		internal OciLobLocator(OracleConnection connection, OracleType lobType)
		{
			this._connection = connection;
			this._connectionCloseCount = connection.CloseCount;
			this._lobType = lobType;
			this._cloneCount = 1;
			switch (lobType)
			{
			case OracleType.BFile:
				this._descriptor = new OciFileDescriptor(connection.ServiceContextHandle);
				return;
			case OracleType.Blob:
			case OracleType.Clob:
				break;
			case OracleType.Char:
				return;
			default:
				if (lobType != OracleType.NClob)
				{
					return;
				}
				break;
			}
			this._descriptor = new OciLobDescriptor(connection.ServiceContextHandle);
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000215 RID: 533 RVA: 0x0005B62C File Offset: 0x0005AA2C
		internal OracleConnection Connection
		{
			get
			{
				return this._connection;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000216 RID: 534 RVA: 0x0005B640 File Offset: 0x0005AA40
		internal bool ConnectionIsClosed
		{
			get
			{
				return this._connection == null || this._connectionCloseCount != this._connection.CloseCount;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000217 RID: 535 RVA: 0x0005B670 File Offset: 0x0005AA70
		internal OciErrorHandle ErrorHandle
		{
			get
			{
				return this.Connection.ErrorHandle;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000218 RID: 536 RVA: 0x0005B688 File Offset: 0x0005AA88
		internal OciHandle Descriptor
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this._descriptor;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000219 RID: 537 RVA: 0x0005B69C File Offset: 0x0005AA9C
		public OracleType LobType
		{
			get
			{
				return this._lobType;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600021A RID: 538 RVA: 0x0005B6B0 File Offset: 0x0005AAB0
		internal OciServiceContextHandle ServiceContextHandle
		{
			get
			{
				return this.Connection.ServiceContextHandle;
			}
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0005B6C8 File Offset: 0x0005AAC8
		internal OciLobLocator Clone()
		{
			Interlocked.Increment(ref this._cloneCount);
			return this;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0005B6E4 File Offset: 0x0005AAE4
		internal void Dispose()
		{
			if (Interlocked.Decrement(ref this._cloneCount) == 0)
			{
				if (this._openMode != 0 && !this.ConnectionIsClosed)
				{
					this.ForceClose();
				}
				OciHandle.SafeDispose(ref this._descriptor);
				GC.KeepAlive(this);
				this._connection = null;
			}
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0005B730 File Offset: 0x0005AB30
		internal void ForceClose()
		{
			if (this._openMode != 0)
			{
				int num = TracedNativeMethods.OCILobClose(this.ServiceContextHandle, this.ErrorHandle, this.Descriptor);
				if (num != 0)
				{
					this.Connection.CheckError(this.ErrorHandle, num);
				}
				this._openMode = 0;
			}
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0005B77C File Offset: 0x0005AB7C
		internal void ForceOpen()
		{
			if (this._openMode != 0)
			{
				int num = TracedNativeMethods.OCILobOpen(this.ServiceContextHandle, this.ErrorHandle, this.Descriptor, (byte)this._openMode);
				if (num != 0)
				{
					this._openMode = 0;
					this.Connection.CheckError(this.ErrorHandle, num);
				}
			}
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0005B7CC File Offset: 0x0005ABCC
		internal void Open(OracleLobOpenMode mode)
		{
			OracleLobOpenMode oracleLobOpenMode = (OracleLobOpenMode)Interlocked.CompareExchange(ref this._openMode, (int)mode, 0);
			if (oracleLobOpenMode == (OracleLobOpenMode)0)
			{
				this.ForceOpen();
				return;
			}
			if (mode != oracleLobOpenMode)
			{
				throw ADP.CannotOpenLobWithDifferentMode(mode, oracleLobOpenMode);
			}
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0005B800 File Offset: 0x0005AC00
		internal static void SafeDispose(ref OciLobLocator locator)
		{
			if (locator != null)
			{
				locator.Dispose();
			}
			locator = null;
		}

		// Token: 0x04000325 RID: 805
		private OracleConnection _connection;

		// Token: 0x04000326 RID: 806
		private int _connectionCloseCount;

		// Token: 0x04000327 RID: 807
		private OracleType _lobType;

		// Token: 0x04000328 RID: 808
		private OciHandle _descriptor;

		// Token: 0x04000329 RID: 809
		private int _cloneCount;

		// Token: 0x0400032A RID: 810
		private int _openMode;
	}
}
