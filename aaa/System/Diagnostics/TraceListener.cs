using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace System.Diagnostics
{
	// Token: 0x020001BA RID: 442
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
	public abstract class TraceListener : MarshalByRefObject, IDisposable
	{
		// Token: 0x06000D80 RID: 3456 RVA: 0x0002B464 File Offset: 0x0002A464
		protected TraceListener()
		{
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x0002B47A File Offset: 0x0002A47A
		protected TraceListener(string name)
		{
			this.listenerName = name;
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000D82 RID: 3458 RVA: 0x0002B497 File Offset: 0x0002A497
		public StringDictionary Attributes
		{
			get
			{
				if (this.attributes == null)
				{
					this.attributes = new StringDictionary();
				}
				return this.attributes;
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000D83 RID: 3459 RVA: 0x0002B4B2 File Offset: 0x0002A4B2
		// (set) Token: 0x06000D84 RID: 3460 RVA: 0x0002B4C8 File Offset: 0x0002A4C8
		public virtual string Name
		{
			get
			{
				if (this.listenerName != null)
				{
					return this.listenerName;
				}
				return "";
			}
			set
			{
				this.listenerName = value;
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000D85 RID: 3461 RVA: 0x0002B4D1 File Offset: 0x0002A4D1
		public virtual bool IsThreadSafe
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x0002B4D4 File Offset: 0x0002A4D4
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x0002B4E3 File Offset: 0x0002A4E3
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x0002B4E5 File Offset: 0x0002A4E5
		public virtual void Close()
		{
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x0002B4E7 File Offset: 0x0002A4E7
		public virtual void Flush()
		{
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000D8A RID: 3466 RVA: 0x0002B4E9 File Offset: 0x0002A4E9
		// (set) Token: 0x06000D8B RID: 3467 RVA: 0x0002B4F1 File Offset: 0x0002A4F1
		public int IndentLevel
		{
			get
			{
				return this.indentLevel;
			}
			set
			{
				this.indentLevel = ((value < 0) ? 0 : value);
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000D8C RID: 3468 RVA: 0x0002B501 File Offset: 0x0002A501
		// (set) Token: 0x06000D8D RID: 3469 RVA: 0x0002B509 File Offset: 0x0002A509
		public int IndentSize
		{
			get
			{
				return this.indentSize;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("IndentSize", value, SR.GetString("TraceListenerIndentSize"));
				}
				this.indentSize = value;
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000D8E RID: 3470 RVA: 0x0002B531 File Offset: 0x0002A531
		// (set) Token: 0x06000D8F RID: 3471 RVA: 0x0002B539 File Offset: 0x0002A539
		[ComVisible(false)]
		public TraceFilter Filter
		{
			get
			{
				return this.filter;
			}
			set
			{
				this.filter = value;
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000D90 RID: 3472 RVA: 0x0002B542 File Offset: 0x0002A542
		// (set) Token: 0x06000D91 RID: 3473 RVA: 0x0002B54A File Offset: 0x0002A54A
		protected bool NeedIndent
		{
			get
			{
				return this.needIndent;
			}
			set
			{
				this.needIndent = value;
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000D92 RID: 3474 RVA: 0x0002B553 File Offset: 0x0002A553
		// (set) Token: 0x06000D93 RID: 3475 RVA: 0x0002B55B File Offset: 0x0002A55B
		[ComVisible(false)]
		public TraceOptions TraceOutputOptions
		{
			get
			{
				return this.traceOptions;
			}
			set
			{
				if (value >> 6 != TraceOptions.None)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.traceOptions = value;
			}
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x0002B574 File Offset: 0x0002A574
		internal void SetAttributes(Hashtable attribs)
		{
			TraceUtils.VerifyAttributes(attribs, this.GetSupportedAttributes(), this);
			this.attributes = new StringDictionary();
			this.attributes.contents = attribs;
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x0002B59A File Offset: 0x0002A59A
		public virtual void Fail(string message)
		{
			this.Fail(message, null);
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x0002B5A4 File Offset: 0x0002A5A4
		public virtual void Fail(string message, string detailMessage)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(SR.GetString("TraceListenerFail"));
			stringBuilder.Append(" ");
			stringBuilder.Append(message);
			if (detailMessage != null)
			{
				stringBuilder.Append(" ");
				stringBuilder.Append(detailMessage);
			}
			this.WriteLine(stringBuilder.ToString());
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x0002B5FF File Offset: 0x0002A5FF
		protected internal virtual string[] GetSupportedAttributes()
		{
			return null;
		}

		// Token: 0x06000D98 RID: 3480
		public abstract void Write(string message);

		// Token: 0x06000D99 RID: 3481 RVA: 0x0002B602 File Offset: 0x0002A602
		public virtual void Write(object o)
		{
			if (this.Filter != null && !this.Filter.ShouldTrace(null, "", TraceEventType.Verbose, 0, null, null, o))
			{
				return;
			}
			if (o == null)
			{
				return;
			}
			this.Write(o.ToString());
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x0002B638 File Offset: 0x0002A638
		public virtual void Write(string message, string category)
		{
			if (this.Filter != null && !this.Filter.ShouldTrace(null, "", TraceEventType.Verbose, 0, message))
			{
				return;
			}
			if (category == null)
			{
				this.Write(message);
				return;
			}
			this.Write(category + ": " + ((message == null) ? string.Empty : message));
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x0002B68C File Offset: 0x0002A68C
		public virtual void Write(object o, string category)
		{
			if (this.Filter != null && !this.Filter.ShouldTrace(null, "", TraceEventType.Verbose, 0, category, null, o))
			{
				return;
			}
			if (category == null)
			{
				this.Write(o);
				return;
			}
			this.Write((o == null) ? "" : o.ToString(), category);
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x0002B6E0 File Offset: 0x0002A6E0
		protected virtual void WriteIndent()
		{
			this.NeedIndent = false;
			for (int i = 0; i < this.indentLevel; i++)
			{
				if (this.indentSize == 4)
				{
					this.Write("    ");
				}
				else
				{
					for (int j = 0; j < this.indentSize; j++)
					{
						this.Write(" ");
					}
				}
			}
		}

		// Token: 0x06000D9D RID: 3485
		public abstract void WriteLine(string message);

		// Token: 0x06000D9E RID: 3486 RVA: 0x0002B737 File Offset: 0x0002A737
		public virtual void WriteLine(object o)
		{
			if (this.Filter != null && !this.Filter.ShouldTrace(null, "", TraceEventType.Verbose, 0, null, null, o))
			{
				return;
			}
			this.WriteLine((o == null) ? "" : o.ToString());
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x0002B774 File Offset: 0x0002A774
		public virtual void WriteLine(string message, string category)
		{
			if (this.Filter != null && !this.Filter.ShouldTrace(null, "", TraceEventType.Verbose, 0, message))
			{
				return;
			}
			if (category == null)
			{
				this.WriteLine(message);
				return;
			}
			this.WriteLine(category + ": " + ((message == null) ? string.Empty : message));
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x0002B7C8 File Offset: 0x0002A7C8
		public virtual void WriteLine(object o, string category)
		{
			if (this.Filter != null && !this.Filter.ShouldTrace(null, "", TraceEventType.Verbose, 0, category, null, o))
			{
				return;
			}
			this.WriteLine((o == null) ? "" : o.ToString(), category);
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x0002B804 File Offset: 0x0002A804
		[ComVisible(false)]
		public virtual void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
		{
			if (this.Filter != null && !this.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data))
			{
				return;
			}
			this.WriteHeader(source, eventType, id);
			string text = string.Empty;
			if (data != null)
			{
				text = data.ToString();
			}
			this.WriteLine(text);
			this.WriteFooter(eventCache);
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x0002B85C File Offset: 0x0002A85C
		[ComVisible(false)]
		public virtual void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
		{
			if (this.Filter != null && !this.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, null, data))
			{
				return;
			}
			this.WriteHeader(source, eventType, id);
			StringBuilder stringBuilder = new StringBuilder();
			if (data != null)
			{
				for (int i = 0; i < data.Length; i++)
				{
					if (i != 0)
					{
						stringBuilder.Append(", ");
					}
					if (data[i] != null)
					{
						stringBuilder.Append(data[i].ToString());
					}
				}
			}
			this.WriteLine(stringBuilder.ToString());
			this.WriteFooter(eventCache);
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x0002B8E4 File Offset: 0x0002A8E4
		[ComVisible(false)]
		public virtual void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
		{
			this.TraceEvent(eventCache, source, eventType, id, string.Empty);
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x0002B8F6 File Offset: 0x0002A8F6
		[ComVisible(false)]
		public virtual void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
		{
			if (this.Filter != null && !this.Filter.ShouldTrace(eventCache, source, eventType, id, message))
			{
				return;
			}
			this.WriteHeader(source, eventType, id);
			this.WriteLine(message);
			this.WriteFooter(eventCache);
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x0002B930 File Offset: 0x0002A930
		[ComVisible(false)]
		public virtual void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
		{
			if (this.Filter != null && !this.Filter.ShouldTrace(eventCache, source, eventType, id, format, args))
			{
				return;
			}
			this.WriteHeader(source, eventType, id);
			if (args != null)
			{
				this.WriteLine(string.Format(CultureInfo.InvariantCulture, format, args));
			}
			else
			{
				this.WriteLine(format);
			}
			this.WriteFooter(eventCache);
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x0002B98F File Offset: 0x0002A98F
		[ComVisible(false)]
		public virtual void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
		{
			this.TraceEvent(eventCache, source, TraceEventType.Transfer, id, message + ", relatedActivityId=" + relatedActivityId.ToString());
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x0002B9B8 File Offset: 0x0002A9B8
		private void WriteHeader(string source, TraceEventType eventType, int id)
		{
			this.Write(string.Format(CultureInfo.InvariantCulture, "{0} {1}: {2} : ", new object[]
			{
				source,
				eventType.ToString(),
				id.ToString(CultureInfo.InvariantCulture)
			}));
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x0002BA04 File Offset: 0x0002AA04
		private void WriteFooter(TraceEventCache eventCache)
		{
			if (eventCache == null)
			{
				return;
			}
			this.indentLevel++;
			if (this.IsEnabled(TraceOptions.ProcessId))
			{
				this.WriteLine("ProcessId=" + eventCache.ProcessId);
			}
			if (this.IsEnabled(TraceOptions.LogicalOperationStack))
			{
				this.Write("LogicalOperationStack=");
				Stack logicalOperationStack = eventCache.LogicalOperationStack;
				bool flag = true;
				foreach (object obj in logicalOperationStack)
				{
					if (!flag)
					{
						this.Write(", ");
					}
					else
					{
						flag = false;
					}
					this.Write(obj.ToString());
				}
				this.WriteLine(string.Empty);
			}
			if (this.IsEnabled(TraceOptions.ThreadId))
			{
				this.WriteLine("ThreadId=" + eventCache.ThreadId);
			}
			if (this.IsEnabled(TraceOptions.DateTime))
			{
				this.WriteLine("DateTime=" + eventCache.DateTime.ToString("o", CultureInfo.InvariantCulture));
			}
			if (this.IsEnabled(TraceOptions.Timestamp))
			{
				this.WriteLine("Timestamp=" + eventCache.Timestamp);
			}
			if (this.IsEnabled(TraceOptions.Callstack))
			{
				this.WriteLine("Callstack=" + eventCache.Callstack);
			}
			this.indentLevel--;
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x0002BB70 File Offset: 0x0002AB70
		internal bool IsEnabled(TraceOptions opts)
		{
			return (opts & this.TraceOutputOptions) != TraceOptions.None;
		}

		// Token: 0x04000ECA RID: 3786
		private int indentLevel;

		// Token: 0x04000ECB RID: 3787
		private int indentSize = 4;

		// Token: 0x04000ECC RID: 3788
		private TraceOptions traceOptions;

		// Token: 0x04000ECD RID: 3789
		private bool needIndent = true;

		// Token: 0x04000ECE RID: 3790
		private string listenerName;

		// Token: 0x04000ECF RID: 3791
		private TraceFilter filter;

		// Token: 0x04000ED0 RID: 3792
		private StringDictionary attributes;

		// Token: 0x04000ED1 RID: 3793
		internal string initializeData;
	}
}
