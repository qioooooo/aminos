using System;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x02000108 RID: 264
	internal class ServiceDescriptionSerializationWriter : XmlSerializationWriter
	{
		// Token: 0x0600075F RID: 1887 RVA: 0x0002063B File Offset: 0x0001F63B
		public void Write125_definitions(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("definitions", "http://schemas.xmlsoap.org/wsdl/");
				return;
			}
			base.TopLevelElement();
			this.Write124_ServiceDescription("definitions", "http://schemas.xmlsoap.org/wsdl/", (ServiceDescription)o, true, false);
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x00020678 File Offset: 0x0001F678
		private void Write124_ServiceDescription(string n, string ns, ServiceDescription o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(ServiceDescription))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("ServiceDescription", "http://schemas.xmlsoap.org/wsdl/");
			}
			XmlAttribute[] extensibleAttributes = o.ExtensibleAttributes;
			if (extensibleAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in extensibleAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("name", "", o.Name);
			base.WriteAttribute("targetNamespace", "", o.TargetNamespace);
			if (o.DocumentationElement != null || o.DocumentationElement == null)
			{
				base.WriteElementLiteral(o.DocumentationElement, "documentation", "http://schemas.xmlsoap.org/wsdl/", false, true);
				ServiceDescriptionFormatExtensionCollection extensions = o.Extensions;
				if (extensions != null)
				{
					for (int j = 0; j < ((ICollection)extensions).Count; j++)
					{
						if (!(extensions[j] is XmlNode) && extensions[j] != null)
						{
							throw base.CreateInvalidAnyTypeException(extensions[j]);
						}
						base.WriteElementLiteral((XmlNode)extensions[j], "", null, false, true);
					}
				}
				ImportCollection imports = o.Imports;
				if (imports != null)
				{
					for (int k = 0; k < ((ICollection)imports).Count; k++)
					{
						this.Write4_Import("import", "http://schemas.xmlsoap.org/wsdl/", imports[k], false, false);
					}
				}
				this.Write67_Types("types", "http://schemas.xmlsoap.org/wsdl/", o.Types, false, false);
				MessageCollection messages = o.Messages;
				if (messages != null)
				{
					for (int l = 0; l < ((ICollection)messages).Count; l++)
					{
						this.Write69_Message("message", "http://schemas.xmlsoap.org/wsdl/", messages[l], false, false);
					}
				}
				PortTypeCollection portTypes = o.PortTypes;
				if (portTypes != null)
				{
					for (int m = 0; m < ((ICollection)portTypes).Count; m++)
					{
						this.Write75_PortType("portType", "http://schemas.xmlsoap.org/wsdl/", portTypes[m], false, false);
					}
				}
				BindingCollection bindings = o.Bindings;
				if (bindings != null)
				{
					for (int num = 0; num < ((ICollection)bindings).Count; num++)
					{
						this.Write117_Binding("binding", "http://schemas.xmlsoap.org/wsdl/", bindings[num], false, false);
					}
				}
				ServiceCollection services = o.Services;
				if (services != null)
				{
					for (int num2 = 0; num2 < ((ICollection)services).Count; num2++)
					{
						this.Write123_Service("service", "http://schemas.xmlsoap.org/wsdl/", services[num2], false, false);
					}
				}
				base.WriteEndElement(o);
				return;
			}
			throw base.CreateInvalidAnyTypeException(o.DocumentationElement);
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x0002091C File Offset: 0x0001F91C
		private void Write123_Service(string n, string ns, Service o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(Service))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("Service", "http://schemas.xmlsoap.org/wsdl/");
			}
			XmlAttribute[] extensibleAttributes = o.ExtensibleAttributes;
			if (extensibleAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in extensibleAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("name", "", o.Name);
			if (o.DocumentationElement != null || o.DocumentationElement == null)
			{
				base.WriteElementLiteral(o.DocumentationElement, "documentation", "http://schemas.xmlsoap.org/wsdl/", false, true);
				ServiceDescriptionFormatExtensionCollection extensions = o.Extensions;
				if (extensions != null)
				{
					for (int j = 0; j < ((ICollection)extensions).Count; j++)
					{
						if (!(extensions[j] is XmlNode) && extensions[j] != null)
						{
							throw base.CreateInvalidAnyTypeException(extensions[j]);
						}
						base.WriteElementLiteral((XmlNode)extensions[j], "", null, false, true);
					}
				}
				PortCollection ports = o.Ports;
				if (ports != null)
				{
					for (int k = 0; k < ((ICollection)ports).Count; k++)
					{
						this.Write122_Port("port", "http://schemas.xmlsoap.org/wsdl/", ports[k], false, false);
					}
				}
				base.WriteEndElement(o);
				return;
			}
			throw base.CreateInvalidAnyTypeException(o.DocumentationElement);
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x00020AA0 File Offset: 0x0001FAA0
		private void Write122_Port(string n, string ns, Port o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(Port))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("Port", "http://schemas.xmlsoap.org/wsdl/");
			}
			XmlAttribute[] extensibleAttributes = o.ExtensibleAttributes;
			if (extensibleAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in extensibleAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("name", "", o.Name);
			base.WriteAttribute("binding", "", base.FromXmlQualifiedName(o.Binding));
			if (o.DocumentationElement != null || o.DocumentationElement == null)
			{
				base.WriteElementLiteral(o.DocumentationElement, "documentation", "http://schemas.xmlsoap.org/wsdl/", false, true);
				ServiceDescriptionFormatExtensionCollection extensions = o.Extensions;
				if (extensions != null)
				{
					for (int j = 0; j < ((ICollection)extensions).Count; j++)
					{
						object obj = extensions[j];
						if (obj is Soap12AddressBinding)
						{
							this.Write121_Soap12AddressBinding("address", "http://schemas.xmlsoap.org/wsdl/soap12/", (Soap12AddressBinding)obj, false, false);
						}
						else if (obj is HttpAddressBinding)
						{
							this.Write118_HttpAddressBinding("address", "http://schemas.xmlsoap.org/wsdl/http/", (HttpAddressBinding)obj, false, false);
						}
						else if (obj is SoapAddressBinding)
						{
							this.Write119_SoapAddressBinding("address", "http://schemas.xmlsoap.org/wsdl/soap/", (SoapAddressBinding)obj, false, false);
						}
						else if (obj is XmlElement)
						{
							XmlElement xmlElement = (XmlElement)obj;
							if (xmlElement == null && xmlElement != null)
							{
								throw base.CreateInvalidAnyTypeException(xmlElement);
							}
							base.WriteElementLiteral(xmlElement, "", null, false, true);
						}
						else if (obj != null)
						{
							throw base.CreateUnknownTypeException(obj);
						}
					}
				}
				base.WriteEndElement(o);
				return;
			}
			throw base.CreateInvalidAnyTypeException(o.DocumentationElement);
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x00020C7C File Offset: 0x0001FC7C
		private void Write119_SoapAddressBinding(string n, string ns, SoapAddressBinding o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(SoapAddressBinding))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("SoapAddressBinding", "http://schemas.xmlsoap.org/wsdl/soap/");
			}
			if (o.Required)
			{
				base.WriteAttribute("required", "http://schemas.xmlsoap.org/wsdl/", XmlConvert.ToString(o.Required));
			}
			base.WriteAttribute("location", "", o.Location);
			base.WriteEndElement(o);
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x00020D18 File Offset: 0x0001FD18
		private void Write118_HttpAddressBinding(string n, string ns, HttpAddressBinding o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(HttpAddressBinding))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("HttpAddressBinding", "http://schemas.xmlsoap.org/wsdl/http/");
			}
			if (o.Required)
			{
				base.WriteAttribute("required", "http://schemas.xmlsoap.org/wsdl/", XmlConvert.ToString(o.Required));
			}
			base.WriteAttribute("location", "", o.Location);
			base.WriteEndElement(o);
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x00020DB4 File Offset: 0x0001FDB4
		private void Write121_Soap12AddressBinding(string n, string ns, Soap12AddressBinding o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(Soap12AddressBinding))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("Soap12AddressBinding", "http://schemas.xmlsoap.org/wsdl/soap12/");
			}
			if (o.Required)
			{
				base.WriteAttribute("required", "http://schemas.xmlsoap.org/wsdl/", XmlConvert.ToString(o.Required));
			}
			base.WriteAttribute("location", "", o.Location);
			base.WriteEndElement(o);
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x00020E50 File Offset: 0x0001FE50
		private void Write117_Binding(string n, string ns, Binding o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(Binding))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("Binding", "http://schemas.xmlsoap.org/wsdl/");
			}
			XmlAttribute[] extensibleAttributes = o.ExtensibleAttributes;
			if (extensibleAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in extensibleAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("name", "", o.Name);
			base.WriteAttribute("type", "", base.FromXmlQualifiedName(o.Type));
			if (o.DocumentationElement != null || o.DocumentationElement == null)
			{
				base.WriteElementLiteral(o.DocumentationElement, "documentation", "http://schemas.xmlsoap.org/wsdl/", false, true);
				ServiceDescriptionFormatExtensionCollection extensions = o.Extensions;
				if (extensions != null)
				{
					for (int j = 0; j < ((ICollection)extensions).Count; j++)
					{
						object obj = extensions[j];
						if (obj is Soap12Binding)
						{
							this.Write84_Soap12Binding("binding", "http://schemas.xmlsoap.org/wsdl/soap12/", (Soap12Binding)obj, false, false);
						}
						else if (obj is HttpBinding)
						{
							this.Write77_HttpBinding("binding", "http://schemas.xmlsoap.org/wsdl/http/", (HttpBinding)obj, false, false);
						}
						else if (obj is SoapBinding)
						{
							this.Write80_SoapBinding("binding", "http://schemas.xmlsoap.org/wsdl/soap/", (SoapBinding)obj, false, false);
						}
						else if (obj is XmlElement)
						{
							XmlElement xmlElement = (XmlElement)obj;
							if (xmlElement == null && xmlElement != null)
							{
								throw base.CreateInvalidAnyTypeException(xmlElement);
							}
							base.WriteElementLiteral(xmlElement, "", null, false, true);
						}
						else if (obj != null)
						{
							throw base.CreateUnknownTypeException(obj);
						}
					}
				}
				OperationBindingCollection operations = o.Operations;
				if (operations != null)
				{
					for (int k = 0; k < ((ICollection)operations).Count; k++)
					{
						this.Write116_OperationBinding("operation", "http://schemas.xmlsoap.org/wsdl/", operations[k], false, false);
					}
				}
				base.WriteEndElement(o);
				return;
			}
			throw base.CreateInvalidAnyTypeException(o.DocumentationElement);
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x0002106C File Offset: 0x0002006C
		private void Write116_OperationBinding(string n, string ns, OperationBinding o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(OperationBinding))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("OperationBinding", "http://schemas.xmlsoap.org/wsdl/");
			}
			XmlAttribute[] extensibleAttributes = o.ExtensibleAttributes;
			if (extensibleAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in extensibleAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("name", "", o.Name);
			if (o.DocumentationElement != null || o.DocumentationElement == null)
			{
				base.WriteElementLiteral(o.DocumentationElement, "documentation", "http://schemas.xmlsoap.org/wsdl/", false, true);
				ServiceDescriptionFormatExtensionCollection extensions = o.Extensions;
				if (extensions != null)
				{
					for (int j = 0; j < ((ICollection)extensions).Count; j++)
					{
						object obj = extensions[j];
						if (obj is Soap12OperationBinding)
						{
							this.Write88_Soap12OperationBinding("operation", "http://schemas.xmlsoap.org/wsdl/soap12/", (Soap12OperationBinding)obj, false, false);
						}
						else if (obj is HttpOperationBinding)
						{
							this.Write85_HttpOperationBinding("operation", "http://schemas.xmlsoap.org/wsdl/http/", (HttpOperationBinding)obj, false, false);
						}
						else if (obj is SoapOperationBinding)
						{
							this.Write86_SoapOperationBinding("operation", "http://schemas.xmlsoap.org/wsdl/soap/", (SoapOperationBinding)obj, false, false);
						}
						else if (obj is XmlElement)
						{
							XmlElement xmlElement = (XmlElement)obj;
							if (xmlElement == null && xmlElement != null)
							{
								throw base.CreateInvalidAnyTypeException(xmlElement);
							}
							base.WriteElementLiteral(xmlElement, "", null, false, true);
						}
						else if (obj != null)
						{
							throw base.CreateUnknownTypeException(obj);
						}
					}
				}
				this.Write110_InputBinding("input", "http://schemas.xmlsoap.org/wsdl/", o.Input, false, false);
				this.Write111_OutputBinding("output", "http://schemas.xmlsoap.org/wsdl/", o.Output, false, false);
				FaultBindingCollection faults = o.Faults;
				if (faults != null)
				{
					for (int k = 0; k < ((ICollection)faults).Count; k++)
					{
						this.Write115_FaultBinding("fault", "http://schemas.xmlsoap.org/wsdl/", faults[k], false, false);
					}
				}
				base.WriteEndElement(o);
				return;
			}
			throw base.CreateInvalidAnyTypeException(o.DocumentationElement);
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x0002129C File Offset: 0x0002029C
		private void Write115_FaultBinding(string n, string ns, FaultBinding o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(FaultBinding))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("FaultBinding", "http://schemas.xmlsoap.org/wsdl/");
			}
			XmlAttribute[] extensibleAttributes = o.ExtensibleAttributes;
			if (extensibleAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in extensibleAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("name", "", o.Name);
			if (o.DocumentationElement != null || o.DocumentationElement == null)
			{
				base.WriteElementLiteral(o.DocumentationElement, "documentation", "http://schemas.xmlsoap.org/wsdl/", false, true);
				ServiceDescriptionFormatExtensionCollection extensions = o.Extensions;
				if (extensions != null)
				{
					for (int j = 0; j < ((ICollection)extensions).Count; j++)
					{
						object obj = extensions[j];
						if (obj is Soap12FaultBinding)
						{
							this.Write114_Soap12FaultBinding("fault", "http://schemas.xmlsoap.org/wsdl/soap12/", (Soap12FaultBinding)obj, false, false);
						}
						else if (obj is SoapFaultBinding)
						{
							this.Write112_SoapFaultBinding("fault", "http://schemas.xmlsoap.org/wsdl/soap/", (SoapFaultBinding)obj, false, false);
						}
						else if (obj is XmlElement)
						{
							XmlElement xmlElement = (XmlElement)obj;
							if (xmlElement == null && xmlElement != null)
							{
								throw base.CreateInvalidAnyTypeException(xmlElement);
							}
							base.WriteElementLiteral(xmlElement, "", null, false, true);
						}
						else if (obj != null)
						{
							throw base.CreateUnknownTypeException(obj);
						}
					}
				}
				base.WriteEndElement(o);
				return;
			}
			throw base.CreateInvalidAnyTypeException(o.DocumentationElement);
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x00021438 File Offset: 0x00020438
		private void Write112_SoapFaultBinding(string n, string ns, SoapFaultBinding o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(SoapFaultBinding))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("SoapFaultBinding", "http://schemas.xmlsoap.org/wsdl/soap/");
			}
			if (o.Required)
			{
				base.WriteAttribute("required", "http://schemas.xmlsoap.org/wsdl/", XmlConvert.ToString(o.Required));
			}
			if (o.Use != SoapBindingUse.Default)
			{
				base.WriteAttribute("use", "", this.Write98_SoapBindingUse(o.Use));
			}
			base.WriteAttribute("name", "", o.Name);
			base.WriteAttribute("namespace", "", o.Namespace);
			if (o.Encoding != null && o.Encoding.Length != 0)
			{
				base.WriteAttribute("encodingStyle", "", o.Encoding);
			}
			base.WriteEndElement(o);
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0002153C File Offset: 0x0002053C
		private string Write98_SoapBindingUse(SoapBindingUse v)
		{
			string text;
			switch (v)
			{
			case SoapBindingUse.Encoded:
				text = "encoded";
				break;
			case SoapBindingUse.Literal:
				text = "literal";
				break;
			default:
				throw base.CreateInvalidEnumValueException(((long)v).ToString(CultureInfo.InvariantCulture), "System.Web.Services.Description.SoapBindingUse");
			}
			return text;
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x0002158C File Offset: 0x0002058C
		private void Write114_Soap12FaultBinding(string n, string ns, Soap12FaultBinding o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(Soap12FaultBinding))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("Soap12FaultBinding", "http://schemas.xmlsoap.org/wsdl/soap12/");
			}
			if (o.Required)
			{
				base.WriteAttribute("required", "http://schemas.xmlsoap.org/wsdl/", XmlConvert.ToString(o.Required));
			}
			if (o.Use != SoapBindingUse.Default)
			{
				base.WriteAttribute("use", "", this.Write100_SoapBindingUse(o.Use));
			}
			base.WriteAttribute("name", "", o.Name);
			base.WriteAttribute("namespace", "", o.Namespace);
			if (o.Encoding != null && o.Encoding.Length != 0)
			{
				base.WriteAttribute("encodingStyle", "", o.Encoding);
			}
			base.WriteEndElement(o);
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x00021690 File Offset: 0x00020690
		private string Write100_SoapBindingUse(SoapBindingUse v)
		{
			string text;
			switch (v)
			{
			case SoapBindingUse.Encoded:
				text = "encoded";
				break;
			case SoapBindingUse.Literal:
				text = "literal";
				break;
			default:
				throw base.CreateInvalidEnumValueException(((long)v).ToString(CultureInfo.InvariantCulture), "System.Web.Services.Description.SoapBindingUse");
			}
			return text;
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x000216E0 File Offset: 0x000206E0
		private void Write111_OutputBinding(string n, string ns, OutputBinding o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(OutputBinding))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("OutputBinding", "http://schemas.xmlsoap.org/wsdl/");
			}
			XmlAttribute[] extensibleAttributes = o.ExtensibleAttributes;
			if (extensibleAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in extensibleAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("name", "", o.Name);
			if (o.DocumentationElement != null || o.DocumentationElement == null)
			{
				base.WriteElementLiteral(o.DocumentationElement, "documentation", "http://schemas.xmlsoap.org/wsdl/", false, true);
				ServiceDescriptionFormatExtensionCollection extensions = o.Extensions;
				if (extensions != null)
				{
					for (int j = 0; j < ((ICollection)extensions).Count; j++)
					{
						object obj = extensions[j];
						if (obj is Soap12BodyBinding)
						{
							this.Write102_Soap12BodyBinding("body", "http://schemas.xmlsoap.org/wsdl/soap12/", (Soap12BodyBinding)obj, false, false);
						}
						else if (obj is Soap12HeaderBinding)
						{
							this.Write109_Soap12HeaderBinding("header", "http://schemas.xmlsoap.org/wsdl/soap12/", (Soap12HeaderBinding)obj, false, false);
						}
						else if (obj is SoapHeaderBinding)
						{
							this.Write106_SoapHeaderBinding("header", "http://schemas.xmlsoap.org/wsdl/soap/", (SoapHeaderBinding)obj, false, false);
						}
						else if (obj is SoapBodyBinding)
						{
							this.Write99_SoapBodyBinding("body", "http://schemas.xmlsoap.org/wsdl/soap/", (SoapBodyBinding)obj, false, false);
						}
						else if (obj is MimeXmlBinding)
						{
							this.Write94_MimeXmlBinding("mimeXml", "http://schemas.xmlsoap.org/wsdl/mime/", (MimeXmlBinding)obj, false, false);
						}
						else if (obj is MimeContentBinding)
						{
							this.Write93_MimeContentBinding("content", "http://schemas.xmlsoap.org/wsdl/mime/", (MimeContentBinding)obj, false, false);
						}
						else if (obj is MimeTextBinding)
						{
							this.Write97_MimeTextBinding("text", "http://microsoft.com/wsdl/mime/textMatching/", (MimeTextBinding)obj, false, false);
						}
						else if (obj is MimeMultipartRelatedBinding)
						{
							this.Write104_MimeMultipartRelatedBinding("multipartRelated", "http://schemas.xmlsoap.org/wsdl/mime/", (MimeMultipartRelatedBinding)obj, false, false);
						}
						else if (obj is XmlElement)
						{
							XmlElement xmlElement = (XmlElement)obj;
							if (xmlElement == null && xmlElement != null)
							{
								throw base.CreateInvalidAnyTypeException(xmlElement);
							}
							base.WriteElementLiteral(xmlElement, "", null, false, true);
						}
						else if (obj != null)
						{
							throw base.CreateUnknownTypeException(obj);
						}
					}
				}
				base.WriteEndElement(o);
				return;
			}
			throw base.CreateInvalidAnyTypeException(o.DocumentationElement);
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x00021964 File Offset: 0x00020964
		private void Write104_MimeMultipartRelatedBinding(string n, string ns, MimeMultipartRelatedBinding o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(MimeMultipartRelatedBinding))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("MimeMultipartRelatedBinding", "http://schemas.xmlsoap.org/wsdl/mime/");
			}
			if (o.Required)
			{
				base.WriteAttribute("required", "http://schemas.xmlsoap.org/wsdl/", XmlConvert.ToString(o.Required));
			}
			MimePartCollection parts = o.Parts;
			if (parts != null)
			{
				for (int i = 0; i < ((ICollection)parts).Count; i++)
				{
					this.Write103_MimePart("part", "http://schemas.xmlsoap.org/wsdl/mime/", parts[i], false, false);
				}
			}
			base.WriteEndElement(o);
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x00021A20 File Offset: 0x00020A20
		private void Write103_MimePart(string n, string ns, MimePart o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(MimePart))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("MimePart", "http://schemas.xmlsoap.org/wsdl/mime/");
			}
			if (o.Required)
			{
				base.WriteAttribute("required", "http://schemas.xmlsoap.org/wsdl/", XmlConvert.ToString(o.Required));
			}
			ServiceDescriptionFormatExtensionCollection extensions = o.Extensions;
			if (extensions != null)
			{
				for (int i = 0; i < ((ICollection)extensions).Count; i++)
				{
					object obj = extensions[i];
					if (obj is Soap12BodyBinding)
					{
						this.Write102_Soap12BodyBinding("body", "http://schemas.xmlsoap.org/wsdl/soap12/", (Soap12BodyBinding)obj, false, false);
					}
					else if (obj is SoapBodyBinding)
					{
						this.Write99_SoapBodyBinding("body", "http://schemas.xmlsoap.org/wsdl/soap/", (SoapBodyBinding)obj, false, false);
					}
					else if (obj is MimeContentBinding)
					{
						this.Write93_MimeContentBinding("content", "http://schemas.xmlsoap.org/wsdl/mime/", (MimeContentBinding)obj, false, false);
					}
					else if (obj is MimeXmlBinding)
					{
						this.Write94_MimeXmlBinding("mimeXml", "http://schemas.xmlsoap.org/wsdl/mime/", (MimeXmlBinding)obj, false, false);
					}
					else if (obj is MimeTextBinding)
					{
						this.Write97_MimeTextBinding("text", "http://microsoft.com/wsdl/mime/textMatching/", (MimeTextBinding)obj, false, false);
					}
					else if (obj is XmlElement)
					{
						XmlElement xmlElement = (XmlElement)obj;
						if (xmlElement == null && xmlElement != null)
						{
							throw base.CreateInvalidAnyTypeException(xmlElement);
						}
						base.WriteElementLiteral(xmlElement, "", null, false, true);
					}
					else if (obj != null)
					{
						throw base.CreateUnknownTypeException(obj);
					}
				}
			}
			base.WriteEndElement(o);
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x00021BC4 File Offset: 0x00020BC4
		private void Write97_MimeTextBinding(string n, string ns, MimeTextBinding o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(MimeTextBinding))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("MimeTextBinding", "http://microsoft.com/wsdl/mime/textMatching/");
			}
			if (o.Required)
			{
				base.WriteAttribute("required", "http://schemas.xmlsoap.org/wsdl/", XmlConvert.ToString(o.Required));
			}
			MimeTextMatchCollection matches = o.Matches;
			if (matches != null)
			{
				for (int i = 0; i < ((ICollection)matches).Count; i++)
				{
					this.Write96_MimeTextMatch("match", "http://microsoft.com/wsdl/mime/textMatching/", matches[i], false, false);
				}
			}
			base.WriteEndElement(o);
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x00021C80 File Offset: 0x00020C80
		private void Write96_MimeTextMatch(string n, string ns, MimeTextMatch o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(MimeTextMatch))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("MimeTextMatch", "http://microsoft.com/wsdl/mime/textMatching/");
			}
			base.WriteAttribute("name", "", o.Name);
			base.WriteAttribute("type", "", o.Type);
			if (o.Group != 1)
			{
				base.WriteAttribute("group", "", XmlConvert.ToString(o.Group));
			}
			if (o.Capture != 0)
			{
				base.WriteAttribute("capture", "", XmlConvert.ToString(o.Capture));
			}
			if (o.RepeatsString != "1")
			{
				base.WriteAttribute("repeats", "", o.RepeatsString);
			}
			base.WriteAttribute("pattern", "", o.Pattern);
			base.WriteAttribute("ignoreCase", "", XmlConvert.ToString(o.IgnoreCase));
			MimeTextMatchCollection matches = o.Matches;
			if (matches != null)
			{
				for (int i = 0; i < ((ICollection)matches).Count; i++)
				{
					this.Write96_MimeTextMatch("match", "http://microsoft.com/wsdl/mime/textMatching/", matches[i], false, false);
				}
			}
			base.WriteEndElement(o);
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x00021DE4 File Offset: 0x00020DE4
		private void Write94_MimeXmlBinding(string n, string ns, MimeXmlBinding o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(MimeXmlBinding))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("MimeXmlBinding", "http://schemas.xmlsoap.org/wsdl/mime/");
			}
			if (o.Required)
			{
				base.WriteAttribute("required", "http://schemas.xmlsoap.org/wsdl/", XmlConvert.ToString(o.Required));
			}
			base.WriteAttribute("part", "", o.Part);
			base.WriteEndElement(o);
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x00021E80 File Offset: 0x00020E80
		private void Write93_MimeContentBinding(string n, string ns, MimeContentBinding o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(MimeContentBinding))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("MimeContentBinding", "http://schemas.xmlsoap.org/wsdl/mime/");
			}
			if (o.Required)
			{
				base.WriteAttribute("required", "http://schemas.xmlsoap.org/wsdl/", XmlConvert.ToString(o.Required));
			}
			base.WriteAttribute("part", "", o.Part);
			base.WriteAttribute("type", "", o.Type);
			base.WriteEndElement(o);
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x00021F34 File Offset: 0x00020F34
		private void Write99_SoapBodyBinding(string n, string ns, SoapBodyBinding o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(SoapBodyBinding))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("SoapBodyBinding", "http://schemas.xmlsoap.org/wsdl/soap/");
			}
			if (o.Required)
			{
				base.WriteAttribute("required", "http://schemas.xmlsoap.org/wsdl/", XmlConvert.ToString(o.Required));
			}
			if (o.Use != SoapBindingUse.Default)
			{
				base.WriteAttribute("use", "", this.Write98_SoapBindingUse(o.Use));
			}
			if (o.Namespace != null && o.Namespace.Length != 0)
			{
				base.WriteAttribute("namespace", "", o.Namespace);
			}
			if (o.Encoding != null && o.Encoding.Length != 0)
			{
				base.WriteAttribute("encodingStyle", "", o.Encoding);
			}
			base.WriteAttribute("parts", "", o.PartsString);
			base.WriteEndElement(o);
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0002204C File Offset: 0x0002104C
		private void Write102_Soap12BodyBinding(string n, string ns, Soap12BodyBinding o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(Soap12BodyBinding))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("Soap12BodyBinding", "http://schemas.xmlsoap.org/wsdl/soap12/");
			}
			if (o.Required)
			{
				base.WriteAttribute("required", "http://schemas.xmlsoap.org/wsdl/", XmlConvert.ToString(o.Required));
			}
			if (o.Use != SoapBindingUse.Default)
			{
				base.WriteAttribute("use", "", this.Write100_SoapBindingUse(o.Use));
			}
			if (o.Namespace != null && o.Namespace.Length != 0)
			{
				base.WriteAttribute("namespace", "", o.Namespace);
			}
			if (o.Encoding != null && o.Encoding.Length != 0)
			{
				base.WriteAttribute("encodingStyle", "", o.Encoding);
			}
			base.WriteAttribute("parts", "", o.PartsString);
			base.WriteEndElement(o);
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x00022164 File Offset: 0x00021164
		private void Write106_SoapHeaderBinding(string n, string ns, SoapHeaderBinding o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(SoapHeaderBinding))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("SoapHeaderBinding", "http://schemas.xmlsoap.org/wsdl/soap/");
			}
			if (o.Required)
			{
				base.WriteAttribute("required", "http://schemas.xmlsoap.org/wsdl/", XmlConvert.ToString(o.Required));
			}
			base.WriteAttribute("message", "", base.FromXmlQualifiedName(o.Message));
			base.WriteAttribute("part", "", o.Part);
			if (o.Use != SoapBindingUse.Default)
			{
				base.WriteAttribute("use", "", this.Write98_SoapBindingUse(o.Use));
			}
			if (o.Encoding != null && o.Encoding.Length != 0)
			{
				base.WriteAttribute("encodingStyle", "", o.Encoding);
			}
			if (o.Namespace != null && o.Namespace.Length != 0)
			{
				base.WriteAttribute("namespace", "", o.Namespace);
			}
			this.Write105_SoapHeaderFaultBinding("headerfault", "http://schemas.xmlsoap.org/wsdl/soap/", o.Fault, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x000222B0 File Offset: 0x000212B0
		private void Write105_SoapHeaderFaultBinding(string n, string ns, SoapHeaderFaultBinding o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(SoapHeaderFaultBinding))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("SoapHeaderFaultBinding", "http://schemas.xmlsoap.org/wsdl/soap/");
			}
			if (o.Required)
			{
				base.WriteAttribute("required", "http://schemas.xmlsoap.org/wsdl/", XmlConvert.ToString(o.Required));
			}
			base.WriteAttribute("message", "", base.FromXmlQualifiedName(o.Message));
			base.WriteAttribute("part", "", o.Part);
			if (o.Use != SoapBindingUse.Default)
			{
				base.WriteAttribute("use", "", this.Write98_SoapBindingUse(o.Use));
			}
			if (o.Encoding != null && o.Encoding.Length != 0)
			{
				base.WriteAttribute("encodingStyle", "", o.Encoding);
			}
			if (o.Namespace != null && o.Namespace.Length != 0)
			{
				base.WriteAttribute("namespace", "", o.Namespace);
			}
			base.WriteEndElement(o);
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x000223E4 File Offset: 0x000213E4
		private void Write109_Soap12HeaderBinding(string n, string ns, Soap12HeaderBinding o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(Soap12HeaderBinding))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("Soap12HeaderBinding", "http://schemas.xmlsoap.org/wsdl/soap12/");
			}
			if (o.Required)
			{
				base.WriteAttribute("required", "http://schemas.xmlsoap.org/wsdl/", XmlConvert.ToString(o.Required));
			}
			base.WriteAttribute("message", "", base.FromXmlQualifiedName(o.Message));
			base.WriteAttribute("part", "", o.Part);
			if (o.Use != SoapBindingUse.Default)
			{
				base.WriteAttribute("use", "", this.Write100_SoapBindingUse(o.Use));
			}
			if (o.Encoding != null && o.Encoding.Length != 0)
			{
				base.WriteAttribute("encodingStyle", "", o.Encoding);
			}
			if (o.Namespace != null && o.Namespace.Length != 0)
			{
				base.WriteAttribute("namespace", "", o.Namespace);
			}
			this.Write107_SoapHeaderFaultBinding("headerfault", "http://schemas.xmlsoap.org/wsdl/soap12/", o.Fault, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x00022530 File Offset: 0x00021530
		private void Write107_SoapHeaderFaultBinding(string n, string ns, SoapHeaderFaultBinding o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(SoapHeaderFaultBinding))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("SoapHeaderFaultBinding", "http://schemas.xmlsoap.org/wsdl/soap12/");
			}
			if (o.Required)
			{
				base.WriteAttribute("required", "http://schemas.xmlsoap.org/wsdl/", XmlConvert.ToString(o.Required));
			}
			base.WriteAttribute("message", "", base.FromXmlQualifiedName(o.Message));
			base.WriteAttribute("part", "", o.Part);
			if (o.Use != SoapBindingUse.Default)
			{
				base.WriteAttribute("use", "", this.Write100_SoapBindingUse(o.Use));
			}
			if (o.Encoding != null && o.Encoding.Length != 0)
			{
				base.WriteAttribute("encodingStyle", "", o.Encoding);
			}
			if (o.Namespace != null && o.Namespace.Length != 0)
			{
				base.WriteAttribute("namespace", "", o.Namespace);
			}
			base.WriteEndElement(o);
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x00022664 File Offset: 0x00021664
		private void Write110_InputBinding(string n, string ns, InputBinding o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(InputBinding))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("InputBinding", "http://schemas.xmlsoap.org/wsdl/");
			}
			XmlAttribute[] extensibleAttributes = o.ExtensibleAttributes;
			if (extensibleAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in extensibleAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("name", "", o.Name);
			if (o.DocumentationElement != null || o.DocumentationElement == null)
			{
				base.WriteElementLiteral(o.DocumentationElement, "documentation", "http://schemas.xmlsoap.org/wsdl/", false, true);
				ServiceDescriptionFormatExtensionCollection extensions = o.Extensions;
				if (extensions != null)
				{
					for (int j = 0; j < ((ICollection)extensions).Count; j++)
					{
						object obj = extensions[j];
						if (obj is Soap12BodyBinding)
						{
							this.Write102_Soap12BodyBinding("body", "http://schemas.xmlsoap.org/wsdl/soap12/", (Soap12BodyBinding)obj, false, false);
						}
						else if (obj is Soap12HeaderBinding)
						{
							this.Write109_Soap12HeaderBinding("header", "http://schemas.xmlsoap.org/wsdl/soap12/", (Soap12HeaderBinding)obj, false, false);
						}
						else if (obj is SoapBodyBinding)
						{
							this.Write99_SoapBodyBinding("body", "http://schemas.xmlsoap.org/wsdl/soap/", (SoapBodyBinding)obj, false, false);
						}
						else if (obj is SoapHeaderBinding)
						{
							this.Write106_SoapHeaderBinding("header", "http://schemas.xmlsoap.org/wsdl/soap/", (SoapHeaderBinding)obj, false, false);
						}
						else if (obj is MimeTextBinding)
						{
							this.Write97_MimeTextBinding("text", "http://microsoft.com/wsdl/mime/textMatching/", (MimeTextBinding)obj, false, false);
						}
						else if (obj is HttpUrlReplacementBinding)
						{
							this.Write91_HttpUrlReplacementBinding("urlReplacement", "http://schemas.xmlsoap.org/wsdl/http/", (HttpUrlReplacementBinding)obj, false, false);
						}
						else if (obj is HttpUrlEncodedBinding)
						{
							this.Write90_HttpUrlEncodedBinding("urlEncoded", "http://schemas.xmlsoap.org/wsdl/http/", (HttpUrlEncodedBinding)obj, false, false);
						}
						else if (obj is MimeContentBinding)
						{
							this.Write93_MimeContentBinding("content", "http://schemas.xmlsoap.org/wsdl/mime/", (MimeContentBinding)obj, false, false);
						}
						else if (obj is MimeMultipartRelatedBinding)
						{
							this.Write104_MimeMultipartRelatedBinding("multipartRelated", "http://schemas.xmlsoap.org/wsdl/mime/", (MimeMultipartRelatedBinding)obj, false, false);
						}
						else if (obj is MimeXmlBinding)
						{
							this.Write94_MimeXmlBinding("mimeXml", "http://schemas.xmlsoap.org/wsdl/mime/", (MimeXmlBinding)obj, false, false);
						}
						else if (obj is XmlElement)
						{
							XmlElement xmlElement = (XmlElement)obj;
							if (xmlElement == null && xmlElement != null)
							{
								throw base.CreateInvalidAnyTypeException(xmlElement);
							}
							base.WriteElementLiteral(xmlElement, "", null, false, true);
						}
						else if (obj != null)
						{
							throw base.CreateUnknownTypeException(obj);
						}
					}
				}
				base.WriteEndElement(o);
				return;
			}
			throw base.CreateInvalidAnyTypeException(o.DocumentationElement);
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x00022938 File Offset: 0x00021938
		private void Write90_HttpUrlEncodedBinding(string n, string ns, HttpUrlEncodedBinding o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(HttpUrlEncodedBinding))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("HttpUrlEncodedBinding", "http://schemas.xmlsoap.org/wsdl/http/");
			}
			if (o.Required)
			{
				base.WriteAttribute("required", "http://schemas.xmlsoap.org/wsdl/", XmlConvert.ToString(o.Required));
			}
			base.WriteEndElement(o);
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x000229C0 File Offset: 0x000219C0
		private void Write91_HttpUrlReplacementBinding(string n, string ns, HttpUrlReplacementBinding o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(HttpUrlReplacementBinding))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("HttpUrlReplacementBinding", "http://schemas.xmlsoap.org/wsdl/http/");
			}
			if (o.Required)
			{
				base.WriteAttribute("required", "http://schemas.xmlsoap.org/wsdl/", XmlConvert.ToString(o.Required));
			}
			base.WriteEndElement(o);
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x00022A48 File Offset: 0x00021A48
		private void Write86_SoapOperationBinding(string n, string ns, SoapOperationBinding o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(SoapOperationBinding))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("SoapOperationBinding", "http://schemas.xmlsoap.org/wsdl/soap/");
			}
			if (o.Required)
			{
				base.WriteAttribute("required", "http://schemas.xmlsoap.org/wsdl/", XmlConvert.ToString(o.Required));
			}
			base.WriteAttribute("soapAction", "", o.SoapAction);
			if (o.Style != SoapBindingStyle.Default)
			{
				base.WriteAttribute("style", "", this.Write79_SoapBindingStyle(o.Style));
			}
			base.WriteEndElement(o);
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x00022B08 File Offset: 0x00021B08
		private string Write79_SoapBindingStyle(SoapBindingStyle v)
		{
			string text;
			switch (v)
			{
			case SoapBindingStyle.Document:
				text = "document";
				break;
			case SoapBindingStyle.Rpc:
				text = "rpc";
				break;
			default:
				throw base.CreateInvalidEnumValueException(((long)v).ToString(CultureInfo.InvariantCulture), "System.Web.Services.Description.SoapBindingStyle");
			}
			return text;
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x00022B58 File Offset: 0x00021B58
		private void Write85_HttpOperationBinding(string n, string ns, HttpOperationBinding o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(HttpOperationBinding))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("HttpOperationBinding", "http://schemas.xmlsoap.org/wsdl/http/");
			}
			if (o.Required)
			{
				base.WriteAttribute("required", "http://schemas.xmlsoap.org/wsdl/", XmlConvert.ToString(o.Required));
			}
			base.WriteAttribute("location", "", o.Location);
			base.WriteEndElement(o);
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x00022BF4 File Offset: 0x00021BF4
		private void Write88_Soap12OperationBinding(string n, string ns, Soap12OperationBinding o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(Soap12OperationBinding))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("Soap12OperationBinding", "http://schemas.xmlsoap.org/wsdl/soap12/");
			}
			if (o.Required)
			{
				base.WriteAttribute("required", "http://schemas.xmlsoap.org/wsdl/", XmlConvert.ToString(o.Required));
			}
			base.WriteAttribute("soapAction", "", o.SoapAction);
			if (o.Style != SoapBindingStyle.Default)
			{
				base.WriteAttribute("style", "", this.Write82_SoapBindingStyle(o.Style));
			}
			if (o.SoapActionRequired)
			{
				base.WriteAttribute("soapActionRequired", "", XmlConvert.ToString(o.SoapActionRequired));
			}
			base.WriteEndElement(o);
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x00022CD8 File Offset: 0x00021CD8
		private string Write82_SoapBindingStyle(SoapBindingStyle v)
		{
			string text;
			switch (v)
			{
			case SoapBindingStyle.Document:
				text = "document";
				break;
			case SoapBindingStyle.Rpc:
				text = "rpc";
				break;
			default:
				throw base.CreateInvalidEnumValueException(((long)v).ToString(CultureInfo.InvariantCulture), "System.Web.Services.Description.SoapBindingStyle");
			}
			return text;
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x00022D28 File Offset: 0x00021D28
		private void Write80_SoapBinding(string n, string ns, SoapBinding o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(SoapBinding))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("SoapBinding", "http://schemas.xmlsoap.org/wsdl/soap/");
			}
			if (o.Required)
			{
				base.WriteAttribute("required", "http://schemas.xmlsoap.org/wsdl/", XmlConvert.ToString(o.Required));
			}
			base.WriteAttribute("transport", "", o.Transport);
			if (o.Style != SoapBindingStyle.Document)
			{
				base.WriteAttribute("style", "", this.Write79_SoapBindingStyle(o.Style));
			}
			base.WriteEndElement(o);
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x00022DEC File Offset: 0x00021DEC
		private void Write77_HttpBinding(string n, string ns, HttpBinding o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(HttpBinding))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("HttpBinding", "http://schemas.xmlsoap.org/wsdl/http/");
			}
			if (o.Required)
			{
				base.WriteAttribute("required", "http://schemas.xmlsoap.org/wsdl/", XmlConvert.ToString(o.Required));
			}
			base.WriteAttribute("verb", "", o.Verb);
			base.WriteEndElement(o);
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x00022E88 File Offset: 0x00021E88
		private void Write84_Soap12Binding(string n, string ns, Soap12Binding o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(Soap12Binding))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("Soap12Binding", "http://schemas.xmlsoap.org/wsdl/soap12/");
			}
			if (o.Required)
			{
				base.WriteAttribute("required", "http://schemas.xmlsoap.org/wsdl/", XmlConvert.ToString(o.Required));
			}
			base.WriteAttribute("transport", "", o.Transport);
			if (o.Style != SoapBindingStyle.Document)
			{
				base.WriteAttribute("style", "", this.Write82_SoapBindingStyle(o.Style));
			}
			base.WriteEndElement(o);
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x00022F4C File Offset: 0x00021F4C
		private void Write75_PortType(string n, string ns, PortType o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(PortType))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("PortType", "http://schemas.xmlsoap.org/wsdl/");
			}
			XmlAttribute[] extensibleAttributes = o.ExtensibleAttributes;
			if (extensibleAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in extensibleAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("name", "", o.Name);
			if (o.DocumentationElement != null || o.DocumentationElement == null)
			{
				base.WriteElementLiteral(o.DocumentationElement, "documentation", "http://schemas.xmlsoap.org/wsdl/", false, true);
				ServiceDescriptionFormatExtensionCollection extensions = o.Extensions;
				if (extensions != null)
				{
					for (int j = 0; j < ((ICollection)extensions).Count; j++)
					{
						if (!(extensions[j] is XmlNode) && extensions[j] != null)
						{
							throw base.CreateInvalidAnyTypeException(extensions[j]);
						}
						base.WriteElementLiteral((XmlNode)extensions[j], "", null, false, true);
					}
				}
				OperationCollection operations = o.Operations;
				if (operations != null)
				{
					for (int k = 0; k < ((ICollection)operations).Count; k++)
					{
						this.Write74_Operation("operation", "http://schemas.xmlsoap.org/wsdl/", operations[k], false, false);
					}
				}
				base.WriteEndElement(o);
				return;
			}
			throw base.CreateInvalidAnyTypeException(o.DocumentationElement);
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x000230D0 File Offset: 0x000220D0
		private void Write74_Operation(string n, string ns, Operation o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(Operation))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("Operation", "http://schemas.xmlsoap.org/wsdl/");
			}
			XmlAttribute[] extensibleAttributes = o.ExtensibleAttributes;
			if (extensibleAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in extensibleAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("name", "", o.Name);
			if (o.ParameterOrderString != null && o.ParameterOrderString.Length != 0)
			{
				base.WriteAttribute("parameterOrder", "", o.ParameterOrderString);
			}
			if (o.DocumentationElement != null || o.DocumentationElement == null)
			{
				base.WriteElementLiteral(o.DocumentationElement, "documentation", "http://schemas.xmlsoap.org/wsdl/", false, true);
				ServiceDescriptionFormatExtensionCollection extensions = o.Extensions;
				if (extensions != null)
				{
					for (int j = 0; j < ((ICollection)extensions).Count; j++)
					{
						if (!(extensions[j] is XmlNode) && extensions[j] != null)
						{
							throw base.CreateInvalidAnyTypeException(extensions[j]);
						}
						base.WriteElementLiteral((XmlNode)extensions[j], "", null, false, true);
					}
				}
				OperationMessageCollection messages = o.Messages;
				if (messages != null)
				{
					for (int k = 0; k < ((ICollection)messages).Count; k++)
					{
						OperationMessage operationMessage = messages[k];
						if (operationMessage is OperationOutput)
						{
							this.Write72_OperationOutput("output", "http://schemas.xmlsoap.org/wsdl/", (OperationOutput)operationMessage, false, false);
						}
						else if (operationMessage is OperationInput)
						{
							this.Write71_OperationInput("input", "http://schemas.xmlsoap.org/wsdl/", (OperationInput)operationMessage, false, false);
						}
						else if (operationMessage != null)
						{
							throw base.CreateUnknownTypeException(operationMessage);
						}
					}
				}
				OperationFaultCollection faults = o.Faults;
				if (faults != null)
				{
					for (int l = 0; l < ((ICollection)faults).Count; l++)
					{
						this.Write73_OperationFault("fault", "http://schemas.xmlsoap.org/wsdl/", faults[l], false, false);
					}
				}
				base.WriteEndElement(o);
				return;
			}
			throw base.CreateInvalidAnyTypeException(o.DocumentationElement);
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x00023300 File Offset: 0x00022300
		private void Write73_OperationFault(string n, string ns, OperationFault o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(OperationFault))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("OperationFault", "http://schemas.xmlsoap.org/wsdl/");
			}
			XmlAttribute[] extensibleAttributes = o.ExtensibleAttributes;
			if (extensibleAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in extensibleAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("name", "", o.Name);
			base.WriteAttribute("message", "", base.FromXmlQualifiedName(o.Message));
			if (o.DocumentationElement != null || o.DocumentationElement == null)
			{
				base.WriteElementLiteral(o.DocumentationElement, "documentation", "http://schemas.xmlsoap.org/wsdl/", false, true);
				ServiceDescriptionFormatExtensionCollection extensions = o.Extensions;
				if (extensions != null)
				{
					for (int j = 0; j < ((ICollection)extensions).Count; j++)
					{
						if (!(extensions[j] is XmlNode) && extensions[j] != null)
						{
							throw base.CreateInvalidAnyTypeException(extensions[j]);
						}
						base.WriteElementLiteral((XmlNode)extensions[j], "", null, false, true);
					}
				}
				base.WriteEndElement(o);
				return;
			}
			throw base.CreateInvalidAnyTypeException(o.DocumentationElement);
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x00023460 File Offset: 0x00022460
		private void Write71_OperationInput(string n, string ns, OperationInput o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(OperationInput))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("OperationInput", "http://schemas.xmlsoap.org/wsdl/");
			}
			XmlAttribute[] extensibleAttributes = o.ExtensibleAttributes;
			if (extensibleAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in extensibleAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("name", "", o.Name);
			base.WriteAttribute("message", "", base.FromXmlQualifiedName(o.Message));
			if (o.DocumentationElement != null || o.DocumentationElement == null)
			{
				base.WriteElementLiteral(o.DocumentationElement, "documentation", "http://schemas.xmlsoap.org/wsdl/", false, true);
				ServiceDescriptionFormatExtensionCollection extensions = o.Extensions;
				if (extensions != null)
				{
					for (int j = 0; j < ((ICollection)extensions).Count; j++)
					{
						if (!(extensions[j] is XmlNode) && extensions[j] != null)
						{
							throw base.CreateInvalidAnyTypeException(extensions[j]);
						}
						base.WriteElementLiteral((XmlNode)extensions[j], "", null, false, true);
					}
				}
				base.WriteEndElement(o);
				return;
			}
			throw base.CreateInvalidAnyTypeException(o.DocumentationElement);
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x000235C0 File Offset: 0x000225C0
		private void Write72_OperationOutput(string n, string ns, OperationOutput o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(OperationOutput))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("OperationOutput", "http://schemas.xmlsoap.org/wsdl/");
			}
			XmlAttribute[] extensibleAttributes = o.ExtensibleAttributes;
			if (extensibleAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in extensibleAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("name", "", o.Name);
			base.WriteAttribute("message", "", base.FromXmlQualifiedName(o.Message));
			if (o.DocumentationElement != null || o.DocumentationElement == null)
			{
				base.WriteElementLiteral(o.DocumentationElement, "documentation", "http://schemas.xmlsoap.org/wsdl/", false, true);
				ServiceDescriptionFormatExtensionCollection extensions = o.Extensions;
				if (extensions != null)
				{
					for (int j = 0; j < ((ICollection)extensions).Count; j++)
					{
						if (!(extensions[j] is XmlNode) && extensions[j] != null)
						{
							throw base.CreateInvalidAnyTypeException(extensions[j]);
						}
						base.WriteElementLiteral((XmlNode)extensions[j], "", null, false, true);
					}
				}
				base.WriteEndElement(o);
				return;
			}
			throw base.CreateInvalidAnyTypeException(o.DocumentationElement);
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x00023720 File Offset: 0x00022720
		private void Write69_Message(string n, string ns, Message o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(Message))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("Message", "http://schemas.xmlsoap.org/wsdl/");
			}
			XmlAttribute[] extensibleAttributes = o.ExtensibleAttributes;
			if (extensibleAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in extensibleAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("name", "", o.Name);
			if (o.DocumentationElement != null || o.DocumentationElement == null)
			{
				base.WriteElementLiteral(o.DocumentationElement, "documentation", "http://schemas.xmlsoap.org/wsdl/", false, true);
				ServiceDescriptionFormatExtensionCollection extensions = o.Extensions;
				if (extensions != null)
				{
					for (int j = 0; j < ((ICollection)extensions).Count; j++)
					{
						if (!(extensions[j] is XmlNode) && extensions[j] != null)
						{
							throw base.CreateInvalidAnyTypeException(extensions[j]);
						}
						base.WriteElementLiteral((XmlNode)extensions[j], "", null, false, true);
					}
				}
				MessagePartCollection parts = o.Parts;
				if (parts != null)
				{
					for (int k = 0; k < ((ICollection)parts).Count; k++)
					{
						this.Write68_MessagePart("part", "http://schemas.xmlsoap.org/wsdl/", parts[k], false, false);
					}
				}
				base.WriteEndElement(o);
				return;
			}
			throw base.CreateInvalidAnyTypeException(o.DocumentationElement);
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x000238A4 File Offset: 0x000228A4
		private void Write68_MessagePart(string n, string ns, MessagePart o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(MessagePart))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("MessagePart", "http://schemas.xmlsoap.org/wsdl/");
			}
			XmlAttribute[] extensibleAttributes = o.ExtensibleAttributes;
			if (extensibleAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in extensibleAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("name", "", o.Name);
			base.WriteAttribute("element", "", base.FromXmlQualifiedName(o.Element));
			base.WriteAttribute("type", "", base.FromXmlQualifiedName(o.Type));
			if (o.DocumentationElement != null || o.DocumentationElement == null)
			{
				base.WriteElementLiteral(o.DocumentationElement, "documentation", "http://schemas.xmlsoap.org/wsdl/", false, true);
				ServiceDescriptionFormatExtensionCollection extensions = o.Extensions;
				if (extensions != null)
				{
					for (int j = 0; j < ((ICollection)extensions).Count; j++)
					{
						if (!(extensions[j] is XmlNode) && extensions[j] != null)
						{
							throw base.CreateInvalidAnyTypeException(extensions[j]);
						}
						base.WriteElementLiteral((XmlNode)extensions[j], "", null, false, true);
					}
				}
				base.WriteEndElement(o);
				return;
			}
			throw base.CreateInvalidAnyTypeException(o.DocumentationElement);
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x00023A20 File Offset: 0x00022A20
		private void Write67_Types(string n, string ns, Types o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(Types))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("Types", "http://schemas.xmlsoap.org/wsdl/");
			}
			XmlAttribute[] extensibleAttributes = o.ExtensibleAttributes;
			if (extensibleAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in extensibleAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			if (o.DocumentationElement != null || o.DocumentationElement == null)
			{
				base.WriteElementLiteral(o.DocumentationElement, "documentation", "http://schemas.xmlsoap.org/wsdl/", false, true);
				ServiceDescriptionFormatExtensionCollection extensions = o.Extensions;
				if (extensions != null)
				{
					for (int j = 0; j < ((ICollection)extensions).Count; j++)
					{
						if (!(extensions[j] is XmlNode) && extensions[j] != null)
						{
							throw base.CreateInvalidAnyTypeException(extensions[j]);
						}
						base.WriteElementLiteral((XmlNode)extensions[j], "", null, false, true);
					}
				}
				XmlSchemas schemas = o.Schemas;
				if (schemas != null)
				{
					for (int k = 0; k < ((ICollection)schemas).Count; k++)
					{
						this.Write66_XmlSchema("schema", "http://www.w3.org/2001/XMLSchema", schemas[k], false, false);
					}
				}
				base.WriteEndElement(o);
				return;
			}
			throw base.CreateInvalidAnyTypeException(o.DocumentationElement);
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x00023B8C File Offset: 0x00022B8C
		private void Write66_XmlSchema(string n, string ns, XmlSchema o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchema))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchema", "http://www.w3.org/2001/XMLSchema");
			}
			if (o.AttributeFormDefault != XmlSchemaForm.None)
			{
				base.WriteAttribute("attributeFormDefault", "", this.Write6_XmlSchemaForm(o.AttributeFormDefault));
			}
			if (o.BlockDefault != XmlSchemaDerivationMethod.None)
			{
				base.WriteAttribute("blockDefault", "", this.Write7_XmlSchemaDerivationMethod(o.BlockDefault));
			}
			if (o.FinalDefault != XmlSchemaDerivationMethod.None)
			{
				base.WriteAttribute("finalDefault", "", this.Write7_XmlSchemaDerivationMethod(o.FinalDefault));
			}
			if (o.ElementFormDefault != XmlSchemaForm.None)
			{
				base.WriteAttribute("elementFormDefault", "", this.Write6_XmlSchemaForm(o.ElementFormDefault));
			}
			base.WriteAttribute("targetNamespace", "", o.TargetNamespace);
			base.WriteAttribute("version", "", o.Version);
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			XmlSchemaObjectCollection includes = o.Includes;
			if (includes != null)
			{
				for (int j = 0; j < ((ICollection)includes).Count; j++)
				{
					XmlSchemaObject xmlSchemaObject = includes[j];
					if (xmlSchemaObject is XmlSchemaRedefine)
					{
						this.Write64_XmlSchemaRedefine("redefine", "http://www.w3.org/2001/XMLSchema", (XmlSchemaRedefine)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaImport)
					{
						this.Write13_XmlSchemaImport("import", "http://www.w3.org/2001/XMLSchema", (XmlSchemaImport)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaInclude)
					{
						this.Write12_XmlSchemaInclude("include", "http://www.w3.org/2001/XMLSchema", (XmlSchemaInclude)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject != null)
					{
						throw base.CreateUnknownTypeException(xmlSchemaObject);
					}
				}
			}
			XmlSchemaObjectCollection items = o.Items;
			if (items != null)
			{
				for (int k = 0; k < ((ICollection)items).Count; k++)
				{
					XmlSchemaObject xmlSchemaObject2 = items[k];
					if (xmlSchemaObject2 is XmlSchemaElement)
					{
						this.Write52_XmlSchemaElement("element", "http://www.w3.org/2001/XMLSchema", (XmlSchemaElement)xmlSchemaObject2, false, false);
					}
					else if (xmlSchemaObject2 is XmlSchemaComplexType)
					{
						this.Write62_XmlSchemaComplexType("complexType", "http://www.w3.org/2001/XMLSchema", (XmlSchemaComplexType)xmlSchemaObject2, false, false);
					}
					else if (xmlSchemaObject2 is XmlSchemaSimpleType)
					{
						this.Write34_XmlSchemaSimpleType("simpleType", "http://www.w3.org/2001/XMLSchema", (XmlSchemaSimpleType)xmlSchemaObject2, false, false);
					}
					else if (xmlSchemaObject2 is XmlSchemaAttribute)
					{
						this.Write36_XmlSchemaAttribute("attribute", "http://www.w3.org/2001/XMLSchema", (XmlSchemaAttribute)xmlSchemaObject2, false, false);
					}
					else if (xmlSchemaObject2 is XmlSchemaAttributeGroup)
					{
						this.Write40_XmlSchemaAttributeGroup("attributeGroup", "http://www.w3.org/2001/XMLSchema", (XmlSchemaAttributeGroup)xmlSchemaObject2, false, false);
					}
					else if (xmlSchemaObject2 is XmlSchemaNotation)
					{
						this.Write65_XmlSchemaNotation("notation", "http://www.w3.org/2001/XMLSchema", (XmlSchemaNotation)xmlSchemaObject2, false, false);
					}
					else if (xmlSchemaObject2 is XmlSchemaGroup)
					{
						this.Write63_XmlSchemaGroup("group", "http://www.w3.org/2001/XMLSchema", (XmlSchemaGroup)xmlSchemaObject2, false, false);
					}
					else if (xmlSchemaObject2 is XmlSchemaAnnotation)
					{
						this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", (XmlSchemaAnnotation)xmlSchemaObject2, false, false);
					}
					else if (xmlSchemaObject2 != null)
					{
						throw base.CreateUnknownTypeException(xmlSchemaObject2);
					}
				}
			}
			base.WriteEndElement(o);
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x00023F1C File Offset: 0x00022F1C
		private void Write11_XmlSchemaAnnotation(string n, string ns, XmlSchemaAnnotation o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaAnnotation))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaAnnotation", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			XmlSchemaObjectCollection items = o.Items;
			if (items != null)
			{
				for (int j = 0; j < ((ICollection)items).Count; j++)
				{
					XmlSchemaObject xmlSchemaObject = items[j];
					if (xmlSchemaObject is XmlSchemaAppInfo)
					{
						this.Write10_XmlSchemaAppInfo("appinfo", "http://www.w3.org/2001/XMLSchema", (XmlSchemaAppInfo)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaDocumentation)
					{
						this.Write9_XmlSchemaDocumentation("documentation", "http://www.w3.org/2001/XMLSchema", (XmlSchemaDocumentation)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject != null)
					{
						throw base.CreateUnknownTypeException(xmlSchemaObject);
					}
				}
			}
			base.WriteEndElement(o);
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x00024048 File Offset: 0x00023048
		private void Write9_XmlSchemaDocumentation(string n, string ns, XmlSchemaDocumentation o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaDocumentation))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaDocumentation", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("source", "", o.Source);
			base.WriteAttribute("lang", "http://www.w3.org/XML/1998/namespace", o.Language);
			XmlNode[] markup = o.Markup;
			if (markup != null)
			{
				foreach (XmlNode xmlNode in markup)
				{
					if (xmlNode is XmlElement)
					{
						XmlElement xmlElement = (XmlElement)xmlNode;
						if (xmlElement == null && xmlElement != null)
						{
							throw base.CreateInvalidAnyTypeException(xmlElement);
						}
						base.WriteElementLiteral(xmlElement, "", null, false, true);
					}
					else if (xmlNode != null)
					{
						xmlNode.WriteTo(base.Writer);
					}
					else if (xmlNode != null)
					{
						throw base.CreateUnknownTypeException(xmlNode);
					}
				}
			}
			base.WriteEndElement(o);
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x00024150 File Offset: 0x00023150
		private void Write10_XmlSchemaAppInfo(string n, string ns, XmlSchemaAppInfo o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaAppInfo))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaAppInfo", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("source", "", o.Source);
			XmlNode[] markup = o.Markup;
			if (markup != null)
			{
				foreach (XmlNode xmlNode in markup)
				{
					if (xmlNode is XmlElement)
					{
						XmlElement xmlElement = (XmlElement)xmlNode;
						if (xmlElement == null && xmlElement != null)
						{
							throw base.CreateInvalidAnyTypeException(xmlElement);
						}
						base.WriteElementLiteral(xmlElement, "", null, false, true);
					}
					else if (xmlNode != null)
					{
						xmlNode.WriteTo(base.Writer);
					}
					else if (xmlNode != null)
					{
						throw base.CreateUnknownTypeException(xmlNode);
					}
				}
			}
			base.WriteEndElement(o);
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x00024240 File Offset: 0x00023240
		private void Write63_XmlSchemaGroup(string n, string ns, XmlSchemaGroup o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaGroup))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaGroup", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("name", "", o.Name);
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			if (o.Particle is XmlSchemaAll)
			{
				this.Write55_XmlSchemaAll("all", "http://www.w3.org/2001/XMLSchema", (XmlSchemaAll)o.Particle, false, false);
			}
			else if (o.Particle is XmlSchemaChoice)
			{
				this.Write54_XmlSchemaChoice("choice", "http://www.w3.org/2001/XMLSchema", (XmlSchemaChoice)o.Particle, false, false);
			}
			else if (o.Particle is XmlSchemaSequence)
			{
				this.Write53_XmlSchemaSequence("sequence", "http://www.w3.org/2001/XMLSchema", (XmlSchemaSequence)o.Particle, false, false);
			}
			else if (o.Particle != null)
			{
				throw base.CreateUnknownTypeException(o.Particle);
			}
			base.WriteEndElement(o);
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x000243B0 File Offset: 0x000233B0
		private void Write53_XmlSchemaSequence(string n, string ns, XmlSchemaSequence o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaSequence))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaSequence", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("minOccurs", "", o.MinOccursString);
			base.WriteAttribute("maxOccurs", "", o.MaxOccursString);
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			XmlSchemaObjectCollection items = o.Items;
			if (items != null)
			{
				for (int j = 0; j < ((ICollection)items).Count; j++)
				{
					XmlSchemaObject xmlSchemaObject = items[j];
					if (xmlSchemaObject is XmlSchemaChoice)
					{
						this.Write54_XmlSchemaChoice("choice", "http://www.w3.org/2001/XMLSchema", (XmlSchemaChoice)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaSequence)
					{
						this.Write53_XmlSchemaSequence("sequence", "http://www.w3.org/2001/XMLSchema", (XmlSchemaSequence)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaGroupRef)
					{
						this.Write44_XmlSchemaGroupRef("group", "http://www.w3.org/2001/XMLSchema", (XmlSchemaGroupRef)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaElement)
					{
						this.Write52_XmlSchemaElement("element", "http://www.w3.org/2001/XMLSchema", (XmlSchemaElement)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaAny)
					{
						this.Write46_XmlSchemaAny("any", "http://www.w3.org/2001/XMLSchema", (XmlSchemaAny)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject != null)
					{
						throw base.CreateUnknownTypeException(xmlSchemaObject);
					}
				}
			}
			base.WriteEndElement(o);
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x00024598 File Offset: 0x00023598
		private void Write46_XmlSchemaAny(string n, string ns, XmlSchemaAny o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaAny))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaAny", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("minOccurs", "", o.MinOccursString);
			base.WriteAttribute("maxOccurs", "", o.MaxOccursString);
			base.WriteAttribute("namespace", "", o.Namespace);
			if (o.ProcessContents != XmlSchemaContentProcessing.None)
			{
				base.WriteAttribute("processContents", "", this.Write38_XmlSchemaContentProcessing(o.ProcessContents));
			}
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x000246C0 File Offset: 0x000236C0
		private string Write38_XmlSchemaContentProcessing(XmlSchemaContentProcessing v)
		{
			string text;
			switch (v)
			{
			case XmlSchemaContentProcessing.Skip:
				text = "skip";
				break;
			case XmlSchemaContentProcessing.Lax:
				text = "lax";
				break;
			case XmlSchemaContentProcessing.Strict:
				text = "strict";
				break;
			default:
				throw base.CreateInvalidEnumValueException(((long)v).ToString(CultureInfo.InvariantCulture), "System.Xml.Schema.XmlSchemaContentProcessing");
			}
			return text;
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0002471C File Offset: 0x0002371C
		private void Write52_XmlSchemaElement(string n, string ns, XmlSchemaElement o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaElement))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaElement", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("minOccurs", "", o.MinOccursString);
			base.WriteAttribute("maxOccurs", "", o.MaxOccursString);
			if (o.IsAbstract)
			{
				base.WriteAttribute("abstract", "", XmlConvert.ToString(o.IsAbstract));
			}
			if (o.Block != XmlSchemaDerivationMethod.None)
			{
				base.WriteAttribute("block", "", this.Write7_XmlSchemaDerivationMethod(o.Block));
			}
			base.WriteAttribute("default", "", o.DefaultValue);
			if (o.Final != XmlSchemaDerivationMethod.None)
			{
				base.WriteAttribute("final", "", this.Write7_XmlSchemaDerivationMethod(o.Final));
			}
			base.WriteAttribute("fixed", "", o.FixedValue);
			if (o.Form != XmlSchemaForm.None)
			{
				base.WriteAttribute("form", "", this.Write6_XmlSchemaForm(o.Form));
			}
			if (o.Name != null && o.Name.Length != 0)
			{
				base.WriteAttribute("name", "", o.Name);
			}
			if (o.IsNillable)
			{
				base.WriteAttribute("nillable", "", XmlConvert.ToString(o.IsNillable));
			}
			base.WriteAttribute("ref", "", base.FromXmlQualifiedName(o.RefName));
			base.WriteAttribute("substitutionGroup", "", base.FromXmlQualifiedName(o.SubstitutionGroup));
			base.WriteAttribute("type", "", base.FromXmlQualifiedName(o.SchemaTypeName));
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			if (o.SchemaType is XmlSchemaComplexType)
			{
				this.Write62_XmlSchemaComplexType("complexType", "http://www.w3.org/2001/XMLSchema", (XmlSchemaComplexType)o.SchemaType, false, false);
			}
			else if (o.SchemaType is XmlSchemaSimpleType)
			{
				this.Write34_XmlSchemaSimpleType("simpleType", "http://www.w3.org/2001/XMLSchema", (XmlSchemaSimpleType)o.SchemaType, false, false);
			}
			else if (o.SchemaType != null)
			{
				throw base.CreateUnknownTypeException(o.SchemaType);
			}
			XmlSchemaObjectCollection constraints = o.Constraints;
			if (constraints != null)
			{
				for (int j = 0; j < ((ICollection)constraints).Count; j++)
				{
					XmlSchemaObject xmlSchemaObject = constraints[j];
					if (xmlSchemaObject is XmlSchemaKeyref)
					{
						this.Write51_XmlSchemaKeyref("keyref", "http://www.w3.org/2001/XMLSchema", (XmlSchemaKeyref)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaUnique)
					{
						this.Write50_XmlSchemaUnique("unique", "http://www.w3.org/2001/XMLSchema", (XmlSchemaUnique)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaKey)
					{
						this.Write49_XmlSchemaKey("key", "http://www.w3.org/2001/XMLSchema", (XmlSchemaKey)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject != null)
					{
						throw base.CreateUnknownTypeException(xmlSchemaObject);
					}
				}
			}
			base.WriteEndElement(o);
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x00024A8C File Offset: 0x00023A8C
		private void Write49_XmlSchemaKey(string n, string ns, XmlSchemaKey o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaKey))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaKey", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("name", "", o.Name);
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			this.Write47_XmlSchemaXPath("selector", "http://www.w3.org/2001/XMLSchema", o.Selector, false, false);
			XmlSchemaObjectCollection fields = o.Fields;
			if (fields != null)
			{
				for (int j = 0; j < ((ICollection)fields).Count; j++)
				{
					this.Write47_XmlSchemaXPath("field", "http://www.w3.org/2001/XMLSchema", (XmlSchemaXPath)fields[j], false, false);
				}
			}
			base.WriteEndElement(o);
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x00024BC0 File Offset: 0x00023BC0
		private void Write47_XmlSchemaXPath(string n, string ns, XmlSchemaXPath o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaXPath))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaXPath", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			if (o.XPath != null && o.XPath.Length != 0)
			{
				base.WriteAttribute("xpath", "", o.XPath);
			}
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x00024CAC File Offset: 0x00023CAC
		private void Write50_XmlSchemaUnique(string n, string ns, XmlSchemaUnique o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaUnique))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaUnique", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("name", "", o.Name);
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			this.Write47_XmlSchemaXPath("selector", "http://www.w3.org/2001/XMLSchema", o.Selector, false, false);
			XmlSchemaObjectCollection fields = o.Fields;
			if (fields != null)
			{
				for (int j = 0; j < ((ICollection)fields).Count; j++)
				{
					this.Write47_XmlSchemaXPath("field", "http://www.w3.org/2001/XMLSchema", (XmlSchemaXPath)fields[j], false, false);
				}
			}
			base.WriteEndElement(o);
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x00024DE0 File Offset: 0x00023DE0
		private void Write51_XmlSchemaKeyref(string n, string ns, XmlSchemaKeyref o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaKeyref))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaKeyref", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("name", "", o.Name);
			base.WriteAttribute("refer", "", base.FromXmlQualifiedName(o.Refer));
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			this.Write47_XmlSchemaXPath("selector", "http://www.w3.org/2001/XMLSchema", o.Selector, false, false);
			XmlSchemaObjectCollection fields = o.Fields;
			if (fields != null)
			{
				for (int j = 0; j < ((ICollection)fields).Count; j++)
				{
					this.Write47_XmlSchemaXPath("field", "http://www.w3.org/2001/XMLSchema", (XmlSchemaXPath)fields[j], false, false);
				}
			}
			base.WriteEndElement(o);
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x00024F30 File Offset: 0x00023F30
		private void Write34_XmlSchemaSimpleType(string n, string ns, XmlSchemaSimpleType o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaSimpleType))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaSimpleType", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("name", "", o.Name);
			if (o.Final != XmlSchemaDerivationMethod.None)
			{
				base.WriteAttribute("final", "", this.Write7_XmlSchemaDerivationMethod(o.Final));
			}
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			if (o.Content is XmlSchemaSimpleTypeUnion)
			{
				this.Write33_XmlSchemaSimpleTypeUnion("union", "http://www.w3.org/2001/XMLSchema", (XmlSchemaSimpleTypeUnion)o.Content, false, false);
			}
			else if (o.Content is XmlSchemaSimpleTypeRestriction)
			{
				this.Write32_XmlSchemaSimpleTypeRestriction("restriction", "http://www.w3.org/2001/XMLSchema", (XmlSchemaSimpleTypeRestriction)o.Content, false, false);
			}
			else if (o.Content is XmlSchemaSimpleTypeList)
			{
				this.Write17_XmlSchemaSimpleTypeList("list", "http://www.w3.org/2001/XMLSchema", (XmlSchemaSimpleTypeList)o.Content, false, false);
			}
			else if (o.Content != null)
			{
				throw base.CreateUnknownTypeException(o.Content);
			}
			base.WriteEndElement(o);
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x000250CC File Offset: 0x000240CC
		private void Write17_XmlSchemaSimpleTypeList(string n, string ns, XmlSchemaSimpleTypeList o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaSimpleTypeList))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaSimpleTypeList", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("itemType", "", base.FromXmlQualifiedName(o.ItemTypeName));
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			this.Write34_XmlSchemaSimpleType("simpleType", "http://www.w3.org/2001/XMLSchema", o.ItemType, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x000251C4 File Offset: 0x000241C4
		private void Write32_XmlSchemaSimpleTypeRestriction(string n, string ns, XmlSchemaSimpleTypeRestriction o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaSimpleTypeRestriction))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaSimpleTypeRestriction", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("base", "", base.FromXmlQualifiedName(o.BaseTypeName));
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			this.Write34_XmlSchemaSimpleType("simpleType", "http://www.w3.org/2001/XMLSchema", o.BaseType, false, false);
			XmlSchemaObjectCollection facets = o.Facets;
			if (facets != null)
			{
				for (int j = 0; j < ((ICollection)facets).Count; j++)
				{
					XmlSchemaObject xmlSchemaObject = facets[j];
					if (xmlSchemaObject is XmlSchemaLengthFacet)
					{
						this.Write23_XmlSchemaLengthFacet("length", "http://www.w3.org/2001/XMLSchema", (XmlSchemaLengthFacet)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaTotalDigitsFacet)
					{
						this.Write24_XmlSchemaTotalDigitsFacet("totalDigits", "http://www.w3.org/2001/XMLSchema", (XmlSchemaTotalDigitsFacet)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaMaxLengthFacet)
					{
						this.Write22_XmlSchemaMaxLengthFacet("maxLength", "http://www.w3.org/2001/XMLSchema", (XmlSchemaMaxLengthFacet)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaFractionDigitsFacet)
					{
						this.Write20_XmlSchemaFractionDigitsFacet("fractionDigits", "http://www.w3.org/2001/XMLSchema", (XmlSchemaFractionDigitsFacet)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaMinLengthFacet)
					{
						this.Write31_XmlSchemaMinLengthFacet("minLength", "http://www.w3.org/2001/XMLSchema", (XmlSchemaMinLengthFacet)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaMaxExclusiveFacet)
					{
						this.Write28_XmlSchemaMaxExclusiveFacet("maxExclusive", "http://www.w3.org/2001/XMLSchema", (XmlSchemaMaxExclusiveFacet)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaWhiteSpaceFacet)
					{
						this.Write29_XmlSchemaWhiteSpaceFacet("whiteSpace", "http://www.w3.org/2001/XMLSchema", (XmlSchemaWhiteSpaceFacet)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaMinExclusiveFacet)
					{
						this.Write30_XmlSchemaMinExclusiveFacet("minExclusive", "http://www.w3.org/2001/XMLSchema", (XmlSchemaMinExclusiveFacet)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaPatternFacet)
					{
						this.Write25_XmlSchemaPatternFacet("pattern", "http://www.w3.org/2001/XMLSchema", (XmlSchemaPatternFacet)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaMinInclusiveFacet)
					{
						this.Write21_XmlSchemaMinInclusiveFacet("minInclusive", "http://www.w3.org/2001/XMLSchema", (XmlSchemaMinInclusiveFacet)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaMaxInclusiveFacet)
					{
						this.Write27_XmlSchemaMaxInclusiveFacet("maxInclusive", "http://www.w3.org/2001/XMLSchema", (XmlSchemaMaxInclusiveFacet)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaEnumerationFacet)
					{
						this.Write26_XmlSchemaEnumerationFacet("enumeration", "http://www.w3.org/2001/XMLSchema", (XmlSchemaEnumerationFacet)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject != null)
					{
						throw base.CreateUnknownTypeException(xmlSchemaObject);
					}
				}
			}
			base.WriteEndElement(o);
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x000254C4 File Offset: 0x000244C4
		private void Write26_XmlSchemaEnumerationFacet(string n, string ns, XmlSchemaEnumerationFacet o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaEnumerationFacet))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaEnumerationFacet", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("value", "", o.Value);
			if (o.IsFixed)
			{
				base.WriteAttribute("fixed", "", XmlConvert.ToString(o.IsFixed));
			}
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x000255C0 File Offset: 0x000245C0
		private void Write27_XmlSchemaMaxInclusiveFacet(string n, string ns, XmlSchemaMaxInclusiveFacet o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaMaxInclusiveFacet))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaMaxInclusiveFacet", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("value", "", o.Value);
			if (o.IsFixed)
			{
				base.WriteAttribute("fixed", "", XmlConvert.ToString(o.IsFixed));
			}
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x000256BC File Offset: 0x000246BC
		private void Write21_XmlSchemaMinInclusiveFacet(string n, string ns, XmlSchemaMinInclusiveFacet o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaMinInclusiveFacet))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaMinInclusiveFacet", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("value", "", o.Value);
			if (o.IsFixed)
			{
				base.WriteAttribute("fixed", "", XmlConvert.ToString(o.IsFixed));
			}
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x000257B8 File Offset: 0x000247B8
		private void Write25_XmlSchemaPatternFacet(string n, string ns, XmlSchemaPatternFacet o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaPatternFacet))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaPatternFacet", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("value", "", o.Value);
			if (o.IsFixed)
			{
				base.WriteAttribute("fixed", "", XmlConvert.ToString(o.IsFixed));
			}
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x000258B4 File Offset: 0x000248B4
		private void Write30_XmlSchemaMinExclusiveFacet(string n, string ns, XmlSchemaMinExclusiveFacet o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaMinExclusiveFacet))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaMinExclusiveFacet", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("value", "", o.Value);
			if (o.IsFixed)
			{
				base.WriteAttribute("fixed", "", XmlConvert.ToString(o.IsFixed));
			}
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x000259B0 File Offset: 0x000249B0
		private void Write29_XmlSchemaWhiteSpaceFacet(string n, string ns, XmlSchemaWhiteSpaceFacet o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaWhiteSpaceFacet))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaWhiteSpaceFacet", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("value", "", o.Value);
			if (o.IsFixed)
			{
				base.WriteAttribute("fixed", "", XmlConvert.ToString(o.IsFixed));
			}
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x00025AAC File Offset: 0x00024AAC
		private void Write28_XmlSchemaMaxExclusiveFacet(string n, string ns, XmlSchemaMaxExclusiveFacet o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaMaxExclusiveFacet))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaMaxExclusiveFacet", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("value", "", o.Value);
			if (o.IsFixed)
			{
				base.WriteAttribute("fixed", "", XmlConvert.ToString(o.IsFixed));
			}
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x00025BA8 File Offset: 0x00024BA8
		private void Write31_XmlSchemaMinLengthFacet(string n, string ns, XmlSchemaMinLengthFacet o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaMinLengthFacet))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaMinLengthFacet", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("value", "", o.Value);
			if (o.IsFixed)
			{
				base.WriteAttribute("fixed", "", XmlConvert.ToString(o.IsFixed));
			}
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x00025CA4 File Offset: 0x00024CA4
		private void Write20_XmlSchemaFractionDigitsFacet(string n, string ns, XmlSchemaFractionDigitsFacet o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaFractionDigitsFacet))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaFractionDigitsFacet", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("value", "", o.Value);
			if (o.IsFixed)
			{
				base.WriteAttribute("fixed", "", XmlConvert.ToString(o.IsFixed));
			}
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x00025DA0 File Offset: 0x00024DA0
		private void Write22_XmlSchemaMaxLengthFacet(string n, string ns, XmlSchemaMaxLengthFacet o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaMaxLengthFacet))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaMaxLengthFacet", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("value", "", o.Value);
			if (o.IsFixed)
			{
				base.WriteAttribute("fixed", "", XmlConvert.ToString(o.IsFixed));
			}
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x00025E9C File Offset: 0x00024E9C
		private void Write24_XmlSchemaTotalDigitsFacet(string n, string ns, XmlSchemaTotalDigitsFacet o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaTotalDigitsFacet))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaTotalDigitsFacet", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("value", "", o.Value);
			if (o.IsFixed)
			{
				base.WriteAttribute("fixed", "", XmlConvert.ToString(o.IsFixed));
			}
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x00025F98 File Offset: 0x00024F98
		private void Write23_XmlSchemaLengthFacet(string n, string ns, XmlSchemaLengthFacet o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaLengthFacet))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaLengthFacet", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("value", "", o.Value);
			if (o.IsFixed)
			{
				base.WriteAttribute("fixed", "", XmlConvert.ToString(o.IsFixed));
			}
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x00026094 File Offset: 0x00025094
		private void Write33_XmlSchemaSimpleTypeUnion(string n, string ns, XmlSchemaSimpleTypeUnion o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaSimpleTypeUnion))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaSimpleTypeUnion", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			XmlQualifiedName[] memberTypes = o.MemberTypes;
			if (memberTypes != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int j = 0; j < memberTypes.Length; j++)
				{
					XmlQualifiedName xmlQualifiedName = memberTypes[j];
					if (j != 0)
					{
						stringBuilder.Append(" ");
					}
					stringBuilder.Append(base.FromXmlQualifiedName(xmlQualifiedName));
				}
				if (stringBuilder.Length != 0)
				{
					base.WriteAttribute("memberTypes", "", stringBuilder.ToString());
				}
			}
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			XmlSchemaObjectCollection baseTypes = o.BaseTypes;
			if (baseTypes != null)
			{
				for (int k = 0; k < ((ICollection)baseTypes).Count; k++)
				{
					this.Write34_XmlSchemaSimpleType("simpleType", "http://www.w3.org/2001/XMLSchema", (XmlSchemaSimpleType)baseTypes[k], false, false);
				}
			}
			base.WriteEndElement(o);
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x00026240 File Offset: 0x00025240
		private string Write7_XmlSchemaDerivationMethod(XmlSchemaDerivationMethod v)
		{
			switch (v)
			{
			case XmlSchemaDerivationMethod.Empty:
				return "";
			case XmlSchemaDerivationMethod.Substitution:
				return "substitution";
			case XmlSchemaDerivationMethod.Extension:
				return "extension";
			case XmlSchemaDerivationMethod.Substitution | XmlSchemaDerivationMethod.Extension:
			case XmlSchemaDerivationMethod.Substitution | XmlSchemaDerivationMethod.Restriction:
			case XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction:
			case XmlSchemaDerivationMethod.Substitution | XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction:
				break;
			case XmlSchemaDerivationMethod.Restriction:
				return "restriction";
			case XmlSchemaDerivationMethod.List:
				return "list";
			default:
				if (v == XmlSchemaDerivationMethod.Union)
				{
					return "union";
				}
				if (v == XmlSchemaDerivationMethod.All)
				{
					return "#all";
				}
				break;
			}
			return XmlSerializationWriter.FromEnum((long)v, new string[] { "", "substitution", "extension", "restriction", "list", "union", "#all" }, new long[] { 0L, 1L, 2L, 4L, 8L, 16L, 255L }, "System.Xml.Schema.XmlSchemaDerivationMethod");
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x00026328 File Offset: 0x00025328
		private void Write62_XmlSchemaComplexType(string n, string ns, XmlSchemaComplexType o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaComplexType))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaComplexType", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("name", "", o.Name);
			if (o.Final != XmlSchemaDerivationMethod.None)
			{
				base.WriteAttribute("final", "", this.Write7_XmlSchemaDerivationMethod(o.Final));
			}
			if (o.IsAbstract)
			{
				base.WriteAttribute("abstract", "", XmlConvert.ToString(o.IsAbstract));
			}
			if (o.Block != XmlSchemaDerivationMethod.None)
			{
				base.WriteAttribute("block", "", this.Write7_XmlSchemaDerivationMethod(o.Block));
			}
			if (o.IsMixed)
			{
				base.WriteAttribute("mixed", "", XmlConvert.ToString(o.IsMixed));
			}
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			if (o.ContentModel is XmlSchemaSimpleContent)
			{
				this.Write61_XmlSchemaSimpleContent("simpleContent", "http://www.w3.org/2001/XMLSchema", (XmlSchemaSimpleContent)o.ContentModel, false, false);
			}
			else if (o.ContentModel is XmlSchemaComplexContent)
			{
				this.Write58_XmlSchemaComplexContent("complexContent", "http://www.w3.org/2001/XMLSchema", (XmlSchemaComplexContent)o.ContentModel, false, false);
			}
			else if (o.ContentModel != null)
			{
				throw base.CreateUnknownTypeException(o.ContentModel);
			}
			if (o.Particle is XmlSchemaChoice)
			{
				this.Write54_XmlSchemaChoice("choice", "http://www.w3.org/2001/XMLSchema", (XmlSchemaChoice)o.Particle, false, false);
			}
			else if (o.Particle is XmlSchemaAll)
			{
				this.Write55_XmlSchemaAll("all", "http://www.w3.org/2001/XMLSchema", (XmlSchemaAll)o.Particle, false, false);
			}
			else if (o.Particle is XmlSchemaSequence)
			{
				this.Write53_XmlSchemaSequence("sequence", "http://www.w3.org/2001/XMLSchema", (XmlSchemaSequence)o.Particle, false, false);
			}
			else if (o.Particle is XmlSchemaGroupRef)
			{
				this.Write44_XmlSchemaGroupRef("group", "http://www.w3.org/2001/XMLSchema", (XmlSchemaGroupRef)o.Particle, false, false);
			}
			else if (o.Particle != null)
			{
				throw base.CreateUnknownTypeException(o.Particle);
			}
			XmlSchemaObjectCollection attributes = o.Attributes;
			if (attributes != null)
			{
				for (int j = 0; j < ((ICollection)attributes).Count; j++)
				{
					XmlSchemaObject xmlSchemaObject = attributes[j];
					if (xmlSchemaObject is XmlSchemaAttributeGroupRef)
					{
						this.Write37_XmlSchemaAttributeGroupRef("attributeGroup", "http://www.w3.org/2001/XMLSchema", (XmlSchemaAttributeGroupRef)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaAttribute)
					{
						this.Write36_XmlSchemaAttribute("attribute", "http://www.w3.org/2001/XMLSchema", (XmlSchemaAttribute)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject != null)
					{
						throw base.CreateUnknownTypeException(xmlSchemaObject);
					}
				}
			}
			this.Write39_XmlSchemaAnyAttribute("anyAttribute", "http://www.w3.org/2001/XMLSchema", o.AnyAttribute, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x00026668 File Offset: 0x00025668
		private void Write39_XmlSchemaAnyAttribute(string n, string ns, XmlSchemaAnyAttribute o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaAnyAttribute))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaAnyAttribute", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("namespace", "", o.Namespace);
			if (o.ProcessContents != XmlSchemaContentProcessing.None)
			{
				base.WriteAttribute("processContents", "", this.Write38_XmlSchemaContentProcessing(o.ProcessContents));
			}
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x00026764 File Offset: 0x00025764
		private void Write36_XmlSchemaAttribute(string n, string ns, XmlSchemaAttribute o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaAttribute))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaAttribute", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("default", "", o.DefaultValue);
			base.WriteAttribute("fixed", "", o.FixedValue);
			if (o.Form != XmlSchemaForm.None)
			{
				base.WriteAttribute("form", "", this.Write6_XmlSchemaForm(o.Form));
			}
			base.WriteAttribute("name", "", o.Name);
			base.WriteAttribute("ref", "", base.FromXmlQualifiedName(o.RefName));
			base.WriteAttribute("type", "", base.FromXmlQualifiedName(o.SchemaTypeName));
			if (o.Use != XmlSchemaUse.None)
			{
				base.WriteAttribute("use", "", this.Write35_XmlSchemaUse(o.Use));
			}
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			this.Write34_XmlSchemaSimpleType("simpleType", "http://www.w3.org/2001/XMLSchema", o.SchemaType, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x00026900 File Offset: 0x00025900
		private string Write35_XmlSchemaUse(XmlSchemaUse v)
		{
			string text;
			switch (v)
			{
			case XmlSchemaUse.Optional:
				text = "optional";
				break;
			case XmlSchemaUse.Prohibited:
				text = "prohibited";
				break;
			case XmlSchemaUse.Required:
				text = "required";
				break;
			default:
				throw base.CreateInvalidEnumValueException(((long)v).ToString(CultureInfo.InvariantCulture), "System.Xml.Schema.XmlSchemaUse");
			}
			return text;
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x0002695C File Offset: 0x0002595C
		private string Write6_XmlSchemaForm(XmlSchemaForm v)
		{
			string text;
			switch (v)
			{
			case XmlSchemaForm.Qualified:
				text = "qualified";
				break;
			case XmlSchemaForm.Unqualified:
				text = "unqualified";
				break;
			default:
				throw base.CreateInvalidEnumValueException(((long)v).ToString(CultureInfo.InvariantCulture), "System.Xml.Schema.XmlSchemaForm");
			}
			return text;
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x000269AC File Offset: 0x000259AC
		private void Write37_XmlSchemaAttributeGroupRef(string n, string ns, XmlSchemaAttributeGroupRef o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaAttributeGroupRef))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaAttributeGroupRef", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("ref", "", base.FromXmlQualifiedName(o.RefName));
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x00026A8C File Offset: 0x00025A8C
		private void Write44_XmlSchemaGroupRef(string n, string ns, XmlSchemaGroupRef o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaGroupRef))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaGroupRef", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("minOccurs", "", o.MinOccursString);
			base.WriteAttribute("maxOccurs", "", o.MaxOccursString);
			base.WriteAttribute("ref", "", base.FromXmlQualifiedName(o.RefName));
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x00026B98 File Offset: 0x00025B98
		private void Write55_XmlSchemaAll(string n, string ns, XmlSchemaAll o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaAll))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaAll", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("minOccurs", "", o.MinOccursString);
			base.WriteAttribute("maxOccurs", "", o.MaxOccursString);
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			XmlSchemaObjectCollection items = o.Items;
			if (items != null)
			{
				for (int j = 0; j < ((ICollection)items).Count; j++)
				{
					this.Write52_XmlSchemaElement("element", "http://www.w3.org/2001/XMLSchema", (XmlSchemaElement)items[j], false, false);
				}
			}
			base.WriteEndElement(o);
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x00026CC8 File Offset: 0x00025CC8
		private void Write54_XmlSchemaChoice(string n, string ns, XmlSchemaChoice o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaChoice))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaChoice", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("minOccurs", "", o.MinOccursString);
			base.WriteAttribute("maxOccurs", "", o.MaxOccursString);
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			XmlSchemaObjectCollection items = o.Items;
			if (items != null)
			{
				for (int j = 0; j < ((ICollection)items).Count; j++)
				{
					XmlSchemaObject xmlSchemaObject = items[j];
					if (xmlSchemaObject is XmlSchemaSequence)
					{
						this.Write53_XmlSchemaSequence("sequence", "http://www.w3.org/2001/XMLSchema", (XmlSchemaSequence)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaChoice)
					{
						this.Write54_XmlSchemaChoice("choice", "http://www.w3.org/2001/XMLSchema", (XmlSchemaChoice)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaGroupRef)
					{
						this.Write44_XmlSchemaGroupRef("group", "http://www.w3.org/2001/XMLSchema", (XmlSchemaGroupRef)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaElement)
					{
						this.Write52_XmlSchemaElement("element", "http://www.w3.org/2001/XMLSchema", (XmlSchemaElement)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaAny)
					{
						this.Write46_XmlSchemaAny("any", "http://www.w3.org/2001/XMLSchema", (XmlSchemaAny)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject != null)
					{
						throw base.CreateUnknownTypeException(xmlSchemaObject);
					}
				}
			}
			base.WriteEndElement(o);
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x00026EB0 File Offset: 0x00025EB0
		private void Write58_XmlSchemaComplexContent(string n, string ns, XmlSchemaComplexContent o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaComplexContent))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaComplexContent", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("mixed", "", XmlConvert.ToString(o.IsMixed));
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			if (o.Content is XmlSchemaComplexContentRestriction)
			{
				this.Write57_Item("restriction", "http://www.w3.org/2001/XMLSchema", (XmlSchemaComplexContentRestriction)o.Content, false, false);
			}
			else if (o.Content is XmlSchemaComplexContentExtension)
			{
				this.Write56_Item("extension", "http://www.w3.org/2001/XMLSchema", (XmlSchemaComplexContentExtension)o.Content, false, false);
			}
			else if (o.Content != null)
			{
				throw base.CreateUnknownTypeException(o.Content);
			}
			base.WriteEndElement(o);
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x00026FFC File Offset: 0x00025FFC
		private void Write56_Item(string n, string ns, XmlSchemaComplexContentExtension o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaComplexContentExtension))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaComplexContentExtension", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("base", "", base.FromXmlQualifiedName(o.BaseTypeName));
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			if (o.Particle is XmlSchemaAll)
			{
				this.Write55_XmlSchemaAll("all", "http://www.w3.org/2001/XMLSchema", (XmlSchemaAll)o.Particle, false, false);
			}
			else if (o.Particle is XmlSchemaSequence)
			{
				this.Write53_XmlSchemaSequence("sequence", "http://www.w3.org/2001/XMLSchema", (XmlSchemaSequence)o.Particle, false, false);
			}
			else if (o.Particle is XmlSchemaChoice)
			{
				this.Write54_XmlSchemaChoice("choice", "http://www.w3.org/2001/XMLSchema", (XmlSchemaChoice)o.Particle, false, false);
			}
			else if (o.Particle is XmlSchemaGroupRef)
			{
				this.Write44_XmlSchemaGroupRef("group", "http://www.w3.org/2001/XMLSchema", (XmlSchemaGroupRef)o.Particle, false, false);
			}
			else if (o.Particle != null)
			{
				throw base.CreateUnknownTypeException(o.Particle);
			}
			XmlSchemaObjectCollection attributes = o.Attributes;
			if (attributes != null)
			{
				for (int j = 0; j < ((ICollection)attributes).Count; j++)
				{
					XmlSchemaObject xmlSchemaObject = attributes[j];
					if (xmlSchemaObject is XmlSchemaAttribute)
					{
						this.Write36_XmlSchemaAttribute("attribute", "http://www.w3.org/2001/XMLSchema", (XmlSchemaAttribute)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaAttributeGroupRef)
					{
						this.Write37_XmlSchemaAttributeGroupRef("attributeGroup", "http://www.w3.org/2001/XMLSchema", (XmlSchemaAttributeGroupRef)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject != null)
					{
						throw base.CreateUnknownTypeException(xmlSchemaObject);
					}
				}
			}
			this.Write39_XmlSchemaAnyAttribute("anyAttribute", "http://www.w3.org/2001/XMLSchema", o.AnyAttribute, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x0002723C File Offset: 0x0002623C
		private void Write57_Item(string n, string ns, XmlSchemaComplexContentRestriction o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaComplexContentRestriction))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaComplexContentRestriction", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("base", "", base.FromXmlQualifiedName(o.BaseTypeName));
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			if (o.Particle is XmlSchemaAll)
			{
				this.Write55_XmlSchemaAll("all", "http://www.w3.org/2001/XMLSchema", (XmlSchemaAll)o.Particle, false, false);
			}
			else if (o.Particle is XmlSchemaSequence)
			{
				this.Write53_XmlSchemaSequence("sequence", "http://www.w3.org/2001/XMLSchema", (XmlSchemaSequence)o.Particle, false, false);
			}
			else if (o.Particle is XmlSchemaChoice)
			{
				this.Write54_XmlSchemaChoice("choice", "http://www.w3.org/2001/XMLSchema", (XmlSchemaChoice)o.Particle, false, false);
			}
			else if (o.Particle is XmlSchemaGroupRef)
			{
				this.Write44_XmlSchemaGroupRef("group", "http://www.w3.org/2001/XMLSchema", (XmlSchemaGroupRef)o.Particle, false, false);
			}
			else if (o.Particle != null)
			{
				throw base.CreateUnknownTypeException(o.Particle);
			}
			XmlSchemaObjectCollection attributes = o.Attributes;
			if (attributes != null)
			{
				for (int j = 0; j < ((ICollection)attributes).Count; j++)
				{
					XmlSchemaObject xmlSchemaObject = attributes[j];
					if (xmlSchemaObject is XmlSchemaAttribute)
					{
						this.Write36_XmlSchemaAttribute("attribute", "http://www.w3.org/2001/XMLSchema", (XmlSchemaAttribute)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaAttributeGroupRef)
					{
						this.Write37_XmlSchemaAttributeGroupRef("attributeGroup", "http://www.w3.org/2001/XMLSchema", (XmlSchemaAttributeGroupRef)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject != null)
					{
						throw base.CreateUnknownTypeException(xmlSchemaObject);
					}
				}
			}
			this.Write39_XmlSchemaAnyAttribute("anyAttribute", "http://www.w3.org/2001/XMLSchema", o.AnyAttribute, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0002747C File Offset: 0x0002647C
		private void Write61_XmlSchemaSimpleContent(string n, string ns, XmlSchemaSimpleContent o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaSimpleContent))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaSimpleContent", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			if (o.Content is XmlSchemaSimpleContentExtension)
			{
				this.Write60_Item("extension", "http://www.w3.org/2001/XMLSchema", (XmlSchemaSimpleContentExtension)o.Content, false, false);
			}
			else if (o.Content is XmlSchemaSimpleContentRestriction)
			{
				this.Write59_Item("restriction", "http://www.w3.org/2001/XMLSchema", (XmlSchemaSimpleContentRestriction)o.Content, false, false);
			}
			else if (o.Content != null)
			{
				throw base.CreateUnknownTypeException(o.Content);
			}
			base.WriteEndElement(o);
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x000275AC File Offset: 0x000265AC
		private void Write59_Item(string n, string ns, XmlSchemaSimpleContentRestriction o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaSimpleContentRestriction))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaSimpleContentRestriction", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("base", "", base.FromXmlQualifiedName(o.BaseTypeName));
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			this.Write34_XmlSchemaSimpleType("simpleType", "http://www.w3.org/2001/XMLSchema", o.BaseType, false, false);
			XmlSchemaObjectCollection facets = o.Facets;
			if (facets != null)
			{
				for (int j = 0; j < ((ICollection)facets).Count; j++)
				{
					XmlSchemaObject xmlSchemaObject = facets[j];
					if (xmlSchemaObject is XmlSchemaMinLengthFacet)
					{
						this.Write31_XmlSchemaMinLengthFacet("minLength", "http://www.w3.org/2001/XMLSchema", (XmlSchemaMinLengthFacet)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaMaxLengthFacet)
					{
						this.Write22_XmlSchemaMaxLengthFacet("maxLength", "http://www.w3.org/2001/XMLSchema", (XmlSchemaMaxLengthFacet)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaLengthFacet)
					{
						this.Write23_XmlSchemaLengthFacet("length", "http://www.w3.org/2001/XMLSchema", (XmlSchemaLengthFacet)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaFractionDigitsFacet)
					{
						this.Write20_XmlSchemaFractionDigitsFacet("fractionDigits", "http://www.w3.org/2001/XMLSchema", (XmlSchemaFractionDigitsFacet)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaTotalDigitsFacet)
					{
						this.Write24_XmlSchemaTotalDigitsFacet("totalDigits", "http://www.w3.org/2001/XMLSchema", (XmlSchemaTotalDigitsFacet)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaMinExclusiveFacet)
					{
						this.Write30_XmlSchemaMinExclusiveFacet("minExclusive", "http://www.w3.org/2001/XMLSchema", (XmlSchemaMinExclusiveFacet)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaMaxInclusiveFacet)
					{
						this.Write27_XmlSchemaMaxInclusiveFacet("maxInclusive", "http://www.w3.org/2001/XMLSchema", (XmlSchemaMaxInclusiveFacet)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaMaxExclusiveFacet)
					{
						this.Write28_XmlSchemaMaxExclusiveFacet("maxExclusive", "http://www.w3.org/2001/XMLSchema", (XmlSchemaMaxExclusiveFacet)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaMinInclusiveFacet)
					{
						this.Write21_XmlSchemaMinInclusiveFacet("minInclusive", "http://www.w3.org/2001/XMLSchema", (XmlSchemaMinInclusiveFacet)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaWhiteSpaceFacet)
					{
						this.Write29_XmlSchemaWhiteSpaceFacet("whiteSpace", "http://www.w3.org/2001/XMLSchema", (XmlSchemaWhiteSpaceFacet)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaEnumerationFacet)
					{
						this.Write26_XmlSchemaEnumerationFacet("enumeration", "http://www.w3.org/2001/XMLSchema", (XmlSchemaEnumerationFacet)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaPatternFacet)
					{
						this.Write25_XmlSchemaPatternFacet("pattern", "http://www.w3.org/2001/XMLSchema", (XmlSchemaPatternFacet)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject != null)
					{
						throw base.CreateUnknownTypeException(xmlSchemaObject);
					}
				}
			}
			XmlSchemaObjectCollection attributes = o.Attributes;
			if (attributes != null)
			{
				for (int k = 0; k < ((ICollection)attributes).Count; k++)
				{
					XmlSchemaObject xmlSchemaObject2 = attributes[k];
					if (xmlSchemaObject2 is XmlSchemaAttribute)
					{
						this.Write36_XmlSchemaAttribute("attribute", "http://www.w3.org/2001/XMLSchema", (XmlSchemaAttribute)xmlSchemaObject2, false, false);
					}
					else if (xmlSchemaObject2 is XmlSchemaAttributeGroupRef)
					{
						this.Write37_XmlSchemaAttributeGroupRef("attributeGroup", "http://www.w3.org/2001/XMLSchema", (XmlSchemaAttributeGroupRef)xmlSchemaObject2, false, false);
					}
					else if (xmlSchemaObject2 != null)
					{
						throw base.CreateUnknownTypeException(xmlSchemaObject2);
					}
				}
			}
			this.Write39_XmlSchemaAnyAttribute("anyAttribute", "http://www.w3.org/2001/XMLSchema", o.AnyAttribute, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x00027948 File Offset: 0x00026948
		private void Write60_Item(string n, string ns, XmlSchemaSimpleContentExtension o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaSimpleContentExtension))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaSimpleContentExtension", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("base", "", base.FromXmlQualifiedName(o.BaseTypeName));
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			XmlSchemaObjectCollection attributes = o.Attributes;
			if (attributes != null)
			{
				for (int j = 0; j < ((ICollection)attributes).Count; j++)
				{
					XmlSchemaObject xmlSchemaObject = attributes[j];
					if (xmlSchemaObject is XmlSchemaAttribute)
					{
						this.Write36_XmlSchemaAttribute("attribute", "http://www.w3.org/2001/XMLSchema", (XmlSchemaAttribute)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaAttributeGroupRef)
					{
						this.Write37_XmlSchemaAttributeGroupRef("attributeGroup", "http://www.w3.org/2001/XMLSchema", (XmlSchemaAttributeGroupRef)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject != null)
					{
						throw base.CreateUnknownTypeException(xmlSchemaObject);
					}
				}
			}
			this.Write39_XmlSchemaAnyAttribute("anyAttribute", "http://www.w3.org/2001/XMLSchema", o.AnyAttribute, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x00027AC0 File Offset: 0x00026AC0
		private void Write65_XmlSchemaNotation(string n, string ns, XmlSchemaNotation o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaNotation))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaNotation", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("name", "", o.Name);
			base.WriteAttribute("public", "", o.Public);
			base.WriteAttribute("system", "", o.System);
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x00027BC4 File Offset: 0x00026BC4
		private void Write40_XmlSchemaAttributeGroup(string n, string ns, XmlSchemaAttributeGroup o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaAttributeGroup))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaAttributeGroup", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("name", "", o.Name);
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			XmlSchemaObjectCollection attributes = o.Attributes;
			if (attributes != null)
			{
				for (int j = 0; j < ((ICollection)attributes).Count; j++)
				{
					XmlSchemaObject xmlSchemaObject = attributes[j];
					if (xmlSchemaObject is XmlSchemaAttributeGroupRef)
					{
						this.Write37_XmlSchemaAttributeGroupRef("attributeGroup", "http://www.w3.org/2001/XMLSchema", (XmlSchemaAttributeGroupRef)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaAttribute)
					{
						this.Write36_XmlSchemaAttribute("attribute", "http://www.w3.org/2001/XMLSchema", (XmlSchemaAttribute)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject != null)
					{
						throw base.CreateUnknownTypeException(xmlSchemaObject);
					}
				}
			}
			this.Write39_XmlSchemaAnyAttribute("anyAttribute", "http://www.w3.org/2001/XMLSchema", o.AnyAttribute, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x00027D38 File Offset: 0x00026D38
		private void Write12_XmlSchemaInclude(string n, string ns, XmlSchemaInclude o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaInclude))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaInclude", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("schemaLocation", "", o.SchemaLocation);
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x00027E10 File Offset: 0x00026E10
		private void Write13_XmlSchemaImport(string n, string ns, XmlSchemaImport o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaImport))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaImport", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("schemaLocation", "", o.SchemaLocation);
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("namespace", "", o.Namespace);
			this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", o.Annotation, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x00027F00 File Offset: 0x00026F00
		private void Write64_XmlSchemaRedefine(string n, string ns, XmlSchemaRedefine o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(XmlSchemaRedefine))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("XmlSchemaRedefine", "http://www.w3.org/2001/XMLSchema");
			}
			base.WriteAttribute("schemaLocation", "", o.SchemaLocation);
			base.WriteAttribute("id", "", o.Id);
			XmlAttribute[] unhandledAttributes = o.UnhandledAttributes;
			if (unhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in unhandledAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			XmlSchemaObjectCollection items = o.Items;
			if (items != null)
			{
				for (int j = 0; j < ((ICollection)items).Count; j++)
				{
					XmlSchemaObject xmlSchemaObject = items[j];
					if (xmlSchemaObject is XmlSchemaSimpleType)
					{
						this.Write34_XmlSchemaSimpleType("simpleType", "http://www.w3.org/2001/XMLSchema", (XmlSchemaSimpleType)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaComplexType)
					{
						this.Write62_XmlSchemaComplexType("complexType", "http://www.w3.org/2001/XMLSchema", (XmlSchemaComplexType)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaGroup)
					{
						this.Write63_XmlSchemaGroup("group", "http://www.w3.org/2001/XMLSchema", (XmlSchemaGroup)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaAttributeGroup)
					{
						this.Write40_XmlSchemaAttributeGroup("attributeGroup", "http://www.w3.org/2001/XMLSchema", (XmlSchemaAttributeGroup)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject is XmlSchemaAnnotation)
					{
						this.Write11_XmlSchemaAnnotation("annotation", "http://www.w3.org/2001/XMLSchema", (XmlSchemaAnnotation)xmlSchemaObject, false, false);
					}
					else if (xmlSchemaObject != null)
					{
						throw base.CreateUnknownTypeException(xmlSchemaObject);
					}
				}
			}
			base.WriteEndElement(o);
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x000280BC File Offset: 0x000270BC
		private void Write4_Import(string n, string ns, Import o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(Import))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, o.Namespaces);
			if (needType)
			{
				base.WriteXsiType("Import", "http://schemas.xmlsoap.org/wsdl/");
			}
			XmlAttribute[] extensibleAttributes = o.ExtensibleAttributes;
			if (extensibleAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in extensibleAttributes)
				{
					base.WriteXmlAttribute(xmlAttribute, o);
				}
			}
			base.WriteAttribute("namespace", "", o.Namespace);
			base.WriteAttribute("location", "", o.Location);
			if (o.DocumentationElement != null || o.DocumentationElement == null)
			{
				base.WriteElementLiteral(o.DocumentationElement, "documentation", "http://schemas.xmlsoap.org/wsdl/", false, true);
				ServiceDescriptionFormatExtensionCollection extensions = o.Extensions;
				if (extensions != null)
				{
					for (int j = 0; j < ((ICollection)extensions).Count; j++)
					{
						if (!(extensions[j] is XmlNode) && extensions[j] != null)
						{
							throw base.CreateInvalidAnyTypeException(extensions[j]);
						}
						base.WriteElementLiteral((XmlNode)extensions[j], "", null, false, true);
					}
				}
				base.WriteEndElement(o);
				return;
			}
			throw base.CreateInvalidAnyTypeException(o.DocumentationElement);
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x00028216 File Offset: 0x00027216
		protected override void InitCallbacks()
		{
		}
	}
}
