using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security;
using System.Xml.XPath;
using System.Xml.Xsl.Runtime;
using MS.Internal.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000166 RID: 358
	internal class RootAction : TemplateBaseAction
	{
		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000F0A RID: 3850 RVA: 0x0004BCBF File Offset: 0x0004ACBF
		internal XsltOutput Output
		{
			get
			{
				if (this.output == null)
				{
					this.output = new XsltOutput();
				}
				return this.output;
			}
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x0004BCDA File Offset: 0x0004ACDA
		internal override void Compile(Compiler compiler)
		{
			base.CompileDocument(compiler, false);
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x0004BCE4 File Offset: 0x0004ACE4
		internal void InsertKey(XmlQualifiedName name, int MatchKey, int UseKey)
		{
			if (this.keyList == null)
			{
				this.keyList = new List<Key>();
			}
			this.keyList.Add(new Key(name, MatchKey, UseKey));
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x0004BD0C File Offset: 0x0004AD0C
		internal AttributeSetAction GetAttributeSet(XmlQualifiedName name)
		{
			AttributeSetAction attributeSetAction = (AttributeSetAction)this.attributeSetTable[name];
			if (attributeSetAction == null)
			{
				throw XsltException.Create("Xslt_NoAttributeSet", new string[] { name.ToString() });
			}
			return attributeSetAction;
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x0004BD4C File Offset: 0x0004AD4C
		public void PorcessAttributeSets(Stylesheet rootStylesheet)
		{
			this.MirgeAttributeSets(rootStylesheet);
			foreach (object obj in this.attributeSetTable.Values)
			{
				AttributeSetAction attributeSetAction = (AttributeSetAction)obj;
				if (attributeSetAction.containedActions != null)
				{
					attributeSetAction.containedActions.Reverse();
				}
			}
			this.CheckAttributeSets_RecurceInList(new Hashtable(), this.attributeSetTable.Keys);
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x0004BDD4 File Offset: 0x0004ADD4
		private void MirgeAttributeSets(Stylesheet stylesheet)
		{
			if (stylesheet.AttributeSetTable != null)
			{
				foreach (object obj in stylesheet.AttributeSetTable.Values)
				{
					AttributeSetAction attributeSetAction = (AttributeSetAction)obj;
					ArrayList containedActions = attributeSetAction.containedActions;
					AttributeSetAction attributeSetAction2 = (AttributeSetAction)this.attributeSetTable[attributeSetAction.Name];
					if (attributeSetAction2 == null)
					{
						attributeSetAction2 = new AttributeSetAction();
						attributeSetAction2.name = attributeSetAction.Name;
						attributeSetAction2.containedActions = new ArrayList();
						this.attributeSetTable[attributeSetAction.Name] = attributeSetAction2;
					}
					ArrayList containedActions2 = attributeSetAction2.containedActions;
					if (containedActions != null)
					{
						int num = containedActions.Count - 1;
						while (0 <= num)
						{
							containedActions2.Add(containedActions[num]);
							num--;
						}
					}
				}
			}
			foreach (object obj2 in stylesheet.Imports)
			{
				Stylesheet stylesheet2 = (Stylesheet)obj2;
				this.MirgeAttributeSets(stylesheet2);
			}
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x0004BF14 File Offset: 0x0004AF14
		private void CheckAttributeSets_RecurceInList(Hashtable markTable, ICollection setQNames)
		{
			foreach (object obj in setQNames)
			{
				XmlQualifiedName xmlQualifiedName = (XmlQualifiedName)obj;
				object obj2 = markTable[xmlQualifiedName];
				if (obj2 == "P")
				{
					throw XsltException.Create("Xslt_CircularAttributeSet", new string[] { xmlQualifiedName.ToString() });
				}
				if (obj2 != "D")
				{
					markTable[xmlQualifiedName] = "P";
					this.CheckAttributeSets_RecurceInContainer(markTable, this.GetAttributeSet(xmlQualifiedName));
					markTable[xmlQualifiedName] = "D";
				}
			}
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x0004BFC0 File Offset: 0x0004AFC0
		private void CheckAttributeSets_RecurceInContainer(Hashtable markTable, ContainerAction container)
		{
			if (container.containedActions == null)
			{
				return;
			}
			foreach (object obj in container.containedActions)
			{
				Action action = (Action)obj;
				if (action is UseAttributeSetsAction)
				{
					this.CheckAttributeSets_RecurceInList(markTable, ((UseAttributeSetsAction)action).UsedSets);
				}
				else if (action is ContainerAction)
				{
					this.CheckAttributeSets_RecurceInContainer(markTable, (ContainerAction)action);
				}
			}
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x0004C04C File Offset: 0x0004B04C
		internal void AddDecimalFormat(XmlQualifiedName name, DecimalFormat formatinfo)
		{
			DecimalFormat decimalFormat = (DecimalFormat)this.decimalFormatTable[name];
			if (decimalFormat != null)
			{
				NumberFormatInfo info = decimalFormat.info;
				NumberFormatInfo info2 = formatinfo.info;
				if (info.NumberDecimalSeparator != info2.NumberDecimalSeparator || info.NumberGroupSeparator != info2.NumberGroupSeparator || info.PositiveInfinitySymbol != info2.PositiveInfinitySymbol || info.NegativeSign != info2.NegativeSign || info.NaNSymbol != info2.NaNSymbol || info.PercentSymbol != info2.PercentSymbol || info.PerMilleSymbol != info2.PerMilleSymbol || decimalFormat.zeroDigit != formatinfo.zeroDigit || decimalFormat.digit != formatinfo.digit || decimalFormat.patternSeparator != formatinfo.patternSeparator)
				{
					throw XsltException.Create("Xslt_DupDecimalFormat", new string[] { name.ToString() });
				}
			}
			this.decimalFormatTable[name] = formatinfo;
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x0004C15D File Offset: 0x0004B15D
		internal DecimalFormat GetDecimalFormat(XmlQualifiedName name)
		{
			return this.decimalFormatTable[name] as DecimalFormat;
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000F14 RID: 3860 RVA: 0x0004C170 File Offset: 0x0004B170
		internal List<Key> KeyList
		{
			get
			{
				return this.keyList;
			}
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x0004C178 File Offset: 0x0004B178
		internal override void Execute(Processor processor, ActionFrame frame)
		{
			switch (frame.State)
			{
			case 0:
			{
				frame.AllocateVariables(this.variableCount);
				XPathNavigator xpathNavigator = processor.Document.Clone();
				xpathNavigator.MoveToRoot();
				frame.InitNodeSet(new XPathSingletonIterator(xpathNavigator));
				if (this.containedActions != null && this.containedActions.Count > 0)
				{
					processor.PushActionFrame(frame);
				}
				frame.State = 2;
				return;
			}
			case 1:
				break;
			case 2:
				frame.NextNode(processor);
				if (processor.Debugger != null)
				{
					processor.PopDebuggerStack();
				}
				processor.PushTemplateLookup(frame.NodeSet, null, null);
				frame.State = 3;
				return;
			case 3:
				frame.Finished();
				break;
			default:
				return;
			}
		}

		// Token: 0x040009BE RID: 2494
		private const int QueryInitialized = 2;

		// Token: 0x040009BF RID: 2495
		private const int RootProcessed = 3;

		// Token: 0x040009C0 RID: 2496
		private Hashtable attributeSetTable = new Hashtable();

		// Token: 0x040009C1 RID: 2497
		private Hashtable decimalFormatTable = new Hashtable();

		// Token: 0x040009C2 RID: 2498
		private List<Key> keyList;

		// Token: 0x040009C3 RID: 2499
		private XsltOutput output;

		// Token: 0x040009C4 RID: 2500
		public Stylesheet builtInSheet;

		// Token: 0x040009C5 RID: 2501
		public PermissionSet permissions;
	}
}
