using System;

namespace System.Web.Util
{
	// Token: 0x02000767 RID: 1895
	internal struct FileTimeInfo
	{
		// Token: 0x06005C07 RID: 23559 RVA: 0x001715BB File Offset: 0x001705BB
		internal FileTimeInfo(long lastWriteTime, long size)
		{
			this.LastWriteTime = lastWriteTime;
			this.Size = size;
		}

		// Token: 0x06005C08 RID: 23560 RVA: 0x001715CC File Offset: 0x001705CC
		public override bool Equals(object obj)
		{
			if (obj is FileTimeInfo)
			{
				FileTimeInfo fileTimeInfo = (FileTimeInfo)obj;
				return this.LastWriteTime == fileTimeInfo.LastWriteTime && this.Size == fileTimeInfo.Size;
			}
			return false;
		}

		// Token: 0x06005C09 RID: 23561 RVA: 0x0017160A File Offset: 0x0017060A
		public static bool operator ==(FileTimeInfo value1, FileTimeInfo value2)
		{
			return value1.LastWriteTime == value2.LastWriteTime && value1.Size == value2.Size;
		}

		// Token: 0x06005C0A RID: 23562 RVA: 0x0017162E File Offset: 0x0017062E
		public static bool operator !=(FileTimeInfo value1, FileTimeInfo value2)
		{
			return !(value1 == value2);
		}

		// Token: 0x06005C0B RID: 23563 RVA: 0x0017163A File Offset: 0x0017063A
		public override int GetHashCode()
		{
			return HashCodeCombiner.CombineHashCodes(this.LastWriteTime.GetHashCode(), this.Size.GetHashCode());
		}

		// Token: 0x0400313B RID: 12603
		internal long LastWriteTime;

		// Token: 0x0400313C RID: 12604
		internal long Size;

		// Token: 0x0400313D RID: 12605
		internal static readonly FileTimeInfo MinValue = new FileTimeInfo(0L, 0L);
	}
}
