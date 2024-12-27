using System;
using System.Text;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000620 RID: 1568
	public class PhysicalAddress
	{
		// Token: 0x06003033 RID: 12339 RVA: 0x000CFF37 File Offset: 0x000CEF37
		public PhysicalAddress(byte[] address)
		{
			this.address = address;
		}

		// Token: 0x06003034 RID: 12340 RVA: 0x000CFF50 File Offset: 0x000CEF50
		public override int GetHashCode()
		{
			if (this.changed)
			{
				this.changed = false;
				this.hash = 0;
				int num = this.address.Length & -4;
				int i;
				for (i = 0; i < num; i += 4)
				{
					this.hash ^= (int)this.address[i] | ((int)this.address[i + 1] << 8) | ((int)this.address[i + 2] << 16) | ((int)this.address[i + 3] << 24);
				}
				if ((this.address.Length & 3) != 0)
				{
					int num2 = 0;
					int num3 = 0;
					while (i < this.address.Length)
					{
						num2 |= (int)this.address[i] << num3;
						num3 += 8;
						i++;
					}
					this.hash ^= num2;
				}
			}
			return this.hash;
		}

		// Token: 0x06003035 RID: 12341 RVA: 0x000D0018 File Offset: 0x000CF018
		public override bool Equals(object comparand)
		{
			PhysicalAddress physicalAddress = (PhysicalAddress)comparand;
			if (this.address.Length != physicalAddress.address.Length)
			{
				return false;
			}
			for (int i = 0; i < physicalAddress.address.Length; i++)
			{
				if (this.address[i] != physicalAddress.address[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003036 RID: 12342 RVA: 0x000D0068 File Offset: 0x000CF068
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte b in this.address)
			{
				int num = (b >> 4) & 15;
				for (int j = 0; j < 2; j++)
				{
					if (num < 10)
					{
						stringBuilder.Append((char)(num + 48));
					}
					else
					{
						stringBuilder.Append((char)(num + 55));
					}
					num = (int)(b & 15);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06003037 RID: 12343 RVA: 0x000D00DC File Offset: 0x000CF0DC
		public byte[] GetAddressBytes()
		{
			byte[] array = new byte[this.address.Length];
			Buffer.BlockCopy(this.address, 0, array, 0, this.address.Length);
			return array;
		}

		// Token: 0x06003038 RID: 12344 RVA: 0x000D0110 File Offset: 0x000CF110
		public static PhysicalAddress Parse(string address)
		{
			int num = 0;
			bool flag = false;
			if (address == null)
			{
				return PhysicalAddress.None;
			}
			byte[] array;
			if (address.IndexOf('-') >= 0)
			{
				flag = true;
				array = new byte[(address.Length + 1) / 3];
			}
			else
			{
				if (address.Length % 2 > 0)
				{
					throw new FormatException(SR.GetString("net_bad_mac_address"));
				}
				array = new byte[address.Length / 2];
			}
			int num2 = 0;
			int i = 0;
			while (i < address.Length)
			{
				int num3 = (int)address[i];
				if (num3 >= 48 && num3 <= 57)
				{
					num3 -= 48;
					goto IL_00C3;
				}
				if (num3 >= 65 && num3 <= 70)
				{
					num3 -= 55;
					goto IL_00C3;
				}
				if (num3 != 45)
				{
					throw new FormatException(SR.GetString("net_bad_mac_address"));
				}
				if (num != 2)
				{
					throw new FormatException(SR.GetString("net_bad_mac_address"));
				}
				num = 0;
				IL_0108:
				i++;
				continue;
				IL_00C3:
				if (flag && num >= 2)
				{
					throw new FormatException(SR.GetString("net_bad_mac_address"));
				}
				if (num % 2 == 0)
				{
					array[num2] = (byte)(num3 << 4);
				}
				else
				{
					byte[] array2 = array;
					int num4 = num2++;
					array2[num4] |= (byte)num3;
				}
				num++;
				goto IL_0108;
			}
			if (num < 2)
			{
				throw new FormatException(SR.GetString("net_bad_mac_address"));
			}
			return new PhysicalAddress(array);
		}

		// Token: 0x04002DEC RID: 11756
		private byte[] address;

		// Token: 0x04002DED RID: 11757
		private bool changed = true;

		// Token: 0x04002DEE RID: 11758
		private int hash;

		// Token: 0x04002DEF RID: 11759
		public static readonly PhysicalAddress None = new PhysicalAddress(new byte[0]);
	}
}
