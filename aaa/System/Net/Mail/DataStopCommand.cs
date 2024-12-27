using System;

namespace System.Net.Mail
{
	// Token: 0x020006C0 RID: 1728
	internal static class DataStopCommand
	{
		// Token: 0x06003568 RID: 13672 RVA: 0x000E35EC File Offset: 0x000E25EC
		private static void CheckResponse(SmtpStatusCode statusCode, string serverResponse)
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
				switch (statusCode)
				{
				}
				break;
			}
			if (statusCode < (SmtpStatusCode)400)
			{
				throw new SmtpException(SR.GetString("net_webstatus_ServerProtocolViolation"), serverResponse);
			}
			throw new SmtpException(statusCode, serverResponse, true);
		}

		// Token: 0x06003569 RID: 13673 RVA: 0x000E3653 File Offset: 0x000E2653
		private static void PrepareCommand(SmtpConnection conn)
		{
			if (conn.IsStreamOpen)
			{
				throw new InvalidOperationException(SR.GetString("SmtpDataStreamOpen"));
			}
			conn.BufferBuilder.Append(SmtpCommands.DataStop);
		}

		// Token: 0x0600356A RID: 13674 RVA: 0x000E3680 File Offset: 0x000E2680
		internal static void Send(SmtpConnection conn)
		{
			DataStopCommand.PrepareCommand(conn);
			string text;
			SmtpStatusCode smtpStatusCode = CheckCommand.Send(conn, out text);
			DataStopCommand.CheckResponse(smtpStatusCode, text);
		}
	}
}
