using System;

namespace Microsoft.VisualBasic
{
	[Flags]
	public enum MsgBoxStyle
	{
		OkOnly = 0,
		OkCancel = 1,
		AbortRetryIgnore = 2,
		YesNoCancel = 3,
		YesNo = 4,
		RetryCancel = 5,
		Critical = 16,
		Question = 32,
		Exclamation = 48,
		Information = 64,
		DefaultButton1 = 0,
		DefaultButton2 = 256,
		DefaultButton3 = 512,
		ApplicationModal = 0,
		SystemModal = 4096,
		MsgBoxHelp = 16384,
		MsgBoxRight = 524288,
		MsgBoxRtlReading = 1048576,
		MsgBoxSetForeground = 65536
	}
}
