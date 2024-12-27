using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Threading;
using System.Web.Services.Description;
using System.Web.Services.Discovery;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace System.Web.Services.Configuration
{
	// Token: 0x0200013D RID: 317
	public sealed class WebServicesSection : ConfigurationSection
	{
		// Token: 0x060009C8 RID: 2504 RVA: 0x000461F4 File Offset: 0x000451F4
		public WebServicesSection()
		{
			this.properties.Add(this.conformanceWarnings);
			this.properties.Add(this.protocols);
			this.properties.Add(this.serviceDescriptionFormatExtensionTypes);
			this.properties.Add(this.soapEnvelopeProcessing);
			this.properties.Add(this.soapExtensionImporterTypes);
			this.properties.Add(this.soapExtensionReflectorTypes);
			this.properties.Add(this.soapExtensionTypes);
			this.properties.Add(this.soapTransportImporterTypes);
			this.properties.Add(this.wsdlHelpGenerator);
			this.properties.Add(this.soapServerProtocolFactoryType);
			this.properties.Add(this.diagnostics);
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x060009C9 RID: 2505 RVA: 0x00046630 File Offset: 0x00045630
		private static object ClassSyncObject
		{
			get
			{
				if (WebServicesSection.classSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref WebServicesSection.classSyncObject, obj, null);
				}
				return WebServicesSection.classSyncObject;
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x060009CA RID: 2506 RVA: 0x0004665C File Offset: 0x0004565C
		[ConfigurationProperty("conformanceWarnings")]
		public WsiProfilesElementCollection ConformanceWarnings
		{
			get
			{
				return (WsiProfilesElementCollection)base[this.conformanceWarnings];
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x060009CB RID: 2507 RVA: 0x00046670 File Offset: 0x00045670
		internal WsiProfiles EnabledConformanceWarnings
		{
			get
			{
				WsiProfiles wsiProfiles = WsiProfiles.None;
				foreach (object obj in this.ConformanceWarnings)
				{
					WsiProfilesElement wsiProfilesElement = (WsiProfilesElement)obj;
					wsiProfiles |= wsiProfilesElement.Name;
				}
				return wsiProfiles;
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x060009CC RID: 2508 RVA: 0x000466D0 File Offset: 0x000456D0
		public static WebServicesSection Current
		{
			get
			{
				WebServicesSection webServicesSection = null;
				if (Thread.GetDomain().GetData(".appDomain") != null)
				{
					webServicesSection = WebServicesSection.GetConfigFromHttpContext();
				}
				if (webServicesSection == null)
				{
					webServicesSection = (WebServicesSection)PrivilegedConfigurationManager.GetSection("system.web/webServices");
				}
				return webServicesSection;
			}
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0004670C File Offset: 0x0004570C
		[ConfigurationPermission(SecurityAction.Assert, Unrestricted = true)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static WebServicesSection GetConfigFromHttpContext()
		{
			HttpContext httpContext = HttpContext.Current;
			if (httpContext != null)
			{
				return (WebServicesSection)httpContext.GetSection("system.web/webServices");
			}
			return null;
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x060009CE RID: 2510 RVA: 0x00046734 File Offset: 0x00045734
		internal XmlSerializer DiscoveryDocumentSerializer
		{
			get
			{
				if (this.discoveryDocumentSerializer == null)
				{
					lock (WebServicesSection.ClassSyncObject)
					{
						if (this.discoveryDocumentSerializer == null)
						{
							XmlAttributeOverrides xmlAttributeOverrides = new XmlAttributeOverrides();
							XmlAttributes xmlAttributes = new XmlAttributes();
							foreach (Type type in this.DiscoveryReferenceTypes)
							{
								object[] customAttributes = type.GetCustomAttributes(typeof(XmlRootAttribute), false);
								if (customAttributes.Length == 0)
								{
									throw new InvalidOperationException(Res.GetString("WebMissingCustomAttribute", new object[] { type.FullName, "XmlRoot" }));
								}
								string elementName = ((XmlRootAttribute)customAttributes[0]).ElementName;
								string @namespace = ((XmlRootAttribute)customAttributes[0]).Namespace;
								XmlElementAttribute xmlElementAttribute = new XmlElementAttribute(elementName, type);
								xmlElementAttribute.Namespace = @namespace;
								xmlAttributes.XmlElements.Add(xmlElementAttribute);
							}
							xmlAttributeOverrides.Add(typeof(DiscoveryDocument), "References", xmlAttributes);
							this.discoveryDocumentSerializer = new DiscoveryDocumentSerializer();
						}
					}
				}
				return this.discoveryDocumentSerializer;
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x060009CF RID: 2511 RVA: 0x0004685C File Offset: 0x0004585C
		internal Type[] DiscoveryReferenceTypes
		{
			get
			{
				return this.discoveryReferenceTypes;
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x060009D0 RID: 2512 RVA: 0x00046864 File Offset: 0x00045864
		public WebServiceProtocols EnabledProtocols
		{
			get
			{
				if (this.enabledProtocols == WebServiceProtocols.Unknown)
				{
					lock (WebServicesSection.ClassSyncObject)
					{
						if (this.enabledProtocols == WebServiceProtocols.Unknown)
						{
							WebServiceProtocols webServiceProtocols = WebServiceProtocols.Unknown;
							foreach (object obj2 in this.Protocols)
							{
								ProtocolElement protocolElement = (ProtocolElement)obj2;
								webServiceProtocols |= protocolElement.Name;
							}
							this.enabledProtocols = webServiceProtocols;
						}
					}
				}
				return this.enabledProtocols;
			}
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x00046904 File Offset: 0x00045904
		internal Type[] GetAllFormatExtensionTypes()
		{
			if (this.ServiceDescriptionFormatExtensionTypes.Count == 0)
			{
				return this.defaultFormatTypes;
			}
			Type[] array = new Type[this.defaultFormatTypes.Length + this.ServiceDescriptionFormatExtensionTypes.Count];
			Array.Copy(this.defaultFormatTypes, array, this.defaultFormatTypes.Length);
			for (int i = 0; i < this.ServiceDescriptionFormatExtensionTypes.Count; i++)
			{
				array[i + this.defaultFormatTypes.Length] = this.ServiceDescriptionFormatExtensionTypes[i].Type;
			}
			return array;
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x00046988 File Offset: 0x00045988
		private static XmlFormatExtensionPointAttribute GetExtensionPointAttribute(Type type)
		{
			object[] customAttributes = type.GetCustomAttributes(typeof(XmlFormatExtensionPointAttribute), false);
			if (customAttributes.Length == 0)
			{
				throw new ArgumentException(Res.GetString("TheSyntaxOfTypeMayNotBeExtended1", new object[] { type.FullName }), "type");
			}
			return (XmlFormatExtensionPointAttribute)customAttributes[0];
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x000469DA File Offset: 0x000459DA
		[ConfigurationPermission(SecurityAction.Assert, Unrestricted = true)]
		public static WebServicesSection GetSection(Configuration config)
		{
			if (config == null)
			{
				throw new ArgumentNullException("config");
			}
			return (WebServicesSection)config.GetSection("system.web/webServices");
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x000469FC File Offset: 0x000459FC
		protected override void InitializeDefault()
		{
			this.ConformanceWarnings.SetDefaults();
			this.Protocols.SetDefaults();
			if (Thread.GetDomain().GetData(".appDomain") != null)
			{
				this.WsdlHelpGenerator.SetDefaults();
			}
			this.SoapServerProtocolFactoryType.Type = typeof(SoapServerProtocolFactory);
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x00046A50 File Offset: 0x00045A50
		internal static void LoadXmlFormatExtensions(Type[] extensionTypes, XmlAttributeOverrides overrides, XmlSerializerNamespaces namespaces)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add(typeof(ServiceDescription), new XmlAttributes());
			hashtable.Add(typeof(Import), new XmlAttributes());
			hashtable.Add(typeof(Port), new XmlAttributes());
			hashtable.Add(typeof(Service), new XmlAttributes());
			hashtable.Add(typeof(FaultBinding), new XmlAttributes());
			hashtable.Add(typeof(InputBinding), new XmlAttributes());
			hashtable.Add(typeof(OutputBinding), new XmlAttributes());
			hashtable.Add(typeof(OperationBinding), new XmlAttributes());
			hashtable.Add(typeof(Binding), new XmlAttributes());
			hashtable.Add(typeof(OperationFault), new XmlAttributes());
			hashtable.Add(typeof(OperationInput), new XmlAttributes());
			hashtable.Add(typeof(OperationOutput), new XmlAttributes());
			hashtable.Add(typeof(Operation), new XmlAttributes());
			hashtable.Add(typeof(PortType), new XmlAttributes());
			hashtable.Add(typeof(Message), new XmlAttributes());
			hashtable.Add(typeof(MessagePart), new XmlAttributes());
			hashtable.Add(typeof(Types), new XmlAttributes());
			Hashtable hashtable2 = new Hashtable();
			foreach (Type type in extensionTypes)
			{
				if (hashtable2[type] == null)
				{
					hashtable2.Add(type, type);
					object[] array = type.GetCustomAttributes(typeof(XmlFormatExtensionAttribute), false);
					if (array.Length == 0)
					{
						throw new ArgumentException(Res.GetString("RequiredXmlFormatExtensionAttributeIsMissing1", new object[] { type.FullName }), "extensionTypes");
					}
					XmlFormatExtensionAttribute xmlFormatExtensionAttribute = (XmlFormatExtensionAttribute)array[0];
					foreach (Type type2 in xmlFormatExtensionAttribute.ExtensionPoints)
					{
						XmlAttributes xmlAttributes = (XmlAttributes)hashtable[type2];
						if (xmlAttributes == null)
						{
							xmlAttributes = new XmlAttributes();
							hashtable.Add(type2, xmlAttributes);
						}
						XmlElementAttribute xmlElementAttribute = new XmlElementAttribute(xmlFormatExtensionAttribute.ElementName, type);
						xmlElementAttribute.Namespace = xmlFormatExtensionAttribute.Namespace;
						xmlAttributes.XmlElements.Add(xmlElementAttribute);
					}
					array = type.GetCustomAttributes(typeof(XmlFormatExtensionPrefixAttribute), false);
					string[] array2 = new string[array.Length];
					Hashtable hashtable3 = new Hashtable();
					for (int k = 0; k < array.Length; k++)
					{
						XmlFormatExtensionPrefixAttribute xmlFormatExtensionPrefixAttribute = (XmlFormatExtensionPrefixAttribute)array[k];
						array2[k] = xmlFormatExtensionPrefixAttribute.Prefix;
						hashtable3.Add(xmlFormatExtensionPrefixAttribute.Prefix, xmlFormatExtensionPrefixAttribute.Namespace);
					}
					Array.Sort(array2, InvariantComparer.Default);
					for (int l = 0; l < array2.Length; l++)
					{
						namespaces.Add(array2[l], (string)hashtable3[array2[l]]);
					}
				}
			}
			foreach (object obj in hashtable.Keys)
			{
				Type type3 = (Type)obj;
				XmlFormatExtensionPointAttribute extensionPointAttribute = WebServicesSection.GetExtensionPointAttribute(type3);
				XmlAttributes xmlAttributes2 = (XmlAttributes)hashtable[type3];
				if (extensionPointAttribute.AllowElements)
				{
					xmlAttributes2.XmlAnyElements.Add(new XmlAnyElementAttribute());
				}
				overrides.Add(type3, extensionPointAttribute.MemberName, xmlAttributes2);
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x060009D6 RID: 2518 RVA: 0x00046DF0 File Offset: 0x00045DF0
		internal Type[] MimeImporterTypes
		{
			get
			{
				return this.mimeImporterTypes;
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x060009D7 RID: 2519 RVA: 0x00046DF8 File Offset: 0x00045DF8
		internal Type[] MimeReflectorTypes
		{
			get
			{
				return this.mimeReflectorTypes;
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x060009D8 RID: 2520 RVA: 0x00046E00 File Offset: 0x00045E00
		internal Type[] ParameterReaderTypes
		{
			get
			{
				return this.parameterReaderTypes;
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x060009D9 RID: 2521 RVA: 0x00046E08 File Offset: 0x00045E08
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x060009DA RID: 2522 RVA: 0x00046E10 File Offset: 0x00045E10
		// (set) Token: 0x060009DB RID: 2523 RVA: 0x00046ED0 File Offset: 0x00045ED0
		internal Type[] ProtocolImporterTypes
		{
			get
			{
				if (this.protocolImporterTypes.Length == 0)
				{
					lock (WebServicesSection.ClassSyncObject)
					{
						if (this.protocolImporterTypes.Length == 0)
						{
							WebServiceProtocols webServiceProtocols = this.EnabledProtocols;
							List<Type> list = new List<Type>();
							if ((webServiceProtocols & WebServiceProtocols.HttpSoap) != WebServiceProtocols.Unknown)
							{
								list.Add(typeof(SoapProtocolImporter));
							}
							if ((webServiceProtocols & WebServiceProtocols.HttpSoap12) != WebServiceProtocols.Unknown)
							{
								list.Add(typeof(Soap12ProtocolImporter));
							}
							if ((webServiceProtocols & WebServiceProtocols.HttpGet) != WebServiceProtocols.Unknown)
							{
								list.Add(typeof(HttpGetProtocolImporter));
							}
							if ((webServiceProtocols & WebServiceProtocols.HttpPost) != WebServiceProtocols.Unknown)
							{
								list.Add(typeof(HttpPostProtocolImporter));
							}
							this.protocolImporterTypes = list.ToArray();
						}
					}
				}
				return this.protocolImporterTypes;
			}
			set
			{
				this.protocolImporterTypes = value;
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x060009DC RID: 2524 RVA: 0x00046EDC File Offset: 0x00045EDC
		// (set) Token: 0x060009DD RID: 2525 RVA: 0x00046F9C File Offset: 0x00045F9C
		internal Type[] ProtocolReflectorTypes
		{
			get
			{
				if (this.protocolReflectorTypes.Length == 0)
				{
					lock (WebServicesSection.ClassSyncObject)
					{
						if (this.protocolReflectorTypes.Length == 0)
						{
							WebServiceProtocols webServiceProtocols = this.EnabledProtocols;
							List<Type> list = new List<Type>();
							if ((webServiceProtocols & WebServiceProtocols.HttpSoap) != WebServiceProtocols.Unknown)
							{
								list.Add(typeof(SoapProtocolReflector));
							}
							if ((webServiceProtocols & WebServiceProtocols.HttpSoap12) != WebServiceProtocols.Unknown)
							{
								list.Add(typeof(Soap12ProtocolReflector));
							}
							if ((webServiceProtocols & WebServiceProtocols.HttpGet) != WebServiceProtocols.Unknown)
							{
								list.Add(typeof(HttpGetProtocolReflector));
							}
							if ((webServiceProtocols & WebServiceProtocols.HttpPost) != WebServiceProtocols.Unknown)
							{
								list.Add(typeof(HttpPostProtocolReflector));
							}
							this.protocolReflectorTypes = list.ToArray();
						}
					}
				}
				return this.protocolReflectorTypes;
			}
			set
			{
				this.protocolReflectorTypes = value;
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x060009DE RID: 2526 RVA: 0x00046FA5 File Offset: 0x00045FA5
		[ConfigurationProperty("protocols")]
		public ProtocolElementCollection Protocols
		{
			get
			{
				return (ProtocolElementCollection)base[this.protocols];
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x060009DF RID: 2527 RVA: 0x00046FB8 File Offset: 0x00045FB8
		// (set) Token: 0x060009E0 RID: 2528 RVA: 0x00046FCB File Offset: 0x00045FCB
		[ConfigurationProperty("soapEnvelopeProcessing")]
		public SoapEnvelopeProcessingElement SoapEnvelopeProcessing
		{
			get
			{
				return (SoapEnvelopeProcessingElement)base[this.soapEnvelopeProcessing];
			}
			set
			{
				base[this.soapEnvelopeProcessing] = value;
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x060009E1 RID: 2529 RVA: 0x00046FDA File Offset: 0x00045FDA
		// (set) Token: 0x060009E2 RID: 2530 RVA: 0x00046FED File Offset: 0x00045FED
		public DiagnosticsElement Diagnostics
		{
			get
			{
				return (DiagnosticsElement)base[this.diagnostics];
			}
			set
			{
				base[this.diagnostics] = value;
			}
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x00046FFC File Offset: 0x00045FFC
		protected override void Reset(ConfigurationElement parentElement)
		{
			this.serverProtocolFactories = null;
			this.enabledProtocols = WebServiceProtocols.Unknown;
			if (parentElement != null)
			{
				WebServicesSection webServicesSection = (WebServicesSection)parentElement;
				this.discoveryDocumentSerializer = webServicesSection.discoveryDocumentSerializer;
			}
			base.Reset(parentElement);
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x060009E4 RID: 2532 RVA: 0x00047034 File Offset: 0x00046034
		internal Type[] ReturnWriterTypes
		{
			get
			{
				return this.returnWriterTypes;
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x060009E5 RID: 2533 RVA: 0x0004703C File Offset: 0x0004603C
		internal ServerProtocolFactory[] ServerProtocolFactories
		{
			get
			{
				if (this.serverProtocolFactories == null)
				{
					lock (WebServicesSection.ClassSyncObject)
					{
						if (this.serverProtocolFactories == null)
						{
							WebServiceProtocols webServiceProtocols = this.EnabledProtocols;
							List<ServerProtocolFactory> list = new List<ServerProtocolFactory>();
							if ((webServiceProtocols & WebServiceProtocols.AnyHttpSoap) != WebServiceProtocols.Unknown)
							{
								list.Add((ServerProtocolFactory)Activator.CreateInstance(this.SoapServerProtocolFactory));
							}
							if ((webServiceProtocols & WebServiceProtocols.HttpPost) != WebServiceProtocols.Unknown)
							{
								list.Add(new HttpPostServerProtocolFactory());
							}
							if ((webServiceProtocols & WebServiceProtocols.HttpPostLocalhost) != WebServiceProtocols.Unknown)
							{
								list.Add(new HttpPostLocalhostServerProtocolFactory());
							}
							if ((webServiceProtocols & WebServiceProtocols.HttpGet) != WebServiceProtocols.Unknown)
							{
								list.Add(new HttpGetServerProtocolFactory());
							}
							if ((webServiceProtocols & WebServiceProtocols.Documentation) != WebServiceProtocols.Unknown)
							{
								list.Add(new DiscoveryServerProtocolFactory());
								list.Add(new DocumentationServerProtocolFactory());
							}
							this.serverProtocolFactories = list.ToArray();
						}
					}
				}
				return this.serverProtocolFactories;
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x060009E6 RID: 2534 RVA: 0x0004710C File Offset: 0x0004610C
		internal bool ServiceDescriptionExtended
		{
			get
			{
				return this.ServiceDescriptionFormatExtensionTypes.Count > 0;
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x060009E7 RID: 2535 RVA: 0x0004711C File Offset: 0x0004611C
		[ConfigurationProperty("serviceDescriptionFormatExtensionTypes")]
		public TypeElementCollection ServiceDescriptionFormatExtensionTypes
		{
			get
			{
				return (TypeElementCollection)base[this.serviceDescriptionFormatExtensionTypes];
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x060009E8 RID: 2536 RVA: 0x0004712F File Offset: 0x0004612F
		[ConfigurationProperty("soapExtensionImporterTypes")]
		public TypeElementCollection SoapExtensionImporterTypes
		{
			get
			{
				return (TypeElementCollection)base[this.soapExtensionImporterTypes];
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x060009E9 RID: 2537 RVA: 0x00047142 File Offset: 0x00046142
		[ConfigurationProperty("soapExtensionReflectorTypes")]
		public TypeElementCollection SoapExtensionReflectorTypes
		{
			get
			{
				return (TypeElementCollection)base[this.soapExtensionReflectorTypes];
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x060009EA RID: 2538 RVA: 0x00047155 File Offset: 0x00046155
		[ConfigurationProperty("soapExtensionTypes")]
		public SoapExtensionTypeElementCollection SoapExtensionTypes
		{
			get
			{
				return (SoapExtensionTypeElementCollection)base[this.soapExtensionTypes];
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x060009EB RID: 2539 RVA: 0x00047168 File Offset: 0x00046168
		[ConfigurationProperty("soapServerProtocolFactory")]
		public TypeElement SoapServerProtocolFactoryType
		{
			get
			{
				return (TypeElement)base[this.soapServerProtocolFactoryType];
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x060009EC RID: 2540 RVA: 0x0004717C File Offset: 0x0004617C
		internal Type SoapServerProtocolFactory
		{
			get
			{
				if (this.soapServerProtocolFactory == null)
				{
					lock (WebServicesSection.ClassSyncObject)
					{
						if (this.soapServerProtocolFactory == null)
						{
							this.soapServerProtocolFactory = this.SoapServerProtocolFactoryType.Type;
						}
					}
				}
				return this.soapServerProtocolFactory;
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x060009ED RID: 2541 RVA: 0x000471D8 File Offset: 0x000461D8
		[ConfigurationProperty("soapTransportImporterTypes")]
		public TypeElementCollection SoapTransportImporterTypes
		{
			get
			{
				return (TypeElementCollection)base[this.soapTransportImporterTypes];
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x060009EE RID: 2542 RVA: 0x000471EC File Offset: 0x000461EC
		internal Type[] SoapTransportImporters
		{
			get
			{
				Type[] array = new Type[1 + this.SoapTransportImporterTypes.Count];
				array[0] = typeof(SoapHttpTransportImporter);
				for (int i = 0; i < this.SoapTransportImporterTypes.Count; i++)
				{
					array[i + 1] = this.SoapTransportImporterTypes[i].Type;
				}
				return array;
			}
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x00047248 File Offset: 0x00046248
		private void TurnOnGetAndPost()
		{
			bool flag = (this.EnabledProtocols & WebServiceProtocols.HttpPost) == WebServiceProtocols.Unknown;
			bool flag2 = (this.EnabledProtocols & WebServiceProtocols.HttpGet) == WebServiceProtocols.Unknown;
			if (!flag2 && !flag)
			{
				return;
			}
			ArrayList arrayList = new ArrayList(this.ProtocolImporterTypes);
			ArrayList arrayList2 = new ArrayList(this.ProtocolReflectorTypes);
			if (flag)
			{
				arrayList.Add(typeof(HttpPostProtocolImporter));
				arrayList2.Add(typeof(HttpPostProtocolReflector));
			}
			if (flag2)
			{
				arrayList.Add(typeof(HttpGetProtocolImporter));
				arrayList2.Add(typeof(HttpGetProtocolReflector));
			}
			this.ProtocolImporterTypes = (Type[])arrayList.ToArray(typeof(Type));
			this.ProtocolReflectorTypes = (Type[])arrayList2.ToArray(typeof(Type));
			this.enabledProtocols |= WebServiceProtocols.HttpGet | WebServiceProtocols.HttpPost;
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x060009F0 RID: 2544 RVA: 0x0004731A File Offset: 0x0004631A
		[ConfigurationProperty("wsdlHelpGenerator")]
		public WsdlHelpGeneratorElement WsdlHelpGenerator
		{
			get
			{
				return (WsdlHelpGeneratorElement)base[this.wsdlHelpGenerator];
			}
		}

		// Token: 0x04000618 RID: 1560
		private const string SectionName = "system.web/webServices";

		// Token: 0x04000619 RID: 1561
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x0400061A RID: 1562
		private static object classSyncObject;

		// Token: 0x0400061B RID: 1563
		private readonly ConfigurationProperty conformanceWarnings = new ConfigurationProperty("conformanceWarnings", typeof(WsiProfilesElementCollection), null, ConfigurationPropertyOptions.None);

		// Token: 0x0400061C RID: 1564
		private readonly ConfigurationProperty protocols = new ConfigurationProperty("protocols", typeof(ProtocolElementCollection), null, ConfigurationPropertyOptions.None);

		// Token: 0x0400061D RID: 1565
		private readonly ConfigurationProperty serviceDescriptionFormatExtensionTypes = new ConfigurationProperty("serviceDescriptionFormatExtensionTypes", typeof(TypeElementCollection), null, ConfigurationPropertyOptions.None);

		// Token: 0x0400061E RID: 1566
		private readonly ConfigurationProperty soapEnvelopeProcessing = new ConfigurationProperty("soapEnvelopeProcessing", typeof(SoapEnvelopeProcessingElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x0400061F RID: 1567
		private readonly ConfigurationProperty soapExtensionImporterTypes = new ConfigurationProperty("soapExtensionImporterTypes", typeof(TypeElementCollection), null, ConfigurationPropertyOptions.None);

		// Token: 0x04000620 RID: 1568
		private readonly ConfigurationProperty soapExtensionReflectorTypes = new ConfigurationProperty("soapExtensionReflectorTypes", typeof(TypeElementCollection), null, ConfigurationPropertyOptions.None);

		// Token: 0x04000621 RID: 1569
		private readonly ConfigurationProperty soapExtensionTypes = new ConfigurationProperty("soapExtensionTypes", typeof(SoapExtensionTypeElementCollection), null, ConfigurationPropertyOptions.None);

		// Token: 0x04000622 RID: 1570
		private readonly ConfigurationProperty soapTransportImporterTypes = new ConfigurationProperty("soapTransportImporterTypes", typeof(TypeElementCollection), null, ConfigurationPropertyOptions.None);

		// Token: 0x04000623 RID: 1571
		private readonly ConfigurationProperty wsdlHelpGenerator = new ConfigurationProperty("wsdlHelpGenerator", typeof(WsdlHelpGeneratorElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04000624 RID: 1572
		private readonly ConfigurationProperty soapServerProtocolFactoryType = new ConfigurationProperty("soapServerProtocolFactory", typeof(TypeElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04000625 RID: 1573
		private readonly ConfigurationProperty diagnostics = new ConfigurationProperty("diagnostics", typeof(DiagnosticsElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04000626 RID: 1574
		private Type[] defaultFormatTypes = new Type[]
		{
			typeof(HttpAddressBinding),
			typeof(HttpBinding),
			typeof(HttpOperationBinding),
			typeof(HttpUrlEncodedBinding),
			typeof(HttpUrlReplacementBinding),
			typeof(MimeContentBinding),
			typeof(MimeXmlBinding),
			typeof(MimeMultipartRelatedBinding),
			typeof(MimeTextBinding),
			typeof(global::System.Web.Services.Description.SoapBinding),
			typeof(SoapOperationBinding),
			typeof(SoapBodyBinding),
			typeof(SoapFaultBinding),
			typeof(SoapHeaderBinding),
			typeof(SoapAddressBinding),
			typeof(Soap12Binding),
			typeof(Soap12OperationBinding),
			typeof(Soap12BodyBinding),
			typeof(Soap12FaultBinding),
			typeof(Soap12HeaderBinding),
			typeof(Soap12AddressBinding)
		};

		// Token: 0x04000627 RID: 1575
		private Type[] discoveryReferenceTypes = new Type[]
		{
			typeof(DiscoveryDocumentReference),
			typeof(ContractReference),
			typeof(SchemaReference),
			typeof(global::System.Web.Services.Discovery.SoapBinding)
		};

		// Token: 0x04000628 RID: 1576
		private XmlSerializer discoveryDocumentSerializer;

		// Token: 0x04000629 RID: 1577
		private WebServiceProtocols enabledProtocols;

		// Token: 0x0400062A RID: 1578
		private Type[] mimeImporterTypes = new Type[]
		{
			typeof(MimeXmlImporter),
			typeof(MimeFormImporter),
			typeof(MimeTextImporter)
		};

		// Token: 0x0400062B RID: 1579
		private Type[] mimeReflectorTypes = new Type[]
		{
			typeof(MimeXmlReflector),
			typeof(MimeFormReflector)
		};

		// Token: 0x0400062C RID: 1580
		private Type[] parameterReaderTypes = new Type[]
		{
			typeof(UrlParameterReader),
			typeof(HtmlFormParameterReader)
		};

		// Token: 0x0400062D RID: 1581
		private Type[] protocolImporterTypes = new Type[0];

		// Token: 0x0400062E RID: 1582
		private Type[] protocolReflectorTypes = new Type[0];

		// Token: 0x0400062F RID: 1583
		private Type[] returnWriterTypes = new Type[] { typeof(XmlReturnWriter) };

		// Token: 0x04000630 RID: 1584
		private ServerProtocolFactory[] serverProtocolFactories;

		// Token: 0x04000631 RID: 1585
		private Type soapServerProtocolFactory;
	}
}
