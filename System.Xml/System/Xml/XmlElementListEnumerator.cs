using System;
using System.Collections;

namespace System.Xml
{
	internal class XmlElementListEnumerator : IEnumerator
	{
		public XmlElementListEnumerator(XmlElementList list)
		{
			this.list = list;
			this.curElem = null;
			this.changeCount = list.ChangeCount;
		}

		public bool MoveNext()
		{
			if (this.list.ChangeCount != this.changeCount)
			{
				throw new InvalidOperationException(Res.GetString("Xdom_Enum_ElementList"));
			}
			this.curElem = this.list.GetNextNode(this.curElem);
			return this.curElem != null;
		}

		public void Reset()
		{
			this.curElem = null;
			this.changeCount = this.list.ChangeCount;
		}

		public object Current
		{
			get
			{
				return this.curElem;
			}
		}

		private XmlElementList list;

		private XmlNode curElem;

		private int changeCount;
	}
}
