using System;
using System.IO;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000299 RID: 665
	internal abstract class Serializer
	{
		// Token: 0x0600227B RID: 8827
		public abstract object Deserialize(Stream s);

		// Token: 0x0600227C RID: 8828
		public abstract void Serialize(Stream s, object o);

		// Token: 0x0600227D RID: 8829 RVA: 0x0026DADC File Offset: 0x0026CEDC
		protected Serializer(Type t)
		{
			this.m_type = t;
		}

		// Token: 0x04001655 RID: 5717
		protected Type m_type;
	}
}
