using System;
using System.Collections;
using System.Globalization;
using System.Web.UI;

namespace System.Web.Compilation
{
	// Token: 0x02000151 RID: 337
	internal class BuildResultNoCompileMasterPage : BuildResultNoCompileUserControl
	{
		// Token: 0x06000F7A RID: 3962 RVA: 0x00045332 File Offset: 0x00044332
		internal BuildResultNoCompileMasterPage(Type baseType, TemplateParser parser)
			: base(baseType, parser)
		{
			this._placeHolderList = ((MasterPageParser)parser).PlaceHolderList;
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x00045350 File Offset: 0x00044350
		public override object CreateInstance()
		{
			MasterPage masterPage = (MasterPage)base.CreateInstance();
			foreach (object obj in this._placeHolderList)
			{
				string text = (string)obj;
				masterPage.ContentPlaceHolders.Add(text.ToLower(CultureInfo.InvariantCulture));
			}
			return masterPage;
		}

		// Token: 0x040015F1 RID: 5617
		private ICollection _placeHolderList;
	}
}
