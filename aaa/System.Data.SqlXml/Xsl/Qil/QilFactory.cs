using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x0200004B RID: 75
	internal sealed class QilFactory
	{
		// Token: 0x060004B9 RID: 1209 RVA: 0x0001FA9D File Offset: 0x0001EA9D
		public QilFactory()
		{
			this.typeCheck = new QilTypeChecker();
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060004BA RID: 1210 RVA: 0x0001FAB0 File Offset: 0x0001EAB0
		public QilTypeChecker TypeChecker
		{
			get
			{
				return this.typeCheck;
			}
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0001FAB8 File Offset: 0x0001EAB8
		public QilExpression QilExpression(QilNode root, QilFactory factory)
		{
			QilExpression qilExpression = new QilExpression(QilNodeType.QilExpression, root, factory);
			qilExpression.XmlType = this.typeCheck.CheckQilExpression(qilExpression);
			return qilExpression;
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0001FAE4 File Offset: 0x0001EAE4
		public QilList FunctionList(IList<QilNode> values)
		{
			QilList qilList = this.FunctionList();
			qilList.Add(values);
			return qilList;
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0001FB00 File Offset: 0x0001EB00
		public QilList GlobalVariableList(IList<QilNode> values)
		{
			QilList qilList = this.GlobalVariableList();
			qilList.Add(values);
			return qilList;
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x0001FB1C File Offset: 0x0001EB1C
		public QilList GlobalParameterList(IList<QilNode> values)
		{
			QilList qilList = this.GlobalParameterList();
			qilList.Add(values);
			return qilList;
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0001FB38 File Offset: 0x0001EB38
		public QilList ActualParameterList(IList<QilNode> values)
		{
			QilList qilList = this.ActualParameterList();
			qilList.Add(values);
			return qilList;
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0001FB54 File Offset: 0x0001EB54
		public QilList FormalParameterList(IList<QilNode> values)
		{
			QilList qilList = this.FormalParameterList();
			qilList.Add(values);
			return qilList;
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0001FB70 File Offset: 0x0001EB70
		public QilList SortKeyList(IList<QilNode> values)
		{
			QilList qilList = this.SortKeyList();
			qilList.Add(values);
			return qilList;
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x0001FB8C File Offset: 0x0001EB8C
		public QilList BranchList(IList<QilNode> values)
		{
			QilList qilList = this.BranchList();
			qilList.Add(values);
			return qilList;
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0001FBA8 File Offset: 0x0001EBA8
		public QilList Sequence(IList<QilNode> values)
		{
			QilList qilList = this.Sequence();
			qilList.Add(values);
			return qilList;
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0001FBC4 File Offset: 0x0001EBC4
		public QilParameter Parameter(XmlQueryType xmlType)
		{
			return this.Parameter(null, null, xmlType);
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0001FBCF File Offset: 0x0001EBCF
		public QilStrConcat StrConcat(QilNode values)
		{
			return this.StrConcat(this.LiteralString(""), values);
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0001FBE3 File Offset: 0x0001EBE3
		public QilName LiteralQName(string local)
		{
			return this.LiteralQName(local, string.Empty, string.Empty);
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0001FBF6 File Offset: 0x0001EBF6
		public QilTargetType TypeAssert(QilNode expr, XmlQueryType xmlType)
		{
			return this.TypeAssert(expr, this.LiteralType(xmlType));
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0001FC06 File Offset: 0x0001EC06
		public QilTargetType IsType(QilNode expr, XmlQueryType xmlType)
		{
			return this.IsType(expr, this.LiteralType(xmlType));
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0001FC16 File Offset: 0x0001EC16
		public QilTargetType XsltConvert(QilNode expr, XmlQueryType xmlType)
		{
			return this.XsltConvert(expr, this.LiteralType(xmlType));
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0001FC26 File Offset: 0x0001EC26
		public QilFunction Function(QilNode arguments, QilNode sideEffects, XmlQueryType xmlType)
		{
			return this.Function(arguments, this.Unknown(xmlType), sideEffects, xmlType);
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x0001FC38 File Offset: 0x0001EC38
		public QilExpression QilExpression(QilNode root)
		{
			QilExpression qilExpression = new QilExpression(QilNodeType.QilExpression, root);
			qilExpression.XmlType = this.typeCheck.CheckQilExpression(qilExpression);
			return qilExpression;
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x0001FC60 File Offset: 0x0001EC60
		public QilList FunctionList()
		{
			QilList qilList = new QilList(QilNodeType.FunctionList);
			qilList.XmlType = this.typeCheck.CheckFunctionList(qilList);
			return qilList;
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x0001FC88 File Offset: 0x0001EC88
		public QilList GlobalVariableList()
		{
			QilList qilList = new QilList(QilNodeType.GlobalVariableList);
			qilList.XmlType = this.typeCheck.CheckGlobalVariableList(qilList);
			return qilList;
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x0001FCB0 File Offset: 0x0001ECB0
		public QilList GlobalParameterList()
		{
			QilList qilList = new QilList(QilNodeType.GlobalParameterList);
			qilList.XmlType = this.typeCheck.CheckGlobalParameterList(qilList);
			return qilList;
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x0001FCD8 File Offset: 0x0001ECD8
		public QilList ActualParameterList()
		{
			QilList qilList = new QilList(QilNodeType.ActualParameterList);
			qilList.XmlType = this.typeCheck.CheckActualParameterList(qilList);
			return qilList;
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0001FD00 File Offset: 0x0001ED00
		public QilList FormalParameterList()
		{
			QilList qilList = new QilList(QilNodeType.FormalParameterList);
			qilList.XmlType = this.typeCheck.CheckFormalParameterList(qilList);
			return qilList;
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x0001FD28 File Offset: 0x0001ED28
		public QilList SortKeyList()
		{
			QilList qilList = new QilList(QilNodeType.SortKeyList);
			qilList.XmlType = this.typeCheck.CheckSortKeyList(qilList);
			return qilList;
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0001FD50 File Offset: 0x0001ED50
		public QilList BranchList()
		{
			QilList qilList = new QilList(QilNodeType.BranchList);
			qilList.XmlType = this.typeCheck.CheckBranchList(qilList);
			return qilList;
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0001FD78 File Offset: 0x0001ED78
		public QilUnary OptimizeBarrier(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.OptimizeBarrier, child);
			qilUnary.XmlType = this.typeCheck.CheckOptimizeBarrier(qilUnary);
			return qilUnary;
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0001FDA0 File Offset: 0x0001EDA0
		public QilNode Unknown(XmlQueryType xmlType)
		{
			QilNode qilNode = new QilNode(QilNodeType.Unknown, xmlType);
			qilNode.XmlType = this.typeCheck.CheckUnknown(qilNode);
			return qilNode;
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x0001FDCC File Offset: 0x0001EDCC
		public QilDataSource DataSource(QilNode name, QilNode baseUri)
		{
			QilDataSource qilDataSource = new QilDataSource(QilNodeType.DataSource, name, baseUri);
			qilDataSource.XmlType = this.typeCheck.CheckDataSource(qilDataSource);
			return qilDataSource;
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x0001FDF8 File Offset: 0x0001EDF8
		public QilUnary Nop(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.Nop, child);
			qilUnary.XmlType = this.typeCheck.CheckNop(qilUnary);
			return qilUnary;
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0001FE24 File Offset: 0x0001EE24
		public QilUnary Error(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.Error, child);
			qilUnary.XmlType = this.typeCheck.CheckError(qilUnary);
			return qilUnary;
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x0001FE50 File Offset: 0x0001EE50
		public QilUnary Warning(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.Warning, child);
			qilUnary.XmlType = this.typeCheck.CheckWarning(qilUnary);
			return qilUnary;
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0001FE7C File Offset: 0x0001EE7C
		public QilIterator For(QilNode binding)
		{
			QilIterator qilIterator = new QilIterator(QilNodeType.For, binding);
			qilIterator.XmlType = this.typeCheck.CheckFor(qilIterator);
			return qilIterator;
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0001FEA8 File Offset: 0x0001EEA8
		public QilIterator Let(QilNode binding)
		{
			QilIterator qilIterator = new QilIterator(QilNodeType.Let, binding);
			qilIterator.XmlType = this.typeCheck.CheckLet(qilIterator);
			return qilIterator;
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x0001FED4 File Offset: 0x0001EED4
		public QilParameter Parameter(QilNode defaultValue, QilNode name, XmlQueryType xmlType)
		{
			QilParameter qilParameter = new QilParameter(QilNodeType.Parameter, defaultValue, name, xmlType);
			qilParameter.XmlType = this.typeCheck.CheckParameter(qilParameter);
			return qilParameter;
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0001FF00 File Offset: 0x0001EF00
		public QilUnary PositionOf(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.PositionOf, child);
			qilUnary.XmlType = this.typeCheck.CheckPositionOf(qilUnary);
			return qilUnary;
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0001FF2C File Offset: 0x0001EF2C
		public QilNode True()
		{
			QilNode qilNode = new QilNode(QilNodeType.True);
			qilNode.XmlType = this.typeCheck.CheckTrue(qilNode);
			return qilNode;
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0001FF54 File Offset: 0x0001EF54
		public QilNode False()
		{
			QilNode qilNode = new QilNode(QilNodeType.False);
			qilNode.XmlType = this.typeCheck.CheckFalse(qilNode);
			return qilNode;
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0001FF7C File Offset: 0x0001EF7C
		public QilLiteral LiteralString(string value)
		{
			QilLiteral qilLiteral = new QilLiteral(QilNodeType.LiteralString, value);
			qilLiteral.XmlType = this.typeCheck.CheckLiteralString(qilLiteral);
			return qilLiteral;
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0001FFA8 File Offset: 0x0001EFA8
		public QilLiteral LiteralInt32(int value)
		{
			QilLiteral qilLiteral = new QilLiteral(QilNodeType.LiteralInt32, value);
			qilLiteral.XmlType = this.typeCheck.CheckLiteralInt32(qilLiteral);
			return qilLiteral;
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0001FFD8 File Offset: 0x0001EFD8
		public QilLiteral LiteralInt64(long value)
		{
			QilLiteral qilLiteral = new QilLiteral(QilNodeType.LiteralInt64, value);
			qilLiteral.XmlType = this.typeCheck.CheckLiteralInt64(qilLiteral);
			return qilLiteral;
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x00020008 File Offset: 0x0001F008
		public QilLiteral LiteralDouble(double value)
		{
			QilLiteral qilLiteral = new QilLiteral(QilNodeType.LiteralDouble, value);
			qilLiteral.XmlType = this.typeCheck.CheckLiteralDouble(qilLiteral);
			return qilLiteral;
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00020038 File Offset: 0x0001F038
		public QilLiteral LiteralDecimal(decimal value)
		{
			QilLiteral qilLiteral = new QilLiteral(QilNodeType.LiteralDecimal, value);
			qilLiteral.XmlType = this.typeCheck.CheckLiteralDecimal(qilLiteral);
			return qilLiteral;
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00020068 File Offset: 0x0001F068
		public QilName LiteralQName(string localName, string namespaceUri, string prefix)
		{
			QilName qilName = new QilName(QilNodeType.LiteralQName, localName, namespaceUri, prefix);
			qilName.XmlType = this.typeCheck.CheckLiteralQName(qilName);
			return qilName;
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00020094 File Offset: 0x0001F094
		public QilLiteral LiteralType(XmlQueryType value)
		{
			QilLiteral qilLiteral = new QilLiteral(QilNodeType.LiteralType, value);
			qilLiteral.XmlType = this.typeCheck.CheckLiteralType(qilLiteral);
			return qilLiteral;
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x000200C0 File Offset: 0x0001F0C0
		public QilLiteral LiteralObject(object value)
		{
			QilLiteral qilLiteral = new QilLiteral(QilNodeType.LiteralObject, value);
			qilLiteral.XmlType = this.typeCheck.CheckLiteralObject(qilLiteral);
			return qilLiteral;
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x000200EC File Offset: 0x0001F0EC
		public QilBinary And(QilNode left, QilNode right)
		{
			QilBinary qilBinary = new QilBinary(QilNodeType.And, left, right);
			qilBinary.XmlType = this.typeCheck.CheckAnd(qilBinary);
			return qilBinary;
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00020118 File Offset: 0x0001F118
		public QilBinary Or(QilNode left, QilNode right)
		{
			QilBinary qilBinary = new QilBinary(QilNodeType.Or, left, right);
			qilBinary.XmlType = this.typeCheck.CheckOr(qilBinary);
			return qilBinary;
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00020144 File Offset: 0x0001F144
		public QilUnary Not(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.Not, child);
			qilUnary.XmlType = this.typeCheck.CheckNot(qilUnary);
			return qilUnary;
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00020170 File Offset: 0x0001F170
		public QilTernary Conditional(QilNode left, QilNode center, QilNode right)
		{
			QilTernary qilTernary = new QilTernary(QilNodeType.Conditional, left, center, right);
			qilTernary.XmlType = this.typeCheck.CheckConditional(qilTernary);
			return qilTernary;
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0002019C File Offset: 0x0001F19C
		public QilChoice Choice(QilNode expression, QilNode branches)
		{
			QilChoice qilChoice = new QilChoice(QilNodeType.Choice, expression, branches);
			qilChoice.XmlType = this.typeCheck.CheckChoice(qilChoice);
			return qilChoice;
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x000201C8 File Offset: 0x0001F1C8
		public QilUnary Length(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.Length, child);
			qilUnary.XmlType = this.typeCheck.CheckLength(qilUnary);
			return qilUnary;
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x000201F4 File Offset: 0x0001F1F4
		public QilList Sequence()
		{
			QilList qilList = new QilList(QilNodeType.Sequence);
			qilList.XmlType = this.typeCheck.CheckSequence(qilList);
			return qilList;
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0002021C File Offset: 0x0001F21C
		public QilBinary Union(QilNode left, QilNode right)
		{
			QilBinary qilBinary = new QilBinary(QilNodeType.Union, left, right);
			qilBinary.XmlType = this.typeCheck.CheckUnion(qilBinary);
			return qilBinary;
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00020248 File Offset: 0x0001F248
		public QilBinary Intersection(QilNode left, QilNode right)
		{
			QilBinary qilBinary = new QilBinary(QilNodeType.Intersection, left, right);
			qilBinary.XmlType = this.typeCheck.CheckIntersection(qilBinary);
			return qilBinary;
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00020274 File Offset: 0x0001F274
		public QilBinary Difference(QilNode left, QilNode right)
		{
			QilBinary qilBinary = new QilBinary(QilNodeType.Difference, left, right);
			qilBinary.XmlType = this.typeCheck.CheckDifference(qilBinary);
			return qilBinary;
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x000202A0 File Offset: 0x0001F2A0
		public QilUnary Average(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.Average, child);
			qilUnary.XmlType = this.typeCheck.CheckAverage(qilUnary);
			return qilUnary;
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x000202CC File Offset: 0x0001F2CC
		public QilUnary Sum(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.Sum, child);
			qilUnary.XmlType = this.typeCheck.CheckSum(qilUnary);
			return qilUnary;
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x000202F8 File Offset: 0x0001F2F8
		public QilUnary Minimum(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.Minimum, child);
			qilUnary.XmlType = this.typeCheck.CheckMinimum(qilUnary);
			return qilUnary;
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00020324 File Offset: 0x0001F324
		public QilUnary Maximum(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.Maximum, child);
			qilUnary.XmlType = this.typeCheck.CheckMaximum(qilUnary);
			return qilUnary;
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00020350 File Offset: 0x0001F350
		public QilUnary Negate(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.Negate, child);
			qilUnary.XmlType = this.typeCheck.CheckNegate(qilUnary);
			return qilUnary;
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0002037C File Offset: 0x0001F37C
		public QilBinary Add(QilNode left, QilNode right)
		{
			QilBinary qilBinary = new QilBinary(QilNodeType.Add, left, right);
			qilBinary.XmlType = this.typeCheck.CheckAdd(qilBinary);
			return qilBinary;
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x000203A8 File Offset: 0x0001F3A8
		public QilBinary Subtract(QilNode left, QilNode right)
		{
			QilBinary qilBinary = new QilBinary(QilNodeType.Subtract, left, right);
			qilBinary.XmlType = this.typeCheck.CheckSubtract(qilBinary);
			return qilBinary;
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x000203D4 File Offset: 0x0001F3D4
		public QilBinary Multiply(QilNode left, QilNode right)
		{
			QilBinary qilBinary = new QilBinary(QilNodeType.Multiply, left, right);
			qilBinary.XmlType = this.typeCheck.CheckMultiply(qilBinary);
			return qilBinary;
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00020400 File Offset: 0x0001F400
		public QilBinary Divide(QilNode left, QilNode right)
		{
			QilBinary qilBinary = new QilBinary(QilNodeType.Divide, left, right);
			qilBinary.XmlType = this.typeCheck.CheckDivide(qilBinary);
			return qilBinary;
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0002042C File Offset: 0x0001F42C
		public QilBinary Modulo(QilNode left, QilNode right)
		{
			QilBinary qilBinary = new QilBinary(QilNodeType.Modulo, left, right);
			qilBinary.XmlType = this.typeCheck.CheckModulo(qilBinary);
			return qilBinary;
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00020458 File Offset: 0x0001F458
		public QilUnary StrLength(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.StrLength, child);
			qilUnary.XmlType = this.typeCheck.CheckStrLength(qilUnary);
			return qilUnary;
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00020484 File Offset: 0x0001F484
		public QilStrConcat StrConcat(QilNode delimiter, QilNode values)
		{
			QilStrConcat qilStrConcat = new QilStrConcat(QilNodeType.StrConcat, delimiter, values);
			qilStrConcat.XmlType = this.typeCheck.CheckStrConcat(qilStrConcat);
			return qilStrConcat;
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x000204B0 File Offset: 0x0001F4B0
		public QilBinary StrParseQName(QilNode left, QilNode right)
		{
			QilBinary qilBinary = new QilBinary(QilNodeType.StrParseQName, left, right);
			qilBinary.XmlType = this.typeCheck.CheckStrParseQName(qilBinary);
			return qilBinary;
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x000204DC File Offset: 0x0001F4DC
		public QilBinary Ne(QilNode left, QilNode right)
		{
			QilBinary qilBinary = new QilBinary(QilNodeType.Ne, left, right);
			qilBinary.XmlType = this.typeCheck.CheckNe(qilBinary);
			return qilBinary;
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00020508 File Offset: 0x0001F508
		public QilBinary Eq(QilNode left, QilNode right)
		{
			QilBinary qilBinary = new QilBinary(QilNodeType.Eq, left, right);
			qilBinary.XmlType = this.typeCheck.CheckEq(qilBinary);
			return qilBinary;
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x00020534 File Offset: 0x0001F534
		public QilBinary Gt(QilNode left, QilNode right)
		{
			QilBinary qilBinary = new QilBinary(QilNodeType.Gt, left, right);
			qilBinary.XmlType = this.typeCheck.CheckGt(qilBinary);
			return qilBinary;
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x00020560 File Offset: 0x0001F560
		public QilBinary Ge(QilNode left, QilNode right)
		{
			QilBinary qilBinary = new QilBinary(QilNodeType.Ge, left, right);
			qilBinary.XmlType = this.typeCheck.CheckGe(qilBinary);
			return qilBinary;
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0002058C File Offset: 0x0001F58C
		public QilBinary Lt(QilNode left, QilNode right)
		{
			QilBinary qilBinary = new QilBinary(QilNodeType.Lt, left, right);
			qilBinary.XmlType = this.typeCheck.CheckLt(qilBinary);
			return qilBinary;
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x000205B8 File Offset: 0x0001F5B8
		public QilBinary Le(QilNode left, QilNode right)
		{
			QilBinary qilBinary = new QilBinary(QilNodeType.Le, left, right);
			qilBinary.XmlType = this.typeCheck.CheckLe(qilBinary);
			return qilBinary;
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x000205E4 File Offset: 0x0001F5E4
		public QilBinary Is(QilNode left, QilNode right)
		{
			QilBinary qilBinary = new QilBinary(QilNodeType.Is, left, right);
			qilBinary.XmlType = this.typeCheck.CheckIs(qilBinary);
			return qilBinary;
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00020610 File Offset: 0x0001F610
		public QilBinary After(QilNode left, QilNode right)
		{
			QilBinary qilBinary = new QilBinary(QilNodeType.After, left, right);
			qilBinary.XmlType = this.typeCheck.CheckAfter(qilBinary);
			return qilBinary;
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0002063C File Offset: 0x0001F63C
		public QilBinary Before(QilNode left, QilNode right)
		{
			QilBinary qilBinary = new QilBinary(QilNodeType.Before, left, right);
			qilBinary.XmlType = this.typeCheck.CheckBefore(qilBinary);
			return qilBinary;
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x00020668 File Offset: 0x0001F668
		public QilLoop Loop(QilNode variable, QilNode body)
		{
			QilLoop qilLoop = new QilLoop(QilNodeType.Loop, variable, body);
			qilLoop.XmlType = this.typeCheck.CheckLoop(qilLoop);
			return qilLoop;
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x00020694 File Offset: 0x0001F694
		public QilLoop Filter(QilNode variable, QilNode body)
		{
			QilLoop qilLoop = new QilLoop(QilNodeType.Filter, variable, body);
			qilLoop.XmlType = this.typeCheck.CheckFilter(qilLoop);
			return qilLoop;
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x000206C0 File Offset: 0x0001F6C0
		public QilLoop Sort(QilNode variable, QilNode body)
		{
			QilLoop qilLoop = new QilLoop(QilNodeType.Sort, variable, body);
			qilLoop.XmlType = this.typeCheck.CheckSort(qilLoop);
			return qilLoop;
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x000206EC File Offset: 0x0001F6EC
		public QilSortKey SortKey(QilNode key, QilNode collation)
		{
			QilSortKey qilSortKey = new QilSortKey(QilNodeType.SortKey, key, collation);
			qilSortKey.XmlType = this.typeCheck.CheckSortKey(qilSortKey);
			return qilSortKey;
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x00020718 File Offset: 0x0001F718
		public QilUnary DocOrderDistinct(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.DocOrderDistinct, child);
			qilUnary.XmlType = this.typeCheck.CheckDocOrderDistinct(qilUnary);
			return qilUnary;
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00020744 File Offset: 0x0001F744
		public QilFunction Function(QilNode arguments, QilNode definition, QilNode sideEffects, XmlQueryType xmlType)
		{
			QilFunction qilFunction = new QilFunction(QilNodeType.Function, arguments, definition, sideEffects, xmlType);
			qilFunction.XmlType = this.typeCheck.CheckFunction(qilFunction);
			return qilFunction;
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00020774 File Offset: 0x0001F774
		public QilInvoke Invoke(QilNode function, QilNode arguments)
		{
			QilInvoke qilInvoke = new QilInvoke(QilNodeType.Invoke, function, arguments);
			qilInvoke.XmlType = this.typeCheck.CheckInvoke(qilInvoke);
			return qilInvoke;
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x000207A0 File Offset: 0x0001F7A0
		public QilUnary Content(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.Content, child);
			qilUnary.XmlType = this.typeCheck.CheckContent(qilUnary);
			return qilUnary;
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x000207CC File Offset: 0x0001F7CC
		public QilBinary Attribute(QilNode left, QilNode right)
		{
			QilBinary qilBinary = new QilBinary(QilNodeType.Attribute, left, right);
			qilBinary.XmlType = this.typeCheck.CheckAttribute(qilBinary);
			return qilBinary;
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x000207F8 File Offset: 0x0001F7F8
		public QilUnary Parent(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.Parent, child);
			qilUnary.XmlType = this.typeCheck.CheckParent(qilUnary);
			return qilUnary;
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00020824 File Offset: 0x0001F824
		public QilUnary Root(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.Root, child);
			qilUnary.XmlType = this.typeCheck.CheckRoot(qilUnary);
			return qilUnary;
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00020850 File Offset: 0x0001F850
		public QilNode XmlContext()
		{
			QilNode qilNode = new QilNode(QilNodeType.XmlContext);
			qilNode.XmlType = this.typeCheck.CheckXmlContext(qilNode);
			return qilNode;
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00020878 File Offset: 0x0001F878
		public QilUnary Descendant(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.Descendant, child);
			qilUnary.XmlType = this.typeCheck.CheckDescendant(qilUnary);
			return qilUnary;
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x000208A4 File Offset: 0x0001F8A4
		public QilUnary DescendantOrSelf(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.DescendantOrSelf, child);
			qilUnary.XmlType = this.typeCheck.CheckDescendantOrSelf(qilUnary);
			return qilUnary;
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x000208D0 File Offset: 0x0001F8D0
		public QilUnary Ancestor(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.Ancestor, child);
			qilUnary.XmlType = this.typeCheck.CheckAncestor(qilUnary);
			return qilUnary;
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x000208FC File Offset: 0x0001F8FC
		public QilUnary AncestorOrSelf(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.AncestorOrSelf, child);
			qilUnary.XmlType = this.typeCheck.CheckAncestorOrSelf(qilUnary);
			return qilUnary;
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x00020928 File Offset: 0x0001F928
		public QilUnary Preceding(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.Preceding, child);
			qilUnary.XmlType = this.typeCheck.CheckPreceding(qilUnary);
			return qilUnary;
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x00020954 File Offset: 0x0001F954
		public QilUnary FollowingSibling(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.FollowingSibling, child);
			qilUnary.XmlType = this.typeCheck.CheckFollowingSibling(qilUnary);
			return qilUnary;
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x00020980 File Offset: 0x0001F980
		public QilUnary PrecedingSibling(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.PrecedingSibling, child);
			qilUnary.XmlType = this.typeCheck.CheckPrecedingSibling(qilUnary);
			return qilUnary;
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x000209AC File Offset: 0x0001F9AC
		public QilBinary NodeRange(QilNode left, QilNode right)
		{
			QilBinary qilBinary = new QilBinary(QilNodeType.NodeRange, left, right);
			qilBinary.XmlType = this.typeCheck.CheckNodeRange(qilBinary);
			return qilBinary;
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x000209D8 File Offset: 0x0001F9D8
		public QilBinary Deref(QilNode left, QilNode right)
		{
			QilBinary qilBinary = new QilBinary(QilNodeType.Deref, left, right);
			qilBinary.XmlType = this.typeCheck.CheckDeref(qilBinary);
			return qilBinary;
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00020A04 File Offset: 0x0001FA04
		public QilBinary ElementCtor(QilNode left, QilNode right)
		{
			QilBinary qilBinary = new QilBinary(QilNodeType.ElementCtor, left, right);
			qilBinary.XmlType = this.typeCheck.CheckElementCtor(qilBinary);
			return qilBinary;
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x00020A30 File Offset: 0x0001FA30
		public QilBinary AttributeCtor(QilNode left, QilNode right)
		{
			QilBinary qilBinary = new QilBinary(QilNodeType.AttributeCtor, left, right);
			qilBinary.XmlType = this.typeCheck.CheckAttributeCtor(qilBinary);
			return qilBinary;
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00020A5C File Offset: 0x0001FA5C
		public QilUnary CommentCtor(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.CommentCtor, child);
			qilUnary.XmlType = this.typeCheck.CheckCommentCtor(qilUnary);
			return qilUnary;
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00020A88 File Offset: 0x0001FA88
		public QilBinary PICtor(QilNode left, QilNode right)
		{
			QilBinary qilBinary = new QilBinary(QilNodeType.PICtor, left, right);
			qilBinary.XmlType = this.typeCheck.CheckPICtor(qilBinary);
			return qilBinary;
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00020AB4 File Offset: 0x0001FAB4
		public QilUnary TextCtor(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.TextCtor, child);
			qilUnary.XmlType = this.typeCheck.CheckTextCtor(qilUnary);
			return qilUnary;
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00020AE0 File Offset: 0x0001FAE0
		public QilUnary RawTextCtor(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.RawTextCtor, child);
			qilUnary.XmlType = this.typeCheck.CheckRawTextCtor(qilUnary);
			return qilUnary;
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00020B0C File Offset: 0x0001FB0C
		public QilUnary DocumentCtor(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.DocumentCtor, child);
			qilUnary.XmlType = this.typeCheck.CheckDocumentCtor(qilUnary);
			return qilUnary;
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x00020B38 File Offset: 0x0001FB38
		public QilBinary NamespaceDecl(QilNode left, QilNode right)
		{
			QilBinary qilBinary = new QilBinary(QilNodeType.NamespaceDecl, left, right);
			qilBinary.XmlType = this.typeCheck.CheckNamespaceDecl(qilBinary);
			return qilBinary;
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00020B64 File Offset: 0x0001FB64
		public QilBinary RtfCtor(QilNode left, QilNode right)
		{
			QilBinary qilBinary = new QilBinary(QilNodeType.RtfCtor, left, right);
			qilBinary.XmlType = this.typeCheck.CheckRtfCtor(qilBinary);
			return qilBinary;
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00020B90 File Offset: 0x0001FB90
		public QilUnary NameOf(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.NameOf, child);
			qilUnary.XmlType = this.typeCheck.CheckNameOf(qilUnary);
			return qilUnary;
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00020BBC File Offset: 0x0001FBBC
		public QilUnary LocalNameOf(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.LocalNameOf, child);
			qilUnary.XmlType = this.typeCheck.CheckLocalNameOf(qilUnary);
			return qilUnary;
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00020BE8 File Offset: 0x0001FBE8
		public QilUnary NamespaceUriOf(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.NamespaceUriOf, child);
			qilUnary.XmlType = this.typeCheck.CheckNamespaceUriOf(qilUnary);
			return qilUnary;
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00020C14 File Offset: 0x0001FC14
		public QilUnary PrefixOf(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.PrefixOf, child);
			qilUnary.XmlType = this.typeCheck.CheckPrefixOf(qilUnary);
			return qilUnary;
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x00020C40 File Offset: 0x0001FC40
		public QilTargetType TypeAssert(QilNode source, QilNode targetType)
		{
			QilTargetType qilTargetType = new QilTargetType(QilNodeType.TypeAssert, source, targetType);
			qilTargetType.XmlType = this.typeCheck.CheckTypeAssert(qilTargetType);
			return qilTargetType;
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x00020C6C File Offset: 0x0001FC6C
		public QilTargetType IsType(QilNode source, QilNode targetType)
		{
			QilTargetType qilTargetType = new QilTargetType(QilNodeType.IsType, source, targetType);
			qilTargetType.XmlType = this.typeCheck.CheckIsType(qilTargetType);
			return qilTargetType;
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00020C98 File Offset: 0x0001FC98
		public QilUnary IsEmpty(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.IsEmpty, child);
			qilUnary.XmlType = this.typeCheck.CheckIsEmpty(qilUnary);
			return qilUnary;
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x00020CC4 File Offset: 0x0001FCC4
		public QilUnary XPathNodeValue(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.XPathNodeValue, child);
			qilUnary.XmlType = this.typeCheck.CheckXPathNodeValue(qilUnary);
			return qilUnary;
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x00020CF0 File Offset: 0x0001FCF0
		public QilUnary XPathFollowing(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.XPathFollowing, child);
			qilUnary.XmlType = this.typeCheck.CheckXPathFollowing(qilUnary);
			return qilUnary;
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00020D1C File Offset: 0x0001FD1C
		public QilUnary XPathPreceding(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.XPathPreceding, child);
			qilUnary.XmlType = this.typeCheck.CheckXPathPreceding(qilUnary);
			return qilUnary;
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x00020D48 File Offset: 0x0001FD48
		public QilUnary XPathNamespace(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.XPathNamespace, child);
			qilUnary.XmlType = this.typeCheck.CheckXPathNamespace(qilUnary);
			return qilUnary;
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x00020D74 File Offset: 0x0001FD74
		public QilUnary XsltGenerateId(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.XsltGenerateId, child);
			qilUnary.XmlType = this.typeCheck.CheckXsltGenerateId(qilUnary);
			return qilUnary;
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x00020DA0 File Offset: 0x0001FDA0
		public QilInvokeLateBound XsltInvokeLateBound(QilNode name, QilNode arguments)
		{
			QilInvokeLateBound qilInvokeLateBound = new QilInvokeLateBound(QilNodeType.XsltInvokeLateBound, name, arguments);
			qilInvokeLateBound.XmlType = this.typeCheck.CheckXsltInvokeLateBound(qilInvokeLateBound);
			return qilInvokeLateBound;
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x00020DCC File Offset: 0x0001FDCC
		public QilInvokeEarlyBound XsltInvokeEarlyBound(QilNode name, QilNode clrMethod, QilNode arguments, XmlQueryType xmlType)
		{
			QilInvokeEarlyBound qilInvokeEarlyBound = new QilInvokeEarlyBound(QilNodeType.XsltInvokeEarlyBound, name, clrMethod, arguments, xmlType);
			qilInvokeEarlyBound.XmlType = this.typeCheck.CheckXsltInvokeEarlyBound(qilInvokeEarlyBound);
			return qilInvokeEarlyBound;
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00020DFC File Offset: 0x0001FDFC
		public QilBinary XsltCopy(QilNode left, QilNode right)
		{
			QilBinary qilBinary = new QilBinary(QilNodeType.XsltCopy, left, right);
			qilBinary.XmlType = this.typeCheck.CheckXsltCopy(qilBinary);
			return qilBinary;
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x00020E28 File Offset: 0x0001FE28
		public QilUnary XsltCopyOf(QilNode child)
		{
			QilUnary qilUnary = new QilUnary(QilNodeType.XsltCopyOf, child);
			qilUnary.XmlType = this.typeCheck.CheckXsltCopyOf(qilUnary);
			return qilUnary;
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x00020E54 File Offset: 0x0001FE54
		public QilTargetType XsltConvert(QilNode source, QilNode targetType)
		{
			QilTargetType qilTargetType = new QilTargetType(QilNodeType.XsltConvert, source, targetType);
			qilTargetType.XmlType = this.typeCheck.CheckXsltConvert(qilTargetType);
			return qilTargetType;
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x00020E7E File Offset: 0x0001FE7E
		[Conditional("QIL_TRACE_NODE_CREATION")]
		public void TraceNode(QilNode n)
		{
		}

		// Token: 0x0400038C RID: 908
		private QilTypeChecker typeCheck;
	}
}
