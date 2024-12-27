using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	// Token: 0x02000180 RID: 384
	[ComVisible(true)]
	public interface IDesignerHost : IServiceContainer, IServiceProvider
	{
		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000C4A RID: 3146
		bool Loading { get; }

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000C4B RID: 3147
		bool InTransaction { get; }

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000C4C RID: 3148
		IContainer Container { get; }

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000C4D RID: 3149
		IComponent RootComponent { get; }

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000C4E RID: 3150
		string RootComponentClassName { get; }

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000C4F RID: 3151
		string TransactionDescription { get; }

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x06000C50 RID: 3152
		// (remove) Token: 0x06000C51 RID: 3153
		event EventHandler Activated;

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x06000C52 RID: 3154
		// (remove) Token: 0x06000C53 RID: 3155
		event EventHandler Deactivated;

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x06000C54 RID: 3156
		// (remove) Token: 0x06000C55 RID: 3157
		event EventHandler LoadComplete;

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x06000C56 RID: 3158
		// (remove) Token: 0x06000C57 RID: 3159
		event DesignerTransactionCloseEventHandler TransactionClosed;

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x06000C58 RID: 3160
		// (remove) Token: 0x06000C59 RID: 3161
		event DesignerTransactionCloseEventHandler TransactionClosing;

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x06000C5A RID: 3162
		// (remove) Token: 0x06000C5B RID: 3163
		event EventHandler TransactionOpened;

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x06000C5C RID: 3164
		// (remove) Token: 0x06000C5D RID: 3165
		event EventHandler TransactionOpening;

		// Token: 0x06000C5E RID: 3166
		void Activate();

		// Token: 0x06000C5F RID: 3167
		IComponent CreateComponent(Type componentClass);

		// Token: 0x06000C60 RID: 3168
		IComponent CreateComponent(Type componentClass, string name);

		// Token: 0x06000C61 RID: 3169
		DesignerTransaction CreateTransaction();

		// Token: 0x06000C62 RID: 3170
		DesignerTransaction CreateTransaction(string description);

		// Token: 0x06000C63 RID: 3171
		void DestroyComponent(IComponent component);

		// Token: 0x06000C64 RID: 3172
		IDesigner GetDesigner(IComponent component);

		// Token: 0x06000C65 RID: 3173
		Type GetType(string typeName);
	}
}
