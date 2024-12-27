using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x020005AE RID: 1454
	public class PaddingConverter : TypeConverter
	{
		// Token: 0x06004B5F RID: 19295 RVA: 0x00110EC0 File Offset: 0x0010FEC0
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06004B60 RID: 19296 RVA: 0x00110ED9 File Offset: 0x0010FED9
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06004B61 RID: 19297 RVA: 0x00110EF4 File Offset: 0x0010FEF4
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			/*
An exception occurred when decompiling this method (06004B61)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Object System.Windows.Forms.PaddingConverter::ConvertFrom(System.ComponentModel.ITypeDescriptorContext,System.Globalization.CultureInfo,System.Object)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at System.Collections.Generic.Dictionary`2.Resize(Int32 newSize, Boolean forceNewHashCodes)
   at System.Collections.Generic.Dictionary`2.TryInsert(TKey key, TValue value, InsertionBehavior behavior)
   at ICSharpCode.Decompiler.ILAst.LoopsAndConditions.BuildGraph(List`1 nodes, ILLabel entryLabel) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\LoopsAndConditions.cs:line 94
   at ICSharpCode.Decompiler.ILAst.LoopsAndConditions.FindConditions(ILBlock block) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\LoopsAndConditions.cs:line 65
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 351
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x06004B62 RID: 19298 RVA: 0x00110FEC File Offset: 0x0010FFEC
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (value is Padding)
			{
				if (destinationType == typeof(string))
				{
					Padding padding = (Padding)value;
					if (culture == null)
					{
						culture = CultureInfo.CurrentCulture;
					}
					string text = culture.TextInfo.ListSeparator + " ";
					TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
					string[] array = new string[4];
					int num = 0;
					array[num++] = converter.ConvertToString(context, culture, padding.Left);
					array[num++] = converter.ConvertToString(context, culture, padding.Top);
					array[num++] = converter.ConvertToString(context, culture, padding.Right);
					array[num++] = converter.ConvertToString(context, culture, padding.Bottom);
					return string.Join(text, array);
				}
				if (destinationType == typeof(InstanceDescriptor))
				{
					Padding padding2 = (Padding)value;
					if (padding2.ShouldSerializeAll())
					{
						return new InstanceDescriptor(typeof(Padding).GetConstructor(new Type[] { typeof(int) }), new object[] { padding2.All });
					}
					return new InstanceDescriptor(typeof(Padding).GetConstructor(new Type[]
					{
						typeof(int),
						typeof(int),
						typeof(int),
						typeof(int)
					}), new object[] { padding2.Left, padding2.Top, padding2.Right, padding2.Bottom });
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x06004B63 RID: 19299 RVA: 0x001111F8 File Offset: 0x001101F8
		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (propertyValues == null)
			{
				throw new ArgumentNullException("propertyValues");
			}
			Padding padding = (Padding)context.PropertyDescriptor.GetValue(context.Instance);
			int num = (int)propertyValues["All"];
			if (padding.All != num)
			{
				return new Padding(num);
			}
			return new Padding((int)propertyValues["Left"], (int)propertyValues["Top"], (int)propertyValues["Right"], (int)propertyValues["Bottom"]);
		}

		// Token: 0x06004B64 RID: 19300 RVA: 0x001112A9 File Offset: 0x001102A9
		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x06004B65 RID: 19301 RVA: 0x001112AC File Offset: 0x001102AC
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(Padding), attributes);
			return properties.Sort(new string[] { "All", "Left", "Top", "Right", "Bottom" });
		}

		// Token: 0x06004B66 RID: 19302 RVA: 0x00111300 File Offset: 0x00110300
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
