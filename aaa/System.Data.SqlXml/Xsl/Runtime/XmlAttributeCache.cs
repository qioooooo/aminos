using System;
using System.Xml.Schema;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000A2 RID: 162
	internal sealed class XmlAttributeCache : XmlRawWriter, IRemovableWriter
	{
		// Token: 0x060007A1 RID: 1953 RVA: 0x000270A5 File Offset: 0x000260A5
		public void Init(XmlRawWriter wrapped)
		{
			this.SetWrappedWriter(wrapped);
			this.numEntries = 0;
			this.idxLastName = 0;
			this.hashCodeUnion = 0;
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060007A2 RID: 1954 RVA: 0x000270C3 File Offset: 0x000260C3
		public int Count
		{
			get
			{
				return this.numEntries;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060007A3 RID: 1955 RVA: 0x000270CB File Offset: 0x000260CB
		// (set) Token: 0x060007A4 RID: 1956 RVA: 0x000270D3 File Offset: 0x000260D3
		public OnRemoveWriter OnRemoveWriterEvent
		{
			get
			{
				return this.onRemove;
			}
			set
			{
				this.onRemove = value;
			}
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x000270DC File Offset: 0x000260DC
		private void SetWrappedWriter(XmlRawWriter writer)
		{
			IRemovableWriter removableWriter = writer as IRemovableWriter;
			if (removableWriter != null)
			{
				removableWriter.OnRemoveWriterEvent = new OnRemoveWriter(this.SetWrappedWriter);
			}
			this.wrapped = writer;
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x0002710C File Offset: 0x0002610C
		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			int num = 0;
			int num2 = 1 << (int)localName[0];
			if ((this.hashCodeUnion & num2) != 0)
			{
				while (!this.arrAttrs[num].IsDuplicate(localName, ns, num2))
				{
					num = this.arrAttrs[num].NextNameIndex;
					if (num == 0)
					{
						break;
					}
				}
			}
			else
			{
				this.hashCodeUnion |= num2;
			}
			this.EnsureAttributeCache();
			if (this.numEntries != 0)
			{
				this.arrAttrs[this.idxLastName].NextNameIndex = this.numEntries;
			}
			this.idxLastName = this.numEntries++;
			this.arrAttrs[this.idxLastName].Init(prefix, localName, ns, num2);
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x000271CB File Offset: 0x000261CB
		public override void WriteEndAttribute()
		{
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x000271CD File Offset: 0x000261CD
		internal override void WriteNamespaceDeclaration(string prefix, string ns)
		{
			this.FlushAttributes();
			this.wrapped.WriteNamespaceDeclaration(prefix, ns);
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x000271E4 File Offset: 0x000261E4
		public override void WriteString(string text)
		{
			this.EnsureAttributeCache();
			this.arrAttrs[this.numEntries++].Init(text);
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x0002721C File Offset: 0x0002621C
		public override void WriteValue(object value)
		{
			this.EnsureAttributeCache();
			this.arrAttrs[this.numEntries++].Init((XmlAtomicValue)value);
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x00027256 File Offset: 0x00026256
		public override void WriteValue(string value)
		{
			this.WriteValue(value);
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x0002725F File Offset: 0x0002625F
		internal override void StartElementContent()
		{
			this.FlushAttributes();
			this.wrapped.StartElementContent();
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x00027272 File Offset: 0x00026272
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x00027274 File Offset: 0x00026274
		internal override void WriteEndElement(string prefix, string localName, string ns)
		{
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x00027276 File Offset: 0x00026276
		public override void WriteComment(string text)
		{
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x00027278 File Offset: 0x00026278
		public override void WriteProcessingInstruction(string name, string text)
		{
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x0002727A File Offset: 0x0002627A
		public override void WriteEntityRef(string name)
		{
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x0002727C File Offset: 0x0002627C
		public override void Close()
		{
			this.wrapped.Close();
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x00027289 File Offset: 0x00026289
		public override void Flush()
		{
			this.wrapped.Flush();
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x00027298 File Offset: 0x00026298
		private void FlushAttributes()
		{
			int num = 0;
			while (num != this.numEntries)
			{
				int nextNameIndex = this.arrAttrs[num].NextNameIndex;
				if (nextNameIndex == 0)
				{
					nextNameIndex = this.numEntries;
				}
				string localName = this.arrAttrs[num].LocalName;
				if (localName != null)
				{
					string prefix = this.arrAttrs[num].Prefix;
					string @namespace = this.arrAttrs[num].Namespace;
					this.wrapped.WriteStartAttribute(prefix, localName, @namespace);
					while (++num != nextNameIndex)
					{
						string text = this.arrAttrs[num].Text;
						if (text != null)
						{
							this.wrapped.WriteString(text);
						}
						else
						{
							this.wrapped.WriteValue(this.arrAttrs[num].Value);
						}
					}
					this.wrapped.WriteEndAttribute();
				}
				else
				{
					num = nextNameIndex;
				}
			}
			if (this.onRemove != null)
			{
				this.onRemove(this.wrapped);
			}
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x00027394 File Offset: 0x00026394
		private void EnsureAttributeCache()
		{
			if (this.arrAttrs == null)
			{
				this.arrAttrs = new XmlAttributeCache.AttrNameVal[32];
				return;
			}
			if (this.numEntries >= this.arrAttrs.Length)
			{
				XmlAttributeCache.AttrNameVal[] array = new XmlAttributeCache.AttrNameVal[this.numEntries * 2];
				Array.Copy(this.arrAttrs, array, this.numEntries);
				this.arrAttrs = array;
			}
		}

		// Token: 0x04000522 RID: 1314
		private const int DefaultCacheSize = 32;

		// Token: 0x04000523 RID: 1315
		private XmlRawWriter wrapped;

		// Token: 0x04000524 RID: 1316
		private OnRemoveWriter onRemove;

		// Token: 0x04000525 RID: 1317
		private XmlAttributeCache.AttrNameVal[] arrAttrs;

		// Token: 0x04000526 RID: 1318
		private int numEntries;

		// Token: 0x04000527 RID: 1319
		private int idxLastName;

		// Token: 0x04000528 RID: 1320
		private int hashCodeUnion;

		// Token: 0x020000A3 RID: 163
		private struct AttrNameVal
		{
			// Token: 0x17000131 RID: 305
			// (get) Token: 0x060007B7 RID: 1975 RVA: 0x000273F6 File Offset: 0x000263F6
			public string LocalName
			{
				get
				{
					return this.localName;
				}
			}

			// Token: 0x17000132 RID: 306
			// (get) Token: 0x060007B8 RID: 1976 RVA: 0x000273FE File Offset: 0x000263FE
			public string Prefix
			{
				get
				{
					return this.prefix;
				}
			}

			// Token: 0x17000133 RID: 307
			// (get) Token: 0x060007B9 RID: 1977 RVA: 0x00027406 File Offset: 0x00026406
			public string Namespace
			{
				get
				{
					return this.namespaceName;
				}
			}

			// Token: 0x17000134 RID: 308
			// (get) Token: 0x060007BA RID: 1978 RVA: 0x0002740E File Offset: 0x0002640E
			public string Text
			{
				get
				{
					return this.text;
				}
			}

			// Token: 0x17000135 RID: 309
			// (get) Token: 0x060007BB RID: 1979 RVA: 0x00027416 File Offset: 0x00026416
			public XmlAtomicValue Value
			{
				get
				{
					return this.value;
				}
			}

			// Token: 0x17000136 RID: 310
			// (get) Token: 0x060007BC RID: 1980 RVA: 0x0002741E File Offset: 0x0002641E
			// (set) Token: 0x060007BD RID: 1981 RVA: 0x00027426 File Offset: 0x00026426
			public int NextNameIndex
			{
				get
				{
					return this.nextNameIndex;
				}
				set
				{
					this.nextNameIndex = value;
				}
			}

			// Token: 0x060007BE RID: 1982 RVA: 0x0002742F File Offset: 0x0002642F
			public void Init(string prefix, string localName, string ns, int hashCode)
			{
				this.localName = localName;
				this.prefix = prefix;
				this.namespaceName = ns;
				this.hashCode = hashCode;
				this.nextNameIndex = 0;
			}

			// Token: 0x060007BF RID: 1983 RVA: 0x00027455 File Offset: 0x00026455
			public void Init(string text)
			{
				this.text = text;
				this.value = null;
			}

			// Token: 0x060007C0 RID: 1984 RVA: 0x00027465 File Offset: 0x00026465
			public void Init(XmlAtomicValue value)
			{
				this.text = null;
				this.value = value;
			}

			// Token: 0x060007C1 RID: 1985 RVA: 0x00027475 File Offset: 0x00026475
			public bool IsDuplicate(string localName, string ns, int hashCode)
			{
				if (this.localName != null && this.hashCode == hashCode && this.localName.Equals(localName) && this.namespaceName.Equals(ns))
				{
					this.localName = null;
					return true;
				}
				return false;
			}

			// Token: 0x04000529 RID: 1321
			private string localName;

			// Token: 0x0400052A RID: 1322
			private string prefix;

			// Token: 0x0400052B RID: 1323
			private string namespaceName;

			// Token: 0x0400052C RID: 1324
			private string text;

			// Token: 0x0400052D RID: 1325
			private XmlAtomicValue value;

			// Token: 0x0400052E RID: 1326
			private int hashCode;

			// Token: 0x0400052F RID: 1327
			private int nextNameIndex;
		}
	}
}
