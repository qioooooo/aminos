using System;

namespace System.Net.Mail
{
	// Token: 0x020006C4 RID: 1732
	internal static class MailCommand
	{
		// Token: 0x0600357A RID: 13690 RVA: 0x000E3997 File Offset: 0x000E2997
		internal static IAsyncResult BeginSend(SmtpConnection conn, byte[] command, string from, AsyncCallback callback, object state)
		{
			MailCommand.PrepareCommand(conn, command, from);
			return CheckCommand.BeginSend(conn, callback, state);
		}

		// Token: 0x0600357B RID: 13691 RVA: 0x000E39AC File Offset: 0x000E29AC
		private static void CheckResponse(SmtpStatusCode statusCode, string response)
		{
			if (statusCode == SmtpStatusCode.Ok)
			{
				return;
			}
			switch (statusCode)
			{
			case SmtpStatusCode.LocalErrorInProcessing:
			case SmtpStatusCode.InsufficientStorage:
				break;
			default:
				if (statusCode != SmtpStatusCode.ExceededStorageAllocation)
				{
				}
				break;
			}
			if (statusCode < (SmtpStatusCode)400)
			{
				throw new SmtpException(SR.GetString("net_webstatus_ServerProtocolViolation"), response);
			}
			throw new SmtpException(statusCode, response, true);
		}

		// Token: 0x0600357C RID: 13692 RVA: 0x000E3A04 File Offset: 0x000E2A04
		internal static void EndSend(IAsyncResult result)
		{
			string text;
			SmtpStatusCode smtpStatusCode = (SmtpStatusCode)CheckCommand.EndSend(result, out text);
			MailCommand.CheckResponse(smtpStatusCode, text);
		}

		// Token: 0x0600357D RID: 13693 RVA: 0x000E3A28 File Offset: 0x000E2A28
		private static void PrepareCommand(SmtpConnection conn, byte[] command, string from)
		{
			if (conn.IsStreamOpen)
			{
				throw new InvalidOperationException(SR.GetString("SmtpDataStreamOpen"));
			}
			conn.BufferBuilder.Append(command);
			conn.BufferBuilder.Append(from);
			conn.BufferBuilder.Append(SmtpCommands.CRLF);
		}

		// Token: 0x0600357E RID: 13694 RVA: 0x000E3A78 File Offset: 0x000E2A78
		internal static void Send(SmtpConnection conn, byte[] command, string from)
		{
			MailCommand.PrepareCommand(conn, command, from);
			string text;
			SmtpStatusCode smtpStatusCode = CheckCommand.Send(conn, out text);
			MailCommand.CheckResponse(smtpStatusCode, text);
		}
	}
}
