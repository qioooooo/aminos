using System;
using System.Collections;
using System.Web.Util;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000502 RID: 1282
	internal class ContentBuilderInternal : TemplateBuilder
	{
		// Token: 0x17000ED3 RID: 3795
		// (get) Token: 0x06003E9F RID: 16031 RVA: 0x00104E89 File Offset: 0x00103E89
		public override Type BindingContainerType
		{
			get
			{
				return typeof(Control);
			}
		}

		// Token: 0x17000ED4 RID: 3796
		// (get) Token: 0x06003EA0 RID: 16032 RVA: 0x00104E95 File Offset: 0x00103E95
		internal string ContentPlaceHolderFilter
		{
			get
			{
				return this._contentPlaceHolderFilter;
			}
		}

		// Token: 0x17000ED5 RID: 3797
		// (get) Token: 0x06003EA1 RID: 16033 RVA: 0x00104E9D File Offset: 0x00103E9D
		internal string ContentPlaceHolder
		{
			get
			{
				return this._contentPlaceHolder;
			}
		}

		// Token: 0x06003EA2 RID: 16034 RVA: 0x00104EA5 File Offset: 0x00103EA5
		public override object BuildObject()
		{
			if (base.InDesigner)
			{
				return base.BuildObjectInternal();
			}
			return base.BuildObject();
		}

		// Token: 0x06003EA3 RID: 16035 RVA: 0x00104EBC File Offset: 0x00103EBC
		public override void InstantiateIn(Control container)
		{
			base.InstantiateIn(container);
			HttpContext httpContext = HttpContext.Current;
			if (httpContext != null)
			{
				TemplateControl templateControl = httpContext.TemplateControl;
				if (templateControl != null && templateControl.NoCompile)
				{
					foreach (object obj in container.Controls)
					{
						Control control = (Control)obj;
						control.TemplateControl = templateControl;
					}
				}
			}
		}

		// Token: 0x06003EA4 RID: 16036 RVA: 0x00104F3C File Offset: 0x00103F3C
		public override void Init(TemplateParser parser, ControlBuilder parentBuilder, Type type, string tagName, string ID, IDictionary attribs)
		{
			ParsedAttributeCollection parsedAttributeCollection = ControlBuilder.ConvertDictionaryToParsedAttributeCollection(attribs);
			foreach (object obj in parsedAttributeCollection.GetFilteredAttributeDictionaries())
			{
				FilteredAttributeDictionary filteredAttributeDictionary = (FilteredAttributeDictionary)obj;
				string filter = filteredAttributeDictionary.Filter;
				foreach (object obj2 in ((IEnumerable)filteredAttributeDictionary))
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
					if (StringUtil.EqualsIgnoreCase((string)dictionaryEntry.Key, "ContentPlaceHolderID"))
					{
						if (this._contentPlaceHolder != null)
						{
							throw new HttpException(SR.GetString("Content_only_one_contentPlaceHolderID_allowed"));
						}
						dictionaryEntry.Key.ToString();
						this._contentPlaceHolder = dictionaryEntry.Value.ToString();
						this._contentPlaceHolderFilter = filter;
					}
				}
			}
			if (!parser.FInDesigner)
			{
				if (this._contentPlaceHolder == null)
				{
					throw new HttpException(SR.GetString("Control_Missing_Attribute", new object[] { "ContentPlaceHolderID", type.Name }));
				}
				attribs.Clear();
			}
			base.Init(parser, parentBuilder, type, tagName, ID, attribs);
		}

		// Token: 0x06003EA5 RID: 16037 RVA: 0x00105098 File Offset: 0x00104098
		internal override void SetParentBuilder(ControlBuilder parentBuilder)
		{
			if (!base.InDesigner && !(parentBuilder is FileLevelPageControlBuilder))
			{
				throw new HttpException(SR.GetString("Content_allowed_in_top_level_only"));
			}
			base.SetParentBuilder(parentBuilder);
		}

		// Token: 0x0400279F RID: 10143
		private const string _contentPlaceHolderIDPropName = "ContentPlaceHolderID";

		// Token: 0x040027A0 RID: 10144
		private string _contentPlaceHolder;

		// Token: 0x040027A1 RID: 10145
		private string _contentPlaceHolderFilter;
	}
}
