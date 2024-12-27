using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.UI;

namespace System.Runtime.Remoting.Channels.Http
{
	// Token: 0x02000039 RID: 57
	public class HttpRemotingHandlerFactory : IHttpHandlerFactory
	{
		// Token: 0x060001DE RID: 478 RVA: 0x00009822 File Offset: 0x00008822
		private void DumpRequest(HttpContext context)
		{
			HttpRequest request = context.Request;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000982C File Offset: 0x0000882C
		private void ConfigureAppName(HttpRequest httpRequest)
		{
			if (RemotingConfiguration.ApplicationName == null)
			{
				lock (HttpRemotingHandlerFactory.s_configLock)
				{
					if (RemotingConfiguration.ApplicationName == null)
					{
						RemotingConfiguration.ApplicationName = httpRequest.ApplicationPath;
					}
				}
			}
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00009878 File Offset: 0x00008878
		public IHttpHandler GetHandler(HttpContext context, string verb, string url, string filePath)
		{
			this.DumpRequest(context);
			HttpRequest request = context.Request;
			this.ConfigureAppName(request);
			string text = request.QueryString[null];
			bool flag = string.Compare(request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase) == 0;
			bool flag2 = File.Exists(request.PhysicalPath);
			if (flag && flag2 && text == null)
			{
				return this.WebServicesFactory.GetHandler(context, verb, url, filePath);
			}
			if (flag2)
			{
				Type compiledType = WebServiceParser.GetCompiledType(url, context);
				string text2 = Dns.GetHostName() + request.ApplicationPath;
				string[] array = request.PhysicalPath.Split(new char[] { '\\' });
				string text3 = array[array.Length - 1];
				Type type = (Type)HttpRemotingHandlerFactory.s_registeredDynamicTypeTable[text3];
				if (type != compiledType)
				{
					RegistrationHelper.RegisterType(text2, compiledType, text3);
					HttpRemotingHandlerFactory.s_registeredDynamicTypeTable[text3] = compiledType;
				}
				return new HttpRemotingHandler();
			}
			return new HttpRemotingHandler();
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x00009968 File Offset: 0x00008968
		private IHttpHandlerFactory WebServicesFactory
		{
			get
			{
				if (this._webServicesFactory == null)
				{
					lock (this)
					{
						if (this._webServicesFactory == null)
						{
							this._webServicesFactory = Activator.CreateInstance(HttpRemotingHandlerFactory.WebServicesFactoryType);
						}
					}
				}
				return (IHttpHandlerFactory)this._webServicesFactory;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x000099C4 File Offset: 0x000089C4
		private static Type WebServicesFactoryType
		{
			get
			{
				if (HttpRemotingHandlerFactory.s_webServicesFactoryType == null)
				{
					Assembly assembly = Assembly.Load("System.Web.Services, Version=2.0.0.0, Culture=neutral, PublicKeyToken= b03f5f7f11d50a3a");
					if (assembly == null)
					{
						throw new RemotingException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_AssemblyLoadFailed"), new object[] { "System.Web.Services" }));
					}
					HttpRemotingHandlerFactory.s_webServicesFactoryType = assembly.GetType("System.Web.Services.Protocols.WebServiceHandlerFactory");
				}
				return HttpRemotingHandlerFactory.s_webServicesFactoryType;
			}
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00009A25 File Offset: 0x00008A25
		public void ReleaseHandler(IHttpHandler handler)
		{
			if (this._webServicesFactory != null)
			{
				((IHttpHandlerFactory)this._webServicesFactory).ReleaseHandler(handler);
				this._webServicesFactory = null;
			}
		}

		// Token: 0x0400015A RID: 346
		internal object _webServicesFactory;

		// Token: 0x0400015B RID: 347
		internal static Type s_webServicesFactoryType = null;

		// Token: 0x0400015C RID: 348
		internal static object s_configLock = new object();

		// Token: 0x0400015D RID: 349
		internal static Hashtable s_registeredDynamicTypeTable = Hashtable.Synchronized(new Hashtable());
	}
}
