using System;
using System.Collections;
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
	// Token: 0x02000451 RID: 1105
	public class GridViewDesigner : DataBoundControlDesigner
	{
		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x0600284F RID: 10319 RVA: 0x000DD5A0 File Offset: 0x000DC5A0
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection designerActionListCollection = new DesignerActionListCollection();
				designerActionListCollection.AddRange(base.ActionLists);
				if (this._actionLists == null)
				{
					this._actionLists = new GridViewActionList(this);
				}
				bool inTemplateMode = base.InTemplateMode;
				int selectedFieldIndex = this.SelectedFieldIndex;
				this.UpdateFieldsCurrentState();
				this._actionLists.AllowRemoveField = ((GridView)base.Component).Columns.Count > 0 && selectedFieldIndex >= 0 && !inTemplateMode;
				this._actionLists.AllowMoveLeft = ((GridView)base.Component).Columns.Count > 0 && selectedFieldIndex > 0 && !inTemplateMode;
				this._actionLists.AllowMoveRight = ((GridView)base.Component).Columns.Count > 0 && selectedFieldIndex >= 0 && ((GridView)base.Component).Columns.Count > selectedFieldIndex + 1 && !inTemplateMode;
				DesignerDataSourceView designerView = base.DesignerView;
				this._actionLists.AllowPaging = !inTemplateMode && designerView != null;
				this._actionLists.AllowSorting = !inTemplateMode && designerView != null && designerView.CanSort;
				this._actionLists.AllowEditing = !inTemplateMode && designerView != null && designerView.CanUpdate;
				this._actionLists.AllowDeleting = !inTemplateMode && designerView != null && designerView.CanDelete;
				this._actionLists.AllowSelection = !inTemplateMode && designerView != null;
				designerActionListCollection.Add(this._actionLists);
				return designerActionListCollection;
			}
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06002850 RID: 10320 RVA: 0x000DD72F File Offset: 0x000DC72F
		public override DesignerAutoFormatCollection AutoFormats
		{
			get
			{
				if (GridViewDesigner._autoFormats == null)
				{
					GridViewDesigner._autoFormats = ControlDesigner.CreateAutoFormats("<Schemes>\r\n        <xsd:schema id=\"Schemes\" xmlns=\"\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\">\r\n          <xsd:element name=\"Scheme\">\r\n            <xsd:complexType>\r\n              <xsd:all>\r\n                <xsd:element name=\"SchemeName\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"ForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"BackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"BorderColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"BorderWidth\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"BorderStyle\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"GridLines\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"CellPadding\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"CellSpacing\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"ItemForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"ItemBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"ItemFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"AltItemForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"AltItemBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"AltItemFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"SelItemForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"SelItemBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"SelItemFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"HeaderForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"HeaderBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"HeaderFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"FooterForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"FooterBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"FooterFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"PagerForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"PagerBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"PagerFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"PagerAlign\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"PagerButtons\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"EditItemForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"EditItemBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"EditItemFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n              </xsd:all>\r\n            </xsd:complexType>\r\n          </xsd:element>\r\n          <xsd:element name=\"Schemes\" msdata:IsDataSet=\"true\">\r\n            <xsd:complexType>\r\n              <xsd:choice maxOccurs=\"unbounded\">\r\n                <xsd:element ref=\"Scheme\"/>\r\n              </xsd:choice>\r\n            </xsd:complexType>\r\n          </xsd:element>\r\n        </xsd:schema>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Empty</SchemeName>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Consistent1</SchemeName>\r\n          <AltItemBackColor>White</AltItemBackColor>\r\n          <GridLines>0</GridLines>\r\n          <CellPadding>4</CellPadding>\r\n          <ForeColor>#333333</ForeColor>\r\n          <ItemForeColor>#333333</ItemForeColor>\r\n          <ItemBackColor>#FFFBD6</ItemBackColor>\r\n          <SelItemForeColor>Navy</SelItemForeColor>\r\n          <SelItemBackColor>#FFCC66</SelItemBackColor>\r\n          <SelItemFont>1</SelItemFont>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>#990000</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>White</FooterForeColor>\r\n          <FooterBackColor>#990000</FooterBackColor>\r\n          <FooterFont>1</FooterFont>\r\n          <PagerForeColor>#333333</PagerForeColor>\r\n          <PagerBackColor>#FFCC66</PagerBackColor>\r\n          <PagerAlign>2</PagerAlign>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Consistent2</SchemeName>\r\n            <AltItemBackColor>White</AltItemBackColor>\r\n            <GridLines>0</GridLines>\r\n            <CellPadding>4</CellPadding>\r\n            <ForeColor>#333333</ForeColor>\r\n            <ItemBackColor>#EFF3FB</ItemBackColor>\r\n            <SelItemForeColor>#333333</SelItemForeColor>\r\n            <SelItemBackColor>#D1DDF1</SelItemBackColor>\r\n            <SelItemFont>1</SelItemFont>\r\n            <HeaderForeColor>White</HeaderForeColor>\r\n            <HeaderBackColor>#507CD1</HeaderBackColor>\r\n            <HeaderFont>1</HeaderFont>\r\n            <FooterForeColor>White</FooterForeColor>\r\n            <FooterBackColor>#507CD1</FooterBackColor>\r\n            <FooterFont>1</FooterFont>\r\n            <PagerForeColor>White</PagerForeColor>\r\n            <PagerBackColor>#2461BF</PagerBackColor>\r\n            <PagerAlign>2</PagerAlign>\r\n            <EditItemBackColor>#2461BF</EditItemBackColor>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Consistent3</SchemeName>\r\n            <AltItemBackColor>White</AltItemBackColor>\r\n            <GridLines>0</GridLines>\r\n            <CellPadding>4</CellPadding>\r\n            <ForeColor>#333333</ForeColor>\r\n            <ItemBackColor>#E3EAEB</ItemBackColor>\r\n            <SelItemForeColor>#333333</SelItemForeColor>\r\n            <SelItemBackColor>#C5BBAF</SelItemBackColor>\r\n            <SelItemFont>1</SelItemFont>\r\n            <HeaderForeColor>White</HeaderForeColor>\r\n            <HeaderBackColor>#1C5E55</HeaderBackColor>\r\n            <HeaderFont>1</HeaderFont>\r\n            <FooterForeColor>White</FooterForeColor>\r\n            <FooterBackColor>#1C5E55</FooterBackColor>\r\n            <FooterFont>1</FooterFont>\r\n            <PagerForeColor>White</PagerForeColor>\r\n            <PagerBackColor>#666666</PagerBackColor>\r\n            <PagerAlign>2</PagerAlign>\r\n            <EditItemBackColor>#7C6F57</EditItemBackColor>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Consistent4</SchemeName>\r\n            <AltItemBackColor>White</AltItemBackColor>\r\n            <AltItemForeColor>#284775</AltItemForeColor>\r\n            <GridLines>0</GridLines>\r\n            <CellPadding>4</CellPadding>\r\n            <ForeColor>#333333</ForeColor>\r\n            <ItemForeColor>#333333</ItemForeColor>\r\n            <ItemBackColor>#F7F6F3</ItemBackColor>\r\n            <SelItemForeColor>#333333</SelItemForeColor>\r\n            <SelItemBackColor>#E2DED6</SelItemBackColor>\r\n            <SelItemFont>1</SelItemFont>\r\n            <HeaderForeColor>White</HeaderForeColor>\r\n            <HeaderBackColor>#5D7B9D</HeaderBackColor>\r\n            <HeaderFont>1</HeaderFont>\r\n            <FooterForeColor>White</FooterForeColor>\r\n            <FooterBackColor>#5D7B9D</FooterBackColor>\r\n            <FooterFont>1</FooterFont>\r\n            <PagerForeColor>White</PagerForeColor>\r\n            <PagerBackColor>#284775</PagerBackColor>\r\n            <PagerAlign>2</PagerAlign>\r\n            <EditItemBackColor>#999999</EditItemBackColor>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Colorful1</SchemeName>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#CC9966</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>1</BorderStyle>\r\n          <GridLines>3</GridLines>\r\n          <CellPadding>4</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <ItemForeColor>#330099</ItemForeColor>\r\n          <ItemBackColor>White</ItemBackColor>\r\n          <SelItemForeColor>#663399</SelItemForeColor>\r\n          <SelItemBackColor>#FFCC66</SelItemBackColor>\r\n          <SelItemFont>1</SelItemFont>\r\n          <HeaderForeColor>#FFFFCC</HeaderForeColor>\r\n          <HeaderBackColor>#990000</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>#330099</FooterForeColor>\r\n          <FooterBackColor>#FFFFCC</FooterBackColor>\r\n          <PagerForeColor>#330099</PagerForeColor>\r\n          <PagerBackColor>#FFFFCC</PagerBackColor>\r\n          <PagerAlign>2</PagerAlign>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Colorful2</SchemeName>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#3366CC</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>1</BorderStyle>\r\n          <GridLines>3</GridLines>\r\n          <CellPadding>4</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <ItemForeColor>#003399</ItemForeColor>\r\n          <ItemBackColor>White</ItemBackColor>\r\n          <SelItemForeColor>#CCFF99</SelItemForeColor>\r\n          <SelItemBackColor>#009999</SelItemBackColor>\r\n          <SelItemFont>1</SelItemFont>\r\n          <HeaderForeColor>#CCCCFF</HeaderForeColor>\r\n          <HeaderBackColor>#003399</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>#003399</FooterForeColor>\r\n          <FooterBackColor>#99CCCC</FooterBackColor>\r\n          <PagerForeColor>#003399</PagerForeColor>\r\n          <PagerBackColor>#99CCCC</PagerBackColor>\r\n          <PagerAlign>1</PagerAlign>\r\n          <PagerButtons>1</PagerButtons>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Colorful3</SchemeName>\r\n          <BackColor>#DEBA84</BackColor>\r\n          <BorderColor>#DEBA84</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>1</BorderStyle>\r\n          <GridLines>3</GridLines>\r\n          <CellPadding>3</CellPadding>\r\n          <CellSpacing>2</CellSpacing>\r\n          <ItemForeColor>#8C4510</ItemForeColor>\r\n          <ItemBackColor>#FFF7E7</ItemBackColor>\r\n          <SelItemForeColor>White</SelItemForeColor>\r\n          <SelItemBackColor>#738A9C</SelItemBackColor>\r\n          <SelItemFont>1</SelItemFont>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>#A55129</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>#8C4510</FooterForeColor>\r\n          <FooterBackColor>#F7DFB5</FooterBackColor>\r\n          <PagerForeColor>#8C4510</PagerForeColor>\r\n          <PagerAlign>2</PagerAlign>\r\n          <PagerButtons>1</PagerButtons>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Colorful4</SchemeName>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#E7E7FF</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>1</BorderStyle>\r\n          <GridLines>1</GridLines>\r\n          <CellPadding>3</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <ItemForeColor>#4A3C8C</ItemForeColor>\r\n          <ItemBackColor>#E7E7FF</ItemBackColor>\r\n          <AltItemBackColor>#F7F7F7</AltItemBackColor>\r\n          <SelItemForeColor>#F7F7F7</SelItemForeColor>\r\n          <SelItemBackColor>#738A9C</SelItemBackColor>\r\n          <SelItemFont>1</SelItemFont>\r\n          <HeaderForeColor>#F7F7F7</HeaderForeColor>\r\n          <HeaderBackColor>#4A3C8C</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>#4A3C8C</FooterForeColor>\r\n          <FooterBackColor>#B5C7DE</FooterBackColor>\r\n          <PagerForeColor>#4A3C8C</PagerForeColor>\r\n          <PagerBackColor>#E7E7FF</PagerBackColor>\r\n          <PagerAlign>3</PagerAlign>\r\n          <PagerButtons>1</PagerButtons>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Colorful5</SchemeName>\r\n          <ForeColor>Black</ForeColor>\r\n          <BackColor>LightGoldenRodYellow</BackColor>\r\n          <BorderColor>Tan</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <GridLines>0</GridLines>\r\n          <CellPadding>2</CellPadding>\r\n          <AltItemBackColor>PaleGoldenRod</AltItemBackColor>\r\n          <HeaderBackColor>Tan</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterBackColor>Tan</FooterBackColor>\r\n          <SelItemBackColor>DarkSlateBlue</SelItemBackColor>\r\n          <SelItemForeColor>GhostWhite</SelItemForeColor>\r\n          <PagerBackColor>PaleGoldenrod</PagerBackColor>\r\n          <PagerForeColor>DarkSlateBlue</PagerForeColor>\r\n          <PagerAlign>2</PagerAlign>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Professional1</SchemeName>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#999999</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>1</BorderStyle>\r\n          <GridLines>2</GridLines>\r\n          <CellPadding>3</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <ItemForeColor>Black</ItemForeColor>\r\n          <ItemBackColor>#EEEEEE</ItemBackColor>\r\n          <AltItemBackColor>#DCDCDC</AltItemBackColor>\r\n          <SelItemForeColor>White</SelItemForeColor>\r\n          <SelItemBackColor>#008A8C</SelItemBackColor>\r\n          <SelItemFont>1</SelItemFont>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>#000084</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>Black</FooterForeColor>\r\n          <FooterBackColor>#CCCCCC</FooterBackColor>\r\n          <PagerForeColor>Black</PagerForeColor>\r\n          <PagerBackColor>#999999</PagerBackColor>\r\n          <PagerAlign>2</PagerAlign>\r\n          <PagerButtons>1</PagerButtons>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Professional2</SchemeName>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#CCCCCC</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>1</BorderStyle>\r\n          <GridLines>3</GridLines>\r\n          <CellPadding>3</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <ItemForeColor>#000066</ItemForeColor>\r\n          <SelItemForeColor>White</SelItemForeColor>\r\n          <SelItemBackColor>#669999</SelItemBackColor>\r\n          <SelItemFont>1</SelItemFont>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>#006699</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>#000066</FooterForeColor>\r\n          <FooterBackColor>White</FooterBackColor>\r\n          <PagerForeColor>#000066</PagerForeColor>\r\n          <PagerBackColor>White</PagerBackColor>\r\n          <PagerAlign>1</PagerAlign>\r\n          <PagerButtons>1</PagerButtons>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Professional3</SchemeName>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>White</BorderColor>\r\n          <BorderWidth>2px</BorderWidth>\r\n          <BorderStyle>7</BorderStyle>\r\n          <GridLines>0</GridLines>\r\n          <CellPadding>3</CellPadding>\r\n          <CellSpacing>1</CellSpacing>\r\n          <ItemForeColor>Black</ItemForeColor>\r\n          <ItemBackColor>#DEDFDE</ItemBackColor>\r\n          <SelItemForeColor>White</SelItemForeColor>\r\n          <SelItemBackColor>#9471DE</SelItemBackColor>\r\n          <SelItemFont>1</SelItemFont>\r\n          <HeaderForeColor>#E7E7FF</HeaderForeColor>\r\n          <HeaderBackColor>#4A3C8C</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>Black</FooterForeColor>\r\n          <FooterBackColor>#C6C3C6</FooterBackColor>\r\n          <PagerForeColor>Black</PagerForeColor>\r\n          <PagerBackColor>#C6C3C6</PagerBackColor>\r\n          <PagerAlign>3</PagerAlign>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Simple1</SchemeName>\r\n          <ForeColor>Black</ForeColor>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#999999</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>4</BorderStyle>\r\n          <GridLines>2</GridLines>\r\n          <CellPadding>3</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <AltItemBackColor>#CCCCCC</AltItemBackColor>\r\n          <SelItemForeColor>White</SelItemForeColor>\r\n          <SelItemBackColor>#000099</SelItemBackColor>\r\n          <SelItemFont>1</SelItemFont>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>Black</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterBackColor>#CCCCCC</FooterBackColor>\r\n          <PagerForeColor>Black</PagerForeColor>\r\n          <PagerBackColor>#999999</PagerBackColor>\r\n          <PagerAlign>2</PagerAlign>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Simple2</SchemeName>\r\n          <ForeColor>Black</ForeColor>\r\n          <BackColor>#CCCCCC</BackColor>\r\n          <BorderColor>#999999</BorderColor>\r\n          <BorderWidth>3px</BorderWidth>\r\n          <BorderStyle>4</BorderStyle>\r\n          <GridLines>3</GridLines>\r\n          <CellPadding>4</CellPadding>\r\n          <CellSpacing>2</CellSpacing>\r\n          <ItemBackColor>White</ItemBackColor>\r\n          <SelItemForeColor>White</SelItemForeColor>\r\n          <SelItemBackColor>#000099</SelItemBackColor>\r\n          <SelItemFont>1</SelItemFont>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>Black</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterBackColor>#CCCCCC</FooterBackColor>\r\n          <PagerForeColor>Black</PagerForeColor>\r\n          <PagerBackColor>#CCCCCC</PagerBackColor>\r\n          <PagerAlign>1</PagerAlign>\r\n          <PagerButtons>1</PagerButtons>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Simple3</SchemeName>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#336666</BorderColor>\r\n          <BorderWidth>3px</BorderWidth>\r\n          <BorderStyle>5</BorderStyle>\r\n          <GridLines>1</GridLines>\r\n          <CellPadding>4</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <ItemForeColor>#333333</ItemForeColor>\r\n          <ItemBackColor>White</ItemBackColor>\r\n          <SelItemForeColor>White</SelItemForeColor>\r\n          <SelItemBackColor>#339966</SelItemBackColor>\r\n          <SelItemFont>1</SelItemFont>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>#336666</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>#333333</FooterForeColor>\r\n          <FooterBackColor>White</FooterBackColor>\r\n          <PagerForeColor>White</PagerForeColor>\r\n          <PagerBackColor>#336666</PagerBackColor>\r\n          <PagerAlign>2</PagerAlign>\r\n          <PagerButtons>1</PagerButtons>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Classic1</SchemeName>\r\n          <ForeColor>Black</ForeColor>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#CCCCCC</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>1</BorderStyle>\r\n          <GridLines>1</GridLines>\r\n          <CellPadding>4</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <SelItemForeColor>White</SelItemForeColor>\r\n          <SelItemBackColor>#CC3333</SelItemBackColor>\r\n          <SelItemFont>1</SelItemFont>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>#333333</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>Black</FooterForeColor>\r\n          <FooterBackColor>#CCCC99</FooterBackColor>\r\n          <PagerForeColor>Black</PagerForeColor>\r\n          <PagerBackColor>White</PagerBackColor>\r\n          <PagerAlign>3</PagerAlign>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Classic2</SchemeName>\r\n          <ForeColor>Black</ForeColor>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#DEDFDE</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>1</BorderStyle>\r\n          <GridLines>2</GridLines>\r\n          <CellPadding>4</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <I[...string is too long...]", (DataRow schemeData) => new GridViewAutoFormat(schemeData));
				}
				return GridViewDesigner._autoFormats;
			}
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x06002851 RID: 10321 RVA: 0x000DD769 File Offset: 0x000DC769
		// (set) Token: 0x06002852 RID: 10322 RVA: 0x000DD774 File Offset: 0x000DC774
		internal bool EnableDeleting
		{
			get
			{
				return this._currentDeleteState;
			}
			set
			{
				Cursor cursor = Cursor.Current;
				try
				{
					Cursor.Current = Cursors.WaitCursor;
					ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.EnableDeletingCallback), value, SR.GetString("GridView_EnableDeletingTransaction"));
				}
				finally
				{
					Cursor.Current = cursor;
				}
			}
		}

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06002853 RID: 10323 RVA: 0x000DD7D4 File Offset: 0x000DC7D4
		// (set) Token: 0x06002854 RID: 10324 RVA: 0x000DD7DC File Offset: 0x000DC7DC
		internal bool EnableEditing
		{
			get
			{
				return this._currentEditState;
			}
			set
			{
				Cursor cursor = Cursor.Current;
				try
				{
					Cursor.Current = Cursors.WaitCursor;
					ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.EnableEditingCallback), value, SR.GetString("GridView_EnableEditingTransaction"));
				}
				finally
				{
					Cursor.Current = cursor;
				}
			}
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x06002855 RID: 10325 RVA: 0x000DD83C File Offset: 0x000DC83C
		// (set) Token: 0x06002856 RID: 10326 RVA: 0x000DD850 File Offset: 0x000DC850
		internal bool EnablePaging
		{
			get
			{
				return ((GridView)base.Component).AllowPaging;
			}
			set
			{
				Cursor cursor = Cursor.Current;
				try
				{
					Cursor.Current = Cursors.WaitCursor;
					ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.EnablePagingCallback), value, SR.GetString("GridView_EnablePagingTransaction"));
				}
				finally
				{
					Cursor.Current = cursor;
				}
			}
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x06002857 RID: 10327 RVA: 0x000DD8B0 File Offset: 0x000DC8B0
		// (set) Token: 0x06002858 RID: 10328 RVA: 0x000DD8B8 File Offset: 0x000DC8B8
		internal bool EnableSelection
		{
			get
			{
				return this._currentSelectState;
			}
			set
			{
				Cursor cursor = Cursor.Current;
				try
				{
					Cursor.Current = Cursors.WaitCursor;
					ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.EnableSelectionCallback), value, SR.GetString("GridView_EnableSelectionTransaction"));
				}
				finally
				{
					Cursor.Current = cursor;
				}
			}
		}

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x06002859 RID: 10329 RVA: 0x000DD918 File Offset: 0x000DC918
		// (set) Token: 0x0600285A RID: 10330 RVA: 0x000DD92C File Offset: 0x000DC92C
		internal bool EnableSorting
		{
			get
			{
				return ((GridView)base.Component).AllowSorting;
			}
			set
			{
				Cursor cursor = Cursor.Current;
				try
				{
					Cursor.Current = Cursors.WaitCursor;
					ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.EnableSortingCallback), value, SR.GetString("GridView_EnableSortingTransaction"));
				}
				finally
				{
					Cursor.Current = cursor;
				}
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x0600285B RID: 10331 RVA: 0x000DD98C File Offset: 0x000DC98C
		protected override int SampleRowCount
		{
			get
			{
				int num = 5;
				GridView gridView = (GridView)base.Component;
				if (gridView.AllowPaging && gridView.PageSize != 0)
				{
					num = Math.Min(gridView.PageSize, 100) + 1;
				}
				return num;
			}
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x0600285C RID: 10332 RVA: 0x000DD9C8 File Offset: 0x000DC9C8
		// (set) Token: 0x0600285D RID: 10333 RVA: 0x000DDA1C File Offset: 0x000DCA1C
		private int SelectedFieldIndex
		{
			get
			{
				object obj = base.DesignerState["SelectedFieldIndex"];
				int count = ((GridView)base.Component).Columns.Count;
				if (obj == null || count == 0 || (int)obj < 0 || (int)obj >= count)
				{
					return -1;
				}
				return (int)obj;
			}
			set
			{
				base.DesignerState["SelectedFieldIndex"] = value;
			}
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x0600285E RID: 10334 RVA: 0x000DDA34 File Offset: 0x000DCA34
		public override TemplateGroupCollection TemplateGroups
		{
			get
			{
				TemplateGroupCollection templateGroups = base.TemplateGroups;
				DataControlFieldCollection columns = ((GridView)base.Component).Columns;
				int count = columns.Count;
				if (count > 0)
				{
					for (int i = 0; i < count; i++)
					{
						TemplateField templateField = columns[i] as TemplateField;
						if (templateField != null)
						{
							string headerText = columns[i].HeaderText;
							string text = SR.GetString("GridView_Field", new object[] { i.ToString(NumberFormatInfo.InvariantInfo) });
							if (headerText != null && headerText.Length != 0)
							{
								text = text + " - " + headerText;
							}
							TemplateGroup templateGroup = new TemplateGroup(text);
							for (int j = 0; j < GridViewDesigner._columnTemplateNames.Length; j++)
							{
								string text2 = GridViewDesigner._columnTemplateNames[j];
								templateGroup.AddTemplateDefinition(new TemplateDefinition(this, text2, columns[i], text2, this.GetTemplateStyle(j + 1000, templateField))
								{
									SupportsDataBinding = GridViewDesigner._columnTemplateSupportsDataBinding[j]
								});
							}
							templateGroups.Add(templateGroup);
						}
					}
				}
				for (int k = 0; k < GridViewDesigner._controlTemplateNames.Length; k++)
				{
					string text3 = GridViewDesigner._controlTemplateNames[k];
					TemplateGroup templateGroup2 = new TemplateGroup(GridViewDesigner._controlTemplateNames[k]);
					templateGroup2.AddTemplateDefinition(new TemplateDefinition(this, text3, base.Component, text3, this.GetTemplateStyle(k, null))
					{
						SupportsDataBinding = GridViewDesigner._controlTemplateSupportsDataBinding[k]
					});
					templateGroups.Add(templateGroup2);
				}
				return templateGroups;
			}
		}

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x0600285F RID: 10335 RVA: 0x000DDBB8 File Offset: 0x000DCBB8
		protected override bool UsePreviewControl
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002860 RID: 10336 RVA: 0x000DDBBC File Offset: 0x000DCBBC
		private void AddKeysAndBoundFields(IDataSourceViewSchema schema)
		{
			DataControlFieldCollection columns = ((GridView)base.Component).Columns;
			if (schema != null)
			{
				IDataSourceFieldSchema[] fields = schema.GetFields();
				if (fields != null && fields.Length > 0)
				{
					ArrayList arrayList = new ArrayList();
					foreach (IDataSourceFieldSchema dataSourceFieldSchema in fields)
					{
						if (((GridView)base.Component).IsBindableType(dataSourceFieldSchema.DataType))
						{
							BoundField boundField;
							if (dataSourceFieldSchema.DataType == typeof(bool) || dataSourceFieldSchema.DataType == typeof(bool?))
							{
								boundField = new CheckBoxField();
							}
							else
							{
								boundField = new BoundField();
							}
							string name = dataSourceFieldSchema.Name;
							if (dataSourceFieldSchema.PrimaryKey)
							{
								arrayList.Add(name);
							}
							boundField.DataField = name;
							boundField.HeaderText = name;
							boundField.SortExpression = name;
							boundField.ReadOnly = dataSourceFieldSchema.PrimaryKey || dataSourceFieldSchema.IsReadOnly;
							boundField.InsertVisible = !dataSourceFieldSchema.Identity;
							columns.Add(boundField);
						}
					}
					((GridView)base.Component).AutoGenerateColumns = false;
					int count = arrayList.Count;
					if (count > 0)
					{
						string[] array2 = new string[count];
						arrayList.CopyTo(array2, 0);
						((GridView)base.Component).DataKeyNames = array2;
					}
				}
			}
		}

		// Token: 0x06002861 RID: 10337 RVA: 0x000DDD14 File Offset: 0x000DCD14
		internal void AddNewField()
		{
			Cursor cursor = Cursor.Current;
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				this._ignoreSchemaRefreshedEvent = true;
				ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.AddNewFieldChangeCallback), null, SR.GetString("GridView_AddNewFieldTransaction"));
				this._ignoreSchemaRefreshedEvent = false;
				this.UpdateDesignTimeHtml();
			}
			finally
			{
				Cursor.Current = cursor;
			}
		}

		// Token: 0x06002862 RID: 10338 RVA: 0x000DDD80 File Offset: 0x000DCD80
		private bool AddNewFieldChangeCallback(object context)
		{
			if (base.DataSourceDesigner != null)
			{
				base.DataSourceDesigner.SuppressDataSourceEvents();
			}
			AddDataControlFieldDialog addDataControlFieldDialog = new AddDataControlFieldDialog(this);
			DialogResult dialogResult = UIServiceHelper.ShowDialog(base.Component.Site, addDataControlFieldDialog);
			if (base.DataSourceDesigner != null)
			{
				base.DataSourceDesigner.ResumeDataSourceEvents();
			}
			return dialogResult == DialogResult.OK;
		}

		// Token: 0x06002863 RID: 10339 RVA: 0x000DDDD0 File Offset: 0x000DCDD0
		protected override void DataBind(BaseDataBoundControl dataBoundControl)
		{
			GridView gridView = (GridView)dataBoundControl;
			gridView.RowDataBound += this.OnRowDataBound;
			try
			{
				base.DataBind(dataBoundControl);
			}
			finally
			{
				gridView.RowDataBound -= this.OnRowDataBound;
			}
		}

		// Token: 0x06002864 RID: 10340 RVA: 0x000DDE24 File Offset: 0x000DCE24
		internal void EditFields()
		{
			Cursor cursor = Cursor.Current;
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				this._ignoreSchemaRefreshedEvent = true;
				ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.EditFieldsChangeCallback), null, SR.GetString("GridView_EditFieldsTransaction"));
				this._ignoreSchemaRefreshedEvent = false;
				this.UpdateDesignTimeHtml();
			}
			finally
			{
				Cursor.Current = cursor;
			}
		}

		// Token: 0x06002865 RID: 10341 RVA: 0x000DDE90 File Offset: 0x000DCE90
		private bool EditFieldsChangeCallback(object context)
		{
			if (base.DataSourceDesigner != null)
			{
				base.DataSourceDesigner.SuppressDataSourceEvents();
			}
			DataControlFieldsEditor dataControlFieldsEditor = new DataControlFieldsEditor(this);
			DialogResult dialogResult = UIServiceHelper.ShowDialog(base.Component.Site, dataControlFieldsEditor);
			if (base.DataSourceDesigner != null)
			{
				base.DataSourceDesigner.ResumeDataSourceEvents();
			}
			return dialogResult == DialogResult.OK;
		}

		// Token: 0x06002866 RID: 10342 RVA: 0x000DDEE0 File Offset: 0x000DCEE0
		private bool EnableDeletingCallback(object context)
		{
			bool flag = !this._currentDeleteState;
			if (context is bool)
			{
				flag = (bool)context;
			}
			this.SaveManipulationSetting(GridViewDesigner.ManipulationMode.Delete, flag);
			return true;
		}

		// Token: 0x06002867 RID: 10343 RVA: 0x000DDF10 File Offset: 0x000DCF10
		private bool EnableEditingCallback(object context)
		{
			bool flag = !this._currentEditState;
			if (context is bool)
			{
				flag = (bool)context;
			}
			this.SaveManipulationSetting(GridViewDesigner.ManipulationMode.Edit, flag);
			return true;
		}

		// Token: 0x06002868 RID: 10344 RVA: 0x000DDF40 File Offset: 0x000DCF40
		private bool EnablePagingCallback(object context)
		{
			bool allowPaging = ((GridView)base.Component).AllowPaging;
			bool flag = !allowPaging;
			if (context is bool)
			{
				flag = (bool)context;
			}
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(typeof(GridView))["AllowPaging"];
			propertyDescriptor.SetValue(base.Component, flag);
			return true;
		}

		// Token: 0x06002869 RID: 10345 RVA: 0x000DDFA0 File Offset: 0x000DCFA0
		private bool EnableSortingCallback(object context)
		{
			bool allowSorting = ((GridView)base.Component).AllowSorting;
			bool flag = !allowSorting;
			if (context is bool)
			{
				flag = (bool)context;
			}
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(typeof(GridView))["AllowSorting"];
			propertyDescriptor.SetValue(base.Component, flag);
			return true;
		}

		// Token: 0x0600286A RID: 10346 RVA: 0x000DE000 File Offset: 0x000DD000
		private bool EnableSelectionCallback(object context)
		{
			bool flag = !this._currentEditState;
			if (context is bool)
			{
				flag = (bool)context;
			}
			this.SaveManipulationSetting(GridViewDesigner.ManipulationMode.Select, flag);
			return true;
		}

		// Token: 0x0600286B RID: 10347 RVA: 0x000DE030 File Offset: 0x000DD030
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

		// Token: 0x0600286C RID: 10348 RVA: 0x000DE0B8 File Offset: 0x000DD0B8
		public override string GetDesignTimeHtml()
		{
			GridView gridView = (GridView)base.ViewControl;
			IDataSourceDesigner dataSourceDesigner = base.DataSourceDesigner;
			this._regionCount = 0;
			bool flag = false;
			IDataSourceViewSchema dataSourceSchema = this.GetDataSourceSchema();
			if (dataSourceSchema != null)
			{
				IDataSourceFieldSchema[] fields = dataSourceSchema.GetFields();
				if (fields != null && fields.Length > 0)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				gridView.DataKeyNames = null;
			}
			if (gridView.Columns.Count == 0)
			{
				gridView.AutoGenerateColumns = true;
			}
			TypeDescriptor.Refresh(base.Component);
			return base.GetDesignTimeHtml();
		}

		// Token: 0x0600286D RID: 10349 RVA: 0x000DE130 File Offset: 0x000DD130
		public override string GetDesignTimeHtml(DesignerRegionCollection regions)
		{
			string designTimeHtml = this.GetDesignTimeHtml();
			GridView gridView = (GridView)base.ViewControl;
			int count = gridView.Columns.Count;
			GridViewRow headerRow = gridView.HeaderRow;
			GridViewRow footerRow = gridView.FooterRow;
			int selectedFieldIndex = this.SelectedFieldIndex;
			if (headerRow != null)
			{
				for (int i = 0; i < count; i++)
				{
					string text = SR.GetString("GridView_Field", new object[] { i.ToString(NumberFormatInfo.InvariantInfo) });
					string headerText = gridView.Columns[i].HeaderText;
					if (headerText.Length == 0)
					{
						text = text + " - " + headerText;
					}
					DesignerRegion designerRegion = new DesignerRegion(this, text, true);
					designerRegion.UserData = i;
					if (i == selectedFieldIndex)
					{
						designerRegion.Highlight = true;
					}
					regions.Add(designerRegion);
				}
			}
			for (int j = 0; j < gridView.Rows.Count; j++)
			{
				GridViewRow gridViewRow = gridView.Rows[j];
				for (int k = 0; k < count; k++)
				{
					DesignerRegion designerRegion2 = new DesignerRegion(this, k.ToString(NumberFormatInfo.InvariantInfo), false);
					designerRegion2.UserData = -1;
					if (k == selectedFieldIndex)
					{
						designerRegion2.Highlight = true;
					}
					regions.Add(designerRegion2);
				}
			}
			if (footerRow != null)
			{
				for (int l = 0; l < count; l++)
				{
					DesignerRegion designerRegion3 = new DesignerRegion(this, l.ToString(NumberFormatInfo.InvariantInfo), false);
					designerRegion3.UserData = -1;
					if (l == selectedFieldIndex)
					{
						designerRegion3.Highlight = true;
					}
					regions.Add(designerRegion3);
				}
			}
			return designTimeHtml;
		}

		// Token: 0x0600286E RID: 10350 RVA: 0x000DE2D1 File Offset: 0x000DD2D1
		public override string GetEditableDesignerRegionContent(EditableDesignerRegion region)
		{
			return string.Empty;
		}

		// Token: 0x0600286F RID: 10351 RVA: 0x000DE2D8 File Offset: 0x000DD2D8
		private Style GetTemplateStyle(int templateIndex, TemplateField templateField)
		{
			Style style = new Style();
			style.CopyFrom(((GridView)base.ViewControl).ControlStyle);
			switch (templateIndex)
			{
			case 0:
				style.CopyFrom(((GridView)base.ViewControl).EmptyDataRowStyle);
				break;
			case 1:
				style.CopyFrom(((GridView)base.ViewControl).PagerStyle);
				break;
			default:
				switch (templateIndex)
				{
				case 1000:
					style.CopyFrom(((GridView)base.ViewControl).RowStyle);
					style.CopyFrom(templateField.ItemStyle);
					break;
				case 1001:
					style.CopyFrom(((GridView)base.ViewControl).RowStyle);
					style.CopyFrom(((GridView)base.ViewControl).AlternatingRowStyle);
					style.CopyFrom(templateField.ItemStyle);
					break;
				case 1002:
					style.CopyFrom(((GridView)base.ViewControl).RowStyle);
					style.CopyFrom(((GridView)base.ViewControl).EditRowStyle);
					style.CopyFrom(templateField.ItemStyle);
					break;
				case 1003:
					style.CopyFrom(((GridView)base.ViewControl).HeaderStyle);
					style.CopyFrom(templateField.HeaderStyle);
					break;
				case 1004:
					style.CopyFrom(((GridView)base.ViewControl).FooterStyle);
					style.CopyFrom(templateField.FooterStyle);
					break;
				}
				break;
			}
			return style;
		}

		// Token: 0x06002870 RID: 10352 RVA: 0x000DE459 File Offset: 0x000DD459
		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(GridView));
			base.Initialize(component);
			if (base.View != null)
			{
				base.View.SetFlags(ViewFlags.TemplateEditing, true);
			}
		}

		// Token: 0x06002871 RID: 10353 RVA: 0x000DE488 File Offset: 0x000DD488
		internal void MoveLeft()
		{
			Cursor cursor = Cursor.Current;
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.MoveLeftCallback), null, SR.GetString("GridView_MoveLeftTransaction"));
				this.UpdateDesignTimeHtml();
			}
			finally
			{
				Cursor.Current = cursor;
			}
		}

		// Token: 0x06002872 RID: 10354 RVA: 0x000DE4E8 File Offset: 0x000DD4E8
		private bool MoveLeftCallback(object context)
		{
			DataControlFieldCollection columns = ((GridView)base.Component).Columns;
			int selectedFieldIndex = this.SelectedFieldIndex;
			if (selectedFieldIndex > 0)
			{
				DataControlField dataControlField = columns[selectedFieldIndex];
				columns.RemoveAt(selectedFieldIndex);
				columns.Insert(selectedFieldIndex - 1, dataControlField);
				this.SelectedFieldIndex--;
				this.UpdateDesignTimeHtml();
				return true;
			}
			return false;
		}

		// Token: 0x06002873 RID: 10355 RVA: 0x000DE544 File Offset: 0x000DD544
		internal void MoveRight()
		{
			Cursor cursor = Cursor.Current;
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.MoveRightCallback), null, SR.GetString("GridView_MoveRightTransaction"));
				this.UpdateDesignTimeHtml();
			}
			finally
			{
				Cursor.Current = cursor;
			}
		}

		// Token: 0x06002874 RID: 10356 RVA: 0x000DE5A4 File Offset: 0x000DD5A4
		private bool MoveRightCallback(object context)
		{
			DataControlFieldCollection columns = ((GridView)base.Component).Columns;
			int selectedFieldIndex = this.SelectedFieldIndex;
			if (selectedFieldIndex >= 0 && columns.Count > selectedFieldIndex + 1)
			{
				DataControlField dataControlField = columns[selectedFieldIndex];
				columns.RemoveAt(selectedFieldIndex);
				columns.Insert(selectedFieldIndex + 1, dataControlField);
				this.SelectedFieldIndex++;
				this.UpdateDesignTimeHtml();
				return true;
			}
			return false;
		}

		// Token: 0x06002875 RID: 10357 RVA: 0x000DE608 File Offset: 0x000DD608
		protected override void OnClick(DesignerRegionMouseEventArgs e)
		{
			if (e.Region != null)
			{
				this.SelectedFieldIndex = (int)e.Region.UserData;
				this.UpdateDesignTimeHtml();
			}
		}

		// Token: 0x06002876 RID: 10358 RVA: 0x000DE630 File Offset: 0x000DD630
		private void OnRowDataBound(object sender, GridViewRowEventArgs e)
		{
			GridViewRow row = e.Row;
			if (row.RowType == DataControlRowType.DataRow || row.RowType == DataControlRowType.Header || row.RowType == DataControlRowType.Footer)
			{
				int count = ((GridView)sender).Columns.Count;
				int num = 0;
				if (((GridView)sender).AutoGenerateDeleteButton || ((GridView)sender).AutoGenerateEditButton || ((GridView)sender).AutoGenerateSelectButton)
				{
					num = 1;
				}
				for (int i = 0; i < count; i++)
				{
					TableCell tableCell = row.Cells[i + num];
					tableCell.Attributes[DesignerRegion.DesignerRegionAttributeName] = this._regionCount.ToString(NumberFormatInfo.InvariantInfo);
					this._regionCount++;
				}
			}
		}

		// Token: 0x06002877 RID: 10359 RVA: 0x000DE6E8 File Offset: 0x000DD6E8
		protected override void OnSchemaRefreshed()
		{
			if (base.InTemplateMode)
			{
				return;
			}
			if (this._ignoreSchemaRefreshedEvent)
			{
				return;
			}
			Cursor cursor = Cursor.Current;
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.SchemaRefreshedCallback), null, SR.GetString("GridView_SchemaRefreshedTransaction"));
				this.UpdateDesignTimeHtml();
			}
			finally
			{
				Cursor.Current = cursor;
			}
		}

		// Token: 0x06002878 RID: 10360 RVA: 0x000DE758 File Offset: 0x000DD758
		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			if (base.InTemplateMode)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties["Columns"];
				properties["Columns"] = TypeDescriptor.CreateProperty(propertyDescriptor.ComponentType, propertyDescriptor, new Attribute[] { BrowsableAttribute.No });
			}
		}

		// Token: 0x06002879 RID: 10361 RVA: 0x000DE7AC File Offset: 0x000DD7AC
		internal void RemoveField()
		{
			Cursor cursor = Cursor.Current;
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.RemoveFieldCallback), null, SR.GetString("GridView_RemoveFieldTransaction"));
				this.UpdateDesignTimeHtml();
			}
			finally
			{
				Cursor.Current = cursor;
			}
		}

		// Token: 0x0600287A RID: 10362 RVA: 0x000DE80C File Offset: 0x000DD80C
		private bool RemoveFieldCallback(object context)
		{
			int selectedFieldIndex = this.SelectedFieldIndex;
			if (selectedFieldIndex >= 0)
			{
				((GridView)base.Component).Columns.RemoveAt(selectedFieldIndex);
				if (selectedFieldIndex == ((GridView)base.Component).Columns.Count)
				{
					this.SelectedFieldIndex--;
					this.UpdateDesignTimeHtml();
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600287B RID: 10363 RVA: 0x000DE86C File Offset: 0x000DD86C
		private void SaveManipulationSetting(GridViewDesigner.ManipulationMode mode, bool newState)
		{
			DataControlFieldCollection columns = ((GridView)base.Component).Columns;
			bool flag = false;
			ArrayList arrayList = new ArrayList();
			foreach (object obj in columns)
			{
				DataControlField dataControlField = (DataControlField)obj;
				CommandField commandField = dataControlField as CommandField;
				if (commandField != null)
				{
					switch (mode)
					{
					case GridViewDesigner.ManipulationMode.Edit:
						commandField.ShowEditButton = newState;
						break;
					case GridViewDesigner.ManipulationMode.Delete:
						commandField.ShowDeleteButton = newState;
						break;
					case GridViewDesigner.ManipulationMode.Select:
						commandField.ShowSelectButton = newState;
						break;
					}
					if (!newState && !commandField.ShowEditButton && !commandField.ShowDeleteButton && !commandField.ShowInsertButton && !commandField.ShowSelectButton)
					{
						arrayList.Add(commandField);
					}
					flag = true;
				}
			}
			foreach (object obj2 in arrayList)
			{
				columns.Remove((DataControlField)obj2);
			}
			if (!flag && newState)
			{
				CommandField commandField2 = new CommandField();
				switch (mode)
				{
				case GridViewDesigner.ManipulationMode.Edit:
					commandField2.ShowEditButton = newState;
					break;
				case GridViewDesigner.ManipulationMode.Delete:
					commandField2.ShowDeleteButton = newState;
					break;
				case GridViewDesigner.ManipulationMode.Select:
					commandField2.ShowSelectButton = newState;
					break;
				}
				columns.Insert(0, commandField2);
			}
			if (!newState)
			{
				GridView gridView = (GridView)base.Component;
				switch (mode)
				{
				case GridViewDesigner.ManipulationMode.Edit:
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(typeof(GridView))["AutoGenerateEditButton"];
					propertyDescriptor.SetValue(base.Component, newState);
					return;
				}
				case GridViewDesigner.ManipulationMode.Delete:
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(typeof(GridView))["AutoGenerateDeleteButton"];
					propertyDescriptor.SetValue(base.Component, newState);
					return;
				}
				case GridViewDesigner.ManipulationMode.Select:
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(typeof(GridView))["AutoGenerateSelectButton"];
					propertyDescriptor.SetValue(base.Component, newState);
					break;
				}
				default:
					return;
				}
			}
		}

		// Token: 0x0600287C RID: 10364 RVA: 0x000DEA9C File Offset: 0x000DDA9C
		private bool SchemaRefreshedCallback(object context)
		{
			IDataSourceViewSchema dataSourceSchema = this.GetDataSourceSchema();
			if (base.DataSourceID.Length > 0 && dataSourceSchema != null)
			{
				if (((GridView)base.Component).Columns.Count > 0 || ((GridView)base.Component).DataKeyNames.Length > 0)
				{
					if (DialogResult.Yes == UIServiceHelper.ShowMessage(base.Component.Site, SR.GetString("DataBoundControl_SchemaRefreshedWarning", new object[]
					{
						SR.GetString("DataBoundControl_GridView"),
						SR.GetString("DataBoundControl_Column")
					}), SR.GetString("DataBoundControl_SchemaRefreshedCaption", new object[] { ((GridView)base.Component).ID }), MessageBoxButtons.YesNo))
					{
						((GridView)base.Component).DataKeyNames = new string[0];
						((GridView)base.Component).Columns.Clear();
						this.SelectedFieldIndex = -1;
						this.AddKeysAndBoundFields(dataSourceSchema);
					}
				}
				else
				{
					this.AddKeysAndBoundFields(dataSourceSchema);
				}
			}
			else if ((((GridView)base.Component).Columns.Count > 0 || ((GridView)base.Component).DataKeyNames.Length > 0) && DialogResult.Yes == UIServiceHelper.ShowMessage(base.Component.Site, SR.GetString("DataBoundControl_SchemaRefreshedWarningNoDataSource", new object[]
			{
				SR.GetString("DataBoundControl_GridView"),
				SR.GetString("DataBoundControl_Column")
			}), SR.GetString("DataBoundControl_SchemaRefreshedCaption", new object[] { ((GridView)base.Component).ID }), MessageBoxButtons.YesNo))
			{
				((GridView)base.Component).DataKeyNames = new string[0];
				((GridView)base.Component).Columns.Clear();
				this.SelectedFieldIndex = -1;
			}
			return true;
		}

		// Token: 0x0600287D RID: 10365 RVA: 0x000DEC74 File Offset: 0x000DDC74
		public override void SetEditableDesignerRegionContent(EditableDesignerRegion region, string content)
		{
		}

		// Token: 0x0600287E RID: 10366 RVA: 0x000DEC78 File Offset: 0x000DDC78
		private void UpdateFieldsCurrentState()
		{
			this._currentSelectState = ((GridView)base.Component).AutoGenerateSelectButton;
			this._currentEditState = ((GridView)base.Component).AutoGenerateEditButton;
			this._currentDeleteState = ((GridView)base.Component).AutoGenerateDeleteButton;
			foreach (object obj in ((GridView)base.Component).Columns)
			{
				DataControlField dataControlField = (DataControlField)obj;
				CommandField commandField = dataControlField as CommandField;
				if (commandField != null)
				{
					if (commandField.ShowSelectButton)
					{
						this._currentSelectState = true;
					}
					if (commandField.ShowEditButton)
					{
						this._currentEditState = true;
					}
					if (commandField.ShowDeleteButton)
					{
						this._currentDeleteState = true;
					}
				}
			}
		}

		// Token: 0x04001C08 RID: 7176
		private const int IDX_COLUMN_HEADER_TEMPLATE = 3;

		// Token: 0x04001C09 RID: 7177
		private const int IDX_COLUMN_ITEM_TEMPLATE = 0;

		// Token: 0x04001C0A RID: 7178
		private const int IDX_COLUMN_ALTITEM_TEMPLATE = 1;

		// Token: 0x04001C0B RID: 7179
		private const int IDX_COLUMN_EDITITEM_TEMPLATE = 2;

		// Token: 0x04001C0C RID: 7180
		private const int IDX_COLUMN_FOOTER_TEMPLATE = 4;

		// Token: 0x04001C0D RID: 7181
		private const int BASE_INDEX = 1000;

		// Token: 0x04001C0E RID: 7182
		private const int IDX_CONTROL_EMPTY_DATA_TEMPLATE = 0;

		// Token: 0x04001C0F RID: 7183
		private const int IDX_CONTROL_PAGER_TEMPLATE = 1;

		// Token: 0x04001C10 RID: 7184
		private static DesignerAutoFormatCollection _autoFormats;

		// Token: 0x04001C11 RID: 7185
		private static string[] _columnTemplateNames = new string[] { "ItemTemplate", "AlternatingItemTemplate", "EditItemTemplate", "HeaderTemplate", "FooterTemplate" };

		// Token: 0x04001C12 RID: 7186
		private static bool[] _columnTemplateSupportsDataBinding = new bool[] { true, true, true, false, false };

		// Token: 0x04001C13 RID: 7187
		private static string[] _controlTemplateNames = new string[] { "EmptyDataTemplate", "PagerTemplate" };

		// Token: 0x04001C14 RID: 7188
		private static bool[] _controlTemplateSupportsDataBinding = new bool[] { true, true };

		// Token: 0x04001C15 RID: 7189
		private GridViewActionList _actionLists;

		// Token: 0x04001C16 RID: 7190
		private int _regionCount;

		// Token: 0x04001C17 RID: 7191
		private bool _currentEditState;

		// Token: 0x04001C18 RID: 7192
		private bool _currentDeleteState;

		// Token: 0x04001C19 RID: 7193
		private bool _currentSelectState;

		// Token: 0x04001C1A RID: 7194
		internal bool _ignoreSchemaRefreshedEvent;

		// Token: 0x04001C1B RID: 7195
		[CompilerGenerated]
		private static ControlDesigner.CreateAutoFormatDelegate <>9__CachedAnonymousMethodDelegate1;

		// Token: 0x02000452 RID: 1106
		private enum ManipulationMode
		{
			// Token: 0x04001C1D RID: 7197
			Edit,
			// Token: 0x04001C1E RID: 7198
			Delete,
			// Token: 0x04001C1F RID: 7199
			Select
		}
	}
}
