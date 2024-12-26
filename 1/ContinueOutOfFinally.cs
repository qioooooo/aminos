using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Microsoft.JScript
{
	// Token: 0x02000052 RID: 82
	[Serializable]
	public sealed class ContinueOutOfFinally : ApplicationException
	{
		// Token: 0x060003D6 RID: 982 RVA: 0x000183FE File Offset: 0x000173FE
		public ContinueOutOfFinally()
			: this(0)
		{
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00018407 File Offset: 0x00017407
		public ContinueOutOfFinally(int target)
		{
			this.target = target;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00018416 File Offset: 0x00017416
		public ContinueOutOfFinally(string m)
			: base(m)
		{
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0001841F File Offset: 0x0001741F
		public ContinueOutOfFinally(string m, Exception e)
			: base(m, e)
		{
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00018429 File Offset: 0x00017429
		private ContinueOutOfFinally(SerializationInfo s, StreamingContext c)
			: base(s, c)
		{
			this.target = s.GetInt32("Target");
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00018444 File Offset: 0x00017444
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo s, StreamingContext c)
		{
			base.GetObjectData(s, c);
			s.AddValue("Target", this.target);
		}

		// Token: 0x040001EE RID: 494
		public int target;
	}
}
