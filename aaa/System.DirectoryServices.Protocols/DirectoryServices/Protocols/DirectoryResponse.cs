using System;
using System.Globalization;
using System.Xml;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200003D RID: 61
	public abstract class DirectoryResponse : DirectoryOperation
	{
		// Token: 0x06000149 RID: 329 RVA: 0x0000627B File Offset: 0x0000527B
		internal DirectoryResponse(XmlNode node)
		{
			this.dsmlNode = node;
			this.dsmlNS = NamespaceUtils.GetDsmlNamespaceManager();
			this.dsmlRequest = true;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x000062A3 File Offset: 0x000052A3
		internal DirectoryResponse(string dn, DirectoryControl[] controls, ResultCode result, string message, Uri[] referral)
		{
			this.dn = dn;
			this.directoryControls = controls;
			this.result = result;
			this.directoryMessage = message;
			this.directoryReferral = referral;
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600014B RID: 331 RVA: 0x000062D8 File Offset: 0x000052D8
		public string RequestId
		{
			get
			{
				if (this.dsmlRequest && this.requestID == null)
				{
					XmlAttribute xmlAttribute = (XmlAttribute)this.dsmlNode.SelectSingleNode("@dsml:requestID", this.dsmlNS);
					if (xmlAttribute == null)
					{
						xmlAttribute = (XmlAttribute)this.dsmlNode.SelectSingleNode("@requestID", this.dsmlNS);
					}
					if (xmlAttribute != null)
					{
						this.requestID = xmlAttribute.Value;
					}
				}
				return this.requestID;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00006345 File Offset: 0x00005345
		public virtual string MatchedDN
		{
			get
			{
				if (this.dsmlRequest && this.dn == null)
				{
					this.dn = this.MatchedDNHelper("@dsml:matchedDN", "@matchedDN");
				}
				return this.dn;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00006374 File Offset: 0x00005374
		public virtual DirectoryControl[] Controls
		{
			get
			{
				if (this.dsmlRequest && this.directoryControls == null)
				{
					this.directoryControls = this.ControlsHelper("dsml:control");
				}
				if (this.directoryControls == null)
				{
					return new DirectoryControl[0];
				}
				DirectoryControl[] array = new DirectoryControl[this.directoryControls.Length];
				for (int i = 0; i < this.directoryControls.Length; i++)
				{
					array[i] = new DirectoryControl(this.directoryControls[i].Type, this.directoryControls[i].GetValue(), this.directoryControls[i].IsCritical, this.directoryControls[i].ServerSide);
				}
				DirectoryControl.TransformControls(array);
				return array;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00006415 File Offset: 0x00005415
		public virtual ResultCode ResultCode
		{
			get
			{
				if (this.dsmlRequest && this.result == (ResultCode)(-1))
				{
					this.result = this.ResultCodeHelper("dsml:resultCode/@dsml:code", "dsml:resultCode/@code");
				}
				return this.result;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00006444 File Offset: 0x00005444
		public virtual string ErrorMessage
		{
			get
			{
				if (this.dsmlRequest && this.directoryMessage == null)
				{
					this.directoryMessage = this.ErrorMessageHelper("dsml:errorMessage");
				}
				return this.directoryMessage;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00006470 File Offset: 0x00005470
		public virtual Uri[] Referral
		{
			get
			{
				if (this.dsmlRequest && this.directoryReferral == null)
				{
					this.directoryReferral = this.ReferralHelper("dsml:referral");
				}
				if (this.directoryReferral == null)
				{
					return new Uri[0];
				}
				Uri[] array = new Uri[this.directoryReferral.Length];
				for (int i = 0; i < this.directoryReferral.Length; i++)
				{
					array[i] = new Uri(this.directoryReferral[i].AbsoluteUri);
				}
				return array;
			}
		}

		// Token: 0x06000151 RID: 337 RVA: 0x000064E4 File Offset: 0x000054E4
		internal string MatchedDNHelper(string primaryXPath, string secondaryXPath)
		{
			XmlAttribute xmlAttribute = (XmlAttribute)this.dsmlNode.SelectSingleNode(primaryXPath, this.dsmlNS);
			if (xmlAttribute != null)
			{
				return xmlAttribute.Value;
			}
			xmlAttribute = (XmlAttribute)this.dsmlNode.SelectSingleNode(secondaryXPath, this.dsmlNS);
			if (xmlAttribute == null)
			{
				return null;
			}
			return xmlAttribute.Value;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00006538 File Offset: 0x00005538
		internal DirectoryControl[] ControlsHelper(string primaryXPath)
		{
			XmlNodeList xmlNodeList = this.dsmlNode.SelectNodes(primaryXPath, this.dsmlNS);
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

		// Token: 0x06000153 RID: 339 RVA: 0x000065D0 File Offset: 0x000055D0
		internal ResultCode ResultCodeHelper(string primaryXPath, string secondaryXPath)
		{
			XmlAttribute xmlAttribute = (XmlAttribute)this.dsmlNode.SelectSingleNode(primaryXPath, this.dsmlNS);
			if (xmlAttribute == null)
			{
				xmlAttribute = (XmlAttribute)this.dsmlNode.SelectSingleNode(secondaryXPath, this.dsmlNS);
				if (xmlAttribute == null)
				{
					throw new DsmlInvalidDocumentException(Res.GetString("MissingOperationResponseResultCode"));
				}
			}
			string value = xmlAttribute.Value;
			int num;
			try
			{
				num = int.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture);
			}
			catch (FormatException)
			{
				throw new DsmlInvalidDocumentException(Res.GetString("BadOperationResponseResultCode", new object[] { value }));
			}
			catch (OverflowException)
			{
				throw new DsmlInvalidDocumentException(Res.GetString("BadOperationResponseResultCode", new object[] { value }));
			}
			if (!Utility.IsResultCode((ResultCode)num))
			{
				throw new DsmlInvalidDocumentException(Res.GetString("BadOperationResponseResultCode", new object[] { value }));
			}
			return (ResultCode)num;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x000066C0 File Offset: 0x000056C0
		internal string ErrorMessageHelper(string primaryXPath)
		{
			XmlElement xmlElement = (XmlElement)this.dsmlNode.SelectSingleNode(primaryXPath, this.dsmlNS);
			if (xmlElement != null)
			{
				return xmlElement.InnerText;
			}
			return null;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x000066F0 File Offset: 0x000056F0
		internal Uri[] ReferralHelper(string primaryXPath)
		{
			XmlNodeList xmlNodeList = this.dsmlNode.SelectNodes(primaryXPath, this.dsmlNS);
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

		// Token: 0x0400011A RID: 282
		internal XmlNode dsmlNode;

		// Token: 0x0400011B RID: 283
		internal XmlNamespaceManager dsmlNS;

		// Token: 0x0400011C RID: 284
		internal bool dsmlRequest;

		// Token: 0x0400011D RID: 285
		internal string dn;

		// Token: 0x0400011E RID: 286
		internal DirectoryControl[] directoryControls;

		// Token: 0x0400011F RID: 287
		internal ResultCode result = (ResultCode)(-1);

		// Token: 0x04000120 RID: 288
		internal string directoryMessage;

		// Token: 0x04000121 RID: 289
		internal Uri[] directoryReferral;

		// Token: 0x04000122 RID: 290
		private string requestID;
	}
}
