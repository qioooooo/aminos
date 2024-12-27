using System;
using System.Xml;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000044 RID: 68
	public class SearchResponse : DirectoryResponse
	{
		// Token: 0x06000164 RID: 356 RVA: 0x00006918 File Offset: 0x00005918
		internal SearchResponse(XmlNode node)
			: base(node)
		{
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00006937 File Offset: 0x00005937
		internal SearchResponse(string dn, DirectoryControl[] controls, ResultCode result, string message, Uri[] referral)
			: base(dn, controls, result, message, referral)
		{
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000166 RID: 358 RVA: 0x0000695C File Offset: 0x0000595C
		public override string MatchedDN
		{
			get
			{
				if (this.dsmlRequest && this.dn == null)
				{
					this.dn = base.MatchedDNHelper("dsml:searchResultDone/@dsml:matchedDN", "dsml:searchResultDone/@matchedDN");
				}
				return this.dn;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000167 RID: 359 RVA: 0x0000698C File Offset: 0x0000598C
		public override DirectoryControl[] Controls
		{
			get
			{
				if (this.dsmlRequest && this.directoryControls == null)
				{
					this.directoryControls = base.ControlsHelper("dsml:searchResultDone/dsml:control");
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

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00006A2F File Offset: 0x00005A2F
		public override ResultCode ResultCode
		{
			get
			{
				if (this.dsmlRequest && this.result == (ResultCode)(-1))
				{
					this.result = base.ResultCodeHelper("dsml:searchResultDone/dsml:resultCode/@dsml:code", "dsml:searchResultDone/dsml:resultCode/@code");
				}
				return this.result;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00006A5E File Offset: 0x00005A5E
		public override string ErrorMessage
		{
			get
			{
				if (this.dsmlRequest && this.directoryMessage == null)
				{
					this.directoryMessage = base.ErrorMessageHelper("dsml:searchResultDone/dsml:errorMessage");
				}
				return this.directoryMessage;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00006A88 File Offset: 0x00005A88
		public override Uri[] Referral
		{
			get
			{
				if (this.dsmlRequest && this.directoryReferral == null)
				{
					this.directoryReferral = base.ReferralHelper("dsml:searchResultDone/dsml:referral");
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

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00006AFC File Offset: 0x00005AFC
		public SearchResultReferenceCollection References
		{
			get
			{
				if (this.dsmlRequest && this.referenceCollection.Count == 0)
				{
					this.referenceCollection = this.ReferenceHelper();
				}
				return this.referenceCollection;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00006B25 File Offset: 0x00005B25
		public SearchResultEntryCollection Entries
		{
			get
			{
				if (this.dsmlRequest && this.entryCollection.Count == 0)
				{
					this.entryCollection = this.EntryHelper();
				}
				return this.entryCollection;
			}
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00006B4E File Offset: 0x00005B4E
		internal void SetReferences(SearchResultReferenceCollection col)
		{
			this.referenceCollection = col;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00006B57 File Offset: 0x00005B57
		internal void SetEntries(SearchResultEntryCollection col)
		{
			this.entryCollection = col;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00006B60 File Offset: 0x00005B60
		private SearchResultReferenceCollection ReferenceHelper()
		{
			SearchResultReferenceCollection searchResultReferenceCollection = new SearchResultReferenceCollection();
			XmlNodeList xmlNodeList = this.dsmlNode.SelectNodes("dsml:searchResultReference", this.dsmlNS);
			if (xmlNodeList.Count != 0)
			{
				foreach (object obj in xmlNodeList)
				{
					XmlNode xmlNode = (XmlNode)obj;
					SearchResultReference searchResultReference = new SearchResultReference((XmlElement)xmlNode);
					searchResultReferenceCollection.Add(searchResultReference);
				}
			}
			return searchResultReferenceCollection;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00006BF0 File Offset: 0x00005BF0
		private SearchResultEntryCollection EntryHelper()
		{
			SearchResultEntryCollection searchResultEntryCollection = new SearchResultEntryCollection();
			XmlNodeList xmlNodeList = this.dsmlNode.SelectNodes("dsml:searchResultEntry", this.dsmlNS);
			if (xmlNodeList.Count != 0)
			{
				foreach (object obj in xmlNodeList)
				{
					XmlNode xmlNode = (XmlNode)obj;
					SearchResultEntry searchResultEntry = new SearchResultEntry((XmlElement)xmlNode);
					searchResultEntryCollection.Add(searchResultEntry);
				}
			}
			return searchResultEntryCollection;
		}

		// Token: 0x04000125 RID: 293
		private SearchResultReferenceCollection referenceCollection = new SearchResultReferenceCollection();

		// Token: 0x04000126 RID: 294
		private SearchResultEntryCollection entryCollection = new SearchResultEntryCollection();

		// Token: 0x04000127 RID: 295
		internal bool searchDone;
	}
}
