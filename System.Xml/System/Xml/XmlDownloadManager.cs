using System;
using System.Collections;
using System.IO;
using System.Net;

namespace System.Xml
{
	internal class XmlDownloadManager
	{
		internal Stream GetStream(Uri uri, ICredentials credentials)
		{
			if (uri.Scheme == "file")
			{
				return new FileStream(uri.LocalPath, FileMode.Open, FileAccess.Read, FileShare.Read, 1);
			}
			return this.GetNonFileStream(uri, credentials);
		}

		private Stream GetNonFileStream(Uri uri, ICredentials credentials)
		{
			WebRequest webRequest = WebRequest.Create(uri);
			if (credentials != null)
			{
				webRequest.Credentials = credentials;
			}
			WebResponse response = webRequest.GetResponse();
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			if (httpWebRequest != null)
			{
				lock (this)
				{
					if (this.connections == null)
					{
						this.connections = new Hashtable();
					}
					OpenedHost openedHost = (OpenedHost)this.connections[httpWebRequest.Address.Host];
					if (openedHost == null)
					{
						openedHost = new OpenedHost();
					}
					if (openedHost.nonCachedConnectionsCount < httpWebRequest.ServicePoint.ConnectionLimit - 1)
					{
						if (openedHost.nonCachedConnectionsCount == 0)
						{
							this.connections.Add(httpWebRequest.Address.Host, openedHost);
						}
						openedHost.nonCachedConnectionsCount++;
						return new XmlRegisteredNonCachedStream(response.GetResponseStream(), this, httpWebRequest.Address.Host);
					}
					return new XmlCachedStream(response.ResponseUri, response.GetResponseStream());
				}
			}
			return response.GetResponseStream();
		}

		internal void Remove(string host)
		{
			lock (this)
			{
				OpenedHost openedHost = (OpenedHost)this.connections[host];
				if (openedHost != null && --openedHost.nonCachedConnectionsCount == 0)
				{
					this.connections.Remove(host);
				}
			}
		}

		private Hashtable connections;
	}
}
