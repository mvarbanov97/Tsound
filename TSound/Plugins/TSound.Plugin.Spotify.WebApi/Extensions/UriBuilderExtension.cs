using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TSound.Plugin.Spotify.WebApi.Extensions
{
    public static class UriBuilderExtensions
    {
        public static void AppendToQuery(this UriBuilder builder, string name, int value)
            => builder.AppendToQuery(name, value.ToString());

        public static void AppendToQuery(this UriBuilder builder, string name, long value)
            => builder.AppendToQuery(name, value.ToString());

        public static void AppendToQuery(this UriBuilder builder, string name, string value)
        {
            if (string.IsNullOrEmpty(builder.Query)) builder.Query = $"{name}={value}";
            else builder.Query = $"{builder.Query.Substring(1)}&{name}={value}";
        }

        public static void AppendToQueryAsCsv(this UriBuilder builder, string name, string[] values)
        {
            if (values != null && values.Length > 0)
                builder.AppendToQuery(name, string.Join(",", values));
        }

        public static void AppendToQueryIfValueGreaterThan0(
            this UriBuilder builder,
            string name,
            long? value)
        {
            if (value.HasValue) builder.AppendToQueryIfValueGreaterThan0(name, value.Value);
        }

        public static void AppendToQueryIfValueGreaterThan0(
            this UriBuilder builder,
            string name,
            long value)
        {
            if (value > 0) builder.AppendToQuery(name, value);
        }

        public static void AppendToQueryIfValueGreaterThan0(
            this UriBuilder builder,
            string name,
            int? value)
        {
            if (value.HasValue) builder.AppendToQueryIfValueGreaterThan0(name, value.Value);
        }

        public static void AppendToQueryIfValueGreaterThan0(
            this UriBuilder builder,
            string name,
            int value)
        {
            if (value > 0) builder.AppendToQuery(name, value);
        }

        public static void AppendToQueryIfValueNotNullOrWhiteSpace(
            this UriBuilder builder,
            string name,
            string value)
        {
            if (!string.IsNullOrWhiteSpace(value)) builder.AppendToQuery(name, value);
        }

        public static void AppendToQueryAsTimestampIso8601(this UriBuilder builder, string name, DateTime? timestamp)
        {
            if (timestamp.HasValue) builder.AppendToQuery(name, timestamp.Value.ToString("s", CultureInfo.InvariantCulture));
        }
    }
}
