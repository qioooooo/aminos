using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000528 RID: 1320
	[DefaultEvent("ServerValidate")]
	[ToolboxData("<{0}:CustomValidator runat=\"server\" ErrorMessage=\"CustomValidator\"></{0}:CustomValidator>")]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class CustomValidator : BaseValidator
	{
		// Token: 0x17000FB0 RID: 4016
		// (get) Token: 0x0600411C RID: 16668 RVA: 0x0010E8CC File Offset: 0x0010D8CC
		// (set) Token: 0x0600411D RID: 16669 RVA: 0x0010E8F9 File Offset: 0x0010D8F9
		[DefaultValue("")]
		[WebCategory("Behavior")]
		[WebSysDescription("CustomValidator_ClientValidationFunction")]
		[Themeable(false)]
		public string ClientValidationFunction
		{
			get
			{
				object obj = this.ViewState["ClientValidationFunction"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["ClientValidationFunction"] = value;
			}
		}

		// Token: 0x17000FB1 RID: 4017
		// (get) Token: 0x0600411E RID: 16670 RVA: 0x0010E90C File Offset: 0x0010D90C
		// (set) Token: 0x0600411F RID: 16671 RVA: 0x0010E935 File Offset: 0x0010D935
		[DefaultValue(false)]
		[WebSysDescription("CustomValidator_ValidateEmptyText")]
		[WebCategory("Behavior")]
		[Themeable(false)]
		public bool ValidateEmptyText
		{
			get
			{
				object obj = this.ViewState["ValidateEmptyText"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["ValidateEmptyText"] = value;
			}
		}

		// Token: 0x1400008E RID: 142
		// (add) Token: 0x06004120 RID: 16672 RVA: 0x0010E94D File Offset: 0x0010D94D
		// (remove) Token: 0x06004121 RID: 16673 RVA: 0x0010E960 File Offset: 0x0010D960
		[WebSysDescription("CustomValidator_ServerValidate")]
		public event ServerValidateEventHandler ServerValidate
		{
			add
			{
				base.Events.AddHandler(CustomValidator.EventServerValidate, value);
			}
			remove
			{
				base.Events.RemoveHandler(CustomValidator.EventServerValidate, value);
			}
		}

		// Token: 0x06004122 RID: 16674 RVA: 0x0010E974 File Offset: 0x0010D974
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			base.AddAttributesToRender(writer);
			if (base.RenderUplevel)
			{
				string clientID = this.ClientID;
				HtmlTextWriter htmlTextWriter = (base.EnableLegacyRendering ? writer : null);
				base.AddExpandoAttribute(htmlTextWriter, clientID, "evaluationfunction", "CustomValidatorEvaluateIsValid", false);
				if (this.ClientValidationFunction.Length > 0)
				{
					base.AddExpandoAttribute(htmlTextWriter, clientID, "clientvalidationfunction", this.ClientValidationFunction);
					if (this.ValidateEmptyText)
					{
						base.AddExpandoAttribute(htmlTextWriter, clientID, "validateemptytext", "true", false);
					}
				}
			}
		}

		// Token: 0x06004123 RID: 16675 RVA: 0x0010E9F4 File Offset: 0x0010D9F4
		protected override bool ControlPropertiesValid()
		{
			string controlToValidate = base.ControlToValidate;
			if (controlToValidate.Length > 0)
			{
				base.CheckControlValidationProperty(controlToValidate, "ControlToValidate");
			}
			return true;
		}

		// Token: 0x06004124 RID: 16676 RVA: 0x0010EA20 File Offset: 0x0010DA20
		protected override bool EvaluateIsValid()
		{
			string text = string.Empty;
			string controlToValidate = base.ControlToValidate;
			if (controlToValidate.Length > 0)
			{
				text = base.GetControlValidationValue(controlToValidate);
				if ((text == null || text.Trim().Length == 0) && !this.ValidateEmptyText)
				{
					return true;
				}
			}
			return this.OnServerValidate(text);
		}

		// Token: 0x06004125 RID: 16677 RVA: 0x0010EA6C File Offset: 0x0010DA6C
		protected virtual bool OnServerValidate(string value)
		{
			ServerValidateEventHandler serverValidateEventHandler = (ServerValidateEventHandler)base.Events[CustomValidator.EventServerValidate];
			ServerValidateEventArgs serverValidateEventArgs = new ServerValidateEventArgs(value, true);
			if (serverValidateEventHandler != null)
			{
				serverValidateEventHandler(this, serverValidateEventArgs);
				return serverValidateEventArgs.IsValid;
			}
			return true;
		}

		// Token: 0x04002895 RID: 10389
		private static readonly object EventServerValidate = new object();
	}
}
