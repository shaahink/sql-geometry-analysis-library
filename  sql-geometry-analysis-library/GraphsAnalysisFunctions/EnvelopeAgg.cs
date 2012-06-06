using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using Microsoft.SqlServer.Types;

[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedAggregate(Format.Native)]
public struct EnvelopeAgg
{

    public double minX;
    public double minY;
    public double maxX;
    public double maxY;
    public void Init()
    {
        minX = double.MaxValue;
        minY = double.MaxValue;
        maxX = double.MinValue;
        maxY = double.MinValue;
    }

    public void Accumulate(SqlGeometry value)
    {
        if (value != null && !value.IsNull && !value.STIsEmpty())
        {
            SqlGeometry envelope = value.STEnvelope();
            if (envelope.STPointN(1).STX.Value < minX)
                minX = envelope.STPointN(1).STX.Value;
            if (envelope.STPointN(1).STY.Value < minY)
                minY = envelope.STPointN(1).STY.Value;
            if (envelope.STPointN(3).STX.Value > maxX)
                maxX = envelope.STPointN(3).STX.Value;
            if (envelope.STPointN(3).STY.Value > maxY)
                maxY = envelope.STPointN(3).STY.Value;
        }

    }

    public void Merge(EnvelopeAgg group)
    {
        if (group.minX < minX)
            minX = group.minX;
        if (group.minY < minY)
            minY = group.minY;
        if (group.maxX > maxX)
            maxX = group.maxX;
        if (group.maxY > maxY)
            maxY = group.maxY;
    }

    public SqlGeometry Terminate()
    {
        if (minX == double.MaxValue) //All inputs were null or empty
            return SqlGeometry.Null;
        return SqlGeometry.Parse(String.Format("POLYGON (({0} {1}, {2} {1}, {2} {3}, {0} {3}, {0} {1}))", minX.ToString("#0.000000").Replace(",", "."), minY.ToString("#0.000000").Replace(",", "."), maxX.ToString("#0.000000").Replace(",", "."), maxY.ToString("#0.000000").Replace(",", ".")));
        //return SqlGeometry.Parse(
    }
}