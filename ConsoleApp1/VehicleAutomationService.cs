using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    interface IVehicleAutomationService
    {
        VehiclePath CalculateVehiclePath(Map map, VehicleInstruction vehicleInstruction);
        VehicleCollision? CollionCheck(List<VehiclePath> vehiclesPath);
    }

    public class VehicleAutomationService : IVehicleAutomationService
    {
        public VehiclePath CalculateVehiclePath(Map map, VehicleInstruction vehicleInstruction)
        {
            var vehiclePath = new VehiclePath(
                vehicleInstruction.Vehicle.VehicleName,
                new List<VehicleStatus>()
                {
                    vehicleInstruction.Vehicle.VehicleStatus
                });

            var currentLocationX = vehicleInstruction.Vehicle.VehicleStatus.X;
            var currentLocationY = vehicleInstruction.Vehicle.VehicleStatus.Y;
            var currentDirection = vehicleInstruction.Vehicle.VehicleStatus.Direction;
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

                vehiclePath.Path.Add(new VehicleStatus(currentLocationX, currentLocationY, currentDirection));
            }

            return vehiclePath;
        }

        public VehicleCollision? CollionCheck(List<VehiclePath> vehiclesPath)
        {
            var numOfVehicles = vehiclesPath.Count;
            var totalNumOfSteps = vehiclesPath.Max(vs => vs.Path.Count);
            for (var i = 0; i < totalNumOfSteps; i++)
            {
                // get location at step i
                var vehiclesStatusAtPositioni = vehiclesPath.Select(vs => GetVehicleStatusAtPosition(vs, i)).ToList();

                var collions = vehiclesStatusAtPositioni.GroupBy(p => new { p.VehicleStatus.X, p.VehicleStatus.Y }).Where(g => g.Count() > 1).SelectMany(g => g).ToList();
                if (collions.Any())
                {
                    var collisionDetail = new VehicleCollision(i, collions);
                    return collisionDetail;
                }
            }

            return null;
        }

        public VehicleCollision? NoCarSwapOnStep(List<VehiclePath> vehiclesPath)
        {
            var totalNumOfSteps = vehiclesPath.Max(vs => vs.Path.Count);
            for (var i = 0; i < totalNumOfSteps; i++)
            {
                if (i == 0)
                {
                    continue; 
                }
                else
                {
                    foreach(var vehicle in vehiclesPath)
                    {
                        // position of vehicle at step i
                        var vehicleStatusAtPositioni = GetVehicleStatusAtPosition(vehicle, i);

                        // all vehicles
                        var vehiclesStatusAtPositioniminus = vehiclesPath.Select(vs => GetVehicleStatusAtPosition(vs, i - 1)).ToList();
                        var possibleSwapVehicles = vehiclesStatusAtPositioniminus.Where(v =>
                            v.VehicleStatus.X == vehicleStatusAtPositioni.VehicleStatus.X
                            && v.VehicleStatus.Y == vehicleStatusAtPositioni.VehicleStatus.Y).ToList();

                        if (possibleSwapVehicles.Any())
                        {
                            var posibleSwapVechicle = possibleSwapVehicles.First();

                            var pathOfPossibleSwapVehicle = vehiclesPath.Where(vp => vp.VehicleName == posibleSwapVechicle.VehicleName).First();
                            
                            var vehicleStatusAtPositioniminus = GetVehicleStatusAtPosition(pathOfPossibleSwapVehicle, i - 1);

                            if (vehicleStatusAtPositioniminus.VehicleStatus.X == vehicleStatusAtPositioniminus.VehicleStatus.X
                                    && vehicleStatusAtPositioniminus.VehicleStatus.Y == vehicleStatusAtPositioniminus.VehicleStatus.Y)
                            {
                                var collision = new List<Vehicle>()
                                    {
                                        vehicleStatusAtPositioni,
                                        vehicleStatusAtPositioniminus,
                                    };
                                var collisionDetail = new VehicleCollision(i, collision);
                                return collisionDetail;
                            }


                            //foreach (var possibleSwapVehicle in possibleSwapVehicles)
                            //{
                            //    possibleSwapVehicle.
                            //    var possibleSwapVehiclePosAti = GetVehicleStatusAtPosition(possibleSwapVehicle, i);
                            //    if (possibleSwapVehiclePosAti.VehicleStatus.X == vehicleStatusAtPositioniminus.VehicleStatus.X
                            //        && possibleSwapVehiclePosAti.VehicleStatus.Y == vehicleStatusAtPositioniminus.VehicleStatus.Y)
                            //    {
                            //        var collision = new List<Vehicle>()
                            //        {
                            //            vehicleStatusAtPositioni,
                            //            possibleSwapVehiclePosAti,
                            //        };
                            //        var collisionDetail = new VehicleCollision(i, collision);
                            //        return collisionDetail;
                            //    }

                            //}
                        }
                    }

                    // check position before, if they swap
                    //var vehiclesStatusAtPositioni = vehiclesPath.Select(vs => GetVehicleStatusAtPosition(vs, i)).ToList();




                }

                // get location at step i


            }


            return null; 
        }

        public Vehicle GetVehicleStatusAtPosition(VehiclePath vehiclePath, int position) 
        {
            if (vehiclePath.Path == null || vehiclePath.Path.Count == 0) throw new ArgumentNullException("Vehicle path is not initiated properly.");
            else 
            {
                VehicleStatus result = vehiclePath.Path.Last();
                if (vehiclePath.Path.Count > position)
                    result = vehiclePath.Path[position];

                return new Vehicle(vehiclePath.VehicleName, new VehicleStatus(result.X, result.Y, result.Direction));
            }
        }
    }
}
