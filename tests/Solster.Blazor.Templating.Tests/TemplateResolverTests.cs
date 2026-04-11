using Microsoft.Extensions.Options;
using Solster.Blazor.Templating.Tests.Components;

namespace Solster.Blazor.Templating.Tests;

public sealed class TemplateResolverTests
{
    private static ITemplateResolver BuildResolver() =>
        new TemplateResolver(Options.Create(new TemplateResolverOptions
        {
            TemplateAssembly = typeof(GreetingComponent).Assembly
        }));

    [Fact]
    public void Resolve_KnownTemplateName_ReturnsCorrectType()
    {
        var resolver = BuildResolver();

        var type = resolver.Resolve("GreetingComponent");

        type.Should().Be(typeof(GreetingComponent));
    }

    [Fact]
    public void Resolve_KnownTemplateName_IsCaseInsensitive()
    {
        var resolver = BuildResolver();

        var type = resolver.Resolve("greetingcomponent");

        type.Should().Be(typeof(GreetingComponent));
    }

    [Fact]
    public void Resolve_UnknownTemplateName_ThrowsInvalidOperationException()
    {
        var resolver = BuildResolver();

        var act = () => resolver.Resolve("NonExistentTemplate");

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Resolve_UnknownTemplateName_ExceptionMessageContainsTemplateName()
    {
        var resolver = BuildResolver();

        var act = () => resolver.Resolve("NonExistentTemplate");

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*NonExistentTemplate*");
    }

    [Fact]
    public void Resolve_UnknownTemplateName_ExceptionMessageListsAvailableTemplates()
    {
        var resolver = BuildResolver();

        var act = () => resolver.Resolve("NonExistentTemplate");

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*GreetingComponent*");
    }
}
