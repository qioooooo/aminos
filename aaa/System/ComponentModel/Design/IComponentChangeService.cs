using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	// Token: 0x02000179 RID: 377
	[ComVisible(true)]
	public interface IComponentChangeService
	{
		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06000C1D RID: 3101
		// (remove) Token: 0x06000C1E RID: 3102
		event ComponentEventHandler ComponentAdded;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06000C1F RID: 3103
		// (remove) Token: 0x06000C20 RID: 3104
		event ComponentEventHandler ComponentAdding;

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06000C21 RID: 3105
		// (remove) Token: 0x06000C22 RID: 3106
		event ComponentChangedEventHandler ComponentChanged;

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x06000C23 RID: 3107
		// (remove) Token: 0x06000C24 RID: 3108
		event ComponentChangingEventHandler ComponentChanging;

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06000C25 RID: 3109
		// (remove) Token: 0x06000C26 RID: 3110
		event ComponentEventHandler ComponentRemoved;

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06000C27 RID: 3111
		// (remove) Token: 0x06000C28 RID: 3112
		event ComponentEventHandler ComponentRemoving;

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06000C29 RID: 3113
		// (remove) Token: 0x06000C2A RID: 3114
		event ComponentRenameEventHandler ComponentRename;

		// Token: 0x06000C2B RID: 3115
		void OnComponentChanged(object component, MemberDescriptor member, object oldValue, object newValue);

		// Token: 0x06000C2C RID: 3116
		void OnComponentChanging(object component, MemberDescriptor member);
	}
}
