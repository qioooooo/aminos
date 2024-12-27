using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004FB RID: 1275
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class CommandField : ButtonFieldBase
	{
		// Token: 0x17000EA7 RID: 3751
		// (get) Token: 0x06003E30 RID: 15920 RVA: 0x00103888 File Offset: 0x00102888
		// (set) Token: 0x06003E31 RID: 15921 RVA: 0x001038B5 File Offset: 0x001028B5
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[WebSysDescription("CommandField_CancelImageUrl")]
		[UrlProperty]
		[WebCategory("Appearance")]
		[DefaultValue("")]
		public virtual string CancelImageUrl
		{
			get
			{
				object obj = base.ViewState["CancelImageUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				if (!object.Equals(value, base.ViewState["CancelImageUrl"]))
				{
					base.ViewState["CancelImageUrl"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000EA8 RID: 3752
		// (get) Token: 0x06003E32 RID: 15922 RVA: 0x001038E8 File Offset: 0x001028E8
		// (set) Token: 0x06003E33 RID: 15923 RVA: 0x0010391A File Offset: 0x0010291A
		[WebSysDescription("CommandField_CancelText")]
		[Localizable(true)]
		[WebSysDefaultValue("CommandField_DefaultCancelCaption")]
		[WebCategory("Appearance")]
		public virtual string CancelText
		{
			get
			{
				object obj = base.ViewState["CancelText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("CommandField_DefaultCancelCaption");
			}
			set
			{
				if (!object.Equals(value, base.ViewState["CancelText"]))
				{
					base.ViewState["CancelText"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000EA9 RID: 3753
		// (get) Token: 0x06003E34 RID: 15924 RVA: 0x0010394C File Offset: 0x0010294C
		// (set) Token: 0x06003E35 RID: 15925 RVA: 0x00103975 File Offset: 0x00102975
		[DefaultValue(true)]
		[WebCategory("Behavior")]
		[WebSysDescription("ButtonFieldBase_CausesValidation")]
		public override bool CausesValidation
		{
			get
			{
				object obj = base.ViewState["CausesValidation"];
				return obj == null || (bool)obj;
			}
			set
			{
				base.CausesValidation = value;
			}
		}

		// Token: 0x17000EAA RID: 3754
		// (get) Token: 0x06003E36 RID: 15926 RVA: 0x00103980 File Offset: 0x00102980
		// (set) Token: 0x06003E37 RID: 15927 RVA: 0x001039AD File Offset: 0x001029AD
		[DefaultValue("")]
		[WebSysDescription("CommandField_DeleteImageUrl")]
		[UrlProperty]
		[WebCategory("Appearance")]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public virtual string DeleteImageUrl
		{
			get
			{
				object obj = base.ViewState["DeleteImageUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				if (!object.Equals(value, base.ViewState["DeleteImageUrl"]))
				{
					base.ViewState["DeleteImageUrl"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000EAB RID: 3755
		// (get) Token: 0x06003E38 RID: 15928 RVA: 0x001039E0 File Offset: 0x001029E0
		// (set) Token: 0x06003E39 RID: 15929 RVA: 0x00103A12 File Offset: 0x00102A12
		[Localizable(true)]
		[WebSysDefaultValue("CommandField_DefaultDeleteCaption")]
		[WebSysDescription("CommandField_DeleteText")]
		[WebCategory("Appearance")]
		public virtual string DeleteText
		{
			get
			{
				object obj = base.ViewState["DeleteText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("CommandField_DefaultDeleteCaption");
			}
			set
			{
				if (!object.Equals(value, base.ViewState["DeleteText"]))
				{
					base.ViewState["DeleteText"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000EAC RID: 3756
		// (get) Token: 0x06003E3A RID: 15930 RVA: 0x00103A44 File Offset: 0x00102A44
		// (set) Token: 0x06003E3B RID: 15931 RVA: 0x00103A71 File Offset: 0x00102A71
		[DefaultValue("")]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[WebSysDescription("CommandField_EditImageUrl")]
		[UrlProperty]
		[WebCategory("Appearance")]
		public virtual string EditImageUrl
		{
			get
			{
				object obj = base.ViewState["EditImageUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				if (!object.Equals(value, base.ViewState["EditImageUrl"]))
				{
					base.ViewState["EditImageUrl"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000EAD RID: 3757
		// (get) Token: 0x06003E3C RID: 15932 RVA: 0x00103AA4 File Offset: 0x00102AA4
		// (set) Token: 0x06003E3D RID: 15933 RVA: 0x00103AD6 File Offset: 0x00102AD6
		[WebSysDescription("CommandField_EditText")]
		[Localizable(true)]
		[WebCategory("Appearance")]
		[WebSysDefaultValue("CommandField_DefaultEditCaption")]
		public virtual string EditText
		{
			get
			{
				object obj = base.ViewState["EditText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("CommandField_DefaultEditCaption");
			}
			set
			{
				if (!object.Equals(value, base.ViewState["EditText"]))
				{
					base.ViewState["EditText"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000EAE RID: 3758
		// (get) Token: 0x06003E3E RID: 15934 RVA: 0x00103B08 File Offset: 0x00102B08
		// (set) Token: 0x06003E3F RID: 15935 RVA: 0x00103B35 File Offset: 0x00102B35
		[WebSysDescription("CommandField_InsertImageUrl")]
		[DefaultValue("")]
		[WebCategory("Appearance")]
		[UrlProperty]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public virtual string InsertImageUrl
		{
			get
			{
				object obj = base.ViewState["InsertImageUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				if (!object.Equals(value, base.ViewState["InsertImageUrl"]))
				{
					base.ViewState["InsertImageUrl"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000EAF RID: 3759
		// (get) Token: 0x06003E40 RID: 15936 RVA: 0x00103B68 File Offset: 0x00102B68
		// (set) Token: 0x06003E41 RID: 15937 RVA: 0x00103B9A File Offset: 0x00102B9A
		[WebCategory("Appearance")]
		[WebSysDefaultValue("CommandField_DefaultInsertCaption")]
		[WebSysDescription("CommandField_InsertText")]
		[Localizable(true)]
		public virtual string InsertText
		{
			get
			{
				object obj = base.ViewState["InsertText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("CommandField_DefaultInsertCaption");
			}
			set
			{
				if (!object.Equals(value, base.ViewState["InsertText"]))
				{
					base.ViewState["InsertText"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000EB0 RID: 3760
		// (get) Token: 0x06003E42 RID: 15938 RVA: 0x00103BCC File Offset: 0x00102BCC
		// (set) Token: 0x06003E43 RID: 15939 RVA: 0x00103BF9 File Offset: 0x00102BF9
		[UrlProperty]
		[DefaultValue("")]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[WebSysDescription("CommandField_NewImageUrl")]
		[WebCategory("Appearance")]
		public virtual string NewImageUrl
		{
			get
			{
				object obj = base.ViewState["NewImageUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				if (!object.Equals(value, base.ViewState["NewImageUrl"]))
				{
					base.ViewState["NewImageUrl"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000EB1 RID: 3761
		// (get) Token: 0x06003E44 RID: 15940 RVA: 0x00103C2C File Offset: 0x00102C2C
		// (set) Token: 0x06003E45 RID: 15941 RVA: 0x00103C5E File Offset: 0x00102C5E
		[Localizable(true)]
		[WebSysDescription("CommandField_NewText")]
		[WebCategory("Appearance")]
		[WebSysDefaultValue("CommandField_DefaultNewCaption")]
		public virtual string NewText
		{
			get
			{
				object obj = base.ViewState["NewText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("CommandField_DefaultNewCaption");
			}
			set
			{
				if (!object.Equals(value, base.ViewState["NewText"]))
				{
					base.ViewState["NewText"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000EB2 RID: 3762
		// (get) Token: 0x06003E46 RID: 15942 RVA: 0x00103C90 File Offset: 0x00102C90
		// (set) Token: 0x06003E47 RID: 15943 RVA: 0x00103CBD File Offset: 0x00102CBD
		[UrlProperty]
		[DefaultValue("")]
		[WebCategory("Appearance")]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[WebSysDescription("CommandField_SelectImageUrl")]
		public virtual string SelectImageUrl
		{
			get
			{
				object obj = base.ViewState["SelectImageUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				if (!object.Equals(value, base.ViewState["SelectImageUrl"]))
				{
					base.ViewState["SelectImageUrl"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000EB3 RID: 3763
		// (get) Token: 0x06003E48 RID: 15944 RVA: 0x00103CF0 File Offset: 0x00102CF0
		// (set) Token: 0x06003E49 RID: 15945 RVA: 0x00103D22 File Offset: 0x00102D22
		[Localizable(true)]
		[WebSysDescription("CommandField_SelectText")]
		[WebCategory("Appearance")]
		[WebSysDefaultValue("CommandField_DefaultSelectCaption")]
		public virtual string SelectText
		{
			get
			{
				object obj = base.ViewState["SelectText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("CommandField_DefaultSelectCaption");
			}
			set
			{
				if (!object.Equals(value, base.ViewState["SelectText"]))
				{
					base.ViewState["SelectText"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000EB4 RID: 3764
		// (get) Token: 0x06003E4A RID: 15946 RVA: 0x00103D54 File Offset: 0x00102D54
		// (set) Token: 0x06003E4B RID: 15947 RVA: 0x00103D80 File Offset: 0x00102D80
		[WebCategory("Behavior")]
		[WebSysDescription("CommandField_ShowCancelButton")]
		[DefaultValue(true)]
		public virtual bool ShowCancelButton
		{
			get
			{
				object obj = base.ViewState["ShowCancelButton"];
				return obj == null || (bool)obj;
			}
			set
			{
				object obj = base.ViewState["ShowCancelButton"];
				if (obj == null || (bool)obj != value)
				{
					base.ViewState["ShowCancelButton"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000EB5 RID: 3765
		// (get) Token: 0x06003E4C RID: 15948 RVA: 0x00103DC8 File Offset: 0x00102DC8
		// (set) Token: 0x06003E4D RID: 15949 RVA: 0x00103DF4 File Offset: 0x00102DF4
		[WebCategory("Behavior")]
		[DefaultValue(false)]
		[WebSysDescription("CommandField_ShowDeleteButton")]
		public virtual bool ShowDeleteButton
		{
			get
			{
				object obj = base.ViewState["ShowDeleteButton"];
				return obj != null && (bool)obj;
			}
			set
			{
				object obj = base.ViewState["ShowDeleteButton"];
				if (obj == null || (bool)obj != value)
				{
					base.ViewState["ShowDeleteButton"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000EB6 RID: 3766
		// (get) Token: 0x06003E4E RID: 15950 RVA: 0x00103E3C File Offset: 0x00102E3C
		// (set) Token: 0x06003E4F RID: 15951 RVA: 0x00103E68 File Offset: 0x00102E68
		[DefaultValue(false)]
		[WebCategory("Behavior")]
		[WebSysDescription("CommandField_ShowEditButton")]
		public virtual bool ShowEditButton
		{
			get
			{
				object obj = base.ViewState["ShowEditButton"];
				return obj != null && (bool)obj;
			}
			set
			{
				object obj = base.ViewState["ShowEditButton"];
				if (obj == null || (bool)obj != value)
				{
					base.ViewState["ShowEditButton"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000EB7 RID: 3767
		// (get) Token: 0x06003E50 RID: 15952 RVA: 0x00103EB0 File Offset: 0x00102EB0
		// (set) Token: 0x06003E51 RID: 15953 RVA: 0x00103EDC File Offset: 0x00102EDC
		[DefaultValue(false)]
		[WebCategory("Behavior")]
		[WebSysDescription("CommandField_ShowSelectButton")]
		public virtual bool ShowSelectButton
		{
			get
			{
				object obj = base.ViewState["ShowSelectButton"];
				return obj != null && (bool)obj;
			}
			set
			{
				object obj = base.ViewState["ShowSelectButton"];
				if (obj == null || (bool)obj != value)
				{
					base.ViewState["ShowSelectButton"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000EB8 RID: 3768
		// (get) Token: 0x06003E52 RID: 15954 RVA: 0x00103F24 File Offset: 0x00102F24
		// (set) Token: 0x06003E53 RID: 15955 RVA: 0x00103F50 File Offset: 0x00102F50
		[WebCategory("Behavior")]
		[DefaultValue(false)]
		[WebSysDescription("CommandField_ShowInsertButton")]
		public virtual bool ShowInsertButton
		{
			get
			{
				object obj = base.ViewState["ShowInsertButton"];
				return obj != null && (bool)obj;
			}
			set
			{
				object obj = base.ViewState["ShowInsertButton"];
				if (obj == null || (bool)obj != value)
				{
					base.ViewState["ShowInsertButton"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000EB9 RID: 3769
		// (get) Token: 0x06003E54 RID: 15956 RVA: 0x00103F98 File Offset: 0x00102F98
		// (set) Token: 0x06003E55 RID: 15957 RVA: 0x00103FC5 File Offset: 0x00102FC5
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[WebCategory("Appearance")]
		[WebSysDescription("CommandField_UpdateImageUrl")]
		[UrlProperty]
		[DefaultValue("")]
		public virtual string UpdateImageUrl
		{
			get
			{
				object obj = base.ViewState["UpdateImageUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				if (!object.Equals(value, base.ViewState["UpdateImageUrl"]))
				{
					base.ViewState["UpdateImageUrl"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000EBA RID: 3770
		// (get) Token: 0x06003E56 RID: 15958 RVA: 0x00103FF8 File Offset: 0x00102FF8
		// (set) Token: 0x06003E57 RID: 15959 RVA: 0x0010402A File Offset: 0x0010302A
		[WebSysDefaultValue("CommandField_DefaultUpdateCaption")]
		[Localizable(true)]
		[WebCategory("Appearance")]
		[WebSysDescription("CommandField_UpdateText")]
		public virtual string UpdateText
		{
			get
			{
				object obj = base.ViewState["UpdateText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("CommandField_DefaultUpdateCaption");
			}
			set
			{
				if (!object.Equals(value, base.ViewState["UpdateText"]))
				{
					base.ViewState["UpdateText"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x06003E58 RID: 15960 RVA: 0x0010405C File Offset: 0x0010305C
		private void AddButtonToCell(DataControlFieldCell cell, string commandName, string buttonText, bool causesValidation, string validationGroup, int rowIndex, string imageUrl)
		{
			IPostBackContainer postBackContainer = base.Control as IPostBackContainer;
			bool flag = true;
			IButtonControl buttonControl;
			switch (this.ButtonType)
			{
			case ButtonType.Button:
				if (postBackContainer != null && !causesValidation)
				{
					buttonControl = new DataControlButton(postBackContainer);
					flag = false;
					goto IL_0083;
				}
				buttonControl = new Button();
				goto IL_0083;
			case ButtonType.Link:
				if (postBackContainer != null && !causesValidation)
				{
					buttonControl = new DataControlLinkButton(postBackContainer);
					flag = false;
					goto IL_0083;
				}
				buttonControl = new DataControlLinkButton(null);
				goto IL_0083;
			}
			if (postBackContainer != null && !causesValidation)
			{
				buttonControl = new DataControlImageButton(postBackContainer);
				flag = false;
			}
			else
			{
				buttonControl = new ImageButton();
			}
			((ImageButton)buttonControl).ImageUrl = imageUrl;
			IL_0083:
			buttonControl.Text = buttonText;
			buttonControl.CommandName = commandName;
			buttonControl.CommandArgument = rowIndex.ToString(CultureInfo.InvariantCulture);
			if (flag)
			{
				buttonControl.CausesValidation = causesValidation;
			}
			buttonControl.ValidationGroup = validationGroup;
			cell.Controls.Add((WebControl)buttonControl);
		}

		// Token: 0x06003E59 RID: 15961 RVA: 0x00104130 File Offset: 0x00103130
		protected override void CopyProperties(DataControlField newField)
		{
			((CommandField)newField).CancelImageUrl = this.CancelImageUrl;
			((CommandField)newField).CancelText = this.CancelText;
			((CommandField)newField).DeleteImageUrl = this.DeleteImageUrl;
			((CommandField)newField).DeleteText = this.DeleteText;
			((CommandField)newField).EditImageUrl = this.EditImageUrl;
			((CommandField)newField).EditText = this.EditText;
			((CommandField)newField).InsertImageUrl = this.InsertImageUrl;
			((CommandField)newField).InsertText = this.InsertText;
			((CommandField)newField).NewImageUrl = this.NewImageUrl;
			((CommandField)newField).NewText = this.NewText;
			((CommandField)newField).SelectImageUrl = this.SelectImageUrl;
			((CommandField)newField).SelectText = this.SelectText;
			((CommandField)newField).UpdateImageUrl = this.UpdateImageUrl;
			((CommandField)newField).UpdateText = this.UpdateText;
			((CommandField)newField).ShowCancelButton = this.ShowCancelButton;
			((CommandField)newField).ShowDeleteButton = this.ShowDeleteButton;
			((CommandField)newField).ShowEditButton = this.ShowEditButton;
			((CommandField)newField).ShowSelectButton = this.ShowSelectButton;
			((CommandField)newField).ShowInsertButton = this.ShowInsertButton;
			base.CopyProperties(newField);
		}

		// Token: 0x06003E5A RID: 15962 RVA: 0x00104287 File Offset: 0x00103287
		protected override DataControlField CreateField()
		{
			return new CommandField();
		}

		// Token: 0x06003E5B RID: 15963 RVA: 0x00104290 File Offset: 0x00103290
		public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
		{
			base.InitializeCell(cell, cellType, rowState, rowIndex);
			bool showEditButton = this.ShowEditButton;
			bool showDeleteButton = this.ShowDeleteButton;
			bool showInsertButton = this.ShowInsertButton;
			bool showSelectButton = this.ShowSelectButton;
			bool showCancelButton = this.ShowCancelButton;
			bool flag = true;
			bool causesValidation = this.CausesValidation;
			string validationGroup = this.ValidationGroup;
			if (cellType == DataControlCellType.DataCell)
			{
				if ((rowState & (DataControlRowState.Edit | DataControlRowState.Insert)) != DataControlRowState.Normal)
				{
					if ((rowState & DataControlRowState.Edit) != DataControlRowState.Normal && showEditButton)
					{
						this.AddButtonToCell(cell, "Update", this.UpdateText, causesValidation, validationGroup, rowIndex, this.UpdateImageUrl);
						if (showCancelButton)
						{
							LiteralControl literalControl = new LiteralControl("&nbsp;");
							cell.Controls.Add(literalControl);
							this.AddButtonToCell(cell, "Cancel", this.CancelText, false, string.Empty, rowIndex, this.CancelImageUrl);
						}
					}
					if ((rowState & DataControlRowState.Insert) != DataControlRowState.Normal && showInsertButton)
					{
						this.AddButtonToCell(cell, "Insert", this.InsertText, causesValidation, validationGroup, rowIndex, this.InsertImageUrl);
						if (showCancelButton)
						{
							LiteralControl literalControl = new LiteralControl("&nbsp;");
							cell.Controls.Add(literalControl);
							this.AddButtonToCell(cell, "Cancel", this.CancelText, false, string.Empty, rowIndex, this.CancelImageUrl);
							return;
						}
					}
				}
				else
				{
					if (showEditButton)
					{
						this.AddButtonToCell(cell, "Edit", this.EditText, false, string.Empty, rowIndex, this.EditImageUrl);
						flag = false;
					}
					if (showDeleteButton)
					{
						if (!flag)
						{
							LiteralControl literalControl = new LiteralControl("&nbsp;");
							cell.Controls.Add(literalControl);
						}
						this.AddButtonToCell(cell, "Delete", this.DeleteText, false, string.Empty, rowIndex, this.DeleteImageUrl);
						flag = false;
					}
					if (showInsertButton)
					{
						if (!flag)
						{
							LiteralControl literalControl = new LiteralControl("&nbsp;");
							cell.Controls.Add(literalControl);
						}
						this.AddButtonToCell(cell, "New", this.NewText, false, string.Empty, rowIndex, this.NewImageUrl);
						flag = false;
					}
					if (showSelectButton)
					{
						if (!flag)
						{
							LiteralControl literalControl = new LiteralControl("&nbsp;");
							cell.Controls.Add(literalControl);
						}
						this.AddButtonToCell(cell, "Select", this.SelectText, false, string.Empty, rowIndex, this.SelectImageUrl);
					}
				}
			}
		}

		// Token: 0x06003E5C RID: 15964 RVA: 0x001044B0 File Offset: 0x001034B0
		public override void ValidateSupportsCallback()
		{
			if (this.ShowSelectButton)
			{
				throw new NotSupportedException(SR.GetString("CommandField_CallbacksNotSupported", new object[] { base.Control.ID }));
			}
		}
	}
}
