using System;
using System.Collections.Generic;
using System.Xml.Xsl.Qil;
using System.Xml.Xsl.Runtime;

namespace System.Xml.Xsl.IlGen
{
	// Token: 0x0200002C RID: 44
	internal class StaticDataManager
	{
		// Token: 0x060001D3 RID: 467 RVA: 0x0000D86F File Offset: 0x0000C86F
		public int DeclareName(string name)
		{
			if (this.uniqueNames == null)
			{
				this.uniqueNames = new UniqueList<string>();
			}
			return this.uniqueNames.Add(name);
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x0000D890 File Offset: 0x0000C890
		public string[] Names
		{
			get
			{
				if (this.uniqueNames == null)
				{
					return null;
				}
				return this.uniqueNames.ToArray();
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000D8A7 File Offset: 0x0000C8A7
		public int DeclareNameFilter(string locName, string nsUri)
		{
			if (this.uniqueFilters == null)
			{
				this.uniqueFilters = new UniqueList<Int32Pair>();
			}
			return this.uniqueFilters.Add(new Int32Pair(this.DeclareName(locName), this.DeclareName(nsUri)));
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x0000D8DA File Offset: 0x0000C8DA
		public Int32Pair[] NameFilters
		{
			get
			{
				if (this.uniqueFilters == null)
				{
					return null;
				}
				return this.uniqueFilters.ToArray();
			}
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000D8F4 File Offset: 0x0000C8F4
		public int DeclarePrefixMappings(IList<QilNode> list)
		{
			StringPair[] array = new StringPair[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				QilBinary qilBinary = (QilBinary)list[i];
				array[i] = new StringPair((QilLiteral)qilBinary.Left, (QilLiteral)qilBinary.Right);
			}
			if (this.prefixMappingsList == null)
			{
				this.prefixMappingsList = new List<StringPair[]>();
			}
			this.prefixMappingsList.Add(array);
			return this.prefixMappingsList.Count - 1;
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x0000D988 File Offset: 0x0000C988
		public StringPair[][] PrefixMappingsList
		{
			get
			{
				if (this.prefixMappingsList == null)
				{
					return null;
				}
				return this.prefixMappingsList.ToArray();
			}
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000D9A0 File Offset: 0x0000C9A0
		public int DeclareGlobalValue(string name)
		{
			if (this.globalNames == null)
			{
				this.globalNames = new List<string>();
			}
			int count = this.globalNames.Count;
			this.globalNames.Add(name);
			return count;
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060001DA RID: 474 RVA: 0x0000D9D9 File Offset: 0x0000C9D9
		public string[] GlobalNames
		{
			get
			{
				if (this.globalNames == null)
				{
					return null;
				}
				return this.globalNames.ToArray();
			}
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000D9F0 File Offset: 0x0000C9F0
		public int DeclareEarlyBound(string namespaceUri, Type ebType)
		{
			if (this.earlyInfo == null)
			{
				this.earlyInfo = new UniqueList<EarlyBoundInfo>();
			}
			return this.earlyInfo.Add(new EarlyBoundInfo(namespaceUri, ebType));
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060001DC RID: 476 RVA: 0x0000DA17 File Offset: 0x0000CA17
		public EarlyBoundInfo[] EarlyBound
		{
			get
			{
				if (this.earlyInfo != null)
				{
					return this.earlyInfo.ToArray();
				}
				return null;
			}
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000DA2E File Offset: 0x0000CA2E
		public int DeclareXmlType(XmlQueryType type)
		{
			if (this.uniqueXmlTypes == null)
			{
				this.uniqueXmlTypes = new UniqueList<XmlQueryType>();
			}
			return this.uniqueXmlTypes.Add(type);
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001DE RID: 478 RVA: 0x0000DA4F File Offset: 0x0000CA4F
		public XmlQueryType[] XmlTypes
		{
			get
			{
				if (this.uniqueXmlTypes == null)
				{
					return null;
				}
				return this.uniqueXmlTypes.ToArray();
			}
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000DA66 File Offset: 0x0000CA66
		public int DeclareCollation(string collation)
		{
			if (this.uniqueCollations == null)
			{
				this.uniqueCollations = new UniqueList<XmlCollation>();
			}
			return this.uniqueCollations.Add(XmlCollation.Create(collation));
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x0000DA8C File Offset: 0x0000CA8C
		public XmlCollation[] Collations
		{
			get
			{
				if (this.uniqueCollations == null)
				{
					return null;
				}
				return this.uniqueCollations.ToArray();
			}
		}

		// Token: 0x04000288 RID: 648
		private UniqueList<string> uniqueNames;

		// Token: 0x04000289 RID: 649
		private UniqueList<Int32Pair> uniqueFilters;

		// Token: 0x0400028A RID: 650
		private List<StringPair[]> prefixMappingsList;

		// Token: 0x0400028B RID: 651
		private List<string> globalNames;

		// Token: 0x0400028C RID: 652
		private UniqueList<EarlyBoundInfo> earlyInfo;

		// Token: 0x0400028D RID: 653
		private UniqueList<XmlQueryType> uniqueXmlTypes;

		// Token: 0x0400028E RID: 654
		private UniqueList<XmlCollation> uniqueCollations;
	}
}
