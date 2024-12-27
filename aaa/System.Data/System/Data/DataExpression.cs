using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlTypes;

namespace System.Data
{
	// Token: 0x020001A7 RID: 423
	internal sealed class DataExpression : IFilter
	{
		// Token: 0x06001899 RID: 6297 RVA: 0x0023A104 File Offset: 0x00239504
		internal DataExpression(DataTable table, string expression)
			: this(table, expression, null)
		{
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x0023A11C File Offset: 0x0023951C
		internal DataExpression(DataTable table, string expression, Type type)
		{
			ExpressionParser expressionParser = new ExpressionParser(table);
			expressionParser.LoadExpression(expression);
			this.originalExpression = expression;
			this.expr = null;
			if (expression != null)
			{
				this._storageType = DataStorage.GetStorageType(type);
				this._dataType = type;
				this.expr = expressionParser.Parse();
				this.parsed = true;
				if (this.expr != null && table != null)
				{
					this.Bind(table);
					return;
				}
				this.bound = false;
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x0600189B RID: 6299 RVA: 0x0023A19C File Offset: 0x0023959C
		internal string Expression
		{
			get
			{
				if (this.originalExpression == null)
				{
					return "";
				}
				return this.originalExpression;
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x0600189C RID: 6300 RVA: 0x0023A1C0 File Offset: 0x002395C0
		internal ExpressionNode ExpressionNode
		{
			get
			{
				return this.expr;
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x0600189D RID: 6301 RVA: 0x0023A1D4 File Offset: 0x002395D4
		internal bool HasValue
		{
			get
			{
				return null != this.expr;
			}
		}

		// Token: 0x0600189E RID: 6302 RVA: 0x0023A1F0 File Offset: 0x002395F0
		internal void Bind(DataTable table)
		{
			this.table = table;
			if (table == null)
			{
				return;
			}
			if (this.expr != null)
			{
				List<DataColumn> list = new List<DataColumn>();
				this.expr.Bind(table, list);
				this.expr = this.expr.Optimize();
				this.table = table;
				this.bound = true;
				this.dependency = list.ToArray();
			}
		}

		// Token: 0x0600189F RID: 6303 RVA: 0x0023A250 File Offset: 0x00239650
		internal bool DependsOn(DataColumn column)
		{
			return this.expr != null && this.expr.DependsOn(column);
		}

		// Token: 0x060018A0 RID: 6304 RVA: 0x0023A274 File Offset: 0x00239674
		internal object Evaluate()
		{
			return this.Evaluate(null, DataRowVersion.Default);
		}

		// Token: 0x060018A1 RID: 6305 RVA: 0x0023A290 File Offset: 0x00239690
		internal object Evaluate(DataRow row, DataRowVersion version)
		{
			if (!this.bound)
			{
				this.Bind(this.table);
			}
			object obj;
			if (this.expr != null)
			{
				obj = this.expr.Eval(row, version);
				if (obj == DBNull.Value && StorageType.Uri >= this._storageType)
				{
					return obj;
				}
				try
				{
					if (StorageType.Object != this._storageType)
					{
						obj = SqlConvert.ChangeType2(obj, this._storageType, this._dataType, this.table.FormatProvider);
					}
					return obj;
				}
				catch (Exception ex)
				{
					if (!ADP.IsCatchableExceptionType(ex))
					{
						throw;
					}
					ExceptionBuilder.TraceExceptionForCapture(ex);
					throw ExprException.DatavalueConvertion(obj, this._dataType, ex);
				}
			}
			obj = null;
			return obj;
		}

		// Token: 0x060018A2 RID: 6306 RVA: 0x0023A344 File Offset: 0x00239744
		internal object Evaluate(DataRow[] rows)
		{
			return this.Evaluate(rows, DataRowVersion.Default);
		}

		// Token: 0x060018A3 RID: 6307 RVA: 0x0023A360 File Offset: 0x00239760
		internal object Evaluate(DataRow[] rows, DataRowVersion version)
		{
			if (!this.bound)
			{
				this.Bind(this.table);
			}
			if (this.expr != null)
			{
				List<int> list = new List<int>();
				foreach (DataRow dataRow in rows)
				{
					if (dataRow.RowState != DataRowState.Deleted && (version != DataRowVersion.Original || dataRow.oldRecord != -1))
					{
						list.Add(dataRow.GetRecordFromVersion(version));
					}
				}
				int[] array = list.ToArray();
				return this.expr.Eval(array);
			}
			return DBNull.Value;
		}

		// Token: 0x060018A4 RID: 6308 RVA: 0x0023A3E8 File Offset: 0x002397E8
		public bool Invoke(DataRow row, DataRowVersion version)
		{
			if (this.expr == null)
			{
				return true;
			}
			if (row == null)
			{
				throw ExprException.InvokeArgument();
			}
			object obj = this.expr.Eval(row, version);
			bool flag;
			try
			{
				flag = DataExpression.ToBoolean(obj);
			}
			catch (EvaluateException)
			{
				throw ExprException.FilterConvertion(this.Expression);
			}
			return flag;
		}

		// Token: 0x060018A5 RID: 6309 RVA: 0x0023A44C File Offset: 0x0023984C
		internal DataColumn[] GetDependency()
		{
			return this.dependency;
		}

		// Token: 0x060018A6 RID: 6310 RVA: 0x0023A460 File Offset: 0x00239860
		internal bool IsTableAggregate()
		{
			return this.expr != null && this.expr.IsTableConstant();
		}

		// Token: 0x060018A7 RID: 6311 RVA: 0x0023A484 File Offset: 0x00239884
		internal static bool IsUnknown(object value)
		{
			return DataStorage.IsObjectNull(value);
		}

		// Token: 0x060018A8 RID: 6312 RVA: 0x0023A498 File Offset: 0x00239898
		internal bool HasLocalAggregate()
		{
			return this.expr != null && this.expr.HasLocalAggregate();
		}

		// Token: 0x060018A9 RID: 6313 RVA: 0x0023A4BC File Offset: 0x002398BC
		internal bool HasRemoteAggregate()
		{
			return this.expr != null && this.expr.HasRemoteAggregate();
		}

		// Token: 0x060018AA RID: 6314 RVA: 0x0023A4E0 File Offset: 0x002398E0
		internal static bool ToBoolean(object value)
		{
			if (DataExpression.IsUnknown(value))
			{
				return false;
			}
			if (value is bool)
			{
				return (bool)value;
			}
			if (value is SqlBoolean)
			{
				return ((SqlBoolean)value).IsTrue;
			}
			if (value is string)
			{
				try
				{
					return bool.Parse((string)value);
				}
				catch (Exception ex)
				{
					if (!ADP.IsCatchableExceptionType(ex))
					{
						throw;
					}
					ExceptionBuilder.TraceExceptionForCapture(ex);
					throw ExprException.DatavalueConvertion(value, typeof(bool), ex);
				}
			}
			throw ExprException.DatavalueConvertion(value, typeof(bool), null);
		}

		// Token: 0x04000D53 RID: 3411
		internal string originalExpression;

		// Token: 0x04000D54 RID: 3412
		private bool parsed;

		// Token: 0x04000D55 RID: 3413
		private bool bound;

		// Token: 0x04000D56 RID: 3414
		private ExpressionNode expr;

		// Token: 0x04000D57 RID: 3415
		private DataTable table;

		// Token: 0x04000D58 RID: 3416
		private readonly StorageType _storageType;

		// Token: 0x04000D59 RID: 3417
		private readonly Type _dataType;

		// Token: 0x04000D5A RID: 3418
		private DataColumn[] dependency = DataTable.zeroColumns;
	}
}
