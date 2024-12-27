using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
	// Token: 0x020002A3 RID: 675
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module, AllowMultiple = false)]
	[ComVisible(true)]
	public sealed class DebuggableAttribute : Attribute
	{
		// Token: 0x06001ACB RID: 6859 RVA: 0x00046AAF File Offset: 0x00045AAF
		public DebuggableAttribute(bool isJITTrackingEnabled, bool isJITOptimizerDisabled)
		{
			this.m_debuggingModes = DebuggableAttribute.DebuggingModes.None;
			if (isJITTrackingEnabled)
			{
				this.m_debuggingModes |= DebuggableAttribute.DebuggingModes.Default;
			}
			if (isJITOptimizerDisabled)
			{
				this.m_debuggingModes |= DebuggableAttribute.DebuggingModes.DisableOptimizations;
			}
		}

		// Token: 0x06001ACC RID: 6860 RVA: 0x00046AE4 File Offset: 0x00045AE4
		public DebuggableAttribute(DebuggableAttribute.DebuggingModes modes)
		{
			this.m_debuggingModes = modes;
		}

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06001ACD RID: 6861 RVA: 0x00046AF3 File Offset: 0x00045AF3
		public bool IsJITTrackingEnabled
		{
			get
			{
				return (this.m_debuggingModes & DebuggableAttribute.DebuggingModes.Default) != DebuggableAttribute.DebuggingModes.None;
			}
		}

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06001ACE RID: 6862 RVA: 0x00046B03 File Offset: 0x00045B03
		public bool IsJITOptimizerDisabled
		{
			get
			{
				return (this.m_debuggingModes & DebuggableAttribute.DebuggingModes.DisableOptimizations) != DebuggableAttribute.DebuggingModes.None;
			}
		}

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06001ACF RID: 6863 RVA: 0x00046B17 File Offset: 0x00045B17
		public DebuggableAttribute.DebuggingModes DebuggingFlags
		{
			get
			{
				return this.m_debuggingModes;
			}
		}

		// Token: 0x04000A02 RID: 2562
		private DebuggableAttribute.DebuggingModes m_debuggingModes;

		// Token: 0x020002A4 RID: 676
		[Flags]
		[ComVisible(true)]
		public enum DebuggingModes
		{
			// Token: 0x04000A04 RID: 2564
			None = 0,
			// Token: 0x04000A05 RID: 2565
			Default = 1,
			// Token: 0x04000A06 RID: 2566
			DisableOptimizations = 256,
			// Token: 0x04000A07 RID: 2567
			IgnoreSymbolStoreSequencePoints = 2,
			// Token: 0x04000A08 RID: 2568
			EnableEditAndContinue = 4
		}
	}
}
