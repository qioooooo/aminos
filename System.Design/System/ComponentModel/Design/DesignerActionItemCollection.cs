using System;
using System.Collections;

namespace System.ComponentModel.Design
{
	public class DesignerActionItemCollection : CollectionBase
	{
		public DesignerActionItem this[int index]
		{
			get
			{
				return (DesignerActionItem)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		public int Add(DesignerActionItem value)
		{
			return base.List.Add(value);
		}

		public bool Contains(DesignerActionItem value)
		{
			return base.List.Contains(value);
		}

		public void CopyTo(DesignerActionItem[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		public int IndexOf(DesignerActionItem value)
		{
			return base.List.IndexOf(value);
		}

		public void Insert(int index, DesignerActionItem value)
		{
			base.List.Insert(index, value);
		}

		public void Remove(DesignerActionItem value)
		{
			base.List.Remove(value);
		}
	}
}
