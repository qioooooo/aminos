using System;
using System.Collections;
using System.ComponentModel;

namespace System.Data.Common
{
	// Token: 0x020000A2 RID: 162
	public abstract class DbDataReader : MarshalByRefObject, IDataReader, IDisposable, IDataRecord, IEnumerable
	{
		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000AB6 RID: 2742
		public abstract int Depth { get; }

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000AB7 RID: 2743
		public abstract int FieldCount { get; }

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000AB8 RID: 2744
		public abstract bool HasRows { get; }

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000AB9 RID: 2745
		public abstract bool IsClosed { get; }

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000ABA RID: 2746
		public abstract int RecordsAffected { get; }

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000ABB RID: 2747 RVA: 0x001F45A8 File Offset: 0x001F39A8
		public virtual int VisibleFieldCount
		{
			get
			{
				return this.FieldCount;
			}
		}

		// Token: 0x17000163 RID: 355
		public abstract object this[int ordinal] { get; }

		// Token: 0x17000164 RID: 356
		public abstract object this[string name] { get; }

		// Token: 0x06000ABE RID: 2750
		public abstract void Close();

		// Token: 0x06000ABF RID: 2751 RVA: 0x001F45BC File Offset: 0x001F39BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x001F45D0 File Offset: 0x001F39D0
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Close();
			}
		}

		// Token: 0x06000AC1 RID: 2753
		public abstract string GetDataTypeName(int ordinal);

		// Token: 0x06000AC2 RID: 2754
		[EditorBrowsable(EditorBrowsableState.Never)]
		public abstract IEnumerator GetEnumerator();

		// Token: 0x06000AC3 RID: 2755
		public abstract Type GetFieldType(int ordinal);

		// Token: 0x06000AC4 RID: 2756
		public abstract string GetName(int ordinal);

		// Token: 0x06000AC5 RID: 2757
		public abstract int GetOrdinal(string name);

		// Token: 0x06000AC6 RID: 2758
		public abstract DataTable GetSchemaTable();

		// Token: 0x06000AC7 RID: 2759
		public abstract bool GetBoolean(int ordinal);

		// Token: 0x06000AC8 RID: 2760
		public abstract byte GetByte(int ordinal);

		// Token: 0x06000AC9 RID: 2761
		public abstract long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length);

		// Token: 0x06000ACA RID: 2762
		public abstract char GetChar(int ordinal);

		// Token: 0x06000ACB RID: 2763
		public abstract long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length);

		// Token: 0x06000ACC RID: 2764 RVA: 0x001F45E8 File Offset: 0x001F39E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public DbDataReader GetData(int ordinal)
		{
			return this.GetDbDataReader(ordinal);
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x001F45FC File Offset: 0x001F39FC
		IDataReader IDataRecord.GetData(int ordinal)
		{
			return this.GetDbDataReader(ordinal);
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x001F4610 File Offset: 0x001F3A10
		protected virtual DbDataReader GetDbDataReader(int ordinal)
		{
			throw ADP.NotSupported();
		}

		// Token: 0x06000ACF RID: 2767
		public abstract DateTime GetDateTime(int ordinal);

		// Token: 0x06000AD0 RID: 2768
		public abstract decimal GetDecimal(int ordinal);

		// Token: 0x06000AD1 RID: 2769
		public abstract double GetDouble(int ordinal);

		// Token: 0x06000AD2 RID: 2770
		public abstract float GetFloat(int ordinal);

		// Token: 0x06000AD3 RID: 2771
		public abstract Guid GetGuid(int ordinal);

		// Token: 0x06000AD4 RID: 2772
		public abstract short GetInt16(int ordinal);

		// Token: 0x06000AD5 RID: 2773
		public abstract int GetInt32(int ordinal);

		// Token: 0x06000AD6 RID: 2774
		public abstract long GetInt64(int ordinal);

		// Token: 0x06000AD7 RID: 2775 RVA: 0x001F4624 File Offset: 0x001F3A24
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual Type GetProviderSpecificFieldType(int ordinal)
		{
			return this.GetFieldType(ordinal);
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x001F4638 File Offset: 0x001F3A38
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual object GetProviderSpecificValue(int ordinal)
		{
			return this.GetValue(ordinal);
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x001F464C File Offset: 0x001F3A4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual int GetProviderSpecificValues(object[] values)
		{
			return this.GetValues(values);
		}

		// Token: 0x06000ADA RID: 2778
		public abstract string GetString(int ordinal);

		// Token: 0x06000ADB RID: 2779
		public abstract object GetValue(int ordinal);

		// Token: 0x06000ADC RID: 2780
		public abstract int GetValues(object[] values);

		// Token: 0x06000ADD RID: 2781
		public abstract bool IsDBNull(int ordinal);

		// Token: 0x06000ADE RID: 2782
		public abstract bool NextResult();

		// Token: 0x06000ADF RID: 2783
		public abstract bool Read();
	}
}
