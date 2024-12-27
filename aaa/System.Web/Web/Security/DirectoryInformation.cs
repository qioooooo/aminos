using System;
using System.Collections;
using System.Configuration.Provider;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.Protocols;
using System.Globalization;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Web.Util;

namespace System.Web.Security
{
	// Token: 0x02000319 RID: 793
	internal sealed class DirectoryInformation
	{
		// Token: 0x06002729 RID: 10025 RVA: 0x000AB300 File Offset: 0x000AA300
		internal DirectoryInformation(string adspath, NetworkCredential credentials, string connProtection, int clientSearchTimeout, int serverSearchTimeout, bool enablePasswordReset)
		{
			AuthenticationTypes[,] array = new AuthenticationTypes[3, 2];
			array[1, 0] = AuthenticationTypes.Secure | AuthenticationTypes.Encryption;
			array[1, 1] = AuthenticationTypes.Encryption;
			array[2, 0] = AuthenticationTypes.Secure | AuthenticationTypes.Signing | AuthenticationTypes.Sealing;
			array[2, 1] = AuthenticationTypes.Secure | AuthenticationTypes.Signing | AuthenticationTypes.Sealing;
			this.authTypes = array;
			AuthType[,] array2 = new AuthType[3, 2];
			array2[0, 0] = AuthType.Negotiate;
			array2[0, 1] = AuthType.Basic;
			array2[1, 0] = AuthType.Negotiate;
			array2[1, 1] = AuthType.Basic;
			array2[2, 0] = AuthType.Negotiate;
			array2[2, 1] = AuthType.Negotiate;
			this.ldapAuthTypes = array2;
			base..ctor();
			this.adspath = adspath;
			this.credentials = credentials;
			this.clientSearchTimeout = clientSearchTimeout;
			this.serverSearchTimeout = serverSearchTimeout;
			if (!adspath.StartsWith("LDAP", StringComparison.Ordinal))
			{
				throw new ProviderException(SR.GetString("ADMembership_OnlyLdap_supported"));
			}
			NativeComInterfaces.IAdsPathname adsPathname = (NativeComInterfaces.IAdsPathname)new NativeComInterfaces.Pathname();
			try
			{
				adsPathname.Set(adspath, 1);
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode == -2147463168)
				{
					throw new ProviderException(SR.GetString("ADMembership_invalid_path"));
				}
				throw;
			}
			try
			{
				this.serverName = adsPathname.Retrieve(9);
			}
			catch (COMException ex2)
			{
				if (ex2.ErrorCode == -2147463168)
				{
					throw new ProviderException(SR.GetString("ADMembership_ServerlessADsPath_not_supported"));
				}
				throw;
			}
			this.creationContainerDN = (this.containerDN = adsPathname.Retrieve(7));
			int num = this.serverName.IndexOf(':');
			if (num != -1)
			{
				string text = this.serverName;
				this.serverName = text.Substring(0, num);
				this.port = int.Parse(text.Substring(num + 1), NumberFormatInfo.InvariantInfo);
				this.portSpecified = true;
			}
			if (string.Compare(connProtection, "Secure", StringComparison.Ordinal) == 0)
			{
				bool flag = false;
				bool flag2 = false;
				if (!this.IsDefaultCredential())
				{
					this.authenticationType = this.GetAuthenticationTypes(ActiveDirectoryConnectionProtection.Ssl, CredentialsType.NonWindows);
					this.ldapAuthType = this.GetLdapAuthenticationTypes(ActiveDirectoryConnectionProtection.Ssl, CredentialsType.NonWindows);
					try
					{
						this.rootdse = new DirectoryEntry(this.GetADsPath("rootdse"), this.GetUsername(), this.GetPassword(), this.authenticationType);
						this.rootdse.RefreshCache();
						this.connectionProtection = ActiveDirectoryConnectionProtection.Ssl;
						if (!this.portSpecified)
						{
							this.port = 636;
							this.portSpecified = true;
						}
						goto IL_0282;
					}
					catch (COMException ex3)
					{
						if (ex3.ErrorCode == -2147023570)
						{
							flag2 = true;
						}
						else
						{
							if (ex3.ErrorCode != -2147016646)
							{
								throw;
							}
							flag = true;
						}
						goto IL_0282;
					}
				}
				flag2 = true;
				IL_0282:
				if (flag2)
				{
					this.authenticationType = this.GetAuthenticationTypes(ActiveDirectoryConnectionProtection.Ssl, CredentialsType.Windows);
					this.ldapAuthType = this.GetLdapAuthenticationTypes(ActiveDirectoryConnectionProtection.Ssl, CredentialsType.Windows);
					try
					{
						this.rootdse = new DirectoryEntry(this.GetADsPath("rootdse"), this.GetUsername(), this.GetPassword(), this.authenticationType);
						this.rootdse.RefreshCache();
						this.connectionProtection = ActiveDirectoryConnectionProtection.Ssl;
						if (!this.portSpecified)
						{
							this.port = 636;
							this.portSpecified = true;
						}
					}
					catch (COMException ex4)
					{
						if (ex4.ErrorCode != -2147016646)
						{
							throw;
						}
						flag = true;
					}
				}
				if (!flag)
				{
					goto IL_0405;
				}
				this.authenticationType = this.GetAuthenticationTypes(ActiveDirectoryConnectionProtection.SignAndSeal, CredentialsType.Windows);
				this.ldapAuthType = this.GetLdapAuthenticationTypes(ActiveDirectoryConnectionProtection.SignAndSeal, CredentialsType.Windows);
				try
				{
					this.rootdse = new DirectoryEntry(this.GetADsPath("rootdse"), this.GetUsername(), this.GetPassword(), this.authenticationType);
					this.rootdse.RefreshCache();
					this.connectionProtection = ActiveDirectoryConnectionProtection.SignAndSeal;
					goto IL_0405;
				}
				catch (COMException ex5)
				{
					throw new ProviderException(SR.GetString("ADMembership_Secure_connection_not_established", new object[] { ex5.Message }), ex5);
				}
			}
			if (this.IsDefaultCredential())
			{
				throw new NotSupportedException(SR.GetString("ADMembership_Default_Creds_not_supported"));
			}
			this.authenticationType = this.GetAuthenticationTypes(this.connectionProtection, CredentialsType.NonWindows);
			this.ldapAuthType = this.GetLdapAuthenticationTypes(this.connectionProtection, CredentialsType.NonWindows);
			this.rootdse = new DirectoryEntry(this.GetADsPath("rootdse"), this.GetUsername(), this.GetPassword(), this.authenticationType);
			IL_0405:
			if (this.rootdse == null)
			{
				this.rootdse = new DirectoryEntry(this.GetADsPath("RootDSE"), this.GetUsername(), this.GetPassword(), this.authenticationType);
			}
			this.directoryType = this.GetDirectoryType();
			if (this.directoryType == DirectoryType.ADAM && this.connectionProtection == ActiveDirectoryConnectionProtection.SignAndSeal)
			{
				throw new ProviderException(SR.GetString("ADMembership_Ssl_connection_not_established"));
			}
			if (this.directoryType == DirectoryType.AD && (this.port == 3268 || this.port == 3269))
			{
				throw new ProviderException(SR.GetString("ADMembership_GCPortsNotSupported"));
			}
			if (string.IsNullOrEmpty(this.containerDN))
			{
				if (this.directoryType == DirectoryType.AD)
				{
					this.containerDN = (string)this.rootdse.Properties["defaultNamingContext"].Value;
					if (this.containerDN == null)
					{
						throw new ProviderException(SR.GetString("ADMembership_DefContainer_not_specified"));
					}
					string adsPath = this.GetADsPath("<WKGUID=a9d1ca15768811d1aded00c04fd8d5cd," + this.containerDN + ">");
					DirectoryEntry directoryEntry = new DirectoryEntry(adsPath, this.GetUsername(), this.GetPassword(), this.authenticationType);
					try
					{
						this.creationContainerDN = (string)PropertyManager.GetPropertyValue(directoryEntry, "distinguishedName");
						goto IL_05DE;
					}
					catch (COMException ex6)
					{
						if (ex6.ErrorCode == -2147016656)
						{
							throw new ProviderException(SR.GetString("ADMembership_DefContainer_does_not_exist"));
						}
						throw;
					}
				}
				throw new ProviderException(SR.GetString("ADMembership_Container_must_be_specified"));
			}
			DirectoryEntry directoryEntry2 = new DirectoryEntry(this.GetADsPath(this.containerDN), this.GetUsername(), this.GetPassword(), this.authenticationType);
			try
			{
				this.creationContainerDN = (this.containerDN = (string)PropertyManager.GetPropertyValue(directoryEntry2, "distinguishedName"));
			}
			catch (COMException ex7)
			{
				if (ex7.ErrorCode == -2147016656)
				{
					throw new ProviderException(SR.GetString("ADMembership_Container_does_not_exist"));
				}
				throw;
			}
			IL_05DE:
			LdapConnection ldapConnection = new LdapConnection(new LdapDirectoryIdentifier(this.serverName + ":" + this.port), DirectoryInformation.GetCredentialsWithDomain(credentials), this.ldapAuthType);
			ldapConnection.SessionOptions.ProtocolVersion = 3;
			try
			{
				ldapConnection.SessionOptions.ReferralChasing = ReferralChasingOptions.None;
				this.SetSessionOptionsForSecureConnection(ldapConnection, false);
				ldapConnection.Bind();
				SearchRequest searchRequest = new SearchRequest();
				searchRequest.DistinguishedName = this.containerDN;
				searchRequest.Filter = "(objectClass=*)";
				searchRequest.Scope = global::System.DirectoryServices.Protocols.SearchScope.Base;
				searchRequest.Attributes.Add("distinguishedName");
				searchRequest.Attributes.Add("objectClass");
				if (this.ServerSearchTimeout != -1)
				{
					searchRequest.TimeLimit = new TimeSpan(0, this.ServerSearchTimeout, 0);
				}
				SearchResponse searchResponse;
				try
				{
					searchResponse = (SearchResponse)ldapConnection.SendRequest(searchRequest);
					if (searchResponse.ResultCode == ResultCode.Referral || searchResponse.ResultCode == ResultCode.NoSuchObject)
					{
						throw new ProviderException(SR.GetString("ADMembership_Container_does_not_exist"));
					}
					if (searchResponse.ResultCode != ResultCode.Success)
					{
						throw new ProviderException(searchResponse.ErrorMessage);
					}
				}
				catch (DirectoryOperationException ex8)
				{
					SearchResponse searchResponse2 = (SearchResponse)ex8.Response;
					if (searchResponse2.ResultCode == ResultCode.NoSuchObject)
					{
						throw new ProviderException(SR.GetString("ADMembership_Container_does_not_exist"));
					}
					throw;
				}
				DirectoryAttribute directoryAttribute = searchResponse.Entries[0].Attributes["objectClass"];
				if (!this.ContainerIsSuperiorOfUser(directoryAttribute))
				{
					throw new ProviderException(SR.GetString("ADMembership_Container_not_superior"));
				}
				if (this.connectionProtection == ActiveDirectoryConnectionProtection.None || this.connectionProtection == ActiveDirectoryConnectionProtection.Ssl)
				{
					this.concurrentBindSupported = this.IsConcurrentBindSupported(ldapConnection);
				}
			}
			finally
			{
				ldapConnection.Dispose();
			}
			if (this.directoryType == DirectoryType.ADAM)
			{
				this.adamPartitionDN = this.GetADAMPartitionFromContainer();
				return;
			}
			if (enablePasswordReset)
			{
				DirectoryEntry directoryEntry3 = new DirectoryEntry(this.GetADsPath((string)PropertyManager.GetPropertyValue(this.rootdse, "defaultNamingContext")), this.GetUsername(), this.GetPassword(), this.AuthenticationTypes);
				NativeComInterfaces.IAdsLargeInteger adsLargeInteger = (NativeComInterfaces.IAdsLargeInteger)PropertyManager.GetPropertyValue(directoryEntry3, "lockoutDuration");
				long num2 = adsLargeInteger.HighPart * 4294967296L + (long)((ulong)((uint)adsLargeInteger.LowPart));
				this.adLockoutDuration = new TimeSpan(-num2);
			}
		}

		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x0600272A RID: 10026 RVA: 0x000ABBF8 File Offset: 0x000AABF8
		internal bool ConcurrentBindSupported
		{
			get
			{
				return this.concurrentBindSupported;
			}
		}

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x0600272B RID: 10027 RVA: 0x000ABC00 File Offset: 0x000AAC00
		internal string ContainerDN
		{
			get
			{
				return this.containerDN;
			}
		}

		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x0600272C RID: 10028 RVA: 0x000ABC08 File Offset: 0x000AAC08
		internal string CreationContainerDN
		{
			get
			{
				return this.creationContainerDN;
			}
		}

		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x0600272D RID: 10029 RVA: 0x000ABC10 File Offset: 0x000AAC10
		internal int Port
		{
			get
			{
				return this.port;
			}
		}

		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x0600272E RID: 10030 RVA: 0x000ABC18 File Offset: 0x000AAC18
		internal bool PortSpecified
		{
			get
			{
				return this.portSpecified;
			}
		}

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x0600272F RID: 10031 RVA: 0x000ABC20 File Offset: 0x000AAC20
		internal DirectoryType DirectoryType
		{
			get
			{
				return this.directoryType;
			}
		}

		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x06002730 RID: 10032 RVA: 0x000ABC28 File Offset: 0x000AAC28
		internal ActiveDirectoryConnectionProtection ConnectionProtection
		{
			get
			{
				return this.connectionProtection;
			}
		}

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x06002731 RID: 10033 RVA: 0x000ABC30 File Offset: 0x000AAC30
		internal AuthenticationTypes AuthenticationTypes
		{
			get
			{
				return this.authenticationType;
			}
		}

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x06002732 RID: 10034 RVA: 0x000ABC38 File Offset: 0x000AAC38
		internal int ClientSearchTimeout
		{
			get
			{
				return this.clientSearchTimeout;
			}
		}

		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x06002733 RID: 10035 RVA: 0x000ABC40 File Offset: 0x000AAC40
		internal int ServerSearchTimeout
		{
			get
			{
				return this.serverSearchTimeout;
			}
		}

		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x06002734 RID: 10036 RVA: 0x000ABC48 File Offset: 0x000AAC48
		internal string ADAMPartitionDN
		{
			get
			{
				return this.adamPartitionDN;
			}
		}

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x06002735 RID: 10037 RVA: 0x000ABC50 File Offset: 0x000AAC50
		internal TimeSpan ADLockoutDuration
		{
			get
			{
				return this.adLockoutDuration;
			}
		}

		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x06002736 RID: 10038 RVA: 0x000ABC58 File Offset: 0x000AAC58
		internal string ForestName
		{
			get
			{
				return this.forestName;
			}
		}

		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x06002737 RID: 10039 RVA: 0x000ABC60 File Offset: 0x000AAC60
		internal string DomainName
		{
			get
			{
				return this.domainName;
			}
		}

		// Token: 0x06002738 RID: 10040 RVA: 0x000ABC68 File Offset: 0x000AAC68
		internal void InitializeDomainAndForestName()
		{
			if (!this.isServer)
			{
				DirectoryContext directoryContext = new DirectoryContext(DirectoryContextType.Domain, this.serverName, this.GetUsername(), this.GetPassword());
				try
				{
					Domain domain = Domain.GetDomain(directoryContext);
					this.domainName = this.GetNetbiosDomainNameIfAvailable(domain.Name);
					this.forestName = domain.Forest.Name;
				}
				catch (ActiveDirectoryObjectNotFoundException)
				{
					this.isServer = true;
				}
			}
			if (this.isServer)
			{
				DirectoryContext directoryContext2 = new DirectoryContext(DirectoryContextType.DirectoryServer, this.serverName, this.GetUsername(), this.GetPassword());
				try
				{
					Domain domain2 = Domain.GetDomain(directoryContext2);
					this.domainName = this.GetNetbiosDomainNameIfAvailable(domain2.Name);
					this.forestName = domain2.Forest.Name;
				}
				catch (ActiveDirectoryObjectNotFoundException)
				{
					throw new ProviderException(SR.GetString("ADMembership_unable_to_contact_domain"));
				}
			}
		}

		// Token: 0x06002739 RID: 10041 RVA: 0x000ABD48 File Offset: 0x000AAD48
		internal void SelectServer()
		{
			this.serverName = this.GetPdcIfDomain(this.serverName);
			this.isServer = true;
		}

		// Token: 0x0600273A RID: 10042 RVA: 0x000ABD64 File Offset: 0x000AAD64
		internal LdapConnection CreateNewLdapConnection(AuthType authType)
		{
			LdapConnection ldapConnection = new LdapConnection(new LdapDirectoryIdentifier(this.serverName + ":" + this.port));
			ldapConnection.AuthType = authType;
			ldapConnection.SessionOptions.ProtocolVersion = 3;
			this.SetSessionOptionsForSecureConnection(ldapConnection, true);
			return ldapConnection;
		}

		// Token: 0x0600273B RID: 10043 RVA: 0x000ABDB8 File Offset: 0x000AADB8
		internal string GetADsPath(string dn)
		{
			string text = "LDAP://" + this.serverName;
			if (this.portSpecified)
			{
				text = text + ":" + this.port;
			}
			NativeComInterfaces.IAdsPathname adsPathname = (NativeComInterfaces.IAdsPathname)new NativeComInterfaces.Pathname();
			adsPathname.Set(dn, 4);
			adsPathname.EscapedMode = 2;
			return text + "/" + adsPathname.Retrieve(7);
		}

		// Token: 0x0600273C RID: 10044 RVA: 0x000ABE28 File Offset: 0x000AAE28
		internal void SetSessionOptionsForSecureConnection(LdapConnection connection, bool useConcurrentBind)
		{
			if (this.connectionProtection == ActiveDirectoryConnectionProtection.Ssl)
			{
				connection.SessionOptions.SecureSocketLayer = true;
			}
			else if (this.connectionProtection == ActiveDirectoryConnectionProtection.SignAndSeal)
			{
				connection.SessionOptions.Signing = true;
				connection.SessionOptions.Sealing = true;
			}
			if (useConcurrentBind && this.concurrentBindSupported)
			{
				try
				{
					connection.SessionOptions.FastConcurrentBind();
				}
				catch (PlatformNotSupportedException)
				{
					this.concurrentBindSupported = false;
				}
				catch (DirectoryOperationException)
				{
					this.concurrentBindSupported = false;
				}
			}
		}

		// Token: 0x0600273D RID: 10045 RVA: 0x000ABEB8 File Offset: 0x000AAEB8
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[EnvironmentPermission(SecurityAction.Assert, Read = "USERNAME")]
		internal string GetUsername()
		{
			if (this.credentials == null)
			{
				return null;
			}
			if (this.credentials.UserName == null)
			{
				return null;
			}
			if (this.credentials.UserName.Length == 0 && (this.credentials.Password == null || this.credentials.Password.Length == 0))
			{
				return null;
			}
			return this.credentials.UserName;
		}

		// Token: 0x0600273E RID: 10046 RVA: 0x000ABF1C File Offset: 0x000AAF1C
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[EnvironmentPermission(SecurityAction.Assert, Read = "USERNAME")]
		internal string GetPassword()
		{
			if (this.credentials == null)
			{
				return null;
			}
			if (this.credentials.Password == null)
			{
				return null;
			}
			if (this.credentials.Password.Length == 0 && (this.credentials.UserName == null || this.credentials.UserName.Length == 0))
			{
				return null;
			}
			return this.credentials.Password;
		}

		// Token: 0x0600273F RID: 10047 RVA: 0x000ABF80 File Offset: 0x000AAF80
		internal AuthenticationTypes GetAuthenticationTypes(ActiveDirectoryConnectionProtection connectionProtection, CredentialsType type)
		{
			return this.authTypes[(int)connectionProtection, (int)type];
		}

		// Token: 0x06002740 RID: 10048 RVA: 0x000ABF8F File Offset: 0x000AAF8F
		internal AuthType GetLdapAuthenticationTypes(ActiveDirectoryConnectionProtection connectionProtection, CredentialsType type)
		{
			return this.ldapAuthTypes[(int)connectionProtection, (int)type];
		}

		// Token: 0x06002741 RID: 10049 RVA: 0x000ABFA0 File Offset: 0x000AAFA0
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[EnvironmentPermission(SecurityAction.Assert, Read = "USERNAME")]
		internal bool IsDefaultCredential()
		{
			return (this.credentials.UserName == null || this.credentials.UserName.Length == 0) && (this.credentials.Password == null || this.credentials.Password.Length == 0);
		}

		// Token: 0x06002742 RID: 10050 RVA: 0x000ABFF0 File Offset: 0x000AAFF0
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[EnvironmentPermission(SecurityAction.Assert, Read = "USERNAME")]
		internal static NetworkCredential GetCredentialsWithDomain(NetworkCredential credentials)
		{
			NetworkCredential networkCredential;
			if (credentials == null)
			{
				networkCredential = new NetworkCredential(null, null);
			}
			else
			{
				string userName = credentials.UserName;
				string text = null;
				string text2 = null;
				string text3 = null;
				if (!string.IsNullOrEmpty(userName))
				{
					int num = userName.IndexOf('\\');
					if (num != -1)
					{
						text3 = userName.Substring(0, num);
						text = userName.Substring(num + 1);
					}
					else
					{
						text = userName;
					}
					text2 = credentials.Password;
				}
				networkCredential = new NetworkCredential(text, text2, text3);
			}
			return networkCredential;
		}

		// Token: 0x06002743 RID: 10051 RVA: 0x000AC05C File Offset: 0x000AB05C
		private bool IsConcurrentBindSupported(LdapConnection ldapConnection)
		{
			bool flag = false;
			SearchRequest searchRequest = new SearchRequest();
			searchRequest.Scope = global::System.DirectoryServices.Protocols.SearchScope.Base;
			searchRequest.Attributes.Add("supportedExtension");
			if (this.ServerSearchTimeout != -1)
			{
				searchRequest.TimeLimit = new TimeSpan(0, this.ServerSearchTimeout, 0);
			}
			SearchResponse searchResponse = (SearchResponse)ldapConnection.SendRequest(searchRequest);
			if (searchResponse.ResultCode != ResultCode.Success)
			{
				throw new ProviderException(searchResponse.ErrorMessage);
			}
			foreach (string text in searchResponse.Entries[0].Attributes["supportedExtension"].GetValues(typeof(string)))
			{
				if (StringUtil.EqualsIgnoreCase(text, "1.2.840.113556.1.4.1781"))
				{
					flag = true;
					break;
				}
			}
			return flag;
		}

		// Token: 0x06002744 RID: 10052 RVA: 0x000AC124 File Offset: 0x000AB124
		private string GetADAMPartitionFromContainer()
		{
			string text = null;
			int num = int.MaxValue;
			foreach (object obj in this.rootdse.Properties["namingContexts"])
			{
				string text2 = (string)obj;
				bool flag = this.containerDN.EndsWith(text2, StringComparison.Ordinal);
				int num2 = this.containerDN.LastIndexOf(text2, StringComparison.Ordinal);
				if (flag && num2 != -1 && num2 < num)
				{
					text = text2;
					num = num2;
				}
			}
			if (text == null)
			{
				throw new ProviderException(SR.GetString("ADMembership_No_ADAM_Partition"));
			}
			return text;
		}

		// Token: 0x06002745 RID: 10053 RVA: 0x000AC1D8 File Offset: 0x000AB1D8
		private bool ContainerIsSuperiorOfUser(DirectoryAttribute objectClass)
		{
			ArrayList arrayList = new ArrayList();
			DirectoryEntry directoryEntry = new DirectoryEntry(this.GetADsPath("schema") + "/user", this.GetUsername(), this.GetPassword(), this.AuthenticationTypes);
			ArrayList arrayList2 = new ArrayList();
			bool flag = false;
			object obj = null;
			try
			{
				obj = directoryEntry.InvokeGet("DerivedFrom");
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode != -2147463155)
				{
					throw;
				}
				flag = true;
			}
			if (!flag)
			{
				if (obj is ICollection)
				{
					arrayList2.AddRange((ICollection)obj);
				}
				else
				{
					arrayList2.Add((string)obj);
				}
			}
			arrayList2.Add("user");
			DirectoryEntry directoryEntry2 = new DirectoryEntry(this.GetADsPath((string)this.rootdse.Properties["schemaNamingContext"].Value), this.GetUsername(), this.GetPassword(), this.AuthenticationTypes);
			DirectorySearcher directorySearcher = new DirectorySearcher(directoryEntry2);
			directorySearcher.Filter = "(&(objectClass=classSchema)(|";
			foreach (object obj2 in arrayList2)
			{
				string text = (string)obj2;
				DirectorySearcher directorySearcher2 = directorySearcher;
				directorySearcher2.Filter = directorySearcher2.Filter + "(ldapDisplayName=" + text + ")";
			}
			DirectorySearcher directorySearcher3 = directorySearcher;
			directorySearcher3.Filter += "))";
			directorySearcher.SearchScope = global::System.DirectoryServices.SearchScope.OneLevel;
			directorySearcher.PropertiesToLoad.Add("possSuperiors");
			directorySearcher.PropertiesToLoad.Add("systemPossSuperiors");
			SearchResultCollection searchResultCollection = directorySearcher.FindAll();
			try
			{
				foreach (object obj3 in searchResultCollection)
				{
					SearchResult searchResult = (SearchResult)obj3;
					arrayList.AddRange(searchResult.Properties["possSuperiors"]);
					arrayList.AddRange(searchResult.Properties["systemPossSuperiors"]);
				}
			}
			finally
			{
				searchResultCollection.Dispose();
			}
			foreach (string text2 in objectClass.GetValues(typeof(string)))
			{
				if (arrayList.Contains(text2))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002746 RID: 10054 RVA: 0x000AC460 File Offset: 0x000AB460
		private DirectoryType GetDirectoryType()
		{
			DirectoryType directoryType = DirectoryType.Unknown;
			foreach (object obj in this.rootdse.Properties["supportedCapabilities"])
			{
				string text = (string)obj;
				if (StringUtil.EqualsIgnoreCase(text, "1.2.840.113556.1.4.1851"))
				{
					directoryType = DirectoryType.ADAM;
					break;
				}
				if (StringUtil.EqualsIgnoreCase(text, "1.2.840.113556.1.4.800"))
				{
					directoryType = DirectoryType.AD;
					break;
				}
			}
			if (directoryType == DirectoryType.Unknown)
			{
				throw new ProviderException(SR.GetString("ADMembership_Valid_Targets"));
			}
			return directoryType;
		}

		// Token: 0x06002747 RID: 10055 RVA: 0x000AC4FC File Offset: 0x000AB4FC
		internal string GetPdcIfDomain(string name)
		{
			IntPtr zero = IntPtr.Zero;
			uint num = 1073741968U;
			string text = null;
			int num2 = 1355;
			int num3 = NativeMethods.DsGetDcName(null, name, IntPtr.Zero, null, num, out zero);
			try
			{
				if (num3 == 0)
				{
					DomainControllerInfo domainControllerInfo = new DomainControllerInfo();
					Marshal.PtrToStructure(zero, domainControllerInfo);
					text = domainControllerInfo.DomainControllerName.Substring(2);
				}
				else
				{
					if (num3 != num2)
					{
						throw new ProviderException(DirectoryInformation.GetErrorMessage(num3));
					}
					text = name;
				}
			}
			finally
			{
				if (zero != IntPtr.Zero)
				{
					NativeMethods.NetApiBufferFree(zero);
				}
			}
			return text;
		}

		// Token: 0x06002748 RID: 10056 RVA: 0x000AC590 File Offset: 0x000AB590
		internal string GetNetbiosDomainNameIfAvailable(string dnsDomainName)
		{
			DirectoryEntry directoryEntry = new DirectoryEntry(this.GetADsPath("CN=Partitions," + (string)PropertyManager.GetPropertyValue(this.rootdse, "configurationNamingContext")), this.GetUsername(), this.GetPassword());
			DirectorySearcher directorySearcher = new DirectorySearcher(directoryEntry);
			directorySearcher.SearchScope = global::System.DirectoryServices.SearchScope.OneLevel;
			StringBuilder stringBuilder = new StringBuilder(15);
			stringBuilder.Append("(&(objectCategory=crossRef)(dnsRoot=");
			stringBuilder.Append(dnsDomainName);
			stringBuilder.Append(")(systemFlags:1.2.840.113556.1.4.804:=1)(systemFlags:1.2.840.113556.1.4.804:=2))");
			directorySearcher.Filter = stringBuilder.ToString();
			directorySearcher.PropertiesToLoad.Add("nETBIOSName");
			SearchResult searchResult = directorySearcher.FindOne();
			string text;
			if (searchResult == null || !searchResult.Properties.Contains("nETBIOSName"))
			{
				text = dnsDomainName;
			}
			else
			{
				text = (string)PropertyManager.GetSearchResultPropertyValue(searchResult, "nETBIOSName");
			}
			return text;
		}

		// Token: 0x06002749 RID: 10057 RVA: 0x000AC660 File Offset: 0x000AB660
		private static string GetErrorMessage(int errorCode)
		{
			uint num = (uint)((errorCode & 65535) | 458752 | int.MinValue);
			string text = string.Empty;
			StringBuilder stringBuilder = new StringBuilder(256);
			int num2 = NativeMethods.FormatMessageW(12800, 0, (int)num, 0, stringBuilder, stringBuilder.Capacity + 1, 0);
			if (num2 != 0)
			{
				text = stringBuilder.ToString(0, num2);
			}
			else
			{
				text = SR.GetString("ADMembership_Unknown_Error", new object[] { string.Format(CultureInfo.InvariantCulture, "{0}", new object[] { errorCode }) });
			}
			return text;
		}

		// Token: 0x04001E0D RID: 7693
		private const string LDAP_CAP_ACTIVE_DIRECTORY_ADAM_OID = "1.2.840.113556.1.4.1851";

		// Token: 0x04001E0E RID: 7694
		private const string LDAP_CAP_ACTIVE_DIRECTORY_OID = "1.2.840.113556.1.4.800";

		// Token: 0x04001E0F RID: 7695
		private const string LDAP_SERVER_FAST_BIND_OID = "1.2.840.113556.1.4.1781";

		// Token: 0x04001E10 RID: 7696
		internal const int SSL_PORT = 636;

		// Token: 0x04001E11 RID: 7697
		private const int GC_PORT = 3268;

		// Token: 0x04001E12 RID: 7698
		private const int GC_SSL_PORT = 3269;

		// Token: 0x04001E13 RID: 7699
		private const string GUID_USERS_CONTAINER_W = "a9d1ca15768811d1aded00c04fd8d5cd";

		// Token: 0x04001E14 RID: 7700
		private string serverName;

		// Token: 0x04001E15 RID: 7701
		private string containerDN;

		// Token: 0x04001E16 RID: 7702
		private string creationContainerDN;

		// Token: 0x04001E17 RID: 7703
		private string adspath;

		// Token: 0x04001E18 RID: 7704
		private int port = 389;

		// Token: 0x04001E19 RID: 7705
		private bool portSpecified;

		// Token: 0x04001E1A RID: 7706
		private DirectoryType directoryType = DirectoryType.Unknown;

		// Token: 0x04001E1B RID: 7707
		private ActiveDirectoryConnectionProtection connectionProtection;

		// Token: 0x04001E1C RID: 7708
		private bool concurrentBindSupported;

		// Token: 0x04001E1D RID: 7709
		private int clientSearchTimeout = -1;

		// Token: 0x04001E1E RID: 7710
		private int serverSearchTimeout = -1;

		// Token: 0x04001E1F RID: 7711
		private DirectoryEntry rootdse;

		// Token: 0x04001E20 RID: 7712
		private NetworkCredential credentials;

		// Token: 0x04001E21 RID: 7713
		private AuthenticationTypes authenticationType;

		// Token: 0x04001E22 RID: 7714
		private AuthType ldapAuthType = AuthType.Basic;

		// Token: 0x04001E23 RID: 7715
		private string adamPartitionDN;

		// Token: 0x04001E24 RID: 7716
		private TimeSpan adLockoutDuration;

		// Token: 0x04001E25 RID: 7717
		private string forestName;

		// Token: 0x04001E26 RID: 7718
		private string domainName;

		// Token: 0x04001E27 RID: 7719
		private bool isServer;

		// Token: 0x04001E28 RID: 7720
		private AuthenticationTypes[,] authTypes;

		// Token: 0x04001E29 RID: 7721
		private AuthType[,] ldapAuthTypes;
	}
}
