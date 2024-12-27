using System;
using System.Collections;
using System.Deployment.Application.Manifest;
using System.Deployment.Internal.Isolation.Manifest;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;

namespace System.Deployment.Application
{
	// Token: 0x02000033 RID: 51
	internal class ComponentVerifier
	{
		// Token: 0x060001B2 RID: 434 RVA: 0x0000BAE0 File Offset: 0x0000AAE0
		public void AddFileForVerification(string filePath, HashCollection verificationHashCollection)
		{
			ComponentVerifier.FileComponent fileComponent = new ComponentVerifier.FileComponent(filePath, verificationHashCollection);
			this._verificationComponents.Add(fileComponent);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000BB04 File Offset: 0x0000AB04
		public void AddSimplyNamedAssemblyForVerification(string filePath, AssemblyManifest assemblyManifest)
		{
			ComponentVerifier.SimplyNamedAssemblyComponent simplyNamedAssemblyComponent = new ComponentVerifier.SimplyNamedAssemblyComponent(filePath, assemblyManifest);
			this._verificationComponents.Add(simplyNamedAssemblyComponent);
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000BB28 File Offset: 0x0000AB28
		public void AddStrongNameAssemblyForVerification(string filePath, AssemblyManifest assemblyManifest)
		{
			ComponentVerifier.StrongNameAssemblyComponent strongNameAssemblyComponent = new ComponentVerifier.StrongNameAssemblyComponent(filePath, assemblyManifest);
			this._verificationComponents.Add(strongNameAssemblyComponent);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000BB4C File Offset: 0x0000AB4C
		public void VerifyComponents()
		{
			foreach (object obj in this._verificationComponents)
			{
				ComponentVerifier.VerificationComponent verificationComponent = (ComponentVerifier.VerificationComponent)obj;
				verificationComponent.Verify();
			}
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000BBA4 File Offset: 0x0000ABA4
		public static void VerifyFileHash(string filePath, HashCollection hashCollection)
		{
			string fileName = Path.GetFileName(filePath);
			if (hashCollection.Count == 0)
			{
				if (PolicyKeys.RequireHashInManifests())
				{
					throw new InvalidDeploymentException(ExceptionTypes.HashValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_HashNotSpecified"), new object[] { fileName }));
				}
				Logger.AddWarningInformation(string.Format(CultureInfo.CurrentUICulture, Resources.GetString("NoHashFile"), new object[] { fileName }));
			}
			foreach (Hash hash in hashCollection)
			{
				ComponentVerifier.VerifyFileHash(filePath, hash);
			}
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000BC60 File Offset: 0x0000AC60
		public static void VerifyFileHash(string filePath, Hash hash)
		{
			string fileName = Path.GetFileName(filePath);
			byte[] array;
			try
			{
				array = ComponentVerifier.GenerateDigestValue(filePath, hash.DigestMethod, hash.Transform);
			}
			catch (IOException ex)
			{
				throw new InvalidDeploymentException(ExceptionTypes.HashValidation, Resources.GetString("Ex_HashValidationException"), ex);
			}
			byte[] digestValue = hash.DigestValue;
			bool flag = false;
			if (array.Length == digestValue.Length)
			{
				int num = 0;
				while (num < digestValue.Length && digestValue[num] == array[num])
				{
					num++;
				}
				if (num >= digestValue.Length)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				throw new InvalidDeploymentException(ExceptionTypes.HashValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_DifferentHashes"), new object[] { fileName }));
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000BD14 File Offset: 0x0000AD14
		public static byte[] GenerateDigestValue(string filePath, CMS_HASH_DIGESTMETHOD digestMethod, CMS_HASH_TRANSFORM transform)
		{
			Stream stream = null;
			byte[] array = null;
			try
			{
				HashAlgorithm hashAlgorithm = ComponentVerifier.GetHashAlgorithm(digestMethod);
				stream = ComponentVerifier.GetTransformedStream(filePath, transform);
				array = hashAlgorithm.ComputeHash(stream);
			}
			finally
			{
				if (stream != null)
				{
					stream.Close();
				}
			}
			return array;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000BD5C File Offset: 0x0000AD5C
		public static bool IsVerifiableHashCollection(HashCollection hashCollection)
		{
			foreach (Hash hash in hashCollection)
			{
				if (!ComponentVerifier.IsVerifiableHash(hash))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000BDB4 File Offset: 0x0000ADB4
		public static bool IsVerifiableHash(Hash hash)
		{
			return Array.IndexOf<CMS_HASH_TRANSFORM>(ComponentVerifier.VerifiableTransformTypes, hash.Transform) >= 0 && Array.IndexOf<CMS_HASH_DIGESTMETHOD>(ComponentVerifier.VerifiableDigestMethods, hash.DigestMethod) >= 0 && hash.DigestValue != null && hash.DigestValue.Length > 0;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000BDF4 File Offset: 0x0000ADF4
		public static HashAlgorithm GetHashAlgorithm(CMS_HASH_DIGESTMETHOD digestMethod)
		{
			if (digestMethod == CMS_HASH_DIGESTMETHOD.CMS_HASH_DIGESTMETHOD_SHA1)
			{
				return new SHA1CryptoServiceProvider();
			}
			if (digestMethod == CMS_HASH_DIGESTMETHOD.CMS_HASH_DIGESTMETHOD_SHA256)
			{
				return new SHA256Managed();
			}
			if (digestMethod == CMS_HASH_DIGESTMETHOD.CMS_HASH_DIGESTMETHOD_SHA384)
			{
				return new SHA384Managed();
			}
			if (digestMethod == CMS_HASH_DIGESTMETHOD.CMS_HASH_DIGESTMETHOD_SHA512)
			{
				return new SHA512Managed();
			}
			throw new NotSupportedException(string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_DigestMethodNotSupported"), new object[] { digestMethod.ToString() }));
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000BE58 File Offset: 0x0000AE58
		public static Stream GetTransformedStream(string filePath, CMS_HASH_TRANSFORM transform)
		{
			Stream stream = null;
			if (transform == CMS_HASH_TRANSFORM.CMS_HASH_TRANSFORM_MANIFESTINVARIANT)
			{
				PEStream pestream = null;
				try
				{
					pestream = new PEStream(filePath, true);
					pestream.ZeroOutOptionalHeaderCheckSum();
					pestream.ZeroOutDefaultId1ManifestResource();
					return pestream;
				}
				finally
				{
					if (pestream != stream && pestream != null)
					{
						pestream.Close();
					}
				}
			}
			if (transform != CMS_HASH_TRANSFORM.CMS_HASH_TRANSFORM_IDENTITY)
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_TransformAlgorithmNotSupported"), new object[] { transform.ToString() }));
			}
			stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
			return stream;
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001BD RID: 445 RVA: 0x0000BEE4 File Offset: 0x0000AEE4
		public static CMS_HASH_TRANSFORM[] VerifiableTransformTypes
		{
			get
			{
				return ComponentVerifier._supportedTransforms;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001BE RID: 446 RVA: 0x0000BEEB File Offset: 0x0000AEEB
		public static CMS_HASH_DIGESTMETHOD[] VerifiableDigestMethods
		{
			get
			{
				return ComponentVerifier._supportedDigestMethods;
			}
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000BEF4 File Offset: 0x0000AEF4
		public static void VerifySimplyNamedAssembly(string filePath, AssemblyManifest assemblyManifest)
		{
			string fileName = Path.GetFileName(filePath);
			if (assemblyManifest.Identity.PublicKeyToken != null)
			{
				throw new InvalidDeploymentException(ExceptionTypes.Validation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_SimplyNamedAsmWithPKT"), new object[] { fileName }));
			}
			if (assemblyManifest.ManifestSourceFormat == ManifestSourceFormat.ID_1 && assemblyManifest.ComplibIdentity != null && assemblyManifest.ComplibIdentity.PublicKeyToken != null)
			{
				throw new InvalidDeploymentException(ExceptionTypes.IdentityMatchValidationForMixedModeAssembly, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_SimplyNamedAsmWithStrongNameComplib"), new object[] { fileName }));
			}
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000BF84 File Offset: 0x0000AF84
		public static void VerifyStrongNameAssembly(string filePath, AssemblyManifest assemblyManifest)
		{
			string fileName = Path.GetFileName(filePath);
			if (assemblyManifest.Identity.PublicKeyToken == null)
			{
				throw new InvalidDeploymentException(ExceptionTypes.Validation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_StrongNameAsmWithNoPKT"), new object[] { fileName }));
			}
			bool flag = false;
			if (assemblyManifest.ManifestSourceFormat == ManifestSourceFormat.XmlFile)
			{
				assemblyManifest.ValidateSignature(null);
			}
			else if (assemblyManifest.ManifestSourceFormat == ManifestSourceFormat.ID_1)
			{
				if (assemblyManifest.ComplibIdentity == null)
				{
					PEStream pestream = null;
					MemoryStream memoryStream = null;
					try
					{
						pestream = new PEStream(filePath, true);
						byte[] defaultId1ManifestResource = pestream.GetDefaultId1ManifestResource();
						if (defaultId1ManifestResource != null)
						{
							memoryStream = new MemoryStream(defaultId1ManifestResource);
						}
						if (memoryStream != null)
						{
							assemblyManifest.ValidateSignature(memoryStream);
							goto IL_01C3;
						}
						throw new InvalidDeploymentException(ExceptionTypes.StronglyNamedAssemblyVerification, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_StronglyNamedAssemblyNotVerifiable"), new object[] { fileName }));
					}
					finally
					{
						if (pestream != null)
						{
							pestream.Close();
						}
						if (memoryStream != null)
						{
							memoryStream.Close();
						}
					}
				}
				if (!assemblyManifest.ComplibIdentity.Equals(assemblyManifest.Identity))
				{
					throw new InvalidDeploymentException(ExceptionTypes.IdentityMatchValidationForMixedModeAssembly, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_IdentitiesDoNotMatchForMixedModeAssembly"), new object[] { fileName }));
				}
				byte b;
				if (NativeMethods.StrongNameSignatureVerificationEx(filePath, 0, out b) == 0)
				{
					throw new InvalidDeploymentException(ExceptionTypes.SignatureValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_StrongNameSignatureInvalid"), new object[] { fileName }));
				}
				flag = true;
			}
			else
			{
				if (assemblyManifest.ManifestSourceFormat != ManifestSourceFormat.CompLib)
				{
					throw new InvalidDeploymentException(ExceptionTypes.StronglyNamedAssemblyVerification, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_StronglyNamedAssemblyNotVerifiable"), new object[] { fileName }));
				}
				byte b2;
				if (NativeMethods.StrongNameSignatureVerificationEx(filePath, 0, out b2) == 0)
				{
					throw new InvalidDeploymentException(ExceptionTypes.SignatureValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_StrongNameSignatureInvalid"), new object[] { fileName }));
				}
				flag = true;
			}
			IL_01C3:
			ComponentVerifier.VerifyManifestComponentFiles(assemblyManifest, filePath, flag);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000C16C File Offset: 0x0000B16C
		protected static void VerifyManifestComponentFiles(AssemblyManifest manifest, string componentPath, bool ignoreSelfReferentialFileHash)
		{
			string directoryName = Path.GetDirectoryName(componentPath);
			foreach (global::System.Deployment.Application.Manifest.File file in manifest.Files)
			{
				string text = Path.Combine(directoryName, file.NameFS);
				if ((!ignoreSelfReferentialFileHash || string.Compare(componentPath, text, StringComparison.OrdinalIgnoreCase) != 0) && global::System.IO.File.Exists(text))
				{
					ComponentVerifier.VerifyFileHash(text, file.HashCollection);
				}
			}
		}

		// Token: 0x04000112 RID: 274
		protected ArrayList _verificationComponents = new ArrayList();

		// Token: 0x04000113 RID: 275
		protected static CMS_HASH_DIGESTMETHOD[] _supportedDigestMethods = new CMS_HASH_DIGESTMETHOD[]
		{
			CMS_HASH_DIGESTMETHOD.CMS_HASH_DIGESTMETHOD_SHA1,
			CMS_HASH_DIGESTMETHOD.CMS_HASH_DIGESTMETHOD_SHA256,
			CMS_HASH_DIGESTMETHOD.CMS_HASH_DIGESTMETHOD_SHA384,
			CMS_HASH_DIGESTMETHOD.CMS_HASH_DIGESTMETHOD_SHA512
		};

		// Token: 0x04000114 RID: 276
		protected static CMS_HASH_TRANSFORM[] _supportedTransforms = new CMS_HASH_TRANSFORM[]
		{
			CMS_HASH_TRANSFORM.CMS_HASH_TRANSFORM_MANIFESTINVARIANT,
			CMS_HASH_TRANSFORM.CMS_HASH_TRANSFORM_IDENTITY
		};

		// Token: 0x02000034 RID: 52
		protected abstract class VerificationComponent
		{
			// Token: 0x060001C4 RID: 452
			public abstract void Verify();
		}

		// Token: 0x02000035 RID: 53
		protected class FileComponent : ComponentVerifier.VerificationComponent
		{
			// Token: 0x060001C6 RID: 454 RVA: 0x0000C226 File Offset: 0x0000B226
			public FileComponent(string filePath, HashCollection hashCollection)
			{
				this._filePath = filePath;
				this._hashCollection = hashCollection;
			}

			// Token: 0x060001C7 RID: 455 RVA: 0x0000C23C File Offset: 0x0000B23C
			public override void Verify()
			{
				ComponentVerifier.VerifyFileHash(this._filePath, this._hashCollection);
			}

			// Token: 0x04000115 RID: 277
			protected string _filePath;

			// Token: 0x04000116 RID: 278
			protected HashCollection _hashCollection;
		}

		// Token: 0x02000036 RID: 54
		protected class SimplyNamedAssemblyComponent : ComponentVerifier.VerificationComponent
		{
			// Token: 0x060001C8 RID: 456 RVA: 0x0000C24F File Offset: 0x0000B24F
			public SimplyNamedAssemblyComponent(string filePath, AssemblyManifest assemblyManifest)
			{
				this._filePath = filePath;
				this._assemblyManifest = assemblyManifest;
			}

			// Token: 0x060001C9 RID: 457 RVA: 0x0000C265 File Offset: 0x0000B265
			public override void Verify()
			{
				ComponentVerifier.VerifySimplyNamedAssembly(this._filePath, this._assemblyManifest);
			}

			// Token: 0x04000117 RID: 279
			protected string _filePath;

			// Token: 0x04000118 RID: 280
			protected AssemblyManifest _assemblyManifest;
		}

		// Token: 0x02000037 RID: 55
		protected class StrongNameAssemblyComponent : ComponentVerifier.VerificationComponent
		{
			// Token: 0x060001CA RID: 458 RVA: 0x0000C278 File Offset: 0x0000B278
			public StrongNameAssemblyComponent(string filePath, AssemblyManifest assemblyManifest)
			{
				this._filePath = filePath;
				this._assemblyManifest = assemblyManifest;
			}

			// Token: 0x060001CB RID: 459 RVA: 0x0000C28E File Offset: 0x0000B28E
			public override void Verify()
			{
				ComponentVerifier.VerifyStrongNameAssembly(this._filePath, this._assemblyManifest);
			}

			// Token: 0x04000119 RID: 281
			protected string _filePath;

			// Token: 0x0400011A RID: 282
			protected AssemblyManifest _assemblyManifest;
		}
	}
}
