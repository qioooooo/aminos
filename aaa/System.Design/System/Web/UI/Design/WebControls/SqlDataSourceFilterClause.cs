using System;
using System.ComponentModel.Design.Data;
using System.Globalization;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020004E2 RID: 1250
	internal sealed class SqlDataSourceFilterClause
	{
		// Token: 0x06002CE1 RID: 11489 RVA: 0x000FE0DC File Offset: 0x000FD0DC
		public SqlDataSourceFilterClause(DesignerDataConnection designerDataConnection, DesignerDataTableBase designerDataTable, DesignerDataColumn designerDataColumn, string operatorFormat, bool isBinary, string value, Parameter parameter)
		{
			this._designerDataConnection = designerDataConnection;
			this._designerDataTable = designerDataTable;
			this._designerDataColumn = designerDataColumn;
			this._isBinary = isBinary;
			this._operatorFormat = operatorFormat;
			this._value = value;
			this._parameter = parameter;
		}

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x06002CE2 RID: 11490 RVA: 0x000FE119 File Offset: 0x000FD119
		public DesignerDataColumn DesignerDataColumn
		{
			get
			{
				return this._designerDataColumn;
			}
		}

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06002CE3 RID: 11491 RVA: 0x000FE121 File Offset: 0x000FD121
		public bool IsBinary
		{
			get
			{
				return this._isBinary;
			}
		}

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x06002CE4 RID: 11492 RVA: 0x000FE129 File Offset: 0x000FD129
		public string OperatorFormat
		{
			get
			{
				return this._operatorFormat;
			}
		}

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x06002CE5 RID: 11493 RVA: 0x000FE131 File Offset: 0x000FD131
		public Parameter Parameter
		{
			get
			{
				return this._parameter;
			}
		}

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x06002CE6 RID: 11494 RVA: 0x000FE139 File Offset: 0x000FD139
		public string Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x06002CE7 RID: 11495 RVA: 0x000FE144 File Offset: 0x000FD144
		public override string ToString()
		{
			SqlDataSourceColumnData sqlDataSourceColumnData = new SqlDataSourceColumnData(this._designerDataConnection, this._designerDataColumn);
			if (this._isBinary)
			{
				return string.Format(CultureInfo.InvariantCulture, this._operatorFormat, new object[] { sqlDataSourceColumnData.EscapedName, this._value });
			}
			return string.Format(CultureInfo.InvariantCulture, this._operatorFormat, new object[] { sqlDataSourceColumnData.EscapedName });
		}

		// Token: 0x04001EA1 RID: 7841
		private DesignerDataColumn _designerDataColumn;

		// Token: 0x04001EA2 RID: 7842
		private DesignerDataTableBase _designerDataTable;

		// Token: 0x04001EA3 RID: 7843
		private DesignerDataConnection _designerDataConnection;

		// Token: 0x04001EA4 RID: 7844
		private bool _isBinary;

		// Token: 0x04001EA5 RID: 7845
		private string _operatorFormat;

		// Token: 0x04001EA6 RID: 7846
		private string _value;

		// Token: 0x04001EA7 RID: 7847
		private Parameter _parameter;
	}
}
