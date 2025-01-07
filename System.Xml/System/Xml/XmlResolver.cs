using System;
using System.IO;
using System.Net;

namespace System.Xml
{
	public abstract class XmlResolver
	{
		public abstract object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn);

		public virtual Uri ResolveUri(Uri baseUri, string relativeUri)
		{
			if (baseUri == null || (!baseUri.IsAbsoluteUri && baseUri.OriginalString.Length == 0))
			{
				Uri uri = new Uri(relativeUri, UriKind.RelativeOrAbsolute);
				if (!uri.IsAbsoluteUri && uri.OriginalString.Length > 0)
				{
					uri = new Uri(Path.GetFullPath(relativeUri));
				}
				return uri;
			}
			if (relativeUri == null || relativeUri.Length == 0)
			{
				return baseUri;
			}
			return new Uri(baseUri, relativeUri);
		}

		public abstract ICredentials Credentials { set; }
	}
}
