using System;
using System.Collections;
using System.Deployment.Application.Manifest;
using System.Deployment.Internal.Isolation;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml;

namespace System.Deployment.Application
{
	// Token: 0x02000077 RID: 119
	internal static class ManifestGenerator
	{
		// Token: 0x06000394 RID: 916 RVA: 0x00014D50 File Offset: 0x00013D50
		public static DefinitionIdentity GenerateManifest(ReferenceIdentity suggestedReferenceIdentity, AssemblyManifest manifest, string outputManifest)
		{
			DefinitionIdentity definitionIdentity = manifest.Identity;
			if (manifest.RawXmlBytes != null)
			{
				using (FileStream fileStream = global::System.IO.File.Open(outputManifest, FileMode.CreateNew, FileAccess.Write))
				{
					fileStream.Write(manifest.RawXmlBytes, 0, manifest.RawXmlBytes.Length);
					return definitionIdentity;
				}
			}
			XmlDocument xmlDocument = ManifestGenerator.CloneAssemblyTemplate();
			definitionIdentity = new DefinitionIdentity(suggestedReferenceIdentity);
			ManifestGenerator.InjectIdentityXml(xmlDocument, definitionIdentity);
			ManifestGenerator.AddFiles(xmlDocument, manifest.Files);
			ManifestGenerator.AddDependencies(xmlDocument, manifest.DependentAssemblies);
			using (FileStream fileStream2 = global::System.IO.File.Open(outputManifest, FileMode.CreateNew, FileAccess.Write))
			{
				xmlDocument.Save(fileStream2);
			}
			return definitionIdentity;
		}

		// Token: 0x06000395 RID: 917 RVA: 0x00014DFC File Offset: 0x00013DFC
		public static void GenerateGACDetectionManifest(ReferenceIdentity refId, string outputManifest)
		{
			XmlDocument xmlDocument = ManifestGenerator.CloneAssemblyTemplate();
			if (ManifestGenerator.GACDetectionTempManifestAsmId == null)
			{
				Interlocked.CompareExchange(ref ManifestGenerator.GACDetectionTempManifestAsmId, new DefinitionIdentity("GACDetectionTempManifest, version=1.0.0.0, type=win32"), null);
			}
			ManifestGenerator.InjectIdentityXml(xmlDocument, (DefinitionIdentity)ManifestGenerator.GACDetectionTempManifestAsmId);
			ManifestGenerator.AddDependencies(xmlDocument, new DependentAssembly[]
			{
				new DependentAssembly(refId)
			});
			using (FileStream fileStream = global::System.IO.File.Open(outputManifest, FileMode.CreateNew, FileAccess.Write))
			{
				xmlDocument.Save(fileStream);
			}
		}

		// Token: 0x06000396 RID: 918 RVA: 0x00014E80 File Offset: 0x00013E80
		private static void AddFiles(XmlDocument document, global::System.Deployment.Application.Manifest.File[] files)
		{
			XmlNamespaceManager namespaceMgr = ManifestGenerator.GetNamespaceMgr(document);
			XmlElement xmlElement = (XmlElement)document.SelectSingleNode("/asmv1:assembly", namespaceMgr);
			foreach (global::System.Deployment.Application.Manifest.File file in files)
			{
				ManifestGenerator.AddFile(document, xmlElement, file);
			}
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00014EC8 File Offset: 0x00013EC8
		private static void AddFile(XmlDocument document, XmlElement assemblyNode, global::System.Deployment.Application.Manifest.File file)
		{
			XmlElement xmlElement = document.CreateElement("file", "urn:schemas-microsoft-com:asm.v1");
			assemblyNode.AppendChild(xmlElement);
			XmlAttribute xmlAttribute = xmlElement.SetAttributeNode("name", null);
			xmlAttribute.Value = file.Name;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00014F08 File Offset: 0x00013F08
		private static void AddDependencies(XmlDocument document, DependentAssembly[] dependentAssemblies)
		{
			Hashtable hashtable = new Hashtable();
			XmlNamespaceManager namespaceMgr = ManifestGenerator.GetNamespaceMgr(document);
			XmlElement xmlElement = (XmlElement)document.SelectSingleNode("/asmv1:assembly", namespaceMgr);
			foreach (DependentAssembly dependentAssembly in dependentAssemblies)
			{
				if (!hashtable.Contains(dependentAssembly.Identity))
				{
					XmlElement xmlElement2 = document.CreateElement("dependency", "urn:schemas-microsoft-com:asm.v1");
					xmlElement.AppendChild(xmlElement2);
					XmlElement xmlElement3 = document.CreateElement("dependentAssembly", "urn:schemas-microsoft-com:asm.v1");
					xmlElement2.AppendChild(xmlElement3);
					ReferenceIdentity identity = dependentAssembly.Identity;
					DefinitionIdentity definitionIdentity = new DefinitionIdentity(identity);
					XmlElement xmlElement4 = ManifestGenerator.CreateAssemblyIdentityElement(document, definitionIdentity);
					xmlElement3.AppendChild(xmlElement4);
					hashtable.Add(identity, definitionIdentity);
				}
			}
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00014FCC File Offset: 0x00013FCC
		private static void InjectIdentityXml(XmlDocument document, DefinitionIdentity asmId)
		{
			XmlElement xmlElement = ManifestGenerator.CreateAssemblyIdentityElement(document, asmId);
			document.DocumentElement.AppendChild(xmlElement);
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00014FF0 File Offset: 0x00013FF0
		private static XmlElement CreateAssemblyIdentityElement(XmlDocument document, DefinitionIdentity asmId)
		{
			XmlElement xmlElement = document.CreateElement("assemblyIdentity", "urn:schemas-microsoft-com:asm.v1");
			IDENTITY_ATTRIBUTE[] attributes = asmId.Attributes;
			StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase;
			foreach (IDENTITY_ATTRIBUTE identity_ATTRIBUTE in attributes)
			{
				string @namespace = identity_ATTRIBUTE.Namespace;
				string text = identity_ATTRIBUTE.Name;
				if (@namespace == null)
				{
					if (text.Equals("name", stringComparison))
					{
						text = "name";
					}
					else if (text.Equals("version", stringComparison))
					{
						text = "version";
					}
					else if (text.Equals("processorArchitecture", stringComparison))
					{
						text = "processorArchitecture";
					}
					else if (text.Equals("publicKeyToken", stringComparison))
					{
						text = "publicKeyToken";
					}
					else if (text.Equals("type", stringComparison))
					{
						text = "type";
					}
					else if (text.Equals("culture", stringComparison))
					{
						text = "language";
					}
				}
				xmlElement.SetAttribute(text, @namespace, identity_ATTRIBUTE.Value);
			}
			return xmlElement;
		}

		// Token: 0x0600039B RID: 923 RVA: 0x000150FC File Offset: 0x000140FC
		private static XmlDocument CloneAssemblyTemplate()
		{
			if (ManifestGenerator.assemblyTemplateDoc == null)
			{
				Assembly executingAssembly = Assembly.GetExecutingAssembly();
				Stream manifestResourceStream = executingAssembly.GetManifestResourceStream("AssemblyTemplate.xml");
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(manifestResourceStream);
				Interlocked.CompareExchange(ref ManifestGenerator.assemblyTemplateDoc, xmlDocument, null);
			}
			return (XmlDocument)((XmlDocument)ManifestGenerator.assemblyTemplateDoc).Clone();
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00015150 File Offset: 0x00014150
		private static XmlNamespaceManager GetNamespaceMgr(XmlDocument document)
		{
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(document.NameTable);
			xmlNamespaceManager.AddNamespace("asmv1", "urn:schemas-microsoft-com:asm.v1");
			xmlNamespaceManager.AddNamespace("asmv2", "urn:schemas-microsoft-com:asm.v2");
			xmlNamespaceManager.AddNamespace("dsig", "http://www.w3.org/2000/09/xmldsig#");
			return xmlNamespaceManager;
		}

		// Token: 0x0400029F RID: 671
		private const string AssemblyTemplateResource = "AssemblyTemplate.xml";

		// Token: 0x040002A0 RID: 672
		private static object assemblyTemplateDoc;

		// Token: 0x040002A1 RID: 673
		private static object GACDetectionTempManifestAsmId;
	}
}
