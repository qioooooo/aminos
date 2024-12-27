using System;

namespace System.Text.RegularExpressions
{
	// Token: 0x02000010 RID: 16
	[Serializable]
	public class Capture
	{
		// Token: 0x06000076 RID: 118 RVA: 0x000037C8 File Offset: 0x000027C8
		internal Capture(string text, int i, int l)
		{
			this._text = text;
			this._index = i;
			this._length = l;
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000077 RID: 119 RVA: 0x000037E5 File Offset: 0x000027E5
		public int Index
		{
			get
			{
				return this._index;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000078 RID: 120 RVA: 0x000037ED File Offset: 0x000027ED
		public int Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000079 RID: 121 RVA: 0x000037F5 File Offset: 0x000027F5
		public string Value
		{
			get
			{
				return this._text.Substring(this._index, this._length);
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0000380E File Offset: 0x0000280E
		public override string ToString()
		{
			return this.Value;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003816 File Offset: 0x00002816
		internal string GetOriginalString()
		{
			return this._text;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x0000381E File Offset: 0x0000281E
		internal string GetLeftSubstring()
		{
			return this._text.Substring(0, this._index);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003832 File Offset: 0x00002832
		internal string GetRightSubstring()
		{
			return this._text.Substring(this._index + this._length, this._text.Length - this._index - this._length);
		}

		// Token: 0x0400064C RID: 1612
		internal string _text;

		// Token: 0x0400064D RID: 1613
		internal int _index;

		// Token: 0x0400064E RID: 1614
		internal int _length;
	}
}
