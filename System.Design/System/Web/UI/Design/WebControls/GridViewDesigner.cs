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
	public class GridViewDesigner : DataBoundControlDesigner
	{
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

		protected override bool UsePreviewControl
		{
			get
			{
				return true;
			}
		}

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

		public override string GetEditableDesignerRegionContent(EditableDesignerRegion region)
		{
			return string.Empty;
		}

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

		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(GridView));
			base.Initialize(component);
			if (base.View != null)
			{
				base.View.SetFlags(ViewFlags.TemplateEditing, true);
			}
		}

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

		protected override void OnClick(DesignerRegionMouseEventArgs e)
		{
			if (e.Region != null)
			{
				this.SelectedFieldIndex = (int)e.Region.UserData;
				this.UpdateDesignTimeHtml();
			}
		}

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

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			if (base.InTemplateMode)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties["Columns"];
				properties["Columns"] = TypeDescriptor.CreateProperty(propertyDescriptor.ComponentType, propertyDescriptor, new Attribute[] { BrowsableAttribute.No });
			}
		}

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

		public override void SetEditableDesignerRegionContent(EditableDesignerRegion region, string content)
		{
		}

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

		private const int IDX_COLUMN_HEADER_TEMPLATE = 3;

		private const int IDX_COLUMN_ITEM_TEMPLATE = 0;

		private const int IDX_COLUMN_ALTITEM_TEMPLATE = 1;

		private const int IDX_COLUMN_EDITITEM_TEMPLATE = 2;

		private const int IDX_COLUMN_FOOTER_TEMPLATE = 4;

		private const int BASE_INDEX = 1000;

		private const int IDX_CONTROL_EMPTY_DATA_TEMPLATE = 0;

		private const int IDX_CONTROL_PAGER_TEMPLATE = 1;

		private static DesignerAutoFormatCollection _autoFormats;

		private static string[] _columnTemplateNames = new string[] { "ItemTemplate", "AlternatingItemTemplate", "EditItemTemplate", "HeaderTemplate", "FooterTemplate" };

		private static bool[] _columnTemplateSupportsDataBinding = new bool[] { true, true, true, false, false };

		private static string[] _controlTemplateNames = new string[] { "EmptyDataTemplate", "PagerTemplate" };

		private static bool[] _controlTemplateSupportsDataBinding = new bool[] { true, true };

		private GridViewActionList _actionLists;

		private int _regionCount;

		private bool _currentEditState;

		private bool _currentDeleteState;

		private bool _currentSelectState;

		internal bool _ignoreSchemaRefreshedEvent;

		[CompilerGenerated]
		private static ControlDesigner.CreateAutoFormatDelegate <>9__CachedAnonymousMethodDelegate1;

		private enum ManipulationMode
		{
			Edit,
			Delete,
			Select
		}
	}
}
