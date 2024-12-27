using System;
using System.Globalization;

namespace System.Net
{
	// Token: 0x0200054E RID: 1358
	internal class FrameHeader
	{
		// Token: 0x06002930 RID: 10544 RVA: 0x000AC558 File Offset: 0x000AB558
		public FrameHeader()
		{
			this._MessageId = 22;
			this._MajorV = 1;
			this._MinorV = 0;
			this._PayloadSize = -1;
		}

		// Token: 0x06002931 RID: 10545 RVA: 0x000AC57D File Offset: 0x000AB57D
		public FrameHeader(int messageId, int majorV, int minorV)
		{
			this._MessageId = messageId;
			this._MajorV = majorV;
			this._MinorV = minorV;
			this._PayloadSize = -1;
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06002932 RID: 10546 RVA: 0x000AC5A1 File Offset: 0x000AB5A1
		public int Size
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x06002933 RID: 10547 RVA: 0x000AC5A4 File Offset: 0x000AB5A4
		public int MaxMessageSize
		{
			get
			{
				return 65535;
			}
		}

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x06002934 RID: 10548 RVA: 0x000AC5AB File Offset: 0x000AB5AB
		// (set) Token: 0x06002935 RID: 10549 RVA: 0x000AC5B3 File Offset: 0x000AB5B3
		public int MessageId
		{
			get
			{
				return this._MessageId;
			}
			set
			{
				this._MessageId = value;
			}
		}

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x06002936 RID: 10550 RVA: 0x000AC5BC File Offset: 0x000AB5BC
		public int MajorV
		{
			get
			{
				return this._MajorV;
			}
		}

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x06002937 RID: 10551 RVA: 0x000AC5C4 File Offset: 0x000AB5C4
		public int MinorV
		{
			get
			{
				return this._MinorV;
			}
		}

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x06002938 RID: 10552 RVA: 0x000AC5CC File Offset: 0x000AB5CC
		// (set) Token: 0x06002939 RID: 10553 RVA: 0x000AC5D4 File Offset: 0x000AB5D4
		public int PayloadSize
		{
			get
			{
				return this._PayloadSize;
			}
			set
			{
				if (value > this.MaxMessageSize)
				{
					throw new ArgumentException(SR.GetString("net_frame_max_size", new object[]
					{
						this.MaxMessageSize.ToString(NumberFormatInfo.InvariantInfo),
						value.ToString(NumberFormatInfo.InvariantInfo)
					}), "PayloadSize");
				}
				this._PayloadSize = value;
			}
		}

		// Token: 0x0600293A RID: 10554 RVA: 0x000AC634 File Offset: 0x000AB634
		public void CopyTo(byte[] dest, int start)
		{
			dest[start++] = (byte)this._MessageId;
			dest[start++] = (byte)this._MajorV;
			dest[start++] = (byte)this._MinorV;
			dest[start++] = (byte)((this._PayloadSize >> 8) & 255);
			dest[start] = (byte)(this._PayloadSize & 255);
		}

		// Token: 0x0600293B RID: 10555 RVA: 0x000AC698 File Offset: 0x000AB698
		public void CopyFrom(byte[] bytes, int start, FrameHeader verifier)
		{
			this._MessageId = (int)bytes[start++];
			this._MajorV = (int)bytes[start++];
			this._MinorV = (int)bytes[start++];
			this._PayloadSize = ((int)bytes[start++] << 8) | (int)bytes[start];
			if (verifier.MessageId != -1 && this.MessageId != verifier.MessageId)
			{
				throw new InvalidOperationException(SR.GetString("net_io_header_id", new object[] { "MessageId", this.MessageId, verifier.MessageId }));
			}
			if (verifier.MajorV != -1 && this.MajorV != verifier.MajorV)
			{
				throw new InvalidOperationException(SR.GetString("net_io_header_id", new object[] { "MajorV", this.MajorV, verifier.MajorV }));
			}
			if (verifier.MinorV != -1 && this.MinorV != verifier.MinorV)
			{
				throw new InvalidOperationException(SR.GetString("net_io_header_id", new object[] { "MinorV", this.MinorV, verifier.MinorV }));
			}
		}

		// Token: 0x04002843 RID: 10307
		public const int IgnoreValue = -1;

		// Token: 0x04002844 RID: 10308
		public const int HandshakeDoneId = 20;

		// Token: 0x04002845 RID: 10309
		public const int HandshakeErrId = 21;

		// Token: 0x04002846 RID: 10310
		public const int HandshakeId = 22;

		// Token: 0x04002847 RID: 10311
		public const int DefaultMajorV = 1;

		// Token: 0x04002848 RID: 10312
		public const int DefaultMinorV = 0;

		// Token: 0x04002849 RID: 10313
		private int _MessageId;

		// Token: 0x0400284A RID: 10314
		private int _MajorV;

		// Token: 0x0400284B RID: 10315
		private int _MinorV;

		// Token: 0x0400284C RID: 10316
		private int _PayloadSize;
	}
}
