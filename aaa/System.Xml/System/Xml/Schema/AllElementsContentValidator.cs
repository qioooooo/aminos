using System;
using System.Collections;

namespace System.Xml.Schema
{
	// Token: 0x020001A5 RID: 421
	internal sealed class AllElementsContentValidator : ContentValidator
	{
		// Token: 0x060015B2 RID: 5554 RVA: 0x0006081C File Offset: 0x0005F81C
		public AllElementsContentValidator(XmlSchemaContentType contentType, int size, bool isEmptiable)
			: base(contentType, false, isEmptiable)
		{
			this.elements = new Hashtable(size);
			this.particles = new object[size];
			this.isRequired = new BitSet(size);
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x0006084C File Offset: 0x0005F84C
		public bool AddElement(XmlQualifiedName name, object particle, bool isEmptiable)
		{
			if (this.elements[name] != null)
			{
				return false;
			}
			int count = this.elements.Count;
			this.elements.Add(name, count);
			this.particles[count] = particle;
			if (!isEmptiable)
			{
				this.isRequired.Set(count);
				this.countRequired++;
			}
			return true;
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x060015B4 RID: 5556 RVA: 0x000608AE File Offset: 0x0005F8AE
		public override bool IsEmptiable
		{
			get
			{
				return base.IsEmptiable || this.countRequired == 0;
			}
		}

		// Token: 0x060015B5 RID: 5557 RVA: 0x000608C3 File Offset: 0x0005F8C3
		public override void InitValidation(ValidationState context)
		{
			context.AllElementsSet = new BitSet(this.elements.Count);
			context.CurrentState.AllElementsRequired = -1;
		}

		// Token: 0x060015B6 RID: 5558 RVA: 0x000608E8 File Offset: 0x0005F8E8
		public override object ValidateElement(XmlQualifiedName name, ValidationState context, out int errorCode)
		{
			object obj = this.elements[name];
			errorCode = 0;
			if (obj == null)
			{
				context.NeedValidateChildren = false;
				return null;
			}
			int num = (int)obj;
			if (context.AllElementsSet[num])
			{
				errorCode = -2;
				return null;
			}
			if (context.CurrentState.AllElementsRequired == -1)
			{
				context.CurrentState.AllElementsRequired = 0;
			}
			context.AllElementsSet.Set(num);
			if (this.isRequired[num])
			{
				context.CurrentState.AllElementsRequired = context.CurrentState.AllElementsRequired + 1;
			}
			return this.particles[num];
		}

		// Token: 0x060015B7 RID: 5559 RVA: 0x0006097B File Offset: 0x0005F97B
		public override bool CompleteValidation(ValidationState context)
		{
			return context.CurrentState.AllElementsRequired == this.countRequired || (this.IsEmptiable && context.CurrentState.AllElementsRequired == -1);
		}

		// Token: 0x060015B8 RID: 5560 RVA: 0x000609AC File Offset: 0x0005F9AC
		public override ArrayList ExpectedElements(ValidationState context, bool isRequiredOnly)
		{
			ArrayList arrayList = null;
			foreach (object obj in this.elements)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				if (!context.AllElementsSet[(int)dictionaryEntry.Value] && (!isRequiredOnly || this.isRequired[(int)dictionaryEntry.Value]))
				{
					if (arrayList == null)
					{
						arrayList = new ArrayList();
					}
					arrayList.Add(dictionaryEntry.Key);
				}
			}
			return arrayList;
		}

		// Token: 0x060015B9 RID: 5561 RVA: 0x00060A50 File Offset: 0x0005FA50
		public override ArrayList ExpectedParticles(ValidationState context, bool isRequiredOnly)
		{
			ArrayList arrayList = new ArrayList();
			foreach (object obj in this.elements)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				if (!context.AllElementsSet[(int)dictionaryEntry.Value] && (!isRequiredOnly || this.isRequired[(int)dictionaryEntry.Value]))
				{
					arrayList.Add(this.particles[(int)dictionaryEntry.Value]);
				}
			}
			return arrayList;
		}

		// Token: 0x04000CEA RID: 3306
		private Hashtable elements;

		// Token: 0x04000CEB RID: 3307
		private object[] particles;

		// Token: 0x04000CEC RID: 3308
		private BitSet isRequired;

		// Token: 0x04000CED RID: 3309
		private int countRequired;
	}
}
