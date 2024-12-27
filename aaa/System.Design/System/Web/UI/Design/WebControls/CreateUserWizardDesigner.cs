using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000417 RID: 1047
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class CreateUserWizardDesigner : WizardDesigner
	{
		// Token: 0x06002665 RID: 9829 RVA: 0x000CF608 File Offset: 0x000CE608
		static CreateUserWizardDesigner()
		{
			CreateUserWizardDesigner._persistedIDConverter.Add("CancelButtonImageButton", "CancelButton");
			CreateUserWizardDesigner._persistedIDConverter.Add("CancelButtonButton", "CancelButton");
			CreateUserWizardDesigner._persistedIDConverter.Add("CancelButtonLinkButton", "CancelButton");
			CreateUserWizardDesigner._persistedIDConverter.Add("StepNextButtonImageButton", "StepNextButton");
			CreateUserWizardDesigner._persistedIDConverter.Add("StepNextButtonButton", "StepNextButton");
			CreateUserWizardDesigner._persistedIDConverter.Add("StepNextButtonLinkButton", "StepNextButton");
			CreateUserWizardDesigner._persistedIDConverter.Add("StepPreviousButtonImageButton", "StepNextButton");
			CreateUserWizardDesigner._persistedIDConverter.Add("StepPreviousButton", "StepNextButton");
			CreateUserWizardDesigner._persistedIDConverter.Add("StepPreviousButtonLinkButton", "StepNextButton");
			CreateUserWizardDesigner._completeStepConverter = new Hashtable();
			CreateUserWizardDesigner._completeStepConverter.Add("ContinueButtonImageButton", "ContinueButton");
			CreateUserWizardDesigner._completeStepConverter.Add("ContinueButtonButton", "ContinueButton");
			CreateUserWizardDesigner._completeStepConverter.Add("ContinueButtonLinkButton", "ContinueButton");
		}

		// Token: 0x06002666 RID: 9830 RVA: 0x000CF9A8 File Offset: 0x000CE9A8
		private static bool IsStepEmpty(WizardStepBase step)
		{
			if (!(step is CreateUserWizardStep) && !(step is CompleteWizardStep))
			{
				return false;
			}
			TemplatedWizardStep templatedWizardStep = (TemplatedWizardStep)step;
			return templatedWizardStep.ContentTemplate == null;
		}

		// Token: 0x06002667 RID: 9831 RVA: 0x000CF9D7 File Offset: 0x000CE9D7
		internal override bool InRegionEditingMode(Wizard viewControl)
		{
			return !base.SupportsDesignerRegions || CreateUserWizardDesigner.IsStepEmpty(this._createUserWizard.ActiveStep);
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x06002668 RID: 9832 RVA: 0x000CF9F8 File Offset: 0x000CE9F8
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection designerActionListCollection = new DesignerActionListCollection();
				designerActionListCollection.AddRange(base.ActionLists);
				designerActionListCollection.Add(new CreateUserWizardDesigner.CreateUserWizardDesignerActionList(this));
				return designerActionListCollection;
			}
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x06002669 RID: 9833 RVA: 0x000CFA2D File Offset: 0x000CEA2D
		public override DesignerAutoFormatCollection AutoFormats
		{
			get
			{
				if (CreateUserWizardDesigner._autoFormats == null)
				{
					CreateUserWizardDesigner._autoFormats = ControlDesigner.CreateAutoFormats("<Schemes>\r\n<xsd:schema id=\"Schemes\" xmlns=\"\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\">\r\n  <xsd:element name=\"Scheme\">\r\n     <xsd:complexType>\r\n       <xsd:all>\r\n        <xsd:element name=\"SchemeName\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"BackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"ForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"BorderColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"BorderWidth\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"BorderStyle\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"BorderPadding\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"FontSize\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"FontName\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"TextLayout\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"TitleTextBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"TitleTextForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"TitleTextFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"TitleTextFontSize\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"InstructionTextForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"InstructionTextFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"TextboxFontSize\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"NavigationButtonStyleBorderWidth\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"NavigationButtonStyleFontName\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"NavigationButtonStyleFontSize\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"NavigationButtonStyleBorderStyle\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"NavigationButtonStyleBorderColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"NavigationButtonStyleForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"NavigationButtonStyleBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"StepStyleBorderWidth\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"StepStyleBorderStyle\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"StepStyleBorderColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"StepStyleForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"StepStyleBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"StepStyleFontSize\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SideBarButtonStyleFontUnderline\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SideBarButtonStyleFontName\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SideBarButtonStyleForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SideBarButtonStyleBorderWidth\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SideBarButtonStyleBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"HeaderStyleForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"HeaderStyleBorderColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"HeaderStyleBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"HeaderStyleFontSize\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"HeaderStyleFontBold\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"HeaderStyleBorderWidth\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"HeaderStyleHorizontalAlign\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"HeaderStyleBorderStyle\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SideBarStyleBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SideBarStyleVerticalAlign\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SideBarStyleFontSize\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SideBarStyleFontUnderline\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SideBarStyleFontStrikeout\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SideBarStyleBorderWidth\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n      </xsd:all>\r\n    </xsd:complexType>\r\n  </xsd:element>\r\n  <xsd:element name=\"Schemes\" msdata:IsDataSet=\"true\">\r\n    <xsd:complexType>\r\n      <xsd:choice maxOccurs=\"unbounded\">\r\n        <xsd:element ref=\"Scheme\"/>\r\n      </xsd:choice>\r\n    </xsd:complexType>\r\n  </xsd:element>\r\n</xsd:schema>\r\n<Scheme>\r\n  <SchemeName>CreateUserWizardScheme_Empty</SchemeName>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>CreateUserWizardScheme_Elegant</SchemeName>\r\n  <BackColor>#F7F7DE</BackColor>\r\n  <BorderColor>#CCCC99</BorderColor>\r\n  <BorderWidth>1</BorderWidth>\r\n  <BorderStyle>4</BorderStyle>\r\n  <FontSize>10</FontSize>\r\n  <FontName>Verdana</FontName>\r\n  <TitleTextBackColor>#6B696B</TitleTextBackColor>\r\n  <TitleTextForeColor>#FFFFFF</TitleTextForeColor>\r\n  <TitleTextFont>1</TitleTextFont>\r\n  <StepStyleBorderWidth>0px</StepStyleBorderWidth>\r\n  <NavigationButtonStyleBorderWidth>1px</NavigationButtonStyleBorderWidth>\r\n  <NavigationButtonStyleFontName>Verdana</NavigationButtonStyleFontName>\r\n  <NavigationButtonStyleBorderStyle>4</NavigationButtonStyleBorderStyle>\r\n  <NavigationButtonStyleBorderColor>#CCCCCC</NavigationButtonStyleBorderColor>\r\n  <NavigationButtonStyleForeColor>#284775</NavigationButtonStyleForeColor>\r\n  <NavigationButtonStyleBackColor>#FFFBFF</NavigationButtonStyleBackColor>\r\n  <SideBarButtonStyleFontUnderline>False</SideBarButtonStyleFontUnderline>\r\n  <SideBarButtonStyleFontName>Verdana</SideBarButtonStyleFontName>\r\n  <SideBarButtonStyleForeColor>#FFFFFF</SideBarButtonStyleForeColor>\r\n  <SideBarButtonStyleBorderWidth>0px</SideBarButtonStyleBorderWidth>\r\n  <HeaderStyleForeColor>#FFFFFF</HeaderStyleForeColor>\r\n  <HeaderStyleBackColor>#6B696B</HeaderStyleBackColor>\r\n  <HeaderStyleFontBold>True</HeaderStyleFontBold>\r\n  <HeaderStyleHorizontalAlign>2</HeaderStyleHorizontalAlign>\r\n  <SideBarStyleBackColor>#7C6F57</SideBarStyleBackColor>\r\n  <SideBarStyleVerticalAlign>1</SideBarStyleVerticalAlign>\r\n  <SideBarStyleFontSize>0.9em</SideBarStyleFontSize>\r\n  <SideBarStyleBorderWidth>0px</SideBarStyleBorderWidth>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>CreateUserWizardScheme_Professional</SchemeName>\r\n  <BackColor>#F7F6F3</BackColor>\r\n  <ForeColor>#333333</ForeColor>\r\n  <BorderColor>#E6E2D8</BorderColor>\r\n  <BorderWidth>1</BorderWidth>\r\n  <BorderStyle>4</BorderStyle>\r\n  <BorderPadding>4</BorderPadding>\r\n  <FontSize>0.8em</FontSize>\r\n  <FontName>Verdana</FontName>\r\n  <TitleTextBackColor>#5D7B9D</TitleTextBackColor>\r\n  <TitleTextForeColor>White</TitleTextForeColor>\r\n  <TitleTextFont>1</TitleTextFont>\r\n  <TitleTextFontSize>0.9em</TitleTextFontSize>\r\n  <InstructionTextForeColor>Black</InstructionTextForeColor>\r\n  <InstructionTextFont>2</InstructionTextFont>\r\n  <TextboxFontSize>0.8em</TextboxFontSize>\r\n  <StepStyleBorderWidth>0px</StepStyleBorderWidth>\r\n  <NavigationButtonStyleBorderWidth>1px</NavigationButtonStyleBorderWidth>\r\n  <NavigationButtonStyleFontName>Verdana</NavigationButtonStyleFontName>\r\n  <NavigationButtonStyleBorderStyle>4</NavigationButtonStyleBorderStyle>\r\n  <NavigationButtonStyleBorderColor>#CCCCCC</NavigationButtonStyleBorderColor>\r\n  <NavigationButtonStyleForeColor>#284775</NavigationButtonStyleForeColor>\r\n  <NavigationButtonStyleBackColor>#FFFBFF</NavigationButtonStyleBackColor>\r\n  <SideBarButtonStyleFontUnderline>False</SideBarButtonStyleFontUnderline>\r\n  <SideBarButtonStyleFontName>Verdana</SideBarButtonStyleFontName>\r\n  <SideBarButtonStyleForeColor>White</SideBarButtonStyleForeColor>\r\n  <SideBarButtonStyleBorderWidth>0px</SideBarButtonStyleBorderWidth>\r\n  <HeaderStyleForeColor>White</HeaderStyleForeColor>\r\n  <HeaderStyleBackColor>#5D7B9D</HeaderStyleBackColor>\r\n  <HeaderStyleFontSize>0.9em</HeaderStyleFontSize>\r\n  <HeaderStyleFontBold>True</HeaderStyleFontBold>\r\n  <HeaderStyleHorizontalAlign>2</HeaderStyleHorizontalAlign>\r\n  <HeaderStyleBorderStyle>4</HeaderStyleBorderStyle>\r\n  <SideBarStyleBackColor>#5D7B9D</SideBarStyleBackColor>\r\n  <SideBarStyleVerticalAlign>1</SideBarStyleVerticalAlign>\r\n  <SideBarStyleFontSize>0.9em</SideBarStyleFontSize>\r\n  <SideBarStyleBorderWidth>0px</SideBarStyleBorderWidth>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>CreateUserWizardScheme_Simple</SchemeName>\r\n  <BackColor>#E3EAEB</BackColor>\r\n  <ForeColor>#333333</ForeColor>\r\n  <BorderColor>#E6E2D8</BorderColor>\r\n  <BorderWidth>1</BorderWidth>\r\n  <BorderStyle>4</BorderStyle>\r\n  <BorderPadding>4</BorderPadding>\r\n  <FontSize>0.8em</FontSize>\r\n  <FontName>Verdana</FontName>\r\n  <TextLayout>1</TextLayout>\r\n  <TitleTextBackColor>#1C5E55</TitleTextBackColor>\r\n  <TitleTextForeColor>White</TitleTextForeColor>\r\n  <TitleTextFont>1</TitleTextFont>\r\n  <TitleTextFontSize>0.9em</TitleTextFontSize>\r\n  <InstructionTextForeColor>Black</InstructionTextForeColor>\r\n  <InstructionTextFont>2</InstructionTextFont>\r\n  <TextboxFontSize>0.8em</TextboxFontSize>\r\n  <StepStyleBorderWidth>0px</StepStyleBorderWidth>\r\n  <NavigationButtonStyleBorderWidth>1px</NavigationButtonStyleBorderWidth>\r\n  <NavigationButtonStyleFontName>Verdana</NavigationButtonStyleFontName>\r\n  <NavigationButtonStyleBorderStyle>4</NavigationButtonStyleBorderStyle>\r\n  <NavigationButtonStyleBorderColor>#C5BBAF</NavigationButtonStyleBorderColor>\r\n  <NavigationButtonStyleForeColor>#1C5E55</NavigationButtonStyleForeColor>\r\n  <NavigationButtonStyleBackColor>White</NavigationButtonStyleBackColor>\r\n  <SideBarButtonStyleFontUnderline>False</SideBarButtonStyleFontUnderline>\r\n  <SideBarButtonStyleForeColor>White</SideBarButtonStyleForeColor>\r\n  <HeaderStyleForeColor>White</HeaderStyleForeColor>\r\n  <HeaderStyleBackColor>#666666</HeaderStyleBackColor>\r\n  <HeaderStyleBorderColor>#E6E2D8</HeaderStyleBorderColor>\r\n  <HeaderStyleFontSize>0.9em</HeaderStyleFontSize>\r\n  <HeaderStyleFontBold>True</HeaderStyleFontBold>\r\n  <HeaderStyleHorizontalAlign>2</HeaderStyleHorizontalAlign>\r\n  <HeaderStyleBorderStyle>4</HeaderStyleBorderStyle>\r\n  <HeaderStyleBorderWidth>2px</HeaderStyleBorderWidth>\r\n  <SideBarStyleBackColor>#1C5E55</SideBarStyleBackColor>\r\n  <SideBarStyleVerticalAlign>1</SideBarStyleVerticalAlign>\r\n  <SideBarStyleFontSize>0.9em</SideBarStyleFontSize>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>CreateUserWizardScheme_Classic</SchemeName>\r\n  <BackColor>#EFF3FB</BackColor>\r\n  <ForeColor>#333333</ForeColor>\r\n  <BorderColor>#B5C7DE</BorderColor>\r\n  <BorderWidth>1</BorderWidth>\r\n  <BorderStyle>4</BorderStyle>\r\n  <BorderPadding>4</BorderPadding>\r\n  <FontSize>0.8em</FontSize>\r\n  <FontName>Verdana</FontName>\r\n  <TitleTextBackColor>#507CD1</TitleTextBackColor>\r\n  <TitleTextForeColor>White</TitleTextForeColor>\r\n  <TitleTextFont>1</TitleTextFont>\r\n  <TitleTextFontSize>0.9em</TitleTextFontSize>\r\n  <InstructionTextForeColor>Black</InstructionTextForeColor>\r\n  <InstructionTextFont>2</InstructionTextFont>\r\n  <TextboxFontSize>0.8em</TextboxFontSize>\r\n  <StepStyleFontSize>0.8em</StepStyleFontSize>\r\n  <NavigationButtonStyleBorderWidth>1px</NavigationButtonStyleBorderWidth>\r\n  <NavigationButtonStyleFontName>Verdana</NavigationButtonStyleFontName>\r\n  <NavigationButtonStyleBorderStyle>4</NavigationButtonStyleBorderStyle>\r\n  <NavigationButtonStyleBorderColor>#507CD1</NavigationButtonStyleBorderColor>\r\n  <NavigationButtonStyleForeColor>#284E98</NavigationButtonStyleForeColor>\r\n  <NavigationButtonStyleBackColor>White</NavigationButtonStyleBackColor>\r\n  <SideBarButtonStyleFontUnderline>False</SideBarButtonStyleFontUnderline>\r\n  <SideBarButtonStyleFontName>Verdana</SideBarButtonStyleFontName>\r\n  <SideBarButtonStyleForeColor>White</SideBarButtonStyleForeColor>\r\n  <SideBarButtonStyleBackColor>#507CD1</SideBarButtonStyleBackColor>\r\n  <HeaderStyleForeColor>White</HeaderStyleForeColor>\r\n  <HeaderStyleBorderColor>#EFF3FB</HeaderStyleBorderColor>\r\n  <HeaderStyleBackColor>#284E98</HeaderStyleBackColor>\r\n  <HeaderStyleFontSize>0.9em</HeaderStyleFontSize>\r\n  <HeaderStyleFontBold>True</HeaderStyleFontBold>\r\n  <HeaderStyleBorderWidth>2px</HeaderStyleBorderWidth>\r\n  <HeaderStyleHorizontalAlign>2</HeaderStyleHorizontalAlign>\r\n  <HeaderStyleBorderStyle>4</HeaderStyleBorderStyle>\r\n  <SideBarStyleBackColor>#507CD1</SideBarStyleBackColor>\r\n  <SideBarStyleVerticalAlign>1</SideBarStyleVerticalAlign>\r\n  <SideBarStyleFontSize>0.9em</SideBarStyleFontSize>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>CreateUserWizardScheme_Colorful</SchemeName>\r\n  <BackColor>#FFFBD6</BackColor>\r\n  <ForeColor>#333333</ForeColor>\r\n  <BorderColor>#FFDFAD</BorderColor>\r\n  <BorderWidth>1</BorderWidth>\r\n  <BorderStyle>4</BorderStyle>\r\n  <BorderPadding>4</BorderPadding>\r\n  <FontSize>0.8em</FontSize>\r\n  <FontName>Verdana</FontName>\r\n  <TextLayout>1</TextLayout>\r\n  <TitleTextBackColor>#990000</TitleTextBackColor>\r\n  <TitleTextForeColor>White</TitleTextForeColor>\r\n  <TitleTextFont>1</TitleTextFont>\r\n  <TitleTextFontSize>0.9em</TitleTextFontSize>\r\n  <InstructionTextForeColor>Black</InstructionTextForeColor>\r\n  <InstructionTextFont>2</InstructionTextFont>\r\n  <TextboxFontSize>0.8em</TextboxFontSize>\r\n  <NavigationButtonStyleBorderWidth>1px</NavigationButtonStyleBorderWidth>\r\n  <NavigationButtonStyleFontName>Verdana</NavigationButtonStyleFontName>\r\n  <NavigationButtonStyleBorderStyle>4</NavigationButtonStyleBorderStyle>\r\n  <NavigationButtonStyleBorderColor>#CC9966</NavigationButtonStyleBorderColor>\r\n  <NavigationButtonStyleForeColor>#990000</NavigationButtonStyleForeColor>\r\n  <NavigationButtonStyleBackColor>White</NavigationButtonStyleBackColor>\r\n  <SideBarButtonStyleFontUnderline>False</SideBarButtonStyleFontUnderline>\r\n  <SideBarButtonStyleForeColor>White</SideBarButtonStyleForeColor>\r\n  <HeaderStyleForeColor>#333333</HeaderStyleForeColor>\r\n  <HeaderStyleBorderColor>#FFFBD6</HeaderStyleBorderColor>\r\n  <HeaderStyleBackColor>#FFCC66</HeaderStyleBackColor>\r\n  <HeaderStyleFontSize>0.9em</HeaderStyleFontSize>\r\n  <HeaderStyleFontBold>True</HeaderStyleFontBold>\r\n  <HeaderStyleBorderWidth>2px</HeaderStyleBorderWidth>\r\n  <HeaderStyleHorizontalAlign>2</HeaderStyleHorizontalAlign>\r\n  <HeaderStyleBorderStyle>4</HeaderStyleBorderStyle>\r\n  <SideBarStyleBackColor>#990000</SideBarStyleBackColor>\r\n  <SideBarStyleVerticalAlign>1</SideBarStyleVerticalAlign>\r\n  <SideBarStyleFontSize>0.9em</SideBarStyleFontSize>\r\n  <SideBarStyleFontUnderline>False</SideBarStyleFontUnderline>\r\n</Scheme>\r\n</Schemes>\r\n", (DataRow schemeData) => new CreateUserWizardAutoFormat(schemeData));
				}
				return CreateUserWizardDesigner._autoFormats;
			}
		}

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x0600266A RID: 9834 RVA: 0x000CFA67 File Offset: 0x000CEA67
		protected override bool UsePreviewControl
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600266B RID: 9835 RVA: 0x000CFA6C File Offset: 0x000CEA6C
		protected override void AddDesignerRegions(DesignerRegionCollection regions)
		{
			if (!base.SupportsDesignerRegions)
			{
				return;
			}
			if (this._createUserWizard.CreateUserStep == null)
			{
				this.CreateChildControls();
				if (this._createUserWizard.CreateUserStep == null)
				{
					return;
				}
			}
			bool flag = this._createUserWizard.CreateUserStep.ContentTemplate == null;
			bool flag2 = this._createUserWizard.CompleteStep.ContentTemplate == null;
			foreach (object obj in this._createUserWizard.WizardSteps)
			{
				WizardStepBase wizardStepBase = (WizardStepBase)obj;
				DesignerRegion designerRegion;
				if ((!flag || !(wizardStepBase is CreateUserWizardStep)) && (!flag2 || !(wizardStepBase is CompleteWizardStep)))
				{
					if (wizardStepBase is TemplatedWizardStep)
					{
						TemplateDefinition templateDefinition = new TemplateDefinition(this, "ContentTemplate", this._createUserWizard, "ContentTemplate", base.TemplateStyleArray[5]);
						designerRegion = new WizardStepTemplatedEditableRegion(templateDefinition, wizardStepBase);
						designerRegion.EnsureSize = false;
					}
					else
					{
						designerRegion = new WizardStepEditableRegion(this, wizardStepBase);
					}
					designerRegion.Description = SR.GetString("ContainerControlDesigner_RegionWatermark");
				}
				else
				{
					designerRegion = new WizardSelectableRegion(this, base.GetRegionName(wizardStepBase), wizardStepBase);
				}
				regions.Add(designerRegion);
			}
			foreach (object obj2 in this._createUserWizard.WizardSteps)
			{
				WizardStepBase wizardStepBase2 = (WizardStepBase)obj2;
				WizardSelectableRegion wizardSelectableRegion = new WizardSelectableRegion(this, "Move to " + base.GetRegionName(wizardStepBase2), wizardStepBase2);
				if (this._createUserWizard.ActiveStep == wizardStepBase2)
				{
					wizardSelectableRegion.Selected = true;
				}
				regions.Add(wizardSelectableRegion);
			}
		}

		// Token: 0x0600266C RID: 9836 RVA: 0x000CFC44 File Offset: 0x000CEC44
		protected override void ConvertToCustomNavigationTemplate()
		{
			try
			{
				if (this._createUserWizard.ActiveStep == this._createUserWizard.CreateUserStep)
				{
					IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
					ITemplate template = ((CreateUserWizard)base.ViewControl).CreateUserStep.CustomNavigationTemplate;
					if (template == null)
					{
						IControlDesignerAccessor createUserWizard = this._createUserWizard;
						IDictionary designModeState = createUserWizard.GetDesignModeState();
						ControlCollection controlCollection = designModeState["CustomNavigationControls"] as ControlCollection;
						if (controlCollection != null)
						{
							string text = string.Empty;
							foreach (object obj in controlCollection)
							{
								Control control = (Control)obj;
								if (control != null && control.Visible)
								{
									foreach (object obj2 in CreateUserWizardDesigner._persistedIDConverter.Keys)
									{
										string text2 = (string)obj2;
										Control control2 = control.FindControl(text2);
										if (control2 != null && control2.Visible)
										{
											control2.ID = (string)CreateUserWizardDesigner._persistedIDConverter[text2];
										}
									}
									if (control is Table)
									{
										text += this.ConvertNavigationTableToHtmlTable((Table)control);
									}
									else
									{
										StringWriter stringWriter = new StringWriter(CultureInfo.CurrentCulture);
										HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);
										control.RenderControl(htmlTextWriter);
										text += stringWriter.ToString();
									}
								}
							}
							template = ControlParser.ParseTemplate(designerHost, text);
						}
					}
					ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(base.ConvertToCustomNavigationTemplateCallBack), template, SR.GetString("Wizard_ConvertToCustomNavigationTemplate"));
					this.UpdateDesignTimeHtml();
				}
				else
				{
					base.ConvertToCustomNavigationTemplate();
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600266D RID: 9837 RVA: 0x000CFE68 File Offset: 0x000CEE68
		private string ConvertTableToHtmlTable(Table originalTable, Control container)
		{
			return this.ConvertTableToHtmlTable(originalTable, container, null);
		}

		// Token: 0x0600266E RID: 9838 RVA: 0x000CFE74 File Offset: 0x000CEE74
		private string ConvertTableToHtmlTable(Table originalTable, Control container, IDictionary persistMap)
		{
			IList list = new ArrayList();
			foreach (object obj in originalTable.Controls)
			{
				Control control = (Control)obj;
				list.Add(control);
			}
			Table table = new Table();
			foreach (object obj2 in list)
			{
				Control control2 = (Control)obj2;
				table.Controls.Add(control2);
			}
			if (originalTable.ControlStyleCreated)
			{
				table.ApplyStyle(originalTable.ControlStyle);
			}
			table.Width = ((WebControl)base.ViewControl).Width;
			table.Height = ((WebControl)base.ViewControl).Height;
			if (container != null)
			{
				container.Controls.Add(table);
				container.Controls.Remove(originalTable);
			}
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (persistMap != null)
			{
				foreach (object obj3 in persistMap.Keys)
				{
					string text = (string)obj3;
					Control control3 = table.FindControl(text);
					if (control3 != null && control3.Visible)
					{
						control3.ID = (string)persistMap[text];
						string text2 = ControlPersister.PersistControl(control3, designerHost);
						LiteralControl literalControl = new LiteralControl(text2);
						control3.Parent.Controls.Add(literalControl);
						control3.Parent.Controls.Remove(control3);
					}
				}
			}
			foreach (string text3 in CreateUserWizardDesigner._persistedControlIDs)
			{
				Control control4 = table.FindControl(text3);
				if (control4 != null)
				{
					if (Array.IndexOf<string>(CreateUserWizardDesigner._persistedIfNotVisibleControlIDs, text3) >= 0)
					{
						control4.Visible = true;
						control4.Parent.Visible = true;
						control4.Parent.Parent.Visible = true;
					}
					if (text3 == "ErrorMessage")
					{
						TableCell tableCell = (TableCell)control4.Parent;
						tableCell.ForeColor = Color.Red;
						tableCell.ApplyStyle(this._createUserWizard.ErrorMessageStyle);
						control4.EnableViewState = false;
					}
					if (control4.Visible)
					{
						string text4 = ControlPersister.PersistControl(control4, designerHost);
						LiteralControl literalControl2 = new LiteralControl(text4);
						control4.Parent.Controls.Add(literalControl2);
						control4.Parent.Controls.Remove(control4);
					}
				}
			}
			StringWriter stringWriter = new StringWriter(CultureInfo.CurrentCulture);
			HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);
			table.RenderControl(htmlTextWriter);
			return stringWriter.ToString();
		}

		// Token: 0x0600266F RID: 9839 RVA: 0x000D0170 File Offset: 0x000CF170
		private string ConvertNavigationTableToHtmlTable(Table table)
		{
			IControlDesignerAccessor createUserWizard = this._createUserWizard;
			createUserWizard.GetDesignModeState();
			StringWriter stringWriter = new StringWriter(CultureInfo.CurrentCulture);
			HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);
			if (table.Width != Unit.Empty)
			{
				htmlTextWriter.AddStyleAttribute(HtmlTextWriterStyle.Width, table.Width.ToString(CultureInfo.CurrentCulture));
			}
			if (table.Height != Unit.Empty)
			{
				htmlTextWriter.AddStyleAttribute(HtmlTextWriterStyle.Height, table.Height.ToString(CultureInfo.CurrentCulture));
			}
			if (table.CellSpacing != 0)
			{
				htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Cellspacing, table.CellSpacing.ToString(CultureInfo.CurrentCulture));
			}
			string text = "0";
			if (table.BorderWidth != Unit.Empty)
			{
				text = table.BorderWidth.ToString(CultureInfo.CurrentCulture);
			}
			htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Border, text);
			htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Table);
			ArrayList arrayList = new ArrayList(table.Rows.Count);
			foreach (object obj in table.Rows)
			{
				TableRow tableRow = (TableRow)obj;
				if (tableRow.Visible)
				{
					ArrayList arrayList2 = new ArrayList(tableRow.Cells.Count);
					foreach (object obj2 in tableRow.Cells)
					{
						TableCell tableCell = (TableCell)obj2;
						if (tableCell.Visible && tableCell.HasControls())
						{
							ArrayList arrayList3 = new ArrayList(tableCell.Controls.Count);
							foreach (object obj3 in tableCell.Controls)
							{
								Control control = (Control)obj3;
								if (control.Visible && (!(control is Literal) || !(control.ID != "ErrorMessage") || ((Literal)control).Text.Length != 0) && (!(control is HyperLink) || ((HyperLink)control).Text.Length != 0) && (!(control is global::System.Web.UI.WebControls.Image) || ((global::System.Web.UI.WebControls.Image)control).ImageUrl.Length != 0))
								{
									arrayList3.Add(control);
								}
							}
							if (arrayList3.Count > 0)
							{
								arrayList2.Add(new CreateUserWizardDesigner.CellControls(tableCell, arrayList3));
							}
						}
					}
					if (arrayList2.Count > 0)
					{
						arrayList.Add(new CreateUserWizardDesigner.RowCells(tableRow, arrayList2));
					}
				}
			}
			foreach (object obj4 in arrayList)
			{
				CreateUserWizardDesigner.RowCells rowCells = (CreateUserWizardDesigner.RowCells)obj4;
				switch (rowCells._row.HorizontalAlign)
				{
				case HorizontalAlign.Center:
					htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Align, "center");
					break;
				case HorizontalAlign.Right:
					htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Align, "right");
					break;
				}
				htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Tr);
				foreach (object obj5 in rowCells._cells)
				{
					CreateUserWizardDesigner.CellControls cellControls = (CreateUserWizardDesigner.CellControls)obj5;
					switch (cellControls._cell.HorizontalAlign)
					{
					case HorizontalAlign.Center:
						htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Align, "center");
						break;
					case HorizontalAlign.Right:
						htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Align, "right");
						break;
					}
					htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Colspan, cellControls._cell.ColumnSpan.ToString(CultureInfo.CurrentCulture));
					StringBuilder stringBuilder = new StringBuilder();
					foreach (object obj6 in cellControls._controls)
					{
						Control control2 = (Control)obj6;
						bool flag = control2.ID == "ErrorMessage";
						if (control2 is Literal && !flag)
						{
							stringBuilder.Append(((Literal)control2).Text);
						}
						else
						{
							if (flag)
							{
								htmlTextWriter.AddStyleAttribute(HtmlTextWriterStyle.Color, "Red");
								control2.EnableViewState = false;
							}
							stringBuilder.Append(ControlPersister.PersistControl(control2));
						}
					}
					htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Td);
					htmlTextWriter.Write(stringBuilder.ToString());
					htmlTextWriter.RenderEndTag();
				}
				htmlTextWriter.RenderEndTag();
			}
			htmlTextWriter.RenderEndTag();
			return stringWriter.ToString();
		}

		// Token: 0x06002670 RID: 9840 RVA: 0x000D06B8 File Offset: 0x000CF6B8
		internal override string GetEditableDesignerRegionContent(IWizardStepEditableRegion region)
		{
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (region.Step == this._createUserWizard.CreateUserStep && ((CreateUserWizardStep)region.Step).ContentTemplate == null && region.Step.Controls[0] is Table)
			{
				Table table = (Table)((Table)region.Step.Controls[0]).Rows[0].Cells[0].Controls[0];
				stringBuilder.Append(this.ConvertTableToHtmlTable(table, ((TemplatedWizardStep)region.Step).ContentTemplateContainer));
				return stringBuilder.ToString();
			}
			if (region.Step == this._createUserWizard.CompleteStep && ((CompleteWizardStep)region.Step).ContentTemplate == null && region.Step.Controls[0] is Table)
			{
				Table table2 = (Table)((Table)region.Step.Controls[0]).Rows[0].Cells[0].Controls[0];
				stringBuilder.Append(this.ConvertTableToHtmlTable(table2, ((TemplatedWizardStep)region.Step).ContentTemplateContainer));
				return stringBuilder.ToString();
			}
			return base.GetEditableDesignerRegionContent(region);
		}

		// Token: 0x06002671 RID: 9841 RVA: 0x000D0824 File Offset: 0x000CF824
		protected override string GetErrorDesignTimeHtml(Exception e)
		{
			return base.CreatePlaceHolderDesignTimeHtml(SR.GetString("Control_ErrorRenderingShort") + "<br />" + e.Message);
		}

		// Token: 0x06002672 RID: 9842 RVA: 0x000D0846 File Offset: 0x000CF846
		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(CreateUserWizard));
			this._createUserWizard = (CreateUserWizard)component;
			base.Initialize(component);
		}

		// Token: 0x06002673 RID: 9843 RVA: 0x000D086C File Offset: 0x000CF86C
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

		// Token: 0x06002674 RID: 9844 RVA: 0x000D08B4 File Offset: 0x000CF8B4
		private void CustomizeCompleteStep()
		{
			IComponent completeStep = this._createUserWizard.CompleteStep;
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["ActiveStepIndex"];
			int num = this._createUserWizard.WizardSteps.IndexOf(this._createUserWizard.CompleteStep);
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.NavigateToStep), num, SR.GetString("CreateUserWizard_NavigateToStep", new object[] { num }), propertyDescriptor);
			PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(completeStep)["ContentTemplate"];
			ControlDesigner.InvokeTransactedChange(base.Component.Site, completeStep, new TransactedChangeCallback(this.CustomizeCompleteStepCallback), null, SR.GetString("CreateUserWizard_CustomizeCompleteStep"), propertyDescriptor2);
		}

		// Token: 0x06002675 RID: 9845 RVA: 0x000D0978 File Offset: 0x000CF978
		private bool CustomizeCompleteStepCallback(object context)
		{
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			CreateUserWizard createUserWizard = (CreateUserWizard)base.ViewControl;
			ITemplate template = createUserWizard.CompleteStep.ContentTemplate;
			if (template == null)
			{
				try
				{
					Hashtable hashtable = new Hashtable(1);
					hashtable.Add("ConvertToTemplate", true);
					((IControlDesignerAccessor)base.ViewControl).SetDesignModeState(hashtable);
					this.ViewControlCreated = false;
					this.GetDesignTimeHtml();
					createUserWizard = (CreateUserWizard)base.ViewControl;
					IControlDesignerAccessor controlDesignerAccessor = createUserWizard;
					controlDesignerAccessor.GetDesignModeState();
					StringBuilder stringBuilder = new StringBuilder();
					TemplatedWizardStep completeStep = createUserWizard.CompleteStep;
					Table table = (Table)((Table)completeStep.Controls[0].Controls[0]).Rows[0].Cells[0].Controls[0];
					if (createUserWizard.ControlStyleCreated)
					{
						Style controlStyle = createUserWizard.ControlStyle;
						table.ForeColor = controlStyle.ForeColor;
						table.BackColor = controlStyle.BackColor;
						table.Font.CopyFrom(controlStyle.Font);
						table.Font.Size = new FontUnit(Unit.Percentage(100.0));
					}
					Style stepStyle = createUserWizard.StepStyle;
					if (!stepStyle.IsEmpty)
					{
						table.ForeColor = stepStyle.ForeColor;
						table.BackColor = stepStyle.BackColor;
						table.Font.CopyFrom(stepStyle.Font);
						table.Font.Size = new FontUnit(Unit.Percentage(100.0));
					}
					stringBuilder.Append(this.ConvertTableToHtmlTable(table, completeStep.ContentTemplateContainer, CreateUserWizardDesigner._completeStepConverter));
					template = ControlParser.ParseTemplate(designerHost, stringBuilder.ToString());
					Hashtable hashtable2 = new Hashtable(1);
					hashtable2.Add("ConvertToTemplate", false);
					((IControlDesignerAccessor)base.ViewControl).SetDesignModeState(hashtable2);
				}
				catch (Exception)
				{
					return false;
				}
			}
			IComponent completeStep2 = this._createUserWizard.CompleteStep;
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(completeStep2)["ContentTemplate"];
			propertyDescriptor.SetValue(completeStep2, template);
			this.UpdateDesignTimeHtml();
			return true;
		}

		// Token: 0x06002676 RID: 9846 RVA: 0x000D0BBC File Offset: 0x000CFBBC
		private void CustomizeCreateUserStep()
		{
			IComponent createUserStep = this._createUserWizard.CreateUserStep;
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["ActiveStepIndex"];
			int num = this._createUserWizard.WizardSteps.IndexOf(this._createUserWizard.CreateUserStep);
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.NavigateToStep), num, SR.GetString("CreateUserWizard_NavigateToStep", new object[] { num }), propertyDescriptor);
			PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(createUserStep)["ContentTemplate"];
			ControlDesigner.InvokeTransactedChange(base.Component.Site, createUserStep, new TransactedChangeCallback(this.CustomizeCreateUserStepCallback), null, SR.GetString("CreateUserWizard_CustomizeCreateUserStep"), propertyDescriptor2);
		}

		// Token: 0x06002677 RID: 9847 RVA: 0x000D0C80 File Offset: 0x000CFC80
		private bool NavigateToStep(object context)
		{
			bool flag;
			try
			{
				int num = (int)context;
				this._createUserWizard.ActiveStepIndex = num;
				flag = true;
			}
			catch (Exception)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06002678 RID: 9848 RVA: 0x000D0CBC File Offset: 0x000CFCBC
		private bool CustomizeCreateUserStepCallback(object context)
		{
			bool flag;
			try
			{
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				CreateUserWizard createUserWizard = (CreateUserWizard)base.ViewControl;
				ITemplate template = createUserWizard.CreateUserStep.ContentTemplate;
				if (template == null)
				{
					this.ViewControlCreated = false;
					Hashtable hashtable = new Hashtable(1);
					hashtable.Add("ConvertToTemplate", true);
					((IControlDesignerAccessor)base.ViewControl).SetDesignModeState(hashtable);
					this.GetDesignTimeHtml();
					createUserWizard = (CreateUserWizard)base.ViewControl;
					IControlDesignerAccessor controlDesignerAccessor = createUserWizard;
					controlDesignerAccessor.GetDesignModeState();
					StringBuilder stringBuilder = new StringBuilder();
					TemplatedWizardStep createUserStep = createUserWizard.CreateUserStep;
					Table table = (Table)((Table)createUserStep.Controls[0].Controls[0]).Rows[0].Cells[0].Controls[0];
					if (createUserWizard.ControlStyleCreated)
					{
						Style controlStyle = createUserWizard.ControlStyle;
						table.ForeColor = controlStyle.ForeColor;
						table.BackColor = controlStyle.BackColor;
						table.Font.CopyFrom(controlStyle.Font);
						table.Font.Size = new FontUnit(Unit.Percentage(100.0));
					}
					Style stepStyle = createUserWizard.StepStyle;
					if (!stepStyle.IsEmpty)
					{
						table.ForeColor = stepStyle.ForeColor;
						table.BackColor = stepStyle.BackColor;
						table.Font.CopyFrom(stepStyle.Font);
						table.Font.Size = new FontUnit(Unit.Percentage(100.0));
					}
					stringBuilder.Append(this.ConvertTableToHtmlTable(table, createUserStep.ContentTemplateContainer));
					template = ControlParser.ParseTemplate(designerHost, stringBuilder.ToString());
					((IControlDesignerAccessor)createUserWizard).SetDesignModeState(new Hashtable(1) { { "ConvertToTemplate", false } });
				}
				IComponent createUserStep2 = this._createUserWizard.CreateUserStep;
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(createUserStep2)["ContentTemplate"];
				propertyDescriptor.SetValue(this._createUserWizard.CreateUserStep, template);
				this.UpdateDesignTimeHtml();
				flag = true;
			}
			catch (Exception)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06002679 RID: 9849 RVA: 0x000D0F00 File Offset: 0x000CFF00
		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			TemplatedWizardStep createUserStep = this._createUserWizard.CreateUserStep;
			bool flag = createUserStep != null && createUserStep.ContentTemplate != null;
			if (flag)
			{
				foreach (string text in CreateUserWizardDesigner._defaultCreateStepProperties)
				{
					PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties[text];
					if (propertyDescriptor != null)
					{
						properties[text] = TypeDescriptor.CreateProperty(propertyDescriptor.ComponentType, propertyDescriptor, new Attribute[] { BrowsableAttribute.No });
					}
				}
			}
			TemplatedWizardStep completeStep = this._createUserWizard.CompleteStep;
			bool flag2 = completeStep != null && completeStep.ContentTemplate != null;
			if (flag2)
			{
				foreach (string text2 in CreateUserWizardDesigner._defaultCompleteStepProperties)
				{
					PropertyDescriptor propertyDescriptor2 = (PropertyDescriptor)properties[text2];
					if (propertyDescriptor2 != null)
					{
						properties[text2] = TypeDescriptor.CreateProperty(propertyDescriptor2.ComponentType, propertyDescriptor2, new Attribute[] { BrowsableAttribute.No });
					}
				}
			}
			if (createUserStep != null && createUserStep.CustomNavigationTemplate != null)
			{
				foreach (string text3 in CreateUserWizardDesigner._defaultCreateUserNavProperties)
				{
					PropertyDescriptor propertyDescriptor3 = (PropertyDescriptor)properties[text3];
					if (propertyDescriptor3 != null)
					{
						properties[text3] = TypeDescriptor.CreateProperty(propertyDescriptor3.ComponentType, propertyDescriptor3, new Attribute[] { BrowsableAttribute.No });
					}
				}
			}
			if (flag2 && flag)
			{
				PropertyDescriptor propertyDescriptor4 = (PropertyDescriptor)properties["TitleTextStyle"];
				if (propertyDescriptor4 != null)
				{
					properties["TitleTextStyle"] = TypeDescriptor.CreateProperty(propertyDescriptor4.ComponentType, propertyDescriptor4, new Attribute[] { BrowsableAttribute.No });
				}
			}
		}

		// Token: 0x0600267A RID: 9850 RVA: 0x000D10C4 File Offset: 0x000D00C4
		private bool ResetCallback(object context)
		{
			bool flag;
			try
			{
				IComponent component = (IComponent)context;
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(component)["ContentTemplate"];
				propertyDescriptor.SetValue(component, null);
				flag = true;
			}
			catch (Exception)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600267B RID: 9851 RVA: 0x000D110C File Offset: 0x000D010C
		private void ResetCompleteStep()
		{
			this.UpdateDesignTimeHtml();
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["WizardSteps"];
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ResetCallback), this._createUserWizard.CompleteStep, SR.GetString("CreateUserWizard_ResetCompleteStepVerb"), propertyDescriptor);
		}

		// Token: 0x0600267C RID: 9852 RVA: 0x000D1164 File Offset: 0x000D0164
		private void ResetCreateUserStep()
		{
			this.UpdateDesignTimeHtml();
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["WizardSteps"];
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ResetCallback), this._createUserWizard.CreateUserStep, SR.GetString("CreateUserWizard_ResetCreateUserStepVerb"), propertyDescriptor);
		}

		// Token: 0x04001A5A RID: 6746
		private const string _userNameID = "UserName";

		// Token: 0x04001A5B RID: 6747
		private const string _passwordID = "Password";

		// Token: 0x04001A5C RID: 6748
		private const string _confirmPasswordID = "ConfirmPassword";

		// Token: 0x04001A5D RID: 6749
		private const string _unknownErrorMessageID = "ErrorMessage";

		// Token: 0x04001A5E RID: 6750
		private const string _emailID = "Email";

		// Token: 0x04001A5F RID: 6751
		private const string _questionID = "Question";

		// Token: 0x04001A60 RID: 6752
		private const string _answerID = "Answer";

		// Token: 0x04001A61 RID: 6753
		private const string _userNameRequiredID = "UserNameRequired";

		// Token: 0x04001A62 RID: 6754
		private const string _passwordRequiredID = "PasswordRequired";

		// Token: 0x04001A63 RID: 6755
		private const string _confirmPasswordRequiredID = "ConfirmPasswordRequired";

		// Token: 0x04001A64 RID: 6756
		private const string _passwordRegExpID = "PasswordRegExp";

		// Token: 0x04001A65 RID: 6757
		private const string _emailRequiredID = "EmailRequired";

		// Token: 0x04001A66 RID: 6758
		private const string _emailRegExpID = "EmailRegExp";

		// Token: 0x04001A67 RID: 6759
		private const string _questionRequiredID = "QuestionRequired";

		// Token: 0x04001A68 RID: 6760
		private const string _answerRequiredID = "AnswerRequired";

		// Token: 0x04001A69 RID: 6761
		private const string _passwordCompareID = "PasswordCompare";

		// Token: 0x04001A6A RID: 6762
		private const string _cancelButtonID = "CancelButton";

		// Token: 0x04001A6B RID: 6763
		private const string _cancelButtonButtonID = "CancelButtonButton";

		// Token: 0x04001A6C RID: 6764
		private const string _cancelButtonImageButtonID = "CancelButtonImageButton";

		// Token: 0x04001A6D RID: 6765
		private const string _cancelButtonLinkButtonID = "CancelButtonLinkButton";

		// Token: 0x04001A6E RID: 6766
		private const string _continueButtonID = "ContinueButton";

		// Token: 0x04001A6F RID: 6767
		private const string _continueButtonButtonID = "ContinueButtonButton";

		// Token: 0x04001A70 RID: 6768
		private const string _continueButtonImageButtonID = "ContinueButtonImageButton";

		// Token: 0x04001A71 RID: 6769
		private const string _continueButtonLinkButtonID = "ContinueButtonLinkButton";

		// Token: 0x04001A72 RID: 6770
		private const string _helpLinkID = "HelpLink";

		// Token: 0x04001A73 RID: 6771
		private const string _editProfileLinkID = "EditProfileLink";

		// Token: 0x04001A74 RID: 6772
		private const string _createUserButtonID = "StepNextButton";

		// Token: 0x04001A75 RID: 6773
		private const string _createUserButtonButtonID = "StepNextButtonButton";

		// Token: 0x04001A76 RID: 6774
		private const string _createUserButtonImageButtonID = "StepNextButtonImageButton";

		// Token: 0x04001A77 RID: 6775
		private const string _createUserButtonLinkButtonID = "StepNextButtonLinkButton";

		// Token: 0x04001A78 RID: 6776
		private const string _createUserNavigationTemplateName = "CreateUserNavigationTemplate";

		// Token: 0x04001A79 RID: 6777
		private const string _previousButtonID = "StepNextButton";

		// Token: 0x04001A7A RID: 6778
		private const string _previousButtonButtonID = "StepPreviousButton";

		// Token: 0x04001A7B RID: 6779
		private const string _previousButtonImageButtonID = "StepPreviousButtonImageButton";

		// Token: 0x04001A7C RID: 6780
		private const string _previousButtonLinkButtonID = "StepPreviousButtonLinkButton";

		// Token: 0x04001A7D RID: 6781
		private CreateUserWizard _createUserWizard;

		// Token: 0x04001A7E RID: 6782
		private static DesignerAutoFormatCollection _autoFormats;

		// Token: 0x04001A7F RID: 6783
		private static readonly Hashtable _persistedIDConverter = new Hashtable();

		// Token: 0x04001A80 RID: 6784
		private static readonly Hashtable _completeStepConverter;

		// Token: 0x04001A81 RID: 6785
		private static readonly string[] _persistedControlIDs = new string[]
		{
			"UserName", "UserNameRequired", "Password", "PasswordRequired", "ConfirmPassword", "Email", "Question", "Answer", "ConfirmPasswordRequired", "PasswordRegExp",
			"EmailRegExp", "EmailRequired", "QuestionRequired", "AnswerRequired", "PasswordCompare", "CancelButton", "ContinueButton", "StepNextButton", "ErrorMessage", "HelpLink",
			"EditProfileLink"
		};

		// Token: 0x04001A82 RID: 6786
		private static readonly string[] _persistedIfNotVisibleControlIDs = new string[] { "ErrorMessage" };

		// Token: 0x04001A83 RID: 6787
		private static readonly string[] _defaultCreateStepProperties = new string[]
		{
			"AnswerLabelText", "ConfirmPasswordLabelText", "ConfirmPasswordCompareErrorMessage", "ConfirmPasswordRequiredErrorMessage", "EmailLabelText", "ErrorMessageStyle", "HelpPageIconUrl", "HelpPageText", "HelpPageUrl", "HyperLinkStyle",
			"InstructionText", "InstructionTextStyle", "LabelStyle", "PasswordHintText", "PasswordHintStyle", "PasswordLabelText", "PasswordRequiredErrorMessage", "QuestionLabelText", "TextBoxStyle", "UserNameLabelText",
			"UserNameRequiredErrorMessage", "AnswerRequiredErrorMessage", "EmailRegularExpression", "EmailRegularExpressionErrorMessage", "EmailRequiredErrorMessage", "PasswordRegularExpression", "PasswordRegularExpressionErrorMessage", "QuestionRequiredErrorMessage", "ValidatorTextStyle"
		};

		// Token: 0x04001A84 RID: 6788
		private static readonly string[] _defaultCreateUserNavProperties = new string[] { "CancelButtonImageUrl", "CancelButtonType", "CancelButtonStyle", "CancelButtonText", "CreateUserButtonImageUrl", "CreateUserButtonType", "CreateUserButtonStyle", "CreateUserButtonText" };

		// Token: 0x04001A85 RID: 6789
		private static readonly string[] _defaultCompleteStepProperties = new string[] { "CompleteSuccessText", "CompleteSuccessTextStyle", "ContinueButtonStyle", "ContinueButtonText", "ContinueButtonType", "ContinueButtonImageUrl", "EditProfileText", "EditProfileIconUrl", "EditProfileUrl" };

		// Token: 0x04001A86 RID: 6790
		[CompilerGenerated]
		private static ControlDesigner.CreateAutoFormatDelegate <>9__CachedAnonymousMethodDelegate3;

		// Token: 0x02000418 RID: 1048
		private class RowCells
		{
			// Token: 0x0600267F RID: 9855 RVA: 0x000D11C2 File Offset: 0x000D01C2
			internal RowCells(TableRow row, ArrayList cells)
			{
				this._row = row;
				this._cells = cells;
			}

			// Token: 0x04001A87 RID: 6791
			internal TableRow _row;

			// Token: 0x04001A88 RID: 6792
			internal ArrayList _cells;
		}

		// Token: 0x02000419 RID: 1049
		private class CellControls
		{
			// Token: 0x06002680 RID: 9856 RVA: 0x000D11D8 File Offset: 0x000D01D8
			internal CellControls(TableCell cell, ArrayList controls)
			{
				this._cell = cell;
				this._controls = controls;
			}

			// Token: 0x04001A89 RID: 6793
			internal TableCell _cell;

			// Token: 0x04001A8A RID: 6794
			internal ArrayList _controls;
		}

		// Token: 0x0200041A RID: 1050
		private class CreateUserWizardDesignerActionList : DesignerActionList
		{
			// Token: 0x06002681 RID: 9857 RVA: 0x000D11EE File Offset: 0x000D01EE
			public CreateUserWizardDesignerActionList(CreateUserWizardDesigner parent)
				: base(parent.Component)
			{
				this._parent = parent;
			}

			// Token: 0x17000723 RID: 1827
			// (get) Token: 0x06002682 RID: 9858 RVA: 0x000D1203 File Offset: 0x000D0203
			// (set) Token: 0x06002683 RID: 9859 RVA: 0x000D1206 File Offset: 0x000D0206
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

			// Token: 0x06002684 RID: 9860 RVA: 0x000D1208 File Offset: 0x000D0208
			public void CustomizeCreateUserStep()
			{
				Cursor cursor = Cursor.Current;
				try
				{
					Cursor.Current = Cursors.WaitCursor;
					this._parent.CustomizeCreateUserStep();
				}
				finally
				{
					Cursor.Current = cursor;
				}
			}

			// Token: 0x06002685 RID: 9861 RVA: 0x000D124C File Offset: 0x000D024C
			public void CustomizeCompleteStep()
			{
				Cursor cursor = Cursor.Current;
				try
				{
					Cursor.Current = Cursors.WaitCursor;
					this._parent.CustomizeCompleteStep();
				}
				finally
				{
					Cursor.Current = cursor;
				}
			}

			// Token: 0x06002686 RID: 9862 RVA: 0x000D1290 File Offset: 0x000D0290
			public void LaunchWebAdmin()
			{
				this._parent.LaunchWebAdmin();
			}

			// Token: 0x06002687 RID: 9863 RVA: 0x000D129D File Offset: 0x000D029D
			public void ResetCreateUserStep()
			{
				this._parent.ResetCreateUserStep();
			}

			// Token: 0x06002688 RID: 9864 RVA: 0x000D12AA File Offset: 0x000D02AA
			public void ResetCompleteStep()
			{
				this._parent.ResetCompleteStep();
			}

			// Token: 0x06002689 RID: 9865 RVA: 0x000D12B8 File Offset: 0x000D02B8
			public override DesignerActionItemCollection GetSortedActionItems()
			{
				if (this._parent.InTemplateMode)
				{
					return new DesignerActionItemCollection();
				}
				DesignerActionItemCollection designerActionItemCollection = new DesignerActionItemCollection();
				if (this._parent._createUserWizard.CreateUserStep.ContentTemplate == null)
				{
					designerActionItemCollection.Add(new DesignerActionMethodItem(this, "CustomizeCreateUserStep", SR.GetString("CreateUserWizard_CustomizeCreateUserStep"), string.Empty, SR.GetString("CreateUserWizard_CustomizeCreateUserStepDescription"), true));
				}
				else
				{
					designerActionItemCollection.Add(new DesignerActionMethodItem(this, "ResetCreateUserStep", SR.GetString("CreateUserWizard_ResetCreateUserStepVerb"), string.Empty, SR.GetString("CreateUserWizard_ResetCreateUserStepVerbDescription"), true));
				}
				if (this._parent._createUserWizard.CompleteStep.ContentTemplate == null)
				{
					designerActionItemCollection.Add(new DesignerActionMethodItem(this, "CustomizeCompleteStep", SR.GetString("CreateUserWizard_CustomizeCompleteStep"), string.Empty, SR.GetString("CreateUserWizard_CustomizeCompleteStepDescription"), true));
				}
				else
				{
					designerActionItemCollection.Add(new DesignerActionMethodItem(this, "ResetCompleteStep", SR.GetString("CreateUserWizard_ResetCompleteStepVerb"), string.Empty, SR.GetString("CreateUserWizard_ResetCompleteStepVerbDescription"), true));
				}
				designerActionItemCollection.Add(new DesignerActionMethodItem(this, "LaunchWebAdmin", SR.GetString("Login_LaunchWebAdmin"), string.Empty, SR.GetString("Login_LaunchWebAdminDescription"), true));
				return designerActionItemCollection;
			}

			// Token: 0x04001A8B RID: 6795
			private CreateUserWizardDesigner _parent;
		}
	}
}
