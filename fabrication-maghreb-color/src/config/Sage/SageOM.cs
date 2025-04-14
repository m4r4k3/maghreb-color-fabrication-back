using System;
using fabrication_maghreb_color.Infrastructure.model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration; // Add this using directive
using Objets100cLib;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public async Task CreateNomenclature(string projetRef, List<Matiere> matieres)
        {
            var mainArticle = BaseCial.FactoryArticle.ReadReference(projetRef); // Main article
            Console.WriteLine("Main Article : " + projetRef.ToString());
            foreach (Matiere matiere in matieres)
            {
                var compArticle = BaseCial.FactoryArticle.ReadReference(matiere.ReferenceMP);
                IBOArticleNomenclature3 nomenclature = (IBOArticleNomenclature3)mainArticle.FactoryArticleNomenclature.Create();

                nomenclature.ArticleComposant = compArticle;
                nomenclature.NO_Qte = matiere.QuantiteUtilise;
                nomenclature.NO_Type = ComposantType.ComposantTypeVariable;
                nomenclature.Write();
                Console.WriteLine("Matiere Composant :" + matiere.ReferenceMP.ToString());
            }
            mainArticle.Write();
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



        public string CreeBonFabrication(BonFabrication bon, string client, List<Matiere> matieres)
        {
        try
{
    _logger.LogInformation("Starting document creation for stock fabrication.");

    // Create the process document for Stock Fabrication
    IPMDocument mProcessDoc = BaseCial.CreateProcess_Document(DocumentType.DocumentTypeStockFabrication);
    _logger.LogInformation("Process document created with type: DocumentTypeStockFabrication.");

    // Assign a document number
    mProcessDoc.Document.DO_Piece = bon.Id.ToString();

    // Add the main article (the fabricated product) — Sage will auto-create nomenclature component lines
    IBOArticle3 mainArticle = BaseCial.FactoryArticle.ReadReference(bon.preparationFabrication.Projet.ReferenceArticle);
    IBODocumentStockLigne3 mLig = (IBODocumentStockLigne3)mProcessDoc.AddArticle(mainArticle, (double)bon.quantite);

    _logger.LogInformation($"Added main fabrication article: {mainArticle.AR_Ref} with quantity {bon.quantite}.");

    // Loop over all document lines to update component quantities
    foreach (IBODocumentStockLigne3 ligne in mProcessDoc.Document.FactoryDocumentLigne.List)
    {
        // Check if it's a component line (nomenclature child)
        if (ligne.ArticleCompose != null)
        {
            string componentRef = ligne.Article.AR_Ref;
            Console.WriteLine($"Processing component: {componentRef}.");
            var matiere = matieres.FirstOrDefault(e => e.ReferenceMP == componentRef);

            if (matiere != null)
            {
                double oldQuantity = ligne.DL_Qte;
                ligne.SetDefaultArticle(ligne.Article, matiere.QuantiteUtilise);
                ligne.Write();

                Console.WriteLine($"✔ Updated {componentRef} quantity from {oldQuantity} to {matiere.QuantiteUtilise}.");
            }
            else
            {
                Console.WriteLine($"⚠ Component {componentRef} not found in your 'matieres' list — quantity left as {ligne.DL_Qte}.");
            }
        }
        else
        {
            // Log for non-component lines (main articles)
            //Console.WriteLine($"Line {ligne.AR_Ref} is not a component (probably the main article). Skipped.");
        }
    }

    _logger.LogInformation("All component lines reviewed and updated where matching 'matieres' were found.");

    // Final document processing
    if (mProcessDoc.CanProcess)
    {
        mProcessDoc.Process();
        _logger.LogInformation($"Document {mProcessDoc.Document.DO_Piece} processed successfully.");
        return mProcessDoc.Document.DO_Piece;
    }
    else
    {
        _logger.LogWarning("Document cannot be processed. Check for missing components or incomplete data.");
        return "";
    }
}
catch (Exception ex)
{
    _logger.LogError($"Error occurred: {ex.Message}. Stack Trace: {ex.StackTrace}");
    return "";
}

        }
    }
}
