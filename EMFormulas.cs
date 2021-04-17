using System;
using NumSharp;

namespace EMSpace
{
    class Program
    {

        static void Main(string[] args)
        {
            var r_s = np.array(new double[] { -1, -2, 0 });
            r_s = r_s.reshape(1, 3);

            var r_m = np.array(new double[] { 0, 0, 0 });
            r_m = r_m.reshape(1, 3);

            var q = np.array(new double[] { 1.6e-19 });
            q = q.reshape(1, 1);

            var PC = PointCharge(q, r_s, r_m);

            
      
            Console.WriteLine(PC.ToString());
        }

        public static NDArray PointCharge(NDArray q, NDArray r_s, NDArray r_m)
        {
            // Calculates the EM field of an assembly of point charges
            // r_m == measurement position (1,3) array
            // r_s == source position (N,3) array
            // q == charge (N,1) array
            // N == # of sources
            double k = 9e9;


            //Define the distance from the source to our measurement
            var r = r_m - r_s;

            // Calculate r^2
            var rSqrd = r[":, 0"] * r[":, 0"] + r[":, 1"] * r[":, 1"] + r[":, 2"] * r[":, 2"];
            
            // Reshape stuff
            rSqrd = rSqrd.reshape(np.size(rSqrd), 1);
            q = q.reshape(np.size(q), 1);

            // Calculate unit vector of EM field

            var rHat = r / np.sqrt(rSqrd);

            // Use Coulomb's Law to analytically calculate the EM field
            var E_array = k * q / rSqrd * rHat;

            // I couldn't figure out how to sum the x, y, z contributions from multiple sources. Some issue with dtype return of np.sum

            //var Ex_array = E_array[":,0"];
            //Ex_array = Ex_array.reshape(np.size(Ex_array));
            //var Ey_array = E_array[":,1"];
            //Ey_array = Ey_array.reshape(np.size(Ey_array));
            //var Ez_array = E_array[":,2"];
            //Ez_array = Ez_array.reshape(np.size(Ez_array));

            //var retArray = np.array(new double[] { np.sum(Ex_array), np.sum(Ey_array), np.sum(Ez_array) });
            
            return E_array;



            
        }
    }
}
