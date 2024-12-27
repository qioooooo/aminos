using System;
using System.Collections.Specialized;
using System.Configuration.Internal;
using System.IO;

namespace System.Configuration
{
	// Token: 0x020000AE RID: 174
	internal class UpdateConfigHost : DelegatingConfigHost
	{
		// Token: 0x06000688 RID: 1672 RVA: 0x0001D8FE File Offset: 0x0001C8FE
		internal UpdateConfigHost(IInternalConfigHost host)
		{
			base.Host = host;
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x0001D90D File Offset: 0x0001C90D
		internal void AddStreamname(string oldStreamname, string newStreamname, bool alwaysIntercept)
		{
			if (string.IsNullOrEmpty(oldStreamname))
			{
				return;
			}
			if (!alwaysIntercept && StringUtil.EqualsIgnoreCase(oldStreamname, newStreamname))
			{
				return;
			}
			if (this._streams == null)
			{
				this._streams = new HybridDictionary(true);
			}
			this._streams[oldStreamname] = new StreamUpdate(newStreamname);
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x0001D94C File Offset: 0x0001C94C
		internal string GetNewStreamname(string oldStreamname)
		{
			StreamUpdate streamUpdate = this.GetStreamUpdate(oldStreamname, false);
			if (streamUpdate != null)
			{
				return streamUpdate.NewStreamname;
			}
			return oldStreamname;
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x0001D970 File Offset: 0x0001C970
		private StreamUpdate GetStreamUpdate(string oldStreamname, bool alwaysIntercept)
		{
			if (this._streams == null)
			{
				return null;
			}
			StreamUpdate streamUpdate = (StreamUpdate)this._streams[oldStreamname];
			if (streamUpdate != null && !alwaysIntercept && !streamUpdate.WriteCompleted)
			{
				streamUpdate = null;
			}
			return streamUpdate;
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x0001D9AC File Offset: 0x0001C9AC
		public override object GetStreamVersion(string streamName)
		{
			StreamUpdate streamUpdate = this.GetStreamUpdate(streamName, false);
			if (streamUpdate != null)
			{
				return InternalConfigHost.StaticGetStreamVersion(streamUpdate.NewStreamname);
			}
			return base.Host.GetStreamVersion(streamName);
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0001D9E0 File Offset: 0x0001C9E0
		public override Stream OpenStreamForRead(string streamName)
		{
			StreamUpdate streamUpdate = this.GetStreamUpdate(streamName, false);
			if (streamUpdate != null)
			{
				return InternalConfigHost.StaticOpenStreamForRead(streamUpdate.NewStreamname);
			}
			return base.Host.OpenStreamForRead(streamName);
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x0001DA14 File Offset: 0x0001CA14
		public override Stream OpenStreamForWrite(string streamName, string templateStreamName, ref object writeContext)
		{
			StreamUpdate streamUpdate = this.GetStreamUpdate(streamName, true);
			if (streamUpdate != null)
			{
				return InternalConfigHost.StaticOpenStreamForWrite(streamUpdate.NewStreamname, templateStreamName, ref writeContext, false);
			}
			return base.Host.OpenStreamForWrite(streamName, templateStreamName, ref writeContext);
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0001DA4C File Offset: 0x0001CA4C
		public override void WriteCompleted(string streamName, bool success, object writeContext)
		{
			StreamUpdate streamUpdate = this.GetStreamUpdate(streamName, true);
			if (streamUpdate != null)
			{
				InternalConfigHost.StaticWriteCompleted(streamUpdate.NewStreamname, success, writeContext, false);
				if (success)
				{
					streamUpdate.WriteCompleted = true;
					return;
				}
			}
			else
			{
				base.Host.WriteCompleted(streamName, success, writeContext);
			}
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0001DA8C File Offset: 0x0001CA8C
		public override bool IsConfigRecordRequired(string configPath)
		{
			return true;
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0001DA90 File Offset: 0x0001CA90
		public override void DeleteStream(string streamName)
		{
			StreamUpdate streamUpdate = this.GetStreamUpdate(streamName, false);
			if (streamUpdate != null)
			{
				InternalConfigHost.StaticDeleteStream(streamUpdate.NewStreamname);
				return;
			}
			base.Host.DeleteStream(streamName);
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x0001DAC4 File Offset: 0x0001CAC4
		public override bool IsFile(string streamName)
		{
			StreamUpdate streamUpdate = this.GetStreamUpdate(streamName, false);
			if (streamUpdate != null)
			{
				return InternalConfigHost.StaticIsFile(streamUpdate.NewStreamname);
			}
			return base.Host.IsFile(streamName);
		}

		// Token: 0x040003FD RID: 1021
		private HybridDictionary _streams;
	}
}
