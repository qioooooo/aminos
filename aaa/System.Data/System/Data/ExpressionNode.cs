using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;

namespace System.Data
{
	// Token: 0x020001A1 RID: 417
	internal abstract class ExpressionNode
	{
		// Token: 0x0600184B RID: 6219 RVA: 0x00236C38 File Offset: 0x00236038
		protected ExpressionNode(DataTable table)
		{
			this._table = table;
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x0600184C RID: 6220 RVA: 0x00236C54 File Offset: 0x00236054
		internal IFormatProvider FormatProvider
		{
			get
			{
				if (this._table == null)
				{
					return CultureInfo.CurrentCulture;
				}
				return this._table.FormatProvider;
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x0600184D RID: 6221 RVA: 0x00236C7C File Offset: 0x0023607C
		internal virtual bool IsSqlColumn
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x0600184E RID: 6222 RVA: 0x00236C8C File Offset: 0x0023608C
		protected DataTable table
		{
			get
			{
				return this._table;
			}
		}

		// Token: 0x0600184F RID: 6223 RVA: 0x00236CA0 File Offset: 0x002360A0
		protected void BindTable(DataTable table)
		{
			this._table = table;
		}

		// Token: 0x06001850 RID: 6224
		internal abstract void Bind(DataTable table, List<DataColumn> list);

		// Token: 0x06001851 RID: 6225
		internal abstract object Eval();

		// Token: 0x06001852 RID: 6226
		internal abstract object Eval(DataRow row, DataRowVersion version);

		// Token: 0x06001853 RID: 6227
		internal abstract object Eval(int[] recordNos);

		// Token: 0x06001854 RID: 6228
		internal abstract bool IsConstant();

		// Token: 0x06001855 RID: 6229
		internal abstract bool IsTableConstant();

		// Token: 0x06001856 RID: 6230
		internal abstract bool HasLocalAggregate();

		// Token: 0x06001857 RID: 6231
		internal abstract bool HasRemoteAggregate();

		// Token: 0x06001858 RID: 6232
		internal abstract ExpressionNode Optimize();

		// Token: 0x06001859 RID: 6233 RVA: 0x00236CB4 File Offset: 0x002360B4
		internal virtual bool DependsOn(DataColumn column)
		{
			return false;
		}

		// Token: 0x0600185A RID: 6234 RVA: 0x00236CC4 File Offset: 0x002360C4
		internal static bool IsInteger(StorageType type)
		{
			return type == StorageType.Int16 || type == StorageType.Int32 || type == StorageType.Int64 || type == StorageType.UInt16 || type == StorageType.UInt32 || type == StorageType.UInt64 || type == StorageType.SByte || type == StorageType.Byte;
		}

		// Token: 0x0600185B RID: 6235 RVA: 0x00236CF8 File Offset: 0x002360F8
		internal static bool IsIntegerSql(StorageType type)
		{
			return type == StorageType.Int16 || type == StorageType.Int32 || type == StorageType.Int64 || type == StorageType.UInt16 || type == StorageType.UInt32 || type == StorageType.UInt64 || type == StorageType.SByte || type == StorageType.Byte || type == StorageType.SqlInt64 || type == StorageType.SqlInt32 || type == StorageType.SqlInt16 || type == StorageType.SqlByte;
		}

		// Token: 0x0600185C RID: 6236 RVA: 0x00236D40 File Offset: 0x00236140
		internal static bool IsSigned(StorageType type)
		{
			return type == StorageType.Int16 || type == StorageType.Int32 || type == StorageType.Int64 || type == StorageType.SByte || ExpressionNode.IsFloat(type);
		}

		// Token: 0x0600185D RID: 6237 RVA: 0x00236D68 File Offset: 0x00236168
		internal static bool IsSignedSql(StorageType type)
		{
			return type == StorageType.Int16 || type == StorageType.Int32 || type == StorageType.Int64 || type == StorageType.SByte || type == StorageType.SqlInt64 || type == StorageType.SqlInt32 || type == StorageType.SqlInt16 || ExpressionNode.IsFloatSql(type);
		}

		// Token: 0x0600185E RID: 6238 RVA: 0x00236DA0 File Offset: 0x002361A0
		internal static bool IsUnsigned(StorageType type)
		{
			return type == StorageType.UInt16 || type == StorageType.UInt32 || type == StorageType.UInt64 || type == StorageType.Byte;
		}

		// Token: 0x0600185F RID: 6239 RVA: 0x00236DC4 File Offset: 0x002361C4
		internal static bool IsUnsignedSql(StorageType type)
		{
			return type == StorageType.UInt16 || type == StorageType.UInt32 || type == StorageType.UInt64 || type == StorageType.SqlByte || type == StorageType.Byte;
		}

		// Token: 0x06001860 RID: 6240 RVA: 0x00236DEC File Offset: 0x002361EC
		internal static bool IsNumeric(StorageType type)
		{
			return ExpressionNode.IsFloat(type) || ExpressionNode.IsInteger(type);
		}

		// Token: 0x06001861 RID: 6241 RVA: 0x00236E0C File Offset: 0x0023620C
		internal static bool IsNumericSql(StorageType type)
		{
			return ExpressionNode.IsFloatSql(type) || ExpressionNode.IsIntegerSql(type);
		}

		// Token: 0x06001862 RID: 6242 RVA: 0x00236E2C File Offset: 0x0023622C
		internal static bool IsFloat(StorageType type)
		{
			return type == StorageType.Single || type == StorageType.Double || type == StorageType.Decimal;
		}

		// Token: 0x06001863 RID: 6243 RVA: 0x00236E4C File Offset: 0x0023624C
		internal static bool IsFloatSql(StorageType type)
		{
			return type == StorageType.Single || type == StorageType.Double || type == StorageType.Decimal || type == StorageType.SqlDouble || type == StorageType.SqlDecimal || type == StorageType.SqlMoney || type == StorageType.SqlSingle;
		}

		// Token: 0x04000D1C RID: 3356
		private DataTable _table;
	}
}
