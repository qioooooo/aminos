using System;
using System.Globalization;

namespace System.Web.Management
{
	// Token: 0x020002D7 RID: 727
	internal class TemplatedMailRuntimeErrorFormatter : UnhandledErrorFormatter
	{
		// Token: 0x0600250F RID: 9487 RVA: 0x0009F10D File Offset: 0x0009E10D
		internal TemplatedMailRuntimeErrorFormatter(Exception e, int eventsRemaining, bool showDetails)
			: base(e)
		{
			this._eventsRemaining = eventsRemaining;
			this._showDetails = showDetails;
			this._dontShowVersion = true;
		}

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x06002510 RID: 9488 RVA: 0x0009F12C File Offset: 0x0009E12C
		protected override string ErrorTitle
		{
			get
			{
				if (HttpException.GetHttpCodeForException(this.Exception) == 404)
				{
					return SR.GetString("MailWebEventProvider_template_file_not_found_error", new object[] { this._eventsRemaining.ToString(CultureInfo.InstalledUICulture) });
				}
				return SR.GetString("MailWebEventProvider_template_runtime_error", new object[] { this._eventsRemaining.ToString(CultureInfo.InstalledUICulture) });
			}
		}

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x06002511 RID: 9489 RVA: 0x0009F196 File Offset: 0x0009E196
		protected override string ColoredSquareTitle
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x06002512 RID: 9490 RVA: 0x0009F199 File Offset: 0x0009E199
		protected override string ColoredSquareContent
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x06002513 RID: 9491 RVA: 0x0009F19C File Offset: 0x0009E19C
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

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x06002514 RID: 9492 RVA: 0x0009F1B7 File Offset: 0x0009E1B7
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

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x06002515 RID: 9493 RVA: 0x0009F1C9 File Offset: 0x0009E1C9
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

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x06002516 RID: 9494 RVA: 0x0009F1DB File Offset: 0x0009E1DB
		protected override string ColoredSquare2Title
		{
			get
			{
				if (this._showDetails)
				{
					return base.ColoredSquare2Title;
				}
				return null;
			}
		}

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x06002517 RID: 9495 RVA: 0x0009F1ED File Offset: 0x0009E1ED
		protected override string ColoredSquare2Content
		{
			get
			{
				if (this._showDetails)
				{
					return base.ColoredSquare2Content;
				}
				return null;
			}
		}

		// Token: 0x04001CBB RID: 7355
		private int _eventsRemaining;

		// Token: 0x04001CBC RID: 7356
		private bool _showDetails;
	}
}
