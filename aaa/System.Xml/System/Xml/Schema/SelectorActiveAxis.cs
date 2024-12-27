using System;
using System.Collections;

namespace System.Xml.Schema
{
	// Token: 0x0200018C RID: 396
	internal class SelectorActiveAxis : ActiveAxis
	{
		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x0600150F RID: 5391 RVA: 0x0005E08F File Offset: 0x0005D08F
		public bool EmptyStack
		{
			get
			{
				return this.KSpointer == 0;
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06001510 RID: 5392 RVA: 0x0005E09A File Offset: 0x0005D09A
		public int lastDepth
		{
			get
			{
				if (this.KSpointer != 0)
				{
					return ((KSStruct)this.KSs[this.KSpointer - 1]).depth;
				}
				return -1;
			}
		}

		// Token: 0x06001511 RID: 5393 RVA: 0x0005E0C3 File Offset: 0x0005D0C3
		public SelectorActiveAxis(Asttree axisTree, ConstraintStruct cs)
			: base(axisTree)
		{
			this.KSs = new ArrayList();
			this.cs = cs;
		}

		// Token: 0x06001512 RID: 5394 RVA: 0x0005E0DE File Offset: 0x0005D0DE
		public override bool EndElement(string localname, string URN)
		{
			base.EndElement(localname, URN);
			return this.KSpointer > 0 && base.CurrentDepth == this.lastDepth;
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x0005E104 File Offset: 0x0005D104
		public int PushKS(int errline, int errcol)
		{
			KeySequence keySequence = new KeySequence(this.cs.TableDim, errline, errcol);
			KSStruct ksstruct;
			if (this.KSpointer < this.KSs.Count)
			{
				ksstruct = (KSStruct)this.KSs[this.KSpointer];
				ksstruct.ks = keySequence;
				for (int i = 0; i < this.cs.TableDim; i++)
				{
					ksstruct.fields[i].Reactivate(keySequence);
				}
			}
			else
			{
				ksstruct = new KSStruct(keySequence, this.cs.TableDim);
				for (int j = 0; j < this.cs.TableDim; j++)
				{
					ksstruct.fields[j] = new LocatedActiveAxis(this.cs.constraint.Fields[j], keySequence, j);
					this.cs.axisFields.Add(ksstruct.fields[j]);
				}
				this.KSs.Add(ksstruct);
			}
			ksstruct.depth = base.CurrentDepth - 1;
			return this.KSpointer++;
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x0005E20C File Offset: 0x0005D20C
		public KeySequence PopKS()
		{
			return ((KSStruct)this.KSs[--this.KSpointer]).ks;
		}

		// Token: 0x04000CA6 RID: 3238
		private ConstraintStruct cs;

		// Token: 0x04000CA7 RID: 3239
		private ArrayList KSs;

		// Token: 0x04000CA8 RID: 3240
		private int KSpointer;
	}
}
