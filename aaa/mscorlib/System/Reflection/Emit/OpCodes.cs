using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000827 RID: 2087
	[ComVisible(true)]
	public class OpCodes
	{
		// Token: 0x06004B80 RID: 19328 RVA: 0x001080BF File Offset: 0x001070BF
		private OpCodes()
		{
		}

		// Token: 0x06004B81 RID: 19329 RVA: 0x001080C8 File Offset: 0x001070C8
		public static bool TakesSingleByteArgument(OpCode inst)
		{
			switch (inst.m_operand)
			{
			case OperandType.ShortInlineBrTarget:
			case OperandType.ShortInlineI:
			case OperandType.ShortInlineVar:
				return true;
			}
			return false;
		}

		// Token: 0x0400264A RID: 9802
		public static readonly OpCode Nop = new OpCode("nop", StackBehaviour.Pop0, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 0, FlowControl.Next, false, 0);

		// Token: 0x0400264B RID: 9803
		public static readonly OpCode Break = new OpCode("break", StackBehaviour.Pop0, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 1, FlowControl.Break, false, 0);

		// Token: 0x0400264C RID: 9804
		public static readonly OpCode Ldarg_0 = new OpCode("ldarg.0", StackBehaviour.Pop0, StackBehaviour.Push1, OperandType.InlineNone, OpCodeType.Macro, 1, byte.MaxValue, 2, FlowControl.Next, false, 1);

		// Token: 0x0400264D RID: 9805
		public static readonly OpCode Ldarg_1 = new OpCode("ldarg.1", StackBehaviour.Pop0, StackBehaviour.Push1, OperandType.InlineNone, OpCodeType.Macro, 1, byte.MaxValue, 3, FlowControl.Next, false, 1);

		// Token: 0x0400264E RID: 9806
		public static readonly OpCode Ldarg_2 = new OpCode("ldarg.2", StackBehaviour.Pop0, StackBehaviour.Push1, OperandType.InlineNone, OpCodeType.Macro, 1, byte.MaxValue, 4, FlowControl.Next, false, 1);

		// Token: 0x0400264F RID: 9807
		public static readonly OpCode Ldarg_3 = new OpCode("ldarg.3", StackBehaviour.Pop0, StackBehaviour.Push1, OperandType.InlineNone, OpCodeType.Macro, 1, byte.MaxValue, 5, FlowControl.Next, false, 1);

		// Token: 0x04002650 RID: 9808
		public static readonly OpCode Ldloc_0 = new OpCode("ldloc.0", StackBehaviour.Pop0, StackBehaviour.Push1, OperandType.InlineNone, OpCodeType.Macro, 1, byte.MaxValue, 6, FlowControl.Next, false, 1);

		// Token: 0x04002651 RID: 9809
		public static readonly OpCode Ldloc_1 = new OpCode("ldloc.1", StackBehaviour.Pop0, StackBehaviour.Push1, OperandType.InlineNone, OpCodeType.Macro, 1, byte.MaxValue, 7, FlowControl.Next, false, 1);

		// Token: 0x04002652 RID: 9810
		public static readonly OpCode Ldloc_2 = new OpCode("ldloc.2", StackBehaviour.Pop0, StackBehaviour.Push1, OperandType.InlineNone, OpCodeType.Macro, 1, byte.MaxValue, 8, FlowControl.Next, false, 1);

		// Token: 0x04002653 RID: 9811
		public static readonly OpCode Ldloc_3 = new OpCode("ldloc.3", StackBehaviour.Pop0, StackBehaviour.Push1, OperandType.InlineNone, OpCodeType.Macro, 1, byte.MaxValue, 9, FlowControl.Next, false, 1);

		// Token: 0x04002654 RID: 9812
		public static readonly OpCode Stloc_0 = new OpCode("stloc.0", StackBehaviour.Pop1, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Macro, 1, byte.MaxValue, 10, FlowControl.Next, false, -1);

		// Token: 0x04002655 RID: 9813
		public static readonly OpCode Stloc_1 = new OpCode("stloc.1", StackBehaviour.Pop1, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Macro, 1, byte.MaxValue, 11, FlowControl.Next, false, -1);

		// Token: 0x04002656 RID: 9814
		public static readonly OpCode Stloc_2 = new OpCode("stloc.2", StackBehaviour.Pop1, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Macro, 1, byte.MaxValue, 12, FlowControl.Next, false, -1);

		// Token: 0x04002657 RID: 9815
		public static readonly OpCode Stloc_3 = new OpCode("stloc.3", StackBehaviour.Pop1, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Macro, 1, byte.MaxValue, 13, FlowControl.Next, false, -1);

		// Token: 0x04002658 RID: 9816
		public static readonly OpCode Ldarg_S = new OpCode("ldarg.s", StackBehaviour.Pop0, StackBehaviour.Push1, OperandType.ShortInlineVar, OpCodeType.Macro, 1, byte.MaxValue, 14, FlowControl.Next, false, 1);

		// Token: 0x04002659 RID: 9817
		public static readonly OpCode Ldarga_S = new OpCode("ldarga.s", StackBehaviour.Pop0, StackBehaviour.Pushi, OperandType.ShortInlineVar, OpCodeType.Macro, 1, byte.MaxValue, 15, FlowControl.Next, false, 1);

		// Token: 0x0400265A RID: 9818
		public static readonly OpCode Starg_S = new OpCode("starg.s", StackBehaviour.Pop1, StackBehaviour.Push0, OperandType.ShortInlineVar, OpCodeType.Macro, 1, byte.MaxValue, 16, FlowControl.Next, false, -1);

		// Token: 0x0400265B RID: 9819
		public static readonly OpCode Ldloc_S = new OpCode("ldloc.s", StackBehaviour.Pop0, StackBehaviour.Push1, OperandType.ShortInlineVar, OpCodeType.Macro, 1, byte.MaxValue, 17, FlowControl.Next, false, 1);

		// Token: 0x0400265C RID: 9820
		public static readonly OpCode Ldloca_S = new OpCode("ldloca.s", StackBehaviour.Pop0, StackBehaviour.Pushi, OperandType.ShortInlineVar, OpCodeType.Macro, 1, byte.MaxValue, 18, FlowControl.Next, false, 1);

		// Token: 0x0400265D RID: 9821
		public static readonly OpCode Stloc_S = new OpCode("stloc.s", StackBehaviour.Pop1, StackBehaviour.Push0, OperandType.ShortInlineVar, OpCodeType.Macro, 1, byte.MaxValue, 19, FlowControl.Next, false, -1);

		// Token: 0x0400265E RID: 9822
		public static readonly OpCode Ldnull = new OpCode("ldnull", StackBehaviour.Pop0, StackBehaviour.Pushref, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 20, FlowControl.Next, false, 1);

		// Token: 0x0400265F RID: 9823
		public static readonly OpCode Ldc_I4_M1 = new OpCode("ldc.i4.m1", StackBehaviour.Pop0, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Macro, 1, byte.MaxValue, 21, FlowControl.Next, false, 1);

		// Token: 0x04002660 RID: 9824
		public static readonly OpCode Ldc_I4_0 = new OpCode("ldc.i4.0", StackBehaviour.Pop0, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Macro, 1, byte.MaxValue, 22, FlowControl.Next, false, 1);

		// Token: 0x04002661 RID: 9825
		public static readonly OpCode Ldc_I4_1 = new OpCode("ldc.i4.1", StackBehaviour.Pop0, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Macro, 1, byte.MaxValue, 23, FlowControl.Next, false, 1);

		// Token: 0x04002662 RID: 9826
		public static readonly OpCode Ldc_I4_2 = new OpCode("ldc.i4.2", StackBehaviour.Pop0, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Macro, 1, byte.MaxValue, 24, FlowControl.Next, false, 1);

		// Token: 0x04002663 RID: 9827
		public static readonly OpCode Ldc_I4_3 = new OpCode("ldc.i4.3", StackBehaviour.Pop0, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Macro, 1, byte.MaxValue, 25, FlowControl.Next, false, 1);

		// Token: 0x04002664 RID: 9828
		public static readonly OpCode Ldc_I4_4 = new OpCode("ldc.i4.4", StackBehaviour.Pop0, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Macro, 1, byte.MaxValue, 26, FlowControl.Next, false, 1);

		// Token: 0x04002665 RID: 9829
		public static readonly OpCode Ldc_I4_5 = new OpCode("ldc.i4.5", StackBehaviour.Pop0, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Macro, 1, byte.MaxValue, 27, FlowControl.Next, false, 1);

		// Token: 0x04002666 RID: 9830
		public static readonly OpCode Ldc_I4_6 = new OpCode("ldc.i4.6", StackBehaviour.Pop0, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Macro, 1, byte.MaxValue, 28, FlowControl.Next, false, 1);

		// Token: 0x04002667 RID: 9831
		public static readonly OpCode Ldc_I4_7 = new OpCode("ldc.i4.7", StackBehaviour.Pop0, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Macro, 1, byte.MaxValue, 29, FlowControl.Next, false, 1);

		// Token: 0x04002668 RID: 9832
		public static readonly OpCode Ldc_I4_8 = new OpCode("ldc.i4.8", StackBehaviour.Pop0, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Macro, 1, byte.MaxValue, 30, FlowControl.Next, false, 1);

		// Token: 0x04002669 RID: 9833
		public static readonly OpCode Ldc_I4_S = new OpCode("ldc.i4.s", StackBehaviour.Pop0, StackBehaviour.Pushi, OperandType.ShortInlineI, OpCodeType.Macro, 1, byte.MaxValue, 31, FlowControl.Next, false, 1);

		// Token: 0x0400266A RID: 9834
		public static readonly OpCode Ldc_I4 = new OpCode("ldc.i4", StackBehaviour.Pop0, StackBehaviour.Pushi, OperandType.InlineI, OpCodeType.Primitive, 1, byte.MaxValue, 32, FlowControl.Next, false, 1);

		// Token: 0x0400266B RID: 9835
		public static readonly OpCode Ldc_I8 = new OpCode("ldc.i8", StackBehaviour.Pop0, StackBehaviour.Pushi8, OperandType.InlineI8, OpCodeType.Primitive, 1, byte.MaxValue, 33, FlowControl.Next, false, 1);

		// Token: 0x0400266C RID: 9836
		public static readonly OpCode Ldc_R4 = new OpCode("ldc.r4", StackBehaviour.Pop0, StackBehaviour.Pushr4, OperandType.ShortInlineR, OpCodeType.Primitive, 1, byte.MaxValue, 34, FlowControl.Next, false, 1);

		// Token: 0x0400266D RID: 9837
		public static readonly OpCode Ldc_R8 = new OpCode("ldc.r8", StackBehaviour.Pop0, StackBehaviour.Pushr8, OperandType.InlineR, OpCodeType.Primitive, 1, byte.MaxValue, 35, FlowControl.Next, false, 1);

		// Token: 0x0400266E RID: 9838
		public static readonly OpCode Dup = new OpCode("dup", StackBehaviour.Pop1, StackBehaviour.Push1_push1, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 37, FlowControl.Next, false, 1);

		// Token: 0x0400266F RID: 9839
		public static readonly OpCode Pop = new OpCode("pop", StackBehaviour.Pop1, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 38, FlowControl.Next, false, -1);

		// Token: 0x04002670 RID: 9840
		public static readonly OpCode Jmp = new OpCode("jmp", StackBehaviour.Pop0, StackBehaviour.Push0, OperandType.InlineMethod, OpCodeType.Primitive, 1, byte.MaxValue, 39, FlowControl.Call, true, 0);

		// Token: 0x04002671 RID: 9841
		public static readonly OpCode Call = new OpCode("call", StackBehaviour.Varpop, StackBehaviour.Varpush, OperandType.InlineMethod, OpCodeType.Primitive, 1, byte.MaxValue, 40, FlowControl.Call, false, 0);

		// Token: 0x04002672 RID: 9842
		public static readonly OpCode Calli = new OpCode("calli", StackBehaviour.Varpop, StackBehaviour.Varpush, OperandType.InlineSig, OpCodeType.Primitive, 1, byte.MaxValue, 41, FlowControl.Call, false, 0);

		// Token: 0x04002673 RID: 9843
		public static readonly OpCode Ret = new OpCode("ret", StackBehaviour.Varpop, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 42, FlowControl.Return, true, 0);

		// Token: 0x04002674 RID: 9844
		public static readonly OpCode Br_S = new OpCode("br.s", StackBehaviour.Pop0, StackBehaviour.Push0, OperandType.ShortInlineBrTarget, OpCodeType.Macro, 1, byte.MaxValue, 43, FlowControl.Branch, true, 0);

		// Token: 0x04002675 RID: 9845
		public static readonly OpCode Brfalse_S = new OpCode("brfalse.s", StackBehaviour.Popi, StackBehaviour.Push0, OperandType.ShortInlineBrTarget, OpCodeType.Macro, 1, byte.MaxValue, 44, FlowControl.Cond_Branch, false, -1);

		// Token: 0x04002676 RID: 9846
		public static readonly OpCode Brtrue_S = new OpCode("brtrue.s", StackBehaviour.Popi, StackBehaviour.Push0, OperandType.ShortInlineBrTarget, OpCodeType.Macro, 1, byte.MaxValue, 45, FlowControl.Cond_Branch, false, -1);

		// Token: 0x04002677 RID: 9847
		public static readonly OpCode Beq_S = new OpCode("beq.s", StackBehaviour.Pop1_pop1, StackBehaviour.Push0, OperandType.ShortInlineBrTarget, OpCodeType.Macro, 1, byte.MaxValue, 46, FlowControl.Cond_Branch, false, -2);

		// Token: 0x04002678 RID: 9848
		public static readonly OpCode Bge_S = new OpCode("bge.s", StackBehaviour.Pop1_pop1, StackBehaviour.Push0, OperandType.ShortInlineBrTarget, OpCodeType.Macro, 1, byte.MaxValue, 47, FlowControl.Cond_Branch, false, -2);

		// Token: 0x04002679 RID: 9849
		public static readonly OpCode Bgt_S = new OpCode("bgt.s", StackBehaviour.Pop1_pop1, StackBehaviour.Push0, OperandType.ShortInlineBrTarget, OpCodeType.Macro, 1, byte.MaxValue, 48, FlowControl.Cond_Branch, false, -2);

		// Token: 0x0400267A RID: 9850
		public static readonly OpCode Ble_S = new OpCode("ble.s", StackBehaviour.Pop1_pop1, StackBehaviour.Push0, OperandType.ShortInlineBrTarget, OpCodeType.Macro, 1, byte.MaxValue, 49, FlowControl.Cond_Branch, false, -2);

		// Token: 0x0400267B RID: 9851
		public static readonly OpCode Blt_S = new OpCode("blt.s", StackBehaviour.Pop1_pop1, StackBehaviour.Push0, OperandType.ShortInlineBrTarget, OpCodeType.Macro, 1, byte.MaxValue, 50, FlowControl.Cond_Branch, false, -2);

		// Token: 0x0400267C RID: 9852
		public static readonly OpCode Bne_Un_S = new OpCode("bne.un.s", StackBehaviour.Pop1_pop1, StackBehaviour.Push0, OperandType.ShortInlineBrTarget, OpCodeType.Macro, 1, byte.MaxValue, 51, FlowControl.Cond_Branch, false, -2);

		// Token: 0x0400267D RID: 9853
		public static readonly OpCode Bge_Un_S = new OpCode("bge.un.s", StackBehaviour.Pop1_pop1, StackBehaviour.Push0, OperandType.ShortInlineBrTarget, OpCodeType.Macro, 1, byte.MaxValue, 52, FlowControl.Cond_Branch, false, -2);

		// Token: 0x0400267E RID: 9854
		public static readonly OpCode Bgt_Un_S = new OpCode("bgt.un.s", StackBehaviour.Pop1_pop1, StackBehaviour.Push0, OperandType.ShortInlineBrTarget, OpCodeType.Macro, 1, byte.MaxValue, 53, FlowControl.Cond_Branch, false, -2);

		// Token: 0x0400267F RID: 9855
		public static readonly OpCode Ble_Un_S = new OpCode("ble.un.s", StackBehaviour.Pop1_pop1, StackBehaviour.Push0, OperandType.ShortInlineBrTarget, OpCodeType.Macro, 1, byte.MaxValue, 54, FlowControl.Cond_Branch, false, -2);

		// Token: 0x04002680 RID: 9856
		public static readonly OpCode Blt_Un_S = new OpCode("blt.un.s", StackBehaviour.Pop1_pop1, StackBehaviour.Push0, OperandType.ShortInlineBrTarget, OpCodeType.Macro, 1, byte.MaxValue, 55, FlowControl.Cond_Branch, false, -2);

		// Token: 0x04002681 RID: 9857
		public static readonly OpCode Br = new OpCode("br", StackBehaviour.Pop0, StackBehaviour.Push0, OperandType.InlineBrTarget, OpCodeType.Primitive, 1, byte.MaxValue, 56, FlowControl.Branch, true, 0);

		// Token: 0x04002682 RID: 9858
		public static readonly OpCode Brfalse = new OpCode("brfalse", StackBehaviour.Popi, StackBehaviour.Push0, OperandType.InlineBrTarget, OpCodeType.Primitive, 1, byte.MaxValue, 57, FlowControl.Cond_Branch, false, -1);

		// Token: 0x04002683 RID: 9859
		public static readonly OpCode Brtrue = new OpCode("brtrue", StackBehaviour.Popi, StackBehaviour.Push0, OperandType.InlineBrTarget, OpCodeType.Primitive, 1, byte.MaxValue, 58, FlowControl.Cond_Branch, false, -1);

		// Token: 0x04002684 RID: 9860
		public static readonly OpCode Beq = new OpCode("beq", StackBehaviour.Pop1_pop1, StackBehaviour.Push0, OperandType.InlineBrTarget, OpCodeType.Macro, 1, byte.MaxValue, 59, FlowControl.Cond_Branch, false, -2);

		// Token: 0x04002685 RID: 9861
		public static readonly OpCode Bge = new OpCode("bge", StackBehaviour.Pop1_pop1, StackBehaviour.Push0, OperandType.InlineBrTarget, OpCodeType.Macro, 1, byte.MaxValue, 60, FlowControl.Cond_Branch, false, -2);

		// Token: 0x04002686 RID: 9862
		public static readonly OpCode Bgt = new OpCode("bgt", StackBehaviour.Pop1_pop1, StackBehaviour.Push0, OperandType.InlineBrTarget, OpCodeType.Macro, 1, byte.MaxValue, 61, FlowControl.Cond_Branch, false, -2);

		// Token: 0x04002687 RID: 9863
		public static readonly OpCode Ble = new OpCode("ble", StackBehaviour.Pop1_pop1, StackBehaviour.Push0, OperandType.InlineBrTarget, OpCodeType.Macro, 1, byte.MaxValue, 62, FlowControl.Cond_Branch, false, -2);

		// Token: 0x04002688 RID: 9864
		public static readonly OpCode Blt = new OpCode("blt", StackBehaviour.Pop1_pop1, StackBehaviour.Push0, OperandType.InlineBrTarget, OpCodeType.Macro, 1, byte.MaxValue, 63, FlowControl.Cond_Branch, false, -2);

		// Token: 0x04002689 RID: 9865
		public static readonly OpCode Bne_Un = new OpCode("bne.un", StackBehaviour.Pop1_pop1, StackBehaviour.Push0, OperandType.InlineBrTarget, OpCodeType.Macro, 1, byte.MaxValue, 64, FlowControl.Cond_Branch, false, -2);

		// Token: 0x0400268A RID: 9866
		public static readonly OpCode Bge_Un = new OpCode("bge.un", StackBehaviour.Pop1_pop1, StackBehaviour.Push0, OperandType.InlineBrTarget, OpCodeType.Macro, 1, byte.MaxValue, 65, FlowControl.Cond_Branch, false, -2);

		// Token: 0x0400268B RID: 9867
		public static readonly OpCode Bgt_Un = new OpCode("bgt.un", StackBehaviour.Pop1_pop1, StackBehaviour.Push0, OperandType.InlineBrTarget, OpCodeType.Macro, 1, byte.MaxValue, 66, FlowControl.Cond_Branch, false, -2);

		// Token: 0x0400268C RID: 9868
		public static readonly OpCode Ble_Un = new OpCode("ble.un", StackBehaviour.Pop1_pop1, StackBehaviour.Push0, OperandType.InlineBrTarget, OpCodeType.Macro, 1, byte.MaxValue, 67, FlowControl.Cond_Branch, false, -2);

		// Token: 0x0400268D RID: 9869
		public static readonly OpCode Blt_Un = new OpCode("blt.un", StackBehaviour.Pop1_pop1, StackBehaviour.Push0, OperandType.InlineBrTarget, OpCodeType.Macro, 1, byte.MaxValue, 68, FlowControl.Cond_Branch, false, -2);

		// Token: 0x0400268E RID: 9870
		public static readonly OpCode Switch = new OpCode("switch", StackBehaviour.Popi, StackBehaviour.Push0, OperandType.InlineSwitch, OpCodeType.Primitive, 1, byte.MaxValue, 69, FlowControl.Cond_Branch, false, -1);

		// Token: 0x0400268F RID: 9871
		public static readonly OpCode Ldind_I1 = new OpCode("ldind.i1", StackBehaviour.Popi, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 70, FlowControl.Next, false, 0);

		// Token: 0x04002690 RID: 9872
		public static readonly OpCode Ldind_U1 = new OpCode("ldind.u1", StackBehaviour.Popi, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 71, FlowControl.Next, false, 0);

		// Token: 0x04002691 RID: 9873
		public static readonly OpCode Ldind_I2 = new OpCode("ldind.i2", StackBehaviour.Popi, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 72, FlowControl.Next, false, 0);

		// Token: 0x04002692 RID: 9874
		public static readonly OpCode Ldind_U2 = new OpCode("ldind.u2", StackBehaviour.Popi, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 73, FlowControl.Next, false, 0);

		// Token: 0x04002693 RID: 9875
		public static readonly OpCode Ldind_I4 = new OpCode("ldind.i4", StackBehaviour.Popi, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 74, FlowControl.Next, false, 0);

		// Token: 0x04002694 RID: 9876
		public static readonly OpCode Ldind_U4 = new OpCode("ldind.u4", StackBehaviour.Popi, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 75, FlowControl.Next, false, 0);

		// Token: 0x04002695 RID: 9877
		public static readonly OpCode Ldind_I8 = new OpCode("ldind.i8", StackBehaviour.Popi, StackBehaviour.Pushi8, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 76, FlowControl.Next, false, 0);

		// Token: 0x04002696 RID: 9878
		public static readonly OpCode Ldind_I = new OpCode("ldind.i", StackBehaviour.Popi, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 77, FlowControl.Next, false, 0);

		// Token: 0x04002697 RID: 9879
		public static readonly OpCode Ldind_R4 = new OpCode("ldind.r4", StackBehaviour.Popi, StackBehaviour.Pushr4, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 78, FlowControl.Next, false, 0);

		// Token: 0x04002698 RID: 9880
		public static readonly OpCode Ldind_R8 = new OpCode("ldind.r8", StackBehaviour.Popi, StackBehaviour.Pushr8, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 79, FlowControl.Next, false, 0);

		// Token: 0x04002699 RID: 9881
		public static readonly OpCode Ldind_Ref = new OpCode("ldind.ref", StackBehaviour.Popi, StackBehaviour.Pushref, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 80, FlowControl.Next, false, 0);

		// Token: 0x0400269A RID: 9882
		public static readonly OpCode Stind_Ref = new OpCode("stind.ref", StackBehaviour.Popi_popi, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 81, FlowControl.Next, false, -2);

		// Token: 0x0400269B RID: 9883
		public static readonly OpCode Stind_I1 = new OpCode("stind.i1", StackBehaviour.Popi_popi, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 82, FlowControl.Next, false, -2);

		// Token: 0x0400269C RID: 9884
		public static readonly OpCode Stind_I2 = new OpCode("stind.i2", StackBehaviour.Popi_popi, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 83, FlowControl.Next, false, -2);

		// Token: 0x0400269D RID: 9885
		public static readonly OpCode Stind_I4 = new OpCode("stind.i4", StackBehaviour.Popi_popi, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 84, FlowControl.Next, false, -2);

		// Token: 0x0400269E RID: 9886
		public static readonly OpCode Stind_I8 = new OpCode("stind.i8", StackBehaviour.Popi_popi8, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 85, FlowControl.Next, false, -2);

		// Token: 0x0400269F RID: 9887
		public static readonly OpCode Stind_R4 = new OpCode("stind.r4", StackBehaviour.Popi_popr4, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 86, FlowControl.Next, false, -2);

		// Token: 0x040026A0 RID: 9888
		public static readonly OpCode Stind_R8 = new OpCode("stind.r8", StackBehaviour.Popi_popr8, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 87, FlowControl.Next, false, -2);

		// Token: 0x040026A1 RID: 9889
		public static readonly OpCode Add = new OpCode("add", StackBehaviour.Pop1_pop1, StackBehaviour.Push1, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 88, FlowControl.Next, false, -1);

		// Token: 0x040026A2 RID: 9890
		public static readonly OpCode Sub = new OpCode("sub", StackBehaviour.Pop1_pop1, StackBehaviour.Push1, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 89, FlowControl.Next, false, -1);

		// Token: 0x040026A3 RID: 9891
		public static readonly OpCode Mul = new OpCode("mul", StackBehaviour.Pop1_pop1, StackBehaviour.Push1, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 90, FlowControl.Next, false, -1);

		// Token: 0x040026A4 RID: 9892
		public static readonly OpCode Div = new OpCode("div", StackBehaviour.Pop1_pop1, StackBehaviour.Push1, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 91, FlowControl.Next, false, -1);

		// Token: 0x040026A5 RID: 9893
		public static readonly OpCode Div_Un = new OpCode("div.un", StackBehaviour.Pop1_pop1, StackBehaviour.Push1, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 92, FlowControl.Next, false, -1);

		// Token: 0x040026A6 RID: 9894
		public static readonly OpCode Rem = new OpCode("rem", StackBehaviour.Pop1_pop1, StackBehaviour.Push1, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 93, FlowControl.Next, false, -1);

		// Token: 0x040026A7 RID: 9895
		public static readonly OpCode Rem_Un = new OpCode("rem.un", StackBehaviour.Pop1_pop1, StackBehaviour.Push1, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 94, FlowControl.Next, false, -1);

		// Token: 0x040026A8 RID: 9896
		public static readonly OpCode And = new OpCode("and", StackBehaviour.Pop1_pop1, StackBehaviour.Push1, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 95, FlowControl.Next, false, -1);

		// Token: 0x040026A9 RID: 9897
		public static readonly OpCode Or = new OpCode("or", StackBehaviour.Pop1_pop1, StackBehaviour.Push1, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 96, FlowControl.Next, false, -1);

		// Token: 0x040026AA RID: 9898
		public static readonly OpCode Xor = new OpCode("xor", StackBehaviour.Pop1_pop1, StackBehaviour.Push1, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 97, FlowControl.Next, false, -1);

		// Token: 0x040026AB RID: 9899
		public static readonly OpCode Shl = new OpCode("shl", StackBehaviour.Pop1_pop1, StackBehaviour.Push1, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 98, FlowControl.Next, false, -1);

		// Token: 0x040026AC RID: 9900
		public static readonly OpCode Shr = new OpCode("shr", StackBehaviour.Pop1_pop1, StackBehaviour.Push1, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 99, FlowControl.Next, false, -1);

		// Token: 0x040026AD RID: 9901
		public static readonly OpCode Shr_Un = new OpCode("shr.un", StackBehaviour.Pop1_pop1, StackBehaviour.Push1, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 100, FlowControl.Next, false, -1);

		// Token: 0x040026AE RID: 9902
		public static readonly OpCode Neg = new OpCode("neg", StackBehaviour.Pop1, StackBehaviour.Push1, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 101, FlowControl.Next, false, 0);

		// Token: 0x040026AF RID: 9903
		public static readonly OpCode Not = new OpCode("not", StackBehaviour.Pop1, StackBehaviour.Push1, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 102, FlowControl.Next, false, 0);

		// Token: 0x040026B0 RID: 9904
		public static readonly OpCode Conv_I1 = new OpCode("conv.i1", StackBehaviour.Pop1, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 103, FlowControl.Next, false, 0);

		// Token: 0x040026B1 RID: 9905
		public static readonly OpCode Conv_I2 = new OpCode("conv.i2", StackBehaviour.Pop1, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 104, FlowControl.Next, false, 0);

		// Token: 0x040026B2 RID: 9906
		public static readonly OpCode Conv_I4 = new OpCode("conv.i4", StackBehaviour.Pop1, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 105, FlowControl.Next, false, 0);

		// Token: 0x040026B3 RID: 9907
		public static readonly OpCode Conv_I8 = new OpCode("conv.i8", StackBehaviour.Pop1, StackBehaviour.Pushi8, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 106, FlowControl.Next, false, 0);

		// Token: 0x040026B4 RID: 9908
		public static readonly OpCode Conv_R4 = new OpCode("conv.r4", StackBehaviour.Pop1, StackBehaviour.Pushr4, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 107, FlowControl.Next, false, 0);

		// Token: 0x040026B5 RID: 9909
		public static readonly OpCode Conv_R8 = new OpCode("conv.r8", StackBehaviour.Pop1, StackBehaviour.Pushr8, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 108, FlowControl.Next, false, 0);

		// Token: 0x040026B6 RID: 9910
		public static readonly OpCode Conv_U4 = new OpCode("conv.u4", StackBehaviour.Pop1, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 109, FlowControl.Next, false, 0);

		// Token: 0x040026B7 RID: 9911
		public static readonly OpCode Conv_U8 = new OpCode("conv.u8", StackBehaviour.Pop1, StackBehaviour.Pushi8, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 110, FlowControl.Next, false, 0);

		// Token: 0x040026B8 RID: 9912
		public static readonly OpCode Callvirt = new OpCode("callvirt", StackBehaviour.Varpop, StackBehaviour.Varpush, OperandType.InlineMethod, OpCodeType.Objmodel, 1, byte.MaxValue, 111, FlowControl.Call, false, 0);

		// Token: 0x040026B9 RID: 9913
		public static readonly OpCode Cpobj = new OpCode("cpobj", StackBehaviour.Popi_popi, StackBehaviour.Push0, OperandType.InlineType, OpCodeType.Objmodel, 1, byte.MaxValue, 112, FlowControl.Next, false, -2);

		// Token: 0x040026BA RID: 9914
		public static readonly OpCode Ldobj = new OpCode("ldobj", StackBehaviour.Popi, StackBehaviour.Push1, OperandType.InlineType, OpCodeType.Objmodel, 1, byte.MaxValue, 113, FlowControl.Next, false, 0);

		// Token: 0x040026BB RID: 9915
		public static readonly OpCode Ldstr = new OpCode("ldstr", StackBehaviour.Pop0, StackBehaviour.Pushref, OperandType.InlineString, OpCodeType.Objmodel, 1, byte.MaxValue, 114, FlowControl.Next, false, 1);

		// Token: 0x040026BC RID: 9916
		public static readonly OpCode Newobj = new OpCode("newobj", StackBehaviour.Varpop, StackBehaviour.Pushref, OperandType.InlineMethod, OpCodeType.Objmodel, 1, byte.MaxValue, 115, FlowControl.Call, false, 1);

		// Token: 0x040026BD RID: 9917
		[ComVisible(true)]
		public static readonly OpCode Castclass = new OpCode("castclass", StackBehaviour.Popref, StackBehaviour.Pushref, OperandType.InlineType, OpCodeType.Objmodel, 1, byte.MaxValue, 116, FlowControl.Next, false, 0);

		// Token: 0x040026BE RID: 9918
		public static readonly OpCode Isinst = new OpCode("isinst", StackBehaviour.Popref, StackBehaviour.Pushi, OperandType.InlineType, OpCodeType.Objmodel, 1, byte.MaxValue, 117, FlowControl.Next, false, 0);

		// Token: 0x040026BF RID: 9919
		public static readonly OpCode Conv_R_Un = new OpCode("conv.r.un", StackBehaviour.Pop1, StackBehaviour.Pushr8, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 118, FlowControl.Next, false, 0);

		// Token: 0x040026C0 RID: 9920
		public static readonly OpCode Unbox = new OpCode("unbox", StackBehaviour.Popref, StackBehaviour.Pushi, OperandType.InlineType, OpCodeType.Primitive, 1, byte.MaxValue, 121, FlowControl.Next, false, 0);

		// Token: 0x040026C1 RID: 9921
		public static readonly OpCode Throw = new OpCode("throw", StackBehaviour.Popref, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Objmodel, 1, byte.MaxValue, 122, FlowControl.Throw, true, -1);

		// Token: 0x040026C2 RID: 9922
		public static readonly OpCode Ldfld = new OpCode("ldfld", StackBehaviour.Popref, StackBehaviour.Push1, OperandType.InlineField, OpCodeType.Objmodel, 1, byte.MaxValue, 123, FlowControl.Next, false, 0);

		// Token: 0x040026C3 RID: 9923
		public static readonly OpCode Ldflda = new OpCode("ldflda", StackBehaviour.Popref, StackBehaviour.Pushi, OperandType.InlineField, OpCodeType.Objmodel, 1, byte.MaxValue, 124, FlowControl.Next, false, 0);

		// Token: 0x040026C4 RID: 9924
		public static readonly OpCode Stfld = new OpCode("stfld", StackBehaviour.Popref_pop1, StackBehaviour.Push0, OperandType.InlineField, OpCodeType.Objmodel, 1, byte.MaxValue, 125, FlowControl.Next, false, -2);

		// Token: 0x040026C5 RID: 9925
		public static readonly OpCode Ldsfld = new OpCode("ldsfld", StackBehaviour.Pop0, StackBehaviour.Push1, OperandType.InlineField, OpCodeType.Objmodel, 1, byte.MaxValue, 126, FlowControl.Next, false, 1);

		// Token: 0x040026C6 RID: 9926
		public static readonly OpCode Ldsflda = new OpCode("ldsflda", StackBehaviour.Pop0, StackBehaviour.Pushi, OperandType.InlineField, OpCodeType.Objmodel, 1, byte.MaxValue, 127, FlowControl.Next, false, 1);

		// Token: 0x040026C7 RID: 9927
		public static readonly OpCode Stsfld = new OpCode("stsfld", StackBehaviour.Pop1, StackBehaviour.Push0, OperandType.InlineField, OpCodeType.Objmodel, 1, byte.MaxValue, 128, FlowControl.Next, false, -1);

		// Token: 0x040026C8 RID: 9928
		public static readonly OpCode Stobj = new OpCode("stobj", StackBehaviour.Popi_pop1, StackBehaviour.Push0, OperandType.InlineType, OpCodeType.Primitive, 1, byte.MaxValue, 129, FlowControl.Next, false, -2);

		// Token: 0x040026C9 RID: 9929
		public static readonly OpCode Conv_Ovf_I1_Un = new OpCode("conv.ovf.i1.un", StackBehaviour.Pop1, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 130, FlowControl.Next, false, 0);

		// Token: 0x040026CA RID: 9930
		public static readonly OpCode Conv_Ovf_I2_Un = new OpCode("conv.ovf.i2.un", StackBehaviour.Pop1, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 131, FlowControl.Next, false, 0);

		// Token: 0x040026CB RID: 9931
		public static readonly OpCode Conv_Ovf_I4_Un = new OpCode("conv.ovf.i4.un", StackBehaviour.Pop1, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 132, FlowControl.Next, false, 0);

		// Token: 0x040026CC RID: 9932
		public static readonly OpCode Conv_Ovf_I8_Un = new OpCode("conv.ovf.i8.un", StackBehaviour.Pop1, StackBehaviour.Pushi8, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 133, FlowControl.Next, false, 0);

		// Token: 0x040026CD RID: 9933
		public static readonly OpCode Conv_Ovf_U1_Un = new OpCode("conv.ovf.u1.un", StackBehaviour.Pop1, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 134, FlowControl.Next, false, 0);

		// Token: 0x040026CE RID: 9934
		public static readonly OpCode Conv_Ovf_U2_Un = new OpCode("conv.ovf.u2.un", StackBehaviour.Pop1, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 135, FlowControl.Next, false, 0);

		// Token: 0x040026CF RID: 9935
		public static readonly OpCode Conv_Ovf_U4_Un = new OpCode("conv.ovf.u4.un", StackBehaviour.Pop1, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 136, FlowControl.Next, false, 0);

		// Token: 0x040026D0 RID: 9936
		public static readonly OpCode Conv_Ovf_U8_Un = new OpCode("conv.ovf.u8.un", StackBehaviour.Pop1, StackBehaviour.Pushi8, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 137, FlowControl.Next, false, 0);

		// Token: 0x040026D1 RID: 9937
		public static readonly OpCode Conv_Ovf_I_Un = new OpCode("conv.ovf.i.un", StackBehaviour.Pop1, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 138, FlowControl.Next, false, 0);

		// Token: 0x040026D2 RID: 9938
		public static readonly OpCode Conv_Ovf_U_Un = new OpCode("conv.ovf.u.un", StackBehaviour.Pop1, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 139, FlowControl.Next, false, 0);

		// Token: 0x040026D3 RID: 9939
		public static readonly OpCode Box = new OpCode("box", StackBehaviour.Pop1, StackBehaviour.Pushref, OperandType.InlineType, OpCodeType.Primitive, 1, byte.MaxValue, 140, FlowControl.Next, false, 0);

		// Token: 0x040026D4 RID: 9940
		public static readonly OpCode Newarr = new OpCode("newarr", StackBehaviour.Popi, StackBehaviour.Pushref, OperandType.InlineType, OpCodeType.Objmodel, 1, byte.MaxValue, 141, FlowControl.Next, false, 0);

		// Token: 0x040026D5 RID: 9941
		public static readonly OpCode Ldlen = new OpCode("ldlen", StackBehaviour.Popref, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Objmodel, 1, byte.MaxValue, 142, FlowControl.Next, false, 0);

		// Token: 0x040026D6 RID: 9942
		public static readonly OpCode Ldelema = new OpCode("ldelema", StackBehaviour.Popref_popi, StackBehaviour.Pushi, OperandType.InlineType, OpCodeType.Objmodel, 1, byte.MaxValue, 143, FlowControl.Next, false, -1);

		// Token: 0x040026D7 RID: 9943
		public static readonly OpCode Ldelem_I1 = new OpCode("ldelem.i1", StackBehaviour.Popref_popi, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Objmodel, 1, byte.MaxValue, 144, FlowControl.Next, false, -1);

		// Token: 0x040026D8 RID: 9944
		public static readonly OpCode Ldelem_U1 = new OpCode("ldelem.u1", StackBehaviour.Popref_popi, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Objmodel, 1, byte.MaxValue, 145, FlowControl.Next, false, -1);

		// Token: 0x040026D9 RID: 9945
		public static readonly OpCode Ldelem_I2 = new OpCode("ldelem.i2", StackBehaviour.Popref_popi, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Objmodel, 1, byte.MaxValue, 146, FlowControl.Next, false, -1);

		// Token: 0x040026DA RID: 9946
		public static readonly OpCode Ldelem_U2 = new OpCode("ldelem.u2", StackBehaviour.Popref_popi, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Objmodel, 1, byte.MaxValue, 147, FlowControl.Next, false, -1);

		// Token: 0x040026DB RID: 9947
		public static readonly OpCode Ldelem_I4 = new OpCode("ldelem.i4", StackBehaviour.Popref_popi, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Objmodel, 1, byte.MaxValue, 148, FlowControl.Next, false, -1);

		// Token: 0x040026DC RID: 9948
		public static readonly OpCode Ldelem_U4 = new OpCode("ldelem.u4", StackBehaviour.Popref_popi, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Objmodel, 1, byte.MaxValue, 149, FlowControl.Next, false, -1);

		// Token: 0x040026DD RID: 9949
		public static readonly OpCode Ldelem_I8 = new OpCode("ldelem.i8", StackBehaviour.Popref_popi, StackBehaviour.Pushi8, OperandType.InlineNone, OpCodeType.Objmodel, 1, byte.MaxValue, 150, FlowControl.Next, false, -1);

		// Token: 0x040026DE RID: 9950
		public static readonly OpCode Ldelem_I = new OpCode("ldelem.i", StackBehaviour.Popref_popi, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Objmodel, 1, byte.MaxValue, 151, FlowControl.Next, false, -1);

		// Token: 0x040026DF RID: 9951
		public static readonly OpCode Ldelem_R4 = new OpCode("ldelem.r4", StackBehaviour.Popref_popi, StackBehaviour.Pushr4, OperandType.InlineNone, OpCodeType.Objmodel, 1, byte.MaxValue, 152, FlowControl.Next, false, -1);

		// Token: 0x040026E0 RID: 9952
		public static readonly OpCode Ldelem_R8 = new OpCode("ldelem.r8", StackBehaviour.Popref_popi, StackBehaviour.Pushr8, OperandType.InlineNone, OpCodeType.Objmodel, 1, byte.MaxValue, 153, FlowControl.Next, false, -1);

		// Token: 0x040026E1 RID: 9953
		public static readonly OpCode Ldelem_Ref = new OpCode("ldelem.ref", StackBehaviour.Popref_popi, StackBehaviour.Pushref, OperandType.InlineNone, OpCodeType.Objmodel, 1, byte.MaxValue, 154, FlowControl.Next, false, -1);

		// Token: 0x040026E2 RID: 9954
		public static readonly OpCode Stelem_I = new OpCode("stelem.i", StackBehaviour.Popref_popi_popi, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Objmodel, 1, byte.MaxValue, 155, FlowControl.Next, false, -3);

		// Token: 0x040026E3 RID: 9955
		public static readonly OpCode Stelem_I1 = new OpCode("stelem.i1", StackBehaviour.Popref_popi_popi, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Objmodel, 1, byte.MaxValue, 156, FlowControl.Next, false, -3);

		// Token: 0x040026E4 RID: 9956
		public static readonly OpCode Stelem_I2 = new OpCode("stelem.i2", StackBehaviour.Popref_popi_popi, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Objmodel, 1, byte.MaxValue, 157, FlowControl.Next, false, -3);

		// Token: 0x040026E5 RID: 9957
		public static readonly OpCode Stelem_I4 = new OpCode("stelem.i4", StackBehaviour.Popref_popi_popi, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Objmodel, 1, byte.MaxValue, 158, FlowControl.Next, false, -3);

		// Token: 0x040026E6 RID: 9958
		public static readonly OpCode Stelem_I8 = new OpCode("stelem.i8", StackBehaviour.Popref_popi_popi8, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Objmodel, 1, byte.MaxValue, 159, FlowControl.Next, false, -3);

		// Token: 0x040026E7 RID: 9959
		public static readonly OpCode Stelem_R4 = new OpCode("stelem.r4", StackBehaviour.Popref_popi_popr4, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Objmodel, 1, byte.MaxValue, 160, FlowControl.Next, false, -3);

		// Token: 0x040026E8 RID: 9960
		public static readonly OpCode Stelem_R8 = new OpCode("stelem.r8", StackBehaviour.Popref_popi_popr8, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Objmodel, 1, byte.MaxValue, 161, FlowControl.Next, false, -3);

		// Token: 0x040026E9 RID: 9961
		public static readonly OpCode Stelem_Ref = new OpCode("stelem.ref", StackBehaviour.Popref_popi_popref, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Objmodel, 1, byte.MaxValue, 162, FlowControl.Next, false, -3);

		// Token: 0x040026EA RID: 9962
		public static readonly OpCode Ldelem = new OpCode("ldelem", StackBehaviour.Popref_popi, StackBehaviour.Push1, OperandType.InlineType, OpCodeType.Objmodel, 1, byte.MaxValue, 163, FlowControl.Next, false, -1);

		// Token: 0x040026EB RID: 9963
		public static readonly OpCode Stelem = new OpCode("stelem", StackBehaviour.Popref_popi_pop1, StackBehaviour.Push0, OperandType.InlineType, OpCodeType.Objmodel, 1, byte.MaxValue, 164, FlowControl.Next, false, 0);

		// Token: 0x040026EC RID: 9964
		public static readonly OpCode Unbox_Any = new OpCode("unbox.any", StackBehaviour.Popref, StackBehaviour.Push1, OperandType.InlineType, OpCodeType.Objmodel, 1, byte.MaxValue, 165, FlowControl.Next, false, 0);

		// Token: 0x040026ED RID: 9965
		public static readonly OpCode Conv_Ovf_I1 = new OpCode("conv.ovf.i1", StackBehaviour.Pop1, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 179, FlowControl.Next, false, 0);

		// Token: 0x040026EE RID: 9966
		public static readonly OpCode Conv_Ovf_U1 = new OpCode("conv.ovf.u1", StackBehaviour.Pop1, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 180, FlowControl.Next, false, 0);

		// Token: 0x040026EF RID: 9967
		public static readonly OpCode Conv_Ovf_I2 = new OpCode("conv.ovf.i2", StackBehaviour.Pop1, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 181, FlowControl.Next, false, 0);

		// Token: 0x040026F0 RID: 9968
		public static readonly OpCode Conv_Ovf_U2 = new OpCode("conv.ovf.u2", StackBehaviour.Pop1, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 182, FlowControl.Next, false, 0);

		// Token: 0x040026F1 RID: 9969
		public static readonly OpCode Conv_Ovf_I4 = new OpCode("conv.ovf.i4", StackBehaviour.Pop1, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 183, FlowControl.Next, false, 0);

		// Token: 0x040026F2 RID: 9970
		public static readonly OpCode Conv_Ovf_U4 = new OpCode("conv.ovf.u4", StackBehaviour.Pop1, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 184, FlowControl.Next, false, 0);

		// Token: 0x040026F3 RID: 9971
		public static readonly OpCode Conv_Ovf_I8 = new OpCode("conv.ovf.i8", StackBehaviour.Pop1, StackBehaviour.Pushi8, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 185, FlowControl.Next, false, 0);

		// Token: 0x040026F4 RID: 9972
		public static readonly OpCode Conv_Ovf_U8 = new OpCode("conv.ovf.u8", StackBehaviour.Pop1, StackBehaviour.Pushi8, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 186, FlowControl.Next, false, 0);

		// Token: 0x040026F5 RID: 9973
		public static readonly OpCode Refanyval = new OpCode("refanyval", StackBehaviour.Pop1, StackBehaviour.Pushi, OperandType.InlineType, OpCodeType.Primitive, 1, byte.MaxValue, 194, FlowControl.Next, false, 0);

		// Token: 0x040026F6 RID: 9974
		public static readonly OpCode Ckfinite = new OpCode("ckfinite", StackBehaviour.Pop1, StackBehaviour.Pushr8, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 195, FlowControl.Next, false, 0);

		// Token: 0x040026F7 RID: 9975
		public static readonly OpCode Mkrefany = new OpCode("mkrefany", StackBehaviour.Popi, StackBehaviour.Push1, OperandType.InlineType, OpCodeType.Primitive, 1, byte.MaxValue, 198, FlowControl.Next, false, 0);

		// Token: 0x040026F8 RID: 9976
		public static readonly OpCode Ldtoken = new OpCode("ldtoken", StackBehaviour.Pop0, StackBehaviour.Pushi, OperandType.InlineTok, OpCodeType.Primitive, 1, byte.MaxValue, 208, FlowControl.Next, false, 1);

		// Token: 0x040026F9 RID: 9977
		public static readonly OpCode Conv_U2 = new OpCode("conv.u2", StackBehaviour.Pop1, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 209, FlowControl.Next, false, 0);

		// Token: 0x040026FA RID: 9978
		public static readonly OpCode Conv_U1 = new OpCode("conv.u1", StackBehaviour.Pop1, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 210, FlowControl.Next, false, 0);

		// Token: 0x040026FB RID: 9979
		public static readonly OpCode Conv_I = new OpCode("conv.i", StackBehaviour.Pop1, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 211, FlowControl.Next, false, 0);

		// Token: 0x040026FC RID: 9980
		public static readonly OpCode Conv_Ovf_I = new OpCode("conv.ovf.i", StackBehaviour.Pop1, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 212, FlowControl.Next, false, 0);

		// Token: 0x040026FD RID: 9981
		public static readonly OpCode Conv_Ovf_U = new OpCode("conv.ovf.u", StackBehaviour.Pop1, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 213, FlowControl.Next, false, 0);

		// Token: 0x040026FE RID: 9982
		public static readonly OpCode Add_Ovf = new OpCode("add.ovf", StackBehaviour.Pop1_pop1, StackBehaviour.Push1, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 214, FlowControl.Next, false, -1);

		// Token: 0x040026FF RID: 9983
		public static readonly OpCode Add_Ovf_Un = new OpCode("add.ovf.un", StackBehaviour.Pop1_pop1, StackBehaviour.Push1, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 215, FlowControl.Next, false, -1);

		// Token: 0x04002700 RID: 9984
		public static readonly OpCode Mul_Ovf = new OpCode("mul.ovf", StackBehaviour.Pop1_pop1, StackBehaviour.Push1, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 216, FlowControl.Next, false, -1);

		// Token: 0x04002701 RID: 9985
		public static readonly OpCode Mul_Ovf_Un = new OpCode("mul.ovf.un", StackBehaviour.Pop1_pop1, StackBehaviour.Push1, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 217, FlowControl.Next, false, -1);

		// Token: 0x04002702 RID: 9986
		public static readonly OpCode Sub_Ovf = new OpCode("sub.ovf", StackBehaviour.Pop1_pop1, StackBehaviour.Push1, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 218, FlowControl.Next, false, -1);

		// Token: 0x04002703 RID: 9987
		public static readonly OpCode Sub_Ovf_Un = new OpCode("sub.ovf.un", StackBehaviour.Pop1_pop1, StackBehaviour.Push1, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 219, FlowControl.Next, false, -1);

		// Token: 0x04002704 RID: 9988
		public static readonly OpCode Endfinally = new OpCode("endfinally", StackBehaviour.Pop0, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 220, FlowControl.Return, true, 0);

		// Token: 0x04002705 RID: 9989
		public static readonly OpCode Leave = new OpCode("leave", StackBehaviour.Pop0, StackBehaviour.Push0, OperandType.InlineBrTarget, OpCodeType.Primitive, 1, byte.MaxValue, 221, FlowControl.Branch, true, 0);

		// Token: 0x04002706 RID: 9990
		public static readonly OpCode Leave_S = new OpCode("leave.s", StackBehaviour.Pop0, StackBehaviour.Push0, OperandType.ShortInlineBrTarget, OpCodeType.Primitive, 1, byte.MaxValue, 222, FlowControl.Branch, true, 0);

		// Token: 0x04002707 RID: 9991
		public static readonly OpCode Stind_I = new OpCode("stind.i", StackBehaviour.Popi_popi, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 223, FlowControl.Next, false, -2);

		// Token: 0x04002708 RID: 9992
		public static readonly OpCode Conv_U = new OpCode("conv.u", StackBehaviour.Pop1, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 1, byte.MaxValue, 224, FlowControl.Next, false, 0);

		// Token: 0x04002709 RID: 9993
		public static readonly OpCode Prefix7 = new OpCode("prefix7", StackBehaviour.Pop0, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Nternal, 1, byte.MaxValue, 248, FlowControl.Meta, false, 0);

		// Token: 0x0400270A RID: 9994
		public static readonly OpCode Prefix6 = new OpCode("prefix6", StackBehaviour.Pop0, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Nternal, 1, byte.MaxValue, 249, FlowControl.Meta, false, 0);

		// Token: 0x0400270B RID: 9995
		public static readonly OpCode Prefix5 = new OpCode("prefix5", StackBehaviour.Pop0, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Nternal, 1, byte.MaxValue, 250, FlowControl.Meta, false, 0);

		// Token: 0x0400270C RID: 9996
		public static readonly OpCode Prefix4 = new OpCode("prefix4", StackBehaviour.Pop0, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Nternal, 1, byte.MaxValue, 251, FlowControl.Meta, false, 0);

		// Token: 0x0400270D RID: 9997
		public static readonly OpCode Prefix3 = new OpCode("prefix3", StackBehaviour.Pop0, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Nternal, 1, byte.MaxValue, 252, FlowControl.Meta, false, 0);

		// Token: 0x0400270E RID: 9998
		public static readonly OpCode Prefix2 = new OpCode("prefix2", StackBehaviour.Pop0, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Nternal, 1, byte.MaxValue, 253, FlowControl.Meta, false, 0);

		// Token: 0x0400270F RID: 9999
		public static readonly OpCode Prefix1 = new OpCode("prefix1", StackBehaviour.Pop0, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Nternal, 1, byte.MaxValue, 254, FlowControl.Meta, false, 0);

		// Token: 0x04002710 RID: 10000
		public static readonly OpCode Prefixref = new OpCode("prefixref", StackBehaviour.Pop0, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Nternal, 1, byte.MaxValue, byte.MaxValue, FlowControl.Meta, false, 0);

		// Token: 0x04002711 RID: 10001
		public static readonly OpCode Arglist = new OpCode("arglist", StackBehaviour.Pop0, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 2, 254, 0, FlowControl.Next, false, 1);

		// Token: 0x04002712 RID: 10002
		public static readonly OpCode Ceq = new OpCode("ceq", StackBehaviour.Pop1_pop1, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 2, 254, 1, FlowControl.Next, false, -1);

		// Token: 0x04002713 RID: 10003
		public static readonly OpCode Cgt = new OpCode("cgt", StackBehaviour.Pop1_pop1, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 2, 254, 2, FlowControl.Next, false, -1);

		// Token: 0x04002714 RID: 10004
		public static readonly OpCode Cgt_Un = new OpCode("cgt.un", StackBehaviour.Pop1_pop1, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 2, 254, 3, FlowControl.Next, false, -1);

		// Token: 0x04002715 RID: 10005
		public static readonly OpCode Clt = new OpCode("clt", StackBehaviour.Pop1_pop1, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 2, 254, 4, FlowControl.Next, false, -1);

		// Token: 0x04002716 RID: 10006
		public static readonly OpCode Clt_Un = new OpCode("clt.un", StackBehaviour.Pop1_pop1, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 2, 254, 5, FlowControl.Next, false, -1);

		// Token: 0x04002717 RID: 10007
		public static readonly OpCode Ldftn = new OpCode("ldftn", StackBehaviour.Pop0, StackBehaviour.Pushi, OperandType.InlineMethod, OpCodeType.Primitive, 2, 254, 6, FlowControl.Next, false, 1);

		// Token: 0x04002718 RID: 10008
		public static readonly OpCode Ldvirtftn = new OpCode("ldvirtftn", StackBehaviour.Popref, StackBehaviour.Pushi, OperandType.InlineMethod, OpCodeType.Primitive, 2, 254, 7, FlowControl.Next, false, 0);

		// Token: 0x04002719 RID: 10009
		public static readonly OpCode Ldarg = new OpCode("ldarg", StackBehaviour.Pop0, StackBehaviour.Push1, OperandType.InlineVar, OpCodeType.Primitive, 2, 254, 9, FlowControl.Next, false, 1);

		// Token: 0x0400271A RID: 10010
		public static readonly OpCode Ldarga = new OpCode("ldarga", StackBehaviour.Pop0, StackBehaviour.Pushi, OperandType.InlineVar, OpCodeType.Primitive, 2, 254, 10, FlowControl.Next, false, 1);

		// Token: 0x0400271B RID: 10011
		public static readonly OpCode Starg = new OpCode("starg", StackBehaviour.Pop1, StackBehaviour.Push0, OperandType.InlineVar, OpCodeType.Primitive, 2, 254, 11, FlowControl.Next, false, -1);

		// Token: 0x0400271C RID: 10012
		public static readonly OpCode Ldloc = new OpCode("ldloc", StackBehaviour.Pop0, StackBehaviour.Push1, OperandType.InlineVar, OpCodeType.Primitive, 2, 254, 12, FlowControl.Next, false, 1);

		// Token: 0x0400271D RID: 10013
		public static readonly OpCode Ldloca = new OpCode("ldloca", StackBehaviour.Pop0, StackBehaviour.Pushi, OperandType.InlineVar, OpCodeType.Primitive, 2, 254, 13, FlowControl.Next, false, 1);

		// Token: 0x0400271E RID: 10014
		public static readonly OpCode Stloc = new OpCode("stloc", StackBehaviour.Pop1, StackBehaviour.Push0, OperandType.InlineVar, OpCodeType.Primitive, 2, 254, 14, FlowControl.Next, false, -1);

		// Token: 0x0400271F RID: 10015
		public static readonly OpCode Localloc = new OpCode("localloc", StackBehaviour.Popi, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 2, 254, 15, FlowControl.Next, false, 0);

		// Token: 0x04002720 RID: 10016
		public static readonly OpCode Endfilter = new OpCode("endfilter", StackBehaviour.Popi, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Primitive, 2, 254, 17, FlowControl.Return, true, -1);

		// Token: 0x04002721 RID: 10017
		public static readonly OpCode Unaligned = new OpCode("unaligned.", StackBehaviour.Pop0, StackBehaviour.Push0, OperandType.ShortInlineI, OpCodeType.Prefix, 2, 254, 18, FlowControl.Meta, false, 0);

		// Token: 0x04002722 RID: 10018
		public static readonly OpCode Volatile = new OpCode("volatile.", StackBehaviour.Pop0, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Prefix, 2, 254, 19, FlowControl.Meta, false, 0);

		// Token: 0x04002723 RID: 10019
		public static readonly OpCode Tailcall = new OpCode("tail.", StackBehaviour.Pop0, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Prefix, 2, 254, 20, FlowControl.Meta, false, 0);

		// Token: 0x04002724 RID: 10020
		public static readonly OpCode Initobj = new OpCode("initobj", StackBehaviour.Popi, StackBehaviour.Push0, OperandType.InlineType, OpCodeType.Objmodel, 2, 254, 21, FlowControl.Next, false, -1);

		// Token: 0x04002725 RID: 10021
		public static readonly OpCode Constrained = new OpCode("constrained.", StackBehaviour.Pop0, StackBehaviour.Push0, OperandType.InlineType, OpCodeType.Prefix, 2, 254, 22, FlowControl.Meta, false, 0);

		// Token: 0x04002726 RID: 10022
		public static readonly OpCode Cpblk = new OpCode("cpblk", StackBehaviour.Popi_popi_popi, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Primitive, 2, 254, 23, FlowControl.Next, false, -3);

		// Token: 0x04002727 RID: 10023
		public static readonly OpCode Initblk = new OpCode("initblk", StackBehaviour.Popi_popi_popi, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Primitive, 2, 254, 24, FlowControl.Next, false, -3);

		// Token: 0x04002728 RID: 10024
		public static readonly OpCode Rethrow = new OpCode("rethrow", StackBehaviour.Pop0, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Objmodel, 2, 254, 26, FlowControl.Throw, true, 0);

		// Token: 0x04002729 RID: 10025
		public static readonly OpCode Sizeof = new OpCode("sizeof", StackBehaviour.Pop0, StackBehaviour.Pushi, OperandType.InlineType, OpCodeType.Primitive, 2, 254, 28, FlowControl.Next, false, 1);

		// Token: 0x0400272A RID: 10026
		public static readonly OpCode Refanytype = new OpCode("refanytype", StackBehaviour.Pop1, StackBehaviour.Pushi, OperandType.InlineNone, OpCodeType.Primitive, 2, 254, 29, FlowControl.Next, false, 0);

		// Token: 0x0400272B RID: 10027
		public static readonly OpCode Readonly = new OpCode("readonly.", StackBehaviour.Pop0, StackBehaviour.Push0, OperandType.InlineNone, OpCodeType.Prefix, 2, 254, 30, FlowControl.Meta, false, 0);
	}
}
