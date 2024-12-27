using System;

namespace System.Net.Mail
{
	// Token: 0x020006D9 RID: 1753
	public enum SmtpStatusCode
	{
		// Token: 0x04003131 RID: 12593
		SystemStatus = 211,
		// Token: 0x04003132 RID: 12594
		HelpMessage = 214,
		// Token: 0x04003133 RID: 12595
		ServiceReady = 220,
		// Token: 0x04003134 RID: 12596
		ServiceClosingTransmissionChannel,
		// Token: 0x04003135 RID: 12597
		Ok = 250,
		// Token: 0x04003136 RID: 12598
		UserNotLocalWillForward,
		// Token: 0x04003137 RID: 12599
		CannotVerifyUserWillAttemptDelivery,
		// Token: 0x04003138 RID: 12600
		StartMailInput = 354,
		// Token: 0x04003139 RID: 12601
		ServiceNotAvailable = 421,
		// Token: 0x0400313A RID: 12602
		MailboxBusy = 450,
		// Token: 0x0400313B RID: 12603
		LocalErrorInProcessing,
		// Token: 0x0400313C RID: 12604
		InsufficientStorage,
		// Token: 0x0400313D RID: 12605
		ClientNotPermitted = 454,
		// Token: 0x0400313E RID: 12606
		CommandUnrecognized = 500,
		// Token: 0x0400313F RID: 12607
		SyntaxError,
		// Token: 0x04003140 RID: 12608
		CommandNotImplemented,
		// Token: 0x04003141 RID: 12609
		BadCommandSequence,
		// Token: 0x04003142 RID: 12610
		MustIssueStartTlsFirst = 530,
		// Token: 0x04003143 RID: 12611
		CommandParameterNotImplemented = 504,
		// Token: 0x04003144 RID: 12612
		MailboxUnavailable = 550,
		// Token: 0x04003145 RID: 12613
		UserNotLocalTryAlternatePath,
		// Token: 0x04003146 RID: 12614
		ExceededStorageAllocation,
		// Token: 0x04003147 RID: 12615
		MailboxNameNotAllowed,
		// Token: 0x04003148 RID: 12616
		TransactionFailed,
		// Token: 0x04003149 RID: 12617
		GeneralFailure = -1
	}
}
