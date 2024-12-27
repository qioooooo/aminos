using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000193 RID: 403
	internal class StateMachine
	{
		// Token: 0x06001143 RID: 4419 RVA: 0x000533F7 File Offset: 0x000523F7
		internal StateMachine()
		{
			this._State = 0;
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06001144 RID: 4420 RVA: 0x00053406 File Offset: 0x00052406
		// (set) Token: 0x06001145 RID: 4421 RVA: 0x0005340E File Offset: 0x0005240E
		internal int State
		{
			get
			{
				return this._State;
			}
			set
			{
				this._State = value;
			}
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x00053417 File Offset: 0x00052417
		internal void Reset()
		{
			this._State = 0;
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x00053420 File Offset: 0x00052420
		internal static int StateOnly(int state)
		{
			return state & 15;
		}

		// Token: 0x06001148 RID: 4424 RVA: 0x00053428 File Offset: 0x00052428
		internal int BeginOutlook(XPathNodeType nodeType)
		{
			return StateMachine.s_BeginTransitions[(int)nodeType][this._State];
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x00053448 File Offset: 0x00052448
		internal int Begin(XPathNodeType nodeType)
		{
			int num = StateMachine.s_BeginTransitions[(int)nodeType][this._State];
			if (num != 16 && num != 32)
			{
				this._State = num & 15;
			}
			return num;
		}

		// Token: 0x0600114A RID: 4426 RVA: 0x0005347C File Offset: 0x0005247C
		internal int EndOutlook(XPathNodeType nodeType)
		{
			return StateMachine.s_EndTransitions[(int)nodeType][this._State];
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x0005349C File Offset: 0x0005249C
		internal int End(XPathNodeType nodeType)
		{
			int num = StateMachine.s_EndTransitions[(int)nodeType][this._State];
			if (num != 16 && num != 32)
			{
				this._State = num & 15;
			}
			return num;
		}

		// Token: 0x04000B8F RID: 2959
		private const int Init = 0;

		// Token: 0x04000B90 RID: 2960
		private const int Elem = 1;

		// Token: 0x04000B91 RID: 2961
		private const int NsN = 2;

		// Token: 0x04000B92 RID: 2962
		private const int NsV = 3;

		// Token: 0x04000B93 RID: 2963
		private const int Ns = 4;

		// Token: 0x04000B94 RID: 2964
		private const int AttrN = 5;

		// Token: 0x04000B95 RID: 2965
		private const int AttrV = 6;

		// Token: 0x04000B96 RID: 2966
		private const int Attr = 7;

		// Token: 0x04000B97 RID: 2967
		private const int InElm = 8;

		// Token: 0x04000B98 RID: 2968
		private const int EndEm = 9;

		// Token: 0x04000B99 RID: 2969
		private const int InCmt = 10;

		// Token: 0x04000B9A RID: 2970
		private const int InPI = 11;

		// Token: 0x04000B9B RID: 2971
		private const int StateMask = 15;

		// Token: 0x04000B9C RID: 2972
		internal const int Error = 16;

		// Token: 0x04000B9D RID: 2973
		private const int Ignor = 32;

		// Token: 0x04000B9E RID: 2974
		private const int Assrt = 48;

		// Token: 0x04000B9F RID: 2975
		private const int U = 256;

		// Token: 0x04000BA0 RID: 2976
		private const int D = 512;

		// Token: 0x04000BA1 RID: 2977
		internal const int DepthMask = 768;

		// Token: 0x04000BA2 RID: 2978
		internal const int DepthUp = 256;

		// Token: 0x04000BA3 RID: 2979
		internal const int DepthDown = 512;

		// Token: 0x04000BA4 RID: 2980
		private const int C = 1024;

		// Token: 0x04000BA5 RID: 2981
		private const int H = 2048;

		// Token: 0x04000BA6 RID: 2982
		private const int M = 4096;

		// Token: 0x04000BA7 RID: 2983
		internal const int BeginChild = 1024;

		// Token: 0x04000BA8 RID: 2984
		internal const int HadChild = 2048;

		// Token: 0x04000BA9 RID: 2985
		internal const int EmptyTag = 4096;

		// Token: 0x04000BAA RID: 2986
		private const int B = 8192;

		// Token: 0x04000BAB RID: 2987
		private const int E = 16384;

		// Token: 0x04000BAC RID: 2988
		internal const int BeginRecord = 8192;

		// Token: 0x04000BAD RID: 2989
		internal const int EndRecord = 16384;

		// Token: 0x04000BAE RID: 2990
		private const int S = 32768;

		// Token: 0x04000BAF RID: 2991
		private const int P = 65536;

		// Token: 0x04000BB0 RID: 2992
		internal const int PushScope = 32768;

		// Token: 0x04000BB1 RID: 2993
		internal const int PopScope = 65536;

		// Token: 0x04000BB2 RID: 2994
		private int _State;

		// Token: 0x04000BB3 RID: 2995
		private static readonly int[][] s_BeginTransitions = new int[][]
		{
			new int[]
			{
				16, 16, 16, 16, 16, 16, 16, 16, 16, 16,
				16, 16
			},
			new int[]
			{
				40961, 42241, 16, 16, 41985, 16, 16, 41985, 40961, 106497,
				16, 16
			},
			new int[]
			{
				16, 261, 16, 16, 5, 16, 16, 5, 16, 16,
				16, 16
			},
			new int[]
			{
				16, 258, 16, 16, 2, 16, 16, 16, 16, 16,
				16, 16
			},
			new int[]
			{
				8200, 9480, 259, 3, 9224, 262, 6, 9224, 8, 73736,
				10, 11
			},
			new int[]
			{
				8200, 9480, 259, 3, 9224, 262, 6, 9224, 8, 73736,
				10, 11
			},
			new int[]
			{
				8200, 9480, 259, 3, 9224, 262, 6, 9224, 8, 73736,
				10, 11
			},
			new int[]
			{
				8203, 9483, 16, 16, 9227, 16, 16, 9227, 8203, 73739,
				16, 16
			},
			new int[]
			{
				8202, 9482, 16, 16, 9226, 16, 16, 9226, 8202, 73738,
				16, 16
			},
			new int[]
			{
				16, 16, 16, 16, 16, 16, 16, 16, 16, 16,
				16, 16
			}
		};

		// Token: 0x04000BB4 RID: 2996
		private static readonly int[][] s_EndTransitions = new int[][]
		{
			new int[]
			{
				48, 48, 48, 48, 48, 48, 48, 48, 48, 48,
				48, 48
			},
			new int[]
			{
				48, 94217, 48, 48, 94729, 48, 48, 94729, 92681, 92681,
				48, 48
			},
			new int[]
			{
				48, 48, 48, 48, 48, 7, 519, 48, 48, 48,
				48, 48
			},
			new int[]
			{
				48, 48, 4, 516, 48, 48, 48, 48, 48, 48,
				48, 48
			},
			new int[]
			{
				48, 48, 48, 48, 48, 48, 48, 48, 48, 48,
				48, 48
			},
			new int[]
			{
				48, 48, 48, 48, 48, 48, 48, 48, 48, 48,
				48, 48
			},
			new int[]
			{
				48, 48, 48, 48, 48, 48, 48, 48, 48, 48,
				48, 48
			},
			new int[]
			{
				48, 48, 48, 48, 48, 48, 48, 48, 48, 48,
				48, 16393
			},
			new int[]
			{
				48, 48, 48, 48, 48, 48, 48, 48, 48, 48,
				16393, 48
			},
			new int[]
			{
				48, 48, 48, 48, 48, 48, 48, 48, 48, 48,
				48, 48
			}
		};
	}
}
