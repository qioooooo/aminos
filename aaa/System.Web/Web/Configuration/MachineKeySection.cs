﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x0200020E RID: 526
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class MachineKeySection : ConfigurationSection
	{
		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06001C3D RID: 7229 RVA: 0x00081212 File Offset: 0x00080212
		internal static bool IsDecryptionKeyAutogenerated
		{
			get
			{
				MachineKeySection.EnsureConfig();
				return MachineKeySection.s_config.AutogenKey;
			}
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06001C3E RID: 7230 RVA: 0x00081223 File Offset: 0x00080223
		internal bool AutogenKey
		{
			get
			{
				this.RuntimeDataInitialize();
				return this._AutogenKey;
			}
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06001C3F RID: 7231 RVA: 0x00081231 File Offset: 0x00080231
		internal byte[] ValidationKeyInternal
		{
			get
			{
				this.RuntimeDataInitialize();
				return (byte[])this._ValidationKey.Clone();
			}
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06001C40 RID: 7232 RVA: 0x00081249 File Offset: 0x00080249
		internal byte[] DecryptionKeyInternal
		{
			get
			{
				this.RuntimeDataInitialize();
				return (byte[])this._DecryptionKey.Clone();
			}
		}

		// Token: 0x06001C41 RID: 7233 RVA: 0x00081264 File Offset: 0x00080264
		static MachineKeySection()
		{
			MachineKeySection._properties.Add(MachineKeySection._propValidationKey);
			MachineKeySection._properties.Add(MachineKeySection._propDecryptionKey);
			MachineKeySection._properties.Add(MachineKeySection._propValidation);
			MachineKeySection._properties.Add(MachineKeySection._propDecryption);
			MachineKeySection._properties.Add(MachineKeySection._propCompatibilityMode);
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06001C43 RID: 7235 RVA: 0x000813A7 File Offset: 0x000803A7
		internal static MachineKeyCompatibilityMode CompatMode
		{
			get
			{
				MachineKeySection.EnsureConfig();
				return MachineKeySection.s_compatMode;
			}
		}

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06001C44 RID: 7236 RVA: 0x000813B3 File Offset: 0x000803B3
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return MachineKeySection._properties;
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06001C45 RID: 7237 RVA: 0x000813BA File Offset: 0x000803BA
		// (set) Token: 0x06001C46 RID: 7238 RVA: 0x000813CC File Offset: 0x000803CC
		[StringValidator(MinLength = 1)]
		[TypeConverter(typeof(WhiteSpaceTrimStringConverter))]
		[ConfigurationProperty("validationKey", DefaultValue = "AutoGenerate,IsolateApps")]
		public string ValidationKey
		{
			get
			{
				return (string)base[MachineKeySection._propValidationKey];
			}
			set
			{
				base[MachineKeySection._propValidationKey] = value;
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06001C47 RID: 7239 RVA: 0x000813DA File Offset: 0x000803DA
		// (set) Token: 0x06001C48 RID: 7240 RVA: 0x000813EC File Offset: 0x000803EC
		[ConfigurationProperty("decryptionKey", DefaultValue = "AutoGenerate,IsolateApps")]
		[StringValidator(MinLength = 1)]
		[TypeConverter(typeof(WhiteSpaceTrimStringConverter))]
		public string DecryptionKey
		{
			get
			{
				return (string)base[MachineKeySection._propDecryptionKey];
			}
			set
			{
				base[MachineKeySection._propDecryptionKey] = value;
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06001C49 RID: 7241 RVA: 0x000813FC File Offset: 0x000803FC
		// (set) Token: 0x06001C4A RID: 7242 RVA: 0x0008149C File Offset: 0x0008049C
		[StringValidator(MinLength = 1)]
		[TypeConverter(typeof(WhiteSpaceTrimStringConverter))]
		[ConfigurationProperty("decryption", DefaultValue = "Auto")]
		public string Decryption
		{
			get
			{
				string text = base[MachineKeySection._propDecryption] as string;
				if (text == null)
				{
					return "Auto";
				}
				if (text != "Auto" && text != "AES" && text != "3DES" && text != "DES")
				{
					throw new ConfigurationErrorsException(SR.GetString("Wrong_decryption_enum"), base.ElementInformation.Properties["decryption"].Source, base.ElementInformation.Properties["decryption"].LineNumber);
				}
				return text;
			}
			set
			{
				if (value != "AES" && value != "3DES" && value != "Auto" && value != "DES")
				{
					throw new ConfigurationErrorsException(SR.GetString("Wrong_decryption_enum"), base.ElementInformation.Properties["decryption"].Source, base.ElementInformation.Properties["decryption"].LineNumber);
				}
				base[MachineKeySection._propDecryption] = value;
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06001C4B RID: 7243 RVA: 0x0008152D File Offset: 0x0008052D
		// (set) Token: 0x06001C4C RID: 7244 RVA: 0x0008155A File Offset: 0x0008055A
		[TypeConverter(typeof(MachineKeyValidationConverter))]
		[ConfigurationProperty("validation", DefaultValue = MachineKeyValidation.SHA1)]
		public MachineKeyValidation Validation
		{
			get
			{
				if (!this._validationIsCached)
				{
					this._cachedValidation = (MachineKeyValidation)base[MachineKeySection._propValidation];
					this._validationIsCached = true;
				}
				return this._cachedValidation;
			}
			set
			{
				base[MachineKeySection._propValidation] = value;
				this._cachedValidation = value;
				this._validationIsCached = true;
			}
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06001C4D RID: 7245 RVA: 0x0008157B File Offset: 0x0008057B
		// (set) Token: 0x06001C4E RID: 7246 RVA: 0x0008158D File Offset: 0x0008058D
		[ConfigurationProperty("compatibilityMode", DefaultValue = MachineKeyCompatibilityMode.Framework20SP1)]
		public MachineKeyCompatibilityMode CompatibilityMode
		{
			get
			{
				return (MachineKeyCompatibilityMode)base[MachineKeySection._propCompatibilityMode];
			}
			set
			{
				base[MachineKeySection._propCompatibilityMode] = value;
			}
		}

		// Token: 0x06001C4F RID: 7247 RVA: 0x000815A0 File Offset: 0x000805A0
		protected override void Reset(ConfigurationElement parentElement)
		{
			MachineKeySection machineKeySection = parentElement as MachineKeySection;
			base.Reset(parentElement);
		}

		// Token: 0x06001C50 RID: 7248 RVA: 0x000815C0 File Offset: 0x000805C0
		private void RuntimeDataInitialize()
		{
			if (!this.DataInitialized)
			{
				byte[] array = null;
				bool flag = false;
				string text = this.ValidationKey;
				string text2 = HttpRuntime.AppDomainAppVirtualPath;
				if (text2 == null)
				{
					text2 = Process.GetCurrentProcess().MainModule.ModuleName;
					if (this.ValidationKey.Contains("AutoGenerate") || this.DecryptionKey.Contains("AutoGenerate"))
					{
						flag = true;
						array = new byte[88];
						RNGCryptoServiceProvider rngcryptoServiceProvider = new RNGCryptoServiceProvider();
						rngcryptoServiceProvider.GetBytes(array);
					}
				}
				bool flag2 = StringUtil.StringEndsWith(text, ",IsolateApps");
				if (flag2)
				{
					text = text.Substring(0, text.Length - ",IsolateApps".Length);
				}
				if (text == "AutoGenerate")
				{
					this._ValidationKey = new byte[64];
					if (flag)
					{
						Buffer.BlockCopy(array, 0, this._ValidationKey, 0, 64);
					}
					else
					{
						Buffer.BlockCopy(HttpRuntime.s_autogenKeys, 0, this._ValidationKey, 0, 64);
					}
				}
				else
				{
					if (text.Length > 128 || text.Length < 40)
					{
						throw new ConfigurationErrorsException(SR.GetString("Unable_to_get_cookie_authentication_validation_key", new object[] { text.Length.ToString(CultureInfo.InvariantCulture) }), base.ElementInformation.Properties["validationKey"].Source, base.ElementInformation.Properties["validationKey"].LineNumber);
					}
					this._ValidationKey = MachineKeySection.HexStringToByteArray(text);
					if (this._ValidationKey == null)
					{
						throw new ConfigurationErrorsException(SR.GetString("Invalid_validation_key"), base.ElementInformation.Properties["validationKey"].Source, base.ElementInformation.Properties["validationKey"].LineNumber);
					}
				}
				if (flag2)
				{
					int hashCode = StringComparer.InvariantCultureIgnoreCase.GetHashCode(text2);
					this._ValidationKey[0] = (byte)(hashCode & 255);
					this._ValidationKey[1] = (byte)((hashCode & 65280) >> 8);
					this._ValidationKey[2] = (byte)((hashCode & 16711680) >> 16);
					this._ValidationKey[3] = (byte)(((long)hashCode & (long)((ulong)(-16777216))) >> 24);
				}
				text = this.DecryptionKey;
				flag2 = StringUtil.StringEndsWith(text, ",IsolateApps");
				if (flag2)
				{
					text = text.Substring(0, text.Length - ",IsolateApps".Length);
				}
				if (text == "AutoGenerate")
				{
					this._DecryptionKey = new byte[24];
					if (flag)
					{
						Buffer.BlockCopy(array, 64, this._DecryptionKey, 0, 24);
					}
					else
					{
						Buffer.BlockCopy(HttpRuntime.s_autogenKeys, 64, this._DecryptionKey, 0, 24);
					}
					this._AutogenKey = true;
				}
				else
				{
					this._AutogenKey = false;
					if (text.Length % 2 != 0)
					{
						throw new ConfigurationErrorsException(SR.GetString("Invalid_decryption_key"), base.ElementInformation.Properties["decryptionKey"].Source, base.ElementInformation.Properties["decryptionKey"].LineNumber);
					}
					this._DecryptionKey = MachineKeySection.HexStringToByteArray(text);
					if (this._DecryptionKey == null)
					{
						throw new ConfigurationErrorsException(SR.GetString("Invalid_decryption_key"), base.ElementInformation.Properties["decryptionKey"].Source, base.ElementInformation.Properties["decryptionKey"].LineNumber);
					}
				}
				if (flag2)
				{
					int hashCode2 = StringComparer.InvariantCultureIgnoreCase.GetHashCode(text2);
					this._DecryptionKey[0] = (byte)(hashCode2 & 255);
					this._DecryptionKey[1] = (byte)((hashCode2 & 65280) >> 8);
					this._DecryptionKey[2] = (byte)((hashCode2 & 16711680) >> 16);
					this._DecryptionKey[3] = (byte)(((long)hashCode2 & (long)((ulong)(-16777216))) >> 24);
				}
				this.DataInitialized = true;
			}
		}

		// Token: 0x06001C51 RID: 7249 RVA: 0x00081977 File Offset: 0x00080977
		internal static byte[] EncryptOrDecryptData(bool fEncrypt, byte[] buf, byte[] modifier, int start, int length)
		{
			return MachineKeySection.EncryptOrDecryptData(fEncrypt, buf, modifier, start, length, IVType.Random, false);
		}

		// Token: 0x06001C52 RID: 7250 RVA: 0x00081986 File Offset: 0x00080986
		internal static byte[] EncryptOrDecryptData(bool fEncrypt, byte[] buf, byte[] modifier, int start, int length, bool useValidationSymAlgo)
		{
			return MachineKeySection.EncryptOrDecryptData(fEncrypt, buf, modifier, start, length, IVType.Random, useValidationSymAlgo);
		}

		// Token: 0x06001C53 RID: 7251 RVA: 0x00081996 File Offset: 0x00080996
		internal static byte[] EncryptOrDecryptData(bool fEncrypt, byte[] buf, byte[] modifier, int start, int length, IVType ivType)
		{
			return MachineKeySection.EncryptOrDecryptData(fEncrypt, buf, modifier, start, length, ivType, false);
		}

		// Token: 0x06001C54 RID: 7252 RVA: 0x000819A8 File Offset: 0x000809A8
		internal static byte[] EncryptOrDecryptData(bool fEncrypt, byte[] buf, byte[] modifier, int start, int length, IVType ivType, bool useValidationSymAlgo)
		{
			return MachineKeySection.EncryptOrDecryptData(fEncrypt, buf, modifier, start, length, ivType, useValidationSymAlgo, !AppSettings.UseLegacyEncryption);
		}

		// Token: 0x06001C55 RID: 7253 RVA: 0x000819CC File Offset: 0x000809CC
		internal static byte[] EncryptOrDecryptData(bool fEncrypt, byte[] buf, byte[] modifier, int start, int length, IVType ivType, bool useValidationSymAlgo, bool signData)
		{
			byte[] array8;
			try
			{
				MachineKeySection.EnsureConfig();
				if (!fEncrypt && signData)
				{
					if (start != 0 || length != buf.Length)
					{
						byte[] array = new byte[length];
						Buffer.BlockCopy(buf, start, array, 0, length);
						buf = array;
						start = 0;
					}
					buf = MachineKeySection.GetUnHashedData(buf);
					if (buf == null)
					{
						throw new HttpException(SR.GetString("Unable_to_validate_data"));
					}
					length = buf.Length;
				}
				MemoryStream memoryStream = new MemoryStream();
				ICryptoTransform cryptoTransform = MachineKeySection.GetCryptoTransform(fEncrypt, useValidationSymAlgo);
				CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
				bool flag = signData || (ivType != IVType.None && MachineKeySection.CompatMode > MachineKeyCompatibilityMode.Framework20SP1);
				if (fEncrypt && flag)
				{
					byte[] array2 = null;
					switch (ivType)
					{
					case IVType.Random:
						array2 = MachineKeySection.GetIVRandom(useValidationSymAlgo);
						break;
					case IVType.Hash:
						array2 = MachineKeySection.GetIVHash(buf, useValidationSymAlgo);
						break;
					}
					cryptoStream.Write(array2, 0, array2.Length);
				}
				cryptoStream.Write(buf, start, length);
				if (fEncrypt && modifier != null)
				{
					cryptoStream.Write(modifier, 0, modifier.Length);
				}
				cryptoStream.FlushFinalBlock();
				byte[] array3 = memoryStream.ToArray();
				cryptoStream.Close();
				MachineKeySection.ReturnCryptoTransform(fEncrypt, cryptoTransform, useValidationSymAlgo);
				byte[] array4;
				if (!fEncrypt && flag)
				{
					int ivlength = MachineKeySection.GetIVLength(useValidationSymAlgo);
					int num = array3.Length - ivlength;
					if (num < 0)
					{
						throw new Exception();
					}
					array4 = new byte[num];
					Buffer.BlockCopy(array3, ivlength, array4, 0, num);
				}
				else
				{
					array4 = array3;
				}
				if (!fEncrypt && modifier != null && modifier.Length > 0)
				{
					bool flag2 = false;
					for (int i = 0; i < modifier.Length; i++)
					{
						if (array4[array4.Length - modifier.Length + i] != modifier[i])
						{
							flag2 = true;
						}
					}
					if (flag2)
					{
						throw new HttpException(SR.GetString("Unable_to_validate_data"));
					}
					byte[] array5 = new byte[array4.Length - modifier.Length];
					Buffer.BlockCopy(array4, 0, array5, 0, array5.Length);
					array4 = array5;
				}
				if (fEncrypt && signData)
				{
					byte[] array6 = MachineKeySection.HashData(array4, null, 0, array4.Length);
					byte[] array7 = new byte[array4.Length + array6.Length];
					Buffer.BlockCopy(array4, 0, array7, 0, array4.Length);
					Buffer.BlockCopy(array6, 0, array7, array4.Length, array6.Length);
					array4 = array7;
				}
				array8 = array4;
			}
			catch
			{
				throw new HttpException(SR.GetString("Unable_to_validate_data"));
			}
			return array8;
		}

		// Token: 0x06001C56 RID: 7254 RVA: 0x00081C04 File Offset: 0x00080C04
		private static byte[] GetIVHash(byte[] buf, bool useValidationSymAlgo)
		{
			int ivlength = MachineKeySection.GetIVLength(useValidationSymAlgo);
			int num = ivlength;
			int i = 0;
			byte[] array = new byte[ivlength];
			byte[] array2 = buf;
			while (i < ivlength)
			{
				byte[] array3 = new byte[20];
				int sha1Hash = UnsafeNativeMethods.GetSHA1Hash(array2, array2.Length, array3, array3.Length);
				Marshal.ThrowExceptionForHR(sha1Hash);
				array2 = array3;
				int num2 = Math.Min(20, num);
				Buffer.BlockCopy(array2, 0, array, i, num2);
				i += num2;
				num -= num2;
			}
			return array;
		}

		// Token: 0x06001C57 RID: 7255 RVA: 0x00081C74 File Offset: 0x00080C74
		private static int GetIVLength(bool useValidationSymAlgo)
		{
			SymmetricAlgorithm symmetricAlgorithm = (useValidationSymAlgo ? MachineKeySection.s_oSymAlgoValidation : MachineKeySection.s_oSymAlgoDecryption);
			int num = symmetricAlgorithm.KeySize;
			if (num % 8 != 0)
			{
				num += 8 - num % 8;
			}
			return num / 8;
		}

		// Token: 0x06001C58 RID: 7256 RVA: 0x00081CA8 File Offset: 0x00080CA8
		private static byte[] GetIVRandom(bool useValidationSymAlgo)
		{
			byte[] array = new byte[MachineKeySection.GetIVLength(useValidationSymAlgo)];
			if (MachineKeySection.s_randomNumberGenerator == null)
			{
				MachineKeySection.s_randomNumberGenerator = new RNGCryptoServiceProvider();
			}
			MachineKeySection.s_randomNumberGenerator.GetBytes(array);
			return array;
		}

		// Token: 0x06001C59 RID: 7257 RVA: 0x00081CE0 File Offset: 0x00080CE0
		private static byte[] MD5HashForData(byte[] buf, byte[] modifier, int start, int length)
		{
			MD5 md = MD5.Create();
			int num = length + MachineKeySection.s_validationKey.Length;
			if (modifier != null)
			{
				num += modifier.Length;
			}
			byte[] array = new byte[num];
			Buffer.BlockCopy(buf, start, array, 0, length);
			if (modifier != null)
			{
				Buffer.BlockCopy(modifier, 0, array, length, modifier.Length);
				length += modifier.Length;
			}
			Buffer.BlockCopy(MachineKeySection.s_validationKey, 0, array, length, MachineKeySection.s_validationKey.Length);
			return md.ComputeHash(array);
		}

		// Token: 0x06001C5A RID: 7258 RVA: 0x00081D48 File Offset: 0x00080D48
		private static void SetInnerOuterKeys(byte[] validationKey)
		{
			if (validationKey.Length > 64)
			{
				byte[] array = new byte[20];
				int sha1Hash = UnsafeNativeMethods.GetSHA1Hash(validationKey, validationKey.Length, array, array.Length);
				Marshal.ThrowExceptionForHR(sha1Hash);
			}
			if (MachineKeySection.s_inner == null)
			{
				MachineKeySection.s_inner = new byte[64];
			}
			if (MachineKeySection.s_outer == null)
			{
				MachineKeySection.s_outer = new byte[64];
			}
			for (int i = 0; i < 64; i++)
			{
				MachineKeySection.s_inner[i] = 54;
				MachineKeySection.s_outer[i] = 92;
			}
			for (int i = 0; i < validationKey.Length; i++)
			{
				byte[] array2 = MachineKeySection.s_inner;
				int num = i;
				array2[num] ^= validationKey[i];
				byte[] array3 = MachineKeySection.s_outer;
				int num2 = i;
				array3[num2] ^= validationKey[i];
			}
		}

		// Token: 0x06001C5B RID: 7259 RVA: 0x00081E04 File Offset: 0x00080E04
		private static byte[] GetHMACSHA1Hash(byte[] buf, byte[] modifier, int start, int length)
		{
			if (length < 0 || buf == null || length > buf.Length)
			{
				throw new ArgumentException(SR.GetString("InvalidArgumentValue", new object[] { "length" }));
			}
			if (start < 0 || start >= length)
			{
				throw new ArgumentException(SR.GetString("InvalidArgumentValue", new object[] { "start" }));
			}
			byte[] array = new byte[20];
			int hmacsha1Hash = UnsafeNativeMethods.GetHMACSHA1Hash(buf, start, length, modifier, (modifier == null) ? 0 : modifier.Length, MachineKeySection.s_inner, MachineKeySection.s_inner.Length, MachineKeySection.s_outer, MachineKeySection.s_outer.Length, array, array.Length);
			Marshal.ThrowExceptionForHR(hmacsha1Hash);
			return array;
		}

		// Token: 0x06001C5C RID: 7260 RVA: 0x00081EA4 File Offset: 0x00080EA4
		internal static string HashAndBase64EncodeString(string s)
		{
			byte[] bytes = Encoding.Unicode.GetBytes(s);
			byte[] array = MachineKeySection.HashData(bytes, null, 0, bytes.Length);
			return Convert.ToBase64String(array);
		}

		// Token: 0x06001C5D RID: 7261 RVA: 0x00081ED4 File Offset: 0x00080ED4
		internal static void DestroyByteArray(byte[] buf)
		{
			if (buf == null || buf.Length < 1)
			{
				return;
			}
			for (int i = 0; i < buf.Length; i++)
			{
				buf[i] = 0;
			}
		}

		// Token: 0x06001C5E RID: 7262 RVA: 0x00081EFD File Offset: 0x00080EFD
		internal void DestroyKeys()
		{
			MachineKeySection.DestroyByteArray(this._ValidationKey);
			MachineKeySection.DestroyByteArray(this._DecryptionKey);
		}

		// Token: 0x06001C5F RID: 7263 RVA: 0x00081F18 File Offset: 0x00080F18
		internal unsafe static string ByteArrayToHexString(byte[] buf, int iLen)
		{
			char[] array = MachineKeySection.s_acharval;
			if (array == null)
			{
				array = new char[16];
				int num = array.Length;
				while (--num >= 0)
				{
					if (num < 10)
					{
						array[num] = (char)(48 + num);
					}
					else
					{
						array[num] = (char)(65 + (num - 10));
					}
				}
				MachineKeySection.s_acharval = array;
			}
			if (buf == null)
			{
				return null;
			}
			if (iLen == 0)
			{
				iLen = buf.Length;
			}
			char[] array2 = new char[iLen * 2];
			fixed (char* ptr = array2, ptr2 = array)
			{
				fixed (byte* ptr3 = buf)
				{
					char* ptr4 = ptr;
					byte* ptr5 = ptr3;
					while (--iLen >= 0)
					{
						*(ptr4++) = ptr2[(*ptr5 & 240) >> 4];
						*(ptr4++) = ptr2[*ptr5 & 15];
						ptr5++;
					}
				}
			}
			return new string(array2);
		}

		// Token: 0x06001C60 RID: 7264 RVA: 0x0008202C File Offset: 0x0008102C
		private static void EnsureConfig()
		{
			if (MachineKeySection.s_config == null)
			{
				lock (MachineKeySection.s_initLock)
				{
					if (MachineKeySection.s_config == null)
					{
						MachineKeySection machineKey = RuntimeConfig.GetAppConfig().MachineKey;
						machineKey.ConfigureEncryptionObject();
						MachineKeySection.s_config = machineKey;
						MachineKeySection.s_compatMode = machineKey.CompatibilityMode;
					}
				}
			}
		}

		// Token: 0x06001C61 RID: 7265 RVA: 0x00082090 File Offset: 0x00081090
		internal static byte[] GetEncodedData(byte[] buf, byte[] modifier, int start, ref int length)
		{
			MachineKeySection.EnsureConfig();
			byte[] array = MachineKeySection.HashData(buf, modifier, start, length);
			byte[] array2;
			if (buf.Length - start - length >= array.Length)
			{
				Buffer.BlockCopy(array, 0, buf, start + length, array.Length);
				array2 = buf;
			}
			else
			{
				array2 = new byte[length + array.Length];
				Buffer.BlockCopy(buf, start, array2, 0, length);
				Buffer.BlockCopy(array, 0, array2, length, array.Length);
				start = 0;
			}
			length += array.Length;
			if (MachineKeySection.s_config.Validation == MachineKeyValidation.TripleDES || MachineKeySection.s_config.Validation == MachineKeyValidation.AES)
			{
				array2 = MachineKeySection.EncryptOrDecryptData(true, array2, modifier, start, length, true);
				length = array2.Length;
			}
			return array2;
		}

		// Token: 0x06001C62 RID: 7266 RVA: 0x0008212C File Offset: 0x0008112C
		internal static byte[] GetDecodedData(byte[] buf, byte[] modifier, int start, int length, ref int dataLength)
		{
			MachineKeySection.EnsureConfig();
			if (MachineKeySection.s_config.Validation == MachineKeyValidation.TripleDES || MachineKeySection.s_config.Validation == MachineKeyValidation.AES)
			{
				buf = MachineKeySection.EncryptOrDecryptData(false, buf, modifier, start, length, true);
				if (buf == null || buf.Length < 20)
				{
					throw new HttpException(SR.GetString("Unable_to_validate_data"));
				}
				length = buf.Length;
				start = 0;
			}
			if (length < 20 || start < 0 || start >= length)
			{
				throw new HttpException(SR.GetString("Unable_to_validate_data"));
			}
			byte[] array = MachineKeySection.HashData(buf, modifier, start, length - 20);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != buf[start + length - 20 + i])
				{
					throw new HttpException(SR.GetString("Unable_to_validate_data"));
				}
			}
			dataLength = length - 20;
			return buf;
		}

		// Token: 0x06001C63 RID: 7267 RVA: 0x000821E8 File Offset: 0x000811E8
		internal static byte[] HashData(byte[] buf, byte[] modifier, int start, int length)
		{
			MachineKeySection.EnsureConfig();
			byte[] array;
			if (MachineKeySection.s_config.Validation == MachineKeyValidation.MD5)
			{
				array = MachineKeySection.MD5HashForData(buf, modifier, start, length);
			}
			else
			{
				array = MachineKeySection.GetHMACSHA1Hash(buf, modifier, start, length);
			}
			if (array.Length < 20)
			{
				byte[] array2 = new byte[20];
				Buffer.BlockCopy(array, 0, array2, 0, array.Length);
				array = array2;
			}
			return array;
		}

		// Token: 0x06001C64 RID: 7268 RVA: 0x0008223C File Offset: 0x0008123C
		private void ConfigureEncryptionObject()
		{
			using (new ApplicationImpersonationContext())
			{
				MachineKeySection.s_validationKey = this.ValidationKeyInternal;
				byte[] decryptionKeyInternal = this.DecryptionKeyInternal;
				MachineKeySection.SetInnerOuterKeys(MachineKeySection.s_validationKey);
				this.DestroyKeys();
				string decryption;
				if ((decryption = this.Decryption) != null)
				{
					if (decryption == "3DES")
					{
						MachineKeySection.s_oSymAlgoDecryption = new TripleDESCryptoServiceProvider();
						goto IL_009B;
					}
					if (decryption == "DES")
					{
						MachineKeySection.s_oSymAlgoDecryption = new DESCryptoServiceProvider();
						goto IL_009B;
					}
					if (decryption == "AES")
					{
						MachineKeySection.s_oSymAlgoDecryption = new RijndaelManaged();
						goto IL_009B;
					}
				}
				if (decryptionKeyInternal.Length == 8)
				{
					MachineKeySection.s_oSymAlgoDecryption = new DESCryptoServiceProvider();
				}
				else
				{
					MachineKeySection.s_oSymAlgoDecryption = new RijndaelManaged();
				}
				IL_009B:
				switch (this.Validation)
				{
				case MachineKeyValidation.TripleDES:
					if (decryptionKeyInternal.Length == 8)
					{
						MachineKeySection.s_oSymAlgoValidation = new DESCryptoServiceProvider();
					}
					else
					{
						MachineKeySection.s_oSymAlgoValidation = new TripleDESCryptoServiceProvider();
					}
					break;
				case MachineKeyValidation.AES:
					MachineKeySection.s_oSymAlgoValidation = new RijndaelManaged();
					break;
				}
				if (MachineKeySection.s_oSymAlgoValidation != null)
				{
					this.SetKeyOnSymAlgorithm(MachineKeySection.s_oSymAlgoValidation, decryptionKeyInternal);
					MachineKeySection.s_oEncryptorStackValidation = new Stack();
					MachineKeySection.s_oDecryptorStackValidation = new Stack();
				}
				this.SetKeyOnSymAlgorithm(MachineKeySection.s_oSymAlgoDecryption, decryptionKeyInternal);
				MachineKeySection.s_oEncryptorStackDecryption = new Stack();
				MachineKeySection.s_oDecryptorStackDecryption = new Stack();
				MachineKeySection.DestroyByteArray(decryptionKeyInternal);
			}
		}

		// Token: 0x06001C65 RID: 7269 RVA: 0x0008239C File Offset: 0x0008139C
		private void SetKeyOnSymAlgorithm(SymmetricAlgorithm symAlgo, byte[] dKey)
		{
			try
			{
				if (dKey.Length == 24 && symAlgo is DESCryptoServiceProvider)
				{
					byte[] array = new byte[8];
					Buffer.BlockCopy(dKey, 0, array, 0, 8);
					symAlgo.Key = array;
					MachineKeySection.DestroyByteArray(array);
				}
				else
				{
					symAlgo.Key = dKey;
				}
				symAlgo.GenerateIV();
				symAlgo.IV = new byte[symAlgo.IV.Length];
			}
			catch (Exception ex)
			{
				throw new ConfigurationErrorsException(SR.GetString("Bad_machine_key", new object[] { ex.Message }), base.ElementInformation.Properties["decryptionKey"].Source, base.ElementInformation.Properties["decryptionKey"].LineNumber);
			}
		}

		// Token: 0x06001C66 RID: 7270 RVA: 0x00082460 File Offset: 0x00081460
		private static ICryptoTransform GetCryptoTransform(bool fEncrypt, bool useValidationSymAlgo)
		{
			Stack stack;
			if (useValidationSymAlgo)
			{
				stack = (fEncrypt ? MachineKeySection.s_oEncryptorStackValidation : MachineKeySection.s_oDecryptorStackValidation);
			}
			else
			{
				stack = (fEncrypt ? MachineKeySection.s_oEncryptorStackDecryption : MachineKeySection.s_oDecryptorStackDecryption);
			}
			lock (stack)
			{
				if (stack.Count > 0)
				{
					return (ICryptoTransform)stack.Pop();
				}
			}
			if (useValidationSymAlgo)
			{
				lock (MachineKeySection.s_oSymAlgoValidation)
				{
					return fEncrypt ? MachineKeySection.s_oSymAlgoValidation.CreateEncryptor() : MachineKeySection.s_oSymAlgoValidation.CreateDecryptor();
				}
			}
			ICryptoTransform cryptoTransform;
			lock (MachineKeySection.s_oSymAlgoDecryption)
			{
				cryptoTransform = (fEncrypt ? MachineKeySection.s_oSymAlgoDecryption.CreateEncryptor() : MachineKeySection.s_oSymAlgoDecryption.CreateDecryptor());
			}
			return cryptoTransform;
		}

		// Token: 0x06001C67 RID: 7271 RVA: 0x00082548 File Offset: 0x00081548
		private static void ReturnCryptoTransform(bool fEncrypt, ICryptoTransform ct, bool useValidationSymAlgo)
		{
			Stack stack;
			if (useValidationSymAlgo)
			{
				stack = (fEncrypt ? MachineKeySection.s_oEncryptorStackValidation : MachineKeySection.s_oDecryptorStackValidation);
			}
			else
			{
				stack = (fEncrypt ? MachineKeySection.s_oEncryptorStackDecryption : MachineKeySection.s_oDecryptorStackDecryption);
			}
			lock (stack)
			{
				if (stack.Count <= 100)
				{
					stack.Push(ct);
				}
			}
		}

		// Token: 0x06001C68 RID: 7272 RVA: 0x000825AC File Offset: 0x000815AC
		internal static byte[] HexStringToByteArray(string str)
		{
			if ((str.Length & 1) == 1)
			{
				return null;
			}
			byte[] array = MachineKeySection.s_ahexval;
			if (array == null)
			{
				array = new byte[103];
				int num = array.Length;
				while (--num >= 0)
				{
					if (48 <= num && num <= 57)
					{
						array[num] = (byte)(num - 48);
					}
					else if (97 <= num && num <= 102)
					{
						array[num] = (byte)(num - 97 + 10);
					}
					else if (65 <= num && num <= 70)
					{
						array[num] = (byte)(num - 65 + 10);
					}
				}
				MachineKeySection.s_ahexval = array;
			}
			byte[] array2 = new byte[str.Length / 2];
			int num2 = 0;
			int num3 = 0;
			int num4 = array2.Length;
			while (--num4 >= 0)
			{
				int num5;
				try
				{
					num5 = (int)array[(int)str[num2++]];
				}
				catch (ArgumentNullException)
				{
					num5 = 0;
					return null;
				}
				catch (ArgumentException)
				{
					num5 = 0;
					return null;
				}
				catch (IndexOutOfRangeException)
				{
					num5 = 0;
					return null;
				}
				int num6;
				try
				{
					num6 = (int)array[(int)str[num2++]];
				}
				catch (ArgumentNullException)
				{
					num6 = 0;
					return null;
				}
				catch (ArgumentException)
				{
					num6 = 0;
					return null;
				}
				catch (IndexOutOfRangeException)
				{
					num6 = 0;
					return null;
				}
				array2[num3++] = (byte)((num5 << 4) + num6);
			}
			return array2;
		}

		// Token: 0x06001C69 RID: 7273 RVA: 0x00082708 File Offset: 0x00081708
		internal static byte[] GetUnHashedData(byte[] bufHashed)
		{
			if (!MachineKeySection.VerifyHashedData(bufHashed))
			{
				return null;
			}
			byte[] array = new byte[bufHashed.Length - 20];
			Buffer.BlockCopy(bufHashed, 0, array, 0, array.Length);
			return array;
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x00082738 File Offset: 0x00081738
		internal static bool VerifyHashedData(byte[] bufHashed)
		{
			MachineKeySection.EnsureConfig();
			if (bufHashed.Length <= 20)
			{
				return false;
			}
			byte[] array = MachineKeySection.HashData(bufHashed, null, 0, bufHashed.Length - 20);
			if (array == null || array.Length != 20)
			{
				return false;
			}
			int num = bufHashed.Length - 20;
			bool flag = false;
			for (int i = 0; i < 20; i++)
			{
				if (array[i] != bufHashed[num + i])
				{
					flag = true;
				}
			}
			return !flag;
		}

		// Token: 0x040018BE RID: 6334
		private const int HASH_SIZE = 20;

		// Token: 0x040018BF RID: 6335
		private const int BLOCK_SIZE = 64;

		// Token: 0x040018C0 RID: 6336
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x040018C1 RID: 6337
		private static readonly ConfigurationProperty _propValidationKey = new ConfigurationProperty("validationKey", typeof(string), "AutoGenerate,IsolateApps", StdValidatorsAndConverters.WhiteSpaceTrimStringConverter, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.None);

		// Token: 0x040018C2 RID: 6338
		private static readonly ConfigurationProperty _propDecryptionKey = new ConfigurationProperty("decryptionKey", typeof(string), "AutoGenerate,IsolateApps", StdValidatorsAndConverters.WhiteSpaceTrimStringConverter, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.None);

		// Token: 0x040018C3 RID: 6339
		private static readonly ConfigurationProperty _propDecryption = new ConfigurationProperty("decryption", typeof(string), "Auto", StdValidatorsAndConverters.WhiteSpaceTrimStringConverter, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.None);

		// Token: 0x040018C4 RID: 6340
		private static readonly ConfigurationProperty _propValidation = new ConfigurationProperty("validation", typeof(MachineKeyValidation), MachineKeyValidation.SHA1, new MachineKeyValidationConverter(), null, ConfigurationPropertyOptions.None);

		// Token: 0x040018C5 RID: 6341
		private static readonly ConfigurationProperty _propCompatibilityMode = new ConfigurationProperty("compatibilityMode", typeof(MachineKeyCompatibilityMode), MachineKeyCompatibilityMode.Framework20SP1, null, null, ConfigurationPropertyOptions.None);

		// Token: 0x040018C6 RID: 6342
		private static object s_initLock = new object();

		// Token: 0x040018C7 RID: 6343
		private static MachineKeySection s_config;

		// Token: 0x040018C8 RID: 6344
		private static MachineKeyCompatibilityMode s_compatMode;

		// Token: 0x040018C9 RID: 6345
		private static RNGCryptoServiceProvider s_randomNumberGenerator;

		// Token: 0x040018CA RID: 6346
		private static SymmetricAlgorithm s_oSymAlgoDecryption;

		// Token: 0x040018CB RID: 6347
		private static Stack s_oEncryptorStackDecryption;

		// Token: 0x040018CC RID: 6348
		private static Stack s_oDecryptorStackDecryption;

		// Token: 0x040018CD RID: 6349
		private static SymmetricAlgorithm s_oSymAlgoValidation;

		// Token: 0x040018CE RID: 6350
		private static Stack s_oEncryptorStackValidation;

		// Token: 0x040018CF RID: 6351
		private static Stack s_oDecryptorStackValidation;

		// Token: 0x040018D0 RID: 6352
		private static byte[] s_validationKey;

		// Token: 0x040018D1 RID: 6353
		private static byte[] s_inner = null;

		// Token: 0x040018D2 RID: 6354
		private static byte[] s_outer = null;

		// Token: 0x040018D3 RID: 6355
		private bool _AutogenKey;

		// Token: 0x040018D4 RID: 6356
		private byte[] _ValidationKey;

		// Token: 0x040018D5 RID: 6357
		private byte[] _DecryptionKey;

		// Token: 0x040018D6 RID: 6358
		private bool DataInitialized;

		// Token: 0x040018D7 RID: 6359
		private bool _validationIsCached;

		// Token: 0x040018D8 RID: 6360
		private MachineKeyValidation _cachedValidation;

		// Token: 0x040018D9 RID: 6361
		private static char[] s_acharval;

		// Token: 0x040018DA RID: 6362
		private static byte[] s_ahexval;
	}
}
