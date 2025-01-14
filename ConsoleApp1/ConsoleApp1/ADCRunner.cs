using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal sealed class ADCRunner(
    IInputOutputService inputOutputService,
    IInputParserService inputParserService,
    IVehicleAutomationService vehicleAutomationService)
    {
        private readonly IInputOutputService inputOutputService = inputOutputService;
        private readonly IInputParserService inputParserService = inputParserService;
        private readonly IVehicleAutomationService vehicleAutomationService = vehicleAutomationService;

        public void ADCRunnerExecute()
        {
            var inputs = inputOutputService.GetInput();
            var ADCInputs = inputParserService.ParseADCInput(inputs);

            // process ADCInput
            if (ADCInputs.VehicleInstructions != null && ADCInputs.VehicleInstructions.Count > 0) 
            { 
                if (ADCInputs.VehicleInstructions.Count == 1)
                {
                    var vehicleStatuses = vehicleAutomationService.CalculateVehiclePath(ADCInputs.Map, ADCInputs.VehicleInstructions.First());
                    inputOutputService.SendOuput(vehicleStatuses);
                }
                else
                {
                    var map = ADCInputs.Map;
                    var allVehiclePath = new List<VehiclePath>();
                    foreach(var vehicleInstruction in ADCInputs.VehicleInstructions)
                    {
                        var vehiclePath = vehicleAutomationService.CalculateVehiclePath(map, vehicleInstruction);
                        allVehiclePath.Add(vehiclePath);
                    }

                    var vehicleStatuses = vehicleAutomationService.CollionCheck(allVehiclePath);
                    inputOutputService.SendOuput(vehicleStatuses);
                }
            }
        }
    }
}
