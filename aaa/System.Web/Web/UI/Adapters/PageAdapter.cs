using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Adapters
{
	// Token: 0x0200043D RID: 1085
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class PageAdapter : ControlAdapter
	{
		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x060033C2 RID: 13250 RVA: 0x000E0FEA File Offset: 0x000DFFEA
		public virtual StringCollection CacheVaryByHeaders
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x060033C3 RID: 13251 RVA: 0x000E0FED File Offset: 0x000DFFED
		public virtual StringCollection CacheVaryByParams
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x060033C4 RID: 13252 RVA: 0x000E0FF0 File Offset: 0x000DFFF0
		protected string ClientState
		{
			get
			{
				if (base.Page != null)
				{
					return base.Page.ClientState;
				}
				return null;
			}
		}

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x060033C5 RID: 13253 RVA: 0x000E1007 File Offset: 0x000E0007
		internal virtual char IdSeparator
		{
			get
			{
				return '$';
			}
		}

		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x060033C6 RID: 13254 RVA: 0x000E100C File Offset: 0x000E000C
		internal string QueryString
		{
			get
			{
				string text = base.Page.ClientQueryString;
				if (base.Page.Request.Browser.RequiresUniqueFilePathSuffix)
				{
					if (!string.IsNullOrEmpty(text))
					{
						text += "&";
					}
					text += base.Page.UniqueFilePathSuffix;
				}
				return text;
			}
		}

		// Token: 0x060033C7 RID: 13255 RVA: 0x000E1063 File Offset: 0x000E0063
		public virtual NameValueCollection DeterminePostBackMode()
		{
			if (base.Control != null)
			{
				return base.Control.Page.DeterminePostBackMode();
			}
			return null;
		}

		// Token: 0x060033C8 RID: 13256 RVA: 0x000E107F File Offset: 0x000E007F
		public virtual ICollection GetRadioButtonsByGroup(string groupName)
		{
			if (this._radioButtonGroups == null)
			{
				return null;
			}
			return (ICollection)this._radioButtonGroups[groupName];
		}

		// Token: 0x060033C9 RID: 13257 RVA: 0x000E109C File Offset: 0x000E009C
		protected internal virtual string GetPostBackFormReference(string formId)
		{
			return "document.forms['" + formId + "']";
		}

		// Token: 0x060033CA RID: 13258 RVA: 0x000E10AE File Offset: 0x000E00AE
		public virtual PageStatePersister GetStatePersister()
		{
			return new HiddenFieldPageStatePersister(base.Page);
		}

		// Token: 0x060033CB RID: 13259 RVA: 0x000E10BC File Offset: 0x000E00BC
		public virtual void RegisterRadioButton(RadioButton radioButton)
		{
			string groupName = radioButton.GroupName;
			if (string.IsNullOrEmpty(groupName))
			{
				return;
			}
			if (this._radioButtonGroups == null)
			{
				this._radioButtonGroups = new ListDictionary();
			}
			ArrayList arrayList;
			if (this._radioButtonGroups.Contains(groupName))
			{
				arrayList = (RadioButtonGroupList)this._radioButtonGroups[groupName];
			}
			else
			{
				arrayList = new RadioButtonGroupList();
				this._radioButtonGroups[groupName] = arrayList;
			}
			arrayList.Add(radioButton);
		}

		// Token: 0x060033CC RID: 13260 RVA: 0x000E112B File Offset: 0x000E012B
		public virtual void RenderBeginHyperlink(HtmlTextWriter writer, string targetUrl, bool encodeUrl, string softkeyLabel)
		{
			this.RenderBeginHyperlink(writer, targetUrl, encodeUrl, softkeyLabel, null);
		}

		// Token: 0x060033CD RID: 13261 RVA: 0x000E113C File Offset: 0x000E013C
		public virtual void RenderBeginHyperlink(HtmlTextWriter writer, string targetUrl, bool encodeUrl, string softkeyLabel, string accessKey)
		{
			if (accessKey != null && accessKey.Length > 1)
			{
				throw new ArgumentOutOfRangeException("accessKey");
			}
			string text;
			if (encodeUrl)
			{
				text = HttpUtility.HtmlAttributeEncode(targetUrl);
			}
			else
			{
				text = targetUrl;
			}
			writer.AddAttribute("href", text);
			if (!string.IsNullOrEmpty(accessKey))
			{
				writer.AddAttribute("accessKey", accessKey);
			}
			writer.RenderBeginTag("a");
		}

		// Token: 0x060033CE RID: 13262 RVA: 0x000E119D File Offset: 0x000E019D
		public virtual void RenderEndHyperlink(HtmlTextWriter writer)
		{
			writer.WriteEndTag("a");
		}

		// Token: 0x060033CF RID: 13263 RVA: 0x000E11AA File Offset: 0x000E01AA
		public virtual void RenderPostBackEvent(HtmlTextWriter writer, string target, string argument, string softkeyLabel, string text)
		{
			this.RenderPostBackEvent(writer, target, argument, softkeyLabel, text, null, null);
		}

		// Token: 0x060033D0 RID: 13264 RVA: 0x000E11BC File Offset: 0x000E01BC
		public virtual void RenderPostBackEvent(HtmlTextWriter writer, string target, string argument, string softkeyLabel, string text, string postUrl, string accessKey)
		{
			this.RenderPostBackEvent(writer, target, argument, softkeyLabel, text, postUrl, accessKey, false);
		}

		// Token: 0x060033D1 RID: 13265 RVA: 0x000E11DC File Offset: 0x000E01DC
		protected void RenderPostBackEvent(HtmlTextWriter writer, string target, string argument, string softkeyLabel, string text, string postUrl, string accessKey, bool encode)
		{
			string text2 = (encode ? "&amp;" : "&");
			bool flag = !string.IsNullOrEmpty(postUrl);
			writer.WriteBeginTag("a");
			writer.Write(" href=\"");
			string text3;
			if (!flag)
			{
				if (base.Browser["requiresAbsolutePostbackUrl"] == "true")
				{
					text3 = base.Page.Response.ApplyAppPathModifier(base.Page.Request.CurrentExecutionFilePath);
				}
				else
				{
					text3 = base.Page.RelativeFilePath;
				}
			}
			else
			{
				text3 = postUrl;
				base.Page.ContainsCrossPagePost = true;
			}
			writer.WriteEncodedUrl(text3);
			writer.Write("?");
			string clientState = this.ClientState;
			if (clientState != null)
			{
				ICollection collection = base.Page.DecomposeViewStateIntoChunks();
				if (collection.Count > 1)
				{
					writer.Write("__VIEWSTATEFIELDCOUNT=" + collection.Count + text2);
				}
				int num = 0;
				foreach (object obj in collection)
				{
					string text4 = (string)obj;
					writer.Write("__VIEWSTATE");
					if (num > 0)
					{
						writer.Write(num.ToString(CultureInfo.CurrentCulture));
					}
					writer.Write("=" + HttpUtility.UrlEncode(text4));
					writer.Write(text2);
					num++;
				}
			}
			if (flag)
			{
				writer.Write("__PREVIOUSPAGE");
				writer.Write("=" + Page.EncryptString(base.Page.Request.CurrentExecutionFilePath));
				writer.Write(text2);
			}
			writer.Write("__EVENTTARGET=" + HttpUtility.UrlEncode(target));
			writer.Write(text2);
			writer.Write("__EVENTARGUMENT=" + HttpUtility.UrlEncode(argument));
			string queryString = this.QueryString;
			if (!string.IsNullOrEmpty(queryString))
			{
				writer.Write(text2);
				writer.Write(queryString);
			}
			writer.Write("\"");
			if (!string.IsNullOrEmpty(accessKey))
			{
				writer.WriteAttribute("accessKey", accessKey);
			}
			writer.Write(">");
			writer.Write(text);
			writer.WriteEndTag("a");
		}

		// Token: 0x060033D2 RID: 13266 RVA: 0x000E1430 File Offset: 0x000E0430
		public virtual string TransformText(string text)
		{
			return text;
		}

		// Token: 0x04002466 RID: 9318
		private IDictionary _radioButtonGroups;
	}
}
