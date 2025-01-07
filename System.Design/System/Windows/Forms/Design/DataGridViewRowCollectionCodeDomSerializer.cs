using System;
using System.CodeDom;
using System.Collections;
using System.ComponentModel.Design.Serialization;

namespace System.Windows.Forms.Design
{
	internal class DataGridViewRowCollectionCodeDomSerializer : CollectionCodeDomSerializer
	{
		private DataGridViewRowCollectionCodeDomSerializer()
		{
		}

		internal static DataGridViewRowCollectionCodeDomSerializer DefaultSerializer
		{
			get
			{
				if (DataGridViewRowCollectionCodeDomSerializer.defaultSerializer == null)
				{
					DataGridViewRowCollectionCodeDomSerializer.defaultSerializer = new DataGridViewRowCollectionCodeDomSerializer();
				}
				return DataGridViewRowCollectionCodeDomSerializer.defaultSerializer;
			}
		}

		protected override object SerializeCollection(IDesignerSerializationManager manager, CodeExpression targetExpression, Type targetType, ICollection originalCollection, ICollection valuesToSerialize)
		{
			return new CodeStatementCollection();
		}

		private static DataGridViewRowCollectionCodeDomSerializer defaultSerializer;
	}
}
