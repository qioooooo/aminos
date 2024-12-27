using System;
using System.Data.Common;
using System.Data.SqlTypes;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Data.OracleClient
{
	// Token: 0x02000072 RID: 114
	internal sealed class OracleParameterBinding
	{
		// Token: 0x06000622 RID: 1570 RVA: 0x0006C7B4 File Offset: 0x0006BBB4
		internal OracleParameterBinding(OracleCommand command, OracleParameter parameter)
		{
			this._command = command;
			this._parameter = parameter;
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000623 RID: 1571 RVA: 0x0006C7D8 File Offset: 0x0006BBD8
		internal OracleParameter Parameter
		{
			get
			{
				return this._parameter;
			}
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x0006C7EC File Offset: 0x0006BBEC
		internal void Bind(OciStatementHandle statementHandle, NativeBuffer parameterBuffer, OracleConnection connection, ref bool mustRelease, ref SafeHandle handleToBind)
		{
			if (!OracleParameterBinding.IsDirection(this.Parameter, ParameterDirection.Output) && this.Parameter.Value == null)
			{
				return;
			}
			string parameterName = this.Parameter.ParameterName;
			OciErrorHandle errorHandle = connection.ErrorHandle;
			OciServiceContextHandle serviceContextHandle = connection.ServiceContextHandle;
			int num = 0;
			OCI.INDICATOR indicator = OCI.INDICATOR.OK;
			OCI.DATATYPE ociType = this._bindingMetaType.OciType;
			IntPtr intPtr = parameterBuffer.DangerousGetDataPtr(this._indicatorOffset);
			IntPtr intPtr2 = parameterBuffer.DangerousGetDataPtr(this._lengthOffset);
			IntPtr intPtr3 = parameterBuffer.DangerousGetDataPtr(this._valueOffset);
			OciHandle.SafeDispose(ref this._dateTimeDescriptor);
			if (OracleParameterBinding.IsDirection(this.Parameter, ParameterDirection.Input))
			{
				if (ADP.IsNull(this._coercedValue))
				{
					indicator = OCI.INDICATOR.ISNULL;
					OCI.DATATYPE datatype = ociType;
					switch (datatype)
					{
					case OCI.DATATYPE.INT_TIMESTAMP:
					case OCI.DATATYPE.INT_TIMESTAMP_TZ:
						break;
					default:
						if (datatype != OCI.DATATYPE.INT_TIMESTAMP_LTZ)
						{
							goto IL_01C1;
						}
						break;
					}
					this._dateTimeDescriptor = OracleDateTime.CreateEmptyDescriptor(ociType, connection);
					handleToBind = this._dateTimeDescriptor;
				}
				else
				{
					num = this.PutOracleValue(this._coercedValue, parameterBuffer, this._valueOffset, this._bindingMetaType, connection, ref handleToBind);
				}
			}
			else
			{
				if (this._bindingMetaType.IsVariableLength)
				{
					num = 0;
				}
				else
				{
					num = this._bufferLength;
				}
				OciLobLocator.SafeDispose(ref this._locator);
				OciHandle.SafeDispose(ref this._descriptor);
				OCI.DATATYPE datatype2 = ociType;
				switch (datatype2)
				{
				case OCI.DATATYPE.CLOB:
				case OCI.DATATYPE.BLOB:
				case OCI.DATATYPE.BFILE:
					this._locator = new OciLobLocator(connection, this._bindingMetaType.OracleType);
					handleToBind = this._locator.Descriptor;
					break;
				case (OCI.DATATYPE)115:
					break;
				case OCI.DATATYPE.RSET:
					this._descriptor = new OciStatementHandle(serviceContextHandle);
					handleToBind = this._descriptor;
					break;
				default:
					switch (datatype2)
					{
					case OCI.DATATYPE.INT_TIMESTAMP:
					case OCI.DATATYPE.INT_TIMESTAMP_TZ:
						break;
					default:
						if (datatype2 != OCI.DATATYPE.INT_TIMESTAMP_LTZ)
						{
							goto IL_01C1;
						}
						break;
					}
					this._dateTimeDescriptor = OracleDateTime.CreateEmptyDescriptor(ociType, connection);
					handleToBind = this._dateTimeDescriptor;
					break;
				}
			}
			IL_01C1:
			if (handleToBind != null)
			{
				handleToBind.DangerousAddRef(ref mustRelease);
				parameterBuffer.WriteIntPtr(this._valueOffset, handleToBind.DangerousGetHandle());
			}
			parameterBuffer.WriteInt16(this._indicatorOffset, (short)indicator);
			if (OCI.DATATYPE.LONGVARCHAR == ociType || OCI.DATATYPE.LONGVARRAW == ociType)
			{
				intPtr2 = IntPtr.Zero;
			}
			else if (this._bindAsUCS2)
			{
				parameterBuffer.WriteInt32(this._lengthOffset, num / ADP.CharSize);
			}
			else
			{
				parameterBuffer.WriteInt32(this._lengthOffset, num);
			}
			int num2;
			if (OracleParameterBinding.IsDirection(this.Parameter, ParameterDirection.Output))
			{
				num2 = this._bufferLength;
			}
			else
			{
				num2 = num;
			}
			OCI.DATATYPE datatype3 = ociType;
			OCI.DATATYPE datatype4 = ociType;
			switch (datatype4)
			{
			case OCI.DATATYPE.INT_TIMESTAMP:
				datatype3 = OCI.DATATYPE.TIMESTAMP;
				break;
			case OCI.DATATYPE.INT_TIMESTAMP_TZ:
				datatype3 = OCI.DATATYPE.TIMESTAMP_TZ;
				break;
			default:
				if (datatype4 == OCI.DATATYPE.INT_TIMESTAMP_LTZ)
				{
					datatype3 = OCI.DATATYPE.TIMESTAMP_LTZ;
				}
				break;
			}
			IntPtr intPtr4;
			int num3 = TracedNativeMethods.OCIBindByName(statementHandle, out intPtr4, errorHandle, parameterName, parameterName.Length, intPtr3, num2, datatype3, intPtr, intPtr2, OCI.MODE.OCI_DEFAULT);
			if (num3 != 0)
			{
				this._command.Connection.CheckError(errorHandle, num3);
			}
			this._bindHandle = new OciBindHandle(statementHandle, intPtr4);
			if (this._bindingMetaType.IsCharacterType)
			{
				if (OCI.ClientVersionAtLeastOracle9i && OracleParameterBinding.IsDirection(this.Parameter, ParameterDirection.Output))
				{
					this._bindHandle.SetAttribute(OCI.ATTR.OCI_ATTR_MAXCHAR_SIZE, this._bindSize, errorHandle);
				}
				if (num2 > this._bindingMetaType.MaxBindSize / ADP.CharSize || (!OCI.ClientVersionAtLeastOracle9i && this._bindingMetaType.UsesNationalCharacterSet))
				{
					this._bindHandle.SetAttribute(OCI.ATTR.OCI_ATTR_MAXDATA_SIZE, this._bindingMetaType.MaxBindSize, errorHandle);
				}
				if (this._bindingMetaType.UsesNationalCharacterSet)
				{
					this._bindHandle.SetAttribute(OCI.ATTR.OCI_ATTR_CHARSET_FORM, 2, errorHandle);
				}
				if (this._bindAsUCS2)
				{
					this._bindHandle.SetAttribute(OCI.ATTR.OCI_ATTR_CHARSET_ID, 1000, errorHandle);
				}
			}
			GC.KeepAlive(parameterBuffer);
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x0006CB80 File Offset: 0x0006BF80
		private OracleLob CreateTemporaryLobForValue(OracleConnection connection, OracleType oracleType, object value)
		{
			OracleType oracleType2 = oracleType;
			switch (oracleType2)
			{
			case OracleType.BFile:
				oracleType = OracleType.Blob;
				goto IL_002D;
			case OracleType.Blob:
			case OracleType.Clob:
				goto IL_002D;
			case OracleType.Char:
				break;
			default:
				if (oracleType2 == OracleType.NClob)
				{
					goto IL_002D;
				}
				break;
			}
			throw ADP.InvalidLobType(oracleType);
			IL_002D:
			OracleLob oracleLob = new OracleLob(connection, oracleType);
			byte[] array = value as byte[];
			if (array != null)
			{
				oracleLob.Write(array, 0, array.Length);
			}
			else
			{
				Encoding encoding = new UnicodeEncoding(false, false);
				oracleLob.Seek(0L, SeekOrigin.Begin);
				StreamWriter streamWriter = new StreamWriter(oracleLob, encoding);
				streamWriter.Write(value);
				streamWriter.Flush();
			}
			return oracleLob;
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x0006CC04 File Offset: 0x0006C004
		internal object GetOutputValue(NativeBuffer parameterBuffer, OracleConnection connection, bool needCLSType)
		{
			if (parameterBuffer.ReadInt16(this._indicatorOffset) == -1)
			{
				return DBNull.Value;
			}
			OCI.DATATYPE ociType = this._bindingMetaType.OciType;
			object obj;
			if (ociType <= OCI.DATATYPE.UNSIGNEDINT)
			{
				if (ociType <= OCI.DATATYPE.DATE)
				{
					switch (ociType)
					{
					case OCI.DATATYPE.VARCHAR2:
					case OCI.DATATYPE.LONG:
						goto IL_0232;
					case OCI.DATATYPE.NUMBER:
					case OCI.DATATYPE.STRING:
					case (OCI.DATATYPE)7:
						goto IL_02EE;
					case OCI.DATATYPE.INTEGER:
					case OCI.DATATYPE.FLOAT:
						break;
					case OCI.DATATYPE.VARNUM:
						obj = new OracleNumber(parameterBuffer, this._valueOffset);
						if (needCLSType)
						{
							object obj2 = ((OracleNumber)obj).Value;
							obj = obj2;
						}
						return obj;
					default:
						if (ociType != OCI.DATATYPE.DATE)
						{
							goto IL_02EE;
						}
						obj = new OracleDateTime(parameterBuffer, this._valueOffset, this._lengthOffset, this._bindingMetaType, connection);
						if (needCLSType)
						{
							object obj3 = ((OracleDateTime)obj).Value;
							obj = obj3;
						}
						return obj;
					}
				}
				else
				{
					switch (ociType)
					{
					case OCI.DATATYPE.RAW:
					case OCI.DATATYPE.LONGRAW:
						goto IL_010B;
					default:
						if (ociType != OCI.DATATYPE.UNSIGNEDINT)
						{
							goto IL_02EE;
						}
						break;
					}
				}
				return parameterBuffer.PtrToStructure(this._valueOffset, this._bindingMetaType.BaseType);
			}
			if (ociType > OCI.DATATYPE.RSET)
			{
				switch (ociType)
				{
				case OCI.DATATYPE.INT_TIMESTAMP:
				case OCI.DATATYPE.INT_TIMESTAMP_TZ:
					break;
				case OCI.DATATYPE.INT_INTERVAL_YM:
					obj = new OracleMonthSpan(parameterBuffer, this._valueOffset);
					if (needCLSType)
					{
						object obj4 = ((OracleMonthSpan)obj).Value;
						obj = obj4;
					}
					return obj;
				case OCI.DATATYPE.INT_INTERVAL_DS:
					obj = new OracleTimeSpan(parameterBuffer, this._valueOffset);
					if (needCLSType)
					{
						object obj5 = ((OracleTimeSpan)obj).Value;
						obj = obj5;
					}
					return obj;
				default:
					if (ociType != OCI.DATATYPE.INT_TIMESTAMP_LTZ)
					{
						goto IL_02EE;
					}
					break;
				}
				obj = new OracleDateTime(this._dateTimeDescriptor, this._bindingMetaType, connection);
				if (needCLSType)
				{
					object obj6 = ((OracleDateTime)obj).Value;
					obj = obj6;
				}
				return obj;
			}
			switch (ociType)
			{
			case OCI.DATATYPE.LONGVARCHAR:
			case OCI.DATATYPE.CHAR:
				goto IL_0232;
			case OCI.DATATYPE.LONGVARRAW:
				break;
			default:
				switch (ociType)
				{
				case OCI.DATATYPE.CLOB:
				case OCI.DATATYPE.BLOB:
					return new OracleLob(this._locator);
				case OCI.DATATYPE.BFILE:
					return new OracleBFile(this._locator);
				case (OCI.DATATYPE)115:
					goto IL_02EE;
				case OCI.DATATYPE.RSET:
					return new OracleDataReader(connection, this._descriptor);
				default:
					goto IL_02EE;
				}
				break;
			}
			IL_010B:
			obj = new OracleBinary(parameterBuffer, this._valueOffset, this._lengthOffset, this._bindingMetaType);
			if (needCLSType)
			{
				object value = ((OracleBinary)obj).Value;
				obj = value;
			}
			return obj;
			IL_0232:
			obj = new OracleString(parameterBuffer, this._valueOffset, this._lengthOffset, this._bindingMetaType, connection, this._bindAsUCS2, true);
			int size = this._parameter.Size;
			if (size != 0 && size < ((OracleString)obj).Length)
			{
				string text = ((OracleString)obj).Value.Substring(0, size);
				if (needCLSType)
				{
					obj = text;
				}
				else
				{
					obj = new OracleString(text);
				}
			}
			else if (needCLSType)
			{
				object value2 = ((OracleString)obj).Value;
				obj = value2;
			}
			return obj;
			IL_02EE:
			throw ADP.TypeNotSupported(this._bindingMetaType.OciType);
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x0006CF10 File Offset: 0x0006C310
		internal void Dispose()
		{
			OciHandle.SafeDispose(ref this._bindHandle);
			if (this._freeTemporaryLob)
			{
				OracleLob oracleLob = this._coercedValue as OracleLob;
				if (oracleLob != null)
				{
					oracleLob.Free();
				}
			}
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x0006CF48 File Offset: 0x0006C348
		internal static bool IsDirection(IDataParameter value, ParameterDirection condition)
		{
			return condition == (condition & value.Direction);
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x0006CF60 File Offset: 0x0006C360
		private bool IsEmpty(object value)
		{
			bool flag = false;
			if (value is string)
			{
				flag = 0 == ((string)value).Length;
			}
			if (value is OracleString)
			{
				flag = 0 == ((OracleString)value).Length;
			}
			if (value is char[])
			{
				flag = 0 == ((char[])value).Length;
			}
			if (value is byte[])
			{
				flag = 0 == ((byte[])value).Length;
			}
			if (value is OracleBinary)
			{
				flag = 0 == ((OracleBinary)value).Length;
			}
			return flag;
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x0006CFE4 File Offset: 0x0006C3E4
		internal void PostExecute(NativeBuffer parameterBuffer, OracleConnection connection)
		{
			OracleParameter parameter = this.Parameter;
			if (OracleParameterBinding.IsDirection(parameter, ParameterDirection.Output) || OracleParameterBinding.IsDirection(parameter, ParameterDirection.ReturnValue))
			{
				bool flag = true;
				if (OracleParameterBinding.IsDirection(parameter, ParameterDirection.Input))
				{
					object value = parameter.Value;
					if (value is INullable)
					{
						flag = false;
					}
				}
				parameter.Value = this.GetOutputValue(parameterBuffer, connection, flag);
			}
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x0006D038 File Offset: 0x0006C438
		internal void PrepareForBind(OracleConnection connection, ref int offset)
		{
			OracleParameter parameter = this.Parameter;
			bool flag = false;
			object value = parameter.Value;
			if (!OracleParameterBinding.IsDirection(parameter, ParameterDirection.Output) && value == null)
			{
				this._bufferLength = 0;
				return;
			}
			this._bindingMetaType = parameter.GetMetaType(value);
			if (OCI.DATATYPE.RSET == this._bindingMetaType.OciType && ADP.IsDirection(parameter.Direction, ParameterDirection.Input))
			{
				throw ADP.InputRefCursorNotSupported(parameter.ParameterName);
			}
			parameter.SetCoercedValueInternal(value, this._bindingMetaType);
			this._coercedValue = parameter.GetCoercedValueInternal();
			switch (this._bindingMetaType.OciType)
			{
			case OCI.DATATYPE.CLOB:
			case OCI.DATATYPE.BLOB:
			case OCI.DATATYPE.BFILE:
				if (!ADP.IsNull(this._coercedValue) && !(this._coercedValue is OracleLob) && !(this._coercedValue is OracleBFile))
				{
					if (connection.HasTransaction)
					{
						this._freeTemporaryLob = true;
						this._coercedValue = this.CreateTemporaryLobForValue(connection, this._bindingMetaType.OracleType, this._coercedValue);
					}
					else
					{
						this._bindingMetaType = MetaType.GetMetaTypeForType(this._bindingMetaType.DbType);
						flag = true;
					}
				}
				break;
			}
			this._bindSize = this._bindingMetaType.BindSize;
			if ((OracleParameterBinding.IsDirection(parameter, ParameterDirection.Output) && this._bindingMetaType.IsVariableLength) || (this._bindSize == 0 && !ADP.IsNull(this._coercedValue)) || this._bindSize > 32767)
			{
				int bindSize = parameter.BindSize;
				if (bindSize != 0)
				{
					this._bindSize = bindSize;
				}
				if ((this._bindSize == 0 || 2147483647 == this._bindSize) && !this.IsEmpty(this._coercedValue))
				{
					throw ADP.ParameterSizeIsMissing(parameter.ParameterName, this._bindingMetaType.BaseType);
				}
			}
			this._bufferLength = this._bindSize;
			if (this._bindingMetaType.IsCharacterType && connection.ServerVersionAtLeastOracle8)
			{
				this._bindAsUCS2 = true;
				this._bufferLength *= ADP.CharSize;
			}
			if (!ADP.IsNull(this._coercedValue) && (this._bindSize > this._bindingMetaType.MaxBindSize || flag))
			{
				OCI.DATATYPE ociType = this._bindingMetaType.OciType;
				if (ociType <= OCI.DATATYPE.LONG)
				{
					if (ociType != OCI.DATATYPE.VARCHAR2 && ociType != OCI.DATATYPE.LONG)
					{
						goto IL_0259;
					}
				}
				else
				{
					switch (ociType)
					{
					case OCI.DATATYPE.RAW:
					case OCI.DATATYPE.LONGRAW:
						this._bindingMetaType = MetaType.oracleTypeMetaType_LONGVARRAW;
						goto IL_0259;
					default:
						if (ociType != OCI.DATATYPE.CHAR)
						{
							goto IL_0259;
						}
						break;
					}
				}
				this._bindingMetaType = (this._bindingMetaType.UsesNationalCharacterSet ? MetaType.oracleTypeMetaType_LONGNVARCHAR : MetaType.oracleTypeMetaType_LONGVARCHAR);
				IL_0259:
				this._bufferLength += 4;
			}
			if (0 > this._bufferLength)
			{
				throw ADP.ParameterSizeIsTooLarge(parameter.ParameterName);
			}
			this._indicatorOffset = offset;
			offset += IntPtr.Size;
			this._lengthOffset = offset;
			offset += IntPtr.Size;
			this._valueOffset = offset;
			offset += this._bufferLength;
			offset = (offset + (IntPtr.Size - 1)) & ~(IntPtr.Size - 1);
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x0006D310 File Offset: 0x0006C710
		internal int PutOracleValue(object value, NativeBuffer buffer, int bufferOffset, MetaType metaType, OracleConnection connection, ref SafeHandle handleToBind)
		{
			handleToBind = null;
			OCI.DATATYPE ociType = metaType.OciType;
			OracleParameter parameter = this.Parameter;
			OCI.DATATYPE datatype = ociType;
			if (datatype <= OCI.DATATYPE.UNSIGNEDINT)
			{
				if (datatype <= OCI.DATATYPE.DATE)
				{
					switch (datatype)
					{
					case OCI.DATATYPE.VARCHAR2:
					case OCI.DATATYPE.LONG:
						goto IL_0285;
					case OCI.DATATYPE.NUMBER:
					case OCI.DATATYPE.STRING:
					case (OCI.DATATYPE)7:
						goto IL_02B0;
					case OCI.DATATYPE.INTEGER:
					case OCI.DATATYPE.FLOAT:
						break;
					case OCI.DATATYPE.VARNUM:
						return OracleNumber.MarshalToNative(value, buffer, bufferOffset, connection);
					default:
						if (datatype != OCI.DATATYPE.DATE)
						{
							goto IL_02B0;
						}
						return OracleDateTime.MarshalDateToNative(value, buffer, bufferOffset, ociType, connection);
					}
				}
				else
				{
					switch (datatype)
					{
					case OCI.DATATYPE.RAW:
					case OCI.DATATYPE.LONGRAW:
						goto IL_00E5;
					default:
						if (datatype != OCI.DATATYPE.UNSIGNEDINT)
						{
							goto IL_02B0;
						}
						break;
					}
				}
				buffer.StructureToPtr(bufferOffset, value);
				return metaType.BindSize;
			}
			if (datatype <= OCI.DATATYPE.BFILE)
			{
				switch (datatype)
				{
				case OCI.DATATYPE.LONGVARCHAR:
				case OCI.DATATYPE.CHAR:
					goto IL_0285;
				case OCI.DATATYPE.LONGVARRAW:
					break;
				default:
					switch (datatype)
					{
					case OCI.DATATYPE.CLOB:
					case OCI.DATATYPE.BLOB:
						if (!(value is OracleLob))
						{
							throw ADP.BadBindValueType(value.GetType(), metaType.OracleType);
						}
						handleToBind = ((OracleLob)value).Descriptor;
						return IntPtr.Size;
					case OCI.DATATYPE.BFILE:
						if (!(value is OracleBFile))
						{
							throw ADP.BadBindValueType(value.GetType(), metaType.OracleType);
						}
						handleToBind = ((OracleBFile)value).Descriptor;
						return IntPtr.Size;
					default:
						goto IL_02B0;
					}
					break;
				}
			}
			else
			{
				switch (datatype)
				{
				case OCI.DATATYPE.INT_TIMESTAMP:
					break;
				case OCI.DATATYPE.INT_TIMESTAMP_TZ:
					if (value is OracleDateTime && !((OracleDateTime)value).HasTimeZoneInfo)
					{
						throw ADP.UnsupportedOracleDateTimeBinding(OracleType.TimestampWithTZ);
					}
					this._dateTimeDescriptor = OracleDateTime.CreateDescriptor(ociType, connection, value);
					handleToBind = this._dateTimeDescriptor;
					return IntPtr.Size;
				case OCI.DATATYPE.INT_INTERVAL_YM:
					return OracleMonthSpan.MarshalToNative(value, buffer, bufferOffset);
				case OCI.DATATYPE.INT_INTERVAL_DS:
					return OracleTimeSpan.MarshalToNative(value, buffer, bufferOffset);
				default:
					if (datatype != OCI.DATATYPE.INT_TIMESTAMP_LTZ)
					{
						goto IL_02B0;
					}
					break;
				}
				if (value is OracleDateTime && !((OracleDateTime)value).HasTimeInfo)
				{
					throw ADP.UnsupportedOracleDateTimeBinding(metaType.OracleType);
				}
				this._dateTimeDescriptor = OracleDateTime.CreateDescriptor(ociType, connection, value);
				handleToBind = this._dateTimeDescriptor;
				return IntPtr.Size;
			}
			IL_00E5:
			byte[] array;
			if (this._coercedValue is OracleBinary)
			{
				array = ((OracleBinary)this._coercedValue).Value;
			}
			else
			{
				array = (byte[])this._coercedValue;
			}
			int num = array.Length - parameter.Offset;
			int actualSize = parameter.GetActualSize();
			if (actualSize != 0)
			{
				num = Math.Min(num, actualSize);
			}
			int num2;
			if (OCI.DATATYPE.LONGVARRAW == ociType)
			{
				buffer.WriteInt32(bufferOffset, num);
				checked
				{
					bufferOffset += 4;
				}
				num2 = num + 4;
			}
			else
			{
				num2 = num;
			}
			buffer.WriteBytes(bufferOffset, array, parameter.Offset, num);
			return num2;
			IL_0285:
			return OracleString.MarshalToNative(value, parameter.Offset, parameter.GetActualSize(), buffer, bufferOffset, ociType, this._bindAsUCS2);
			IL_02B0:
			throw ADP.TypeNotSupported(ociType);
		}

		// Token: 0x04000495 RID: 1173
		private OracleCommand _command;

		// Token: 0x04000496 RID: 1174
		private OracleParameter _parameter;

		// Token: 0x04000497 RID: 1175
		private object _coercedValue;

		// Token: 0x04000498 RID: 1176
		private MetaType _bindingMetaType;

		// Token: 0x04000499 RID: 1177
		private OciBindHandle _bindHandle;

		// Token: 0x0400049A RID: 1178
		private int _bindSize;

		// Token: 0x0400049B RID: 1179
		private int _bufferLength;

		// Token: 0x0400049C RID: 1180
		private int _indicatorOffset;

		// Token: 0x0400049D RID: 1181
		private int _lengthOffset;

		// Token: 0x0400049E RID: 1182
		private int _valueOffset;

		// Token: 0x0400049F RID: 1183
		private bool _bindAsUCS2;

		// Token: 0x040004A0 RID: 1184
		private bool _freeTemporaryLob;

		// Token: 0x040004A1 RID: 1185
		private OciStatementHandle _descriptor;

		// Token: 0x040004A2 RID: 1186
		private OciLobLocator _locator;

		// Token: 0x040004A3 RID: 1187
		private OciDateTimeDescriptor _dateTimeDescriptor;
	}
}
