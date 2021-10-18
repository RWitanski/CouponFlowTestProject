using Xunit;
using Flurl.Http;
using Flurl;
using System.Collections.Generic;
using Models.TrendingOffers;
using System.Linq;

namespace CouponFlowTestProject
{
    public class TrendingOffersTests
    {
        private readonly string _baseUrl = "https://couponfollow.com/api";
        
        [Fact]
        public async void GivenVersionHeaderEndpointReturnsAListOfOffers()
        {
            var result = await _baseUrl.AppendPathSegment("extension/trendingOffers")
                .WithHeader("catc-version", "5.0.0")
                .GetJsonAsync<IEnumerable<Offer>>();

            Assert.NotEmpty(result);
            Assert.IsType<List<Offer>>(result);
        }

        [Fact]
        public async void GivenMissingHeaderEndpointReturnsForbidderHttpError()
        {
            var result = await _baseUrl.AppendPathSegment("extension/trendingOffers")
                .AllowAnyHttpStatus()
                .GetAsync();

            Assert.True(result.StatusCode.Equals(403));
        }

        [Fact]
        public async void GivenOffersCountIsNotHigherThan20()
        {
            var result = await _baseUrl.AppendPathSegment("extension/trendingOffers")
                .WithHeader("catc-version", "5.0.0")
                .GetJsonAsync<IEnumerable<Offer>>();

            Assert.True(result.Count() <= 20);
        }

        [Fact]
        public async void GivenOffersAreUniqueInTermsInDomainName()
        {
            var result = await _baseUrl.AppendPathSegment("extension/trendingOffers")
                .WithHeader("catc-version", "5.0.0")
                .GetJsonAsync<IEnumerable<Offer>>();

            var isUnique = result.GroupBy(x => x.DomainName)
                .Max(x => x.Count()) == 1;

            Assert.True(isUnique);
        }
    }
}