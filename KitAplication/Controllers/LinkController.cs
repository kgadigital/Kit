using KitAplication.Data;
using KitAplication.Interface;
using KitAplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KitAplication.Controllers
{
    [Authorize]
    public class LinkController : Controller
    {
        private readonly ILinkService _linkService;

        public LinkController(ILinkService linkService)
        {
            _linkService = linkService;
        }

        public async Task<IActionResult> Index()
        {
            var links = await _linkService.GetLinksAsync();
            return View(links);
        }
        // GET: Link/Details/5
        public async Task<IActionResult> Details(int id)
        {

            var link = await _linkService.GetLinkByIdAsync(id);
            if (link == null)
                return NotFound();
            else
                return View(link);
        }
        // GET: Link/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Url")] LinkModel model)
        {
            if (ModelState.IsValid)
            {
                var link =await _linkService.CreateLinkAsync(model);
                if(link!=null)
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        public async Task<IActionResult> Edit(int id)
        {

            var link = await _linkService.GetLinkByIdAsync(id);
            if (link == null)
            {
                return NotFound("Link dosnt exist");
            }
            return View(link);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Url")] LinkModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _linkService.UpdateLinkAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Link/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            
            var linkEntity = await _linkService.GetLinkByIdAsync(id);
            if (linkEntity == null)
            {
                return NotFound("Link not found");
            }

            return View(linkEntity);
        }

        // POST: Link/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
          
            var linkEntity = await _linkService.GetLinkByIdAsync(id);
            if (linkEntity != null)
            {
               await _linkService.DeleteLinkAsync(linkEntity.Id);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
