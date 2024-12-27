using System;
using System.Xml;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200004C RID: 76
	public class SearchResultReference
	{
		// Token: 0x06000184 RID: 388 RVA: 0x00007416 File Offset: 0x00006416
		internal SearchResultReference(XmlNode node)
		{
			this.dsmlNode = node;
			this.dsmlNS = NamespaceUtils.GetDsmlNamespaceManager();
			this.dsmlRequest = true;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00007437 File Offset: 0x00006437
		internal SearchResultReference(Uri[] uris)
		{
			this.resultReferences = uris;
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000186 RID: 390 RVA: 0x00007448 File Offset: 0x00006448
		public Uri[] Reference
		{
			get
			{
				if (this.dsmlRequest && this.resultReferences == null)
				{
					this.resultReferences = this.UriHelper();
				}
				if (this.resultReferences == null)
				{
					return new Uri[0];
				}
				Uri[] array = new Uri[this.resultReferences.Length];
				for (int i = 0; i < this.resultReferences.Length; i++)
				{
					array[i] = new Uri(this.resultReferences[i].AbsoluteUri);
				}
				return array;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000187 RID: 391 RVA: 0x000074B8 File Offset: 0x000064B8
		public DirectoryControl[] Controls
		{
			get
			{
				if (this.dsmlRequest && this.resultControls == null)
				{
					this.resultControls = this.ControlsHelper();
				}
				if (this.resultControls == null)
				{
					return new DirectoryControl[0];
				}
				DirectoryControl[] array = new DirectoryControl[this.resultControls.Length];
				for (int i = 0; i < this.resultControls.Length; i++)
				{
					array[i] = new DirectoryControl(this.resultControls[i].Type, this.resultControls[i].GetValue(), this.resultControls[i].IsCritical, this.resultControls[i].ServerSide);
				}
				DirectoryControl.TransformControls(array);
				return array;
			}
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00007558 File Offset: 0x00006558
		private Uri[] UriHelper()
		{
			XmlNodeList xmlNodeList = this.dsmlNode.SelectNodes("dsml:ref", this.dsmlNS);
			if (xmlNodeList.Count == 0)
			{
				return new Uri[0];
			}
			Uri[] array = new Uri[xmlNodeList.Count];
			int num = 0;
			foreach (object obj in xmlNodeList)
			{
				XmlNode xmlNode = (XmlNode)obj;
				array[num] = new Uri(xmlNode.InnerText);
				num++;
			}
			return array;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x000075F4 File Offset: 0x000065F4
		private DirectoryControl[] ControlsHelper()
		{
			XmlNodeList xmlNodeList = this.dsmlNode.SelectNodes("dsml:control", this.dsmlNS);
			if (xmlNodeList.Count == 0)
			{
				return new DirectoryControl[0];
			}
			DirectoryControl[] array = new DirectoryControl[xmlNodeList.Count];
			int num = 0;
			foreach (object obj in xmlNodeList)
			{
				XmlNode xmlNode = (XmlNode)obj;
				array[num] = new DirectoryControl((XmlElement)xmlNode);
				num++;
			}
			return array;
		}

		// Token: 0x04000165 RID: 357
		private XmlNode dsmlNode;

		// Token: 0x04000166 RID: 358
		private XmlNamespaceManager dsmlNS;

		// Token: 0x04000167 RID: 359
		private bool dsmlRequest;

		// Token: 0x04000168 RID: 360
		private Uri[] resultReferences;

		// Token: 0x04000169 RID: 361
		private DirectoryControl[] resultControls;
	}
}
