using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes
{
	// Token: 0x02000346 RID: 838
	[XmlSchemaProvider("GetXsdType")]
	[Serializable]
	public sealed class SqlChars : INullable, IXmlSerializable, ISerializable
	{
		// Token: 0x06002C21 RID: 11297 RVA: 0x002A445C File Offset: 0x002A385C
		public SqlChars()
		{
			this.SetNull();
		}

		// Token: 0x06002C22 RID: 11298 RVA: 0x002A4478 File Offset: 0x002A3878
		public SqlChars(char[] buffer)
		{
			this.m_rgchBuf = buffer;
			this.m_stream = null;
			if (this.m_rgchBuf == null)
			{
				this.m_state = SqlBytesCharsState.Null;
				this.m_lCurLen = -1L;
			}
			else
			{
				this.m_state = SqlBytesCharsState.Buffer;
				this.m_lCurLen = (long)this.m_rgchBuf.Length;
			}
			this.m_rgchWorkBuf = null;
		}

		// Token: 0x06002C23 RID: 11299 RVA: 0x002A44D0 File Offset: 0x002A38D0
		public SqlChars(SqlString value)
			: this(value.IsNull ? null : value.Value.ToCharArray())
		{
		}

		// Token: 0x06002C24 RID: 11300 RVA: 0x002A44FC File Offset: 0x002A38FC
		internal SqlChars(SqlStreamChars s)
		{
			this.m_rgchBuf = null;
			this.m_lCurLen = -1L;
			this.m_stream = s;
			this.m_state = ((s == null) ? SqlBytesCharsState.Null : SqlBytesCharsState.Stream);
			this.m_rgchWorkBuf = null;
		}

		// Token: 0x06002C25 RID: 11301 RVA: 0x002A453C File Offset: 0x002A393C
		private SqlChars(SerializationInfo info, StreamingContext context)
		{
			this.m_stream = null;
			this.m_rgchWorkBuf = null;
			if (info.GetBoolean("IsNull"))
			{
				this.m_state = SqlBytesCharsState.Null;
				this.m_rgchBuf = null;
				return;
			}
			this.m_state = SqlBytesCharsState.Buffer;
			this.m_rgchBuf = (char[])info.GetValue("data", typeof(char[]));
			this.m_lCurLen = (long)this.m_rgchBuf.Length;
		}

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x06002C26 RID: 11302 RVA: 0x002A45B0 File Offset: 0x002A39B0
		public bool IsNull
		{
			get
			{
				return this.m_state == SqlBytesCharsState.Null;
			}
		}

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x06002C27 RID: 11303 RVA: 0x002A45C8 File Offset: 0x002A39C8
		public char[] Buffer
		{
			get
			{
				if (this.FStream())
				{
					this.CopyStreamToBuffer();
				}
				return this.m_rgchBuf;
			}
		}

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x06002C28 RID: 11304 RVA: 0x002A45EC File Offset: 0x002A39EC
		public long Length
		{
			get
			{
				SqlBytesCharsState state = this.m_state;
				if (state == SqlBytesCharsState.Null)
				{
					throw new SqlNullValueException();
				}
				if (state != SqlBytesCharsState.Stream)
				{
					return this.m_lCurLen;
				}
				return this.m_stream.Length;
			}
		}

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x06002C29 RID: 11305 RVA: 0x002A4624 File Offset: 0x002A3A24
		public long MaxLength
		{
			get
			{
				SqlBytesCharsState state = this.m_state;
				if (state == SqlBytesCharsState.Stream)
				{
					return -1L;
				}
				if (this.m_rgchBuf != null)
				{
					return (long)this.m_rgchBuf.Length;
				}
				return -1L;
			}
		}

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x06002C2A RID: 11306 RVA: 0x002A4654 File Offset: 0x002A3A54
		public char[] Value
		{
			get
			{
				SqlBytesCharsState state = this.m_state;
				if (state != SqlBytesCharsState.Null)
				{
					char[] array;
					if (state != SqlBytesCharsState.Stream)
					{
						array = new char[this.m_lCurLen];
						Array.Copy(this.m_rgchBuf, array, (int)this.m_lCurLen);
					}
					else
					{
						if (this.m_stream.Length > 2147483647L)
						{
							throw new SqlTypeException(Res.GetString("SqlMisc_BufferInsufficientMessage"));
						}
						array = new char[this.m_stream.Length];
						if (this.m_stream.Position != 0L)
						{
							this.m_stream.Seek(0L, SeekOrigin.Begin);
						}
						this.m_stream.Read(array, 0, checked((int)this.m_stream.Length));
					}
					return array;
				}
				throw new SqlNullValueException();
			}
		}

		// Token: 0x1700072E RID: 1838
		public char this[long offset]
		{
			get
			{
				if (offset < 0L || offset >= this.Length)
				{
					throw new ArgumentOutOfRangeException("offset");
				}
				if (this.m_rgchWorkBuf == null)
				{
					this.m_rgchWorkBuf = new char[1];
				}
				this.Read(offset, this.m_rgchWorkBuf, 0, 1);
				return this.m_rgchWorkBuf[0];
			}
			set
			{
				if (this.m_rgchWorkBuf == null)
				{
					this.m_rgchWorkBuf = new char[1];
				}
				this.m_rgchWorkBuf[0] = value;
				this.Write(offset, this.m_rgchWorkBuf, 0, 1);
			}
		}

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x06002C2D RID: 11309 RVA: 0x002A479C File Offset: 0x002A3B9C
		// (set) Token: 0x06002C2E RID: 11310 RVA: 0x002A47C0 File Offset: 0x002A3BC0
		internal SqlStreamChars Stream
		{
			get
			{
				if (!this.FStream())
				{
					return new StreamOnSqlChars(this);
				}
				return this.m_stream;
			}
			set
			{
				this.m_lCurLen = -1L;
				this.m_stream = value;
				this.m_state = ((value == null) ? SqlBytesCharsState.Null : SqlBytesCharsState.Stream);
			}
		}

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x06002C2F RID: 11311 RVA: 0x002A47EC File Offset: 0x002A3BEC
		public StorageState Storage
		{
			get
			{
				switch (this.m_state)
				{
				case SqlBytesCharsState.Null:
					throw new SqlNullValueException();
				case SqlBytesCharsState.Buffer:
					return StorageState.Buffer;
				case SqlBytesCharsState.Stream:
					return StorageState.Stream;
				}
				return StorageState.UnmanagedBuffer;
			}
		}

		// Token: 0x06002C30 RID: 11312 RVA: 0x002A4824 File Offset: 0x002A3C24
		public void SetNull()
		{
			this.m_lCurLen = -1L;
			this.m_stream = null;
			this.m_state = SqlBytesCharsState.Null;
		}

		// Token: 0x06002C31 RID: 11313 RVA: 0x002A4848 File Offset: 0x002A3C48
		public void SetLength(long value)
		{
			if (value < 0L)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			if (this.FStream())
			{
				this.m_stream.SetLength(value);
				return;
			}
			if (this.m_rgchBuf == null)
			{
				throw new SqlTypeException(Res.GetString("SqlMisc_NoBufferMessage"));
			}
			if (value > (long)this.m_rgchBuf.Length)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			if (this.IsNull)
			{
				this.m_state = SqlBytesCharsState.Buffer;
			}
			this.m_lCurLen = value;
		}

		// Token: 0x06002C32 RID: 11314 RVA: 0x002A48C0 File Offset: 0x002A3CC0
		public long Read(long offset, char[] buffer, int offsetInBuffer, int count)
		{
			if (this.IsNull)
			{
				throw new SqlNullValueException();
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset > this.Length || offset < 0L)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offsetInBuffer > buffer.Length || offsetInBuffer < 0)
			{
				throw new ArgumentOutOfRangeException("offsetInBuffer");
			}
			if (count < 0 || count > buffer.Length - offsetInBuffer)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if ((long)count > this.Length - offset)
			{
				count = (int)(this.Length - offset);
			}
			if (count != 0)
			{
				SqlBytesCharsState state = this.m_state;
				if (state == SqlBytesCharsState.Stream)
				{
					if (this.m_stream.Position != offset)
					{
						this.m_stream.Seek(offset, SeekOrigin.Begin);
					}
					this.m_stream.Read(buffer, offsetInBuffer, count);
				}
				else
				{
					Array.Copy(this.m_rgchBuf, offset, buffer, (long)offsetInBuffer, (long)count);
				}
			}
			return (long)count;
		}

		// Token: 0x06002C33 RID: 11315 RVA: 0x002A499C File Offset: 0x002A3D9C
		public void Write(long offset, char[] buffer, int offsetInBuffer, int count)
		{
			if (this.FStream())
			{
				if (this.m_stream.Position != offset)
				{
					this.m_stream.Seek(offset, SeekOrigin.Begin);
				}
				this.m_stream.Write(buffer, offsetInBuffer, count);
				return;
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (this.m_rgchBuf == null)
			{
				throw new SqlTypeException(Res.GetString("SqlMisc_NoBufferMessage"));
			}
			if (offset < 0L)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset > (long)this.m_rgchBuf.Length)
			{
				throw new SqlTypeException(Res.GetString("SqlMisc_BufferInsufficientMessage"));
			}
			if (offsetInBuffer < 0 || offsetInBuffer > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offsetInBuffer");
			}
			if (count < 0 || count > buffer.Length - offsetInBuffer)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if ((long)count > (long)this.m_rgchBuf.Length - offset)
			{
				throw new SqlTypeException(Res.GetString("SqlMisc_BufferInsufficientMessage"));
			}
			if (this.IsNull)
			{
				if (offset != 0L)
				{
					throw new SqlTypeException(Res.GetString("SqlMisc_WriteNonZeroOffsetOnNullMessage"));
				}
				this.m_lCurLen = 0L;
				this.m_state = SqlBytesCharsState.Buffer;
			}
			else if (offset > this.m_lCurLen)
			{
				throw new SqlTypeException(Res.GetString("SqlMisc_WriteOffsetLargerThanLenMessage"));
			}
			if (count != 0)
			{
				Array.Copy(buffer, (long)offsetInBuffer, this.m_rgchBuf, offset, (long)count);
				if (this.m_lCurLen < offset + (long)count)
				{
					this.m_lCurLen = offset + (long)count;
				}
			}
		}

		// Token: 0x06002C34 RID: 11316 RVA: 0x002A4AF4 File Offset: 0x002A3EF4
		public SqlString ToSqlString()
		{
			if (!this.IsNull)
			{
				return new string(this.Value);
			}
			return SqlString.Null;
		}

		// Token: 0x06002C35 RID: 11317 RVA: 0x002A4B20 File Offset: 0x002A3F20
		public static explicit operator SqlString(SqlChars value)
		{
			return value.ToSqlString();
		}

		// Token: 0x06002C36 RID: 11318 RVA: 0x002A4B34 File Offset: 0x002A3F34
		public static explicit operator SqlChars(SqlString value)
		{
			return new SqlChars(value);
		}

		// Token: 0x06002C37 RID: 11319 RVA: 0x002A4B48 File Offset: 0x002A3F48
		[Conditional("DEBUG")]
		private void AssertValid()
		{
			if (this.IsNull)
			{
			}
		}

		// Token: 0x06002C38 RID: 11320 RVA: 0x002A4B60 File Offset: 0x002A3F60
		internal bool FStream()
		{
			return this.m_state == SqlBytesCharsState.Stream;
		}

		// Token: 0x06002C39 RID: 11321 RVA: 0x002A4B78 File Offset: 0x002A3F78
		private void CopyStreamToBuffer()
		{
			long length = this.m_stream.Length;
			if (length >= 2147483647L)
			{
				throw new SqlTypeException(Res.GetString("SqlMisc_BufferInsufficientMessage"));
			}
			if (this.m_rgchBuf == null || (long)this.m_rgchBuf.Length < length)
			{
				this.m_rgchBuf = new char[length];
			}
			if (this.m_stream.Position != 0L)
			{
				this.m_stream.Seek(0L, SeekOrigin.Begin);
			}
			this.m_stream.Read(this.m_rgchBuf, 0, (int)length);
			this.m_stream = null;
			this.m_lCurLen = length;
			this.m_state = SqlBytesCharsState.Buffer;
		}

		// Token: 0x06002C3A RID: 11322 RVA: 0x002A4C14 File Offset: 0x002A4014
		private void SetBuffer(char[] buffer)
		{
			this.m_rgchBuf = buffer;
			this.m_lCurLen = ((this.m_rgchBuf == null) ? (-1L) : ((long)this.m_rgchBuf.Length));
			this.m_stream = null;
			this.m_state = ((this.m_rgchBuf == null) ? SqlBytesCharsState.Null : SqlBytesCharsState.Buffer);
		}

		// Token: 0x06002C3B RID: 11323 RVA: 0x002A4C5C File Offset: 0x002A405C
		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		// Token: 0x06002C3C RID: 11324 RVA: 0x002A4C6C File Offset: 0x002A406C
		void IXmlSerializable.ReadXml(XmlReader r)
		{
			string attribute = r.GetAttribute("nil", "http://www.w3.org/2001/XMLSchema-instance");
			if (attribute != null && XmlConvert.ToBoolean(attribute))
			{
				this.SetNull();
				return;
			}
			char[] array = r.ReadElementString().ToCharArray();
			this.SetBuffer(array);
		}

		// Token: 0x06002C3D RID: 11325 RVA: 0x002A4CB4 File Offset: 0x002A40B4
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			if (this.IsNull)
			{
				writer.WriteAttributeString("xsi", "nil", "http://www.w3.org/2001/XMLSchema-instance", "true");
				return;
			}
			char[] buffer = this.Buffer;
			writer.WriteString(new string(buffer, 0, (int)this.Length));
		}

		// Token: 0x06002C3E RID: 11326 RVA: 0x002A4D00 File Offset: 0x002A4100
		public static XmlQualifiedName GetXsdType(XmlSchemaSet schemaSet)
		{
			return new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
		}

		// Token: 0x06002C3F RID: 11327 RVA: 0x002A4D1C File Offset: 0x002A411C
		[SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			switch (this.m_state)
			{
			default:
				info.AddValue("IsNull", true);
				return;
			case SqlBytesCharsState.Buffer:
				break;
			case SqlBytesCharsState.Stream:
				this.CopyStreamToBuffer();
				break;
			}
			info.AddValue("IsNull", false);
			info.AddValue("data", this.m_rgchBuf);
		}

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06002C40 RID: 11328 RVA: 0x002A4D78 File Offset: 0x002A4178
		public static SqlChars Null
		{
			get
			{
				return new SqlChars(null);
			}
		}

		// Token: 0x04001C6F RID: 7279
		private const long x_lMaxLen = 2147483647L;

		// Token: 0x04001C70 RID: 7280
		private const long x_lNull = -1L;

		// Token: 0x04001C71 RID: 7281
		internal char[] m_rgchBuf;

		// Token: 0x04001C72 RID: 7282
		private long m_lCurLen;

		// Token: 0x04001C73 RID: 7283
		private IntPtr m_pchData;

		// Token: 0x04001C74 RID: 7284
		internal SqlStreamChars m_stream;

		// Token: 0x04001C75 RID: 7285
		private SqlBytesCharsState m_state;

		// Token: 0x04001C76 RID: 7286
		private char[] m_rgchWorkBuf;
	}
}
