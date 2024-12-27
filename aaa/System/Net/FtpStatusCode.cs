using System;

namespace System.Net
{
	// Token: 0x020003B8 RID: 952
	public enum FtpStatusCode
	{
		// Token: 0x04001DAF RID: 7599
		Undefined,
		// Token: 0x04001DB0 RID: 7600
		RestartMarker = 110,
		// Token: 0x04001DB1 RID: 7601
		ServiceTemporarilyNotAvailable = 120,
		// Token: 0x04001DB2 RID: 7602
		DataAlreadyOpen = 125,
		// Token: 0x04001DB3 RID: 7603
		OpeningData = 150,
		// Token: 0x04001DB4 RID: 7604
		CommandOK = 200,
		// Token: 0x04001DB5 RID: 7605
		CommandExtraneous = 202,
		// Token: 0x04001DB6 RID: 7606
		DirectoryStatus = 212,
		// Token: 0x04001DB7 RID: 7607
		FileStatus,
		// Token: 0x04001DB8 RID: 7608
		SystemType = 215,
		// Token: 0x04001DB9 RID: 7609
		SendUserCommand = 220,
		// Token: 0x04001DBA RID: 7610
		ClosingControl,
		// Token: 0x04001DBB RID: 7611
		ClosingData = 226,
		// Token: 0x04001DBC RID: 7612
		EnteringPassive,
		// Token: 0x04001DBD RID: 7613
		LoggedInProceed = 230,
		// Token: 0x04001DBE RID: 7614
		ServerWantsSecureSession = 234,
		// Token: 0x04001DBF RID: 7615
		FileActionOK = 250,
		// Token: 0x04001DC0 RID: 7616
		PathnameCreated = 257,
		// Token: 0x04001DC1 RID: 7617
		SendPasswordCommand = 331,
		// Token: 0x04001DC2 RID: 7618
		NeedLoginAccount,
		// Token: 0x04001DC3 RID: 7619
		FileCommandPending = 350,
		// Token: 0x04001DC4 RID: 7620
		ServiceNotAvailable = 421,
		// Token: 0x04001DC5 RID: 7621
		CantOpenData = 425,
		// Token: 0x04001DC6 RID: 7622
		ConnectionClosed,
		// Token: 0x04001DC7 RID: 7623
		ActionNotTakenFileUnavailableOrBusy = 450,
		// Token: 0x04001DC8 RID: 7624
		ActionAbortedLocalProcessingError,
		// Token: 0x04001DC9 RID: 7625
		ActionNotTakenInsufficientSpace,
		// Token: 0x04001DCA RID: 7626
		CommandSyntaxError = 500,
		// Token: 0x04001DCB RID: 7627
		ArgumentSyntaxError,
		// Token: 0x04001DCC RID: 7628
		CommandNotImplemented,
		// Token: 0x04001DCD RID: 7629
		BadCommandSequence,
		// Token: 0x04001DCE RID: 7630
		NotLoggedIn = 530,
		// Token: 0x04001DCF RID: 7631
		AccountNeeded = 532,
		// Token: 0x04001DD0 RID: 7632
		ActionNotTakenFileUnavailable = 550,
		// Token: 0x04001DD1 RID: 7633
		ActionAbortedUnknownPageType,
		// Token: 0x04001DD2 RID: 7634
		FileActionAborted,
		// Token: 0x04001DD3 RID: 7635
		ActionNotTakenFilenameNotAllowed
	}
}
