using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Microsoft.JScript
{
	// Token: 0x02000032 RID: 50
	[Serializable]
	public sealed class BreakOutOfFinally : ApplicationException
	{
		// Token: 0x060001FE RID: 510 RVA: 0x0000F15A File Offset: 0x0000E15A
		public BreakOutOfFinally(int target)
		{
			this.target = target;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000F169 File Offset: 0x0000E169
		public BreakOutOfFinally(string m)
			: base(m)
		{
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000F172 File Offset: 0x0000E172
		public BreakOutOfFinally(string m, Exception e)
			: base(m, e)
		{
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000F17C File Offset: 0x0000E17C
		private BreakOutOfFinally(SerializationInfo s, StreamingContext c)
			: base(s, c)
		{
			this.target = s.GetInt32("Target");
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000F197 File Offset: 0x0000E197
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo s, StreamingContext c)
		{
			base.GetObjectData(s, c);
			s.AddValue("Target", this.target);
		}

		// Token: 0x0400009E RID: 158
		public int target;
	}
}
