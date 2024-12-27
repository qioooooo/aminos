using System;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;

namespace System.Windows.Forms.Design.Behavior
{
	// Token: 0x020002EF RID: 751
	internal sealed class DesignerActionKeyboardBehavior : Behavior
	{
		// Token: 0x06001D1F RID: 7455 RVA: 0x000A236C File Offset: 0x000A136C
		public DesignerActionKeyboardBehavior(DesignerActionPanel panel, IServiceProvider serviceProvider, BehaviorService behaviorService)
			: base(true, behaviorService)
		{
			this.panel = panel;
			if (serviceProvider != null)
			{
				this.menuService = serviceProvider.GetService(typeof(IMenuCommandService)) as IMenuCommandService;
				this.daUISvc = serviceProvider.GetService(typeof(DesignerActionUIService)) as DesignerActionUIService;
			}
		}

		// Token: 0x06001D20 RID: 7456 RVA: 0x000A23C4 File Offset: 0x000A13C4
		public override MenuCommand FindCommand(CommandID commandId)
		{
			if (this.panel != null && this.menuService != null)
			{
				foreach (CommandID commandID in this.panel.FilteredCommandIDs)
				{
					if (commandID.Equals(commandId))
					{
						return new MenuCommand(delegate
						{
						}, commandId)
						{
							Enabled = false
						};
					}
				}
				if (this.daUISvc != null && commandId.Guid == DesignerActionKeyboardBehavior.VSStandardCommandSet97 && commandId.ID == 1124)
				{
					this.daUISvc.HideUI(null);
				}
			}
			return base.FindCommand(commandId);
		}

		// Token: 0x04001629 RID: 5673
		private DesignerActionPanel panel;

		// Token: 0x0400162A RID: 5674
		private IMenuCommandService menuService;

		// Token: 0x0400162B RID: 5675
		private DesignerActionUIService daUISvc;

		// Token: 0x0400162C RID: 5676
		private static readonly Guid VSStandardCommandSet97 = new Guid("{5efc7975-14bc-11cf-9b2b-00aa00573819}");

		// Token: 0x0400162D RID: 5677
		[CompilerGenerated]
		private static EventHandler <>9__CachedAnonymousMethodDelegate1;
	}
}
