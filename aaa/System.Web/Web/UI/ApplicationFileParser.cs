using System;

namespace System.Web.UI
{
	// Token: 0x02000385 RID: 901
	internal sealed class ApplicationFileParser : TemplateParser
	{
		// Token: 0x06002C20 RID: 11296 RVA: 0x000C555C File Offset: 0x000C455C
		internal ApplicationFileParser()
		{
		}

		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x06002C21 RID: 11297 RVA: 0x000C5564 File Offset: 0x000C4564
		internal override Type DefaultBaseType
		{
			get
			{
				return typeof(HttpApplication);
			}
		}

		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x06002C22 RID: 11298 RVA: 0x000C5570 File Offset: 0x000C4570
		internal override bool FApplicationFile
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000980 RID: 2432
		// (get) Token: 0x06002C23 RID: 11299 RVA: 0x000C5573 File Offset: 0x000C4573
		internal override string DefaultDirectiveName
		{
			get
			{
				return "application";
			}
		}

		// Token: 0x06002C24 RID: 11300 RVA: 0x000C557A File Offset: 0x000C457A
		internal override void CheckObjectTagScope(ref ObjectTagScope scope)
		{
			if (scope == ObjectTagScope.Default)
			{
				scope = ObjectTagScope.AppInstance;
			}
			if (scope == ObjectTagScope.Page)
			{
				throw new HttpException(SR.GetString("Page_scope_in_global_asax"));
			}
		}

		// Token: 0x0400207D RID: 8317
		internal const string defaultDirectiveName = "application";
	}
}
