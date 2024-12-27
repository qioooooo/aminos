using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Web.Hosting;
using System.Web.Util;

namespace System.Web.Security
{
	// Token: 0x02000352 RID: 850
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public class RolePrincipal : IPrincipal, ISerializable
	{
		// Token: 0x0600292F RID: 10543 RVA: 0x000B45C8 File Offset: 0x000B35C8
		public RolePrincipal(IIdentity identity, string encryptedTicket)
		{
			if (identity == null)
			{
				throw new ArgumentNullException("identity");
			}
			if (encryptedTicket == null)
			{
				throw new ArgumentNullException("encryptedTicket");
			}
			this._Identity = identity;
			this._ProviderName = Roles.Provider.Name;
			if (identity.IsAuthenticated)
			{
				this.InitFromEncryptedTicket(encryptedTicket);
				return;
			}
			this.Init();
		}

		// Token: 0x06002930 RID: 10544 RVA: 0x000B4624 File Offset: 0x000B3624
		public RolePrincipal(IIdentity identity)
		{
			if (identity == null)
			{
				throw new ArgumentNullException("identity");
			}
			this._Identity = identity;
			this.Init();
		}

		// Token: 0x06002931 RID: 10545 RVA: 0x000B4648 File Offset: 0x000B3648
		public RolePrincipal(string providerName, IIdentity identity)
		{
			if (identity == null)
			{
				throw new ArgumentNullException("identity");
			}
			if (providerName == null)
			{
				throw new ArgumentException(SR.GetString("Role_provider_name_invalid"), "providerName");
			}
			this._ProviderName = providerName;
			if (Roles.Providers[providerName] == null)
			{
				throw new ArgumentException(SR.GetString("Role_provider_name_invalid"), "providerName");
			}
			this._Identity = identity;
			this.Init();
		}

		// Token: 0x06002932 RID: 10546 RVA: 0x000B46B8 File Offset: 0x000B36B8
		public RolePrincipal(string providerName, IIdentity identity, string encryptedTicket)
		{
			if (identity == null)
			{
				throw new ArgumentNullException("identity");
			}
			if (encryptedTicket == null)
			{
				throw new ArgumentNullException("encryptedTicket");
			}
			if (providerName == null)
			{
				throw new ArgumentException(SR.GetString("Role_provider_name_invalid"), "providerName");
			}
			this._ProviderName = providerName;
			if (Roles.Providers[this._ProviderName] == null)
			{
				throw new ArgumentException(SR.GetString("Role_provider_name_invalid"), "providerName");
			}
			this._Identity = identity;
			if (identity.IsAuthenticated)
			{
				this.InitFromEncryptedTicket(encryptedTicket);
				return;
			}
			this.Init();
		}

		// Token: 0x06002933 RID: 10547 RVA: 0x000B474C File Offset: 0x000B374C
		private void InitFromEncryptedTicket(string encryptedTicket)
		{
			if (HostingEnvironment.IsHosted && EtwTrace.IsTraceEnabled(4, 8))
			{
				EtwTrace.Trace(EtwTraceType.ETW_TYPE_ROLE_BEGIN, HttpContext.Current.WorkerRequest);
			}
			if (!string.IsNullOrEmpty(encryptedTicket))
			{
				byte[] array = CookieProtectionHelper.Decode(Roles.CookieProtectionValue, encryptedTicket);
				if (array != null)
				{
					RolePrincipal rolePrincipal = null;
					MemoryStream memoryStream = null;
					try
					{
						memoryStream = new MemoryStream(array);
						rolePrincipal = new BinaryFormatter().Deserialize(memoryStream) as RolePrincipal;
					}
					catch
					{
					}
					finally
					{
						memoryStream.Close();
					}
					if (rolePrincipal != null && StringUtil.EqualsIgnoreCase(rolePrincipal._Username, this._Identity.Name) && StringUtil.EqualsIgnoreCase(rolePrincipal._ProviderName, this._ProviderName) && !(DateTime.UtcNow > rolePrincipal._ExpireDate))
					{
						this._Version = rolePrincipal._Version;
						this._ExpireDate = rolePrincipal._ExpireDate;
						this._IssueDate = rolePrincipal._IssueDate;
						this._IsRoleListCached = rolePrincipal._IsRoleListCached;
						this._CachedListChanged = false;
						this._Username = rolePrincipal._Username;
						this._Roles = rolePrincipal._Roles;
						this.RenewIfOld();
						if (HostingEnvironment.IsHosted && EtwTrace.IsTraceEnabled(4, 8))
						{
							EtwTrace.Trace(EtwTraceType.ETW_TYPE_ROLE_END, HttpContext.Current.WorkerRequest, "RolePrincipal", this._Identity.Name);
						}
						return;
					}
				}
			}
			this.Init();
			this._CachedListChanged = true;
			if (HostingEnvironment.IsHosted && EtwTrace.IsTraceEnabled(4, 8))
			{
				EtwTrace.Trace(EtwTraceType.ETW_TYPE_ROLE_END, HttpContext.Current.WorkerRequest, "RolePrincipal", this._Identity.Name);
			}
		}

		// Token: 0x06002934 RID: 10548 RVA: 0x000B48F0 File Offset: 0x000B38F0
		private void Init()
		{
			this._Version = 1;
			this._IssueDate = DateTime.UtcNow;
			this._ExpireDate = DateTime.UtcNow.AddMinutes((double)Roles.CookieTimeout);
			this._IsRoleListCached = false;
			this._CachedListChanged = false;
			if (this._ProviderName == null)
			{
				this._ProviderName = Roles.Provider.Name;
			}
			if (this._Roles == null)
			{
				this._Roles = new HybridDictionary(true);
			}
			if (this._Identity != null)
			{
				this._Username = this._Identity.Name;
			}
		}

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x06002935 RID: 10549 RVA: 0x000B497B File Offset: 0x000B397B
		public int Version
		{
			get
			{
				return this._Version;
			}
		}

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x06002936 RID: 10550 RVA: 0x000B4983 File Offset: 0x000B3983
		public DateTime ExpireDate
		{
			get
			{
				return this._ExpireDate.ToLocalTime();
			}
		}

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x06002937 RID: 10551 RVA: 0x000B4990 File Offset: 0x000B3990
		public DateTime IssueDate
		{
			get
			{
				return this._IssueDate.ToLocalTime();
			}
		}

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x06002938 RID: 10552 RVA: 0x000B499D File Offset: 0x000B399D
		public bool Expired
		{
			get
			{
				return this._ExpireDate < DateTime.UtcNow;
			}
		}

		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x06002939 RID: 10553 RVA: 0x000B49AF File Offset: 0x000B39AF
		public string CookiePath
		{
			get
			{
				return Roles.CookiePath;
			}
		}

		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x0600293A RID: 10554 RVA: 0x000B49B6 File Offset: 0x000B39B6
		public IIdentity Identity
		{
			get
			{
				return this._Identity;
			}
		}

		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x0600293B RID: 10555 RVA: 0x000B49BE File Offset: 0x000B39BE
		public bool IsRoleListCached
		{
			get
			{
				return this._IsRoleListCached;
			}
		}

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x0600293C RID: 10556 RVA: 0x000B49C6 File Offset: 0x000B39C6
		public bool CachedListChanged
		{
			get
			{
				return this._CachedListChanged;
			}
		}

		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x0600293D RID: 10557 RVA: 0x000B49CE File Offset: 0x000B39CE
		public string ProviderName
		{
			get
			{
				return this._ProviderName;
			}
		}

		// Token: 0x0600293E RID: 10558 RVA: 0x000B49D8 File Offset: 0x000B39D8
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public string ToEncryptedTicket()
		{
			if (!Roles.Enabled)
			{
				return null;
			}
			if (this._Identity != null && !this._Identity.IsAuthenticated)
			{
				return null;
			}
			if (this._Identity == null && string.IsNullOrEmpty(this._Username))
			{
				return null;
			}
			if (this._Roles.Count > Roles.MaxCachedResults)
			{
				return null;
			}
			MemoryStream memoryStream = new MemoryStream();
			byte[] array = null;
			IIdentity identity = this._Identity;
			try
			{
				this._Identity = null;
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(memoryStream, this);
				array = memoryStream.ToArray();
			}
			finally
			{
				memoryStream.Close();
				this._Identity = identity;
			}
			return CookieProtectionHelper.Encode(Roles.CookieProtectionValue, array, array.Length);
		}

		// Token: 0x0600293F RID: 10559 RVA: 0x000B4A8C File Offset: 0x000B3A8C
		private void RenewIfOld()
		{
			if (!Roles.CookieSlidingExpiration)
			{
				return;
			}
			DateTime utcNow = DateTime.UtcNow;
			TimeSpan timeSpan = utcNow - this._IssueDate;
			TimeSpan timeSpan2 = this._ExpireDate - utcNow;
			if (timeSpan2 > timeSpan)
			{
				return;
			}
			this._ExpireDate = utcNow + (this._ExpireDate - this._IssueDate);
			this._IssueDate = utcNow;
			this._CachedListChanged = true;
		}

		// Token: 0x06002940 RID: 10560 RVA: 0x000B4AF8 File Offset: 0x000B3AF8
		public string[] GetRoles()
		{
			if (this._Identity == null)
			{
				throw new ProviderException(SR.GetString("Role_Principal_not_fully_constructed"));
			}
			if (!this._Identity.IsAuthenticated)
			{
				return new string[0];
			}
			string[] array;
			if (!this._IsRoleListCached || !this._GetRolesCalled)
			{
				this._Roles.Clear();
				array = Roles.Providers[this._ProviderName].GetRolesForUser(this.Identity.Name);
				foreach (string text in array)
				{
					if (this._Roles[text] == null)
					{
						this._Roles.Add(text, string.Empty);
					}
				}
				this._IsRoleListCached = true;
				this._CachedListChanged = true;
				this._GetRolesCalled = true;
				return array;
			}
			array = new string[this._Roles.Count];
			int num = 0;
			foreach (object obj in this._Roles.Keys)
			{
				string text2 = (string)obj;
				array[num++] = text2;
			}
			return array;
		}

		// Token: 0x06002941 RID: 10561 RVA: 0x000B4C2C File Offset: 0x000B3C2C
		public bool IsInRole(string role)
		{
			if (this._Identity == null)
			{
				throw new ProviderException(SR.GetString("Role_Principal_not_fully_constructed"));
			}
			if (!this._Identity.IsAuthenticated || role == null)
			{
				return false;
			}
			role = role.Trim();
			if (!this.IsRoleListCached)
			{
				this._Roles.Clear();
				string[] rolesForUser = Roles.Providers[this._ProviderName].GetRolesForUser(this.Identity.Name);
				foreach (string text in rolesForUser)
				{
					if (this._Roles[text] == null)
					{
						this._Roles.Add(text, string.Empty);
					}
				}
				this._IsRoleListCached = true;
				this._CachedListChanged = true;
			}
			return this._Roles[role] != null;
		}

		// Token: 0x06002942 RID: 10562 RVA: 0x000B4CF2 File Offset: 0x000B3CF2
		public void SetDirty()
		{
			this._IsRoleListCached = false;
			this._CachedListChanged = true;
		}

		// Token: 0x06002943 RID: 10563 RVA: 0x000B4D04 File Offset: 0x000B3D04
		private RolePrincipal(SerializationInfo info, StreamingContext context)
		{
			this._Version = info.GetInt32("_Version");
			this._ExpireDate = info.GetDateTime("_ExpireDate");
			this._IssueDate = info.GetDateTime("_IssueDate");
			try
			{
				this._Identity = info.GetValue("_Identity", typeof(IIdentity)) as IIdentity;
			}
			catch
			{
			}
			this._ProviderName = info.GetString("_ProviderName");
			this._Username = info.GetString("_Username");
			this._IsRoleListCached = info.GetBoolean("_IsRoleListCached");
			this._Roles = new HybridDictionary(true);
			string @string = info.GetString("_AllRoles");
			if (@string != null)
			{
				foreach (string text in @string.Split(new char[] { ',' }))
				{
					if (this._Roles[text] == null)
					{
						this._Roles.Add(text, string.Empty);
					}
				}
			}
		}

		// Token: 0x06002944 RID: 10564 RVA: 0x000B4E18 File Offset: 0x000B3E18
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("_Version", this._Version);
			info.AddValue("_ExpireDate", this._ExpireDate);
			info.AddValue("_IssueDate", this._IssueDate);
			if (this._Identity != null)
			{
				try
				{
					info.AddValue("_Identity", this._Identity);
				}
				catch
				{
				}
			}
			info.AddValue("_ProviderName", this._ProviderName);
			info.AddValue("_Username", (this._Identity == null) ? this._Username : this._Identity.Name);
			info.AddValue("_IsRoleListCached", this._IsRoleListCached);
			if (this._Roles.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder(this._Roles.Count * 10);
				foreach (object obj in this._Roles.Keys)
				{
					stringBuilder.Append((string)obj + ",");
				}
				string text = stringBuilder.ToString();
				info.AddValue("_AllRoles", text.Substring(0, text.Length - 1));
				return;
			}
			info.AddValue("_AllRoles", string.Empty);
		}

		// Token: 0x04001EE9 RID: 7913
		private int _Version;

		// Token: 0x04001EEA RID: 7914
		private DateTime _ExpireDate;

		// Token: 0x04001EEB RID: 7915
		private DateTime _IssueDate;

		// Token: 0x04001EEC RID: 7916
		private IIdentity _Identity;

		// Token: 0x04001EED RID: 7917
		private string _ProviderName;

		// Token: 0x04001EEE RID: 7918
		private string _Username;

		// Token: 0x04001EEF RID: 7919
		private bool _IsRoleListCached;

		// Token: 0x04001EF0 RID: 7920
		private bool _CachedListChanged;

		// Token: 0x04001EF1 RID: 7921
		[NonSerialized]
		private HybridDictionary _Roles;

		// Token: 0x04001EF2 RID: 7922
		[NonSerialized]
		private bool _GetRolesCalled;
	}
}
