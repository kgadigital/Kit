namespace KitAplication.Models.ViewModels
{
    public class IndexViewModel
    {
        public int RouteValueSelectSystem { get; set; } = 0;
        public SystemModel? SystemModel { get; set; }
        public MessageModel? MessageModel { get; set; }
        //public SystemModel? CreateSystem { get; set; }
        //public MessageModel? MessageModel { get; set; }
        //public SystemModel? UpdateSystem { get; set; }
        public SystemActiveModel SystemActive { get; set; }
    }
}
