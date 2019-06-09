using app.Security;

namespace app.Configuration
{
    public interface IConfigurationReader
    {
        AuthentificationConfiguration GetConfiguration();
    }
}