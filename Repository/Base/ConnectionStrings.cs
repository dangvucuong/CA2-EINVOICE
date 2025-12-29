using Contracts.Repository.Base;
using Model.Static;

namespace Repository.Base
{
    public class ConnectionStrings : IConnectionStrings
    {
        private IMSSQLConnection _defaultConnection;
        private IMSSQLConnection _logConnection;
        public IMSSQLConnection Default => _defaultConnection??= new MSSQLConnection(eConectionStringKey.DefaultConnection.ToString());

        public IMSSQLConnection Log => _logConnection??= new MSSQLConnection(eConectionStringKey.LogConnection.ToString());
    }
}