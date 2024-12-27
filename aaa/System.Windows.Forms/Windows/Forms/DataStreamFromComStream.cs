using System;
using System.IO;

namespace System.Windows.Forms
{
	// Token: 0x020003A9 RID: 937
	internal class DataStreamFromComStream : Stream
	{
		// Token: 0x06003913 RID: 14611 RVA: 0x000D155E File Offset: 0x000D055E
		public DataStreamFromComStream(UnsafeNativeMethods.IStream comStream)
		{
			this.comStream = comStream;
		}

		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x06003914 RID: 14612 RVA: 0x000D156D File Offset: 0x000D056D
		// (set) Token: 0x06003915 RID: 14613 RVA: 0x000D1578 File Offset: 0x000D0578
		public override long Position
		{
			get
			{
				return this.Seek(0L, SeekOrigin.Current);
			}
			set
			{
				this.Seek(value, SeekOrigin.Begin);
			}
		}

		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x06003916 RID: 14614 RVA: 0x000D1583 File Offset: 0x000D0583
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x06003917 RID: 14615 RVA: 0x000D1586 File Offset: 0x000D0586
		public override bool CanSeek
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x06003918 RID: 14616 RVA: 0x000D1589 File Offset: 0x000D0589
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000AA8 RID: 2728
		// (get) Token: 0x06003919 RID: 14617 RVA: 0x000D158C File Offset: 0x000D058C
		public override long Length
		{
			get
			{
				long position = this.Position;
				long num = this.Seek(0L, SeekOrigin.End);
				this.Position = position;
				return num - position;
			}
		}

		// Token: 0x0600391A RID: 14618 RVA: 0x000D15B4 File Offset: 0x000D05B4
		private unsafe int _Read(void* handle, int bytes)
		{
			return this.comStream.Read((IntPtr)handle, bytes);
		}

		// Token: 0x0600391B RID: 14619 RVA: 0x000D15C8 File Offset: 0x000D05C8
		private unsafe int _Write(void* handle, int bytes)
		{
			return this.comStream.Write((IntPtr)handle, bytes);
		}

		// Token: 0x0600391C RID: 14620 RVA: 0x000D15DC File Offset: 0x000D05DC
		public override void Flush()
		{
		}

		// Token: 0x0600391D RID: 14621 RVA: 0x000D15E0 File Offset: 0x000D05E0
		public unsafe override int Read(byte[] buffer, int index, int count)
		{
			int num = 0;
			if (count > 0 && index >= 0 && count + index <= buffer.Length)
			{
				fixed (byte* ptr = buffer)
				{
					num = this._Read((void*)((byte*)ptr + index), count);
				}
			}
			return num;
		}

		// Token: 0x0600391E RID: 14622 RVA: 0x000D1626 File Offset: 0x000D0626
		public override void SetLength(long value)
		{
			this.comStream.SetSize(value);
		}

		// Token: 0x0600391F RID: 14623 RVA: 0x000D1634 File Offset: 0x000D0634
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.comStream.Seek(offset, (int)origin);
		}

		// Token: 0x06003920 RID: 14624 RVA: 0x000D1644 File Offset: 0x000D0644
		public unsafe override void Write(byte[] buffer, int index, int count)
		{
			int num = 0;
			if (count > 0 && index >= 0 && count + index <= buffer.Length)
			{
				try
				{
					try
					{
						fixed (byte* ptr = buffer)
						{
							num = this._Write((void*)((byte*)ptr + index), count);
						}
					}
					finally
					{
						byte* ptr = null;
					}
				}
				catch
				{
				}
			}
			if (num < count)
			{
				throw new IOException(SR.GetString("DataStreamWrite"));
			}
		}

		// Token: 0x06003921 RID: 14625 RVA: 0x000D16C4 File Offset: 0x000D06C4
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && this.comStream != null)
				{
					try
					{
						this.comStream.Commit(0);
					}
					catch (Exception)
					{
					}
				}
				this.comStream = null;
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06003922 RID: 14626 RVA: 0x000D171C File Offset: 0x000D071C
		~DataStreamFromComStream()
		{
			this.Dispose(false);
		}

		// Token: 0x04001CA4 RID: 7332
		private UnsafeNativeMethods.IStream comStream;
	}
}
