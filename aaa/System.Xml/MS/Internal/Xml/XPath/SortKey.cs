using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x0200015D RID: 349
	internal sealed class SortKey
	{
		// Token: 0x060012F9 RID: 4857 RVA: 0x0005264E File Offset: 0x0005164E
		public SortKey(int numKeys, int originalPosition, XPathNavigator node)
		{
			this.numKeys = numKeys;
			this.keys = new object[numKeys];
			this.originalPosition = originalPosition;
			this.node = node;
		}

		// Token: 0x1700049D RID: 1181
		public object this[int index]
		{
			get
			{
				return this.keys[index];
			}
			set
			{
				this.keys[index] = value;
			}
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x060012FC RID: 4860 RVA: 0x0005268C File Offset: 0x0005168C
		public int NumKeys
		{
			get
			{
				return this.numKeys;
			}
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x060012FD RID: 4861 RVA: 0x00052694 File Offset: 0x00051694
		public int OriginalPosition
		{
			get
			{
				return this.originalPosition;
			}
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x060012FE RID: 4862 RVA: 0x0005269C File Offset: 0x0005169C
		public XPathNavigator Node
		{
			get
			{
				return this.node;
			}
		}

		// Token: 0x04000BD6 RID: 3030
		private int numKeys;

		// Token: 0x04000BD7 RID: 3031
		private object[] keys;

		// Token: 0x04000BD8 RID: 3032
		private int originalPosition;

		// Token: 0x04000BD9 RID: 3033
		private XPathNavigator node;
	}
}
