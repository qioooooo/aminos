using System;

namespace System.Xml.Serialization
{
	// Token: 0x020002BF RID: 703
	internal class ElementAccessor : Accessor
	{
		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x0600218C RID: 8588 RVA: 0x0009EF9E File Offset: 0x0009DF9E
		// (set) Token: 0x0600218D RID: 8589 RVA: 0x0009EFA6 File Offset: 0x0009DFA6
		internal bool IsSoap
		{
			get
			{
				return this.isSoap;
			}
			set
			{
				this.isSoap = value;
			}
		}

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x0600218E RID: 8590 RVA: 0x0009EFAF File Offset: 0x0009DFAF
		// (set) Token: 0x0600218F RID: 8591 RVA: 0x0009EFB7 File Offset: 0x0009DFB7
		internal bool IsNullable
		{
			get
			{
				return this.nullable;
			}
			set
			{
				this.nullable = value;
			}
		}

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x06002190 RID: 8592 RVA: 0x0009EFC0 File Offset: 0x0009DFC0
		// (set) Token: 0x06002191 RID: 8593 RVA: 0x0009EFC8 File Offset: 0x0009DFC8
		internal bool IsUnbounded
		{
			get
			{
				return this.unbounded;
			}
			set
			{
				this.unbounded = value;
			}
		}

		// Token: 0x06002192 RID: 8594 RVA: 0x0009EFD4 File Offset: 0x0009DFD4
		internal ElementAccessor Clone()
		{
			return new ElementAccessor
			{
				nullable = this.nullable,
				IsTopLevelInSchema = base.IsTopLevelInSchema,
				Form = base.Form,
				isSoap = this.isSoap,
				Name = this.Name,
				Default = base.Default,
				Namespace = base.Namespace,
				Mapping = base.Mapping,
				Any = base.Any
			};
		}

		// Token: 0x04001466 RID: 5222
		private bool nullable;

		// Token: 0x04001467 RID: 5223
		private bool isSoap;

		// Token: 0x04001468 RID: 5224
		private bool unbounded;
	}
}
