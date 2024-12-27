using System;
using System.Collections;

namespace System.Xml.Serialization
{
	// Token: 0x020002DF RID: 735
	internal class NameTable : INameScope
	{
		// Token: 0x06002266 RID: 8806 RVA: 0x000A0FDC File Offset: 0x0009FFDC
		internal void Add(XmlQualifiedName qname, object value)
		{
			this.Add(qname.Name, qname.Namespace, value);
		}

		// Token: 0x06002267 RID: 8807 RVA: 0x000A0FF4 File Offset: 0x0009FFF4
		internal void Add(string name, string ns, object value)
		{
			NameKey nameKey = new NameKey(name, ns);
			this.table.Add(nameKey, value);
		}

		// Token: 0x1700086B RID: 2155
		internal object this[XmlQualifiedName qname]
		{
			get
			{
				return this.table[new NameKey(qname.Name, qname.Namespace)];
			}
			set
			{
				this.table[new NameKey(qname.Name, qname.Namespace)] = value;
			}
		}

		// Token: 0x1700086C RID: 2156
		internal object this[string name, string ns]
		{
			get
			{
				return this.table[new NameKey(name, ns)];
			}
			set
			{
				this.table[new NameKey(name, ns)] = value;
			}
		}

		// Token: 0x1700086D RID: 2157
		object INameScope.this[string name, string ns]
		{
			get
			{
				return this.table[new NameKey(name, ns)];
			}
			set
			{
				this.table[new NameKey(name, ns)] = value;
			}
		}

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x0600226E RID: 8814 RVA: 0x000A10A5 File Offset: 0x000A00A5
		internal ICollection Values
		{
			get
			{
				return this.table.Values;
			}
		}

		// Token: 0x0600226F RID: 8815 RVA: 0x000A10B4 File Offset: 0x000A00B4
		internal Array ToArray(Type type)
		{
			Array array = Array.CreateInstance(type, this.table.Count);
			this.table.Values.CopyTo(array, 0);
			return array;
		}

		// Token: 0x040014C2 RID: 5314
		private Hashtable table = new Hashtable();
	}
}
