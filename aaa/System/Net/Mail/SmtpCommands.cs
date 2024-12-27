using System;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x020006C6 RID: 1734
	internal static class SmtpCommands
	{
		// Token: 0x040030CB RID: 12491
		internal static readonly byte[] Auth = Encoding.ASCII.GetBytes("AUTH ");

		// Token: 0x040030CC RID: 12492
		internal static readonly byte[] CRLF = Encoding.ASCII.GetBytes("\r\n");

		// Token: 0x040030CD RID: 12493
		internal static readonly byte[] Data = Encoding.ASCII.GetBytes("DATA\r\n");

		// Token: 0x040030CE RID: 12494
		internal static readonly byte[] DataStop = Encoding.ASCII.GetBytes("\r\n.\r\n");

		// Token: 0x040030CF RID: 12495
		internal static readonly byte[] EHello = Encoding.ASCII.GetBytes("EHLO ");

		// Token: 0x040030D0 RID: 12496
		internal static readonly byte[] Expand = Encoding.ASCII.GetBytes("EXPN ");

		// Token: 0x040030D1 RID: 12497
		internal static readonly byte[] Hello = Encoding.ASCII.GetBytes("HELO ");

		// Token: 0x040030D2 RID: 12498
		internal static readonly byte[] Help = Encoding.ASCII.GetBytes("HELP");

		// Token: 0x040030D3 RID: 12499
		internal static readonly byte[] Mail = Encoding.ASCII.GetBytes("MAIL FROM:");

		// Token: 0x040030D4 RID: 12500
		internal static readonly byte[] Noop = Encoding.ASCII.GetBytes("NOOP\r\n");

		// Token: 0x040030D5 RID: 12501
		internal static readonly byte[] Quit = Encoding.ASCII.GetBytes("QUIT\r\n");

		// Token: 0x040030D6 RID: 12502
		internal static readonly byte[] Recipient = Encoding.ASCII.GetBytes("RCPT TO:");

		// Token: 0x040030D7 RID: 12503
		internal static readonly byte[] Reset = Encoding.ASCII.GetBytes("RSET\r\n");

		// Token: 0x040030D8 RID: 12504
		internal static readonly byte[] Send = Encoding.ASCII.GetBytes("SEND FROM:");

		// Token: 0x040030D9 RID: 12505
		internal static readonly byte[] SendAndMail = Encoding.ASCII.GetBytes("SAML FROM:");

		// Token: 0x040030DA RID: 12506
		internal static readonly byte[] SendOrMail = Encoding.ASCII.GetBytes("SOML FROM:");

		// Token: 0x040030DB RID: 12507
		internal static readonly byte[] Turn = Encoding.ASCII.GetBytes("TURN\r\n");

		// Token: 0x040030DC RID: 12508
		internal static readonly byte[] Verify = Encoding.ASCII.GetBytes("VRFY ");

		// Token: 0x040030DD RID: 12509
		internal static readonly byte[] StartTls = Encoding.ASCII.GetBytes("STARTTLS");
	}
}
