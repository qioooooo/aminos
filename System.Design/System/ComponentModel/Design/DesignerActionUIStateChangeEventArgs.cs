using System;

namespace System.ComponentModel.Design
{
	public class DesignerActionUIStateChangeEventArgs : EventArgs
	{
		public DesignerActionUIStateChangeEventArgs(object relatedObject, DesignerActionUIStateChangeType changeType)
		{
			this.relatedObject = relatedObject;
			this.changeType = changeType;
		}

		public DesignerActionUIStateChangeType ChangeType
		{
			get
			{
				return this.changeType;
			}
		}

		public object RelatedObject
		{
			get
			{
				return this.relatedObject;
			}
		}

		private object relatedObject;

		private DesignerActionUIStateChangeType changeType;
	}
}
