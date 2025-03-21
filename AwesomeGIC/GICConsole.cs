using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeGIC
{
    public class GICConsole
    {
        private readonly IIOService _ioService;
        private readonly IGICDataAccess _gicDataAccess;
        private readonly IGICService _gicService;

        public GICConsole()
        {
            _ioService = new ConsoleIOService();
            _gicDataAccess = new GICData();
            _gicService = new GICService(_ioService, _gicDataAccess);
        }

        public void Run()
        {
            string option;

            while (_gicDataAccess.GetRunningState())
            {
                option = _ioService.GetInput(GICConstants.MainMenu);
                var processor = _gicService.GetProcessor(option);

                if (processor != null)
                    processor.Process();
            }
        }
    }
}
