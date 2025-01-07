using System;
using System.IO;
using System.Net;
using System.Security.Permissions;
using System.Threading;

namespace System.Xml
{
	public class XmlUrlResolver : XmlResolver
	{
		private static XmlDownloadManager DownloadManager
		{
			get
			{
				if (XmlUrlResolver.s_DownloadManager == null)
				{
					object obj = new XmlDownloadManager();
					Interlocked.CompareExchange(ref XmlUrlResolver.s_DownloadManager, obj, null);
				}
				return (XmlDownloadManager)XmlUrlResolver.s_DownloadManager;
			}
		}

		public override ICredentials Credentials
		{
			set
			{
				this._credentials = value;
			}
		}

		public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
		{
			if (ofObjectToReturn == null || ofObjectToReturn == typeof(Stream))
			{
				return XmlUrlResolver.DownloadManager.GetStream(absoluteUri, this._credentials);
			}
			throw new XmlException("Xml_UnsupportedClass", string.Empty);
		}

		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		public override Uri ResolveUri(Uri baseUri, string relativeUri)
		{
			return base.ResolveUri(baseUri, relativeUri);
		}

		private static object s_DownloadManager;

		private ICredentials _credentials;
	}
}
