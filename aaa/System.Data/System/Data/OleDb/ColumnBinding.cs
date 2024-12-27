using System;
using System.Data.Common;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Data.OleDb
{
	// Token: 0x0200020C RID: 524
	internal sealed class ColumnBinding
	{
		// Token: 0x06001CF5 RID: 7413 RVA: 0x0024F4A0 File Offset: 0x0024E8A0
		internal ColumnBinding(OleDbDataReader dataReader, int index, int indexForAccessor, int indexWithinAccessor, OleDbParameter parameter, RowBinding rowbinding, Bindings bindings, tagDBBINDING binding, int offset, bool ifIRowsetElseIRow)
		{
			this._dataReader = dataReader;
			this._rowbinding = rowbinding;
			this._bindings = bindings;
			this._index = index;
			this._indexForAccessor = indexForAccessor;
			this._indexWithinAccessor = indexWithinAccessor;
			if (parameter != null)
			{
				this._parameter = parameter;
				this._parameterChangeID = parameter.ChangeID;
			}
			this._offsetStatus = binding.obStatus.ToInt32() + offset;
			this._offsetLength = binding.obLength.ToInt32() + offset;
			this._offsetValue = binding.obValue.ToInt32() + offset;
			this._ordinal = binding.iOrdinal.ToInt32();
			this._maxLen = binding.cbMaxLen.ToInt32();
			this._wType = binding.wType;
			this._precision = binding.bPrecision;
			this._ifIRowsetElseIRow = ifIRowsetElseIRow;
			this.SetSize(this.Bindings.ParamSize.ToInt32());
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06001CF6 RID: 7414 RVA: 0x0024F598 File Offset: 0x0024E998
		internal Bindings Bindings
		{
			get
			{
				this._bindings.CurrentIndex = this.IndexWithinAccessor;
				return this._bindings;
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06001CF7 RID: 7415 RVA: 0x0024F5BC File Offset: 0x0024E9BC
		internal RowBinding RowBinding
		{
			get
			{
				return this._rowbinding;
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06001CF8 RID: 7416 RVA: 0x0024F5D0 File Offset: 0x0024E9D0
		internal int ColumnBindingOrdinal
		{
			get
			{
				return this._ordinal;
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06001CF9 RID: 7417 RVA: 0x0024F5E4 File Offset: 0x0024E9E4
		private int ColumnBindingMaxLen
		{
			get
			{
				return this._maxLen;
			}
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06001CFA RID: 7418 RVA: 0x0024F5F8 File Offset: 0x0024E9F8
		private byte ColumnBindingPrecision
		{
			get
			{
				return this._precision;
			}
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06001CFB RID: 7419 RVA: 0x0024F60C File Offset: 0x0024EA0C
		private short DbType
		{
			get
			{
				return this._wType;
			}
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06001CFC RID: 7420 RVA: 0x0024F620 File Offset: 0x0024EA20
		private Type ExpectedType
		{
			get
			{
				return NativeDBType.FromDBType(this.DbType, false, false).dataType;
			}
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06001CFD RID: 7421 RVA: 0x0024F640 File Offset: 0x0024EA40
		internal int Index
		{
			get
			{
				return this._index;
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06001CFE RID: 7422 RVA: 0x0024F654 File Offset: 0x0024EA54
		internal int IndexForAccessor
		{
			get
			{
				return this._indexForAccessor;
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06001CFF RID: 7423 RVA: 0x0024F668 File Offset: 0x0024EA68
		internal int IndexWithinAccessor
		{
			get
			{
				return this._indexWithinAccessor;
			}
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06001D00 RID: 7424 RVA: 0x0024F67C File Offset: 0x0024EA7C
		private int ValueBindingOffset
		{
			get
			{
				return this._valueBindingOffset;
			}
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06001D01 RID: 7425 RVA: 0x0024F690 File Offset: 0x0024EA90
		private int ValueBindingSize
		{
			get
			{
				return this._valueBindingSize;
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06001D02 RID: 7426 RVA: 0x0024F6A4 File Offset: 0x0024EAA4
		internal int ValueOffset
		{
			get
			{
				return this._offsetValue;
			}
		}

		// Token: 0x06001D03 RID: 7427 RVA: 0x0024F6B8 File Offset: 0x0024EAB8
		private OleDbDataReader DataReader()
		{
			return this._dataReader;
		}

		// Token: 0x06001D04 RID: 7428 RVA: 0x0024F6CC File Offset: 0x0024EACC
		internal bool IsParameterBindingInvalid(OleDbParameter parameter)
		{
			return this._parameter.ChangeID != this._parameterChangeID || this._parameter != parameter;
		}

		// Token: 0x06001D05 RID: 7429 RVA: 0x0024F6FC File Offset: 0x0024EAFC
		internal bool IsValueNull()
		{
			return DBStatus.S_ISNULL == this.StatusValue() || ((12 == this.DbType || 138 == this.DbType) && Convert.IsDBNull(this.ValueVariant()));
		}

		// Token: 0x06001D06 RID: 7430 RVA: 0x0024F738 File Offset: 0x0024EB38
		private int LengthValue()
		{
			int num;
			if (this._ifIRowsetElseIRow)
			{
				num = this.RowBinding.ReadIntPtr(this._offsetLength).ToInt32();
			}
			else
			{
				num = this.Bindings.DBColumnAccess[this.IndexWithinAccessor].cbDataLen.ToInt32();
			}
			return Math.Max(num, 0);
		}

		// Token: 0x06001D07 RID: 7431 RVA: 0x0024F794 File Offset: 0x0024EB94
		private void LengthValue(int value)
		{
			this.RowBinding.WriteIntPtr(this._offsetLength, (IntPtr)value);
		}

		// Token: 0x06001D08 RID: 7432 RVA: 0x0024F7B8 File Offset: 0x0024EBB8
		internal OleDbParameter Parameter()
		{
			return this._parameter;
		}

		// Token: 0x06001D09 RID: 7433 RVA: 0x0024F7CC File Offset: 0x0024EBCC
		internal void ResetValue()
		{
			this._value = null;
			StringMemHandle sptr = this._sptr;
			this._sptr = null;
			if (sptr != null)
			{
				sptr.Dispose();
			}
			if (this._pinnedBuffer.IsAllocated)
			{
				this._pinnedBuffer.Free();
			}
		}

		// Token: 0x06001D0A RID: 7434 RVA: 0x0024F810 File Offset: 0x0024EC10
		internal DBStatus StatusValue()
		{
			if (this._ifIRowsetElseIRow)
			{
				return (DBStatus)this.RowBinding.ReadInt32(this._offsetStatus);
			}
			return (DBStatus)this.Bindings.DBColumnAccess[this.IndexWithinAccessor].dwStatus;
		}

		// Token: 0x06001D0B RID: 7435 RVA: 0x0024F854 File Offset: 0x0024EC54
		internal void StatusValue(DBStatus value)
		{
			this.RowBinding.WriteInt32(this._offsetStatus, (int)value);
		}

		// Token: 0x06001D0C RID: 7436 RVA: 0x0024F874 File Offset: 0x0024EC74
		internal void SetOffset(int offset)
		{
			if (0 > offset)
			{
				throw ADP.InvalidOffsetValue(offset);
			}
			this._valueBindingOffset = Math.Max(offset, 0);
		}

		// Token: 0x06001D0D RID: 7437 RVA: 0x0024F89C File Offset: 0x0024EC9C
		internal void SetSize(int size)
		{
			this._valueBindingSize = Math.Max(size, 0);
		}

		// Token: 0x06001D0E RID: 7438 RVA: 0x0024F8B8 File Offset: 0x0024ECB8
		private void SetValueDBNull()
		{
			this.LengthValue(0);
			this.StatusValue(DBStatus.S_ISNULL);
			this.RowBinding.WriteInt64(this.ValueOffset, 0L);
		}

		// Token: 0x06001D0F RID: 7439 RVA: 0x0024F8E8 File Offset: 0x0024ECE8
		private void SetValueEmpty()
		{
			this.LengthValue(0);
			this.StatusValue(DBStatus.S_DEFAULT);
			this.RowBinding.WriteInt64(this.ValueOffset, 0L);
		}

		// Token: 0x06001D10 RID: 7440 RVA: 0x0024F918 File Offset: 0x0024ED18
		internal object Value()
		{
			object obj = this._value;
			if (obj == null)
			{
				DBStatus dbstatus = this.StatusValue();
				switch (dbstatus)
				{
				case DBStatus.S_OK:
				{
					short dbType = this.DbType;
					if (dbType <= 64)
					{
						switch (dbType)
						{
						case 0:
						case 1:
							obj = DBNull.Value;
							goto IL_0381;
						case 2:
							obj = this.Value_I2();
							goto IL_0381;
						case 3:
							obj = this.Value_I4();
							goto IL_0381;
						case 4:
							obj = this.Value_R4();
							goto IL_0381;
						case 5:
							obj = this.Value_R8();
							goto IL_0381;
						case 6:
							obj = this.Value_CY();
							goto IL_0381;
						case 7:
							obj = this.Value_DATE();
							goto IL_0381;
						case 8:
							obj = this.Value_BSTR();
							goto IL_0381;
						case 9:
							obj = this.Value_IDISPATCH();
							goto IL_0381;
						case 10:
							obj = this.Value_ERROR();
							goto IL_0381;
						case 11:
							obj = this.Value_BOOL();
							goto IL_0381;
						case 12:
							obj = this.Value_VARIANT();
							goto IL_0381;
						case 13:
							obj = this.Value_IUNKNOWN();
							goto IL_0381;
						case 14:
							obj = this.Value_DECIMAL();
							goto IL_0381;
						case 15:
							break;
						case 16:
							obj = (short)this.Value_I1();
							goto IL_0381;
						case 17:
							obj = this.Value_UI1();
							goto IL_0381;
						case 18:
							obj = (int)this.Value_UI2();
							goto IL_0381;
						case 19:
							obj = (long)((ulong)this.Value_UI4());
							goto IL_0381;
						case 20:
							obj = this.Value_I8();
							goto IL_0381;
						case 21:
							obj = this.Value_UI8();
							goto IL_0381;
						default:
							if (dbType == 64)
							{
								obj = this.Value_FILETIME();
								goto IL_0381;
							}
							break;
						}
					}
					else
					{
						if (dbType == 72)
						{
							obj = this.Value_GUID();
							goto IL_0381;
						}
						switch (dbType)
						{
						case 128:
							obj = this.Value_BYTES();
							goto IL_0381;
						case 129:
						case 132:
						case 137:
							break;
						case 130:
							obj = this.Value_WSTR();
							goto IL_0381;
						case 131:
							obj = this.Value_NUMERIC();
							goto IL_0381;
						case 133:
							obj = this.Value_DBDATE();
							goto IL_0381;
						case 134:
							obj = this.Value_DBTIME();
							goto IL_0381;
						case 135:
							obj = this.Value_DBTIMESTAMP();
							goto IL_0381;
						case 136:
							obj = this.Value_HCHAPTER();
							goto IL_0381;
						case 138:
							obj = this.Value_VARIANT();
							goto IL_0381;
						default:
							switch (dbType)
							{
							case 16512:
								obj = this.Value_ByRefBYTES();
								goto IL_0381;
							case 16514:
								obj = this.Value_ByRefWSTR();
								goto IL_0381;
							}
							break;
						}
					}
					throw ODB.GVtUnknown((int)this.DbType);
				}
				case DBStatus.E_BADACCESSOR:
				case DBStatus.E_CANTCONVERTVALUE:
					goto IL_037A;
				case DBStatus.S_ISNULL:
					break;
				case DBStatus.S_TRUNCATED:
				{
					short dbType2 = this.DbType;
					switch (dbType2)
					{
					case 128:
						obj = this.Value_BYTES();
						goto IL_0381;
					case 129:
						break;
					case 130:
						obj = this.Value_WSTR();
						goto IL_0381;
					default:
						switch (dbType2)
						{
						case 16512:
							obj = this.Value_ByRefBYTES();
							goto IL_0381;
						case 16514:
							obj = this.Value_ByRefWSTR();
							goto IL_0381;
						}
						break;
					}
					throw ODB.GVtUnknown((int)this.DbType);
				}
				default:
					if (dbstatus != DBStatus.S_DEFAULT)
					{
						goto IL_037A;
					}
					break;
				}
				obj = DBNull.Value;
				goto IL_0381;
				IL_037A:
				throw this.CheckTypeValueStatusValue();
				IL_0381:
				this._value = obj;
			}
			return obj;
		}

		// Token: 0x06001D11 RID: 7441 RVA: 0x0024FCB0 File Offset: 0x0024F0B0
		internal void Value(object value)
		{
			if (value == null)
			{
				this.SetValueEmpty();
				return;
			}
			if (Convert.IsDBNull(value))
			{
				this.SetValueDBNull();
				return;
			}
			short dbType = this.DbType;
			if (dbType <= 64)
			{
				switch (dbType)
				{
				case 0:
					this.SetValueEmpty();
					return;
				case 1:
					this.SetValueDBNull();
					return;
				case 2:
					this.Value_I2((short)value);
					return;
				case 3:
					this.Value_I4((int)value);
					return;
				case 4:
					this.Value_R4((float)value);
					return;
				case 5:
					this.Value_R8((double)value);
					return;
				case 6:
					this.Value_CY((decimal)value);
					return;
				case 7:
					this.Value_DATE((DateTime)value);
					return;
				case 8:
					this.Value_BSTR((string)value);
					return;
				case 9:
					this.Value_IDISPATCH(value);
					return;
				case 10:
					this.Value_ERROR((int)value);
					return;
				case 11:
					this.Value_BOOL((bool)value);
					return;
				case 12:
					this.Value_VARIANT(value);
					return;
				case 13:
					this.Value_IUNKNOWN(value);
					return;
				case 14:
					this.Value_DECIMAL((decimal)value);
					return;
				case 15:
					break;
				case 16:
					if (value is short)
					{
						this.Value_I1(Convert.ToSByte((short)value, CultureInfo.InvariantCulture));
						return;
					}
					this.Value_I1((sbyte)value);
					return;
				case 17:
					this.Value_UI1((byte)value);
					return;
				case 18:
					if (value is int)
					{
						this.Value_UI2(Convert.ToUInt16((int)value, CultureInfo.InvariantCulture));
						return;
					}
					this.Value_UI2((ushort)value);
					return;
				case 19:
					if (value is long)
					{
						this.Value_UI4(Convert.ToUInt32((long)value, CultureInfo.InvariantCulture));
						return;
					}
					this.Value_UI4((uint)value);
					return;
				case 20:
					this.Value_I8((long)value);
					return;
				case 21:
					if (value is decimal)
					{
						this.Value_UI8(Convert.ToUInt64((decimal)value, CultureInfo.InvariantCulture));
						return;
					}
					this.Value_UI8((ulong)value);
					return;
				default:
					if (dbType == 64)
					{
						this.Value_FILETIME((DateTime)value);
						return;
					}
					break;
				}
			}
			else
			{
				if (dbType == 72)
				{
					this.Value_GUID((Guid)value);
					return;
				}
				switch (dbType)
				{
				case 128:
					this.Value_BYTES((byte[])value);
					return;
				case 129:
				case 132:
				case 136:
				case 137:
					break;
				case 130:
					if (value is string)
					{
						this.Value_WSTR((string)value);
						return;
					}
					this.Value_WSTR((char[])value);
					return;
				case 131:
					this.Value_NUMERIC((decimal)value);
					return;
				case 133:
					this.Value_DBDATE((DateTime)value);
					return;
				case 134:
					this.Value_DBTIME((TimeSpan)value);
					return;
				case 135:
					this.Value_DBTIMESTAMP((DateTime)value);
					return;
				case 138:
					this.Value_VARIANT(value);
					return;
				default:
					switch (dbType)
					{
					case 16512:
						this.Value_ByRefBYTES((byte[])value);
						return;
					case 16514:
						if (value is string)
						{
							this.Value_ByRefWSTR((string)value);
							return;
						}
						this.Value_ByRefWSTR((char[])value);
						return;
					}
					break;
				}
			}
			throw ODB.SVtUnknown((int)this.DbType);
		}

		// Token: 0x06001D12 RID: 7442 RVA: 0x0024FFF0 File Offset: 0x0024F3F0
		internal bool Value_BOOL()
		{
			short num = this.RowBinding.ReadInt16(this.ValueOffset);
			return 0 != num;
		}

		// Token: 0x06001D13 RID: 7443 RVA: 0x00250018 File Offset: 0x0024F418
		private void Value_BOOL(bool value)
		{
			this.LengthValue(0);
			this.StatusValue(DBStatus.S_OK);
			this.RowBinding.WriteInt16(this.ValueOffset, value ? -1 : 0);
		}

		// Token: 0x06001D14 RID: 7444 RVA: 0x0025004C File Offset: 0x0024F44C
		private string Value_BSTR()
		{
			string text = "";
			RowBinding rowBinding = this.RowBinding;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				rowBinding.DangerousAddRef(ref flag);
				IntPtr intPtr = rowBinding.ReadIntPtr(this.ValueOffset);
				if (ADP.PtrZero != intPtr)
				{
					text = Marshal.PtrToStringBSTR(intPtr);
				}
			}
			finally
			{
				if (flag)
				{
					rowBinding.DangerousRelease();
				}
			}
			return text;
		}

		// Token: 0x06001D15 RID: 7445 RVA: 0x002500C0 File Offset: 0x0024F4C0
		private void Value_BSTR(string value)
		{
			this.LengthValue(value.Length * 2);
			this.StatusValue(DBStatus.S_OK);
			this.RowBinding.SetBstrValue(this.ValueOffset, value);
		}

		// Token: 0x06001D16 RID: 7446 RVA: 0x002500F4 File Offset: 0x0024F4F4
		private byte[] Value_ByRefBYTES()
		{
			byte[] array = null;
			RowBinding rowBinding = this.RowBinding;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				rowBinding.DangerousAddRef(ref flag);
				IntPtr intPtr = rowBinding.ReadIntPtr(this.ValueOffset);
				if (ADP.PtrZero != intPtr)
				{
					array = new byte[this.LengthValue()];
					Marshal.Copy(intPtr, array, 0, array.Length);
				}
			}
			finally
			{
				if (flag)
				{
					rowBinding.DangerousRelease();
				}
			}
			if (array == null)
			{
				return new byte[0];
			}
			return array;
		}

		// Token: 0x06001D17 RID: 7447 RVA: 0x00250180 File Offset: 0x0024F580
		private void Value_ByRefBYTES(byte[] value)
		{
			int num = ((this.ValueBindingOffset < value.Length) ? (value.Length - this.ValueBindingOffset) : 0);
			this.LengthValue((0 < this.ValueBindingSize) ? Math.Min(this.ValueBindingSize, num) : num);
			this.StatusValue(DBStatus.S_OK);
			IntPtr intPtr = ADP.PtrZero;
			if (0 < num)
			{
				this._pinnedBuffer = GCHandle.Alloc(value, GCHandleType.Pinned);
				intPtr = this._pinnedBuffer.AddrOfPinnedObject();
				intPtr = ADP.IntPtrOffset(intPtr, this.ValueBindingOffset);
			}
			this.RowBinding.SetByRefValue(this.ValueOffset, intPtr);
		}

		// Token: 0x06001D18 RID: 7448 RVA: 0x00250210 File Offset: 0x0024F610
		private string Value_ByRefWSTR()
		{
			string text = "";
			RowBinding rowBinding = this.RowBinding;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				rowBinding.DangerousAddRef(ref flag);
				IntPtr intPtr = rowBinding.ReadIntPtr(this.ValueOffset);
				if (ADP.PtrZero != intPtr)
				{
					int num = this.LengthValue() / 2;
					text = Marshal.PtrToStringUni(intPtr, num);
				}
			}
			finally
			{
				if (flag)
				{
					rowBinding.DangerousRelease();
				}
			}
			return text;
		}

		// Token: 0x06001D19 RID: 7449 RVA: 0x00250290 File Offset: 0x0024F690
		private void Value_ByRefWSTR(string value)
		{
			int num = ((this.ValueBindingOffset < value.Length) ? (value.Length - this.ValueBindingOffset) : 0);
			this.LengthValue(((0 < this.ValueBindingSize) ? Math.Min(this.ValueBindingSize, num) : num) * 2);
			this.StatusValue(DBStatus.S_OK);
			IntPtr intPtr = ADP.PtrZero;
			if (0 < num)
			{
				this._pinnedBuffer = GCHandle.Alloc(value, GCHandleType.Pinned);
				intPtr = this._pinnedBuffer.AddrOfPinnedObject();
				intPtr = ADP.IntPtrOffset(intPtr, this.ValueBindingOffset);
			}
			this.RowBinding.SetByRefValue(this.ValueOffset, intPtr);
		}

		// Token: 0x06001D1A RID: 7450 RVA: 0x00250328 File Offset: 0x0024F728
		private void Value_ByRefWSTR(char[] value)
		{
			int num = ((this.ValueBindingOffset < value.Length) ? (value.Length - this.ValueBindingOffset) : 0);
			this.LengthValue(((0 < this.ValueBindingSize) ? Math.Min(this.ValueBindingSize, num) : num) * 2);
			this.StatusValue(DBStatus.S_OK);
			IntPtr intPtr = ADP.PtrZero;
			if (0 < num)
			{
				this._pinnedBuffer = GCHandle.Alloc(value, GCHandleType.Pinned);
				intPtr = this._pinnedBuffer.AddrOfPinnedObject();
				intPtr = ADP.IntPtrOffset(intPtr, this.ValueBindingOffset);
			}
			this.RowBinding.SetByRefValue(this.ValueOffset, intPtr);
		}

		// Token: 0x06001D1B RID: 7451 RVA: 0x002503B8 File Offset: 0x0024F7B8
		private byte[] Value_BYTES()
		{
			int num = Math.Min(this.LengthValue(), this.ColumnBindingMaxLen);
			byte[] array = new byte[num];
			this.RowBinding.ReadBytes(this.ValueOffset, array, 0, num);
			return array;
		}

		// Token: 0x06001D1C RID: 7452 RVA: 0x002503F4 File Offset: 0x0024F7F4
		private void Value_BYTES(byte[] value)
		{
			int num = ((this.ValueBindingOffset < value.Length) ? Math.Min(value.Length - this.ValueBindingOffset, this.ColumnBindingMaxLen) : 0);
			this.LengthValue(num);
			this.StatusValue(DBStatus.S_OK);
			if (0 < num)
			{
				this.RowBinding.WriteBytes(this.ValueOffset, value, this.ValueBindingOffset, num);
			}
		}

		// Token: 0x06001D1D RID: 7453 RVA: 0x00250450 File Offset: 0x0024F850
		private decimal Value_CY()
		{
			return decimal.FromOACurrency(this.RowBinding.ReadInt64(this.ValueOffset));
		}

		// Token: 0x06001D1E RID: 7454 RVA: 0x00250474 File Offset: 0x0024F874
		private void Value_CY(decimal value)
		{
			this.LengthValue(0);
			this.StatusValue(DBStatus.S_OK);
			this.RowBinding.WriteInt64(this.ValueOffset, decimal.ToOACurrency(value));
		}

		// Token: 0x06001D1F RID: 7455 RVA: 0x002504A8 File Offset: 0x0024F8A8
		private DateTime Value_DATE()
		{
			return DateTime.FromOADate(this.RowBinding.ReadDouble(this.ValueOffset));
		}

		// Token: 0x06001D20 RID: 7456 RVA: 0x002504CC File Offset: 0x0024F8CC
		private void Value_DATE(DateTime value)
		{
			this.LengthValue(0);
			this.StatusValue(DBStatus.S_OK);
			this.RowBinding.WriteDouble(this.ValueOffset, value.ToOADate());
		}

		// Token: 0x06001D21 RID: 7457 RVA: 0x00250500 File Offset: 0x0024F900
		private DateTime Value_DBDATE()
		{
			return this.RowBinding.ReadDate(this.ValueOffset);
		}

		// Token: 0x06001D22 RID: 7458 RVA: 0x00250520 File Offset: 0x0024F920
		private void Value_DBDATE(DateTime value)
		{
			this.LengthValue(0);
			this.StatusValue(DBStatus.S_OK);
			this.RowBinding.WriteDate(this.ValueOffset, value);
		}

		// Token: 0x06001D23 RID: 7459 RVA: 0x00250550 File Offset: 0x0024F950
		private TimeSpan Value_DBTIME()
		{
			return this.RowBinding.ReadTime(this.ValueOffset);
		}

		// Token: 0x06001D24 RID: 7460 RVA: 0x00250570 File Offset: 0x0024F970
		private void Value_DBTIME(TimeSpan value)
		{
			this.LengthValue(0);
			this.StatusValue(DBStatus.S_OK);
			this.RowBinding.WriteTime(this.ValueOffset, value);
		}

		// Token: 0x06001D25 RID: 7461 RVA: 0x002505A0 File Offset: 0x0024F9A0
		private DateTime Value_DBTIMESTAMP()
		{
			return this.RowBinding.ReadDateTime(this.ValueOffset);
		}

		// Token: 0x06001D26 RID: 7462 RVA: 0x002505C0 File Offset: 0x0024F9C0
		private void Value_DBTIMESTAMP(DateTime value)
		{
			this.LengthValue(0);
			this.StatusValue(DBStatus.S_OK);
			this.RowBinding.WriteDateTime(this.ValueOffset, value);
		}

		// Token: 0x06001D27 RID: 7463 RVA: 0x002505F0 File Offset: 0x0024F9F0
		private decimal Value_DECIMAL()
		{
			int[] array = new int[4];
			this.RowBinding.ReadInt32Array(this.ValueOffset, array, 0, 4);
			return new decimal(array[2], array[3], array[1], 0 != (array[0] & int.MinValue), (byte)((array[0] & 16711680) >> 16));
		}

		// Token: 0x06001D28 RID: 7464 RVA: 0x00250644 File Offset: 0x0024FA44
		private void Value_DECIMAL(decimal value)
		{
			this.LengthValue(0);
			this.StatusValue(DBStatus.S_OK);
			int[] bits = decimal.GetBits(value);
			int[] array = new int[]
			{
				bits[3],
				bits[2],
				bits[0],
				bits[1]
			};
			this.RowBinding.WriteInt32Array(this.ValueOffset, array, 0, 4);
		}

		// Token: 0x06001D29 RID: 7465 RVA: 0x0025069C File Offset: 0x0024FA9C
		private int Value_ERROR()
		{
			return this.RowBinding.ReadInt32(this.ValueOffset);
		}

		// Token: 0x06001D2A RID: 7466 RVA: 0x002506BC File Offset: 0x0024FABC
		private void Value_ERROR(int value)
		{
			this.LengthValue(0);
			this.StatusValue(DBStatus.S_OK);
			this.RowBinding.WriteInt32(this.ValueOffset, value);
		}

		// Token: 0x06001D2B RID: 7467 RVA: 0x002506EC File Offset: 0x0024FAEC
		private DateTime Value_FILETIME()
		{
			long num = this.RowBinding.ReadInt64(this.ValueOffset);
			return DateTime.FromFileTime(num);
		}

		// Token: 0x06001D2C RID: 7468 RVA: 0x00250714 File Offset: 0x0024FB14
		private void Value_FILETIME(DateTime value)
		{
			this.LengthValue(0);
			this.StatusValue(DBStatus.S_OK);
			long num = value.ToFileTime();
			this.RowBinding.WriteInt64(this.ValueOffset, num);
		}

		// Token: 0x06001D2D RID: 7469 RVA: 0x0025074C File Offset: 0x0024FB4C
		internal Guid Value_GUID()
		{
			return this.RowBinding.ReadGuid(this.ValueOffset);
		}

		// Token: 0x06001D2E RID: 7470 RVA: 0x0025076C File Offset: 0x0024FB6C
		private void Value_GUID(Guid value)
		{
			this.LengthValue(0);
			this.StatusValue(DBStatus.S_OK);
			this.RowBinding.WriteGuid(this.ValueOffset, value);
		}

		// Token: 0x06001D2F RID: 7471 RVA: 0x0025079C File Offset: 0x0024FB9C
		internal OleDbDataReader Value_HCHAPTER()
		{
			return this.DataReader().ResetChapter(this.IndexForAccessor, this.IndexWithinAccessor, this.RowBinding, this.ValueOffset);
		}

		// Token: 0x06001D30 RID: 7472 RVA: 0x002507CC File Offset: 0x0024FBCC
		private sbyte Value_I1()
		{
			byte b = this.RowBinding.ReadByte(this.ValueOffset);
			return (sbyte)b;
		}

		// Token: 0x06001D31 RID: 7473 RVA: 0x002507F0 File Offset: 0x0024FBF0
		private void Value_I1(sbyte value)
		{
			this.LengthValue(0);
			this.StatusValue(DBStatus.S_OK);
			this.RowBinding.WriteByte(this.ValueOffset, (byte)value);
		}

		// Token: 0x06001D32 RID: 7474 RVA: 0x00250820 File Offset: 0x0024FC20
		internal short Value_I2()
		{
			return this.RowBinding.ReadInt16(this.ValueOffset);
		}

		// Token: 0x06001D33 RID: 7475 RVA: 0x00250840 File Offset: 0x0024FC40
		private void Value_I2(short value)
		{
			this.LengthValue(0);
			this.StatusValue(DBStatus.S_OK);
			this.RowBinding.WriteInt16(this.ValueOffset, value);
		}

		// Token: 0x06001D34 RID: 7476 RVA: 0x00250870 File Offset: 0x0024FC70
		private int Value_I4()
		{
			return this.RowBinding.ReadInt32(this.ValueOffset);
		}

		// Token: 0x06001D35 RID: 7477 RVA: 0x00250890 File Offset: 0x0024FC90
		private void Value_I4(int value)
		{
			this.LengthValue(0);
			this.StatusValue(DBStatus.S_OK);
			this.RowBinding.WriteInt32(this.ValueOffset, value);
		}

		// Token: 0x06001D36 RID: 7478 RVA: 0x002508C0 File Offset: 0x0024FCC0
		private long Value_I8()
		{
			return this.RowBinding.ReadInt64(this.ValueOffset);
		}

		// Token: 0x06001D37 RID: 7479 RVA: 0x002508E0 File Offset: 0x0024FCE0
		private void Value_I8(long value)
		{
			this.LengthValue(0);
			this.StatusValue(DBStatus.S_OK);
			this.RowBinding.WriteInt64(this.ValueOffset, value);
		}

		// Token: 0x06001D38 RID: 7480 RVA: 0x00250910 File Offset: 0x0024FD10
		private object Value_IDISPATCH()
		{
			RowBinding rowBinding = this.RowBinding;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			object objectForIUnknown;
			try
			{
				rowBinding.DangerousAddRef(ref flag);
				IntPtr intPtr = rowBinding.ReadIntPtr(this.ValueOffset);
				objectForIUnknown = Marshal.GetObjectForIUnknown(intPtr);
			}
			finally
			{
				if (flag)
				{
					rowBinding.DangerousRelease();
				}
			}
			return objectForIUnknown;
		}

		// Token: 0x06001D39 RID: 7481 RVA: 0x00250970 File Offset: 0x0024FD70
		private void Value_IDISPATCH(object value)
		{
			new NamedPermissionSet("FullTrust").Demand();
			this.LengthValue(0);
			this.StatusValue(DBStatus.S_OK);
			IntPtr idispatchForObject = Marshal.GetIDispatchForObject(value);
			this.RowBinding.WriteIntPtr(this.ValueOffset, idispatchForObject);
		}

		// Token: 0x06001D3A RID: 7482 RVA: 0x002509B4 File Offset: 0x0024FDB4
		private object Value_IUNKNOWN()
		{
			RowBinding rowBinding = this.RowBinding;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			object objectForIUnknown;
			try
			{
				rowBinding.DangerousAddRef(ref flag);
				IntPtr intPtr = rowBinding.ReadIntPtr(this.ValueOffset);
				objectForIUnknown = Marshal.GetObjectForIUnknown(intPtr);
			}
			finally
			{
				if (flag)
				{
					rowBinding.DangerousRelease();
				}
			}
			return objectForIUnknown;
		}

		// Token: 0x06001D3B RID: 7483 RVA: 0x00250A14 File Offset: 0x0024FE14
		private void Value_IUNKNOWN(object value)
		{
			new NamedPermissionSet("FullTrust").Demand();
			this.LengthValue(0);
			this.StatusValue(DBStatus.S_OK);
			IntPtr iunknownForObject = Marshal.GetIUnknownForObject(value);
			this.RowBinding.WriteIntPtr(this.ValueOffset, iunknownForObject);
		}

		// Token: 0x06001D3C RID: 7484 RVA: 0x00250A58 File Offset: 0x0024FE58
		private decimal Value_NUMERIC()
		{
			return this.RowBinding.ReadNumeric(this.ValueOffset);
		}

		// Token: 0x06001D3D RID: 7485 RVA: 0x00250A78 File Offset: 0x0024FE78
		private void Value_NUMERIC(decimal value)
		{
			this.LengthValue(0);
			this.StatusValue(DBStatus.S_OK);
			this.RowBinding.WriteNumeric(this.ValueOffset, value, this.ColumnBindingPrecision);
		}

		// Token: 0x06001D3E RID: 7486 RVA: 0x00250AAC File Offset: 0x0024FEAC
		private float Value_R4()
		{
			return this.RowBinding.ReadSingle(this.ValueOffset);
		}

		// Token: 0x06001D3F RID: 7487 RVA: 0x00250ACC File Offset: 0x0024FECC
		private void Value_R4(float value)
		{
			this.LengthValue(0);
			this.StatusValue(DBStatus.S_OK);
			this.RowBinding.WriteSingle(this.ValueOffset, value);
		}

		// Token: 0x06001D40 RID: 7488 RVA: 0x00250AFC File Offset: 0x0024FEFC
		private double Value_R8()
		{
			return this.RowBinding.ReadDouble(this.ValueOffset);
		}

		// Token: 0x06001D41 RID: 7489 RVA: 0x00250B1C File Offset: 0x0024FF1C
		private void Value_R8(double value)
		{
			this.LengthValue(0);
			this.StatusValue(DBStatus.S_OK);
			this.RowBinding.WriteDouble(this.ValueOffset, value);
		}

		// Token: 0x06001D42 RID: 7490 RVA: 0x00250B4C File Offset: 0x0024FF4C
		private byte Value_UI1()
		{
			return this.RowBinding.ReadByte(this.ValueOffset);
		}

		// Token: 0x06001D43 RID: 7491 RVA: 0x00250B6C File Offset: 0x0024FF6C
		private void Value_UI1(byte value)
		{
			this.LengthValue(0);
			this.StatusValue(DBStatus.S_OK);
			this.RowBinding.WriteByte(this.ValueOffset, value);
		}

		// Token: 0x06001D44 RID: 7492 RVA: 0x00250B9C File Offset: 0x0024FF9C
		internal ushort Value_UI2()
		{
			return (ushort)this.RowBinding.ReadInt16(this.ValueOffset);
		}

		// Token: 0x06001D45 RID: 7493 RVA: 0x00250BBC File Offset: 0x0024FFBC
		private void Value_UI2(ushort value)
		{
			this.LengthValue(0);
			this.StatusValue(DBStatus.S_OK);
			this.RowBinding.WriteInt16(this.ValueOffset, (short)value);
		}

		// Token: 0x06001D46 RID: 7494 RVA: 0x00250BEC File Offset: 0x0024FFEC
		internal uint Value_UI4()
		{
			return (uint)this.RowBinding.ReadInt32(this.ValueOffset);
		}

		// Token: 0x06001D47 RID: 7495 RVA: 0x00250C0C File Offset: 0x0025000C
		private void Value_UI4(uint value)
		{
			this.LengthValue(0);
			this.StatusValue(DBStatus.S_OK);
			this.RowBinding.WriteInt32(this.ValueOffset, (int)value);
		}

		// Token: 0x06001D48 RID: 7496 RVA: 0x00250C3C File Offset: 0x0025003C
		internal ulong Value_UI8()
		{
			return (ulong)this.RowBinding.ReadInt64(this.ValueOffset);
		}

		// Token: 0x06001D49 RID: 7497 RVA: 0x00250C5C File Offset: 0x0025005C
		private void Value_UI8(ulong value)
		{
			this.LengthValue(0);
			this.StatusValue(DBStatus.S_OK);
			this.RowBinding.WriteInt64(this.ValueOffset, (long)value);
		}

		// Token: 0x06001D4A RID: 7498 RVA: 0x00250C8C File Offset: 0x0025008C
		private string Value_WSTR()
		{
			int num = Math.Min(this.LengthValue(), this.ColumnBindingMaxLen - 2);
			return this.RowBinding.PtrToStringUni(this.ValueOffset, num / 2);
		}

		// Token: 0x06001D4B RID: 7499 RVA: 0x00250CC4 File Offset: 0x002500C4
		private void Value_WSTR(string value)
		{
			int num = ((this.ValueBindingOffset < value.Length) ? Math.Min(value.Length - this.ValueBindingOffset, (this.ColumnBindingMaxLen - 2) / 2) : 0);
			this.LengthValue(num * 2);
			this.StatusValue(DBStatus.S_OK);
			if (0 < num)
			{
				char[] array = value.ToCharArray(this.ValueBindingOffset, num);
				this.RowBinding.WriteCharArray(this.ValueOffset, array, this.ValueBindingOffset, num);
			}
		}

		// Token: 0x06001D4C RID: 7500 RVA: 0x00250D3C File Offset: 0x0025013C
		private void Value_WSTR(char[] value)
		{
			int num = ((this.ValueBindingOffset < value.Length) ? Math.Min(value.Length - this.ValueBindingOffset, (this.ColumnBindingMaxLen - 2) / 2) : 0);
			this.LengthValue(num * 2);
			this.StatusValue(DBStatus.S_OK);
			if (0 < num)
			{
				this.RowBinding.WriteCharArray(this.ValueOffset, value, this.ValueBindingOffset, num);
			}
		}

		// Token: 0x06001D4D RID: 7501 RVA: 0x00250DA0 File Offset: 0x002501A0
		private object Value_VARIANT()
		{
			return this.RowBinding.GetVariantValue(this.ValueOffset);
		}

		// Token: 0x06001D4E RID: 7502 RVA: 0x00250DC0 File Offset: 0x002501C0
		private void Value_VARIANT(object value)
		{
			this.LengthValue(0);
			this.StatusValue(DBStatus.S_OK);
			this.RowBinding.SetVariantValue(this.ValueOffset, value);
		}

		// Token: 0x06001D4F RID: 7503 RVA: 0x00250DF0 File Offset: 0x002501F0
		internal bool ValueBoolean()
		{
			DBStatus dbstatus = this.StatusValue();
			if (dbstatus == DBStatus.S_OK)
			{
				bool flag;
				switch (this.DbType)
				{
				case 11:
					flag = this.Value_BOOL();
					break;
				case 12:
					flag = (bool)this.ValueVariant();
					break;
				default:
					throw ODB.ConversionRequired();
				}
				return flag;
			}
			throw this.CheckTypeValueStatusValue(typeof(bool));
		}

		// Token: 0x06001D50 RID: 7504 RVA: 0x00250E54 File Offset: 0x00250254
		internal byte[] ValueByteArray()
		{
			byte[] array = (byte[])this._value;
			if (array == null)
			{
				DBStatus dbstatus = this.StatusValue();
				if (dbstatus != DBStatus.S_OK)
				{
					if (dbstatus != DBStatus.S_TRUNCATED)
					{
						throw this.CheckTypeValueStatusValue(typeof(byte[]));
					}
					short dbType = this.DbType;
					if (dbType != 128)
					{
						if (dbType != 16512)
						{
							throw ODB.ConversionRequired();
						}
						array = this.Value_ByRefBYTES();
					}
					else
					{
						array = this.Value_BYTES();
					}
				}
				else
				{
					short dbType2 = this.DbType;
					if (dbType2 != 12)
					{
						if (dbType2 != 128)
						{
							if (dbType2 != 16512)
							{
								throw ODB.ConversionRequired();
							}
							array = this.Value_ByRefBYTES();
						}
						else
						{
							array = this.Value_BYTES();
						}
					}
					else
					{
						array = (byte[])this.ValueVariant();
					}
				}
				this._value = array;
			}
			return array;
		}

		// Token: 0x06001D51 RID: 7505 RVA: 0x00250F18 File Offset: 0x00250318
		internal byte ValueByte()
		{
			DBStatus dbstatus = this.StatusValue();
			if (dbstatus == DBStatus.S_OK)
			{
				short dbType = this.DbType;
				byte b;
				if (dbType != 12)
				{
					if (dbType != 17)
					{
						throw ODB.ConversionRequired();
					}
					b = this.Value_UI1();
				}
				else
				{
					b = (byte)this.ValueVariant();
				}
				return b;
			}
			throw this.CheckTypeValueStatusValue(typeof(byte));
		}

		// Token: 0x06001D52 RID: 7506 RVA: 0x00250F70 File Offset: 0x00250370
		internal OleDbDataReader ValueChapter()
		{
			OleDbDataReader oleDbDataReader = (OleDbDataReader)this._value;
			if (oleDbDataReader == null)
			{
				DBStatus dbstatus = this.StatusValue();
				if (dbstatus != DBStatus.S_OK)
				{
					throw this.CheckTypeValueStatusValue(typeof(string));
				}
				short dbType = this.DbType;
				if (dbType != 136)
				{
					throw ODB.ConversionRequired();
				}
				oleDbDataReader = this.Value_HCHAPTER();
				this._value = oleDbDataReader;
			}
			return oleDbDataReader;
		}

		// Token: 0x06001D53 RID: 7507 RVA: 0x00250FD0 File Offset: 0x002503D0
		internal DateTime ValueDateTime()
		{
			DBStatus dbstatus = this.StatusValue();
			if (dbstatus == DBStatus.S_OK)
			{
				short dbType = this.DbType;
				if (dbType <= 12)
				{
					if (dbType == 7)
					{
						return this.Value_DATE();
					}
					if (dbType == 12)
					{
						return (DateTime)this.ValueVariant();
					}
				}
				else
				{
					if (dbType == 64)
					{
						return this.Value_FILETIME();
					}
					switch (dbType)
					{
					case 133:
						return this.Value_DBDATE();
					case 135:
						return this.Value_DBTIMESTAMP();
					}
				}
				throw ODB.ConversionRequired();
			}
			throw this.CheckTypeValueStatusValue(typeof(short));
		}

		// Token: 0x06001D54 RID: 7508 RVA: 0x00251068 File Offset: 0x00250468
		internal decimal ValueDecimal()
		{
			DBStatus dbstatus = this.StatusValue();
			if (dbstatus == DBStatus.S_OK)
			{
				short dbType = this.DbType;
				if (dbType <= 14)
				{
					if (dbType == 6)
					{
						return this.Value_CY();
					}
					switch (dbType)
					{
					case 12:
						return (decimal)this.ValueVariant();
					case 14:
						return this.Value_DECIMAL();
					}
				}
				else
				{
					if (dbType == 21)
					{
						return this.Value_UI8();
					}
					if (dbType == 131)
					{
						return this.Value_NUMERIC();
					}
				}
				throw ODB.ConversionRequired();
			}
			throw this.CheckTypeValueStatusValue(typeof(short));
		}

		// Token: 0x06001D55 RID: 7509 RVA: 0x00251108 File Offset: 0x00250508
		internal Guid ValueGuid()
		{
			DBStatus dbstatus = this.StatusValue();
			if (dbstatus != DBStatus.S_OK)
			{
				throw this.CheckTypeValueStatusValue(typeof(short));
			}
			short dbType = this.DbType;
			if (dbType == 72)
			{
				return this.Value_GUID();
			}
			throw ODB.ConversionRequired();
		}

		// Token: 0x06001D56 RID: 7510 RVA: 0x00251150 File Offset: 0x00250550
		internal short ValueInt16()
		{
			DBStatus dbstatus = this.StatusValue();
			if (dbstatus == DBStatus.S_OK)
			{
				short dbType = this.DbType;
				short num;
				if (dbType != 2)
				{
					if (dbType != 12)
					{
						if (dbType != 16)
						{
							throw ODB.ConversionRequired();
						}
						num = (short)this.Value_I1();
					}
					else
					{
						object obj = this.ValueVariant();
						if (obj is sbyte)
						{
							num = (short)((sbyte)obj);
						}
						else
						{
							num = (short)obj;
						}
					}
				}
				else
				{
					num = this.Value_I2();
				}
				return num;
			}
			throw this.CheckTypeValueStatusValue(typeof(short));
		}

		// Token: 0x06001D57 RID: 7511 RVA: 0x002511CC File Offset: 0x002505CC
		internal int ValueInt32()
		{
			DBStatus dbstatus = this.StatusValue();
			if (dbstatus == DBStatus.S_OK)
			{
				short dbType = this.DbType;
				int num;
				if (dbType != 3)
				{
					if (dbType != 12)
					{
						if (dbType != 18)
						{
							throw ODB.ConversionRequired();
						}
						num = (int)this.Value_UI2();
					}
					else
					{
						object obj = this.ValueVariant();
						if (obj is ushort)
						{
							num = (int)((ushort)obj);
						}
						else
						{
							num = (int)obj;
						}
					}
				}
				else
				{
					num = this.Value_I4();
				}
				return num;
			}
			throw this.CheckTypeValueStatusValue(typeof(int));
		}

		// Token: 0x06001D58 RID: 7512 RVA: 0x00251248 File Offset: 0x00250648
		internal long ValueInt64()
		{
			DBStatus dbstatus = this.StatusValue();
			if (dbstatus == DBStatus.S_OK)
			{
				short dbType = this.DbType;
				long num;
				if (dbType != 12)
				{
					switch (dbType)
					{
					case 19:
						num = (long)((ulong)this.Value_UI4());
						break;
					case 20:
						num = this.Value_I8();
						break;
					default:
						throw ODB.ConversionRequired();
					}
				}
				else
				{
					object obj = this.ValueVariant();
					if (obj is uint)
					{
						num = (long)((ulong)((uint)obj));
					}
					else
					{
						num = (long)obj;
					}
				}
				return num;
			}
			throw this.CheckTypeValueStatusValue(typeof(long));
		}

		// Token: 0x06001D59 RID: 7513 RVA: 0x002512CC File Offset: 0x002506CC
		internal float ValueSingle()
		{
			DBStatus dbstatus = this.StatusValue();
			if (dbstatus == DBStatus.S_OK)
			{
				short dbType = this.DbType;
				float num;
				if (dbType != 4)
				{
					if (dbType != 12)
					{
						throw ODB.ConversionRequired();
					}
					num = (float)this.ValueVariant();
				}
				else
				{
					num = this.Value_R4();
				}
				return num;
			}
			throw this.CheckTypeValueStatusValue(typeof(float));
		}

		// Token: 0x06001D5A RID: 7514 RVA: 0x00251328 File Offset: 0x00250728
		internal double ValueDouble()
		{
			DBStatus dbstatus = this.StatusValue();
			if (dbstatus == DBStatus.S_OK)
			{
				short dbType = this.DbType;
				double num;
				if (dbType != 5)
				{
					if (dbType != 12)
					{
						throw ODB.ConversionRequired();
					}
					num = (double)this.ValueVariant();
				}
				else
				{
					num = this.Value_R8();
				}
				return num;
			}
			throw this.CheckTypeValueStatusValue(typeof(double));
		}

		// Token: 0x06001D5B RID: 7515 RVA: 0x00251384 File Offset: 0x00250784
		internal string ValueString()
		{
			string text = (string)this._value;
			if (text == null)
			{
				DBStatus dbstatus = this.StatusValue();
				if (dbstatus == DBStatus.S_OK)
				{
					short dbType = this.DbType;
					if (dbType <= 12)
					{
						if (dbType == 8)
						{
							text = this.Value_BSTR();
							goto IL_00C0;
						}
						if (dbType == 12)
						{
							text = (string)this.ValueVariant();
							goto IL_00C0;
						}
					}
					else
					{
						if (dbType == 130)
						{
							text = this.Value_WSTR();
							goto IL_00C0;
						}
						if (dbType == 16514)
						{
							text = this.Value_ByRefWSTR();
							goto IL_00C0;
						}
					}
					throw ODB.ConversionRequired();
				}
				if (dbstatus != DBStatus.S_TRUNCATED)
				{
					throw this.CheckTypeValueStatusValue(typeof(string));
				}
				short dbType2 = this.DbType;
				if (dbType2 != 130)
				{
					if (dbType2 != 16514)
					{
						throw ODB.ConversionRequired();
					}
					text = this.Value_ByRefWSTR();
				}
				else
				{
					text = this.Value_WSTR();
				}
				IL_00C0:
				this._value = text;
			}
			return text;
		}

		// Token: 0x06001D5C RID: 7516 RVA: 0x0025145C File Offset: 0x0025085C
		private object ValueVariant()
		{
			object obj = this._value;
			if (obj == null)
			{
				obj = this.Value_VARIANT();
				this._value = obj;
			}
			return obj;
		}

		// Token: 0x06001D5D RID: 7517 RVA: 0x00251484 File Offset: 0x00250884
		private Exception CheckTypeValueStatusValue()
		{
			return this.CheckTypeValueStatusValue(this.ExpectedType);
		}

		// Token: 0x06001D5E RID: 7518 RVA: 0x002514A0 File Offset: 0x002508A0
		private Exception CheckTypeValueStatusValue(Type expectedType)
		{
			switch (this.StatusValue())
			{
			case DBStatus.S_OK:
			case DBStatus.E_CANTCONVERTVALUE:
			case DBStatus.S_TRUNCATED:
				return ODB.CantConvertValue();
			case DBStatus.E_BADACCESSOR:
				return ODB.BadAccessor();
			case DBStatus.S_ISNULL:
				return ADP.InvalidCast();
			case DBStatus.E_SIGNMISMATCH:
				return ODB.SignMismatch(expectedType);
			case DBStatus.E_DATAOVERFLOW:
				return ODB.DataOverflow(expectedType);
			case DBStatus.E_CANTCREATE:
				return ODB.CantCreate(expectedType);
			case DBStatus.E_UNAVAILABLE:
				return ODB.Unavailable(expectedType);
			default:
				return ODB.UnexpectedStatusValue(this.StatusValue());
			}
		}

		// Token: 0x0400108F RID: 4239
		private readonly OleDbDataReader _dataReader;

		// Token: 0x04001090 RID: 4240
		private readonly RowBinding _rowbinding;

		// Token: 0x04001091 RID: 4241
		private readonly Bindings _bindings;

		// Token: 0x04001092 RID: 4242
		private readonly OleDbParameter _parameter;

		// Token: 0x04001093 RID: 4243
		private readonly int _parameterChangeID;

		// Token: 0x04001094 RID: 4244
		private readonly int _offsetStatus;

		// Token: 0x04001095 RID: 4245
		private readonly int _offsetLength;

		// Token: 0x04001096 RID: 4246
		private readonly int _offsetValue;

		// Token: 0x04001097 RID: 4247
		private readonly int _ordinal;

		// Token: 0x04001098 RID: 4248
		private readonly int _maxLen;

		// Token: 0x04001099 RID: 4249
		private readonly short _wType;

		// Token: 0x0400109A RID: 4250
		private readonly byte _precision;

		// Token: 0x0400109B RID: 4251
		private readonly int _index;

		// Token: 0x0400109C RID: 4252
		private readonly int _indexForAccessor;

		// Token: 0x0400109D RID: 4253
		private readonly int _indexWithinAccessor;

		// Token: 0x0400109E RID: 4254
		private readonly bool _ifIRowsetElseIRow;

		// Token: 0x0400109F RID: 4255
		private int _valueBindingOffset;

		// Token: 0x040010A0 RID: 4256
		private int _valueBindingSize;

		// Token: 0x040010A1 RID: 4257
		internal StringMemHandle _sptr;

		// Token: 0x040010A2 RID: 4258
		private GCHandle _pinnedBuffer;

		// Token: 0x040010A3 RID: 4259
		private object _value;
	}
}
