using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Xml;
using Microsoft.Win32;

namespace System
{
	// Token: 0x02000354 RID: 852
	[TypeConverter(typeof(UriTypeConverter))]
	[Serializable]
	public class Uri : ISerializable
	{
		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06001AC2 RID: 6850 RVA: 0x0005D607 File Offset: 0x0005C607
		private bool IsImplicitFile
		{
			get
			{
				return (this.m_Flags & Uri.Flags.ImplicitFile) != Uri.Flags.Zero;
			}
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x06001AC3 RID: 6851 RVA: 0x0005D61D File Offset: 0x0005C61D
		private bool IsUncOrDosPath
		{
			get
			{
				return (this.m_Flags & (Uri.Flags.DosPath | Uri.Flags.UncPath)) != Uri.Flags.Zero;
			}
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06001AC4 RID: 6852 RVA: 0x0005D633 File Offset: 0x0005C633
		private bool IsDosPath
		{
			get
			{
				return (this.m_Flags & Uri.Flags.DosPath) != Uri.Flags.Zero;
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06001AC5 RID: 6853 RVA: 0x0005D649 File Offset: 0x0005C649
		private bool IsUncPath
		{
			get
			{
				return (this.m_Flags & Uri.Flags.UncPath) != Uri.Flags.Zero;
			}
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06001AC6 RID: 6854 RVA: 0x0005D65F File Offset: 0x0005C65F
		private Uri.Flags HostType
		{
			get
			{
				return this.m_Flags & Uri.Flags.HostTypeMask;
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06001AC7 RID: 6855 RVA: 0x0005D66E File Offset: 0x0005C66E
		private UriParser Syntax
		{
			get
			{
				return this.m_Syntax;
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06001AC8 RID: 6856 RVA: 0x0005D676 File Offset: 0x0005C676
		private bool IsNotAbsoluteUri
		{
			get
			{
				return this.m_Syntax == null;
			}
		}

		// Token: 0x06001AC9 RID: 6857 RVA: 0x0005D681 File Offset: 0x0005C681
		private static bool IriParsingStatic(UriParser syntax)
		{
			return Uri.s_IriParsing && ((syntax != null && syntax.InFact(UriSyntaxFlags.AllowIriParsing)) || syntax == null);
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06001ACA RID: 6858 RVA: 0x0005D6A4 File Offset: 0x0005C6A4
		private bool AllowIdn
		{
			get
			{
				return this.m_Syntax != null && (this.m_Syntax.Flags & UriSyntaxFlags.AllowIdn) != (UriSyntaxFlags)0 && (Uri.s_IdnScope == UriIdnScope.All || (Uri.s_IdnScope == UriIdnScope.AllExceptIntranet && this.NotAny(Uri.Flags.IntranetUri)));
			}
		}

		// Token: 0x06001ACB RID: 6859 RVA: 0x0005D6F1 File Offset: 0x0005C6F1
		private bool AllowIdnStatic(UriParser syntax, Uri.Flags flags)
		{
			return syntax != null && (syntax.Flags & UriSyntaxFlags.AllowIdn) != (UriSyntaxFlags)0 && (Uri.s_IdnScope == UriIdnScope.All || (Uri.s_IdnScope == UriIdnScope.AllExceptIntranet && Uri.StaticNotAny(flags, Uri.Flags.IntranetUri)));
		}

		// Token: 0x06001ACC RID: 6860 RVA: 0x0005D72C File Offset: 0x0005C72C
		private bool IsIntranet(string schemeHost)
		{
			bool flag = false;
			int num = -1;
			int num2 = -2147467259;
			if (this.m_Syntax.SchemeName.Length > 32)
			{
				return false;
			}
			if (Uri.s_ManagerRef == null)
			{
				lock (Uri.s_IntranetLock)
				{
					if (Uri.s_ManagerRef == null)
					{
						Uri.s_ManagerRef = (IInternetSecurityManager)new InternetSecurityManager();
					}
				}
			}
			try
			{
				Uri.s_ManagerRef.MapUrlToZone(schemeHost.TrimStart(Uri._WSchars), out num, 0);
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode == num2)
				{
					flag = true;
				}
			}
			if (num == 1)
			{
				return true;
			}
			if (num == 2 || num == 4 || flag)
			{
				for (int i = 0; i < schemeHost.Length; i++)
				{
					if (schemeHost[i] == '.')
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06001ACD RID: 6861 RVA: 0x0005D808 File Offset: 0x0005C808
		internal bool UserDrivenParsing
		{
			get
			{
				return (this.m_Flags & Uri.Flags.UserDrivenParsing) != Uri.Flags.Zero;
			}
		}

		// Token: 0x06001ACE RID: 6862 RVA: 0x0005D81E File Offset: 0x0005C81E
		private void SetUserDrivenParsing()
		{
			this.m_Flags = Uri.Flags.UserDrivenParsing | (this.m_Flags & Uri.Flags.UserEscaped);
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06001ACF RID: 6863 RVA: 0x0005D83C File Offset: 0x0005C83C
		private ushort SecuredPathIndex
		{
			get
			{
				if (this.IsDosPath)
				{
					char c = this.m_String[(int)this.m_Info.Offset.Path];
					return (c == '/' || c == '\\') ? 3 : 2;
				}
				return 0;
			}
		}

		// Token: 0x06001AD0 RID: 6864 RVA: 0x0005D87E File Offset: 0x0005C87E
		private bool NotAny(Uri.Flags flags)
		{
			return (this.m_Flags & flags) == Uri.Flags.Zero;
		}

		// Token: 0x06001AD1 RID: 6865 RVA: 0x0005D88C File Offset: 0x0005C88C
		private bool InFact(Uri.Flags flags)
		{
			return (this.m_Flags & flags) != Uri.Flags.Zero;
		}

		// Token: 0x06001AD2 RID: 6866 RVA: 0x0005D89D File Offset: 0x0005C89D
		private static bool StaticNotAny(Uri.Flags allFlags, Uri.Flags checkFlags)
		{
			return (allFlags & checkFlags) == Uri.Flags.Zero;
		}

		// Token: 0x06001AD3 RID: 6867 RVA: 0x0005D8A6 File Offset: 0x0005C8A6
		private static bool StaticInFact(Uri.Flags allFlags, Uri.Flags checkFlags)
		{
			return (allFlags & checkFlags) != Uri.Flags.Zero;
		}

		// Token: 0x06001AD4 RID: 6868 RVA: 0x0005D8B4 File Offset: 0x0005C8B4
		private Uri.UriInfo EnsureUriInfo()
		{
			Uri.Flags flags = this.m_Flags;
			if ((this.m_Flags & Uri.Flags.MinimalUriInfoSet) == Uri.Flags.Zero)
			{
				this.CreateUriInfo(flags);
			}
			return this.m_Info;
		}

		// Token: 0x06001AD5 RID: 6869 RVA: 0x0005D8E6 File Offset: 0x0005C8E6
		private void EnsureParseRemaining()
		{
			if ((this.m_Flags & (Uri.Flags)(-2147483648)) == Uri.Flags.Zero)
			{
				this.ParseRemaining();
			}
		}

		// Token: 0x06001AD6 RID: 6870 RVA: 0x0005D8FF File Offset: 0x0005C8FF
		private void EnsureHostString(bool allowDnsOptimization)
		{
			this.EnsureUriInfo();
			if (this.m_Info.Host == null)
			{
				if (allowDnsOptimization && this.InFact(Uri.Flags.CanonicalDnsHost))
				{
					return;
				}
				this.CreateHostString();
			}
		}

		// Token: 0x06001AD7 RID: 6871 RVA: 0x0005D92D File Offset: 0x0005C92D
		public Uri(string uriString)
		{
			if (uriString == null)
			{
				throw new ArgumentNullException("uriString");
			}
			this.CreateThis(uriString, false, UriKind.Absolute);
		}

		// Token: 0x06001AD8 RID: 6872 RVA: 0x0005D94C File Offset: 0x0005C94C
		[Obsolete("The constructor has been deprecated. Please use new Uri(string). The dontEscape parameter is deprecated and is always false. http://go.microsoft.com/fwlink/?linkid=14202")]
		public Uri(string uriString, bool dontEscape)
		{
			if (uriString == null)
			{
				throw new ArgumentNullException("uriString");
			}
			this.CreateThis(uriString, dontEscape, UriKind.Absolute);
		}

		// Token: 0x06001AD9 RID: 6873 RVA: 0x0005D96B File Offset: 0x0005C96B
		public Uri(string uriString, UriKind uriKind)
		{
			if (uriString == null)
			{
				throw new ArgumentNullException("uriString");
			}
			this.CreateThis(uriString, false, uriKind);
		}

		// Token: 0x06001ADA RID: 6874 RVA: 0x0005D98A File Offset: 0x0005C98A
		public Uri(Uri baseUri, string relativeUri)
		{
			if (baseUri == null)
			{
				throw new ArgumentNullException("baseUri");
			}
			if (!baseUri.IsAbsoluteUri)
			{
				throw new ArgumentOutOfRangeException("baseUri");
			}
			this.CreateUri(baseUri, relativeUri, false);
		}

		// Token: 0x06001ADB RID: 6875 RVA: 0x0005D9BC File Offset: 0x0005C9BC
		[Obsolete("The constructor has been deprecated. Please new Uri(Uri, string). The dontEscape parameter is deprecated and is always false. http://go.microsoft.com/fwlink/?linkid=14202")]
		public Uri(Uri baseUri, string relativeUri, bool dontEscape)
		{
			if (baseUri == null)
			{
				throw new ArgumentNullException("baseUri");
			}
			if (!baseUri.IsAbsoluteUri)
			{
				throw new ArgumentOutOfRangeException("baseUri");
			}
			this.CreateUri(baseUri, relativeUri, dontEscape);
		}

		// Token: 0x06001ADC RID: 6876 RVA: 0x0005D9F0 File Offset: 0x0005C9F0
		private void CreateUri(Uri baseUri, string relativeUri, bool dontEscape)
		{
			this.CreateThis(relativeUri, dontEscape, UriKind.RelativeOrAbsolute);
			if (baseUri.Syntax.IsSimple)
			{
				UriFormatException ex;
				Uri uri = Uri.ResolveHelper(baseUri, this, ref relativeUri, ref dontEscape, out ex);
				if (ex != null)
				{
					throw ex;
				}
				if (uri != null)
				{
					if (uri != this)
					{
						this.CreateThisFromUri(uri);
					}
					return;
				}
			}
			else
			{
				dontEscape = false;
				UriFormatException ex;
				relativeUri = baseUri.Syntax.InternalResolve(baseUri, this, out ex);
				if (ex != null)
				{
					throw ex;
				}
			}
			this.m_Flags = Uri.Flags.Zero;
			this.m_Info = null;
			this.m_Syntax = null;
			this.CreateThis(relativeUri, dontEscape, UriKind.Absolute);
		}

		// Token: 0x06001ADD RID: 6877 RVA: 0x0005DA74 File Offset: 0x0005CA74
		public Uri(Uri baseUri, Uri relativeUri)
		{
			if (baseUri == null)
			{
				throw new ArgumentNullException("baseUri");
			}
			if (!baseUri.IsAbsoluteUri)
			{
				throw new ArgumentOutOfRangeException("baseUri");
			}
			this.CreateThisFromUri(relativeUri);
			string text = null;
			bool flag;
			if (baseUri.Syntax.IsSimple)
			{
				flag = this.InFact(Uri.Flags.UserEscaped);
				UriFormatException ex;
				relativeUri = Uri.ResolveHelper(baseUri, this, ref text, ref flag, out ex);
				if (ex != null)
				{
					throw ex;
				}
				if (relativeUri != null)
				{
					if (relativeUri != this)
					{
						this.CreateThisFromUri(relativeUri);
					}
					return;
				}
			}
			else
			{
				flag = false;
				UriFormatException ex;
				text = baseUri.Syntax.InternalResolve(baseUri, this, out ex);
				if (ex != null)
				{
					throw ex;
				}
			}
			this.m_Flags = Uri.Flags.Zero;
			this.m_Info = null;
			this.m_Syntax = null;
			this.CreateThis(text, flag, UriKind.Absolute);
		}

		// Token: 0x06001ADE RID: 6878 RVA: 0x0005DB2C File Offset: 0x0005CB2C
		protected Uri(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			string text = serializationInfo.GetString("AbsoluteUri");
			if (text.Length != 0)
			{
				this.CreateThis(text, false, UriKind.Absolute);
				return;
			}
			text = serializationInfo.GetString("RelativeUri");
			if (text == null)
			{
				throw new ArgumentNullException("uriString");
			}
			this.CreateThis(text, false, UriKind.Relative);
		}

		// Token: 0x06001ADF RID: 6879 RVA: 0x0005DB80 File Offset: 0x0005CB80
		private unsafe static Uri.ParsingError GetCombinedString(Uri baseUri, string relativeStr, bool dontEscape, ref string result)
		{
			int num = 0;
			while (num < relativeStr.Length && relativeStr[num] != '/' && relativeStr[num] != '\\' && relativeStr[num] != '?' && relativeStr[num] != '#')
			{
				if (relativeStr[num] == ':')
				{
					if (num >= 2)
					{
						string text = relativeStr.Substring(0, num);
						fixed (char* ptr = text)
						{
							UriParser uriParser = null;
							if (Uri.CheckSchemeSyntax(ptr, (ushort)text.Length, ref uriParser) == Uri.ParsingError.None)
							{
								if (baseUri.Syntax != uriParser)
								{
									result = relativeStr;
									return Uri.ParsingError.None;
								}
								if (num + 1 < relativeStr.Length)
								{
									relativeStr = relativeStr.Substring(num + 1);
								}
								else
								{
									relativeStr = string.Empty;
								}
							}
						}
						break;
					}
					break;
				}
				else
				{
					num++;
				}
			}
			if (relativeStr.Length == 0)
			{
				result = baseUri.OriginalString;
				return Uri.ParsingError.None;
			}
			result = Uri.CombineUri(baseUri, relativeStr, dontEscape ? UriFormat.UriEscaped : UriFormat.SafeUnescaped);
			return Uri.ParsingError.None;
		}

		// Token: 0x06001AE0 RID: 6880 RVA: 0x0005DC74 File Offset: 0x0005CC74
		private static UriFormatException GetException(Uri.ParsingError err)
		{
			switch (err)
			{
			case Uri.ParsingError.None:
				return null;
			case Uri.ParsingError.BadFormat:
				return ExceptionHelper.BadFormatException;
			case Uri.ParsingError.BadScheme:
				return ExceptionHelper.BadSchemeException;
			case Uri.ParsingError.BadAuthority:
				return ExceptionHelper.BadAuthorityException;
			case Uri.ParsingError.EmptyUriString:
				return ExceptionHelper.EmptyUriException;
			case Uri.ParsingError.SchemeLimit:
				return ExceptionHelper.SchemeLimitException;
			case Uri.ParsingError.SizeLimit:
				return ExceptionHelper.SizeLimitException;
			case Uri.ParsingError.MustRootedPath:
				return ExceptionHelper.MustRootedPathException;
			case Uri.ParsingError.BadHostName:
				return ExceptionHelper.BadHostNameException;
			case Uri.ParsingError.NonEmptyHost:
				return ExceptionHelper.BadFormatException;
			case Uri.ParsingError.BadPort:
				return ExceptionHelper.BadPortException;
			case Uri.ParsingError.BadAuthorityTerminator:
				return ExceptionHelper.BadAuthorityTerminatorException;
			case Uri.ParsingError.CannotCreateRelative:
				return ExceptionHelper.CannotCreateRelativeException;
			default:
				return ExceptionHelper.BadFormatException;
			}
		}

		// Token: 0x06001AE1 RID: 6881 RVA: 0x0005DD0E File Offset: 0x0005CD0E
		[SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x06001AE2 RID: 6882 RVA: 0x0005DD18 File Offset: 0x0005CD18
		[SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
		protected void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			if (this.IsAbsoluteUri)
			{
				serializationInfo.AddValue("AbsoluteUri", this.GetParts(UriComponents.SerializationInfoString, UriFormat.UriEscaped));
				return;
			}
			serializationInfo.AddValue("AbsoluteUri", string.Empty);
			serializationInfo.AddValue("RelativeUri", this.GetParts(UriComponents.SerializationInfoString, UriFormat.UriEscaped));
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06001AE3 RID: 6883 RVA: 0x0005DD6C File Offset: 0x0005CD6C
		public string AbsolutePath
		{
			get
			{
				if (this.IsNotAbsoluteUri)
				{
					throw new InvalidOperationException(SR.GetString("net_uri_NotAbsolute"));
				}
				string text = this.PrivateAbsolutePath;
				if (this.IsDosPath && text[0] == '/')
				{
					text = text.Substring(1);
				}
				return text;
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06001AE4 RID: 6884 RVA: 0x0005DDB4 File Offset: 0x0005CDB4
		private string PrivateAbsolutePath
		{
			get
			{
				Uri.UriInfo uriInfo = this.EnsureUriInfo();
				if (uriInfo.MoreInfo == null)
				{
					uriInfo.MoreInfo = new Uri.MoreInfo();
				}
				string text = uriInfo.MoreInfo.Path;
				if (text == null)
				{
					text = this.GetParts(UriComponents.Path | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
					uriInfo.MoreInfo.Path = text;
				}
				return text;
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06001AE5 RID: 6885 RVA: 0x0005DE04 File Offset: 0x0005CE04
		public string AbsoluteUri
		{
			get
			{
				if (this.m_Syntax == null)
				{
					throw new InvalidOperationException(SR.GetString("net_uri_NotAbsolute"));
				}
				Uri.UriInfo uriInfo = this.EnsureUriInfo();
				if (uriInfo.MoreInfo == null)
				{
					uriInfo.MoreInfo = new Uri.MoreInfo();
				}
				string text = uriInfo.MoreInfo.AbsoluteUri;
				if (text == null)
				{
					text = this.GetParts(UriComponents.AbsoluteUri, UriFormat.UriEscaped);
					uriInfo.MoreInfo.AbsoluteUri = text;
				}
				return text;
			}
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06001AE6 RID: 6886 RVA: 0x0005DE69 File Offset: 0x0005CE69
		public string Authority
		{
			get
			{
				if (this.IsNotAbsoluteUri)
				{
					throw new InvalidOperationException(SR.GetString("net_uri_NotAbsolute"));
				}
				return this.GetParts(UriComponents.Host | UriComponents.Port, UriFormat.UriEscaped);
			}
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x06001AE7 RID: 6887 RVA: 0x0005DE8C File Offset: 0x0005CE8C
		public string Host
		{
			get
			{
				if (this.IsNotAbsoluteUri)
				{
					throw new InvalidOperationException(SR.GetString("net_uri_NotAbsolute"));
				}
				return this.GetParts(UriComponents.Host, UriFormat.UriEscaped);
			}
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06001AE8 RID: 6888 RVA: 0x0005DEB0 File Offset: 0x0005CEB0
		public UriHostNameType HostNameType
		{
			get
			{
				if (this.IsNotAbsoluteUri)
				{
					throw new InvalidOperationException(SR.GetString("net_uri_NotAbsolute"));
				}
				if (this.m_Syntax.IsSimple)
				{
					this.EnsureUriInfo();
				}
				else
				{
					this.EnsureHostString(false);
				}
				Uri.Flags hostType = this.HostType;
				if (hostType <= Uri.Flags.DnsHostType)
				{
					if (hostType == Uri.Flags.IPv6HostType)
					{
						return UriHostNameType.IPv6;
					}
					if (hostType == Uri.Flags.IPv4HostType)
					{
						return UriHostNameType.IPv4;
					}
					if (hostType == Uri.Flags.DnsHostType)
					{
						return UriHostNameType.Dns;
					}
				}
				else
				{
					if (hostType == Uri.Flags.UncHostType)
					{
						return UriHostNameType.Basic;
					}
					if (hostType == Uri.Flags.BasicHostType)
					{
						return UriHostNameType.Basic;
					}
					if (hostType == Uri.Flags.HostTypeMask)
					{
						return UriHostNameType.Unknown;
					}
				}
				return UriHostNameType.Unknown;
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06001AE9 RID: 6889 RVA: 0x0005DF4C File Offset: 0x0005CF4C
		public bool IsDefaultPort
		{
			get
			{
				if (this.IsNotAbsoluteUri)
				{
					throw new InvalidOperationException(SR.GetString("net_uri_NotAbsolute"));
				}
				if (this.m_Syntax.IsSimple)
				{
					this.EnsureUriInfo();
				}
				else
				{
					this.EnsureHostString(false);
				}
				return this.NotAny(Uri.Flags.NotDefaultPort);
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06001AEA RID: 6890 RVA: 0x0005DF9A File Offset: 0x0005CF9A
		public bool IsFile
		{
			get
			{
				if (this.IsNotAbsoluteUri)
				{
					throw new InvalidOperationException(SR.GetString("net_uri_NotAbsolute"));
				}
				return this.m_Syntax.SchemeName == Uri.UriSchemeFile;
			}
		}

		// Token: 0x06001AEB RID: 6891 RVA: 0x0005DFC6 File Offset: 0x0005CFC6
		private static bool StaticIsFile(UriParser syntax)
		{
			return syntax.InFact(UriSyntaxFlags.FileLikeUri);
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06001AEC RID: 6892 RVA: 0x0005DFD3 File Offset: 0x0005CFD3
		public bool IsLoopback
		{
			get
			{
				if (this.IsNotAbsoluteUri)
				{
					throw new InvalidOperationException(SR.GetString("net_uri_NotAbsolute"));
				}
				this.EnsureHostString(false);
				return this.InFact(Uri.Flags.LoopbackHost);
			}
		}

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06001AED RID: 6893 RVA: 0x0005E000 File Offset: 0x0005D000
		public bool IsUnc
		{
			get
			{
				if (this.IsNotAbsoluteUri)
				{
					throw new InvalidOperationException(SR.GetString("net_uri_NotAbsolute"));
				}
				return this.IsUncPath;
			}
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06001AEE RID: 6894 RVA: 0x0005E020 File Offset: 0x0005D020
		public string LocalPath
		{
			get
			{
				if (this.IsNotAbsoluteUri)
				{
					throw new InvalidOperationException(SR.GetString("net_uri_NotAbsolute"));
				}
				return this.GetLocalPath();
			}
		}

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06001AEF RID: 6895 RVA: 0x0005E040 File Offset: 0x0005D040
		internal static object InitializeLock
		{
			get
			{
				if (Uri.s_initLock == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref Uri.s_initLock, obj, null);
				}
				return Uri.s_initLock;
			}
		}

		// Token: 0x06001AF0 RID: 6896 RVA: 0x0005E06C File Offset: 0x0005D06C
		private static void InitializeUriConfig()
		{
			if (!Uri.s_ConfigInitialized)
			{
				lock (Uri.InitializeLock)
				{
					if (!Uri.s_ConfigInitialized)
					{
						Uri.s_ConfigInitialized = true;
						Uri.GetConfig(ref Uri.s_IdnScope, ref Uri.s_IriParsing);
					}
				}
			}
		}

		// Token: 0x06001AF1 RID: 6897 RVA: 0x0005E0C4 File Offset: 0x0005D0C4
		private static void GetConfig(ref UriIdnScope idnScope, ref bool iriParsing)
		{
			string text = null;
			new FileIOPermission(PermissionState.Unrestricted).Assert();
			try
			{
				text = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			if (Uri.IsWebConfig(text))
			{
				try
				{
					UriSectionInternal section = UriSectionInternal.GetSection();
					if (section != null)
					{
						idnScope = section.Idn;
						iriParsing = section.IriParsing;
					}
					return;
				}
				catch (ConfigurationException)
				{
					return;
				}
			}
			string text2 = null;
			new FileIOPermission(PermissionState.Unrestricted).Assert();
			try
			{
				text2 = RuntimeEnvironment.GetRuntimeDirectory();
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			string text3 = Path.Combine(Path.Combine(text2, "Config"), "machine.config");
			Uri.IdnScopeFromConfig idnScopeFromConfig;
			Uri.IriParsingFromConfig iriParsingFromConfig;
			Uri.ParseConfigFile(text3, out idnScopeFromConfig, out iriParsingFromConfig);
			Uri.IdnScopeFromConfig idnScopeFromConfig2;
			Uri.IriParsingFromConfig iriParsingFromConfig2;
			Uri.ParseConfigFile(text, out idnScopeFromConfig2, out iriParsingFromConfig2);
			switch (idnScopeFromConfig2)
			{
			case Uri.IdnScopeFromConfig.None:
				idnScope = UriIdnScope.None;
				break;
			case Uri.IdnScopeFromConfig.AllExceptIntranet:
				idnScope = UriIdnScope.AllExceptIntranet;
				break;
			case Uri.IdnScopeFromConfig.All:
				idnScope = UriIdnScope.All;
				break;
			default:
				switch (idnScopeFromConfig)
				{
				case Uri.IdnScopeFromConfig.None:
					idnScope = UriIdnScope.None;
					break;
				case Uri.IdnScopeFromConfig.AllExceptIntranet:
					idnScope = UriIdnScope.AllExceptIntranet;
					break;
				case Uri.IdnScopeFromConfig.All:
					idnScope = UriIdnScope.All;
					break;
				default:
					idnScope = UriIdnScope.None;
					break;
				}
				break;
			}
			switch (iriParsingFromConfig2)
			{
			case Uri.IriParsingFromConfig.False:
				iriParsing = false;
				return;
			case Uri.IriParsingFromConfig.True:
				iriParsing = true;
				return;
			default:
				switch (iriParsingFromConfig)
				{
				case Uri.IriParsingFromConfig.False:
					iriParsing = false;
					return;
				case Uri.IriParsingFromConfig.True:
					iriParsing = true;
					return;
				default:
					iriParsing = false;
					break;
				}
				break;
			}
		}

		// Token: 0x06001AF2 RID: 6898 RVA: 0x0005E220 File Offset: 0x0005D220
		private static bool IsWebConfig(string appConfigFile)
		{
			string text = AppDomain.CurrentDomain.GetData(".appVPath") as string;
			return text != null || (appConfigFile != null && (appConfigFile.StartsWith("http://", true, CultureInfo.InvariantCulture) || appConfigFile.StartsWith("https://", true, CultureInfo.InvariantCulture)));
		}

		// Token: 0x06001AF3 RID: 6899 RVA: 0x0005E274 File Offset: 0x0005D274
		private static void ParseConfigFile(string file, out Uri.IdnScopeFromConfig idnStateConfig, out Uri.IriParsingFromConfig iriParsingConfig)
		{
			idnStateConfig = Uri.IdnScopeFromConfig.NotFound;
			iriParsingConfig = Uri.IriParsingFromConfig.NotFound;
			new FileIOPermission(FileIOPermissionAccess.Read, file).Assert();
			try
			{
				FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
				using (fileStream)
				{
					XmlReader xmlReader = XmlReader.Create(fileStream, new XmlReaderSettings
					{
						IgnoreComments = true,
						IgnoreWhitespace = true,
						IgnoreProcessingInstructions = true
					});
					using (xmlReader)
					{
						if (xmlReader.ReadToFollowing("configuration"))
						{
							if (xmlReader.ReadToFollowing("uri"))
							{
								while (xmlReader.NodeType != XmlNodeType.EndElement || !Uri.ConfigStringEqual(xmlReader.Name, "uri"))
								{
									if (xmlReader.NodeType == XmlNodeType.Element)
									{
										if (Uri.ConfigStringEqual(xmlReader.Name, "idn"))
										{
											string text = xmlReader.GetAttribute("enabled");
											if (text != null)
											{
												if (Uri.ConfigStringEqual(text, "None"))
												{
													idnStateConfig = Uri.IdnScopeFromConfig.None;
												}
												else if (Uri.ConfigStringEqual(text, "AllExceptIntranet"))
												{
													idnStateConfig = Uri.IdnScopeFromConfig.AllExceptIntranet;
												}
												else if (Uri.ConfigStringEqual(text, "All"))
												{
													idnStateConfig = Uri.IdnScopeFromConfig.All;
												}
												else
												{
													idnStateConfig = Uri.IdnScopeFromConfig.Invalid;
												}
											}
										}
										else if (Uri.ConfigStringEqual(xmlReader.Name, "iriParsing"))
										{
											string text = xmlReader.GetAttribute("enabled");
											if (text != null)
											{
												if (Uri.ConfigStringEqual(text, "false"))
												{
													iriParsingConfig = Uri.IriParsingFromConfig.False;
												}
												else if (Uri.ConfigStringEqual(text, "true"))
												{
													iriParsingConfig = Uri.IriParsingFromConfig.True;
												}
												else
												{
													iriParsingConfig = Uri.IriParsingFromConfig.Invalid;
												}
											}
										}
									}
									if (!xmlReader.Read())
									{
										break;
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (NclUtilities.IsFatal(ex))
				{
					throw;
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x06001AF4 RID: 6900 RVA: 0x0005E470 File Offset: 0x0005D470
		private static bool ConfigStringEqual(string string1, string string2)
		{
			return string.Compare(string1, string2, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x06001AF5 RID: 6901 RVA: 0x0005E480 File Offset: 0x0005D480
		private string GetLocalPath()
		{
			this.EnsureParseRemaining();
			if (!this.IsUncOrDosPath)
			{
				return this.GetUnescapedParts(UriComponents.Path | UriComponents.KeepDelimiter, UriFormat.Unescaped);
			}
			this.EnsureHostString(false);
			int num;
			if (this.NotAny(Uri.Flags.HostNotCanonical | Uri.Flags.PathNotCanonical | Uri.Flags.ShouldBeCompressed))
			{
				num = (int)(this.IsUncPath ? (this.m_Info.Offset.Host - 2) : this.m_Info.Offset.Path);
				string text = ((this.IsImplicitFile && this.m_Info.Offset.Host == (this.IsDosPath ? 0 : 2) && this.m_Info.Offset.Query == this.m_Info.Offset.End) ? this.m_String : ((this.IsDosPath && (this.m_String[num] == '/' || this.m_String[num] == '\\')) ? this.m_String.Substring(num + 1, (int)this.m_Info.Offset.Query - num - 1) : this.m_String.Substring(num, (int)this.m_Info.Offset.Query - num)));
				if (this.IsDosPath && text[1] == '|')
				{
					text = text.Remove(1, 1);
					text = text.Insert(1, ":");
				}
				for (int i = 0; i < text.Length; i++)
				{
					if (text[i] == '/')
					{
						text = text.Replace('/', '\\');
						break;
					}
				}
				return text;
			}
			int num2 = 0;
			num = (int)this.m_Info.Offset.Path;
			string host = this.m_Info.Host;
			char[] array = new char[host.Length + 3 + (int)this.m_Info.Offset.Fragment - (int)this.m_Info.Offset.Path];
			if (this.IsUncPath)
			{
				array[0] = '\\';
				array[1] = '\\';
				num2 = 2;
				Uri.UnescapeString(host, 0, host.Length, array, ref num2, char.MaxValue, char.MaxValue, char.MaxValue, Uri.UnescapeMode.CopyOnly, this.m_Syntax, false, false);
			}
			else if (this.m_String[num] == '/' || this.m_String[num] == '\\')
			{
				num++;
			}
			ushort num3 = (ushort)num2;
			Uri.UnescapeMode unescapeMode = ((this.InFact(Uri.Flags.PathNotCanonical) && !this.IsImplicitFile) ? (Uri.UnescapeMode.Unescape | Uri.UnescapeMode.UnescapeAll) : Uri.UnescapeMode.CopyOnly);
			Uri.UnescapeString(this.m_String, num, (int)this.m_Info.Offset.Query, array, ref num2, char.MaxValue, char.MaxValue, char.MaxValue, unescapeMode, this.m_Syntax, true, false);
			if (array[1] == '|')
			{
				array[1] = ':';
			}
			if (this.InFact(Uri.Flags.ShouldBeCompressed))
			{
				array = Uri.Compress(array, this.IsDosPath ? (num3 + 2) : num3, ref num2, this.m_Syntax);
			}
			for (ushort num4 = 0; num4 < (ushort)num2; num4 += 1)
			{
				if (array[(int)num4] == '/')
				{
					array[(int)num4] = '\\';
				}
			}
			return new string(array, 0, num2);
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06001AF6 RID: 6902 RVA: 0x0005E774 File Offset: 0x0005D774
		public string PathAndQuery
		{
			get
			{
				if (this.IsNotAbsoluteUri)
				{
					throw new InvalidOperationException(SR.GetString("net_uri_NotAbsolute"));
				}
				string text = this.GetParts(UriComponents.PathAndQuery, UriFormat.UriEscaped);
				if (this.IsDosPath && text[0] == '/')
				{
					text = text.Substring(1);
				}
				return text;
			}
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06001AF7 RID: 6903 RVA: 0x0005E7C0 File Offset: 0x0005D7C0
		public int Port
		{
			get
			{
				if (this.IsNotAbsoluteUri)
				{
					throw new InvalidOperationException(SR.GetString("net_uri_NotAbsolute"));
				}
				if (this.m_Syntax.IsSimple)
				{
					this.EnsureUriInfo();
				}
				else
				{
					this.EnsureHostString(false);
				}
				if (this.InFact(Uri.Flags.NotDefaultPort))
				{
					return (int)this.m_Info.Offset.PortValue;
				}
				return this.m_Syntax.DefaultPort;
			}
		}

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06001AF8 RID: 6904 RVA: 0x0005E82C File Offset: 0x0005D82C
		public string Query
		{
			get
			{
				if (this.IsNotAbsoluteUri)
				{
					throw new InvalidOperationException(SR.GetString("net_uri_NotAbsolute"));
				}
				Uri.UriInfo uriInfo = this.EnsureUriInfo();
				if (uriInfo.MoreInfo == null)
				{
					uriInfo.MoreInfo = new Uri.MoreInfo();
				}
				string text = uriInfo.MoreInfo.Query;
				if (text == null)
				{
					text = this.GetParts(UriComponents.Query | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
					uriInfo.MoreInfo.Query = text;
				}
				return text;
			}
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06001AF9 RID: 6905 RVA: 0x0005E894 File Offset: 0x0005D894
		public string Fragment
		{
			get
			{
				if (this.IsNotAbsoluteUri)
				{
					throw new InvalidOperationException(SR.GetString("net_uri_NotAbsolute"));
				}
				Uri.UriInfo uriInfo = this.EnsureUriInfo();
				if (uriInfo.MoreInfo == null)
				{
					uriInfo.MoreInfo = new Uri.MoreInfo();
				}
				string text = uriInfo.MoreInfo.Fragment;
				if (text == null)
				{
					text = this.GetParts(UriComponents.Fragment | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
					uriInfo.MoreInfo.Fragment = text;
				}
				return text;
			}
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06001AFA RID: 6906 RVA: 0x0005E8FC File Offset: 0x0005D8FC
		public string Scheme
		{
			get
			{
				if (this.IsNotAbsoluteUri)
				{
					throw new InvalidOperationException(SR.GetString("net_uri_NotAbsolute"));
				}
				return this.m_Syntax.SchemeName;
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06001AFB RID: 6907 RVA: 0x0005E924 File Offset: 0x0005D924
		private bool OriginalStringSwitched
		{
			get
			{
				return (this.m_iriParsing && this.InFact(Uri.Flags.HasUnicode)) || (this.AllowIdn && (this.InFact(Uri.Flags.IdnHost) || this.InFact(Uri.Flags.UnicodeHost)));
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06001AFC RID: 6908 RVA: 0x0005E978 File Offset: 0x0005D978
		public string OriginalString
		{
			get
			{
				if (!this.OriginalStringSwitched)
				{
					return this.m_String;
				}
				return this.m_originalUnicodeString;
			}
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06001AFD RID: 6909 RVA: 0x0005E990 File Offset: 0x0005D990
		public string DnsSafeHost
		{
			get
			{
				if (this.IsNotAbsoluteUri)
				{
					throw new InvalidOperationException(SR.GetString("net_uri_NotAbsolute"));
				}
				if (this.AllowIdn && ((this.m_Flags & Uri.Flags.IdnHost) != Uri.Flags.Zero || (this.m_Flags & Uri.Flags.UnicodeHost) != Uri.Flags.Zero))
				{
					this.EnsureUriInfo();
					return this.m_Info.DnsSafeHost;
				}
				this.EnsureHostString(false);
				string text = this.m_Info.Host;
				if (this.HostType == Uri.Flags.IPv6HostType)
				{
					text = text.Substring(1, text.Length - 2);
					if (this.m_Info.ScopeId != null)
					{
						text += this.m_Info.ScopeId;
					}
				}
				else if (this.HostType == Uri.Flags.BasicHostType && this.InFact(Uri.Flags.HostNotCanonical | Uri.Flags.E_HostNotCanonical))
				{
					char[] array = new char[text.Length];
					int num = 0;
					Uri.UnescapeString(text, 0, text.Length, array, ref num, char.MaxValue, char.MaxValue, char.MaxValue, Uri.UnescapeMode.CopyOnly, this.m_Syntax, false, false);
					text = new string(array, 0, num);
				}
				return text;
			}
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06001AFE RID: 6910 RVA: 0x0005EAA7 File Offset: 0x0005DAA7
		public bool IsAbsoluteUri
		{
			get
			{
				return this.m_Syntax != null;
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06001AFF RID: 6911 RVA: 0x0005EAB8 File Offset: 0x0005DAB8
		public string[] Segments
		{
			get
			{
				if (this.IsNotAbsoluteUri)
				{
					throw new InvalidOperationException(SR.GetString("net_uri_NotAbsolute"));
				}
				string[] array = null;
				if (array == null)
				{
					string privateAbsolutePath = this.PrivateAbsolutePath;
					if (privateAbsolutePath.Length == 0)
					{
						array = new string[0];
					}
					else
					{
						ArrayList arrayList = new ArrayList();
						int num;
						for (int i = 0; i < privateAbsolutePath.Length; i = num + 1)
						{
							num = privateAbsolutePath.IndexOf('/', i);
							if (num == -1)
							{
								num = privateAbsolutePath.Length - 1;
							}
							arrayList.Add(privateAbsolutePath.Substring(i, num - i + 1));
						}
						array = (string[])arrayList.ToArray(typeof(string));
					}
				}
				return array;
			}
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06001B00 RID: 6912 RVA: 0x0005EB57 File Offset: 0x0005DB57
		public bool UserEscaped
		{
			get
			{
				return this.InFact(Uri.Flags.UserEscaped);
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06001B01 RID: 6913 RVA: 0x0005EB65 File Offset: 0x0005DB65
		public string UserInfo
		{
			get
			{
				if (this.IsNotAbsoluteUri)
				{
					throw new InvalidOperationException(SR.GetString("net_uri_NotAbsolute"));
				}
				return this.GetParts(UriComponents.UserInfo, UriFormat.UriEscaped);
			}
		}

		// Token: 0x06001B02 RID: 6914 RVA: 0x0005EB88 File Offset: 0x0005DB88
		public unsafe static UriHostNameType CheckHostName(string name)
		{
			if (name == null || name.Length == 0 || name.Length > 32767)
			{
				return UriHostNameType.Unknown;
			}
			int num = name.Length;
			fixed (char* ptr = name)
			{
				UriHostNameType uriHostNameType;
				if (name[0] == '[' && name[name.Length - 1] == ']' && IPv6AddressHelper.IsValid(ptr, 1, ref num) && num == name.Length)
				{
					uriHostNameType = UriHostNameType.IPv6;
				}
				else
				{
					num = name.Length;
					if (IPv4AddressHelper.IsValid(ptr, 0, ref num, false, false) && num == name.Length)
					{
						uriHostNameType = UriHostNameType.IPv4;
					}
					else
					{
						num = name.Length;
						bool flag = false;
						if (DomainNameHelper.IsValid(ptr, 0, ref num, ref flag, false) && num == name.Length)
						{
							uriHostNameType = UriHostNameType.Dns;
						}
						else
						{
							string text = null;
							num = name.Length + 2;
							name = "[" + name + "]";
							fixed (char* ptr2 = name)
							{
								if (!IPv6AddressHelper.IsValid(ptr2, 1, ref num) || num != name.Length)
								{
									string text2 = null;
									return UriHostNameType.Unknown;
								}
								uriHostNameType = UriHostNameType.IPv6;
							}
						}
					}
				}
				return uriHostNameType;
			}
		}

		// Token: 0x06001B03 RID: 6915 RVA: 0x0005EC94 File Offset: 0x0005DC94
		public static bool CheckSchemeName(string schemeName)
		{
			if (schemeName == null || schemeName.Length == 0 || !Uri.IsAsciiLetter(schemeName[0]))
			{
				return false;
			}
			for (int i = schemeName.Length - 1; i > 0; i--)
			{
				if (!Uri.IsAsciiLetterOrDigit(schemeName[i]) && schemeName[i] != '+' && schemeName[i] != '-' && schemeName[i] != '.')
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001B04 RID: 6916 RVA: 0x0005ED04 File Offset: 0x0005DD04
		public static int FromHex(char digit)
		{
			if ((digit < '0' || digit > '9') && (digit < 'A' || digit > 'F') && (digit < 'a' || digit > 'f'))
			{
				throw new ArgumentException("digit");
			}
			if (digit > '9')
			{
				return (int)(((digit <= 'F') ? (digit - 'A') : (digit - 'a')) + '\n');
			}
			return (int)(digit - '0');
		}

		// Token: 0x06001B05 RID: 6917 RVA: 0x0005ED58 File Offset: 0x0005DD58
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public override int GetHashCode()
		{
			if (this.IsNotAbsoluteUri)
			{
				return Uri.CalculateCaseInsensitiveHashCode(this.OriginalString);
			}
			Uri.UriInfo uriInfo = this.EnsureUriInfo();
			if (uriInfo.MoreInfo == null)
			{
				uriInfo.MoreInfo = new Uri.MoreInfo();
			}
			int num = uriInfo.MoreInfo.Hash;
			if (num == 0)
			{
				string text = uriInfo.MoreInfo.RemoteUrl;
				if (text == null)
				{
					text = this.GetParts(UriComponents.HttpRequestUrl, UriFormat.SafeUnescaped);
				}
				num = Uri.CalculateCaseInsensitiveHashCode(text);
				if (num == 0)
				{
					num = 16777216;
				}
				uriInfo.MoreInfo.Hash = num;
			}
			return num;
		}

		// Token: 0x06001B06 RID: 6918 RVA: 0x0005EDD8 File Offset: 0x0005DDD8
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public override string ToString()
		{
			if (this.m_Syntax != null)
			{
				this.EnsureUriInfo();
				if (this.m_Info.String == null)
				{
					if (this.Syntax.IsSimple)
					{
						this.m_Info.String = this.GetComponentsHelper(UriComponents.AbsoluteUri, (UriFormat)32767);
					}
					else
					{
						this.m_Info.String = this.GetParts(UriComponents.AbsoluteUri, UriFormat.SafeUnescaped);
					}
				}
				return this.m_Info.String;
			}
			if (!this.m_iriParsing || !this.InFact(Uri.Flags.HasUnicode))
			{
				return this.OriginalString;
			}
			return this.m_String;
		}

		// Token: 0x06001B07 RID: 6919 RVA: 0x0005EE6E File Offset: 0x0005DE6E
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public static bool operator ==(Uri uri1, Uri uri2)
		{
			return uri1 == uri2 || (uri1 != null && uri2 != null && uri2.Equals(uri1));
		}

		// Token: 0x06001B08 RID: 6920 RVA: 0x0005EE85 File Offset: 0x0005DE85
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public static bool operator !=(Uri uri1, Uri uri2)
		{
			return uri1 != uri2 && (uri1 == null || uri2 == null || !uri2.Equals(uri1));
		}

		// Token: 0x06001B09 RID: 6921 RVA: 0x0005EEA0 File Offset: 0x0005DEA0
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public unsafe override bool Equals(object comparand)
		{
			if (comparand == null)
			{
				return false;
			}
			if (this == comparand)
			{
				return true;
			}
			Uri uri = comparand as Uri;
			if (uri == null)
			{
				string text = comparand as string;
				if (text == null)
				{
					return false;
				}
				if (!Uri.TryCreate(text, UriKind.RelativeOrAbsolute, out uri))
				{
					return false;
				}
			}
			if (this.m_String == uri.m_String)
			{
				return true;
			}
			if (this.IsAbsoluteUri != uri.IsAbsoluteUri)
			{
				return false;
			}
			if (this.IsNotAbsoluteUri)
			{
				return this.OriginalString.Equals(uri.OriginalString);
			}
			if (this.NotAny((Uri.Flags)(-2147483648)) || uri.NotAny((Uri.Flags)(-2147483648)))
			{
				if (!this.IsUncOrDosPath)
				{
					if (this.m_String.Length == uri.m_String.Length)
					{
						fixed (char* @string = this.m_String)
						{
							fixed (char* string2 = uri.m_String)
							{
								int num = this.m_String.Length - 1;
								while (num >= 0 && @string[num] == string2[num])
								{
									num--;
								}
								if (num == -1)
								{
									return true;
								}
							}
						}
					}
				}
				else if (string.Compare(this.m_String, uri.m_String, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return true;
				}
			}
			this.EnsureUriInfo();
			uri.EnsureUriInfo();
			if (!this.UserDrivenParsing && !uri.UserDrivenParsing && this.Syntax.IsSimple && uri.Syntax.IsSimple)
			{
				if (this.InFact(Uri.Flags.CanonicalDnsHost) && uri.InFact(Uri.Flags.CanonicalDnsHost))
				{
					ushort num2 = this.m_Info.Offset.Host;
					ushort num3 = this.m_Info.Offset.Path;
					ushort num4 = uri.m_Info.Offset.Host;
					ushort path = uri.m_Info.Offset.Path;
					string string3 = uri.m_String;
					if (num3 - num2 > path - num4)
					{
						num3 = num2 + path - num4;
					}
					while (num2 < num3)
					{
						if (this.m_String[(int)num2] != string3[(int)num4])
						{
							return false;
						}
						if (string3[(int)num4] == ':')
						{
							break;
						}
						num2 += 1;
						num4 += 1;
					}
					if (num2 < this.m_Info.Offset.Path && this.m_String[(int)num2] != ':')
					{
						return false;
					}
					if (num4 < path && string3[(int)num4] != ':')
					{
						return false;
					}
				}
				else
				{
					this.EnsureHostString(false);
					uri.EnsureHostString(false);
					if (!this.m_Info.Host.Equals(uri.m_Info.Host))
					{
						return false;
					}
				}
				if (this.Port != uri.Port)
				{
					return false;
				}
			}
			Uri.UriInfo info = this.m_Info;
			Uri.UriInfo info2 = uri.m_Info;
			if (info.MoreInfo == null)
			{
				info.MoreInfo = new Uri.MoreInfo();
			}
			if (info2.MoreInfo == null)
			{
				info2.MoreInfo = new Uri.MoreInfo();
			}
			string text2 = info.MoreInfo.RemoteUrl;
			if (text2 == null)
			{
				text2 = this.GetParts(UriComponents.HttpRequestUrl, UriFormat.SafeUnescaped);
				info.MoreInfo.RemoteUrl = text2;
			}
			string text3 = info2.MoreInfo.RemoteUrl;
			if (text3 == null)
			{
				text3 = uri.GetParts(UriComponents.HttpRequestUrl, UriFormat.SafeUnescaped);
				info2.MoreInfo.RemoteUrl = text3;
			}
			if (this.IsUncOrDosPath)
			{
				return string.Compare(info.MoreInfo.RemoteUrl, info2.MoreInfo.RemoteUrl, this.IsUncOrDosPath ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) == 0;
			}
			if (text2.Length != text3.Length)
			{
				return false;
			}
			fixed (char* ptr = text2)
			{
				fixed (char* ptr2 = text3)
				{
					char* ptr3 = ptr + text2.Length;
					char* ptr4 = ptr2 + text2.Length;
					while (ptr3 != ptr)
					{
						if (*(--ptr3) != *(--ptr4))
						{
							return false;
						}
					}
					return true;
				}
			}
		}

		// Token: 0x06001B0A RID: 6922 RVA: 0x0005F2A4 File Offset: 0x0005E2A4
		public string GetLeftPart(UriPartial part)
		{
			if (this.IsNotAbsoluteUri)
			{
				throw new InvalidOperationException(SR.GetString("net_uri_NotAbsolute"));
			}
			this.EnsureUriInfo();
			switch (part)
			{
			case UriPartial.Scheme:
				return this.GetParts(UriComponents.Scheme | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
			case UriPartial.Authority:
				if (this.NotAny(Uri.Flags.AuthorityFound) || this.IsDosPath)
				{
					return string.Empty;
				}
				return this.GetParts(UriComponents.Scheme | UriComponents.UserInfo | UriComponents.Host | UriComponents.Port, UriFormat.UriEscaped);
			case UriPartial.Path:
				return this.GetParts(UriComponents.Scheme | UriComponents.UserInfo | UriComponents.Host | UriComponents.Port | UriComponents.Path, UriFormat.UriEscaped);
			case UriPartial.Query:
				return this.GetParts(UriComponents.Scheme | UriComponents.UserInfo | UriComponents.Host | UriComponents.Port | UriComponents.Path | UriComponents.Query, UriFormat.UriEscaped);
			default:
				throw new ArgumentException("part");
			}
		}

		// Token: 0x06001B0B RID: 6923 RVA: 0x0005F33C File Offset: 0x0005E33C
		public static string HexEscape(char character)
		{
			if (character > 'ÿ')
			{
				throw new ArgumentOutOfRangeException("character");
			}
			char[] array = new char[3];
			int num = 0;
			Uri.EscapeAsciiChar(character, array, ref num);
			return new string(array);
		}

		// Token: 0x06001B0C RID: 6924 RVA: 0x0005F374 File Offset: 0x0005E374
		public static char HexUnescape(string pattern, ref int index)
		{
			if (index < 0 || index >= pattern.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (pattern[index] == '%' && pattern.Length - index >= 3)
			{
				char c = Uri.EscapedAscii(pattern[index + 1], pattern[index + 2]);
				if (c != '\uffff')
				{
					index += 3;
					return c;
				}
			}
			return pattern[index++];
		}

		// Token: 0x06001B0D RID: 6925 RVA: 0x0005F3EC File Offset: 0x0005E3EC
		public static bool IsHexDigit(char character)
		{
			return (character >= '0' && character <= '9') || (character >= 'A' && character <= 'F') || (character >= 'a' && character <= 'f');
		}

		// Token: 0x06001B0E RID: 6926 RVA: 0x0005F413 File Offset: 0x0005E413
		public static bool IsHexEncoding(string pattern, int index)
		{
			return pattern.Length - index >= 3 && (pattern[index] == '%' && Uri.EscapedAscii(pattern[index + 1], pattern[index + 1]) != char.MaxValue);
		}

		// Token: 0x06001B0F RID: 6927 RVA: 0x0005F450 File Offset: 0x0005E450
		[Obsolete("The method has been deprecated. Please use MakeRelativeUri(Uri uri). http://go.microsoft.com/fwlink/?linkid=14202")]
		public string MakeRelative(Uri toUri)
		{
			if (this.IsNotAbsoluteUri || toUri.IsNotAbsoluteUri)
			{
				throw new InvalidOperationException(SR.GetString("net_uri_NotAbsolute"));
			}
			if (this.Scheme == toUri.Scheme && this.Host == toUri.Host && this.Port == toUri.Port)
			{
				return Uri.PathDifference(this.AbsolutePath, toUri.AbsolutePath, !this.IsUncOrDosPath);
			}
			return toUri.ToString();
		}

		// Token: 0x06001B10 RID: 6928 RVA: 0x0005F4D4 File Offset: 0x0005E4D4
		public Uri MakeRelativeUri(Uri uri)
		{
			if (this.IsNotAbsoluteUri || uri.IsNotAbsoluteUri)
			{
				throw new InvalidOperationException(SR.GetString("net_uri_NotAbsolute"));
			}
			if (this.Scheme == uri.Scheme && this.Host == uri.Host && this.Port == uri.Port)
			{
				return new Uri(Uri.PathDifference(this.AbsolutePath, uri.AbsolutePath, !this.IsUncOrDosPath) + uri.GetParts(UriComponents.Query | UriComponents.Fragment, UriFormat.UriEscaped), UriKind.Relative);
			}
			return uri;
		}

		// Token: 0x06001B11 RID: 6929 RVA: 0x0005F568 File Offset: 0x0005E568
		private unsafe static bool TestForSubPath(char* pMe, ushort meLength, char* pShe, ushort sheLength, bool ignoreCase)
		{
			ushort num = 0;
			bool flag = true;
			while (num < meLength)
			{
				if (num >= sheLength)
				{
					break;
				}
				char c = pMe[num];
				char c2 = pShe[num];
				if (c == '?' || c == '#')
				{
					return true;
				}
				if (c == '/')
				{
					if (c2 != '/')
					{
						return false;
					}
					if (!flag)
					{
						return false;
					}
					flag = true;
				}
				else
				{
					if (c2 == '?' || c2 == '#')
					{
						break;
					}
					if (!ignoreCase)
					{
						if (c != c2)
						{
							flag = false;
						}
					}
					else if (char.ToLower(c, CultureInfo.InvariantCulture) != char.ToLower(c2, CultureInfo.InvariantCulture))
					{
						flag = false;
					}
				}
				num += 1;
			}
			while (num < meLength)
			{
				char c;
				if ((c = pMe[num]) == '?' || c == '#')
				{
					return true;
				}
				if (c == '/')
				{
					return false;
				}
				num += 1;
			}
			return true;
		}

		// Token: 0x06001B12 RID: 6930 RVA: 0x0005F610 File Offset: 0x0005E610
		internal static string InternalEscapeString(string rawString)
		{
			if (rawString == null)
			{
				return string.Empty;
			}
			int num = 0;
			char[] array = Uri.EscapeString(rawString, 0, rawString.Length, null, ref num, true, '?', '#', '%');
			if (array == null)
			{
				return rawString;
			}
			return new string(array, 0, num);
		}

		// Token: 0x06001B13 RID: 6931 RVA: 0x0005F650 File Offset: 0x0005E650
		private unsafe static Uri.ParsingError ParseScheme(string uriString, ref Uri.Flags flags, ref UriParser syntax)
		{
			int length = uriString.Length;
			if (length == 0)
			{
				return Uri.ParsingError.EmptyUriString;
			}
			if (length >= 65520)
			{
				return Uri.ParsingError.SizeLimit;
			}
			fixed (char* ptr = uriString)
			{
				Uri.ParsingError parsingError = Uri.ParsingError.None;
				ushort num = Uri.ParseSchemeCheckImplicitFile(ptr, (ushort)length, ref parsingError, ref flags, ref syntax);
				if (parsingError != Uri.ParsingError.None)
				{
					return parsingError;
				}
				flags |= (Uri.Flags)num;
			}
			return Uri.ParsingError.None;
		}

		// Token: 0x06001B14 RID: 6932 RVA: 0x0005F6A8 File Offset: 0x0005E6A8
		internal UriFormatException ParseMinimal()
		{
			Uri.ParsingError parsingError = this.PrivateParseMinimal();
			if (parsingError == Uri.ParsingError.None)
			{
				return null;
			}
			this.m_Flags |= Uri.Flags.ErrorOrParsingRecursion;
			return Uri.GetException(parsingError);
		}

		// Token: 0x06001B15 RID: 6933 RVA: 0x0005F6DC File Offset: 0x0005E6DC
		private unsafe Uri.ParsingError PrivateParseMinimal()
		{
			ushort num = (ushort)(this.m_Flags & Uri.Flags.IndexMask);
			ushort num2 = (ushort)this.m_String.Length;
			string text = null;
			this.m_Flags &= ~(Uri.Flags.SchemeNotCanonical | Uri.Flags.UserNotCanonical | Uri.Flags.HostNotCanonical | Uri.Flags.PortNotCanonical | Uri.Flags.PathNotCanonical | Uri.Flags.QueryNotCanonical | Uri.Flags.FragmentNotCanonical | Uri.Flags.E_UserNotCanonical | Uri.Flags.E_HostNotCanonical | Uri.Flags.E_PortNotCanonical | Uri.Flags.E_PathNotCanonical | Uri.Flags.E_QueryNotCanonical | Uri.Flags.E_FragmentNotCanonical | Uri.Flags.ShouldBeCompressed | Uri.Flags.FirstSlashAbsent | Uri.Flags.BackslashInPath | Uri.Flags.UserDrivenParsing);
			fixed (char* ptr = ((this.m_iriParsing && (this.m_Flags & Uri.Flags.HasUnicode) != Uri.Flags.Zero && (this.m_Flags & Uri.Flags.HostUnicodeNormalized) == Uri.Flags.Zero) ? this.m_originalUnicodeString : this.m_String))
			{
				if (num2 > num && Uri.IsLWS(ptr[num2 - 1]))
				{
					num2 -= 1;
					while (num2 != num && Uri.IsLWS(ptr[(IntPtr)(num2 -= 1) * 2]))
					{
					}
					num2 += 1;
				}
				if (this.m_Syntax.IsAllSet(UriSyntaxFlags.AllowEmptyHost | UriSyntaxFlags.AllowDOSPath) && this.NotAny(Uri.Flags.ImplicitFile) && num + 1 < num2)
				{
					ushort num3 = num;
					char c;
					while (num3 < num2 && ((c = ptr[num3]) == '\\' || c == '/'))
					{
						num3 += 1;
					}
					if (this.m_Syntax.InFact(UriSyntaxFlags.FileLikeUri) || num3 - num <= 3)
					{
						if (num3 - num >= 2)
						{
							this.m_Flags |= Uri.Flags.AuthorityFound;
						}
						if (num3 + 1 < num2 && ((c = ptr[num3 + 1]) == ':' || c == '|') && Uri.IsAsciiLetter(ptr[num3]))
						{
							if (num3 + 2 >= num2 || ((c = ptr[num3 + 2]) != '\\' && c != '/'))
							{
								if (this.m_Syntax.InFact(UriSyntaxFlags.FileLikeUri))
								{
									return Uri.ParsingError.MustRootedPath;
								}
							}
							else
							{
								this.m_Flags |= Uri.Flags.DosPath;
								if (this.m_Syntax.InFact(UriSyntaxFlags.MustHaveAuthority))
								{
									this.m_Flags |= Uri.Flags.AuthorityFound;
								}
								if (num3 != num && num3 - num != 2)
								{
									num = num3 - 1;
								}
								else
								{
									num = num3;
								}
							}
						}
						else if (this.m_Syntax.InFact(UriSyntaxFlags.FileLikeUri) && num3 - num >= 2 && num3 - num != 3 && num3 < num2 && ptr[num3] != '?' && ptr[num3] != '#')
						{
							this.m_Flags |= Uri.Flags.UncPath;
							num = num3;
						}
					}
				}
				if ((this.m_Flags & (Uri.Flags.DosPath | Uri.Flags.UncPath)) == Uri.Flags.Zero)
				{
					if (num + 2 <= num2)
					{
						char c2 = ptr[num];
						char c3 = ptr[num + 1];
						if (this.m_Syntax.InFact(UriSyntaxFlags.MustHaveAuthority))
						{
							if ((c2 != '/' && c2 != '\\') || (c3 != '/' && c3 != '\\'))
							{
								return Uri.ParsingError.BadAuthority;
							}
							this.m_Flags |= Uri.Flags.AuthorityFound;
							num += 2;
						}
						else if (this.m_Syntax.InFact(UriSyntaxFlags.OptionalAuthority) && (this.InFact(Uri.Flags.AuthorityFound) || (c2 == '/' && c3 == '/')))
						{
							this.m_Flags |= Uri.Flags.AuthorityFound;
							num += 2;
						}
						else if (this.m_Syntax.NotAny(UriSyntaxFlags.MailToLikeUri))
						{
							this.m_Flags |= (Uri.Flags)num | Uri.Flags.HostTypeMask;
							return Uri.ParsingError.None;
						}
					}
					else
					{
						if (this.m_Syntax.InFact(UriSyntaxFlags.MustHaveAuthority))
						{
							return Uri.ParsingError.BadAuthority;
						}
						if (this.m_Syntax.NotAny(UriSyntaxFlags.MailToLikeUri))
						{
							this.m_Flags |= (Uri.Flags)num | Uri.Flags.HostTypeMask;
							return Uri.ParsingError.None;
						}
					}
				}
				Uri.ParsingError parsingError;
				if (this.InFact(Uri.Flags.DosPath))
				{
					this.m_Flags |= (((this.m_Flags & Uri.Flags.AuthorityFound) != Uri.Flags.Zero) ? Uri.Flags.BasicHostType : Uri.Flags.HostTypeMask);
					this.m_Flags |= (Uri.Flags)num;
					parsingError = Uri.ParsingError.None;
				}
				else
				{
					Uri.ParsingError parsingError2 = Uri.ParsingError.None;
					num = this.CheckAuthorityHelper(ptr, num, num2, ref parsingError2, ref this.m_Flags, this.m_Syntax, ref text);
					if (parsingError2 != Uri.ParsingError.None)
					{
						parsingError = parsingError2;
					}
					else
					{
						if (num >= num2 || ptr[num] != '\\' || !this.NotAny(Uri.Flags.ImplicitFile) || !this.m_Syntax.NotAny(UriSyntaxFlags.AllowDOSPath))
						{
							this.m_Flags |= (Uri.Flags)num;
							string text2 = null;
							if (Uri.s_IdnScope != UriIdnScope.None || this.m_iriParsing)
							{
								this.PrivateParseMinimalIri(text, num);
							}
							return Uri.ParsingError.None;
						}
						parsingError = Uri.ParsingError.BadAuthorityTerminator;
					}
				}
				return parsingError;
			}
		}

		// Token: 0x06001B16 RID: 6934 RVA: 0x0005FB48 File Offset: 0x0005EB48
		private void PrivateParseMinimalIri(string newHost, ushort idx)
		{
			if (newHost != null)
			{
				this.m_String = newHost;
			}
			if ((!this.m_iriParsing && this.AllowIdn && ((this.m_Flags & Uri.Flags.IdnHost) != Uri.Flags.Zero || (this.m_Flags & Uri.Flags.UnicodeHost) != Uri.Flags.Zero)) || (this.m_iriParsing && (this.m_Flags & Uri.Flags.HasUnicode) == Uri.Flags.Zero && this.AllowIdn && (this.m_Flags & Uri.Flags.IdnHost) != Uri.Flags.Zero))
			{
				this.m_Flags &= ~(Uri.Flags.SchemeNotCanonical | Uri.Flags.UserNotCanonical | Uri.Flags.HostNotCanonical | Uri.Flags.PortNotCanonical | Uri.Flags.PathNotCanonical | Uri.Flags.QueryNotCanonical | Uri.Flags.FragmentNotCanonical | Uri.Flags.E_UserNotCanonical | Uri.Flags.E_HostNotCanonical | Uri.Flags.E_PortNotCanonical | Uri.Flags.E_PathNotCanonical | Uri.Flags.E_QueryNotCanonical | Uri.Flags.E_FragmentNotCanonical | Uri.Flags.ShouldBeCompressed | Uri.Flags.FirstSlashAbsent | Uri.Flags.BackslashInPath);
				this.m_Flags |= (Uri.Flags)((long)this.m_String.Length);
				this.m_String += this.m_originalUnicodeString.Substring((int)idx, this.m_originalUnicodeString.Length - (int)idx);
			}
			if (this.m_iriParsing && (this.m_Flags & Uri.Flags.HasUnicode) != Uri.Flags.Zero)
			{
				this.m_Flags |= Uri.Flags.UseOrigUncdStrOffset;
			}
		}

		// Token: 0x06001B17 RID: 6935 RVA: 0x0005FC5C File Offset: 0x0005EC5C
		private unsafe void CreateUriInfo(Uri.Flags cF)
		{
			Uri.UriInfo uriInfo = new Uri.UriInfo();
			uriInfo.Offset.End = (ushort)this.m_String.Length;
			if (!this.UserDrivenParsing)
			{
				bool flag = false;
				ushort num;
				if ((cF & Uri.Flags.ImplicitFile) != Uri.Flags.Zero)
				{
					num = 0;
					while (Uri.IsLWS(this.m_String[(int)num]))
					{
						num += 1;
						Uri.UriInfo uriInfo2 = uriInfo;
						uriInfo2.Offset.Scheme = uriInfo2.Offset.Scheme + 1;
					}
					if (Uri.StaticInFact(cF, Uri.Flags.UncPath))
					{
						for (num += 2; num < (ushort)(cF & Uri.Flags.IndexMask); num += 1)
						{
							if (this.m_String[(int)num] != '/' && this.m_String[(int)num] != '\\')
							{
								break;
							}
						}
					}
				}
				else
				{
					num = (ushort)this.m_Syntax.SchemeName.Length;
					for (;;)
					{
						string @string = this.m_String;
						ushort num2 = num;
						num = num2 + 1;
						if (@string[(int)num2] == ':')
						{
							break;
						}
						Uri.UriInfo uriInfo3 = uriInfo;
						uriInfo3.Offset.Scheme = uriInfo3.Offset.Scheme + 1;
					}
					if ((cF & Uri.Flags.AuthorityFound) != Uri.Flags.Zero)
					{
						if (this.m_String[(int)num] == '\\' || this.m_String[(int)(num + 1)] == '\\')
						{
							flag = true;
						}
						num += 2;
						if ((cF & (Uri.Flags.DosPath | Uri.Flags.UncPath)) != Uri.Flags.Zero)
						{
							while (num < (ushort)(cF & Uri.Flags.IndexMask) && (this.m_String[(int)num] == '/' || this.m_String[(int)num] == '\\'))
							{
								flag = true;
								num += 1;
							}
						}
					}
				}
				if (this.m_Syntax.DefaultPort != -1)
				{
					uriInfo.Offset.PortValue = (ushort)this.m_Syntax.DefaultPort;
				}
				if ((cF & Uri.Flags.HostTypeMask) == Uri.Flags.HostTypeMask || Uri.StaticInFact(cF, Uri.Flags.DosPath))
				{
					uriInfo.Offset.User = (ushort)(cF & Uri.Flags.IndexMask);
					uriInfo.Offset.Host = uriInfo.Offset.User;
					uriInfo.Offset.Path = uriInfo.Offset.User;
					cF &= ~(Uri.Flags.SchemeNotCanonical | Uri.Flags.UserNotCanonical | Uri.Flags.HostNotCanonical | Uri.Flags.PortNotCanonical | Uri.Flags.PathNotCanonical | Uri.Flags.QueryNotCanonical | Uri.Flags.FragmentNotCanonical | Uri.Flags.E_UserNotCanonical | Uri.Flags.E_HostNotCanonical | Uri.Flags.E_PortNotCanonical | Uri.Flags.E_PathNotCanonical | Uri.Flags.E_QueryNotCanonical | Uri.Flags.E_FragmentNotCanonical | Uri.Flags.ShouldBeCompressed | Uri.Flags.FirstSlashAbsent | Uri.Flags.BackslashInPath);
					if (flag)
					{
						cF |= Uri.Flags.SchemeNotCanonical;
					}
				}
				else
				{
					uriInfo.Offset.User = num;
					if (this.HostType == Uri.Flags.BasicHostType)
					{
						uriInfo.Offset.Host = num;
						uriInfo.Offset.Path = (ushort)(cF & Uri.Flags.IndexMask);
						cF &= ~(Uri.Flags.SchemeNotCanonical | Uri.Flags.UserNotCanonical | Uri.Flags.HostNotCanonical | Uri.Flags.PortNotCanonical | Uri.Flags.PathNotCanonical | Uri.Flags.QueryNotCanonical | Uri.Flags.FragmentNotCanonical | Uri.Flags.E_UserNotCanonical | Uri.Flags.E_HostNotCanonical | Uri.Flags.E_PortNotCanonical | Uri.Flags.E_PathNotCanonical | Uri.Flags.E_QueryNotCanonical | Uri.Flags.E_FragmentNotCanonical | Uri.Flags.ShouldBeCompressed | Uri.Flags.FirstSlashAbsent | Uri.Flags.BackslashInPath);
					}
					else
					{
						if ((cF & Uri.Flags.HasUserInfo) != Uri.Flags.Zero)
						{
							while (this.m_String[(int)num] != '@')
							{
								num += 1;
							}
							num += 1;
							uriInfo.Offset.Host = num;
						}
						else
						{
							uriInfo.Offset.Host = num;
						}
						num = (ushort)(cF & Uri.Flags.IndexMask);
						cF &= ~(Uri.Flags.SchemeNotCanonical | Uri.Flags.UserNotCanonical | Uri.Flags.HostNotCanonical | Uri.Flags.PortNotCanonical | Uri.Flags.PathNotCanonical | Uri.Flags.QueryNotCanonical | Uri.Flags.FragmentNotCanonical | Uri.Flags.E_UserNotCanonical | Uri.Flags.E_HostNotCanonical | Uri.Flags.E_PortNotCanonical | Uri.Flags.E_PathNotCanonical | Uri.Flags.E_QueryNotCanonical | Uri.Flags.E_FragmentNotCanonical | Uri.Flags.ShouldBeCompressed | Uri.Flags.FirstSlashAbsent | Uri.Flags.BackslashInPath);
						if (flag)
						{
							cF |= Uri.Flags.SchemeNotCanonical;
						}
						uriInfo.Offset.Path = num;
						bool flag2 = false;
						bool flag3 = (cF & Uri.Flags.UseOrigUncdStrOffset) != Uri.Flags.Zero;
						cF &= ~Uri.Flags.UseOrigUncdStrOffset;
						if (flag3)
						{
							uriInfo.Offset.End = (ushort)this.m_originalUnicodeString.Length;
						}
						if (num < uriInfo.Offset.End)
						{
							fixed (char* ptr = (flag3 ? this.m_originalUnicodeString : this.m_String))
							{
								if (ptr[num] == ':')
								{
									int num3 = 0;
									if ((num += 1) < uriInfo.Offset.End)
									{
										num3 = (int)((ushort)(ptr[num] - '0'));
										if (num3 != 65535 && num3 != 15 && num3 != 65523)
										{
											flag2 = true;
											if (num3 == 0)
											{
												cF |= Uri.Flags.PortNotCanonical | Uri.Flags.E_PortNotCanonical;
											}
											for (num += 1; num < uriInfo.Offset.End; num += 1)
											{
												ushort num4 = (ushort)(ptr[num] - '0');
												if (num4 == 65535 || num4 == 15 || num4 == 65523)
												{
													break;
												}
												num3 = num3 * 10 + (int)num4;
											}
										}
									}
									if (flag2 && uriInfo.Offset.PortValue != (ushort)num3)
									{
										uriInfo.Offset.PortValue = (ushort)num3;
										cF |= Uri.Flags.NotDefaultPort;
									}
									else
									{
										cF |= Uri.Flags.PortNotCanonical | Uri.Flags.E_PortNotCanonical;
									}
									uriInfo.Offset.Path = num;
								}
							}
						}
					}
				}
			}
			cF |= Uri.Flags.MinimalUriInfoSet;
			uriInfo.DnsSafeHost = this.m_DnsSafeHost;
			lock (this.m_String)
			{
				if ((this.m_Flags & Uri.Flags.MinimalUriInfoSet) == Uri.Flags.Zero)
				{
					this.m_Info = uriInfo;
					this.m_Flags = (this.m_Flags & ~(Uri.Flags.SchemeNotCanonical | Uri.Flags.UserNotCanonical | Uri.Flags.HostNotCanonical | Uri.Flags.PortNotCanonical | Uri.Flags.PathNotCanonical | Uri.Flags.QueryNotCanonical | Uri.Flags.FragmentNotCanonical | Uri.Flags.E_UserNotCanonical | Uri.Flags.E_HostNotCanonical | Uri.Flags.E_PortNotCanonical | Uri.Flags.E_PathNotCanonical | Uri.Flags.E_QueryNotCanonical | Uri.Flags.E_FragmentNotCanonical | Uri.Flags.ShouldBeCompressed | Uri.Flags.FirstSlashAbsent | Uri.Flags.BackslashInPath)) | cF;
				}
			}
		}

		// Token: 0x06001B18 RID: 6936 RVA: 0x000600F4 File Offset: 0x0005F0F4
		private unsafe void CreateHostString()
		{
			if (!this.m_Syntax.IsSimple)
			{
				lock (this.m_Info)
				{
					if (this.NotAny(Uri.Flags.ErrorOrParsingRecursion))
					{
						this.m_Flags |= Uri.Flags.ErrorOrParsingRecursion;
						this.GetHostViaCustomSyntax();
						this.m_Flags &= ~Uri.Flags.ErrorOrParsingRecursion;
						return;
					}
				}
			}
			Uri.Flags flags = this.m_Flags;
			string text = Uri.CreateHostStringHelper(this.m_String, this.m_Info.Offset.Host, this.m_Info.Offset.Path, ref flags, ref this.m_Info.ScopeId);
			if (text.Length != 0)
			{
				if (this.HostType == Uri.Flags.BasicHostType)
				{
					ushort num = 0;
					Uri.Check check;
					fixed (char* ptr = text)
					{
						check = this.CheckCanonical(ptr, ref num, (ushort)text.Length, char.MaxValue);
					}
					if ((check & Uri.Check.DisplayCanonical) == Uri.Check.None && (this.NotAny(Uri.Flags.ImplicitFile) || (check & Uri.Check.ReservedFound) != Uri.Check.None))
					{
						flags |= Uri.Flags.HostNotCanonical;
					}
					if (this.InFact(Uri.Flags.ImplicitFile) && (check & (Uri.Check.EscapedCanonical | Uri.Check.ReservedFound)) != Uri.Check.None)
					{
						check &= ~Uri.Check.EscapedCanonical;
					}
					if ((check & (Uri.Check.EscapedCanonical | Uri.Check.BackslashInPath)) != Uri.Check.EscapedCanonical)
					{
						flags |= Uri.Flags.E_HostNotCanonical;
						if (this.NotAny(Uri.Flags.UserEscaped))
						{
							int num2 = 0;
							char[] array = Uri.EscapeString(text, 0, text.Length, null, ref num2, true, '?', '#', this.IsImplicitFile ? char.MaxValue : '%');
							if (array != null)
							{
								text = new string(array, 0, num2);
							}
						}
					}
				}
				else if (this.NotAny(Uri.Flags.CanonicalDnsHost))
				{
					if (this.m_Info.ScopeId != null)
					{
						flags |= Uri.Flags.HostNotCanonical | Uri.Flags.E_HostNotCanonical;
					}
					else
					{
						for (int i = 0; i < text.Length; i++)
						{
							if ((int)this.m_Info.Offset.Host + i >= (int)this.m_Info.Offset.End || text[i] != this.m_String[(int)this.m_Info.Offset.Host + i])
							{
								flags |= Uri.Flags.HostNotCanonical | Uri.Flags.E_HostNotCanonical;
								break;
							}
						}
					}
				}
			}
			this.m_Info.Host = text;
			lock (this.m_Info)
			{
				this.m_Flags |= flags;
			}
		}

		// Token: 0x06001B19 RID: 6937 RVA: 0x00060370 File Offset: 0x0005F370
		private static string CreateHostStringHelper(string str, ushort idx, ushort end, ref Uri.Flags flags, ref string scopeId)
		{
			bool flag = false;
			Uri.Flags flags2 = flags & Uri.Flags.HostTypeMask;
			string text;
			if (flags2 <= Uri.Flags.DnsHostType)
			{
				if (flags2 == Uri.Flags.IPv6HostType)
				{
					text = IPv6AddressHelper.ParseCanonicalName(str, (int)idx, ref flag, ref scopeId);
					goto IL_00C4;
				}
				if (flags2 == Uri.Flags.IPv4HostType)
				{
					text = IPv4AddressHelper.ParseCanonicalName(str, (int)idx, (int)end, ref flag);
					goto IL_00C4;
				}
				if (flags2 == Uri.Flags.DnsHostType)
				{
					text = DomainNameHelper.ParseCanonicalName(str, (int)idx, (int)end, ref flag);
					goto IL_00C4;
				}
			}
			else
			{
				if (flags2 == Uri.Flags.UncHostType)
				{
					text = UncNameHelper.ParseCanonicalName(str, (int)idx, (int)end, ref flag);
					goto IL_00C4;
				}
				if (flags2 != Uri.Flags.BasicHostType)
				{
					if (flags2 == Uri.Flags.HostTypeMask)
					{
						text = string.Empty;
						goto IL_00C4;
					}
				}
				else
				{
					if (Uri.StaticInFact(flags, Uri.Flags.DosPath))
					{
						text = string.Empty;
					}
					else
					{
						text = str.Substring((int)idx, (int)(end - idx));
					}
					if (text.Length == 0)
					{
						flag = true;
						goto IL_00C4;
					}
					goto IL_00C4;
				}
			}
			throw Uri.GetException(Uri.ParsingError.BadHostName);
			IL_00C4:
			if (flag)
			{
				flags |= Uri.Flags.LoopbackHost;
			}
			return text;
		}

		// Token: 0x06001B1A RID: 6938 RVA: 0x00060450 File Offset: 0x0005F450
		private unsafe void GetHostViaCustomSyntax()
		{
			if (this.m_Info.Host != null)
			{
				return;
			}
			string text = this.m_Syntax.InternalGetComponents(this, UriComponents.Host, UriFormat.UriEscaped);
			if (this.m_Info.Host == null)
			{
				if (text.Length >= 65520)
				{
					throw Uri.GetException(Uri.ParsingError.SizeLimit);
				}
				Uri.ParsingError parsingError = Uri.ParsingError.None;
				Uri.Flags flags = this.m_Flags & ~Uri.Flags.HostTypeMask;
				fixed (char* ptr = text)
				{
					string text2 = null;
					if (this.CheckAuthorityHelper(ptr, 0, (ushort)text.Length, ref parsingError, ref flags, this.m_Syntax, ref text2) != (ushort)text.Length)
					{
						flags &= ~Uri.Flags.HostTypeMask;
						flags |= Uri.Flags.HostTypeMask;
					}
				}
				if (parsingError != Uri.ParsingError.None || (flags & Uri.Flags.HostTypeMask) == Uri.Flags.HostTypeMask)
				{
					this.m_Flags = (this.m_Flags & ~Uri.Flags.HostTypeMask) | Uri.Flags.BasicHostType;
				}
				else
				{
					text = Uri.CreateHostStringHelper(text, 0, (ushort)text.Length, ref flags, ref this.m_Info.ScopeId);
					for (int i = 0; i < text.Length; i++)
					{
						if ((int)this.m_Info.Offset.Host + i >= (int)this.m_Info.Offset.End || text[i] != this.m_String[(int)this.m_Info.Offset.Host + i])
						{
							this.m_Flags |= Uri.Flags.HostNotCanonical | Uri.Flags.E_HostNotCanonical;
							break;
						}
					}
					this.m_Flags = (this.m_Flags & ~Uri.Flags.HostTypeMask) | (flags & Uri.Flags.HostTypeMask);
				}
			}
			string text3 = this.m_Syntax.InternalGetComponents(this, UriComponents.StrongPort, UriFormat.UriEscaped);
			int num = 0;
			if (text3 == null || text3.Length == 0)
			{
				this.m_Flags &= ~Uri.Flags.NotDefaultPort;
				this.m_Flags |= Uri.Flags.PortNotCanonical | Uri.Flags.E_PortNotCanonical;
				this.m_Info.Offset.PortValue = 0;
			}
			else
			{
				for (int j = 0; j < text3.Length; j++)
				{
					int num2 = (int)(text3[j] - '0');
					if (num2 < 0 || num2 > 9 || (num = num * 10 + num2) > 65535)
					{
						throw new UriFormatException(SR.GetString("net_uri_PortOutOfRange", new object[]
						{
							this.m_Syntax.GetType().FullName,
							text3
						}));
					}
				}
				if (num != (int)this.m_Info.Offset.PortValue)
				{
					if (num == this.m_Syntax.DefaultPort)
					{
						this.m_Flags &= ~Uri.Flags.NotDefaultPort;
					}
					else
					{
						this.m_Flags |= Uri.Flags.NotDefaultPort;
					}
					this.m_Flags |= Uri.Flags.PortNotCanonical | Uri.Flags.E_PortNotCanonical;
					this.m_Info.Offset.PortValue = (ushort)num;
				}
			}
			this.m_Info.Host = text;
		}

		// Token: 0x06001B1B RID: 6939 RVA: 0x0006072D File Offset: 0x0005F72D
		internal string GetParts(UriComponents uriParts, UriFormat formatAs)
		{
			return this.GetComponents(uriParts, formatAs);
		}

		// Token: 0x06001B1C RID: 6940 RVA: 0x00060738 File Offset: 0x0005F738
		private string GetEscapedParts(UriComponents uriParts)
		{
			ushort num = (ushort)(((ushort)this.m_Flags & 16256) >> 6);
			if (this.InFact(Uri.Flags.SchemeNotCanonical))
			{
				num |= 1;
			}
			if ((uriParts & UriComponents.Path) != (UriComponents)0)
			{
				if (this.InFact(Uri.Flags.ShouldBeCompressed | Uri.Flags.FirstSlashAbsent | Uri.Flags.BackslashInPath))
				{
					num |= 16;
				}
				else if (this.IsDosPath && this.m_String[(int)(this.m_Info.Offset.Path + this.SecuredPathIndex - 1)] == '|')
				{
					num |= 16;
				}
			}
			if (((ushort)uriParts & num) == 0)
			{
				string uriPartsFromUserString = this.GetUriPartsFromUserString(uriParts);
				if (uriPartsFromUserString != null)
				{
					return uriPartsFromUserString;
				}
			}
			return this.ReCreateParts(uriParts, num, UriFormat.UriEscaped);
		}

		// Token: 0x06001B1D RID: 6941 RVA: 0x000607D4 File Offset: 0x0005F7D4
		private string GetUnescapedParts(UriComponents uriParts, UriFormat formatAs)
		{
			ushort num = (ushort)this.m_Flags & 127;
			if ((uriParts & UriComponents.Path) != (UriComponents)0)
			{
				if ((this.m_Flags & (Uri.Flags.ShouldBeCompressed | Uri.Flags.FirstSlashAbsent | Uri.Flags.BackslashInPath)) != Uri.Flags.Zero)
				{
					num |= 16;
				}
				else if (this.IsDosPath && this.m_String[(int)(this.m_Info.Offset.Path + this.SecuredPathIndex - 1)] == '|')
				{
					num |= 16;
				}
			}
			if (((ushort)uriParts & num) == 0)
			{
				string uriPartsFromUserString = this.GetUriPartsFromUserString(uriParts);
				if (uriPartsFromUserString != null)
				{
					return uriPartsFromUserString;
				}
			}
			return this.ReCreateParts(uriParts, num, formatAs);
		}

		// Token: 0x06001B1E RID: 6942 RVA: 0x00060860 File Offset: 0x0005F860
		private string ReCreateParts(UriComponents parts, ushort nonCanonical, UriFormat formatAs)
		{
			this.EnsureHostString(false);
			string text = (((parts & UriComponents.Host) == (UriComponents)0) ? string.Empty : this.m_Info.Host);
			int num = (int)((this.m_Info.Offset.End - this.m_Info.Offset.User) * ((formatAs == UriFormat.UriEscaped) ? 12 : 1));
			char[] array = new char[text.Length + num + this.m_Syntax.SchemeName.Length + 3 + 1];
			num = 0;
			if ((parts & UriComponents.Scheme) != (UriComponents)0)
			{
				this.m_Syntax.SchemeName.CopyTo(0, array, num, this.m_Syntax.SchemeName.Length);
				num += this.m_Syntax.SchemeName.Length;
				if (parts != UriComponents.Scheme)
				{
					array[num++] = ':';
					if (this.InFact(Uri.Flags.AuthorityFound))
					{
						array[num++] = '/';
						array[num++] = '/';
					}
				}
			}
			if ((parts & UriComponents.UserInfo) != (UriComponents)0 && this.InFact(Uri.Flags.HasUserInfo))
			{
				if ((nonCanonical & 2) != 0)
				{
					switch (formatAs)
					{
					case UriFormat.UriEscaped:
						if (this.NotAny(Uri.Flags.UserEscaped))
						{
							array = Uri.EscapeString(this.m_String, (int)this.m_Info.Offset.User, (int)this.m_Info.Offset.Host, array, ref num, true, '?', '#', '%');
						}
						else
						{
							this.InFact(Uri.Flags.E_UserNotCanonical);
							this.m_String.CopyTo((int)this.m_Info.Offset.User, array, num, (int)(this.m_Info.Offset.Host - this.m_Info.Offset.User));
							num += (int)(this.m_Info.Offset.Host - this.m_Info.Offset.User);
						}
						break;
					case UriFormat.Unescaped:
						array = Uri.UnescapeString(this.m_String, (int)this.m_Info.Offset.User, (int)this.m_Info.Offset.Host, array, ref num, char.MaxValue, char.MaxValue, char.MaxValue, Uri.UnescapeMode.Unescape | Uri.UnescapeMode.UnescapeAll, this.m_Syntax, false, false);
						break;
					case UriFormat.SafeUnescaped:
						array = Uri.UnescapeString(this.m_String, (int)this.m_Info.Offset.User, (int)(this.m_Info.Offset.Host - 1), array, ref num, '@', '/', '\\', this.InFact(Uri.Flags.UserEscaped) ? Uri.UnescapeMode.Unescape : Uri.UnescapeMode.EscapeUnescape, this.m_Syntax, false, false);
						array[num++] = '@';
						break;
					default:
						array = Uri.UnescapeString(this.m_String, (int)this.m_Info.Offset.User, (int)this.m_Info.Offset.Host, array, ref num, char.MaxValue, char.MaxValue, char.MaxValue, Uri.UnescapeMode.CopyOnly, this.m_Syntax, false, false);
						break;
					}
				}
				else
				{
					Uri.UnescapeString(this.m_String, (int)this.m_Info.Offset.User, (int)this.m_Info.Offset.Host, array, ref num, char.MaxValue, char.MaxValue, char.MaxValue, Uri.UnescapeMode.CopyOnly, this.m_Syntax, false, false);
				}
				if (parts == UriComponents.UserInfo)
				{
					num--;
				}
			}
			if ((parts & UriComponents.Host) != (UriComponents)0 && text.Length != 0)
			{
				Uri.UnescapeMode unescapeMode;
				if (formatAs != UriFormat.UriEscaped && this.HostType == Uri.Flags.BasicHostType && (nonCanonical & 4) != 0)
				{
					unescapeMode = ((formatAs == UriFormat.Unescaped) ? (Uri.UnescapeMode.Unescape | Uri.UnescapeMode.UnescapeAll) : (this.InFact(Uri.Flags.UserEscaped) ? Uri.UnescapeMode.Unescape : Uri.UnescapeMode.EscapeUnescape));
				}
				else
				{
					unescapeMode = Uri.UnescapeMode.CopyOnly;
				}
				array = Uri.UnescapeString(text, 0, text.Length, array, ref num, '/', '?', '#', unescapeMode, this.m_Syntax, false, false);
				if ((parts & UriComponents.SerializationInfoString) != (UriComponents)0 && this.HostType == Uri.Flags.IPv6HostType && this.m_Info.ScopeId != null)
				{
					this.m_Info.ScopeId.CopyTo(0, array, num - 1, this.m_Info.ScopeId.Length);
					num += this.m_Info.ScopeId.Length;
					array[num - 1] = ']';
				}
			}
			if ((parts & UriComponents.Port) != (UriComponents)0)
			{
				if ((nonCanonical & 8) == 0)
				{
					if (this.InFact(Uri.Flags.NotDefaultPort))
					{
						ushort num2 = this.m_Info.Offset.Path;
						while (this.m_String[(int)(num2 -= 1)] != ':')
						{
						}
						this.m_String.CopyTo((int)num2, array, num, (int)(this.m_Info.Offset.Path - num2));
						num += (int)(this.m_Info.Offset.Path - num2);
					}
					else if ((parts & UriComponents.StrongPort) != (UriComponents)0 && this.m_Syntax.DefaultPort != -1)
					{
						array[num++] = ':';
						text = this.m_Info.Offset.PortValue.ToString(CultureInfo.InvariantCulture);
						text.CopyTo(0, array, num, text.Length);
						num += text.Length;
					}
				}
				else if (this.InFact(Uri.Flags.NotDefaultPort) || ((parts & UriComponents.StrongPort) != (UriComponents)0 && this.m_Syntax.DefaultPort != -1))
				{
					array[num++] = ':';
					text = this.m_Info.Offset.PortValue.ToString(CultureInfo.InvariantCulture);
					text.CopyTo(0, array, num, text.Length);
					num += text.Length;
				}
			}
			if ((parts & UriComponents.Path) != (UriComponents)0)
			{
				array = this.GetCanonicalPath(array, ref num, formatAs);
				if (parts == UriComponents.Path)
				{
					ushort num3;
					if (this.InFact(Uri.Flags.AuthorityFound) && num != 0 && array[0] == '/')
					{
						num3 = 1;
						num--;
					}
					else
					{
						num3 = 0;
					}
					if (num != 0)
					{
						return new string(array, (int)num3, num);
					}
					return string.Empty;
				}
			}
			if ((parts & UriComponents.Query) != (UriComponents)0 && this.m_Info.Offset.Query < this.m_Info.Offset.Fragment)
			{
				ushort num3 = this.m_Info.Offset.Query + 1;
				if (parts != UriComponents.Query)
				{
					array[num++] = '?';
				}
				if ((nonCanonical & 32) != 0)
				{
					switch (formatAs)
					{
					case UriFormat.UriEscaped:
						if (this.NotAny(Uri.Flags.UserEscaped))
						{
							array = Uri.EscapeString(this.m_String, (int)num3, (int)this.m_Info.Offset.Fragment, array, ref num, true, '#', char.MaxValue, '%');
						}
						else
						{
							Uri.UnescapeString(this.m_String, (int)num3, (int)this.m_Info.Offset.Fragment, array, ref num, char.MaxValue, char.MaxValue, char.MaxValue, Uri.UnescapeMode.CopyOnly, this.m_Syntax, true, false);
						}
						break;
					case UriFormat.Unescaped:
						array = Uri.UnescapeString(this.m_String, (int)num3, (int)this.m_Info.Offset.Fragment, array, ref num, '#', char.MaxValue, char.MaxValue, Uri.UnescapeMode.Unescape | Uri.UnescapeMode.UnescapeAll, this.m_Syntax, true, false);
						break;
					default:
						if (formatAs != (UriFormat)32767)
						{
							array = Uri.UnescapeString(this.m_String, (int)num3, (int)this.m_Info.Offset.Fragment, array, ref num, '#', char.MaxValue, char.MaxValue, this.InFact(Uri.Flags.UserEscaped) ? Uri.UnescapeMode.Unescape : Uri.UnescapeMode.EscapeUnescape, this.m_Syntax, true, false);
						}
						else
						{
							array = Uri.UnescapeString(this.m_String, (int)num3, (int)this.m_Info.Offset.Fragment, array, ref num, '#', char.MaxValue, char.MaxValue, (this.InFact(Uri.Flags.UserEscaped) ? Uri.UnescapeMode.Unescape : Uri.UnescapeMode.EscapeUnescape) | Uri.UnescapeMode.V1ToStringFlag, this.m_Syntax, true, false);
						}
						break;
					}
				}
				else
				{
					Uri.UnescapeString(this.m_String, (int)num3, (int)this.m_Info.Offset.Fragment, array, ref num, char.MaxValue, char.MaxValue, char.MaxValue, Uri.UnescapeMode.CopyOnly, this.m_Syntax, true, false);
				}
			}
			if ((parts & UriComponents.Fragment) != (UriComponents)0 && this.m_Info.Offset.Fragment < this.m_Info.Offset.End)
			{
				ushort num3 = this.m_Info.Offset.Fragment + 1;
				if (parts != UriComponents.Fragment)
				{
					array[num++] = '#';
				}
				if ((nonCanonical & 64) != 0)
				{
					switch (formatAs)
					{
					case UriFormat.UriEscaped:
						if (this.NotAny(Uri.Flags.UserEscaped))
						{
							array = Uri.EscapeString(this.m_String, (int)num3, (int)this.m_Info.Offset.End, array, ref num, true, '#', char.MaxValue, '%');
						}
						else
						{
							Uri.UnescapeString(this.m_String, (int)num3, (int)this.m_Info.Offset.End, array, ref num, char.MaxValue, char.MaxValue, char.MaxValue, Uri.UnescapeMode.CopyOnly, this.m_Syntax, false, false);
						}
						break;
					case UriFormat.Unescaped:
						array = Uri.UnescapeString(this.m_String, (int)num3, (int)this.m_Info.Offset.End, array, ref num, '#', char.MaxValue, char.MaxValue, Uri.UnescapeMode.Unescape | Uri.UnescapeMode.UnescapeAll, this.m_Syntax, false, false);
						break;
					default:
						if (formatAs != (UriFormat)32767)
						{
							array = Uri.UnescapeString(this.m_String, (int)num3, (int)this.m_Info.Offset.End, array, ref num, '#', char.MaxValue, char.MaxValue, this.InFact(Uri.Flags.UserEscaped) ? Uri.UnescapeMode.Unescape : Uri.UnescapeMode.EscapeUnescape, this.m_Syntax, false, false);
						}
						else
						{
							array = Uri.UnescapeString(this.m_String, (int)num3, (int)this.m_Info.Offset.End, array, ref num, '#', char.MaxValue, char.MaxValue, (this.InFact(Uri.Flags.UserEscaped) ? Uri.UnescapeMode.Unescape : Uri.UnescapeMode.EscapeUnescape) | Uri.UnescapeMode.V1ToStringFlag, this.m_Syntax, false, false);
						}
						break;
					}
				}
				else
				{
					Uri.UnescapeString(this.m_String, (int)num3, (int)this.m_Info.Offset.End, array, ref num, char.MaxValue, char.MaxValue, char.MaxValue, Uri.UnescapeMode.CopyOnly, this.m_Syntax, false, false);
				}
			}
			return new string(array, 0, num);
		}

		// Token: 0x06001B1F RID: 6943 RVA: 0x000611F4 File Offset: 0x000601F4
		private string GetUriPartsFromUserString(UriComponents uriParts)
		{
			UriComponents uriComponents = uriParts & ~UriComponents.KeepDelimiter;
			if (uriComponents <= UriComponents.PathAndQuery)
			{
				if (uriComponents <= UriComponents.Path)
				{
					switch (uriComponents)
					{
					case UriComponents.Scheme:
						if (uriParts != UriComponents.Scheme)
						{
							return this.m_String.Substring((int)this.m_Info.Offset.Scheme, (int)(this.m_Info.Offset.User - this.m_Info.Offset.Scheme));
						}
						return this.m_Syntax.SchemeName;
					case UriComponents.UserInfo:
					{
						if (this.NotAny(Uri.Flags.HasUserInfo))
						{
							return string.Empty;
						}
						ushort num;
						if (uriParts == UriComponents.UserInfo)
						{
							num = this.m_Info.Offset.Host - 1;
						}
						else
						{
							num = this.m_Info.Offset.Host;
						}
						if (this.m_Info.Offset.User >= num)
						{
							return string.Empty;
						}
						return this.m_String.Substring((int)this.m_Info.Offset.User, (int)(num - this.m_Info.Offset.User));
					}
					case UriComponents.Scheme | UriComponents.UserInfo:
						goto IL_099D;
					case UriComponents.Host:
					{
						ushort num2 = this.m_Info.Offset.Path;
						if (this.InFact(Uri.Flags.PortNotCanonical | Uri.Flags.NotDefaultPort))
						{
							while (this.m_String[(int)(num2 -= 1)] != ':')
							{
							}
						}
						if (num2 - this.m_Info.Offset.Host != 0)
						{
							return this.m_String.Substring((int)this.m_Info.Offset.Host, (int)(num2 - this.m_Info.Offset.Host));
						}
						return string.Empty;
					}
					default:
						switch (uriComponents)
						{
						case UriComponents.SchemeAndServer:
							if (!this.InFact(Uri.Flags.HasUserInfo))
							{
								return this.m_String.Substring((int)this.m_Info.Offset.Scheme, (int)(this.m_Info.Offset.Path - this.m_Info.Offset.Scheme));
							}
							return this.m_String.Substring((int)this.m_Info.Offset.Scheme, (int)(this.m_Info.Offset.User - this.m_Info.Offset.Scheme)) + this.m_String.Substring((int)this.m_Info.Offset.Host, (int)(this.m_Info.Offset.Path - this.m_Info.Offset.Host));
						case UriComponents.UserInfo | UriComponents.Host | UriComponents.Port:
							break;
						case UriComponents.Scheme | UriComponents.UserInfo | UriComponents.Host | UriComponents.Port:
							return this.m_String.Substring((int)this.m_Info.Offset.Scheme, (int)(this.m_Info.Offset.Path - this.m_Info.Offset.Scheme));
						case UriComponents.Path:
						{
							ushort num;
							if (uriParts == UriComponents.Path && this.InFact(Uri.Flags.AuthorityFound) && this.m_Info.Offset.End > this.m_Info.Offset.Path && this.m_String[(int)this.m_Info.Offset.Path] == '/')
							{
								num = this.m_Info.Offset.Path + 1;
							}
							else
							{
								num = this.m_Info.Offset.Path;
							}
							if (num >= this.m_Info.Offset.Query)
							{
								return string.Empty;
							}
							return this.m_String.Substring((int)num, (int)(this.m_Info.Offset.Query - num));
						}
						default:
							goto IL_099D;
						}
						break;
					}
				}
				else if (uriComponents != UriComponents.Query)
				{
					if (uriComponents != UriComponents.PathAndQuery)
					{
						goto IL_099D;
					}
					return this.m_String.Substring((int)this.m_Info.Offset.Path, (int)(this.m_Info.Offset.Fragment - this.m_Info.Offset.Path));
				}
				else
				{
					ushort num;
					if (uriParts == UriComponents.Query)
					{
						num = this.m_Info.Offset.Query + 1;
					}
					else
					{
						num = this.m_Info.Offset.Query;
					}
					if (num >= this.m_Info.Offset.Fragment)
					{
						return string.Empty;
					}
					return this.m_String.Substring((int)num, (int)(this.m_Info.Offset.Fragment - num));
				}
			}
			else if (uriComponents <= (UriComponents.Path | UriComponents.Query | UriComponents.Fragment))
			{
				switch (uriComponents)
				{
				case UriComponents.HttpRequestUrl:
					if (this.InFact(Uri.Flags.HasUserInfo))
					{
						return this.m_String.Substring((int)this.m_Info.Offset.Scheme, (int)(this.m_Info.Offset.User - this.m_Info.Offset.Scheme)) + this.m_String.Substring((int)this.m_Info.Offset.Host, (int)(this.m_Info.Offset.Fragment - this.m_Info.Offset.Host));
					}
					if (this.m_Info.Offset.Scheme == 0 && (int)this.m_Info.Offset.Fragment == this.m_String.Length)
					{
						return this.m_String;
					}
					return this.m_String.Substring((int)this.m_Info.Offset.Scheme, (int)(this.m_Info.Offset.Fragment - this.m_Info.Offset.Scheme));
				case UriComponents.UserInfo | UriComponents.Host | UriComponents.Port | UriComponents.Path | UriComponents.Query:
					goto IL_099D;
				case UriComponents.Scheme | UriComponents.UserInfo | UriComponents.Host | UriComponents.Port | UriComponents.Path | UriComponents.Query:
					if (this.m_Info.Offset.Scheme == 0 && (int)this.m_Info.Offset.Fragment == this.m_String.Length)
					{
						return this.m_String;
					}
					return this.m_String.Substring((int)this.m_Info.Offset.Scheme, (int)(this.m_Info.Offset.Fragment - this.m_Info.Offset.Scheme));
				case UriComponents.Fragment:
				{
					ushort num;
					if (uriParts == UriComponents.Fragment)
					{
						num = this.m_Info.Offset.Fragment + 1;
					}
					else
					{
						num = this.m_Info.Offset.Fragment;
					}
					if (num >= this.m_Info.Offset.End)
					{
						return string.Empty;
					}
					return this.m_String.Substring((int)num, (int)(this.m_Info.Offset.End - num));
				}
				default:
					if (uriComponents != (UriComponents.Path | UriComponents.Query | UriComponents.Fragment))
					{
						goto IL_099D;
					}
					return this.m_String.Substring((int)this.m_Info.Offset.Path, (int)(this.m_Info.Offset.End - this.m_Info.Offset.Path));
				}
			}
			else
			{
				switch (uriComponents)
				{
				case UriComponents.Scheme | UriComponents.Host | UriComponents.Port | UriComponents.Path | UriComponents.Query | UriComponents.Fragment:
					if (this.InFact(Uri.Flags.HasUserInfo))
					{
						return this.m_String.Substring((int)this.m_Info.Offset.Scheme, (int)(this.m_Info.Offset.User - this.m_Info.Offset.Scheme)) + this.m_String.Substring((int)this.m_Info.Offset.Host, (int)(this.m_Info.Offset.End - this.m_Info.Offset.Host));
					}
					if (this.m_Info.Offset.Scheme == 0 && (int)this.m_Info.Offset.End == this.m_String.Length)
					{
						return this.m_String;
					}
					return this.m_String.Substring((int)this.m_Info.Offset.Scheme, (int)(this.m_Info.Offset.End - this.m_Info.Offset.Scheme));
				case UriComponents.UserInfo | UriComponents.Host | UriComponents.Port | UriComponents.Path | UriComponents.Query | UriComponents.Fragment:
					goto IL_099D;
				case UriComponents.AbsoluteUri:
					if (this.m_Info.Offset.Scheme == 0 && (int)this.m_Info.Offset.End == this.m_String.Length)
					{
						return this.m_String;
					}
					return this.m_String.Substring((int)this.m_Info.Offset.Scheme, (int)(this.m_Info.Offset.End - this.m_Info.Offset.Scheme));
				default:
					switch (uriComponents)
					{
					case UriComponents.HostAndPort:
						if (this.InFact(Uri.Flags.HasUserInfo))
						{
							if (this.InFact(Uri.Flags.NotDefaultPort) || this.m_Syntax.DefaultPort == -1)
							{
								return this.m_String.Substring((int)this.m_Info.Offset.Host, (int)(this.m_Info.Offset.Path - this.m_Info.Offset.Host));
							}
							return this.m_String.Substring((int)this.m_Info.Offset.Host, (int)(this.m_Info.Offset.Path - this.m_Info.Offset.Host)) + ':' + this.m_Info.Offset.PortValue.ToString(CultureInfo.InvariantCulture);
						}
						break;
					case UriComponents.Scheme | UriComponents.Host | UriComponents.StrongPort:
						goto IL_099D;
					case UriComponents.StrongAuthority:
						break;
					default:
						goto IL_099D;
					}
					if (!this.InFact(Uri.Flags.NotDefaultPort) && this.m_Syntax.DefaultPort != -1)
					{
						return this.m_String.Substring((int)this.m_Info.Offset.User, (int)(this.m_Info.Offset.Path - this.m_Info.Offset.User)) + ':' + this.m_Info.Offset.PortValue.ToString(CultureInfo.InvariantCulture);
					}
					break;
				}
			}
			if (this.m_Info.Offset.Path - this.m_Info.Offset.User != 0)
			{
				return this.m_String.Substring((int)this.m_Info.Offset.User, (int)(this.m_Info.Offset.Path - this.m_Info.Offset.User));
			}
			return string.Empty;
			IL_099D:
			return null;
		}

		// Token: 0x06001B20 RID: 6944 RVA: 0x00061BA0 File Offset: 0x00060BA0
		private unsafe void ParseRemaining()
		{
			this.EnsureUriInfo();
			Uri.Flags flags = Uri.Flags.Zero;
			if (!this.UserDrivenParsing)
			{
				bool flag = this.m_iriParsing && (this.m_Flags & Uri.Flags.HasUnicode) != Uri.Flags.Zero && (this.m_Flags & Uri.Flags.RestUnicodeNormalized) == Uri.Flags.Zero;
				ushort num = this.m_Info.Offset.Scheme;
				ushort num2 = (ushort)this.m_String.Length;
				UriSyntaxFlags flags2 = this.m_Syntax.Flags;
				Uri.Check check;
				fixed (char* @string = this.m_String)
				{
					if (num2 > num && Uri.IsLWS(@string[num2 - 1]))
					{
						num2 -= 1;
						while (num2 != num && Uri.IsLWS(@string[(IntPtr)(num2 -= 1) * 2]))
						{
						}
						num2 += 1;
					}
					if (this.IsImplicitFile)
					{
						flags |= Uri.Flags.SchemeNotCanonical;
					}
					else
					{
						ushort num3 = 0;
						ushort num4 = (ushort)this.m_Syntax.SchemeName.Length;
						while (num3 < num4)
						{
							if (this.m_Syntax.SchemeName[(int)num3] != @string[num + num3])
							{
								flags |= Uri.Flags.SchemeNotCanonical;
							}
							num3 += 1;
						}
						if ((this.m_Flags & Uri.Flags.AuthorityFound) != Uri.Flags.Zero && (num + num3 + 3 >= num2 || @string[num + num3 + 1] != '/' || @string[num + num3 + 2] != '/'))
						{
							flags |= Uri.Flags.SchemeNotCanonical;
						}
					}
					if ((this.m_Flags & Uri.Flags.HasUserInfo) != Uri.Flags.Zero)
					{
						num = this.m_Info.Offset.User;
						check = this.CheckCanonical(@string, ref num, this.m_Info.Offset.Host, '@');
						if ((check & Uri.Check.DisplayCanonical) == Uri.Check.None)
						{
							flags |= Uri.Flags.UserNotCanonical;
						}
						if ((check & (Uri.Check.EscapedCanonical | Uri.Check.BackslashInPath)) != Uri.Check.EscapedCanonical)
						{
							flags |= Uri.Flags.E_UserNotCanonical;
						}
						if (this.m_iriParsing && (check & (Uri.Check.EscapedCanonical | Uri.Check.DisplayCanonical | Uri.Check.BackslashInPath | Uri.Check.NotIriCanonical | Uri.Check.FoundNonAscii)) == (Uri.Check.DisplayCanonical | Uri.Check.FoundNonAscii))
						{
							flags |= Uri.Flags.UserIriCanonical;
						}
					}
				}
				num = this.m_Info.Offset.Path;
				ushort num5 = this.m_Info.Offset.Path;
				if (flag)
				{
					this.m_Info.Offset.Path = (ushort)this.m_String.Length;
					num = this.m_Info.Offset.Path;
					ushort num6 = num5;
					if (this.IsImplicitFile || (flags2 & (UriSyntaxFlags.MayHaveQuery | UriSyntaxFlags.MayHaveFragment)) == (UriSyntaxFlags)0)
					{
						this.FindEndOfComponent(this.m_originalUnicodeString, ref num5, (ushort)this.m_originalUnicodeString.Length, char.MaxValue);
					}
					else
					{
						this.FindEndOfComponent(this.m_originalUnicodeString, ref num5, (ushort)this.m_originalUnicodeString.Length, this.m_Syntax.InFact(UriSyntaxFlags.MayHaveQuery) ? '?' : (this.m_Syntax.InFact(UriSyntaxFlags.MayHaveFragment) ? '#' : '\ufffe'));
					}
					string text = this.EscapeUnescapeIri(this.m_originalUnicodeString, (int)num6, (int)num5, UriComponents.Path);
					try
					{
						this.m_String += text.Normalize(NormalizationForm.FormC);
					}
					catch (ArgumentException)
					{
						UriFormatException exception = Uri.GetException(Uri.ParsingError.BadFormat);
						throw exception;
					}
					if (!ServicePointManager.AllowAllUriEncodingExpansion && this.m_String.Length > 65535)
					{
						UriFormatException exception2 = Uri.GetException(Uri.ParsingError.SizeLimit);
						throw exception2;
					}
					num2 = (ushort)this.m_String.Length;
				}
				fixed (char* string2 = this.m_String)
				{
					if (this.IsImplicitFile || (flags2 & (UriSyntaxFlags.MayHaveQuery | UriSyntaxFlags.MayHaveFragment)) == (UriSyntaxFlags)0)
					{
						check = this.CheckCanonical(string2, ref num, num2, char.MaxValue);
					}
					else
					{
						check = this.CheckCanonical(string2, ref num, num2, ((flags2 & UriSyntaxFlags.MayHaveQuery) != (UriSyntaxFlags)0) ? '?' : (this.m_Syntax.InFact(UriSyntaxFlags.MayHaveFragment) ? '#' : '\ufffe'));
					}
					if ((this.m_Flags & Uri.Flags.AuthorityFound) != Uri.Flags.Zero && (flags2 & UriSyntaxFlags.PathIsRooted) != (UriSyntaxFlags)0 && (this.m_Info.Offset.Path == num2 || (string2[this.m_Info.Offset.Path] != '/' && string2[this.m_Info.Offset.Path] != '\\')))
					{
						flags |= Uri.Flags.FirstSlashAbsent;
					}
				}
				bool flag2 = false;
				if (this.IsDosPath || ((this.m_Flags & Uri.Flags.AuthorityFound) != Uri.Flags.Zero && (flags2 & (UriSyntaxFlags.ConvertPathSlashes | UriSyntaxFlags.CompressPath | UriSyntaxFlags.UnEscapeDotsAndSlashes)) != (UriSyntaxFlags)0))
				{
					if ((flags2 & UriSyntaxFlags.UnEscapeDotsAndSlashes) != (UriSyntaxFlags)0 && (check & Uri.Check.DotSlashEscaped) != Uri.Check.None)
					{
						flags |= Uri.Flags.PathNotCanonical | Uri.Flags.E_PathNotCanonical;
						flag2 = true;
					}
					if ((flags2 & UriSyntaxFlags.ConvertPathSlashes) != (UriSyntaxFlags)0 && (check & Uri.Check.BackslashInPath) != Uri.Check.None)
					{
						flags |= Uri.Flags.PathNotCanonical | Uri.Flags.E_PathNotCanonical;
						flag2 = true;
					}
					if ((flags2 & UriSyntaxFlags.CompressPath) != (UriSyntaxFlags)0 && ((flags & Uri.Flags.E_PathNotCanonical) != Uri.Flags.Zero || (check & Uri.Check.DotSlashAttn) != Uri.Check.None))
					{
						flags |= Uri.Flags.ShouldBeCompressed;
					}
					if ((check & Uri.Check.BackslashInPath) != Uri.Check.None)
					{
						flags |= Uri.Flags.BackslashInPath;
					}
				}
				else if ((check & Uri.Check.BackslashInPath) != Uri.Check.None)
				{
					flags |= Uri.Flags.E_PathNotCanonical;
					flag2 = true;
				}
				if ((check & Uri.Check.DisplayCanonical) == Uri.Check.None && ((this.m_Flags & Uri.Flags.ImplicitFile) == Uri.Flags.Zero || (this.m_Flags & Uri.Flags.UserEscaped) != Uri.Flags.Zero || (check & Uri.Check.ReservedFound) != Uri.Check.None))
				{
					flags |= Uri.Flags.PathNotCanonical;
					flag2 = true;
				}
				if ((this.m_Flags & Uri.Flags.ImplicitFile) != Uri.Flags.Zero && (check & (Uri.Check.EscapedCanonical | Uri.Check.ReservedFound)) != Uri.Check.None)
				{
					check &= ~Uri.Check.EscapedCanonical;
				}
				if ((check & Uri.Check.EscapedCanonical) == Uri.Check.None)
				{
					flags |= Uri.Flags.E_PathNotCanonical;
				}
				if (this.m_iriParsing && (!flag2 & ((check & (Uri.Check.EscapedCanonical | Uri.Check.DisplayCanonical | Uri.Check.NotIriCanonical | Uri.Check.FoundNonAscii)) == (Uri.Check.DisplayCanonical | Uri.Check.FoundNonAscii))))
				{
					flags |= Uri.Flags.PathIriCanonical;
				}
				if (flag)
				{
					ushort num7 = num5;
					if ((int)num5 < this.m_originalUnicodeString.Length && this.m_originalUnicodeString[(int)num5] == '?')
					{
						num5 += 1;
						this.FindEndOfComponent(this.m_originalUnicodeString, ref num5, (ushort)this.m_originalUnicodeString.Length, ((flags2 & UriSyntaxFlags.MayHaveFragment) != (UriSyntaxFlags)0) ? '#' : '\ufffe');
						string text2 = this.EscapeUnescapeIri(this.m_originalUnicodeString, (int)num7, (int)num5, UriComponents.Query);
						try
						{
							this.m_String += text2.Normalize(NormalizationForm.FormC);
						}
						catch (ArgumentException)
						{
							UriFormatException exception3 = Uri.GetException(Uri.ParsingError.BadFormat);
							throw exception3;
						}
						if (!ServicePointManager.AllowAllUriEncodingExpansion && this.m_String.Length > 65535)
						{
							UriFormatException exception4 = Uri.GetException(Uri.ParsingError.SizeLimit);
							throw exception4;
						}
						num2 = (ushort)this.m_String.Length;
					}
				}
				this.m_Info.Offset.Query = num;
				fixed (char* string3 = this.m_String)
				{
					if (num < num2 && string3[num] == '?')
					{
						num += 1;
						check = this.CheckCanonical(string3, ref num, num2, ((flags2 & UriSyntaxFlags.MayHaveFragment) != (UriSyntaxFlags)0) ? '#' : '\ufffe');
						if ((check & Uri.Check.DisplayCanonical) == Uri.Check.None)
						{
							flags |= Uri.Flags.QueryNotCanonical;
						}
						if ((check & (Uri.Check.EscapedCanonical | Uri.Check.BackslashInPath)) != Uri.Check.EscapedCanonical)
						{
							flags |= Uri.Flags.E_QueryNotCanonical;
						}
						if (this.m_iriParsing && (check & (Uri.Check.EscapedCanonical | Uri.Check.DisplayCanonical | Uri.Check.BackslashInPath | Uri.Check.NotIriCanonical | Uri.Check.FoundNonAscii)) == (Uri.Check.DisplayCanonical | Uri.Check.FoundNonAscii))
						{
							flags |= Uri.Flags.QueryIriCanonical;
						}
					}
				}
				if (flag)
				{
					ushort num8 = num5;
					if ((int)num5 < this.m_originalUnicodeString.Length && this.m_originalUnicodeString[(int)num5] == '#')
					{
						num5 += 1;
						this.FindEndOfComponent(this.m_originalUnicodeString, ref num5, (ushort)this.m_originalUnicodeString.Length, '\ufffe');
						string text3 = this.EscapeUnescapeIri(this.m_originalUnicodeString, (int)num8, (int)num5, UriComponents.Fragment);
						try
						{
							this.m_String += text3.Normalize(NormalizationForm.FormC);
						}
						catch (ArgumentException)
						{
							UriFormatException exception5 = Uri.GetException(Uri.ParsingError.BadFormat);
							throw exception5;
						}
						if (!ServicePointManager.AllowAllUriEncodingExpansion && this.m_String.Length > 65535)
						{
							UriFormatException exception6 = Uri.GetException(Uri.ParsingError.SizeLimit);
							throw exception6;
						}
						num2 = (ushort)this.m_String.Length;
					}
				}
				this.m_Info.Offset.Fragment = num;
				fixed (char* string4 = this.m_String)
				{
					if (num < num2 && string4[num] == '#')
					{
						num += 1;
						check = this.CheckCanonical(string4, ref num, num2, '\ufffe');
						if ((check & Uri.Check.DisplayCanonical) == Uri.Check.None)
						{
							flags |= Uri.Flags.FragmentNotCanonical;
						}
						if ((check & (Uri.Check.EscapedCanonical | Uri.Check.BackslashInPath)) != Uri.Check.EscapedCanonical)
						{
							flags |= Uri.Flags.E_FragmentNotCanonical;
						}
						if (this.m_iriParsing && (check & (Uri.Check.EscapedCanonical | Uri.Check.DisplayCanonical | Uri.Check.BackslashInPath | Uri.Check.NotIriCanonical | Uri.Check.FoundNonAscii)) == (Uri.Check.DisplayCanonical | Uri.Check.FoundNonAscii))
						{
							flags |= Uri.Flags.FragmentIriCanonical;
						}
					}
				}
				this.m_Info.Offset.End = num;
			}
			flags |= (Uri.Flags)int.MinValue;
			lock (this.m_Info)
			{
				this.m_Flags |= flags;
			}
			this.m_Flags |= Uri.Flags.RestUnicodeNormalized;
		}

		// Token: 0x06001B21 RID: 6945 RVA: 0x00062418 File Offset: 0x00061418
		private unsafe static ushort ParseSchemeCheckImplicitFile(char* uriString, ushort length, ref Uri.ParsingError err, ref Uri.Flags flags, ref UriParser syntax)
		{
			ushort num = 0;
			while (num < length && Uri.IsLWS(uriString[num]))
			{
				num += 1;
			}
			ushort num2 = num;
			while (num2 < length && uriString[num2] != ':')
			{
				num2 += 1;
			}
			if (IntPtr.Size == 4 && num2 != length && num2 >= num + 3 && Uri.CheckKnownSchemes((long*)(uriString + num), num2 - num, ref syntax))
			{
				return num2 + 1;
			}
			if (num + 2 >= length || num2 == num)
			{
				err = Uri.ParsingError.BadFormat;
				return 0;
			}
			char c;
			if ((c = uriString[num + 1]) == ':' || c == '|')
			{
				if (!Uri.IsAsciiLetter(uriString[num]))
				{
					if (c == ':')
					{
						err = Uri.ParsingError.BadScheme;
					}
					else
					{
						err = Uri.ParsingError.BadFormat;
					}
					return 0;
				}
				if ((c = uriString[num + 2]) == '\\' || c == '/')
				{
					flags |= Uri.Flags.AuthorityFound | Uri.Flags.DosPath | Uri.Flags.ImplicitFile;
					syntax = UriParser.FileUri;
					return num;
				}
				err = Uri.ParsingError.MustRootedPath;
				return 0;
			}
			else if ((c = uriString[num]) == '/' || c == '\\')
			{
				if ((c = uriString[num + 1]) == '\\' || c == '/')
				{
					flags |= Uri.Flags.AuthorityFound | Uri.Flags.UncPath | Uri.Flags.ImplicitFile;
					syntax = UriParser.FileUri;
					num += 2;
					while (num < length && ((c = uriString[num]) == '/' || c == '\\'))
					{
						num += 1;
					}
					return num;
				}
				err = Uri.ParsingError.BadFormat;
				return 0;
			}
			else
			{
				if (num2 == length)
				{
					err = Uri.ParsingError.BadFormat;
					return 0;
				}
				if (num2 - num > 1024)
				{
					err = Uri.ParsingError.SchemeLimit;
					return 0;
				}
				char* ptr = stackalloc char[2 * (num2 - num)];
				length = 0;
				while (num < num2)
				{
					ref short ptr2 = ref *(short*)ptr;
					ushort num3 = length;
					length = num3 + 1;
					*((ref ptr2) + (IntPtr)num3 * 2) = (short)uriString[num];
					num += 1;
				}
				err = Uri.CheckSchemeSyntax(ptr, length, ref syntax);
				if (err != Uri.ParsingError.None)
				{
					return 0;
				}
				return num2 + 1;
			}
		}

		// Token: 0x06001B22 RID: 6946 RVA: 0x000625AC File Offset: 0x000615AC
		private unsafe static bool CheckKnownSchemes(long* lptr, ushort nChars, ref UriParser syntax)
		{
			long num = *lptr | 9007336695791648L;
			if (num <= 29273878621519975L)
			{
				if (num <= 16326029693157478L)
				{
					if (num != 12948347151515758L)
					{
						if (num == 16326029693157478L)
						{
							if (nChars == 3)
							{
								syntax = UriParser.FtpUri;
								return true;
							}
						}
					}
					else
					{
						if (nChars == 8 && (lptr[1] | 9007336695791648L) == 28429453690994800L)
						{
							syntax = UriParser.NetPipeUri;
							return true;
						}
						if (nChars == 7 && (lptr[1] | 9007336695791648L) == 16326029692043380L)
						{
							syntax = UriParser.NetTcpUri;
							return true;
						}
					}
				}
				else if (num != 28147948650299509L)
				{
					if (num != 28429436511125606L)
					{
						if (num == 29273878621519975L)
						{
							if (nChars == 6 && (*(int*)(lptr + 1) | 2097184) == 7471205)
							{
								syntax = UriParser.GopherUri;
								return true;
							}
						}
					}
					else if (nChars == 4)
					{
						syntax = UriParser.FileUri;
						return true;
					}
				}
				else if (nChars == 4)
				{
					syntax = UriParser.UuidUri;
					return true;
				}
			}
			else if (num <= 31525614009974892L)
			{
				if (num != 30399748462674029L)
				{
					if (num != 30962711301259380L)
					{
						if (num == 31525614009974892L)
						{
							if (nChars == 4)
							{
								syntax = UriParser.LdapUri;
								return true;
							}
						}
					}
					else if (nChars == 6 && (*(int*)(lptr + 1) | 2097184) == 7602277)
					{
						syntax = UriParser.TelnetUri;
						return true;
					}
				}
				else if (nChars == 6 && (*(int*)(lptr + 1) | 2097184) == 7274612)
				{
					syntax = UriParser.MailToUri;
					return true;
				}
			}
			else if (num != 31525695615008878L)
			{
				if (num != 31525695615402088L)
				{
					if (num == 32370133429452910L)
					{
						if (nChars == 4)
						{
							syntax = UriParser.NewsUri;
							return true;
						}
					}
				}
				else
				{
					if (nChars == 4)
					{
						syntax = UriParser.HttpUri;
						return true;
					}
					if (nChars == 5 && (*(ushort*)(lptr + 1) | 32) == 115)
					{
						syntax = UriParser.HttpsUri;
						return true;
					}
				}
			}
			else if (nChars == 4)
			{
				syntax = UriParser.NntpUri;
				return true;
			}
			return false;
		}

		// Token: 0x06001B23 RID: 6947 RVA: 0x000627DC File Offset: 0x000617DC
		private unsafe static Uri.ParsingError CheckSchemeSyntax(char* ptr, ushort length, ref UriParser syntax)
		{
			char c = *ptr;
			if (c < 'a' || c > 'z')
			{
				if (c < 'A' || c > 'Z')
				{
					return Uri.ParsingError.BadScheme;
				}
				*ptr = c | ' ';
			}
			for (ushort num = 1; num < length; num += 1)
			{
				char c2 = ptr[num];
				if (c2 < 'a' || c2 > 'z')
				{
					if (c2 >= 'A' && c2 <= 'Z')
					{
						ptr[num] = c2 | ' ';
					}
					else if ((c2 < '0' || c2 > '9') && c2 != '+' && c2 != '-' && c2 != '.')
					{
						return Uri.ParsingError.BadScheme;
					}
				}
			}
			string text = new string(ptr, 0, (int)length);
			syntax = UriParser.FindOrFetchAsUnknownV1Syntax(text);
			return Uri.ParsingError.None;
		}

		// Token: 0x06001B24 RID: 6948 RVA: 0x00062870 File Offset: 0x00061870
		private unsafe ushort CheckAuthorityHelper(char* pString, ushort idx, ushort length, ref Uri.ParsingError err, ref Uri.Flags flags, UriParser syntax, ref string newHost)
		{
			int num = (int)length;
			int num2 = (int)idx;
			ushort num3 = idx;
			newHost = null;
			bool flag = false;
			bool flag2 = Uri.s_IriParsing && Uri.IriParsingStatic(syntax);
			bool flag3 = (flags & Uri.Flags.HasUnicode) != Uri.Flags.Zero;
			bool flag4 = (flags & Uri.Flags.HostUnicodeNormalized) == Uri.Flags.Zero;
			UriSyntaxFlags flags2 = syntax.Flags;
			if (flag3 && flag2 && flag4)
			{
				newHost = this.m_originalUnicodeString.Substring(0, num2);
			}
			char c;
			if (idx == length || ((c = pString[idx]) == '/' || (c == '\\' && Uri.StaticIsFile(syntax))) || c == '#' || c == '?')
			{
				if (syntax.InFact(UriSyntaxFlags.AllowEmptyHost))
				{
					flags &= ~Uri.Flags.UncPath;
					if (Uri.StaticInFact(flags, Uri.Flags.ImplicitFile))
					{
						err = Uri.ParsingError.BadHostName;
					}
					else
					{
						flags |= Uri.Flags.BasicHostType;
					}
				}
				else
				{
					err = Uri.ParsingError.BadHostName;
				}
				if (flag3 && flag2 && flag4)
				{
					flags |= Uri.Flags.HostUnicodeNormalized;
				}
				return idx;
			}
			string text = null;
			if ((flags2 & UriSyntaxFlags.MayHaveUserInfo) != (UriSyntaxFlags)0)
			{
				while ((int)num3 < num)
				{
					if ((int)num3 == num - 1 || pString[num3] == '?' || pString[num3] == '#' || pString[num3] == '\\' || pString[num3] == '/')
					{
						num3 = idx;
						break;
					}
					if (pString[num3] == '@')
					{
						flags |= Uri.Flags.HasUserInfo;
						if (flag2 || Uri.s_IdnScope != UriIdnScope.None)
						{
							if (flag2 && flag3 && flag4)
							{
								text = this.EscapeUnescapeIri(pString, num2, (int)(num3 + 1), UriComponents.UserInfo);
								try
								{
									text = text.Normalize(NormalizationForm.FormC);
								}
								catch (ArgumentException)
								{
									err = Uri.ParsingError.BadFormat;
									return idx;
								}
								newHost += text;
								if (!ServicePointManager.AllowAllUriEncodingExpansion && newHost.Length > 65535)
								{
									err = Uri.ParsingError.SizeLimit;
									return idx;
								}
							}
							else
							{
								text = new string(pString, num2, (int)num3 - num2 + 1);
							}
						}
						num3 += 1;
						c = pString[num3];
						break;
					}
					num3 += 1;
				}
			}
			bool flag5 = (flags2 & UriSyntaxFlags.SimpleUserSyntax) == (UriSyntaxFlags)0;
			if (c == '[' && syntax.InFact(UriSyntaxFlags.AllowIPv6Host) && IPv6AddressHelper.IsValid(pString, (int)(num3 + 1), ref num))
			{
				flags |= Uri.Flags.IPv6HostType;
				if (!Uri.s_ConfigInitialized)
				{
					Uri.InitializeUriConfig();
					this.m_iriParsing = Uri.s_IriParsing && Uri.IriParsingStatic(syntax);
				}
				if (flag3 && flag2 && flag4)
				{
					newHost += new string(pString, (int)num3, num - (int)num3);
					flags |= Uri.Flags.HostUnicodeNormalized;
					flag = true;
				}
			}
			else if (c <= '9' && c >= '0' && syntax.InFact(UriSyntaxFlags.AllowIPv4Host) && IPv4AddressHelper.IsValid(pString, (int)num3, ref num, false, Uri.StaticNotAny(flags, Uri.Flags.ImplicitFile)))
			{
				flags |= Uri.Flags.IPv4HostType;
				if (flag3 && flag2 && flag4)
				{
					newHost += new string(pString, (int)num3, num - (int)num3);
					flags |= Uri.Flags.HostUnicodeNormalized;
					flag = true;
				}
			}
			else if ((flags2 & UriSyntaxFlags.AllowDnsHost) != (UriSyntaxFlags)0 && !flag2 && DomainNameHelper.IsValid(pString, num3, ref num, ref flag5, Uri.StaticNotAny(flags, Uri.Flags.ImplicitFile)))
			{
				flags |= Uri.Flags.DnsHostType;
				if (!flag5)
				{
					flags |= Uri.Flags.CanonicalDnsHost;
				}
				if (Uri.s_IdnScope != UriIdnScope.None)
				{
					if (Uri.s_IdnScope == UriIdnScope.AllExceptIntranet && this.IsIntranet(new string(pString, 0, num)))
					{
						flags |= Uri.Flags.IntranetUri;
					}
					if (this.AllowIdnStatic(syntax, flags))
					{
						bool flag6 = true;
						bool flag7 = false;
						string text2 = DomainNameHelper.UnicodeEquivalent(pString, (int)num3, num, ref flag6, ref flag7);
						if (flag7)
						{
							if (Uri.StaticNotAny(flags, Uri.Flags.HasUnicode))
							{
								this.m_originalUnicodeString = this.m_String;
							}
							flags |= Uri.Flags.IdnHost;
							newHost = this.m_originalUnicodeString.Substring(0, num2) + text + text2;
							flags |= Uri.Flags.CanonicalDnsHost;
							this.m_DnsSafeHost = new string(pString, (int)num3, num - (int)num3);
							flag = true;
						}
						flags |= Uri.Flags.HostUnicodeNormalized;
					}
				}
			}
			else if ((flag2 || Uri.s_IdnScope != UriIdnScope.None) && (flags2 & UriSyntaxFlags.AllowDnsHost) != (UriSyntaxFlags)0 && ((flag2 && flag4) || this.AllowIdnStatic(syntax, flags)) && DomainNameHelper.IsValidByIri(pString, num3, ref num, ref flag5, Uri.StaticNotAny(flags, Uri.Flags.ImplicitFile)))
			{
				this.CheckAuthorityHelperHandleDnsIri(pString, num3, num, num2, flag2, flag3, syntax, text, ref flags, ref flag, ref newHost, ref err);
			}
			else if (Uri.s_IdnScope == UriIdnScope.None && !Uri.s_IriParsing && (flags2 & UriSyntaxFlags.AllowUncHost) != (UriSyntaxFlags)0 && UncNameHelper.IsValid(pString, num3, ref num, Uri.StaticNotAny(flags, Uri.Flags.ImplicitFile)) && num - (int)num3 <= 256)
			{
				flags |= Uri.Flags.UncHostType;
			}
			if (num < (int)length && pString[num] == '\\' && (flags & Uri.Flags.HostTypeMask) != Uri.Flags.Zero && !Uri.StaticIsFile(syntax))
			{
				if (syntax.InFact(UriSyntaxFlags.V1_UnknownUri))
				{
					err = Uri.ParsingError.BadHostName;
					flags |= Uri.Flags.HostTypeMask;
					return (ushort)num;
				}
				flags &= ~Uri.Flags.HostTypeMask;
			}
			else if (num < (int)length && pString[num] == ':')
			{
				if (syntax.InFact(UriSyntaxFlags.MayHavePort))
				{
					int num4 = 0;
					int num5 = num;
					idx = (ushort)(num + 1);
					while (idx < length)
					{
						ushort num6 = (ushort)(pString[idx] - '0');
						if (num6 >= 0 && num6 <= 9)
						{
							if ((num4 = num4 * 10 + (int)num6) > 65535)
							{
								break;
							}
							idx += 1;
						}
						else
						{
							if (num6 == 65535 || num6 == 15 || num6 == 65523)
							{
								break;
							}
							if (syntax.InFact(UriSyntaxFlags.AllowAnyOtherHost) && syntax.NotAny(UriSyntaxFlags.V1_UnknownUri))
							{
								flags &= ~Uri.Flags.HostTypeMask;
								break;
							}
							err = Uri.ParsingError.BadPort;
							return idx;
						}
					}
					if (num4 > 65535)
					{
						if (!syntax.InFact(UriSyntaxFlags.AllowAnyOtherHost))
						{
							err = Uri.ParsingError.BadPort;
							return idx;
						}
						flags &= ~Uri.Flags.HostTypeMask;
					}
					if (flag2 && flag3 && flag)
					{
						newHost += new string(pString, num5, (int)idx - num5);
					}
				}
				else
				{
					flags &= ~Uri.Flags.HostTypeMask;
				}
			}
			if ((flags & Uri.Flags.HostTypeMask) == Uri.Flags.Zero)
			{
				flags &= ~Uri.Flags.HasUserInfo;
				if (syntax.InFact(UriSyntaxFlags.AllowAnyOtherHost))
				{
					flags |= Uri.Flags.BasicHostType;
					num = (int)idx;
					while (num < (int)length && pString[num] != '/' && pString[num] != '?' && pString[num] != '#')
					{
						num++;
					}
					this.CheckAuthorityHelperHandleAnyHostIri(pString, num2, num, flag2, flag3, syntax, ref flags, ref newHost, ref err);
				}
				else if (syntax.InFact(UriSyntaxFlags.V1_UnknownUri))
				{
					bool flag8 = false;
					int num7 = (int)idx;
					num = (int)idx;
					while (num < (int)length && (!flag8 || (pString[num] != '/' && pString[num] != '?' && pString[num] != '#')))
					{
						if (num >= (int)(idx + 2) || pString[num] != '.')
						{
							err = Uri.ParsingError.BadHostName;
							flags |= Uri.Flags.HostTypeMask;
							return idx;
						}
						flag8 = true;
						num++;
					}
					flags |= Uri.Flags.BasicHostType;
					if (flag2 && flag3 && Uri.StaticNotAny(flags, Uri.Flags.HostUnicodeNormalized))
					{
						string text3 = new string(pString, num7, num7 - num);
						try
						{
							newHost += text3.Normalize(NormalizationForm.FormC);
						}
						catch (ArgumentException)
						{
							err = Uri.ParsingError.BadFormat;
							return idx;
						}
						flags |= Uri.Flags.HostUnicodeNormalized;
					}
				}
				else if (syntax.InFact(UriSyntaxFlags.MustHaveAuthority))
				{
					err = Uri.ParsingError.BadHostName;
					flags |= Uri.Flags.HostTypeMask;
					return idx;
				}
			}
			return (ushort)num;
		}

		// Token: 0x06001B25 RID: 6949 RVA: 0x00063040 File Offset: 0x00062040
		private unsafe void CheckAuthorityHelperHandleDnsIri(char* pString, ushort start, int end, int startInput, bool iriParsing, bool hasUnicode, UriParser syntax, string userInfoString, ref Uri.Flags flags, ref bool justNormalized, ref string newHost, ref Uri.ParsingError err)
		{
			flags |= Uri.Flags.DnsHostType;
			if (Uri.s_IdnScope == UriIdnScope.AllExceptIntranet && this.IsIntranet(new string(pString, 0, end)))
			{
				flags |= Uri.Flags.IntranetUri;
			}
			if (this.AllowIdnStatic(syntax, flags))
			{
				bool flag = true;
				bool flag2 = false;
				string text = DomainNameHelper.IdnEquivalent(pString, (int)start, end, ref flag, ref flag2);
				string text2 = DomainNameHelper.UnicodeEquivalent(text, pString, (int)start, end);
				if (!flag)
				{
					flags |= Uri.Flags.UnicodeHost;
				}
				if (flag2)
				{
					flags |= Uri.Flags.IdnHost;
				}
				if (flag && flag2 && Uri.StaticNotAny(flags, Uri.Flags.HasUnicode))
				{
					this.m_originalUnicodeString = this.m_String;
					newHost = this.m_originalUnicodeString.Substring(0, startInput) + (Uri.StaticInFact(flags, Uri.Flags.HasUserInfo) ? userInfoString : null);
					justNormalized = true;
				}
				else if (!iriParsing && (Uri.StaticInFact(flags, Uri.Flags.UnicodeHost) || Uri.StaticInFact(flags, Uri.Flags.IdnHost)))
				{
					this.m_originalUnicodeString = this.m_String;
					newHost = this.m_originalUnicodeString.Substring(0, startInput) + (Uri.StaticInFact(flags, Uri.Flags.HasUserInfo) ? userInfoString : null);
					justNormalized = true;
				}
				if (!flag || flag2)
				{
					this.m_DnsSafeHost = text;
					newHost += text2;
					justNormalized = true;
				}
				else if (flag && !flag2 && iriParsing && hasUnicode)
				{
					newHost += text2;
					justNormalized = true;
				}
			}
			else if (hasUnicode)
			{
				string text3 = Uri.StripBidiControlCharacter(pString, (int)start, end - (int)start);
				try
				{
					newHost += ((text3 != null) ? text3.Normalize(NormalizationForm.FormC) : null);
				}
				catch (ArgumentException)
				{
					err = Uri.ParsingError.BadHostName;
				}
				justNormalized = true;
			}
			flags |= Uri.Flags.HostUnicodeNormalized;
		}

		// Token: 0x06001B26 RID: 6950 RVA: 0x0006321C File Offset: 0x0006221C
		private unsafe void CheckAuthorityHelperHandleAnyHostIri(char* pString, int startInput, int end, bool iriParsing, bool hasUnicode, UriParser syntax, ref Uri.Flags flags, ref string newHost, ref Uri.ParsingError err)
		{
			if (Uri.StaticNotAny(flags, Uri.Flags.HostUnicodeNormalized) && (this.AllowIdnStatic(syntax, flags) || (iriParsing && hasUnicode)))
			{
				string text = new string(pString, startInput, end - startInput);
				if (this.AllowIdnStatic(syntax, flags))
				{
					bool flag = true;
					bool flag2 = false;
					string text2 = DomainNameHelper.UnicodeEquivalent(pString, startInput, end, ref flag, ref flag2);
					if (((flag && flag2) || !flag) && (!iriParsing || !hasUnicode))
					{
						this.m_originalUnicodeString = this.m_String;
						newHost = this.m_originalUnicodeString.Substring(0, startInput);
						flags |= Uri.Flags.HasUnicode;
					}
					if (flag2 || !flag)
					{
						newHost += text2;
						string text3 = null;
						this.m_DnsSafeHost = DomainNameHelper.IdnEquivalent(pString, startInput, end, ref flag, ref text3);
						if (flag2)
						{
							flags |= Uri.Flags.IdnHost;
						}
						if (!flag)
						{
							flags |= Uri.Flags.UnicodeHost;
						}
					}
					else if (iriParsing && hasUnicode)
					{
						newHost += text;
					}
				}
				else
				{
					try
					{
						newHost += text.Normalize(NormalizationForm.FormC);
					}
					catch (ArgumentException)
					{
						err = Uri.ParsingError.BadHostName;
					}
				}
				flags |= Uri.Flags.HostUnicodeNormalized;
			}
		}

		// Token: 0x06001B27 RID: 6951 RVA: 0x0006335C File Offset: 0x0006235C
		private unsafe void FindEndOfComponent(string input, ref ushort idx, ushort end, char delim)
		{
			fixed (char* ptr = input)
			{
				this.FindEndOfComponent(ptr, ref idx, end, delim);
			}
		}

		// Token: 0x06001B28 RID: 6952 RVA: 0x00063384 File Offset: 0x00062384
		private unsafe void FindEndOfComponent(char* str, ref ushort idx, ushort end, char delim)
		{
			ushort num;
			for (num = idx; num < end; num += 1)
			{
				char c = str[num];
				if (c == delim || (delim == '?' && c == '#' && this.m_Syntax != null && this.m_Syntax.InFact(UriSyntaxFlags.MayHaveFragment)))
				{
					break;
				}
			}
			idx = num;
		}

		// Token: 0x06001B29 RID: 6953 RVA: 0x000633D8 File Offset: 0x000623D8
		private unsafe Uri.Check CheckCanonical(char* str, ref ushort idx, ushort end, char delim)
		{
			Uri.Check check = Uri.Check.None;
			bool flag = false;
			bool flag2 = false;
			ushort num;
			for (num = idx; num < end; num += 1)
			{
				char c = str[num];
				if (c <= '\u001f' || (c >= '\u007f' && c <= '\u009f'))
				{
					flag = true;
					flag2 = true;
					check |= Uri.Check.ReservedFound;
				}
				else if (c > 'z' && c != '~')
				{
					if (this.m_iriParsing)
					{
						bool flag3 = false;
						check |= Uri.Check.FoundNonAscii;
						if (char.IsHighSurrogate(c))
						{
							if (num + 1 < end)
							{
								bool flag4 = false;
								flag3 = Uri.CheckIriUnicodeRange(c, str[num + 1], ref flag4, true);
							}
						}
						else
						{
							flag3 = Uri.CheckIriUnicodeRange(c, true);
						}
						if (!flag3)
						{
							check |= Uri.Check.NotIriCanonical;
						}
					}
					if (!flag)
					{
						flag = true;
					}
				}
				else
				{
					if (c == delim || (delim == '?' && c == '#' && this.m_Syntax != null && this.m_Syntax.InFact(UriSyntaxFlags.MayHaveFragment)))
					{
						break;
					}
					if (c == '?')
					{
						if (this.IsImplicitFile || (this.m_Syntax != null && !this.m_Syntax.InFact(UriSyntaxFlags.MayHaveQuery) && delim != '\ufffe'))
						{
							check |= Uri.Check.ReservedFound;
							flag2 = true;
							flag = true;
						}
					}
					else if (c == '#')
					{
						flag = true;
						if (this.IsImplicitFile || (this.m_Syntax != null && !this.m_Syntax.InFact(UriSyntaxFlags.MayHaveFragment)))
						{
							check |= Uri.Check.ReservedFound;
							flag2 = true;
						}
					}
					else if (c == '/' || c == '\\')
					{
						if ((check & Uri.Check.BackslashInPath) == Uri.Check.None && c == '\\')
						{
							check |= Uri.Check.BackslashInPath;
						}
						if ((check & Uri.Check.DotSlashAttn) == Uri.Check.None && num + 1 != end && (str[num + 1] == '/' || str[num + 1] == '\\'))
						{
							check |= Uri.Check.DotSlashAttn;
						}
					}
					else if (c == '.')
					{
						if (((check & Uri.Check.DotSlashAttn) == Uri.Check.None && num + 1 == end) || str[num + 1] == '.' || str[num + 1] == '/' || str[num + 1] == '\\' || str[num + 1] == '?' || str[num + 1] == '#')
						{
							check |= Uri.Check.DotSlashAttn;
						}
					}
					else if (!flag && ((c <= '"' && c != '!') || (c >= '[' && c <= '^') || c == '>' || c == '<' || c == '`'))
					{
						flag = true;
					}
					else if (c == '%')
					{
						if (!flag2)
						{
							flag2 = true;
						}
						if (num + 2 < end && (c = Uri.EscapedAscii(str[num + 1], str[num + 2])) != '\uffff')
						{
							if (c == '.' || c == '/' || c == '\\')
							{
								check |= Uri.Check.DotSlashEscaped;
							}
							num += 2;
						}
						else if (!flag)
						{
							flag = true;
						}
					}
				}
			}
			if (flag2)
			{
				if (!flag)
				{
					check |= Uri.Check.EscapedCanonical;
				}
			}
			else
			{
				check |= Uri.Check.DisplayCanonical;
				if (!flag)
				{
					check |= Uri.Check.EscapedCanonical;
				}
			}
			idx = num;
			return check;
		}

		// Token: 0x06001B2A RID: 6954 RVA: 0x00063694 File Offset: 0x00062694
		private unsafe char[] GetCanonicalPath(char[] dest, ref int pos, UriFormat formatAs)
		{
			if (this.InFact(Uri.Flags.FirstSlashAbsent))
			{
				dest[pos++] = '/';
			}
			if (this.m_Info.Offset.Path == this.m_Info.Offset.Query)
			{
				return dest;
			}
			int num = pos;
			int securedPathIndex = (int)this.SecuredPathIndex;
			if (formatAs == UriFormat.UriEscaped)
			{
				if (this.InFact(Uri.Flags.ShouldBeCompressed))
				{
					this.m_String.CopyTo((int)this.m_Info.Offset.Path, dest, num, (int)(this.m_Info.Offset.Query - this.m_Info.Offset.Path));
					num += (int)(this.m_Info.Offset.Query - this.m_Info.Offset.Path);
					if (this.m_Syntax.InFact(UriSyntaxFlags.UnEscapeDotsAndSlashes) && this.InFact(Uri.Flags.PathNotCanonical) && !this.IsImplicitFile)
					{
						fixed (char* ptr = dest)
						{
							Uri.UnescapeOnly(ptr, pos, ref num, '.', '/', this.m_Syntax.InFact(UriSyntaxFlags.ConvertPathSlashes) ? '\\' : char.MaxValue);
						}
					}
				}
				else if (this.InFact(Uri.Flags.E_PathNotCanonical) && this.NotAny(Uri.Flags.UserEscaped))
				{
					string text = this.m_String;
					if (securedPathIndex != 0 && text[securedPathIndex + (int)this.m_Info.Offset.Path - 1] == '|')
					{
						text = text.Remove(securedPathIndex + (int)this.m_Info.Offset.Path - 1, 1);
						text = text.Insert(securedPathIndex + (int)this.m_Info.Offset.Path - 1, ":");
					}
					dest = Uri.EscapeString(text, (int)this.m_Info.Offset.Path, (int)this.m_Info.Offset.Query, dest, ref num, true, '?', '#', this.IsImplicitFile ? char.MaxValue : '%');
				}
				else
				{
					this.m_String.CopyTo((int)this.m_Info.Offset.Path, dest, num, (int)(this.m_Info.Offset.Query - this.m_Info.Offset.Path));
					num += (int)(this.m_Info.Offset.Query - this.m_Info.Offset.Path);
				}
			}
			else
			{
				this.m_String.CopyTo((int)this.m_Info.Offset.Path, dest, num, (int)(this.m_Info.Offset.Query - this.m_Info.Offset.Path));
				num += (int)(this.m_Info.Offset.Query - this.m_Info.Offset.Path);
				if (this.InFact(Uri.Flags.ShouldBeCompressed) && this.m_Syntax.InFact(UriSyntaxFlags.UnEscapeDotsAndSlashes) && this.InFact(Uri.Flags.PathNotCanonical) && !this.IsImplicitFile)
				{
					fixed (char* ptr2 = dest)
					{
						Uri.UnescapeOnly(ptr2, pos, ref num, '.', '/', this.m_Syntax.InFact(UriSyntaxFlags.ConvertPathSlashes) ? '\\' : char.MaxValue);
					}
				}
			}
			if (securedPathIndex != 0 && dest[securedPathIndex + pos - 1] == '|')
			{
				dest[securedPathIndex + pos - 1] = ':';
			}
			if (this.InFact(Uri.Flags.ShouldBeCompressed))
			{
				dest = Uri.Compress(dest, (ushort)(pos + securedPathIndex), ref num, this.m_Syntax);
				if (dest[pos] == '\\')
				{
					dest[pos] = '/';
				}
				if (formatAs == UriFormat.UriEscaped && this.NotAny(Uri.Flags.UserEscaped) && this.InFact(Uri.Flags.E_PathNotCanonical))
				{
					string text2 = new string(dest, pos, num - pos);
					dest = Uri.EscapeString(text2, 0, num - pos, dest, ref pos, true, '?', '#', this.IsImplicitFile ? char.MaxValue : '%');
					num = pos;
				}
			}
			else if (this.m_Syntax.InFact(UriSyntaxFlags.ConvertPathSlashes) && this.InFact(Uri.Flags.BackslashInPath))
			{
				for (int i = pos; i < num; i++)
				{
					if (dest[i] == '\\')
					{
						dest[i] = '/';
					}
				}
			}
			if (formatAs != UriFormat.UriEscaped && this.InFact(Uri.Flags.PathNotCanonical))
			{
				Uri.UnescapeMode unescapeMode;
				if (this.InFact(Uri.Flags.PathNotCanonical))
				{
					if (formatAs != UriFormat.Unescaped)
					{
						if (formatAs == (UriFormat)32767)
						{
							unescapeMode = (this.InFact(Uri.Flags.UserEscaped) ? Uri.UnescapeMode.Unescape : Uri.UnescapeMode.EscapeUnescape) | Uri.UnescapeMode.V1ToStringFlag;
							if (this.IsImplicitFile)
							{
								unescapeMode &= ~Uri.UnescapeMode.Unescape;
							}
						}
						else
						{
							unescapeMode = (this.InFact(Uri.Flags.UserEscaped) ? Uri.UnescapeMode.Unescape : Uri.UnescapeMode.EscapeUnescape);
							if (this.IsImplicitFile)
							{
								unescapeMode &= ~Uri.UnescapeMode.Unescape;
							}
						}
					}
					else
					{
						unescapeMode = (this.IsImplicitFile ? Uri.UnescapeMode.CopyOnly : (Uri.UnescapeMode.Unescape | Uri.UnescapeMode.UnescapeAll));
					}
				}
				else
				{
					unescapeMode = Uri.UnescapeMode.CopyOnly;
				}
				char[] array = new char[dest.Length];
				Buffer.BlockCopy(dest, 0, array, 0, num << 1);
				fixed (char* ptr3 = array)
				{
					dest = Uri.UnescapeString(ptr3, pos, num, dest, ref pos, '?', '#', char.MaxValue, unescapeMode, this.m_Syntax, false, false);
				}
			}
			else
			{
				pos = num;
			}
			return dest;
		}

		// Token: 0x06001B2B RID: 6955 RVA: 0x00063BD0 File Offset: 0x00062BD0
		private unsafe static void UnescapeOnly(char* pch, int start, ref int end, char ch1, char ch2, char ch3)
		{
			if (end - start < 3)
			{
				return;
			}
			char* ptr = pch + end - 2;
			pch += start;
			char* ptr2 = null;
			while (pch < ptr)
			{
				if (*(pch++) == '%')
				{
					char c = Uri.EscapedAscii(*(pch++), *(pch++));
					if (c == ch1 || c == ch2 || c == ch3)
					{
						ptr2 = pch - 2;
						*(ptr2 - 1) = c;
						while (pch < ptr)
						{
							if ((*(ptr2++) = *(pch++)) == '%')
							{
								c = Uri.EscapedAscii(*(ptr2++) = *(pch++), *(ptr2++) = *(pch++));
								if (c == ch1 || c == ch2 || c == ch3)
								{
									ptr2 -= 2;
									*(ptr2 - 1) = c;
								}
							}
						}
						break;
					}
				}
			}
			ptr += 2;
			if (ptr2 == null)
			{
				return;
			}
			if (pch == ptr)
			{
				end -= (int)((long)(pch - ptr2));
				return;
			}
			*(ptr2++) = *(pch++);
			if (pch == ptr)
			{
				end -= (int)((long)(pch - ptr2));
				return;
			}
			*(ptr2++) = *(pch++);
			end -= (int)((long)(pch - ptr2));
		}

		// Token: 0x06001B2C RID: 6956 RVA: 0x00063CF8 File Offset: 0x00062CF8
		private static char EscapedAscii(char digit, char next)
		{
			if ((digit < '0' || digit > '9') && (digit < 'A' || digit > 'F') && (digit < 'a' || digit > 'f'))
			{
				return char.MaxValue;
			}
			int num = (int)((digit <= '9') ? (digit - '0') : (((digit <= 'F') ? (digit - 'A') : (digit - 'a')) + '\n'));
			if ((next < '0' || next > '9') && (next < 'A' || next > 'F') && (next < 'a' || next > 'f'))
			{
				return char.MaxValue;
			}
			return (char)((num << 4) + (int)((next <= '9') ? (next - '0') : (((next <= 'F') ? (next - 'A') : (next - 'a')) + '\n')));
		}

		// Token: 0x06001B2D RID: 6957 RVA: 0x00063D90 File Offset: 0x00062D90
		private static char[] Compress(char[] dest, ushort start, ref int destLength, UriParser syntax)
		{
			ushort num = 0;
			ushort num2 = 0;
			ushort num3 = 0;
			ushort num4 = 0;
			ushort num5 = (ushort)destLength - 1;
			start -= 1;
			while (num5 != start)
			{
				char c = dest[(int)num5];
				if (c == '\\' && syntax.InFact(UriSyntaxFlags.ConvertPathSlashes))
				{
					c = (dest[(int)num5] = '/');
				}
				if (c == '/')
				{
					num += 1;
				}
				else
				{
					if (num > 1)
					{
						num2 = num5 + 1;
					}
					num = 0;
				}
				if (c == '.')
				{
					num3 += 1;
				}
				else
				{
					if (num3 != 0)
					{
						bool flag = syntax.NotAny(UriSyntaxFlags.CanonicalizeAsFilePath) && (num3 > 2 || c != '/' || num5 == start);
						if (!flag && c == '/')
						{
							if (num2 == num5 + num3 + 1 || (num2 == 0 && (int)(num5 + num3 + 1) == destLength))
							{
								num2 = num5 + 1 + num3 + ((num2 == 0) ? 0 : 1);
								Buffer.BlockCopy(dest, (int)num2 << 1, dest, (int)(num5 + 1) << 1, destLength - (int)num2 << 1);
								destLength -= (int)(num2 - num5 - 1);
								num2 = num5;
								if (num3 == 2)
								{
									num4 += 1;
								}
								num3 = 0;
								goto IL_017F;
							}
						}
						else if (!flag && num4 == 0 && (num2 == num5 + num3 + 1 || (num2 == 0 && (int)(num5 + num3 + 1) == destLength)))
						{
							num3 = num5 + 1 + num3;
							Buffer.BlockCopy(dest, (int)num3 << 1, dest, (int)(num5 + 1) << 1, destLength - (int)num3 << 1);
							destLength -= (int)(num3 - num5 - 1);
							num2 = 0;
							num3 = 0;
							goto IL_017F;
						}
						num3 = 0;
					}
					if (c == '/')
					{
						if (num4 != 0)
						{
							num4 -= 1;
							num2 += 1;
							Buffer.BlockCopy(dest, (int)num2 << 1, dest, (int)(num5 + 1) << 1, destLength - (int)num2 << 1);
							destLength -= (int)(num2 - num5 - 1);
						}
						num2 = num5;
					}
				}
				IL_017F:
				num5 -= 1;
			}
			start += 1;
			if ((ushort)destLength > start && syntax.InFact(UriSyntaxFlags.CanonicalizeAsFilePath) && num <= 1)
			{
				if (num4 != 0 && dest[(int)start] != '/')
				{
					num2 += 1;
					Buffer.BlockCopy(dest, (int)num2 << 1, dest, (int)start << 1, destLength - (int)num2 << 1);
					destLength -= (int)num2;
				}
				else if (num3 != 0 && (num2 == num3 + 1 || (num2 == 0 && (int)(num3 + 1) == destLength)))
				{
					num3 += ((num2 == 0) ? 0 : 1);
					Buffer.BlockCopy(dest, (int)num3 << 1, dest, (int)start << 1, destLength - (int)num3 << 1);
					destLength -= (int)num3;
				}
			}
			return dest;
		}

		// Token: 0x06001B2E RID: 6958 RVA: 0x00063FAC File Offset: 0x00062FAC
		private static void EscapeAsciiChar(char ch, char[] to, ref int pos)
		{
			to[pos++] = '%';
			to[pos++] = Uri.HexUpperChars[(int)((ch & 'ð') >> 4)];
			to[pos++] = Uri.HexUpperChars[(int)(ch & '\u000f')];
		}

		// Token: 0x06001B2F RID: 6959 RVA: 0x00063FF5 File Offset: 0x00062FF5
		internal static int CalculateCaseInsensitiveHashCode(string text)
		{
			return StringComparer.InvariantCultureIgnoreCase.GetHashCode(text);
		}

		// Token: 0x06001B30 RID: 6960 RVA: 0x00064004 File Offset: 0x00063004
		private static string CombineUri(Uri basePart, string relativePart, UriFormat uriFormat)
		{
			char c = relativePart[0];
			if (basePart.IsDosPath && (c == '/' || c == '\\') && (relativePart.Length == 1 || (relativePart[1] != '/' && relativePart[1] != '\\')))
			{
				int num = basePart.OriginalString.IndexOf(':');
				if (basePart.IsImplicitFile)
				{
					return basePart.OriginalString.Substring(0, num + 1) + relativePart;
				}
				num = basePart.OriginalString.IndexOf(':', num + 1);
				return basePart.OriginalString.Substring(0, num + 1) + relativePart;
			}
			else if (Uri.StaticIsFile(basePart.Syntax) && (c == '\\' || c == '/'))
			{
				if (relativePart.Length >= 2 && (relativePart[1] == '\\' || relativePart[1] == '/'))
				{
					if (!basePart.IsImplicitFile)
					{
						return "file:" + relativePart;
					}
					return relativePart;
				}
				else
				{
					if (!basePart.IsUnc)
					{
						return "file://" + relativePart;
					}
					string text = basePart.GetParts(UriComponents.Path | UriComponents.KeepDelimiter, UriFormat.Unescaped);
					for (int i = 1; i < text.Length; i++)
					{
						if (text[i] == '/')
						{
							text = text.Substring(0, i);
							break;
						}
					}
					if (basePart.IsImplicitFile)
					{
						return "\\\\" + basePart.GetParts(UriComponents.Host, UriFormat.Unescaped) + text + relativePart;
					}
					return "file://" + basePart.GetParts(UriComponents.Host, uriFormat) + text + relativePart;
				}
			}
			else
			{
				bool flag = basePart.Syntax.InFact(UriSyntaxFlags.ConvertPathSlashes);
				string text2;
				if (c != '/' && (c != '\\' || !flag))
				{
					text2 = basePart.GetParts(UriComponents.Path | UriComponents.KeepDelimiter, basePart.IsImplicitFile ? UriFormat.Unescaped : uriFormat);
					int j = text2.Length;
					char[] array = new char[j + relativePart.Length];
					if (j > 0)
					{
						text2.CopyTo(0, array, 0, j);
						while (j > 0)
						{
							if (array[--j] == '/')
							{
								j++;
								break;
							}
						}
					}
					relativePart.CopyTo(0, array, j, relativePart.Length);
					c = (basePart.Syntax.InFact(UriSyntaxFlags.MayHaveQuery) ? '?' : char.MaxValue);
					char c2 = ((!basePart.IsImplicitFile && basePart.Syntax.InFact(UriSyntaxFlags.MayHaveFragment)) ? '#' : char.MaxValue);
					string text3 = string.Empty;
					if (c != '\uffff' || c2 != '\uffff')
					{
						int num2 = 0;
						while (num2 < relativePart.Length && array[j + num2] != c && array[j + num2] != c2)
						{
							num2++;
						}
						if (num2 == 0)
						{
							text3 = relativePart;
						}
						else if (num2 < relativePart.Length)
						{
							text3 = relativePart.Substring(num2);
						}
						j += num2;
					}
					else
					{
						j += relativePart.Length;
					}
					if (basePart.HostType == Uri.Flags.IPv6HostType)
					{
						if (basePart.IsImplicitFile)
						{
							text2 = "\\\\[" + basePart.DnsSafeHost + ']';
						}
						else
						{
							text2 = string.Concat(new object[]
							{
								basePart.GetParts(UriComponents.Scheme | UriComponents.UserInfo, uriFormat),
								'[',
								basePart.DnsSafeHost,
								']',
								basePart.GetParts(UriComponents.Port | UriComponents.KeepDelimiter, uriFormat)
							});
						}
					}
					else if (basePart.IsImplicitFile)
					{
						if (basePart.IsDosPath)
						{
							array = Uri.Compress(array, 3, ref j, basePart.Syntax);
							return new string(array, 1, j - 1) + text3;
						}
						text2 = "\\\\" + basePart.GetParts(UriComponents.Host, UriFormat.Unescaped);
					}
					else
					{
						text2 = basePart.GetParts(UriComponents.Scheme | UriComponents.UserInfo | UriComponents.Host | UriComponents.Port, uriFormat);
					}
					array = Uri.Compress(array, basePart.SecuredPathIndex, ref j, basePart.Syntax);
					return text2 + new string(array, 0, j) + text3;
				}
				if (relativePart.Length >= 2 && relativePart[1] == '/')
				{
					return basePart.Scheme + ':' + relativePart;
				}
				if (basePart.HostType == Uri.Flags.IPv6HostType)
				{
					text2 = string.Concat(new object[]
					{
						basePart.GetParts(UriComponents.Scheme | UriComponents.UserInfo, uriFormat),
						'[',
						basePart.DnsSafeHost,
						']',
						basePart.GetParts(UriComponents.Port | UriComponents.KeepDelimiter, uriFormat)
					});
				}
				else
				{
					text2 = basePart.GetParts(UriComponents.Scheme | UriComponents.UserInfo | UriComponents.Host | UriComponents.Port, uriFormat);
				}
				if (flag && c == '\\')
				{
					relativePart = '/' + relativePart.Substring(1);
				}
				return text2 + relativePart;
			}
		}

		// Token: 0x06001B31 RID: 6961 RVA: 0x00064470 File Offset: 0x00063470
		private static string PathDifference(string path1, string path2, bool compareCase)
		{
			int num = -1;
			int i = 0;
			while (i < path1.Length && i < path2.Length && (path1[i] == path2[i] || (!compareCase && char.ToLower(path1[i], CultureInfo.InvariantCulture) == char.ToLower(path2[i], CultureInfo.InvariantCulture))))
			{
				if (path1[i] == '/')
				{
					num = i;
				}
				i++;
			}
			if (i == 0)
			{
				return path2;
			}
			if (i == path1.Length && i == path2.Length)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			while (i < path1.Length)
			{
				if (path1[i] == '/')
				{
					stringBuilder.Append("../");
				}
				i++;
			}
			return stringBuilder.ToString() + path2.Substring(num + 1);
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06001B32 RID: 6962 RVA: 0x0006453A File Offset: 0x0006353A
		internal bool HasAuthority
		{
			get
			{
				return this.InFact(Uri.Flags.AuthorityFound);
			}
		}

		// Token: 0x06001B33 RID: 6963 RVA: 0x00064548 File Offset: 0x00063548
		private static bool IsLWS(char ch)
		{
			return ch <= ' ' && (ch == ' ' || ch == '\n' || ch == '\r' || ch == '\t');
		}

		// Token: 0x06001B34 RID: 6964 RVA: 0x00064567 File Offset: 0x00063567
		private static bool IsAsciiLetter(char character)
		{
			return (character >= 'a' && character <= 'z') || (character >= 'A' && character <= 'Z');
		}

		// Token: 0x06001B35 RID: 6965 RVA: 0x00064584 File Offset: 0x00063584
		private static bool IsAsciiLetterOrDigit(char character)
		{
			return Uri.IsAsciiLetter(character) || (character >= '0' && character <= '9');
		}

		// Token: 0x06001B36 RID: 6966 RVA: 0x0006459F File Offset: 0x0006359F
		internal static bool IsGenDelim(char ch)
		{
			return ch == ':' || ch == '/' || ch == '?' || ch == '#' || ch == '[' || ch == ']' || ch == '@';
		}

		// Token: 0x06001B37 RID: 6967 RVA: 0x000645C6 File Offset: 0x000635C6
		internal static bool IsBidiControlCharacter(char ch)
		{
			return ch == '\u200e' || ch == '\u200f' || ch == '\u202a' || ch == '\u202b' || ch == '\u202c' || ch == '\u202d' || ch == '\u202e';
		}

		// Token: 0x06001B38 RID: 6968 RVA: 0x00064604 File Offset: 0x00063604
		internal unsafe static string StripBidiControlCharacter(char* strToClean, int start, int length)
		{
			if (length <= 0)
			{
				return "";
			}
			char[] array = new char[length];
			int num = 0;
			for (int i = 0; i < length; i++)
			{
				char c = strToClean[start + i];
				if (c < '\u200e' || c > '\u202e' || !Uri.IsBidiControlCharacter(c))
				{
					array[num++] = c;
				}
			}
			return new string(array, 0, num);
		}

		// Token: 0x06001B39 RID: 6969 RVA: 0x00064662 File Offset: 0x00063662
		[Obsolete("The method has been deprecated. It is not used by the system. http://go.microsoft.com/fwlink/?linkid=14202")]
		protected virtual void Parse()
		{
		}

		// Token: 0x06001B3A RID: 6970 RVA: 0x00064664 File Offset: 0x00063664
		[Obsolete("The method has been deprecated. It is not used by the system. http://go.microsoft.com/fwlink/?linkid=14202")]
		protected virtual void Canonicalize()
		{
		}

		// Token: 0x06001B3B RID: 6971 RVA: 0x00064666 File Offset: 0x00063666
		[Obsolete("The method has been deprecated. It is not used by the system. http://go.microsoft.com/fwlink/?linkid=14202")]
		protected virtual void Escape()
		{
		}

		// Token: 0x06001B3C RID: 6972 RVA: 0x00064668 File Offset: 0x00063668
		[Obsolete("The method has been deprecated. Please use GetComponents() or static UnescapeDataString() to unescape a Uri component or a string. http://go.microsoft.com/fwlink/?linkid=14202")]
		protected virtual string Unescape(string path)
		{
			char[] array = new char[path.Length];
			int num = 0;
			array = Uri.UnescapeString(path, 0, path.Length, array, ref num, char.MaxValue, char.MaxValue, char.MaxValue, Uri.UnescapeMode.Unescape | Uri.UnescapeMode.UnescapeAll, null, false, true);
			return new string(array, 0, num);
		}

		// Token: 0x06001B3D RID: 6973 RVA: 0x000646B0 File Offset: 0x000636B0
		[Obsolete("The method has been deprecated. Please use GetComponents() or static EscapeUriString() to escape a Uri component or a string. http://go.microsoft.com/fwlink/?linkid=14202")]
		protected static string EscapeString(string str)
		{
			if (str == null)
			{
				return string.Empty;
			}
			int num = 0;
			char[] array = Uri.EscapeString(str, 0, str.Length, null, ref num, true, '?', '#', '%');
			if (array == null)
			{
				return str;
			}
			return new string(array, 0, num);
		}

		// Token: 0x06001B3E RID: 6974 RVA: 0x000646ED File Offset: 0x000636ED
		[Obsolete("The method has been deprecated. It is not used by the system. http://go.microsoft.com/fwlink/?linkid=14202")]
		protected virtual void CheckSecurity()
		{
			this.Scheme == "telnet";
		}

		// Token: 0x06001B3F RID: 6975 RVA: 0x00064700 File Offset: 0x00063700
		[Obsolete("The method has been deprecated. It is not used by the system. http://go.microsoft.com/fwlink/?linkid=14202")]
		protected virtual bool IsReservedCharacter(char character)
		{
			return character == ';' || character == '/' || character == ':' || character == '@' || character == '&' || character == '=' || character == '+' || character == '$' || character == ',';
		}

		// Token: 0x06001B40 RID: 6976 RVA: 0x00064734 File Offset: 0x00063734
		[Obsolete("The method has been deprecated. It is not used by the system. http://go.microsoft.com/fwlink/?linkid=14202")]
		protected static bool IsExcludedCharacter(char character)
		{
			return character <= ' ' || character >= '\u007f' || character == '<' || character == '>' || character == '#' || character == '%' || character == '"' || character == '{' || character == '}' || character == '|' || character == '\\' || character == '^' || character == '[' || character == ']' || character == '`';
		}

		// Token: 0x06001B41 RID: 6977 RVA: 0x00064790 File Offset: 0x00063790
		[Obsolete("The method has been deprecated. It is not used by the system. http://go.microsoft.com/fwlink/?linkid=14202")]
		protected virtual bool IsBadFileSystemCharacter(char character)
		{
			return character < ' ' || character == ';' || character == '/' || character == '?' || character == ':' || character == '&' || character == '=' || character == ',' || character == '*' || character == '<' || character == '>' || character == '"' || character == '|' || character == '\\' || character == '^';
		}

		// Token: 0x06001B42 RID: 6978 RVA: 0x000647EC File Offset: 0x000637EC
		private void CreateThis(string uri, bool dontEscape, UriKind uriKind)
		{
			if (uriKind < UriKind.RelativeOrAbsolute || uriKind > UriKind.Relative)
			{
				throw new ArgumentException(SR.GetString("net_uri_InvalidUriKind", new object[] { uriKind }));
			}
			this.m_String = ((uri == null) ? string.Empty : uri);
			if (dontEscape)
			{
				this.m_Flags |= Uri.Flags.UserEscaped;
			}
			Uri.ParsingError parsingError = Uri.ParseScheme(this.m_String, ref this.m_Flags, ref this.m_Syntax);
			UriFormatException ex;
			this.InitializeUri(parsingError, uriKind, out ex);
			if (ex != null)
			{
				throw ex;
			}
		}

		// Token: 0x06001B43 RID: 6979 RVA: 0x00064870 File Offset: 0x00063870
		private void InitializeUri(Uri.ParsingError err, UriKind uriKind, out UriFormatException e)
		{
			if (err == Uri.ParsingError.None)
			{
				if (this.IsImplicitFile)
				{
					if (this.NotAny(Uri.Flags.DosPath) && uriKind != UriKind.Absolute && (uriKind == UriKind.Relative || (this.m_String.Length >= 2 && (this.m_String[0] != '\\' || this.m_String[1] != '\\'))))
					{
						this.m_Syntax = null;
						this.m_Flags &= Uri.Flags.UserEscaped;
						e = null;
						return;
					}
					if (uriKind == UriKind.Relative && this.InFact(Uri.Flags.DosPath))
					{
						this.m_Syntax = null;
						this.m_Flags &= Uri.Flags.UserEscaped;
						e = null;
						return;
					}
				}
			}
			else if (err > Uri.ParsingError.EmptyUriString)
			{
				this.m_String = null;
				e = Uri.GetException(err);
				return;
			}
			bool flag = false;
			if (!Uri.s_ConfigInitialized && this.CheckForConfigLoad(this.m_String))
			{
				Uri.InitializeUriConfig();
			}
			this.m_iriParsing = Uri.s_IriParsing && (this.m_Syntax == null || this.m_Syntax.InFact(UriSyntaxFlags.AllowIriParsing));
			if (this.m_iriParsing && this.CheckForUnicode(this.m_String))
			{
				this.m_Flags |= Uri.Flags.HasUnicode;
				flag = true;
				this.m_originalUnicodeString = this.m_String;
			}
			if (this.m_Syntax != null)
			{
				if (this.m_Syntax.IsSimple)
				{
					if ((err = this.PrivateParseMinimal()) != Uri.ParsingError.None)
					{
						if (uriKind != UriKind.Absolute && err <= Uri.ParsingError.EmptyUriString)
						{
							this.m_Syntax = null;
							e = null;
							this.m_Flags &= Uri.Flags.UserEscaped;
						}
						else
						{
							e = Uri.GetException(err);
						}
					}
					else if (uriKind == UriKind.Relative)
					{
						e = Uri.GetException(Uri.ParsingError.CannotCreateRelative);
					}
					else
					{
						e = null;
					}
					if (!this.m_iriParsing || !flag)
					{
						return;
					}
					try
					{
						this.EnsureParseRemaining();
						return;
					}
					catch (UriFormatException ex)
					{
						if (ServicePointManager.AllowAllUriEncodingExpansion)
						{
							throw;
						}
						e = ex;
						return;
					}
				}
				this.m_Syntax = this.m_Syntax.InternalOnNewUri();
				this.m_Flags |= Uri.Flags.UserDrivenParsing;
				this.m_Syntax.InternalValidate(this, out e);
				if (e != null)
				{
					if (uriKind != UriKind.Absolute && err != Uri.ParsingError.None && err <= Uri.ParsingError.EmptyUriString)
					{
						this.m_Syntax = null;
						e = null;
						this.m_Flags &= Uri.Flags.UserEscaped;
						return;
					}
					return;
				}
				else
				{
					if (err != Uri.ParsingError.None || this.InFact(Uri.Flags.ErrorOrParsingRecursion))
					{
						this.SetUserDrivenParsing();
					}
					else if (uriKind == UriKind.Relative)
					{
						e = Uri.GetException(Uri.ParsingError.CannotCreateRelative);
					}
					if (!this.m_iriParsing || !flag)
					{
						return;
					}
					try
					{
						this.EnsureParseRemaining();
						return;
					}
					catch (UriFormatException ex2)
					{
						if (ServicePointManager.AllowAllUriEncodingExpansion)
						{
							throw;
						}
						e = ex2;
						return;
					}
				}
			}
			if (err != Uri.ParsingError.None && uriKind != UriKind.Absolute && err <= Uri.ParsingError.EmptyUriString)
			{
				e = null;
				this.m_Flags &= Uri.Flags.UserEscaped | Uri.Flags.HasUnicode;
				if (!this.m_iriParsing || !flag)
				{
					return;
				}
				this.m_String = this.EscapeUnescapeIri(this.m_originalUnicodeString, 0, this.m_originalUnicodeString.Length, (UriComponents)0);
				try
				{
					this.m_String = this.m_String.Normalize(NormalizationForm.FormC);
					return;
				}
				catch (ArgumentException)
				{
					e = Uri.GetException(Uri.ParsingError.BadFormat);
					return;
				}
			}
			this.m_String = null;
			e = Uri.GetException(err);
		}

		// Token: 0x06001B44 RID: 6980 RVA: 0x00064BA8 File Offset: 0x00063BA8
		private unsafe bool CheckForConfigLoad(string data)
		{
			bool flag = false;
			int length = data.Length;
			fixed (char* ptr = data)
			{
				for (int i = 0; i < length; i++)
				{
					if (ptr[i] > '\u007f' || ptr[i] == '%' || (ptr[i] == 'x' && i + 3 < length && ptr[i + 1] == 'n' && ptr[i + 2] == '-' && ptr[i + 3] == '-'))
					{
						flag = true;
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x06001B45 RID: 6981 RVA: 0x00064C30 File Offset: 0x00063C30
		private unsafe bool CheckForUnicode(string data)
		{
			bool flag = false;
			char[] array = new char[data.Length];
			int num = 0;
			array = Uri.UnescapeString(data, 0, data.Length, array, ref num, char.MaxValue, char.MaxValue, char.MaxValue, Uri.UnescapeMode.Unescape | Uri.UnescapeMode.UnescapeAll, null, false, false);
			string text = new string(array, 0, num);
			int length = text.Length;
			fixed (char* ptr = text)
			{
				for (int i = 0; i < length; i++)
				{
					if (ptr[i] > '\u007f')
					{
						flag = true;
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x06001B46 RID: 6982 RVA: 0x00064CBC File Offset: 0x00063CBC
		internal static bool CheckIriUnicodeRange(string uri, int offset, ref bool surrogatePair, bool isQuery)
		{
			char maxValue = char.MaxValue;
			return Uri.CheckIriUnicodeRange(uri[offset], (offset + 1 < uri.Length) ? uri[offset + 1] : maxValue, ref surrogatePair, isQuery);
		}

		// Token: 0x06001B47 RID: 6983 RVA: 0x00064CF4 File Offset: 0x00063CF4
		internal static bool CheckIriUnicodeRange(char unicode, bool isQuery)
		{
			return (unicode >= '\u00a0' && unicode <= '\ud7ff') || (unicode >= '豈' && unicode <= '﷏') || (unicode >= 'ﷰ' && unicode <= '\uffef') || (isQuery && unicode >= '\ue000' && unicode <= '\uf8ff');
		}

		// Token: 0x06001B48 RID: 6984 RVA: 0x00064D48 File Offset: 0x00063D48
		internal static bool CheckIriUnicodeRange(char highSurr, char lowSurr, ref bool surrogatePair, bool isQuery)
		{
			bool flag = false;
			surrogatePair = false;
			if (Uri.CheckIriUnicodeRange(highSurr, isQuery))
			{
				flag = true;
			}
			else if (char.IsHighSurrogate(highSurr) && char.IsSurrogatePair(highSurr, lowSurr))
			{
				surrogatePair = true;
				char[] array = new char[] { highSurr, lowSurr };
				string text = new string(array);
				if ((text.CompareTo("\ud800\udc00") >= 0 && text.CompareTo("\ud83f\udffd") <= 0) || (text.CompareTo("\ud840\udc00") >= 0 && text.CompareTo("\ud87f\udffd") <= 0) || (text.CompareTo("\ud880\udc00") >= 0 && text.CompareTo("\ud8bf\udffd") <= 0) || (text.CompareTo("\ud8c0\udc00") >= 0 && text.CompareTo("\ud8ff\udffd") <= 0) || (text.CompareTo("\ud900\udc00") >= 0 && text.CompareTo("\ud93f\udffd") <= 0) || (text.CompareTo("\ud940\udc00") >= 0 && text.CompareTo("\ud97f\udffd") <= 0) || (text.CompareTo("\ud980\udc00") >= 0 && text.CompareTo("\ud9bf\udffd") <= 0) || (text.CompareTo("\ud9c0\udc00") >= 0 && text.CompareTo("\ud9ff\udffd") <= 0) || (text.CompareTo("\uda00\udc00") >= 0 && text.CompareTo("\uda3f\udffd") <= 0) || (text.CompareTo("\uda40\udc00") >= 0 && text.CompareTo("\uda7f\udffd") <= 0) || (text.CompareTo("\uda80\udc00") >= 0 && text.CompareTo("\udabf\udffd") <= 0) || (text.CompareTo("\udac0\udc00") >= 0 && text.CompareTo("\udaff\udffd") <= 0) || (text.CompareTo("\udb00\udc00") >= 0 && text.CompareTo("\udb3f\udffd") <= 0) || (text.CompareTo("\udb40\udc00") >= 0 && text.CompareTo("\udb7f\udffd") <= 0) || (isQuery && ((text.CompareTo("\udb80\udc00") >= 0 && text.CompareTo("\udbbf\udffd") <= 0) || (text.CompareTo("\udbc0\udc00") >= 0 && text.CompareTo("\udbff\udffd") <= 0))))
				{
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06001B49 RID: 6985 RVA: 0x00064F84 File Offset: 0x00063F84
		public static bool TryCreate(string uriString, UriKind uriKind, out Uri result)
		{
			if (uriString == null)
			{
				result = null;
				return false;
			}
			UriFormatException ex = null;
			result = Uri.CreateHelper(uriString, false, uriKind, ref ex);
			return ex == null && result != null;
		}

		// Token: 0x06001B4A RID: 6986 RVA: 0x00064FB4 File Offset: 0x00063FB4
		public static bool TryCreate(Uri baseUri, string relativeUri, out Uri result)
		{
			Uri uri;
			if (!Uri.TryCreate(relativeUri, UriKind.RelativeOrAbsolute, out uri))
			{
				result = null;
				return false;
			}
			if (!uri.IsAbsoluteUri)
			{
				return Uri.TryCreate(baseUri, uri, out result);
			}
			result = uri;
			return true;
		}

		// Token: 0x06001B4B RID: 6987 RVA: 0x00064FE8 File Offset: 0x00063FE8
		public static bool TryCreate(Uri baseUri, Uri relativeUri, out Uri result)
		{
			result = null;
			if (baseUri == null)
			{
				return false;
			}
			if (baseUri.IsNotAbsoluteUri)
			{
				return false;
			}
			string text = null;
			bool flag;
			UriFormatException ex;
			if (baseUri.Syntax.IsSimple)
			{
				flag = relativeUri.UserEscaped;
				result = Uri.ResolveHelper(baseUri, relativeUri, ref text, ref flag, out ex);
			}
			else
			{
				flag = false;
				text = baseUri.Syntax.InternalResolve(baseUri, relativeUri, out ex);
			}
			if (ex != null)
			{
				return false;
			}
			if (result == null)
			{
				result = Uri.CreateHelper(text, flag, UriKind.Absolute, ref ex);
			}
			return ex == null && result != null && result.IsAbsoluteUri;
		}

		// Token: 0x06001B4C RID: 6988 RVA: 0x0006506B File Offset: 0x0006406B
		public bool IsBaseOf(Uri uri)
		{
			if (!this.IsAbsoluteUri)
			{
				return false;
			}
			if (this.Syntax.IsSimple)
			{
				return this.IsBaseOfHelper(uri);
			}
			return this.Syntax.InternalIsBaseOf(this, uri);
		}

		// Token: 0x06001B4D RID: 6989 RVA: 0x0006509C File Offset: 0x0006409C
		public string GetComponents(UriComponents components, UriFormat format)
		{
			if ((components & UriComponents.SerializationInfoString) != (UriComponents)0 && components != UriComponents.SerializationInfoString)
			{
				throw new ArgumentOutOfRangeException("UriComponents.SerializationInfoString");
			}
			if ((format & (UriFormat)(-4)) != (UriFormat)0)
			{
				throw new ArgumentOutOfRangeException("format");
			}
			if (this.IsNotAbsoluteUri)
			{
				if (components == UriComponents.SerializationInfoString)
				{
					return this.GetRelativeSerializationString(format);
				}
				throw new InvalidOperationException(SR.GetString("net_uri_NotAbsolute"));
			}
			else
			{
				if (this.Syntax.IsSimple)
				{
					return this.GetComponentsHelper(components, format);
				}
				return this.Syntax.InternalGetComponents(this, components, format);
			}
		}

		// Token: 0x06001B4E RID: 6990 RVA: 0x00065122 File Offset: 0x00064122
		public bool IsWellFormedOriginalString()
		{
			if (this.IsNotAbsoluteUri || this.Syntax.IsSimple)
			{
				return this.InternalIsWellFormedOriginalString();
			}
			return this.Syntax.InternalIsWellFormedOriginalString(this);
		}

		// Token: 0x06001B4F RID: 6991 RVA: 0x0006514C File Offset: 0x0006414C
		public static bool IsWellFormedUriString(string uriString, UriKind uriKind)
		{
			Uri uri;
			return Uri.TryCreate(uriString, uriKind, out uri) && uri.IsWellFormedOriginalString();
		}

		// Token: 0x06001B50 RID: 6992 RVA: 0x0006516C File Offset: 0x0006416C
		public static int Compare(Uri uri1, Uri uri2, UriComponents partsToCompare, UriFormat compareFormat, StringComparison comparisonType)
		{
			if (uri1 == null)
			{
				if (uri2 == null)
				{
					return 0;
				}
				return -1;
			}
			else
			{
				if (uri2 == null)
				{
					return 1;
				}
				if (uri1.IsAbsoluteUri && uri2.IsAbsoluteUri)
				{
					return string.Compare(uri1.GetParts(partsToCompare, compareFormat), uri2.GetParts(partsToCompare, compareFormat), comparisonType);
				}
				if (uri1.IsAbsoluteUri)
				{
					return 1;
				}
				if (!uri2.IsAbsoluteUri)
				{
					return string.Compare(uri1.OriginalString, uri2.OriginalString, comparisonType);
				}
				return -1;
			}
		}

		// Token: 0x06001B51 RID: 6993 RVA: 0x000651E0 File Offset: 0x000641E0
		public unsafe static string UnescapeDataString(string stringToUnescape)
		{
			if (stringToUnescape == null)
			{
				throw new ArgumentNullException("stringToUnescape");
			}
			if (stringToUnescape.Length == 0)
			{
				return string.Empty;
			}
			IntPtr intPtr2;
			IntPtr intPtr = (intPtr2 = stringToUnescape);
			if (intPtr != 0)
			{
				intPtr2 = (IntPtr)((int)intPtr + RuntimeHelpers.OffsetToStringData);
			}
			char* ptr = intPtr2;
			int num = 0;
			while (num < stringToUnescape.Length && ptr[num] != '%')
			{
				num++;
			}
			string text;
			if (num == stringToUnescape.Length)
			{
				text = stringToUnescape;
			}
			else
			{
				num = 0;
				char[] array = new char[stringToUnescape.Length];
				array = Uri.UnescapeString(stringToUnescape, 0, stringToUnescape.Length, array, ref num, char.MaxValue, char.MaxValue, char.MaxValue, (Uri.UnescapeMode)26, null, false, true);
				text = new string(array, 0, num);
			}
			return text;
		}

		// Token: 0x06001B52 RID: 6994 RVA: 0x00065284 File Offset: 0x00064284
		public static string EscapeUriString(string stringToEscape)
		{
			if (stringToEscape == null)
			{
				throw new ArgumentNullException("stringToUnescape");
			}
			if (stringToEscape.Length == 0)
			{
				return string.Empty;
			}
			int num = 0;
			char[] array = Uri.EscapeString(stringToEscape, 0, stringToEscape.Length, null, ref num, true, char.MaxValue, char.MaxValue, char.MaxValue);
			if (array == null)
			{
				return stringToEscape;
			}
			return new string(array, 0, num);
		}

		// Token: 0x06001B53 RID: 6995 RVA: 0x000652E0 File Offset: 0x000642E0
		public static string EscapeDataString(string stringToEscape)
		{
			if (stringToEscape == null)
			{
				throw new ArgumentNullException("stringToUnescape");
			}
			if (stringToEscape.Length == 0)
			{
				return string.Empty;
			}
			int num = 0;
			char[] array = Uri.EscapeString(stringToEscape, 0, stringToEscape.Length, null, ref num, false, char.MaxValue, char.MaxValue, char.MaxValue);
			if (array == null)
			{
				return stringToEscape;
			}
			return new string(array, 0, num);
		}

		// Token: 0x06001B54 RID: 6996 RVA: 0x0006533C File Offset: 0x0006433C
		private unsafe static char[] EscapeString(string input, int start, int end, char[] dest, ref int destPos, bool isUriString, char force1, char force2, char rsvd)
		{
			if (end - start >= 65520)
			{
				throw Uri.GetException(Uri.ParsingError.SizeLimit);
			}
			int i = start;
			int num = start;
			byte* ptr = stackalloc byte[1 * 160];
			fixed (char* ptr2 = input)
			{
				while (i < end)
				{
					char c = ptr2[i];
					if (c > '\u007f')
					{
						short num2 = (short)Math.Min(end - i, 39);
						short num3 = 1;
						while (num3 < num2 && ptr2[i + (int)num3] > '\u007f')
						{
							num3 += 1;
						}
						if (ptr2[i + (int)num3 - 1] >= '\ud800' && ptr2[i + (int)num3 - 1] <= '\udbff')
						{
							if (num3 == 1 || (int)num3 == end - i)
							{
								throw new UriFormatException(SR.GetString("net_uri_BadString"));
							}
							num3 += 1;
						}
						dest = Uri.EnsureDestinationSize(ptr2, dest, i, num3 * 4 * 3, 480, ref destPos, num);
						short num4 = (short)Encoding.UTF8.GetBytes(ptr2 + i, (int)num3, ptr, 160);
						if (num4 == 0)
						{
							throw new UriFormatException(SR.GetString("net_uri_BadString"));
						}
						i += (int)(num3 - 1);
						for (num3 = 0; num3 < num4; num3 += 1)
						{
							Uri.EscapeAsciiChar((char)ptr[num3], dest, ref destPos);
						}
						num = i + 1;
					}
					else if (c == '%' && rsvd == '%')
					{
						dest = Uri.EnsureDestinationSize(ptr2, dest, i, 3, 120, ref destPos, num);
						if (i + 2 < end && Uri.EscapedAscii(ptr2[i + 1], ptr2[i + 2]) != '\uffff')
						{
							dest[destPos++] = '%';
							dest[destPos++] = ptr2[i + 1];
							dest[destPos++] = ptr2[i + 2];
							i += 2;
						}
						else
						{
							Uri.EscapeAsciiChar('%', dest, ref destPos);
						}
						num = i + 1;
					}
					else if (c == force1 || c == force2)
					{
						dest = Uri.EnsureDestinationSize(ptr2, dest, i, 3, 120, ref destPos, num);
						Uri.EscapeAsciiChar(c, dest, ref destPos);
						num = i + 1;
					}
					else if (c != rsvd && (isUriString ? Uri.IsNotReservedNotUnreservedNotHash(c) : Uri.IsNotUnreserved(c)))
					{
						dest = Uri.EnsureDestinationSize(ptr2, dest, i, 3, 120, ref destPos, num);
						Uri.EscapeAsciiChar(c, dest, ref destPos);
						num = i + 1;
					}
					i++;
				}
				if (num != i && (num != start || dest != null))
				{
					dest = Uri.EnsureDestinationSize(ptr2, dest, i, 0, 0, ref destPos, num);
				}
			}
			return dest;
		}

		// Token: 0x06001B55 RID: 6997 RVA: 0x000655A8 File Offset: 0x000645A8
		private unsafe static char[] EnsureDestinationSize(char* pStr, char[] dest, int currentInputPos, short charsToAdd, short minReallocateChars, ref int destPos, int prevInputPos)
		{
			if (dest == null || dest.Length < destPos + (currentInputPos - prevInputPos) + (int)charsToAdd)
			{
				char[] array = new char[destPos + (currentInputPos - prevInputPos) + (int)minReallocateChars];
				if (dest != null && destPos != 0)
				{
					Buffer.BlockCopy(dest, 0, array, 0, destPos << 1);
				}
				dest = array;
			}
			while (prevInputPos != currentInputPos)
			{
				dest[destPos++] = pStr[prevInputPos++];
			}
			return dest;
		}

		// Token: 0x06001B56 RID: 6998 RVA: 0x00065614 File Offset: 0x00064614
		private static bool IsNotReservedNotUnreservedNotHash(char c)
		{
			return (c > 'z' && c != '~') || (c > 'Z' && c < 'a' && c != '_') || c < '!' || (c == '>' || c == '<' || c == '%' || c == '"' || c == '`');
		}

		// Token: 0x06001B57 RID: 6999 RVA: 0x00065664 File Offset: 0x00064664
		private static bool IsNotUnreserved(char c)
		{
			return (c > 'z' && c != '~') || ((c > '9' && c < 'A') || (c > 'Z' && c < 'a' && c != '_')) || (c < '\'' && c != '!') || (c == '+' || c == ',' || c == '/');
		}

		// Token: 0x06001B58 RID: 7000 RVA: 0x000656B8 File Offset: 0x000646B8
		private unsafe static char[] UnescapeString(string input, int start, int end, char[] dest, ref int destPosition, char rsvd1, char rsvd2, char rsvd3, Uri.UnescapeMode unescapeMode, UriParser syntax, bool isQuery, bool readOnlyConfig)
		{
			IntPtr intPtr2;
			IntPtr intPtr = (intPtr2 = input);
			if (intPtr != 0)
			{
				intPtr2 = (IntPtr)((int)intPtr + RuntimeHelpers.OffsetToStringData);
			}
			char* ptr = intPtr2;
			return Uri.UnescapeString(ptr, start, end, dest, ref destPosition, rsvd1, rsvd2, rsvd3, unescapeMode, syntax, isQuery, readOnlyConfig);
		}

		// Token: 0x06001B59 RID: 7001 RVA: 0x000656F0 File Offset: 0x000646F0
		private unsafe static char[] UnescapeString(char* pStr, int start, int end, char[] dest, ref int destPosition, char rsvd1, char rsvd2, char rsvd3, Uri.UnescapeMode unescapeMode, UriParser syntax, bool isQuery, bool readOnlyConfig)
		{
			byte[] array = null;
			byte b = 0;
			bool flag = false;
			int i = start;
			bool flag2 = Uri.s_IriParsing && (readOnlyConfig || (!readOnlyConfig && Uri.IriParsingStatic(syntax))) && (unescapeMode & Uri.UnescapeMode.EscapeUnescape) == Uri.UnescapeMode.EscapeUnescape;
			for (;;)
			{
				try
				{
					fixed (char* ptr = dest)
					{
						if ((unescapeMode & Uri.UnescapeMode.EscapeUnescape) == Uri.UnescapeMode.CopyOnly)
						{
							while (start < end)
							{
								ptr[(IntPtr)(destPosition++) * 2] = pStr[start++];
							}
							return dest;
						}
						for (;;)
						{
							IL_007D:
							char c = '\0';
							while (i < end)
							{
								if ((c = pStr[i]) == '%')
								{
									if ((unescapeMode & Uri.UnescapeMode.Unescape) == Uri.UnescapeMode.CopyOnly)
									{
										flag = true;
									}
									else if (i + 2 < end)
									{
										c = Uri.EscapedAscii(pStr[i + 1], pStr[i + 2]);
										if (unescapeMode >= Uri.UnescapeMode.UnescapeAll)
										{
											if (c == '\uffff')
											{
												if (unescapeMode >= Uri.UnescapeMode.UnescapeAllOrThrow)
												{
													goto Block_14;
												}
												goto IL_01E6;
											}
										}
										else if (c == '\uffff')
										{
											if ((unescapeMode & Uri.UnescapeMode.Escape) == Uri.UnescapeMode.CopyOnly)
											{
												goto IL_01E6;
											}
											flag = true;
										}
										else
										{
											if (c == '%')
											{
												i += 2;
												goto IL_01E6;
											}
											if (c == rsvd1 || c == rsvd2 || c == rsvd3)
											{
												i += 2;
												goto IL_01E6;
											}
											if ((unescapeMode & Uri.UnescapeMode.V1ToStringFlag) == Uri.UnescapeMode.CopyOnly && Uri.IsNotSafeForUnescape(c))
											{
												i += 2;
												goto IL_01E6;
											}
											if (flag2 && ((c <= '\u009f' && Uri.IsNotSafeForUnescape(c)) || (c > '\u009f' && !Uri.CheckIriUnicodeRange(c, isQuery))))
											{
												i += 2;
												goto IL_01E6;
											}
										}
									}
									else if (unescapeMode >= Uri.UnescapeMode.UnescapeAll)
									{
										if (unescapeMode >= Uri.UnescapeMode.UnescapeAllOrThrow)
										{
											goto Block_26;
										}
										goto IL_01E6;
									}
									else
									{
										flag = true;
									}
								}
								else
								{
									if ((unescapeMode & (Uri.UnescapeMode.Unescape | Uri.UnescapeMode.UnescapeAll)) == (Uri.UnescapeMode.Unescape | Uri.UnescapeMode.UnescapeAll) || (unescapeMode & Uri.UnescapeMode.Escape) == Uri.UnescapeMode.CopyOnly)
									{
										goto IL_01E6;
									}
									if (c == rsvd1 || c == rsvd2 || c == rsvd3)
									{
										flag = true;
									}
									else
									{
										if ((unescapeMode & Uri.UnescapeMode.V1ToStringFlag) != Uri.UnescapeMode.CopyOnly || (c > '\u001f' && (c < '\u007f' || c > '\u009f')))
										{
											goto IL_01E6;
										}
										flag = true;
									}
								}
								IL_0213:
								while (start < i)
								{
									ptr[(IntPtr)(destPosition++) * 2] = pStr[start++];
								}
								if (i != end)
								{
									if (flag)
									{
										if (b == 0)
										{
											goto Block_38;
										}
										b -= 1;
										Uri.EscapeAsciiChar(pStr[i], dest, ref destPosition);
										flag = false;
										i = (start = i + 1);
										goto IL_007D;
									}
									else
									{
										if (c <= '\u007f')
										{
											dest[destPosition++] = c;
											i += 3;
											start = i;
											goto IL_007D;
										}
										int num = 1;
										if (array == null)
										{
											array = new byte[end - i];
										}
										array[0] = (byte)c;
										i += 3;
										while (i < end && pStr[i] == '%' && i + 2 < end)
										{
											c = Uri.EscapedAscii(pStr[i + 1], pStr[i + 2]);
											if (c == '\uffff' || c < '\u0080')
											{
												break;
											}
											array[num++] = (byte)c;
											i += 3;
										}
										Encoding encoding = Encoding.GetEncoding("utf-8", new EncoderReplacementFallback(""), new DecoderReplacementFallback(""));
										char[] array2 = new char[array.Length];
										int chars = encoding.GetChars(array, 0, num, array2, 0);
										if (chars != 0)
										{
											start = i;
											Uri.MatchUTF8Sequence(ptr, dest, ref destPosition, array2, chars, array, isQuery, flag2);
										}
										else
										{
											if (unescapeMode >= Uri.UnescapeMode.UnescapeAllOrThrow)
											{
												goto Block_48;
											}
											i = start + 3;
											start = i;
											dest[destPosition++] = (char)array[0];
										}
									}
								}
								if (i == end)
								{
									goto Block_49;
								}
								goto IL_007D;
								IL_01E6:
								i++;
							}
							goto IL_0213;
						}
						Block_14:
						throw new UriFormatException(SR.GetString("net_uri_BadString"));
						Block_26:
						throw new UriFormatException(SR.GetString("net_uri_BadString"));
						Block_38:
						b = 30;
						char[] array3 = new char[dest.Length + (int)(b * 3)];
						fixed (char* ptr2 = array3)
						{
							for (int j = 0; j < destPosition; j++)
							{
								ptr2[j] = ptr[j];
							}
						}
						dest = array3;
						continue;
						Block_48:
						throw new UriFormatException(SR.GetString("net_uri_BadString"));
						Block_49:;
					}
				}
				finally
				{
					char* ptr = null;
				}
				break;
			}
			return dest;
		}

		// Token: 0x06001B5A RID: 7002 RVA: 0x00065AF0 File Offset: 0x00064AF0
		private unsafe static void MatchUTF8Sequence(char* pDest, char[] dest, ref int destOffset, char[] unescapedChars, int charCount, byte[] bytes, bool isQuery, bool iriParsing)
		{
			int num = 0;
			fixed (char* ptr = unescapedChars)
			{
				for (int i = 0; i < charCount; i++)
				{
					bool flag = char.IsHighSurrogate(ptr[i]);
					byte[] bytes2 = Encoding.UTF8.GetBytes(unescapedChars, i, flag ? 2 : 1);
					int num2 = bytes2.Length;
					bool flag2 = false;
					if (iriParsing)
					{
						if (!flag)
						{
							flag2 = Uri.CheckIriUnicodeRange(unescapedChars[i], isQuery);
						}
						else
						{
							bool flag3 = false;
							flag2 = Uri.CheckIriUnicodeRange(unescapedChars[i], unescapedChars[i + 1], ref flag3, isQuery);
						}
					}
					for (;;)
					{
						if (bytes[num] == bytes2[0])
						{
							bool flag4 = true;
							int j;
							for (j = 0; j < num2; j++)
							{
								if (bytes[num + j] != bytes2[j])
								{
									flag4 = false;
									break;
								}
							}
							if (flag4)
							{
								break;
							}
							for (int k = 0; k < j; k++)
							{
								Uri.EscapeAsciiChar((char)bytes[num++], dest, ref destOffset);
							}
						}
						else
						{
							Uri.EscapeAsciiChar((char)bytes[num++], dest, ref destOffset);
						}
					}
					num += num2;
					if (iriParsing)
					{
						if (!flag2)
						{
							for (int l = 0; l < bytes2.Length; l++)
							{
								Uri.EscapeAsciiChar((char)bytes2[l], dest, ref destOffset);
							}
						}
						else if (!Uri.IsBidiControlCharacter(ptr[i]))
						{
							pDest[destOffset++] = ptr[i];
							if (flag)
							{
								pDest[destOffset++] = ptr[i + 1];
							}
						}
					}
					else
					{
						pDest[destOffset++] = ptr[i];
						if (flag)
						{
							pDest[destOffset++] = ptr[i + 1];
						}
					}
					if (flag)
					{
						i++;
					}
				}
			}
		}

		// Token: 0x06001B5B RID: 7003 RVA: 0x00065CC0 File Offset: 0x00064CC0
		internal bool CheckIsReserved(char ch)
		{
			char[] array = new char[] { ':', '/', '?', '#', '[', ']', '@' };
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == ch)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001B5C RID: 7004 RVA: 0x00065CF8 File Offset: 0x00064CF8
		internal bool CheckIsReserved(char ch, UriComponents component)
		{
			if (component != UriComponents.Scheme || component != UriComponents.UserInfo || component != UriComponents.Host || component != UriComponents.Port || component != UriComponents.Path || component != UriComponents.Query || component != UriComponents.Fragment)
			{
				return component == (UriComponents)0 && this.CheckIsReserved(ch);
			}
			if (component <= UriComponents.Path)
			{
				switch (component)
				{
				case UriComponents.UserInfo:
					if (ch == '/' || ch == '?' || ch == '#' || ch == '[' || ch == ']' || ch == '@')
					{
						return true;
					}
					break;
				case UriComponents.Scheme | UriComponents.UserInfo:
					break;
				case UriComponents.Host:
					if (ch == ':' || ch == '/' || ch == '?' || ch == '#' || ch == '[' || ch == ']' || ch == '@')
					{
						return true;
					}
					break;
				default:
					if (component == UriComponents.Path)
					{
						if (ch == '/' || ch == '?' || ch == '#' || ch == '[' || ch == ']')
						{
							return true;
						}
					}
					break;
				}
			}
			else if (component != UriComponents.Query)
			{
				if (component == UriComponents.Fragment)
				{
					if (ch == '#' || ch == '[' || ch == ']')
					{
						return true;
					}
				}
			}
			else if (ch == '#' || ch == '[' || ch == ']')
			{
				return true;
			}
			return false;
		}

		// Token: 0x06001B5D RID: 7005 RVA: 0x00065DE8 File Offset: 0x00064DE8
		internal unsafe string EscapeUnescapeIri(string input, int start, int end, UriComponents component)
		{
			IntPtr intPtr2;
			IntPtr intPtr = (intPtr2 = input);
			if (intPtr != 0)
			{
				intPtr2 = (IntPtr)((int)intPtr + RuntimeHelpers.OffsetToStringData);
			}
			char* ptr = intPtr2;
			return this.EscapeUnescapeIri(ptr, start, end, component);
		}

		// Token: 0x06001B5E RID: 7006 RVA: 0x00065E14 File Offset: 0x00064E14
		internal unsafe string EscapeUnescapeIri(char* pInput, int start, int end, UriComponents component)
		{
			char[] array = new char[end - start];
			byte[] array2 = null;
			GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			char* ptr = (char*)(void*)gchandle.AddrOfPinnedObject();
			int num = 0;
			int i = start;
			int num2 = 0;
			bool flag = false;
			while (i < end)
			{
				bool flag2 = false;
				flag = false;
				char c;
				if ((c = pInput[i]) == '%')
				{
					if (i + 2 >= end)
					{
						ptr[(IntPtr)(num2++) * 2] = pInput[i];
						goto IL_02DC;
					}
					c = Uri.EscapedAscii(pInput[i + 1], pInput[i + 2]);
					if (c == '\uffff' || c == '%' || this.CheckIsReserved(c, component) || Uri.IsNotSafeForUnescape(c))
					{
						ptr[(IntPtr)(num2++) * 2] = pInput[i++];
						ptr[(IntPtr)(num2++) * 2] = pInput[i++];
						ptr[(IntPtr)(num2++) * 2] = pInput[i];
					}
					else if (c <= '\u007f')
					{
						ptr[(IntPtr)(num2++) * 2] = c;
						i += 2;
					}
					else
					{
						int num3 = i;
						int num4 = 1;
						if (array2 == null)
						{
							array2 = new byte[end - i];
						}
						array2[0] = (byte)c;
						i += 3;
						while (i < end && pInput[i] == '%' && i + 2 < end)
						{
							c = Uri.EscapedAscii(pInput[i + 1], pInput[i + 2]);
							if (c == '\uffff' || c < '\u0080')
							{
								break;
							}
							array2[num4++] = (byte)c;
							i += 3;
						}
						i--;
						Encoding encoding = Encoding.GetEncoding("utf-8", new EncoderReplacementFallback(""), new DecoderReplacementFallback(""));
						char[] array3 = new char[array2.Length];
						int chars = encoding.GetChars(array2, 0, num4, array3, 0);
						if (chars != 0)
						{
							Uri.MatchUTF8Sequence(ptr, array, ref num2, array3, chars, array2, component == UriComponents.Query, true);
							goto IL_02DC;
						}
						for (int j = num3; j <= i; j++)
						{
							ptr[(IntPtr)(num2++) * 2] = pInput[j];
						}
						goto IL_02DC;
					}
				}
				else
				{
					if (c <= '\u007f')
					{
						ptr[(IntPtr)(num2++) * 2] = pInput[i];
						goto IL_02DC;
					}
					if (char.IsHighSurrogate(c) && i + 1 < end)
					{
						char c2 = pInput[i + 1];
						flag2 = !Uri.CheckIriUnicodeRange(c, c2, ref flag, component == UriComponents.Query);
						if (!flag2)
						{
							ptr[(IntPtr)(num2++) * 2] = pInput[i++];
							ptr[(IntPtr)(num2++) * 2] = pInput[i];
							goto IL_02DC;
						}
						goto IL_02DC;
					}
					else
					{
						if (!Uri.CheckIriUnicodeRange(c, component == UriComponents.Query))
						{
							flag2 = true;
							goto IL_02DC;
						}
						if (!Uri.IsBidiControlCharacter(c))
						{
							ptr[(IntPtr)(num2++) * 2] = pInput[i];
							goto IL_02DC;
						}
						goto IL_02DC;
					}
				}
				IL_03E2:
				i++;
				continue;
				IL_02DC:
				if (flag2)
				{
					if (num < 12)
					{
						char[] array4;
						checked
						{
							int num5 = array.Length + 90;
							num += 90;
							array4 = new char[num5];
						}
						fixed (char* ptr2 = array4)
						{
							for (int k = 0; k < num2; k++)
							{
								ptr2[k] = ptr[k];
							}
						}
						if (gchandle.IsAllocated)
						{
							gchandle.Free();
						}
						array = array4;
						gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
						ptr = (char*)(void*)gchandle.AddrOfPinnedObject();
					}
					byte[] array5 = new byte[4];
					fixed (byte* ptr3 = array5)
					{
						int bytes = Encoding.UTF8.GetBytes(pInput + i, flag ? 2 : 1, ptr3, 4);
						num -= bytes * 3;
						for (int l = 0; l < bytes; l++)
						{
							Uri.EscapeAsciiChar((char)array5[l], array, ref num2);
						}
					}
					goto IL_03E2;
				}
				goto IL_03E2;
			}
			if (gchandle.IsAllocated)
			{
				gchandle.Free();
			}
			return new string(array, 0, num2);
		}

		// Token: 0x06001B5F RID: 7007 RVA: 0x0006622C File Offset: 0x0006522C
		private static bool IsNotSafeForUnescape(char ch)
		{
			return ch <= '\u001f' || (ch >= '\u007f' && ch <= '\u009f') || ((ch >= ';' && ch <= '@' && (ch | '\u0002') != '>') || (ch >= '#' && ch <= '&') || ch == '+' || ch == ',' || ch == '/' || ch == '\\');
		}

		// Token: 0x06001B60 RID: 7008 RVA: 0x00066280 File Offset: 0x00065280
		internal unsafe bool InternalIsWellFormedOriginalString()
		{
			if (this.UserDrivenParsing)
			{
				throw new InvalidOperationException(SR.GetString("net_uri_UserDrivenParsing", new object[] { base.GetType().FullName }));
			}
			fixed (char* @string = this.m_String)
			{
				ushort num = 0;
				bool flag;
				if (!this.IsAbsoluteUri)
				{
					flag = (this.CheckCanonical(@string, ref num, (ushort)this.m_String.Length, '\ufffe') & (Uri.Check.EscapedCanonical | Uri.Check.BackslashInPath)) == Uri.Check.EscapedCanonical;
				}
				else if (this.IsImplicitFile)
				{
					flag = false;
				}
				else
				{
					this.EnsureParseRemaining();
					Uri.Flags flags = this.m_Flags & (Uri.Flags.E_UserNotCanonical | Uri.Flags.E_HostNotCanonical | Uri.Flags.E_PortNotCanonical | Uri.Flags.E_PathNotCanonical | Uri.Flags.E_QueryNotCanonical | Uri.Flags.E_FragmentNotCanonical | Uri.Flags.UserIriCanonical | Uri.Flags.PathIriCanonical | Uri.Flags.QueryIriCanonical | Uri.Flags.FragmentIriCanonical);
					if ((flags & Uri.Flags.E_CannotDisplayCanonical & (Uri.Flags.E_UserNotCanonical | Uri.Flags.E_PathNotCanonical | Uri.Flags.E_QueryNotCanonical | Uri.Flags.E_FragmentNotCanonical)) != Uri.Flags.Zero && (!this.m_iriParsing || (this.m_iriParsing && ((flags & Uri.Flags.E_UserNotCanonical) == Uri.Flags.Zero || (flags & Uri.Flags.UserIriCanonical) == Uri.Flags.Zero) && ((flags & Uri.Flags.E_PathNotCanonical) == Uri.Flags.Zero || (flags & Uri.Flags.PathIriCanonical) == Uri.Flags.Zero) && ((flags & Uri.Flags.E_QueryNotCanonical) == Uri.Flags.Zero || (flags & Uri.Flags.QueryIriCanonical) == Uri.Flags.Zero) && ((flags & Uri.Flags.E_FragmentNotCanonical) == Uri.Flags.Zero || (flags & Uri.Flags.FragmentIriCanonical) == Uri.Flags.Zero))))
					{
						flag = false;
					}
					else
					{
						if (this.InFact(Uri.Flags.AuthorityFound))
						{
							num = (ushort)((int)this.m_Info.Offset.Scheme + this.m_Syntax.SchemeName.Length + 2);
							if (num >= this.m_Info.Offset.User || this.m_String[(int)(num - 1)] == '\\' || this.m_String[(int)num] == '\\')
							{
								return false;
							}
							if (this.InFact(Uri.Flags.DosPath | Uri.Flags.UncPath) && (num += 1) < this.m_Info.Offset.User && (this.m_String[(int)num] == '/' || this.m_String[(int)num] == '\\'))
							{
								return false;
							}
						}
						if (this.InFact(Uri.Flags.FirstSlashAbsent) && this.m_Info.Offset.Query > this.m_Info.Offset.Path)
						{
							flag = false;
						}
						else if (this.InFact(Uri.Flags.BackslashInPath))
						{
							flag = false;
						}
						else
						{
							if (!this.IsDosPath || this.m_String[(int)(this.m_Info.Offset.Path + this.SecuredPathIndex - 1)] != '|')
							{
								if ((this.m_Flags & Uri.Flags.CanonicalDnsHost) == Uri.Flags.Zero)
								{
									num = this.m_Info.Offset.User;
									if (!this.m_iriParsing || this.HostType != Uri.Flags.IPv6HostType)
									{
										Uri.Check check = this.CheckCanonical(@string, ref num, this.m_Info.Offset.Path, '/');
										if ((check & (Uri.Check.EscapedCanonical | Uri.Check.BackslashInPath | Uri.Check.ReservedFound)) != Uri.Check.EscapedCanonical && (!this.m_iriParsing || (this.m_iriParsing && (check & (Uri.Check.DisplayCanonical | Uri.Check.NotIriCanonical | Uri.Check.FoundNonAscii)) != (Uri.Check.DisplayCanonical | Uri.Check.FoundNonAscii))))
										{
											return false;
										}
									}
								}
								if ((this.m_Flags & (Uri.Flags.SchemeNotCanonical | Uri.Flags.AuthorityFound)) == (Uri.Flags.SchemeNotCanonical | Uri.Flags.AuthorityFound))
								{
									num = (ushort)this.m_Syntax.SchemeName.Length;
									IntPtr intPtr;
									ushort num2;
									do
									{
										intPtr = @string;
										num2 = num;
										num = num2 + 1;
									}
									while (*(intPtr + (IntPtr)num2 * 2) != 58);
									if ((int)(num + 1) >= this.m_String.Length || @string[num] != '/' || @string[num + 1] != '/')
									{
										return false;
									}
								}
								string text = null;
								return true;
							}
							flag = false;
						}
					}
				}
				return flag;
			}
		}

		// Token: 0x06001B61 RID: 7009 RVA: 0x000665F1 File Offset: 0x000655F1
		private Uri(Uri.Flags flags, UriParser uriParser, string uri)
		{
			this.m_Flags = flags;
			this.m_Syntax = uriParser;
			this.m_String = uri;
		}

		// Token: 0x06001B62 RID: 7010 RVA: 0x00066610 File Offset: 0x00065610
		internal static Uri CreateHelper(string uriString, bool dontEscape, UriKind uriKind, ref UriFormatException e)
		{
			if (uriKind < UriKind.RelativeOrAbsolute || uriKind > UriKind.Relative)
			{
				throw new ArgumentException(SR.GetString("net_uri_InvalidUriKind", new object[] { uriKind }));
			}
			UriParser uriParser = null;
			Uri.Flags flags = Uri.Flags.Zero;
			Uri.ParsingError parsingError = Uri.ParseScheme(uriString, ref flags, ref uriParser);
			if (dontEscape)
			{
				flags |= Uri.Flags.UserEscaped;
			}
			if (parsingError == Uri.ParsingError.None)
			{
				Uri uri = new Uri(flags, uriParser, uriString);
				Uri uri2;
				try
				{
					uri.InitializeUri(parsingError, uriKind, out e);
					if (e == null)
					{
						uri2 = uri;
					}
					else
					{
						uri2 = null;
					}
				}
				catch (UriFormatException ex)
				{
					e = ex;
					uri2 = null;
				}
				return uri2;
			}
			if (uriKind != UriKind.Absolute && parsingError <= Uri.ParsingError.EmptyUriString)
			{
				return new Uri(flags & Uri.Flags.UserEscaped, null, uriString);
			}
			return null;
		}

		// Token: 0x06001B63 RID: 7011 RVA: 0x000666C0 File Offset: 0x000656C0
		internal static Uri ResolveHelper(Uri baseUri, Uri relativeUri, ref string newUriString, ref bool userEscaped, out UriFormatException e)
		{
			e = null;
			string text = string.Empty;
			if (relativeUri != null)
			{
				if (relativeUri.IsAbsoluteUri)
				{
					return relativeUri;
				}
				text = relativeUri.OriginalString;
				userEscaped = relativeUri.UserEscaped;
			}
			else
			{
				text = string.Empty;
			}
			if (text.Length > 0 && (Uri.IsLWS(text[0]) || Uri.IsLWS(text[text.Length - 1])))
			{
				text = text.Trim(Uri._WSchars);
			}
			if (text.Length == 0)
			{
				newUriString = baseUri.GetParts(UriComponents.AbsoluteUri, baseUri.UserEscaped ? UriFormat.UriEscaped : UriFormat.SafeUnescaped);
				return null;
			}
			if (text[0] == '#' && !baseUri.IsImplicitFile && baseUri.Syntax.InFact(UriSyntaxFlags.MayHaveFragment))
			{
				newUriString = baseUri.GetParts(UriComponents.Scheme | UriComponents.UserInfo | UriComponents.Host | UriComponents.Port | UriComponents.Path | UriComponents.Query, UriFormat.UriEscaped) + text;
				return null;
			}
			if (text.Length >= 3 && (text[1] == ':' || text[1] == '|') && Uri.IsAsciiLetter(text[0]) && (text[2] == '\\' || text[2] == '/'))
			{
				if (baseUri.IsImplicitFile)
				{
					newUriString = text;
					return null;
				}
				if (baseUri.Syntax.InFact(UriSyntaxFlags.AllowDOSPath))
				{
					string text2;
					if (baseUri.InFact(Uri.Flags.AuthorityFound))
					{
						text2 = (baseUri.Syntax.InFact(UriSyntaxFlags.PathIsRooted) ? ":///" : "://");
					}
					else
					{
						text2 = (baseUri.Syntax.InFact(UriSyntaxFlags.PathIsRooted) ? ":/" : ":");
					}
					newUriString = baseUri.Scheme + text2 + text;
					return null;
				}
			}
			Uri.ParsingError combinedString = Uri.GetCombinedString(baseUri, text, userEscaped, ref newUriString);
			if (combinedString != Uri.ParsingError.None)
			{
				e = Uri.GetException(combinedString);
				return null;
			}
			if (newUriString == baseUri.m_String)
			{
				return baseUri;
			}
			return null;
		}

		// Token: 0x06001B64 RID: 7012 RVA: 0x00066878 File Offset: 0x00065878
		private string GetRelativeSerializationString(UriFormat format)
		{
			if (format == UriFormat.UriEscaped)
			{
				if (this.m_String.Length == 0)
				{
					return string.Empty;
				}
				int num = 0;
				char[] array = Uri.EscapeString(this.m_String, 0, this.m_String.Length, null, ref num, true, char.MaxValue, char.MaxValue, '%');
				if (array == null)
				{
					return this.m_String;
				}
				return new string(array, 0, num);
			}
			else
			{
				if (format == UriFormat.Unescaped)
				{
					return Uri.UnescapeDataString(this.m_String);
				}
				if (format != UriFormat.SafeUnescaped)
				{
					throw new ArgumentOutOfRangeException("format");
				}
				if (this.m_String.Length == 0)
				{
					return string.Empty;
				}
				char[] array2 = new char[this.m_String.Length];
				int num2 = 0;
				array2 = Uri.UnescapeString(this.m_String, 0, this.m_String.Length, array2, ref num2, char.MaxValue, char.MaxValue, char.MaxValue, Uri.UnescapeMode.EscapeUnescape, null, false, true);
				return new string(array2, 0, num2);
			}
		}

		// Token: 0x06001B65 RID: 7013 RVA: 0x00066954 File Offset: 0x00065954
		internal string GetComponentsHelper(UriComponents uriComponents, UriFormat uriFormat)
		{
			if (uriComponents == UriComponents.Scheme)
			{
				return this.m_Syntax.SchemeName;
			}
			if ((uriComponents & UriComponents.SerializationInfoString) != (UriComponents)0)
			{
				uriComponents |= UriComponents.AbsoluteUri;
			}
			this.EnsureParseRemaining();
			if ((uriComponents & UriComponents.Host) != (UriComponents)0)
			{
				this.EnsureHostString(true);
			}
			if (uriComponents == UriComponents.Port || uriComponents == UriComponents.StrongPort)
			{
				if ((this.m_Flags & Uri.Flags.NotDefaultPort) != Uri.Flags.Zero || (uriComponents == UriComponents.StrongPort && this.m_Syntax.DefaultPort != -1))
				{
					return this.m_Info.Offset.PortValue.ToString(CultureInfo.InvariantCulture);
				}
				return string.Empty;
			}
			else
			{
				if ((uriComponents & UriComponents.StrongPort) != (UriComponents)0)
				{
					uriComponents |= UriComponents.Port;
				}
				if (uriComponents == UriComponents.Host && (uriFormat == UriFormat.UriEscaped || (this.m_Flags & (Uri.Flags.HostNotCanonical | Uri.Flags.E_HostNotCanonical)) == Uri.Flags.Zero))
				{
					this.EnsureHostString(false);
					return this.m_Info.Host;
				}
				switch (uriFormat)
				{
				case UriFormat.UriEscaped:
					return this.GetEscapedParts(uriComponents);
				case UriFormat.Unescaped:
				case UriFormat.SafeUnescaped:
					break;
				default:
					if (uriFormat != (UriFormat)32767)
					{
						throw new ArgumentOutOfRangeException("uriFormat");
					}
					break;
				}
				return this.GetUnescapedParts(uriComponents, uriFormat);
			}
		}

		// Token: 0x06001B66 RID: 7014 RVA: 0x00066A5C File Offset: 0x00065A5C
		internal unsafe bool IsBaseOfHelper(Uri uriLink)
		{
			if (!this.IsAbsoluteUri || this.UserDrivenParsing)
			{
				return false;
			}
			if (!uriLink.IsAbsoluteUri)
			{
				string text = null;
				bool flag = false;
				UriFormatException ex;
				uriLink = Uri.ResolveHelper(this, uriLink, ref text, ref flag, out ex);
				if (ex != null)
				{
					return false;
				}
				if (uriLink == null)
				{
					uriLink = Uri.CreateHelper(text, flag, UriKind.Absolute, ref ex);
				}
				if (ex != null)
				{
					return false;
				}
			}
			if (this.Syntax.SchemeName != uriLink.Syntax.SchemeName)
			{
				return false;
			}
			string parts = this.GetParts(UriComponents.Scheme | UriComponents.UserInfo | UriComponents.Host | UriComponents.Port | UriComponents.Path | UriComponents.Query, UriFormat.SafeUnescaped);
			string parts2 = uriLink.GetParts(UriComponents.Scheme | UriComponents.UserInfo | UriComponents.Host | UriComponents.Port | UriComponents.Path | UriComponents.Query, UriFormat.SafeUnescaped);
			fixed (char* ptr = parts)
			{
				fixed (char* ptr2 = parts2)
				{
					return Uri.TestForSubPath(ptr, (ushort)parts.Length, ptr2, (ushort)parts2.Length, this.IsUncOrDosPath || uriLink.IsUncOrDosPath);
				}
			}
		}

		// Token: 0x06001B67 RID: 7015 RVA: 0x00066B38 File Offset: 0x00065B38
		private void CreateThisFromUri(Uri otherUri)
		{
			this.m_Info = null;
			this.m_Flags = otherUri.m_Flags;
			if (this.InFact(Uri.Flags.MinimalUriInfoSet))
			{
				this.m_Flags &= ~(Uri.Flags.SchemeNotCanonical | Uri.Flags.UserNotCanonical | Uri.Flags.HostNotCanonical | Uri.Flags.PortNotCanonical | Uri.Flags.PathNotCanonical | Uri.Flags.QueryNotCanonical | Uri.Flags.FragmentNotCanonical | Uri.Flags.E_UserNotCanonical | Uri.Flags.E_HostNotCanonical | Uri.Flags.E_PortNotCanonical | Uri.Flags.E_PathNotCanonical | Uri.Flags.E_QueryNotCanonical | Uri.Flags.E_FragmentNotCanonical | Uri.Flags.ShouldBeCompressed | Uri.Flags.FirstSlashAbsent | Uri.Flags.BackslashInPath | Uri.Flags.MinimalUriInfoSet | Uri.Flags.AllUriInfoSet);
				this.m_Flags |= (Uri.Flags)otherUri.m_Info.Offset.Path;
			}
			this.m_Syntax = otherUri.m_Syntax;
			this.m_String = otherUri.m_String;
			this.m_iriParsing = otherUri.m_iriParsing;
			if (otherUri.OriginalStringSwitched)
			{
				this.m_originalUnicodeString = otherUri.m_originalUnicodeString;
			}
			if (otherUri.AllowIdn && (otherUri.InFact(Uri.Flags.IdnHost) || otherUri.InFact(Uri.Flags.UnicodeHost)))
			{
				this.m_DnsSafeHost = otherUri.m_DnsSafeHost;
			}
		}

		// Token: 0x04001B78 RID: 7032
		private const int c_Max16BitUtf8SequenceLength = 12;

		// Token: 0x04001B79 RID: 7033
		private const int c_MaxUriBufferSize = 65520;

		// Token: 0x04001B7A RID: 7034
		private const int c_MaxUriSchemeName = 1024;

		// Token: 0x04001B7B RID: 7035
		private const UriFormat V1ToStringUnescape = (UriFormat)32767;

		// Token: 0x04001B7C RID: 7036
		private const char c_DummyChar = '\uffff';

		// Token: 0x04001B7D RID: 7037
		private const char c_EOL = '\ufffe';

		// Token: 0x04001B7E RID: 7038
		private const short c_MaxAsciiCharsReallocate = 40;

		// Token: 0x04001B7F RID: 7039
		private const short c_MaxUnicodeCharsReallocate = 40;

		// Token: 0x04001B80 RID: 7040
		private const short c_MaxUTF_8BytesPerUnicodeChar = 4;

		// Token: 0x04001B81 RID: 7041
		private const short c_EncodedCharsPerByte = 3;

		// Token: 0x04001B82 RID: 7042
		public static readonly string UriSchemeFile = UriParser.FileUri.SchemeName;

		// Token: 0x04001B83 RID: 7043
		public static readonly string UriSchemeFtp = UriParser.FtpUri.SchemeName;

		// Token: 0x04001B84 RID: 7044
		public static readonly string UriSchemeGopher = UriParser.GopherUri.SchemeName;

		// Token: 0x04001B85 RID: 7045
		public static readonly string UriSchemeHttp = UriParser.HttpUri.SchemeName;

		// Token: 0x04001B86 RID: 7046
		public static readonly string UriSchemeHttps = UriParser.HttpsUri.SchemeName;

		// Token: 0x04001B87 RID: 7047
		public static readonly string UriSchemeMailto = UriParser.MailToUri.SchemeName;

		// Token: 0x04001B88 RID: 7048
		public static readonly string UriSchemeNews = UriParser.NewsUri.SchemeName;

		// Token: 0x04001B89 RID: 7049
		public static readonly string UriSchemeNntp = UriParser.NntpUri.SchemeName;

		// Token: 0x04001B8A RID: 7050
		public static readonly string UriSchemeNetTcp = UriParser.NetTcpUri.SchemeName;

		// Token: 0x04001B8B RID: 7051
		public static readonly string UriSchemeNetPipe = UriParser.NetPipeUri.SchemeName;

		// Token: 0x04001B8C RID: 7052
		public static readonly string SchemeDelimiter = "://";

		// Token: 0x04001B8D RID: 7053
		private string m_String;

		// Token: 0x04001B8E RID: 7054
		private string m_originalUnicodeString;

		// Token: 0x04001B8F RID: 7055
		private UriParser m_Syntax;

		// Token: 0x04001B90 RID: 7056
		private string m_DnsSafeHost;

		// Token: 0x04001B91 RID: 7057
		private Uri.Flags m_Flags;

		// Token: 0x04001B92 RID: 7058
		private Uri.UriInfo m_Info;

		// Token: 0x04001B93 RID: 7059
		private bool m_iriParsing;

		// Token: 0x04001B94 RID: 7060
		private static IInternetSecurityManager s_ManagerRef = null;

		// Token: 0x04001B95 RID: 7061
		private static object s_IntranetLock = new object();

		// Token: 0x04001B96 RID: 7062
		private static bool s_ConfigInitialized;

		// Token: 0x04001B97 RID: 7063
		private static UriIdnScope s_IdnScope = UriIdnScope.None;

		// Token: 0x04001B98 RID: 7064
		private static bool s_IriParsing = false;

		// Token: 0x04001B99 RID: 7065
		private static object s_initLock;

		// Token: 0x04001B9A RID: 7066
		private static readonly char[] HexUpperChars = new char[]
		{
			'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
			'A', 'B', 'C', 'D', 'E', 'F'
		};

		// Token: 0x04001B9B RID: 7067
		internal static readonly char[] HexLowerChars = new char[]
		{
			'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
			'a', 'b', 'c', 'd', 'e', 'f'
		};

		// Token: 0x04001B9C RID: 7068
		private static readonly char[] _WSchars = new char[] { ' ', '\n', '\r', '\t' };

		// Token: 0x02000355 RID: 853
		private enum ParsingError
		{
			// Token: 0x04001B9E RID: 7070
			None,
			// Token: 0x04001B9F RID: 7071
			BadFormat,
			// Token: 0x04001BA0 RID: 7072
			BadScheme,
			// Token: 0x04001BA1 RID: 7073
			BadAuthority,
			// Token: 0x04001BA2 RID: 7074
			EmptyUriString,
			// Token: 0x04001BA3 RID: 7075
			LastRelativeUriOkErrIndex = 4,
			// Token: 0x04001BA4 RID: 7076
			SchemeLimit,
			// Token: 0x04001BA5 RID: 7077
			SizeLimit,
			// Token: 0x04001BA6 RID: 7078
			MustRootedPath,
			// Token: 0x04001BA7 RID: 7079
			LastFatalErrIndex = 7,
			// Token: 0x04001BA8 RID: 7080
			BadHostName,
			// Token: 0x04001BA9 RID: 7081
			NonEmptyHost,
			// Token: 0x04001BAA RID: 7082
			BadPort,
			// Token: 0x04001BAB RID: 7083
			BadAuthorityTerminator,
			// Token: 0x04001BAC RID: 7084
			CannotCreateRelative
		}

		// Token: 0x02000356 RID: 854
		[Flags]
		private enum Flags : ulong
		{
			// Token: 0x04001BAE RID: 7086
			Zero = 0UL,
			// Token: 0x04001BAF RID: 7087
			SchemeNotCanonical = 1UL,
			// Token: 0x04001BB0 RID: 7088
			UserNotCanonical = 2UL,
			// Token: 0x04001BB1 RID: 7089
			HostNotCanonical = 4UL,
			// Token: 0x04001BB2 RID: 7090
			PortNotCanonical = 8UL,
			// Token: 0x04001BB3 RID: 7091
			PathNotCanonical = 16UL,
			// Token: 0x04001BB4 RID: 7092
			QueryNotCanonical = 32UL,
			// Token: 0x04001BB5 RID: 7093
			FragmentNotCanonical = 64UL,
			// Token: 0x04001BB6 RID: 7094
			CannotDisplayCanonical = 127UL,
			// Token: 0x04001BB7 RID: 7095
			E_UserNotCanonical = 128UL,
			// Token: 0x04001BB8 RID: 7096
			E_HostNotCanonical = 256UL,
			// Token: 0x04001BB9 RID: 7097
			E_PortNotCanonical = 512UL,
			// Token: 0x04001BBA RID: 7098
			E_PathNotCanonical = 1024UL,
			// Token: 0x04001BBB RID: 7099
			E_QueryNotCanonical = 2048UL,
			// Token: 0x04001BBC RID: 7100
			E_FragmentNotCanonical = 4096UL,
			// Token: 0x04001BBD RID: 7101
			E_CannotDisplayCanonical = 8064UL,
			// Token: 0x04001BBE RID: 7102
			ShouldBeCompressed = 8192UL,
			// Token: 0x04001BBF RID: 7103
			FirstSlashAbsent = 16384UL,
			// Token: 0x04001BC0 RID: 7104
			BackslashInPath = 32768UL,
			// Token: 0x04001BC1 RID: 7105
			IndexMask = 65535UL,
			// Token: 0x04001BC2 RID: 7106
			HostTypeMask = 458752UL,
			// Token: 0x04001BC3 RID: 7107
			HostNotParsed = 0UL,
			// Token: 0x04001BC4 RID: 7108
			IPv6HostType = 65536UL,
			// Token: 0x04001BC5 RID: 7109
			IPv4HostType = 131072UL,
			// Token: 0x04001BC6 RID: 7110
			DnsHostType = 196608UL,
			// Token: 0x04001BC7 RID: 7111
			UncHostType = 262144UL,
			// Token: 0x04001BC8 RID: 7112
			BasicHostType = 327680UL,
			// Token: 0x04001BC9 RID: 7113
			UnusedHostType = 393216UL,
			// Token: 0x04001BCA RID: 7114
			UnknownHostType = 458752UL,
			// Token: 0x04001BCB RID: 7115
			UserEscaped = 524288UL,
			// Token: 0x04001BCC RID: 7116
			AuthorityFound = 1048576UL,
			// Token: 0x04001BCD RID: 7117
			HasUserInfo = 2097152UL,
			// Token: 0x04001BCE RID: 7118
			LoopbackHost = 4194304UL,
			// Token: 0x04001BCF RID: 7119
			NotDefaultPort = 8388608UL,
			// Token: 0x04001BD0 RID: 7120
			UserDrivenParsing = 16777216UL,
			// Token: 0x04001BD1 RID: 7121
			CanonicalDnsHost = 33554432UL,
			// Token: 0x04001BD2 RID: 7122
			ErrorOrParsingRecursion = 67108864UL,
			// Token: 0x04001BD3 RID: 7123
			DosPath = 134217728UL,
			// Token: 0x04001BD4 RID: 7124
			UncPath = 268435456UL,
			// Token: 0x04001BD5 RID: 7125
			ImplicitFile = 536870912UL,
			// Token: 0x04001BD6 RID: 7126
			MinimalUriInfoSet = 1073741824UL,
			// Token: 0x04001BD7 RID: 7127
			AllUriInfoSet = 2147483648UL,
			// Token: 0x04001BD8 RID: 7128
			IdnHost = 4294967296UL,
			// Token: 0x04001BD9 RID: 7129
			HasUnicode = 8589934592UL,
			// Token: 0x04001BDA RID: 7130
			HostUnicodeNormalized = 17179869184UL,
			// Token: 0x04001BDB RID: 7131
			RestUnicodeNormalized = 34359738368UL,
			// Token: 0x04001BDC RID: 7132
			UnicodeHost = 68719476736UL,
			// Token: 0x04001BDD RID: 7133
			IntranetUri = 137438953472UL,
			// Token: 0x04001BDE RID: 7134
			UseOrigUncdStrOffset = 274877906944UL,
			// Token: 0x04001BDF RID: 7135
			UserIriCanonical = 549755813888UL,
			// Token: 0x04001BE0 RID: 7136
			PathIriCanonical = 1099511627776UL,
			// Token: 0x04001BE1 RID: 7137
			QueryIriCanonical = 2199023255552UL,
			// Token: 0x04001BE2 RID: 7138
			FragmentIriCanonical = 4398046511104UL,
			// Token: 0x04001BE3 RID: 7139
			IriCanonical = 8246337208320UL
		}

		// Token: 0x02000357 RID: 855
		private class UriInfo
		{
			// Token: 0x04001BE4 RID: 7140
			public string Host;

			// Token: 0x04001BE5 RID: 7141
			public string ScopeId;

			// Token: 0x04001BE6 RID: 7142
			public string String;

			// Token: 0x04001BE7 RID: 7143
			public Uri.Offset Offset;

			// Token: 0x04001BE8 RID: 7144
			public string DnsSafeHost;

			// Token: 0x04001BE9 RID: 7145
			public Uri.MoreInfo MoreInfo;
		}

		// Token: 0x02000358 RID: 856
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct Offset
		{
			// Token: 0x04001BEA RID: 7146
			public ushort Scheme;

			// Token: 0x04001BEB RID: 7147
			public ushort User;

			// Token: 0x04001BEC RID: 7148
			public ushort Host;

			// Token: 0x04001BED RID: 7149
			public ushort PortValue;

			// Token: 0x04001BEE RID: 7150
			public ushort Path;

			// Token: 0x04001BEF RID: 7151
			public ushort Query;

			// Token: 0x04001BF0 RID: 7152
			public ushort Fragment;

			// Token: 0x04001BF1 RID: 7153
			public ushort End;
		}

		// Token: 0x02000359 RID: 857
		private class MoreInfo
		{
			// Token: 0x04001BF2 RID: 7154
			public string Path;

			// Token: 0x04001BF3 RID: 7155
			public string Query;

			// Token: 0x04001BF4 RID: 7156
			public string Fragment;

			// Token: 0x04001BF5 RID: 7157
			public string AbsoluteUri;

			// Token: 0x04001BF6 RID: 7158
			public int Hash;

			// Token: 0x04001BF7 RID: 7159
			public string RemoteUrl;
		}

		// Token: 0x0200035A RID: 858
		private enum IdnScopeFromConfig
		{
			// Token: 0x04001BF9 RID: 7161
			None,
			// Token: 0x04001BFA RID: 7162
			AllExceptIntranet,
			// Token: 0x04001BFB RID: 7163
			All,
			// Token: 0x04001BFC RID: 7164
			Invalid = 2147483646,
			// Token: 0x04001BFD RID: 7165
			NotFound
		}

		// Token: 0x0200035B RID: 859
		private enum IriParsingFromConfig
		{
			// Token: 0x04001BFF RID: 7167
			False,
			// Token: 0x04001C00 RID: 7168
			True,
			// Token: 0x04001C01 RID: 7169
			Invalid = 2147483646,
			// Token: 0x04001C02 RID: 7170
			NotFound
		}

		// Token: 0x0200035C RID: 860
		[Flags]
		private enum Check
		{
			// Token: 0x04001C04 RID: 7172
			None = 0,
			// Token: 0x04001C05 RID: 7173
			EscapedCanonical = 1,
			// Token: 0x04001C06 RID: 7174
			DisplayCanonical = 2,
			// Token: 0x04001C07 RID: 7175
			DotSlashAttn = 4,
			// Token: 0x04001C08 RID: 7176
			DotSlashEscaped = 128,
			// Token: 0x04001C09 RID: 7177
			BackslashInPath = 16,
			// Token: 0x04001C0A RID: 7178
			ReservedFound = 32,
			// Token: 0x04001C0B RID: 7179
			NotIriCanonical = 64,
			// Token: 0x04001C0C RID: 7180
			FoundNonAscii = 8
		}

		// Token: 0x0200035D RID: 861
		[Flags]
		private enum UnescapeMode
		{
			// Token: 0x04001C0E RID: 7182
			CopyOnly = 0,
			// Token: 0x04001C0F RID: 7183
			Escape = 1,
			// Token: 0x04001C10 RID: 7184
			Unescape = 2,
			// Token: 0x04001C11 RID: 7185
			EscapeUnescape = 3,
			// Token: 0x04001C12 RID: 7186
			V1ToStringFlag = 4,
			// Token: 0x04001C13 RID: 7187
			UnescapeAll = 8,
			// Token: 0x04001C14 RID: 7188
			UnescapeAllOrThrow = 24
		}
	}
}
