using FileTypeChecker.Extensions;
using FileTypeChecker.Types;

namespace Forum.Application.Extensions
{
    public static class StreamImageExtension
    {
        public static bool ValidateImageExtension(this Stream stream)
        {
            var result = false;

            if (stream.Is<PortableNetworkGraphic>())
            {
                result = true;
            }
            else if (stream.Is<JointPhotographicExpertsGroup>())
            {
                result = true;
            }

            stream.Position = 0;

            return result;
        }
    }
}
