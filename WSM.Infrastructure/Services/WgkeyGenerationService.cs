
//using NSec.Cryptography;
//using System.Net.NetworkInformation;
using System.Security.Cryptography;
using WSM.Domain.Interfaces;
using Sodium;
namespace WSM.Infrastructure.Services

{
    public  class WgkeyGenerationService : IWgKeyGenerationService
    {

        public (string PrivateKey, string PublicKey) GenerateWgKeyPair()
        {
            //var algorithm = KeyAgreementAlgorithm.X25519;
            //var creationParameters = new KeyCreationParameters
            //{
            //    ExportPolicy = KeyExportPolicies.AllowPlaintextExport
            //}; 
            //var key = Key.Create(algorithm, creationParameters);
            //var privateKey = Convert.ToBase64String(key.Export(KeyBlobFormat.RawPrivateKey));
            //var publicKey = Convert.ToBase64String(key.Export(KeyBlobFormat.RawPublicKey));
            //return (privateKey, publicKey);

            byte[] privateKey = SecretBox.GenerateKey(); // Generates a 32-byte key suitable for X25519

            // Derive the public key using ScalarMult.Base
            byte[] publicKey = ScalarMult.Base(privateKey);

            // Convert keys to Base64 (optional for readability)
            string privateKeyBase64 = Convert.ToBase64String(privateKey);
            string publicKeyBase64 = Convert.ToBase64String(publicKey);
            return (privateKeyBase64, publicKeyBase64);


        }

    }

}