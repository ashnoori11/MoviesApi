using NetTopologySuite.Geometries;
using System.Collections.Generic;

namespace Domain.Entities;

public class MovieTheater
{
    private MovieTheater()
    {

    }

    public MovieTheater(string name, Point location)
    {
        Name = name;
        Location = location;
    }

    public void SetChanges(string name, Point location)
    {
        Name = name;
        Location = location;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public Point Location { get; private set; }

    #region NetTopologySuite.Geometries Raw Sql
    //INSERT INTO my_table(geometry_column) VALUES(ST_GeomFromText('POINT(10 20)', 4326));
    //UPDATE my_table SET geometry_column = ST_GeomFromText('LINESTRING(0 0, 10 10)', 4326) WHERE id = 1;
    //SELECT ST_AsText(geometry_column) FROM my_table WHERE ST_Intersects(geometry_column, ST_GeomFromText('POLYGON((0 0, 10 0, 10 10, 0 10, 0 0))', 4326));
    #endregion
}
