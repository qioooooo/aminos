using System;
using System.Web.Configuration;

namespace System.Web.Caching
{
	// Token: 0x0200010D RID: 269
	internal class CacheMemoryStats
	{
		// Token: 0x06000C84 RID: 3204 RVA: 0x00031C37 File Offset: 0x00030C37
		internal CacheMemoryStats()
		{
			CacheMemoryPressure.GetMemoryStatusOnce();
			this._pressureTotalMemory = new CacheMemoryTotalMemoryPressure();
			this._pressurePrivateBytes = new CacheMemoryPrivateBytesPressure();
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06000C85 RID: 3205 RVA: 0x00031C62 File Offset: 0x00030C62
		internal CacheMemoryPrivateBytesPressure PrivateBytesPressure
		{
			get
			{
				return this._pressurePrivateBytes;
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06000C86 RID: 3206 RVA: 0x00031C6A File Offset: 0x00030C6A
		internal CacheMemoryTotalMemoryPressure TotalMemoryPressure
		{
			get
			{
				return this._pressureTotalMemory;
			}
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x00031C72 File Offset: 0x00030C72
		internal bool IsGcCollectIneffective(long totalMemoryChange)
		{
			if (this._minTotalMemoryChange == -1L && this._pressurePrivateBytes.HasLimit())
			{
				this._minTotalMemoryChange = this._pressurePrivateBytes.MemoryLimit / 100L;
			}
			this._lastTotalMemoryChange = totalMemoryChange;
			return totalMemoryChange < this._minTotalMemoryChange;
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x00031CB0 File Offset: 0x00030CB0
		internal bool IsAboveHighPressure()
		{
			return this._pressureTotalMemory.IsAboveHighPressure() || this._pressurePrivateBytes.IsAboveHighPressure();
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x00031CCC File Offset: 0x00030CCC
		internal bool IsAboveMediumPressure()
		{
			return this._pressureTotalMemory.IsAboveMediumPressure() || this._pressurePrivateBytes.IsAboveMediumPressure();
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x00031CE8 File Offset: 0x00030CE8
		internal void ReadConfig(CacheSection cacheSection)
		{
			this._pressureTotalMemory.ReadConfig(cacheSection);
			this._pressurePrivateBytes.ReadConfig(cacheSection);
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x00031D02 File Offset: 0x00030D02
		internal void Update()
		{
			this._pressureTotalMemory.Update();
			this._pressurePrivateBytes.Update();
		}

		// Token: 0x04001447 RID: 5191
		private CacheMemoryTotalMemoryPressure _pressureTotalMemory;

		// Token: 0x04001448 RID: 5192
		private CacheMemoryPrivateBytesPressure _pressurePrivateBytes;

		// Token: 0x04001449 RID: 5193
		private long _minTotalMemoryChange = -1L;

		// Token: 0x0400144A RID: 5194
		private long _lastTotalMemoryChange;
	}
}
