using System;
using System.ComponentModel;

namespace System.DirectoryServices
{
	// Token: 0x02000028 RID: 40
	public class DirectorySynchronization
	{
		// Token: 0x0600011F RID: 287 RVA: 0x00005C97 File Offset: 0x00004C97
		public DirectorySynchronization()
		{
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00005CAB File Offset: 0x00004CAB
		public DirectorySynchronization(DirectorySynchronizationOptions option)
		{
			this.Option = option;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00005CC6 File Offset: 0x00004CC6
		public DirectorySynchronization(DirectorySynchronization sync)
		{
			if (sync != null)
			{
				this.Option = sync.Option;
				this.ResetDirectorySynchronizationCookie(sync.GetDirectorySynchronizationCookie());
			}
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00005CF5 File Offset: 0x00004CF5
		public DirectorySynchronization(byte[] cookie)
		{
			this.ResetDirectorySynchronizationCookie(cookie);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00005D10 File Offset: 0x00004D10
		public DirectorySynchronization(DirectorySynchronizationOptions option, byte[] cookie)
		{
			this.Option = option;
			this.ResetDirectorySynchronizationCookie(cookie);
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00005D32 File Offset: 0x00004D32
		// (set) Token: 0x06000125 RID: 293 RVA: 0x00005D3C File Offset: 0x00004D3C
		[DefaultValue(DirectorySynchronizationOptions.None)]
		[DSDescription("DSDirectorySynchronizationFlag")]
		public DirectorySynchronizationOptions Option
		{
			get
			{
				return this.flag;
			}
			set
			{
				long num = (long)(value & ~(DirectorySynchronizationOptions.ObjectSecurity | DirectorySynchronizationOptions.ParentsFirst | DirectorySynchronizationOptions.PublicDataOnly | DirectorySynchronizationOptions.IncrementalValues));
				if (num != 0L)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DirectorySynchronizationOptions));
				}
				this.flag = value;
			}
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00005D78 File Offset: 0x00004D78
		public byte[] GetDirectorySynchronizationCookie()
		{
			byte[] array = new byte[this.cookie.Length];
			for (int i = 0; i < this.cookie.Length; i++)
			{
				array[i] = this.cookie[i];
			}
			return array;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00005DB2 File Offset: 0x00004DB2
		public void ResetDirectorySynchronizationCookie()
		{
			this.cookie = new byte[0];
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00005DC0 File Offset: 0x00004DC0
		public void ResetDirectorySynchronizationCookie(byte[] cookie)
		{
			if (cookie == null)
			{
				this.cookie = new byte[0];
				return;
			}
			this.cookie = new byte[cookie.Length];
			for (int i = 0; i < cookie.Length; i++)
			{
				this.cookie[i] = cookie[i];
			}
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00005E04 File Offset: 0x00004E04
		public DirectorySynchronization Copy()
		{
			return new DirectorySynchronization(this.flag, this.cookie);
		}

		// Token: 0x040001AB RID: 427
		private DirectorySynchronizationOptions flag;

		// Token: 0x040001AC RID: 428
		private byte[] cookie = new byte[0];
	}
}
