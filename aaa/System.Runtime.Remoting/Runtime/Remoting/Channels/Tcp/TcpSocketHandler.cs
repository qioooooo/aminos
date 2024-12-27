using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace System.Runtime.Remoting.Channels.Tcp
{
	// Token: 0x02000041 RID: 65
	internal abstract class TcpSocketHandler : SocketHandler
	{
		// Token: 0x0600021B RID: 539 RVA: 0x0000ACE9 File Offset: 0x00009CE9
		public TcpSocketHandler(Socket socket, Stream stream)
			: this(socket, null, stream)
		{
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000ACF4 File Offset: 0x00009CF4
		public TcpSocketHandler(Socket socket, RequestQueue requestQueue, Stream stream)
			: base(socket, requestQueue, stream)
		{
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000ACFF File Offset: 0x00009CFF
		private void ReadAndMatchPreamble()
		{
			if (!base.ReadAndMatchFourBytes(TcpSocketHandler.s_protocolPreamble))
			{
				throw new RemotingException(CoreChannel.GetResourceString("Remoting_Tcp_ExpectingPreamble"));
			}
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000AD1E File Offset: 0x00009D1E
		protected void WritePreambleAndVersion(Stream outputStream)
		{
			outputStream.Write(TcpSocketHandler.s_protocolPreamble, 0, TcpSocketHandler.s_protocolPreamble.Length);
			outputStream.Write(TcpSocketHandler.s_protocolVersion1_0, 0, TcpSocketHandler.s_protocolVersion1_0.Length);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000AD48 File Offset: 0x00009D48
		protected void ReadVersionAndOperation(out ushort operation)
		{
			this.ReadAndMatchPreamble();
			byte b = (byte)base.ReadByte();
			byte b2 = (byte)base.ReadByte();
			if (b != 1 || b2 != 0)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Tcp_UnknownProtocolVersion"), new object[] { b.ToString(CultureInfo.CurrentCulture) + "." + b2.ToString(CultureInfo.CurrentCulture) }));
			}
			operation = base.ReadUInt16();
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000ADC4 File Offset: 0x00009DC4
		protected void ReadContentLength(out bool chunked, out int contentLength)
		{
			contentLength = -1;
			ushort num = base.ReadUInt16();
			if (num == 1)
			{
				chunked = true;
				return;
			}
			if (num == 0)
			{
				chunked = false;
				contentLength = base.ReadInt32();
				return;
			}
			throw new RemotingException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Tcp_ExpectingContentLengthHeader"), new object[] { num.ToString(CultureInfo.CurrentCulture) }));
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000AE24 File Offset: 0x00009E24
		protected void ReadToEndOfHeaders(BaseTransportHeaders headers)
		{
			bool flag = false;
			string text = null;
			for (ushort num = base.ReadUInt16(); num != 0; num = base.ReadUInt16())
			{
				if (num == 1)
				{
					string text2 = this.ReadCountedString();
					string text3 = this.ReadCountedString();
					headers[text2] = text3;
				}
				else if (num == 4)
				{
					this.ReadAndVerifyHeaderFormat("RequestUri", 1);
					string text4 = this.ReadCountedString();
					string text5;
					if (TcpChannelHelper.ParseURL(text4, out text5) == null)
					{
						text5 = text4;
					}
					headers.RequestUri = text5;
				}
				else if (num == 2)
				{
					this.ReadAndVerifyHeaderFormat("StatusCode", 3);
					ushort num2 = base.ReadUInt16();
					if (num2 != 0)
					{
						flag = true;
					}
				}
				else if (num == 3)
				{
					this.ReadAndVerifyHeaderFormat("StatusPhrase", 1);
					text = this.ReadCountedString();
				}
				else if (num == 6)
				{
					this.ReadAndVerifyHeaderFormat("Content-Type", 1);
					string text6 = this.ReadCountedString();
					headers.ContentType = text6;
				}
				else
				{
					byte b = (byte)base.ReadByte();
					switch (b)
					{
					case 0:
						break;
					case 1:
						this.ReadCountedString();
						break;
					case 2:
						base.ReadByte();
						break;
					case 3:
						base.ReadUInt16();
						break;
					case 4:
						base.ReadInt32();
						break;
					default:
						throw new RemotingException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Tcp_UnknownHeaderType"), new object[] { num, b }));
					}
				}
			}
			if (flag)
			{
				if (text == null)
				{
					text = "";
				}
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Tcp_GenericServerError"), new object[] { text }));
			}
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000AFCC File Offset: 0x00009FCC
		protected void WriteHeaders(ITransportHeaders headers, Stream outputStream)
		{
			BaseTransportHeaders baseTransportHeaders = headers as BaseTransportHeaders;
			IEnumerator enumerator;
			if (baseTransportHeaders != null)
			{
				if (baseTransportHeaders.ContentType != null)
				{
					this.WriteContentTypeHeader(baseTransportHeaders.ContentType, outputStream);
				}
				enumerator = baseTransportHeaders.GetOtherHeadersEnumerator();
			}
			else
			{
				enumerator = headers.GetEnumerator();
			}
			if (enumerator != null)
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					string text = (string)dictionaryEntry.Key;
					if (!StringHelper.StartsWithDoubleUnderscore(text))
					{
						string text2 = dictionaryEntry.Value.ToString();
						if (baseTransportHeaders == null && string.Compare(text, "Content-Type", StringComparison.OrdinalIgnoreCase) == 0)
						{
							this.WriteContentTypeHeader(text2, outputStream);
						}
						else
						{
							this.WriteCustomHeader(text, text2, outputStream);
						}
					}
				}
			}
			base.WriteUInt16(0, outputStream);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000B074 File Offset: 0x0000A074
		private void WriteContentTypeHeader(string value, Stream outputStream)
		{
			base.WriteUInt16(6, outputStream);
			base.WriteByte(1, outputStream);
			this.WriteCountedString(value, outputStream);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000B08E File Offset: 0x0000A08E
		private void WriteCustomHeader(string name, string value, Stream outputStream)
		{
			base.WriteUInt16(1, outputStream);
			this.WriteCountedString(name, outputStream);
			this.WriteCountedString(value, outputStream);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000B0A8 File Offset: 0x0000A0A8
		protected string ReadCountedString()
		{
			byte b = (byte)base.ReadByte();
			int num = base.ReadInt32();
			if (num <= 0)
			{
				return null;
			}
			byte[] array = new byte[num];
			base.Read(array, 0, num);
			switch (b)
			{
			case 0:
				return Encoding.Unicode.GetString(array);
			case 1:
				return Encoding.UTF8.GetString(array);
			default:
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Tcp_UnrecognizedStringFormat"), new object[] { b.ToString(CultureInfo.CurrentCulture) }));
			}
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000B138 File Offset: 0x0000A138
		protected void WriteCountedString(string str, Stream outputStream)
		{
			int num = 0;
			if (str != null)
			{
				num = str.Length;
			}
			if (num > 0)
			{
				byte[] bytes = Encoding.UTF8.GetBytes(str);
				base.WriteByte(1, outputStream);
				base.WriteInt32(bytes.Length, outputStream);
				outputStream.Write(bytes, 0, bytes.Length);
				return;
			}
			base.WriteByte(0, outputStream);
			base.WriteInt32(0, outputStream);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000B190 File Offset: 0x0000A190
		private void ReadAndVerifyHeaderFormat(string headerName, byte expectedFormat)
		{
			byte b = (byte)base.ReadByte();
			if (b != expectedFormat)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Tcp_IncorrectHeaderFormat"), new object[] { expectedFormat, headerName }));
			}
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000B1D8 File Offset: 0x0000A1D8
		// Note: this type is marked as 'beforefieldinit'.
		static TcpSocketHandler()
		{
			byte[] array = new byte[2];
			array[0] = 1;
			TcpSocketHandler.s_protocolVersion1_0 = array;
		}

		// Token: 0x04000188 RID: 392
		private static byte[] s_protocolPreamble = Encoding.ASCII.GetBytes(".NET");

		// Token: 0x04000189 RID: 393
		private static byte[] s_protocolVersion1_0;
	}
}
