using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200008F RID: 143
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class MemberDescriptor
	{
		// Token: 0x06000519 RID: 1305 RVA: 0x00015D68 File Offset: 0x00014D68
		protected MemberDescriptor(string name)
			: this(name, null)
		{
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00015D74 File Offset: 0x00014D74
		protected MemberDescriptor(string name, Attribute[] attributes)
		{
			try
			{
				if (name == null || name.Length == 0)
				{
					throw new ArgumentException(SR.GetString("InvalidMemberName"));
				}
				this.name = name;
				this.displayName = name;
				this.nameHash = name.GetHashCode();
				if (attributes != null)
				{
					this.attributes = attributes;
					this.attributesFiltered = false;
				}
				this.originalAttributes = this.attributes;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x00015DF0 File Offset: 0x00014DF0
		protected MemberDescriptor(MemberDescriptor descr)
		{
			this.name = descr.Name;
			this.displayName = this.name;
			this.nameHash = this.name.GetHashCode();
			this.attributes = new Attribute[descr.Attributes.Count];
			descr.Attributes.CopyTo(this.attributes, 0);
			this.attributesFiltered = true;
			this.originalAttributes = this.attributes;
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00015E68 File Offset: 0x00014E68
		protected MemberDescriptor(MemberDescriptor oldMemberDescriptor, Attribute[] newAttributes)
		{
			this.name = oldMemberDescriptor.Name;
			this.displayName = oldMemberDescriptor.DisplayName;
			this.nameHash = this.name.GetHashCode();
			ArrayList arrayList = new ArrayList();
			if (oldMemberDescriptor.Attributes.Count != 0)
			{
				foreach (object obj in oldMemberDescriptor.Attributes)
				{
					arrayList.Add(obj);
				}
			}
			if (newAttributes != null)
			{
				foreach (Attribute obj2 in newAttributes)
				{
					arrayList.Add(obj2);
				}
			}
			this.attributes = new Attribute[arrayList.Count];
			arrayList.CopyTo(this.attributes, 0);
			this.attributesFiltered = false;
			this.originalAttributes = this.attributes;
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x00015F5C File Offset: 0x00014F5C
		// (set) Token: 0x0600051E RID: 1310 RVA: 0x00015F70 File Offset: 0x00014F70
		protected virtual Attribute[] AttributeArray
		{
			get
			{
				this.CheckAttributesValid();
				this.FilterAttributesIfNeeded();
				return this.attributes;
			}
			set
			{
				lock (this)
				{
					this.attributes = value;
					this.originalAttributes = value;
					this.attributesFiltered = false;
					this.attributeCollection = null;
				}
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600051F RID: 1311 RVA: 0x00015FBC File Offset: 0x00014FBC
		public virtual AttributeCollection Attributes
		{
			get
			{
				this.CheckAttributesValid();
				if (this.attributeCollection == null)
				{
					this.attributeCollection = this.CreateAttributeCollection();
				}
				return this.attributeCollection;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x00015FDE File Offset: 0x00014FDE
		public virtual string Category
		{
			get
			{
				if (this.category == null)
				{
					this.category = ((CategoryAttribute)this.Attributes[typeof(CategoryAttribute)]).Category;
				}
				return this.category;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000521 RID: 1313 RVA: 0x00016013 File Offset: 0x00015013
		public virtual string Description
		{
			get
			{
				if (this.description == null)
				{
					this.description = ((DescriptionAttribute)this.Attributes[typeof(DescriptionAttribute)]).Description;
				}
				return this.description;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000522 RID: 1314 RVA: 0x00016048 File Offset: 0x00015048
		public virtual bool IsBrowsable
		{
			get
			{
				return ((BrowsableAttribute)this.Attributes[typeof(BrowsableAttribute)]).Browsable;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x00016069 File Offset: 0x00015069
		public virtual string Name
		{
			get
			{
				if (this.name == null)
				{
					return "";
				}
				return this.name;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x0001607F File Offset: 0x0001507F
		protected virtual int NameHashCode
		{
			get
			{
				return this.nameHash;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000525 RID: 1317 RVA: 0x00016087 File Offset: 0x00015087
		public virtual bool DesignTimeOnly
		{
			get
			{
				return DesignOnlyAttribute.Yes.Equals(this.Attributes[typeof(DesignOnlyAttribute)]);
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x000160A8 File Offset: 0x000150A8
		public virtual string DisplayName
		{
			get
			{
				DisplayNameAttribute displayNameAttribute = this.Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute;
				if (displayNameAttribute == null || displayNameAttribute.IsDefaultAttribute())
				{
					return this.displayName;
				}
				return displayNameAttribute.DisplayName;
			}
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x000160E8 File Offset: 0x000150E8
		private void CheckAttributesValid()
		{
			if (this.attributesFiltered && this.metadataVersion != TypeDescriptor.MetadataVersion)
			{
				this.attributesFilled = false;
				this.attributesFiltered = false;
				this.attributeCollection = null;
			}
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00016114 File Offset: 0x00015114
		protected virtual AttributeCollection CreateAttributeCollection()
		{
			return new AttributeCollection(this.AttributeArray);
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x00016124 File Offset: 0x00015124
		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			if (obj == null)
			{
				return false;
			}
			if (obj.GetType() != base.GetType())
			{
				return false;
			}
			MemberDescriptor memberDescriptor = (MemberDescriptor)obj;
			this.FilterAttributesIfNeeded();
			memberDescriptor.FilterAttributesIfNeeded();
			if (memberDescriptor.nameHash != this.nameHash)
			{
				return false;
			}
			if (memberDescriptor.category == null != (this.category == null) || (this.category != null && !memberDescriptor.category.Equals(this.category)))
			{
				return false;
			}
			if (memberDescriptor.description == null != (this.description == null) || (this.description != null && !memberDescriptor.category.Equals(this.description)))
			{
				return false;
			}
			if (memberDescriptor.attributes == null != (this.attributes == null))
			{
				return false;
			}
			bool flag = true;
			if (this.attributes != null)
			{
				if (this.attributes.Length != memberDescriptor.attributes.Length)
				{
					return false;
				}
				for (int i = 0; i < this.attributes.Length; i++)
				{
					if (!this.attributes[i].Equals(memberDescriptor.attributes[i]))
					{
						flag = false;
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x00016234 File Offset: 0x00015234
		protected virtual void FillAttributes(IList attributeList)
		{
			if (this.originalAttributes != null)
			{
				foreach (Attribute attribute in this.originalAttributes)
				{
					attributeList.Add(attribute);
				}
			}
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0001626C File Offset: 0x0001526C
		private void FilterAttributesIfNeeded()
		{
			/*
An exception occurred when decompiling this method (0600052B)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.ComponentModel.MemberDescriptor::FilterAttributesIfNeeded()

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.ILExpression.GetBranchTargets() in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstTypes.cs:line 703
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.RemoveRedundantCode(DecompilerContext context, ILBlock method, List`1 listExpr, List`1 listBlock, Dictionary`2 labelRefCount) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 1452
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 361
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x00016374 File Offset: 0x00015374
		protected static MethodInfo FindMethod(Type componentClass, string name, Type[] args, Type returnType)
		{
			return MemberDescriptor.FindMethod(componentClass, name, args, returnType, true);
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x00016380 File Offset: 0x00015380
		protected static MethodInfo FindMethod(Type componentClass, string name, Type[] args, Type returnType, bool publicOnly)
		{
			MethodInfo methodInfo;
			if (publicOnly)
			{
				methodInfo = componentClass.GetMethod(name, args);
			}
			else
			{
				methodInfo = componentClass.GetMethod(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, args, null);
			}
			if (methodInfo != null && methodInfo.ReturnType != returnType)
			{
				methodInfo = null;
			}
			return methodInfo;
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x000163BA File Offset: 0x000153BA
		public override int GetHashCode()
		{
			return this.nameHash;
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x000163C2 File Offset: 0x000153C2
		protected virtual object GetInvocationTarget(Type type, object instance)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			return TypeDescriptor.GetAssociation(type, instance);
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x000163E7 File Offset: 0x000153E7
		protected static ISite GetSite(object component)
		{
			if (!(component is IComponent))
			{
				return null;
			}
			return ((IComponent)component).Site;
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x000163FE File Offset: 0x000153FE
		[Obsolete("This method has been deprecated. Use GetInvocationTarget instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		protected static object GetInvokee(Type componentClass, object component)
		{
			if (componentClass == null)
			{
				throw new ArgumentNullException("componentClass");
			}
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			return TypeDescriptor.GetAssociation(componentClass, component);
		}

		// Token: 0x040008B6 RID: 2230
		private string name;

		// Token: 0x040008B7 RID: 2231
		private string displayName;

		// Token: 0x040008B8 RID: 2232
		private int nameHash;

		// Token: 0x040008B9 RID: 2233
		private AttributeCollection attributeCollection;

		// Token: 0x040008BA RID: 2234
		private Attribute[] attributes;

		// Token: 0x040008BB RID: 2235
		private Attribute[] originalAttributes;

		// Token: 0x040008BC RID: 2236
		private bool attributesFiltered;

		// Token: 0x040008BD RID: 2237
		private bool attributesFilled;

		// Token: 0x040008BE RID: 2238
		private int metadataVersion;

		// Token: 0x040008BF RID: 2239
		private string category;

		// Token: 0x040008C0 RID: 2240
		private string description;
	}
}
