using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000B0 RID: 176
	internal struct XmlNavigatorStack
	{
		// Token: 0x06000825 RID: 2085 RVA: 0x00028904 File Offset: 0x00027904
		public void Push(XPathNavigator nav)
		{
			if (this.stkNav == null)
			{
				this.stkNav = new XPathNavigator[8];
			}
			else if (this.sp >= this.stkNav.Length)
			{
				XPathNavigator[] array = this.stkNav;
				this.stkNav = new XPathNavigator[2 * this.sp];
				Array.Copy(array, this.stkNav, this.sp);
			}
			this.stkNav[this.sp++] = nav;
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x0002897C File Offset: 0x0002797C
		public XPathNavigator Pop()
		{
			return this.stkNav[--this.sp];
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x000289A1 File Offset: 0x000279A1
		public XPathNavigator Peek()
		{
			return this.stkNav[this.sp - 1];
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x000289B2 File Offset: 0x000279B2
		public void Reset()
		{
			this.sp = 0;
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000829 RID: 2089 RVA: 0x000289BB File Offset: 0x000279BB
		public bool IsEmpty
		{
			get
			{
				return this.sp == 0;
			}
		}

		// Token: 0x04000570 RID: 1392
		private const int InitialStackSize = 8;

		// Token: 0x04000571 RID: 1393
		private XPathNavigator[] stkNav;

		// Token: 0x04000572 RID: 1394
		private int sp;
	}
}
