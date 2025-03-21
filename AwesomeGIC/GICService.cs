using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeGIC
{
    public interface IGICService
    {
        IProcessor? GetProcessor(string input);
    }

    public class GICService : IGICService
    {
        private readonly IIOService _ioService;
        private readonly IGICDataAccess _gicDataAccess;

        public GICService(IIOService ioService, IGICDataAccess gicDataAccess)
        {
            _ioService = ioService;
            _gicDataAccess = gicDataAccess;
        }


        public IProcessor? GetProcessor(string input)
        {
            IProcessor? processor = null;

            switch (input.ToLower())
            {
                case "t":
                    processor = new TransactionProcessor(_ioService, _gicDataAccess);
                    break;

                case "i":
                    processor = new DefineInterestProcessor(_ioService, _gicDataAccess);
                    break;

                case "p":
                    processor = new PrintStatementProcessor(_ioService, _gicDataAccess);
                    break;

                case "q":
                    processor = new QuitProcessor(_ioService, _gicDataAccess);
                    break;

                default:
                    processor = null;
                    break;
            }

            return processor;
        }
    }
}
