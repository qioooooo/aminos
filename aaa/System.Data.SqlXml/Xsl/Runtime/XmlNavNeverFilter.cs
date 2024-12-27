using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000AF RID: 175
	internal class XmlNavNeverFilter : XmlNavigatorFilter
	{
		// Token: 0x0600081A RID: 2074 RVA: 0x0002886C File Offset: 0x0002786C
		public static XmlNavigatorFilter Create()
		{
			return XmlNavNeverFilter.Singleton;
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x00028873 File Offset: 0x00027873
		private XmlNavNeverFilter()
		{
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x0002887B File Offset: 0x0002787B
		public override bool MoveToContent(XPathNavigator navigator)
		{
			return XmlNavNeverFilter.MoveToFirstAttributeContent(navigator);
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x00028883 File Offset: 0x00027883
		public override bool MoveToNextContent(XPathNavigator navigator)
		{
			return XmlNavNeverFilter.MoveToNextAttributeContent(navigator);
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x0002888B File Offset: 0x0002788B
		public override bool MoveToFollowingSibling(XPathNavigator navigator)
		{
			return navigator.MoveToNext();
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x00028893 File Offset: 0x00027893
		public override bool MoveToPreviousSibling(XPathNavigator navigator)
		{
			return navigator.MoveToPrevious();
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x0002889B File Offset: 0x0002789B
		public override bool MoveToFollowing(XPathNavigator navigator, XPathNavigator navEnd)
		{
			return navigator.MoveToFollowing(XPathNodeType.All, navEnd);
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x000288A6 File Offset: 0x000278A6
		public override bool IsFiltered(XPathNavigator navigator)
		{
			return false;
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x000288A9 File Offset: 0x000278A9
		public static bool MoveToFirstAttributeContent(XPathNavigator navigator)
		{
			return navigator.MoveToFirstAttribute() || navigator.MoveToFirstChild();
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x000288BB File Offset: 0x000278BB
		public static bool MoveToNextAttributeContent(XPathNavigator navigator)
		{
			if (navigator.NodeType == XPathNodeType.Attribute)
			{
				if (!navigator.MoveToNextAttribute())
				{
					navigator.MoveToParent();
					if (!navigator.MoveToFirstChild())
					{
						navigator.MoveToFirstAttribute();
						while (navigator.MoveToNextAttribute())
						{
						}
						return false;
					}
				}
				return true;
			}
			return navigator.MoveToNext();
		}

		// Token: 0x0400056F RID: 1391
		private static XmlNavigatorFilter Singleton = new XmlNavNeverFilter();
	}
}
