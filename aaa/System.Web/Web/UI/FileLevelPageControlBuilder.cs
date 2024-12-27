using System;
using System.Collections;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI
{
	// Token: 0x02000427 RID: 1063
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class FileLevelPageControlBuilder : RootBuilder
	{
		// Token: 0x17000B3A RID: 2874
		// (get) Token: 0x0600330B RID: 13067 RVA: 0x000DDC85 File Offset: 0x000DCC85
		internal ICollection ContentBuilderEntries
		{
			get
			{
				return this._contentBuilderEntries;
			}
		}

		// Token: 0x0600330C RID: 13068 RVA: 0x000DDC90 File Offset: 0x000DCC90
		public override void AppendLiteralString(string text)
		{
			if (this._firstLiteralText == null && !Util.IsWhiteSpaceString(text))
			{
				int num = Util.FirstNonWhiteSpaceIndex(text);
				if (num < 0)
				{
					num = 0;
				}
				this._firstLiteralLineNumber = base.Parser._lineNumber - Util.LineCount(text, num, text.Length);
				this._firstLiteralText = text;
				if (this._containsContentPage)
				{
					throw new HttpException(SR.GetString("Only_Content_supported_on_content_page"));
				}
			}
			base.AppendLiteralString(text);
		}

		// Token: 0x0600330D RID: 13069 RVA: 0x000DDD00 File Offset: 0x000DCD00
		public override void AppendSubBuilder(ControlBuilder subBuilder)
		{
			if (subBuilder is ContentBuilderInternal)
			{
				ContentBuilderInternal contentBuilderInternal = (ContentBuilderInternal)subBuilder;
				this._containsContentPage = true;
				if (this._contentBuilderEntries == null)
				{
					this._contentBuilderEntries = new ArrayList();
				}
				if (this._firstLiteralText != null)
				{
					throw new HttpParseException(SR.GetString("Only_Content_supported_on_content_page"), null, base.Parser.CurrentVirtualPath, this._firstLiteralText, this._firstLiteralLineNumber);
				}
				if (this._firstControlBuilder != null)
				{
					base.Parser._lineNumber = this._firstControlBuilder.Line;
					throw new HttpException(SR.GetString("Only_Content_supported_on_content_page"));
				}
				TemplatePropertyEntry templatePropertyEntry = new TemplatePropertyEntry();
				templatePropertyEntry.Filter = contentBuilderInternal.ContentPlaceHolderFilter;
				templatePropertyEntry.Name = contentBuilderInternal.ContentPlaceHolder;
				templatePropertyEntry.Builder = contentBuilderInternal;
				this._contentBuilderEntries.Add(templatePropertyEntry);
			}
			else if (this._firstControlBuilder == null)
			{
				if (this._containsContentPage)
				{
					throw new HttpException(SR.GetString("Only_Content_supported_on_content_page"));
				}
				this._firstControlBuilder = subBuilder;
			}
			base.AppendSubBuilder(subBuilder);
		}

		// Token: 0x0600330E RID: 13070 RVA: 0x000DDDFC File Offset: 0x000DCDFC
		internal override void InitObject(object obj)
		{
			base.InitObject(obj);
			if (this._contentBuilderEntries == null)
			{
				return;
			}
			ICollection filteredPropertyEntrySet = base.GetFilteredPropertyEntrySet(this._contentBuilderEntries);
			foreach (object obj2 in filteredPropertyEntrySet)
			{
				TemplatePropertyEntry templatePropertyEntry = (TemplatePropertyEntry)obj2;
				ContentBuilderInternal contentBuilderInternal = (ContentBuilderInternal)templatePropertyEntry.Builder;
				try
				{
					contentBuilderInternal.SetServiceProvider(base.ServiceProvider);
					this.AddContentTemplate(obj, contentBuilderInternal.ContentPlaceHolder, contentBuilderInternal.BuildObject() as ITemplate);
				}
				finally
				{
					contentBuilderInternal.SetServiceProvider(null);
				}
			}
		}

		// Token: 0x0600330F RID: 13071 RVA: 0x000DDEB0 File Offset: 0x000DCEB0
		internal virtual void AddContentTemplate(object obj, string templateName, ITemplate template)
		{
			Page page = (Page)obj;
			page.AddContentTemplate(templateName, template);
		}

		// Token: 0x06003310 RID: 13072 RVA: 0x000DDECC File Offset: 0x000DCECC
		internal override void SortEntries()
		{
			base.SortEntries();
			ControlBuilder.FilteredPropertyEntryComparer filteredPropertyEntryComparer = null;
			base.ProcessAndSortPropertyEntries(this._contentBuilderEntries, ref filteredPropertyEntryComparer);
		}

		// Token: 0x040023E2 RID: 9186
		private ArrayList _contentBuilderEntries;

		// Token: 0x040023E3 RID: 9187
		private ControlBuilder _firstControlBuilder;

		// Token: 0x040023E4 RID: 9188
		private int _firstLiteralLineNumber;

		// Token: 0x040023E5 RID: 9189
		private bool _containsContentPage;

		// Token: 0x040023E6 RID: 9190
		private string _firstLiteralText;
	}
}
