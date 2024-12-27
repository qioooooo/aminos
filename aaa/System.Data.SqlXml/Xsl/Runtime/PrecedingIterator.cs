using System;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x02000094 RID: 148
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct PrecedingIterator
	{
		// Token: 0x06000759 RID: 1881 RVA: 0x000263E0 File Offset: 0x000253E0
		public void Create(XPathNavigator context, XmlNavigatorFilter filter)
		{
			this.navCurrent = XmlQueryRuntime.SyncToNavigator(this.navCurrent, context);
			this.navCurrent.MoveToRoot();
			this.stack.Reset();
			if (!this.navCurrent.IsSamePosition(context))
			{
				if (!filter.IsFiltered(this.navCurrent))
				{
					this.stack.Push(this.navCurrent.Clone());
				}
				while (filter.MoveToFollowing(this.navCurrent, context))
				{
					this.stack.Push(this.navCurrent.Clone());
				}
			}
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x0002646E File Offset: 0x0002546E
		public bool MoveNext()
		{
			if (this.stack.IsEmpty)
			{
				return false;
			}
			this.navCurrent = this.stack.Pop();
			return true;
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x0600075B RID: 1883 RVA: 0x00026491 File Offset: 0x00025491
		public XPathNavigator Current
		{
			get
			{
				return this.navCurrent;
			}
		}

		// Token: 0x040004F4 RID: 1268
		private XmlNavigatorStack stack;

		// Token: 0x040004F5 RID: 1269
		private XPathNavigator navCurrent;
	}
}
