using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x02000538 RID: 1336
	internal class ServiceNameStore
	{
		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x060028D6 RID: 10454 RVA: 0x000A98B9 File Offset: 0x000A88B9
		public ServiceNameCollection ServiceNames
		{
			get
			{
				if (this.serviceNameCollection == null)
				{
					this.serviceNameCollection = new ServiceNameCollection(this.serviceNames);
				}
				return this.serviceNameCollection;
			}
		}

		// Token: 0x060028D7 RID: 10455 RVA: 0x000A98DA File Offset: 0x000A88DA
		public ServiceNameStore()
		{
			this.serviceNames = new List<string>();
			this.serviceNameCollection = null;
		}

		// Token: 0x060028D8 RID: 10456 RVA: 0x000A98F4 File Offset: 0x000A88F4
		private bool AddSingleServiceName(string spn)
		{
			if (this.Contains(spn))
			{
				return false;
			}
			this.serviceNames.Add(spn);
			return true;
		}

		// Token: 0x060028D9 RID: 10457 RVA: 0x000A9910 File Offset: 0x000A8910
		public bool Add(string uriPrefix)
		{
			string[] array = this.BuildServiceNames(uriPrefix);
			bool flag = false;
			foreach (string text in array)
			{
				if (this.AddSingleServiceName(text))
				{
					flag = true;
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.HttpListener, string.Concat(new string[]
						{
							"ServiceNameStore#",
							ValidationHelper.HashString(this),
							"::Add() adding default SPNs '",
							text,
							"' from prefix '",
							uriPrefix,
							"'"
						}));
					}
				}
			}
			if (flag)
			{
				this.serviceNameCollection = null;
			}
			else if (Logging.On)
			{
				Logging.PrintInfo(Logging.HttpListener, string.Concat(new string[]
				{
					"ServiceNameStore#",
					ValidationHelper.HashString(this),
					"::Add() no default SPN added for prefix '",
					uriPrefix,
					"'"
				}));
			}
			return flag;
		}

		// Token: 0x060028DA RID: 10458 RVA: 0x000A99FC File Offset: 0x000A89FC
		public bool Remove(string uriPrefix)
		{
			string text = this.BuildSimpleServiceName(uriPrefix);
			bool flag = this.Contains(text);
			if (flag)
			{
				this.serviceNames.Remove(text);
				this.serviceNameCollection = null;
			}
			if (Logging.On)
			{
				if (flag)
				{
					Logging.PrintInfo(Logging.HttpListener, string.Concat(new string[]
					{
						"ServiceNameStore#",
						ValidationHelper.HashString(this),
						"::Remove() removing default SPN '",
						text,
						"' from prefix '",
						uriPrefix,
						"'"
					}));
				}
				else
				{
					Logging.PrintInfo(Logging.HttpListener, string.Concat(new string[]
					{
						"ServiceNameStore#",
						ValidationHelper.HashString(this),
						"::Remove() no default SPN removed for prefix '",
						uriPrefix,
						"'"
					}));
				}
			}
			return flag;
		}

		// Token: 0x060028DB RID: 10459 RVA: 0x000A9AC4 File Offset: 0x000A8AC4
		private bool Contains(string newServiceName)
		{
			if (newServiceName == null)
			{
				return false;
			}
			bool flag = false;
			foreach (string text in this.serviceNames)
			{
				if (string.Compare(text, newServiceName, StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					flag = true;
					break;
				}
			}
			return flag;
		}

		// Token: 0x060028DC RID: 10460 RVA: 0x000A9B28 File Offset: 0x000A8B28
		public void Clear()
		{
			this.serviceNames.Clear();
			this.serviceNameCollection = null;
		}

		// Token: 0x060028DD RID: 10461 RVA: 0x000A9B3C File Offset: 0x000A8B3C
		private string ExtractHostname(string uriPrefix, bool allowInvalidUriStrings)
		{
			if (Uri.IsWellFormedUriString(uriPrefix, UriKind.Absolute))
			{
				Uri uri = new Uri(uriPrefix);
				return uri.Host;
			}
			if (allowInvalidUriStrings)
			{
				int num = uriPrefix.IndexOf("://") + 3;
				int num2 = num;
				bool flag = false;
				while (num2 < uriPrefix.Length && uriPrefix[num2] != '/' && (uriPrefix[num2] != ':' || flag))
				{
					if (uriPrefix[num2] == '[')
					{
						if (flag)
						{
							num2 = num;
							break;
						}
						flag = true;
					}
					if (flag && uriPrefix[num2] == ']')
					{
						flag = false;
					}
					num2++;
				}
				return uriPrefix.Substring(num, num2 - num);
			}
			return null;
		}

		// Token: 0x060028DE RID: 10462 RVA: 0x000A9BD0 File Offset: 0x000A8BD0
		public string BuildSimpleServiceName(string uriPrefix)
		{
			string text = this.ExtractHostname(uriPrefix, false);
			if (text != null)
			{
				return "HTTP/" + text;
			}
			return null;
		}

		// Token: 0x060028DF RID: 10463 RVA: 0x000A9BF8 File Offset: 0x000A8BF8
		public string[] BuildServiceNames(string uriPrefix)
		{
			string text = this.ExtractHostname(uriPrefix, true);
			IPAddress ipaddress = null;
			if (string.Compare(text, "*", StringComparison.InvariantCultureIgnoreCase) != 0 && string.Compare(text, "+", StringComparison.InvariantCultureIgnoreCase) != 0)
			{
				if (!IPAddress.TryParse(text, out ipaddress))
				{
					goto IL_007D;
				}
			}
			try
			{
				string hostName = Dns.GetHostEntry(string.Empty).HostName;
				return new string[] { "HTTP/" + hostName };
			}
			catch (SocketException)
			{
				return new string[0];
			}
			catch (SecurityException)
			{
				return new string[0];
			}
			IL_007D:
			if (!text.Contains("."))
			{
				try
				{
					string hostName2 = Dns.GetHostEntry(text).HostName;
					return new string[]
					{
						"HTTP/" + text,
						"HTTP/" + hostName2
					};
				}
				catch (SocketException)
				{
					return new string[] { "HTTP/" + text };
				}
				catch (SecurityException)
				{
					return new string[] { "HTTP/" + text };
				}
			}
			return new string[] { "HTTP/" + text };
		}

		// Token: 0x040027BD RID: 10173
		private List<string> serviceNames;

		// Token: 0x040027BE RID: 10174
		private ServiceNameCollection serviceNameCollection;
	}
}
