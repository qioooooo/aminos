using System;
using System.Collections;

namespace System.Diagnostics
{
	// Token: 0x020001DA RID: 474
	public class TraceListenerCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x06000EEF RID: 3823 RVA: 0x0002F938 File Offset: 0x0002E938
		internal TraceListenerCollection()
		{
			this.list = new ArrayList(1);
		}

		// Token: 0x17000311 RID: 785
		public TraceListener this[int i]
		{
			get
			{
				return (TraceListener)this.list[i];
			}
			set
			{
				this.InitializeListener(value);
				this.list[i] = value;
			}
		}

		// Token: 0x17000312 RID: 786
		public TraceListener this[string name]
		{
			get
			{
				foreach (object obj in this)
				{
					TraceListener traceListener = (TraceListener)obj;
					if (traceListener.Name == name)
					{
						return traceListener;
					}
				}
				return null;
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000EF3 RID: 3827 RVA: 0x0002F9DC File Offset: 0x0002E9DC
		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x0002F9EC File Offset: 0x0002E9EC
		public int Add(TraceListener listener)
		{
			this.InitializeListener(listener);
			int num;
			lock (TraceInternal.critSec)
			{
				num = this.list.Add(listener);
			}
			return num;
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x0002FA34 File Offset: 0x0002EA34
		public void AddRange(TraceListener[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			for (int i = 0; i < value.Length; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x0002FA68 File Offset: 0x0002EA68
		public void AddRange(TraceListenerCollection value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			int count = value.Count;
			for (int i = 0; i < count; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x0002FAA4 File Offset: 0x0002EAA4
		public void Clear()
		{
			this.list = new ArrayList();
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x0002FAB1 File Offset: 0x0002EAB1
		public bool Contains(TraceListener listener)
		{
			return ((IList)this).Contains(listener);
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x0002FABA File Offset: 0x0002EABA
		public void CopyTo(TraceListener[] listeners, int index)
		{
			((ICollection)this).CopyTo(listeners, index);
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x0002FAC4 File Offset: 0x0002EAC4
		public IEnumerator GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x0002FAD1 File Offset: 0x0002EAD1
		internal void InitializeListener(TraceListener listener)
		{
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			listener.IndentSize = TraceInternal.IndentSize;
			listener.IndentLevel = TraceInternal.IndentLevel;
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x0002FAF7 File Offset: 0x0002EAF7
		public int IndexOf(TraceListener listener)
		{
			return ((IList)this).IndexOf(listener);
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x0002FB00 File Offset: 0x0002EB00
		public void Insert(int index, TraceListener listener)
		{
			this.InitializeListener(listener);
			lock (TraceInternal.critSec)
			{
				this.list.Insert(index, listener);
			}
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x0002FB48 File Offset: 0x0002EB48
		public void Remove(TraceListener listener)
		{
			((IList)this).Remove(listener);
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x0002FB54 File Offset: 0x0002EB54
		public void Remove(string name)
		{
			TraceListener traceListener = this[name];
			if (traceListener != null)
			{
				((IList)this).Remove(traceListener);
			}
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x0002FB74 File Offset: 0x0002EB74
		public void RemoveAt(int index)
		{
			lock (TraceInternal.critSec)
			{
				this.list.RemoveAt(index);
			}
		}

		// Token: 0x17000314 RID: 788
		object IList.this[int index]
		{
			get
			{
				return this.list[index];
			}
			set
			{
				TraceListener traceListener = value as TraceListener;
				if (traceListener == null)
				{
					throw new ArgumentException(SR.GetString("MustAddListener"), "value");
				}
				this.InitializeListener(traceListener);
				this.list[index] = traceListener;
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000F03 RID: 3843 RVA: 0x0002FC04 File Offset: 0x0002EC04
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000F04 RID: 3844 RVA: 0x0002FC07 File Offset: 0x0002EC07
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x0002FC0C File Offset: 0x0002EC0C
		int IList.Add(object value)
		{
			TraceListener traceListener = value as TraceListener;
			if (traceListener == null)
			{
				throw new ArgumentException(SR.GetString("MustAddListener"), "value");
			}
			this.InitializeListener(traceListener);
			int num;
			lock (TraceInternal.critSec)
			{
				num = this.list.Add(value);
			}
			return num;
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x0002FC74 File Offset: 0x0002EC74
		bool IList.Contains(object value)
		{
			return this.list.Contains(value);
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x0002FC82 File Offset: 0x0002EC82
		int IList.IndexOf(object value)
		{
			return this.list.IndexOf(value);
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x0002FC90 File Offset: 0x0002EC90
		void IList.Insert(int index, object value)
		{
			TraceListener traceListener = value as TraceListener;
			if (traceListener == null)
			{
				throw new ArgumentException(SR.GetString("MustAddListener"), "value");
			}
			this.InitializeListener(traceListener);
			lock (TraceInternal.critSec)
			{
				this.list.Insert(index, value);
			}
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x0002FCF8 File Offset: 0x0002ECF8
		void IList.Remove(object value)
		{
			lock (TraceInternal.critSec)
			{
				this.list.Remove(value);
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000F0A RID: 3850 RVA: 0x0002FD38 File Offset: 0x0002ED38
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000F0B RID: 3851 RVA: 0x0002FD3B File Offset: 0x0002ED3B
		bool ICollection.IsSynchronized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x0002FD40 File Offset: 0x0002ED40
		void ICollection.CopyTo(Array array, int index)
		{
			lock (TraceInternal.critSec)
			{
				this.list.CopyTo(array, index);
			}
		}

		// Token: 0x04000F34 RID: 3892
		private ArrayList list;
	}
}
