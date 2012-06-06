using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using Microsoft.SqlServer.Types;

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlGeometry AverageGraph(SqlGeometry sourceGraph)
    {
        SqlGeometry result = new SqlGeometry();
        int dotQty = Convert.ToInt32(sourceGraph.STNumPoints());
        // For each dot, find union of all dots with same X.

        return result;
    }
};

