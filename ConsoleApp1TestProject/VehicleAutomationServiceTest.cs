namespace ConsoleApp1TestProject
{
    using ConsoleApp1;

    public class VehicleAutomationServiceTest
    {
        private readonly VehicleAutomationService _vechicleAutomationService;
        private readonly InputParserService _inputParserService ;
        public VehicleAutomationServiceTest()
        {
            _vechicleAutomationService = new VehicleAutomationService();
            _inputParserService = new InputParserService();
        }

        [Fact]
        public void VehicleCannotMoveBelow0()
        {
            var map = new Map(10, 10);
            var vehicle = new Vehicle(string.Empty, new VehicleStatus(0, 0, Direction.S));
            var vehicleInstruction = new VehicleInstruction(vehicle, new List<Instruction>() { Instruction.F });
            var vehicleStatus = _vechicleAutomationService.CalculateVehicleStatus(map, vehicleInstruction);

            var expectedX = 0;
            var expectedY = 0;
            Assert.Equal(vehicleStatus.Last().X, expectedX);
            Assert.Equal(vehicleStatus.Last().Y, expectedY);
        }

        [Fact]
        public void VehicleCannotMoveAboveMap()
        {
            var map = new Map(10, 10);
            var vehicle = new Vehicle(string.Empty, new VehicleStatus(0, 9, Direction.N));
            var vehicleInstruction = new VehicleInstruction(vehicle, new List<Instruction>() { Instruction.F });
            var vehicleStatus = _vechicleAutomationService.CalculateVehicleStatus(map, vehicleInstruction);

            var expectedX = 0;
            var expectedY = 9;
            Assert.Equal(vehicleStatus.Last().X, expectedX);
            Assert.Equal(vehicleStatus.Last().Y, expectedY);
        }

        [Fact]
        public void VehicleCannotMoveLeftMap()
        {
            var map = new Map(10, 10);
            var vehicle = new Vehicle(string.Empty, new VehicleStatus(0, 0, Direction.W));
            var vehicleInstruction = new VehicleInstruction(vehicle, new List<Instruction>() { Instruction.F });
            var vehicleStatus = _vechicleAutomationService.CalculateVehicleStatus(map, vehicleInstruction);

            var expectedX = 0;
            var expectedY = 0;
            Assert.Equal(vehicleStatus.Last().X, expectedX);
            Assert.Equal(vehicleStatus.Last().Y, expectedY);
        }

        [Fact]
        public void VehicleCannotMoveRightMap()
        {
            var map = new Map(10, 10);
            var vehicle = new Vehicle(string.Empty, new VehicleStatus(9, 0, Direction.E));
            var vehicleInstruction = new VehicleInstruction(vehicle, new List<Instruction>() { Instruction.F });
            var vehicleStatus = _vechicleAutomationService.CalculateVehicleStatus(map, vehicleInstruction);

            var expectedX = 9;
            var expectedY = 0;
            Assert.Equal(vehicleStatus.Last().X, expectedX);
            Assert.Equal(vehicleStatus.Last().Y, expectedY);
        }

        [Fact]
        public void VehicleFollowSampleOutput1()
        {
            var inputs = new List<string>()
            {
                "10 10",
                "1 2 N",
                "FFRFFFRRLF",
            };
            var ADCInputs = _inputParserService.ParseADCInput(inputs);
          
            var vehicleStatus = _vechicleAutomationService.CalculateVehicleStatus(ADCInputs.Map, ADCInputs.VehicleInstructions.First());

            var expectedX = 4;
            var expectedY = 3;
            var expectedDirection = Direction.S;
            Assert.Equal(vehicleStatus.Last().X, expectedX);
            Assert.Equal(vehicleStatus.Last().Y, expectedY);
            Assert.Equal(vehicleStatus.Last().Direction, expectedDirection);
        }
    }
}