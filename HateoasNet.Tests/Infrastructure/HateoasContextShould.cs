using FluentAssertions;
using HateoasNet.Abstractions;
using HateoasNet.Infrastructure;
using HateoasNet.Tests.TestHelpers;
using System;
using System.Reflection;
using Xunit;

namespace HateoasNet.Tests.Infrastructure
{
    public class HateoasContextShould : IDisposable
    {
        private readonly IHateoasContext _sut;
        public HateoasContextShould()
        {
            _sut = new HateoasContext();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


        [Fact]
        [Trait(nameof(IHateoasContext), "New")]
        public void New_WithOutParameters_HateoasContext()
        {
            _sut.Should().BeAssignableTo<IHateoasContext>().And.BeOfType<HateoasContext>();
        }

        [Fact]
        [Trait(nameof(IHateoasContext), nameof(HateoasContext.GetOrInsert))]
        public void GetOrInsert_WithClassType_ReturnIHateoasSource()
        {
            // act
            var interfaceHateoasSource = (_sut as HateoasContext)?.GetOrInsert(typeof(HateoasSample));
            var stronglyTypedHateoasSource = (_sut as HateoasContext)?.GetOrInsert<HateoasSample>();

            // assert
            stronglyTypedHateoasSource.Should().NotBeNull().And.BeSameAs(interfaceHateoasSource);
        }

        [Fact]
        [Trait(nameof(IHateoasContext), nameof(IHateoasContext.GetApplicableLinkBuilders))]
        public void GetApplicableLinks_WithNotConfiguredType_ReturnEmptyLinkBuilders()
        {
            _sut.GetApplicableLinkBuilders(new HateoasSample())
                .Should().NotBeNull().And.BeEmpty();
        }

        [Fact]
        [Trait(nameof(IHateoasContext), nameof(IHateoasContext.Configure))]
        [Trait(nameof(IHateoasContext), nameof(IHateoasContext.GetApplicableLinkBuilders))]
        public void GetApplicableLinks_WithConfiguredType_ReturnLinkBuilders()
        {
            _sut.Configure<HateoasSample>(resource =>
            {
                resource.AddLink("test")
                        .When(x => x.Id != null && x.Id != Guid.Empty)
                        .HasRouteData(x => new { id = x.Id });
            }).GetApplicableLinkBuilders(new HateoasSample())
              .Should().NotBeNull().And.NotBeEmpty();
        }

        [Fact]
        [Trait(nameof(IHateoasContext), nameof(IHateoasContext.ApplyConfiguration))]
        [Trait(nameof(IHateoasContext), nameof(IHateoasContext.GetApplicableLinkBuilders))]
        public void GetApplicableLinks_WithApplyConfigurationForType_ReturnLinkBuilders()
        {
            _sut.ApplyConfiguration(new HateoasSampleBuilder())
                .GetApplicableLinkBuilders(new HateoasSample())
                .Should().NotBeNull().And.NotBeEmpty();
        }

        [Fact]
        [Trait(nameof(IHateoasContext), nameof(IHateoasContext.ConfigureFromAssembly))]
        [Trait(nameof(IHateoasContext), nameof(IHateoasContext.GetApplicableLinkBuilders))]
        public void GetApplicableLinks_WithTypeConfiguredFromAssembly_ReturnLinkBuilders()
        {
            _sut.ConfigureFromAssembly(new HateoasSampleBuilder().GetType().Assembly)
                .GetApplicableLinkBuilders(new HateoasSample())
                .Should().NotBeNull().And.NotBeEmpty();
        }

        [Fact]
        [Trait(nameof(IHateoasContext), nameof(IHateoasContext.ApplyConfiguration))]
        [Trait(nameof(IHateoasContext), "Exceptions")]
        public void ApplyConfiguration_WithResourceNull_ThrowsArgumentNullException()
        {
            // arrange
            const string parameterName = "configuration";

            // act
            Action actual = () => _sut.ApplyConfiguration<HateoasSample>(null);

            Assert.Throws<ArgumentNullException>(parameterName, actual);
        }

        [Fact]
        [Trait(nameof(IHateoasContext), nameof(IHateoasContext.Configure))]
        [Trait(nameof(IHateoasContext), "Exceptions")]
        public void Configure_WithResourceNull_ThrowsArgumentNullException()
        {
            // arrange
            const string parameterName = "resource";

            // act
            Action actual = () => _sut.Configure<HateoasSample>(null);

            // assert
            Assert.Throws<ArgumentNullException>(parameterName, actual);
        }

        [Fact]
        [Trait(nameof(IHateoasContext), nameof(IHateoasContext.ConfigureFromAssembly))]
        [Trait(nameof(IHateoasContext), "Exceptions")]
        public void ConfigureFromAssembly_WhenHasNo_IHateoasSourceConfiguration_ThrowsTargetException()
        {
            // act
            Action actual = () => _sut.ConfigureFromAssembly(typeof(string).Assembly);

            // assert
            Assert.Throws<TargetException>(actual);
        }

        [Fact]
        [Trait(nameof(IHateoasContext), nameof(IHateoasContext.ConfigureFromAssembly))]
        [Trait(nameof(IHateoasContext), "Exceptions")]
        public void ConfigureFromAssembly_WithResourceNull_ThrowsArgumentNullException()
        {
            // arrange
            const string parameterName = "assembly";

            // act
            Action actual = () => _sut.ConfigureFromAssembly(null);

            // assert
            Assert.Throws<ArgumentNullException>(parameterName, actual);
        }
    }
}
