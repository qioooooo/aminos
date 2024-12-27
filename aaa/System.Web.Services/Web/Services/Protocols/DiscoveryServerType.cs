using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Web.Services.Description;
using System.Web.Services.Discovery;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200002D RID: 45
	internal class DiscoveryServerType : ServerType
	{
		// Token: 0x060000F0 RID: 240 RVA: 0x000041FC File Offset: 0x000031FC
		internal DiscoveryServerType(Type type, string uri)
			: base(typeof(DiscoveryServerProtocol))
		{
			Uri uri2 = new Uri(uri, true);
			uri = uri2.GetLeftPart(UriPartial.Path);
			this.methodInfo = new LogicalMethodInfo(typeof(DiscoveryServerProtocol).GetMethod("Discover", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));
			ServiceDescriptionReflector serviceDescriptionReflector = new ServiceDescriptionReflector();
			serviceDescriptionReflector.Reflect(type, uri);
			XmlSchemas schemas = serviceDescriptionReflector.Schemas;
			this.description = serviceDescriptionReflector.ServiceDescription;
			XmlSerializer serializer = ServiceDescription.Serializer;
			this.AddSchemaImports(schemas, uri, serviceDescriptionReflector.ServiceDescriptions);
			for (int i = 1; i < serviceDescriptionReflector.ServiceDescriptions.Count; i++)
			{
				ServiceDescription serviceDescription = serviceDescriptionReflector.ServiceDescriptions[i];
				Import import = new Import();
				import.Namespace = serviceDescription.TargetNamespace;
				string text = "wsdl" + i.ToString(CultureInfo.InvariantCulture);
				import.Location = uri + "?wsdl=" + text;
				serviceDescriptionReflector.ServiceDescription.Imports.Add(import);
				this.wsdlTable.Add(text, serviceDescription);
			}
			this.discoDoc = new DiscoveryDocument();
			this.discoDoc.References.Add(new ContractReference(uri + "?wsdl", uri));
			foreach (object obj in serviceDescriptionReflector.ServiceDescription.Services)
			{
				Service service = (Service)obj;
				foreach (object obj2 in service.Ports)
				{
					Port port = (Port)obj2;
					SoapAddressBinding soapAddressBinding = (SoapAddressBinding)port.Extensions.Find(typeof(SoapAddressBinding));
					if (soapAddressBinding != null)
					{
						global::System.Web.Services.Discovery.SoapBinding soapBinding = new global::System.Web.Services.Discovery.SoapBinding();
						soapBinding.Binding = port.Binding;
						soapBinding.Address = soapAddressBinding.Location;
						this.discoDoc.References.Add(soapBinding);
					}
				}
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00004448 File Offset: 0x00003448
		internal void AddExternal(XmlSchema schema, string ns, string location)
		{
			if (schema == null)
			{
				return;
			}
			if (schema.TargetNamespace == ns)
			{
				XmlSchemaInclude xmlSchemaInclude = new XmlSchemaInclude();
				xmlSchemaInclude.SchemaLocation = location;
				schema.Includes.Add(xmlSchemaInclude);
				return;
			}
			XmlSchemaImport xmlSchemaImport = new XmlSchemaImport();
			xmlSchemaImport.SchemaLocation = location;
			xmlSchemaImport.Namespace = ns;
			schema.Includes.Add(xmlSchemaImport);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000044A4 File Offset: 0x000034A4
		private void AddSchemaImports(XmlSchemas schemas, string uri, ServiceDescriptionCollection descriptions)
		{
			int num = 0;
			foreach (object obj in schemas)
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				if (xmlSchema != null)
				{
					if (xmlSchema.Id == null || xmlSchema.Id.Length == 0)
					{
						XmlSchema xmlSchema2 = xmlSchema;
						string text = "schema";
						int num2;
						num = (num2 = num + 1);
						xmlSchema2.Id = text + num2.ToString(CultureInfo.InvariantCulture);
					}
					string text2 = uri + "?schema=" + xmlSchema.Id;
					foreach (object obj2 in descriptions)
					{
						ServiceDescription serviceDescription = (ServiceDescription)obj2;
						if (serviceDescription.Types.Schemas.Count == 0)
						{
							XmlSchema xmlSchema3 = new XmlSchema();
							xmlSchema3.TargetNamespace = serviceDescription.TargetNamespace;
							xmlSchema.ElementFormDefault = XmlSchemaForm.Qualified;
							this.AddExternal(xmlSchema3, xmlSchema.TargetNamespace, text2);
							serviceDescription.Types.Schemas.Add(xmlSchema3);
						}
						else
						{
							this.AddExternal(serviceDescription.Types.Schemas[0], xmlSchema.TargetNamespace, text2);
						}
					}
					this.schemaTable.Add(xmlSchema.Id, xmlSchema);
				}
			}
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00004630 File Offset: 0x00003630
		internal XmlSchema GetSchema(string id)
		{
			return (XmlSchema)this.schemaTable[id];
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00004643 File Offset: 0x00003643
		internal ServiceDescription GetServiceDescription(string id)
		{
			return (ServiceDescription)this.wsdlTable[id];
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00004656 File Offset: 0x00003656
		internal ServiceDescription Description
		{
			get
			{
				return this.description;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x0000465E File Offset: 0x0000365E
		internal LogicalMethodInfo MethodInfo
		{
			get
			{
				return this.methodInfo;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00004666 File Offset: 0x00003666
		internal DiscoveryDocument Disco
		{
			get
			{
				return this.discoDoc;
			}
		}

		// Token: 0x04000266 RID: 614
		private ServiceDescription description;

		// Token: 0x04000267 RID: 615
		private LogicalMethodInfo methodInfo;

		// Token: 0x04000268 RID: 616
		private Hashtable schemaTable = new Hashtable();

		// Token: 0x04000269 RID: 617
		private Hashtable wsdlTable = new Hashtable();

		// Token: 0x0400026A RID: 618
		private DiscoveryDocument discoDoc;
	}
}
