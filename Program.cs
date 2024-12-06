//using PgpCore;
//using System;
//using System.IO;

//class Program
//{
//    static void Main(string[] args)
//    {
//        Console.WriteLine("PGP File Encryption and Decryption");

//        string inputFilePath = @"C:\projects\input.txt"; // File to be encrypted
//        string encryptedFilePath = @"C:\projects\encrypted.pgp"; // Encrypted file
//        string decryptedFilePath = @"C:\projects\decrypted.txt"; // Decrypted file
//        string publicKeyPath = @"C:\projects\publicKey.asc"; // Public key for encryption
//        string privateKeyPath = @"C:\projects\privateKey.asc"; // Private key for decryption
//        string passphrase = "12345678"; // Passphrase for private key

//        File.WriteAllText(inputFilePath, "This is a secret message!");

//        try
//        {
//            Console.WriteLine("Encrypting file...");
//            EncryptFile(inputFilePath, encryptedFilePath, publicKeyPath);
//            Console.WriteLine("File encrypted successfully!");

//            Console.WriteLine("Decrypting file...");
//            DecryptFile(encryptedFilePath, decryptedFilePath, privateKeyPath, passphrase);
//            Console.WriteLine("File decrypted successfully!");

//            string decryptedContent = File.ReadAllText(decryptedFilePath);
//            Console.WriteLine("Decrypted Content: " + decryptedContent);
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine("Error: " + ex.Message);
//        }
//    }

//    static void EncryptFile(string inputFile, string outputFile, string publicKeyPath)
//    {
//        // Initialize PGP with EncryptionKeys for encryption
//        EncryptionKeys encryptionKeys = new EncryptionKeys(publicKeyPath);

//        using (PGP pgp = new PGP(encryptionKeys))
//        {
//            using (FileStream inputFileStream = new FileStream(inputFile, FileMode.Open))
//            using (FileStream outputFileStream = new FileStream(outputFile, FileMode.Create))
//            {
//                // Encrypt the stream
//                pgp.EncryptStream(inputFileStream, outputFileStream);
//            }
//        }
//    }

//    static void DecryptFile(string inputFile, string outputFile, string privateKeyPath, string passphrase)
//    {
//        // Initialize PGP with EncryptionKeys for decryption
//        EncryptionKeys decryptionKeys = new EncryptionKeys(privateKeyPath, passphrase);

//        using (PGP pgp = new PGP(decryptionKeys))
//        {
//            using (FileStream inputFileStream = new FileStream(inputFile, FileMode.Open))
//            using (FileStream outputFileStream = new FileStream(outputFile, FileMode.Create))
//            {
//                // Decrypt the stream
//                pgp.DecryptStream(inputFileStream, outputFileStream);
//            }
//        }
//    }
//}
//using PgpCore;

//using (PGP pgp = new PGP())
//{
//    // Generate keys
//    pgp.GenerateKey(new FileInfo(@"C:\projects\public.asc"), new FileInfo(@"C:\projects\private.asc"), "email@email.com", "password");
//}


using PgpCore;
using System;
using System.IO;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            Console.WriteLine("Starting encryption process...");

            // Load public key
            FileInfo publicKey = new FileInfo(@"C:\projects\public.asc");
            if (!publicKey.Exists)
            {
                Console.WriteLine("Public key file not found at: " + publicKey.FullName);
                return;
            }

            EncryptionKeys encryptionKeys = new EncryptionKeys(publicKey);

            // Reference input/output files
            FileInfo inputFile = new FileInfo(@"C:\projects\input.txt");
            FileInfo encryptedFile = new FileInfo(@"C:\projects\encrypted.pgp");

            // Ensure input file exists
            if (!inputFile.Exists)
            {
                Console.WriteLine("Input file not found at: " + inputFile.FullName);
                return;
            }

            Console.WriteLine("Encrypting file...");
            using (PGP pgp = new PGP(encryptionKeys))
            {
                // Encrypt asynchronously
                await pgp.EncryptAsync(inputFile, encryptedFile);
            }

            Console.WriteLine($"File encrypted successfully: {encryptedFile.FullName}");


            Console.WriteLine("Decrypting the file...");
            FileInfo privateKey = new FileInfo(@"C:\projects\private.asc");
            EncryptionKeys decryptionKeys = new EncryptionKeys(privateKey, "password");

            // Reference input/output files
            FileInfo encfile = new FileInfo(@"C:\projects\encrypted.pgp");
            FileInfo decryptedFile = new FileInfo(@"C:\projects\decrypted.txt");
            using (PGP pgp = new PGP(decryptionKeys))
            {
                await pgp.DecryptAsync(encfile, decryptedFile);
            }
            Console.WriteLine($"File decrypted successfully: {decryptedFile.FullName}");


        }
        catch (Exception ex)
        {
            Console.WriteLine("Error during encryption: " + ex.Message);
        }
    }
}
