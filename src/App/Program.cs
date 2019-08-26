using Bogus;
using MongoDbExample.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoDbExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var faker = new Faker();
            var vehicleRepo = new VehiclesRepository();

            // Delete All
            //vehicleRepo.DeleteAll();

            // Add
            var newVehicle = vehicleRepo.Add(new Vehicle
            {
                Manufacturer = faker.Vehicle.Manufacturer(),
                Model = faker.Vehicle.Model(),
                Year = faker.Date.Past(100).Year,
                Vin = faker.Vehicle.Vin(),
                Description = faker.Lorem.Lines(1)
            });
            Console.WriteLine($"Vehicle Added:\n{VehicleToString(newVehicle)}");

            // Update
            newVehicle.Description = faker.Lorem.Lines(1);
            VehiclesRepository.Update(newVehicle);
            Console.WriteLine("Vehicle Updated with new description\n");

            // Get By ID
            var vehicle = VehiclesRepository.GetById(newVehicle.Vin);
            if (vehicle != null)
                Console.WriteLine($"Get Vehicle By Id:\n{VehicleToString(vehicle)}");

            // Get All
            var vehicles = VehiclesRepository.GetAll();
            if (vehicles != null)
            {
                Console.WriteLine($"All Vehicles ({vehicles.Count}):");
                Console.WriteLine(VehiclesToString(vehicles));
            }

            // Delete
            var deletedCount = vehicleRepo.Delete(vehicle?.Vin);
            Console.WriteLine($"{deletedCount} vehicle(s) were deleted\n");

            // Get All
            vehicles = VehiclesRepository.GetAll();
            if (vehicles != null)
            {
                Console.WriteLine($"All Vehicles ({vehicles.Count}):");
                Console.WriteLine(VehiclesToString(vehicles));
            }

            Console.WriteLine("Press any key to terminate the program");
            Console.ReadKey();
        }

        private static string VehiclesToString(IEnumerable<Vehicle> vehicles)
        {
            var sb = new StringBuilder();

            foreach (var vehicle in vehicles)
            {
                sb.Append(VehicleToString(vehicle));
            }

            return sb.ToString();
        }

        private static string VehicleToString(Vehicle vehicle)
        {
            return $"Id: {vehicle.Id}\n" +
                $"Manufacturer: {vehicle.Manufacturer}\n" +
                $"Model: {vehicle.Model}\n" +
                $"Year: {vehicle.Year}\n" +
                $"VIN: {vehicle.Vin}\n" +
                $"Description: {vehicle.Description}\n\n";
        }
    }
}
