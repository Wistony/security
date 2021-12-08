namespace Lab3
{
    public static class ExtendedEuclideanAlgorithm
    {
        public static (int,int,int) GetGcd (int a, int b) {
            int x;
            int y;
            if (a == 0) {
                x = 0;
                y = 1;
                return (b, x, y);
            }

            var (d,x1, y1) = GetGcd(b%a, a);
            x = y1 - (b / a) * x1;
            y = x1;
            
            return (d,x,y);
        }
    }
}