using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Design;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace System.Web.UI.Design.WebControls.WebParts
{
	// Token: 0x0200053F RID: 1343
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class EditorZoneDesigner : ToolZoneDesigner
	{
		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x06002F5E RID: 12126 RVA: 0x0010E3E2 File Offset: 0x0010D3E2
		public override DesignerAutoFormatCollection AutoFormats
		{
			get
			{
				if (EditorZoneDesigner._autoFormats == null)
				{
					EditorZoneDesigner._autoFormats = ControlDesigner.CreateAutoFormats("<Schemes>\r\n<xsd:schema id=\"Schemes\" xmlns=\"\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\">\r\n  <xsd:element name=\"Scheme\">\r\n     <xsd:complexType>\r\n       <xsd:all>\r\n        <xsd:element name=\"SchemeName\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"BackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"BorderColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"BorderWidth\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"EditUIStyle-Font-Names\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"EditUIStyle-Font-Size\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"EditUIStyle-ForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"EmptyZoneTextStyle-Font-Size\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"EmptyZoneTextStyle-ForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"ErrorStyle-Font-Size\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"Font-Names\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"FooterStyle-BackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"FooterStyle-HorizontalAlign\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"HeaderStyle-BackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"HeaderStyle-Font-Bold\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"HeaderStyle-Font-Size\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"HeaderStyle-Font--ClearDefaults\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"HeaderStyle-ForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"HeaderVerbStyle-Font-Bold\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"HeaderVerbStyle-Font-Size\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"HeaderVerbStyle-Font-Underline\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"HeaderVerbStyle-Font--ClearDefaults\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"HeaderVerbStyle-ForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"InstructionTextStyle-Font-Size\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"InstructionTextStyle-ForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"LabelStyle-Font-Size\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"LabelStyle-ForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"Padding\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"PartChromeStyle-BorderColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"PartChromeStyle-BorderStyle\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"PartChromeStyle-BorderWidth\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"PartStyle-BorderColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"PartStyle-BorderWidth\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"PartTitleStyle-Font-Bold\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"PartTitleStyle-Font-Size\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"PartTitleStyle-Font--ClearDefaults\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"PartTitleStyle-ForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"VerbStyle-Font-Names\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"VerbStyle-Font-Size\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"VerbStyle-ForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n      </xsd:all>\r\n    </xsd:complexType>\r\n  </xsd:element>\r\n  <xsd:element name=\"Schemes\" msdata:IsDataSet=\"true\">\r\n    <xsd:complexType>\r\n      <xsd:choice maxOccurs=\"unbounded\">\r\n        <xsd:element ref=\"Scheme\"/>\r\n      </xsd:choice>\r\n    </xsd:complexType>\r\n  </xsd:element>\r\n</xsd:schema>\r\n<Scheme>\r\n  <SchemeName>WebPartScheme_Empty</SchemeName>\r\n  <HeaderStyle-Font-Bold>False</HeaderStyle-Font-Bold>\r\n  <HeaderStyle-Font--ClearDefaults>True</HeaderStyle-Font--ClearDefaults>\r\n  <HeaderVerbStyle-Font-Bold>False</HeaderVerbStyle-Font-Bold>\r\n  <HeaderVerbStyle-Font-Underline>False</HeaderVerbStyle-Font-Underline>\r\n  <HeaderVerbStyle-Font--ClearDefaults>True</HeaderVerbStyle-Font--ClearDefaults>\r\n  <Padding>2</Padding>\r\n  <PartChromeStyle-BorderStyle>NotSet</PartChromeStyle-BorderStyle>\r\n  <PartTitleStyle-Font-Bold>False</PartTitleStyle-Font-Bold>\r\n  <PartTitleStyle-Font--ClearDefaults>True</PartTitleStyle-Font--ClearDefaults>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>WebPartScheme_Professional</SchemeName>\r\n  <BackColor>#F7F6F3</BackColor>\r\n  <BorderColor>#CCCCCC</BorderColor>\r\n  <BorderWidth>1px</BorderWidth>\r\n  <EditUIStyle-Font-Names>Verdana</EditUIStyle-Font-Names>\r\n  <EditUIStyle-Font-Size>0.8em</EditUIStyle-Font-Size>\r\n  <EditUIStyle-ForeColor>#333333</EditUIStyle-ForeColor>\r\n  <EmptyZoneTextStyle-Font-Size>0.8em</EmptyZoneTextStyle-Font-Size>\r\n  <EmptyZoneTextStyle-ForeColor>#333333</EmptyZoneTextStyle-ForeColor>\r\n  <ErrorStyle-Font-Size>0.8em</ErrorStyle-Font-Size>\r\n  <Font-Names>Verdana</Font-Names>\r\n  <FooterStyle-BackColor>#E2DED6</FooterStyle-BackColor>\r\n  <FooterStyle-HorizontalAlign>Right</FooterStyle-HorizontalAlign>\r\n  <HeaderStyle-BackColor>#E2DED6</HeaderStyle-BackColor>\r\n  <HeaderStyle-Font-Bold>True</HeaderStyle-Font-Bold>\r\n  <HeaderStyle-Font-Size>0.8em</HeaderStyle-Font-Size>\r\n  <HeaderStyle-ForeColor>#333333</HeaderStyle-ForeColor>\r\n  <HeaderVerbStyle-Font-Bold>False</HeaderVerbStyle-Font-Bold>\r\n  <HeaderVerbStyle-Font-Size>0.8em</HeaderVerbStyle-Font-Size>\r\n  <HeaderVerbStyle-Font-Underline>False</HeaderVerbStyle-Font-Underline>\r\n  <HeaderVerbStyle-ForeColor>#333333</HeaderVerbStyle-ForeColor>\r\n  <InstructionTextStyle-Font-Size>0.8em</InstructionTextStyle-Font-Size>\r\n  <InstructionTextStyle-ForeColor>#333333</InstructionTextStyle-ForeColor>\r\n  <LabelStyle-Font-Size>0.8em</LabelStyle-Font-Size>\r\n  <LabelStyle-ForeColor>#333333</LabelStyle-ForeColor>\r\n  <Padding>6</Padding>\r\n  <PartChromeStyle-BorderColor>#E2DED6</PartChromeStyle-BorderColor>\r\n  <PartChromeStyle-BorderStyle>Solid</PartChromeStyle-BorderStyle>\r\n  <PartChromeStyle-BorderWidth>1px</PartChromeStyle-BorderWidth>\r\n  <PartStyle-BorderColor>#F7F6F3</PartStyle-BorderColor>\r\n  <PartStyle-BorderWidth>5px</PartStyle-BorderWidth>\r\n  <PartTitleStyle-Font-Bold>True</PartTitleStyle-Font-Bold>\r\n  <PartTitleStyle-Font-Size>0.8em</PartTitleStyle-Font-Size>\r\n  <PartTitleStyle-ForeColor>#333333</PartTitleStyle-ForeColor>\r\n  <VerbStyle-Font-Names>Verdana</VerbStyle-Font-Names>\r\n  <VerbStyle-Font-Size>0.8em</VerbStyle-Font-Size>\r\n  <VerbStyle-ForeColor>#333333</VerbStyle-ForeColor>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>WebPartScheme_Simple</SchemeName>\r\n  <BackColor>#E3EAEB</BackColor>\r\n  <BorderColor>#CCCCCC</BorderColor>\r\n  <BorderWidth>1px</BorderWidth>\r\n  <EditUIStyle-Font-Names>Verdana</EditUIStyle-Font-Names>\r\n  <EditUIStyle-Font-Size>0.8em</EditUIStyle-Font-Size>\r\n  <EditUIStyle-ForeColor>#333333</EditUIStyle-ForeColor>\r\n  <EmptyZoneTextStyle-Font-Size>0.8em</EmptyZoneTextStyle-Font-Size>\r\n  <EmptyZoneTextStyle-ForeColor>#333333</EmptyZoneTextStyle-ForeColor>\r\n  <ErrorStyle-Font-Size>0.8em</ErrorStyle-Font-Size>\r\n  <Font-Names>Verdana</Font-Names>\r\n  <FooterStyle-BackColor>#C5BBAF</FooterStyle-BackColor>\r\n  <FooterStyle-HorizontalAlign>Right</FooterStyle-HorizontalAlign>\r\n  <HeaderStyle-BackColor>#C5BBAF</HeaderStyle-BackColor>\r\n  <HeaderStyle-Font-Bold>True</HeaderStyle-Font-Bold>\r\n  <HeaderStyle-Font-Size>0.8em</HeaderStyle-Font-Size>\r\n  <HeaderStyle-ForeColor>#333333</HeaderStyle-ForeColor>\r\n  <HeaderVerbStyle-Font-Bold>False</HeaderVerbStyle-Font-Bold>\r\n  <HeaderVerbStyle-Font-Size>0.8em</HeaderVerbStyle-Font-Size>\r\n  <HeaderVerbStyle-Font-Underline>False</HeaderVerbStyle-Font-Underline>\r\n  <HeaderVerbStyle-ForeColor>#333333</HeaderVerbStyle-ForeColor>\r\n  <InstructionTextStyle-Font-Size>0.8em</InstructionTextStyle-Font-Size>\r\n  <InstructionTextStyle-ForeColor>#333333</InstructionTextStyle-ForeColor>\r\n  <LabelStyle-Font-Size>0.8em</LabelStyle-Font-Size>\r\n  <LabelStyle-ForeColor>#333333</LabelStyle-ForeColor>\r\n  <Padding>6</Padding>\r\n  <PartChromeStyle-BorderColor>#C5BBAF</PartChromeStyle-BorderColor>\r\n  <PartChromeStyle-BorderStyle>Solid</PartChromeStyle-BorderStyle>\r\n  <PartChromeStyle-BorderWidth>1px</PartChromeStyle-BorderWidth>\r\n  <PartStyle-BorderColor>#E3EAEB</PartStyle-BorderColor>\r\n  <PartStyle-BorderWidth>5px</PartStyle-BorderWidth>\r\n  <PartTitleStyle-Font-Bold>True</PartTitleStyle-Font-Bold>\r\n  <PartTitleStyle-Font-Size>0.8em</PartTitleStyle-Font-Size>\r\n  <PartTitleStyle-ForeColor>#333333</PartTitleStyle-ForeColor>\r\n  <VerbStyle-Font-Names>Verdana</VerbStyle-Font-Names>\r\n  <VerbStyle-Font-Size>0.8em</VerbStyle-Font-Size>\r\n  <VerbStyle-ForeColor>#333333</VerbStyle-ForeColor>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>WebPartScheme_Classic</SchemeName>\r\n  <BackColor>#EFF3FB</BackColor>\r\n  <BorderColor>#CCCCCC</BorderColor>\r\n  <BorderWidth>1px</BorderWidth>\r\n  <EditUIStyle-Font-Names>Verdana</EditUIStyle-Font-Names>\r\n  <EditUIStyle-Font-Size>0.8em</EditUIStyle-Font-Size>\r\n  <EditUIStyle-ForeColor>#333333</EditUIStyle-ForeColor>\r\n  <EmptyZoneTextStyle-Font-Size>0.8em</EmptyZoneTextStyle-Font-Size>\r\n  <EmptyZoneTextStyle-ForeColor>#333333</EmptyZoneTextStyle-ForeColor>\r\n  <ErrorStyle-Font-Size>0.8em</ErrorStyle-Font-Size>\r\n  <Font-Names>Verdana</Font-Names>\r\n  <FooterStyle-BackColor>#D1DDF1</FooterStyle-BackColor>\r\n  <FooterStyle-HorizontalAlign>Right</FooterStyle-HorizontalAlign>\r\n  <HeaderStyle-BackColor>#D1DDF1</HeaderStyle-BackColor>\r\n  <HeaderStyle-Font-Bold>True</HeaderStyle-Font-Bold>\r\n  <HeaderStyle-Font-Size>0.8em</HeaderStyle-Font-Size>\r\n  <HeaderStyle-ForeColor>#333333</HeaderStyle-ForeColor>\r\n  <HeaderVerbStyle-Font-Bold>False</HeaderVerbStyle-Font-Bold>\r\n  <HeaderVerbStyle-Font-Size>0.8em</HeaderVerbStyle-Font-Size>\r\n  <HeaderVerbStyle-Font-Underline>False</HeaderVerbStyle-Font-Underline>\r\n  <HeaderVerbStyle-ForeColor>#333333</HeaderVerbStyle-ForeColor>\r\n  <InstructionTextStyle-Font-Size>0.8em</InstructionTextStyle-Font-Size>\r\n  <InstructionTextStyle-ForeColor>#333333</InstructionTextStyle-ForeColor>\r\n  <LabelStyle-Font-Size>0.8em</LabelStyle-Font-Size>\r\n  <LabelStyle-ForeColor>#333333</LabelStyle-ForeColor>\r\n  <Padding>6</Padding>\r\n  <PartChromeStyle-BorderColor>#D1DDF1</PartChromeStyle-BorderColor>\r\n  <PartChromeStyle-BorderStyle>Solid</PartChromeStyle-BorderStyle>\r\n  <PartChromeStyle-BorderWidth>1px</PartChromeStyle-BorderWidth>\r\n  <PartStyle-BorderColor>#EFF3FB</PartStyle-BorderColor>\r\n  <PartStyle-BorderWidth>5px</PartStyle-BorderWidth>\r\n  <PartTitleStyle-Font-Bold>True</PartTitleStyle-Font-Bold>\r\n  <PartTitleStyle-Font-Size>0.8em</PartTitleStyle-Font-Size>\r\n  <PartTitleStyle-ForeColor>#333333</PartTitleStyle-ForeColor>\r\n  <VerbStyle-Font-Names>Verdana</VerbStyle-Font-Names>\r\n  <VerbStyle-Font-Size>0.8em</VerbStyle-Font-Size>\r\n  <VerbStyle-ForeColor>#333333</VerbStyle-ForeColor>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>WebPartScheme_Colorful</SchemeName>\r\n  <BackColor>#FFFBD6</BackColor>\r\n  <BorderColor>#CCCCCC</BorderColor>\r\n  <BorderWidth>1px</BorderWidth>\r\n  <EditUIStyle-Font-Names>Verdana</EditUIStyle-Font-Names>\r\n  <EditUIStyle-Font-Size>0.8em</EditUIStyle-Font-Size>\r\n  <EditUIStyle-ForeColor>#333333</EditUIStyle-ForeColor>\r\n  <EmptyZoneTextStyle-Font-Size>0.8em</EmptyZoneTextStyle-Font-Size>\r\n  <EmptyZoneTextStyle-ForeColor>#333333</EmptyZoneTextStyle-ForeColor>\r\n  <ErrorStyle-Font-Size>0.8em</ErrorStyle-Font-Size>\r\n  <Font-Names>Verdana</Font-Names>\r\n  <FooterStyle-BackColor>#FFCC66</FooterStyle-BackColor>\r\n  <FooterStyle-HorizontalAlign>Right</FooterStyle-HorizontalAlign>\r\n  <HeaderStyle-BackColor>#FFCC66</HeaderStyle-BackColor>\r\n  <HeaderStyle-Font-Bold>True</HeaderStyle-Font-Bold>\r\n  <HeaderStyle-Font-Size>0.8em</HeaderStyle-Font-Size>\r\n  <HeaderStyle-ForeColor>#333333</HeaderStyle-ForeColor>\r\n  <HeaderVerbStyle-Font-Bold>False</HeaderVerbStyle-Font-Bold>\r\n  <HeaderVerbStyle-Font-Size>0.8em</HeaderVerbStyle-Font-Size>\r\n  <HeaderVerbStyle-Font-Underline>False</HeaderVerbStyle-Font-Underline>\r\n  <HeaderVerbStyle-ForeColor>#333333</HeaderVerbStyle-ForeColor>\r\n  <InstructionTextStyle-Font-Size>0.8em</InstructionTextStyle-Font-Size>\r\n  <InstructionTextStyle-ForeColor>#333333</InstructionTextStyle-ForeColor>\r\n  <LabelStyle-Font-Size>0.8em</LabelStyle-Font-Size>\r\n  <LabelStyle-ForeColor>#333333</LabelStyle-ForeColor>\r\n  <Padding>6</Padding>\r\n  <PartChromeStyle-BorderColor>#FFCC66</PartChromeStyle-BorderColor>\r\n  <PartChromeStyle-BorderStyle>Solid</PartChromeStyle-BorderStyle>\r\n  <PartChromeStyle-BorderWidth>1px</PartChromeStyle-BorderWidth>\r\n  <PartStyle-BorderColor>#FFFBD6</PartStyle-BorderColor>\r\n  <PartStyle-BorderWidth>5px</PartStyle-BorderWidth>\r\n  <PartTitleStyle-Font-Bold>True</PartTitleStyle-Font-Bold>\r\n  <PartTitleStyle-Font-Size>0.8em</PartTitleStyle-Font-Size>\r\n  <PartTitleStyle-ForeColor>#333333</PartTitleStyle-ForeColor>\r\n  <VerbStyle-Font-Names>Verdana</VerbStyle-Font-Names>\r\n  <VerbStyle-Font-Size>0.8em</VerbStyle-Font-Size>\r\n  <VerbStyle-ForeColor>#333333</VerbStyle-ForeColor>\r\n</Scheme>\r\n</Schemes>\r\n", (DataRow schemeData) => new EditorZoneAutoFormat(schemeData));
				}
				return EditorZoneDesigner._autoFormats;
			}
		}

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x06002F5F RID: 12127 RVA: 0x0010E41C File Offset: 0x0010D41C
		public override TemplateGroupCollection TemplateGroups
		{
			get
			{
				TemplateGroupCollection templateGroups = base.TemplateGroups;
				if (this._templateGroup == null)
				{
					this._templateGroup = base.CreateZoneTemplateGroup();
				}
				templateGroups.Add(this._templateGroup);
				return templateGroups;
			}
		}

		// Token: 0x06002F60 RID: 12128 RVA: 0x0010E452 File Offset: 0x0010D452
		public override string GetDesignTimeHtml()
		{
			return this.GetDesignTimeHtml(null);
		}

		// Token: 0x06002F61 RID: 12129 RVA: 0x0010E45C File Offset: 0x0010D45C
		public override string GetDesignTimeHtml(DesignerRegionCollection regions)
		{
			string text;
			try
			{
				EditorZone editorZone = (EditorZone)base.ViewControl;
				bool flag = base.UseRegions(regions, this._zone.ZoneTemplate, editorZone.ZoneTemplate);
				if (editorZone.ZoneTemplate == null && !flag)
				{
					text = this.GetEmptyDesignTimeHtml();
				}
				else
				{
					((ICompositeControlDesignerAccessor)editorZone).RecreateChildControls();
					if (regions != null && flag)
					{
						editorZone.Controls.Clear();
						EditorZoneDesigner.EditorPartEditableDesignerRegion editorPartEditableDesignerRegion = new EditorZoneDesigner.EditorPartEditableDesignerRegion(editorZone, base.TemplateDefinition);
						editorPartEditableDesignerRegion.Properties[typeof(Control)] = editorZone;
						editorPartEditableDesignerRegion.IsSingleInstanceTemplate = true;
						editorPartEditableDesignerRegion.Description = SR.GetString("ContainerControlDesigner_RegionWatermark");
						regions.Add(editorPartEditableDesignerRegion);
					}
					text = base.GetDesignTimeHtml();
				}
				if (base.ViewInBrowseMode && editorZone.ID != "AutoFormatPreviewControl")
				{
					text = base.CreatePlaceHolderDesignTimeHtml();
				}
			}
			catch (Exception ex)
			{
				text = this.GetErrorDesignTimeHtml(ex);
			}
			return text;
		}

		// Token: 0x06002F62 RID: 12130 RVA: 0x0010E544 File Offset: 0x0010D544
		public override string GetEditableDesignerRegionContent(EditableDesignerRegion region)
		{
			return ControlPersister.PersistTemplate(this._zone.ZoneTemplate, (IDesignerHost)base.Component.Site.GetService(typeof(IDesignerHost)));
		}

		// Token: 0x06002F63 RID: 12131 RVA: 0x0010E575 File Offset: 0x0010D575
		protected override string GetEmptyDesignTimeHtml()
		{
			return base.CreatePlaceHolderDesignTimeHtml(SR.GetString("EditorZoneDesigner_Empty"));
		}

		// Token: 0x06002F64 RID: 12132 RVA: 0x0010E587 File Offset: 0x0010D587
		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(EditorZone));
			base.Initialize(component);
			this._zone = (EditorZone)component;
		}

		// Token: 0x06002F65 RID: 12133 RVA: 0x0010E5AC File Offset: 0x0010D5AC
		public override void SetEditableDesignerRegionContent(EditableDesignerRegion region, string content)
		{
			this._zone.ZoneTemplate = ControlParser.ParseTemplate((IDesignerHost)base.Component.Site.GetService(typeof(IDesignerHost)), content);
			base.IsDirtyInternal = true;
		}

		// Token: 0x04002041 RID: 8257
		private static DesignerAutoFormatCollection _autoFormats;

		// Token: 0x04002042 RID: 8258
		private EditorZone _zone;

		// Token: 0x04002043 RID: 8259
		private TemplateGroup _templateGroup;

		// Token: 0x04002044 RID: 8260
		[CompilerGenerated]
		private static ControlDesigner.CreateAutoFormatDelegate <>9__CachedAnonymousMethodDelegate1;

		// Token: 0x02000540 RID: 1344
		private sealed class EditorPartEditableDesignerRegion : TemplatedEditableDesignerRegion
		{
			// Token: 0x06002F68 RID: 12136 RVA: 0x0010E5ED File Offset: 0x0010D5ED
			public EditorPartEditableDesignerRegion(EditorZone zone, TemplateDefinition templateDefinition)
				: base(templateDefinition)
			{
				this._zone = zone;
			}

			// Token: 0x06002F69 RID: 12137 RVA: 0x0010E600 File Offset: 0x0010D600
			public override ViewRendering GetChildViewRendering(Control control)
			{
				if (control == null)
				{
					throw new ArgumentNullException("control");
				}
				DesignerEditorPartChrome designerEditorPartChrome = new DesignerEditorPartChrome(this._zone);
				return designerEditorPartChrome.GetViewRendering(control);
			}

			// Token: 0x04002045 RID: 8261
			private EditorZone _zone;
		}
	}
}
