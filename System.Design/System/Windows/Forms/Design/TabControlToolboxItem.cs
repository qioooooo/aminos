using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Runtime.Serialization;

namespace System.Windows.Forms.Design
{
	[Serializable]
	internal class TabControlToolboxItem : ToolboxItem
	{
		public TabControlToolboxItem()
			: base(typeof(TabControl))
		{
		}

		private TabControlToolboxItem(SerializationInfo info, StreamingContext context)
		{
			this.Deserialize(info, context);
		}

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
