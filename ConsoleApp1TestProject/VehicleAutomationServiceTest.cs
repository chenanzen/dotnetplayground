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
            var vehiclePath = _vechicleAutomationService.CalculateVehiclePath(map, vehicleInstruction);

            var expectedX = 0;
            var expectedY = 0;
            Assert.Equal(vehiclePath.Path.Last().X, expectedX);
            Assert.Equal(vehiclePath.Path.Last().Y, expectedY);
        }

        [Fact]
        public void VehicleCannotMoveAboveMap()
        {
            var map = new Map(10, 10);
            var vehicle = new Vehicle(string.Empty, new VehicleStatus(0, 9, Direction.N));
            var vehicleInstruction = new VehicleInstruction(vehicle, new List<Instruction>() { Instruction.F });
            var vehiclePath = _vechicleAutomationService.CalculateVehiclePath(map, vehicleInstruction);

            var expectedX = 0;
            var expectedY = 9;
            Assert.Equal(vehiclePath.Path.Last().X, expectedX);
            Assert.Equal(vehiclePath.Path.Last().Y, expectedY);
        }

        [Fact]
        public void VehicleCannotMoveLeftMap()
        {
            var map = new Map(10, 10);
            var vehicle = new Vehicle(string.Empty, new VehicleStatus(0, 0, Direction.W));
            var vehicleInstruction = new VehicleInstruction(vehicle, new List<Instruction>() { Instruction.F });
            var vehicleStatus = _vechicleAutomationService.CalculateVehiclePath(map, vehicleInstruction);

            var expectedX = 0;
            var expectedY = 0;
            Assert.Equal(vehicleStatus.Path.Last().X, expectedX);
            Assert.Equal(vehicleStatus.Path.Last().Y, expectedY);
        }

        [Fact]
        public void VehicleCannotMoveRightMap()
        {
            var map = new Map(10, 10);
            var vehicle = new Vehicle(string.Empty, new VehicleStatus(9, 0, Direction.E));
            var vehicleInstruction = new VehicleInstruction(vehicle, new List<Instruction>() { Instruction.F });
            var vehicleStatus = _vechicleAutomationService.CalculateVehiclePath(map, vehicleInstruction);

            var expectedX = 9;
            var expectedY = 0;
            Assert.Equal(vehicleStatus.Path.Last().X, expectedX);
            Assert.Equal(vehicleStatus.Path.Last().Y, expectedY);
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
          
            var vehicleStatus = _vechicleAutomationService.CalculateVehiclePath(ADCInputs.Map, ADCInputs.VehicleInstructions.First());

            var expectedX = 4;
            var expectedY = 3;
            var expectedDirection = Direction.S;
            Assert.Equal(vehicleStatus.Path.Last().X, expectedX);
            Assert.Equal(vehicleStatus.Path.Last().Y, expectedY);
            Assert.Equal(vehicleStatus.Path.Last().Direction, expectedDirection);
        }

        [Fact]
        public void VehicleCollion()
        {
            var vehiclesStatuses = new List<VehiclePath>()
            {
                new VehiclePath("A", new List<VehicleStatus>() { new VehicleStatus(1, 2, Direction.N), new VehicleStatus(1, 1, Direction.E), new VehicleStatus(2, 1, Direction.E) }),
                new VehiclePath("B", new List<VehicleStatus>() { new VehicleStatus(1, 3, Direction.S), new VehicleStatus(5, 1, Direction.E) }),
                new VehiclePath("C", new List<VehicleStatus>() { new VehicleStatus(6, 6, Direction.N), new VehicleStatus(6, 6, Direction.E), new VehicleStatus(6, 6, Direction.S) }),
                new VehiclePath("D", new List<VehicleStatus>() { new VehicleStatus(2, 1, Direction.N) })
            };

            var collisionDetail = _vechicleAutomationService.CollionCheck(vehiclesStatuses);
            if (collisionDetail != null)
            {
                var expectedStepNo = 2;
                var expectedNoOfVehicleCollided = 2;

                Assert.Equal(collisionDetail.StepNo, expectedStepNo);
                Assert.Equal(collisionDetail.CollidedVehicle.Count(), expectedNoOfVehicleCollided);
            }
        }

        [Fact]
        public void CarSwapCanCauseCollision()
        {
            var vehiclesStatuses = new List<VehiclePath>()
            {
                new VehiclePath("A", new List<VehicleStatus>() { new VehicleStatus(1, 2, Direction.N), new VehicleStatus(1, 3, Direction.N) }),
                new VehiclePath("B", new List<VehicleStatus>() { new VehicleStatus(1, 3, Direction.S), new VehicleStatus(1, 2, Direction.S) })
            };

            var collisionDetail = _vechicleAutomationService.NoCarSwapOnStep(vehiclesStatuses);

            Assert.NotNull(collisionDetail);
            if (collisionDetail != null)
            {
                var expectedStepNo = 1;
                var expectedNoOfVehicleCollided = 2;

                Assert.Equal(collisionDetail.StepNo, expectedStepNo);
                Assert.Equal(collisionDetail.CollidedVehicle.Count(), expectedNoOfVehicleCollided);

            }
        }
    }
}