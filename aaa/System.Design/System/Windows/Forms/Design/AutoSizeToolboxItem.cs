using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Runtime.Serialization;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200017C RID: 380
	[Serializable]
	internal class AutoSizeToolboxItem : ToolboxItem
	{
		// Token: 0x06000DC8 RID: 3528 RVA: 0x00038523 File Offset: 0x00037523
		public AutoSizeToolboxItem()
		{
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x0003852B File Offset: 0x0003752B
		public AutoSizeToolboxItem(Type toolType)
			: base(toolType)
		{
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x00038534 File Offset: 0x00037534
		private AutoSizeToolboxItem(SerializationInfo info, StreamingContext context)
		{
			this.Deserialize(info, context);
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x00038544 File Offset: 0x00037544
		protected override IComponent[] CreateComponentsCore(IDesignerHost host)
		{
			IComponent[] array = base.CreateComponentsCore(host);
			if (array != null && array.Length > 0 && array[0] is Control)
			{
				Control control = array[0] as Control;
				control.AutoSize = true;
			}
			return array;
		}
	}
}
