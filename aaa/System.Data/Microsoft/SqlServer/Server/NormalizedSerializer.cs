using System;
using System.IO;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x0200029A RID: 666
	internal sealed class NormalizedSerializer : Serializer
	{
		// Token: 0x0600227E RID: 8830 RVA: 0x0026DAF8 File Offset: 0x0026CEF8
		internal NormalizedSerializer(Type t)
			: base(t)
		{
			SqlUserDefinedTypeAttribute udtAttribute = SerializationHelperSql9.GetUdtAttribute(t);
			this.m_normalizer = new BinaryOrderedUdtNormalizer(t, true);
			this.m_isFixedSize = udtAttribute.IsFixedLength;
			this.m_maxSize = this.m_normalizer.Size;
		}

		// Token: 0x0600227F RID: 8831 RVA: 0x0026DB40 File Offset: 0x0026CF40
		public override void Serialize(Stream s, object o)
		{
			this.m_normalizer.NormalizeTopObject(o, s);
		}

		// Token: 0x06002280 RID: 8832 RVA: 0x0026DB5C File Offset: 0x0026CF5C
		public override object Deserialize(Stream s)
		{
			return this.m_normalizer.DeNormalizeTopObject(this.m_type, s);
		}

		// Token: 0x04001656 RID: 5718
		private BinaryOrderedUdtNormalizer m_normalizer;

		// Token: 0x04001657 RID: 5719
		private bool m_isFixedSize;

		// Token: 0x04001658 RID: 5720
		private int m_maxSize;
	}
}
