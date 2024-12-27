using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004FC RID: 1276
	[ToolboxData("<{0}:CompareValidator runat=\"server\" ErrorMessage=\"CompareValidator\"></{0}:CompareValidator>")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class CompareValidator : BaseCompareValidator
	{
		// Token: 0x17000EBB RID: 3771
		// (get) Token: 0x06003E5D RID: 15965 RVA: 0x001044EC File Offset: 0x001034EC
		// (set) Token: 0x06003E5E RID: 15966 RVA: 0x00104519 File Offset: 0x00103519
		[TypeConverter(typeof(ValidatedControlConverter))]
		[WebCategory("Behavior")]
		[Themeable(false)]
		[DefaultValue("")]
		[WebSysDescription("CompareValidator_ControlToCompare")]
		public string ControlToCompare
		{
			get
			{
				object obj = this.ViewState["ControlToCompare"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["ControlToCompare"] = value;
			}
		}

		// Token: 0x17000EBC RID: 3772
		// (get) Token: 0x06003E5F RID: 15967 RVA: 0x0010452C File Offset: 0x0010352C
		// (set) Token: 0x06003E60 RID: 15968 RVA: 0x00104555 File Offset: 0x00103555
		[WebCategory("Behavior")]
		[Themeable(false)]
		[DefaultValue(ValidationCompareOperator.Equal)]
		[WebSysDescription("CompareValidator_Operator")]
		public ValidationCompareOperator Operator
		{
			get
			{
				object obj = this.ViewState["Operator"];
				if (obj != null)
				{
					return (ValidationCompareOperator)obj;
				}
				return ValidationCompareOperator.Equal;
			}
			set
			{
				if (value < ValidationCompareOperator.Equal || value > ValidationCompareOperator.DataTypeCheck)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["Operator"] = value;
			}
		}

		// Token: 0x17000EBD RID: 3773
		// (get) Token: 0x06003E61 RID: 15969 RVA: 0x00104580 File Offset: 0x00103580
		// (set) Token: 0x06003E62 RID: 15970 RVA: 0x001045AD File Offset: 0x001035AD
		[DefaultValue("")]
		[WebSysDescription("CompareValidator_ValueToCompare")]
		[WebCategory("Behavior")]
		[Themeable(false)]
		public string ValueToCompare
		{
			get
			{
				object obj = this.ViewState["ValueToCompare"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["ValueToCompare"] = value;
			}
		}

		// Token: 0x06003E63 RID: 15971 RVA: 0x001045C0 File Offset: 0x001035C0
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			base.AddAttributesToRender(writer);
			if (base.RenderUplevel)
			{
				string clientID = this.ClientID;
				HtmlTextWriter htmlTextWriter = (base.EnableLegacyRendering ? writer : null);
				base.AddExpandoAttribute(htmlTextWriter, clientID, "evaluationfunction", "CompareValidatorEvaluateIsValid", false);
				if (this.ControlToCompare.Length > 0)
				{
					string controlRenderID = base.GetControlRenderID(this.ControlToCompare);
					base.AddExpandoAttribute(htmlTextWriter, clientID, "controltocompare", controlRenderID);
					base.AddExpandoAttribute(htmlTextWriter, clientID, "controlhookup", controlRenderID);
				}
				if (this.ValueToCompare.Length > 0)
				{
					string text = this.ValueToCompare;
					if (base.CultureInvariantValues)
					{
						text = base.ConvertCultureInvariantToCurrentCultureFormat(text, base.Type);
					}
					base.AddExpandoAttribute(htmlTextWriter, clientID, "valuetocompare", text);
				}
				if (this.Operator != ValidationCompareOperator.Equal)
				{
					base.AddExpandoAttribute(htmlTextWriter, clientID, "operator", PropertyConverter.EnumToString(typeof(ValidationCompareOperator), this.Operator), false);
				}
			}
		}

		// Token: 0x06003E64 RID: 15972 RVA: 0x001046A8 File Offset: 0x001036A8
		protected override bool ControlPropertiesValid()
		{
			if (this.ControlToCompare.Length > 0)
			{
				base.CheckControlValidationProperty(this.ControlToCompare, "ControlToCompare");
				if (StringUtil.EqualsIgnoreCase(base.ControlToValidate, this.ControlToCompare))
				{
					throw new HttpException(SR.GetString("Validator_bad_compare_control", new object[] { this.ID, this.ControlToCompare }));
				}
			}
			else if (this.Operator != ValidationCompareOperator.DataTypeCheck && !BaseCompareValidator.CanConvert(this.ValueToCompare, base.Type, base.CultureInvariantValues))
			{
				throw new HttpException(SR.GetString("Validator_value_bad_type", new string[]
				{
					this.ValueToCompare,
					"ValueToCompare",
					this.ID,
					PropertyConverter.EnumToString(typeof(ValidationDataType), base.Type)
				}));
			}
			return base.ControlPropertiesValid();
		}

		// Token: 0x06003E65 RID: 15973 RVA: 0x0010478C File Offset: 0x0010378C
		protected override bool EvaluateIsValid()
		{
			string text = base.GetControlValidationValue(base.ControlToValidate);
			if (text.Trim().Length == 0)
			{
				return true;
			}
			bool flag = base.Type == ValidationDataType.Date && !this.DetermineRenderUplevel();
			if (flag && !base.IsInStandardDateFormat(text))
			{
				text = base.ConvertToShortDateString(text);
			}
			bool flag2 = false;
			string text2 = string.Empty;
			if (this.ControlToCompare.Length > 0)
			{
				text2 = base.GetControlValidationValue(this.ControlToCompare);
				if (flag && !base.IsInStandardDateFormat(text2))
				{
					text2 = base.ConvertToShortDateString(text2);
				}
			}
			else
			{
				text2 = this.ValueToCompare;
				flag2 = base.CultureInvariantValues;
			}
			return BaseCompareValidator.Compare(text, false, text2, flag2, this.Operator, base.Type);
		}
	}
}
