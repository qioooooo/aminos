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
	public class DetailsViewDesigner : DataBoundControlDesigner
	{
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection designerActionListCollection = new DesignerActionListCollection();
				designerActionListCollection.AddRange(base.ActionLists);
				if (this._actionLists == null)
				{
					this._actionLists = new DetailsViewActionList(this);
				}
				bool inTemplateMode = base.InTemplateMode;
				int selectedFieldIndex = this.SelectedFieldIndex;
				this.UpdateFieldsCurrentState();
				this._actionLists.AllowRemoveField = ((DetailsView)base.Component).Fields.Count > 0 && selectedFieldIndex >= 0 && !inTemplateMode;
				this._actionLists.AllowMoveUp = ((DetailsView)base.Component).Fields.Count > 0 && selectedFieldIndex > 0 && !inTemplateMode;
				this._actionLists.AllowMoveDown = ((DetailsView)base.Component).Fields.Count > 0 && selectedFieldIndex >= 0 && ((DetailsView)base.Component).Fields.Count > selectedFieldIndex + 1 && !inTemplateMode;
				DesignerDataSourceView designerView = base.DesignerView;
				this._actionLists.AllowPaging = !inTemplateMode && designerView != null;
				this._actionLists.AllowInserting = !inTemplateMode && designerView != null && designerView.CanInsert;
				this._actionLists.AllowEditing = !inTemplateMode && designerView != null && designerView.CanUpdate;
				this._actionLists.AllowDeleting = !inTemplateMode && designerView != null && designerView.CanDelete;
				designerActionListCollection.Add(this._actionLists);
				return designerActionListCollection;
			}
		}

		public override DesignerAutoFormatCollection AutoFormats
		{
			get
			{
				if (DetailsViewDesigner._autoFormats == null)
				{
					DetailsViewDesigner._autoFormats = ControlDesigner.CreateAutoFormats("<Schemes>\r\n        <xsd:schema id=\"Schemes\" xmlns=\"\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\">\r\n          <xsd:element name=\"Scheme\">\r\n            <xsd:complexType>\r\n              <xsd:all>\r\n                <xsd:element name=\"SchemeName\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"ForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"BackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"BorderColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"BorderWidth\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"BorderStyle\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"GridLines\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"CellPadding\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"CellSpacing\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"RowForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"RowBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"RowFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"AltRowForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"AltRowBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"AltRowFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"CommandRowForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"CommandRowBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"CommandRowFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"FieldHeaderForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"FieldHeaderBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"FieldHeaderFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"EditRowForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"EditRowBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"EditRowFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"HeaderForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"HeaderBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"HeaderFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"FooterForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"FooterBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"FooterFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"PagerForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"PagerBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"PagerFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"PagerAlign\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"PagerButtons\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n              </xsd:all>\r\n            </xsd:complexType>\r\n          </xsd:element>\r\n          <xsd:element name=\"Schemes\" msdata:IsDataSet=\"true\">\r\n            <xsd:complexType>\r\n              <xsd:choice maxOccurs=\"unbounded\">\r\n                <xsd:element ref=\"Scheme\"/>\r\n              </xsd:choice>\r\n            </xsd:complexType>\r\n          </xsd:element>\r\n        </xsd:schema>\r\n        <Scheme>\r\n          <SchemeName>DVScheme_Empty</SchemeName>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>DVScheme_Consistent1</SchemeName>\r\n          <AltRowBackColor>White</AltRowBackColor>\r\n          <GridLines>0</GridLines>\r\n          <CellPadding>4</CellPadding>\r\n          <ForeColor>#333333</ForeColor>\r\n          <RowForeColor>#333333</RowForeColor>\r\n          <RowBackColor>#FFFBD6</RowBackColor>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>#990000</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>White</FooterForeColor>\r\n          <FooterBackColor>#990000</FooterBackColor>\r\n          <FooterFont>1</FooterFont>\r\n          <PagerForeColor>#333333</PagerForeColor>\r\n          <PagerBackColor>#FFCC66</PagerBackColor>\r\n          <PagerAlign>2</PagerAlign>\r\n          <CommandRowBackColor>#FFFFC0</CommandRowBackColor>\r\n          <CommandRowFont>1</CommandRowFont>\r\n          <FieldHeaderFont>1</FieldHeaderFont>\r\n          <FieldHeaderBackColor>#FFFF99</FieldHeaderBackColor>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>DVScheme_Consistent2</SchemeName>\r\n            <AltRowBackColor>White</AltRowBackColor>\r\n            <GridLines>0</GridLines>\r\n            <CellPadding>4</CellPadding>\r\n            <ForeColor>#333333</ForeColor>\r\n            <RowBackColor>#EFF3FB</RowBackColor>\r\n            <HeaderForeColor>White</HeaderForeColor>\r\n            <HeaderBackColor>#507CD1</HeaderBackColor>\r\n            <HeaderFont>1</HeaderFont>\r\n            <FooterForeColor>White</FooterForeColor>\r\n            <FooterBackColor>#507CD1</FooterBackColor>\r\n            <FooterFont>1</FooterFont>\r\n            <PagerForeColor>White</PagerForeColor>\r\n            <PagerBackColor>#2461BF</PagerBackColor>\r\n            <PagerAlign>2</PagerAlign>\r\n            <EditRowBackColor>#2461BF</EditRowBackColor>\r\n            <CommandRowBackColor>#D1DDF1</CommandRowBackColor>\r\n            <CommandRowFont>1</CommandRowFont>\r\n            <FieldHeaderFont>1</FieldHeaderFont>\r\n            <FieldHeaderBackColor>#DEE8F5</FieldHeaderBackColor>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>DVScheme_Consistent3</SchemeName>\r\n            <AltRowBackColor>White</AltRowBackColor>\r\n            <GridLines>0</GridLines>\r\n            <CellPadding>4</CellPadding>\r\n            <ForeColor>#333333</ForeColor>\r\n            <RowBackColor>#E3EAEB</RowBackColor>\r\n            <HeaderForeColor>White</HeaderForeColor>\r\n            <HeaderBackColor>#1C5E55</HeaderBackColor>\r\n            <HeaderFont>1</HeaderFont>\r\n            <FooterForeColor>White</FooterForeColor>\r\n            <FooterBackColor>#1C5E55</FooterBackColor>\r\n            <FooterFont>1</FooterFont>\r\n            <PagerForeColor>White</PagerForeColor>\r\n            <PagerBackColor>#666666</PagerBackColor>\r\n            <PagerAlign>2</PagerAlign>\r\n            <EditRowBackColor>#7C6F57</EditRowBackColor>\r\n            <CommandRowBackColor>#C5BBAF</CommandRowBackColor>\r\n            <CommandRowFont>1</CommandRowFont>\r\n            <FieldHeaderFont>1</FieldHeaderFont>\r\n            <FieldHeaderBackColor>#D0D0D0</FieldHeaderBackColor>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>DVScheme_Consistent4</SchemeName>\r\n            <AltRowBackColor>White</AltRowBackColor>\r\n            <AltRowForeColor>#284775</AltRowForeColor>\r\n            <GridLines>0</GridLines>\r\n            <CellPadding>4</CellPadding>\r\n            <ForeColor>#333333</ForeColor>\r\n            <RowForeColor>#333333</RowForeColor>\r\n            <RowBackColor>#F7F6F3</RowBackColor>\r\n            <HeaderForeColor>White</HeaderForeColor>\r\n            <HeaderBackColor>#5D7B9D</HeaderBackColor>\r\n            <HeaderFont>1</HeaderFont>\r\n            <FooterForeColor>White</FooterForeColor>\r\n            <FooterBackColor>#5D7B9D</FooterBackColor>\r\n            <FooterFont>1</FooterFont>\r\n            <PagerForeColor>White</PagerForeColor>\r\n            <PagerBackColor>#284775</PagerBackColor>\r\n            <PagerAlign>2</PagerAlign>\r\n            <EditRowBackColor>#999999</EditRowBackColor>\r\n            <CommandRowBackColor>#E2DED6</CommandRowBackColor>\r\n            <CommandRowFont>1</CommandRowFont>\r\n            <FieldHeaderFont>1</FieldHeaderFont>\r\n            <FieldHeaderBackColor>#E9ECF1</FieldHeaderBackColor>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>DVScheme_Colorful1</SchemeName>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#CC9966</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>1</BorderStyle>\r\n          <GridLines>3</GridLines>\r\n          <CellPadding>4</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <RowForeColor>#330099</RowForeColor>\r\n          <RowBackColor>White</RowBackColor>\r\n          <EditRowForeColor>#663399</EditRowForeColor>\r\n          <EditRowBackColor>#FFCC66</EditRowBackColor>\r\n          <EditRowFont>1</EditRowFont>\r\n          <HeaderForeColor>#FFFFCC</HeaderForeColor>\r\n          <HeaderBackColor>#990000</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>#330099</FooterForeColor>\r\n          <FooterBackColor>#FFFFCC</FooterBackColor>\r\n          <PagerForeColor>#330099</PagerForeColor>\r\n          <PagerBackColor>#FFFFCC</PagerBackColor>\r\n          <PagerAlign>2</PagerAlign>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>DVScheme_Colorful2</SchemeName>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#3366CC</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>1</BorderStyle>\r\n          <GridLines>3</GridLines>\r\n          <CellPadding>4</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <RowForeColor>#003399</RowForeColor>\r\n          <RowBackColor>White</RowBackColor>\r\n          <EditRowForeColor>#CCFF99</EditRowForeColor>\r\n          <EditRowBackColor>#009999</EditRowBackColor>\r\n          <EditRowFont>1</EditRowFont>\r\n          <HeaderForeColor>#CCCCFF</HeaderForeColor>\r\n          <HeaderBackColor>#003399</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>#003399</FooterForeColor>\r\n          <FooterBackColor>#99CCCC</FooterBackColor>\r\n          <PagerForeColor>#003399</PagerForeColor>\r\n          <PagerBackColor>#99CCCC</PagerBackColor>\r\n          <PagerAlign>1</PagerAlign>\r\n          <PagerButtons>1</PagerButtons>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>DVScheme_Colorful3</SchemeName>\r\n          <BackColor>#DEBA84</BackColor>\r\n          <BorderColor>#DEBA84</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>1</BorderStyle>\r\n          <GridLines>3</GridLines>\r\n          <CellPadding>3</CellPadding>\r\n          <CellSpacing>2</CellSpacing>\r\n          <RowForeColor>#8C4510</RowForeColor>\r\n          <RowBackColor>#FFF7E7</RowBackColor>\r\n          <EditRowForeColor>White</EditRowForeColor>\r\n          <EditRowBackColor>#738A9C</EditRowBackColor>\r\n          <EditRowFont>1</EditRowFont>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>#A55129</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>#8C4510</FooterForeColor>\r\n          <FooterBackColor>#F7DFB5</FooterBackColor>\r\n          <PagerForeColor>#8C4510</PagerForeColor>\r\n          <PagerAlign>2</PagerAlign>\r\n          <PagerButtons>1</PagerButtons>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>DVScheme_Colorful4</SchemeName>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#E7E7FF</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>1</BorderStyle>\r\n          <GridLines>1</GridLines>\r\n          <CellPadding>3</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <RowForeColor>#4A3C8C</RowForeColor>\r\n          <RowBackColor>#E7E7FF</RowBackColor>\r\n          <AltRowBackColor>#F7F7F7</AltRowBackColor>\r\n          <EditRowForeColor>#F7F7F7</EditRowForeColor>\r\n          <EditRowBackColor>#738A9C</EditRowBackColor>\r\n          <EditRowFont>1</EditRowFont>\r\n          <HeaderForeColor>#F7F7F7</HeaderForeColor>\r\n          <HeaderBackColor>#4A3C8C</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>#4A3C8C</FooterForeColor>\r\n          <FooterBackColor>#B5C7DE</FooterBackColor>\r\n          <PagerForeColor>#4A3C8C</PagerForeColor>\r\n          <PagerBackColor>#E7E7FF</PagerBackColor>\r\n          <PagerAlign>3</PagerAlign>\r\n          <PagerButtons>1</PagerButtons>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>DVScheme_Colorful5</SchemeName>\r\n          <ForeColor>Black</ForeColor>\r\n          <BackColor>LightGoldenRodYellow</BackColor>\r\n          <BorderColor>Tan</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <GridLines>0</GridLines>\r\n          <CellPadding>2</CellPadding>\r\n          <AltRowBackColor>PaleGoldenRod</AltRowBackColor>\r\n          <HeaderBackColor>Tan</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterBackColor>Tan</FooterBackColor>\r\n          <EditRowBackColor>DarkSlateBlue</EditRowBackColor>\r\n          <EditRowForeColor>GhostWhite</EditRowForeColor>\r\n          <PagerBackColor>PaleGoldenrod</PagerBackColor>\r\n          <PagerForeColor>DarkSlateBlue</PagerForeColor>\r\n          <PagerAlign>2</PagerAlign>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>DVScheme_Professional1</SchemeName>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#999999</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>1</BorderStyle>\r\n          <GridLines>2</GridLines>\r\n          <CellPadding>3</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <RowForeColor>Black</RowForeColor>\r\n          <RowBackColor>#EEEEEE</RowBackColor>\r\n          <AltRowBackColor>#DCDCDC</AltRowBackColor>\r\n          <EditRowForeColor>White</EditRowForeColor>\r\n          <EditRowBackColor>#008A8C</EditRowBackColor>\r\n          <EditRowFont>1</EditRowFont>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>#000084</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>Black</FooterForeColor>\r\n          <FooterBackColor>#CCCCCC</FooterBackColor>\r\n          <PagerForeColor>Black</PagerForeColor>\r\n          <PagerBackColor>#999999</PagerBackColor>\r\n          <PagerAlign>2</PagerAlign>\r\n          <PagerButtons>1</PagerButtons>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>DVScheme_Professional2</SchemeName>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#CCCCCC</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>1</BorderStyle>\r\n          <GridLines>3</GridLines>\r\n          <CellPadding>3</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <RowForeColor>#000066</RowForeColor>\r\n          <EditRowForeColor>White</EditRowForeColor>\r\n          <EditRowBackColor>#669999</EditRowBackColor>\r\n          <EditRowFont>1</EditRowFont>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>#006699</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>#000066</FooterForeColor>\r\n          <FooterBackColor>White</FooterBackColor>\r\n          <PagerForeColor>#000066</PagerForeColor>\r\n          <PagerBackColor>White</PagerBackColor>\r\n          <PagerAlign>1</PagerAlign>\r\n          <PagerButtons>1</PagerButtons>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>DVScheme_Professional3</SchemeName>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>White</BorderColor>\r\n          <BorderWidth>2px</BorderWidth>\r\n          <BorderStyle>7</BorderStyle>\r\n          <GridLines>0</GridLines>\r\n          <CellPadding>3</CellPadding>\r\n          <CellSpacing>1</CellSpacing>\r\n          <RowForeColor>Black</RowForeColor>\r\n          <RowBackColor>#DEDFDE</RowBackColor>\r\n          <EditRowForeColor>White</EditRowForeColor>\r\n          <EditRowBackColor>#9471DE</EditRowBackColor>\r\n          <EditRowFont>1</EditRowFont>\r\n          <HeaderForeColor>#E7E7FF</HeaderForeColor>\r\n          <HeaderBackColor>#4A3C8C</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>Black</FooterForeColor>\r\n          <FooterBackColor>#C6C3C6</FooterBackColor>\r\n          <PagerForeColor>Black</PagerForeColor>\r\n          <PagerBackColor>#C6C3C6</PagerBackColor>\r\n          <PagerAlign>3</PagerAlign>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>DVScheme_Simple1</SchemeName>\r\n          <ForeColor>Black</ForeColor>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#999999</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>4</BorderStyle>\r\n          <GridLines>2</GridLines>\r\n          <CellPadding>3</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <AltRowBackColor>#CCCCCC</AltRowBackColor>\r\n          <EditRowForeColor>White</EditRowForeColor>\r\n          <EditRowBackColor>#000099</EditRowBackColor>\r\n          <EditRowFont>1</EditRowFont>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>Black</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterBackColor>#CCCCCC</FooterBackColor>\r\n          <PagerForeColor>Black</PagerForeColor>\r\n          <PagerBackColor>#999999</PagerBackColor>\r\n          <PagerAlign>2</PagerAlign>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>DVScheme_Simple2</SchemeName>\r\n          <ForeColor>Black</ForeColor>\r\n          <BackColor>#CCCCCC</BackColor>\r\n          <BorderColor>#999999</BorderColor>\r\n          <BorderWidth>3px</BorderWidth>\r\n          <BorderStyle>4</BorderStyle>\r\n          <GridLines>3</GridLines>\r\n          <CellPadding>4</CellPadding>\r\n          <CellSpacing>2</CellSpacing>\r\n          <RowBackColor>White</RowBackColor>\r\n          <EditRowForeColor>White</EditRowForeColor>\r\n          <EditRowBackColor>#000099</EditRowBackColor>\r\n          <EditRowFont>1</EditRowFont>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>Black</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterBackColor>#CCCCCC</FooterBackColor>\r\n          <PagerForeColor>Black</PagerForeColor>\r\n          <PagerBackColor>#CCCCCC</PagerBackColor>\r\n          <PagerAlign>1</PagerAlign>\r\n          <PagerButtons>1</PagerButtons>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>DVScheme_Simple3</SchemeName>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#336666</BorderColor>\r\n          <BorderWidth>3px</BorderWidth>\r\n          <BorderStyle>5</BorderStyle>\r\n          <GridLines>1</GridLines>\r\n          <CellPadding>4</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <RowForeColor>#333333</RowForeColor>\r\n          <RowBackColor>White</RowBackColor>\r\n          <EditRowForeColor>White</EditRowForeColor>\r\n          <EditRowBackColor>#339966</EditRowBackColor>\r\n          <EditRowFont>1</EditRowFont>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>#336666</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>#333333</FooterForeColor>\r\n          <FooterBackColor>White</FooterBackColor>\r\n          <PagerForeColor>White</PagerForeColor>\r\n          <PagerBackColor>#336666</PagerBackColor>\r\n          <PagerAlign>2</PagerAlign>\r\n          <PagerButtons>1</PagerButtons>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>DVScheme_Classic1</SchemeName>\r\n          <ForeColor>Black</ForeColor>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#CCCCCC</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>1</BorderStyle>\r\n          <GridLines>1</GridLines>\r\n          <CellPadding>4</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <EditRowForeColor>White</EditRowForeColor>\r\n          <EditRowBackColor>#CC3333</EditRowBackColor>\r\n          <EditRowFont>1</EditRowFont>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>#333333</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>Black</FooterForeColor>\r\n          <FooterBackColor>#CCCC99</FooterBackColor>\r\n          <PagerForeColor>Black</PagerForeColor>\r\n          <PagerBackColor>White</PagerBackColor>\r[...string is too long...]", (DataRow schemeData) => new DetailsViewAutoFormat(schemeData));
				}
				return DetailsViewDesigner._autoFormats;
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
					ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.EnableDeletingCallback), value, SR.GetString("DetailsView_EnableDeletingTransaction"));
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
					ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.EnableEditingCallback), value, SR.GetString("DetailsView_EnableEditingTransaction"));
				}
				finally
				{
					Cursor.Current = cursor;
				}
			}
		}

		internal bool EnableInserting
		{
			get
			{
				return this._currentInsertState;
			}
			set
			{
				Cursor cursor = Cursor.Current;
				try
				{
					Cursor.Current = Cursors.WaitCursor;
					ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.EnableInsertingCallback), value, SR.GetString("DetailsView_EnableInsertingTransaction"));
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
				return ((DetailsView)base.Component).AllowPaging;
			}
			set
			{
				Cursor cursor = Cursor.Current;
				try
				{
					Cursor.Current = Cursors.WaitCursor;
					ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.EnablePagingCallback), value, SR.GetString("DetailsView_EnablePagingTransaction"));
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
				return 2;
			}
		}

		private int SelectedFieldIndex
		{
			get
			{
				object obj = base.DesignerState["SelectedFieldIndex"];
				int count = ((DetailsView)base.Component).Fields.Count;
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
				DataControlFieldCollection fields = ((DetailsView)base.Component).Fields;
				int count = fields.Count;
				if (count > 0)
				{
					for (int i = 0; i < count; i++)
					{
						TemplateField templateField = fields[i] as TemplateField;
						if (templateField != null)
						{
							string headerText = fields[i].HeaderText;
							string text = SR.GetString("DetailsView_Field", new object[] { i.ToString(NumberFormatInfo.InvariantInfo) });
							if (headerText != null && headerText.Length != 0)
							{
								text = text + " - " + headerText;
							}
							TemplateGroup templateGroup = new TemplateGroup(text);
							for (int j = 0; j < DetailsViewDesigner._rowTemplateNames.Length; j++)
							{
								string text2 = DetailsViewDesigner._rowTemplateNames[j];
								templateGroup.AddTemplateDefinition(new TemplateDefinition(this, text2, fields[i], text2, this.GetTemplateStyle(j + 1000, templateField))
								{
									SupportsDataBinding = DetailsViewDesigner._rowTemplateSupportsDataBinding[j]
								});
							}
							templateGroups.Add(templateGroup);
						}
					}
				}
				for (int k = 0; k < DetailsViewDesigner._controlTemplateNames.Length; k++)
				{
					string text3 = DetailsViewDesigner._controlTemplateNames[k];
					TemplateGroup templateGroup2 = new TemplateGroup(DetailsViewDesigner._controlTemplateNames[k], this.GetTemplateStyle(k, null));
					templateGroup2.AddTemplateDefinition(new TemplateDefinition(this, text3, base.Component, text3)
					{
						SupportsDataBinding = DetailsViewDesigner._controlTemplateSupportsDataBinding[k]
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
			DataControlFieldCollection fields = ((DetailsView)base.Component).Fields;
			if (schema != null)
			{
				IDataSourceFieldSchema[] fields2 = schema.GetFields();
				if (fields2 != null && fields2.Length > 0)
				{
					ArrayList arrayList = new ArrayList();
					foreach (IDataSourceFieldSchema dataSourceFieldSchema in fields2)
					{
						if (((DetailsView)base.Component).IsBindableType(dataSourceFieldSchema.DataType))
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
							fields.Add(boundField);
						}
					}
					((DetailsView)base.Component).AutoGenerateRows = false;
					int count = arrayList.Count;
					if (count > 0)
					{
						string[] array2 = new string[count];
						arrayList.CopyTo(array2, 0);
						((DetailsView)base.Component).DataKeyNames = array2;
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
				ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.AddNewFieldChangeCallback), null, SR.GetString("DetailsView_AddNewFieldTransaction"));
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
			base.DataBind(dataBoundControl);
			DetailsView detailsView = (DetailsView)dataBoundControl;
			Table table = detailsView.Controls[0] as Table;
			int num = 0;
			int num2 = 1;
			int num3 = 1;
			int num4 = 0;
			if (detailsView.AllowPaging)
			{
				if (detailsView.PagerSettings.Position == PagerPosition.TopAndBottom)
				{
					num4 = 2;
				}
				else
				{
					num4 = 1;
				}
			}
			if (detailsView.AutoGenerateRows)
			{
				int num5 = 0;
				if (detailsView.AutoGenerateInsertButton || detailsView.AutoGenerateDeleteButton || detailsView.AutoGenerateEditButton)
				{
					num5 = 1;
				}
				int count = table.Rows.Count;
				num = count - detailsView.Fields.Count - num5 - num2 - num3 - num4;
			}
			this.SetRegionAttributes(num);
		}

		internal void EditFields()
		{
			Cursor cursor = Cursor.Current;
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				this._ignoreSchemaRefreshedEvent = true;
				ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.EditFieldsChangeCallback), null, SR.GetString("DetailsView_EditFieldsTransaction"));
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
			this.SaveManipulationSetting(DetailsViewDesigner.ManipulationMode.Delete, flag);
			return true;
		}

		private bool EnableEditingCallback(object context)
		{
			bool flag = !this._currentEditState;
			if (context is bool)
			{
				flag = (bool)context;
			}
			this.SaveManipulationSetting(DetailsViewDesigner.ManipulationMode.Edit, flag);
			return true;
		}

		private bool EnableInsertingCallback(object context)
		{
			bool flag = !this._currentInsertState;
			if (context is bool)
			{
				flag = (bool)context;
			}
			this.SaveManipulationSetting(DetailsViewDesigner.ManipulationMode.Insert, flag);
			return true;
		}

		private bool EnablePagingCallback(object context)
		{
			bool allowPaging = ((DetailsView)base.Component).AllowPaging;
			bool flag = !allowPaging;
			if (context is bool)
			{
				flag = (bool)context;
			}
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(typeof(DetailsView))["AllowPaging"];
			propertyDescriptor.SetValue(base.Component, flag);
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
			DetailsView detailsView = (DetailsView)base.ViewControl;
			if (detailsView.Fields.Count == 0)
			{
				detailsView.AutoGenerateRows = true;
			}
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
				detailsView.DataKeyNames = new string[0];
			}
			TypeDescriptor.Refresh(base.Component);
			return base.GetDesignTimeHtml();
		}

		public override string GetDesignTimeHtml(DesignerRegionCollection regions)
		{
			string designTimeHtml = this.GetDesignTimeHtml();
			DetailsView detailsView = (DetailsView)base.ViewControl;
			int count = detailsView.Rows.Count;
			int selectedFieldIndex = this.SelectedFieldIndex;
			DetailsViewRowCollection rows = detailsView.Rows;
			for (int i = 0; i < detailsView.Fields.Count; i++)
			{
				string text = SR.GetString("DetailsView_Field", new object[] { i.ToString(NumberFormatInfo.InvariantInfo) });
				string headerText = detailsView.Fields[i].HeaderText;
				if (headerText.Length == 0)
				{
					text = text + " - " + headerText;
				}
				if (i < count)
				{
					DetailsViewRow detailsViewRow = rows[i];
					for (int j = 0; j < detailsViewRow.Cells.Count; j++)
					{
						TableCell tableCell = detailsViewRow.Cells[j];
						if (j == 0)
						{
							DesignerRegion designerRegion = new DesignerRegion(this, text, true);
							designerRegion.UserData = i;
							if (i == selectedFieldIndex)
							{
								designerRegion.Highlight = true;
							}
							regions.Add(designerRegion);
						}
						else
						{
							DesignerRegion designerRegion2 = new DesignerRegion(this, i.ToString(NumberFormatInfo.InvariantInfo), false);
							designerRegion2.UserData = -1;
							if (i == selectedFieldIndex)
							{
								designerRegion2.Highlight = true;
							}
							regions.Add(designerRegion2);
						}
					}
				}
			}
			return designTimeHtml;
		}

		private Style GetTemplateStyle(int templateIndex, TemplateField templateField)
		{
			Style style = new Style();
			style.CopyFrom(((DetailsView)base.ViewControl).ControlStyle);
			switch (templateIndex)
			{
			case 0:
				style.CopyFrom(((DetailsView)base.ViewControl).FooterStyle);
				break;
			case 1:
				style.CopyFrom(((DetailsView)base.ViewControl).HeaderStyle);
				break;
			case 2:
				style.CopyFrom(((DetailsView)base.ViewControl).EmptyDataRowStyle);
				break;
			case 3:
				style.CopyFrom(((DetailsView)base.ViewControl).PagerStyle);
				break;
			default:
				switch (templateIndex)
				{
				case 1000:
					style.CopyFrom(((DetailsView)base.ViewControl).RowStyle);
					style.CopyFrom(templateField.ItemStyle);
					break;
				case 1001:
					style.CopyFrom(((DetailsView)base.ViewControl).RowStyle);
					style.CopyFrom(((DetailsView)base.ViewControl).AlternatingRowStyle);
					style.CopyFrom(templateField.ItemStyle);
					break;
				case 1002:
					style.CopyFrom(((DetailsView)base.ViewControl).RowStyle);
					style.CopyFrom(((DetailsView)base.ViewControl).EditRowStyle);
					style.CopyFrom(templateField.ItemStyle);
					break;
				case 1003:
					style.CopyFrom(((DetailsView)base.ViewControl).RowStyle);
					style.CopyFrom(((DetailsView)base.ViewControl).InsertRowStyle);
					style.CopyFrom(templateField.ItemStyle);
					break;
				case 1004:
					style.CopyFrom(((DetailsView)base.ViewControl).HeaderStyle);
					style.CopyFrom(templateField.HeaderStyle);
					break;
				}
				break;
			}
			return style;
		}

		public override string GetEditableDesignerRegionContent(EditableDesignerRegion region)
		{
			return string.Empty;
		}

		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(DetailsView));
			base.Initialize(component);
			if (base.View != null)
			{
				base.View.SetFlags(ViewFlags.TemplateEditing, true);
			}
		}

		internal void MoveDown()
		{
			Cursor cursor = Cursor.Current;
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.MoveDownCallback), null, SR.GetString("DetailsView_MoveDownTransaction"));
				this.UpdateDesignTimeHtml();
			}
			finally
			{
				Cursor.Current = cursor;
			}
		}

		private bool MoveDownCallback(object context)
		{
			DataControlFieldCollection fields = ((DetailsView)base.Component).Fields;
			int selectedFieldIndex = this.SelectedFieldIndex;
			if (selectedFieldIndex >= 0 && fields.Count > selectedFieldIndex + 1)
			{
				DataControlField dataControlField = fields[selectedFieldIndex];
				fields.RemoveAt(selectedFieldIndex);
				fields.Insert(selectedFieldIndex + 1, dataControlField);
				this.SelectedFieldIndex++;
				this.UpdateDesignTimeHtml();
				return true;
			}
			return false;
		}

		internal void MoveUp()
		{
			Cursor cursor = Cursor.Current;
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.MoveUpCallback), null, SR.GetString("DetailsView_MoveUpTransaction"));
				this.UpdateDesignTimeHtml();
			}
			finally
			{
				Cursor.Current = cursor;
			}
		}

		private bool MoveUpCallback(object context)
		{
			DataControlFieldCollection fields = ((DetailsView)base.Component).Fields;
			int selectedFieldIndex = this.SelectedFieldIndex;
			if (selectedFieldIndex > 0)
			{
				DataControlField dataControlField = fields[selectedFieldIndex];
				fields.RemoveAt(selectedFieldIndex);
				fields.Insert(selectedFieldIndex - 1, dataControlField);
				this.SelectedFieldIndex--;
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
				ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.SchemaRefreshedCallback), null, SR.GetString("DataControls_SchemaRefreshedTransaction"));
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
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties["Fields"];
				properties["Fields"] = TypeDescriptor.CreateProperty(propertyDescriptor.ComponentType, propertyDescriptor, new Attribute[] { BrowsableAttribute.No });
			}
		}

		private void SaveManipulationSetting(DetailsViewDesigner.ManipulationMode mode, bool newState)
		{
			DataControlFieldCollection fields = ((DetailsView)base.Component).Fields;
			bool flag = false;
			ArrayList arrayList = new ArrayList();
			foreach (object obj in fields)
			{
				DataControlField dataControlField = (DataControlField)obj;
				CommandField commandField = dataControlField as CommandField;
				if (commandField != null)
				{
					switch (mode)
					{
					case DetailsViewDesigner.ManipulationMode.Edit:
						commandField.ShowEditButton = newState;
						break;
					case DetailsViewDesigner.ManipulationMode.Delete:
						commandField.ShowDeleteButton = newState;
						break;
					case DetailsViewDesigner.ManipulationMode.Insert:
						commandField.ShowInsertButton = newState;
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
				fields.Remove((DataControlField)obj2);
			}
			if (!flag && newState)
			{
				CommandField commandField2 = new CommandField();
				switch (mode)
				{
				case DetailsViewDesigner.ManipulationMode.Edit:
					commandField2.ShowEditButton = newState;
					break;
				case DetailsViewDesigner.ManipulationMode.Delete:
					commandField2.ShowDeleteButton = newState;
					break;
				case DetailsViewDesigner.ManipulationMode.Insert:
					commandField2.ShowInsertButton = newState;
					break;
				}
				fields.Add(commandField2);
			}
			if (!newState)
			{
				DetailsView detailsView = (DetailsView)base.Component;
				switch (mode)
				{
				case DetailsViewDesigner.ManipulationMode.Edit:
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(typeof(DetailsView))["AutoGenerateEditButton"];
					propertyDescriptor.SetValue(base.Component, newState);
					return;
				}
				case DetailsViewDesigner.ManipulationMode.Delete:
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(typeof(DetailsView))["AutoGenerateDeleteButton"];
					propertyDescriptor.SetValue(base.Component, newState);
					return;
				}
				case DetailsViewDesigner.ManipulationMode.Insert:
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(typeof(DetailsView))["AutoGenerateInsertButton"];
					propertyDescriptor.SetValue(base.Component, newState);
					break;
				}
				default:
					return;
				}
			}
		}

		private bool RemoveCallback(object context)
		{
			int selectedFieldIndex = this.SelectedFieldIndex;
			if (selectedFieldIndex >= 0)
			{
				((DetailsView)base.Component).Fields.RemoveAt(selectedFieldIndex);
				if (selectedFieldIndex == ((DetailsView)base.Component).Fields.Count)
				{
					this.SelectedFieldIndex--;
					this.UpdateDesignTimeHtml();
				}
				return true;
			}
			return false;
		}

		internal void RemoveField()
		{
			Cursor cursor = Cursor.Current;
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.RemoveCallback), null, SR.GetString("DetailsView_RemoveFieldTransaction"));
				this.UpdateDesignTimeHtml();
			}
			finally
			{
				Cursor.Current = cursor;
			}
		}

		private bool SchemaRefreshedCallback(object context)
		{
			IDataSourceViewSchema dataSourceSchema = this.GetDataSourceSchema();
			if (base.DataSourceID.Length > 0 && dataSourceSchema != null)
			{
				if (((DetailsView)base.Component).Fields.Count > 0 || ((DetailsView)base.Component).DataKeyNames.Length > 0)
				{
					if (DialogResult.Yes == UIServiceHelper.ShowMessage(base.Component.Site, SR.GetString("DataBoundControl_SchemaRefreshedWarning", new object[]
					{
						SR.GetString("DataBoundControl_DetailsView"),
						SR.GetString("DataBoundControl_Row")
					}), SR.GetString("DataBoundControl_SchemaRefreshedCaption", new object[] { ((DetailsView)base.Component).ID }), MessageBoxButtons.YesNo))
					{
						((DetailsView)base.Component).DataKeyNames = new string[0];
						((DetailsView)base.Component).Fields.Clear();
						this.SelectedFieldIndex = -1;
						this.AddKeysAndBoundFields(dataSourceSchema);
					}
				}
				else
				{
					this.AddKeysAndBoundFields(dataSourceSchema);
				}
			}
			else if ((((DetailsView)base.Component).Fields.Count > 0 || ((DetailsView)base.Component).DataKeyNames.Length > 0) && DialogResult.Yes == UIServiceHelper.ShowMessage(base.Component.Site, SR.GetString("DataBoundControl_SchemaRefreshedWarningNoDataSource", new object[]
			{
				SR.GetString("DataBoundControl_DetailsView"),
				SR.GetString("DataBoundControl_Row")
			}), SR.GetString("DataBoundControl_SchemaRefreshedCaption", new object[] { ((DetailsView)base.Component).ID }), MessageBoxButtons.YesNo))
			{
				((DetailsView)base.Component).DataKeyNames = new string[0];
				((DetailsView)base.Component).Fields.Clear();
				this.SelectedFieldIndex = -1;
			}
			return true;
		}

		public override void SetEditableDesignerRegionContent(EditableDesignerRegion region, string content)
		{
		}

		private void SetRegionAttributes(int autoGeneratedRows)
		{
			int num = 0;
			DetailsView detailsView = (DetailsView)base.ViewControl;
			Table table = detailsView.Controls[0] as Table;
			if (table != null)
			{
				int num2 = 0;
				if (detailsView.AllowPaging && detailsView.PagerSettings.Position != PagerPosition.Bottom)
				{
					num2 = 1;
				}
				int num3 = autoGeneratedRows + 1 + num2;
				TableRowCollection rows = table.Rows;
				int num4 = num3;
				while (num4 < detailsView.Fields.Count + num3 && num4 < rows.Count)
				{
					TableRow tableRow = rows[num4];
					foreach (object obj in tableRow.Cells)
					{
						TableCell tableCell = (TableCell)obj;
						tableCell.Attributes[DesignerRegion.DesignerRegionAttributeName] = num.ToString(NumberFormatInfo.InvariantInfo);
						num++;
					}
					num4++;
				}
			}
		}

		private void UpdateFieldsCurrentState()
		{
			this._currentInsertState = ((DetailsView)base.Component).AutoGenerateInsertButton;
			this._currentEditState = ((DetailsView)base.Component).AutoGenerateEditButton;
			this._currentDeleteState = ((DetailsView)base.Component).AutoGenerateDeleteButton;
			foreach (object obj in ((DetailsView)base.Component).Fields)
			{
				DataControlField dataControlField = (DataControlField)obj;
				CommandField commandField = dataControlField as CommandField;
				if (commandField != null)
				{
					if (commandField.ShowInsertButton)
					{
						this._currentInsertState = true;
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

		private const int IDX_ROW_HEADER_TEMPLATE = 4;

		private const int IDX_ROW_ITEM_TEMPLATE = 0;

		private const int IDX_ROW_ALTITEM_TEMPLATE = 1;

		private const int IDX_ROW_EDITITEM_TEMPLATE = 2;

		private const int IDX_ROW_INSERTITEM_TEMPLATE = 3;

		private const int BASE_INDEX = 1000;

		private const int IDX_CONTROL_HEADER_TEMPLATE = 1;

		private const int IDX_CONTROL_FOOTER_TEMPLATE = 0;

		private const int IDX_CONTROL_EMPTY_DATA_TEMPLATE = 2;

		private const int IDX_CONTROL_PAGER_TEMPLATE = 3;

		private static DesignerAutoFormatCollection _autoFormats;

		private static string[] _rowTemplateNames = new string[] { "ItemTemplate", "AlternatingItemTemplate", "EditItemTemplate", "InsertItemTemplate", "HeaderTemplate" };

		private static bool[] _rowTemplateSupportsDataBinding = new bool[] { true, true, true, true, false };

		private static string[] _controlTemplateNames = new string[] { "FooterTemplate", "HeaderTemplate", "EmptyDataTemplate", "PagerTemplate" };

		private static bool[] _controlTemplateSupportsDataBinding = new bool[] { true, true, true, true };

		private DetailsViewActionList _actionLists;

		private bool _currentEditState;

		private bool _currentDeleteState;

		private bool _currentInsertState;

		internal bool _ignoreSchemaRefreshedEvent;

		[CompilerGenerated]
		private static ControlDesigner.CreateAutoFormatDelegate <>9__CachedAnonymousMethodDelegate1;

		private enum ManipulationMode
		{
			Edit,
			Delete,
			Insert
		}
	}
}
