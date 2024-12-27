using System;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x0200004C RID: 76
	internal class SmiXetterAccessMap
	{
		// Token: 0x060002F0 RID: 752 RVA: 0x001CDA34 File Offset: 0x001CCE34
		internal static bool IsGetterAccessValid(SmiMetaData metaData, SmiXetterTypeCode xetterType)
		{
			return SmiXetterAccessMap.__isGetterAccessValid[(int)metaData.SqlDbType, (int)xetterType];
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x001CDA54 File Offset: 0x001CCE54
		internal static bool IsSetterAccessValid(SmiMetaData metaData, SmiXetterTypeCode xetterType)
		{
			return SmiXetterAccessMap.__isSetterAccessValid[(int)metaData.SqlDbType, (int)xetterType];
		}

		// Token: 0x040005F3 RID: 1523
		private const bool X = true;

		// Token: 0x040005F4 RID: 1524
		private const bool _ = false;

		// Token: 0x040005F5 RID: 1525
		private static bool[,] __isGetterAccessValid = new bool[,]
		{
			{
				false, false, false, false, false, false, false, true, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, true, false, false, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				true, false, false, false, false, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, true, true, true, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, true, false, false, false, false, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				true, false, false, false, false, false, false
			},
			{
				false, false, false, false, false, false, false, false, false, true,
				false, false, false, false, false, false, false
			},
			{
				false, false, true, false, false, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, false, false, false, false, true, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, false, false, false, false, false, true, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, true, true, true, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, true, true, true, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, true, true, true, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, false, false, false, false, false, false, true, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, false, true, false, false, false, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, true, false, false, false, false, false
			},
			{
				false, false, false, false, false, true, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, false, false, false, false, false, true, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, true, true, true, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, true, false, false, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, true, false, false, false, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, true, false, false, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, true, true, true, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				true, true, true, true, true, true, true, true, true, true,
				true, true, true, true, false, true, true
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, true, false, false, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, true, false, false, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, true, false, false, false, false, false, false, false,
				false, false, false, false, true, false, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, true, false, false, false, false, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, false, false, false, false, true, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, true, false, false, false, false, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, false, false, false, false, false, true
			}
		};

		// Token: 0x040005F6 RID: 1526
		private static bool[,] __isSetterAccessValid = new bool[,]
		{
			{
				false, false, false, false, false, false, false, true, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, true, false, false, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				true, false, false, false, false, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, false, true, true, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, true, false, false, false, false, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				true, false, false, false, false, false, false
			},
			{
				false, false, false, false, false, false, false, false, false, true,
				false, false, false, false, false, false, false
			},
			{
				false, false, true, false, false, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, false, false, false, false, true, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, false, false, false, false, false, true, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, false, true, true, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, false, true, true, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, false, true, true, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, false, false, false, false, false, false, true, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, false, true, false, false, false, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, true, false, false, false, false, false
			},
			{
				false, false, false, false, false, true, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, false, false, false, false, false, true, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, false, true, true, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, true, false, false, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, true, false, false, false, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, true, false, false, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, false, true, true, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				true, true, true, true, true, true, true, true, true, true,
				true, true, true, true, false, true, true
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, true, false, true, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, true, false, false, false, false, false, false, false,
				false, false, false, false, false, false, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, false, false, false, true, false, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, true, false, false, false, false, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, false, false, false, false, true, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, true, false, false, false, false, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, false, false, false, false, false, true
			}
		};
	}
}
