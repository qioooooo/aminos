using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x0200004A RID: 74
	internal class SmiSettersStream : Stream
	{
		// Token: 0x060002D6 RID: 726 RVA: 0x001CD8A0 File Offset: 0x001CCCA0
		internal SmiSettersStream(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData)
		{
			this._sink = sink;
			this._setters = setters;
			this._ordinal = ordinal;
			this._lengthWritten = 0L;
			this._metaData = metaData;
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x001CD8D8 File Offset: 0x001CCCD8
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x001CD8E8 File Offset: 0x001CCCE8
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x001CD8F8 File Offset: 0x001CCCF8
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060002DA RID: 730 RVA: 0x001CD908 File Offset: 0x001CCD08
		public override long Length
		{
			get
			{
				return this._lengthWritten;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060002DB RID: 731 RVA: 0x001CD91C File Offset: 0x001CCD1C
		// (set) Token: 0x060002DC RID: 732 RVA: 0x001CD930 File Offset: 0x001CCD30
		public override long Position
		{
			get
			{
				return this._lengthWritten;
			}
			set
			{
				throw SQL.StreamSeekNotSupported();
			}
		}

		// Token: 0x060002DD RID: 733 RVA: 0x001CD944 File Offset: 0x001CCD44
		public override void Flush()
		{
			this._lengthWritten = ValueUtilsSmi.SetBytesLength(this._sink, this._setters, this._ordinal, this._metaData, this._lengthWritten);
		}

		// Token: 0x060002DE RID: 734 RVA: 0x001CD97C File Offset: 0x001CCD7C
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw SQL.StreamSeekNotSupported();
		}

		// Token: 0x060002DF RID: 735 RVA: 0x001CD990 File Offset: 0x001CCD90
		public override void SetLength(long value)
		{
			if (value < 0L)
			{
				throw ADP.ArgumentOutOfRange("value");
			}
			ValueUtilsSmi.SetBytesLength(this._sink, this._setters, this._ordinal, this._metaData, value);
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x001CD9CC File Offset: 0x001CCDCC
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw SQL.StreamReadNotSupported();
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x001CD9E0 File Offset: 0x001CCDE0
		public override void Write(byte[] buffer, int offset, int count)
		{
			this._lengthWritten += ValueUtilsSmi.SetBytes(this._sink, this._setters, this._ordinal, this._metaData, this._lengthWritten, buffer, offset, count);
		}

		// Token: 0x040005EE RID: 1518
		private SmiEventSink_Default _sink;

		// Token: 0x040005EF RID: 1519
		private ITypedSettersV3 _setters;

		// Token: 0x040005F0 RID: 1520
		private int _ordinal;

		// Token: 0x040005F1 RID: 1521
		private long _lengthWritten;

		// Token: 0x040005F2 RID: 1522
		private SmiMetaData _metaData;
	}
}
