using System.Configuration;

namespace UserStorage.Factory.Infrastructure.CustomConfigSections
{
    public class StoragesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new StorageElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((StorageElement)element).Name;
        }
    }
}