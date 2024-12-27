using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x02000324 RID: 804
	[ComVisible(true)]
	[Serializable]
	public struct ParameterModifier
	{
		// Token: 0x06001F5D RID: 8029 RVA: 0x0004F62F File Offset: 0x0004E62F
		public ParameterModifier(int parameterCount)
		{
			if (parameterCount <= 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_ParmArraySize"));
			}
			this._byRef = new bool[parameterCount];
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06001F5E RID: 8030 RVA: 0x0004F651 File Offset: 0x0004E651
		internal bool[] IsByRefArray
		{
			get
			{
				return this._byRef;
			}
		}

		// Token: 0x17000541 RID: 1345
		public bool this[int index]
		{
			get
			{
				return this._byRef[index];
			}
			set
			{
				this._byRef[index] = value;
			}
		}

		// Token: 0x04000D5C RID: 3420
		private bool[] _byRef;
	}
}
