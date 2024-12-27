using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Security.Permissions;
using System.Text;
using System.Web.Configuration;
using System.Web.Handlers;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x0200047A RID: 1146
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class TraceContext
	{
		// Token: 0x060035D0 RID: 13776 RVA: 0x000E8154 File Offset: 0x000E7154
		public TraceContext(HttpContext context)
		{
			this._traceMode = TraceMode.Default;
			this._isEnabled = (DeploymentSection.RetailInternal ? TraceEnable.Disable : TraceEnable.Default);
			this._context = context;
			this._firstTime = -1L;
			this._lastTime = -1L;
			this._endDataCollected = false;
			this._traceRecords = new ArrayList();
		}

		// Token: 0x17000C0B RID: 3083
		// (get) Token: 0x060035D1 RID: 13777 RVA: 0x000E81B3 File Offset: 0x000E71B3
		// (set) Token: 0x060035D2 RID: 13778 RVA: 0x000E81CF File Offset: 0x000E71CF
		public TraceMode TraceMode
		{
			get
			{
				if (this._traceMode == TraceMode.Default)
				{
					return HttpRuntime.Profile.OutputMode;
				}
				return this._traceMode;
			}
			set
			{
				if (value < TraceMode.SortByTime || value > TraceMode.Default)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._traceMode = value;
				if (this.IsEnabled)
				{
					this.ApplyTraceMode();
				}
			}
		}

		// Token: 0x17000C0C RID: 3084
		// (get) Token: 0x060035D3 RID: 13779 RVA: 0x000E81F9 File Offset: 0x000E71F9
		// (set) Token: 0x060035D4 RID: 13780 RVA: 0x000E821A File Offset: 0x000E721A
		public bool IsEnabled
		{
			get
			{
				if (this._isEnabled == TraceEnable.Default)
				{
					return HttpRuntime.Profile.IsEnabled;
				}
				return this._isEnabled == TraceEnable.Enable;
			}
			set
			{
				if (DeploymentSection.RetailInternal)
				{
					return;
				}
				if (value)
				{
					this._isEnabled = TraceEnable.Enable;
					return;
				}
				this._isEnabled = TraceEnable.Disable;
			}
		}

		// Token: 0x17000C0D RID: 3085
		// (get) Token: 0x060035D5 RID: 13781 RVA: 0x000E8236 File Offset: 0x000E7236
		internal bool PageOutput
		{
			get
			{
				if (this._isEnabled == TraceEnable.Default)
				{
					return HttpRuntime.Profile.PageOutput;
				}
				return this._isEnabled == TraceEnable.Enable;
			}
		}

		// Token: 0x17000C0E RID: 3086
		// (set) Token: 0x060035D6 RID: 13782 RVA: 0x000E8258 File Offset: 0x000E7258
		internal int StatusCode
		{
			set
			{
				this.VerifyStart();
				DataRow dataRow = this._requestData.Tables["Trace_Request"].Rows[0];
				dataRow["Trace_Status_Code"] = value;
			}
		}

		// Token: 0x14000044 RID: 68
		// (add) Token: 0x060035D7 RID: 13783 RVA: 0x000E829D File Offset: 0x000E729D
		// (remove) Token: 0x060035D8 RID: 13784 RVA: 0x000E82B0 File Offset: 0x000E72B0
		public event TraceContextEventHandler TraceFinished
		{
			add
			{
				this._events.AddHandler(TraceContext.EventTraceFinished, value);
			}
			remove
			{
				this._events.RemoveHandler(TraceContext.EventTraceFinished, value);
			}
		}

		// Token: 0x060035D9 RID: 13785 RVA: 0x000E82C4 File Offset: 0x000E72C4
		private void ApplyTraceMode()
		{
			this.VerifyStart();
			if (this.TraceMode == TraceMode.SortByCategory)
			{
				this._requestData.Tables["Trace_Trace_Information"].DefaultView.Sort = "Trace_Category";
				return;
			}
			this._requestData.Tables["Trace_Trace_Information"].DefaultView.Sort = "Trace_From_First";
		}

		// Token: 0x060035DA RID: 13786 RVA: 0x000E8329 File Offset: 0x000E7329
		internal void CopySettingsTo(TraceContext tc)
		{
			tc._traceMode = this._traceMode;
			tc._isEnabled = this._isEnabled;
		}

		// Token: 0x060035DB RID: 13787 RVA: 0x000E8344 File Offset: 0x000E7344
		internal void OnTraceFinished(TraceContextEventArgs e)
		{
			TraceContextEventHandler traceContextEventHandler = (TraceContextEventHandler)this._events[TraceContext.EventTraceFinished];
			if (traceContextEventHandler != null)
			{
				traceContextEventHandler(this, e);
			}
		}

		// Token: 0x060035DC RID: 13788 RVA: 0x000E8372 File Offset: 0x000E7372
		internal static void SetWriteToDiagnosticsTrace(bool value)
		{
			TraceContext._writeToDiagnosticsTrace = value;
		}

		// Token: 0x060035DD RID: 13789 RVA: 0x000E837A File Offset: 0x000E737A
		public void Write(string message)
		{
			this.Write(string.Empty, message, null, false, TraceContext._writeToDiagnosticsTrace);
		}

		// Token: 0x060035DE RID: 13790 RVA: 0x000E838F File Offset: 0x000E738F
		public void Write(string category, string message)
		{
			this.Write(category, message, null, false, TraceContext._writeToDiagnosticsTrace);
		}

		// Token: 0x060035DF RID: 13791 RVA: 0x000E83A0 File Offset: 0x000E73A0
		public void Write(string category, string message, Exception errorInfo)
		{
			this.Write(category, message, errorInfo, false, TraceContext._writeToDiagnosticsTrace);
		}

		// Token: 0x060035E0 RID: 13792 RVA: 0x000E83B1 File Offset: 0x000E73B1
		internal void WriteInternal(string message, bool writeToDiagnostics)
		{
			this.Write(string.Empty, message, null, false, writeToDiagnostics);
		}

		// Token: 0x060035E1 RID: 13793 RVA: 0x000E83C2 File Offset: 0x000E73C2
		internal void WriteInternal(string category, string message, bool writeToDiagnostics)
		{
			this.Write(category, message, null, false, writeToDiagnostics);
		}

		// Token: 0x060035E2 RID: 13794 RVA: 0x000E83CF File Offset: 0x000E73CF
		public void Warn(string message)
		{
			this.Write(string.Empty, message, null, true, TraceContext._writeToDiagnosticsTrace);
		}

		// Token: 0x060035E3 RID: 13795 RVA: 0x000E83E4 File Offset: 0x000E73E4
		public void Warn(string category, string message)
		{
			this.Write(category, message, null, true, TraceContext._writeToDiagnosticsTrace);
		}

		// Token: 0x060035E4 RID: 13796 RVA: 0x000E83F5 File Offset: 0x000E73F5
		public void Warn(string category, string message, Exception errorInfo)
		{
			this.Write(category, message, errorInfo, true, TraceContext._writeToDiagnosticsTrace);
		}

		// Token: 0x060035E5 RID: 13797 RVA: 0x000E8406 File Offset: 0x000E7406
		internal void WarnInternal(string category, string message, bool writeToDiagnostics)
		{
			this.Write(category, message, null, true, writeToDiagnostics);
		}

		// Token: 0x060035E6 RID: 13798 RVA: 0x000E8414 File Offset: 0x000E7414
		private void Write(string category, string message, Exception errorInfo, bool isWarning, bool writeToDiagnostics)
		{
			lock (this)
			{
				if (!this.IsEnabled || this._writing || this._endDataCollected)
				{
					return;
				}
				this.VerifyStart();
				if (category == null)
				{
					category = string.Empty;
				}
				if (message == null)
				{
					message = string.Empty;
				}
				long value = Counter.Value;
				DataRow dataRow = this.NewRow(this._requestData, "Trace_Trace_Information");
				dataRow["Trace_Category"] = category;
				dataRow["Trace_Message"] = message;
				dataRow["Trace_Warning"] = (isWarning ? "yes" : "no");
				if (errorInfo != null)
				{
					dataRow["ErrorInfoMessage"] = errorInfo.Message;
					dataRow["ErrorInfoStack"] = errorInfo.StackTrace;
				}
				if (this._firstTime != -1L)
				{
					dataRow["Trace_From_First"] = (double)(value - this._firstTime) / (double)Counter.Frequency;
				}
				else
				{
					this._firstTime = value;
				}
				if (this._lastTime != -1L)
				{
					dataRow["Trace_From_Last"] = ((double)(value - this._lastTime) / (double)Counter.Frequency).ToString("0.000000", CultureInfo.CurrentCulture);
				}
				this._lastTime = value;
				this.AddRow(this._requestData, "Trace_Trace_Information", dataRow);
				string text = message;
				if (errorInfo != null)
				{
					string text2 = errorInfo.Message;
					if (text2 == null)
					{
						text2 = string.Empty;
					}
					string text3 = errorInfo.StackTrace;
					if (text3 == null)
					{
						text3 = string.Empty;
					}
					StringBuilder stringBuilder = new StringBuilder(message.Length + text2.Length + text3.Length);
					stringBuilder.Append(message);
					stringBuilder.Append(" -- ");
					stringBuilder.Append(text2);
					stringBuilder.Append(": ");
					stringBuilder.Append(text3);
					text = stringBuilder.ToString();
				}
				if (writeToDiagnostics)
				{
					this._writing = true;
					Trace.WriteLine(text, category);
					this._writing = false;
				}
				if (this._context != null && this._context.WorkerRequest != null)
				{
					this._context.WorkerRequest.RaiseTraceEvent(isWarning ? IntegratedTraceType.TraceWarn : IntegratedTraceType.TraceWrite, text);
				}
			}
			this._traceRecords.Add(new TraceContextRecord(category, message, isWarning, errorInfo));
		}

		// Token: 0x060035E7 RID: 13799 RVA: 0x000E865C File Offset: 0x000E765C
		internal void AddNewControl(string id, string parentId, string type, int viewStateSize, int controlStateSize)
		{
			this.VerifyStart();
			DataRow dataRow = this.NewRow(this._requestData, "Trace_Control_Tree");
			if (id == null)
			{
				id = "__UnassignedID" + this._uniqueIdCounter++;
			}
			dataRow["Trace_Control_Id"] = id;
			if (parentId == null)
			{
				parentId = "__PAGE";
			}
			dataRow["Trace_Parent_Id"] = parentId;
			dataRow["Trace_Type"] = type;
			dataRow["Trace_Viewstate_Size"] = viewStateSize;
			dataRow["Trace_Controlstate_Size"] = controlStateSize;
			dataRow["Trace_Render_Size"] = 0;
			try
			{
				this.AddRow(this._requestData, "Trace_Control_Tree", dataRow);
			}
			catch (ConstraintException)
			{
				throw new HttpException(SR.GetString("Duplicate_id_used", new object[] { id, "Trace" }));
			}
		}

		// Token: 0x060035E8 RID: 13800 RVA: 0x000E8754 File Offset: 0x000E7754
		internal void AddControlSize(string controlId, int renderSize)
		{
			this.VerifyStart();
			DataTable dataTable = this._requestData.Tables["Trace_Control_Tree"];
			if (controlId == null)
			{
				controlId = "__PAGE";
			}
			DataRow dataRow = dataTable.Rows.Find(controlId);
			if (dataRow != null)
			{
				dataRow["Trace_Render_Size"] = renderSize;
			}
		}

		// Token: 0x060035E9 RID: 13801 RVA: 0x000E87A8 File Offset: 0x000E77A8
		internal void AddControlStateSize(string controlId, int viewstateSize, int controlstateSize)
		{
			this.VerifyStart();
			DataTable dataTable = this._requestData.Tables["Trace_Control_Tree"];
			if (controlId == null)
			{
				controlId = "__PAGE";
			}
			DataRow dataRow = dataTable.Rows.Find(controlId);
			if (dataRow != null)
			{
				dataRow["Trace_Viewstate_Size"] = viewstateSize;
				dataRow["Trace_Controlstate_Size"] = controlstateSize;
			}
		}

		// Token: 0x060035EA RID: 13802 RVA: 0x000E8810 File Offset: 0x000E7810
		internal void Render(HtmlTextWriter output)
		{
			if (this.PageOutput && this._requestData != null)
			{
				TraceEnable isEnabled = this._isEnabled;
				this._isEnabled = TraceEnable.Disable;
				output.Write("<div id=\"__asptrace\">\r\n");
				output.Write(TraceHandler.StyleSheet);
				output.Write("<span class=\"tracecontent\">\r\n");
				Control control = TraceHandler.CreateDetailsTable(this._requestData.Tables["Trace_Request"]);
				if (control != null)
				{
					control.RenderControl(output);
				}
				control = TraceHandler.CreateTraceTable(this._requestData.Tables["Trace_Trace_Information"]);
				if (control != null)
				{
					control.RenderControl(output);
				}
				control = TraceHandler.CreateControlTable(this._requestData.Tables["Trace_Control_Tree"]);
				if (control != null)
				{
					control.RenderControl(output);
				}
				control = TraceHandler.CreateTable(this._requestData.Tables["Trace_Session_State"], true);
				if (control != null)
				{
					control.RenderControl(output);
				}
				control = TraceHandler.CreateTable(this._requestData.Tables["Trace_Application_State"], true);
				if (control != null)
				{
					control.RenderControl(output);
				}
				control = TraceHandler.CreateTable(this._requestData.Tables["Trace_Request_Cookies_Collection"], true);
				if (control != null)
				{
					control.RenderControl(output);
				}
				control = TraceHandler.CreateTable(this._requestData.Tables["Trace_Response_Cookies_Collection"], true);
				if (control != null)
				{
					control.RenderControl(output);
				}
				control = TraceHandler.CreateTable(this._requestData.Tables["Trace_Headers_Collection"], true);
				if (control != null)
				{
					control.RenderControl(output);
				}
				control = TraceHandler.CreateTable(this._requestData.Tables["Trace_Response_Headers_Collection"], true);
				if (control != null)
				{
					control.RenderControl(output);
				}
				control = TraceHandler.CreateTable(this._requestData.Tables["Trace_Form_Collection"]);
				if (control != null)
				{
					control.RenderControl(output);
				}
				control = TraceHandler.CreateTable(this._requestData.Tables["Trace_Querystring_Collection"]);
				if (control != null)
				{
					control.RenderControl(output);
				}
				control = TraceHandler.CreateTable(this._requestData.Tables["Trace_Server_Variables"], true);
				if (control != null)
				{
					control.RenderControl(output);
				}
				output.Write("<hr width=100% size=1 color=silver>\r\n\r\n");
				output.Write(string.Concat(new string[]
				{
					SR.GetString("Error_Formatter_CLR_Build"),
					VersionInfo.ClrVersion,
					SR.GetString("Error_Formatter_ASPNET_Build"),
					VersionInfo.EngineVersion,
					"\r\n\r\n"
				}));
				output.Write("</font>\r\n\r\n");
				output.Write("</span>\r\n</div>\r\n");
				this._isEnabled = isEnabled;
			}
		}

		// Token: 0x060035EB RID: 13803 RVA: 0x000E8A92 File Offset: 0x000E7A92
		internal DataSet GetData()
		{
			return this._requestData;
		}

		// Token: 0x060035EC RID: 13804 RVA: 0x000E8A9C File Offset: 0x000E7A9C
		internal void VerifyStart()
		{
			if (TraceContext._masterRequest == null)
			{
				lock (this)
				{
					if (TraceContext._masterRequest == null)
					{
						this.InitMaster();
					}
				}
			}
			if (this._requestData == null)
			{
				this.InitRequest();
			}
		}

		// Token: 0x060035ED RID: 13805 RVA: 0x000E8AEC File Offset: 0x000E7AEC
		internal void StopTracing()
		{
			this._endDataCollected = true;
		}

		// Token: 0x060035EE RID: 13806 RVA: 0x000E8AF8 File Offset: 0x000E7AF8
		internal void EndRequest()
		{
			this.VerifyStart();
			if (this._endDataCollected)
			{
				return;
			}
			DataRow dataRow = this._requestData.Tables["Trace_Request"].Rows[0];
			dataRow["Trace_Status_Code"] = this._context.Response.StatusCode;
			dataRow["Trace_Response_Encoding"] = this._context.Response.ContentEncoding.EncodingName;
			this._context.Application.Lock();
			try
			{
				IEnumerator enumerator = this._context.Application.GetEnumerator();
				while (enumerator.MoveNext())
				{
					dataRow = this.NewRow(this._requestData, "Trace_Application_State");
					string text = (string)enumerator.Current;
					dataRow["Trace_Application_Key"] = ((text != null) ? text : "<null>");
					object obj = this._context.Application[text];
					if (obj != null)
					{
						dataRow["Trace_Type"] = obj.GetType();
						dataRow["Trace_Value"] = obj.ToString();
					}
					else
					{
						dataRow["Trace_Type"] = "<null>";
						dataRow["Trace_Value"] = "<null>";
					}
					this.AddRow(this._requestData, "Trace_Application_State", dataRow);
				}
			}
			finally
			{
				this._context.Application.UnLock();
			}
			HttpCookieCollection httpCookieCollection = new HttpCookieCollection();
			this._context.Request.FillInCookiesCollection(httpCookieCollection, false);
			HttpCookie[] array = new HttpCookie[httpCookieCollection.Count];
			httpCookieCollection.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				dataRow = this.NewRow(this._requestData, "Trace_Request_Cookies_Collection");
				dataRow["Trace_Name"] = array[i].Name;
				if (array[i].Values.HasKeys())
				{
					NameValueCollection values = array[i].Values;
					StringBuilder stringBuilder = new StringBuilder();
					foreach (object obj2 in values)
					{
						string text = (string)obj2;
						stringBuilder.Append("(");
						stringBuilder.Append(text + "=");
						stringBuilder.Append(array[i][text] + ")  ");
					}
					dataRow["Trace_Value"] = stringBuilder.ToString();
				}
				else
				{
					dataRow["Trace_Value"] = array[i].Value;
				}
				int num = ((array[i].Name == null) ? 0 : array[i].Name.Length);
				num += ((array[i].Value == null) ? 0 : array[i].Value.Length);
				dataRow["Trace_Size"] = num + 1;
				this.AddRow(this._requestData, "Trace_Request_Cookies_Collection", dataRow);
			}
			array = new HttpCookie[this._context.Response.Cookies.Count];
			this._context.Response.Cookies.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				dataRow = this.NewRow(this._requestData, "Trace_Response_Cookies_Collection");
				dataRow["Trace_Name"] = array[i].Name;
				if (array[i].Values.HasKeys())
				{
					NameValueCollection values2 = array[i].Values;
					StringBuilder stringBuilder2 = new StringBuilder();
					foreach (object obj3 in values2)
					{
						string text = (string)obj3;
						stringBuilder2.Append("(");
						stringBuilder2.Append(text + "=");
						stringBuilder2.Append(array[i][text] + ")  ");
					}
					dataRow["Trace_Value"] = stringBuilder2.ToString();
				}
				else
				{
					dataRow["Trace_Value"] = array[i].Value;
				}
				int num2 = ((array[i].Name == null) ? 0 : array[i].Name.Length);
				num2 += ((array[i].Value == null) ? 0 : array[i].Value.Length);
				dataRow["Trace_Size"] = num2 + 1;
				this.AddRow(this._requestData, "Trace_Response_Cookies_Collection", dataRow);
			}
			HttpSessionState session = this._context.Session;
			if (session != null)
			{
				dataRow = this._requestData.Tables["Trace_Request"].Rows[0];
				try
				{
					dataRow["Trace_Session_Id"] = HttpUtility.UrlEncode(session.SessionID);
				}
				catch
				{
				}
				IEnumerator enumerator = session.GetEnumerator();
				while (enumerator.MoveNext())
				{
					dataRow = this.NewRow(this._requestData, "Trace_Session_State");
					string text = (string)enumerator.Current;
					dataRow["Trace_Session_Key"] = ((text != null) ? text : "<null>");
					object obj = session[text];
					if (obj != null)
					{
						dataRow["Trace_Type"] = obj.GetType();
						dataRow["Trace_Value"] = obj.ToString();
					}
					else
					{
						dataRow["Trace_Type"] = "<null>";
						dataRow["Trace_Value"] = "<null>";
					}
					this.AddRow(this._requestData, "Trace_Session_State", dataRow);
				}
			}
			this.ApplyTraceMode();
			this.OnTraceFinished(new TraceContextEventArgs(this._traceRecords));
		}

		// Token: 0x060035EF RID: 13807 RVA: 0x000E9098 File Offset: 0x000E8098
		private void InitMaster()
		{
			DataSet dataSet = new DataSet();
			dataSet.Locale = CultureInfo.InvariantCulture;
			Type typeFromHandle = typeof(string);
			Type typeFromHandle2 = typeof(int);
			Type typeFromHandle3 = typeof(double);
			DataTable dataTable = dataSet.Tables.Add("Trace_Request");
			dataTable.Columns.Add("Trace_No", typeFromHandle2);
			dataTable.Columns.Add("Trace_Time_of_Request", typeFromHandle);
			dataTable.Columns.Add("Trace_Url", typeFromHandle);
			dataTable.Columns.Add("Trace_Request_Type", typeFromHandle);
			dataTable.Columns.Add("Trace_Status_Code", typeFromHandle2);
			dataTable.Columns.Add("Trace_Session_Id", typeFromHandle);
			dataTable.Columns.Add("Trace_Request_Encoding", typeFromHandle);
			dataTable.Columns.Add("Trace_Response_Encoding", typeFromHandle);
			dataTable = dataSet.Tables.Add("Trace_Control_Tree");
			dataTable.Columns.Add("Trace_Parent_Id", typeFromHandle);
			DataColumn[] array = new DataColumn[]
			{
				new DataColumn("Trace_Control_Id", typeFromHandle)
			};
			dataTable.Columns.Add(array[0]);
			dataTable.PrimaryKey = array;
			dataTable.Columns.Add("Trace_Type", typeFromHandle);
			dataTable.Columns.Add("Trace_Render_Size", typeFromHandle2);
			dataTable.Columns.Add("Trace_Viewstate_Size", typeFromHandle2);
			dataTable.Columns.Add("Trace_Controlstate_Size", typeFromHandle2);
			dataTable = dataSet.Tables.Add("Trace_Session_State");
			dataTable.Columns.Add("Trace_Session_Key", typeFromHandle);
			dataTable.Columns.Add("Trace_Type", typeFromHandle);
			dataTable.Columns.Add("Trace_Value", typeFromHandle);
			dataTable = dataSet.Tables.Add("Trace_Application_State");
			dataTable.Columns.Add("Trace_Application_Key", typeFromHandle);
			dataTable.Columns.Add("Trace_Type", typeFromHandle);
			dataTable.Columns.Add("Trace_Value", typeFromHandle);
			dataTable = dataSet.Tables.Add("Trace_Request_Cookies_Collection");
			dataTable.Columns.Add("Trace_Name", typeFromHandle);
			dataTable.Columns.Add("Trace_Value", typeFromHandle);
			dataTable.Columns.Add("Trace_Size", typeFromHandle2);
			dataTable = dataSet.Tables.Add("Trace_Response_Cookies_Collection");
			dataTable.Columns.Add("Trace_Name", typeFromHandle);
			dataTable.Columns.Add("Trace_Value", typeFromHandle);
			dataTable.Columns.Add("Trace_Size", typeFromHandle2);
			dataTable = dataSet.Tables.Add("Trace_Headers_Collection");
			dataTable.Columns.Add("Trace_Name", typeFromHandle);
			dataTable.Columns.Add("Trace_Value", typeFromHandle);
			dataTable = dataSet.Tables.Add("Trace_Response_Headers_Collection");
			dataTable.Columns.Add("Trace_Name", typeFromHandle);
			dataTable.Columns.Add("Trace_Value", typeFromHandle);
			dataTable = dataSet.Tables.Add("Trace_Form_Collection");
			dataTable.Columns.Add("Trace_Name", typeFromHandle);
			dataTable.Columns.Add("Trace_Value", typeFromHandle);
			dataTable = dataSet.Tables.Add("Trace_Querystring_Collection");
			dataTable.Columns.Add("Trace_Name", typeFromHandle);
			dataTable.Columns.Add("Trace_Value", typeFromHandle);
			dataTable = dataSet.Tables.Add("Trace_Trace_Information");
			dataTable.Columns.Add("Trace_Category", typeFromHandle);
			dataTable.Columns.Add("Trace_Warning", typeFromHandle);
			dataTable.Columns.Add("Trace_Message", typeFromHandle);
			dataTable.Columns.Add("Trace_From_First", typeFromHandle3);
			dataTable.Columns.Add("Trace_From_Last", typeFromHandle);
			dataTable.Columns.Add("ErrorInfoMessage", typeFromHandle);
			dataTable.Columns.Add("ErrorInfoStack", typeFromHandle);
			dataTable = dataSet.Tables.Add("Trace_Server_Variables");
			dataTable.Columns.Add("Trace_Name", typeFromHandle);
			dataTable.Columns.Add("Trace_Value", typeFromHandle);
			TraceContext._masterRequest = dataSet;
		}

		// Token: 0x060035F0 RID: 13808 RVA: 0x000E94CD File Offset: 0x000E84CD
		private DataRow NewRow(DataSet ds, string table)
		{
			return ds.Tables[table].NewRow();
		}

		// Token: 0x060035F1 RID: 13809 RVA: 0x000E94E0 File Offset: 0x000E84E0
		private void AddRow(DataSet ds, string table, DataRow row)
		{
			ds.Tables[table].Rows.Add(row);
		}

		// Token: 0x060035F2 RID: 13810 RVA: 0x000E94FC File Offset: 0x000E84FC
		private void InitRequest()
		{
			DataSet dataSet = TraceContext._masterRequest.Clone();
			DataRow dataRow = this.NewRow(dataSet, "Trace_Request");
			dataRow["Trace_Time_of_Request"] = this._context.Timestamp.ToString("G");
			string text = this._context.Request.RawUrl;
			int num = text.IndexOf("?", StringComparison.Ordinal);
			if (num != -1)
			{
				text = text.Substring(0, num);
			}
			dataRow["Trace_Url"] = text;
			dataRow["Trace_Request_Type"] = this._context.Request.HttpMethod;
			try
			{
				dataRow["Trace_Request_Encoding"] = this._context.Request.ContentEncoding.EncodingName;
			}
			catch
			{
			}
			if (this.TraceMode == TraceMode.SortByCategory)
			{
				dataSet.Tables["Trace_Trace_Information"].DefaultView.Sort = "Trace_Category";
			}
			this.AddRow(dataSet, "Trace_Request", dataRow);
			string[] array = this._context.Request.Headers.AllKeys;
			for (int i = 0; i < array.Length; i++)
			{
				dataRow = this.NewRow(dataSet, "Trace_Headers_Collection");
				dataRow["Trace_Name"] = array[i];
				dataRow["Trace_Value"] = this._context.Request.Headers[array[i]];
				this.AddRow(dataSet, "Trace_Headers_Collection", dataRow);
			}
			ArrayList arrayList = this._context.Response.GenerateResponseHeaders(false);
			int num2 = ((arrayList != null) ? arrayList.Count : 0);
			for (int i = 0; i < num2; i++)
			{
				HttpResponseHeader httpResponseHeader = (HttpResponseHeader)arrayList[i];
				dataRow = this.NewRow(dataSet, "Trace_Response_Headers_Collection");
				dataRow["Trace_Name"] = httpResponseHeader.Name;
				dataRow["Trace_Value"] = httpResponseHeader.Value;
				this.AddRow(dataSet, "Trace_Response_Headers_Collection", dataRow);
			}
			array = this._context.Request.Form.AllKeys;
			for (int i = 0; i < array.Length; i++)
			{
				dataRow = this.NewRow(dataSet, "Trace_Form_Collection");
				dataRow["Trace_Name"] = array[i];
				dataRow["Trace_Value"] = this._context.Request.Form[array[i]];
				this.AddRow(dataSet, "Trace_Form_Collection", dataRow);
			}
			array = this._context.Request.QueryString.AllKeys;
			for (int i = 0; i < array.Length; i++)
			{
				dataRow = this.NewRow(dataSet, "Trace_Querystring_Collection");
				dataRow["Trace_Name"] = array[i];
				dataRow["Trace_Value"] = this._context.Request.QueryString[array[i]];
				this.AddRow(dataSet, "Trace_Querystring_Collection", dataRow);
			}
			if (HttpRuntime.HasAppPathDiscoveryPermission())
			{
				array = this._context.Request.ServerVariables.AllKeys;
				for (int i = 0; i < array.Length; i++)
				{
					dataRow = this.NewRow(dataSet, "Trace_Server_Variables");
					dataRow["Trace_Name"] = array[i];
					dataRow["Trace_Value"] = this._context.Request.ServerVariables.Get(array[i]);
					this.AddRow(dataSet, "Trace_Server_Variables", dataRow);
				}
			}
			this._requestData = dataSet;
		}

		// Token: 0x0400255C RID: 9564
		private const string PAGEKEYNAME = "__PAGE";

		// Token: 0x0400255D RID: 9565
		private const string NULLSTRING = "<null>";

		// Token: 0x0400255E RID: 9566
		private const string NULLIDPREFIX = "__UnassignedID";

		// Token: 0x0400255F RID: 9567
		private static DataSet _masterRequest;

		// Token: 0x04002560 RID: 9568
		private static bool _writeToDiagnosticsTrace = false;

		// Token: 0x04002561 RID: 9569
		private static readonly object EventTraceFinished = new object();

		// Token: 0x04002562 RID: 9570
		private EventHandlerList _events = new EventHandlerList();

		// Token: 0x04002563 RID: 9571
		private TraceMode _traceMode;

		// Token: 0x04002564 RID: 9572
		private TraceEnable _isEnabled;

		// Token: 0x04002565 RID: 9573
		private HttpContext _context;

		// Token: 0x04002566 RID: 9574
		private DataSet _requestData;

		// Token: 0x04002567 RID: 9575
		private long _firstTime;

		// Token: 0x04002568 RID: 9576
		private long _lastTime;

		// Token: 0x04002569 RID: 9577
		private int _uniqueIdCounter;

		// Token: 0x0400256A RID: 9578
		private ArrayList _traceRecords;

		// Token: 0x0400256B RID: 9579
		private bool _endDataCollected;

		// Token: 0x0400256C RID: 9580
		private bool _writing;
	}
}
