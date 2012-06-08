using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using Microsoft.SqlServer.Types;

public partial class UserDefinedFunctions
{
    
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlGeometry CoreAnalysis(SqlGeometry sourceGraph, AnalysisTypeEnum type)
    {
        SqlGeometry polygonsUnion = new SqlGeometry();
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
            try
            {
                if (type.TypeValue() == AnalysisTypeEnum.AnalysisType.Average)
                {
                    double xValue = 0;
                    double ySum = 0;

                    xValue = borderDot.STX.Value;
                    for (int j = 1; j <= (int)intersection.STNumPoints(); j++)
                        ySum += intersection.STPointN(j).STY.Value;
                    ySum = ySum / (float)intersection.STNumPoints();
                    borderDot = SqlGeometry.Parse(String.Format("POINT ({0} {1})", xValue.ToString("#0.000000").Replace(",", "."), ySum.ToString("#0.000000").Replace(",", ".")));
                    polygonsUnion = polygonsUnion.STUnion(borderDot);

                }
                if (type.TypeValue() == AnalysisTypeEnum.AnalysisType.Top)
                {

                    double yMax = borderDot.STY.Value;
                    for (int j = 2; j <= (int)intersection.STNumPoints(); j++)
                        if (intersection.STPointN(j).STY.Value > yMax)
                        {
                            borderDot = intersection.STPointN(j);
                            yMax = borderDot.STY.Value;
                        }
                    polygonsUnion = polygonsUnion.STUnion(borderDot);
                }
                if (type.TypeValue() == AnalysisTypeEnum.AnalysisType.Bottom)
                {
                    double yMin = borderDot.STY.Value;
                    for (int j = 2; j <= (int)intersection.STNumPoints(); j++)
                        if (intersection.STPointN(j).STY.Value < yMin)
                        {
                            borderDot = intersection.STPointN(j);
                            yMin = borderDot.STY.Value;
                        }
                    polygonsUnion = polygonsUnion.STUnion(borderDot);
                }
                if (type.TypeValue() == AnalysisTypeEnum.AnalysisType.Outer)
                {
                    polygonsUnion = polygonsUnion.STUnion(intersection);
                }
            }
            catch { }
                
        }
        int polygonsQty = (int)polygonsUnion.STNumGeometries();
        double[] xValues = new double[polygonsQty];
        int[] orderNumbers = new int[polygonsQty];
        for (int i = 1; i <= polygonsQty; i++)
        {
            xValues[i - 1] = polygonsUnion.STGeometryN(i).STPointN(1).STX.Value;
            orderNumbers[i - 1] = i;
        }
        Array.Sort(xValues, orderNumbers);

        for (int i = 0; i < polygonsQty - 1; i++)
        {
            SqlGeometry currentPolygon = polygonsUnion.STGeometryN(orderNumbers[i]);
            SqlGeometry nextPolygon = polygonsUnion.STGeometryN(orderNumbers[i + 1]);
            currentPolygon = currentPolygon.STUnion(nextPolygon).STConvexHull();
            result = result.STUnion(currentPolygon);
        }

        return result;
    }
};

