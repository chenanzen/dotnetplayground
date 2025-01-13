using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal sealed class ADCRunner(
    IInputOutputService inputService,
    IInputParserService inputParserService,
    IVehicleAutomationService vehicleAutomationService)
    {
        private readonly IInputOutputService inputService = inputService;
        private readonly IInputParserService inputParserService = inputParserService;
        private readonly IVehicleAutomationService vehicleAutomationService = vehicleAutomationService;

        public void ADCRunner()
        {
            var inputs = inputService.GetInput();
            var ADCInputs = inputParserService.ParseADCInput(inputs);


            // process ADCInput
            if (ADCInputs.VehicleInstructions != null && ADCInputs.VehicleInstructions.Count > 0) 
            { 
                if (ADCInputs.VehicleInstructions.Count == 1)
                {
                    var vehicleStatuses = vehicleAutomationService.CalculateVehicleStatus(ADCInputs.Map, ADCInputs.VehicleInstructions);
                    var output = 
                }
                else
                {

                }
            }
            foreach (var vehicleInstructions in ADCInputs.VehicleInstructions)
            {
            }
        }
    }
}
