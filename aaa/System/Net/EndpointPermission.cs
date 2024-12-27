using System;
using System.Globalization;
using System.Security;

namespace System.Net
{
	// Token: 0x02000442 RID: 1090
	[Serializable]
	public class EndpointPermission
	{
		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x0600223F RID: 8767 RVA: 0x00087D84 File Offset: 0x00086D84
		public string Hostname
		{
			get
			{
				return this.hostname;
			}
		}

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06002240 RID: 8768 RVA: 0x00087D8C File Offset: 0x00086D8C
		public TransportType Transport
		{
			get
			{
				return this.transport;
			}
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06002241 RID: 8769 RVA: 0x00087D94 File Offset: 0x00086D94
		public int Port
		{
			get
			{
				return this.port;
			}
		}

		// Token: 0x06002242 RID: 8770 RVA: 0x00087D9C File Offset: 0x00086D9C
		internal EndpointPermission(string epname, int port, TransportType trtype)
		{
			if (EndpointPermission.CheckEndPointName(epname) == EndpointPermission.EndPointType.Invalid)
			{
				throw new ArgumentException(SR.GetString("net_perm_epname", new object[] { epname }), "epname");
			}
			if (!ValidationHelper.ValidateTcpPort(port) && port != -1)
			{
				throw new ArgumentOutOfRangeException(SR.GetString("net_perm_invalid_val", new object[]
				{
					"Port",
					port.ToString(NumberFormatInfo.InvariantInfo)
				}));
			}
			this.hostname = epname;
			this.port = port;
			this.transport = trtype;
			this.wildcard = false;
		}

		// Token: 0x06002243 RID: 8771 RVA: 0x00087E30 File Offset: 0x00086E30
		public override bool Equals(object obj)
		{
			EndpointPermission endpointPermission = (EndpointPermission)obj;
			return string.Compare(this.hostname, endpointPermission.hostname, StringComparison.OrdinalIgnoreCase) == 0 && this.port == endpointPermission.port && this.transport == endpointPermission.transport;
		}

		// Token: 0x06002244 RID: 8772 RVA: 0x00087E7B File Offset: 0x00086E7B
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x06002245 RID: 8773 RVA: 0x00087E88 File Offset: 0x00086E88
		internal bool IsDns
		{
			get
			{
				return !this.IsValidWildcard && EndpointPermission.CheckEndPointName(this.hostname) == EndpointPermission.EndPointType.DnsOrWildcard;
			}
		}

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x06002246 RID: 8774 RVA: 0x00087EA4 File Offset: 0x00086EA4
		private bool IsValidWildcard
		{
			get
			{
				int length = this.hostname.Length;
				if (length < 3)
				{
					return false;
				}
				if (this.hostname[0] == '.' || this.hostname[length - 1] == '.')
				{
					return false;
				}
				int num = 0;
				int num2 = 0;
				for (int i = 0; i < this.hostname.Length; i++)
				{
					if (this.hostname[i] == '.')
					{
						num++;
					}
					else if (this.hostname[i] == '*')
					{
						num2++;
					}
					else if (!char.IsDigit(this.hostname[i]))
					{
						return false;
					}
				}
				return num == 3 && num2 > 0;
			}
		}

		// Token: 0x06002247 RID: 8775 RVA: 0x00087F4C File Offset: 0x00086F4C
		internal bool MatchAddress(EndpointPermission e)
		{
			if (this.Hostname.Length == 0 || e.Hostname.Length == 0)
			{
				return false;
			}
			if (this.Hostname.Equals("0.0.0.0"))
			{
				return e.Hostname.Equals("*.*.*.*") || e.Hostname.Equals("0.0.0.0");
			}
			if (this.IsDns && e.IsDns)
			{
				return string.Compare(this.hostname, e.hostname, StringComparison.OrdinalIgnoreCase) == 0;
			}
			this.Resolve();
			e.Resolve();
			if ((this.address == null && !this.wildcard) || (e.address == null && !e.wildcard))
			{
				return false;
			}
			if (this.wildcard && !e.wildcard)
			{
				return false;
			}
			if (e.wildcard)
			{
				if (this.wildcard)
				{
					if (this.MatchWildcard(e.hostname))
					{
						return true;
					}
				}
				else
				{
					for (int i = 0; i < this.address.Length; i++)
					{
						if (e.MatchWildcard(this.address[i].ToString()))
						{
							return true;
						}
					}
				}
			}
			else
			{
				for (int j = 0; j < this.address.Length; j++)
				{
					for (int k = 0; k < e.address.Length; k++)
					{
						if (this.address[j].Equals(e.address[k]))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06002248 RID: 8776 RVA: 0x000880A4 File Offset: 0x000870A4
		internal bool MatchWildcard(string str)
		{
			string[] array = this.hostname.Split(EndpointPermission.DotSeparator);
			string[] array2 = str.Split(EndpointPermission.DotSeparator);
			if (array2.Length != 4 || array.Length != 4)
			{
				return false;
			}
			for (int i = 0; i < 4; i++)
			{
				if (array2[i] != array[i] && array[i] != "*")
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002249 RID: 8777 RVA: 0x00088108 File Offset: 0x00087108
		internal void Resolve()
		{
			if (this.cached)
			{
				return;
			}
			if (this.wildcard)
			{
				return;
			}
			if (this.IsValidWildcard)
			{
				this.wildcard = true;
				this.cached = true;
				return;
			}
			IPAddress ipaddress;
			if (IPAddress.TryParse(this.hostname, out ipaddress))
			{
				this.address = new IPAddress[1];
				this.address[0] = ipaddress;
				this.cached = true;
				return;
			}
			try
			{
				bool flag;
				IPHostEntry iphostEntry = Dns.InternalResolveFast(this.hostname, -1, out flag);
				if (iphostEntry != null)
				{
					this.address = iphostEntry.AddressList;
				}
			}
			catch (SecurityException)
			{
				throw;
			}
			catch
			{
			}
		}

		// Token: 0x0600224A RID: 8778 RVA: 0x000881AC File Offset: 0x000871AC
		internal bool SubsetMatch(EndpointPermission e)
		{
			return (this.transport == e.transport || e.transport == TransportType.All) && (this.port == e.port || e.port == -1 || this.port == 0) && this.MatchAddress(e);
		}

		// Token: 0x0600224B RID: 8779 RVA: 0x000881F8 File Offset: 0x000871F8
		public override string ToString()
		{
			object[] array = new object[5];
			array[0] = this.hostname;
			array[1] = "#";
			array[2] = this.port;
			array[3] = "#";
			object[] array2 = array;
			int num = 4;
			int num2 = (int)this.transport;
			array2[num] = num2.ToString(NumberFormatInfo.InvariantInfo);
			return string.Concat(array);
		}

		// Token: 0x0600224C RID: 8780 RVA: 0x00088250 File Offset: 0x00087250
		internal EndpointPermission Intersect(EndpointPermission E)
		{
			string text = null;
			TransportType transportType;
			if (this.transport == E.transport)
			{
				transportType = this.transport;
			}
			else if (this.transport == TransportType.All)
			{
				transportType = E.transport;
			}
			else
			{
				if (E.transport != TransportType.All)
				{
					return null;
				}
				transportType = this.transport;
			}
			int num;
			if (this.port == E.port)
			{
				num = this.port;
			}
			else if (this.port == -1)
			{
				num = E.port;
			}
			else
			{
				if (E.port != -1)
				{
					return null;
				}
				num = this.port;
			}
			if (this.Hostname.Equals("0.0.0.0"))
			{
				if (!E.Hostname.Equals("*.*.*.*") && !E.Hostname.Equals("0.0.0.0"))
				{
					return null;
				}
				text = this.Hostname;
			}
			else if (E.Hostname.Equals("0.0.0.0"))
			{
				if (!this.Hostname.Equals("*.*.*.*") && !this.Hostname.Equals("0.0.0.0"))
				{
					return null;
				}
				text = E.Hostname;
			}
			else if (this.IsDns && E.IsDns)
			{
				if (string.Compare(this.hostname, E.hostname, StringComparison.OrdinalIgnoreCase) != 0)
				{
					return null;
				}
				text = this.hostname;
			}
			else
			{
				this.Resolve();
				E.Resolve();
				if ((this.address == null && !this.wildcard) || (E.address == null && !E.wildcard))
				{
					return null;
				}
				if (this.wildcard && E.wildcard)
				{
					string[] array = this.hostname.Split(EndpointPermission.DotSeparator);
					string[] array2 = E.hostname.Split(EndpointPermission.DotSeparator);
					string text2 = "";
					if (array2.Length != 4 || array.Length != 4)
					{
						return null;
					}
					for (int i = 0; i < 4; i++)
					{
						if (i != 0)
						{
							text2 += ".";
						}
						if (array2[i] == array[i])
						{
							text2 += array2[i];
						}
						else if (array2[i] == "*")
						{
							text2 += array[i];
						}
						else
						{
							if (!(array[i] == "*"))
							{
								return null;
							}
							text2 += array2[i];
						}
					}
					text = text2;
				}
				else if (this.wildcard)
				{
					for (int j = 0; j < E.address.Length; j++)
					{
						if (this.MatchWildcard(E.address[j].ToString()))
						{
							text = E.hostname;
							break;
						}
					}
				}
				else if (E.wildcard)
				{
					for (int k = 0; k < this.address.Length; k++)
					{
						if (E.MatchWildcard(this.address[k].ToString()))
						{
							text = this.hostname;
							break;
						}
					}
				}
				else
				{
					if (this.address == E.address)
					{
						text = this.hostname;
					}
					int num2 = 0;
					while (text == null && num2 < this.address.Length)
					{
						for (int l = 0; l < E.address.Length; l++)
						{
							if (this.address[num2].Equals(E.address[l]))
							{
								text = this.hostname;
								break;
							}
						}
						num2++;
					}
				}
				if (text == null)
				{
					return null;
				}
			}
			return new EndpointPermission(text, num, transportType);
		}

		// Token: 0x0600224D RID: 8781 RVA: 0x00088598 File Offset: 0x00087598
		private static EndpointPermission.EndPointType CheckEndPointName(string name)
		{
			if (name == null)
			{
				return EndpointPermission.EndPointType.Invalid;
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			int i = 0;
			while (i < name.Length)
			{
				char c = name[i];
				char c2 = c;
				if (c2 <= '.')
				{
					if (c2 == '%')
					{
						goto IL_005B;
					}
					switch (c2)
					{
					case '*':
					case '-':
						goto IL_0057;
					case '+':
					case ',':
						goto IL_005F;
					case '.':
						break;
					default:
						goto IL_005F;
					}
				}
				else
				{
					if (c2 == ':')
					{
						goto IL_005B;
					}
					if (c2 == '_')
					{
						goto IL_0057;
					}
					goto IL_005F;
				}
				IL_00A5:
				i++;
				continue;
				IL_0057:
				flag2 = true;
				goto IL_00A5;
				IL_005B:
				flag = true;
				goto IL_00A5;
				IL_005F:
				if ((c > 'f' && c <= 'z') || (c > 'F' && c <= 'Z'))
				{
					flag2 = true;
					goto IL_00A5;
				}
				if ((c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F'))
				{
					flag3 = true;
					goto IL_00A5;
				}
				if (c < '0' || c > '9')
				{
					return EndpointPermission.EndPointType.Invalid;
				}
				goto IL_00A5;
			}
			if (!flag)
			{
				if (flag2)
				{
					return EndpointPermission.EndPointType.DnsOrWildcard;
				}
				if (!flag3)
				{
					return EndpointPermission.EndPointType.IPv4;
				}
				return EndpointPermission.EndPointType.DnsOrWildcard;
			}
			else
			{
				if (!flag2)
				{
					return EndpointPermission.EndPointType.IPv6;
				}
				return EndpointPermission.EndPointType.Invalid;
			}
		}

		// Token: 0x0400221A RID: 8730
		private const string encSeperator = "#";

		// Token: 0x0400221B RID: 8731
		internal string hostname;

		// Token: 0x0400221C RID: 8732
		internal int port;

		// Token: 0x0400221D RID: 8733
		internal TransportType transport;

		// Token: 0x0400221E RID: 8734
		internal bool wildcard;

		// Token: 0x0400221F RID: 8735
		internal IPAddress[] address;

		// Token: 0x04002220 RID: 8736
		internal bool cached;

		// Token: 0x04002221 RID: 8737
		private static char[] DotSeparator = new char[] { '.' };

		// Token: 0x02000443 RID: 1091
		private enum EndPointType
		{
			// Token: 0x04002223 RID: 8739
			Invalid,
			// Token: 0x04002224 RID: 8740
			IPv6,
			// Token: 0x04002225 RID: 8741
			DnsOrWildcard,
			// Token: 0x04002226 RID: 8742
			IPv4
		}
	}
}
