using System;

namespace System.Net.Mail
{
	// Token: 0x020006BE RID: 1726
	internal static class AuthCommand
	{
		// Token: 0x0600355B RID: 13659 RVA: 0x000E342C File Offset: 0x000E242C
		internal static IAsyncResult BeginSend(SmtpConnection conn, string type, string message, AsyncCallback callback, object state)
		{
			AuthCommand.PrepareCommand(conn, type, message);
			return ReadLinesCommand.BeginSend(conn, callback, state);
		}

		// Token: 0x0600355C RID: 13660 RVA: 0x000E343F File Offset: 0x000E243F
		internal static IAsyncResult BeginSend(SmtpConnection conn, string message, AsyncCallback callback, object state)
		{
			AuthCommand.PrepareCommand(conn, message);
			return ReadLinesCommand.BeginSend(conn, callback, state);
		}

		// Token: 0x0600355D RID: 13661 RVA: 0x000E3450 File Offset: 0x000E2450
		private static LineInfo CheckResponse(LineInfo[] lines)
		{
			if (lines == null || lines.Length == 0)
			{
				throw new SmtpException(SR.GetString("SmtpAuthResponseInvalid"));
			}
			return lines[0];
		}

		// Token: 0x0600355E RID: 13662 RVA: 0x000E3476 File Offset: 0x000E2476
		internal static LineInfo EndSend(IAsyncResult result)
		{
			return AuthCommand.CheckResponse(ReadLinesCommand.EndSend(result));
		}

		// Token: 0x0600355F RID: 13663 RVA: 0x000E3484 File Offset: 0x000E2484
		private static void PrepareCommand(SmtpConnection conn, string type, string message)
		{
			conn.BufferBuilder.Append(SmtpCommands.Auth);
			conn.BufferBuilder.Append(type);
			conn.BufferBuilder.Append(32);
			conn.BufferBuilder.Append(message);
			conn.BufferBuilder.Append(SmtpCommands.CRLF);
		}

		// Token: 0x06003560 RID: 13664 RVA: 0x000E34D6 File Offset: 0x000E24D6
		private static void PrepareCommand(SmtpConnection conn, string message)
		{
			conn.BufferBuilder.Append(message);
			conn.BufferBuilder.Append(SmtpCommands.CRLF);
		}

		// Token: 0x06003561 RID: 13665 RVA: 0x000E34F4 File Offset: 0x000E24F4
		internal static LineInfo Send(SmtpConnection conn, string type, string message)
		{
			AuthCommand.PrepareCommand(conn, type, message);
			return AuthCommand.CheckResponse(ReadLinesCommand.Send(conn));
		}

		// Token: 0x06003562 RID: 13666 RVA: 0x000E3509 File Offset: 0x000E2509
		internal static LineInfo Send(SmtpConnection conn, string message)
		{
			AuthCommand.PrepareCommand(conn, message);
			return AuthCommand.CheckResponse(ReadLinesCommand.Send(conn));
		}
	}
}
