using gpxbelt_cs;
using System;
using System.Collections.Generic;
using System.IO;
using Tiles.Tools;

namespace sample
{
    class Program
    {
        public static List<Tile> BboxToTiles(double[] bbox, int level)
        {
            var minTile = Tilebelt.PointToTile(bbox[0], bbox[1], level);
            var maxTile = Tilebelt.PointToTile(bbox[2], bbox[3], level);
            var result = new List<Tile>();
            for (var lon = minTile.X; lon <= maxTile.X; lon++)
            {
                for (var lat = maxTile.Y; lat <= minTile.Y; lat++)
                {
                    var t = new Tile(lon, lat, level);
                    result.Add(t);
                }
            }
            return result;
        }

        public static List<Tile> GetIntersectingTiles(GpxDocument doc, int level)
        {
            var result = new List<Tile>();
            var bounds = doc.Bounds();
            var tiles = BboxToTiles(bounds, 14);

            foreach (var tile in tiles)
            {
                if (doc.Intersects(tile.Bounds()))
                {
                    result.Add(tile);
                }
            }
            return result;
        }

        public static List<Tile> GetIntersectingTiles(string datadir)
        {
            var files = Directory.GetFiles("data");
            var intersectingtiles = new List<Tile>();

            foreach (var file in files)
            {
                var gpxDocument = new GpxDocument(file);
                var tiles = GetIntersectingTiles(gpxDocument, 14);

                foreach (var tile in tiles)
                {
                    if (!intersectingtiles.Contains(tile))
                    {
                        intersectingtiles.Add(tile);
                    };
                }
            }
            return intersectingtiles;

        }

        static void Main(string[] args)
        {
            var intersectingtiles = GetIntersectingTiles("data");
            Console.WriteLine("Tiles: " + intersectingtiles.Count);
            Console.ReadKey();
        }
    }
}
