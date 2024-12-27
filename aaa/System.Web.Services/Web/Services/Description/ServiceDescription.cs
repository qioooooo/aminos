using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.Services.Configuration;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000DF RID: 223
	[XmlRoot("definitions", Namespace = "http://schemas.xmlsoap.org/wsdl/")]
	[XmlFormatExtensionPoint("Extensions")]
	public sealed class ServiceDescription : NamedItem
	{
		// Token: 0x060005E4 RID: 1508 RVA: 0x0001CAB8 File Offset: 0x0001BAB8
		private static void InstanceValidation(object sender, ValidationEventArgs args)
		{
			ServiceDescription.warnings.Add(Res.GetString("WsdlInstanceValidationDetails", new object[]
			{
				args.Message,
				args.Exception.LineNumber.ToString(CultureInfo.InvariantCulture),
				args.Exception.LinePosition.ToString(CultureInfo.InvariantCulture)
			}));
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060005E5 RID: 1509 RVA: 0x0001CB21 File Offset: 0x0001BB21
		// (set) Token: 0x060005E6 RID: 1510 RVA: 0x0001CB37 File Offset: 0x0001BB37
		[XmlIgnore]
		public string RetrievalUrl
		{
			get
			{
				if (this.retrievalUrl != null)
				{
					return this.retrievalUrl;
				}
				return string.Empty;
			}
			set
			{
				this.retrievalUrl = value;
			}
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0001CB40 File Offset: 0x0001BB40
		internal void SetParent(ServiceDescriptionCollection parent)
		{
			this.parent = parent;
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060005E8 RID: 1512 RVA: 0x0001CB49 File Offset: 0x0001BB49
		[XmlIgnore]
		public ServiceDescriptionCollection ServiceDescriptions
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060005E9 RID: 1513 RVA: 0x0001CB51 File Offset: 0x0001BB51
		[XmlIgnore]
		public override ServiceDescriptionFormatExtensionCollection Extensions
		{
			get
			{
				if (this.extensions == null)
				{
					this.extensions = new ServiceDescriptionFormatExtensionCollection(this);
				}
				return this.extensions;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060005EA RID: 1514 RVA: 0x0001CB6D File Offset: 0x0001BB6D
		[XmlElement("import")]
		public ImportCollection Imports
		{
			get
			{
				if (this.imports == null)
				{
					this.imports = new ImportCollection(this);
				}
				return this.imports;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060005EB RID: 1515 RVA: 0x0001CB89 File Offset: 0x0001BB89
		// (set) Token: 0x060005EC RID: 1516 RVA: 0x0001CBA4 File Offset: 0x0001BBA4
		[XmlElement("types")]
		public Types Types
		{
			get
			{
				if (this.types == null)
				{
					this.types = new Types();
				}
				return this.types;
			}
			set
			{
				this.types = value;
			}
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x0001CBAD File Offset: 0x0001BBAD
		private bool ShouldSerializeTypes()
		{
			return this.Types.HasItems();
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060005EE RID: 1518 RVA: 0x0001CBBA File Offset: 0x0001BBBA
		[XmlElement("message")]
		public MessageCollection Messages
		{
			get
			{
				if (this.messages == null)
				{
					this.messages = new MessageCollection(this);
				}
				return this.messages;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060005EF RID: 1519 RVA: 0x0001CBD6 File Offset: 0x0001BBD6
		[XmlElement("portType")]
		public PortTypeCollection PortTypes
		{
			get
			{
				if (this.portTypes == null)
				{
					this.portTypes = new PortTypeCollection(this);
				}
				return this.portTypes;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060005F0 RID: 1520 RVA: 0x0001CBF2 File Offset: 0x0001BBF2
		[XmlElement("binding")]
		public BindingCollection Bindings
		{
			get
			{
				if (this.bindings == null)
				{
					this.bindings = new BindingCollection(this);
				}
				return this.bindings;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060005F1 RID: 1521 RVA: 0x0001CC0E File Offset: 0x0001BC0E
		[XmlElement("service")]
		public ServiceCollection Services
		{
			get
			{
				if (this.services == null)
				{
					this.services = new ServiceCollection(this);
				}
				return this.services;
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060005F2 RID: 1522 RVA: 0x0001CC2A File Offset: 0x0001BC2A
		// (set) Token: 0x060005F3 RID: 1523 RVA: 0x0001CC32 File Offset: 0x0001BC32
		[XmlAttribute("targetNamespace")]
		public string TargetNamespace
		{
			get
			{
				return this.targetNamespace;
			}
			set
			{
				this.targetNamespace = value;
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060005F4 RID: 1524 RVA: 0x0001CC3B File Offset: 0x0001BC3B
		public static XmlSchema Schema
		{
			get
			{
				if (ServiceDescription.schema == null)
				{
					ServiceDescription.schema = XmlSchema.Read(new StringReader("<?xml version='1.0' encoding='UTF-8' ?> \r\n<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'\r\n           xmlns:wsdl='http://schemas.xmlsoap.org/wsdl/'\r\n           targetNamespace='http://schemas.xmlsoap.org/wsdl/'\r\n           elementFormDefault='qualified' >\r\n   \r\n  <xs:complexType mixed='true' name='tDocumentation' >\r\n    <xs:sequence>\r\n      <xs:any minOccurs='0' maxOccurs='unbounded' processContents='lax' />\r\n    </xs:sequence>\r\n  </xs:complexType>\r\n\r\n  <xs:complexType name='tDocumented' >\r\n    <xs:annotation>\r\n      <xs:documentation>\r\n      This type is extended by  component types to allow them to be documented\r\n      </xs:documentation>\r\n    </xs:annotation>\r\n    <xs:sequence>\r\n      <xs:element name='documentation' type='wsdl:tDocumentation' minOccurs='0' />\r\n    </xs:sequence>\r\n  </xs:complexType>\r\n <!-- allow extensibility via elements and attributes on all elements swa124 -->\r\n <xs:complexType name='tExtensibleAttributesDocumented' abstract='true' >\r\n    <xs:complexContent>\r\n      <xs:extension base='wsdl:tDocumented' >\r\n        <xs:annotation>\r\n          <xs:documentation>\r\n          This type is extended by component types to allow attributes from other namespaces to be added.\r\n          </xs:documentation>\r\n        </xs:annotation>\r\n        <xs:sequence>\r\n          <xs:any namespace='##other' minOccurs='0' maxOccurs='unbounded' processContents='lax' />\r\n        </xs:sequence>\r\n        <xs:anyAttribute namespace='##other' processContents='lax' />   \r\n      </xs:extension>\r\n    </xs:complexContent>\r\n  </xs:complexType>\r\n  <xs:complexType name='tExtensibleDocumented' abstract='true' >\r\n    <xs:complexContent>\r\n      <xs:extension base='wsdl:tDocumented' >\r\n        <xs:annotation>\r\n          <xs:documentation>\r\n          This type is extended by component types to allow elements from other namespaces to be added.\r\n          </xs:documentation>\r\n        </xs:annotation>\r\n        <xs:sequence>\r\n          <xs:any namespace='##other' minOccurs='0' maxOccurs='unbounded' processContents='lax' />\r\n        </xs:sequence>\r\n        <xs:anyAttribute namespace='##other' processContents='lax' />   \r\n      </xs:extension>\r\n    </xs:complexContent>\r\n  </xs:complexType>\r\n  <!-- original wsdl removed as part of swa124 resolution\r\n  <xs:complexType name='tExtensibleAttributesDocumented' abstract='true' >\r\n    <xs:complexContent>\r\n      <xs:extension base='wsdl:tDocumented' >\r\n        <xs:annotation>\r\n          <xs:documentation>\r\n          This type is extended by component types to allow attributes from other namespaces to be added.\r\n          </xs:documentation>\r\n        </xs:annotation>\r\n        <xs:anyAttribute namespace='##other' processContents='lax' />    \r\n      </xs:extension>\r\n    </xs:complexContent>\r\n  </xs:complexType>\r\n\r\n  <xs:complexType name='tExtensibleDocumented' abstract='true' >\r\n    <xs:complexContent>\r\n      <xs:extension base='wsdl:tDocumented' >\r\n        <xs:annotation>\r\n          <xs:documentation>\r\n          This type is extended by component types to allow elements from other namespaces to be added.\r\n          </xs:documentation>\r\n        </xs:annotation>\r\n        <xs:sequence>\r\n          <xs:any namespace='##other' minOccurs='0' maxOccurs='unbounded' processContents='lax' />\r\n        </xs:sequence>\r\n      </xs:extension>\r\n    </xs:complexContent>\r\n  </xs:complexType>\r\n -->\r\n  <xs:element name='definitions' type='wsdl:tDefinitions' >\r\n    <xs:key name='message' >\r\n      <xs:selector xpath='wsdl:message' />\r\n      <xs:field xpath='@name' />\r\n    </xs:key>\r\n    <xs:key name='portType' >\r\n      <xs:selector xpath='wsdl:portType' />\r\n      <xs:field xpath='@name' />\r\n    </xs:key>\r\n    <xs:key name='binding' >\r\n      <xs:selector xpath='wsdl:binding' />\r\n      <xs:field xpath='@name' />\r\n    </xs:key>\r\n    <xs:key name='service' >\r\n      <xs:selector xpath='wsdl:service' />\r\n      <xs:field xpath='@name' />\r\n    </xs:key>\r\n    <xs:key name='import' >\r\n      <xs:selector xpath='wsdl:import' />\r\n      <xs:field xpath='@namespace' />\r\n    </xs:key>\r\n  </xs:element>\r\n\r\n  <xs:group name='anyTopLevelOptionalElement' >\r\n    <xs:annotation>\r\n      <xs:documentation>\r\n      Any top level optional element allowed to appear more then once - any child of definitions element except wsdl:types. Any extensibility element is allowed in any place.\r\n      </xs:documentation>\r\n    </xs:annotation>\r\n    <xs:choice>\r\n      <xs:element name='import' type='wsdl:tImport' />\r\n      <xs:element name='types' type='wsdl:tTypes' />                     \r\n      <xs:element name='message'  type='wsdl:tMessage' >\r\n        <xs:unique name='part' >\r\n          <xs:selector xpath='wsdl:part' />\r\n          <xs:field xpath='@name' />\r\n        </xs:unique>\r\n      </xs:element>\r\n      <xs:element name='portType' type='wsdl:tPortType' />\r\n      <xs:element name='binding'  type='wsdl:tBinding' />\r\n      <xs:element name='service'  type='wsdl:tService' >\r\n        <xs:unique name='port' >\r\n          <xs:selector xpath='wsdl:port' />\r\n          <xs:field xpath='@name' />\r\n        </xs:unique>\r\n\t  </xs:element>\r\n    </xs:choice>\r\n  </xs:group>\r\n\r\n  <xs:complexType name='tDefinitions' >\r\n    <xs:complexContent>\r\n      <xs:extension base='wsdl:tExtensibleDocumented' >\r\n        <xs:sequence>\r\n          <xs:group ref='wsdl:anyTopLevelOptionalElement'  minOccurs='0'   maxOccurs='unbounded' />\r\n        </xs:sequence>\r\n        <xs:attribute name='targetNamespace' type='xs:anyURI' use='optional' />\r\n        <xs:attribute name='name' type='xs:NCName' use='optional' />\r\n      </xs:extension>\r\n    </xs:complexContent>\r\n  </xs:complexType>\r\n   \r\n  <xs:complexType name='tImport' >\r\n    <xs:complexContent>\r\n      <xs:extension base='wsdl:tExtensibleAttributesDocumented' >\r\n        <xs:attribute name='namespace' type='xs:anyURI' use='required' />\r\n        <xs:attribute name='location' type='xs:anyURI' use='required' />\r\n      </xs:extension>\r\n    </xs:complexContent>\r\n  </xs:complexType>\r\n   \r\n  <xs:complexType name='tTypes' >\r\n    <xs:complexContent>   \r\n      <xs:extension base='wsdl:tExtensibleDocumented' />\r\n    </xs:complexContent>   \r\n  </xs:complexType>\r\n     \r\n  <xs:complexType name='tMessage' >\r\n    <xs:complexContent>   \r\n      <xs:extension base='wsdl:tExtensibleDocumented' >\r\n        <xs:sequence>\r\n          <xs:element name='part' type='wsdl:tPart' minOccurs='0' maxOccurs='unbounded' />\r\n        </xs:sequence>\r\n        <xs:attribute name='name' type='xs:NCName' use='required' />\r\n      </xs:extension>\r\n    </xs:complexContent>   \r\n  </xs:complexType>\r\n\r\n  <xs:complexType name='tPart' >\r\n    <xs:complexContent>   \r\n      <xs:extension base='wsdl:tExtensibleAttributesDocumented' >\r\n        <xs:attribute name='name' type='xs:NCName' use='required' />\r\n        <xs:attribute name='element' type='xs:QName' use='optional' />\r\n        <xs:attribute name='type' type='xs:QName' use='optional' />    \r\n      </xs:extension>\r\n    </xs:complexContent>   \r\n  </xs:complexType>\r\n\r\n  <xs:complexType name='tPortType' >\r\n    <xs:complexContent>   \r\n      <xs:extension base='wsdl:tExtensibleAttributesDocumented' >\r\n        <xs:sequence>\r\n          <xs:element name='operation' type='wsdl:tOperation' minOccurs='0' maxOccurs='unbounded' />\r\n        </xs:sequence>\r\n        <xs:attribute name='name' type='xs:NCName' use='required' />\r\n      </xs:extension>\r\n    </xs:complexContent>   \r\n  </xs:complexType>\r\n   \r\n  <xs:complexType name='tOperation' >\r\n    <xs:complexContent>   \r\n      <xs:extension base='wsdl:tExtensibleDocumented' >\r\n\t    <xs:sequence>\r\n          <xs:choice>\r\n            <xs:group ref='wsdl:request-response-or-one-way-operation' />\r\n            <xs:group ref='wsdl:solicit-response-or-notification-operation' />\r\n          </xs:choice>\r\n        </xs:sequence>\r\n        <xs:attribute name='name' type='xs:NCName' use='required' />\r\n        <xs:attribute name='parameterOrder' type='xs:NMTOKENS' use='optional' />\r\n      </xs:extension>\r\n    </xs:complexContent>   \r\n  </xs:complexType>\r\n    \r\n  <xs:group name='request-response-or-one-way-operation' >\r\n    <xs:sequence>\r\n      <xs:element name='input' type='wsdl:tParam' />\r\n\t  <xs:sequence minOccurs='0' >\r\n\t    <xs:element name='output' type='wsdl:tParam' />\r\n\t\t<xs:element name='fault' type='wsdl:tFault' minOccurs='0' maxOccurs='unbounded' />\r\n      </xs:sequence>\r\n    </xs:sequence>\r\n  </xs:group>\r\n\r\n  <xs:group name='solicit-response-or-notification-operation' >\r\n    <xs:sequence>\r\n      <xs:element name='output' type='wsdl:tParam' />\r\n\t  <xs:sequence minOccurs='0' >\r\n\t    <xs:element name='input' type='wsdl:tParam' />\r\n\t\t<xs:element name='fault' type='wsdl:tFault' minOccurs='0' maxOccurs='unbounded' />\r\n\t  </xs:sequence>\r\n    </xs:sequence>\r\n  </xs:group>\r\n        \r\n  <xs:complexType name='tParam' >\r\n    <xs:complexContent>\r\n      <xs:extension base='wsdl:tExtensibleAttributesDocumented' >\r\n        <xs:attribute name='name' type='xs:NCName' use='optional' />\r\n        <xs:attribute name='message' type='xs:QName' use='required' />\r\n      </xs:extension>\r\n    </xs:complexContent>\r\n  </xs:complexType>\r\n\r\n  <xs:complexType name='tFault' >\r\n    <xs:complexContent>\r\n      <xs:extension base='wsdl:tExtensibleAttributesDocumented' >\r\n        <xs:attribute name='name' type='xs:NCName'  use='required' />\r\n        <xs:attribute name='message' type='xs:QName' use='required' />\r\n      </xs:extension>\r\n    </xs:complexContent>\r\n  </xs:complexType>\r\n     \r\n  <xs:complexType name='tBinding' >\r\n    <xs:complexContent>\r\n      <xs:extension base='wsdl:tExtensibleDocumented' >\r\n        <xs:sequence>\r\n          <xs:element name='operation' type='wsdl:tBindingOperation' minOccurs='0' maxOccurs='unbounded' />\r\n        </xs:sequence>\r\n        <xs:attribute name='name' type='xs:NCName' use='required' />\r\n        <xs:attribute name='type' type='xs:QName' use='required' />\r\n      </xs:extension>\r\n    </xs:complexContent>\r\n  </xs:complexType>\r\n    \r\n  <xs:complexType name='tBindingOperationMessage' >\r\n    <xs:complexContent>\r\n      <xs:extension base='wsdl:tExtensibleDocumented' >\r\n        <xs:attribute name='name' type='xs:NCName' use='optional' />\r\n      </xs:extension>\r\n    </xs:complexContent>\r\n  </xs:complexType>\r\n  \r\n  <xs:complexType name='tBindingOperationFault' >\r\n    <xs:complexContent>\r\n      <xs:extension base='wsdl:tExtensibleDocumented' >\r\n        <xs:attribute name='name' type='xs:NCName' use='required' />\r\n      </xs:extension>\r\n    </xs:complexContent>\r\n  </xs:complexType>\r\n\r\n  <xs:complexType name='tBindingOperation' >\r\n    <xs:complexContent>\r\n      <xs:extension base='wsdl:tExtensibleDocumented' >\r\n        <xs:sequence>\r\n          <xs:element name='input' type='wsdl:tBindingOperationMessage' minOccurs='0' />\r\n          <xs:element name='output' type='wsdl:tBindingOperationMessage' minOccurs='0' />\r\n          <xs:element name='fault' type='wsdl:tBindingOperationFault' minOccurs='0' maxOccurs='unbounded' />\r\n        </xs:sequence>\r\n        <xs:attribute name='name' type='xs:NCName' use='required' />\r\n      </xs:extension>\r\n    </xs:complexContent>\r\n  </xs:complexType>\r\n     \r\n  <xs:complexType name='tService' >\r\n    <xs:complexContent>\r\n      <xs:extension base='wsdl:tExtensibleDocumented' >\r\n        <xs:sequence>\r\n          <xs:element name='port' type='wsdl:tPort' minOccurs='0' maxOccurs='unbounded' />\r\n        </xs:sequence>\r\n        <xs:attribute name='name' type='xs:NCName' use='required' />\r\n      </xs:extension>\r\n    </xs:complexContent>\r\n  </xs:complexType>\r\n     \r\n  <xs:complexType name='tPort' >\r\n    <xs:complexContent>\r\n      <xs:extension base='wsdl:tExtensibleDocumented' >\r\n        <xs:attribute name='name' type='xs:NCName' use='required' />\r\n        <xs:attribute name='binding' type='xs:QName' use='required' />\r\n      </xs:extension>\r\n    </xs:complexContent>\r\n  </xs:complexType>\r\n\r\n  <xs:attribute name='arrayType' type='xs:string' />\r\n  <xs:attribute name='required' type='xs:boolean' />\r\n  <xs:complexType name='tExtensibilityElement' abstract='true' >\r\n    <xs:attribute ref='wsdl:required' use='optional' />\r\n  </xs:complexType>\r\n\r\n</xs:schema>"), null);
				}
				return ServiceDescription.schema;
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060005F5 RID: 1525 RVA: 0x0001CC5E File Offset: 0x0001BC5E
		internal static XmlSchema SoapEncodingSchema
		{
			get
			{
				if (ServiceDescription.soapEncodingSchema == null)
				{
					ServiceDescription.soapEncodingSchema = XmlSchema.Read(new StringReader("<?xml version='1.0' encoding='UTF-8' ?>\r\n<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'\r\n           xmlns:tns='http://schemas.xmlsoap.org/soap/encoding/'\r\n           targetNamespace='http://schemas.xmlsoap.org/soap/encoding/' >\r\n        \r\n <xs:attribute name='root' >\r\n   <xs:simpleType>\r\n     <xs:restriction base='xs:boolean'>\r\n\t   <xs:pattern value='0|1' />\r\n\t </xs:restriction>\r\n   </xs:simpleType>\r\n </xs:attribute>\r\n\r\n  <xs:attributeGroup name='commonAttributes' >\r\n    <xs:attribute name='id' type='xs:ID' />\r\n    <xs:attribute name='href' type='xs:anyURI' />\r\n    <xs:anyAttribute namespace='##other' processContents='lax' />\r\n  </xs:attributeGroup>\r\n   \r\n  <xs:simpleType name='arrayCoordinate' >\r\n    <xs:restriction base='xs:string' />\r\n  </xs:simpleType>\r\n          \r\n  <xs:attribute name='arrayType' type='xs:string' />\r\n  <xs:attribute name='offset' type='tns:arrayCoordinate' />\r\n  \r\n  <xs:attributeGroup name='arrayAttributes' >\r\n    <xs:attribute ref='tns:arrayType' />\r\n    <xs:attribute ref='tns:offset' />\r\n  </xs:attributeGroup>    \r\n  \r\n  <xs:attribute name='position' type='tns:arrayCoordinate' /> \r\n  \r\n  <xs:attributeGroup name='arrayMemberAttributes' >\r\n    <xs:attribute ref='tns:position' />\r\n  </xs:attributeGroup>    \r\n\r\n  <xs:group name='Array' >\r\n    <xs:sequence>\r\n      <xs:any namespace='##any' minOccurs='0' maxOccurs='unbounded' processContents='lax' />\r\n\t</xs:sequence>\r\n  </xs:group>\r\n\r\n  <xs:element name='Array' type='tns:Array' />\r\n  <xs:complexType name='Array' >\r\n    <xs:group ref='tns:Array' minOccurs='0' />\r\n    <xs:attributeGroup ref='tns:arrayAttributes' />\r\n    <xs:attributeGroup ref='tns:commonAttributes' />\r\n  </xs:complexType> \r\n  <xs:element name='Struct' type='tns:Struct' />\r\n  <xs:group name='Struct' >\r\n    <xs:sequence>\r\n      <xs:any namespace='##any' minOccurs='0' maxOccurs='unbounded' processContents='lax' />\r\n\t</xs:sequence>\r\n  </xs:group>\r\n\r\n  <xs:complexType name='Struct' >\r\n    <xs:group ref='tns:Struct' minOccurs='0' />\r\n    <xs:attributeGroup ref='tns:commonAttributes'/>\r\n  </xs:complexType> \r\n  \r\n  <xs:simpleType name='base64' >\r\n    <xs:restriction base='xs:base64Binary' />\r\n  </xs:simpleType>\r\n\r\n  <xs:element name='duration' type='tns:duration' />\r\n  <xs:complexType name='duration' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:duration' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='dateTime' type='tns:dateTime' />\r\n  <xs:complexType name='dateTime' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:dateTime' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n\r\n\r\n  <xs:element name='NOTATION' type='tns:NOTATION' />\r\n  <xs:complexType name='NOTATION' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:QName' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n  \r\n\r\n  <xs:element name='time' type='tns:time' />\r\n  <xs:complexType name='time' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:time' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='date' type='tns:date' />\r\n  <xs:complexType name='date' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:date' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='gYearMonth' type='tns:gYearMonth' />\r\n  <xs:complexType name='gYearMonth' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:gYearMonth' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='gYear' type='tns:gYear' />\r\n  <xs:complexType name='gYear' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:gYear' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='gMonthDay' type='tns:gMonthDay' />\r\n  <xs:complexType name='gMonthDay' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:gMonthDay' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='gDay' type='tns:gDay' />\r\n  <xs:complexType name='gDay' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:gDay' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='gMonth' type='tns:gMonth' />\r\n  <xs:complexType name='gMonth' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:gMonth' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n  \r\n  <xs:element name='boolean' type='tns:boolean' />\r\n  <xs:complexType name='boolean' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:boolean' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='base64Binary' type='tns:base64Binary' />\r\n  <xs:complexType name='base64Binary' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:base64Binary' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='hexBinary' type='tns:hexBinary' />\r\n  <xs:complexType name='hexBinary' >\r\n    <xs:simpleContent>\r\n     <xs:extension base='xs:hexBinary' >\r\n       <xs:attributeGroup ref='tns:commonAttributes' />\r\n     </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='float' type='tns:float' />\r\n  <xs:complexType name='float' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:float' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='double' type='tns:double' />\r\n  <xs:complexType name='double' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:double' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='anyURI' type='tns:anyURI' />\r\n  <xs:complexType name='anyURI' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:anyURI' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='QName' type='tns:QName' />\r\n  <xs:complexType name='QName' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:QName' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  \r\n  <xs:element name='string' type='tns:string' />\r\n  <xs:complexType name='string' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:string' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='normalizedString' type='tns:normalizedString' />\r\n  <xs:complexType name='normalizedString' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:normalizedString' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='token' type='tns:token' />\r\n  <xs:complexType name='token' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:token' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='language' type='tns:language' />\r\n  <xs:complexType name='language' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:language' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='Name' type='tns:Name' />\r\n  <xs:complexType name='Name' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:Name' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='NMTOKEN' type='tns:NMTOKEN' />\r\n  <xs:complexType name='NMTOKEN' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:NMTOKEN' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='NCName' type='tns:NCName' />\r\n  <xs:complexType name='NCName' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:NCName' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='NMTOKENS' type='tns:NMTOKENS' />\r\n  <xs:complexType name='NMTOKENS' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:NMTOKENS' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='ID' type='tns:ID' />\r\n  <xs:complexType name='ID' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:ID' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='IDREF' type='tns:IDREF' />\r\n  <xs:complexType name='IDREF' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:IDREF' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='ENTITY' type='tns:ENTITY' />\r\n  <xs:complexType name='ENTITY' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:ENTITY' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='IDREFS' type='tns:IDREFS' />\r\n  <xs:complexType name='IDREFS' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:IDREFS' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='ENTITIES' type='tns:ENTITIES' />\r\n  <xs:complexType name='ENTITIES' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:ENTITIES' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='decimal' type='tns:decimal' />\r\n  <xs:complexType name='decimal' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:decimal' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='integer' type='tns:integer' />\r\n  <xs:complexType name='integer' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:integer' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='nonPositiveInteger' type='tns:nonPositiveInteger' />\r\n  <xs:complexType name='nonPositiveInteger' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:nonPositiveInteger' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='negativeInteger' type='tns:negativeInteger' />\r\n  <xs:complexType name='negativeInteger' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:negativeInteger' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='long' type='tns:long' />\r\n  <xs:complexType name='long' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:long' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='int' type='tns:int' />\r\n  <xs:complexType name='int' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:int' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='short' type='tns:short' />\r\n  <xs:complexType name='short' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:short' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='byte' type='tns:byte' />\r\n  <xs:complexType name='byte' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:byte' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='nonNegativeInteger' type='tns:nonNegativeInteger' />\r\n  <xs:complexType name='nonNegativeInteger' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:nonNegativeInteger' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='unsignedLong' type='tns:unsignedLong' />\r\n  <xs:complexType name='unsignedLong' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:unsignedLong' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='unsignedInt' type='tns:unsignedInt' />\r\n  <xs:complexType name='unsignedInt' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:unsignedInt' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='unsignedShort' type='tns:unsignedShort' />\r\n  <xs:complexType name='unsignedShort' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:unsignedShort' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='unsignedByte' type='tns:unsignedByte' />\r\n  <xs:complexType name='unsignedByte' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:unsignedByte' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='positiveInteger' type='tns:positiveInteger' />\r\n  <xs:complexType name='positiveInteger' >\r\n    <xs:simpleContent>\r\n      <xs:extension base='xs:positiveInteger' >\r\n        <xs:attributeGroup ref='tns:commonAttributes' />\r\n      </xs:extension>\r\n    </xs:simpleContent>\r\n  </xs:complexType>\r\n\r\n  <xs:element name='anyType' />\r\n</xs:schema>"), null);
				}
				return ServiceDescription.soapEncodingSchema;
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060005F6 RID: 1526 RVA: 0x0001CC81 File Offset: 0x0001BC81
		[XmlIgnore]
		public StringCollection ValidationWarnings
		{
			get
			{
				if (this.validationWarnings == null)
				{
					this.validationWarnings = new StringCollection();
				}
				return this.validationWarnings;
			}
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0001CC9C File Offset: 0x0001BC9C
		internal void SetWarnings(StringCollection warnings)
		{
			this.validationWarnings = warnings;
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060005F8 RID: 1528 RVA: 0x0001CCA8 File Offset: 0x0001BCA8
		[XmlIgnore]
		public static XmlSerializer Serializer
		{
			get
			{
				if (ServiceDescription.serializer == null)
				{
					WebServicesSection webServicesSection = WebServicesSection.Current;
					XmlAttributeOverrides xmlAttributeOverrides = new XmlAttributeOverrides();
					XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
					xmlSerializerNamespaces.Add("s", "http://www.w3.org/2001/XMLSchema");
					WebServicesSection.LoadXmlFormatExtensions(webServicesSection.GetAllFormatExtensionTypes(), xmlAttributeOverrides, xmlSerializerNamespaces);
					ServiceDescription.namespaces = xmlSerializerNamespaces;
					if (webServicesSection.ServiceDescriptionExtended)
					{
						ServiceDescription.serializer = new XmlSerializer(typeof(ServiceDescription), xmlAttributeOverrides);
					}
					else
					{
						ServiceDescription.serializer = new ServiceDescription.ServiceDescriptionSerializer();
					}
					ServiceDescription.serializer.UnknownElement += RuntimeUtils.OnUnknownElement;
				}
				return ServiceDescription.serializer;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060005F9 RID: 1529 RVA: 0x0001CD35 File Offset: 0x0001BD35
		// (set) Token: 0x060005FA RID: 1530 RVA: 0x0001CD3D File Offset: 0x0001BD3D
		internal string AppSettingBaseUrl
		{
			get
			{
				return this.appSettingBaseUrl;
			}
			set
			{
				this.appSettingBaseUrl = value;
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060005FB RID: 1531 RVA: 0x0001CD46 File Offset: 0x0001BD46
		// (set) Token: 0x060005FC RID: 1532 RVA: 0x0001CD4E File Offset: 0x0001BD4E
		internal string AppSettingUrlKey
		{
			get
			{
				return this.appSettingUrlKey;
			}
			set
			{
				this.appSettingUrlKey = value;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060005FD RID: 1533 RVA: 0x0001CD57 File Offset: 0x0001BD57
		// (set) Token: 0x060005FE RID: 1534 RVA: 0x0001CD5F File Offset: 0x0001BD5F
		internal ServiceDescription Next
		{
			get
			{
				return this.next;
			}
			set
			{
				this.next = value;
			}
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x0001CD68 File Offset: 0x0001BD68
		public static ServiceDescription Read(TextReader textReader)
		{
			return ServiceDescription.Read(textReader, false);
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x0001CD71 File Offset: 0x0001BD71
		public static ServiceDescription Read(Stream stream)
		{
			return ServiceDescription.Read(stream, false);
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x0001CD7A File Offset: 0x0001BD7A
		public static ServiceDescription Read(XmlReader reader)
		{
			return ServiceDescription.Read(reader, false);
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x0001CD83 File Offset: 0x0001BD83
		public static ServiceDescription Read(string fileName)
		{
			return ServiceDescription.Read(fileName, false);
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x0001CD8C File Offset: 0x0001BD8C
		public static ServiceDescription Read(TextReader textReader, bool validate)
		{
			return ServiceDescription.Read(new XmlTextReader(textReader)
			{
				WhitespaceHandling = WhitespaceHandling.Significant,
				XmlResolver = null,
				ProhibitDtd = true
			}, validate);
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x0001CDBC File Offset: 0x0001BDBC
		public static ServiceDescription Read(Stream stream, bool validate)
		{
			return ServiceDescription.Read(new XmlTextReader(stream)
			{
				WhitespaceHandling = WhitespaceHandling.Significant,
				XmlResolver = null,
				ProhibitDtd = true
			}, validate);
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0001CDEC File Offset: 0x0001BDEC
		public static ServiceDescription Read(string fileName, bool validate)
		{
			StreamReader streamReader = new StreamReader(fileName, Encoding.Default, true);
			ServiceDescription serviceDescription;
			try
			{
				serviceDescription = ServiceDescription.Read(streamReader, validate);
			}
			finally
			{
				streamReader.Close();
			}
			return serviceDescription;
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x0001CE28 File Offset: 0x0001BE28
		public static ServiceDescription Read(XmlReader reader, bool validate)
		{
			if (validate)
			{
				XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
				xmlReaderSettings.ValidationType = ValidationType.Schema;
				xmlReaderSettings.ValidationFlags = XmlSchemaValidationFlags.ProcessIdentityConstraints;
				xmlReaderSettings.Schemas.Add(ServiceDescription.Schema);
				xmlReaderSettings.Schemas.Add(SoapBinding.Schema);
				xmlReaderSettings.ValidationEventHandler += ServiceDescription.InstanceValidation;
				ServiceDescription.warnings.Clear();
				XmlReader xmlReader = XmlReader.Create(reader, xmlReaderSettings);
				if (reader.ReadState != ReadState.Initial)
				{
					xmlReader.Read();
				}
				ServiceDescription serviceDescription = (ServiceDescription)ServiceDescription.Serializer.Deserialize(xmlReader);
				serviceDescription.SetWarnings(ServiceDescription.warnings);
				return serviceDescription;
			}
			return (ServiceDescription)ServiceDescription.Serializer.Deserialize(reader);
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x0001CED2 File Offset: 0x0001BED2
		public static bool CanRead(XmlReader reader)
		{
			return ServiceDescription.Serializer.CanDeserialize(reader);
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x0001CEE0 File Offset: 0x0001BEE0
		public void Write(string fileName)
		{
			StreamWriter streamWriter = new StreamWriter(fileName);
			try
			{
				this.Write(streamWriter);
			}
			finally
			{
				streamWriter.Close();
			}
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x0001CF14 File Offset: 0x0001BF14
		public void Write(TextWriter writer)
		{
			this.Write(new XmlTextWriter(writer)
			{
				Formatting = Formatting.Indented,
				Indentation = 2
			});
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x0001CF40 File Offset: 0x0001BF40
		public void Write(Stream stream)
		{
			TextWriter textWriter = new StreamWriter(stream);
			this.Write(textWriter);
			textWriter.Flush();
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0001CF64 File Offset: 0x0001BF64
		public void Write(XmlWriter writer)
		{
			XmlSerializer xmlSerializer = ServiceDescription.Serializer;
			XmlSerializerNamespaces xmlSerializerNamespaces;
			if (base.Namespaces == null || base.Namespaces.Count == 0)
			{
				xmlSerializerNamespaces = new XmlSerializerNamespaces(ServiceDescription.namespaces);
				xmlSerializerNamespaces.Add("wsdl", "http://schemas.xmlsoap.org/wsdl/");
				if (this.TargetNamespace != null && this.TargetNamespace.Length != 0)
				{
					xmlSerializerNamespaces.Add("tns", this.TargetNamespace);
				}
				for (int i = 0; i < this.Types.Schemas.Count; i++)
				{
					string text = this.Types.Schemas[i].TargetNamespace;
					if (text != null && text.Length > 0 && text != this.TargetNamespace && text != "http://schemas.xmlsoap.org/wsdl/")
					{
						xmlSerializerNamespaces.Add("s" + i.ToString(CultureInfo.InvariantCulture), text);
					}
				}
				for (int j = 0; j < this.Imports.Count; j++)
				{
					Import import = this.Imports[j];
					if (import.Namespace.Length > 0)
					{
						xmlSerializerNamespaces.Add("i" + j.ToString(CultureInfo.InvariantCulture), import.Namespace);
					}
				}
			}
			else
			{
				xmlSerializerNamespaces = base.Namespaces;
			}
			xmlSerializer.Serialize(writer, this, xmlSerializerNamespaces);
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x0001D0B4 File Offset: 0x0001C0B4
		internal static WsiProfiles GetConformanceClaims(XmlElement documentation)
		{
			if (documentation == null)
			{
				return WsiProfiles.None;
			}
			WsiProfiles wsiProfiles = WsiProfiles.None;
			XmlNode nextSibling;
			for (XmlNode xmlNode = documentation.FirstChild; xmlNode != null; xmlNode = nextSibling)
			{
				nextSibling = xmlNode.NextSibling;
				if (xmlNode is XmlElement)
				{
					XmlElement xmlElement = (XmlElement)xmlNode;
					if (xmlElement.LocalName == "Claim" && xmlElement.NamespaceURI == "http://ws-i.org/schemas/conformanceClaim/" && "http://ws-i.org/profiles/basic/1.1" == xmlElement.GetAttribute("conformsTo"))
					{
						wsiProfiles |= WsiProfiles.BasicProfile1_1;
					}
				}
			}
			return wsiProfiles;
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x0001D12C File Offset: 0x0001C12C
		internal static void AddConformanceClaims(XmlElement documentation, WsiProfiles claims)
		{
			claims &= WsiProfiles.BasicProfile1_1;
			if (claims == WsiProfiles.None)
			{
				return;
			}
			WsiProfiles conformanceClaims = ServiceDescription.GetConformanceClaims(documentation);
			claims &= ~conformanceClaims;
			if (claims == WsiProfiles.None)
			{
				return;
			}
			XmlDocument ownerDocument = documentation.OwnerDocument;
			if ((claims & WsiProfiles.BasicProfile1_1) != WsiProfiles.None)
			{
				XmlElement xmlElement = ownerDocument.CreateElement("wsi", "Claim", "http://ws-i.org/schemas/conformanceClaim/");
				xmlElement.SetAttribute("conformsTo", "http://ws-i.org/profiles/basic/1.1");
				documentation.InsertBefore(xmlElement, null);
			}
		}

		// Token: 0x0400043B RID: 1083
		public const string Namespace = "http://schemas.xmlsoap.org/wsdl/";

		// Token: 0x0400043C RID: 1084
		internal const string Prefix = "wsdl";

		// Token: 0x0400043D RID: 1085
		private const WsiProfiles SupportedClaims = WsiProfiles.BasicProfile1_1;

		// Token: 0x0400043E RID: 1086
		private Types types;

		// Token: 0x0400043F RID: 1087
		private ImportCollection imports;

		// Token: 0x04000440 RID: 1088
		private MessageCollection messages;

		// Token: 0x04000441 RID: 1089
		private PortTypeCollection portTypes;

		// Token: 0x04000442 RID: 1090
		private BindingCollection bindings;

		// Token: 0x04000443 RID: 1091
		private ServiceCollection services;

		// Token: 0x04000444 RID: 1092
		private string targetNamespace;

		// Token: 0x04000445 RID: 1093
		private ServiceDescriptionFormatExtensionCollection extensions;

		// Token: 0x04000446 RID: 1094
		private ServiceDescriptionCollection parent;

		// Token: 0x04000447 RID: 1095
		private string appSettingUrlKey;

		// Token: 0x04000448 RID: 1096
		private string appSettingBaseUrl;

		// Token: 0x04000449 RID: 1097
		private string retrievalUrl;

		// Token: 0x0400044A RID: 1098
		private static XmlSerializer serializer;

		// Token: 0x0400044B RID: 1099
		private static XmlSerializerNamespaces namespaces;

		// Token: 0x0400044C RID: 1100
		private static XmlSchema schema = null;

		// Token: 0x0400044D RID: 1101
		private static XmlSchema soapEncodingSchema = null;

		// Token: 0x0400044E RID: 1102
		private StringCollection validationWarnings;

		// Token: 0x0400044F RID: 1103
		private static StringCollection warnings = new StringCollection();

		// Token: 0x04000450 RID: 1104
		private ServiceDescription next;

		// Token: 0x020000E0 RID: 224
		internal class ServiceDescriptionSerializer : XmlSerializer
		{
			// Token: 0x06000610 RID: 1552 RVA: 0x0001D1AE File Offset: 0x0001C1AE
			protected override XmlSerializationReader CreateReader()
			{
				return new ServiceDescriptionSerializationReader();
			}

			// Token: 0x06000611 RID: 1553 RVA: 0x0001D1B5 File Offset: 0x0001C1B5
			protected override XmlSerializationWriter CreateWriter()
			{
				return new ServiceDescriptionSerializationWriter();
			}

			// Token: 0x06000612 RID: 1554 RVA: 0x0001D1BC File Offset: 0x0001C1BC
			public override bool CanDeserialize(XmlReader xmlReader)
			{
				return xmlReader.IsStartElement("definitions", "http://schemas.xmlsoap.org/wsdl/");
			}

			// Token: 0x06000613 RID: 1555 RVA: 0x0001D1CE File Offset: 0x0001C1CE
			protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
			{
				((ServiceDescriptionSerializationWriter)writer).Write125_definitions(objectToSerialize);
			}

			// Token: 0x06000614 RID: 1556 RVA: 0x0001D1DC File Offset: 0x0001C1DC
			protected override object Deserialize(XmlSerializationReader reader)
			{
				return ((ServiceDescriptionSerializationReader)reader).Read125_definitions();
			}
		}
	}
}
