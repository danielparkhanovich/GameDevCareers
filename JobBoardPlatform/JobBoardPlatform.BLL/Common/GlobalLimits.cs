namespace JobBoardPlatform.BLL.Common
{
    public partial class GlobalBLL
    {
        public class Limits
        {
            public const int MaximumProfileImageSizeInMb = 1;
            public const int MaximumResumeSizeInMb = 5;

            private const int MbToBytes = 1_000_000;


            public static int GetValueInBytesFromMb(int valueInMb)
            {
                return valueInMb * MbToBytes;
            }

            public static int GetValueInMbFromBytes(int valueInMb)
            {
                return valueInMb / MbToBytes;
            }
        }
    }
}
