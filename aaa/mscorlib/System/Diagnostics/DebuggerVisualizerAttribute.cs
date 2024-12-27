using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
	// Token: 0x020002A9 RID: 681
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
	[ComVisible(true)]
	public sealed class DebuggerVisualizerAttribute : Attribute
	{
		// Token: 0x06001AE3 RID: 6883 RVA: 0x00046C59 File Offset: 0x00045C59
		public DebuggerVisualizerAttribute(string visualizerTypeName)
		{
			this.visualizerName = visualizerTypeName;
		}

		// Token: 0x06001AE4 RID: 6884 RVA: 0x00046C68 File Offset: 0x00045C68
		public DebuggerVisualizerAttribute(string visualizerTypeName, string visualizerObjectSourceTypeName)
		{
			this.visualizerName = visualizerTypeName;
			this.visualizerObjectSourceName = visualizerObjectSourceTypeName;
		}

		// Token: 0x06001AE5 RID: 6885 RVA: 0x00046C7E File Offset: 0x00045C7E
		public DebuggerVisualizerAttribute(string visualizerTypeName, Type visualizerObjectSource)
		{
			if (visualizerObjectSource == null)
			{
				throw new ArgumentNullException("visualizerObjectSource");
			}
			this.visualizerName = visualizerTypeName;
			this.visualizerObjectSourceName = visualizerObjectSource.AssemblyQualifiedName;
		}

		// Token: 0x06001AE6 RID: 6886 RVA: 0x00046CA7 File Offset: 0x00045CA7
		public DebuggerVisualizerAttribute(Type visualizer)
		{
			if (visualizer == null)
			{
				throw new ArgumentNullException("visualizer");
			}
			this.visualizerName = visualizer.AssemblyQualifiedName;
		}

		// Token: 0x06001AE7 RID: 6887 RVA: 0x00046CC9 File Offset: 0x00045CC9
		public DebuggerVisualizerAttribute(Type visualizer, Type visualizerObjectSource)
		{
			if (visualizer == null)
			{
				throw new ArgumentNullException("visualizer");
			}
			if (visualizerObjectSource == null)
			{
				throw new ArgumentNullException("visualizerObjectSource");
			}
			this.visualizerName = visualizer.AssemblyQualifiedName;
			this.visualizerObjectSourceName = visualizerObjectSource.AssemblyQualifiedName;
		}

		// Token: 0x06001AE8 RID: 6888 RVA: 0x00046D05 File Offset: 0x00045D05
		public DebuggerVisualizerAttribute(Type visualizer, string visualizerObjectSourceTypeName)
		{
			if (visualizer == null)
			{
				throw new ArgumentNullException("visualizer");
			}
			this.visualizerName = visualizer.AssemblyQualifiedName;
			this.visualizerObjectSourceName = visualizerObjectSourceTypeName;
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06001AE9 RID: 6889 RVA: 0x00046D2E File Offset: 0x00045D2E
		public string VisualizerObjectSourceTypeName
		{
			get
			{
				return this.visualizerObjectSourceName;
			}
		}

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06001AEA RID: 6890 RVA: 0x00046D36 File Offset: 0x00045D36
		public string VisualizerTypeName
		{
			get
			{
				return this.visualizerName;
			}
		}

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06001AEB RID: 6891 RVA: 0x00046D3E File Offset: 0x00045D3E
		// (set) Token: 0x06001AEC RID: 6892 RVA: 0x00046D46 File Offset: 0x00045D46
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06001AEE RID: 6894 RVA: 0x00046D72 File Offset: 0x00045D72
		// (set) Token: 0x06001AED RID: 6893 RVA: 0x00046D4F File Offset: 0x00045D4F
		public Type Target
		{
			get
			{
				return this.target;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.targetName = value.AssemblyQualifiedName;
				this.target = value;
			}
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06001AF0 RID: 6896 RVA: 0x00046D83 File Offset: 0x00045D83
		// (set) Token: 0x06001AEF RID: 6895 RVA: 0x00046D7A File Offset: 0x00045D7A
		public string TargetTypeName
		{
			get
			{
				return this.targetName;
			}
			set
			{
				this.targetName = value;
			}
		}

		// Token: 0x04000A16 RID: 2582
		private string visualizerObjectSourceName;

		// Token: 0x04000A17 RID: 2583
		private string visualizerName;

		// Token: 0x04000A18 RID: 2584
		private string description;

		// Token: 0x04000A19 RID: 2585
		private string targetName;

		// Token: 0x04000A1A RID: 2586
		private Type target;
	}
}
