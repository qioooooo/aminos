using System;

namespace System.Net.Mail
{
	// Token: 0x020006C5 RID: 1733
	internal static class RecipientCommand
	{
		// Token: 0x0600357F RID: 13695 RVA: 0x000E3A9D File Offset: 0x000E2A9D
		internal static IAsyncResult BeginSend(SmtpConnection conn, string to, AsyncCallback callback, object state)
		{
			RecipientCommand.PrepareCommand(conn, to);
			return CheckCommand.BeginSend(conn, callback, state);
		}

		// Token: 0x06003580 RID: 13696 RVA: 0x000E3AB0 File Offset: 0x000E2AB0
		private static bool CheckResponse(SmtpStatusCode statusCode, string response)
		{
			switch (statusCode)
			{
			case SmtpStatusCode.Ok:
			case SmtpStatusCode.UserNotLocalWillForward:
				return true;
			default:
				switch (statusCode)
				{
				case SmtpStatusCode.MailboxBusy:
				case SmtpStatusCode.InsufficientStorage:
					break;
				case SmtpStatusCode.LocalErrorInProcessing:
					goto IL_0050;
				default:
					switch (statusCode)
					{
					case SmtpStatusCode.MailboxUnavailable:
					case SmtpStatusCode.UserNotLocalTryAlternatePath:
					case SmtpStatusCode.ExceededStorageAllocation:
					case SmtpStatusCode.MailboxNameNotAllowed:
						break;
					default:
						goto IL_0050;
					}
					break;
				}
				return false;
				IL_0050:
				if (statusCode < (SmtpStatusCode)400)
				{
					throw new SmtpException(SR.GetString("net_webstatus_ServerProtocolViolation"), response);
				}
				throw new SmtpException(statusCode, response, true);
			}
		}

		// Token: 0x06003581 RID: 13697 RVA: 0x000E3B30 File Offset: 0x000E2B30
		internal static bool EndSend(IAsyncResult result, out string response)
		{
			SmtpStatusCode smtpStatusCode = (SmtpStatusCode)CheckCommand.EndSend(result, out response);
			return RecipientCommand.CheckResponse(smtpStatusCode, response);
		}

		// Token: 0x06003582 RID: 13698 RVA: 0x000E3B54 File Offset: 0x000E2B54
		private static void PrepareCommand(SmtpConnection conn, string to)
		{
			if (conn.IsStreamOpen)
			{
				throw new InvalidOperationException(SR.GetString("SmtpDataStreamOpen"));
			}
			conn.BufferBuilder.Append(SmtpCommands.Recipient);
			conn.BufferBuilder.Append(to);
			conn.BufferBuilder.Append(SmtpCommands.CRLF);
		}

		// Token: 0x06003583 RID: 13699 RVA: 0x000E3BA8 File Offset: 0x000E2BA8
		internal static bool Send(SmtpConnection conn, string to, out string response)
		{
			RecipientCommand.PrepareCommand(conn, to);
			SmtpStatusCode smtpStatusCode = CheckCommand.Send(conn, out response);
			return RecipientCommand.CheckResponse(smtpStatusCode, response);
		}
	}
}
