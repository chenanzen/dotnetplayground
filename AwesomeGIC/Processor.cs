using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeGIC
{
    public interface IProcessor
    {
        public void Process();
    }





    public class QuitProcessor : IProcessor
    {
        private readonly IIOService _ioService;
        private readonly IGICDataAccess _gicDataAccess;

        public QuitProcessor(IIOService ioService, IGICDataAccess gicDataAccess)
        {
            _ioService = ioService;
            _gicDataAccess = gicDataAccess;
        }

        public void Process()
        {
            _gicDataAccess.SetRunningState(false);
            _ioService.ShowMessage(GICConstants.ExitMessage);
            return;
        }
    }
}
