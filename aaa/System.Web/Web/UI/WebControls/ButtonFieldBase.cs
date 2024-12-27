using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004D9 RID: 1241
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class ButtonFieldBase : DataControlField
	{
		// Token: 0x17000DB2 RID: 3506
		// (get) Token: 0x06003BA3 RID: 15267 RVA: 0x000FAB20 File Offset: 0x000F9B20
		// (set) Token: 0x06003BA4 RID: 15268 RVA: 0x000FAB4C File Offset: 0x000F9B4C
		[WebCategory("Appearance")]
		[DefaultValue(ButtonType.Link)]
		[WebSysDescription("ButtonFieldBase_ButtonType")]
		public virtual ButtonType ButtonType
		{
			get
			{
				object obj = base.ViewState["ButtonType"];
				if (obj != null)
				{
					return (ButtonType)obj;
				}
				return ButtonType.Link;
			}
			set
			{
				if (value < ButtonType.Button || value > ButtonType.Link)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				object obj = base.ViewState["ButtonType"];
				if (obj == null || (ButtonType)obj != value)
				{
					base.ViewState["ButtonType"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000DB3 RID: 3507
		// (get) Token: 0x06003BA5 RID: 15269 RVA: 0x000FABA8 File Offset: 0x000F9BA8
		// (set) Token: 0x06003BA6 RID: 15270 RVA: 0x000FABD4 File Offset: 0x000F9BD4
		[DefaultValue(false)]
		[WebCategory("Behavior")]
		[WebSysDescription("ButtonFieldBase_CausesValidation")]
		public virtual bool CausesValidation
		{
			get
			{
				object obj = base.ViewState["CausesValidation"];
				return obj != null && (bool)obj;
			}
			set
			{
				object obj = base.ViewState["CausesValidation"];
				if (obj == null || (bool)obj != value)
				{
					base.ViewState["CausesValidation"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000DB4 RID: 3508
		// (get) Token: 0x06003BA7 RID: 15271 RVA: 0x000FAC1C File Offset: 0x000F9C1C
		// (set) Token: 0x06003BA8 RID: 15272 RVA: 0x000FAC48 File Offset: 0x000F9C48
		[WebSysDescription("DataControlField_ShowHeader")]
		[WebCategory("Behavior")]
		[DefaultValue(false)]
		public override bool ShowHeader
		{
			get
			{
				object obj = base.ViewState["ShowHeader"];
				return obj != null && (bool)obj;
			}
			set
			{
				object obj = base.ViewState["ShowHeader"];
				if (obj == null || (bool)obj != value)
				{
					base.ViewState["ShowHeader"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000DB5 RID: 3509
		// (get) Token: 0x06003BA9 RID: 15273 RVA: 0x000FAC90 File Offset: 0x000F9C90
		// (set) Token: 0x06003BAA RID: 15274 RVA: 0x000FACBD File Offset: 0x000F9CBD
		[WebSysDescription("ButtonFieldBase_ValidationGroup")]
		[WebCategory("Behavior")]
		[DefaultValue("")]
		public virtual string ValidationGroup
		{
			get
			{
				object obj = base.ViewState["ValidationGroup"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				if (!object.Equals(value, base.ViewState["ValidationGroup"]))
				{
					base.ViewState["ValidationGroup"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x06003BAB RID: 15275 RVA: 0x000FACEE File Offset: 0x000F9CEE
		protected override void CopyProperties(DataControlField newField)
		{
			((ButtonFieldBase)newField).ButtonType = this.ButtonType;
			((ButtonFieldBase)newField).CausesValidation = this.CausesValidation;
			((ButtonFieldBase)newField).ValidationGroup = this.ValidationGroup;
			base.CopyProperties(newField);
		}
	}
}
