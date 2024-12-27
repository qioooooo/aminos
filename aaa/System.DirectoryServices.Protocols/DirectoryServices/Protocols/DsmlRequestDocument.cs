using System;
using System.Collections;
using System.ComponentModel;
using System.Xml;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200005E RID: 94
	public class DsmlRequestDocument : DsmlDocument, IList, ICollection, IEnumerable
	{
		// Token: 0x060001B6 RID: 438 RVA: 0x00007C38 File Offset: 0x00006C38
		public DsmlRequestDocument()
		{
			Utility.CheckOSVersion();
			this.dsmlRequests = new ArrayList();
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00007C57 File Offset: 0x00006C57
		// (set) Token: 0x060001B8 RID: 440 RVA: 0x00007C5F File Offset: 0x00006C5F
		public DsmlDocumentProcessing DocumentProcessing
		{
			get
			{
				return this.docProcessing;
			}
			set
			{
				if (value < DsmlDocumentProcessing.Sequential || value > DsmlDocumentProcessing.Parallel)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DsmlDocumentProcessing));
				}
				this.docProcessing = value;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x00007C86 File Offset: 0x00006C86
		// (set) Token: 0x060001BA RID: 442 RVA: 0x00007C8E File Offset: 0x00006C8E
		public DsmlResponseOrder ResponseOrder
		{
			get
			{
				return this.resOrder;
			}
			set
			{
				if (value < DsmlResponseOrder.Sequential || value > DsmlResponseOrder.Unordered)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DsmlResponseOrder));
				}
				this.resOrder = value;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001BB RID: 443 RVA: 0x00007CB5 File Offset: 0x00006CB5
		// (set) Token: 0x060001BC RID: 444 RVA: 0x00007CBD File Offset: 0x00006CBD
		public DsmlErrorProcessing ErrorProcessing
		{
			get
			{
				return this.errProcessing;
			}
			set
			{
				if (value < DsmlErrorProcessing.Resume || value > DsmlErrorProcessing.Exit)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DsmlErrorProcessing));
				}
				this.errProcessing = value;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001BD RID: 445 RVA: 0x00007CE4 File Offset: 0x00006CE4
		// (set) Token: 0x060001BE RID: 446 RVA: 0x00007CEC File Offset: 0x00006CEC
		public string RequestId
		{
			get
			{
				return this.dsmlRequestID;
			}
			set
			{
				this.dsmlRequestID = value;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001BF RID: 447 RVA: 0x00007CF5 File Offset: 0x00006CF5
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x00007CF8 File Offset: 0x00006CF8
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x00007CFB File Offset: 0x00006CFB
		object ICollection.SyncRoot
		{
			get
			{
				return this.dsmlRequests.SyncRoot;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x00007D08 File Offset: 0x00006D08
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.dsmlRequests.IsSynchronized;
			}
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00007D15 File Offset: 0x00006D15
		public IEnumerator GetEnumerator()
		{
			return this.dsmlRequests.GetEnumerator();
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x00007D22 File Offset: 0x00006D22
		protected bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x00007D25 File Offset: 0x00006D25
		protected bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x00007D28 File Offset: 0x00006D28
		protected object SyncRoot
		{
			get
			{
				return this.dsmlRequests.SyncRoot;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x00007D35 File Offset: 0x00006D35
		protected bool IsSynchronized
		{
			get
			{
				return this.dsmlRequests.IsSynchronized;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x00007D42 File Offset: 0x00006D42
		public int Count
		{
			get
			{
				return this.dsmlRequests.Count;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x00007D4F File Offset: 0x00006D4F
		int ICollection.Count
		{
			get
			{
				return this.dsmlRequests.Count;
			}
		}

		// Token: 0x17000083 RID: 131
		public DirectoryRequest this[int index]
		{
			get
			{
				return (DirectoryRequest)this.dsmlRequests[index];
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.dsmlRequests[index] = value;
			}
		}

		// Token: 0x17000084 RID: 132
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (!(value is DirectoryRequest))
				{
					throw new ArgumentException(Res.GetString("InvalidValueType", new object[] { "DirectoryRequest" }), "value");
				}
				this.dsmlRequests[index] = (DirectoryRequest)value;
			}
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00007DF2 File Offset: 0x00006DF2
		public int Add(DirectoryRequest request)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}
			return this.dsmlRequests.Add(request);
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00007E10 File Offset: 0x00006E10
		int IList.Add(object request)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}
			if (!(request is DirectoryRequest))
			{
				throw new ArgumentException(Res.GetString("InvalidValueType", new object[] { "DirectoryRequest" }), "request");
			}
			return this.Add((DirectoryRequest)request);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00007E64 File Offset: 0x00006E64
		public void Clear()
		{
			this.dsmlRequests.Clear();
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00007E71 File Offset: 0x00006E71
		void IList.Clear()
		{
			this.Clear();
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00007E79 File Offset: 0x00006E79
		public bool Contains(DirectoryRequest value)
		{
			return this.dsmlRequests.Contains(value);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00007E87 File Offset: 0x00006E87
		bool IList.Contains(object value)
		{
			return this.Contains((DirectoryRequest)value);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00007E95 File Offset: 0x00006E95
		public int IndexOf(DirectoryRequest value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return this.dsmlRequests.IndexOf(value);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00007EB1 File Offset: 0x00006EB1
		int IList.IndexOf(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return this.IndexOf((DirectoryRequest)value);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00007ECD File Offset: 0x00006ECD
		public void Insert(int index, DirectoryRequest value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.dsmlRequests.Insert(index, value);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00007EEC File Offset: 0x00006EEC
		void IList.Insert(int index, object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!(value is DirectoryRequest))
			{
				throw new ArgumentException(Res.GetString("InvalidValueType", new object[] { "DirectoryRequest" }), "value");
			}
			this.Insert(index, (DirectoryRequest)value);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00007F41 File Offset: 0x00006F41
		public void Remove(DirectoryRequest value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.dsmlRequests.Remove(value);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00007F5D File Offset: 0x00006F5D
		void IList.Remove(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.Remove((DirectoryRequest)value);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00007F79 File Offset: 0x00006F79
		public void RemoveAt(int index)
		{
			this.dsmlRequests.RemoveAt(index);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00007F87 File Offset: 0x00006F87
		void IList.RemoveAt(int index)
		{
			this.RemoveAt(index);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00007F90 File Offset: 0x00006F90
		public void CopyTo(DirectoryRequest[] value, int i)
		{
			this.dsmlRequests.CopyTo(value, i);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00007F9F File Offset: 0x00006F9F
		void ICollection.CopyTo(Array value, int i)
		{
			this.dsmlRequests.CopyTo(value, i);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00007FB0 File Offset: 0x00006FB0
		public override XmlDocument ToXml()
		{
			XmlDocument xmlDocument = new XmlDocument();
			this.StartBatchRequest(xmlDocument);
			foreach (object obj in this.dsmlRequests)
			{
				DirectoryRequest directoryRequest = (DirectoryRequest)obj;
				xmlDocument.DocumentElement.AppendChild(directoryRequest.ToXmlNodeHelper(xmlDocument));
			}
			return xmlDocument;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00008024 File Offset: 0x00007024
		private void StartBatchRequest(XmlDocument xmldoc)
		{
			string text = "<batchRequest xmlns=\"urn:oasis:names:tc:DSML:2:0:core\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" />";
			xmldoc.LoadXml(text);
			XmlAttribute xmlAttribute = xmldoc.CreateAttribute("processing", null);
			switch (this.docProcessing)
			{
			case DsmlDocumentProcessing.Sequential:
				xmlAttribute.InnerText = "sequential";
				break;
			case DsmlDocumentProcessing.Parallel:
				xmlAttribute.InnerText = "parallel";
				break;
			}
			xmldoc.DocumentElement.Attributes.Append(xmlAttribute);
			xmlAttribute = xmldoc.CreateAttribute("responseOrder", null);
			switch (this.resOrder)
			{
			case DsmlResponseOrder.Sequential:
				xmlAttribute.InnerText = "sequential";
				break;
			case DsmlResponseOrder.Unordered:
				xmlAttribute.InnerText = "unordered";
				break;
			}
			xmldoc.DocumentElement.Attributes.Append(xmlAttribute);
			xmlAttribute = xmldoc.CreateAttribute("onError", null);
			switch (this.errProcessing)
			{
			case DsmlErrorProcessing.Resume:
				xmlAttribute.InnerText = "resume";
				break;
			case DsmlErrorProcessing.Exit:
				xmlAttribute.InnerText = "exit";
				break;
			}
			xmldoc.DocumentElement.Attributes.Append(xmlAttribute);
			if (this.dsmlRequestID != null)
			{
				xmlAttribute = xmldoc.CreateAttribute("requestID", null);
				xmlAttribute.InnerText = this.dsmlRequestID;
				xmldoc.DocumentElement.Attributes.Append(xmlAttribute);
			}
		}

		// Token: 0x040001E0 RID: 480
		private DsmlDocumentProcessing docProcessing;

		// Token: 0x040001E1 RID: 481
		private DsmlResponseOrder resOrder;

		// Token: 0x040001E2 RID: 482
		private DsmlErrorProcessing errProcessing = DsmlErrorProcessing.Exit;

		// Token: 0x040001E3 RID: 483
		private ArrayList dsmlRequests;
	}
}
