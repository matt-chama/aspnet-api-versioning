﻿#if WEBAPI
namespace Microsoft.Web.Http.Versioning
#else
namespace Microsoft.AspNetCore.Mvc.Versioning
#endif
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using static ApiVersion;

    /// <summary>
    /// Represents the base implementation for the metadata that describes the <see cref="ApiVersion">API versions</see> associated with a service.
    /// </summary>
    [SuppressMessage( "Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "An accessor property is provided, but the values are typed; not strings." )]
    public abstract class ApiVersionsBaseAttribute : Attribute
    {
        readonly Lazy<int> computedHashCode;
        readonly Lazy<IReadOnlyList<ApiVersion>> versions;
        readonly bool isRange = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiVersionsBaseAttribute"/> class.
        /// </summary>
        /// <param name="version">The <see cref="ApiVersion">API version</see>.</param>
        protected ApiVersionsBaseAttribute( ApiVersion version ) : this( new[] { version } ) => Arg.NotNull( version, nameof( version ) );

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiVersionsBaseAttribute"/> class.
        /// </summary>
        /// <param name="versions">An <see cref="Array">array</see> of <see cref="ApiVersion">API versions</see>.</param>
        protected ApiVersionsBaseAttribute( params ApiVersion[] versions )
        {
            Arg.NotNull( versions, nameof( versions ) );

            computedHashCode = new Lazy<int>( () => ComputeHashCode( versions ) );
            this.versions = new Lazy<IReadOnlyList<ApiVersion>>( () => versions );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiVersionsBaseAttribute"/> class.
        /// </summary>
        /// <param name="minimum">The minimum supported API version.</param>
        /// <param name="maximum">The maximum supported API version.</param>
        protected ApiVersionsBaseAttribute( ApiVersion minimum, ApiVersion maximum ) : this(new[] { minimum, maximum } )
        {
            isRange = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiVersionsBaseAttribute"/> class.
        /// </summary>
        /// <param name="version">The API version string.</param>
        public ApiVersionsBaseAttribute( string version ) : this( new[] { version } ) => Arg.NotNullOrEmpty( version, nameof( version ) );

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiVersionsBaseAttribute"/> class.
        /// </summary>
        /// <param name="versions">An <see cref="Array">array</see> of API version strings.</param>
        [CLSCompliant( false )]
        public ApiVersionsBaseAttribute( params string[] versions )
        {
            Arg.NotNull( versions, nameof( versions ) );

            computedHashCode = new Lazy<int>( () => ComputeHashCode( Versions ) );
            this.versions = new Lazy<IReadOnlyList<ApiVersion>>( () => versions.Select( Parse ).Distinct().ToSortedReadOnlyList() );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiVersionsBaseAttribute"/> class.
        /// </summary>
        /// <param name="minimum">The minimum supported API version.</param>
        /// <param name="maximum">The maximum supported API version.</param>
        public ApiVersionsBaseAttribute( string minimum, string maximum ) : this(new[] { minimum, maximum })
        {
            isRange = true;
        }

        static int ComputeHashCode( IEnumerable<ApiVersion> versions )
        {
            Contract.Requires( versions != null );

            var hashCode = 0;

            using ( var iterator = versions.GetEnumerator() )
            {
                if ( !iterator.MoveNext() )
                {
                    return hashCode;
                }

                hashCode = iterator.Current.GetHashCode();

                unchecked
                {
                    while ( iterator.MoveNext() )
                    {
                        hashCode = ( hashCode * 397 ) ^ iterator.Current.GetHashCode();
                    }
                }
            }

            return hashCode;
        }

        /// <summary>
        /// Gets the API versions defined by the attribute.
        /// </summary>
        /// <value>A <see cref="IReadOnlyList{T}">read-only list</see> of <see cref="ApiVersion">API versions</see>.</value>
        public IReadOnlyList<ApiVersion> Versions => versions.Value;

        /// <summary>
        /// Returns a value indicating whether the specified object is equal to the current instance.
        /// </summary>
        /// <param name="obj">The <see cref="object">object</see> to be evaluated.</param>
        /// <returns>True if the current instance equals the specified object; otherwise, false.</returns>
        public override bool Equals( object obj ) => ( obj is ApiVersionsBaseAttribute ) && GetHashCode() == obj.GetHashCode();

        /// <summary>
        /// Returns a hash code for the current instance.
        /// </summary>
        /// <returns>A hash code.</returns>
        public override int GetHashCode() => computedHashCode.Value;
    }
}