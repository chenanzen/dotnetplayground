using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{

    public interface IInputOutputService
    {
        List<string> GetInput();
        void SendOuput(VehiclePath vehiclePath);
        void SendOuput(VehicleCollision? collision);

    }

    internal class ConsoleInputOutputService: IInputOutputService
    {

        public List<string> GetInput()
        {
            Console.WriteLine($"Enter 2x newline to complete input");
            Console.WriteLine($"Please enter input in following format");
            Console.WriteLine("{Width of Map} {Height of Map}");
            Console.WriteLine("{X point of vehicle} {Y point of vehicle} {Direction}");
            Console.WriteLine("{instruction}...");
            Console.WriteLine("Note:");
            Console.WriteLine("1. Width and Height of map must be integer larger than 0");
            Console.WriteLine("2. X and Y must be larger than 0, X must be smaller than Width, Y must be smaller than Height");
            Console.WriteLine("3. Direction follow 4 directions of compass: N, E, S, W");
            Console.WriteLine("4. Instructions consist of 1 letter instruction with total of 10 insructions.");

            bool keepReading = true;
            var inputs = new List<string>();
            string? input;
            while (keepReading)
            {
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    input = Console.ReadLine();
                    if (string.IsNullOrEmpty(input))
                    { break; }
                }
                inputs.Add(input);
            }

            return inputs;
        }

        public void SendOuput(VehiclePath vehiclePath)
        {
            var lastPosition = vehiclePath?.Path?.LastOrDefault();
            if (lastPosition != null)
            {
                Console.WriteLine($"{lastPosition.X} {lastPosition.Y} {lastPosition.Direction.ToString()}");
            }
            else
            {
                Console.WriteLine("Vehicle path is not calculated correctly.");
            }
        }

        public void SendOuput(VehicleCollision? collision)
        {
            if (collision?.CollidedVehicle != null && collision.CollidedVehicle.Count > 0)
            {
                var vehicleNames = string.Join(" ", collision.CollidedVehicle.SelectMany(v => v.VehicleName));
                var vehicleStat = collision.CollidedVehicle.First().VehicleStatus;
                Console.WriteLine(vehicleNames);
                Console.WriteLine($"{vehicleStat.X} {vehicleStat.Y}");
                Console.WriteLine($"{collision.StepNo}");
            }
            else
            {
                Console.WriteLine("no collision");
            }
        }
    }


}
