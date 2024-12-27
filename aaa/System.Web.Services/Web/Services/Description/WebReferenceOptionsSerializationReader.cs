using System;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x0200012B RID: 299
	internal class WebReferenceOptionsSerializationReader : XmlSerializationReader
	{
		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000922 RID: 2338 RVA: 0x00042B88 File Offset: 0x00041B88
		internal Hashtable CodeGenerationOptionsValues
		{
			get
			{
				if (this._CodeGenerationOptionsValues == null)
				{
					this._CodeGenerationOptionsValues = new Hashtable
					{
						{ "properties", 1L },
						{ "newAsync", 2L },
						{ "oldAsync", 4L },
						{ "order", 8L },
						{ "enableDataBinding", 16L }
					};
				}
				return this._CodeGenerationOptionsValues;
			}
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x00042C0B File Offset: 0x00041C0B
		private CodeGenerationOptions Read1_CodeGenerationOptions(string s)
		{
			return (CodeGenerationOptions)XmlSerializationReader.ToEnum(s, this.CodeGenerationOptionsValues, "System.Xml.Serialization.CodeGenerationOptions");
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x00042C20 File Offset: 0x00041C20
		private ServiceDescriptionImportStyle Read2_ServiceDescriptionImportStyle(string s)
		{
			if (s != null)
			{
				if (s == "client")
				{
					return ServiceDescriptionImportStyle.Client;
				}
				if (s == "server")
				{
					return ServiceDescriptionImportStyle.Server;
				}
				if (s == "serverInterface")
				{
					return ServiceDescriptionImportStyle.ServerInterface;
				}
			}
			throw base.CreateUnknownConstantException(s, typeof(ServiceDescriptionImportStyle));
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x00042C74 File Offset: 0x00041C74
		private WebReferenceOptions Read4_WebReferenceOptions(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id1_webReferenceOptions || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			WebReferenceOptions webReferenceOptions = new WebReferenceOptions();
			StringCollection schemaImporterExtensions = webReferenceOptions.SchemaImporterExtensions;
			bool[] array = new bool[4];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(webReferenceOptions);
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return webReferenceOptions;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array[0] && base.Reader.LocalName == this.id3_codeGenerationOptions && base.Reader.NamespaceURI == this.id2_Item)
					{
						if (base.Reader.IsEmptyElement)
						{
							base.Reader.Skip();
						}
						else
						{
							webReferenceOptions.CodeGenerationOptions = this.Read1_CodeGenerationOptions(base.Reader.ReadElementString());
						}
						array[0] = true;
					}
					else if (base.Reader.LocalName == this.id4_schemaImporterExtensions && base.Reader.NamespaceURI == this.id2_Item)
					{
						if (!base.ReadNull())
						{
							StringCollection schemaImporterExtensions2 = webReferenceOptions.SchemaImporterExtensions;
							if (schemaImporterExtensions2 == null || base.Reader.IsEmptyElement)
							{
								base.Reader.Skip();
							}
							else
							{
								base.Reader.ReadStartElement();
								base.Reader.MoveToContent();
								int num2 = 0;
								int readerCount2 = base.ReaderCount;
								while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
								{
									if (base.Reader.NodeType == XmlNodeType.Element)
									{
										if (base.Reader.LocalName == this.id5_type && base.Reader.NamespaceURI == this.id2_Item)
										{
											if (base.ReadNull())
											{
												schemaImporterExtensions2.Add(null);
											}
											else
											{
												schemaImporterExtensions2.Add(base.Reader.ReadElementString());
											}
										}
										else
										{
											base.UnknownNode(null, "http://microsoft.com/webReference/:type");
										}
									}
									else
									{
										base.UnknownNode(null, "http://microsoft.com/webReference/:type");
									}
									base.Reader.MoveToContent();
									base.CheckReaderCount(ref num2, ref readerCount2);
								}
								base.ReadEndElement();
							}
						}
					}
					else if (!array[2] && base.Reader.LocalName == this.id6_style && base.Reader.NamespaceURI == this.id2_Item)
					{
						if (base.Reader.IsEmptyElement)
						{
							base.Reader.Skip();
						}
						else
						{
							webReferenceOptions.Style = this.Read2_ServiceDescriptionImportStyle(base.Reader.ReadElementString());
						}
						array[2] = true;
					}
					else if (!array[3] && base.Reader.LocalName == this.id7_verbose && base.Reader.NamespaceURI == this.id2_Item)
					{
						webReferenceOptions.Verbose = XmlConvert.ToBoolean(base.Reader.ReadElementString());
						array[3] = true;
					}
					else
					{
						base.UnknownNode(webReferenceOptions, "http://microsoft.com/webReference/:codeGenerationOptions, http://microsoft.com/webReference/:schemaImporterExtensions, http://microsoft.com/webReference/:style, http://microsoft.com/webReference/:verbose");
					}
				}
				else
				{
					base.UnknownNode(webReferenceOptions, "http://microsoft.com/webReference/:codeGenerationOptions, http://microsoft.com/webReference/:schemaImporterExtensions, http://microsoft.com/webReference/:style, http://microsoft.com/webReference/:verbose");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return webReferenceOptions;
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x0004300B File Offset: 0x0004200B
		protected override void InitCallbacks()
		{
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x00043010 File Offset: 0x00042010
		internal object Read5_webReferenceOptions()
		{
			object obj = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id1_webReferenceOptions || base.Reader.NamespaceURI != this.id2_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				obj = this.Read4_WebReferenceOptions(true, true);
			}
			else
			{
				base.UnknownNode(null, "http://microsoft.com/webReference/:webReferenceOptions");
			}
			return obj;
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x00043080 File Offset: 0x00042080
		protected override void InitIDs()
		{
			this.id2_Item = base.Reader.NameTable.Add("http://microsoft.com/webReference/");
			this.id5_type = base.Reader.NameTable.Add("type");
			this.id4_schemaImporterExtensions = base.Reader.NameTable.Add("schemaImporterExtensions");
			this.id3_codeGenerationOptions = base.Reader.NameTable.Add("codeGenerationOptions");
			this.id6_style = base.Reader.NameTable.Add("style");
			this.id7_verbose = base.Reader.NameTable.Add("verbose");
			this.id1_webReferenceOptions = base.Reader.NameTable.Add("webReferenceOptions");
		}

		// Token: 0x040005ED RID: 1517
		private Hashtable _CodeGenerationOptionsValues;

		// Token: 0x040005EE RID: 1518
		private string id2_Item;

		// Token: 0x040005EF RID: 1519
		private string id5_type;

		// Token: 0x040005F0 RID: 1520
		private string id4_schemaImporterExtensions;

		// Token: 0x040005F1 RID: 1521
		private string id3_codeGenerationOptions;

		// Token: 0x040005F2 RID: 1522
		private string id6_style;

		// Token: 0x040005F3 RID: 1523
		private string id7_verbose;

		// Token: 0x040005F4 RID: 1524
		private string id1_webReferenceOptions;
	}
}
