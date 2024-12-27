using System;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200086C RID: 2156
	[ComVisible(true)]
	public abstract class HashAlgorithm : ICryptoTransform, IDisposable
	{
		// Token: 0x17000DCF RID: 3535
		// (get) Token: 0x06004F0E RID: 20238 RVA: 0x00113FC7 File Offset: 0x00112FC7
		public virtual int HashSize
		{
			get
			{
				return this.HashSizeValue;
			}
		}

		// Token: 0x17000DD0 RID: 3536
		// (get) Token: 0x06004F0F RID: 20239 RVA: 0x00113FD0 File Offset: 0x00112FD0
		public virtual byte[] Hash
		{
			get
			{
				if (this.m_bDisposed)
				{
					throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_Generic"));
				}
				if (this.State != 0)
				{
					throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("Cryptography_HashNotYetFinalized"));
				}
				return (byte[])this.HashValue.Clone();
			}
		}

		// Token: 0x06004F10 RID: 20240 RVA: 0x0011401E File Offset: 0x0011301E
		public static HashAlgorithm Create()
		{
			return HashAlgorithm.Create("System.Security.Cryptography.HashAlgorithm");
		}

		// Token: 0x06004F11 RID: 20241 RVA: 0x0011402A File Offset: 0x0011302A
		public static HashAlgorithm Create(string hashName)
		{
			return (HashAlgorithm)CryptoConfig.CreateFromName(hashName);
		}

		// Token: 0x06004F12 RID: 20242 RVA: 0x00114038 File Offset: 0x00113038
		public byte[] ComputeHash(Stream inputStream)
		{
			if (this.m_bDisposed)
			{
				throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_Generic"));
			}
			byte[] array = new byte[4096];
			int num;
			do
			{
				num = inputStream.Read(array, 0, 4096);
				if (num > 0)
				{
					this.HashCore(array, 0, num);
				}
			}
			while (num > 0);
			this.HashValue = this.HashFinal();
			byte[] array2 = (byte[])this.HashValue.Clone();
			this.Initialize();
			return array2;
		}

		// Token: 0x06004F13 RID: 20243 RVA: 0x001140AC File Offset: 0x001130AC
		public byte[] ComputeHash(byte[] buffer)
		{
			if (this.m_bDisposed)
			{
				throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_Generic"));
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			this.HashCore(buffer, 0, buffer.Length);
			this.HashValue = this.HashFinal();
			byte[] array = (byte[])this.HashValue.Clone();
			this.Initialize();
			return array;
		}

		// Token: 0x06004F14 RID: 20244 RVA: 0x00114110 File Offset: 0x00113110
		public byte[] ComputeHash(byte[] buffer, int offset, int count)
		{
			if (this.m_bDisposed)
			{
				throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_Generic"));
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (count < 0 || count > buffer.Length)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidValue"));
			}
			if (buffer.Length - count < offset)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			this.HashCore(buffer, offset, count);
			this.HashValue = this.HashFinal();
			byte[] array = (byte[])this.HashValue.Clone();
			this.Initialize();
			return array;
		}

		// Token: 0x17000DD1 RID: 3537
		// (get) Token: 0x06004F15 RID: 20245 RVA: 0x001141BC File Offset: 0x001131BC
		public virtual int InputBlockSize
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000DD2 RID: 3538
		// (get) Token: 0x06004F16 RID: 20246 RVA: 0x001141BF File Offset: 0x001131BF
		public virtual int OutputBlockSize
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000DD3 RID: 3539
		// (get) Token: 0x06004F17 RID: 20247 RVA: 0x001141C2 File Offset: 0x001131C2
		public virtual bool CanTransformMultipleBlocks
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000DD4 RID: 3540
		// (get) Token: 0x06004F18 RID: 20248 RVA: 0x001141C5 File Offset: 0x001131C5
		public virtual bool CanReuseTransform
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004F19 RID: 20249 RVA: 0x001141C8 File Offset: 0x001131C8
		public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			if (this.m_bDisposed)
			{
				throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_Generic"));
			}
			if (inputBuffer == null)
			{
				throw new ArgumentNullException("inputBuffer");
			}
			if (inputOffset < 0)
			{
				throw new ArgumentOutOfRangeException("inputOffset", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (inputCount < 0 || inputCount > inputBuffer.Length)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidValue"));
			}
			if (inputBuffer.Length - inputCount < inputOffset)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			this.State = 1;
			this.HashCore(inputBuffer, inputOffset, inputCount);
			if (outputBuffer != null && (inputBuffer != outputBuffer || inputOffset != outputOffset))
			{
				Buffer.BlockCopy(inputBuffer, inputOffset, outputBuffer, outputOffset, inputCount);
			}
			return inputCount;
		}

		// Token: 0x06004F1A RID: 20250 RVA: 0x00114274 File Offset: 0x00113274
		public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			if (this.m_bDisposed)
			{
				throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_Generic"));
			}
			if (inputBuffer == null)
			{
				throw new ArgumentNullException("inputBuffer");
			}
			if (inputOffset < 0)
			{
				throw new ArgumentOutOfRangeException("inputOffset", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (inputCount < 0 || inputCount > inputBuffer.Length)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidValue"));
			}
			if (inputBuffer.Length - inputCount < inputOffset)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			this.HashCore(inputBuffer, inputOffset, inputCount);
			this.HashValue = this.HashFinal();
			byte[] array = new byte[inputCount];
			if (inputCount != 0)
			{
				Buffer.InternalBlockCopy(inputBuffer, inputOffset, array, 0, inputCount);
			}
			this.State = 0;
			return array;
		}

		// Token: 0x06004F1B RID: 20251 RVA: 0x00114324 File Offset: 0x00113324
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06004F1C RID: 20252 RVA: 0x00114333 File Offset: 0x00113333
		public void Clear()
		{
			((IDisposable)this).Dispose();
		}

		// Token: 0x06004F1D RID: 20253 RVA: 0x0011433B File Offset: 0x0011333B
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.HashValue != null)
				{
					Array.Clear(this.HashValue, 0, this.HashValue.Length);
				}
				this.HashValue = null;
				this.m_bDisposed = true;
			}
		}

		// Token: 0x06004F1E RID: 20254
		public abstract void Initialize();

		// Token: 0x06004F1F RID: 20255
		protected abstract void HashCore(byte[] array, int ibStart, int cbSize);

		// Token: 0x06004F20 RID: 20256
		protected abstract byte[] HashFinal();

		// Token: 0x040028AA RID: 10410
		protected int HashSizeValue;

		// Token: 0x040028AB RID: 10411
		protected internal byte[] HashValue;

		// Token: 0x040028AC RID: 10412
		protected int State;

		// Token: 0x040028AD RID: 10413
		private bool m_bDisposed;
	}
}
