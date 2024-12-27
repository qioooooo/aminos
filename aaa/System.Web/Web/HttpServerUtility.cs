using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x0200008C RID: 140
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HttpServerUtility
	{
		// Token: 0x06000722 RID: 1826 RVA: 0x0001F114 File Offset: 0x0001E114
		internal HttpServerUtility(HttpContext context)
		{
			this._context = context;
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0001F123 File Offset: 0x0001E123
		internal HttpServerUtility(HttpApplication application)
		{
			this._application = application;
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x0001F134 File Offset: 0x0001E134
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		public object CreateObject(string progID)
		{
			Type type = null;
			try
			{
				type = Type.GetTypeFromProgID(progID);
			}
			catch
			{
			}
			if (type == null)
			{
				throw new HttpException(SR.GetString("Could_not_create_object_of_type", new object[] { progID }));
			}
			AspCompatApplicationStep.CheckThreadingModel(progID, type.GUID);
			object obj = Activator.CreateInstance(type);
			AspCompatApplicationStep.OnPageStart(obj);
			return obj;
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0001F19C File Offset: 0x0001E19C
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		public object CreateObject(Type type)
		{
			AspCompatApplicationStep.CheckThreadingModel(type.FullName, type.GUID);
			object obj = Activator.CreateInstance(type);
			AspCompatApplicationStep.OnPageStart(obj);
			return obj;
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x0001F1C8 File Offset: 0x0001E1C8
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		public object CreateObjectFromClsid(string clsid)
		{
			object obj = null;
			Guid guid = new Guid(clsid);
			AspCompatApplicationStep.CheckThreadingModel(clsid, guid);
			try
			{
				Type typeFromCLSID = Type.GetTypeFromCLSID(guid, null, true);
				obj = Activator.CreateInstance(typeFromCLSID);
			}
			catch
			{
			}
			if (obj == null)
			{
				throw new HttpException(SR.GetString("Could_not_create_object_from_clsid", new object[] { clsid }));
			}
			AspCompatApplicationStep.OnPageStart(obj);
			return obj;
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x0001F234 File Offset: 0x0001E234
		internal static CultureInfo CreateReadOnlyCultureInfo(string name)
		{
			if (!HttpServerUtility._cultureCache.Contains(name))
			{
				lock (HttpServerUtility._cultureCache)
				{
					if (HttpServerUtility._cultureCache[name] == null)
					{
						HttpServerUtility._cultureCache[name] = CultureInfo.ReadOnly(new CultureInfo(name));
					}
				}
			}
			return (CultureInfo)HttpServerUtility._cultureCache[name];
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x0001F2A8 File Offset: 0x0001E2A8
		internal static CultureInfo CreateReadOnlySpecificCultureInfo(string name)
		{
			if (name.IndexOf('-') > 0)
			{
				return HttpServerUtility.CreateReadOnlyCultureInfo(name);
			}
			CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture(name);
			if (!HttpServerUtility._cultureCache.Contains(cultureInfo.Name))
			{
				lock (HttpServerUtility._cultureCache)
				{
					if (HttpServerUtility._cultureCache[cultureInfo.Name] == null)
					{
						HttpServerUtility._cultureCache[cultureInfo.Name] = CultureInfo.ReadOnly(cultureInfo);
					}
				}
			}
			return (CultureInfo)HttpServerUtility._cultureCache[cultureInfo.Name];
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x0001F344 File Offset: 0x0001E344
		internal static CultureInfo CreateReadOnlyCultureInfo(int culture)
		{
			if (!HttpServerUtility._cultureCache.Contains(culture))
			{
				lock (HttpServerUtility._cultureCache)
				{
					if (HttpServerUtility._cultureCache[culture] == null)
					{
						HttpServerUtility._cultureCache[culture] = CultureInfo.ReadOnly(new CultureInfo(culture));
					}
				}
			}
			return (CultureInfo)HttpServerUtility._cultureCache[culture];
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x0001F3CC File Offset: 0x0001E3CC
		public string MapPath(string path)
		{
			if (this._context == null)
			{
				throw new HttpException(SR.GetString("Server_not_available"));
			}
			bool hideRequestResponse = this._context.HideRequestResponse;
			string text;
			try
			{
				if (hideRequestResponse)
				{
					this._context.HideRequestResponse = false;
				}
				text = this._context.Request.MapPath(path);
			}
			finally
			{
				if (hideRequestResponse)
				{
					this._context.HideRequestResponse = true;
				}
			}
			return text;
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x0001F444 File Offset: 0x0001E444
		public Exception GetLastError()
		{
			if (this._context != null)
			{
				return this._context.Error;
			}
			if (this._application != null)
			{
				return this._application.LastError;
			}
			return null;
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0001F46F File Offset: 0x0001E46F
		public void ClearError()
		{
			if (this._context != null)
			{
				this._context.ClearError();
				return;
			}
			if (this._application != null)
			{
				this._application.ClearError();
			}
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x0001F498 File Offset: 0x0001E498
		public void Execute(string path)
		{
			this.Execute(path, null, true);
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x0001F4A3 File Offset: 0x0001E4A3
		public void Execute(string path, TextWriter writer)
		{
			this.Execute(path, writer, true);
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x0001F4AE File Offset: 0x0001E4AE
		public void Execute(string path, bool preserveForm)
		{
			this.Execute(path, null, preserveForm);
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x0001F4BC File Offset: 0x0001E4BC
		public void Execute(string path, TextWriter writer, bool preserveForm)
		{
			if (this._context == null)
			{
				throw new HttpException(SR.GetString("Server_not_available"));
			}
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			string text = null;
			HttpRequest request = this._context.Request;
			HttpResponse response = this._context.Response;
			path = response.RemoveAppPathModifier(path);
			int num = path.IndexOf('?');
			if (num >= 0)
			{
				text = path.Substring(num + 1);
				path = path.Substring(0, num);
			}
			if (!UrlPath.IsValidVirtualPathWithoutProtocol(path))
			{
				throw new ArgumentException(SR.GetString("Invalid_path_for_child_request", new object[] { path }));
			}
			VirtualPath virtualPath = VirtualPath.Create(path);
			IHttpHandler httpHandler = null;
			string text2 = request.MapPath(virtualPath);
			VirtualPath virtualPath2 = request.FilePathObject.Combine(virtualPath);
			InternalSecurityPermissions.FileReadAccess(text2).Demand();
			InternalSecurityPermissions.Unrestricted.Assert();
			try
			{
				if (StringUtil.StringEndsWith(virtualPath.VirtualPathString, '.'))
				{
					throw new HttpException(404, string.Empty);
				}
				bool flag = !virtualPath2.IsWithinAppRoot;
				using (new HttpContextWrapper(this._context))
				{
					try
					{
						this._context.ServerExecuteDepth++;
						if (this._context.WorkerRequest is IIS7WorkerRequest)
						{
							httpHandler = this._context.ApplicationInstance.MapIntegratedHttpHandler(this._context, request.RequestType, virtualPath2, text2, flag, true);
						}
						else
						{
							httpHandler = this._context.ApplicationInstance.MapHttpHandler(this._context, request.RequestType, virtualPath2, text2, flag);
						}
					}
					finally
					{
						this._context.ServerExecuteDepth--;
					}
				}
			}
			catch (Exception ex)
			{
				if (ex is HttpException)
				{
					int httpCode = ((HttpException)ex).GetHttpCode();
					if (httpCode != 500 && httpCode != 404)
					{
						ex = null;
					}
				}
				throw new HttpException(SR.GetString("Error_executing_child_request_for_path", new object[] { path }), ex);
			}
			this.ExecuteInternal(httpHandler, writer, preserveForm, true, virtualPath, virtualPath2, text2, null, text);
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x0001F6F0 File Offset: 0x0001E6F0
		public void Execute(IHttpHandler handler, TextWriter writer, bool preserveForm)
		{
			if (this._context == null)
			{
				throw new HttpException(SR.GetString("Server_not_available"));
			}
			this.Execute(handler, writer, preserveForm, true);
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x0001F714 File Offset: 0x0001E714
		internal void Execute(IHttpHandler handler, TextWriter writer, bool preserveForm, bool setPreviousPage)
		{
			HttpRequest request = this._context.Request;
			VirtualPath currentExecutionFilePathObject = request.CurrentExecutionFilePathObject;
			string text = request.MapPath(currentExecutionFilePathObject);
			this.ExecuteInternal(handler, writer, preserveForm, setPreviousPage, null, currentExecutionFilePathObject, text, null, null);
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0001F74C File Offset: 0x0001E74C
		private void ExecuteInternal(IHttpHandler handler, TextWriter writer, bool preserveForm, bool setPreviousPage, VirtualPath path, VirtualPath filePath, string physPath, Exception error, string queryStringOverride)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			HttpRequest request = this._context.Request;
			HttpResponse response = this._context.Response;
			HttpApplication applicationInstance = this._context.ApplicationInstance;
			HttpValueCollection httpValueCollection = null;
			VirtualPath virtualPath = null;
			string text = null;
			TextWriter textWriter = null;
			AspNetSynchronizationContext aspNetSynchronizationContext = null;
			this.VerifyTransactionFlow(handler);
			this._context.PushTraceContext();
			this._context.SetCurrentHandler(handler);
			bool enabled = this._context.SyncContext.Enabled;
			this._context.SyncContext.Disable();
			try
			{
				try
				{
					this._context.ServerExecuteDepth++;
					virtualPath = request.SwitchCurrentExecutionFilePath(filePath);
					if (!preserveForm)
					{
						httpValueCollection = request.SwitchForm(new HttpValueCollection());
						if (queryStringOverride == null)
						{
							queryStringOverride = string.Empty;
						}
					}
					if (queryStringOverride != null)
					{
						text = request.QueryStringText;
						request.QueryStringText = queryStringOverride;
					}
					if (writer != null)
					{
						textWriter = response.SwitchWriter(writer);
					}
					Page page = handler as Page;
					if (page != null)
					{
						if (setPreviousPage)
						{
							page.SetPreviousPage(this._context.PreviousHandler as Page);
						}
						Page page2 = this._context.Handler as Page;
						if (page2 != null && page2.SmartNavigation)
						{
							page.SmartNavigation = true;
						}
						if (page is IHttpAsyncHandler)
						{
							aspNetSynchronizationContext = this._context.InstallNewAspNetSynchronizationContext();
						}
					}
					if ((handler is StaticFileHandler || handler is DefaultHttpHandler) && !DefaultHttpHandler.IsClassicAspRequest(filePath.VirtualPathString))
					{
						try
						{
							response.WriteFile(physPath);
							goto IL_027C;
						}
						catch
						{
							error = new HttpException(404, string.Empty);
							goto IL_027C;
						}
					}
					if (!(handler is Page))
					{
						error = new HttpException(404, string.Empty);
					}
					else
					{
						if (handler is IHttpAsyncHandler)
						{
							bool isInCancellablePeriod = this._context.IsInCancellablePeriod;
							if (isInCancellablePeriod)
							{
								this._context.EndCancellablePeriod();
							}
							try
							{
								IHttpAsyncHandler httpAsyncHandler = (IHttpAsyncHandler)handler;
								IAsyncResult asyncResult = httpAsyncHandler.BeginProcessRequest(this._context, null, null);
								if (!asyncResult.IsCompleted)
								{
									bool flag = false;
									try
									{
										try
										{
										}
										finally
										{
											Monitor.Exit(applicationInstance);
											flag = true;
										}
										WaitHandle asyncWaitHandle = asyncResult.AsyncWaitHandle;
										if (asyncWaitHandle != null)
										{
											asyncWaitHandle.WaitOne();
										}
										else
										{
											while (!asyncResult.IsCompleted)
											{
												Thread.Sleep(1);
											}
										}
									}
									finally
									{
										if (flag)
										{
											Monitor.Enter(applicationInstance);
										}
									}
								}
								try
								{
									httpAsyncHandler.EndProcessRequest(asyncResult);
								}
								catch (Exception ex)
								{
									error = ex;
								}
								goto IL_027C;
							}
							finally
							{
								if (isInCancellablePeriod)
								{
									this._context.BeginCancellablePeriod();
								}
							}
						}
						using (new HttpContextWrapper(this._context))
						{
							try
							{
								handler.ProcessRequest(this._context);
							}
							catch (Exception ex2)
							{
								error = ex2;
							}
						}
					}
					IL_027C:;
				}
				finally
				{
					this._context.ServerExecuteDepth--;
					this._context.RestoreCurrentHandler();
					if (textWriter != null)
					{
						response.SwitchWriter(textWriter);
					}
					if (queryStringOverride != null && text != null)
					{
						request.QueryStringText = text;
					}
					if (httpValueCollection != null)
					{
						request.SwitchForm(httpValueCollection);
					}
					request.SwitchCurrentExecutionFilePath(virtualPath);
					if (aspNetSynchronizationContext != null)
					{
						this._context.RestoreSavedAspNetSynchronizationContext(aspNetSynchronizationContext);
					}
					if (enabled)
					{
						this._context.SyncContext.Enable();
					}
					this._context.PopTraceContext();
				}
			}
			catch
			{
				throw;
			}
			if (error == null)
			{
				return;
			}
			if (error is HttpException && ((HttpException)error).GetHttpCode() != 500)
			{
				error = null;
			}
			if (path != null)
			{
				throw new HttpException(SR.GetString("Error_executing_child_request_for_path", new object[] { path }), error);
			}
			throw new HttpException(SR.GetString("Error_executing_child_request_for_handler", new object[] { handler.GetType().ToString() }), error);
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0001FBB8 File Offset: 0x0001EBB8
		public void Transfer(string path, bool preserveForm)
		{
			Page page = this._context.Handler as Page;
			if (page != null && page.IsCallback)
			{
				throw new ApplicationException(SR.GetString("Transfer_not_allowed_in_callback"));
			}
			this.Execute(path, null, preserveForm);
			this._context.Response.End();
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x0001FC0C File Offset: 0x0001EC0C
		public void Transfer(string path)
		{
			bool preventPostback = this._context.PreventPostback;
			this._context.PreventPostback = true;
			this.Transfer(path, true);
			this._context.PreventPostback = preventPostback;
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0001FC48 File Offset: 0x0001EC48
		public void Transfer(IHttpHandler handler, bool preserveForm)
		{
			Page page = handler as Page;
			if (page != null && page.IsCallback)
			{
				throw new ApplicationException(SR.GetString("Transfer_not_allowed_in_callback"));
			}
			this.Execute(handler, null, preserveForm);
			this._context.Response.End();
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x0001FC90 File Offset: 0x0001EC90
		public void TransferRequest(string path)
		{
			this.TransferRequest(path, false, null, null);
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x0001FC9C File Offset: 0x0001EC9C
		public void TransferRequest(string path, bool preserveForm)
		{
			this.TransferRequest(path, preserveForm, null, null);
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0001FCA8 File Offset: 0x0001ECA8
		public void TransferRequest(string path, bool preserveForm, string method, NameValueCollection headers)
		{
			if (!HttpRuntime.UseIntegratedPipeline)
			{
				throw new PlatformNotSupportedException(SR.GetString("Requires_Iis_Integrated_Mode"));
			}
			if (this._context == null)
			{
				throw new HttpException(SR.GetString("Server_not_available"));
			}
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			IIS7WorkerRequest iis7WorkerRequest = this._context.WorkerRequest as IIS7WorkerRequest;
			HttpRequest request = this._context.Request;
			HttpResponse response = this._context.Response;
			if (iis7WorkerRequest == null)
			{
				throw new HttpException(SR.GetString("Server_not_available"));
			}
			path = response.RemoveAppPathModifier(path);
			string text = null;
			int num = path.IndexOf('?');
			if (num >= 0)
			{
				text = ((num < path.Length - 1) ? path.Substring(num + 1) : string.Empty);
				path = path.Substring(0, num);
			}
			if (!UrlPath.IsValidVirtualPathWithoutProtocol(path))
			{
				throw new ArgumentException(SR.GetString("Invalid_path_for_child_request", new object[] { path }));
			}
			VirtualPath virtualPath = request.FilePathObject.Combine(VirtualPath.Create(path));
			string text2 = request.MapPath(path);
			InternalSecurityPermissions.FileReadAccess(text2).Demand();
			iis7WorkerRequest.ScheduleExecuteUrl(virtualPath.VirtualPathString, text, method, preserveForm, preserveForm ? request.EntityBody : null, headers);
			this._context.ApplicationInstance.EnsureReleaseState();
			this._context.ApplicationInstance.CompleteRequest();
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0001FE00 File Offset: 0x0001EE00
		private void VerifyTransactionFlow(IHttpHandler handler)
		{
			Page page = this._context.Handler as Page;
			Page page2 = handler as Page;
			if (page2 != null && page2.IsInAspCompatMode && page != null && !page.IsInAspCompatMode && Transactions.Utils.IsInTransaction)
			{
				throw new HttpException(SR.GetString("Transacted_page_calls_aspcompat"));
			}
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0001FE54 File Offset: 0x0001EE54
		internal static void ExecuteLocalRequestAndCaptureResponse(string path, TextWriter writer, ErrorFormatterGenerator errorFormatterGenerator)
		{
			HttpRequest httpRequest = new HttpRequest(VirtualPath.CreateAbsolute(path), string.Empty);
			HttpResponse httpResponse = new HttpResponse(writer);
			HttpContext httpContext = new HttpContext(httpRequest, httpResponse);
			HttpApplication httpApplication = HttpApplicationFactory.GetApplicationInstance(httpContext) as HttpApplication;
			httpContext.ApplicationInstance = httpApplication;
			try
			{
				httpContext.Server.Execute(path);
			}
			catch (HttpException ex)
			{
				if (errorFormatterGenerator != null)
				{
					httpContext.Response.SetOverrideErrorFormatter(errorFormatterGenerator.GetErrorFormatter(ex));
				}
				httpContext.Response.ReportRuntimeError(ex, false, true);
			}
			finally
			{
				if (httpApplication != null)
				{
					httpContext.ApplicationInstance = null;
					HttpApplicationFactory.RecycleApplicationInstance(httpApplication);
				}
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x0600073C RID: 1852 RVA: 0x0001FEFC File Offset: 0x0001EEFC
		public string MachineName
		{
			[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Medium)]
			get
			{
				return HttpServerUtility.GetMachineNameInternal();
			}
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0001FF04 File Offset: 0x0001EF04
		internal static string GetMachineNameInternal()
		{
			if (HttpServerUtility._machineName != null)
			{
				return HttpServerUtility._machineName;
			}
			lock (HttpServerUtility._machineNameLock)
			{
				if (HttpServerUtility._machineName != null)
				{
					return HttpServerUtility._machineName;
				}
				StringBuilder stringBuilder = new StringBuilder(256);
				int num = 256;
				if (UnsafeNativeMethods.GetComputerName(stringBuilder, ref num) == 0)
				{
					throw new HttpException(SR.GetString("Get_computer_name_failed"));
				}
				HttpServerUtility._machineName = stringBuilder.ToString();
			}
			return HttpServerUtility._machineName;
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x0600073E RID: 1854 RVA: 0x0001FF90 File Offset: 0x0001EF90
		// (set) Token: 0x0600073F RID: 1855 RVA: 0x0001FFCA File Offset: 0x0001EFCA
		public int ScriptTimeout
		{
			get
			{
				if (this._context != null)
				{
					return Convert.ToInt32(this._context.Timeout.TotalSeconds, CultureInfo.InvariantCulture);
				}
				return 110;
			}
			[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Medium)]
			set
			{
				if (this._context == null)
				{
					throw new HttpException(SR.GetString("Server_not_available"));
				}
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._context.Timeout = new TimeSpan(0, 0, value);
			}
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x00020006 File Offset: 0x0001F006
		public string HtmlDecode(string s)
		{
			return HttpUtility.HtmlDecode(s);
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x0002000E File Offset: 0x0001F00E
		public void HtmlDecode(string s, TextWriter output)
		{
			HttpUtility.HtmlDecode(s, output);
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x00020017 File Offset: 0x0001F017
		public string HtmlEncode(string s)
		{
			return HttpUtility.HtmlEncode(s);
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0002001F File Offset: 0x0001F01F
		public void HtmlEncode(string s, TextWriter output)
		{
			HttpUtility.HtmlEncode(s, output);
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x00020028 File Offset: 0x0001F028
		public string UrlEncode(string s)
		{
			Encoding encoding = ((this._context != null) ? this._context.Response.ContentEncoding : Encoding.UTF8);
			return HttpUtility.UrlEncode(s, encoding);
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x0002005C File Offset: 0x0001F05C
		public string UrlPathEncode(string s)
		{
			return HttpUtility.UrlPathEncode(s);
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x00020064 File Offset: 0x0001F064
		public void UrlEncode(string s, TextWriter output)
		{
			if (s != null)
			{
				output.Write(this.UrlEncode(s));
			}
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x00020078 File Offset: 0x0001F078
		public string UrlDecode(string s)
		{
			Encoding encoding = ((this._context != null) ? this._context.Request.ContentEncoding : Encoding.UTF8);
			return HttpUtility.UrlDecode(s, encoding);
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x000200AC File Offset: 0x0001F0AC
		public void UrlDecode(string s, TextWriter output)
		{
			if (s != null)
			{
				output.Write(this.UrlDecode(s));
			}
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x000200C0 File Offset: 0x0001F0C0
		public static string UrlTokenEncode(byte[] input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (input.Length < 1)
			{
				return string.Empty;
			}
			string text = Convert.ToBase64String(input);
			if (text == null)
			{
				return null;
			}
			int num = text.Length;
			while (num > 0 && text[num - 1] == '=')
			{
				num--;
			}
			char[] array = new char[num + 1];
			array[num] = (char)(48 + text.Length - num);
			for (int i = 0; i < num; i++)
			{
				char c = text[i];
				char c2 = c;
				if (c2 != '+')
				{
					if (c2 != '/')
					{
						if (c2 != '=')
						{
							array[i] = c;
						}
						else
						{
							array[i] = c;
						}
					}
					else
					{
						array[i] = '_';
					}
				}
				else
				{
					array[i] = '-';
				}
			}
			return new string(array);
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0002017C File Offset: 0x0001F17C
		public static byte[] UrlTokenDecode(string input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			int length = input.Length;
			if (length < 1)
			{
				return new byte[0];
			}
			int num = (int)(input[length - 1] - '0');
			if (num < 0 || num > 10)
			{
				return null;
			}
			char[] array = new char[length - 1 + num];
			for (int i = 0; i < length - 1; i++)
			{
				char c = input[i];
				char c2 = c;
				if (c2 != '-')
				{
					if (c2 != '_')
					{
						array[i] = c;
					}
					else
					{
						array[i] = '/';
					}
				}
				else
				{
					array[i] = '+';
				}
			}
			for (int j = length - 1; j < array.Length; j++)
			{
				array[j] = '=';
			}
			return Convert.FromBase64CharArray(array, 0, array.Length);
		}

		// Token: 0x04001149 RID: 4425
		private const int _maxMachineNameLength = 256;

		// Token: 0x0400114A RID: 4426
		private HttpContext _context;

		// Token: 0x0400114B RID: 4427
		private HttpApplication _application;

		// Token: 0x0400114C RID: 4428
		private static IDictionary _cultureCache = Hashtable.Synchronized(new Hashtable());

		// Token: 0x0400114D RID: 4429
		private static object _machineNameLock = new object();

		// Token: 0x0400114E RID: 4430
		private static string _machineName;
	}
}
