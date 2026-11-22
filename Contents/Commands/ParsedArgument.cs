namespace MonoCoC.Contents.Commands
{
    public sealed record ParsedArgument(string Name, StringRange Range, object? Result);
}
