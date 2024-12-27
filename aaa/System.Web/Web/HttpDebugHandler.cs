using System;
using System.Globalization;
using System.Text;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x02000069 RID: 105
	internal class HttpDebugHandler : IHttpHandler
	{
		// Token: 0x0600048E RID: 1166 RVA: 0x000138B4 File Offset: 0x000128B4
		internal HttpDebugHandler()
		{
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x000138BC File Offset: 0x000128BC
		public void ProcessRequest(HttpContext context)
		{
			if (!HttpRuntime.DebuggingEnabled)
			{
				context.Response.Write(SR.GetString("Debugging_forbidden", new object[] { context.Request.Path }));
				context.Response.StatusCode = 403;
			}
			else
			{
				string text = context.Request.Headers["Command"];
				if (text == null)
				{
					context.Response.Write(SR.GetString("Invalid_Debug_Request"));
					context.Response.StatusCode = 500;
				}
				else if (StringUtil.EqualsIgnoreCase(text, "stop-debug"))
				{
					context.Response.Write("OK");
				}
				else if (!StringUtil.EqualsIgnoreCase(text, "start-debug"))
				{
					context.Response.Write(SR.GetString("Invalid_Debug_Request"));
					context.Response.StatusCode = 500;
				}
				else
				{
					string serverVariable = context.WorkerRequest.GetServerVariable("AUTH_TYPE");
					string serverVariable2 = context.WorkerRequest.GetServerVariable("LOGON_USER");
					if (string.IsNullOrEmpty(serverVariable2) || string.IsNullOrEmpty(serverVariable) || StringUtil.EqualsIgnoreCase(serverVariable, "basic"))
					{
						context.Response.Write(SR.GetString("Debug_Access_Denied", new object[] { context.Request.Path }));
						context.Response.StatusCode = 401;
					}
					else
					{
						string text2 = context.Request.Form["DebugSessionID"];
						if (string.IsNullOrEmpty(text2))
						{
							context.Response.Write(SR.GetString("Invalid_Debug_ID"));
							context.Response.StatusCode = 500;
						}
						else
						{
							string text3 = text2.Replace(';', '&');
							HttpValueCollection httpValueCollection = new HttpValueCollection(text3, true, true, Encoding.UTF8);
							string text4 = httpValueCollection["autoattachclsid"];
							bool flag = false;
							if (text4 != null)
							{
								for (int i = 0; i < HttpDebugHandler.validClsIds.Length; i++)
								{
									if (StringUtil.EqualsIgnoreCase(text4, HttpDebugHandler.validClsIds[i]))
									{
										flag = true;
										break;
									}
								}
							}
							if (!flag)
							{
								context.Response.Write(SR.GetString("Debug_Access_Denied", new object[] { context.Request.Path }));
								context.Response.StatusCode = 401;
							}
							else
							{
								int num = UnsafeNativeMethods.AttachDebugger(text4, text2, context.WorkerRequest.GetUserToken());
								if (num != 0)
								{
									context.Response.Write(SR.GetString("Error_Attaching_with_MDM", new object[] { "0x" + num.ToString("X8", CultureInfo.InvariantCulture) }));
									context.Response.StatusCode = 500;
								}
								else
								{
									PerfCounters.IncrementCounter(AppPerfCounter.DEBUGGING_REQUESTS);
									context.Response.Write("OK");
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000490 RID: 1168 RVA: 0x00013B9D File Offset: 0x00012B9D
		public bool IsReusable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04001034 RID: 4148
		private static string[] validClsIds = new string[] { "{70f65411-fe8c-4248-bcff-701c8b2f4529}", "{62a78ac2-7d9a-4377-b97e-6965919fdd02}", "{cc23651f-4574-438f-b4aa-bcb28b6b3ecf}", "{dbfdb1d0-04a4-4315-b15c-f874f6b6e90b}", "{a4fcb474-2687-4924-b0ad-7caf331db826}", "{beb261f6-d5f0-43ba-baf4-8b79785fffaf}", "{8e2f5e28-d4e2-44c0-aa02-f8c5beb70cac}", "{08100915-0f41-4ccf-9564-ebaa5d49446c}" };
	}
}
