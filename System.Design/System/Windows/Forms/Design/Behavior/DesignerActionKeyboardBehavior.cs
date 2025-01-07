using System;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;

namespace System.Windows.Forms.Design.Behavior
{
	internal sealed class DesignerActionKeyboardBehavior : Behavior
	{
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

		private DesignerActionPanel panel;

		private IMenuCommandService menuService;

		private DesignerActionUIService daUISvc;

		private static readonly Guid VSStandardCommandSet97 = new Guid("{5efc7975-14bc-11cf-9b2b-00aa00573819}");

		[CompilerGenerated]
		private static EventHandler <>9__CachedAnonymousMethodDelegate1;
	}
}
