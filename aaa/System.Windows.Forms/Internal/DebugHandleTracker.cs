using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;

namespace System.Internal
{
	// Token: 0x02000035 RID: 53
	internal class DebugHandleTracker
	{
		// Token: 0x06000168 RID: 360 RVA: 0x00005B54 File Offset: 0x00004B54
		static DebugHandleTracker()
		{
			if (CompModSwitches.HandleLeak.Level > TraceLevel.Off || CompModSwitches.TraceCollect.Enabled)
			{
				HandleCollector.HandleAdded += DebugHandleTracker.tracker.OnHandleAdd;
				HandleCollector.HandleRemoved += DebugHandleTracker.tracker.OnHandleRemove;
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00005BC2 File Offset: 0x00004BC2
		private DebugHandleTracker()
		{
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00005BCC File Offset: 0x00004BCC
		public static void IgnoreCurrentHandlesAsLeaks()
		{
			lock (DebugHandleTracker.internalSyncObject)
			{
				if (CompModSwitches.HandleLeak.Level >= TraceLevel.Warning)
				{
					DebugHandleTracker.HandleType[] array = new DebugHandleTracker.HandleType[DebugHandleTracker.handleTypes.Values.Count];
					DebugHandleTracker.handleTypes.Values.CopyTo(array, 0);
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i] != null)
						{
							array[i].IgnoreCurrentHandlesAsLeaks();
						}
					}
				}
			}
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00005C4C File Offset: 0x00004C4C
		public static void CheckLeaks()
		{
			lock (DebugHandleTracker.internalSyncObject)
			{
				if (CompModSwitches.HandleLeak.Level >= TraceLevel.Warning)
				{
					GC.Collect();
					GC.WaitForPendingFinalizers();
					DebugHandleTracker.HandleType[] array = new DebugHandleTracker.HandleType[DebugHandleTracker.handleTypes.Values.Count];
					DebugHandleTracker.handleTypes.Values.CopyTo(array, 0);
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i] != null)
						{
							array[i].CheckLeaks();
						}
					}
				}
			}
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00005CD8 File Offset: 0x00004CD8
		public static void Initialize()
		{
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00005CDC File Offset: 0x00004CDC
		private void OnHandleAdd(string handleName, IntPtr handle, int handleCount)
		{
			DebugHandleTracker.HandleType handleType = (DebugHandleTracker.HandleType)DebugHandleTracker.handleTypes[handleName];
			if (handleType == null)
			{
				handleType = new DebugHandleTracker.HandleType(handleName);
				DebugHandleTracker.handleTypes[handleName] = handleType;
			}
			handleType.Add(handle);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00005D18 File Offset: 0x00004D18
		private void OnHandleRemove(string handleName, IntPtr handle, int HandleCount)
		{
			DebugHandleTracker.HandleType handleType = (DebugHandleTracker.HandleType)DebugHandleTracker.handleTypes[handleName];
			bool flag = false;
			if (handleType != null)
			{
				flag = handleType.Remove(handle);
			}
			if (!flag)
			{
				TraceLevel level = CompModSwitches.HandleLeak.Level;
			}
		}

		// Token: 0x04000B32 RID: 2866
		private static Hashtable handleTypes = new Hashtable();

		// Token: 0x04000B33 RID: 2867
		private static DebugHandleTracker tracker = new DebugHandleTracker();

		// Token: 0x04000B34 RID: 2868
		private static object internalSyncObject = new object();

		// Token: 0x02000036 RID: 54
		private class HandleType
		{
			// Token: 0x0600016F RID: 367 RVA: 0x00005D53 File Offset: 0x00004D53
			public HandleType(string name)
			{
				this.name = name;
				this.buckets = new DebugHandleTracker.HandleType.HandleEntry[10];
			}

			// Token: 0x06000170 RID: 368 RVA: 0x00005D70 File Offset: 0x00004D70
			public void Add(IntPtr handle)
			{
				lock (this)
				{
					int num = this.ComputeHash(handle);
					if (CompModSwitches.HandleLeak.Level >= TraceLevel.Info)
					{
						TraceLevel level = CompModSwitches.HandleLeak.Level;
					}
					for (DebugHandleTracker.HandleType.HandleEntry handleEntry = this.buckets[num]; handleEntry != null; handleEntry = handleEntry.next)
					{
					}
					this.buckets[num] = new DebugHandleTracker.HandleType.HandleEntry(this.buckets[num], handle);
					this.handleCount++;
				}
			}

			// Token: 0x06000171 RID: 369 RVA: 0x00005DFC File Offset: 0x00004DFC
			public void CheckLeaks()
			{
				lock (this)
				{
					bool flag = false;
					if (this.handleCount > 0)
					{
						for (int i = 0; i < 10; i++)
						{
							for (DebugHandleTracker.HandleType.HandleEntry handleEntry = this.buckets[i]; handleEntry != null; handleEntry = handleEntry.next)
							{
								if (!handleEntry.ignorableAsLeak && !flag)
								{
									flag = true;
								}
							}
						}
					}
				}
			}

			// Token: 0x06000172 RID: 370 RVA: 0x00005E64 File Offset: 0x00004E64
			public void IgnoreCurrentHandlesAsLeaks()
			{
				lock (this)
				{
					if (this.handleCount > 0)
					{
						for (int i = 0; i < 10; i++)
						{
							for (DebugHandleTracker.HandleType.HandleEntry handleEntry = this.buckets[i]; handleEntry != null; handleEntry = handleEntry.next)
							{
								handleEntry.ignorableAsLeak = true;
							}
						}
					}
				}
			}

			// Token: 0x06000173 RID: 371 RVA: 0x00005EC4 File Offset: 0x00004EC4
			private int ComputeHash(IntPtr handle)
			{
				return ((int)handle & 65535) % 10;
			}

			// Token: 0x06000174 RID: 372 RVA: 0x00005ED8 File Offset: 0x00004ED8
			public bool Remove(IntPtr handle)
			{
				bool flag;
				lock (this)
				{
					int num = this.ComputeHash(handle);
					if (CompModSwitches.HandleLeak.Level >= TraceLevel.Info)
					{
						TraceLevel level = CompModSwitches.HandleLeak.Level;
					}
					DebugHandleTracker.HandleType.HandleEntry handleEntry = this.buckets[num];
					DebugHandleTracker.HandleType.HandleEntry handleEntry2 = null;
					while (handleEntry != null && handleEntry.handle != handle)
					{
						handleEntry2 = handleEntry;
						handleEntry = handleEntry.next;
					}
					if (handleEntry != null)
					{
						if (handleEntry2 == null)
						{
							this.buckets[num] = handleEntry.next;
						}
						else
						{
							handleEntry2.next = handleEntry.next;
						}
						this.handleCount--;
						flag = true;
					}
					else
					{
						flag = false;
					}
				}
				return flag;
			}

			// Token: 0x04000B35 RID: 2869
			private const int BUCKETS = 10;

			// Token: 0x04000B36 RID: 2870
			public readonly string name;

			// Token: 0x04000B37 RID: 2871
			private int handleCount;

			// Token: 0x04000B38 RID: 2872
			private DebugHandleTracker.HandleType.HandleEntry[] buckets;

			// Token: 0x02000037 RID: 55
			private class HandleEntry
			{
				// Token: 0x06000175 RID: 373 RVA: 0x00005F88 File Offset: 0x00004F88
				public HandleEntry(DebugHandleTracker.HandleType.HandleEntry next, IntPtr handle)
				{
					this.handle = handle;
					this.next = next;
					if (CompModSwitches.HandleLeak.Level > TraceLevel.Off)
					{
						this.callStack = Environment.StackTrace;
						return;
					}
					this.callStack = null;
				}

				// Token: 0x06000176 RID: 374 RVA: 0x00005FC0 File Offset: 0x00004FC0
				public string ToString(DebugHandleTracker.HandleType type)
				{
					DebugHandleTracker.HandleType.HandleEntry.StackParser stackParser = new DebugHandleTracker.HandleType.HandleEntry.StackParser(this.callStack);
					stackParser.DiscardTo("HandleCollector.Add");
					stackParser.DiscardNext();
					stackParser.Truncate(40);
					string text = "";
					return Convert.ToString((int)this.handle, 16) + text + ": " + stackParser.ToString();
				}

				// Token: 0x04000B39 RID: 2873
				public readonly IntPtr handle;

				// Token: 0x04000B3A RID: 2874
				public DebugHandleTracker.HandleType.HandleEntry next;

				// Token: 0x04000B3B RID: 2875
				public readonly string callStack;

				// Token: 0x04000B3C RID: 2876
				public bool ignorableAsLeak;

				// Token: 0x02000038 RID: 56
				private class StackParser
				{
					// Token: 0x06000177 RID: 375 RVA: 0x0000601B File Offset: 0x0000501B
					public StackParser(string callStack)
					{
						this.releventStack = callStack;
						this.length = this.releventStack.Length;
					}

					// Token: 0x06000178 RID: 376 RVA: 0x0000603C File Offset: 0x0000503C
					private static bool ContainsString(string str, string token)
					{
						int num = str.Length;
						int num2 = token.Length;
						for (int i = 0; i < num; i++)
						{
							int num3 = 0;
							while (num3 < num2 && str[i + num3] == token[num3])
							{
								num3++;
							}
							if (num3 == num2)
							{
								return true;
							}
						}
						return false;
					}

					// Token: 0x06000179 RID: 377 RVA: 0x00006088 File Offset: 0x00005088
					public void DiscardNext()
					{
						this.GetLine();
					}

					// Token: 0x0600017A RID: 378 RVA: 0x00006094 File Offset: 0x00005094
					public void DiscardTo(string discardText)
					{
						while (this.startIndex < this.length)
						{
							string line = this.GetLine();
							if (line == null)
							{
								break;
							}
							if (DebugHandleTracker.HandleType.HandleEntry.StackParser.ContainsString(line, discardText))
							{
								return;
							}
						}
					}

					// Token: 0x0600017B RID: 379 RVA: 0x000060C8 File Offset: 0x000050C8
					private string GetLine()
					{
						this.endIndex = this.releventStack.IndexOf('\r', this.startIndex);
						if (this.endIndex < 0)
						{
							this.endIndex = this.length - 1;
						}
						string text = this.releventStack.Substring(this.startIndex, this.endIndex - this.startIndex);
						char c;
						while (this.endIndex < this.length && ((c = this.releventStack[this.endIndex]) == '\r' || c == '\n'))
						{
							this.endIndex++;
						}
						if (this.startIndex == this.endIndex)
						{
							return null;
						}
						this.startIndex = this.endIndex;
						return text.Replace('\t', ' ');
					}

					// Token: 0x0600017C RID: 380 RVA: 0x00006186 File Offset: 0x00005186
					public override string ToString()
					{
						return this.releventStack.Substring(this.startIndex);
					}

					// Token: 0x0600017D RID: 381 RVA: 0x0000619C File Offset: 0x0000519C
					public void Truncate(int lines)
					{
						string text = "";
						while (lines-- > 0 && this.startIndex < this.length)
						{
							if (text == null)
							{
								text = this.GetLine();
							}
							else
							{
								text = text + ": " + this.GetLine();
							}
							text += Environment.NewLine;
						}
						this.releventStack = text;
						this.startIndex = 0;
						this.endIndex = 0;
						this.length = this.releventStack.Length;
					}

					// Token: 0x04000B3D RID: 2877
					internal string releventStack;

					// Token: 0x04000B3E RID: 2878
					internal int startIndex;

					// Token: 0x04000B3F RID: 2879
					internal int endIndex;

					// Token: 0x04000B40 RID: 2880
					internal int length;
				}
			}
		}
	}
}
