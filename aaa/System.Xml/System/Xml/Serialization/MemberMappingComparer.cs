using System;
using System.Collections;

namespace System.Xml.Serialization
{
	// Token: 0x020002CF RID: 719
	internal class MemberMappingComparer : IComparer
	{
		// Token: 0x0600220A RID: 8714 RVA: 0x0009FCC8 File Offset: 0x0009ECC8
		public int Compare(object o1, object o2)
		{
			MemberMapping memberMapping = (MemberMapping)o1;
			MemberMapping memberMapping2 = (MemberMapping)o2;
			bool isText = memberMapping.IsText;
			if (isText)
			{
				if (memberMapping2.IsText)
				{
					return 0;
				}
				return 1;
			}
			else
			{
				if (memberMapping2.IsText)
				{
					return -1;
				}
				if (memberMapping.SequenceId < 0 && memberMapping2.SequenceId < 0)
				{
					return 0;
				}
				if (memberMapping.SequenceId < 0)
				{
					return 1;
				}
				if (memberMapping2.SequenceId < 0)
				{
					return -1;
				}
				if (memberMapping.SequenceId < memberMapping2.SequenceId)
				{
					return -1;
				}
				if (memberMapping.SequenceId > memberMapping2.SequenceId)
				{
					return 1;
				}
				return 0;
			}
		}
	}
}
