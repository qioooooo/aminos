using System;

namespace System.Windows.Forms
{
	// Token: 0x02000243 RID: 579
	public struct BindingMemberInfo
	{
		// Token: 0x06001BAC RID: 7084 RVA: 0x00035E20 File Offset: 0x00034E20
		public BindingMemberInfo(string dataMember)
		{
			if (dataMember == null)
			{
				dataMember = "";
			}
			int num = dataMember.LastIndexOf(".");
			if (num != -1)
			{
				this.dataList = dataMember.Substring(0, num);
				this.dataField = dataMember.Substring(num + 1);
				return;
			}
			this.dataList = "";
			this.dataField = dataMember;
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06001BAD RID: 7085 RVA: 0x00035E77 File Offset: 0x00034E77
		public string BindingPath
		{
			get
			{
				if (this.dataList == null)
				{
					return "";
				}
				return this.dataList;
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06001BAE RID: 7086 RVA: 0x00035E8D File Offset: 0x00034E8D
		public string BindingField
		{
			get
			{
				if (this.dataField == null)
				{
					return "";
				}
				return this.dataField;
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06001BAF RID: 7087 RVA: 0x00035EA3 File Offset: 0x00034EA3
		public string BindingMember
		{
			get
			{
				if (this.BindingPath.Length <= 0)
				{
					return this.BindingField;
				}
				return this.BindingPath + "." + this.BindingField;
			}
		}

		// Token: 0x06001BB0 RID: 7088 RVA: 0x00035ED0 File Offset: 0x00034ED0
		public override bool Equals(object otherObject)
		{
			if (otherObject is BindingMemberInfo)
			{
				BindingMemberInfo bindingMemberInfo = (BindingMemberInfo)otherObject;
				return string.Equals(this.BindingMember, bindingMemberInfo.BindingMember, StringComparison.OrdinalIgnoreCase);
			}
			return false;
		}

		// Token: 0x06001BB1 RID: 7089 RVA: 0x00035F01 File Offset: 0x00034F01
		public static bool operator ==(BindingMemberInfo a, BindingMemberInfo b)
		{
			return a.Equals(b);
		}

		// Token: 0x06001BB2 RID: 7090 RVA: 0x00035F16 File Offset: 0x00034F16
		public static bool operator !=(BindingMemberInfo a, BindingMemberInfo b)
		{
			return !a.Equals(b);
		}

		// Token: 0x06001BB3 RID: 7091 RVA: 0x00035F2E File Offset: 0x00034F2E
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04001327 RID: 4903
		private string dataList;

		// Token: 0x04001328 RID: 4904
		private string dataField;
	}
}
