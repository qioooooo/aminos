using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Cryptography.Pkcs
{
	// Token: 0x0200006E RID: 110
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class RecipientInfoCollection : ICollection, IEnumerable
	{
		// Token: 0x06000153 RID: 339 RVA: 0x0000717D File Offset: 0x0000617D
		internal RecipientInfoCollection()
		{
			this.m_safeCryptMsgHandle = SafeCryptMsgHandle.InvalidHandle;
			this.m_recipientInfos = new ArrayList();
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000719B File Offset: 0x0000619B
		internal RecipientInfoCollection(RecipientInfo recipientInfo)
		{
			this.m_safeCryptMsgHandle = SafeCryptMsgHandle.InvalidHandle;
			this.m_recipientInfos = new ArrayList(1);
			this.m_recipientInfos.Add(recipientInfo);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x000071C8 File Offset: 0x000061C8
		internal unsafe RecipientInfoCollection(SafeCryptMsgHandle safeCryptMsgHandle)
		{
			bool flag = PkcsUtils.CmsSupported();
			uint num = 0U;
			uint num2 = (uint)Marshal.SizeOf(typeof(uint));
			if (flag)
			{
				if (!CAPISafe.CryptMsgGetParam(safeCryptMsgHandle, 33U, 0U, new IntPtr((void*)(&num)), new IntPtr((void*)(&num2))))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
			}
			else if (!CAPISafe.CryptMsgGetParam(safeCryptMsgHandle, 17U, 0U, new IntPtr((void*)(&num)), new IntPtr((void*)(&num2))))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			this.m_recipientInfos = new ArrayList();
			for (uint num3 = 0U; num3 < num; num3 += 1U)
			{
				if (flag)
				{
					SafeLocalAllocHandle safeLocalAllocHandle;
					uint num4;
					PkcsUtils.GetParam(safeCryptMsgHandle, 36U, num3, out safeLocalAllocHandle, out num4);
					CAPIBase.CMSG_CMS_RECIPIENT_INFO cmsg_CMS_RECIPIENT_INFO = (CAPIBase.CMSG_CMS_RECIPIENT_INFO)Marshal.PtrToStructure(safeLocalAllocHandle.DangerousGetHandle(), typeof(CAPIBase.CMSG_CMS_RECIPIENT_INFO));
					switch (cmsg_CMS_RECIPIENT_INFO.dwRecipientChoice)
					{
					case 1U:
					{
						CAPIBase.CMSG_KEY_TRANS_RECIPIENT_INFO cmsg_KEY_TRANS_RECIPIENT_INFO = (CAPIBase.CMSG_KEY_TRANS_RECIPIENT_INFO)Marshal.PtrToStructure(cmsg_CMS_RECIPIENT_INFO.pRecipientInfo, typeof(CAPIBase.CMSG_KEY_TRANS_RECIPIENT_INFO));
						this.m_recipientInfos.Add(new KeyTransRecipientInfo(safeLocalAllocHandle, cmsg_KEY_TRANS_RECIPIENT_INFO, num3));
						break;
					}
					case 2U:
					{
						CAPIBase.CMSG_KEY_AGREE_RECIPIENT_INFO cmsg_KEY_AGREE_RECIPIENT_INFO = (CAPIBase.CMSG_KEY_AGREE_RECIPIENT_INFO)Marshal.PtrToStructure(cmsg_CMS_RECIPIENT_INFO.pRecipientInfo, typeof(CAPIBase.CMSG_KEY_AGREE_RECIPIENT_INFO));
						switch (cmsg_KEY_AGREE_RECIPIENT_INFO.dwOriginatorChoice)
						{
						case 1U:
						{
							CAPIBase.CMSG_KEY_AGREE_CERT_ID_RECIPIENT_INFO cmsg_KEY_AGREE_CERT_ID_RECIPIENT_INFO = (CAPIBase.CMSG_KEY_AGREE_CERT_ID_RECIPIENT_INFO)Marshal.PtrToStructure(cmsg_CMS_RECIPIENT_INFO.pRecipientInfo, typeof(CAPIBase.CMSG_KEY_AGREE_CERT_ID_RECIPIENT_INFO));
							for (uint num5 = 0U; num5 < cmsg_KEY_AGREE_CERT_ID_RECIPIENT_INFO.cRecipientEncryptedKeys; num5 += 1U)
							{
								this.m_recipientInfos.Add(new KeyAgreeRecipientInfo(safeLocalAllocHandle, cmsg_KEY_AGREE_CERT_ID_RECIPIENT_INFO, num3, num5));
							}
							break;
						}
						case 2U:
						{
							CAPIBase.CMSG_KEY_AGREE_PUBLIC_KEY_RECIPIENT_INFO cmsg_KEY_AGREE_PUBLIC_KEY_RECIPIENT_INFO = (CAPIBase.CMSG_KEY_AGREE_PUBLIC_KEY_RECIPIENT_INFO)Marshal.PtrToStructure(cmsg_CMS_RECIPIENT_INFO.pRecipientInfo, typeof(CAPIBase.CMSG_KEY_AGREE_PUBLIC_KEY_RECIPIENT_INFO));
							for (uint num6 = 0U; num6 < cmsg_KEY_AGREE_PUBLIC_KEY_RECIPIENT_INFO.cRecipientEncryptedKeys; num6 += 1U)
							{
								this.m_recipientInfos.Add(new KeyAgreeRecipientInfo(safeLocalAllocHandle, cmsg_KEY_AGREE_PUBLIC_KEY_RECIPIENT_INFO, num3, num6));
							}
							break;
						}
						default:
							throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Cms_Invalid_Originator_Identifier_Choice"), cmsg_KEY_AGREE_RECIPIENT_INFO.dwOriginatorChoice.ToString(CultureInfo.CurrentCulture));
						}
						break;
					}
					default:
						throw new CryptographicException(-2147483647);
					}
				}
				else
				{
					SafeLocalAllocHandle safeLocalAllocHandle2;
					uint num7;
					PkcsUtils.GetParam(safeCryptMsgHandle, 19U, num3, out safeLocalAllocHandle2, out num7);
					CAPIBase.CERT_INFO cert_INFO = (CAPIBase.CERT_INFO)Marshal.PtrToStructure(safeLocalAllocHandle2.DangerousGetHandle(), typeof(CAPIBase.CERT_INFO));
					this.m_recipientInfos.Add(new KeyTransRecipientInfo(safeLocalAllocHandle2, cert_INFO, num3));
				}
			}
			this.m_safeCryptMsgHandle = safeCryptMsgHandle;
		}

		// Token: 0x17000041 RID: 65
		public RecipientInfo this[int index]
		{
			get
			{
				if (index < 0 || index >= this.m_recipientInfos.Count)
				{
					throw new ArgumentOutOfRangeException("index", SecurityResources.GetResourceString("ArgumentOutOfRange_Index"));
				}
				return (RecipientInfo)this.m_recipientInfos[index];
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000157 RID: 343 RVA: 0x0000746C File Offset: 0x0000646C
		public int Count
		{
			get
			{
				return this.m_recipientInfos.Count;
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00007479 File Offset: 0x00006479
		public RecipientInfoEnumerator GetEnumerator()
		{
			return new RecipientInfoEnumerator(this);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00007481 File Offset: 0x00006481
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new RecipientInfoEnumerator(this);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000748C File Offset: 0x0000648C
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

		// Token: 0x0600015B RID: 347 RVA: 0x00007526 File Offset: 0x00006526
		public void CopyTo(RecipientInfo[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600015C RID: 348 RVA: 0x00007530 File Offset: 0x00006530
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00007533 File Offset: 0x00006533
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x04000487 RID: 1159
		private SafeCryptMsgHandle m_safeCryptMsgHandle;

		// Token: 0x04000488 RID: 1160
		private ArrayList m_recipientInfos;
	}
}
