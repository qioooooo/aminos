using System;
using System.ComponentModel;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000070 RID: 112
	public class ActiveDirectorySchedule
	{
		// Token: 0x06000280 RID: 640 RVA: 0x00008B08 File Offset: 0x00007B08
		public ActiveDirectorySchedule()
		{
			this.utcOffSet = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Ticks / 36000000000L;
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00008B54 File Offset: 0x00007B54
		public ActiveDirectorySchedule(ActiveDirectorySchedule schedule)
			: this()
		{
			if (schedule == null)
			{
				throw new ArgumentNullException();
			}
			bool[] array = schedule.scheduleArray;
			for (int i = 0; i < 672; i++)
			{
				this.scheduleArray[i] = array[i];
			}
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00008B94 File Offset: 0x00007B94
		internal ActiveDirectorySchedule(bool[] schedule)
			: this()
		{
			for (int i = 0; i < 672; i++)
			{
				this.scheduleArray[i] = schedule[i];
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000283 RID: 643 RVA: 0x00008BC4 File Offset: 0x00007BC4
		// (set) Token: 0x06000284 RID: 644 RVA: 0x00008C20 File Offset: 0x00007C20
		public bool[,,] RawSchedule
		{
			get
			{
				bool[,,] array = new bool[7, 24, 4];
				for (int i = 0; i < 7; i++)
				{
					for (int j = 0; j < 24; j++)
					{
						for (int k = 0; k < 4; k++)
						{
							array[i, j, k] = this.scheduleArray[i * 24 * 4 + j * 4 + k];
						}
					}
				}
				return array;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.ValidateRawArray(value);
				for (int i = 0; i < 7; i++)
				{
					for (int j = 0; j < 24; j++)
					{
						for (int k = 0; k < 4; k++)
						{
							this.scheduleArray[i * 24 * 4 + j * 4 + k] = value[i, j, k];
						}
					}
				}
			}
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00008C84 File Offset: 0x00007C84
		public void SetSchedule(DayOfWeek day, HourOfDay fromHour, MinuteOfHour fromMinute, HourOfDay toHour, MinuteOfHour toMinute)
		{
			if (day < DayOfWeek.Sunday || day > DayOfWeek.Saturday)
			{
				throw new InvalidEnumArgumentException("day", (int)day, typeof(DayOfWeek));
			}
			if (fromHour < HourOfDay.Zero || fromHour > HourOfDay.TwentyThree)
			{
				throw new InvalidEnumArgumentException("fromHour", (int)fromHour, typeof(HourOfDay));
			}
			if (fromMinute != MinuteOfHour.Zero && fromMinute != MinuteOfHour.Fifteen && fromMinute != MinuteOfHour.Thirty && fromMinute != MinuteOfHour.FortyFive)
			{
				throw new InvalidEnumArgumentException("fromMinute", (int)fromMinute, typeof(MinuteOfHour));
			}
			if (toHour < HourOfDay.Zero || toHour > HourOfDay.TwentyThree)
			{
				throw new InvalidEnumArgumentException("toHour", (int)toHour, typeof(HourOfDay));
			}
			if (toMinute != MinuteOfHour.Zero && toMinute != MinuteOfHour.Fifteen && toMinute != MinuteOfHour.Thirty && toMinute != MinuteOfHour.FortyFive)
			{
				throw new InvalidEnumArgumentException("toMinute", (int)toMinute, typeof(MinuteOfHour));
			}
			if (fromHour * (HourOfDay)60 + (int)fromMinute > toHour * (HourOfDay)60 + (int)toMinute)
			{
				throw new ArgumentException(Res.GetString("InvalidTime"));
			}
			int num = (int)(day * (DayOfWeek)24 * DayOfWeek.Thursday + (int)(fromHour * HourOfDay.Four) + (int)(fromMinute / MinuteOfHour.Fifteen));
			int num2 = (int)(day * (DayOfWeek)24 * DayOfWeek.Thursday + (int)(toHour * HourOfDay.Four) + (int)(toMinute / MinuteOfHour.Fifteen));
			for (int i = num; i <= num2; i++)
			{
				this.scheduleArray[i] = true;
			}
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00008D9C File Offset: 0x00007D9C
		public void SetSchedule(DayOfWeek[] days, HourOfDay fromHour, MinuteOfHour fromMinute, HourOfDay toHour, MinuteOfHour toMinute)
		{
			if (days == null)
			{
				throw new ArgumentNullException("days");
			}
			for (int i = 0; i < days.Length; i++)
			{
				if (days[i] < DayOfWeek.Sunday || days[i] > DayOfWeek.Saturday)
				{
					throw new InvalidEnumArgumentException("days", (int)days[i], typeof(DayOfWeek));
				}
			}
			for (int j = 0; j < days.Length; j++)
			{
				this.SetSchedule(days[j], fromHour, fromMinute, toHour, toMinute);
			}
		}

		// Token: 0x06000287 RID: 647 RVA: 0x00008E08 File Offset: 0x00007E08
		public void SetDailySchedule(HourOfDay fromHour, MinuteOfHour fromMinute, HourOfDay toHour, MinuteOfHour toMinute)
		{
			for (int i = 0; i < 7; i++)
			{
				this.SetSchedule((DayOfWeek)i, fromHour, fromMinute, toHour, toMinute);
			}
		}

		// Token: 0x06000288 RID: 648 RVA: 0x00008E30 File Offset: 0x00007E30
		public void ResetSchedule()
		{
			for (int i = 0; i < 672; i++)
			{
				this.scheduleArray[i] = false;
			}
		}

		// Token: 0x06000289 RID: 649 RVA: 0x00008E58 File Offset: 0x00007E58
		private void ValidateRawArray(bool[,,] array)
		{
			if (array.Length != 672)
			{
				throw new ArgumentException("value");
			}
			int length = array.GetLength(0);
			int length2 = array.GetLength(1);
			int length3 = array.GetLength(2);
			if (length != 7 || length2 != 24 || length3 != 4)
			{
				throw new ArgumentException("value");
			}
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00008EB0 File Offset: 0x00007EB0
		internal byte[] GetUnmanagedSchedule()
		{
			byte[] array = new byte[188];
			array[0] = 188;
			array[8] = 1;
			array[16] = 20;
			for (int i = 20; i < 188; i++)
			{
				byte b = 0;
				int num = (i - 20) * 4;
				if (this.scheduleArray[num])
				{
					b |= 1;
				}
				if (this.scheduleArray[num + 1])
				{
					b |= 2;
				}
				if (this.scheduleArray[num + 2])
				{
					b |= 4;
				}
				if (this.scheduleArray[num + 3])
				{
					b |= 8;
				}
				int num2 = i - (int)this.utcOffSet;
				if (num2 >= 188)
				{
					num2 = num2 - 188 + 20;
				}
				else if (num2 < 20)
				{
					num2 = 188 - (20 - num2);
				}
				array[num2] = b;
			}
			return array;
		}

		// Token: 0x0600028B RID: 651 RVA: 0x00008F7C File Offset: 0x00007F7C
		internal void SetUnmanagedSchedule(byte[] unmanagedSchedule)
		{
			for (int i = 20; i < 188; i++)
			{
				int num = (i - 20) * 4;
				int num2 = i - (int)this.utcOffSet;
				if (num2 >= 188)
				{
					num2 = num2 - 188 + 20;
				}
				else if (num2 < 20)
				{
					num2 = 188 - (20 - num2);
				}
				int num3 = (int)unmanagedSchedule[num2];
				if ((num3 & 1) != 0)
				{
					this.scheduleArray[num] = true;
				}
				if ((num3 & 2) != 0)
				{
					this.scheduleArray[num + 1] = true;
				}
				if ((num3 & 4) != 0)
				{
					this.scheduleArray[num + 2] = true;
				}
				if ((num3 & 8) != 0)
				{
					this.scheduleArray[num + 3] = true;
				}
			}
		}

		// Token: 0x040002C3 RID: 707
		private bool[] scheduleArray = new bool[672];

		// Token: 0x040002C4 RID: 708
		private long utcOffSet;
	}
}
