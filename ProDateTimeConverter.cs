using ProDateTime.Constants;
using ProDateTime.Interfaces;

namespace ProDateTime;

/// <summary>
/// Default implementation of <see cref="IDateTimeConverter"/>.
/// Provides timezone conversion using the BCL <see cref="TimeZoneInfo"/> API
/// with automatic fallback between IANA and Windows timezone identifiers
/// (requires .NET 6+ on Windows).
///
/// <para>Override <see cref="ResolveTimeZone"/> in a derived class to plug in
/// a custom timezone provider (e.g. NodaTime, mocked zones for unit tests).</para>
/// </summary>
/// <example>
/// <code>
/// var converter = new ProDateTimeConverter();
///
/// // Using a constant
/// DateTime ist = DateTime.UtcNow;
/// DateTime jst = converter.ConvertToTimezone(ist, TimezoneConstant.AsiaTokyo);
///
/// // Fluent extension
/// DateTime ny = DateTime.UtcNow.ConvertToTimezone(TimezoneConstant.AmericaNewYork);
/// </code>
/// </example>
public class ProDateTimeConverter : IDateTimeConverter
{
    // ──────────────────────────────────────────────────────────────────────────
    //  IDateTimeConverter — explicit interface implementation
    // ──────────────────────────────────────────────────────────────────────────

    /// <inheritdoc/>
    DateTime IDateTimeConverter.Convert(DateTime dateTime, string targetTimezoneId)
        => ConvertToTimezone(dateTime, targetTimezoneId);

    /// <inheritdoc/>
    DateTimeOffset IDateTimeConverter.Convert(DateTimeOffset dateTimeOffset, string targetTimezoneId)
        => ConvertToTimezone(dateTimeOffset, targetTimezoneId);

    // ──────────────────────────────────────────────────────────────────────────
    //  Public overridable API
    // ──────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Converts a <see cref="DateTime"/> to the target timezone.
    /// <para>
    /// If <paramref name="dateTime"/> has <see cref="DateTimeKind.Unspecified"/>,
    /// it is treated as UTC before conversion.
    /// </para>
    /// </summary>
    /// <param name="dateTime">Source date/time value.</param>
    /// <param name="targetTimezoneId">
    /// IANA or Windows timezone id.
    /// Use <see cref="TimezoneConstant"/> constants for type-safe calls.
    /// </param>
    /// <returns>Local wall-clock time in the target timezone (Kind = Unspecified).</returns>
    public virtual DateTime ConvertToTimezone(DateTime dateTime, string targetTimezoneId)
    {
        ArgumentNullException.ThrowIfNull(targetTimezoneId, nameof(targetTimezoneId));

        TimeZoneInfo tz = ResolveTimeZone(targetTimezoneId);

        // Treat Unspecified as UTC so behaviour is deterministic
        DateTime utcSource = dateTime.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)
            : dateTime;

        return TimeZoneInfo.ConvertTime(utcSource, tz);
    }

    /// <summary>
    /// Converts a <see cref="DateTimeOffset"/> to the target timezone.
    /// </summary>
    /// <param name="dateTimeOffset">Source date/time offset value.</param>
    /// <param name="targetTimezoneId">
    /// IANA or Windows timezone id.
    /// Use <see cref="TimezoneConstant"/> constants for type-safe calls.
    /// </param>
    /// <returns>
    /// A <see cref="DateTimeOffset"/> with the correct UTC offset for the
    /// target timezone at the given instant.
    /// </returns>
    public virtual DateTimeOffset ConvertToTimezone(DateTimeOffset dateTimeOffset, string targetTimezoneId)
    {
        ArgumentNullException.ThrowIfNull(targetTimezoneId, nameof(targetTimezoneId));

        TimeZoneInfo tz = ResolveTimeZone(targetTimezoneId);
        return TimeZoneInfo.ConvertTime(dateTimeOffset, tz);
    }

    /// <summary>
    /// Returns the current UTC time converted to the target timezone.
    /// </summary>
    /// <param name="targetTimezoneId">
    /// IANA or Windows timezone id.
    /// Use <see cref="TimezoneConstant"/> constants for type-safe calls.
    /// </param>
    /// <returns>Current wall-clock time in the target timezone.</returns>
    public virtual DateTime NowIn(string targetTimezoneId)
        => ConvertToTimezone(DateTime.UtcNow, targetTimezoneId);

    /// <summary>
    /// Returns the UTC offset for a given timezone at the current instant.
    /// </summary>
    /// <param name="timezoneId">
    /// IANA or Windows timezone id.
    /// Use <see cref="TimezoneConstant"/> constants for type-safe calls.
    /// </param>
    /// <returns>The <see cref="TimeSpan"/> UTC offset.</returns>
    public virtual TimeSpan GetUtcOffset(string timezoneId)
    {
        TimeZoneInfo tz = ResolveTimeZone(timezoneId);
        return tz.GetUtcOffset(DateTime.UtcNow);
    }

    /// <summary>
    /// Returns a human-readable display name for the timezone, e.g.
    /// "(UTC+05:30) Chennai, Kolkata, Mumbai, New Delhi".
    /// </summary>
    /// <param name="timezoneId">
    /// IANA or Windows timezone id.
    /// Use <see cref="TimezoneConstant"/> constants for type-safe calls.
    /// </param>
    /// <returns>Display name string.</returns>
    public virtual string GetTimezoneName(string timezoneId)
        => ResolveTimeZone(timezoneId).DisplayName;

    // ──────────────────────────────────────────────────────────────────────────
    //  Protected overridable — for subclass injection / NodaTime swap-out
    // ──────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Resolves a timezone identifier to a <see cref="TimeZoneInfo"/> instance.
    /// <para>
    /// Override in a derived class to replace BCL timezone resolution with a
    /// custom provider (e.g. NodaTime, fixed-offset zones, mocked zones for
    /// unit tests).
    /// </para>
    /// </summary>
    /// <param name="timezoneId">IANA or Windows timezone identifier.</param>
    /// <returns>A <see cref="TimeZoneInfo"/> instance for the given id.</returns>
    /// <exception cref="TimeZoneNotFoundException">
    /// Thrown when the id cannot be resolved on this platform.
    /// </exception>
    protected virtual TimeZoneInfo ResolveTimeZone(string timezoneId)
    {
        // .NET 6+ on Windows: tries IANA first, falls back to Windows id.
        if (TimeZoneInfo.TryFindSystemTimeZoneById(timezoneId, out TimeZoneInfo? tz))
            return tz;

        throw new TimeZoneNotFoundException(
            $"Timezone '{timezoneId}' was not found. " +
            $"Use a constant from {nameof(TimezoneConstant)} or supply a valid IANA / Windows id.");
    }
}
