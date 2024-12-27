using System;

namespace System.Security
{
	// Token: 0x020005FC RID: 1532
	[Serializable]
	internal sealed class SecurityDocumentElement : ISecurityElementFactory
	{
		// Token: 0x060037D1 RID: 14289 RVA: 0x000BBEB6 File Offset: 0x000BAEB6
		internal SecurityDocumentElement(SecurityDocument document, int position)
		{
			this.m_document = document;
			this.m_position = position;
		}

		// Token: 0x060037D2 RID: 14290 RVA: 0x000BBECC File Offset: 0x000BAECC
		SecurityElement ISecurityElementFactory.CreateSecurityElement()
		{
			return this.m_document.GetElement(this.m_position, true);
		}

		// Token: 0x060037D3 RID: 14291 RVA: 0x000BBEE0 File Offset: 0x000BAEE0
		object ISecurityElementFactory.Copy()
		{
			return new SecurityDocumentElement(this.m_document, this.m_position);
		}

		// Token: 0x060037D4 RID: 14292 RVA: 0x000BBEF3 File Offset: 0x000BAEF3
		string ISecurityElementFactory.GetTag()
		{
			return this.m_document.GetTagForElement(this.m_position);
		}

		// Token: 0x060037D5 RID: 14293 RVA: 0x000BBF06 File Offset: 0x000BAF06
		string ISecurityElementFactory.Attribute(string attributeName)
		{
			return this.m_document.GetAttributeForElement(this.m_position, attributeName);
		}

		// Token: 0x04001CC6 RID: 7366
		private int m_position;

		// Token: 0x04001CC7 RID: 7367
		private SecurityDocument m_document;
	}
}
