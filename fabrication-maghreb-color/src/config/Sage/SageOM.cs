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



        public bool CreeBonFabrication(BonFabrication bon, string client)
        {
            try
            {
                IPMDocument mProcessDoc = BaseCial.CreateProcess_Document(DocumentType.DocumentTypeStockFabrication);
                IBODocumentStock3 bonSage = (IBODocumentStock3)BaseCial.FactoryDocumentStock.CreateType(DocumentType.DocumentTypeStockFabrication);
                bonSage.DO_Date = bon.DateCreation;
                bonSage.DO_Piece = bon.Id.ToString();
                IBODocumentStockLigne3 mLig = (IBODocumentStockLigne3)mProcessDoc.AddArticle(BaseCial.FactoryArticle.ReadReference(bon.preparationFabrication.Projet.ReferenceArticle), (double)bon.preparationFabrication.Projet.quantite);

                IBODocumentStockLigne3 cLig = (IBODocumentStockLigne3)mProcessDoc.(BaseCial.FactoryArticle.ReadReference("MP000001"), (double)-1000);

                // hadi cration d article composé
                IBODocumentVenteLigne3 DocLigneCompose = DocEntete.FactoryDocumentLigne.Create();
                DocLigneCompose.SetDefaultArticle(bCial.FactoryArticle.ReadReference(bon.preparationFabrication.Projet.ReferenceArticle), (double)bon.preparationFabrication.Projet.quantite);
                DocLigneCompose.ArticleCompose = bCial.FactoryArticle.ReadReference("MP000001");
                DocLigneCompose.Write();

                // hadi les articles composants
                foreach (var mIboArtItem in bCial.FactoryArticle.ReadReference("ENSHF").FactoryArticleNomenclature.List)
                {
                    IBODocumentVenteLigne3 DocLigneComposant = DocEntete.FactoryDocumentLigne.Create();
                    DocLigneComposant.SetDefaultArticle(mIboArtItem.ArticleComposant, mIboArtItem.NO_Qte);
                    DocLigneComposant.ArticleCompose = mIboArtItem.Article;

                    // Ajout de la remise
                    DocLigneComposant.Remise.Remise(1).REM_Type = RemiseType.RemiseTypePourcent;
                    DocLigneComposant.Remise.Remise(1).REM_Valeur = dRemise;

                    DocLigneComposant.Write();
                }

                //logger.LogAsync($"Article {item.Reference} ajouté au document d'achat.");

                bonSage.WriteDefault();

                if (mProcessDoc.CanProcess)
                {
                    mProcessDoc.Process();
                    return true;
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
                return false;
            }
        }
    }
}
