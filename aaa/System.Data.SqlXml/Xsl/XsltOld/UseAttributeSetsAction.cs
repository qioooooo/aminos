using System;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x0200016E RID: 366
	internal class UseAttributeSetsAction : CompiledAction
	{
		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000F43 RID: 3907 RVA: 0x0004CD90 File Offset: 0x0004BD90
		internal XmlQualifiedName[] UsedSets
		{
			get
			{
				return this.useAttributeSets;
			}
		}

		// Token: 0x06000F44 RID: 3908 RVA: 0x0004CD98 File Offset: 0x0004BD98
		internal override void Compile(Compiler compiler)
		{
			this.useString = compiler.Input.Value;
			if (this.useString.Length == 0)
			{
				this.useAttributeSets = new XmlQualifiedName[0];
				return;
			}
			string[] array = XmlConvert.SplitString(this.useString);
			try
			{
				this.useAttributeSets = new XmlQualifiedName[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					this.useAttributeSets[i] = compiler.CreateXPathQName(array[i]);
				}
			}
			catch (XsltException)
			{
				if (!compiler.ForwardCompatibility)
				{
					throw;
				}
				this.useAttributeSets = new XmlQualifiedName[0];
			}
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x0004CE34 File Offset: 0x0004BE34
		internal override void Execute(Processor processor, ActionFrame frame)
		{
			switch (frame.State)
			{
			case 0:
				frame.Counter = 0;
				frame.State = 2;
				break;
			case 1:
				return;
			case 2:
				break;
			default:
				return;
			}
			if (frame.Counter < this.useAttributeSets.Length)
			{
				AttributeSetAction attributeSet = processor.RootAction.GetAttributeSet(this.useAttributeSets[frame.Counter]);
				frame.IncrementCounter();
				processor.PushActionFrame(attributeSet, frame.NodeSet);
				return;
			}
			frame.Finished();
		}

		// Token: 0x040009DE RID: 2526
		private const int ProcessingSets = 2;

		// Token: 0x040009DF RID: 2527
		private XmlQualifiedName[] useAttributeSets;

		// Token: 0x040009E0 RID: 2528
		private string useString;
	}
}
