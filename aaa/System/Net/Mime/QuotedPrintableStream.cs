using System;
using System.IO;

namespace System.Net.Mime
{
	// Token: 0x020006B2 RID: 1714
	internal class QuotedPrintableStream : DelegatedStream
	{
		// Token: 0x060034F5 RID: 13557 RVA: 0x000E0E49 File Offset: 0x000DFE49
		internal QuotedPrintableStream(Stream stream, int lineLength)
			: base(stream)
		{
			if (lineLength < 0)
			{
				throw new ArgumentOutOfRangeException("lineLength");
			}
			this.lineLength = lineLength;
		}

		// Token: 0x060034F6 RID: 13558 RVA: 0x000E0E68 File Offset: 0x000DFE68
		internal QuotedPrintableStream(Stream stream, bool encodeCRLF)
			: this(stream, QuotedPrintableStream.DefaultLineLength)
		{
			this.encodeCRLF = encodeCRLF;
		}

		// Token: 0x060034F7 RID: 13559 RVA: 0x000E0E7D File Offset: 0x000DFE7D
		internal QuotedPrintableStream()
		{
			this.lineLength = QuotedPrintableStream.DefaultLineLength;
		}

		// Token: 0x060034F8 RID: 13560 RVA: 0x000E0E90 File Offset: 0x000DFE90
		internal QuotedPrintableStream(int lineLength)
		{
			this.lineLength = lineLength;
		}

		// Token: 0x17000C5A RID: 3162
		// (get) Token: 0x060034F9 RID: 13561 RVA: 0x000E0E9F File Offset: 0x000DFE9F
		private QuotedPrintableStream.ReadStateInfo ReadState
		{
			get
			{
				if (this.readState == null)
				{
					this.readState = new QuotedPrintableStream.ReadStateInfo();
				}
				return this.readState;
			}
		}

		// Token: 0x17000C5B RID: 3163
		// (get) Token: 0x060034FA RID: 13562 RVA: 0x000E0EBA File Offset: 0x000DFEBA
		internal QuotedPrintableStream.WriteStateInfo WriteState
		{
			get
			{
				if (this.writeState == null)
				{
					this.writeState = new QuotedPrintableStream.WriteStateInfo(1024);
				}
				return this.writeState;
			}
		}

		// Token: 0x060034FB RID: 13563 RVA: 0x000E0EDC File Offset: 0x000DFEDC
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			QuotedPrintableStream.WriteAsyncResult writeAsyncResult = new QuotedPrintableStream.WriteAsyncResult(this, buffer, offset, count, callback, state);
			writeAsyncResult.Write();
			return writeAsyncResult;
		}

		// Token: 0x060034FC RID: 13564 RVA: 0x000E0F34 File Offset: 0x000DFF34
		public override void Close()
		{
			this.FlushInternal();
			base.Close();
		}

		// Token: 0x060034FD RID: 13565 RVA: 0x000E0F44 File Offset: 0x000DFF44
		internal unsafe int DecodeBytes(byte[] buffer, int offset, int count)
		{
			try
			{
				fixed (byte* ptr = buffer)
				{
					byte* ptr2 = ptr + offset;
					byte* ptr3 = ptr2;
					byte* ptr4 = ptr2;
					byte* ptr5 = ptr2 + count;
					if (this.ReadState.IsEscaped)
					{
						if (this.ReadState.Byte == -1)
						{
							if (count == 1)
							{
								this.ReadState.Byte = (short)(*ptr3);
								return 0;
							}
							if (*ptr3 != 13 || ptr3[1] != 10)
							{
								byte b = QuotedPrintableStream.hexDecodeMap[(int)(*ptr3)];
								byte b2 = QuotedPrintableStream.hexDecodeMap[(int)ptr3[1]];
								if (b == 255)
								{
									throw new FormatException(SR.GetString("InvalidHexDigit", new object[] { b }));
								}
								if (b2 == 255)
								{
									throw new FormatException(SR.GetString("InvalidHexDigit", new object[] { b2 }));
								}
								*(ptr4++) = (byte)(((int)b << 4) + (int)b2);
							}
							ptr3 += 2;
						}
						else
						{
							if (this.ReadState.Byte != 13 || *ptr3 != 10)
							{
								byte b3 = QuotedPrintableStream.hexDecodeMap[(int)this.ReadState.Byte];
								byte b4 = QuotedPrintableStream.hexDecodeMap[(int)(*ptr3)];
								if (b3 == 255)
								{
									throw new FormatException(SR.GetString("InvalidHexDigit", new object[] { b3 }));
								}
								if (b4 == 255)
								{
									throw new FormatException(SR.GetString("InvalidHexDigit", new object[] { b4 }));
								}
								*(ptr4++) = (byte)(((int)b3 << 4) + (int)b4);
							}
							ptr3++;
						}
						this.ReadState.IsEscaped = false;
						this.ReadState.Byte = -1;
					}
					while (ptr3 < ptr5)
					{
						if (*ptr3 != 61)
						{
							*(ptr4++) = *(ptr3++);
						}
						else
						{
							long num = (long)(ptr5 - ptr3);
							if (num <= 2L && num >= 1L)
							{
								switch ((int)(num - 1L))
								{
								case 0:
									break;
								case 1:
									this.ReadState.Byte = (short)ptr3[1];
									break;
								default:
									goto IL_0226;
								}
								this.ReadState.IsEscaped = true;
								break;
							}
							IL_0226:
							if (ptr3[1] != 13 || ptr3[2] != 10)
							{
								byte b5 = QuotedPrintableStream.hexDecodeMap[(int)ptr3[1]];
								byte b6 = QuotedPrintableStream.hexDecodeMap[(int)ptr3[2]];
								if (b5 == 255)
								{
									throw new FormatException(SR.GetString("InvalidHexDigit", new object[] { b5 }));
								}
								if (b6 == 255)
								{
									throw new FormatException(SR.GetString("InvalidHexDigit", new object[] { b6 }));
								}
								*(ptr4++) = (byte)(((int)b5 << 4) + (int)b6);
							}
							ptr3 += 3;
						}
					}
					count = (int)((long)(ptr4 - ptr2));
				}
			}
			finally
			{
				byte* ptr = null;
			}
			return count;
		}

		// Token: 0x060034FE RID: 13566 RVA: 0x000E1250 File Offset: 0x000E0250
		internal int EncodeBytes(byte[] buffer, int offset, int count)
		{
			int i;
			for (i = offset; i < count + offset; i++)
			{
				if (this.lineLength != -1 && this.WriteState.CurrentLineLength + 5 >= this.lineLength && (buffer[i] == 32 || buffer[i] == 9 || buffer[i] == 13 || buffer[i] == 10))
				{
					if (this.WriteState.Buffer.Length - this.WriteState.Length < 3)
					{
						return i - offset;
					}
					this.WriteState.CurrentLineLength = 0;
					this.WriteState.Buffer[this.WriteState.Length++] = 61;
					this.WriteState.Buffer[this.WriteState.Length++] = 13;
					this.WriteState.Buffer[this.WriteState.Length++] = 10;
				}
				if (this.WriteState.CurrentLineLength == 0 && buffer[i] == 46)
				{
					this.WriteState.Buffer[this.WriteState.Length++] = 46;
				}
				if (buffer[i] == 13 && i + 1 < count + offset && buffer[i + 1] == 10)
				{
					if (this.WriteState.Buffer.Length - this.WriteState.Length < (this.encodeCRLF ? 6 : 2))
					{
						return i - offset;
					}
					i++;
					if (this.encodeCRLF)
					{
						this.WriteState.Buffer[this.WriteState.Length++] = 61;
						this.WriteState.Buffer[this.WriteState.Length++] = 48;
						this.WriteState.Buffer[this.WriteState.Length++] = 68;
						this.WriteState.Buffer[this.WriteState.Length++] = 61;
						this.WriteState.Buffer[this.WriteState.Length++] = 48;
						this.WriteState.Buffer[this.WriteState.Length++] = 65;
						this.WriteState.CurrentLineLength += 6;
					}
					else
					{
						this.WriteState.Buffer[this.WriteState.Length++] = 13;
						this.WriteState.Buffer[this.WriteState.Length++] = 10;
						this.WriteState.CurrentLineLength = 0;
					}
				}
				else if ((buffer[i] < 32 && buffer[i] != 9) || buffer[i] == 61 || buffer[i] > 126)
				{
					if (this.WriteState.Buffer.Length - this.WriteState.Length < 3)
					{
						return i - offset;
					}
					this.WriteState.CurrentLineLength += 3;
					this.WriteState.Buffer[this.WriteState.Length++] = 61;
					this.WriteState.Buffer[this.WriteState.Length++] = QuotedPrintableStream.hexEncodeMap[buffer[i] >> 4];
					this.WriteState.Buffer[this.WriteState.Length++] = QuotedPrintableStream.hexEncodeMap[(int)(buffer[i] & 15)];
				}
				else
				{
					if (this.WriteState.Buffer.Length - this.WriteState.Length < 1)
					{
						return i - offset;
					}
					this.WriteState.CurrentLineLength++;
					this.WriteState.Buffer[this.WriteState.Length++] = buffer[i];
				}
			}
			return i - offset;
		}

		// Token: 0x060034FF RID: 13567 RVA: 0x000E1670 File Offset: 0x000E0670
		public override void EndWrite(IAsyncResult asyncResult)
		{
			QuotedPrintableStream.WriteAsyncResult.End(asyncResult);
		}

		// Token: 0x06003500 RID: 13568 RVA: 0x000E1678 File Offset: 0x000E0678
		public override void Flush()
		{
			this.FlushInternal();
			base.Flush();
		}

		// Token: 0x06003501 RID: 13569 RVA: 0x000E1688 File Offset: 0x000E0688
		private void FlushInternal()
		{
			if (this.writeState != null && this.writeState.Length > 0)
			{
				base.Write(this.WriteState.Buffer, 0, this.WriteState.Length);
				this.WriteState.Length = 0;
			}
		}

		// Token: 0x06003502 RID: 13570 RVA: 0x000E16D4 File Offset: 0x000E06D4
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			int num = 0;
			for (;;)
			{
				num += this.EncodeBytes(buffer, offset + num, count - num);
				if (num >= count)
				{
					break;
				}
				this.FlushInternal();
			}
		}

		// Token: 0x04003095 RID: 12437
		private bool encodeCRLF;

		// Token: 0x04003096 RID: 12438
		private static int DefaultLineLength = 76;

		// Token: 0x04003097 RID: 12439
		private static byte[] hexDecodeMap = new byte[]
		{
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 0, 1,
			2, 3, 4, 5, 6, 7, 8, 9, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 10, 11, 12, 13, 14,
			15, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 10, 11, 12,
			13, 14, 15, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue
		};

		// Token: 0x04003098 RID: 12440
		private static byte[] hexEncodeMap = new byte[]
		{
			48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
			65, 66, 67, 68, 69, 70
		};

		// Token: 0x04003099 RID: 12441
		private int lineLength;

		// Token: 0x0400309A RID: 12442
		private QuotedPrintableStream.ReadStateInfo readState;

		// Token: 0x0400309B RID: 12443
		private QuotedPrintableStream.WriteStateInfo writeState;

		// Token: 0x020006B3 RID: 1715
		private class ReadStateInfo
		{
			// Token: 0x17000C5C RID: 3164
			// (get) Token: 0x06003504 RID: 13572 RVA: 0x000E1882 File Offset: 0x000E0882
			// (set) Token: 0x06003505 RID: 13573 RVA: 0x000E188A File Offset: 0x000E088A
			internal bool IsEscaped
			{
				get
				{
					return this.isEscaped;
				}
				set
				{
					this.isEscaped = value;
				}
			}

			// Token: 0x17000C5D RID: 3165
			// (get) Token: 0x06003506 RID: 13574 RVA: 0x000E1893 File Offset: 0x000E0893
			// (set) Token: 0x06003507 RID: 13575 RVA: 0x000E189B File Offset: 0x000E089B
			internal short Byte
			{
				get
				{
					return this.b1;
				}
				set
				{
					this.b1 = value;
				}
			}

			// Token: 0x0400309C RID: 12444
			private bool isEscaped;

			// Token: 0x0400309D RID: 12445
			private short b1 = -1;
		}

		// Token: 0x020006B4 RID: 1716
		internal class WriteStateInfo
		{
			// Token: 0x06003509 RID: 13577 RVA: 0x000E18B3 File Offset: 0x000E08B3
			internal WriteStateInfo(int bufferSize)
			{
				this.buffer = new byte[bufferSize];
			}

			// Token: 0x17000C5E RID: 3166
			// (get) Token: 0x0600350A RID: 13578 RVA: 0x000E18C7 File Offset: 0x000E08C7
			internal byte[] Buffer
			{
				get
				{
					return this.buffer;
				}
			}

			// Token: 0x17000C5F RID: 3167
			// (get) Token: 0x0600350B RID: 13579 RVA: 0x000E18CF File Offset: 0x000E08CF
			// (set) Token: 0x0600350C RID: 13580 RVA: 0x000E18D7 File Offset: 0x000E08D7
			internal int CurrentLineLength
			{
				get
				{
					return this.currentLineLength;
				}
				set
				{
					this.currentLineLength = value;
				}
			}

			// Token: 0x17000C60 RID: 3168
			// (get) Token: 0x0600350D RID: 13581 RVA: 0x000E18E0 File Offset: 0x000E08E0
			// (set) Token: 0x0600350E RID: 13582 RVA: 0x000E18E8 File Offset: 0x000E08E8
			internal int Length
			{
				get
				{
					return this.length;
				}
				set
				{
					this.length = value;
				}
			}

			// Token: 0x0400309E RID: 12446
			private int currentLineLength;

			// Token: 0x0400309F RID: 12447
			private byte[] buffer;

			// Token: 0x040030A0 RID: 12448
			private int length;
		}

		// Token: 0x020006B5 RID: 1717
		private class WriteAsyncResult : LazyAsyncResult
		{
			// Token: 0x0600350F RID: 13583 RVA: 0x000E18F1 File Offset: 0x000E08F1
			internal WriteAsyncResult(QuotedPrintableStream parent, byte[] buffer, int offset, int count, AsyncCallback callback, object state)
				: base(null, state, callback)
			{
				this.parent = parent;
				this.buffer = buffer;
				this.offset = offset;
				this.count = count;
			}

			// Token: 0x06003510 RID: 13584 RVA: 0x000E191B File Offset: 0x000E091B
			private void CompleteWrite(IAsyncResult result)
			{
				this.parent.BaseStream.EndWrite(result);
				this.parent.WriteState.Length = 0;
			}

			// Token: 0x06003511 RID: 13585 RVA: 0x000E1940 File Offset: 0x000E0940
			internal static void End(IAsyncResult result)
			{
				QuotedPrintableStream.WriteAsyncResult writeAsyncResult = (QuotedPrintableStream.WriteAsyncResult)result;
				writeAsyncResult.InternalWaitForCompletion();
			}

			// Token: 0x06003512 RID: 13586 RVA: 0x000E195C File Offset: 0x000E095C
			private static void OnWrite(IAsyncResult result)
			{
				if (!result.CompletedSynchronously)
				{
					QuotedPrintableStream.WriteAsyncResult writeAsyncResult = (QuotedPrintableStream.WriteAsyncResult)result.AsyncState;
					try
					{
						writeAsyncResult.CompleteWrite(result);
						writeAsyncResult.Write();
					}
					catch (Exception ex)
					{
						writeAsyncResult.InvokeCallback(ex);
					}
					catch
					{
						writeAsyncResult.InvokeCallback(new Exception(SR.GetString("net_nonClsCompliantException")));
					}
				}
			}

			// Token: 0x06003513 RID: 13587 RVA: 0x000E19CC File Offset: 0x000E09CC
			internal void Write()
			{
				for (;;)
				{
					this.written += this.parent.EncodeBytes(this.buffer, this.offset + this.written, this.count - this.written);
					if (this.written >= this.count)
					{
						goto IL_0093;
					}
					IAsyncResult asyncResult = this.parent.BaseStream.BeginWrite(this.parent.WriteState.Buffer, 0, this.parent.WriteState.Length, QuotedPrintableStream.WriteAsyncResult.onWrite, this);
					if (!asyncResult.CompletedSynchronously)
					{
						break;
					}
					this.CompleteWrite(asyncResult);
				}
				return;
				IL_0093:
				base.InvokeCallback();
			}

			// Token: 0x040030A1 RID: 12449
			private QuotedPrintableStream parent;

			// Token: 0x040030A2 RID: 12450
			private byte[] buffer;

			// Token: 0x040030A3 RID: 12451
			private int offset;

			// Token: 0x040030A4 RID: 12452
			private int count;

			// Token: 0x040030A5 RID: 12453
			private static AsyncCallback onWrite = new AsyncCallback(QuotedPrintableStream.WriteAsyncResult.OnWrite);

			// Token: 0x040030A6 RID: 12454
			private int written;
		}
	}
}
