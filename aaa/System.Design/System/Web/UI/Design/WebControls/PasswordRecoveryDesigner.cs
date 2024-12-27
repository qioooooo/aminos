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
	// Token: 0x0200049E RID: 1182
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class PasswordRecoveryDesigner : ControlDesigner
	{
		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x06002AD0 RID: 10960 RVA: 0x000ED29C File Offset: 0x000EC29C
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

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x06002AD1 RID: 10961 RVA: 0x000ED2D1 File Offset: 0x000EC2D1
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

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x06002AD2 RID: 10962 RVA: 0x000ED30C File Offset: 0x000EC30C
		// (set) Token: 0x06002AD3 RID: 10963 RVA: 0x000ED335 File Offset: 0x000EC335
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

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x06002AD4 RID: 10964 RVA: 0x000ED34D File Offset: 0x000EC34D
		private bool Templated
		{
			get
			{
				return this.GetTemplate(this._passwordRecovery) != null;
			}
		}

		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x06002AD5 RID: 10965 RVA: 0x000ED364 File Offset: 0x000EC364
		private TemplateDefinition TemplateDefinition
		{
			get
			{
				string text = PasswordRecoveryDesigner._templateNames[(int)this.CurrentView];
				return new TemplateDefinition(this, text, this._passwordRecovery, text, ((WebControl)base.ViewControl).ControlStyle);
			}
		}

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x06002AD6 RID: 10966 RVA: 0x000ED39C File Offset: 0x000EC39C
		private PropertyDescriptor TemplateDescriptor
		{
			get
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(base.Component);
				string text = PasswordRecoveryDesigner._templateNames[(int)this.CurrentView];
				return properties.Find(text, false);
			}
		}

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x06002AD7 RID: 10967 RVA: 0x000ED3CC File Offset: 0x000EC3CC
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

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x06002AD8 RID: 10968 RVA: 0x000ED44E File Offset: 0x000EC44E
		protected override bool UsePreviewControl
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002AD9 RID: 10969 RVA: 0x000ED454 File Offset: 0x000EC454
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

		// Token: 0x06002ADA RID: 10970 RVA: 0x000ED4B4 File Offset: 0x000EC4B4
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

		// Token: 0x06002ADB RID: 10971 RVA: 0x000ED524 File Offset: 0x000EC524
		protected override string GetErrorDesignTimeHtml(Exception e)
		{
			return base.CreatePlaceHolderDesignTimeHtml(SR.GetString("Control_ErrorRenderingShort") + "<br />" + e.Message);
		}

		// Token: 0x06002ADC RID: 10972 RVA: 0x000ED548 File Offset: 0x000EC548
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

		// Token: 0x06002ADD RID: 10973 RVA: 0x000ED58C File Offset: 0x000EC58C
		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(PasswordRecovery));
			this._passwordRecovery = (PasswordRecovery)component;
			base.Initialize(component);
		}

		// Token: 0x06002ADE RID: 10974 RVA: 0x000ED5B4 File Offset: 0x000EC5B4
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

		// Token: 0x06002ADF RID: 10975 RVA: 0x000ED5FA File Offset: 0x000EC5FA
		private void ConvertToTemplate()
		{
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ConvertToTemplateChangeCallback), null, SR.GetString("WebControls_ConvertToTemplate"), this.TemplateDescriptor);
		}

		// Token: 0x06002AE0 RID: 10976 RVA: 0x000ED624 File Offset: 0x000EC624
		private void Reset()
		{
			this.UpdateDesignTimeHtml();
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ResetChangeCallback), null, SR.GetString("WebControls_Reset"), this.TemplateDescriptor);
		}

		// Token: 0x06002AE1 RID: 10977 RVA: 0x000ED654 File Offset: 0x000EC654
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

		// Token: 0x06002AE2 RID: 10978 RVA: 0x000ED6C0 File Offset: 0x000EC6C0
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

		// Token: 0x06002AE3 RID: 10979 RVA: 0x000ED6FC File Offset: 0x000EC6FC
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

		// Token: 0x06002AE4 RID: 10980 RVA: 0x000ED780 File Offset: 0x000EC780
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

		// Token: 0x06002AE5 RID: 10981 RVA: 0x000ED7C0 File Offset: 0x000EC7C0
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

		// Token: 0x04001D48 RID: 7496
		private const string _failureTextID = "FailureText";

		// Token: 0x04001D49 RID: 7497
		private PasswordRecovery _passwordRecovery;

		// Token: 0x04001D4A RID: 7498
		private static DesignerAutoFormatCollection _autoFormats;

		// Token: 0x04001D4B RID: 7499
		private static readonly string[] _userNameViewRegionToPropertyMap = new string[] { "UserNameLabelText", "UserNameTitleText", "UserNameInstructionText" };

		// Token: 0x04001D4C RID: 7500
		private static readonly string[] _questionViewRegionToPropertyMap = new string[] { "UserNameLabelText", "QuestionTitleText", "QuestionLabelText", "QuestionInstructionText", "AnswerLabelText" };

		// Token: 0x04001D4D RID: 7501
		private static readonly string[] _successViewRegionToPropertyMap = new string[] { "SuccessText" };

		// Token: 0x04001D4E RID: 7502
		private static readonly string[] _templateNames = new string[] { "UserNameTemplate", "QuestionTemplate", "SuccessTemplate" };

		// Token: 0x04001D4F RID: 7503
		private static readonly string[] _nonTemplateProperties = new string[]
		{
			"AnswerLabelText", "AnswerRequiredErrorMessage", "BorderPadding", "HelpPageIconUrl", "FailureTextStyle", "HelpPageText", "HelpPageUrl", "HyperLinkStyle", "InstructionTextStyle", "LabelStyle",
			"QuestionInstructionText", "QuestionLabelText", "QuestionTitleText", "SubmitButtonImageUrl", "SubmitButtonStyle", "SubmitButtonText", "SubmitButtonType", "SuccessText", "SuccessTextStyle", "TextBoxStyle",
			"TextLayout", "TitleTextStyle", "UserNameInstructionText", "UserNameLabelText", "UserNameRequiredErrorMessage", "UserNameTitleText", "ValidatorTextStyle"
		};

		// Token: 0x04001D50 RID: 7504
		[CompilerGenerated]
		private static ControlDesigner.CreateAutoFormatDelegate <>9__CachedAnonymousMethodDelegate1;

		// Token: 0x0200049F RID: 1183
		private enum ViewType
		{
			// Token: 0x04001D52 RID: 7506
			UserName,
			// Token: 0x04001D53 RID: 7507
			Question,
			// Token: 0x04001D54 RID: 7508
			Success
		}

		// Token: 0x020004A0 RID: 1184
		private class PasswordRecoveryDesignerActionList : DesignerActionList
		{
			// Token: 0x06002AE9 RID: 10985 RVA: 0x000EDA06 File Offset: 0x000ECA06
			public PasswordRecoveryDesignerActionList(PasswordRecoveryDesigner designer)
				: base(designer.Component)
			{
				this._designer = designer;
			}

			// Token: 0x17000800 RID: 2048
			// (get) Token: 0x06002AEA RID: 10986 RVA: 0x000EDA1B File Offset: 0x000ECA1B
			// (set) Token: 0x06002AEB RID: 10987 RVA: 0x000EDA1E File Offset: 0x000ECA1E
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

			// Token: 0x17000801 RID: 2049
			// (get) Token: 0x06002AEC RID: 10988 RVA: 0x000EDA20 File Offset: 0x000ECA20
			// (set) Token: 0x06002AED RID: 10989 RVA: 0x000EDA7C File Offset: 0x000ECA7C
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

			// Token: 0x06002AEE RID: 10990 RVA: 0x000EDB08 File Offset: 0x000ECB08
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

			// Token: 0x06002AEF RID: 10991 RVA: 0x000EDB4C File Offset: 0x000ECB4C
			public void LaunchWebAdmin()
			{
				this._designer.LaunchWebAdmin();
			}

			// Token: 0x06002AF0 RID: 10992 RVA: 0x000EDB5C File Offset: 0x000ECB5C
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

			// Token: 0x06002AF1 RID: 10993 RVA: 0x000EDC3A File Offset: 0x000ECC3A
			public void Reset()
			{
				this._designer.Reset();
			}

			// Token: 0x04001D55 RID: 7509
			private PasswordRecoveryDesigner _designer;

			// Token: 0x020004A1 RID: 1185
			private class PasswordRecoveryViewTypeConverter : TypeConverter
			{
				// Token: 0x06002AF2 RID: 10994 RVA: 0x000EDC48 File Offset: 0x000ECC48
				public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
				{
					return new TypeConverter.StandardValuesCollection(new string[]
					{
						SR.GetString("PasswordRecovery_UserNameView"),
						SR.GetString("PasswordRecovery_QuestionView"),
						SR.GetString("PasswordRecovery_SuccessView")
					});
				}

				// Token: 0x06002AF3 RID: 10995 RVA: 0x000EDC89 File Offset: 0x000ECC89
				public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
				{
					return true;
				}

				// Token: 0x06002AF4 RID: 10996 RVA: 0x000EDC8C File Offset: 0x000ECC8C
				public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
				{
					return true;
				}
			}
		}

		// Token: 0x020004A2 RID: 1186
		private sealed class ConvertToTemplateHelper : LoginDesignerUtil.GenericConvertToTemplateHelper<PasswordRecovery, PasswordRecoveryDesigner>
		{
			// Token: 0x06002AF6 RID: 10998 RVA: 0x000EDC97 File Offset: 0x000ECC97
			public ConvertToTemplateHelper(PasswordRecoveryDesigner designer, IDesignerHost designerHost)
				: base(designer, designerHost)
			{
			}

			// Token: 0x17000802 RID: 2050
			// (get) Token: 0x06002AF7 RID: 10999 RVA: 0x000EDCA1 File Offset: 0x000ECCA1
			protected override string[] PersistedControlIDs
			{
				get
				{
					return PasswordRecoveryDesigner.ConvertToTemplateHelper._persistedControlIDs;
				}
			}

			// Token: 0x17000803 RID: 2051
			// (get) Token: 0x06002AF8 RID: 11000 RVA: 0x000EDCA8 File Offset: 0x000ECCA8
			protected override string[] PersistedIfNotVisibleControlIDs
			{
				get
				{
					return PasswordRecoveryDesigner.ConvertToTemplateHelper._persistedIfNotVisibleControlIDs;
				}
			}

			// Token: 0x06002AF9 RID: 11001 RVA: 0x000EDCAF File Offset: 0x000ECCAF
			protected override Style GetFailureTextStyle(PasswordRecovery control)
			{
				return control.FailureTextStyle;
			}

			// Token: 0x06002AFA RID: 11002 RVA: 0x000EDCB8 File Offset: 0x000ECCB8
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

			// Token: 0x06002AFB RID: 11003 RVA: 0x000EDD43 File Offset: 0x000ECD43
			protected override ITemplate GetTemplate(PasswordRecovery control)
			{
				return base.Designer.GetTemplate(control);
			}

			// Token: 0x04001D56 RID: 7510
			private static readonly string[] _persistedControlIDs = new string[] { "UserName", "UserNameRequired", "Question", "Answer", "AnswerRequired", "SubmitButton", "SubmitImageButton", "SubmitLinkButton", "FailureText", "HelpLink" };

			// Token: 0x04001D57 RID: 7511
			private static readonly string[] _persistedIfNotVisibleControlIDs = new string[] { "UserName", "Question", "FailureText" };
		}
	}
}
