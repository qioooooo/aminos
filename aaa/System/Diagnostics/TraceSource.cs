using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x020001DD RID: 477
	public class TraceSource
	{
		// Token: 0x06000F14 RID: 3860 RVA: 0x0002FEA9 File Offset: 0x0002EEA9
		public TraceSource(string name)
			: this(name, SourceLevels.Off)
		{
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x0002FEB4 File Offset: 0x0002EEB4
		public TraceSource(string name, SourceLevels defaultLevel)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException("name");
			}
			this.sourceName = name;
			this.switchLevel = defaultLevel;
			lock (TraceSource.tracesources)
			{
				TraceSource.tracesources.Add(new WeakReference(this));
			}
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x0002FF38 File Offset: 0x0002EF38
		private void Initialize()
		{
			if (!this._initCalled)
			{
				lock (this)
				{
					if (!this._initCalled)
					{
						SourceElementsCollection sources = DiagnosticsConfiguration.Sources;
						if (sources != null)
						{
							SourceElement sourceElement = sources[this.sourceName];
							if (sourceElement != null)
							{
								if (!string.IsNullOrEmpty(sourceElement.SwitchName))
								{
									this.CreateSwitch(sourceElement.SwitchType, sourceElement.SwitchName);
								}
								else
								{
									this.CreateSwitch(sourceElement.SwitchType, this.sourceName);
									if (!string.IsNullOrEmpty(sourceElement.SwitchValue))
									{
										this.internalSwitch.Level = (SourceLevels)Enum.Parse(typeof(SourceLevels), sourceElement.SwitchValue);
									}
								}
								this.listeners = sourceElement.Listeners.GetRuntimeObject();
								this.attributes = new StringDictionary();
								TraceUtils.VerifyAttributes(sourceElement.Attributes, this.GetSupportedAttributes(), this);
								this.attributes.contents = sourceElement.Attributes;
							}
							else
							{
								this.NoConfigInit();
							}
						}
						else
						{
							this.NoConfigInit();
						}
						this._initCalled = true;
					}
				}
			}
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x00030058 File Offset: 0x0002F058
		private void NoConfigInit()
		{
			this.internalSwitch = new SourceSwitch(this.sourceName, this.switchLevel.ToString());
			this.listeners = new TraceListenerCollection();
			this.listeners.Add(new DefaultTraceListener());
			this.attributes = null;
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x000300AC File Offset: 0x0002F0AC
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public void Close()
		{
			if (this.listeners != null)
			{
				lock (TraceInternal.critSec)
				{
					foreach (object obj in this.listeners)
					{
						TraceListener traceListener = (TraceListener)obj;
						traceListener.Close();
					}
				}
			}
		}

		// Token: 0x06000F19 RID: 3865 RVA: 0x00030130 File Offset: 0x0002F130
		public void Flush()
		{
			if (this.listeners != null)
			{
				if (TraceInternal.UseGlobalLock)
				{
					lock (TraceInternal.critSec)
					{
						foreach (object obj in this.listeners)
						{
							TraceListener traceListener = (TraceListener)obj;
							traceListener.Flush();
						}
						return;
					}
				}
				foreach (object obj2 in this.listeners)
				{
					TraceListener traceListener2 = (TraceListener)obj2;
					if (!traceListener2.IsThreadSafe)
					{
						lock (traceListener2)
						{
							traceListener2.Flush();
							continue;
						}
					}
					traceListener2.Flush();
				}
			}
		}

		// Token: 0x06000F1A RID: 3866 RVA: 0x0003023C File Offset: 0x0002F23C
		protected internal virtual string[] GetSupportedAttributes()
		{
			return null;
		}

		// Token: 0x06000F1B RID: 3867 RVA: 0x00030240 File Offset: 0x0002F240
		internal static void RefreshAll()
		{
			lock (TraceSource.tracesources)
			{
				for (int i = 0; i < TraceSource.tracesources.Count; i++)
				{
					TraceSource traceSource = (TraceSource)TraceSource.tracesources[i].Target;
					if (traceSource != null)
					{
						traceSource.Refresh();
					}
				}
			}
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x000302A8 File Offset: 0x0002F2A8
		internal void Refresh()
		{
			if (!this._initCalled)
			{
				this.Initialize();
				return;
			}
			SourceElementsCollection sources = DiagnosticsConfiguration.Sources;
			if (sources != null)
			{
				SourceElement sourceElement = sources[this.Name];
				if (sourceElement != null)
				{
					if ((string.IsNullOrEmpty(sourceElement.SwitchType) && this.internalSwitch.GetType() != typeof(SourceSwitch)) || sourceElement.SwitchType != this.internalSwitch.GetType().AssemblyQualifiedName)
					{
						if (!string.IsNullOrEmpty(sourceElement.SwitchName))
						{
							this.CreateSwitch(sourceElement.SwitchType, sourceElement.SwitchName);
						}
						else
						{
							this.CreateSwitch(sourceElement.SwitchType, this.Name);
							if (!string.IsNullOrEmpty(sourceElement.SwitchValue))
							{
								this.internalSwitch.Level = (SourceLevels)Enum.Parse(typeof(SourceLevels), sourceElement.SwitchValue);
							}
						}
					}
					else if (!string.IsNullOrEmpty(sourceElement.SwitchName))
					{
						if (sourceElement.SwitchName != this.internalSwitch.DisplayName)
						{
							this.CreateSwitch(sourceElement.SwitchType, sourceElement.SwitchName);
						}
						else
						{
							this.internalSwitch.Refresh();
						}
					}
					else if (!string.IsNullOrEmpty(sourceElement.SwitchValue))
					{
						this.internalSwitch.Level = (SourceLevels)Enum.Parse(typeof(SourceLevels), sourceElement.SwitchValue);
					}
					else
					{
						this.internalSwitch.Level = SourceLevels.Off;
					}
					TraceListenerCollection traceListenerCollection = new TraceListenerCollection();
					foreach (object obj in sourceElement.Listeners)
					{
						ListenerElement listenerElement = (ListenerElement)obj;
						TraceListener traceListener = this.listeners[listenerElement.Name];
						if (traceListener != null)
						{
							traceListenerCollection.Add(listenerElement.RefreshRuntimeObject(traceListener));
						}
						else
						{
							traceListenerCollection.Add(listenerElement.GetRuntimeObject());
						}
					}
					TraceUtils.VerifyAttributes(sourceElement.Attributes, this.GetSupportedAttributes(), this);
					this.attributes = new StringDictionary();
					this.attributes.contents = sourceElement.Attributes;
					this.listeners = traceListenerCollection;
					return;
				}
				this.internalSwitch.Level = this.switchLevel;
				this.listeners.Clear();
				this.attributes = null;
			}
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x000304FC File Offset: 0x0002F4FC
		[Conditional("TRACE")]
		public void TraceEvent(TraceEventType eventType, int id)
		{
			this.Initialize();
			if (this.internalSwitch.ShouldTrace(eventType) && this.listeners != null)
			{
				if (TraceInternal.UseGlobalLock)
				{
					lock (TraceInternal.critSec)
					{
						for (int i = 0; i < this.listeners.Count; i++)
						{
							TraceListener traceListener = this.listeners[i];
							traceListener.TraceEvent(this.manager, this.Name, eventType, id);
							if (Trace.AutoFlush)
							{
								traceListener.Flush();
							}
						}
						goto IL_0107;
					}
				}
				int j = 0;
				while (j < this.listeners.Count)
				{
					TraceListener traceListener2 = this.listeners[j];
					if (!traceListener2.IsThreadSafe)
					{
						lock (traceListener2)
						{
							traceListener2.TraceEvent(this.manager, this.Name, eventType, id);
							if (Trace.AutoFlush)
							{
								traceListener2.Flush();
							}
							goto IL_00F5;
						}
						goto IL_00D4;
					}
					goto IL_00D4;
					IL_00F5:
					j++;
					continue;
					IL_00D4:
					traceListener2.TraceEvent(this.manager, this.Name, eventType, id);
					if (Trace.AutoFlush)
					{
						traceListener2.Flush();
						goto IL_00F5;
					}
					goto IL_00F5;
				}
				IL_0107:
				this.manager.Clear();
			}
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x00030638 File Offset: 0x0002F638
		[Conditional("TRACE")]
		public void TraceEvent(TraceEventType eventType, int id, string message)
		{
			this.Initialize();
			if (this.internalSwitch.ShouldTrace(eventType) && this.listeners != null)
			{
				if (TraceInternal.UseGlobalLock)
				{
					lock (TraceInternal.critSec)
					{
						for (int i = 0; i < this.listeners.Count; i++)
						{
							TraceListener traceListener = this.listeners[i];
							traceListener.TraceEvent(this.manager, this.Name, eventType, id, message);
							if (Trace.AutoFlush)
							{
								traceListener.Flush();
							}
						}
						goto IL_010A;
					}
				}
				int j = 0;
				while (j < this.listeners.Count)
				{
					TraceListener traceListener2 = this.listeners[j];
					if (!traceListener2.IsThreadSafe)
					{
						lock (traceListener2)
						{
							traceListener2.TraceEvent(this.manager, this.Name, eventType, id, message);
							if (Trace.AutoFlush)
							{
								traceListener2.Flush();
							}
							goto IL_00F8;
						}
						goto IL_00D6;
					}
					goto IL_00D6;
					IL_00F8:
					j++;
					continue;
					IL_00D6:
					traceListener2.TraceEvent(this.manager, this.Name, eventType, id, message);
					if (Trace.AutoFlush)
					{
						traceListener2.Flush();
						goto IL_00F8;
					}
					goto IL_00F8;
				}
				IL_010A:
				this.manager.Clear();
			}
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x00030778 File Offset: 0x0002F778
		[Conditional("TRACE")]
		public void TraceEvent(TraceEventType eventType, int id, string format, params object[] args)
		{
			this.Initialize();
			if (this.internalSwitch.ShouldTrace(eventType) && this.listeners != null)
			{
				if (TraceInternal.UseGlobalLock)
				{
					lock (TraceInternal.critSec)
					{
						for (int i = 0; i < this.listeners.Count; i++)
						{
							TraceListener traceListener = this.listeners[i];
							traceListener.TraceEvent(this.manager, this.Name, eventType, id, format, args);
							if (Trace.AutoFlush)
							{
								traceListener.Flush();
							}
						}
						goto IL_0113;
					}
				}
				int j = 0;
				while (j < this.listeners.Count)
				{
					TraceListener traceListener2 = this.listeners[j];
					if (!traceListener2.IsThreadSafe)
					{
						lock (traceListener2)
						{
							traceListener2.TraceEvent(this.manager, this.Name, eventType, id, format, args);
							if (Trace.AutoFlush)
							{
								traceListener2.Flush();
							}
							goto IL_00FE;
						}
						goto IL_00DA;
					}
					goto IL_00DA;
					IL_00FE:
					j++;
					continue;
					IL_00DA:
					traceListener2.TraceEvent(this.manager, this.Name, eventType, id, format, args);
					if (Trace.AutoFlush)
					{
						traceListener2.Flush();
						goto IL_00FE;
					}
					goto IL_00FE;
				}
				IL_0113:
				this.manager.Clear();
			}
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x000308C0 File Offset: 0x0002F8C0
		[Conditional("TRACE")]
		public void TraceData(TraceEventType eventType, int id, object data)
		{
			this.Initialize();
			if (this.internalSwitch.ShouldTrace(eventType) && this.listeners != null)
			{
				if (TraceInternal.UseGlobalLock)
				{
					lock (TraceInternal.critSec)
					{
						for (int i = 0; i < this.listeners.Count; i++)
						{
							TraceListener traceListener = this.listeners[i];
							traceListener.TraceData(this.manager, this.Name, eventType, id, data);
							if (Trace.AutoFlush)
							{
								traceListener.Flush();
							}
						}
						goto IL_010A;
					}
				}
				int j = 0;
				while (j < this.listeners.Count)
				{
					TraceListener traceListener2 = this.listeners[j];
					if (!traceListener2.IsThreadSafe)
					{
						lock (traceListener2)
						{
							traceListener2.TraceData(this.manager, this.Name, eventType, id, data);
							if (Trace.AutoFlush)
							{
								traceListener2.Flush();
							}
							goto IL_00F8;
						}
						goto IL_00D6;
					}
					goto IL_00D6;
					IL_00F8:
					j++;
					continue;
					IL_00D6:
					traceListener2.TraceData(this.manager, this.Name, eventType, id, data);
					if (Trace.AutoFlush)
					{
						traceListener2.Flush();
						goto IL_00F8;
					}
					goto IL_00F8;
				}
				IL_010A:
				this.manager.Clear();
			}
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x00030A00 File Offset: 0x0002FA00
		[Conditional("TRACE")]
		public void TraceData(TraceEventType eventType, int id, params object[] data)
		{
			this.Initialize();
			if (this.internalSwitch.ShouldTrace(eventType) && this.listeners != null)
			{
				if (TraceInternal.UseGlobalLock)
				{
					lock (TraceInternal.critSec)
					{
						for (int i = 0; i < this.listeners.Count; i++)
						{
							TraceListener traceListener = this.listeners[i];
							traceListener.TraceData(this.manager, this.Name, eventType, id, data);
							if (Trace.AutoFlush)
							{
								traceListener.Flush();
							}
						}
						goto IL_010A;
					}
				}
				int j = 0;
				while (j < this.listeners.Count)
				{
					TraceListener traceListener2 = this.listeners[j];
					if (!traceListener2.IsThreadSafe)
					{
						lock (traceListener2)
						{
							traceListener2.TraceData(this.manager, this.Name, eventType, id, data);
							if (Trace.AutoFlush)
							{
								traceListener2.Flush();
							}
							goto IL_00F8;
						}
						goto IL_00D6;
					}
					goto IL_00D6;
					IL_00F8:
					j++;
					continue;
					IL_00D6:
					traceListener2.TraceData(this.manager, this.Name, eventType, id, data);
					if (Trace.AutoFlush)
					{
						traceListener2.Flush();
						goto IL_00F8;
					}
					goto IL_00F8;
				}
				IL_010A:
				this.manager.Clear();
			}
		}

		// Token: 0x06000F22 RID: 3874 RVA: 0x00030B40 File Offset: 0x0002FB40
		[Conditional("TRACE")]
		public void TraceInformation(string message)
		{
			this.TraceEvent(TraceEventType.Information, 0, message, null);
		}

		// Token: 0x06000F23 RID: 3875 RVA: 0x00030B4C File Offset: 0x0002FB4C
		[Conditional("TRACE")]
		public void TraceInformation(string format, params object[] args)
		{
			this.TraceEvent(TraceEventType.Information, 0, format, args);
		}

		// Token: 0x06000F24 RID: 3876 RVA: 0x00030B58 File Offset: 0x0002FB58
		[Conditional("TRACE")]
		public void TraceTransfer(int id, string message, Guid relatedActivityId)
		{
			this.Initialize();
			if (this.internalSwitch.ShouldTrace(TraceEventType.Transfer) && this.listeners != null)
			{
				if (TraceInternal.UseGlobalLock)
				{
					lock (TraceInternal.critSec)
					{
						for (int i = 0; i < this.listeners.Count; i++)
						{
							TraceListener traceListener = this.listeners[i];
							traceListener.TraceTransfer(this.manager, this.Name, id, message, relatedActivityId);
							if (Trace.AutoFlush)
							{
								traceListener.Flush();
							}
						}
						goto IL_010E;
					}
				}
				int j = 0;
				while (j < this.listeners.Count)
				{
					TraceListener traceListener2 = this.listeners[j];
					if (!traceListener2.IsThreadSafe)
					{
						lock (traceListener2)
						{
							traceListener2.TraceTransfer(this.manager, this.Name, id, message, relatedActivityId);
							if (Trace.AutoFlush)
							{
								traceListener2.Flush();
							}
							goto IL_00FC;
						}
						goto IL_00DA;
					}
					goto IL_00DA;
					IL_00FC:
					j++;
					continue;
					IL_00DA:
					traceListener2.TraceTransfer(this.manager, this.Name, id, message, relatedActivityId);
					if (Trace.AutoFlush)
					{
						traceListener2.Flush();
						goto IL_00FC;
					}
					goto IL_00FC;
				}
				IL_010E:
				this.manager.Clear();
			}
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x00030C9C File Offset: 0x0002FC9C
		private void CreateSwitch(string typename, string name)
		{
			if (!string.IsNullOrEmpty(typename))
			{
				this.internalSwitch = (SourceSwitch)TraceUtils.GetRuntimeObject(typename, typeof(SourceSwitch), name);
				return;
			}
			this.internalSwitch = new SourceSwitch(name, this.switchLevel.ToString());
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000F26 RID: 3878 RVA: 0x00030CEA File Offset: 0x0002FCEA
		public StringDictionary Attributes
		{
			get
			{
				this.Initialize();
				if (this.attributes == null)
				{
					this.attributes = new StringDictionary();
				}
				return this.attributes;
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000F27 RID: 3879 RVA: 0x00030D0B File Offset: 0x0002FD0B
		public string Name
		{
			get
			{
				return this.sourceName;
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000F28 RID: 3880 RVA: 0x00030D13 File Offset: 0x0002FD13
		public TraceListenerCollection Listeners
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				this.Initialize();
				return this.listeners;
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000F29 RID: 3881 RVA: 0x00030D21 File Offset: 0x0002FD21
		// (set) Token: 0x06000F2A RID: 3882 RVA: 0x00030D2F File Offset: 0x0002FD2F
		public SourceSwitch Switch
		{
			get
			{
				this.Initialize();
				return this.internalSwitch;
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Switch");
				}
				this.Initialize();
				this.internalSwitch = value;
			}
		}

		// Token: 0x04000F42 RID: 3906
		private static List<WeakReference> tracesources = new List<WeakReference>();

		// Token: 0x04000F43 RID: 3907
		private readonly TraceEventCache manager = new TraceEventCache();

		// Token: 0x04000F44 RID: 3908
		private SourceSwitch internalSwitch;

		// Token: 0x04000F45 RID: 3909
		private TraceListenerCollection listeners;

		// Token: 0x04000F46 RID: 3910
		private StringDictionary attributes;

		// Token: 0x04000F47 RID: 3911
		private SourceLevels switchLevel;

		// Token: 0x04000F48 RID: 3912
		private string sourceName;

		// Token: 0x04000F49 RID: 3913
		internal bool _initCalled;
	}
}
