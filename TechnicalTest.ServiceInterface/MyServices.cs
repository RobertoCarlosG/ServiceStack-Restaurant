using System;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using ServiceStack;
using ServiceStack.OrmLite;
using TechnicalTest.ServiceModel;

namespace TechnicalTest.ServiceInterface
{
    public class MyServices : Service
    {
        OrmLiteConnectionFactory dbFactory = new OrmLiteConnectionFactory(
                                                "Server=DESKTOP-HAQL8HN\\SQLEXPRESS;Database=Prueba;User Id=testing;Password=QWERTY1234;Encrypt=False;TrustServerCertificate=True;MultipleActiveResultSets=True;",
                                                SqlServerDialect.Provider);
        
        public object Any(Hello request)
        {
            return new HelloResponse { Result = $"Hello, {request.Name}!" };
        }
        public object Any(Restaurante restaurante)
        {
            using (var db = dbFactory.OpenDbConnection())
            {
                try {
                    var allRecords = db.Select<Restaurante>();
                    return new GetRestaurant() { Restaurantes = allRecords };
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                
            }
        }

        public object POST(Restaurante request)
        {
            try
            {
                using (var db = dbFactory.OpenDbConnection())
                {
                    // Begin a transaction
                    using (var trans = db.OpenTransaction())
                    {
                        try
                        {
                            var newRecord = new Restaurante
                            {
                                Name = request.Name,
                                Address = request.Address,
                                Telephone = request.Telephone,
                                Type = request.Type
                            };

                            var recordId = db.Select<Restaurante>(z => z.Name == request.Name
                                                                && z.Address == request.Address
                                                                && z.Telephone == request.Telephone
                                                                && z.Type == request.Type)
                                                                .FirstOrDefault();
                            if (recordId != null)
                            {
                                trans.Rollback();
                                return new RestaurantResponse { Message = "No se pueden ingresar dos restaurantes iguales", Code= 502 };
                            }

                            db.Insert(request);
                            trans.Commit();

                            return new RestaurantResponse { Message = $"Registro del restaurante {request.Name} completada", Code=200 };
                        }
                        
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            return new RestaurantResponse { Message = $"Transaction rolled back due to an exception: {ex.Message} ", Code = 503 };
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                return new RestaurantResponse { Message = $"Ocurrio un error: {ex.Message} ", Code = 500 };
            }
            
        }
        public object PUT(Restaurante request)
        {
            using (var db = dbFactory.OpenDbConnection())
            {
                using (var transaction = db.OpenTransaction())
                {
                    try
                    {

                        var recordId = db.Select<Restaurante>(z => z.Id == request.Id
                                                            || z.Name == request.Name 
                                                            || z.Address == request.Address 
                                                            || z.Telephone == request.Telephone)
                                                            .FirstOrDefault();
                        var recordToUpdate = db.SingleById<Restaurante>(recordId.Id);
                        if (recordToUpdate != null)
                        {
                            recordToUpdate.Name = request.Name;
                            recordToUpdate.Address = request.Address;
                            recordToUpdate.Telephone = request.Telephone;
                            recordToUpdate.Type = request.Type;

                            db.Update(recordToUpdate);

                            transaction.Commit();
                            return new RestaurantResponse{ Message = $"Actualizacion de  {request.Name} completada", Code = 200 };
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return new RestaurantResponse { Message = $"Transaction rolled back due to an exception: {ex.Message} ", Code = 502 };
                    }

                }
            }
            return new RestaurantResponse { Message = $"Actualizacion del restaurante {request.Name} completada", Code = 200 };
        }
        public object DELETE(Restaurante request)
        {
            using (var db = dbFactory.OpenDbConnection())
            {
                try
                {
                    var allRecords = db.Delete<Restaurante>(new { request.Name, request.Address,request.Telephone, request.Type });
                    return new RestaurantResponse() { Message = "Eliminacion completada",  Code = 200 };
                }
                catch (Exception ex)
                {
                    return new RestaurantResponse { Message = $"No se pudo eliminar el restaurante: {ex.Message} ", Code = 502 };
                }

            }
        }

    }
}
