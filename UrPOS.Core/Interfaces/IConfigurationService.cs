using UrPOS.Core.Entities;

namespace UrPOS.Core.Interfaces
{
    public interface IConfigurationService
    {
        AppConfigurations GetConfigurations();
        bool SaveConfigrations(AppConfigurations config);
    }
}
