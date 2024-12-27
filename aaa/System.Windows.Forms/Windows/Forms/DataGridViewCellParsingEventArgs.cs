using System;

namespace System.Windows.Forms
{
	// Token: 0x0200031D RID: 797
	public class DataGridViewCellParsingEventArgs : ConvertEventArgs
	{
		// Token: 0x06003373 RID: 13171 RVA: 0x000B45E5 File Offset: 0x000B35E5
		public DataGridViewCellParsingEventArgs(int rowIndex, int columnIndex, object value, Type desiredType, DataGridViewCellStyle inheritedCellStyle)
			: base(value, desiredType)
		{
			this.rowIndex = rowIndex;
			this.columnIndex = columnIndex;
			this.inheritedCellStyle = inheritedCellStyle;
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x06003374 RID: 13172 RVA: 0x000B4606 File Offset: 0x000B3606
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x06003375 RID: 13173 RVA: 0x000B460E File Offset: 0x000B360E
		public int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x06003376 RID: 13174 RVA: 0x000B4616 File Offset: 0x000B3616
		// (set) Token: 0x06003377 RID: 13175 RVA: 0x000B461E File Offset: 0x000B361E
		public DataGridViewCellStyle InheritedCellStyle
		{
			get
			{
				return this.inheritedCellStyle;
			}
			set
			{
				this.inheritedCellStyle = value;
			}
		}

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x06003378 RID: 13176 RVA: 0x000B4627 File Offset: 0x000B3627
		// (set) Token: 0x06003379 RID: 13177 RVA: 0x000B462F File Offset: 0x000B362F
		public bool ParsingApplied
		{
			get
			{
				return this.parsingApplied;
			}
			set
			{
				this.parsingApplied = value;
			}
		}

		// Token: 0x04001ACC RID: 6860
		private int rowIndex;

		// Token: 0x04001ACD RID: 6861
		private int columnIndex;

		// Token: 0x04001ACE RID: 6862
		private DataGridViewCellStyle inheritedCellStyle;

		// Token: 0x04001ACF RID: 6863
		private bool parsingApplied;
	}
}
