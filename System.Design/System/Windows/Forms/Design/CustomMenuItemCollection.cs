﻿using System;
using System.Collections;

namespace System.Windows.Forms.Design
{
	internal class CustomMenuItemCollection : CollectionBase
	{
		public int Add(ToolStripItem value)
		{
			return base.List.Add(value);
		}

		public void AddRange(ToolStripItem[] value)
		{
			for (int i = 0; i < value.Length; i++)
			{
				this.Add(value[i]);
			}
		}

		public virtual void RefreshItems()
		{
		}
	}
}
