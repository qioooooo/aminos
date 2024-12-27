using System;

namespace System.Web.UI
{
	// Token: 0x02000428 RID: 1064
	internal class FileLevelMasterPageControlBuilder : FileLevelPageControlBuilder
	{
		// Token: 0x06003312 RID: 13074 RVA: 0x000DDEF8 File Offset: 0x000DCEF8
		internal override void AddContentTemplate(object obj, string templateName, ITemplate template)
		{
			MasterPage masterPage = (MasterPage)obj;
			masterPage.AddContentTemplate(templateName, template);
		}
	}
}
