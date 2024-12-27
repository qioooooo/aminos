using System;
using System.IO;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x0200027E RID: 638
	public interface IBinarySerialize
	{
		// Token: 0x0600219A RID: 8602
		void Read(BinaryReader r);

		// Token: 0x0600219B RID: 8603
		void Write(BinaryWriter w);
	}
}
