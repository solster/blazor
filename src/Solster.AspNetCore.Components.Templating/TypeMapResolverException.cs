namespace Solster.AspNetCore.Components.Templating;

public sealed class TypeMapResolverException(
    String message,
    Exception? innerException = null)
    : Exception(message, innerException);

