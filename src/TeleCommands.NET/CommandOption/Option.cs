using System.Collections.Immutable;
using TeleCommands.NET.CommandOption.Interfaces;
using TeleCommands.NET.CommandOption.OptionStructs;

namespace TeleCommands.NET.CommandOption
{
    internal abstract class Option<T, TSource> where T : Option<T, TSource> 
    {
        public virtual ImmutableArray<Argument<T, TSource>> Arguments { get; }

        public abstract Task<IResult<TSource>> ExecuteOptionAsync(OptionData data);
    }
}
