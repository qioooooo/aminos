using System;
using System.Security.Policy;

namespace System.Xml.Serialization
{
	// Token: 0x02000339 RID: 825
	public class XmlSerializerFactory
	{
		// Token: 0x06002871 RID: 10353 RVA: 0x000D1804 File Offset: 0x000D0804
		public XmlSerializer CreateSerializer(Type type, XmlAttributeOverrides overrides, Type[] extraTypes, XmlRootAttribute root, string defaultNamespace)
		{
			return this.CreateSerializer(type, overrides, extraTypes, root, defaultNamespace, null, null);
		}

		// Token: 0x06002872 RID: 10354 RVA: 0x000D1815 File Offset: 0x000D0815
		public XmlSerializer CreateSerializer(Type type, XmlRootAttribute root)
		{
			return this.CreateSerializer(type, null, new Type[0], root, null, null, null);
		}

		// Token: 0x06002873 RID: 10355 RVA: 0x000D1829 File Offset: 0x000D0829
		public XmlSerializer CreateSerializer(Type type, Type[] extraTypes)
		{
			return this.CreateSerializer(type, null, extraTypes, null, null, null, null);
		}

		// Token: 0x06002874 RID: 10356 RVA: 0x000D1838 File Offset: 0x000D0838
		public XmlSerializer CreateSerializer(Type type, XmlAttributeOverrides overrides)
		{
			return this.CreateSerializer(type, overrides, new Type[0], null, null, null, null);
		}

		// Token: 0x06002875 RID: 10357 RVA: 0x000D184C File Offset: 0x000D084C
		public XmlSerializer CreateSerializer(XmlTypeMapping xmlTypeMapping)
		{
			TempAssembly tempAssembly = XmlSerializer.GenerateTempAssembly(xmlTypeMapping);
			return (XmlSerializer)tempAssembly.Contract.TypedSerializers[xmlTypeMapping.Key];
		}

		// Token: 0x06002876 RID: 10358 RVA: 0x000D187B File Offset: 0x000D087B
		public XmlSerializer CreateSerializer(Type type)
		{
			return this.CreateSerializer(type, null);
		}

		// Token: 0x06002877 RID: 10359 RVA: 0x000D1888 File Offset: 0x000D0888
		public XmlSerializer CreateSerializer(Type type, string defaultNamespace)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			TempAssembly tempAssembly = XmlSerializerFactory.cache[defaultNamespace, type];
			XmlTypeMapping xmlTypeMapping = null;
			if (tempAssembly == null)
			{
				lock (XmlSerializerFactory.cache)
				{
					tempAssembly = XmlSerializerFactory.cache[defaultNamespace, type];
					if (tempAssembly == null)
					{
						XmlSerializerImplementation xmlSerializerImplementation;
						if (TempAssembly.LoadGeneratedAssembly(type, defaultNamespace, out xmlSerializerImplementation) == null)
						{
							XmlReflectionImporter xmlReflectionImporter = new XmlReflectionImporter(defaultNamespace);
							xmlTypeMapping = xmlReflectionImporter.ImportTypeMapping(type, null, defaultNamespace);
							tempAssembly = XmlSerializer.GenerateTempAssembly(xmlTypeMapping, type, defaultNamespace);
						}
						else
						{
							tempAssembly = new TempAssembly(xmlSerializerImplementation);
						}
						XmlSerializerFactory.cache.Add(defaultNamespace, type, tempAssembly);
					}
				}
			}
			if (xmlTypeMapping == null)
			{
				xmlTypeMapping = XmlReflectionImporter.GetTopLevelMapping(type, defaultNamespace);
			}
			return tempAssembly.Contract.GetSerializer(type);
		}

		// Token: 0x06002878 RID: 10360 RVA: 0x000D1944 File Offset: 0x000D0944
		public XmlSerializer CreateSerializer(Type type, XmlAttributeOverrides overrides, Type[] extraTypes, XmlRootAttribute root, string defaultNamespace, string location, Evidence evidence)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			XmlReflectionImporter xmlReflectionImporter = new XmlReflectionImporter(overrides, defaultNamespace);
			for (int i = 0; i < extraTypes.Length; i++)
			{
				xmlReflectionImporter.IncludeType(extraTypes[i]);
			}
			XmlTypeMapping xmlTypeMapping = xmlReflectionImporter.ImportTypeMapping(type, root, defaultNamespace);
			TempAssembly tempAssembly = XmlSerializer.GenerateTempAssembly(xmlTypeMapping, type, defaultNamespace, location, evidence);
			return (XmlSerializer)tempAssembly.Contract.TypedSerializers[xmlTypeMapping.Key];
		}

		// Token: 0x04001680 RID: 5760
		private static TempAssemblyCache cache = new TempAssemblyCache();
	}
}
