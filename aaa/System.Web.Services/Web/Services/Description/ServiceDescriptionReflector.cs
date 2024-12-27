using System;
using System.Collections;
using System.Security.Permissions;
using System.Web.Services.Configuration;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x02000106 RID: 262
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class ServiceDescriptionReflector
	{
		// Token: 0x1700020D RID: 525
		// (get) Token: 0x0600073B RID: 1851 RVA: 0x0001FE44 File Offset: 0x0001EE44
		public ServiceDescriptionCollection ServiceDescriptions
		{
			get
			{
				return this.descriptions;
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x0600073C RID: 1852 RVA: 0x0001FE4C File Offset: 0x0001EE4C
		public XmlSchemas Schemas
		{
			get
			{
				return this.schemas;
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x0600073D RID: 1853 RVA: 0x0001FE54 File Offset: 0x0001EE54
		internal ServiceDescriptionCollection ServiceDescriptionsWithPost
		{
			get
			{
				return this.descriptionsWithPost;
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x0600073E RID: 1854 RVA: 0x0001FE5C File Offset: 0x0001EE5C
		internal XmlSchemas SchemasWithPost
		{
			get
			{
				return this.schemasWithPost;
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x0600073F RID: 1855 RVA: 0x0001FE64 File Offset: 0x0001EE64
		internal ServiceDescription ServiceDescription
		{
			get
			{
				return this.description;
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000740 RID: 1856 RVA: 0x0001FE6C File Offset: 0x0001EE6C
		internal Service Service
		{
			get
			{
				return this.service;
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000741 RID: 1857 RVA: 0x0001FE74 File Offset: 0x0001EE74
		internal Type ServiceType
		{
			get
			{
				return this.serviceType;
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000742 RID: 1858 RVA: 0x0001FE7C File Offset: 0x0001EE7C
		internal LogicalMethodInfo[] Methods
		{
			get
			{
				return this.methods;
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000743 RID: 1859 RVA: 0x0001FE84 File Offset: 0x0001EE84
		internal string ServiceUrl
		{
			get
			{
				return this.serviceUrl;
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000744 RID: 1860 RVA: 0x0001FE8C File Offset: 0x0001EE8C
		internal XmlSchemaExporter SchemaExporter
		{
			get
			{
				return this.exporter;
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000745 RID: 1861 RVA: 0x0001FE94 File Offset: 0x0001EE94
		internal XmlReflectionImporter ReflectionImporter
		{
			get
			{
				return this.importer;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000746 RID: 1862 RVA: 0x0001FE9C File Offset: 0x0001EE9C
		internal WebServiceAttribute ServiceAttribute
		{
			get
			{
				return this.serviceAttr;
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000747 RID: 1863 RVA: 0x0001FEA4 File Offset: 0x0001EEA4
		internal Hashtable ReflectionContext
		{
			get
			{
				if (this.reflectionContext == null)
				{
					this.reflectionContext = new Hashtable();
				}
				return this.reflectionContext;
			}
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x0001FEC0 File Offset: 0x0001EEC0
		public ServiceDescriptionReflector()
		{
			Type[] protocolReflectorTypes = WebServicesSection.Current.ProtocolReflectorTypes;
			this.reflectors = new ProtocolReflector[protocolReflectorTypes.Length];
			for (int i = 0; i < this.reflectors.Length; i++)
			{
				ProtocolReflector protocolReflector = (ProtocolReflector)Activator.CreateInstance(protocolReflectorTypes[i]);
				protocolReflector.Initialize(this);
				this.reflectors[i] = protocolReflector;
			}
			WebServiceProtocols enabledProtocols = WebServicesSection.Current.EnabledProtocols;
			if ((enabledProtocols & WebServiceProtocols.HttpPost) == WebServiceProtocols.Unknown && (enabledProtocols & WebServiceProtocols.HttpPostLocalhost) != WebServiceProtocols.Unknown)
			{
				this.reflectorsWithPost = new ProtocolReflector[this.reflectors.Length + 1];
				for (int j = 0; j < this.reflectorsWithPost.Length - 1; j++)
				{
					ProtocolReflector protocolReflector2 = (ProtocolReflector)Activator.CreateInstance(protocolReflectorTypes[j]);
					protocolReflector2.Initialize(this);
					this.reflectorsWithPost[j] = protocolReflector2;
				}
				ProtocolReflector protocolReflector3 = new HttpPostProtocolReflector();
				protocolReflector3.Initialize(this);
				this.reflectorsWithPost[this.reflectorsWithPost.Length - 1] = protocolReflector3;
			}
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0001FFBC File Offset: 0x0001EFBC
		private void ReflectInternal(ProtocolReflector[] reflectors)
		{
			this.description = new ServiceDescription();
			this.description.TargetNamespace = this.serviceAttr.Namespace;
			this.ServiceDescriptions.Add(this.description);
			this.service = new Service();
			string text = this.serviceAttr.Name;
			if (text == null || text.Length == 0)
			{
				text = this.serviceType.Name;
			}
			this.service.Name = XmlConvert.EncodeLocalName(text);
			if (this.serviceAttr.Description != null && this.serviceAttr.Description.Length > 0)
			{
				this.service.Documentation = this.serviceAttr.Description;
			}
			this.description.Services.Add(this.service);
			this.reflectionContext = new Hashtable();
			this.exporter = new XmlSchemaExporter(this.description.Types.Schemas);
			this.importer = SoapReflector.CreateXmlImporter(this.serviceAttr.Namespace, SoapReflector.ServiceDefaultIsEncoded(this.serviceType));
			WebMethodReflector.IncludeTypes(this.methods, this.importer);
			for (int i = 0; i < reflectors.Length; i++)
			{
				reflectors[i].Reflect();
			}
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x000200F8 File Offset: 0x0001F0F8
		public void Reflect(Type type, string url)
		{
			this.serviceType = type;
			this.serviceUrl = url;
			this.serviceAttr = WebServiceReflector.GetAttribute(type);
			this.methods = WebMethodReflector.GetMethods(type);
			this.CheckForDuplicateMethods(this.methods);
			this.descriptionsWithPost = this.descriptions;
			this.schemasWithPost = this.schemas;
			if (this.reflectorsWithPost != null)
			{
				this.ReflectInternal(this.reflectorsWithPost);
				this.descriptions = new ServiceDescriptionCollection();
				this.schemas = new XmlSchemas();
			}
			this.ReflectInternal(this.reflectors);
			if (this.serviceAttr.Description != null && this.serviceAttr.Description.Length > 0)
			{
				this.ServiceDescription.Documentation = this.serviceAttr.Description;
			}
			this.ServiceDescription.Types.Schemas.Compile(null, false);
			if (this.ServiceDescriptions.Count > 1)
			{
				this.Schemas.Add(this.ServiceDescription.Types.Schemas);
				this.ServiceDescription.Types.Schemas.Clear();
				return;
			}
			if (this.ServiceDescription.Types.Schemas.Count > 0)
			{
				XmlSchema[] array = new XmlSchema[this.ServiceDescription.Types.Schemas.Count];
				this.ServiceDescription.Types.Schemas.CopyTo(array, 0);
				foreach (XmlSchema xmlSchema in array)
				{
					if (XmlSchemas.IsDataSet(xmlSchema))
					{
						this.ServiceDescription.Types.Schemas.Remove(xmlSchema);
						this.Schemas.Add(xmlSchema);
					}
				}
			}
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0002029C File Offset: 0x0001F29C
		private void CheckForDuplicateMethods(LogicalMethodInfo[] methods)
		{
			Hashtable hashtable = new Hashtable();
			foreach (LogicalMethodInfo logicalMethodInfo in methods)
			{
				WebMethodAttribute methodAttribute = logicalMethodInfo.MethodAttribute;
				string text = methodAttribute.MessageName;
				if (text.Length == 0)
				{
					text = logicalMethodInfo.Name;
				}
				string text2 = ((logicalMethodInfo.Binding == null) ? text : (logicalMethodInfo.Binding.Name + "." + text));
				LogicalMethodInfo logicalMethodInfo2 = (LogicalMethodInfo)hashtable[text2];
				if (logicalMethodInfo2 != null)
				{
					throw new InvalidOperationException(Res.GetString("BothAndUseTheMessageNameUseTheMessageName3", new object[]
					{
						logicalMethodInfo,
						logicalMethodInfo2,
						XmlConvert.EncodeLocalName(text)
					}));
				}
				hashtable.Add(text2, logicalMethodInfo);
			}
		}

		// Token: 0x040004A4 RID: 1188
		private ProtocolReflector[] reflectors;

		// Token: 0x040004A5 RID: 1189
		private ProtocolReflector[] reflectorsWithPost;

		// Token: 0x040004A6 RID: 1190
		private ServiceDescriptionCollection descriptions = new ServiceDescriptionCollection();

		// Token: 0x040004A7 RID: 1191
		private XmlSchemas schemas = new XmlSchemas();

		// Token: 0x040004A8 RID: 1192
		private ServiceDescriptionCollection descriptionsWithPost;

		// Token: 0x040004A9 RID: 1193
		private XmlSchemas schemasWithPost;

		// Token: 0x040004AA RID: 1194
		private WebServiceAttribute serviceAttr;

		// Token: 0x040004AB RID: 1195
		private ServiceDescription description;

		// Token: 0x040004AC RID: 1196
		private Service service;

		// Token: 0x040004AD RID: 1197
		private LogicalMethodInfo[] methods;

		// Token: 0x040004AE RID: 1198
		private XmlSchemaExporter exporter;

		// Token: 0x040004AF RID: 1199
		private XmlReflectionImporter importer;

		// Token: 0x040004B0 RID: 1200
		private Type serviceType;

		// Token: 0x040004B1 RID: 1201
		private string serviceUrl;

		// Token: 0x040004B2 RID: 1202
		private Hashtable reflectionContext;
	}
}
