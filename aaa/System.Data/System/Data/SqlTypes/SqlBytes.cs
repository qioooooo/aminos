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
	// Token: 0x02000344 RID: 836
	[XmlSchemaProvider("GetXsdType")]
	[Serializable]
	public sealed class SqlBytes : INullable, IXmlSerializable, ISerializable
	{
		// Token: 0x06002BF0 RID: 11248 RVA: 0x002A36CC File Offset: 0x002A2ACC
		public SqlBytes()
		{
			this.SetNull();
		}

		// Token: 0x06002BF1 RID: 11249 RVA: 0x002A36E8 File Offset: 0x002A2AE8
		public SqlBytes(byte[] buffer)
		{
			this.m_rgbBuf = buffer;
			this.m_stream = null;
			if (this.m_rgbBuf == null)
			{
				this.m_state = SqlBytesCharsState.Null;
				this.m_lCurLen = -1L;
			}
			else
			{
				this.m_state = SqlBytesCharsState.Buffer;
				this.m_lCurLen = (long)this.m_rgbBuf.Length;
			}
			this.m_rgbWorkBuf = null;
		}

		// Token: 0x06002BF2 RID: 11250 RVA: 0x002A3740 File Offset: 0x002A2B40
		public SqlBytes(SqlBinary value)
			: this(value.IsNull ? null : value.Value)
		{
		}

		// Token: 0x06002BF3 RID: 11251 RVA: 0x002A3768 File Offset: 0x002A2B68
		public SqlBytes(Stream s)
		{
			this.m_rgbBuf = null;
			this.m_lCurLen = -1L;
			this.m_stream = s;
			this.m_state = ((s == null) ? SqlBytesCharsState.Null : SqlBytesCharsState.Stream);
			this.m_rgbWorkBuf = null;
		}

		// Token: 0x06002BF4 RID: 11252 RVA: 0x002A37A8 File Offset: 0x002A2BA8
		private SqlBytes(SerializationInfo info, StreamingContext context)
		{
			this.m_stream = null;
			this.m_rgbWorkBuf = null;
			if (info.GetBoolean("IsNull"))
			{
				this.m_state = SqlBytesCharsState.Null;
				this.m_rgbBuf = null;
				return;
			}
			this.m_state = SqlBytesCharsState.Buffer;
			this.m_rgbBuf = (byte[])info.GetValue("data", typeof(byte[]));
			this.m_lCurLen = (long)this.m_rgbBuf.Length;
		}

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x06002BF5 RID: 11253 RVA: 0x002A381C File Offset: 0x002A2C1C
		public bool IsNull
		{
			get
			{
				return this.m_state == SqlBytesCharsState.Null;
			}
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x06002BF6 RID: 11254 RVA: 0x002A3834 File Offset: 0x002A2C34
		public byte[] Buffer
		{
			get
			{
				if (this.FStream())
				{
					this.CopyStreamToBuffer();
				}
				return this.m_rgbBuf;
			}
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x06002BF7 RID: 11255 RVA: 0x002A3858 File Offset: 0x002A2C58
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

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x06002BF8 RID: 11256 RVA: 0x002A3890 File Offset: 0x002A2C90
		public long MaxLength
		{
			get
			{
				SqlBytesCharsState state = this.m_state;
				if (state == SqlBytesCharsState.Stream)
				{
					return -1L;
				}
				if (this.m_rgbBuf != null)
				{
					return (long)this.m_rgbBuf.Length;
				}
				return -1L;
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x06002BF9 RID: 11257 RVA: 0x002A38C0 File Offset: 0x002A2CC0
		public byte[] Value
		{
			get
			{
				SqlBytesCharsState state = this.m_state;
				if (state != SqlBytesCharsState.Null)
				{
					byte[] array;
					if (state != SqlBytesCharsState.Stream)
					{
						array = new byte[this.m_lCurLen];
						Array.Copy(this.m_rgbBuf, array, (int)this.m_lCurLen);
					}
					else
					{
						if (this.m_stream.Length > 2147483647L)
						{
							throw new SqlTypeException(Res.GetString("SqlMisc_BufferInsufficientMessage"));
						}
						array = new byte[this.m_stream.Length];
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

		// Token: 0x17000720 RID: 1824
		public byte this[long offset]
		{
			get
			{
				if (offset < 0L || offset >= this.Length)
				{
					throw new ArgumentOutOfRangeException("offset");
				}
				if (this.m_rgbWorkBuf == null)
				{
					this.m_rgbWorkBuf = new byte[1];
				}
				this.Read(offset, this.m_rgbWorkBuf, 0, 1);
				return this.m_rgbWorkBuf[0];
			}
			set
			{
				if (this.m_rgbWorkBuf == null)
				{
					this.m_rgbWorkBuf = new byte[1];
				}
				this.m_rgbWorkBuf[0] = value;
				this.Write(offset, this.m_rgbWorkBuf, 0, 1);
			}
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x06002BFC RID: 11260 RVA: 0x002A3A08 File Offset: 0x002A2E08
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

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x06002BFD RID: 11261 RVA: 0x002A3A40 File Offset: 0x002A2E40
		// (set) Token: 0x06002BFE RID: 11262 RVA: 0x002A3A64 File Offset: 0x002A2E64
		public Stream Stream
		{
			get
			{
				if (!this.FStream())
				{
					return new StreamOnSqlBytes(this);
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

		// Token: 0x06002BFF RID: 11263 RVA: 0x002A3A90 File Offset: 0x002A2E90
		public void SetNull()
		{
			this.m_lCurLen = -1L;
			this.m_stream = null;
			this.m_state = SqlBytesCharsState.Null;
		}

		// Token: 0x06002C00 RID: 11264 RVA: 0x002A3AB4 File Offset: 0x002A2EB4
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
			if (this.m_rgbBuf == null)
			{
				throw new SqlTypeException(Res.GetString("SqlMisc_NoBufferMessage"));
			}
			if (value > (long)this.m_rgbBuf.Length)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			if (this.IsNull)
			{
				this.m_state = SqlBytesCharsState.Buffer;
			}
			this.m_lCurLen = value;
		}

		// Token: 0x06002C01 RID: 11265 RVA: 0x002A3B2C File Offset: 0x002A2F2C
		public long Read(long offset, byte[] buffer, int offsetInBuffer, int count)
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
					Array.Copy(this.m_rgbBuf, offset, buffer, (long)offsetInBuffer, (long)count);
				}
			}
			return (long)count;
		}

		// Token: 0x06002C02 RID: 11266 RVA: 0x002A3C08 File Offset: 0x002A3008
		public void Write(long offset, byte[] buffer, int offsetInBuffer, int count)
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
			if (this.m_rgbBuf == null)
			{
				throw new SqlTypeException(Res.GetString("SqlMisc_NoBufferMessage"));
			}
			if (offset < 0L)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset > (long)this.m_rgbBuf.Length)
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
			if ((long)count > (long)this.m_rgbBuf.Length - offset)
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
				Array.Copy(buffer, (long)offsetInBuffer, this.m_rgbBuf, offset, (long)count);
				if (this.m_lCurLen < offset + (long)count)
				{
					this.m_lCurLen = offset + (long)count;
				}
			}
		}

		// Token: 0x06002C03 RID: 11267 RVA: 0x002A3D60 File Offset: 0x002A3160
		public SqlBinary ToSqlBinary()
		{
			if (!this.IsNull)
			{
				return new SqlBinary(this.Value);
			}
			return SqlBinary.Null;
		}

		// Token: 0x06002C04 RID: 11268 RVA: 0x002A3D88 File Offset: 0x002A3188
		public static explicit operator SqlBinary(SqlBytes value)
		{
			return value.ToSqlBinary();
		}

		// Token: 0x06002C05 RID: 11269 RVA: 0x002A3D9C File Offset: 0x002A319C
		public static explicit operator SqlBytes(SqlBinary value)
		{
			return new SqlBytes(value);
		}

		// Token: 0x06002C06 RID: 11270 RVA: 0x002A3DB0 File Offset: 0x002A31B0
		[Conditional("DEBUG")]
		private void AssertValid()
		{
			if (this.IsNull)
			{
			}
		}

		// Token: 0x06002C07 RID: 11271 RVA: 0x002A3DC8 File Offset: 0x002A31C8
		private void CopyStreamToBuffer()
		{
			long length = this.m_stream.Length;
			if (length >= 2147483647L)
			{
				throw new SqlTypeException(Res.GetString("SqlMisc_WriteOffsetLargerThanLenMessage"));
			}
			if (this.m_rgbBuf == null || (long)this.m_rgbBuf.Length < length)
			{
				this.m_rgbBuf = new byte[length];
			}
			if (this.m_stream.Position != 0L)
			{
				this.m_stream.Seek(0L, SeekOrigin.Begin);
			}
			this.m_stream.Read(this.m_rgbBuf, 0, (int)length);
			this.m_stream = null;
			this.m_lCurLen = length;
			this.m_state = SqlBytesCharsState.Buffer;
		}

		// Token: 0x06002C08 RID: 11272 RVA: 0x002A3E64 File Offset: 0x002A3264
		internal bool FStream()
		{
			return this.m_state == SqlBytesCharsState.Stream;
		}

		// Token: 0x06002C09 RID: 11273 RVA: 0x002A3E7C File Offset: 0x002A327C
		private void SetBuffer(byte[] buffer)
		{
			this.m_rgbBuf = buffer;
			this.m_lCurLen = ((this.m_rgbBuf == null) ? (-1L) : ((long)this.m_rgbBuf.Length));
			this.m_stream = null;
			this.m_state = ((this.m_rgbBuf == null) ? SqlBytesCharsState.Null : SqlBytesCharsState.Buffer);
		}

		// Token: 0x06002C0A RID: 11274 RVA: 0x002A3EC4 File Offset: 0x002A32C4
		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		// Token: 0x06002C0B RID: 11275 RVA: 0x002A3ED4 File Offset: 0x002A32D4
		void IXmlSerializable.ReadXml(XmlReader r)
		{
			byte[] array = null;
			string attribute = r.GetAttribute("nil", "http://www.w3.org/2001/XMLSchema-instance");
			if (attribute != null && XmlConvert.ToBoolean(attribute))
			{
				this.SetNull();
			}
			else
			{
				string text = r.ReadElementString();
				if (text == null)
				{
					array = new byte[0];
				}
				else
				{
					text = text.Trim();
					if (text.Length == 0)
					{
						array = new byte[0];
					}
					else
					{
						array = Convert.FromBase64String(text);
					}
				}
			}
			this.SetBuffer(array);
		}

		// Token: 0x06002C0C RID: 11276 RVA: 0x002A3F40 File Offset: 0x002A3340
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			if (this.IsNull)
			{
				writer.WriteAttributeString("xsi", "nil", "http://www.w3.org/2001/XMLSchema-instance", "true");
				return;
			}
			byte[] buffer = this.Buffer;
			writer.WriteString(Convert.ToBase64String(buffer, 0, (int)this.Length));
		}

		// Token: 0x06002C0D RID: 11277 RVA: 0x002A3F8C File Offset: 0x002A338C
		public static XmlQualifiedName GetXsdType(XmlSchemaSet schemaSet)
		{
			return new XmlQualifiedName("base64Binary", "http://www.w3.org/2001/XMLSchema");
		}

		// Token: 0x06002C0E RID: 11278 RVA: 0x002A3FA8 File Offset: 0x002A33A8
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
			info.AddValue("data", this.m_rgbBuf);
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x06002C0F RID: 11279 RVA: 0x002A4004 File Offset: 0x002A3404
		public static SqlBytes Null
		{
			get
			{
				return new SqlBytes(null);
			}
		}

		// Token: 0x04001C65 RID: 7269
		private const long x_lMaxLen = 2147483647L;

		// Token: 0x04001C66 RID: 7270
		private const long x_lNull = -1L;

		// Token: 0x04001C67 RID: 7271
		internal byte[] m_rgbBuf;

		// Token: 0x04001C68 RID: 7272
		private long m_lCurLen;

		// Token: 0x04001C69 RID: 7273
		private IntPtr m_pbData;

		// Token: 0x04001C6A RID: 7274
		internal Stream m_stream;

		// Token: 0x04001C6B RID: 7275
		private SqlBytesCharsState m_state;

		// Token: 0x04001C6C RID: 7276
		private byte[] m_rgbWorkBuf;
	}
}
