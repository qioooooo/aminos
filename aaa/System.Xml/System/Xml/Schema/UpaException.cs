using System;

namespace System.Xml.Schema
{
	// Token: 0x02000191 RID: 401
	internal class UpaException : Exception
	{
		// Token: 0x06001532 RID: 5426 RVA: 0x0005E900 File Offset: 0x0005D900
		public UpaException(object particle1, object particle2)
		{
			this.particle1 = particle1;
			this.particle2 = particle2;
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06001533 RID: 5427 RVA: 0x0005E916 File Offset: 0x0005D916
		public object Particle1
		{
			get
			{
				return this.particle1;
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06001534 RID: 5428 RVA: 0x0005E91E File Offset: 0x0005D91E
		public object Particle2
		{
			get
			{
				return this.particle2;
			}
		}

		// Token: 0x04000CB9 RID: 3257
		private object particle1;

		// Token: 0x04000CBA RID: 3258
		private object particle2;
	}
}
