using System;

namespace System.ComponentModel
{
	// Token: 0x02000194 RID: 404
	[AttributeUsage(AttributeTargets.All)]
	public sealed class ParenthesizePropertyNameAttribute : Attribute
	{
		// Token: 0x06000CB4 RID: 3252 RVA: 0x00029557 File Offset: 0x00028557
		public ParenthesizePropertyNameAttribute()
			: this(false)
		{
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x00029560 File Offset: 0x00028560
		public ParenthesizePropertyNameAttribute(bool needParenthesis)
		{
			this.needParenthesis = needParenthesis;
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000CB6 RID: 3254 RVA: 0x0002956F File Offset: 0x0002856F
		public bool NeedParenthesis
		{
			get
			{
				return this.needParenthesis;
			}
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x00029577 File Offset: 0x00028577
		public override bool Equals(object o)
		{
			return o is ParenthesizePropertyNameAttribute && ((ParenthesizePropertyNameAttribute)o).NeedParenthesis == this.needParenthesis;
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x00029596 File Offset: 0x00028596
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x0002959E File Offset: 0x0002859E
		public override bool IsDefaultAttribute()
		{
			return this.Equals(ParenthesizePropertyNameAttribute.Default);
		}

		// Token: 0x04000AE5 RID: 2789
		public static readonly ParenthesizePropertyNameAttribute Default = new ParenthesizePropertyNameAttribute();

		// Token: 0x04000AE6 RID: 2790
		private bool needParenthesis;
	}
}
