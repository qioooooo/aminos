using System;
using System.Collections;
using System.Threading;
using System.Xml.XmlConfiguration;

namespace System.Xml.Schema
{
	// Token: 0x0200023C RID: 572
	[Obsolete("Use System.Xml.Schema.XmlSchemaSet for schema compilation and validation. http://go.microsoft.com/fwlink/?linkid=14202")]
	public sealed class XmlSchemaCollection : ICollection, IEnumerable
	{
		// Token: 0x06001B45 RID: 6981 RVA: 0x000813A9 File Offset: 0x000803A9
		public XmlSchemaCollection()
			: this(new NameTable())
		{
		}

		// Token: 0x06001B46 RID: 6982 RVA: 0x000813B8 File Offset: 0x000803B8
		public XmlSchemaCollection(XmlNameTable nametable)
		{
			if (nametable == null)
			{
				throw new ArgumentNullException("nametable");
			}
			this.nameTable = nametable;
			this.collection = Hashtable.Synchronized(new Hashtable());
			this.xmlResolver = XmlReaderSection.CreateDefaultResolver();
			this.isThreadSafe = true;
			if (this.isThreadSafe)
			{
				this.wLock = new ReaderWriterLock();
			}
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06001B47 RID: 6983 RVA: 0x00081423 File Offset: 0x00080423
		public int Count
		{
			get
			{
				return this.collection.Count;
			}
		}

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06001B48 RID: 6984 RVA: 0x00081430 File Offset: 0x00080430
		public XmlNameTable NameTable
		{
			get
			{
				return this.nameTable;
			}
		}

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06001B49 RID: 6985 RVA: 0x00081438 File Offset: 0x00080438
		// (remove) Token: 0x06001B4A RID: 6986 RVA: 0x00081451 File Offset: 0x00080451
		public event ValidationEventHandler ValidationEventHandler
		{
			add
			{
				this.validationEventHandler = (ValidationEventHandler)Delegate.Combine(this.validationEventHandler, value);
			}
			remove
			{
				this.validationEventHandler = (ValidationEventHandler)Delegate.Remove(this.validationEventHandler, value);
			}
		}

		// Token: 0x170006D6 RID: 1750
		// (set) Token: 0x06001B4B RID: 6987 RVA: 0x0008146A File Offset: 0x0008046A
		internal XmlResolver XmlResolver
		{
			set
			{
				this.xmlResolver = value;
			}
		}

		// Token: 0x06001B4C RID: 6988 RVA: 0x00081474 File Offset: 0x00080474
		public XmlSchema Add(string ns, string uri)
		{
			if (uri == null || uri.Length == 0)
			{
				throw new ArgumentNullException("uri");
			}
			XmlTextReader xmlTextReader = new XmlTextReader(uri, this.nameTable);
			xmlTextReader.XmlResolver = this.xmlResolver;
			XmlSchema xmlSchema = null;
			try
			{
				xmlSchema = this.Add(ns, xmlTextReader, this.xmlResolver);
				while (xmlTextReader.Read())
				{
				}
			}
			finally
			{
				xmlTextReader.Close();
			}
			return xmlSchema;
		}

		// Token: 0x06001B4D RID: 6989 RVA: 0x000814E4 File Offset: 0x000804E4
		public XmlSchema Add(string ns, XmlReader reader)
		{
			return this.Add(ns, reader, this.xmlResolver);
		}

		// Token: 0x06001B4E RID: 6990 RVA: 0x000814F4 File Offset: 0x000804F4
		public XmlSchema Add(string ns, XmlReader reader, XmlResolver resolver)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			XmlNameTable xmlNameTable = reader.NameTable;
			SchemaInfo schemaInfo = new SchemaInfo();
			Parser parser = new Parser(SchemaType.None, xmlNameTable, this.GetSchemaNames(xmlNameTable), this.validationEventHandler);
			parser.XmlResolver = resolver;
			SchemaType schemaType;
			try
			{
				schemaType = parser.Parse(reader, ns);
			}
			catch (XmlSchemaException ex)
			{
				this.SendValidationEvent(ex);
				return null;
			}
			if (schemaType == SchemaType.XSD)
			{
				schemaInfo.SchemaType = SchemaType.XSD;
				return this.Add(ns, schemaInfo, parser.XmlSchema, true, resolver);
			}
			SchemaInfo xdrSchema = parser.XdrSchema;
			return this.Add(ns, parser.XdrSchema, null, true, resolver);
		}

		// Token: 0x06001B4F RID: 6991 RVA: 0x00081598 File Offset: 0x00080598
		public XmlSchema Add(XmlSchema schema)
		{
			return this.Add(schema, this.xmlResolver);
		}

		// Token: 0x06001B50 RID: 6992 RVA: 0x000815A8 File Offset: 0x000805A8
		public XmlSchema Add(XmlSchema schema, XmlResolver resolver)
		{
			if (schema == null)
			{
				throw new ArgumentNullException("schema");
			}
			SchemaInfo schemaInfo = new SchemaInfo();
			schemaInfo.SchemaType = SchemaType.XSD;
			return this.Add(schema.TargetNamespace, schemaInfo, schema, true, resolver);
		}

		// Token: 0x06001B51 RID: 6993 RVA: 0x000815E0 File Offset: 0x000805E0
		public void Add(XmlSchemaCollection schema)
		{
			if (schema == null)
			{
				throw new ArgumentNullException("schema");
			}
			if (this == schema)
			{
				return;
			}
			IDictionaryEnumerator enumerator = schema.collection.GetEnumerator();
			while (enumerator.MoveNext())
			{
				XmlSchemaCollectionNode xmlSchemaCollectionNode = (XmlSchemaCollectionNode)enumerator.Value;
				this.Add(xmlSchemaCollectionNode.NamespaceURI, xmlSchemaCollectionNode);
			}
		}

		// Token: 0x170006D7 RID: 1751
		public XmlSchema this[string ns]
		{
			get
			{
				XmlSchemaCollectionNode xmlSchemaCollectionNode = (XmlSchemaCollectionNode)this.collection[(ns != null) ? ns : string.Empty];
				if (xmlSchemaCollectionNode == null)
				{
					return null;
				}
				return xmlSchemaCollectionNode.Schema;
			}
		}

		// Token: 0x06001B53 RID: 6995 RVA: 0x00081664 File Offset: 0x00080664
		public bool Contains(XmlSchema schema)
		{
			if (schema == null)
			{
				throw new ArgumentNullException("schema");
			}
			return this[schema.TargetNamespace] != null;
		}

		// Token: 0x06001B54 RID: 6996 RVA: 0x00081686 File Offset: 0x00080686
		public bool Contains(string ns)
		{
			return this.collection[(ns != null) ? ns : string.Empty] != null;
		}

		// Token: 0x06001B55 RID: 6997 RVA: 0x000816A4 File Offset: 0x000806A4
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new XmlSchemaCollectionEnumerator(this.collection);
		}

		// Token: 0x06001B56 RID: 6998 RVA: 0x000816B1 File Offset: 0x000806B1
		public XmlSchemaCollectionEnumerator GetEnumerator()
		{
			return new XmlSchemaCollectionEnumerator(this.collection);
		}

		// Token: 0x06001B57 RID: 6999 RVA: 0x000816C0 File Offset: 0x000806C0
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			XmlSchemaCollectionEnumerator enumerator = this.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (index == array.Length && array.IsFixedSize)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				array.SetValue(enumerator.Current, index++);
			}
		}

		// Token: 0x06001B58 RID: 7000 RVA: 0x0008172C File Offset: 0x0008072C
		public void CopyTo(XmlSchema[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			foreach (XmlSchema xmlSchema in this)
			{
				if (xmlSchema != null)
				{
					if (index == array.Length)
					{
						throw new ArgumentOutOfRangeException("index");
					}
					XmlSchemaCollectionEnumerator enumerator;
					array[index++] = enumerator.Current;
				}
			}
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06001B59 RID: 7001 RVA: 0x00081790 File Offset: 0x00080790
		bool ICollection.IsSynchronized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06001B5A RID: 7002 RVA: 0x00081793 File Offset: 0x00080793
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06001B5B RID: 7003 RVA: 0x00081796 File Offset: 0x00080796
		int ICollection.Count
		{
			get
			{
				return this.collection.Count;
			}
		}

		// Token: 0x06001B5C RID: 7004 RVA: 0x000817A4 File Offset: 0x000807A4
		internal SchemaInfo GetSchemaInfo(string ns)
		{
			XmlSchemaCollectionNode xmlSchemaCollectionNode = (XmlSchemaCollectionNode)this.collection[(ns != null) ? ns : string.Empty];
			if (xmlSchemaCollectionNode == null)
			{
				return null;
			}
			return xmlSchemaCollectionNode.SchemaInfo;
		}

		// Token: 0x06001B5D RID: 7005 RVA: 0x000817D8 File Offset: 0x000807D8
		internal SchemaNames GetSchemaNames(XmlNameTable nt)
		{
			if (this.nameTable != nt)
			{
				return new SchemaNames(nt);
			}
			if (this.schemaNames == null)
			{
				this.schemaNames = new SchemaNames(this.nameTable);
			}
			return this.schemaNames;
		}

		// Token: 0x06001B5E RID: 7006 RVA: 0x00081809 File Offset: 0x00080809
		internal XmlSchema Add(string ns, SchemaInfo schemaInfo, XmlSchema schema, bool compile)
		{
			return this.Add(ns, schemaInfo, schema, compile, this.xmlResolver);
		}

		// Token: 0x06001B5F RID: 7007 RVA: 0x0008181C File Offset: 0x0008081C
		private XmlSchema Add(string ns, SchemaInfo schemaInfo, XmlSchema schema, bool compile, XmlResolver resolver)
		{
			int num = 0;
			if (schema != null)
			{
				if (schema.ErrorCount == 0 && compile)
				{
					if (!schema.CompileSchema(this, resolver, schemaInfo, ns, this.validationEventHandler, this.nameTable, true))
					{
						num = 1;
					}
					ns = ((schema.TargetNamespace == null) ? string.Empty : schema.TargetNamespace);
				}
				num += schema.ErrorCount;
			}
			else
			{
				num += schemaInfo.ErrorCount;
				ns = this.NameTable.Add(ns);
			}
			if (num == 0)
			{
				this.Add(ns, new XmlSchemaCollectionNode
				{
					NamespaceURI = ns,
					SchemaInfo = schemaInfo,
					Schema = schema
				});
				return schema;
			}
			return null;
		}

		// Token: 0x06001B60 RID: 7008 RVA: 0x000818B8 File Offset: 0x000808B8
		private void Add(string ns, XmlSchemaCollectionNode node)
		{
			if (this.isThreadSafe)
			{
				this.wLock.AcquireWriterLock(this.timeout);
			}
			try
			{
				if (this.collection[ns] != null)
				{
					this.collection.Remove(ns);
				}
				this.collection.Add(ns, node);
			}
			finally
			{
				if (this.isThreadSafe)
				{
					this.wLock.ReleaseWriterLock();
				}
			}
		}

		// Token: 0x06001B61 RID: 7009 RVA: 0x0008192C File Offset: 0x0008092C
		private void SendValidationEvent(XmlSchemaException e)
		{
			if (this.validationEventHandler != null)
			{
				this.validationEventHandler(this, new ValidationEventArgs(e));
				return;
			}
			throw e;
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06001B62 RID: 7010 RVA: 0x0008194A File Offset: 0x0008094A
		// (set) Token: 0x06001B63 RID: 7011 RVA: 0x00081952 File Offset: 0x00080952
		internal ValidationEventHandler EventHandler
		{
			get
			{
				return this.validationEventHandler;
			}
			set
			{
				this.validationEventHandler = value;
			}
		}

		// Token: 0x04001101 RID: 4353
		private Hashtable collection;

		// Token: 0x04001102 RID: 4354
		private XmlNameTable nameTable;

		// Token: 0x04001103 RID: 4355
		private SchemaNames schemaNames;

		// Token: 0x04001104 RID: 4356
		private ReaderWriterLock wLock;

		// Token: 0x04001105 RID: 4357
		private int timeout = -1;

		// Token: 0x04001106 RID: 4358
		private bool isThreadSafe = true;

		// Token: 0x04001107 RID: 4359
		private ValidationEventHandler validationEventHandler;

		// Token: 0x04001108 RID: 4360
		private XmlResolver xmlResolver;
	}
}
