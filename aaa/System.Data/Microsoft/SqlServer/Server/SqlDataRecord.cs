using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.ProviderBase;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000050 RID: 80
	public class SqlDataRecord : IDataRecord
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000317 RID: 791 RVA: 0x001CDC68 File Offset: 0x001CD068
		public virtual int FieldCount
		{
			get
			{
				this.EnsureSubclassOverride();
				return this._columnMetaData.Length;
			}
		}

		// Token: 0x06000318 RID: 792 RVA: 0x001CDC84 File Offset: 0x001CD084
		public virtual string GetName(int ordinal)
		{
			this.EnsureSubclassOverride();
			return this.GetSqlMetaData(ordinal).Name;
		}

		// Token: 0x06000319 RID: 793 RVA: 0x001CDCA4 File Offset: 0x001CD0A4
		public virtual string GetDataTypeName(int ordinal)
		{
			this.EnsureSubclassOverride();
			SqlMetaData sqlMetaData = this.GetSqlMetaData(ordinal);
			if (SqlDbType.Udt == sqlMetaData.SqlDbType)
			{
				return sqlMetaData.UdtTypeName;
			}
			return MetaType.GetMetaTypeFromSqlDbType(sqlMetaData.SqlDbType, false).TypeName;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x001CDCE4 File Offset: 0x001CD0E4
		public virtual Type GetFieldType(int ordinal)
		{
			this.EnsureSubclassOverride();
			if (SqlDbType.Udt == this.GetSqlMetaData(ordinal).SqlDbType)
			{
				return this.GetSqlMetaData(ordinal).Type;
			}
			SqlMetaData sqlMetaData = this.GetSqlMetaData(ordinal);
			return MetaType.GetMetaTypeFromSqlDbType(sqlMetaData.SqlDbType, false).ClassType;
		}

		// Token: 0x0600031B RID: 795 RVA: 0x001CDD30 File Offset: 0x001CD130
		public virtual object GetValue(int ordinal)
		{
			this.EnsureSubclassOverride();
			SmiMetaData smiMetaData = this.GetSmiMetaData(ordinal);
			if (this.SmiVersion >= 210UL)
			{
				return ValueUtilsSmi.GetValue200(this._eventSink, this._recordBuffer, ordinal, smiMetaData, this._recordContext);
			}
			return ValueUtilsSmi.GetValue(this._eventSink, this._recordBuffer, ordinal, smiMetaData, this._recordContext);
		}

		// Token: 0x0600031C RID: 796 RVA: 0x001CDD8C File Offset: 0x001CD18C
		public virtual int GetValues(object[] values)
		{
			this.EnsureSubclassOverride();
			if (values == null)
			{
				throw ADP.ArgumentNull("values");
			}
			int num = ((values.Length < this.FieldCount) ? values.Length : this.FieldCount);
			for (int i = 0; i < num; i++)
			{
				values[i] = this.GetValue(i);
			}
			return num;
		}

		// Token: 0x0600031D RID: 797 RVA: 0x001CDDDC File Offset: 0x001CD1DC
		public virtual int GetOrdinal(string name)
		{
			this.EnsureSubclassOverride();
			if (this._fieldNameLookup == null)
			{
				string[] array = new string[this.FieldCount];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this.GetSqlMetaData(i).Name;
				}
				this._fieldNameLookup = new FieldNameLookup(array, -1);
			}
			return this._fieldNameLookup.GetOrdinal(name);
		}

		// Token: 0x1700005E RID: 94
		public virtual object this[int ordinal]
		{
			get
			{
				this.EnsureSubclassOverride();
				return this.GetValue(ordinal);
			}
		}

		// Token: 0x1700005F RID: 95
		public virtual object this[string name]
		{
			get
			{
				this.EnsureSubclassOverride();
				return this.GetValue(this.GetOrdinal(name));
			}
		}

		// Token: 0x06000320 RID: 800 RVA: 0x001CDE78 File Offset: 0x001CD278
		public virtual bool GetBoolean(int ordinal)
		{
			this.EnsureSubclassOverride();
			return ValueUtilsSmi.GetBoolean(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal));
		}

		// Token: 0x06000321 RID: 801 RVA: 0x001CDEA4 File Offset: 0x001CD2A4
		public virtual byte GetByte(int ordinal)
		{
			this.EnsureSubclassOverride();
			return ValueUtilsSmi.GetByte(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal));
		}

		// Token: 0x06000322 RID: 802 RVA: 0x001CDED0 File Offset: 0x001CD2D0
		public virtual long GetBytes(int ordinal, long fieldOffset, byte[] buffer, int bufferOffset, int length)
		{
			this.EnsureSubclassOverride();
			return ValueUtilsSmi.GetBytes(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), fieldOffset, buffer, bufferOffset, length, true);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x001CDF04 File Offset: 0x001CD304
		public virtual char GetChar(int ordinal)
		{
			this.EnsureSubclassOverride();
			throw ADP.NotSupported();
		}

		// Token: 0x06000324 RID: 804 RVA: 0x001CDF1C File Offset: 0x001CD31C
		public virtual long GetChars(int ordinal, long fieldOffset, char[] buffer, int bufferOffset, int length)
		{
			this.EnsureSubclassOverride();
			return ValueUtilsSmi.GetChars(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), fieldOffset, buffer, bufferOffset, length);
		}

		// Token: 0x06000325 RID: 805 RVA: 0x001CDF50 File Offset: 0x001CD350
		public virtual Guid GetGuid(int ordinal)
		{
			this.EnsureSubclassOverride();
			return ValueUtilsSmi.GetGuid(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal));
		}

		// Token: 0x06000326 RID: 806 RVA: 0x001CDF7C File Offset: 0x001CD37C
		public virtual short GetInt16(int ordinal)
		{
			this.EnsureSubclassOverride();
			return ValueUtilsSmi.GetInt16(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal));
		}

		// Token: 0x06000327 RID: 807 RVA: 0x001CDFA8 File Offset: 0x001CD3A8
		public virtual int GetInt32(int ordinal)
		{
			this.EnsureSubclassOverride();
			return ValueUtilsSmi.GetInt32(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal));
		}

		// Token: 0x06000328 RID: 808 RVA: 0x001CDFD4 File Offset: 0x001CD3D4
		public virtual long GetInt64(int ordinal)
		{
			this.EnsureSubclassOverride();
			return ValueUtilsSmi.GetInt64(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal));
		}

		// Token: 0x06000329 RID: 809 RVA: 0x001CE000 File Offset: 0x001CD400
		public virtual float GetFloat(int ordinal)
		{
			this.EnsureSubclassOverride();
			return ValueUtilsSmi.GetSingle(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal));
		}

		// Token: 0x0600032A RID: 810 RVA: 0x001CE02C File Offset: 0x001CD42C
		public virtual double GetDouble(int ordinal)
		{
			this.EnsureSubclassOverride();
			return ValueUtilsSmi.GetDouble(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal));
		}

		// Token: 0x0600032B RID: 811 RVA: 0x001CE058 File Offset: 0x001CD458
		public virtual string GetString(int ordinal)
		{
			this.EnsureSubclassOverride();
			SmiMetaData smiMetaData = this.GetSmiMetaData(ordinal);
			if (this._usesStringStorageForXml && SqlDbType.Xml == smiMetaData.SqlDbType)
			{
				return ValueUtilsSmi.GetString(this._eventSink, this._recordBuffer, ordinal, SqlDataRecord.__maxNVarCharForXml);
			}
			return ValueUtilsSmi.GetString(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal));
		}

		// Token: 0x0600032C RID: 812 RVA: 0x001CE0B8 File Offset: 0x001CD4B8
		public virtual decimal GetDecimal(int ordinal)
		{
			this.EnsureSubclassOverride();
			return ValueUtilsSmi.GetDecimal(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal));
		}

		// Token: 0x0600032D RID: 813 RVA: 0x001CE0E4 File Offset: 0x001CD4E4
		public virtual DateTime GetDateTime(int ordinal)
		{
			this.EnsureSubclassOverride();
			return ValueUtilsSmi.GetDateTime(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal));
		}

		// Token: 0x0600032E RID: 814 RVA: 0x001CE110 File Offset: 0x001CD510
		public virtual DateTimeOffset GetDateTimeOffset(int ordinal)
		{
			this.EnsureSubclassOverride();
			return ValueUtilsSmi.GetDateTimeOffset(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal));
		}

		// Token: 0x0600032F RID: 815 RVA: 0x001CE13C File Offset: 0x001CD53C
		public virtual TimeSpan GetTimeSpan(int ordinal)
		{
			this.EnsureSubclassOverride();
			return ValueUtilsSmi.GetTimeSpan(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal));
		}

		// Token: 0x06000330 RID: 816 RVA: 0x001CE168 File Offset: 0x001CD568
		[EditorBrowsable(EditorBrowsableState.Never)]
		IDataReader IDataRecord.GetData(int ordinal)
		{
			throw ADP.NotSupported();
		}

		// Token: 0x06000331 RID: 817 RVA: 0x001CE17C File Offset: 0x001CD57C
		public virtual bool IsDBNull(int ordinal)
		{
			this.EnsureSubclassOverride();
			this.ThrowIfInvalidOrdinal(ordinal);
			return ValueUtilsSmi.IsDBNull(this._eventSink, this._recordBuffer, ordinal);
		}

		// Token: 0x06000332 RID: 818 RVA: 0x001CE1A8 File Offset: 0x001CD5A8
		public virtual SqlMetaData GetSqlMetaData(int ordinal)
		{
			this.EnsureSubclassOverride();
			return this._columnMetaData[ordinal];
		}

		// Token: 0x06000333 RID: 819 RVA: 0x001CE1C4 File Offset: 0x001CD5C4
		public virtual Type GetSqlFieldType(int ordinal)
		{
			this.EnsureSubclassOverride();
			SqlMetaData sqlMetaData = this.GetSqlMetaData(ordinal);
			return MetaType.GetMetaTypeFromSqlDbType(sqlMetaData.SqlDbType, false).SqlType;
		}

		// Token: 0x06000334 RID: 820 RVA: 0x001CE1F0 File Offset: 0x001CD5F0
		public virtual object GetSqlValue(int ordinal)
		{
			this.EnsureSubclassOverride();
			SmiMetaData smiMetaData = this.GetSmiMetaData(ordinal);
			if (this.SmiVersion >= 210UL)
			{
				return ValueUtilsSmi.GetSqlValue200(this._eventSink, this._recordBuffer, ordinal, smiMetaData, this._recordContext);
			}
			return ValueUtilsSmi.GetSqlValue(this._eventSink, this._recordBuffer, ordinal, smiMetaData, this._recordContext);
		}

		// Token: 0x06000335 RID: 821 RVA: 0x001CE24C File Offset: 0x001CD64C
		public virtual int GetSqlValues(object[] values)
		{
			this.EnsureSubclassOverride();
			if (values == null)
			{
				throw ADP.ArgumentNull("values");
			}
			int num = ((values.Length < this.FieldCount) ? values.Length : this.FieldCount);
			for (int i = 0; i < num; i++)
			{
				values[i] = this.GetSqlValue(i);
			}
			return num;
		}

		// Token: 0x06000336 RID: 822 RVA: 0x001CE29C File Offset: 0x001CD69C
		public virtual SqlBinary GetSqlBinary(int ordinal)
		{
			this.EnsureSubclassOverride();
			return ValueUtilsSmi.GetSqlBinary(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal));
		}

		// Token: 0x06000337 RID: 823 RVA: 0x001CE2C8 File Offset: 0x001CD6C8
		public virtual SqlBytes GetSqlBytes(int ordinal)
		{
			this.EnsureSubclassOverride();
			return ValueUtilsSmi.GetSqlBytes(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), this._recordContext);
		}

		// Token: 0x06000338 RID: 824 RVA: 0x001CE2FC File Offset: 0x001CD6FC
		public virtual SqlXml GetSqlXml(int ordinal)
		{
			this.EnsureSubclassOverride();
			return ValueUtilsSmi.GetSqlXml(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), this._recordContext);
		}

		// Token: 0x06000339 RID: 825 RVA: 0x001CE330 File Offset: 0x001CD730
		public virtual SqlBoolean GetSqlBoolean(int ordinal)
		{
			this.EnsureSubclassOverride();
			return ValueUtilsSmi.GetSqlBoolean(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal));
		}

		// Token: 0x0600033A RID: 826 RVA: 0x001CE35C File Offset: 0x001CD75C
		public virtual SqlByte GetSqlByte(int ordinal)
		{
			this.EnsureSubclassOverride();
			return ValueUtilsSmi.GetSqlByte(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal));
		}

		// Token: 0x0600033B RID: 827 RVA: 0x001CE388 File Offset: 0x001CD788
		public virtual SqlChars GetSqlChars(int ordinal)
		{
			this.EnsureSubclassOverride();
			return ValueUtilsSmi.GetSqlChars(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), this._recordContext);
		}

		// Token: 0x0600033C RID: 828 RVA: 0x001CE3BC File Offset: 0x001CD7BC
		public virtual SqlInt16 GetSqlInt16(int ordinal)
		{
			this.EnsureSubclassOverride();
			return ValueUtilsSmi.GetSqlInt16(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal));
		}

		// Token: 0x0600033D RID: 829 RVA: 0x001CE3E8 File Offset: 0x001CD7E8
		public virtual SqlInt32 GetSqlInt32(int ordinal)
		{
			this.EnsureSubclassOverride();
			return ValueUtilsSmi.GetSqlInt32(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal));
		}

		// Token: 0x0600033E RID: 830 RVA: 0x001CE414 File Offset: 0x001CD814
		public virtual SqlInt64 GetSqlInt64(int ordinal)
		{
			this.EnsureSubclassOverride();
			return ValueUtilsSmi.GetSqlInt64(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal));
		}

		// Token: 0x0600033F RID: 831 RVA: 0x001CE440 File Offset: 0x001CD840
		public virtual SqlSingle GetSqlSingle(int ordinal)
		{
			this.EnsureSubclassOverride();
			return ValueUtilsSmi.GetSqlSingle(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal));
		}

		// Token: 0x06000340 RID: 832 RVA: 0x001CE46C File Offset: 0x001CD86C
		public virtual SqlDouble GetSqlDouble(int ordinal)
		{
			this.EnsureSubclassOverride();
			return ValueUtilsSmi.GetSqlDouble(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal));
		}

		// Token: 0x06000341 RID: 833 RVA: 0x001CE498 File Offset: 0x001CD898
		public virtual SqlMoney GetSqlMoney(int ordinal)
		{
			this.EnsureSubclassOverride();
			return ValueUtilsSmi.GetSqlMoney(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal));
		}

		// Token: 0x06000342 RID: 834 RVA: 0x001CE4C4 File Offset: 0x001CD8C4
		public virtual SqlDateTime GetSqlDateTime(int ordinal)
		{
			this.EnsureSubclassOverride();
			return ValueUtilsSmi.GetSqlDateTime(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal));
		}

		// Token: 0x06000343 RID: 835 RVA: 0x001CE4F0 File Offset: 0x001CD8F0
		public virtual SqlDecimal GetSqlDecimal(int ordinal)
		{
			this.EnsureSubclassOverride();
			return ValueUtilsSmi.GetSqlDecimal(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal));
		}

		// Token: 0x06000344 RID: 836 RVA: 0x001CE51C File Offset: 0x001CD91C
		public virtual SqlString GetSqlString(int ordinal)
		{
			this.EnsureSubclassOverride();
			return ValueUtilsSmi.GetSqlString(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal));
		}

		// Token: 0x06000345 RID: 837 RVA: 0x001CE548 File Offset: 0x001CD948
		public virtual SqlGuid GetSqlGuid(int ordinal)
		{
			this.EnsureSubclassOverride();
			return ValueUtilsSmi.GetSqlGuid(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal));
		}

		// Token: 0x06000346 RID: 838 RVA: 0x001CE574 File Offset: 0x001CD974
		public virtual int SetValues(params object[] values)
		{
			this.EnsureSubclassOverride();
			if (values == null)
			{
				throw ADP.ArgumentNull("values");
			}
			int num = ((values.Length > this.FieldCount) ? this.FieldCount : values.Length);
			ExtendedClrTypeCode[] array = new ExtendedClrTypeCode[num];
			for (int i = 0; i < num; i++)
			{
				SqlMetaData sqlMetaData = this.GetSqlMetaData(i);
				array[i] = MetaDataUtilsSmi.DetermineExtendedTypeCodeForUseWithSqlDbType(sqlMetaData.SqlDbType, false, values[i], sqlMetaData.Type, this.SmiVersion);
				if (ExtendedClrTypeCode.Invalid == array[i])
				{
					throw ADP.InvalidCast();
				}
			}
			for (int j = 0; j < num; j++)
			{
				if (this.SmiVersion >= 210UL)
				{
					ValueUtilsSmi.SetCompatibleValueV200(this._eventSink, this._recordBuffer, j, this.GetSmiMetaData(j), values[j], array[j], 0, 0, null);
				}
				else
				{
					ValueUtilsSmi.SetCompatibleValue(this._eventSink, this._recordBuffer, j, this.GetSmiMetaData(j), values[j], array[j], 0);
				}
			}
			return num;
		}

		// Token: 0x06000347 RID: 839 RVA: 0x001CE654 File Offset: 0x001CDA54
		public virtual void SetValue(int ordinal, object value)
		{
			this.EnsureSubclassOverride();
			SqlMetaData sqlMetaData = this.GetSqlMetaData(ordinal);
			ExtendedClrTypeCode extendedClrTypeCode = MetaDataUtilsSmi.DetermineExtendedTypeCodeForUseWithSqlDbType(sqlMetaData.SqlDbType, false, value, sqlMetaData.Type, this.SmiVersion);
			if (ExtendedClrTypeCode.Invalid == extendedClrTypeCode)
			{
				throw ADP.InvalidCast();
			}
			if (this.SmiVersion >= 210UL)
			{
				ValueUtilsSmi.SetCompatibleValueV200(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), value, extendedClrTypeCode, 0, 0, null);
				return;
			}
			ValueUtilsSmi.SetCompatibleValue(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), value, extendedClrTypeCode, 0);
		}

		// Token: 0x06000348 RID: 840 RVA: 0x001CE6DC File Offset: 0x001CDADC
		public virtual void SetBoolean(int ordinal, bool value)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetBoolean(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), value);
		}

		// Token: 0x06000349 RID: 841 RVA: 0x001CE70C File Offset: 0x001CDB0C
		public virtual void SetByte(int ordinal, byte value)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetByte(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), value);
		}

		// Token: 0x0600034A RID: 842 RVA: 0x001CE73C File Offset: 0x001CDB3C
		public virtual void SetBytes(int ordinal, long fieldOffset, byte[] buffer, int bufferOffset, int length)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetBytes(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), fieldOffset, buffer, bufferOffset, length);
		}

		// Token: 0x0600034B RID: 843 RVA: 0x001CE770 File Offset: 0x001CDB70
		public virtual void SetChar(int ordinal, char value)
		{
			this.EnsureSubclassOverride();
			throw ADP.NotSupported();
		}

		// Token: 0x0600034C RID: 844 RVA: 0x001CE788 File Offset: 0x001CDB88
		public virtual void SetChars(int ordinal, long fieldOffset, char[] buffer, int bufferOffset, int length)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetChars(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), fieldOffset, buffer, bufferOffset, length);
		}

		// Token: 0x0600034D RID: 845 RVA: 0x001CE7BC File Offset: 0x001CDBBC
		public virtual void SetInt16(int ordinal, short value)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetInt16(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), value);
		}

		// Token: 0x0600034E RID: 846 RVA: 0x001CE7EC File Offset: 0x001CDBEC
		public virtual void SetInt32(int ordinal, int value)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetInt32(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), value);
		}

		// Token: 0x0600034F RID: 847 RVA: 0x001CE81C File Offset: 0x001CDC1C
		public virtual void SetInt64(int ordinal, long value)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetInt64(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), value);
		}

		// Token: 0x06000350 RID: 848 RVA: 0x001CE84C File Offset: 0x001CDC4C
		public virtual void SetFloat(int ordinal, float value)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetSingle(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), value);
		}

		// Token: 0x06000351 RID: 849 RVA: 0x001CE87C File Offset: 0x001CDC7C
		public virtual void SetDouble(int ordinal, double value)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetDouble(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), value);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x001CE8AC File Offset: 0x001CDCAC
		public virtual void SetString(int ordinal, string value)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetString(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), value);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x001CE8DC File Offset: 0x001CDCDC
		public virtual void SetDecimal(int ordinal, decimal value)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetDecimal(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), value);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x001CE90C File Offset: 0x001CDD0C
		public virtual void SetDateTime(int ordinal, DateTime value)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetDateTime(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), value);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x001CE93C File Offset: 0x001CDD3C
		public virtual void SetTimeSpan(int ordinal, TimeSpan value)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetTimeSpan(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), value, this.SmiVersion >= 210UL);
		}

		// Token: 0x06000356 RID: 854 RVA: 0x001CE97C File Offset: 0x001CDD7C
		public virtual void SetDateTimeOffset(int ordinal, DateTimeOffset value)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetDateTimeOffset(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), value, this.SmiVersion >= 210UL);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x001CE9BC File Offset: 0x001CDDBC
		public virtual void SetDBNull(int ordinal)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetDBNull(this._eventSink, this._recordBuffer, ordinal, true);
		}

		// Token: 0x06000358 RID: 856 RVA: 0x001CE9E4 File Offset: 0x001CDDE4
		public virtual void SetGuid(int ordinal, Guid value)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetGuid(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), value);
		}

		// Token: 0x06000359 RID: 857 RVA: 0x001CEA14 File Offset: 0x001CDE14
		public virtual void SetSqlBoolean(int ordinal, SqlBoolean value)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetSqlBoolean(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), value);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x001CEA44 File Offset: 0x001CDE44
		public virtual void SetSqlByte(int ordinal, SqlByte value)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetSqlByte(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), value);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x001CEA74 File Offset: 0x001CDE74
		public virtual void SetSqlInt16(int ordinal, SqlInt16 value)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetSqlInt16(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), value);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x001CEAA4 File Offset: 0x001CDEA4
		public virtual void SetSqlInt32(int ordinal, SqlInt32 value)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetSqlInt32(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), value);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x001CEAD4 File Offset: 0x001CDED4
		public virtual void SetSqlInt64(int ordinal, SqlInt64 value)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetSqlInt64(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), value);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x001CEB04 File Offset: 0x001CDF04
		public virtual void SetSqlSingle(int ordinal, SqlSingle value)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetSqlSingle(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), value);
		}

		// Token: 0x0600035F RID: 863 RVA: 0x001CEB34 File Offset: 0x001CDF34
		public virtual void SetSqlDouble(int ordinal, SqlDouble value)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetSqlDouble(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), value);
		}

		// Token: 0x06000360 RID: 864 RVA: 0x001CEB64 File Offset: 0x001CDF64
		public virtual void SetSqlMoney(int ordinal, SqlMoney value)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetSqlMoney(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), value);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x001CEB94 File Offset: 0x001CDF94
		public virtual void SetSqlDateTime(int ordinal, SqlDateTime value)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetSqlDateTime(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), value);
		}

		// Token: 0x06000362 RID: 866 RVA: 0x001CEBC4 File Offset: 0x001CDFC4
		public virtual void SetSqlXml(int ordinal, SqlXml value)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetSqlXml(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), value);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x001CEBF4 File Offset: 0x001CDFF4
		public virtual void SetSqlDecimal(int ordinal, SqlDecimal value)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetSqlDecimal(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), value);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x001CEC24 File Offset: 0x001CE024
		public virtual void SetSqlString(int ordinal, SqlString value)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetSqlString(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), value);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x001CEC54 File Offset: 0x001CE054
		public virtual void SetSqlBinary(int ordinal, SqlBinary value)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetSqlBinary(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), value);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x001CEC84 File Offset: 0x001CE084
		public virtual void SetSqlGuid(int ordinal, SqlGuid value)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetSqlGuid(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), value);
		}

		// Token: 0x06000367 RID: 871 RVA: 0x001CECB4 File Offset: 0x001CE0B4
		public virtual void SetSqlChars(int ordinal, SqlChars value)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetSqlChars(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), value);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x001CECE4 File Offset: 0x001CE0E4
		public virtual void SetSqlBytes(int ordinal, SqlBytes value)
		{
			this.EnsureSubclassOverride();
			ValueUtilsSmi.SetSqlBytes(this._eventSink, this._recordBuffer, ordinal, this.GetSmiMetaData(ordinal), value);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x001CED14 File Offset: 0x001CE114
		public SqlDataRecord(params SqlMetaData[] metaData)
		{
			if (metaData == null)
			{
				throw ADP.ArgumentNull("metadata");
			}
			this._columnMetaData = new SqlMetaData[metaData.Length];
			this._columnSmiMetaData = new SmiExtendedMetaData[metaData.Length];
			ulong smiVersion = this.SmiVersion;
			for (int i = 0; i < this._columnSmiMetaData.Length; i++)
			{
				this._columnMetaData[i] = metaData[i];
				this._columnSmiMetaData[i] = MetaDataUtilsSmi.SqlMetaDataToSmiExtendedMetaData(this._columnMetaData[i]);
				if (!MetaDataUtilsSmi.IsValidForSmiVersion(this._columnSmiMetaData[i], smiVersion))
				{
					throw ADP.VersionDoesNotSupportDataType(this._columnSmiMetaData[i].TypeName);
				}
			}
			this._eventSink = new SmiEventSink_Default();
			if (InOutOfProcHelper.InProc)
			{
				this._recordContext = SmiContextFactory.Instance.GetCurrentContext();
				this._recordBuffer = this._recordContext.CreateRecordBuffer(this._columnSmiMetaData, this._eventSink);
				this._usesStringStorageForXml = false;
			}
			else
			{
				this._recordContext = null;
				this._recordBuffer = new MemoryRecordBuffer(this._columnSmiMetaData);
				this._usesStringStorageForXml = true;
			}
			this._eventSink.ProcessMessagesAndThrow();
		}

		// Token: 0x0600036A RID: 874 RVA: 0x001CEE20 File Offset: 0x001CE220
		internal SqlDataRecord(SmiRecordBuffer recordBuffer, params SmiExtendedMetaData[] metaData)
		{
			this._columnMetaData = new SqlMetaData[metaData.Length];
			this._columnSmiMetaData = new SmiExtendedMetaData[metaData.Length];
			for (int i = 0; i < this._columnSmiMetaData.Length; i++)
			{
				this._columnSmiMetaData[i] = metaData[i];
				this._columnMetaData[i] = MetaDataUtilsSmi.SmiExtendedMetaDataToSqlMetaData(this._columnSmiMetaData[i]);
			}
			this._eventSink = new SmiEventSink_Default();
			if (InOutOfProcHelper.InProc)
			{
				this._recordContext = SmiContextFactory.Instance.GetCurrentContext();
			}
			else
			{
				this._recordContext = null;
			}
			this._recordBuffer = recordBuffer;
			this._eventSink.ProcessMessagesAndThrow();
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600036B RID: 875 RVA: 0x001CEEC0 File Offset: 0x001CE2C0
		internal SmiRecordBuffer RecordBuffer
		{
			get
			{
				return this._recordBuffer;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600036C RID: 876 RVA: 0x001CEED4 File Offset: 0x001CE2D4
		internal SmiContext RecordContext
		{
			get
			{
				return this._recordContext;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600036D RID: 877 RVA: 0x001CEEE8 File Offset: 0x001CE2E8
		private ulong SmiVersion
		{
			get
			{
				if (!InOutOfProcHelper.InProc)
				{
					return 210UL;
				}
				return SmiContextFactory.Instance.NegotiatedSmiVersion;
			}
		}

		// Token: 0x0600036E RID: 878 RVA: 0x001CEF10 File Offset: 0x001CE310
		internal SqlMetaData[] InternalGetMetaData()
		{
			return this._columnMetaData;
		}

		// Token: 0x0600036F RID: 879 RVA: 0x001CEF24 File Offset: 0x001CE324
		internal SmiExtendedMetaData[] InternalGetSmiMetaData()
		{
			return this._columnSmiMetaData;
		}

		// Token: 0x06000370 RID: 880 RVA: 0x001CEF38 File Offset: 0x001CE338
		internal SmiExtendedMetaData GetSmiMetaData(int ordinal)
		{
			return this._columnSmiMetaData[ordinal];
		}

		// Token: 0x06000371 RID: 881 RVA: 0x001CEF50 File Offset: 0x001CE350
		internal void ThrowIfInvalidOrdinal(int ordinal)
		{
			if (0 > ordinal || this.FieldCount <= ordinal)
			{
				throw ADP.IndexOutOfRange(ordinal);
			}
		}

		// Token: 0x06000372 RID: 882 RVA: 0x001CEF74 File Offset: 0x001CE374
		private void EnsureSubclassOverride()
		{
			if (this._recordBuffer == null)
			{
				throw SQL.SubclassMustOverride();
			}
		}

		// Token: 0x0400060D RID: 1549
		private SmiRecordBuffer _recordBuffer;

		// Token: 0x0400060E RID: 1550
		private SmiContext _recordContext;

		// Token: 0x0400060F RID: 1551
		private SmiExtendedMetaData[] _columnSmiMetaData;

		// Token: 0x04000610 RID: 1552
		private SmiEventSink_Default _eventSink;

		// Token: 0x04000611 RID: 1553
		private SqlMetaData[] _columnMetaData;

		// Token: 0x04000612 RID: 1554
		private FieldNameLookup _fieldNameLookup;

		// Token: 0x04000613 RID: 1555
		private bool _usesStringStorageForXml;

		// Token: 0x04000614 RID: 1556
		private static readonly SmiMetaData __maxNVarCharForXml = new SmiMetaData(SqlDbType.NVarChar, -1L, SmiMetaData.DefaultNVarChar_NoCollation.Precision, SmiMetaData.DefaultNVarChar_NoCollation.Scale, SmiMetaData.DefaultNVarChar.LocaleId, SmiMetaData.DefaultNVarChar.CompareOptions, null);
	}
}
