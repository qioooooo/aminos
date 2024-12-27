using System;
using System.Net.Security;

namespace System.Net
{
	// Token: 0x020004AE RID: 1198
	internal class AuthenticationState
	{
		// Token: 0x060024E0 RID: 9440 RVA: 0x00091F4C File Offset: 0x00090F4C
		internal NTAuthentication GetSecurityContext(IAuthenticationModule module)
		{
			if (module != this.Module)
			{
				return null;
			}
			return this.SecurityContext;
		}

		// Token: 0x060024E1 RID: 9441 RVA: 0x00091F5F File Offset: 0x00090F5F
		internal void SetSecurityContext(NTAuthentication securityContext, IAuthenticationModule module)
		{
			this.SecurityContext = securityContext;
		}

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x060024E2 RID: 9442 RVA: 0x00091F68 File Offset: 0x00090F68
		// (set) Token: 0x060024E3 RID: 9443 RVA: 0x00091F70 File Offset: 0x00090F70
		internal TransportContext TransportContext
		{
			get
			{
				return this._TransportContext;
			}
			set
			{
				this._TransportContext = value;
			}
		}

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x060024E4 RID: 9444 RVA: 0x00091F79 File Offset: 0x00090F79
		internal HttpResponseHeader AuthenticateHeader
		{
			get
			{
				if (!this.IsProxyAuth)
				{
					return HttpResponseHeader.WwwAuthenticate;
				}
				return HttpResponseHeader.ProxyAuthenticate;
			}
		}

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x060024E5 RID: 9445 RVA: 0x00091F88 File Offset: 0x00090F88
		internal string AuthorizationHeader
		{
			get
			{
				if (!this.IsProxyAuth)
				{
					return "Authorization";
				}
				return "Proxy-Authorization";
			}
		}

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x060024E6 RID: 9446 RVA: 0x00091F9D File Offset: 0x00090F9D
		internal HttpStatusCode StatusCodeMatch
		{
			get
			{
				if (!this.IsProxyAuth)
				{
					return HttpStatusCode.Unauthorized;
				}
				return HttpStatusCode.ProxyAuthenticationRequired;
			}
		}

		// Token: 0x060024E7 RID: 9447 RVA: 0x00091FB2 File Offset: 0x00090FB2
		internal AuthenticationState(bool isProxyAuth)
		{
			this.IsProxyAuth = isProxyAuth;
		}

		// Token: 0x060024E8 RID: 9448 RVA: 0x00091FC4 File Offset: 0x00090FC4
		private void PrepareState(HttpWebRequest httpWebRequest)
		{
			Uri uri = (this.IsProxyAuth ? httpWebRequest.ServicePoint.InternalAddress : httpWebRequest.Address);
			if (this.ChallengedUri != uri)
			{
				if (this.ChallengedUri == null || this.ChallengedUri.Scheme != uri.Scheme || this.ChallengedUri.Host != uri.Host || this.ChallengedUri.Port != uri.Port)
				{
					this.ChallengedSpn = null;
				}
				this.ChallengedUri = uri;
			}
			httpWebRequest.CurrentAuthenticationState = this;
		}

		// Token: 0x060024E9 RID: 9449 RVA: 0x00092054 File Offset: 0x00091054
		internal string GetComputeSpn(HttpWebRequest httpWebRequest)
		{
			if (this.ChallengedSpn != null)
			{
				return this.ChallengedSpn;
			}
			string text = httpWebRequest.ChallengedUri.GetParts(UriComponents.Scheme | UriComponents.Host | UriComponents.Port | UriComponents.Path, UriFormat.SafeUnescaped);
			string text2 = AuthenticationManager.SpnDictionary.InternalGet(text);
			if (text2 == null)
			{
				if (!this.IsProxyAuth && httpWebRequest.ServicePoint.InternalProxyServicePoint)
				{
					text2 = httpWebRequest.ChallengedUri.Host;
					if (httpWebRequest.ChallengedUri.HostNameType == UriHostNameType.IPv6 || httpWebRequest.ChallengedUri.HostNameType == UriHostNameType.IPv4 || text2.IndexOf('.') != -1)
					{
						goto IL_009F;
					}
					try
					{
						text2 = Dns.InternalGetHostByName(text2).HostName;
						goto IL_009F;
					}
					catch (Exception ex)
					{
						if (NclUtilities.IsFatal(ex))
						{
							throw;
						}
						goto IL_009F;
					}
				}
				text2 = httpWebRequest.ServicePoint.Hostname;
				IL_009F:
				text2 = "HTTP/" + text2;
				text = httpWebRequest.ChallengedUri.GetParts(UriComponents.SchemeAndServer, UriFormat.SafeUnescaped) + "/";
				AuthenticationManager.SpnDictionary.InternalSet(text, text2);
			}
			return this.ChallengedSpn = text2;
		}

		// Token: 0x060024EA RID: 9450 RVA: 0x0009214C File Offset: 0x0009114C
		internal void PreAuthIfNeeded(HttpWebRequest httpWebRequest, ICredentials authInfo)
		{
			if (!this.TriedPreAuth)
			{
				this.TriedPreAuth = true;
				if (authInfo != null)
				{
					this.PrepareState(httpWebRequest);
					try
					{
						Authorization authorization = AuthenticationManager.PreAuthenticate(httpWebRequest, authInfo);
						if (authorization != null && authorization.Message != null)
						{
							this.UniqueGroupId = authorization.ConnectionGroupId;
							httpWebRequest.Headers.Set(this.AuthorizationHeader, authorization.Message);
						}
					}
					catch (Exception)
					{
						this.ClearSession(httpWebRequest);
					}
					catch
					{
						this.ClearSession(httpWebRequest);
					}
				}
			}
		}

		// Token: 0x060024EB RID: 9451 RVA: 0x000921DC File Offset: 0x000911DC
		internal bool AttemptAuthenticate(HttpWebRequest httpWebRequest, ICredentials authInfo)
		{
			if (this.Authorization != null && this.Authorization.Complete)
			{
				if (this.IsProxyAuth)
				{
					this.ClearAuthReq(httpWebRequest);
				}
				return false;
			}
			if (authInfo == null)
			{
				return false;
			}
			string text = httpWebRequest.AuthHeader(this.AuthenticateHeader);
			if (text == null)
			{
				if (!this.IsProxyAuth && this.Authorization != null && httpWebRequest.ProxyAuthenticationState.Authorization != null)
				{
					httpWebRequest.Headers.Set(this.AuthorizationHeader, this.Authorization.Message);
				}
				return false;
			}
			this.PrepareState(httpWebRequest);
			try
			{
				this.Authorization = AuthenticationManager.Authenticate(text, httpWebRequest, authInfo);
			}
			catch (Exception)
			{
				this.Authorization = null;
				this.ClearSession(httpWebRequest);
				throw;
			}
			catch
			{
				this.Authorization = null;
				this.ClearSession(httpWebRequest);
				throw;
			}
			if (this.Authorization == null)
			{
				return false;
			}
			if (this.Authorization.Message == null)
			{
				this.Authorization = null;
				return false;
			}
			this.UniqueGroupId = this.Authorization.ConnectionGroupId;
			try
			{
				httpWebRequest.Headers.Set(this.AuthorizationHeader, this.Authorization.Message);
			}
			catch
			{
				this.Authorization = null;
				this.ClearSession(httpWebRequest);
				throw;
			}
			return true;
		}

		// Token: 0x060024EC RID: 9452 RVA: 0x00092324 File Offset: 0x00091324
		internal void ClearAuthReq(HttpWebRequest httpWebRequest)
		{
			this.TriedPreAuth = false;
			this.Authorization = null;
			this.UniqueGroupId = null;
			httpWebRequest.Headers.Remove(this.AuthorizationHeader);
		}

		// Token: 0x060024ED RID: 9453 RVA: 0x0009234C File Offset: 0x0009134C
		internal void Update(HttpWebRequest httpWebRequest)
		{
			if (this.Authorization != null)
			{
				this.PrepareState(httpWebRequest);
				ISessionAuthenticationModule sessionAuthenticationModule = this.Module as ISessionAuthenticationModule;
				if (sessionAuthenticationModule != null)
				{
					string text = httpWebRequest.AuthHeader(this.AuthenticateHeader);
					if (this.IsProxyAuth || httpWebRequest.ResponseStatusCode != HttpStatusCode.ProxyAuthenticationRequired)
					{
						bool flag = true;
						try
						{
							flag = sessionAuthenticationModule.Update(text, httpWebRequest);
						}
						catch (Exception)
						{
							this.ClearSession(httpWebRequest);
							if (httpWebRequest.AuthenticationLevel == AuthenticationLevel.MutualAuthRequired && (httpWebRequest.CurrentAuthenticationState == null || httpWebRequest.CurrentAuthenticationState.Authorization == null || !httpWebRequest.CurrentAuthenticationState.Authorization.MutuallyAuthenticated))
							{
								throw;
							}
						}
						catch
						{
							this.ClearSession(httpWebRequest);
							if (httpWebRequest.AuthenticationLevel == AuthenticationLevel.MutualAuthRequired && (httpWebRequest.CurrentAuthenticationState == null || httpWebRequest.CurrentAuthenticationState.Authorization == null || !httpWebRequest.CurrentAuthenticationState.Authorization.MutuallyAuthenticated))
							{
								throw;
							}
						}
						this.Authorization.SetComplete(flag);
					}
				}
				if (this.Module != null && this.Authorization.Complete && this.Module.CanPreAuthenticate && httpWebRequest.ResponseStatusCode != this.StatusCodeMatch)
				{
					AuthenticationManager.BindModule(this.ChallengedUri, this.Authorization, this.Module);
				}
			}
		}

		// Token: 0x060024EE RID: 9454 RVA: 0x00092498 File Offset: 0x00091498
		internal void ClearSession()
		{
			if (this.SecurityContext != null)
			{
				this.SecurityContext.CloseContext();
				this.SecurityContext = null;
			}
		}

		// Token: 0x060024EF RID: 9455 RVA: 0x000924B4 File Offset: 0x000914B4
		internal void ClearSession(HttpWebRequest httpWebRequest)
		{
			this.PrepareState(httpWebRequest);
			ISessionAuthenticationModule sessionAuthenticationModule = this.Module as ISessionAuthenticationModule;
			this.Module = null;
			if (sessionAuthenticationModule != null)
			{
				try
				{
					sessionAuthenticationModule.ClearSession(httpWebRequest);
				}
				catch (Exception ex)
				{
					if (NclUtilities.IsFatal(ex))
					{
						throw;
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x040024D8 RID: 9432
		private bool TriedPreAuth;

		// Token: 0x040024D9 RID: 9433
		internal Authorization Authorization;

		// Token: 0x040024DA RID: 9434
		internal IAuthenticationModule Module;

		// Token: 0x040024DB RID: 9435
		internal string UniqueGroupId;

		// Token: 0x040024DC RID: 9436
		private bool IsProxyAuth;

		// Token: 0x040024DD RID: 9437
		internal Uri ChallengedUri;

		// Token: 0x040024DE RID: 9438
		private string ChallengedSpn;

		// Token: 0x040024DF RID: 9439
		private NTAuthentication SecurityContext;

		// Token: 0x040024E0 RID: 9440
		private TransportContext _TransportContext;
	}
}
