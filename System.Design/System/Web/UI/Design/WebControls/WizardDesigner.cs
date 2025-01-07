using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Design;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class WizardDesigner : CompositeControlDesigner
	{
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection designerActionListCollection = new DesignerActionListCollection();
				designerActionListCollection.AddRange(base.ActionLists);
				designerActionListCollection.Add(new WizardDesigner.WizardDesignerActionList(this));
				return designerActionListCollection;
			}
		}

		internal WizardStepBase ActiveStep
		{
			get
			{
				if (this.ActiveStepIndex != -1)
				{
					return this._wizard.WizardSteps[this.ActiveStepIndex];
				}
				return null;
			}
		}

		internal int ActiveStepIndex
		{
			get
			{
				int activeStepIndex = this._wizard.ActiveStepIndex;
				if (activeStepIndex == -1 && this._wizard.WizardSteps.Count > 0)
				{
					return 0;
				}
				return activeStepIndex;
			}
		}

		public override DesignerAutoFormatCollection AutoFormats
		{
			get
			{
				if (this._autoFormats == null)
				{
					this._autoFormats = ControlDesigner.CreateAutoFormats("<Schemes>\r\n        <xsd:schema id=\"Schemes\" xmlns=\"\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\">\r\n          <xsd:element name=\"Scheme\">\r\n            <xsd:complexType>\r\n              <xsd:all>\r\n                <xsd:element name=\"SchemeName\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"FontName\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"FontSize\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"BackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"BorderColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"BorderWidth\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"BorderStyle\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"NavigationButtonStyleBorderWidth\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"NavigationButtonStyleFontName\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"NavigationButtonStyleFontSize\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"NavigationButtonStyleBorderStyle\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"NavigationButtonStyleBorderColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"NavigationButtonStyleForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"NavigationButtonStyleBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"StepStyleBorderWidth\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"StepStyleBorderStyle\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"StepStyleBorderColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"StepStyleForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"StepStyleBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"StepStyleFontSize\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"SideBarButtonStyleFontUnderline\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"SideBarButtonStyleFontName\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"SideBarButtonStyleForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"SideBarButtonStyleBorderWidth\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"SideBarButtonStyleBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"HeaderStyleForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"HeaderStyleBorderColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"HeaderStyleBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"HeaderStyleFontSize\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"HeaderStyleFontBold\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"HeaderStyleBorderWidth\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"HeaderStyleHorizontalAlign\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"HeaderStyleBorderStyle\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"SideBarStyleBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"SideBarStyleVerticalAlign\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"SideBarStyleFontSize\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"SideBarStyleFontUnderline\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"SideBarStyleFontStrikeout\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"SideBarStyleBorderWidth\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n              </xsd:all>\r\n            </xsd:complexType>\r\n          </xsd:element>\r\n          <xsd:element name=\"Schemes\" msdata:IsDataSet=\"true\">\r\n            <xsd:complexType>\r\n              <xsd:choice maxOccurs=\"unbounded\">\r\n                <xsd:element ref=\"Scheme\"/>\r\n              </xsd:choice>\r\n            </xsd:complexType>\r\n          </xsd:element>\r\n        </xsd:schema>\r\n        <Scheme>\r\n          <SchemeName>WizardAFmt_Scheme_Default</SchemeName>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>WizardAFmt_Scheme_Colorful</SchemeName>\r\n          <FontName>Verdana</FontName>\r\n          <FontSize>0.8em</FontSize>\r\n          <BackColor>#FFFBD6</BackColor>\r\n          <BorderColor>#FFDFAD</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <NavigationButtonStyleBorderWidth>1px</NavigationButtonStyleBorderWidth>\r\n          <NavigationButtonStyleFontName>Verdana</NavigationButtonStyleFontName>\r\n          <NavigationButtonStyleFontSize>0.8em</NavigationButtonStyleFontSize>\r\n          <NavigationButtonStyleBorderStyle>4</NavigationButtonStyleBorderStyle>\r\n          <NavigationButtonStyleBorderColor>#CC9966</NavigationButtonStyleBorderColor>\r\n          <NavigationButtonStyleForeColor>#990000</NavigationButtonStyleForeColor>\r\n          <NavigationButtonStyleBackColor>White</NavigationButtonStyleBackColor>\r\n          <SideBarButtonStyleFontUnderline>False</SideBarButtonStyleFontUnderline>\r\n          <SideBarButtonStyleForeColor>White</SideBarButtonStyleForeColor>\r\n          <HeaderStyleForeColor>#333333</HeaderStyleForeColor>\r\n          <HeaderStyleBorderColor>#FFFBD6</HeaderStyleBorderColor>\r\n          <HeaderStyleBackColor>#FFCC66</HeaderStyleBackColor>\r\n          <HeaderStyleFontSize>0.9em</HeaderStyleFontSize>\r\n          <HeaderStyleFontBold>True</HeaderStyleFontBold>\r\n          <HeaderStyleBorderWidth>2px</HeaderStyleBorderWidth>\r\n          <HeaderStyleHorizontalAlign>2</HeaderStyleHorizontalAlign>\r\n          <HeaderStyleBorderStyle>4</HeaderStyleBorderStyle>\r\n          <SideBarStyleBackColor>#990000</SideBarStyleBackColor>\r\n          <SideBarStyleVerticalAlign>1</SideBarStyleVerticalAlign>\r\n          <SideBarStyleFontSize>0.9em</SideBarStyleFontSize>\r\n          <SideBarStyleFontUnderline>False</SideBarStyleFontUnderline>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>WizardAFmt_Scheme_Professional</SchemeName>\r\n          <FontName>Verdana</FontName>\r\n          <FontSize>0.8em</FontSize>\r\n          <BackColor>#F7F6F3</BackColor>\r\n          <BorderColor>#CCCCCC</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>4</BorderStyle>\r\n          <StepStyleForeColor>#5D7B9D</StepStyleForeColor>\r\n          <StepStyleBorderWidth>0px</StepStyleBorderWidth>\r\n          <NavigationButtonStyleBorderWidth>1px</NavigationButtonStyleBorderWidth>\r\n          <NavigationButtonStyleFontName>Verdana</NavigationButtonStyleFontName>\r\n          <NavigationButtonStyleFontSize>0.8em</NavigationButtonStyleFontSize>\r\n          <NavigationButtonStyleBorderStyle>4</NavigationButtonStyleBorderStyle>\r\n          <NavigationButtonStyleBorderColor>#CCCCCC</NavigationButtonStyleBorderColor>\r\n          <NavigationButtonStyleForeColor>#284775</NavigationButtonStyleForeColor>\r\n          <NavigationButtonStyleBackColor>#FFFBFF</NavigationButtonStyleBackColor>\r\n          <SideBarButtonStyleFontUnderline>False</SideBarButtonStyleFontUnderline>\r\n          <SideBarButtonStyleFontName>Verdana</SideBarButtonStyleFontName>\r\n          <SideBarButtonStyleForeColor>White</SideBarButtonStyleForeColor>\r\n          <SideBarButtonStyleBorderWidth>0px</SideBarButtonStyleBorderWidth>\r\n          <HeaderStyleForeColor>White</HeaderStyleForeColor>\r\n          <HeaderStyleBackColor>#5D7B9D</HeaderStyleBackColor>\r\n          <HeaderStyleFontSize>0.9em</HeaderStyleFontSize>\r\n          <HeaderStyleFontBold>True</HeaderStyleFontBold>\r\n          <HeaderStyleHorizontalAlign>1</HeaderStyleHorizontalAlign>\r\n          <HeaderStyleBorderStyle>4</HeaderStyleBorderStyle>\r\n          <SideBarStyleBackColor>#7C6F57</SideBarStyleBackColor>\r\n          <SideBarStyleVerticalAlign>1</SideBarStyleVerticalAlign>\r\n          <SideBarStyleFontSize>0.9em</SideBarStyleFontSize>\r\n          <SideBarStyleBorderWidth>0px</SideBarStyleBorderWidth>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>WizardAFmt_Scheme_Classic</SchemeName>\r\n          <FontName>Verdana</FontName>\r\n          <FontSize>0.8em</FontSize>\r\n          <BackColor>#EFF3FB</BackColor>\r\n          <BorderColor>#B5C7DE</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <StepStyleForeColor>#333333</StepStyleForeColor>\r\n          <StepStyleFontSize>0.8em</StepStyleFontSize>\r\n          <NavigationButtonStyleBorderWidth>1px</NavigationButtonStyleBorderWidth>\r\n          <NavigationButtonStyleFontName>Verdana</NavigationButtonStyleFontName>\r\n          <NavigationButtonStyleFontSize>0.8em</NavigationButtonStyleFontSize>\r\n          <NavigationButtonStyleBorderStyle>4</NavigationButtonStyleBorderStyle>\r\n          <NavigationButtonStyleBorderColor>#507CD1</NavigationButtonStyleBorderColor>\r\n          <NavigationButtonStyleForeColor>#284E98</NavigationButtonStyleForeColor>\r\n          <NavigationButtonStyleBackColor>White</NavigationButtonStyleBackColor>\r\n          <SideBarButtonStyleFontUnderline>False</SideBarButtonStyleFontUnderline>\r\n          <SideBarButtonStyleFontName>Verdana</SideBarButtonStyleFontName>\r\n          <SideBarButtonStyleForeColor>White</SideBarButtonStyleForeColor>\r\n          <SideBarButtonStyleBackColor>#507CD1</SideBarButtonStyleBackColor>\r\n          <HeaderStyleForeColor>White</HeaderStyleForeColor>\r\n          <HeaderStyleBorderColor>#EFF3FB</HeaderStyleBorderColor>\r\n          <HeaderStyleBackColor>#284E98</HeaderStyleBackColor>\r\n          <HeaderStyleFontSize>0.9em</HeaderStyleFontSize>\r\n          <HeaderStyleFontBold>True</HeaderStyleFontBold>\r\n          <HeaderStyleBorderWidth>2px</HeaderStyleBorderWidth>\r\n          <HeaderStyleHorizontalAlign>2</HeaderStyleHorizontalAlign>\r\n          <HeaderStyleBorderStyle>4</HeaderStyleBorderStyle>\r\n          <SideBarStyleBackColor>#507CD1</SideBarStyleBackColor>\r\n          <SideBarStyleVerticalAlign>1</SideBarStyleVerticalAlign>\r\n          <SideBarStyleFontSize>0.9em</SideBarStyleFontSize>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>WizardAFmt_Scheme_Simple</SchemeName>\r\n          <FontName>Verdana</FontName>\r\n          <FontSize>0.8em</FontSize>\r\n          <BackColor>#E6E2D8</BackColor>\r\n          <BorderColor>#999999</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>4</BorderStyle>\r\n          <StepStyleBorderStyle>4</StepStyleBorderStyle>\r\n          <StepStyleBorderColor>#E6E2D8</StepStyleBorderColor>\r\n          <StepStyleBackColor>#F7F6F3</StepStyleBackColor>\r\n          <StepStyleBorderWidth>2px</StepStyleBorderWidth>\r\n          <NavigationButtonStyleBorderWidth>1px</NavigationButtonStyleBorderWidth>\r\n          <NavigationButtonStyleFontName>Verdana</NavigationButtonStyleFontName>\r\n          <NavigationButtonStyleFontSize>0.8em</NavigationButtonStyleFontSize>\r\n          <NavigationButtonStyleBorderStyle>4</NavigationButtonStyleBorderStyle>\r\n          <NavigationButtonStyleBorderColor>#C5BBAF</NavigationButtonStyleBorderColor>\r\n          <NavigationButtonStyleForeColor>#1C5E55</NavigationButtonStyleForeColor>\r\n          <NavigationButtonStyleBackColor>White</NavigationButtonStyleBackColor>\r\n          <SideBarButtonStyleFontUnderline>False</SideBarButtonStyleFontUnderline>\r\n          <SideBarButtonStyleForeColor>White</SideBarButtonStyleForeColor>\r\n          <HeaderStyleForeColor>White</HeaderStyleForeColor>\r\n          <HeaderStyleBackColor>#666666</HeaderStyleBackColor>\r\n          <HeaderStyleBorderColor>#E6E2D8</HeaderStyleBorderColor>\r\n          <HeaderStyleFontSize>0.9em</HeaderStyleFontSize>\r\n          <HeaderStyleFontBold>True</HeaderStyleFontBold>\r\n          <HeaderStyleHorizontalAlign>2</HeaderStyleHorizontalAlign>\r\n          <HeaderStyleBorderStyle>4</HeaderStyleBorderStyle>\r\n          <HeaderStyleBorderWidth>2px</HeaderStyleBorderWidth>\r\n          <SideBarStyleBackColor>#1C5E55</SideBarStyleBackColor>\r\n          <SideBarStyleVerticalAlign>1</SideBarStyleVerticalAlign>\r\n          <SideBarStyleFontSize>0.9em</SideBarStyleFontSize>\r\n        </Scheme>\r\n      </Schemes>", (DataRow schemeData) => new WizardAutoFormat(schemeData));
				}
				return this._autoFormats;
			}
		}

		protected bool DisplaySideBar
		{
			get
			{
				return ((Wizard)base.Component).DisplaySideBar;
			}
			set
			{
				TypeDescriptor.Refresh(base.Component);
				((Wizard)base.Component).DisplaySideBar = value;
				TypeDescriptor.Refresh(base.Component);
			}
		}

		internal bool SupportsDesignerRegions
		{
			get
			{
				if (this._supportsDesignerRegionQueried)
				{
					return this._supportsDesignerRegion;
				}
				if (base.View != null)
				{
					this._supportsDesignerRegion = base.View.SupportsRegions;
				}
				this._supportsDesignerRegionQueried = true;
				return this._supportsDesignerRegion;
			}
		}

		internal virtual bool InRegionEditingMode(Wizard viewControl)
		{
			if (!this.SupportsDesignerRegions)
			{
				return true;
			}
			TemplatedWizardStep templatedWizardStep = this.ActiveStep as TemplatedWizardStep;
			if (templatedWizardStep != null && templatedWizardStep.ContentTemplate == null)
			{
				TemplatedWizardStep templatedWizardStep2 = viewControl.WizardSteps[this.ActiveStepIndex] as TemplatedWizardStep;
				if (templatedWizardStep2 != null && templatedWizardStep2.ContentTemplate != null)
				{
					return true;
				}
			}
			return false;
		}

		public override TemplateGroupCollection TemplateGroups
		{
			get
			{
				TemplateGroupCollection templateGroups = base.TemplateGroups;
				for (int i = 0; i < WizardDesigner._controlTemplateNames.Length; i++)
				{
					string text = WizardDesigner._controlTemplateNames[i];
					TemplateGroup templateGroup = new TemplateGroup(text);
					templateGroup.AddTemplateDefinition(new TemplateDefinition(this, text, this._wizard, text, this.TemplateStyleArray[i]));
					templateGroups.Add(templateGroup);
				}
				foreach (object obj in this._wizard.WizardSteps)
				{
					WizardStepBase wizardStepBase = (WizardStepBase)obj;
					string regionName = this.GetRegionName(wizardStepBase);
					TemplateGroup templateGroup2 = new TemplateGroup(regionName);
					if (wizardStepBase is TemplatedWizardStep)
					{
						for (int j = 0; j < WizardDesigner._stepTemplateNames.Length; j++)
						{
							templateGroup2.AddTemplateDefinition(new TemplateDefinition(this, WizardDesigner._stepTemplateNames[j], wizardStepBase, WizardDesigner._stepTemplateNames[j], this.StepTemplateStyleArray[j]));
						}
					}
					else if (!this.SupportsDesignerRegions)
					{
						templateGroup2.AddTemplateDefinition(new WizardStepBaseTemplateDefinition(this, wizardStepBase, regionName, this.StepTemplateStyleArray[0]));
					}
					if (!templateGroup2.IsEmpty)
					{
						templateGroups.Add(templateGroup2);
					}
				}
				return templateGroups;
			}
		}

		internal Style[] TemplateStyleArray
		{
			get
			{
				Style style = new Style();
				Wizard wizard = (Wizard)base.ViewControl;
				style.CopyFrom(wizard.ControlStyle);
				style.CopyFrom(wizard.HeaderStyle);
				Style style2 = new Style();
				style2.CopyFrom(wizard.ControlStyle);
				style2.CopyFrom(wizard.SideBarStyle);
				Style style3 = new Style();
				style3.CopyFrom(wizard.ControlStyle);
				style3.CopyFrom(wizard.NavigationStyle);
				return new Style[] { style, style2, style3, style3, style3, style3 };
			}
		}

		private Style[] StepTemplateStyleArray
		{
			get
			{
				Style style = new Style();
				Wizard wizard = (Wizard)base.ViewControl;
				style.CopyFrom(wizard.ControlStyle);
				style.CopyFrom(wizard.StepStyle);
				Style style2 = new Style();
				style2.CopyFrom(wizard.ControlStyle);
				style2.CopyFrom(wizard.NavigationStyle);
				return new Style[] { style, style2 };
			}
		}

		protected override bool UsePreviewControl
		{
			get
			{
				return true;
			}
		}

		protected virtual void AddDesignerRegions(DesignerRegionCollection regions)
		{
			if (!this.SupportsDesignerRegions)
			{
				return;
			}
			foreach (object obj in this._wizard.WizardSteps)
			{
				WizardStepBase wizardStepBase = (WizardStepBase)obj;
				if (wizardStepBase is TemplatedWizardStep)
				{
					TemplateDefinition templateDefinition = new TemplateDefinition(this, "ContentTemplate", this._wizard, "ContentTemplate", this.TemplateStyleArray[5]);
					regions.Add(new WizardStepTemplatedEditableRegion(templateDefinition, wizardStepBase)
					{
						Description = SR.GetString("ContainerControlDesigner_RegionWatermark")
					});
				}
				else
				{
					regions.Add(new WizardStepEditableRegion(this, wizardStepBase)
					{
						Description = SR.GetString("ContainerControlDesigner_RegionWatermark")
					});
				}
			}
			foreach (object obj2 in this._wizard.WizardSteps)
			{
				WizardStepBase wizardStepBase2 = (WizardStepBase)obj2;
				regions.Add(new WizardSelectableRegion(this, "Move to " + this.GetRegionName(wizardStepBase2), wizardStepBase2));
			}
		}

		private ITemplate GetTemplateFromDesignModeState(string[] keys)
		{
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			IControlDesignerAccessor wizard = this._wizard;
			IDictionary designModeState = wizard.GetDesignModeState();
			this.ResetInternalControls(designModeState);
			string text = string.Empty;
			foreach (string text2 in keys)
			{
				Control control = designModeState[text2] as Control;
				if (control != null && control.Visible)
				{
					control.ID = text2;
					text += ControlPersister.PersistControl(control, designerHost);
				}
			}
			return ControlParser.ParseTemplate(designerHost, text);
		}

		protected void ConvertToTemplate(string description, IComponent component, string templateName, string[] keys)
		{
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ConvertToTemplateCallBack), new Triplet(component, templateName, keys), description);
			this.UpdateDesignTimeHtml();
		}

		private bool ConvertToTemplateCallBack(object context)
		{
			Triplet triplet = (Triplet)context;
			IComponent component = (IComponent)triplet.First;
			string text = (string)triplet.Second;
			string[] array = (string[])triplet.Third;
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(component)[text];
			propertyDescriptor.SetValue(component, this.GetTemplateFromDesignModeState(array));
			return true;
		}

		protected virtual void ConvertToCustomNavigationTemplate()
		{
			try
			{
				ITemplate template = null;
				string @string = SR.GetString("Wizard_ConvertToCustomNavigationTemplate");
				TemplatedWizardStep templatedWizardStep = this.ActiveStep as TemplatedWizardStep;
				if (templatedWizardStep != null)
				{
					TemplatedWizardStep templatedWizardStep2 = ((Wizard)base.ViewControl).ActiveStep as TemplatedWizardStep;
					if (templatedWizardStep2 != null && templatedWizardStep2.CustomNavigationTemplate != null)
					{
						template = templatedWizardStep2.CustomNavigationTemplate;
					}
					else
					{
						switch (this._wizard.GetStepType(templatedWizardStep, this.ActiveStepIndex))
						{
						case WizardStepType.Finish:
							template = this.GetTemplateFromDesignModeState(WizardDesigner._finishButtonIDs);
							break;
						case WizardStepType.Start:
							template = this.GetTemplateFromDesignModeState(WizardDesigner._startButtonIDs);
							break;
						case WizardStepType.Step:
							template = this.GetTemplateFromDesignModeState(WizardDesigner._stepButtonIDs);
							break;
						}
					}
					ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ConvertToCustomNavigationTemplateCallBack), template, @string);
				}
			}
			catch (Exception)
			{
			}
		}

		internal bool ConvertToCustomNavigationTemplateCallBack(object context)
		{
			ITemplate template = (ITemplate)context;
			TemplatedWizardStep templatedWizardStep = this.ActiveStep as TemplatedWizardStep;
			templatedWizardStep.CustomNavigationTemplate = template;
			return true;
		}

		private void ConvertToStartNavigationTemplate()
		{
			this.ConvertToTemplate(SR.GetString("Wizard_ConvertToStartNavigationTemplate"), base.Component, "StartNavigationTemplate", WizardDesigner._startButtonIDs);
		}

		private void ConvertToStepNavigationTemplate()
		{
			this.ConvertToTemplate(SR.GetString("Wizard_ConvertToStepNavigationTemplate"), base.Component, "StepNavigationTemplate", WizardDesigner._stepButtonIDs);
		}

		private void ConvertToFinishNavigationTemplate()
		{
			this.ConvertToTemplate(SR.GetString("Wizard_ConvertToFinishNavigationTemplate"), base.Component, "FinishNavigationTemplate", WizardDesigner._finishButtonIDs);
		}

		private void ConvertToSideBarTemplate()
		{
			this.ConvertToTemplate(SR.GetString("Wizard_ConvertToSideBarTemplate"), base.Component, "SideBarTemplate", new string[] { "SideBarList" });
		}

		protected override void CreateChildControls()
		{
			base.CreateChildControls();
			Wizard wizard = (Wizard)base.ViewControl;
			if (wizard.ActiveStepIndex == -1 && wizard.WizardSteps.Count > 0)
			{
				wizard.ActiveStepIndex = 0;
			}
			IControlDesignerAccessor controlDesignerAccessor = wizard;
			IDictionary designModeState = controlDesignerAccessor.GetDesignModeState();
			TemplatedWizardStep templatedWizardStep = wizard.ActiveStep as TemplatedWizardStep;
			if (templatedWizardStep != null && templatedWizardStep.ContentTemplate != null && ((TemplatedWizardStep)this._wizard.WizardSteps[wizard.ActiveStepIndex]).ContentTemplate == null)
			{
				return;
			}
			TableCell tableCell = designModeState["StepTableCell"] as TableCell;
			if (tableCell != null && wizard.ActiveStepIndex != -1)
			{
				tableCell.Attributes["_designerRegion"] = wizard.ActiveStepIndex.ToString(NumberFormatInfo.InvariantInfo);
			}
		}

		private void DataListItemDataBound(object sender, DataListItemEventArgs e)
		{
			DataListItem item = e.Item;
			WebControl webControl = item.FindControl("SideBarButton") as WebControl;
			if (webControl != null)
			{
				int num = item.ItemIndex + ((Wizard)base.ViewControl).WizardSteps.Count;
				webControl.Attributes["_designerRegion"] = num.ToString(NumberFormatInfo.InvariantInfo);
			}
		}

		public override string GetDesignTimeHtml()
		{
			if (this.ActiveStepIndex == -1)
			{
				return this.GetEmptyDesignTimeHtml();
			}
			Wizard wizard = (Wizard)base.ViewControl;
			IControlDesignerAccessor controlDesignerAccessor = wizard;
			IDictionary designModeState = controlDesignerAccessor.GetDesignModeState();
			DataList dataList = designModeState["SideBarList"] as DataList;
			if (dataList != null)
			{
				dataList.ItemDataBound += this.DataListItemDataBound;
				ICompositeControlDesignerAccessor compositeControlDesignerAccessor = wizard;
				compositeControlDesignerAccessor.RecreateChildControls();
			}
			ArrayList arrayList = new ArrayList(wizard.WizardSteps.Count);
			foreach (object obj in wizard.WizardSteps)
			{
				WizardStepBase wizardStepBase = (WizardStepBase)obj;
				arrayList.Add(wizardStepBase.Title);
				if ((wizardStepBase.Title == null || wizardStepBase.Title.Length == 0) && (wizardStepBase.ID == null || wizardStepBase.ID.Length == 0))
				{
					wizardStepBase.Title = this.GetRegionName(wizardStepBase);
				}
			}
			if (!this.InRegionEditingMode(wizard))
			{
				wizard.Enabled = true;
			}
			string text = base.GetDesignTimeHtml();
			if (text == null || text.Length == 0)
			{
				text = this.GetEmptyDesignTimeHtml();
			}
			return text;
		}

		public override string GetDesignTimeHtml(DesignerRegionCollection regions)
		{
			this.AddDesignerRegions(regions);
			IControlDesignerAccessor wizard = this._wizard;
			IDictionary dictionary = null;
			try
			{
				dictionary = wizard.GetDesignModeState();
			}
			catch (Exception ex)
			{
				return this.GetErrorDesignTimeHtml(ex);
			}
			DataList dataList = dictionary["SideBarList"] as DataList;
			if (dataList != null)
			{
				dataList.ItemDataBound += this.DataListItemDataBound;
			}
			Wizard wizard2 = (Wizard)base.ViewControl;
			IControlDesignerAccessor controlDesignerAccessor = wizard2;
			IDictionary designModeState = controlDesignerAccessor.GetDesignModeState();
			if (designModeState != null)
			{
				designModeState["ShouldRenderWizardSteps"] = this.InRegionEditingMode(wizard2);
			}
			return this.GetDesignTimeHtml();
		}

		public override string GetEditableDesignerRegionContent(EditableDesignerRegion region)
		{
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}
			IWizardStepEditableRegion wizardStepEditableRegion = region as IWizardStepEditableRegion;
			if (wizardStepEditableRegion == null)
			{
				throw new ArgumentException(SR.GetString("Wizard_InvalidRegion"));
			}
			return this.GetEditableDesignerRegionContent(wizardStepEditableRegion);
		}

		internal virtual string GetEditableDesignerRegionContent(IWizardStepEditableRegion region)
		{
			StringBuilder stringBuilder = new StringBuilder();
			ControlCollection controls = region.Step.Controls;
			IDesignerHost designerHost = (IDesignerHost)base.Component.Site.GetService(typeof(IDesignerHost));
			if (region.Step is TemplatedWizardStep)
			{
				TemplatedWizardStep templatedWizardStep = (TemplatedWizardStep)region.Step;
				return ControlPersister.PersistTemplate(templatedWizardStep.ContentTemplate, designerHost);
			}
			if (controls.Count == 1 && controls[0] is LiteralControl)
			{
				string text = ((LiteralControl)controls[0]).Text;
				if (text == null || text.Trim().Length == 0)
				{
					return string.Empty;
				}
			}
			foreach (object obj in controls)
			{
				Control control = (Control)obj;
				stringBuilder.Append(ControlPersister.PersistControl(control, designerHost));
			}
			return stringBuilder.ToString();
		}

		internal string GetRegionName(WizardStepBase step)
		{
			if (step.Title != null && step.Title.Length > 0)
			{
				return step.Title;
			}
			if (step.ID != null && step.ID.Length > 0)
			{
				return step.ID;
			}
			int num = step.Wizard.WizardSteps.IndexOf(step) + 1;
			return "[step (" + num + ")]";
		}

		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(Wizard));
			this._wizard = (Wizard)component;
			base.Initialize(component);
			base.SetViewFlags(ViewFlags.TemplateEditing, true);
		}

		private void MarkPropertyNonBrowsable(IDictionary properties, string propName)
		{
			PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties[propName];
			if (propertyDescriptor != null)
			{
				properties[propName] = TypeDescriptor.CreateProperty(propertyDescriptor.ComponentType, propertyDescriptor, new Attribute[] { BrowsableAttribute.No });
			}
		}

		protected override void OnClick(DesignerRegionMouseEventArgs e)
		{
			base.OnClick(e);
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			WizardSelectableRegion wizardSelectableRegion = e.Region as WizardSelectableRegion;
			if (wizardSelectableRegion != null)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this._wizard)["ActiveStepIndex"];
				int num = this._wizard.WizardSteps.IndexOf(wizardSelectableRegion.Step);
				if (this.ActiveStepIndex != num)
				{
					using (DesignerTransaction designerTransaction = designerHost.CreateTransaction("Update ActiveStepIndex"))
					{
						propertyDescriptor.SetValue(base.Component, num);
						designerTransaction.Commit();
					}
				}
			}
		}

		public override void OnComponentChanged(object sender, ComponentChangedEventArgs ce)
		{
			if (ce != null && ce.Member != null && ce.Member.Name == "WizardSteps" && this._wizard.ActiveStepIndex >= this._wizard.WizardSteps.Count)
			{
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				using (DesignerTransaction designerTransaction = designerHost.CreateTransaction("Update ActiveStepIndex"))
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this._wizard)["ActiveStepIndex"];
					propertyDescriptor.SetValue(base.Component, this._wizard.WizardSteps.Count - 1);
					designerTransaction.Commit();
				}
			}
			base.OnComponentChanged(sender, ce);
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties["DisplaySideBar"];
			if (propertyDescriptor != null)
			{
				properties["DisplaySideBar"] = TypeDescriptor.CreateProperty(base.GetType(), propertyDescriptor, null);
			}
			if (base.InTemplateMode)
			{
				this.MarkPropertyNonBrowsable(properties, "WizardSteps");
			}
			if (this._wizard.StartNavigationTemplate != null)
			{
				foreach (string text in WizardDesigner._startNavigationTemplateProperties)
				{
					this.MarkPropertyNonBrowsable(properties, text);
				}
			}
			if (this._wizard.StepNavigationTemplate != null)
			{
				foreach (string text2 in WizardDesigner._stepNavigationTemplateProperties)
				{
					this.MarkPropertyNonBrowsable(properties, text2);
				}
			}
			if (this._wizard.FinishNavigationTemplate != null)
			{
				foreach (string text3 in WizardDesigner._finishNavigationTemplateProperties)
				{
					this.MarkPropertyNonBrowsable(properties, text3);
				}
			}
			if (this._wizard.StartNavigationTemplate != null && this._wizard.StepNavigationTemplate != null && this._wizard.FinishNavigationTemplate != null)
			{
				foreach (string text4 in WizardDesigner._generalNavigationButtonProperties)
				{
					this.MarkPropertyNonBrowsable(properties, text4);
				}
			}
			if (this._wizard.HeaderTemplate != null)
			{
				foreach (string text5 in WizardDesigner._headerProperties)
				{
					this.MarkPropertyNonBrowsable(properties, text5);
				}
			}
			if (this._wizard.SideBarTemplate != null)
			{
				foreach (string text6 in WizardDesigner._sideBarProperties)
				{
					this.MarkPropertyNonBrowsable(properties, text6);
				}
			}
		}

		private void ResetInternalControls(IDictionary dictionary)
		{
			DataList dataList = (DataList)dictionary["SideBarList"];
			if (dataList != null)
			{
				dataList.SelectedIndex = -1;
			}
		}

		private void ResetCustomNavigationTemplate()
		{
			WizardStepBase activeStep = this.ActiveStep;
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ResetCustomNavigationTemplateCallBack), null, SR.GetString("Wizard_ResetCustomNavigationTemplate"));
		}

		private bool ResetCustomNavigationTemplateCallBack(object context)
		{
			WizardStepBase activeStep = this.ActiveStep;
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(activeStep)["CustomNavigationTemplate"];
			propertyDescriptor.ResetValue(activeStep);
			return true;
		}

		private void ResetStartNavigationTemplate()
		{
			this.ResetTemplate(SR.GetString("Wizard_ResetStartNavigationTemplate"), base.Component, "StartNavigationTemplate");
		}

		private void ResetStepNavigationTemplate()
		{
			this.ResetTemplate(SR.GetString("Wizard_ResetStepNavigationTemplate"), base.Component, "StepNavigationTemplate");
		}

		private void ResetFinishNavigationTemplate()
		{
			this.ResetTemplate(SR.GetString("Wizard_ResetFinishNavigationTemplate"), base.Component, "FinishNavigationTemplate");
		}

		private void ResetSideBarTemplate()
		{
			this.ResetTemplate(SR.GetString("Wizard_ResetSideBarTemplate"), base.Component, "SideBarTemplate");
		}

		protected void ResetTemplate(string description, IComponent component, string templateName)
		{
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ResetTemplateCallBack), new Pair(component, templateName), description);
			this.UpdateDesignTimeHtml();
		}

		private bool ResetTemplateCallBack(object context)
		{
			Pair pair = (Pair)context;
			IComponent component = (IComponent)pair.First;
			string text = (string)pair.Second;
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(component)[text];
			propertyDescriptor.ResetValue(component);
			return true;
		}

		public override void SetEditableDesignerRegionContent(EditableDesignerRegion region, string content)
		{
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}
			IWizardStepEditableRegion wizardStepEditableRegion = region as IWizardStepEditableRegion;
			if (wizardStepEditableRegion == null)
			{
				throw new ArgumentException(SR.GetString("Wizard_InvalidRegion"));
			}
			IDesignerHost designerHost = (IDesignerHost)base.Component.Site.GetService(typeof(IDesignerHost));
			if (wizardStepEditableRegion.Step is TemplatedWizardStep)
			{
				IComponent step = wizardStepEditableRegion.Step;
				ITemplate template = ControlParser.ParseTemplate(designerHost, content);
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(step)["ContentTemplate"];
				using (DesignerTransaction designerTransaction = designerHost.CreateTransaction("SetEditableDesignerRegionContent"))
				{
					propertyDescriptor.SetValue(step, template);
					designerTransaction.Commit();
				}
				this.ViewControlCreated = false;
				return;
			}
			this.SetWizardStepContent(wizardStepEditableRegion.Step, content, designerHost);
		}

		private void SetWizardStepContent(WizardStepBase step, string content, IDesignerHost host)
		{
			Control[] array = null;
			if (content != null && content.Length > 0)
			{
				array = ControlParser.ParseControls(host, content);
			}
			step.Controls.Clear();
			if (array == null)
			{
				return;
			}
			foreach (Control control in array)
			{
				step.Controls.Add(control);
			}
		}

		private void StartWizardStepCollectionEditor()
		{
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["WizardSteps"];
			using (DesignerTransaction designerTransaction = designerHost.CreateTransaction(SR.GetString("Wizard_StartWizardStepCollectionEditor")))
			{
				UITypeEditor uitypeEditor = (UITypeEditor)propertyDescriptor.GetEditor(typeof(UITypeEditor));
				object obj = uitypeEditor.EditValue(new TypeDescriptorContext(designerHost, propertyDescriptor, base.Component), new WindowsFormsEditorServiceHelper(this), propertyDescriptor.GetValue(base.Component));
				if (obj != null)
				{
					designerTransaction.Commit();
				}
			}
			if (this._wizard.ActiveStepIndex >= -1 && this._wizard.ActiveStepIndex < this._wizard.WizardSteps.Count)
			{
				try
				{
					this.ViewControlCreated = false;
					this.CreateChildControls();
				}
				catch
				{
				}
			}
		}

		private const string _headerTemplateName = "HeaderTemplate";

		internal const string _customNavigationTemplateName = "CustomNavigationTemplate";

		private const string _startNavigationTemplateName = "StartNavigationTemplate";

		private const string _stepNavigationTemplateName = "StepNavigationTemplate";

		private const string _finishNavigationTemplateName = "FinishNavigationTemplate";

		private const string _sideBarTemplateName = "SideBarTemplate";

		private const string _activeStepIndexPropName = "ActiveStepIndex";

		private const string _activeStepIndexTransactionDescription = "Update ActiveStepIndex";

		private const string _startNextButtonID = "StartNextButton";

		private const string _cancelButtonID = "CancelButton";

		private const string _stepTableCellID = "StepTableCell";

		private const string _displaySideBarPropName = "DisplaySideBar";

		private const string _stepPreviousButtonID = "StepPreviousButton";

		private const string _stepNextButtonID = "StepNextButton";

		private const string _finishButtonID = "FinishButton";

		private const string _finishPreviousButtonID = "FinishPreviousButton";

		private const string _dataListID = "SideBarList";

		private const string _sideBarButtonID = "SideBarButton";

		internal const string _customNavigationControls = "CustomNavigationControls";

		private const string _wizardStepsPropertyName = "WizardSteps";

		internal const string _contentTemplateName = "ContentTemplate";

		private const string _navigationTemplateName = "CustomNavigationTemplate";

		internal const int _navigationStyleLength = 6;

		private Wizard _wizard;

		private DesignerAutoFormatCollection _autoFormats;

		private bool _supportsDesignerRegion;

		private bool _supportsDesignerRegionQueried;

		private static string[] _stepTemplateNames = new string[] { "ContentTemplate", "CustomNavigationTemplate" };

		private static string[] _controlTemplateNames = new string[] { "HeaderTemplate", "SideBarTemplate", "StartNavigationTemplate", "StepNavigationTemplate", "FinishNavigationTemplate" };

		private static readonly string[] _startNavigationTemplateProperties = new string[] { "StartNextButtonText", "StartNextButtonType", "StartNextButtonImageUrl", "StartNextButtonStyle" };

		private static readonly string[] _stepNavigationTemplateProperties = new string[] { "StepNextButtonText", "StepNextButtonType", "StepNextButtonImageUrl", "StepPreviousButtonText", "StepPreviousButtonType", "StepPreviousButtonImageUrl", "StepPreviousButtonStyle", "StepNextButtonStyle" };

		private static readonly string[] _finishNavigationTemplateProperties = new string[] { "FinishCompleteButtonText", "FinishCompleteButtonType", "FinishCompleteButtonImageUrl", "FinishPreviousButtonText", "FinishPreviousButtonType", "FinishPreviousButtonImageUrl", "FinishCompleteButtonStyle", "FinishPreviousButtonStyle" };

		private static readonly string[] _generalNavigationButtonProperties = new string[] { "CancelButtonImageUrl", "CancelButtonText", "CancelButtonType", "DisplayCancelButton", "CancelButtonStyle", "NavigationButtonStyle" };

		private static readonly string[] _headerProperties = new string[] { "HeaderText" };

		private static readonly string[] _sideBarProperties = new string[] { "SideBarButtonStyle" };

		private static string[] _startButtonIDs = new string[] { "StartNextButton", "CancelButton" };

		private static string[] _stepButtonIDs = new string[] { "StepPreviousButton", "StepNextButton", "CancelButton" };

		private static string[] _finishButtonIDs = new string[] { "FinishPreviousButton", "FinishButton", "CancelButton" };

		[CompilerGenerated]
		private static ControlDesigner.CreateAutoFormatDelegate <>9__CachedAnonymousMethodDelegate1;

		private class WizardDesignerActionList : DesignerActionList
		{
			public WizardDesignerActionList(WizardDesigner designer)
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

			[TypeConverter(typeof(WizardDesigner.WizardDesignerActionList.WizardStepTypeConverter))]
			public int View
			{
				get
				{
					return this._designer.ActiveStepIndex;
				}
				set
				{
					if (value == this._designer.ActiveStepIndex)
					{
						return;
					}
					IDesignerHost designerHost = (IDesignerHost)this._designer.GetService(typeof(IDesignerHost));
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this._designer.Component)["ActiveStepIndex"];
					using (DesignerTransaction designerTransaction = designerHost.CreateTransaction(SR.GetString("Wizard_OnViewChanged")))
					{
						propertyDescriptor.SetValue(this._designer.Component, value);
						designerTransaction.Commit();
					}
					this._designer.UpdateDesignTimeHtml();
					TypeDescriptor.Refresh(this._designer.Component);
				}
			}

			public override DesignerActionItemCollection GetSortedActionItems()
			{
				DesignerActionItemCollection designerActionItemCollection = new DesignerActionItemCollection();
				if (!this._designer.InTemplateMode)
				{
					if (this._designer._wizard.WizardSteps.Count > 0)
					{
						designerActionItemCollection.Add(new DesignerActionPropertyItem("View", SR.GetString("Wizard_StepsView"), string.Empty, SR.GetString("Wizard_StepsViewDescription")));
					}
					designerActionItemCollection.Add(new DesignerActionMethodItem(this, "StartWizardStepCollectionEditor", SR.GetString("Wizard_StartWizardStepCollectionEditor"), string.Empty, SR.GetString("Wizard_StartWizardStepCollectionEditorDescription"), true));
					Wizard wizard = this._designer._wizard;
					int activeStepIndex = this._designer.ActiveStepIndex;
					if (activeStepIndex >= 0 && activeStepIndex < wizard.WizardSteps.Count)
					{
						if (wizard.StartNavigationTemplate != null)
						{
							designerActionItemCollection.Add(new DesignerActionMethodItem(this, "ResetStartNavigationTemplate", SR.GetString("Wizard_ResetStartNavigationTemplate"), string.Empty, SR.GetString("Wizard_ResetDescription", new object[] { "StartNavigation" }), true));
						}
						else
						{
							designerActionItemCollection.Add(new DesignerActionMethodItem(this, "ConvertToStartNavigationTemplate", SR.GetString("Wizard_ConvertToStartNavigationTemplate"), string.Empty, SR.GetString("Wizard_ConvertToTemplateDescription", new object[] { "StartNavigation" }), true));
						}
						if (wizard.StepNavigationTemplate != null)
						{
							designerActionItemCollection.Add(new DesignerActionMethodItem(this, "ResetStepNavigationTemplate", SR.GetString("Wizard_ResetStepNavigationTemplate"), string.Empty, SR.GetString("Wizard_ResetDescription", new object[] { "StepNavigation" }), true));
						}
						else
						{
							designerActionItemCollection.Add(new DesignerActionMethodItem(this, "ConvertToStepNavigationTemplate", SR.GetString("Wizard_ConvertToStepNavigationTemplate"), string.Empty, SR.GetString("Wizard_ConvertToTemplateDescription", new object[] { "StepNavigation" }), true));
						}
						if (wizard.FinishNavigationTemplate != null)
						{
							designerActionItemCollection.Add(new DesignerActionMethodItem(this, "ResetFinishNavigationTemplate", SR.GetString("Wizard_ResetFinishNavigationTemplate"), string.Empty, SR.GetString("Wizard_ResetDescription", new object[] { "FinishNavigation" }), true));
						}
						else
						{
							designerActionItemCollection.Add(new DesignerActionMethodItem(this, "ConvertToFinishNavigationTemplate", SR.GetString("Wizard_ConvertToFinishNavigationTemplate"), string.Empty, SR.GetString("Wizard_ConvertToTemplateDescription", new object[] { "FinishNavigation" }), true));
						}
						if (wizard.DisplaySideBar)
						{
							if (wizard.SideBarTemplate != null)
							{
								designerActionItemCollection.Add(new DesignerActionMethodItem(this, "ResetSideBarTemplate", SR.GetString("Wizard_ResetSideBarTemplate"), string.Empty, SR.GetString("Wizard_ResetDescription", new object[] { "SideBar" }), true));
							}
							else
							{
								designerActionItemCollection.Add(new DesignerActionMethodItem(this, "ConvertToSideBarTemplate", SR.GetString("Wizard_ConvertToSideBarTemplate"), string.Empty, SR.GetString("Wizard_ConvertToTemplateDescription", new object[] { "SideBar" }), true));
							}
						}
						TemplatedWizardStep templatedWizardStep = this._designer.ActiveStep as TemplatedWizardStep;
						if (templatedWizardStep != null && templatedWizardStep.StepType != WizardStepType.Complete)
						{
							if (templatedWizardStep.CustomNavigationTemplate != null)
							{
								designerActionItemCollection.Add(new DesignerActionMethodItem(this, "ResetCustomNavigationTemplate", SR.GetString("Wizard_ResetCustomNavigationTemplate"), string.Empty, SR.GetString("Wizard_ResetDescription", new object[] { "CustomNavigation" }), true));
							}
							else
							{
								designerActionItemCollection.Add(new DesignerActionMethodItem(this, "ConvertToCustomNavigationTemplate", SR.GetString("Wizard_ConvertToCustomNavigationTemplate"), string.Empty, SR.GetString("Wizard_ConvertToTemplateDescription", new object[] { "CustomNavigation" }), true));
							}
						}
					}
				}
				return designerActionItemCollection;
			}

			public void ConvertToCustomNavigationTemplate()
			{
				this._designer.ConvertToCustomNavigationTemplate();
			}

			public void ConvertToFinishNavigationTemplate()
			{
				this._designer.ConvertToFinishNavigationTemplate();
			}

			public void ConvertToSideBarTemplate()
			{
				this._designer.ConvertToSideBarTemplate();
			}

			public void ConvertToStartNavigationTemplate()
			{
				this._designer.ConvertToStartNavigationTemplate();
			}

			public void ConvertToStepNavigationTemplate()
			{
				this._designer.ConvertToStepNavigationTemplate();
			}

			public void ResetCustomNavigationTemplate()
			{
				this._designer.ResetCustomNavigationTemplate();
			}

			public void ResetFinishNavigationTemplate()
			{
				this._designer.ResetFinishNavigationTemplate();
			}

			public void ResetSideBarTemplate()
			{
				this._designer.ResetSideBarTemplate();
			}

			public void ResetStartNavigationTemplate()
			{
				this._designer.ResetStartNavigationTemplate();
			}

			public void ResetStepNavigationTemplate()
			{
				this._designer.ResetStepNavigationTemplate();
			}

			public void StartWizardStepCollectionEditor()
			{
				this._designer.StartWizardStepCollectionEditor();
			}

			private WizardDesigner _designer;

			private class WizardStepTypeConverter : TypeConverter
			{
				public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
				{
					int[] array = null;
					if (context != null)
					{
						WizardDesigner.WizardDesignerActionList wizardDesignerActionList = (WizardDesigner.WizardDesignerActionList)context.Instance;
						WizardDesigner designer = wizardDesignerActionList._designer;
						WizardStepCollection wizardSteps = designer._wizard.WizardSteps;
						array = new int[wizardSteps.Count];
						for (int i = 0; i < wizardSteps.Count; i++)
						{
							array[i] = i;
						}
					}
					return new TypeConverter.StandardValuesCollection(array);
				}

				public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
				{
					return true;
				}

				public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
				{
					return true;
				}

				public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
				{
					if (destinationType == typeof(string))
					{
						if (value is string)
						{
							return value;
						}
						WizardDesigner.WizardDesignerActionList wizardDesignerActionList = (WizardDesigner.WizardDesignerActionList)context.Instance;
						WizardDesigner designer = wizardDesignerActionList._designer;
						WizardStepCollection wizardSteps = designer._wizard.WizardSteps;
						if (value is int)
						{
							int num = (int)value;
							if (num == -1 && wizardSteps.Count > 0)
							{
								num = 0;
							}
							if (num >= wizardSteps.Count)
							{
								return null;
							}
							return designer.GetRegionName(wizardSteps[num]);
						}
					}
					return base.ConvertTo(context, culture, value, destinationType);
				}

				public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
				{
					if (value is string)
					{
						WizardDesigner.WizardDesignerActionList wizardDesignerActionList = (WizardDesigner.WizardDesignerActionList)context.Instance;
						WizardDesigner designer = wizardDesignerActionList._designer;
						WizardStepCollection wizardSteps = designer._wizard.WizardSteps;
						for (int i = 0; i < wizardSteps.Count; i++)
						{
							if (string.Compare(designer.GetRegionName(wizardSteps[i]), (string)value, StringComparison.Ordinal) == 0)
							{
								return i;
							}
						}
					}
					return base.ConvertFrom(context, culture, value);
				}

				public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
				{
					return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
				}

				public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
				{
					return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
				}
			}
		}
	}
}
