using System;

namespace System.Net.Mail
{
	// Token: 0x020006C1 RID: 1729
	internal static class EHelloCommand
	{
		// Token: 0x0600356B RID: 13675 RVA: 0x000E36A3 File Offset: 0x000E26A3
		internal static IAsyncResult BeginSend(SmtpConnection conn, string domain, AsyncCallback callback, object state)
		{
			EHelloCommand.PrepareCommand(conn, domain);
			return ReadLinesCommand.BeginSend(conn, callback, state);
		}

		// Token: 0x0600356C RID: 13676 RVA: 0x000E36B4 File Offset: 0x000E26B4
		private static string[] CheckResponse(LineInfo[] lines)
		{
			if (lines == null || lines.Length == 0)
			{
				throw new SmtpException(SR.GetString("SmtpEhloResponseInvalid"));
			}
			if (lines[0].StatusCode == SmtpStatusCode.Ok)
			{
				string[] array = new string[lines.Length - 1];
				for (int i = 1; i < lines.Length; i++)
				{
					array[i - 1] = lines[i].Line;
				}
				return array;
			}
			if (lines[0].StatusCode < (SmtpStatusCode)400)
			{
				throw new SmtpException(SR.GetString("net_webstatus_ServerProtocolViolation"), lines[0].Line);
			}
			throw new SmtpException(lines[0].StatusCode, lines[0].Line, true);
		}

		// Token: 0x0600356D RID: 13677 RVA: 0x000E3765 File Offset: 0x000E2765
		internal static string[] EndSend(IAsyncResult result)
		{
			return EHelloCommand.CheckResponse(ReadLinesCommand.EndSend(result));
		}

		// Token: 0x0600356E RID: 13678 RVA: 0x000E3774 File Offset: 0x000E2774
		private static void PrepareCommand(SmtpConnection conn, string domain)
		{
			if (conn.IsStreamOpen)
			{
				throw new InvalidOperationException(SR.GetString("SmtpDataStreamOpen"));
			}
			conn.BufferBuilder.Append(SmtpCommands.EHello);
			conn.BufferBuilder.Append(domain);
			conn.BufferBuilder.Append(SmtpCommands.CRLF);
		}

		// Token: 0x0600356F RID: 13679 RVA: 0x000E37C5 File Offset: 0x000E27C5
		internal static string[] Send(SmtpConnection conn, string domain)
		{
			EHelloCommand.PrepareCommand(conn, domain);
			return EHelloCommand.CheckResponse(ReadLinesCommand.Send(conn));
		}
	}
}
