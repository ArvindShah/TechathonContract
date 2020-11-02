using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper.DataLakeHelper
{
    public class StorageBlob
    {
        private readonly string _containername = string.Empty;

        /// <summary>
        /// Gets or sets the Azure Storage BLOB client.
        /// </summary>
        /// <value>
        /// The Azure Storage BLOB client.
        /// </value>
        protected CloudBlobClient BlobClient { get; set; }

        /// <summary>
        /// Gets or sets the Cloud Storage account.
        /// </summary>
        /// <value>
        /// The Cloud Storage Account account.
        /// </value>
        public CloudStorageAccount Account { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageBlob"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string for the storage account.</param>
        /// <param name="container">The name of the container.</param>
        public StorageBlob(string connectionString, string container)
        {
            if (connectionString == null) throw new ArgumentNullException("connectionString");
            if (container == null) throw new ArgumentNullException("container");

            try
            {
                // Retrieve storage account from connection string
                Account = CloudStorageAccount.Parse(connectionString);
                _containername = container;
                BlobClient = Account.CreateCloudBlobClient();
            }
            catch (InvalidOperationException e)
            {
                throw;
            }
        }

        /// <summary>
        /// Downloads the BLOB as byte array.
        /// </summary>
        /// <param name="blobName">Name of the BLOB.</param>
        /// <returns>The array of bytes of the BLOB.</returns>
        public async Task<byte[]> DownloadBlobAsByteArray(string blobName)
        {
            try
            {
                CloudBlobContainer container = BlobClient.GetContainerReference(_containername);
                CloudBlockBlob blob = null;
                Uri result;
                if (Uri.TryCreate(blobName, UriKind.Absolute, out result))
                {
                    blob = (CloudBlockBlob)await BlobClient.GetBlobReferenceFromServerAsync(result);
                }
                else
                {
                    blob = container.GetBlockBlobReference(blobName);
                }
                await blob.FetchAttributesAsync();
                byte[] fileByteArray = new byte[blob.Properties.Length];
                await blob.DownloadToByteArrayAsync(fileByteArray, 0);

                return fileByteArray;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Updates the blob in the container with a new file.
        /// </summary>
        /// <param name="blobName">Name of the BLOB.</param>
        /// <param name="fileData">The file data.</param>
        /// <returns>Returns the Uri to the blob.</returns>
        public string UploadFileIntoContainer(string blobName, byte[] fileData)
        {
            try
            {
                // Retrieve reference to a previously created container
                CloudBlobContainer container = BlobClient.GetContainerReference(_containername);

                // Retrieve reference to a blob named "myblob"
                CloudBlockBlob blob = container.GetBlockBlobReference(blobName);

                // Create or overwrite the blob with file data from a byte array
                blob.UploadFromByteArrayAsync(fileData, 0, fileData.Length);
                return blob.Uri.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }

}
