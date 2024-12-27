using System;
using System.Collections;
using System.IO;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x020003A9 RID: 937
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class ChtmlTextWriter : Html32TextWriter
	{
		// Token: 0x06002DD6 RID: 11734 RVA: 0x000CD571 File Offset: 0x000CC571
		public ChtmlTextWriter(TextWriter writer)
			: this(writer, "\t")
		{
		}

		// Token: 0x06002DD7 RID: 11735 RVA: 0x000CD580 File Offset: 0x000CC580
		public ChtmlTextWriter(TextWriter writer, string tabString)
			: base(writer, tabString)
		{
			this._globalSuppressedAttributes["onclick"] = true;
			this._globalSuppressedAttributes["ondblclick"] = true;
			this._globalSuppressedAttributes["onmousedown"] = true;
			this._globalSuppressedAttributes["onmouseup"] = true;
			this._globalSuppressedAttributes["onmouseover"] = true;
			this._globalSuppressedAttributes["onmousemove"] = true;
			this._globalSuppressedAttributes["onmouseout"] = true;
			this._globalSuppressedAttributes["onkeypress"] = true;
			this._globalSuppressedAttributes["onkeydown"] = true;
			this._globalSuppressedAttributes["onkeyup"] = true;
			this.RemoveRecognizedAttributeInternal("div", "accesskey");
			this.RemoveRecognizedAttributeInternal("div", "cellspacing");
			this.RemoveRecognizedAttributeInternal("div", "cellpadding");
			this.RemoveRecognizedAttributeInternal("div", "gridlines");
			this.RemoveRecognizedAttributeInternal("div", "rules");
			this.RemoveRecognizedAttributeInternal("span", "cellspacing");
			this.RemoveRecognizedAttributeInternal("span", "cellpadding");
			this.RemoveRecognizedAttributeInternal("span", "gridlines");
			this.RemoveRecognizedAttributeInternal("span", "rules");
		}

		// Token: 0x06002DD8 RID: 11736 RVA: 0x000CD734 File Offset: 0x000CC734
		public virtual void AddRecognizedAttribute(string elementName, string attributeName)
		{
			Hashtable hashtable = (Hashtable)this._recognizedAttributes[elementName];
			if (hashtable == null)
			{
				hashtable = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
				this._recognizedAttributes[elementName] = hashtable;
			}
			hashtable.Add(attributeName, true);
		}

		// Token: 0x06002DD9 RID: 11737 RVA: 0x000CD77C File Offset: 0x000CC77C
		protected override bool OnAttributeRender(string name, string value, HtmlTextWriterAttribute key)
		{
			Hashtable hashtable = (Hashtable)this._recognizedAttributes[base.TagName];
			if (hashtable != null && hashtable[name] != null)
			{
				return true;
			}
			if (this._globalSuppressedAttributes[name] != null)
			{
				return false;
			}
			Hashtable hashtable2 = (Hashtable)this._suppressedAttributes[base.TagName];
			return hashtable2 == null || hashtable2[name] == null;
		}

		// Token: 0x06002DDA RID: 11738 RVA: 0x000CD7E4 File Offset: 0x000CC7E4
		protected override bool OnStyleAttributeRender(string name, string value, HtmlTextWriterStyle key)
		{
			return (key != HtmlTextWriterStyle.TextDecoration || !StringUtil.EqualsIgnoreCase("line-through", value)) && base.OnStyleAttributeRender(name, value, key);
		}

		// Token: 0x06002DDB RID: 11739 RVA: 0x000CD803 File Offset: 0x000CC803
		protected override bool OnTagRender(string name, HtmlTextWriterTag key)
		{
			return base.OnTagRender(name, key) && key != HtmlTextWriterTag.Span;
		}

		// Token: 0x06002DDC RID: 11740 RVA: 0x000CD819 File Offset: 0x000CC819
		public virtual void RemoveRecognizedAttribute(string elementName, string attributeName)
		{
			this.RemoveRecognizedAttributeInternal(elementName, attributeName);
		}

		// Token: 0x06002DDD RID: 11741 RVA: 0x000CD824 File Offset: 0x000CC824
		private void RemoveRecognizedAttributeInternal(string elementName, string attributeName)
		{
			Hashtable hashtable = (Hashtable)this._suppressedAttributes[elementName];
			if (hashtable == null)
			{
				hashtable = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
				this._suppressedAttributes[elementName] = hashtable;
			}
			hashtable.Add(attributeName, true);
			hashtable = (Hashtable)this._recognizedAttributes[elementName];
			if (hashtable == null)
			{
				hashtable = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
				this._recognizedAttributes[elementName] = hashtable;
			}
			hashtable.Remove(attributeName);
		}

		// Token: 0x06002DDE RID: 11742 RVA: 0x000CD89F File Offset: 0x000CC89F
		public override void WriteBreak()
		{
			this.Write("<br>");
		}

		// Token: 0x06002DDF RID: 11743 RVA: 0x000CD8AC File Offset: 0x000CC8AC
		public override void WriteEncodedText(string text)
		{
			if (text == null || text.Length == 0)
			{
				return;
			}
			int length = text.Length;
			int num = -1;
			for (int i = 0; i < length; i++)
			{
				int num2 = (int)text[i];
				if (num2 > 160 && num2 < 256)
				{
					if (num != -1)
					{
						base.WriteEncodedText(text.Substring(num, i - num));
						num = -1;
					}
					base.Write(text[i]);
				}
				else if (num == -1)
				{
					num = i;
				}
			}
			if (num != -1)
			{
				if (num == 0)
				{
					base.WriteEncodedText(text);
					return;
				}
				base.WriteEncodedText(text.Substring(num, length - num));
			}
		}

		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x06002DE0 RID: 11744 RVA: 0x000CD93C File Offset: 0x000CC93C
		protected Hashtable RecognizedAttributes
		{
			get
			{
				return this._recognizedAttributes;
			}
		}

		// Token: 0x170009F5 RID: 2549
		// (get) Token: 0x06002DE1 RID: 11745 RVA: 0x000CD944 File Offset: 0x000CC944
		protected Hashtable SuppressedAttributes
		{
			get
			{
				return this._suppressedAttributes;
			}
		}

		// Token: 0x170009F6 RID: 2550
		// (get) Token: 0x06002DE2 RID: 11746 RVA: 0x000CD94C File Offset: 0x000CC94C
		protected Hashtable GlobalSuppressedAttributes
		{
			get
			{
				return this._globalSuppressedAttributes;
			}
		}

		// Token: 0x04002141 RID: 8513
		private Hashtable _recognizedAttributes = new Hashtable(StringComparer.CurrentCultureIgnoreCase);

		// Token: 0x04002142 RID: 8514
		private Hashtable _suppressedAttributes = new Hashtable(StringComparer.CurrentCultureIgnoreCase);

		// Token: 0x04002143 RID: 8515
		private Hashtable _globalSuppressedAttributes = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
	}
}
