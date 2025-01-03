﻿using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x020004B2 RID: 1202
	internal class MdiWindowListItemConverter : ComponentConverter
	{
		// Token: 0x06004807 RID: 18439 RVA: 0x00105D9F File Offset: 0x00104D9F
		public MdiWindowListItemConverter(Type type)
			: base(type)
		{
		}

		// Token: 0x06004808 RID: 18440 RVA: 0x00105DA8 File Offset: 0x00104DA8
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			MenuStrip menuStrip = context.Instance as MenuStrip;
			if (menuStrip != null)
			{
				TypeConverter.StandardValuesCollection standardValues = base.GetStandardValues(context);
				ArrayList arrayList = new ArrayList();
				int count = standardValues.Count;
				for (int i = 0; i < count; i++)
				{
					ToolStripItem toolStripItem = standardValues[i] as ToolStripItem;
					if (toolStripItem != null && toolStripItem.Owner == menuStrip)
					{
						arrayList.Add(toolStripItem);
					}
				}
				return new TypeConverter.StandardValuesCollection(arrayList);
			}
			return base.GetStandardValues(context);
		}
	}
}
