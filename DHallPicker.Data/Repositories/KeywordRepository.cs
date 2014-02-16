using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DHallPicker.Models.Models;
using Dapper;

namespace DHallPicker.Data.Repositories
{
    public class KeywordRepository
    {
        public List<DishKeyword> SelectFilteredKeywords(int[] diets)
        {
            string sql = "SELECT ExclusionKeywordID, Keyword, DishFilters.DishFilterID, FilterName " +
                         "FROM ExclusionKeywords " +
                         "INNER JOIN DishFilters ON ExclusionKeywords.DishFilterID = DishFilters.DishFilterID " +
                         "WHERE";

            for (int i = 0; i < diets.Length; i++)
            {
                if (i == 0)
                    sql += " ExclusionKeywords.DishFilterID = " + diets[i];
                else
                    sql += " OR ExclusionKeywords.DishFilterID = " + diets[i];

                if (i == diets.Length - 1)
                    sql += ";";
            }

            using (var connection = new SqlConnection(ConfigurationSettings.GetConnectionString()))
            {
                return connection.Query<DishKeyword>(sql).ToList();
            }
        }
    }
}
