using System;
using System.Collections;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000190 RID: 400
	internal class Key
	{
		// Token: 0x06001119 RID: 4377 RVA: 0x000524F5 File Offset: 0x000514F5
		public Key(XmlQualifiedName name, int matchkey, int usekey)
		{
			this.name = name;
			this.matchKey = matchkey;
			this.useKey = usekey;
			this.keyNodes = null;
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x0600111A RID: 4378 RVA: 0x00052519 File Offset: 0x00051519
		public XmlQualifiedName Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x0600111B RID: 4379 RVA: 0x00052521 File Offset: 0x00051521
		public int MatchKey
		{
			get
			{
				return this.matchKey;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x0600111C RID: 4380 RVA: 0x00052529 File Offset: 0x00051529
		public int UseKey
		{
			get
			{
				return this.useKey;
			}
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x00052531 File Offset: 0x00051531
		public void AddKey(XPathNavigator root, Hashtable table)
		{
			if (this.keyNodes == null)
			{
				this.keyNodes = new ArrayList();
			}
			this.keyNodes.Add(new DocumentKeyList(root, table));
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x00052560 File Offset: 0x00051560
		public Hashtable GetKeys(XPathNavigator root)
		{
			if (this.keyNodes != null)
			{
				for (int i = 0; i < this.keyNodes.Count; i++)
				{
					if (((DocumentKeyList)this.keyNodes[i]).RootNav.IsSamePosition(root))
					{
						return ((DocumentKeyList)this.keyNodes[i]).KeyTable;
					}
				}
			}
			return null;
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x000525C7 File Offset: 0x000515C7
		public Key Clone()
		{
			return new Key(this.name, this.matchKey, this.useKey);
		}

		// Token: 0x04000B53 RID: 2899
		private XmlQualifiedName name;

		// Token: 0x04000B54 RID: 2900
		private int matchKey;

		// Token: 0x04000B55 RID: 2901
		private int useKey;

		// Token: 0x04000B56 RID: 2902
		private ArrayList keyNodes;
	}
}
