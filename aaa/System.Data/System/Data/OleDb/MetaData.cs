using System;

namespace System.Data.OleDb
{
	// Token: 0x02000223 RID: 547
	internal sealed class MetaData : IComparable
	{
		// Token: 0x06001F86 RID: 8070 RVA: 0x0025DFF0 File Offset: 0x0025D3F0
		int IComparable.CompareTo(object obj)
		{
			if (this.isHidden == (obj as MetaData).isHidden)
			{
				return (int)this.ordinal - (int)(obj as MetaData).ordinal;
			}
			if (!this.isHidden)
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x06001F87 RID: 8071 RVA: 0x0025E038 File Offset: 0x0025D438
		internal MetaData()
		{
		}

		// Token: 0x040012CC RID: 4812
		internal Bindings bindings;

		// Token: 0x040012CD RID: 4813
		internal ColumnBinding columnBinding;

		// Token: 0x040012CE RID: 4814
		internal string columnName;

		// Token: 0x040012CF RID: 4815
		internal Guid guid;

		// Token: 0x040012D0 RID: 4816
		internal int kind;

		// Token: 0x040012D1 RID: 4817
		internal IntPtr propid;

		// Token: 0x040012D2 RID: 4818
		internal string idname;

		// Token: 0x040012D3 RID: 4819
		internal NativeDBType type;

		// Token: 0x040012D4 RID: 4820
		internal IntPtr ordinal;

		// Token: 0x040012D5 RID: 4821
		internal int size;

		// Token: 0x040012D6 RID: 4822
		internal int flags;

		// Token: 0x040012D7 RID: 4823
		internal byte precision;

		// Token: 0x040012D8 RID: 4824
		internal byte scale;

		// Token: 0x040012D9 RID: 4825
		internal bool isAutoIncrement;

		// Token: 0x040012DA RID: 4826
		internal bool isUnique;

		// Token: 0x040012DB RID: 4827
		internal bool isKeyColumn;

		// Token: 0x040012DC RID: 4828
		internal bool isHidden;

		// Token: 0x040012DD RID: 4829
		internal string baseSchemaName;

		// Token: 0x040012DE RID: 4830
		internal string baseCatalogName;

		// Token: 0x040012DF RID: 4831
		internal string baseTableName;

		// Token: 0x040012E0 RID: 4832
		internal string baseColumnName;
	}
}
