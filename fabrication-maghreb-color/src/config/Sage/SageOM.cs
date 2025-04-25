using System;
using fabrication_maghreb_color.Infrastructure.dto;
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
        readonly BSCPTAApplication100c BaseCpta = new BSCPTAApplication100c();
        public readonly ILogger<SageOM> _logger;


        public SageOM(IConfiguration? configuration, ILogger<SageOM> logger)
        {
            var settings = configuration.GetSection("SageSettings");
            _logger = logger;

            BaseCpta.Name = BaseCial.Name = settings["DatabasePath"];
            BaseCpta.Loggable.UserName
= BaseCial.Loggable.UserName = settings["Username"];
            BaseCpta.Loggable.UserPwd =
            BaseCial.Loggable.UserPwd = settings["Password"];
            BaseCial.CptaApplication = BaseCpta;

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

        public async Task<Dictionary<string, string>> CreateArticle(string description, string familyCode, Projet projet, chargeCompte charge)
        {

            string reference = GenerateArticleReference(); // Automatically generate reference

            IBOArticle3 article = (IBOArticle3)BaseCial.FactoryArticle.Create();

            article.AR_Ref = reference;
            article.AR_Design = description;
            article.AR_Nomencl = NomenclatureType.NomenclatureTypeFabrication;
            article.AR_SuiviStock = SuiviStockType.SuiviStockTypeCmup;

            IBOFamille3 famille = BaseCial.FactoryFamille.ReadCode(FamilleType.FamilleTypeDetail, familyCode);
            if (famille != null)
            {
                article.Famille = famille;  // Assign the family before saving
            }
            else
                throw new Exception("Invalid family code: " + familyCode);

            article.Write(); // Save to database
            string numbBc = CreerBonDeCommande(reference, projet, charge);

            return new Dictionary<string, string>
{
    { "Reference", reference },
    { "NumeroBC", numbBc }
};

        }
        public async Task CreateNomenclature(string projetRef, List<Matiere> matieres)
        {
            var mainArticle = BaseCial.FactoryArticle.ReadReference(projetRef); // Main article
            foreach (Matiere matiere in matieres)
            {
                var compArticle = BaseCial.FactoryArticle.ReadReference(matiere.ReferenceMP);
                IBOArticleNomenclature3 nomenclature = (IBOArticleNomenclature3)mainArticle.FactoryArticleNomenclature.Create();

                nomenclature.ArticleComposant = compArticle;
                nomenclature.NO_Qte = (double)matiere.QuantiteUtilise;
                nomenclature.NO_Type = ComposantType.ComposantTypeVariable;
                nomenclature.Write();
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

        private string GenererReferenceBonCommande()
        {
            return "BC" + DateTime.Now.ToString("yyyyMMddHHmmss");  // Simple, basé sur la date
        }

        public string CreerBonDeCommande(string codeArticle, Projet projet, chargeCompte chargecomp)
        {

            // Création du process de document "Bon de Commande Client"
            IPMDocument processusCommande = BaseCial.CreateProcess_Document(DocumentType.DocumentTypeVenteCommande);
            IBODocumentVente3 bonCommande = (IBODocumentVente3)processusCommande.Document;

            bonCommande.DO_Date = DateTime.Now;
            bonCommande.DO_Ref = GenererReferenceBonCommande();

            bonCommande.Collaborateur = BaseCpta.FactoryCollaborateur.ReadNomPrenom(chargecomp.nom, chargecomp.prenom);

            IBOClient3 client = BaseCpta.FactoryClient.ReadNumero(projet.ReferenceClient);  // Remplace CLIENTCODE par le vrai code.
            if (client == null)
            {
                _logger.LogError($"Client introuvable : CLIENTCODE");
                return "";
            }
            bonCommande.SetDefaultClient(client);
            // Chargement de l'article
            IBOArticle3 article = BaseCial.FactoryArticle.ReadReference(codeArticle);
            if (article == null)
                throw new Exception("Article introuvable : " + codeArticle);

            // Création de la ligne de commande
            IBODocumentLigne3 ligneCommande = (IBODocumentLigne3)bonCommande.FactoryDocumentLigne.Create();
            ligneCommande.SetDefaultArticle(article, (double)projet.quantite);
            ligneCommande.Write();

            // Validation via Process et non Write !
            if (processusCommande.CanProcess)
            {
                processusCommande.Process();
                _logger.LogInformation($"Bon de commande '{bonCommande.DO_Piece}' créé et validé.");
                return bonCommande.DO_Piece;
            }
            else
            {
                _logger.LogError("Le bon de commande n'est pas valide : échec de validation (CanProcess = false).");
                return "";
            }

        }


        public void CreateClient(Compte compte)
        {
            IBOClient3 client = (IBOClient3)BaseCpta.FactoryClient.Create();
            // Assign client properties
            client.CT_Num = compte.num;
            client.CT_Intitule = compte.intitule;
            client.CompteGPrinc = BaseCpta.FactoryCompteG.ReadNumero("34210000");
            client.Write();

            Console.WriteLine("Client successfully created!");
        }
        public async Task TransformDocument(DocumentDto document)
        {
            var docBC = BaseCial.FactoryDocumentVente.ReadPiece(DocumentType.DocumentTypeVenteCommande, document.reference);

            // Initialiser le processus de transformation 'Livrer'
            var transfo = BaseCial.Transformation.Vente.CreateProcess_Livrer();

            foreach (IBODocumentVenteLigne3 line in transfo.ListLignesATransformer)
            {

                line.DL_Qte = (double)document.Quantity;
            }

            // Ajouter le bon de commande au processus
            transfo.AddDocument(docBC);

            // Vérifier si le traitement peut être fait
            if (transfo.CanProcess)
            {
                transfo.Process();
                Console.WriteLine("Transformation effectuée avec succès !");
            }
            else
            {
                Console.WriteLine("Transformation impossible. Vérifiez les données du document.");
            }
        }

        public async Task<string> CreeBonFabrication(BonFabrication bon, chargeCompte collab)
        {


            if (bon.matieres == null)
            {
                bon.matieres = new List<Matiere>(); // Initialize to empty list
                _logger.LogWarning("Matieres list was null, initialized to empty list");
            }
            _logger.LogInformation("Starting document creation for stock fabrication.");

            // Create the process document for Stock Fabrication
            IPMDocument mProcessDoc = BaseCial.CreateProcess_Document(DocumentType.DocumentTypeStockFabrication);
            _logger.LogInformation("Process document created with type: DocumentTypeStockFabrication.");

            Console.WriteLine("machine :" + bon.machine.Label.ToString());
            // Assign a document number
            mProcessDoc.Document.DO_Piece = bon.Id.ToString();
            // mProcessDoc.DocumentResult.InfoLibre["Machine"]= 1;
            mProcessDoc.Document.InfoLibre["Numero_PF"] = bon.Pf_id.ToString();
            mProcessDoc.Document.InfoLibre["NombreBobins"] = bon.NombreBobins.ToString();
            mProcessDoc.Document.InfoLibre["Passe"] = bon.Passe.ToString();
            mProcessDoc.Document.InfoLibre["MetrageLineaire"] = bon.MetrageLineaire.ToString();
            mProcessDoc.Document.InfoLibre["Laize"] = bon.Laize.ToString();

            _logger.LogDebug("type infolibre:" + mProcessDoc.Document.InfoLibre.GetType());

            // Add the main article (the fabricated product) — Sage will auto-create nomenclature component lines
            IBOArticle3 mainArticle = BaseCial.FactoryArticle.ReadReference(bon.preparationFabrication.Projet.ReferenceArticle);
            IBODocumentStockLigne3 mLig = (IBODocumentStockLigne3)mProcessDoc.AddArticle(mainArticle, (double)bon.QuantiteFini);

            _logger.LogInformation($"Added main fabrication article: {mainArticle.AR_Ref} with quantity {bon.QuantiteFini}.");

            // Loop over all document lines to update component quantities
            foreach (IBODocumentStockLigne3 ligne in mProcessDoc.Document.FactoryDocumentLigne.List)
            {
                // Check if it's a component line (nomenclature child)
                if (ligne.ArticleCompose != null)
                {
                    string componentRef = ligne.Article.AR_Ref;
                    Console.WriteLine($"Processing component: {componentRef}.");
                    var matiere = bon.matieres.FirstOrDefault(e => e.ReferenceMP == componentRef);

                    if (matiere != null)
                    {
                        double oldQuantity = ligne.DL_Qte;
                        ligne.SetDefaultArticle(ligne.Article, (double)matiere.QuantiteUtilise);
                        ligne.Write();

                        Console.WriteLine($"✔ Updated {componentRef} quantity from {oldQuantity} to {matiere.QuantiteUtilise}.");
                    }
                    else
                    {
                        Console.WriteLine($"⚠ Component {componentRef} not found in your 'matieres' list — quantity left as {ligne.DL_Qte}.");
                    }
                }

            }

            _logger.LogInformation("All component lines reviewed and updated where matching 'matieres' were found.");

            // foreach (BonFile file in bon.files)
            // {
            //     Console.WriteLine("File path: " + Path.GetFullPath(file.FilePath));
            //     IBIMedia media = (IBIMedia)mProcessDoc.Document.FactoryDocumentMedia.Create();
            //     media.ME_Fichier = Path.GetFullPath(file.FilePath);
            //     media.Write();
            // }
            // Final document processing

            mProcessDoc.Process();
            return mProcessDoc.Document.DO_Piece;

        }
        public void UpdateBCQuantity(string refBC, double quantiy)
        {
            IBODocumentVente3 doc = (IBODocumentVente3)BaseCial.FactoryDocumentVente.ReadPiece(DocumentType.DocumentTypeVenteCommande, refBC);

            // Get the first line (index 1)
            IBODocumentVenteLigne3 ligne = (IBODocumentVenteLigne3)doc.FactoryDocumentLigne.List[1];

            // Update the quantity
            ligne.DL_Qte = quantiy;
            ligne.Write();

            // Save changes to the document
            doc.Write();
        }
    }
}
