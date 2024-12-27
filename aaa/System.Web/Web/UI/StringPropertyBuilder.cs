using System;
using System.Collections;

namespace System.Web.UI
{
	// Token: 0x02000469 RID: 1129
	internal sealed class StringPropertyBuilder : ControlBuilder
	{
		// Token: 0x06003586 RID: 13702 RVA: 0x000E72C8 File Offset: 0x000E62C8
		internal StringPropertyBuilder()
		{
		}

		// Token: 0x06003587 RID: 13703 RVA: 0x000E72D0 File Offset: 0x000E62D0
		internal StringPropertyBuilder(string text)
		{
			this._text = text;
		}

		// Token: 0x17000BFC RID: 3068
		// (get) Token: 0x06003588 RID: 13704 RVA: 0x000E72DF File Offset: 0x000E62DF
		public string Text
		{
			get
			{
				if (this._text != null)
				{
					return this._text;
				}
				return string.Empty;
			}
		}

		// Token: 0x06003589 RID: 13705 RVA: 0x000E72F5 File Offset: 0x000E62F5
		public override void AppendLiteralString(string s)
		{
			if (base.ParentBuilder != null && base.ParentBuilder.HtmlDecodeLiterals())
			{
				s = HttpUtility.HtmlDecode(s);
			}
			this._text = s;
		}

		// Token: 0x0600358A RID: 13706 RVA: 0x000E731C File Offset: 0x000E631C
		public override void AppendSubBuilder(ControlBuilder subBuilder)
		{
			throw new HttpException(SR.GetString("StringPropertyBuilder_CannotHaveChildObjects", new object[]
			{
				base.TagName,
				(base.ParentBuilder != null) ? base.ParentBuilder.TagName : string.Empty
			}));
		}

		// Token: 0x0600358B RID: 13707 RVA: 0x000E7366 File Offset: 0x000E6366
		public override object BuildObject()
		{
			return this.Text;
		}

		// Token: 0x0600358C RID: 13708 RVA: 0x000E736E File Offset: 0x000E636E
		public override void Init(TemplateParser parser, ControlBuilder parentBuilder, Type type, string tagName, string ID, IDictionary attribs)
		{
			base.Init(parser, parentBuilder, type, tagName, ID, attribs);
			base.SetControlType(typeof(string));
		}

		// Token: 0x04002532 RID: 9522
		private string _text;
	}
}
