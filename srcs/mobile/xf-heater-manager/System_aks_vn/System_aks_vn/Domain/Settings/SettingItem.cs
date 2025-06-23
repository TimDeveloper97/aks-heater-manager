using System_aks_vn.Domain;

namespace System_aks_vn.Models.Settings
{
    public class SettingItem : BaseBinding
    {
        private string icon;

        public string Title { get; set; }

        public string Description { get; set; }

        public string Page { get; set; }

        public string Icon { get => icon; set => SetProperty(ref icon, value); }
    }
}