using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Text;

namespace System.Web
{
	// Token: 0x02000022 RID: 34
	internal class ConfigErrorFormatter : FormatterWithFileInfo
	{
		// Token: 0x060000D0 RID: 208 RVA: 0x000054E4 File Offset: 0x000044E4
		internal ConfigErrorFormatter(ConfigurationException e)
			: base(null, e.Filename, null, e.Line)
		{
			this._e = e;
			PerfCounters.IncrementCounter(AppPerfCounter.ERRORS_PRE_PROCESSING);
			PerfCounters.IncrementCounter(AppPerfCounter.ERRORS_TOTAL);
			this._message = HttpUtility.FormatPlainTextAsHtml(e.BareMessage);
			this._adaptiveMiscContent.Add(this._message);
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00005548 File Offset: 0x00004548
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x00005550 File Offset: 0x00004550
		public bool AllowSourceCode
		{
			get
			{
				return this._allowSourceCode;
			}
			set
			{
				this._allowSourceCode = value;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00005559 File Offset: 0x00004559
		protected override Encoding SourceFileEncoding
		{
			get
			{
				return Encoding.UTF8;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00005560 File Offset: 0x00004560
		protected override Exception Exception
		{
			get
			{
				return this._e;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x00005568 File Offset: 0x00004568
		protected override string ErrorTitle
		{
			get
			{
				return SR.GetString("Config_Error");
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00005574 File Offset: 0x00004574
		protected override string Description
		{
			get
			{
				return SR.GetString("Config_Desc");
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00005580 File Offset: 0x00004580
		protected override string MiscSectionTitle
		{
			get
			{
				return SR.GetString("Parser_Error_Message");
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x0000558C File Offset: 0x0000458C
		protected override string MiscSectionContent
		{
			get
			{
				return this._message;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00005594 File Offset: 0x00004594
		protected override string ColoredSquareTitle
		{
			get
			{
				return SR.GetString("Parser_Source_Error");
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060000DA RID: 218 RVA: 0x000055A0 File Offset: 0x000045A0
		protected override StringCollection AdaptiveMiscContent
		{
			get
			{
				return this._adaptiveMiscContent;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060000DB RID: 219 RVA: 0x000055A8 File Offset: 0x000045A8
		protected override string ColoredSquareContent
		{
			get
			{
				if (!this.AllowSourceCode)
				{
					return SR.GetString("Generic_Err_Remote_Desc");
				}
				return base.ColoredSquareContent;
			}
		}

		// Token: 0x04000D38 RID: 3384
		protected string _message;

		// Token: 0x04000D39 RID: 3385
		private Exception _e;

		// Token: 0x04000D3A RID: 3386
		private StringCollection _adaptiveMiscContent = new StringCollection();

		// Token: 0x04000D3B RID: 3387
		private bool _allowSourceCode;
	}
}
