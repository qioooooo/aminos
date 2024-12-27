using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Design;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000442 RID: 1090
	[SupportsPreviewControl(true)]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class DataListDesigner : BaseDataListDesigner
	{
		// Token: 0x0600276C RID: 10092 RVA: 0x000D79A7 File Offset: 0x000D69A7
		public DataListDesigner()
		{
			this.templateVerbsDirty = true;
		}

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x0600276D RID: 10093 RVA: 0x000D79B6 File Offset: 0x000D69B6
		public override bool AllowResize
		{
			get
			{
				return this.TemplatesExist || base.InTemplateModeInternal;
			}
		}

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x0600276E RID: 10094 RVA: 0x000D79D0 File Offset: 0x000D69D0
		public override DesignerAutoFormatCollection AutoFormats
		{
			get
			{
				if (DataListDesigner._autoFormats == null)
				{
					DataListDesigner._autoFormats = ControlDesigner.CreateAutoFormats("<Schemes>\r\n        <xsd:schema id=\"Schemes\" xmlns=\"\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\">\r\n          <xsd:element name=\"Scheme\">\r\n            <xsd:complexType>\r\n              <xsd:all>\r\n                <xsd:element name=\"SchemeName\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"ForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"BackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"BorderColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"BorderWidth\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"BorderStyle\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"GridLines\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"CellPadding\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"CellSpacing\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"ItemForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"ItemBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"ItemFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"AltItemForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"AltItemBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"AltItemFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"SelItemForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"SelItemBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"SelItemFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"HeaderForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"HeaderBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"HeaderFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"FooterForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"FooterBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"FooterFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"PagerForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"PagerBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"PagerFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"PagerAlign\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"PagerMode\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"EditItemForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"EditItemBackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n                <xsd:element name=\"EditItemFont\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n              </xsd:all>\r\n            </xsd:complexType>\r\n          </xsd:element>\r\n          <xsd:element name=\"Schemes\" msdata:IsDataSet=\"true\">\r\n            <xsd:complexType>\r\n              <xsd:choice maxOccurs=\"unbounded\">\r\n                <xsd:element ref=\"Scheme\"/>\r\n              </xsd:choice>\r\n            </xsd:complexType>\r\n          </xsd:element>\r\n        </xsd:schema>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Empty</SchemeName>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Consistent1</SchemeName>\r\n          <AltItemBackColor>White</AltItemBackColor>\r\n          <GridLines>0</GridLines>\r\n          <CellPadding>4</CellPadding>\r\n          <ForeColor>#333333</ForeColor>\r\n          <ItemForeColor>#333333</ItemForeColor>\r\n          <ItemBackColor>#FFFBD6</ItemBackColor>\r\n          <SelItemForeColor>Navy</SelItemForeColor>\r\n          <SelItemBackColor>#FFCC66</SelItemBackColor>\r\n          <SelItemFont>1</SelItemFont>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>#990000</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>White</FooterForeColor>\r\n          <FooterBackColor>#990000</FooterBackColor>\r\n          <FooterFont>1</FooterFont>\r\n          <PagerForeColor>#333333</PagerForeColor>\r\n          <PagerBackColor>#FFCC66</PagerBackColor>\r\n          <PagerAlign>2</PagerAlign>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Consistent2</SchemeName>\r\n            <AltItemBackColor>White</AltItemBackColor>\r\n            <GridLines>0</GridLines>\r\n            <CellPadding>4</CellPadding>\r\n            <ForeColor>#333333</ForeColor>\r\n            <ItemBackColor>#EFF3FB</ItemBackColor>\r\n            <SelItemForeColor>#333333</SelItemForeColor>\r\n            <SelItemBackColor>#D1DDF1</SelItemBackColor>\r\n            <SelItemFont>1</SelItemFont>\r\n            <HeaderForeColor>White</HeaderForeColor>\r\n            <HeaderBackColor>#507CD1</HeaderBackColor>\r\n            <HeaderFont>1</HeaderFont>\r\n            <FooterForeColor>White</FooterForeColor>\r\n            <FooterBackColor>#507CD1</FooterBackColor>\r\n            <FooterFont>1</FooterFont>\r\n            <PagerForeColor>White</PagerForeColor>\r\n            <PagerBackColor>#2461BF</PagerBackColor>\r\n            <PagerAlign>2</PagerAlign>\r\n            <EditItemBackColor>#2461BF</EditItemBackColor>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Consistent3</SchemeName>\r\n            <AltItemBackColor>White</AltItemBackColor>\r\n            <GridLines>0</GridLines>\r\n            <CellPadding>4</CellPadding>\r\n            <ForeColor>#333333</ForeColor>\r\n            <ItemBackColor>#E3EAEB</ItemBackColor>\r\n            <SelItemForeColor>#333333</SelItemForeColor>\r\n            <SelItemBackColor>#C5BBAF</SelItemBackColor>\r\n            <SelItemFont>1</SelItemFont>\r\n            <HeaderForeColor>White</HeaderForeColor>\r\n            <HeaderBackColor>#1C5E55</HeaderBackColor>\r\n            <HeaderFont>1</HeaderFont>\r\n            <FooterForeColor>White</FooterForeColor>\r\n            <FooterBackColor>#1C5E55</FooterBackColor>\r\n            <FooterFont>1</FooterFont>\r\n            <PagerForeColor>White</PagerForeColor>\r\n            <PagerBackColor>#666666</PagerBackColor>\r\n            <PagerAlign>2</PagerAlign>\r\n            <EditItemBackColor>#7C6F57</EditItemBackColor>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Consistent4</SchemeName>\r\n            <AltItemBackColor>White</AltItemBackColor>\r\n            <AltItemForeColor>#284775</AltItemForeColor>\r\n            <GridLines>0</GridLines>\r\n            <CellPadding>4</CellPadding>\r\n            <ForeColor>#333333</ForeColor>\r\n            <ItemForeColor>#333333</ItemForeColor>\r\n            <ItemBackColor>#F7F6F3</ItemBackColor>\r\n            <SelItemForeColor>#333333</SelItemForeColor>\r\n            <SelItemBackColor>#E2DED6</SelItemBackColor>\r\n            <SelItemFont>1</SelItemFont>\r\n            <HeaderForeColor>White</HeaderForeColor>\r\n            <HeaderBackColor>#5D7B9D</HeaderBackColor>\r\n            <HeaderFont>1</HeaderFont>\r\n            <FooterForeColor>White</FooterForeColor>\r\n            <FooterBackColor>#5D7B9D</FooterBackColor>\r\n            <FooterFont>1</FooterFont>\r\n            <PagerForeColor>White</PagerForeColor>\r\n            <PagerBackColor>#284775</PagerBackColor>\r\n            <PagerAlign>2</PagerAlign>\r\n            <EditItemBackColor>#999999</EditItemBackColor>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Colorful1</SchemeName>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#CC9966</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>1</BorderStyle>\r\n          <GridLines>3</GridLines>\r\n          <CellPadding>4</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <ItemForeColor>#330099</ItemForeColor>\r\n          <ItemBackColor>White</ItemBackColor>\r\n          <SelItemForeColor>#663399</SelItemForeColor>\r\n          <SelItemBackColor>#FFCC66</SelItemBackColor>\r\n          <SelItemFont>1</SelItemFont>\r\n          <HeaderForeColor>#FFFFCC</HeaderForeColor>\r\n          <HeaderBackColor>#990000</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>#330099</FooterForeColor>\r\n          <FooterBackColor>#FFFFCC</FooterBackColor>\r\n          <PagerForeColor>#330099</PagerForeColor>\r\n          <PagerBackColor>#FFFFCC</PagerBackColor>\r\n          <PagerAlign>2</PagerAlign>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Colorful2</SchemeName>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#3366CC</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>1</BorderStyle>\r\n          <GridLines>3</GridLines>\r\n          <CellPadding>4</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <ItemForeColor>#003399</ItemForeColor>\r\n          <ItemBackColor>White</ItemBackColor>\r\n          <SelItemForeColor>#CCFF99</SelItemForeColor>\r\n          <SelItemBackColor>#009999</SelItemBackColor>\r\n          <SelItemFont>1</SelItemFont>\r\n          <HeaderForeColor>#CCCCFF</HeaderForeColor>\r\n          <HeaderBackColor>#003399</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>#003399</FooterForeColor>\r\n          <FooterBackColor>#99CCCC</FooterBackColor>\r\n          <PagerForeColor>#003399</PagerForeColor>\r\n          <PagerBackColor>#99CCCC</PagerBackColor>\r\n          <PagerAlign>1</PagerAlign>\r\n          <PagerMode>1</PagerMode>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Colorful3</SchemeName>\r\n          <BackColor>#DEBA84</BackColor>\r\n          <BorderColor>#DEBA84</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>1</BorderStyle>\r\n          <GridLines>3</GridLines>\r\n          <CellPadding>3</CellPadding>\r\n          <CellSpacing>2</CellSpacing>\r\n          <ItemForeColor>#8C4510</ItemForeColor>\r\n          <ItemBackColor>#FFF7E7</ItemBackColor>\r\n          <SelItemForeColor>White</SelItemForeColor>\r\n          <SelItemBackColor>#738A9C</SelItemBackColor>\r\n          <SelItemFont>1</SelItemFont>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>#A55129</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>#8C4510</FooterForeColor>\r\n          <FooterBackColor>#F7DFB5</FooterBackColor>\r\n          <PagerForeColor>#8C4510</PagerForeColor>\r\n          <PagerAlign>2</PagerAlign>\r\n          <PagerMode>1</PagerMode>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Colorful4</SchemeName>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#E7E7FF</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>1</BorderStyle>\r\n          <GridLines>1</GridLines>\r\n          <CellPadding>3</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <ItemForeColor>#4A3C8C</ItemForeColor>\r\n          <ItemBackColor>#E7E7FF</ItemBackColor>\r\n          <AltItemBackColor>#F7F7F7</AltItemBackColor>\r\n          <SelItemForeColor>#F7F7F7</SelItemForeColor>\r\n          <SelItemBackColor>#738A9C</SelItemBackColor>\r\n          <SelItemFont>1</SelItemFont>\r\n          <HeaderForeColor>#F7F7F7</HeaderForeColor>\r\n          <HeaderBackColor>#4A3C8C</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>#4A3C8C</FooterForeColor>\r\n          <FooterBackColor>#B5C7DE</FooterBackColor>\r\n          <PagerForeColor>#4A3C8C</PagerForeColor>\r\n          <PagerBackColor>#E7E7FF</PagerBackColor>\r\n          <PagerAlign>3</PagerAlign>\r\n          <PagerMode>1</PagerMode>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Colorful5</SchemeName>\r\n          <ForeColor>Black</ForeColor>\r\n          <BackColor>LightGoldenRodYellow</BackColor>\r\n          <BorderColor>Tan</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <GridLines>0</GridLines>\r\n          <CellPadding>2</CellPadding>\r\n          <AltItemBackColor>PaleGoldenRod</AltItemBackColor>\r\n          <HeaderBackColor>Tan</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterBackColor>Tan</FooterBackColor>\r\n          <SelItemBackColor>DarkSlateBlue</SelItemBackColor>\r\n          <SelItemForeColor>GhostWhite</SelItemForeColor>\r\n          <PagerBackColor>PaleGoldenrod</PagerBackColor>\r\n          <PagerForeColor>DarkSlateBlue</PagerForeColor>\r\n          <PagerAlign>2</PagerAlign>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Professional1</SchemeName>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#999999</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>1</BorderStyle>\r\n          <GridLines>2</GridLines>\r\n          <CellPadding>3</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <ItemForeColor>Black</ItemForeColor>\r\n          <ItemBackColor>#EEEEEE</ItemBackColor>\r\n          <AltItemBackColor>#DCDCDC</AltItemBackColor>\r\n          <SelItemForeColor>White</SelItemForeColor>\r\n          <SelItemBackColor>#008A8C</SelItemBackColor>\r\n          <SelItemFont>1</SelItemFont>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>#000084</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>Black</FooterForeColor>\r\n          <FooterBackColor>#CCCCCC</FooterBackColor>\r\n          <PagerForeColor>Black</PagerForeColor>\r\n          <PagerBackColor>#999999</PagerBackColor>\r\n          <PagerAlign>2</PagerAlign>\r\n          <PagerMode>1</PagerMode>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Professional2</SchemeName>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#CCCCCC</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>1</BorderStyle>\r\n          <GridLines>3</GridLines>\r\n          <CellPadding>3</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <ItemForeColor>#000066</ItemForeColor>\r\n          <SelItemForeColor>White</SelItemForeColor>\r\n          <SelItemBackColor>#669999</SelItemBackColor>\r\n          <SelItemFont>1</SelItemFont>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>#006699</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>#000066</FooterForeColor>\r\n          <FooterBackColor>White</FooterBackColor>\r\n          <PagerForeColor>#000066</PagerForeColor>\r\n          <PagerBackColor>White</PagerBackColor>\r\n          <PagerAlign>1</PagerAlign>\r\n          <PagerMode>1</PagerMode>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Professional3</SchemeName>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>White</BorderColor>\r\n          <BorderWidth>2px</BorderWidth>\r\n          <BorderStyle>7</BorderStyle>\r\n          <GridLines>0</GridLines>\r\n          <CellPadding>3</CellPadding>\r\n          <CellSpacing>1</CellSpacing>\r\n          <ItemForeColor>Black</ItemForeColor>\r\n          <ItemBackColor>#DEDFDE</ItemBackColor>\r\n          <SelItemForeColor>White</SelItemForeColor>\r\n          <SelItemBackColor>#9471DE</SelItemBackColor>\r\n          <SelItemFont>1</SelItemFont>\r\n          <HeaderForeColor>#E7E7FF</HeaderForeColor>\r\n          <HeaderBackColor>#4A3C8C</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>Black</FooterForeColor>\r\n          <FooterBackColor>#C6C3C6</FooterBackColor>\r\n          <PagerForeColor>Black</PagerForeColor>\r\n          <PagerBackColor>#C6C3C6</PagerBackColor>\r\n          <PagerAlign>3</PagerAlign>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Simple1</SchemeName>\r\n          <ForeColor>Black</ForeColor>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#999999</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>4</BorderStyle>\r\n          <GridLines>2</GridLines>\r\n          <CellPadding>3</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <AltItemBackColor>#CCCCCC</AltItemBackColor>\r\n          <SelItemForeColor>White</SelItemForeColor>\r\n          <SelItemBackColor>#000099</SelItemBackColor>\r\n          <SelItemFont>1</SelItemFont>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>Black</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterBackColor>#CCCCCC</FooterBackColor>\r\n          <PagerForeColor>Black</PagerForeColor>\r\n          <PagerBackColor>#999999</PagerBackColor>\r\n          <PagerAlign>2</PagerAlign>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Simple2</SchemeName>\r\n          <ForeColor>Black</ForeColor>\r\n          <BackColor>#CCCCCC</BackColor>\r\n          <BorderColor>#999999</BorderColor>\r\n          <BorderWidth>3px</BorderWidth>\r\n          <BorderStyle>4</BorderStyle>\r\n          <GridLines>3</GridLines>\r\n          <CellPadding>4</CellPadding>\r\n          <CellSpacing>2</CellSpacing>\r\n          <ItemBackColor>White</ItemBackColor>\r\n          <SelItemForeColor>White</SelItemForeColor>\r\n          <SelItemBackColor>#000099</SelItemBackColor>\r\n          <SelItemFont>1</SelItemFont>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>Black</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterBackColor>#CCCCCC</FooterBackColor>\r\n          <PagerForeColor>Black</PagerForeColor>\r\n          <PagerBackColor>#CCCCCC</PagerBackColor>\r\n          <PagerAlign>1</PagerAlign>\r\n          <PagerMode>1</PagerMode>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Simple3</SchemeName>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#336666</BorderColor>\r\n          <BorderWidth>3px</BorderWidth>\r\n          <BorderStyle>5</BorderStyle>\r\n          <GridLines>1</GridLines>\r\n          <CellPadding>4</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <ItemForeColor>#333333</ItemForeColor>\r\n          <ItemBackColor>White</ItemBackColor>\r\n          <SelItemForeColor>White</SelItemForeColor>\r\n          <SelItemBackColor>#339966</SelItemBackColor>\r\n          <SelItemFont>1</SelItemFont>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>#336666</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>#333333</FooterForeColor>\r\n          <FooterBackColor>White</FooterBackColor>\r\n          <PagerForeColor>White</PagerForeColor>\r\n          <PagerBackColor>#336666</PagerBackColor>\r\n          <PagerAlign>2</PagerAlign>\r\n          <PagerMode>1</PagerMode>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Classic1</SchemeName>\r\n          <ForeColor>Black</ForeColor>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#CCCCCC</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>1</BorderStyle>\r\n          <GridLines>1</GridLines>\r\n          <CellPadding>4</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <SelItemForeColor>White</SelItemForeColor>\r\n          <SelItemBackColor>#CC3333</SelItemBackColor>\r\n          <SelItemFont>1</SelItemFont>\r\n          <HeaderForeColor>White</HeaderForeColor>\r\n          <HeaderBackColor>#333333</HeaderBackColor>\r\n          <HeaderFont>1</HeaderFont>\r\n          <FooterForeColor>Black</FooterForeColor>\r\n          <FooterBackColor>#CCCC99</FooterBackColor>\r\n          <PagerForeColor>Black</PagerForeColor>\r\n          <PagerBackColor>White</PagerBackColor>\r\n          <PagerAlign>3</PagerAlign>\r\n        </Scheme>\r\n        <Scheme>\r\n          <SchemeName>BDLScheme_Classic2</SchemeName>\r\n          <ForeColor>Black</ForeColor>\r\n          <BackColor>White</BackColor>\r\n          <BorderColor>#DEDFDE</BorderColor>\r\n          <BorderWidth>1px</BorderWidth>\r\n          <BorderStyle>1</BorderStyle>\r\n          <GridLines>2</GridLines>\r\n          <CellPadding>4</CellPadding>\r\n          <CellSpacing>0</CellSpacing>\r\n          <ItemBackColor>#F7F7DE</ItemBackColor>\r\n       [...string is too long...]", (DataRow schemeData) => new DataListAutoFormat(schemeData));
				}
				return DataListDesigner._autoFormats;
			}
		}

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x0600276F RID: 10095 RVA: 0x000D7A0C File Offset: 0x000D6A0C
		protected bool TemplatesExist
		{
			get
			{
				DataList dataList = (DataList)base.ViewControl;
				ITemplate itemTemplate = dataList.ItemTemplate;
				if (itemTemplate != null)
				{
					string textFromTemplate = base.GetTextFromTemplate(itemTemplate);
					return textFromTemplate != null && textFromTemplate.Length > 0;
				}
				return false;
			}
		}

		// Token: 0x06002770 RID: 10096 RVA: 0x000D7A4C File Offset: 0x000D6A4C
		private void CreateDefaultTemplate()
		{
			string text = string.Empty;
			StringBuilder stringBuilder = new StringBuilder();
			DataList dataList = (DataList)base.Component;
			IDataSourceViewSchema dataSourceSchema = this.GetDataSourceSchema();
			IDataSourceFieldSchema[] array = null;
			if (dataSourceSchema != null)
			{
				array = dataSourceSchema.GetFields();
			}
			if (array != null && array.Length > 0)
			{
				foreach (IDataSourceFieldSchema dataSourceFieldSchema in array)
				{
					string name = dataSourceFieldSchema.Name;
					char[] array3 = new char[name.Length];
					for (int j = 0; j < name.Length; j++)
					{
						char c = name[j];
						if (char.IsLetterOrDigit(c) || c == '_')
						{
							array3[j] = c;
						}
						else
						{
							array3[j] = '_';
						}
					}
					string text2 = new string(array3);
					stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "{0}: <asp:Label Text='<%# {1} %>' runat=\"server\" id=\"{2}Label\"/><br />", new object[]
					{
						name,
						DesignTimeDataBinding.CreateEvalExpression(name, string.Empty),
						text2
					}));
					stringBuilder.Append(Environment.NewLine);
					if (dataSourceFieldSchema.PrimaryKey && dataList.DataKeyField.Length == 0)
					{
						dataList.DataKeyField = name;
					}
				}
				stringBuilder.Append("<br />");
				stringBuilder.Append(Environment.NewLine);
				text = stringBuilder.ToString();
			}
			if (text != null && text.Length > 0)
			{
				try
				{
					dataList.ItemTemplate = base.GetTemplateFromText(text, dataList.ItemTemplate);
				}
				catch
				{
				}
			}
		}

		// Token: 0x06002771 RID: 10097 RVA: 0x000D7BD4 File Offset: 0x000D6BD4
		[Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
		protected override ITemplateEditingFrame CreateTemplateEditingFrame(TemplateEditingVerb verb)
		{
			ITemplateEditingService templateEditingService = (ITemplateEditingService)this.GetService(typeof(ITemplateEditingService));
			DataList dataList = (DataList)base.ViewControl;
			string[] array = null;
			Style[] array2 = null;
			switch (verb.Index)
			{
			case 0:
				array = DataListDesigner.ItemTemplateNames;
				array2 = new Style[] { dataList.ItemStyle, dataList.AlternatingItemStyle, dataList.SelectedItemStyle, dataList.EditItemStyle };
				break;
			case 1:
				array = DataListDesigner.HeaderFooterTemplateNames;
				array2 = new Style[] { dataList.HeaderStyle, dataList.FooterStyle };
				break;
			case 2:
				array = DataListDesigner.SeparatorTemplateNames;
				array2 = new Style[] { dataList.SeparatorStyle };
				break;
			}
			return templateEditingService.CreateFrame(this, verb.Text, array, dataList.ControlStyle, array2);
		}

		// Token: 0x06002772 RID: 10098 RVA: 0x000D7CBA File Offset: 0x000D6CBA
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.DisposeTemplateVerbs();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06002773 RID: 10099 RVA: 0x000D7CCC File Offset: 0x000D6CCC
		private void DisposeTemplateVerbs()
		{
			if (this.templateVerbs != null)
			{
				for (int i = 0; i < this.templateVerbs.Length; i++)
				{
					this.templateVerbs[i].Dispose();
				}
				this.templateVerbs = null;
				this.templateVerbsDirty = true;
			}
		}

		// Token: 0x06002774 RID: 10100 RVA: 0x000D7D10 File Offset: 0x000D6D10
		[Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
		protected override TemplateEditingVerb[] GetCachedTemplateEditingVerbs()
		{
			if (this.templateVerbsDirty)
			{
				this.DisposeTemplateVerbs();
				this.templateVerbs = new TemplateEditingVerb[3];
				this.templateVerbs[0] = new TemplateEditingVerb(SR.GetString("DataList_ItemTemplates"), 0, this);
				this.templateVerbs[1] = new TemplateEditingVerb(SR.GetString("DataList_HeaderFooterTemplates"), 1, this);
				this.templateVerbs[2] = new TemplateEditingVerb(SR.GetString("DataList_SeparatorTemplate"), 2, this);
				this.templateVerbsDirty = false;
			}
			return this.templateVerbs;
		}

		// Token: 0x06002775 RID: 10101 RVA: 0x000D7D90 File Offset: 0x000D6D90
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

		// Token: 0x06002776 RID: 10102 RVA: 0x000D7E18 File Offset: 0x000D6E18
		public override string GetDesignTimeHtml()
		{
			bool templatesExist = this.TemplatesExist;
			string text = null;
			if (templatesExist)
			{
				DataList dataList = (DataList)base.ViewControl;
				bool flag = false;
				DesignerDataSourceView designerView = base.DesignerView;
				IEnumerable enumerable;
				if (designerView == null)
				{
					enumerable = base.GetDesignTimeDataSource(5, out flag);
				}
				else
				{
					try
					{
						enumerable = designerView.GetDesignTimeData(5, out flag);
					}
					catch (Exception ex)
					{
						if (base.Component.Site != null)
						{
							IComponentDesignerDebugService componentDesignerDebugService = (IComponentDesignerDebugService)base.Component.Site.GetService(typeof(IComponentDesignerDebugService));
							if (componentDesignerDebugService != null)
							{
								componentDesignerDebugService.Fail(SR.GetString("DataSource_DebugService_FailedCall", new object[] { "DesignerDataSourceView.GetDesignTimeData", ex.Message }));
							}
						}
						enumerable = null;
					}
				}
				bool flag2 = false;
				string text2 = null;
				bool flag3 = false;
				string text3 = null;
				try
				{
					try
					{
						dataList.DataSource = enumerable;
						text2 = dataList.DataKeyField;
						if (text2.Length != 0)
						{
							flag2 = true;
							dataList.DataKeyField = string.Empty;
						}
						text3 = dataList.DataSourceID;
						dataList.DataSourceID = string.Empty;
						flag3 = true;
						dataList.DataBind();
						text = base.GetDesignTimeHtml();
					}
					catch (Exception ex2)
					{
						text = this.GetErrorDesignTimeHtml(ex2);
					}
					return text;
				}
				finally
				{
					dataList.DataSource = null;
					if (flag2)
					{
						dataList.DataKeyField = text2;
					}
					if (flag3)
					{
						dataList.DataSourceID = text3;
					}
				}
			}
			text = this.GetEmptyDesignTimeHtml();
			return text;
		}

		// Token: 0x06002777 RID: 10103 RVA: 0x000D7F88 File Offset: 0x000D6F88
		protected override string GetEmptyDesignTimeHtml()
		{
			string text;
			if (base.CanEnterTemplateMode)
			{
				text = SR.GetString("DataList_NoTemplatesInst");
			}
			else
			{
				text = SR.GetString("DataList_NoTemplatesInst2");
			}
			return base.CreatePlaceHolderDesignTimeHtml(text);
		}

		// Token: 0x06002778 RID: 10104 RVA: 0x000D7FBC File Offset: 0x000D6FBC
		protected override string GetErrorDesignTimeHtml(Exception e)
		{
			return base.CreatePlaceHolderDesignTimeHtml(SR.GetString("Control_ErrorRendering"));
		}

		// Token: 0x06002779 RID: 10105 RVA: 0x000D7FCE File Offset: 0x000D6FCE
		[Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
		public override string GetTemplateContainerDataItemProperty(string templateName)
		{
			return "DataItem";
		}

		// Token: 0x0600277A RID: 10106 RVA: 0x000D7FD8 File Offset: 0x000D6FD8
		[Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
		public override string GetTemplateContent(ITemplateEditingFrame editingFrame, string templateName, out bool allowEditing)
		{
			allowEditing = true;
			DataList dataList = (DataList)base.Component;
			ITemplate template = null;
			string text = string.Empty;
			switch (editingFrame.Verb.Index)
			{
			case 0:
				if (templateName.Equals(DataListDesigner.ItemTemplateNames[0]))
				{
					template = dataList.ItemTemplate;
				}
				else if (templateName.Equals(DataListDesigner.ItemTemplateNames[1]))
				{
					template = dataList.AlternatingItemTemplate;
				}
				else if (templateName.Equals(DataListDesigner.ItemTemplateNames[2]))
				{
					template = dataList.SelectedItemTemplate;
				}
				else if (templateName.Equals(DataListDesigner.ItemTemplateNames[3]))
				{
					template = dataList.EditItemTemplate;
				}
				break;
			case 1:
				if (templateName.Equals(DataListDesigner.HeaderFooterTemplateNames[0]))
				{
					template = dataList.HeaderTemplate;
				}
				else if (templateName.Equals(DataListDesigner.HeaderFooterTemplateNames[1]))
				{
					template = dataList.FooterTemplate;
				}
				break;
			case 2:
				template = dataList.SeparatorTemplate;
				break;
			}
			if (template != null)
			{
				text = base.GetTextFromTemplate(template);
			}
			return text;
		}

		// Token: 0x0600277B RID: 10107 RVA: 0x000D80C2 File Offset: 0x000D70C2
		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(DataList));
			base.Initialize(component);
		}

		// Token: 0x0600277C RID: 10108 RVA: 0x000D80DB File Offset: 0x000D70DB
		protected override void OnSchemaRefreshed()
		{
			if (base.InTemplateModeInternal)
			{
				return;
			}
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.RefreshSchemaCallback), null, SR.GetString("DataList_RefreshSchemaTransaction"));
		}

		// Token: 0x0600277D RID: 10109 RVA: 0x000D8108 File Offset: 0x000D7108
		protected override void OnTemplateEditingVerbsChanged()
		{
			this.templateVerbsDirty = true;
		}

		// Token: 0x0600277E RID: 10110 RVA: 0x000D8114 File Offset: 0x000D7114
		private bool RefreshSchemaCallback(object context)
		{
			DataList dataList = (DataList)base.Component;
			bool flag = dataList.ItemTemplate == null && dataList.EditItemTemplate == null && dataList.AlternatingItemTemplate == null && dataList.SelectedItemTemplate == null;
			IDataSourceViewSchema dataSourceSchema = this.GetDataSourceSchema();
			if (base.DataSourceID.Length > 0 && dataSourceSchema != null)
			{
				if (flag || (!flag && DialogResult.Yes == UIServiceHelper.ShowMessage(base.Component.Site, SR.GetString("DataList_RegenerateTemplates"), SR.GetString("DataList_ClearTemplatesCaption"), MessageBoxButtons.YesNo)))
				{
					dataList.ItemTemplate = null;
					dataList.EditItemTemplate = null;
					dataList.AlternatingItemTemplate = null;
					dataList.SelectedItemTemplate = null;
					dataList.DataKeyField = string.Empty;
					this.CreateDefaultTemplate();
					this.UpdateDesignTimeHtml();
				}
			}
			else if (flag || (!flag && DialogResult.Yes == UIServiceHelper.ShowMessage(base.Component.Site, SR.GetString("DataList_ClearTemplates"), SR.GetString("DataList_ClearTemplatesCaption"), MessageBoxButtons.YesNo)))
			{
				dataList.ItemTemplate = null;
				dataList.EditItemTemplate = null;
				dataList.AlternatingItemTemplate = null;
				dataList.SelectedItemTemplate = null;
				dataList.DataKeyField = string.Empty;
				this.UpdateDesignTimeHtml();
			}
			return true;
		}

		// Token: 0x0600277F RID: 10111 RVA: 0x000D8230 File Offset: 0x000D7230
		[Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
		public override void SetTemplateContent(ITemplateEditingFrame editingFrame, string templateName, string templateContent)
		{
			ITemplate template = null;
			DataList dataList = (DataList)base.Component;
			if (templateContent != null && templateContent.Length != 0)
			{
				ITemplate template2 = null;
				switch (editingFrame.Verb.Index)
				{
				case 0:
					if (templateName.Equals(DataListDesigner.ItemTemplateNames[0]))
					{
						template2 = dataList.ItemTemplate;
					}
					else if (templateName.Equals(DataListDesigner.ItemTemplateNames[1]))
					{
						template2 = dataList.AlternatingItemTemplate;
					}
					else if (templateName.Equals(DataListDesigner.ItemTemplateNames[2]))
					{
						template2 = dataList.SelectedItemTemplate;
					}
					else if (templateName.Equals(DataListDesigner.ItemTemplateNames[3]))
					{
						template2 = dataList.EditItemTemplate;
					}
					break;
				case 1:
					if (templateName.Equals(DataListDesigner.HeaderFooterTemplateNames[0]))
					{
						template2 = dataList.HeaderTemplate;
					}
					else if (templateName.Equals(DataListDesigner.HeaderFooterTemplateNames[1]))
					{
						template2 = dataList.FooterTemplate;
					}
					break;
				case 2:
					template2 = dataList.SeparatorTemplate;
					break;
				}
				template = base.GetTemplateFromText(templateContent, template2);
			}
			switch (editingFrame.Verb.Index)
			{
			case 0:
				if (templateName.Equals(DataListDesigner.ItemTemplateNames[0]))
				{
					dataList.ItemTemplate = template;
					return;
				}
				if (templateName.Equals(DataListDesigner.ItemTemplateNames[1]))
				{
					dataList.AlternatingItemTemplate = template;
					return;
				}
				if (templateName.Equals(DataListDesigner.ItemTemplateNames[2]))
				{
					dataList.SelectedItemTemplate = template;
					return;
				}
				if (templateName.Equals(DataListDesigner.ItemTemplateNames[3]))
				{
					dataList.EditItemTemplate = template;
					return;
				}
				break;
			case 1:
				if (templateName.Equals(DataListDesigner.HeaderFooterTemplateNames[0]))
				{
					dataList.HeaderTemplate = template;
					return;
				}
				if (templateName.Equals(DataListDesigner.HeaderFooterTemplateNames[1]))
				{
					dataList.FooterTemplate = template;
					return;
				}
				break;
			case 2:
				dataList.SeparatorTemplate = template;
				break;
			default:
				return;
			}
		}

		// Token: 0x04001B36 RID: 6966
		private const string templateFieldString = "{0}: <asp:Label Text='<%# {1} %>' runat=\"server\" id=\"{2}Label\"/><br />";

		// Token: 0x04001B37 RID: 6967
		private const string breakString = "<br />";

		// Token: 0x04001B38 RID: 6968
		private const int HeaderFooterTemplates = 1;

		// Token: 0x04001B39 RID: 6969
		private const int ItemTemplates = 0;

		// Token: 0x04001B3A RID: 6970
		private const int SeparatorTemplate = 2;

		// Token: 0x04001B3B RID: 6971
		private const int IDX_HEADER_TEMPLATE = 0;

		// Token: 0x04001B3C RID: 6972
		private const int IDX_FOOTER_TEMPLATE = 1;

		// Token: 0x04001B3D RID: 6973
		private const int IDX_ITEM_TEMPLATE = 0;

		// Token: 0x04001B3E RID: 6974
		private const int IDX_ALTITEM_TEMPLATE = 1;

		// Token: 0x04001B3F RID: 6975
		private const int IDX_SELITEM_TEMPLATE = 2;

		// Token: 0x04001B40 RID: 6976
		private const int IDX_EDITITEM_TEMPLATE = 3;

		// Token: 0x04001B41 RID: 6977
		private const int IDX_SEPARATOR_TEMPLATE = 0;

		// Token: 0x04001B42 RID: 6978
		internal static TraceSwitch DataListDesignerSwitch = new TraceSwitch("DATALISTDESIGNER", "Enable DataList designer general purpose traces.");

		// Token: 0x04001B43 RID: 6979
		private static string[] HeaderFooterTemplateNames = new string[] { "HeaderTemplate", "FooterTemplate" };

		// Token: 0x04001B44 RID: 6980
		private static string[] ItemTemplateNames = new string[] { "ItemTemplate", "AlternatingItemTemplate", "SelectedItemTemplate", "EditItemTemplate" };

		// Token: 0x04001B45 RID: 6981
		private static string[] SeparatorTemplateNames = new string[] { "SeparatorTemplate" };

		// Token: 0x04001B46 RID: 6982
		private TemplateEditingVerb[] templateVerbs;

		// Token: 0x04001B47 RID: 6983
		private bool templateVerbsDirty;

		// Token: 0x04001B48 RID: 6984
		private static DesignerAutoFormatCollection _autoFormats;

		// Token: 0x04001B49 RID: 6985
		[CompilerGenerated]
		private static ControlDesigner.CreateAutoFormatDelegate <>9__CachedAnonymousMethodDelegate1;
	}
}
