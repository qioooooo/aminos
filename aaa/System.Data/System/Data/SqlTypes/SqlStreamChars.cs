using System;
using System.IO;

namespace System.Data.SqlTypes
{
	// Token: 0x020002C1 RID: 705
	internal abstract class SqlStreamChars : INullable, IDisposable
	{
		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x0600238F RID: 9103
		public abstract bool IsNull { get; }

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06002390 RID: 9104
		public abstract bool CanRead { get; }

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06002391 RID: 9105
		public abstract bool CanSeek { get; }

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06002392 RID: 9106
		public abstract bool CanWrite { get; }

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06002393 RID: 9107
		public abstract long Length { get; }

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06002394 RID: 9108
		// (set) Token: 0x06002395 RID: 9109
		public abstract long Position { get; set; }

		// Token: 0x06002396 RID: 9110
		public abstract int Read(char[] buffer, int offset, int count);

		// Token: 0x06002397 RID: 9111
		public abstract void Write(char[] buffer, int offset, int count);

		// Token: 0x06002398 RID: 9112
		public abstract long Seek(long offset, SeekOrigin origin);

		// Token: 0x06002399 RID: 9113
		public abstract void SetLength(long value);

		// Token: 0x0600239A RID: 9114
		public abstract void Flush();

		// Token: 0x0600239B RID: 9115 RVA: 0x00272428 File Offset: 0x00271828
		public virtual void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x0600239C RID: 9116 RVA: 0x0027243C File Offset: 0x0027183C
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x0600239D RID: 9117 RVA: 0x00272450 File Offset: 0x00271850
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x0600239E RID: 9118 RVA: 0x00272460 File Offset: 0x00271860
		public virtual int ReadChar()
		{
			char[] array = new char[1];
			if (this.Read(array, 0, 1) == 0)
			{
				return -1;
			}
			return (int)array[0];
		}

		// Token: 0x0600239F RID: 9119 RVA: 0x00272488 File Offset: 0x00271888
		public virtual void WriteChar(char value)
		{
			this.Write(new char[] { value }, 0, 1);
		}

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x060023A0 RID: 9120 RVA: 0x002724AC File Offset: 0x002718AC
		public static SqlStreamChars Null
		{
			get
			{
				return new SqlStreamChars.NullSqlStreamChars();
			}
		}

		// Token: 0x020002C2 RID: 706
		private class NullSqlStreamChars : SqlStreamChars
		{
			// Token: 0x060023A2 RID: 9122 RVA: 0x002724D4 File Offset: 0x002718D4
			internal NullSqlStreamChars()
			{
			}

			// Token: 0x17000550 RID: 1360
			// (get) Token: 0x060023A3 RID: 9123 RVA: 0x002724E8 File Offset: 0x002718E8
			public override bool IsNull
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000551 RID: 1361
			// (get) Token: 0x060023A4 RID: 9124 RVA: 0x002724F8 File Offset: 0x002718F8
			public override bool CanRead
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000552 RID: 1362
			// (get) Token: 0x060023A5 RID: 9125 RVA: 0x00272508 File Offset: 0x00271908
			public override bool CanSeek
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000553 RID: 1363
			// (get) Token: 0x060023A6 RID: 9126 RVA: 0x00272518 File Offset: 0x00271918
			public override bool CanWrite
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000554 RID: 1364
			// (get) Token: 0x060023A7 RID: 9127 RVA: 0x00272528 File Offset: 0x00271928
			public override long Length
			{
				get
				{
					throw new SqlNullValueException();
				}
			}

			// Token: 0x17000555 RID: 1365
			// (get) Token: 0x060023A8 RID: 9128 RVA: 0x0027253C File Offset: 0x0027193C
			// (set) Token: 0x060023A9 RID: 9129 RVA: 0x00272550 File Offset: 0x00271950
			public override long Position
			{
				get
				{
					throw new SqlNullValueException();
				}
				set
				{
					throw new SqlNullValueException();
				}
			}

			// Token: 0x060023AA RID: 9130 RVA: 0x00272564 File Offset: 0x00271964
			public override int Read(char[] buffer, int offset, int count)
			{
				throw new SqlNullValueException();
			}

			// Token: 0x060023AB RID: 9131 RVA: 0x00272578 File Offset: 0x00271978
			public override void Write(char[] buffer, int offset, int count)
			{
				throw new SqlNullValueException();
			}

			// Token: 0x060023AC RID: 9132 RVA: 0x0027258C File Offset: 0x0027198C
			public override long Seek(long offset, SeekOrigin origin)
			{
				throw new SqlNullValueException();
			}

			// Token: 0x060023AD RID: 9133 RVA: 0x002725A0 File Offset: 0x002719A0
			public override void SetLength(long value)
			{
				throw new SqlNullValueException();
			}

			// Token: 0x060023AE RID: 9134 RVA: 0x002725B4 File Offset: 0x002719B4
			public override void Flush()
			{
				throw new SqlNullValueException();
			}

			// Token: 0x060023AF RID: 9135 RVA: 0x002725C8 File Offset: 0x002719C8
			public override void Close()
			{
			}
		}
	}
}
