using System;
using System.IO;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x0200029B RID: 667
	internal sealed class BinarySerializeSerializer : Serializer
	{
		// Token: 0x06002281 RID: 8833 RVA: 0x0026DB80 File Offset: 0x0026CF80
		internal BinarySerializeSerializer(Type t)
			: base(t)
		{
		}

		// Token: 0x06002282 RID: 8834 RVA: 0x0026DB94 File Offset: 0x0026CF94
		public override void Serialize(Stream s, object o)
		{
			BinaryWriter binaryWriter = new BinaryWriter(s);
			((IBinarySerialize)o).Write(binaryWriter);
		}

		// Token: 0x06002283 RID: 8835 RVA: 0x0026DBB4 File Offset: 0x0026CFB4
		public override object Deserialize(Stream s)
		{
			object obj = Activator.CreateInstance(this.m_type);
			BinaryReader binaryReader = new BinaryReader(s);
			((IBinarySerialize)obj).Read(binaryReader);
			return obj;
		}
	}
}
