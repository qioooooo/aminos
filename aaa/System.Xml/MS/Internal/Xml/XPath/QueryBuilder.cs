﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000156 RID: 342
	internal sealed class QueryBuilder
	{
		// Token: 0x060012C4 RID: 4804 RVA: 0x00051630 File Offset: 0x00050630
		private void Reset()
		{
			this.needContext = false;
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x0005163C File Offset: 0x0005063C
		private Query ProcessAxis(Axis root, QueryBuilder.Flags flags, out QueryBuilder.Props props)
		{
			if (root.Prefix.Length > 0)
			{
				this.needContext = true;
			}
			this.firstInput = null;
			Query query2;
			Query query3;
			if (root.Input != null)
			{
				QueryBuilder.Flags flags2 = QueryBuilder.Flags.None;
				if ((flags & QueryBuilder.Flags.PosFilter) == QueryBuilder.Flags.None)
				{
					Axis axis = root.Input as Axis;
					if (axis != null && root.TypeOfAxis == Axis.AxisType.Child && axis.TypeOfAxis == Axis.AxisType.DescendantOrSelf && axis.NodeType == XPathNodeType.All)
					{
						Query query;
						if (axis.Input != null)
						{
							query = this.ProcessNode(axis.Input, QueryBuilder.Flags.SmartDesc, out props);
						}
						else
						{
							query = new ContextQuery();
							props = QueryBuilder.Props.None;
						}
						query2 = new DescendantQuery(query, root.Name, root.Prefix, root.NodeType, false, axis.AbbrAxis);
						if ((props & QueryBuilder.Props.NonFlat) != QueryBuilder.Props.None)
						{
							query2 = new DocumentOrderQuery(query2);
						}
						props |= QueryBuilder.Props.NonFlat;
						return query2;
					}
					if (root.TypeOfAxis == Axis.AxisType.Descendant || root.TypeOfAxis == Axis.AxisType.DescendantOrSelf)
					{
						flags2 |= QueryBuilder.Flags.SmartDesc;
					}
				}
				query3 = this.ProcessNode(root.Input, flags2, out props);
			}
			else
			{
				query3 = new ContextQuery();
				props = QueryBuilder.Props.None;
			}
			switch (root.TypeOfAxis)
			{
			case Axis.AxisType.Ancestor:
				query2 = new XPathAncestorQuery(query3, root.Name, root.Prefix, root.NodeType, false);
				props |= QueryBuilder.Props.NonFlat;
				break;
			case Axis.AxisType.AncestorOrSelf:
				query2 = new XPathAncestorQuery(query3, root.Name, root.Prefix, root.NodeType, true);
				props |= QueryBuilder.Props.NonFlat;
				break;
			case Axis.AxisType.Attribute:
				query2 = new AttributeQuery(query3, root.Name, root.Prefix, root.NodeType);
				break;
			case Axis.AxisType.Child:
				if ((props & QueryBuilder.Props.NonFlat) != QueryBuilder.Props.None)
				{
					query2 = new CacheChildrenQuery(query3, root.Name, root.Prefix, root.NodeType);
				}
				else
				{
					query2 = new ChildrenQuery(query3, root.Name, root.Prefix, root.NodeType);
				}
				break;
			case Axis.AxisType.Descendant:
				if ((flags & QueryBuilder.Flags.SmartDesc) != QueryBuilder.Flags.None)
				{
					query2 = new DescendantOverDescendantQuery(query3, false, root.Name, root.Prefix, root.NodeType, false);
				}
				else
				{
					query2 = new DescendantQuery(query3, root.Name, root.Prefix, root.NodeType, false, false);
					if ((props & QueryBuilder.Props.NonFlat) != QueryBuilder.Props.None)
					{
						query2 = new DocumentOrderQuery(query2);
					}
				}
				props |= QueryBuilder.Props.NonFlat;
				break;
			case Axis.AxisType.DescendantOrSelf:
				if ((flags & QueryBuilder.Flags.SmartDesc) != QueryBuilder.Flags.None)
				{
					query2 = new DescendantOverDescendantQuery(query3, true, root.Name, root.Prefix, root.NodeType, root.AbbrAxis);
				}
				else
				{
					query2 = new DescendantQuery(query3, root.Name, root.Prefix, root.NodeType, true, root.AbbrAxis);
					if ((props & QueryBuilder.Props.NonFlat) != QueryBuilder.Props.None)
					{
						query2 = new DocumentOrderQuery(query2);
					}
				}
				props |= QueryBuilder.Props.NonFlat;
				break;
			case Axis.AxisType.Following:
				query2 = new FollowingQuery(query3, root.Name, root.Prefix, root.NodeType);
				props |= QueryBuilder.Props.NonFlat;
				break;
			case Axis.AxisType.FollowingSibling:
				query2 = new FollSiblingQuery(query3, root.Name, root.Prefix, root.NodeType);
				if ((props & QueryBuilder.Props.NonFlat) != QueryBuilder.Props.None)
				{
					query2 = new DocumentOrderQuery(query2);
				}
				break;
			case Axis.AxisType.Namespace:
				if ((root.NodeType == XPathNodeType.All || root.NodeType == XPathNodeType.Element || root.NodeType == XPathNodeType.Attribute) && root.Prefix.Length == 0)
				{
					query2 = new NamespaceQuery(query3, root.Name, root.Prefix, root.NodeType);
				}
				else
				{
					query2 = new EmptyQuery();
				}
				break;
			case Axis.AxisType.Parent:
				query2 = new ParentQuery(query3, root.Name, root.Prefix, root.NodeType);
				break;
			case Axis.AxisType.Preceding:
				query2 = new PrecedingQuery(query3, root.Name, root.Prefix, root.NodeType);
				props |= QueryBuilder.Props.NonFlat;
				break;
			case Axis.AxisType.PrecedingSibling:
				query2 = new PreSiblingQuery(query3, root.Name, root.Prefix, root.NodeType);
				break;
			case Axis.AxisType.Self:
				query2 = new XPathSelfQuery(query3, root.Name, root.Prefix, root.NodeType);
				break;
			default:
				throw XPathException.Create("Xp_NotSupported", this.query);
			}
			return query2;
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x000519FF File Offset: 0x000509FF
		private bool CanBeNumber(Query q)
		{
			return q.StaticType == XPathResultType.Any || q.StaticType == XPathResultType.Number;
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x00051A18 File Offset: 0x00050A18
		private Query ProcessFilter(Filter root, QueryBuilder.Flags flags, out QueryBuilder.Props props)
		{
			bool flag = (flags & QueryBuilder.Flags.Filter) == QueryBuilder.Flags.None;
			QueryBuilder.Props props2;
			Query query = this.ProcessNode(root.Condition, QueryBuilder.Flags.None, out props2);
			if (this.CanBeNumber(query) || (props2 & (QueryBuilder.Props)6) != QueryBuilder.Props.None)
			{
				props2 |= QueryBuilder.Props.HasPosition;
				flags |= QueryBuilder.Flags.PosFilter;
			}
			flags &= (QueryBuilder.Flags)(-2);
			Query query2 = this.ProcessNode(root.Input, flags | QueryBuilder.Flags.Filter, out props);
			if (root.Input.Type != AstNode.AstType.Filter)
			{
				props &= (QueryBuilder.Props)(-2);
			}
			if ((props2 & QueryBuilder.Props.HasPosition) != QueryBuilder.Props.None)
			{
				props |= QueryBuilder.Props.PosFilter;
			}
			FilterQuery filterQuery = query2 as FilterQuery;
			if (filterQuery != null && (props2 & QueryBuilder.Props.HasPosition) == QueryBuilder.Props.None && filterQuery.Condition.StaticType != XPathResultType.Any)
			{
				Query query3 = filterQuery.Condition;
				if (query3.StaticType == XPathResultType.Number)
				{
					query3 = new LogicalExpr(Operator.Op.EQ, new NodeFunctions(Function.FunctionType.FuncPosition, null), query3);
				}
				query = new BooleanExpr(Operator.Op.AND, query3, query);
				query2 = filterQuery.qyInput;
			}
			if ((props & QueryBuilder.Props.PosFilter) != QueryBuilder.Props.None && query2 is DocumentOrderQuery)
			{
				query2 = ((DocumentOrderQuery)query2).input;
			}
			if (this.firstInput == null)
			{
				this.firstInput = query2 as BaseAxisQuery;
			}
			bool flag2 = (query2.Properties & QueryProps.Merge) != QueryProps.None;
			bool flag3 = (query2.Properties & QueryProps.Reverse) != QueryProps.None;
			if ((props2 & QueryBuilder.Props.HasPosition) != QueryBuilder.Props.None)
			{
				if (flag3)
				{
					query2 = new ReversePositionQuery(query2);
				}
				else if ((props2 & QueryBuilder.Props.HasLast) != QueryBuilder.Props.None)
				{
					query2 = new ForwardPositionQuery(query2);
				}
			}
			if (flag && this.firstInput != null)
			{
				if (flag2 && (props & QueryBuilder.Props.PosFilter) != QueryBuilder.Props.None)
				{
					query2 = new FilterQuery(query2, query, false);
					Query qyInput = this.firstInput.qyInput;
					if (!(qyInput is ContextQuery))
					{
						this.firstInput.qyInput = new ContextQuery();
						this.firstInput = null;
						return new MergeFilterQuery(qyInput, query2);
					}
					this.firstInput = null;
					return query2;
				}
				else
				{
					this.firstInput = null;
				}
			}
			return new FilterQuery(query2, query, (props2 & QueryBuilder.Props.HasPosition) == QueryBuilder.Props.None);
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x00051BC0 File Offset: 0x00050BC0
		private Query ProcessOperator(Operator root, out QueryBuilder.Props props)
		{
			QueryBuilder.Props props2;
			Query query = this.ProcessNode(root.Operand1, QueryBuilder.Flags.None, out props2);
			QueryBuilder.Props props3;
			Query query2 = this.ProcessNode(root.Operand2, QueryBuilder.Flags.None, out props3);
			props = props2 | props3;
			switch (root.OperatorType)
			{
			case Operator.Op.LT:
			case Operator.Op.GT:
			case Operator.Op.LE:
			case Operator.Op.GE:
			case Operator.Op.EQ:
			case Operator.Op.NE:
				return new LogicalExpr(root.OperatorType, query, query2);
			case Operator.Op.OR:
			case Operator.Op.AND:
				return new BooleanExpr(root.OperatorType, query, query2);
			case Operator.Op.PLUS:
			case Operator.Op.MINUS:
			case Operator.Op.MUL:
			case Operator.Op.MOD:
			case Operator.Op.DIV:
				return new NumericExpr(root.OperatorType, query, query2);
			case Operator.Op.UNION:
				props |= QueryBuilder.Props.NonFlat;
				return new UnionExpr(query, query2);
			default:
				return null;
			}
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x00051C74 File Offset: 0x00050C74
		private Query ProcessVariable(Variable root)
		{
			this.needContext = true;
			if (!this.allowVar)
			{
				throw XPathException.Create("Xp_InvalidKeyPattern", this.query);
			}
			return new VariableQuery(root.Localname, root.Prefix);
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x00051CA8 File Offset: 0x00050CA8
		private Query ProcessFunction(Function root, out QueryBuilder.Props props)
		{
			props = QueryBuilder.Props.None;
			switch (root.TypeOfFunction)
			{
			case Function.FunctionType.FuncLast:
			{
				Query query = new NodeFunctions(root.TypeOfFunction, null);
				props |= QueryBuilder.Props.HasLast;
				return query;
			}
			case Function.FunctionType.FuncPosition:
			{
				Query query = new NodeFunctions(root.TypeOfFunction, null);
				props |= QueryBuilder.Props.HasPosition;
				return query;
			}
			case Function.FunctionType.FuncCount:
				return new NodeFunctions(Function.FunctionType.FuncCount, this.ProcessNode((AstNode)root.ArgumentList[0], QueryBuilder.Flags.None, out props));
			case Function.FunctionType.FuncID:
			{
				Query query = new IDQuery(this.ProcessNode((AstNode)root.ArgumentList[0], QueryBuilder.Flags.None, out props));
				props |= QueryBuilder.Props.NonFlat;
				return query;
			}
			case Function.FunctionType.FuncLocalName:
			case Function.FunctionType.FuncNameSpaceUri:
			case Function.FunctionType.FuncName:
				if (root.ArgumentList != null && root.ArgumentList.Count > 0)
				{
					return new NodeFunctions(root.TypeOfFunction, this.ProcessNode((AstNode)root.ArgumentList[0], QueryBuilder.Flags.None, out props));
				}
				return new NodeFunctions(root.TypeOfFunction, null);
			case Function.FunctionType.FuncString:
			case Function.FunctionType.FuncConcat:
			case Function.FunctionType.FuncStartsWith:
			case Function.FunctionType.FuncContains:
			case Function.FunctionType.FuncSubstringBefore:
			case Function.FunctionType.FuncSubstringAfter:
			case Function.FunctionType.FuncSubstring:
			case Function.FunctionType.FuncStringLength:
			case Function.FunctionType.FuncNormalize:
			case Function.FunctionType.FuncTranslate:
				return new StringFunctions(root.TypeOfFunction, this.ProcessArguments(root.ArgumentList, out props));
			case Function.FunctionType.FuncBoolean:
			case Function.FunctionType.FuncNot:
			case Function.FunctionType.FuncLang:
				return new BooleanFunctions(root.TypeOfFunction, this.ProcessNode((AstNode)root.ArgumentList[0], QueryBuilder.Flags.None, out props));
			case Function.FunctionType.FuncNumber:
			case Function.FunctionType.FuncSum:
			case Function.FunctionType.FuncFloor:
			case Function.FunctionType.FuncCeiling:
			case Function.FunctionType.FuncRound:
				if (root.ArgumentList != null && root.ArgumentList.Count > 0)
				{
					return new NumberFunctions(root.TypeOfFunction, this.ProcessNode((AstNode)root.ArgumentList[0], QueryBuilder.Flags.None, out props));
				}
				return new NumberFunctions(Function.FunctionType.FuncNumber, null);
			case Function.FunctionType.FuncTrue:
			case Function.FunctionType.FuncFalse:
				return new BooleanFunctions(root.TypeOfFunction, null);
			case Function.FunctionType.FuncUserDefined:
			{
				this.needContext = true;
				if (!this.allowCurrent && root.Name == "current" && root.Prefix.Length == 0)
				{
					throw XPathException.Create("Xp_CurrentNotAllowed");
				}
				if (!this.allowKey && root.Name == "key" && root.Prefix.Length == 0)
				{
					throw XPathException.Create("Xp_InvalidKeyPattern", this.query);
				}
				Query query = new FunctionQuery(root.Prefix, root.Name, this.ProcessArguments(root.ArgumentList, out props));
				props |= QueryBuilder.Props.NonFlat;
				return query;
			}
			default:
				throw XPathException.Create("Xp_NotSupported", this.query);
			}
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x00051F2C File Offset: 0x00050F2C
		private List<Query> ProcessArguments(ArrayList args, out QueryBuilder.Props props)
		{
			int num = ((args != null) ? args.Count : 0);
			List<Query> list = new List<Query>(num);
			props = QueryBuilder.Props.None;
			for (int i = 0; i < num; i++)
			{
				QueryBuilder.Props props2;
				list.Add(this.ProcessNode((AstNode)args[i], QueryBuilder.Flags.None, out props2));
				props |= props2;
			}
			return list;
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x00051F80 File Offset: 0x00050F80
		private Query ProcessNode(AstNode root, QueryBuilder.Flags flags, out QueryBuilder.Props props)
		{
			Query query = null;
			props = QueryBuilder.Props.None;
			switch (root.Type)
			{
			case AstNode.AstType.Axis:
				query = this.ProcessAxis((Axis)root, flags, out props);
				break;
			case AstNode.AstType.Operator:
				query = this.ProcessOperator((Operator)root, out props);
				break;
			case AstNode.AstType.Filter:
				query = this.ProcessFilter((Filter)root, flags, out props);
				break;
			case AstNode.AstType.ConstantOperand:
				query = new OperandQuery(((Operand)root).OperandValue);
				break;
			case AstNode.AstType.Function:
				query = this.ProcessFunction((Function)root, out props);
				break;
			case AstNode.AstType.Group:
				query = new GroupQuery(this.ProcessNode(((Group)root).GroupNode, QueryBuilder.Flags.None, out props));
				break;
			case AstNode.AstType.Root:
				query = new AbsoluteQuery();
				break;
			case AstNode.AstType.Variable:
				query = this.ProcessVariable((Variable)root);
				break;
			}
			return query;
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x0005204C File Offset: 0x0005104C
		private Query Build(AstNode root, string query)
		{
			this.Reset();
			this.query = query;
			QueryBuilder.Props props;
			return this.ProcessNode(root, QueryBuilder.Flags.None, out props);
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x00052072 File Offset: 0x00051072
		internal Query Build(string query, bool allowVar, bool allowKey)
		{
			this.allowVar = allowVar;
			this.allowKey = allowKey;
			this.allowCurrent = true;
			return this.Build(XPathParser.ParseXPathExpresion(query), query);
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x00052098 File Offset: 0x00051098
		internal Query Build(string query, out bool needContext)
		{
			Query query2 = this.Build(query, true, true);
			needContext = this.needContext;
			return query2;
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x000520B8 File Offset: 0x000510B8
		internal Query BuildPatternQuery(string query, bool allowVar, bool allowKey)
		{
			this.allowVar = allowVar;
			this.allowKey = allowKey;
			this.allowCurrent = false;
			return this.Build(XPathParser.ParseXPathPattern(query), query);
		}

		// Token: 0x060012D1 RID: 4817 RVA: 0x000520DC File Offset: 0x000510DC
		internal Query BuildPatternQuery(string query, out bool needContext)
		{
			Query query2 = this.BuildPatternQuery(query, true, true);
			needContext = this.needContext;
			return query2;
		}

		// Token: 0x04000BBC RID: 3004
		private string query;

		// Token: 0x04000BBD RID: 3005
		private bool allowVar;

		// Token: 0x04000BBE RID: 3006
		private bool allowKey;

		// Token: 0x04000BBF RID: 3007
		private bool allowCurrent;

		// Token: 0x04000BC0 RID: 3008
		private bool needContext;

		// Token: 0x04000BC1 RID: 3009
		private BaseAxisQuery firstInput;

		// Token: 0x02000157 RID: 343
		private enum Flags
		{
			// Token: 0x04000BC3 RID: 3011
			None,
			// Token: 0x04000BC4 RID: 3012
			SmartDesc,
			// Token: 0x04000BC5 RID: 3013
			PosFilter,
			// Token: 0x04000BC6 RID: 3014
			Filter = 4
		}

		// Token: 0x02000158 RID: 344
		private enum Props
		{
			// Token: 0x04000BC8 RID: 3016
			None,
			// Token: 0x04000BC9 RID: 3017
			PosFilter,
			// Token: 0x04000BCA RID: 3018
			HasPosition,
			// Token: 0x04000BCB RID: 3019
			HasLast = 4,
			// Token: 0x04000BCC RID: 3020
			NonFlat = 8
		}
	}
}
