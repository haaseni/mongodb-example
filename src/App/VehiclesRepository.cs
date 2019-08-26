using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbExample.Models;
using System.Collections.Generic;

namespace MongoDbExample
{
    public class VehiclesRepository
    {
        private static IMongoCollection<Vehicle> _collection;

        public VehiclesRepository()
        {
            var mongoClient = new MongoClient();
            var database = mongoClient.GetDatabase("VehiclesDb");
            _collection = database.GetCollection<Vehicle>("Vehicles");
        }

        public Vehicle Add(Vehicle vehicle)
        {
            _collection.InsertOne(vehicle);

            return vehicle;
        }

        public static void Update(Vehicle vehicle)
        {
            var filter = Builders<Vehicle>.Filter.Eq("Vin", vehicle.Vin);

            _collection.FindOneAndUpdate<Vehicle>(
                filter,
                Builders<Vehicle>.Update.Set("Manufacturer", vehicle.Manufacturer)
                                        .Set("Model", vehicle.Model)
                                        .Set("Year", vehicle.Year)
                                        .Set("Vin", vehicle.Vin)
                                        .Set("Description", vehicle.Description));
        }

        public static Vehicle GetById(string vin)
        {
            var filter = Builders<Vehicle>.Filter.Eq("Vin", vin);
            var vehicle = _collection
                .Find(filter)
                .FirstOrDefault();

            return vehicle;
        }

        public static List<Vehicle> GetAll()
        {
            var vehicles = _collection.Find(new BsonDocument());

            return vehicles.ToList();
        }

        public long Delete(string vin)
        {
            if (string.IsNullOrWhiteSpace(vin))
                return 0;

            var filter = Builders<Vehicle>.Filter.Eq("Vin", vin);
            var deleteResult = _collection.DeleteMany(filter);

            return deleteResult.DeletedCount;
        }

        public void DeleteAll()
        {
            _collection.DeleteMany(new BsonDocument());
        }
    }
}
