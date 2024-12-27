using System;

namespace System.Web.Security
{
	// Token: 0x0200034C RID: 844
	internal class PassportAuthFailedErrorFormatter : ErrorFormatter
	{
		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x060028D8 RID: 10456 RVA: 0x000B2FA1 File Offset: 0x000B1FA1
		protected override string ErrorTitle
		{
			get
			{
				return SR.GetString("PassportAuthFailed_Title");
			}
		}

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x060028D9 RID: 10457 RVA: 0x000B2FAD File Offset: 0x000B1FAD
		protected override string Description
		{
			get
			{
				return SR.GetString("PassportAuthFailed_Description");
			}
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x060028DA RID: 10458 RVA: 0x000B2FB9 File Offset: 0x000B1FB9
		protected override string MiscSectionTitle
		{
			get
			{
				return SR.GetString("Assess_Denied_Title");
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x060028DB RID: 10459 RVA: 0x000B2FC5 File Offset: 0x000B1FC5
		protected override string MiscSectionContent
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x060028DC RID: 10460 RVA: 0x000B2FC8 File Offset: 0x000B1FC8
		protected override string ColoredSquareTitle
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x060028DD RID: 10461 RVA: 0x000B2FCB File Offset: 0x000B1FCB
		protected override string ColoredSquareContent
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x060028DE RID: 10462 RVA: 0x000B2FCE File Offset: 0x000B1FCE
		protected override bool ShowSourceFileInfo
		{
			get
			{
				return false;
			}
		}
	}
}
