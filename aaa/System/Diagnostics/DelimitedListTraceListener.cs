using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Text;

namespace System.Diagnostics
{
	// Token: 0x020001C0 RID: 448
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
	public class DelimitedListTraceListener : TextWriterTraceListener
	{
		// Token: 0x06000DF8 RID: 3576 RVA: 0x0002C4C8 File Offset: 0x0002B4C8
		public DelimitedListTraceListener(Stream stream)
			: base(stream)
		{
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x0002C4E7 File Offset: 0x0002B4E7
		public DelimitedListTraceListener(Stream stream, string name)
			: base(stream, name)
		{
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x0002C507 File Offset: 0x0002B507
		public DelimitedListTraceListener(TextWriter writer)
			: base(writer)
		{
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x0002C526 File Offset: 0x0002B526
		public DelimitedListTraceListener(TextWriter writer, string name)
			: base(writer, name)
		{
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x0002C546 File Offset: 0x0002B546
		public DelimitedListTraceListener(string fileName)
			: base(fileName)
		{
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x0002C565 File Offset: 0x0002B565
		public DelimitedListTraceListener(string fileName, string name)
			: base(fileName, name)
		{
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000DFE RID: 3582 RVA: 0x0002C588 File Offset: 0x0002B588
		// (set) Token: 0x06000DFF RID: 3583 RVA: 0x0002C5F4 File Offset: 0x0002B5F4
		public string Delimiter
		{
			get
			{
				lock (this)
				{
					if (!this.initializedDelim)
					{
						if (base.Attributes.ContainsKey("delimiter"))
						{
							this.delimiter = base.Attributes["delimiter"];
						}
						this.initializedDelim = true;
					}
				}
				return this.delimiter;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Delimiter");
				}
				if (value.Length == 0)
				{
					throw new ArgumentException(SR.GetString("Generic_ArgCantBeEmptyString", new object[] { "Delimiter" }));
				}
				lock (this)
				{
					this.delimiter = value;
					this.initializedDelim = true;
				}
				if (this.delimiter == ",")
				{
					this.secondaryDelim = ";";
					return;
				}
				this.secondaryDelim = ",";
			}
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x0002C690 File Offset: 0x0002B690
		protected internal override string[] GetSupportedAttributes()
		{
			return new string[] { "delimiter" };
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x0002C6B0 File Offset: 0x0002B6B0
		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(eventCache, source, eventType, id, format, args))
			{
				return;
			}
			this.WriteHeader(source, eventType, id);
			if (args != null)
			{
				this.WriteEscaped(string.Format(CultureInfo.InvariantCulture, format, args));
			}
			else
			{
				this.WriteEscaped(format);
			}
			this.Write(this.Delimiter);
			this.Write(this.Delimiter);
			this.WriteFooter(eventCache);
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x0002C728 File Offset: 0x0002B728
		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(eventCache, source, eventType, id, message))
			{
				return;
			}
			this.WriteHeader(source, eventType, id);
			this.WriteEscaped(message);
			this.Write(this.Delimiter);
			this.Write(this.Delimiter);
			this.WriteFooter(eventCache);
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x0002C784 File Offset: 0x0002B784
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data))
			{
				return;
			}
			this.WriteHeader(source, eventType, id);
			this.Write(this.Delimiter);
			this.WriteEscaped(data.ToString());
			this.Write(this.Delimiter);
			this.WriteFooter(eventCache);
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x0002C7E8 File Offset: 0x0002B7E8
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, null, data))
			{
				return;
			}
			this.WriteHeader(source, eventType, id);
			this.Write(this.Delimiter);
			if (data != null)
			{
				for (int i = 0; i < data.Length; i++)
				{
					if (i != 0)
					{
						this.Write(this.secondaryDelim);
					}
					this.WriteEscaped(data[i].ToString());
				}
			}
			this.Write(this.Delimiter);
			this.WriteFooter(eventCache);
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x0002C870 File Offset: 0x0002B870
		private void WriteHeader(string source, TraceEventType eventType, int id)
		{
			this.WriteEscaped(source);
			this.Write(this.Delimiter);
			this.Write(eventType.ToString());
			this.Write(this.Delimiter);
			this.Write(id.ToString(CultureInfo.InvariantCulture));
			this.Write(this.Delimiter);
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x0002C8CC File Offset: 0x0002B8CC
		private void WriteFooter(TraceEventCache eventCache)
		{
			if (eventCache != null)
			{
				if (base.IsEnabled(TraceOptions.ProcessId))
				{
					this.Write(eventCache.ProcessId.ToString(CultureInfo.InvariantCulture));
				}
				this.Write(this.Delimiter);
				if (base.IsEnabled(TraceOptions.LogicalOperationStack))
				{
					this.WriteStackEscaped(eventCache.LogicalOperationStack);
				}
				this.Write(this.Delimiter);
				if (base.IsEnabled(TraceOptions.ThreadId))
				{
					this.WriteEscaped(eventCache.ThreadId.ToString(CultureInfo.InvariantCulture));
				}
				this.Write(this.Delimiter);
				if (base.IsEnabled(TraceOptions.DateTime))
				{
					this.WriteEscaped(eventCache.DateTime.ToString("o", CultureInfo.InvariantCulture));
				}
				this.Write(this.Delimiter);
				if (base.IsEnabled(TraceOptions.Timestamp))
				{
					this.Write(eventCache.Timestamp.ToString(CultureInfo.InvariantCulture));
				}
				this.Write(this.Delimiter);
				if (base.IsEnabled(TraceOptions.Callstack))
				{
					this.WriteEscaped(eventCache.Callstack);
				}
			}
			else
			{
				for (int i = 0; i < 5; i++)
				{
					this.Write(this.Delimiter);
				}
			}
			this.WriteLine("");
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x0002C9F8 File Offset: 0x0002B9F8
		private void WriteEscaped(string message)
		{
			if (!string.IsNullOrEmpty(message))
			{
				StringBuilder stringBuilder = new StringBuilder("\"");
				int num = 0;
				int num2;
				while ((num2 = message.IndexOf('"', num)) != -1)
				{
					stringBuilder.Append(message, num, num2 - num);
					stringBuilder.Append("\"\"");
					num = num2 + 1;
				}
				stringBuilder.Append(message, num, message.Length - num);
				stringBuilder.Append("\"");
				this.Write(stringBuilder.ToString());
			}
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x0002CA70 File Offset: 0x0002BA70
		private void WriteStackEscaped(Stack stack)
		{
			StringBuilder stringBuilder = new StringBuilder("\"");
			bool flag = true;
			foreach (object obj in stack)
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				else
				{
					flag = false;
				}
				string text = obj.ToString();
				int num = 0;
				int num2;
				while ((num2 = text.IndexOf('"', num)) != -1)
				{
					stringBuilder.Append(text, num, num2 - num);
					stringBuilder.Append("\"\"");
					num = num2 + 1;
				}
				stringBuilder.Append(text, num, text.Length - num);
			}
			stringBuilder.Append("\"");
			this.Write(stringBuilder.ToString());
		}

		// Token: 0x04000EDA RID: 3802
		private string delimiter = ";";

		// Token: 0x04000EDB RID: 3803
		private string secondaryDelim = ",";

		// Token: 0x04000EDC RID: 3804
		private bool initializedDelim;
	}
}
