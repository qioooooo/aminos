using System;
using System.Globalization;
using System.Net.Sockets;
using System.Text;

namespace System.Net
{
	// Token: 0x0200043E RID: 1086
	public class SocketAddress
	{
		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x0600220C RID: 8716 RVA: 0x000867AC File Offset: 0x000857AC
		public AddressFamily Family
		{
			get
			{
				return (AddressFamily)((int)this.m_Buffer[0] | ((int)this.m_Buffer[1] << 8));
			}
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x0600220D RID: 8717 RVA: 0x000867CE File Offset: 0x000857CE
		public int Size
		{
			get
			{
				return this.m_Size;
			}
		}

		// Token: 0x1700073D RID: 1853
		public byte this[int offset]
		{
			get
			{
				if (offset < 0 || offset >= this.Size)
				{
					throw new IndexOutOfRangeException();
				}
				return this.m_Buffer[offset];
			}
			set
			{
				if (offset < 0 || offset >= this.Size)
				{
					throw new IndexOutOfRangeException();
				}
				if (this.m_Buffer[offset] != value)
				{
					this.m_changed = true;
				}
				this.m_Buffer[offset] = value;
			}
		}

		// Token: 0x06002210 RID: 8720 RVA: 0x00086823 File Offset: 0x00085823
		public SocketAddress(AddressFamily family)
			: this(family, 32)
		{
		}

		// Token: 0x06002211 RID: 8721 RVA: 0x00086830 File Offset: 0x00085830
		public SocketAddress(AddressFamily family, int size)
		{
			if (size < 2)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			this.m_Size = size;
			this.m_Buffer = new byte[(size / IntPtr.Size + 2) * IntPtr.Size];
			this.m_Buffer[0] = (byte)family;
			this.m_Buffer[1] = (byte)(family >> 8);
		}

		// Token: 0x06002212 RID: 8722 RVA: 0x00086890 File Offset: 0x00085890
		internal void CopyAddressSizeIntoBuffer()
		{
			this.m_Buffer[this.m_Buffer.Length - IntPtr.Size] = (byte)this.m_Size;
			this.m_Buffer[this.m_Buffer.Length - IntPtr.Size + 1] = (byte)(this.m_Size >> 8);
			this.m_Buffer[this.m_Buffer.Length - IntPtr.Size + 2] = (byte)(this.m_Size >> 16);
			this.m_Buffer[this.m_Buffer.Length - IntPtr.Size + 3] = (byte)(this.m_Size >> 24);
		}

		// Token: 0x06002213 RID: 8723 RVA: 0x0008691B File Offset: 0x0008591B
		internal int GetAddressSizeOffset()
		{
			return this.m_Buffer.Length - IntPtr.Size;
		}

		// Token: 0x06002214 RID: 8724 RVA: 0x0008692B File Offset: 0x0008592B
		internal unsafe void SetSize(IntPtr ptr)
		{
			this.m_Size = *(int*)(void*)ptr;
		}

		// Token: 0x06002215 RID: 8725 RVA: 0x0008693C File Offset: 0x0008593C
		public override bool Equals(object comparand)
		{
			SocketAddress socketAddress = comparand as SocketAddress;
			if (socketAddress == null || this.Size != socketAddress.Size)
			{
				return false;
			}
			for (int i = 0; i < this.Size; i++)
			{
				if (this[i] != socketAddress[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002216 RID: 8726 RVA: 0x00086988 File Offset: 0x00085988
		public override int GetHashCode()
		{
			if (this.m_changed)
			{
				this.m_changed = false;
				this.m_hash = 0;
				int num = this.Size & -4;
				int i;
				for (i = 0; i < num; i += 4)
				{
					this.m_hash ^= (int)this.m_Buffer[i] | ((int)this.m_Buffer[i + 1] << 8) | ((int)this.m_Buffer[i + 2] << 16) | ((int)this.m_Buffer[i + 3] << 24);
				}
				if ((this.Size & 3) != 0)
				{
					int num2 = 0;
					int num3 = 0;
					while (i < this.Size)
					{
						num2 |= (int)this.m_Buffer[i] << num3;
						num3 += 8;
						i++;
					}
					this.m_hash ^= num2;
				}
			}
			return this.m_hash;
		}

		// Token: 0x06002217 RID: 8727 RVA: 0x00086A48 File Offset: 0x00085A48
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 2; i < this.Size; i++)
			{
				if (i > 2)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(this[i].ToString(NumberFormatInfo.InvariantInfo));
			}
			return string.Concat(new string[]
			{
				this.Family.ToString(),
				":",
				this.Size.ToString(NumberFormatInfo.InvariantInfo),
				":{",
				stringBuilder.ToString(),
				"}"
			});
		}

		// Token: 0x04002202 RID: 8706
		internal const int IPv6AddressSize = 28;

		// Token: 0x04002203 RID: 8707
		internal const int IPv4AddressSize = 16;

		// Token: 0x04002204 RID: 8708
		private const int WriteableOffset = 2;

		// Token: 0x04002205 RID: 8709
		private const int MaxSize = 32;

		// Token: 0x04002206 RID: 8710
		internal int m_Size;

		// Token: 0x04002207 RID: 8711
		internal byte[] m_Buffer;

		// Token: 0x04002208 RID: 8712
		private bool m_changed = true;

		// Token: 0x04002209 RID: 8713
		private int m_hash;
	}
}
