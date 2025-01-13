using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{

    interface IInputParserService
    {
        ADCInputs ParseADCInput(List<string> inputs);
        Map ParseMapInput(string mapValue);
        Vehicle ParseVehicleInput(string vehicleName, string vehicleValue);
        List<Instruction> ParseInstructionInput(string instructionValue);
        Direction ParseDirection(string direction);
    }

    public class InputParserService: IInputParserService
    {

        public ADCInputs ParseADCInput(List<string> inputs)
        {
            var numOfLines = inputs.Count;

            if (numOfLines < 3)
            {
                // invalid input
                throw new ArgumentException("Input must contain exactly two integers separated by a space.");
            }
            else if (numOfLines == 3)
            {
                // part 1
                var map = ParseMapInput(inputs[0]);
                var vehicleInstructions = new List<VehicleInstruction>();

                var vehicle = ParseVehicleInput(string.Empty, inputs[1]);
                var instructions = ParseInstructionInput(inputs[2]);
                var vehicleInstruction = new VehicleInstruction(vehicle, instructions);
                vehicleInstructions.Add(vehicleInstruction);

                ADCInputs input = new ADCInputs(map, vehicleInstructions);
                return input;
            }
            else
            {
                // Part 2: 
                var map = ParseMapInput(inputs[0]);
                var vehicleInstructions = new List<VehicleInstruction>();

                // in part 2, each vehicle need 4 lines of instruction
                var divisibleBy4 = (numOfLines - 1) % 4;
                var numOfVehicles = (numOfLines - 1) / 4;
                if (divisibleBy4 == 0)
                {
                    for (int i = 0; i < numOfVehicles; i++)
                    {
                        var vehicle = ParseVehicleInput(inputs[2 + (i * 4)], inputs[3 + (i * 4)]);
                        var instructions = ParseInstructionInput(inputs[4 + (i * 4)]);
                        var vehicleInstruction = new VehicleInstruction(vehicle, instructions);
                        vehicleInstructions.Add(vehicleInstruction);
                    }
                }

                ADCInputs input = new ADCInputs(map, vehicleInstructions);
                return input;
            }
        }

        public Map ParseMapInput(string mapValue)
        {
            string[] parts = mapValue.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
            {
                throw new ArgumentException("Input must contain exactly two integers separated by a space.");
            }

            if (int.TryParse(parts[0], out int width) && int.TryParse(parts[1], out int height))
            {
                return new Map(width, height);
            }
            else
            {
                throw new ArgumentException("Input must contain valid integers.");
            }
        }

        public Vehicle ParseVehicleInput(string vehicleName, string vehicleValue)
        {
            string[] parts = vehicleValue.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 3)
            {
                throw new ArgumentException("Input must contain exactly three parts separated by a space.");
            }

            if (int.TryParse(parts[0], out int x) && int.TryParse(parts[1], out int y))
            {
                var directionStr = parts[2];
                Direction vehicleDirection;
                vehicleDirection = ParseDirection(directionStr);
                var vehicleInitialStatus = new VehicleStatus(x, y, vehicleDirection);
                return new Vehicle(vehicleName, vehicleInitialStatus);
            }
            else
            {
                throw new ArgumentException("Input must contain valid integers.");
            }
        }

        public List<Instruction> ParseInstructionInput(string instructionValue)
        {
            var instructions = new List<Instruction>();

            foreach (var ch in instructionValue)
            {
                if (Enum.TryParse<Instruction>(ch.ToString(), out var result))
                {
                    instructions.Add(result);
                }
                else
                {
                    throw new ArgumentException("Direction input must only be one of these value: L, R or F.");
                }
            }

            return instructions;
        }

        public Direction ParseDirection(string direction)
        {
            if (Enum.TryParse<Direction>(direction, out var result))
            {
                return result;
            }
            else
            {
                throw new ArgumentException("Direction input must only be one of these value: N, E, S or W.");
            }
        }
    }
}
