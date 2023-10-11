using KitAplication.Data;
using KitAplication.Interface;
using KitAplication.Models;

namespace KitAplication.Services
{
    public class LinkService : ILinkService
    {
        private readonly IRepository<LinkEntity> _linkRepository;

        public LinkService(IRepository<LinkEntity> linkRepository)
        {
            _linkRepository = linkRepository;
        }

        /// <summary>
        ///  Creates a new link asynchronously in the repository based on the provided LinkModel object, maps the created entity to a new LinkModel object, and returns the new object.
        /// </summary>
        /// <param name="model">The LinkModel object to be created in the repository.</param>
        /// <returns>An asynchronous operation that represents the newly created LinkModel object</returns>
        public async Task<LinkModel> CreateLinkAsync(LinkModel model)
        {
            var link = new LinkEntity
            {
                Name = model.Name,
                Url = model.Url
            };
            var linkentity = await _linkRepository.AddAsync(link);
            var linkmodel =new LinkModel { 
                Id= linkentity.Id,
                Name= linkentity.Name,
                Url= linkentity.Url,
            };
             
            return linkmodel;

        }

        /// <summary>
        /// Retrieves a list of LinkModel objects asynchronously from the _linkRepository
        /// </summary>
        /// <returns>A Task containing a List of LinkModel objects</returns>
        public async Task<List<LinkModel>> GetLinksAsync()
        {
            var links = await _linkRepository.GetAllAsync();
            var list = new List<LinkModel>();
            foreach(var link in links)
            {
                list.Add(new LinkModel
                {
                    Id = link.Id,
                    Name = link.Name,
                    Url = link.Url
                });
            }
            return list;
        }

        /// <summary>
        /// Retrieves a single LinkModel object asynchronously by ID from the _linkRepository
        /// </summary>
        /// <param name="id">The ID of the LinkModel object to retrieve</param>
        /// <returns>A Task containing a single LinkModel object, or null if not found</returns>
        public async Task<LinkModel> GetLinkByIdAsync(int id)
        {
            var link = await _linkRepository.GetByIdAsync(id);

            if (link == null)
            {
                return null;
            }

            return new LinkModel
            {
                Id = link.Id,
                Name = link.Name,
                Url = link.Url
            };
        }
        /// <summary>
        /// Updates a LinkModel object asynchronously in the _linkRepository
        /// </summary>
        /// <param name="model">The LinkModel object to update</param>
        /// <returns>A Task containing the updated LinkModel object</returns>
        public async Task<LinkModel> UpdateLinkAsync(LinkModel model)
        {
            var link = await _linkRepository.GetByIdAsync(model.Id);

            if (link != null)
            {
                link.Name = model.Name;
                link.Url = model.Url;

                await _linkRepository.UpdateAsync(link);
            }
            return model;
        }
        /// <summary>
        ///  Deletes a LinkModel object asynchronously by ID from the _linkRepository
        /// </summary>
        /// <param name="id">The ID of the LinkModel object to delete</param>
        /// <returns>A Task</returns>
        public async Task DeleteLinkAsync(int id)
        {
            var link = await _linkRepository.GetByIdAsync(id);

            if (link != null)
            {
               await  _linkRepository.DeleteAsync(link);
            }
        }
    }
}
