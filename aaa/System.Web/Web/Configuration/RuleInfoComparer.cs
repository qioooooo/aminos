using System;
using System.Collections;

namespace System.Web.Configuration
{
	// Token: 0x02000241 RID: 577
	internal class RuleInfoComparer : IComparer
	{
		// Token: 0x06001EAA RID: 7850 RVA: 0x000896CC File Offset: 0x000886CC
		public int Compare(object x, object y)
		{
			Type realType = ((HealthMonitoringSectionHelper.RuleInfo)x)._eventMappingSettings.RealType;
			Type realType2 = ((HealthMonitoringSectionHelper.RuleInfo)y)._eventMappingSettings.RealType;
			int num;
			if (realType.Equals(realType2))
			{
				num = 0;
			}
			else if (realType.IsSubclassOf(realType2))
			{
				num = 1;
			}
			else
			{
				if (!realType2.IsSubclassOf(realType))
				{
					return string.Compare(realType.ToString(), realType2.ToString(), StringComparison.Ordinal);
				}
				num = -1;
			}
			return num;
		}
	}
}
