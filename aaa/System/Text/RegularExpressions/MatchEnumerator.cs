using System;
using System.Collections;

namespace System.Text.RegularExpressions
{
	// Token: 0x02000028 RID: 40
	[Serializable]
	internal class MatchEnumerator : IEnumerator
	{
		// Token: 0x060001C2 RID: 450 RVA: 0x0000DA46 File Offset: 0x0000CA46
		internal MatchEnumerator(MatchCollection matchcoll)
		{
			this._matchcoll = matchcoll;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000DA58 File Offset: 0x0000CA58
		public bool MoveNext()
		{
			if (this._done)
			{
				return false;
			}
			this._match = this._matchcoll.GetMatch(this._curindex++);
			if (this._match == null)
			{
				this._done = true;
				return false;
			}
			return true;
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x0000DAA3 File Offset: 0x0000CAA3
		public object Current
		{
			get
			{
				if (this._match == null)
				{
					throw new InvalidOperationException(SR.GetString("EnumNotStarted"));
				}
				return this._match;
			}
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000DAC3 File Offset: 0x0000CAC3
		public void Reset()
		{
			this._curindex = 0;
			this._done = false;
			this._match = null;
		}

		// Token: 0x0400075F RID: 1887
		internal MatchCollection _matchcoll;

		// Token: 0x04000760 RID: 1888
		internal Match _match;

		// Token: 0x04000761 RID: 1889
		internal int _curindex;

		// Token: 0x04000762 RID: 1890
		internal bool _done;
	}
}
