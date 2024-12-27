using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Web.Services.Configuration;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Microsoft.CSharp;

namespace System.Web.Services.Description
{
	// Token: 0x02000105 RID: 261
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class ServiceDescriptionImporter
	{
		// Token: 0x0600071F RID: 1823 RVA: 0x0001EA60 File Offset: 0x0001DA60
		public ServiceDescriptionImporter()
		{
			Type[] protocolImporterTypes = WebServicesSection.Current.ProtocolImporterTypes;
			this.importers = new ProtocolImporter[protocolImporterTypes.Length];
			for (int i = 0; i < this.importers.Length; i++)
			{
				this.importers[i] = (ProtocolImporter)Activator.CreateInstance(protocolImporterTypes[i]);
				this.importers[i].Initialize(this);
			}
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x0001EB00 File Offset: 0x0001DB00
		internal ServiceDescriptionImporter(CodeCompileUnit codeCompileUnit)
			: this()
		{
			this.codeCompileUnit = codeCompileUnit;
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000721 RID: 1825 RVA: 0x0001EB0F File Offset: 0x0001DB0F
		public ServiceDescriptionCollection ServiceDescriptions
		{
			get
			{
				return this.serviceDescriptions;
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000722 RID: 1826 RVA: 0x0001EB17 File Offset: 0x0001DB17
		public XmlSchemas Schemas
		{
			get
			{
				return this.schemas;
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000723 RID: 1827 RVA: 0x0001EB1F File Offset: 0x0001DB1F
		// (set) Token: 0x06000724 RID: 1828 RVA: 0x0001EB27 File Offset: 0x0001DB27
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

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000725 RID: 1829 RVA: 0x0001EB30 File Offset: 0x0001DB30
		// (set) Token: 0x06000726 RID: 1830 RVA: 0x0001EB38 File Offset: 0x0001DB38
		[ComVisible(false)]
		public CodeGenerationOptions CodeGenerationOptions
		{
			get
			{
				return this.options;
			}
			set
			{
				this.options = value;
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000727 RID: 1831 RVA: 0x0001EB41 File Offset: 0x0001DB41
		internal CodeCompileUnit CodeCompileUnit
		{
			get
			{
				return this.codeCompileUnit;
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000728 RID: 1832 RVA: 0x0001EB49 File Offset: 0x0001DB49
		// (set) Token: 0x06000729 RID: 1833 RVA: 0x0001EB64 File Offset: 0x0001DB64
		[ComVisible(false)]
		public CodeDomProvider CodeGenerator
		{
			get
			{
				if (this.codeProvider == null)
				{
					this.codeProvider = new CSharpCodeProvider();
				}
				return this.codeProvider;
			}
			set
			{
				this.codeProvider = value;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x0600072A RID: 1834 RVA: 0x0001EB6D File Offset: 0x0001DB6D
		internal List<Type> Extensions
		{
			get
			{
				if (this.extensions == null)
				{
					this.extensions = new List<Type>();
				}
				return this.extensions;
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x0600072B RID: 1835 RVA: 0x0001EB88 File Offset: 0x0001DB88
		// (set) Token: 0x0600072C RID: 1836 RVA: 0x0001EB9E File Offset: 0x0001DB9E
		public string ProtocolName
		{
			get
			{
				if (this.protocolName != null)
				{
					return this.protocolName;
				}
				return string.Empty;
			}
			set
			{
				this.protocolName = value;
			}
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x0001EBA8 File Offset: 0x0001DBA8
		private ProtocolImporter FindImporterByName(string protocolName)
		{
			for (int i = 0; i < this.importers.Length; i++)
			{
				ProtocolImporter protocolImporter = this.importers[i];
				if (string.Compare(this.ProtocolName, protocolImporter.ProtocolName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return protocolImporter;
				}
			}
			throw new ArgumentException(Res.GetString("ProtocolWithNameIsNotRecognized1", new object[] { protocolName }), "protocolName");
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x0600072E RID: 1838 RVA: 0x0001EC07 File Offset: 0x0001DC07
		internal XmlSchemas AllSchemas
		{
			get
			{
				return this.allSchemas;
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x0001EC0F File Offset: 0x0001DC0F
		internal XmlSchemas AbstractSchemas
		{
			get
			{
				return this.abstractSchemas;
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000730 RID: 1840 RVA: 0x0001EC17 File Offset: 0x0001DC17
		internal XmlSchemas ConcreteSchemas
		{
			get
			{
				return this.concreteSchemas;
			}
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x0001EC1F File Offset: 0x0001DC1F
		public void AddServiceDescription(ServiceDescription serviceDescription, string appSettingUrlKey, string appSettingBaseUrl)
		{
			if (serviceDescription == null)
			{
				throw new ArgumentNullException("serviceDescription");
			}
			serviceDescription.AppSettingUrlKey = appSettingUrlKey;
			serviceDescription.AppSettingBaseUrl = appSettingBaseUrl;
			this.ServiceDescriptions.Add(serviceDescription);
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x0001EC4C File Offset: 0x0001DC4C
		public ServiceDescriptionImportWarnings Import(CodeNamespace codeNamespace, CodeCompileUnit codeCompileUnit)
		{
			if (codeCompileUnit != null)
			{
				codeCompileUnit.ReferencedAssemblies.Add("System.dll");
				codeCompileUnit.ReferencedAssemblies.Add("System.Xml.dll");
				codeCompileUnit.ReferencedAssemblies.Add("System.Web.Services.dll");
				codeCompileUnit.ReferencedAssemblies.Add("System.EnterpriseServices.dll");
			}
			return this.Import(codeNamespace, new ImportContext(new CodeIdentifiers(), false), new Hashtable(), new StringCollection());
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0001ECBC File Offset: 0x0001DCBC
		public static StringCollection GenerateWebReferences(WebReferenceCollection webReferences, CodeDomProvider codeProvider, CodeCompileUnit codeCompileUnit, WebReferenceOptions options)
		{
			if (codeCompileUnit != null)
			{
				codeCompileUnit.ReferencedAssemblies.Add("System.dll");
				codeCompileUnit.ReferencedAssemblies.Add("System.Xml.dll");
				codeCompileUnit.ReferencedAssemblies.Add("System.Web.Services.dll");
				codeCompileUnit.ReferencedAssemblies.Add("System.EnterpriseServices.dll");
			}
			Hashtable hashtable = new Hashtable();
			Hashtable hashtable2 = new Hashtable();
			foreach (object obj in webReferences)
			{
				WebReference webReference = (WebReference)obj;
				ServiceDescriptionImporter serviceDescriptionImporter = new ServiceDescriptionImporter(codeCompileUnit);
				XmlSchemas xmlSchemas = new XmlSchemas();
				ServiceDescriptionCollection serviceDescriptionCollection = new ServiceDescriptionCollection();
				foreach (object obj2 in webReference.Documents)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
					ServiceDescriptionImporter.AddDocument((string)dictionaryEntry.Key, dictionaryEntry.Value, xmlSchemas, serviceDescriptionCollection, webReference.ValidationWarnings);
				}
				serviceDescriptionImporter.Schemas.Add(xmlSchemas);
				foreach (object obj3 in serviceDescriptionCollection)
				{
					ServiceDescription serviceDescription = (ServiceDescription)obj3;
					serviceDescriptionImporter.AddServiceDescription(serviceDescription, webReference.AppSettingUrlKey, webReference.AppSettingBaseUrl);
				}
				serviceDescriptionImporter.CodeGenerator = codeProvider;
				serviceDescriptionImporter.ProtocolName = webReference.ProtocolName;
				serviceDescriptionImporter.Style = options.Style;
				serviceDescriptionImporter.CodeGenerationOptions = options.CodeGenerationOptions;
				foreach (string text in options.SchemaImporterExtensions)
				{
					serviceDescriptionImporter.Extensions.Add(Type.GetType(text, true));
				}
				ImportContext importContext = ServiceDescriptionImporter.Context(webReference.ProxyCode, hashtable, options.Verbose);
				webReference.Warnings = serviceDescriptionImporter.Import(webReference.ProxyCode, importContext, hashtable2, webReference.ValidationWarnings);
				if (webReference.ValidationWarnings.Count != 0)
				{
					webReference.Warnings |= ServiceDescriptionImportWarnings.SchemaValidation;
				}
			}
			StringCollection stringCollection = new StringCollection();
			if (options.Verbose)
			{
				foreach (object obj4 in hashtable.Values)
				{
					ImportContext importContext2 = (ImportContext)obj4;
					foreach (string text2 in importContext2.Warnings)
					{
						stringCollection.Add(text2);
					}
				}
			}
			return stringCollection;
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0001F010 File Offset: 0x0001E010
		internal static ImportContext Context(CodeNamespace ns, Hashtable namespaces, bool verbose)
		{
			if (namespaces[ns.Name] == null)
			{
				namespaces[ns.Name] = new ImportContext(new CodeIdentifiers(), true);
			}
			return (ImportContext)namespaces[ns.Name];
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x0001F048 File Offset: 0x0001E048
		internal static void AddDocument(string path, object document, XmlSchemas schemas, ServiceDescriptionCollection descriptions, StringCollection warnings)
		{
			ServiceDescription serviceDescription = document as ServiceDescription;
			if (serviceDescription != null)
			{
				descriptions.Add(serviceDescription);
				return;
			}
			XmlSchema xmlSchema = document as XmlSchema;
			if (xmlSchema != null)
			{
				schemas.Add(xmlSchema);
			}
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0001F07C File Offset: 0x0001E07C
		private void FindUse(MessagePart part, out bool isEncoded, out bool isLiteral)
		{
			isEncoded = false;
			isLiteral = false;
			string name = part.Message.Name;
			Operation operation = null;
			ServiceDescription serviceDescription = part.Message.ServiceDescription;
			foreach (object obj in serviceDescription.PortTypes)
			{
				PortType portType = (PortType)obj;
				foreach (object obj2 in portType.Operations)
				{
					Operation operation2 = (Operation)obj2;
					foreach (object obj3 in operation2.Messages)
					{
						OperationMessage operationMessage = (OperationMessage)obj3;
						if (operationMessage.Message.Equals(new XmlQualifiedName(part.Message.Name, serviceDescription.TargetNamespace)))
						{
							operation = operation2;
							this.FindUse(operation, serviceDescription, name, ref isEncoded, ref isLiteral);
						}
					}
				}
			}
			if (operation == null)
			{
				this.FindUse(null, serviceDescription, name, ref isEncoded, ref isLiteral);
			}
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x0001F1D4 File Offset: 0x0001E1D4
		private void FindUse(Operation operation, ServiceDescription description, string messageName, ref bool isEncoded, ref bool isLiteral)
		{
			string targetNamespace = description.TargetNamespace;
			foreach (object obj in description.Bindings)
			{
				Binding binding = (Binding)obj;
				if (operation == null || new XmlQualifiedName(operation.PortType.Name, targetNamespace).Equals(binding.Type))
				{
					foreach (object obj2 in binding.Operations)
					{
						OperationBinding operationBinding = (OperationBinding)obj2;
						if (operationBinding.Input != null)
						{
							foreach (object obj3 in operationBinding.Input.Extensions)
							{
								if (operation != null)
								{
									SoapBodyBinding soapBodyBinding = obj3 as SoapBodyBinding;
									if (soapBodyBinding != null && operation.IsBoundBy(operationBinding))
									{
										if (soapBodyBinding.Use == SoapBindingUse.Encoded)
										{
											isEncoded = true;
										}
										else if (soapBodyBinding.Use == SoapBindingUse.Literal)
										{
											isLiteral = true;
										}
									}
								}
								else
								{
									SoapHeaderBinding soapHeaderBinding = obj3 as SoapHeaderBinding;
									if (soapHeaderBinding != null && soapHeaderBinding.Message.Name == messageName)
									{
										if (soapHeaderBinding.Use == SoapBindingUse.Encoded)
										{
											isEncoded = true;
										}
										else if (soapHeaderBinding.Use == SoapBindingUse.Literal)
										{
											isLiteral = true;
										}
									}
								}
							}
						}
						if (operationBinding.Output != null)
						{
							foreach (object obj4 in operationBinding.Output.Extensions)
							{
								if (operation != null)
								{
									if (operation.IsBoundBy(operationBinding))
									{
										SoapBodyBinding soapBodyBinding2 = obj4 as SoapBodyBinding;
										if (soapBodyBinding2 != null)
										{
											if (soapBodyBinding2.Use == SoapBindingUse.Encoded)
											{
												isEncoded = true;
											}
											else if (soapBodyBinding2.Use == SoapBindingUse.Literal)
											{
												isLiteral = true;
											}
										}
										else if (obj4 is MimeXmlBinding)
										{
											isLiteral = true;
										}
									}
								}
								else
								{
									SoapHeaderBinding soapHeaderBinding2 = obj4 as SoapHeaderBinding;
									if (soapHeaderBinding2 != null && soapHeaderBinding2.Message.Name == messageName)
									{
										if (soapHeaderBinding2.Use == SoapBindingUse.Encoded)
										{
											isEncoded = true;
										}
										else if (soapHeaderBinding2.Use == SoapBindingUse.Literal)
										{
											isLiteral = true;
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x0001F48C File Offset: 0x0001E48C
		private void AddImport(XmlSchema schema, Hashtable imports)
		{
			if (schema == null || imports[schema] != null)
			{
				return;
			}
			imports.Add(schema, schema);
			foreach (XmlSchemaObject xmlSchemaObject in schema.Includes)
			{
				XmlSchemaExternal xmlSchemaExternal = (XmlSchemaExternal)xmlSchemaObject;
				if (xmlSchemaExternal is XmlSchemaImport)
				{
					XmlSchemaImport xmlSchemaImport = (XmlSchemaImport)xmlSchemaExternal;
					foreach (object obj in this.allSchemas.GetSchemas(xmlSchemaImport.Namespace))
					{
						XmlSchema xmlSchema = (XmlSchema)obj;
						this.AddImport(xmlSchema, imports);
					}
				}
			}
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0001F560 File Offset: 0x0001E560
		private ServiceDescriptionImportWarnings Import(CodeNamespace codeNamespace, ImportContext importContext, Hashtable exportContext, StringCollection warnings)
		{
			this.allSchemas = new XmlSchemas();
			foreach (object obj in this.schemas)
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				this.allSchemas.Add(xmlSchema);
			}
			foreach (object obj2 in this.serviceDescriptions)
			{
				ServiceDescription serviceDescription = (ServiceDescription)obj2;
				foreach (object obj3 in serviceDescription.Types.Schemas)
				{
					XmlSchema xmlSchema2 = (XmlSchema)obj3;
					this.allSchemas.Add(xmlSchema2);
				}
			}
			Hashtable hashtable = new Hashtable();
			if (!this.allSchemas.Contains("http://schemas.xmlsoap.org/wsdl/"))
			{
				this.allSchemas.AddReference(ServiceDescription.Schema);
				hashtable[ServiceDescription.Schema] = ServiceDescription.Schema;
			}
			if (!this.allSchemas.Contains("http://schemas.xmlsoap.org/soap/encoding/"))
			{
				this.allSchemas.AddReference(ServiceDescription.SoapEncodingSchema);
				hashtable[ServiceDescription.SoapEncodingSchema] = ServiceDescription.SoapEncodingSchema;
			}
			this.allSchemas.Compile(null, false);
			foreach (object obj4 in this.serviceDescriptions)
			{
				ServiceDescription serviceDescription2 = (ServiceDescription)obj4;
				foreach (object obj5 in serviceDescription2.Messages)
				{
					Message message = (Message)obj5;
					foreach (object obj6 in message.Parts)
					{
						MessagePart messagePart = (MessagePart)obj6;
						bool flag;
						bool flag2;
						this.FindUse(messagePart, out flag, out flag2);
						if (messagePart.Element != null && !messagePart.Element.IsEmpty)
						{
							if (flag)
							{
								throw new InvalidOperationException(Res.GetString("CanTSpecifyElementOnEncodedMessagePartsPart", new object[] { messagePart.Name, message.Name }));
							}
							XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)this.allSchemas.Find(messagePart.Element, typeof(XmlSchemaElement));
							if (xmlSchemaElement != null)
							{
								ServiceDescriptionImporter.AddSchema(xmlSchemaElement.Parent as XmlSchema, flag, flag2, this.abstractSchemas, this.concreteSchemas, hashtable);
								if (xmlSchemaElement.SchemaTypeName != null && !xmlSchemaElement.SchemaTypeName.IsEmpty)
								{
									XmlSchemaType xmlSchemaType = (XmlSchemaType)this.allSchemas.Find(xmlSchemaElement.SchemaTypeName, typeof(XmlSchemaType));
									if (xmlSchemaType != null)
									{
										ServiceDescriptionImporter.AddSchema(xmlSchemaType.Parent as XmlSchema, flag, flag2, this.abstractSchemas, this.concreteSchemas, hashtable);
									}
								}
							}
						}
						if (messagePart.Type != null && !messagePart.Type.IsEmpty)
						{
							XmlSchemaType xmlSchemaType2 = (XmlSchemaType)this.allSchemas.Find(messagePart.Type, typeof(XmlSchemaType));
							if (xmlSchemaType2 != null)
							{
								ServiceDescriptionImporter.AddSchema(xmlSchemaType2.Parent as XmlSchema, flag, flag2, this.abstractSchemas, this.concreteSchemas, hashtable);
							}
						}
					}
				}
			}
			Hashtable hashtable2;
			foreach (XmlSchemas xmlSchemas in new XmlSchemas[] { this.abstractSchemas, this.concreteSchemas })
			{
				hashtable2 = new Hashtable();
				foreach (object obj7 in xmlSchemas)
				{
					XmlSchema xmlSchema3 = (XmlSchema)obj7;
					this.AddImport(xmlSchema3, hashtable2);
				}
				foreach (object obj8 in hashtable2.Keys)
				{
					XmlSchema xmlSchema4 = (XmlSchema)obj8;
					if (hashtable[xmlSchema4] == null && !xmlSchemas.Contains(xmlSchema4))
					{
						xmlSchemas.Add(xmlSchema4);
					}
				}
			}
			hashtable2 = new Hashtable();
			foreach (object obj9 in this.allSchemas)
			{
				XmlSchema xmlSchema5 = (XmlSchema)obj9;
				if (!this.abstractSchemas.Contains(xmlSchema5) && !this.concreteSchemas.Contains(xmlSchema5))
				{
					this.AddImport(xmlSchema5, hashtable2);
				}
			}
			foreach (object obj10 in hashtable2.Keys)
			{
				XmlSchema xmlSchema6 = (XmlSchema)obj10;
				if (hashtable[xmlSchema6] == null)
				{
					if (!this.abstractSchemas.Contains(xmlSchema6))
					{
						this.abstractSchemas.Add(xmlSchema6);
					}
					if (!this.concreteSchemas.Contains(xmlSchema6))
					{
						this.concreteSchemas.Add(xmlSchema6);
					}
				}
			}
			if (this.abstractSchemas.Count > 0)
			{
				foreach (object obj11 in hashtable.Values)
				{
					XmlSchema xmlSchema7 = (XmlSchema)obj11;
					this.abstractSchemas.AddReference(xmlSchema7);
				}
				StringCollection stringCollection = SchemaCompiler.Compile(this.abstractSchemas);
				foreach (string text in stringCollection)
				{
					warnings.Add(text);
				}
			}
			if (this.concreteSchemas.Count > 0)
			{
				foreach (object obj12 in hashtable.Values)
				{
					XmlSchema xmlSchema8 = (XmlSchema)obj12;
					this.concreteSchemas.AddReference(xmlSchema8);
				}
				StringCollection stringCollection2 = SchemaCompiler.Compile(this.concreteSchemas);
				foreach (string text2 in stringCollection2)
				{
					warnings.Add(text2);
				}
			}
			if (this.ProtocolName.Length > 0)
			{
				ProtocolImporter protocolImporter = this.FindImporterByName(this.ProtocolName);
				if (protocolImporter.GenerateCode(codeNamespace, importContext, exportContext))
				{
					return protocolImporter.Warnings;
				}
			}
			else
			{
				for (int j = 0; j < this.importers.Length; j++)
				{
					ProtocolImporter protocolImporter2 = this.importers[j];
					if (protocolImporter2.GenerateCode(codeNamespace, importContext, exportContext))
					{
						return protocolImporter2.Warnings;
					}
				}
			}
			return ServiceDescriptionImportWarnings.NoCodeGenerated;
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0001FDE4 File Offset: 0x0001EDE4
		private static void AddSchema(XmlSchema schema, bool isEncoded, bool isLiteral, XmlSchemas abstractSchemas, XmlSchemas concreteSchemas, Hashtable references)
		{
			if (schema != null)
			{
				if (isEncoded && !abstractSchemas.Contains(schema))
				{
					if (references.Contains(schema))
					{
						abstractSchemas.AddReference(schema);
					}
					else
					{
						abstractSchemas.Add(schema);
					}
				}
				if (isLiteral && !concreteSchemas.Contains(schema))
				{
					if (references.Contains(schema))
					{
						concreteSchemas.AddReference(schema);
						return;
					}
					concreteSchemas.Add(schema);
				}
			}
		}

		// Token: 0x04000498 RID: 1176
		private ServiceDescriptionImportStyle style;

		// Token: 0x04000499 RID: 1177
		private ServiceDescriptionCollection serviceDescriptions = new ServiceDescriptionCollection();

		// Token: 0x0400049A RID: 1178
		private XmlSchemas schemas = new XmlSchemas();

		// Token: 0x0400049B RID: 1179
		private XmlSchemas allSchemas = new XmlSchemas();

		// Token: 0x0400049C RID: 1180
		private string protocolName;

		// Token: 0x0400049D RID: 1181
		private CodeGenerationOptions options = CodeGenerationOptions.GenerateOldAsync;

		// Token: 0x0400049E RID: 1182
		private CodeCompileUnit codeCompileUnit;

		// Token: 0x0400049F RID: 1183
		private CodeDomProvider codeProvider;

		// Token: 0x040004A0 RID: 1184
		private ProtocolImporter[] importers;

		// Token: 0x040004A1 RID: 1185
		private XmlSchemas abstractSchemas = new XmlSchemas();

		// Token: 0x040004A2 RID: 1186
		private XmlSchemas concreteSchemas = new XmlSchemas();

		// Token: 0x040004A3 RID: 1187
		private List<Type> extensions;
	}
}
