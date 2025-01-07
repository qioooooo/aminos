using System;
using System.CodeDom;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Forms.Design
{
	public class AxParameterData
	{
		public AxParameterData(string inname, string typeName)
		{
			this.Name = inname;
			this.typeName = typeName;
		}

		public AxParameterData(string inname, Type type)
		{
			this.Name = inname;
			this.type = type;
			this.typeName = AxWrapperGen.MapTypeName(type);
		}

		public AxParameterData(ParameterInfo info)
			: this(info, false)
		{
		}

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

		public bool IsByRef
		{
			get
			{
				return this.isByRef;
			}
		}

		public bool IsIn
		{
			get
			{
				return this.isIn;
			}
		}

		public bool IsOut
		{
			get
			{
				return this.isOut;
			}
		}

		public bool IsOptional
		{
			get
			{
				return this.isOptional;
			}
		}

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

		public Type ParameterType
		{
			get
			{
				return this.type;
			}
		}

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

		internal ParameterInfo ParameterInfo
		{
			get
			{
				return this.paramInfo;
			}
		}

		internal Type ParameterBaseType
		{
			get
			{
				return AxParameterData.GetByRefBaseType(this.ParameterType);
			}
		}

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

		public static AxParameterData[] Convert(ParameterInfo[] infos)
		{
			return AxParameterData.Convert(infos, false);
		}

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

		private string name;

		private string typeName;

		private Type type;

		private bool isByRef;

		private bool isOut;

		private bool isIn;

		private bool isOptional;

		private ParameterInfo paramInfo;
	}
}
