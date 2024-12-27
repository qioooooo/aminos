using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006EC RID: 1772
	[AttributeUsage(AttributeTargets.Property)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebDisplayNameAttribute : Attribute
	{
		// Token: 0x060056C4 RID: 22212 RVA: 0x0015E295 File Offset: 0x0015D295
		public WebDisplayNameAttribute()
			: this(string.Empty)
		{
		}

		// Token: 0x060056C5 RID: 22213 RVA: 0x0015E2A2 File Offset: 0x0015D2A2
		public WebDisplayNameAttribute(string displayName)
		{
			this._displayName = displayName;
		}

		// Token: 0x17001666 RID: 5734
		// (get) Token: 0x060056C6 RID: 22214 RVA: 0x0015E2B1 File Offset: 0x0015D2B1
		public virtual string DisplayName
		{
			get
			{
				return this.DisplayNameValue;
			}
		}

		// Token: 0x17001667 RID: 5735
		// (get) Token: 0x060056C7 RID: 22215 RVA: 0x0015E2B9 File Offset: 0x0015D2B9
		// (set) Token: 0x060056C8 RID: 22216 RVA: 0x0015E2C1 File Offset: 0x0015D2C1
		protected string DisplayNameValue
		{
			get
			{
				return this._displayName;
			}
			set
			{
				this._displayName = value;
			}
		}

		// Token: 0x060056C9 RID: 22217 RVA: 0x0015E2CC File Offset: 0x0015D2CC
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			WebDisplayNameAttribute webDisplayNameAttribute = obj as WebDisplayNameAttribute;
			return webDisplayNameAttribute != null && webDisplayNameAttribute.DisplayName == this.DisplayName;
		}

		// Token: 0x060056CA RID: 22218 RVA: 0x0015E2FC File Offset: 0x0015D2FC
		public override int GetHashCode()
		{
			return this.DisplayName.GetHashCode();
		}

		// Token: 0x060056CB RID: 22219 RVA: 0x0015E309 File Offset: 0x0015D309
		public override bool IsDefaultAttribute()
		{
			return this.Equals(WebDisplayNameAttribute.Default);
		}

		// Token: 0x04002F74 RID: 12148
		public static readonly WebDisplayNameAttribute Default = new WebDisplayNameAttribute();

		// Token: 0x04002F75 RID: 12149
		private string _displayName;
	}
}
