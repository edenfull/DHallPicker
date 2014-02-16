using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DHallPicker.Models.Models;
using Dapper;

namespace DHallPicker.Data.Repositories
{
    public class DietRepository
    {
        public List<Diet> SelectAllDiets()
        {
            using (var connection = new SqlConnection(ConfigurationSettings.GetConnectionString()))
            {
                return connection.Query<Diet>("SelectAllDiets", commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
}
