using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace System.IO
{
	// Token: 0x020005AF RID: 1455
	[ComVisible(true)]
	[Serializable]
	public abstract class TextReader : MarshalByRefObject, IDisposable
	{
		// Token: 0x0600367A RID: 13946 RVA: 0x000B925F File Offset: 0x000B825F
		public virtual void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x0600367B RID: 13947 RVA: 0x000B9268 File Offset: 0x000B8268
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x0600367C RID: 13948 RVA: 0x000B9271 File Offset: 0x000B8271
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x0600367D RID: 13949 RVA: 0x000B9273 File Offset: 0x000B8273
		public virtual int Peek()
		{
			return -1;
		}

		// Token: 0x0600367E RID: 13950 RVA: 0x000B9276 File Offset: 0x000B8276
		public virtual int Read()
		{
			return -1;
		}

		// Token: 0x0600367F RID: 13951 RVA: 0x000B927C File Offset: 0x000B827C
		public virtual int Read([In] [Out] char[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", Environment.GetResourceString("ArgumentNull_Buffer"));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			int num = 0;
			do
			{
				int num2 = this.Read();
				if (num2 == -1)
				{
					break;
				}
				buffer[index + num++] = (char)num2;
			}
			while (num < count);
			return num;
		}

		// Token: 0x06003680 RID: 13952 RVA: 0x000B9308 File Offset: 0x000B8308
		public virtual string ReadToEnd()
		{
			char[] array = new char[4096];
			StringBuilder stringBuilder = new StringBuilder(4096);
			int num;
			while ((num = this.Read(array, 0, array.Length)) != 0)
			{
				stringBuilder.Append(array, 0, num);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06003681 RID: 13953 RVA: 0x000B934C File Offset: 0x000B834C
		public virtual int ReadBlock([In] [Out] char[] buffer, int index, int count)
		{
			int num = 0;
			int num2;
			do
			{
				num += (num2 = this.Read(buffer, index + num, count - num));
			}
			while (num2 > 0 && num < count);
			return num;
		}

		// Token: 0x06003682 RID: 13954 RVA: 0x000B9378 File Offset: 0x000B8378
		public virtual string ReadLine()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num;
			for (;;)
			{
				num = this.Read();
				if (num == -1)
				{
					goto IL_0043;
				}
				if (num == 13 || num == 10)
				{
					break;
				}
				stringBuilder.Append((char)num);
			}
			if (num == 13 && this.Peek() == 10)
			{
				this.Read();
			}
			return stringBuilder.ToString();
			IL_0043:
			if (stringBuilder.Length > 0)
			{
				return stringBuilder.ToString();
			}
			return null;
		}

		// Token: 0x06003683 RID: 13955 RVA: 0x000B93D9 File Offset: 0x000B83D9
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		public static TextReader Synchronized(TextReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			if (reader is TextReader.SyncTextReader)
			{
				return reader;
			}
			return new TextReader.SyncTextReader(reader);
		}

		// Token: 0x04001C5C RID: 7260
		public static readonly TextReader Null = new TextReader.NullTextReader();

		// Token: 0x020005B0 RID: 1456
		[Serializable]
		private sealed class NullTextReader : TextReader
		{
			// Token: 0x06003685 RID: 13957 RVA: 0x000B9405 File Offset: 0x000B8405
			public override int Read(char[] buffer, int index, int count)
			{
				return 0;
			}

			// Token: 0x06003686 RID: 13958 RVA: 0x000B9408 File Offset: 0x000B8408
			public override string ReadLine()
			{
				return null;
			}
		}

		// Token: 0x020005B1 RID: 1457
		[Serializable]
		internal sealed class SyncTextReader : TextReader
		{
			// Token: 0x06003688 RID: 13960 RVA: 0x000B9413 File Offset: 0x000B8413
			internal SyncTextReader(TextReader t)
			{
				this._in = t;
			}

			// Token: 0x06003689 RID: 13961 RVA: 0x000B9422 File Offset: 0x000B8422
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Close()
			{
				this._in.Close();
			}

			// Token: 0x0600368A RID: 13962 RVA: 0x000B942F File Offset: 0x000B842F
			[MethodImpl(MethodImplOptions.Synchronized)]
			protected override void Dispose(bool disposing)
			{
				if (disposing)
				{
					((IDisposable)this._in).Dispose();
				}
			}

			// Token: 0x0600368B RID: 13963 RVA: 0x000B943F File Offset: 0x000B843F
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override int Peek()
			{
				return this._in.Peek();
			}

			// Token: 0x0600368C RID: 13964 RVA: 0x000B944C File Offset: 0x000B844C
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override int Read()
			{
				return this._in.Read();
			}

			// Token: 0x0600368D RID: 13965 RVA: 0x000B9459 File Offset: 0x000B8459
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override int Read([In] [Out] char[] buffer, int index, int count)
			{
				return this._in.Read(buffer, index, count);
			}

			// Token: 0x0600368E RID: 13966 RVA: 0x000B9469 File Offset: 0x000B8469
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override int ReadBlock([In] [Out] char[] buffer, int index, int count)
			{
				return this._in.ReadBlock(buffer, index, count);
			}

			// Token: 0x0600368F RID: 13967 RVA: 0x000B9479 File Offset: 0x000B8479
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override string ReadLine()
			{
				return this._in.ReadLine();
			}

			// Token: 0x06003690 RID: 13968 RVA: 0x000B9486 File Offset: 0x000B8486
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override string ReadToEnd()
			{
				return this._in.ReadToEnd();
			}

			// Token: 0x04001C5D RID: 7261
			internal TextReader _in;
		}
	}
}
