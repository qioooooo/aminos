using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Globalization
{
	// Token: 0x020003B3 RID: 947
	[ComVisible(true)]
	[Serializable]
	public class TextElementEnumerator : IEnumerator
	{
		// Token: 0x06002718 RID: 10008 RVA: 0x00075FA6 File Offset: 0x00074FA6
		internal TextElementEnumerator(string str, int startIndex, int strLen)
		{
			this.str = str;
			this.startIndex = startIndex;
			this.strLen = strLen;
			this.Reset();
		}

		// Token: 0x06002719 RID: 10009 RVA: 0x00075FC9 File Offset: 0x00074FC9
		[OnDeserializing]
		private void OnDeserializing(StreamingContext ctx)
		{
			this.charLen = -1;
		}

		// Token: 0x0600271A RID: 10010 RVA: 0x00075FD4 File Offset: 0x00074FD4
		[OnDeserialized]
		private void OnDeserialized(StreamingContext ctx)
		{
			this.strLen = this.endIndex + 1;
			this.currTextElementLen = this.nextTextElementLen;
			if (this.charLen == -1)
			{
				this.uc = CharUnicodeInfo.InternalGetUnicodeCategory(this.str, this.index, out this.charLen);
			}
		}

		// Token: 0x0600271B RID: 10011 RVA: 0x00076021 File Offset: 0x00075021
		[OnSerializing]
		private void OnSerializing(StreamingContext ctx)
		{
			this.endIndex = this.strLen - 1;
			this.nextTextElementLen = this.currTextElementLen;
		}

		// Token: 0x0600271C RID: 10012 RVA: 0x00076040 File Offset: 0x00075040
		public bool MoveNext()
		{
			if (this.index >= this.strLen)
			{
				this.index = this.strLen + 1;
				return false;
			}
			this.currTextElementLen = StringInfo.GetCurrentTextElementLen(this.str, this.index, this.strLen, ref this.uc, ref this.charLen);
			this.index += this.currTextElementLen;
			return true;
		}

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x0600271D RID: 10013 RVA: 0x000760A8 File Offset: 0x000750A8
		public object Current
		{
			get
			{
				return this.GetTextElement();
			}
		}

		// Token: 0x0600271E RID: 10014 RVA: 0x000760B0 File Offset: 0x000750B0
		public string GetTextElement()
		{
			if (this.index == this.startIndex)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
			}
			if (this.index > this.strLen)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumEnded"));
			}
			return this.str.Substring(this.index - this.currTextElementLen, this.currTextElementLen);
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x0600271F RID: 10015 RVA: 0x00076117 File Offset: 0x00075117
		public int ElementIndex
		{
			get
			{
				if (this.index == this.startIndex)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
				}
				return this.index - this.currTextElementLen;
			}
		}

		// Token: 0x06002720 RID: 10016 RVA: 0x00076144 File Offset: 0x00075144
		public void Reset()
		{
			this.index = this.startIndex;
			if (this.index < this.strLen)
			{
				this.uc = CharUnicodeInfo.InternalGetUnicodeCategory(this.str, this.index, out this.charLen);
			}
		}

		// Token: 0x040011A0 RID: 4512
		private string str;

		// Token: 0x040011A1 RID: 4513
		private int index;

		// Token: 0x040011A2 RID: 4514
		private int startIndex;

		// Token: 0x040011A3 RID: 4515
		[NonSerialized]
		private int strLen;

		// Token: 0x040011A4 RID: 4516
		[NonSerialized]
		private int currTextElementLen;

		// Token: 0x040011A5 RID: 4517
		[OptionalField(VersionAdded = 2)]
		private UnicodeCategory uc;

		// Token: 0x040011A6 RID: 4518
		[OptionalField(VersionAdded = 2)]
		private int charLen;

		// Token: 0x040011A7 RID: 4519
		private int endIndex;

		// Token: 0x040011A8 RID: 4520
		private int nextTextElementLen;
	}
}
