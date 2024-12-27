using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Runtime.Serialization;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200029C RID: 668
	[Serializable]
	internal class TabControlToolboxItem : ToolboxItem
	{
		// Token: 0x060018D8 RID: 6360 RVA: 0x00084D0F File Offset: 0x00083D0F
		public TabControlToolboxItem()
			: base(typeof(TabControl))
		{
		}

		// Token: 0x060018D9 RID: 6361 RVA: 0x00084D21 File Offset: 0x00083D21
		private TabControlToolboxItem(SerializationInfo info, StreamingContext context)
		{
			this.Deserialize(info, context);
		}

		// Token: 0x060018DA RID: 6362 RVA: 0x00084D34 File Offset: 0x00083D34
		protected override IComponent[] CreateComponentsCore(IDesignerHost host)
		{
			IComponent[] array = base.CreateComponentsCore(host);
			if (array != null && array.Length > 0 && array[0] is TabControl)
			{
				TabControl tabControl = (TabControl)array[0];
				tabControl.ShowToolTips = true;
			}
			return array;
		}
	}
}
