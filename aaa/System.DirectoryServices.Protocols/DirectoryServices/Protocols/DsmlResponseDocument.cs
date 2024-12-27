using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200005F RID: 95
	public class DsmlResponseDocument : DsmlDocument, ICollection, IEnumerable
	{
		// Token: 0x060001E0 RID: 480 RVA: 0x0000815D File Offset: 0x0000715D
		private DsmlResponseDocument()
		{
			this.dsmlResponse = new ArrayList();
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00008170 File Offset: 0x00007170
		internal DsmlResponseDocument(HttpWebResponse resp, string xpathToResponse)
			: this()
		{
			Stream responseStream = resp.GetResponseStream();
			StreamReader streamReader = new StreamReader(responseStream);
			try
			{
				this.dsmlDocument = new XmlDocument();
				try
				{
					this.dsmlDocument.Load(streamReader);
				}
				catch (XmlException)
				{
					throw new DsmlInvalidDocumentException(Res.GetString("NotWellFormedResponse"));
				}
				this.dsmlNS = NamespaceUtils.GetDsmlNamespaceManager();
				this.dsmlBatchResponse = (XmlElement)this.dsmlDocument.SelectSingleNode(xpathToResponse, this.dsmlNS);
				if (this.dsmlBatchResponse == null)
				{
					throw new DsmlInvalidDocumentException(Res.GetString("NotWellFormedResponse"));
				}
				XmlNodeList childNodes = this.dsmlBatchResponse.ChildNodes;
				foreach (object obj in childNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (xmlNode.NodeType == XmlNodeType.Element)
					{
						DirectoryResponse directoryResponse = this.ConstructElement((XmlElement)xmlNode);
						this.dsmlResponse.Add(directoryResponse);
					}
				}
			}
			finally
			{
				streamReader.Close();
			}
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00008294 File Offset: 0x00007294
		internal DsmlResponseDocument(StringBuilder responseString, string xpathToResponse)
			: this()
		{
			this.dsmlDocument = new XmlDocument();
			try
			{
				this.dsmlDocument.LoadXml(responseString.ToString());
			}
			catch (XmlException)
			{
				throw new DsmlInvalidDocumentException(Res.GetString("NotWellFormedResponse"));
			}
			this.dsmlNS = NamespaceUtils.GetDsmlNamespaceManager();
			this.dsmlBatchResponse = (XmlElement)this.dsmlDocument.SelectSingleNode(xpathToResponse, this.dsmlNS);
			if (this.dsmlBatchResponse == null)
			{
				throw new DsmlInvalidDocumentException(Res.GetString("NotWellFormedResponse"));
			}
			XmlNodeList childNodes = this.dsmlBatchResponse.ChildNodes;
			foreach (object obj in childNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					DirectoryResponse directoryResponse = this.ConstructElement((XmlElement)xmlNode);
					this.dsmlResponse.Add(directoryResponse);
				}
			}
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00008394 File Offset: 0x00007394
		private DsmlResponseDocument(string responseString)
			: this(new StringBuilder(responseString), "se:Envelope/se:Body/dsml:batchResponse")
		{
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x000083A8 File Offset: 0x000073A8
		public bool IsErrorResponse
		{
			get
			{
				foreach (object obj in this.dsmlResponse)
				{
					DirectoryResponse directoryResponse = (DirectoryResponse)obj;
					if (directoryResponse is DsmlErrorResponse)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x0000840C File Offset: 0x0000740C
		public bool IsOperationError
		{
			get
			{
				foreach (object obj in this.dsmlResponse)
				{
					DirectoryResponse directoryResponse = (DirectoryResponse)obj;
					if (!(directoryResponse is DsmlErrorResponse))
					{
						ResultCode resultCode = directoryResponse.ResultCode;
						if (resultCode != ResultCode.Success && ResultCode.CompareTrue != resultCode && ResultCode.CompareFalse != resultCode && ResultCode.Referral != resultCode && ResultCode.ReferralV2 != resultCode)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x0000848C File Offset: 0x0000748C
		public string RequestId
		{
			get
			{
				XmlAttribute xmlAttribute = (XmlAttribute)this.dsmlBatchResponse.SelectSingleNode("@dsml:requestID", this.dsmlNS);
				if (xmlAttribute != null)
				{
					return xmlAttribute.Value;
				}
				xmlAttribute = (XmlAttribute)this.dsmlBatchResponse.SelectSingleNode("@requestID", this.dsmlNS);
				if (xmlAttribute == null)
				{
					return null;
				}
				return xmlAttribute.Value;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x000084E6 File Offset: 0x000074E6
		internal string ResponseString
		{
			get
			{
				if (this.dsmlDocument != null)
				{
					return this.dsmlDocument.InnerXml;
				}
				return null;
			}
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00008500 File Offset: 0x00007500
		public override XmlDocument ToXml()
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(this.dsmlBatchResponse.OuterXml);
			return xmlDocument;
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x00008525 File Offset: 0x00007525
		object ICollection.SyncRoot
		{
			get
			{
				return this.dsmlResponse.SyncRoot;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001EA RID: 490 RVA: 0x00008532 File Offset: 0x00007532
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.dsmlResponse.IsSynchronized;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001EB RID: 491 RVA: 0x0000853F File Offset: 0x0000753F
		int ICollection.Count
		{
			get
			{
				return this.dsmlResponse.Count;
			}
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000854C File Offset: 0x0000754C
		void ICollection.CopyTo(Array value, int i)
		{
			this.dsmlResponse.CopyTo(value, i);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000855B File Offset: 0x0000755B
		public IEnumerator GetEnumerator()
		{
			return this.dsmlResponse.GetEnumerator();
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001EE RID: 494 RVA: 0x00008568 File Offset: 0x00007568
		protected object SyncRoot
		{
			get
			{
				return this.dsmlResponse.SyncRoot;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001EF RID: 495 RVA: 0x00008575 File Offset: 0x00007575
		protected bool IsSynchronized
		{
			get
			{
				return this.dsmlResponse.IsSynchronized;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x00008582 File Offset: 0x00007582
		public int Count
		{
			get
			{
				return this.dsmlResponse.Count;
			}
		}

		// Token: 0x1700008F RID: 143
		public DirectoryResponse this[int index]
		{
			get
			{
				return (DirectoryResponse)this.dsmlResponse[index];
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x000085A2 File Offset: 0x000075A2
		public void CopyTo(DirectoryResponse[] value, int i)
		{
			this.dsmlResponse.CopyTo(value, i);
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x000085B4 File Offset: 0x000075B4
		private DirectoryResponse ConstructElement(XmlElement node)
		{
			string localName;
			if ((localName = node.LocalName) != null)
			{
				if (<PrivateImplementationDetails>{0F2CC45B-8908-4A79-99BF-EFE68DF0A5C8}.$$method0x60001f1-1 == null)
				{
					<PrivateImplementationDetails>{0F2CC45B-8908-4A79-99BF-EFE68DF0A5C8}.$$method0x60001f1-1 = new Dictionary<string, int>(9)
					{
						{ "errorResponse", 0 },
						{ "searchResponse", 1 },
						{ "modifyResponse", 2 },
						{ "addResponse", 3 },
						{ "delResponse", 4 },
						{ "modDNResponse", 5 },
						{ "compareResponse", 6 },
						{ "extendedResponse", 7 },
						{ "authResponse", 8 }
					};
				}
				int num;
				if (<PrivateImplementationDetails>{0F2CC45B-8908-4A79-99BF-EFE68DF0A5C8}.$$method0x60001f1-1.TryGetValue(localName, out num))
				{
					DirectoryResponse directoryResponse;
					switch (num)
					{
					case 0:
						directoryResponse = new DsmlErrorResponse(node);
						break;
					case 1:
						directoryResponse = new SearchResponse(node);
						break;
					case 2:
						directoryResponse = new ModifyResponse(node);
						break;
					case 3:
						directoryResponse = new AddResponse(node);
						break;
					case 4:
						directoryResponse = new DeleteResponse(node);
						break;
					case 5:
						directoryResponse = new ModifyDNResponse(node);
						break;
					case 6:
						directoryResponse = new CompareResponse(node);
						break;
					case 7:
						directoryResponse = new ExtendedResponse(node);
						break;
					case 8:
						directoryResponse = new DsmlAuthResponse(node);
						break;
					default:
						goto IL_0120;
					}
					return directoryResponse;
				}
			}
			IL_0120:
			throw new DsmlInvalidDocumentException(Res.GetString("UnknownResponseElement"));
		}

		// Token: 0x040001E4 RID: 484
		private ArrayList dsmlResponse;

		// Token: 0x040001E5 RID: 485
		private XmlDocument dsmlDocument;

		// Token: 0x040001E6 RID: 486
		private XmlElement dsmlBatchResponse;

		// Token: 0x040001E7 RID: 487
		private XmlNamespaceManager dsmlNS;
	}
}
