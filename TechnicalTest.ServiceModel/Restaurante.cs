using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ServiceStack;
using ServiceStack.DataAnnotations;

namespace TechnicalTest.ServiceModel
{
    
    [Route("/restaurant")]
    [Route("/restaurant/{Name}")]
    public class Restaurante 
    {
        [AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Type { get; set; }
    }
   
    public class QueryRestaurant : QueryDb<Restaurante>
    {
        public string[] names { get; set; }
    }
    public class GetRestaurant 
    {
        public List<Restaurante> Restaurantes { get; set; }
    }

    public class RestaurantResponse
    {
        public string Message { get; set; }
    }

    public class CreateRestaurant : ICreateDb<Restaurante>, IReturn<Restaurante>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Type { get; set; }
    }

    public class UpdateRestaurant : IUpdateDb<Restaurante>, IReturn<Restaurante>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Type { get; set; }
    }
    public class DeleteRestaurant
    {
        public string Confirmed { get; set; }
    }

}
