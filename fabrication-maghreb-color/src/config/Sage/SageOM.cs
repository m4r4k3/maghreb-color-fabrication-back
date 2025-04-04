using System;
using fabrication_maghreb_color.Infrastructure.model;
using Microsoft.Extensions.Configuration; // Add this using directive
using Objets100cLib;

namespace fabrication_maghreb_color.Config.Sage
{
    public class SageOM
    {
        readonly BSCIALApplication100c BaseCial = new BSCIALApplication100c(); // Initialize BaseCial
        public readonly ILogger<SageOM> _logger;


        public SageOM(IConfiguration? configuration, ILogger<SageOM> logger)
        {
            var settings = configuration.GetSection("SageSettings");
            _logger = logger;

            BaseCial.Name = settings["DatabasePath"];
            BaseCial.Loggable.UserName = settings["Username"];
            BaseCial.Loggable.UserPwd = settings["Password"];
            try
            {
                _logger.LogInformation("Connecting ...");
                BaseCial.Open();
                _logger.LogInformation("Connected to Sage 100c successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error connecting to Sage 100c: " + ex.Message);
            }
        }

        public void CloseConnection()
        {
            BaseCial.Close();
            _logger.LogInformation("Connection closed.");
        }

        public string CreateArticle(string description, string familyCode)
        {
            try
            {
                string reference = GenerateArticleReference(); // Automatically generate reference

                IBOArticle3 article = (IBOArticle3)BaseCial.FactoryArticle.Create();

                article.AR_Ref = reference;
                article.AR_Design = description;
                article.AR_Nomencl = NomenclatureType.NomenclatureTypeFabrication;

                IBOFamille3 famille = BaseCial.FactoryFamille.ReadCode(FamilleType.FamilleTypeDetail, familyCode);
                if (famille != null)
                {
                    article.Famille = famille;  // Assign the family before saving
                }
                else
                    throw new Exception("Invalid family code: " + familyCode);

                article.Write(); // Save to database

                return reference;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
                return "";
            }
        }
        private string GenerateArticleReference()
        {
            try
            {
                IBICollection articles = BaseCial.FactoryArticle.List; // Get all articles
                int maxNumber = 0;

                foreach (IBOArticle3 article in articles)
                {
                    string refCode = article.AR_Ref;

                    // Filter only references that start with "PF"
                    if (refCode.StartsWith("PF") && int.TryParse(refCode.Replace("PF", ""), out int num))
                    {
                        if (num > maxNumber)
                            maxNumber = num;
                    }
                }

                // Generate the new reference by incrementing the highest found
                return "PF" + (maxNumber + 1).ToString("D6");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching last reference: " + ex.Message);
                return "PF000001"; // Default in case of error
            }
        }



        public string CreeBonFabrication(BonFabrication bon, string client , List<Matiere> matieres)
        {
            try
            {
                _logger.LogInformation("Starting document creation for stock fabrication.");
                IPMDocument mProcessDoc = BaseCial.CreateProcess_Document(DocumentType.DocumentTypeStockFabrication);
                _logger.LogInformation("Process document created with type: DocumentTypeStockFabrication.");

                mProcessDoc.Document.DO_Piece = bon.Id.ToString();
                //IBODocumentStock3 bonSage = (IBODocumentStock3)BaseCial.FactoryDocumentStock.CreateType(DocumentType.DocumentTypeStockFabrication);
                //bonSage.DO_Date = bon.DateCreation;
                //_logger.LogInformation($"Created stock document with Date: {bon.DateCreation}, Piece ID: {bon.Id}");

                // Adding the first article (product being fabricated)
                IBODocumentStockLigne3 mLig = (IBODocumentStockLigne3)mProcessDoc.AddArticle(BaseCial.FactoryArticle.ReadReference(bon.preparationFabrication.Projet.ReferenceArticle), (double)bon.preparationFabrication.Projet.quantite);
                
                mLig.SetDefaultArticle(BaseCial.FactoryArticle.ReadReference(bon.preparationFabrication.Projet.ReferenceArticle), (double)bon.preparationFabrication.Projet.quantite);
                mLig.ArticleCompose = BaseCial.FactoryArticle.ReadReference(bon.preparationFabrication.Projet.ReferenceArticle);
                mLig.Write();
                _logger.LogInformation($"Added article for fabrication: {bon.preparationFabrication.Projet.ReferenceArticle} with quantity {bon.preparationFabrication.Projet.quantite}");

                // MP000001 is a consumed article, so we track its consumption
                
                foreach ( Matiere materiel in matieres)
                {
                    IBODocumentStockLigne3 cLig = (IBODocumentStockLigne3)mProcessDoc.AddArticle(BaseCial.FactoryArticle.ReadReference(materiel.ReferenceMP),(double) materiel.QuantiteUtilise);
                    cLig.SetDefaultArticle(BaseCial.FactoryArticle.ReadReference(materiel.ReferenceMP), (double)materiel.QuantiteUtilise);
                    cLig.ArticleCompose = BaseCial.FactoryArticle.ReadReference(materiel.ReferenceMP);
                    cLig.Write();
                    _logger.LogInformation($"Added consumed article: {materiel.ReferenceMP} with quantity {materiel.QuantiteUtilise}");
                }
             
                _logger.LogInformation("Added consumed article: MP000001 with quantity 69.");

                // Optional: Handling composed article creation (commented-out)
                    /*
                IBIPersistObject DocLigneCompose = bonSage.FactoryDocumentLigne.Create();
                DocLigneCompose.SetDefaultArticle(BaseCial.FactoryArticle.ReadReference(bon.preparationFabrication.Projet.ReferenceArticle), (double)bon.preparationFabrication.Projet.quantite);
                DocLigneCompose.ArticleCompose = bCial.FactoryArticle.ReadReference("MP000001");
                DocLigneCompose.Write();
                _logger.LogInformation("Composed article creation (optional) processed.");
                */

                //bonSage.WriteDefault();
                _logger.LogInformation("Stock document finalized and written.");

                // Process the document if ready
                if (mProcessDoc.CanProcess)
                {
                    mProcessDoc.Process();
                    _logger.LogInformation("Document processed successfully.");
                    return mProcessDoc.Document.DO_Piece
;
                }

                _logger.LogWarning("Document cannot be processed.");
                return "";
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred: {ex.Message}. Stack Trace: {ex.StackTrace}");
                return "";
            }

        }
    }
}
