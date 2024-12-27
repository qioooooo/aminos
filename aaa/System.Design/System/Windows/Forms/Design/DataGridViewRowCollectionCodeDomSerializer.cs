using System;
using System.CodeDom;
using System.Collections;
using System.ComponentModel.Design.Serialization;

namespace System.Windows.Forms.Design
{
	// Token: 0x020001F3 RID: 499
	internal class DataGridViewRowCollectionCodeDomSerializer : CollectionCodeDomSerializer
	{
		// Token: 0x06001335 RID: 4917 RVA: 0x000624CC File Offset: 0x000614CC
		private DataGridViewRowCollectionCodeDomSerializer()
		{
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06001336 RID: 4918 RVA: 0x000624D4 File Offset: 0x000614D4
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

		// Token: 0x06001337 RID: 4919 RVA: 0x000624EC File Offset: 0x000614EC
		protected override object SerializeCollection(IDesignerSerializationManager manager, CodeExpression targetExpression, Type targetType, ICollection originalCollection, ICollection valuesToSerialize)
		{
			return new CodeStatementCollection();
		}

		// Token: 0x04001199 RID: 4505
		private static DataGridViewRowCollectionCodeDomSerializer defaultSerializer;
	}
}
