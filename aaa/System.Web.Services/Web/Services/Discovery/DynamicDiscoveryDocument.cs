using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace System.Web.Services.Discovery
{
	// Token: 0x020000AA RID: 170
	[XmlRoot("dynamicDiscovery", Namespace = "urn:schemas-dynamicdiscovery:disco.2000-03-17")]
	public sealed class DynamicDiscoveryDocument
	{
		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x00016BE8 File Offset: 0x00015BE8
		// (set) Token: 0x0600048A RID: 1162 RVA: 0x00016BF0 File Offset: 0x00015BF0
		[XmlElement("exclude", typeof(ExcludePathInfo))]
		public ExcludePathInfo[] ExcludePaths
		{
			get
			{
				return this.excludePaths;
			}
			set
			{
				if (value == null)
				{
					value = new ExcludePathInfo[0];
				}
				this.excludePaths = value;
			}
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x00016C04 File Offset: 0x00015C04
		public void Write(Stream stream)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(DynamicDiscoveryDocument));
			xmlSerializer.Serialize(new StreamWriter(stream, new UTF8Encoding(false)), this);
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00016C34 File Offset: 0x00015C34
		public static DynamicDiscoveryDocument Load(Stream stream)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(DynamicDiscoveryDocument));
			return (DynamicDiscoveryDocument)xmlSerializer.Deserialize(stream);
		}

		// Token: 0x040003C5 RID: 965
		public const string Namespace = "urn:schemas-dynamicdiscovery:disco.2000-03-17";

		// Token: 0x040003C6 RID: 966
		private ExcludePathInfo[] excludePaths = new ExcludePathInfo[0];
	}
}
