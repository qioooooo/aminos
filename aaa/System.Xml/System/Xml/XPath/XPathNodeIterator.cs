using System;
using System.Collections;
using System.Diagnostics;

namespace System.Xml.XPath
{
	// Token: 0x020000BD RID: 189
	[DebuggerDisplay("Position={CurrentPosition}, Current={debuggerDisplayProxy}")]
	public abstract class XPathNodeIterator : ICloneable, IEnumerable
	{
		// Token: 0x06000B52 RID: 2898 RVA: 0x00034C34 File Offset: 0x00033C34
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		// Token: 0x06000B53 RID: 2899
		public abstract XPathNodeIterator Clone();

		// Token: 0x06000B54 RID: 2900
		public abstract bool MoveNext();

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000B55 RID: 2901
		public abstract XPathNavigator Current { get; }

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000B56 RID: 2902
		public abstract int CurrentPosition { get; }

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000B57 RID: 2903 RVA: 0x00034C3C File Offset: 0x00033C3C
		public virtual int Count
		{
			get
			{
				if (this.count == -1)
				{
					XPathNodeIterator xpathNodeIterator = this.Clone();
					while (xpathNodeIterator.MoveNext())
					{
					}
					this.count = xpathNodeIterator.CurrentPosition;
				}
				return this.count;
			}
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x00034C73 File Offset: 0x00033C73
		public virtual IEnumerator GetEnumerator()
		{
			return new XPathNodeIterator.Enumerator(this);
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000B59 RID: 2905 RVA: 0x00034C7B File Offset: 0x00033C7B
		private object debuggerDisplayProxy
		{
			get
			{
				if (this.Current != null)
				{
					return new XPathNavigator.DebuggerDisplayProxy(this.Current);
				}
				return null;
			}
		}

		// Token: 0x040008DA RID: 2266
		internal int count = -1;

		// Token: 0x020000BE RID: 190
		private class Enumerator : IEnumerator
		{
			// Token: 0x06000B5B RID: 2907 RVA: 0x00034CA6 File Offset: 0x00033CA6
			public Enumerator(XPathNodeIterator original)
			{
				this.original = original.Clone();
			}

			// Token: 0x17000277 RID: 631
			// (get) Token: 0x06000B5C RID: 2908 RVA: 0x00034CBC File Offset: 0x00033CBC
			public virtual object Current
			{
				get
				{
					if (!this.iterationStarted)
					{
						throw new InvalidOperationException(Res.GetString("Sch_EnumNotStarted", new object[] { string.Empty }));
					}
					if (this.current == null)
					{
						throw new InvalidOperationException(Res.GetString("Sch_EnumFinished", new object[] { string.Empty }));
					}
					return this.current.Current.Clone();
				}
			}

			// Token: 0x06000B5D RID: 2909 RVA: 0x00034D2C File Offset: 0x00033D2C
			public virtual bool MoveNext()
			{
				if (!this.iterationStarted)
				{
					this.current = this.original.Clone();
					this.iterationStarted = true;
				}
				if (this.current == null || !this.current.MoveNext())
				{
					this.current = null;
					return false;
				}
				return true;
			}

			// Token: 0x06000B5E RID: 2910 RVA: 0x00034D78 File Offset: 0x00033D78
			public virtual void Reset()
			{
				this.iterationStarted = false;
			}

			// Token: 0x040008DB RID: 2267
			private XPathNodeIterator original;

			// Token: 0x040008DC RID: 2268
			private XPathNodeIterator current;

			// Token: 0x040008DD RID: 2269
			private bool iterationStarted;
		}
	}
}
