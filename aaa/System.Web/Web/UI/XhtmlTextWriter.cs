using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x0200048E RID: 1166
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class XhtmlTextWriter : HtmlTextWriter
	{
		// Token: 0x17000C31 RID: 3121
		// (get) Token: 0x060036A5 RID: 13989 RVA: 0x000EB9BB File Offset: 0x000EA9BB
		internal override bool RenderDivAroundHiddenInputs
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060036A6 RID: 13990 RVA: 0x000EB9BE File Offset: 0x000EA9BE
		public XhtmlTextWriter(TextWriter writer)
			: this(writer, "\t")
		{
		}

		// Token: 0x060036A7 RID: 13991 RVA: 0x000EB9CC File Offset: 0x000EA9CC
		public XhtmlTextWriter(TextWriter writer, string tabString)
			: base(writer, tabString)
		{
			this._commonAttributes.Add("class", true);
			this._commonAttributes.Add("id", true);
			this._commonAttributes.Add("title", true);
			this._commonAttributes.Add("xml:lang", true);
			this.AddRecognizedAttributes("head", new string[] { "xml:lang" });
			this._suppressCommonAttributes["head"] = true;
			this.AddRecognizedAttributes("html", new string[] { "xml:lang", "version", "xmlns" });
			this._suppressCommonAttributes["html"] = true;
			this.AddRecognizedAttributes("title", new string[] { "xml:lang" });
			this._suppressCommonAttributes["title"] = true;
			this.AddRecognizedAttributes("blockquote", new string[] { "cite" });
			this.AddRecognizedAttributes("br", new string[] { "class", "id", "title" });
			this._suppressCommonAttributes["br"] = true;
			this.AddRecognizedAttributes("pre", new string[] { "xml:space" });
			this.AddRecognizedAttributes("q", new string[] { "cite" });
			this.AddRecognizedAttributes("a", new string[] { "accesskey", "charset", "href", "hreflang", "rel", "rev", "tabindex", "type", "title" });
			this.AddRecognizedAttributes("form", new string[] { "action", "method", "enctype" });
			this.AddRecognizedAttributes("input", new string[]
			{
				"accesskey", "checked", "maxlength", "name", "size", "src", "tabindex", "type", "value", "title",
				"disabled"
			});
			this.AddRecognizedAttributes("label", new string[] { "accesskey" });
			this.AddRecognizedAttributes("label", new string[] { "for" });
			this.AddRecognizedAttributes("select", new string[] { "multiple", "name", "size", "tabindex", "disabled" });
			this.AddRecognizedAttributes("option", new string[] { "selected", "value" });
			this.AddRecognizedAttributes("textarea", new string[] { "accesskey", "cols", "name", "rows", "tabindex" });
			this.AddRecognizedAttributes("table", new string[] { "summary", "width" });
			this.AddRecognizedAttributes("td", new string[] { "abbr", "align", "axis", "colspan", "headers", "rowspan", "scope", "valign" });
			this.AddRecognizedAttributes("th", new string[] { "abbr", "align", "axis", "colspan", "headers", "rowspan", "scope", "valign" });
			this.AddRecognizedAttributes("tr", new string[] { "align", "valign" });
			this.AddRecognizedAttributes("img", new string[] { "alt", "height", "longdesc", "src", "width" });
			this.AddRecognizedAttributes("object", new string[]
			{
				"archive", "classid", "codebase", "codetype", "data", "declare", "height", "name", "standby", "tabindex",
				"type", "width"
			});
			this.AddRecognizedAttributes("param", new string[] { "id", "name", "type", "value", "valuetype" });
			this.AddRecognizedAttributes("meta", new string[] { "xml:lang", "content", "http-equiv", "name", "scheme" });
			this._suppressCommonAttributes["meta"] = true;
			this.AddRecognizedAttributes("link", new string[] { "charset", "href", "hreflang", "media", "rel", "rev", "type" });
			this.AddRecognizedAttributes("base", new string[] { "href" });
			this._suppressCommonAttributes["base"] = true;
			this.AddRecognizedAttributes("optgroup", new string[] { "disabled", "label" });
			this.AddRecognizedAttributes("ol", new string[] { "start" });
			this.AddRecognizedAttributes("li", new string[] { "value" });
			this.AddRecognizedAttributes("style", new string[] { "xml:lang", "media", "title", "type", "xml:space" });
			this._suppressCommonAttributes["style"] = true;
		}

		// Token: 0x060036A8 RID: 13992 RVA: 0x000EC14C File Offset: 0x000EB14C
		public virtual void AddRecognizedAttribute(string elementName, string attributeName)
		{
			this.AddRecognizedAttributes(elementName, new string[] { attributeName });
		}

		// Token: 0x060036A9 RID: 13993 RVA: 0x000EC16C File Offset: 0x000EB16C
		private void AddRecognizedAttributes(string elementName, params string[] attributes)
		{
			Hashtable hashtable = (Hashtable)this._elementSpecificAttributes[elementName];
			if (hashtable == null)
			{
				hashtable = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
				this._elementSpecificAttributes[elementName] = hashtable;
			}
			foreach (string text in attributes)
			{
				hashtable.Add(text, true);
			}
		}

		// Token: 0x060036AA RID: 13994 RVA: 0x000EC1C8 File Offset: 0x000EB1C8
		public override bool IsValidFormAttribute(string attributeName)
		{
			Hashtable hashtable = (Hashtable)this._elementSpecificAttributes["form"];
			return hashtable != null && hashtable[attributeName] != null;
		}

		// Token: 0x060036AB RID: 13995 RVA: 0x000EC200 File Offset: 0x000EB200
		protected override bool OnAttributeRender(string name, string value, HtmlTextWriterAttribute key)
		{
			return (this._commonAttributes[name] != null && this._suppressCommonAttributes[base.TagName] == null) || (this._elementSpecificAttributes[base.TagName] != null && ((Hashtable)this._elementSpecificAttributes[base.TagName])[name] != null);
		}

		// Token: 0x060036AC RID: 13996 RVA: 0x000EC268 File Offset: 0x000EB268
		protected override bool OnStyleAttributeRender(string name, string value, HtmlTextWriterStyle key)
		{
			return this._docType != XhtmlMobileDocType.XhtmlBasic && (!base.TagName.ToLower(CultureInfo.InvariantCulture).Equals("div") || !name.ToLower(CultureInfo.InvariantCulture).Equals("border-collapse"));
		}

		// Token: 0x060036AD RID: 13997 RVA: 0x000EC2B8 File Offset: 0x000EB2B8
		public virtual void RemoveRecognizedAttribute(string elementName, string attributeName)
		{
			Hashtable hashtable = (Hashtable)this._elementSpecificAttributes[elementName];
			if (hashtable == null)
			{
				hashtable = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
				this._elementSpecificAttributes[elementName] = hashtable;
			}
			if (this._commonAttributes[attributeName] == null || this._suppressCommonAttributes[elementName] != null)
			{
				hashtable.Remove(attributeName);
				return;
			}
			this._suppressCommonAttributes[elementName] = true;
			foreach (object obj in this._commonAttributes.Keys)
			{
				string text = (string)obj;
				if (text != attributeName)
				{
					hashtable.Add(attributeName, true);
				}
			}
		}

		// Token: 0x060036AE RID: 13998 RVA: 0x000EC388 File Offset: 0x000EB388
		public virtual void SetDocType(XhtmlMobileDocType docType)
		{
			this._docType = docType;
			if (docType != XhtmlMobileDocType.XhtmlBasic && this._commonAttributes["style"] == null)
			{
				this._commonAttributes.Add("style", true);
			}
		}

		// Token: 0x060036AF RID: 13999 RVA: 0x000EC3BC File Offset: 0x000EB3BC
		public override void WriteBreak()
		{
			this.WriteFullBeginTag("br/");
		}

		// Token: 0x17000C32 RID: 3122
		// (get) Token: 0x060036B0 RID: 14000 RVA: 0x000EC3C9 File Offset: 0x000EB3C9
		protected Hashtable CommonAttributes
		{
			get
			{
				return this._commonAttributes;
			}
		}

		// Token: 0x17000C33 RID: 3123
		// (get) Token: 0x060036B1 RID: 14001 RVA: 0x000EC3D1 File Offset: 0x000EB3D1
		protected Hashtable ElementSpecificAttributes
		{
			get
			{
				return this._elementSpecificAttributes;
			}
		}

		// Token: 0x17000C34 RID: 3124
		// (get) Token: 0x060036B2 RID: 14002 RVA: 0x000EC3D9 File Offset: 0x000EB3D9
		protected Hashtable SuppressCommonAttributes
		{
			get
			{
				return this._suppressCommonAttributes;
			}
		}

		// Token: 0x040025AB RID: 9643
		private Hashtable _commonAttributes = new Hashtable();

		// Token: 0x040025AC RID: 9644
		private Hashtable _elementSpecificAttributes = new Hashtable(StringComparer.CurrentCultureIgnoreCase);

		// Token: 0x040025AD RID: 9645
		private Hashtable _suppressCommonAttributes = new Hashtable(StringComparer.CurrentCultureIgnoreCase);

		// Token: 0x040025AE RID: 9646
		private XhtmlMobileDocType _docType;
	}
}
