namespace Microsoft.Web.Http
{
    using Microsoft.Web.Http.Versioning;

    /// <summary>
    /// An attribute used to decorate an endpoint that will respond
    /// to a range of versions.
    /// </summary>
    public class RangeApiVersionAttribute : ApiVersionsBaseAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RangeApiVersionAttribute"/> class.
        /// </summary>
        /// <param name="minimum">The minimum supported API version.</param>
        /// <param name="maximum">The maximum supported API version.</param>
        public RangeApiVersionAttribute( ApiVersion minimum, ApiVersion maximum )
            : base(minimum, maximum)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeApiVersionAttribute"/> class.
        /// </summary>
        /// <param name="minimum">The minimum supported API version.</param>
        /// <param name="maximum">The maximum supported API version.</param>
        public RangeApiVersionAttribute( string minimum, string maximum )
            : base( minimum, maximum )
        {
        }
    }
}