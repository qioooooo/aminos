using System;
using System.Globalization;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x0200009F RID: 159
	internal class JSClosureProperty : JSWrappedProperty
	{
		// Token: 0x06000721 RID: 1825 RVA: 0x000313A7 File Offset: 0x000303A7
		internal JSClosureProperty(PropertyInfo property, MethodInfo getMeth, MethodInfo setMeth)
			: base(property, null)
		{
			this.getMeth = getMeth;
			this.setMeth = setMeth;
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x000313BF File Offset: 0x000303BF
		public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
		{
			if (this.getMeth == null)
			{
				throw new MissingMethodException();
			}
			return this.getMeth.Invoke(obj, invokeAttr, binder, index, culture);
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x000313E1 File Offset: 0x000303E1
		public override MethodInfo GetGetMethod(bool nonPublic)
		{
			if (nonPublic || (this.getMeth != null && this.getMeth.IsPublic))
			{
				return this.getMeth;
			}
			return null;
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x00031403 File Offset: 0x00030403
		public override MethodInfo GetSetMethod(bool nonPublic)
		{
			if (nonPublic || (this.setMeth != null && this.setMeth.IsPublic))
			{
				return this.setMeth;
			}
			return null;
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00031428 File Offset: 0x00030428
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
		{
			if (this.setMeth == null)
			{
				throw new MissingMethodException();
			}
			int num = ((index == null) ? 0 : index.Length);
			object[] array = new object[num + 1];
			array[0] = value;
			if (num > 0)
			{
				ArrayObject.Copy(index, 0, array, 1, num);
			}
			this.setMeth.Invoke(obj, invokeAttr, binder, array, culture);
		}

		// Token: 0x04000321 RID: 801
		private MethodInfo getMeth;

		// Token: 0x04000322 RID: 802
		private MethodInfo setMeth;
	}
}
