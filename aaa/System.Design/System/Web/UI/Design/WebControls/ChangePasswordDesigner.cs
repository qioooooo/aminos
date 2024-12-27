using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Design;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000402 RID: 1026
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ChangePasswordDesigner : ControlDesigner
	{
		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x0600259D RID: 9629 RVA: 0x000CB060 File Offset: 0x000CA060
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection designerActionListCollection = new DesignerActionListCollection();
				designerActionListCollection.AddRange(base.ActionLists);
				designerActionListCollection.Add(new ChangePasswordDesigner.ChangePasswordDesignerActionList(this));
				return designerActionListCollection;
			}
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x0600259E RID: 9630 RVA: 0x000CB095 File Offset: 0x000CA095
		public override DesignerAutoFormatCollection AutoFormats
		{
			get
			{
				if (ChangePasswordDesigner._autoFormats == null)
				{
					ChangePasswordDesigner._autoFormats = ControlDesigner.CreateAutoFormats("<Schemes>\r\n<xsd:schema id=\"Schemes\" xmlns=\"\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\">\r\n  <xsd:element name=\"Scheme\">\r\n     <xsd:complexType>\r\n       <xsd:all>\r\n        <xsd:element name=\"SchemeName\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"BackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"ForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"BorderColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"BorderWidth\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"BorderStyle\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"BorderPadding\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"FontSize\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"FontName\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"TitleTextBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"TitleTextForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"TitleTextFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"TitleTextFontSize\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"PasswordHintForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"PasswordHintFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"InstructionTextForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"InstructionTextFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"TextboxFontSize\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"ButtonBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"ButtonForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"ButtonFontSize\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"ButtonFontName\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"ButtonBorderColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"ButtonBorderWidth\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"ButtonBorderStyle\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n      </xsd:all>\r\n    </xsd:complexType>\r\n  </xsd:element>\r\n  <xsd:element name=\"Schemes\" msdata:IsDataSet=\"true\">\r\n    <xsd:complexType>\r\n      <xsd:choice maxOccurs=\"unbounded\">\r\n        <xsd:element ref=\"Scheme\"/>\r\n      </xsd:choice>\r\n    </xsd:complexType>\r\n  </xsd:element>\r\n</xsd:schema>\r\n<Scheme>\r\n  <SchemeName>ChangePasswordScheme_Empty</SchemeName>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>ChangePasswordScheme_Elegant</SchemeName>\r\n  <BackColor>#F7F7DE</BackColor>\r\n  <BorderColor>#CCCC99</BorderColor>\r\n  <BorderWidth>1</BorderWidth>\r\n  <BorderStyle>4</BorderStyle>\r\n  <FontSize>10</FontSize>\r\n  <FontName>Verdana</FontName>\r\n  <TitleTextBackColor>#6B696B</TitleTextBackColor>\r\n  <TitleTextForeColor>#FFFFFF</TitleTextForeColor>\r\n  <TitleTextFont>1</TitleTextFont>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>ChangePasswordScheme_Professional</SchemeName>\r\n  <BackColor>#F7F6F3</BackColor>\r\n  <ForeColor>#333333</ForeColor>\r\n  <BorderColor>#E6E2D8</BorderColor>\r\n  <BorderWidth>1</BorderWidth>\r\n  <BorderStyle>4</BorderStyle>\r\n  <BorderPadding>4</BorderPadding>\r\n  <FontSize>0.8em</FontSize>\r\n  <FontName>Verdana</FontName>\r\n  <TitleTextBackColor>#5D7B9D</TitleTextBackColor>\r\n  <TitleTextForeColor>White</TitleTextForeColor>\r\n  <TitleTextFont>1</TitleTextFont>\r\n  <TitleTextFontSize>0.9em</TitleTextFontSize>\r\n  <InstructionTextForeColor>Black</InstructionTextForeColor>\r\n  <InstructionTextFont>2</InstructionTextFont>\r\n  <PasswordHintForeColor>#888888</PasswordHintForeColor>\r\n  <PasswordHintFont>2</PasswordHintFont>\r\n  <TextboxFontSize>0.8em</TextboxFontSize>\r\n  <ButtonBackColor>#FFFBFF</ButtonBackColor>\r\n  <ButtonForeColor>#284775</ButtonForeColor>\r\n  <ButtonFontSize>0.8em</ButtonFontSize>\r\n  <ButtonFontName>Verdana</ButtonFontName>\r\n  <ButtonBorderColor>#CCCCCC</ButtonBorderColor>\r\n  <ButtonBorderWidth>1</ButtonBorderWidth>\r\n  <ButtonBorderStyle>4</ButtonBorderStyle>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>ChangePasswordScheme_Simple</SchemeName>\r\n  <BackColor>#E3EAEB</BackColor>\r\n  <ForeColor>#333333</ForeColor>\r\n  <BorderColor>#E6E2D8</BorderColor>\r\n  <BorderWidth>1</BorderWidth>\r\n  <BorderStyle>4</BorderStyle>\r\n  <BorderPadding>4</BorderPadding>\r\n  <FontSize>0.8em</FontSize>\r\n  <FontName>Verdana</FontName>\r\n  <TitleTextBackColor>#1C5E55</TitleTextBackColor>\r\n  <TitleTextForeColor>White</TitleTextForeColor>\r\n  <TitleTextFont>1</TitleTextFont>\r\n  <TitleTextFontSize>0.9em</TitleTextFontSize>\r\n  <InstructionTextForeColor>Black</InstructionTextForeColor>\r\n  <InstructionTextFont>2</InstructionTextFont>\r\n  <TextboxFontSize>0.8em</TextboxFontSize>\r\n  <ButtonBackColor>White</ButtonBackColor>\r\n  <ButtonForeColor>#1C5E55</ButtonForeColor>\r\n  <ButtonFontSize>0.8em</ButtonFontSize>\r\n  <ButtonFontName>Verdana</ButtonFontName>\r\n  <ButtonBorderColor>#C5BBAF</ButtonBorderColor>\r\n  <ButtonBorderWidth>1</ButtonBorderWidth>\r\n  <ButtonBorderStyle>4</ButtonBorderStyle>\r\n  <PasswordHintForeColor>#1C5E55</PasswordHintForeColor>\r\n  <PasswordHintFont>2</PasswordHintFont>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>ChangePasswordScheme_Classic</SchemeName>\r\n  <BackColor>#EFF3FB</BackColor>\r\n  <ForeColor>#333333</ForeColor>\r\n  <BorderColor>#B5C7DE</BorderColor>\r\n  <BorderWidth>1</BorderWidth>\r\n  <BorderStyle>4</BorderStyle>\r\n  <BorderPadding>4</BorderPadding>\r\n  <FontSize>0.8em</FontSize>\r\n  <FontName>Verdana</FontName>\r\n  <TitleTextBackColor>#507CD1</TitleTextBackColor>\r\n  <TitleTextForeColor>White</TitleTextForeColor>\r\n  <TitleTextFont>1</TitleTextFont>\r\n  <TitleTextFontSize>0.9em</TitleTextFontSize>\r\n  <InstructionTextForeColor>Black</InstructionTextForeColor>\r\n  <InstructionTextFont>2</InstructionTextFont>\r\n  <TextboxFontSize>0.8em</TextboxFontSize>\r\n  <ButtonBackColor>White</ButtonBackColor>\r\n  <ButtonForeColor>#284E98</ButtonForeColor>\r\n  <ButtonFontSize>0.8em</ButtonFontSize>\r\n  <ButtonFontName>Verdana</ButtonFontName>\r\n  <ButtonBorderColor>#507CD1</ButtonBorderColor>\r\n  <ButtonBorderWidth>1</ButtonBorderWidth>\r\n  <ButtonBorderStyle>4</ButtonBorderStyle>\r\n  <PasswordHintForeColor>#507CD1</PasswordHintForeColor>\r\n  <PasswordHintFont>2</PasswordHintFont>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>ChangePasswordScheme_Colorful</SchemeName>\r\n  <BackColor>#FFFBD6</BackColor>\r\n  <ForeColor>#333333</ForeColor>\r\n  <BorderColor>#FFDFAD</BorderColor>\r\n  <BorderWidth>1</BorderWidth>\r\n  <BorderStyle>4</BorderStyle>\r\n  <BorderPadding>4</BorderPadding>\r\n  <FontSize>0.8em</FontSize>\r\n  <FontName>Verdana</FontName>\r\n  <TitleTextBackColor>#990000</TitleTextBackColor>\r\n  <TitleTextForeColor>White</TitleTextForeColor>\r\n  <TitleTextFont>1</TitleTextFont>\r\n  <TitleTextFontSize>0.9em</TitleTextFontSize>\r\n  <InstructionTextForeColor>Black</InstructionTextForeColor>\r\n  <InstructionTextFont>2</InstructionTextFont>\r\n  <TextboxFontSize>0.8em</TextboxFontSize>\r\n  <ButtonBackColor>White</ButtonBackColor>\r\n  <ButtonForeColor>#990000</ButtonForeColor>\r\n  <ButtonFontSize>0.8em</ButtonFontSize>\r\n  <ButtonFontName>Verdana</ButtonFontName>\r\n  <ButtonBorderColor>#CC9966</ButtonBorderColor>\r\n  <ButtonBorderWidth>1</ButtonBorderWidth>\r\n  <ButtonBorderStyle>4</ButtonBorderStyle>\r\n  <PasswordHintForeColor>#888888</PasswordHintForeColor>\r\n  <PasswordHintFont>2</PasswordHintFont>\r\n</Scheme>\r\n</Schemes>\r\n", (DataRow schemeData) => new ChangePasswordAutoFormat(schemeData));
				}
				return ChangePasswordDesigner._autoFormats;
			}
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x0600259F RID: 9631 RVA: 0x000CB0D0 File Offset: 0x000CA0D0
		// (set) Token: 0x060025A0 RID: 9632 RVA: 0x000CB0F9 File Offset: 0x000CA0F9
		private ChangePasswordDesigner.ViewType CurrentView
		{
			get
			{
				object obj = base.DesignerState["CurrentView"];
				if (obj != null)
				{
					return (ChangePasswordDesigner.ViewType)obj;
				}
				return ChangePasswordDesigner.ViewType.ChangePassword;
			}
			set
			{
				base.DesignerState["CurrentView"] = value;
			}
		}

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x060025A1 RID: 9633 RVA: 0x000CB111 File Offset: 0x000CA111
		private bool Templated
		{
			get
			{
				return this.GetTemplate(this._changePassword) != null;
			}
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x060025A2 RID: 9634 RVA: 0x000CB128 File Offset: 0x000CA128
		private PropertyDescriptor TemplateDescriptor
		{
			get
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(base.Component);
				string text = ChangePasswordDesigner._templateNames[(int)this.CurrentView];
				return properties.Find(text, false);
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x060025A3 RID: 9635 RVA: 0x000CB158 File Offset: 0x000CA158
		private TemplateDefinition TemplateDefinition
		{
			get
			{
				string text = ChangePasswordDesigner._templateNames[(int)this.CurrentView];
				return new TemplateDefinition(this, text, this._changePassword, text, ((WebControl)base.ViewControl).ControlStyle);
			}
		}

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x060025A4 RID: 9636 RVA: 0x000CB190 File Offset: 0x000CA190
		public override TemplateGroupCollection TemplateGroups
		{
			get
			{
				TemplateGroupCollection templateGroups = base.TemplateGroups;
				TemplateGroupCollection templateGroupCollection = new TemplateGroupCollection();
				for (int i = 0; i < ChangePasswordDesigner._templateNames.Length; i++)
				{
					string text = ChangePasswordDesigner._templateNames[i];
					TemplateGroup templateGroup = new TemplateGroup(text, ((WebControl)base.ViewControl).ControlStyle);
					templateGroup.AddTemplateDefinition(new TemplateDefinition(this, text, this._changePassword, text, ((WebControl)base.ViewControl).ControlStyle));
					templateGroupCollection.Add(templateGroup);
				}
				templateGroups.AddRange(templateGroupCollection);
				return templateGroups;
			}
		}

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x060025A5 RID: 9637 RVA: 0x000CB212 File Offset: 0x000CA212
		protected override bool UsePreviewControl
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060025A6 RID: 9638 RVA: 0x000CB218 File Offset: 0x000CA218
		private bool ConvertToTemplateChangeCallback(object context)
		{
			bool flag;
			try
			{
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				ChangePasswordDesigner.ConvertToTemplateHelper convertToTemplateHelper = new ChangePasswordDesigner.ConvertToTemplateHelper(this, designerHost);
				ITemplate template = convertToTemplateHelper.ConvertToTemplate();
				this.TemplateDescriptor.SetValue(this._changePassword, template);
				flag = true;
			}
			catch (Exception)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060025A7 RID: 9639 RVA: 0x000CB278 File Offset: 0x000CA278
		public override string GetDesignTimeHtml()
		{
			return this.GetDesignTimeHtml(null);
		}

		// Token: 0x060025A8 RID: 9640 RVA: 0x000CB284 File Offset: 0x000CA284
		public override string GetDesignTimeHtml(DesignerRegionCollection regions)
		{
			IDictionary dictionary = new HybridDictionary(2);
			dictionary["CurrentView"] = this.CurrentView;
			bool flag = base.UseRegions(regions, this.GetTemplate(this._changePassword));
			if (flag)
			{
				((WebControl)base.ViewControl).Enabled = true;
				dictionary.Add("RegionEditing", true);
				regions.Add(new TemplatedEditableDesignerRegion(this.TemplateDefinition)
				{
					Description = SR.GetString("ContainerControlDesigner_RegionWatermark")
				});
			}
			string text = string.Empty;
			try
			{
				((IControlDesignerAccessor)base.ViewControl).SetDesignModeState(dictionary);
				((ICompositeControlDesignerAccessor)base.ViewControl).RecreateChildControls();
				text = base.GetDesignTimeHtml();
			}
			catch (Exception ex)
			{
				text = this.GetErrorDesignTimeHtml(ex);
			}
			return text;
		}

		// Token: 0x060025A9 RID: 9641 RVA: 0x000CB354 File Offset: 0x000CA354
		public override string GetEditableDesignerRegionContent(EditableDesignerRegion region)
		{
			ITemplate template = this.GetTemplate(this._changePassword);
			if (template == null)
			{
				return this.GetEmptyDesignTimeHtml();
			}
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			return ControlPersister.PersistTemplate(template, designerHost);
		}

		// Token: 0x060025AA RID: 9642 RVA: 0x000CB395 File Offset: 0x000CA395
		protected override string GetErrorDesignTimeHtml(Exception e)
		{
			return base.CreatePlaceHolderDesignTimeHtml(SR.GetString("Control_ErrorRenderingShort") + "<br />" + e.Message);
		}

		// Token: 0x060025AB RID: 9643 RVA: 0x000CB3B8 File Offset: 0x000CA3B8
		private ITemplate GetTemplate(ChangePassword changePassword)
		{
			ITemplate template = null;
			switch (this.CurrentView)
			{
			case ChangePasswordDesigner.ViewType.ChangePassword:
				template = changePassword.ChangePasswordTemplate;
				break;
			case ChangePasswordDesigner.ViewType.Success:
				template = changePassword.SuccessTemplate;
				break;
			}
			return template;
		}

		// Token: 0x060025AC RID: 9644 RVA: 0x000CB3EF File Offset: 0x000CA3EF
		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(ChangePassword));
			this._changePassword = (ChangePassword)component;
			base.Initialize(component);
		}

		// Token: 0x060025AD RID: 9645 RVA: 0x000CB414 File Offset: 0x000CA414
		private void LaunchWebAdmin()
		{
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (designerHost != null)
			{
				IWebAdministrationService webAdministrationService = (IWebAdministrationService)designerHost.GetService(typeof(IWebAdministrationService));
				if (webAdministrationService != null)
				{
					webAdministrationService.Start(null);
				}
			}
		}

		// Token: 0x060025AE RID: 9646 RVA: 0x000CB45A File Offset: 0x000CA45A
		private void ConvertToTemplate()
		{
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ConvertToTemplateChangeCallback), null, SR.GetString("WebControls_ConvertToTemplate"), this.TemplateDescriptor);
		}

		// Token: 0x060025AF RID: 9647 RVA: 0x000CB484 File Offset: 0x000CA484
		private void Reset()
		{
			this.UpdateDesignTimeHtml();
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ResetChangeCallback), null, SR.GetString("WebControls_Reset"), this.TemplateDescriptor);
		}

		// Token: 0x060025B0 RID: 9648 RVA: 0x000CB4B4 File Offset: 0x000CA4B4
		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			if (this.Templated)
			{
				foreach (string text in ChangePasswordDesigner._nonTemplateProperties)
				{
					PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties[text];
					if (propertyDescriptor != null)
					{
						properties[text] = TypeDescriptor.CreateProperty(propertyDescriptor.ComponentType, propertyDescriptor, new Attribute[] { BrowsableAttribute.No });
					}
				}
			}
		}

		// Token: 0x060025B1 RID: 9649 RVA: 0x000CB51E File Offset: 0x000CA51E
		private bool ResetChangeCallback(object context)
		{
			this.TemplateDescriptor.SetValue(base.Component, null);
			return true;
		}

		// Token: 0x060025B2 RID: 9650 RVA: 0x000CB534 File Offset: 0x000CA534
		public override void SetEditableDesignerRegionContent(EditableDesignerRegion region, string content)
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)[region.Name];
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			ITemplate template = ControlParser.ParseTemplate(designerHost, content);
			using (DesignerTransaction designerTransaction = designerHost.CreateTransaction("SetEditableDesignerRegionContent"))
			{
				propertyDescriptor.SetValue(base.Component, template);
				designerTransaction.Commit();
			}
		}

		// Token: 0x040019E1 RID: 6625
		private const string _failureTextID = "FailureText";

		// Token: 0x040019E2 RID: 6626
		private static DesignerAutoFormatCollection _autoFormats;

		// Token: 0x040019E3 RID: 6627
		private ChangePassword _changePassword;

		// Token: 0x040019E4 RID: 6628
		private static readonly string[] _templateNames = new string[] { "ChangePasswordTemplate", "SuccessTemplate" };

		// Token: 0x040019E5 RID: 6629
		private static readonly string[] _changePasswordViewRegionToPropertyMap = new string[] { "ChangePasswordTitleText", "UserNameLabelText", "PasswordLabelText", "InstructionText", "PasswordHintText", "NewPasswordLabelText", "ConfirmNewPasswordLabelText" };

		// Token: 0x040019E6 RID: 6630
		private static readonly string[] _successViewRegionToPropertyMap = new string[] { "SuccessText", "SuccessTitleText" };

		// Token: 0x040019E7 RID: 6631
		private static readonly string[] _nonTemplateProperties = new string[]
		{
			"BorderPadding", "CancelButtonImageUrl", "CancelButtonStyle", "CancelButtonText", "CancelButtonType", "ChangePasswordButtonImageUrl", "ChangePasswordButtonStyle", "ChangePasswordButtonText", "ChangePasswordButtonType", "ChangePasswordTitleText",
			"ConfirmNewPasswordLabelText", "ConfirmPasswordCompareErrorMessage", "ConfirmPasswordRequiredErrorMessage", "ContinueButtonImageUrl", "ContinueButtonStyle", "ContinueButtonText", "ContinueButtonType", "CreateUserIconUrl", "CreateUserText", "CreateUserUrl",
			"DisplayUserName", "EditProfileText", "EditProfileIconUrl", "EditProfileUrl", "FailureTextStyle", "HelpPageIconUrl", "HelpPageText", "HelpPageUrl", "HyperLinkStyle", "InstructionText",
			"InstructionTextStyle", "LabelStyle", "NewPasswordLabelText", "NewPasswordRequiredErrorMessage", "NewPasswordRegularExpression", "NewPasswordRegularExpressionErrorMessage", "PasswordHintText", "PasswordHintStyle", "PasswordLabelText", "PasswordRecoveryText",
			"PasswordRecoveryUrl", "PasswordRecoveryIconUrl", "PasswordRequiredErrorMessage", "SuccessTitleText", "SuccessText", "SuccessTextStyle", "TextBoxStyle", "TitleTextStyle", "UserNameLabelText", "UserNameRequiredErrorMessage",
			"ValidatorTextStyle"
		};

		// Token: 0x040019E8 RID: 6632
		[CompilerGenerated]
		private static ControlDesigner.CreateAutoFormatDelegate <>9__CachedAnonymousMethodDelegate1;

		// Token: 0x02000403 RID: 1027
		private enum ViewType
		{
			// Token: 0x040019EA RID: 6634
			ChangePassword,
			// Token: 0x040019EB RID: 6635
			Success
		}

		// Token: 0x02000404 RID: 1028
		private class ChangePasswordDesignerActionList : DesignerActionList
		{
			// Token: 0x060025B6 RID: 9654 RVA: 0x000CB818 File Offset: 0x000CA818
			public ChangePasswordDesignerActionList(ChangePasswordDesigner designer)
				: base(designer.Component)
			{
				this._designer = designer;
			}

			// Token: 0x17000704 RID: 1796
			// (get) Token: 0x060025B7 RID: 9655 RVA: 0x000CB82D File Offset: 0x000CA82D
			// (set) Token: 0x060025B8 RID: 9656 RVA: 0x000CB830 File Offset: 0x000CA830
			public override bool AutoShow
			{
				get
				{
					return true;
				}
				set
				{
				}
			}

			// Token: 0x17000705 RID: 1797
			// (get) Token: 0x060025B9 RID: 9657 RVA: 0x000CB832 File Offset: 0x000CA832
			// (set) Token: 0x060025BA RID: 9658 RVA: 0x000CB858 File Offset: 0x000CA858
			[TypeConverter(typeof(ChangePasswordDesigner.ChangePasswordDesignerActionList.ChangePasswordViewTypeConverter))]
			public string View
			{
				get
				{
					if (this._designer.CurrentView == ChangePasswordDesigner.ViewType.ChangePassword)
					{
						return SR.GetString("ChangePassword_ChangePasswordView");
					}
					return SR.GetString("ChangePassword_SuccessView");
				}
				set
				{
					if (string.Compare(value, SR.GetString("ChangePassword_ChangePasswordView"), StringComparison.Ordinal) == 0)
					{
						this._designer.CurrentView = ChangePasswordDesigner.ViewType.ChangePassword;
					}
					else if (string.Compare(value, SR.GetString("ChangePassword_SuccessView"), StringComparison.Ordinal) == 0)
					{
						this._designer.CurrentView = ChangePasswordDesigner.ViewType.Success;
					}
					TypeDescriptor.Refresh(this._designer.Component);
					this._designer.UpdateDesignTimeHtml();
				}
			}

			// Token: 0x060025BB RID: 9659 RVA: 0x000CB8C0 File Offset: 0x000CA8C0
			public void ConvertToTemplate()
			{
				Cursor cursor = Cursor.Current;
				try
				{
					Cursor.Current = Cursors.WaitCursor;
					this._designer.ConvertToTemplate();
				}
				finally
				{
					Cursor.Current = cursor;
				}
			}

			// Token: 0x060025BC RID: 9660 RVA: 0x000CB904 File Offset: 0x000CA904
			public void LaunchWebAdmin()
			{
				this._designer.LaunchWebAdmin();
			}

			// Token: 0x060025BD RID: 9661 RVA: 0x000CB914 File Offset: 0x000CA914
			public override DesignerActionItemCollection GetSortedActionItems()
			{
				DesignerActionItemCollection designerActionItemCollection = new DesignerActionItemCollection();
				designerActionItemCollection.Add(new DesignerActionPropertyItem("View", SR.GetString("WebControls_Views"), string.Empty, SR.GetString("WebControls_ViewsDescription")));
				if (this._designer.Templated)
				{
					designerActionItemCollection.Add(new DesignerActionMethodItem(this, "Reset", SR.GetString("WebControls_Reset"), string.Empty, SR.GetString("WebControls_ResetDescriptionViews"), true));
				}
				else
				{
					designerActionItemCollection.Add(new DesignerActionMethodItem(this, "ConvertToTemplate", SR.GetString("WebControls_ConvertToTemplate"), string.Empty, SR.GetString("WebControls_ConvertToTemplateDescriptionViews"), true));
				}
				designerActionItemCollection.Add(new DesignerActionMethodItem(this, "LaunchWebAdmin", SR.GetString("Login_LaunchWebAdmin"), string.Empty, SR.GetString("Login_LaunchWebAdminDescription"), true));
				return designerActionItemCollection;
			}

			// Token: 0x060025BE RID: 9662 RVA: 0x000CB9E5 File Offset: 0x000CA9E5
			public void Reset()
			{
				this._designer.Reset();
			}

			// Token: 0x040019EC RID: 6636
			private ChangePasswordDesigner _designer;

			// Token: 0x02000405 RID: 1029
			private class ChangePasswordViewTypeConverter : TypeConverter
			{
				// Token: 0x060025BF RID: 9663 RVA: 0x000CB9F4 File Offset: 0x000CA9F4
				public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
				{
					return new TypeConverter.StandardValuesCollection(new string[]
					{
						SR.GetString("ChangePassword_ChangePasswordView"),
						SR.GetString("ChangePassword_SuccessView")
					});
				}

				// Token: 0x060025C0 RID: 9664 RVA: 0x000CBA28 File Offset: 0x000CAA28
				public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
				{
					return true;
				}

				// Token: 0x060025C1 RID: 9665 RVA: 0x000CBA2B File Offset: 0x000CAA2B
				public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
				{
					return true;
				}
			}
		}

		// Token: 0x02000408 RID: 1032
		private sealed class ConvertToTemplateHelper : LoginDesignerUtil.GenericConvertToTemplateHelper<ChangePassword, ChangePasswordDesigner>
		{
			// Token: 0x060025CE RID: 9678 RVA: 0x000CBC68 File Offset: 0x000CAC68
			public ConvertToTemplateHelper(ChangePasswordDesigner designer, IDesignerHost designerHost)
				: base(designer, designerHost)
			{
			}

			// Token: 0x1700070A RID: 1802
			// (get) Token: 0x060025CF RID: 9679 RVA: 0x000CBC72 File Offset: 0x000CAC72
			protected override string[] PersistedControlIDs
			{
				get
				{
					return ChangePasswordDesigner.ConvertToTemplateHelper._persistedControlIDs;
				}
			}

			// Token: 0x1700070B RID: 1803
			// (get) Token: 0x060025D0 RID: 9680 RVA: 0x000CBC79 File Offset: 0x000CAC79
			protected override string[] PersistedIfNotVisibleControlIDs
			{
				get
				{
					return ChangePasswordDesigner.ConvertToTemplateHelper._persistedIfNotVisibleControlIDs;
				}
			}

			// Token: 0x060025D1 RID: 9681 RVA: 0x000CBC80 File Offset: 0x000CAC80
			protected override Style GetFailureTextStyle(ChangePassword control)
			{
				return control.FailureTextStyle;
			}

			// Token: 0x060025D2 RID: 9682 RVA: 0x000CBC88 File Offset: 0x000CAC88
			protected override Control GetDefaultTemplateContents()
			{
				Control control = null;
				switch (base.Designer.CurrentView)
				{
				case ChangePasswordDesigner.ViewType.ChangePassword:
					control = base.Designer.ViewControl.Controls[0];
					break;
				case ChangePasswordDesigner.ViewType.Success:
					control = base.Designer.ViewControl.Controls[1];
					break;
				}
				return (Table)control.Controls[0];
			}

			// Token: 0x060025D3 RID: 9683 RVA: 0x000CBCF6 File Offset: 0x000CACF6
			protected override ITemplate GetTemplate(ChangePassword control)
			{
				return base.Designer.GetTemplate(control);
			}

			// Token: 0x040019F0 RID: 6640
			private static readonly string[] _persistedControlIDs = new string[]
			{
				"UserName", "UserNameRequired", "CurrentPassword", "CurrentPasswordRequired", "NewPassword", "NewPasswordRequired", "NewPasswordRegExp", "ConfirmNewPassword", "ConfirmNewPasswordRequired", "NewPasswordCompare",
				"ChangePasswordPushButton", "ChangePasswordImageButton", "ChangePasswordLinkButton", "CancelPushButton", "CancelImageButton", "CancelLinkButton", "ContinuePushButton", "ContinueImageButton", "ContinueLinkButton", "FailureText",
				"HelpLink", "CreateUserLink", "PasswordRecoveryLink", "EditProfileLink", "EditProfileLinkSuccess"
			};

			// Token: 0x040019F1 RID: 6641
			private static readonly string[] _persistedIfNotVisibleControlIDs = new string[] { "FailureText" };
		}
	}
}
