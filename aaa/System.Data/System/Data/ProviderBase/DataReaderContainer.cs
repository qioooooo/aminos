using System;
using System.Data.Common;

namespace System.Data.ProviderBase
{
	// Token: 0x02000263 RID: 611
	internal abstract class DataReaderContainer
	{
		// Token: 0x060020E1 RID: 8417 RVA: 0x00264F10 File Offset: 0x00264310
		internal static DataReaderContainer Create(IDataReader dataReader, bool returnProviderSpecificTypes)
		{
			if (returnProviderSpecificTypes)
			{
				DbDataReader dbDataReader = dataReader as DbDataReader;
				if (dbDataReader != null)
				{
					return new DataReaderContainer.ProviderSpecificDataReader(dataReader, dbDataReader);
				}
			}
			return new DataReaderContainer.CommonLanguageSubsetDataReader(dataReader);
		}

		// Token: 0x060020E2 RID: 8418 RVA: 0x00264F38 File Offset: 0x00264338
		protected DataReaderContainer(IDataReader dataReader)
		{
			this._dataReader = dataReader;
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x060020E3 RID: 8419 RVA: 0x00264F54 File Offset: 0x00264354
		internal int FieldCount
		{
			get
			{
				return this._fieldCount;
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x060020E4 RID: 8420
		internal abstract bool ReturnProviderSpecificTypes { get; }

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x060020E5 RID: 8421
		protected abstract int VisibleFieldCount { get; }

		// Token: 0x060020E6 RID: 8422
		internal abstract Type GetFieldType(int ordinal);

		// Token: 0x060020E7 RID: 8423
		internal abstract object GetValue(int ordinal);

		// Token: 0x060020E8 RID: 8424
		internal abstract int GetValues(object[] values);

		// Token: 0x060020E9 RID: 8425 RVA: 0x00264F68 File Offset: 0x00264368
		internal string GetName(int ordinal)
		{
			string name = this._dataReader.GetName(ordinal);
			if (name == null)
			{
				return "";
			}
			return name;
		}

		// Token: 0x060020EA RID: 8426 RVA: 0x00264F8C File Offset: 0x0026438C
		internal DataTable GetSchemaTable()
		{
			return this._dataReader.GetSchemaTable();
		}

		// Token: 0x060020EB RID: 8427 RVA: 0x00264FA4 File Offset: 0x002643A4
		internal bool NextResult()
		{
			this._fieldCount = 0;
			if (this._dataReader.NextResult())
			{
				this._fieldCount = this.VisibleFieldCount;
				return true;
			}
			return false;
		}

		// Token: 0x060020EC RID: 8428 RVA: 0x00264FD4 File Offset: 0x002643D4
		internal bool Read()
		{
			return this._dataReader.Read();
		}

		// Token: 0x04001542 RID: 5442
		protected readonly IDataReader _dataReader;

		// Token: 0x04001543 RID: 5443
		protected int _fieldCount;

		// Token: 0x02000264 RID: 612
		private sealed class ProviderSpecificDataReader : DataReaderContainer
		{
			// Token: 0x060020ED RID: 8429 RVA: 0x00264FEC File Offset: 0x002643EC
			internal ProviderSpecificDataReader(IDataReader dataReader, DbDataReader dbDataReader)
				: base(dataReader)
			{
				this._providerSpecificDataReader = dbDataReader;
				this._fieldCount = this.VisibleFieldCount;
			}

			// Token: 0x17000489 RID: 1161
			// (get) Token: 0x060020EE RID: 8430 RVA: 0x00265014 File Offset: 0x00264414
			internal override bool ReturnProviderSpecificTypes
			{
				get
				{
					return true;
				}
			}

			// Token: 0x1700048A RID: 1162
			// (get) Token: 0x060020EF RID: 8431 RVA: 0x00265024 File Offset: 0x00264424
			protected override int VisibleFieldCount
			{
				get
				{
					int visibleFieldCount = this._providerSpecificDataReader.VisibleFieldCount;
					if (0 > visibleFieldCount)
					{
						return 0;
					}
					return visibleFieldCount;
				}
			}

			// Token: 0x060020F0 RID: 8432 RVA: 0x00265044 File Offset: 0x00264444
			internal override Type GetFieldType(int ordinal)
			{
				return this._providerSpecificDataReader.GetProviderSpecificFieldType(ordinal);
			}

			// Token: 0x060020F1 RID: 8433 RVA: 0x00265060 File Offset: 0x00264460
			internal override object GetValue(int ordinal)
			{
				return this._providerSpecificDataReader.GetProviderSpecificValue(ordinal);
			}

			// Token: 0x060020F2 RID: 8434 RVA: 0x0026507C File Offset: 0x0026447C
			internal override int GetValues(object[] values)
			{
				return this._providerSpecificDataReader.GetProviderSpecificValues(values);
			}

			// Token: 0x04001544 RID: 5444
			private DbDataReader _providerSpecificDataReader;
		}

		// Token: 0x02000265 RID: 613
		private sealed class CommonLanguageSubsetDataReader : DataReaderContainer
		{
			// Token: 0x060020F3 RID: 8435 RVA: 0x00265098 File Offset: 0x00264498
			internal CommonLanguageSubsetDataReader(IDataReader dataReader)
				: base(dataReader)
			{
				this._fieldCount = this.VisibleFieldCount;
			}

			// Token: 0x1700048B RID: 1163
			// (get) Token: 0x060020F4 RID: 8436 RVA: 0x002650B8 File Offset: 0x002644B8
			internal override bool ReturnProviderSpecificTypes
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700048C RID: 1164
			// (get) Token: 0x060020F5 RID: 8437 RVA: 0x002650C8 File Offset: 0x002644C8
			protected override int VisibleFieldCount
			{
				get
				{
					int fieldCount = this._dataReader.FieldCount;
					if (0 > fieldCount)
					{
						return 0;
					}
					return fieldCount;
				}
			}

			// Token: 0x060020F6 RID: 8438 RVA: 0x002650E8 File Offset: 0x002644E8
			internal override Type GetFieldType(int ordinal)
			{
				return this._dataReader.GetFieldType(ordinal);
			}

			// Token: 0x060020F7 RID: 8439 RVA: 0x00265104 File Offset: 0x00264504
			internal override object GetValue(int ordinal)
			{
				return this._dataReader.GetValue(ordinal);
			}

			// Token: 0x060020F8 RID: 8440 RVA: 0x00265120 File Offset: 0x00264520
			internal override int GetValues(object[] values)
			{
				return this._dataReader.GetValues(values);
			}
		}
	}
}
