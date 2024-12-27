using System;
using System.Collections.Specialized;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.Management
{
	// Token: 0x020002C3 RID: 707
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class BufferedWebEventProvider : WebEventProvider
	{
		// Token: 0x0600246B RID: 9323 RVA: 0x0009B7A4 File Offset: 0x0009A7A4
		public override void Initialize(string name, NameValueCollection config)
		{
			ProviderUtil.GetAndRemoveBooleanAttribute(config, "buffer", name, ref this._buffer);
			if (this._buffer)
			{
				ProviderUtil.GetAndRemoveRequiredNonEmptyStringAttribute(config, "bufferMode", name, ref this._bufferMode);
				this._webEventBuffer = new WebEventBuffer(this, this._bufferMode, new WebEventBufferFlushCallback(this.ProcessEventFlush));
			}
			else
			{
				ProviderUtil.GetAndRemoveStringAttribute(config, "bufferMode", name, ref this._bufferMode);
			}
			base.Initialize(name, config);
			ProviderUtil.CheckUnrecognizedAttributes(config, name);
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x0600246C RID: 9324 RVA: 0x0009B81F File Offset: 0x0009A81F
		public bool UseBuffering
		{
			get
			{
				return this._buffer;
			}
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x0600246D RID: 9325 RVA: 0x0009B827 File Offset: 0x0009A827
		public string BufferMode
		{
			get
			{
				return this._bufferMode;
			}
		}

		// Token: 0x0600246E RID: 9326 RVA: 0x0009B830 File Offset: 0x0009A830
		public override void ProcessEvent(WebBaseEvent eventRaised)
		{
			if (this._buffer)
			{
				this._webEventBuffer.AddEvent(eventRaised);
				return;
			}
			WebEventBufferFlushInfo webEventBufferFlushInfo = new WebEventBufferFlushInfo(new WebBaseEventCollection(eventRaised), EventNotificationType.Unbuffered, 0, DateTime.MinValue, 0, 0);
			this.ProcessEventFlush(webEventBufferFlushInfo);
		}

		// Token: 0x0600246F RID: 9327
		public abstract void ProcessEventFlush(WebEventBufferFlushInfo flushInfo);

		// Token: 0x06002470 RID: 9328 RVA: 0x0009B86E File Offset: 0x0009A86E
		public override void Flush()
		{
			if (this._buffer)
			{
				this._webEventBuffer.Flush(int.MaxValue, FlushCallReason.StaticFlush);
			}
		}

		// Token: 0x06002471 RID: 9329 RVA: 0x0009B889 File Offset: 0x0009A889
		public override void Shutdown()
		{
			if (this._webEventBuffer != null)
			{
				this._webEventBuffer.Shutdown();
			}
		}

		// Token: 0x04001C45 RID: 7237
		private bool _buffer = true;

		// Token: 0x04001C46 RID: 7238
		private string _bufferMode;

		// Token: 0x04001C47 RID: 7239
		private WebEventBuffer _webEventBuffer;
	}
}
