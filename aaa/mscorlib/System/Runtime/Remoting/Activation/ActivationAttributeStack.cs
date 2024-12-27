using System;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x0200068E RID: 1678
	internal class ActivationAttributeStack
	{
		// Token: 0x06003D1B RID: 15643 RVA: 0x000D1EA7 File Offset: 0x000D0EA7
		internal ActivationAttributeStack()
		{
			this.activationTypes = new object[4];
			this.activationAttributes = new object[4];
			this.freeIndex = 0;
		}

		// Token: 0x06003D1C RID: 15644 RVA: 0x000D1ED0 File Offset: 0x000D0ED0
		internal void Push(Type typ, object[] attr)
		{
			if (this.freeIndex == this.activationTypes.Length)
			{
				object[] array = new object[this.activationTypes.Length * 2];
				object[] array2 = new object[this.activationAttributes.Length * 2];
				Array.Copy(this.activationTypes, array, this.activationTypes.Length);
				Array.Copy(this.activationAttributes, array2, this.activationAttributes.Length);
				this.activationTypes = array;
				this.activationAttributes = array2;
			}
			this.activationTypes[this.freeIndex] = typ;
			this.activationAttributes[this.freeIndex] = attr;
			this.freeIndex++;
		}

		// Token: 0x06003D1D RID: 15645 RVA: 0x000D1F6D File Offset: 0x000D0F6D
		internal object[] Peek(Type typ)
		{
			if (this.freeIndex == 0 || this.activationTypes[this.freeIndex - 1] != typ)
			{
				return null;
			}
			return (object[])this.activationAttributes[this.freeIndex - 1];
		}

		// Token: 0x06003D1E RID: 15646 RVA: 0x000D1FA0 File Offset: 0x000D0FA0
		internal void Pop(Type typ)
		{
			if (this.freeIndex != 0 && this.activationTypes[this.freeIndex - 1] == typ)
			{
				this.freeIndex--;
				this.activationTypes[this.freeIndex] = null;
				this.activationAttributes[this.freeIndex] = null;
			}
		}

		// Token: 0x04001F1C RID: 7964
		private object[] activationTypes;

		// Token: 0x04001F1D RID: 7965
		private object[] activationAttributes;

		// Token: 0x04001F1E RID: 7966
		private int freeIndex;
	}
}
