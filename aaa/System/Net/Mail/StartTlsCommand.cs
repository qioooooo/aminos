using System;

namespace System.Net.Mail
{
	// Token: 0x020006C3 RID: 1731
	internal static class StartTlsCommand
	{
		// Token: 0x06003575 RID: 13685 RVA: 0x000E38C4 File Offset: 0x000E28C4
		internal static IAsyncResult BeginSend(SmtpConnection conn, AsyncCallback callback, object state)
		{
			StartTlsCommand.PrepareCommand(conn);
			return CheckCommand.BeginSend(conn, callback, state);
		}

		// Token: 0x06003576 RID: 13686 RVA: 0x000E38D4 File Offset: 0x000E28D4
		private static void CheckResponse(SmtpStatusCode statusCode, string response)
		{
			if (statusCode == SmtpStatusCode.ServiceReady)
			{
				return;
			}
			if (statusCode != SmtpStatusCode.ClientNotPermitted)
			{
			}
			if (statusCode < (SmtpStatusCode)400)
			{
				throw new SmtpException(SR.GetString("net_webstatus_ServerProtocolViolation"), response);
			}
			throw new SmtpException(statusCode, response, true);
		}

		// Token: 0x06003577 RID: 13687 RVA: 0x000E3918 File Offset: 0x000E2918
		internal static void EndSend(IAsyncResult result)
		{
			string text;
			SmtpStatusCode smtpStatusCode = (SmtpStatusCode)CheckCommand.EndSend(result, out text);
			StartTlsCommand.CheckResponse(smtpStatusCode, text);
		}

		// Token: 0x06003578 RID: 13688 RVA: 0x000E393A File Offset: 0x000E293A
		private static void PrepareCommand(SmtpConnection conn)
		{
			if (conn.IsStreamOpen)
			{
				throw new InvalidOperationException(SR.GetString("SmtpDataStreamOpen"));
			}
			conn.BufferBuilder.Append(SmtpCommands.StartTls);
			conn.BufferBuilder.Append(SmtpCommands.CRLF);
		}

		// Token: 0x06003579 RID: 13689 RVA: 0x000E3974 File Offset: 0x000E2974
		internal static void Send(SmtpConnection conn)
		{
			StartTlsCommand.PrepareCommand(conn);
			string text;
			SmtpStatusCode smtpStatusCode = CheckCommand.Send(conn, out text);
			StartTlsCommand.CheckResponse(smtpStatusCode, text);
		}
	}
}
