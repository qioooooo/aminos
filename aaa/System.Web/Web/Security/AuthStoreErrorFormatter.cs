using System;

namespace System.Web.Security
{
	// Token: 0x0200032A RID: 810
	internal sealed class AuthStoreErrorFormatter : ErrorFormatter
	{
		// Token: 0x060027D2 RID: 10194 RVA: 0x000AEB74 File Offset: 0x000ADB74
		internal AuthStoreErrorFormatter()
		{
		}

		// Token: 0x060027D3 RID: 10195 RVA: 0x000AEB7C File Offset: 0x000ADB7C
		internal static string GetErrorText()
		{
			if (AuthStoreErrorFormatter.s_errMsg != null)
			{
				return AuthStoreErrorFormatter.s_errMsg;
			}
			lock (AuthStoreErrorFormatter.s_Lock)
			{
				if (AuthStoreErrorFormatter.s_errMsg != null)
				{
					return AuthStoreErrorFormatter.s_errMsg;
				}
				AuthStoreErrorFormatter authStoreErrorFormatter = new AuthStoreErrorFormatter();
				AuthStoreErrorFormatter.s_errMsg = authStoreErrorFormatter.GetErrorMessage();
			}
			return AuthStoreErrorFormatter.s_errMsg;
		}

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x060027D4 RID: 10196 RVA: 0x000AEBE4 File Offset: 0x000ADBE4
		protected override string ErrorTitle
		{
			get
			{
				return SR.GetString("AuthStoreNotInstalled_Title");
			}
		}

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x060027D5 RID: 10197 RVA: 0x000AEBF0 File Offset: 0x000ADBF0
		protected override string Description
		{
			get
			{
				return SR.GetString("AuthStoreNotInstalled_Description");
			}
		}

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x060027D6 RID: 10198 RVA: 0x000AEBFC File Offset: 0x000ADBFC
		protected override string MiscSectionTitle
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x060027D7 RID: 10199 RVA: 0x000AEBFF File Offset: 0x000ADBFF
		protected override string MiscSectionContent
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x060027D8 RID: 10200 RVA: 0x000AEC02 File Offset: 0x000ADC02
		protected override string ColoredSquareTitle
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x060027D9 RID: 10201 RVA: 0x000AEC05 File Offset: 0x000ADC05
		protected override string ColoredSquareContent
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x060027DA RID: 10202 RVA: 0x000AEC08 File Offset: 0x000ADC08
		protected override bool ShowSourceFileInfo
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04001E6E RID: 7790
		private static string s_errMsg = null;

		// Token: 0x04001E6F RID: 7791
		private static object s_Lock = new object();
	}
}
