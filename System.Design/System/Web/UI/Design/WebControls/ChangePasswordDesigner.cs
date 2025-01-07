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
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ChangePasswordDesigner : ControlDesigner
	{
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

		private bool Templated
		{
			get
			{
				return this.GetTemplate(this._changePassword) != null;
			}
		}

		private PropertyDescriptor TemplateDescriptor
		{
			get
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(base.Component);
				string text = ChangePasswordDesigner._templateNames[(int)this.CurrentView];
				return properties.Find(text, false);
			}
		}

		private TemplateDefinition TemplateDefinition
		{
			get
			{
				string text = ChangePasswordDesigner._templateNames[(int)this.CurrentView];
				return new TemplateDefinition(this, text, this._changePassword, text, ((WebControl)base.ViewControl).ControlStyle);
			}
		}

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

		protected override bool UsePreviewControl
		{
			get
			{
				return true;
			}
		}

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

		public override string GetDesignTimeHtml()
		{
			return this.GetDesignTimeHtml(null);
		}

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

		protected override string GetErrorDesignTimeHtml(Exception e)
		{
			return base.CreatePlaceHolderDesignTimeHtml(SR.GetString("Control_ErrorRenderingShort") + "<br />" + e.Message);
		}

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

		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(ChangePassword));
			this._changePassword = (ChangePassword)component;
			base.Initialize(component);
		}

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

		private void ConvertToTemplate()
		{
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ConvertToTemplateChangeCallback), null, SR.GetString("WebControls_ConvertToTemplate"), this.TemplateDescriptor);
		}

		private void Reset()
		{
			this.UpdateDesignTimeHtml();
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ResetChangeCallback), null, SR.GetString("WebControls_Reset"), this.TemplateDescriptor);
		}

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

		private bool ResetChangeCallback(object context)
		{
			this.TemplateDescriptor.SetValue(base.Component, null);
			return true;
		}

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

		private const string _failureTextID = "FailureText";

		private static DesignerAutoFormatCollection _autoFormats;

		private ChangePassword _changePassword;

		private static readonly string[] _templateNames = new string[] { "ChangePasswordTemplate", "SuccessTemplate" };

		private static readonly string[] _changePasswordViewRegionToPropertyMap = new string[] { "ChangePasswordTitleText", "UserNameLabelText", "PasswordLabelText", "InstructionText", "PasswordHintText", "NewPasswordLabelText", "ConfirmNewPasswordLabelText" };

		private static readonly string[] _successViewRegionToPropertyMap = new string[] { "SuccessText", "SuccessTitleText" };

		private static readonly string[] _nonTemplateProperties = new string[]
		{
			"BorderPadding", "CancelButtonImageUrl", "CancelButtonStyle", "CancelButtonText", "CancelButtonType", "ChangePasswordButtonImageUrl", "ChangePasswordButtonStyle", "ChangePasswordButtonText", "ChangePasswordButtonType", "ChangePasswordTitleText",
			"ConfirmNewPasswordLabelText", "ConfirmPasswordCompareErrorMessage", "ConfirmPasswordRequiredErrorMessage", "ContinueButtonImageUrl", "ContinueButtonStyle", "ContinueButtonText", "ContinueButtonType", "CreateUserIconUrl", "CreateUserText", "CreateUserUrl",
			"DisplayUserName", "EditProfileText", "EditProfileIconUrl", "EditProfileUrl", "FailureTextStyle", "HelpPageIconUrl", "HelpPageText", "HelpPageUrl", "HyperLinkStyle", "InstructionText",
			"InstructionTextStyle", "LabelStyle", "NewPasswordLabelText", "NewPasswordRequiredErrorMessage", "NewPasswordRegularExpression", "NewPasswordRegularExpressionErrorMessage", "PasswordHintText", "PasswordHintStyle", "PasswordLabelText", "PasswordRecoveryText",
			"PasswordRecoveryUrl", "PasswordRecoveryIconUrl", "PasswordRequiredErrorMessage", "SuccessTitleText", "SuccessText", "SuccessTextStyle", "TextBoxStyle", "TitleTextStyle", "UserNameLabelText", "UserNameRequiredErrorMessage",
			"ValidatorTextStyle"
		};

		[CompilerGenerated]
		private static ControlDesigner.CreateAutoFormatDelegate <>9__CachedAnonymousMethodDelegate1;

		private enum ViewType
		{
			ChangePassword,
			Success
		}

		private class ChangePasswordDesignerActionList : DesignerActionList
		{
			public ChangePasswordDesignerActionList(ChangePasswordDesigner designer)
				: base(designer.Component)
			{
				this._designer = designer;
			}

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

			public void LaunchWebAdmin()
			{
				this._designer.LaunchWebAdmin();
			}

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

			public void Reset()
			{
				this._designer.Reset();
			}

			private ChangePasswordDesigner _designer;

			private class ChangePasswordViewTypeConverter : TypeConverter
			{
				public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
				{
					return new TypeConverter.StandardValuesCollection(new string[]
					{
						SR.GetString("ChangePassword_ChangePasswordView"),
						SR.GetString("ChangePassword_SuccessView")
					});
				}

				public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
				{
					return true;
				}

				public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
				{
					return true;
				}
			}
		}

		private sealed class ConvertToTemplateHelper : LoginDesignerUtil.GenericConvertToTemplateHelper<ChangePassword, ChangePasswordDesigner>
		{
			public ConvertToTemplateHelper(ChangePasswordDesigner designer, IDesignerHost designerHost)
				: base(designer, designerHost)
			{
			}

			protected override string[] PersistedControlIDs
			{
				get
				{
					return ChangePasswordDesigner.ConvertToTemplateHelper._persistedControlIDs;
				}
			}

			protected override string[] PersistedIfNotVisibleControlIDs
			{
				get
				{
					return ChangePasswordDesigner.ConvertToTemplateHelper._persistedIfNotVisibleControlIDs;
				}
			}

			protected override Style GetFailureTextStyle(ChangePassword control)
			{
				return control.FailureTextStyle;
			}

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

			protected override ITemplate GetTemplate(ChangePassword control)
			{
				return base.Designer.GetTemplate(control);
			}

			private static readonly string[] _persistedControlIDs = new string[]
			{
				"UserName", "UserNameRequired", "CurrentPassword", "CurrentPasswordRequired", "NewPassword", "NewPasswordRequired", "NewPasswordRegExp", "ConfirmNewPassword", "ConfirmNewPasswordRequired", "NewPasswordCompare",
				"ChangePasswordPushButton", "ChangePasswordImageButton", "ChangePasswordLinkButton", "CancelPushButton", "CancelImageButton", "CancelLinkButton", "ContinuePushButton", "ContinueImageButton", "ContinueLinkButton", "FailureText",
				"HelpLink", "CreateUserLink", "PasswordRecoveryLink", "EditProfileLink", "EditProfileLinkSuccess"
			};

			private static readonly string[] _persistedIfNotVisibleControlIDs = new string[] { "FailureText" };
		}
	}
}
