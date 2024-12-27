using System;

namespace System.Reflection.Emit
{
	// Token: 0x02000812 RID: 2066
	internal class __ExceptionInfo
	{
		// Token: 0x06004A03 RID: 18947 RVA: 0x00101EE4 File Offset: 0x00100EE4
		private __ExceptionInfo()
		{
			this.m_startAddr = 0;
			this.m_filterAddr = null;
			this.m_catchAddr = null;
			this.m_catchEndAddr = null;
			this.m_endAddr = 0;
			this.m_currentCatch = 0;
			this.m_type = null;
			this.m_endFinally = -1;
			this.m_currentState = 0;
		}

		// Token: 0x06004A04 RID: 18948 RVA: 0x00101F38 File Offset: 0x00100F38
		internal __ExceptionInfo(int startAddr, Label endLabel)
		{
			this.m_startAddr = startAddr;
			this.m_endAddr = -1;
			this.m_filterAddr = new int[4];
			this.m_catchAddr = new int[4];
			this.m_catchEndAddr = new int[4];
			this.m_catchClass = new Type[4];
			this.m_currentCatch = 0;
			this.m_endLabel = endLabel;
			this.m_type = new int[4];
			this.m_endFinally = -1;
			this.m_currentState = 0;
		}

		// Token: 0x06004A05 RID: 18949 RVA: 0x00101FB4 File Offset: 0x00100FB4
		private void MarkHelper(int catchorfilterAddr, int catchEndAddr, Type catchClass, int type)
		{
			if (this.m_currentCatch >= this.m_catchAddr.Length)
			{
				this.m_filterAddr = ILGenerator.EnlargeArray(this.m_filterAddr);
				this.m_catchAddr = ILGenerator.EnlargeArray(this.m_catchAddr);
				this.m_catchEndAddr = ILGenerator.EnlargeArray(this.m_catchEndAddr);
				this.m_catchClass = ILGenerator.EnlargeArray(this.m_catchClass);
				this.m_type = ILGenerator.EnlargeArray(this.m_type);
			}
			if (type == 1)
			{
				this.m_type[this.m_currentCatch] = type;
				this.m_filterAddr[this.m_currentCatch] = catchorfilterAddr;
				this.m_catchAddr[this.m_currentCatch] = -1;
				if (this.m_currentCatch > 0)
				{
					this.m_catchEndAddr[this.m_currentCatch - 1] = catchorfilterAddr;
				}
			}
			else
			{
				this.m_catchClass[this.m_currentCatch] = catchClass;
				if (this.m_type[this.m_currentCatch] != 1)
				{
					this.m_type[this.m_currentCatch] = type;
				}
				this.m_catchAddr[this.m_currentCatch] = catchorfilterAddr;
				if (this.m_currentCatch > 0 && this.m_type[this.m_currentCatch] != 1)
				{
					this.m_catchEndAddr[this.m_currentCatch - 1] = catchEndAddr;
				}
				this.m_catchEndAddr[this.m_currentCatch] = -1;
				this.m_currentCatch++;
			}
			if (this.m_endAddr == -1)
			{
				this.m_endAddr = catchorfilterAddr;
			}
		}

		// Token: 0x06004A06 RID: 18950 RVA: 0x00102107 File Offset: 0x00101107
		internal virtual void MarkFilterAddr(int filterAddr)
		{
			this.m_currentState = 1;
			this.MarkHelper(filterAddr, filterAddr, null, 1);
		}

		// Token: 0x06004A07 RID: 18951 RVA: 0x0010211A File Offset: 0x0010111A
		internal virtual void MarkFaultAddr(int faultAddr)
		{
			this.m_currentState = 4;
			this.MarkHelper(faultAddr, faultAddr, null, 4);
		}

		// Token: 0x06004A08 RID: 18952 RVA: 0x0010212D File Offset: 0x0010112D
		internal virtual void MarkCatchAddr(int catchAddr, Type catchException)
		{
			this.m_currentState = 2;
			this.MarkHelper(catchAddr, catchAddr, catchException, 0);
		}

		// Token: 0x06004A09 RID: 18953 RVA: 0x00102140 File Offset: 0x00101140
		internal virtual void MarkFinallyAddr(int finallyAddr, int endCatchAddr)
		{
			if (this.m_endFinally != -1)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_TooManyFinallyClause"));
			}
			this.m_currentState = 3;
			this.m_endFinally = finallyAddr;
			this.MarkHelper(finallyAddr, endCatchAddr, null, 2);
		}

		// Token: 0x06004A0A RID: 18954 RVA: 0x00102173 File Offset: 0x00101173
		internal virtual void Done(int endAddr)
		{
			this.m_catchEndAddr[this.m_currentCatch - 1] = endAddr;
			this.m_currentState = 5;
		}

		// Token: 0x06004A0B RID: 18955 RVA: 0x0010218C File Offset: 0x0010118C
		internal virtual int GetStartAddress()
		{
			return this.m_startAddr;
		}

		// Token: 0x06004A0C RID: 18956 RVA: 0x00102194 File Offset: 0x00101194
		internal virtual int GetEndAddress()
		{
			return this.m_endAddr;
		}

		// Token: 0x06004A0D RID: 18957 RVA: 0x0010219C File Offset: 0x0010119C
		internal virtual int GetFinallyEndAddress()
		{
			return this.m_endFinally;
		}

		// Token: 0x06004A0E RID: 18958 RVA: 0x001021A4 File Offset: 0x001011A4
		internal virtual Label GetEndLabel()
		{
			return this.m_endLabel;
		}

		// Token: 0x06004A0F RID: 18959 RVA: 0x001021AC File Offset: 0x001011AC
		internal virtual int[] GetFilterAddresses()
		{
			return this.m_filterAddr;
		}

		// Token: 0x06004A10 RID: 18960 RVA: 0x001021B4 File Offset: 0x001011B4
		internal virtual int[] GetCatchAddresses()
		{
			return this.m_catchAddr;
		}

		// Token: 0x06004A11 RID: 18961 RVA: 0x001021BC File Offset: 0x001011BC
		internal virtual int[] GetCatchEndAddresses()
		{
			return this.m_catchEndAddr;
		}

		// Token: 0x06004A12 RID: 18962 RVA: 0x001021C4 File Offset: 0x001011C4
		internal virtual Type[] GetCatchClass()
		{
			return this.m_catchClass;
		}

		// Token: 0x06004A13 RID: 18963 RVA: 0x001021CC File Offset: 0x001011CC
		internal virtual int GetNumberOfCatches()
		{
			return this.m_currentCatch;
		}

		// Token: 0x06004A14 RID: 18964 RVA: 0x001021D4 File Offset: 0x001011D4
		internal virtual int[] GetExceptionTypes()
		{
			return this.m_type;
		}

		// Token: 0x06004A15 RID: 18965 RVA: 0x001021DC File Offset: 0x001011DC
		internal virtual void SetFinallyEndLabel(Label lbl)
		{
			this.m_finallyEndLabel = lbl;
		}

		// Token: 0x06004A16 RID: 18966 RVA: 0x001021E5 File Offset: 0x001011E5
		internal virtual Label GetFinallyEndLabel()
		{
			return this.m_finallyEndLabel;
		}

		// Token: 0x06004A17 RID: 18967 RVA: 0x001021F0 File Offset: 0x001011F0
		internal bool IsInner(__ExceptionInfo exc)
		{
			int num = exc.m_currentCatch - 1;
			int num2 = this.m_currentCatch - 1;
			return exc.m_catchEndAddr[num] < this.m_catchEndAddr[num2] || (exc.m_catchEndAddr[num] == this.m_catchEndAddr[num2] && exc.GetEndAddress() > this.GetEndAddress());
		}

		// Token: 0x06004A18 RID: 18968 RVA: 0x00102246 File Offset: 0x00101246
		internal virtual int GetCurrentState()
		{
			return this.m_currentState;
		}

		// Token: 0x040025A3 RID: 9635
		internal const int None = 0;

		// Token: 0x040025A4 RID: 9636
		internal const int Filter = 1;

		// Token: 0x040025A5 RID: 9637
		internal const int Finally = 2;

		// Token: 0x040025A6 RID: 9638
		internal const int Fault = 4;

		// Token: 0x040025A7 RID: 9639
		internal const int PreserveStack = 4;

		// Token: 0x040025A8 RID: 9640
		internal const int State_Try = 0;

		// Token: 0x040025A9 RID: 9641
		internal const int State_Filter = 1;

		// Token: 0x040025AA RID: 9642
		internal const int State_Catch = 2;

		// Token: 0x040025AB RID: 9643
		internal const int State_Finally = 3;

		// Token: 0x040025AC RID: 9644
		internal const int State_Fault = 4;

		// Token: 0x040025AD RID: 9645
		internal const int State_Done = 5;

		// Token: 0x040025AE RID: 9646
		internal int m_startAddr;

		// Token: 0x040025AF RID: 9647
		internal int[] m_filterAddr;

		// Token: 0x040025B0 RID: 9648
		internal int[] m_catchAddr;

		// Token: 0x040025B1 RID: 9649
		internal int[] m_catchEndAddr;

		// Token: 0x040025B2 RID: 9650
		internal int[] m_type;

		// Token: 0x040025B3 RID: 9651
		internal Type[] m_catchClass;

		// Token: 0x040025B4 RID: 9652
		internal Label m_endLabel;

		// Token: 0x040025B5 RID: 9653
		internal Label m_finallyEndLabel;

		// Token: 0x040025B6 RID: 9654
		internal int m_endAddr;

		// Token: 0x040025B7 RID: 9655
		internal int m_endFinally;

		// Token: 0x040025B8 RID: 9656
		internal int m_currentCatch;

		// Token: 0x040025B9 RID: 9657
		private int m_currentState;
	}
}
