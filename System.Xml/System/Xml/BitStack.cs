using System;

namespace System.Xml
{
	internal class BitStack
	{
		public BitStack()
		{
			this.curr = 1U;
		}

		public void PushBit(bool bit)
		{
			if ((this.curr & 2147483648U) != 0U)
			{
				this.PushCurr();
			}
			this.curr = (this.curr << 1) | (bit ? 1U : 0U);
		}

		public bool PopBit()
		{
			bool flag = (this.curr & 1U) != 0U;
			this.curr >>= 1;
			if (this.curr == 1U)
			{
				this.PopCurr();
			}
			return flag;
		}

		public bool PeekBit()
		{
			return (this.curr & 1U) != 0U;
		}

		public bool IsEmpty
		{
			get
			{
				return this.curr == 1U;
			}
		}

		private void PushCurr()
		{
			if (this.bitStack == null)
			{
				this.bitStack = new uint[16];
			}
			this.bitStack[this.stackPos++] = this.curr;
			this.curr = 1U;
			int num = this.bitStack.Length;
			if (this.stackPos >= num)
			{
				uint[] array = new uint[2 * num];
				Array.Copy(this.bitStack, array, num);
				this.bitStack = array;
			}
		}

		private void PopCurr()
		{
			if (this.stackPos > 0)
			{
				this.curr = this.bitStack[--this.stackPos];
			}
		}

		private uint[] bitStack;

		private int stackPos;

		private uint curr;
	}
}
