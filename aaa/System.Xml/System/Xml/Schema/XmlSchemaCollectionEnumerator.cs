using System;
using System.Collections;

namespace System.Xml.Schema
{
	// Token: 0x0200023E RID: 574
	public sealed class XmlSchemaCollectionEnumerator : IEnumerator
	{
		// Token: 0x06001B6B RID: 7019 RVA: 0x00081996 File Offset: 0x00080996
		internal XmlSchemaCollectionEnumerator(Hashtable collection)
		{
			this.enumerator = collection.GetEnumerator();
		}

		// Token: 0x06001B6C RID: 7020 RVA: 0x000819AA File Offset: 0x000809AA
		void IEnumerator.Reset()
		{
			this.enumerator.Reset();
		}

		// Token: 0x06001B6D RID: 7021 RVA: 0x000819B7 File Offset: 0x000809B7
		bool IEnumerator.MoveNext()
		{
			return this.enumerator.MoveNext();
		}

		// Token: 0x06001B6E RID: 7022 RVA: 0x000819C4 File Offset: 0x000809C4
		public bool MoveNext()
		{
			return this.enumerator.MoveNext();
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06001B6F RID: 7023 RVA: 0x000819D1 File Offset: 0x000809D1
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06001B70 RID: 7024 RVA: 0x000819DC File Offset: 0x000809DC
		public XmlSchema Current
		{
			get
			{
				XmlSchemaCollectionNode xmlSchemaCollectionNode = (XmlSchemaCollectionNode)this.enumerator.Value;
				if (xmlSchemaCollectionNode != null)
				{
					return xmlSchemaCollectionNode.Schema;
				}
				return null;
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06001B71 RID: 7025 RVA: 0x00081A08 File Offset: 0x00080A08
		internal XmlSchemaCollectionNode CurrentNode
		{
			get
			{
				return (XmlSchemaCollectionNode)this.enumerator.Value;
			}
		}

		// Token: 0x0400110C RID: 4364
		private IDictionaryEnumerator enumerator;
	}
}
