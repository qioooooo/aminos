using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Microsoft.JScript
{
	// Token: 0x02000041 RID: 65
	[ComVisible(true)]
	[Guid("CA0F511A-FAF2-4942-B9A8-17D5E46514E8")]
	public class COMFieldInfo : FieldInfo, MemberInfoInitializer
	{
		// Token: 0x0600029E RID: 670 RVA: 0x000156D1 File Offset: 0x000146D1
		public COMFieldInfo()
		{
			this._comObject = null;
			this._name = null;
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600029F RID: 671 RVA: 0x000156E7 File Offset: 0x000146E7
		public override FieldAttributes Attributes
		{
			get
			{
				return FieldAttributes.Public;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x000156EA File Offset: 0x000146EA
		public override Type DeclaringType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x000156ED File Offset: 0x000146ED
		public override RuntimeFieldHandle FieldHandle
		{
			get
			{
				throw new JScriptException(JSError.InternalError);
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x000156F6 File Offset: 0x000146F6
		public override Type FieldType
		{
			get
			{
				return typeof(object);
			}
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00015702 File Offset: 0x00014702
		public override object GetValue(object obj)
		{
			return this._comObject.GetValue(BindingFlags.Default, null, new object[0], null);
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00015718 File Offset: 0x00014718
		public virtual void Initialize(string name, COMMemberInfo dispatch)
		{
			this._name = name;
			this._comObject = dispatch;
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00015728 File Offset: 0x00014728
		public COMMemberInfo GetCOMMemberInfo()
		{
			return this._comObject;
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x00015730 File Offset: 0x00014730
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Field;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x00015733 File Offset: 0x00014733
		public override string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060002A8 RID: 680 RVA: 0x0001573B File Offset: 0x0001473B
		public override Type ReflectedType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0001573E File Offset: 0x0001473E
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
		{
			this._comObject.SetValue(value, invokeAttr, binder, new object[0], culture);
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00015757 File Offset: 0x00014757
		public override object[] GetCustomAttributes(Type t, bool inherit)
		{
			return new FieldInfo[0];
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0001575F File Offset: 0x0001475F
		public override object[] GetCustomAttributes(bool inherit)
		{
			return new FieldInfo[0];
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00015767 File Offset: 0x00014767
		public override bool IsDefined(Type t, bool inherit)
		{
			return false;
		}

		// Token: 0x040001B5 RID: 437
		private COMMemberInfo _comObject;

		// Token: 0x040001B6 RID: 438
		private string _name;
	}
}
