using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Xml.Schema;
using System.Xml.XPath;
using MS.Internal.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000B5 RID: 181
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class XmlQueryRuntime
	{
		// Token: 0x06000894 RID: 2196 RVA: 0x0002A038 File Offset: 0x00029038
		internal XmlQueryRuntime(XmlQueryStaticData data, object defaultDataSource, XmlResolver dataSources, XsltArgumentList argList, XmlSequenceWriter seqWrt)
		{
			string[] names = data.Names;
			Int32Pair[] array = data.Filters;
			WhitespaceRuleLookup whitespaceRuleLookup = ((data.WhitespaceRules != null && data.WhitespaceRules.Count != 0) ? new WhitespaceRuleLookup(data.WhitespaceRules) : null);
			this.ctxt = new XmlQueryContext(this, defaultDataSource, dataSources, argList, whitespaceRuleLookup);
			this.xsltLib = null;
			this.earlyInfo = data.EarlyBound;
			this.earlyObjects = ((this.earlyInfo != null) ? new object[this.earlyInfo.Length] : null);
			this.globalNames = data.GlobalNames;
			this.globalValues = ((this.globalNames != null) ? new object[this.globalNames.Length] : null);
			this.nameTableQuery = this.ctxt.QueryNameTable;
			this.atomizedNames = null;
			if (names != null)
			{
				XmlNameTable defaultNameTable = this.ctxt.DefaultNameTable;
				this.atomizedNames = new string[names.Length];
				if (defaultNameTable != this.nameTableQuery && defaultNameTable != null)
				{
					for (int i = 0; i < names.Length; i++)
					{
						string text = defaultNameTable.Get(names[i]);
						this.atomizedNames[i] = this.nameTableQuery.Add(text ?? names[i]);
					}
				}
				else
				{
					for (int i = 0; i < names.Length; i++)
					{
						this.atomizedNames[i] = this.nameTableQuery.Add(names[i]);
					}
				}
			}
			this.filters = null;
			if (array != null)
			{
				this.filters = new XmlNavigatorFilter[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					this.filters[i] = XmlNavNameFilter.Create(this.atomizedNames[array[i].Left], this.atomizedNames[array[i].Right]);
				}
			}
			this.prefixMappingsList = data.PrefixMappingsList;
			this.types = data.Types;
			this.collations = data.Collations;
			this.docOrderCmp = new DocumentOrderComparer();
			this.indexes = null;
			this.stkOutput = new Stack<XmlQueryOutput>(16);
			this.output = new XmlQueryOutput(this, seqWrt);
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x0002A236 File Offset: 0x00029236
		public string[] DebugGetGlobalNames()
		{
			return this.globalNames;
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x0002A240 File Offset: 0x00029240
		public IList DebugGetGlobalValue(string name)
		{
			for (int i = 0; i < this.globalNames.Length; i++)
			{
				if (this.globalNames[i] == name)
				{
					return (IList)this.globalValues[i];
				}
			}
			return null;
		}

		// 