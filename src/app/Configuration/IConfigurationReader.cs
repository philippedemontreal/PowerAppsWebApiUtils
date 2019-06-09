using app.Foundation.Authentification;

namespace app.Configuration
{
    public interface IConfigurationReader
    {
        AuthentificationConfiguration GetConfiguration();
    }
}