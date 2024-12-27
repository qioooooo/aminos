using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Web.Services.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Discovery
{
	// Token: 0x0200009F RID: 159
	[XmlRoot("discovery", Namespace = "http://schemas.xmlsoap.org/disco/")]
	public sealed class DiscoveryDocument
	{
		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000434 RID: 1076 RVA: 0x00014DBF File Offset: 0x00013DBF
		[XmlIgnore]
		public IList References
		{
			get
			{
				return this.references;
			}
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00014DC8 File Offset: 0x00013DC8
		public static DiscoveryDocument Read(Stream stream)
		{
			return DiscoveryDocument.Read(new XmlTextReader(stream)
			{
				WhitespaceHandling = WhitespaceHandling.Significant,
				XmlResolver = null,
				ProhibitDtd = true
			});
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00014DF8 File Offset: 0x00013DF8
		public static DiscoveryDocument Read(TextReader reader)
		{
			return DiscoveryDocument.Read(new XmlTextReader(reader)
			{
				WhitespaceHandling = WhitespaceHandling.Significant,
				XmlResolver = null,
				ProhibitDtd = true
			});
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00014E27 File Offset: 0x00013E27
		public static DiscoveryDocument Read(XmlReader xmlReader)
		{
			return (DiscoveryDocument)WebServicesSection.Current.DiscoveryDocumentSerializer.Deserialize(xmlReader);
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00014E3E File Offset: 0x00013E3E
		public static bool CanRead(XmlReader xmlReader)
		{
			return WebServicesSection.Current.DiscoveryDocumentSerializer.CanDeserialize(xmlReader);
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00014E50 File Offset: 0x00013E50
		public void Write(TextWriter writer)
		{
			this.Write(new XmlTextWriter(writer)
			{
				Formatting = Formatting.Indented,
				Indentation = 2
			});
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00014E7C File Offset: 0x00013E7C
		public void Write(Stream stream)
		{
			TextWriter textWriter = new StreamWriter(stream, new UTF8Encoding(false));
			this.Write(textWriter);
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00014EA0 File Offset: 0x00013EA0
		public void Write(XmlWriter writer)
		{
			XmlSerializer discoveryDocumentSerializer = WebServicesSection.Current.DiscoveryDocumentSerializer;
			XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
			discoveryDocumentSerializer.Serialize(writer, this, xmlSerializerNamespaces);
		}

		// Token: 0x040003A5 RID: 933
		public const string Namespace = "http://schemas.xmlsoap.org/disco/";

		// Token: 0x040003A6 RID: 934
		private ArrayList references = new ArrayList();
	}
}
