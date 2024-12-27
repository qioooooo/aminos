using System;
using System.Collections;

namespace System.Management
{
	// Token: 0x02000027 RID: 39
	public class ManagementOperationObserver
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000134 RID: 308 RVA: 0x00007E18 File Offset: 0x00006E18
		// (remove) Token: 0x06000135 RID: 309 RVA: 0x00007E31 File Offset: 0x00006E31
		public event ObjectReadyEventHandler ObjectReady;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000136 RID: 310 RVA: 0x00007E4A File Offset: 0x00006E4A
		// (remove) Token: 0x06000137 RID: 311 RVA: 0x00007E63 File Offset: 0x00006E63
		public event CompletedEventHandler Completed;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000138 RID: 312 RVA: 0x00007E7C File Offset: 0x00006E7C
		// (remove) Token: 0x06000139 RID: 313 RVA: 0x00007E95 File Offset: 0x00006E95
		public event ProgressEventHandler Progress;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600013A RID: 314 RVA: 0x00007EAE File Offset: 0x00006EAE
		// (remove) Token: 0x0600013B RID: 315 RVA: 0x00007EC7 File Offset: 0x00006EC7
		public event ObjectPutEventHandler ObjectPut;

		// Token: 0x0600013C RID: 316 RVA: 0x00007EE0 File Offset: 0x00006EE0
		public ManagementOperationObserver()
		{
			this.m_sinkCollection = new Hashtable();
			this.delegateInvoker = new WmiDelegateInvoker(this);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00007F00 File Offset: 0x00006F00
		public void Cancel()
		{
			Hashtable hashtable = new Hashtable();
			lock (this.m_sinkCollection)
			{
				IDictionaryEnumerator enumerator = this.m_sinkCollection.GetEnumerator();
				try
				{
					enumerator.Reset();
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
						hashtable.Add(dictionaryEntry.Key, dictionaryEntry.Value);
					}
				}
				catch
				{
				}
			}
			try
			{
				IDictionaryEnumerator enumerator2 = hashtable.GetEnumerator();
				enumerator2.Reset();
				while (enumerator2.MoveNext())
				{
					object obj2 = enumerator2.Current;
					WmiEventSink wmiEventSink = (WmiEventSink)((DictionaryEntry)obj2).Value;
					try
					{
						wmiEventSink.Cancel();
					}
					catch
					{
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00007FE4 File Offset: 0x00006FE4
		internal WmiEventSink GetNewSink(ManagementScope scope, object context)
		{
			WmiEventSink wmiEventSink2;
			try
			{
				WmiEventSink wmiEventSink = WmiEventSink.GetWmiEventSink(this, context, scope, null, null);
				lock (this.m_sinkCollection)
				{
					this.m_sinkCollection.Add(wmiEventSink.GetHashCode(), wmiEventSink);
				}
				wmiEventSink2 = wmiEventSink;
			}
			catch
			{
				wmiEventSink2 = null;
			}
			return wmiEventSink2;
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00008050 File Offset: 0x00007050
		internal bool HaveListenersForProgress
		{
			get
			{
				bool flag = false;
				try
				{
					if (this.Progress != null)
					{
						flag = this.Progress.GetInvocationList().Length > 0;
					}
				}
				catch
				{
				}
				return flag;
			}
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00008090 File Offset: 0x00007090
		internal WmiEventSink GetNewPutSink(ManagementScope scope, object context, string path, string className)
		{
			WmiEventSink wmiEventSink2;
			try
			{
				WmiEventSink wmiEventSink = WmiEventSink.GetWmiEventSink(this, context, scope, path, className);
				lock (this.m_sinkCollection)
				{
					this.m_sinkCollection.Add(wmiEventSink.GetHashCode(), wmiEventSink);
				}
				wmiEventSink2 = wmiEventSink;
			}
			catch
			{
				wmiEventSink2 = null;
			}
			return wmiEventSink2;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x000080FC File Offset: 0x000070FC
		internal WmiGetEventSink GetNewGetSink(ManagementScope scope, object context, ManagementObject managementObject)
		{
			WmiGetEventSink wmiGetEventSink2;
			try
			{
				WmiGetEventSink wmiGetEventSink = WmiGetEventSink.GetWmiGetEventSink(this, context, scope, managementObject);
				lock (this.m_sinkCollection)
				{
					this.m_sinkCollection.Add(wmiGetEventSink.GetHashCode(), wmiGetEventSink);
				}
				wmiGetEventSink2 = wmiGetEventSink;
			}
			catch
			{
				wmiGetEventSink2 = null;
			}
			return wmiGetEventSink2;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00008168 File Offset: 0x00007168
		internal void RemoveSink(WmiEventSink eventSink)
		{
			try
			{
				lock (this.m_sinkCollection)
				{
					this.m_sinkCollection.Remove(eventSink.GetHashCode());
				}
				eventSink.ReleaseStub();
			}
			catch
			{
			}
		}

		// Token: 0x06000143 RID: 323 RVA: 0x000081C8 File Offset: 0x000071C8
		internal void FireObjectReady(ObjectReadyEventArgs args)
		{
			try
			{
				this.delegateInvoker.FireEventToDelegates(this.ObjectReady, args);
			}
			catch
			{
			}
		}

		// Token: 0x06000144 RID: 324 RVA: 0x000081FC File Offset: 0x000071FC
		internal void FireCompleted(CompletedEventArgs args)
		{
			try
			{
				this.delegateInvoker.FireEventToDelegates(this.Completed, args);
			}
			catch
			{
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00008230 File Offset: 0x00007230
		internal void FireProgress(ProgressEventArgs args)
		{
			try
			{
				this.delegateInvoker.FireEventToDelegates(this.Progress, args);
			}
			catch
			{
			}
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00008264 File Offset: 0x00007264
		internal void FireObjectPut(ObjectPutEventArgs args)
		{
			try
			{
				this.delegateInvoker.FireEventToDelegates(this.ObjectPut, args);
			}
			catch
			{
			}
		}

		// Token: 0x0400011A RID: 282
		private Hashtable m_sinkCollection;

		// Token: 0x0400011B RID: 283
		private WmiDelegateInvoker delegateInvoker;
	}
}
