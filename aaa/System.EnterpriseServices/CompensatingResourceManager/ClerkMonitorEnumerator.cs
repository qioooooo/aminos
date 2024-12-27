using System;
using System.Collections;

namespace System.EnterpriseServices.CompensatingResourceManager
{
	// Token: 0x020000B1 RID: 177
	internal class ClerkMonitorEnumerator : IEnumerator
	{
		// Token: 0x0600043C RID: 1084 RVA: 0x0000D4AC File Offset: 0x0000C4AC
		internal ClerkMonitorEnumerator(ClerkMonitor c)
		{
			this._monitor = c;
			this._version = c._version;
			this._endIndex = c.Count;
			this._curElement = null;
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0000D4E4 File Offset: 0x0000C4E4
		public virtual bool MoveNext()
		{
			if (this._version != this._monitor._version)
			{
				throw new InvalidOperationException(Resource.FormatString("InvalidOperation_EnumFailedVersion"));
			}
			if (this._curIndex < this._endIndex)
			{
				this._curIndex++;
			}
			if (this._curIndex < this._endIndex)
			{
				this._curElement = this._monitor[this._curIndex];
				return true;
			}
			this._curElement = null;
			return false;
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600043E RID: 1086 RVA: 0x0000D55F File Offset: 0x0000C55F
		public virtual object Current
		{
			get
			{
				if (this._curIndex < 0)
				{
					throw new InvalidOperationException(Resource.FormatString("InvalidOperation_EnumNotStarted"));
				}
				if (this._curIndex >= this._endIndex)
				{
					throw new InvalidOperationException(Resource.FormatString("InvalidOperation_EnumEnded"));
				}
				return this._curElement;
			}
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0000D59E File Offset: 0x0000C59E
		public virtual void Reset()
		{
			if (this._version != this._monitor._version)
			{
				throw new InvalidOperationException(Resource.FormatString("InvalidOperation_EnumFailedVersion"));
			}
			this._curIndex = -1;
			this._curElement = null;
		}

		// Token: 0x040001E4 RID: 484
		private ClerkMonitor _monitor;

		// Token: 0x040001E5 RID: 485
		private int _version;

		// Token: 0x040001E6 RID: 486
		private int _curIndex = -1;

		// Token: 0x040001E7 RID: 487
		private int _endIndex;

		// Token: 0x040001E8 RID: 488
		private object _curElement;
	}
}
