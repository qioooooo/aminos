using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000481 RID: 1153
	[AttributeUsage(AttributeTargets.Property)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class UrlPropertyAttribute : Attribute
	{
		// Token: 0x06003602 RID: 13826 RVA: 0x000E9921 File Offset: 0x000E8921
		public UrlPropertyAttribute()
			: this("*.*")
		{
		}

		// Token: 0x06003603 RID: 13827 RVA: 0x000E992E File Offset: 0x000E892E
		public UrlPropertyAttribute(string filter)
		{
			if (filter == null)
			{
				this._filter = "*.*";
				return;
			}
			this._filter = filter;
		}

		// Token: 0x17000C14 RID: 3092
		// (get) Token: 0x06003604 RID: 13828 RVA: 0x000E994C File Offset: 0x000E894C
		public string Filter
		{
			get
			{
				return this._filter;
			}
		}

		// Token: 0x06003605 RID: 13829 RVA: 0x000E9954 File Offset: 0x000E8954
		public override int GetHashCode()
		{
			return this.Filter.GetHashCode();
		}

		// Token: 0x06003606 RID: 13830 RVA: 0x000E9964 File Offset: 0x000E8964
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			UrlPropertyAttribute urlPropertyAttribute = obj as UrlPropertyAttribute;
			return urlPropertyAttribute != null && this.Filter.Equals(urlPropertyAttribute.Filter);
		}

		// Token: 0x0400257D RID: 9597
		private string _filter;
	}
}
