using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.CodeDom
{
	// Token: 0x02000084 RID: 132
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeTypeReference : CodeObject
	{
		// Token: 0x060004A1 RID: 1185 RVA: 0x00015088 File Offset: 0x00014088
		public CodeTypeReference()
		{
			this.baseType = string.Empty;
			this.arrayRank = 0;
			this.arrayElementType = null;
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x000150AC File Offset: 0x000140AC
		public CodeTypeReference(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (type.IsArray)
			{
				this.arrayRank = type.GetArrayRank();
				this.arrayElementType = new CodeTypeReference(type.GetElementType());
				this.baseType = null;
			}
			else
			{
				this.Initialize(type.FullName);
				this.arrayRank = 0;
				this.arrayElementType = null;
			}
			this.isInterface = type.IsInterface;
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00015121 File Offset: 0x00014121
		public CodeTypeReference(Type type, CodeTypeReferenceOptions codeTypeReferenceOption)
			: this(type)
		{
			this.referenceOptions = codeTypeReferenceOption;
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00015131 File Offset: 0x00014131
		public CodeTypeReference(string typeName, CodeTypeReferenceOptions codeTypeReferenceOption)
			: this(typeName)
		{
			this.referenceOptions = codeTypeReferenceOption;
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00015141 File Offset: 0x00014141
		public CodeTypeReference(string typeName)
		{
			this.Initialize(typeName);
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x00015150 File Offset: 0x00014150
		private void Initialize(string typeName)
		{
			if (typeName == null || typeName.Length == 0)
			{
				typeName = typeof(void).FullName;
				this.baseType = typeName;
				this.arrayRank = 0;
				this.arrayElementType = null;
				return;
			}
			typeName = this.RipOffAssemblyInformationFromTypeName(typeName);
			int num = typeName.Length - 1;
			int i = num;
			this.needsFixup = true;
			Queue queue = new Queue();
			while (i >= 0)
			{
				int num2 = 1;
				if (typeName[i--] != ']')
				{
					break;
				}
				while (i >= 0 && typeName[i] == ',')
				{
					num2++;
					i--;
				}
				if (i < 0 || typeName[i] != '[')
				{
					break;
				}
				queue.Enqueue(num2);
				i--;
				num = i;
			}
			i = num;
			ArrayList arrayList = new ArrayList();
			Stack stack = new Stack();
			if (i > 0 && typeName[i--] == ']')
			{
				this.needsFixup = false;
				int num3 = 1;
				int num4 = num;
				while (i >= 0)
				{
					if (typeName[i] == '[')
					{
						if (--num3 == 0)
						{
							break;
						}
					}
					else if (typeName[i] == ']')
					{
						num3++;
					}
					else if (typeName[i] == ',' && num3 == 1)
					{
						if (i + 1 < num4)
						{
							stack.Push(typeName.Substring(i + 1, num4 - i - 1));
						}
						num4 = i;
					}
					i--;
				}
				if (i > 0 && num - i - 1 > 0)
				{
					if (i + 1 < num4)
					{
						stack.Push(typeName.Substring(i + 1, num4 - i - 1));
					}
					while (stack.Count > 0)
					{
						string text = this.RipOffAssemblyInformationFromTypeName((string)stack.Pop());
						arrayList.Add(new CodeTypeReference(text));
					}
					num = i - 1;
				}
			}
			if (num < 0)
			{
				this.baseType = typeName;
				return;
			}
			if (queue.Count > 0)
			{
				CodeTypeReference codeTypeReference = new CodeTypeReference(typeName.Substring(0, num + 1));
				for (int j = 0; j < arrayList.Count; j++)
				{
					codeTypeReference.TypeArguments.Add((CodeTypeReference)arrayList[j]);
				}
				while (queue.Count > 1)
				{
					codeTypeReference = new CodeTypeReference(codeTypeReference, (int)queue.Dequeue());
				}
				this.baseType = null;
				this.arrayRank = (int)queue.Dequeue();
				this.arrayElementType = codeTypeReference;
			}
			else if (arrayList.Count > 0)
			{
				for (int k = 0; k < arrayList.Count; k++)
				{
					this.TypeArguments.Add((CodeTypeReference)arrayList[k]);
				}
				this.baseType = typeName.Substring(0, num + 1);
			}
			else
			{
				this.baseType = typeName;
			}
			if (this.baseType != null && this.baseType.IndexOf('`') != -1)
			{
				this.needsFixup = false;
			}
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00015403 File Offset: 0x00014403
		public CodeTypeReference(string typeName, params CodeTypeReference[] typeArguments)
			: this(typeName)
		{
			if (typeArguments != null && typeArguments.Length > 0)
			{
				this.TypeArguments.AddRange(typeArguments);
			}
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00015421 File Offset: 0x00014421
		public CodeTypeReference(CodeTypeParameter typeParameter)
			: this((typeParameter == null) ? null : typeParameter.Name)
		{
			this.referenceOptions = CodeTypeReferenceOptions.GenericTypeParameter;
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0001543C File Offset: 0x0001443C
		public CodeTypeReference(string baseType, int rank)
		{
			this.baseType = null;
			this.arrayRank = rank;
			this.arrayElementType = new CodeTypeReference(baseType);
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0001545E File Offset: 0x0001445E
		public CodeTypeReference(CodeTypeReference arrayType, int rank)
		{
			this.baseType = null;
			this.arrayRank = rank;
			this.arrayElementType = arrayType;
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060004AB RID: 1195 RVA: 0x0001547B File Offset: 0x0001447B
		// (set) Token: 0x060004AC RID: 1196 RVA: 0x00015483 File Offset: 0x00014483
		public CodeTypeReference ArrayElementType
		{
			get
			{
				return this.arrayElementType;
			}
			set
			{
				this.arrayElementType = value;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x0001548C File Offset: 0x0001448C
		// (set) Token: 0x060004AE RID: 1198 RVA: 0x00015494 File Offset: 0x00014494
		public int ArrayRank
		{
			get
			{
				return this.arrayRank;
			}
			set
			{
				this.arrayRank = value;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060004AF RID: 1199 RVA: 0x000154A0 File Offset: 0x000144A0
		// (set) Token: 0x060004B0 RID: 1200 RVA: 0x00015521 File Offset: 0x00014521
		public string BaseType
		{
			get
			{
				if (this.arrayRank > 0 && this.arrayElementType != null)
				{
					return this.arrayElementType.BaseType;
				}
				if (string.IsNullOrEmpty(this.baseType))
				{
					return string.Empty;
				}
				string text = this.baseType;
				if (this.needsFixup && this.TypeArguments.Count > 0)
				{
					text = text + '`' + this.TypeArguments.Count.ToString(CultureInfo.InvariantCulture);
				}
				return text;
			}
			set
			{
				this.baseType = value;
				this.Initialize(this.baseType);
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x00015536 File Offset: 0x00014536
		// (set) Token: 0x060004B2 RID: 1202 RVA: 0x0001553E File Offset: 0x0001453E
		[ComVisible(false)]
		public CodeTypeReferenceOptions Options
		{
			get
			{
				return this.referenceOptions;
			}
			set
			{
				this.referenceOptions = value;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x00015547 File Offset: 0x00014547
		[ComVisible(false)]
		public CodeTypeReferenceCollection TypeArguments
		{
			get
			{
				if (this.arrayRank > 0 && this.arrayElementType != null)
				{
					return this.arrayElementType.TypeArguments;
				}
				if (this.typeArguments == null)
				{
					this.typeArguments = new CodeTypeReferenceCollection();
				}
				return this.typeArguments;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x0001557F File Offset: 0x0001457F
		internal bool IsInterface
		{
			get
			{
				return this.isInterface;
			}
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00015588 File Offset: 0x00014588
		private string RipOffAssemblyInformationFromTypeName(string typeName)
		{
			/*
An exception occurred when decompiling this method (060004B5)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.String System.CodeDom.CodeTypeReference::RipOffAssemblyInformationFromTypeName(System.String)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.VariableSlot.CloneVariableState(VariableSlot[] state) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 78
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.StackAnalysis(MethodDef methodDef) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 456
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Build(MethodDef methodDef, Boolean optimize, DecompilerContext context) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 278
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 117
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x04000893 RID: 2195
		private string baseType;

		// Token: 0x04000894 RID: 2196
		[OptionalField]
		private bool isInterface;

		// Token: 0x04000895 RID: 2197
		private int arrayRank;

		// Token: 0x04000896 RID: 2198
		private CodeTypeReference arrayElementType;

		// Token: 0x04000897 RID: 2199
		[OptionalField]
		private CodeTypeReferenceCollection typeArguments;

		// Token: 0x04000898 RID: 2200
		[OptionalField]
		private CodeTypeReferenceOptions referenceOptions;

		// Token: 0x04000899 RID: 2201
		[OptionalField]
		private bool needsFixup;
	}
}
