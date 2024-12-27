using System;

namespace System.Windows.Forms
{
	// Token: 0x020001D9 RID: 473
	[Flags]
	public enum AccessibleStates
	{
		// Token: 0x04000FF9 RID: 4089
		None = 0,
		// Token: 0x04000FFA RID: 4090
		Unavailable = 1,
		// Token: 0x04000FFB RID: 4091
		Selected = 2,
		// Token: 0x04000FFC RID: 4092
		Focused = 4,
		// Token: 0x04000FFD RID: 4093
		Pressed = 8,
		// Token: 0x04000FFE RID: 4094
		Checked = 16,
		// Token: 0x04000FFF RID: 4095
		Mixed = 32,
		// Token: 0x04001000 RID: 4096
		Indeterminate = 32,
		// Token: 0x04001001 RID: 4097
		ReadOnly = 64,
		// Token: 0x04001002 RID: 4098
		HotTracked = 128,
		// Token: 0x04001003 RID: 4099
		Default = 256,
		// Token: 0x04001004 RID: 4100
		Expanded = 512,
		// Token: 0x04001005 RID: 4101
		Collapsed = 1024,
		// Token: 0x04001006 RID: 4102
		Busy = 2048,
		// Token: 0x04001007 RID: 4103
		Floating = 4096,
		// Token: 0x04001008 RID: 4104
		Marqueed = 8192,
		// Token: 0x04001009 RID: 4105
		Animated = 16384,
		// Token: 0x0400100A RID: 4106
		Invisible = 32768,
		// Token: 0x0400100B RID: 4107
		Offscreen = 65536,
		// Token: 0x0400100C RID: 4108
		Sizeable = 131072,
		// Token: 0x0400100D RID: 4109
		Moveable = 262144,
		// Token: 0x0400100E RID: 4110
		SelfVoicing = 524288,
		// Token: 0x0400100F RID: 4111
		Focusable = 1048576,
		// Token: 0x04001010 RID: 4112
		Selectable = 2097152,
		// Token: 0x04001011 RID: 4113
		Linked = 4194304,
		// Token: 0x04001012 RID: 4114
		Traversed = 8388608,
		// Token: 0x04001013 RID: 4115
		MultiSelectable = 16777216,
		// Token: 0x04001014 RID: 4116
		ExtSelectable = 33554432,
		// Token: 0x04001015 RID: 4117
		AlertLow = 67108864,
		// Token: 0x04001016 RID: 4118
		AlertMedium = 134217728,
		// Token: 0x04001017 RID: 4119
		AlertHigh = 268435456,
		// Token: 0x04001018 RID: 4120
		Protected = 536870912,
		// Token: 0x04001019 RID: 4121
		HasPopup = 1073741824,
		// Token: 0x0400101A RID: 4122
		[Obsolete("This enumeration value has been deprecated. There is no replacement. http://go.microsoft.com/fwlink/?linkid=14202")]
		Valid = 1073741823
	}
}
