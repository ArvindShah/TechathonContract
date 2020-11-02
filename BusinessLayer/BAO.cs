using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Common.Entity;
using Common.Helper.DataLakeHelper;
using DataAccessLayer;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Hosting;
using OpenXmlPowerTools;
using spend.selfserve.businessLayer.Helper.DataLakeHelper;

namespace BusinessLayer
{
    public class BAO : IBAO
    {
        private readonly IDAO _DAO;
        private readonly IHostingEnvironment _env;

        public BAO(IDAO DAO, IHostingEnvironment env)
        {
            _DAO = DAO;
            _env = env;
        }

        public int SaveUser(int userId)
        {
            return _DAO.SaveUser(userId);
        }

        public List<ContentMaster> GetContentData(bool IsClause, int ContentId = 0)
        {
            return _DAO.GetContentData(IsClause);
        }
        public int SaveUserTransaction(int id, int UserId, int Templateid, string LastVersion, string CurrentVersion, DateTime ModifiedDate)
        {
            return _DAO.SaveUserTransaction(id, UserId, Templateid, LastVersion, CurrentVersion, ModifiedDate);

        }
        public async void UploadFileToDatalake(Stream fileStream, string fileName, int documentTypeId, int documentTemplateId)
        {
            try
            {
                string fileType = fileName.Substring(fileName.Length - 4);
                string newFileName = fileName.Remove(fileName.Length - 5);
                int lastIndex = newFileName.LastIndexOf(".");
                if (lastIndex < 0)
                {
                    newFileName = newFileName + "_v1.0.0.0";
                    lastIndex = newFileName.LastIndexOf(".");
                }
                string lastNumber = newFileName.Substring(lastIndex + 1);
                string increment = (int.Parse(lastNumber) + 1).ToString();
                string result = string.Concat(newFileName.Substring(0, lastIndex + 1), increment) + "." + fileType;

                var folderPath = $"{"Techathon20"}/{documentTypeId}/{documentTemplateId}";
                var blobStorageDetail = _DAO.GetDataLakeStorageDetails();
                var resourceConfigurations = _DAO.GetResourcesConfigurations();
                bool isFileMoved = await DataLakeHelper.MoveFileToDataLakeWithRetry(blobStorageDetail.StorageName, folderPath, result, fileStream, resourceConfigurations);
                if (!isFileMoved)
                    throw new ApplicationException("Error: File could not moved due to unknown problem at blob storage");
            }
            catch
            {
                throw;
            }

        }
        public List<UserMaster> GetAllUsers(String UserId = "")
        {
            return _DAO.GetAllUsers(UserId);
        }
        public List<TemplateMaster> GetAllTemplate(int TemplateId = 0)
        {
            return _DAO.GetAllTemplate(TemplateId);
        }
        public List<UserTransactiondata> GetAllUserTransaction()
        {
            return _DAO.GetAllUserTransaction();
        }
        public List<UserTemplateMapping> GetAllUserTemplateMapping(int userid = 0)
        {
            return _DAO.GetAllUserTemplateMapping(userid);
        }
        public string SaveUserTemplateMapping(UserTemplateMapping objUserTemplateMapping)
        {
            return _DAO.SaveUserTemplateMapping(objUserTemplateMapping);
        }
        public List<string> GetAllVersionByTeplateId(int TemplateId)
        {
            return _DAO.GetAllVersionByTeplateId(TemplateId);
        }
        public List<TemplateMaster> GetAllTemplateByUserId(int UserId = 0)
        {
            return _DAO.GetAllTemplateByUserId(UserId);
        }
        public string UpdateUserToAdmin(UserAdmin objUserAdmin)
        {
            return _DAO.UpdateUserToAdmin(objUserAdmin);
        }

        public string DeleteTemplateUserMapping(DelUserTemp delObj)
        {
            return _DAO.DeleteTemplateUserMapping(delObj);
        }

        public async Task<Stream> GetUploadedDoc(string docName = "Master Agreement_Template (1) (1)", string version = "1.0.0.1")
        {
            BlobStorageDetail blobStorageDetail = _DAO.GetDataLakeStorageDetails();

            string storageConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}",
                                                                blobStorageDetail.StorageName, blobStorageDetail.StorageKey);

            var storageBlob = new StorageBlob(storageConnectionString, blobStorageDetail.FSContainerName);

            string filepath = storageBlob.Account.BlobStorageUri.PrimaryUri.AbsoluteUri + blobStorageDetail.FSContainerName + "/Techathon20/12/13/" + docName + "_v" + version + ".docx";

            Stream file = await DataLakeHelper.GetFileStreamWithStorageBlob(storageBlob, filepath);
            //var parameters = ClientRequestParametersProvider.GetClientParameters(HttpContext, clientId);
            return file;
        }

        public StringBuilder getWordDoc(string docName, string version)
        {
            var str = GetUploadedDoc(docName, version);
            Stream stream = str.Result;
            StringBuilder word = new StringBuilder();
            using (WordprocessingDocument doc = WordprocessingDocument.Open(stream, true))
            {
                HtmlConverterSettings settings = new HtmlConverterSettings()
                {
                    PageTitle = docName
                };
                XElement html = HtmlConverter.ConvertToHtml(doc, settings);
                word.Append(html);
            }

            return word;
        }

        public List<ContentControl> GetContentControl(string fileName, string version)
        {
            try
            {
                List<ContentControl> contentControls = new List<ContentControl>();
                var str = GetUploadedDoc(fileName, version);
                Stream stream = str.Result;
                using (WordprocessingDocument doc = WordprocessingDocument.Open(stream, true))
                {

                    MainDocumentPart parteDocumento = doc.MainDocumentPart;
                    var content = doc.MainDocumentPart.RootElement.Descendants().Where(e => e is SdtBlock || e is SdtRun);
                    foreach (var cc in content)
                    {

                        SdtProperties props = cc.Elements<SdtProperties>().FirstOrDefault();
                        var content1 = cc.Elements<SdtContentRun>().FirstOrDefault();
                        var run = content1.Descendants<Run>().FirstOrDefault();
                        var runProp = cc.Elements<SdtContentRun>().FirstOrDefault().Descendants<Run>().FirstOrDefault().Descendants<RunProperties>().FirstOrDefault();
                        Tag tag = props.Elements<Tag>().FirstOrDefault();
                        Text tcxt = cc.Descendants<Text>().First();

                        contentControls.Add(new ContentControl() { Content = tcxt.Text, Tag = tag.Val });
                    }
                }
                return contentControls;
            }
            catch
            {
                throw;
            }
        }

        public void CheckOutContentControl(string fileName, string version, string tagVal, string contentControl)
        {
            try
            {
                List<ContentControl> contentControls = new List<ContentControl>();
                var str = GetUploadedDoc(fileName, version);
                Stream stream = str.Result;

                using (WordprocessingDocument doc = WordprocessingDocument.Open(stream, true))
                {

                    MainDocumentPart parteDocumento = doc.MainDocumentPart;
                    var content = doc.MainDocumentPart.RootElement.Descendants().Where(e => e is SdtBlock || e is SdtRun);
                    foreach (var cc in content)
                    {

                        SdtProperties props = cc.Elements<SdtProperties>().FirstOrDefault();
                        var content1 = cc.Elements<SdtContentRun>().FirstOrDefault();
                        var run = content1.Descendants<Run>().FirstOrDefault();
                        var runProp = cc.Elements<SdtContentRun>().FirstOrDefault().Descendants<Run>().FirstOrDefault().Descendants<RunProperties>().FirstOrDefault();
                        Tag tag = props.Elements<Tag>().FirstOrDefault();
                        if (tag.Val == tagVal)
                        {
                            Text tcxt = cc.Descendants<Text>().First();
                            tcxt.Text = string.Empty;

                            Run formattedRun = new Run();
                            RunProperties runPro = new RunProperties();
                            RunFonts runFont = new RunFonts() { Ascii = "Cambria(Headings)", HighAnsi = "Cambria(Headings)" };
                            Bold bold = new Bold();
                            Text text = new Text(contentControl);
                            Color color = new Color() { Val = "365F91", ThemeColor = ThemeColorValues.Accent1, ThemeShade = "BF" };
                            runPro.Append(runFont);
                            runPro.Append(bold);
                            runPro.Append(color);
                            runPro.Append(text);
                            formattedRun.Append(runPro);
                            //runProp.Append(runPro);
                            content1.Append(formattedRun);
                        }
                    }
                    parteDocumento.Document.Save();
                    //doc.Close();
                }
                //Stream stream2 = new MemoryStream(result);
                stream.Position = 0; //let's rewind it

                UploadFileToDatalake(stream, fileName +"_v"+ version+".docx", 12, 12);
            }
            catch
            {
                throw;
            }
        }

        public void SaveDOCX(string fileName, string BodyText, bool isLandScape = false, double rMargin = 1, double lMargin = 1, double bMargin = 1, double tMargin = 1)
        {
            string destFile = Path.Combine(Path.Combine(_env.ContentRootPath, @"App_Data\Files"), "Clause Templat1_v1.0.0.5.docx");
            WordprocessingDocument document = WordprocessingDocument.Create(destFile, WordprocessingDocumentType.Document);
            MainDocumentPart mainDocumenPart = document.MainDocumentPart;

            //Place the HTML String into a MemoryStream Object
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(BodyText));

            //Assign an HTML Section for the String Text
            string htmlSectionID = "Sect1";

            // Create alternative format import part.
            AlternativeFormatImportPart formatImportPart = mainDocumenPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.Html, htmlSectionID);

            // Feed HTML data into format import part (chunk).
            formatImportPart.FeedData(ms);
            AltChunk altChunk = new AltChunk();
            altChunk.Id = htmlSectionID;

            //Clear out the Document Body and Insert just the HTML string.  (This prevents an empty First Line)
            mainDocumenPart.Document.Body.RemoveAllChildren();
            mainDocumenPart.Document.Body.Append(altChunk);

            /*
             Set the Page Orientation and Margins Based on Page Size
             inch equiv = 1440 (1 inch margin)
             */
            double width = 8.5 * 1440;
            double height = 11 * 1440;

            SectionProperties sectionProps = new SectionProperties();
            PageSize pageSize;
            if (isLandScape)
                pageSize = new PageSize() { Width = (UInt32Value)height, Height = (UInt32Value)width, Orient = PageOrientationValues.Landscape };
            else
                pageSize = new PageSize() { Width = (UInt32Value)width, Height = (UInt32Value)height, Orient = PageOrientationValues.Portrait };

            rMargin = rMargin * 1440;
            lMargin = lMargin * 1440;
            bMargin = bMargin * 1440;
            tMargin = tMargin * 1440;

            PageMargin pageMargin = new PageMargin() { Top = (Int32)tMargin, Right = (UInt32Value)rMargin, Bottom = (Int32)bMargin, Left = (UInt32Value)lMargin, Header = (UInt32Value)360U, Footer = (UInt32Value)360U, Gutter = (UInt32Value)0U };

            sectionProps.Append(pageSize);
            sectionProps.Append(pageMargin);
            mainDocumenPart.Document.Body.Append(sectionProps);

            //Saving/Disposing of the created word Document
            document.MainDocumentPart.Document.Save();
            Stream stream = document.MainDocumentPart.GetStream();
            UploadFileToDatalake(stream, "Clause Templat1_v1.0.0.5.docx", 12, 13);
            document.Dispose();
        }

        public List<ContentType> GetContentType()
        {
            try
            {
                return _DAO.GetContentType();
            }
            catch
            {
                throw;
            }
        }

    }
}
