using System;
using System.Collections.Generic;
using System.Xml.XPath;

namespace System.Xml.Xsl.XPath
{
	// Token: 0x020000D1 RID: 209
	internal interface IXPathBuilder<Node>
	{
		// Token: 0x060009D5 RID: 2517
		void StartBuild();

		// Token: 0x060009D6 RID: 2518
		Node EndBuild(Node result);

		// Token: 0x060009D7 RID: 2519
		Node String(string value);

		// Token: 0x060009D8 RID: 2520
		Node Number(double value);

		// Token: 0x060009D9 RID: 2521
		Node Operator(XPathOperator op, Node left, Node right);

		// Token: 0x060009DA RID: 2522
		Node Axis(XPathAxis xpathAxis, XPathNodeType nodeType, string prefix, string name);

		// Token: 0x060009DB RID: 2523
		Node JoinStep(Node left, Node right);

		// Token: 0x060009DC RID: 2524
		Node Predicate(Node node, Node condition, bool reverseStep);

		// Token: 0x060009DD RID: 2525
		Node Variable(string prefix, string name);

		// Token: 0x060009DE RID: 2526
		Node Function(string prefix, string name, IList<Node> args);
	}
}
