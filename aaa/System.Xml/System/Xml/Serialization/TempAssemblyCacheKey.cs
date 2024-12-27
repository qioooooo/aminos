using System;

namespace System.Xml.Serialization
{
	// Token: 0x020002B5 RID: 693
	internal class TempAssemblyCacheKey
	{
		// Token: 0x06002139 RID: 8505 RVA: 0x0009D7B9 File Offset: 0x0009C7B9
		internal TempAssemblyCacheKey(string ns, object type)
		{
			this.type = type;
			this.ns = ns;
		}

		// Token: 0x0600213A RID: 8506 RVA: 0x0009D7D0 File Offset: 0x0009C7D0
		public override bool Equals(object o)
		{
			TempAssemblyCacheKey tempAssemblyCacheKey = o as TempAssemblyCacheKey;
			return tempAssemblyCacheKey != null && tempAssemblyCacheKey.type == this.type && tempAssemblyCacheKey.ns == this.ns;
		}

		// Token: 0x0600213B RID: 8507 RVA: 0x0009D80A File Offset: 0x0009C80A
		public override int GetHashCode()
		{
			return ((this.ns != null) ? this.ns.GetHashCode() : 0) ^ ((this.type != null) ? this.type.GetHashCode() : 0);
		}

		// Token: 0x04001444 RID: 5188
		private string ns;

		// Token: 0x04001445 RID: 5189
		private object type;
	}
}
