using System;
using System.Data.Common;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Data.OracleClient
{
	// Token: 0x0200004D RID: 77
	internal sealed class OracleColumn
	{
		// Token: 0x06000283 RID: 643 RVA: 0x0005CA08 File Offset: 0x0005BE08
		internal OracleColumn(OciStatementHandle statementHandle, int ordinal, OciErrorHandle errorHandle, OracleConnection connection)
		{
			this._ordinal = ordinal;
			this._describeHandle = statementHandle.GetDescriptor(this._ordinal, errorHandle);
			this._connection = connection;
			this._connectionCloseCount = connection.CloseCount;
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000284 RID: 644 RVA: 0x0005CA4C File Offset: 0x0005BE4C
		internal string ColumnName
		{
			get
			{
				return this._columnName;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000285 RID: 645 RVA: 0x0005CA60 File Offset: 0x0005BE60
		internal bool IsNullable
		{
			get
			{
				return this._isNullable;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000286 RID: 646 RVA: 0x0005CA74 File Offset: 0x0005BE74
		internal bool IsLob
		{
			get
			{
				return this._metaType.IsLob;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000287 RID: 647 RVA: 0x0005CA8C File Offset: 0x0005BE8C
		internal bool IsLong
		{
			get
			{
				return this._metaType.IsLong;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000288 RID: 648 RVA: 0x0005CAA4 File Offset: 0x0005BEA4
		internal OracleType OracleType
		{
			get
			{
				return this._metaType.OracleType;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000289 RID: 649 RVA: 0x0005CABC File Offset: 0x0005BEBC
		internal int Ordinal
		{
			get
			{
				return this._ordinal;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600028A RID: 650 RVA: 0x0005CAD0 File Offset: 0x0005BED0
		internal byte Precision
		{
			get
			{
				return this._precision;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600028B RID: 651 RVA: 0x0005CAE4 File Offset: 0x0005BEE4
		internal byte Scale
		{
			get
			{
				return this._scale;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600028C RID: 652 RVA: 0x0005CAF8 File Offset: 0x0005BEF8
		internal int SchemaTableSize
		{
			get
			{
				if (!this._bindAsUTF16 || this._metaType.IsLong)
				{
					return this._byteSize;
				}
				return this._byteSize / 2;
			}
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0005CB2C File Offset: 0x0005BF2C
		private int _callback_GetColumnPiecewise(IntPtr octxp, IntPtr defnp, uint iter, IntPtr bufpp, IntPtr alenp, IntPtr piecep, IntPtr indpp, IntPtr rcodep)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc._callback_GetColumnPiecewise|ADV|OCI> octxp=0x%-07Ix defnp=0x%-07Ix iter=%-2d bufpp=0x%-07Ix alenp=0x%-07Ix piecep=0x%-07Ix indpp=0x%-07Ix rcodep=0x%-07Ix\n", octxp, defnp, (int)iter, bufpp, alenp, piecep, indpp, rcodep);
			}
			IntPtr intPtr = ((-1 != this._indicatorOffset) ? this._rowBuffer.DangerousGetDataPtr(this._indicatorOffset) : IntPtr.Zero);
			IntPtr intPtr2;
			IntPtr chunk = this._longBuffer.GetChunk(out intPtr2);
			Marshal.WriteIntPtr(bufpp, chunk);
			Marshal.WriteIntPtr(indpp, intPtr);
			Marshal.WriteIntPtr(alenp, intPtr2);
			Marshal.WriteInt32(intPtr2, NativeBuffer_LongColumnData.MaxChunkSize);
			GC.KeepAlive(this);
			return -24200;
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0005CBB8 File Offset: 0x0005BFB8
		internal void Bind(OciStatementHandle statementHandle, NativeBuffer_RowBuffer buffer, OciErrorHandle errorHandle, int rowBufferLength)
		{
			OciDefineHandle ociDefineHandle = null;
			OCI.MODE mode = OCI.MODE.OCI_DEFAULT;
			OCI.DATATYPE ociType = this._metaType.OciType;
			this._rowBuffer = buffer;
			int num;
			if (this._metaType.IsLong)
			{
				mode = OCI.MODE.OCI_OBJECT;
				num = int.MaxValue;
			}
			else
			{
				num = this._byteSize;
			}
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			IntPtr intPtr3 = this._rowBuffer.DangerousGetDataPtr(this._valueOffset);
			if (-1 != this._indicatorOffset)
			{
				intPtr = this._rowBuffer.DangerousGetDataPtr(this._indicatorOffset);
			}
			if (-1 != this._lengthOffset && !this._metaType.IsLong)
			{
				intPtr2 = this._rowBuffer.DangerousGetDataPtr(this._lengthOffset);
			}
			checked
			{
				try
				{
					IntPtr intPtr4;
					int num2 = TracedNativeMethods.OCIDefineByPos(statementHandle, out intPtr4, errorHandle, (uint)this._ordinal + 1U, intPtr3, num, ociType, intPtr, intPtr2, IntPtr.Zero, mode);
					if (num2 != 0)
					{
						this._connection.CheckError(errorHandle, num2);
					}
					ociDefineHandle = new OciDefineHandle(statementHandle, intPtr4);
					if (rowBufferLength != 0)
					{
						uint num3 = (uint)rowBufferLength;
						uint num4 = ((-1 != this._indicatorOffset) ? num3 : 0U);
						uint num5 = ((-1 != this._lengthOffset && !this._metaType.IsLong) ? num3 : 0U);
						num2 = TracedNativeMethods.OCIDefineArrayOfStruct(ociDefineHandle, errorHandle, num3, num4, num5, 0U);
						if (num2 != 0)
						{
							this._connection.CheckError(errorHandle, num2);
						}
					}
					if (this._metaType.UsesNationalCharacterSet)
					{
						ociDefineHandle.SetAttribute(OCI.ATTR.OCI_ATTR_CHARSET_FORM, 2, errorHandle);
					}
					if (!this._connection.UnicodeEnabled && this._bindAsUTF16)
					{
						ociDefineHandle.SetAttribute(OCI.ATTR.OCI_ATTR_CHARSET_ID, 1000, errorHandle);
					}
					if (this._metaType.IsLong)
					{
						this._rowBuffer.WriteIntPtr(this._valueOffset, IntPtr.Zero);
						this._callback = new OCI.Callback.OCICallbackDefine(this._callback_GetColumnPiecewise);
						num2 = TracedNativeMethods.OCIDefineDynamic(ociDefineHandle, errorHandle, IntPtr.Zero, this._callback);
						if (num2 != 0)
						{
							this._connection.CheckError(errorHandle, num2);
						}
					}
				}
				finally
				{
					NativeBuffer.SafeDispose(ref this._longBuffer);
					OciHandle.SafeDispose(ref ociDefineHandle);
				}
			}
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0005CDB4 File Offset: 0x0005C1B4
		internal bool Describe(ref int offset, OracleConnection connection, OciErrorHandle errorHandle)
		{
			bool flag = false;
			bool flag2 = false;
			this._describeHandle.GetAttribute(OCI.ATTR.OCI_ATTR_SQLCODE, out this._columnName, errorHandle, this._connection);
			short num;
			this._describeHandle.GetAttribute(OCI.ATTR.OCI_ATTR_OBJECT, out num, errorHandle);
			byte b;
			this._describeHandle.GetAttribute(OCI.ATTR.OCI_ATTR_SESSION, out b, errorHandle);
			this._isNullable = 0 != b;
			OCI.DATATYPE datatype = (OCI.DATATYPE)num;
			OCI.DATATYPE datatype2 = datatype;
			if (datatype2 <= OCI.DATATYPE.CHAR)
			{
				if (datatype2 <= OCI.DATATYPE.DATE)
				{
					switch (datatype2)
					{
					case OCI.DATATYPE.VARCHAR2:
						break;
					case OCI.DATATYPE.NUMBER:
						this._metaType = MetaType.GetMetaTypeForType(OracleType.Number);
						this._byteSize = this._metaType.BindSize;
						this._describeHandle.GetAttribute(OCI.ATTR.OCI_ATTR_ENV, out this._precision, errorHandle);
						this._describeHandle.GetAttribute(OCI.ATTR.OCI_ATTR_SERVER, out this._scale, errorHandle);
						goto IL_043B;
					default:
						switch (datatype2)
						{
						case OCI.DATATYPE.LONG:
							this._metaType = MetaType.GetMetaTypeForType(OracleType.LongVarChar);
							this._byteSize = this._metaType.BindSize;
							flag = true;
							flag2 = true;
							this._bindAsUTF16 = connection.ServerVersionAtLeastOracle8;
							goto IL_043B;
						case (OCI.DATATYPE)9:
						case (OCI.DATATYPE)10:
							goto IL_0434;
						case OCI.DATATYPE.ROWID:
							goto IL_0318;
						case OCI.DATATYPE.DATE:
							this._metaType = MetaType.GetMetaTypeForType(OracleType.DateTime);
							this._byteSize = this._metaType.BindSize;
							flag = true;
							goto IL_043B;
						default:
							goto IL_0434;
						}
						break;
					}
				}
				else
				{
					switch (datatype2)
					{
					case OCI.DATATYPE.RAW:
						this._metaType = MetaType.GetMetaTypeForType(OracleType.Raw);
						this._describeHandle.GetAttribute(OCI.ATTR.OCI_ATTR_FNCODE, out this._byteSize, errorHandle);
						flag = true;
						goto IL_043B;
					case OCI.DATATYPE.LONGRAW:
						this._metaType = MetaType.GetMetaTypeForType(OracleType.LongRaw);
						this._byteSize = this._metaType.BindSize;
						flag = true;
						flag2 = true;
						goto IL_043B;
					default:
						if (datatype2 != OCI.DATATYPE.CHAR)
						{
							goto IL_0434;
						}
						break;
					}
				}
				this._describeHandle.GetAttribute(OCI.ATTR.OCI_ATTR_FNCODE, out this._byteSize, errorHandle);
				this._describeHandle.GetAttribute(OCI.ATTR.OCI_ATTR_CHARSET_FORM, out b, errorHandle);
				OCI.CHARSETFORM charsetform = (OCI.CHARSETFORM)b;
				this._bindAsUTF16 = connection.ServerVersionAtLeastOracle8;
				int num2;
				if (connection.ServerVersionAtLeastOracle9i && OCI.ClientVersionAtLeastOracle9i)
				{
					this._describeHandle.GetAttribute(OCI.ATTR.OCI_ATTR_CHAR_SIZE, out num, errorHandle);
					num2 = (int)num;
				}
				else
				{
					num2 = this._byteSize;
				}
				if (charsetform == OCI.CHARSETFORM.SQLCS_NCHAR)
				{
					this._metaType = MetaType.GetMetaTypeForType((OCI.DATATYPE.CHAR == datatype) ? OracleType.NChar : OracleType.NVarChar);
				}
				else
				{
					this._metaType = MetaType.GetMetaTypeForType((OCI.DATATYPE.CHAR == datatype) ? OracleType.Char : OracleType.VarChar);
					if (this._bindAsUTF16)
					{
						this._byteSize *= ADP.CharSize;
					}
				}
				this._byteSize = Math.Max(this._byteSize, num2 * ADP.CharSize);
				flag = true;
				goto IL_043B;
			}
			if (datatype2 <= OCI.DATATYPE.BFILE)
			{
				if (datatype2 != OCI.DATATYPE.ROWID_DESC)
				{
					switch (datatype2)
					{
					case OCI.DATATYPE.CLOB:
						this._describeHandle.GetAttribute(OCI.ATTR.OCI_ATTR_CHARSET_FORM, out b, errorHandle);
						this._metaType = MetaType.GetMetaTypeForType((2 == b) ? OracleType.NClob : OracleType.Clob);
						this._byteSize = this._metaType.BindSize;
						flag2 = true;
						goto IL_043B;
					case OCI.DATATYPE.BLOB:
						this._metaType = MetaType.GetMetaTypeForType(OracleType.Blob);
						this._byteSize = this._metaType.BindSize;
						flag2 = true;
						goto IL_043B;
					case OCI.DATATYPE.BFILE:
						this._metaType = MetaType.GetMetaTypeForType(OracleType.BFile);
						this._byteSize = this._metaType.BindSize;
						flag2 = true;
						goto IL_043B;
					default:
						goto IL_0434;
					}
				}
			}
			else
			{
				switch (datatype2)
				{
				case OCI.DATATYPE.TIMESTAMP:
					this._metaType = MetaType.GetMetaTypeForType(OracleType.Timestamp);
					this._byteSize = this._metaType.BindSize;
					flag = true;
					goto IL_043B;
				case OCI.DATATYPE.TIMESTAMP_TZ:
					this._metaType = MetaType.GetMetaTypeForType(OracleType.TimestampWithTZ);
					this._byteSize = this._metaType.BindSize;
					flag = true;
					goto IL_043B;
				case OCI.DATATYPE.INTERVAL_YM:
					this._metaType = MetaType.GetMetaTypeForType(OracleType.IntervalYearToMonth);
					this._byteSize = this._metaType.BindSize;
					goto IL_043B;
				case OCI.DATATYPE.INTERVAL_DS:
					this._metaType = MetaType.GetMetaTypeForType(OracleType.IntervalDayToSecond);
					this._byteSize = this._metaType.BindSize;
					goto IL_043B;
				default:
					if (datatype2 != OCI.DATATYPE.UROWID)
					{
						if (datatype2 != OCI.DATATYPE.TIMESTAMP_LTZ)
						{
							goto IL_0434;
						}
						this._metaType = MetaType.GetMetaTypeForType(OracleType.TimestampLocal);
						this._byteSize = this._metaType.BindSize;
						flag = true;
						goto IL_043B;
					}
					break;
				}
			}
			IL_0318:
			this._metaType = MetaType.GetMetaTypeForType(OracleType.RowId);
			this._byteSize = this._metaType.BindSize;
			if (connection.UnicodeEnabled)
			{
				this._bindAsUTF16 = true;
				this._byteSize *= ADP.CharSize;
			}
			flag = true;
			goto IL_043B;
			IL_0434:
			throw ADP.TypeNotSupported(datatype);
			IL_043B:
			if (this._isNullable)
			{
				this._indicatorOffset = offset;
				offset += IntPtr.Size;
			}
			else
			{
				this._indicatorOffset = -1;
			}
			if (flag)
			{
				this._lengthOffset = offset;
				offset += IntPtr.Size;
			}
			else
			{
				this._lengthOffset = -1;
			}
			this._valueOffset = offset;
			if (OCI.DATATYPE.LONG == datatype || OCI.DATATYPE.LONGRAW == datatype)
			{
				offset += IntPtr.Size;
			}
			else
			{
				offset += this._byteSize;
			}
			offset = (offset + (IntPtr.Size - 1)) & ~(IntPtr.Size - 1);
			OciHandle.SafeDispose(ref this._describeHandle);
			return flag2;
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0005D288 File Offset: 0x0005C688
		internal void Dispose()
		{
			NativeBuffer.SafeDispose(ref this._longBuffer);
			OciLobLocator.SafeDispose(ref this._lobLocator);
			OciHandle.SafeDispose(ref this._describeHandle);
			this._columnName = null;
			this._metaType = null;
			this._callback = null;
			this._connection = null;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0005D2D4 File Offset: 0x0005C6D4
		internal void FixupLongValueLength(NativeBuffer buffer)
		{
			if (this._longBuffer != null && -1 == this._longLength)
			{
				this._longLength = this._longBuffer.TotalLengthInBytes;
				if (this._bindAsUTF16)
				{
					this._longLength /= 2;
				}
				buffer.WriteInt32(this._lengthOffset, this._longLength);
			}
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0005D32C File Offset: 0x0005C72C
		internal string GetDataTypeName()
		{
			return this._metaType.DataTypeName;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0005D344 File Offset: 0x0005C744
		internal Type GetFieldType()
		{
			return this._metaType.BaseType;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0005D35C File Offset: 0x0005C75C
		internal Type GetFieldOracleType()
		{
			return this._metaType.NoConvertType;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0005D374 File Offset: 0x0005C774
		internal object GetValue(NativeBuffer_RowBuffer buffer)
		{
			if (this.IsDBNull(buffer))
			{
				return DBNull.Value;
			}
			OCI.DATATYPE ociType = this._metaType.OciType;
			if (ociType <= OCI.DATATYPE.LONGRAW)
			{
				if (ociType <= OCI.DATATYPE.LONG)
				{
					if (ociType == OCI.DATATYPE.VARCHAR2)
					{
						goto IL_013F;
					}
					switch (ociType)
					{
					case OCI.DATATYPE.VARNUM:
						return this.GetDecimal(buffer);
					case (OCI.DATATYPE)7:
						goto IL_0154;
					case OCI.DATATYPE.LONG:
						goto IL_013F;
					default:
						goto IL_0154;
					}
				}
				else if (ociType != OCI.DATATYPE.DATE)
				{
					switch (ociType)
					{
					case OCI.DATATYPE.RAW:
					case OCI.DATATYPE.LONGRAW:
					{
						long bytes = this.GetBytes(buffer, 0L, null, 0, 0);
						byte[] array = new byte[bytes];
						this.GetBytes(buffer, 0L, array, 0, (int)bytes);
						return array;
					}
					default:
						goto IL_0154;
					}
				}
			}
			else if (ociType <= OCI.DATATYPE.BFILE)
			{
				if (ociType == OCI.DATATYPE.CHAR)
				{
					goto IL_013F;
				}
				switch (ociType)
				{
				case OCI.DATATYPE.CLOB:
				case OCI.DATATYPE.BLOB:
				{
					object value;
					using (OracleLob oracleLob = this.GetOracleLob(buffer))
					{
						value = oracleLob.Value;
					}
					return value;
				}
				case OCI.DATATYPE.BFILE:
				{
					object value2;
					using (OracleBFile oracleBFile = this.GetOracleBFile(buffer))
					{
						value2 = oracleBFile.Value;
					}
					return value2;
				}
				default:
					goto IL_0154;
				}
			}
			else
			{
				switch (ociType)
				{
				case OCI.DATATYPE.INT_TIMESTAMP:
				case OCI.DATATYPE.INT_TIMESTAMP_TZ:
					break;
				case OCI.DATATYPE.INT_INTERVAL_YM:
					return this.GetInt32(buffer);
				case OCI.DATATYPE.INT_INTERVAL_DS:
					return this.GetTimeSpan(buffer);
				default:
					if (ociType != OCI.DATATYPE.INT_TIMESTAMP_LTZ)
					{
						goto IL_0154;
					}
					break;
				}
			}
			return this.GetDateTime(buffer);
			IL_013F:
			return this.GetString(buffer);
			IL_0154:
			throw ADP.TypeNotSupported(this._metaType.OciType);
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0005D51C File Offset: 0x0005C91C
		internal object GetOracleValue(NativeBuffer_RowBuffer buffer)
		{
			OCI.DATATYPE ociType = this._metaType.OciType;
			if (ociType <= OCI.DATATYPE.LONGRAW)
			{
				if (ociType <= OCI.DATATYPE.LONG)
				{
					if (ociType == OCI.DATATYPE.VARCHAR2)
					{
						goto IL_00E1;
					}
					switch (ociType)
					{
					case OCI.DATATYPE.VARNUM:
						return this.GetOracleNumber(buffer);
					case (OCI.DATATYPE)7:
						goto IL_00FB;
					case OCI.DATATYPE.LONG:
						goto IL_00E1;
					default:
						goto IL_00FB;
					}
				}
				else if (ociType != OCI.DATATYPE.DATE)
				{
					switch (ociType)
					{
					case OCI.DATATYPE.RAW:
					case OCI.DATATYPE.LONGRAW:
						return this.GetOracleBinary(buffer);
					default:
						goto IL_00FB;
					}
				}
			}
			else if (ociType <= OCI.DATATYPE.BFILE)
			{
				if (ociType == OCI.DATATYPE.CHAR)
				{
					goto IL_00E1;
				}
				switch (ociType)
				{
				case OCI.DATATYPE.CLOB:
				case OCI.DATATYPE.BLOB:
					return this.GetOracleLob(buffer);
				case OCI.DATATYPE.BFILE:
					return this.GetOracleBFile(buffer);
				default:
					goto IL_00FB;
				}
			}
			else
			{
				switch (ociType)
				{
				case OCI.DATATYPE.INT_TIMESTAMP:
				case OCI.DATATYPE.INT_TIMESTAMP_TZ:
					break;
				case OCI.DATATYPE.INT_INTERVAL_YM:
					return this.GetOracleMonthSpan(buffer);
				case OCI.DATATYPE.INT_INTERVAL_DS:
					return this.GetOracleTimeSpan(buffer);
				default:
					if (ociType != OCI.DATATYPE.INT_TIMESTAMP_LTZ)
					{
						goto IL_00FB;
					}
					break;
				}
			}
			return this.GetOracleDateTime(buffer);
			IL_00E1:
			return this.GetOracleString(buffer);
			IL_00FB:
			throw ADP.TypeNotSupported(this._metaType.OciType);
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0005D634 File Offset: 0x0005CA34
		internal long GetBytes(NativeBuffer_RowBuffer buffer, long fieldOffset, byte[] destinationBuffer, int destinationOffset, int length)
		{
			if (length < 0)
			{
				throw ADP.InvalidDataLength((long)length);
			}
			if (destinationOffset < 0 || (destinationBuffer != null && destinationOffset >= destinationBuffer.Length))
			{
				throw ADP.InvalidDestinationBufferIndex(destinationBuffer.Length, destinationOffset, "bufferoffset");
			}
			if (0L > fieldOffset || (ulong)(-1) < (ulong)fieldOffset)
			{
				throw ADP.InvalidSourceOffset("fieldOffset", 0L, (long)((ulong)(-1)));
			}
			int num3;
			if (this.IsLob)
			{
				OracleType oracleType = this._metaType.OracleType;
				if (OracleType.Blob != oracleType && OracleType.BFile != oracleType)
				{
					throw ADP.InvalidCast();
				}
				if (this.IsDBNull(buffer))
				{
					throw ADP.DataReaderNoData();
				}
				using (OracleLob oracleLob = new OracleLob(this._lobLocator))
				{
					uint num = (uint)oracleLob.Length;
					uint num2 = (uint)fieldOffset;
					if (num2 > num)
					{
						throw ADP.InvalidSourceBufferIndex((int)num, (long)num2, "fieldOffset");
					}
					num3 = (int)(num - num2);
					if (destinationBuffer != null)
					{
						num3 = Math.Min(num3, length);
						if (0 < num3)
						{
							oracleLob.Seek((long)((ulong)num2), SeekOrigin.Begin);
							oracleLob.Read(destinationBuffer, destinationOffset, num3);
						}
					}
					goto IL_0155;
				}
			}
			if (OracleType.Raw != this.OracleType && OracleType.LongRaw != this.OracleType)
			{
				throw ADP.InvalidCast();
			}
			if (this.IsDBNull(buffer))
			{
				throw ADP.DataReaderNoData();
			}
			this.FixupLongValueLength(buffer);
			int length2 = OracleBinary.GetLength(buffer, this._lengthOffset, this._metaType);
			int num4 = (int)fieldOffset;
			num3 = length2 - num4;
			if (destinationBuffer != null)
			{
				num3 = Math.Min(num3, length);
				if (0 < num3)
				{
					OracleBinary.GetBytes(buffer, this._valueOffset, this._metaType, num4, destinationBuffer, destinationOffset, num3);
				}
			}
			IL_0155:
			return (long)Math.Max(0, num3);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0005D7BC File Offset: 0x0005CBBC
		internal long GetChars(NativeBuffer_RowBuffer buffer, long fieldOffset, char[] destinationBuffer, int destinationOffset, int length)
		{
			if (length < 0)
			{
				throw ADP.InvalidDataLength((long)length);
			}
			if (destinationOffset < 0 || (destinationBuffer != null && destinationOffset >= destinationBuffer.Length))
			{
				throw ADP.InvalidDestinationBufferIndex(destinationBuffer.Length, destinationOffset, "bufferoffset");
			}
			if (0L > fieldOffset || (ulong)(-1) < (ulong)fieldOffset)
			{
				throw ADP.InvalidSourceOffset("fieldOffset", 0L, (long)((ulong)(-1)));
			}
			int num2;
			if (this.IsLob)
			{
				OracleType oracleType = this._metaType.OracleType;
				if (OracleType.Clob != oracleType && OracleType.NClob != oracleType && OracleType.BFile != oracleType)
				{
					throw ADP.InvalidCast();
				}
				if (this.IsDBNull(buffer))
				{
					throw ADP.DataReaderNoData();
				}
				using (OracleLob oracleLob = new OracleLob(this._lobLocator))
				{
					string text = (string)oracleLob.Value;
					int length2 = text.Length;
					int num = (int)fieldOffset;
					if (num < 0)
					{
						throw ADP.InvalidSourceBufferIndex(length2, (long)num, "fieldOffset");
					}
					num2 = length2 - num;
					if (destinationBuffer != null)
					{
						num2 = Math.Min(num2, length);
						if (0 < num2)
						{
							char[] array = text.ToCharArray(num, num2);
							Buffer.BlockCopy(array, 0, destinationBuffer, destinationOffset, num2);
						}
					}
					goto IL_0198;
				}
			}
			if (OracleType.Char != this.OracleType && OracleType.VarChar != this.OracleType && OracleType.LongVarChar != this.OracleType && OracleType.NChar != this.OracleType && OracleType.NVarChar != this.OracleType)
			{
				throw ADP.InvalidCast();
			}
			if (this.IsDBNull(buffer))
			{
				throw ADP.DataReaderNoData();
			}
			this.FixupLongValueLength(buffer);
			int length3 = OracleString.GetLength(buffer, this._lengthOffset, this._metaType);
			int num3 = (int)fieldOffset;
			num2 = length3 - num3;
			if (destinationBuffer != null)
			{
				num2 = Math.Min(num2, length);
				if (0 < num2)
				{
					OracleString.GetChars(buffer, this._valueOffset, this._lengthOffset, this._metaType, this._connection, this._bindAsUTF16, num3, destinationBuffer, destinationOffset, num2);
				}
			}
			IL_0198:
			return (long)Math.Max(0, num2);
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0005D988 File Offset: 0x0005CD88
		internal DateTime GetDateTime(NativeBuffer_RowBuffer buffer)
		{
			if (this.IsDBNull(buffer))
			{
				throw ADP.DataReaderNoData();
			}
			if (typeof(DateTime) != this._metaType.BaseType)
			{
				throw ADP.InvalidCast();
			}
			return OracleDateTime.MarshalToDateTime(buffer, this._valueOffset, this._lengthOffset, this._metaType, this._connection);
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0005D9E4 File Offset: 0x0005CDE4
		internal decimal GetDecimal(NativeBuffer_RowBuffer buffer)
		{
			if (typeof(decimal) != this._metaType.BaseType)
			{
				throw ADP.InvalidCast();
			}
			if (this.IsDBNull(buffer))
			{
				throw ADP.DataReaderNoData();
			}
			return OracleNumber.MarshalToDecimal(buffer, this._valueOffset, this._connection);
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0005DA34 File Offset: 0x0005CE34
		internal double GetDouble(NativeBuffer_RowBuffer buffer)
		{
			if (typeof(decimal) != this._metaType.BaseType)
			{
				throw ADP.InvalidCast();
			}
			if (this.IsDBNull(buffer))
			{
				throw ADP.DataReaderNoData();
			}
			decimal num = OracleNumber.MarshalToDecimal(buffer, this._valueOffset, this._connection);
			return (double)num;
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0005DA8C File Offset: 0x0005CE8C
		internal float GetFloat(NativeBuffer_RowBuffer buffer)
		{
			if (typeof(decimal) != this._metaType.BaseType)
			{
				throw ADP.InvalidCast();
			}
			if (this.IsDBNull(buffer))
			{
				throw ADP.DataReaderNoData();
			}
			decimal num = OracleNumber.MarshalToDecimal(buffer, this._valueOffset, this._connection);
			return (float)num;
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0005DAE4 File Offset: 0x0005CEE4
		internal int GetInt32(NativeBuffer_RowBuffer buffer)
		{
			if (typeof(int) != this._metaType.BaseType && typeof(decimal) != this._metaType.BaseType)
			{
				throw ADP.InvalidCast();
			}
			if (this.IsDBNull(buffer))
			{
				throw ADP.DataReaderNoData();
			}
			int num;
			if (typeof(int) == this._metaType.BaseType)
			{
				num = OracleMonthSpan.MarshalToInt32(buffer, this._valueOffset);
			}
			else
			{
				num = OracleNumber.MarshalToInt32(buffer, this._valueOffset, this._connection);
			}
			return num;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0005DB70 File Offset: 0x0005CF70
		internal long GetInt64(NativeBuffer_RowBuffer buffer)
		{
			if (typeof(decimal) != this._metaType.BaseType)
			{
				throw ADP.InvalidCast();
			}
			if (this.IsDBNull(buffer))
			{
				throw ADP.DataReaderNoData();
			}
			return OracleNumber.MarshalToInt64(buffer, this._valueOffset, this._connection);
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0005DBC0 File Offset: 0x0005CFC0
		internal string GetString(NativeBuffer_RowBuffer buffer)
		{
			if (this.IsLob)
			{
				OracleType oracleType = this._metaType.OracleType;
				if (OracleType.Clob != oracleType && OracleType.NClob != oracleType && OracleType.BFile != oracleType)
				{
					throw ADP.InvalidCast();
				}
				if (this.IsDBNull(buffer))
				{
					throw ADP.DataReaderNoData();
				}
				string text;
				using (OracleLob oracleLob = new OracleLob(this._lobLocator))
				{
					text = (string)oracleLob.Value;
				}
				return text;
			}
			else
			{
				if (typeof(string) != this._metaType.BaseType)
				{
					throw ADP.InvalidCast();
				}
				if (this.IsDBNull(buffer))
				{
					throw ADP.DataReaderNoData();
				}
				this.FixupLongValueLength(buffer);
				return OracleString.MarshalToString(buffer, this._valueOffset, this._lengthOffset, this._metaType, this._connection, this._bindAsUTF16, false);
			}
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0005DCA0 File Offset: 0x0005D0A0
		internal TimeSpan GetTimeSpan(NativeBuffer_RowBuffer buffer)
		{
			if (typeof(TimeSpan) != this._metaType.BaseType)
			{
				throw ADP.InvalidCast();
			}
			if (this.IsDBNull(buffer))
			{
				throw ADP.DataReaderNoData();
			}
			return OracleTimeSpan.MarshalToTimeSpan(buffer, this._valueOffset);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0005DCE8 File Offset: 0x0005D0E8
		internal OracleBFile GetOracleBFile(NativeBuffer_RowBuffer buffer)
		{
			if (typeof(OracleBFile) != this._metaType.NoConvertType)
			{
				throw ADP.InvalidCast();
			}
			if (this.IsDBNull(buffer))
			{
				return OracleBFile.Null;
			}
			return new OracleBFile(this._lobLocator);
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0005DD30 File Offset: 0x0005D130
		internal OracleBinary GetOracleBinary(NativeBuffer_RowBuffer buffer)
		{
			if (typeof(OracleBinary) != this._metaType.NoConvertType)
			{
				throw ADP.InvalidCast();
			}
			this.FixupLongValueLength(buffer);
			if (this.IsDBNull(buffer))
			{
				return OracleBinary.Null;
			}
			OracleBinary oracleBinary = new OracleBinary(buffer, this._valueOffset, this._lengthOffset, this._metaType);
			return oracleBinary;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0005DD8C File Offset: 0x0005D18C
		internal OracleDateTime GetOracleDateTime(NativeBuffer_RowBuffer buffer)
		{
			if (typeof(OracleDateTime) != this._metaType.NoConvertType)
			{
				throw ADP.InvalidCast();
			}
			if (this.IsDBNull(buffer))
			{
				return OracleDateTime.Null;
			}
			OracleDateTime oracleDateTime = new OracleDateTime(buffer, this._valueOffset, this._lengthOffset, this._metaType, this._connection);
			return oracleDateTime;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0005DDE8 File Offset: 0x0005D1E8
		internal OracleLob GetOracleLob(NativeBuffer_RowBuffer buffer)
		{
			if (typeof(OracleLob) != this._metaType.NoConvertType)
			{
				throw ADP.InvalidCast();
			}
			if (this.IsDBNull(buffer))
			{
				return OracleLob.Null;
			}
			return new OracleLob(this._lobLocator);
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0005DE30 File Offset: 0x0005D230
		internal OracleMonthSpan GetOracleMonthSpan(NativeBuffer_RowBuffer buffer)
		{
			if (typeof(OracleMonthSpan) != this._metaType.NoConvertType)
			{
				throw ADP.InvalidCast();
			}
			if (this.IsDBNull(buffer))
			{
				return OracleMonthSpan.Null;
			}
			OracleMonthSpan oracleMonthSpan = new OracleMonthSpan(buffer, this._valueOffset);
			return oracleMonthSpan;
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0005DE78 File Offset: 0x0005D278
		internal OracleNumber GetOracleNumber(NativeBuffer_RowBuffer buffer)
		{
			if (typeof(OracleNumber) != this._metaType.NoConvertType)
			{
				throw ADP.InvalidCast();
			}
			if (this.IsDBNull(buffer))
			{
				return OracleNumber.Null;
			}
			OracleNumber oracleNumber = new OracleNumber(buffer, this._valueOffset);
			return oracleNumber;
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0005DEC0 File Offset: 0x0005D2C0
		internal OracleString GetOracleString(NativeBuffer_RowBuffer buffer)
		{
			if (typeof(OracleString) != this._metaType.NoConvertType)
			{
				throw ADP.InvalidCast();
			}
			if (this.IsDBNull(buffer))
			{
				return OracleString.Null;
			}
			this.FixupLongValueLength(buffer);
			OracleString oracleString = new OracleString(buffer, this._valueOffset, this._lengthOffset, this._metaType, this._connection, this._bindAsUTF16, false);
			return oracleString;
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0005DF28 File Offset: 0x0005D328
		internal OracleTimeSpan GetOracleTimeSpan(NativeBuffer_RowBuffer buffer)
		{
			if (typeof(OracleTimeSpan) != this._metaType.NoConvertType)
			{
				throw ADP.InvalidCast();
			}
			if (this.IsDBNull(buffer))
			{
				return OracleTimeSpan.Null;
			}
			OracleTimeSpan oracleTimeSpan = new OracleTimeSpan(buffer, this._valueOffset);
			return oracleTimeSpan;
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0005DF70 File Offset: 0x0005D370
		internal bool IsDBNull(NativeBuffer_RowBuffer buffer)
		{
			return this._isNullable && buffer.ReadInt16(this._indicatorOffset) == -1;
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0005DF98 File Offset: 0x0005D398
		internal void Rebind(OracleConnection connection, ref bool mustRelease, ref SafeHandle handleToBind)
		{
			handleToBind = null;
			OCI.DATATYPE ociType = this._metaType.OciType;
			if (ociType != OCI.DATATYPE.LONG && ociType != OCI.DATATYPE.LONGRAW)
			{
				switch (ociType)
				{
				case OCI.DATATYPE.CLOB:
				case OCI.DATATYPE.BLOB:
				case OCI.DATATYPE.BFILE:
					OciLobLocator.SafeDispose(ref this._lobLocator);
					this._lobLocator = new OciLobLocator(connection, this._metaType.OracleType);
					handleToBind = this._lobLocator.Descriptor;
					break;
				}
			}
			else
			{
				this._rowBuffer.WriteInt32(this._lengthOffset, 0);
				this._longLength = -1;
				if (this._longBuffer != null)
				{
					this._longBuffer.Reset();
				}
				else
				{
					this._longBuffer = new NativeBuffer_LongColumnData();
				}
				handleToBind = this._longBuffer;
			}
			if (handleToBind != null)
			{
				handleToBind.DangerousAddRef(ref mustRelease);
				this._rowBuffer.WriteIntPtr(this._valueOffset, handleToBind.DangerousGetHandle());
			}
		}

		// Token: 0x0400033C RID: 828
		private OciParameterDescriptor _describeHandle;

		// Token: 0x0400033D RID: 829
		private int _ordinal;

		// Token: 0x0400033E RID: 830
		private string _columnName;

		// Token: 0x0400033F RID: 831
		private MetaType _metaType;

		// Token: 0x04000340 RID: 832
		private byte _precision;

		// Token: 0x04000341 RID: 833
		private byte _scale;

		// Token: 0x04000342 RID: 834
		private int _byteSize;

		// Token: 0x04000343 RID: 835
		private bool _isNullable;

		// Token: 0x04000344 RID: 836
		private int _indicatorOffset;

		// Token: 0x04000345 RID: 837
		private int _lengthOffset;

		// Token: 0x04000346 RID: 838
		private int _valueOffset;

		// Token: 0x04000347 RID: 839
		private NativeBuffer_RowBuffer _rowBuffer;

		// Token: 0x04000348 RID: 840
		private NativeBuffer_LongColumnData _longBuffer;

		// Token: 0x04000349 RID: 841
		private int _longLength;

		// Token: 0x0400034A RID: 842
		private OCI.Callback.OCICallbackDefine _callback;

		// Token: 0x0400034B RID: 843
		private OciLobLocator _lobLocator;

		// Token: 0x0400034C RID: 844
		private OracleConnection _connection;

		// Token: 0x0400034D RID: 845
		private int _connectionCloseCount;

		// Token: 0x0400034E RID: 846
		private bool _bindAsUTF16;
	}
}
