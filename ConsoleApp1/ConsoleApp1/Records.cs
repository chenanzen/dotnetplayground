using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public enum Direction : int
    {
        N = 0,
        E = 1,
        S = 2,
        W = 3
    }

    public enum Instruction : int
    {
        L = -1,
        R = +1,
        F = 0
    }

    public record ADCInputs(Map Map, List<VehicleInstruction> VehicleInstructions);
    public record Map(int Width, int Height);
    public record Vehicle(string VehicleName, VehicleStatus VehicleStatus);
    public record VehiclePath(string VehicleName, List<VehicleStatus> Path);
    public record VehicleInstruction(Vehicle Vehicle, List<Instruction> Instructions);
    public record VehicleStatus(int X, int Y, Direction Direction);
    public record VehicleCollision(int StepNo, List<Vehicle> CollidedVehicle);
}
