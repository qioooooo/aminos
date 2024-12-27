using System;
using System.ComponentModel;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Web.Services.Diagnostics;
using System.Web.Services.Protocols;
using System.Xml;

namespace System.Web.Services.Discovery
{
	// Token: 0x020000A8 RID: 168
	public sealed class DiscoveryRequestHandler : IHttpHandler
	{
		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000475 RID: 1141 RVA: 0x00016620 File Offset: 0x00015620
		public bool IsReusable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00016624 File Offset: 0x00015624
		public void ProcessRequest(HttpContext context)
		{
			TraceMethod traceMethod = (Tracing.On ? new TraceMethod(this, "ProcessRequest", new object[0]) : null);
			if (Tracing.On)
			{
				Tracing.Enter("IHttpHandler.ProcessRequest", traceMethod, Tracing.Details(context.Request));
			}
			new PermissionSet(PermissionState.Unrestricted).Demand();
			string physicalPath = context.Request.PhysicalPath;
			bool traceVerbose = CompModSwitches.DynamicDiscoverySearcher.TraceVerbose;
			if (File.Exists(physicalPath))
			{
				DynamicDiscoveryDocument dynamicDiscoveryDocument = null;
				FileStream fileStream = null;
				try
				{
					fileStream = new FileStream(physicalPath, FileMode.Open, FileAccess.Read);
					if (new XmlTextReader(fileStream)
					{
						WhitespaceHandling = WhitespaceHandling.Significant,
						XmlResolver = null,
						ProhibitDtd = true
					}.IsStartElement("dynamicDiscovery", "urn:schemas-dynamicdiscovery:disco.2000-03-17"))
					{
						fileStream.Position = 0L;
						dynamicDiscoveryDocument = DynamicDiscoveryDocument.Load(fileStream);
					}
				}
				finally
				{
					if (fileStream != null)
					{
						fileStream.Close();
					}
				}
				if (dynamicDiscoveryDocument != null)
				{
					string[] array = new string[dynamicDiscoveryDocument.ExcludePaths.Length];
					string directoryName = Path.GetDirectoryName(physicalPath);
					string text = Path.GetFileName(physicalPath);
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = dynamicDiscoveryDocument.ExcludePaths[i].Path;
					}
					Uri url = context.Request.Url;
					string text2 = url.GetComponents(UriComponents.AbsoluteUri, UriFormat.UriEscaped).Replace("#", "%23");
					string dirPartOfPath = DiscoveryRequestHandler.GetDirPartOfPath(text2);
					string dirPartOfPath2 = DiscoveryRequestHandler.GetDirPartOfPath(url.LocalPath);
					DynamicDiscoSearcher dynamicDiscoSearcher;
					if (dirPartOfPath2.Length == 0 || CompModSwitches.DynamicDiscoveryVirtualSearch.Enabled)
					{
						text = DiscoveryRequestHandler.GetFilePartOfPath(text2);
						dynamicDiscoSearcher = new DynamicVirtualDiscoSearcher(directoryName, array, dirPartOfPath);
					}
					else
					{
						dynamicDiscoSearcher = new DynamicPhysicalDiscoSearcher(directoryName, array, dirPartOfPath);
					}
					bool traceVerbose2 = CompModSwitches.DynamicDiscoverySearcher.TraceVerbose;
					dynamicDiscoSearcher.Search(text);
					DiscoveryDocument discoveryDocument = dynamicDiscoSearcher.DiscoveryDocument;
					MemoryStream memoryStream = new MemoryStream(1024);
					StreamWriter streamWriter = new StreamWriter(memoryStream, new UTF8Encoding(false));
					discoveryDocument.Write(streamWriter);
					memoryStream.Position = 0L;
					byte[] array2 = new byte[(int)memoryStream.Length];
					int num = memoryStream.Read(array2, 0, array2.Length);
					context.Response.ContentType = ContentType.Compose("text/xml", Encoding.UTF8);
					context.Response.OutputStream.Write(array2, 0, num);
				}
				else
				{
					context.Response.ContentType = "text/xml";
					context.Response.WriteFile(physicalPath);
				}
				if (Tracing.On)
				{
					Tracing.Exit("IHttpHandler.ProcessRequest", traceMethod);
				}
				return;
			}
			if (Tracing.On)
			{
				Tracing.Exit("IHttpHandler.ProcessRequest", traceMethod);
			}
			throw new HttpException(404, Res.GetString("WebPathNotFound", new object[] { context.Request.Path }));
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x000168D0 File Offset: 0x000158D0
		private static string GetDirPartOfPath(string str)
		{
			int num = str.LastIndexOf('/');
			if (num <= 0)
			{
				return "";
			}
			return str.Substring(0, num);
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x000168F8 File Offset: 0x000158F8
		private static string GetFilePartOfPath(string str)
		{
			int num = str.LastIndexOf('/');
			if (num < 0)
			{
				return str;
			}
			if (num == str.Length - 1)
			{
				return "";
			}
			return str.Substring(num + 1);
		}
	}
}
