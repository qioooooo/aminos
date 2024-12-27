using System;

namespace System.Xml.Serialization
{
	// Token: 0x02000306 RID: 774
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = false)]
	public class XmlChoiceIdentifierAttribute : Attribute
	{
		// Token: 0x0600243B RID: 9275 RVA: 0x000AABCE File Offset: 0x000A9BCE
		public XmlChoiceIdentifierAttribute()
		{
		}

		// Token: 0x0600243C RID: 9276 RVA: 0x000AABD6 File Offset: 0x000A9BD6
		public XmlChoiceIdentifierAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x0600243D RID: 9277 RVA: 0x000AABE5 File Offset: 0x000A9BE5
		// (set) Token: 0x0600243E RID: 9278 RVA: 0x000AABFB File Offset: 0x000A9BFB
		public string MemberName
		{
			get
			{
				if (this.name != null)
				{
					return this.name;
				}
				return string.Empty;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x0400156F RID: 5487
		private string name;
	}
}
