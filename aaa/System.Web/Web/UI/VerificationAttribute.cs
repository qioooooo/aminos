using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000486 RID: 1158
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class VerificationAttribute : Attribute
	{
		// Token: 0x06003679 RID: 13945 RVA: 0x000EB4FC File Offset: 0x000EA4FC
		public VerificationAttribute(string guideline, string checkpoint, VerificationReportLevel reportLevel, int priority, string message)
			: this(guideline, checkpoint, reportLevel, priority, message, VerificationRule.Required, string.Empty, VerificationConditionalOperator.Equals, string.Empty, string.Empty)
		{
		}

		// Token: 0x0600367A RID: 13946 RVA: 0x000EB528 File Offset: 0x000EA528
		public VerificationAttribute(string guideline, string checkpoint, VerificationReportLevel reportLevel, int priority, string message, VerificationRule rule, string conditionalProperty)
			: this(guideline, checkpoint, reportLevel, priority, message, rule, conditionalProperty, VerificationConditionalOperator.NotEquals, string.Empty, string.Empty)
		{
		}

		// Token: 0x0600367B RID: 13947 RVA: 0x000EB554 File Offset: 0x000EA554
		internal VerificationAttribute(string guideline, string checkpoint, VerificationReportLevel reportLevel, int priority, string message, VerificationRule rule, string conditionalProperty, VerificationConditionalOperator conditionalOperator, string conditionalValue)
			: this(guideline, checkpoint, reportLevel, priority, message, rule, conditionalProperty, conditionalOperator, conditionalValue, string.Empty)
		{
		}

		// Token: 0x0600367C RID: 13948 RVA: 0x000EB57C File Offset: 0x000EA57C
		public VerificationAttribute(string guideline, string checkpoint, VerificationReportLevel reportLevel, int priority, string message, VerificationRule rule, string conditionalProperty, VerificationConditionalOperator conditionalOperator, string conditionalValue, string guidelineUrl)
		{
			this._guideline = guideline;
			this._checkpoint = checkpoint;
			this._reportLevel = reportLevel;
			this._priority = priority;
			this._message = message;
			this._rule = rule;
			this._conditionalProperty = conditionalProperty;
			this._conditionalOperator = conditionalOperator;
			this._conditionalValue = conditionalValue;
			this._guidelineUrl = guidelineUrl;
		}

		// Token: 0x0600367D RID: 13949 RVA: 0x000EB5DC File Offset: 0x000EA5DC
		private VerificationAttribute()
		{
		}

		// Token: 0x17000C1B RID: 3099
		// (get) Token: 0x0600367E RID: 13950 RVA: 0x000EB5E4 File Offset: 0x000EA5E4
		public string Guideline
		{
			get
			{
				return this._guideline;
			}
		}

		// Token: 0x17000C1C RID: 3100
		// (get) Token: 0x0600367F RID: 13951 RVA: 0x000EB5EC File Offset: 0x000EA5EC
		public string Checkpoint
		{
			get
			{
				return this._checkpoint;
			}
		}

		// Token: 0x17000C1D RID: 3101
		// (get) Token: 0x06003680 RID: 13952 RVA: 0x000EB5F4 File Offset: 0x000EA5F4
		public VerificationReportLevel VerificationReportLevel
		{
			get
			{
				return this._reportLevel;
			}
		}

		// Token: 0x17000C1E RID: 3102
		// (get) Token: 0x06003681 RID: 13953 RVA: 0x000EB5FC File Offset: 0x000EA5FC
		public int Priority
		{
			get
			{
				return this._priority;
			}
		}

		// Token: 0x17000C1F RID: 3103
		// (get) Token: 0x06003682 RID: 13954 RVA: 0x000EB604 File Offset: 0x000EA604
		public string Message
		{
			get
			{
				return this._message;
			}
		}

		// Token: 0x17000C20 RID: 3104
		// (get) Token: 0x06003683 RID: 13955 RVA: 0x000EB60C File Offset: 0x000EA60C
		public VerificationRule VerificationRule
		{
			get
			{
				return this._rule;
			}
		}

		// Token: 0x17000C21 RID: 3105
		// (get) Token: 0x06003684 RID: 13956 RVA: 0x000EB614 File Offset: 0x000EA614
		public string ConditionalProperty
		{
			get
			{
				return this._conditionalProperty;
			}
		}

		// Token: 0x17000C22 RID: 3106
		// (get) Token: 0x06003685 RID: 13957 RVA: 0x000EB61C File Offset: 0x000EA61C
		public VerificationConditionalOperator VerificationConditionalOperator
		{
			get
			{
				return this._conditionalOperator;
			}
		}

		// Token: 0x17000C23 RID: 3107
		// (get) Token: 0x06003686 RID: 13958 RVA: 0x000EB624 File Offset: 0x000EA624
		public string ConditionalValue
		{
			get
			{
				return this._conditionalValue;
			}
		}

		// Token: 0x17000C24 RID: 3108
		// (get) Token: 0x06003687 RID: 13959 RVA: 0x000EB62C File Offset: 0x000EA62C
		public string GuidelineUrl
		{
			get
			{
				return this._guidelineUrl;
			}
		}

		// Token: 0x04002584 RID: 9604
		private string _guideline;

		// Token: 0x04002585 RID: 9605
		private string _checkpoint;

		// Token: 0x04002586 RID: 9606
		private VerificationReportLevel _reportLevel;

		// Token: 0x04002587 RID: 9607
		private int _priority;

		// Token: 0x04002588 RID: 9608
		private string _message;

		// Token: 0x04002589 RID: 9609
		private VerificationRule _rule;

		// Token: 0x0400258A RID: 9610
		private string _conditionalProperty;

		// Token: 0x0400258B RID: 9611
		private VerificationConditionalOperator _conditionalOperator;

		// Token: 0x0400258C RID: 9612
		private string _conditionalValue;

		// Token: 0x0400258D RID: 9613
		private string _guidelineUrl;
	}
}
