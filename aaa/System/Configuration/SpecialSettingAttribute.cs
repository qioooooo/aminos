using System;

namespace System.Configuration
{
	// Token: 0x0200070E RID: 1806
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
	public sealed class SpecialSettingAttribute : Attribute
	{
		// Token: 0x06003751 RID: 14161 RVA: 0x000EB00A File Offset: 0x000EA00A
		public SpecialSettingAttribute(SpecialSetting specialSetting)
		{
			this._specialSetting = specialSetting;
		}

		// Token: 0x17000CCE RID: 3278
		// (get) Token: 0x06003752 RID: 14162 RVA: 0x000EB019 File Offset: 0x000EA019
		public SpecialSetting SpecialSetting
		{
			get
			{
				return this._specialSetting;
			}
		}

		// Token: 0x040031B9 RID: 12729
		private readonly SpecialSetting _specialSetting;
	}
}
