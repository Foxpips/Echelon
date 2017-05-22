using Echelon.Data;

namespace Echelon.Mediators
{
    public class ChatMediator : IMediator
    {
        private IDataService _dataService;

        public ChatMediator(IDataService dataService)
        {
            _dataService = dataService;
        }
    }
}