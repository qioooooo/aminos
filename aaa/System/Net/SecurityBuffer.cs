using System;
using System.Runtime.InteropServices;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x02000409 RID: 1033
	internal class SecurityBuffer
	{
		// Token: 0x060020B8 RID: 8376 RVA: 0x00080ED8 File Offset: 0x0007FED8
		public SecurityBuffer(byte[] data, int offset, int size, BufferType tokentype)
		{
			this.offset = ((data == null || offset < 0) ? 0 : Math.Min(offset, data.Length));
			this.size = ((data == null || size < 0) ? 0 : Math.Min(size, data.Length - this.offset));
			this.type = tokentype;
			this.token = ((size == 0) ? null : data);
		}

		// Token: 0x060020B9 RID: 8377 RVA: 0x00080F39 File Offset: 0x0007FF39
		public SecurityBuffer(byte[] data, BufferType tokentype)
		{
			this.size = ((data == null) ? 0 : data.Length);
			this.type = tokentype;
			this.token = ((this.size == 0) ? null : data);
		}

		// Token: 0x060020BA RID: 8378 RVA: 0x00080F69 File Offset: 0x0007FF69
		public SecurityBuffer(int size, BufferType tokentype)
		{
			this.size = size;
			this.type = tokentype;
			this.token = ((size == 0) ? null : new byte[size]);
		}

		// Token: 0x060020BB RID: 8379 RVA: 0x00080F91 File Offset: 0x0007FF91
		public SecurityBuffer(ChannelBinding binding)
		{
			this.size = ((binding == null) ? 0 : binding.Size);
			this.type = BufferType.ChannelBindings;
			this.unmanagedToken = binding;
		}

		// Token: 0x0400208E RID: 8334
		public int size;

		// Token: 0x0400208F RID: 8335
		public BufferType type;

		// Token: 0x04002090 RID: 8336
		public byte[] token;

		// Token: 0x04002091 RID: 8337
		public SafeHandle unmanagedToken;

		// Token: 0x04002092 RID: 8338
		public int offset;
	}
}
