using System;
using System.Threading;

namespace System.Reflection.Emit
{
	// Token: 0x02000802 RID: 2050
	internal class DynamicResolver : Resolver
	{
		// Token: 0x0600494B RID: 18763 RVA: 0x000FFFE4 File Offset: 0x000FEFE4
		internal DynamicResolver(DynamicILGenerator ilGenerator)
		{
			this.m_stackSize = ilGenerator.GetMaxStackSize();
			this.m_exceptions = ilGenerator.GetExceptions();
			this.m_code = ilGenerator.BakeByteArray();
			this.m_localSignature = ilGenerator.m_localSignature.InternalGetSignatureArray();
			this.m_scope = ilGenerator.m_scope;
			this.m_method = (DynamicMethod)ilGenerator.m_methodBuilder;
			this.m_method.m_resolver = this;
		}

		// Token: 0x0600494C RID: 18764 RVA: 0x00100058 File Offset: 0x000FF058
		internal DynamicResolver(DynamicILInfo dynamicILInfo)
		{
			this.m_stackSize = dynamicILInfo.MaxStackSize;
			this.m_code = dynamicILInfo.Code;
			this.m_localSignature = dynamicILInfo.LocalSignature;
			this.m_exceptionHeader = dynamicILInfo.Exceptions;
			this.m_scope = dynamicILInfo.DynamicScope;
			this.m_method = dynamicILInfo.DynamicMethod;
			this.m_method.m_resolver = this;
		}

		// Token: 0x0600494D RID: 18765 RVA: 0x001000C0 File Offset: 0x000FF0C0
		protected override void Finalize()
		{
			try
			{
				DynamicMethod method = this.m_method;
				if (method != null)
				{
					if (!method.m_method.IsNullHandle())
					{
						DynamicResolver.DestroyScout destroyScout = null;
						try
						{
							destroyScout = new DynamicResolver.DestroyScout();
						}
						catch
						{
							if (!Environment.HasShutdownStarted && !AppDomain.CurrentDomain.IsFinalizingForUnload())
							{
								GC.ReRegisterForFinalize(this);
							}
							return;
						}
						destroyScout.m_method = method.m_method;
					}
				}
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x0600494E RID: 18766 RVA: 0x00100140 File Offset: 0x000FF140
		internal override void GetJitContext(ref int securityControlFlags, ref RuntimeTypeHandle typeOwner)
		{
			DynamicResolver.SecurityControlFlags securityControlFlags2 = DynamicResolver.SecurityControlFlags.Default;
			if (this.m_method.m_restrictedSkipVisibility)
			{
				securityControlFlags2 |= DynamicResolver.SecurityControlFlags.RestrictedSkipVisibilityChecks;
			}
			else if (this.m_method.m_skipVisibility)
			{
				securityControlFlags2 |= DynamicResolver.SecurityControlFlags.SkipVisibilityChecks;
			}
			typeOwner = ((this.m_method.m_typeOwner != null) ? this.m_method.m_typeOwner.TypeHandle : RuntimeTypeHandle.EmptyHandle);
			if (this.m_method.m_creationContext != null)
			{
				securityControlFlags2 |= DynamicResolver.SecurityControlFlags.HasCreationContext;
				if (this.m_method.m_creationContext.CanSkipEvaluation)
				{
					securityControlFlags2 |= DynamicResolver.SecurityControlFlags.CanSkipCSEvaluation;
				}
			}
			securityControlFlags = (int)securityControlFlags2;
		}

		// Token: 0x0600494F RID: 18767 RVA: 0x001001C8 File Offset: 0x000FF1C8
		internal override byte[] GetCodeInfo(ref int stackSize, ref int initLocals, ref int EHCount)
		{
			stackSize = this.m_stackSize;
			if (this.m_exceptionHeader != null && this.m_exceptionHeader.Length != 0)
			{
				if (this.m_exceptionHeader.Length < 4)
				{
					throw new FormatException();
				}
				byte b = this.m_exceptionHeader[0];
				if ((b & 64) != 0)
				{
					byte[] array = new byte[4];
					for (int i = 0; i < 3; i++)
					{
						array[i] = this.m_exceptionHeader[i + 1];
					}
					EHCount = (BitConverter.ToInt32(array, 0) - 4) / 24;
				}
				else
				{
					EHCount = (int)((this.m_exceptionHeader[1] - 2) / 12);
				}
			}
			else
			{
				EHCount = ILGenerator.CalculateNumberOfExceptions(this.m_exceptions);
			}
			initLocals = (this.m_method.InitLocals ? 1 : 0);
			return this.m_code;
		}

		// Token: 0x06004950 RID: 18768 RVA: 0x00100276 File Offset: 0x000FF276
		internal override byte[] GetLocalsSignature()
		{
			return this.m_localSignature;
		}

		// Token: 0x06004951 RID: 18769 RVA: 0x0010027E File Offset: 0x000FF27E
		internal override byte[] GetRawEHInfo()
		{
			return this.m_exceptionHeader;
		}

		// Token: 0x06004952 RID: 18770 RVA: 0x00100288 File Offset: 0x000FF288
		internal unsafe override void GetEHInfo(int excNumber, void* exc)
		{
			for (int i = 0; i < this.m_exceptions.Length; i++)
			{
				int numberOfCatches = this.m_exceptions[i].GetNumberOfCatches();
				if (excNumber < numberOfCatches)
				{
					((Resolver.CORINFO_EH_CLAUSE*)exc)->Flags = this.m_exceptions[i].GetExceptionTypes()[excNumber];
					((Resolver.CORINFO_EH_CLAUSE*)exc)->Flags = ((Resolver.CORINFO_EH_CLAUSE*)exc)->Flags | 536870912;
					((Resolver.CORINFO_EH_CLAUSE*)exc)->TryOffset = this.m_exceptions[i].GetStartAddress();
					if ((((Resolver.CORINFO_EH_CLAUSE*)exc)->Flags & 2) != 2)
					{
						((Resolver.CORINFO_EH_CLAUSE*)exc)->TryLength = this.m_exceptions[i].GetEndAddress() - ((Resolver.CORINFO_EH_CLAUSE*)exc)->TryOffset;
					}
					else
					{
						((Resolver.CORINFO_EH_CLAUSE*)exc)->TryLength = this.m_exceptions[i].GetFinallyEndAddress() - ((Resolver.CORINFO_EH_CLAUSE*)exc)->TryOffset;
					}
					((Resolver.CORINFO_EH_CLAUSE*)exc)->HandlerOffset = this.m_exceptions[i].GetCatchAddresses()[excNumber];
					((Resolver.CORINFO_EH_CLAUSE*)exc)->HandlerLength = this.m_exceptions[i].GetCatchEndAddresses()[excNumber] - ((Resolver.CORINFO_EH_CLAUSE*)exc)->HandlerOffset;
					((Resolver.CORINFO_EH_CLAUSE*)exc)->ClassTokenOrFilterOffset = this.m_exceptions[i].GetFilterAddresses()[excNumber];
					return;
				}
				excNumber -= numberOfCatches;
			}
		}

		// Token: 0x06004953 RID: 18771 RVA: 0x0010038C File Offset: 0x000FF38C
		internal override string GetStringLiteral(int token)
		{
			return this.m_scope.GetString(token);
		}

		// Token: 0x06004954 RID: 18772 RVA: 0x0010039C File Offset: 0x000FF39C
		private int GetMethodToken()
		{
			if (this.IsValidToken(this.m_methodToken) == 0)
			{
				int tokenFor = this.m_scope.GetTokenFor(this.m_method.GetMethodDescriptor());
				Interlocked.CompareExchange(ref this.m_methodToken, tokenFor, 0);
			}
			return this.m_methodToken;
		}

		// Token: 0x06004955 RID: 18773 RVA: 0x001003E2 File Offset: 0x000FF3E2
		internal override int IsValidToken(int token)
		{
			if (this.m_scope[token] == null)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06004956 RID: 18774 RVA: 0x001003F5 File Offset: 0x000FF3F5
		internal override CompressedStack GetSecurityContext()
		{
			return this.m_method.m_creationContext;
		}

		// Token: 0x06004957 RID: 18775 RVA: 0x00100404 File Offset: 0x000FF404
		internal unsafe override void* ResolveToken(int token)
		{
			object obj = this.m_scope[token];
			if (obj is RuntimeTypeHandle)
			{
				return (void*)((RuntimeTypeHandle)obj).Value;
			}
			if (obj is RuntimeMethodHandle)
			{
				return (void*)((RuntimeMethodHandle)obj).Value;
			}
			if (obj is RuntimeFieldHandle)
			{
				return (void*)((RuntimeFieldHandle)obj).Value;
			}
			if (obj is DynamicMethod)
			{
				DynamicMethod dynamicMethod = (DynamicMethod)obj;
				return (void*)dynamicMethod.GetMethodDescriptor().Value;
			}
			if (obj is GenericMethodInfo)
			{
				GenericMethodInfo genericMethodInfo = (GenericMethodInfo)obj;
				return (void*)genericMethodInfo.m_method.Value;
			}
			if (obj is GenericFieldInfo)
			{
				GenericFieldInfo genericFieldInfo = (GenericFieldInfo)obj;
				return (void*)genericFieldInfo.m_field.Value;
			}
			if (!(obj is VarArgMethod))
			{
				return null;
			}
			VarArgMethod varArgMethod = (VarArgMethod)obj;
			DynamicMethod dynamicMethod2 = varArgMethod.m_method as DynamicMethod;
			if (dynamicMethod2 == null)
			{
				return (void*)varArgMethod.m_method.MethodHandle.Value;
			}
			return (void*)dynamicMethod2.GetMethodDescriptor().Value;
		}

		// Token: 0x06004958 RID: 18776 RVA: 0x00100531 File Offset: 0x000FF531
		internal override byte[] ResolveSignature(int token, int fromMethod)
		{
			return this.m_scope.ResolveSignature(token, fromMethod);
		}

		// Token: 0x06004959 RID: 18777 RVA: 0x00100540 File Offset: 0x000FF540
		internal override int ParentToken(int token)
		{
			RuntimeTypeHandle runtimeTypeHandle = RuntimeTypeHandle.EmptyHandle;
			object obj = this.m_scope[token];
			if (obj is RuntimeMethodHandle)
			{
				runtimeTypeHandle = ((RuntimeMethodHandle)obj).GetDeclaringType();
			}
			else if (obj is RuntimeFieldHandle)
			{
				runtimeTypeHandle = ((RuntimeFieldHandle)obj).GetApproxDeclaringType();
			}
			else if (obj is DynamicMethod)
			{
				DynamicMethod dynamicMethod = (DynamicMethod)obj;
				runtimeTypeHandle = dynamicMethod.m_method.GetDeclaringType();
			}
			else if (obj is GenericMethodInfo)
			{
				GenericMethodInfo genericMethodInfo = (GenericMethodInfo)obj;
				runtimeTypeHandle = genericMethodInfo.m_context;
			}
			else if (obj is GenericFieldInfo)
			{
				GenericFieldInfo genericFieldInfo = (GenericFieldInfo)obj;
				runtimeTypeHandle = genericFieldInfo.m_context;
			}
			else if (obj is VarArgMethod)
			{
				VarArgMethod varArgMethod = (VarArgMethod)obj;
				DynamicMethod dynamicMethod2 = varArgMethod.m_method as DynamicMethod;
				if (dynamicMethod2 != null)
				{
					runtimeTypeHandle = dynamicMethod2.GetMethodDescriptor().GetDeclaringType();
				}
				else if (varArgMethod.m_method.DeclaringType == null)
				{
					runtimeTypeHandle = varArgMethod.m_method.MethodHandle.GetDeclaringType();
				}
				else
				{
					runtimeTypeHandle = varArgMethod.m_method.DeclaringType.TypeHandle;
				}
			}
			if (runtimeTypeHandle.IsNullHandle())
			{
				return -1;
			}
			return this.m_scope.GetTokenFor(runtimeTypeHandle);
		}

		// Token: 0x0600495A RID: 18778 RVA: 0x00100675 File Offset: 0x000FF675
		internal override MethodInfo GetDynamicMethod()
		{
			return this.m_method.GetMethodInfo();
		}

		// Token: 0x0400255E RID: 9566
		private __ExceptionInfo[] m_exceptions;

		// Token: 0x0400255F RID: 9567
		private byte[] m_exceptionHeader;

		// Token: 0x04002560 RID: 9568
		private DynamicMethod m_method;

		// Token: 0x04002561 RID: 9569
		private byte[] m_code;

		// Token: 0x04002562 RID: 9570
		private byte[] m_localSignature;

		// Token: 0x04002563 RID: 9571
		private int m_stackSize;

		// Token: 0x04002564 RID: 9572
		private DynamicScope m_scope;

		// Token: 0x04002565 RID: 9573
		private int m_methodToken;

		// Token: 0x02000803 RID: 2051
		private class DestroyScout
		{
			// Token: 0x0600495B RID: 18779 RVA: 0x00100684 File Offset: 0x000FF684
			~DestroyScout()
			{
				if (!this.m_method.IsNullHandle())
				{
					if (this.m_method.GetResolver() != null)
					{
						if (!Environment.HasShutdownStarted && !AppDomain.CurrentDomain.IsFinalizingForUnload())
						{
							GC.ReRegisterForFinalize(this);
						}
					}
					else
					{
						this.m_method.Destroy();
					}
				}
			}

			// Token: 0x04002566 RID: 9574
			internal RuntimeMethodHandle m_method;
		}

		// Token: 0x02000804 RID: 2052
		[Flags]
		internal enum SecurityControlFlags
		{
			// Token: 0x04002568 RID: 9576
			Default = 0,
			// Token: 0x04002569 RID: 9577
			SkipVisibilityChecks = 1,
			// Token: 0x0400256A RID: 9578
			RestrictedSkipVisibilityChecks = 2,
			// Token: 0x0400256B RID: 9579
			HasCreationContext = 4,
			// Token: 0x0400256C RID: 9580
			CanSkipCSEvaluation = 8
		}
	}
}
