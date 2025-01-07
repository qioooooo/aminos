using System;
using System.Collections;
using System.Xml.XPath;
using MS.Internal.Xml.XPath;

namespace System.Xml.Schema
{
	internal class Asttree
	{
		internal ArrayList SubtreeArray
		{
			get
			{
				return this.fAxisArray;
			}
		}

		public Asttree(string xPath, bool isField, XmlNamespaceManager nsmgr)
		{
			this.xpathexpr = xPath;
			this.isField = isField;
			this.nsmgr = nsmgr;
			this.CompileXPath(xPath, isField, nsmgr);
		}

		private static bool IsNameTest(Axis ast)
		{
			return ast.TypeOfAxis == Axis.AxisType.Child && ast.NodeType == XPathNodeType.Element;
		}

		internal static bool IsAttribute(Axis ast)
		{
			return ast.TypeOfAxis == Axis.AxisType.Attribute && ast.NodeType == XPathNodeType.Attribute;
		}

		private static bool IsDescendantOrSelf(Axis ast)
		{
			return ast.TypeOfAxis == Axis.AxisType.DescendantOrSelf && ast.NodeType == XPathNodeType.All && ast.AbbrAxis;
		}

		internal static bool IsSelf(Axis ast)
		{
			return ast.TypeOfAxis == Axis.AxisType.Self && ast.NodeType == XPathNodeType.All && ast.AbbrAxis;
		}

		public void CompileXPath(string xPath, bool isField, XmlNamespaceManager nsmgr)
		{
			if (xPath == null || xPath.Length == 0)
			{
				throw new XmlSchemaException("Sch_EmptyXPath", string.Empty);
			}
			string[] array = xPath.Split(new char[] { '|' });
			ArrayList arrayList = new ArrayList(array.Length);
			this.fAxisArray = new ArrayList(array.Length);
			try
			{
				foreach (string text in array)
				{
					Axis axis = (Axis)XPathParser.ParseXPathExpresion(text);
					arrayList.Add(axis);
				}
			}
			catch
			{
				throw new XmlSchemaException("Sch_ICXpathError", xPath);
			}
			foreach (object obj in arrayList)
			{
				Axis axis2 = (Axis)obj;
				Axis axis3;
				if ((axis3 = axis2) == null)
				{
					throw new XmlSchemaException("Sch_ICXpathError", xPath);
				}
				Axis axis4 = axis3;
				if (Asttree.IsAttribute(axis3))
				{
					if (!isField)
					{
						throw new XmlSchemaException("Sch_SelectorAttr", xPath);
					}
					this.SetURN(axis3, nsmgr);
					try
					{
						axis3 = (Axis)axis3.Input;
						goto IL_014D;
					}
					catch
					{
						throw new XmlSchemaException("Sch_ICXpathError", xPath);
					}
					goto IL_00FB;
				}
				IL_014D:
				if (axis3 == null || (!Asttree.IsNameTest(axis3) && !Asttree.IsSelf(axis3)))
				{
					axis4.Input = null;
					if (axis3 == null)
					{
						if (Asttree.IsSelf(axis2) && axis2.Input != null)
						{
							this.fAxisArray.Add(new ForwardAxis(DoubleLinkAxis.ConvertTree((Axis)axis2.Input), false));
							continue;
						}
						this.fAxisArray.Add(new ForwardAxis(DoubleLinkAxis.ConvertTree(axis2), false));
						continue;
					}
					else
					{
						if (!Asttree.IsDescendantOrSelf(axis3))
						{
							throw new XmlSchemaException("Sch_ICXpathError", xPath);
						}
						try
						{
							axis3 = (Axis)axis3.Input;
						}
						catch
						{
							throw new XmlSchemaException("Sch_ICXpathError", xPath);
						}
						if (axis3 == null || !Asttree.IsSelf(axis3) || axis3.Input != null)
						{
							throw new XmlSchemaException("Sch_ICXpathError", xPath);
						}
						if (Asttree.IsSelf(axis2) && axis2.Input != null)
						{
							this.fAxisArray.Add(new ForwardAxis(DoubleLinkAxis.ConvertTree((Axis)axis2.Input), true));
							continue;
						}
						this.fAxisArray.Add(new ForwardAxis(DoubleLinkAxis.ConvertTree(axis2), true));
						continue;
					}
				}
				IL_00FB:
				if (Asttree.IsSelf(axis3) && axis2 != axis3)
				{
					axis4.Input = axis3.Input;
				}
				else
				{
					axis4 = axis3;
					if (Asttree.IsNameTest(axis3))
					{
						this.SetURN(axis3, nsmgr);
					}
				}
				try
				{
					axis3 = (Axis)axis3.Input;
				}
				catch
				{
					throw new XmlSchemaException("Sch_ICXpathError", xPath);
				}
				goto IL_014D;
			}
		}

		private void SetURN(Axis axis, XmlNamespaceManager nsmgr)
		{
			if (axis.Prefix.Length != 0)
			{
				axis.Urn = nsmgr.LookupNamespace(axis.Prefix);
				if (axis.Urn == null)
				{
					throw new XmlSchemaException("Sch_UnresolvedPrefix", axis.Prefix);
				}
			}
			else
			{
				if (axis.Name.Length != 0)
				{
					axis.Urn = null;
					return;
				}
				axis.Urn = "";
			}
		}

		private ArrayList fAxisArray;

		private string xpathexpr;

		private bool isField;

		private XmlNamespaceManager nsmgr;
	}
}
