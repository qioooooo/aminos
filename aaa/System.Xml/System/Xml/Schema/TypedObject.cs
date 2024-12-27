using System;
using System.Globalization;

namespace System.Xml.Schema
{
	// Token: 0x0200018E RID: 398
	internal class TypedObject
	{
		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06001516 RID: 5398 RVA: 0x0005E25A File Offset: 0x0005D25A
		public int Dim
		{
			get
			{
				return this.dim;
			}
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06001517 RID: 5399 RVA: 0x0005E262 File Offset: 0x0005D262
		public bool IsList
		{
			get
			{
				return this.isList;
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06001518 RID: 5400 RVA: 0x0005E26A File Offset: 0x0005D26A
		public bool IsDecimal
		{
			get
			{
				return this.dstruct.IsDecimal;
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06001519 RID: 5401 RVA: 0x0005E277 File Offset: 0x0005D277
		public decimal[] Dvalue
		{
			get
			{
				return this.dstruct.Dvalue;
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x0600151A RID: 5402 RVA: 0x0005E284 File Offset: 0x0005D284
		// (set) Token: 0x0600151B RID: 5403 RVA: 0x0005E28C File Offset: 0x0005D28C
		public object Value
		{
			get
			{
				return this.ovalue;
			}
			set
			{
				this.ovalue = value;
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x0600151C RID: 5404 RVA: 0x0005E295 File Offset: 0x0005D295
		// (set) Token: 0x0600151D RID: 5405 RVA: 0x0005E29D File Offset: 0x0005D29D
		public XmlSchemaDatatype Type
		{
			get
			{
				return this.xsdtype;
			}
			set
			{
				this.xsdtype = value;
			}
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x0005E2A8 File Offset: 0x0005D2A8
		public TypedObject(object obj, string svalue, XmlSchemaDatatype xsdtype)
		{
			this.ovalue = obj;
			this.svalue = svalue;
			this.xsdtype = xsdtype;
			if (xsdtype.Variety == XmlSchemaDatatypeVariety.List || xsdtype is Datatype_base64Binary || xsdtype is Datatype_hexBinary)
			{
				this.isList = true;
				this.dim = ((Array)obj).Length;
			}
		}

		// Token: 0x0600151F RID: 5407 RVA: 0x0005E308 File Offset: 0x0005D308
		public override string ToString()
		{
			return this.svalue;
		}

		// Token: 0x06001520 RID: 5408 RVA: 0x0005E310 File Offset: 0x0005D310
		public void SetDecimal()
		{
			if (this.dstruct != null)
			{
				return;
			}
			XmlTypeCode typeCode = this.xsdtype.TypeCode;
			if (typeCode != XmlTypeCode.Decimal)
			{
				switch (typeCode)
				{
				case XmlTypeCode.Integer:
				case XmlTypeCode.NonPositiveInteger:
				case XmlTypeCode.NegativeInteger:
				case XmlTypeCode.Long:
				case XmlTypeCode.Int:
				case XmlTypeCode.Short:
				case XmlTypeCode.Byte:
				case XmlTypeCode.NonNegativeInteger:
				case XmlTypeCode.UnsignedLong:
				case XmlTypeCode.UnsignedInt:
				case XmlTypeCode.UnsignedShort:
				case XmlTypeCode.UnsignedByte:
				case XmlTypeCode.PositiveInteger:
					break;
				default:
					if (this.isList)
					{
						this.dstruct = new TypedObject.DecimalStruct(this.dim);
						return;
					}
					this.dstruct = new TypedObject.DecimalStruct();
					return;
				}
			}
			if (this.isList)
			{
				this.dstruct = new TypedObject.DecimalStruct(this.dim);
				for (int i = 0; i < this.dim; i++)
				{
					this.dstruct.Dvalue[i] = Convert.ToDecimal(((Array)this.ovalue).GetValue(i), NumberFormatInfo.InvariantInfo);
				}
			}
			else
			{
				this.dstruct = new TypedObject.DecimalStruct();
				this.dstruct.Dvalue[0] = Convert.ToDecimal(this.ovalue, NumberFormatInfo.InvariantInfo);
			}
			this.dstruct.IsDecimal = true;
		}

		// Token: 0x06001521 RID: 5409 RVA: 0x0005E43C File Offset: 0x0005D43C
		private bool ListDValueEquals(TypedObject other)
		{
			for (int i = 0; i < this.Dim; i++)
			{
				if (this.Dvalue[i] != other.Dvalue[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001522 RID: 5410 RVA: 0x0005E488 File Offset: 0x0005D488
		public bool Equals(TypedObject other)
		{
			if (this.Dim != other.Dim)
			{
				return false;
			}
			if (this.Type != other.Type)
			{
				if (!this.Type.IsComparable(other.Type))
				{
					return false;
				}
				other.SetDecimal();
				this.SetDecimal();
				if (this.IsDecimal && other.IsDecimal)
				{
					return this.ListDValueEquals(other);
				}
			}
			if (this.IsList)
			{
				if (other.IsList)
				{
					return this.Type.Compare(this.Value, other.Value) == 0;
				}
				Array array = this.Value as Array;
				XmlAtomicValue[] array2 = array as XmlAtomicValue[];
				if (array2 != null)
				{
					return array2.Length == 1 && array2.GetValue(0).Equals(other.Value);
				}
				return array.Length == 1 && array.GetValue(0).Equals(other.Value);
			}
			else
			{
				if (!other.IsList)
				{
					return this.Value.Equals(other.Value);
				}
				Array array3 = other.Value as Array;
				XmlAtomicValue[] array4 = array3 as XmlAtomicValue[];
				if (array4 != null)
				{
					return array4.Length == 1 && array4.GetValue(0).Equals(this.Value);
				}
				return array3.Length == 1 && array3.GetValue(0).Equals(this.Value);
			}
		}

		// Token: 0x04000CAC RID: 3244
		private TypedObject.DecimalStruct dstruct;

		// Token: 0x04000CAD RID: 3245
		private object ovalue;

		// Token: 0x04000CAE RID: 3246
		private string svalue;

		// Token: 0x04000CAF RID: 3247
		private XmlSchemaDatatype xsdtype;

		// Token: 0x04000CB0 RID: 3248
		private int dim = 1;

		// Token: 0x04000CB1 RID: 3249
		private bool isList;

		// Token: 0x0200018F RID: 399
		private class DecimalStruct
		{
			// Token: 0x1700050F RID: 1295
			// (get) Token: 0x06001523 RID: 5411 RVA: 0x0005E5CE File Offset: 0x0005D5CE
			// (set) Token: 0x06001524 RID: 5412 RVA: 0x0005E5D6 File Offset: 0x0005D5D6
			public bool IsDecimal
			{
				get
				{
					return this.isDecimal;
				}
				set
				{
					this.isDecimal = value;
				}
			}

			// Token: 0x17000510 RID: 1296
			// (get) Token: 0x06001525 RID: 5413 RVA: 0x0005E5DF File Offset: 0x0005D5DF
			public decimal[] Dvalue
			{
				get
				{
					return this.dvalue;
				}
			}

			// Token: 0x06001526 RID: 5414 RVA: 0x0005E5E7 File Offset: 0x0005D5E7
			public DecimalStruct()
			{
				this.dvalue = new decimal[1];
			}

			// Token: 0x06001527 RID: 5415 RVA: 0x0005E5FB File Offset: 0x0005D5FB
			public DecimalStruct(int dim)
			{
				this.dvalue = new decimal[dim];
			}

			// Token: 0x04000CB2 RID: 3250
			private bool isDecimal;

			// Token: 0x04000CB3 RID: 3251
			private decimal[] dvalue;
		}
	}
}
