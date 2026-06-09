namespace ProDateTime.Interfaces;

/// <summary>
/// Defines a contract for converting a <see cref="DateTime"/> value
/// to a different timezone.
/// Implement this interface to supply a custom conversion strategy
/// (e.g. NodaTime, custom DST rules, or mocked time in tests).
/// </summary>
public interface IDateTimeConverter
{
    /// <summary>
    /// Converts the given <paramref name="dateTime"/> to the specified
    /// <paramref name="targetTimezoneId"/>.
    /// </summary>
    /// <param name="dateTime">The source <see cref="DateTime"/> to convert.</param>
    /// <param name="targetTimezoneId">
    /// An IANA or Windows timezone identifier.
    /// Use constants from <see cref="ProDateTime.Constants.TimezoneConstant"/> for clarity.
    /// </param>
    /// <returns>
    /// A <see cref="DateTime"/> whose <see cref="DateTime.Kind"/> is
    /// <see cref="DateTimeKind.Unspecified"/> and represents the local
    /// wall-clock time in the target timezone.
    /// </returns>
    /// <exception cref="TimeZoneNotFoundException">
    /// Thrown when <paramref name="targetTimezoneId"/> is not recognised by the platform.
    /// </exception>
    DateTime Convert(DateTime dateTime, string targetTimezoneId);

    /// <summary>
    /// Converts the given <paramref name="dateTimeOffset"/> to the specified
    /// <paramref name="targetTimezoneId"/>.
    /// </summary>
    /// <param name="dateTimeOffset">The source <see cref="DateTimeOffset"/> to convert.</param>
    /// <param name="targetTimezoneId">
    /// An IANA or Windows timezone identifier.
    /// Use constants from <see cref="ProDateTime.Constants.TimezoneConstant"/> for clarity.
    /// </param>
    /// <returns>
    /// A <see cref="DateTimeOffset"/> representing the wall-clock time
    /// in the target timezone, with the correct UTC offset applied.
    /// </returns>
    /// <exception cref="TimeZoneNotFoundException">
    /// Thrown when <paramref name="targetTimezoneId"/> is not recognised by the platform.
    /// </exception>
    DateTimeOffset Convert(DateTimeOffset dateTimeOffset, string targetTimezoneId);
}
