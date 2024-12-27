using System;
using System.Collections;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x02000006 RID: 6
	internal class BaseTransportHeadersEnumerator : IEnumerator
	{
		// Token: 0x06000010 RID: 16 RVA: 0x000022EF File Offset: 0x000012EF
		public BaseTransportHeadersEnumerator(BaseTransportHeaders headers)
		{
			this._headers = headers;
			this.Reset();
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002304 File Offset: 0x00001304
		public bool MoveNext()
		{
			if (this._currentIndex != -1)
			{
				if (this._bStarted)
				{
					this._currentIndex++;
				}
				else
				{
					this._bStarted = true;
				}
				while (this._currentIndex != -1)
				{
					if (this._currentIndex >= 4)
					{
						this._otherHeadersEnumerator = this._headers.GetOtherHeadersEnumerator();
						this._currentIndex = -1;
					}
					else
					{
						if (this._headers.GetValueFromHeaderIndex(this._currentIndex) != null)
						{
							return true;
						}
						this._currentIndex++;
					}
				}
			}
			if (this._otherHeadersEnumerator == null)
			{
				return false;
			}
			if (!this._otherHeadersEnumerator.MoveNext())
			{
				this._otherHeadersEnumerator = null;
				return false;
			}
			return true;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000023AB File Offset: 0x000013AB
		public void Reset()
		{
			this._bStarted = false;
			this._currentIndex = 0;
			this._otherHeadersEnumerator = null;
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000013 RID: 19 RVA: 0x000023C4 File Offset: 0x000013C4
		public object Current
		{
			get
			{
				if (!this._bStarted)
				{
					return null;
				}
				if (this._currentIndex != -1)
				{
					return new DictionaryEntry(this._headers.MapHeaderIndexToName(this._currentIndex), this._headers.GetValueFromHeaderIndex(this._currentIndex));
				}
				if (this._otherHeadersEnumerator != null)
				{
					return this._otherHeadersEnumerator.Current;
				}
				return null;
			}
		}

		// Token: 0x04000036 RID: 54
		private BaseTransportHeaders _headers;

		// Token: 0x04000037 RID: 55
		private bool _bStarted;

		// Token: 0x04000038 RID: 56
		private int _currentIndex;

		// Token: 0x04000039 RID: 57
		private IEnumerator _otherHeadersEnumerator;
	}
}
