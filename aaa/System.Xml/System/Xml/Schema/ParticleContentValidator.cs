using System;
using System.Collections;

namespace System.Xml.Schema
{
	// Token: 0x020001A0 RID: 416
	internal sealed class ParticleContentValidator : ContentValidator
	{
		// Token: 0x06001585 RID: 5509 RVA: 0x0005F383 File Offset: 0x0005E383
		public ParticleContentValidator(XmlSchemaContentType contentType)
			: this(contentType, true)
		{
		}

		// Token: 0x06001586 RID: 5510 RVA: 0x0005F38D File Offset: 0x0005E38D
		public ParticleContentValidator(XmlSchemaContentType contentType, bool enableUpaCheck)
			: base(contentType)
		{
			this.enableUpaCheck = enableUpaCheck;
		}

		// Token: 0x06001587 RID: 5511 RVA: 0x0005F39D File Offset: 0x0005E39D
		public override void InitValidation(ValidationState context)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06001588 RID: 5512 RVA: 0x0005F3A4 File Offset: 0x0005E3A4
		public override object ValidateElement(XmlQualifiedName name, ValidationState context, out int errorCode)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06001589 RID: 5513 RVA: 0x0005F3AB File Offset: 0x0005E3AB
		public override bool CompleteValidation(ValidationState context)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x0600158A RID: 5514 RVA: 0x0005F3B2 File Offset: 0x0005E3B2
		public void Start()
		{
			this.symbols = new SymbolsDictionary();
			this.positions = new Positions();
			this.stack = new Stack();
		}

		// Token: 0x0600158B RID: 5515 RVA: 0x0005F3D5 File Offset: 0x0005E3D5
		public void OpenGroup()
		{
			this.stack.Push(null);
		}

		// Token: 0x0600158C RID: 5516 RVA: 0x0005F3E4 File Offset: 0x0005E3E4
		public void CloseGroup()
		{
			SyntaxTreeNode syntaxTreeNode = (SyntaxTreeNode)this.stack.Pop();
			if (syntaxTreeNode == null)
			{
				return;
			}
			if (this.stack.Count == 0)
			{
				this.contentNode = syntaxTreeNode;
				this.isPartial = false;
				return;
			}
			InteriorNode interiorNode = (InteriorNode)this.stack.Pop();
			if (interiorNode != null)
			{
				interiorNode.RightChild = syntaxTreeNode;
				syntaxTreeNode = interiorNode;
				this.isPartial = true;
			}
			else
			{
				this.isPartial = false;
			}
			this.stack.Push(syntaxTreeNode);
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x0005F45B File Offset: 0x0005E45B
		public bool Exists(XmlQualifiedName name)
		{
			return this.symbols.Exists(name);
		}

		// Token: 0x0600158E RID: 5518 RVA: 0x0005F46E File Offset: 0x0005E46E
		public void AddName(XmlQualifiedName name, object particle)
		{
			this.AddLeafNode(new LeafNode(this.positions.Add(this.symbols.AddName(name, particle), particle)));
		}

		// Token: 0x0600158F RID: 5519 RVA: 0x0005F494 File Offset: 0x0005E494
		public void AddNamespaceList(NamespaceList namespaceList, object particle)
		{
			this.symbols.AddNamespaceList(namespaceList, particle, false);
			this.AddLeafNode(new NamespaceListNode(namespaceList, particle));
		}

		// Token: 0x06001590 RID: 5520 RVA: 0x0005F4B4 File Offset: 0x0005E4B4
		private void AddLeafNode(SyntaxTreeNode node)
		{
			if (this.stack.Count > 0)
			{
				InteriorNode interiorNode = (InteriorNode)this.stack.Pop();
				if (interiorNode != null)
				{
					interiorNode.RightChild = node;
					node = interiorNode;
				}
			}
			this.stack.Push(node);
			this.isPartial = true;
		}

		// Token: 0x06001591 RID: 5521 RVA: 0x0005F500 File Offset: 0x0005E500
		public void AddChoice()
		{
			SyntaxTreeNode syntaxTreeNode = (SyntaxTreeNode)this.stack.Pop();
			InteriorNode interiorNode = new ChoiceNode();
			interiorNode.LeftChild = syntaxTreeNode;
			this.stack.Push(interiorNode);
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x0005F538 File Offset: 0x0005E538
		public void AddSequence()
		{
			SyntaxTreeNode syntaxTreeNode = (SyntaxTreeNode)this.stack.Pop();
			InteriorNode interiorNode = new SequenceNode();
			interiorNode.LeftChild = syntaxTreeNode;
			this.stack.Push(interiorNode);
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x0005F56F File Offset: 0x0005E56F
		public void AddStar()
		{
			this.Closure(new StarNode());
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x0005F57C File Offset: 0x0005E57C
		public void AddPlus()
		{
			this.Closure(new PlusNode());
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x0005F589 File Offset: 0x0005E589
		public void AddQMark()
		{
			this.Closure(new QmarkNode());
		}

		// Token: 0x06001596 RID: 5526 RVA: 0x0005F598 File Offset: 0x0005E598
		public void AddLeafRange(decimal min, decimal max)
		{
			LeafRangeNode leafRangeNode = new LeafRangeNode(min, max);
			int num = this.positions.Add(-2, leafRangeNode);
			leafRangeNode.Pos = num;
			this.Closure(new SequenceNode
			{
				RightChild = leafRangeNode
			});
			this.minMaxNodesCount++;
		}

		// Token: 0x06001597 RID: 5527 RVA: 0x0005F5E8 File Offset: 0x0005E5E8
		private void Closure(InteriorNode node)
		{
			if (this.stack.Count > 0)
			{
				SyntaxTreeNode syntaxTreeNode = (SyntaxTreeNode)this.stack.Pop();
				InteriorNode interiorNode = syntaxTreeNode as InteriorNode;
				if (this.isPartial && interiorNode != null)
				{
					node.LeftChild = interiorNode.RightChild;
					interiorNode.RightChild = node;
				}
				else
				{
					node.LeftChild = syntaxTreeNode;
					syntaxTreeNode = node;
				}
				this.stack.Push(syntaxTreeNode);
				return;
			}
			if (this.contentNode != null)
			{
				node.LeftChild = this.contentNode;
				this.contentNode = node;
			}
		}

		// Token: 0x06001598 RID: 5528 RVA: 0x0005F66C File Offset: 0x0005E66C
		public ContentValidator Finish()
		{
			return this.Finish(true);
		}

		// Token: 0x06001599 RID: 5529 RVA: 0x0005F678 File Offset: 0x0005E678
		public ContentValidator Finish(bool useDFA)
		{
			if (this.contentNode == null)
			{
				if (base.ContentType != XmlSchemaContentType.Mixed)
				{
					return ContentValidator.Empty;
				}
				bool isOpen = base.IsOpen;
				if (!base.IsOpen)
				{
					return ContentValidator.TextOnly;
				}
				return ContentValidator.Any;
			}
			else
			{
				InteriorNode interiorNode = new SequenceNode();
				interiorNode.LeftChild = this.contentNode;
				LeafNode leafNode = new LeafNode(this.positions.Add(this.symbols.AddName(XmlQualifiedName.Empty, null), null));
				interiorNode.RightChild = leafNode;
				this.contentNode.ExpandTree(interiorNode, this.symbols, this.positions);
				int count = this.symbols.Count;
				int count2 = this.positions.Count;
				BitSet bitSet = new BitSet(count2);
				BitSet bitSet2 = new BitSet(count2);
				BitSet[] array = new BitSet[count2];
				for (int i = 0; i < count2; i++)
				{
					array[i] = new BitSet(count2);
				}
				interiorNode.ConstructPos(bitSet, bitSet2, array);
				if (this.minMaxNodesCount > 0)
				{
					BitSet bitSet3;
					BitSet[] array2 = this.CalculateTotalFollowposForRangeNodes(bitSet, array, out bitSet3);
					if (this.enableUpaCheck)
					{
						this.CheckCMUPAWithLeafRangeNodes(this.GetApplicableMinMaxFollowPos(bitSet, bitSet3, array2));
						for (int j = 0; j < count2; j++)
						{
							this.CheckCMUPAWithLeafRangeNodes(this.GetApplicableMinMaxFollowPos(array[j], bitSet3, array2));
						}
					}
					return new RangeContentValidator(bitSet, array, this.symbols, this.positions, leafNode.Pos, base.ContentType, interiorNode.LeftChild.IsNullable, bitSet3, this.minMaxNodesCount);
				}
				int[][] array3 = null;
				if (!this.symbols.IsUpaEnforced)
				{
					if (this.enableUpaCheck)
					{
						this.CheckUniqueParticleAttribution(bitSet, array);
					}
				}
				else if (useDFA)
				{
					array3 = this.BuildTransitionTable(bitSet, array, leafNode.Pos);
				}
				if (array3 != null)
				{
					return new DfaContentValidator(array3, this.symbols, base.ContentType, base.IsOpen, interiorNode.LeftChild.IsNullable);
				}
				return new NfaContentValidator(bitSet, array, this.symbols, this.positions, leafNode.Pos, base.ContentType, base.IsOpen, interiorNode.LeftChild.IsNullable);
			}
		}

		// Token: 0x0600159A RID: 5530 RVA: 0x0005F87C File Offset: 0x0005E87C
		private BitSet[] CalculateTotalFollowposForRangeNodes(BitSet firstpos, BitSet[] followpos, out BitSet posWithRangeTerminals)
		{
			int count = this.positions.Count;
			posWithRangeTerminals = new BitSet(count);
			BitSet[] array = new BitSet[this.minMaxNodesCount];
			int num = 0;
			for (int i = count - 1; i >= 0; i--)
			{
				Position position = this.positions[i];
				if (position.symbol == -2)
				{
					LeafRangeNode leafRangeNode = position.particle as LeafRangeNode;
					BitSet bitSet = new BitSet(count);
					bitSet.Clear();
					bitSet.Or(followpos[i]);
					if (leafRangeNode.Min != leafRangeNode.Max)
					{
						bitSet.Or(leafRangeNode.NextIteration);
					}
					for (int num2 = bitSet.NextSet(-1); num2 != -1; num2 = bitSet.NextSet(num2))
					{
						if (num2 > i)
						{
							Position position2 = this.positions[num2];
							if (position2.symbol == -2)
							{
								LeafRangeNode leafRangeNode2 = position2.particle as LeafRangeNode;
								bitSet.Or(array[leafRangeNode2.Pos]);
							}
						}
					}
					array[num] = bitSet;
					leafRangeNode.Pos = num++;
					posWithRangeTerminals.Set(i);
				}
			}
			return array;
		}

		// Token: 0x0600159B RID: 5531 RVA: 0x0005F998 File Offset: 0x0005E998
		private void CheckCMUPAWithLeafRangeNodes(BitSet curpos)
		{
			object[] array = new object[this.symbols.Count];
			for (int num = curpos.NextSet(-1); num != -1; num = curpos.NextSet(num))
			{
				Position position = this.positions[num];
				int symbol = position.symbol;
				if (symbol >= 0)
				{
					if (array[symbol] != null)
					{
						throw new UpaException(array[symbol], position.particle);
					}
					array[symbol] = position.particle;
				}
			}
		}

		// Token: 0x0600159C RID: 5532 RVA: 0x0005FA04 File Offset: 0x0005EA04
		private BitSet GetApplicableMinMaxFollowPos(BitSet curpos, BitSet posWithRangeTerminals, BitSet[] minmaxFollowPos)
		{
			if (curpos.Intersects(posWithRangeTerminals))
			{
				BitSet bitSet = new BitSet(this.positions.Count);
				bitSet.Or(curpos);
				bitSet.And(posWithRangeTerminals);
				curpos = curpos.Clone();
				for (int num = bitSet.NextSet(-1); num != -1; num = bitSet.NextSet(num))
				{
					LeafRangeNode leafRangeNode = this.positions[num].particle as LeafRangeNode;
					curpos.Or(minmaxFollowPos[leafRangeNode.Pos]);
				}
			}
			return curpos;
		}

		// Token: 0x0600159D RID: 5533 RVA: 0x0005FA80 File Offset: 0x0005EA80
		private void CheckUniqueParticleAttribution(BitSet firstpos, BitSet[] followpos)
		{
			this.CheckUniqueParticleAttribution(firstpos);
			for (int i = 0; i < this.positions.Count; i++)
			{
				this.CheckUniqueParticleAttribution(followpos[i]);
			}
		}

		// Token: 0x0600159E RID: 5534 RVA: 0x0005FAB4 File Offset: 0x0005EAB4
		private void CheckUniqueParticleAttribution(BitSet curpos)
		{
			object[] array = new object[this.symbols.Count];
			for (int num = curpos.NextSet(-1); num != -1; num = curpos.NextSet(num))
			{
				int symbol = this.positions[num].symbol;
				if (array[symbol] == null)
				{
					array[symbol] = this.positions[num].particle;
				}
				else if (array[symbol] != this.positions[num].particle)
				{
					throw new UpaException(array[symbol], this.positions[num].particle);
				}
			}
		}

		// Token: 0x0600159F RID: 5535 RVA: 0x0005FB48 File Offset: 0x0005EB48
		private int[][] BuildTransitionTable(BitSet firstpos, BitSet[] followpos, int endMarkerPos)
		{
			int count = this.positions.Count;
			int num = 8192 / count;
			int count2 = this.symbols.Count;
			ArrayList arrayList = new ArrayList();
			Hashtable hashtable = new Hashtable();
			hashtable.Add(new BitSet(count), -1);
			Queue queue = new Queue();
			int num2 = 0;
			queue.Enqueue(firstpos);
			hashtable.Add(firstpos, 0);
			arrayList.Add(new int[count2 + 1]);
			while (queue.Count > 0)
			{
				BitSet bitSet = (BitSet)queue.Dequeue();
				int[] array = (int[])arrayList[num2];
				if (bitSet[endMarkerPos])
				{
					array[count2] = 1;
				}
				for (int i = 0; i < count2; i++)
				{
					BitSet bitSet2 = new BitSet(count);
					for (int num3 = bitSet.NextSet(-1); num3 != -1; num3 = bitSet.NextSet(num3))
					{
						if (i == this.positions[num3].symbol)
						{
							bitSet2.Or(followpos[num3]);
						}
					}
					object obj = hashtable[bitSet2];
					if (obj != null)
					{
						array[i] = (int)obj;
					}
					else
					{
						int num4 = hashtable.Count - 1;
						if (num4 >= num)
						{
							return null;
						}
						queue.Enqueue(bitSet2);
						hashtable.Add(bitSet2, num4);
						arrayList.Add(new int[count2 + 1]);
						array[i] = num4;
					}
				}
				num2++;
			}
			return (int[][])arrayList.ToArray(typeof(int[]));
		}

		// Token: 0x04000CD3 RID: 3283
		private SymbolsDictionary symbols;

		// Token: 0x04000CD4 RID: 3284
		private Positions positions;

		// Token: 0x04000CD5 RID: 3285
		private Stack stack;

		// Token: 0x04000CD6 RID: 3286
		private SyntaxTreeNode contentNode;

		// Token: 0x04000CD7 RID: 3287
		private bool isPartial;

		// Token: 0x04000CD8 RID: 3288
		private int minMaxNodesCount;

		// Token: 0x04000CD9 RID: 3289
		private bool enableUpaCheck;
	}
}
