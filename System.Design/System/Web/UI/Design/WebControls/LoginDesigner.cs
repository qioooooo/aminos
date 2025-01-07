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
	public class LoginDesigner : CompositeControlDesigner
	{
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection designerActionListCollection = new DesignerActionListCollection();
				designerActionListCollection.AddRange(base.ActionLists);
				designerActionListCollection.Add(new LoginDesigner.LoginDesignerActionList(this));
				return designerActionListCollection;
			}
		}

		public override DesignerAutoFormatCollection AutoFormats
		{
			get
			{
				if (LoginDesigner._autoFormats == null)
				{
					LoginDesigner._autoFormats = ControlDesigner.CreateAutoFormats("<Schemes>\r\n<xsd:schema id=\"Schemes\" xmlns=\"\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\">\r\n  <xsd:element name=\"Scheme\">\r\n     <xsd:complexType>\r\n       <xsd:all>\r\n        <xsd:element name=\"SchemeName\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"BackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"ForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"BorderColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"BorderWidth\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"BorderStyle\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"BorderPadding\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"FontSize\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"FontName\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"TextLayout\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"TitleTextBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"TitleTextForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"TitleTextFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"TitleTextFontSize\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"InstructionTextForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"InstructionTextFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"TextboxFontSize\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SubmitButtonBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SubmitButtonForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SubmitButtonFontSize\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SubmitButtonFontName\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SubmitButtonBorderColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SubmitButtonBorderWidth\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SubmitButtonBorderStyle\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n      </xsd:all>\r\n    </xsd:complexType>\r\n  </xsd:element>\r\n  <xsd:element name=\"Schemes\" msdata:IsDataSet=\"true\">\r\n    <xsd:complexType>\r\n      <xsd:choice maxOccurs=\"unbounded\">\r\n        <xsd:element ref=\"Scheme\"/>\r\n      </xsd:choice>\r\n    </xsd:complexType>\r\n  </xsd:element>\r\n</xsd:schema>\r\n<Scheme>\r\n  <SchemeName>LoginScheme_Empty</SchemeName>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>LoginScheme_Elegant</SchemeName>\r\n  <BackColor>#F7F7DE</BackColor>\r\n  <BorderColor>#CCCC99</BorderColor>\r\n  <BorderWidth>1</BorderWidth>\r\n  <BorderStyle>4</BorderStyle>\r\n  <FontSize>10</FontSize>\r\n  <FontName>Verdana</FontName>\r\n  <TitleTextBackColor>#6B696B</TitleTextBackColor>\r\n  <TitleTextForeColor>#FFFFFF</TitleTextForeColor>\r\n  <TitleTextFont>1</TitleTextFont>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>LoginScheme_Professional</SchemeName>\r\n  <BackColor>#F7F6F3</BackColor>\r\n  <ForeColor>#333333</ForeColor>\r\n  <BorderColor>#E6E2D8</BorderColor>\r\n  <BorderWidth>1</BorderWidth>\r\n  <BorderStyle>4</BorderStyle>\r\n  <BorderPadding>4</BorderPadding>\r\n  <FontSize>0.8em</FontSize>\r\n  <FontName>Verdana</FontName>\r\n  <TitleTextBackColor>#5D7B9D</TitleTextBackColor>\r\n  <TitleTextForeColor>White</TitleTextForeColor>\r\n  <TitleTextFont>1</TitleTextFont>\r\n  <TitleTextFontSize>0.9em</TitleTextFontSize>\r\n  <InstructionTextForeColor>Black</InstructionTextForeColor>\r\n  <InstructionTextFont>2</InstructionTextFont>\r\n  <TextboxFontSize>0.8em</TextboxFontSize>\r\n  <SubmitButtonBackColor>#FFFBFF</SubmitButtonBackColor>\r\n  <SubmitButtonForeColor>#284775</SubmitButtonForeColor>\r\n  <SubmitButtonFontSize>0.8em</SubmitButtonFontSize>\r\n  <SubmitButtonFontName>Verdana</SubmitButtonFontName>\r\n  <SubmitButtonBorderColor>#CCCCCC</SubmitButtonBorderColor>\r\n  <SubmitButtonBorderWidth>1</SubmitButtonBorderWidth>\r\n  <SubmitButtonBorderStyle>4</SubmitButtonBorderStyle>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>LoginScheme_Simple</SchemeName>\r\n  <BackColor>#E3EAEB</BackColor>\r\n  <ForeColor>#333333</ForeColor>\r\n  <BorderColor>#E6E2D8</BorderColor>\r\n  <BorderWidth>1</BorderWidth>\r\n  <BorderStyle>4</BorderStyle>\r\n  <BorderPadding>4</BorderPadding>\r\n  <FontSize>0.8em</FontSize>\r\n  <FontName>Verdana</FontName>\r\n  <TextLayout>1</TextLayout>\r\n  <TitleTextBackColor>#1C5E55</TitleTextBackColor>\r\n  <TitleTextForeColor>White</TitleTextForeColor>\r\n  <TitleTextFont>1</TitleTextFont>\r\n  <TitleTextFontSize>0.9em</TitleTextFontSize>\r\n  <InstructionTextForeColor>Black</InstructionTextForeColor>\r\n  <InstructionTextFont>2</InstructionTextFont>\r\n  <TextboxFontSize>0.8em</TextboxFontSize>\r\n  <SubmitButtonBackColor>White</SubmitButtonBackColor>\r\n  <SubmitButtonForeColor>#1C5E55</SubmitButtonForeColor>\r\n  <SubmitButtonFontSize>0.8em</SubmitButtonFontSize>\r\n  <SubmitButtonFontName>Verdana</SubmitButtonFontName>\r\n  <SubmitButtonBorderColor>#C5BBAF</SubmitButtonBorderColor>\r\n  <SubmitButtonBorderWidth>1</SubmitButtonBorderWidth>\r\n  <SubmitButtonBorderStyle>4</SubmitButtonBorderStyle>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>LoginScheme_Classic</SchemeName>\r\n  <BackColor>#EFF3FB</BackColor>\r\n  <ForeColor>#333333</ForeColor>\r\n  <BorderColor>#B5C7DE</BorderColor>\r\n  <BorderWidth>1</BorderWidth>\r\n  <BorderStyle>4</BorderStyle>\r\n  <BorderPadding>4</BorderPadding>\r\n  <FontSize>0.8em</FontSize>\r\n  <FontName>Verdana</FontName>\r\n  <TitleTextBackColor>#507CD1</TitleTextBackColor>\r\n  <TitleTextForeColor>White</TitleTextForeColor>\r\n  <TitleTextFont>1</TitleTextFont>\r\n  <TitleTextFontSize>0.9em</TitleTextFontSize>\r\n  <InstructionTextForeColor>Black</InstructionTextForeColor>\r\n  <InstructionTextFont>2</InstructionTextFont>\r\n  <TextboxFontSize>0.8em</TextboxFontSize>\r\n  <SubmitButtonBackColor>White</SubmitButtonBackColor>\r\n  <SubmitButtonForeColor>#284E98</SubmitButtonForeColor>\r\n  <SubmitButtonFontSize>0.8em</SubmitButtonFontSize>\r\n  <SubmitButtonFontName>Verdana</SubmitButtonFontName>\r\n  <SubmitButtonBorderColor>#507CD1</SubmitButtonBorderColor>\r\n  <SubmitButtonBorderWidth>1</SubmitButtonBorderWidth>\r\n  <SubmitButtonBorderStyle>4</SubmitButtonBorderStyle>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>LoginScheme_Colorful</SchemeName>\r\n  <BackColor>#FFFBD6</BackColor>\r\n  <ForeColor>#333333</ForeColor>\r\n  <BorderColor>#FFDFAD</BorderColor>\r\n  <BorderWidth>1</BorderWidth>\r\n  <BorderStyle>4</BorderStyle>\r\n  <BorderPadding>4</BorderPadding>\r\n  <FontSize>0.8em</FontSize>\r\n  <FontName>Verdana</FontName>\r\n  <TextLayout>1</TextLayout>\r\n  <TitleTextBackColor>#990000</TitleTextBackColor>\r\n  <TitleTextForeColor>White</TitleTextForeColor>\r\n  <TitleTextFont>1</TitleTextFont>\r\n  <TitleTextFontSize>0.9em</TitleTextFontSize>\r\n  <InstructionTextForeColor>Black</InstructionTextForeColor>\r\n  <InstructionTextFont>2</InstructionTextFont>\r\n  <TextboxFontSize>0.8em</TextboxFontSize>\r\n  <SubmitButtonBackColor>White</SubmitButtonBackColor>\r\n  <SubmitButtonForeColor>#990000</SubmitButtonForeColor>\r\n  <SubmitButtonFontSize>0.8em</SubmitButtonFontSize>\r\n  <SubmitButtonFontName>Verdana</SubmitButtonFontName>\r\n  <SubmitButtonBorderColor>#CC9966</SubmitButtonBorderColor>\r\n  <SubmitButtonBorderWidth>1</SubmitButtonBorderWidth>\r\n  <SubmitButtonBorderStyle>4</SubmitButtonBorderStyle>\r\n</Scheme>\r\n</Schemes>\r\n", (DataRow schemeData) => new LoginAutoFormat(schemeData));
				}
				return LoginDesigner._autoFormats;
			}
		}

		private bool Templated
		{
			get
			{
				return this._login.LayoutTemplate != null;
			}
		}

		private TemplateDefinition TemplateDefinition
		{
			get
			{
				return new TemplateDefinition(this, "LayoutTemplate", this._login, "LayoutTemplate", ((WebControl)base.ViewControl).ControlStyle);
			}
		}

		private PropertyDescriptor TemplateDescriptor
		{
			get
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(base.Component);
				return properties.Find("LayoutTemplate", false);
			}
		}

		public override TemplateGroupCollection TemplateGroups
		{
			get
			{
				TemplateGroupCollection templateGroups = base.TemplateGroups;
				TemplateGroup templateGroup = new TemplateGroup("LayoutTemplate", ((WebControl)base.ViewControl).ControlStyle);
				templateGroup.AddTemplateDefinition(new TemplateDefinition(this, "LayoutTemplate", this._login, "LayoutTemplate", ((WebControl)base.ViewControl).ControlStyle));
				templateGroups.Add(templateGroup);
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

		private void ConvertToTemplate()
		{
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ConvertToTemplateChangeCallback), null, SR.GetString("WebControls_ConvertToTemplate"), this.TemplateDescriptor);
		}

		private bool ConvertToTemplateChangeCallback(object context)
		{
			bool flag;
			try
			{
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				LoginDesigner.ConvertToTemplateHelper convertToTemplateHelper = new LoginDesigner.ConvertToTemplateHelper(this, designerHost);
				ITemplate template = convertToTemplateHelper.ConvertToTemplate();
				this.TemplateDescriptor.SetValue(this._login, template);
				flag = true;
			}
			catch (Exception)
			{
				flag = false;
			}
			return flag;
		}

		protected override string GetErrorDesignTimeHtml(Exception e)
		{
			return base.CreatePlaceHolderDesignTimeHtml(SR.GetString("Control_ErrorRenderingShort") + "<br />" + e.Message);
		}

		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(Login));
			this._login = (Login)component;
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

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			if (this.Templated)
			{
				foreach (string text in LoginDesigner._nonTemplateProperties)
				{
					PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties[text];
					if (propertyDescriptor != null)
					{
						properties[text] = TypeDescriptor.CreateProperty(propertyDescriptor.ComponentType, propertyDescriptor, new Attribute[] { BrowsableAttribute.No });
					}
				}
			}
		}

		private void Reset()
		{
			this.UpdateDesignTimeHtml();
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ResetChangeCallback), null, SR.GetString("WebControls_Reset"), this.TemplateDescriptor);
		}

		private bool ResetChangeCallback(object context)
		{
			this.TemplateDescriptor.SetValue(this._login, null);
			return true;
		}

		public override string GetDesignTimeHtml(DesignerRegionCollection regions)
		{
			bool flag = base.UseRegions(regions, this._login.LayoutTemplate);
			if (flag)
			{
				((WebControl)base.ViewControl).Enabled = true;
				IDictionary dictionary = new HybridDictionary(1);
				dictionary.Add("RegionEditing", true);
				((IControlDesignerAccessor)base.ViewControl).SetDesignModeState(dictionary);
				regions.Add(new TemplatedEditableDesignerRegion(this.TemplateDefinition)
				{
					Description = SR.GetString("ContainerControlDesigner_RegionWatermark")
				});
			}
			return this.GetDesignTimeHtml();
		}

		public override string GetEditableDesignerRegionContent(EditableDesignerRegion region)
		{
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			return ControlPersister.PersistTemplate(this._login.LayoutTemplate, designerHost);
		}

		public override void SetEditableDesignerRegionContent(EditableDesignerRegion region, string content)
		{
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			ITemplate template = ControlParser.ParseTemplate(designerHost, content);
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)[region.Name];
			using (DesignerTransaction designerTransaction = designerHost.CreateTransaction("SetEditableDesignerRegionContent"))
			{
				propertyDescriptor.SetValue(base.Component, template);
				designerTransaction.Commit();
			}
		}

		private const string _templateName = "LayoutTemplate";

		private const string _failureTextID = "FailureText";

		private Login _login;

		private static DesignerAutoFormatCollection _autoFormats;

		private static readonly string[] _nonTemplateProperties = new string[]
		{
			"BorderPadding", "CheckBoxStyle", "CreateUserIconUrl", "CreateUserText", "CreateUserUrl", "DisplayRememberMe", "FailureTextStyle", "HelpPageIconUrl", "HelpPageText", "HelpPageUrl",
			"HyperLinkStyle", "InstructionText", "InstructionTextStyle", "LabelStyle", "Orientation", "PasswordLabelText", "PasswordRecoveryIconUrl", "PasswordRecoveryText", "PasswordRecoveryUrl", "PasswordRequiredErrorMessage",
			"RememberMeText", "LoginButtonImageUrl", "LoginButtonStyle", "LoginButtonText", "LoginButtonType", "TextBoxStyle", "TextLayout", "TitleText", "TitleTextStyle", "UserNameLabelText",
			"UserNameRequiredErrorMessage", "ValidatorTextStyle"
		};

		[CompilerGenerated]
		private static ControlDesigner.CreateAutoFormatDelegate <>9__CachedAnonymousMethodDelegate1;

		private class LoginDesignerActionList : DesignerActionList
		{
			public LoginDesignerActionList(LoginDesigner parent)
				: base(parent.Component)
			{
				this._parent = parent;
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

			public void ConvertToTemplate()
			{
				Cursor cursor = Cursor.Current;
				try
				{
					Cursor.Current = Cursors.WaitCursor;
					this._parent.ConvertToTemplate();
				}
				finally
				{
					Cursor.Current = cursor;
				}
			}

			public void LaunchWebAdmin()
			{
				this._parent.LaunchWebAdmin();
			}

			public void Reset()
			{
				this._parent.Reset();
			}

			public override DesignerActionItemCollection GetSortedActionItems()
			{
				if (this._parent.InTemplateMode)
				{
					return new DesignerActionItemCollection();
				}
				DesignerActionItemCollection designerActionItemCollection = new DesignerActionItemCollection();
				if (!this._parent.Templated)
				{
					designerActionItemCollection.Add(new DesignerActionMethodItem(this, "ConvertToTemplate", SR.GetString("WebControls_ConvertToTemplate"), string.Empty, SR.GetString("WebControls_ConvertToTemplateDescription"), true));
				}
				else
				{
					designerActionItemCollection.Add(new DesignerActionMethodItem(this, "Reset", SR.GetString("WebControls_Reset"), string.Empty, SR.GetString("WebControls_ResetDescription"), true));
				}
				designerActionItemCollection.Add(new DesignerActionMethodItem(this, "LaunchWebAdmin", SR.GetString("Login_LaunchWebAdmin"), string.Empty, SR.GetString("Login_LaunchWebAdminDescription"), true));
				return designerActionItemCollection;
			}

			private LoginDesigner _parent;
		}

		private sealed class ConvertToTemplateHelper : LoginDesignerUtil.GenericConvertToTemplateHelper<Login, LoginDesigner>
		{
			public ConvertToTemplateHelper(LoginDesigner designer, IDesignerHost designerHost)
				: base(designer, designerHost)
			{
			}

			protected override string[] PersistedControlIDs
			{
				get
				{
					return LoginDesigner.ConvertToTemplateHelper._persistedControlIDs;
				}
			}

			protected override string[] PersistedIfNotVisibleControlIDs
			{
				get
				{
					return LoginDesigner.ConvertToTemplateHelper._persistedIfNotVisibleControlIDs;
				}
			}

			protected override Style GetFailureTextStyle(Login control)
			{
				return control.FailureTextStyle;
			}

			protected override Control GetDefaultTemplateContents()
			{
				Control control = base.Designer.ViewControl.Controls[0];
				return (Table)control.Controls[0];
			}

			protected override ITemplate GetTemplate(Login control)
			{
				return control.LayoutTemplate;
			}

			private static readonly string[] _persistedControlIDs = new string[]
			{
				"UserName", "UserNameRequired", "Password", "PasswordRequired", "RememberMe", "LoginButton", "LoginImageButton", "LoginLinkButton", "FailureText", "CreateUserLink",
				"PasswordRecoveryLink", "HelpLink"
			};

			private static readonly string[] _persistedIfNotVisibleControlIDs = new string[] { "FailureText" };
		}
	}
}
