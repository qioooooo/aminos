using System;
using System.Data.Common;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Xml;

namespace System.Data.SqlClient
{
	// Token: 0x0200030E RID: 782
	internal sealed class SqlStream : Stream
	{
		// Token: 0x060028E2 RID: 10466 RVA: 0x002910C8 File Offset: 0x002904C8
		internal SqlStream(SqlDataReader reader, bool addByteOrderMark, bool processAllRows)
			: this(0, reader, addByteOrderMark, processAllRows, true)
		{
		}

		// Token: 0x060028E3 RID: 10467 RVA: 0x002910E0 File Offset: 0x002904E0
		internal SqlStream(int columnOrdinal, SqlDataReader reader, bool addByteOrderMark, bool processAllRows, bool advanceReader)
		{
			this._columnOrdinal = columnOrdinal;
			this._reader = reader;
			this._bom = (addByteOrderMark ? 65279 : 0);
			this._processAllRows = processAllRows;
			this._advanceReader = advanceReader;
		}

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x060028E4 RID: 10468 RVA: 0x00291124 File Offset: 0x00290524
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x060028E5 RID: 10469 RVA: 0x00291134 File Offset: 0x00290534
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x060028E6 RID: 10470 RVA: 0x00291144 File Offset: 0x00290544
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x060028E7 RID: 10471 RVA: 0x00291154 File Offset: 0x00290554
		public override long Length
		{
			get
			{
				throw ADP.NotSupported();
			}
		}

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x060028E8 RID: 10472 RVA: 0x00291168 File Offset: 0x00290568
		// (set) Token: 0x060028E9 RID: 10473 RVA: 0x0029117C File Offset: 0x0029057C
		public override long Position
		{
			get
			{
				throw ADP.NotSupported();
			}
			set
			{
				throw ADP.NotSupported();
			}
		}

		// Token: 0x060028EA RID: 10474 RVA: 0x00291190 File Offset: 0x00290590
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && this._advanceReader && this._reader != null && !this._reader.IsClosed)
				{
					this._reader.Close();
				}
				this._reader = null;
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x060028EB RID: 10475 RVA: 0x002911F8 File Offset: 0x002905F8
		public override void Flush()
		{
			throw ADP.NotSupported();
		}

		// Token: 0x060028EC RID: 10476 RVA: 0x0029120C File Offset: 0x0029060C
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = 0;
			int num2 = 0;
			if (this._reader == null)
			{
				throw ADP.StreamClosed("Read");
			}
			if (buffer == null)
			{
				throw ADP.ArgumentNull("buffer");
			}
			if (offset < 0 || count < 0)
			{
				throw ADP.ArgumentOutOfRange(string.Empty, (offset < 0) ? "offset" : "count");
			}
			if (buffer.Length - offset < count)
			{
				throw ADP.ArgumentOutOfRange("count");
			}
			if (this._bom > 0)
			{
				this._bufferedData = new byte[2];
				num2 = this.ReadBytes(this._bufferedData, 0, 2);
				if (num2 < 2 || (this._bufferedData[0] == 223 && this._bufferedData[1] == 255))
				{
					this._bom = 0;
				}
				while (count > 0 && this._bom > 0)
				{
					buffer[offset] = (byte)this._bom;
					this._bom >>= 8;
					offset++;
					count--;
					num++;
				}
			}
			if (num2 > 0)
			{
				while (count > 0)
				{
					buffer[offset++] = this._bufferedData[0];
					num++;
					count--;
					if (num2 > 1 && count > 0)
					{
						buffer[offset++] = this._bufferedData[1];
						num++;
						count--;
						break;
					}
				}
				this._bufferedData = null;
			}
			return num + this.ReadBytes(buffer, offset, count);
		}

		// Token: 0x060028ED RID: 10477 RVA: 0x00291354 File Offset: 0x00290754
		private int ReadBytes(byte[] buffer, int offset, int count)
		{
			bool flag = true;
			int num = 0;
			if (this._reader.IsClosed || this._endOfColumn)
			{
				return 0;
			}
			try
			{
				while (count > 0)
				{
					if (this._advanceReader && 0L == this._bytesCol)
					{
						flag = false;
						while (!this._readFirstRow || this._processAllRows)
						{
							if (this._reader.Read())
							{
								this._readFirstRow = true;
								flag = true;
								goto IL_0079;
							}
							if (!this._reader.NextResult())
							{
								goto IL_0079;
							}
						}
						this._reader.Close();
					}
					IL_0079:
					if (!flag)
					{
						break;
					}
					int num2 = (int)this._reader.GetBytesInternal(this._columnOrdinal, this._bytesCol, buffer, offset, count);
					if (num2 < count)
					{
						this._bytesCol = 0L;
						flag = false;
						if (!this._advanceReader)
						{
							this._endOfColumn = true;
						}
					}
					else
					{
						this._bytesCol += (long)num2;
					}
					count -= num2;
					offset += num2;
					num += num2;
				}
				if (!flag && this._advanceReader)
				{
					this._reader.Close();
				}
			}
			catch (Exception ex)
			{
				if (this._advanceReader && ADP.IsCatchableExceptionType(ex))
				{
					this._reader.Close();
				}
				throw;
			}
			return num;
		}

		// Token: 0x060028EE RID: 10478 RVA: 0x00291494 File Offset: 0x00290894
		internal XmlReader ToXmlReader()
		{
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.ConformanceLevel = ConformanceLevel.Fragment;
			xmlReaderSettings.CloseInput = true;
			MethodInfo method = typeof(XmlReader).GetMethod("CreateSqlReader", BindingFlags.Static | BindingFlags.NonPublic);
			object[] array = new object[3];
			array[0] = this;
			array[1] = xmlReaderSettings;
			object[] array2 = array;
			new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Assert();
			XmlReader xmlReader;
			try
			{
				xmlReader = (XmlReader)method.Invoke(null, array2);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return xmlReader;
		}

		// Token: 0x060028EF RID: 10479 RVA: 0x00291520 File Offset: 0x00290920
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw ADP.NotSupported();
		}

		// Token: 0x060028F0 RID: 10480 RVA: 0x00291534 File Offset: 0x00290934
		public override void SetLength(long value)
		{
			throw ADP.NotSupported();
		}

		// Token: 0x060028F1 RID: 10481 RVA: 0x00291548 File Offset: 0x00290948
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw ADP.NotSupported();
		}

		// Token: 0x04001988 RID: 6536
		private SqlDataReader _reader;

		// Token: 0x04001989 RID: 6537
		private int _columnOrdinal;

		// Token: 0x0400198A RID: 6538
		private long _bytesCol;

		// Token: 0x0400198B RID: 6539
		private int _bom;

		// Token: 0x0400198C RID: 6540
		private byte[] _bufferedData;

		// Token: 0x0400198D RID: 6541
		private bool _processAllRows;

		// Token: 0x0400198E RID: 6542
		private bool _advanceReader;

		// Token: 0x0400198F RID: 6543
		private bool _readFirstRow;

		// Token: 0x04001990 RID: 6544
		private bool _endOfColumn;
	}
}
