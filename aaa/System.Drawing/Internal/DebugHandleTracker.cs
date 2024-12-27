using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;

namespace System.Internal
{
	// Token: 0x0200000C RID: 12
	internal class DebugHandleTracker
	{
		// Token: 0x06000021 RID: 33 RVA: 0x000026D8 File Offset: 0x000016D8
		static DebugHandleTracker()
		{
			if (CompModSwitches.HandleLeak.Level > TraceLevel.Off || CompModSwitches.TraceCollect.Enabled)
			{
				HandleCollector.HandleAdded += DebugHandleTracker.tracker.OnHandleAdd;
				HandleCollector.HandleRemoved += DebugHandleTracker.tracker.OnHandleRemove;
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002746 File Offset: 0x00001746
		private DebugHandleTracker()
		{
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002750 File Offset: 0x00001750
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

		// Token: 0x06000024 RID: 36 RVA: 0x000027D0 File Offset: 0x000017D0
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

		// Token: 0x06000025 RID: 37 RVA: 0x0000285C File Offset: 0x0000185C
		public static void Initialize()
		{
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002860 File Offset: 0x00001860
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

		// Token: 0x06000027 RID: 39 RVA: 0x0000289C File Offset: 0x0000189C
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

		// Token: 0x040000BD RID: 189
		private static Hashtable handleTypes = new Hashtable();

		// Token: 0x040000BE RID: 190
		private static DebugHandleTracker tracker = new DebugHandleTracker();

		// Token: 0x040000BF RID: 191
		private static object internalSyncObject = new object();

		// Token: 0x0200000D RID: 13
		private class HandleType
		{
			// Token: 0x06000028 RID: 40 RVA: 0x000028D7 File Offset: 0x000018D7
			public HandleType(string name)
			{
				this.name = name;
				this.buckets = new DebugHandleTracker.HandleType.HandleEntry[10];
			}

			// Token: 0x06000029 RID: 41 RVA: 0x000028F4 File Offset: 0x000018F4
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

			// Token: 0x0600002A RID: 42 RVA: 0x00002980 File Offset: 0x00001980
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

			// Token: 0x0600002B RID: 43 RVA: 0x000029E8 File Offset: 0x000019E8
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

			// Token: 0x0600002C RID: 44 RVA: 0x00002A48 File Offset: 0x00001A48
			private int ComputeHash(IntPtr handle)
			{
				return ((int)handle & 65535) % 10;
			}

			// Token: 0x0600002D RID: 45 RVA: 0x00002A5C File Offset: 0x00001A5C
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

			// Token: 0x040000C0 RID: 192
			private const int BUCKETS = 10;

			// Token: 0x040000C1 RID: 193
			public readonly string name;

			// Token: 0x040000C2 RID: 194
			private int handleCount;

			// Token: 0x040000C3 RID: 195
			private DebugHandleTracker.HandleType.HandleEntry[] buckets;

			// Token: 0x0200000E RID: 14
			private class HandleEntry
			{
				// Token: 0x0600002E RID: 46 RVA: 0x00002B0C File Offset: 0x00001B0C
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

				// Token: 0x0600002F RID: 47 RVA: 0x00002B44 File Offset: 0x00001B44
				public string ToString(DebugHandleTracker.HandleType type)
				{
					DebugHandleTracker.HandleType.HandleEntry.StackParser stackParser = new DebugHandleTracker.HandleType.HandleEntry.StackParser(this.callStack);
					stackParser.DiscardTo("HandleCollector.Add");
					stackParser.DiscardNext();
					stackParser.Truncate(40);
					string text = "";
					return Convert.ToString((int)this.handle, 16) + text + ": " + stackParser.ToString();
				}

				// Token: 0x040000C4 RID: 196
				public readonly IntPtr handle;

				// Token: 0x040000C5 RID: 197
				public DebugHandleTracker.HandleType.HandleEntry next;

				// Token: 0x040000C6 RID: 198
				public readonly string callStack;

				// Token: 0x040000C7 RID: 199
				public bool ignorableAsLeak;

				// Token: 0x0200000F RID: 15
				private class StackParser
				{
					// Token: 0x06000030 RID: 48 RVA: 0x00002B9F File Offset: 0x00001B9F
					public StackParser(string callStack)
					{
						this.releventStack = callStack;
						this.length = this.releventStack.Length;
					}

					// Token: 0x06000031 RID: 49 RVA: 0x00002BC0 File Offset: 0x00001BC0
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

					// Token: 0x06000032 RID: 50 RVA: 0x00002C0C File Offset: 0x00001C0C
					public void DiscardNext()
					{
						this.GetLine();
					}

					// Token: 0x06000033 RID: 51 RVA: 0x00002C18 File Offset: 0x00001C18
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

					// Token: 0x06000034 RID: 52 RVA: 0x00002C4C File Offset: 0x00001C4C
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

					// Token: 0x06000035 RID: 53 RVA: 0x00002D0A File Offset: 0x00001D0A
					public override string ToString()
					{
						return this.releventStack.Substring(this.startIndex);
					}

					// Token: 0x06000036 RID: 54 RVA: 0x00002D20 File Offset: 0x00001D20
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

					// Token: 0x040000C8 RID: 200
					internal string releventStack;

					// Token: 0x040000C9 RID: 201
					internal int startIndex;

					// Token: 0x040000CA RID: 202
					internal int endIndex;

					// Token: 0x040000CB RID: 203
					internal int length;
				}
			}
		}
	}
}
