using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Cryptography.Pkcs
{
	// Token: 0x0200007C RID: 124
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class SignerInfoCollection : ICollection, IEnumerable
	{
		// Token: 0x060001E4 RID: 484 RVA: 0x0000B2A6 File Offset: 0x0000A2A6
		internal SignerInfoCollection()
		{
			this.m_signerInfos = new SignerInfo[0];
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000B2BC File Offset: 0x0000A2BC
		internal unsafe SignerInfoCollection(SignedCms signedCms)
		{
			uint num = 0U;
			uint num2 = (uint)Marshal.SizeOf(typeof(uint));
			SafeCryptMsgHandle cryptMsgHandle = signedCms.GetCryptMsgHandle();
			if (!CAPISafe.CryptMsgGetParam(cryptMsgHandle, 5U, 0U, new IntPtr((void*)(&num)), new IntPtr((void*)(&num2))))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			SignerInfo[] array = new SignerInfo[num];
			int num3 = 0;
			while ((long)num3 < (long)((ulong)num))
			{
				uint num4 = 0U;
				if (!CAPISafe.CryptMsgGetParam(cryptMsgHandle, 6U, (uint)num3, IntPtr.Zero, new IntPtr((void*)(&num4))))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
				SafeLocalAllocHandle safeLocalAllocHandle = CAPI.LocalAlloc(0U, new IntPtr((long)((ulong)num4)));
				if (!CAPISafe.CryptMsgGetParam(cryptMsgHandle, 6U, (uint)num3, safeLocalAllocHandle, new IntPtr((void*)(&num4))))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
				array[num3] = new SignerInfo(signedCms, safeLocalAllocHandle);
				num3++;
			}
			this.m_signerInfos = array;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000B390 File Offset: 0x0000A390
		internal SignerInfoCollection(SignedCms signedCms, SignerInfo signerInfo)
		{
			SignerInfo[] array = new SignerInfo[0];
			int num = 0;
			int num2 = 0;
			foreach (CryptographicAttributeObject cryptographicAttributeObject in signerInfo.UnsignedAttributes)
			{
				if (cryptographicAttributeObject.Oid.Value == "1.2.840.113549.1.9.6")
				{
					num += cryptographicAttributeObject.Values.Count;
				}
			}
			array = new SignerInfo[num];
			foreach (CryptographicAttributeObject cryptographicAttributeObject2 in signerInfo.UnsignedAttributes)
			{
				if (cryptographicAttributeObject2.Oid.Value == "1.2.840.113549.1.9.6")
				{
					for (int i = 0; i < cryptographicAttributeObject2.Values.Count; i++)
					{
						AsnEncodedData asnEncodedData = cryptographicAttributeObject2.Values[i];
						array[num2++] = new SignerInfo(signedCms, signerInfo, asnEncodedData.RawData);
					}
				}
			}
			this.m_signerInfos = array;
		}

		// Token: 0x1700005F RID: 95
		public SignerInfo this[int index]
		{
			get
			{
				if (index < 0 || index >= this.m_signerInfos.Length)
				{
					throw new ArgumentOutOfRangeException("index", SecurityResources.GetResourceString("ArgumentOutOfRange_Index"));
				}
				return this.m_signerInfos[index];
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x0000B4A8 File Offset: 0x0000A4A8
		public int Count
		{
			get
			{
				return this.m_signerInfos.Length;
			}
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000B4B2 File Offset: 0x0000A4B2
		public SignerInfoEnumerator GetEnumerator()
		{
			return new SignerInfoEnumerator(this);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000B4BA File Offset: 0x0000A4BA
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new SignerInfoEnumerator(this);
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000B4C4 File Offset: 0x0000A4C4
		public void CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(SecurityResources.GetResourceString("Arg_RankMultiDimNotSupported"));
			}
			if (index < 0 || index >= array.Length)
			{
				throw new ArgumentOutOfRangeException("index", SecurityResources.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (index + this.Count > array.Length)
			{
				throw new ArgumentException(SecurityResources.GetResourceString("Argument_InvalidOffLen"));
			}
			for (int i = 0; i < this.Count; i++)
			{
				array.SetValue(this[i], index);
				index++;
			}
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000B55E File Offset: 0x0000A55E
		public void CopyTo(SignerInfo[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001ED RID: 493 RVA: 0x0000B568 File Offset: 0x0000A568
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001EE RID: 494 RVA: 0x0000B56B File Offset: 0x0000A56B
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x040004B8 RID: 1208
		private SignerInfo[] m_signerInfos;
	}
}
