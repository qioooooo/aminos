using System;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x02000096 RID: 150
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct XPathPrecedingDocOrderIterator
	{
		// Token: 0x0600075F RID: 1887 RVA: 0x00026512 File Offset: 0x00025512
		public void Create(XPathNavigator input, XmlNavigatorFilter filter)
		{
			this.navCurrent = XmlQueryRuntime.SyncToNavigator(this.navCurrent, input);
			this.filter = filter;
			this.PushAncestors();
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x00026534 File Offset: 0x00025534
		public bool MoveNext()
		{
			if (!this.navStack.IsEmpty)
			{
				while (!this.filter.MoveToFollowing(this.navCurrent, this.navStack.Peek()))
				{
					this.navCurrent.MoveTo(this.navStack.Pop());
					if (this.navStack.IsEmpty)
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000761 RID: 1889 RVA: 0x00026593 File Offset: 0x00025593
		public XPathNavigator Current
		{
			get
			{
				return this.navCurrent;
			}
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x0002659B File Offset: 0x0002559B
		private void PushAncestors()
		{
			this.navStack.Reset();
			do
			{
				this.navStack.Push(this.navCurrent.Clone());
			}
			while (this.navCurrent.MoveToParent());
			this.navStack.Pop();
		}

		// Token: 0x040004F8 RID: 1272
		private XmlNavigatorFilter filter;

		// Token: 0x040004F9 RID: 1273
		private XPathNavigator navCurrent;

		// Token: 0x040004FA RID: 1274
		private XmlNavigatorStack navStack;
	}
}
