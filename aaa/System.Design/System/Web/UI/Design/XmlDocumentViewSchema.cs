using System;
using System.Collections;
using System.Collections.Specialized;

namespace System.Web.UI.Design
{
	// Token: 0x020003C0 RID: 960
	internal sealed class XmlDocumentViewSchema : IDataSourceViewSchema
	{
		// Token: 0x06002350 RID: 9040 RVA: 0x000BEA58 File Offset: 0x000BDA58
		public XmlDocumentViewSchema(string name, Pair data, bool includeSpecialSchema)
		{
			this._includeSpecialSchema = includeSpecialSchema;
			this._children = (OrderedDictionary)data.First;
			this._attrs = (ArrayList)data.Second;
			this._name = name;
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x06002351 RID: 9041 RVA: 0x000BEA90 File Offset: 0x000BDA90
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x06002352 RID: 9042 RVA: 0x000BEA98 File Offset: 0x000BDA98
		public IDataSourceViewSchema[] GetChildren()
		{
			if (this._viewSchemas == null)
			{
				this._viewSchemas = new IDataSourceViewSchema[this._children.Count];
				int num = 0;
				foreach (object obj in this._children)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					this._viewSchemas[num] = new XmlDocumentViewSchema((string)dictionaryEntry.Key, (Pair)dictionaryEntry.Value, this._includeSpecialSchema);
					num++;
				}
			}
			return this._viewSchemas;
		}

		// Token: 0x06002353 RID: 9043 RVA: 0x000BEB40 File Offset: 0x000BDB40
		public IDataSourceFieldSchema[] GetFields()
		{
			if (this._fieldSchemas == null)
			{
				int num = (this._includeSpecialSchema ? 3 : 0);
				this._fieldSchemas = new IDataSourceFieldSchema[this._attrs.Count + num];
				if (this._includeSpecialSchema)
				{
					this._fieldSchemas[0] = new XmlDocumentFieldSchema("#Name");
					this._fieldSchemas[1] = new XmlDocumentFieldSchema("#Value");
					this._fieldSchemas[2] = new XmlDocumentFieldSchema("#InnerText");
				}
				for (int i = 0; i < this._attrs.Count; i++)
				{
					this._fieldSchemas[i + num] = new XmlDocumentFieldSchema((string)this._attrs[i]);
				}
			}
			return this._fieldSchemas;
		}

		// Token: 0x04001897 RID: 6295
		private string _name;

		// Token: 0x04001898 RID: 6296
		private OrderedDictionary _children;

		// Token: 0x04001899 RID: 6297
		private ArrayList _attrs;

		// Token: 0x0400189A RID: 6298
		private IDataSourceViewSchema[] _viewSchemas;

		// Token: 0x0400189B RID: 6299
		private IDataSourceFieldSchema[] _fieldSchemas;

		// Token: 0x0400189C RID: 6300
		private bool _includeSpecialSchema;
	}
}
