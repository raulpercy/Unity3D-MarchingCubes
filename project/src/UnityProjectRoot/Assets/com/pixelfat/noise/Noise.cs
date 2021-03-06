using System;

namespace com.pixelfat.noise {
	
	public class PerlinNoise
	{
	    public delegate void TickEventHandler(double percent);
	    public event TickEventHandler Tick;
	
	    Random r;// = new Random();
	    int r1;// = r.Next(1000, 10000);
	    int r2;// = r.Next(100000, 1000000);
	    int r3;// = r.Next(1000000000, 2000000000);
	    int _seed;
	    public double[,] result;
	
	    public double frequency = .035;    // USER ADJUSTABLE
	    public double persistence = .55;   // USER ADJUSTABLE
	    public double octaves = 8;         // USER ADJUSTABLE
	    public double amplitude = 2;       // USER ADJUSTABLE
	
	    public double cloudCoverage = 0;   // USER ADJUSTABLE
	    public double cloudDensity = 1;    // USER ADJUSTABLE
	
	    public int Seed { get { return _seed; } set { NewRandom(value); _seed = value; } }
	
	    public PerlinNoise(int seed)
	    {
	        this._seed = seed;
	        this.r = new Random(this.Seed);
	        this.r1 = r.Next(1000, 10000);
	        this.r2 = r.Next(100000, 1000000);
	        this.r3 = r.Next(1000000000, 2000000000);
	    }
	
	    public PerlinNoise()
	    {
	        this.r = new Random();
	        this.r1 = r.Next(1000, 10000);
	        this.r2 = r.Next(100000, 1000000);
	        this.r3 = r.Next(1000000000, 2000000000);
	    }
	
	    public void NewRandom(int seed)
	    {
	        this._seed = seed;
	        this.r = new Random(this.Seed);
	        this.r1 = r.Next(1000, 10000);
	        this.r2 = r.Next(100000, 1000000);
	        this.r3 = r.Next(1000000000, 2000000000);
	    }
	
	    public void NewRandom()
	    {
	        this.Seed = 0;
	        this.r = new Random();
	        this.r1 = r.Next(1000, 10000);
	        this.r2 = r.Next(100000, 1000000);
	        this.r3 = r.Next(1000000000, 2000000000);
	    }
	
	    double Noise2D(int x, int y)
	    {
	        int n = x + y * 57;
	        n = (n << 13) ^ n;
	
	        return (1.0 - ((n * (n * n * r1 + r2) + r3) & 0x7fffffff) / 1073741824.0);
	    }
	
	    double PerlinNoise2d(int x, int y)
	    {
	        double total = 0.0;
	
	        double f = frequency;
	        double a = amplitude;
	
	        for (int lcv = 0; lcv < octaves; lcv++)
	        {
	            total = total + Smooth2D(x * f, y * f) * a;
	            f = f * 2;
	            a = a * persistence;
	        }
	
	        total = (total + cloudCoverage) * cloudDensity;
	
	        if (total < -2) total = -2.0;
	        if (total > 2) total = 2.0;
	
	        total = total / 2; //[-1, 1]
	        //total = total * (1 - Math.Pow((double)y/64, 2));
	
	        return total / 2 + 0.5;
	    }
	
	    double Smooth2D(double x, double y)
	    {
	        double n1 = Noise2D((int)x, (int)y);
	        double n2 = Noise2D((int)x + 1, (int)y);
	        double n3 = Noise2D((int)x, (int)y + 1);
	        double n4 = Noise2D((int)x + 1, (int)y + 1);
	
	        double i1 = Interpolate2D(n1, n2, x - (int)x);
	        double i2 = Interpolate2D(n3, n4, x - (int)x);
	
	        return Interpolate2D(i1, i2, y - (int)y);
	    }
	
	    double Interpolate2D(double x, double y, double a)
	    {
	        double val = (1 - Math.Cos(a * Math.PI)) * .5;
	        return x * (1 - val) + y * val;
	    }
	
	    public double[,] Generate2D(int h, int w)
	    {
	        double[,] r = new double[h, w];
	        for (int x = 0; x < w; x++)
	            for (int y = 0; y < h; y++)
	                r[x, y] = PerlinNoise2d(x, y);
	        return r;
	    }
	
	
	
	    double Noise3D(int x, int y, int z)
	    {
	        int n = x + y * 57 + z * 42;
	        n = (n << 13) ^ n;
	
	        return (1.0 - ((n * (n * n * r1 + r2) + r3) & 0x7fffffff) / 1073741824.0);
	    }
	
	    double PerlinNoise3D(int x, int y, int z)
	    {
	        double total = 0.0;
	
	        double f = frequency;
	        double a = amplitude;
	        double maxa = 1;// a;
	        for (int lcv = 0; lcv < octaves; lcv++)
	        {
	            total = total + Smooth3D(x * f, y * f, z * f) * a;
	            f = f * 2;
	            a = a * persistence;
	            //maxa = maxa + a;
	        }
	
	        total = (total + cloudCoverage) * cloudDensity;
	
	        if (total < -maxa) total = -maxa;
	        if (total > maxa) total = maxa;
	
	        total = total / maxa; //[-1;1]
	        //total = total / (2 * maxa) + 0.5; //Math.Sqrt(total / 4 + 0.5);
	        return total;
	    }
	
	    double Smooth3D(double x, double y, double z)
	    {
	        double n11 = Noise3D((int)x, (int)y, (int)z);
	        double n21 = Noise3D((int)x + 1, (int)y, (int)z);
	        double n31 = Noise3D((int)x, (int)y + 1, (int)z);
	        double n41 = Noise3D((int)x + 1, (int)y + 1, (int)z);
	        double n12 = Noise3D((int)x, (int)y, (int)z + 1);
	        double n22 = Noise3D((int)x + 1, (int)y, (int)z + 1);
	        double n32 = Noise3D((int)x, (int)y + 1, (int)z + 1);
	        double n42 = Noise3D((int)x + 1, (int)y + 1, (int)z + 1);
	
	        double i11 = Interpolate3D(n11, n21, x - (int)x);
	        double i21 = Interpolate3D(n31, n41, x - (int)x);
	        double i12 = Interpolate3D(n12, n22, x - (int)x);
	        double i22 = Interpolate3D(n32, n42, x - (int)x);
	
	        double j1 = Interpolate3D(i11, i21, y - (int)y);
	        double j2 = Interpolate3D(i12, i22, y - (int)y);
	
	        return Interpolate3D(j1, j2, z - (int)z);
	    }
	
	    double Interpolate3D(double x, double y, double a)
	    {
	        //double val = (1 - Math.Cos(a * Math.PI)) * .5; //косинусная интерполяция
	        double val = a; //линейная интерполяция
	        return x * (1 - val) + y * val;
	    }
	
	    public double[, ,] Generate3D(int w, int h, int d)
	    {
	        // double c = 0;
	        //double max = w * h * d;
	        double[, ,] r = new double[w, h, d];
	        for (int x = 0; x < w; x++)
	            for (int y = 0; y < h; y++)
	                for (int z = 0; z < d; z++)
	                {
	                    r[x, y, z] = PerlinNoise3D(x, y, z);
	                    //Tick(c/max);
	                    //c++;
	                }
	        return r;
	    }
	
	    public static void FilterY(ref double[, ,] data, double power, int w, int h, int d)
	    {
	        for (int x = 0; x < w; x++)
	            for (int y = 0; y < h; y++)
	                for (int z = 0; z < d; z++)
	                    //data[x, y, z] = data[x, y, z] * (1 - Math.Sqrt((double)y / h));
	                    //data[x, y, z] = data[x, y, z] *(Math.Pow(1 - (double)y / h, power));
	                    //data[x, y, z] = (data[x, y, z] + ((double)y / h) * (1 - data[x, y, z]) )* ((double)y / h);
	                    data[x, y, z] = 1 - Math.Pow(1 - (double)y / h, 0.8) * (1 - data[x, y, z] * (Math.Pow((double)y / h, 0.8)));
	        //data[x, y, z] = - (((2.0 + data[x, y, z]) * (Math.Pow((1.0 - (double)y / (double)h) * 2.0 - 1.0, 3) / 2 + .5) - 2.0));
	        //data[x, y, z] = (data[x, y, z] + (Math.Pow((1.0 - (double)y / (double)h) * 2.0 - 1.0, 9) * 2.0));
	    }
	
	    public static void ApplyHeight(ref double[, ,] data, ref double[,] heights, int w, int h, int d, double mult)
	    {
	        for (int x = 0; x < w; x++)
	            for (int y = 0; y < h; y++)
	                for (int z = 0; z < d; z++)
	                {
	                    //if (heights[x, z] < ((double)y / (double)h)) data[x, y, z] = data[x, y, z]-1.0;
	                    data[x, y, z] = (heights[x, z] * 0.5 + 0.25 - (double)y / (double)h) + data[x, y, z] * mult;
	                    //else data[x, y, z] = data[x, y, z] - 1;
	                    //data[x, y, z] = heights[x, z] - ((double)y / (double)h);
	                    //data[x, y, z] = data[x, y, z] + heights[x, z] * (1 - (double)y / (double)h);
	                }
	    }
	}
	
	public class PerlinNoiseI
	{
	    public double frequency = .035;    // USER ADJUSTABLE
	    public double persistence = .55;   // USER ADJUSTABLE
	    public double octaves = 8;         // USER ADJUSTABLE
	    public double amplitude = 2;       // USER ADJUSTABLE
	
	    public double cloudCoverage = 0;   // USER ADJUSTABLE
	    public double cloudDensity = 1;    // USER ADJUSTABLE
	    private const int GradientSizeTable = 256;
	    private readonly Random _random;
	    private readonly double[] _gradients = new double[GradientSizeTable * 3];
	    /* Borrowed from Darwyn Peachey (see references above).
	       The gradient table is indexed with an XYZ triplet, which is first turned
	       into a single random index using a lookup in this table. The table simply
	       contains all numbers in [0..255] in random order. */
	    private readonly byte[] _perm = new byte[] {
	          225,155,210,108,175,199,221,144,203,116, 70,213, 69,158, 33,252,
	            5, 82,173,133,222,139,174, 27,  9, 71, 90,246, 75,130, 91,191,
	          169,138,  2,151,194,235, 81,  7, 25,113,228,159,205,253,134,142,
	          248, 65,224,217, 22,121,229, 63, 89,103, 96,104,156, 17,201,129,
	           36,  8,165,110,237,117,231, 56,132,211,152, 20,181,111,239,218,
	          170,163, 51,172,157, 47, 80,212,176,250, 87, 49, 99,242,136,189,
	          162,115, 44, 43,124, 94,150, 16,141,247, 32, 10,198,223,255, 72,
	           53,131, 84, 57,220,197, 58, 50,208, 11,241, 28,  3,192, 62,202,
	           18,215,153, 24, 76, 41, 15,179, 39, 46, 55,  6,128,167, 23,188,
	          106, 34,187,140,164, 73,112,182,244,195,227, 13, 35, 77,196,185,
	           26,200,226,119, 31,123,168,125,249, 68,183,230,177,135,160,180,
	           12,  1,243,148,102,166, 38,238,251, 37,240,126, 64, 74,161, 40,
	          184,149,171,178,101, 66, 29, 59,146, 61,254,107, 42, 86,154,  4,
	          236,232,120, 21,233,209, 45, 98,193,114, 78, 19,206, 14,118,127,
	           48, 79,147, 85, 30,207,219, 54, 88,234,190,122, 95, 67,143,109,
	          137,214,145, 93, 92,100,245,  0,216,186, 60, 83,105, 97,204, 52};
	
	    public PerlinNoiseI(int seed)
	    {
	        _random = new Random(seed);
	        InitGradients();
	    }
	
	    public double Noise(double x, double y, double z)
	    {
	        /* The main noise function. Looks up the pseudorandom gradients at the nearest
	           lattice points, dots them with the input vector, and interpolates the
	           results to produce a single output value in [0, 1] range. */
	
	        int ix = (int)Math.Floor(x);
	        double fx0 = x - ix;
	        double fx1 = fx0 - 1;
	        double wx = Smooth(fx0);
	
	        int iy = (int)Math.Floor(y);
	        double fy0 = y - iy;
	        double fy1 = fy0 - 1;
	        double wy = Smooth(fy0);
	
	        int iz = (int)Math.Floor(z);
	        double fz0 = z - iz;
	        double fz1 = fz0 - 1;
	        double wz = Smooth(fz0);
	
	        double vx0 = Lattice(ix, iy, iz, fx0, fy0, fz0);
	        double vx1 = Lattice(ix + 1, iy, iz, fx1, fy0, fz0);
	        double vy0 = Lerp(wx, vx0, vx1);
	
	        vx0 = Lattice(ix, iy + 1, iz, fx0, fy1, fz0);
	        vx1 = Lattice(ix + 1, iy + 1, iz, fx1, fy1, fz0);
	        double vy1 = Lerp(wx, vx0, vx1);
	
	        double vz0 = Lerp(wy, vy0, vy1);
	
	        vx0 = Lattice(ix, iy, iz + 1, fx0, fy0, fz1);
	        vx1 = Lattice(ix + 1, iy, iz + 1, fx1, fy0, fz1);
	        vy0 = Lerp(wx, vx0, vx1);
	
	        vx0 = Lattice(ix, iy + 1, iz + 1, fx0, fy1, fz1);
	        vx1 = Lattice(ix + 1, iy + 1, iz + 1, fx1, fy1, fz1);
	        vy1 = Lerp(wx, vx0, vx1);
	
	        double vz1 = Lerp(wy, vy0, vy1);
	        return Lerp(wz, vz0, vz1);
	    }
	
	    private void InitGradients()
	    {
	        for (int i = 0; i < GradientSizeTable; i++)
	        {
	            double z = 1f - 2f * _random.NextDouble();
	            double r = Math.Sqrt(1f - z * z);
	            double theta = 2 * Math.PI * _random.NextDouble();
	            _gradients[i * 3] = r * Math.Cos(theta);
	            _gradients[i * 3 + 1] = r * Math.Sin(theta);
	            _gradients[i * 3 + 2] = z;
	        }
	    }
	
	    private int Permutate(int x)
	    {
	        const int mask = GradientSizeTable - 1;
	        return _perm[x & mask];
	    }
	
	    private int Index(int ix, int iy, int iz)
	    {
	        // Turn an XYZ triplet into a single gradient table index.
	        return Permutate(ix + Permutate(iy + Permutate(iz)));
	    }
	
	    private double Lattice(int ix, int iy, int iz, double fx, double fy, double fz)
	    {
	        // Look up a random gradient at [ix,iy,iz] and dot it with the [fx,fy,fz] vector.
	        int index = Index(ix, iy, iz);
	        int g = index * 3;
	        return _gradients[g] * fx + _gradients[g + 1] * fy + _gradients[g + 2] * fz;
	    }
	
	    private double Lerp(double t, double value0, double value1)
	    {
	        // Simple linear interpolation.
	        return value0 + t * (value1 - value0);
	    }
	
	    private double Smooth(double x)
	    {
	        /* Smoothing curve. This is used to calculate interpolants so that the noise
	          doesn't look blocky when the frequency is low. */
	        return x * x * (3 - 2 * x);
	    }
	
	
	    public double[, ,] Generate3D(int w, int h, int d, int x, int y, int z)
	    {
			w++;
			h++;
			d++;
			
			// double c = 0;
	        // double max = w * h * d;
	        double[, ,] r = new double[w, h, d];
	        double widthDivisor = 1 / (double)w;
	        double heightDivisor = 1 / (double)h;
	        double depthDivisor = 1 / (double)h;
	
	        for (int i = x; i < w + x; i++)
	            for (int j = y; j < h + y; j++)
	                for (int k = z; k < d + z; k++)
	                {
	                    r[i - x, j - y, k - z] =
	                        (Noise(2 * i * widthDivisor, 2 * j * heightDivisor, 2 * k * depthDivisor) + 1) / 2 * 0.7 +
	                        (Noise(4 * i * widthDivisor, 4 * j * heightDivisor, 4 * k * depthDivisor) + 1) / 2 * 0.3 +
	                        (Noise(8 * i * widthDivisor, 8 * j * heightDivisor, 8 * k * depthDivisor) + 1) / 2 * 0.1 +
	                        (Noise(16 * i * widthDivisor, 16 * j * heightDivisor, 16 * k * depthDivisor) + 1) / 2 * 0.03 +
	                        (Noise(32 * i * widthDivisor, 32 * j * heightDivisor, 32 * k * depthDivisor) + 1) / 2 * 0.007;
	                    //r[x, y, z] = Noise(x, y, z);
	                    //Tick(c/max);
	                    //c++;
	                }
	        return r;
	    }
	
	
	}
	
	public class SimplexNoise
	{
	    #region tables
	    private static int[][] grad3 = {
	                                       new[] {1, 1, 0}, new [] {-1, 1, 0}, new [] {1, -1, 0}, new[]{-1, -1, 0},
	                                      new[]{1, 0, 1}, new[]{-1, 0, 1}, new[]{1, 0, -1}, new[]{-1, 0, -1},
	                                      new[]{0, 1, 1},new[] {0, -1, 1},new[] {0, 1, -1}, new[]{0, -1, -1}
	                                  };
	
	    private static int[,] grad4 = {
	                                      {0, 1, 1, 1}, {0, 1, 1, -1}, {0, 1, -1, 1}, {0, 1, -1, -1},
	                                      {0, -1, 1, 1}, {0, -1, 1, -1}, {0, -1, -1, 1}, {0, -1, -1, -1},
	                                      {1, 0, 1, 1}, {1, 0, 1, -1}, {1, 0, -1, 1}, {1, 0, -1, -1},
	                                      {-1, 0, 1, 1}, {-1, 0, 1, -1}, {-1, 0, -1, 1}, {-1, 0, -1, -1},
	                                      {1, 1, 0, 1}, {1, 1, 0, -1}, {1, -1, 0, 1}, {1, -1, 0, -1},
	                                      {-1, 1, 0, 1}, {-1, 1, 0, -1}, {-1, -1, 0, 1}, {-1, -1, 0, -1},
	                                      {1, 1, 1, 0}, {1, 1, -1, 0}, {1, -1, 1, 0}, {1, -1, -1, 0},
	                                      {-1, 1, 1, 0}, {-1, 1, -1, 0}, {-1, -1, 1, 0}, {-1, -1, -1, 0}
	                                  };
	
	    private static int[] p = {
	                                 151, 160, 137, 91, 90, 15, 131, 13, 201, 95, 96, 53, 194, 233, 7, 225, 140, 36, 103
	                                 , 30, 69, 142, 8, 99, 37, 240, 21, 10, 23, 190, 6, 148, 247, 120, 234, 75, 0, 26,
	                                 197, 62, 94, 252, 219, 203, 117, 35, 11, 32, 57, 177, 33, 88, 237, 149, 56, 87, 174
	                                 , 20, 125, 136, 171, 168, 68, 175, 74, 165, 71, 134, 139, 48, 27, 166, 77, 146, 158
	                                 , 231, 83, 111, 229, 122, 60, 211, 133, 230, 220, 105, 92, 41, 55, 46, 245, 40, 244
	                                 , 102, 143, 54, 65, 25, 63, 161, 1, 216, 80, 73, 209, 76, 132, 187, 208, 89, 18,
	                                 169, 200, 196, 135, 130, 116, 188, 159, 86, 164, 100, 109, 198, 173, 186, 3, 64, 52
	                                 , 217, 226, 250, 124, 123, 5, 202, 38, 147, 118, 126, 255, 82, 85, 212, 207, 206,
	                                 59, 227, 47, 16, 58, 17, 182, 189, 28, 42, 223, 183, 170, 213, 119, 248, 152, 2, 44
	                                 , 154, 163, 70, 221, 153, 101, 155, 167, 43, 172, 9, 129, 22, 39, 253, 19, 98, 108,
	                                 110, 79, 113, 224, 232, 178, 185, 112, 104, 218, 246, 97, 228, 251, 34, 242, 193,
	                                 238, 210, 144, 12, 191, 179, 162, 241, 81, 51, 145, 235, 249, 14, 239, 107, 49, 192
	                                 , 214, 31, 181, 199, 106, 157, 184, 84, 204, 176, 115, 121, 50, 45, 127, 4, 150,
	                                 254, 138, 236, 205, 93, 222, 114, 67, 29, 24, 72, 243, 141, 128, 195, 78, 66, 215,
	                                 61, 156, 180
	                             };
	
	    private static int[,] simplex = {
	                                        {0, 1, 2, 3}, {0, 1, 3, 2}, {0, 0, 0, 0}, {0, 2, 3, 1}, {0, 0, 0, 0},
	                                        {0, 0, 0, 0}, {0, 0, 0, 0}, {1, 2, 3, 0}, {0, 2, 1, 3}, {0, 0, 0, 0},
	                                        {0, 3, 1, 2}, {0, 3, 2, 1}, {0, 0, 0, 0}, {0, 0, 0, 0}, {0, 0, 0, 0},
	                                        {1, 3, 2, 0}, {0, 0, 0, 0}, {0, 0, 0, 0}, {0, 0, 0, 0}, {0, 0, 0, 0},
	                                        {0, 0, 0, 0}, {0, 0, 0, 0}, {0, 0, 0, 0}, {0, 0, 0, 0}, {1, 2, 0, 3},
	                                        {0, 0, 0, 0}, {1, 3, 0, 2}, {0, 0, 0, 0}, {0, 0, 0, 0}, {0, 0, 0, 0},
	                                        {2, 3, 0, 1}, {2, 3, 1, 0}, {1, 0, 2, 3}, {1, 0, 3, 2}, {0, 0, 0, 0},
	                                        {0, 0, 0, 0}, {0, 0, 0, 0}, {2, 0, 3, 1}, {0, 0, 0, 0}, {2, 1, 3, 0},
	                                        {0, 0, 0, 0}, {0, 0, 0, 0}, {0, 0, 0, 0}, {0, 0, 0, 0}, {0, 0, 0, 0},
	                                        {0, 0, 0, 0}, {0, 0, 0, 0}, {0, 0, 0, 0}, {2, 0, 1, 3}, {0, 0, 0, 0},
	                                        {0, 0, 0, 0}, {0, 0, 0, 0}, {3, 0, 1, 2}, {3, 0, 2, 1}, {0, 0, 0, 0},
	                                        {3, 1, 2, 0}, {2, 1, 0, 3}, {0, 0, 0, 0}, {0, 0, 0, 0}, {0, 0, 0, 0},
	                                        {3, 1, 0, 2}, {0, 0, 0, 0}, {3, 2, 0, 1}, {3, 2, 1, 0}
	                                    };
	
	
	    #endregion
	
	    // To remove the need for index wrapping, double the permutation table length
	    private static int[] perm = new int[512];
	    //static { for(int i=0; i<512; i++) perm[i]=p[i & 255]; }
	    // A lookup table to traverse the simplex around a given point in 4D.
	    // Details can be found where this table is used, in the 4D noise method.
	
	    public SimplexNoise()
	    {
	        for (int i = 0; i < 512; i++) 
	            perm[i] = p[i & 255];
	    }
	
	    // This method is a *lot* faster than using (int)Math.floor(x)
	    private static int fastfloor(double x)
	    {
	        return x > 0 ? (int)x : (int)x - 1;
	    }
	    private static double dot(int[] g, double x, double y)
	    {
	        return g[0] * x + g[1] * y;
	    }
	    private static double dot(int[] g, double x, double y, double z)
	    {
	        return g[0] * x + g[1] * y + g[2] * z;
	    }
	    private static double dot(int[] g, double x, double y, double z, double w)
	    {
	        return g[0] * x + g[1] * y + g[2] * z + g[3] * w;
	    }
	
	    // Skew the input space to determine which simplex cell we're in
	    static double F3 = 1.0 / 3.0;
	    static double G3 = 1.0 / 6.0; // Very nice and simple unskew factor, too
	
	    // 3D simplex noise
	    public static double Noise(double xin, double yin, double zin)
	    {
	        double n0, n1, n2, n3; // Noise contributions from the four corners
	
	        double s = (xin + yin + zin) * F3; // Very nice and simple skew factor for 3D
	        int i = fastfloor(xin + s);
	        int j = fastfloor(yin + s);
	        int k = fastfloor(zin + s);
	
	        double t = (i + j + k) * G3;
	        double X0 = i - t; // Unskew the cell origin back to (x,y,z) space
	        double Y0 = j - t;
	        double Z0 = k - t;
	        double x0 = xin - X0; // The x,y,z distances from the cell origin
	        double y0 = yin - Y0;
	        double z0 = zin - Z0;
	        // For the 3D case, the simplex shape is a slightly irregular tetrahedron.
	        // Determine which simplex we are in.
	        int i1, j1, k1; // Offsets for second corner of simplex in (i,j,k) coords
	        int i2, j2, k2; // Offsets for third corner of simplex in (i,j,k) coords
	        if (x0 >= y0)
	        {
	            if (y0 >= z0)
	            { i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 1; k2 = 0; } // X Y Z order
	            else if (x0 >= z0) { i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 0; k2 = 1; } // X Z Y order
	            else { i1 = 0; j1 = 0; k1 = 1; i2 = 1; j2 = 0; k2 = 1; } // Z X Y order
	        }
	        else
	        { // x0<y0
	            if (y0 < z0) { i1 = 0; j1 = 0; k1 = 1; i2 = 0; j2 = 1; k2 = 1; } // Z Y X order
	            else if (x0 < z0) { i1 = 0; j1 = 1; k1 = 0; i2 = 0; j2 = 1; k2 = 1; } // Y Z X order
	            else { i1 = 0; j1 = 1; k1 = 0; i2 = 1; j2 = 1; k2 = 0; } // Y X Z order
	        }
	        // A step of (1,0,0) in (i,j,k) means a step of (1-c,-c,-c) in (x,y,z),
	        // a step of (0,1,0) in (i,j,k) means a step of (-c,1-c,-c) in (x,y,z), and
	        // a step of (0,0,1) in (i,j,k) means a step of (-c,-c,1-c) in (x,y,z), where
	        // c = 1/6.    
	        double x1 = x0 - i1 + G3; // Offsets for second corner in (x,y,z) coords
	        double y1 = y0 - j1 + G3;
	        double z1 = z0 - k1 + G3;
	        double x2 = x0 - i2 + 2.0 * G3; // Offsets for third corner in (x,y,z) coords
	        double y2 = y0 - j2 + 2.0 * G3;
	        double z2 = z0 - k2 + 2.0 * G3;
	        double x3 = x0 - 1.0 + 3.0 * G3; // Offsets for last corner in (x,y,z) coords
	        double y3 = y0 - 1.0 + 3.0 * G3;
	        double z3 = z0 - 1.0 + 3.0 * G3;
	        // Work out the hashed gradient indices of the four simplex corners
	        int ii = i & 255;
	        int jj = j & 255;
	        int kk = k & 255;
	        int gi0 = perm[ii + perm[jj + perm[kk]]] % 12;
	        int gi1 = perm[ii + i1 + perm[jj + j1 + perm[kk + k1]]] % 12;
	        int gi2 = perm[ii + i2 + perm[jj + j2 + perm[kk + k2]]] % 12;
	        int gi3 = perm[ii + 1 + perm[jj + 1 + perm[kk + 1]]] % 12;
	        // Calculate the contribution from the four corners
	        double t0 = 0.6 - x0 * x0 - y0 * y0 - z0 * z0;
	        if (t0 < 0) n0 = 0.0;
	        else
	        {
	            t0 *= t0;
	            n0 = t0 * t0 * dot(grad3[gi0], x0, y0, z0);
	        }
	        double t1 = 0.6 - x1 * x1 - y1 * y1 - z1 * z1;
	        if (t1 < 0) n1 = 0.0;
	        else
	        {
	            t1 *= t1;
	            n1 = t1 * t1 * dot(grad3[gi1], x1, y1, z1);
	        }
	        double t2 = 0.6 - x2 * x2 - y2 * y2 - z2 * z2;
	        if (t2 < 0) n2 = 0.0;
	        else
	        {
	            t2 *= t2;
	            n2 = t2 * t2 * dot(grad3[gi2], x2, y2, z2);
	        }
	        double t3 = 0.6 - x3 * x3 - y3 * y3 - z3 * z3;
	        if (t3 < 0) n3 = 0.0;
	        else
	        {
	            t3 *= t3;
	            n3 = t3 * t3 * dot(grad3[gi3], x3, y3, z3);
	        }
	        // Add contributions from each corner to get the final noise value.
	        // The result is scaled to stay just inside [-1,1]
	        return 32.0 * (n0 + n1 + n2 + n3);
	    }
	
	    public double[,,] Generate3D(int w, int h, int d, int x, int y, int z)
	    {
	        // double c = 0;
	        // double max = w * h * d;
	        double[, ,] r = new double[w, h, d];
	        double widthDivisor = 1 / (double)w;
	        double heightDivisor = 1 / (double)h;
	        double depthDivisor = 1 / (double)h;
	
	        for (int i = x; i < w + x; i++)
	            for (int j = y; j < h + y; j++)
	                for (int k = z; k < d + z; k++)
	                {
	                    r[i - x, j - y, k - z] =
	                        (Noise(2 * i * widthDivisor, 2 * j * heightDivisor, 2 * k * depthDivisor) + 1) / 2.0 * 0.7 +
	                        (Noise(4 * i * widthDivisor, 4 * j * heightDivisor, 4 * k * depthDivisor) + 1) / 2.0 * 0.3 +
	                        (Noise(8 * i * widthDivisor, 8 * j * heightDivisor, 8 * k * depthDivisor) + 1) / 2.0 * 0.1;// +
	                    //(Noise(16 * i * widthDivisor, 16 * j * heightDivisor, 16 * k * depthDivisor) + 1) / 2.0 * 0.03 +
	                    //(Noise(32 * i * widthDivisor, 32 * j * heightDivisor, 32 * k * depthDivisor) + 1) / 2.0 * 0.007;
	                }
	        return r;
			
	    }
	
	}
	
}