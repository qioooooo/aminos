using System;
using System.Collections;
using System.Deployment.Internal.Isolation;
using System.Deployment.Internal.Isolation.Manifest;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System
{
	// Token: 0x02000065 RID: 101
	[ComVisible(false)]
	[Serializable]
	public sealed class ActivationContext : IDisposable, ISerializable
	{
		// Token: 0x0600060D RID: 1549 RVA: 0x00014F1A File Offset: 0x00013F1A
		private ActivationContext()
		{
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00014F24 File Offset: 0x00013F24
		private ActivationContext(SerializationInfo info, StreamingContext context)
		{
			string text = (string)info.GetValue("FullName", typeof(string));
			string[] array = (string[])info.GetValue("ManifestPaths", typeof(string[]));
			if (array == null)
			{
				this.CreateFromName(new ApplicationIdentity(text));
				return;
			}
			this.CreateFromNameAndManifests(new ApplicationIdentity(text), array);
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00014F8A File Offset: 0x00013F8A
		internal ActivationContext(ApplicationIdentity applicationIdentity)
		{
			this.CreateFromName(applicationIdentity);
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x00014F99 File Offset: 0x00013F99
		internal ActivationContext(ApplicationIdentity applicationIdentity, string[] manifestPaths)
		{
			this.CreateFromNameAndManifests(applicationIdentity, manifestPaths);
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00014FAC File Offset: 0x00013FAC
		private void CreateFromName(ApplicationIdentity applicationIdentity)
		{
			if (applicationIdentity == null)
			{
				throw new ArgumentNullException("applicationIdentity");
			}
			this._applicationIdentity = applicationIdentity;
			IEnumDefinitionIdentity enumDefinitionIdentity = this._applicationIdentity.Identity.EnumAppPath();
			this._definitionIdentities = new ArrayList(2);
			IDefinitionIdentity[] array = new IDefinitionIdentity[1];
			while (enumDefinitionIdentity.Next(1U, array) == 1U)
			{
				this._definitionIdentities.Add(array[0]);
			}
			this._definitionIdentities.TrimToSize();
			if (this._definitionIdentities.Count <= 1)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidAppId"));
			}
			this._manifestPaths = null;
			this._manifests = null;
			this._actContext = IsolationInterop.CreateActContext(this._applicationIdentity.Identity);
			this._form = ActivationContext.ContextForm.StoreBounded;
			this._appRunState = ActivationContext.ApplicationStateDisposition.Undefined;
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x0001506C File Offset: 0x0001406C
		private void CreateFromNameAndManifests(ApplicationIdentity applicationIdentity, string[] manifestPaths)
		{
			if (applicationIdentity == null)
			{
				throw new ArgumentNullException("applicationIdentity");
			}
			if (manifestPaths == null)
			{
				throw new ArgumentNullException("manifestPaths");
			}
			this._applicationIdentity = applicationIdentity;
			IEnumDefinitionIdentity enumDefinitionIdentity = this._applicationIdentity.Identity.EnumAppPath();
			this._manifests = new ArrayList(2);
			this._manifestPaths = new string[manifestPaths.Length];
			IDefinitionIdentity[] array = new IDefinitionIdentity[1];
			int num = 0;
			while (enumDefinitionIdentity.Next(1U, array) == 1U)
			{
				ICMS icms = (ICMS)IsolationInterop.ParseManifest(manifestPaths[num], null, ref IsolationInterop.IID_ICMS);
				if (!IsolationInterop.IdentityAuthority.AreDefinitionsEqual(0U, icms.Identity, array[0]))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_IllegalAppIdMismatch"));
				}
				this._manifests.Add(icms);
				this._manifestPaths[num] = manifestPaths[num];
				num++;
			}
			if (num != manifestPaths.Length)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_IllegalAppId"));
			}
			this._manifests.TrimToSize();
			if (this._manifests.Count <= 1)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidAppId"));
			}
			this._definitionIdentities = null;
			this._actContext = null;
			this._form = ActivationContext.ContextForm.Loose;
			this._appRunState = ActivationContext.ApplicationStateDisposition.Undefined;
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00015190 File Offset: 0x00014190
		~ActivationContext()
		{
			this.Dispose(false);
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x000151C0 File Offset: 0x000141C0
		public static ActivationContext CreatePartialActivationContext(ApplicationIdentity identity)
		{
			return new ActivationContext(identity);
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x000151C8 File Offset: 0x000141C8
		public static ActivationContext CreatePartialActivationContext(ApplicationIdentity identity, string[] manifestPaths)
		{
			return new ActivationContext(identity, manifestPaths);
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000616 RID: 1558 RVA: 0x000151D1 File Offset: 0x000141D1
		public ApplicationIdentity Identity
		{
			get
			{
				return this._applicationIdentity;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000617 RID: 1559 RVA: 0x000151D9 File Offset: 0x000141D9
		public ActivationContext.ContextForm Form
		{
			get
			{
				return this._form;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000618 RID: 1560 RVA: 0x000151E1 File Offset: 0x000141E1
		public byte[] ApplicationManifestBytes
		{
			get
			{
				return this.GetApplicationManifestBytes();
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000619 RID: 1561 RVA: 0x000151E9 File Offset: 0x000141E9
		public byte[] DeploymentManifestBytes
		{
			get
			{
				return this.GetDeploymentManifestBytes();
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600061A RID: 1562 RVA: 0x000151F1 File Offset: 0x000141F1
		internal string[] ManifestPaths
		{
			get
			{
				return this._manifestPaths;
			}
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x000151F9 File Offset: 0x000141F9
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x00015208 File Offset: 0x00014208
		internal string ApplicationDirectory
		{
			get
			{
				if (this._form == ActivationContext.ContextForm.Loose)
				{
					return Path.GetDirectoryName(this._manifestPaths[this._manifestPaths.Length - 1]);
				}
				string text;
				this._actContext.ApplicationBasePath(0U, out text);
				return text;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600061D RID: 1565 RVA: 0x00015244 File Offset: 0x00014244
		internal string DataDirectory
		{
			get
			{
				if (this._form == ActivationContext.ContextForm.Loose)
				{
					return null;
				}
				string text;
				this._actContext.GetApplicationStateFilesystemLocation(1U, UIntPtr.Zero, IntPtr.Zero, out text);
				return text;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x00015274 File Offset: 0x00014274
		internal ICMS ActivationContextData
		{
			get
			{
				return this.ApplicationComponentManifest;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x0001527C File Offset: 0x0001427C
		internal ICMS DeploymentComponentManifest
		{
			get
			{
				if (this._form == ActivationContext.ContextForm.Loose)
				{
					return (ICMS)this._manifests[0];
				}
				return this.GetComponentManifest((IDefinitionIdentity)this._definitionIdentities[0]);
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x000152B0 File Offset: 0x000142B0
		internal ICMS ApplicationComponentManifest
		{
			get
			{
				if (this._form == ActivationContext.ContextForm.Loose)
				{
					return (ICMS)this._manifests[this._manifests.Count - 1];
				}
				return this.GetComponentManifest((IDefinitionIdentity)this._definitionIdentities[this._definitionIdentities.Count - 1]);
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000621 RID: 1569 RVA: 0x00015306 File Offset: 0x00014306
		internal ActivationContext.ApplicationStateDisposition LastApplicationStateResult
		{
			get
			{
				return this._appRunState;
			}
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00015310 File Offset: 0x00014310
		internal ICMS GetComponentManifest(IDefinitionIdentity component)
		{
			object obj;
			this._actContext.GetComponentManifest(0U, component, ref IsolationInterop.IID_ICMS, out obj);
			return obj as ICMS;
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00015338 File Offset: 0x00014338
		internal byte[] GetDeploymentManifestBytes()
		{
			string text;
			if (this._form == ActivationContext.ContextForm.Loose)
			{
				text = this._manifestPaths[0];
			}
			else
			{
				object obj;
				this._actContext.GetComponentManifest(0U, (IDefinitionIdentity)this._definitionIdentities[0], ref IsolationInterop.IID_IManifestInformation, out obj);
				((IManifestInformation)obj).get_FullPath(out text);
			}
			return ActivationContext.ReadBytesFromFile(text);
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00015390 File Offset: 0x00014390
		internal byte[] GetApplicationManifestBytes()
		{
			string text;
			if (this._form == ActivationContext.ContextForm.Loose)
			{
				text = this._manifestPaths[this._manifests.Count - 1];
			}
			else
			{
				object obj;
				this._actContext.GetComponentManifest(0U, (IDefinitionIdentity)this._definitionIdentities[1], ref IsolationInterop.IID_IManifestInformation, out obj);
				((IManifestInformation)obj).get_FullPath(out text);
			}
			return ActivationContext.ReadBytesFromFile(text);
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x000153F3 File Offset: 0x000143F3
		internal void PrepareForExecution()
		{
			if (this._form == ActivationContext.ContextForm.Loose)
			{
				return;
			}
			this._actContext.PrepareForExecution(IntPtr.Zero, IntPtr.Zero);
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00015414 File Offset: 0x00014414
		internal ActivationContext.ApplicationStateDisposition SetApplicationState(ActivationContext.ApplicationState s)
		{
			if (this._form == ActivationContext.ContextForm.Loose)
			{
				return ActivationContext.ApplicationStateDisposition.Undefined;
			}
			uint num;
			this._actContext.SetApplicationRunningState(0U, (uint)s, out num);
			this._appRunState = (ActivationContext.ApplicationStateDisposition)num;
			return this._appRunState;
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00015447 File Offset: 0x00014447
		private void Dispose(bool fDisposing)
		{
			this._applicationIdentity = null;
			this._definitionIdentities = null;
			this._manifests = null;
			this._manifestPaths = null;
			if (this._actContext != null)
			{
				Marshal.ReleaseComObject(this._actContext);
			}
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x0001547C File Offset: 0x0001447C
		private static byte[] ReadBytesFromFile(string manifestPath)
		{
			byte[] array = null;
			using (FileStream fileStream = new FileStream(manifestPath, FileMode.Open, FileAccess.Read))
			{
				int num = (int)fileStream.Length;
				array = new byte[num];
				if (fileStream.CanSeek)
				{
					fileStream.Seek(0L, SeekOrigin.Begin);
				}
				fileStream.Read(array, 0, num);
			}
			return array;
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x000154DC File Offset: 0x000144DC
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (this._applicationIdentity != null)
			{
				info.AddValue("FullName", this._applicationIdentity.FullName, typeof(string));
			}
			if (this._manifestPaths != null)
			{
				info.AddValue("ManifestPaths", this._manifestPaths, typeof(string[]));
			}
		}

		// Token: 0x040001DB RID: 475
		private const int DefaultComponentCount = 2;

		// Token: 0x040001DC RID: 476
		private ApplicationIdentity _applicationIdentity;

		// Token: 0x040001DD RID: 477
		private ArrayList _definitionIdentities;

		// Token: 0x040001DE RID: 478
		private ArrayList _manifests;

		// Token: 0x040001DF RID: 479
		private string[] _manifestPaths;

		// Token: 0x040001E0 RID: 480
		private ActivationContext.ContextForm _form;

		// Token: 0x040001E1 RID: 481
		private ActivationContext.ApplicationStateDisposition _appRunState;

		// Token: 0x040001E2 RID: 482
		private IActContext _actContext;

		// Token: 0x02000066 RID: 102
		public enum ContextForm
		{
			// Token: 0x040001E4 RID: 484
			Loose,
			// Token: 0x040001E5 RID: 485
			StoreBounded
		}

		// Token: 0x02000067 RID: 103
		internal enum ApplicationState
		{
			// Token: 0x040001E7 RID: 487
			Undefined,
			// Token: 0x040001E8 RID: 488
			Starting,
			// Token: 0x040001E9 RID: 489
			Running
		}

		// Token: 0x02000068 RID: 104
		internal enum ApplicationStateDisposition
		{
			// Token: 0x040001EB RID: 491
			Undefined,
			// Token: 0x040001EC RID: 492
			Starting,
			// Token: 0x040001ED RID: 493
			StartingMigrated = 65537,
			// Token: 0x040001EE RID: 494
			Running = 2,
			// Token: 0x040001EF RID: 495
			RunningFirstTime = 131074
		}
	}
}
