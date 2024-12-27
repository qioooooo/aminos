using System;

namespace System.Diagnostics.CodeAnalysis
{
	// Token: 0x020002B4 RID: 692
	[Conditional("CODE_ANALYSIS")]
	[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
	public sealed class SuppressMessageAttribute : Attribute
	{
		// Token: 0x06001B54 RID: 6996 RVA: 0x00047F9E File Offset: 0x00046F9E
		public SuppressMessageAttribute(string category, string checkId)
		{
			this.category = category;
			this.checkId = checkId;
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06001B55 RID: 6997 RVA: 0x00047FB4 File Offset: 0x00046FB4
		public string Category
		{
			get
			{
				return this.category;
			}
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06001B56 RID: 6998 RVA: 0x00047FBC File Offset: 0x00046FBC
		public string CheckId
		{
			get
			{
				return this.checkId;
			}
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06001B57 RID: 6999 RVA: 0x00047FC4 File Offset: 0x00046FC4
		// (set) Token: 0x06001B58 RID: 7000 RVA: 0x00047FCC File Offset: 0x00046FCC
		public string Scope
		{
			get
			{
				return this.scope;
			}
			set
			{
				this.scope = value;
			}
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x06001B59 RID: 7001 RVA: 0x00047FD5 File Offset: 0x00046FD5
		// (set) Token: 0x06001B5A RID: 7002 RVA: 0x00047FDD File Offset: 0x00046FDD
		public string Target
		{
			get
			{
				return this.target;
			}
			set
			{
				this.target = value;
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06001B5B RID: 7003 RVA: 0x00047FE6 File Offset: 0x00046FE6
		// (set) Token: 0x06001B5C RID: 7004 RVA: 0x00047FEE File Offset: 0x00046FEE
		public string MessageId
		{
			get
			{
				return this.messageId;
			}
			set
			{
				this.messageId = value;
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06001B5D RID: 7005 RVA: 0x00047FF7 File Offset: 0x00046FF7
		// (set) Token: 0x06001B5E RID: 7006 RVA: 0x00047FFF File Offset: 0x00046FFF
		public string Justification
		{
			get
			{
				return this.justification;
			}
			set
			{
				this.justification = value;
			}
		}

		// Token: 0x04000A56 RID: 2646
		private string category;

		// Token: 0x04000A57 RID: 2647
		private string justification;

		// Token: 0x04000A58 RID: 2648
		private string checkId;

		// Token: 0x04000A59 RID: 2649
		private string scope;

		// Token: 0x04000A5A RID: 2650
		private string target;

		// Token: 0x04000A5B RID: 2651
		private string messageId;
	}
}
