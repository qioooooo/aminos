using System;

namespace System.Data.Common
{
	// Token: 0x02000143 RID: 323
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	[Serializable]
	public sealed class DbProviderSpecificTypePropertyAttribute : Attribute
	{
		// Token: 0x060014FE RID: 5374 RVA: 0x00228D6C File Offset: 0x0022816C
		public DbProviderSpecificTypePropertyAttribute(bool isProviderSpecificTypeProperty)
		{
			this._isProviderSpecificTypeProperty = isProviderSpecificTypeProperty;
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x060014FF RID: 5375 RVA: 0x00228D88 File Offset: 0x00228188
		public bool IsProviderSpecificTypeProperty
		{
			get
			{
				return this._isProviderSpecificTypeProperty;
			}
		}

		// Token: 0x04000C58 RID: 3160
		private bool _isProviderSpecificTypeProperty;
	}
}
