using System;
using System.Collections;

namespace System.Text.RegularExpressions
{
	// Token: 0x02000011 RID: 17
	[Serializable]
	public class CaptureCollection : ICollection, IEnumerable
	{
		// Token: 0x0600007E RID: 126 RVA: 0x00003865 File Offset: 0x00002865
		internal CaptureCollection(Group group)
		{
			this._group = group;
			this._capcount = this._group._capcount;
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00003885 File Offset: 0x00002885
		public object SyncRoot
		{
			get
			{
				return this._group;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000080 RID: 128 RVA: 0x0000388D File Offset: 0x0000288D
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00003890 File Offset: 0x00002890
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00003893 File Offset: 0x00002893
		public int Count
		{
			get
			{
				return this._capcount;
			}
		}

		// Token: 0x17000020 RID: 32
		public Capture this[int i]
		{
			get
			{
				return this.GetCapture(i);
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000038A4 File Offset: 0x000028A4
		public void CopyTo(Array array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			int num = arrayIndex;
			for (int i = 0; i < this.Count; i++)
			{
				array.SetValue(this[i], num);
				num++;
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000038E4 File Offset: 0x000028E4
		public IEnumerator GetEnumerator()
		{
			return new CaptureEnumerator(this);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000038EC File Offset: 0x000028EC
		internal Capture GetCapture(int i)
		{
			if (i == this._capcount - 1 && i >= 0)
			{
				return this._group;
			}
			if (i >= this._capcount || i < 0)
			{
				throw new ArgumentOutOfRangeException("i");
			}
			if (this._captures == null)
			{
				this._captures = new Capture[this._capcount];
				for (int j = 0; j < this._capcount - 1; j++)
				{
					this._captures[j] = new Capture(this._group._text, this._group._caps[j * 2], this._group._caps[j * 2 + 1]);
				}
			}
			return this._captures[i];
		}

		// Token: 0x0400064F RID: 1615
		internal Group _group;

		// Token: 0x04000650 RID: 1616
		internal int _capcount;

		// Token: 0x04000651 RID: 1617
		internal Capture[] _captures;
	}
}
