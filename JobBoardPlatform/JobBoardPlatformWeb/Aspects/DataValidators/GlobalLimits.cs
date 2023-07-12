namespace JobBoardPlatform.PL.Aspects.DataValidators
{
    public static class GlobalLimits
    {
        public const int MaximumProfileImageSizeInMb = 1;
        public const int MaximumResumeSizeInMb = 5;

        public static int GetValueInBytesFromMb(int valueInMb)
        {
            return valueInMb * (int)Math.Pow(10, 6);
        }
    }
}
