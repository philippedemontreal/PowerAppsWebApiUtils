using System;
using System.Net.Http;
using System.Net.Http.Headers;
using app.Configuration;
using app.Repositories;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace app
{
    class Program
    
    {

     static void Main(string[] args)
        {
            var contactRepository = new ContactRepository(new ConfigurationReader());
            var contact = contactRepository.GetById(Guid.Parse("6cc83de0-0e39-e911-a97c-000d3af490cc"), "firstname, lastname").Result;
            Console.WriteLine($"Hello World {contact.FirstName} {contact.LastName}!");
         }  
    }
}
