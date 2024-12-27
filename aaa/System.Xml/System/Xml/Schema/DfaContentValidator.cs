using System;
using System.Collections;

namespace System.Xml.Schema
{
	// Token: 0x020001A1 RID: 417
	internal sealed class DfaContentValidator : ContentValidator
	{
		// Token: 0x060015A0 RID: 5536 RVA: 0x0005FCD6 File Offset: 0x0005ECD6
		internal DfaContentValidator(int[][] transitionTable, SymbolsDictionary symbols, XmlSchemaContentType contentType, bool isOpen, bool isEmptiable)
			: base(contentType, isOpen, isEmptiable)
		{
			this.transitionTable = transitionTable;
			this.symbols = symbols;
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x0005FCF1 File Offset: 0x0005ECF1
		public override void InitValidation(ValidationState context)
		{
			context.CurrentState.State = 0;
			context.HasMatched = this.transitionTable[0][this.symbols.Count] > 0;
		}

		// Token: 0x060015A2 RID: 5538 RVA: 0x0005FD1C File Offset: 0x0005ED1C
		public override object ValidateElement(XmlQualifiedName name, ValidationState context, out int errorCode)
		{
			int num = this.symbols[name];
			int num2 = this.transitionTable[context.CurrentState.State][num];
			errorCode = 0;
			if (num2 != -1)
			{
				context.CurrentState.State = num2;
				context.HasMatched = this.transitionTable[context.CurrentState.State][this.symbols.Count] > 0;
				return this.symbols.GetParticle(num);
			}
			if (base.IsOpen && context.HasMatched)
			{
				return null;
			}
			context.NeedValidateChildren = false;
			errorCode = -1;
			return null;
		}

		// Token: 0x060015A3 RID: 5539 RVA: 0x0005FDAF File Offset: 0x0005EDAF
		public override bool CompleteValidation(ValidationState context)
		{
			return context.HasMatched;
		}

		// Token: 0x060015A4 RID: 5540 RVA: 0x0005FDBC File Offset: 0x0005EDBC
		public override ArrayList ExpectedElements(ValidationState context, bool isRequiredOnly)
		{
			ArrayList arrayList = null;
			int[] array = this.transitionTable[context.CurrentState.State];
			if (array != null)
			{
				for (int i = 0; i < array.Length - 1; i++)
				{
					if (array[i] != -1)
					{
						if (arrayList == null)
						{
							arrayList = new ArrayList();
						}
						XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)this.symbols.GetParticle(i);
						if (xmlSchemaParticle == null)
						{
							string text = this.symbols.NameOf(i);
							if (text.Length != 0)
							{
								arrayList.Add(text);
							}
						}
						else
						{
							string nameString = xmlSchemaParticle.NameString;
							if (!arrayList.Contains(nameString))
							{
								arrayList.Add(nameString);
							}
						}
					}
				}
			}
			return arrayList;
		}

		// Token: 0x060015A5 RID: 5541 RVA: 0x0005FE54 File Offset: 0x0005EE54
		public override ArrayList ExpectedParticles(ValidationState context, bool isRequiredOnly)
		{
			ArrayList arrayList = new ArrayList();
			int[] array = this.transitionTable[context.CurrentState.State];
			if (array != null)
			{
				for (int i = 0; i < array.Length - 1; i++)
				{
					if (array[i] != -1)
					{
						XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)this.symbols.GetParticle(i);
						if (xmlSchemaParticle != null && !arrayList.Contains(xmlSchemaParticle))
						{
							arrayList.Add(xmlSchemaParticle);
						}
					}
				}
			}
			return arrayList;
		}

		// Token: 0x04000CDA RID: 3290
		private int[][] transitionTable;

		// Token: 0x04000CDB RID: 3291
		private SymbolsDictionary symbols;
	}
}
