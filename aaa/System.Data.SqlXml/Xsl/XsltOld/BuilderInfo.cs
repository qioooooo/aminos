using System;
using System.Text;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x0200013B RID: 315
	internal class BuilderInfo
	{
		// Token: 0x06000DBF RID: 3519 RVA: 0x000474D1 File Offset: 0x000464D1
		internal BuilderInfo()
		{
			this.Initialize(string.Empty, string.Empty, string.Empty);
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x000474FA File Offset: 0x000464FA
		internal void Initialize(string prefix, string name, string nspace)
		{
			this.prefix = prefix;
			this.localName = name;
			this.namespaceURI = nspace;
			this.name = null;
			this.htmlProps = null;
			this.htmlAttrProps = null;
			this.TextInfoCount = 0;
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x00047530 File Offset: 0x00046530
		internal void Initialize(BuilderInfo src)
		{
			this.prefix = src.Prefix;
			this.localName = src.LocalName;
			this.namespaceURI = src.NamespaceURI;
			this.name = null;
			this.depth = src.Depth;
			this.nodeType = src.NodeType;
			this.htmlProps = src.htmlProps;
			this.htmlAttrProps = src.htmlAttrProps;
			this.TextInfoCount = 0;
			this.EnsureTextInfoSize(src.TextInfoCount);
			src.TextInfo.CopyTo(this.TextInfo, 0);
			this.TextInfoCount = src.TextInfoCount;
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x000475CC File Offset: 0x000465CC
		private void EnsureTextInfoSize(int newSize)
		{
			if (this.TextInfo.Length < newSize)
			{
				string[] array = new string[newSize * 2];
				Array.Copy(this.TextInfo, array, this.TextInfoCount);
				this.TextInfo = array;
			}
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x00047608 File Offset: 0x00046608
		internal BuilderInfo Clone()
		{
			BuilderInfo builderInfo = new BuilderInfo();
			builderInfo.Initialize(this);
			return builderInfo;
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000DC4 RID: 3524 RVA: 0x00047624 File Offset: 0x00046624
		internal string Name
		{
			get
			{
				if (this.name == null)
				{
					string text = this.Prefix;
					string text2 = this.LocalName;
					if (text != null && 0 < text.Length)
					{
						if (text2.Length > 0)
						{
							this.name = text + ":" + text2;
						}
						else
						{
							this.name = text;
						}
					}
					else
					{
						this.name = text2;
					}
				}
				return this.name;
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000DC5 RID: 3525 RVA: 0x00047686 File Offset: 0x00046686
		// (set) Token: 0x06000DC6 RID: 3526 RVA: 0x0004768E File Offset: 0x0004668E
		internal string LocalName
		{
			get
			{
				return this.localName;
			}
			set
			{
				this.localName = value;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000DC7 RID: 3527 RVA: 0x00047697 File Offset: 0x00046697
		// (set) Token: 0x06000DC8 RID: 3528 RVA: 0x0004769F File Offset: 0x0004669F
		internal string NamespaceURI
		{
			get
			{
				return this.namespaceURI;
			}
			set
			{
				this.namespaceURI = value;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000DC9 RID: 3529 RVA: 0x000476A8 File Offset: 0x000466A8
		// (set) Token: 0x06000DCA RID: 3530 RVA: 0x000476B0 File Offset: 0x000466B0
		internal string Prefix
		{
			get
			{
				return this.prefix;
			}
			set
			{
				this.prefix = value;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000DCB RID: 3531 RVA: 0x000476BC File Offset: 0x000466BC
		// (set) Token: 0x06000DCC RID: 3532 RVA: 0x00047753 File Offset: 0x00046753
		internal string Value
		{
			get
			{
				switch (this.TextInfoCount)
				{
				case 0:
					return string.Empty;
				case 1:
					return this.TextInfo[0];
				default:
				{
					int num = 0;
					for (int i = 0; i < this.TextInfoCount; i++)
					{
						string text = this.TextInfo[i];
						if (text != null)
						{
							num += text.Length;
						}
					}
					StringBuilder stringBuilder = new StringBuilder(num);
					for (int j = 0; j < this.TextInfoCount; j++)
					{
						string text2 = this.TextInfo[j];
						if (text2 != null)
						{
							stringBuilder.Append(text2);
						}
					}
					return stringBuilder.ToString();
				}
				}
			}
			set
			{
				this.TextInfoCount = 0;
				this.ValueAppend(value, false);
			}
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x00047764 File Offset: 0x00046764
		internal void ValueAppend(string s, bool disableEscaping)
		{
			if (s == null || s.Length == 0)
			{
				return;
			}
			this.EnsureTextInfoSize(this.TextInfoCount + (disableEscaping ? 2 : 1));
			if (disableEscaping)
			{
				this.TextInfo[this.TextInfoCount++] = null;
			}
			this.TextInfo[this.TextInfoCount++] = s;
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000DCE RID: 3534 RVA: 0x000477C6 File Offset: 0x000467C6
		// (set) Token: 0x06000DCF RID: 3535 RVA: 0x000477CE File Offset: 0x000467CE
		internal XmlNodeType NodeType
		{
			get
			{
				return this.nodeType;
			}
			set
			{
				this.nodeType = value;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000DD0 RID: 3536 RVA: 0x000477D7 File Offset: 0x000467D7
		// (set) Token: 0x06000DD1 RID: 3537 RVA: 0x000477DF File Offset: 0x000467DF
		internal int Depth
		{
			get
			{
				return this.depth;
			}
			set
			{
				this.depth = value;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000DD2 RID: 3538 RVA: 0x000477E8 File Offset: 0x000467E8
		// (set) Token: 0x06000DD3 RID: 3539 RVA: 0x000477F0 File Offset: 0x000467F0
		internal bool IsEmptyTag
		{
			get
			{
				return this.isEmptyTag;
			}
			set
			{
				this.isEmptyTag = value;
			}
		}

		// Token: 0x0400090C RID: 2316
		private string name;

		// Token: 0x0400090D RID: 2317
		private string localName;

		// Token: 0x0400090E RID: 2318
		private string namespaceURI;

		// Token: 0x0400090F RID: 2319
		private string prefix;

		// Token: 0x04000910 RID: 2320
		private XmlNodeType nodeType;

		// Token: 0x04000911 RID: 2321
		private int depth;

		// Token: 0x04000912 RID: 2322
		private bool isEmptyTag;

		// Token: 0x04000913 RID: 2323
		internal string[] TextInfo = new string[4];

		// Token: 0x04000914 RID: 2324
		internal int TextInfoCount;

		// Token: 0x04000915 RID: 2325
		internal bool search;

		// Token: 0x04000916 RID: 2326
		internal HtmlElementProps htmlProps;

		// Token: 0x04000917 RID: 2327
		internal HtmlAttributeProps htmlAttrProps;
	}
}
