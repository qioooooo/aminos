using System;
using System.Data.Common;
using System.Data.SqlTypes;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Data.OracleClient
{
	// Token: 0x0200006C RID: 108
	public sealed class OracleLob : Stream, ICloneable, IDisposable, INullable
	{
		// Token: 0x0600051A RID: 1306 RVA: 0x00068918 File Offset: 0x00067D18
		internal OracleLob()
		{
			this._isNull = true;
			this._lobType = OracleType.Blob;
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0006893C File Offset: 0x00067D3C
		internal OracleLob(OciLobLocator lobLocator)
		{
			this._lobLocator = lobLocator.Clone();
			this._lobType = this._lobLocator.LobType;
			this._charsetForm = ((OracleType.NClob == this._lobType) ? OCI.CHARSETFORM.SQLCS_NCHAR : OCI.CHARSETFORM.SQLCS_IMPLICIT);
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00068980 File Offset: 0x00067D80
		internal OracleLob(OracleLob lob)
		{
			this._lobLocator = lob._lobLocator.Clone();
			this._lobType = lob._lobLocator.LobType;
			this._charsetForm = lob._charsetForm;
			this._currentPosition = lob._currentPosition;
			this._isTemporaryState = lob._isTemporaryState;
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x000689DC File Offset: 0x00067DDC
		internal OracleLob(OracleConnection connection, OracleType oracleType)
		{
			this._lobLocator = new OciLobLocator(connection, oracleType);
			this._lobType = oracleType;
			this._charsetForm = ((OracleType.NClob == this._lobType) ? OCI.CHARSETFORM.SQLCS_NCHAR : OCI.CHARSETFORM.SQLCS_IMPLICIT);
			this._isTemporaryState = 1;
			OCI.LOB_TYPE lob_TYPE = ((OracleType.Blob == oracleType) ? OCI.LOB_TYPE.OCI_TEMP_BLOB : OCI.LOB_TYPE.OCI_TEMP_CLOB);
			int num = TracedNativeMethods.OCILobCreateTemporary(connection.ServiceContextHandle, connection.ErrorHandle, this._lobLocator.Descriptor, 0, this._charsetForm, lob_TYPE, 0, OCI.DURATION.OCI_DURATION_BEGIN);
			if (num != 0)
			{
				connection.CheckError(this.ErrorHandle, num);
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x00068A60 File Offset: 0x00067E60
		public override bool CanRead
		{
			get
			{
				return this.IsNull || !this.IsDisposed;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x0600051F RID: 1311 RVA: 0x00068A80 File Offset: 0x00067E80
		public override bool CanSeek
		{
			get
			{
				return this.IsNull || !this.IsDisposed;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x00068AA0 File Offset: 0x00067EA0
		public override bool CanWrite
		{
			get
			{
				bool flag = OracleType.BFile != this._lobType;
				if (!this.IsNull)
				{
					flag = !this.IsDisposed;
				}
				return flag;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000521 RID: 1313 RVA: 0x00068AD0 File Offset: 0x00067ED0
		public int ChunkSize
		{
			get
			{
				this.AssertObjectNotDisposed();
				if (this.IsNull)
				{
					return 0;
				}
				this.AssertConnectionIsOpen();
				uint num = 0U;
				int num2 = TracedNativeMethods.OCILobGetChunkSize(this.ServiceContextHandle, this.ErrorHandle, this.Descriptor, out num);
				if (num2 != 0)
				{
					this.Connection.CheckError(this.ErrorHandle, num2);
				}
				return (int)num;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000522 RID: 1314 RVA: 0x00068B28 File Offset: 0x00067F28
		public OracleConnection Connection
		{
			get
			{
				this.AssertObjectNotDisposed();
				OciLobLocator lobLocator = this.LobLocator;
				if (lobLocator == null)
				{
					return null;
				}
				return lobLocator.Connection;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x00068B50 File Offset: 0x00067F50
		private bool ConnectionIsClosed
		{
			get
			{
				return this.LobLocator == null || this.LobLocator.ConnectionIsClosed;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x00068B74 File Offset: 0x00067F74
		private uint CurrentOraclePosition
		{
			get
			{
				return (uint)this.AdjustOffsetToOracle(this._currentPosition) + 1U;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000525 RID: 1317 RVA: 0x00068B90 File Offset: 0x00067F90
		internal OciHandle Descriptor
		{
			get
			{
				return this.LobLocator.Descriptor;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x00068BA8 File Offset: 0x00067FA8
		internal OciErrorHandle ErrorHandle
		{
			get
			{
				return this.LobLocator.ErrorHandle;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000527 RID: 1319 RVA: 0x00068BC0 File Offset: 0x00067FC0
		public bool IsBatched
		{
			get
			{
				if (this.IsNull || this.IsDisposed || this.ConnectionIsClosed)
				{
					return false;
				}
				int num2;
				int num = TracedNativeMethods.OCILobIsOpen(this.ServiceContextHandle, this.ErrorHandle, this.Descriptor, out num2);
				if (num != 0)
				{
					this.Connection.CheckError(this.ErrorHandle, num);
				}
				return num2 != 0;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x00068C20 File Offset: 0x00068020
		private bool IsCharacterLob
		{
			get
			{
				return OracleType.Clob == this._lobType || OracleType.NClob == this._lobType;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000529 RID: 1321 RVA: 0x00068C44 File Offset: 0x00068044
		private bool IsDisposed
		{
			get
			{
				return !this._isNull && null == this.LobLocator;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x00068C64 File Offset: 0x00068064
		public bool IsNull
		{
			get
			{
				return this._isNull;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600052B RID: 1323 RVA: 0x00068C78 File Offset: 0x00068078
		public bool IsTemporary
		{
			get
			{
				this.AssertObjectNotDisposed();
				if (this.IsNull)
				{
					return false;
				}
				this.AssertConnectionIsOpen();
				if (this._isTemporaryState == 0)
				{
					int num2;
					int num = TracedNativeMethods.OCILobIsTemporary(this.Connection.EnvironmentHandle, this.ErrorHandle, this.Descriptor, out num2);
					if (num != 0)
					{
						this.Connection.CheckError(this.ErrorHandle, num);
					}
					this._isTemporaryState = ((num2 != 0) ? 1 : 2);
				}
				return 1 == this._isTemporaryState;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x0600052C RID: 1324 RVA: 0x00068CF0 File Offset: 0x000680F0
		internal OciLobLocator LobLocator
		{
			get
			{
				return this._lobLocator;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x0600052D RID: 1325 RVA: 0x00068D04 File Offset: 0x00068104
		public OracleType LobType
		{
			get
			{
				return this._lobType;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x00068D18 File Offset: 0x00068118
		public override long Length
		{
			get
			{
				this.AssertObjectNotDisposed();
				if (this.IsNull)
				{
					return 0L;
				}
				this.AssertConnectionIsOpen();
				uint num2;
				int num = TracedNativeMethods.OCILobGetLength(this.ServiceContextHandle, this.ErrorHandle, this.Descriptor, out num2);
				if (num != 0)
				{
					this.Connection.CheckError(this.ErrorHandle, num);
				}
				return this.AdjustOracleToOffset((long)((ulong)num2));
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x0600052F RID: 1327 RVA: 0x00068D74 File Offset: 0x00068174
		// (set) Token: 0x06000530 RID: 1328 RVA: 0x00068DA0 File Offset: 0x000681A0
		public override long Position
		{
			get
			{
				this.AssertObjectNotDisposed();
				if (this.IsNull)
				{
					return 0L;
				}
				this.AssertConnectionIsOpen();
				return this._currentPosition;
			}
			set
			{
				if (!this.IsNull)
				{
					this.Seek(value, SeekOrigin.Begin);
				}
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000531 RID: 1329 RVA: 0x00068DC0 File Offset: 0x000681C0
		internal OciServiceContextHandle ServiceContextHandle
		{
			get
			{
				return this.LobLocator.ServiceContextHandle;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x00068DD8 File Offset: 0x000681D8
		public object Value
		{
			get
			{
				this.AssertObjectNotDisposed();
				if (this.IsNull)
				{
					return DBNull.Value;
				}
				long currentPosition = this._currentPosition;
				int num = (int)this.Length;
				bool flag = OracleType.Blob == this._lobType || OracleType.BFile == this._lobType;
				if (num != 0)
				{
					string text;
					try
					{
						this.Seek(0L, SeekOrigin.Begin);
						if (flag)
						{
							byte[] array = new byte[num];
							this.Read(array, 0, num);
							return array;
						}
						try
						{
							StreamReader streamReader = new StreamReader(this, Encoding.Unicode);
							text = streamReader.ReadToEnd();
						}
						finally
						{
						}
					}
					finally
					{
						this._currentPosition = currentPosition;
					}
					return text;
				}
				if (flag)
				{
					return new byte[0];
				}
				return string.Empty;
			}
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00068EB4 File Offset: 0x000682B4
		internal int AdjustOffsetToOracle(int amount)
		{
			return this.IsCharacterLob ? (amount / 2) : amount;
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x00068ED4 File Offset: 0x000682D4
		internal long AdjustOffsetToOracle(long amount)
		{
			return this.IsCharacterLob ? (amount / 2L) : amount;
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x00068EF4 File Offset: 0x000682F4
		internal int AdjustOracleToOffset(int amount)
		{
			return this.IsCharacterLob ? checked(amount * 2) : amount;
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x00068F14 File Offset: 0x00068314
		internal long AdjustOracleToOffset(long amount)
		{
			return this.IsCharacterLob ? checked(amount * 2L) : amount;
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x00068F34 File Offset: 0x00068334
		internal void AssertAmountIsEven(long amount, string argName)
		{
			if (this.IsCharacterLob && 1L == (amount & 1L))
			{
				throw ADP.LobAmountMustBeEven(argName);
			}
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x00068F58 File Offset: 0x00068358
		internal void AssertAmountIsValidOddOK(long amount, string argName)
		{
			if (amount < 0L || amount >= (long)((ulong)(-1)))
			{
				throw ADP.LobAmountExceeded(argName);
			}
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00068F78 File Offset: 0x00068378
		internal void AssertAmountIsValid(long amount, string argName)
		{
			this.AssertAmountIsValidOddOK(amount, argName);
			this.AssertAmountIsEven(amount, argName);
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00068F98 File Offset: 0x00068398
		internal void AssertConnectionIsOpen()
		{
			if (this.ConnectionIsClosed)
			{
				throw ADP.ClosedConnectionError();
			}
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x00068FB4 File Offset: 0x000683B4
		internal void AssertObjectNotDisposed()
		{
			if (this.IsDisposed)
			{
				throw ADP.ObjectDisposed("OracleLob");
			}
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00068FD4 File Offset: 0x000683D4
		internal void AssertPositionIsValid()
		{
			if (this.IsCharacterLob && 1L == (this._currentPosition & 1L))
			{
				throw ADP.LobPositionMustBeEven();
			}
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00068FFC File Offset: 0x000683FC
		internal void AssertTransactionExists()
		{
			if (!this.Connection.HasTransaction)
			{
				throw ADP.LobWriteRequiresTransaction();
			}
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0006901C File Offset: 0x0006841C
		public void Append(OracleLob source)
		{
			if (source == null)
			{
				throw ADP.ArgumentNull("source");
			}
			this.AssertObjectNotDisposed();
			source.AssertObjectNotDisposed();
			if (this.IsNull)
			{
				throw ADP.LobWriteInvalidOnNull();
			}
			if (!source.IsNull)
			{
				this.AssertConnectionIsOpen();
				int num = TracedNativeMethods.OCILobAppend(this.ServiceContextHandle, this.ErrorHandle, this.Descriptor, source.Descriptor);
				if (num != 0)
				{
					this.Connection.CheckError(this.ErrorHandle, num);
				}
			}
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00069094 File Offset: 0x00068494
		public void BeginBatch()
		{
			this.BeginBatch(OracleLobOpenMode.ReadOnly);
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x000690A8 File Offset: 0x000684A8
		public void BeginBatch(OracleLobOpenMode mode)
		{
			this.AssertObjectNotDisposed();
			if (!this.IsNull)
			{
				this.AssertConnectionIsOpen();
				this.LobLocator.Open(mode);
			}
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x000690D8 File Offset: 0x000684D8
		public object Clone()
		{
			this.AssertObjectNotDisposed();
			if (this.IsNull)
			{
				return OracleLob.Null;
			}
			this.AssertConnectionIsOpen();
			return new OracleLob(this);
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x00069108 File Offset: 0x00068508
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && !this.IsNull && !this.ConnectionIsClosed)
				{
					this.Flush();
					OciLobLocator.SafeDispose(ref this._lobLocator);
					this._lobLocator = null;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x00069168 File Offset: 0x00068568
		public long CopyTo(OracleLob destination)
		{
			return this.CopyTo(0L, destination, 0L, this.Length);
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x00069188 File Offset: 0x00068588
		public long CopyTo(OracleLob destination, long destinationOffset)
		{
			return this.CopyTo(0L, destination, destinationOffset, this.Length);
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x000691A8 File Offset: 0x000685A8
		public long CopyTo(long sourceOffset, OracleLob destination, long destinationOffset, long amount)
		{
			if (destination == null)
			{
				throw ADP.ArgumentNull("destination");
			}
			this.AssertObjectNotDisposed();
			destination.AssertObjectNotDisposed();
			this.AssertAmountIsValid(amount, "amount");
			this.AssertAmountIsValid(sourceOffset, "sourceOffset");
			this.AssertAmountIsValid(destinationOffset, "destinationOffset");
			if (destination.IsNull)
			{
				throw ADP.LobWriteInvalidOnNull();
			}
			if (this.IsNull)
			{
				return 0L;
			}
			this.AssertConnectionIsOpen();
			this.AssertTransactionExists();
			long num = this.AdjustOffsetToOracle(Math.Min(this.Length - sourceOffset, amount));
			long num2 = this.AdjustOffsetToOracle(destinationOffset) + 1L;
			long num3 = this.AdjustOffsetToOracle(sourceOffset) + 1L;
			if (0L >= num)
			{
				return 0L;
			}
			int num4 = TracedNativeMethods.OCILobCopy(this.ServiceContextHandle, this.ErrorHandle, destination.Descriptor, this.Descriptor, (uint)num, (uint)num2, (uint)num3);
			if (num4 != 0)
			{
				this.Connection.CheckError(this.ErrorHandle, num4);
			}
			return this.AdjustOracleToOffset(num);
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x00069294 File Offset: 0x00068694
		public void EndBatch()
		{
			this.AssertObjectNotDisposed();
			if (!this.IsNull)
			{
				this.AssertConnectionIsOpen();
				this.LobLocator.ForceClose();
			}
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x000692C0 File Offset: 0x000686C0
		public long Erase()
		{
			return this.Erase(0L, this.Length);
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x000692DC File Offset: 0x000686DC
		public long Erase(long offset, long amount)
		{
			this.AssertObjectNotDisposed();
			if (this.IsNull)
			{
				throw ADP.LobWriteInvalidOnNull();
			}
			this.AssertAmountIsValid(amount, "amount");
			this.AssertAmountIsEven(offset, "offset");
			this.AssertPositionIsValid();
			this.AssertConnectionIsOpen();
			this.AssertTransactionExists();
			if (offset < 0L || offset >= (long)((ulong)(-1)))
			{
				return 0L;
			}
			uint num = (uint)this.AdjustOffsetToOracle(amount);
			uint num2 = (uint)this.AdjustOffsetToOracle(offset) + 1U;
			int num3 = TracedNativeMethods.OCILobErase(this.ServiceContextHandle, this.ErrorHandle, this.Descriptor, ref num, num2);
			if (num3 != 0)
			{
				this.Connection.CheckError(this.ErrorHandle, num3);
			}
			return this.AdjustOracleToOffset((long)((ulong)num));
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00069384 File Offset: 0x00068784
		internal void Free()
		{
			int num = TracedNativeMethods.OCILobFreeTemporary(this._lobLocator.ServiceContextHandle, this._lobLocator.ErrorHandle, this._lobLocator.Descriptor);
			if (num != 0)
			{
				this._lobLocator.Connection.CheckError(this.ErrorHandle, num);
			}
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x000693D4 File Offset: 0x000687D4
		public override void Flush()
		{
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x000693E4 File Offset: 0x000687E4
		public override int Read(byte[] buffer, int offset, int count)
		{
			this.AssertObjectNotDisposed();
			if (count < 0)
			{
				throw ADP.MustBePositive("count");
			}
			if (offset < 0)
			{
				throw ADP.MustBePositive("offset");
			}
			if (buffer == null)
			{
				throw ADP.ArgumentNull("buffer");
			}
			if ((long)buffer.Length < (long)offset + (long)count)
			{
				throw ADP.BufferExceeded("count");
			}
			if (this.IsNull || count == 0)
			{
				return 0;
			}
			this.AssertConnectionIsOpen();
			this.AssertAmountIsValidOddOK((long)offset, "offset");
			this.AssertAmountIsValidOddOK((long)count, "count");
			uint num = (uint)this._currentPosition;
			int num2 = 0;
			byte[] array = buffer;
			int num3 = offset;
			int num4 = count;
			if (this.IsCharacterLob)
			{
				num2 = (int)(num & 1U);
				int num5 = offset & 1;
				int num6 = count & 1;
				num /= 2U;
				if (1 == num5 || 1 == num2 || 1 == num6)
				{
					num3 = 0;
					num4 = count + num6 + 2 * num2;
					array = new byte[num4];
				}
			}
			ushort num7 = (this.IsCharacterLob ? 1000 : 0);
			int num8 = 0;
			int num9 = this.AdjustOffsetToOracle(num4);
			GCHandle gchandle = default(GCHandle);
			try
			{
				gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
				IntPtr intPtr = new IntPtr((long)gchandle.AddrOfPinnedObject() + (long)num3);
				num8 = TracedNativeMethods.OCILobRead(this.ServiceContextHandle, this.ErrorHandle, this.Descriptor, ref num9, num + 1U, intPtr, checked((uint)num4), num7, this._charsetForm);
			}
			finally
			{
				if (gchandle.IsAllocated)
				{
					gchandle.Free();
				}
			}
			if (99 == num8)
			{
				num8 = 0;
			}
			if (100 == num8)
			{
				return 0;
			}
			if (num8 != 0)
			{
				this.Connection.CheckError(this.ErrorHandle, num8);
			}
			num9 = this.AdjustOracleToOffset(num9);
			if (array != buffer)
			{
				if (num9 >= count)
				{
					num9 = count;
				}
				else
				{
					num9 -= num2;
				}
				Buffer.BlockCopy(array, num2, buffer, offset, num9);
				array = null;
			}
			this._currentPosition += (long)num9;
			return num9;
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x000695B8 File Offset: 0x000689B8
		public override long Seek(long offset, SeekOrigin origin)
		{
			this.AssertObjectNotDisposed();
			if (this.IsNull)
			{
				return 0L;
			}
			long length = this.Length;
			long num;
			switch (origin)
			{
			case SeekOrigin.Begin:
				num = offset;
				break;
			case SeekOrigin.Current:
				num = this._currentPosition + offset;
				break;
			case SeekOrigin.End:
				num = length + offset;
				break;
			default:
				throw ADP.InvalidSeekOrigin(origin);
			}
			if (num < 0L || num > length)
			{
				throw ADP.SeekBeyondEnd("offset");
			}
			this._currentPosition = num;
			return this._currentPosition;
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00069634 File Offset: 0x00068A34
		public override void SetLength(long value)
		{
			this.AssertObjectNotDisposed();
			if (this.IsNull)
			{
				throw ADP.LobWriteInvalidOnNull();
			}
			this.AssertConnectionIsOpen();
			this.AssertAmountIsValid(value, "value");
			this.AssertTransactionExists();
			uint num = (uint)this.AdjustOffsetToOracle(value);
			int num2 = TracedNativeMethods.OCILobTrim(this.ServiceContextHandle, this.ErrorHandle, this.Descriptor, num);
			if (num2 != 0)
			{
				this.Connection.CheckError(this.ErrorHandle, num2);
			}
			this._currentPosition = Math.Min(this._currentPosition, value);
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x000696B8 File Offset: 0x00068AB8
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.AssertObjectNotDisposed();
			this.AssertConnectionIsOpen();
			if (count < 0)
			{
				throw ADP.MustBePositive("count");
			}
			if (offset < 0)
			{
				throw ADP.MustBePositive("offset");
			}
			if (buffer == null)
			{
				throw ADP.ArgumentNull("buffer");
			}
			if ((long)buffer.Length < (long)offset + (long)count)
			{
				throw ADP.BufferExceeded("count");
			}
			this.AssertTransactionExists();
			if (this.IsNull)
			{
				throw ADP.LobWriteInvalidOnNull();
			}
			this.AssertAmountIsValid((long)offset, "offset");
			this.AssertAmountIsValid((long)count, "count");
			this.AssertPositionIsValid();
			OCI.CHARSETFORM charsetForm = this._charsetForm;
			ushort num = (this.IsCharacterLob ? 1000 : 0);
			int num2 = this.AdjustOffsetToOracle(count);
			int num3 = 0;
			if (num2 == 0)
			{
				return;
			}
			GCHandle gchandle = default(GCHandle);
			try
			{
				gchandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
				IntPtr intPtr = new IntPtr((long)gchandle.AddrOfPinnedObject() + (long)offset);
				num3 = TracedNativeMethods.OCILobWrite(this.ServiceContextHandle, this.ErrorHandle, this.Descriptor, ref num2, this.CurrentOraclePosition, intPtr, (uint)count, 0, num, charsetForm);
			}
			finally
			{
				if (gchandle.IsAllocated)
				{
					gchandle.Free();
				}
			}
			if (num3 != 0)
			{
				this.Connection.CheckError(this.ErrorHandle, num3);
			}
			num2 = this.AdjustOracleToOffset(num2);
			this._currentPosition += (long)num2;
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00069818 File Offset: 0x00068C18
		public override void WriteByte(byte value)
		{
			if (OracleType.Clob == this._lobType || OracleType.NClob == this._lobType)
			{
				throw ADP.WriteByteForBinaryLobsOnly();
			}
			base.WriteByte(value);
		}

		// Token: 0x04000456 RID: 1110
		private const byte x_IsTemporaryUnknown = 0;

		// Token: 0x04000457 RID: 1111
		private const byte x_IsTemporary = 1;

		// Token: 0x04000458 RID: 1112
		private const byte x_IsNotTemporary = 2;

		// Token: 0x04000459 RID: 1113
		private bool _isNull;

		// Token: 0x0400045A RID: 1114
		private OciLobLocator _lobLocator;

		// Token: 0x0400045B RID: 1115
		private OracleType _lobType;

		// Token: 0x0400045C RID: 1116
		private OCI.CHARSETFORM _charsetForm;

		// Token: 0x0400045D RID: 1117
		private long _currentPosition;

		// Token: 0x0400045E RID: 1118
		private byte _isTemporaryState;

		// Token: 0x0400045F RID: 1119
		public new static readonly OracleLob Null = new OracleLob();
	}
}
