using System;
using System.Runtime.InteropServices;

namespace System.Security.Principal
{
	// Token: 0x020004AF RID: 1199
	[ComVisible(true)]
	[Serializable]
	public class GenericIdentity : IIdentity
	{
		// Token: 0x06003095 RID: 12437 RVA: 0x000A7710 File Offset: 0x000A6710
		public GenericIdentity(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.m_name = name;
			this.m_type = "";
		}

		// Token: 0x06003096 RID: 12438 RVA: 0x000A7738 File Offset: 0x000A6738
		public GenericIdentity(string name, string type)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this.m_name = name;
			this.m_type = type;
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x06003097 RID: 12439 RVA: 0x000A776A File Offset: 0x000A676A
		public virtual string Name
		{
			get
			{
				return this.m_name;
			}
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x06003098 RID: 12440 RVA: 0x000A7772 File Offset: 0x000A6772
		public virtual string AuthenticationType
		{
			get
			{
				return this.m_type;
			}
		}

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x06003099 RID: 12441 RVA: 0x000A777A File Offset: 0x000A677A
		public virtual bool IsAuthenticated
		{
			get
			{
				return !this.m_name.Equals("");
			}
		}

		// Token: 0x0400182C RID: 6188
		private string m_name;

		// Token: 0x0400182D RID: 6189
		private string m_type;
	}
}
