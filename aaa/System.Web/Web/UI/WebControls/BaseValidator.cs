using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004C8 RID: 1224
	[Designer("System.Web.UI.Design.WebControls.BaseValidatorDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultProperty("ErrorMessage")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class BaseValidator : Label, IValidator
	{
		// Token: 0x06003A63 RID: 14947 RVA: 0x000F6821 File Offset: 0x000F5821
		protected BaseValidator()
		{
			this.isValid = true;
			this.propertiesChecked = false;
			this.propertiesValid = true;
			this.renderUplevel = false;
			this.ForeColor = Color.Red;
		}

		// Token: 0x17000D49 RID: 3401
		// (get) Token: 0x06003A64 RID: 14948 RVA: 0x000F6850 File Offset: 0x000F5850
		// (set) Token: 0x06003A65 RID: 14949 RVA: 0x000F6858 File Offset: 0x000F5858
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override string AssociatedControlID
		{
			get
			{
				return base.AssociatedControlID;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("Property_Not_Supported", new object[]
				{
					"AssociatedControlID",
					base.GetType().ToString()
				}));
			}
		}

		// Token: 0x17000D4A RID: 3402
		// (get) Token: 0x06003A66 RID: 14950 RVA: 0x000F6892 File Offset: 0x000F5892
		// (set) Token: 0x06003A67 RID: 14951 RVA: 0x000F689A File Offset: 0x000F589A
		[DefaultValue(typeof(Color), "Red")]
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
			}
		}

		// Token: 0x17000D4B RID: 3403
		// (get) Token: 0x06003A68 RID: 14952 RVA: 0x000F68A4 File Offset: 0x000F58A4
		// (set) Token: 0x06003A69 RID: 14953 RVA: 0x000F68D1 File Offset: 0x000F58D1
		[WebCategory("Behavior")]
		[DefaultValue("")]
		[TypeConverter(typeof(ValidatedControlConverter))]
		[Themeable(false)]
		[IDReferenceProperty]
		[WebSysDescription("BaseValidator_ControlToValidate")]
		public string ControlToValidate
		{
			get
			{
				object obj = this.ViewState["ControlToValidate"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["ControlToValidate"] = value;
			}
		}

		// Token: 0x17000D4C RID: 3404
		// (get) Token: 0x06003A6A RID: 14954 RVA: 0x000F68E4 File Offset: 0x000F58E4
		// (set) Token: 0x06003A6B RID: 14955 RVA: 0x000F6911 File Offset: 0x000F5911
		[WebCategory("Appearance")]
		[DefaultValue("")]
		[WebSysDescription("BaseValidator_ErrorMessage")]
		[Localizable(true)]
		public string ErrorMessage
		{
			get
			{
				object obj = this.ViewState["ErrorMessage"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["ErrorMessage"] = value;
			}
		}

		// Token: 0x17000D4D RID: 3405
		// (get) Token: 0x06003A6C RID: 14956 RVA: 0x000F6924 File Offset: 0x000F5924
		// (set) Token: 0x06003A6D RID: 14957 RVA: 0x000F694D File Offset: 0x000F594D
		[Themeable(false)]
		[DefaultValue(true)]
		[WebSysDescription("BaseValidator_EnableClientScript")]
		[WebCategory("Behavior")]
		public bool EnableClientScript
		{
			get
			{
				object obj = this.ViewState["EnableClientScript"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["EnableClientScript"] = value;
			}
		}

		// Token: 0x17000D4E RID: 3406
		// (get) Token: 0x06003A6E RID: 14958 RVA: 0x000F6965 File Offset: 0x000F5965
		// (set) Token: 0x06003A6F RID: 14959 RVA: 0x000F696D File Offset: 0x000F596D
		public override bool Enabled
		{
			get
			{
				return base.Enabled;
			}
			set
			{
				base.Enabled = value;
				if (!value)
				{
					this.isValid = true;
				}
			}
		}

		// Token: 0x17000D4F RID: 3407
		// (get) Token: 0x06003A70 RID: 14960 RVA: 0x000F6980 File Offset: 0x000F5980
		internal override bool IsReloadable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D50 RID: 3408
		// (get) Token: 0x06003A71 RID: 14961 RVA: 0x000F6983 File Offset: 0x000F5983
		// (set) Token: 0x06003A72 RID: 14962 RVA: 0x000F698B File Offset: 0x000F598B
		[DefaultValue(true)]
		[Browsable(false)]
		[WebCategory("Behavior")]
		[Themeable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebSysDescription("BaseValidator_IsValid")]
		public bool IsValid
		{
			get
			{
				return this.isValid;
			}
			set
			{
				this.isValid = value;
			}
		}

		// Token: 0x17000D51 RID: 3409
		// (get) Token: 0x06003A73 RID: 14963 RVA: 0x000F6994 File Offset: 0x000F5994
		protected bool PropertiesValid
		{
			get
			{
				if (!this.propertiesChecked)
				{
					this.propertiesValid = this.ControlPropertiesValid();
					this.propertiesChecked = true;
				}
				return this.propertiesValid;
			}
		}

		// Token: 0x17000D52 RID: 3410
		// (get) Token: 0x06003A74 RID: 14964 RVA: 0x000F69B7 File Offset: 0x000F59B7
		protected bool RenderUplevel
		{
			get
			{
				return this.renderUplevel;
			}
		}

		// Token: 0x17000D53 RID: 3411
		// (get) Token: 0x06003A75 RID: 14965 RVA: 0x000F69C0 File Offset: 0x000F59C0
		// (set) Token: 0x06003A76 RID: 14966 RVA: 0x000F69E9 File Offset: 0x000F59E9
		[WebCategory("Appearance")]
		[WebSysDescription("BaseValidator_Display")]
		[Themeable(true)]
		[DefaultValue(ValidatorDisplay.Static)]
		public ValidatorDisplay Display
		{
			get
			{
				object obj = this.ViewState["Display"];
				if (obj != null)
				{
					return (ValidatorDisplay)obj;
				}
				return ValidatorDisplay.Static;
			}
			set
			{
				if (value < ValidatorDisplay.None || value > ValidatorDisplay.Dynamic)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["Display"] = value;
			}
		}

		// Token: 0x17000D54 RID: 3412
		// (get) Token: 0x06003A77 RID: 14967 RVA: 0x000F6A14 File Offset: 0x000F5A14
		// (set) Token: 0x06003A78 RID: 14968 RVA: 0x000F6A3D File Offset: 0x000F5A3D
		[WebSysDescription("BaseValidator_SetFocusOnError")]
		[DefaultValue(false)]
		[Themeable(false)]
		[WebCategory("Behavior")]
		public bool SetFocusOnError
		{
			get
			{
				object obj = this.ViewState["SetFocusOnError"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["SetFocusOnError"] = value;
			}
		}

		// Token: 0x17000D55 RID: 3413
		// (get) Token: 0x06003A79 RID: 14969 RVA: 0x000F6A55 File Offset: 0x000F5A55
		// (set) Token: 0x06003A7A RID: 14970 RVA: 0x000F6A5D File Offset: 0x000F5A5D
		[PersistenceMode(PersistenceMode.InnerDefaultProperty)]
		[DefaultValue("")]
		[WebCategory("Appearance")]
		[WebSysDescription("BaseValidator_Text")]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		// Token: 0x17000D56 RID: 3414
		// (get) Token: 0x06003A7B RID: 14971 RVA: 0x000F6A68 File Offset: 0x000F5A68
		// (set) Token: 0x06003A7C RID: 14972 RVA: 0x000F6A95 File Offset: 0x000F5A95
		[Themeable(false)]
		[WebCategory("Behavior")]
		[WebSysDescription("BaseValidator_ValidationGroup")]
		[DefaultValue("")]
		public virtual string ValidationGroup
		{
			get
			{
				object obj = this.ViewState["ValidationGroup"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["ValidationGroup"] = value;
			}
		}

		// Token: 0x06003A7D RID: 14973 RVA: 0x000F6AA8 File Offset: 0x000F5AA8
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			bool flag = !this.Enabled;
			if (flag)
			{
				this.Enabled = true;
			}
			try
			{
				if (this.RenderUplevel)
				{
					base.EnsureID();
					string clientID = this.ClientID;
					HtmlTextWriter htmlTextWriter = (base.EnableLegacyRendering ? writer : null);
					if (this.ControlToValidate.Length > 0)
					{
						this.AddExpandoAttribute(htmlTextWriter, clientID, "controltovalidate", this.GetControlRenderID(this.ControlToValidate));
					}
					if (this.SetFocusOnError)
					{
						this.AddExpandoAttribute(htmlTextWriter, clientID, "focusOnError", "t", false);
					}
					if (this.ErrorMessage.Length > 0)
					{
						this.AddExpandoAttribute(htmlTextWriter, clientID, "errormessage", this.ErrorMessage);
					}
					ValidatorDisplay display = this.Display;
					if (display != ValidatorDisplay.Static)
					{
						this.AddExpandoAttribute(htmlTextWriter, clientID, "display", PropertyConverter.EnumToString(typeof(ValidatorDisplay), display), false);
					}
					if (!this.IsValid)
					{
						this.AddExpandoAttribute(htmlTextWriter, clientID, "isvalid", "False", false);
					}
					if (flag)
					{
						this.AddExpandoAttribute(htmlTextWriter, clientID, "enabled", "False", false);
					}
					if (this.ValidationGroup.Length > 0)
					{
						this.AddExpandoAttribute(htmlTextWriter, clientID, "validationGroup", this.ValidationGroup);
					}
				}
				base.AddAttributesToRender(writer);
			}
			finally
			{
				if (flag)
				{
					this.Enabled = false;
				}
			}
		}

		// Token: 0x06003A7E RID: 14974 RVA: 0x000F6C04 File Offset: 0x000F5C04
		internal void AddExpandoAttribute(HtmlTextWriter writer, string controlId, string attributeName, string attributeValue)
		{
			this.AddExpandoAttribute(writer, controlId, attributeName, attributeValue, true);
		}

		// Token: 0x06003A7F RID: 14975 RVA: 0x000F6C12 File Offset: 0x000F5C12
		internal void AddExpandoAttribute(HtmlTextWriter writer, string controlId, string attributeName, string attributeValue, bool encode)
		{
			BaseValidator.AddExpandoAttribute(this, writer, controlId, attributeName, attributeValue, encode);
		}

		// Token: 0x06003A80 RID: 14976 RVA: 0x000F6C24 File Offset: 0x000F5C24
		internal static void AddExpandoAttribute(Control control, HtmlTextWriter writer, string controlId, string attributeName, string attributeValue, bool encode)
		{
			if (writer != null)
			{
				writer.AddAttribute(attributeName, attributeValue, encode);
				return;
			}
			Page page = control.Page;
			if (!page.IsPartialRenderingSupported)
			{
				page.ClientScript.RegisterExpandoAttribute(controlId, attributeName, attributeValue, encode);
				return;
			}
			ValidatorCompatibilityHelper.RegisterExpandoAttribute(control, controlId, attributeName, attributeValue, encode);
		}

		// Token: 0x06003A81 RID: 14977 RVA: 0x000F6C70 File Offset: 0x000F5C70
		protected void CheckControlValidationProperty(string name, string propertyName)
		{
			Control control = this.NamingContainer.FindControl(name);
			if (control == null)
			{
				throw new HttpException(SR.GetString("Validator_control_not_found", new object[] { name, propertyName, this.ID }));
			}
			if (BaseValidator.GetValidationProperty(control) == null)
			{
				throw new HttpException(SR.GetString("Validator_bad_control_type", new object[] { name, propertyName, this.ID }));
			}
		}

		// Token: 0x06003A82 RID: 14978 RVA: 0x000F6CEC File Offset: 0x000F5CEC
		protected virtual bool ControlPropertiesValid()
		{
			string controlToValidate = this.ControlToValidate;
			if (controlToValidate.Length == 0)
			{
				throw new HttpException(SR.GetString("Validator_control_blank", new object[] { this.ID }));
			}
			this.CheckControlValidationProperty(controlToValidate, "ControlToValidate");
			return true;
		}

		// Token: 0x06003A83 RID: 14979 RVA: 0x000F6D38 File Offset: 0x000F5D38
		protected virtual bool DetermineRenderUplevel()
		{
			Page page = this.Page;
			return page != null && page.RequestInternal != null && (this.EnableClientScript && page.Request.Browser.W3CDomVersion.Major >= 1) && page.Request.Browser.EcmaScriptVersion.CompareTo(new Version(1, 2)) >= 0;
		}

		// Token: 0x06003A84 RID: 14980
		protected abstract bool EvaluateIsValid();

		// Token: 0x06003A85 RID: 14981 RVA: 0x000F6DA0 File Offset: 0x000F5DA0
		protected string GetControlRenderID(string name)
		{
			Control control = this.FindControl(name);
			if (control == null)
			{
				return string.Empty;
			}
			return control.ClientID;
		}

		// Token: 0x06003A86 RID: 14982 RVA: 0x000F6DC4 File Offset: 0x000F5DC4
		protected string GetControlValidationValue(string name)
		{
			Control control = this.NamingContainer.FindControl(name);
			if (control == null)
			{
				return null;
			}
			PropertyDescriptor validationProperty = BaseValidator.GetValidationProperty(control);
			if (validationProperty == null)
			{
				return null;
			}
			object value = validationProperty.GetValue(control);
			if (value is ListItem)
			{
				return ((ListItem)value).Value;
			}
			if (value != null)
			{
				return value.ToString();
			}
			return string.Empty;
		}

		// Token: 0x06003A87 RID: 14983 RVA: 0x000F6E1C File Offset: 0x000F5E1C
		public static PropertyDescriptor GetValidationProperty(object component)
		{
			ValidationPropertyAttribute validationPropertyAttribute = (ValidationPropertyAttribute)TypeDescriptor.GetAttributes(component)[typeof(ValidationPropertyAttribute)];
			if (validationPropertyAttribute != null && validationPropertyAttribute.Name != null)
			{
				return TypeDescriptor.GetProperties(component, null)[validationPropertyAttribute.Name];
			}
			return null;
		}

		// Token: 0x06003A88 RID: 14984 RVA: 0x000F6E63 File Offset: 0x000F5E63
		protected internal override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.Page.Validators.Add(this);
		}

		// Token: 0x06003A89 RID: 14985 RVA: 0x000F6E7D File Offset: 0x000F5E7D
		protected internal override void OnUnload(EventArgs e)
		{
			if (this.Page != null)
			{
				this.Page.Validators.Remove(this);
			}
			base.OnUnload(e);
		}

		// Token: 0x06003A8A RID: 14986 RVA: 0x000F6E9F File Offset: 0x000F5E9F
		protected internal override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			this.preRenderCalled = true;
			this.propertiesChecked = false;
			bool flag = this.PropertiesValid;
			this.renderUplevel = this.DetermineRenderUplevel();
			if (this.renderUplevel)
			{
				this.RegisterValidatorCommonScript();
			}
		}

		// Token: 0x06003A8B RID: 14987 RVA: 0x000F6ED8 File Offset: 0x000F5ED8
		protected void RegisterValidatorCommonScript()
		{
			if (this.Page.IsPartialRenderingSupported)
			{
				ValidatorCompatibilityHelper.RegisterClientScriptResource(this, typeof(BaseValidator), "WebUIValidation.js");
				ValidatorCompatibilityHelper.RegisterStartupScript(this, typeof(BaseValidator), "ValidatorIncludeScript", "\r\nvar Page_ValidationActive = false;\r\nif (typeof(ValidatorOnLoad) == \"function\") {\r\n    ValidatorOnLoad();\r\n}\r\n\r\nfunction ValidatorOnSubmit() {\r\n    if (Page_ValidationActive) {\r\n        return ValidatorCommonOnSubmit();\r\n    }\r\n    else {\r\n        return true;\r\n    }\r\n}\r\n        ", true);
				ValidatorCompatibilityHelper.RegisterOnSubmitStatement(this, typeof(BaseValidator), "ValidatorOnSubmit", "if (typeof(ValidatorOnSubmit) == \"function\" && ValidatorOnSubmit() == false) return false;");
				return;
			}
			if (this.Page.ClientScript.IsClientScriptBlockRegistered(typeof(BaseValidator), "ValidatorIncludeScript"))
			{
				return;
			}
			this.Page.ClientScript.RegisterClientScriptResource(typeof(BaseValidator), "WebUIValidation.js");
			this.Page.ClientScript.RegisterStartupScript(typeof(BaseValidator), "ValidatorIncludeScript", "\r\nvar Page_ValidationActive = false;\r\nif (typeof(ValidatorOnLoad) == \"function\") {\r\n    ValidatorOnLoad();\r\n}\r\n\r\nfunction ValidatorOnSubmit() {\r\n    if (Page_ValidationActive) {\r\n        return ValidatorCommonOnSubmit();\r\n    }\r\n    else {\r\n        return true;\r\n    }\r\n}\r\n        ", true);
			this.Page.ClientScript.RegisterOnSubmitStatement(typeof(BaseValidator), "ValidatorOnSubmit", "if (typeof(ValidatorOnSubmit) == \"function\" && ValidatorOnSubmit() == false) return false;");
		}

		// Token: 0x06003A8C RID: 14988 RVA: 0x000F6FCC File Offset: 0x000F5FCC
		protected virtual void RegisterValidatorDeclaration()
		{
			string text = "document.getElementById(\"" + this.ClientID + "\")";
			if (!this.Page.IsPartialRenderingSupported)
			{
				this.Page.ClientScript.RegisterArrayDeclaration("Page_Validators", text);
				return;
			}
			ValidatorCompatibilityHelper.RegisterArrayDeclaration(this, "Page_Validators", text);
			ValidatorCompatibilityHelper.RegisterStartupScript(this, typeof(BaseValidator), this.ClientID + "_DisposeScript", string.Format(CultureInfo.InvariantCulture, "\r\ndocument.getElementById('{0}').dispose = function() {{\r\n    Array.remove({1}, document.getElementById('{0}'));\r\n}}\r\n", new object[] { this.ClientID, "Page_Validators" }), true);
		}

		// Token: 0x06003A8D RID: 14989 RVA: 0x000F7068 File Offset: 0x000F6068
		protected internal override void Render(HtmlTextWriter writer)
		{
			bool flag;
			if (base.DesignMode || (!this.preRenderCalled && this.Page == null))
			{
				this.propertiesChecked = true;
				this.propertiesValid = true;
				this.renderUplevel = false;
				flag = true;
			}
			else
			{
				flag = this.Enabled && !this.IsValid;
			}
			if (!this.PropertiesValid)
			{
				return;
			}
			if (this.Page != null)
			{
				this.Page.VerifyRenderingInServerForm(this);
			}
			ValidatorDisplay display = this.Display;
			bool flag2;
			bool flag3;
			if (this.RenderUplevel)
			{
				flag2 = true;
				flag3 = display != ValidatorDisplay.None;
			}
			else
			{
				flag3 = display != ValidatorDisplay.None && flag;
				flag2 = flag3;
			}
			if (flag2 && this.RenderUplevel)
			{
				this.RegisterValidatorDeclaration();
				if (display == ValidatorDisplay.None || (!flag && display == ValidatorDisplay.Dynamic))
				{
					base.Style["display"] = "none";
				}
				else if (!flag)
				{
					base.Style["visibility"] = "hidden";
				}
			}
			if (flag2)
			{
				this.RenderBeginTag(writer);
			}
			if (flag3)
			{
				if (this.Text.Trim().Length > 0)
				{
					this.RenderContents(writer);
				}
				else if (base.HasRenderingData())
				{
					base.RenderContents(writer);
				}
				else
				{
					writer.Write(this.ErrorMessage);
				}
			}
			else if (!this.RenderUplevel && display == ValidatorDisplay.Static)
			{
				writer.Write("&nbsp;");
			}
			if (flag2)
			{
				this.RenderEndTag(writer);
			}
		}

		// Token: 0x06003A8E RID: 14990 RVA: 0x000F71B0 File Offset: 0x000F61B0
		public void Validate()
		{
			this.IsValid = true;
			if (!this.Visible || !this.Enabled)
			{
				return;
			}
			this.propertiesChecked = false;
			if (!this.PropertiesValid)
			{
				return;
			}
			this.IsValid = this.EvaluateIsValid();
			if (!this.IsValid)
			{
				Page page = this.Page;
				if (page != null && this.SetFocusOnError)
				{
					string text = this.ControlToValidate;
					Control control = this.NamingContainer.FindControl(text);
					if (control != null)
					{
						text = control.ClientID;
					}
					this.Page.SetValidatorInvalidControlFocus(text);
				}
			}
		}

		// Token: 0x04002684 RID: 9860
		private const string ValidatorFileName = "WebUIValidation.js";

		// Token: 0x04002685 RID: 9861
		private const string ValidatorIncludeScriptKey = "ValidatorIncludeScript";

		// Token: 0x04002686 RID: 9862
		private const string ValidatorStartupScript = "\r\nvar Page_ValidationActive = false;\r\nif (typeof(ValidatorOnLoad) == \"function\") {\r\n    ValidatorOnLoad();\r\n}\r\n\r\nfunction ValidatorOnSubmit() {\r\n    if (Page_ValidationActive) {\r\n        return ValidatorCommonOnSubmit();\r\n    }\r\n    else {\r\n        return true;\r\n    }\r\n}\r\n        ";

		// Token: 0x04002687 RID: 9863
		private bool preRenderCalled;

		// Token: 0x04002688 RID: 9864
		private bool isValid;

		// Token: 0x04002689 RID: 9865
		private bool propertiesChecked;

		// Token: 0x0400268A RID: 9866
		private bool propertiesValid;

		// Token: 0x0400268B RID: 9867
		private bool renderUplevel;
	}
}
