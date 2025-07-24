using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEBAPI_m1IL_1.Models;

namespace WEBAPI_m1IL_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdsController(AdsContext context) : ControllerBase
    {
        private readonly AdsContext _context = context;

        #region CRUD
        /// <summary>
        /// Retourne les annonces
        /// </summary>
        /// <returns>Liste des annonces</returns>
        // GET: api/Ads
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdsItems>>> GetAdsItems()
        {
            return await _context.AdsItems.ToListAsync();
        }

        /// <summary>
        /// Retourne une annonce précise par son identifiant.
        /// </summary>
        /// <param name="id">ID de l'annonce</param>
        /// <returns>Détails de l'annonce</returns>
        // GET: api/Ads/5
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AdsItems>> GetAdsItems(int id)
        {
            var adsItems = await _context.AdsItems.FindAsync(id);

            if (adsItems == null)
            {
                return NotFound();
            }

            return adsItems;
        }

        /// <summary>
        /// Modification d'une annonce existance
        /// </summary>
        /// <param name="id">ID de l'annonce</param>
        /// <param name="adsItems">Données modifiées</param>
        /// <returns>Réponse de mise à jour</returns>
        // PUT: api/Ads/5
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutAdsItems(int id, AdsItems adsItems)
        {
            if (id != adsItems.Id)
            {
                return BadRequest();
            }

            _context.Entry(adsItems).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdsItemsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Crée une nouvelle annonce.
        /// </summary>
        /// <param name="adsItems">Données de la nouvelle annonce</param>
        /// <returns>Annonce créée</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<AdsItems>> PostAdsItems(AdsItems adsItems)
        {
            adsItems.UserId = 1;

            _context.AdsItems.Add(adsItems);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdsItems", new { id = adsItems.Id, userId = 1 }, adsItems);
        }

        /// <summary>
        /// Supprime une annonce existante.
        /// </summary>
        /// <param name="id">ID de l'annonce</param>
        /// <returns>Réponse de suppression</returns>
        // DELETE: api/Ads/5
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAdsItems(int id)
        {
            var adsItems = await _context.AdsItems.FindAsync(id);
            if (adsItems == null)
            {
                return NotFound();
            }

            _context.AdsItems.Remove(adsItems);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdsItemsExists(int id)
        {
            return _context.AdsItems.Any(e => e.Id == id);
        }
        #endregion

        #region Autres fonctionnalités
        /// <summary>
        /// Permet de faire une recherche filtrée en fonction de mot clé, de la catégorie ou de la localisation
        /// </summary>
        /// <param name="keyword">Mot-clé à rechercher</param>
        /// <param name="category">Catégorie</param>
        /// <param name="location">Localisation</param>
        /// <returns>Liste des annonces correspondant aux critères</returns>
        /// <returns></returns>
        // GET: api/Ads/search?keyword=perceuse&category=outillage&location=Nantes
        [Authorize]
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<AdsItems>>> SearchAds(
            string? keyword = null,
            string? category = null,
            string? location = null)
        {
            var query = _context.AdsItems.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var loweredKeyword = keyword.ToLower();
                query = query.Where(a => a.Title.Contains(keyword) || a.Description.Contains(keyword));
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                var loweredCategory = category.ToLower();
                query = query.Where(a => a.Category.Contains(category));
            }

            if (!string.IsNullOrWhiteSpace(location))
            {
                var loweredLocation = location.ToLower();
                query = query.Where(a => a.Location.Contains(location));
            }

            var results = await query.ToListAsync();

            if (results.Count == 0)
                return NotFound("Aucune annonce ne correspond à votre recherche.");

            return Ok(results);
        }

        /// <summary>
        /// Génère la facture PDF pour une annonce donnée.
        /// </summary>
        /// <param name="adId">ID de l'annonce</param>
        /// <returns>Fichier PDF de la facture</returns>
        [HttpGet("invoice/{adId}")]
        [Authorize]
        public async Task<IActionResult> GetInvoicePdf(int adId)
        {
            // Récupération de l'annonce par ID
            AdsItems? ad = await _context.AdsItems.FindAsync(adId);
            if (ad == null)
                return NotFound("Annonce introuvable");

            // Générer le PDF dans un MemoryStream (plus besoin d'écrire sur disque)
            var pdfStream = PdfService.GenerateInvoicePdf(ad);

            // Renvoi du fichier PDF en réponse HTTP
            return File(pdfStream, "application/pdf", $"facture_{ad.Id}.pdf");
        }
        #endregion
    }
}
