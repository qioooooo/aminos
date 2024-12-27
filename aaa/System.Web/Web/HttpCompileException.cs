using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x0200006D RID: 109
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public sealed class HttpCompileException : HttpException
	{
		// Token: 0x060004B1 RID: 1201 RVA: 0x00013E87 File Offset: 0x00012E87
		public HttpCompileException()
		{
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00013E8F File Offset: 0x00012E8F
		public HttpCompileException(string message)
			: base(message)
		{
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00013E98 File Offset: 0x00012E98
		public HttpCompileException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00013EA2 File Offset: 0x00012EA2
		public HttpCompileException(CompilerResults results, string sourceCode)
		{
			this._results = results;
			this._sourceCode = sourceCode;
			base.SetFormatter(new DynamicCompileErrorFormatter(this));
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00013EC4 File Offset: 0x00012EC4
		private HttpCompileException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this._results = (CompilerResults)info.GetValue("_results", typeof(CompilerResults));
			this._sourceCode = info.GetString("_sourceCode");
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x060004B6 RID: 1206 RVA: 0x00013EFF File Offset: 0x00012EFF
		// (set) Token: 0x060004B7 RID: 1207 RVA: 0x00013F07 File Offset: 0x00012F07
		internal bool DontCache
		{
			get
			{
				return this._dontCache;
			}
			set
			{
				this._dontCache = value;
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x060004B8 RID: 1208 RVA: 0x00013F10 File Offset: 0x00012F10
		// (set) Token: 0x060004B9 RID: 1209 RVA: 0x00013F18 File Offset: 0x00012F18
		internal ICollection VirtualPathDependencies
		{
			get
			{
				return this._virtualPathDependencies;
			}
			set
			{
				this._virtualPathDependencies = value;
			}
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x00013F21 File Offset: 0x00012F21
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("_results", this._results);
			info.AddValue("_sourceCode", this._sourceCode);
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x00013F50 File Offset: 0x00012F50
		public override string Message
		{
			get
			{
				CompilerError firstCompileError = this.FirstCompileError;
				if (firstCompileError == null)
				{
					return base.Message;
				}
				return string.Format(CultureInfo.CurrentCulture, "{0}({1}): error {2}: {3}", new object[] { firstCompileError.FileName, firstCompileError.Line, firstCompileError.ErrorNumber, firstCompileError.ErrorText });
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x060004BC RID: 1212 RVA: 0x00013FB0 File Offset: 0x00012FB0
		public CompilerResults Results
		{
			[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.High)]
			get
			{
				return this._results;
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x00013FB8 File Offset: 0x00012FB8
		public string SourceCode
		{
			[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.High)]
			get
			{
				return this._sourceCode;
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x060004BE RID: 1214 RVA: 0x00013FC0 File Offset: 0x00012FC0
		internal CompilerError FirstCompileError
		{
			get
			{
				if (this._results == null || !this._results.Errors.HasErrors)
				{
					return null;
				}
				CompilerError compilerError = null;
				foreach (object obj in this._results.Errors)
				{
					CompilerError compilerError2 = (CompilerError)obj;
					if (!compilerError2.IsWarning)
					{
						if (HttpRuntime.CodegenDirInternal != null && compilerError2.FileName != null && !StringUtil.StringStartsWith(compilerError2.FileName, HttpRuntime.CodegenDirInternal))
						{
							compilerError = compilerError2;
							break;
						}
						if (compilerError == null)
						{
							compilerError = compilerError2;
						}
					}
				}
				return compilerError;
			}
		}

		// Token: 0x04001039 RID: 4153
		private const string compileErrorFormat = "{0}({1}): error {2}: {3}";

		// Token: 0x0400103A RID: 4154
		private CompilerResults _results;

		// Token: 0x0400103B RID: 4155
		private string _sourceCode;

		// Token: 0x0400103C RID: 4156
		private bool _dontCache;

		// Token: 0x0400103D RID: 4157
		private ICollection _virtualPathDependencies;
	}
}
