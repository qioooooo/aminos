using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace System.Reflection.Emit
{
	// Token: 0x02000821 RID: 2081
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_CustomAttributeBuilder))]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public class CustomAttributeBuilder : _CustomAttributeBuilder
	{
		// Token: 0x06004AFA RID: 19194 RVA: 0x00104905 File Offset: 0x00103905
		public CustomAttributeBuilder(ConstructorInfo con, object[] constructorArgs)
		{
			this.InitCustomAttributeBuilder(con, constructorArgs, new PropertyInfo[0], new object[0], new FieldInfo[0], new object[0]);
		}

		// Token: 0x06004AFB RID: 19195 RVA: 0x0010492D File Offset: 0x0010392D
		public CustomAttributeBuilder(ConstructorInfo con, object[] constructorArgs, PropertyInfo[] namedProperties, object[] propertyValues)
		{
			this.InitCustomAttributeBuilder(con, constructorArgs, namedProperties, propertyValues, new FieldInfo[0], new object[0]);
		}

		// Token: 0x06004AFC RID: 19196 RVA: 0x0010494C File Offset: 0x0010394C
		public CustomAttributeBuilder(ConstructorInfo con, object[] constructorArgs, FieldInfo[] namedFields, object[] fieldValues)
		{
			this.InitCustomAttributeBuilder(con, constructorArgs, new PropertyInfo[0], new object[0], namedFields, fieldValues);
		}

		// Token: 0x06004AFD RID: 19197 RVA: 0x0010496B File Offset: 0x0010396B
		public CustomAttributeBuilder(ConstructorInfo con, object[] constructorArgs, PropertyInfo[] namedProperties, object[] propertyValues, FieldInfo[] namedFields, object[] fieldValues)
		{
			this.InitCustomAttributeBuilder(con, constructorArgs, namedProperties, propertyValues, namedFields, fieldValues);
		}

		// Token: 0x06004AFE RID: 19198 RVA: 0x00104984 File Offset: 0x00103984
		private bool ValidateType(Type t)
		{
			if (t.IsPrimitive || t == typeof(string) || t == typeof(Type))
			{
				return true;
			}
			if (t.IsEnum)
			{
				switch (Type.GetTypeCode(Enum.GetUnderlyingType(t)))
				{
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
				case TypeCode.UInt64:
					return true;
				default:
					return false;
				}
			}
			else
			{
				if (t.IsArray)
				{
					return t.GetArrayRank() == 1 && this.ValidateType(t.GetElementType());
				}
				return t == typeof(object);
			}
		}

		// Token: 0x06004AFF RID: 19199 RVA: 0x00104A24 File Offset: 0x00103A24
		internal void InitCustomAttributeBuilder(ConstructorInfo con, object[] constructorArgs, PropertyInfo[] namedProperties, object[] propertyValues, FieldInfo[] namedFields, object[] fieldValues)
		{
			if (con == null)
			{
				throw new ArgumentNullException("con");
			}
			if (constructorArgs == null)
			{
				throw new ArgumentNullException("constructorArgs");
			}
			if (namedProperties == null)
			{
				throw new ArgumentNullException("constructorArgs");
			}
			if (propertyValues == null)
			{
				throw new ArgumentNullException("propertyValues");
			}
			if (namedFields == null)
			{
				throw new ArgumentNullException("namedFields");
			}
			if (fieldValues == null)
			{
				throw new ArgumentNullException("fieldValues");
			}
			if (namedProperties.Length != propertyValues.Length)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_ArrayLengthsDiffer"), "namedProperties, propertyValues");
			}
			if (namedFields.Length != fieldValues.Length)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_ArrayLengthsDiffer"), "namedFields, fieldValues");
			}
			if ((con.Attributes & MethodAttributes.Static) == MethodAttributes.Static || (con.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Private)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadConstructor"));
			}
			if ((con.CallingConvention & CallingConventions.Standard) != CallingConventions.Standard)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadConstructorCallConv"));
			}
			this.m_con = con;
			this.m_constructorArgs = new object[constructorArgs.Length];
			Array.Copy(constructorArgs, this.m_constructorArgs, constructorArgs.Length);
			Type[] array;
			if (con is ConstructorBuilder)
			{
				array = ((ConstructorBuilder)con).GetParameterTypes();
			}
			else
			{
				ParameterInfo[] parametersNoCopy = con.GetParametersNoCopy();
				array = new Type[parametersNoCopy.Length];
				for (int i = 0; i < parametersNoCopy.Length; i++)
				{
					array[i] = parametersNoCopy[i].ParameterType;
				}
			}
			if (array.Length != constructorArgs.Length)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadParameterCountsForConstructor"));
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (!this.ValidateType(array[i]))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_BadTypeInCustomAttribute"));
				}
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (constructorArgs[i] != null)
				{
					TypeCode typeCode = Type.GetTypeCode(array[i]);
					if (typeCode != Type.GetTypeCode(constructorArgs[i].GetType()) && (typeCode != TypeCode.Object || !this.ValidateType(constructorArgs[i].GetType())))
					{
						throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_BadParameterTypeForConstructor"), new object[] { i }));
					}
				}
			}
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(1);
			for (int i = 0; i < constructorArgs.Length; i++)
			{
				this.EmitValue(binaryWriter, array[i], constructorArgs[i]);
			}
			binaryWriter.Write((ushort)(namedProperties.Length + namedFields.Length));
			for (int i = 0; i < namedProperties.Length; i++)
			{
				if (namedProperties[i] == null)
				{
					throw new ArgumentNullException("namedProperties[" + i + "]");
				}
				Type propertyType = namedProperties[i].PropertyType;
				if (propertyValues[i] == null && propertyType.IsPrimitive)
				{
					throw new ArgumentNullException("propertyValues[" + i + "]");
				}
				if (!this.ValidateType(propertyType))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_BadTypeInCustomAttribute"));
				}
				if (!namedProperties[i].CanWrite)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_NotAWritableProperty"));
				}
				if (namedProperties[i].DeclaringType != con.DeclaringType && !(con.DeclaringType is TypeBuilderInstantiation) && !con.DeclaringType.IsSubclassOf(namedProperties[i].DeclaringType) && !TypeBuilder.IsTypeEqual(namedProperties[i].DeclaringType, con.DeclaringType) && (!(namedProperties[i].DeclaringType is TypeBuilder) || !con.DeclaringType.IsSubclassOf(((TypeBuilder)namedProperties[i].DeclaringType).m_runtimeType)))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_BadPropertyForConstructorBuilder"));
				}
				if (propertyValues[i] != null && propertyType != typeof(object) && Type.GetTypeCode(propertyValues[i].GetType()) != Type.GetTypeCode(propertyType))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_ConstantDoesntMatch"));
				}
				binaryWriter.Write(84);
				this.EmitType(binaryWriter, propertyType);
				this.EmitString(binaryWriter, namedProperties[i].Name);
				this.EmitValue(binaryWriter, propertyType, propertyValues[i]);
			}
			for (int i = 0; i < namedFields.Length; i++)
			{
				if (namedFields[i] == null)
				{
					throw new ArgumentNullException("namedFields[" + i + "]");
				}
				Type fieldType = namedFields[i].FieldType;
				if (fieldValues[i] == null && fieldType.IsPrimitive)
				{
					throw new ArgumentNullException("fieldValues[" + i + "]");
				}
				if (!this.ValidateType(fieldType))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_BadTypeInCustomAttribute"));
				}
				if (namedFields[i].DeclaringType != con.DeclaringType && !(con.DeclaringType is TypeBuilderInstantiation) && !con.DeclaringType.IsSubclassOf(namedFields[i].DeclaringType) && !TypeBuilder.IsTypeEqual(namedFields[i].DeclaringType, con.DeclaringType) && (!(namedFields[i].DeclaringType is TypeBuilder) || !con.DeclaringType.IsSubclassOf(((TypeBuilder)namedFields[i].DeclaringType).m_runtimeType)))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_BadFieldForConstructorBuilder"));
				}
				if (fieldValues[i] != null && fieldType != typeof(object) && Type.GetTypeCode(fieldValues[i].GetType()) != Type.GetTypeCode(fieldType))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_ConstantDoesntMatch"));
				}
				binaryWriter.Write(83);
				this.EmitType(binaryWriter, fieldType);
				this.EmitString(binaryWriter, namedFields[i].Name);
				this.EmitValue(binaryWriter, fieldType, fieldValues[i]);
			}
			this.m_blob = ((MemoryStream)binaryWriter.BaseStream).ToArray();
		}

		// Token: 0x06004B00 RID: 19200 RVA: 0x00104F80 File Offset: 0x00103F80
		private void EmitType(BinaryWriter writer, Type type)
		{
			if (type.IsPrimitive)
			{
				switch (Type.GetTypeCode(type))
				{
				case TypeCode.Boolean:
					writer.Write(2);
					return;
				case TypeCode.Char:
					writer.Write(3);
					return;
				case TypeCode.SByte:
					writer.Write(4);
					return;
				case TypeCode.Byte:
					writer.Write(5);
					return;
				case TypeCode.Int16:
					writer.Write(6);
					return;
				case TypeCode.UInt16:
					writer.Write(7);
					return;
				case TypeCode.Int32:
					writer.Write(8);
					return;
				case TypeCode.UInt32:
					writer.Write(9);
					return;
				case TypeCode.Int64:
					writer.Write(10);
					return;
				case TypeCode.UInt64:
					writer.Write(11);
					return;
				case TypeCode.Single:
					writer.Write(12);
					return;
				case TypeCode.Double:
					writer.Write(13);
					return;
				default:
					return;
				}
			}
			else
			{
				if (type.IsEnum)
				{
					writer.Write(85);
					this.EmitString(writer, type.AssemblyQualifiedName);
					return;
				}
				if (type == typeof(string))
				{
					writer.Write(14);
					return;
				}
				if (type == typeof(Type))
				{
					writer.Write(80);
					return;
				}
				if (type.IsArray)
				{
					writer.Write(29);
					this.EmitType(writer, type.GetElementType());
					return;
				}
				writer.Write(81);
				return;
			}
		}

		// Token: 0x06004B01 RID: 19201 RVA: 0x001050B0 File Offset: 0x001040B0
		private void EmitString(BinaryWriter writer, string str)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(str);
			uint num = (uint)bytes.Length;
			if (num <= 127U)
			{
				writer.Write((byte)num);
			}
			else if (num <= 16383U)
			{
				writer.Write((byte)((num >> 8) | 128U));
				writer.Write((byte)(num & 255U));
			}
			else
			{
				writer.Write((byte)((num >> 24) | 192U));
				writer.Write((byte)((num >> 16) & 255U));
				writer.Write((byte)((num >> 8) & 255U));
				writer.Write((byte)(num & 255U));
			}
			writer.Write(bytes);
		}

		// Token: 0x06004B02 RID: 19202 RVA: 0x0010514C File Offset: 0x0010414C
		private void EmitValue(BinaryWriter writer, Type type, object value)
		{
			if (type.IsEnum)
			{
				switch (Type.GetTypeCode(Enum.GetUnderlyingType(type)))
				{
				case TypeCode.SByte:
					writer.Write((sbyte)value);
					return;
				case TypeCode.Byte:
					writer.Write((byte)value);
					return;
				case TypeCode.Int16:
					writer.Write((short)value);
					return;
				case TypeCode.UInt16:
					writer.Write((ushort)value);
					return;
				case TypeCode.Int32:
					writer.Write((int)value);
					return;
				case TypeCode.UInt32:
					writer.Write((uint)value);
					return;
				case TypeCode.Int64:
					writer.Write((long)value);
					return;
				case TypeCode.UInt64:
					writer.Write((ulong)value);
					return;
				default:
					return;
				}
			}
			else if (type == typeof(string))
			{
				if (value == null)
				{
					writer.Write(byte.MaxValue);
					return;
				}
				this.EmitString(writer, (string)value);
				return;
			}
			else if (type == typeof(Type))
			{
				if (value == null)
				{
					writer.Write(byte.MaxValue);
					return;
				}
				string text = TypeNameBuilder.ToString((Type)value, TypeNameBuilder.Format.AssemblyQualifiedName);
				if (text == null)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidTypeForCA"), new object[] { value.GetType() }));
				}
				this.EmitString(writer, text);
				return;
			}
			else if (type.IsArray)
			{
				if (value == null)
				{
					writer.Write(uint.MaxValue);
					return;
				}
				Array array = (Array)value;
				Type elementType = type.GetElementType();
				writer.Write(array.Length);
				for (int i = 0; i < array.Length; i++)
				{
					this.EmitValue(writer, elementType, array.GetValue(i));
				}
				return;
			}
			else if (type.IsPrimitive)
			{
				switch (Type.GetTypeCode(type))
				{
				case TypeCode.Boolean:
					writer.Write(((bool)value) ? 1 : 0);
					return;
				case TypeCode.Char:
					writer.Write(Convert.ToInt16((char)value));
					return;
				case TypeCode.SByte:
					writer.Write((sbyte)value);
					return;
				case TypeCode.Byte:
					writer.Write((byte)value);
					return;
				case TypeCode.Int16:
					writer.Write((short)value);
					return;
				case TypeCode.UInt16:
					writer.Write((ushort)value);
					return;
				case TypeCode.Int32:
					writer.Write((int)value);
					return;
				case TypeCode.UInt32:
					writer.Write((uint)value);
					return;
				case TypeCode.Int64:
					writer.Write((long)value);
					return;
				case TypeCode.UInt64:
					writer.Write((ulong)value);
					return;
				case TypeCode.Single:
					writer.Write((float)value);
					return;
				case TypeCode.Double:
					writer.Write((double)value);
					return;
				default:
					return;
				}
			}
			else
			{
				if (type == typeof(object))
				{
					Type type2 = ((value == null) ? typeof(string) : ((value is Type) ? typeof(Type) : value.GetType()));
					this.EmitType(writer, type2);
					this.EmitValue(writer, type2, value);
					return;
				}
				string text2 = "null";
				if (value != null)
				{
					text2 = value.GetType().ToString();
				}
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_BadParameterTypeForCAB"), new object[] { text2 }));
			}
		}

		// Token: 0x06004B03 RID: 19203 RVA: 0x00105460 File Offset: 0x00104460
		internal void CreateCustomAttribute(ModuleBuilder mod, int tkOwner)
		{
			this.CreateCustomAttribute(mod, tkOwner, mod.GetConstructorToken(this.m_con).Token, false);
		}

		// Token: 0x06004B04 RID: 19204 RVA: 0x0010548C File Offset: 0x0010448C
		internal int PrepareCreateCustomAttributeToDisk(ModuleBuilder mod)
		{
			return mod.InternalGetConstructorToken(this.m_con, true).Token;
		}

		// Token: 0x06004B05 RID: 19205 RVA: 0x001054AE File Offset: 0x001044AE
		internal void CreateCustomAttribute(ModuleBuilder mod, int tkOwner, int tkAttrib, bool toDisk)
		{
			TypeBuilder.InternalCreateCustomAttribute(tkOwner, tkAttrib, this.m_blob, mod, toDisk, typeof(DebuggableAttribute) == this.m_con.DeclaringType);
		}

		// Token: 0x06004B06 RID: 19206 RVA: 0x001054D7 File Offset: 0x001044D7
		void _CustomAttributeBuilder.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004B07 RID: 19207 RVA: 0x001054DE File Offset: 0x001044DE
		void _CustomAttributeBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004B08 RID: 19208 RVA: 0x001054E5 File Offset: 0x001044E5
		void _CustomAttributeBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004B09 RID: 19209 RVA: 0x001054EC File Offset: 0x001044EC
		void _CustomAttributeBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0400261C RID: 9756
		private const byte SERIALIZATION_TYPE_BOOLEAN = 2;

		// Token: 0x0400261D RID: 9757
		private const byte SERIALIZATION_TYPE_CHAR = 3;

		// Token: 0x0400261E RID: 9758
		private const byte SERIALIZATION_TYPE_I1 = 4;

		// Token: 0x0400261F RID: 9759
		private const byte SERIALIZATION_TYPE_U1 = 5;

		// Token: 0x04002620 RID: 9760
		private const byte SERIALIZATION_TYPE_I2 = 6;

		// Token: 0x04002621 RID: 9761
		private const byte SERIALIZATION_TYPE_U2 = 7;

		// Token: 0x04002622 RID: 9762
		private const byte SERIALIZATION_TYPE_I4 = 8;

		// Token: 0x04002623 RID: 9763
		private const byte SERIALIZATION_TYPE_U4 = 9;

		// Token: 0x04002624 RID: 9764
		private const byte SERIALIZATION_TYPE_I8 = 10;

		// Token: 0x04002625 RID: 9765
		private const byte SERIALIZATION_TYPE_U8 = 11;

		// Token: 0x04002626 RID: 9766
		private const byte SERIALIZATION_TYPE_R4 = 12;

		// Token: 0x04002627 RID: 9767
		private const byte SERIALIZATION_TYPE_R8 = 13;

		// Token: 0x04002628 RID: 9768
		private const byte SERIALIZATION_TYPE_STRING = 14;

		// Token: 0x04002629 RID: 9769
		private const byte SERIALIZATION_TYPE_SZARRAY = 29;

		// Token: 0x0400262A RID: 9770
		private const byte SERIALIZATION_TYPE_TYPE = 80;

		// Token: 0x0400262B RID: 9771
		private const byte SERIALIZATION_TYPE_TAGGED_OBJECT = 81;

		// Token: 0x0400262C RID: 9772
		private const byte SERIALIZATION_TYPE_FIELD = 83;

		// Token: 0x0400262D RID: 9773
		private const byte SERIALIZATION_TYPE_PROPERTY = 84;

		// Token: 0x0400262E RID: 9774
		private const byte SERIALIZATION_TYPE_ENUM = 85;

		// Token: 0x0400262F RID: 9775
		internal ConstructorInfo m_con;

		// Token: 0x04002630 RID: 9776
		internal object[] m_constructorArgs;

		// Token: 0x04002631 RID: 9777
		internal byte[] m_blob;
	}
}
