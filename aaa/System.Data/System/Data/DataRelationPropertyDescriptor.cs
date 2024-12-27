using System;
using System.ComponentModel;

namespace System.Data
{
	// Token: 0x02000080 RID: 128
	internal sealed class DataRelationPropertyDescriptor : PropertyDescriptor
	{
		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000769 RID: 1897 RVA: 0x001DF804 File Offset: 0x001DEC04
		internal DataRelation Relation
		{
			get
			{
				return this.relation;
			}
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x001DF818 File Offset: 0x001DEC18
		internal DataRelationPropertyDescriptor(DataRelation dataRelation)
			: base(dataRelation.RelationName, null)
		{
			this.relation = dataRelation;
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600076B RID: 1899 RVA: 0x001DF83C File Offset: 0x001DEC3C
		public override Type ComponentType
		{
			get
			{
				return typeof(DataRowView);
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600076C RID: 1900 RVA: 0x001DF854 File Offset: 0x001DEC54
		public override bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600076D RID: 1901 RVA: 0x001DF864 File Offset: 0x001DEC64
		public override Type PropertyType
		{
			get
			{
				return typeof(IBindingList);
			}
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x001DF87C File Offset: 0x001DEC7C
		public override bool Equals(object other)
		{
			if (other is DataRelationPropertyDescriptor)
			{
				DataRelationPropertyDescriptor dataRelationPropertyDescriptor = (DataRelationPropertyDescriptor)other;
				return dataRelationPropertyDescriptor.Relation == this.Relation;
			}
			return false;
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x001DF8A8 File Offset: 0x001DECA8
		public override int GetHashCode()
		{
			return this.Relation.GetHashCode();
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x001DF8C0 File Offset: 0x001DECC0
		public override bool CanResetValue(object component)
		{
			return false;
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x001DF8D0 File Offset: 0x001DECD0
		public override object GetValue(object component)
		{
			DataRowView dataRowView = (DataRowView)component;
			return dataRowView.CreateChildView(this.relation);
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x001DF8F0 File Offset: 0x001DECF0
		public override void ResetValue(object component)
		{
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x001DF900 File Offset: 0x001DED00
		public override void SetValue(object component, object value)
		{
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x001DF910 File Offset: 0x001DED10
		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}

		// Token: 0x04000733 RID: 1843
		private DataRelation relation;
	}
}
