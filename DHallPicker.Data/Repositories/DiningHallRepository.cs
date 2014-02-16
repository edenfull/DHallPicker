using System.Data;
using DHallPicker.Models.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using Dapper;

namespace DHallPicker.Data.Repositories
{
    public class DiningHallRepository
    {
        public List<DiningHall> SelectAllHalls()
        {
            using (var connection = new SqlConnection(ConfigurationSettings.GetConnectionString()))
            {
                return connection.Query<DiningHall>("SelectAllHalls", commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
}
