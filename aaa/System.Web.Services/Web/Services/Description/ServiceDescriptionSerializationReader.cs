using System;
using System.Collections;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x02000109 RID: 265
	internal class ServiceDescriptionSerializationReader : XmlSerializationReader
	{
		// Token: 0x060007C2 RID: 1986 RVA: 0x00028220 File Offset: 0x00027220
		public object Read125_definitions()
		{
			object obj = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id1_definitions || base.Reader.NamespaceURI != this.id2_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				obj = this.Read124_ServiceDescription(true, true);
			}
			else
			{
				base.UnknownNode(null, "http://schemas.xmlsoap.org/wsdl/:definitions");
			}
			return obj;
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x00028290 File Offset: 0x00027290
		private ServiceDescription Read124_ServiceDescription(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id3_ServiceDescription || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			ServiceDescription serviceDescription = new ServiceDescription();
			XmlAttribute[] array = null;
			int num = 0;
			ServiceDescriptionFormatExtensionCollection extensions = serviceDescription.Extensions;
			ImportCollection imports = serviceDescription.Imports;
			MessageCollection messages = serviceDescription.Messages;
			PortTypeCollection portTypes = serviceDescription.PortTypes;
			BindingCollection bindings = serviceDescription.Bindings;
			ServiceCollection services = serviceDescription.Services;
			bool[] array2 = new bool[12];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[3] && base.Reader.LocalName == this.id4_name && base.Reader.NamespaceURI == this.id5_Item)
				{
					serviceDescription.Name = base.Reader.Value;
					array2[3] = true;
				}
				else if (!array2[11] && base.Reader.LocalName == this.id6_targetNamespace && base.Reader.NamespaceURI == this.id5_Item)
				{
					serviceDescription.TargetNamespace = base.Reader.Value;
					array2[11] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (serviceDescription.Namespaces == null)
					{
						serviceDescription.Namespaces = new XmlSerializerNamespaces();
					}
					serviceDescription.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			serviceDescription.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				serviceDescription.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return serviceDescription;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[0] && base.Reader.LocalName == this.id7_documentation && base.Reader.NamespaceURI == this.id2_Item)
					{
						serviceDescription.DocumentationElement = (XmlElement)base.ReadXmlNode(false);
						array2[0] = true;
					}
					else if (base.Reader.LocalName == this.id8_import && base.Reader.NamespaceURI == this.id2_Item)
					{
						if (imports == null)
						{
							base.Reader.Skip();
						}
						else
						{
							imports.Add(this.Read4_Import(false, true));
						}
					}
					else if (!array2[6] && base.Reader.LocalName == this.id9_types && base.Reader.NamespaceURI == this.id2_Item)
					{
						serviceDescription.Types = this.Read67_Types(false, true);
						array2[6] = true;
					}
					else if (base.Reader.LocalName == this.id10_message && base.Reader.NamespaceURI == this.id2_Item)
					{
						if (messages == null)
						{
							base.Reader.Skip();
						}
						else
						{
							messages.Add(this.Read69_Message(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id11_portType && base.Reader.NamespaceURI == this.id2_Item)
					{
						if (portTypes == null)
						{
							base.Reader.Skip();
						}
						else
						{
							portTypes.Add(this.Read75_PortType(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id12_binding && base.Reader.NamespaceURI == this.id2_Item)
					{
						if (bindings == null)
						{
							base.Reader.Skip();
						}
						else
						{
							bindings.Add(this.Read117_Binding(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id13_service && base.Reader.NamespaceURI == this.id2_Item)
					{
						if (services == null)
						{
							base.Reader.Skip();
						}
						else
						{
							services.Add(this.Read123_Service(false, true));
						}
					}
					else
					{
						extensions.Add((XmlElement)base.ReadXmlNode(false));
					}
				}
				else
				{
					base.UnknownNode(serviceDescription, "http://schemas.xmlsoap.org/wsdl/:documentation, http://schemas.xmlsoap.org/wsdl/:import, http://schemas.xmlsoap.org/wsdl/:types, http://schemas.xmlsoap.org/wsdl/:message, http://schemas.xmlsoap.org/wsdl/:portType, http://schemas.xmlsoap.org/wsdl/:binding, http://schemas.xmlsoap.org/wsdl/:service");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			serviceDescription.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return serviceDescription;
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x000287A0 File Offset: 0x000277A0
		private Service Read123_Service(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id14_Service || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			Service service = new Service();
			XmlAttribute[] array = null;
			int num = 0;
			ServiceDescriptionFormatExtensionCollection extensions = service.Extensions;
			PortCollection ports = service.Ports;
			bool[] array2 = new bool[6];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[3] && base.Reader.LocalName == this.id4_name && base.Reader.NamespaceURI == this.id5_Item)
				{
					service.Name = base.Reader.Value;
					array2[3] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (service.Namespaces == null)
					{
						service.Namespaces = new XmlSerializerNamespaces();
					}
					service.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			service.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				service.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return service;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[0] && base.Reader.LocalName == this.id7_documentation && base.Reader.NamespaceURI == this.id2_Item)
					{
						service.DocumentationElement = (XmlElement)base.ReadXmlNode(false);
						array2[0] = true;
					}
					else if (base.Reader.LocalName == this.id15_port && base.Reader.NamespaceURI == this.id2_Item)
					{
						if (ports == null)
						{
							base.Reader.Skip();
						}
						else
						{
							ports.Add(this.Read122_Port(false, true));
						}
					}
					else
					{
						extensions.Add((XmlElement)base.ReadXmlNode(false));
					}
				}
				else
				{
					base.UnknownNode(service, "http://schemas.xmlsoap.org/wsdl/:documentation, http://schemas.xmlsoap.org/wsdl/:port");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			service.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return service;
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x00028AC8 File Offset: 0x00027AC8
		private Port Read122_Port(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id16_Port || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			Port port = new Port();
			XmlAttribute[] array = null;
			int num = 0;
			ServiceDescriptionFormatExtensionCollection extensions = port.Extensions;
			bool[] array2 = new bool[6];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[3] && base.Reader.LocalName == this.id4_name && base.Reader.NamespaceURI == this.id5_Item)
				{
					port.Name = base.Reader.Value;
					array2[3] = true;
				}
				else if (!array2[5] && base.Reader.LocalName == this.id12_binding && base.Reader.NamespaceURI == this.id5_Item)
				{
					port.Binding = base.ToXmlQualifiedName(base.Reader.Value);
					array2[5] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (port.Namespaces == null)
					{
						port.Namespaces = new XmlSerializerNamespaces();
					}
					port.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			port.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				port.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return port;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[0] && base.Reader.LocalName == this.id7_documentation && base.Reader.NamespaceURI == this.id2_Item)
					{
						port.DocumentationElement = (XmlElement)base.ReadXmlNode(false);
						array2[0] = true;
					}
					else if (base.Reader.LocalName == this.id17_address && base.Reader.NamespaceURI == this.id18_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read118_HttpAddressBinding(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id17_address && base.Reader.NamespaceURI == this.id19_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read119_SoapAddressBinding(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id17_address && base.Reader.NamespaceURI == this.id20_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read121_Soap12AddressBinding(false, true));
						}
					}
					else
					{
						extensions.Add((XmlElement)base.ReadXmlNode(false));
					}
				}
				else
				{
					base.UnknownNode(port, "http://schemas.xmlsoap.org/wsdl/:documentation, http://schemas.xmlsoap.org/wsdl/http/:address, http://schemas.xmlsoap.org/wsdl/soap/:address, http://schemas.xmlsoap.org/wsdl/soap12/:address");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			port.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return port;
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x00028ED0 File Offset: 0x00027ED0
		private Soap12AddressBinding Read121_Soap12AddressBinding(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id21_Soap12AddressBinding || xmlQualifiedName.Namespace != this.id20_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			Soap12AddressBinding soap12AddressBinding = new Soap12AddressBinding();
			bool[] array = new bool[2];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id22_required && base.Reader.NamespaceURI == this.id2_Item)
				{
					soap12AddressBinding.Required = XmlConvert.ToBoolean(base.Reader.Value);
					array[0] = true;
				}
				else if (!array[1] && base.Reader.LocalName == this.id23_location && base.Reader.NamespaceURI == this.id5_Item)
				{
					soap12AddressBinding.Location = base.Reader.Value;
					array[1] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(soap12AddressBinding, "http://schemas.xmlsoap.org/wsdl/:required, :location");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return soap12AddressBinding;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					base.UnknownNode(soap12AddressBinding, "");
				}
				else
				{
					base.UnknownNode(soap12AddressBinding, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return soap12AddressBinding;
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x000290A0 File Offset: 0x000280A0
		private SoapAddressBinding Read119_SoapAddressBinding(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id24_SoapAddressBinding || xmlQualifiedName.Namespace != this.id19_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			SoapAddressBinding soapAddressBinding = new SoapAddressBinding();
			bool[] array = new bool[2];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id22_required && base.Reader.NamespaceURI == this.id2_Item)
				{
					soapAddressBinding.Required = XmlConvert.ToBoolean(base.Reader.Value);
					array[0] = true;
				}
				else if (!array[1] && base.Reader.LocalName == this.id23_location && base.Reader.NamespaceURI == this.id5_Item)
				{
					soapAddressBinding.Location = base.Reader.Value;
					array[1] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(soapAddressBinding, "http://schemas.xmlsoap.org/wsdl/:required, :location");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return soapAddressBinding;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					base.UnknownNode(soapAddressBinding, "");
				}
				else
				{
					base.UnknownNode(soapAddressBinding, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return soapAddressBinding;
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x00029270 File Offset: 0x00028270
		private HttpAddressBinding Read118_HttpAddressBinding(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id25_HttpAddressBinding || xmlQualifiedName.Namespace != this.id18_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			HttpAddressBinding httpAddressBinding = new HttpAddressBinding();
			bool[] array = new bool[2];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id22_required && base.Reader.NamespaceURI == this.id2_Item)
				{
					httpAddressBinding.Required = XmlConvert.ToBoolean(base.Reader.Value);
					array[0] = true;
				}
				else if (!array[1] && base.Reader.LocalName == this.id23_location && base.Reader.NamespaceURI == this.id5_Item)
				{
					httpAddressBinding.Location = base.Reader.Value;
					array[1] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(httpAddressBinding, "http://schemas.xmlsoap.org/wsdl/:required, :location");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return httpAddressBinding;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					base.UnknownNode(httpAddressBinding, "");
				}
				else
				{
					base.UnknownNode(httpAddressBinding, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return httpAddressBinding;
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x00029440 File Offset: 0x00028440
		private Binding Read117_Binding(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id26_Binding || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			Binding binding = new Binding();
			XmlAttribute[] array = null;
			int num = 0;
			ServiceDescriptionFormatExtensionCollection extensions = binding.Extensions;
			OperationBindingCollection operations = binding.Operations;
			bool[] array2 = new bool[7];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[3] && base.Reader.LocalName == this.id4_name && base.Reader.NamespaceURI == this.id5_Item)
				{
					binding.Name = base.Reader.Value;
					array2[3] = true;
				}
				else if (!array2[6] && base.Reader.LocalName == this.id27_type && base.Reader.NamespaceURI == this.id5_Item)
				{
					binding.Type = base.ToXmlQualifiedName(base.Reader.Value);
					array2[6] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (binding.Namespaces == null)
					{
						binding.Namespaces = new XmlSerializerNamespaces();
					}
					binding.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			binding.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				binding.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return binding;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[0] && base.Reader.LocalName == this.id7_documentation && base.Reader.NamespaceURI == this.id2_Item)
					{
						binding.DocumentationElement = (XmlElement)base.ReadXmlNode(false);
						array2[0] = true;
					}
					else if (base.Reader.LocalName == this.id12_binding && base.Reader.NamespaceURI == this.id18_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read77_HttpBinding(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id12_binding && base.Reader.NamespaceURI == this.id19_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read80_SoapBinding(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id12_binding && base.Reader.NamespaceURI == this.id20_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read84_Soap12Binding(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id28_operation && base.Reader.NamespaceURI == this.id2_Item)
					{
						if (operations == null)
						{
							base.Reader.Skip();
						}
						else
						{
							operations.Add(this.Read116_OperationBinding(false, true));
						}
					}
					else
					{
						extensions.Add((XmlElement)base.ReadXmlNode(false));
					}
				}
				else
				{
					base.UnknownNode(binding, "http://schemas.xmlsoap.org/wsdl/:documentation, http://schemas.xmlsoap.org/wsdl/http/:binding, http://schemas.xmlsoap.org/wsdl/soap/:binding, http://schemas.xmlsoap.org/wsdl/soap12/:binding, http://schemas.xmlsoap.org/wsdl/:operation");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			binding.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return binding;
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x000298A0 File Offset: 0x000288A0
		private OperationBinding Read116_OperationBinding(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id29_OperationBinding || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			OperationBinding operationBinding = new OperationBinding();
			XmlAttribute[] array = null;
			int num = 0;
			ServiceDescriptionFormatExtensionCollection extensions = operationBinding.Extensions;
			FaultBindingCollection faults = operationBinding.Faults;
			bool[] array2 = new bool[8];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[3] && base.Reader.LocalName == this.id4_name && base.Reader.NamespaceURI == this.id5_Item)
				{
					operationBinding.Name = base.Reader.Value;
					array2[3] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (operationBinding.Namespaces == null)
					{
						operationBinding.Namespaces = new XmlSerializerNamespaces();
					}
					operationBinding.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			operationBinding.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				operationBinding.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return operationBinding;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[0] && base.Reader.LocalName == this.id7_documentation && base.Reader.NamespaceURI == this.id2_Item)
					{
						operationBinding.DocumentationElement = (XmlElement)base.ReadXmlNode(false);
						array2[0] = true;
					}
					else if (base.Reader.LocalName == this.id28_operation && base.Reader.NamespaceURI == this.id18_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read85_HttpOperationBinding(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id28_operation && base.Reader.NamespaceURI == this.id19_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read86_SoapOperationBinding(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id28_operation && base.Reader.NamespaceURI == this.id20_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read88_Soap12OperationBinding(false, true));
						}
					}
					else if (!array2[5] && base.Reader.LocalName == this.id30_input && base.Reader.NamespaceURI == this.id2_Item)
					{
						operationBinding.Input = this.Read110_InputBinding(false, true);
						array2[5] = true;
					}
					else if (!array2[6] && base.Reader.LocalName == this.id31_output && base.Reader.NamespaceURI == this.id2_Item)
					{
						operationBinding.Output = this.Read111_OutputBinding(false, true);
						array2[6] = true;
					}
					else if (base.Reader.LocalName == this.id32_fault && base.Reader.NamespaceURI == this.id2_Item)
					{
						if (faults == null)
						{
							base.Reader.Skip();
						}
						else
						{
							faults.Add(this.Read115_FaultBinding(false, true));
						}
					}
					else
					{
						extensions.Add((XmlElement)base.ReadXmlNode(false));
					}
				}
				else
				{
					base.UnknownNode(operationBinding, "http://schemas.xmlsoap.org/wsdl/:documentation, http://schemas.xmlsoap.org/wsdl/http/:operation, http://schemas.xmlsoap.org/wsdl/soap/:operation, http://schemas.xmlsoap.org/wsdl/soap12/:operation, http://schemas.xmlsoap.org/wsdl/:input, http://schemas.xmlsoap.org/wsdl/:output, http://schemas.xmlsoap.org/wsdl/:fault");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			operationBinding.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return operationBinding;
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x00029D40 File Offset: 0x00028D40
		private FaultBinding Read115_FaultBinding(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id33_FaultBinding || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			FaultBinding faultBinding = new FaultBinding();
			XmlAttribute[] array = null;
			int num = 0;
			ServiceDescriptionFormatExtensionCollection extensions = faultBinding.Extensions;
			bool[] array2 = new bool[5];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[3] && base.Reader.LocalName == this.id4_name && base.Reader.NamespaceURI == this.id5_Item)
				{
					faultBinding.Name = base.Reader.Value;
					array2[3] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (faultBinding.Namespaces == null)
					{
						faultBinding.Namespaces = new XmlSerializerNamespaces();
					}
					faultBinding.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			faultBinding.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				faultBinding.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return faultBinding;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[0] && base.Reader.LocalName == this.id7_documentation && base.Reader.NamespaceURI == this.id2_Item)
					{
						faultBinding.DocumentationElement = (XmlElement)base.ReadXmlNode(false);
						array2[0] = true;
					}
					else if (base.Reader.LocalName == this.id32_fault && base.Reader.NamespaceURI == this.id19_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read112_SoapFaultBinding(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id32_fault && base.Reader.NamespaceURI == this.id20_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read114_Soap12FaultBinding(false, true));
						}
					}
					else
					{
						extensions.Add((XmlElement)base.ReadXmlNode(false));
					}
				}
				else
				{
					base.UnknownNode(faultBinding, "http://schemas.xmlsoap.org/wsdl/:documentation, http://schemas.xmlsoap.org/wsdl/soap/:fault, http://schemas.xmlsoap.org/wsdl/soap12/:fault");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			faultBinding.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return faultBinding;
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x0002A0AC File Offset: 0x000290AC
		private Soap12FaultBinding Read114_Soap12FaultBinding(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id34_Soap12FaultBinding || xmlQualifiedName.Namespace != this.id20_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			Soap12FaultBinding soap12FaultBinding = new Soap12FaultBinding();
			bool[] array = new bool[5];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id22_required && base.Reader.NamespaceURI == this.id2_Item)
				{
					soap12FaultBinding.Required = XmlConvert.ToBoolean(base.Reader.Value);
					array[0] = true;
				}
				else if (!array[1] && base.Reader.LocalName == this.id35_use && base.Reader.NamespaceURI == this.id5_Item)
				{
					soap12FaultBinding.Use = this.Read100_SoapBindingUse(base.Reader.Value);
					array[1] = true;
				}
				else if (!array[2] && base.Reader.LocalName == this.id4_name && base.Reader.NamespaceURI == this.id5_Item)
				{
					soap12FaultBinding.Name = base.Reader.Value;
					array[2] = true;
				}
				else if (!array[3] && base.Reader.LocalName == this.id36_namespace && base.Reader.NamespaceURI == this.id5_Item)
				{
					soap12FaultBinding.Namespace = base.Reader.Value;
					array[3] = true;
				}
				else if (!array[4] && base.Reader.LocalName == this.id37_encodingStyle && base.Reader.NamespaceURI == this.id5_Item)
				{
					soap12FaultBinding.Encoding = base.Reader.Value;
					array[4] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(soap12FaultBinding, "http://schemas.xmlsoap.org/wsdl/:required, :use, :name, :namespace, :encodingStyle");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return soap12FaultBinding;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					base.UnknownNode(soap12FaultBinding, "");
				}
				else
				{
					base.UnknownNode(soap12FaultBinding, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return soap12FaultBinding;
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x0002A354 File Offset: 0x00029354
		private SoapBindingUse Read100_SoapBindingUse(string s)
		{
			if (s != null)
			{
				if (s == "encoded")
				{
					return SoapBindingUse.Encoded;
				}
				if (s == "literal")
				{
					return SoapBindingUse.Literal;
				}
			}
			throw base.CreateUnknownConstantException(s, typeof(SoapBindingUse));
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x0002A398 File Offset: 0x00029398
		private SoapFaultBinding Read112_SoapFaultBinding(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id38_SoapFaultBinding || xmlQualifiedName.Namespace != this.id19_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			SoapFaultBinding soapFaultBinding = new SoapFaultBinding();
			bool[] array = new bool[5];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id22_required && base.Reader.NamespaceURI == this.id2_Item)
				{
					soapFaultBinding.Required = XmlConvert.ToBoolean(base.Reader.Value);
					array[0] = true;
				}
				else if (!array[1] && base.Reader.LocalName == this.id35_use && base.Reader.NamespaceURI == this.id5_Item)
				{
					soapFaultBinding.Use = this.Read98_SoapBindingUse(base.Reader.Value);
					array[1] = true;
				}
				else if (!array[2] && base.Reader.LocalName == this.id4_name && base.Reader.NamespaceURI == this.id5_Item)
				{
					soapFaultBinding.Name = base.Reader.Value;
					array[2] = true;
				}
				else if (!array[3] && base.Reader.LocalName == this.id36_namespace && base.Reader.NamespaceURI == this.id5_Item)
				{
					soapFaultBinding.Namespace = base.Reader.Value;
					array[3] = true;
				}
				else if (!array[4] && base.Reader.LocalName == this.id37_encodingStyle && base.Reader.NamespaceURI == this.id5_Item)
				{
					soapFaultBinding.Encoding = base.Reader.Value;
					array[4] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(soapFaultBinding, "http://schemas.xmlsoap.org/wsdl/:required, :use, :name, :namespace, :encodingStyle");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return soapFaultBinding;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					base.UnknownNode(soapFaultBinding, "");
				}
				else
				{
					base.UnknownNode(soapFaultBinding, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return soapFaultBinding;
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x0002A640 File Offset: 0x00029640
		private SoapBindingUse Read98_SoapBindingUse(string s)
		{
			if (s != null)
			{
				if (s == "encoded")
				{
					return SoapBindingUse.Encoded;
				}
				if (s == "literal")
				{
					return SoapBindingUse.Literal;
				}
			}
			throw base.CreateUnknownConstantException(s, typeof(SoapBindingUse));
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x0002A684 File Offset: 0x00029684
		private OutputBinding Read111_OutputBinding(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id39_OutputBinding || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			OutputBinding outputBinding = new OutputBinding();
			XmlAttribute[] array = null;
			int num = 0;
			ServiceDescriptionFormatExtensionCollection extensions = outputBinding.Extensions;
			bool[] array2 = new bool[5];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[3] && base.Reader.LocalName == this.id4_name && base.Reader.NamespaceURI == this.id5_Item)
				{
					outputBinding.Name = base.Reader.Value;
					array2[3] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (outputBinding.Namespaces == null)
					{
						outputBinding.Namespaces = new XmlSerializerNamespaces();
					}
					outputBinding.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			outputBinding.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				outputBinding.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return outputBinding;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[0] && base.Reader.LocalName == this.id7_documentation && base.Reader.NamespaceURI == this.id2_Item)
					{
						outputBinding.DocumentationElement = (XmlElement)base.ReadXmlNode(false);
						array2[0] = true;
					}
					else if (base.Reader.LocalName == this.id40_content && base.Reader.NamespaceURI == this.id41_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read93_MimeContentBinding(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id42_mimeXml && base.Reader.NamespaceURI == this.id41_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read94_MimeXmlBinding(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id43_multipartRelated && base.Reader.NamespaceURI == this.id41_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read104_MimeMultipartRelatedBinding(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id44_text && base.Reader.NamespaceURI == this.id45_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read97_MimeTextBinding(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id46_body && base.Reader.NamespaceURI == this.id19_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read99_SoapBodyBinding(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id47_header && base.Reader.NamespaceURI == this.id19_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read106_SoapHeaderBinding(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id46_body && base.Reader.NamespaceURI == this.id20_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read102_Soap12BodyBinding(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id47_header && base.Reader.NamespaceURI == this.id20_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read109_Soap12HeaderBinding(false, true));
						}
					}
					else
					{
						extensions.Add((XmlElement)base.ReadXmlNode(false));
					}
				}
				else
				{
					base.UnknownNode(outputBinding, "http://schemas.xmlsoap.org/wsdl/:documentation, http://schemas.xmlsoap.org/wsdl/mime/:content, http://schemas.xmlsoap.org/wsdl/mime/:mimeXml, http://schemas.xmlsoap.org/wsdl/mime/:multipartRelated, http://microsoft.com/wsdl/mime/textMatching/:text, http://schemas.xmlsoap.org/wsdl/soap/:body, http://schemas.xmlsoap.org/wsdl/soap/:header, http://schemas.xmlsoap.org/wsdl/soap12/:body, http://schemas.xmlsoap.org/wsdl/soap12/:header");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			outputBinding.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return outputBinding;
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x0002ABCC File Offset: 0x00029BCC
		private Soap12HeaderBinding Read109_Soap12HeaderBinding(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id48_Soap12HeaderBinding || xmlQualifiedName.Namespace != this.id20_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			Soap12HeaderBinding soap12HeaderBinding = new Soap12HeaderBinding();
			bool[] array = new bool[7];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id22_required && base.Reader.NamespaceURI == this.id2_Item)
				{
					soap12HeaderBinding.Required = XmlConvert.ToBoolean(base.Reader.Value);
					array[0] = true;
				}
				else if (!array[1] && base.Reader.LocalName == this.id10_message && base.Reader.NamespaceURI == this.id5_Item)
				{
					soap12HeaderBinding.Message = base.ToXmlQualifiedName(base.Reader.Value);
					array[1] = true;
				}
				else if (!array[2] && base.Reader.LocalName == this.id49_part && base.Reader.NamespaceURI == this.id5_Item)
				{
					soap12HeaderBinding.Part = base.Reader.Value;
					array[2] = true;
				}
				else if (!array[3] && base.Reader.LocalName == this.id35_use && base.Reader.NamespaceURI == this.id5_Item)
				{
					soap12HeaderBinding.Use = this.Read100_SoapBindingUse(base.Reader.Value);
					array[3] = true;
				}
				else if (!array[4] && base.Reader.LocalName == this.id37_encodingStyle && base.Reader.NamespaceURI == this.id5_Item)
				{
					soap12HeaderBinding.Encoding = base.Reader.Value;
					array[4] = true;
				}
				else if (!array[5] && base.Reader.LocalName == this.id36_namespace && base.Reader.NamespaceURI == this.id5_Item)
				{
					soap12HeaderBinding.Namespace = base.Reader.Value;
					array[5] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(soap12HeaderBinding, "http://schemas.xmlsoap.org/wsdl/:required, :message, :part, :use, :encodingStyle, :namespace");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return soap12HeaderBinding;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array[6] && base.Reader.LocalName == this.id50_headerfault && base.Reader.NamespaceURI == this.id20_Item)
					{
						soap12HeaderBinding.Fault = this.Read107_SoapHeaderFaultBinding(false, true);
						array[6] = true;
					}
					else
					{
						base.UnknownNode(soap12HeaderBinding, "http://schemas.xmlsoap.org/wsdl/soap12/:headerfault");
					}
				}
				else
				{
					base.UnknownNode(soap12HeaderBinding, "http://schemas.xmlsoap.org/wsdl/soap12/:headerfault");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return soap12HeaderBinding;
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x0002AF00 File Offset: 0x00029F00
		private SoapHeaderFaultBinding Read107_SoapHeaderFaultBinding(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id51_SoapHeaderFaultBinding || xmlQualifiedName.Namespace != this.id20_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			SoapHeaderFaultBinding soapHeaderFaultBinding = new SoapHeaderFaultBinding();
			bool[] array = new bool[6];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id22_required && base.Reader.NamespaceURI == this.id2_Item)
				{
					soapHeaderFaultBinding.Required = XmlConvert.ToBoolean(base.Reader.Value);
					array[0] = true;
				}
				else if (!array[1] && base.Reader.LocalName == this.id10_message && base.Reader.NamespaceURI == this.id5_Item)
				{
					soapHeaderFaultBinding.Message = base.ToXmlQualifiedName(base.Reader.Value);
					array[1] = true;
				}
				else if (!array[2] && base.Reader.LocalName == this.id49_part && base.Reader.NamespaceURI == this.id5_Item)
				{
					soapHeaderFaultBinding.Part = base.Reader.Value;
					array[2] = true;
				}
				else if (!array[3] && base.Reader.LocalName == this.id35_use && base.Reader.NamespaceURI == this.id5_Item)
				{
					soapHeaderFaultBinding.Use = this.Read100_SoapBindingUse(base.Reader.Value);
					array[3] = true;
				}
				else if (!array[4] && base.Reader.LocalName == this.id37_encodingStyle && base.Reader.NamespaceURI == this.id5_Item)
				{
					soapHeaderFaultBinding.Encoding = base.Reader.Value;
					array[4] = true;
				}
				else if (!array[5] && base.Reader.LocalName == this.id36_namespace && base.Reader.NamespaceURI == this.id5_Item)
				{
					soapHeaderFaultBinding.Namespace = base.Reader.Value;
					array[5] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(soapHeaderFaultBinding, "http://schemas.xmlsoap.org/wsdl/:required, :message, :part, :use, :encodingStyle, :namespace");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return soapHeaderFaultBinding;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					base.UnknownNode(soapHeaderFaultBinding, "");
				}
				else
				{
					base.UnknownNode(soapHeaderFaultBinding, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return soapHeaderFaultBinding;
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x0002B1F0 File Offset: 0x0002A1F0
		private Soap12BodyBinding Read102_Soap12BodyBinding(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id52_Soap12BodyBinding || xmlQualifiedName.Namespace != this.id20_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			Soap12BodyBinding soap12BodyBinding = new Soap12BodyBinding();
			bool[] array = new bool[5];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id22_required && base.Reader.NamespaceURI == this.id2_Item)
				{
					soap12BodyBinding.Required = XmlConvert.ToBoolean(base.Reader.Value);
					array[0] = true;
				}
				else if (!array[1] && base.Reader.LocalName == this.id35_use && base.Reader.NamespaceURI == this.id5_Item)
				{
					soap12BodyBinding.Use = this.Read100_SoapBindingUse(base.Reader.Value);
					array[1] = true;
				}
				else if (!array[2] && base.Reader.LocalName == this.id36_namespace && base.Reader.NamespaceURI == this.id5_Item)
				{
					soap12BodyBinding.Namespace = base.Reader.Value;
					array[2] = true;
				}
				else if (!array[3] && base.Reader.LocalName == this.id37_encodingStyle && base.Reader.NamespaceURI == this.id5_Item)
				{
					soap12BodyBinding.Encoding = base.Reader.Value;
					array[3] = true;
				}
				else if (!array[4] && base.Reader.LocalName == this.id53_parts && base.Reader.NamespaceURI == this.id5_Item)
				{
					soap12BodyBinding.PartsString = base.Reader.Value;
					array[4] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(soap12BodyBinding, "http://schemas.xmlsoap.org/wsdl/:required, :use, :namespace, :encodingStyle, :parts");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return soap12BodyBinding;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					base.UnknownNode(soap12BodyBinding, "");
				}
				else
				{
					base.UnknownNode(soap12BodyBinding, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return soap12BodyBinding;
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x0002B498 File Offset: 0x0002A498
		private SoapHeaderBinding Read106_SoapHeaderBinding(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id54_SoapHeaderBinding || xmlQualifiedName.Namespace != this.id19_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			SoapHeaderBinding soapHeaderBinding = new SoapHeaderBinding();
			bool[] array = new bool[7];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id22_required && base.Reader.NamespaceURI == this.id2_Item)
				{
					soapHeaderBinding.Required = XmlConvert.ToBoolean(base.Reader.Value);
					array[0] = true;
				}
				else if (!array[1] && base.Reader.LocalName == this.id10_message && base.Reader.NamespaceURI == this.id5_Item)
				{
					soapHeaderBinding.Message = base.ToXmlQualifiedName(base.Reader.Value);
					array[1] = true;
				}
				else if (!array[2] && base.Reader.LocalName == this.id49_part && base.Reader.NamespaceURI == this.id5_Item)
				{
					soapHeaderBinding.Part = base.Reader.Value;
					array[2] = true;
				}
				else if (!array[3] && base.Reader.LocalName == this.id35_use && base.Reader.NamespaceURI == this.id5_Item)
				{
					soapHeaderBinding.Use = this.Read98_SoapBindingUse(base.Reader.Value);
					array[3] = true;
				}
				else if (!array[4] && base.Reader.LocalName == this.id37_encodingStyle && base.Reader.NamespaceURI == this.id5_Item)
				{
					soapHeaderBinding.Encoding = base.Reader.Value;
					array[4] = true;
				}
				else if (!array[5] && base.Reader.LocalName == this.id36_namespace && base.Reader.NamespaceURI == this.id5_Item)
				{
					soapHeaderBinding.Namespace = base.Reader.Value;
					array[5] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(soapHeaderBinding, "http://schemas.xmlsoap.org/wsdl/:required, :message, :part, :use, :encodingStyle, :namespace");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return soapHeaderBinding;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array[6] && base.Reader.LocalName == this.id50_headerfault && base.Reader.NamespaceURI == this.id19_Item)
					{
						soapHeaderBinding.Fault = this.Read105_SoapHeaderFaultBinding(false, true);
						array[6] = true;
					}
					else
					{
						base.UnknownNode(soapHeaderBinding, "http://schemas.xmlsoap.org/wsdl/soap/:headerfault");
					}
				}
				else
				{
					base.UnknownNode(soapHeaderBinding, "http://schemas.xmlsoap.org/wsdl/soap/:headerfault");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return soapHeaderBinding;
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x0002B7CC File Offset: 0x0002A7CC
		private SoapHeaderFaultBinding Read105_SoapHeaderFaultBinding(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id51_SoapHeaderFaultBinding || xmlQualifiedName.Namespace != this.id19_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			SoapHeaderFaultBinding soapHeaderFaultBinding = new SoapHeaderFaultBinding();
			bool[] array = new bool[6];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id22_required && base.Reader.NamespaceURI == this.id2_Item)
				{
					soapHeaderFaultBinding.Required = XmlConvert.ToBoolean(base.Reader.Value);
					array[0] = true;
				}
				else if (!array[1] && base.Reader.LocalName == this.id10_message && base.Reader.NamespaceURI == this.id5_Item)
				{
					soapHeaderFaultBinding.Message = base.ToXmlQualifiedName(base.Reader.Value);
					array[1] = true;
				}
				else if (!array[2] && base.Reader.LocalName == this.id49_part && base.Reader.NamespaceURI == this.id5_Item)
				{
					soapHeaderFaultBinding.Part = base.Reader.Value;
					array[2] = true;
				}
				else if (!array[3] && base.Reader.LocalName == this.id35_use && base.Reader.NamespaceURI == this.id5_Item)
				{
					soapHeaderFaultBinding.Use = this.Read98_SoapBindingUse(base.Reader.Value);
					array[3] = true;
				}
				else if (!array[4] && base.Reader.LocalName == this.id37_encodingStyle && base.Reader.NamespaceURI == this.id5_Item)
				{
					soapHeaderFaultBinding.Encoding = base.Reader.Value;
					array[4] = true;
				}
				else if (!array[5] && base.Reader.LocalName == this.id36_namespace && base.Reader.NamespaceURI == this.id5_Item)
				{
					soapHeaderFaultBinding.Namespace = base.Reader.Value;
					array[5] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(soapHeaderFaultBinding, "http://schemas.xmlsoap.org/wsdl/:required, :message, :part, :use, :encodingStyle, :namespace");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return soapHeaderFaultBinding;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					base.UnknownNode(soapHeaderFaultBinding, "");
				}
				else
				{
					base.UnknownNode(soapHeaderFaultBinding, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return soapHeaderFaultBinding;
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x0002BABC File Offset: 0x0002AABC
		private SoapBodyBinding Read99_SoapBodyBinding(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id55_SoapBodyBinding || xmlQualifiedName.Namespace != this.id19_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			SoapBodyBinding soapBodyBinding = new SoapBodyBinding();
			bool[] array = new bool[5];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id22_required && base.Reader.NamespaceURI == this.id2_Item)
				{
					soapBodyBinding.Required = XmlConvert.ToBoolean(base.Reader.Value);
					array[0] = true;
				}
				else if (!array[1] && base.Reader.LocalName == this.id35_use && base.Reader.NamespaceURI == this.id5_Item)
				{
					soapBodyBinding.Use = this.Read98_SoapBindingUse(base.Reader.Value);
					array[1] = true;
				}
				else if (!array[2] && base.Reader.LocalName == this.id36_namespace && base.Reader.NamespaceURI == this.id5_Item)
				{
					soapBodyBinding.Namespace = base.Reader.Value;
					array[2] = true;
				}
				else if (!array[3] && base.Reader.LocalName == this.id37_encodingStyle && base.Reader.NamespaceURI == this.id5_Item)
				{
					soapBodyBinding.Encoding = base.Reader.Value;
					array[3] = true;
				}
				else if (!array[4] && base.Reader.LocalName == this.id53_parts && base.Reader.NamespaceURI == this.id5_Item)
				{
					soapBodyBinding.PartsString = base.Reader.Value;
					array[4] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(soapBodyBinding, "http://schemas.xmlsoap.org/wsdl/:required, :use, :namespace, :encodingStyle, :parts");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return soapBodyBinding;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					base.UnknownNode(soapBodyBinding, "");
				}
				else
				{
					base.UnknownNode(soapBodyBinding, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return soapBodyBinding;
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x0002BD64 File Offset: 0x0002AD64
		private MimeTextBinding Read97_MimeTextBinding(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id56_MimeTextBinding || xmlQualifiedName.Namespace != this.id45_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			MimeTextBinding mimeTextBinding = new MimeTextBinding();
			MimeTextMatchCollection matches = mimeTextBinding.Matches;
			bool[] array = new bool[2];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id22_required && base.Reader.NamespaceURI == this.id2_Item)
				{
					mimeTextBinding.Required = XmlConvert.ToBoolean(base.Reader.Value);
					array[0] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(mimeTextBinding, "http://schemas.xmlsoap.org/wsdl/:required");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return mimeTextBinding;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (base.Reader.LocalName == this.id57_match && base.Reader.NamespaceURI == this.id45_Item)
					{
						if (matches == null)
						{
							base.Reader.Skip();
						}
						else
						{
							matches.Add(this.Read96_MimeTextMatch(false, true));
						}
					}
					else
					{
						base.UnknownNode(mimeTextBinding, "http://microsoft.com/wsdl/mime/textMatching/:match");
					}
				}
				else
				{
					base.UnknownNode(mimeTextBinding, "http://microsoft.com/wsdl/mime/textMatching/:match");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return mimeTextBinding;
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x0002BF44 File Offset: 0x0002AF44
		private MimeTextMatch Read96_MimeTextMatch(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id58_MimeTextMatch || xmlQualifiedName.Namespace != this.id45_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			MimeTextMatch mimeTextMatch = new MimeTextMatch();
			MimeTextMatchCollection matches = mimeTextMatch.Matches;
			bool[] array = new bool[8];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id4_name && base.Reader.NamespaceURI == this.id5_Item)
				{
					mimeTextMatch.Name = base.Reader.Value;
					array[0] = true;
				}
				else if (!array[1] && base.Reader.LocalName == this.id27_type && base.Reader.NamespaceURI == this.id5_Item)
				{
					mimeTextMatch.Type = base.Reader.Value;
					array[1] = true;
				}
				else if (!array[2] && base.Reader.LocalName == this.id59_group && base.Reader.NamespaceURI == this.id5_Item)
				{
					mimeTextMatch.Group = XmlConvert.ToInt32(base.Reader.Value);
					array[2] = true;
				}
				else if (!array[3] && base.Reader.LocalName == this.id60_capture && base.Reader.NamespaceURI == this.id5_Item)
				{
					mimeTextMatch.Capture = XmlConvert.ToInt32(base.Reader.Value);
					array[3] = true;
				}
				else if (!array[4] && base.Reader.LocalName == this.id61_repeats && base.Reader.NamespaceURI == this.id5_Item)
				{
					mimeTextMatch.RepeatsString = base.Reader.Value;
					array[4] = true;
				}
				else if (!array[5] && base.Reader.LocalName == this.id62_pattern && base.Reader.NamespaceURI == this.id5_Item)
				{
					mimeTextMatch.Pattern = base.Reader.Value;
					array[5] = true;
				}
				else if (!array[6] && base.Reader.LocalName == this.id63_ignoreCase && base.Reader.NamespaceURI == this.id5_Item)
				{
					mimeTextMatch.IgnoreCase = XmlConvert.ToBoolean(base.Reader.Value);
					array[6] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(mimeTextMatch, ":name, :type, :group, :capture, :repeats, :pattern, :ignoreCase");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return mimeTextMatch;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (base.Reader.LocalName == this.id57_match && base.Reader.NamespaceURI == this.id45_Item)
					{
						if (matches == null)
						{
							base.Reader.Skip();
						}
						else
						{
							matches.Add(this.Read96_MimeTextMatch(false, true));
						}
					}
					else
					{
						base.UnknownNode(mimeTextMatch, "http://microsoft.com/wsdl/mime/textMatching/:match");
					}
				}
				else
				{
					base.UnknownNode(mimeTextMatch, "http://microsoft.com/wsdl/mime/textMatching/:match");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return mimeTextMatch;
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x0002C2DC File Offset: 0x0002B2DC
		private MimeMultipartRelatedBinding Read104_MimeMultipartRelatedBinding(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id64_MimeMultipartRelatedBinding || xmlQualifiedName.Namespace != this.id41_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			MimeMultipartRelatedBinding mimeMultipartRelatedBinding = new MimeMultipartRelatedBinding();
			MimePartCollection parts = mimeMultipartRelatedBinding.Parts;
			bool[] array = new bool[2];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id22_required && base.Reader.NamespaceURI == this.id2_Item)
				{
					mimeMultipartRelatedBinding.Required = XmlConvert.ToBoolean(base.Reader.Value);
					array[0] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(mimeMultipartRelatedBinding, "http://schemas.xmlsoap.org/wsdl/:required");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return mimeMultipartRelatedBinding;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (base.Reader.LocalName == this.id49_part && base.Reader.NamespaceURI == this.id41_Item)
					{
						if (parts == null)
						{
							base.Reader.Skip();
						}
						else
						{
							parts.Add(this.Read103_MimePart(false, true));
						}
					}
					else
					{
						base.UnknownNode(mimeMultipartRelatedBinding, "http://schemas.xmlsoap.org/wsdl/mime/:part");
					}
				}
				else
				{
					base.UnknownNode(mimeMultipartRelatedBinding, "http://schemas.xmlsoap.org/wsdl/mime/:part");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return mimeMultipartRelatedBinding;
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x0002C4BC File Offset: 0x0002B4BC
		private MimePart Read103_MimePart(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id65_MimePart || xmlQualifiedName.Namespace != this.id41_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			MimePart mimePart = new MimePart();
			ServiceDescriptionFormatExtensionCollection extensions = mimePart.Extensions;
			bool[] array = new bool[2];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id22_required && base.Reader.NamespaceURI == this.id2_Item)
				{
					mimePart.Required = XmlConvert.ToBoolean(base.Reader.Value);
					array[0] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(mimePart, "http://schemas.xmlsoap.org/wsdl/:required");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return mimePart;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (base.Reader.LocalName == this.id40_content && base.Reader.NamespaceURI == this.id41_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read93_MimeContentBinding(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id42_mimeXml && base.Reader.NamespaceURI == this.id41_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read94_MimeXmlBinding(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id44_text && base.Reader.NamespaceURI == this.id45_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read97_MimeTextBinding(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id46_body && base.Reader.NamespaceURI == this.id19_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read99_SoapBodyBinding(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id46_body && base.Reader.NamespaceURI == this.id20_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read102_Soap12BodyBinding(false, true));
						}
					}
					else
					{
						extensions.Add((XmlElement)base.ReadXmlNode(false));
					}
				}
				else
				{
					base.UnknownNode(mimePart, "http://schemas.xmlsoap.org/wsdl/mime/:content, http://schemas.xmlsoap.org/wsdl/mime/:mimeXml, http://microsoft.com/wsdl/mime/textMatching/:text, http://schemas.xmlsoap.org/wsdl/soap/:body, http://schemas.xmlsoap.org/wsdl/soap12/:body");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return mimePart;
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x0002C7D4 File Offset: 0x0002B7D4
		private MimeXmlBinding Read94_MimeXmlBinding(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id66_MimeXmlBinding || xmlQualifiedName.Namespace != this.id41_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			MimeXmlBinding mimeXmlBinding = new MimeXmlBinding();
			bool[] array = new bool[2];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id22_required && base.Reader.NamespaceURI == this.id2_Item)
				{
					mimeXmlBinding.Required = XmlConvert.ToBoolean(base.Reader.Value);
					array[0] = true;
				}
				else if (!array[1] && base.Reader.LocalName == this.id49_part && base.Reader.NamespaceURI == this.id5_Item)
				{
					mimeXmlBinding.Part = base.Reader.Value;
					array[1] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(mimeXmlBinding, "http://schemas.xmlsoap.org/wsdl/:required, :part");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return mimeXmlBinding;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					base.UnknownNode(mimeXmlBinding, "");
				}
				else
				{
					base.UnknownNode(mimeXmlBinding, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return mimeXmlBinding;
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x0002C9A4 File Offset: 0x0002B9A4
		private MimeContentBinding Read93_MimeContentBinding(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id67_MimeContentBinding || xmlQualifiedName.Namespace != this.id41_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			MimeContentBinding mimeContentBinding = new MimeContentBinding();
			bool[] array = new bool[3];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id22_required && base.Reader.NamespaceURI == this.id2_Item)
				{
					mimeContentBinding.Required = XmlConvert.ToBoolean(base.Reader.Value);
					array[0] = true;
				}
				else if (!array[1] && base.Reader.LocalName == this.id49_part && base.Reader.NamespaceURI == this.id5_Item)
				{
					mimeContentBinding.Part = base.Reader.Value;
					array[1] = true;
				}
				else if (!array[2] && base.Reader.LocalName == this.id27_type && base.Reader.NamespaceURI == this.id5_Item)
				{
					mimeContentBinding.Type = base.Reader.Value;
					array[2] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(mimeContentBinding, "http://schemas.xmlsoap.org/wsdl/:required, :part, :type");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return mimeContentBinding;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					base.UnknownNode(mimeContentBinding, "");
				}
				else
				{
					base.UnknownNode(mimeContentBinding, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return mimeContentBinding;
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x0002CBBC File Offset: 0x0002BBBC
		private InputBinding Read110_InputBinding(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id68_InputBinding || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			InputBinding inputBinding = new InputBinding();
			XmlAttribute[] array = null;
			int num = 0;
			ServiceDescriptionFormatExtensionCollection extensions = inputBinding.Extensions;
			bool[] array2 = new bool[5];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[3] && base.Reader.LocalName == this.id4_name && base.Reader.NamespaceURI == this.id5_Item)
				{
					inputBinding.Name = base.Reader.Value;
					array2[3] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (inputBinding.Namespaces == null)
					{
						inputBinding.Namespaces = new XmlSerializerNamespaces();
					}
					inputBinding.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			inputBinding.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				inputBinding.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return inputBinding;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[0] && base.Reader.LocalName == this.id7_documentation && base.Reader.NamespaceURI == this.id2_Item)
					{
						inputBinding.DocumentationElement = (XmlElement)base.ReadXmlNode(false);
						array2[0] = true;
					}
					else if (base.Reader.LocalName == this.id69_urlEncoded && base.Reader.NamespaceURI == this.id18_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read90_HttpUrlEncodedBinding(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id70_urlReplacement && base.Reader.NamespaceURI == this.id18_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read91_HttpUrlReplacementBinding(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id40_content && base.Reader.NamespaceURI == this.id41_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read93_MimeContentBinding(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id42_mimeXml && base.Reader.NamespaceURI == this.id41_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read94_MimeXmlBinding(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id43_multipartRelated && base.Reader.NamespaceURI == this.id41_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read104_MimeMultipartRelatedBinding(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id44_text && base.Reader.NamespaceURI == this.id45_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read97_MimeTextBinding(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id46_body && base.Reader.NamespaceURI == this.id19_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read99_SoapBodyBinding(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id47_header && base.Reader.NamespaceURI == this.id19_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read106_SoapHeaderBinding(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id46_body && base.Reader.NamespaceURI == this.id20_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read102_Soap12BodyBinding(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id47_header && base.Reader.NamespaceURI == this.id20_Item)
					{
						if (extensions == null)
						{
							base.Reader.Skip();
						}
						else
						{
							extensions.Add(this.Read109_Soap12HeaderBinding(false, true));
						}
					}
					else
					{
						extensions.Add((XmlElement)base.ReadXmlNode(false));
					}
				}
				else
				{
					base.UnknownNode(inputBinding, "http://schemas.xmlsoap.org/wsdl/:documentation, http://schemas.xmlsoap.org/wsdl/http/:urlEncoded, http://schemas.xmlsoap.org/wsdl/http/:urlReplacement, http://schemas.xmlsoap.org/wsdl/mime/:content, http://schemas.xmlsoap.org/wsdl/mime/:mimeXml, http://schemas.xmlsoap.org/wsdl/mime/:multipartRelated, http://microsoft.com/wsdl/mime/textMatching/:text, http://schemas.xmlsoap.org/wsdl/soap/:body, http://schemas.xmlsoap.org/wsdl/soap/:header, http://schemas.xmlsoap.org/wsdl/soap12/:body, http://schemas.xmlsoap.org/wsdl/soap12/:header");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			inputBinding.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return inputBinding;
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x0002D1A0 File Offset: 0x0002C1A0
		private HttpUrlReplacementBinding Read91_HttpUrlReplacementBinding(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id71_HttpUrlReplacementBinding || xmlQualifiedName.Namespace != this.id18_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			HttpUrlReplacementBinding httpUrlReplacementBinding = new HttpUrlReplacementBinding();
			bool[] array = new bool[1];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id22_required && base.Reader.NamespaceURI == this.id2_Item)
				{
					httpUrlReplacementBinding.Required = XmlConvert.ToBoolean(base.Reader.Value);
					array[0] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(httpUrlReplacementBinding, "http://schemas.xmlsoap.org/wsdl/:required");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return httpUrlReplacementBinding;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					base.UnknownNode(httpUrlReplacementBinding, "");
				}
				else
				{
					base.UnknownNode(httpUrlReplacementBinding, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return httpUrlReplacementBinding;
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x0002D328 File Offset: 0x0002C328
		private HttpUrlEncodedBinding Read90_HttpUrlEncodedBinding(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id72_HttpUrlEncodedBinding || xmlQualifiedName.Namespace != this.id18_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			HttpUrlEncodedBinding httpUrlEncodedBinding = new HttpUrlEncodedBinding();
			bool[] array = new bool[1];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id22_required && base.Reader.NamespaceURI == this.id2_Item)
				{
					httpUrlEncodedBinding.Required = XmlConvert.ToBoolean(base.Reader.Value);
					array[0] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(httpUrlEncodedBinding, "http://schemas.xmlsoap.org/wsdl/:required");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return httpUrlEncodedBinding;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					base.UnknownNode(httpUrlEncodedBinding, "");
				}
				else
				{
					base.UnknownNode(httpUrlEncodedBinding, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return httpUrlEncodedBinding;
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x0002D4B0 File Offset: 0x0002C4B0
		private Soap12OperationBinding Read88_Soap12OperationBinding(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id73_Soap12OperationBinding || xmlQualifiedName.Namespace != this.id20_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			Soap12OperationBinding soap12OperationBinding = new Soap12OperationBinding();
			bool[] array = new bool[4];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id22_required && base.Reader.NamespaceURI == this.id2_Item)
				{
					soap12OperationBinding.Required = XmlConvert.ToBoolean(base.Reader.Value);
					array[0] = true;
				}
				else if (!array[1] && base.Reader.LocalName == this.id74_soapAction && base.Reader.NamespaceURI == this.id5_Item)
				{
					soap12OperationBinding.SoapAction = base.Reader.Value;
					array[1] = true;
				}
				else if (!array[2] && base.Reader.LocalName == this.id75_style && base.Reader.NamespaceURI == this.id5_Item)
				{
					soap12OperationBinding.Style = this.Read82_SoapBindingStyle(base.Reader.Value);
					array[2] = true;
				}
				else if (!array[3] && base.Reader.LocalName == this.id76_soapActionRequired && base.Reader.NamespaceURI == this.id5_Item)
				{
					soap12OperationBinding.SoapActionRequired = XmlConvert.ToBoolean(base.Reader.Value);
					array[3] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(soap12OperationBinding, "http://schemas.xmlsoap.org/wsdl/:required, :soapAction, :style, :soapActionRequired");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return soap12OperationBinding;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					base.UnknownNode(soap12OperationBinding, "");
				}
				else
				{
					base.UnknownNode(soap12OperationBinding, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return soap12OperationBinding;
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x0002D718 File Offset: 0x0002C718
		private SoapBindingStyle Read82_SoapBindingStyle(string s)
		{
			if (s != null)
			{
				if (s == "document")
				{
					return SoapBindingStyle.Document;
				}
				if (s == "rpc")
				{
					return SoapBindingStyle.Rpc;
				}
			}
			throw base.CreateUnknownConstantException(s, typeof(SoapBindingStyle));
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x0002D75C File Offset: 0x0002C75C
		private SoapOperationBinding Read86_SoapOperationBinding(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id77_SoapOperationBinding || xmlQualifiedName.Namespace != this.id19_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			SoapOperationBinding soapOperationBinding = new SoapOperationBinding();
			bool[] array = new bool[3];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id22_required && base.Reader.NamespaceURI == this.id2_Item)
				{
					soapOperationBinding.Required = XmlConvert.ToBoolean(base.Reader.Value);
					array[0] = true;
				}
				else if (!array[1] && base.Reader.LocalName == this.id74_soapAction && base.Reader.NamespaceURI == this.id5_Item)
				{
					soapOperationBinding.SoapAction = base.Reader.Value;
					array[1] = true;
				}
				else if (!array[2] && base.Reader.LocalName == this.id75_style && base.Reader.NamespaceURI == this.id5_Item)
				{
					soapOperationBinding.Style = this.Read79_SoapBindingStyle(base.Reader.Value);
					array[2] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(soapOperationBinding, "http://schemas.xmlsoap.org/wsdl/:required, :soapAction, :style");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return soapOperationBinding;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					base.UnknownNode(soapOperationBinding, "");
				}
				else
				{
					base.UnknownNode(soapOperationBinding, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return soapOperationBinding;
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x0002D978 File Offset: 0x0002C978
		private SoapBindingStyle Read79_SoapBindingStyle(string s)
		{
			if (s != null)
			{
				if (s == "document")
				{
					return SoapBindingStyle.Document;
				}
				if (s == "rpc")
				{
					return SoapBindingStyle.Rpc;
				}
			}
			throw base.CreateUnknownConstantException(s, typeof(SoapBindingStyle));
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x0002D9BC File Offset: 0x0002C9BC
		private HttpOperationBinding Read85_HttpOperationBinding(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id78_HttpOperationBinding || xmlQualifiedName.Namespace != this.id18_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			HttpOperationBinding httpOperationBinding = new HttpOperationBinding();
			bool[] array = new bool[2];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id22_required && base.Reader.NamespaceURI == this.id2_Item)
				{
					httpOperationBinding.Required = XmlConvert.ToBoolean(base.Reader.Value);
					array[0] = true;
				}
				else if (!array[1] && base.Reader.LocalName == this.id23_location && base.Reader.NamespaceURI == this.id5_Item)
				{
					httpOperationBinding.Location = base.Reader.Value;
					array[1] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(httpOperationBinding, "http://schemas.xmlsoap.org/wsdl/:required, :location");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return httpOperationBinding;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					base.UnknownNode(httpOperationBinding, "");
				}
				else
				{
					base.UnknownNode(httpOperationBinding, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return httpOperationBinding;
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x0002DB8C File Offset: 0x0002CB8C
		private Soap12Binding Read84_Soap12Binding(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id79_Soap12Binding || xmlQualifiedName.Namespace != this.id20_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			Soap12Binding soap12Binding = new Soap12Binding();
			bool[] array = new bool[3];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id22_required && base.Reader.NamespaceURI == this.id2_Item)
				{
					soap12Binding.Required = XmlConvert.ToBoolean(base.Reader.Value);
					array[0] = true;
				}
				else if (!array[1] && base.Reader.LocalName == this.id80_transport && base.Reader.NamespaceURI == this.id5_Item)
				{
					soap12Binding.Transport = base.Reader.Value;
					array[1] = true;
				}
				else if (!array[2] && base.Reader.LocalName == this.id75_style && base.Reader.NamespaceURI == this.id5_Item)
				{
					soap12Binding.Style = this.Read82_SoapBindingStyle(base.Reader.Value);
					array[2] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(soap12Binding, "http://schemas.xmlsoap.org/wsdl/:required, :transport, :style");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return soap12Binding;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					base.UnknownNode(soap12Binding, "");
				}
				else
				{
					base.UnknownNode(soap12Binding, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return soap12Binding;
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x0002DDA8 File Offset: 0x0002CDA8
		private SoapBinding Read80_SoapBinding(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id81_SoapBinding || xmlQualifiedName.Namespace != this.id19_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			SoapBinding soapBinding = new SoapBinding();
			bool[] array = new bool[3];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id22_required && base.Reader.NamespaceURI == this.id2_Item)
				{
					soapBinding.Required = XmlConvert.ToBoolean(base.Reader.Value);
					array[0] = true;
				}
				else if (!array[1] && base.Reader.LocalName == this.id80_transport && base.Reader.NamespaceURI == this.id5_Item)
				{
					soapBinding.Transport = base.Reader.Value;
					array[1] = true;
				}
				else if (!array[2] && base.Reader.LocalName == this.id75_style && base.Reader.NamespaceURI == this.id5_Item)
				{
					soapBinding.Style = this.Read79_SoapBindingStyle(base.Reader.Value);
					array[2] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(soapBinding, "http://schemas.xmlsoap.org/wsdl/:required, :transport, :style");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return soapBinding;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					base.UnknownNode(soapBinding, "");
				}
				else
				{
					base.UnknownNode(soapBinding, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return soapBinding;
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x0002DFC4 File Offset: 0x0002CFC4
		private HttpBinding Read77_HttpBinding(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id82_HttpBinding || xmlQualifiedName.Namespace != this.id18_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			HttpBinding httpBinding = new HttpBinding();
			bool[] array = new bool[2];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id22_required && base.Reader.NamespaceURI == this.id2_Item)
				{
					httpBinding.Required = XmlConvert.ToBoolean(base.Reader.Value);
					array[0] = true;
				}
				else if (!array[1] && base.Reader.LocalName == this.id83_verb && base.Reader.NamespaceURI == this.id5_Item)
				{
					httpBinding.Verb = base.Reader.Value;
					array[1] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(httpBinding, "http://schemas.xmlsoap.org/wsdl/:required, :verb");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return httpBinding;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					base.UnknownNode(httpBinding, "");
				}
				else
				{
					base.UnknownNode(httpBinding, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return httpBinding;
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x0002E194 File Offset: 0x0002D194
		private PortType Read75_PortType(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id84_PortType || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			PortType portType = new PortType();
			XmlAttribute[] array = null;
			int num = 0;
			ServiceDescriptionFormatExtensionCollection extensions = portType.Extensions;
			OperationCollection operations = portType.Operations;
			bool[] array2 = new bool[6];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[3] && base.Reader.LocalName == this.id4_name && base.Reader.NamespaceURI == this.id5_Item)
				{
					portType.Name = base.Reader.Value;
					array2[3] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (portType.Namespaces == null)
					{
						portType.Namespaces = new XmlSerializerNamespaces();
					}
					portType.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			portType.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				portType.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return portType;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[0] && base.Reader.LocalName == this.id7_documentation && base.Reader.NamespaceURI == this.id2_Item)
					{
						portType.DocumentationElement = (XmlElement)base.ReadXmlNode(false);
						array2[0] = true;
					}
					else if (base.Reader.LocalName == this.id28_operation && base.Reader.NamespaceURI == this.id2_Item)
					{
						if (operations == null)
						{
							base.Reader.Skip();
						}
						else
						{
							operations.Add(this.Read74_Operation(false, true));
						}
					}
					else
					{
						extensions.Add((XmlElement)base.ReadXmlNode(false));
					}
				}
				else
				{
					base.UnknownNode(portType, "http://schemas.xmlsoap.org/wsdl/:documentation, http://schemas.xmlsoap.org/wsdl/:operation");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			portType.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return portType;
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x0002E4BC File Offset: 0x0002D4BC
		private Operation Read74_Operation(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id85_Operation || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			Operation operation = new Operation();
			XmlAttribute[] array = null;
			int num = 0;
			ServiceDescriptionFormatExtensionCollection extensions = operation.Extensions;
			OperationMessageCollection messages = operation.Messages;
			OperationFaultCollection faults = operation.Faults;
			bool[] array2 = new bool[8];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[3] && base.Reader.LocalName == this.id4_name && base.Reader.NamespaceURI == this.id5_Item)
				{
					operation.Name = base.Reader.Value;
					array2[3] = true;
				}
				else if (!array2[5] && base.Reader.LocalName == this.id86_parameterOrder && base.Reader.NamespaceURI == this.id5_Item)
				{
					operation.ParameterOrderString = base.Reader.Value;
					array2[5] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (operation.Namespaces == null)
					{
						operation.Namespaces = new XmlSerializerNamespaces();
					}
					operation.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			operation.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				operation.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return operation;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[0] && base.Reader.LocalName == this.id7_documentation && base.Reader.NamespaceURI == this.id2_Item)
					{
						operation.DocumentationElement = (XmlElement)base.ReadXmlNode(false);
						array2[0] = true;
					}
					else if (base.Reader.LocalName == this.id30_input && base.Reader.NamespaceURI == this.id2_Item)
					{
						if (messages == null)
						{
							base.Reader.Skip();
						}
						else
						{
							messages.Add(this.Read71_OperationInput(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id31_output && base.Reader.NamespaceURI == this.id2_Item)
					{
						if (messages == null)
						{
							base.Reader.Skip();
						}
						else
						{
							messages.Add(this.Read72_OperationOutput(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id32_fault && base.Reader.NamespaceURI == this.id2_Item)
					{
						if (faults == null)
						{
							base.Reader.Skip();
						}
						else
						{
							faults.Add(this.Read73_OperationFault(false, true));
						}
					}
					else
					{
						extensions.Add((XmlElement)base.ReadXmlNode(false));
					}
				}
				else
				{
					base.UnknownNode(operation, "http://schemas.xmlsoap.org/wsdl/:documentation, http://schemas.xmlsoap.org/wsdl/:input, http://schemas.xmlsoap.org/wsdl/:output, http://schemas.xmlsoap.org/wsdl/:fault");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			operation.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return operation;
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x0002E8D0 File Offset: 0x0002D8D0
		private OperationFault Read73_OperationFault(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id87_OperationFault || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			OperationFault operationFault = new OperationFault();
			XmlAttribute[] array = null;
			int num = 0;
			ServiceDescriptionFormatExtensionCollection extensions = operationFault.Extensions;
			bool[] array2 = new bool[6];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[3] && base.Reader.LocalName == this.id4_name && base.Reader.NamespaceURI == this.id5_Item)
				{
					operationFault.Name = base.Reader.Value;
					array2[3] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id10_message && base.Reader.NamespaceURI == this.id5_Item)
				{
					operationFault.Message = base.ToXmlQualifiedName(base.Reader.Value);
					array2[4] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (operationFault.Namespaces == null)
					{
						operationFault.Namespaces = new XmlSerializerNamespaces();
					}
					operationFault.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			operationFault.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				operationFault.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return operationFault;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[0] && base.Reader.LocalName == this.id7_documentation && base.Reader.NamespaceURI == this.id2_Item)
					{
						operationFault.DocumentationElement = (XmlElement)base.ReadXmlNode(false);
						array2[0] = true;
					}
					else
					{
						extensions.Add((XmlElement)base.ReadXmlNode(false));
					}
				}
				else
				{
					base.UnknownNode(operationFault, "http://schemas.xmlsoap.org/wsdl/:documentation");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			operationFault.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return operationFault;
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0002EBF4 File Offset: 0x0002DBF4
		private OperationOutput Read72_OperationOutput(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id88_OperationOutput || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			OperationOutput operationOutput = new OperationOutput();
			XmlAttribute[] array = null;
			int num = 0;
			ServiceDescriptionFormatExtensionCollection extensions = operationOutput.Extensions;
			bool[] array2 = new bool[6];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[3] && base.Reader.LocalName == this.id4_name && base.Reader.NamespaceURI == this.id5_Item)
				{
					operationOutput.Name = base.Reader.Value;
					array2[3] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id10_message && base.Reader.NamespaceURI == this.id5_Item)
				{
					operationOutput.Message = base.ToXmlQualifiedName(base.Reader.Value);
					array2[4] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (operationOutput.Namespaces == null)
					{
						operationOutput.Namespaces = new XmlSerializerNamespaces();
					}
					operationOutput.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			operationOutput.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				operationOutput.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return operationOutput;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[0] && base.Reader.LocalName == this.id7_documentation && base.Reader.NamespaceURI == this.id2_Item)
					{
						operationOutput.DocumentationElement = (XmlElement)base.ReadXmlNode(false);
						array2[0] = true;
					}
					else
					{
						extensions.Add((XmlElement)base.ReadXmlNode(false));
					}
				}
				else
				{
					base.UnknownNode(operationOutput, "http://schemas.xmlsoap.org/wsdl/:documentation");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			operationOutput.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return operationOutput;
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x0002EF18 File Offset: 0x0002DF18
		private OperationInput Read71_OperationInput(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id89_OperationInput || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			OperationInput operationInput = new OperationInput();
			XmlAttribute[] array = null;
			int num = 0;
			ServiceDescriptionFormatExtensionCollection extensions = operationInput.Extensions;
			bool[] array2 = new bool[6];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[3] && base.Reader.LocalName == this.id4_name && base.Reader.NamespaceURI == this.id5_Item)
				{
					operationInput.Name = base.Reader.Value;
					array2[3] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id10_message && base.Reader.NamespaceURI == this.id5_Item)
				{
					operationInput.Message = base.ToXmlQualifiedName(base.Reader.Value);
					array2[4] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (operationInput.Namespaces == null)
					{
						operationInput.Namespaces = new XmlSerializerNamespaces();
					}
					operationInput.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			operationInput.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				operationInput.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return operationInput;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[0] && base.Reader.LocalName == this.id7_documentation && base.Reader.NamespaceURI == this.id2_Item)
					{
						operationInput.DocumentationElement = (XmlElement)base.ReadXmlNode(false);
						array2[0] = true;
					}
					else
					{
						extensions.Add((XmlElement)base.ReadXmlNode(false));
					}
				}
				else
				{
					base.UnknownNode(operationInput, "http://schemas.xmlsoap.org/wsdl/:documentation");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			operationInput.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return operationInput;
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x0002F23C File Offset: 0x0002E23C
		private Message Read69_Message(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id90_Message || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			Message message = new Message();
			XmlAttribute[] array = null;
			int num = 0;
			ServiceDescriptionFormatExtensionCollection extensions = message.Extensions;
			MessagePartCollection parts = message.Parts;
			bool[] array2 = new bool[6];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[3] && base.Reader.LocalName == this.id4_name && base.Reader.NamespaceURI == this.id5_Item)
				{
					message.Name = base.Reader.Value;
					array2[3] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (message.Namespaces == null)
					{
						message.Namespaces = new XmlSerializerNamespaces();
					}
					message.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			message.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				message.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return message;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[0] && base.Reader.LocalName == this.id7_documentation && base.Reader.NamespaceURI == this.id2_Item)
					{
						message.DocumentationElement = (XmlElement)base.ReadXmlNode(false);
						array2[0] = true;
					}
					else if (base.Reader.LocalName == this.id49_part && base.Reader.NamespaceURI == this.id2_Item)
					{
						if (parts == null)
						{
							base.Reader.Skip();
						}
						else
						{
							parts.Add(this.Read68_MessagePart(false, true));
						}
					}
					else
					{
						extensions.Add((XmlElement)base.ReadXmlNode(false));
					}
				}
				else
				{
					base.UnknownNode(message, "http://schemas.xmlsoap.org/wsdl/:documentation, http://schemas.xmlsoap.org/wsdl/:part");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			message.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return message;
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x0002F564 File Offset: 0x0002E564
		private MessagePart Read68_MessagePart(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id91_MessagePart || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			MessagePart messagePart = new MessagePart();
			XmlAttribute[] array = null;
			int num = 0;
			ServiceDescriptionFormatExtensionCollection extensions = messagePart.Extensions;
			bool[] array2 = new bool[7];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[3] && base.Reader.LocalName == this.id4_name && base.Reader.NamespaceURI == this.id5_Item)
				{
					messagePart.Name = base.Reader.Value;
					array2[3] = true;
				}
				else if (!array2[5] && base.Reader.LocalName == this.id92_element && base.Reader.NamespaceURI == this.id5_Item)
				{
					messagePart.Element = base.ToXmlQualifiedName(base.Reader.Value);
					array2[5] = true;
				}
				else if (!array2[6] && base.Reader.LocalName == this.id27_type && base.Reader.NamespaceURI == this.id5_Item)
				{
					messagePart.Type = base.ToXmlQualifiedName(base.Reader.Value);
					array2[6] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (messagePart.Namespaces == null)
					{
						messagePart.Namespaces = new XmlSerializerNamespaces();
					}
					messagePart.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			messagePart.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				messagePart.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return messagePart;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[0] && base.Reader.LocalName == this.id7_documentation && base.Reader.NamespaceURI == this.id2_Item)
					{
						messagePart.DocumentationElement = (XmlElement)base.ReadXmlNode(false);
						array2[0] = true;
					}
					else
					{
						extensions.Add((XmlElement)base.ReadXmlNode(false));
					}
				}
				else
				{
					base.UnknownNode(messagePart, "http://schemas.xmlsoap.org/wsdl/:documentation");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			messagePart.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return messagePart;
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x0002F8D4 File Offset: 0x0002E8D4
		private Types Read67_Types(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id93_Types || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			Types types = new Types();
			XmlAttribute[] array = null;
			int num = 0;
			ServiceDescriptionFormatExtensionCollection extensions = types.Extensions;
			XmlSchemas schemas = types.Schemas;
			bool[] array2 = new bool[5];
			while (base.Reader.MoveToNextAttribute())
			{
				if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (types.Namespaces == null)
					{
						types.Namespaces = new XmlSerializerNamespaces();
					}
					types.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			types.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				types.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return types;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[0] && base.Reader.LocalName == this.id7_documentation && base.Reader.NamespaceURI == this.id2_Item)
					{
						types.DocumentationElement = (XmlElement)base.ReadXmlNode(false);
						array2[0] = true;
					}
					else if (base.Reader.LocalName == this.id94_schema && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (schemas == null)
						{
							base.Reader.Skip();
						}
						else
						{
							schemas.Add(this.Read66_XmlSchema(false, true));
						}
					}
					else
					{
						extensions.Add((XmlElement)base.ReadXmlNode(false));
					}
				}
				else
				{
					base.UnknownNode(types, "http://schemas.xmlsoap.org/wsdl/:documentation, http://www.w3.org/2001/XMLSchema:schema");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			types.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return types;
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x0002FBB8 File Offset: 0x0002EBB8
		private XmlSchema Read66_XmlSchema(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id96_XmlSchema || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchema xmlSchema = new XmlSchema();
			XmlSchemaObjectCollection includes = xmlSchema.Includes;
			XmlSchemaObjectCollection items = xmlSchema.Items;
			XmlAttribute[] array = null;
			int num = 0;
			bool[] array2 = new bool[11];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id97_attributeFormDefault && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchema.AttributeFormDefault = this.Read6_XmlSchemaForm(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[2] && base.Reader.LocalName == this.id98_blockDefault && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchema.BlockDefault = this.Read7_XmlSchemaDerivationMethod(base.Reader.Value);
					array2[2] = true;
				}
				else if (!array2[3] && base.Reader.LocalName == this.id99_finalDefault && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchema.FinalDefault = this.Read7_XmlSchemaDerivationMethod(base.Reader.Value);
					array2[3] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id100_elementFormDefault && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchema.ElementFormDefault = this.Read6_XmlSchemaForm(base.Reader.Value);
					array2[4] = true;
				}
				else if (!array2[5] && base.Reader.LocalName == this.id6_targetNamespace && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchema.TargetNamespace = base.CollapseWhitespace(base.Reader.Value);
					array2[5] = true;
				}
				else if (!array2[6] && base.Reader.LocalName == this.id101_version && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchema.Version = base.CollapseWhitespace(base.Reader.Value);
					array2[6] = true;
				}
				else if (!array2[9] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchema.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[9] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchema.Namespaces == null)
					{
						xmlSchema.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchema.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchema.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchema.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchema;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (base.Reader.LocalName == this.id103_include && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (includes == null)
						{
							base.Reader.Skip();
						}
						else
						{
							includes.Add(this.Read12_XmlSchemaInclude(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id8_import && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (includes == null)
						{
							base.Reader.Skip();
						}
						else
						{
							includes.Add(this.Read13_XmlSchemaImport(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id104_redefine && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (includes == null)
						{
							base.Reader.Skip();
						}
						else
						{
							includes.Add(this.Read64_XmlSchemaRedefine(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id105_simpleType && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (items == null)
						{
							base.Reader.Skip();
						}
						else
						{
							items.Add(this.Read34_XmlSchemaSimpleType(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id106_complexType && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (items == null)
						{
							base.Reader.Skip();
						}
						else
						{
							items.Add(this.Read62_XmlSchemaComplexType(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (items == null)
						{
							base.Reader.Skip();
						}
						else
						{
							items.Add(this.Read11_XmlSchemaAnnotation(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id108_notation && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (items == null)
						{
							base.Reader.Skip();
						}
						else
						{
							items.Add(this.Read65_XmlSchemaNotation(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id59_group && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (items == null)
						{
							base.Reader.Skip();
						}
						else
						{
							items.Add(this.Read63_XmlSchemaGroup(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id92_element && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (items == null)
						{
							base.Reader.Skip();
						}
						else
						{
							items.Add(this.Read52_XmlSchemaElement(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id109_attribute && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (items == null)
						{
							base.Reader.Skip();
						}
						else
						{
							items.Add(this.Read36_XmlSchemaAttribute(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id110_attributeGroup && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (items == null)
						{
							base.Reader.Skip();
						}
						else
						{
							items.Add(this.Read40_XmlSchemaAttributeGroup(false, true));
						}
					}
					else
					{
						base.UnknownNode(xmlSchema, "http://www.w3.org/2001/XMLSchema:include, http://www.w3.org/2001/XMLSchema:import, http://www.w3.org/2001/XMLSchema:redefine, http://www.w3.org/2001/XMLSchema:simpleType, http://www.w3.org/2001/XMLSchema:complexType, http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:notation, http://www.w3.org/2001/XMLSchema:group, http://www.w3.org/2001/XMLSchema:element, http://www.w3.org/2001/XMLSchema:attribute, http://www.w3.org/2001/XMLSchema:attributeGroup");
					}
				}
				else
				{
					base.UnknownNode(xmlSchema, "http://www.w3.org/2001/XMLSchema:include, http://www.w3.org/2001/XMLSchema:import, http://www.w3.org/2001/XMLSchema:redefine, http://www.w3.org/2001/XMLSchema:simpleType, http://www.w3.org/2001/XMLSchema:complexType, http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:notation, http://www.w3.org/2001/XMLSchema:group, http://www.w3.org/2001/XMLSchema:element, http://www.w3.org/2001/XMLSchema:attribute, http://www.w3.org/2001/XMLSchema:attributeGroup");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchema.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchema;
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x00030384 File Offset: 0x0002F384
		private XmlSchemaAttributeGroup Read40_XmlSchemaAttributeGroup(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id111_XmlSchemaAttributeGroup || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaAttributeGroup xmlSchemaAttributeGroup = new XmlSchemaAttributeGroup();
			XmlAttribute[] array = null;
			int num = 0;
			XmlSchemaObjectCollection attributes = xmlSchemaAttributeGroup.Attributes;
			bool[] array2 = new bool[7];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaAttributeGroup.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id4_name && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaAttributeGroup.Name = base.Reader.Value;
					array2[4] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaAttributeGroup.Namespaces == null)
					{
						xmlSchemaAttributeGroup.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaAttributeGroup.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaAttributeGroup.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaAttributeGroup.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaAttributeGroup;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaAttributeGroup.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else if (base.Reader.LocalName == this.id109_attribute && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (attributes == null)
						{
							base.Reader.Skip();
						}
						else
						{
							attributes.Add(this.Read36_XmlSchemaAttribute(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id110_attributeGroup && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (attributes == null)
						{
							base.Reader.Skip();
						}
						else
						{
							attributes.Add(this.Read37_XmlSchemaAttributeGroupRef(false, true));
						}
					}
					else if (!array2[6] && base.Reader.LocalName == this.id112_anyAttribute && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaAttributeGroup.AnyAttribute = this.Read39_XmlSchemaAnyAttribute(false, true);
						array2[6] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaAttributeGroup, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:attribute, http://www.w3.org/2001/XMLSchema:attributeGroup, http://www.w3.org/2001/XMLSchema:anyAttribute");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaAttributeGroup, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:attribute, http://www.w3.org/2001/XMLSchema:attributeGroup, http://www.w3.org/2001/XMLSchema:anyAttribute");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaAttributeGroup.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaAttributeGroup;
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x00030780 File Offset: 0x0002F780
		private XmlSchemaAnyAttribute Read39_XmlSchemaAnyAttribute(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id113_XmlSchemaAnyAttribute || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaAnyAttribute xmlSchemaAnyAttribute = new XmlSchemaAnyAttribute();
			XmlAttribute[] array = null;
			int num = 0;
			bool[] array2 = new bool[6];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaAnyAttribute.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id36_namespace && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaAnyAttribute.Namespace = base.Reader.Value;
					array2[4] = true;
				}
				else if (!array2[5] && base.Reader.LocalName == this.id114_processContents && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaAnyAttribute.ProcessContents = this.Read38_XmlSchemaContentProcessing(base.Reader.Value);
					array2[5] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaAnyAttribute.Namespaces == null)
					{
						xmlSchemaAnyAttribute.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaAnyAttribute.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaAnyAttribute.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaAnyAttribute.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaAnyAttribute;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaAnyAttribute.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaAnyAttribute, "http://www.w3.org/2001/XMLSchema:annotation");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaAnyAttribute, "http://www.w3.org/2001/XMLSchema:annotation");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaAnyAttribute.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaAnyAttribute;
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x00030AE0 File Offset: 0x0002FAE0
		private XmlSchemaAnnotation Read11_XmlSchemaAnnotation(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id115_XmlSchemaAnnotation || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaAnnotation xmlSchemaAnnotation = new XmlSchemaAnnotation();
			XmlSchemaObjectCollection items = xmlSchemaAnnotation.Items;
			XmlAttribute[] array = null;
			int num = 0;
			bool[] array2 = new bool[4];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaAnnotation.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaAnnotation.Namespaces == null)
					{
						xmlSchemaAnnotation.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaAnnotation.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaAnnotation.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaAnnotation.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaAnnotation;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (base.Reader.LocalName == this.id7_documentation && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (items == null)
						{
							base.Reader.Skip();
						}
						else
						{
							items.Add(this.Read9_XmlSchemaDocumentation(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id116_appinfo && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (items == null)
						{
							base.Reader.Skip();
						}
						else
						{
							items.Add(this.Read10_XmlSchemaAppInfo(false, true));
						}
					}
					else
					{
						base.UnknownNode(xmlSchemaAnnotation, "http://www.w3.org/2001/XMLSchema:documentation, http://www.w3.org/2001/XMLSchema:appinfo");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaAnnotation, "http://www.w3.org/2001/XMLSchema:documentation, http://www.w3.org/2001/XMLSchema:appinfo");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaAnnotation.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaAnnotation;
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x00030E0C File Offset: 0x0002FE0C
		private XmlSchemaAppInfo Read10_XmlSchemaAppInfo(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id117_XmlSchemaAppInfo || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaAppInfo xmlSchemaAppInfo = new XmlSchemaAppInfo();
			XmlNode[] array = null;
			int num = 0;
			bool[] array2 = new bool[3];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id118_source && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaAppInfo.Source = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaAppInfo.Namespaces == null)
					{
						xmlSchemaAppInfo.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaAppInfo.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					base.UnknownNode(xmlSchemaAppInfo, ":source");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaAppInfo.Markup = (XmlNode[])base.ShrinkArray(array, num, typeof(XmlNode), true);
				return xmlSchemaAppInfo;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					array = (XmlNode[])base.EnsureArrayIndex(array, num, typeof(XmlNode));
					array[num++] = base.ReadXmlNode(false);
				}
				else if (base.Reader.NodeType == XmlNodeType.Text || base.Reader.NodeType == XmlNodeType.CDATA || base.Reader.NodeType == XmlNodeType.Whitespace || base.Reader.NodeType == XmlNodeType.SignificantWhitespace)
				{
					array = (XmlNode[])base.EnsureArrayIndex(array, num, typeof(XmlNode));
					array[num++] = base.Document.CreateTextNode(base.Reader.ReadString());
				}
				else
				{
					base.UnknownNode(xmlSchemaAppInfo, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaAppInfo.Markup = (XmlNode[])base.ShrinkArray(array, num, typeof(XmlNode), true);
			base.ReadEndElement();
			return xmlSchemaAppInfo;
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x000310D4 File Offset: 0x000300D4
		private XmlSchemaDocumentation Read9_XmlSchemaDocumentation(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id119_XmlSchemaDocumentation || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaDocumentation xmlSchemaDocumentation = new XmlSchemaDocumentation();
			XmlNode[] array = null;
			int num = 0;
			bool[] array2 = new bool[4];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id118_source && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaDocumentation.Source = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[2] && base.Reader.LocalName == this.id120_lang && base.Reader.NamespaceURI == this.id121_Item)
				{
					xmlSchemaDocumentation.Language = base.Reader.Value;
					array2[2] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaDocumentation.Namespaces == null)
					{
						xmlSchemaDocumentation.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaDocumentation.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					base.UnknownNode(xmlSchemaDocumentation, ":source, http://www.w3.org/XML/1998/namespace");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaDocumentation.Markup = (XmlNode[])base.ShrinkArray(array, num, typeof(XmlNode), true);
				return xmlSchemaDocumentation;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					array = (XmlNode[])base.EnsureArrayIndex(array, num, typeof(XmlNode));
					array[num++] = base.ReadXmlNode(false);
				}
				else if (base.Reader.NodeType == XmlNodeType.Text || base.Reader.NodeType == XmlNodeType.CDATA || base.Reader.NodeType == XmlNodeType.Whitespace || base.Reader.NodeType == XmlNodeType.SignificantWhitespace)
				{
					array = (XmlNode[])base.EnsureArrayIndex(array, num, typeof(XmlNode));
					array[num++] = base.Document.CreateTextNode(base.Reader.ReadString());
				}
				else
				{
					base.UnknownNode(xmlSchemaDocumentation, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaDocumentation.Markup = (XmlNode[])base.ShrinkArray(array, num, typeof(XmlNode), true);
			base.ReadEndElement();
			return xmlSchemaDocumentation;
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x000313E4 File Offset: 0x000303E4
		private XmlSchemaContentProcessing Read38_XmlSchemaContentProcessing(string s)
		{
			if (s != null)
			{
				if (s == "skip")
				{
					return XmlSchemaContentProcessing.Skip;
				}
				if (s == "lax")
				{
					return XmlSchemaContentProcessing.Lax;
				}
				if (s == "strict")
				{
					return XmlSchemaContentProcessing.Strict;
				}
			}
			throw base.CreateUnknownConstantException(s, typeof(XmlSchemaContentProcessing));
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x00031438 File Offset: 0x00030438
		private XmlSchemaAttributeGroupRef Read37_XmlSchemaAttributeGroupRef(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id122_XmlSchemaAttributeGroupRef || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaAttributeGroupRef xmlSchemaAttributeGroupRef = new XmlSchemaAttributeGroupRef();
			XmlAttribute[] array = null;
			int num = 0;
			bool[] array2 = new bool[5];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaAttributeGroupRef.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id123_ref && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaAttributeGroupRef.RefName = base.ToXmlQualifiedName(base.Reader.Value);
					array2[4] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaAttributeGroupRef.Namespaces == null)
					{
						xmlSchemaAttributeGroupRef.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaAttributeGroupRef.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaAttributeGroupRef.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaAttributeGroupRef.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaAttributeGroupRef;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaAttributeGroupRef.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaAttributeGroupRef, "http://www.w3.org/2001/XMLSchema:annotation");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaAttributeGroupRef, "http://www.w3.org/2001/XMLSchema:annotation");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaAttributeGroupRef.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaAttributeGroupRef;
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x00031750 File Offset: 0x00030750
		private XmlSchemaAttribute Read36_XmlSchemaAttribute(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id124_XmlSchemaAttribute || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaAttribute xmlSchemaAttribute = new XmlSchemaAttribute();
			XmlAttribute[] array = null;
			int num = 0;
			bool[] array2 = new bool[12];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaAttribute.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id125_default && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaAttribute.DefaultValue = base.Reader.Value;
					array2[4] = true;
				}
				else if (!array2[5] && base.Reader.LocalName == this.id126_fixed && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaAttribute.FixedValue = base.Reader.Value;
					array2[5] = true;
				}
				else if (!array2[6] && base.Reader.LocalName == this.id127_form && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaAttribute.Form = this.Read6_XmlSchemaForm(base.Reader.Value);
					array2[6] = true;
				}
				else if (!array2[7] && base.Reader.LocalName == this.id4_name && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaAttribute.Name = base.Reader.Value;
					array2[7] = true;
				}
				else if (!array2[8] && base.Reader.LocalName == this.id123_ref && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaAttribute.RefName = base.ToXmlQualifiedName(base.Reader.Value);
					array2[8] = true;
				}
				else if (!array2[9] && base.Reader.LocalName == this.id27_type && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaAttribute.SchemaTypeName = base.ToXmlQualifiedName(base.Reader.Value);
					array2[9] = true;
				}
				else if (!array2[11] && base.Reader.LocalName == this.id35_use && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaAttribute.Use = this.Read35_XmlSchemaUse(base.Reader.Value);
					array2[11] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaAttribute.Namespaces == null)
					{
						xmlSchemaAttribute.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaAttribute.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaAttribute.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaAttribute.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaAttribute;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaAttribute.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else if (!array2[10] && base.Reader.LocalName == this.id105_simpleType && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaAttribute.SchemaType = this.Read34_XmlSchemaSimpleType(false, true);
						array2[10] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaAttribute, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:simpleType");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaAttribute, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:simpleType");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaAttribute.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaAttribute;
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x00031C74 File Offset: 0x00030C74
		private XmlSchemaSimpleType Read34_XmlSchemaSimpleType(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id128_XmlSchemaSimpleType || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaSimpleType xmlSchemaSimpleType = new XmlSchemaSimpleType();
			XmlAttribute[] array = null;
			int num = 0;
			bool[] array2 = new bool[7];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaSimpleType.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id4_name && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaSimpleType.Name = base.Reader.Value;
					array2[4] = true;
				}
				else if (!array2[5] && base.Reader.LocalName == this.id129_final && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaSimpleType.Final = this.Read7_XmlSchemaDerivationMethod(base.Reader.Value);
					array2[5] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaSimpleType.Namespaces == null)
					{
						xmlSchemaSimpleType.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaSimpleType.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaSimpleType.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaSimpleType.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaSimpleType;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaSimpleType.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else if (!array2[6] && base.Reader.LocalName == this.id130_list && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaSimpleType.Content = this.Read17_XmlSchemaSimpleTypeList(false, true);
						array2[6] = true;
					}
					else if (!array2[6] && base.Reader.LocalName == this.id131_restriction && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaSimpleType.Content = this.Read32_XmlSchemaSimpleTypeRestriction(false, true);
						array2[6] = true;
					}
					else if (!array2[6] && base.Reader.LocalName == this.id132_union && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaSimpleType.Content = this.Read33_XmlSchemaSimpleTypeUnion(false, true);
						array2[6] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaSimpleType, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:list, http://www.w3.org/2001/XMLSchema:restriction, http://www.w3.org/2001/XMLSchema:union");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaSimpleType, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:list, http://www.w3.org/2001/XMLSchema:restriction, http://www.w3.org/2001/XMLSchema:union");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaSimpleType.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaSimpleType;
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x000320A4 File Offset: 0x000310A4
		private XmlSchemaSimpleTypeUnion Read33_XmlSchemaSimpleTypeUnion(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id133_XmlSchemaSimpleTypeUnion || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaSimpleTypeUnion xmlSchemaSimpleTypeUnion = new XmlSchemaSimpleTypeUnion();
			XmlAttribute[] array = null;
			int num = 0;
			XmlSchemaObjectCollection baseTypes = xmlSchemaSimpleTypeUnion.BaseTypes;
			XmlQualifiedName[] array2 = null;
			int num2 = 0;
			bool[] array3 = new bool[6];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array3[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaSimpleTypeUnion.Id = base.CollapseWhitespace(base.Reader.Value);
					array3[1] = true;
				}
				else if (base.Reader.LocalName == this.id134_memberTypes && base.Reader.NamespaceURI == this.id5_Item)
				{
					string value = base.Reader.Value;
					string[] array4 = value.Split(null);
					for (int i = 0; i < array4.Length; i++)
					{
						array2 = (XmlQualifiedName[])base.EnsureArrayIndex(array2, num2, typeof(XmlQualifiedName));
						array2[num2++] = base.ToXmlQualifiedName(array4[i]);
					}
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaSimpleTypeUnion.Namespaces == null)
					{
						xmlSchemaSimpleTypeUnion.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaSimpleTypeUnion.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaSimpleTypeUnion.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			xmlSchemaSimpleTypeUnion.MemberTypes = (XmlQualifiedName[])base.ShrinkArray(array2, num2, typeof(XmlQualifiedName), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaSimpleTypeUnion.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				xmlSchemaSimpleTypeUnion.MemberTypes = (XmlQualifiedName[])base.ShrinkArray(array2, num2, typeof(XmlQualifiedName), true);
				return xmlSchemaSimpleTypeUnion;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num3 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array3[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaSimpleTypeUnion.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array3[2] = true;
					}
					else if (base.Reader.LocalName == this.id105_simpleType && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (baseTypes == null)
						{
							base.Reader.Skip();
						}
						else
						{
							baseTypes.Add(this.Read34_XmlSchemaSimpleType(false, true));
						}
					}
					else
					{
						base.UnknownNode(xmlSchemaSimpleTypeUnion, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:simpleType");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaSimpleTypeUnion, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:simpleType");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num3, ref readerCount);
			}
			xmlSchemaSimpleTypeUnion.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			xmlSchemaSimpleTypeUnion.MemberTypes = (XmlQualifiedName[])base.ShrinkArray(array2, num2, typeof(XmlQualifiedName), true);
			base.ReadEndElement();
			return xmlSchemaSimpleTypeUnion;
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x000324B0 File Offset: 0x000314B0
		private XmlSchemaSimpleTypeRestriction Read32_XmlSchemaSimpleTypeRestriction(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id135_XmlSchemaSimpleTypeRestriction || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaSimpleTypeRestriction xmlSchemaSimpleTypeRestriction = new XmlSchemaSimpleTypeRestriction();
			XmlAttribute[] array = null;
			int num = 0;
			XmlSchemaObjectCollection facets = xmlSchemaSimpleTypeRestriction.Facets;
			bool[] array2 = new bool[7];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaSimpleTypeRestriction.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id136_base && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaSimpleTypeRestriction.BaseTypeName = base.ToXmlQualifiedName(base.Reader.Value);
					array2[4] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaSimpleTypeRestriction.Namespaces == null)
					{
						xmlSchemaSimpleTypeRestriction.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaSimpleTypeRestriction.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaSimpleTypeRestriction.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaSimpleTypeRestriction.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaSimpleTypeRestriction;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaSimpleTypeRestriction.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else if (!array2[5] && base.Reader.LocalName == this.id105_simpleType && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaSimpleTypeRestriction.BaseType = this.Read34_XmlSchemaSimpleType(false, true);
						array2[5] = true;
					}
					else if (base.Reader.LocalName == this.id137_fractionDigits && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (facets == null)
						{
							base.Reader.Skip();
						}
						else
						{
							facets.Add(this.Read20_XmlSchemaFractionDigitsFacet(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id138_minInclusive && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (facets == null)
						{
							base.Reader.Skip();
						}
						else
						{
							facets.Add(this.Read21_XmlSchemaMinInclusiveFacet(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id139_maxLength && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (facets == null)
						{
							base.Reader.Skip();
						}
						else
						{
							facets.Add(this.Read22_XmlSchemaMaxLengthFacet(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id140_length && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (facets == null)
						{
							base.Reader.Skip();
						}
						else
						{
							facets.Add(this.Read23_XmlSchemaLengthFacet(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id141_totalDigits && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (facets == null)
						{
							base.Reader.Skip();
						}
						else
						{
							facets.Add(this.Read24_XmlSchemaTotalDigitsFacet(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id62_pattern && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (facets == null)
						{
							base.Reader.Skip();
						}
						else
						{
							facets.Add(this.Read25_XmlSchemaPatternFacet(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id142_enumeration && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (facets == null)
						{
							base.Reader.Skip();
						}
						else
						{
							facets.Add(this.Read26_XmlSchemaEnumerationFacet(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id143_maxInclusive && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (facets == null)
						{
							base.Reader.Skip();
						}
						else
						{
							facets.Add(this.Read27_XmlSchemaMaxInclusiveFacet(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id144_maxExclusive && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (facets == null)
						{
							base.Reader.Skip();
						}
						else
						{
							facets.Add(this.Read28_XmlSchemaMaxExclusiveFacet(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id145_whiteSpace && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (facets == null)
						{
							base.Reader.Skip();
						}
						else
						{
							facets.Add(this.Read29_XmlSchemaWhiteSpaceFacet(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id146_minExclusive && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (facets == null)
						{
							base.Reader.Skip();
						}
						else
						{
							facets.Add(this.Read30_XmlSchemaMinExclusiveFacet(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id147_minLength && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (facets == null)
						{
							base.Reader.Skip();
						}
						else
						{
							facets.Add(this.Read31_XmlSchemaMinLengthFacet(false, true));
						}
					}
					else
					{
						base.UnknownNode(xmlSchemaSimpleTypeRestriction, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:simpleType, http://www.w3.org/2001/XMLSchema:fractionDigits, http://www.w3.org/2001/XMLSchema:minInclusive, http://www.w3.org/2001/XMLSchema:maxLength, http://www.w3.org/2001/XMLSchema:length, http://www.w3.org/2001/XMLSchema:totalDigits, http://www.w3.org/2001/XMLSchema:pattern, http://www.w3.org/2001/XMLSchema:enumeration, http://www.w3.org/2001/XMLSchema:maxInclusive, http://www.w3.org/2001/XMLSchema:maxExclusive, http://www.w3.org/2001/XMLSchema:whiteSpace, http://www.w3.org/2001/XMLSchema:minExclusive, http://www.w3.org/2001/XMLSchema:minLength");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaSimpleTypeRestriction, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:simpleType, http://www.w3.org/2001/XMLSchema:fractionDigits, http://www.w3.org/2001/XMLSchema:minInclusive, http://www.w3.org/2001/XMLSchema:maxLength, http://www.w3.org/2001/XMLSchema:length, http://www.w3.org/2001/XMLSchema:totalDigits, http://www.w3.org/2001/XMLSchema:pattern, http://www.w3.org/2001/XMLSchema:enumeration, http://www.w3.org/2001/XMLSchema:maxInclusive, http://www.w3.org/2001/XMLSchema:maxExclusive, http://www.w3.org/2001/XMLSchema:whiteSpace, http://www.w3.org/2001/XMLSchema:minExclusive, http://www.w3.org/2001/XMLSchema:minLength");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaSimpleTypeRestriction.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaSimpleTypeRestriction;
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x00032BC4 File Offset: 0x00031BC4
		private XmlSchemaMinLengthFacet Read31_XmlSchemaMinLengthFacet(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id148_XmlSchemaMinLengthFacet || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaMinLengthFacet xmlSchemaMinLengthFacet = new XmlSchemaMinLengthFacet();
			XmlAttribute[] array = null;
			int num = 0;
			bool[] array2 = new bool[6];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaMinLengthFacet.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id149_value && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaMinLengthFacet.Value = base.Reader.Value;
					array2[4] = true;
				}
				else if (!array2[5] && base.Reader.LocalName == this.id126_fixed && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaMinLengthFacet.IsFixed = XmlConvert.ToBoolean(base.Reader.Value);
					array2[5] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaMinLengthFacet.Namespaces == null)
					{
						xmlSchemaMinLengthFacet.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaMinLengthFacet.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaMinLengthFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaMinLengthFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaMinLengthFacet;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaMinLengthFacet.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaMinLengthFacet, "http://www.w3.org/2001/XMLSchema:annotation");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaMinLengthFacet, "http://www.w3.org/2001/XMLSchema:annotation");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaMinLengthFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaMinLengthFacet;
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x00032F24 File Offset: 0x00031F24
		private XmlSchemaMinExclusiveFacet Read30_XmlSchemaMinExclusiveFacet(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id150_XmlSchemaMinExclusiveFacet || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaMinExclusiveFacet xmlSchemaMinExclusiveFacet = new XmlSchemaMinExclusiveFacet();
			XmlAttribute[] array = null;
			int num = 0;
			bool[] array2 = new bool[6];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaMinExclusiveFacet.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id149_value && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaMinExclusiveFacet.Value = base.Reader.Value;
					array2[4] = true;
				}
				else if (!array2[5] && base.Reader.LocalName == this.id126_fixed && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaMinExclusiveFacet.IsFixed = XmlConvert.ToBoolean(base.Reader.Value);
					array2[5] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaMinExclusiveFacet.Namespaces == null)
					{
						xmlSchemaMinExclusiveFacet.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaMinExclusiveFacet.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaMinExclusiveFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaMinExclusiveFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaMinExclusiveFacet;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaMinExclusiveFacet.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaMinExclusiveFacet, "http://www.w3.org/2001/XMLSchema:annotation");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaMinExclusiveFacet, "http://www.w3.org/2001/XMLSchema:annotation");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaMinExclusiveFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaMinExclusiveFacet;
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x00033284 File Offset: 0x00032284
		private XmlSchemaWhiteSpaceFacet Read29_XmlSchemaWhiteSpaceFacet(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id151_XmlSchemaWhiteSpaceFacet || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaWhiteSpaceFacet xmlSchemaWhiteSpaceFacet = new XmlSchemaWhiteSpaceFacet();
			XmlAttribute[] array = null;
			int num = 0;
			bool[] array2 = new bool[6];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaWhiteSpaceFacet.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id149_value && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaWhiteSpaceFacet.Value = base.Reader.Value;
					array2[4] = true;
				}
				else if (!array2[5] && base.Reader.LocalName == this.id126_fixed && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaWhiteSpaceFacet.IsFixed = XmlConvert.ToBoolean(base.Reader.Value);
					array2[5] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaWhiteSpaceFacet.Namespaces == null)
					{
						xmlSchemaWhiteSpaceFacet.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaWhiteSpaceFacet.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaWhiteSpaceFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaWhiteSpaceFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaWhiteSpaceFacet;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaWhiteSpaceFacet.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaWhiteSpaceFacet, "http://www.w3.org/2001/XMLSchema:annotation");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaWhiteSpaceFacet, "http://www.w3.org/2001/XMLSchema:annotation");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaWhiteSpaceFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaWhiteSpaceFacet;
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x000335E4 File Offset: 0x000325E4
		private XmlSchemaMaxExclusiveFacet Read28_XmlSchemaMaxExclusiveFacet(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id152_XmlSchemaMaxExclusiveFacet || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaMaxExclusiveFacet xmlSchemaMaxExclusiveFacet = new XmlSchemaMaxExclusiveFacet();
			XmlAttribute[] array = null;
			int num = 0;
			bool[] array2 = new bool[6];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaMaxExclusiveFacet.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id149_value && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaMaxExclusiveFacet.Value = base.Reader.Value;
					array2[4] = true;
				}
				else if (!array2[5] && base.Reader.LocalName == this.id126_fixed && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaMaxExclusiveFacet.IsFixed = XmlConvert.ToBoolean(base.Reader.Value);
					array2[5] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaMaxExclusiveFacet.Namespaces == null)
					{
						xmlSchemaMaxExclusiveFacet.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaMaxExclusiveFacet.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaMaxExclusiveFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaMaxExclusiveFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaMaxExclusiveFacet;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaMaxExclusiveFacet.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaMaxExclusiveFacet, "http://www.w3.org/2001/XMLSchema:annotation");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaMaxExclusiveFacet, "http://www.w3.org/2001/XMLSchema:annotation");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaMaxExclusiveFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaMaxExclusiveFacet;
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x00033944 File Offset: 0x00032944
		private XmlSchemaMaxInclusiveFacet Read27_XmlSchemaMaxInclusiveFacet(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id153_XmlSchemaMaxInclusiveFacet || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaMaxInclusiveFacet xmlSchemaMaxInclusiveFacet = new XmlSchemaMaxInclusiveFacet();
			XmlAttribute[] array = null;
			int num = 0;
			bool[] array2 = new bool[6];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaMaxInclusiveFacet.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id149_value && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaMaxInclusiveFacet.Value = base.Reader.Value;
					array2[4] = true;
				}
				else if (!array2[5] && base.Reader.LocalName == this.id126_fixed && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaMaxInclusiveFacet.IsFixed = XmlConvert.ToBoolean(base.Reader.Value);
					array2[5] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaMaxInclusiveFacet.Namespaces == null)
					{
						xmlSchemaMaxInclusiveFacet.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaMaxInclusiveFacet.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaMaxInclusiveFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaMaxInclusiveFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaMaxInclusiveFacet;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaMaxInclusiveFacet.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaMaxInclusiveFacet, "http://www.w3.org/2001/XMLSchema:annotation");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaMaxInclusiveFacet, "http://www.w3.org/2001/XMLSchema:annotation");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaMaxInclusiveFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaMaxInclusiveFacet;
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x00033CA4 File Offset: 0x00032CA4
		private XmlSchemaEnumerationFacet Read26_XmlSchemaEnumerationFacet(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id154_XmlSchemaEnumerationFacet || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaEnumerationFacet xmlSchemaEnumerationFacet = new XmlSchemaEnumerationFacet();
			XmlAttribute[] array = null;
			int num = 0;
			bool[] array2 = new bool[6];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaEnumerationFacet.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id149_value && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaEnumerationFacet.Value = base.Reader.Value;
					array2[4] = true;
				}
				else if (!array2[5] && base.Reader.LocalName == this.id126_fixed && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaEnumerationFacet.IsFixed = XmlConvert.ToBoolean(base.Reader.Value);
					array2[5] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaEnumerationFacet.Namespaces == null)
					{
						xmlSchemaEnumerationFacet.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaEnumerationFacet.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaEnumerationFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaEnumerationFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaEnumerationFacet;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaEnumerationFacet.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaEnumerationFacet, "http://www.w3.org/2001/XMLSchema:annotation");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaEnumerationFacet, "http://www.w3.org/2001/XMLSchema:annotation");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaEnumerationFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaEnumerationFacet;
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x00034004 File Offset: 0x00033004
		private XmlSchemaPatternFacet Read25_XmlSchemaPatternFacet(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id155_XmlSchemaPatternFacet || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaPatternFacet xmlSchemaPatternFacet = new XmlSchemaPatternFacet();
			XmlAttribute[] array = null;
			int num = 0;
			bool[] array2 = new bool[6];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaPatternFacet.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id149_value && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaPatternFacet.Value = base.Reader.Value;
					array2[4] = true;
				}
				else if (!array2[5] && base.Reader.LocalName == this.id126_fixed && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaPatternFacet.IsFixed = XmlConvert.ToBoolean(base.Reader.Value);
					array2[5] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaPatternFacet.Namespaces == null)
					{
						xmlSchemaPatternFacet.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaPatternFacet.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaPatternFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaPatternFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaPatternFacet;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaPatternFacet.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaPatternFacet, "http://www.w3.org/2001/XMLSchema:annotation");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaPatternFacet, "http://www.w3.org/2001/XMLSchema:annotation");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaPatternFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaPatternFacet;
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x00034364 File Offset: 0x00033364
		private XmlSchemaTotalDigitsFacet Read24_XmlSchemaTotalDigitsFacet(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id156_XmlSchemaTotalDigitsFacet || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaTotalDigitsFacet xmlSchemaTotalDigitsFacet = new XmlSchemaTotalDigitsFacet();
			XmlAttribute[] array = null;
			int num = 0;
			bool[] array2 = new bool[6];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaTotalDigitsFacet.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id149_value && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaTotalDigitsFacet.Value = base.Reader.Value;
					array2[4] = true;
				}
				else if (!array2[5] && base.Reader.LocalName == this.id126_fixed && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaTotalDigitsFacet.IsFixed = XmlConvert.ToBoolean(base.Reader.Value);
					array2[5] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaTotalDigitsFacet.Namespaces == null)
					{
						xmlSchemaTotalDigitsFacet.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaTotalDigitsFacet.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaTotalDigitsFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaTotalDigitsFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaTotalDigitsFacet;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaTotalDigitsFacet.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaTotalDigitsFacet, "http://www.w3.org/2001/XMLSchema:annotation");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaTotalDigitsFacet, "http://www.w3.org/2001/XMLSchema:annotation");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaTotalDigitsFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaTotalDigitsFacet;
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x000346C4 File Offset: 0x000336C4
		private XmlSchemaLengthFacet Read23_XmlSchemaLengthFacet(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id157_XmlSchemaLengthFacet || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaLengthFacet xmlSchemaLengthFacet = new XmlSchemaLengthFacet();
			XmlAttribute[] array = null;
			int num = 0;
			bool[] array2 = new bool[6];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaLengthFacet.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id149_value && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaLengthFacet.Value = base.Reader.Value;
					array2[4] = true;
				}
				else if (!array2[5] && base.Reader.LocalName == this.id126_fixed && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaLengthFacet.IsFixed = XmlConvert.ToBoolean(base.Reader.Value);
					array2[5] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaLengthFacet.Namespaces == null)
					{
						xmlSchemaLengthFacet.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaLengthFacet.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaLengthFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaLengthFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaLengthFacet;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaLengthFacet.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaLengthFacet, "http://www.w3.org/2001/XMLSchema:annotation");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaLengthFacet, "http://www.w3.org/2001/XMLSchema:annotation");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaLengthFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaLengthFacet;
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00034A24 File Offset: 0x00033A24
		private XmlSchemaMaxLengthFacet Read22_XmlSchemaMaxLengthFacet(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id158_XmlSchemaMaxLengthFacet || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaMaxLengthFacet xmlSchemaMaxLengthFacet = new XmlSchemaMaxLengthFacet();
			XmlAttribute[] array = null;
			int num = 0;
			bool[] array2 = new bool[6];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaMaxLengthFacet.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id149_value && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaMaxLengthFacet.Value = base.Reader.Value;
					array2[4] = true;
				}
				else if (!array2[5] && base.Reader.LocalName == this.id126_fixed && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaMaxLengthFacet.IsFixed = XmlConvert.ToBoolean(base.Reader.Value);
					array2[5] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaMaxLengthFacet.Namespaces == null)
					{
						xmlSchemaMaxLengthFacet.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaMaxLengthFacet.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaMaxLengthFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaMaxLengthFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaMaxLengthFacet;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaMaxLengthFacet.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaMaxLengthFacet, "http://www.w3.org/2001/XMLSchema:annotation");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaMaxLengthFacet, "http://www.w3.org/2001/XMLSchema:annotation");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaMaxLengthFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaMaxLengthFacet;
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00034D84 File Offset: 0x00033D84
		private XmlSchemaMinInclusiveFacet Read21_XmlSchemaMinInclusiveFacet(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id159_XmlSchemaMinInclusiveFacet || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaMinInclusiveFacet xmlSchemaMinInclusiveFacet = new XmlSchemaMinInclusiveFacet();
			XmlAttribute[] array = null;
			int num = 0;
			bool[] array2 = new bool[6];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaMinInclusiveFacet.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id149_value && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaMinInclusiveFacet.Value = base.Reader.Value;
					array2[4] = true;
				}
				else if (!array2[5] && base.Reader.LocalName == this.id126_fixed && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaMinInclusiveFacet.IsFixed = XmlConvert.ToBoolean(base.Reader.Value);
					array2[5] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaMinInclusiveFacet.Namespaces == null)
					{
						xmlSchemaMinInclusiveFacet.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaMinInclusiveFacet.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaMinInclusiveFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaMinInclusiveFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaMinInclusiveFacet;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaMinInclusiveFacet.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaMinInclusiveFacet, "http://www.w3.org/2001/XMLSchema:annotation");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaMinInclusiveFacet, "http://www.w3.org/2001/XMLSchema:annotation");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaMinInclusiveFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaMinInclusiveFacet;
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x000350E4 File Offset: 0x000340E4
		private XmlSchemaFractionDigitsFacet Read20_XmlSchemaFractionDigitsFacet(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id160_XmlSchemaFractionDigitsFacet || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaFractionDigitsFacet xmlSchemaFractionDigitsFacet = new XmlSchemaFractionDigitsFacet();
			XmlAttribute[] array = null;
			int num = 0;
			bool[] array2 = new bool[6];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaFractionDigitsFacet.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id149_value && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaFractionDigitsFacet.Value = base.Reader.Value;
					array2[4] = true;
				}
				else if (!array2[5] && base.Reader.LocalName == this.id126_fixed && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaFractionDigitsFacet.IsFixed = XmlConvert.ToBoolean(base.Reader.Value);
					array2[5] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaFractionDigitsFacet.Namespaces == null)
					{
						xmlSchemaFractionDigitsFacet.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaFractionDigitsFacet.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaFractionDigitsFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaFractionDigitsFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaFractionDigitsFacet;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaFractionDigitsFacet.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaFractionDigitsFacet, "http://www.w3.org/2001/XMLSchema:annotation");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaFractionDigitsFacet, "http://www.w3.org/2001/XMLSchema:annotation");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaFractionDigitsFacet.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaFractionDigitsFacet;
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00035444 File Offset: 0x00034444
		private XmlSchemaSimpleTypeList Read17_XmlSchemaSimpleTypeList(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id161_XmlSchemaSimpleTypeList || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaSimpleTypeList xmlSchemaSimpleTypeList = new XmlSchemaSimpleTypeList();
			XmlAttribute[] array = null;
			int num = 0;
			bool[] array2 = new bool[6];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaSimpleTypeList.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id162_itemType && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaSimpleTypeList.ItemTypeName = base.ToXmlQualifiedName(base.Reader.Value);
					array2[4] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaSimpleTypeList.Namespaces == null)
					{
						xmlSchemaSimpleTypeList.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaSimpleTypeList.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaSimpleTypeList.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaSimpleTypeList.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaSimpleTypeList;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaSimpleTypeList.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else if (!array2[5] && base.Reader.LocalName == this.id105_simpleType && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaSimpleTypeList.ItemType = this.Read34_XmlSchemaSimpleType(false, true);
						array2[5] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaSimpleTypeList, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:simpleType");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaSimpleTypeList, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:simpleType");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaSimpleTypeList.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaSimpleTypeList;
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000809 RID: 2057 RVA: 0x000357A4 File Offset: 0x000347A4
		internal Hashtable XmlSchemaDerivationMethodValues
		{
			get
			{
				if (this._XmlSchemaDerivationMethodValues == null)
				{
					this._XmlSchemaDerivationMethodValues = new Hashtable
					{
						{ "", 0L },
						{ "substitution", 1L },
						{ "extension", 2L },
						{ "restriction", 4L },
						{ "list", 8L },
						{ "union", 16L },
						{ "#all", 255L }
					};
				}
				return this._XmlSchemaDerivationMethodValues;
			}
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x00035852 File Offset: 0x00034852
		private XmlSchemaDerivationMethod Read7_XmlSchemaDerivationMethod(string s)
		{
			return (XmlSchemaDerivationMethod)XmlSerializationReader.ToEnum(s, this.XmlSchemaDerivationMethodValues, "global::System.Xml.Schema.XmlSchemaDerivationMethod");
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x00035868 File Offset: 0x00034868
		private XmlSchemaUse Read35_XmlSchemaUse(string s)
		{
			if (s != null)
			{
				if (s == "optional")
				{
					return XmlSchemaUse.Optional;
				}
				if (s == "prohibited")
				{
					return XmlSchemaUse.Prohibited;
				}
				if (s == "required")
				{
					return XmlSchemaUse.Required;
				}
			}
			throw base.CreateUnknownConstantException(s, typeof(XmlSchemaUse));
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x000358BC File Offset: 0x000348BC
		private XmlSchemaForm Read6_XmlSchemaForm(string s)
		{
			if (s != null)
			{
				if (s == "qualified")
				{
					return XmlSchemaForm.Qualified;
				}
				if (s == "unqualified")
				{
					return XmlSchemaForm.Unqualified;
				}
			}
			throw base.CreateUnknownConstantException(s, typeof(XmlSchemaForm));
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x00035900 File Offset: 0x00034900
		private XmlSchemaElement Read52_XmlSchemaElement(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id163_XmlSchemaElement || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaElement xmlSchemaElement = new XmlSchemaElement();
			XmlAttribute[] array = null;
			int num = 0;
			XmlSchemaObjectCollection constraints = xmlSchemaElement.Constraints;
			bool[] array2 = new bool[19];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaElement.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id164_minOccurs && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaElement.MinOccursString = base.Reader.Value;
					array2[4] = true;
				}
				else if (!array2[5] && base.Reader.LocalName == this.id165_maxOccurs && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaElement.MaxOccursString = base.Reader.Value;
					array2[5] = true;
				}
				else if (!array2[6] && base.Reader.LocalName == this.id166_abstract && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaElement.IsAbstract = XmlConvert.ToBoolean(base.Reader.Value);
					array2[6] = true;
				}
				else if (!array2[7] && base.Reader.LocalName == this.id167_block && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaElement.Block = this.Read7_XmlSchemaDerivationMethod(base.Reader.Value);
					array2[7] = true;
				}
				else if (!array2[8] && base.Reader.LocalName == this.id125_default && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaElement.DefaultValue = base.Reader.Value;
					array2[8] = true;
				}
				else if (!array2[9] && base.Reader.LocalName == this.id129_final && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaElement.Final = this.Read7_XmlSchemaDerivationMethod(base.Reader.Value);
					array2[9] = true;
				}
				else if (!array2[10] && base.Reader.LocalName == this.id126_fixed && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaElement.FixedValue = base.Reader.Value;
					array2[10] = true;
				}
				else if (!array2[11] && base.Reader.LocalName == this.id127_form && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaElement.Form = this.Read6_XmlSchemaForm(base.Reader.Value);
					array2[11] = true;
				}
				else if (!array2[12] && base.Reader.LocalName == this.id4_name && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaElement.Name = base.Reader.Value;
					array2[12] = true;
				}
				else if (!array2[13] && base.Reader.LocalName == this.id168_nillable && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaElement.IsNillable = XmlConvert.ToBoolean(base.Reader.Value);
					array2[13] = true;
				}
				else if (!array2[14] && base.Reader.LocalName == this.id123_ref && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaElement.RefName = base.ToXmlQualifiedName(base.Reader.Value);
					array2[14] = true;
				}
				else if (!array2[15] && base.Reader.LocalName == this.id169_substitutionGroup && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaElement.SubstitutionGroup = base.ToXmlQualifiedName(base.Reader.Value);
					array2[15] = true;
				}
				else if (!array2[16] && base.Reader.LocalName == this.id27_type && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaElement.SchemaTypeName = base.ToXmlQualifiedName(base.Reader.Value);
					array2[16] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaElement.Namespaces == null)
					{
						xmlSchemaElement.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaElement.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaElement.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaElement.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaElement;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaElement.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else if (!array2[17] && base.Reader.LocalName == this.id105_simpleType && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaElement.SchemaType = this.Read34_XmlSchemaSimpleType(false, true);
						array2[17] = true;
					}
					else if (!array2[17] && base.Reader.LocalName == this.id106_complexType && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaElement.SchemaType = this.Read62_XmlSchemaComplexType(false, true);
						array2[17] = true;
					}
					else if (base.Reader.LocalName == this.id170_key && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (constraints == null)
						{
							base.Reader.Skip();
						}
						else
						{
							constraints.Add(this.Read49_XmlSchemaKey(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id171_unique && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (constraints == null)
						{
							base.Reader.Skip();
						}
						else
						{
							constraints.Add(this.Read50_XmlSchemaUnique(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id172_keyref && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (constraints == null)
						{
							base.Reader.Skip();
						}
						else
						{
							constraints.Add(this.Read51_XmlSchemaKeyref(false, true));
						}
					}
					else
					{
						base.UnknownNode(xmlSchemaElement, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:simpleType, http://www.w3.org/2001/XMLSchema:complexType, http://www.w3.org/2001/XMLSchema:key, http://www.w3.org/2001/XMLSchema:unique, http://www.w3.org/2001/XMLSchema:keyref");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaElement, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:simpleType, http://www.w3.org/2001/XMLSchema:complexType, http://www.w3.org/2001/XMLSchema:key, http://www.w3.org/2001/XMLSchema:unique, http://www.w3.org/2001/XMLSchema:keyref");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaElement.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaElement;
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x00036124 File Offset: 0x00035124
		private XmlSchemaKeyref Read51_XmlSchemaKeyref(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id173_XmlSchemaKeyref || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaKeyref xmlSchemaKeyref = new XmlSchemaKeyref();
			XmlAttribute[] array = null;
			int num = 0;
			XmlSchemaObjectCollection fields = xmlSchemaKeyref.Fields;
			bool[] array2 = new bool[8];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaKeyref.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id4_name && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaKeyref.Name = base.Reader.Value;
					array2[4] = true;
				}
				else if (!array2[7] && base.Reader.LocalName == this.id174_refer && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaKeyref.Refer = base.ToXmlQualifiedName(base.Reader.Value);
					array2[7] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaKeyref.Namespaces == null)
					{
						xmlSchemaKeyref.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaKeyref.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaKeyref.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaKeyref.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaKeyref;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaKeyref.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else if (!array2[5] && base.Reader.LocalName == this.id175_selector && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaKeyref.Selector = this.Read47_XmlSchemaXPath(false, true);
						array2[5] = true;
					}
					else if (base.Reader.LocalName == this.id176_field && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (fields == null)
						{
							base.Reader.Skip();
						}
						else
						{
							fields.Add(this.Read47_XmlSchemaXPath(false, true));
						}
					}
					else
					{
						base.UnknownNode(xmlSchemaKeyref, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:selector, http://www.w3.org/2001/XMLSchema:field");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaKeyref, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:selector, http://www.w3.org/2001/XMLSchema:field");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaKeyref.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaKeyref;
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x00036520 File Offset: 0x00035520
		private XmlSchemaXPath Read47_XmlSchemaXPath(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id177_XmlSchemaXPath || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaXPath xmlSchemaXPath = new XmlSchemaXPath();
			XmlAttribute[] array = null;
			int num = 0;
			bool[] array2 = new bool[5];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaXPath.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id178_xpath && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaXPath.XPath = base.Reader.Value;
					array2[4] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaXPath.Namespaces == null)
					{
						xmlSchemaXPath.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaXPath.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaXPath.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaXPath.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaXPath;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaXPath.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaXPath, "http://www.w3.org/2001/XMLSchema:annotation");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaXPath, "http://www.w3.org/2001/XMLSchema:annotation");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaXPath.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaXPath;
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x00036834 File Offset: 0x00035834
		private XmlSchemaUnique Read50_XmlSchemaUnique(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id179_XmlSchemaUnique || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaUnique xmlSchemaUnique = new XmlSchemaUnique();
			XmlAttribute[] array = null;
			int num = 0;
			XmlSchemaObjectCollection fields = xmlSchemaUnique.Fields;
			bool[] array2 = new bool[7];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaUnique.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id4_name && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaUnique.Name = base.Reader.Value;
					array2[4] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaUnique.Namespaces == null)
					{
						xmlSchemaUnique.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaUnique.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaUnique.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaUnique.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaUnique;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaUnique.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else if (!array2[5] && base.Reader.LocalName == this.id175_selector && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaUnique.Selector = this.Read47_XmlSchemaXPath(false, true);
						array2[5] = true;
					}
					else if (base.Reader.LocalName == this.id176_field && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (fields == null)
						{
							base.Reader.Skip();
						}
						else
						{
							fields.Add(this.Read47_XmlSchemaXPath(false, true));
						}
					}
					else
					{
						base.UnknownNode(xmlSchemaUnique, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:selector, http://www.w3.org/2001/XMLSchema:field");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaUnique, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:selector, http://www.w3.org/2001/XMLSchema:field");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaUnique.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaUnique;
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x00036BE0 File Offset: 0x00035BE0
		private XmlSchemaKey Read49_XmlSchemaKey(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id180_XmlSchemaKey || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaKey xmlSchemaKey = new XmlSchemaKey();
			XmlAttribute[] array = null;
			int num = 0;
			XmlSchemaObjectCollection fields = xmlSchemaKey.Fields;
			bool[] array2 = new bool[7];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaKey.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id4_name && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaKey.Name = base.Reader.Value;
					array2[4] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaKey.Namespaces == null)
					{
						xmlSchemaKey.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaKey.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaKey.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaKey.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaKey;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaKey.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else if (!array2[5] && base.Reader.LocalName == this.id175_selector && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaKey.Selector = this.Read47_XmlSchemaXPath(false, true);
						array2[5] = true;
					}
					else if (base.Reader.LocalName == this.id176_field && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (fields == null)
						{
							base.Reader.Skip();
						}
						else
						{
							fields.Add(this.Read47_XmlSchemaXPath(false, true));
						}
					}
					else
					{
						base.UnknownNode(xmlSchemaKey, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:selector, http://www.w3.org/2001/XMLSchema:field");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaKey, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:selector, http://www.w3.org/2001/XMLSchema:field");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaKey.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaKey;
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x00036F8C File Offset: 0x00035F8C
		private XmlSchemaComplexType Read62_XmlSchemaComplexType(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id181_XmlSchemaComplexType || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlAttribute[] array = null;
			int num = 0;
			XmlSchemaObjectCollection attributes = xmlSchemaComplexType.Attributes;
			bool[] array2 = new bool[13];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaComplexType.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id4_name && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaComplexType.Name = base.Reader.Value;
					array2[4] = true;
				}
				else if (!array2[5] && base.Reader.LocalName == this.id129_final && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaComplexType.Final = this.Read7_XmlSchemaDerivationMethod(base.Reader.Value);
					array2[5] = true;
				}
				else if (!array2[6] && base.Reader.LocalName == this.id166_abstract && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaComplexType.IsAbstract = XmlConvert.ToBoolean(base.Reader.Value);
					array2[6] = true;
				}
				else if (!array2[7] && base.Reader.LocalName == this.id167_block && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaComplexType.Block = this.Read7_XmlSchemaDerivationMethod(base.Reader.Value);
					array2[7] = true;
				}
				else if (!array2[8] && base.Reader.LocalName == this.id182_mixed && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaComplexType.IsMixed = XmlConvert.ToBoolean(base.Reader.Value);
					array2[8] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaComplexType.Namespaces == null)
					{
						xmlSchemaComplexType.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaComplexType.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaComplexType.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaComplexType.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaComplexType;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaComplexType.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else if (!array2[9] && base.Reader.LocalName == this.id183_complexContent && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaComplexType.ContentModel = this.Read58_XmlSchemaComplexContent(false, true);
						array2[9] = true;
					}
					else if (!array2[9] && base.Reader.LocalName == this.id184_simpleContent && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaComplexType.ContentModel = this.Read61_XmlSchemaSimpleContent(false, true);
						array2[9] = true;
					}
					else if (!array2[10] && base.Reader.LocalName == this.id59_group && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaComplexType.Particle = this.Read44_XmlSchemaGroupRef(false, true);
						array2[10] = true;
					}
					else if (!array2[10] && base.Reader.LocalName == this.id185_sequence && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaComplexType.Particle = this.Read53_XmlSchemaSequence(false, true);
						array2[10] = true;
					}
					else if (!array2[10] && base.Reader.LocalName == this.id186_choice && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaComplexType.Particle = this.Read54_XmlSchemaChoice(false, true);
						array2[10] = true;
					}
					else if (!array2[10] && base.Reader.LocalName == this.id187_all && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaComplexType.Particle = this.Read55_XmlSchemaAll(false, true);
						array2[10] = true;
					}
					else if (base.Reader.LocalName == this.id109_attribute && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (attributes == null)
						{
							base.Reader.Skip();
						}
						else
						{
							attributes.Add(this.Read36_XmlSchemaAttribute(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id110_attributeGroup && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (attributes == null)
						{
							base.Reader.Skip();
						}
						else
						{
							attributes.Add(this.Read37_XmlSchemaAttributeGroupRef(false, true));
						}
					}
					else if (!array2[12] && base.Reader.LocalName == this.id112_anyAttribute && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaComplexType.AnyAttribute = this.Read39_XmlSchemaAnyAttribute(false, true);
						array2[12] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaComplexType, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:complexContent, http://www.w3.org/2001/XMLSchema:simpleContent, http://www.w3.org/2001/XMLSchema:group, http://www.w3.org/2001/XMLSchema:sequence, http://www.w3.org/2001/XMLSchema:choice, http://www.w3.org/2001/XMLSchema:all, http://www.w3.org/2001/XMLSchema:attribute, http://www.w3.org/2001/XMLSchema:attributeGroup, http://www.w3.org/2001/XMLSchema:anyAttribute");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaComplexType, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:complexContent, http://www.w3.org/2001/XMLSchema:simpleContent, http://www.w3.org/2001/XMLSchema:group, http://www.w3.org/2001/XMLSchema:sequence, http://www.w3.org/2001/XMLSchema:choice, http://www.w3.org/2001/XMLSchema:all, http://www.w3.org/2001/XMLSchema:attribute, http://www.w3.org/2001/XMLSchema:attributeGroup, http://www.w3.org/2001/XMLSchema:anyAttribute");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaComplexType.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaComplexType;
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x00037660 File Offset: 0x00036660
		private XmlSchemaAll Read55_XmlSchemaAll(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id188_XmlSchemaAll || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaAll xmlSchemaAll = new XmlSchemaAll();
			XmlAttribute[] array = null;
			int num = 0;
			XmlSchemaObjectCollection items = xmlSchemaAll.Items;
			bool[] array2 = new bool[7];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaAll.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id164_minOccurs && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaAll.MinOccursString = base.Reader.Value;
					array2[4] = true;
				}
				else if (!array2[5] && base.Reader.LocalName == this.id165_maxOccurs && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaAll.MaxOccursString = base.Reader.Value;
					array2[5] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaAll.Namespaces == null)
					{
						xmlSchemaAll.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaAll.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaAll.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaAll.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaAll;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaAll.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else if (base.Reader.LocalName == this.id92_element && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (items == null)
						{
							base.Reader.Skip();
						}
						else
						{
							items.Add(this.Read52_XmlSchemaElement(false, true));
						}
					}
					else
					{
						base.UnknownNode(xmlSchemaAll, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:element");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaAll, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:element");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaAll.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaAll;
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x00037A10 File Offset: 0x00036A10
		private XmlSchemaChoice Read54_XmlSchemaChoice(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id189_XmlSchemaChoice || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaChoice xmlSchemaChoice = new XmlSchemaChoice();
			XmlAttribute[] array = null;
			int num = 0;
			XmlSchemaObjectCollection items = xmlSchemaChoice.Items;
			bool[] array2 = new bool[7];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaChoice.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id164_minOccurs && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaChoice.MinOccursString = base.Reader.Value;
					array2[4] = true;
				}
				else if (!array2[5] && base.Reader.LocalName == this.id165_maxOccurs && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaChoice.MaxOccursString = base.Reader.Value;
					array2[5] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaChoice.Namespaces == null)
					{
						xmlSchemaChoice.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaChoice.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaChoice.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaChoice.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaChoice;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaChoice.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else if (base.Reader.LocalName == this.id190_any && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (items == null)
						{
							base.Reader.Skip();
						}
						else
						{
							items.Add(this.Read46_XmlSchemaAny(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id186_choice && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (items == null)
						{
							base.Reader.Skip();
						}
						else
						{
							items.Add(this.Read54_XmlSchemaChoice(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id185_sequence && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (items == null)
						{
							base.Reader.Skip();
						}
						else
						{
							items.Add(this.Read53_XmlSchemaSequence(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id92_element && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (items == null)
						{
							base.Reader.Skip();
						}
						else
						{
							items.Add(this.Read52_XmlSchemaElement(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id59_group && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (items == null)
						{
							base.Reader.Skip();
						}
						else
						{
							items.Add(this.Read44_XmlSchemaGroupRef(false, true));
						}
					}
					else
					{
						base.UnknownNode(xmlSchemaChoice, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:any, http://www.w3.org/2001/XMLSchema:choice, http://www.w3.org/2001/XMLSchema:sequence, http://www.w3.org/2001/XMLSchema:element, http://www.w3.org/2001/XMLSchema:group");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaChoice, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:any, http://www.w3.org/2001/XMLSchema:choice, http://www.w3.org/2001/XMLSchema:sequence, http://www.w3.org/2001/XMLSchema:element, http://www.w3.org/2001/XMLSchema:group");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaChoice.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaChoice;
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x00037EF8 File Offset: 0x00036EF8
		private XmlSchemaGroupRef Read44_XmlSchemaGroupRef(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id191_XmlSchemaGroupRef || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaGroupRef xmlSchemaGroupRef = new XmlSchemaGroupRef();
			XmlAttribute[] array = null;
			int num = 0;
			bool[] array2 = new bool[7];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaGroupRef.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id164_minOccurs && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaGroupRef.MinOccursString = base.Reader.Value;
					array2[4] = true;
				}
				else if (!array2[5] && base.Reader.LocalName == this.id165_maxOccurs && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaGroupRef.MaxOccursString = base.Reader.Value;
					array2[5] = true;
				}
				else if (!array2[6] && base.Reader.LocalName == this.id123_ref && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaGroupRef.RefName = base.ToXmlQualifiedName(base.Reader.Value);
					array2[6] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaGroupRef.Namespaces == null)
					{
						xmlSchemaGroupRef.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaGroupRef.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaGroupRef.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaGroupRef.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaGroupRef;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaGroupRef.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaGroupRef, "http://www.w3.org/2001/XMLSchema:annotation");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaGroupRef, "http://www.w3.org/2001/XMLSchema:annotation");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaGroupRef.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaGroupRef;
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x000382A0 File Offset: 0x000372A0
		private XmlSchemaSequence Read53_XmlSchemaSequence(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id192_XmlSchemaSequence || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			XmlAttribute[] array = null;
			int num = 0;
			XmlSchemaObjectCollection items = xmlSchemaSequence.Items;
			bool[] array2 = new bool[7];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaSequence.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id164_minOccurs && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaSequence.MinOccursString = base.Reader.Value;
					array2[4] = true;
				}
				else if (!array2[5] && base.Reader.LocalName == this.id165_maxOccurs && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaSequence.MaxOccursString = base.Reader.Value;
					array2[5] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaSequence.Namespaces == null)
					{
						xmlSchemaSequence.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaSequence.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaSequence.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaSequence.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaSequence;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaSequence.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else if (base.Reader.LocalName == this.id92_element && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (items == null)
						{
							base.Reader.Skip();
						}
						else
						{
							items.Add(this.Read52_XmlSchemaElement(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id185_sequence && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (items == null)
						{
							base.Reader.Skip();
						}
						else
						{
							items.Add(this.Read53_XmlSchemaSequence(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id190_any && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (items == null)
						{
							base.Reader.Skip();
						}
						else
						{
							items.Add(this.Read46_XmlSchemaAny(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id186_choice && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (items == null)
						{
							base.Reader.Skip();
						}
						else
						{
							items.Add(this.Read54_XmlSchemaChoice(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id59_group && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (items == null)
						{
							base.Reader.Skip();
						}
						else
						{
							items.Add(this.Read44_XmlSchemaGroupRef(false, true));
						}
					}
					else
					{
						base.UnknownNode(xmlSchemaSequence, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:element, http://www.w3.org/2001/XMLSchema:sequence, http://www.w3.org/2001/XMLSchema:any, http://www.w3.org/2001/XMLSchema:choice, http://www.w3.org/2001/XMLSchema:group");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaSequence, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:element, http://www.w3.org/2001/XMLSchema:sequence, http://www.w3.org/2001/XMLSchema:any, http://www.w3.org/2001/XMLSchema:choice, http://www.w3.org/2001/XMLSchema:group");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaSequence.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaSequence;
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x00038788 File Offset: 0x00037788
		private XmlSchemaAny Read46_XmlSchemaAny(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id193_XmlSchemaAny || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaAny xmlSchemaAny = new XmlSchemaAny();
			XmlAttribute[] array = null;
			int num = 0;
			bool[] array2 = new bool[8];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaAny.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id164_minOccurs && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaAny.MinOccursString = base.Reader.Value;
					array2[4] = true;
				}
				else if (!array2[5] && base.Reader.LocalName == this.id165_maxOccurs && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaAny.MaxOccursString = base.Reader.Value;
					array2[5] = true;
				}
				else if (!array2[6] && base.Reader.LocalName == this.id36_namespace && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaAny.Namespace = base.Reader.Value;
					array2[6] = true;
				}
				else if (!array2[7] && base.Reader.LocalName == this.id114_processContents && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaAny.ProcessContents = this.Read38_XmlSchemaContentProcessing(base.Reader.Value);
					array2[7] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaAny.Namespaces == null)
					{
						xmlSchemaAny.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaAny.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaAny.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaAny.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaAny;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaAny.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaAny, "http://www.w3.org/2001/XMLSchema:annotation");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaAny, "http://www.w3.org/2001/XMLSchema:annotation");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaAny.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaAny;
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x00038B74 File Offset: 0x00037B74
		private XmlSchemaSimpleContent Read61_XmlSchemaSimpleContent(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id194_XmlSchemaSimpleContent || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaSimpleContent xmlSchemaSimpleContent = new XmlSchemaSimpleContent();
			XmlAttribute[] array = null;
			int num = 0;
			bool[] array2 = new bool[5];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaSimpleContent.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaSimpleContent.Namespaces == null)
					{
						xmlSchemaSimpleContent.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaSimpleContent.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaSimpleContent.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaSimpleContent.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaSimpleContent;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaSimpleContent.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else if (!array2[4] && base.Reader.LocalName == this.id131_restriction && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaSimpleContent.Content = this.Read59_Item(false, true);
						array2[4] = true;
					}
					else if (!array2[4] && base.Reader.LocalName == this.id195_extension && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaSimpleContent.Content = this.Read60_Item(false, true);
						array2[4] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaSimpleContent, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:restriction, http://www.w3.org/2001/XMLSchema:extension");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaSimpleContent, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:restriction, http://www.w3.org/2001/XMLSchema:extension");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaSimpleContent.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaSimpleContent;
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x00038ECC File Offset: 0x00037ECC
		private XmlSchemaSimpleContentExtension Read60_Item(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id196_Item || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaSimpleContentExtension xmlSchemaSimpleContentExtension = new XmlSchemaSimpleContentExtension();
			XmlAttribute[] array = null;
			int num = 0;
			XmlSchemaObjectCollection attributes = xmlSchemaSimpleContentExtension.Attributes;
			bool[] array2 = new bool[7];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaSimpleContentExtension.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id136_base && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaSimpleContentExtension.BaseTypeName = base.ToXmlQualifiedName(base.Reader.Value);
					array2[4] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaSimpleContentExtension.Namespaces == null)
					{
						xmlSchemaSimpleContentExtension.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaSimpleContentExtension.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaSimpleContentExtension.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaSimpleContentExtension.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaSimpleContentExtension;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaSimpleContentExtension.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else if (base.Reader.LocalName == this.id110_attributeGroup && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (attributes == null)
						{
							base.Reader.Skip();
						}
						else
						{
							attributes.Add(this.Read37_XmlSchemaAttributeGroupRef(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id109_attribute && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (attributes == null)
						{
							base.Reader.Skip();
						}
						else
						{
							attributes.Add(this.Read36_XmlSchemaAttribute(false, true));
						}
					}
					else if (!array2[6] && base.Reader.LocalName == this.id112_anyAttribute && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaSimpleContentExtension.AnyAttribute = this.Read39_XmlSchemaAnyAttribute(false, true);
						array2[6] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaSimpleContentExtension, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:attributeGroup, http://www.w3.org/2001/XMLSchema:attribute, http://www.w3.org/2001/XMLSchema:anyAttribute");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaSimpleContentExtension, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:attributeGroup, http://www.w3.org/2001/XMLSchema:attribute, http://www.w3.org/2001/XMLSchema:anyAttribute");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaSimpleContentExtension.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaSimpleContentExtension;
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x000392D0 File Offset: 0x000382D0
		private XmlSchemaSimpleContentRestriction Read59_Item(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id197_Item || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaSimpleContentRestriction xmlSchemaSimpleContentRestriction = new XmlSchemaSimpleContentRestriction();
			XmlAttribute[] array = null;
			int num = 0;
			XmlSchemaObjectCollection facets = xmlSchemaSimpleContentRestriction.Facets;
			XmlSchemaObjectCollection attributes = xmlSchemaSimpleContentRestriction.Attributes;
			bool[] array2 = new bool[9];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaSimpleContentRestriction.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id136_base && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaSimpleContentRestriction.BaseTypeName = base.ToXmlQualifiedName(base.Reader.Value);
					array2[4] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaSimpleContentRestriction.Namespaces == null)
					{
						xmlSchemaSimpleContentRestriction.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaSimpleContentRestriction.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaSimpleContentRestriction.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaSimpleContentRestriction.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaSimpleContentRestriction;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaSimpleContentRestriction.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else if (!array2[5] && base.Reader.LocalName == this.id105_simpleType && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaSimpleContentRestriction.BaseType = this.Read34_XmlSchemaSimpleType(false, true);
						array2[5] = true;
					}
					else if (base.Reader.LocalName == this.id138_minInclusive && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (facets == null)
						{
							base.Reader.Skip();
						}
						else
						{
							facets.Add(this.Read21_XmlSchemaMinInclusiveFacet(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id144_maxExclusive && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (facets == null)
						{
							base.Reader.Skip();
						}
						else
						{
							facets.Add(this.Read28_XmlSchemaMaxExclusiveFacet(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id145_whiteSpace && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (facets == null)
						{
							base.Reader.Skip();
						}
						else
						{
							facets.Add(this.Read29_XmlSchemaWhiteSpaceFacet(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id147_minLength && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (facets == null)
						{
							base.Reader.Skip();
						}
						else
						{
							facets.Add(this.Read31_XmlSchemaMinLengthFacet(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id62_pattern && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (facets == null)
						{
							base.Reader.Skip();
						}
						else
						{
							facets.Add(this.Read25_XmlSchemaPatternFacet(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id142_enumeration && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (facets == null)
						{
							base.Reader.Skip();
						}
						else
						{
							facets.Add(this.Read26_XmlSchemaEnumerationFacet(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id143_maxInclusive && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (facets == null)
						{
							base.Reader.Skip();
						}
						else
						{
							facets.Add(this.Read27_XmlSchemaMaxInclusiveFacet(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id140_length && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (facets == null)
						{
							base.Reader.Skip();
						}
						else
						{
							facets.Add(this.Read23_XmlSchemaLengthFacet(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id139_maxLength && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (facets == null)
						{
							base.Reader.Skip();
						}
						else
						{
							facets.Add(this.Read22_XmlSchemaMaxLengthFacet(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id146_minExclusive && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (facets == null)
						{
							base.Reader.Skip();
						}
						else
						{
							facets.Add(this.Read30_XmlSchemaMinExclusiveFacet(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id141_totalDigits && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (facets == null)
						{
							base.Reader.Skip();
						}
						else
						{
							facets.Add(this.Read24_XmlSchemaTotalDigitsFacet(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id137_fractionDigits && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (facets == null)
						{
							base.Reader.Skip();
						}
						else
						{
							facets.Add(this.Read20_XmlSchemaFractionDigitsFacet(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id110_attributeGroup && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (attributes == null)
						{
							base.Reader.Skip();
						}
						else
						{
							attributes.Add(this.Read37_XmlSchemaAttributeGroupRef(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id109_attribute && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (attributes == null)
						{
							base.Reader.Skip();
						}
						else
						{
							attributes.Add(this.Read36_XmlSchemaAttribute(false, true));
						}
					}
					else if (!array2[8] && base.Reader.LocalName == this.id112_anyAttribute && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaSimpleContentRestriction.AnyAttribute = this.Read39_XmlSchemaAnyAttribute(false, true);
						array2[8] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaSimpleContentRestriction, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:simpleType, http://www.w3.org/2001/XMLSchema:minInclusive, http://www.w3.org/2001/XMLSchema:maxExclusive, http://www.w3.org/2001/XMLSchema:whiteSpace, http://www.w3.org/2001/XMLSchema:minLength, http://www.w3.org/2001/XMLSchema:pattern, http://www.w3.org/2001/XMLSchema:enumeration, http://www.w3.org/2001/XMLSchema:maxInclusive, http://www.w3.org/2001/XMLSchema:length, http://www.w3.org/2001/XMLSchema:maxLength, http://www.w3.org/2001/XMLSchema:minExclusive, http://www.w3.org/2001/XMLSchema:totalDigits, http://www.w3.org/2001/XMLSchema:fractionDigits, http://www.w3.org/2001/XMLSchema:attributeGroup, http://www.w3.org/2001/XMLSchema:attribute, http://www.w3.org/2001/XMLSchema:anyAttribute");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaSimpleContentRestriction, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:simpleType, http://www.w3.org/2001/XMLSchema:minInclusive, http://www.w3.org/2001/XMLSchema:maxExclusive, http://www.w3.org/2001/XMLSchema:whiteSpace, http://www.w3.org/2001/XMLSchema:minLength, http://www.w3.org/2001/XMLSchema:pattern, http://www.w3.org/2001/XMLSchema:enumeration, http://www.w3.org/2001/XMLSchema:maxInclusive, http://www.w3.org/2001/XMLSchema:length, http://www.w3.org/2001/XMLSchema:maxLength, http://www.w3.org/2001/XMLSchema:minExclusive, http://www.w3.org/2001/XMLSchema:totalDigits, http://www.w3.org/2001/XMLSchema:fractionDigits, http://www.w3.org/2001/XMLSchema:attributeGroup, http://www.w3.org/2001/XMLSchema:attribute, http://www.w3.org/2001/XMLSchema:anyAttribute");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaSimpleContentRestriction.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaSimpleContentRestriction;
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x00039AD4 File Offset: 0x00038AD4
		private XmlSchemaComplexContent Read58_XmlSchemaComplexContent(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id198_XmlSchemaComplexContent || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaComplexContent xmlSchemaComplexContent = new XmlSchemaComplexContent();
			XmlAttribute[] array = null;
			int num = 0;
			bool[] array2 = new bool[6];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaComplexContent.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id182_mixed && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaComplexContent.IsMixed = XmlConvert.ToBoolean(base.Reader.Value);
					array2[4] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaComplexContent.Namespaces == null)
					{
						xmlSchemaComplexContent.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaComplexContent.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaComplexContent.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaComplexContent.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaComplexContent;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaComplexContent.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else if (!array2[5] && base.Reader.LocalName == this.id195_extension && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaComplexContent.Content = this.Read56_Item(false, true);
						array2[5] = true;
					}
					else if (!array2[5] && base.Reader.LocalName == this.id131_restriction && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaComplexContent.Content = this.Read57_Item(false, true);
						array2[5] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaComplexContent, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:extension, http://www.w3.org/2001/XMLSchema:restriction");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaComplexContent, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:extension, http://www.w3.org/2001/XMLSchema:restriction");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaComplexContent.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaComplexContent;
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x00039E78 File Offset: 0x00038E78
		private XmlSchemaComplexContentRestriction Read57_Item(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id199_Item || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaComplexContentRestriction xmlSchemaComplexContentRestriction = new XmlSchemaComplexContentRestriction();
			XmlAttribute[] array = null;
			int num = 0;
			XmlSchemaObjectCollection attributes = xmlSchemaComplexContentRestriction.Attributes;
			bool[] array2 = new bool[8];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaComplexContentRestriction.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id136_base && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaComplexContentRestriction.BaseTypeName = base.ToXmlQualifiedName(base.Reader.Value);
					array2[4] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaComplexContentRestriction.Namespaces == null)
					{
						xmlSchemaComplexContentRestriction.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaComplexContentRestriction.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaComplexContentRestriction.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaComplexContentRestriction.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaComplexContentRestriction;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaComplexContentRestriction.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else if (!array2[5] && base.Reader.LocalName == this.id186_choice && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaComplexContentRestriction.Particle = this.Read54_XmlSchemaChoice(false, true);
						array2[5] = true;
					}
					else if (!array2[5] && base.Reader.LocalName == this.id59_group && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaComplexContentRestriction.Particle = this.Read44_XmlSchemaGroupRef(false, true);
						array2[5] = true;
					}
					else if (!array2[5] && base.Reader.LocalName == this.id187_all && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaComplexContentRestriction.Particle = this.Read55_XmlSchemaAll(false, true);
						array2[5] = true;
					}
					else if (!array2[5] && base.Reader.LocalName == this.id185_sequence && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaComplexContentRestriction.Particle = this.Read53_XmlSchemaSequence(false, true);
						array2[5] = true;
					}
					else if (base.Reader.LocalName == this.id110_attributeGroup && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (attributes == null)
						{
							base.Reader.Skip();
						}
						else
						{
							attributes.Add(this.Read37_XmlSchemaAttributeGroupRef(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id109_attribute && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (attributes == null)
						{
							base.Reader.Skip();
						}
						else
						{
							attributes.Add(this.Read36_XmlSchemaAttribute(false, true));
						}
					}
					else if (!array2[7] && base.Reader.LocalName == this.id112_anyAttribute && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaComplexContentRestriction.AnyAttribute = this.Read39_XmlSchemaAnyAttribute(false, true);
						array2[7] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaComplexContentRestriction, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:choice, http://www.w3.org/2001/XMLSchema:group, http://www.w3.org/2001/XMLSchema:all, http://www.w3.org/2001/XMLSchema:sequence, http://www.w3.org/2001/XMLSchema:attributeGroup, http://www.w3.org/2001/XMLSchema:attribute, http://www.w3.org/2001/XMLSchema:anyAttribute");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaComplexContentRestriction, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:choice, http://www.w3.org/2001/XMLSchema:group, http://www.w3.org/2001/XMLSchema:all, http://www.w3.org/2001/XMLSchema:sequence, http://www.w3.org/2001/XMLSchema:attributeGroup, http://www.w3.org/2001/XMLSchema:attribute, http://www.w3.org/2001/XMLSchema:anyAttribute");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaComplexContentRestriction.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaComplexContentRestriction;
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x0003A38C File Offset: 0x0003938C
		private XmlSchemaComplexContentExtension Read56_Item(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id200_Item || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaComplexContentExtension xmlSchemaComplexContentExtension = new XmlSchemaComplexContentExtension();
			XmlAttribute[] array = null;
			int num = 0;
			XmlSchemaObjectCollection attributes = xmlSchemaComplexContentExtension.Attributes;
			bool[] array2 = new bool[8];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaComplexContentExtension.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id136_base && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaComplexContentExtension.BaseTypeName = base.ToXmlQualifiedName(base.Reader.Value);
					array2[4] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaComplexContentExtension.Namespaces == null)
					{
						xmlSchemaComplexContentExtension.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaComplexContentExtension.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaComplexContentExtension.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaComplexContentExtension.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaComplexContentExtension;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaComplexContentExtension.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else if (!array2[5] && base.Reader.LocalName == this.id59_group && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaComplexContentExtension.Particle = this.Read44_XmlSchemaGroupRef(false, true);
						array2[5] = true;
					}
					else if (!array2[5] && base.Reader.LocalName == this.id186_choice && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaComplexContentExtension.Particle = this.Read54_XmlSchemaChoice(false, true);
						array2[5] = true;
					}
					else if (!array2[5] && base.Reader.LocalName == this.id187_all && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaComplexContentExtension.Particle = this.Read55_XmlSchemaAll(false, true);
						array2[5] = true;
					}
					else if (!array2[5] && base.Reader.LocalName == this.id185_sequence && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaComplexContentExtension.Particle = this.Read53_XmlSchemaSequence(false, true);
						array2[5] = true;
					}
					else if (base.Reader.LocalName == this.id110_attributeGroup && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (attributes == null)
						{
							base.Reader.Skip();
						}
						else
						{
							attributes.Add(this.Read37_XmlSchemaAttributeGroupRef(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id109_attribute && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (attributes == null)
						{
							base.Reader.Skip();
						}
						else
						{
							attributes.Add(this.Read36_XmlSchemaAttribute(false, true));
						}
					}
					else if (!array2[7] && base.Reader.LocalName == this.id112_anyAttribute && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaComplexContentExtension.AnyAttribute = this.Read39_XmlSchemaAnyAttribute(false, true);
						array2[7] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaComplexContentExtension, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:group, http://www.w3.org/2001/XMLSchema:choice, http://www.w3.org/2001/XMLSchema:all, http://www.w3.org/2001/XMLSchema:sequence, http://www.w3.org/2001/XMLSchema:attributeGroup, http://www.w3.org/2001/XMLSchema:attribute, http://www.w3.org/2001/XMLSchema:anyAttribute");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaComplexContentExtension, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:group, http://www.w3.org/2001/XMLSchema:choice, http://www.w3.org/2001/XMLSchema:all, http://www.w3.org/2001/XMLSchema:sequence, http://www.w3.org/2001/XMLSchema:attributeGroup, http://www.w3.org/2001/XMLSchema:attribute, http://www.w3.org/2001/XMLSchema:anyAttribute");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaComplexContentExtension.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaComplexContentExtension;
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x0003A8A0 File Offset: 0x000398A0
		private XmlSchemaGroup Read63_XmlSchemaGroup(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id201_XmlSchemaGroup || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaGroup xmlSchemaGroup = new XmlSchemaGroup();
			XmlAttribute[] array = null;
			int num = 0;
			bool[] array2 = new bool[6];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaGroup.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id4_name && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaGroup.Name = base.Reader.Value;
					array2[4] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaGroup.Namespaces == null)
					{
						xmlSchemaGroup.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaGroup.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaGroup.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaGroup.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaGroup;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaGroup.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else if (!array2[5] && base.Reader.LocalName == this.id185_sequence && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaGroup.Particle = this.Read53_XmlSchemaSequence(false, true);
						array2[5] = true;
					}
					else if (!array2[5] && base.Reader.LocalName == this.id186_choice && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaGroup.Particle = this.Read54_XmlSchemaChoice(false, true);
						array2[5] = true;
					}
					else if (!array2[5] && base.Reader.LocalName == this.id187_all && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaGroup.Particle = this.Read55_XmlSchemaAll(false, true);
						array2[5] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaGroup, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:sequence, http://www.w3.org/2001/XMLSchema:choice, http://www.w3.org/2001/XMLSchema:all");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaGroup, "http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:sequence, http://www.w3.org/2001/XMLSchema:choice, http://www.w3.org/2001/XMLSchema:all");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaGroup.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaGroup;
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x0003AC80 File Offset: 0x00039C80
		private XmlSchemaNotation Read65_XmlSchemaNotation(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id202_XmlSchemaNotation || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaNotation xmlSchemaNotation = new XmlSchemaNotation();
			XmlAttribute[] array = null;
			int num = 0;
			bool[] array2 = new bool[7];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaNotation.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id4_name && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaNotation.Name = base.Reader.Value;
					array2[4] = true;
				}
				else if (!array2[5] && base.Reader.LocalName == this.id203_public && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaNotation.Public = base.Reader.Value;
					array2[5] = true;
				}
				else if (!array2[6] && base.Reader.LocalName == this.id204_system && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaNotation.System = base.Reader.Value;
					array2[6] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaNotation.Namespaces == null)
					{
						xmlSchemaNotation.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaNotation.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaNotation.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaNotation.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaNotation;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[2] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaNotation.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[2] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaNotation, "http://www.w3.org/2001/XMLSchema:annotation");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaNotation, "http://www.w3.org/2001/XMLSchema:annotation");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaNotation.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaNotation;
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x0003B020 File Offset: 0x0003A020
		private XmlSchemaRedefine Read64_XmlSchemaRedefine(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id205_XmlSchemaRedefine || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaRedefine xmlSchemaRedefine = new XmlSchemaRedefine();
			XmlAttribute[] array = null;
			int num = 0;
			XmlSchemaObjectCollection items = xmlSchemaRedefine.Items;
			bool[] array2 = new bool[5];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id206_schemaLocation && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaRedefine.SchemaLocation = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[2] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaRedefine.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[2] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaRedefine.Namespaces == null)
					{
						xmlSchemaRedefine.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaRedefine.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaRedefine.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaRedefine.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaRedefine;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (base.Reader.LocalName == this.id110_attributeGroup && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (items == null)
						{
							base.Reader.Skip();
						}
						else
						{
							items.Add(this.Read40_XmlSchemaAttributeGroup(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id106_complexType && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (items == null)
						{
							base.Reader.Skip();
						}
						else
						{
							items.Add(this.Read62_XmlSchemaComplexType(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id105_simpleType && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (items == null)
						{
							base.Reader.Skip();
						}
						else
						{
							items.Add(this.Read34_XmlSchemaSimpleType(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (items == null)
						{
							base.Reader.Skip();
						}
						else
						{
							items.Add(this.Read11_XmlSchemaAnnotation(false, true));
						}
					}
					else if (base.Reader.LocalName == this.id59_group && base.Reader.NamespaceURI == this.id95_Item)
					{
						if (items == null)
						{
							base.Reader.Skip();
						}
						else
						{
							items.Add(this.Read63_XmlSchemaGroup(false, true));
						}
					}
					else
					{
						base.UnknownNode(xmlSchemaRedefine, "http://www.w3.org/2001/XMLSchema:attributeGroup, http://www.w3.org/2001/XMLSchema:complexType, http://www.w3.org/2001/XMLSchema:simpleType, http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:group");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaRedefine, "http://www.w3.org/2001/XMLSchema:attributeGroup, http://www.w3.org/2001/XMLSchema:complexType, http://www.w3.org/2001/XMLSchema:simpleType, http://www.w3.org/2001/XMLSchema:annotation, http://www.w3.org/2001/XMLSchema:group");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaRedefine.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaRedefine;
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x0003B484 File Offset: 0x0003A484
		private XmlSchemaImport Read13_XmlSchemaImport(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id207_XmlSchemaImport || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaImport xmlSchemaImport = new XmlSchemaImport();
			XmlAttribute[] array = null;
			int num = 0;
			bool[] array2 = new bool[6];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id206_schemaLocation && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaImport.SchemaLocation = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[2] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaImport.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[2] = true;
				}
				else if (!array2[4] && base.Reader.LocalName == this.id36_namespace && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaImport.Namespace = base.CollapseWhitespace(base.Reader.Value);
					array2[4] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaImport.Namespaces == null)
					{
						xmlSchemaImport.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaImport.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaImport.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaImport.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaImport;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[5] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaImport.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[5] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaImport, "http://www.w3.org/2001/XMLSchema:annotation");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaImport, "http://www.w3.org/2001/XMLSchema:annotation");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaImport.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaImport;
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x0003B7E8 File Offset: 0x0003A7E8
		private XmlSchemaInclude Read12_XmlSchemaInclude(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id208_XmlSchemaInclude || xmlQualifiedName.Namespace != this.id95_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			base.DecodeName = false;
			XmlSchemaInclude xmlSchemaInclude = new XmlSchemaInclude();
			XmlAttribute[] array = null;
			int num = 0;
			bool[] array2 = new bool[5];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id206_schemaLocation && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaInclude.SchemaLocation = base.CollapseWhitespace(base.Reader.Value);
					array2[1] = true;
				}
				else if (!array2[2] && base.Reader.LocalName == this.id102_id && base.Reader.NamespaceURI == this.id5_Item)
				{
					xmlSchemaInclude.Id = base.CollapseWhitespace(base.Reader.Value);
					array2[2] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (xmlSchemaInclude.Namespaces == null)
					{
						xmlSchemaInclude.Namespaces = new XmlSerializerNamespaces();
					}
					xmlSchemaInclude.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			xmlSchemaInclude.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				xmlSchemaInclude.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return xmlSchemaInclude;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[4] && base.Reader.LocalName == this.id107_annotation && base.Reader.NamespaceURI == this.id95_Item)
					{
						xmlSchemaInclude.Annotation = this.Read11_XmlSchemaAnnotation(false, true);
						array2[4] = true;
					}
					else
					{
						base.UnknownNode(xmlSchemaInclude, "http://www.w3.org/2001/XMLSchema:annotation");
					}
				}
				else
				{
					base.UnknownNode(xmlSchemaInclude, "http://www.w3.org/2001/XMLSchema:annotation");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			xmlSchemaInclude.UnhandledAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return xmlSchemaInclude;
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x0003BB00 File Offset: 0x0003AB00
		private Import Read4_Import(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = (checkType ? base.GetXsiType() : null);
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id209_Import || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			Import import = new Import();
			XmlAttribute[] array = null;
			int num = 0;
			ServiceDescriptionFormatExtensionCollection extensions = import.Extensions;
			bool[] array2 = new bool[6];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[4] && base.Reader.LocalName == this.id36_namespace && base.Reader.NamespaceURI == this.id5_Item)
				{
					import.Namespace = base.Reader.Value;
					array2[4] = true;
				}
				else if (!array2[5] && base.Reader.LocalName == this.id23_location && base.Reader.NamespaceURI == this.id5_Item)
				{
					import.Location = base.Reader.Value;
					array2[5] = true;
				}
				else if (base.IsXmlnsAttribute(base.Reader.Name))
				{
					if (import.Namespaces == null)
					{
						import.Namespaces = new XmlSerializerNamespaces();
					}
					import.Namespaces.Add((base.Reader.Name.Length == 5) ? "" : base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
					base.ParseWsdlArrayType(xmlAttribute);
					array = (XmlAttribute[])base.EnsureArrayIndex(array, num, typeof(XmlAttribute));
					array[num++] = xmlAttribute;
				}
			}
			import.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				import.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
				return import;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[0] && base.Reader.LocalName == this.id7_documentation && base.Reader.NamespaceURI == this.id2_Item)
					{
						import.DocumentationElement = (XmlElement)base.ReadXmlNode(false);
						array2[0] = true;
					}
					else
					{
						extensions.Add((XmlElement)base.ReadXmlNode(false));
					}
				}
				else
				{
					base.UnknownNode(import, "http://schemas.xmlsoap.org/wsdl/:documentation");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			import.ExtensibleAttributes = (XmlAttribute[])base.ShrinkArray(array, num, typeof(XmlAttribute), true);
			base.ReadEndElement();
			return import;
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x0003BE1B File Offset: 0x0003AE1B
		protected override void InitCallbacks()
		{
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x0003BE20 File Offset: 0x0003AE20
		protected override void InitIDs()
		{
			this.id133_XmlSchemaSimpleTypeUnion = base.Reader.NameTable.Add("XmlSchemaSimpleTypeUnion");
			this.id143_maxInclusive = base.Reader.NameTable.Add("maxInclusive");
			this.id46_body = base.Reader.NameTable.Add("body");
			this.id190_any = base.Reader.NameTable.Add("any");
			this.id88_OperationOutput = base.Reader.NameTable.Add("OperationOutput");
			this.id6_targetNamespace = base.Reader.NameTable.Add("targetNamespace");
			this.id158_XmlSchemaMaxLengthFacet = base.Reader.NameTable.Add("XmlSchemaMaxLengthFacet");
			this.id11_portType = base.Reader.NameTable.Add("portType");
			this.id182_mixed = base.Reader.NameTable.Add("mixed");
			this.id172_keyref = base.Reader.NameTable.Add("keyref");
			this.id187_all = base.Reader.NameTable.Add("all");
			this.id162_itemType = base.Reader.NameTable.Add("itemType");
			this.id68_InputBinding = base.Reader.NameTable.Add("InputBinding");
			this.id25_HttpAddressBinding = base.Reader.NameTable.Add("HttpAddressBinding");
			this.id82_HttpBinding = base.Reader.NameTable.Add("HttpBinding");
			this.id17_address = base.Reader.NameTable.Add("address");
			this.id3_ServiceDescription = base.Reader.NameTable.Add("ServiceDescription");
			this.id38_SoapFaultBinding = base.Reader.NameTable.Add("SoapFaultBinding");
			this.id123_ref = base.Reader.NameTable.Add("ref");
			this.id198_XmlSchemaComplexContent = base.Reader.NameTable.Add("XmlSchemaComplexContent");
			this.id53_parts = base.Reader.NameTable.Add("parts");
			this.id35_use = base.Reader.NameTable.Add("use");
			this.id157_XmlSchemaLengthFacet = base.Reader.NameTable.Add("XmlSchemaLengthFacet");
			this.id207_XmlSchemaImport = base.Reader.NameTable.Add("XmlSchemaImport");
			this.id44_text = base.Reader.NameTable.Add("text");
			this.id117_XmlSchemaAppInfo = base.Reader.NameTable.Add("XmlSchemaAppInfo");
			this.id203_public = base.Reader.NameTable.Add("public");
			this.id69_urlEncoded = base.Reader.NameTable.Add("urlEncoded");
			this.id7_documentation = base.Reader.NameTable.Add("documentation");
			this.id19_Item = base.Reader.NameTable.Add("http://schemas.xmlsoap.org/wsdl/soap/");
			this.id129_final = base.Reader.NameTable.Add("final");
			this.id163_XmlSchemaElement = base.Reader.NameTable.Add("XmlSchemaElement");
			this.id60_capture = base.Reader.NameTable.Add("capture");
			this.id37_encodingStyle = base.Reader.NameTable.Add("encodingStyle");
			this.id185_sequence = base.Reader.NameTable.Add("sequence");
			this.id166_abstract = base.Reader.NameTable.Add("abstract");
			this.id23_location = base.Reader.NameTable.Add("location");
			this.id111_XmlSchemaAttributeGroup = base.Reader.NameTable.Add("XmlSchemaAttributeGroup");
			this.id192_XmlSchemaSequence = base.Reader.NameTable.Add("XmlSchemaSequence");
			this.id33_FaultBinding = base.Reader.NameTable.Add("FaultBinding");
			this.id153_XmlSchemaMaxInclusiveFacet = base.Reader.NameTable.Add("XmlSchemaMaxInclusiveFacet");
			this.id201_XmlSchemaGroup = base.Reader.NameTable.Add("XmlSchemaGroup");
			this.id43_multipartRelated = base.Reader.NameTable.Add("multipartRelated");
			this.id168_nillable = base.Reader.NameTable.Add("nillable");
			this.id149_value = base.Reader.NameTable.Add("value");
			this.id64_MimeMultipartRelatedBinding = base.Reader.NameTable.Add("MimeMultipartRelatedBinding");
			this.id193_XmlSchemaAny = base.Reader.NameTable.Add("XmlSchemaAny");
			this.id191_XmlSchemaGroupRef = base.Reader.NameTable.Add("XmlSchemaGroupRef");
			this.id74_soapAction = base.Reader.NameTable.Add("soapAction");
			this.id63_ignoreCase = base.Reader.NameTable.Add("ignoreCase");
			this.id101_version = base.Reader.NameTable.Add("version");
			this.id47_header = base.Reader.NameTable.Add("header");
			this.id195_extension = base.Reader.NameTable.Add("extension");
			this.id48_Soap12HeaderBinding = base.Reader.NameTable.Add("Soap12HeaderBinding");
			this.id134_memberTypes = base.Reader.NameTable.Add("memberTypes");
			this.id121_Item = base.Reader.NameTable.Add("http://www.w3.org/XML/1998/namespace");
			this.id146_minExclusive = base.Reader.NameTable.Add("minExclusive");
			this.id84_PortType = base.Reader.NameTable.Add("PortType");
			this.id42_mimeXml = base.Reader.NameTable.Add("mimeXml");
			this.id138_minInclusive = base.Reader.NameTable.Add("minInclusive");
			this.id118_source = base.Reader.NameTable.Add("source");
			this.id73_Soap12OperationBinding = base.Reader.NameTable.Add("Soap12OperationBinding");
			this.id131_restriction = base.Reader.NameTable.Add("restriction");
			this.id152_XmlSchemaMaxExclusiveFacet = base.Reader.NameTable.Add("XmlSchemaMaxExclusiveFacet");
			this.id135_XmlSchemaSimpleTypeRestriction = base.Reader.NameTable.Add("XmlSchemaSimpleTypeRestriction");
			this.id188_XmlSchemaAll = base.Reader.NameTable.Add("XmlSchemaAll");
			this.id116_appinfo = base.Reader.NameTable.Add("appinfo");
			this.id86_parameterOrder = base.Reader.NameTable.Add("parameterOrder");
			this.id147_minLength = base.Reader.NameTable.Add("minLength");
			this.id78_HttpOperationBinding = base.Reader.NameTable.Add("HttpOperationBinding");
			this.id161_XmlSchemaSimpleTypeList = base.Reader.NameTable.Add("XmlSchemaSimpleTypeList");
			this.id205_XmlSchemaRedefine = base.Reader.NameTable.Add("XmlSchemaRedefine");
			this.id194_XmlSchemaSimpleContent = base.Reader.NameTable.Add("XmlSchemaSimpleContent");
			this.id91_MessagePart = base.Reader.NameTable.Add("MessagePart");
			this.id92_element = base.Reader.NameTable.Add("element");
			this.id114_processContents = base.Reader.NameTable.Add("processContents");
			this.id18_Item = base.Reader.NameTable.Add("http://schemas.xmlsoap.org/wsdl/http/");
			this.id50_headerfault = base.Reader.NameTable.Add("headerfault");
			this.id154_XmlSchemaEnumerationFacet = base.Reader.NameTable.Add("XmlSchemaEnumerationFacet");
			this.id96_XmlSchema = base.Reader.NameTable.Add("XmlSchema");
			this.id127_form = base.Reader.NameTable.Add("form");
			this.id176_field = base.Reader.NameTable.Add("field");
			this.id49_part = base.Reader.NameTable.Add("part");
			this.id5_Item = base.Reader.NameTable.Add("");
			this.id57_match = base.Reader.NameTable.Add("match");
			this.id52_Soap12BodyBinding = base.Reader.NameTable.Add("Soap12BodyBinding");
			this.id104_redefine = base.Reader.NameTable.Add("redefine");
			this.id20_Item = base.Reader.NameTable.Add("http://schemas.xmlsoap.org/wsdl/soap12/");
			this.id21_Soap12AddressBinding = base.Reader.NameTable.Add("Soap12AddressBinding");
			this.id142_enumeration = base.Reader.NameTable.Add("enumeration");
			this.id24_SoapAddressBinding = base.Reader.NameTable.Add("SoapAddressBinding");
			this.id103_include = base.Reader.NameTable.Add("include");
			this.id139_maxLength = base.Reader.NameTable.Add("maxLength");
			this.id165_maxOccurs = base.Reader.NameTable.Add("maxOccurs");
			this.id65_MimePart = base.Reader.NameTable.Add("MimePart");
			this.id102_id = base.Reader.NameTable.Add("id");
			this.id196_Item = base.Reader.NameTable.Add("XmlSchemaSimpleContentExtension");
			this.id140_length = base.Reader.NameTable.Add("length");
			this.id27_type = base.Reader.NameTable.Add("type");
			this.id106_complexType = base.Reader.NameTable.Add("complexType");
			this.id31_output = base.Reader.NameTable.Add("output");
			this.id1_definitions = base.Reader.NameTable.Add("definitions");
			this.id4_name = base.Reader.NameTable.Add("name");
			this.id132_union = base.Reader.NameTable.Add("union");
			this.id29_OperationBinding = base.Reader.NameTable.Add("OperationBinding");
			this.id170_key = base.Reader.NameTable.Add("key");
			this.id45_Item = base.Reader.NameTable.Add("http://microsoft.com/wsdl/mime/textMatching/");
			this.id95_Item = base.Reader.NameTable.Add("http://www.w3.org/2001/XMLSchema");
			this.id169_substitutionGroup = base.Reader.NameTable.Add("substitutionGroup");
			this.id178_xpath = base.Reader.NameTable.Add("xpath");
			this.id9_types = base.Reader.NameTable.Add("types");
			this.id97_attributeFormDefault = base.Reader.NameTable.Add("attributeFormDefault");
			this.id62_pattern = base.Reader.NameTable.Add("pattern");
			this.id58_MimeTextMatch = base.Reader.NameTable.Add("MimeTextMatch");
			this.id180_XmlSchemaKey = base.Reader.NameTable.Add("XmlSchemaKey");
			this.id10_message = base.Reader.NameTable.Add("message");
			this.id8_import = base.Reader.NameTable.Add("import");
			this.id148_XmlSchemaMinLengthFacet = base.Reader.NameTable.Add("XmlSchemaMinLengthFacet");
			this.id105_simpleType = base.Reader.NameTable.Add("simpleType");
			this.id181_XmlSchemaComplexType = base.Reader.NameTable.Add("XmlSchemaComplexType");
			this.id164_minOccurs = base.Reader.NameTable.Add("minOccurs");
			this.id144_maxExclusive = base.Reader.NameTable.Add("maxExclusive");
			this.id160_XmlSchemaFractionDigitsFacet = base.Reader.NameTable.Add("XmlSchemaFractionDigitsFacet");
			this.id124_XmlSchemaAttribute = base.Reader.NameTable.Add("XmlSchemaAttribute");
			this.id209_Import = base.Reader.NameTable.Add("Import");
			this.id206_schemaLocation = base.Reader.NameTable.Add("schemaLocation");
			this.id179_XmlSchemaUnique = base.Reader.NameTable.Add("XmlSchemaUnique");
			this.id75_style = base.Reader.NameTable.Add("style");
			this.id119_XmlSchemaDocumentation = base.Reader.NameTable.Add("XmlSchemaDocumentation");
			this.id136_base = base.Reader.NameTable.Add("base");
			this.id66_MimeXmlBinding = base.Reader.NameTable.Add("MimeXmlBinding");
			this.id30_input = base.Reader.NameTable.Add("input");
			this.id40_content = base.Reader.NameTable.Add("content");
			this.id93_Types = base.Reader.NameTable.Add("Types");
			this.id94_schema = base.Reader.NameTable.Add("schema");
			this.id200_Item = base.Reader.NameTable.Add("XmlSchemaComplexContentExtension");
			this.id67_MimeContentBinding = base.Reader.NameTable.Add("MimeContentBinding");
			this.id59_group = base.Reader.NameTable.Add("group");
			this.id32_fault = base.Reader.NameTable.Add("fault");
			this.id80_transport = base.Reader.NameTable.Add("transport");
			this.id98_blockDefault = base.Reader.NameTable.Add("blockDefault");
			this.id13_service = base.Reader.NameTable.Add("service");
			this.id54_SoapHeaderBinding = base.Reader.NameTable.Add("SoapHeaderBinding");
			this.id204_system = base.Reader.NameTable.Add("system");
			this.id16_Port = base.Reader.NameTable.Add("Port");
			this.id108_notation = base.Reader.NameTable.Add("notation");
			this.id186_choice = base.Reader.NameTable.Add("choice");
			this.id110_attributeGroup = base.Reader.NameTable.Add("attributeGroup");
			this.id79_Soap12Binding = base.Reader.NameTable.Add("Soap12Binding");
			this.id77_SoapOperationBinding = base.Reader.NameTable.Add("SoapOperationBinding");
			this.id115_XmlSchemaAnnotation = base.Reader.NameTable.Add("XmlSchemaAnnotation");
			this.id83_verb = base.Reader.NameTable.Add("verb");
			this.id72_HttpUrlEncodedBinding = base.Reader.NameTable.Add("HttpUrlEncodedBinding");
			this.id39_OutputBinding = base.Reader.NameTable.Add("OutputBinding");
			this.id183_complexContent = base.Reader.NameTable.Add("complexContent");
			this.id202_XmlSchemaNotation = base.Reader.NameTable.Add("XmlSchemaNotation");
			this.id81_SoapBinding = base.Reader.NameTable.Add("SoapBinding");
			this.id199_Item = base.Reader.NameTable.Add("XmlSchemaComplexContentRestriction");
			this.id28_operation = base.Reader.NameTable.Add("operation");
			this.id122_XmlSchemaAttributeGroupRef = base.Reader.NameTable.Add("XmlSchemaAttributeGroupRef");
			this.id155_XmlSchemaPatternFacet = base.Reader.NameTable.Add("XmlSchemaPatternFacet");
			this.id76_soapActionRequired = base.Reader.NameTable.Add("soapActionRequired");
			this.id90_Message = base.Reader.NameTable.Add("Message");
			this.id159_XmlSchemaMinInclusiveFacet = base.Reader.NameTable.Add("XmlSchemaMinInclusiveFacet");
			this.id208_XmlSchemaInclude = base.Reader.NameTable.Add("XmlSchemaInclude");
			this.id85_Operation = base.Reader.NameTable.Add("Operation");
			this.id130_list = base.Reader.NameTable.Add("list");
			this.id14_Service = base.Reader.NameTable.Add("Service");
			this.id22_required = base.Reader.NameTable.Add("required");
			this.id174_refer = base.Reader.NameTable.Add("refer");
			this.id71_HttpUrlReplacementBinding = base.Reader.NameTable.Add("HttpUrlReplacementBinding");
			this.id56_MimeTextBinding = base.Reader.NameTable.Add("MimeTextBinding");
			this.id87_OperationFault = base.Reader.NameTable.Add("OperationFault");
			this.id125_default = base.Reader.NameTable.Add("default");
			this.id15_port = base.Reader.NameTable.Add("port");
			this.id51_SoapHeaderFaultBinding = base.Reader.NameTable.Add("SoapHeaderFaultBinding");
			this.id128_XmlSchemaSimpleType = base.Reader.NameTable.Add("XmlSchemaSimpleType");
			this.id36_namespace = base.Reader.NameTable.Add("namespace");
			this.id175_selector = base.Reader.NameTable.Add("selector");
			this.id150_XmlSchemaMinExclusiveFacet = base.Reader.NameTable.Add("XmlSchemaMinExclusiveFacet");
			this.id100_elementFormDefault = base.Reader.NameTable.Add("elementFormDefault");
			this.id26_Binding = base.Reader.NameTable.Add("Binding");
			this.id197_Item = base.Reader.NameTable.Add("XmlSchemaSimpleContentRestriction");
			this.id126_fixed = base.Reader.NameTable.Add("fixed");
			this.id107_annotation = base.Reader.NameTable.Add("annotation");
			this.id99_finalDefault = base.Reader.NameTable.Add("finalDefault");
			this.id137_fractionDigits = base.Reader.NameTable.Add("fractionDigits");
			this.id70_urlReplacement = base.Reader.NameTable.Add("urlReplacement");
			this.id189_XmlSchemaChoice = base.Reader.NameTable.Add("XmlSchemaChoice");
			this.id2_Item = base.Reader.NameTable.Add("http://schemas.xmlsoap.org/wsdl/");
			this.id112_anyAttribute = base.Reader.NameTable.Add("anyAttribute");
			this.id89_OperationInput = base.Reader.NameTable.Add("OperationInput");
			this.id141_totalDigits = base.Reader.NameTable.Add("totalDigits");
			this.id61_repeats = base.Reader.NameTable.Add("repeats");
			this.id184_simpleContent = base.Reader.NameTable.Add("simpleContent");
			this.id55_SoapBodyBinding = base.Reader.NameTable.Add("SoapBodyBinding");
			this.id145_whiteSpace = base.Reader.NameTable.Add("whiteSpace");
			this.id167_block = base.Reader.NameTable.Add("block");
			this.id151_XmlSchemaWhiteSpaceFacet = base.Reader.NameTable.Add("XmlSchemaWhiteSpaceFacet");
			this.id12_binding = base.Reader.NameTable.Add("binding");
			this.id109_attribute = base.Reader.NameTable.Add("attribute");
			this.id171_unique = base.Reader.NameTable.Add("unique");
			this.id120_lang = base.Reader.NameTable.Add("lang");
			this.id173_XmlSchemaKeyref = base.Reader.NameTable.Add("XmlSchemaKeyref");
			this.id177_XmlSchemaXPath = base.Reader.NameTable.Add("XmlSchemaXPath");
			this.id34_Soap12FaultBinding = base.Reader.NameTable.Add("Soap12FaultBinding");
			this.id41_Item = base.Reader.NameTable.Add("http://schemas.xmlsoap.org/wsdl/mime/");
			this.id156_XmlSchemaTotalDigitsFacet = base.Reader.NameTable.Add("XmlSchemaTotalDigitsFacet");
			this.id113_XmlSchemaAnyAttribute = base.Reader.NameTable.Add("XmlSchemaAnyAttribute");
		}

		// Token: 0x040004B3 RID: 1203
		private Hashtable _XmlSchemaDerivationMethodValues;

		// Token: 0x040004B4 RID: 1204
		private string id133_XmlSchemaSimpleTypeUnion;

		// Token: 0x040004B5 RID: 1205
		private string id143_maxInclusive;

		// Token: 0x040004B6 RID: 1206
		private string id46_body;

		// Token: 0x040004B7 RID: 1207
		private string id190_any;

		// Token: 0x040004B8 RID: 1208
		private string id88_OperationOutput;

		// Token: 0x040004B9 RID: 1209
		private string id6_targetNamespace;

		// Token: 0x040004BA RID: 1210
		private string id158_XmlSchemaMaxLengthFacet;

		// Token: 0x040004BB RID: 1211
		private string id11_portType;

		// Token: 0x040004BC RID: 1212
		private string id182_mixed;

		// Token: 0x040004BD RID: 1213
		private string id172_keyref;

		// Token: 0x040004BE RID: 1214
		private string id187_all;

		// Token: 0x040004BF RID: 1215
		private string id162_itemType;

		// Token: 0x040004C0 RID: 1216
		private string id68_InputBinding;

		// Token: 0x040004C1 RID: 1217
		private string id25_HttpAddressBinding;

		// Token: 0x040004C2 RID: 1218
		private string id82_HttpBinding;

		// Token: 0x040004C3 RID: 1219
		private string id17_address;

		// Token: 0x040004C4 RID: 1220
		private string id3_ServiceDescription;

		// Token: 0x040004C5 RID: 1221
		private string id38_SoapFaultBinding;

		// Token: 0x040004C6 RID: 1222
		private string id123_ref;

		// Token: 0x040004C7 RID: 1223
		private string id198_XmlSchemaComplexContent;

		// Token: 0x040004C8 RID: 1224
		private string id53_parts;

		// Token: 0x040004C9 RID: 1225
		private string id35_use;

		// Token: 0x040004CA RID: 1226
		private string id157_XmlSchemaLengthFacet;

		// Token: 0x040004CB RID: 1227
		private string id207_XmlSchemaImport;

		// Token: 0x040004CC RID: 1228
		private string id44_text;

		// Token: 0x040004CD RID: 1229
		private string id117_XmlSchemaAppInfo;

		// Token: 0x040004CE RID: 1230
		private string id203_public;

		// Token: 0x040004CF RID: 1231
		private string id69_urlEncoded;

		// Token: 0x040004D0 RID: 1232
		private string id7_documentation;

		// Token: 0x040004D1 RID: 1233
		private string id19_Item;

		// Token: 0x040004D2 RID: 1234
		private string id129_final;

		// Token: 0x040004D3 RID: 1235
		private string id163_XmlSchemaElement;

		// Token: 0x040004D4 RID: 1236
		private string id60_capture;

		// Token: 0x040004D5 RID: 1237
		private string id37_encodingStyle;

		// Token: 0x040004D6 RID: 1238
		private string id185_sequence;

		// Token: 0x040004D7 RID: 1239
		private string id166_abstract;

		// Token: 0x040004D8 RID: 1240
		private string id23_location;

		// Token: 0x040004D9 RID: 1241
		private string id111_XmlSchemaAttributeGroup;

		// Token: 0x040004DA RID: 1242
		private string id192_XmlSchemaSequence;

		// Token: 0x040004DB RID: 1243
		private string id33_FaultBinding;

		// Token: 0x040004DC RID: 1244
		private string id153_XmlSchemaMaxInclusiveFacet;

		// Token: 0x040004DD RID: 1245
		private string id201_XmlSchemaGroup;

		// Token: 0x040004DE RID: 1246
		private string id43_multipartRelated;

		// Token: 0x040004DF RID: 1247
		private string id168_nillable;

		// Token: 0x040004E0 RID: 1248
		private string id149_value;

		// Token: 0x040004E1 RID: 1249
		private string id64_MimeMultipartRelatedBinding;

		// Token: 0x040004E2 RID: 1250
		private string id193_XmlSchemaAny;

		// Token: 0x040004E3 RID: 1251
		private string id191_XmlSchemaGroupRef;

		// Token: 0x040004E4 RID: 1252
		private string id74_soapAction;

		// Token: 0x040004E5 RID: 1253
		private string id63_ignoreCase;

		// Token: 0x040004E6 RID: 1254
		private string id101_version;

		// Token: 0x040004E7 RID: 1255
		private string id47_header;

		// Token: 0x040004E8 RID: 1256
		private string id195_extension;

		// Token: 0x040004E9 RID: 1257
		private string id48_Soap12HeaderBinding;

		// Token: 0x040004EA RID: 1258
		private string id134_memberTypes;

		// Token: 0x040004EB RID: 1259
		private string id121_Item;

		// Token: 0x040004EC RID: 1260
		private string id146_minExclusive;

		// Token: 0x040004ED RID: 1261
		private string id84_PortType;

		// Token: 0x040004EE RID: 1262
		private string id42_mimeXml;

		// Token: 0x040004EF RID: 1263
		private string id138_minInclusive;

		// Token: 0x040004F0 RID: 1264
		private string id118_source;

		// Token: 0x040004F1 RID: 1265
		private string id73_Soap12OperationBinding;

		// Token: 0x040004F2 RID: 1266
		private string id131_restriction;

		// Token: 0x040004F3 RID: 1267
		private string id152_XmlSchemaMaxExclusiveFacet;

		// Token: 0x040004F4 RID: 1268
		private string id135_XmlSchemaSimpleTypeRestriction;

		// Token: 0x040004F5 RID: 1269
		private string id188_XmlSchemaAll;

		// Token: 0x040004F6 RID: 1270
		private string id116_appinfo;

		// Token: 0x040004F7 RID: 1271
		private string id86_parameterOrder;

		// Token: 0x040004F8 RID: 1272
		private string id147_minLength;

		// Token: 0x040004F9 RID: 1273
		private string id78_HttpOperationBinding;

		// Token: 0x040004FA RID: 1274
		private string id161_XmlSchemaSimpleTypeList;

		// Token: 0x040004FB RID: 1275
		private string id205_XmlSchemaRedefine;

		// Token: 0x040004FC RID: 1276
		private string id194_XmlSchemaSimpleContent;

		// Token: 0x040004FD RID: 1277
		private string id91_MessagePart;

		// Token: 0x040004FE RID: 1278
		private string id92_element;

		// Token: 0x040004FF RID: 1279
		private string id114_processContents;

		// Token: 0x04000500 RID: 1280
		private string id18_Item;

		// Token: 0x04000501 RID: 1281
		private string id50_headerfault;

		// Token: 0x04000502 RID: 1282
		private string id154_XmlSchemaEnumerationFacet;

		// Token: 0x04000503 RID: 1283
		private string id96_XmlSchema;

		// Token: 0x04000504 RID: 1284
		private string id127_form;

		// Token: 0x04000505 RID: 1285
		private string id176_field;

		// Token: 0x04000506 RID: 1286
		private string id49_part;

		// Token: 0x04000507 RID: 1287
		private string id5_Item;

		// Token: 0x04000508 RID: 1288
		private string id57_match;

		// Token: 0x04000509 RID: 1289
		private string id52_Soap12BodyBinding;

		// Token: 0x0400050A RID: 1290
		private string id104_redefine;

		// Token: 0x0400050B RID: 1291
		private string id20_Item;

		// Token: 0x0400050C RID: 1292
		private string id21_Soap12AddressBinding;

		// Token: 0x0400050D RID: 1293
		private string id142_enumeration;

		// Token: 0x0400050E RID: 1294
		private string id24_SoapAddressBinding;

		// Token: 0x0400050F RID: 1295
		private string id103_include;

		// Token: 0x04000510 RID: 1296
		private string id139_maxLength;

		// Token: 0x04000511 RID: 1297
		private string id165_maxOccurs;

		// Token: 0x04000512 RID: 1298
		private string id65_MimePart;

		// Token: 0x04000513 RID: 1299
		private string id102_id;

		// Token: 0x04000514 RID: 1300
		private string id196_Item;

		// Token: 0x04000515 RID: 1301
		private string id140_length;

		// Token: 0x04000516 RID: 1302
		private string id27_type;

		// Token: 0x04000517 RID: 1303
		private string id106_complexType;

		// Token: 0x04000518 RID: 1304
		private string id31_output;

		// Token: 0x04000519 RID: 1305
		private string id1_definitions;

		// Token: 0x0400051A RID: 1306
		private string id4_name;

		// Token: 0x0400051B RID: 1307
		private string id132_union;

		// Token: 0x0400051C RID: 1308
		private string id29_OperationBinding;

		// Token: 0x0400051D RID: 1309
		private string id170_key;

		// Token: 0x0400051E RID: 1310
		private string id45_Item;

		// Token: 0x0400051F RID: 1311
		private string id95_Item;

		// Token: 0x04000520 RID: 1312
		private string id169_substitutionGroup;

		// Token: 0x04000521 RID: 1313
		private string id178_xpath;

		// Token: 0x04000522 RID: 1314
		private string id9_types;

		// Token: 0x04000523 RID: 1315
		private string id97_attributeFormDefault;

		// Token: 0x04000524 RID: 1316
		private string id62_pattern;

		// Token: 0x04000525 RID: 1317
		private string id58_MimeTextMatch;

		// Token: 0x04000526 RID: 1318
		private string id180_XmlSchemaKey;

		// Token: 0x04000527 RID: 1319
		private string id10_message;

		// Token: 0x04000528 RID: 1320
		private string id8_import;

		// Token: 0x04000529 RID: 1321
		private string id148_XmlSchemaMinLengthFacet;

		// Token: 0x0400052A RID: 1322
		private string id105_simpleType;

		// Token: 0x0400052B RID: 1323
		private string id181_XmlSchemaComplexType;

		// Token: 0x0400052C RID: 1324
		private string id164_minOccurs;

		// Token: 0x0400052D RID: 1325
		private string id144_maxExclusive;

		// Token: 0x0400052E RID: 1326
		private string id160_XmlSchemaFractionDigitsFacet;

		// Token: 0x0400052F RID: 1327
		private string id124_XmlSchemaAttribute;

		// Token: 0x04000530 RID: 1328
		private string id209_Import;

		// Token: 0x04000531 RID: 1329
		private string id206_schemaLocation;

		// Token: 0x04000532 RID: 1330
		private string id179_XmlSchemaUnique;

		// Token: 0x04000533 RID: 1331
		private string id75_style;

		// Token: 0x04000534 RID: 1332
		private string id119_XmlSchemaDocumentation;

		// Token: 0x04000535 RID: 1333
		private string id136_base;

		// Token: 0x04000536 RID: 1334
		private string id66_MimeXmlBinding;

		// Token: 0x04000537 RID: 1335
		private string id30_input;

		// Token: 0x04000538 RID: 1336
		private string id40_content;

		// Token: 0x04000539 RID: 1337
		private string id93_Types;

		// Token: 0x0400053A RID: 1338
		private string id94_schema;

		// Token: 0x0400053B RID: 1339
		private string id200_Item;

		// Token: 0x0400053C RID: 1340
		private string id67_MimeContentBinding;

		// Token: 0x0400053D RID: 1341
		private string id59_group;

		// Token: 0x0400053E RID: 1342
		private string id32_fault;

		// Token: 0x0400053F RID: 1343
		private string id80_transport;

		// Token: 0x04000540 RID: 1344
		private string id98_blockDefault;

		// Token: 0x04000541 RID: 1345
		private string id13_service;

		// Token: 0x04000542 RID: 1346
		private string id54_SoapHeaderBinding;

		// Token: 0x04000543 RID: 1347
		private string id204_system;

		// Token: 0x04000544 RID: 1348
		private string id16_Port;

		// Token: 0x04000545 RID: 1349
		private string id108_notation;

		// Token: 0x04000546 RID: 1350
		private string id186_choice;

		// Token: 0x04000547 RID: 1351
		private string id110_attributeGroup;

		// Token: 0x04000548 RID: 1352
		private string id79_Soap12Binding;

		// Token: 0x04000549 RID: 1353
		private string id77_SoapOperationBinding;

		// Token: 0x0400054A RID: 1354
		private string id115_XmlSchemaAnnotation;

		// Token: 0x0400054B RID: 1355
		private string id83_verb;

		// Token: 0x0400054C RID: 1356
		private string id72_HttpUrlEncodedBinding;

		// Token: 0x0400054D RID: 1357
		private string id39_OutputBinding;

		// Token: 0x0400054E RID: 1358
		private string id183_complexContent;

		// Token: 0x0400054F RID: 1359
		private string id202_XmlSchemaNotation;

		// Token: 0x04000550 RID: 1360
		private string id81_SoapBinding;

		// Token: 0x04000551 RID: 1361
		private string id199_Item;

		// Token: 0x04000552 RID: 1362
		private string id28_operation;

		// Token: 0x04000553 RID: 1363
		private string id122_XmlSchemaAttributeGroupRef;

		// Token: 0x04000554 RID: 1364
		private string id155_XmlSchemaPatternFacet;

		// Token: 0x04000555 RID: 1365
		private string id76_soapActionRequired;

		// Token: 0x04000556 RID: 1366
		private string id90_Message;

		// Token: 0x04000557 RID: 1367
		private string id159_XmlSchemaMinInclusiveFacet;

		// Token: 0x04000558 RID: 1368
		private string id208_XmlSchemaInclude;

		// Token: 0x04000559 RID: 1369
		private string id85_Operation;

		// Token: 0x0400055A RID: 1370
		private string id130_list;

		// Token: 0x0400055B RID: 1371
		private string id14_Service;

		// Token: 0x0400055C RID: 1372
		private string id22_required;

		// Token: 0x0400055D RID: 1373
		private string id174_refer;

		// Token: 0x0400055E RID: 1374
		private string id71_HttpUrlReplacementBinding;

		// Token: 0x0400055F RID: 1375
		private string id56_MimeTextBinding;

		// Token: 0x04000560 RID: 1376
		private string id87_OperationFault;

		// Token: 0x04000561 RID: 1377
		private string id125_default;

		// Token: 0x04000562 RID: 1378
		private string id15_port;

		// Token: 0x04000563 RID: 1379
		private string id51_SoapHeaderFaultBinding;

		// Token: 0x04000564 RID: 1380
		private string id128_XmlSchemaSimpleType;

		// Token: 0x04000565 RID: 1381
		private string id36_namespace;

		// Token: 0x04000566 RID: 1382
		private string id175_selector;

		// Token: 0x04000567 RID: 1383
		private string id150_XmlSchemaMinExclusiveFacet;

		// Token: 0x04000568 RID: 1384
		private string id100_elementFormDefault;

		// Token: 0x04000569 RID: 1385
		private string id26_Binding;

		// Token: 0x0400056A RID: 1386
		private string id197_Item;

		// Token: 0x0400056B RID: 1387
		private string id126_fixed;

		// Token: 0x0400056C RID: 1388
		private string id107_annotation;

		// Token: 0x0400056D RID: 1389
		private string id99_finalDefault;

		// Token: 0x0400056E RID: 1390
		private string id137_fractionDigits;

		// Token: 0x0400056F RID: 1391
		private string id70_urlReplacement;

		// Token: 0x04000570 RID: 1392
		private string id189_XmlSchemaChoice;

		// Token: 0x04000571 RID: 1393
		private string id2_Item;

		// Token: 0x04000572 RID: 1394
		private string id112_anyAttribute;

		// Token: 0x04000573 RID: 1395
		private string id89_OperationInput;

		// Token: 0x04000574 RID: 1396
		private string id141_totalDigits;

		// Token: 0x04000575 RID: 1397
		private string id61_repeats;

		// Token: 0x04000576 RID: 1398
		private string id184_simpleContent;

		// Token: 0x04000577 RID: 1399
		private string id55_SoapBodyBinding;

		// Token: 0x04000578 RID: 1400
		private string id145_whiteSpace;

		// Token: 0x04000579 RID: 1401
		private string id167_block;

		// Token: 0x0400057A RID: 1402
		private string id151_XmlSchemaWhiteSpaceFacet;

		// Token: 0x0400057B RID: 1403
		private string id12_binding;

		// Token: 0x0400057C RID: 1404
		private string id109_attribute;

		// Token: 0x0400057D RID: 1405
		private string id171_unique;

		// Token: 0x0400057E RID: 1406
		private string id120_lang;

		// Token: 0x0400057F RID: 1407
		private string id173_XmlSchemaKeyref;

		// Token: 0x04000580 RID: 1408
		private string id177_XmlSchemaXPath;

		// Token: 0x04000581 RID: 1409
		private string id34_Soap12FaultBinding;

		// Token: 0x04000582 RID: 1410
		private string id41_Item;

		// Token: 0x04000583 RID: 1411
		private string id156_XmlSchemaTotalDigitsFacet;

		// Token: 0x04000584 RID: 1412
		private string id113_XmlSchemaAnyAttribute;
	}
}
