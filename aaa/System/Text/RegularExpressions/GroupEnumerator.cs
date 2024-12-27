using System;
using System.Collections;

namespace System.Text.RegularExpressions
{
	// Token: 0x02000022 RID: 34
	internal class GroupEnumerator : IEnumerator
	{
		// Token: 0x0600015F RID: 351 RVA: 0x0000B488 File Offset: 0x0000A488
		internal GroupEnumerator(GroupCollection rgc)
		{
			this._curindex = -1;
			this._rgc = rgc;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000B4A0 File Offset: 0x0000A4A0
		public bool MoveNext()
		{
			int count = this._rgc.Count;
			if (this._curindex >= count)
			{
				return false;
			}
			this._curindex++;
			return this._curindex < count;
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000161 RID: 353 RVA: 0x0000B4DB File Offset: 0x0000A4DB
		public object Current
		{
			get
			{
				return this.Capture;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000162 RID: 354 RVA: 0x0000B4E3 File Offset: 0x0000A4E3
		public Capture Capture
		{
			get
			{
				if (this._curindex < 0 || this._curindex >= this._rgc.Count)
				{
					throw new InvalidOperationException(SR.GetString("EnumNotStarted"));
				}
				return this._rgc[this._curindex];
			}
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0000B522 File Offset: 0x0000A522
		public void Reset()
		{
			this._curindex = -1;
		}

		// Token: 0x04000730 RID: 1840
		internal GroupCollection _rgc;

		// Token: 0x04000731 RID: 1841
		internal int _curindex;
	}
}
