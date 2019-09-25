using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Android.OS;
using Android.Runtime;
using Android.Security;
using Android.Security.Keystore;
using Java.Security;
using Javax.Crypto;
using Javax.Crypto.Spec;
using Uno.UI;
using CipherMode = Javax.Crypto.CipherMode;

namespace Windows.Security.Credentials
{
	sealed partial class PasswordVault
	{
		public PasswordVault()
			: this(new KeyStorePersister())
		{
		}

		public PasswordVault(string filePath)
			: this(new KeyStorePersister(filePath))
		{
		}

		private sealed class KeyStorePersister : FilePersister
		{
			private const string _notSupported = @"There is no way to properly persist secured content on this device.
The 'AndroidKeyStore' is missing (or is innacessible), but it is a requirement for the 'PasswordVault' to store data securly.
This usually means that the device is using an API older than 18 (4.3). More details: https://developer.android.com/reference/java/security/KeyStore";

			private const string _algo = KeyProperties.KeyAlgorithmAes;
			private const string _block = KeyProperties.BlockModeCbc;
			private const string _padding = KeyProperties.EncryptionPaddingPkcs7;
			private const string _fullTransform = _algo + "/" + _block + "/" + _padding;
			private const string _lowLevelDeviceTransform = "RSA/ECB/PKCS1Padding";
			private const string _provider = "AndroidKeyStore";
			private const string _alias = "uno_passwordvault";
			private static readonly byte[] _iv = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(_alias));

			private readonly IKey _key;
			private readonly KeyPair _keyPair;


			public KeyStorePersister(string filePath = null)
				: base(filePath)
			{
				KeyStore store;
				try
				{
					store = KeyStore.GetInstance(_provider);
				}
				catch (Exception e)
				{
					throw new NotSupportedException(_notSupported, e);
				}
				if (store == null)
				{
					throw new NotSupportedException(_notSupported);
				}

				store.Load(null);

				if (store.ContainsAlias(_alias))
				{
					if (Android.OS.Build.VERSION.SdkInt > BuildVersionCodes.LollipopMr1)
					{
						var key = store.GetKey(_alias, null);
						_key = key;
					}
					else
					{
						var privateKey = store.GetKey(_alias, null)?.JavaCast<IPrivateKey>();
						var cert = store.GetCertificate(_alias);
						_keyPair = new KeyPair(cert.PublicKey, privateKey);
					}
				}
				else
				{
					if (Android.OS.Build.VERSION.SdkInt > BuildVersionCodes.LollipopMr1)
					{
						var generator = KeyGenerator.GetInstance(_algo, _provider);
						generator.Init(new KeyGenParameterSpec.Builder(_alias, KeyStorePurpose.Encrypt | KeyStorePurpose.Decrypt)
							.SetBlockModes(_block)
							.SetEncryptionPaddings(_padding)
							.SetRandomizedEncryptionRequired(false)
							.Build());
						_key = generator.GenerateKey();
					}
					else
					{
						// We Allow KeyPairGenerator for API Level inferior to 23
#pragma warning disable CS0618
						var asymmetricAlias = $"{_alias}.asymmetric";
						var end = DateTime.UtcNow.AddYears(20);
						var generator = KeyPairGenerator.GetInstance(KeyProperties.KeyAlgorithmRsa, _provider);

						var builder = new KeyPairGeneratorSpec.Builder(ContextHelper.Current.ApplicationContext)
							.SetAlias(_alias)
							.SetSerialNumber(Java.Math.BigInteger.One)
							.SetSubject(new Javax.Security.Auth.X500.X500Principal($"CN={asymmetricAlias} CA Certificate"))
							.SetStartDate(new Java.Util.Date())
							.SetEndDate(new Java.Util.Date(end.Year, end.Month, end.Day));
						generator.Initialize(builder.Build());

						_keyPair = generator.GenerateKeyPair();
#pragma warning restore CS0618
					}
				}
			}

			/// <inheritdoc />
			protected override Stream Encrypt(Stream outputStream)
			{
				Cipher cipher = null;
				if (Android.OS.Build.VERSION.SdkInt > BuildVersionCodes.LollipopMr1)
				{
					cipher = Cipher.GetInstance(_fullTransform);
					var iv = new IvParameterSpec(_iv, 0, cipher.BlockSize);
					cipher.Init(CipherMode.EncryptMode, _key, iv);
				}
				else
				{
					cipher = Cipher.GetInstance(_lowLevelDeviceTransform);
					// Android API level 22 and inferior supports an IV of 12 for RSA 
					cipher.Init(CipherMode.EncryptMode, _keyPair.Public);
				}

				return new CipherStreamAdapter(new CipherOutputStream(outputStream, cipher));
			}

			/// <inheritdoc />
			protected override Stream Decrypt(Stream inputStream)
			{
				Cipher cipher = null;
				if (Android.OS.Build.VERSION.SdkInt > BuildVersionCodes.LollipopMr1)
				{
					cipher = Cipher.GetInstance(_fullTransform);
					var iv = new IvParameterSpec(_iv, 0, cipher.BlockSize);

					cipher.Init(CipherMode.DecryptMode, _key, iv);
				}
				else
				{
					cipher = Cipher.GetInstance(_lowLevelDeviceTransform);
					cipher.Init(CipherMode.DecryptMode, _keyPair.Private);
				}
				return new InputStreamInvoker(new CipherInputStream(inputStream, cipher));
			}

			private class CipherStreamAdapter : Stream
			{
				private readonly CipherOutputStream _output;
				private readonly Stream _adapter;

				private bool _isDisposed;

				public CipherStreamAdapter(CipherOutputStream output)
				{
					_output = output;
					_adapter = new OutputStreamInvoker(output);
				}

				public override bool CanRead => _adapter.CanRead;

				public override bool CanSeek => _adapter.CanSeek;

				public override bool CanWrite => _adapter.CanWrite;

				public override long Length => _adapter.Length;

				public override long Position
				{
					get => _adapter.Position;
					set => _adapter.Position = value;
				}

				public override void Flush()
					=> _adapter.Flush();

				protected override void Dispose(bool disposing)
				{
					if (_isDisposed)
					{
						// We cannot .Close() the _output multiple times.
						return;
					}
					_isDisposed = true;
					// Will need to try to reduice the key lenght for API Level Bellow 23
					if (disposing)
					{
						_output.Close();
					}

					_adapter.Dispose();
					_output.Dispose();
					base.Dispose(disposing);
				}

				public override int Read(byte[] buffer, int offset, int count)
					=> _adapter.Read(buffer, offset, count);

				public override long Seek(long offset, SeekOrigin origin)
					=> _adapter.Seek(offset, origin);

				public override void SetLength(long value)
					=> _adapter.SetLength(value);

				public override void Write(byte[] buffer, int offset, int count)
					=> _adapter.Write(buffer, offset, count);
			}
		}
	}
}
