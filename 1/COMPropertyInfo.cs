using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Microsoft.JScript
{
	// Token: 0x02000042 RID: 66
	[Guid("6A02951C-B129-4d26-AB92-B9CA19BDCA26")]
	[ComVisible(true)]
	public sealed class COMPropertyInfo : PropertyInfo, MemberInfoInitializer
	{
		// Token: 0x060002AD RID: 685 RVA: 0x0001576A File Offset: 0x0001476A
		public COMPropertyInfo()
		{
			this._comObject = null;
			this._name = null;
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060002AE RID: 686 RVA: 0x00015780 File Offset: 0x00014780
		public override PropertyAttributes Attributes
		{
			get
			{
				return PropertyAttributes.None;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060002AF RID: 687 RVA: 0x00015783 File Offset: 0x00014783
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x00015786 File Offset: 0x00014786
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x00015789 File Offset: 0x00014789
		public override Type DeclaringType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0001578C File Offset: 0x0001478C
		public override MethodInfo[] GetAccessors(bool nonPublic)
		{
			return new MethodInfo[]
			{
				this.GetGetMethod(nonPublic),
				this.GetSetMethod(nonPublic)
			};
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x000157B5 File Offset: 0x000147B5
		public override object[] GetCustomAttributes(Type t, bool inherit)
		{
			return new FieldInfo[0];
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x000157BD File Offset: 0x000147BD
		public override object[] GetCustomAttributes(bool inherit)
		{
			return new FieldInfo[0];
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x000157C8 File Offset: 0x000147C8
		public override MethodInfo GetGetMethod(bool nonPublic)
		{
			COMGetterMethod comgetterMethod = new COMGetterMethod();
			comgetterMethod.Initialize(this._name, this._comObject);
			return comgetterMethod;
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x000157EE File Offset: 0x000147EE
		public override ParameterInfo[] GetIndexParameters()
		{
			return new ParameterInfo[0];
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x000157F8 File Offset: 0x000147F8
		public override MethodInfo GetSetMethod(bool nonPublic)
		{
			COMSetterMethod comsetterMethod = new COMSetterMethod();
			comsetterMethod.Initialize(this._name, this._comObject);
			return comsetterMethod;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0001581E File Offset: 0x0001481E
		public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
		{
			return this._comObject.GetValue(invokeAttr, binder, (index != null) ? index : new object[0], culture);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0001583D File Offset: 0x0001483D
		public void Initialize(string name, COMMemberInfo dispatch)
		{
			this._name = name;
			this._comObject = dispatch;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0001584D File Offset: 0x0001484D
		public COMMemberInfo GetCOMMemberInfo()
		{
			return this._comObject;
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060002BB RID: 699 RVA: 0x00015855 File Offset: 0x00014855
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Property;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060002BC RID: 700 RVA: 0x00015859 File Offset: 0x00014859
		public override string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060002BD RID: 701 RVA: 0x00015861 File Offset: 0x00014861
		public override Type ReflectedType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00015864 File Offset: 0x00014864
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
		{
			this._comObject.SetValue(value, invokeAttr, binder, (index != null) ? index : new object[0], culture);
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060002BF RID: 703 RVA: 0x00015885 File Offset: 0x00014885
		public override Type PropertyType
		{
			get
			{
				return typeof(object);
			}
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00015891 File Offset: 0x00014891
		public override bool IsDefined(Type t, bool inherit)
		{
			return false;
		}

		// Token: 0x040001B7 RID: 439
		private COMMemberInfo _comObject;

		// Token: 0x040001B8 RID: 440
		private string _name;
	}
}
