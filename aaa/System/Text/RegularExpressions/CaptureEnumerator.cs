using System;
using System.Collections;

namespace System.Text.RegularExpressions
{
	// Token: 0x02000012 RID: 18
	[Serializable]
	internal class CaptureEnumerator : IEnumerator
	{
		// Token: 0x06000087 RID: 135 RVA: 0x00003993 File Offset: 0x00002993
		internal CaptureEnumerator(CaptureCollection rcc)
		{
			this._curindex = -1;
			this._rcc = rcc;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000039AC File Offset: 0x000029AC
		public bool MoveNext()
		{
			int count = this._rcc.Count;
			if (this._curindex >= count)
			{
				return false;
			}
			this._curindex++;
			return this._curindex < count;
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000089 RID: 137 RVA: 0x000039E7 File Offset: 0x000029E7
		public object Current
		{
			get
			{
				return this.Capture;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600008A RID: 138 RVA: 0x000039EF File Offset: 0x000029EF
		public Capture Capture
		{
			get
			{
				if (this._curindex < 0 || this._curindex >= this._rcc.Count)
				{
					throw new InvalidOperationException(SR.GetString("EnumNotStarted"));
				}
				return this._rcc[this._curindex];
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003A2E File Offset: 0x00002A2E
		public void Reset()
		{
			this._curindex = -1;
		}

		// Token: 0x04000652 RID: 1618
		internal CaptureCollection _rcc;

		// Token: 0x04000653 RID: 1619
		internal int _curindex;
	}
}
