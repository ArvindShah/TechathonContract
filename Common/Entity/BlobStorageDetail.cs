using System;
using System.Runtime.Serialization;

namespace Common.Entity
{
    [DataContract]
    [Serializable]
    public class BlobStorageDetail
    {
        [DataMember]
        public string StorageName { get; set; }

        [DataMember]
        public string StorageKey { get; set; }

        [DataMember]
        public string ProjectName { get; set; }

        [DataMember]
        public string FSContainerName { get; set; }

        [DataMember]
        public string BLOBContainerName { get; set; }

        [DataMember]
        public string FilePath { get; set; }

        [DataMember]
        public string Extension { get; set; }
    }
}
