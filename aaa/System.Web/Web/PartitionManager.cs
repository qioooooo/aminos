using System;
using System.Collections.Specialized;
using System.Threading;

namespace System.Web
{
	// Token: 0x020000B8 RID: 184
	internal class PartitionManager : IDisposable
	{
		// Token: 0x060008A9 RID: 2217 RVA: 0x00027834 File Offset: 0x00026834
		internal PartitionManager(CreatePartitionInfo createCallback)
		{
			this._createCallback = createCallback;
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x0002785C File Offset: 0x0002685C
		internal object GetPartition(IPartitionResolver partitionResolver, string id)
		{
			if (EtwTrace.IsTraceEnabled(5, 1))
			{
				EtwTrace.Trace(EtwTraceType.ETW_TYPE_SESSIONSTATE_PARTITION_START, HttpContext.Current.WorkerRequest, partitionResolver.GetType().FullName, id);
			}
			string text = null;
			string text2 = null;
			IPartitionInfo partitionInfo = null;
			object obj;
			try
			{
				try
				{
					text = partitionResolver.ResolvePartition(id);
					if (text == null)
					{
						throw new HttpException(SR.GetString("Bad_partition_resolver_connection_string", new object[] { partitionResolver.GetType().FullName }));
					}
				}
				catch (Exception ex)
				{
					text2 = ex.Message;
					throw;
				}
				try
				{
					this._lock.AcquireReaderLock(-1);
					partitionInfo = (IPartitionInfo)this._partitions[text];
					if (partitionInfo != null)
					{
						return partitionInfo;
					}
				}
				finally
				{
					if (this._lock.IsReaderLockHeld)
					{
						this._lock.ReleaseReaderLock();
					}
				}
				try
				{
					this._lock.AcquireWriterLock(-1);
					partitionInfo = (IPartitionInfo)this._partitions[text];
					if (partitionInfo == null)
					{
						partitionInfo = this._createCallback(text);
						this._partitions.Add(text, partitionInfo);
					}
					obj = partitionInfo;
				}
				finally
				{
					if (this._lock.IsWriterLockHeld)
					{
						this._lock.ReleaseWriterLock();
					}
				}
			}
			finally
			{
				if (EtwTrace.IsTraceEnabled(5, 1))
				{
					string text3 = text2;
					if (text3 == null)
					{
						if (partitionInfo != null)
						{
							text3 = partitionInfo.GetTracingPartitionString();
						}
						else
						{
							text3 = string.Empty;
						}
					}
					EtwTrace.Trace(EtwTraceType.ETW_TYPE_SESSIONSTATE_PARTITION_END, HttpContext.Current.WorkerRequest, text3);
				}
			}
			return obj;
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x000279E4 File Offset: 0x000269E4
		public void Dispose()
		{
			if (this._partitions == null)
			{
				return;
			}
			try
			{
				this._lock.AcquireWriterLock(-1);
				if (this._partitions != null)
				{
					foreach (object obj in this._partitions.Values)
					{
						PartitionInfo partitionInfo = (PartitionInfo)obj;
						partitionInfo.Dispose();
					}
					this._partitions = null;
				}
			}
			catch
			{
			}
			finally
			{
				if (this._lock.IsWriterLockHeld)
				{
					this._lock.ReleaseWriterLock();
				}
			}
		}

		// Token: 0x040011E1 RID: 4577
		private HybridDictionary _partitions = new HybridDictionary();

		// Token: 0x040011E2 RID: 4578
		private ReaderWriterLock _lock = new ReaderWriterLock();

		// Token: 0x040011E3 RID: 4579
		private CreatePartitionInfo _createCallback;
	}
}
