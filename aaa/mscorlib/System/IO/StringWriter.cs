using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace System.IO
{
	// Token: 0x020005BA RID: 1466
	[ComVisible(true)]
	[Serializable]
	public class StringWriter : TextWriter
	{
		// Token: 0x0600373F RID: 14143 RVA: 0x000BB2E3 File Offset: 0x000BA2E3
		public StringWriter()
			: this(new StringBuilder(), CultureInfo.CurrentCulture)
		{
		}

		// Token: 0x06003740 RID: 14144 RVA: 0x000BB2F5 File Offset: 0x000BA2F5
		public StringWriter(IFormatProvider formatProvider)
			: this(new StringBuilder(), formatProvider)
		{
		}

		// Token: 0x06003741 RID: 14145 RVA: 0x000BB303 File Offset: 0x000BA303
		public StringWriter(StringBuilder sb)
			: this(sb, CultureInfo.CurrentCulture)
		{
		}

		// Token: 0x06003742 RID: 14146 RVA: 0x000BB311 File Offset: 0x000BA311
		public StringWriter(StringBuilder sb, IFormatProvider formatProvider)
			: base(formatProvider)
		{
			if (sb == null)
			{
				throw new ArgumentNullException("sb", Environment.GetResourceString("ArgumentNull_Buffer"));
			}
			this._sb = sb;
			this._isOpen = true;
		}

		// Token: 0x06003743 RID: 14147 RVA: 0x000BB340 File Offset: 0x000BA340
		public override void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x06003744 RID: 14148 RVA: 0x000BB349 File Offset: 0x000BA349
		protected override void Dispose(bool disposing)
		{
			this._isOpen = false;
			base.Dispose(disposing);
		}

		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x06003745 RID: 14149 RVA: 0x000BB359 File Offset: 0x000BA359
		public override Encoding Encoding
		{
			get
			{
				if (StringWriter.m_encoding == null)
				{
					StringWriter.m_encoding = new UnicodeEncoding(false, false);
				}
				return StringWriter.m_encoding;
			}
		}

		// Token: 0x06003746 RID: 14150 RVA: 0x000BB373 File Offset: 0x000BA373
		public virtual StringBuilder GetStringBuilder()
		{
			return this._sb;
		}

		// Token: 0x06003747 RID: 14151 RVA: 0x000BB37B File Offset: 0x000BA37B
		public override void Write(char value)
		{
			if (!this._isOpen)
			{
				__Error.WriterClosed();
			}
			this._sb.Append(value);
		}

		// Token: 0x06003748 RID: 14152 RVA: 0x000BB398 File Offset: 0x000BA398
		public override void Write(char[] buffer, int index, int count)
		{
			if (!this._isOpen)
			{
				__Error.WriterClosed();
			}
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
			this._sb.Append(buffer, index, count);
		}

		// Token: 0x06003749 RID: 14153 RVA: 0x000BB423 File Offset: 0x000BA423
		public override void Write(string value)
		{
			if (!this._isOpen)
			{
				__Error.WriterClosed();
			}
			if (value != null)
			{
				this._sb.Append(value);
			}
		}

		// Token: 0x0600374A RID: 14154 RVA: 0x000BB442 File Offset: 0x000BA442
		public override string ToString()
		{
			return this._sb.ToString();
		}

		// Token: 0x04001C8B RID: 7307
		private static UnicodeEncoding m_encoding;

		// Token: 0x04001C8C RID: 7308
		private StringBuilder _sb;

		// Token: 0x04001C8D RID: 7309
		private bool _isOpen;
	}
}
