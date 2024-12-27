using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Security.Policy;
using System.Security.Util;
using System.Text;
using System.Threading;

namespace System.Security
{
	// Token: 0x0200065E RID: 1630
	[ComVisible(true)]
	[StrongNameIdentityPermission(SecurityAction.InheritanceDemand, Name = "mscorlib", PublicKey = "0x00000000000000000400000000000000")]
	[Serializable]
	public class PermissionSet : ISecurityEncodable, ICollection, IEnumerable, IStackWalk, IDeserializationCallback
	{
		// Token: 0x06003B1D RID: 15133 RVA: 0x000C9155 File Offset: 0x000C8155
		[Conditional("_DEBUG")]
		private static void DEBUG_WRITE(string str)
		{
		}

		// Token: 0x06003B1E RID: 15134 RVA: 0x000C9157 File Offset: 0x000C8157
		[Conditional("_DEBUG")]
		private static void DEBUG_COND_WRITE(bool exp, string str)
		{
		}

		// Token: 0x06003B1F RID: 15135 RVA: 0x000C9159 File Offset: 0x000C8159
		[Conditional("_DEBUG")]
		private static void DEBUG_PRINTSTACK(Exception e)
		{
		}

		// Token: 0x06003B20 RID: 15136 RVA: 0x000C915B File Offset: 0x000C815B
		[OnDeserializing]
		private void OnDeserializing(StreamingContext ctx)
		{
			this.Reset();
		}

		// Token: 0x06003B21 RID: 15137 RVA: 0x000C9164 File Offset: 0x000C8164
		[OnDeserialized]
		private void OnDeserialized(StreamingContext ctx)
		{
			if (this.m_serializedPermissionSet != null)
			{
				this.FromXml(SecurityElement.FromString(this.m_serializedPermissionSet));
			}
			else if (this.m_normalPermSet != null)
			{
				this.m_permSet = this.m_normalPermSet.SpecialUnion(this.m_unrestrictedPermSet, ref this.m_canUnrestrictedOverride);
			}
			else if (this.m_unrestrictedPermSet != null)
			{
				this.m_permSet = this.m_unrestrictedPermSet.SpecialUnion(this.m_normalPermSet, ref this.m_canUnrestrictedOverride);
			}
			this.m_serializedPermissionSet = null;
			this.m_normalPermSet = null;
			this.m_unrestrictedPermSet = null;
		}

		// Token: 0x06003B22 RID: 15138 RVA: 0x000C91F0 File Offset: 0x000C81F0
		[OnSerializing]
		private void OnSerializing(StreamingContext ctx)
		{
			if ((ctx.State & ~(StreamingContextStates.Clone | StreamingContextStates.CrossAppDomain)) != (StreamingContextStates)0)
			{
				this.m_serializedPermissionSet = this.ToString();
				if (this.m_permSet != null)
				{
					this.m_permSet.SpecialSplit(ref this.m_unrestrictedPermSet, ref this.m_normalPermSet, this.m_ignoreTypeLoadFailures);
				}
				this.m_permSetSaved = this.m_permSet;
				this.m_permSet = null;
			}
		}

		// Token: 0x06003B23 RID: 15139 RVA: 0x000C9250 File Offset: 0x000C8250
		[OnSerialized]
		private void OnSerialized(StreamingContext context)
		{
			if ((context.State & ~(StreamingContextStates.Clone | StreamingContextStates.CrossAppDomain)) != (StreamingContextStates)0)
			{
				this.m_serializedPermissionSet = null;
				this.m_permSet = this.m_permSetSaved;
				this.m_permSetSaved = null;
				this.m_unrestrictedPermSet = null;
				this.m_normalPermSet = null;
			}
		}

		// Token: 0x06003B24 RID: 15140 RVA: 0x000C9289 File Offset: 0x000C8289
		internal PermissionSet()
		{
			this.Reset();
			this.m_Unrestricted = true;
		}

		// Token: 0x06003B25 RID: 15141 RVA: 0x000C929E File Offset: 0x000C829E
		internal PermissionSet(bool fUnrestricted)
			: this()
		{
			this.SetUnrestricted(fUnrestricted);
		}

		// Token: 0x06003B26 RID: 15142 RVA: 0x000C92AD File Offset: 0x000C82AD
		public PermissionSet(PermissionState state)
			: this()
		{
			if (state == PermissionState.Unrestricted)
			{
				this.SetUnrestricted(true);
				return;
			}
			if (state == PermissionState.None)
			{
				this.SetUnrestricted(false);
				return;
			}
			throw new ArgumentException(Environment.GetResourceString("Argument_InvalidPermissionState"));
		}

		// Token: 0x06003B27 RID: 15143 RVA: 0x000C92DC File Offset: 0x000C82DC
		public PermissionSet(PermissionSet permSet)
			: this()
		{
			if (permSet == null)
			{
				this.Reset();
				return;
			}
			this.m_Unrestricted = permSet.m_Unrestricted;
			this.m_CheckedForNonCas = permSet.m_CheckedForNonCas;
			this.m_ContainsCas = permSet.m_ContainsCas;
			this.m_ContainsNonCas = permSet.m_ContainsNonCas;
			this.m_canUnrestrictedOverride = permSet.m_canUnrestrictedOverride;
			this.m_ignoreTypeLoadFailures = permSet.m_ignoreTypeLoadFailures;
			if (permSet.m_permSet != null)
			{
				this.m_permSet = new TokenBasedSet(permSet.m_permSet);
				for (int i = this.m_permSet.GetStartingIndex(); i <= this.m_permSet.GetMaxUsedIndex(); i++)
				{
					object item = this.m_permSet.GetItem(i);
					IPermission permission = item as IPermission;
					ISecurityElementFactory securityElementFactory = item as ISecurityElementFactory;
					if (permission != null)
					{
						this.m_permSet.SetItem(i, permission.Copy());
					}
					else if (securityElementFactory != null)
					{
						this.m_permSet.SetItem(i, securityElementFactory.Copy());
					}
				}
			}
		}

		// Token: 0x06003B28 RID: 15144 RVA: 0x000C93C4 File Offset: 0x000C83C4
		public virtual void CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			PermissionSetEnumeratorInternal permissionSetEnumeratorInternal = new PermissionSetEnumeratorInternal(this);
			while (permissionSetEnumeratorInternal.MoveNext())
			{
				object obj = permissionSetEnumeratorInternal.Current;
				array.SetValue(obj, index++);
			}
		}

		// Token: 0x06003B29 RID: 15145 RVA: 0x000C9405 File Offset: 0x000C8405
		private PermissionSet(object trash, object junk)
		{
			this.m_Unrestricted = false;
		}

		// Token: 0x170009F6 RID: 2550
		// (get) Token: 0x06003B2A RID: 15146 RVA: 0x000C9414 File Offset: 0x000C8414
		public virtual object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x170009F7 RID: 2551
		// (get) Token: 0x06003B2B RID: 15147 RVA: 0x000C9417 File Offset: 0x000C8417
		public virtual bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170009F8 RID: 2552
		// (get) Token: 0x06003B2C RID: 15148 RVA: 0x000C941A File Offset: 0x000C841A
		public virtual bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003B2D RID: 15149 RVA: 0x000C9420 File Offset: 0x000C8420
		internal void Reset()
		{
			this.m_Unrestricted = false;
			this.m_allPermissionsDecoded = true;
			this.m_canUnrestrictedOverride = true;
			this.m_permSet = null;
			this.m_ignoreTypeLoadFailures = false;
			this.m_CheckedForNonCas = false;
			this.m_ContainsCas = false;
			this.m_ContainsNonCas = false;
			this.m_permSetSaved = null;
		}

		// Token: 0x06003B2E RID: 15150 RVA: 0x000C946C File Offset: 0x000C846C
		internal void CheckSet()
		{
			if (this.m_permSet == null)
			{
				this.m_permSet = new TokenBasedSet();
			}
		}

		// Token: 0x06003B2F RID: 15151 RVA: 0x000C9484 File Offset: 0x000C8484
		public bool IsEmpty()
		{
			if (this.m_Unrestricted)
			{
				return false;
			}
			if (this.m_permSet == null || this.m_permSet.FastIsEmpty())
			{
				return true;
			}
			PermissionSetEnumeratorInternal permissionSetEnumeratorInternal = new PermissionSetEnumeratorInternal(this);
			while (permissionSetEnumeratorInternal.MoveNext())
			{
				object obj = permissionSetEnumeratorInternal.Current;
				IPermission permission = (IPermission)obj;
				if (!permission.IsSubsetOf(null))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003B30 RID: 15152 RVA: 0x000C94DE File Offset: 0x000C84DE
		internal bool FastIsEmpty()
		{
			return !this.m_Unrestricted && (this.m_permSet == null || this.m_permSet.FastIsEmpty());
		}

		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x06003B31 RID: 15153 RVA: 0x000C9504 File Offset: 0x000C8504
		public virtual int Count
		{
			get
			{
				int num = 0;
				if (this.m_permSet != null)
				{
					num += this.m_permSet.GetCount();
				}
				return num;
			}
		}

		// Token: 0x06003B32 RID: 15154 RVA: 0x000C952C File Offset: 0x000C852C
		internal IPermission GetPermission(int index)
		{
			if (this.m_permSet == null)
			{
				return null;
			}
			object item = this.m_permSet.GetItem(index);
			if (item == null)
			{
				return null;
			}
			IPermission permission = item as IPermission;
			if (permission != null)
			{
				return permission;
			}
			permission = this.CreatePermission(item, index);
			if (permission == null)
			{
				return null;
			}
			return permission;
		}

		// Token: 0x06003B33 RID: 15155 RVA: 0x000C9570 File Offset: 0x000C8570
		internal IPermission GetPermission(PermissionToken permToken)
		{
			if (permToken == null)
			{
				return null;
			}
			return this.GetPermission(permToken.m_index);
		}

		// Token: 0x06003B34 RID: 15156 RVA: 0x000C9583 File Offset: 0x000C8583
		internal IPermission GetPermission(IPermission perm)
		{
			if (perm == null)
			{
				return null;
			}
			return this.GetPermission(PermissionToken.GetToken(perm));
		}

		// Token: 0x06003B35 RID: 15157 RVA: 0x000C9596 File Offset: 0x000C8596
		public IPermission GetPermission(Type permClass)
		{
			if (permClass == null)
			{
				return null;
			}
			return this.GetPermission(PermissionToken.FindToken(permClass));
		}

		// Token: 0x06003B36 RID: 15158 RVA: 0x000C95A9 File Offset: 0x000C85A9
		public IPermission SetPermission(IPermission perm)
		{
			return this.SetPermission(perm, true);
		}

		// Token: 0x06003B37 RID: 15159 RVA: 0x000C95B4 File Offset: 0x000C85B4
		internal IPermission SetPermission(IPermission perm, bool fReplace)
		{
			if (perm == null)
			{
				return null;
			}
			if (!CodeAccessPermission.CanUnrestrictedOverride(perm))
			{
				this.m_canUnrestrictedOverride = false;
			}
			PermissionToken token = PermissionToken.GetToken(perm);
			if ((token.m_type & PermissionTokenType.IUnrestricted) != (PermissionTokenType)0)
			{
				this.m_Unrestricted = false;
			}
			this.CheckSet();
			IPermission permission = this.GetPermission(token.m_index);
			if (permission != null && !fReplace)
			{
				return permission;
			}
			this.m_CheckedForNonCas = false;
			this.m_permSet.SetItem(token.m_index, perm);
			return perm;
		}

		// Token: 0x06003B38 RID: 15160 RVA: 0x000C9624 File Offset: 0x000C8624
		public IPermission AddPermission(IPermission perm)
		{
			if (perm == null)
			{
				return null;
			}
			if (!CodeAccessPermission.CanUnrestrictedOverride(perm))
			{
				this.m_canUnrestrictedOverride = false;
			}
			this.m_CheckedForNonCas = false;
			PermissionToken token = PermissionToken.GetToken(perm);
			if (this.IsUnrestricted() && (token.m_type & PermissionTokenType.IUnrestricted) != (PermissionTokenType)0)
			{
				Type type = perm.GetType();
				return (IPermission)Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, new object[] { PermissionState.Unrestricted }, null);
			}
			this.CheckSet();
			IPermission permission = this.GetPermission(token.m_index);
			if (permission != null)
			{
				IPermission permission2 = permission.Union(perm);
				this.m_permSet.SetItem(token.m_index, permission2);
				return permission2;
			}
			this.m_permSet.SetItem(token.m_index, perm);
			return perm;
		}

		// Token: 0x06003B39 RID: 15161 RVA: 0x000C96D8 File Offset: 0x000C86D8
		internal IPermission RemovePermission(int index)
		{
			if (this.GetPermission(index) == null)
			{
				return null;
			}
			return (IPermission)this.m_permSet.RemoveItem(index);
		}

		// Token: 0x06003B3A RID: 15162 RVA: 0x000C9703 File Offset: 0x000C8703
		internal IPermission RemovePermission(PermissionToken permToken)
		{
			if (permToken == null)
			{
				return null;
			}
			return this.RemovePermission(permToken.m_index);
		}

		// Token: 0x06003B3B RID: 15163 RVA: 0x000C9716 File Offset: 0x000C8716
		public IPermission RemovePermission(Type permClass)
		{
			if (permClass == null)
			{
				return null;
			}
			return this.RemovePermission(PermissionToken.FindToken(permClass));
		}

		// Token: 0x06003B3C RID: 15164 RVA: 0x000C9729 File Offset: 0x000C8729
		internal void SetUnrestricted(bool unrestricted)
		{
			this.m_Unrestricted = unrestricted;
		}

		// Token: 0x06003B3D RID: 15165 RVA: 0x000C9732 File Offset: 0x000C8732
		public bool IsUnrestricted()
		{
			return this.m_Unrestricted;
		}

		// Token: 0x06003B3E RID: 15166 RVA: 0x000C973A File Offset: 0x000C873A
		internal bool CanUnrestrictedOverride()
		{
			return this.m_canUnrestrictedOverride;
		}

		// Token: 0x06003B3F RID: 15167 RVA: 0x000C9744 File Offset: 0x000C8744
		internal bool IsSubsetOfHelper(PermissionSet target, PermissionSet.IsSubsetOfType type, out IPermission firstPermThatFailed, bool ignoreNonCas)
		{
			firstPermThatFailed = null;
			if (target == null || target.FastIsEmpty())
			{
				if (this.IsEmpty())
				{
					return true;
				}
				firstPermThatFailed = this.GetFirstPerm();
				return false;
			}
			else
			{
				if (this.IsUnrestricted() && !target.IsUnrestricted())
				{
					return false;
				}
				if (this.m_permSet == null)
				{
					return true;
				}
				target.CheckSet();
				for (int i = this.m_permSet.GetStartingIndex(); i <= this.m_permSet.GetMaxUsedIndex(); i++)
				{
					IPermission permission = this.GetPermission(i);
					if (permission != null && !permission.IsSubsetOf(null))
					{
						IPermission permission2 = target.GetPermission(i);
						if (!target.m_Unrestricted || !CodeAccessPermission.CanUnrestrictedOverride(permission))
						{
							CodeAccessPermission codeAccessPermission = permission as CodeAccessPermission;
							if (codeAccessPermission == null)
							{
								if (!ignoreNonCas && !permission.IsSubsetOf(permission2))
								{
									firstPermThatFailed = permission;
									return false;
								}
							}
							else
							{
								firstPermThatFailed = permission;
								switch (type)
								{
								case PermissionSet.IsSubsetOfType.Normal:
									if (!permission.IsSubsetOf(permission2))
									{
										return false;
									}
									break;
								case PermissionSet.IsSubsetOfType.CheckDemand:
									if (!codeAccessPermission.CheckDemand((CodeAccessPermission)permission2))
									{
										return false;
									}
									break;
								case PermissionSet.IsSubsetOfType.CheckPermitOnly:
									if (!codeAccessPermission.CheckPermitOnly((CodeAccessPermission)permission2))
									{
										return false;
									}
									break;
								case PermissionSet.IsSubsetOfType.CheckAssertion:
									if (!codeAccessPermission.CheckAssert((CodeAccessPermission)permission2))
									{
										return false;
									}
									break;
								}
								firstPermThatFailed = null;
							}
						}
					}
				}
				return true;
			}
		}

		// Token: 0x06003B40 RID: 15168 RVA: 0x000C9868 File Offset: 0x000C8868
		public bool IsSubsetOf(PermissionSet target)
		{
			IPermission permission;
			return this.IsSubsetOfHelper(target, PermissionSet.IsSubsetOfType.Normal, out permission, false);
		}

		// Token: 0x06003B41 RID: 15169 RVA: 0x000C9880 File Offset: 0x000C8880
		internal bool CheckDemand(PermissionSet target, out IPermission firstPermThatFailed)
		{
			return this.IsSubsetOfHelper(target, PermissionSet.IsSubsetOfType.CheckDemand, out firstPermThatFailed, true);
		}

		// Token: 0x06003B42 RID: 15170 RVA: 0x000C988C File Offset: 0x000C888C
		internal bool CheckPermitOnly(PermissionSet target, out IPermission firstPermThatFailed)
		{
			return this.IsSubsetOfHelper(target, PermissionSet.IsSubsetOfType.CheckPermitOnly, out firstPermThatFailed, true);
		}

		// Token: 0x06003B43 RID: 15171 RVA: 0x000C9898 File Offset: 0x000C8898
		internal bool CheckAssertion(PermissionSet target)
		{
			IPermission permission;
			return this.IsSubsetOfHelper(target, PermissionSet.IsSubsetOfType.CheckAssertion, out permission, true);
		}

		// Token: 0x06003B44 RID: 15172 RVA: 0x000C98B0 File Offset: 0x000C88B0
		internal bool CheckDeny(PermissionSet deniedSet, out IPermission firstPermThatFailed)
		{
			firstPermThatFailed = null;
			if (deniedSet == null || deniedSet.FastIsEmpty() || this.FastIsEmpty())
			{
				return true;
			}
			if (this.m_Unrestricted && deniedSet.m_Unrestricted)
			{
				return false;
			}
			PermissionSetEnumeratorInternal permissionSetEnumeratorInternal = new PermissionSetEnumeratorInternal(this);
			while (permissionSetEnumeratorInternal.MoveNext())
			{
				object obj = permissionSetEnumeratorInternal.Current;
				CodeAccessPermission codeAccessPermission = obj as CodeAccessPermission;
				if (codeAccessPermission != null && !codeAccessPermission.IsSubsetOf(null))
				{
					if (deniedSet.m_Unrestricted && codeAccessPermission.CanUnrestrictedOverride())
					{
						firstPermThatFailed = codeAccessPermission;
						return false;
					}
					CodeAccessPermission codeAccessPermission2 = (CodeAccessPermission)deniedSet.GetPermission(permissionSetEnumeratorInternal.GetCurrentIndex());
					if (!codeAccessPermission.CheckDeny(codeAccessPermission2))
					{
						firstPermThatFailed = codeAccessPermission;
						return false;
					}
				}
			}
			if (this.m_Unrestricted)
			{
				PermissionSetEnumeratorInternal permissionSetEnumeratorInternal2 = new PermissionSetEnumeratorInternal(deniedSet);
				while (permissionSetEnumeratorInternal2.MoveNext())
				{
					if (permissionSetEnumeratorInternal2.Current is IPermission && CodeAccessPermission.CanUnrestrictedOverride((IPermission)permissionSetEnumeratorInternal2.Current))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06003B45 RID: 15173 RVA: 0x000C9988 File Offset: 0x000C8988
		internal void CheckDecoded(CodeAccessPermission demandedPerm, PermissionToken tokenDemandedPerm)
		{
			if (this.m_allPermissionsDecoded || this.m_permSet == null)
			{
				return;
			}
			if (tokenDemandedPerm == null)
			{
				tokenDemandedPerm = PermissionToken.GetToken(demandedPerm);
			}
			this.CheckDecoded(tokenDemandedPerm.m_index);
		}

		// Token: 0x06003B46 RID: 15174 RVA: 0x000C99B2 File Offset: 0x000C89B2
		internal void CheckDecoded(int index)
		{
			if (this.m_allPermissionsDecoded || this.m_permSet == null)
			{
				return;
			}
			this.GetPermission(index);
		}

		// Token: 0x06003B47 RID: 15175 RVA: 0x000C99D0 File Offset: 0x000C89D0
		internal void CheckDecoded(PermissionSet demandedSet)
		{
			if (this.m_allPermissionsDecoded || this.m_permSet == null)
			{
				return;
			}
			PermissionSetEnumeratorInternal enumeratorInternal = demandedSet.GetEnumeratorInternal();
			while (enumeratorInternal.MoveNext())
			{
				this.CheckDecoded(enumeratorInternal.GetCurrentIndex());
			}
		}

		// Token: 0x06003B48 RID: 15176 RVA: 0x000C9A10 File Offset: 0x000C8A10
		internal static void SafeChildAdd(SecurityElement parent, ISecurityElementFactory child, bool copy)
		{
			if (child == parent)
			{
				return;
			}
			if (child.GetTag().Equals("IPermission") || child.GetTag().Equals("Permission"))
			{
				parent.AddChild(child);
				return;
			}
			if (parent.Tag.Equals(child.GetTag()))
			{
				SecurityElement securityElement = (SecurityElement)child;
				for (int i = 0; i < securityElement.InternalChildren.Count; i++)
				{
					ISecurityElementFactory securityElementFactory = (ISecurityElementFactory)securityElement.InternalChildren[i];
					parent.AddChildNoDuplicates(securityElementFactory);
				}
				return;
			}
			parent.AddChild((ISecurityElementFactory)(copy ? child.Copy() : child));
		}

		// Token: 0x06003B49 RID: 15177 RVA: 0x000C9AB0 File Offset: 0x000C8AB0
		internal void InplaceIntersect(PermissionSet other)
		{
			Exception ex = null;
			this.m_CheckedForNonCas = false;
			if (this == other)
			{
				return;
			}
			if (other == null || other.FastIsEmpty())
			{
				this.Reset();
				return;
			}
			if (this.FastIsEmpty())
			{
				return;
			}
			int num = ((this.m_permSet == null) ? (-1) : this.m_permSet.GetMaxUsedIndex());
			int num2 = ((other.m_permSet == null) ? (-1) : other.m_permSet.GetMaxUsedIndex());
			if (this.IsUnrestricted() && num < num2)
			{
				num = num2;
				this.CheckSet();
			}
			if (other.IsUnrestricted())
			{
				other.CheckSet();
			}
			for (int i = 0; i <= num; i++)
			{
				object item = this.m_permSet.GetItem(i);
				IPermission permission = item as IPermission;
				ISecurityElementFactory securityElementFactory = item as ISecurityElementFactory;
				object item2 = other.m_permSet.GetItem(i);
				IPermission permission2 = item2 as IPermission;
				ISecurityElementFactory securityElementFactory2 = item2 as ISecurityElementFactory;
				if (item != null || item2 != null)
				{
					if (securityElementFactory != null && securityElementFactory2 != null)
					{
						if (securityElementFactory.GetTag().Equals("PermissionIntersection") || securityElementFactory.GetTag().Equals("PermissionUnrestrictedIntersection"))
						{
							PermissionSet.SafeChildAdd((SecurityElement)securityElementFactory, securityElementFactory2, true);
						}
						else
						{
							bool flag = true;
							if (this.IsUnrestricted())
							{
								SecurityElement securityElement = new SecurityElement("PermissionUnrestrictedUnion");
								securityElement.AddAttribute("class", securityElementFactory.Attribute("class"));
								PermissionSet.SafeChildAdd(securityElement, securityElementFactory, false);
								securityElementFactory = securityElement;
							}
							if (other.IsUnrestricted())
							{
								SecurityElement securityElement2 = new SecurityElement("PermissionUnrestrictedUnion");
								securityElement2.AddAttribute("class", securityElementFactory2.Attribute("class"));
								PermissionSet.SafeChildAdd(securityElement2, securityElementFactory2, true);
								securityElementFactory2 = securityElement2;
								flag = false;
							}
							SecurityElement securityElement3 = new SecurityElement("PermissionIntersection");
							securityElement3.AddAttribute("class", securityElementFactory.Attribute("class"));
							PermissionSet.SafeChildAdd(securityElement3, securityElementFactory, false);
							PermissionSet.SafeChildAdd(securityElement3, securityElementFactory2, flag);
							this.m_permSet.SetItem(i, securityElement3);
						}
					}
					else if (item == null)
					{
						if (this.IsUnrestricted())
						{
							if (securityElementFactory2 != null)
							{
								SecurityElement securityElement4 = new SecurityElement("PermissionUnrestrictedIntersection");
								securityElement4.AddAttribute("class", securityElementFactory2.Attribute("class"));
								PermissionSet.SafeChildAdd(securityElement4, securityElementFactory2, true);
								this.m_permSet.SetItem(i, securityElement4);
							}
							else
							{
								PermissionToken permissionToken = (PermissionToken)PermissionToken.s_tokenSet.GetItem(i);
								if ((permissionToken.m_type & PermissionTokenType.IUnrestricted) != (PermissionTokenType)0)
								{
									this.m_permSet.SetItem(i, permission2.Copy());
								}
							}
						}
					}
					else if (item2 == null)
					{
						if (other.IsUnrestricted())
						{
							if (securityElementFactory != null)
							{
								SecurityElement securityElement5 = new SecurityElement("PermissionUnrestrictedIntersection");
								securityElement5.AddAttribute("class", securityElementFactory.Attribute("class"));
								PermissionSet.SafeChildAdd(securityElement5, securityElementFactory, false);
								this.m_permSet.SetItem(i, securityElement5);
							}
							else
							{
								PermissionToken permissionToken2 = (PermissionToken)PermissionToken.s_tokenSet.GetItem(i);
								if ((permissionToken2.m_type & PermissionTokenType.IUnrestricted) == (PermissionTokenType)0)
								{
									this.m_permSet.SetItem(i, null);
								}
							}
						}
						else
						{
							this.m_permSet.SetItem(i, null);
						}
					}
					else
					{
						if (securityElementFactory != null)
						{
							permission = this.CreatePermission(securityElementFactory, i);
						}
						if (securityElementFactory2 != null)
						{
							permission2 = other.CreatePermission(securityElementFactory2, i);
						}
						try
						{
							IPermission permission3;
							if (permission == null)
							{
								permission3 = permission2;
							}
							else if (permission2 == null)
							{
								permission3 = permission;
							}
							else
							{
								permission3 = permission.Intersect(permission2);
							}
							this.m_permSet.SetItem(i, permission3);
						}
						catch (Exception ex2)
						{
							if (ex == null)
							{
								ex = ex2;
							}
						}
					}
				}
			}
			this.m_Unrestricted = this.m_Unrestricted && other.m_Unrestricted;
			if (ex != null)
			{
				throw ex;
			}
		}

		// Token: 0x06003B4A RID: 15178 RVA: 0x000C9E44 File Offset: 0x000C8E44
		public PermissionSet Intersect(PermissionSet other)
		{
			if (other == null || other.FastIsEmpty() || this.FastIsEmpty())
			{
				return null;
			}
			int num = ((this.m_permSet == null) ? (-1) : this.m_permSet.GetMaxUsedIndex());
			int num2 = ((other.m_permSet == null) ? (-1) : other.m_permSet.GetMaxUsedIndex());
			int num3 = ((num < num2) ? num : num2);
			if (this.IsUnrestricted() && num3 < num2)
			{
				num3 = num2;
				this.CheckSet();
			}
			if (other.IsUnrestricted() && num3 < num)
			{
				num3 = num;
				other.CheckSet();
			}
			PermissionSet permissionSet = new PermissionSet(false);
			if (num3 > -1)
			{
				permissionSet.m_permSet = new TokenBasedSet();
			}
			for (int i = 0; i <= num3; i++)
			{
				object item = this.m_permSet.GetItem(i);
				IPermission permission = item as IPermission;
				ISecurityElementFactory securityElementFactory = item as ISecurityElementFactory;
				object item2 = other.m_permSet.GetItem(i);
				IPermission permission2 = item2 as IPermission;
				ISecurityElementFactory securityElementFactory2 = item2 as ISecurityElementFactory;
				if (item != null || item2 != null)
				{
					if (securityElementFactory != null && securityElementFactory2 != null)
					{
						bool flag = true;
						bool flag2 = true;
						SecurityElement securityElement = new SecurityElement("PermissionIntersection");
						securityElement.AddAttribute("class", securityElementFactory2.Attribute("class"));
						if (this.IsUnrestricted())
						{
							SecurityElement securityElement2 = new SecurityElement("PermissionUnrestrictedUnion");
							securityElement2.AddAttribute("class", securityElementFactory.Attribute("class"));
							PermissionSet.SafeChildAdd(securityElement2, securityElementFactory, true);
							flag2 = false;
							securityElementFactory = securityElement2;
						}
						if (other.IsUnrestricted())
						{
							SecurityElement securityElement3 = new SecurityElement("PermissionUnrestrictedUnion");
							securityElement3.AddAttribute("class", securityElementFactory2.Attribute("class"));
							PermissionSet.SafeChildAdd(securityElement3, securityElementFactory2, true);
							flag = false;
							securityElementFactory2 = securityElement3;
						}
						PermissionSet.SafeChildAdd(securityElement, securityElementFactory2, flag);
						PermissionSet.SafeChildAdd(securityElement, securityElementFactory, flag2);
						permissionSet.m_permSet.SetItem(i, securityElement);
					}
					else if (item == null)
					{
						if (this.m_Unrestricted)
						{
							if (securityElementFactory2 != null)
							{
								SecurityElement securityElement4 = new SecurityElement("PermissionUnrestrictedIntersection");
								securityElement4.AddAttribute("class", securityElementFactory2.Attribute("class"));
								PermissionSet.SafeChildAdd(securityElement4, securityElementFactory2, true);
								permissionSet.m_permSet.SetItem(i, securityElement4);
							}
							else if (permission2 != null)
							{
								PermissionToken permissionToken = (PermissionToken)PermissionToken.s_tokenSet.GetItem(i);
								if ((permissionToken.m_type & PermissionTokenType.IUnrestricted) != (PermissionTokenType)0)
								{
									permissionSet.m_permSet.SetItem(i, permission2.Copy());
								}
							}
						}
					}
					else if (item2 == null)
					{
						if (other.m_Unrestricted)
						{
							if (securityElementFactory != null)
							{
								SecurityElement securityElement5 = new SecurityElement("PermissionUnrestrictedIntersection");
								securityElement5.AddAttribute("class", securityElementFactory.Attribute("class"));
								PermissionSet.SafeChildAdd(securityElement5, securityElementFactory, true);
								permissionSet.m_permSet.SetItem(i, securityElement5);
							}
							else if (permission != null)
							{
								PermissionToken permissionToken2 = (PermissionToken)PermissionToken.s_tokenSet.GetItem(i);
								if ((permissionToken2.m_type & PermissionTokenType.IUnrestricted) != (PermissionTokenType)0)
								{
									permissionSet.m_permSet.SetItem(i, permission.Copy());
								}
							}
						}
					}
					else
					{
						if (securityElementFactory != null)
						{
							permission = this.CreatePermission(securityElementFactory, i);
						}
						if (securityElementFactory2 != null)
						{
							permission2 = other.CreatePermission(securityElementFactory2, i);
						}
						IPermission permission3;
						if (permission == null)
						{
							permission3 = permission2;
						}
						else if (permission2 == null)
						{
							permission3 = permission;
						}
						else
						{
							permission3 = permission.Intersect(permission2);
						}
						permissionSet.m_permSet.SetItem(i, permission3);
					}
				}
			}
			permissionSet.m_Unrestricted = this.m_Unrestricted && other.m_Unrestricted;
			if (permissionSet.FastIsEmpty())
			{
				return null;
			}
			return permissionSet;
		}

		// Token: 0x06003B4B RID: 15179 RVA: 0x000CA1B8 File Offset: 0x000C91B8
		internal void InplaceUnion(PermissionSet other)
		{
			if (this == other)
			{
				return;
			}
			if (other == null || other.FastIsEmpty())
			{
				return;
			}
			this.m_CheckedForNonCas = false;
			this.m_Unrestricted = this.m_Unrestricted || other.m_Unrestricted;
			if (this.m_Unrestricted)
			{
				if (CodeAccessSecurityEngine.DoesFullTrustMeanFullTrust())
				{
					this.m_permSet = null;
					return;
				}
				if (this.m_canUnrestrictedOverride && other.m_canUnrestrictedOverride)
				{
					this.m_permSet = null;
					return;
				}
				if (other.m_canUnrestrictedOverride)
				{
					return;
				}
			}
			int num = -1;
			if (other.m_permSet != null)
			{
				num = other.m_permSet.GetMaxUsedIndex();
				this.CheckSet();
			}
			Exception ex = null;
			for (int i = 0; i <= num; i++)
			{
				object item = this.m_permSet.GetItem(i);
				IPermission permission = item as IPermission;
				ISecurityElementFactory securityElementFactory = item as ISecurityElementFactory;
				object item2 = other.m_permSet.GetItem(i);
				IPermission permission2 = item2 as IPermission;
				ISecurityElementFactory securityElementFactory2 = item2 as ISecurityElementFactory;
				if (item != null || item2 != null)
				{
					if (securityElementFactory != null && securityElementFactory2 != null)
					{
						if (securityElementFactory.GetTag().Equals("PermissionUnion") || securityElementFactory.GetTag().Equals("PermissionUnrestrictedUnion"))
						{
							PermissionSet.SafeChildAdd((SecurityElement)securityElementFactory, securityElementFactory2, true);
						}
						else
						{
							SecurityElement securityElement;
							if (this.IsUnrestricted() || other.IsUnrestricted())
							{
								securityElement = new SecurityElement("PermissionUnrestrictedUnion");
							}
							else
							{
								securityElement = new SecurityElement("PermissionUnion");
							}
							securityElement.AddAttribute("class", securityElementFactory.Attribute("class"));
							PermissionSet.SafeChildAdd(securityElement, securityElementFactory, false);
							PermissionSet.SafeChildAdd(securityElement, securityElementFactory2, true);
							this.m_permSet.SetItem(i, securityElement);
						}
					}
					else if (item == null)
					{
						if (securityElementFactory2 != null)
						{
							this.m_permSet.SetItem(i, securityElementFactory2.Copy());
						}
						else if (permission2 != null)
						{
							PermissionToken permissionToken = (PermissionToken)PermissionToken.s_tokenSet.GetItem(i);
							if ((permissionToken.m_type & PermissionTokenType.IUnrestricted) == (PermissionTokenType)0 || !this.m_Unrestricted)
							{
								this.m_permSet.SetItem(i, permission2.Copy());
							}
						}
					}
					else if (item2 != null)
					{
						if (securityElementFactory != null)
						{
							permission = this.CreatePermission(securityElementFactory, i);
						}
						if (securityElementFactory2 != null)
						{
							permission2 = other.CreatePermission(securityElementFactory2, i);
						}
						try
						{
							IPermission permission3;
							if (permission == null)
							{
								permission3 = permission2;
							}
							else if (permission2 == null)
							{
								permission3 = permission;
							}
							else
							{
								permission3 = permission.Union(permission2);
							}
							this.m_permSet.SetItem(i, permission3);
						}
						catch (Exception ex2)
						{
							if (ex == null)
							{
								ex = ex2;
							}
						}
					}
				}
			}
			if (ex != null)
			{
				throw ex;
			}
		}

		// Token: 0x06003B4C RID: 15180 RVA: 0x000CA424 File Offset: 0x000C9424
		public PermissionSet Union(PermissionSet other)
		{
			if (other == null || other.FastIsEmpty())
			{
				return this.Copy();
			}
			if (this.FastIsEmpty())
			{
				return other.Copy();
			}
			PermissionSet permissionSet = new PermissionSet();
			permissionSet.m_Unrestricted = this.m_Unrestricted || other.m_Unrestricted;
			if (permissionSet.m_Unrestricted)
			{
				if (CodeAccessSecurityEngine.DoesFullTrustMeanFullTrust())
				{
					return permissionSet;
				}
				if (this.m_canUnrestrictedOverride && other.m_canUnrestrictedOverride)
				{
					return permissionSet;
				}
				if (other.m_canUnrestrictedOverride)
				{
					permissionSet.m_permSet = ((this.m_permSet != null) ? new TokenBasedSet(this.m_permSet) : null);
					return permissionSet;
				}
			}
			this.CheckSet();
			other.CheckSet();
			int num = ((this.m_permSet.GetMaxUsedIndex() > other.m_permSet.GetMaxUsedIndex()) ? this.m_permSet.GetMaxUsedIndex() : other.m_permSet.GetMaxUsedIndex());
			permissionSet.m_permSet = new TokenBasedSet();
			for (int i = 0; i <= num; i++)
			{
				object item = this.m_permSet.GetItem(i);
				IPermission permission = item as IPermission;
				ISecurityElementFactory securityElementFactory = item as ISecurityElementFactory;
				object item2 = other.m_permSet.GetItem(i);
				IPermission permission2 = item2 as IPermission;
				ISecurityElementFactory securityElementFactory2 = item2 as ISecurityElementFactory;
				if (item != null || item2 != null)
				{
					if (securityElementFactory != null && securityElementFactory2 != null)
					{
						SecurityElement securityElement;
						if (this.IsUnrestricted() || other.IsUnrestricted())
						{
							securityElement = new SecurityElement("PermissionUnrestrictedUnion");
						}
						else
						{
							securityElement = new SecurityElement("PermissionUnion");
						}
						securityElement.AddAttribute("class", securityElementFactory.Attribute("class"));
						PermissionSet.SafeChildAdd(securityElement, securityElementFactory, true);
						PermissionSet.SafeChildAdd(securityElement, securityElementFactory2, true);
						permissionSet.m_permSet.SetItem(i, securityElement);
					}
					else if (item == null)
					{
						if (securityElementFactory2 != null)
						{
							permissionSet.m_permSet.SetItem(i, securityElementFactory2.Copy());
						}
						else if (permission2 != null)
						{
							PermissionToken permissionToken = (PermissionToken)PermissionToken.s_tokenSet.GetItem(i);
							if ((permissionToken.m_type & PermissionTokenType.IUnrestricted) == (PermissionTokenType)0 || !permissionSet.m_Unrestricted)
							{
								permissionSet.m_permSet.SetItem(i, permission2.Copy());
							}
						}
					}
					else if (item2 == null)
					{
						if (securityElementFactory != null)
						{
							permissionSet.m_permSet.SetItem(i, securityElementFactory.Copy());
						}
						else if (permission != null)
						{
							PermissionToken permissionToken2 = (PermissionToken)PermissionToken.s_tokenSet.GetItem(i);
							if ((permissionToken2.m_type & PermissionTokenType.IUnrestricted) == (PermissionTokenType)0 || !permissionSet.m_Unrestricted)
							{
								permissionSet.m_permSet.SetItem(i, permission.Copy());
							}
						}
					}
					else
					{
						if (securityElementFactory != null)
						{
							permission = this.CreatePermission(securityElementFactory, i);
						}
						if (securityElementFactory2 != null)
						{
							permission2 = other.CreatePermission(securityElementFactory2, i);
						}
						IPermission permission3;
						if (permission == null)
						{
							permission3 = permission2;
						}
						else if (permission2 == null)
						{
							permission3 = permission;
						}
						else
						{
							permission3 = permission.Union(permission2);
						}
						permissionSet.m_permSet.SetItem(i, permission3);
					}
				}
			}
			return permissionSet;
		}

		// Token: 0x06003B4D RID: 15181 RVA: 0x000CA6E0 File Offset: 0x000C96E0
		internal void MergeDeniedSet(PermissionSet denied)
		{
			if (denied == null || denied.FastIsEmpty() || this.FastIsEmpty())
			{
				return;
			}
			this.m_CheckedForNonCas = false;
			if (this.m_permSet == null || denied.m_permSet == null)
			{
				return;
			}
			int num = ((denied.m_permSet.GetMaxUsedIndex() > this.m_permSet.GetMaxUsedIndex()) ? this.m_permSet.GetMaxUsedIndex() : denied.m_permSet.GetMaxUsedIndex());
			for (int i = 0; i <= num; i++)
			{
				IPermission permission = denied.m_permSet.GetItem(i) as IPermission;
				if (permission != null)
				{
					IPermission permission2 = this.m_permSet.GetItem(i) as IPermission;
					if (permission2 == null && !this.m_Unrestricted)
					{
						denied.m_permSet.SetItem(i, null);
					}
					else if (permission2 != null && permission != null && permission2.IsSubsetOf(permission))
					{
						this.m_permSet.SetItem(i, null);
						denied.m_permSet.SetItem(i, null);
					}
				}
			}
		}

		// Token: 0x06003B4E RID: 15182 RVA: 0x000CA7C0 File Offset: 0x000C97C0
		internal bool Contains(IPermission perm)
		{
			if (perm == null)
			{
				return true;
			}
			if (this.m_Unrestricted && CodeAccessPermission.CanUnrestrictedOverride(perm))
			{
				return true;
			}
			if (this.FastIsEmpty())
			{
				return false;
			}
			PermissionToken token = PermissionToken.GetToken(perm);
			if (this.m_permSet.GetItem(token.m_index) == null)
			{
				return perm.IsSubsetOf(null);
			}
			IPermission permission = this.GetPermission(token.m_index);
			if (permission != null)
			{
				return perm.IsSubsetOf(permission);
			}
			return perm.IsSubsetOf(null);
		}

		// Token: 0x06003B4F RID: 15183 RVA: 0x000CA834 File Offset: 0x000C9834
		[ComVisible(false)]
		public override bool Equals(object obj)
		{
			PermissionSet permissionSet = obj as PermissionSet;
			if (permissionSet == null)
			{
				return false;
			}
			if (this.m_Unrestricted != permissionSet.m_Unrestricted)
			{
				return false;
			}
			this.CheckSet();
			permissionSet.CheckSet();
			this.DecodeAllPermissions();
			permissionSet.DecodeAllPermissions();
			int num = Math.Max(this.m_permSet.GetMaxUsedIndex(), permissionSet.m_permSet.GetMaxUsedIndex());
			for (int i = 0; i <= num; i++)
			{
				IPermission permission = (IPermission)this.m_permSet.GetItem(i);
				IPermission permission2 = (IPermission)permissionSet.m_permSet.GetItem(i);
				if (permission != null || permission2 != null)
				{
					if (permission == null)
					{
						if (!permission2.IsSubsetOf(null))
						{
							return false;
						}
					}
					else if (permission2 == null)
					{
						if (!permission.IsSubsetOf(null))
						{
							return false;
						}
					}
					else if (!permission.Equals(permission2))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06003B50 RID: 15184 RVA: 0x000CA8F4 File Offset: 0x000C98F4
		[ComVisible(false)]
		public override int GetHashCode()
		{
			int num = (this.m_Unrestricted ? (-1) : 0);
			if (this.m_permSet != null)
			{
				this.DecodeAllPermissions();
				int maxUsedIndex = this.m_permSet.GetMaxUsedIndex();
				for (int i = this.m_permSet.GetStartingIndex(); i <= maxUsedIndex; i++)
				{
					IPermission permission = (IPermission)this.m_permSet.GetItem(i);
					if (permission != null)
					{
						num ^= permission.GetHashCode();
					}
				}
			}
			return num;
		}

		// Token: 0x06003B51 RID: 15185 RVA: 0x000CA960 File Offset: 0x000C9960
		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Demand()
		{
			if (this.FastIsEmpty())
			{
				return;
			}
			this.ContainsNonCodeAccessPermissions();
			if (this.m_ContainsCas)
			{
				StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCallersCaller;
				CodeAccessSecurityEngine.Check(this.GetCasOnlySet(), ref stackCrawlMark);
			}
			if (this.m_ContainsNonCas)
			{
				this.DemandNonCAS();
			}
		}

		// Token: 0x06003B52 RID: 15186 RVA: 0x000CA9A4 File Offset: 0x000C99A4
		internal void DemandNonCAS()
		{
			this.ContainsNonCodeAccessPermissions();
			if (this.m_ContainsNonCas && this.m_permSet != null)
			{
				this.CheckSet();
				for (int i = this.m_permSet.GetStartingIndex(); i <= this.m_permSet.GetMaxUsedIndex(); i++)
				{
					IPermission permission = this.GetPermission(i);
					if (permission != null && !(permission is CodeAccessPermission))
					{
						permission.Demand();
					}
				}
			}
		}

		// Token: 0x06003B53 RID: 15187 RVA: 0x000CAA08 File Offset: 0x000C9A08
		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Assert()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			SecurityRuntime.Assert(this, ref stackCrawlMark);
		}

		// Token: 0x06003B54 RID: 15188 RVA: 0x000CAA20 File Offset: 0x000C9A20
		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Deny()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			SecurityRuntime.Deny(this, ref stackCrawlMark);
		}

		// Token: 0x06003B55 RID: 15189 RVA: 0x000CAA38 File Offset: 0x000C9A38
		[MethodImpl(MethodImplOptions.NoInlining)]
		public void PermitOnly()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			SecurityRuntime.PermitOnly(this, ref stackCrawlMark);
		}

		// Token: 0x06003B56 RID: 15190 RVA: 0x000CAA50 File Offset: 0x000C9A50
		internal IPermission GetFirstPerm()
		{
			IEnumerator enumerator = this.GetEnumerator();
			if (!enumerator.MoveNext())
			{
				return null;
			}
			return enumerator.Current as IPermission;
		}

		// Token: 0x06003B57 RID: 15191 RVA: 0x000CAA79 File Offset: 0x000C9A79
		public virtual PermissionSet Copy()
		{
			return new PermissionSet(this);
		}

		// Token: 0x06003B58 RID: 15192 RVA: 0x000CAA84 File Offset: 0x000C9A84
		internal PermissionSet CopyWithNoIdentityPermissions()
		{
			PermissionSet permissionSet = this.Copy();
			permissionSet.RemovePermission(typeof(GacIdentityPermission));
			permissionSet.RemovePermission(typeof(PublisherIdentityPermission));
			permissionSet.RemovePermission(typeof(StrongNameIdentityPermission));
			permissionSet.RemovePermission(typeof(UrlIdentityPermission));
			permissionSet.RemovePermission(typeof(ZoneIdentityPermission));
			return permissionSet;
		}

		// Token: 0x06003B59 RID: 15193 RVA: 0x000CAAEE File Offset: 0x000C9AEE
		public IEnumerator GetEnumerator()
		{
			return new PermissionSetEnumerator(this);
		}

		// Token: 0x06003B5A RID: 15194 RVA: 0x000CAAF6 File Offset: 0x000C9AF6
		internal PermissionSetEnumeratorInternal GetEnumeratorInternal()
		{
			return new PermissionSetEnumeratorInternal(this);
		}

		// Token: 0x06003B5B RID: 15195 RVA: 0x000CAAFE File Offset: 0x000C9AFE
		public override string ToString()
		{
			return this.ToXml().ToString();
		}

		// Token: 0x06003B5C RID: 15196 RVA: 0x000CAB0C File Offset: 0x000C9B0C
		private void NormalizePermissionSet()
		{
			PermissionSet permissionSet = new PermissionSet(false);
			permissionSet.m_Unrestricted = this.m_Unrestricted;
			if (this.m_permSet != null)
			{
				for (int i = this.m_permSet.GetStartingIndex(); i <= this.m_permSet.GetMaxUsedIndex(); i++)
				{
					object item = this.m_permSet.GetItem(i);
					IPermission permission = item as IPermission;
					ISecurityElementFactory securityElementFactory = item as ISecurityElementFactory;
					if (securityElementFactory != null)
					{
						permission = this.CreatePerm(securityElementFactory);
					}
					if (permission != null)
					{
						permissionSet.SetPermission(permission);
					}
				}
			}
			this.m_permSet = permissionSet.m_permSet;
		}

		// Token: 0x06003B5D RID: 15197 RVA: 0x000CAB94 File Offset: 0x000C9B94
		private bool DecodeXml(byte[] data, HostProtectionResource fullTrustOnlyResources, HostProtectionResource inaccessibleResources)
		{
			if (data != null && data.Length > 0)
			{
				this.FromXml(new Parser(data, Tokenizer.ByteTokenEncoding.UnicodeTokens).GetTopElement());
			}
			this.FilterHostProtectionPermissions(fullTrustOnlyResources, inaccessibleResources);
			this.DecodeAllPermissions();
			return true;
		}

		// Token: 0x06003B5E RID: 15198 RVA: 0x000CABC0 File Offset: 0x000C9BC0
		private void DecodeAllPermissions()
		{
			if (this.m_permSet == null)
			{
				this.m_allPermissionsDecoded = true;
				return;
			}
			this.m_canUnrestrictedOverride = true;
			int maxUsedIndex = this.m_permSet.GetMaxUsedIndex();
			for (int i = this.m_permSet.GetStartingIndex(); i <= maxUsedIndex; i++)
			{
				IPermission permission = this.GetPermission(i);
				if (permission != null && !CodeAccessPermission.CanUnrestrictedOverride(permission))
				{
					this.m_canUnrestrictedOverride = false;
				}
			}
			this.m_allPermissionsDecoded = true;
		}

		// Token: 0x06003B5F RID: 15199 RVA: 0x000CAC28 File Offset: 0x000C9C28
		internal void FilterHostProtectionPermissions(HostProtectionResource fullTrustOnly, HostProtectionResource inaccessible)
		{
			HostProtectionPermission.protectedResources = fullTrustOnly;
			HostProtectionPermission hostProtectionPermission = (HostProtectionPermission)this.GetPermission(HostProtectionPermission.GetTokenIndex());
			if (hostProtectionPermission == null)
			{
				return;
			}
			HostProtectionPermission hostProtectionPermission2 = (HostProtectionPermission)hostProtectionPermission.Intersect(new HostProtectionPermission(fullTrustOnly));
			if (hostProtectionPermission2 == null)
			{
				this.RemovePermission(PermissionToken.FindTokenByIndex(HostProtectionPermission.GetTokenIndex()));
				return;
			}
			if (hostProtectionPermission2.Resources != hostProtectionPermission.Resources)
			{
				this.SetPermission(hostProtectionPermission2);
			}
		}

		// Token: 0x06003B60 RID: 15200 RVA: 0x000CAC90 File Offset: 0x000C9C90
		private bool DecodeUsingSerialization(byte[] data)
		{
			MemoryStream memoryStream = new MemoryStream(data);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			object obj = binaryFormatter.Deserialize(memoryStream);
			if (obj != null)
			{
				PermissionSet permissionSet = (PermissionSet)obj;
				this.m_Unrestricted = permissionSet.m_Unrestricted;
				this.m_permSet = permissionSet.m_permSet;
				this.m_CheckedForNonCas = false;
				return true;
			}
			return false;
		}

		// Token: 0x06003B61 RID: 15201 RVA: 0x000CACE0 File Offset: 0x000C9CE0
		public virtual void FromXml(SecurityElement et)
		{
			this.FromXml(et, false, false);
		}

		// Token: 0x06003B62 RID: 15202 RVA: 0x000CACEC File Offset: 0x000C9CEC
		internal static bool IsPermissionTag(string tag, bool allowInternalOnly)
		{
			return tag.Equals("Permission") || tag.Equals("IPermission") || (allowInternalOnly && (tag.Equals("PermissionUnion") || tag.Equals("PermissionIntersection") || tag.Equals("PermissionUnrestrictedIntersection") || tag.Equals("PermissionUnrestrictedUnion")));
		}

		// Token: 0x06003B63 RID: 15203 RVA: 0x000CAD50 File Offset: 0x000C9D50
		internal virtual void FromXml(SecurityElement et, bool allowInternalOnly, bool ignoreTypeLoadFailures)
		{
			if (et == null)
			{
				throw new ArgumentNullException("et");
			}
			if (!et.Tag.Equals("PermissionSet"))
			{
				throw new ArgumentException(string.Format(null, Environment.GetResourceString("Argument_InvalidXMLElement"), new object[]
				{
					"PermissionSet",
					base.GetType().FullName
				}));
			}
			this.Reset();
			this.m_ignoreTypeLoadFailures = ignoreTypeLoadFailures;
			this.m_allPermissionsDecoded = false;
			this.m_Unrestricted = XMLUtil.IsUnrestricted(et);
			if (et.InternalChildren != null)
			{
				int count = et.InternalChildren.Count;
				for (int i = 0; i < count; i++)
				{
					SecurityElement securityElement = (SecurityElement)et.Children[i];
					if (PermissionSet.IsPermissionTag(securityElement.Tag, allowInternalOnly))
					{
						string text = securityElement.Attribute("class");
						PermissionToken permissionToken;
						object obj;
						if (text != null)
						{
							permissionToken = PermissionToken.GetToken(text);
							if (permissionToken == null)
							{
								obj = this.CreatePerm(securityElement);
								if (obj != null)
								{
									permissionToken = PermissionToken.GetToken((IPermission)obj);
								}
							}
							else
							{
								obj = securityElement;
							}
						}
						else
						{
							IPermission permission = this.CreatePerm(securityElement);
							if (permission == null)
							{
								permissionToken = null;
								obj = null;
							}
							else
							{
								permissionToken = PermissionToken.GetToken(permission);
								obj = permission;
							}
						}
						if (permissionToken != null && obj != null)
						{
							if (this.m_permSet == null)
							{
								this.m_permSet = new TokenBasedSet();
							}
							if (this.m_permSet.GetItem(permissionToken.m_index) != null)
							{
								IPermission permission2;
								if (this.m_permSet.GetItem(permissionToken.m_index) is IPermission)
								{
									permission2 = (IPermission)this.m_permSet.GetItem(permissionToken.m_index);
								}
								else
								{
									permission2 = this.CreatePerm((SecurityElement)this.m_permSet.GetItem(permissionToken.m_index));
								}
								if (obj is IPermission)
								{
									obj = ((IPermission)obj).Union(permission2);
								}
								else
								{
									obj = this.CreatePerm((SecurityElement)obj).Union(permission2);
								}
							}
							if (this.m_Unrestricted && obj is IPermission && CodeAccessPermission.CanUnrestrictedOverride((IPermission)obj))
							{
								obj = null;
							}
							this.m_permSet.SetItem(permissionToken.m_index, obj);
						}
					}
				}
			}
		}

		// Token: 0x06003B64 RID: 15204 RVA: 0x000CAF74 File Offset: 0x000C9F74
		internal virtual void FromXml(SecurityDocument doc, int position, bool allowInternalOnly)
		{
			if (doc == null)
			{
				throw new ArgumentNullException("doc");
			}
			if (!doc.GetTagForElement(position).Equals("PermissionSet"))
			{
				throw new ArgumentException(string.Format(null, Environment.GetResourceString("Argument_InvalidXMLElement"), new object[]
				{
					"PermissionSet",
					base.GetType().FullName
				}));
			}
			this.Reset();
			this.m_allPermissionsDecoded = false;
			Exception ex = null;
			string attributeForElement = doc.GetAttributeForElement(position, "Unrestricted");
			if (attributeForElement != null)
			{
				this.m_Unrestricted = attributeForElement.Equals("True") || attributeForElement.Equals("true") || attributeForElement.Equals("TRUE");
			}
			else
			{
				this.m_Unrestricted = false;
			}
			ArrayList childrenPositionForElement = doc.GetChildrenPositionForElement(position);
			int count = childrenPositionForElement.Count;
			for (int i = 0; i < count; i++)
			{
				int num = (int)childrenPositionForElement[i];
				if (PermissionSet.IsPermissionTag(doc.GetTagForElement(num), allowInternalOnly))
				{
					try
					{
						string attributeForElement2 = doc.GetAttributeForElement(num, "class");
						PermissionToken permissionToken;
						object obj;
						if (attributeForElement2 != null)
						{
							permissionToken = PermissionToken.GetToken(attributeForElement2);
							if (permissionToken == null)
							{
								obj = this.CreatePerm(doc.GetElement(num, true));
								if (obj != null)
								{
									permissionToken = PermissionToken.GetToken((IPermission)obj);
								}
							}
							else
							{
								obj = ((ISecurityElementFactory)new SecurityDocumentElement(doc, num)).CreateSecurityElement();
							}
						}
						else
						{
							IPermission permission = this.CreatePerm(doc.GetElement(num, true));
							if (permission == null)
							{
								permissionToken = null;
								obj = null;
							}
							else
							{
								permissionToken = PermissionToken.GetToken(permission);
								obj = permission;
							}
						}
						if (permissionToken != null && obj != null)
						{
							if (this.m_permSet == null)
							{
								this.m_permSet = new TokenBasedSet();
							}
							IPermission permission2 = null;
							if (this.m_permSet.GetItem(permissionToken.m_index) != null)
							{
								if (this.m_permSet.GetItem(permissionToken.m_index) is IPermission)
								{
									permission2 = (IPermission)this.m_permSet.GetItem(permissionToken.m_index);
								}
								else
								{
									permission2 = this.CreatePerm(this.m_permSet.GetItem(permissionToken.m_index));
								}
							}
							if (permission2 != null)
							{
								if (obj is IPermission)
								{
									obj = permission2.Union((IPermission)obj);
								}
								else
								{
									obj = permission2.Union(this.CreatePerm(obj));
								}
							}
							if (this.m_Unrestricted && obj is IPermission && CodeAccessPermission.CanUnrestrictedOverride((IPermission)obj))
							{
								obj = null;
							}
							this.m_permSet.SetItem(permissionToken.m_index, obj);
						}
					}
					catch (Exception ex2)
					{
						if (ex == null)
						{
							ex = ex2;
						}
					}
				}
			}
			if (ex != null)
			{
				throw ex;
			}
		}

		// Token: 0x06003B65 RID: 15205 RVA: 0x000CB210 File Offset: 0x000CA210
		private IPermission CreatePerm(object obj)
		{
			return PermissionSet.CreatePerm(obj, this.m_ignoreTypeLoadFailures);
		}

		// Token: 0x06003B66 RID: 15206 RVA: 0x000CB220 File Offset: 0x000CA220
		internal static IPermission CreatePerm(object obj, bool ignoreTypeLoadFailures)
		{
			SecurityElement securityElement = obj as SecurityElement;
			ISecurityElementFactory securityElementFactory = obj as ISecurityElementFactory;
			if (securityElement == null && securityElementFactory != null)
			{
				securityElement = securityElementFactory.CreateSecurityElement();
			}
			IPermission permission = null;
			string tag;
			switch (tag = securityElement.Tag)
			{
			case "PermissionUnion":
				foreach (object obj2 in securityElement.Children)
				{
					IPermission permission2 = PermissionSet.CreatePerm((SecurityElement)obj2, ignoreTypeLoadFailures);
					if (permission != null)
					{
						permission = permission.Union(permission2);
					}
					else
					{
						permission = permission2;
					}
				}
				break;
			case "PermissionIntersection":
				foreach (object obj3 in securityElement.Children)
				{
					IPermission permission3 = PermissionSet.CreatePerm((SecurityElement)obj3, ignoreTypeLoadFailures);
					if (permission != null)
					{
						permission = permission.Intersect(permission3);
					}
					else
					{
						permission = permission3;
					}
					if (permission == null)
					{
						return null;
					}
				}
				break;
			case "PermissionUnrestrictedUnion":
			{
				IEnumerator enumerator = securityElement.Children.GetEnumerator();
				bool flag = true;
				while (enumerator.MoveNext())
				{
					object obj4 = enumerator.Current;
					IPermission permission4 = PermissionSet.CreatePerm((SecurityElement)obj4, ignoreTypeLoadFailures);
					if (permission4 != null)
					{
						PermissionToken token = PermissionToken.GetToken(permission4);
						if ((token.m_type & PermissionTokenType.IUnrestricted) != (PermissionTokenType)0)
						{
							permission = XMLUtil.CreatePermission(PermissionSet.GetPermissionElement((SecurityElement)enumerator.Current), PermissionState.Unrestricted, ignoreTypeLoadFailures);
							break;
						}
						if (flag)
						{
							permission = permission4;
						}
						else
						{
							permission = permission4.Union(permission);
						}
						flag = false;
					}
				}
				break;
			}
			case "PermissionUnrestrictedIntersection":
				foreach (object obj5 in securityElement.Children)
				{
					IPermission permission5 = PermissionSet.CreatePerm((SecurityElement)obj5, ignoreTypeLoadFailures);
					if (permission5 == null)
					{
						return null;
					}
					PermissionToken token2 = PermissionToken.GetToken(permission5);
					if ((token2.m_type & PermissionTokenType.IUnrestricted) != (PermissionTokenType)0)
					{
						if (permission != null)
						{
							permission = permission5.Intersect(permission);
						}
						else
						{
							permission = permission5;
						}
					}
					else
					{
						permission = null;
					}
					if (permission == null)
					{
						return null;
					}
				}
				break;
			case "IPermission":
			case "Permission":
				permission = securityElement.ToPermission(ignoreTypeLoadFailures);
				break;
			}
			return permission;
		}

		// Token: 0x06003B67 RID: 15207 RVA: 0x000CB458 File Offset: 0x000CA458
		internal IPermission CreatePermission(object obj, int index)
		{
			IPermission permission = this.CreatePerm(obj);
			if (permission == null)
			{
				return null;
			}
			if (this.m_Unrestricted && CodeAccessPermission.CanUnrestrictedOverride(permission))
			{
				permission = null;
			}
			this.CheckSet();
			this.m_permSet.SetItem(index, permission);
			if (permission != null)
			{
				PermissionToken token = PermissionToken.GetToken(permission);
				if (token != null && token.m_index != index)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_UnableToGeneratePermissionSet"));
				}
			}
			return permission;
		}

		// Token: 0x06003B68 RID: 15208 RVA: 0x000CB4C0 File Offset: 0x000CA4C0
		private static SecurityElement GetPermissionElement(SecurityElement el)
		{
			string tag;
			if ((tag = el.Tag) != null && (tag == "IPermission" || tag == "Permission"))
			{
				return el;
			}
			IEnumerator enumerator = el.Children.GetEnumerator();
			if (enumerator.MoveNext())
			{
				return PermissionSet.GetPermissionElement((SecurityElement)enumerator.Current);
			}
			return null;
		}

		// Token: 0x06003B69 RID: 15209 RVA: 0x000CB51C File Offset: 0x000CA51C
		internal static SecurityElement CreateEmptyPermissionSetXml()
		{
			SecurityElement securityElement = new SecurityElement("PermissionSet");
			securityElement.AddAttribute("class", "System.Security.PermissionSet");
			securityElement.AddAttribute("version", "1");
			return securityElement;
		}

		// Token: 0x06003B6A RID: 15210 RVA: 0x000CB558 File Offset: 0x000CA558
		internal SecurityElement ToXml(string permName)
		{
			SecurityElement securityElement = new SecurityElement("PermissionSet");
			securityElement.AddAttribute("class", permName);
			securityElement.AddAttribute("version", "1");
			PermissionSetEnumeratorInternal permissionSetEnumeratorInternal = new PermissionSetEnumeratorInternal(this);
			if (this.m_Unrestricted)
			{
				securityElement.AddAttribute("Unrestricted", "true");
			}
			while (permissionSetEnumeratorInternal.MoveNext())
			{
				object obj = permissionSetEnumeratorInternal.Current;
				IPermission permission = (IPermission)obj;
				if (!this.m_Unrestricted || !CodeAccessPermission.CanUnrestrictedOverride(permission))
				{
					securityElement.AddChild(permission.ToXml());
				}
			}
			return securityElement;
		}

		// Token: 0x06003B6B RID: 15211 RVA: 0x000CB5E4 File Offset: 0x000CA5E4
		internal SecurityElement InternalToXml()
		{
			SecurityElement securityElement = new SecurityElement("PermissionSet");
			securityElement.AddAttribute("class", base.GetType().FullName);
			securityElement.AddAttribute("version", "1");
			if (this.m_Unrestricted)
			{
				securityElement.AddAttribute("Unrestricted", "true");
			}
			if (this.m_permSet != null)
			{
				int maxUsedIndex = this.m_permSet.GetMaxUsedIndex();
				for (int i = this.m_permSet.GetStartingIndex(); i <= maxUsedIndex; i++)
				{
					object item = this.m_permSet.GetItem(i);
					if (item != null)
					{
						if (item is IPermission)
						{
							if (!this.m_Unrestricted || !CodeAccessPermission.CanUnrestrictedOverride((IPermission)item))
							{
								securityElement.AddChild(((IPermission)item).ToXml());
							}
						}
						else
						{
							securityElement.AddChild((SecurityElement)item);
						}
					}
				}
			}
			return securityElement;
		}

		// Token: 0x06003B6C RID: 15212 RVA: 0x000CB6B1 File Offset: 0x000CA6B1
		public virtual SecurityElement ToXml()
		{
			return this.ToXml("System.Security.PermissionSet");
		}

		// Token: 0x06003B6D RID: 15213 RVA: 0x000CB6C0 File Offset: 0x000CA6C0
		internal byte[] EncodeUsingSerialization()
		{
			MemoryStream memoryStream = new MemoryStream();
			new BinaryFormatter().Serialize(memoryStream, this);
			return memoryStream.ToArray();
		}

		// Token: 0x06003B6E RID: 15214 RVA: 0x000CB6E8 File Offset: 0x000CA6E8
		internal byte[] EncodeXml()
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream, Encoding.Unicode);
			binaryWriter.Write(this.ToXml().ToString());
			binaryWriter.Flush();
			memoryStream.Position = 2L;
			int num = (int)memoryStream.Length - 2;
			byte[] array = new byte[num];
			memoryStream.Read(array, 0, array.Length);
			return array;
		}

		// Token: 0x06003B6F RID: 15215 RVA: 0x000CB744 File Offset: 0x000CA744
		public static byte[] ConvertPermissionSet(string inFormat, byte[] inData, string outFormat)
		{
			if (inData == null)
			{
				return null;
			}
			if (inFormat == null)
			{
				throw new ArgumentNullException("inFormat");
			}
			if (outFormat == null)
			{
				throw new ArgumentNullException("outFormat");
			}
			PermissionSet permissionSet = new PermissionSet(false);
			inFormat = string.SmallCharToUpper(inFormat);
			outFormat = string.SmallCharToUpper(outFormat);
			if (inFormat.Equals("XMLASCII") || inFormat.Equals("XML"))
			{
				permissionSet.FromXml(new Parser(inData, Tokenizer.ByteTokenEncoding.ByteTokens).GetTopElement());
			}
			else if (inFormat.Equals("XMLUNICODE"))
			{
				permissionSet.FromXml(new Parser(inData, Tokenizer.ByteTokenEncoding.UnicodeTokens).GetTopElement());
			}
			else
			{
				if (!inFormat.Equals("BINARY"))
				{
					return null;
				}
				permissionSet.DecodeUsingSerialization(inData);
			}
			if (outFormat.Equals("XMLASCII") || outFormat.Equals("XML"))
			{
				MemoryStream memoryStream = new MemoryStream();
				StreamWriter streamWriter = new StreamWriter(memoryStream, Encoding.ASCII);
				streamWriter.Write(permissionSet.ToXml().ToString());
				streamWriter.Flush();
				return memoryStream.ToArray();
			}
			if (outFormat.Equals("XMLUNICODE"))
			{
				MemoryStream memoryStream2 = new MemoryStream();
				StreamWriter streamWriter2 = new StreamWriter(memoryStream2, Encoding.Unicode);
				streamWriter2.Write(permissionSet.ToXml().ToString());
				streamWriter2.Flush();
				memoryStream2.Position = 2L;
				int num = (int)memoryStream2.Length - 2;
				byte[] array = new byte[num];
				memoryStream2.Read(array, 0, array.Length);
				return array;
			}
			if (outFormat.Equals("BINARY"))
			{
				return permissionSet.EncodeUsingSerialization();
			}
			return null;
		}

		// Token: 0x06003B70 RID: 15216 RVA: 0x000CB8B8 File Offset: 0x000CA8B8
		public bool ContainsNonCodeAccessPermissions()
		{
			if (this.m_CheckedForNonCas)
			{
				return this.m_ContainsNonCas;
			}
			lock (this)
			{
				if (this.m_CheckedForNonCas)
				{
					return this.m_ContainsNonCas;
				}
				this.m_ContainsCas = false;
				this.m_ContainsNonCas = false;
				if (this.IsUnrestricted())
				{
					this.m_ContainsCas = true;
				}
				if (this.m_permSet != null)
				{
					PermissionSetEnumeratorInternal permissionSetEnumeratorInternal = new PermissionSetEnumeratorInternal(this);
					while (permissionSetEnumeratorInternal.MoveNext() && (!this.m_ContainsCas || !this.m_ContainsNonCas))
					{
						IPermission permission = permissionSetEnumeratorInternal.Current as IPermission;
						if (permission != null)
						{
							if (permission is CodeAccessPermission)
							{
								this.m_ContainsCas = true;
							}
							else
							{
								this.m_ContainsNonCas = true;
							}
						}
					}
				}
				this.m_CheckedForNonCas = true;
			}
			return this.m_ContainsNonCas;
		}

		// Token: 0x06003B71 RID: 15217 RVA: 0x000CB988 File Offset: 0x000CA988
		private PermissionSet GetCasOnlySet()
		{
			if (!this.m_ContainsNonCas)
			{
				return this;
			}
			if (this.IsUnrestricted())
			{
				return this;
			}
			PermissionSet permissionSet = new PermissionSet(false);
			PermissionSetEnumeratorInternal permissionSetEnumeratorInternal = new PermissionSetEnumeratorInternal(this);
			while (permissionSetEnumeratorInternal.MoveNext())
			{
				object obj = permissionSetEnumeratorInternal.Current;
				IPermission permission = (IPermission)obj;
				if (permission is CodeAccessPermission)
				{
					permissionSet.AddPermission(permission);
				}
			}
			permissionSet.m_CheckedForNonCas = true;
			permissionSet.m_ContainsCas = !permissionSet.IsEmpty();
			permissionSet.m_ContainsNonCas = false;
			return permissionSet;
		}

		// Token: 0x06003B72 RID: 15218 RVA: 0x000CBA00 File Offset: 0x000CAA00
		private static void SetupSecurity()
		{
			PolicyLevel policyLevel = PolicyLevel.CreateAppDomainLevel();
			CodeGroup codeGroup = new UnionCodeGroup(new AllMembershipCondition(), policyLevel.GetNamedPermissionSet("Execution"));
			StrongNamePublicKeyBlob strongNamePublicKeyBlob = new StrongNamePublicKeyBlob("002400000480000094000000060200000024000052534131000400000100010007D1FA57C4AED9F0A32E84AA0FAEFD0DE9E8FD6AEC8F87FB03766C834C99921EB23BE79AD9D5DCC1DD9AD236132102900B723CF980957FC4E177108FC607774F29E8320E92EA05ECE4E821C0A5EFE8F1645C4C0C93C1AB99285D622CAA652C1DFAD63D745D6F2DE5F17E5EAF0FC4963D261C8A12436518206DC093344D5AD293");
			CodeGroup codeGroup2 = new UnionCodeGroup(new StrongNameMembershipCondition(strongNamePublicKeyBlob, null, null), policyLevel.GetNamedPermissionSet("FullTrust"));
			StrongNamePublicKeyBlob strongNamePublicKeyBlob2 = new StrongNamePublicKeyBlob("00000000000000000400000000000000");
			CodeGroup codeGroup3 = new UnionCodeGroup(new StrongNameMembershipCondition(strongNamePublicKeyBlob2, null, null), policyLevel.GetNamedPermissionSet("FullTrust"));
			CodeGroup codeGroup4 = new UnionCodeGroup(new GacMembershipCondition(), policyLevel.GetNamedPermissionSet("FullTrust"));
			codeGroup.AddChild(codeGroup2);
			codeGroup.AddChild(codeGroup3);
			codeGroup.AddChild(codeGroup4);
			policyLevel.RootCodeGroup = codeGroup;
			try
			{
				AppDomain.CurrentDomain.SetAppDomainPolicy(policyLevel);
			}
			catch (PolicyException)
			{
			}
		}

		// Token: 0x06003B73 RID: 15219 RVA: 0x000CBACC File Offset: 0x000CAACC
		private static void MergePermission(IPermission perm, bool separateCasFromNonCas, ref PermissionSet casPset, ref PermissionSet nonCasPset)
		{
			if (perm == null)
			{
				return;
			}
			if (!separateCasFromNonCas || perm is CodeAccessPermission)
			{
				if (casPset == null)
				{
					casPset = new PermissionSet(false);
				}
				IPermission permission = casPset.GetPermission(perm);
				IPermission permission2 = casPset.AddPermission(perm);
				if (permission != null && !permission.IsSubsetOf(permission2))
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_DeclarativeUnion"));
				}
			}
			else
			{
				if (nonCasPset == null)
				{
					nonCasPset = new PermissionSet(false);
				}
				IPermission permission3 = nonCasPset.GetPermission(perm);
				IPermission permission4 = nonCasPset.AddPermission(perm);
				if (permission3 != null && !permission3.IsSubsetOf(permission4))
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_DeclarativeUnion"));
				}
			}
		}

		// Token: 0x06003B74 RID: 15220 RVA: 0x000CBB5C File Offset: 0x000CAB5C
		private static byte[] CreateSerialized(object[] attrs, bool serialize, ref byte[] nonCasBlob, out PermissionSet casPset, HostProtectionResource fullTrustOnlyResources)
		{
			casPset = null;
			PermissionSet permissionSet = null;
			for (int i = 0; i < attrs.Length; i++)
			{
				if (attrs[i] is PermissionSetAttribute)
				{
					PermissionSet permissionSet2 = ((PermissionSetAttribute)attrs[i]).CreatePermissionSet();
					if (permissionSet2 == null)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_UnableToGeneratePermissionSet"));
					}
					PermissionSetEnumeratorInternal permissionSetEnumeratorInternal = new PermissionSetEnumeratorInternal(permissionSet2);
					while (permissionSetEnumeratorInternal.MoveNext())
					{
						object obj = permissionSetEnumeratorInternal.Current;
						IPermission permission = (IPermission)obj;
						PermissionSet.MergePermission(permission, serialize, ref casPset, ref permissionSet);
					}
					if (casPset == null)
					{
						casPset = new PermissionSet(false);
					}
					if (permissionSet2.IsUnrestricted())
					{
						casPset.SetUnrestricted(true);
					}
				}
				else
				{
					IPermission permission2 = ((SecurityAttribute)attrs[i]).CreatePermission();
					PermissionSet.MergePermission(permission2, serialize, ref casPset, ref permissionSet);
				}
			}
			if (casPset != null)
			{
				casPset.FilterHostProtectionPermissions(fullTrustOnlyResources, HostProtectionResource.None);
				casPset.ContainsNonCodeAccessPermissions();
			}
			if (permissionSet != null)
			{
				permissionSet.FilterHostProtectionPermissions(fullTrustOnlyResources, HostProtectionResource.None);
				permissionSet.ContainsNonCodeAccessPermissions();
			}
			byte[] array = null;
			nonCasBlob = null;
			if (serialize)
			{
				if (casPset != null)
				{
					array = casPset.EncodeXml();
				}
				if (permissionSet != null)
				{
					nonCasBlob = permissionSet.EncodeXml();
				}
			}
			return array;
		}

		// Token: 0x06003B75 RID: 15221 RVA: 0x000CBC5D File Offset: 0x000CAC5D
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			this.NormalizePermissionSet();
			this.m_CheckedForNonCas = false;
		}

		// Token: 0x06003B76 RID: 15222 RVA: 0x000CBC6C File Offset: 0x000CAC6C
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void RevertAssert()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			SecurityRuntime.RevertAssert(ref stackCrawlMark);
		}

		// Token: 0x06003B77 RID: 15223 RVA: 0x000CBC84 File Offset: 0x000CAC84
		internal static PermissionSet RemoveRefusedPermissionSet(PermissionSet assertSet, PermissionSet refusedSet, out bool bFailedToCompress)
		{
			PermissionSet permissionSet = null;
			bFailedToCompress = false;
			if (assertSet == null)
			{
				return null;
			}
			if (refusedSet != null)
			{
				if (refusedSet.IsUnrestricted())
				{
					return null;
				}
				PermissionSetEnumeratorInternal permissionSetEnumeratorInternal = new PermissionSetEnumeratorInternal(refusedSet);
				while (permissionSetEnumeratorInternal.MoveNext())
				{
					object obj = permissionSetEnumeratorInternal.Current;
					CodeAccessPermission codeAccessPermission = (CodeAccessPermission)obj;
					int currentIndex = permissionSetEnumeratorInternal.GetCurrentIndex();
					if (codeAccessPermission != null)
					{
						CodeAccessPermission codeAccessPermission2 = (CodeAccessPermission)assertSet.GetPermission(currentIndex);
						try
						{
							if (codeAccessPermission.Intersect(codeAccessPermission2) != null)
							{
								if (!codeAccessPermission.Equals(codeAccessPermission2))
								{
									bFailedToCompress = true;
									return assertSet;
								}
								if (permissionSet == null)
								{
									permissionSet = assertSet.Copy();
								}
								permissionSet.RemovePermission(currentIndex);
							}
						}
						catch (ArgumentException)
						{
							if (permissionSet == null)
							{
								permissionSet = assertSet.Copy();
							}
							permissionSet.RemovePermission(currentIndex);
						}
						continue;
					}
				}
			}
			if (permissionSet != null)
			{
				return permissionSet;
			}
			return assertSet;
		}

		// Token: 0x06003B78 RID: 15224 RVA: 0x000CBD44 File Offset: 0x000CAD44
		internal static void RemoveAssertedPermissionSet(PermissionSet demandSet, PermissionSet assertSet, out PermissionSet alteredDemandSet)
		{
			alteredDemandSet = null;
			PermissionSetEnumeratorInternal permissionSetEnumeratorInternal = new PermissionSetEnumeratorInternal(demandSet);
			while (permissionSetEnumeratorInternal.MoveNext())
			{
				object obj = permissionSetEnumeratorInternal.Current;
				CodeAccessPermission codeAccessPermission = (CodeAccessPermission)obj;
				int currentIndex = permissionSetEnumeratorInternal.GetCurrentIndex();
				if (codeAccessPermission != null)
				{
					CodeAccessPermission codeAccessPermission2 = (CodeAccessPermission)assertSet.GetPermission(currentIndex);
					try
					{
						if (codeAccessPermission.CheckAssert(codeAccessPermission2))
						{
							if (alteredDemandSet == null)
							{
								alteredDemandSet = demandSet.Copy();
							}
							alteredDemandSet.RemovePermission(currentIndex);
						}
					}
					catch (ArgumentException)
					{
					}
				}
			}
		}

		// Token: 0x06003B79 RID: 15225 RVA: 0x000CBDC0 File Offset: 0x000CADC0
		internal static bool IsIntersectingAssertedPermissions(PermissionSet assertSet1, PermissionSet assertSet2)
		{
			bool flag = false;
			if (assertSet1 != null && assertSet2 != null)
			{
				PermissionSetEnumeratorInternal permissionSetEnumeratorInternal = new PermissionSetEnumeratorInternal(assertSet2);
				while (permissionSetEnumeratorInternal.MoveNext())
				{
					object obj = permissionSetEnumeratorInternal.Current;
					CodeAccessPermission codeAccessPermission = (CodeAccessPermission)obj;
					int currentIndex = permissionSetEnumeratorInternal.GetCurrentIndex();
					if (codeAccessPermission != null)
					{
						CodeAccessPermission codeAccessPermission2 = (CodeAccessPermission)assertSet1.GetPermission(currentIndex);
						try
						{
							if (codeAccessPermission2 != null && !codeAccessPermission2.Equals(codeAccessPermission))
							{
								flag = true;
							}
						}
						catch (ArgumentException)
						{
							flag = true;
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x170009FA RID: 2554
		// (set) Token: 0x06003B7A RID: 15226 RVA: 0x000CBE38 File Offset: 0x000CAE38
		internal bool IgnoreTypeLoadFailures
		{
			set
			{
				this.m_ignoreTypeLoadFailures = value;
			}
		}

		// Token: 0x04001E6F RID: 7791
		private const string s_str_PermissionSet = "PermissionSet";

		// Token: 0x04001E70 RID: 7792
		private const string s_str_Permission = "Permission";

		// Token: 0x04001E71 RID: 7793
		private const string s_str_IPermission = "IPermission";

		// Token: 0x04001E72 RID: 7794
		private const string s_str_Unrestricted = "Unrestricted";

		// Token: 0x04001E73 RID: 7795
		private const string s_str_PermissionUnion = "PermissionUnion";

		// Token: 0x04001E74 RID: 7796
		private const string s_str_PermissionIntersection = "PermissionIntersection";

		// Token: 0x04001E75 RID: 7797
		private const string s_str_PermissionUnrestrictedUnion = "PermissionUnrestrictedUnion";

		// Token: 0x04001E76 RID: 7798
		private const string s_str_PermissionUnrestrictedIntersection = "PermissionUnrestrictedIntersection";

		// Token: 0x04001E77 RID: 7799
		private bool m_Unrestricted;

		// Token: 0x04001E78 RID: 7800
		[OptionalField(VersionAdded = 2)]
		private bool m_allPermissionsDecoded;

		// Token: 0x04001E79 RID: 7801
		[OptionalField(VersionAdded = 2)]
		private bool m_canUnrestrictedOverride;

		// Token: 0x04001E7A RID: 7802
		[OptionalField(VersionAdded = 2)]
		internal TokenBasedSet m_permSet;

		// Token: 0x04001E7B RID: 7803
		[OptionalField(VersionAdded = 2)]
		private bool m_ignoreTypeLoadFailures;

		// Token: 0x04001E7C RID: 7804
		[OptionalField(VersionAdded = 2)]
		private string m_serializedPermissionSet;

		// Token: 0x04001E7D RID: 7805
		[NonSerialized]
		private bool m_CheckedForNonCas;

		// Token: 0x04001E7E RID: 7806
		[NonSerialized]
		private bool m_ContainsCas;

		// Token: 0x04001E7F RID: 7807
		[NonSerialized]
		private bool m_ContainsNonCas;

		// Token: 0x04001E80 RID: 7808
		[NonSerialized]
		private TokenBasedSet m_permSetSaved;

		// Token: 0x04001E81 RID: 7809
		private bool readableonly;

		// Token: 0x04001E82 RID: 7810
		private TokenBasedSet m_unrestrictedPermSet;

		// Token: 0x04001E83 RID: 7811
		private TokenBasedSet m_normalPermSet;

		// Token: 0x04001E84 RID: 7812
		internal static readonly PermissionSet s_fullTrust = new PermissionSet(true);

		// Token: 0x0200065F RID: 1631
		internal enum IsSubsetOfType
		{
			// Token: 0x04001E86 RID: 7814
			Normal,
			// Token: 0x04001E87 RID: 7815
			CheckDemand,
			// Token: 0x04001E88 RID: 7816
			CheckPermitOnly,
			// Token: 0x04001E89 RID: 7817
			CheckAssertion
		}
	}
}
