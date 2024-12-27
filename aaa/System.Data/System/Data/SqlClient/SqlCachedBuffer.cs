using System;
using System.Collections;
using System.Data.SqlTypes;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Xml;

namespace System.Data.SqlClient
{
	// Token: 0x020002BB RID: 699
	internal sealed class SqlCachedBuffer : INullable
	{
		// Token: 0x06002360 RID: 9056 RVA: 0x00271CE4 File Offset: 0x002710E4
		internal SqlCachedBuffer(SqlMetaDataPriv metadata, TdsParser parser, TdsParserStateObject stateObj)
		{
			this._cachedBytes = new ArrayList();
			ulong num = parser.PlpBytesLeft(stateObj);
			while (num != 0UL)
			{
				do
				{
					int num2 = ((num > 2048UL) ? 2048 : ((int)num));
					byte[] array = new byte[num2];
					num2 = stateObj.ReadPlpBytes(ref array, 0, num2);
					if (this._cachedBytes.Count == 0)
					{
						this.AddByteOrderMark(array);
					}
					this._cachedBytes.Add(array);
					num -= (ulong)((long)num2);
				}
				while (num > 0UL);
				num = parser.PlpBytesLeft(stateObj);
				if (num <= 0UL)
				{
					return;
				}
			}
		}

		// Token: 0x06002361 RID: 9057 RVA: 0x00271D70 File Offset: 0x00271170
		internal SqlCachedBuffer(SqlDataReader dataRdr, int columnOrdinal, long startPosition)
		{
			this._cachedBytes = new ArrayList();
			long num = startPosition;
			int num2;
			do
			{
				byte[] array = new byte[2048];
				num2 = (int)dataRdr.GetBytesInternal(columnOrdinal, num, array, 0, 2048);
				num += (long)num2;
				if (this._cachedBytes.Count == 0)
				{
					this.AddByteOrderMark(array, num2);
				}
				if (0 < num2)
				{
					if (num2 < array.Length)
					{
						byte[] array2 = new byte[num2];
						Buffer.BlockCopy(array, 0, array2, 0, num2);
						array = array2;
					}
					this._cachedBytes.Add(array);
				}
			}
			while (0 < num2);
		}

		// Token: 0x06002362 RID: 9058 RVA: 0x00271DF4 File Offset: 0x002711F4
		private void AddByteOrderMark(byte[] byteArr)
		{
			this.AddByteOrderMark(byteArr, byteArr.Length);
		}

		// Token: 0x06002363 RID: 9059 RVA: 0x00271E0C File Offset: 0x0027120C
		private void AddByteOrderMark(byte[] byteArr, int length)
		{
			int num = 65279;
			if (length >= 2 && byteArr[0] == 223 && byteArr[1] == 255)
			{
				num = 0;
			}
			if (num != 0)
			{
				byte[] array = new byte[2];
				array[0] = (byte)num;
				num >>= 8;
				array[1] = (byte)num;
				this._cachedBytes.Add(array);
			}
		}

		// Token: 0x06002364 RID: 9060 RVA: 0x00271E60 File Offset: 0x00271260
		private SqlCachedBuffer()
		{
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06002365 RID: 9061 RVA: 0x00271E74 File Offset: 0x00271274
		internal ArrayList CachedBytes
		{
			get
			{
				return this._cachedBytes;
			}
		}

		// Token: 0x06002366 RID: 9062 RVA: 0x00271E88 File Offset: 0x00271288
		public override string ToString()
		{
			if (this.IsNull)
			{
				throw new SqlNullValueException();
			}
			if (this._cachedBytes.Count == 0)
			{
				return string.Empty;
			}
			SqlCachedStream sqlCachedStream = new SqlCachedStream(this);
			SqlXml sqlXml = new SqlXml(sqlCachedStream);
			return sqlXml.Value;
		}

		// Token: 0x06002367 RID: 9063 RVA: 0x00271ECC File Offset: 0x002712CC
		internal SqlString ToSqlString()
		{
			if (this.IsNull)
			{
				return SqlString.Null;
			}
			string text = this.ToString();
			return new SqlString(text);
		}

		// Token: 0x06002368 RID: 9064 RVA: 0x00271EF4 File Offset: 0x002712F4
		internal SqlXml ToSqlXml()
		{
			SqlCachedStream sqlCachedStream = new SqlCachedStream(this);
			return new SqlXml(sqlCachedStream);
		}

		// Token: 0x06002369 RID: 9065 RVA: 0x00271F10 File Offset: 0x00271310
		internal XmlReader ToXmlReader()
		{
			SqlCachedStream sqlCachedStream = new SqlCachedStream(this);
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.ConformanceLevel = ConformanceLevel.Fragment;
			MethodInfo method = typeof(XmlReader).GetMethod("CreateSqlReader", BindingFlags.Static | BindingFlags.NonPublic);
			object[] array = new object[3];
			array[0] = sqlCachedStream;
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

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x0600236A RID: 9066 RVA: 0x00271F9C File Offset: 0x0027139C
		public bool IsNull
		{
			get
			{
				return this._cachedBytes == null;
			}
		}

		// Token: 0x040016FA RID: 5882
		private const int _maxChunkSize = 2048;

		// Token: 0x040016FB RID: 5883
		private ArrayList _cachedBytes;

		// Token: 0x040016FC RID: 5884
		public static readonly SqlCachedBuffer Null = new SqlCachedBuffer();
	}
}
