using System;
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
	public class TreeViewDesigner : HierarchicalDataBoundControlDesigner
	{
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection designerActionListCollection = new DesignerActionListCollection();
				designerActionListCollection.AddRange(base.ActionLists);
				designerActionListCollection.Add(new TreeViewDesigner.TreeViewDesignerActionList(this));
				return designerActionListCollection;
			}
		}

		public override DesignerAutoFormatCollection AutoFormats
		{
			get
			{
				if (TreeViewDesigner._autoFormats == null)
				{
					TreeViewDesigner._autoFormats = ControlDesigner.CreateAutoFormats("<Schemes>\r\n<xsd:schema id=\"Schemes\" xmlns=\"\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\">\r\n  <xsd:element name=\"Scheme\">\r\n     <xsd:complexType>\r\n       <xsd:all>\r\n        <xsd:element name=\"SchemeName\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"ImageSet\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"NodeIndent\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"ShowLines\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"ShowExpandCollapse\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"NodeStyle-Font-Size\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"NodeStyle-Font-Names\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"NodeStyle-Font--ClearDefaults\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"NodeStyle-ForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"NodeStyle-HorizontalPadding\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"NodeStyle-NodeSpacing\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"NodeStyle-VerticalPadding\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"ParentNodeStyle-Font-Bold\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"ParentNodeStyle-Font--ClearDefaults\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"ParentNodeStyle-ForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SelectedNodeStyle-BackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SelectedNodeStyle-BorderColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SelectedNodeStyle-BorderStyle\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SelectedNodeStyle-BorderWidth\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SelectedNodeStyle-Font-Underline\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SelectedNodeStyle-Font--ClearDefaults\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SelectedNodeStyle-ForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SelectedNodeStyle-HorizontalPadding\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"SelectedNodeStyle-VerticalPadding\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"HoverNodeStyle-BackColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"HoverNodeStyle-BorderColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"HoverNodeStyle-BorderStyle\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"HoverNodeStyle-BorderWidth\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"HoverNodeStyle-Font-Underline\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"HoverNodeStyle-Font--ClearDefaults\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n        <xsd:element name=\"HoverNodeStyle-ForeColor\" minOccurs=\"0\" type=\"xsd:string\"/>\r\n      </xsd:all>\r\n    </xsd:complexType>\r\n  </xsd:element>\r\n  <xsd:element name=\"Schemes\" msdata:IsDataSet=\"true\">\r\n    <xsd:complexType>\r\n      <xsd:choice maxOccurs=\"unbounded\">\r\n        <xsd:element ref=\"Scheme\"/>\r\n      </xsd:choice>\r\n    </xsd:complexType>\r\n  </xsd:element>\r\n</xsd:schema>\r\n<Scheme>\r\n  <SchemeName>TVScheme_Empty</SchemeName>\r\n  <ImageSet>Custom</ImageSet>\r\n  <NodeIndent>20</NodeIndent>\r\n  <ShowLines>false</ShowLines>\r\n  <ShowExpandCollapse>true</ShowExpandCollapse>\r\n  <NodeStyle-Font-Size></NodeStyle-Font-Size>\r\n  <NodeStyle-Font-Names></NodeStyle-Font-Names>\r\n  <NodeStyle-Font--ClearDefaults>true</NodeStyle-Font--ClearDefaults>\r\n  <NodeStyle-ForeColor></NodeStyle-ForeColor>\r\n  <NodeStyle-HorizontalPadding></NodeStyle-HorizontalPadding>\r\n  <NodeStyle-NodeSpacing></NodeStyle-NodeSpacing>\r\n  <NodeStyle-VerticalPadding></NodeStyle-VerticalPadding>\r\n  <ParentNodeStyle-Font-Bold>false</ParentNodeStyle-Font-Bold>\r\n  <ParentNodeStyle-Font--ClearDefaults>true</ParentNodeStyle-Font--ClearDefaults>\r\n  <ParentNodeStyle-ForeColor></ParentNodeStyle-ForeColor>\r\n  <SelectedNodeStyle-BackColor></SelectedNodeStyle-BackColor>\r\n  <SelectedNodeStyle-BorderColor></SelectedNodeStyle-BorderColor>\r\n  <SelectedNodeStyle-BorderStyle>NotSet</SelectedNodeStyle-BorderStyle>\r\n  <SelectedNodeStyle-BorderWidth></SelectedNodeStyle-BorderWidth>\r\n  <SelectedNodeStyle-Font-Underline>false</SelectedNodeStyle-Font-Underline>\r\n  <SelectedNodeStyle-Font--ClearDefaults>true</SelectedNodeStyle-Font--ClearDefaults>\r\n  <SelectedNodeStyle-ForeColor></SelectedNodeStyle-ForeColor>\r\n  <SelectedNodeStyle-HorizontalPadding></SelectedNodeStyle-HorizontalPadding>\r\n  <SelectedNodeStyle-VerticalPadding></SelectedNodeStyle-VerticalPadding>\r\n  <HoverNodeStyle-BackColor></HoverNodeStyle-BackColor>\r\n  <HoverNodeStyle-BorderColor></HoverNodeStyle-BorderColor>\r\n  <HoverNodeStyle-BorderStyle>NotSet</HoverNodeStyle-BorderStyle>\r\n  <HoverNodeStyle-BorderWidth></HoverNodeStyle-BorderWidth>\r\n  <HoverNodeStyle-Font-Underline>false</HoverNodeStyle-Font-Underline>\r\n  <HoverNodeStyle-Font--ClearDefaults>true</HoverNodeStyle-Font--ClearDefaults>\r\n  <HoverNodeStyle-ForeColor></HoverNodeStyle-ForeColor>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>TVScheme_Arrows</SchemeName>\r\n  <ImageSet>Arrows</ImageSet>\r\n  <NodeIndent>20</NodeIndent>\r\n  <ShowLines>false</ShowLines>\r\n  <ShowExpandCollapse>true</ShowExpandCollapse>\r\n  <NodeStyle-Font-Size>8</NodeStyle-Font-Size>\r\n  <NodeStyle-Font-Names>Verdana</NodeStyle-Font-Names>\r\n  <NodeStyle-Font--ClearDefaults>false</NodeStyle-Font--ClearDefaults>\r\n  <NodeStyle-ForeColor>Black</NodeStyle-ForeColor>\r\n  <NodeStyle-HorizontalPadding>5</NodeStyle-HorizontalPadding>\r\n  <NodeStyle-NodeSpacing>0</NodeStyle-NodeSpacing>\r\n  <NodeStyle-VerticalPadding>0</NodeStyle-VerticalPadding>\r\n  <ParentNodeStyle-Font-Bold>false</ParentNodeStyle-Font-Bold>\r\n  <ParentNodeStyle-Font--ClearDefaults>false</ParentNodeStyle-Font--ClearDefaults>\r\n  <ParentNodeStyle-ForeColor></ParentNodeStyle-ForeColor>\r\n  <SelectedNodeStyle-BackColor></SelectedNodeStyle-BackColor>\r\n  <SelectedNodeStyle-BorderColor></SelectedNodeStyle-BorderColor>\r\n  <SelectedNodeStyle-BorderStyle>NotSet</SelectedNodeStyle-BorderStyle>\r\n  <SelectedNodeStyle-BorderWidth></SelectedNodeStyle-BorderWidth>\r\n  <SelectedNodeStyle-Font-Underline>true</SelectedNodeStyle-Font-Underline>\r\n  <SelectedNodeStyle-Font--ClearDefaults>false</SelectedNodeStyle-Font--ClearDefaults>\r\n  <SelectedNodeStyle-ForeColor>#5555DD</SelectedNodeStyle-ForeColor>\r\n  <SelectedNodeStyle-HorizontalPadding>0</SelectedNodeStyle-HorizontalPadding>\r\n  <SelectedNodeStyle-VerticalPadding>0</SelectedNodeStyle-VerticalPadding>\r\n  <HoverNodeStyle-BackColor></HoverNodeStyle-BackColor>\r\n  <HoverNodeStyle-BorderColor></HoverNodeStyle-BorderColor>\r\n  <HoverNodeStyle-BorderStyle>NotSet</HoverNodeStyle-BorderStyle>\r\n  <HoverNodeStyle-BorderWidth></HoverNodeStyle-BorderWidth>\r\n  <HoverNodeStyle-Font-Underline>true</HoverNodeStyle-Font-Underline>\r\n  <HoverNodeStyle-Font--ClearDefaults>false</HoverNodeStyle-Font--ClearDefaults>\r\n  <HoverNodeStyle-ForeColor>#5555DD</HoverNodeStyle-ForeColor>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>TVScheme_Arrows2</SchemeName>\r\n  <ImageSet>Arrows</ImageSet>\r\n  <NodeIndent>20</NodeIndent>\r\n  <ShowLines>false</ShowLines>\r\n  <ShowExpandCollapse>true</ShowExpandCollapse>\r\n  <NodeStyle-Font-Size>10</NodeStyle-Font-Size>\r\n  <NodeStyle-Font-Names>Tahoma</NodeStyle-Font-Names>\r\n  <NodeStyle-Font--ClearDefaults>false</NodeStyle-Font--ClearDefaults>\r\n  <NodeStyle-ForeColor>Black</NodeStyle-ForeColor>\r\n  <NodeStyle-HorizontalPadding>5</NodeStyle-HorizontalPadding>\r\n  <NodeStyle-NodeSpacing>0</NodeStyle-NodeSpacing>\r\n  <NodeStyle-VerticalPadding>0</NodeStyle-VerticalPadding>\r\n  <ParentNodeStyle-Font-Bold>false</ParentNodeStyle-Font-Bold>\r\n  <ParentNodeStyle-Font--ClearDefaults>false</ParentNodeStyle-Font--ClearDefaults>\r\n  <ParentNodeStyle-ForeColor></ParentNodeStyle-ForeColor>\r\n  <SelectedNodeStyle-BackColor></SelectedNodeStyle-BackColor>\r\n  <SelectedNodeStyle-BorderColor></SelectedNodeStyle-BorderColor>\r\n  <SelectedNodeStyle-BorderStyle>NotSet</SelectedNodeStyle-BorderStyle>\r\n  <SelectedNodeStyle-BorderWidth></SelectedNodeStyle-BorderWidth>\r\n  <SelectedNodeStyle-Font-Underline>true</SelectedNodeStyle-Font-Underline>\r\n  <SelectedNodeStyle-Font--ClearDefaults>false</SelectedNodeStyle-Font--ClearDefaults>\r\n  <SelectedNodeStyle-ForeColor>#5555DD</SelectedNodeStyle-ForeColor>\r\n  <SelectedNodeStyle-HorizontalPadding>0</SelectedNodeStyle-HorizontalPadding>\r\n  <SelectedNodeStyle-VerticalPadding>0</SelectedNodeStyle-VerticalPadding>\r\n  <HoverNodeStyle-BackColor></HoverNodeStyle-BackColor>\r\n  <HoverNodeStyle-BorderColor></HoverNodeStyle-BorderColor>\r\n  <HoverNodeStyle-BorderStyle>NotSet</HoverNodeStyle-BorderStyle>\r\n  <HoverNodeStyle-BorderWidth></HoverNodeStyle-BorderWidth>\r\n  <HoverNodeStyle-Font-Underline>true</HoverNodeStyle-Font-Underline>\r\n  <HoverNodeStyle-Font--ClearDefaults>false</HoverNodeStyle-Font--ClearDefaults>\r\n  <HoverNodeStyle-ForeColor>#5555DD</HoverNodeStyle-ForeColor>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>TVScheme_BulletedList</SchemeName>\r\n  <ImageSet>BulletedList</ImageSet>\r\n  <NodeIndent>20</NodeIndent>\r\n  <ShowLines>false</ShowLines>\r\n  <ShowExpandCollapse>false</ShowExpandCollapse>\r\n  <NodeStyle-Font-Size>8</NodeStyle-Font-Size>\r\n  <NodeStyle-Font-Names>Verdana</NodeStyle-Font-Names>\r\n  <NodeStyle-Font--ClearDefaults>false</NodeStyle-Font--ClearDefaults>\r\n  <NodeStyle-ForeColor>Black</NodeStyle-ForeColor>\r\n  <NodeStyle-HorizontalPadding>0</NodeStyle-HorizontalPadding>\r\n  <NodeStyle-NodeSpacing>0</NodeStyle-NodeSpacing>\r\n  <NodeStyle-VerticalPadding>0</NodeStyle-VerticalPadding>\r\n  <ParentNodeStyle-Font-Bold>false</ParentNodeStyle-Font-Bold>\r\n  <ParentNodeStyle-Font--ClearDefaults>false</ParentNodeStyle-Font--ClearDefaults>\r\n  <ParentNodeStyle-ForeColor></ParentNodeStyle-ForeColor>\r\n  <SelectedNodeStyle-BackColor></SelectedNodeStyle-BackColor>\r\n  <SelectedNodeStyle-BorderColor></SelectedNodeStyle-BorderColor>\r\n  <SelectedNodeStyle-BorderStyle>NotSet</SelectedNodeStyle-BorderStyle>\r\n  <SelectedNodeStyle-BorderWidth></SelectedNodeStyle-BorderWidth>\r\n  <SelectedNodeStyle-Font-Underline>true</SelectedNodeStyle-Font-Underline>\r\n  <SelectedNodeStyle-Font--ClearDefaults>false</SelectedNodeStyle-Font--ClearDefaults>\r\n  <SelectedNodeStyle-ForeColor>#5555DD</SelectedNodeStyle-ForeColor>\r\n  <SelectedNodeStyle-HorizontalPadding>0</SelectedNodeStyle-HorizontalPadding>\r\n  <SelectedNodeStyle-VerticalPadding>0</SelectedNodeStyle-VerticalPadding>\r\n  <HoverNodeStyle-BackColor></HoverNodeStyle-BackColor>\r\n  <HoverNodeStyle-BorderColor></HoverNodeStyle-BorderColor>\r\n  <HoverNodeStyle-BorderStyle>NotSet</HoverNodeStyle-BorderStyle>\r\n  <HoverNodeStyle-BorderWidth></HoverNodeStyle-BorderWidth>\r\n  <HoverNodeStyle-Font-Underline>true</HoverNodeStyle-Font-Underline>\r\n  <HoverNodeStyle-Font--ClearDefaults>false</HoverNodeStyle-Font--ClearDefaults>\r\n  <HoverNodeStyle-ForeColor>#5555DD</HoverNodeStyle-ForeColor>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>TVScheme_BulletedList2</SchemeName>\r\n  <ImageSet>BulletedList2</ImageSet>\r\n  <NodeIndent>20</NodeIndent>\r\n  <ShowLines>false</ShowLines>\r\n  <ShowExpandCollapse>false</ShowExpandCollapse>\r\n  <NodeStyle-Font-Size>8</NodeStyle-Font-Size>\r\n  <NodeStyle-Font-Names>Verdana</NodeStyle-Font-Names>\r\n  <NodeStyle-Font--ClearDefaults>false</NodeStyle-Font--ClearDefaults>\r\n  <NodeStyle-ForeColor>Black</NodeStyle-ForeColor>\r\n  <NodeStyle-HorizontalPadding>0</NodeStyle-HorizontalPadding>\r\n  <NodeStyle-NodeSpacing>0</NodeStyle-NodeSpacing>\r\n  <NodeStyle-VerticalPadding>0</NodeStyle-VerticalPadding>\r\n  <ParentNodeStyle-Font-Bold>false</ParentNodeStyle-Font-Bold>\r\n  <ParentNodeStyle-Font--ClearDefaults>false</ParentNodeStyle-Font--ClearDefaults>\r\n  <ParentNodeStyle-ForeColor></ParentNodeStyle-ForeColor>\r\n  <SelectedNodeStyle-BackColor></SelectedNodeStyle-BackColor>\r\n  <SelectedNodeStyle-BorderColor></SelectedNodeStyle-BorderColor>\r\n  <SelectedNodeStyle-BorderStyle>NotSet</SelectedNodeStyle-BorderStyle>\r\n  <SelectedNodeStyle-BorderWidth></SelectedNodeStyle-BorderWidth>\r\n  <SelectedNodeStyle-Font-Underline>true</SelectedNodeStyle-Font-Underline>\r\n  <SelectedNodeStyle-Font--ClearDefaults>false</SelectedNodeStyle-Font--ClearDefaults>\r\n  <SelectedNodeStyle-ForeColor>#5555DD</SelectedNodeStyle-ForeColor>\r\n  <SelectedNodeStyle-HorizontalPadding>0</SelectedNodeStyle-HorizontalPadding>\r\n  <SelectedNodeStyle-VerticalPadding>0</SelectedNodeStyle-VerticalPadding>\r\n  <HoverNodeStyle-BackColor></HoverNodeStyle-BackColor>\r\n  <HoverNodeStyle-BorderColor></HoverNodeStyle-BorderColor>\r\n  <HoverNodeStyle-BorderStyle>NotSet</HoverNodeStyle-BorderStyle>\r\n  <HoverNodeStyle-BorderWidth></HoverNodeStyle-BorderWidth>\r\n  <HoverNodeStyle-Font-Underline>true</HoverNodeStyle-Font-Underline>\r\n  <HoverNodeStyle-Font--ClearDefaults>false</HoverNodeStyle-Font--ClearDefaults>\r\n  <HoverNodeStyle-ForeColor>#5555DD</HoverNodeStyle-ForeColor>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>TVScheme_BulletedList3</SchemeName>\r\n  <ImageSet>BulletedList3</ImageSet>\r\n  <NodeIndent>20</NodeIndent>\r\n  <ShowLines>false</ShowLines>\r\n  <ShowExpandCollapse>false</ShowExpandCollapse>\r\n  <NodeStyle-Font-Size>8</NodeStyle-Font-Size>\r\n  <NodeStyle-Font-Names>Verdana</NodeStyle-Font-Names>\r\n  <NodeStyle-Font--ClearDefaults>false</NodeStyle-Font--ClearDefaults>\r\n  <NodeStyle-ForeColor>Black</NodeStyle-ForeColor>\r\n  <NodeStyle-HorizontalPadding>5</NodeStyle-HorizontalPadding>\r\n  <NodeStyle-NodeSpacing>0</NodeStyle-NodeSpacing>\r\n  <NodeStyle-VerticalPadding>0</NodeStyle-VerticalPadding>\r\n  <ParentNodeStyle-Font-Bold>false</ParentNodeStyle-Font-Bold>\r\n  <ParentNodeStyle-Font--ClearDefaults>false</ParentNodeStyle-Font--ClearDefaults>\r\n  <ParentNodeStyle-ForeColor></ParentNodeStyle-ForeColor>\r\n  <SelectedNodeStyle-BackColor></SelectedNodeStyle-BackColor>\r\n  <SelectedNodeStyle-BorderColor></SelectedNodeStyle-BorderColor>\r\n  <SelectedNodeStyle-BorderStyle>NotSet</SelectedNodeStyle-BorderStyle>\r\n  <SelectedNodeStyle-BorderWidth></SelectedNodeStyle-BorderWidth>\r\n  <SelectedNodeStyle-Font-Underline>true</SelectedNodeStyle-Font-Underline>\r\n  <SelectedNodeStyle-Font--ClearDefaults>false</SelectedNodeStyle-Font--ClearDefaults>\r\n  <SelectedNodeStyle-ForeColor>#5555DD</SelectedNodeStyle-ForeColor>\r\n  <SelectedNodeStyle-HorizontalPadding>0</SelectedNodeStyle-HorizontalPadding>\r\n  <SelectedNodeStyle-VerticalPadding>0</SelectedNodeStyle-VerticalPadding>\r\n  <HoverNodeStyle-BackColor></HoverNodeStyle-BackColor>\r\n  <HoverNodeStyle-BorderColor></HoverNodeStyle-BorderColor>\r\n  <HoverNodeStyle-BorderStyle>NotSet</HoverNodeStyle-BorderStyle>\r\n  <HoverNodeStyle-BorderWidth></HoverNodeStyle-BorderWidth>\r\n  <HoverNodeStyle-Font-Underline>true</HoverNodeStyle-Font-Underline>\r\n  <HoverNodeStyle-Font--ClearDefaults>false</HoverNodeStyle-Font--ClearDefaults>\r\n  <HoverNodeStyle-ForeColor>#5555DD</HoverNodeStyle-ForeColor>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>TVScheme_BulletedList4</SchemeName>\r\n  <ImageSet>BulletedList</ImageSet>\r\n  <NodeIndent>20</NodeIndent>\r\n  <ShowLines>false</ShowLines>\r\n  <ShowExpandCollapse>false</ShowExpandCollapse>\r\n  <NodeStyle-Font-Size>10</NodeStyle-Font-Size>\r\n  <NodeStyle-Font-Names>Tahoma</NodeStyle-Font-Names>\r\n  <NodeStyle-Font--ClearDefaults>false</NodeStyle-Font--ClearDefaults>\r\n  <NodeStyle-ForeColor>Black</NodeStyle-ForeColor>\r\n  <NodeStyle-HorizontalPadding>5</NodeStyle-HorizontalPadding>\r\n  <NodeStyle-NodeSpacing>0</NodeStyle-NodeSpacing>\r\n  <NodeStyle-VerticalPadding>0</NodeStyle-VerticalPadding>\r\n  <ParentNodeStyle-Font-Bold>false</ParentNodeStyle-Font-Bold>\r\n  <ParentNodeStyle-Font--ClearDefaults>false</ParentNodeStyle-Font--ClearDefaults>\r\n  <ParentNodeStyle-ForeColor></ParentNodeStyle-ForeColor>\r\n  <SelectedNodeStyle-BackColor></SelectedNodeStyle-BackColor>\r\n  <SelectedNodeStyle-BorderColor></SelectedNodeStyle-BorderColor>\r\n  <SelectedNodeStyle-BorderStyle>NotSet</SelectedNodeStyle-BorderStyle>\r\n  <SelectedNodeStyle-BorderWidth></SelectedNodeStyle-BorderWidth>\r\n  <SelectedNodeStyle-Font-Underline>true</SelectedNodeStyle-Font-Underline>\r\n  <SelectedNodeStyle-Font--ClearDefaults>false</SelectedNodeStyle-Font--ClearDefaults>\r\n  <SelectedNodeStyle-ForeColor>#5555DD</SelectedNodeStyle-ForeColor>\r\n  <SelectedNodeStyle-HorizontalPadding>0</SelectedNodeStyle-HorizontalPadding>\r\n  <SelectedNodeStyle-VerticalPadding>0</SelectedNodeStyle-VerticalPadding>\r\n  <HoverNodeStyle-BackColor></HoverNodeStyle-BackColor>\r\n  <HoverNodeStyle-BorderColor></HoverNodeStyle-BorderColor>\r\n  <HoverNodeStyle-BorderStyle>NotSet</HoverNodeStyle-BorderStyle>\r\n  <HoverNodeStyle-BorderWidth></HoverNodeStyle-BorderWidth>\r\n  <HoverNodeStyle-Font-Underline>true</HoverNodeStyle-Font-Underline>\r\n  <HoverNodeStyle-Font--ClearDefaults>false</HoverNodeStyle-Font--ClearDefaults>\r\n  <HoverNodeStyle-ForeColor>#5555DD</HoverNodeStyle-ForeColor>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>TVScheme_BulletedList5</SchemeName>\r\n  <ImageSet>BulletedList2</ImageSet>\r\n  <NodeIndent>20</NodeIndent>\r\n  <ShowLines>false</ShowLines>\r\n  <ShowExpandCollapse>false</ShowExpandCollapse>\r\n  <NodeStyle-Font-Size>10</NodeStyle-Font-Size>\r\n  <NodeStyle-Font-Names>Tahoma</NodeStyle-Font-Names>\r\n  <NodeStyle-Font--ClearDefaults>false</NodeStyle-Font--ClearDefaults>\r\n  <NodeStyle-ForeColor>Black</NodeStyle-ForeColor>\r\n  <NodeStyle-HorizontalPadding>5</NodeStyle-HorizontalPadding>\r\n  <NodeStyle-NodeSpacing>0</NodeStyle-NodeSpacing>\r\n  <NodeStyle-VerticalPadding>0</NodeStyle-VerticalPadding>\r\n  <ParentNodeStyle-Font-Bold>false</ParentNodeStyle-Font-Bold>\r\n  <ParentNodeStyle-Font--ClearDefaults>false</ParentNodeStyle-Font--ClearDefaults>\r\n  <ParentNodeStyle-ForeColor></ParentNodeStyle-ForeColor>\r\n  <SelectedNodeStyle-BackColor></SelectedNodeStyle-BackColor>\r\n  <SelectedNodeStyle-BorderColor></SelectedNodeStyle-BorderColor>\r\n  <SelectedNodeStyle-BorderStyle>NotSet</SelectedNodeStyle-BorderStyle>\r\n  <SelectedNodeStyle-BorderWidth></SelectedNodeStyle-BorderWidth>\r\n  <SelectedNodeStyle-Font-Underline>true</SelectedNodeStyle-Font-Underline>\r\n  <SelectedNodeStyle-Font--ClearDefaults>false</SelectedNodeStyle-Font--ClearDefaults>\r\n  <SelectedNodeStyle-ForeColor>#5555DD</SelectedNodeStyle-ForeColor>\r\n  <SelectedNodeStyle-HorizontalPadding>0</SelectedNodeStyle-HorizontalPadding>\r\n  <SelectedNodeStyle-VerticalPadding>0</SelectedNodeStyle-VerticalPadding>\r\n  <HoverNodeStyle-BackColor></HoverNodeStyle-BackColor>\r\n  <HoverNodeStyle-BorderColor></HoverNodeStyle-BorderColor>\r\n  <HoverNodeStyle-BorderStyle>NotSet</HoverNodeStyle-BorderStyle>\r\n  <HoverNodeStyle-BorderWidth></HoverNodeStyle-BorderWidth>\r\n  <HoverNodeStyle-Font-Underline>true</HoverNodeStyle-Font-Underline>\r\n  <HoverNodeStyle-Font--ClearDefaults>false</HoverNodeStyle-Font--ClearDefaults>\r\n  <HoverNodeStyle-ForeColor>#5555DD</HoverNodeStyle-ForeColor>\r\n</Scheme>\r\n<Scheme>\r\n  <SchemeName>TVScheme_BulletedList6</SchemeName>\r\n  <ImageSet>BulletedList4</ImageSet>\r\n  <NodeIndent>20</NodeIndent>\r\n  <ShowLines>false</ShowLines>\r\n  <ShowExpandCollapse>false</ShowExpandCollapse>\r\n  <NodeStyle-Font-Size>10</NodeStyle-Font-Size>\r\n  <NodeStyle-Font-Names>Tahoma</NodeStyle-Font-Names>\r\n  <NodeStyle-Font--ClearDefaults>false</NodeStyle-Font--ClearDefaults>\r\n  <NodeStyle-ForeColor>Black</NodeStyle-ForeColor>\r\n  <NodeStyle-HorizontalPadding>5</NodeStyle-HorizontalPadding>\r\n  <NodeStyle-NodeSpacing>0</NodeStyle-NodeSpacing>\r\n  <NodeStyle-VerticalPadding>0</NodeStyle-VerticalPadding>\r\n  <ParentNodeStyle-Font-Bold>false</ParentNodeStyle-Font-Bold>\r\n  <ParentNodeStyle-Font--ClearDefaults>false</ParentNodeStyle-Font--ClearDefaults>\r\n  <ParentNodeStyle-ForeColor></ParentNodeStyle-ForeColor>\r\n  <SelectedNodeStyle-BackColor></SelectedNodeStyle-BackColor>\r\n  <SelectedNodeStyle-BorderColor></SelectedNodeStyle-BorderColor>\r\n  <SelectedNodeStyle-BorderStyle>NotSet</SelectedNodeStyle-BorderStyle>\r\n  <SelectedNodeStyle-BorderWidth></SelectedNodeStyle-BorderWidth>\r\n  <SelectedNodeStyle-Font-Underline>true</SelectedNodeStyle-Font-Underline>[...string is too long...]", (DataRow schemeData) => new BaseAutoFormat(schemeData));
				}
				return TreeViewDesigner._autoFormats;
			}
		}

		protected override bool UsePreviewControl
		{
			get
			{
				return true;
			}
		}

		protected void CreateLineImages()
		{
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.CreateLineImagesCallBack), null, SR.GetString("TreeViewDesigner_CreateLineImagesTransactionDescription"));
		}

		private bool CreateLineImagesCallBack(object context)
		{
			TreeViewImageGenerator treeViewImageGenerator = new TreeViewImageGenerator(this._treeView);
			return UIServiceHelper.ShowDialog(base.Component.Site, treeViewImageGenerator) == DialogResult.OK;
		}

		protected override void DataBind(BaseDataBoundControl dataBoundControl)
		{
			global::System.Web.UI.WebControls.TreeView treeView = (global::System.Web.UI.WebControls.TreeView)dataBoundControl;
			this._usingSampleData = false;
			this._emptyDataBinding = false;
			if ((treeView.DataSourceID != null && treeView.DataSourceID.Length > 0) || treeView.DataSource != null || treeView.Nodes.Count == 0)
			{
				treeView.Nodes.Clear();
				base.DataBind(treeView);
			}
			if (this._usingSampleData)
			{
				treeView.ExpandAll();
				return;
			}
			this.ExpandToDepth(treeView.Nodes, treeView.ExpandDepth);
			if (treeView.Nodes.Count == 0)
			{
				this._emptyDataBinding = true;
			}
		}

		protected void EditBindings()
		{
			IServiceProvider site = this._treeView.Site;
			TreeViewBindingsEditorForm treeViewBindingsEditorForm = new TreeViewBindingsEditorForm(site, this._treeView, this);
			UIServiceHelper.ShowDialog(site, treeViewBindingsEditorForm);
		}

		protected void EditNodes()
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["Nodes"];
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.EditNodesChangeCallback), null, SR.GetString("TreeViewDesigner_EditNodesTransactionDescription"), propertyDescriptor);
		}

		private bool EditNodesChangeCallback(object context)
		{
			IServiceProvider site = this._treeView.Site;
			TreeNodeCollectionEditorDialog treeNodeCollectionEditorDialog = new TreeNodeCollectionEditorDialog(this._treeView, this);
			DialogResult dialogResult = UIServiceHelper.ShowDialog(site, treeNodeCollectionEditorDialog);
			return dialogResult == DialogResult.OK;
		}

		private void ExpandToDepth(global::System.Web.UI.WebControls.TreeNodeCollection nodes, int depth)
		{
			foreach (object obj in nodes)
			{
				global::System.Web.UI.WebControls.TreeNode treeNode = (global::System.Web.UI.WebControls.TreeNode)obj;
				if (treeNode.Expanded != false && (depth == -1 || treeNode.Depth < depth))
				{
					treeNode.Expanded = new bool?(true);
					this.ExpandToDepth(treeNode.ChildNodes, depth);
				}
			}
		}

		protected override IHierarchicalEnumerable GetSampleDataSource()
		{
			this._usingSampleData = true;
			((global::System.Web.UI.WebControls.TreeView)base.ViewControl).AutoGenerateDataBindings = true;
			return base.GetSampleDataSource();
		}

		public override string GetDesignTimeHtml()
		{
			string text = base.GetDesignTimeHtml();
			if (this._emptyDataBinding)
			{
				text = this.GetEmptyDataBindingDesignTimeHtml();
			}
			return text;
		}

		private string GetEmptyDataBindingDesignTimeHtml()
		{
			string name = this._treeView.Site.Name;
			return string.Format(CultureInfo.CurrentUICulture, "\r\n                <table cellpadding=4 cellspacing=0 style=\"font-family:Tahoma;font-size:8pt;color:buttontext;background-color:buttonface\">\r\n                  <tr><td><span style=\"font-weight:bold\">TreeView</span> - {0}</td></tr>\r\n                  <tr><td>{1}</td></tr>\r\n                </table>\r\n             ", new object[]
			{
				name,
				SR.GetString("TreeViewDesigner_EmptyDataBinding")
			});
		}

		protected override string GetEmptyDesignTimeHtml()
		{
			string name = this._treeView.Site.Name;
			return string.Format(CultureInfo.CurrentUICulture, "\r\n                <table cellpadding=4 cellspacing=0 style=\"font-family:Tahoma;font-size:8pt;color:buttontext;background-color:buttonface\">\r\n                  <tr><td><span style=\"font-weight:bold\">TreeView</span> - {0}</td></tr>\r\n                  <tr><td>{1}</td></tr>\r\n                </table>\r\n             ", new object[]
			{
				name,
				SR.GetString("TreeViewDesigner_Empty")
			});
		}

		protected override string GetErrorDesignTimeHtml(Exception e)
		{
			string name = this._treeView.Site.Name;
			return string.Format(CultureInfo.CurrentUICulture, "\r\n                <table cellpadding=4 cellspacing=0 style=\"font-family:Tahoma;font-size:8pt;color:buttontext;background-color:buttonface;border: solid 1px;border-top-color:buttonhighlight;border-left-color:buttonhighlight;border-bottom-color:buttonshadow;border-right-color:buttonshadow\">\r\n                  <tr><td><span style=\"font-weight:bold\">TreeView</span> - {0}</td></tr>\r\n                  <tr><td>{1}</td></tr>\r\n                </table>\r\n             ", new object[]
			{
				name,
				SR.GetString("TreeViewDesigner_Error", new object[] { e.Message })
			});
		}

		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(global::System.Web.UI.WebControls.TreeView));
			base.Initialize(component);
			this._treeView = (global::System.Web.UI.WebControls.TreeView)component;
		}

		internal void InvokeTreeNodeCollectionEditor()
		{
			this.EditNodes();
		}

		internal void InvokeTreeViewBindingsEditor()
		{
			this.EditBindings();
		}

		private const string emptyDesignTimeHtml = "\r\n                <table cellpadding=4 cellspacing=0 style=\"font-family:Tahoma;font-size:8pt;color:buttontext;background-color:buttonface\">\r\n                  <tr><td><span style=\"font-weight:bold\">TreeView</span> - {0}</td></tr>\r\n                  <tr><td>{1}</td></tr>\r\n                </table>\r\n             ";

		private const string errorDesignTimeHtml = "\r\n                <table cellpadding=4 cellspacing=0 style=\"font-family:Tahoma;font-size:8pt;color:buttontext;background-color:buttonface;border: solid 1px;border-top-color:buttonhighlight;border-left-color:buttonhighlight;border-bottom-color:buttonshadow;border-right-color:buttonshadow\">\r\n                  <tr><td><span style=\"font-weight:bold\">TreeView</span> - {0}</td></tr>\r\n                  <tr><td>{1}</td></tr>\r\n                </table>\r\n             ";

		private global::System.Web.UI.WebControls.TreeView _treeView;

		private bool _usingSampleData;

		private bool _emptyDataBinding;

		private static DesignerAutoFormatCollection _autoFormats;

		[CompilerGenerated]
		private static ControlDesigner.CreateAutoFormatDelegate <>9__CachedAnonymousMethodDelegate1;

		private class TreeViewDesignerActionList : DesignerActionList
		{
			public TreeViewDesignerActionList(TreeViewDesigner parent)
				: base(parent.Component)
			{
				this._parent = parent;
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

			public bool ShowLines
			{
				get
				{
					return ((global::System.Web.UI.WebControls.TreeView)base.Component).ShowLines;
				}
				set
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(typeof(global::System.Web.UI.WebControls.TreeView))["ShowLines"];
					propertyDescriptor.SetValue(base.Component, value);
					TypeDescriptor.Refresh(base.Component);
				}
			}

			public void CreateLineImages()
			{
				this._parent.CreateLineImages();
			}

			public void EditBindings()
			{
				this._parent.EditBindings();
			}

			public void EditNodes()
			{
				this._parent.EditNodes();
			}

			public override DesignerActionItemCollection GetSortedActionItems()
			{
				DesignerActionItemCollection designerActionItemCollection = new DesignerActionItemCollection();
				string @string = SR.GetString("TreeViewDesigner_DataActionGroup");
				if (string.IsNullOrEmpty(this._parent.DataSourceID))
				{
					designerActionItemCollection.Add(new DesignerActionMethodItem(this, "EditNodes", SR.GetString("TreeViewDesigner_EditNodes"), @string, SR.GetString("TreeViewDesigner_EditNodesDescription"), true));
				}
				else
				{
					designerActionItemCollection.Add(new DesignerActionMethodItem(this, "EditBindings", SR.GetString("TreeViewDesigner_EditBindings"), @string, SR.GetString("TreeViewDesigner_EditBindingsDescription"), true));
				}
				if (this.ShowLines)
				{
					designerActionItemCollection.Add(new DesignerActionMethodItem(this, "CreateLineImages", SR.GetString("TreeViewDesigner_CreateLineImages"), @string, SR.GetString("TreeViewDesigner_CreateLineImagesDescription"), true));
				}
				designerActionItemCollection.Add(new DesignerActionPropertyItem("ShowLines", SR.GetString("TreeViewDesigner_ShowLines"), "Actions", SR.GetString("TreeViewDesigner_ShowLinesDescription")));
				return designerActionItemCollection;
			}

			private TreeViewDesigner _parent;
		}
	}
}
