using CommonTestUtilities.Requests;
using System.Collections;

namespace UseCases.Test.User.InlineData
{
    public class ImageTypeInlineData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var images = FormFileBuilder.ImageCollection();
            foreach (var image in images)
            {
                yield return new object[] { image };
            }
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
