using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Design;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000476 RID: 1142
	public class MenuDesigner : HierarchicalDataBoundControlDesigner, IDataBindingSchemaProvider
	{
		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x0600296F RID: 10607 RVA: 0x000E3650 File Offset: 0x000E2650
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection designerActionListCollection = new DesignerActionListCollection();
				designerActionListCollection.AddRange(base.ActionLists);
				designerActionListCollection.Add(new MenuDesigner.MenuDesignerActionList(this));
				return designerActionListCollection;
			}
		}

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x06002970 RID: 10608 RVA: 0x000E3685 File Offset: 0x000E2685
		public override DesignerAutoFormatCollection AutoFormats
		{
			get
			{
				if (MenuDesigner._autoFormats == null)
				{
					MenuDesigner._autoFormats = ControlDesigner.CreateAutoFormats("<Schemes>\r\n<xsd:schema id=\"Schemes\" xmlns=\"\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\">\r\n  <xsd:element name=\"Scheme\">\r\n     <xsd:complexType>\r\n       <xsd:all>\r\n        <xsd:element name=\"SchemeName\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"BackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"BorderColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"BorderWidth\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"BorderStyle\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"DynamicHorizontalOffset\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"DynamicHoverStyle-BackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"DynamicHoverStyle-Font--ClearDefaults\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"DynamicHoverStyle-ForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"DynamicMenuItemStyle-HorizontalPadding\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"DynamicMenuItemStyle-VerticalPadding\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"DynamicMenuStyle-BackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"DynamicSelectedStyle-BackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"Font-Size\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"Font-Names\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"ForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"StaticHoverStyle-BackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"StaticHoverStyle-Font--ClearDefaults\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"StaticHoverStyle-ForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"StaticMenuItemStyle-HorizontalPadding\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"StaticMenuItemStyle-VerticalPadding\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"StaticSelectedStyle-BackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"StaticSubMenuIndent\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n      </xsd:all>\r\n    </xsd:complexType>\r\n  </xsd:element>\r\n  <xsd:element name=\"Schemes\" msdata:IsDataSet=\"true\">\r\n    <xsd:complexType>\r\n      <xsd:choice maxOccurs=\"unbounded\">\r\n        <xsd:element ref=\"Scheme\"/>\r\n      </xsd:choice>\r\n    </xsd:complexType>\r\n  </xsd:element>\r\n</xsd:schema>\r\n<Scheme>\r\n    <SchemeName>MenuScheme_Empty</SchemeName>\r\n    <BackColor></BackColor>\r\n    <BorderColor></BorderColor>\r\n    <BorderWidth></BorderWidth>\r\n    <BorderStyle>notset</BorderStyle>\r\n    <DynamicHorizontalOffset>0</DynamicHorizontalOffset>\r\n    <DynamicHoverStyle-BackColor></DynamicHoverStyle-BackColor>\r\n    <DynamicHoverStyle-Font--ClearDefaults>true</DynamicHoverStyle-Font--ClearDefaults>\r\n    <DynamicHoverStyle-ForeColor></DynamicHoverStyle-ForeColor>\r\n    <DynamicMenuItemStyle-HorizontalPadding></DynamicMenuItemStyle-HorizontalPadding>\r\n    <DynamicMenuItemStyle-VerticalPadding></DynamicMenuItemStyle-VerticalPadding>\r\n    <DynamicMenuStyle-BackColor></DynamicMenuStyle-BackColor>\r\n    <DynamicSelectedStyle-BackColor></DynamicSelectedStyle-BackColor>\r\n    <Font-Size></Font-Size>\r\n    <Font-Names></Font-Names>\r\n    <ForeColor></ForeColor>\r\n    <StaticHoverStyle-BackColor></StaticHoverStyle-BackColor>\r\n    <StaticHoverStyle-Font--ClearDefaults>true</StaticHoverStyle-Font--ClearDefaults>\r\n    <StaticHoverStyle-ForeColor></StaticHoverStyle-ForeColor>\r\n    <StaticMenuItemStyle-HorizontalPadding></StaticMenuItemStyle-HorizontalPadding>\r\n    <StaticMenuItemStyle-VerticalPadding></StaticMenuItemStyle-VerticalPadding>\r\n    <StaticSelectedStyle-BackColor></StaticSelectedStyle-BackColor>\r\n    <StaticSubMenuIndent>16px</StaticSubMenuIndent>\r\n</Scheme>\r\n  <Scheme>\r\n    <SchemeName>MenuScheme_Classic</SchemeName>\r\n    <BackColor>#B5C7DE</BackColor>\r\n    <BorderColor></BorderColor>\r\n    <BorderWidth></BorderWidth>\r\n    <BorderStyle>notset</BorderStyle>\r\n    <DynamicHorizontalOffset>2</DynamicHorizontalOffset>\r\n    <DynamicHoverStyle-BackColor>#284E98</DynamicHoverStyle-BackColor>\r\n    <DynamicHoverStyle-Font--ClearDefaults>false</DynamicHoverStyle-Font--ClearDefaults>\r\n    <DynamicHoverStyle-ForeColor>White</DynamicHoverStyle-ForeColor>\r\n    <DynamicMenuItemStyle-HorizontalPadding>5</DynamicMenuItemStyle-HorizontalPadding>\r\n    <DynamicMenuItemStyle-VerticalPadding>2</DynamicMenuItemStyle-VerticalPadding>\r\n    <DynamicMenuStyle-BackColor>#B5C7DE</DynamicMenuStyle-BackColor>\r\n    <DynamicSelectedStyle-BackColor>#507CD1</DynamicSelectedStyle-BackColor>\r\n    <Font-Names>Verdana</Font-Names>\r\n    <Font-Size>0.8em</Font-Size>\r\n    <ForeColor>#284E98</ForeColor>\r\n    <StaticHoverStyle-BackColor>#284E98</StaticHoverStyle-BackColor>\r\n    <StaticHoverStyle-Font--ClearDefaults>false</StaticHoverStyle-Font--ClearDefaults>\r\n    <StaticHoverStyle-ForeColor>White</StaticHoverStyle-ForeColor>\r\n    <StaticMenuItemStyle-HorizontalPadding>5</StaticMenuItemStyle-HorizontalPadding>\r\n    <StaticMenuItemStyle-VerticalPadding>2</StaticMenuItemStyle-VerticalPadding>\r\n    <StaticSelectedStyle-BackColor>#507CD1</StaticSelectedStyle-BackColor>\r\n    <StaticSubMenuIndent>10px</StaticSubMenuIndent>\r\n  </Scheme>\r\n<Scheme>\r\n    <SchemeName>MenuScheme_Colorful</SchemeName>\r\n    <BackColor>#FFFBD6</BackColor>\r\n    <BorderColor></BorderColor>\r\n    <BorderWidth></BorderWidth>\r\n    <BorderStyle>notset</BorderStyle>\r\n    <DynamicHorizontalOffset>2</DynamicHorizontalOffset>\r\n    <DynamicHoverStyle-BackColor>#990000</DynamicHoverStyle-BackColor>\r\n    <DynamicHoverStyle-Font--ClearDefaults>false</DynamicHoverStyle-Font--ClearDefaults>\r\n    <DynamicHoverStyle-ForeColor>White</DynamicHoverStyle-ForeColor>\r\n    <DynamicMenuItemStyle-HorizontalPadding>5</DynamicMenuItemStyle-HorizontalPadding>\r\n    <DynamicMenuItemStyle-VerticalPadding>2</DynamicMenuItemStyle-VerticalPadding>\r\n    <DynamicMenuStyle-BackColor>#FFFBD6</DynamicMenuStyle-BackColor>\r\n    <DynamicSelectedStyle-BackColor>#FFCC66</DynamicSelectedStyle-BackColor>\r\n    <Font-Names>Verdana</Font-Names>\r\n    <Font-Size>0.8em</Font-Size>\r\n    <ForeColor>#990000</ForeColor>\r\n    <StaticHoverStyle-BackColor>#990000</StaticHoverStyle-BackColor>\r\n    <StaticHoverStyle-Font--ClearDefaults>false</StaticHoverStyle-Font--ClearDefaults>\r\n    <StaticHoverStyle-ForeColor>White</StaticHoverStyle-ForeColor>\r\n    <StaticMenuItemStyle-HorizontalPadding>5</StaticMenuItemStyle-HorizontalPadding>\r\n    <StaticMenuItemStyle-VerticalPadding>2</StaticMenuItemStyle-VerticalPadding>\r\n    <StaticSelectedStyle-BackColor>#FFCC66</StaticSelectedStyle-BackColor>\r\n    <StaticSubMenuIndent>10px</StaticSubMenuIndent>\r\n</Scheme>\r\n<Scheme>\r\n    <SchemeName>MenuScheme_Professional</SchemeName>\r\n    <BackColor>#F7F6F3</BackColor>\r\n    <BorderColor></BorderColor>\r\n    <BorderWidth></BorderWidth>\r\n    <BorderStyle>notset</BorderStyle>\r\n    <DynamicHorizontalOffset>2</DynamicHorizontalOffset>\r\n    <DynamicHoverStyle-BackColor>#7C6F57</DynamicHoverStyle-BackColor>\r\n    <DynamicHoverStyle-Font--ClearDefaults>false</DynamicHoverStyle-Font--ClearDefaults>\r\n    <DynamicHoverStyle-ForeColor>White</DynamicHoverStyle-ForeColor>\r\n    <DynamicMenuItemStyle-HorizontalPadding>5</DynamicMenuItemStyle-HorizontalPadding>\r\n    <DynamicMenuItemStyle-VerticalPadding>2</DynamicMenuItemStyle-VerticalPadding>\r\n    <DynamicMenuStyle-BackColor>#F7F6F3</DynamicMenuStyle-BackColor>\r\n    <DynamicSelectedStyle-BackColor>#5D7B9D</DynamicSelectedStyle-BackColor>\r\n    <Font-Names>Verdana</Font-Names>\r\n    <Font-Size>0.8em</Font-Size>\r\n    <ForeColor>#7C6F57</ForeColor>\r\n    <StaticHoverStyle-BackColor>#7C6F57</StaticHoverStyle-BackColor>\r\n    <StaticHoverStyle-Font--ClearDefaults>false</StaticHoverStyle-Font--ClearDefaults>\r\n    <StaticHoverStyle-ForeColor>White</StaticHoverStyle-ForeColor>\r\n    <StaticMenuItemStyle-HorizontalPadding>5</StaticMenuItemStyle-HorizontalPadding>\r\n    <StaticMenuItemStyle-VerticalPadding>2</StaticMenuItemStyle-VerticalPadding>\r\n    <StaticSelectedStyle-BackColor>#5D7B9D</StaticSelectedStyle-BackColor>\r\n    <StaticSubMenuIndent>10px</StaticSubMenuIndent>\r\n</Scheme>\r\n  <Scheme>\r\n    <SchemeName>MenuScheme_Simple</SchemeName>\r\n    <BackColor>#E3EAEB</BackColor>\r\n    <BorderColor></BorderColor>\r\n    <BorderWidth></BorderWidth>\r\n    <BorderStyle>notset</BorderStyle>\r\n    <DynamicHorizontalOffset>2</DynamicHorizontalOffset>\r\n    <DynamicHoverStyle-BackColor>#666666</DynamicHoverStyle-BackColor>\r\n    <DynamicHoverStyle-Font--ClearDefaults>false</DynamicHoverStyle-Font--ClearDefaults>\r\n    <DynamicHoverStyle-ForeColor>White</DynamicHoverStyle-ForeColor>\r\n    <DynamicMenuItemStyle-HorizontalPadding>5</DynamicMenuItemStyle-HorizontalPadding>\r\n    <DynamicMenuItemStyle-VerticalPadding>2</DynamicMenuItemStyle-VerticalPadding>\r\n    <DynamicMenuStyle-BackColor>#E3EAEB</DynamicMenuStyle-BackColor>\r\n    <DynamicSelectedStyle-BackColor>#1C5E55</DynamicSelectedStyle-BackColor>\r\n    <Font-Names>Verdana</Font-Names>\r\n    <Font-Size>0.8em</Font-Size>\r\n    <ForeColor>#666666</ForeColor>\r\n    <StaticHoverStyle-BackColor>#666666</StaticHoverStyle-BackColor>\r\n    <StaticHoverStyle-Font--ClearDefaults>false</StaticHoverStyle-Font--ClearDefaults>\r\n    <StaticHoverStyle-ForeColor>White</StaticHoverStyle-ForeColor>\r\n    <StaticMenuItemStyle-HorizontalPadding>5</StaticMenuItemStyle-HorizontalPadding>\r\n    <StaticMenuItemStyle-VerticalPadding>2</StaticMenuItemStyle-VerticalPadding>\r\n    <StaticSelectedStyle-BackColor>#1C5E55</StaticSelectedStyle-BackColor>\r\n    <StaticSubMenuIndent>10px</StaticSubMenuIndent>\r\n  </Scheme>\r\n</Schemes>\r\n", (DataRow schemeData) => new MenuAutoFormat(schemeData));
				}
				return MenuDesigner._autoFormats;
			}
		}

		// Token: 0x06002971 RID: 10609 RVA: 0x000E36BF File Offset: 0x000E26BF
		private void ConvertToDynamicTemplate()
		{
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ConvertToDynamicTemplateChangeCallback), null, SR.GetString("MenuDesigner_ConvertToDynamicTemplate"));
		}

		// Token: 0x06002972 RID: 10610 RVA: 0x000E36E4 File Offset: 0x000E26E4
		private bool ConvertToDynamicTemplateChangeCallback(object context)
		{
			string dynamicItemFormatString = this._menu.DynamicItemFormatString;
			string text;
			if (dynamicItemFormatString != null && dynamicItemFormatString.Length != 0)
			{
				text = "<%# Eval(\"Text\", \"" + dynamicItemFormatString + "\") %>";
			}
			else
			{
				text = "<%# Eval(\"Text\") %>";
			}
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (designerHost != null)
			{
				this._menu.DynamicItemTemplate = ControlParser.ParseTemplate(designerHost, text);
			}
			return true;
		}

		// Token: 0x06002973 RID: 10611 RVA: 0x000E374F File Offset: 0x000E274F
		private void ConvertToStaticTemplate()
		{
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ConvertToStaticTemplateChangeCallback), null, SR.GetString("MenuDesigner_ConvertToStaticTemplate"));
		}

		// Token: 0x06002974 RID: 10612 RVA: 0x000E3774 File Offset: 0x000E2774
		private bool ConvertToStaticTemplateChangeCallback(object context)
		{
			string staticItemFormatString = this._menu.StaticItemFormatString;
			string text;
			if (staticItemFormatString != null && staticItemFormatString.Length != 0)
			{
				text = "<%# Eval(\"Text\", \"" + staticItemFormatString + "\") %>";
			}
			else
			{
				text = "<%# Eval(\"Text\") %>";
			}
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (designerHost != null)
			{
				this._menu.StaticItemTemplate = ControlParser.ParseTemplate(designerHost, text);
			}
			return true;
		}

		// Token: 0x06002975 RID: 10613 RVA: 0x000E37DF File Offset: 0x000E27DF
		private void ResetDynamicTemplate()
		{
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ResetDynamicTemplateChangeCallback), null, SR.GetString("MenuDesigner_ResetDynamicTemplate"));
		}

		// Token: 0x06002976 RID: 10614 RVA: 0x000E3803 File Offset: 0x000E2803
		private bool ResetDynamicTemplateChangeCallback(object context)
		{
			this._menu.Controls.Clear();
			this._menu.DynamicItemTemplate = null;
			return true;
		}

		// Token: 0x06002977 RID: 10615 RVA: 0x000E3822 File Offset: 0x000E2822
		private void ResetStaticTemplate()
		{
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ResetStaticTemplateChangeCallback), null, SR.GetString("MenuDesigner_ResetStaticTemplate"));
		}

		// Token: 0x06002978 RID: 10616 RVA: 0x000E3846 File Offset: 0x000E2846
		private bool ResetStaticTemplateChangeCallback(object context)
		{
			this._menu.Controls.Clear();
			this._menu.StaticItemTemplate = null;
			return true;
		}

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x06002979 RID: 10617 RVA: 0x000E3865 File Offset: 0x000E2865
		private bool DynamicTemplated
		{
			get
			{
				return this._menu.DynamicItemTemplate != null;
			}
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x0600297A RID: 10618 RVA: 0x000E3878 File Offset: 0x000E2878
		private bool StaticTemplated
		{
			get
			{
				return this._menu.StaticItemTemplate != null;
			}
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x0600297B RID: 10619 RVA: 0x000E388C File Offset: 0x000E288C
		public override TemplateGroupCollection TemplateGroups
		{
			get
			{
				TemplateGroupCollection templateGroups = base.TemplateGroups;
				if (this._templateGroups == null)
				{
					this._templateGroups = new TemplateGroupCollection();
					TemplateGroup templateGroup = new TemplateGroup("Item Templates", ((WebControl)base.ViewControl).ControlStyle);
					templateGroup.AddTemplateDefinition(new TemplateDefinition(this, MenuDesigner._templateNames[0], this._menu, MenuDesigner._templateNames[0], ((global::System.Web.UI.WebControls.Menu)base.ViewControl).StaticMenuStyle)
					{
						SupportsDataBinding = true
					});
					templateGroup.AddTemplateDefinition(new TemplateDefinition(this, MenuDesigner._templateNames[1], this._menu, MenuDesigner._templateNames[1], ((global::System.Web.UI.WebControls.Menu)base.ViewControl).DynamicMenuStyle)
					{
						SupportsDataBinding = true
					});
					this._templateGroups.Add(templateGroup);
				}
				templateGroups.AddRange(this._templateGroups);
				return templateGroups;
			}
		}

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x0600297C RID: 10620 RVA: 0x000E395D File Offset: 0x000E295D
		protected override bool UsePreviewControl
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600297D RID: 10621 RVA: 0x000E3960 File Offset: 0x000E2960
		protected override void DataBind(BaseDataBoundControl dataBoundControl)
		{
			global::System.Web.UI.WebControls.Menu menu = (global::System.Web.UI.WebControls.Menu)dataBoundControl;
			if ((menu.DataSourceID != null && menu.DataSourceID.Length > 0) || menu.DataSource != null || menu.Items.Count == 0)
			{
				menu.Items.Clear();
				base.DataBind(menu);
			}
		}

		// Token: 0x0600297E RID: 10622 RVA: 0x000E39B4 File Offset: 0x000E29B4
		private void EditBindings()
		{
			IServiceProvider site = this._menu.Site;
			MenuBindingsEditorForm menuBindingsEditorForm = new MenuBindingsEditorForm(site, this._menu, this);
			UIServiceHelper.ShowDialog(site, menuBindingsEditorForm);
		}

		// Token: 0x0600297F RID: 10623 RVA: 0x000E39E4 File Offset: 0x000E29E4
		private void EditMenuItems()
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["Items"];
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.EditMenuItemsChangeCallback), null, SR.GetString("MenuDesigner_EditNodesTransactionDescription"), propertyDescriptor);
		}

		// Token: 0x06002980 RID: 10624 RVA: 0x000E3A2C File Offset: 0x000E2A2C
		private bool EditMenuItemsChangeCallback(object context)
		{
			IServiceProvider site = this._menu.Site;
			MenuItemCollectionEditorDialog menuItemCollectionEditorDialog = new MenuItemCollectionEditorDialog(this._menu, this);
			DialogResult dialogResult = UIServiceHelper.ShowDialog(site, menuItemCollectionEditorDialog);
			return dialogResult == DialogResult.OK;
		}

		// Token: 0x06002981 RID: 10625 RVA: 0x000E3A60 File Offset: 0x000E2A60
		public override string GetDesignTimeHtml()
		{
			string text;
			try
			{
				global::System.Web.UI.WebControls.Menu menu = (global::System.Web.UI.WebControls.Menu)base.ViewControl;
				ListDictionary listDictionary = new ListDictionary();
				listDictionary.Add("DesignTimeTextWriterType", typeof(DesignTimeHtmlTextWriter));
				((IControlDesignerAccessor)base.ViewControl).SetDesignModeState(listDictionary);
				int maximumDynamicDisplayLevels = menu.MaximumDynamicDisplayLevels;
				if (maximumDynamicDisplayLevels > 10)
				{
					menu.MaximumDynamicDisplayLevels = 10;
				}
				this.DataBind((BaseDataBoundControl)base.ViewControl);
				IDictionary designModeState = ((IControlDesignerAccessor)base.ViewControl).GetDesignModeState();
				switch (this._currentView)
				{
				case MenuDesigner.ViewType.Static:
					text = (string)designModeState["GetDesignTimeStaticHtml"];
					break;
				case MenuDesigner.ViewType.Dynamic:
					text = (string)designModeState["GetDesignTimeDynamicHtml"];
					break;
				default:
					if (maximumDynamicDisplayLevels > 10)
					{
						menu.MaximumDynamicDisplayLevels = maximumDynamicDisplayLevels;
					}
					text = base.GetDesignTimeHtml();
					break;
				}
			}
			catch (Exception ex)
			{
				text = this.GetErrorDesignTimeHtml(ex);
			}
			return text;
		}

		// Token: 0x06002982 RID: 10626 RVA: 0x000E3B48 File Offset: 0x000E2B48
		protected override string GetEmptyDesignTimeHtml()
		{
			string name = this._menu.Site.Name;
			return string.Format(CultureInfo.CurrentUICulture, "\r\n                <table cellpadding=4 cellspacing=0 style=\"font-family:Tahoma;font-size:8pt;color:buttontext;background-color:buttonface\">\r\n                  <tr><td><span style=\"font-weight:bold\">Menu</span> - {0}</td></tr>\r\n                  <tr><td>{1}</td></tr>\r\n                </table>\r\n             ", new object[]
			{
				name,
				SR.GetString("MenuDesigner_Empty")
			});
		}

		// Token: 0x06002983 RID: 10627 RVA: 0x000E3B90 File Offset: 0x000E2B90
		protected override string GetErrorDesignTimeHtml(Exception e)
		{
			string name = this._menu.Site.Name;
			return string.Format(CultureInfo.CurrentUICulture, "\r\n                <table cellpadding=4 cellspacing=0 style=\"font-family:Tahoma;font-size:8pt;color:buttontext;background-color:buttonface;border: solid 1px;border-top-color:buttonhighlight;border-left-color:buttonhighlight;border-bottom-color:buttonshadow;border-right-color:buttonshadow\">\r\n                  <tr><td><span style=\"font-weight:bold\">Menu</span> - {0}</td></tr>\r\n                  <tr><td>{1}</td></tr>\r\n                </table>\r\n             ", new object[]
			{
				name,
				SR.GetString("MenuDesigner_Error", new object[] { e.Message })
			});
		}

		// Token: 0x06002984 RID: 10628 RVA: 0x000E3BE7 File Offset: 0x000E2BE7
		protected override IHierarchicalEnumerable GetSampleDataSource()
		{
			return new MenuDesigner.MenuSampleData(this._menu, 0, string.Empty);
		}

		// Token: 0x06002985 RID: 10629 RVA: 0x000E3BFA File Offset: 0x000E2BFA
		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(global::System.Web.UI.WebControls.Menu));
			base.Initialize(component);
			this._menu = (global::System.Web.UI.WebControls.Menu)component;
			base.SetViewFlags(ViewFlags.TemplateEditing, true);
		}

		// Token: 0x06002986 RID: 10630 RVA: 0x000E3C27 File Offset: 0x000E2C27
		internal void InvokeMenuBindingsEditor()
		{
			this.EditBindings();
		}

		// Token: 0x06002987 RID: 10631 RVA: 0x000E3C2F File Offset: 0x000E2C2F
		internal void InvokeMenuItemCollectionEditor()
		{
			this.EditMenuItems();
		}

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x06002988 RID: 10632 RVA: 0x000E3C37 File Offset: 0x000E2C37
		bool IDataBindingSchemaProvider.CanRefreshSchema
		{
			get
			{
				return this.CanRefreshSchema;
			}
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x06002989 RID: 10633 RVA: 0x000E3C3F File Offset: 0x000E2C3F
		protected bool CanRefreshSchema
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x0600298A RID: 10634 RVA: 0x000E3C42 File Offset: 0x000E2C42
		IDataSourceViewSchema IDataBindingSchemaProvider.Schema
		{
			get
			{
				return this.Schema;
			}
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x0600298B RID: 10635 RVA: 0x000E3C4A File Offset: 0x000E2C4A
		protected IDataSourceViewSchema Schema
		{
			get
			{
				return new MenuDesigner.MenuItemSchema();
			}
		}

		// Token: 0x0600298C RID: 10636 RVA: 0x000E3C51 File Offset: 0x000E2C51
		protected void RefreshSchema(bool preferSilent)
		{
		}

		// Token: 0x0600298D RID: 10637 RVA: 0x000E3C53 File Offset: 0x000E2C53
		void IDataBindingSchemaProvider.RefreshSchema(bool preferSilent)
		{
			this.RefreshSchema(preferSilent);
		}

		// Token: 0x04001C7C RID: 7292
		private const string _getDesignTimeStaticHtml = "GetDesignTimeStaticHtml";

		// Token: 0x04001C7D RID: 7293
		private const string _getDesignTimeDynamicHtml = "GetDesignTimeDynamicHtml";

		// Token: 0x04001C7E RID: 7294
		private const string emptyDesignTimeHtml = "\r\n                <table cellpadding=4 cellspacing=0 style=\"font-family:Tahoma;font-size:8pt;color:buttontext;background-color:buttonface\">\r\n                  <tr><td><span style=\"font-weight:bold\">Menu</span> - {0}</td></tr>\r\n                  <tr><td>{1}</td></tr>\r\n                </table>\r\n             ";

		// Token: 0x04001C7F RID: 7295
		private const string errorDesignTimeHtml = "\r\n                <table cellpadding=4 cellspacing=0 style=\"font-family:Tahoma;font-size:8pt;color:buttontext;background-color:buttonface;border: solid 1px;border-top-color:buttonhighlight;border-left-color:buttonhighlight;border-bottom-color:buttonshadow;border-right-color:buttonshadow\">\r\n                  <tr><td><span style=\"font-weight:bold\">Menu</span> - {0}</td></tr>\r\n                  <tr><td>{1}</td></tr>\r\n                </table>\r\n             ";

		// Token: 0x04001C80 RID: 7296
		private const int _maxDesignDepth = 10;

		// Token: 0x04001C81 RID: 7297
		private global::System.Web.UI.WebControls.Menu _menu;

		// Token: 0x04001C82 RID: 7298
		private TemplateGroupCollection _templateGroups;

		// Token: 0x04001C83 RID: 7299
		private static DesignerAutoFormatCollection _autoFormats;

		// Token: 0x04001C84 RID: 7300
		private MenuDesigner.ViewType _currentView;

		// Token: 0x04001C85 RID: 7301
		private static readonly string[] _templateNames = new string[] { "StaticItemTemplate", "DynamicItemTemplate" };

		// Token: 0x04001C86 RID: 7302
		[CompilerGenerated]
		private static ControlDesigner.CreateAutoFormatDelegate <>9__CachedAnonymousMethodDelegate1;

		// Token: 0x02000477 RID: 1143
		private enum ViewType
		{
			// Token: 0x04001C88 RID: 7304
			Static,
			// Token: 0x04001C89 RID: 7305
			Dynamic
		}

		// Token: 0x02000478 RID: 1144
		private class MenuDesignerActionList : DesignerActionList
		{
			// Token: 0x06002991 RID: 10641 RVA: 0x000E3C8E File Offset: 0x000E2C8E
			public MenuDesignerActionList(MenuDesigner parent)
				: base(parent.Component)
			{
				this._parent = parent;
			}

			// Token: 0x170007BD RID: 1981
			// (get) Token: 0x06002992 RID: 10642 RVA: 0x000E3CA3 File Offset: 0x000E2CA3
			// (set) Token: 0x06002993 RID: 10643 RVA: 0x000E3CA6 File Offset: 0x000E2CA6
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

			// Token: 0x170007BE RID: 1982
			// (get) Token: 0x06002994 RID: 10644 RVA: 0x000E3CA8 File Offset: 0x000E2CA8
			// (set) Token: 0x06002995 RID: 10645 RVA: 0x000E3CE0 File Offset: 0x000E2CE0
			[TypeConverter(typeof(MenuDesigner.MenuDesignerActionList.MenuViewTypeConverter))]
			public string View
			{
				get
				{
					if (this._parent._currentView == MenuDesigner.ViewType.Static)
					{
						return SR.GetString("Menu_StaticView");
					}
					if (this._parent._currentView == MenuDesigner.ViewType.Dynamic)
					{
						return SR.GetString("Menu_DynamicView");
					}
					return string.Empty;
				}
				set
				{
					if (string.Compare(value, SR.GetString("Menu_StaticView"), StringComparison.Ordinal) == 0)
					{
						this._parent._currentView = MenuDesigner.ViewType.Static;
					}
					else if (string.Compare(value, SR.GetString("Menu_DynamicView"), StringComparison.Ordinal) == 0)
					{
						this._parent._currentView = MenuDesigner.ViewType.Dynamic;
					}
					TypeDescriptor.Refresh(this._parent.Component);
					this._parent.UpdateDesignTimeHtml();
				}
			}

			// Token: 0x06002996 RID: 10646 RVA: 0x000E3D48 File Offset: 0x000E2D48
			public void ConvertToDynamicTemplate()
			{
				this._parent.ConvertToDynamicTemplate();
			}

			// Token: 0x06002997 RID: 10647 RVA: 0x000E3D55 File Offset: 0x000E2D55
			public void ResetDynamicTemplate()
			{
				this._parent.ResetDynamicTemplate();
			}

			// Token: 0x06002998 RID: 10648 RVA: 0x000E3D62 File Offset: 0x000E2D62
			public void ConvertToStaticTemplate()
			{
				this._parent.ConvertToStaticTemplate();
			}

			// Token: 0x06002999 RID: 10649 RVA: 0x000E3D6F File Offset: 0x000E2D6F
			public void ResetStaticTemplate()
			{
				this._parent.ResetStaticTemplate();
			}

			// Token: 0x0600299A RID: 10650 RVA: 0x000E3D7C File Offset: 0x000E2D7C
			public void EditBindings()
			{
				this._parent.EditBindings();
			}

			// Token: 0x0600299B RID: 10651 RVA: 0x000E3D89 File Offset: 0x000E2D89
			public void EditMenuItems()
			{
				this._parent.EditMenuItems();
			}

			// Token: 0x0600299C RID: 10652 RVA: 0x000E3D98 File Offset: 0x000E2D98
			public override DesignerActionItemCollection GetSortedActionItems()
			{
				DesignerActionItemCollection designerActionItemCollection = new DesignerActionItemCollection();
				string @string = SR.GetString("MenuDesigner_DataActionGroup");
				designerActionItemCollection.Add(new DesignerActionPropertyItem("View", SR.GetString("WebControls_Views"), @string, SR.GetString("MenuDesigner_ViewsDescription")));
				if (string.IsNullOrEmpty(this._parent.DataSourceID))
				{
					designerActionItemCollection.Add(new DesignerActionMethodItem(this, "EditMenuItems", SR.GetString("MenuDesigner_EditMenuItems"), @string, SR.GetString("MenuDesigner_EditMenuItemsDescription"), true));
				}
				else
				{
					designerActionItemCollection.Add(new DesignerActionMethodItem(this, "EditBindings", SR.GetString("MenuDesigner_EditBindings"), @string, SR.GetString("MenuDesigner_EditBindingsDescription"), true));
				}
				if (this._parent.DynamicTemplated)
				{
					designerActionItemCollection.Add(new DesignerActionMethodItem(this, "ResetDynamicTemplate", SR.GetString("MenuDesigner_ResetDynamicTemplate"), @string, SR.GetString("MenuDesigner_ResetDynamicTemplateDescription"), true));
				}
				else
				{
					designerActionItemCollection.Add(new DesignerActionMethodItem(this, "ConvertToDynamicTemplate", SR.GetString("MenuDesigner_ConvertToDynamicTemplate"), @string, SR.GetString("MenuDesigner_ConvertToDynamicTemplateDescription"), true));
				}
				if (this._parent.StaticTemplated)
				{
					designerActionItemCollection.Add(new DesignerActionMethodItem(this, "ResetStaticTemplate", SR.GetString("MenuDesigner_ResetStaticTemplate"), @string, SR.GetString("MenuDesigner_ResetStaticTemplateDescription"), true));
				}
				else
				{
					designerActionItemCollection.Add(new DesignerActionMethodItem(this, "ConvertToStaticTemplate", SR.GetString("MenuDesigner_ConvertToStaticTemplate"), @string, SR.GetString("MenuDesigner_ConvertToStaticTemplateDescription"), true));
				}
				return designerActionItemCollection;
			}

			// Token: 0x04001C8A RID: 7306
			private MenuDesigner _parent;

			// Token: 0x02000479 RID: 1145
			private class MenuViewTypeConverter : TypeConverter
			{
				// Token: 0x0600299D RID: 10653 RVA: 0x000E3F00 File Offset: 0x000E2F00
				public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
				{
					return new TypeConverter.StandardValuesCollection(new string[]
					{
						SR.GetString("Menu_StaticView"),
						SR.GetString("Menu_DynamicView")
					});
				}

				// Token: 0x0600299E RID: 10654 RVA: 0x000E3F34 File Offset: 0x000E2F34
				public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
				{
					return true;
				}

				// Token: 0x0600299F RID: 10655 RVA: 0x000E3F37 File Offset: 0x000E2F37
				public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
				{
					return true;
				}
			}
		}

		// Token: 0x0200047A RID: 1146
		private class MenuSampleData : IHierarchicalEnumerable, IEnumerable
		{
			// Token: 0x060029A1 RID: 10657 RVA: 0x000E3F44 File Offset: 0x000E2F44
			public MenuSampleData(global::System.Web.UI.WebControls.Menu menu, int depth, string path)
			{
				this._list = new ArrayList();
				this._menu = menu;
				int num = this._menu.StaticDisplayLevels + this._menu.MaximumDynamicDisplayLevels;
				if (num < this._menu.StaticDisplayLevels || num < this._menu.MaximumDynamicDisplayLevels)
				{
					num = int.MaxValue;
				}
				if (depth == 0)
				{
					this._list.Add(new MenuDesigner.MenuSampleDataNode(this._menu, SR.GetString("HierarchicalDataBoundControlDesigner_SampleRoot"), depth, path, false));
					this._list.Add(new MenuDesigner.MenuSampleDataNode(this._menu, SR.GetString("HierarchicalDataBoundControlDesigner_SampleRoot"), depth, path));
					this._list.Add(new MenuDesigner.MenuSampleDataNode(this._menu, SR.GetString("HierarchicalDataBoundControlDesigner_SampleRoot"), depth, path, false));
					this._list.Add(new MenuDesigner.MenuSampleDataNode(this._menu, SR.GetString("HierarchicalDataBoundControlDesigner_SampleRoot"), depth, path, false));
					this._list.Add(new MenuDesigner.MenuSampleDataNode(this._menu, SR.GetString("HierarchicalDataBoundControlDesigner_SampleRoot"), depth, path, false));
					return;
				}
				if (depth <= this._menu.StaticDisplayLevels && depth < 10)
				{
					this._list.Add(new MenuDesigner.MenuSampleDataNode(this._menu, SR.GetString("HierarchicalDataBoundControlDesigner_SampleParent", new object[] { depth }), depth, path));
					return;
				}
				if (depth < num && depth < 10)
				{
					this._list.Add(new MenuDesigner.MenuSampleDataNode(this._menu, SR.GetString("HierarchicalDataBoundControlDesigner_SampleLeaf", new object[] { 1 }), depth, path));
					this._list.Add(new MenuDesigner.MenuSampleDataNode(this._menu, SR.GetString("HierarchicalDataBoundControlDesigner_SampleLeaf", new object[] { 2 }), depth, path));
					this._list.Add(new MenuDesigner.MenuSampleDataNode(this._menu, SR.GetString("HierarchicalDataBoundControlDesigner_SampleLeaf", new object[] { 3 }), depth, path));
					this._list.Add(new MenuDesigner.MenuSampleDataNode(this._menu, SR.GetString("HierarchicalDataBoundControlDesigner_SampleLeaf", new object[] { 4 }), depth, path));
				}
			}

			// Token: 0x060029A2 RID: 10658 RVA: 0x000E418A File Offset: 0x000E318A
			public IEnumerator GetEnumerator()
			{
				return this._list.GetEnumerator();
			}

			// Token: 0x060029A3 RID: 10659 RVA: 0x000E4197 File Offset: 0x000E3197
			public IHierarchyData GetHierarchyData(object enumeratedItem)
			{
				return (IHierarchyData)enumeratedItem;
			}

			// Token: 0x04001C8B RID: 7307
			private ArrayList _list;

			// Token: 0x04001C8C RID: 7308
			private global::System.Web.UI.WebControls.Menu _menu;
		}

		// Token: 0x0200047B RID: 1147
		private class MenuSampleDataNode : IHierarchyData
		{
			// Token: 0x060029A4 RID: 10660 RVA: 0x000E419F File Offset: 0x000E319F
			public MenuSampleDataNode(global::System.Web.UI.WebControls.Menu menu, string text, int depth, string path)
				: this(menu, text, depth, path, true)
			{
			}

			// Token: 0x060029A5 RID: 10661 RVA: 0x000E41AD File Offset: 0x000E31AD
			public MenuSampleDataNode(global::System.Web.UI.WebControls.Menu menu, string text, int depth, string path, bool hasChildren)
			{
				this._text = text;
				this._depth = depth;
				this._path = path + '\\' + text;
				this._menu = menu;
				this._hasChildren = hasChildren;
			}

			// Token: 0x170007BF RID: 1983
			// (get) Token: 0x060029A6 RID: 10662 RVA: 0x000E41E8 File Offset: 0x000E31E8
			public bool HasChildren
			{
				get
				{
					if (!this._hasChildren)
					{
						return false;
					}
					int num = this._menu.StaticDisplayLevels + this._menu.MaximumDynamicDisplayLevels;
					if (num < this._menu.StaticDisplayLevels || num < this._menu.MaximumDynamicDisplayLevels)
					{
						num = int.MaxValue;
					}
					return this._depth < num && this._depth < 10;
				}
			}

			// Token: 0x170007C0 RID: 1984
			// (get) Token: 0x060029A7 RID: 10663 RVA: 0x000E424F File Offset: 0x000E324F
			public string Path
			{
				get
				{
					return this._path;
				}
			}

			// Token: 0x170007C1 RID: 1985
			// (get) Token: 0x060029A8 RID: 10664 RVA: 0x000E4257 File Offset: 0x000E3257
			public object Item
			{
				get
				{
					return this;
				}
			}

			// Token: 0x170007C2 RID: 1986
			// (get) Token: 0x060029A9 RID: 10665 RVA: 0x000E425A File Offset: 0x000E325A
			public string Type
			{
				get
				{
					return "SampleData";
				}
			}

			// Token: 0x060029AA RID: 10666 RVA: 0x000E4261 File Offset: 0x000E3261
			public override string ToString()
			{
				return this._text;
			}

			// Token: 0x060029AB RID: 10667 RVA: 0x000E4269 File Offset: 0x000E3269
			public IHierarchicalEnumerable GetChildren()
			{
				return new MenuDesigner.MenuSampleData(this._menu, this._depth + 1, this._path);
			}

			// Token: 0x060029AC RID: 10668 RVA: 0x000E4284 File Offset: 0x000E3284
			public IHierarchyData GetParent()
			{
				return null;
			}

			// Token: 0x04001C8D RID: 7309
			private string _text;

			// Token: 0x04001C8E RID: 7310
			private int _depth;

			// Token: 0x04001C8F RID: 7311
			private string _path;

			// Token: 0x04001C90 RID: 7312
			private global::System.Web.UI.WebControls.Menu _menu;

			// Token: 0x04001C91 RID: 7313
			private bool _hasChildren;
		}

		// Token: 0x0200047C RID: 1148
		private class MenuItemSchema : IDataSourceViewSchema
		{
			// Token: 0x060029AD RID: 10669 RVA: 0x000E4288 File Offset: 0x000E3288
			static MenuItemSchema()
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(global::System.Web.UI.WebControls.MenuItem));
				MenuDesigner.MenuItemSchema._fieldSchema = new IDataSourceFieldSchema[]
				{
					new TypeFieldSchema(properties["DataPath"]),
					new TypeFieldSchema(properties["Depth"]),
					new TypeFieldSchema(properties["Enabled"]),
					new TypeFieldSchema(properties["ImageUrl"]),
					new TypeFieldSchema(properties["NavigateUrl"]),
					new TypeFieldSchema(properties["PopOutImageUrl"]),
					new TypeFieldSchema(properties["Selectable"]),
					new TypeFieldSchema(properties["Selected"]),
					new TypeFieldSchema(properties["SeparatorImageUrl"]),
					new TypeFieldSchema(properties["Target"]),
					new TypeFieldSchema(properties["Text"]),
					new TypeFieldSchema(properties["ToolTip"]),
					new TypeFieldSchema(properties["Value"]),
					new TypeFieldSchema(properties["ValuePath"])
				};
			}

			// Token: 0x170007C3 RID: 1987
			// (get) Token: 0x060029AF RID: 10671 RVA: 0x000E43CA File Offset: 0x000E33CA
			string IDataSourceViewSchema.Name
			{
				get
				{
					return "MenuItem";
				}
			}

			// Token: 0x060029B0 RID: 10672 RVA: 0x000E43D1 File Offset: 0x000E33D1
			IDataSourceViewSchema[] IDataSourceViewSchema.GetChildren()
			{
				return new IDataSourceViewSchema[0];
			}

			// Token: 0x060029B1 RID: 10673 RVA: 0x000E43D9 File Offset: 0x000E33D9
			IDataSourceFieldSchema[] IDataSourceViewSchema.GetFields()
			{
				return MenuDesigner.MenuItemSchema._fieldSchema;
			}

			// Token: 0x04001C92 RID: 7314
			private static IDataSourceFieldSchema[] _fieldSchema;
		}
	}
}
