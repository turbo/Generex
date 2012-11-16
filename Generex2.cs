﻿using System;
using System.Collections.Generic;
using RT.Util.ExtensionMethods;

namespace RT.Generexes
{
    /// <summary>
    /// Provides regular-expression functionality for collections of arbitrary objects.
    /// </summary>
    /// <typeparam name="T">Type of the objects in the collection.</typeparam>
    /// <typeparam name="TResult">Type of objects generated from each match of the regular expression.</typeparam>
    /// <remarks>This type is not directly instantiated; use <see cref="Generex{T}.Process"/>.</remarks>
    public sealed class Generex<T, TResult> : GenerexWithResultBase<T, TResult, Generex<T, TResult>, GenerexMatch<T, TResult>>
    {
        internal sealed override GenerexMatch<T, TResult> createMatchWithResult(TResult result, T[] input, int index, int length)
        {
            return new GenerexMatch<T, TResult>(result, input, index, length);
        }

        internal Generex(matcher forward, matcher backward) : base(forward, backward) { }
        static Generex() { Constructor = (forward, backward) => new Generex<T, TResult>(forward, backward); }

        /// <summary>Processes each match of this regular expression by running it through a provided selector.</summary>
        /// <typeparam name="TOtherResult">Type of the object returned by <paramref name="selector"/>.</typeparam>
        /// <param name="selector">Function to process a regular expression match.</param>
        public Generex<T, TOtherResult> Process<TOtherResult>(Func<GenerexMatch<T, TResult>, TOtherResult> selector)
        {
            return base.Process<Generex<T, TOtherResult>, GenerexMatch<T, TOtherResult>, TOtherResult>(selector);
        }

        /// <summary>Processes each match of this regular expression by running each result through a provided selector.</summary>
        /// <typeparam name="TOtherResult">Type of the object returned by <paramref name="selector"/>.</typeparam>
        /// <param name="selector">Function to process the result of a regular expression match.</param>
        public Generex<T, TOtherResult> ProcessRaw<TOtherResult>(Func<TResult, TOtherResult> selector)
        {
            return base.ProcessRaw<Generex<T, TOtherResult>, GenerexMatch<T, TOtherResult>, TOtherResult>(selector);
        }

        /// <summary>
        /// Returns a regular expression that matches this regular expression, followed by the specified ones,
        /// and generates a match object that combines the result of this regular expression with the match of the other.
        /// </summary>
        public Generex<T, TCombined> Then<TCombined>(Generex<T> other, Func<TResult, GenerexMatch<T>, TCombined> selector)
        {
            return base.Then<Generex<T>, GenerexMatch<T>, Generex<T, TCombined>, GenerexMatch<T, TCombined>, TCombined>(other, selector);
        }

        /// <summary>
        /// Returns a regular expression that matches this regular expression, followed by the specified one,
        /// and generates a match object that combines the result of this regular expression with the match of the other.
        /// </summary>
        public Generex<T, TCombined> Then<TOther, TCombined>(Generex<T, TOther> other, Func<TResult, GenerexMatch<T, TOther>, TCombined> selector)
        {
            return base.Then<Generex<T, TOther>, GenerexMatch<T, TOther>, TOther, Generex<T, TCombined>, GenerexMatch<T, TCombined>, TCombined>(other, selector);
        }

        /// <summary>
        /// Returns a regular expression that matches this regular expression, followed by the specified ones,
        /// and generates a match object that combines the original two matches.
        /// </summary>
        public Generex<T, TCombined> ThenRaw<TOther, TCombined>(Generex<T, TOther> other, Func<TResult, TOther, TCombined> selector)
        {
            return base.ThenRaw<Generex<T, TOther>, GenerexMatch<T, TOther>, TOther, Generex<T, TCombined>, GenerexMatch<T, TCombined>, TCombined>(other, selector);
        }

        /// <summary>
        /// Returns a regular expression that matches this regular expression zero times or once. Once is prioritised (cf. "?" in traditional regular expression syntax).
        /// </summary>
        public Generex<T, IEnumerable<TResult>> OptionalGreedy() { return repeatBetween<Generex<T, IEnumerable<TResult>>, GenerexMatch<T, IEnumerable<TResult>>>(0, 1, greedy: true); }
        /// <summary>
        /// Returns a regular expression that matches this regular expression zero times or once. Zero times is prioritised (cf. "??" in traditional regular expression syntax).
        /// </summary>
        public Generex<T, IEnumerable<TResult>> Optional() { return repeatBetween<Generex<T, IEnumerable<TResult>>, GenerexMatch<T, IEnumerable<TResult>>>(0, 1, greedy: false); }
        /// <summary>
        /// Returns a regular expression that matches this regular expression zero or more times. More times are prioritised (cf. "*" in traditional regular expression syntax).
        /// </summary>
        public Generex<T, IEnumerable<TResult>> RepeatGreedy() { return repeatInfinite<Generex<T, IEnumerable<TResult>>, GenerexMatch<T, IEnumerable<TResult>>>(greedy: true); }
        /// <summary>
        /// Returns a regular expression that matches this regular expression zero or more times. Fewer times are prioritised (cf. "*?" in traditional regular expression syntax).
        /// </summary>
        public Generex<T, IEnumerable<TResult>> Repeat() { return repeatInfinite<Generex<T, IEnumerable<TResult>>, GenerexMatch<T, IEnumerable<TResult>>>(greedy: false); }
        /// <summary>
        /// Returns a regular expression that matches this regular expression the specified number of times or more. More times are prioritised (cf. "{min,}" in traditional regular expression syntax).
        /// </summary>
        public Generex<T, IEnumerable<TResult>> RepeatGreedy(int min) { return repeatMin<Generex<T, IEnumerable<TResult>>, GenerexMatch<T, IEnumerable<TResult>>>(min, greedy: true); }
        /// <summary>
        /// Returns a regular expression that matches this regular expression the specified number of times or more. Fewer times are prioritised (cf. "{min,}?" in traditional regular expression syntax).
        /// </summary>
        public Generex<T, IEnumerable<TResult>> Repeat(int min) { return repeatMin<Generex<T, IEnumerable<TResult>>, GenerexMatch<T, IEnumerable<TResult>>>(min, greedy: false); }
        /// <summary>
        /// Returns a regular expression that matches this regular expression any number of times within specified boundaries. More times are prioritised (cf. "{min,max}" in traditional regular expression syntax).
        /// </summary>
        /// <param name="min">Minimum number of times to match.</param>
        /// <param name="max">Maximum number of times to match.</param>
        public Generex<T, IEnumerable<TResult>> RepeatGreedy(int min, int max) { return repeatBetween<Generex<T, IEnumerable<TResult>>, GenerexMatch<T, IEnumerable<TResult>>>(min, max, greedy: true); }
        /// <summary>
        /// Returns a regular expression that matches this regular expression any number of times within specified boundaries. Fewer times are prioritised (cf. "{min,max}?" in traditional regular expression syntax).
        /// </summary>
        /// <param name="min">Minimum number of times to match.</param>
        /// <param name="max">Maximum number of times to match.</param>
        public Generex<T, IEnumerable<TResult>> Repeat(int min, int max) { return repeatBetween<Generex<T, IEnumerable<TResult>>, GenerexMatch<T, IEnumerable<TResult>>>(min, max, greedy: false); }
        /// <summary>
        /// Returns a regular expression that matches this regular expression the specified number of times (cf. "{times}" in traditional regular expression syntax).
        /// </summary>
        public Generex<T, IEnumerable<TResult>> Times(int times)
        {
            if (times < 0) throw new ArgumentException("'times' cannot be negative.", "times");
            return repeatBetween<Generex<T, IEnumerable<TResult>>, GenerexMatch<T, IEnumerable<TResult>>>(times, times, true);
        }

        /// <summary>
        /// Returns a regular expression that matches this regular expression one or more times, interspersed with a separator. Fewer times are prioritised.
        /// </summary>
        public Generex<T, IEnumerable<TResult>> RepeatWithSeparator(Generex<T> separator)
        {
            return ThenRaw(separator.Then(this).Repeat(), IEnumerableExtensions.Concat);
        }
        /// <summary>
        /// Returns a regular expression that matches this regular expression one or more times, interspersed with a separator. More times are prioritised.
        /// </summary>
        public Generex<T, IEnumerable<TResult>> RepeatWithSeparatorGreedy(Generex<T> separator)
        {
            return ThenRaw(separator.Then(this).RepeatGreedy(), IEnumerableExtensions.Concat);
        }
    }
}
