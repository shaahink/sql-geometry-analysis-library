-- Examples for queries that exercise different SQL objects implemented by this assembly

-----------------------------------------------------------------------------------------
-- Stored procedure
-----------------------------------------------------------------------------------------
-- exec StoredProcedureName


-----------------------------------------------------------------------------------------
-- User defined function
-----------------------------------------------------------------------------------------
-- select dbo.FunctionName()


-----------------------------------------------------------------------------------------
-- User defined type
-----------------------------------------------------------------------------------------
-- CREATE TABLE test_table (col1 UserType)
--
-- INSERT INTO test_table VALUES (convert(uri, 'Instantiation String 1'))
-- INSERT INTO test_table VALUES (convert(uri, 'Instantiation String 2'))
-- INSERT INTO test_table VALUES (convert(uri, 'Instantiation String 3'))
--
-- select col1::method1() from test_table



-----------------------------------------------------------------------------------------
-- User defined type
-----------------------------------------------------------------------------------------
-- select dbo.AggregateName(Column1) from Table1


--SELECT dbo.AdjacencyPolygon(graph, 0.2).ToString() from Graphs where id =6
--select dbo.AdjacencyPolygon(geometry::STGeomFromText('LINESTRING(0 0, 1 0)', 0), 1);
select dbo.SimilarityDegree(g1.graph, g2.graph, 0.2)
from Graphs g1, Graphs g2
where g1.id =6 and g2.id = 7