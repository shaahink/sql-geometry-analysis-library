using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.Native)]
public struct AnalysisTypeEnum : INullable
{
    public override string ToString()
    {
        return ((AnalysisType)value).ToString();
    }

    public AnalysisType TypeValue()
    {
        return (AnalysisType)value;
    }
    public bool IsNull
    {
        get
        {
            // Put your code here
            if (value == -1)
                return true;
            return false;
        }
    }

    public static AnalysisTypeEnum Null
    {
        get
        {
            AnalysisTypeEnum h = new AnalysisTypeEnum();
            h.m_Null = true;
            return h;
        }
    }

    // Should be never used. Need to correspond to UDP specification
    public static AnalysisTypeEnum Parse(SqlString s)
    {
        AnalysisTypeEnum u = new AnalysisTypeEnum();
        u.value = -1;
        return u;
    }
    public static AnalysisTypeEnum SetValue(AnalysisType s)
    {
        AnalysisTypeEnum u = new AnalysisTypeEnum();
        u.value = (int)s;
        return u;
    }

    public enum AnalysisType
    {
        Outer = 0, Top = 1, Bottom = 2, Average = 3
    }

    public int value;
    // Private member
    private bool m_Null;
}


