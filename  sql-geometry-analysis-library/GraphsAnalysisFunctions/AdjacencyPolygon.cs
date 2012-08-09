using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using Microsoft.SqlServer.Types;

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlGeometry AdjacencyPolygon(SqlGeometry sourceGraph, SqlDouble deviation)
    {
        return CoreAnalysis(sourceGraph, AnalysisTypeEnum.SetValue(AnalysisTypeEnum.AnalysisType.Adjacency), (float) deviation);
    }
};

