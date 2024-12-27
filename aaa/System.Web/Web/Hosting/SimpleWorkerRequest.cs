using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Web.Configuration;
using System.Web.Util;

namespace System.Web.Hosting
{
	// Token: 0x020002C1 RID: 705
	[ComVisible(false)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class SimpleWorkerRequest : HttpWorkerRequest
	{
		// Token: 0x06002443 RID: 9283 RVA: 0x0009B370 File Offset: 0x0009A370
		private void ExtractPagePathInfo()
		{
			int num = this._page.IndexOf('/');
			if (num >= 0)
			{
				this._pathInfo = this._page.Substring(num);
				this._page = this._page.Substring(0, num);
			}
		}

		// Token: 0x06002444 RID: 9284 RVA: 0x0009B3B4 File Offset: 0x0009A3B4
		private string GetPathInternal(bool includePathInfo)
		{
			string text = (this._appVirtPath.Equals("/") ? ("/" + this._page) : (this._appVirtPath + "/" + this._page));
			if (includePathInfo && this._pathInfo != null)
			{
				return text + this._pathInfo;
			}
			return text;
		}

		// Token: 0x06002445 RID: 9285 RVA: 0x0009B415 File Offset: 0x0009A415
		public override string GetUriPath()
		{
			return this.GetPathInternal(true);
		}

		// Token: 0x06002446 RID: 9286 RVA: 0x0009B41E File Offset: 0x0009A41E
		public override string GetQueryString()
		{
			return this._queryString;
		}

		// Token: 0x06002447 RID: 9287 RVA: 0x0009B428 File Offset: 0x0009A428
		public override string GetRawUrl()
		{
			string queryString = this.GetQueryString();
			if (!string.IsNullOrEmpty(queryString))
			{
				return this.GetPathInternal(true) + "?" + queryString;
			}
			return this.GetPathInternal(true);
		}

		// Token: 0x06002448 RID: 9288 RVA: 0x0009B45E File Offset: 0x0009A45E
		public override string GetHttpVerbName()
		{
			return "GET";
		}

		// Token: 0x06002449 RID: 9289 RVA: 0x0009B465 File Offset: 0x0009A465
		public override string GetHttpVersion()
		{
			return "HTTP/1.0";
		}

		// Token: 0x0600244A RID: 9290 RVA: 0x0009B46C File Offset: 0x0009A46C
		public override string GetRemoteAddress()
		{
			return "127.0.0.1";
		}

		// Token: 0x0600244B RID: 9291 RVA: 0x0009B473 File Offset: 0x0009A473
		public override int GetRemotePort()
		{
			return 0;
		}

		// Token: 0x0600244C RID: 9292 RVA: 0x0009B476 File Offset: 0x0009A476
		public override string GetLocalAddress()
		{
			return "127.0.0.1";
		}

		// Token: 0x0600244D RID: 9293 RVA: 0x0009B47D File Offset: 0x0009A47D
		public override int GetLocalPort()
		{
			return 80;
		}

		// Token: 0x0600244E RID: 9294 RVA: 0x0009B481 File Offset: 0x0009A481
		public override IntPtr GetUserToken()
		{
			return IntPtr.Zero;
		}

		// Token: 0x0600244F RID: 9295 RVA: 0x0009B488 File Offset: 0x0009A488
		public override string GetFilePath()
		{
			return this.GetPathInternal(false);
		}

		// Token: 0x06002450 RID: 9296 RVA: 0x0009B494 File Offset: 0x0009A494
		public override string GetFilePathTranslated()
		{
			string text = this._appPhysPath + this._page.Replace('/', '\\');
			InternalSecurityPermissions.PathDiscovery(text).Demand();
			return text;
		}

		// Token: 0x06002451 RID: 9297 RVA: 0x0009B4C8 File Offset: 0x0009A4C8
		public override string GetPathInfo()
		{
			if (this._pathInfo == null)
			{
				return string.Empty;
			}
			return this._pathInfo;
		}

		// Token: 0x06002452 RID: 9298 RVA: 0x0009B4DE File Offset: 0x0009A4DE
		public override string GetAppPath()
		{
			return this._appVirtPath;
		}

		// Token: 0x06002453 RID: 9299 RVA: 0x0009B4E6 File Offset: 0x0009A4E6
		public override string GetAppPathTranslated()
		{
			InternalSecurityPermissions.PathDiscovery(this._appPhysPath).Demand();
			return this._appPhysPath;
		}

		// Token: 0x06002454 RID: 9300 RVA: 0x0009B4FE File Offset: 0x0009A4FE
		public override string GetServerVariable(string name)
		{
			return string.Empty;
		}

		// Token: 0x06002455 RID: 9301 RVA: 0x0009B508 File Offset: 0x0009A508
		public override string MapPath(string path)
		{
			if (!this._hasRuntimeInfo)
			{
				return null;
			}
			string text = null;
			string text2 = this._appPhysPath.Substring(0, this._appPhysPath.Length - 1);
			if (string.IsNullOrEmpty(path) || path.Equals("/"))
			{
				text = text2;
			}
			if (StringUtil.StringStartsWith(path, this._appVirtPath))
			{
				text = text2 + path.Substring(this._appVirtPath.Length).Replace('/', '\\');
			}
			InternalSecurityPermissions.PathDiscovery(text).Demand();
			return text;
		}

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x06002456 RID: 9302 RVA: 0x0009B590 File Offset: 0x0009A590
		public override string MachineConfigPath
		{
			get
			{
				if (this._hasRuntimeInfo)
				{
					string machineConfigurationFilePath = HttpConfigurationSystem.MachineConfigurationFilePath;
					InternalSecurityPermissions.PathDiscovery(machineConfigurationFilePath).Demand();
					return machineConfigurationFilePath;
				}
				return null;
			}
		}

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x06002457 RID: 9303 RVA: 0x0009B5BC File Offset: 0x0009A5BC
		public override string RootWebConfigPath
		{
			get
			{
				if (this._hasRuntimeInfo)
				{
					string rootWebConfigurationFilePath = HttpConfigurationSystem.RootWebConfigurationFilePath;
					InternalSecurityPermissions.PathDiscovery(rootWebConfigurationFilePath).Demand();
					return rootWebConfigurationFilePath;
				}
				return null;
			}
		}

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x06002458 RID: 9304 RVA: 0x0009B5E5 File Offset: 0x0009A5E5
		public override string MachineInstallDirectory
		{
			get
			{
				if (this._hasRuntimeInfo)
				{
					InternalSecurityPermissions.PathDiscovery(this._installDir).Demand();
					return this._installDir;
				}
				return null;
			}
		}

		// Token: 0x06002459 RID: 9305 RVA: 0x0009B607 File Offset: 0x0009A607
		public override void SendStatus(int statusCode, string statusDescription)
		{
		}

		// Token: 0x0600245A RID: 9306 RVA: 0x0009B609 File Offset: 0x0009A609
		public override void SendKnownResponseHeader(int index, string value)
		{
		}

		// Token: 0x0600245B RID: 9307 RVA: 0x0009B60B File Offset: 0x0009A60B
		public override void SendUnknownResponseHeader(string name, string value)
		{
		}

		// Token: 0x0600245C RID: 9308 RVA: 0x0009B60D File Offset: 0x0009A60D
		public override void SendResponseFromMemory(byte[] data, int length)
		{
			this._output.Write(Encoding.Default.GetChars(data, 0, length));
		}

		// Token: 0x0600245D RID: 9309 RVA: 0x0009B627 File Offset: 0x0009A627
		public override void SendResponseFromFile(string filename, long offset, long length)
		{
		}

		// Token: 0x0600245E RID: 9310 RVA: 0x0009B629 File Offset: 0x0009A629
		public override void SendResponseFromFile(IntPtr handle, long offset, long length)
		{
		}

		// Token: 0x0600245F RID: 9311 RVA: 0x0009B62B File Offset: 0x0009A62B
		public override void FlushResponse(bool finalFlush)
		{
		}

		// Token: 0x06002460 RID: 9312 RVA: 0x0009B62D File Offset: 0x0009A62D
		public override void EndOfRequest()
		{
		}

		// Token: 0x06002461 RID: 9313 RVA: 0x0009B62F File Offset: 0x0009A62F
		internal override void UpdateResponseCounters(bool finalFlush, int bytesOut)
		{
			if (finalFlush)
			{
				PerfCounters.DecrementGlobalCounter(GlobalPerfCounter.REQUESTS_CURRENT);
				PerfCounters.DecrementCounter(AppPerfCounter.REQUESTS_EXECUTING);
			}
			if (bytesOut > 0)
			{
				PerfCounters.IncrementCounterEx(AppPerfCounter.REQUEST_BYTES_OUT, bytesOut);
			}
		}

		// Token: 0x06002462 RID: 9314 RVA: 0x0009B64E File Offset: 0x0009A64E
		internal override void UpdateRequestCounters(int bytesIn)
		{
			if (bytesIn > 0)
			{
				PerfCounters.IncrementCounterEx(AppPerfCounter.REQUEST_BYTES_IN, bytesIn);
			}
		}

		// Token: 0x06002463 RID: 9315 RVA: 0x0009B65C File Offset: 0x0009A65C
		private SimpleWorkerRequest()
		{
			PerfCounters.IncrementGlobalCounter(GlobalPerfCounter.REQUESTS_CURRENT);
			PerfCounters.IncrementCounter(AppPerfCounter.REQUESTS_TOTAL);
		}

		// Token: 0x06002464 RID: 9316 RVA: 0x0009B674 File Offset: 0x0009A674
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		public SimpleWorkerRequest(string page, string query, TextWriter output)
			: this()
		{
			this._queryString = query;
			this._output = output;
			this._page = page;
			this.ExtractPagePathInfo();
			this._appPhysPath = Thread.GetDomain().GetData(".appPath").ToString();
			this._appVirtPath = Thread.GetDomain().GetData(".appVPath").ToString();
			this._installDir = HttpRuntime.AspInstallDirectoryInternal;
			this._hasRuntimeInfo = true;
		}

		// Token: 0x06002465 RID: 9317 RVA: 0x0009B6E8 File Offset: 0x0009A6E8
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		public SimpleWorkerRequest(string appVirtualDir, string appPhysicalDir, string page, string query, TextWriter output)
			: this()
		{
			if (Thread.GetDomain().GetData(".appPath") != null)
			{
				throw new HttpException(SR.GetString("Wrong_SimpleWorkerRequest"));
			}
			this._appVirtPath = appVirtualDir;
			this._appPhysPath = appPhysicalDir;
			this._queryString = query;
			this._output = output;
			this._page = page;
			this.ExtractPagePathInfo();
			if (!StringUtil.StringEndsWith(this._appPhysPath, '\\'))
			{
				this._appPhysPath += "\\";
			}
			this._hasRuntimeInfo = false;
		}

		// Token: 0x04001C3C RID: 7228
		private bool _hasRuntimeInfo;

		// Token: 0x04001C3D RID: 7229
		private string _appVirtPath;

		// Token: 0x04001C3E RID: 7230
		private string _appPhysPath;

		// Token: 0x04001C3F RID: 7231
		private string _page;

		// Token: 0x04001C40 RID: 7232
		private string _pathInfo;

		// Token: 0x04001C41 RID: 7233
		private string _queryString;

		// Token: 0x04001C42 RID: 7234
		private TextWriter _output;

		// Token: 0x04001C43 RID: 7235
		private string _installDir;
	}
}
