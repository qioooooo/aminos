using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Net.Configuration;
using System.Reflection;
using System.Security.Authentication.ExtendedProtection;
using System.Threading;

namespace System.Net
{
	// Token: 0x02000377 RID: 887
	public class AuthenticationManager
	{
		// Token: 0x06001BBD RID: 7101 RVA: 0x00068AEA File Offset: 0x00067AEA
		private AuthenticationManager()
		{
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06001BBE RID: 7102 RVA: 0x00068AF2 File Offset: 0x00067AF2
		// (set) Token: 0x06001BBF RID: 7103 RVA: 0x00068AF9 File Offset: 0x00067AF9
		public static ICredentialPolicy CredentialPolicy
		{
			get
			{
				return AuthenticationManager.s_ICredentialPolicy;
			}
			set
			{
				ExceptionHelper.ControlPolicyPermission.Demand();
				AuthenticationManager.s_ICredentialPolicy = value;
			}
		}

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06001BC0 RID: 7104 RVA: 0x00068B0B File Offset: 0x00067B0B
		public static StringDictionary CustomTargetNameDictionary
		{
			get
			{
				return AuthenticationManager.m_SpnDictionary;
			}
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06001BC1 RID: 7105 RVA: 0x00068B12 File Offset: 0x00067B12
		internal static SpnDictionary SpnDictionary
		{
			get
			{
				return AuthenticationManager.m_SpnDictionary;
			}
		}

		// Token: 0x06001BC2 RID: 7106 RVA: 0x00068B1C File Offset: 0x00067B1C
		internal static void EnsureConfigLoaded()
		{
			try
			{
				ArrayList moduleList = AuthenticationManager.ModuleList;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is OutOfMemoryException || ex is StackOverflowException)
				{
					throw;
				}
			}
			catch
			{
			}
		}

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06001BC3 RID: 7107 RVA: 0x00068B70 File Offset: 0x00067B70
		internal static bool OSSupportsExtendedProtection
		{
			get
			{
				if (AuthenticationManager.s_OSSupportsExtendedProtection == TriState.Unspecified)
				{
					if (ComNetOS.IsWin7)
					{
						AuthenticationManager.s_OSSupportsExtendedProtection = TriState.True;
					}
					else if (AuthenticationManager.SspSupportsExtendedProtection)
					{
						if (UnsafeNclNativeMethods.HttpApi.ExtendedProtectionSupported)
						{
							AuthenticationManager.s_OSSupportsExtendedProtection = TriState.True;
						}
						else
						{
							AuthenticationManager.s_OSSupportsExtendedProtection = TriState.False;
						}
					}
					else
					{
						AuthenticationManager.s_OSSupportsExtendedProtection = TriState.False;
					}
				}
				return AuthenticationManager.s_OSSupportsExtendedProtection == TriState.True;
			}
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06001BC4 RID: 7108 RVA: 0x00068BC0 File Offset: 0x00067BC0
		internal static bool SspSupportsExtendedProtection
		{
			get
			{
				if (AuthenticationManager.s_SspSupportsExtendedProtection == TriState.Unspecified)
				{
					if (ComNetOS.IsWin7)
					{
						AuthenticationManager.s_SspSupportsExtendedProtection = TriState.True;
					}
					else
					{
						ContextFlags contextFlags = ContextFlags.ReplayDetect | ContextFlags.SequenceDetect | ContextFlags.Confidentiality | ContextFlags.Connection | ContextFlags.AcceptIntegrity;
						NTAuthentication ntauthentication = new NTAuthentication(false, "NTLM", SystemNetworkCredential.defaultCredential, "http/localhost", contextFlags, null);
						try
						{
							NTAuthentication ntauthentication2 = new NTAuthentication(true, "NTLM", SystemNetworkCredential.defaultCredential, null, ContextFlags.Connection, null);
							try
							{
								byte[] array = null;
								while (!ntauthentication2.IsCompleted)
								{
									SecurityStatus securityStatus;
									array = ntauthentication.GetOutgoingBlob(array, true, out securityStatus);
									array = ntauthentication2.GetOutgoingBlob(array, true, out securityStatus);
								}
								if (ntauthentication2.OSSupportsExtendedProtection)
								{
									AuthenticationManager.s_SspSupportsExtendedProtection = TriState.True;
								}
								else
								{
									if (Logging.On)
									{
										Logging.PrintWarning(Logging.Web, SR.GetString("net_ssp_dont_support_cbt"));
									}
									AuthenticationManager.s_SspSupportsExtendedProtection = TriState.False;
								}
							}
							finally
							{
								ntauthentication2.CloseContext();
							}
						}
						finally
						{
							ntauthentication.CloseContext();
						}
					}
				}
				return AuthenticationManager.s_SspSupportsExtendedProtection == TriState.True;
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06001BC5 RID: 7109 RVA: 0x00068CB0 File Offset: 0x00067CB0
		private static ArrayList ModuleList
		{
			get
			{
				if (AuthenticationManager.s_ModuleList == null)
				{
					lock (AuthenticationManager.s_ModuleBinding)
					{
						if (AuthenticationManager.s_ModuleList == null)
						{
							List<Type> authenticationModules = AuthenticationModulesSectionInternal.GetSection().AuthenticationModules;
							ArrayList arrayList = new ArrayList();
							foreach (Type type in authenticationModules)
							{
								try
								{
									IAuthenticationModule authenticationModule = Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, new object[0], CultureInfo.InvariantCulture) as IAuthenticationModule;
									if (authenticationModule != null)
									{
										AuthenticationManager.RemoveAuthenticationType(arrayList, authenticationModule.AuthenticationType);
										arrayList.Add(authenticationModule);
									}
								}
								catch (Exception)
								{
								}
								catch
								{
								}
							}
							AuthenticationManager.s_ModuleList = arrayList;
						}
					}
				}
				return AuthenticationManager.s_ModuleList;
			}
		}

		// Token: 0x06001BC6 RID: 7110 RVA: 0x00068D9C File Offset: 0x00067D9C
		private static void RemoveAuthenticationType(ArrayList list, string typeToRemove)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (string.Compare(((IAuthenticationModule)list[i]).AuthenticationType, typeToRemove, StringComparison.OrdinalIgnoreCase) == 0)
				{
					list.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x06001BC7 RID: 7111 RVA: 0x00068DDC File Offset: 0x00067DDC
		public static Authorization Authenticate(string challenge, WebRequest request, ICredentials credentials)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}
			if (credentials == null)
			{
				throw new ArgumentNullException("credentials");
			}
			if (challenge == null)
			{
				throw new ArgumentNullException("challenge");
			}
			Authorization authorization = null;
			HttpWebRequest httpWebRequest = request as HttpWebRequest;
			if (httpWebRequest != null && httpWebRequest.CurrentAuthenticationState.Module != null)
			{
				authorization = httpWebRequest.CurrentAuthenticationState.Module.Authenticate(challenge, request, credentials);
			}
			else
			{
				lock (AuthenticationManager.s_ModuleBinding)
				{
					for (int i = 0; i < AuthenticationManager.ModuleList.Count; i++)
					{
						IAuthenticationModule authenticationModule = (IAuthenticationModule)AuthenticationManager.ModuleList[i];
						if (httpWebRequest != null)
						{
							httpWebRequest.CurrentAuthenticationState.Module = authenticationModule;
						}
						authorization = authenticationModule.Authenticate(challenge, request, credentials);
						if (authorization != null)
						{
							break;
						}
					}
				}
			}
			return authorization;
		}

		// Token: 0x06001BC8 RID: 7112 RVA: 0x00068EAC File Offset: 0x00067EAC
		private static bool ModuleRequiresChannelBinding(IAuthenticationModule authenticationModule)
		{
			return authenticationModule is NtlmClient || authenticationModule is KerberosClient || authenticationModule is NegotiateClient || authenticationModule is DigestClient;
		}

		// Token: 0x06001BC9 RID: 7113 RVA: 0x00068ED4 File Offset: 0x00067ED4
		public static Authorization PreAuthenticate(WebRequest request, ICredentials credentials)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}
			if (credentials == null)
			{
				return null;
			}
			HttpWebRequest httpWebRequest = request as HttpWebRequest;
			if (httpWebRequest == null)
			{
				return null;
			}
			string text = AuthenticationManager.s_ModuleBinding.Lookup(httpWebRequest.ChallengedUri.AbsoluteUri) as string;
			if (text == null)
			{
				return null;
			}
			IAuthenticationModule authenticationModule = AuthenticationManager.findModule(text);
			if (authenticationModule == null)
			{
				return null;
			}
			if (httpWebRequest.ChallengedUri.Scheme == Uri.UriSchemeHttps)
			{
				object cachedChannelBinding = httpWebRequest.ServicePoint.CachedChannelBinding;
				ChannelBinding channelBinding = cachedChannelBinding as ChannelBinding;
				if (channelBinding != null)
				{
					httpWebRequest.CurrentAuthenticationState.TransportContext = new CachedTransportContext(channelBinding);
				}
			}
			Authorization authorization = authenticationModule.PreAuthenticate(request, credentials);
			if (authorization != null && !authorization.Complete && httpWebRequest != null)
			{
				httpWebRequest.CurrentAuthenticationState.Module = authenticationModule;
			}
			return authorization;
		}

		// Token: 0x06001BCA RID: 7114 RVA: 0x00068F98 File Offset: 0x00067F98
		public static void Register(IAuthenticationModule authenticationModule)
		{
			ExceptionHelper.UnmanagedPermission.Demand();
			if (authenticationModule == null)
			{
				throw new ArgumentNullException("authenticationModule");
			}
			lock (AuthenticationManager.s_ModuleBinding)
			{
				IAuthenticationModule authenticationModule2 = AuthenticationManager.findModule(authenticationModule.AuthenticationType);
				if (authenticationModule2 != null)
				{
					AuthenticationManager.ModuleList.Remove(authenticationModule2);
				}
				AuthenticationManager.ModuleList.Add(authenticationModule);
			}
		}

		// Token: 0x06001BCB RID: 7115 RVA: 0x00069008 File Offset: 0x00068008
		public static void Unregister(IAuthenticationModule authenticationModule)
		{
			ExceptionHelper.UnmanagedPermission.Demand();
			if (authenticationModule == null)
			{
				throw new ArgumentNullException("authenticationModule");
			}
			lock (AuthenticationManager.s_ModuleBinding)
			{
				if (!AuthenticationManager.ModuleList.Contains(authenticationModule))
				{
					throw new InvalidOperationException(SR.GetString("net_authmodulenotregistered"));
				}
				AuthenticationManager.ModuleList.Remove(authenticationModule);
			}
		}

		// Token: 0x06001BCC RID: 7116 RVA: 0x0006907C File Offset: 0x0006807C
		public static void Unregister(string authenticationScheme)
		{
			ExceptionHelper.UnmanagedPermission.Demand();
			if (authenticationScheme == null)
			{
				throw new ArgumentNullException("authenticationScheme");
			}
			lock (AuthenticationManager.s_ModuleBinding)
			{
				IAuthenticationModule authenticationModule = AuthenticationManager.findModule(authenticationScheme);
				if (authenticationModule == null)
				{
					throw new InvalidOperationException(SR.GetString("net_authschemenotregistered"));
				}
				AuthenticationManager.ModuleList.Remove(authenticationModule);
			}
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06001BCD RID: 7117 RVA: 0x000690EC File Offset: 0x000680EC
		public static IEnumerator RegisteredModules
		{
			get
			{
				return AuthenticationManager.ModuleList.GetEnumerator();
			}
		}

		// Token: 0x06001BCE RID: 7118 RVA: 0x000690F8 File Offset: 0x000680F8
		internal static void BindModule(Uri uri, Authorization response, IAuthenticationModule module)
		{
			if (response.ProtectionRealm != null)
			{
				string[] protectionRealm = response.ProtectionRealm;
				for (int i = 0; i < protectionRealm.Length; i++)
				{
					AuthenticationManager.s_ModuleBinding.Add(protectionRealm[i], module.AuthenticationType);
				}
				return;
			}
			string text = AuthenticationManager.generalize(uri);
			AuthenticationManager.s_ModuleBinding.Add(text, module.AuthenticationType);
		}

		// Token: 0x06001BCF RID: 7119 RVA: 0x00069150 File Offset: 0x00068150
		private static IAuthenticationModule findModule(string authenticationType)
		{
			IAuthenticationModule authenticationModule = null;
			ArrayList moduleList = AuthenticationManager.ModuleList;
			for (int i = 0; i < moduleList.Count; i++)
			{
				IAuthenticationModule authenticationModule2 = (IAuthenticationModule)moduleList[i];
				if (string.Compare(authenticationModule2.AuthenticationType, authenticationType, StringComparison.OrdinalIgnoreCase) == 0)
				{
					authenticationModule = authenticationModule2;
					break;
				}
			}
			return authenticationModule;
		}

		// Token: 0x06001BD0 RID: 7120 RVA: 0x00069198 File Offset: 0x00068198
		private static string generalize(Uri location)
		{
			string absoluteUri = location.AbsoluteUri;
			int num = absoluteUri.LastIndexOf('/');
			if (num < 0)
			{
				return absoluteUri;
			}
			return absoluteUri.Substring(0, num + 1);
		}

		// Token: 0x06001BD1 RID: 7121 RVA: 0x000691C8 File Offset: 0x000681C8
		internal static int FindSubstringNotInQuotes(string challenge, string signature)
		{
			int num = -1;
			if (challenge != null && signature != null && challenge.Length >= signature.Length)
			{
				int num2 = -1;
				int num3 = -1;
				for (int i = 0; i < challenge.Length; i++)
				{
					if (challenge[i] == '"')
					{
						if (num2 <= num3)
						{
							num2 = i;
						}
						else
						{
							num3 = i;
						}
					}
					if (i == challenge.Length - 1 || (challenge[i] == '"' && num2 > num3))
					{
						if (i == challenge.Length - 1)
						{
							num2 = challenge.Length;
						}
						if (num2 >= num3 + 3)
						{
							num = AuthenticationManager.IndexOf(challenge, signature, num3 + 1, num2 - num3 - 1);
							if (num >= 0)
							{
								if ((num == 0 || challenge[num - 1] == ' ' || challenge[num - 1] == ',') && (num + signature.Length == challenge.Length || challenge[num + signature.Length] == ' ' || challenge[num + signature.Length] == ','))
								{
									break;
								}
								num = -1;
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06001BD2 RID: 7122 RVA: 0x000692C4 File Offset: 0x000682C4
		private static int IndexOf(string challenge, string lwrCaseSignature, int start, int count)
		{
			count += start + 1 - lwrCaseSignature.Length;
			while (start < count)
			{
				int num = 0;
				while (num < lwrCaseSignature.Length && (challenge[start + num] | ' ') == lwrCaseSignature[num])
				{
					num++;
				}
				if (num == lwrCaseSignature.Length)
				{
					return start;
				}
				start++;
			}
			return -1;
		}

		// Token: 0x06001BD3 RID: 7123 RVA: 0x0006931C File Offset: 0x0006831C
		internal static int SplitNoQuotes(string challenge, ref int offset)
		{
			int num = offset;
			offset = -1;
			if (challenge != null && num < challenge.Length)
			{
				int num2 = -1;
				int num3 = -1;
				for (int i = num; i < challenge.Length; i++)
				{
					if (num2 > num3 && challenge[i] == '\\' && i + 1 < challenge.Length && challenge[i + 1] == '"')
					{
						i++;
					}
					else if (challenge[i] == '"')
					{
						if (num2 <= num3)
						{
							num2 = i;
						}
						else
						{
							num3 = i;
						}
					}
					else if (challenge[i] == '=' && num2 <= num3 && offset < 0)
					{
						offset = i;
					}
					else if (challenge[i] == ',' && num2 <= num3)
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x06001BD4 RID: 7124 RVA: 0x000693C5 File Offset: 0x000683C5
		internal static Authorization GetGroupAuthorization(IAuthenticationModule thisModule, string token, bool finished, NTAuthentication authSession, bool shareAuthenticatedConnections, bool mutualAuth)
		{
			return new Authorization(token, finished, shareAuthenticatedConnections ? null : (thisModule.GetType().FullName + "/" + authSession.UniqueUserId), mutualAuth);
		}

		// Token: 0x04001C7C RID: 7292
		private static PrefixLookup s_ModuleBinding = new PrefixLookup();

		// Token: 0x04001C7D RID: 7293
		private static ArrayList s_ModuleList;

		// Token: 0x04001C7E RID: 7294
		private static ICredentialPolicy s_ICredentialPolicy;

		// Token: 0x04001C7F RID: 7295
		private static SpnDictionary m_SpnDictionary = new SpnDictionary();

		// Token: 0x04001C80 RID: 7296
		private static TriState s_OSSupportsExtendedProtection = TriState.Unspecified;

		// Token: 0x04001C81 RID: 7297
		private static TriState s_SspSupportsExtendedProtection = TriState.Unspecified;
	}
}
