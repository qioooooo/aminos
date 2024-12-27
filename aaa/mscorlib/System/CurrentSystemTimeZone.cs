﻿using System;
using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System
{
	// Token: 0x0200009E RID: 158
	[Serializable]
	internal class CurrentSystemTimeZone : TimeZone
	{
		// Token: 0x06000959 RID: 2393 RVA: 0x0001C762 File Offset: 0x0001B762
		internal CurrentSystemTimeZone()
		{
			this.m_ticksOffset = (long)CurrentSystemTimeZone.nativeGetTimeZoneMinuteOffset() * 600000000L;
			this.m_standardName = null;
			this.m_daylightName = null;
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600095A RID: 2394 RVA: 0x0001C796 File Offset: 0x0001B796
		public override string StandardName
		{
			get
			{
				if (this.m_standardName == null)
				{
					this.m_standardName = CurrentSystemTimeZone.nativeGetStandardName();
				}
				return this.m_standardName;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600095B RID: 2395 RVA: 0x0001C7B1 File Offset: 0x0001B7B1
		public override string DaylightName
		{
			get
			{
				if (this.m_daylightName == null)
				{
					this.m_daylightName = CurrentSystemTimeZone.nativeGetDaylightName();
					if (this.m_daylightName == null)
					{
						this.m_daylightName = this.StandardName;
					}
				}
				return this.m_daylightName;
			}
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x0001C7E0 File Offset: 0x0001B7E0
		internal long GetUtcOffsetFromUniversalTime(DateTime time, ref bool isAmbiguousLocalDst)
		{
			TimeSpan timeSpan = new TimeSpan(this.m_ticksOffset);
			DaylightTime daylightChanges = this.GetDaylightChanges(time.Year);
			isAmbiguousLocalDst = false;
			if (daylightChanges == null || daylightChanges.Delta.Ticks == 0L)
			{
				return timeSpan.Ticks;
			}
			DateTime dateTime = daylightChanges.Start - timeSpan;
			DateTime dateTime2 = daylightChanges.End - timeSpan - daylightChanges.Delta;
			DateTime dateTime3;
			DateTime dateTime4;
			if (daylightChanges.Delta.Ticks > 0L)
			{
				dateTime3 = dateTime2 - daylightChanges.Delta;
				dateTime4 = dateTime2;
			}
			else
			{
				dateTime3 = dateTime;
				dateTime4 = dateTime - daylightChanges.Delta;
			}
			bool flag;
			if (dateTime > dateTime2)
			{
				flag = time < dateTime2 || time >= dateTime;
			}
			else
			{
				flag = time >= dateTime && time < dateTime2;
			}
			if (flag)
			{
				timeSpan += daylightChanges.Delta;
				if (time >= dateTime3 && time < dateTime4)
				{
					isAmbiguousLocalDst = true;
				}
			}
			return timeSpan.Ticks;
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x0001C8EC File Offset: 0x0001B8EC
		public override DateTime ToLocalTime(DateTime time)
		{
			if (time.Kind == DateTimeKind.Local)
			{
				return time;
			}
			bool flag = false;
			long utcOffsetFromUniversalTime = this.GetUtcOffsetFromUniversalTime(time, ref flag);
			long num = time.Ticks + utcOffsetFromUniversalTime;
			if (num > 3155378975999999999L)
			{
				return new DateTime(3155378975999999999L, DateTimeKind.Local);
			}
			if (num < 0L)
			{
				return new DateTime(0L, DateTimeKind.Local);
			}
			return new DateTime(num, DateTimeKind.Local, flag);
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600095E RID: 2398 RVA: 0x0001C950 File Offset: 0x0001B950
		private static object InternalSyncObject
		{
			get
			{
				if (CurrentSystemTimeZone.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref CurrentSystemTimeZone.s_InternalSyncObject, obj, null);
				}
				return CurrentSystemTimeZone.s_InternalSyncObject;
			}
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0001C97C File Offset: 0x0001B97C
		public override DaylightTime GetDaylightChanges(int year)
		{
			if (year < 1 || year > 9999)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 1, 9999 }));
			}
			object obj = year;
			if (!this.m_CachedDaylightChanges.Contains(obj))
			{
				lock (CurrentSystemTimeZone.InternalSyncObject)
				{
					if (!this.m_CachedDaylightChanges.Contains(obj))
					{
						short[] array = CurrentSystemTimeZone.nativeGetDaylightChanges();
						if (array == null)
						{
							this.m_CachedDaylightChanges.Add(obj, new DaylightTime(DateTime.MinValue, DateTime.MinValue, TimeSpan.Zero));
						}
						else
						{
							DateTime dayOfWeek = CurrentSystemTimeZone.GetDayOfWeek(year, array[0] != 0, (int)array[1], (int)array[2], (int)array[3], (int)array[4], (int)array[5], (int)array[6], (int)array[7]);
							DateTime dayOfWeek2 = CurrentSystemTimeZone.GetDayOfWeek(year, array[8] != 0, (int)array[9], (int)array[10], (int)array[11], (int)array[12], (int)array[13], (int)array[14], (int)array[15]);
							TimeSpan timeSpan = new TimeSpan((long)array[16] * 600000000L);
							DaylightTime daylightTime = new DaylightTime(dayOfWeek, dayOfWeek2, timeSpan);
							this.m_CachedDaylightChanges.Add(obj, daylightTime);
						}
					}
				}
			}
			return (DaylightTime)this.m_CachedDaylightChanges[obj];
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x0001CAE4 File Offset: 0x0001BAE4
		public override TimeSpan GetUtcOffset(DateTime time)
		{
			if (time.Kind == DateTimeKind.Utc)
			{
				return TimeSpan.Zero;
			}
			return new TimeSpan(TimeZone.CalculateUtcOffset(time, this.GetDaylightChanges(time.Year)).Ticks + this.m_ticksOffset);
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0001CB28 File Offset: 0x0001BB28
		private static DateTime GetDayOfWeek(int year, bool fixedDate, int month, int targetDayOfWeek, int numberOfSunday, int hour, int minute, int second, int millisecond)
		{
			DateTime dateTime;
			if (fixedDate)
			{
				int num = DateTime.DaysInMonth(year, month);
				dateTime = new DateTime(year, month, (num < numberOfSunday) ? num : numberOfSunday, hour, minute, second, millisecond, DateTimeKind.Local);
			}
			else if (numberOfSunday <= 4)
			{
				dateTime = new DateTime(year, month, 1, hour, minute, second, millisecond, DateTimeKind.Local);
				int dayOfWeek = (int)dateTime.DayOfWeek;
				int num2 = targetDayOfWeek - dayOfWeek;
				if (num2 < 0)
				{
					num2 += 7;
				}
				num2 += 7 * (numberOfSunday - 1);
				if (num2 > 0)
				{
					dateTime = dateTime.AddDays((double)num2);
				}
			}
			else
			{
				Calendar defaultInstance = GregorianCalendar.GetDefaultInstance();
				dateTime = new DateTime(year, month, defaultInstance.GetDaysInMonth(year, month), hour, minute, second, millisecond, DateTimeKind.Local);
				int dayOfWeek2 = (int)dateTime.DayOfWeek;
				int num3 = dayOfWeek2 - targetDayOfWeek;
				if (num3 < 0)
				{
					num3 += 7;
				}
				if (num3 > 0)
				{
					dateTime = dateTime.AddDays((double)(-(double)num3));
				}
			}
			return dateTime;
		}

		// Token: 0x06000962 RID: 2402
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int nativeGetTimeZoneMinuteOffset();

		// Token: 0x06000963 RID: 2403
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string nativeGetDaylightName();

		// Token: 0x06000964 RID: 2404
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string nativeGetStandardName();

		// Token: 0x06000965 RID: 2405
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern short[] nativeGetDaylightChanges();

		// Token: 0x0400036D RID: 877
		private const long TicksPerMillisecond = 10000L;

		// Token: 0x0400036E RID: 878
		private const long TicksPerSecond = 10000000L;

		// Token: 0x0400036F RID: 879
		private const long TicksPerMinute = 600000000L;

		// Token: 0x04000370 RID: 880
		private Hashtable m_CachedDaylightChanges = new Hashtable();

		// Token: 0x04000371 RID: 881
		private long m_ticksOffset;

		// Token: 0x04000372 RID: 882
		private string m_standardName;

		// Token: 0x04000373 RID: 883
		private string m_daylightName;

		// Token: 0x04000374 RID: 884
		private static object s_InternalSyncObject;
	}
}
