using System;
using System.CodeDom;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000193 RID: 403
	public class AxParameterData
	{
		// Token: 0x06000EF1 RID: 3825 RVA: 0x0003EA6C File Offset: 0x0003DA6C
		public AxParameterData(string inname, string typeName)
		{
			this.Name = inname;
			this.typeName = typeName;
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x0003EA82 File Offset: 0x0003DA82
		public AxParameterData(string inname, Type type)
		{
			this.Name = inname;
			this.type = type;
			this.typeName = AxWrapperGen.MapTypeName(type);
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x0003EAA4 File Offset: 0x0003DAA4
		public AxParameterData(ParameterInfo info)
			: this(info, false)
		{
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x0003EAB0 File Offset: 0x0003DAB0
		public AxParameterData(ParameterInfo info, bool ignoreByRefs)
		{
			this.paramInfo = info;
			this.Name = info.Name;
			this.type = info.ParameterType;
			this.typeName = AxWrapperGen.MapTypeName(info.ParameterType);
			this.isByRef = info.ParameterType.IsByRef && !ignoreByRefs;
			this.isIn = info.IsIn && !ignoreByRefs;
			this.isOut = info.IsOut && !this.isIn && !ignoreByRefs;
			this.isOptional = info.IsOptional;
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000EF5 RID: 3829 RVA: 0x0003EB4B File Offset: 0x0003DB4B
		public FieldDirection Direction
		{
			get
			{
				if (this.IsOut)
				{
					return FieldDirection.Out;
				}
				if (this.IsByRef)
				{
					return FieldDirection.Ref;
				}
				return FieldDirection.In;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000EF6 RID: 3830 RVA: 0x0003EB62 File Offset: 0x0003DB62
		public bool IsByRef
		{
			get
			{
				return this.isByRef;
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000EF7 RID: 3831 RVA: 0x0003EB6A File Offset: 0x0003DB6A
		public bool IsIn
		{
			get
			{
				return this.isIn;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000EF8 RID: 3832 RVA: 0x0003EB72 File Offset: 0x0003DB72
		public bool IsOut
		{
			get
			{
				return this.isOut;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000EF9 RID: 3833 RVA: 0x0003EB7A File Offset: 0x0003DB7A
		public bool IsOptional
		{
			get
			{
				return this.isOptional;
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000EFA RID: 3834 RVA: 0x0003EB82 File Offset: 0x0003DB82
		// (set) Token: 0x06000EFB RID: 3835 RVA: 0x0003EB8C File Offset: 0x0003DB8C
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (value == null)
				{
					this.name = null;
					return;
				}
				if (value != null && value.Length > 0 && char.IsUpper(value[0]))
				{
					char[] array = value.ToCharArray();
					if (array.Length > 0)
					{
						array[0] = char.ToLower(array[0], CultureInfo.InvariantCulture);
					}
					this.name = new string(array);
					return;
				}
				this.name = value;
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000EFC RID: 3836 RVA: 0x0003EBEF File Offset: 0x0003DBEF
		public Type ParameterType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x0003EBF8 File Offset: 0x0003DBF8
		internal static Type GetByRefBaseType(Type t)
		{
			if (t.IsByRef && t.FullName.EndsWith("&"))
			{
				Type type = t.Assembly.GetType(t.FullName.Substring(0, t.FullName.Length - 1), false);
				if (type != null)
				{
					t = type;
				}
			}
			return t;
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000EFE RID: 3838 RVA: 0x0003EC4C File Offset: 0x0003DC4C
		internal ParameterInfo ParameterInfo
		{
			get
			{
				return this.paramInfo;
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000EFF RID: 3839 RVA: 0x0003EC54 File Offset: 0x0003DC54
		internal Type ParameterBaseType
		{
			get
			{
				return AxParameterData.GetByRefBaseType(this.ParameterType);
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000F00 RID: 3840 RVA: 0x0003EC64 File Offset: 0x0003DC64
		public string TypeName
		{
			get
			{
				if (this.typeName == null)
				{
					this.typeName = this.ParameterBaseType.FullName;
				}
				else if (this.typeName.EndsWith("&"))
				{
					this.typeName = this.typeName.TrimEnd(new char[] { '&' });
				}
				return this.typeName;
			}
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x0003ECC2 File Offset: 0x0003DCC2
		public static AxParameterData[] Convert(ParameterInfo[] infos)
		{
			return AxParameterData.Convert(infos, false);
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x0003ECCC File Offset: 0x0003DCCC
		public static AxParameterData[] Convert(ParameterInfo[] infos, bool ignoreByRefs)
		{
			if (infos == null)
			{
				return new AxParameterData[0];
			}
			int num = 0;
			AxParameterData[] array = new AxParameterData[infos.Length];
			for (int i = 0; i < infos.Length; i++)
			{
				array[i] = new AxParameterData(infos[i], ignoreByRefs);
				if (array[i].Name == null || array[i].Name == "")
				{
					array[i].Name = "param" + num++;
				}
			}
			return array;
		}

		// Token: 0x04000FA4 RID: 4004
		private string name;

		// Token: 0x04000FA5 RID: 4005
		private string typeName;

		// Token: 0x04000FA6 RID: 4006
		private Type type;

		// Token: 0x04000FA7 RID: 4007
		private bool isByRef;

		// Token: 0x04000FA8 RID: 4008
		private bool isOut;

		// Token: 0x04000FA9 RID: 4009
		private bool isIn;

		// Token: 0x04000FAA RID: 4010
		private bool isOptional;

		// Token: 0x04000FAB RID: 4011
		private ParameterInfo paramInfo;
	}
}
