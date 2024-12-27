using System;
using System.Collections;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.IlGen
{
	// Token: 0x02000035 RID: 53
	internal class XmlILElementAnalyzer : XmlILStateAnalyzer
	{
		// Token: 0x0600033C RID: 828 RVA: 0x00015896 File Offset: 0x00014896
		public XmlILElementAnalyzer(QilFactory fac)
			: base(fac)
		{
		}

		// Token: 0x0600033D RID: 829 RVA: 0x000158B8 File Offset: 0x000148B8
		public override QilNode Analyze(QilNode ndElem, QilNode ndContent)
		{
			this.parentInfo = XmlILConstructInfo.Write(ndElem);
			this.parentInfo.MightHaveNamespacesAfterAttributes = false;
			this.parentInfo.MightHaveAttributes = false;
			this.parentInfo.MightHaveDuplicateAttributes = false;
			this.parentInfo.MightHaveNamespaces = !this.parentInfo.IsNamespaceInScope;
			this.dupAttrs.Clear();
			return base.Analyze(ndElem, ndContent);
		}

		// Token: 0x0600033E RID: 830 RVA: 0x00015921 File Offset: 0x00014921
		protected override void AnalyzeLoop(QilLoop ndLoop, XmlILConstructInfo info)
		{
			if (ndLoop.XmlType.MaybeMany)
			{
				this.CheckAttributeNamespaceConstruct(ndLoop.XmlType);
			}
			base.AnalyzeLoop(ndLoop, info);
		}

		// Token: 0x0600033F RID: 831 RVA: 0x00015944 File Offset: 0x00014944
		protected override void AnalyzeCopy(QilNode ndCopy, XmlILConstructInfo info)
		{
			if (ndCopy.NodeType == QilNodeType.AttributeCtor)
			{
				this.AnalyzeAttributeCtor(ndCopy as QilBinary, info);
			}
			else
			{
				this.CheckAttributeNamespaceConstruct(ndCopy.XmlType);
			}
			base.AnalyzeCopy(ndCopy, info);
		}

		// Token: 0x06000340 RID: 832 RVA: 0x00015974 File Offset: 0x00014974
		private void AnalyzeAttributeCtor(QilBinary ndAttr, XmlILConstructInfo info)
		{
			if (ndAttr.Left.NodeType == QilNodeType.LiteralQName)
			{
				QilName qilName = ndAttr.Left as QilName;
				this.parentInfo.MightHaveAttributes = true;
				if (!this.parentInfo.MightHaveDuplicateAttributes)
				{
					XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(this.attrNames.Add(qilName.LocalName), this.attrNames.Add(qilName.NamespaceUri));
					int i;
					for (i = 0; i < this.dupAttrs.Count; i++)
					{
						XmlQualifiedName xmlQualifiedName2 = (XmlQualifiedName)this.dupAttrs[i];
						if (xmlQualifiedName2.Name == xmlQualifiedName.Name && xmlQualifiedName2.Namespace == xmlQualifiedName.Namespace)
						{
							this.parentInfo.MightHaveDuplicateAttributes = true;
						}
					}
					if (i >= this.dupAttrs.Count)
					{
						this.dupAttrs.Add(xmlQualifiedName);
					}
				}
				if (!info.IsNamespaceInScope)
				{
					this.parentInfo.MightHaveNamespaces = true;
					return;
				}
			}
			else
			{
				this.CheckAttributeNamespaceConstruct(ndAttr.XmlType);
			}
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00015A70 File Offset: 0x00014A70
		private void CheckAttributeNamespaceConstruct(XmlQueryType typ)
		{
			if ((typ.NodeKinds & XmlNodeKindFlags.Attribute) != XmlNodeKindFlags.None)
			{
				this.parentInfo.MightHaveAttributes = true;
				this.parentInfo.MightHaveDuplicateAttributes = true;
				this.parentInfo.MightHaveNamespaces = true;
			}
			if ((typ.NodeKinds & XmlNodeKindFlags.Namespace) != XmlNodeKindFlags.None)
			{
				this.parentInfo.MightHaveNamespaces = true;
				if (this.parentInfo.MightHaveAttributes)
				{
					this.parentInfo.MightHaveNamespacesAfterAttributes = true;
				}
			}
		}

		// Token: 0x040002BB RID: 699
		private NameTable attrNames = new NameTable();

		// Token: 0x040002BC RID: 700
		private ArrayList dupAttrs = new ArrayList();
	}
}
