using System;

namespace iCADEventServer.Service
{
    public class GlobalMercator
    {
        private Int32 TileSize;
        private Double InitialResolution;
        private Double OriginShift;
        private const Int32 EarthRadius = 6378137;
        public GlobalMercator()
        {
            TileSize = 256;
            InitialResolution = 2 * Math.PI * EarthRadius / TileSize;
            OriginShift = 2 * Math.PI * EarthRadius / 2;
        }
        public DPoint LatLonToMeters(Double lat, Double lon)
        {
            var p = new DPoint();
            p.X = lon * OriginShift / 180;
            p.Y = Math.Log(Math.Tan((90 + lat) * Math.PI / 360)) / (Math.PI / 180);
            p.Y = p.Y * OriginShift / 180;
            return p;
        }
        public GeoPoint MetersToLatLon(DPoint m)
        {
            var ll = new GeoPoint();
            ll.Longitude = (m.X / OriginShift) * 180;
            ll.Latitude = (m.Y / OriginShift) * 180;
            ll.Latitude = 180 / Math.PI * (2 * Math.Atan(Math.Exp(ll.Latitude * Math.PI / 180)) - Math.PI / 2);
            return ll;
        }
        public DPoint PixelsToMeters(DPoint p, Int32 zoom)
        {
            var res = Resolution(zoom);
            var met = new DPoint();
            met.X = p.X * res - OriginShift;
            met.Y = p.Y * res - OriginShift;
            return met;
        }
        public DPoint MetersToPixels(DPoint m, Int32 zoom)
        {
            var res = Resolution(zoom);
            var pix = new DPoint();
            pix.X = (m.X + OriginShift) / res;
            pix.Y = (m.Y + OriginShift) / res;
            return pix;
        }
        public Point PixelsToTile(DPoint p)
        {
            var t = new Point();
            t.X = (Int32)Math.Ceiling(p.X / (Double)TileSize) - 1;
            t.Y = (Int32)Math.Ceiling(p.Y / (Double)TileSize) - 1;
            return t;
        }
        public Point PixelsToRaster(Point p, Int32 zoom)
        {
            var mapSize = TileSize << zoom;
            return new Point(p.X, mapSize - p.Y);
        }
        public Point MetersToTile(Point m, Int32 zoom)
        {
            var p = MetersToPixels(m, zoom);
            return PixelsToTile(p);
        }

        public Pair<DPoint> TileBounds(Point t, Int32 zoom)
        {
            var min = PixelsToMeters(new DPoint(t.X * TileSize, t.Y * TileSize), zoom);
            var max = PixelsToMeters(new DPoint((t.X + 1) * TileSize, (t.Y + 1) * TileSize), zoom);
            return new Pair<DPoint>(min, max);
        }
        public Pair<GeoPoint> TileLatLonBounds(Point t, Int32 zoom)
        {
            var bound = TileBounds(t, zoom);
            var min = MetersToLatLon(bound.Min);
            var max = MetersToLatLon(bound.Max);
            return new Pair<GeoPoint>(min, max);
        }
        public Double Resolution(Int32 zoom)
        {
            return InitialResolution / (2 ^ zoom);
        }
        public Double ZoomForPixelSize(Double pixelSize)
        {
            for (var i = 0; i < 30; i++)
                if (pixelSize > Resolution(i))
                    return i != 0 ? i - 1 : 0;
            throw new InvalidOperationException();
        }
        public Point ToGoogleTile(Point t, Int32 zoom)
        {
            return new Point(t.X, ((Int32)Math.Pow(2, zoom) - 1) - t.Y);
        }
        public Point ToTmsTile(Point t, Int32 zoom)
        {
            return new Point(t.X, ((Int32)Math.Pow(2, zoom) - 1) - t.Y);
        }
    }
    public struct Point
    {
        public Point(Int32 x, Int32 y)
            : this()
        {
            X = x;
            Y = y;
        }
        public Int32 X { get; set; }
        public Int32 Y { get; set; }
    }
    public struct DPoint
    {
        public DPoint(Double x, Double y)
            : this()
        {
            this.X = x;
            this.Y = y;
        }
        public Double X { get; set; }
        public Double Y { get; set; }
        public static implicit operator DPoint(Point p)
        {
            return new DPoint(p.X, p.Y);
        }
    }
    public class GeoPoint
    {
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }
    }
    public class Pair<T>
    {
        public Pair() { }
        public Pair(T min, T max)
        {
            Min = min;
            Max = max;
        }
        public T Min { get; set; }
        public T Max { get; set; }
    }
}
