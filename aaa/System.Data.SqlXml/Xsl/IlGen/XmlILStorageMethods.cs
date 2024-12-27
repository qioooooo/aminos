using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.XPath;
using System.Xml.Xsl.Runtime;

namespace System.Xml.Xsl.IlGen
{
	// Token: 0x0200001E RID: 30
	internal class XmlILStorageMethods
	{
		// Token: 0x06000138 RID: 312 RVA: 0x00009900 File Offset: 0x00008900
		public XmlILStorageMethods(Type storageType)
		{
			if (storageType == typeof(int) || storageType == typeof(long) || storageType == typeof(decimal) || storageType == typeof(double))
			{
				Type type = Type.GetType("System.Xml.Xsl.Runtime." + storageType.Name + "Aggregator");
				this.AggAvg = XmlILMethods.GetMethod(type, "Average");
				this.AggAvgResult = XmlILMethods.GetMethod(type, "get_AverageResult");
				this.AggCreate = XmlILMethods.GetMethod(type, "Create");
				this.AggIsEmpty = XmlILMethods.GetMethod(type, "get_IsEmpty");
				this.AggMax = XmlILMethods.GetMethod(type, "Maximum");
				this.AggMaxResult = XmlILMethods.GetMethod(type, "get_MaximumResult");
				this.AggMin = XmlILMethods.GetMethod(type, "Minimum");
				this.AggMinResult = XmlILMethods.GetMethod(type, "get_MinimumResult");
				this.AggSum = XmlILMethods.GetMethod(type, "Sum");
				this.AggSumResult = XmlILMethods.GetMethod(type, "get_SumResult");
			}
			if (storageType == typeof(XPathNavigator))
			{
				this.SeqType = typeof(XmlQueryNodeSequence);
				this.SeqAdd = XmlILMethods.GetMethod(this.SeqType, "AddClone");
			}
			else if (storageType == typeof(XPathItem))
			{
				this.SeqType = typeof(XmlQueryItemSequence);
				this.SeqAdd = XmlILMethods.GetMethod(this.SeqType, "AddClone");
			}
			else
			{
				this.SeqType = typeof(XmlQuerySequence<>).MakeGenericType(new Type[] { storageType });
				this.SeqAdd = XmlILMethods.GetMethod(this.SeqType, "Add");
			}
			this.SeqEmpty = this.SeqType.GetField("Empty");
			this.SeqReuse = XmlILMethods.GetMethod(this.SeqType, "CreateOrReuse", new Type[] { this.SeqType });
			this.SeqReuseSgl = XmlILMethods.GetMethod(this.SeqType, "CreateOrReuse", new Type[] { this.SeqType, storageType });
			this.SeqSortByKeys = XmlILMethods.GetMethod(this.SeqType, "SortByKeys");
			this.IListType = typeof(IList<>).MakeGenericType(new Type[] { storageType });
			this.IListItem = XmlILMethods.GetMethod(this.IListType, "get_Item");
			this.IListCount = XmlILMethods.GetMethod(typeof(ICollection<>).MakeGenericType(new Type[] { storageType }), "get_Count");
			if (storageType == typeof(string))
			{
				this.ValueAs = XmlILMethods.GetMethod(typeof(XPathItem), "get_Value");
			}
			else if (storageType == typeof(int))
			{
				this.ValueAs = XmlILMethods.GetMethod(typeof(XPathItem), "get_ValueAsInt");
			}
			else if (storageType == typeof(long))
			{
				this.ValueAs = XmlILMethods.GetMethod(typeof(XPathItem), "get_ValueAsLong");
			}
			else if (storageType == typeof(DateTime))
			{
				this.ValueAs = XmlILMethods.GetMethod(typeof(XPathItem), "get_ValueAsDateTime");
			}
			else if (storageType == typeof(double))
			{
				this.ValueAs = XmlILMethods.GetMethod(typeof(XPathItem), "get_ValueAsDouble");
			}
			else if (storageType == typeof(bool))
			{
				this.ValueAs = XmlILMethods.GetMethod(typeof(XPathItem), "get_ValueAsBoolean");
			}
			if (storageType == typeof(byte[]))
			{
				this.ToAtomicValue = XmlILMethods.GetMethod(typeof(XmlILStorageConverter), "BytesToAtomicValue");
				return;
			}
			if (storageType != typeof(XPathItem) && storageType != typeof(XPathNavigator))
			{
				this.ToAtomicValue = XmlILMethods.GetMethod(typeof(XmlILStorageConverter), storageType.Name + "ToAtomicValue");
			}
		}

		// Token: 0x0400014A RID: 330
		public MethodInfo AggAvg;

		// Token: 0x0400014B RID: 331
		public MethodInfo AggAvgResult;

		// Token: 0x0400014C RID: 332
		public MethodInfo AggCreate;

		// Token: 0x0400014D RID: 333
		public MethodInfo AggIsEmpty;

		// Token: 0x0400014E RID: 334
		public MethodInfo AggMax;

		// Token: 0x0400014F RID: 335
		public MethodInfo AggMaxResult;

		// Token: 0x04000150 RID: 336
		public MethodInfo AggMin;

		// Token: 0x04000151 RID: 337
		public MethodInfo AggMinResult;

		// Token: 0x04000152 RID: 338
		public MethodInfo AggSum;

		// Token: 0x04000153 RID: 339
		public MethodInfo AggSumResult;

		// Token: 0x04000154 RID: 340
		public Type SeqType;

		// Token: 0x04000155 RID: 341
		public FieldInfo SeqEmpty;

		// Token: 0x04000156 RID: 342
		public MethodInfo SeqReuse;

		// Token: 0x04000157 RID: 343
		public MethodInfo SeqReuseSgl;

		// Token: 0x04000158 RID: 344
		public MethodInfo SeqAdd;

		// Token: 0x04000159 RID: 345
		public MethodInfo SeqSortByKeys;

		// Token: 0x0400015A RID: 346
		public Type IListType;

		// Token: 0x0400015B RID: 347
		public MethodInfo IListCount;

		// Token: 0x0400015C RID: 348
		public MethodInfo IListItem;

		// Token: 0x0400015D RID: 349
		public MethodInfo ValueAs;

		// Token: 0x0400015E RID: 350
		public MethodInfo ToAtomicValue;
	}
}
