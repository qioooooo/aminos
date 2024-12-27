using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000828 RID: 2088
	[ComVisible(true)]
	public struct OpCode
	{
		// Token: 0x06004B83 RID: 19331 RVA: 0x00109DB8 File Offset: 0x00108DB8
		internal OpCode(string stringname, StackBehaviour pop, StackBehaviour push, OperandType operand, OpCodeType type, int size, byte s1, byte s2, FlowControl ctrl, bool endsjmpblk, int stack)
		{
			this.m_stringname = stringname;
			this.m_pop = pop;
			this.m_push = push;
			this.m_operand = operand;
			this.m_type = type;
			this.m_size = size;
			this.m_s1 = s1;
			this.m_s2 = s2;
			this.m_ctrl = ctrl;
			this.m_endsUncondJmpBlk = endsjmpblk;
			this.m_stackChange = stack;
		}

		// Token: 0x06004B84 RID: 19332 RVA: 0x00109E1A File Offset: 0x00108E1A
		internal bool EndsUncondJmpBlk()
		{
			return this.m_endsUncondJmpBlk;
		}

		// Token: 0x06004B85 RID: 19333 RVA: 0x00109E22 File Offset: 0x00108E22
		internal int StackChange()
		{
			return this.m_stackChange;
		}

		// Token: 0x17000D03 RID: 3331
		// (get) Token: 0x06004B86 RID: 19334 RVA: 0x00109E2A File Offset: 0x00108E2A
		public OperandType OperandType
		{
			get
			{
				return this.m_operand;
			}
		}

		// Token: 0x17000D04 RID: 3332
		// (get) Token: 0x06004B87 RID: 19335 RVA: 0x00109E32 File Offset: 0x00108E32
		public FlowControl FlowControl
		{
			get
			{
				return this.m_ctrl;
			}
		}

		// Token: 0x17000D05 RID: 3333
		// (get) Token: 0x06004B88 RID: 19336 RVA: 0x00109E3A File Offset: 0x00108E3A
		public OpCodeType OpCodeType
		{
			get
			{
				return this.m_type;
			}
		}

		// Token: 0x17000D06 RID: 3334
		// (get) Token: 0x06004B89 RID: 19337 RVA: 0x00109E42 File Offset: 0x00108E42
		public StackBehaviour StackBehaviourPop
		{
			get
			{
				return this.m_pop;
			}
		}

		// Token: 0x17000D07 RID: 3335
		// (get) Token: 0x06004B8A RID: 19338 RVA: 0x00109E4A File Offset: 0x00108E4A
		public StackBehaviour StackBehaviourPush
		{
			get
			{
				return this.m_push;
			}
		}

		// Token: 0x17000D08 RID: 3336
		// (get) Token: 0x06004B8B RID: 19339 RVA: 0x00109E52 File Offset: 0x00108E52
		public int Size
		{
			get
			{
				return this.m_size;
			}
		}

		// Token: 0x17000D09 RID: 3337
		// (get) Token: 0x06004B8C RID: 19340 RVA: 0x00109E5A File Offset: 0x00108E5A
		public short Value
		{
			get
			{
				if (this.m_size == 2)
				{
					return (short)(((int)this.m_s1 << 8) | (int)this.m_s2);
				}
				return (short)this.m_s2;
			}
		}

		// Token: 0x17000D0A RID: 3338
		// (get) Token: 0x06004B8D RID: 19341 RVA: 0x00109E7C File Offset: 0x00108E7C
		public string Name
		{
			get
			{
				return this.m_stringname;
			}
		}

		// Token: 0x06004B8E RID: 19342 RVA: 0x00109E84 File Offset: 0x00108E84
		public override bool Equals(object obj)
		{
			return obj is OpCode && this.Equals((OpCode)obj);
		}

		// Token: 0x06004B8F RID: 19343 RVA: 0x00109E9C File Offset: 0x00108E9C
		public bool Equals(OpCode obj)
		{
			return obj.m_s1 == this.m_s1 && obj.m_s2 == this.m_s2;
		}

		// Token: 0x06004B90 RID: 19344 RVA: 0x00109EBE File Offset: 0x00108EBE
		public static bool operator ==(OpCode a, OpCode b)
		{
			return a.Equals(b);
		}

		// Token: 0x06004B91 RID: 19345 RVA: 0x00109EC8 File Offset: 0x00108EC8
		public static bool operator !=(OpCode a, OpCode b)
		{
			return !(a == b);
		}

		// Token: 0x06004B92 RID: 19346 RVA: 0x00109ED4 File Offset: 0x00108ED4
		public override int GetHashCode()
		{
			return this.m_stringname.GetHashCode();
		}

		// Token: 0x06004B93 RID: 19347 RVA: 0x00109EE1 File Offset: 0x00108EE1
		public override string ToString()
		{
			return this.m_stringname;
		}

		// Token: 0x0400272C RID: 10028
		internal string m_stringname;

		// Token: 0x0400272D RID: 10029
		internal StackBehaviour m_pop;

		// Token: 0x0400272E RID: 10030
		internal StackBehaviour m_push;

		// Token: 0x0400272F RID: 10031
		internal OperandType m_operand;

		// Token: 0x04002730 RID: 10032
		internal OpCodeType m_type;

		// Token: 0x04002731 RID: 10033
		internal int m_size;

		// Token: 0x04002732 RID: 10034
		internal byte m_s1;

		// Token: 0x04002733 RID: 10035
		internal byte m_s2;

		// Token: 0x04002734 RID: 10036
		internal FlowControl m_ctrl;

		// Token: 0x04002735 RID: 10037
		internal bool m_endsUncondJmpBlk;

		// Token: 0x04002736 RID: 10038
		internal int m_stackChange;
	}
}
