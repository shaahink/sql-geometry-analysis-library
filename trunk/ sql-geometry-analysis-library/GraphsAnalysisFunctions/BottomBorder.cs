using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using Microsoft.SqlServer.Types;

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlGeometry BottomBorder(SqlGeometry sourceGraph)
    {
        SqlGeometry dottedLine = new SqlGeometry();
        SqlGeometry result = new SqlGeometry();
        //int dotQty = Convert.ToInt32(sourceGraph.STNumPoints());
        int dotQty = (int)sourceGraph.STNumPoints();
        // For each dot, find union of all dots with same X.
        double minY;
        double maxY;
        SqlGeometry envelope = sourceGraph.STEnvelope();
        minY = envelope.STPointN(1).STY.Value;
        maxY = envelope.STPointN(3).STY.Value;
        for (int i = 1; i <= dotQty; i++)
        {
            SqlGeometry currentDot = sourceGraph.STPointN(i);
            SqlGeometry verticalLine = SqlGeometry.Parse(String.Format("LINESTRING({0} {1}, {0} {2})", currentDot.STX.Value.ToString("#0.000000").Replace(",", "."), minY.ToString("#0.000000").Replace(",", "."), maxY.ToString("#0.000000").Replace(",", ".")));
            SqlGeometry intersection = sourceGraph.STIntersection(verticalLine).STEnvelope();
            SqlGeometry borderDot = intersection.STPointN(1);
            double yMin = borderDot.STY.Value;
            for (int j = 2; j <= (int)intersection.STNumPoints(); j++)
                if (intersection.STPointN(j).STY.Value < yMin)
                {
                    borderDot = intersection.STPointN(j);
                    yMin = borderDot.STY.Value;
                }
            dottedLine = dottedLine.STUnion(borderDot);
        }
        int polygonsQty = (int)dottedLine.STNumGeometries();
        double[] xValues = new double[polygonsQty];
        int[] orderNumbers = new int[polygonsQty];
        for (int i = 1; i <= polygonsQty; i++)
        {
            xValues[i - 1] = dottedLine.STGeometryN(i).STPointN(1).STX.Value;
            orderNumbers[i - 1] = i;
        }
        Array.Sort(xValues, orderNumbers);

        for (int i = 0; i < polygonsQty - 1; i++)
        {
            SqlGeometry currentPolygon = dottedLine.STGeometryN(orderNumbers[i]);
            SqlGeometry nextPolygon = dottedLine.STGeometryN(orderNumbers[i + 1]);
            currentPolygon = currentPolygon.STUnion(nextPolygon).STConvexHull();
            result = result.STUnion(currentPolygon);
        }

        return result;
    }
};

