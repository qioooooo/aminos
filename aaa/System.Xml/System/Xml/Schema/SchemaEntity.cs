using System;

namespace System.Xml.Schema
{
	// Token: 0x02000211 RID: 529
	internal sealed class SchemaEntity
	{
		// Token: 0x0600196D RID: 6509 RVA: 0x00079CF4 File Offset: 0x00078CF4
		internal SchemaEntity(XmlQualifiedName name, bool isParameter)
		{
			this.name = name;
			this.isParameter = isParameter;
		}

		// Token: 0x0600196E RID: 6510 RVA: 0x00079D18 File Offset: 0x00078D18
		internal static bool IsPredefinedEntity(string n)
		{
			return n == "lt" || n == "gt" || n == "amp" || n == "apos" || n == "quot";
		}

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x0600196F RID: 6511 RVA: 0x00079D66 File Offset: 0x00078D66
		internal XmlQualifiedName Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x06001970 RID: 6512 RVA: 0x00079D6E File Offset: 0x00078D6E
		// (set) Token: 0x06001971 RID: 6513 RVA: 0x00079D76 File Offset: 0x00078D76
		internal string Url
		{
			get
			{
				return this.url;
			}
			set
			{
				this.url = value;
				this.isExternal = true;
			}
		}

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x06001972 RID: 6514 RVA: 0x00079D86 File Offset: 0x00078D86
		// (set) Token: 0x06001973 RID: 6515 RVA: 0x00079D8E File Offset: 0x00078D8E
		internal string Pubid
		{
			get
			{
				return this.pubid;
			}
			set
			{
				this.pubid = value;
			}
		}

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x06001974 RID: 6516 RVA: 0x00079D97 File Offset: 0x00078D97
		// (set) Token: 0x06001975 RID: 6517 RVA: 0x00079D9F File Offset: 0x00078D9F
		internal bool IsProcessed
		{
			get
			{
				return this.isProcessed;
			}
			set
			{
				this.isProcessed = value;
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x06001976 RID: 6518 RVA: 0x00079DA8 File Offset: 0x00078DA8
		// (set) Token: 0x06001977 RID: 6519 RVA: 0x00079DB0 File Offset: 0x00078DB0
		internal bool IsExternal
		{
			get
			{
				return this.isExternal;
			}
			set
			{
				this.isExternal = value;
			}
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x06001978 RID: 6520 RVA: 0x00079DB9 File Offset: 0x00078DB9
		// (set) Token: 0x06001979 RID: 6521 RVA: 0x00079DC1 File Offset: 0x00078DC1
		internal bool DeclaredInExternal
		{
			get
			{
				return this.isDeclaredInExternal;
			}
			set
			{
				this.isDeclaredInExternal = value;
			}
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x0600197A RID: 6522 RVA: 0x00079DCA File Offset: 0x00078DCA
		// (set) Token: 0x0600197B RID: 6523 RVA: 0x00079DD2 File Offset: 0x00078DD2
		internal bool IsParEntity
		{
			get
			{
				return this.isParameter;
			}
			set
			{
				this.isParameter = value;
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x0600197C RID: 6524 RVA: 0x00079DDB File Offset: 0x00078DDB
		// (set) Token: 0x0600197D RID: 6525 RVA: 0x00079DE3 File Offset: 0x00078DE3
		internal XmlQualifiedName NData
		{
			get
			{
				return this.ndata;
			}
			set
			{
				this.ndata = value;
			}
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x0600197E RID: 6526 RVA: 0x00079DEC File Offset: 0x00078DEC
		// (set) Token: 0x0600197F RID: 6527 RVA: 0x00079DF4 File Offset: 0x00078DF4
		internal string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				this.text = value;
				this.isExternal = false;
			}
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x06001980 RID: 6528 RVA: 0x00079E04 File Offset: 0x00078E04
		// (set) Token: 0x06001981 RID: 6529 RVA: 0x00079E0C File Offset: 0x00078E0C
		internal int Line
		{
			get
			{
				return this.lineNumber;
			}
			set
			{
				this.lineNumber = value;
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x06001982 RID: 6530 RVA: 0x00079E15 File Offset: 0x00078E15
		// (set) Token: 0x06001983 RID: 6531 RVA: 0x00079E1D File Offset: 0x00078E1D
		internal int Pos
		{
			get
			{
				return this.linePosition;
			}
			set
			{
				this.linePosition = value;
			}
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x06001984 RID: 6532 RVA: 0x00079E26 File Offset: 0x00078E26
		// (set) Token: 0x06001985 RID: 6533 RVA: 0x00079E3C File Offset: 0x00078E3C
		internal string BaseURI
		{
			get
			{
				if (this.baseURI != null)
				{
					return this.baseURI;
				}
				return string.Empty;
			}
			set
			{
				this.baseURI = value;
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x06001986 RID: 6534 RVA: 0x00079E45 File Offset: 0x00078E45
		// (set) Token: 0x06001987 RID: 6535 RVA: 0x00079E5B File Offset: 0x00078E5B
		internal string DeclaredURI
		{
			get
			{
				if (this.declaredURI != null)
				{
					return this.declaredURI;
				}
				return string.Empty;
			}
			set
			{
				this.declaredURI = value;
			}
		}

		// Token: 0x04000ED5 RID: 3797
		private XmlQualifiedName name;

		// Token: 0x04000ED6 RID: 3798
		private string url;

		// Token: 0x04000ED7 RID: 3799
		private string pubid;

		// Token: 0x04000ED8 RID: 3800
		private string text;

		// Token: 0x04000ED9 RID: 3801
		private XmlQualifiedName ndata = XmlQualifiedName.Empty;

		// Token: 0x04000EDA RID: 3802
		private int lineNumber;

		// Token: 0x04000EDB RID: 3803
		private int linePosition;

		// Token: 0x04000EDC RID: 3804
		private bool isParameter;

		// Token: 0x04000EDD RID: 3805
		private bool isExternal;

		// Token: 0x04000EDE RID: 3806
		private bool isProcessed;

		// Token: 0x04000EDF RID: 3807
		private bool isDeclaredInExternal;

		// Token: 0x04000EE0 RID: 3808
		private string baseURI;

		// Token: 0x04000EE1 RID: 3809
		private string declaredURI;
	}
}
