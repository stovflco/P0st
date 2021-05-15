using Xamarin.Forms;

namespace P0st
{
    public class CaptchaImage : Image
    {
        protected override SizeRequest OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            return new SizeRequest(new Size(widthConstraint, widthConstraint * this.AspectRatio));
        }

        public double AspectRatio { get; set; }
    }
}