using System;

namespace System.Configuration
{
	// Token: 0x0200070B RID: 1803
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
	public sealed class SettingsManageabilityAttribute : Attribute
	{
		// Token: 0x0600374A RID: 14154 RVA: 0x000EAFAE File Offset: 0x000E9FAE
		public SettingsManageabilityAttribute(SettingsManageability manageability)
		{
			this._manageability = manageability;
		}

		// Token: 0x17000CCB RID: 3275
		// (get) Token: 0x0600374B RID: 14155 RVA: 0x000EAFBD File Offset: 0x000E9FBD
		public SettingsManageability Manageability
		{
			get
			{
				return this._manageability;
			}
		}

		// Token: 0x040031B6 RID: 12726
		private readonly SettingsManageability _manageability;
	}
}
