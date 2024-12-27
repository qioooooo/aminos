using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlTypes;

namespace System.Data
{
	// Token: 0x020001B8 RID: 440
	internal sealed class UnaryNode : ExpressionNode
	{
		// Token: 0x0600193C RID: 6460 RVA: 0x0023E304 File Offset: 0x0023D704
		internal UnaryNode(DataTable table, int op, ExpressionNode right)
			: base(table)
		{
			this.op = op;
			this.right = right;
		}

		// Token: 0x0600193D RID: 6461 RVA: 0x0023E328 File Offset: 0x0023D728
		internal override void Bind(DataTable table, List<DataColumn> list)
		{
			base.BindTable(table);
			this.right.Bind(table, list);
		}

		// Token: 0x0600193E RID: 6462 RVA: 0x0023E34C File Offset: 0x0023D74C
		internal override object Eval()
		{
			return this.Eval(null, DataRowVersion.Default);
		}

		// Token: 0x0600193F RID: 6463 RVA: 0x0023E368 File Offset: 0x0023D768
		internal override object Eval(DataRow row, DataRowVersion version)
		{
			return this.EvalUnaryOp(this.op, this.right.Eval(row, version));
		}

		// Token: 0x06001940 RID: 6464 RVA: 0x0023E390 File Offset: 0x0023D790
		internal override object Eval(int[] recordNos)
		{
			return this.right.Eval(recordNos);
		}

		// Token: 0x06001941 RID: 6465 RVA: 0x0023E3AC File Offset: 0x0023D7AC
		private object EvalUnaryOp(int op, object vl)
		{
			object value = DBNull.Value;
			if (DataExpression.IsUnknown(vl))
			{
				return DBNull.Value;
			}
			switch (op)
			{
			case 0:
				return vl;
			case 1:
			{
				StorageType storageType = DataStorage.GetStorageType(vl.GetType());
				if (ExpressionNode.IsNumericSql(storageType))
				{
					StorageType storageType2 = storageType;
					switch (storageType2)
					{
					case StorageType.Byte:
						return (int)(-(int)((byte)vl));
					case StorageType.Int16:
						return (int)(-(int)((short)vl));
					case StorageType.UInt16:
					case StorageType.UInt32:
					case StorageType.UInt64:
						break;
					case StorageType.Int32:
						return -(int)vl;
					case StorageType.Int64:
						return -(long)vl;
					case StorageType.Single:
						return -(float)vl;
					case StorageType.Double:
						return -(double)vl;
					case StorageType.Decimal:
						return -(decimal)vl;
					default:
						switch (storageType2)
						{
						case StorageType.SqlDecimal:
							return -(SqlDecimal)vl;
						case StorageType.SqlDouble:
							return -(SqlDouble)vl;
						case StorageType.SqlInt16:
							return -(SqlInt16)vl;
						case StorageType.SqlInt32:
							return -(SqlInt32)vl;
						case StorageType.SqlInt64:
							return -(SqlInt64)vl;
						case StorageType.SqlMoney:
							return -(SqlMoney)vl;
						case StorageType.SqlSingle:
							return -(SqlSingle)vl;
						}
						break;
					}
					return DBNull.Value;
				}
				throw ExprException.TypeMismatch(this.ToString());
			}
			case 2:
			{
				StorageType storageType = DataStorage.GetStorageType(vl.GetType());
				if (ExpressionNode.IsNumericSql(storageType))
				{
					return vl;
				}
				throw ExprException.TypeMismatch(this.ToString());
			}
			case 3:
				if (vl is SqlBoolean)
				{
					if (((SqlBoolean)vl).IsFalse)
					{
						return SqlBoolean.True;
					}
					if (((SqlBoolean)vl).IsTrue)
					{
						return SqlBoolean.False;
					}
					throw ExprException.UnsupportedOperator(op);
				}
				else
				{
					if (DataExpression.ToBoolean(vl))
					{
						return false;
					}
					return true;
				}
				break;
			default:
				throw ExprException.UnsupportedOperator(op);
			}
		}

		// Token: 0x06001942 RID: 6466 RVA: 0x0023E608 File Offset: 0x0023DA08
		internal override bool IsConstant()
		{
			return this.right.IsConstant();
		}

		// Token: 0x06001943 RID: 6467 RVA: 0x0023E620 File Offset: 0x0023DA20
		internal override bool IsTableConstant()
		{
			return this.right.IsTableConstant();
		}

		// Token: 0x06001944 RID: 6468 RVA: 0x0023E638 File Offset: 0x0023DA38
		internal override bool HasLocalAggregate()
		{
			return this.right.HasLocalAggregate();
		}

		// Token: 0x06001945 RID: 6469 RVA: 0x0023E650 File Offset: 0x0023DA50
		internal override bool HasRemoteAggregate()
		{
			return this.right.HasRemoteAggregate();
		}

		// Token: 0x06001946 RID: 6470 RVA: 0x0023E668 File Offset: 0x0023DA68
		internal override bool DependsOn(DataColumn column)
		{
			return this.right.DependsOn(column);
		}

		// Token: 0x06001947 RID: 6471 RVA: 0x0023E684 File Offset: 0x0023DA84
		internal override ExpressionNode Optimize()
		{
			this.right = this.right.Optimize();
			if (this.IsConstant())
			{
				object obj = this.Eval();
				return new ConstNode(base.table, ValueType.Object, obj, false);
			}
			return this;
		}

		// Token: 0x04000E23 RID: 3619
		internal readonly int op;

		// Token: 0x04000E24 RID: 3620
		internal ExpressionNode right;
	}
}
