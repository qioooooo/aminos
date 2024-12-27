using System;
using System.Reflection;
using System.Web.Services.Description;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000032 RID: 50
	internal class DocumentationServerType : ServerType
	{
		// Token: 0x06000122 RID: 290 RVA: 0x00004EB8 File Offset: 0x00003EB8
		internal DocumentationServerType(Type type, string uri)
			: base(typeof(DocumentationServerProtocol))
		{
			Uri uri2 = new Uri(uri, true);
			uri = uri2.GetLeftPart(UriPartial.Path);
			this.methodInfo = new LogicalMethodInfo(typeof(DocumentationServerProtocol).GetMethod("Documentation", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));
			ServiceDescriptionReflector serviceDescriptionReflector = new ServiceDescriptionReflector();
			serviceDescriptionReflector.Reflect(type, uri);
			this.schemas = serviceDescriptionReflector.Schemas;
			this.serviceDescriptions = serviceDescriptionReflector.ServiceDescriptions;
			this.schemasWithPost = serviceDescriptionReflector.SchemasWithPost;
			this.serviceDescriptionsWithPost = serviceDescriptionReflector.ServiceDescriptionsWithPost;
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00004F45 File Offset: 0x00003F45
		internal LogicalMethodInfo MethodInfo
		{
			get
			{
				return this.methodInfo;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00004F4D File Offset: 0x00003F4D
		internal XmlSchemas Schemas
		{
			get
			{
				return this.schemas;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00004F55 File Offset: 0x00003F55
		internal ServiceDescriptionCollection ServiceDescriptions
		{
			get
			{
				return this.serviceDescriptions;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000126 RID: 294 RVA: 0x00004F5D File Offset: 0x00003F5D
		internal ServiceDescriptionCollection ServiceDescriptionsWithPost
		{
			get
			{
				return this.serviceDescriptionsWithPost;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00004F65 File Offset: 0x00003F65
		internal XmlSchemas SchemasWithPost
		{
			get
			{
				return this.schemasWithPost;
			}
		}

		// Token: 0x04000273 RID: 627
		private ServiceDescriptionCollection serviceDescriptions;

		// Token: 0x04000274 RID: 628
		private ServiceDescriptionCollection serviceDescriptionsWithPost;

		// Token: 0x04000275 RID: 629
		private XmlSchemas schemas;

		// Token: 0x04000276 RID: 630
		private XmlSchemas schemasWithPost;

		// Token: 0x04000277 RID: 631
		private LogicalMethodInfo methodInfo;
	}
}
