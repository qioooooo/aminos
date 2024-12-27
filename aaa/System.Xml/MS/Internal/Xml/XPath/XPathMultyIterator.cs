using System;
using System.Collections;
using System.Xml;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000169 RID: 361
	internal class XPathMultyIterator : ResetableIterator
	{
		// Token: 0x06001357 RID: 4951 RVA: 0x0005379C File Offset: 0x0005279C
		public XPathMultyIterator(ArrayList inputArray)
		{
			this.arr = new ResetableIterator[inputArray.Count];
			for (int i = 0; i < this.arr.Length; i++)
			{
				this.arr[i] = new XPathArrayIterator((ArrayList)inputArray[i]);
			}
			this.Init();
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x000537F4 File Offset: 0x000527F4
		private void Init()
		{
			for (int i = 0; i < this.arr.Length; i++)
			{
				this.Advance(i);
			}
			int num = this.arr.Length - 2;
			while (this.firstNotEmpty <= num)
			{
				if (this.SiftItem(num))
				{
					num--;
				}
			}
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x00053840 File Offset: 0x00052840
		private bool Advance(int pos)
		{
			if (!this.arr[pos].MoveNext())
			{
				if (this.firstNotEmpty != pos)
				{
					ResetableIterator resetableIterator = this.arr[pos];
					Array.Copy(this.arr, this.firstNotEmpty, this.arr, this.firstNotEmpty + 1, pos - this.firstNotEmpty);
					this.arr[this.firstNotEmpty] = resetableIterator;
				}
				this.firstNotEmpty++;
				return false;
			}
			return true;
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x000538B4 File Offset: 0x000528B4
		private bool SiftItem(int item)
		{
			ResetableIterator resetableIterator = this.arr[item];
			while (item + 1 < this.arr.Length)
			{
				XmlNodeOrder xmlNodeOrder = Query.CompareNodes(resetableIterator.Current, this.arr[item + 1].Current);
				if (xmlNodeOrder == XmlNodeOrder.Before)
				{
					break;
				}
				if (xmlNodeOrder == XmlNodeOrder.After)
				{
					this.arr[item] = this.arr[item + 1];
					item++;
				}
				else
				{
					this.arr[item] = resetableIterator;
					if (!this.Advance(item))
					{
						return false;
					}
					resetableIterator = this.arr[item];
				}
			}
			this.arr[item] = resetableIterator;
			return true;
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x0005393C File Offset: 0x0005293C
		public override void Reset()
		{
			this.firstNotEmpty = 0;
			this.position = 0;
			for (int i = 0; i < this.arr.Length; i++)
			{
				this.arr[i].Reset();
			}
			this.Init();
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x0005397D File Offset: 0x0005297D
		public XPathMultyIterator(XPathMultyIterator it)
		{
			this.arr = (ResetableIterator[])it.arr.Clone();
			this.firstNotEmpty = it.firstNotEmpty;
			this.position = it.position;
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x000539B3 File Offset: 0x000529B3
		public override XPathNodeIterator Clone()
		{
			return new XPathMultyIterator(this);
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x0600135E RID: 4958 RVA: 0x000539BB File Offset: 0x000529BB
		public override XPathNavigator Current
		{
			get
			{
				return this.arr[this.firstNotEmpty].Current;
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x0600135F RID: 4959 RVA: 0x000539CF File Offset: 0x000529CF
		public override int CurrentPosition
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x06001360 RID: 4960 RVA: 0x000539D8 File Offset: 0x000529D8
		public override bool MoveNext()
		{
			if (this.firstNotEmpty >= this.arr.Length)
			{
				return false;
			}
			if (this.position != 0)
			{
				if (this.Advance(this.firstNotEmpty))
				{
					this.SiftItem(this.firstNotEmpty);
				}
				if (this.firstNotEmpty >= this.arr.Length)
				{
					return false;
				}
			}
			this.position++;
			return true;
		}

		// Token: 0x04000BF0 RID: 3056
		protected ResetableIterator[] arr;

		// Token: 0x04000BF1 RID: 3057
		protected int firstNotEmpty;

		// Token: 0x04000BF2 RID: 3058
		protected int position;
	}
}
