using System;
using System.Collections;
using System.Diagnostics;
using System.Xml;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000165 RID: 357
	[DebuggerDisplay("Position={CurrentPosition}, Current={debuggerDisplayProxy, nq}")]
	internal class XPathArrayIterator : ResetableIterator
	{
		// Token: 0x06001339 RID: 4921 RVA: 0x000534F8 File Offset: 0x000524F8
		public XPathArrayIterator(IList list)
		{
			this.list = list;
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x00053507 File Offset: 0x00052507
		public XPathArrayIterator(XPathArrayIterator it)
		{
			this.list = it.list;
			this.index = it.index;
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x00053527 File Offset: 0x00052527
		public XPathArrayIterator(XPathNodeIterator nodeIterator)
		{
			this.list = new ArrayList();
			while (nodeIterator.MoveNext())
			{
				XPathNavigator xpathNavigator = nodeIterator.Current;
				this.list.Add(xpathNavigator.Clone());
			}
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x0600133C RID: 4924 RVA: 0x0005355B File Offset: 0x0005255B
		public IList AsList
		{
			get
			{
				return this.list;
			}
		}

		// Token: 0x0600133D RID: 4925 RVA: 0x00053563 File Offset: 0x00052563
		public override XPathNodeIterator Clone()
		{
			return new XPathArrayIterator(this);
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x0600133E RID: 4926 RVA: 0x0005356C File Offset: 0x0005256C
		public override XPathNavigator Current
		{
			get
			{
				if (this.index < 1)
				{
					throw new InvalidOperationException(Res.GetString("Sch_EnumNotStarted", new object[] { string.Empty }));
				}
				return (XPathNavigator)this.list[this.index - 1];
			}
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x0600133F RID: 4927 RVA: 0x000535BA File Offset: 0x000525BA
		public override int CurrentPosition
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06001340 RID: 4928 RVA: 0x000535C2 File Offset: 0x000525C2
		public override int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x000535CF File Offset: 0x000525CF
		public override bool MoveNext()
		{
			if (this.index == this.list.Count)
			{
				return false;
			}
			this.index++;
			return true;
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x000535F5 File Offset: 0x000525F5
		public override void Reset()
		{
			this.index = 0;
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x000535FE File Offset: 0x000525FE
		public override IEnumerator GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06001344 RID: 4932 RVA: 0x0005360B File Offset: 0x0005260B
		private object debuggerDisplayProxy
		{
			get
			{
				if (this.index >= 1)
				{
					return new XPathNavigator.DebuggerDisplayProxy(this.Current);
				}
				return null;
			}
		}

		// Token: 0x04000BEC RID: 3052
		protected IList list;

		// Token: 0x04000BED RID: 3053
		protected int index;
	}
}
