using System;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;

namespace System.Deployment.Application
{
	// Token: 0x02000082 RID: 130
	internal static class ManifestValidatingReader
	{
		// Token: 0x06000400 RID: 1024 RVA: 0x00015A3C File Offset: 0x00014A3C
		public static XmlReader Create(Stream stream)
		{
			return ManifestValidatingReader.Create(stream, ManifestValidatingReader.ManifestSchemaSet);
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00015A4C File Offset: 0x00014A4C
		private static XmlReader Create(Stream stream, XmlSchemaSet schemaSet)
		{
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.Schemas = schemaSet;
			xmlReaderSettings.ValidationType = ValidationType.Schema;
			xmlReaderSettings.ProhibitDtd = true;
			xmlReaderSettings.XmlResolver = null;
			ManifestValidatingReader.XmlFilteredReader xmlFilteredReader = new ManifestValidatingReader.XmlFilteredReader(stream);
			return XmlReader.Create(xmlFilteredReader, xmlReaderSettings);
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x00015A8C File Offset: 0x00014A8C
		private static XmlSchemaSet ManifestSchemaSet
		{
			get
			{
				if (ManifestValidatingReader._manifestSchemaSet == null)
				{
					lock (ManifestValidatingReader._manifestSchemaSetLock)
					{
						if (ManifestValidatingReader._manifestSchemaSet == null)
						{
							ManifestValidatingReader._manifestSchemaSet = ManifestValidatingReader.MakeSchemaSet(ManifestValidatingReader._manifestSchemas);
						}
					}
				}
				return ManifestValidatingReader._manifestSchemaSet;
			}
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00015AE0 File Offset: 0x00014AE0
		private static XmlSchemaSet MakeSchemaSet(string[] schemas)
		{
			XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			xmlSchemaSet.XmlResolver = new ManifestValidatingReader.ResourceResolver(executingAssembly);
			for (int i = 0; i < schemas.Length; i++)
			{
				using (Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(schemas[i]))
				{
					xmlSchemaSet.Add(null, new XmlTextReader(manifestResourceStream));
				}
			}
			return xmlSchemaSet;
		}

		// Token: 0x040002BA RID: 698
		private static string[] _manifestSchemas = new string[] { "manifest.2.0.0.15-pre.adaptive.xsd" };

		// Token: 0x040002BB RID: 699
		private static XmlSchemaSet _manifestSchemaSet = null;

		// Token: 0x040002BC RID: 700
		private static object _manifestSchemaSetLock = new object();

		// Token: 0x02000083 RID: 131
		private class ResourceResolver : XmlUrlResolver
		{
			// Token: 0x06000405 RID: 1029 RVA: 0x00015B7A File Offset: 0x00014B7A
			public ResourceResolver(Assembly assembly)
			{
				this._assembly = assembly;
			}

			// Token: 0x06000406 RID: 1030 RVA: 0x00015B8C File Offset: 0x00014B8C
			public override Uri ResolveUri(Uri baseUri, string relativeUri)
			{
				if (baseUri == null || baseUri.ToString() == string.Empty || (baseUri.IsAbsoluteUri && baseUri.AbsoluteUri.StartsWith("df://resources/", StringComparison.Ordinal)))
				{
					return new Uri("df://resources/" + relativeUri);
				}
				return base.ResolveUri(baseUri, relativeUri);
			}

			// Token: 0x06000407 RID: 1031 RVA: 0x00015BE8 File Offset: 0x00014BE8
			public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
			{
				if (!absoluteUri.AbsoluteUri.StartsWith("df://resources/", StringComparison.Ordinal))
				{
					return base.GetEntity(absoluteUri, role, ofObjectToReturn);
				}
				if (ofObjectToReturn != null && ofObjectToReturn != typeof(Stream))
				{
					throw new XmlException(Resources.GetString("Ex_OnlyStreamTypeSupported"));
				}
				if (absoluteUri.ToString() == "df://resources/-//W3C//DTD XMLSCHEMA 200102//EN")
				{
					return this._assembly.GetManifestResourceStream("XMLSchema.dtd");
				}
				if (absoluteUri.ToString() == "df://resources/xs-datatypes")
				{
					return this._assembly.GetManifestResourceStream("datatypes.dtd");
				}
				string text = absoluteUri.AbsoluteUri.Remove(0, "df://resources/".Length);
				return this._assembly.GetManifestResourceStream(text);
			}

			// Token: 0x040002BD RID: 701
			private const string Prefix = "df://resources/";

			// Token: 0x040002BE RID: 702
			private Assembly _assembly;
		}

		// Token: 0x02000084 RID: 132
		private class XmlFilteredReader : XmlTextReader
		{
			// Token: 0x06000408 RID: 1032 RVA: 0x00015C9E File Offset: 0x00014C9E
			static XmlFilteredReader()
			{
				ManifestValidatingReader.XmlFilteredReader.KnownNamespaces.Add("urn:schemas-microsoft-com:asm.v1");
				ManifestValidatingReader.XmlFilteredReader.KnownNamespaces.Add("urn:schemas-microsoft-com:asm.v2");
				ManifestValidatingReader.XmlFilteredReader.KnownNamespaces.Add("http://www.w3.org/2000/09/xmldsig#");
			}

			// Token: 0x06000409 RID: 1033 RVA: 0x00015CDA File Offset: 0x00014CDA
			public XmlFilteredReader(Stream stream)
				: base(stream)
			{
				base.ProhibitDtd = true;
			}

			// Token: 0x0600040A RID: 1034 RVA: 0x00015CEC File Offset: 0x00014CEC
			public override bool Read()
			{
				bool flag = base.Read();
				XmlNodeType nodeType = base.NodeType;
				if (nodeType == XmlNodeType.Element && !ManifestValidatingReader.XmlFilteredReader.KnownNamespaces.Contains(base.NamespaceURI))
				{
					base.Skip();
				}
				return flag;
			}

			// Token: 0x040002BF RID: 703
			private static StringCollection KnownNamespaces = new StringCollection();
		}
	}
}
