using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace System.IO
{
	// Token: 0x020005B4 RID: 1460
	[ComVisible(true)]
	[Serializable]
	public abstract class TextWriter : MarshalByRefObject, IDisposable
	{
		// Token: 0x060036B9 RID: 14009 RVA: 0x000BA0F0 File Offset: 0x000B90F0
		protected TextWriter()
		{
			this.InternalFormatProvider = null;
		}

		// Token: 0x060036BA RID: 14010 RVA: 0x000BA124 File Offset: 0x000B9124
		protected TextWriter(IFormatProvider formatProvider)
		{
			this.InternalFormatProvider = formatProvider;
		}

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x060036BB RID: 14011 RVA: 0x000BA156 File Offset: 0x000B9156
		public virtual IFormatProvider FormatProvider
		{
			get
			{
				if (this.InternalFormatProvider == null)
				{
					return Thread.CurrentThread.CurrentCulture;
				}
				return this.InternalFormatProvider;
			}
		}

		// Token: 0x060036BC RID: 14012 RVA: 0x000BA171 File Offset: 0x000B9171
		public virtual void Close()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060036BD RID: 14013 RVA: 0x000BA180 File Offset: 0x000B9180
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x060036BE RID: 14014 RVA: 0x000BA182 File Offset: 0x000B9182
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060036BF RID: 14015 RVA: 0x000BA191 File Offset: 0x000B9191
		public virtual void Flush()
		{
		}

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x060036C0 RID: 14016
		public abstract Encoding Encoding { get; }

		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x060036C1 RID: 14017 RVA: 0x000BA193 File Offset: 0x000B9193
		// (set) Token: 0x060036C2 RID: 14018 RVA: 0x000BA1A0 File Offset: 0x000B91A0
		public virtual string NewLine
		{
			get
			{
				return new string(this.CoreNewLine);
			}
			set
			{
				if (value == null)
				{
					value = "\r\n";
				}
				this.CoreNewLine = value.ToCharArray();
			}
		}

		// Token: 0x060036C3 RID: 14019 RVA: 0x000BA1B8 File Offset: 0x000B91B8
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		public static TextWriter Synchronized(TextWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			if (writer is TextWriter.SyncTextWriter)
			{
				return writer;
			}
			return new TextWriter.SyncTextWriter(writer);
		}

		// Token: 0x060036C4 RID: 14020 RVA: 0x000BA1D8 File Offset: 0x000B91D8
		public virtual void Write(char value)
		{
		}

		// Token: 0x060036C5 RID: 14021 RVA: 0x000BA1DA File Offset: 0x000B91DA
		public virtual void Write(char[] buffer)
		{
			if (buffer != null)
			{
				this.Write(buffer, 0, buffer.Length);
			}
		}

		// Token: 0x060036C6 RID: 14022 RVA: 0x000BA1EC File Offset: 0x000B91EC
		public virtual void Write(char[] buffer, int index, int count)
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
			for (int i = 0; i < count; i++)
			{
				this.Write(buffer[index + i]);
			}
		}

		// Token: 0x060036C7 RID: 14023 RVA: 0x000BA272 File Offset: 0x000B9272
		public virtual void Write(bool value)
		{
			this.Write(value ? "True" : "False");
		}

		// Token: 0x060036C8 RID: 14024 RVA: 0x000BA289 File Offset: 0x000B9289
		public virtual void Write(int value)
		{
			this.Write(value.ToString(this.FormatProvider));
		}

		// Token: 0x060036C9 RID: 14025 RVA: 0x000BA29E File Offset: 0x000B929E
		[CLSCompliant(false)]
		public virtual void Write(uint value)
		{
			this.Write(value.ToString(this.FormatProvider));
		}

		// Token: 0x060036CA RID: 14026 RVA: 0x000BA2B3 File Offset: 0x000B92B3
		public virtual void Write(long value)
		{
			this.Write(value.ToString(this.FormatProvider));
		}

		// Token: 0x060036CB RID: 14027 RVA: 0x000BA2C8 File Offset: 0x000B92C8
		[CLSCompliant(false)]
		public virtual void Write(ulong value)
		{
			this.Write(value.ToString(this.FormatProvider));
		}

		// Token: 0x060036CC RID: 14028 RVA: 0x000BA2DD File Offset: 0x000B92DD
		public virtual void Write(float value)
		{
			this.Write(value.ToString(this.FormatProvider));
		}

		// Token: 0x060036CD RID: 14029 RVA: 0x000BA2F2 File Offset: 0x000B92F2
		public virtual void Write(double value)
		{
			this.Write(value.ToString(this.FormatProvider));
		}

		// Token: 0x060036CE RID: 14030 RVA: 0x000BA307 File Offset: 0x000B9307
		public virtual void Write(decimal value)
		{
			this.Write(value.ToString(this.FormatProvider));
		}

		// Token: 0x060036CF RID: 14031 RVA: 0x000BA31C File Offset: 0x000B931C
		public virtual void Write(string value)
		{
			if (value != null)
			{
				this.Write(value.ToCharArray());
			}
		}

		// Token: 0x060036D0 RID: 14032 RVA: 0x000BA330 File Offset: 0x000B9330
		public virtual void Write(object value)
		{
			if (value != null)
			{
				IFormattable formattable = value as IFormattable;
				if (formattable != null)
				{
					this.Write(formattable.ToString(null, this.FormatProvider));
					return;
				}
				this.Write(value.ToString());
			}
		}

		// Token: 0x060036D1 RID: 14033 RVA: 0x000BA36C File Offset: 0x000B936C
		public virtual void Write(string format, object arg0)
		{
			this.Write(string.Format(this.FormatProvider, format, new object[] { arg0 }));
		}

		// Token: 0x060036D2 RID: 14034 RVA: 0x000BA398 File Offset: 0x000B9398
		public virtual void Write(string format, object arg0, object arg1)
		{
			this.Write(string.Format(this.FormatProvider, format, new object[] { arg0, arg1 }));
		}

		// Token: 0x060036D3 RID: 14035 RVA: 0x000BA3C8 File Offset: 0x000B93C8
		public virtual void Write(string format, object arg0, object arg1, object arg2)
		{
			this.Write(string.Format(this.FormatProvider, format, new object[] { arg0, arg1, arg2 }));
		}

		// Token: 0x060036D4 RID: 14036 RVA: 0x000BA3FC File Offset: 0x000B93FC
		public virtual void Write(string format, params object[] arg)
		{
			this.Write(string.Format(this.FormatProvider, format, arg));
		}

		// Token: 0x060036D5 RID: 14037 RVA: 0x000BA411 File Offset: 0x000B9411
		public virtual void WriteLine()
		{
			this.Write(this.CoreNewLine);
		}

		// Token: 0x060036D6 RID: 14038 RVA: 0x000BA41F File Offset: 0x000B941F
		public virtual void WriteLine(char value)
		{
			this.Write(value);
			this.WriteLine();
		}

		// Token: 0x060036D7 RID: 14039 RVA: 0x000BA42E File Offset: 0x000B942E
		public virtual void WriteLine(char[] buffer)
		{
			this.Write(buffer);
			this.WriteLine();
		}

		// Token: 0x060036D8 RID: 14040 RVA: 0x000BA43D File Offset: 0x000B943D
		public virtual void WriteLine(char[] buffer, int index, int count)
		{
			this.Write(buffer, index, count);
			this.WriteLine();
		}

		// Token: 0x060036D9 RID: 14041 RVA: 0x000BA44E File Offset: 0x000B944E
		public virtual void WriteLine(bool value)
		{
			this.Write(value);
			this.WriteLine();
		}

		// Token: 0x060036DA RID: 14042 RVA: 0x000BA45D File Offset: 0x000B945D
		public virtual void WriteLine(int value)
		{
			this.Write(value);
			this.WriteLine();
		}

		// Token: 0x060036DB RID: 14043 RVA: 0x000BA46C File Offset: 0x000B946C
		[CLSCompliant(false)]
		public virtual void WriteLine(uint value)
		{
			this.Write(value);
			this.WriteLine();
		}

		// Token: 0x060036DC RID: 14044 RVA: 0x000BA47B File Offset: 0x000B947B
		public virtual void WriteLine(long value)
		{
			this.Write(value);
			this.WriteLine();
		}

		// Token: 0x060036DD RID: 14045 RVA: 0x000BA48A File Offset: 0x000B948A
		[CLSCompliant(false)]
		public virtual void WriteLine(ulong value)
		{
			this.Write(value);
			this.WriteLine();
		}

		// Token: 0x060036DE RID: 14046 RVA: 0x000BA499 File Offset: 0x000B9499
		public virtual void WriteLine(float value)
		{
			this.Write(value);
			this.WriteLine();
		}

		// Token: 0x060036DF RID: 14047 RVA: 0x000BA4A8 File Offset: 0x000B94A8
		public virtual void WriteLine(double value)
		{
			this.Write(value);
			this.WriteLine();
		}

		// Token: 0x060036E0 RID: 14048 RVA: 0x000BA4B7 File Offset: 0x000B94B7
		public virtual void WriteLine(decimal value)
		{
			this.Write(value);
			this.WriteLine();
		}

		// Token: 0x060036E1 RID: 14049 RVA: 0x000BA4C8 File Offset: 0x000B94C8
		public virtual void WriteLine(string value)
		{
			if (value == null)
			{
				this.WriteLine();
				return;
			}
			int length = value.Length;
			int num = this.CoreNewLine.Length;
			char[] array = new char[length + num];
			value.CopyTo(0, array, 0, length);
			if (num == 2)
			{
				array[length] = this.CoreNewLine[0];
				array[length + 1] = this.CoreNewLine[1];
			}
			else if (num == 1)
			{
				array[length] = this.CoreNewLine[0];
			}
			else
			{
				Buffer.InternalBlockCopy(this.CoreNewLine, 0, array, length * 2, num * 2);
			}
			this.Write(array, 0, length + num);
		}

		// Token: 0x060036E2 RID: 14050 RVA: 0x000BA550 File Offset: 0x000B9550
		public virtual void WriteLine(object value)
		{
			if (value == null)
			{
				this.WriteLine();
				return;
			}
			IFormattable formattable = value as IFormattable;
			if (formattable != null)
			{
				this.WriteLine(formattable.ToString(null, this.FormatProvider));
				return;
			}
			this.WriteLine(value.ToString());
		}

		// Token: 0x060036E3 RID: 14051 RVA: 0x000BA594 File Offset: 0x000B9594
		public virtual void WriteLine(string format, object arg0)
		{
			this.WriteLine(string.Format(this.FormatProvider, format, new object[] { arg0 }));
		}

		// Token: 0x060036E4 RID: 14052 RVA: 0x000BA5C0 File Offset: 0x000B95C0
		public virtual void WriteLine(string format, object arg0, object arg1)
		{
			this.WriteLine(string.Format(this.FormatProvider, format, new object[] { arg0, arg1 }));
		}

		// Token: 0x060036E5 RID: 14053 RVA: 0x000BA5F0 File Offset: 0x000B95F0
		public virtual void WriteLine(string format, object arg0, object arg1, object arg2)
		{
			this.WriteLine(string.Format(this.FormatProvider, format, new object[] { arg0, arg1, arg2 }));
		}

		// Token: 0x060036E6 RID: 14054 RVA: 0x000BA624 File Offset: 0x000B9624
		public virtual void WriteLine(string format, params object[] arg)
		{
			this.WriteLine(string.Format(this.FormatProvider, format, arg));
		}

		// Token: 0x04001C71 RID: 7281
		private const string InitialNewLine = "\r\n";

		// Token: 0x04001C72 RID: 7282
		public static readonly TextWriter Null = new TextWriter.NullTextWriter();

		// Token: 0x04001C73 RID: 7283
		protected char[] CoreNewLine = new char[] { '\r', '\n' };

		// Token: 0x04001C74 RID: 7284
		private IFormatProvider InternalFormatProvider;

		// Token: 0x020005B5 RID: 1461
		[Serializable]
		private sealed class NullTextWriter : TextWriter
		{
			// Token: 0x060036E8 RID: 14056 RVA: 0x000BA645 File Offset: 0x000B9645
			internal NullTextWriter()
				: base(CultureInfo.InvariantCulture)
			{
			}

			// Token: 0x17000951 RID: 2385
			// (get) Token: 0x060036E9 RID: 14057 RVA: 0x000BA652 File Offset: 0x000B9652
			public override Encoding Encoding
			{
				get
				{
					return Encoding.Default;
				}
			}

			// Token: 0x060036EA RID: 14058 RVA: 0x000BA659 File Offset: 0x000B9659
			public override void Write(char[] buffer, int index, int count)
			{
			}

			// Token: 0x060036EB RID: 14059 RVA: 0x000BA65B File Offset: 0x000B965B
			public override void Write(string value)
			{
			}

			// Token: 0x060036EC RID: 14060 RVA: 0x000BA65D File Offset: 0x000B965D
			public override void WriteLine()
			{
			}

			// Token: 0x060036ED RID: 14061 RVA: 0x000BA65F File Offset: 0x000B965F
			public override void WriteLine(string value)
			{
			}

			// Token: 0x060036EE RID: 14062 RVA: 0x000BA661 File Offset: 0x000B9661
			public override void WriteLine(object value)
			{
			}
		}

		// Token: 0x020005B6 RID: 1462
		[Serializable]
		internal sealed class SyncTextWriter : TextWriter, IDisposable
		{
			// Token: 0x060036EF RID: 14063 RVA: 0x000BA663 File Offset: 0x000B9663
			internal SyncTextWriter(TextWriter t)
				: base(t.FormatProvider)
			{
				this._out = t;
			}

			// Token: 0x17000952 RID: 2386
			// (get) Token: 0x060036F0 RID: 14064 RVA: 0x000BA678 File Offset: 0x000B9678
			public override Encoding Encoding
			{
				get
				{
					return this._out.Encoding;
				}
			}

			// Token: 0x17000953 RID: 2387
			// (get) Token: 0x060036F1 RID: 14065 RVA: 0x000BA685 File Offset: 0x000B9685
			public override IFormatProvider FormatProvider
			{
				get
				{
					return this._out.FormatProvider;
				}
			}

			// Token: 0x17000954 RID: 2388
			// (get) Token: 0x060036F2 RID: 14066 RVA: 0x000BA692 File Offset: 0x000B9692
			// (set) Token: 0x060036F3 RID: 14067 RVA: 0x000BA69F File Offset: 0x000B969F
			public override string NewLine
			{
				[MethodImpl(MethodImplOptions.Synchronized)]
				get
				{
					return this._out.NewLine;
				}
				[MethodImpl(MethodImplOptions.Synchronized)]
				set
				{
					this._out.NewLine = value;
				}
			}

			// Token: 0x060036F4 RID: 14068 RVA: 0x000BA6AD File Offset: 0x000B96AD
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Close()
			{
				this._out.Close();
			}

			// Token: 0x060036F5 RID: 14069 RVA: 0x000BA6BA File Offset: 0x000B96BA
			[MethodImpl(MethodImplOptions.Synchronized)]
			protected override void Dispose(bool disposing)
			{
				if (disposing)
				{
					((IDisposable)this._out).Dispose();
				}
			}

			// Token: 0x060036F6 RID: 14070 RVA: 0x000BA6CA File Offset: 0x000B96CA
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Flush()
			{
				this._out.Flush();
			}

			// Token: 0x060036F7 RID: 14071 RVA: 0x000BA6D7 File Offset: 0x000B96D7
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(char value)
			{
				this._out.Write(value);
			}

			// Token: 0x060036F8 RID: 14072 RVA: 0x000BA6E5 File Offset: 0x000B96E5
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(char[] buffer)
			{
				this._out.Write(buffer);
			}

			// Token: 0x060036F9 RID: 14073 RVA: 0x000BA6F3 File Offset: 0x000B96F3
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(char[] buffer, int index, int count)
			{
				this._out.Write(buffer, index, count);
			}

			// Token: 0x060036FA RID: 14074 RVA: 0x000BA703 File Offset: 0x000B9703
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(bool value)
			{
				this._out.Write(value);
			}

			// Token: 0x060036FB RID: 14075 RVA: 0x000BA711 File Offset: 0x000B9711
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(int value)
			{
				this._out.Write(value);
			}

			// Token: 0x060036FC RID: 14076 RVA: 0x000BA71F File Offset: 0x000B971F
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(uint value)
			{
				this._out.Write(value);
			}

			// Token: 0x060036FD RID: 14077 RVA: 0x000BA72D File Offset: 0x000B972D
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(long value)
			{
				this._out.Write(value);
			}

			// Token: 0x060036FE RID: 14078 RVA: 0x000BA73B File Offset: 0x000B973B
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(ulong value)
			{
				this._out.Write(value);
			}

			// Token: 0x060036FF RID: 14079 RVA: 0x000BA749 File Offset: 0x000B9749
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(float value)
			{
				this._out.Write(value);
			}

			// Token: 0x06003700 RID: 14080 RVA: 0x000BA757 File Offset: 0x000B9757
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(double value)
			{
				this._out.Write(value);
			}

			// Token: 0x06003701 RID: 14081 RVA: 0x000BA765 File Offset: 0x000B9765
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(decimal value)
			{
				this._out.Write(value);
			}

			// Token: 0x06003702 RID: 14082 RVA: 0x000BA773 File Offset: 0x000B9773
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(string value)
			{
				this._out.Write(value);
			}

			// Token: 0x06003703 RID: 14083 RVA: 0x000BA781 File Offset: 0x000B9781
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(object value)
			{
				this._out.Write(value);
			}

			// Token: 0x06003704 RID: 14084 RVA: 0x000BA78F File Offset: 0x000B978F
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(string format, object arg0)
			{
				this._out.Write(format, arg0);
			}

			// Token: 0x06003705 RID: 14085 RVA: 0x000BA79E File Offset: 0x000B979E
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(string format, object arg0, object arg1)
			{
				this._out.Write(format, arg0, arg1);
			}

			// Token: 0x06003706 RID: 14086 RVA: 0x000BA7AE File Offset: 0x000B97AE
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(string format, object arg0, object arg1, object arg2)
			{
				this._out.Write(format, arg0, arg1, arg2);
			}

			// Token: 0x06003707 RID: 14087 RVA: 0x000BA7C0 File Offset: 0x000B97C0
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(string format, object[] arg)
			{
				this._out.Write(format, arg);
			}

			// Token: 0x06003708 RID: 14088 RVA: 0x000BA7CF File Offset: 0x000B97CF
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine()
			{
				this._out.WriteLine();
			}

			// Token: 0x06003709 RID: 14089 RVA: 0x000BA7DC File Offset: 0x000B97DC
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(char value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x0600370A RID: 14090 RVA: 0x000BA7EA File Offset: 0x000B97EA
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(decimal value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x0600370B RID: 14091 RVA: 0x000BA7F8 File Offset: 0x000B97F8
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(char[] buffer)
			{
				this._out.WriteLine(buffer);
			}

			// Token: 0x0600370C RID: 14092 RVA: 0x000BA806 File Offset: 0x000B9806
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(char[] buffer, int index, int count)
			{
				this._out.WriteLine(buffer, index, count);
			}

			// Token: 0x0600370D RID: 14093 RVA: 0x000BA816 File Offset: 0x000B9816
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(bool value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x0600370E RID: 14094 RVA: 0x000BA824 File Offset: 0x000B9824
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(int value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x0600370F RID: 14095 RVA: 0x000BA832 File Offset: 0x000B9832
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(uint value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x06003710 RID: 14096 RVA: 0x000BA840 File Offset: 0x000B9840
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(long value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x06003711 RID: 14097 RVA: 0x000BA84E File Offset: 0x000B984E
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(ulong value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x06003712 RID: 14098 RVA: 0x000BA85C File Offset: 0x000B985C
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(float value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x06003713 RID: 14099 RVA: 0x000BA86A File Offset: 0x000B986A
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(double value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x06003714 RID: 14100 RVA: 0x000BA878 File Offset: 0x000B9878
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(string value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x06003715 RID: 14101 RVA: 0x000BA886 File Offset: 0x000B9886
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(object value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x06003716 RID: 14102 RVA: 0x000BA894 File Offset: 0x000B9894
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(string format, object arg0)
			{
				this._out.WriteLine(format, arg0);
			}

			// Token: 0x06003717 RID: 14103 RVA: 0x000BA8A3 File Offset: 0x000B98A3
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(string format, object arg0, object arg1)
			{
				this._out.WriteLine(format, arg0, arg1);
			}

			// Token: 0x06003718 RID: 14104 RVA: 0x000BA8B3 File Offset: 0x000B98B3
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(string format, object arg0, object arg1, object arg2)
			{
				this._out.WriteLine(format, arg0, arg1, arg2);
			}

			// Token: 0x06003719 RID: 14105 RVA: 0x000BA8C5 File Offset: 0x000B98C5
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(string format, object[] arg)
			{
				this._out.WriteLine(format, arg);
			}

			// Token: 0x04001C75 RID: 7285
			private TextWriter _out;
		}
	}
}
