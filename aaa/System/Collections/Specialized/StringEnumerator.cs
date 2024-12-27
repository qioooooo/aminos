using System;

namespace System.Collections.Specialized
{
	// Token: 0x02000260 RID: 608
	public class StringEnumerator
	{
		// Token: 0x06001520 RID: 5408 RVA: 0x00045C79 File Offset: 0x00044C79
		internal StringEnumerator(StringCollection mappings)
		{
			this.temp = mappings;
			this.baseEnumerator = this.temp.GetEnumerator();
		}

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x06001521 RID: 5409 RVA: 0x00045C99 File Offset: 0x00044C99
		public string Current
		{
			get
			{
				return (string)this.baseEnumerator.Current;
			}
		}

		// Token: 0x06001522 RID: 5410 RVA: 0x00045CAB File Offset: 0x00044CAB
		public bool MoveNext()
		{
			return this.baseEnumerator.MoveNext();
		}

		// Token: 0x06001523 RID: 5411 RVA: 0x00045CB8 File Offset: 0x00044CB8
		public void Reset()
		{
			this.baseEnumerator.Reset();
		}

		// Token: 0x040011B9 RID: 4537
		private IEnumerator baseEnumerator;

		// Token: 0x040011BA RID: 4538
		private IEnumerable temp;
	}
}
