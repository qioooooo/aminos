using System;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Security.Cryptography
{
	// Token: 0x02000858 RID: 2136
	[ComVisible(true)]
	public class ToBase64Transform : ICryptoTransform, IDisposable
	{
		// Token: 0x17000DA4 RID: 3492
		// (get) Token: 0x06004E55 RID: 20053 RVA: 0x0010FE12 File Offset: 0x0010EE12
		public int InputBlockSize
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17000DA5 RID: 3493
		// (get) Token: 0x06004E56 RID: 20054 RVA: 0x0010FE15 File Offset: 0x0010EE15
		public int OutputBlockSize
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x17000DA6 RID: 3494
		// (get) Token: 0x06004E57 RID: 20055 RVA: 0x0010FE18 File Offset: 0x0010EE18
		public bool CanTransformMultipleBlocks
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000DA7 RID: 3495
		// (get) Token: 0x06004E58 RID: 20056 RVA: 0x0010FE1B File Offset: 0x0010EE1B
		public virtual bool CanReuseTransform
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004E59 RID: 20057 RVA: 0x0010FE20 File Offset: 0x0010EE20
		public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			if (this.asciiEncoding == null)
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
			char[] array = new char[4];
			Convert.ToBase64CharArray(inputBuffer, inputOffset, 3, array, 0);
			byte[] bytes = this.asciiEncoding.GetBytes(array);
			if (bytes.Length != 4)
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptography_SSE_InvalidDataSize"));
			}
			Buffer.BlockCopy(bytes, 0, outputBuffer, outputOffset, bytes.Length);
			return bytes.Length;
		}

		// Token: 0x06004E5A RID: 20058 RVA: 0x0010FEE8 File Offset: 0x0010EEE8
		public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			if (this.asciiEncoding == null)
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
			if (inputCount == 0)
			{
				return new byte[0];
			}
			char[] array = new char[4];
			Convert.ToBase64CharArray(inputBuffer, inputOffset, inputCount, array, 0);
			return this.asciiEncoding.GetBytes(array);
		}

		// Token: 0x06004E5B RID: 20059 RVA: 0x0010FF91 File Offset: 0x0010EF91
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06004E5C RID: 20060 RVA: 0x0010FFA0 File Offset: 0x0010EFA0
		public void Clear()
		{
			((IDisposable)this).Dispose();
		}

		// Token: 0x06004E5D RID: 20061 RVA: 0x0010FFA8 File Offset: 0x0010EFA8
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.asciiEncoding = null;
			}
		}

		// Token: 0x06004E5E RID: 20062 RVA: 0x0010FFB4 File Offset: 0x0010EFB4
		~ToBase64Transform()
		{
			this.Dispose(false);
		}

		// Token: 0x0400284B RID: 10315
		private ASCIIEncoding asciiEncoding = new ASCIIEncoding();
	}
}
