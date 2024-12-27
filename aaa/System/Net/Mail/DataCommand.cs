using System;

namespace System.Net.Mail
{
	// Token: 0x020006BF RID: 1727
	internal static class DataCommand
	{
		// Token: 0x06003563 RID: 13667 RVA: 0x000E351D File Offset: 0x000E251D
		internal static IAsyncResult BeginSend(SmtpConnection conn, AsyncCallback callback, object state)
		{
			DataCommand.PrepareCommand(conn);
			return CheckCommand.BeginSend(conn, callback, state);
		}

		// Token: 0x06003564 RID: 13668 RVA: 0x000E3530 File Offset: 0x000E2530
		private static void CheckResponse(SmtpStatusCode statusCode, string serverResponse)
		{
			if (statusCode == SmtpStatusCode.StartMailInput)
			{
				return;
			}
			if (statusCode != SmtpStatusCode.LocalErrorInProcessing && statusCode != SmtpStatusCode.TransactionFailed)
			{
			}
			if (statusCode < (SmtpStatusCode)400)
			{
				throw new SmtpException(SR.GetString("net_webstatus_ServerProtocolViolation"), serverResponse);
			}
			throw new SmtpException(statusCode, serverResponse, true);
		}

		// Token: 0x06003565 RID: 13669 RVA: 0x000E357C File Offset: 0x000E257C
		internal static void EndSend(IAsyncResult result)
		{
			string text;
			SmtpStatusCode smtpStatusCode = (SmtpStatusCode)CheckCommand.EndSend(result, out text);
			DataCommand.CheckResponse(smtpStatusCode, text);
		}

		// Token: 0x06003566 RID: 13670 RVA: 0x000E359E File Offset: 0x000E259E
		private static void PrepareCommand(SmtpConnection conn)
		{
			if (conn.IsStreamOpen)
			{
				throw new InvalidOperationException(SR.GetString("SmtpDataStreamOpen"));
			}
			conn.BufferBuilder.Append(SmtpCommands.Data);
		}

		// Token: 0x06003567 RID: 13671 RVA: 0x000E35C8 File Offset: 0x000E25C8
		internal static void Send(SmtpConnection conn)
		{
			DataCommand.PrepareCommand(conn);
			string text;
			SmtpStatusCode smtpStatusCode = CheckCommand.Send(conn, out text);
			DataCommand.CheckResponse(smtpStatusCode, text);
		}
	}
}
