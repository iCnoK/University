namespace Paint.Utility
{
    public static class CustomColorConverter
    {
        public static System.Drawing.Color ConvertFromSWMCToSDC(System.Windows.Media.Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static System.Windows.Media.Color ConvertFromSDCToSWMC(System.Drawing.Color color)
        {
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}
