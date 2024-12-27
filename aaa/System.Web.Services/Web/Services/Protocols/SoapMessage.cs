using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Web.Services.Diagnostics;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200005E RID: 94
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class SoapMessage
	{
		// Token: 0x06000234 RID: 564 RVA: 0x0000AF18 File Offset: 0x00009F18
		internal SoapMessage()
		{
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000AF2B File Offset: 0x00009F2B
		internal void SetParameterValues(object[] parameterValues)
		{
			this.parameterValues = parameterValues;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000AF34 File Offset: 0x00009F34
		internal object[] GetParameterValues()
		{
			return this.parameterValues;
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000237 RID: 567
		public abstract bool OneWay { get; }

		// Token: 0x06000238 RID: 568 RVA: 0x0000AF3C File Offset: 0x00009F3C
		public object GetInParameterValue(int index)
		{
			this.EnsureInStage();
			this.EnsureNoException();
			if (index < 0 || index >= this.parameterValues.Length)
			{
				throw new IndexOutOfRangeException(Res.GetString("indexMustBeBetweenAnd0Inclusive", new object[] { this.parameterValues.Length }));
			}
			return this.parameterValues[index];
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000AF94 File Offset: 0x00009F94
		public object GetOutParameterValue(int index)
		{
			this.EnsureOutStage();
			this.EnsureNoException();
			if (!this.MethodInfo.IsVoid)
			{
				if (index == 2147483647)
				{
					throw new IndexOutOfRangeException(Res.GetString("indexMustBeBetweenAnd0Inclusive", new object[] { this.parameterValues.Length }));
				}
				index++;
			}
			if (index < 0 || index >= this.parameterValues.Length)
			{
				throw new IndexOutOfRangeException(Res.GetString("indexMustBeBetweenAnd0Inclusive", new object[] { this.parameterValues.Length }));
			}
			return this.parameterValues[index];
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000B02E File Offset: 0x0000A02E
		public object GetReturnValue()
		{
			this.EnsureOutStage();
			this.EnsureNoException();
			if (this.MethodInfo.IsVoid)
			{
				throw new InvalidOperationException(Res.GetString("WebNoReturnValue"));
			}
			return this.parameterValues[0];
		}

		// Token: 0x0600023B RID: 571
		protected abstract void EnsureOutStage();

		// Token: 0x0600023C RID: 572
		protected abstract void EnsureInStage();

		// Token: 0x0600023D RID: 573 RVA: 0x0000B061 File Offset: 0x0000A061
		private void EnsureNoException()
		{
			if (this.exception != null)
			{
				throw new InvalidOperationException(Res.GetString("WebCannotAccessValue"), this.exception);
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600023E RID: 574 RVA: 0x0000B081 File Offset: 0x0000A081
		// (set) Token: 0x0600023F RID: 575 RVA: 0x0000B089 File Offset: 0x0000A089
		public SoapException Exception
		{
			get
			{
				return this.exception;
			}
			set
			{
				this.exception = value;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000240 RID: 576
		public abstract LogicalMethodInfo MethodInfo { get; }

		// Token: 0x06000241 RID: 577 RVA: 0x0000B094 File Offset: 0x0000A094
		protected void EnsureStage(SoapMessageStage stage)
		{
			if ((this.stage & stage) == (SoapMessageStage)0)
			{
				throw new InvalidOperationException(Res.GetString("WebCannotAccessValueStage", new object[] { this.stage.ToString() }));
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000242 RID: 578 RVA: 0x0000B0D6 File Offset: 0x0000A0D6
		public SoapHeaderCollection Headers
		{
			get
			{
				return this.headers;
			}
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000B0DE File Offset: 0x0000A0DE
		internal void SetStream(Stream stream)
		{
			if (this.extensionStream != null)
			{
				this.extensionStream.SetInnerStream(stream);
				this.extensionStream.SetStreamReady();
				this.extensionStream = null;
				return;
			}
			this.stream = stream;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000B10E File Offset: 0x0000A10E
		internal void SetExtensionStream(SoapExtensionStream extensionStream)
		{
			this.extensionStream = extensionStream;
			this.stream = extensionStream;
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000245 RID: 581 RVA: 0x0000B11E File Offset: 0x0000A11E
		public Stream Stream
		{
			get
			{
				return this.stream;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000246 RID: 582 RVA: 0x0000B126 File Offset: 0x0000A126
		// (set) Token: 0x06000247 RID: 583 RVA: 0x0000B135 File Offset: 0x0000A135
		public string ContentType
		{
			get
			{
				this.EnsureStage((SoapMessageStage)5);
				return this.contentType;
			}
			set
			{
				this.EnsureStage((SoapMessageStage)5);
				this.contentType = value;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000248 RID: 584 RVA: 0x0000B145 File Offset: 0x0000A145
		// (set) Token: 0x06000249 RID: 585 RVA: 0x0000B154 File Offset: 0x0000A154
		public string ContentEncoding
		{
			get
			{
				this.EnsureStage((SoapMessageStage)5);
				return this.contentEncoding;
			}
			set
			{
				this.EnsureStage((SoapMessageStage)5);
				this.contentEncoding = value;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600024A RID: 586 RVA: 0x0000B164 File Offset: 0x0000A164
		public SoapMessageStage Stage
		{
			get
			{
				return this.stage;
			}
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000B16C File Offset: 0x0000A16C
		internal void SetStage(SoapMessageStage stage)
		{
			this.stage = stage;
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600024C RID: 588
		public abstract string Url { get; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600024D RID: 589
		public abstract string Action { get; }

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600024E RID: 590 RVA: 0x0000B175 File Offset: 0x0000A175
		[DefaultValue(SoapProtocolVersion.Default)]
		[ComVisible(false)]
		public virtual SoapProtocolVersion SoapVersion
		{
			get
			{
				return SoapProtocolVersion.Default;
			}
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000B178 File Offset: 0x0000A178
		internal static SoapExtension[] InitializeExtensions(SoapReflectedExtension[] reflectedExtensions, object[] extensionInitializers)
		{
			if (reflectedExtensions == null)
			{
				return null;
			}
			SoapExtension[] array = new SoapExtension[reflectedExtensions.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = reflectedExtensions[i].CreateInstance(extensionInitializers[i]);
			}
			return array;
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000B1B0 File Offset: 0x0000A1B0
		internal void InitExtensionStreamChain(SoapExtension[] extensions)
		{
			if (extensions == null)
			{
				return;
			}
			for (int i = 0; i < extensions.Length; i++)
			{
				this.stream = extensions[i].ChainStream(this.stream);
			}
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000B1E4 File Offset: 0x0000A1E4
		internal void RunExtensions(SoapExtension[] extensions, bool throwOnException)
		{
			if (extensions == null)
			{
				return;
			}
			TraceMethod traceMethod = (Tracing.On ? new TraceMethod(this, "RunExtensions", new object[] { extensions, throwOnException }) : null);
			if ((this.stage & (SoapMessageStage)12) != (SoapMessageStage)0)
			{
				for (int i = 0; i < extensions.Length; i++)
				{
					if (Tracing.On)
					{
						Tracing.Enter("SoapExtension", traceMethod, new TraceMethod(extensions[i], "ProcessMessage", new object[] { this.stage }));
					}
					extensions[i].ProcessMessage(this);
					if (Tracing.On)
					{
						Tracing.Exit("SoapExtension", traceMethod);
					}
					if (this.Exception != null)
					{
						if (throwOnException)
						{
							throw this.Exception;
						}
						if (Tracing.On)
						{
							Tracing.ExceptionIgnore(TraceEventType.Warning, traceMethod, this.Exception);
						}
					}
				}
				return;
			}
			for (int j = extensions.Length - 1; j >= 0; j--)
			{
				if (Tracing.On)
				{
					Tracing.Enter("SoapExtension", traceMethod, new TraceMethod(extensions[j], "ProcessMessage", new object[] { this.stage }));
				}
				extensions[j].ProcessMessage(this);
				if (Tracing.On)
				{
					Tracing.Exit("SoapExtension", traceMethod);
				}
				if (this.Exception != null)
				{
					if (throwOnException)
					{
						throw this.Exception;
					}
					if (Tracing.On)
					{
						Tracing.ExceptionIgnore(TraceEventType.Warning, traceMethod, this.Exception);
					}
				}
			}
		}

		// Token: 0x040002D4 RID: 724
		private SoapMessageStage stage;

		// Token: 0x040002D5 RID: 725
		private SoapHeaderCollection headers = new SoapHeaderCollection();

		// Token: 0x040002D6 RID: 726
		private Stream stream;

		// Token: 0x040002D7 RID: 727
		private SoapExtensionStream extensionStream;

		// Token: 0x040002D8 RID: 728
		private string contentType;

		// Token: 0x040002D9 RID: 729
		private string contentEncoding;

		// Token: 0x040002DA RID: 730
		private object[] parameterValues;

		// Token: 0x040002DB RID: 731
		private SoapException exception;
	}
}
