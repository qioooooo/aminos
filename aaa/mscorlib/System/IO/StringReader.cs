using System;
using System.Runtime.InteropServices;

namespace System.IO
{
	// Token: 0x020005B9 RID: 1465
	[ComVisible(true)]
	[Serializable]
	public class StringReader : TextReader
	{
		// Token: 0x06003737 RID: 14135 RVA: 0x000BB034 File Offset: 0x000BA034
		public StringReader(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			this._s = s;
			this._length = ((s == null) ? 0 : s.Length);
		}

		// Token: 0x06003738 RID: 14136 RVA: 0x000BB063 File Offset: 0x000BA063
		public override void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x06003739 RID: 14137 RVA: 0x000BB06C File Offset: 0x000BA06C
		protected override void Dispose(bool disposing)
		{
			this._s = null;
			this._pos = 0;
			this._length = 0;
			base.Dispose(disposing);
		}

		// Token: 0x0600373A RID: 14138 RVA: 0x000BB08A File Offset: 0x000BA08A
		public override int Peek()
		{
			if (this._s == null)
			{
				__Error.ReaderClosed();
			}
			if (this._pos == this._length)
			{
				return -1;
			}
			return (int)this._s[this._pos];
		}

		// Token: 0x0600373B RID: 14139 RVA: 0x000BB0BC File Offset: 0x000BA0BC
		public override int Read()
		{
			if (this._s == null)
			{
				__Error.ReaderClosed();
			}
			if (this._pos == this._length)
			{
				return -1;
			}
			return (int)this._s[this._pos++];
		}

		// Token: 0x0600373C RID: 14140 RVA: 0x000BB104 File Offset: 0x000BA104
		public override int Read([In] [Out] char[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", Environment.GetResourceString("ArgumentNull_Buffer"));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			if (this._s == null)
			{
				__Error.ReaderClosed();
			}
			int num = this._length - this._pos;
			if (num > 0)
			{
				if (num > count)
				{
					num = count;
				}
				this._s.CopyTo(this._pos, buffer, index, num);
				this._pos += num;
			}
			return num;
		}

		// Token: 0x0600373D RID: 14141 RVA: 0x000BB1BC File Offset: 0x000BA1BC
		public override string ReadToEnd()
		{
			if (this._s == null)
			{
				__Error.ReaderClosed();
			}
			string text;
			if (this._pos == 0)
			{
				text = this._s;
			}
			else
			{
				text = this._s.Substring(this._pos, this._length - this._pos);
			}
			this._pos = this._length;
			return text;
		}

		// Token: 0x0600373E RID: 14142 RVA: 0x000BB214 File Offset: 0x000BA214
		public override string ReadLine()
		{
			if (this._s == null)
			{
				__Error.ReaderClosed();
			}
			int i;
			for (i = this._pos; i < this._length; i++)
			{
				char c = this._s[i];
				if (c == '\r' || c == '\n')
				{
					string text = this._s.Substring(this._pos, i - this._pos);
					this._pos = i + 1;
					if (c == '\r' && this._pos < this._length && this._s[this._pos] == '\n')
					{
						this._pos++;
					}
					return text;
				}
			}
			if (i > this._pos)
			{
				string text2 = this._s.Substring(this._pos, i - this._pos);
				this._pos = i;
				return text2;
			}
			return null;
		}

		// Token: 0x04001C88 RID: 7304
		private string _s;

		// Token: 0x04001C89 RID: 7305
		private int _pos;

		// Token: 0x04001C8A RID: 7306
		private int _length;
	}
}
