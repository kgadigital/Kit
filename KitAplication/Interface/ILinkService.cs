using KitAplication.Data;
using KitAplication.Models;

namespace KitAplication.Interface
{
    public interface ILinkService
    {
        Task<LinkModel> CreateLinkAsync(LinkModel link);
        Task<List<LinkModel>> GetLinksAsync();
        Task<LinkModel> GetLinkByIdAsync(int id);
        Task<LinkModel> UpdateLinkAsync(LinkModel link);
        Task DeleteLinkAsync(int id);
    }
}
