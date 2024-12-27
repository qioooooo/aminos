using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x02000129 RID: 297
	[XmlType("webReferenceOptions", Namespace = "http://microsoft.com/webReference/")]
	[XmlRoot("webReferenceOptions", Namespace = "http://microsoft.com/webReference/")]
	public class WebReferenceOptions
	{
		// Token: 0x17000261 RID: 609
		// (get) Token: 0x0600090F RID: 2319 RVA: 0x0004271D File Offset: 0x0004171D
		// (set) Token: 0x06000910 RID: 2320 RVA: 0x00042725 File Offset: 0x00041725
		[DefaultValue(CodeGenerationOptions.GenerateOldAsync)]
		[XmlElement("codeGenerationOptions")]
		public CodeGenerationOptions CodeGenerationOptions
		{
			get
			{
				return this.codeGenerationOptions;
			}
			set
			{
				this.codeGenerationOptions = value;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000911 RID: 2321 RVA: 0x0004272E File Offset: 0x0004172E
		[XmlArray("schemaImporterExtensions")]
		[XmlArrayItem("type")]
		public StringCollection SchemaImporterExtensions
		{
			get
			{
				if (this.schemaImporterExtensions == null)
				{
					this.schemaImporterExtensions = new StringCollection();
				}
				return this.schemaImporterExtensions;
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000912 RID: 2322 RVA: 0x00042749 File Offset: 0x00041749
		// (set) Token: 0x06000913 RID: 2323 RVA: 0x00042751 File Offset: 0x00041751
		[DefaultValue(ServiceDescriptionImportStyle.Client)]
		[XmlElement("style")]
		public ServiceDescriptionImportStyle Style
		{
			get
			{
				return this.style;
			}
			set
			{
				this.style = value;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000914 RID: 2324 RVA: 0x0004275A File Offset: 0x0004175A
		// (set) Token: 0x06000915 RID: 2325 RVA: 0x00042762 File Offset: 0x00041762
		[XmlElement("verbose")]
		public bool Verbose
		{
			get
			{
				return this.verbose;
			}
			set
			{
				this.verbose = value;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000916 RID: 2326 RVA: 0x0004276B File Offset: 0x0004176B
		public static XmlSchema Schema
		{
			get
			{
				if (WebReferenceOptions.schema == null)
				{
					WebReferenceOptions.schema = XmlSchema.Read(new StringReader("<?xml version='1.0' encoding='UTF-8' ?>\r\n<xs:schema xmlns:tns='http://microsoft.com/webReference/' elementFormDefault='qualified' targetNamespace='http://microsoft.com/webReference/' xmlns:xs='http://www.w3.org/2001/XMLSchema'>\r\n  <xs:simpleType name='options'>\r\n    <xs:list>\r\n      <xs:simpleType>\r\n        <xs:restriction base='xs:string'>\r\n          <xs:enumeration value='properties' />\r\n          <xs:enumeration value='newAsync' />\r\n          <xs:enumeration value='oldAsync' />\r\n          <xs:enumeration value='order' />\r\n          <xs:enumeration value='enableDataBinding' />\r\n        </xs:restriction>\r\n      </xs:simpleType>\r\n    </xs:list>\r\n  </xs:simpleType>\r\n  <xs:simpleType name='style'>\r\n    <xs:restriction base='xs:string'>\r\n      <xs:enumeration value='client' />\r\n      <xs:enumeration value='server' />\r\n      <xs:enumeration value='serverInterface' />\r\n    </xs:restriction>\r\n  </xs:simpleType>\r\n  <xs:complexType name='webReferenceOptions'>\r\n    <xs:all>\r\n      <xs:element minOccurs='0' default='oldAsync' name='codeGenerationOptions' type='tns:options' />\r\n      <xs:element minOccurs='0' default='client' name='style' type='tns:style' />\r\n      <xs:element minOccurs='0' default='false' name='verbose' type='xs:boolean' />\r\n      <xs:element minOccurs='0' name='schemaImporterExtensions'>\r\n        <xs:complexType>\r\n          <xs:sequence>\r\n            <xs:element minOccurs='0' maxOccurs='unbounded' name='type' type='xs:string' />\r\n          </xs:sequence>\r\n        </xs:complexType>\r\n      </xs:element>\r\n    </xs:all>\r\n  </xs:complexType>\r\n  <xs:element name='webReferenceOptions' type='tns:webReferenceOptions' />\r\n  <xs:complexType name='wsdlParameters'>\r\n    <xs:all>\r\n      <xs:element minOccurs='0' name='appSettingBaseUrl' type='xs:string' />\r\n      <xs:element minOccurs='0' name='appSettingUrlKey' type='xs:string' />\r\n      <xs:element minOccurs='0' name='domain' type='xs:string' />\r\n      <xs:element minOccurs='0' name='out' type='xs:string' />\r\n      <xs:element minOccurs='0' name='password' type='xs:string' />\r\n      <xs:element minOccurs='0' name='proxy' type='xs:string' />\r\n      <xs:element minOccurs='0' name='proxydomain' type='xs:string' />\r\n      <xs:element minOccurs='0' name='proxypassword' type='xs:string' />\r\n      <xs:element minOccurs='0' name='proxyusername' type='xs:string' />\r\n      <xs:element minOccurs='0' name='username' type='xs:string' />\r\n      <xs:element minOccurs='0' name='namespace' type='xs:string' />\r\n      <xs:element minOccurs='0' name='language' type='xs:string' />\r\n      <xs:element minOccurs='0' name='protocol' type='xs:string' />\r\n      <xs:element minOccurs='0' name='nologo' type='xs:boolean' />\r\n      <xs:element minOccurs='0' name='parsableerrors' type='xs:boolean' />\r\n      <xs:element minOccurs='0' name='sharetypes' type='xs:boolean' />\r\n      <xs:element minOccurs='0' name='webReferenceOptions' type='tns:webReferenceOptions' />\r\n      <xs:element minOccurs='0' name='documents'>\r\n        <xs:complexType>\r\n          <xs:sequence>\r\n            <xs:element minOccurs='0' maxOccurs='unbounded' name='document' type='xs:string' />\r\n          </xs:sequence>\r\n        </xs:complexType>\r\n      </xs:element>\r\n    </xs:all>\r\n  </xs:complexType>\r\n  <xs:element name='wsdlParameters' type='tns:wsdlParameters' />\r\n</xs:schema>"), null);
				}
				return WebReferenceOptions.schema;
			}
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x00042790 File Offset: 0x00041790
		public static WebReferenceOptions Read(TextReader reader, ValidationEventHandler validationEventHandler)
		{
			return WebReferenceOptions.Read(new XmlTextReader(reader)
			{
				XmlResolver = null,
				ProhibitDtd = true
			}, validationEventHandler);
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x000427BC File Offset: 0x000417BC
		public static WebReferenceOptions Read(Stream stream, ValidationEventHandler validationEventHandler)
		{
			return WebReferenceOptions.Read(new XmlTextReader(stream)
			{
				XmlResolver = null,
				ProhibitDtd = true
			}, validationEventHandler);
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x000427E8 File Offset: 0x000417E8
		public static WebReferenceOptions Read(XmlReader xmlReader, ValidationEventHandler validationEventHandler)
		{
			XmlValidatingReader xmlValidatingReader = new XmlValidatingReader(xmlReader);
			xmlValidatingReader.ValidationType = ValidationType.Schema;
			if (validationEventHandler != null)
			{
				xmlValidatingReader.ValidationEventHandler += validationEventHandler;
			}
			else
			{
				xmlValidatingReader.ValidationEventHandler += WebReferenceOptions.SchemaValidationHandler;
			}
			xmlValidatingReader.Schemas.Add(WebReferenceOptions.Schema);
			webReferenceOptionsSerializer webReferenceOptionsSerializer = new webReferenceOptionsSerializer();
			WebReferenceOptions webReferenceOptions;
			try
			{
				webReferenceOptions = (WebReferenceOptions)webReferenceOptionsSerializer.Deserialize(xmlValidatingReader);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			catch
			{
				throw;
			}
			finally
			{
				xmlValidatingReader.Close();
			}
			return webReferenceOptions;
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x00042880 File Offset: 0x00041880
		private static void SchemaValidationHandler(object sender, ValidationEventArgs args)
		{
			if (args.Severity != XmlSeverityType.Error)
			{
				return;
			}
			throw new InvalidOperationException(Res.GetString("WsdlInstanceValidationDetails", new object[]
			{
				args.Message,
				args.Exception.LineNumber.ToString(CultureInfo.InvariantCulture),
				args.Exception.LinePosition.ToString(CultureInfo.InvariantCulture)
			}));
		}

		// Token: 0x040005E7 RID: 1511
		public const string TargetNamespace = "http://microsoft.com/webReference/";

		// Token: 0x040005E8 RID: 1512
		private static XmlSchema schema;

		// Token: 0x040005E9 RID: 1513
		private CodeGenerationOptions codeGenerationOptions = CodeGenerationOptions.GenerateOldAsync;

		// Token: 0x040005EA RID: 1514
		private ServiceDescriptionImportStyle style;

		// Token: 0x040005EB RID: 1515
		private StringCollection schemaImporterExtensions;

		// Token: 0x040005EC RID: 1516
		private bool verbose;
	}
}
