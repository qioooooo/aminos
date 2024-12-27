using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Cryptography.Pkcs
{
	// Token: 0x02000077 RID: 119
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class ContentInfo
	{
		// Token: 0x06000185 RID: 389 RVA: 0x00007FEA File Offset: 0x00006FEA
		private ContentInfo()
			: this(new Oid("1.2.840.113549.1.7.1"), new byte[0])
		{
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00008002 File Offset: 0x00007002
		public ContentInfo(byte[] content)
			: this(new Oid("1.2.840.113549.1.7.1"), content)
		{
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00008015 File Offset: 0x00007015
		internal ContentInfo(string contentType, byte[] content)
			: this(new Oid(contentType), content)
		{
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00008024 File Offset: 0x00007024
		public ContentInfo(Oid contentType, byte[] content)
		{
			if (contentType == null)
			{
				throw new ArgumentNullException("contentType");
			}
			if (content == null)
			{
				throw new ArgumentNullException("content");
			}
			this.m_contentType = contentType;
			this.m_content = content;
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000189 RID: 393 RVA: 0x00008061 File Offset: 0x00007061
		public Oid ContentType
		{
			get
			{
				return this.m_contentType;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00008069 File Offset: 0x00007069
		public byte[] Content
		{
			get
			{
				return this.m_content;
			}
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00008074 File Offset: 0x00007074
		~ContentInfo()
		{
			if (this.m_gcHandle.IsAllocated)
			{
				this.m_gcHandle.Free();
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600018C RID: 396 RVA: 0x000080B4 File Offset: 0x000070B4
		internal IntPtr pContent
		{
			get
			{
				if (IntPtr.Zero == this.m_pContent && this.m_content != null && this.m_content.Length != 0)
				{
					this.m_gcHandle = GCHandle.Alloc(this.m_content, GCHandleType.Pinned);
					this.m_pContent = Marshal.UnsafeAddrOfPinnedArrayElement(this.m_content, 0);
				}
				return this.m_pContent;
			}
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00008110 File Offset: 0x00007110
		public static Oid GetContentType(byte[] encodedMessage)
		{
			if (encodedMessage == null)
			{
				throw new ArgumentNullException("encodedMessage");
			}
			SafeCryptMsgHandle safeCryptMsgHandle = CAPISafe.CryptMsgOpenToDecode(65537U, 0U, 0U, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
			if (safeCryptMsgHandle == null || safeCryptMsgHandle.IsInvalid)
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			if (!CAPISafe.CryptMsgUpdate(safeCryptMsgHandle, encodedMessage, (uint)encodedMessage.Length, true))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			Oid oid;
			switch (PkcsUtils.GetMessageType(safeCryptMsgHandle))
			{
			case 1U:
				oid = new Oid("1.2.840.113549.1.7.1");
				break;
			case 2U:
				oid = new Oid("1.2.840.113549.1.7.2");
				break;
			case 3U:
				oid = new Oid("1.2.840.113549.1.7.3");
				break;
			case 4U:
				oid = new Oid("1.2.840.113549.1.7.4");
				break;
			case 5U:
				oid = new Oid("1.2.840.113549.1.7.5");
				break;
			case 6U:
				oid = new Oid("1.2.840.113549.1.7.6");
				break;
			default:
				throw new CryptographicException(-2146889724);
			}
			safeCryptMsgHandle.Dispose();
			return oid;
		}

		// Token: 0x040004A2 RID: 1186
		private Oid m_contentType;

		// Token: 0x040004A3 RID: 1187
		private byte[] m_content;

		// Token: 0x040004A4 RID: 1188
		private IntPtr m_pContent = IntPtr.Zero;

		// Token: 0x040004A5 RID: 1189
		private GCHandle m_gcHandle;
	}
}
