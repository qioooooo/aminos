using System;
using System.ComponentModel;
using System.Data;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000580 RID: 1408
	[DefaultProperty("FormField")]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class FormParameter : Parameter
	{
		// Token: 0x06004506 RID: 17670 RVA: 0x0011B873 File Offset: 0x0011A873
		public FormParameter()
		{
		}

		// Token: 0x06004507 RID: 17671 RVA: 0x0011B87B File Offset: 0x0011A87B
		public FormParameter(string name, string formField)
			: base(name)
		{
			this.FormField = formField;
		}

		// Token: 0x06004508 RID: 17672 RVA: 0x0011B88B File Offset: 0x0011A88B
		public FormParameter(string name, DbType dbType, string formField)
			: base(name, dbType)
		{
			this.FormField = formField;
		}

		// Token: 0x06004509 RID: 17673 RVA: 0x0011B89C File Offset: 0x0011A89C
		public FormParameter(string name, TypeCode type, string formField)
			: base(name, type)
		{
			this.FormField = formField;
		}

		// Token: 0x0600450A RID: 17674 RVA: 0x0011B8AD File Offset: 0x0011A8AD
		protected FormParameter(FormParameter original)
			: base(original)
		{
			this.FormField = original.FormField;
		}

		// Token: 0x170010DF RID: 4319
		// (get) Token: 0x0600450B RID: 17675 RVA: 0x0011B8C4 File Offset: 0x0011A8C4
		// (set) Token: 0x0600450C RID: 17676 RVA: 0x0011B8F1 File Offset: 0x0011A8F1
		[DefaultValue("")]
		[WebSysDescription("FormParameter_FormField")]
		[WebCategory("Parameter")]
		public string FormField
		{
			get
			{
				object obj = base.ViewState["FormField"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
			set
			{
				if (this.FormField != value)
				{
					base.ViewState["FormField"] = value;
					base.OnParameterChanged();
				}
			}
		}

		// Token: 0x0600450D RID: 17677 RVA: 0x0011B918 File Offset: 0x0011A918
		protected override Parameter Clone()
		{
			return new FormParameter(this);
		}

		// Token: 0x0600450E RID: 17678 RVA: 0x0011B920 File Offset: 0x0011A920
		protected override object Evaluate(HttpContext context, Control control)
		{
			if (context == null || context.Request == null)
			{
				return null;
			}
			return context.Request.Form[this.FormField];
		}
	}
}
