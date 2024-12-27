using System;
using System.Reflection;

namespace System.Runtime.Serialization
{
	// Token: 0x0200036E RID: 878
	internal class ValueTypeFixupInfo
	{
		// Token: 0x060022BF RID: 8895 RVA: 0x000586EC File Offset: 0x000576EC
		public ValueTypeFixupInfo(long containerID, FieldInfo member, int[] parentIndex)
		{
			if (containerID == 0L && member == null)
			{
				this.m_containerID = containerID;
				this.m_parentField = member;
				this.m_parentIndex = parentIndex;
			}
			if (member == null && parentIndex == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustSupplyParent"));
			}
			if (member != null)
			{
				if (parentIndex != null)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_MemberAndArray"));
				}
				if (member.FieldType.IsValueType && containerID == 0L)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_MustSupplyContainer"));
				}
			}
			this.m_containerID = containerID;
			this.m_parentField = member;
			this.m_parentIndex = parentIndex;
		}

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x060022C0 RID: 8896 RVA: 0x0005877F File Offset: 0x0005777F
		public long ContainerID
		{
			get
			{
				return this.m_containerID;
			}
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x060022C1 RID: 8897 RVA: 0x00058787 File Offset: 0x00057787
		public FieldInfo ParentField
		{
			get
			{
				return this.m_parentField;
			}
		}

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x060022C2 RID: 8898 RVA: 0x0005878F File Offset: 0x0005778F
		public int[] ParentIndex
		{
			get
			{
				return this.m_parentIndex;
			}
		}

		// Token: 0x04000E6E RID: 3694
		private long m_containerID;

		// Token: 0x04000E6F RID: 3695
		private FieldInfo m_parentField;

		// Token: 0x04000E70 RID: 3696
		private int[] m_parentIndex;
	}
}
