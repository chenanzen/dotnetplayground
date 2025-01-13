using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    interface IVehicleAutomationService
    {
        List<VehicleStatus> CalculateVehicleStatus(Map map, VehicleInstruction vehicleInstruction);
    }
    public class VehicleAutomationService : IVehicleAutomationService
    {
        public List<VehicleStatus> CalculateVehicleStatus(Map map, VehicleInstruction vehicleInstruction)
        {
            var vehicleStatus = new List<VehicleStatus>()
            {
                vehicleInstruction.Vehicle.VehicleInitialStatus
            };

            var currentLocationX = vehicleInstruction.Vehicle.VehicleInitialStatus.X;
            var currentLocationY = vehicleInstruction.Vehicle.VehicleInitialStatus.Y;
            var currentDirection = vehicleInstruction.Vehicle.VehicleInitialStatus.Direction;
            foreach (var instruction in vehicleInstruction.Instructions)
            {
                // run instruction
                switch (instruction)
                {
                    case Instruction.L:
                    case Instruction.R:
                        var newDirectionValue = (int)currentDirection + (int)instruction;
                        if (newDirectionValue < 0) newDirectionValue += 4;
                        else if (newDirectionValue >= 4) newDirectionValue -= 4;
                        currentDirection = (Direction)newDirectionValue;
                        break;

                    case Instruction.F:
                        var newLocationX = currentLocationX;
                        var newLocationY = currentLocationY;
                        switch (currentDirection)
                        {
                            case Direction.N:
                                newLocationY += 1;
                                // cannot move beyond map
                                if (newLocationY >= map.Height) newLocationY = currentLocationY;
                                break;

                            case Direction.E:
                                newLocationX += 1;
                                // cannot move beyond map
                                if (newLocationX >= map.Width) newLocationX = currentLocationX;
                                break;

                            case Direction.S:
                                newLocationY -= 1;
                                // cannot move beyond map
                                if (newLocationY < 0) newLocationY = currentLocationY;
                                break;

                            case Direction.W:
                                newLocationX -= 1;
                                // cannot move beyond map
                                if (newLocationX < 0) newLocationX = currentLocationX;
                                break;
                        }
                        // update vehicle location
                        currentLocationX = newLocationX;
                        currentLocationY = newLocationY;
                        break;

                    default:
                        // do nothing
                        break;
                }

                vehicleStatus.Add(new VehicleStatus(currentLocationX, currentLocationY, currentDirection));
            }

            return vehicleStatus;
        }

        public void CollionCheck(List<List<VehicleStatus>> vehiclesStatuses)
        {
            var numOfVehicles = vehiclesStatuses.Count;
            var totalNumOfTeps = vehiclesStatuses.Max(vs => vs.Count);

            for (var i = 0; i < totalNumOfTeps; i++)
            {
                // get location at step i

                foreach (var vehicleStatus in vehiclesStatuses)
                {
                    if (vehicleStatus.Count > i)
                    {

                    }
                    else
                    {

                    }
                    vehicleStatus[i]
                }
            }
    }
}
