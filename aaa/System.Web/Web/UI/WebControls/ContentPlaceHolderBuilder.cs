using System;
using System.Collections;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000506 RID: 1286
	internal class ContentPlaceHolderBuilder : ControlBuilder
	{
		// Token: 0x17000ED7 RID: 3799
		// (get) Token: 0x06003EB8 RID: 16056 RVA: 0x001051AE File Offset: 0x001041AE
		internal string Name
		{
			get
			{
				return this._templateName;
			}
		}

		// Token: 0x06003EB9 RID: 16057 RVA: 0x001051B8 File Offset: 0x001041B8
		public override void Init(TemplateParser parser, ControlBuilder parentBuilder, Type type, string tagName, string ID, IDictionary attribs)
		{
			this._contentPlaceHolderID = ID;
			if (parser.FInDesigner)
			{
				base.Init(parser, parentBuilder, type, tagName, ID, attribs);
				return;
			}
			if (string.IsNullOrEmpty(ID))
			{
				throw new HttpException(SR.GetString("Control_Missing_Attribute", new object[] { "ID", type.Name }));
			}
			this._templateName = ID;
			MasterPageParser masterPageParser = parser as MasterPageParser;
			if (masterPageParser == null)
			{
				throw new HttpException(SR.GetString("ContentPlaceHolder_only_in_master"));
			}
			base.Init(parser, parentBuilder, type, tagName, ID, attribs);
			if (masterPageParser.PlaceHolderList.Contains(this.Name))
			{
				throw new HttpException(SR.GetString("ContentPlaceHolder_duplicate_contentPlaceHolderID", new object[] { this.Name }));
			}
			masterPageParser.PlaceHolderList.Add(this.Name);
		}

		// Token: 0x06003EBA RID: 16058 RVA: 0x00105290 File Offset: 0x00104290
		public override object BuildObject()
		{
			MasterPage masterPage = base.TemplateControl as MasterPage;
			ContentPlaceHolder contentPlaceHolder = (ContentPlaceHolder)base.BuildObject();
			if (this.PageProvidesMatchingContent(masterPage))
			{
				ITemplate template = (ITemplate)masterPage.ContentTemplates[this._contentPlaceHolderID];
				HttpContext httpContext = HttpContext.Current;
				TemplateControl templateControl = httpContext.TemplateControl;
				httpContext.TemplateControl = masterPage._ownerControl;
				try
				{
					template.InstantiateIn(contentPlaceHolder);
				}
				finally
				{
					httpContext.TemplateControl = templateControl;
				}
			}
			return contentPlaceHolder;
		}

		// Token: 0x06003EBB RID: 16059 RVA: 0x00105314 File Offset: 0x00104314
		internal override void BuildChildren(object parentObj)
		{
			MasterPage masterPage = base.TemplateControl as MasterPage;
			if (this.PageProvidesMatchingContent(masterPage))
			{
				return;
			}
			base.BuildChildren(parentObj);
		}

		// Token: 0x06003EBC RID: 16060 RVA: 0x0010533E File Offset: 0x0010433E
		private bool PageProvidesMatchingContent(MasterPage masterPage)
		{
			return masterPage != null && masterPage.ContentTemplates != null && masterPage.ContentTemplates.Contains(this._contentPlaceHolderID);
		}

		// Token: 0x040027A7 RID: 10151
		private string _contentPlaceHolderID;

		// Token: 0x040027A8 RID: 10152
		private string _templateName;
	}
}
