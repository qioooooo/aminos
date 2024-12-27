using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Security.Cryptography
{
	// Token: 0x02000880 RID: 2176
	[ComVisible(true)]
	public class Rfc2898DeriveBytes : DeriveBytes
	{
		// Token: 0x06004FA7 RID: 20391 RVA: 0x00115FB6 File Offset: 0x00114FB6
		public Rfc2898DeriveBytes(string password, int saltSize)
			: this(password, saltSize, 1000)
		{
		}

		// Token: 0x06004FA8 RID: 20392 RVA: 0x00115FC8 File Offset: 0x00114FC8
		public Rfc2898DeriveBytes(string password, int saltSize, int iterations)
		{
			if (saltSize < 0)
			{
				throw new ArgumentOutOfRangeException("saltSize", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			byte[] array = new byte[saltSize];
			Utils.StaticRandomNumberGenerator.GetBytes(array);
			this.Salt = array;
			this.IterationCount = iterations;
			this.m_hmacsha1 = new HMACSHA1(new UTF8Encoding(false).GetBytes(password));
			this.Initialize();
		}

		// Token: 0x06004FA9 RID: 20393 RVA: 0x00116031 File Offset: 0x00115031
		public Rfc2898DeriveBytes(string password, byte[] salt)
			: this(password, salt, 1000)
		{
		}

		// Token: 0x06004FAA RID: 20394 RVA: 0x00116040 File Offset: 0x00115040
		public Rfc2898DeriveBytes(string password, byte[] salt, int iterations)
			: this(new UTF8Encoding(false).GetBytes(password), salt, iterations)
		{
		}

		// Token: 0x06004FAB RID: 20395 RVA: 0x00116056 File Offset: 0x00115056
		public Rfc2898DeriveBytes(byte[] password, byte[] salt, int iterations)
		{
			this.Salt = salt;
			this.IterationCount = iterations;
			this.m_hmacsha1 = new HMACSHA1(password);
			this.Initialize();
		}

		// Token: 0x17000DFA RID: 3578
		// (get) Token: 0x06004FAC RID: 20396 RVA: 0x0011607E File Offset: 0x0011507E
		// (set) Token: 0x06004FAD RID: 20397 RVA: 0x00116086 File Offset: 0x00115086
		public int IterationCount
		{
			get
			{
				return (int)this.m_iterations;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				this.m_iterations = (uint)value;
				this.Initialize();
			}
		}

		// Token: 0x17000DFB RID: 3579
		// (get) Token: 0x06004FAE RID: 20398 RVA: 0x001160AE File Offset: 0x001150AE
		// (set) Token: 0x06004FAF RID: 20399 RVA: 0x001160C0 File Offset: 0x001150C0
		public byte[] Salt
		{
			get
			{
				return (byte[])this.m_salt.Clone();
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value.Length < 8)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Cryptography_PasswordDerivedBytes_FewBytesSalt"), new object[0]));
				}
				this.m_salt = (byte[])value.Clone();
				this.Initialize();
			}
		}

		// Token: 0x06004FB0 RID: 20400 RVA: 0x00116118 File Offset: 0x00115118
		public override byte[] GetBytes(int cb)
		{
			if (cb <= 0)
			{
				throw new ArgumentOutOfRangeException("cb", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			byte[] array = new byte[cb];
			int i = 0;
			int num = this.m_endIndex - this.m_startIndex;
			if (num > 0)
			{
				if (cb < num)
				{
					Buffer.InternalBlockCopy(this.m_buffer, this.m_startIndex, array, 0, cb);
					this.m_startIndex += cb;
					return array;
				}
				Buffer.InternalBlockCopy(this.m_buffer, this.m_startIndex, array, 0, num);
				this.m_startIndex = (this.m_endIndex = 0);
				i += num;
			}
			while (i < cb)
			{
				byte[] array2 = this.Func();
				int num2 = cb - i;
				if (num2 <= 20)
				{
					Buffer.InternalBlockCopy(array2, 0, array, i, num2);
					i += num2;
					Buffer.InternalBlockCopy(array2, num2, this.m_buffer, this.m_startIndex, 20 - num2);
					this.m_endIndex += 20 - num2;
					return array;
				}
				Buffer.InternalBlockCopy(array2, 0, array, i, 20);
				i += 20;
			}
			return array;
		}

		// Token: 0x06004FB1 RID: 20401 RVA: 0x00116219 File Offset: 0x00115219
		public override void Reset()
		{
			this.Initialize();
		}

		// Token: 0x06004FB2 RID: 20402 RVA: 0x00116224 File Offset: 0x00115224
		private void Initialize()
		{
			if (this.m_buffer != null)
			{
				Array.Clear(this.m_buffer, 0, this.m_buffer.Length);
			}
			this.m_buffer = new byte[20];
			this.m_block = 1U;
			this.m_startIndex = (this.m_endIndex = 0);
		}

		// Token: 0x06004FB3 RID: 20403 RVA: 0x00116274 File Offset: 0x00115274
		private byte[] Func()
		{
			byte[] array = Utils.Int(this.m_block);
			this.m_hmacsha1.TransformBlock(this.m_salt, 0, this.m_salt.Length, this.m_salt, 0);
			this.m_hmacsha1.TransformFinalBlock(array, 0, array.Length);
			byte[] array2 = this.m_hmacsha1.Hash;
			this.m_hmacsha1.Initialize();
			byte[] array3 = array2;
			int num = 2;
			while ((long)num <= (long)((ulong)this.m_iterations))
			{
				array2 = this.m_hmacsha1.ComputeHash(array2);
				for (int i = 0; i < 20; i++)
				{
					byte[] array4 = array3;
					int num2 = i;
					array4[num2] ^= array2[i];
				}
				num++;
			}
			this.m_block += 1U;
			return array3;
		}

		// Token: 0x040028D9 RID: 10457
		private const int BlockSize = 20;

		// Token: 0x040028DA RID: 10458
		private byte[] m_buffer;

		// Token: 0x040028DB RID: 10459
		private byte[] m_salt;

		// Token: 0x040028DC RID: 10460
		private HMACSHA1 m_hmacsha1;

		// Token: 0x040028DD RID: 10461
		private uint m_iterations;

		// Token: 0x040028DE RID: 10462
		private uint m_block;

		// Token: 0x040028DF RID: 10463
		private int m_startIndex;

		// Token: 0x040028E0 RID: 10464
		private int m_endIndex;
	}
}
