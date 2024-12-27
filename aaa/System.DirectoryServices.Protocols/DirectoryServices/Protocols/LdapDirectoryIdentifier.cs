using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000075 RID: 117
	public class LdapDirectoryIdentifier : DirectoryIdentifier
	{
		// Token: 0x06000275 RID: 629 RVA: 0x0000D174 File Offset: 0x0000C174
		public LdapDirectoryIdentifier(string server)
			: this((server != null) ? new string[] { server } : null, false, false)
		{
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000D19C File Offset: 0x0000C19C
		public LdapDirectoryIdentifier(string server, int portNumber)
			: this((server != null) ? new string[] { server } : null, portNumber, false, false)
		{
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000D1C4 File Offset: 0x0000C1C4
		public LdapDirectoryIdentifier(string server, bool fullyQualifiedDnsHostName, bool connectionless)
			: this((server != null) ? new string[] { server } : null, fullyQualifiedDnsHostName, connectionless)
		{
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000D1EC File Offset: 0x0000C1EC
		public LdapDirectoryIdentifier(string server, int portNumber, bool fullyQualifiedDnsHostName, bool connectionless)
			: this((server != null) ? new string[] { server } : null, portNumber, fullyQualifiedDnsHostName, connectionless)
		{
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000D218 File Offset: 0x0000C218
		public LdapDirectoryIdentifier(string[] servers, bool fullyQualifiedDnsHostName, bool connectionless)
		{
			if (servers != null)
			{
				this.servers = new string[servers.Length];
				for (int i = 0; i < servers.Length; i++)
				{
					if (servers[i] != null)
					{
						string text = servers[i].Trim();
						string[] array = text.Split(new char[] { ' ' });
						if (array.Length > 1)
						{
							throw new ArgumentException(Res.GetString("WhiteSpaceServerName"));
						}
						this.servers[i] = text;
					}
				}
			}
			this.fullyQualifiedDnsHostName = fullyQualifiedDnsHostName;
			this.connectionless = connectionless;
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000D2A4 File Offset: 0x0000C2A4
		public LdapDirectoryIdentifier(string[] servers, int portNumber, bool fullyQualifiedDnsHostName, bool connectionless)
			: this(servers, fullyQualifiedDnsHostName, connectionless)
		{
			this.portNumber = portNumber;
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600027B RID: 635 RVA: 0x0000D2B8 File Offset: 0x0000C2B8
		public string[] Servers
		{
			get
			{
				if (this.servers == null)
				{
					return new string[0];
				}
				string[] array = new string[this.servers.Length];
				for (int i = 0; i < this.servers.Length; i++)
				{
					if (this.servers[i] != null)
					{
						array[i] = string.Copy(this.servers[i]);
					}
					else
					{
						array[i] = null;
					}
				}
				return array;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600027C RID: 636 RVA: 0x0000D316 File Offset: 0x0000C316
		public bool Connectionless
		{
			get
			{
				return this.connectionless;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600027D RID: 637 RVA: 0x0000D31E File Offset: 0x0000C31E
		public bool FullyQualifiedDnsHostName
		{
			get
			{
				return this.fullyQualifiedDnsHostName;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600027E RID: 638 RVA: 0x0000D326 File Offset: 0x0000C326
		public int PortNumber
		{
			get
			{
				return this.portNumber;
			}
		}

		// Token: 0x04000252 RID: 594
		private string[] servers;

		// Token: 0x04000253 RID: 595
		private bool fullyQualifiedDnsHostName;

		// Token: 0x04000254 RID: 596
		private bool connectionless;

		// Token: 0x04000255 RID: 597
		private int portNumber = 389;
	}
}
