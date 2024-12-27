using System;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x020003F2 RID: 1010
	internal static class ExceptionHelper
	{
		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x0600209B RID: 8347 RVA: 0x00080B96 File Offset: 0x0007FB96
		internal static NotImplementedException MethodNotImplementedException
		{
			get
			{
				return new NotImplementedException(SR.GetString("net_MethodNotImplementedException"));
			}
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x0600209C RID: 8348 RVA: 0x00080BA7 File Offset: 0x0007FBA7
		internal static NotImplementedException PropertyNotImplementedException
		{
			get
			{
				return new NotImplementedException(SR.GetString("net_PropertyNotImplementedException"));
			}
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x0600209D RID: 8349 RVA: 0x00080BB8 File Offset: 0x0007FBB8
		internal static NotSupportedException MethodNotSupportedException
		{
			get
			{
				return new NotSupportedException(SR.GetString("net_MethodNotSupportedException"));
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x0600209E RID: 8350 RVA: 0x00080BC9 File Offset: 0x0007FBC9
		internal static NotSupportedException PropertyNotSupportedException
		{
			get
			{
				return new NotSupportedException(SR.GetString("net_PropertyNotSupportedException"));
			}
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x0600209F RID: 8351 RVA: 0x00080BDA File Offset: 0x0007FBDA
		internal static WebException IsolatedException
		{
			get
			{
				return new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.KeepAliveFailure), WebExceptionStatus.KeepAliveFailure, WebExceptionInternalStatus.Isolated, null);
			}
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x060020A0 RID: 8352 RVA: 0x00080BF1 File Offset: 0x0007FBF1
		internal static WebException RequestAbortedException
		{
			get
			{
				return new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestCanceled), WebExceptionStatus.RequestCanceled);
			}
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x060020A1 RID: 8353 RVA: 0x00080C04 File Offset: 0x0007FC04
		internal static UriFormatException BadSchemeException
		{
			get
			{
				return new UriFormatException(SR.GetString("net_uri_BadScheme"));
			}
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x060020A2 RID: 8354 RVA: 0x00080C15 File Offset: 0x0007FC15
		internal static UriFormatException BadAuthorityException
		{
			get
			{
				return new UriFormatException(SR.GetString("net_uri_BadAuthority"));
			}
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x060020A3 RID: 8355 RVA: 0x00080C26 File Offset: 0x0007FC26
		internal static UriFormatException EmptyUriException
		{
			get
			{
				return new UriFormatException(SR.GetString("net_uri_EmptyUri"));
			}
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x060020A4 RID: 8356 RVA: 0x00080C37 File Offset: 0x0007FC37
		internal static UriFormatException SchemeLimitException
		{
			get
			{
				return new UriFormatException(SR.GetString("net_uri_SchemeLimit"));
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x060020A5 RID: 8357 RVA: 0x00080C48 File Offset: 0x0007FC48
		internal static UriFormatException SizeLimitException
		{
			get
			{
				return new UriFormatException(SR.GetString("net_uri_SizeLimit"));
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x060020A6 RID: 8358 RVA: 0x00080C59 File Offset: 0x0007FC59
		internal static UriFormatException MustRootedPathException
		{
			get
			{
				return new UriFormatException(SR.GetString("net_uri_MustRootedPath"));
			}
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x060020A7 RID: 8359 RVA: 0x00080C6A File Offset: 0x0007FC6A
		internal static UriFormatException BadHostNameException
		{
			get
			{
				return new UriFormatException(SR.GetString("net_uri_BadHostName"));
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x060020A8 RID: 8360 RVA: 0x00080C7B File Offset: 0x0007FC7B
		internal static UriFormatException BadPortException
		{
			get
			{
				return new UriFormatException(SR.GetString("net_uri_BadPort"));
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x060020A9 RID: 8361 RVA: 0x00080C8C File Offset: 0x0007FC8C
		internal static UriFormatException BadAuthorityTerminatorException
		{
			get
			{
				return new UriFormatException(SR.GetString("net_uri_BadAuthorityTerminator"));
			}
		}

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x060020AA RID: 8362 RVA: 0x00080C9D File Offset: 0x0007FC9D
		internal static UriFormatException BadFormatException
		{
			get
			{
				return new UriFormatException(SR.GetString("net_uri_BadFormat"));
			}
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x060020AB RID: 8363 RVA: 0x00080CAE File Offset: 0x0007FCAE
		internal static UriFormatException CannotCreateRelativeException
		{
			get
			{
				return new UriFormatException(SR.GetString("net_uri_CannotCreateRelative"));
			}
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x060020AC RID: 8364 RVA: 0x00080CBF File Offset: 0x0007FCBF
		internal static WebException CacheEntryNotFoundException
		{
			get
			{
				return new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.CacheEntryNotFound), WebExceptionStatus.CacheEntryNotFound);
			}
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x060020AD RID: 8365 RVA: 0x00080CD4 File Offset: 0x0007FCD4
		internal static WebException RequestProhibitedByCachePolicyException
		{
			get
			{
				return new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestProhibitedByCachePolicy), WebExceptionStatus.RequestProhibitedByCachePolicy);
			}
		}

		// Token: 0x04001FD6 RID: 8150
		internal static readonly KeyContainerPermission KeyContainerPermissionOpen = new KeyContainerPermission(KeyContainerPermissionFlags.Open);

		// Token: 0x04001FD7 RID: 8151
		internal static readonly WebPermission WebPermissionUnrestricted = new WebPermission(NetworkAccess.Connect);

		// Token: 0x04001FD8 RID: 8152
		internal static readonly SecurityPermission UnmanagedPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);

		// Token: 0x04001FD9 RID: 8153
		internal static readonly SocketPermission UnrestrictedSocketPermission = new SocketPermission(PermissionState.Unrestricted);

		// Token: 0x04001FDA RID: 8154
		internal static readonly SecurityPermission InfrastructurePermission = new SecurityPermission(SecurityPermissionFlag.Infrastructure);

		// Token: 0x04001FDB RID: 8155
		internal static readonly SecurityPermission ControlPolicyPermission = new SecurityPermission(SecurityPermissionFlag.ControlPolicy);

		// Token: 0x04001FDC RID: 8156
		internal static readonly SecurityPermission ControlPrincipalPermission = new SecurityPermission(SecurityPermissionFlag.ControlPrincipal);
	}
}
