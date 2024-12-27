using System;

namespace System.Xml.XPath
{
	// Token: 0x0200011C RID: 284
	internal class XmlEmptyNavigator : XPathNavigator
	{
		// Token: 0x06001103 RID: 4355 RVA: 0x0004D33A File Offset: 0x0004C33A
		private XmlEmptyNavigator()
		{
		}

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06001104 RID: 4356 RVA: 0x0004D342 File Offset: 0x0004C342
		public static XmlEmptyNavigator Singleton
		{
			get
			{
				if (XmlEmptyNavigator.singleton == null)
				{
					XmlEmptyNavigator.singleton = new XmlEmptyNavigator();
				}
				return XmlEmptyNavigator.singleton;
			}
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06001105 RID: 4357 RVA: 0x0004D35A File Offset: 0x0004C35A
		public override XPathNodeType NodeType
		{
			get
			{
				return XPathNodeType.All;
			}
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06001106 RID: 4358 RVA: 0x0004D35E File Offset: 0x0004C35E
		public override string NamespaceURI
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06001107 RID: 4359 RVA: 0x0004D365 File Offset: 0x0004C365
		public override string LocalName
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06001108 RID: 4360 RVA: 0x0004D36C File Offset: 0x0004C36C
		public override string Name
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x06001109 RID: 4361 RVA: 0x0004D373 File Offset: 0x0004C373
		public override string Prefix
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x0600110A RID: 4362 RVA: 0x0004D37A File Offset: 0x0004C37A
		public override string BaseURI
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x0600110B RID: 4363 RVA: 0x0004D381 File Offset: 0x0004C381
		public override string Value
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x0600110C RID: 4364 RVA: 0x0004D388 File Offset: 0x0004C388
		public override bool IsEmptyElement
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x0600110D RID: 4365 RVA: 0x0004D38B File Offset: 0x0004C38B
		public override string XmlLang
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x0600110E RID: 4366 RVA: 0x0004D392 File Offset: 0x0004C392
		public override bool HasAttributes
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x0600110F RID: 4367 RVA: 0x0004D395 File Offset: 0x0004C395
		public override bool HasChildren
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06001110 RID: 4368 RVA: 0x0004D398 File Offset: 0x0004C398
		public override XmlNameTable NameTable
		{
			get
			{
				return new NameTable();
			}
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x0004D39F File Offset: 0x0004C39F
		public override bool MoveToFirstChild()
		{
			return false;
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x0004D3A2 File Offset: 0x0004C3A2
		public override void MoveToRoot()
		{
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x0004D3A4 File Offset: 0x0004C3A4
		public override bool MoveToNext()
		{
			return false;
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x0004D3A7 File Offset: 0x0004C3A7
		public override bool MoveToPrevious()
		{
			return false;
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x0004D3AA File Offset: 0x0004C3AA
		public override bool MoveToFirst()
		{
			return false;
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x0004D3AD File Offset: 0x0004C3AD
		public override bool MoveToFirstAttribute()
		{
			return false;
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x0004D3B0 File Offset: 0x0004C3B0
		public override bool MoveToNextAttribute()
		{
			return false;
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x0004D3B3 File Offset: 0x0004C3B3
		public override bool MoveToId(string id)
		{
			return false;
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x0004D3B6 File Offset: 0x0004C3B6
		public override string GetAttribute(string localName, string namespaceName)
		{
			return null;
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x0004D3B9 File Offset: 0x0004C3B9
		public override bool MoveToAttribute(string localName, string namespaceName)
		{
			return false;
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x0004D3BC File Offset: 0x0004C3BC
		public override string GetNamespace(string name)
		{
			return null;
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x0004D3BF File Offset: 0x0004C3BF
		public override bool MoveToNamespace(string prefix)
		{
			return false;
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x0004D3C2 File Offset: 0x0004C3C2
		public override bool MoveToFirstNamespace(XPathNamespaceScope scope)
		{
			return false;
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x0004D3C5 File Offset: 0x0004C3C5
		public override bool MoveToNextNamespace(XPathNamespaceScope scope)
		{
			return false;
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x0004D3C8 File Offset: 0x0004C3C8
		public override bool MoveToParent()
		{
			return false;
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x0004D3CB File Offset: 0x0004C3CB
		public override bool MoveTo(XPathNavigator other)
		{
			return this == other;
		}

		// Token: 0x06001121 RID: 4385 RVA: 0x0004D3D1 File Offset: 0x0004C3D1
		public override XmlNodeOrder ComparePosition(XPathNavigator other)
		{
			if (this != other)
			{
				return XmlNodeOrder.Unknown;
			}
			return XmlNodeOrder.Same;
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x0004D3DA File Offset: 0x0004C3DA
		public override bool IsSamePosition(XPathNavigator other)
		{
			return this == other;
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x0004D3E0 File Offset: 0x0004C3E0
		public override XPathNavigator Clone()
		{
			return this;
		}

		// Token: 0x04000B06 RID: 2822
		private static XmlEmptyNavigator singleton;
	}
}
