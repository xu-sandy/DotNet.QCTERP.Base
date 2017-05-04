using System.Collections.Generic;

namespace Qct.Objects.ValueObjects
{
    public class MenuModel
    {
        public MenuModel() { }
        private List<MenuModel> _menuModels;
        public string Id { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string ParentId { get; set; }
        public int Level { get; set; }
        public List<MenuModel> Children
        {
            get { return _menuModels ?? (_menuModels = new List<MenuModel>()); }
            set { _menuModels = value; }
        }
    }
}
