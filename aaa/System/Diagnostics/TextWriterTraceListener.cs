using System;
using System.IO;
using System.Security.Permissions;
using System.Text;

namespace System.Diagnostics
{
	// Token: 0x020001BB RID: 443
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
	public class TextWriterTraceListener : TraceListener
	{
		// Token: 0x06000DAA RID: 3498 RVA: 0x0002BB80 File Offset: 0x0002AB80
		public TextWriterTraceListener()
		{
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x0002BB88 File Offset: 0x0002AB88
		public TextWriterTraceListener(Stream stream)
			: this(stream, string.Empty)
		{
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x0002BB96 File Offset: 0x0002AB96
		public TextWriterTraceListener(Stream stream, string name)
			: base(name)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this.writer = new StreamWriter(stream);
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x0002BBB9 File Offset: 0x0002ABB9
		public TextWriterTraceListener(TextWriter writer)
			: this(writer, string.Empty)
		{
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x0002BBC7 File Offset: 0x0002ABC7
		public TextWriterTraceListener(TextWriter writer, string name)
			: base(name)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			this.writer = writer;
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x0002BBE5 File Offset: 0x0002ABE5
		public TextWriterTraceListener(string fileName)
		{
			this.fileName = fileName;
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x0002BBF4 File Offset: 0x0002ABF4
		public TextWriterTraceListener(string fileName, string name)
			: base(name)
		{
			this.fileName = fileName;
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000DB1 RID: 3505 RVA: 0x0002BC04 File Offset: 0x0002AC04
		// (set) Token: 0x06000DB2 RID: 3506 RVA: 0x0002BC13 File Offset: 0x0002AC13
		public TextWriter Writer
		{
			get
			{
				this.EnsureWriter();
				return this.writer;
			}
			set
			{
				this.writer = value;
			}
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x0002BC1C File Offset: 0x0002AC1C
		public override void Close()
		{
			if (this.writer != null)
			{
				this.writer.Close();
			}
			this.writer = null;
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x0002BC38 File Offset: 0x0002AC38
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Close();
			}
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x0002BC43 File Offset: 0x0002AC43
		public override void Flush()
		{
			if (!this.EnsureWriter())
			{
				return;
			}
			this.writer.Flush();
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x0002BC59 File Offset: 0x0002AC59
		public override void Write(string message)
		{
			if (!this.EnsureWriter())
			{
				return;
			}
			if (base.NeedIndent)
			{
				this.WriteIndent();
			}
			this.writer.Write(message);
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x0002BC7E File Offset: 0x0002AC7E
		public override void WriteLine(string message)
		{
			if (!this.EnsureWriter())
			{
				return;
			}
			if (base.NeedIndent)
			{
				this.WriteIndent();
			}
			this.writer.WriteLine(message);
			base.NeedIndent = true;
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x0002BCAC File Offset: 0x0002ACAC
		private static Encoding GetEncodingWithFallback(Encoding encoding)
		{
			Encoding encoding2 = (Encoding)encoding.Clone();
			encoding2.EncoderFallback = EncoderFallback.ReplacementFallback;
			encoding2.DecoderFallback = DecoderFallback.ReplacementFallback;
			return encoding2;
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x0002BCDC File Offset: 0x0002ACDC
		internal bool EnsureWriter()
		{
			bool flag = true;
			if (this.writer == null)
			{
				flag = false;
				if (this.fileName == null)
				{
					return flag;
				}
				Encoding encodingWithFallback = TextWriterTraceListener.GetEncodingWithFallback(new UTF8Encoding(false));
				string text = Path.GetFullPath(this.fileName);
				string directoryName = Path.GetDirectoryName(text);
				string text2 = Path.GetFileName(text);
				for (int i = 0; i < 2; i++)
				{
					try
					{
						this.writer = new StreamWriter(text, true, encodingWithFallback, 4096);
						flag = true;
						break;
					}
					catch (IOException)
					{
						text2 = Guid.NewGuid().ToString() + text2;
						text = Path.Combine(directoryName, text2);
					}
					catch (UnauthorizedAccessException)
					{
						break;
					}
					catch (Exception)
					{
						break;
					}
				}
				if (!flag)
				{
					this.fileName = null;
				}
			}
			return flag;
		}

		// Token: 0x04000ED2 RID: 3794
		internal TextWriter writer;

		// Token: 0x04000ED3 RID: 3795
		private string fileName;
	}
}
