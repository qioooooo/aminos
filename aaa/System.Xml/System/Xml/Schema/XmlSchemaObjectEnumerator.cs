using System;
using System.Collections;

namespace System.Xml.Schema
{
	// Token: 0x02000269 RID: 617
	public class XmlSchemaObjectEnumerator : IEnumerator
	{
		// Token: 0x06001CBB RID: 7355 RVA: 0x0008362E File Offset: 0x0008262E
		internal XmlSchemaObjectEnumerator(IEnumerator enumerator)
		{
			this.enumerator = enumerator;
		}

		// Token: 0x06001CBC RID: 7356 RVA: 0x0008363D File Offset: 0x0008263D
		public void Reset()
		{
			this.enumerator.Reset();
		}

		// Token: 0x06001CBD RID: 7357 RVA: 0x0008364A File Offset: 0x0008264A
		public bool MoveNext()
		{
			return this.enumerator.MoveNext();
		}

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x06001CBE RID: 7358 RVA: 0x00083657 File Offset: 0x00082657
		public XmlSchemaObject Current
		{
			get
			{
				return (XmlSchemaObject)this.enumerator.Current;
			}
		}

		// Token: 0x06001CBF RID: 7359 RVA: 0x00083669 File Offset: 0x00082669
		void IEnumerator.Reset()
		{
			this.enumerator.Reset();
		}

		// Token: 0x06001CC0 RID: 7360 RVA: 0x00083676 File Offset: 0x00082676
		bool IEnumerator.MoveNext()
		{
			return this.enumerator.MoveNext();
		}

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x06001CC1 RID: 7361 RVA: 0x00083683 File Offset: 0x00082683
		object IEnumerator.Current
		{
			get
			{
				return this.enumerator.Current;
			}
		}

		// Token: 0x040011A2 RID: 4514
		private IEnumerator enumerator;
	}
}
