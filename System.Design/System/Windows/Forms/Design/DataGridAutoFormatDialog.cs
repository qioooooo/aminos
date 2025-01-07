using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Xml;

namespace System.Windows.Forms.Design
{
	internal partial class DataGridAutoFormatDialog : Form
	{
		internal DataGridAutoFormatDialog(DataGrid dgrid)
		{
			this.dgrid = dgrid;
			base.ShowInTaskbar = false;
			this.dataSet.Locale = CultureInfo.InvariantCulture;
			this.dataSet.ReadXmlSchema(new XmlTextReader(new StringReader("<xsd:schema id=\"pulica\" xmlns=\"\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\"><xsd:element name=\"Scheme\"><xsd:complexType><xsd:all><xsd:element name=\"SchemeName\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"SchemePicture\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"BorderStyle\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"FlatMode\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"Font\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"CaptionFont\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"HeaderFont\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"AlternatingBackColor\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"BackColor\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"BackgroundColor\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"CaptionForeColor\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"CaptionBackColor\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"ForeColor\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"GridLineColor\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"GridLineStyle\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"HeaderBackColor\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"HeaderForeColor\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"LinkColor\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"LinkHoverColor\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"ParentRowsBackColor\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"ParentRowsForeColor\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"SelectionForeColor\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"SelectionBackColor\" minOccurs=\"0\" type=\"xsd:string\"/></xsd:all></xsd:complexType></xsd:element></xsd:schema>")));
			this.dataSet.ReadXml(new StringReader("<pulica><Scheme><SchemeName>Default</SchemeName><SchemePicture>default.bmp</SchemePicture><BorderStyle></BorderStyle><FlatMode></FlatMode><CaptionFont></CaptionFont><Font></Font><HeaderFont></HeaderFont><AlternatingBackColor></AlternatingBackColor><BackColor></BackColor><CaptionForeColor></CaptionForeColor><CaptionBackColor></CaptionBackColor><ForeColor></ForeColor><GridLineColor></GridLineColor><GridLineStyle></GridLineStyle><HeaderBackColor></HeaderBackColor><HeaderForeColor></HeaderForeColor><LinkColor></LinkColor><LinkHoverColor></LinkHoverColor><ParentRowsBackColor></ParentRowsBackColor><ParentRowsForeColor></ParentRowsForeColor><SelectionForeColor></SelectionForeColor><SelectionBackColor></SelectionBackColor></Scheme><Scheme><SchemeName>Professional 1</SchemeName><SchemePicture>professional1.bmp</SchemePicture><CaptionFont>Verdana, 10pt</CaptionFont><AlternatingBackColor>LightGray</AlternatingBackColor><CaptionForeColor>Navy</CaptionForeColor><CaptionBackColor>White</CaptionBackColor><ForeColor>Black</ForeColor><BackColor>DarkGray</BackColor><GridLineColor>Black</GridLineColor><GridLineStyle>None</GridLineStyle><HeaderBackColor>Silver</HeaderBackColor><HeaderForeColor>Black</HeaderForeColor><LinkColor>Navy</LinkColor><LinkHoverColor>Blue</LinkHoverColor><ParentRowsBackColor>White</ParentRowsBackColor><ParentRowsForeColor>Black</ParentRowsForeColor><SelectionForeColor>White</SelectionForeColor><SelectionBackColor>Navy</SelectionBackColor></Scheme><Scheme><SchemeName>Professional 2</SchemeName><SchemePicture>professional2.bmp</SchemePicture><BorderStyle>FixedSingle</BorderStyle><FlatMode>True</FlatMode><CaptionFont>Tahoma, 8pt</CaptionFont><AlternatingBackColor>Gainsboro</AlternatingBackColor><BackColor>Silver</BackColor><CaptionForeColor>White</CaptionForeColor><CaptionBackColor>DarkSlateBlue</CaptionBackColor><ForeColor>Black</ForeColor><GridLineColor>White</GridLineColor><HeaderBackColor>DarkGray</HeaderBackColor><HeaderForeColor>Black</HeaderForeColor><LinkColor>DarkSlateBlue</LinkColor><LinkHoverColor>RoyalBlue</LinkHoverColor><ParentRowsBackColor>Black</ParentRowsBackColor><ParentRowsForeColor>White</ParentRowsForeColor><SelectionForeColor>White</SelectionForeColor><SelectionBackColor>DarkSlateBlue</SelectionBackColor></Scheme><Scheme><SchemeName>Professional 3</SchemeName><SchemePicture>professional3.bmp</SchemePicture><BorderStyle>None</BorderStyle><FlatMode>True</FlatMode><CaptionFont>Tahoma, 8pt, style=1</CaptionFont><HeaderFont>Tahoma, 8pt, style=1</HeaderFont><Font>Tahoma, 8pt</Font><AlternatingBackColor>LightGray</AlternatingBackColor><BackColor>Gainsboro</BackColor><BackgroundColor>Silver</BackgroundColor><CaptionForeColor>MidnightBlue</CaptionForeColor><CaptionBackColor>LightSteelBlue</CaptionBackColor><ForeColor>Black</ForeColor><GridLineColor>DimGray</GridLineColor><GridLineStyle>None</GridLineStyle><HeaderBackColor>MidnightBlue</HeaderBackColor><HeaderForeColor>White</HeaderForeColor><LinkColor>MidnightBlue</LinkColor><LinkHoverColor>RoyalBlue</LinkHoverColor><ParentRowsBackColor>DarkGray</ParentRowsBackColor><ParentRowsForeColor>Black</ParentRowsForeColor><SelectionForeColor>White</SelectionForeColor><SelectionBackColor>CadetBlue</SelectionBackColor></Scheme><Scheme><SchemeName>Professional 4</SchemeName><SchemePicture>professional4.bmp</SchemePicture><BorderStyle>None</BorderStyle><FlatMode>True</FlatMode><CaptionFont>Tahoma, 8pt, style=1</CaptionFont><HeaderFont>Tahoma, 8pt, style=1</HeaderFont><Font>Tahoma, 8pt</Font><AlternatingBackColor>Lavender</AlternatingBackColor><BackColor>WhiteSmoke</BackColor><BackgroundColor>LightGray</BackgroundColor><CaptionForeColor>MidnightBlue</CaptionForeColor><CaptionBackColor>LightSteelBlue</CaptionBackColor><ForeColor>MidnightBlue</ForeColor><GridLineColor>Gainsboro</GridLineColor><GridLineStyle>None</GridLineStyle><HeaderBackColor>MidnightBlue</HeaderBackColor><HeaderForeColor>WhiteSmoke</HeaderForeColor><LinkColor>Teal</LinkColor><LinkHoverColor>DarkMagenta</LinkHoverColor><ParentRowsBackColor>Gainsboro</ParentRowsBackColor><ParentRowsForeColor>MidnightBlue</ParentRowsForeColor><SelectionForeColor>WhiteSmoke</SelectionForeColor><SelectionBackColor>CadetBlue</SelectionBackColor></Scheme><Scheme><SchemeName>Classic</SchemeName><SchemePicture>classic.bmp</SchemePicture><BorderStyle>FixedSingle</BorderStyle><FlatMode>True</FlatMode><Font>Times New Roman, 9pt</Font><HeaderFont>Tahoma, 8pt, style=1</HeaderFont><CaptionFont>Tahoma, 8pt, style=1</CaptionFont><AlternatingBackColor>WhiteSmoke</AlternatingBackColor><BackColor>Gainsboro</BackColor><BackgroundColor>DarkGray</BackgroundColor><CaptionForeColor>Black</CaptionForeColor><CaptionBackColor>DarkKhaki</CaptionBackColor><ForeColor>Black</ForeColor><GridLineColor>Silver</GridLineColor><HeaderBackColor>Black</HeaderBackColor><HeaderForeColor>White</HeaderForeColor><LinkColor>DarkSlateBlue</LinkColor><LinkHoverColor>Firebrick</LinkHoverColor><ParentRowsForeColor>Black</ParentRowsForeColor><ParentRowsBackColor>LightGray</ParentRowsBackColor><SelectionForeColor>White</SelectionForeColor><SelectionBackColor>Firebrick</SelectionBackColor></Scheme><Scheme><SchemeName>Simple</SchemeName><SchemePicture>Simple.bmp</SchemePicture><BorderStyle>FixedSingle</BorderStyle><FlatMode>True</FlatMode><Font>Courier New, 9pt</Font><HeaderFont>Courier New, 10pt, style=1</HeaderFont><CaptionFont>Courier New, 10pt, style=1</CaptionFont><AlternatingBackColor>White</AlternatingBackColor><BackColor>White</BackColor><BackgroundColor>Gainsboro</BackgroundColor><CaptionForeColor>Black</CaptionForeColor><CaptionBackColor>Silver</CaptionBackColor><ForeColor>DarkSlateGray</ForeColor><GridLineColor>DarkGray</GridLineColor><HeaderBackColor>DarkGreen</HeaderBackColor><HeaderForeColor>White</HeaderForeColor><LinkColor>DarkGreen</LinkColor><LinkHoverColor>Blue</LinkHoverColor><ParentRowsForeColor>Black</ParentRowsForeColor><ParentRowsBackColor>Gainsboro</ParentRowsBackColor><SelectionForeColor>Black</SelectionForeColor><SelectionBackColor>DarkSeaGreen</SelectionBackColor></Scheme><Scheme><SchemeName>Colorful 1</SchemeName><SchemePicture>colorful1.bmp</SchemePicture><BorderStyle>FixedSingle</BorderStyle><FlatMode>True</FlatMode><Font>Tahoma, 8pt</Font><CaptionFont>Tahoma, 9pt, style=1</CaptionFont><HeaderFont>Tahoma, 9pt, style=1</HeaderFont><AlternatingBackColor>LightGoldenrodYellow</AlternatingBackColor><BackColor>White</BackColor><BackgroundColor>LightGoldenrodYellow</BackgroundColor><CaptionForeColor>DarkSlateBlue</CaptionForeColor><CaptionBackColor>LightGoldenrodYellow</CaptionBackColor><ForeColor>DarkSlateBlue</ForeColor><GridLineColor>Peru</GridLineColor><GridLineStyle>None</GridLineStyle><HeaderBackColor>Maroon</HeaderBackColor><HeaderForeColor>LightGoldenrodYellow</HeaderForeColor><LinkColor>Maroon</LinkColor><LinkHoverColor>SlateBlue</LinkHoverColor><ParentRowsBackColor>BurlyWood</ParentRowsBackColor><ParentRowsForeColor>DarkSlateBlue</ParentRowsForeColor><SelectionForeColor>GhostWhite</SelectionForeColor><SelectionBackColor>DarkSlateBlue</SelectionBackColor></Scheme><Scheme><SchemeName>Colorful 2</SchemeName><SchemePicture>colorful2.bmp</SchemePicture><BorderStyle>None</BorderStyle><FlatMode>True</FlatMode><Font>Tahoma, 8pt</Font><CaptionFont>Tahoma, 8pt, style=1</CaptionFont><HeaderFont>Tahoma, 8pt, style=1</HeaderFont><AlternatingBackColor>GhostWhite</AlternatingBackColor><BackColor>GhostWhite</BackColor><BackgroundColor>Lavender</BackgroundColor><CaptionForeColor>White</CaptionForeColor><CaptionBackColor>RoyalBlue</CaptionBackColor><ForeColor>MidnightBlue</ForeColor><GridLineColor>RoyalBlue</GridLineColor><HeaderBackColor>MidnightBlue</HeaderBackColor><HeaderForeColor>Lavender</HeaderForeColor><LinkColor>Teal</LinkColor><LinkHoverColor>DodgerBlue</LinkHoverColor><ParentRowsBackColor>Lavender</ParentRowsBackColor><ParentRowsForeColor>MidnightBlue</ParentRowsForeColor><SelectionForeColor>PaleGreen</SelectionForeColor><SelectionBackColor>Teal</SelectionBackColor></Scheme><Scheme><SchemeName>Colorful 3</SchemeName><SchemePicture>colorful3.bmp</SchemePicture><BorderStyle>None</BorderStyle><FlatMode>True</FlatMode><Font>Tahoma, 8pt</Font><CaptionFont>Tahoma, 8pt, style=1</CaptionFont><HeaderFont>Tahoma, 8pt, style=1</HeaderFont><AlternatingBackColor>OldLace</AlternatingBackColor><BackColor>OldLace</BackColor><BackgroundColor>Tan</BackgroundColor><CaptionForeColor>OldLace</CaptionForeColor><CaptionBackColor>SaddleBrown</CaptionBackColor><ForeColor>DarkSlateGray</ForeColor><GridLineColor>Tan</GridLineColor><GridLineStyle>Solid</GridLineStyle><HeaderBackColor>Wheat</HeaderBackColor><HeaderForeColor>SaddleBrown</HeaderForeColor><LinkColor>DarkSlateBlue</LinkColor><LinkHoverColor>Teal</LinkHoverColor><ParentRowsBackColor>OldLace</ParentRowsBackColor><ParentRowsForeColor>DarkSlateGray</ParentRowsForeColor><SelectionForeColor>White</SelectionForeColor><SelectionBackColor>SlateGray</SelectionBackColor></Scheme><Scheme><SchemeName>Colorful 4</SchemeName><SchemePicture>colorful4.bmp</SchemePicture><BorderStyle>FixedSingle</BorderStyle><FlatMode>True</FlatMode><Font>Tahoma, 8pt</Font><CaptionFont>Tahoma, 8pt, style=1</CaptionFont><HeaderFont>Tahoma, 8pt, style=1</HeaderFont><AlternatingBackColor>White</AlternatingBackColor><BackColor>White</BackColor><BackgroundColor>Ivory</BackgroundColor><CaptionForeColor>Lavender</CaptionForeColor><CaptionBackColor>DarkSlateBlue</CaptionBackColor><ForeColor>Black</ForeColor><GridLineColor>Wheat</GridLineColor><HeaderBackColor>CadetBlue</HeaderBackColor><HeaderForeColor>Black</HeaderForeColor><LinkColor>DarkSlateBlue</LinkColor><LinkHoverColor>LightSeaGreen</LinkHoverColor><ParentRowsBackColor>Ivory</ParentRowsBackColor><ParentRowsForeColor>Black</ParentRowsForeColor><SelectionForeColor>DarkSlateBlue</SelectionForeColor><SelectionBackColor>Wheat</SelectionBackColor></Scheme><Scheme><SchemeName>256 Color 1</SchemeName><SchemePicture>256_1.bmp</SchemePicture><Font>Tahoma, 8pt</Font><CaptionFont>Tahoma, 8 pt</CaptionFont><HeaderFont>Tahoma, 8pt</HeaderFont><AlternatingBackColor>Silver</AlternatingBackColor><BackColor>White</BackColor><CaptionForeColor>White</CaptionForeColor><CaptionBackColor>Maroon</CaptionBackColor><ForeColor>Black</ForeColor><GridLineColor>Silver</GridLineColor><HeaderBackColor>Silver</HeaderBackColor><HeaderForeColor>Black</HeaderForeColor><LinkColor>Maroon</LinkColor><LinkHoverColor>Red</LinkHoverColor><ParentRowsBackColor>Silver</ParentRowsBackColor><ParentRowsForeColor>Black</ParentRowsForeColor><SelectionForeColor>White</SelectionForeColor><SelectionBackColor>Maroon</SelectionBackColor></Scheme><Scheme><SchemeName>256 Color 2</SchemeName><SchemePicture>256_2.bmp</SchemePicture><BorderStyle>FixedSingle</BorderStyle><FlatMode>True</FlatMode><CaptionFont>Microsoft Sans Serif, 10 pt, style=1</CaptionFont><Font>Tahoma, 8pt</Font><HeaderFont>Tahoma, 8pt</HeaderFont><AlternatingBackColor>White</AlternatingBackColor><BackColor>White</BackColor><CaptionForeColor>White</CaptionForeColor><CaptionBackColor>Teal</CaptionBackColor><ForeColor>Black</ForeColor><GridLineColor>Silver</GridLineColor><HeaderBackColor>Black</HeaderBackColor><HeaderForeColor>White</HeaderForeColor><LinkColor>Purple</LinkColor><LinkHoverColor>Fuchsia</LinkHoverColor><ParentRowsBackColor>Gray</ParentRowsBackColor><ParentRowsForeColor>White</ParentRowsForeColor><SelectionForeColor>White</SelectionForeColor><SelectionBackColor>Maroon</SelectionBackColor></Scheme></pulica>"), XmlReadMode.IgnoreSchema);
			this.schemeTable = this.dataSet.Tables["Scheme"];
			this.IMBusy = true;
			this.InitializeComponent();
			this.schemeName.DataSource = this.schemeTable;
			this.AddDataToDataGrid();
			this.AddStyleSheetInformationToDataGrid();
			if (dgrid.Site != null)
			{
				IUIService iuiservice = (IUIService)dgrid.Site.GetService(typeof(IUIService));
				if (iuiservice != null)
				{
					Font font = (Font)iuiservice.Styles["DialogFont"];
					if (font != null)
					{
						this.Font = font;
					}
				}
			}
			this.IMBusy = false;
		}

		private void AddStyleSheetInformationToDataGrid()
		{
			DataGridTableStyle dataGridTableStyle = new DataGridTableStyle();
			dataGridTableStyle.MappingName = "Table1";
			DataGridColumnStyle dataGridColumnStyle = new DataGridTextBoxColumn();
			dataGridColumnStyle.MappingName = "First Name";
			dataGridColumnStyle.HeaderText = SR.GetString("DataGridAutoFormatTableFirstColumn");
			DataGridColumnStyle dataGridColumnStyle2 = new DataGridTextBoxColumn();
			dataGridColumnStyle2.MappingName = "Last Name";
			dataGridColumnStyle2.HeaderText = SR.GetString("DataGridAutoFormatTableSecondColumn");
			dataGridTableStyle.GridColumnStyles.Add(dataGridColumnStyle);
			dataGridTableStyle.GridColumnStyles.Add(dataGridColumnStyle2);
			DataRowCollection rows = this.dataSet.Tables["Scheme"].Rows;
			DataRow dataRow = rows[0];
			dataRow["SchemeName"] = SR.GetString("DataGridAutoFormatSchemeNameDefault");
			dataRow = rows[1];
			dataRow["SchemeName"] = SR.GetString("DataGridAutoFormatSchemeNameProfessional1");
			dataRow = rows[2];
			dataRow["SchemeName"] = SR.GetString("DataGridAutoFormatSchemeNameProfessional2");
			dataRow = rows[3];
			dataRow["SchemeName"] = SR.GetString("DataGridAutoFormatSchemeNameProfessional3");
			dataRow = rows[4];
			dataRow["SchemeName"] = SR.GetString("DataGridAutoFormatSchemeNameProfessional4");
			dataRow = rows[5];
			dataRow["SchemeName"] = SR.GetString("DataGridAutoFormatSchemeNameClassic");
			dataRow = rows[6];
			dataRow["SchemeName"] = SR.GetString("DataGridAutoFormatSchemeNameSimple");
			dataRow = rows[7];
			dataRow["SchemeName"] = SR.GetString("DataGridAutoFormatSchemeNameColorful1");
			dataRow = rows[8];
			dataRow["SchemeName"] = SR.GetString("DataGridAutoFormatSchemeNameColorful2");
			dataRow = rows[9];
			dataRow["SchemeName"] = SR.GetString("DataGridAutoFormatSchemeNameColorful3");
			dataRow = rows[10];
			dataRow["SchemeName"] = SR.GetString("DataGridAutoFormatSchemeNameColorful4");
			dataRow = rows[11];
			dataRow["SchemeName"] = SR.GetString("DataGridAutoFormatSchemeName256Color1");
			dataRow = rows[12];
			dataRow["SchemeName"] = SR.GetString("DataGridAutoFormatSchemeName256Color2");
			this.dataGrid.TableStyles.Add(dataGridTableStyle);
			this.tableStyle = dataGridTableStyle;
		}

		private void AddDataToDataGrid()
		{
			DataTable dataTable = new DataTable("Table1");
			dataTable.Locale = CultureInfo.InvariantCulture;
			dataTable.Columns.Add(new DataColumn("First Name"));
			dataTable.Columns.Add(new DataColumn("Last Name"));
			DataRow dataRow = dataTable.NewRow();
			dataRow["First Name"] = "Robert";
			dataRow["Last Name"] = "Brown";
			dataTable.Rows.Add(dataRow);
			dataRow = dataTable.NewRow();
			dataRow["First Name"] = "Nate";
			dataRow["Last Name"] = "Sun";
			dataTable.Rows.Add(dataRow);
			dataRow = dataTable.NewRow();
			dataRow["First Name"] = "Carole";
			dataRow["Last Name"] = "Poland";
			dataTable.Rows.Add(dataRow);
			this.dataGrid.SetDataBinding(dataTable, "");
		}

		private void AutoFormat_HelpRequested(object sender, HelpEventArgs e)
		{
			if (this.dgrid == null || this.dgrid.Site == null)
			{
				return;
			}
			IDesignerHost designerHost = this.dgrid.Site.GetService(typeof(IDesignerHost)) as IDesignerHost;
			if (designerHost == null)
			{
				return;
			}
			IHelpService helpService = designerHost.GetService(typeof(IHelpService)) as IHelpService;
			if (helpService != null)
			{
				helpService.ShowHelpFromKeyword("vs.DataGridAutoFormatDialog");
			}
		}

		private void Button1_Clicked(object sender, EventArgs e)
		{
			this.selectedIndex = this.schemeName.SelectedIndex;
		}

		private static bool IsTableProperty(string propName)
		{
			return propName.Equals("HeaderColor") || propName.Equals("AlternatingBackColor") || propName.Equals("BackColor") || propName.Equals("ForeColor") || propName.Equals("GridLineColor") || propName.Equals("GridLineStyle") || propName.Equals("HeaderBackColor") || propName.Equals("HeaderForeColor") || propName.Equals("LinkColor") || propName.Equals("LinkHoverColor") || propName.Equals("SelectionForeColor") || propName.Equals("SelectionBackColor") || propName.Equals("HeaderFont");
		}

		private void SchemeName_SelectionChanged(object sender, EventArgs e)
		{
			if (this.IMBusy)
			{
				return;
			}
			DataRow row = ((DataRowView)this.schemeName.SelectedItem).Row;
			if (row != null)
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(DataGrid));
				PropertyDescriptorCollection properties2 = TypeDescriptor.GetProperties(typeof(DataGridTableStyle));
				foreach (object obj in row.Table.Columns)
				{
					DataColumn dataColumn = (DataColumn)obj;
					object obj2 = row[dataColumn];
					PropertyDescriptor propertyDescriptor;
					object obj3;
					if (DataGridAutoFormatDialog.IsTableProperty(dataColumn.ColumnName))
					{
						propertyDescriptor = properties2[dataColumn.ColumnName];
						obj3 = this.tableStyle;
					}
					else
					{
						propertyDescriptor = properties[dataColumn.ColumnName];
						obj3 = this.dataGrid;
					}
					if (propertyDescriptor != null)
					{
						if (Convert.IsDBNull(obj2) || obj2.ToString().Length == 0)
						{
							propertyDescriptor.ResetValue(obj3);
						}
						else
						{
							try
							{
								TypeConverter converter = propertyDescriptor.Converter;
								object obj4 = converter.ConvertFromString(obj2.ToString());
								propertyDescriptor.SetValue(obj3, obj4);
							}
							catch
							{
							}
						}
					}
				}
			}
		}

		public DataRow SelectedData
		{
			get
			{
				if (this.schemeName != null)
				{
					return ((DataRowView)this.schemeName.Items[this.selectedIndex]).Row;
				}
				return null;
			}
		}

		internal const string scheme = "<xsd:schema id=\"pulica\" xmlns=\"\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\"><xsd:element name=\"Scheme\"><xsd:complexType><xsd:all><xsd:element name=\"SchemeName\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"SchemePicture\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"BorderStyle\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"FlatMode\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"Font\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"CaptionFont\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"HeaderFont\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"AlternatingBackColor\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"BackColor\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"BackgroundColor\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"CaptionForeColor\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"CaptionBackColor\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"ForeColor\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"GridLineColor\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"GridLineStyle\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"HeaderBackColor\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"HeaderForeColor\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"LinkColor\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"LinkHoverColor\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"ParentRowsBackColor\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"ParentRowsForeColor\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"SelectionForeColor\" minOccurs=\"0\" type=\"xsd:string\"/><xsd:element name=\"SelectionBackColor\" minOccurs=\"0\" type=\"xsd:string\"/></xsd:all></xsd:complexType></xsd:element></xsd:schema>";

		internal const string data = "<pulica><Scheme><SchemeName>Default</SchemeName><SchemePicture>default.bmp</SchemePicture><BorderStyle></BorderStyle><FlatMode></FlatMode><CaptionFont></CaptionFont><Font></Font><HeaderFont></HeaderFont><AlternatingBackColor></AlternatingBackColor><BackColor></BackColor><CaptionForeColor></CaptionForeColor><CaptionBackColor></CaptionBackColor><ForeColor></ForeColor><GridLineColor></GridLineColor><GridLineStyle></GridLineStyle><HeaderBackColor></HeaderBackColor><HeaderForeColor></HeaderForeColor><LinkColor></LinkColor><LinkHoverColor></LinkHoverColor><ParentRowsBackColor></ParentRowsBackColor><ParentRowsForeColor></ParentRowsForeColor><SelectionForeColor></SelectionForeColor><SelectionBackColor></SelectionBackColor></Scheme><Scheme><SchemeName>Professional 1</SchemeName><SchemePicture>professional1.bmp</SchemePicture><CaptionFont>Verdana, 10pt</CaptionFont><AlternatingBackColor>LightGray</AlternatingBackColor><CaptionForeColor>Navy</CaptionForeColor><CaptionBackColor>White</CaptionBackColor><ForeColor>Black</ForeColor><BackColor>DarkGray</BackColor><GridLineColor>Black</GridLineColor><GridLineStyle>None</GridLineStyle><HeaderBackColor>Silver</HeaderBackColor><HeaderForeColor>Black</HeaderForeColor><LinkColor>Navy</LinkColor><LinkHoverColor>Blue</LinkHoverColor><ParentRowsBackColor>White</ParentRowsBackColor><ParentRowsForeColor>Black</ParentRowsForeColor><SelectionForeColor>White</SelectionForeColor><SelectionBackColor>Navy</SelectionBackColor></Scheme><Scheme><SchemeName>Professional 2</SchemeName><SchemePicture>professional2.bmp</SchemePicture><BorderStyle>FixedSingle</BorderStyle><FlatMode>True</FlatMode><CaptionFont>Tahoma, 8pt</CaptionFont><AlternatingBackColor>Gainsboro</AlternatingBackColor><BackColor>Silver</BackColor><CaptionForeColor>White</CaptionForeColor><CaptionBackColor>DarkSlateBlue</CaptionBackColor><ForeColor>Black</ForeColor><GridLineColor>White</GridLineColor><HeaderBackColor>DarkGray</HeaderBackColor><HeaderForeColor>Black</HeaderForeColor><LinkColor>DarkSlateBlue</LinkColor><LinkHoverColor>RoyalBlue</LinkHoverColor><ParentRowsBackColor>Black</ParentRowsBackColor><ParentRowsForeColor>White</ParentRowsForeColor><SelectionForeColor>White</SelectionForeColor><SelectionBackColor>DarkSlateBlue</SelectionBackColor></Scheme><Scheme><SchemeName>Professional 3</SchemeName><SchemePicture>professional3.bmp</SchemePicture><BorderStyle>None</BorderStyle><FlatMode>True</FlatMode><CaptionFont>Tahoma, 8pt, style=1</CaptionFont><HeaderFont>Tahoma, 8pt, style=1</HeaderFont><Font>Tahoma, 8pt</Font><AlternatingBackColor>LightGray</AlternatingBackColor><BackColor>Gainsboro</BackColor><BackgroundColor>Silver</BackgroundColor><CaptionForeColor>MidnightBlue</CaptionForeColor><CaptionBackColor>LightSteelBlue</CaptionBackColor><ForeColor>Black</ForeColor><GridLineColor>DimGray</GridLineColor><GridLineStyle>None</GridLineStyle><HeaderBackColor>MidnightBlue</HeaderBackColor><HeaderForeColor>White</HeaderForeColor><LinkColor>MidnightBlue</LinkColor><LinkHoverColor>RoyalBlue</LinkHoverColor><ParentRowsBackColor>DarkGray</ParentRowsBackColor><ParentRowsForeColor>Black</ParentRowsForeColor><SelectionForeColor>White</SelectionForeColor><SelectionBackColor>CadetBlue</SelectionBackColor></Scheme><Scheme><SchemeName>Professional 4</SchemeName><SchemePicture>professional4.bmp</SchemePicture><BorderStyle>None</BorderStyle><FlatMode>True</FlatMode><CaptionFont>Tahoma, 8pt, style=1</CaptionFont><HeaderFont>Tahoma, 8pt, style=1</HeaderFont><Font>Tahoma, 8pt</Font><AlternatingBackColor>Lavender</AlternatingBackColor><BackColor>WhiteSmoke</BackColor><BackgroundColor>LightGray</BackgroundColor><CaptionForeColor>MidnightBlue</CaptionForeColor><CaptionBackColor>LightSteelBlue</CaptionBackColor><ForeColor>MidnightBlue</ForeColor><GridLineColor>Gainsboro</GridLineColor><GridLineStyle>None</GridLineStyle><HeaderBackColor>MidnightBlue</HeaderBackColor><HeaderForeColor>WhiteSmoke</HeaderForeColor><LinkColor>Teal</LinkColor><LinkHoverColor>DarkMagenta</LinkHoverColor><ParentRowsBackColor>Gainsboro</ParentRowsBackColor><ParentRowsForeColor>MidnightBlue</ParentRowsForeColor><SelectionForeColor>WhiteSmoke</SelectionForeColor><SelectionBackColor>CadetBlue</SelectionBackColor></Scheme><Scheme><SchemeName>Classic</SchemeName><SchemePicture>classic.bmp</SchemePicture><BorderStyle>FixedSingle</BorderStyle><FlatMode>True</FlatMode><Font>Times New Roman, 9pt</Font><HeaderFont>Tahoma, 8pt, style=1</HeaderFont><CaptionFont>Tahoma, 8pt, style=1</CaptionFont><AlternatingBackColor>WhiteSmoke</AlternatingBackColor><BackColor>Gainsboro</BackColor><BackgroundColor>DarkGray</BackgroundColor><CaptionForeColor>Black</CaptionForeColor><CaptionBackColor>DarkKhaki</CaptionBackColor><ForeColor>Black</ForeColor><GridLineColor>Silver</GridLineColor><HeaderBackColor>Black</HeaderBackColor><HeaderForeColor>White</HeaderForeColor><LinkColor>DarkSlateBlue</LinkColor><LinkHoverColor>Firebrick</LinkHoverColor><ParentRowsForeColor>Black</ParentRowsForeColor><ParentRowsBackColor>LightGray</ParentRowsBackColor><SelectionForeColor>White</SelectionForeColor><SelectionBackColor>Firebrick</SelectionBackColor></Scheme><Scheme><SchemeName>Simple</SchemeName><SchemePicture>Simple.bmp</SchemePicture><BorderStyle>FixedSingle</BorderStyle><FlatMode>True</FlatMode><Font>Courier New, 9pt</Font><HeaderFont>Courier New, 10pt, style=1</HeaderFont><CaptionFont>Courier New, 10pt, style=1</CaptionFont><AlternatingBackColor>White</AlternatingBackColor><BackColor>White</BackColor><BackgroundColor>Gainsboro</BackgroundColor><CaptionForeColor>Black</CaptionForeColor><CaptionBackColor>Silver</CaptionBackColor><ForeColor>DarkSlateGray</ForeColor><GridLineColor>DarkGray</GridLineColor><HeaderBackColor>DarkGreen</HeaderBackColor><HeaderForeColor>White</HeaderForeColor><LinkColor>DarkGreen</LinkColor><LinkHoverColor>Blue</LinkHoverColor><ParentRowsForeColor>Black</ParentRowsForeColor><ParentRowsBackColor>Gainsboro</ParentRowsBackColor><SelectionForeColor>Black</SelectionForeColor><SelectionBackColor>DarkSeaGreen</SelectionBackColor></Scheme><Scheme><SchemeName>Colorful 1</SchemeName><SchemePicture>colorful1.bmp</SchemePicture><BorderStyle>FixedSingle</BorderStyle><FlatMode>True</FlatMode><Font>Tahoma, 8pt</Font><CaptionFont>Tahoma, 9pt, style=1</CaptionFont><HeaderFont>Tahoma, 9pt, style=1</HeaderFont><AlternatingBackColor>LightGoldenrodYellow</AlternatingBackColor><BackColor>White</BackColor><BackgroundColor>LightGoldenrodYellow</BackgroundColor><CaptionForeColor>DarkSlateBlue</CaptionForeColor><CaptionBackColor>LightGoldenrodYellow</CaptionBackColor><ForeColor>DarkSlateBlue</ForeColor><GridLineColor>Peru</GridLineColor><GridLineStyle>None</GridLineStyle><HeaderBackColor>Maroon</HeaderBackColor><HeaderForeColor>LightGoldenrodYellow</HeaderForeColor><LinkColor>Maroon</LinkColor><LinkHoverColor>SlateBlue</LinkHoverColor><ParentRowsBackColor>BurlyWood</ParentRowsBackColor><ParentRowsForeColor>DarkSlateBlue</ParentRowsForeColor><SelectionForeColor>GhostWhite</SelectionForeColor><SelectionBackColor>DarkSlateBlue</SelectionBackColor></Scheme><Scheme><SchemeName>Colorful 2</SchemeName><SchemePicture>colorful2.bmp</SchemePicture><BorderStyle>None</BorderStyle><FlatMode>True</FlatMode><Font>Tahoma, 8pt</Font><CaptionFont>Tahoma, 8pt, style=1</CaptionFont><HeaderFont>Tahoma, 8pt, style=1</HeaderFont><AlternatingBackColor>GhostWhite</AlternatingBackColor><BackColor>GhostWhite</BackColor><BackgroundColor>Lavender</BackgroundColor><CaptionForeColor>White</CaptionForeColor><CaptionBackColor>RoyalBlue</CaptionBackColor><ForeColor>MidnightBlue</ForeColor><GridLineColor>RoyalBlue</GridLineColor><HeaderBackColor>MidnightBlue</HeaderBackColor><HeaderForeColor>Lavender</HeaderForeColor><LinkColor>Teal</LinkColor><LinkHoverColor>DodgerBlue</LinkHoverColor><ParentRowsBackColor>Lavender</ParentRowsBackColor><ParentRowsForeColor>MidnightBlue</ParentRowsForeColor><SelectionForeColor>PaleGreen</SelectionForeColor><SelectionBackColor>Teal</SelectionBackColor></Scheme><Scheme><SchemeName>Colorful 3</SchemeName><SchemePicture>colorful3.bmp</SchemePicture><BorderStyle>None</BorderStyle><FlatMode>True</FlatMode><Font>Tahoma, 8pt</Font><CaptionFont>Tahoma, 8pt, style=1</CaptionFont><HeaderFont>Tahoma, 8pt, style=1</HeaderFont><AlternatingBackColor>OldLace</AlternatingBackColor><BackColor>OldLace</BackColor><BackgroundColor>Tan</BackgroundColor><CaptionForeColor>OldLace</CaptionForeColor><CaptionBackColor>SaddleBrown</CaptionBackColor><ForeColor>DarkSlateGray</ForeColor><GridLineColor>Tan</GridLineColor><GridLineStyle>Solid</GridLineStyle><HeaderBackColor>Wheat</HeaderBackColor><HeaderForeColor>SaddleBrown</HeaderForeColor><LinkColor>DarkSlateBlue</LinkColor><LinkHoverColor>Teal</LinkHoverColor><ParentRowsBackColor>OldLace</ParentRowsBackColor><ParentRowsForeColor>DarkSlateGray</ParentRowsForeColor><SelectionForeColor>White</SelectionForeColor><SelectionBackColor>SlateGray</SelectionBackColor></Scheme><Scheme><SchemeName>Colorful 4</SchemeName><SchemePicture>colorful4.bmp</SchemePicture><BorderStyle>FixedSingle</BorderStyle><FlatMode>True</FlatMode><Font>Tahoma, 8pt</Font><CaptionFont>Tahoma, 8pt, style=1</CaptionFont><HeaderFont>Tahoma, 8pt, style=1</HeaderFont><AlternatingBackColor>White</AlternatingBackColor><BackColor>White</BackColor><BackgroundColor>Ivory</BackgroundColor><CaptionForeColor>Lavender</CaptionForeColor><CaptionBackColor>DarkSlateBlue</CaptionBackColor><ForeColor>Black</ForeColor><GridLineColor>Wheat</GridLineColor><HeaderBackColor>CadetBlue</HeaderBackColor><HeaderForeColor>Black</HeaderForeColor><LinkColor>DarkSlateBlue</LinkColor><LinkHoverColor>LightSeaGreen</LinkHoverColor><ParentRowsBackColor>Ivory</ParentRowsBackColor><ParentRowsForeColor>Black</ParentRowsForeColor><SelectionForeColor>DarkSlateBlue</SelectionForeColor><SelectionBackColor>Wheat</SelectionBackColor></Scheme><Scheme><SchemeName>256 Color 1</SchemeName><SchemePicture>256_1.bmp</SchemePicture><Font>Tahoma, 8pt</Font><CaptionFont>Tahoma, 8 pt</CaptionFont><HeaderFont>Tahoma, 8pt</HeaderFont><AlternatingBackColor>Silver</AlternatingBackColor><BackColor>White</BackColor><CaptionForeColor>White</CaptionForeColor><CaptionBackColor>Maroon</CaptionBackColor><ForeColor>Black</ForeColor><GridLineColor>Silver</GridLineColor><HeaderBackColor>Silver</HeaderBackColor><HeaderForeColor>Black</HeaderForeColor><LinkColor>Maroon</LinkColor><LinkHoverColor>Red</LinkHoverColor><ParentRowsBackColor>Silver</ParentRowsBackColor><ParentRowsForeColor>Black</ParentRowsForeColor><SelectionForeColor>White</SelectionForeColor><SelectionBackColor>Maroon</SelectionBackColor></Scheme><Scheme><SchemeName>256 Color 2</SchemeName><SchemePicture>256_2.bmp</SchemePicture><BorderStyle>FixedSingle</BorderStyle><FlatMode>True</FlatMode><CaptionFont>Microsoft Sans Serif, 10 pt, style=1</CaptionFont><Font>Tahoma, 8pt</Font><HeaderFont>Tahoma, 8pt</HeaderFont><AlternatingBackColor>White</AlternatingBackColor><BackColor>White</BackColor><CaptionForeColor>White</CaptionForeColor><CaptionBackColor>Teal</CaptionBackColor><ForeColor>Black</ForeColor><GridLineColor>Silver</GridLineColor><HeaderBackColor>Black</HeaderBackColor><HeaderForeColor>White</HeaderForeColor><LinkColor>Purple</LinkColor><LinkHoverColor>Fuchsia</LinkHoverColor><ParentRowsBackColor>Gray</ParentRowsBackColor><ParentRowsForeColor>White</ParentRowsForeColor><SelectionForeColor>White</SelectionForeColor><SelectionBackColor>Maroon</SelectionBackColor></Scheme></pulica>";

		private DataGrid dgrid;

		private DataTable schemeTable;

		private DataSet dataSet = new DataSet();

		private DataGridTableStyle tableStyle;

		private bool IMBusy;

		private int selectedIndex = -1;

		private class AutoFormatDataGrid : DataGrid
		{
			protected override void OnKeyDown(KeyEventArgs e)
			{
			}

			protected override bool ProcessDialogKey(Keys keyData)
			{
				return false;
			}

			protected override bool ProcessKeyPreview(ref Message m)
			{
				return false;
			}

			protected override void OnMouseDown(MouseEventArgs e)
			{
			}

			protected override void OnMouseUp(MouseEventArgs e)
			{
			}

			protected override void OnMouseMove(MouseEventArgs e)
			{
			}
		}
	}
}
