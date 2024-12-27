using System;

namespace System.Web
{
	// Token: 0x0200001B RID: 27
	internal class SecurityErrorFormatter : UnhandledErrorFormatter
	{
		// Token: 0x06000095 RID: 149 RVA: 0x000047A6 File Offset: 0x000037A6
		internal SecurityErrorFormatter(Exception e)
			: base(e)
		{
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000096 RID: 150 RVA: 0x000047AF File Offset: 0x000037AF
		protected override string ErrorTitle
		{
			get
			{
				return SR.GetString("Security_Err_Error");
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000097 RID: 151 RVA: 0x000047BB File Offset: 0x000037BB
		protected override string Description
		{
			get
			{
				return HttpUtility.FormatPlainTextAsHtml(SR.GetString("Security_Err_Desc"));
			}
		}
	}
}
