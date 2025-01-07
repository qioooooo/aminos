using System;
using System.Net;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;

namespace System.Xml
{
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class XmlSecureResolver : XmlResolver
	{
		public XmlSecureResolver(XmlResolver resolver, string securityUrl)
			: this(resolver, XmlSecureResolver.CreateEvidenceForUrl(securityUrl))
		{
		}

		public XmlSecureResolver(XmlResolver resolver, Evidence evidence)
			: this(resolver, SecurityManager.ResolvePolicy(evidence))
		{
		}

		public XmlSecureResolver(XmlResolver resolver, PermissionSet permissionSet)
		{
			this.resolver = resolver;
			this.permissionSet = permissionSet;
		}

		public override ICredentials Credentials
		{
			set
			{
				this.resolver.Credentials = value;
			}
		}

		public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
		{
			this.permissionSet.PermitOnly();
			return this.resolver.GetEntity(absoluteUri, role, ofObjectToReturn);
		}

		public override Uri ResolveUri(Uri baseUri, string relativeUri)
		{
			return this.resolver.ResolveUri(baseUri, relativeUri);
		}

		public static Evidence CreateEvidenceForUrl(string securityUrl)
		{
			Evidence evidence = new Evidence();
			if (securityUrl != null && securityUrl.Length > 0)
			{
				evidence.AddHost(new Url(securityUrl));
				evidence.AddHost(Zone.CreateFromUrl(securityUrl));
				Uri uri = new Uri(securityUrl, UriKind.RelativeOrAbsolute);
				if (uri.IsAbsoluteUri && !uri.IsFile)
				{
					evidence.AddHost(Site.CreateFromUrl(securityUrl));
				}
			}
			return evidence;
		}

		private XmlResolver resolver;

		private PermissionSet permissionSet;
	}
}
