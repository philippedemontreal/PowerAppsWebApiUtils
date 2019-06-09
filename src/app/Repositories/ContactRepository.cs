using app.Configuration;
using app.Entities;

namespace app.Repositories
{
    public class ContactRepository : GenericRepository<Contact>
    {
        public ContactRepository(IConfigurationReader configurationReader) : 
        base(configurationReader, "contacts")
        {
        }
    }
}