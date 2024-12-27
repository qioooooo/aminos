using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Design;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x0200044E RID: 1102
	public class FormViewDesigner : DataBoundControlDesigner
	{
		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x0600280E RID: 10254 RVA: 0x000DB9D0 File Offset: 0x000DA9D0
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection designerActionListCollection = new DesignerActionListCollection();
				designerActionListCollection.AddRange(base.ActionLists);
				if (this._actionLists == null)
				{
					this._actionLists = new FormViewActionList(this);
				}
				bool inTemplateMode = base.InTemplateMode;
				DesignerDataSourceView designerView = base.DesignerView;
				this._actionLists.AllowPaging = !inTemplateMode && designerView != null;
				this._actionLists.AllowDynamicData = this.CheckDynamicDataAllowed();
				designerActionListCollection.Add(this._actionLists);
				return designerActionListCollection;
			}
		}

		// Token: 0x0600280F RID: 10255 RVA: 0x000DBA48 File Offset: 0x000DAA48
		private bool CheckDynamicDataAllowed()
		{
			IDesignerHost designerHost = (IDesignerHost)base.Component.Site.GetService(typeof(IDesignerHost));
			WebFormsRootDesigner webFormsRootDesigner = designerHost.GetDesigner(designerHost.RootComponent) as WebFormsRootDesigner;
			if (webFormsRootDesigner != null)
			{
				WebFormsReferenceManager referenceManager = webFormsRootDesigner.ReferenceManager;
				return referenceManager.GetType("asp", "DynamicControl") != null;
			}
			return false;
		}

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06002810 RID: 10256 RVA: 0x000DBAB1 File Offset: 0x000DAAB1
		public override DesignerAutoFormatCollection AutoFormats
		{
			get
			{
				if (FormViewDesigner._autoFormats == null)
				{
					FormViewDesigner._autoFormats = ControlDesigner.CreateAutoFormats("<Schemes>\r\n        <xsd:schema id=\"Schemes\" xmlns=\"\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\">\r\n          <xsd:element name=\"Scheme\">\r\n            <xsd:complexType>\r\n              <xsd:all>\r\n                <xsd:element name=\"SchemeName\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"ForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"BackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"BorderColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"BorderWidth\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"BorderStyle\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"GridLines\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"CellPadding\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"CellSpacing\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"RowForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"RowBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"RowFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"EditRowForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"EditRowBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"EditRowFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"HeaderForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"HeaderBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"HeaderFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"FooterForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"FooterBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"FooterFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"PagerForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"PagerBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"PagerFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"PagerAlign\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"PagerButtons\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n              </xsd:all>\r\n            </xsd:complexType>\r\n          </xsd:element>\r\n          <xsd:element name=\"Schemes\" msdata:IsDataSet=\"true\">\r\n            <xsd:complexType>\r\n              <xsd:choice maxOccurs=\"unbounded\">\r\n                <xsd:element ref=\"Scheme\"/>\r\n              </xsd:choice>\r\n            </xsd:complexType>\r\n          </xsd:element>\r\n        </xsd:schema>\r\n        <Scheme>\r\n          <SchemeName>FVScheme_Empty</SchemeName>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>DVScheme_Consistent1</SchemeName>\r\n          <AltRowBackColor>White</AltRowBackColor>\r\n          <GridLines>0</GridLines>\r\n          <CellPadding>4</CellPadding>\r\n          <ForeColor>#333333</ForeColor>\r\n          <RowForeColor>#333333</RowForeColor>\r\n          <RowBackColor>#FFFBD6</RowBackColor>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>#990000</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>White</FooterForeColor>\r\n          <FooterBackColor>#990000</FooterBackColor>\r\n          <FooterFont>1</FooterFont>\r\n          <PagerForeColor>#333333</PagerForeColor>\r\n          <PagerBackColor>#FFCC66</PagerBackColor>\r\n          <PagerAlign>2</PagerAlign>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>DVScheme_Consistent2</SchemeName>\r\n            <AltRowBackColor>White</AltRowBackColor>\r\n            <GridLines>0</GridLines>\r\n            <CellPadding>4</CellPadding>\r\n            <ForeColor>#333333</ForeColor>\r\n            <RowBackColor>#EFF3FB</RowBackColor>\r\n            <HeaderForeColor>White</HeaderForeColor>\r\n            <HeaderBackColor>#507CD1</HeaderBackColor>\r\n            <HeaderFont>1</HeaderFont>\r\n            <FooterForeColor>White</FooterForeColor>\r\n            <FooterBackColor>#507CD1</FooterBackColor>\r\n            <FooterFont>1</FooterFont>\r\n            <PagerForeColor>White</PagerForeColor>\r\n            <PagerBackColor>#2461BF</PagerBackColor>\r\n            <PagerAlign>2</PagerAlign>\r\n            <EditRowBackColor>#2461BF</EditRowBackColor>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>DVScheme_Consistent3</SchemeName>\r\n            <AltRowBackColor>White</AltRowBackColor>\r\n            <GridLines>0</GridLines>\r\n            <CellPadding>4</CellPadding>\r\n            <ForeColor>#333333</ForeColor>\r\n            <RowBackColor>#E3EAEB</RowBackColor>\r\n            <HeaderForeColor>White</HeaderForeColor>\r\n            <HeaderBackColor>#1C5E55</HeaderBackColor>\r\n            <HeaderFont>1</HeaderFont>\r\n            <FooterForeColor>White</FooterForeColor>\r\n            <FooterBackColor>#1C5E55</FooterBackColor>\r\n            <FooterFont>1</FooterFont>\r\n            <PagerForeColor>White</PagerForeColor>\r\n            <PagerBackColor>#666666</PagerBackColor>\r\n            <PagerAlign>2</PagerAlign>\r\n            <EditRowBackColor>#7C6F57</EditRowBackColor>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>DVScheme_Consistent4</SchemeName>\r\n            <AltRowBackColor>White</AltRowBackColor>\r\n            <AltRowForeColor>#284775</AltRowForeColor>\r\n            <GridLines>0</GridLines>\r\n            <CellPadding>4</CellPadding>\r\n            <ForeColor>#333333</ForeColor>\r\n            <RowForeColor>#333333</RowForeColor>\r\n            <RowBackColor>#F7F6F3</RowBackColor>\r\n            <HeaderForeColor>White</HeaderForeColor>\r\n            <HeaderBackColor>#5D7B9D</HeaderBackColor>\r\n            <HeaderFont>1</HeaderFont>\r\n            <FooterForeColor>White</FooterForeColor>\r\n            <FooterBackColor>#5D7B9D</FooterBackColor>\r\n            <FooterFont>1</FooterFont>\r\n            <PagerForeColor>White</PagerForeColor>\r\n            <PagerBackColor>#284775</PagerBackColor>\r\n            <PagerAlign>2</PagerAlign>\r\n            <EditRowBackColor>#999999</EditRowBackColor>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>FVScheme_Colorful1</SchemeName>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#CC9966</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>1</BorderStyle>\r\n          <GridLines>3</GridLines>\r\n          <CellPadding>4</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <RowForeColor>#330099</RowForeColor>\r\n          <RowBackColor>White</RowBackColor>\r\n          <EditRowForeColor>#663399</EditRowForeColor>\r\n          <EditRowBackColor>#FFCC66</EditRowBackColor>\r\n          <EditRowFont>1</EditRowFont>\r\n          <HeaderForeColor>#FFFFCC</HeaderForeColor>\r\n          <HeaderBackColor>#990000</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>#330099</FooterForeColor>\r\n          <FooterBackColor>#FFFFCC</FooterBackColor>\r\n          <PagerForeColor>#330099</PagerForeColor>\r\n          <PagerBackColor>#FFFFCC</PagerBackColor>\r\n          <PagerAlign>2</PagerAlign>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>FVScheme_Colorful2</SchemeName>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#3366CC</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>1</BorderStyle>\r\n          <GridLines>3</GridLines>\r\n          <CellPadding>4</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <RowForeColor>#003399</RowForeColor>\r\n          <RowBackColor>White</RowBackColor>\r\n          <EditRowForeColor>#CCFF99</EditRowForeColor>\r\n          <EditRowBackColor>#009999</EditRowBackColor>\r\n          <EditRowFont>1</EditRowFont>\r\n          <HeaderForeColor>#CCCCFF</HeaderForeColor>\r\n          <HeaderBackColor>#003399</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>#003399</FooterForeColor>\r\n          <FooterBackColor>#99CCCC</FooterBackColor>\r\n          <PagerForeColor>#003399</PagerForeColor>\r\n          <PagerBackColor>#99CCCC</PagerBackColor>\r\n          <PagerAlign>1</PagerAlign>\r\n          <PagerButtons>1</PagerButtons>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>FVScheme_Colorful3</SchemeName>\r\n          <BackColor>#DEBA84</BackColor>\r\n          <BorderColor>#DEBA84</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>1</BorderStyle>\r\n          <GridLines>3</GridLines>\r\n          <CellPadding>3</CellPadding>\r\n          <CellSpacing>2</CellSpacing>\r\n          <RowForeColor>#8C4510</RowForeColor>\r\n          <RowBackColor>#FFF7E7</RowBackColor>\r\n          <EditRowForeColor>White</EditRowForeColor>\r\n          <EditRowBackColor>#738A9C</EditRowBackColor>\r\n          <EditRowFont>1</EditRowFont>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>#A55129</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>#8C4510</FooterForeColor>\r\n          <FooterBackColor>#F7DFB5</FooterBackColor>\r\n          <PagerForeColor>#8C4510</PagerForeColor>\r\n          <PagerAlign>2</PagerAlign>\r\n          <PagerButtons>1</PagerButtons>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>FVScheme_Colorful4</SchemeName>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#E7E7FF</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>1</BorderStyle>\r\n          <GridLines>1</GridLines>\r\n          <CellPadding>3</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <RowForeColor>#4A3C8C</RowForeColor>\r\n          <RowBackColor>#E7E7FF</RowBackColor>\r\n          <EditRowForeColor>#F7F7F7</EditRowForeColor>\r\n          <EditRowBackColor>#738A9C</EditRowBackColor>\r\n          <EditRowFont>1</EditRowFont>\r\n          <HeaderForeColor>#F7F7F7</HeaderForeColor>\r\n          <HeaderBackColor>#4A3C8C</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>#4A3C8C</FooterForeColor>\r\n          <FooterBackColor>#B5C7DE</FooterBackColor>\r\n          <PagerForeColor>#4A3C8C</PagerForeColor>\r\n          <PagerBackColor>#E7E7FF</PagerBackColor>\r\n          <PagerAlign>3</PagerAlign>\r\n          <PagerButtons>1</PagerButtons>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>FVScheme_Colorful5</SchemeName>\r\n          <ForeColor>Black</ForeColor>\r\n          <BackColor>LightGoldenRodYellow</BackColor>\r\n          <BorderColor>Tan</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <GridLines>0</GridLines>\r\n          <CellPadding>2</CellPadding>\r\n          <HeaderBackColor>Tan</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterBackColor>Tan</FooterBackColor>\r\n          <EditRowBackColor>DarkSlateBlue</EditRowBackColor>\r\n          <EditRowForeColor>GhostWhite</EditRowForeColor>\r\n          <PagerBackColor>PaleGoldenrod</PagerBackColor>\r\n          <PagerForeColor>DarkSlateBlue</PagerForeColor>\r\n          <PagerAlign>2</PagerAlign>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>FVScheme_Professional1</SchemeName>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#999999</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>1</BorderStyle>\r\n          <GridLines>2</GridLines>\r\n          <CellPadding>3</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <RowForeColor>Black</RowForeColor>\r\n          <RowBackColor>#EEEEEE</RowBackColor>\r\n          <EditRowForeColor>White</EditRowForeColor>\r\n          <EditRowBackColor>#008A8C</EditRowBackColor>\r\n          <EditRowFont>1</EditRowFont>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>#000084</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>Black</FooterForeColor>\r\n          <FooterBackColor>#CCCCCC</FooterBackColor>\r\n          <PagerForeColor>Black</PagerForeColor>\r\n          <PagerBackColor>#999999</PagerBackColor>\r\n          <PagerAlign>2</PagerAlign>\r\n          <PagerButtons>1</PagerButtons>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>FVScheme_Professional2</SchemeName>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#CCCCCC</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>1</BorderStyle>\r\n          <GridLines>3</GridLines>\r\n          <CellPadding>3</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <RowForeColor>#000066</RowForeColor>\r\n          <EditRowForeColor>White</EditRowForeColor>\r\n          <EditRowBackColor>#669999</EditRowBackColor>\r\n          <EditRowFont>1</EditRowFont>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>#006699</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>#000066</FooterForeColor>\r\n          <FooterBackColor>White</FooterBackColor>\r\n          <PagerForeColor>#000066</PagerForeColor>\r\n          <PagerBackColor>White</PagerBackColor>\r\n          <PagerAlign>1</PagerAlign>\r\n          <PagerButtons>1</PagerButtons>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>FVScheme_Professional3</SchemeName>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>White</BorderColor>\r\n          <BorderWidth>2px</BorderWidth>\r\n          <BorderStyle>7</BorderStyle>\r\n          <GridLines>0</GridLines>\r\n          <CellPadding>3</CellPadding>\r\n          <CellSpacing>1</CellSpacing>\r\n          <RowForeColor>Black</RowForeColor>\r\n          <RowBackColor>#DEDFDE</RowBackColor>\r\n          <EditRowForeColor>White</EditRowForeColor>\r\n          <EditRowBackColor>#9471DE</EditRowBackColor>\r\n          <EditRowFont>1</EditRowFont>\r\n          <HeaderForeColor>#E7E7FF</HeaderForeColor>\r\n          <HeaderBackColor>#4A3C8C</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>Black</FooterForeColor>\r\n          <FooterBackColor>#C6C3C6</FooterBackColor>\r\n          <PagerForeColor>Black</PagerForeColor>\r\n          <PagerBackColor>#C6C3C6</PagerBackColor>\r\n          <PagerAlign>3</PagerAlign>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>FVScheme_Simple1</SchemeName>\r\n          <ForeColor>Black</ForeColor>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#999999</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>4</BorderStyle>\r\n          <GridLines>2</GridLines>\r\n          <CellPadding>3</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <EditRowForeColor>White</EditRowForeColor>\r\n          <EditRowBackColor>#000099</EditRowBackColor>\r\n          <EditRowFont>1</EditRowFont>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>Black</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterBackColor>#CCCCCC</FooterBackColor>\r\n          <PagerForeColor>Black</PagerForeColor>\r\n          <PagerBackColor>#999999</PagerBackColor>\r\n          <PagerAlign>2</PagerAlign>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>FVScheme_Simple2</SchemeName>\r\n          <ForeColor>Black</ForeColor>\r\n          <BackColor>#CCCCCC</BackColor>\r\n          <BorderColor>#999999</BorderColor>\r\n          <BorderWidth>3px</BorderWidth>\r\n          <BorderStyle>4</BorderStyle>\r\n          <GridLines>3</GridLines>\r\n          <CellPadding>4</CellPadding>\r\n          <CellSpacing>2</CellSpacing>\r\n          <RowBackColor>White</RowBackColor>\r\n          <EditRowForeColor>White</EditRowForeColor>\r\n          <EditRowBackColor>#000099</EditRowBackColor>\r\n          <EditRowFont>1</EditRowFont>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>Black</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterBackColor>#CCCCCC</FooterBackColor>\r\n          <PagerForeColor>Black</PagerForeColor>\r\n          <PagerBackColor>#CCCCCC</PagerBackColor>\r\n          <PagerAlign>1</PagerAlign>\r\n          <PagerButtons>1</PagerButtons>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>FVScheme_Simple3</SchemeName>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#336666</BorderColor>\r\n          <BorderWidth>3px</BorderWidth>\r\n          <BorderStyle>5</BorderStyle>\r\n          <GridLines>1</GridLines>\r\n          <CellPadding>4</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <RowForeColor>#333333</RowForeColor>\r\n          <RowBackColor>White</RowBackColor>\r\n          <EditRowForeColor>White</EditRowForeColor>\r\n          <EditRowBackColor>#339966</EditRowBackColor>\r\n          <EditRowFont>1</EditRowFont>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>#336666</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>#333333</FooterForeColor>\r\n          <FooterBackColor>White</FooterBackColor>\r\n          <PagerForeColor>White</PagerForeColor>\r\n          <PagerBackColor>#336666</PagerBackColor>\r\n          <PagerAlign>2</PagerAlign>\r\n          <PagerButtons>1</PagerButtons>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>FVScheme_Classic1</SchemeName>\r\n          <ForeColor>Black</ForeColor>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#CCCCCC</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>1</BorderStyle>\r\n          <GridLines>1</GridLines>\r\n          <CellPadding>4</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <EditRowForeColor>White</EditRowForeColor>\r\n          <EditRowBackColor>#CC3333</EditRowBackColor>\r\n          <EditRowFont>1</EditRowFont>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>#333333</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>Black</FooterForeColor>\r\n          <FooterBackColor>#CCCC99</FooterBackColor>\r\n          <PagerForeColor>Black</PagerForeColor>\r\n          <PagerBackColor>White</PagerBackColor>\r\n          <PagerAlign>3</PagerAlign>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>FVScheme_Classic2</SchemeName>\r\n          <ForeColor>Black</ForeColor>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#DEDFDE</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>1</BorderStyle>\r\n          <GridLines>2</GridLines>\r\n          <CellPadding>4</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <RowBackColor>#F7F7DE</RowBackColor>\r\n          <EditRowForeColor>White</EditRowForeColor>\r\n          <EditRowBackColor>#CE5D5A</EditRowBackColor>\r\n          <EditRowFont>1</EditRowFont>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>#6B696B</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterBackColor>#CCCC99</FooterBackColor>\r\n          <PagerForeColor>Black</PagerForeColor>\r\n          <PagerBackColor>#F7F7DE</PagerBackColor>\r\n          <PagerAlign>3</PagerAlign>\r\n          <PagerButtons>1</PagerButtons>\r\n        </Scheme>\r\n      </Schemes>", (DataRow schemeData) => new FormViewAutoFormat(schemeData));
				}
				return FormViewDesigner._autoFormats;
			}
		}

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06002811 RID: 10257 RVA: 0x000DBAEC File Offset: 0x000DAAEC
		private bool CurrentModeTemplateExists
		{
			get
			{
				ITemplate template = null;
				if (((FormView)base.ViewControl).CurrentMode == FormViewMode.ReadOnly)
				{
					template = ((FormView)base.ViewControl).ItemTemplate;
				}
				if (((FormView)base.ViewControl).CurrentMode == FormViewMode.Insert)
				{
					template = ((FormView)base.ViewControl).InsertItemTemplate;
				}
				if (((FormView)base.ViewControl).CurrentMode == FormViewMode.Edit || (((FormView)base.ViewControl).CurrentMode == FormViewMode.Insert && template == null))
				{
					template = ((FormView)base.ViewControl).EditItemTemplate;
				}
				if (template != null)
				{
					IDesignerHost designerHost = (IDesignerHost)base.ViewControl.Site.GetService(typeof(IDesignerHost));
					string text = ControlPersister.PersistTemplate(template, designerHost);
					return text != null && text.Length > 0;
				}
				return false;
			}
		}

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06002812 RID: 10258 RVA: 0x000DBBB7 File Offset: 0x000DABB7
		// (set) Token: 0x06002813 RID: 10259 RVA: 0x000DBBC0 File Offset: 0x000DABC0
		internal bool EnableDynamicData
		{
			get
			{
				return this._enableDynamicData;
			}
			set
			{
				this._enableDynamicData = value;
				IDataSourceViewSchema dataSourceSchema = this.GetDataSourceSchema();
				if (dataSourceSchema != null)
				{
					this.OnSchemaRefreshed();
				}
			}
		}

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06002814 RID: 10260 RVA: 0x000DBBE4 File Offset: 0x000DABE4
		// (set) Token: 0x06002815 RID: 10261 RVA: 0x000DBBF8 File Offset: 0x000DABF8
		internal bool EnablePaging
		{
			get
			{
				return ((FormView)base.Component).AllowPaging;
			}
			set
			{
				Cursor cursor = Cursor.Current;
				try
				{
					Cursor.Current = Cursors.WaitCursor;
					ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.EnablePagingCallback), value, SR.GetString("FormView_EnablePagingTransaction"));
				}
				finally
				{
					Cursor.Current = cursor;
				}
			}
		}

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06002816 RID: 10262 RVA: 0x000DBC58 File Offset: 0x000DAC58
		protected override int SampleRowCount
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06002817 RID: 10263 RVA: 0x000DBC5C File Offset: 0x000DAC5C
		public override TemplateGroupCollection TemplateGroups
		{
			get
			{
				TemplateGroupCollection templateGroups = base.TemplateGroups;
				for (int i = 0; i < FormViewDesigner._controlTemplateNames.Length; i++)
				{
					string text = FormViewDesigner._controlTemplateNames[i];
					TemplateGroup templateGroup = new TemplateGroup(FormViewDesigner._controlTemplateNames[i], this.GetTemplateStyle(i));
					templateGroup.AddTemplateDefinition(new TemplateDefinition(this, text, base.Component, text)
					{
						SupportsDataBinding = FormViewDesigner._controlTemplateSupportsDataBinding[i]
					});
					templateGroups.Add(templateGroup);
				}
				return templateGroups;
			}
		}

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x06002818 RID: 10264 RVA: 0x000DBCCD File Offset: 0x000DACCD
		protected override bool UsePreviewControl
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002819 RID: 10265 RVA: 0x000DBCD0 File Offset: 0x000DACD0
		private void AddTemplatesAndKeys(IDataSourceViewSchema schema)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			StringBuilder stringBuilder3 = new StringBuilder();
			IDesignerHost designerHost = (IDesignerHost)base.Component.Site.GetService(typeof(IDesignerHost));
			if (schema != null)
			{
				IDataSourceFieldSchema[] fields = schema.GetFields();
				if (fields != null && fields.Length > 0)
				{
					ArrayList arrayList = new ArrayList();
					foreach (IDataSourceFieldSchema dataSourceFieldSchema in fields)
					{
						string name = dataSourceFieldSchema.Name;
						char[] array2 = new char[name.Length];
						for (int j = 0; j < name.Length; j++)
						{
							char c = name[j];
							if (char.IsLetterOrDigit(c) || c == '_')
							{
								array2[j] = c;
							}
							else
							{
								array2[j] = '_';
							}
						}
						string text = new string(array2);
						string text2 = DesignTimeDataBinding.CreateEvalExpression(name, string.Empty);
						string text3 = DesignTimeDataBinding.CreateBindExpression(name, string.Empty);
						if (dataSourceFieldSchema.PrimaryKey || dataSourceFieldSchema.Identity)
						{
							if (this.EnableDynamicData)
							{
								stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "{0}: <asp:DynamicControl DataField=\"{0}\" runat=\"server\" id=\"{2}DynamicControl\" Mode=\"{1}\" /><br />", new object[] { name, "ReadOnly", text }));
								stringBuilder2.Append(string.Format(CultureInfo.InvariantCulture, "{0}: <asp:DynamicControl DataField=\"{0}\" runat=\"server\" id=\"{2}DynamicControl\" Mode=\"{1}\" /><br />", new object[] { name, "ReadOnly", text }));
							}
							else
							{
								stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "{0}: <asp:Label Text='<%# {1} %>' runat=\"server\" id=\"{2}Label1\" /><br />", new object[] { name, text2, text }));
								stringBuilder2.Append(string.Format(CultureInfo.InvariantCulture, "{0}: <asp:Label Text='<%# {1} %>' runat=\"server\" id=\"{2}Label\" /><br />", new object[] { name, text2, text }));
							}
							if (!dataSourceFieldSchema.Identity)
							{
								if (this.EnableDynamicData)
								{
									stringBuilder3.Append(string.Format(CultureInfo.InvariantCulture, "{0}: <asp:DynamicControl DataField=\"{0}\" runat=\"server\" id=\"{1}DynamicControl\" Mode=\"Insert\" ValidationGroup=\"Insert\" /><br />", new object[] { name, text }));
								}
								else
								{
									stringBuilder3.Append(string.Format(CultureInfo.InvariantCulture, "{0}: <asp:TextBox Text='<%# {1} %>' runat=\"server\" id=\"{2}TextBox\" /><br />", new object[] { name, text3, text }));
								}
							}
						}
						else if (this.EnableDynamicData)
						{
							stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "{0}: <asp:DynamicControl DataField=\"{0}\" runat=\"server\" id=\"{2}DynamicControl\" Mode=\"{1}\" /><br />", new object[] { name, "Edit", text }));
							stringBuilder2.Append(string.Format(CultureInfo.InvariantCulture, "{0}: <asp:DynamicControl DataField=\"{0}\" runat=\"server\" id=\"{2}DynamicControl\" Mode=\"{1}\" /><br />", new object[] { name, "ReadOnly", text }));
							stringBuilder3.Append(string.Format(CultureInfo.InvariantCulture, "{0}: <asp:DynamicControl DataField=\"{0}\" runat=\"server\" id=\"{1}DynamicControl\" Mode=\"Insert\" ValidationGroup=\"Insert\" /><br />", new object[] { name, text }));
						}
						else if (dataSourceFieldSchema.DataType == typeof(bool) || dataSourceFieldSchema.DataType == typeof(bool?))
						{
							stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "{0}: <asp:CheckBox Checked='<%# {1} %>' runat=\"server\" id=\"{2}CheckBox\" /><br />", new object[] { name, text3, text }));
							stringBuilder2.Append(string.Format(CultureInfo.InvariantCulture, "{0}: <asp:CheckBox Checked='<%# {1} %>' runat=\"server\" id=\"{2}CheckBox\" Enabled=\"false\" /><br />", new object[] { name, text3, text }));
							stringBuilder3.Append(string.Format(CultureInfo.InvariantCulture, "{0}: <asp:CheckBox Checked='<%# {1} %>' runat=\"server\" id=\"{2}CheckBox\" /><br />", new object[] { name, text3, text }));
						}
						else
						{
							stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "{0}: <asp:TextBox Text='<%# {1} %>' runat=\"server\" id=\"{2}TextBox\" /><br />", new object[] { name, text3, text }));
							stringBuilder2.Append(string.Format(CultureInfo.InvariantCulture, "{0}: <asp:Label Text='<%# {1} %>' runat=\"server\" id=\"{2}Label\" /><br />", new object[] { name, text3, text }));
							stringBuilder3.Append(string.Format(CultureInfo.InvariantCulture, "{0}: <asp:TextBox Text='<%# {1} %>' runat=\"server\" id=\"{2}TextBox\" /><br />", new object[] { name, text3, text }));
						}
						stringBuilder.Append(Environment.NewLine);
						stringBuilder2.Append(Environment.NewLine);
						stringBuilder3.Append(Environment.NewLine);
						if (dataSourceFieldSchema.PrimaryKey)
						{
							arrayList.Add(name);
						}
					}
					bool flag = true;
					if (base.DesignerView.CanUpdate)
					{
						stringBuilder2.Append(string.Format(CultureInfo.InvariantCulture, "<asp:LinkButton runat=\"server\" Text=\"{3}\" CommandName=\"{0}\" id=\"{1}{0}Button\" CausesValidation=\"{2}\" />", new object[]
						{
							"Edit",
							string.Empty,
							bool.FalseString,
							SR.GetString("FormView_Edit")
						}));
						flag = false;
					}
					if (this.EnableDynamicData)
					{
						stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<asp:LinkButton runat=\"server\" Text=\"{3}\" CommandName=\"{0}\" id=\"{1}{0}Button\" CausesValidation=\"{2}\" ValidationGroup=\"Insert\" />", new object[]
						{
							"Update",
							string.Empty,
							bool.TrueString,
							SR.GetString("FormView_Update")
						}));
					}
					else
					{
						stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<asp:LinkButton runat=\"server\" Text=\"{3}\" CommandName=\"{0}\" id=\"{1}{0}Button\" CausesValidation=\"{2}\" />", new object[]
						{
							"Update",
							string.Empty,
							bool.TrueString,
							SR.GetString("FormView_Update")
						}));
					}
					stringBuilder.Append("&nbsp;");
					stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<asp:LinkButton runat=\"server\" Text=\"{3}\" CommandName=\"{0}\" id=\"{1}{0}Button\" CausesValidation=\"{2}\" />", new object[]
					{
						"Cancel",
						"Update",
						bool.FalseString,
						SR.GetString("FormView_Cancel")
					}));
					if (base.DesignerView.CanDelete)
					{
						if (!flag)
						{
							stringBuilder2.Append("&nbsp;");
						}
						stringBuilder2.Append(string.Format(CultureInfo.InvariantCulture, "<asp:LinkButton runat=\"server\" Text=\"{3}\" CommandName=\"{0}\" id=\"{1}{0}Button\" CausesValidation=\"{2}\" />", new object[]
						{
							"Delete",
							string.Empty,
							bool.FalseString,
							SR.GetString("FormView_Delete")
						}));
						flag = false;
					}
					if (base.DesignerView.CanInsert)
					{
						if (!flag)
						{
							stringBuilder2.Append("&nbsp;");
						}
						stringBuilder2.Append(string.Format(CultureInfo.InvariantCulture, "<asp:LinkButton runat=\"server\" Text=\"{3}\" CommandName=\"{0}\" id=\"{1}{0}Button\" CausesValidation=\"{2}\" />", new object[]
						{
							"New",
							string.Empty,
							bool.FalseString,
							SR.GetString("FormView_New")
						}));
					}
					if (this.EnableDynamicData)
					{
						stringBuilder3.Append(string.Format(CultureInfo.InvariantCulture, "<asp:LinkButton runat=\"server\" Text=\"{3}\" CommandName=\"{0}\" id=\"{1}{0}Button\" CausesValidation=\"{2}\" ValidationGroup=\"Insert\" />", new object[]
						{
							"Insert",
							string.Empty,
							bool.TrueString,
							SR.GetString("FormView_Insert")
						}));
					}
					else
					{
						stringBuilder3.Append(string.Format(CultureInfo.InvariantCulture, "<asp:LinkButton runat=\"server\" Text=\"{3}\" CommandName=\"{0}\" id=\"{1}{0}Button\" CausesValidation=\"{2}\" />", new object[]
						{
							"Insert",
							string.Empty,
							bool.TrueString,
							SR.GetString("FormView_Insert")
						}));
					}
					stringBuilder3.Append("&nbsp;");
					stringBuilder3.Append(string.Format(CultureInfo.InvariantCulture, "<asp:LinkButton runat=\"server\" Text=\"{3}\" CommandName=\"{0}\" id=\"{1}{0}Button\" CausesValidation=\"{2}\" />", new object[]
					{
						"Cancel",
						"Insert",
						bool.FalseString,
						SR.GetString("FormView_Cancel")
					}));
					stringBuilder.Append(Environment.NewLine);
					stringBuilder2.Append(Environment.NewLine);
					stringBuilder3.Append(Environment.NewLine);
					try
					{
						((FormView)base.Component).EditItemTemplate = ControlParser.ParseTemplate(designerHost, stringBuilder.ToString());
						((FormView)base.Component).ItemTemplate = ControlParser.ParseTemplate(designerHost, stringBuilder2.ToString());
						((FormView)base.Component).InsertItemTemplate = ControlParser.ParseTemplate(designerHost, stringBuilder3.ToString());
					}
					catch
					{
					}
					int count = arrayList.Count;
					if (count > 0)
					{
						string[] array3 = new string[count];
						arrayList.CopyTo(array3, 0);
						((FormView)base.Component).DataKeyNames = array3;
					}
				}
			}
		}

		// Token: 0x0600281A RID: 10266 RVA: 0x000DC544 File Offset: 0x000DB544
		private bool EnablePagingCallback(object context)
		{
			bool allowPaging = ((FormView)base.Component).AllowPaging;
			bool flag = !allowPaging;
			if (context is bool)
			{
				flag = (bool)context;
			}
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(typeof(FormView))["AllowPaging"];
			propertyDescriptor.SetValue(base.Component, flag);
			return true;
		}

		// Token: 0x0600281B RID: 10267 RVA: 0x000DC5A4 File Offset: 0x000DB5A4
		private IDataSourceViewSchema GetDataSourceSchema()
		{
			DesignerDataSourceView designerView = base.DesignerView;
			if (designerView != null)
			{
				try
				{
					return designerView.Schema;
				}
				catch (Exception ex)
				{
					IComponentDesignerDebugService componentDesignerDebugService = (IComponentDesignerDebugService)base.Component.Site.GetService(typeof(IComponentDesignerDebugService));
					if (componentDesignerDebugService != null)
					{
						componentDesignerDebugService.Fail(SR.GetString("DataSource_DebugService_FailedCall", new object[] { "DesignerDataSourceView.Schema", ex.Message }));
					}
				}
			}
			return null;
		}

		// Token: 0x0600281C RID: 10268 RVA: 0x000DC62C File Offset: 0x000DB62C
		public override string GetDesignTimeHtml()
		{
			FormView formView = (FormView)base.ViewControl;
			bool flag = false;
			string[] array = null;
			string text = null;
			if (this.CurrentModeTemplateExists)
			{
				bool flag2 = false;
				IDataSourceViewSchema dataSourceSchema = this.GetDataSourceSchema();
				if (dataSourceSchema != null)
				{
					IDataSourceFieldSchema[] fields = dataSourceSchema.GetFields();
					if (fields != null && fields.Length > 0)
					{
						flag2 = true;
					}
				}
				try
				{
					if (!flag2)
					{
						array = formView.DataKeyNames;
						formView.DataKeyNames = new string[0];
						flag = true;
					}
					TypeDescriptor.Refresh(base.Component);
					return base.GetDesignTimeHtml();
				}
				finally
				{
					if (flag)
					{
						formView.DataKeyNames = array;
					}
				}
			}
			text = this.GetEmptyDesignTimeHtml();
			return text;
		}

		// Token: 0x0600281D RID: 10269 RVA: 0x000DC6CC File Offset: 0x000DB6CC
		protected override string GetEmptyDesignTimeHtml()
		{
			return base.CreatePlaceHolderDesignTimeHtml(SR.GetString("DataList_NoTemplatesInst"));
		}

		// Token: 0x0600281E RID: 10270 RVA: 0x000DC6E0 File Offset: 0x000DB6E0
		private Style GetTemplateStyle(int templateIndex)
		{
			Style style = new Style();
			style.CopyFrom(((FormView)base.ViewControl).ControlStyle);
			switch (templateIndex)
			{
			case 0:
				style.CopyFrom(((FormView)base.ViewControl).RowStyle);
				break;
			case 1:
				style.CopyFrom(((FormView)base.ViewControl).FooterStyle);
				break;
			case 2:
				style.CopyFrom(((FormView)base.ViewControl).RowStyle);
				style.CopyFrom(((FormView)base.ViewControl).EditRowStyle);
				break;
			case 3:
				style.CopyFrom(((FormView)base.ViewControl).RowStyle);
				style.CopyFrom(((FormView)base.ViewControl).InsertRowStyle);
				break;
			case 4:
				style.CopyFrom(((FormView)base.ViewControl).HeaderStyle);
				break;
			case 5:
				style.CopyFrom(((FormView)base.ViewControl).EmptyDataRowStyle);
				break;
			case 6:
				style.CopyFrom(((FormView)base.ViewControl).PagerStyle);
				break;
			}
			return style;
		}

		// Token: 0x0600281F RID: 10271 RVA: 0x000DC80B File Offset: 0x000DB80B
		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(FormView));
			base.Initialize(component);
			if (base.View != null)
			{
				base.View.SetFlags(ViewFlags.TemplateEditing, true);
			}
		}

		// Token: 0x06002820 RID: 10272 RVA: 0x000DC839 File Offset: 0x000DB839
		protected override void OnSchemaRefreshed()
		{
			if (base.InTemplateMode)
			{
				return;
			}
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.SchemaRefreshedCallback), null, SR.GetString("DataControls_SchemaRefreshedTransaction"));
		}

		// Token: 0x06002821 RID: 10273 RVA: 0x000DC868 File Offset: 0x000DB868
		private bool SchemaRefreshedCallback(object context)
		{
			IDataSourceViewSchema dataSourceSchema = this.GetDataSourceSchema();
			if (base.DataSourceID.Length > 0 && dataSourceSchema != null)
			{
				if (((FormView)base.Component).DataKeyNames.Length > 0 || ((FormView)base.Component).ItemTemplate != null || ((FormView)base.Component).EditItemTemplate != null)
				{
					if (DialogResult.Yes == UIServiceHelper.ShowMessage(base.Component.Site, SR.GetString("FormView_SchemaRefreshedWarning"), SR.GetString("FormView_SchemaRefreshedCaption", new object[] { ((FormView)base.Component).ID }), MessageBoxButtons.YesNo))
					{
						((FormView)base.Component).DataKeyNames = new string[0];
						this.AddTemplatesAndKeys(dataSourceSchema);
					}
				}
				else
				{
					this.AddTemplatesAndKeys(dataSourceSchema);
				}
			}
			else if ((((FormView)base.Component).DataKeyNames.Length > 0 || ((FormView)base.Component).ItemTemplate != null || ((FormView)base.Component).EditItemTemplate != null) && DialogResult.Yes == UIServiceHelper.ShowMessage(base.Component.Site, SR.GetString("FormView_SchemaRefreshedWarningNoDataSource"), SR.GetString("FormView_SchemaRefreshedCaption", new object[] { ((FormView)base.Component).ID }), MessageBoxButtons.YesNo))
			{
				((FormView)base.Component).DataKeyNames = new string[0];
				((FormView)base.Component).ItemTemplate = null;
				((FormView)base.Component).InsertItemTemplate = null;
				((FormView)base.Component).EditItemTemplate = null;
			}
			this.UpdateDesignTimeHtml();
			return true;
		}

		// Token: 0x04001BC4 RID: 7108
		private const int IDX_CONTROL_HEADER_TEMPLATE = 4;

		// Token: 0x04001BC5 RID: 7109
		private const int IDX_CONTROL_ITEM_TEMPLATE = 0;

		// Token: 0x04001BC6 RID: 7110
		private const int IDX_CONTROL_EDITITEM_TEMPLATE = 2;

		// Token: 0x04001BC7 RID: 7111
		private const int IDX_CONTROL_INSERTITEM_TEMPLATE = 3;

		// Token: 0x04001BC8 RID: 7112
		private const int IDX_CONTROL_FOOTER_TEMPLATE = 1;

		// Token: 0x04001BC9 RID: 7113
		private const int IDX_CONTROL_EMPTY_DATA_TEMPLATE = 5;

		// Token: 0x04001BCA RID: 7114
		private const int IDX_CONTROL_PAGER_TEMPLATE = 6;

		// Token: 0x04001BCB RID: 7115
		private const string itemTemplateFieldString = "{0}: <asp:Label Text='<%# {1} %>' runat=\"server\" id=\"{2}Label\" /><br />";

		// Token: 0x04001BCC RID: 7116
		private const string keyItemTemplateFieldString = "{0}: <asp:Label Text='<%# {1} %>' runat=\"server\" id=\"{2}Label\" /><br />";

		// Token: 0x04001BCD RID: 7117
		private const string boolItemTemplateFieldString = "{0}: <asp:CheckBox Checked='<%# {1} %>' runat=\"server\" id=\"{2}CheckBox\" Enabled=\"false\" /><br />";

		// Token: 0x04001BCE RID: 7118
		private const string dynamicDataItemTemplateFieldString = "{0}: <asp:DynamicControl DataField=\"{0}\" runat=\"server\" id=\"{2}DynamicControl\" Mode=\"{1}\" /><br />";

		// Token: 0x04001BCF RID: 7119
		private const string editItemTemplateFieldString = "{0}: <asp:TextBox Text='<%# {1} %>' runat=\"server\" id=\"{2}TextBox\" /><br />";

		// Token: 0x04001BD0 RID: 7120
		private const string boolEditItemTemplateFieldString = "{0}: <asp:CheckBox Checked='<%# {1} %>' runat=\"server\" id=\"{2}CheckBox\" /><br />";

		// Token: 0x04001BD1 RID: 7121
		private const string keyEditItemTemplateFieldString = "{0}: <asp:Label Text='<%# {1} %>' runat=\"server\" id=\"{2}Label1\" /><br />";

		// Token: 0x04001BD2 RID: 7122
		private const string insertItemTemplateFieldString = "{0}: <asp:TextBox Text='<%# {1} %>' runat=\"server\" id=\"{2}TextBox\" /><br />";

		// Token: 0x04001BD3 RID: 7123
		private const string boolInsertItemTemplateFieldString = "{0}: <asp:CheckBox Checked='<%# {1} %>' runat=\"server\" id=\"{2}CheckBox\" /><br />";

		// Token: 0x04001BD4 RID: 7124
		private const string dynamicDataInsertItemTemplateFieldString = "{0}: <asp:DynamicControl DataField=\"{0}\" runat=\"server\" id=\"{1}DynamicControl\" Mode=\"Insert\" ValidationGroup=\"Insert\" /><br />";

		// Token: 0x04001BD5 RID: 7125
		private const string templateButtonString = "<asp:LinkButton runat=\"server\" Text=\"{3}\" CommandName=\"{0}\" id=\"{1}{0}Button\" CausesValidation=\"{2}\" />";

		// Token: 0x04001BD6 RID: 7126
		private const string dynamicDataInsertTemplateButtonString = "<asp:LinkButton runat=\"server\" Text=\"{3}\" CommandName=\"{0}\" id=\"{1}{0}Button\" CausesValidation=\"{2}\" ValidationGroup=\"Insert\" />";

		// Token: 0x04001BD7 RID: 7127
		private const string nonBreakingSpace = "&nbsp;";

		// Token: 0x04001BD8 RID: 7128
		private static DesignerAutoFormatCollection _autoFormats;

		// Token: 0x04001BD9 RID: 7129
		private static string[] _controlTemplateNames = new string[] { "ItemTemplate", "FooterTemplate", "EditItemTemplate", "InsertItemTemplate", "HeaderTemplate", "EmptyDataTemplate", "PagerTemplate" };

		// Token: 0x04001BDA RID: 7130
		private static bool[] _controlTemplateSupportsDataBinding = new bool[] { true, true, true, true, true, true, true };

		// Token: 0x04001BDB RID: 7131
		private FormViewActionList _actionLists;

		// Token: 0x04001BDC RID: 7132
		private bool _enableDynamicData;

		// Token: 0x04001BDD RID: 7133
		[CompilerGenerated]
		private static ControlDesigner.CreateAutoFormatDelegate <>9__CachedAnonymousMethodDelegate1;
	}
}
