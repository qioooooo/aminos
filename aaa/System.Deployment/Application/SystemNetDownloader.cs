using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;

namespace System.Deployment.Application
{
	// Token: 0x020000CC RID: 204
	internal class SystemNetDownloader : FileDownloader
	{
		// Token: 0x0600055C RID: 1372 RVA: 0x0001C1F8 File Offset: 0x0001B1F8
		private static Stream GetOutputFileStream(string targetPath)
		{
			return new FileStream(targetPath, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0001C210 File Offset: 0x0001B210
		protected void DownloadSingleFile(FileDownloader.DownloadQueueItem next)
		{
			WebRequest webRequest = WebRequest.Create(next._sourceUri);
			webRequest.Credentials = CredentialCache.DefaultCredentials;
			RequestCachePolicy requestCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
			webRequest.CachePolicy = requestCachePolicy;
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			if (httpWebRequest != null)
			{
				httpWebRequest.UnsafeAuthenticatedConnectionSharing = true;
				httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip;
				httpWebRequest.CookieContainer = SystemNetDownloader.GetUriCookieContainer(httpWebRequest.RequestUri);
				IWebProxy defaultWebProxy = WebRequest.DefaultWebProxy;
				if (defaultWebProxy != null)
				{
					defaultWebProxy.Credentials = CredentialCache.DefaultCredentials;
				}
			}
			if (this._fCancelPending)
			{
				return;
			}
			WebResponse webResponse = null;
			try
			{
				webResponse = webRequest.GetResponse();
				UriHelper.ValidateSupportedScheme(webResponse.ResponseUri);
				if (!this._fCancelPending)
				{
					this._eventArgs._fileSourceUri = next._sourceUri;
					this._eventArgs._fileResponseUri = webResponse.ResponseUri;
					this._eventArgs.FileLocalPath = next._targetPath;
					this._eventArgs.Cookie = null;
					if (webResponse.ContentLength > 0L)
					{
						base.CheckForSizeLimit((ulong)webResponse.ContentLength, false);
						this._accumulatedBytesTotal += webResponse.ContentLength;
					}
					base.SetBytesTotal();
					base.OnModified();
					Stream stream = null;
					Stream stream2 = null;
					int tickCount = Environment.TickCount;
					try
					{
						stream = webResponse.GetResponseStream();
						Directory.CreateDirectory(Path.GetDirectoryName(next._targetPath));
						stream2 = SystemNetDownloader.GetOutputFileStream(next._targetPath);
						if (stream2 != null)
						{
							long num = 0L;
							if (webResponse.ContentLength > 0L)
							{
								stream2.SetLength(webResponse.ContentLength);
							}
							while (!this._fCancelPending)
							{
								int num2 = stream.Read(this._buffer, 0, this._buffer.Length);
								if (num2 > 0)
								{
									stream2.Write(this._buffer, 0, num2);
								}
								this._eventArgs._bytesCompleted += (long)num2;
								if (webResponse.ContentLength <= 0L)
								{
									this._accumulatedBytesTotal += (long)num2;
									base.SetBytesTotal();
								}
								num += (long)num2;
								if (next._maxFileSize != -1 && num > (long)next._maxFileSize)
								{
									throw new InvalidDeploymentException(ExceptionTypes.FileSizeValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_FileBeingDownloadedTooLarge"), new object[]
									{
										next._sourceUri.ToString(),
										next._maxFileSize
									}));
								}
								base.CheckForSizeLimit((ulong)((long)num2), true);
								if (this._eventArgs._bytesTotal > 0L)
								{
									this._eventArgs._progress = (int)(this._eventArgs._bytesCompleted * 100L / this._eventArgs._bytesTotal);
								}
								base.OnModifiedWithThrottle(ref tickCount);
								if (num2 <= 0)
								{
									if (webResponse.ContentLength != num)
									{
										stream2.SetLength(num);
										goto IL_0294;
									}
									goto IL_0294;
								}
							}
							return;
						}
						IL_0294:;
					}
					finally
					{
						if (stream != null)
						{
							stream.Close();
						}
						if (stream2 != null)
						{
							stream2.Close();
						}
					}
					this._eventArgs.Cookie = next._cookie;
					this._eventArgs._filesCompleted++;
					base.OnModified();
					DownloadResult downloadResult = new DownloadResult();
					downloadResult.ResponseUri = webResponse.ResponseUri;
					downloadResult.ServerInformation.Server = webResponse.Headers["Server"];
					downloadResult.ServerInformation.PoweredBy = webResponse.Headers["X-Powered-By"];
					downloadResult.ServerInformation.AspNetVersion = webResponse.Headers["X-AspNet-Version"];
					this._downloadResults.Add(downloadResult);
				}
			}
			catch (InvalidOperationException ex)
			{
				string text = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_FailedWhileDownloading"), new object[] { next._sourceUri });
				throw new DeploymentDownloadException(text, ex);
			}
			catch (IOException ex2)
			{
				string text2 = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_FailedWhileDownloading"), new object[] { next._sourceUri });
				throw new DeploymentDownloadException(text2, ex2);
			}
			catch (UnauthorizedAccessException ex3)
			{
				string text3 = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_FailedWhileDownloading"), new object[] { next._sourceUri });
				throw new DeploymentDownloadException(text3, ex3);
			}
			finally
			{
				if (webResponse != null)
				{
					webResponse.Close();
				}
			}
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x0001C6A0 File Offset: 0x0001B6A0
		protected override void DownloadAllFiles()
		{
			do
			{
				FileDownloader.DownloadQueueItem downloadQueueItem = null;
				lock (this._fileQueue)
				{
					if (this._fileQueue.Count > 0)
					{
						downloadQueueItem = (FileDownloader.DownloadQueueItem)this._fileQueue.Dequeue();
					}
				}
				if (downloadQueueItem == null)
				{
					break;
				}
				this.DownloadSingleFile(downloadQueueItem);
			}
			while (!this._fCancelPending);
			if (this._fCancelPending)
			{
				throw new DownloadCancelledException();
			}
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x0001C714 File Offset: 0x0001B714
		private static CookieContainer GetUriCookieContainer(Uri uri)
		{
			CookieContainer cookieContainer = null;
			uint num = 0U;
			if (NativeMethods.InternetGetCookieW(uri.ToString(), null, null, ref num))
			{
				StringBuilder stringBuilder = new StringBuilder((int)(num / 2U));
				if (NativeMethods.InternetGetCookieW(uri.ToString(), null, stringBuilder, ref num) && stringBuilder.Length > 0)
				{
					try
					{
						cookieContainer = new CookieContainer();
						cookieContainer.SetCookies(uri, stringBuilder.ToString().Replace(';', ','));
					}
					catch (CookieException)
					{
						cookieContainer = null;
					}
				}
			}
			return cookieContainer;
		}
	}
}
