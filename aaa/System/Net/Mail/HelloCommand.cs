using System;

namespace System.Net.Mail
{
	// Token: 0x020006C2 RID: 1730
	internal static class HelloCommand
	{
		// Token: 0x06003570 RID: 13680 RVA: 0x000E37D9 File Offset: 0x000E27D9
		internal static IAsyncResult BeginSend(SmtpConnection conn, string domain, AsyncCallback callback, object state)
		{
			HelloCommand.PrepareCommand(conn, domain);
			return CheckCommand.BeginSend(conn, callback, state);
		}

		// Token: 0x06003571 RID: 13681 RVA: 0x000E37EC File Offset: 0x000E27EC
		private static void CheckResponse(SmtpStatusCode statusCode, string serverResponse)
		{
			if (statusCode == SmtpStatusCode.Ok)
			{
				return;
			}
			if (statusCode < (SmtpStatusCode)400)
			{
				throw new SmtpException(SR.GetString("net_webstatus_ServerProtocolViolation"), serverResponse);
			}
			throw new SmtpException(statusCode, serverResponse, true);
		}

		// Token: 0x06003572 RID: 13682 RVA: 0x000E3828 File Offset: 0x000E2828
		internal static void EndSend(IAsyncResult result)
		{
			string text;
			SmtpStatusCode smtpStatusCode = (SmtpStatusCode)CheckCommand.EndSend(result, out text);
			HelloCommand.CheckResponse(smtpStatusCode, text);
		}

		// Token: 0x06003573 RID: 13683 RVA: 0x000E384C File Offset: 0x000E284C
		private static void PrepareCommand(SmtpConnection conn, string domain)
		{
			if (conn.IsStreamOpen)
			{
				throw new InvalidOperationException(SR.GetString("SmtpDataStreamOpen"));
			}
			conn.BufferBuilder.Append(SmtpCommands.Hello);
			conn.BufferBuilder.Append(domain);
			conn.BufferBuilder.Append(SmtpCommands.CRLF);
		}

		// Token: 0x06003574 RID: 13684 RVA: 0x000E38A0 File Offset: 0x000E28A0
		internal static void Send(SmtpConnection conn, string domain)
		{
			HelloCommand.PrepareCommand(conn, domain);
			string text;
			SmtpStatusCode smtpStatusCode = CheckCommand.Send(conn, out text);
			HelloCommand.CheckResponse(smtpStatusCode, text);
		}
	}
}
