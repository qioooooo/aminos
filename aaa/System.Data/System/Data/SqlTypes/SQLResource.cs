using System;

namespace System.Data.SqlTypes
{
	// Token: 0x02000354 RID: 852
	internal sealed class SQLResource
	{
		// Token: 0x06002E92 RID: 11922 RVA: 0x002ADE90 File Offset: 0x002AD290
		private SQLResource()
		{
		}

		// Token: 0x06002E93 RID: 11923 RVA: 0x002ADEA4 File Offset: 0x002AD2A4
		internal static string InvalidOpStreamClosed(string method)
		{
			return Res.GetString("SqlMisc_InvalidOpStreamClosed", new object[] { method });
		}

		// Token: 0x06002E94 RID: 11924 RVA: 0x002ADEC8 File Offset: 0x002AD2C8
		internal static string InvalidOpStreamNonWritable(string method)
		{
			return Res.GetString("SqlMisc_InvalidOpStreamNonWritable", new object[] { method });
		}

		// Token: 0x06002E95 RID: 11925 RVA: 0x002ADEEC File Offset: 0x002AD2EC
		internal static string InvalidOpStreamNonReadable(string method)
		{
			return Res.GetString("SqlMisc_InvalidOpStreamNonReadable", new object[] { method });
		}

		// Token: 0x06002E96 RID: 11926 RVA: 0x002ADF10 File Offset: 0x002AD310
		internal static string InvalidOpStreamNonSeekable(string method)
		{
			return Res.GetString("SqlMisc_InvalidOpStreamNonSeekable", new object[] { method });
		}

		// Token: 0x04001D11 RID: 7441
		internal static readonly string NullString = Res.GetString("SqlMisc_NullString");

		// Token: 0x04001D12 RID: 7442
		internal static readonly string MessageString = Res.GetString("SqlMisc_MessageString");

		// Token: 0x04001D13 RID: 7443
		internal static readonly string ArithOverflowMessage = Res.GetString("SqlMisc_ArithOverflowMessage");

		// Token: 0x04001D14 RID: 7444
		internal static readonly string DivideByZeroMessage = Res.GetString("SqlMisc_DivideByZeroMessage");

		// Token: 0x04001D15 RID: 7445
		internal static readonly string NullValueMessage = Res.GetString("SqlMisc_NullValueMessage");

		// Token: 0x04001D16 RID: 7446
		internal static readonly string TruncationMessage = Res.GetString("SqlMisc_TruncationMessage");

		// Token: 0x04001D17 RID: 7447
		internal static readonly string DateTimeOverflowMessage = Res.GetString("SqlMisc_DateTimeOverflowMessage");

		// Token: 0x04001D18 RID: 7448
		internal static readonly string ConcatDiffCollationMessage = Res.GetString("SqlMisc_ConcatDiffCollationMessage");

		// Token: 0x04001D19 RID: 7449
		internal static readonly string CompareDiffCollationMessage = Res.GetString("SqlMisc_CompareDiffCollationMessage");

		// Token: 0x04001D1A RID: 7450
		internal static readonly string InvalidFlagMessage = Res.GetString("SqlMisc_InvalidFlagMessage");

		// Token: 0x04001D1B RID: 7451
		internal static readonly string NumeToDecOverflowMessage = Res.GetString("SqlMisc_NumeToDecOverflowMessage");

		// Token: 0x04001D1C RID: 7452
		internal static readonly string ConversionOverflowMessage = Res.GetString("SqlMisc_ConversionOverflowMessage");

		// Token: 0x04001D1D RID: 7453
		internal static readonly string InvalidDateTimeMessage = Res.GetString("SqlMisc_InvalidDateTimeMessage");

		// Token: 0x04001D1E RID: 7454
		internal static readonly string TimeZoneSpecifiedMessage = Res.GetString("SqlMisc_TimeZoneSpecifiedMessage");

		// Token: 0x04001D1F RID: 7455
		internal static readonly string InvalidArraySizeMessage = Res.GetString("SqlMisc_InvalidArraySizeMessage");

		// Token: 0x04001D20 RID: 7456
		internal static readonly string InvalidPrecScaleMessage = Res.GetString("SqlMisc_InvalidPrecScaleMessage");

		// Token: 0x04001D21 RID: 7457
		internal static readonly string FormatMessage = Res.GetString("SqlMisc_FormatMessage");

		// Token: 0x04001D22 RID: 7458
		internal static readonly string NotFilledMessage = Res.GetString("SqlMisc_NotFilledMessage");

		// Token: 0x04001D23 RID: 7459
		internal static readonly string AlreadyFilledMessage = Res.GetString("SqlMisc_AlreadyFilledMessage");

		// Token: 0x04001D24 RID: 7460
		internal static readonly string ClosedXmlReaderMessage = Res.GetString("SqlMisc_ClosedXmlReaderMessage");
	}
}
