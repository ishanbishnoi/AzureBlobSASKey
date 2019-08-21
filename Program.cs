using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace project9_csharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Azure Blob Storage - .NET quickstart sample\n");

            // Run the examples asynchronously, wait for the results before proceeding
            ProcessAsync().GetAwaiter().GetResult();    
            MyClass c1 = new MyClass();
            Console.WriteLine($"Hello World! {c1.ReturnMessage()}");
            Console.WriteLine("Press any key to exit the sample application.");
            Console.ReadLine();    
        }

        private static string GetBlobSasUri(CloudBlobContainer container, string blobName, string policyName = null)
        {
            string sasBlobToken;

            // Get a reference to a blob within the container.
            // Note that the blob may not exist yet, but a SAS can still be created for it.
            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);

            if (policyName == null)
            {
                // Create a new access policy and define its constraints.
                // Note that the SharedAccessBlobPolicy class is used both to define the parameters of an ad hoc SAS, and
                // to construct a shared access policy that is saved to the container's shared access policies.
                SharedAccessBlobPolicy adHocSAS = new SharedAccessBlobPolicy()
                {
                    // When the start time for the SAS is omitted, the start time is assumed to be the time when the storage service receives the request.
                    // Omitting the start time for a SAS that is effective immediately helps to avoid clock skew.
                    SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24),
                    Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Create
                };

                // Generate the shared access signature on the blob, setting the constraints directly on the signature.
                sasBlobToken = blob.GetSharedAccessSignature(adHocSAS);

                Console.WriteLine("SAS for blob (ad hoc): {0}", sasBlobToken);
                Console.WriteLine();
            }
            else
            {
                // Generate the shared access signature on the blob. In this case, all of the constraints for the
                // shared access signature are specified on the container's stored access policy.
                sasBlobToken = blob.GetSharedAccessSignature(null, policyName);

                Console.WriteLine("SAS for blob (stored access policy): {0}", sasBlobToken);
                Console.WriteLine();
            }

            // Return the URI string for the container, including the SAS token.
            return blob.Uri + sasBlobToken;
        }





        private static async Task ProcessAsync()
        {
            // Retrieve the connection string for use with the application. The storage 
            // connection string is stored in an environment variable on the machine 
            // running the application called CONNECT_STR. If the 
            // environment variable is created after the application is launched in a 
            // console or with Visual Studio, the shell or application needs to be closed
            // and reloaded to take the environment variable into account.

            // string storageConnectionString = Environment.GetEnvironmentVariable("CONNECT_STR");
            string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=xxxxxxx;AccountKey=xxxxxxxxxxxxxxxxxxxxx;EndpointSuffix=core.windows.net";
            // Check whether the connection string can be parsed.
            CloudStorageAccount storageAccount;
            Console.WriteLine(storageConnectionString);

            if (CloudStorageAccount.TryParse(storageConnectionString, out storageAccount))
            {
                // If the connection string is valid, proceed with operations against Blob
                // storage here.
                // ADD OTHER OPERATIONS HERE
                Console.WriteLine("The connection string is successful");
                CloudBlobClient blobclientwithSAS = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobclientwithSAS.GetContainerReference("testib");
                // Console.WriteLine(GetBlobSasUri(container1,"pmadb","testib_ap_fromexplorer"));
                Console.WriteLine(GetBlobSasUri(container,"hem"));
                Console.ReadLine();
            }
            else
            {
                // Otherwise, let the user know that they need to define the environment variable.
                Console.WriteLine(
                    "A connection string has not been defined in the system environment variables. " +
                    "Add an environment variable named 'CONNECT_STR' with your storage " +
                    "connection string as a value.");
                Console.WriteLine("Press any key to exit the application.");
                Console.ReadLine();
            }
        }
    }
}
