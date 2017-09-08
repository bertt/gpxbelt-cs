using System.Xml;

namespace gpxbelt_cs
{
    public class GpxDocument
    {
        private XmlDocument gpxDoc;
        private XmlNamespaceManager nsmgr;

        public GpxDocument(string file)
        {
            gpxDoc = new XmlDocument();
            nsmgr = new XmlNamespaceManager(gpxDoc.NameTable);
            nsmgr.AddNamespace("x", "http://www.topografix.com/GPX/1/1");
            gpxDoc.Load(file);
        }

        public int Count
        {
            get
            {
                XmlNodeList nl = gpxDoc.SelectNodes("//x:trkpt", nsmgr);
                return nl.Count;
            }
        }

        public double[] Bounds()
        {
            double x0 = double.MaxValue, y0 = double.MaxValue, x1 =double.MinValue, y1 = double.MinValue;
            XmlNodeList nl = gpxDoc.SelectNodes("//x:trkpt", nsmgr);
            foreach (XmlNode n in nl)
            {
                var lat = double.Parse(n.Attributes["lat"].Value);
                var lon = double.Parse(n.Attributes["lon"].Value);
                x0 = (lon < x0 ? lon : x0);
                x1 = (lon > x1 ? lon : x1);
                y0 = (lat < y0 ? lat : y0);
                y1 = (lat > y1 ? lat : y1);
            }
            return new double[4] { x0, y0, x1, y1 };
        }

        public bool Intersects(double[] bounds)
        {
            XmlNodeList nl = gpxDoc.SelectNodes("//x:trkpt", nsmgr);
            foreach (XmlNode n in nl)
            {
                var lat = double.Parse(n.Attributes["lat"].Value);
                var lon = double.Parse(n.Attributes["lon"].Value);

                if (lon >= bounds[0] && lon <= bounds[2] && lat >= bounds[1] && lat <= bounds[3])
                {
                    return true;
                }
            }
            return false;
        }
    }
}
