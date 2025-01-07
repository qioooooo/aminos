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
	public class PasswordRecoveryDesigner : ControlDesigner
	{
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection designerActionListCollection = new DesignerActionListCollection();
				designerActionListCollection.AddRange(base.ActionLists);
				designerActionListCollection.Add(new PasswordRecoveryDesigner.PasswordRecoveryDesignerActionList(this));
				return designerActionListCollection;
			}
		}

		public override DesignerAutoFormatCollection AutoFormats
		{
			get
			{
				if (PasswordRecoveryDesigner._autoFormats == null)
				{
					PasswordRecoveryDesigner._autoFormats = ControlDesigner.CreateAutoFormats("<Schemes>\r\n<xsd:schema id=\"Schemes\" xmlns=\"\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\">\r\n  <xsd:element name=\"Scheme\">\r\n     <xsd:complexType>\r\n       <xsd:all>\r\n        <xsd:element name=\"SchemeName\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"BackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"ForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"BorderColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"BorderWidth\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"BorderStyle\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"BorderPadding\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"FontSize\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"FontName\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"TitleTextBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"TitleTextForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"TitleTextFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"TitleTextFontSize\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SuccessTextForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SuccessTextFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"InstructionTextForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"InstructionTextFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"TextboxFontSize\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SubmitButtonBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SubmitButtonForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SubmitButtonFontSize\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SubmitButtonFontName\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SubmitButtonBorderColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SubmitButtonBorderWidth\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SubmitButtonBorderStyle\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n      </xsd:all>\r\n    </xsd:complexType>\r\n  </xsd:element>\r\n  <xsd:element name=\"Schemes\" msdata:IsDataSet=\"true\">\r\n    <xsd:complexType>\r\n      <xsd:choice maxOccurs=\"unbounded\">\r\n        <xsd:element ref=\"Scheme\"/>\r\n      </xsd:choice>\r\n    </xsd:complexType>\r\n  </xsd:element>\r\n</xsd:schema>\r\n<Scheme>\r\n  <SchemeName>PasswordRecoveryScheme_Empty</SchemeName>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>PasswordRecoveryScheme_Elegant</SchemeName>\r\n  <BackColor>#F7F7DE</BackColor>\r\n  <BorderColor>#CCCC99</BorderColor>\r\n  <BorderWidth>1</BorderWidth>\r\n  <BorderStyle>4</BorderStyle>\r\n  <FontSize>10</FontSize>\r\n  <FontName>Verdana</FontName>\r\n  <TitleTextBackColor>#6B696B</TitleTextBackColor>\r\n  <TitleTextForeColor>#FFFFFF</TitleTextForeColor>\r\n  <TitleTextFont>1</TitleTextFont>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>PasswordRecoveryScheme_Professional</SchemeName>\r\n  <BackColor>#F7F6F3</BackColor>\r\n  <ForeColor>#333333</ForeColor>\r\n  <BorderColor>#E6E2D8</BorderColor>\r\n  <BorderWidth>1</BorderWidth>\r\n  <BorderStyle>4</BorderStyle>\r\n  <BorderPadding>4</BorderPadding>\r\n  <FontSize>0.8em</FontSize>\r\n  <FontName>Verdana</FontName>\r\n  <TitleTextBackColor>#5D7B9D</TitleTextBackColor>\r\n  <TitleTextForeColor>White</TitleTextForeColor>\r\n  <TitleTextFont>1</TitleTextFont>\r\n  <TitleTextFontSize>0.9em</TitleTextFontSize>\r\n  <InstructionTextForeColor>Black</InstructionTextForeColor>\r\n  <InstructionTextFont>2</InstructionTextFont>\r\n  <SuccessTextForeColor>#5D7B9D</SuccessTextForeColor>\r\n  <SuccessTextFont>1</SuccessTextFont>\r\n  <TextboxFontSize>0.8em</TextboxFontSize>\r\n  <SubmitButtonBackColor>#FFFBFF</SubmitButtonBackColor>\r\n  <SubmitButtonForeColor>#284775</SubmitButtonForeColor>\r\n  <SubmitButtonFontSize>0.8em</SubmitButtonFontSize>\r\n  <SubmitButtonFontName>Verdana</SubmitButtonFontName>\r\n  <SubmitButtonBorderColor>#CCCCCC</SubmitButtonBorderColor>\r\n  <SubmitButtonBorderWidth>1</SubmitButtonBorderWidth>\r\n  <SubmitButtonBorderStyle>4</SubmitButtonBorderStyle>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>PasswordRecoveryScheme_Simple</SchemeName>\r\n  <BackColor>#E3EAEB</BackColor>\r\n  <ForeColor>#333333</ForeColor>\r\n  <BorderColor>#E6E2D8</BorderColor>\r\n  <BorderWidth>1</BorderWidth>\r\n  <BorderStyle>4</BorderStyle>\r\n  <BorderPadding>4</BorderPadding>\r\n  <FontSize>0.8em</FontSize>\r\n  <FontName>Verdana</FontName>\r\n  <TitleTextBackColor>#1C5E55</TitleTextBackColor>\r\n  <TitleTextForeColor>White</TitleTextForeColor>\r\n  <TitleTextFont>1</TitleTextFont>\r\n  <TitleTextFontSize>0.9em</TitleTextFontSize>\r\n  <InstructionTextForeColor>Black</InstructionTextForeColor>\r\n  <InstructionTextFont>2</InstructionTextFont>\r\n  <SuccessTextForeColor>#1C5E55</SuccessTextForeColor>\r\n  <SuccessTextFont>1</SuccessTextFont>\r\n  <TextboxFontSize>0.8em</TextboxFontSize>\r\n  <SubmitButtonBackColor>White</SubmitButtonBackColor>\r\n  <SubmitButtonForeColor>#1C5E55</SubmitButtonForeColor>\r\n  <SubmitButtonFontSize>0.8em</SubmitButtonFontSize>\r\n  <SubmitButtonFontName>Verdana</SubmitButtonFontName>\r\n  <SubmitButtonBorderColor>#C5BBAF</SubmitButtonBorderColor>\r\n  <SubmitButtonBorderWidth>1</SubmitButtonBorderWidth>\r\n  <SubmitButtonBorderStyle>4</SubmitButtonBorderStyle>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>PasswordRecoveryScheme_Classic</SchemeName>\r\n  <BackColor>#EFF3FB</BackColor>\r\n  <ForeColor>#333333</ForeColor>\r\n  <BorderColor>#B5C7DE</BorderColor>\r\n  <BorderWidth>1</BorderWidth>\r\n  <BorderStyle>4</BorderStyle>\r\n  <BorderPadding>4</BorderPadding>\r\n  <FontSize>0.8em</FontSize>\r\n  <FontName>Verdana</FontName>\r\n  <TitleTextBackColor>#507CD1</TitleTextBackColor>\r\n  <TitleTextForeColor>White</TitleTextForeColor>\r\n  <TitleTextFont>1</TitleTextFont>\r\n  <TitleTextFontSize>0.9em</TitleTextFontSize>\r\n  <InstructionTextForeColor>Black</InstructionTextForeColor>\r\n  <InstructionTextFont>2</InstructionTextFont>\r\n  <SuccessTextForeColor>#507CD1</SuccessTextForeColor>\r\n  <SuccessTextFont>1</SuccessTextFont>\r\n  <TextboxFontSize>0.8em</TextboxFontSize>\r\n  <SubmitButtonBackColor>White</SubmitButtonBackColor>\r\n  <SubmitButtonForeColor>#284E98</SubmitButtonForeColor>\r\n  <SubmitButtonFontSize>0.8em</SubmitButtonFontSize>\r\n  <SubmitButtonFontName>Verdana</SubmitButtonFontName>\r\n  <SubmitButtonBorderColor>#507CD1</SubmitButtonBorderColor>\r\n  <SubmitButtonBorderWidth>1</SubmitButtonBorderWidth>\r\n  <SubmitButtonBorderStyle>4</SubmitButtonBorderStyle>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>PasswordRecoveryScheme_Colorful</SchemeName>\r\n  <BackColor>#FFFBD6</BackColor>\r\n  <ForeColor>#333333</ForeColor>\r\n  <BorderColor>#FFDFAD</BorderColor>\r\n  <BorderWidth>1</BorderWidth>\r\n  <BorderStyle>4</BorderStyle>\r\n  <BorderPadding>4</BorderPadding>\r\n  <FontSize>0.8em</FontSize>\r\n  <FontName>Verdana</FontName>\r\n  <TitleTextBackColor>#990000</TitleTextBackColor>\r\n  <TitleTextForeColor>White</TitleTextForeColor>\r\n  <TitleTextFont>1</TitleTextFont>\r\n  <TitleTextFontSize>0.9em</TitleTextFontSize>\r\n  <InstructionTextForeColor>Black</InstructionTextForeColor>\r\n  <InstructionTextFont>2</InstructionTextFont>\r\n  <SuccessTextForeColor>#990000</SuccessTextForeColor>\r\n  <SuccessTextFont>1</SuccessTextFont>\r\n  <TextboxFontSize>0.8em</TextboxFontSize>\r\n  <SubmitButtonBackColor>White</SubmitButtonBackColor>\r\n  <SubmitButtonForeColor>#990000</SubmitButtonForeColor>\r\n  <SubmitButtonFontSize>0.8em</SubmitButtonFontSize>\r\n  <SubmitButtonFontName>Verdana</SubmitButtonFontName>\r\n  <SubmitButtonBorderColor>#CC9966</SubmitButtonBorderColor>\r\n  <SubmitButtonBorderWidth>1</SubmitButtonBorderWidth>\r\n  <SubmitButtonBorderStyle>4</SubmitButtonBorderStyle>\r\n</Scheme>\r\n</Schemes>\r\n", (DataRow schemeData) => new PasswordRecoveryAutoFormat(schemeData));
				}
				return PasswordRecoveryDesigner._autoFormats;
			}
		}

		private PasswordRecoveryDesigner.ViewType CurrentView
		{
			get
			{
				object obj = base.DesignerState["CurrentView"];
				if (obj != null)
				{
					return (PasswordRecoveryDesigner.ViewType)obj;
				}
				return PasswordRecoveryDesigner.ViewType.UserName;
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
				return this.GetTemplate(this._passwordRecovery) != null;
			}
		}

		private TemplateDefinition TemplateDefinition
		{
			get
			{
				string text = PasswordRecoveryDesigner._templateNames[(int)this.CurrentView];
				return new TemplateDefinition(this, text, this._passwordRecovery, text, ((WebControl)base.ViewControl).ControlStyle);
			}
		}

		private PropertyDescriptor TemplateDescriptor
		{
			get
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(base.Component);
				string text = PasswordRecoveryDesigner._templateNames[(int)this.CurrentView];
				return properties.Find(text, false);
			}
		}

		public override TemplateGroupCollection TemplateGroups
		{
			get
			{
				TemplateGroupCollection templateGroups = base.TemplateGroups;
				TemplateGroupCollection templateGroupCollection = new TemplateGroupCollection();
				for (int i = 0; i < PasswordRecoveryDesigner._templateNames.Length; i++)
				{
					string text = PasswordRecoveryDesigner._templateNames[i];
					TemplateGroup templateGroup = new TemplateGroup(text, ((WebControl)base.ViewControl).ControlStyle);
					templateGroup.AddTemplateDefinition(new TemplateDefinition(this, text, this._passwordRecovery, text, ((WebControl)base.ViewControl).ControlStyle));
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
				PasswordRecoveryDesigner.ConvertToTemplateHelper convertToTemplateHelper = new PasswordRecoveryDesigner.ConvertToTemplateHelper(this, designerHost);
				ITemplate template = convertToTemplateHelper.ConvertToTemplate();
				this.TemplateDescriptor.SetValue(this._passwordRecovery, template);
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
			string text;
			try
			{
				IDictionary dictionary = new HybridDictionary(1);
				dictionary["CurrentView"] = this.CurrentView;
				((IControlDesignerAccessor)base.ViewControl).SetDesignModeState(dictionary);
				ICompositeControlDesignerAccessor compositeControlDesignerAccessor = (ICompositeControlDesignerAccessor)base.ViewControl;
				compositeControlDesignerAccessor.RecreateChildControls();
				text = base.GetDesignTimeHtml();
			}
			catch (Exception ex)
			{
				text = this.GetErrorDesignTimeHtml(ex);
			}
			return text;
		}

		protected override string GetErrorDesignTimeHtml(Exception e)
		{
			return base.CreatePlaceHolderDesignTimeHtml(SR.GetString("Control_ErrorRenderingShort") + "<br />" + e.Message);
		}

		private ITemplate GetTemplate(PasswordRecovery passwordRecovery)
		{
			ITemplate template = null;
			switch (this.CurrentView)
			{
			case PasswordRecoveryDesigner.ViewType.UserName:
				template = passwordRecovery.UserNameTemplate;
				break;
			case PasswordRecoveryDesigner.ViewType.Question:
				template = passwordRecovery.QuestionTemplate;
				break;
			case PasswordRecoveryDesigner.ViewType.Success:
				template = passwordRecovery.SuccessTemplate;
				break;
			}
			return template;
		}

		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(PasswordRecovery));
			this._passwordRecovery = (PasswordRecovery)component;
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
				foreach (string text in PasswordRecoveryDesigner._nonTemplateProperties)
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
			bool flag;
			try
			{
				this.TemplateDescriptor.SetValue(this._passwordRecovery, null);
				flag = true;
			}
			catch (Exception)
			{
				flag = false;
			}
			return flag;
		}

		public override string GetDesignTimeHtml(DesignerRegionCollection regions)
		{
			bool flag = base.UseRegions(regions, this.GetTemplate(this._passwordRecovery));
			if (flag)
			{
				regions.Add(new TemplatedEditableDesignerRegion(this.TemplateDefinition)
				{
					Description = SR.GetString("ContainerControlDesigner_RegionWatermark")
				});
				((WebControl)base.ViewControl).Enabled = true;
				IDictionary dictionary = new HybridDictionary(1);
				dictionary.Add("RegionEditing", true);
				((IControlDesignerAccessor)base.ViewControl).SetDesignModeState(dictionary);
			}
			return this.GetDesignTimeHtml();
		}

		public override string GetEditableDesignerRegionContent(EditableDesignerRegion region)
		{
			ITemplate template = this.GetTemplate(this._passwordRecovery);
			if (template == null)
			{
				return string.Empty;
			}
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			return ControlPersister.PersistTemplate(template, designerHost);
		}

		public override void SetEditableDesignerRegionContent(EditableDesignerRegion region, string content)
		{
			IDesignerHost designerHost = (IDesignerHost)base.Component.Site.GetService(typeof(IDesignerHost));
			ITemplate template = ControlParser.ParseTemplate(designerHost, content);
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)[region.Name];
			using (DesignerTransaction designerTransaction = designerHost.CreateTransaction("SetEditableDesignerRegionContent"))
			{
				propertyDescriptor.SetValue(base.Component, template);
				designerTransaction.Commit();
			}
		}

		private const string _failureTextID = "FailureText";

		private PasswordRecovery _passwordRecovery;

		private static DesignerAutoFormatCollection _autoFormats;

		private static readonly string[] _userNameViewRegionToPropertyMap = new string[] { "UserNameLabelText", "UserNameTitleText", "UserNameInstructionText" };

		private static readonly string[] _questionViewRegionToPropertyMap = new string[] { "UserNameLabelText", "QuestionTitleText", "QuestionLabelText", "QuestionInstructionText", "AnswerLabelText" };

		private static readonly string[] _successViewRegionToPropertyMap = new string[] { "SuccessText" };

		private static readonly string[] _templateNames = new string[] { "UserNameTemplate", "QuestionTemplate", "SuccessTemplate" };

		private static readonly string[] _nonTemplateProperties = new string[]
		{
			"AnswerLabelText", "AnswerRequiredErrorMessage", "BorderPadding", "HelpPageIconUrl", "FailureTextStyle", "HelpPageText", "HelpPageUrl", "HyperLinkStyle", "InstructionTextStyle", "LabelStyle",
			"QuestionInstructionText", "QuestionLabelText", "QuestionTitleText", "SubmitButtonImageUrl", "SubmitButtonStyle", "SubmitButtonText", "SubmitButtonType", "SuccessText", "SuccessTextStyle", "TextBoxStyle",
			"TextLayout", "TitleTextStyle", "UserNameInstructionText", "UserNameLabelText", "UserNameRequiredErrorMessage", "UserNameTitleText", "ValidatorTextStyle"
		};

		[CompilerGenerated]
		private static ControlDesigner.CreateAutoFormatDelegate <>9__CachedAnonymousMethodDelegate1;

		private enum ViewType
		{
			UserName,
			Question,
			Success
		}

		private class PasswordRecoveryDesignerActionList : DesignerActionList
		{
			public PasswordRecoveryDesignerActionList(PasswordRecoveryDesigner designer)
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

			[TypeConverter(typeof(PasswordRecoveryDesigner.PasswordRecoveryDesignerActionList.PasswordRecoveryViewTypeConverter))]
			public string View
			{
				get
				{
					if (this._designer.CurrentView == PasswordRecoveryDesigner.ViewType.UserName)
					{
						return SR.GetString("PasswordRecovery_UserNameView");
					}
					if (this._designer.CurrentView == PasswordRecoveryDesigner.ViewType.Question)
					{
						return SR.GetString("PasswordRecovery_QuestionView");
					}
					if (this._designer.CurrentView == PasswordRecoveryDesigner.ViewType.Success)
					{
						return SR.GetString("PasswordRecovery_SuccessView");
					}
					return string.Empty;
				}
				set
				{
					if (string.Compare(value, SR.GetString("PasswordRecovery_UserNameView"), StringComparison.Ordinal) == 0)
					{
						this._designer.CurrentView = PasswordRecoveryDesigner.ViewType.UserName;
					}
					else if (string.Compare(value, SR.GetString("PasswordRecovery_QuestionView"), StringComparison.Ordinal) == 0)
					{
						this._designer.CurrentView = PasswordRecoveryDesigner.ViewType.Question;
					}
					else if (string.Compare(value, SR.GetString("PasswordRecovery_SuccessView"), StringComparison.Ordinal) == 0)
					{
						this._designer.CurrentView = PasswordRecoveryDesigner.ViewType.Success;
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
				if (!this._designer.InTemplateMode)
				{
					if (this._designer.Templated)
					{
						designerActionItemCollection.Add(new DesignerActionMethodItem(this, "Reset", SR.GetString("WebControls_Reset"), string.Empty, SR.GetString("WebControls_ResetDescriptionViews"), true));
					}
					else
					{
						designerActionItemCollection.Add(new DesignerActionMethodItem(this, "ConvertToTemplate", SR.GetString("WebControls_ConvertToTemplate"), string.Empty, SR.GetString("WebControls_ConvertToTemplateDescriptionViews"), true));
					}
				}
				designerActionItemCollection.Add(new DesignerActionMethodItem(this, "LaunchWebAdmin", SR.GetString("Login_LaunchWebAdmin"), string.Empty, SR.GetString("Login_LaunchWebAdminDescription"), true));
				return designerActionItemCollection;
			}

			public void Reset()
			{
				this._designer.Reset();
			}

			private PasswordRecoveryDesigner _designer;

			private class PasswordRecoveryViewTypeConverter : TypeConverter
			{
				public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
				{
					return new TypeConverter.StandardValuesCollection(new string[]
					{
						SR.GetString("PasswordRecovery_UserNameView"),
						SR.GetString("PasswordRecovery_QuestionView"),
						SR.GetString("PasswordRecovery_SuccessView")
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

		private sealed class ConvertToTemplateHelper : LoginDesignerUtil.GenericConvertToTemplateHelper<PasswordRecovery, PasswordRecoveryDesigner>
		{
			public ConvertToTemplateHelper(PasswordRecoveryDesigner designer, IDesignerHost designerHost)
				: base(designer, designerHost)
			{
			}

			protected override string[] PersistedControlIDs
			{
				get
				{
					return PasswordRecoveryDesigner.ConvertToTemplateHelper._persistedControlIDs;
				}
			}

			protected override string[] PersistedIfNotVisibleControlIDs
			{
				get
				{
					return PasswordRecoveryDesigner.ConvertToTemplateHelper._persistedIfNotVisibleControlIDs;
				}
			}

			protected override Style GetFailureTextStyle(PasswordRecovery control)
			{
				return control.FailureTextStyle;
			}

			protected override Control GetDefaultTemplateContents()
			{
				Control control = null;
				switch (base.Designer.CurrentView)
				{
				case PasswordRecoveryDesigner.ViewType.UserName:
					control = base.Designer.ViewControl.Controls[0];
					break;
				case PasswordRecoveryDesigner.ViewType.Question:
					control = base.Designer.ViewControl.Controls[1];
					break;
				case PasswordRecoveryDesigner.ViewType.Success:
					control = base.Designer.ViewControl.Controls[2];
					break;
				}
				return (Table)control.Controls[0];
			}

			protected override ITemplate GetTemplate(PasswordRecovery control)
			{
				return base.Designer.GetTemplate(control);
			}

			private static readonly string[] _persistedControlIDs = new string[] { "UserName", "UserNameRequired", "Question", "Answer", "AnswerRequired", "SubmitButton", "SubmitImageButton", "SubmitLinkButton", "FailureText", "HelpLink" };

			private static readonly string[] _persistedIfNotVisibleControlIDs = new string[] { "UserName", "Question", "FailureText" };
		}
	}
}
