using System;
using System.Collections;

namespace System.Xml
{
	// Token: 0x020000CF RID: 207
	internal sealed class XmlChildEnumerator : IEnumerator
	{
		// Token: 0x06000C55 RID: 3157 RVA: 0x00037EE0 File Offset: 0x00036EE0
		internal XmlChildEnumerator(XmlNode container)
		{
			this.container = container;
			this.child = container.FirstChild;
			this.isFirst = true;
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x00037F02 File Offset: 0x00036F02
		bool IEnumerator.MoveNext()
		{
			return this.MoveNext();
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x00037F0C File Offset: 0x00036F0C
		internal bool MoveNext()
		{
			if (this.isFirst)
			{
				this.child = this.container.FirstChild;
				this.isFirst = false;
			}
			else if (this.child != null)
			{
				this.child = this.child.NextSibling;
			}
			return this.child != null;
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x00037F60 File Offset: 0x00036F60
		void IEnumerator.Reset()
		{
			this.isFirst = true;
			this.child = this.container.FirstChild;
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000C59 RID: 3161 RVA: 0x00037F7A File Offset: 0x00036F7A
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000C5A RID: 3162 RVA: 0x00037F82 File Offset: 0x00036F82
		internal XmlNode Current
		{
			get
			{
				if (this.isFirst || this.child == null)
				{
					throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
				}
				return this.child;
			}
		}

		// Token: 0x040008F2 RID: 2290
		internal XmlNode container;

		// Token: 0x040008F3 RID: 2291
		internal XmlNode child;

		// Token: 0x040008F4 RID: 2292
		internal bool isFirst;
	}
}
