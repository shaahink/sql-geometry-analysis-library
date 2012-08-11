using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using Microsoft.SqlServer.Types;

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlDouble SimilarityDegree(SqlGeometry pattern, SqlGeometry testee, SqlDouble deviation)
    {
        float PatternArea = (float)AdjacencyPolygon(pattern, deviation).STArea();
        float intersectionArea = (float)AdjacencyPolygon(pattern, deviation).STIntersection(AdjacencyPolygon(testee, deviation)).STArea();
        float result = 0;
        if (PatternArea != 0)
            result = intersectionArea / PatternArea;
        return (SqlDouble)result;
    }
};

