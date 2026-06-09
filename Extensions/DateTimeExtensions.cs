using ProDateTime.Constants;

namespace ProDateTime.Extensions;

/// <summary>
/// Fluent extension methods on <see cref="DateTime"/> and <see cref="DateTimeOffset"/>
/// that delegate to a shared <see cref="ProDateTimeConverter"/> instance.
/// </summary>
public static class DateTimeExtensions
{
    // Shared default converter — replace with a custom instance if needed
    private static ProDateTimeConverter _defaultConverter = new();

    /// <summary>
    /// Replaces the shared default converter used by all extension methods.
    /// Useful in tests or when you have a subclassed converter.
    /// </summary>
    /// <param name="converter">Custom converter instance to use globally.</param>
    public static void UseConverter(ProDateTimeConverter converter)
    {
        ArgumentNullException.ThrowIfNull(converter, nameof(converter));
        _defaultConverter = converter;
    }

    // ──────────────────────────────────────────────────────────────────────────
    //  DateTime extensions
    // ──────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Converts this <see cref="DateTime"/> to the specified timezone.
    /// </summary>
    /// <param name="dateTime">Source date/time.</param>
    /// <param name="targetTimezoneId">
    /// IANA or Windows timezone id.
    /// Prefer constants from <see cref="TimezoneConstant"/>.
    /// </param>
    /// <returns>Wall-clock time in the target timezone.</returns>
    /// <example>
    /// <code>
    /// DateTime kolkata = DateTime.UtcNow.ConvertToTimezone(TimezoneConstant.AsiaKolkata);
    /// </code>
    /// </example>
    public static DateTime ConvertToTimezone(this DateTime dateTime, string targetTimezoneId)
        => _defaultConverter.ConvertToTimezone(dateTime, targetTimezoneId);

    /// <summary>
    /// Converts this <see cref="DateTime"/> to UTC.
    /// </summary>
    public static DateTime ToUtc(this DateTime dateTime)
        => _defaultConverter.ConvertToTimezone(dateTime, TimezoneConstant.Utc);

    /// <summary>
    /// Converts this <see cref="DateTime"/> to India Standard Time (UTC+05:30).
    /// </summary>
    public static DateTime ToIndiaTime(this DateTime dateTime)
        => _defaultConverter.ConvertToTimezone(dateTime, TimezoneConstant.AsiaKolkata);

    /// <summary>
    /// Converts this <see cref="DateTime"/> to Eastern Time (New York).
    /// </summary>
    public static DateTime ToEasternTime(this DateTime dateTime)
        => _defaultConverter.ConvertToTimezone(dateTime, TimezoneConstant.AmericaNewYork);

    /// <summary>
    /// Converts this <see cref="DateTime"/> to Japan Standard Time (UTC+09:00).
    /// </summary>
    public static DateTime ToJapanTime(this DateTime dateTime)
        => _defaultConverter.ConvertToTimezone(dateTime, TimezoneConstant.AsiaTokyo);

    /// <summary>
    /// Converts this <see cref="DateTime"/> to London time (GMT/BST).
    /// </summary>
    public static DateTime ToLondonTime(this DateTime dateTime)
        => _defaultConverter.ConvertToTimezone(dateTime, TimezoneConstant.EuropeLondon);

    // ──────────────────────────────────────────────────────────────────────────
    //  DateTimeOffset extensions
    // ──────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Converts this <see cref="DateTimeOffset"/> to the specified timezone.
    /// </summary>
    /// <param name="dateTimeOffset">Source date/time offset.</param>
    /// <param name="targetTimezoneId">
    /// IANA or Windows timezone id.
    /// Prefer constants from <see cref="TimezoneConstant"/>.
    /// </param>
    /// <returns>
    /// A <see cref="DateTimeOffset"/> representing the same instant
    /// expressed in the target timezone's UTC offset.
    /// </returns>
    public static DateTimeOffset ConvertToTimezone(this DateTimeOffset dateTimeOffset, string targetTimezoneId)
        => _defaultConverter.ConvertToTimezone(dateTimeOffset, targetTimezoneId);

    /// <summary>
    /// Converts this <see cref="DateTimeOffset"/> to India Standard Time.
    /// </summary>
    public static DateTimeOffset ToIndiaTime(this DateTimeOffset dateTimeOffset)
        => _defaultConverter.ConvertToTimezone(dateTimeOffset, TimezoneConstant.AsiaKolkata);

    /// <summary>
    /// Converts this <see cref="DateTimeOffset"/> to UTC.
    /// </summary>
    public static DateTimeOffset ToUtc(this DateTimeOffset dateTimeOffset)
        => dateTimeOffset.ToUniversalTime();
}
