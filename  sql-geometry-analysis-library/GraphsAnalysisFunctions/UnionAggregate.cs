using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using Microsoft.SqlServer.Types;
using System.IO;

[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedAggregate(Format.UserDefined, MaxByteSize = -1)]
public struct UnionAggregate : IBinarySerialize
{
    public SqlGeometry union;
    public void Init()
    {
        union = new SqlGeometry();
    }

    public void Accumulate(SqlGeometry value)
    {
        if (union.IsNull || union.STIsEmpty())
            union = value;
        else
            union = union.STUnion(value);
    }

    public void Merge(UnionAggregate group)
    {
        if (union.IsNull || union.STIsEmpty())
            union = group.union;
        else
            union = union.STUnion(group.union);
    }

    public SqlGeometry Terminate()
    {
        return union;
    }

    public void Read(BinaryReader r)
    {
        union = new SqlGeometry();
        union.Read(r);
    }

    public void Write(BinaryWriter w)
    {
        union.Write(w);
    }

}
