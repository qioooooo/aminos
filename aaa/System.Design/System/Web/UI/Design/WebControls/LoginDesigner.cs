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
	// Token: 0x02000466 RID: 1126
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class LoginDesigner : CompositeControlDesigner
	{
		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x060028E9 RID: 10473 RVA: 0x000E0C18 File Offset: 0x000DFC18
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

		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x060028EA RID: 10474 RVA: 0x000E0C4D File Offset: 0x000DFC4D
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

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x060028EB RID: 10475 RVA: 0x000E0C87 File Offset: 0x000DFC87
		private bool Templated
		{
			get
			{
				return this._login.LayoutTemplate != null;
			}
		}

		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x060028EC RID: 10476 RVA: 0x000E0C9A File Offset: 0x000DFC9A
		private TemplateDefinition TemplateDefinition
		{
			get
			{
				return new TemplateDefinition(this, "LayoutTemplate", this._login, "LayoutTemplate", ((WebControl)base.ViewControl).ControlStyle);
			}
		}

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x060028ED RID: 10477 RVA: 0x000E0CC4 File Offset: 0x000DFCC4
		private PropertyDescriptor TemplateDescriptor
		{
			get
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(base.Component);
				return properties.Find("LayoutTemplate", false);
			}
		}

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x060028EE RID: 10478 RVA: 0x000E0CEC File Offset: 0x000DFCEC
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

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x060028EF RID: 10479 RVA: 0x000E0D50 File Offset: 0x000DFD50
		protected override bool UsePreviewControl
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060028F0 RID: 10480 RVA: 0x000E0D53 File Offset: 0x000DFD53
		private void ConvertToTemplate()
		{
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ConvertToTemplateChangeCallback), null, SR.GetString("WebControls_ConvertToTemplate"), this.TemplateDescriptor);
		}

		// Token: 0x060028F1 RID: 10481 RVA: 0x000E0D80 File Offset: 0x000DFD80
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

		// Token: 0x060028F2 RID: 10482 RVA: 0x000E0DE0 File Offset: 0x000DFDE0
		protected override string GetErrorDesignTimeHtml(Exception e)
		{
			return base.CreatePlaceHolderDesignTimeHtml(SR.GetString("Control_ErrorRenderingShort") + "<br />" + e.Message);
		}

		// Token: 0x060028F3 RID: 10483 RVA: 0x000E0E02 File Offset: 0x000DFE02
		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(Login));
			this._login = (Login)component;
			base.Initialize(component);
		}

		// Token: 0x060028F4 RID: 10484 RVA: 0x000E0E28 File Offset: 0x000DFE28
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

		// Token: 0x060028F5 RID: 10485 RVA: 0x000E0E70 File Offset: 0x000DFE70
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

		// Token: 0x060028F6 RID: 10486 RVA: 0x000E0EDA File Offset: 0x000DFEDA
		private void Reset()
		{
			this.UpdateDesignTimeHtml();
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ResetChangeCallback), null, SR.GetString("WebControls_Reset"), this.TemplateDescriptor);
		}

		// Token: 0x060028F7 RID: 10487 RVA: 0x000E0F0A File Offset: 0x000DFF0A
		private bool ResetChangeCallback(object context)
		{
			this.TemplateDescriptor.SetValue(this._login, null);
			return true;
		}

		// Token: 0x060028F8 RID: 10488 RVA: 0x000E0F20 File Offset: 0x000DFF20
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

		// Token: 0x060028F9 RID: 10489 RVA: 0x000E0FA4 File Offset: 0x000DFFA4
		public override string GetEditableDesignerRegionContent(EditableDesignerRegion region)
		{
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			return ControlPersister.PersistTemplate(this._login.LayoutTemplate, designerHost);
		}

		// Token: 0x060028FA RID: 10490 RVA: 0x000E0FD8 File Offset: 0x000DFFD8
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

		// Token: 0x04001C4F RID: 7247
		private const string _templateName = "LayoutTemplate";

		// Token: 0x04001C50 RID: 7248
		private const string _failureTextID = "FailureText";

		// Token: 0x04001C51 RID: 7249
		private Login _login;

		// Token: 0x04001C52 RID: 7250
		private static DesignerAutoFormatCollection _autoFormats;

		// Token: 0x04001C53 RID: 7251
		private static readonly string[] _nonTemplateProperties = new string[]
		{
			"BorderPadding", "CheckBoxStyle", "CreateUserIconUrl", "CreateUserText", "CreateUserUrl", "DisplayRememberMe", "FailureTextStyle", "HelpPageIconUrl", "HelpPageText", "HelpPageUrl",
			"HyperLinkStyle", "InstructionText", "InstructionTextStyle", "LabelStyle", "Orientation", "PasswordLabelText", "PasswordRecoveryIconUrl", "PasswordRecoveryText", "PasswordRecoveryUrl", "PasswordRequiredErrorMessage",
			"RememberMeText", "LoginButtonImageUrl", "LoginButtonStyle", "LoginButtonText", "LoginButtonType", "TextBoxStyle", "TextLayout", "TitleText", "TitleTextStyle", "UserNameLabelText",
			"UserNameRequiredErrorMessage", "ValidatorTextStyle"
		};

		// Token: 0x04001C54 RID: 7252
		[CompilerGenerated]
		private static ControlDesigner.CreateAutoFormatDelegate <>9__CachedAnonymousMethodDelegate1;

		// Token: 0x02000467 RID: 1127
		private class LoginDesignerActionList : DesignerActionList
		{
			// Token: 0x060028FE RID: 10494 RVA: 0x000E1192 File Offset: 0x000E0192
			public LoginDesignerActionList(LoginDesigner parent)
				: base(parent.Component)
			{
				this._parent = parent;
			}

			// Token: 0x17000797 RID: 1943
			// (get) Token: 0x060028FF RID: 10495 RVA: 0x000E11A7 File Offset: 0x000E01A7
			// (set) Token: 0x06002900 RID: 10496 RVA: 0x000E11AA File Offset: 0x000E01AA
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

			// Token: 0x06002901 RID: 10497 RVA: 0x000E11AC File Offset: 0x000E01AC
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

			// Token: 0x06002902 RID: 10498 RVA: 0x000E11F0 File Offset: 0x000E01F0
			public void LaunchWebAdmin()
			{
				this._parent.LaunchWebAdmin();
			}

			// Token: 0x06002903 RID: 10499 RVA: 0x000E11FD File Offset: 0x000E01FD
			public void Reset()
			{
				this._parent.Reset();
			}

			// Token: 0x06002904 RID: 10500 RVA: 0x000E120C File Offset: 0x000E020C
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

			// Token: 0x04001C55 RID: 7253
			private LoginDesigner _parent;
		}

		// Token: 0x02000468 RID: 1128
		private sealed class ConvertToTemplateHelper : LoginDesignerUtil.GenericConvertToTemplateHelper<Login, LoginDesigner>
		{
			// Token: 0x06002905 RID: 10501 RVA: 0x000E12C6 File Offset: 0x000E02C6
			public ConvertToTemplateHelper(LoginDesigner designer, IDesignerHost designerHost)
				: base(designer, designerHost)
			{
			}

			// Token: 0x17000798 RID: 1944
			// (get) Token: 0x06002906 RID: 10502 RVA: 0x000E12D0 File Offset: 0x000E02D0
			protected override string[] PersistedControlIDs
			{
				get
				{
					return LoginDesigner.ConvertToTemplateHelper._persistedControlIDs;
				}
			}

			// Token: 0x17000799 RID: 1945
			// (get) Token: 0x06002907 RID: 10503 RVA: 0x000E12D7 File Offset: 0x000E02D7
			protected override string[] PersistedIfNotVisibleControlIDs
			{
				get
				{
					return LoginDesigner.ConvertToTemplateHelper._persistedIfNotVisibleControlIDs;
				}
			}

			// Token: 0x06002908 RID: 10504 RVA: 0x000E12DE File Offset: 0x000E02DE
			protected override Style GetFailureTextStyle(Login control)
			{
				return control.FailureTextStyle;
			}

			// Token: 0x06002909 RID: 10505 RVA: 0x000E12E8 File Offset: 0x000E02E8
			protected override Control GetDefaultTemplateContents()
			{
				Control control = base.Designer.ViewControl.Controls[0];
				return (Table)control.Controls[0];
			}

			// Token: 0x0600290A RID: 10506 RVA: 0x000E131F File Offset: 0x000E031F
			protected override ITemplate GetTemplate(Login control)
			{
				return control.LayoutTemplate;
			}

			// Token: 0x04001C56 RID: 7254
			private static readonly string[] _persistedControlIDs = new string[]
			{
				"UserName", "UserNameRequired", "Password", "PasswordRequired", "RememberMe", "LoginButton", "LoginImageButton", "LoginLinkButton", "FailureText", "CreateUserLink",
				"PasswordRecoveryLink", "HelpLink"
			};

			// Token: 0x04001C57 RID: 7255
			private static readonly string[] _persistedIfNotVisibleControlIDs = new string[] { "FailureText" };
		}
	}
}
