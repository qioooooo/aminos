using System;
using System.ComponentModel.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x020001D8 RID: 472
	internal class DataGridColumnCollectionEditor : CollectionEditor
	{
		// Token: 0x0600123D RID: 4669 RVA: 0x0005AB9A File Offset: 0x00059B9A
		public DataGridColumnCollectionEditor(Type type)
			: base(type)
		{
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x0005ABA4 File Offset: 0x00059BA4
		protected override Type[] CreateNewItemTypes()
		{
			return new Type[]
			{
				typeof(DataGridTextBoxColumn),
				typeof(DataGridBoolColumn)
			};
		}
	}
}
