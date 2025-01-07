using System;
using System.ComponentModel.Design;

namespace System.Windows.Forms.Design
{
	internal class DataGridColumnCollectionEditor : CollectionEditor
	{
		public DataGridColumnCollectionEditor(Type type)
			: base(type)
		{
		}

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
