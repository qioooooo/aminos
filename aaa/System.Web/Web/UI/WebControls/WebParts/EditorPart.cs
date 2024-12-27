using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x0200069E RID: 1694
	[Designer("System.Web.UI.Design.WebControls.WebParts.EditorPartDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[Bindable(false)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class EditorPart : Part
	{
		// Token: 0x17001511 RID: 5393
		// (get) Token: 0x060052D3 RID: 21203 RVA: 0x0014E884 File Offset: 0x0014D884
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool Display
		{
			get
			{
				return base.DesignMode || (this.WebPartToEdit != null && !(this.WebPartToEdit is ProxyWebPart) && (this.WebPartToEdit.AllowEdit || !this.WebPartToEdit.IsShared || this.WebPartManager == null || this.WebPartManager.Personalization.Scope != PersonalizationScope.User));
			}
		}

		// Token: 0x17001512 RID: 5394
		// (get) Token: 0x060052D4 RID: 21204 RVA: 0x0014E8EC File Offset: 0x0014D8EC
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string DisplayTitle
		{
			get
			{
				string text = this.Title;
				if (string.IsNullOrEmpty(text))
				{
					text = SR.GetString("Part_Untitled");
				}
				return text;
			}
		}

		// Token: 0x17001513 RID: 5395
		// (get) Token: 0x060052D5 RID: 21205 RVA: 0x0014E914 File Offset: 0x0014D914
		protected WebPartManager WebPartManager
		{
			get
			{
				return this._webPartManager;
			}
		}

		// Token: 0x17001514 RID: 5396
		// (get) Token: 0x060052D6 RID: 21206 RVA: 0x0014E91C File Offset: 0x0014D91C
		protected WebPart WebPartToEdit
		{
			get
			{
				return this._webPartToEdit;
			}
		}

		// Token: 0x17001515 RID: 5397
		// (get) Token: 0x060052D7 RID: 21207 RVA: 0x0014E924 File Offset: 0x0014D924
		protected EditorZoneBase Zone
		{
			get
			{
				return this._zone;
			}
		}

		// Token: 0x060052D8 RID: 21208
		public abstract bool ApplyChanges();

		// Token: 0x060052D9 RID: 21209 RVA: 0x0014E92C File Offset: 0x0014D92C
		internal string CreateErrorMessage(string exceptionMessage)
		{
			if (this.Context != null && this.Context.IsCustomErrorEnabled)
			{
				return SR.GetString("EditorPart_ErrorSettingProperty");
			}
			return SR.GetString("EditorPart_ErrorSettingPropertyWithExceptionMessage", new object[] { exceptionMessage });
		}

		// Token: 0x060052DA RID: 21210 RVA: 0x0014E970 File Offset: 0x0014D970
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		protected override IDictionary GetDesignModeState()
		{
			IDictionary dictionary = new HybridDictionary(1);
			dictionary["Zone"] = this.Zone;
			return dictionary;
		}

		// Token: 0x060052DB RID: 21211 RVA: 0x0014E998 File Offset: 0x0014D998
		protected internal override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			if (this.Zone == null)
			{
				throw new InvalidOperationException(SR.GetString("EditorPart_MustBeInZone", new object[] { this.ID }));
			}
			if (!this.Display)
			{
				this.Visible = false;
			}
		}

		// Token: 0x060052DC RID: 21212 RVA: 0x0014E9E4 File Offset: 0x0014D9E4
		private void RenderDisplayName(HtmlTextWriter writer, string displayName, string associatedClientID)
		{
			if (this.Zone != null)
			{
				this.Zone.LabelStyle.AddAttributesToRender(writer, this);
			}
			writer.AddAttribute(HtmlTextWriterAttribute.For, associatedClientID);
			writer.RenderBeginTag(HtmlTextWriterTag.Label);
			writer.WriteEncodedText(displayName);
			writer.RenderEndTag();
		}

		// Token: 0x060052DD RID: 21213 RVA: 0x0014EA20 File Offset: 0x0014DA20
		internal void RenderPropertyEditors(HtmlTextWriter writer, string[] propertyDisplayNames, string[] propertyDescriptions, WebControl[] propertyEditors, string[] errorMessages)
		{
			if (propertyDisplayNames.Length == 0)
			{
				return;
			}
			writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "4");
			writer.RenderBeginTag(HtmlTextWriterTag.Table);
			for (int i = 0; i < propertyDisplayNames.Length; i++)
			{
				WebControl webControl = propertyEditors[i];
				if (this.Zone != null && !this.Zone.EditUIStyle.IsEmpty)
				{
					webControl.ApplyStyle(this.Zone.EditUIStyle);
				}
				string text = ((propertyDescriptions != null) ? propertyDescriptions[i] : null);
				if (!string.IsNullOrEmpty(text))
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Title, text);
				}
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				if (webControl is CheckBox)
				{
					webControl.RenderControl(writer);
					writer.Write("&nbsp;");
					this.RenderDisplayName(writer, propertyDisplayNames[i], webControl.ClientID);
				}
				else
				{
					CompositeControl compositeControl = webControl as CompositeControl;
					string text2;
					if (compositeControl != null)
					{
						text2 = compositeControl.Controls[0].ClientID;
					}
					else
					{
						text2 = webControl.ClientID;
					}
					this.RenderDisplayName(writer, propertyDisplayNames[i] + ":", text2);
					writer.WriteBreak();
					writer.WriteLine();
					webControl.RenderControl(writer);
				}
				writer.WriteBreak();
				writer.WriteLine();
				string text3 = errorMessages[i];
				if (!string.IsNullOrEmpty(text3))
				{
					if (this.Zone != null && !this.Zone.ErrorStyle.IsEmpty)
					{
						this.Zone.ErrorStyle.AddAttributesToRender(writer, this);
					}
					writer.RenderBeginTag(HtmlTextWriterTag.Span);
					writer.WriteEncodedText(text3);
					writer.RenderEndTag();
					writer.WriteBreak();
					writer.WriteLine();
				}
				writer.RenderEndTag();
				writer.RenderEndTag();
			}
			writer.RenderEndTag();
		}

		// Token: 0x060052DE RID: 21214 RVA: 0x0014EBBC File Offset: 0x0014DBBC
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		protected override void SetDesignModeState(IDictionary data)
		{
			if (data != null)
			{
				object obj = data["Zone"];
				if (obj != null)
				{
					this.SetZone((EditorZoneBase)obj);
				}
			}
		}

		// Token: 0x060052DF RID: 21215 RVA: 0x0014EBE7 File Offset: 0x0014DBE7
		internal void SetWebPartToEdit(WebPart webPartToEdit)
		{
			this._webPartToEdit = webPartToEdit;
		}

		// Token: 0x060052E0 RID: 21216 RVA: 0x0014EBF0 File Offset: 0x0014DBF0
		internal void SetWebPartManager(WebPartManager webPartManager)
		{
			this._webPartManager = webPartManager;
		}

		// Token: 0x060052E1 RID: 21217 RVA: 0x0014EBF9 File Offset: 0x0014DBF9
		internal void SetZone(EditorZoneBase zone)
		{
			this._zone = zone;
		}

		// Token: 0x060052E2 RID: 21218
		public abstract void SyncChanges();

		// Token: 0x04002E07 RID: 11783
		private WebPart _webPartToEdit;

		// Token: 0x04002E08 RID: 11784
		private WebPartManager _webPartManager;

		// Token: 0x04002E09 RID: 11785
		private EditorZoneBase _zone;
	}
}
