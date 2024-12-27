using System;

namespace System
{
	// Token: 0x02000093 RID: 147
	[Serializable]
	public struct ConsoleKeyInfo
	{
		// Token: 0x060007FC RID: 2044 RVA: 0x0001A288 File Offset: 0x00019288
		public ConsoleKeyInfo(char keyChar, ConsoleKey key, bool shift, bool alt, bool control)
		{
			if (key < (ConsoleKey)0 || key > (ConsoleKey)255)
			{
				throw new ArgumentOutOfRangeException("key", Environment.GetResourceString("ArgumentOutOfRange_ConsoleKey"));
			}
			this._keyChar = keyChar;
			this._key = key;
			this._mods = (ConsoleModifiers)0;
			if (shift)
			{
				this._mods |= ConsoleModifiers.Shift;
			}
			if (alt)
			{
				this._mods |= ConsoleModifiers.Alt;
			}
			if (control)
			{
				this._mods |= ConsoleModifiers.Control;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060007FD RID: 2045 RVA: 0x0001A300 File Offset: 0x00019300
		public char KeyChar
		{
			get
			{
				return this._keyChar;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060007FE RID: 2046 RVA: 0x0001A308 File Offset: 0x00019308
		public ConsoleKey Key
		{
			get
			{
				return this._key;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x0001A310 File Offset: 0x00019310
		public ConsoleModifiers Modifiers
		{
			get
			{
				return this._mods;
			}
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x0001A318 File Offset: 0x00019318
		public override bool Equals(object value)
		{
			return value is ConsoleKeyInfo && this.Equals((ConsoleKeyInfo)value);
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x0001A330 File Offset: 0x00019330
		public bool Equals(ConsoleKeyInfo obj)
		{
			return obj._keyChar == this._keyChar && obj._key == this._key && obj._mods == this._mods;
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0001A361 File Offset: 0x00019361
		public static bool operator ==(ConsoleKeyInfo a, ConsoleKeyInfo b)
		{
			return a.Equals(b);
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0001A36B File Offset: 0x0001936B
		public static bool operator !=(ConsoleKeyInfo a, ConsoleKeyInfo b)
		{
			return !(a == b);
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x0001A377 File Offset: 0x00019377
		public override int GetHashCode()
		{
			return (int)((ConsoleModifiers)this._keyChar | this._mods);
		}

		// Token: 0x04000359 RID: 857
		private char _keyChar;

		// Token: 0x0400035A RID: 858
		private ConsoleKey _key;

		// Token: 0x0400035B RID: 859
		private ConsoleModifiers _mods;
	}
}
