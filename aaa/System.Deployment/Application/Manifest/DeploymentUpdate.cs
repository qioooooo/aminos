using System;
using System.Deployment.Internal.Isolation.Manifest;

namespace System.Deployment.Application.Manifest
{
	// Token: 0x02000019 RID: 25
	internal class DeploymentUpdate
	{
		// Token: 0x060000D6 RID: 214 RVA: 0x00005A14 File Offset: 0x00004A14
		public DeploymentUpdate(DeploymentMetadataEntry entry)
		{
			this._beforeApplicationStartup = (entry.DeploymentFlags & 4U) != 0U;
			this._maximumAgeAllowed = DeploymentUpdate.GetTimeSpanFromItem(entry.MaximumAge, entry.MaximumAge_Unit, out this._maximumAgeCount, out this._maximumAgeUnit, out this._maximumAgeSpecified);
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00005A64 File Offset: 0x00004A64
		public bool BeforeApplicationStartup
		{
			get
			{
				return this._beforeApplicationStartup;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00005A6C File Offset: 0x00004A6C
		public bool MaximumAgeSpecified
		{
			get
			{
				return this._maximumAgeSpecified;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00005A74 File Offset: 0x00004A74
		public TimeSpan MaximumAgeAllowed
		{
			get
			{
				return this._maximumAgeAllowed;
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00005A7C File Offset: 0x00004A7C
		private static TimeSpan GetTimeSpanFromItem(ushort time, byte elapsedunit, out uint count, out timeUnitType unit, out bool specified)
		{
			specified = true;
			TimeSpan timeSpan;
			switch (elapsedunit)
			{
			case 1:
				timeSpan = TimeSpan.FromHours((double)time);
				count = (uint)time;
				unit = timeUnitType.hours;
				break;
			case 2:
				timeSpan = TimeSpan.FromDays((double)time);
				count = (uint)time;
				unit = timeUnitType.days;
				break;
			case 3:
				timeSpan = TimeSpan.FromDays((double)(time * 7));
				count = (uint)time;
				unit = timeUnitType.weeks;
				break;
			default:
				specified = false;
				timeSpan = TimeSpan.Zero;
				count = 0U;
				unit = timeUnitType.days;
				break;
			}
			return timeSpan;
		}

		// Token: 0x0400007D RID: 125
		private readonly bool _beforeApplicationStartup;

		// Token: 0x0400007E RID: 126
		private readonly bool _maximumAgeSpecified;

		// Token: 0x0400007F RID: 127
		private readonly TimeSpan _maximumAgeAllowed;

		// Token: 0x04000080 RID: 128
		private readonly uint _maximumAgeCount;

		// Token: 0x04000081 RID: 129
		private readonly timeUnitType _maximumAgeUnit;
	}
}
