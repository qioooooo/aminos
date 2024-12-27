using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Cryptography
{
	// Token: 0x0200085B RID: 2139
	[ComVisible(true)]
	public sealed class CryptoAPITransform : ICryptoTransform, IDisposable
	{
		// Token: 0x06004E6E RID: 20078 RVA: 0x001103D4 File Offset: 0x0010F3D4
		private CryptoAPITransform()
		{
		}

		// Token: 0x06004E6F RID: 20079 RVA: 0x001103DC File Offset: 0x0010F3DC
		internal CryptoAPITransform(int algid, int cArgs, int[] rgArgIds, object[] rgArgValues, byte[] rgbKey, PaddingMode padding, CipherMode cipherChainingMode, int blockSize, int feedbackSize, bool useSalt, CryptoAPITransformMode encDecMode)
		{
			this.BlockSizeValue = blockSize;
			this.ModeValue = cipherChainingMode;
			this.PaddingValue = padding;
			this.encryptOrDecrypt = encDecMode;
			int[] array = new int[rgArgIds.Length];
			Array.Copy(rgArgIds, array, rgArgIds.Length);
			this._rgbKey = new byte[rgbKey.Length];
			Array.Copy(rgbKey, this._rgbKey, rgbKey.Length);
			object[] array2 = new object[rgArgValues.Length];
			for (int i = 0; i < rgArgValues.Length; i++)
			{
				if (rgArgValues[i] is byte[])
				{
					byte[] array3 = (byte[])rgArgValues[i];
					byte[] array4 = new byte[array3.Length];
					Array.Copy(array3, array4, array3.Length);
					array2[i] = array4;
				}
				else if (rgArgValues[i] is int)
				{
					array2[i] = (int)rgArgValues[i];
				}
				else if (rgArgValues[i] is CipherMode)
				{
					array2[i] = (int)rgArgValues[i];
				}
			}
			this._safeProvHandle = Utils.AcquireProvHandle(new CspParameters(Utils.DefaultRsaProviderType));
			SafeKeyHandle invalidHandle = SafeKeyHandle.InvalidHandle;
			Utils._ImportBulkKey(this._safeProvHandle, algid, useSalt, this._rgbKey, ref invalidHandle);
			this._safeKeyHandle = invalidHandle;
			int j = 0;
			while (j < cArgs)
			{
				int num = rgArgIds[j];
				int num2;
				switch (num)
				{
				case 1:
				{
					this.IVValue = (byte[])array2[j];
					byte[] ivvalue = this.IVValue;
					Utils._SetKeyParamRgb(this._safeKeyHandle, array[j], ivvalue);
					break;
				}
				case 2:
				case 3:
					goto IL_01C9;
				case 4:
					this.ModeValue = (CipherMode)array2[j];
					num2 = (int)array2[j];
					goto IL_019F;
				case 5:
					num2 = (int)array2[j];
					goto IL_019F;
				default:
					if (num != 19)
					{
						goto IL_01C9;
					}
					num2 = (int)array2[j];
					goto IL_019F;
				}
				IL_01DE:
				j++;
				continue;
				IL_019F:
				Utils._SetKeyParamDw(this._safeKeyHandle, array[j], num2);
				goto IL_01DE;
				IL_01C9:
				throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidKeyParameter"), "_rgArgIds[i]");
			}
		}

		// Token: 0x06004E70 RID: 20080 RVA: 0x001105D5 File Offset: 0x0010F5D5
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06004E71 RID: 20081 RVA: 0x001105E4 File Offset: 0x0010F5E4
		public void Clear()
		{
			((IDisposable)this).Dispose();
		}

		// Token: 0x06004E72 RID: 20082 RVA: 0x001105EC File Offset: 0x0010F5EC
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this._rgbKey != null)
				{
					Array.Clear(this._rgbKey, 0, this._rgbKey.Length);
					this._rgbKey = null;
				}
				if (this.IVValue != null)
				{
					Array.Clear(this.IVValue, 0, this.IVValue.Length);
					this.IVValue = null;
				}
				if (this._depadBuffer != null)
				{
					Array.Clear(this._depadBuffer, 0, this._depadBuffer.Length);
					this._depadBuffer = null;
				}
			}
			if (this._safeKeyHandle != null && !this._safeKeyHandle.IsClosed)
			{
				this._safeKeyHandle.Dispose();
			}
			if (this._safeProvHandle != null && !this._safeProvHandle.IsClosed)
			{
				this._safeProvHandle.Dispose();
			}
		}

		// Token: 0x17000DAC RID: 3500
		// (get) Token: 0x06004E73 RID: 20083 RVA: 0x001106A5 File Offset: 0x0010F6A5
		public IntPtr KeyHandle
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				return this._safeKeyHandle.DangerousGetHandle();
			}
		}

		// Token: 0x17000DAD RID: 3501
		// (get) Token: 0x06004E74 RID: 20084 RVA: 0x001106B2 File Offset: 0x0010F6B2
		public int InputBlockSize
		{
			get
			{
				return this.BlockSizeValue / 8;
			}
		}

		// Token: 0x17000DAE RID: 3502
		// (get) Token: 0x06004E75 RID: 20085 RVA: 0x001106BC File Offset: 0x0010F6BC
		public int OutputBlockSize
		{
			get
			{
				return this.BlockSizeValue / 8;
			}
		}

		// Token: 0x17000DAF RID: 3503
		// (get) Token: 0x06004E76 RID: 20086 RVA: 0x001106C6 File Offset: 0x0010F6C6
		public bool CanTransformMultipleBlocks
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000DB0 RID: 3504
		// (get) Token: 0x06004E77 RID: 20087 RVA: 0x001106C9 File Offset: 0x0010F6C9
		public bool CanReuseTransform
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004E78 RID: 20088 RVA: 0x001106CC File Offset: 0x0010F6CC
		[ComVisible(false)]
		public void Reset()
		{
			this._depadBuffer = null;
			byte[] array = null;
			Utils._EncryptData(this._safeKeyHandle, new byte[0], 0, 0, ref array, 0, this.PaddingValue, true);
		}

		// Token: 0x06004E79 RID: 20089 RVA: 0x00110700 File Offset: 0x0010F700
		public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			if (inputBuffer == null)
			{
				throw new ArgumentNullException("inputBuffer");
			}
			if (outputBuffer == null)
			{
				throw new ArgumentNullException("outputBuffer");
			}
			if (inputOffset < 0)
			{
				throw new ArgumentOutOfRangeException("inputOffset", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (inputCount <= 0 || inputCount % this.InputBlockSize != 0 || inputCount > inputBuffer.Length)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidValue"));
			}
			if (inputBuffer.Length - inputCount < inputOffset)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			if (this.encryptOrDecrypt == CryptoAPITransformMode.Encrypt)
			{
				return Utils._EncryptData(this._safeKeyHandle, inputBuffer, inputOffset, inputCount, ref outputBuffer, outputOffset, this.PaddingValue, false);
			}
			if (this.PaddingValue == PaddingMode.Zeros || this.PaddingValue == PaddingMode.None)
			{
				return Utils._DecryptData(this._safeKeyHandle, inputBuffer, inputOffset, inputCount, ref outputBuffer, outputOffset, this.PaddingValue, false);
			}
			if (this._depadBuffer == null)
			{
				this._depadBuffer = new byte[this.InputBlockSize];
				int num = inputCount - this.InputBlockSize;
				Buffer.InternalBlockCopy(inputBuffer, inputOffset + num, this._depadBuffer, 0, this.InputBlockSize);
				return Utils._DecryptData(this._safeKeyHandle, inputBuffer, inputOffset, num, ref outputBuffer, outputOffset, this.PaddingValue, false);
			}
			int num2 = Utils._DecryptData(this._safeKeyHandle, this._depadBuffer, 0, this._depadBuffer.Length, ref outputBuffer, outputOffset, this.PaddingValue, false);
			outputOffset += this.OutputBlockSize;
			int num3 = inputCount - this.InputBlockSize;
			Buffer.InternalBlockCopy(inputBuffer, inputOffset + num3, this._depadBuffer, 0, this.InputBlockSize);
			num2 = Utils._DecryptData(this._safeKeyHandle, inputBuffer, inputOffset, num3, ref outputBuffer, outputOffset, this.PaddingValue, false);
			return this.OutputBlockSize + num2;
		}

		// Token: 0x06004E7A RID: 20090 RVA: 0x00110894 File Offset: 0x0010F894
		public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
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
			if (this.encryptOrDecrypt == CryptoAPITransformMode.Encrypt)
			{
				byte[] array = null;
				Utils._EncryptData(this._safeKeyHandle, inputBuffer, inputOffset, inputCount, ref array, 0, this.PaddingValue, true);
				this.Reset();
				return array;
			}
			if (inputCount % this.InputBlockSize != 0)
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptography_SSD_InvalidDataSize"));
			}
			if (this._depadBuffer == null)
			{
				byte[] array2 = null;
				Utils._DecryptData(this._safeKeyHandle, inputBuffer, inputOffset, inputCount, ref array2, 0, this.PaddingValue, true);
				this.Reset();
				return array2;
			}
			byte[] array3 = new byte[this._depadBuffer.Length + inputCount];
			Buffer.InternalBlockCopy(this._depadBuffer, 0, array3, 0, this._depadBuffer.Length);
			Buffer.InternalBlockCopy(inputBuffer, inputOffset, array3, this._depadBuffer.Length, inputCount);
			byte[] array4 = null;
			Utils._DecryptData(this._safeKeyHandle, array3, 0, array3.Length, ref array4, 0, this.PaddingValue, true);
			this.Reset();
			return array4;
		}

		// Token: 0x04002852 RID: 10322
		private int BlockSizeValue;

		// Token: 0x04002853 RID: 10323
		private byte[] IVValue;

		// Token: 0x04002854 RID: 10324
		private CipherMode ModeValue;

		// Token: 0x04002855 RID: 10325
		private PaddingMode PaddingValue;

		// Token: 0x04002856 RID: 10326
		private CryptoAPITransformMode encryptOrDecrypt;

		// Token: 0x04002857 RID: 10327
		private byte[] _rgbKey;

		// Token: 0x04002858 RID: 10328
		private byte[] _depadBuffer;

		// Token: 0x04002859 RID: 10329
		private SafeKeyHandle _safeKeyHandle;

		// Token: 0x0400285A RID: 10330
		private SafeProvHandle _safeProvHandle;
	}
}
