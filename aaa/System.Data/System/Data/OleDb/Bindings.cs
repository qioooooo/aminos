using System;
using System.Data.Common;
using System.Text;

namespace System.Data.OleDb
{
	// Token: 0x0200020D RID: 525
	internal sealed class Bindings
	{
		// Token: 0x06001D5F RID: 7519 RVA: 0x0025151C File Offset: 0x0025091C
		private Bindings(int count)
		{
			this._count = count;
			this._dbbindings = new tagDBBINDING[count];
			for (int i = 0; i < this._dbbindings.Length; i++)
			{
				this._dbbindings[i] = new tagDBBINDING();
			}
			this._dbcolumns = new tagDBCOLUMNACCESS[count];
		}

		// Token: 0x06001D60 RID: 7520 RVA: 0x00251570 File Offset: 0x00250970
		internal Bindings(OleDbParameter[] parameters, int collectionChangeID)
			: this(parameters.Length)
		{
			this._bindInfo = new tagDBPARAMBINDINFO[parameters.Length];
			this._parameters = parameters;
			this._collectionChangeID = collectionChangeID;
			this._ifIRowsetElseIRow = true;
		}

		// Token: 0x06001D61 RID: 7521 RVA: 0x002515AC File Offset: 0x002509AC
		internal Bindings(OleDbDataReader dataReader, bool ifIRowsetElseIRow, int count)
			: this(count)
		{
			this._dataReader = dataReader;
			this._ifIRowsetElseIRow = ifIRowsetElseIRow;
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06001D62 RID: 7522 RVA: 0x002515D0 File Offset: 0x002509D0
		internal tagDBPARAMBINDINFO[] BindInfo
		{
			get
			{
				return this._bindInfo;
			}
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06001D63 RID: 7523 RVA: 0x002515E4 File Offset: 0x002509E4
		internal tagDBCOLUMNACCESS[] DBColumnAccess
		{
			get
			{
				return this._dbcolumns;
			}
		}

		// Token: 0x170003F2 RID: 1010
		// (set) Token: 0x06001D64 RID: 7524 RVA: 0x002515F8 File Offset: 0x002509F8
		internal int CurrentIndex
		{
			set
			{
				this._index = value;
			}
		}

		// Token: 0x06001D65 RID: 7525 RVA: 0x0025160C File Offset: 0x00250A0C
		internal ColumnBinding[] ColumnBindings()
		{
			return this._columnBindings;
		}

		// Token: 0x06001D66 RID: 7526 RVA: 0x00251620 File Offset: 0x00250A20
		internal OleDbParameter[] Parameters()
		{
			return this._parameters;
		}

		// Token: 0x06001D67 RID: 7527 RVA: 0x00251634 File Offset: 0x00250A34
		internal RowBinding RowBinding()
		{
			return this._rowBinding;
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06001D68 RID: 7528 RVA: 0x00251648 File Offset: 0x00250A48
		// (set) Token: 0x06001D69 RID: 7529 RVA: 0x0025165C File Offset: 0x00250A5C
		internal bool ForceRebind
		{
			get
			{
				return this._forceRebind;
			}
			set
			{
				this._forceRebind = value;
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (set) Token: 0x06001D6A RID: 7530 RVA: 0x00251670 File Offset: 0x00250A70
		internal IntPtr DataSourceType
		{
			set
			{
				this._bindInfo[this._index].pwszDataSourceType = value;
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (set) Token: 0x06001D6B RID: 7531 RVA: 0x00251694 File Offset: 0x00250A94
		internal IntPtr Name
		{
			set
			{
				this._bindInfo[this._index].pwszName = value;
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06001D6C RID: 7532 RVA: 0x002516B8 File Offset: 0x00250AB8
		// (set) Token: 0x06001D6D RID: 7533 RVA: 0x002516EC File Offset: 0x00250AEC
		internal IntPtr ParamSize
		{
			get
			{
				if (this._bindInfo != null)
				{
					return this._bindInfo[this._index].ulParamSize;
				}
				return IntPtr.Zero;
			}
			set
			{
				this._bindInfo[this._index].ulParamSize = value;
			}
		}

		// Token: 0x170003F7 RID: 1015
		// (set) Token: 0x06001D6E RID: 7534 RVA: 0x00251710 File Offset: 0x00250B10
		internal int Flags
		{
			set
			{
				this._bindInfo[this._index].dwFlags = value;
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (set) Token: 0x06001D6F RID: 7535 RVA: 0x00251734 File Offset: 0x00250B34
		internal IntPtr Ordinal
		{
			set
			{
				this._dbbindings[this._index].iOrdinal = value;
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (set) Token: 0x06001D70 RID: 7536 RVA: 0x00251754 File Offset: 0x00250B54
		internal int Part
		{
			set
			{
				this._dbbindings[this._index].dwPart = value;
			}
		}

		// Token: 0x170003FA RID: 1018
		// (set) Token: 0x06001D71 RID: 7537 RVA: 0x00251774 File Offset: 0x00250B74
		internal int ParamIO
		{
			set
			{
				this._dbbindings[this._index].eParamIO = value;
			}
		}

		// Token: 0x170003FB RID: 1019
		// (set) Token: 0x06001D72 RID: 7538 RVA: 0x00251794 File Offset: 0x00250B94
		internal int MaxLen
		{
			set
			{
				this._dbbindings[this._index].obStatus = (IntPtr)this._dataBufferSize;
				this._dbbindings[this._index].obLength = (IntPtr)(this._dataBufferSize + ADP.PtrSize);
				this._dbbindings[this._index].obValue = (IntPtr)(this._dataBufferSize + ADP.PtrSize + ADP.PtrSize);
				this._dataBufferSize += ADP.PtrSize + ADP.PtrSize;
				int dbType = this.DbType;
				if (dbType <= 12)
				{
					if (dbType != 8 && dbType != 12)
					{
						goto IL_00E8;
					}
				}
				else
				{
					switch (dbType)
					{
					case 136:
					case 138:
						break;
					case 137:
						goto IL_00E8;
					default:
						switch (dbType)
						{
						case 16512:
						case 16514:
							break;
						case 16513:
							goto IL_00E8;
						default:
							goto IL_00E8;
						}
						break;
					}
				}
				this._dataBufferSize += global::System.Data.OleDb.RowBinding.AlignDataSize(value * 2);
				this._needToReset = true;
				goto IL_00FB;
				IL_00E8:
				this._dataBufferSize += global::System.Data.OleDb.RowBinding.AlignDataSize(value);
				IL_00FB:
				this._dbbindings[this._index].cbMaxLen = (IntPtr)value;
				this._dbcolumns[this._index].cbMaxLen = (IntPtr)value;
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06001D73 RID: 7539 RVA: 0x002518D0 File Offset: 0x00250CD0
		// (set) Token: 0x06001D74 RID: 7540 RVA: 0x002518F0 File Offset: 0x00250CF0
		internal int DbType
		{
			get
			{
				return (int)this._dbbindings[this._index].wType;
			}
			set
			{
				this._dbbindings[this._index].wType = (short)value;
				this._dbcolumns[this._index].wType = (short)value;
			}
		}

		// Token: 0x170003FD RID: 1021
		// (set) Token: 0x06001D75 RID: 7541 RVA: 0x0025192C File Offset: 0x00250D2C
		internal byte Precision
		{
			set
			{
				if (this._bindInfo != null)
				{
					this._bindInfo[this._index].bPrecision = value;
				}
				this._dbbindings[this._index].bPrecision = value;
				this._dbcolumns[this._index].bPrecision = value;
			}
		}

		// Token: 0x170003FE RID: 1022
		// (set) Token: 0x06001D76 RID: 7542 RVA: 0x00251984 File Offset: 0x00250D84
		internal byte Scale
		{
			set
			{
				if (this._bindInfo != null)
				{
					this._bindInfo[this._index].bScale = value;
				}
				this._dbbindings[this._index].bScale = value;
				this._dbcolumns[this._index].bScale = value;
			}
		}

		// Token: 0x06001D77 RID: 7543 RVA: 0x002519DC File Offset: 0x00250DDC
		internal int AllocateForAccessor(OleDbDataReader dataReader, int indexStart, int indexForAccessor)
		{
			RowBinding rowBinding = global::System.Data.OleDb.RowBinding.CreateBuffer(this._count, this._dataBufferSize, this._needToReset);
			this._rowBinding = rowBinding;
			ColumnBinding[] array = rowBinding.SetBindings(dataReader, this, indexStart, indexForAccessor, this._parameters, this._dbbindings, this._ifIRowsetElseIRow);
			this._columnBindings = array;
			if (!this._ifIRowsetElseIRow)
			{
				for (int i = 0; i < array.Length; i++)
				{
					this._dbcolumns[i].pData = rowBinding.DangerousGetDataPtr(array[i].ValueOffset);
				}
			}
			return indexStart + array.Length;
		}

		// Token: 0x06001D78 RID: 7544 RVA: 0x00251A68 File Offset: 0x00250E68
		internal void ApplyInputParameters()
		{
			ColumnBinding[] array = this.ColumnBindings();
			OleDbParameter[] array2 = this.Parameters();
			this.RowBinding().StartDataBlock();
			for (int i = 0; i < array2.Length; i++)
			{
				if (ADP.IsDirection(array2[i], ParameterDirection.Input))
				{
					array[i].SetOffset(array2[i].Offset);
					array[i].Value(array2[i].GetCoercedValue());
				}
				else
				{
					array2[i].Value = null;
				}
			}
		}

		// Token: 0x06001D79 RID: 7545 RVA: 0x00251AD4 File Offset: 0x00250ED4
		internal void ApplyOutputParameters()
		{
			ColumnBinding[] array = this.ColumnBindings();
			OleDbParameter[] array2 = this.Parameters();
			for (int i = 0; i < array2.Length; i++)
			{
				if (ADP.IsDirection(array2[i], ParameterDirection.Output))
				{
					array2[i].Value = array[i].Value();
				}
			}
			this.CleanupBindings();
		}

		// Token: 0x06001D7A RID: 7546 RVA: 0x00251B20 File Offset: 0x00250F20
		internal bool AreParameterBindingsInvalid(OleDbParameterCollection collection)
		{
			ColumnBinding[] array = this.ColumnBindings();
			if (!this.ForceRebind && collection.ChangeID == this._collectionChangeID && this._parameters.Length == collection.Count)
			{
				for (int i = 0; i < array.Length; i++)
				{
					ColumnBinding columnBinding = array[i];
					if (columnBinding.IsParameterBindingInvalid(collection[i]))
					{
						return true;
					}
				}
				return false;
			}
			return true;
		}

		// Token: 0x06001D7B RID: 7547 RVA: 0x00251B80 File Offset: 0x00250F80
		internal void CleanupBindings()
		{
			RowBinding rowBinding = this.RowBinding();
			if (rowBinding != null)
			{
				rowBinding.ResetValues();
				foreach (ColumnBinding columnBinding in this.ColumnBindings())
				{
					if (columnBinding != null)
					{
						columnBinding.ResetValue();
					}
				}
			}
		}

		// Token: 0x06001D7C RID: 7548 RVA: 0x00251BC0 File Offset: 0x00250FC0
		internal void CloseFromConnection()
		{
			if (this._rowBinding != null)
			{
				this._rowBinding.CloseFromConnection();
			}
			this.Dispose();
		}

		// Token: 0x06001D7D RID: 7549 RVA: 0x00251BE8 File Offset: 0x00250FE8
		internal OleDbHResult CreateAccessor(UnsafeNativeMethods.IAccessor iaccessor, int flags)
		{
			return this._rowBinding.CreateAccessor(iaccessor, flags, this._columnBindings);
		}

		// Token: 0x06001D7E RID: 7550 RVA: 0x00251C08 File Offset: 0x00251008
		public void Dispose()
		{
			this._parameters = null;
			this._dataReader = null;
			this._columnBindings = null;
			RowBinding rowBinding = this._rowBinding;
			this._rowBinding = null;
			if (rowBinding != null)
			{
				rowBinding.Dispose();
			}
		}

		// Token: 0x06001D7F RID: 7551 RVA: 0x00251C44 File Offset: 0x00251044
		internal void GuidKindName(Guid guid, int eKind, IntPtr propid)
		{
			tagDBCOLUMNACCESS[] dbcolumnAccess = this.DBColumnAccess;
			dbcolumnAccess[this._index].columnid.uGuid = guid;
			dbcolumnAccess[this._index].columnid.eKind = eKind;
			dbcolumnAccess[this._index].columnid.ulPropid = propid;
		}

		// Token: 0x06001D80 RID: 7552 RVA: 0x00251CA0 File Offset: 0x002510A0
		internal void ParameterStatus(StringBuilder builder)
		{
			ColumnBinding[] array = this.ColumnBindings();
			for (int i = 0; i < array.Length; i++)
			{
				ODB.CommandParameterStatus(builder, i, array[i].StatusValue());
			}
		}

		// Token: 0x040010A4 RID: 4260
		private readonly tagDBPARAMBINDINFO[] _bindInfo;

		// Token: 0x040010A5 RID: 4261
		private readonly tagDBBINDING[] _dbbindings;

		// Token: 0x040010A6 RID: 4262
		private readonly tagDBCOLUMNACCESS[] _dbcolumns;

		// Token: 0x040010A7 RID: 4263
		private OleDbParameter[] _parameters;

		// Token: 0x040010A8 RID: 4264
		private int _collectionChangeID;

		// Token: 0x040010A9 RID: 4265
		private OleDbDataReader _dataReader;

		// Token: 0x040010AA RID: 4266
		private ColumnBinding[] _columnBindings;

		// Token: 0x040010AB RID: 4267
		private RowBinding _rowBinding;

		// Token: 0x040010AC RID: 4268
		private int _index;

		// Token: 0x040010AD RID: 4269
		private int _count;

		// Token: 0x040010AE RID: 4270
		private int _dataBufferSize;

		// Token: 0x040010AF RID: 4271
		private bool _ifIRowsetElseIRow;

		// Token: 0x040010B0 RID: 4272
		private bool _forceRebind;

		// Token: 0x040010B1 RID: 4273
		private bool _needToReset;
	}
}
