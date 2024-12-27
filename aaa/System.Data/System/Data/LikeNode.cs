using System;
using System.Data.Common;
using System.Data.SqlTypes;

namespace System.Data
{
	// Token: 0x020001A5 RID: 421
	internal sealed class LikeNode : BinaryNode
	{
		// Token: 0x06001889 RID: 6281 RVA: 0x00239810 File Offset: 0x00238C10
		internal LikeNode(DataTable table, int op, ExpressionNode left, ExpressionNode right)
			: base(table, op, left, right)
		{
		}

		// Token: 0x0600188A RID: 6282 RVA: 0x00239828 File Offset: 0x00238C28
		internal override object Eval(DataRow row, DataRowVersion version)
		{
			object obj = this.left.Eval(row, version);
			if (obj == DBNull.Value || (this.left.IsSqlColumn && DataStorage.IsObjectSqlNull(obj)))
			{
				return DBNull.Value;
			}
			string text2;
			if (this.pattern == null)
			{
				object obj2 = this.right.Eval(row, version);
				if (!(obj2 is string) && !(obj2 is SqlString))
				{
					base.SetTypeMismatchError(this.op, obj.GetType(), obj2.GetType());
				}
				if (obj2 == DBNull.Value || DataStorage.IsObjectSqlNull(obj2))
				{
					return DBNull.Value;
				}
				string text = (string)SqlConvert.ChangeType2(obj2, StorageType.String, typeof(string), base.FormatProvider);
				text2 = this.AnalyzePattern(text);
				if (this.right.IsConstant())
				{
					this.pattern = text2;
				}
			}
			else
			{
				text2 = this.pattern;
			}
			if (!(obj is string) && !(obj is SqlString))
			{
				base.SetTypeMismatchError(this.op, obj.GetType(), typeof(string));
			}
			char[] array = new char[] { ' ', '\u3000' };
			string text3;
			if (obj is SqlString)
			{
				text3 = ((SqlString)obj).Value;
			}
			else
			{
				text3 = (string)obj;
			}
			string text4 = text3.TrimEnd(array);
			switch (this.kind)
			{
			case 1:
				return 0 == base.table.IndexOf(text4, text2);
			case 2:
			{
				string text5 = text2.TrimEnd(array);
				return base.table.IsSuffix(text4, text5);
			}
			case 3:
				return 0 <= base.table.IndexOf(text4, text2);
			case 4:
				return 0 == base.table.Compare(text4, text2);
			case 5:
				return true;
			default:
				return DBNull.Value;
			}
		}

		// Token: 0x0600188B RID: 6283 RVA: 0x00239A0C File Offset: 0x00238E0C
		internal string AnalyzePattern(string pat)
		{
			int length = pat.Length;
			char[] array = new char[length + 1];
			pat.CopyTo(0, array, 0, length);
			array[length] = '\0';
			char[] array2 = new char[length + 1];
			int num = 0;
			int num2 = 0;
			int i = 0;
			while (i < length)
			{
				if (array[i] != '*')
				{
					if (array[i] != '%')
					{
						if (array[i] != '[')
						{
							array2[num++] = array[i];
							i++;
							continue;
						}
						i++;
						if (i >= length)
						{
							throw ExprException.InvalidPattern(pat);
						}
						array2[num++] = array[i++];
						if (i >= length)
						{
							throw ExprException.InvalidPattern(pat);
						}
						if (array[i] != ']')
						{
							throw ExprException.InvalidPattern(pat);
						}
						i++;
						continue;
					}
				}
				while ((array[i] == '*' || array[i] == '%') && i < length)
				{
					i++;
				}
				if ((i < length && num > 0) || num2 >= 2)
				{
					throw ExprException.InvalidPattern(pat);
				}
				num2++;
			}
			string text = new string(array2, 0, num);
			if (num2 == 0)
			{
				this.kind = 4;
			}
			else if (num > 0)
			{
				if (array[0] == '*' || array[0] == '%')
				{
					if (array[length - 1] == '*' || array[length - 1] == '%')
					{
						this.kind = 3;
					}
					else
					{
						this.kind = 2;
					}
				}
				else
				{
					this.kind = 1;
				}
			}
			else
			{
				this.kind = 5;
			}
			return text;
		}

		// Token: 0x04000D4B RID: 3403
		internal const int match_left = 1;

		// Token: 0x04000D4C RID: 3404
		internal const int match_right = 2;

		// Token: 0x04000D4D RID: 3405
		internal const int match_middle = 3;

		// Token: 0x04000D4E RID: 3406
		internal const int match_exact = 4;

		// Token: 0x04000D4F RID: 3407
		internal const int match_all = 5;

		// Token: 0x04000D50 RID: 3408
		private int kind;

		// Token: 0x04000D51 RID: 3409
		private string pattern;
	}
}
