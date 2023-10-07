using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;

namespace Millionandup.MsIdentityServer.Services
{
    /// <summary>
    /// Class responsible for offering encryption services in AES 256 under the GCM model
    /// </summary>
    public class AesGcmService : IDisposable
    {
        private readonly AesGcm _aes;
        /// <summary>
        /// Get AesGcmService instance
        /// </summary>
        /// <param name="password">Password</param>
        public AesGcmService(string password)
        {
            // Derive key
            // AES key size is 16 bytes
            // We use a fixed salt and small iteration count here; the latter should be increased for weaker passwords
            byte[] key = new Rfc2898DeriveBytes(password, new byte[8], 1000).GetBytes(16);

            // Initialize AES implementation
            _aes = new AesGcm(key);
        }

        /// <summary>
        /// Encrypt data
        /// </summary>
        /// <param name="plain">Plain text</param>
        /// <returns>Data encrypted</returns>
        public string Encrypt(string plain)
        {
            // Get bytes of plaintext string
            byte[] plainBytes = Encoding.UTF8.GetBytes(plain);

            // Get parameter sizes
            int nonceSize = AesGcm.NonceByteSizes.MaxSize;
            int tagSize = AesGcm.TagByteSizes.MaxSize;
            int cipherSize = plainBytes.Length;

            // We write everything into one big array for easier encoding
            int encryptedDataLength = 4 + nonceSize + 4 + tagSize + cipherSize;
            Span<byte> encryptedData = encryptedDataLength < 1024 ? stackalloc byte[encryptedDataLength] : new byte[encryptedDataLength].AsSpan();

            // Copy parameters
            BinaryPrimitives.WriteInt32LittleEndian(encryptedData.Slice(0, 4), nonceSize);
            BinaryPrimitives.WriteInt32LittleEndian(encryptedData.Slice(4 + nonceSize, 4), tagSize);
            var nonce = encryptedData.Slice(4, nonceSize);
            var tag = encryptedData.Slice(4 + nonceSize + 4, tagSize);
            var cipherBytes = encryptedData.Slice(4 + nonceSize + 4 + tagSize, cipherSize);

            // Generate secure nonce
            RandomNumberGenerator.Fill(nonce);

            // Encrypt
            _aes.Encrypt(nonce, plainBytes.AsSpan(), cipherBytes, tag);

            // Encode for transmission
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// Decrypt data
        /// </summary>
        /// <param name="cipher">Data encrypted</param>
        /// <returns>Plain text</returns>
        public string Decrypt(string cipher)
        {
            // Decode
            Span<byte> encryptedData = Convert.FromBase64String(cipher).AsSpan();

            // Extract parameter sizes
            int nonceSize = BinaryPrimitives.ReadInt32LittleEndian(encryptedData.Slice(0, 4));
            int tagSize = BinaryPrimitives.ReadInt32LittleEndian(encryptedData.Slice(4 + nonceSize, 4));
            int cipherSize = encryptedData.Length - 4 - nonceSize - 4 - tagSize;

            // Extract parameters
            var nonce = encryptedData.Slice(4, nonceSize);
            var tag = encryptedData.Slice(4 + nonceSize + 4, tagSize);
            var cipherBytes = encryptedData.Slice(4 + nonceSize + 4 + tagSize, cipherSize);

            // Decrypt
            Span<byte> plainBytes = cipherSize < 1024 ? stackalloc byte[cipherSize] : new byte[cipherSize];
            _aes.Decrypt(nonce, cipherBytes, tag, plainBytes);

            // Convert plain bytes back into string
            return Encoding.UTF8.GetString(plainBytes);
        }

        /// <summary>
        /// Dispose service
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Dispose service
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            _aes.Dispose();
        }
    }
}
