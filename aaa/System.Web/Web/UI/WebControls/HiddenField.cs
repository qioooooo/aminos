using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005B2 RID: 1458
	[PersistChildren(false)]
	[ControlValueProperty("Value")]
	[SupportsEventValidation]
	[Designer("System.Web.UI.Design.WebControls.HiddenFieldDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ParseChildren(true)]
	[NonVisualControl]
	[DefaultProperty("Value")]
	[DefaultEvent("ValueChanged")]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class HiddenField : Control, IPostBackDataHandler
	{
		// Token: 0x1700119A RID: 4506
		// (get) Token: 0x0600475D RID: 18269 RVA: 0x00123EB3 File Offset: 0x00122EB3
		// (set) Token: 0x0600475E RID: 18270 RVA: 0x00123EB8 File Offset: 0x00122EB8
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DefaultValue(false)]
		public override bool EnableTheming
		{
			get
			{
				return false;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("NoThemingSupport", new object[] { base.GetType().Name }));
			}
		}

		// Token: 0x1700119B RID: 4507
		// (get) Token: 0x0600475F RID: 18271 RVA: 0x00123EEA File Offset: 0x00122EEA
		// (set) Token: 0x06004760 RID: 18272 RVA: 0x00123EF4 File Offset: 0x00122EF4
		[DefaultValue("")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override string SkinID
		{
			get
			{
				return string.Empty;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("NoThemingSupport", new object[] { base.GetType().Name }));
			}
		}

		// Token: 0x1700119C RID: 4508
		// (get) Token: 0x06004761 RID: 18273 RVA: 0x00123F28 File Offset: 0x00122F28
		// (set) Token: 0x06004762 RID: 18274 RVA: 0x00123F55 File Offset: 0x00122F55
		[Bindable(true)]
		[DefaultValue("")]
		[WebSysDescription("HiddenField_Value")]
		[WebCategory("Behavior")]
		public virtual string Value
		{
			get
			{
				string text = (string)this.ViewState["Value"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				this.ViewState["Value"] = value;
			}
		}

		// Token: 0x140000CC RID: 204
		// (add) Token: 0x06004763 RID: 18275 RVA: 0x00123F68 File Offset: 0x00122F68
		// (remove) Token: 0x06004764 RID: 18276 RVA: 0x00123F7B File Offset: 0x00122F7B
		[WebCategory("Action")]
		[WebSysDescription("HiddenField_OnValueChanged")]
		public event EventHandler ValueChanged
		{
			add
			{
				base.Events.AddHandler(HiddenField.EventValueChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(HiddenField.EventValueChanged, value);
			}
		}

		// Token: 0x06004765 RID: 18277 RVA: 0x00123F8E File Offset: 0x00122F8E
		protected override ControlCollection CreateControlCollection()
		{
			return new EmptyControlCollection(this);
		}

		// Token: 0x06004766 RID: 18278 RVA: 0x00123F98 File Offset: 0x00122F98
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void Focus()
		{
			throw new NotSupportedException(SR.GetString("NoFocusSupport", new object[] { base.GetType().Name }));
		}

		// Token: 0x06004767 RID: 18279 RVA: 0x00123FCC File Offset: 0x00122FCC
		protected virtual bool LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			base.ValidateEvent(this.UniqueID);
			string value = this.Value;
			string text = postCollection[postDataKey];
			if (!value.Equals(text, StringComparison.Ordinal))
			{
				this.Value = text;
				return true;
			}
			return false;
		}

		// Token: 0x06004768 RID: 18280 RVA: 0x00124008 File Offset: 0x00123008
		protected internal override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			if (!this.SaveValueViewState)
			{
				this.ViewState.SetItemDirty("Value", false);
			}
		}

		// Token: 0x06004769 RID: 18281 RVA: 0x0012402C File Offset: 0x0012302C
		protected virtual void OnValueChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[HiddenField.EventValueChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600476A RID: 18282 RVA: 0x0012405A File Offset: 0x0012305A
		protected virtual void RaisePostDataChangedEvent()
		{
			this.OnValueChanged(EventArgs.Empty);
		}

		// Token: 0x0600476B RID: 18283 RVA: 0x00124068 File Offset: 0x00123068
		protected internal override void Render(HtmlTextWriter writer)
		{
			string uniqueID = this.UniqueID;
			if (this.Page != null)
			{
				this.Page.VerifyRenderingInServerForm(this);
				this.Page.ClientScript.RegisterForEventValidation(uniqueID);
			}
			writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
			if (uniqueID != null)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Name, uniqueID);
			}
			if (this.ID != null)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);
			}
			string value = this.Value;
			if (value.Length > 0)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Value, value);
			}
			writer.RenderBeginTag(HtmlTextWriterTag.Input);
			writer.RenderEndTag();
		}

		// Token: 0x1700119D RID: 4509
		// (get) Token: 0x0600476C RID: 18284 RVA: 0x001240F7 File Offset: 0x001230F7
		private bool SaveValueViewState
		{
			get
			{
				return base.Events[HiddenField.EventValueChanged] != null || !this.Visible || base.GetType() != typeof(HiddenField);
			}
		}

		// Token: 0x0600476D RID: 18285 RVA: 0x00124128 File Offset: 0x00123128
		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			return this.LoadPostData(postDataKey, postCollection);
		}

		// Token: 0x0600476E RID: 18286 RVA: 0x00124132 File Offset: 0x00123132
		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
			this.RaisePostDataChangedEvent();
		}

		// Token: 0x04002A96 RID: 10902
		private static readonly object EventValueChanged = new object();
	}
}
