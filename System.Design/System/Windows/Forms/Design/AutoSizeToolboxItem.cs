using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Runtime.Serialization;

namespace System.Windows.Forms.Design
{
	[Serializable]
	internal class AutoSizeToolboxItem : ToolboxItem
	{
		public AutoSizeToolboxItem()
		{
		}

		public AutoSizeToolboxItem(Type toolType)
			: base(toolType)
		{
		}

		private AutoSizeToolboxItem(SerializationInfo info, StreamingContext context)
		{
			this.Deserialize(info, context);
		}

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
