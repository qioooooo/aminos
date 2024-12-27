using System;

namespace System.ComponentModel.Design
{
	// Token: 0x02000177 RID: 375
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
	[Serializable]
	public sealed class HelpKeywordAttribute : Attribute
	{
		// Token: 0x06000C15 RID: 3093 RVA: 0x00029381 File Offset: 0x00028381
		public HelpKeywordAttribute()
		{
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x00029389 File Offset: 0x00028389
		public HelpKeywordAttribute(string keyword)
		{
			if (keyword == null)
			{
				throw new ArgumentNullException("keyword");
			}
			this.contextKeyword = keyword;
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x000293A6 File Offset: 0x000283A6
		public HelpKeywordAttribute(Type t)
		{
			if (t == null)
			{
				throw new ArgumentNullException("t");
			}
			this.contextKeyword = t.FullName;
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000C18 RID: 3096 RVA: 0x000293C8 File Offset: 0x000283C8
		public string HelpKeyword
		{
			get
			{
				return this.contextKeyword;
			}
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x000293D0 File Offset: 0x000283D0
		public override bool Equals(object obj)
		{
			return obj == this || (obj != null && obj is HelpKeywordAttribute && ((HelpKeywordAttribute)obj).HelpKeyword == this.HelpKeyword);
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x000293FB File Offset: 0x000283FB
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x00029403 File Offset: 0x00028403
		public override bool IsDefaultAttribute()
		{
			return this.Equals(HelpKeywordAttribute.Default);
		}

		// Token: 0x04000AD2 RID: 2770
		public static readonly HelpKeywordAttribute Default = new HelpKeywordAttribute();

		// Token: 0x04000AD3 RID: 2771
		private string contextKeyword;
	}
}
