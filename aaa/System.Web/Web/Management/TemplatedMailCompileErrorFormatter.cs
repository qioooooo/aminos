using System;
using System.Globalization;

namespace System.Web.Management
{
	// Token: 0x020002D6 RID: 726
	internal class TemplatedMailCompileErrorFormatter : DynamicCompileErrorFormatter
	{
		// Token: 0x0600250A RID: 9482 RVA: 0x0009F074 File Offset: 0x0009E074
		internal TemplatedMailCompileErrorFormatter(HttpCompileException e, int eventsRemaining, bool showDetails)
			: base(e)
		{
			this._eventsRemaining = eventsRemaining;
			this._showDetails = showDetails;
			this._hideDetailedCompilerOutput = true;
			this._dontShowVersion = true;
		}

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x0600250B RID: 9483 RVA: 0x0009F09C File Offset: 0x0009E09C
		protected override string ErrorTitle
		{
			get
			{
				return SR.GetString("MailWebEventProvider_template_compile_error", new object[] { this._eventsRemaining.ToString(CultureInfo.InstalledUICulture) });
			}
		}

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x0600250C RID: 9484 RVA: 0x0009F0CE File Offset: 0x0009E0CE
		protected override string Description
		{
			get
			{
				if (this._showDetails)
				{
					return base.Description;
				}
				return SR.GetString("MailWebEventProvider_template_error_no_details");
			}
		}

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x0600250D RID: 9485 RVA: 0x0009F0E9 File Offset: 0x0009E0E9
		protected override string MiscSectionTitle
		{
			get
			{
				if (this._showDetails)
				{
					return base.MiscSectionTitle;
				}
				return null;
			}
		}

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x0600250E RID: 9486 RVA: 0x0009F0FB File Offset: 0x0009E0FB
		protected override string MiscSectionContent
		{
			get
			{
				if (this._showDetails)
				{
					return base.MiscSectionContent;
				}
				return null;
			}
		}

		// Token: 0x04001CB9 RID: 7353
		private int _eventsRemaining;

		// Token: 0x04001CBA RID: 7354
		private bool _showDetails;
	}
}
