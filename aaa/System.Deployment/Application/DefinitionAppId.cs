using System;
using System.Deployment.Internal.Isolation;
using System.Runtime.InteropServices;

namespace System.Deployment.Application
{
	// Token: 0x0200000E RID: 14
	internal class DefinitionAppId
	{
		// Token: 0x06000072 RID: 114 RVA: 0x00004D6E File Offset: 0x00003D6E
		public DefinitionAppId()
		{
			this._idComPtr = IsolationInterop.AppIdAuthority.CreateDefinition();
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00004D86 File Offset: 0x00003D86
		public DefinitionAppId(params DefinitionIdentity[] idPath)
			: this(null, idPath)
		{
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00004D90 File Offset: 0x00003D90
		public DefinitionAppId(string codebase, params DefinitionIdentity[] idPath)
		{
			uint num = (uint)idPath.Length;
			IDefinitionIdentity[] array = new IDefinitionIdentity[num];
			for (uint num2 = 0U; num2 < num; num2 += 1U)
			{
				array[(int)((UIntPtr)num2)] = idPath[(int)((UIntPtr)num2)].ComPointer;
			}
			this._idComPtr = IsolationInterop.AppIdAuthority.CreateDefinition();
			this._idComPtr.put_Codebase(codebase);
			this._idComPtr.SetAppPath(num, array);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00004DF1 File Offset: 0x00003DF1
		public DefinitionAppId(string text)
		{
			this._idComPtr = IsolationInterop.AppIdAuthority.TextToDefinition(0U, text);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00004E0B File Offset: 0x00003E0B
		public DefinitionAppId(IDefinitionAppId idComPtr)
		{
			this._idComPtr = idComPtr;
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00004E1A File Offset: 0x00003E1A
		public ulong Hash
		{
			get
			{
				return IsolationInterop.AppIdAuthority.HashDefinition(0U, this._idComPtr);
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00004E2D File Offset: 0x00003E2D
		public IDefinitionAppId ComPointer
		{
			get
			{
				return this._idComPtr;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00004E35 File Offset: 0x00003E35
		public string Codebase
		{
			get
			{
				return this._idComPtr.get_Codebase();
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00004E42 File Offset: 0x00003E42
		public DefinitionIdentity DeploymentIdentity
		{
			get
			{
				return this.PathComponent(0U);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00004E4B File Offset: 0x00003E4B
		public DefinitionIdentity ApplicationIdentity
		{
			get
			{
				return this.PathComponent(1U);
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00004E54 File Offset: 0x00003E54
		public DefinitionAppId ToDeploymentAppId()
		{
			return new DefinitionAppId(this.Codebase, new DefinitionIdentity[] { this.DeploymentIdentity });
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00004E7D File Offset: 0x00003E7D
		public ApplicationIdentity ToApplicationIdentity()
		{
			return new ApplicationIdentity(IsolationInterop.AppIdAuthority.DefinitionToText(0U, this._idComPtr));
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00004E95 File Offset: 0x00003E95
		public override bool Equals(object obj)
		{
			return obj is DefinitionAppId && IsolationInterop.AppIdAuthority.AreDefinitionsEqual(0U, this.ComPointer, ((DefinitionAppId)obj).ComPointer);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00004EBD File Offset: 0x00003EBD
		public override int GetHashCode()
		{
			return (int)this.Hash;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00004EC6 File Offset: 0x00003EC6
		public override string ToString()
		{
			return IsolationInterop.AppIdAuthority.DefinitionToText(0U, this._idComPtr);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00004EDC File Offset: 0x00003EDC
		private DefinitionIdentity PathComponent(uint index)
		{
			IEnumDefinitionIdentity enumDefinitionIdentity = null;
			DefinitionIdentity definitionIdentity;
			try
			{
				enumDefinitionIdentity = this._idComPtr.EnumAppPath();
				if (index > 0U)
				{
					enumDefinitionIdentity.Skip(index);
				}
				IDefinitionIdentity[] array = new IDefinitionIdentity[1];
				uint num = enumDefinitionIdentity.Next(1U, array);
				definitionIdentity = ((num == 1U) ? new DefinitionIdentity(array[0]) : null);
			}
			finally
			{
				if (enumDefinitionIdentity != null)
				{
					Marshal.ReleaseComObject(enumDefinitionIdentity);
				}
			}
			return definitionIdentity;
		}

		// Token: 0x04000048 RID: 72
		private IDefinitionAppId _idComPtr;
	}
}
