using System;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000043 RID: 67
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class HttpMethodAttribute : Attribute
	{
		// Token: 0x0600016F RID: 367 RVA: 0x0000645D File Offset: 0x0000545D
		public HttpMethodAttribute()
		{
			this.returnFormatter = null;
			this.parameterFormatter = null;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00006473 File Offset: 0x00005473
		public HttpMethodAttribute(Type returnFormatter, Type parameterFormatter)
		{
			this.returnFormatter = returnFormatter;
			this.parameterFormatter = parameterFormatter;
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000171 RID: 369 RVA: 0x00006489 File Offset: 0x00005489
		// (set) Token: 0x06000172 RID: 370 RVA: 0x00006491 File Offset: 0x00005491
		public Type ReturnFormatter
		{
			get
			{
				return this.returnFormatter;
			}
			set
			{
				this.returnFormatter = value;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000173 RID: 371 RVA: 0x0000649A File Offset: 0x0000549A
		// (set) Token: 0x06000174 RID: 372 RVA: 0x000064A2 File Offset: 0x000054A2
		public Type ParameterFormatter
		{
			get
			{
				return this.parameterFormatter;
			}
			set
			{
				this.parameterFormatter = value;
			}
		}

		// Token: 0x0400028D RID: 653
		private Type returnFormatter;

		// Token: 0x0400028E RID: 654
		private Type parameterFormatter;
	}
}
