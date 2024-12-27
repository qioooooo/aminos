using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000025 RID: 37
	public class DirSyncRequestControl : DirectoryControl
	{
		// Token: 0x0600009B RID: 155 RVA: 0x00004586 File Offset: 0x00003586
		public DirSyncRequestControl()
			: base("1.2.840.113556.1.4.841", null, true, true)
		{
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000045A1 File Offset: 0x000035A1
		public DirSyncRequestControl(byte[] cookie)
			: this()
		{
			this.dirsyncCookie = cookie;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000045B0 File Offset: 0x000035B0
		public DirSyncRequestControl(byte[] cookie, DirectorySynchronizationOptions option)
			: this(cookie)
		{
			this.Option = option;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000045C0 File Offset: 0x000035C0
		public DirSyncRequestControl(byte[] cookie, DirectorySynchronizationOptions option, int attributeCount)
			: this(cookie, option)
		{
			this.AttributeCount = attributeCount;
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600009F RID: 159 RVA: 0x000045D4 File Offset: 0x000035D4
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x00004618 File Offset: 0x00003618
		public byte[] Cookie
		{
			get
			{
				if (this.dirsyncCookie == null)
				{
					return new byte[0];
				}
				byte[] array = new byte[this.dirsyncCookie.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this.dirsyncCookie[i];
				}
				return array;
			}
			set
			{
				this.dirsyncCookie = value;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00004621 File Offset: 0x00003621
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x00004629 File Offset: 0x00003629
		public DirectorySynchronizationOptions Option
		{
			get
			{
				return this.flag;
			}
			set
			{
				this.flag = value;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00004632 File Offset: 0x00003632
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x0000463A File Offset: 0x0000363A
		public int AttributeCount
		{
			get
			{
				return this.count;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(Res.GetString("ValidValue"), "value");
				}
				this.count = value;
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0000465C File Offset: 0x0000365C
		public override byte[] GetValue()
		{
			object[] array = new object[]
			{
				(int)this.flag,
				this.count,
				this.dirsyncCookie
			};
			this.directoryControlValue = BerConverter.Encode("{iio}", array);
			return base.GetValue();
		}

		// Token: 0x040000E9 RID: 233
		private byte[] dirsyncCookie;

		// Token: 0x040000EA RID: 234
		private DirectorySynchronizationOptions flag;

		// Token: 0x040000EB RID: 235
		private int count = 1048576;
	}
}
